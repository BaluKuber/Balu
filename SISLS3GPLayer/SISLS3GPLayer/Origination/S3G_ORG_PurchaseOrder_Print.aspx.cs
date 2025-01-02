#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Purchase Order Print
/// Created By			: Chandrasekar K
/// Created Date		: 12-Jan-2014
/// <Program Summary>
#endregion

#region Name Spaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
//using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;
using System.Collections;

#endregion

#region Delivery Instruction / LPO
public partial class Origination_S3G_ORG_PurchaseOrder_Print : ApplyThemeForProject
{
    #region Intialization
    Dictionary<string, string> dictParam = null;
    string strDateFormat;
    int intErrCode;
    int intPO_Hdr_ID;
    int intUserId;
    int intCompanyId;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    string strKey = "Insert";
    
    static string strPageName = "DeliveryInstruction / LPO";
    DataTable dtPO = new DataTable();
    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    Dictionary<string, string> Procparam_PO_PDF= new Dictionary<string, string>();
    UserInfo ObjUserInfo = new UserInfo();
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_PurchaseOrder_Print.aspx?qsMode=C';";
    string strExceptionEmail = "";
    string filePath_zip = "";
    SerializationMode SerMode = SerializationMode.Binary;

    //ReportDocument rpd = new ReportDocument();

    public static Origination_S3G_ORG_PurchaseOrder_Print obj_Page;

    public int ProPageNumRW                                                     // to retain the current page size and number
    {
        get;
        set;
    }
    public int ProPageSizeRW
    {
        get;
        set;
    }

    #endregion

    #region Page Load

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (rpd != null)
        //{
        //    rpd.Close();
        //    rpd.Dispose();
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        ProgramCode = "304";
        obj_Page = this;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

       
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        bModify = ObjUserInfo.ProModifyRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bQuery = ObjUserInfo.ProViewRW;

        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        ProPageNumRW = 1;                                                           // to set the default page number
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        if (txtPageSize.Text != "")
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        else
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));
        PageAssignValue obj = new PageAssignValue(this.AssignValue);
        ucCustomPaging.callback = obj;
        ucCustomPaging.ProPageNumRW = ProPageNumRW;
        ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

        strDateFormat = ObjS3GSession.ProDateFormatRW;
        CalendarExtenderFromDate.Format = strDateFormat;
        CalendarExtenderToDate.Format = strDateFormat;
        txtPO_From_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtPO_From_Date.ClientID + "','" + strDateFormat + "',false,  false);");
        txtPO_To_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtPO_To_Date.ClientID + "','" + strDateFormat + "',false,  false);");

        if (!IsPostBack)
        {
            RBLPOType.SelectedIndex = 0;
        }

        if (PageMode == PageModes.WorkFlow && !IsPostBack)
        {
            try
            {
                PreparePageForWFLoad();
            }
            catch (Exception ex)
            {

                Utility.FunShowAlertMsg(this, "Invalid data to load, access side menu");
            }
        }
    }

    private void PreparePageForWFLoad()
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();

            DataRow HeaderValues = GetHeaderDetailsFromDOCNo(WFSessionValues.PANUM, ProgramCode);
        }
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;              // To set the page Number
            ProPageSizeRW = intPageSize;            // To set the page size    
            FunPriBindPOGrid();   // Binding the Landing grid
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region Button Events

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FunPriBindPOGrid();
    }

    private void FunPriGeneratePOFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code,int IsPrint)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strPONumber = "";
            string PO_Header_ID = "";
            string LPODate = "";
            string GSTEffectiveDate = "";
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = "";

            FilePath = Server.MapPath(".") + "\\PDF Files\\";

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(CompanyID));
            GSTEffectiveDate = Utility.GetDefaultData("S3G_SYSAD_GSTEFFECTIVEDATE", dictParam).Rows[0]["GST_Effective_From"].ToString();

            foreach (GridViewRow grvRow in gvPO.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    strPONumber = ((Label)grvRow.FindControl("lblPO_Number")).Text;
                    PO_Header_ID = ((Label)grvRow.FindControl("lblPO_dtl_ID")).Text;
                    LPODate = ((Label)grvRow.FindControl("lblPO_Date")).Text;

                    String strHTML = String.Empty;
                    // if (Utility.StringToDate(LPODate) >= Utility.StringToDate(GSTEffectiveDate))
                    if (RBLPOType.SelectedValue == "0")
                    {
                        Template_Type_Code = 32;
                    }
                    else
                    {
                        Template_Type_Code = 79;
                    }
                    //else
                    //Template_Type_Code = 10;

                    strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, Template_Type_Code, PO_Header_ID);
                    if (strHTML == "")
                    {
                        Utility.FunShowAlertMsg(this, "Template Master not defined");
                        return;
                    }

                    string FileName = PDFPageSetup.FunPubGetFileName(strPONumber +"_"+ intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                    string DownFile = string.Empty;
                    if (ddlPrintType.SelectedValue == "P")
                    {
                        DownFile = FilePath + FileName + ".pdf";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                        Procparam_PO_PDF.Add(strPONumber, DownFile);
                    }
                    else if (ddlPrintType.SelectedValue == "W")
                    {
                        DownFile = FilePath + FileName + ".doc";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc";
                    }

                    dictParam = new Dictionary<string, string>();
                    dictParam.Add("@Company_ID", intCompanyId.ToString());
                    dictParam.Add("@PO_Number", strPONumber);
                    DataSet dsHeader = new DataSet();
                    if (RBLPOType.SelectedValue == "0")
                    {
                        dsHeader = Utility.GetDataset("S3G_ORG_PurchaseOrder_Print", dictParam);
                    }
                    else
                    {
                        dsHeader = Utility.GetDataset("S3G_ORG_PurchaseOrder_Print_MPO", dictParam);
                    }

                    if (dsHeader.Tables[1].Rows.Count == 0)
                        if (strHTML.Contains("~PO_Header_Table~"))
                            strHTML = FunPubDeleteTable("~PO_Header_Table~", strHTML);

                    if (dsHeader.Tables[2].Rows.Count == 0)
                        if (strHTML.Contains("~PO_Annex1_Table~"))
                            strHTML = FunPubDeleteTable("~PO_Annex1_Table~", strHTML);

                    if (dsHeader.Tables[3].Rows.Count == 0)
                        if (strHTML.Contains("~PO_Annex2_Table~"))
                            strHTML = FunPubDeleteTable("~PO_Annex2_Table~", strHTML);

                    if (dsHeader.Tables[1].Rows.Count > 0)
                        if (strHTML.Contains("~PO_Header_Table~"))
                            strHTML = PDFPageSetup.FunPubBindTable("~PO_Header_Table~", strHTML, dsHeader.Tables[1]);

                    if (dsHeader.Tables[2].Rows.Count > 0)
                        if (strHTML.Contains("~PO_Annex1_Table~"))
                            strHTML = PDFPageSetup.FunPubBindTable("~PO_Annex1_Table~", strHTML, dsHeader.Tables[2]);

                    if (dsHeader.Tables[3].Rows.Count > 0)
                        if (strHTML.Contains("~PO_Annex2_Table~"))
                            strHTML = PDFPageSetup.FunPubBindTable("~PO_Annex2_Table~", strHTML, dsHeader.Tables[3]);

                    if (dsHeader.Tables[0].Rows.Count > 0)
                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    List<string> listImagePath = new List<string>();
                    if (Utility.StringToDate(LPODate) >= Utility.StringToDate("06/06/2023"))
                    {
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    }
                    else
                    {
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
                    }
                    //listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                    strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                    FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);
                    filepaths.Add(DownFile);
                    
                }
            }
            if (IsPrint==1)
            {
                if (filepaths.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                    return;
                }
                else
                {
                    FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);
                    FunPriDownloadFile(OutputFile);
                }
            }
           
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    public static string FunPubDeleteTable(string strtblName, string strHTML)
    {
        try
        {
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);

            strHTML = strHTML.Replace(strTable, "");
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType)
    {
        string strhtmlFile = FilePath + "\\" + FileName + ".html";
        string strwordFile = FilePath + "\\" + FileName + ".doc";
        string strpdfFile = FilePath + "\\" + FileName + ".pdf";
        object file = strhtmlFile;
        object oMissing = System.Reflection.Missing.Value;
        object readOnly = false;
        object oFalse = false;
        object fileFormat = null;
        Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
        Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            if (File.Exists(strwordFile) == true)
            {
                File.Delete(strwordFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText("\t");
            oDoc.ActiveWindow.Selection.TypeText("           ");
            string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            if (DocumentType == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
            }

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);
        }
    }

    private void FunPriGenerateFiles(List<string> filepaths, string OutputFile, string DocumentType)
    {
        try
        {
            object fileFormat = null;
            object file = null;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string[] filesToMerge = filepaths.ToArray();

            if (DocumentType == "P")
            {
                PDFPageSetup.MergePDFs(filesToMerge, OutputFile);

                for (int i = 0; i < filesToMerge.Length; i++)
                {
                    if (File.Exists(filesToMerge[i]) == true)
                    {
                        File.Delete(filesToMerge[i]);
                    }
                }
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = OutputFile;
                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;
                int temp = 0;
                foreach (string file1 in filesToMerge)
                {
                    temp++;
                    Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(file1);
                    PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
                    CurrentDocument.Range().Copy();
                    selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
                    if (temp != filesToMerge.Length)
                        selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);
                }
                wordDocument.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
                wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

                for (int i = 0; i < filesToMerge.Length; i++)
                {
                    if (File.Exists(filesToMerge[i]) == true)
                    {
                        File.Delete(filesToMerge[i]);
                    }
                }
            }

        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print");
        }
    }

    private void FunPriDownloadFile(string OutputFile)
    {
        if (ddlPrintType.SelectedValue == "P")
        {
            Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
            Response.ContentType = "application/pdf";
            Response.WriteFile(OutputFile);
        }
        else if (ddlPrintType.SelectedValue == "W")
        {
            Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.doc");
            Response.ContentType = "application/vnd.ms-word";
            Response.WriteFile(OutputFile);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriGeneratePOFiles(intCompanyId, 3, 0, 10,1);

            //string strPONumber = "";
            //foreach (GridViewRow grvRow in gvPO.Rows)
            //{
            //    if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
            //    {
            //        strPONumber += ((Label)grvRow.FindControl("lblPO_Number")).Text + ",";
            //    }
            //}

            //if (strPONumber != "")
            //{
            //    Guid objGuid;
            //    objGuid = Guid.NewGuid();
            //    DataSet dset = new DataSet();
            //    DataSet dsetChk = new DataSet();

            //    dictParam = new Dictionary<string, string>();
            //    dictParam.Add("@Company_ID", intCompanyId.ToString());
            //    dictParam.Add("@PO_Number", strPONumber);

            //    dset = Utility.GetDataset("S3G_ORG_PurchaseOrder_Print", dictParam);

            //    rpd.Load(Server.MapPath("PurchaseOrderPrint.rpt"));
            //    rpd.SetDataSource(dset.Tables[0]);
            //    rpd.Subreports[0].SetDataSource(dset.Tables[1]);
            //    rpd.Subreports[1].SetDataSource(dset.Tables[2]);
            //    rpd.Subreports[2].SetDataSource(dset.Tables[3]);

            //    string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + txtPO_From_Date.Text.ToString() + objGuid.ToString() + ".pdf";

            //    string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

            //    if (!(System.IO.Directory.Exists(strFolder)))
            //    {
            //        DirectoryInfo di = Directory.CreateDirectory(strFolder);

            //    }
            //    string strScipt = "";
            //    rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

            //    if (rpd != null)
            //    {
            //        rpd.Close();
            //        rpd.Dispose();
            //    }

            //    strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            //}
            //else
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Select atleast one PO");
            //}
        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print PO");

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
        finally
        {
            //if (rpd != null)
            //{
            //    rpd.Close();
            //    rpd.Dispose();
            //}
        }
    }
    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancelPO_Click(object sender, EventArgs e)
    {
        LoanAdminMgtServicesClient objLoanAdmin_MasterClient = new LoanAdminMgtServicesClient(); ;
        try
        {
            int ErrorCode = 0;
            int intOption = 1;
            string strPONumber = "";
            string strPONumberOut = "";
            string strReason_For_Cancellation = txtCancReason.Text;
            string strRedi = "";
            foreach (GridViewRow grvRow in gvPO.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    strPONumber += ((Label)grvRow.FindControl("lblPO_Number")).Text + ",";
                }
            }

            if (strPONumber != "")
            {
                int intResult = objLoanAdmin_MasterClient.FunPubCancelMultiplePO(out ErrorCode, out strPONumberOut, strPONumber, intOption, strReason_For_Cancellation);

                if (intResult == 0)
                {

                    FunEmailGeneration(1);

                    if (strExceptionEmail != "")
                    {
                        strRedi = "../Origination/S3G_ORG_PurchaseOrder_Print.aspx";
                        Utility.FunShowAlertMsg(this.Page, "Purchase Order Cancelled successfully and unable to sent Email to " + strExceptionEmail, strRedi);
                        return;
                    }
                    else
                    {
                        strRedi = "../Origination/S3G_ORG_PurchaseOrder_Print.aspx";
                        Utility.FunShowAlertMsg(this.Page, "Purchase Order Cancelled and Email sent successfully", strRedi);
                        return;
                    }
                }

                if (intResult == 10)
                {
                    Utility.FunShowAlertMsg(this.Page, "Proforma Invoice Processed, Unable to Cancel PO - " + strPONumberOut);
                }
                if (intResult == 11)
                {
                    Utility.FunShowAlertMsg(this.Page, "Vendor Invoice Processed, Unable to Cancel PO - " + strPONumberOut);
                }
                if (intResult == 12)
                {
                    Utility.FunShowAlertMsg(this.Page, "Already PO has been Cancelled.");
                }
                if (intResult == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, "Purchase Order Cancelation Failed");
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one PO");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
        finally
        {
            objLoanAdmin_MasterClient.Close();
        }
    }

    protected void lnkUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkView = (sender as LinkButton);
            GridViewRow row = (lnkView.NamingContainer as GridViewRow);
            string PO_Header_ID = (row.FindControl("lblPO_dtl_ID") as Label).Text;
            string PO_Type = (row.FindControl("lblPOType") as Label).Text;
            Session["VendorGroup"] = (row.FindControl("lblPO_Vendor_Group") as Label).Text;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(PO_Header_ID, false, 0);
            string strPage = "S3G_ORG_PurchaseOrder_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M&Popup=yes&POType="+ PO_Type;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + strPage + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
            return;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {

        }
    }

    protected void btnExport1_Click(object sender, EventArgs e)
    {
        ExportToExcel(1);
    }

    protected void btnExport2_Click(object sender, EventArgs e)
    {
        ExportToExcel(2);
    }

    protected void btnExport3_Click(object sender, EventArgs e)
    {
        ExportToExcel(3);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtPO_From_Date.Text = txtPO_To_Date.Text = String.Empty;
        ddlCustomerName.Clear();
        ddlVendorName.Clear();
        ddlLoadSequenceNo.Clear();
        ddlPONo.Clear();
        pnlPO.Visible = false;
        gvPO.DataSource = null;
        gvPO.DataBind();
        btnPrint.Enabled = btnExport1.Enabled = btnExport2.Enabled = btnExport3.Enabled = btnCancelPO.Enabled = false;

    }

    public void ExportToExcel(int intOption)
    {
        try
        {
            Guid objGuid;
            objGuid = Guid.NewGuid();
            DataTable dtPO = new DataTable();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            if (!String.IsNullOrEmpty(txtPO_From_Date.Text))
                dictParam.Add("@PO_From_Date", Utility.StringToDate(txtPO_From_Date.Text).ToString());
            if (!String.IsNullOrEmpty(txtPO_To_Date.Text))
                dictParam.Add("@PO_To_Date", Utility.StringToDate(txtPO_To_Date.Text).ToString());
            if (ddlCustomerName.SelectedValue != "0")
                dictParam.Add("@Customer_Id", ddlCustomerName.SelectedValue);
            if (ddlVendorName.SelectedValue != "0")
                dictParam.Add("@Entity_ID", ddlVendorName.SelectedValue);
            if (ddlLoadSequenceNo.SelectedValue != "0")
                dictParam.Add("@LoadSequenceNo", ddlLoadSequenceNo.SelectedValue);
            if (ddlPONo.SelectedValue != "0")
                dictParam.Add("@PO_Number", ddlPONo.SelectedText);
            dictParam.Add("@Option", intOption.ToString());
            dtPO = Utility.GetDefaultData("S3G_ORG_PurchaseOrder_Export", dictParam);

            string filename = "PurchaseOrder.xls";

            if (intOption == 2)
                filename = "ProformaInvoice.xls";
            else if (intOption == 3)
                filename = "VendorInvoice.xls";

            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = dtPO;
            dgGrid.DataBind();
            dgGrid.RenderControl(hw);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            this.EnableViewState = false;
            Response.Write(tw.ToString());
            Response.End();

        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Export PO");

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    protected void gvPO_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvPO.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindPOGrid()
    {
        if (txtPO_From_Date.Text == "" && txtPO_To_Date.Text == "" && (ddlCustomerName.SelectedValue == "0" 
            && ddlCustomerName.SelectedText == String.Empty) && (ddlVendorName.SelectedValue == "0" && ddlVendorName.SelectedText == String.Empty)
            && (ddlLoadSequenceNo.SelectedValue == "0" && ddlLoadSequenceNo.SelectedText == String.Empty)
            &&(ddlPONo.SelectedValue == "0" || ddlPONo.SelectedText == String.Empty )
            )
        {
            Utility.FunShowAlertMsg(this.Page, "Select any one of input detail");
            return;
        }

        dictParam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtPO_From_Date.Text))
            dictParam.Add("@PO_From_Date", Utility.StringToDate(txtPO_From_Date.Text).ToString());

        if (!String.IsNullOrEmpty(txtPO_To_Date.Text))
            dictParam.Add("@PO_To_Date", Utility.StringToDate(txtPO_To_Date.Text).ToString());

        if (ddlCustomerName.SelectedValue != "0" && ddlCustomerName.SelectedText != String.Empty)
            dictParam.Add("@Customer_Id", ddlCustomerName.SelectedValue);
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        else if (ddlCustomerName.SelectedValue == "0" && ddlCustomerName.SelectedText != String.Empty)
            dictParam.Add("@Customer_Id", ddlCustomerName.SelectedValue);
        else if (ddlCustomerName.SelectedValue != "0" && ddlCustomerName.SelectedText == String.Empty)
            dictParam.Add("@Customer_Id", null);
        //added by vinodha m for the issue(invalid input,grid showing records)ends

        if (ddlVendorName.SelectedValue != "0" && ddlVendorName.SelectedText != String.Empty)
            dictParam.Add("@Entity_ID", ddlVendorName.SelectedValue);
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        else if (ddlVendorName.SelectedValue == "0" && ddlVendorName.SelectedText != String.Empty)
            dictParam.Add("@Entity_ID", ddlCustomerName.SelectedValue);
        else if (ddlVendorName.SelectedValue != "0" && ddlVendorName.SelectedText == String.Empty)
            dictParam.Add("@Entity_ID", null);
        //added by vinodha m for the issue(invalid input,grid showing records)ends        

        if (ddlLoadSequenceNo.SelectedValue != "0" && ddlLoadSequenceNo.SelectedText != String.Empty)
            dictParam.Add("@LoadSequenceNo", ddlLoadSequenceNo.SelectedValue);
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        else if (ddlLoadSequenceNo.SelectedValue == "0" && ddlLoadSequenceNo.SelectedText != String.Empty)
            dictParam.Add("@LoadSequenceNo", ddlLoadSequenceNo.SelectedValue);
        else if (ddlLoadSequenceNo.SelectedValue != "0" && ddlLoadSequenceNo.SelectedText == String.Empty)
            dictParam.Add("@LoadSequenceNo", null);
        //added by vinodha m for the issue(invalid input,grid showing records)ends

        //added by vinodha m for the issue(invalid input,grid showing records)ends        

        if (ddlPONo.SelectedValue != "0" && ddlPONo.SelectedText != String.Empty)
            dictParam.Add("@PO_Number", ddlPONo.SelectedText);
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        else if (ddlPONo.SelectedValue == "0" && ddlPONo.SelectedText != String.Empty)
            dictParam.Add("@PO_Number", ddlPONo.SelectedText);
        else if (ddlPONo.SelectedValue != "0" && ddlPONo.SelectedText == String.Empty)
            dictParam.Add("@PO_Number", null);
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        dictParam.Add("@PO_Type", RBLPOType.SelectedValue);
        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyId;
        gvPO.BindGridView("S3G_Org_Get_PO", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucCustomPaging.Visible = true;
        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucCustomPaging.setPageSize(ProPageSizeRW);
        pnlPO.Visible = true;

        if (bIsNewRow == true)
        {
            gvPO.Rows[0].Visible = false;
            btnExport1.Enabled = btnExport2.Enabled = btnExport3.Enabled = btnCancelPO.Enabled = btnPrint.Enabled = btnEmail.Enabled = false;
        }
        else
        {
            btnExport1.Enabled = btnExport2.Enabled = btnExport3.Enabled = btnCancelPO.Enabled = btnPrint.Enabled = btnEmail.Enabled = true;
        }

        if (!bModify)
            gvPO.Columns[8].Visible = false;

        if (!bDelete)
            btnCancelPO.Enabled = false;

        if(RBLPOType.SelectedValue=="1")
        {
            btnCancelPO.Enabled = false;
        }
        else
        {
            btnCancelPO.Enabled = true;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        obj_Page.ddlVendorName.SelectedValue = String.Empty;
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));
        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetPONo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        obj_Page.ddlPONo.SelectedValue = String.Empty;
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        Procparam.Clear();
        Procparam.Add("@PO_Type", obj_Page.RBLPOType.SelectedValue);
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@SearchText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_LoadPurchaseOrderNo_AGT", Procparam));
        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetCustomerNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        obj_Page.ddlCustomerName.SelectedValue = String.Empty;
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", Procparam), false);
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLSQNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataSet Ds = new DataSet();
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        obj_Page.ddlLoadSequenceNo.SelectedValue = String.Empty;
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyId));
        Procparam.Add("@SearchText", prefixText);
        Procparam.Add("@ProgramCode", "PURORD");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_GetPO_LSQNo_AGT", Procparam));
        return suggetions.ToArray();
    }

    private void FunEmailGeneration(int IsCancelled)
    {
        StringBuilder strPODetails = new StringBuilder();
        int intRowCt = 0;
        string strHTML_Email = "";
        string strHTML_Email_Base = "";
        string strPONos = "";
        StringBuilder strBody = new StringBuilder();
        ArrayList arrVendor = new ArrayList();
        DataSet dsEmail = new DataSet();
        try
        {

            FunPriGeneratePOFiles(intCompanyId, 3, 0, 10, 0);

            strPODetails.Append("<Root>");

            foreach (GridViewRow grvRow in gvPO.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    intRowCt = intRowCt + 1;
                    strPODetails.Append(
                        " <Details PO_Number = '" + ((Label)grvRow.FindControl("lblPO_Number")).Text + "' />");
                    Label lblVendor =(Label) grvRow.FindControl("lblEntity_ID");
                    if (!arrVendor.Contains(lblVendor.Text))
                    {
                        arrVendor.Add(lblVendor.Text);
                    }
                }
            }
            strPODetails.Append("</Root>");

            if (intRowCt == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one PO");
                return;
            }

            if (IsCancelled == 0)
            {
                strHTML_Email = PDFPageSetup.FunPubGetTemplateContent(1, 3, 0, 66, "0");
            }
            else
            {
                //if()
                strHTML_Email = PDFPageSetup.FunPubGetTemplateContent(1, 3, 0, 67, "0");

            }
            if (strHTML_Email == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined for E-Mail");
                return;
            }

            List<string> listImageName = new List<string>();
            listImageName.Add("~CompanyLogoEmail~");
            List<string> listImagePath = new List<string>();
            listImagePath.Add("cid:CompanyLogoEmail");
            strHTML_Email = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML_Email);
            strHTML_Email_Base = strHTML_Email;

            ArrayList arrMailAttachement = new ArrayList();
            string strPOFile = "";
            string filePath_PO = Server.MapPath(".") + "\\PDF Files\\PO" + intUserId;
            string strLesseName = "";
            //Sending Mails for each Vendor Id Start
            for (int i = 0; i < arrVendor.Count; i++)
            {
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@XMLPODetail", strPODetails.ToString());
                dictParam.Add("@Vendor_ID", arrVendor[i].ToString());

                if (RBLPOType.SelectedValue == "0")
                {
                    dsEmail = Utility.GetDataset("S3G_ORG_Get_PO_Email_Det", dictParam);
                }
                else
                {
                    dsEmail = Utility.GetDataset("S3G_ORG_Get_PO_Email_Det_MPO", dictParam);
                }
                
                arrMailAttachement.Clear();
                strBody.Clear();
                strPONos = "";
                strHTML_Email = strHTML_Email_Base;

                Dictionary<string, string> dictMail = new Dictionary<string, string>();
                if (dictMail.Count == 0)
                {
                    strLesseName = dsEmail.Tables[0].Rows[0]["Customer_Name"].ToString();
                    dictMail.Add("FromMail", dsEmail.Tables[0].Rows[0]["From_Email"].ToString());
                    dictMail.Add("ToCC", dsEmail.Tables[0].Rows[0]["CC"].ToString());
                    dictMail.Add("DisplayName", dsEmail.Tables[0].Rows[0]["Display_Name"].ToString());
                    dictMail.Add("ToMail", dsEmail.Tables[0].Rows[0]["Email_To"].ToString());
                    if (IsCancelled == 0)
                    {
                        dictMail.Add("Subject", dsEmail.Tables[0].Rows[0]["Subject_Name"].ToString());
                    }
                    else
                    {
                        dictMail.Add("Subject", "Cancelled - " + dsEmail.Tables[0].Rows[0]["Subject_Name"].ToString());
                    }
                    dictMail.Add("imgPath", Server.MapPath("../Images/TemplateImages/" + CompanyId + "/Company_Logo_Email_Img.jpg"));
                    filePath_zip = Server.MapPath(".") + "\\PDF Files\\PO_OPC_" + dsEmail.Tables[0].Rows[0]["Short_Name"].ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".zip";
                }

                if (Directory.Exists(filePath_PO))
                {
                    string[] str1 = Directory.GetFiles(filePath_PO);
                    foreach (string fileName in str1)
                    {
                        File.Delete(fileName);
                    }

                    Directory.Delete(filePath_PO);
                }

                if (!(System.IO.Directory.Exists(filePath_PO)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(filePath_PO);

                }
                foreach (DataRow rowPO in dsEmail.Tables[1].Rows)
                {

                    if (Procparam_PO_PDF.TryGetValue(rowPO["PO_Number"].ToString(), out strPOFile))
                    {
                        if (!(File.Exists(filePath_PO + "\\" + Path.GetFileName(strPOFile))))
                        {
                            File.Copy(strPOFile, filePath_PO + "\\" + Path.GetFileName(strPOFile));
                        }
                    }

                    if (strPONos == "")
                    {
                        strPONos = rowPO["PO_Number"].ToString();
                    }
                    else
                    {
                        strPONos = strPONos + "," + rowPO["PO_Number"].ToString();

                        //if (!(strPONos.Contains(rowPO["PO_Number"].ToString())))
                        //{
                        //    strPONos = strPONos + "," + rowPO["PO_Number"].ToString();

                        //}
                    }
                   
                }

                //before creation of compressed folder,deleting it if exists
                //if (File.Exists(filePath_zip))
                //{
                //    File.Delete(filePath_zip);
                //}
                //if (IsCancelled == 0)
                //{
                if (!File.Exists(filePath_zip))
                {
                    //creating a zip file from one folder to another folder
                    ZipFile.CreateFromDirectory(filePath_PO, filePath_zip);
                    arrMailAttachement.Add(filePath_zip);
                    //Delete The pdf file which is created

                    string[] str = Directory.GetFiles(filePath_PO);
                    foreach (string fileName in str)
                    {
                        File.Delete(fileName);
                    }

                    if (Directory.Exists(filePath_PO))
                    {
                        Directory.Delete(filePath_PO);
                    }
                }

                if (IsCancelled == 0)
                {
                    //opc083 start
                    if (dsEmail.Tables[0].Rows.Count > 0)
                        strHTML_Email = PDFPageSetup.FunPubBindCommonVariables(strHTML_Email, dsEmail.Tables[0]);
                    //opc083 end
                    if (dsEmail.Tables[1].Rows.Count > 0)
                        if (dsEmail.Tables[1].Columns.Contains("EndCust_Name"))
                        {
                            strHTML_Email = FunPubDeleteTable("~PONos_Table~", strHTML_Email);
                            strHTML_Email = PDFPageSetup.FunPubBindTable("~PONos_Table_1~", strHTML_Email, dsEmail.Tables[1]);
                        }
                    else
                        {
                            strHTML_Email = FunPubDeleteTable("~PONos_Table_1~", strHTML_Email);
                            strHTML_Email = PDFPageSetup.FunPubBindTable("~PONos_Table~", strHTML_Email, dsEmail.Tables[1]);
                        }
                    strHTML_Email = strHTML_Email.Replace("~User_Name~", ObjUserInfo.ProUserNameRW);
                }
                else
                {
                    strHTML_Email = strHTML_Email.Replace("~PO_Nos~", strPONos);
                    strHTML_Email = strHTML_Email.Replace("~Lessee_Name~", strLesseName);
                    strHTML_Email = strHTML_Email.Replace("~User_Name~", ObjUserInfo.ProUserNameRW);
                }
                strBody = strBody.Append(strHTML_Email);
                string strErrMsg = Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
                if (strErrMsg != "")
                {
                    if (strExceptionEmail == "")
                    {
                        strExceptionEmail = dsEmail.Tables[0].Rows[0]["Entity_Name"].ToString();
                    }
                    else
                    {
                        strExceptionEmail = strExceptionEmail + "," + dsEmail.Tables[0].Rows[0]["Entity_Name"].ToString();
                    }
                }
            }
            //Sending Mails for each Vendor Id End
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        
    }
   // opc038 start
    protected void btnEmail_Click(object sender, EventArgs e)
    {
        try
        {
            FunEmailGeneration(0);
            if (strExceptionEmail!="")
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to Send Mail to Vendor : "+ strExceptionEmail);
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Mail Sent Successfully");
            }
         }
       catch(Exception objException)
        {
            //ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Send Mail. ");
        }
       
    }
    //opc038 end
    /*
   private StringBuilder GetHTMLTextEmail()
   {
       StringBuilder strMailBodey = new StringBuilder();
       strMailBodey.Append(

           "<font size=\"1\"  color=\"black\" face=\"Times New Roman\">" +

          " <table width=\"100%\">" +

 "<tr >" +
           "<td  align=\"Left\" >" +
               "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Test Mail from S3G Support " + "</font> " +
           "</td>" +
      " </tr>" +
   "</table>" + "</font>");

       return strMailBodey;

   }
   */

}

#endregion

