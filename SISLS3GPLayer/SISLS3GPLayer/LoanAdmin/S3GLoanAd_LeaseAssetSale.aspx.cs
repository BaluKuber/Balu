/// <Program Summary>
/// Module Name         :   Loan Admin
/// Screen Name         :   Lease Asset Sale
/// Created By          :   Rajendran
/// Created Date        :   05-Oct-2010
/// Last Updated By	    : Thalaiselvam N
/// Last Updated Date   : 23-09-2010
/// Reason              : User Mgmt Conversion
/// 
/// <Program Summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;

//This NameSpace for PDF Format
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.UI.HtmlControls;
using S3GBusEntity.LoanAdmin;
using System.Data;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Xml;
using System.Web.Security;

//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using System.Configuration;
using System.ServiceModel;
using System.IO.Compression;
using QRCoder;
using System.Drawing;
/// <summary>
/// Author : Rajendran
/// </summary>
public partial class LoanAdmin_S3GLoanAd_LeaseAssetSale : ApplyThemeForProject
{
    int intCompanyId = 0;
    int intUserId = 0, Flag = 0;
    string SheetName = String.Empty;
    string strKey = "Insert";
    int intRegAddress = 0;
    #region " Declaration"
    UserInfo ObjUserInfo = new UserInfo();
    public string strDateFormat;
    S3GSession ObjS3GSession = new S3GSession();
    AssetMgtServicesReference.AssetMgtServicesClient ObjLeaseAssetSaleClient;
    AssetMgtServices.S3G_LOANAD_LeaseAssetSaleDataTable ObjS3G_LOANAD_LeaseAssetSaleDataTable = null;
    AssetMgtServices.S3G_LAD_LASDTLDataTable objLASDT = null;
    SerializationMode SerMode = SerializationMode.Binary;
    public static LoanAdmin_S3GLoanAd_LeaseAssetSale obj_Page;

    Dictionary<string, string> dictParam, dictLASstatus, dictEntityType = null;
    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=LAS";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAd_LeaseAssetSale.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=LAS';";
    static string strPageName = "Lease Asset Sale";
    Int64 intLASID = 0;
    bool blnGo = false;
    string strMode = string.Empty;
    string strAlert = "alert('__ALERT__');";
    static bool blnSlctAll;
    //ReportDocument rpd = new ReportDocument();

    #region "Page Variables"

    #region "Transfer Paging"

    PagingValues ObjPaging = new PagingValues();

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public int ProPageNumRW
    {
        get;
        set;
    }

    public int ProPageSizeRW
    {
        get;
        set;
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGrid();
    }

    #endregion

    #region "Map Invoice Disposed Paging"

    PagingValues ObjDspPaging = new PagingValues();

    public delegate void PageAssignValueDsp(int ProPageNumRWDsp, int intPageSizeDsp);

    public int ProPageNumRWDsp
    {
        get;
        set;
    }

    public int ProPageSizeRWDsp
    {
        get;
        set;
    }

    protected void AssignValueDsp(int intPageNum, int intPageSize)
    {
        ProPageNumRWDsp = intPageNum;
        ProPageSizeRWDsp = intPageSize;
        FunPriGetTotal();
        FunPriBindDspDtl();
    }

    #endregion

    #region "Disposed Paging"

    PagingValues ObjSalePaging = new PagingValues();

    public delegate void PageAssignValueSale(int ProPageNumRWSale, int intPageSizeSale);

    public int ProPageNumRWSale
    {
        get;
        set;
    }

    public int ProPageSizeRWSale
    {
        get;
        set;
    }

    protected void AssignValueSale(int intPageNum, int intPageSize)
    {
        ProPageNumRWSale = intPageNum;
        ProPageSizeRWSale = intPageSize;
        FunPriBindSaleDtl();
    }

    #endregion

    #endregion

    #endregion

    #region "Events"

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (rpd != null)
        //{
        //    rpd.Close();
        //    rpd.Dispose();
        //}
    }

    #region "Page Load"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            flUpload.Attributes.Add("onchange", "fnAssignPath('" + flUpload.ClientID + "','" + hdnSelectedPath.ClientID + "'); fnLoadPath('" + btnBrowse.ClientID + "');");
            btnDlg.OnClientClick = "fnLoadPath('" + flUpload.ClientID + "');";

            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
            txtUserName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtUserName.ToolTip = txtUserName.Text;

            #region Paging Config

            ProPageNumRW = 1;

            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            TextBox txtGotoPage = (TextBox)ucCustomPaging.FindControl("txtGotoPage");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            if (txtGotoPage.Text != "")
                ProPageNumRW = Convert.ToInt32(txtGotoPage.Text);

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

            #region "Map Invoice Disposed Paging"

            ProPageNumRWDsp = 1;

            TextBox txtDspPageSize = (TextBox)ucDisposedPaging.FindControl("txtPageSize");
            TextBox txtDspGotoPage = (TextBox)ucDisposedPaging.FindControl("txtGotoPage");
            if (txtDspPageSize.Text != "")
                ProPageSizeRWDsp = Convert.ToInt32(txtDspPageSize.Text);
            else
                ProPageSizeRWDsp = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            if (txtDspGotoPage.Text != "")
                ProPageNumRWDsp = Convert.ToInt32(txtDspGotoPage.Text);

            PageAssignValueDsp objDsp = new PageAssignValueDsp(this.AssignValueDsp);
            ucDisposedPaging.callback = objDsp;
            ucDisposedPaging.ProPageNumRW = ProPageNumRWDsp;
            ucDisposedPaging.ProPageSizeRW = ProPageSizeRWDsp;

            #endregion

            #region "Disposed Paging"

            ProPageNumRWSale = 1;

            TextBox txtSalePageSize = (TextBox)ucPageDispose.FindControl("txtPageSize");
            TextBox txtSaleGotoPage = (TextBox)ucPageDispose.FindControl("txtGotoPage");
            if (txtSalePageSize.Text != "")
                ProPageSizeRWSale = Convert.ToInt32(txtSalePageSize.Text);
            else
                ProPageSizeRWSale = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            if (txtSaleGotoPage.Text != "")
                ProPageNumRWSale = Convert.ToInt32(txtSaleGotoPage.Text);

            PageAssignValueSale objSale = new PageAssignValueSale(this.AssignValueSale);
            ucPageDispose.callback = objSale;
            ucPageDispose.ProPageNumRW = ProPageNumRWSale;
            ucPageDispose.ProPageSizeRW = ProPageSizeRWSale;

            #endregion

            #endregion

            txtLASDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtLASDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtDueDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDueDate.ClientID + "','" + strDateFormat + "',null,null);");

            CalendarLASDate.OnClientDateSelectionChanged = "checkDate_NextSystemDate";
            CalendarLASDate.Format = strDateFormat;
            CalExtenderDueDate.Format = strDateFormat; 

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                    intLASID = Convert.ToInt64(fromTicket.Name);
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }
            else
            {
                strMode = Request.QueryString["qsMode"];
            }

            if (!IsPostBack)
            {
                FunPriLoadLov();
                FunPriEnblDsblCtrl();
            }

            FunPriInitiateLOVCode();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "DROPDOWN EVENTS"

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }

    }

    protected void ddlTPAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddllastype.SelectedValue) == 2)
            {
                FunPriGetInvoiceDtl();
                FunPriBindGrid();
            }
            else if (Convert.ToInt32(ddllastype.SelectedValue) == 3)
            {
                FunPriGetInvoiceDtl();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void ddllastype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriEnblDsblPnl();
            FunPriClearGrid();
            FunPriClearCustomerDetails();
            FunPriClearUploadDtls();
            pnlLesseeInfo.Visible = (Convert.ToInt32(ddllastype.SelectedValue) == 3) ? true : false;
            pnlLeaseGrid.Visible = ucPageDispose.Visible = ucCustomPaging.Visible = btnExportDspDtl.Enabled = false;
            FunPriClearTransferDtls();
            lblLesseeName.Visible = txtLessee.Visible = (Convert.ToInt32(ddllastype.SelectedValue) == 2) ? true : false;
            txtLessee.ReadOnly = (Convert.ToInt32(ddllastype.SelectedValue) == 2) ? false : true;
            FunPriClearMapDtls(2);
            ddlEntitytype.SelectedValue = (Convert.ToInt32(ddllastype.SelectedValue) == 3) ? "1" : "0";
            ((Label)ucCustomerAddress.FindControl("lblCustomerCode")).Visible = false;
            ((TextBox)ucCustomerAddress.FindControl("txtCustomerCode")).Visible = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void ddlEntitytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriInitiateLOVCode();
            FunPriClearCustomerDetails();
            pnlLeaseGrid.Visible = chkDspManual.Checked = chkDspUpload.Checked = false;
            tblUpload.Style["display"] = tblManual.Style["display"] = "none";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #region "Hide on 01Sep2015"

    //protected void ddlLesseeName_Item_Selected(object Sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Convert.ToString(ddlLesseeName.SelectedText) != "" && Convert.ToInt64(ddlLesseeName.SelectedValue) > 0)
    //        {
    //            ddlTrancheName.Clear();
    //            FunPriGetTrasferRSDtl();
    //        }
    //        else
    //            FunPriBindGrid(3, null);
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    //protected void ddlTrancheName_Item_Selected(object Sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Convert.ToString(ddlTrancheName.SelectedText) != "" && Convert.ToInt64(ddlTrancheName.SelectedValue) > 0)
    //        {
    //            FunPriGetTrasferRSDtl();
    //        }
    //        else
    //            FunPriBindGrid(3, null);
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    #endregion

    #endregion

    #region "BUTTON EVENTS"

    protected void btnCreateCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = ucCustomerCodeLov.FindControl("hdnID") as HiddenField;
            if (hdnCustomerId != null)
            {
                ViewState["CustomerID"] = hdnCustomerId.Value;
                FunPriGetCustomerAddress(Convert.ToInt64(hdnCustomerId.Value));
                FunPriBindGrid(2, null);
                FunPriClearUploadDtls();
                FunPriClearMapDtls(2);
                pnlLeaseGrid.Visible = chkDspManual.Checked = chkDspUpload.Checked = false;
                tblUpload.Style["display"] = tblManual.Style["display"] = "none";
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                if (Convert.ToInt32(ddllastype.SelectedValue) == 2) //Transfer
                {
                    if (gvRSDtls == null)
                    {
                        Utility.FunShowAlertMsg(this, "No Rental Schedule's found to Transfer");
                        return;
                    }
                    Int32 intChkCnt = 0;
                    for (Int32 i = 0; i < gvRSDtls.Rows.Count; i++)
                    {
                        CheckBox cbxSelectRS = (CheckBox)gvRSDtls.Rows[i].FindControl("cbxSelectRS");
                        if (cbxSelectRS.Checked == true)
                            intChkCnt++;
                    }

                    if (intChkCnt == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atleast One Rental Schedule to Transfer");
                        return;
                    }

                    if (Convert.ToString(hdnIsChange.Value).Trim() == "1")
                    {
                        Utility.FunShowAlertMsg(this, "One or More Rental Schedules may included/excluded.Kindly click Move before save");
                        return;
                    }
                }
                else if (Convert.ToInt32(ddllastype.SelectedValue) == 3)    //Disposed
                {
                    //if (!FunPriValidateData())
                    //    return;
                    if (grvDisPosedDtl == null || grvDisPosedDtl.Rows.Count == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atleast One Invoice to Dispose");
                        return;
                    }

                    if (grvDisPosedDtl != null && grvDisPosedDtl.Rows.Count > 0)
                    {
                        FunPriClearDict();
                        dictParam.Add("@Option", "21");
                        dictParam.Add("@User_ID", Convert.ToString(intUserId));
                        dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));

                        DataTable dtError = Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);
                        if (Convert.ToInt32(dtError.Rows[0]["Error_Code"]) == 1)
                        {
                            Utility.FunShowAlertMsg(this, "Sale Price should not be 0");
                            return;
                        }
                    }
                }
                FunPriSaveLAS();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("S3GLoanAd_LeaseAssetSale.aspx?qsMode=C");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
            }
            else
            {
                Response.Redirect(strRedirectPage, false);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            lblUploadErrorMsg.InnerHtml = "";
            string strPathName = ViewState["FilePath"].ToString();
            string strFilepath = System.IO.Path.GetFullPath(strPathName);
            if (Path.GetExtension(strFilepath) == ".xls" || Path.GetExtension(strFilepath) == ".xlsx")
            {
                FunPriFileUpload(Convert.ToString(ViewState["FileName"]), Convert.ToString(ViewState["FilePath"]), Path.GetExtension(strFilepath));
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnBrowse_Click(object sender, EventArgs e)
    {
        try
        {
            gvMapInvoiceDtl.DataSource = null;
            gvMapInvoiceDtl.DataBind();
            ucDisposedPaging.Visible = false;
            btnUpload.Enabled = btnMoveUpload.Enabled = btnException.Enabled = btnMove.Enabled = btnSelectAll.Enabled = btnMoveAll.Enabled = btnMapInvUpdate.Enabled = false;
            HttpFileCollection hfc = Request.Files;
            HttpPostedFile hpf = hfc[0];
            string strFilePath = string.Empty;
            if (hpf.ContentLength > 0)
            {
                if (ViewState["Docpath"] == null)
                {
                    Utility.FunShowAlertMsg(this, "Define Document Path Setup");
                    return;
                }
                if (!((Path.GetExtension(hpf.FileName) == ".xls") || (Path.GetExtension(hpf.FileName) == ".xlsx")))
                {
                    Utility.FunShowAlertMsg(this, "Upload Excel File and Extension should be .xls or .xslx");
                    return;
                }
                strFilePath = ViewState["Docpath"].ToString();
                chkSelect.Checked = true;
                chkSelect.ToolTip = flUpload.ToolTip = hdnSelectedPath.Value;

                string[] filename = flUpload.FileName.Split('.');
                string FileNameFormat = filename[0] + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + filename[1];
                ViewState["FileName"] = FileNameFormat;
                if (!Directory.Exists(strFilePath))
                    Directory.CreateDirectory(strFilePath);

                strFilePath = strFilePath + "\\" + FileNameFormat;
                flUpload.SaveAs(strFilePath);
                lblCurrentPath.Text = strFilePath;
                ViewState["FilePath"] = strFilePath;
                hyplnkView.Enabled = btnUpload.Enabled = true;
                lblExcelCurrentPath.Text = Convert.ToString(ViewState["FileName"]);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    protected void btnPrintSale_Click(object sender, EventArgs e)
    {
        try
        {


            String strHTML = String.Empty;

            if (intLASID < 1250)
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, 3, 0, 62, Convert.ToString(intLASID));
            else
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, 3, 0, 57, Convert.ToString(intLASID));


            if (strHTML == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined");
                return;
            }

            string FileName = PDFPageSetup.FunPubGetFileName(txtLASNo.Text + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));


            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string DownFile = FilePath + FileName + ".pdf";
            SaveDocument(strHTML, txtLASNo.Text, FilePath, FileName);
            //if (!File.Exists(DownFile))
            //{
            //    Utility.FunShowAlertMsg(this, "File not exists");
            //    return;
            //}
            //Response.AppendHeader("content-disposition", "attachment; filename=PurchaseOrder.pdf");
            //Response.ContentType = "application/pdf";
            //Response.WriteFile(DownFile);
        }
        catch (Exception ex)
        {
           throw ex;

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


    protected void SaveDocument(string strHTML, string ReferenceNumber, string FilePath, string FileName)
    {
        try
        {
            int nDigi_Flag = 0;
            string strQRCode = string.Empty;
            string InvoiceNoValue = string.Empty;
            List<string> filepaths = new List<string>();

            Dictionary<string, string> dictLAS = new Dictionary<string, string>();
            dictLAS.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictLAS.Add("@Usr_ID", Convert.ToString(intUserId));
            dictLAS.Add("@LAS_ID", Convert.ToString(intLASID));
            dictLAS.Add("@Option", "2");
            //DataSet dsLAS = Utility.GetDataset("S3G_GETPrint_LeaseAssetSale", dictLAS);
            DataSet dsLAS = new DataSet();
            dsLAS = Utility.GetDataset("S3G_GETPrint_LeaseAssetSale", dictLAS);

            //if (dsHeader.Tables[0].Rows.Count != 0)
            //    dsHeader.Tables[0].Rows[0]["Grand_Total"] = Convert.ToDecimal((dsHeader.Tables[1].Compute("sum(Total1)", ""))).ToString(Funsetsuffix());

            //if (strHTML.Contains("~PO_Header_Table~"))
            //    strHTML = PDFPageSetup.FunPubBindTable("~PO_Header_Table~", strHTML, dsHeader.Tables[1]);
            //if (strHTML.Contains("~PO_Annex1_Table~"))
            //    strHTML = PDFPageSetup.FunPubBindTable("~PO_Annex1_Table~", strHTML, dsHeader.Tables[2]);
            //if (strHTML.Contains("~PO_Annex2_Table~"))
            //    strHTML = PDFPageSetup.FunPubBindTable("~PO_Annex2_Table~", strHTML, dsHeader.Tables[3]);
            string strHTMLtemp = string.Empty;

            DataTable dtSale = dsLAS.Tables[0];

            DataTable tempSale = dtSale.Clone();
            DataView DV = dtSale.AsDataView();
            DataView DVDet = dsLAS.Tables[1].AsDataView();

            Int32 k = 0;

            string SignedFile = "";

            for (int i = 0; i < dtSale.Rows.Count; i++)
            {

                if (dtSale.Rows[i]["Digi_Sign_Enable"].ToString() == "1")
                    nDigi_Flag = 1;

                if (intLASID > 2169 && dtSale.Rows[i]["Way_Bill"].ToString().ToUpper() == "YES")
                {
                    strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, 3, 0, 71, Convert.ToString(intLASID));
                }
                else if (intLASID > 2169 && dtSale.Rows[i]["Way_Bill"].ToString().ToUpper() == "NO")
                {
                    strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, 3, 0, 70, Convert.ToString(intLASID));
                }

                strHTMLtemp = strHTML;
                k = i + 1;
                DV.RowFilter = "Record_Cnt = '" + k + "'";
                
                tempSale = DV.ToTable();

                DVDet.RowFilter = "Invoice_No = '" + tempSale.Rows[0]["Invoice_No"].ToString() + "'";

                if (dtSale.Rows.Count > 0 && dtSale.Rows[i]["Is_VAT"].ToString() == "1")
                {
                    if (strHTMLtemp.Contains("~SGSTCGST~"))
                        strHTMLtemp = FunPubDeleteTable("~SGSTCGST~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~IGST~"))
                        strHTMLtemp = FunPubDeleteTable("~IGST~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~KGST~"))
                        strHTMLtemp = FunPubDeleteTable("~KGST~", strHTMLtemp);

                    strHTMLtemp = FunPubDeleteTableHeader("~VAT~", strHTMLtemp);
                }
                else if (dtSale.Rows.Count > 0 && dtSale.Rows[i]["Is_IGST"].ToString() == "1")
                {
                    if (strHTMLtemp.Contains("~SGSTCGST~"))
                        strHTMLtemp = FunPubDeleteTable("~SGSTCGST~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~KGST~"))
                        strHTMLtemp = FunPubDeleteTable("~KGST~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~IGST~"))
                    {
                        strHTMLtemp = PDFPageSetup.FunPubBindTable("~IGST~", strHTMLtemp, DVDet.ToTable());
                    }

                    if (strHTMLtemp.Contains("~VAT~"))
                        strHTMLtemp = FunPubDeleteTable("~VAT~", strHTMLtemp);
                }
                else if (dtSale.Rows.Count > 0 && dtSale.Rows[i]["Is_KGST"].ToString() == "1")
                {
                    if (strHTMLtemp.Contains("~SGSTCGST~"))
                        strHTMLtemp = FunPubDeleteTable("~SGSTCGST~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~IGST~"))
                        strHTMLtemp = FunPubDeleteTable("~IGST~", strHTMLtemp);

                    strHTMLtemp = FunPubDeleteTableHeader("~KGST~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~VAT~"))
                        strHTMLtemp = FunPubDeleteTable("~VAT~", strHTMLtemp);
                }
                else
                {
                    if (strHTMLtemp.Contains("~IGST~"))
                        strHTMLtemp = FunPubDeleteTable("~IGST~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~KGST~"))
                        strHTMLtemp = FunPubDeleteTable("~KGST~", strHTMLtemp);

                    //strHTMLtemp = FunPubDeleteTableHeader("~SGSTCGST~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~VAT~"))
                        strHTMLtemp = FunPubDeleteTable("~VAT~", strHTMLtemp);

                    if (strHTMLtemp.Contains("~SGSTCGST~"))
                    {
                        strHTMLtemp = PDFPageSetup.FunPubBindTable("~SGSTCGST~", strHTMLtemp, DVDet.ToTable());
                    }
                }
                if(Convert.ToString(tempSale.Rows[0]["Invoice_No"]).Trim()!=string.Empty)
                {
                    InvoiceNoValue = Convert.ToString(tempSale.Rows[0]["Invoice_No"]).Replace("/", "_");
                }
                else
                    InvoiceNoValue="Check";

                FunPubGetQrCode(Convert.ToString(tempSale.Rows[0]["QRCode"]));

                strQRCode = Server.MapPath(".") + "\\PDF Files\\LASQRCode.png";

                strHTMLtemp = PDFPageSetup.FunPubBindCommonVariables(strHTMLtemp, tempSale);

                tempSale.Clear();

                List<string> listImageName = new List<string>();
                listImageName.Add("~CompanyLogo~");
                listImageName.Add("~InvoiceSignStamp~");
                listImageName.Add("~POSignStamp~");
                listImageName.Add("~OPCQRCode~");
                List<string> listImagePath = new List<string>();

                //opc082 start
                //listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                if (intLASID > 2333)
                {
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                }
                else
                {
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
                }
                //opc082 end
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));
                listImagePath.Add(strQRCode);
                strHTMLtemp = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTMLtemp);
                //FunPubSaveDocument(strHTMLtemp, FilePath, FileName + k.ToString(), "P");

                if (intLASID > 2354)
                {
                    intRegAddress = 1;
                }
                else
                {
                    intRegAddress = 0;
                }

                FileName = InvoiceNoValue;
                FunPrintWord(strHTMLtemp, FilePath, FileName, "0", intRegAddress);

                //SignedFile = FilePath + "Signed" + intUserId.ToString() + ".pdf";
                SignedFile = Server.MapPath(".") + "\\PDF Files\\Signed\\";

                if (!System.IO.Directory.Exists(SignedFile))
                {
                    System.IO.Directory.CreateDirectory(SignedFile);
                }

                string DownFile = FilePath + FileName + ".pdf";

                if (nDigi_Flag == 1)
                {
                    S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                    ObjPDFSign.DigiPDFSign(DownFile, SignedFile + Path.GetFileName(DownFile), "RIGHT");

                    filepaths.Add(SignedFile + Path.GetFileName(DownFile));
                }
                else
                {
                    filepaths.Add(FilePath + FileName + ".pdf");
                }
            }

            string OutputFile = FilePath + "LeaseAssetSale" + intUserId.ToString() + ".pdf";
            
            if (nDigi_Flag == 1)
            {
                string filePath = Server.MapPath(".") + "\\PDF Files\\Signed.zip";

                //before creation of compressed folder,deleting it if exists
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }


                if (!File.Exists(filePath))
                {
                    //creating a zip file from one folder to another folder
                    ZipFile.CreateFromDirectory(SignedFile, filePath);

                    //Delete The excel file which is created

                    string[] str = Directory.GetFiles(SignedFile);
                    foreach (string fileName in str)
                    {
                        File.Delete(fileName);
                    }

                    if (Directory.Exists(SignedFile))
                    {
                        Directory.Delete(SignedFile);
                    }
                }
                SignedFile = filePath;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, "P");


                //S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                //if (nDigi_Flag == 1)
                //    ObjPDFSign.DigiPDFSign(OutputFile, SignedFile, "RIGHT");
                //else
                    SignedFile = OutputFile;
            }


            //FunPriDownloadFile(SignedFile);
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(SignedFile, false, 0);
            string strScipt1 = "";
            if (SignedFile.Contains("/File.pdf"))
            {
                strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + SignedFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            }
            else
            {
                strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + SignedFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPrintWord(string strHTML, string strnewfile, string strnewfile1, string strIsCov,int intRegAddress)
    {
        string strhtmlFile = (strnewfile + "Bill_Html" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
        string strwordFile = string.Empty;
        string strpdfFile = string.Empty;
        string strpdfFileName = string.Empty;

        strpdfFileName = strnewfile1;
        strpdfFile = strnewfile + "\\" + strnewfile1;

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;

            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            Microsoft.Office.Interop.Word.Range rng = null;
            string img = string.Empty;

            //if (oDoc.InlineShapes.Count >= 1)
            //{
            //    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            //    string strFileName = (string)AppReader.GetValue("ImagePath", typeof(string));// @"D:\S3G\SISLS3GPLayer\SISLS3GPLayer\Config.ini";// 
            //    img = strFileName + @"\login\s3g_logo.png";
            //    rng = oDoc.InlineShapes[1].Range;
            //    rng.Delete();
            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //}

            //if (oDoc.InlineShapes.Count == 1)
            //{
            //    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            //    string strFileName = (string)AppReader.GetValue("ImagePath", typeof(string));// @"D:\S3G\SISLS3GPLayer\SISLS3GPLayer\Config.ini";// 
            //    img = strFileName + @"\Billsign.png";
            //    rng = oDoc.InlineShapes[1].Range;
            //    rng.Delete();
            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //}
            object fileFormat = null;


            fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            file = strpdfFile;

            //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            ////oDoc.ActiveWindow.Selection.TypeText(" \t ");
            //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            //Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            //oDoc.ActiveWindow.Selection.TypeText(" / ");
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            if (Utility.StringToDate(txtLASDate.Text) < Utility.StringToDate("01-Oct-2020"))
            {
                oDoc.PageSetup.LeftMargin = 0f;
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            }

            //oDoc.ActiveWindow.Selection.Font.Size = 9;
            //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);

            if (strIsCov == "0")
            {
                if (Utility.StringToDate(txtLASDate.Text) < Utility.StringToDate("30-Jun-2020"))
                {
                    string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
                    oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                    oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    oDoc.ActiveWindow.Selection.Font.Size = 7;
                    oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                    oDoc.ActiveWindow.Selection.TypeText(textDisc);
                }

                oDoc.ActiveWindow.Selection.Font.Size = 9;
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);

                Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
                oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText(" / ");
                Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
                oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);
            }
            string text = "";
            //string text = "\nRegd. Office: D-16, Nelson Chambers, Nelson Manickam Road, Chennai, Tamil Nadu - 600029.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            //string text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            if (intRegAddress == 0)
            {
                text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            }
            if (intRegAddress == 1)
            {
                text = "\nRegd. Office: Ground Floor, Block No B, 809, EGA Trade Centre, Poonamallee High Road, Kilpauk, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            }

            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.TypeText(text);
            //System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            //string strFileName = @"D:\S3G_OPC\OPC_Service_LIVE_GST"; //(string)AppReader.GetValue("ImagePath", typeof(string));
            //string footerimagepath = strFileName + @"\TemplateImages\1\OPCFooter.png";
            string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);


        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriDownloadFile(string OutputFile)
    {

        Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(OutputFile);


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
                    //PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
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

    private string FunPubDeleteTableHeader(string strtblName, string strHTML)
    {
        try
        {
            string row = "";
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strrow = "";
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);
            string strtempTable = strTable;

            startTag = "<TR";
            endTag = "</TR>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
            strtempTable = strtempTable.Replace(strrow, "");
            strHTML = strHTML.Replace(strTable, strtempTable);

            return strHTML;
        }
        catch (Exception ex)
        {
            // ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private string FunPubDeleteTable(string strtblName, string strHTML)
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

    private void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType)
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
            oDoc.PageSetup.LeftMargin = 0f;
            oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;

            string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            oDoc.ActiveWindow.Selection.Font.Size = 7;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.TypeText(textDisc);

            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);

            //string text = "\nRegd. Office: D-16, Nelson Chambers, Nelson Manickam Road, Chennai, Tamil Nadu - 600029.\nHead Office: 303, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.        ";
            string text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.TypeText(text);

            //string strFileName = @"D:\S3G_OPC\OPC_Service_LIVE_GST";
            //string footerimagepath = strFileName + @"\TemplateImages\1\OPCFooter.png";
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

    protected void btnPrintAnnexures_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();

            //Procparam.Add("@closure_ID", Convert.ToString(intClosureDetailId));
            //Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            //Procparam.Add("@USER_ID", Convert.ToString(intUserID));

            //Procparam.Add("@option", Convert.ToString(2));
            //dtpv = new DataTable();
            //dtpv = Utility.GetDefaultData("S3G_FUNDMGT_GETPrintPMC", Procparam);
            Dictionary<string, string> dictLAS = new Dictionary<string, string>();
            dictLAS.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictLAS.Add("@Usr_ID", Convert.ToString(intUserId));
            dictLAS.Add("@LAS_ID", Convert.ToString(intLASID));
            dictLAS.Add("@option", Convert.ToString(1));

            DataTable dtSale = new DataTable();
            //dtSale = Utility.GetDefaultData("S3G_GETPrint_LeaseAssetSale", dictLAS);
            DataSet dsLAS = Utility.GetDataset("S3G_GETPrint_LeaseAssetSale", dictLAS);
            dtSale = dsLAS.Tables[0];
            GridView Grv = new GridView();

            Grv.DataSource = dtSale;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Annex-Sale Of Asset.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");
                dtHeader.Columns.Add("Column2");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Annexure for Lease Asset Sale";
                dtHeader.Rows.Add(row);

                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 10;
                grv1.Rows[1].Cells[0].ColumnSpan = 10;

                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;

                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;
                grv1.RenderControl(htw);

                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnGoInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            hdnIsMapInvoiceChanged.Value = "0";
            ProPageNumRWDsp = 1;
            ViewState["InvTotal"] = null;
            ViewState["SelectAll"] = null;
            ViewState["Is_New"] = "1";
            FunPriBindDspDtl();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnInvClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearMapDtls(1);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ExportToExcel();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnMove_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(hdnIsMapInvoiceChanged.Value) == "1")
            {
                Utility.FunShowAlertMsg(this, "Certains Details are changed.kindly click Update before Move");
                return;
            }
            //FunPriMoveSaleDtls();
            FunPriClearDict();
            dictParam.Add("@Option", "4");
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            dictParam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));
            DataTable dtDspInv = Utility.GetDefaultData("S3G_LAD_GetLASPanumInvDtl", dictParam);
            if (dtDspInv != null && dtDspInv.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtDspInv.Rows[0]["Error_Code"]) == 0)
                {
                    string strAggregate = Convert.ToString(txtAggregateAmt.Text);
                    FunPriBindSaleDtl();
                    FunPriClearMapDtls(1);
                    txtAggregateAmt.Text = strAggregate;
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Either Sale Quantity or Selling State or Buyer Brnach are not entered for some Invoice");
                }

            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnMoveAll_Click(object sender, EventArgs e)
    {
        try
        {
            #region "Hide on 10Sep2015"

            //FunPriClearDict();

            //if (Convert.ToString(ddlmapsrchLessee.SelectedText) != "" && Convert.ToInt64(ddlmapsrchLessee.SelectedValue) > 0)
            //    dictParam.Add("@Customer_ID", Convert.ToString(ddlmapsrchLessee.SelectedValue));
            //if (Convert.ToString(ddlmapSrchTranche.SelectedText) != "" && Convert.ToInt64(ddlmapSrchTranche.SelectedValue) > 0)
            //    dictParam.Add("@Tranche_ID", Convert.ToString(ddlmapSrchTranche.SelectedValue));
            //if (Convert.ToString(ddlmapSrchAsset.SelectedText) != "" && Convert.ToInt64(ddlmapSrchAsset.SelectedValue) > 0)
            //    dictParam.Add("@Asset_Category_ID", Convert.ToString(ddlmapSrchAsset.SelectedValue));
            //if (Convert.ToString(ddlmapSrchAssetType.SelectedText) != "" && Convert.ToInt64(ddlmapSrchAssetType.SelectedValue) > 0)
            //    dictParam.Add("@Asset_Type_ID", Convert.ToString(ddlmapSrchAssetType.SelectedValue));
            //if (Convert.ToString(ddlmapSrchAssetSubType.SelectedText) != "" && Convert.ToInt64(ddlmapSrchAssetSubType.SelectedValue) > 0)
            //    dictParam.Add("@Asset_SubType_ID", Convert.ToString(ddlmapSrchAssetSubType.SelectedValue));
            //if (Convert.ToString(ddlLATNo.SelectedText) != "" && Convert.ToInt64(ddlLATNo.SelectedValue) > 0)
            //    dictParam.Add("@LAS_ID", Convert.ToString(ddlLATNo.SelectedValue));

            //dictParam.Add("@Option", "15");
            //dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            //dictParam.Add("@User_ID", Convert.ToString(intUserId));
            //dictParam.Add("@Mode", (Convert.ToString(strMode) == "") ? "C" : Convert.ToString(strMode));

            //Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);
            //FunPriMoveSaleDtls();

            #endregion

            FunPriClearDict();
            dictParam.Add("@Option", "2");
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            dictParam.Add("@Mode", (Convert.ToString(strMode) == "" || Convert.ToString(strMode) == "M") ? "C" : Convert.ToString(strMode));
            DataTable dtDspInv = Utility.GetDefaultData("S3G_LAD_GETLASDisposeDtl", dictParam);

            if (dtDspInv != null && dtDspInv.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtDspInv.Rows[0]["Error_Code"]) == 0)
                {
                    FunPriBindSaleDtl();
                    FunPriClearMapDtls(1);
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Either Sale Quantity or Selling State or Buyer Brnach are not entered for some Invoice");
                }

            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        try
        {
            hdnIsMapInvoiceChanged.Value = "1";
            ViewState["SelectAll"] = 1;
            FunPriBindDspDtl();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }
    protected void btnInvoicePrinting_Click(object sender, EventArgs e)
    {
        //DataTable dtInvoicePrinting;
        //FunPriUpdateInvoiceStatus();
        //dtInvoicePrinting = BuildTableForLASInvoice();
        //Session["AssetInfo"] = dtInvoicePrinting;
        ////ShowPDF();


        ////hdnPrint.Value = "1";
        //string strScipt = "window.open('../LoanAdmin/S3GRptLoanAd_LeaseAssetSale.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Lease Asset Sale", strScipt, true);
    }

    protected void btnMoveTransferDtl_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvRSDtls == null)
            {
                Utility.FunShowAlertMsg(this, "No Rental Schedule's found to Transfer");
                return;
            }
            Int32 intChkCnt = 0;
            for (Int32 i = 0; i < gvRSDtls.Rows.Count; i++)
            {
                CheckBox cbxSelectRS = (CheckBox)gvRSDtls.Rows[i].FindControl("cbxSelectRS");
                if (cbxSelectRS.Checked == true)
                    intChkCnt++;
            }

            if (intChkCnt == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast One Rental Schedule to Transfer");
                return;
            }
            hdnIsChange.Value = "";
            blnGo = true;
            FunPriBindGrid();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnMapInvUpdate_Click(object sender, EventArgs e)
    {

        try
        {
            if (gvMapInvoiceDtl != null && gvMapInvoiceDtl.Rows.Count > 0)
            {
                Int32 iChkCnt = 0;
                for (Int32 i = 0; i < gvMapInvoiceDtl.Rows.Count; i++)
                {
                    CheckBox chkSelectInv = (CheckBox)gvMapInvoiceDtl.Rows[i].FindControl("ChkSelectInvoice");
                    if (chkSelectInv.Checked == true)
                    {
                        iChkCnt++;
                    }
                }

                if (iChkCnt == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atleast one record to update");
                    return;
                }

                StringBuilder strBuild = new StringBuilder();
                strBuild.Append("<Root>");
                for (Int32 i = 0; i < gvMapInvoiceDtl.Rows.Count; i++)
                {
                    CheckBox chkSelectInv = (CheckBox)gvMapInvoiceDtl.Rows[i].FindControl("ChkSelectInvoice");
                    TextBox txtSaleStateID = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtSellingStateID");
                    TextBox txtSaleStateDesc = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtmapSellingState");
                    TextBox txtTaxType = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtmapTaxType");
                    CheckBox cbxCForm = (CheckBox)gvMapInvoiceDtl.Rows[i].FindControl("cbxmapCForm");
                    TextBox txtCFormNo = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtmapCFormNo");
                    TextBox txtBuyerBranch = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtMapBuyerBranch");
                    TextBox txtBuyer_GSTIN = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtBuyer_GSTIN");
                    TextBox txtBuyer_Delivery_Address = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtBuyer_Delivery_Address");
                    TextBox txtShip_From = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtShip_From");
                    TextBox txtSaleQty = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtMapSaleQty");
                    TextBox txtSalePrice = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtMapSalePrice");
                    Label lblPaSaRefID = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapPaSaRefID");
                    Label lblInvoiceNo = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapInvoiceNo");
                    Label lblInvoiceID = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapInvoiceID");
                    Label lblRVAmount = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapRVAmt");

                    TextBox txtShip_From_Pin = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtShip_From_Pin");
                    TextBox txtShip_To_Pin = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtShip_To_Pin");
                    TextBox txtBuyer_PIN_Code = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtBuyer_PIN_Code");

                    if (chkSelectInv.Checked == true)
                    {
                        if (Convert.ToString(txtSaleStateID.Text) == "" || Convert.ToString(txtSaleStateID.Text) == "0")
                        {
                            Utility.FunShowAlertMsg(this, "Enter the Selling State for Invoice No " + Convert.ToString(lblInvoiceNo.Text));
                            return;
                        }
                        if (Convert.ToString(txtBuyerBranch.Text) == "")
                        {
                            Utility.FunShowAlertMsg(this, "Enter the Buyer Branch for Invoice No " + Convert.ToString(lblInvoiceNo.Text));
                            return;
                        }
                    }

                    strBuild.Append("<Details INVOICE_ID = '" + Convert.ToString(lblInvoiceID.Text) + "'");
                    strBuild.Append(" PASA_REF_ID = '" + Convert.ToString(lblPaSaRefID.Text) + "'");
                    strBuild.Append(" SLAE_QTY = '" + Convert.ToString(txtSaleQty.Text) + "'");
                    strBuild.Append(" SALE_PRICE = '" + ((Convert.ToString(txtSalePrice.Text) == "") ? "0" : Convert.ToString(txtSalePrice.Text)) + "'");
                    strBuild.Append(" SALE_STATE_ID = '" + Convert.ToString(txtSaleStateID.Text) + "'");
                    strBuild.Append(" SALE_STATE_DESC = '" + Convert.ToString(txtSaleStateDesc.Text) + "'");
                    strBuild.Append(" TAX_TYPE = '" + Convert.ToString(txtTaxType.Text) + "'");
                    strBuild.Append(" CFORMAPPL = '" + ((cbxCForm.Checked == true) ? "1" : "0") + "'");
                    strBuild.Append(" CFORMNO = '" + Convert.ToString(txtCFormNo.Text) + "'");
                    strBuild.Append(" BUYER_BRANCH = '" + Convert.ToString(txtBuyerBranch.Text) + "'");
                    strBuild.Append(" BUYER_GSTIN = '" + Convert.ToString(txtBuyer_GSTIN.Text) + "'");
                    strBuild.Append(" BUYER_DELIVERY_ADDRESS = '" + Convert.ToString(txtBuyer_Delivery_Address.Text) + "'");
                    strBuild.Append(" SHIP_FROM = '" + Convert.ToString(txtShip_From.Text) + "'");
                    strBuild.Append(" IS_CHECKED = '" + ((chkSelectInv.Checked == true) ? "1" : "0") + "'");
                    strBuild.Append(" RV_AMT = '" + Convert.ToString(lblRVAmount.Text) + "'");

                    strBuild.Append(" SHIP_FROM_PIN = '" + Convert.ToString(txtShip_From_Pin.Text) + "'");
                    strBuild.Append(" SHIP_TO_PIN = '" + Convert.ToString(txtShip_To_Pin.Text) + "'");
                    strBuild.Append(" Buyer_PIN_Code = '" + Convert.ToString(txtBuyer_PIN_Code.Text) + "'");

                    strBuild.Append(" />");
                }
                strBuild.Append("</Root>");
                FunPriClearDict();

                dictParam.Add("@Option", "17");
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                dictParam.Add("@User_ID", Convert.ToString(intUserId));
                dictParam.Add("@Mode", (Convert.ToString(strMode) == "" || Convert.ToString(strMode) == "M") ? "C" : Convert.ToString(strMode));
                dictParam.Add("@XML_Invoice", Convert.ToString(strBuild));
                dictParam.Add("@Asset_Flag", ddlAssetFlag.SelectedValue);

                DataTable dtTotal = new DataTable();
                dtTotal = Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);

                if (dtTotal != null && dtTotal.Rows.Count > 0)
                {
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFAvlblInvAmt")).Text = dtTotal.Rows[0]["Avlbl_Inv_Amt"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFAvlblQty")).Text = dtTotal.Rows[0]["Avlb_Qty"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFSalePrice")).Text = dtTotal.Rows[0]["Sale_Price"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFRVAmt")).Text = dtTotal.Rows[0]["RV_Amount"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFTotal")).Text = dtTotal.Rows[0]["Sale_Qty"].ToString();
                }
                hdnIsMapInvoiceChanged.Value = "0";
                Utility.FunShowAlertMsg(this, "Updated successfully…! Click move to proceed…!");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnViewTot_Click(object sender, EventArgs e)
    {
        FunPriGetTotal();
    }

    protected void btnValidate_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearDict();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));

            if (Convert.ToInt32(obj_Page.ddlEntitytype.SelectedValue) == 1)
                dictParam.Add("@Customer_ID", Convert.ToString(obj_Page.ViewState["CustomerID"]));
            if (Convert.ToInt32(obj_Page.ddlEntitytype.SelectedValue) > 1)
                dictParam.Add("@Vendor_ID", Convert.ToString(obj_Page.ViewState["CustomerID"]));
            dictParam.Add("@LASDate", Convert.ToString(Utility.StringToDate(txtLASDate.Text)));
            dictParam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));

            DataTable dtVldt = Utility.GetDefaultData("S3G_LAD_VALIDATELASDSPDTL", dictParam);
            if (dtVldt != null && dtVldt.Rows.Count > 0)
            {
                btnValidate.Enabled = false;
                btnMoveUpload.Enabled = (Convert.ToInt32(dtVldt.Rows[0]["Error_Code"]) == 0) ? true : false;
                btnException.Enabled = (Convert.ToInt32(dtVldt.Rows[0]["Error_Code"]) == 1) ? true : false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnMoveUpload_Click(object sender, EventArgs e)
    {
        try
        {
            hdnIsMapInvoiceChanged.Value = "0";
            ViewState["Is_New"] = "1";
            FunPriBindDspDtl();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnException_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearDict();
            dictParam.Add("@Option", "20");
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            DataTable dtExcp = Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);

            GridView Grv = new GridView();

            Grv.DataSource = dtExcp;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Annex-UploadException.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnExportDspDtl_Click(object sender, EventArgs e)
    {
        try
        {
            String strProcName = string.Empty;
            FunPriClearDict();
            if (Convert.ToInt32(ddllastype.SelectedValue) == 2)     //Transfer
            {
                dictParam.Add("@XML_PaSaRefID", FunPriGenerateXML());
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                dictParam.Add("@Option", "3");
                strProcName = "S3G_LAD_GETLASTransferDtl";
            }
            else            //Dispose
            {
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                dictParam.Add("@User_ID", Convert.ToString(intUserId));
                dictParam.Add("@IsExport", "1");
                if (Convert.ToString(strMode) == "C" || Convert.ToString(strMode) == "M")
                {
                    dictParam.Add("@Option", "3");
                    dictParam.Add("@Aggregate_Amt", (Convert.ToString(txtAggregateAmt.Text) == "") ? "0" : Convert.ToString(txtAggregateAmt.Text));
                    dictParam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));
                    strProcName = "S3G_LAD_GetLASPanumInvDtl";
                }
                else
                {
                    dictParam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));
                    dictParam.Add("@LAS_ID", Convert.ToString(intLASID));
                    strProcName = "S3G_LAD_GetLASDSPDTL_Query";
                }
            }
            DataTable dtExport = Utility.GetDefaultData(strProcName, dictParam);

            GridView Grv = new GridView();

            Grv.DataSource = dtExport;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Sale Of Asset Disposal.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception objException)
        {
        }
    }

    #endregion

    #region "GRIDVIEW EVENTS"



    #endregion

    #region"HYPERLINK EVENTS"

    protected void hyplnkView_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void lnkbtnDownloadTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = Server.MapPath(@"Upload Template\LAS Dispose Template.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=FileUpload.xls");
            Response.TransmitFile(strPath);
            Response.End();
        }
        catch (Exception objException)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "TEXTBOX EVENTS"

    //protected void txtDspQty_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string strSelectID = ((TextBox)sender).ClientID;
    //        int _iRowIdx = Utility.FunPubGetGridRowID("grvDisPosedDtl", strSelectID);
    //        Label lblDspViDetID = (Label)grvDisPosedDtl.Rows[_iRowIdx].FindControl("lblDspViDetID");
    //        Label lblOrgQty = (Label)grvDisPosedDtl.Rows[_iRowIdx].FindControl("lblOrgQty");
    //        TextBox txtDspQty = (TextBox)grvDisPosedDtl.Rows[_iRowIdx].FindControl("txtDspQty");

    //        if (Convert.ToInt32(txtDspQty.Text) > Convert.ToInt32(lblOrgQty.Text))
    //        {
    //            Utility.FunShowAlertMsg(this, "Disposed Quantity should not exceed Actual Quantity Value.(ie." + Convert.ToString(lblOrgQty.Text) + ")");
    //            txtDspQty.Text = lblOrgQty.Text;
    //        }


    //        DataTable dtDspDtl = (DataTable)ViewState["DisposedDtl"];
    //        DataRow[] drDtl = dtDspDtl.Select("Invoice_Detail_ID = " + Convert.ToString(lblDspViDetID.Text));
    //        if (drDtl.Length > 0)
    //        {
    //            drDtl[0]["Disposed_Quantity"] = Convert.ToInt32(txtDspQty.Text);
    //            dtDspDtl.AcceptChanges();
    //        }
    //        ViewState["DisposedDtl"] = dtDspDtl;

    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    protected void txtLASDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(txtLessee.Text) == "")
                hdnLessee.Value = "";
            if (Convert.ToString(hdnLessee.Value) != "" && ddllastype.SelectedValue == "2" && txtLASDate.Text != "")
            {
                txtTrancheName.Text = hdnTranche.Value = "";
                FunPriGetTrasferRSDtl();
            }
            else if (ddllastype.SelectedValue == "3" && chkDspManual.Checked && txtLASDate.Text != "")
            {
                FunPriBindDspDtl();
            }
            else
            {
                FunPriClearTnsfrGrid();
                tblManual.Style["display"] = "none";
                tblUpload.Style["display"] = (chkDspUpload.Checked == true) ? "block" : "none";
                btnValidate.Enabled = btnMoveUpload.Enabled = btnException.Enabled = false;
                FunPriClrDspInvtl();
                FunPriClearMapDtls(2);
                FunPriClearUploadDtls();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void txtLessee_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(txtLessee.Text) == "")
                hdnLessee.Value = "";
            if (Convert.ToString(hdnLessee.Value) != "")
            {
                txtTrancheName.Text = hdnTranche.Value = "";
                FunPriGetTrasferRSDtl();
            }
            else
            {
                FunPriClearTnsfrGrid();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void txtTrancheName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(txtTrancheName.Text) == "")
                hdnTranche.Value = "";
            if (Convert.ToString(hdnLessee.Value) != "")
            {
                FunPriGetTrasferRSDtl();
            }
            else
            {
                FunPriClearTnsfrGrid();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void txtAggregateAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriBindSaleDtl();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnAggregateAmt_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindSaleDtl();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region"CHECKBOX EVENTS"

    #region "Hide"

    //protected void chkDspSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string strSelectID = ((CheckBox)sender).ClientID;
    //        int _iRowIdx = Utility.FunPubGetGridRowID("grvDisPosedDtl", strSelectID);
    //        Label lblDspViDetID = (Label)grvDisPosedDtl.Rows[_iRowIdx].FindControl("lblDspViDetID");
    //        CheckBox chkDspSelect = (CheckBox)grvDisPosedDtl.Rows[_iRowIdx].FindControl("chkDspSelect");

    //        DataTable dtDspDtl = (DataTable)ViewState["DisposedDtl"];
    //        DataRow[] drDtl = dtDspDtl.Select("Invoice_Detail_ID = " + Convert.ToString(lblDspViDetID.Text));
    //        if (drDtl.Length > 0)
    //        {
    //            drDtl[0]["Is_Checked"] = (chkDspSelect.Checked == true) ? 1 : 0;
    //            dtDspDtl.AcceptChanges();
    //        }
    //        ViewState["DisposedDtl"] = dtDspDtl;
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    //protected void chkSelectAllInvoice_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox chkSelectAllInvoice = (CheckBox)grvDisPosedDtl.HeaderRow.FindControl("chkSelectAllInvoice");
    //        DataTable dtDspDtl = (DataTable)ViewState["DisposedDtl"];
    //        dtDspDtl.Columns.Remove("Is_Checked");
    //        DataColumn dclmn = new DataColumn("Is_Checked");
    //        dclmn.DefaultValue = (chkSelectAllInvoice.Checked == true) ? 1 : 0;
    //        dtDspDtl.Columns.Add(dclmn);
    //        dtDspDtl.AcceptChanges();
    //        blnSlctAll = chkSelectAllInvoice.Checked;
    //        FunPriBindGrid(2, dtDspDtl);
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    //protected void cbxSelectRS_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string strSelectID = ((CheckBox)sender).ClientID;
    //        int _iRowIdx = Utility.FunPubGetGridRowID("gvRSDtls", strSelectID);
    //        Label lblPasaRefID = (Label)gvRSDtls.Rows[_iRowIdx].FindControl("lblgdPaSaRefID");
    //        Int32 intChkCnt = 0;
    //        for (Int32 i = 0; i < gvRSDtls.Rows.Count; i++)
    //        {
    //            CheckBox cbxSelectRS = (CheckBox)gvRSDtls.Rows[i].FindControl("cbxSelectRS");
    //            if (cbxSelectRS.Checked == true)
    //                intChkCnt++;
    //        }

    //        CheckBox cbxSelectAll = (CheckBox)gvRSDtls.HeaderRow.FindControl("cbxSelectAllRs");
    //        cbxSelectAll.Checked = (intChkCnt == gvRSDtls.Rows.Count) ? true : false;

    //        FunPriBindGrid();
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    //protected void cbxSelectAllRs_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox cbxSelectAll = (CheckBox)gvRSDtls.HeaderRow.FindControl("cbxSelectAllRs");
    //        for (Int32 i = 0; i < gvRSDtls.Rows.Count; i++)
    //        {
    //            CheckBox cbxSelectRS = (CheckBox)gvRSDtls.Rows[i].FindControl("cbxSelectRS");
    //            cbxSelectRS.Checked = cbxSelectAll.Checked;
    //        }
    //        FunPriBindGrid();
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    //protected void ChkSelectInvoice_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string strSelectID = ((CheckBox)sender).ClientID;
    //        int _iRowIdx = Utility.FunPubGetGridRowID("gvMapInvoiceDtl", strSelectID);

    //        Label lblInvoiceID = (Label)gvMapInvoiceDtl.Rows[_iRowIdx].FindControl("lblmapInvoiceID");
    //        CheckBox ChkSelect = (CheckBox)gvMapInvoiceDtl.Rows[_iRowIdx].FindControl("ChkSelectInvoice");

    //        if (dictParam != null)
    //            dictParam.Clear();
    //        else
    //            dictParam = new Dictionary<string, string>();

    //        dictParam.Add("@Option", (ChkSelect.Checked == true) ? "10" : "11");
    //        dictParam.Add("@Invoice_ID", Convert.ToString(lblInvoiceID.Text));
    //        dictParam.Add("@Mode", (Convert.ToString(strMode) == "") ? "C" : Convert.ToString(strMode));
    //        dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
    //        dictParam.Add("@User_ID", Convert.ToString(intUserId));

    //        Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
    //    }
    //}

    #endregion

    protected void chkDspManual_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDspManual.Checked == true)
            {
                if (Convert.ToInt32(ddlEntitytype.SelectedValue) == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select Buyer Type");
                    chkDspManual.Checked = false;
                    return;
                }

                if (Convert.ToInt32(ddlAssetFlag.SelectedValue) == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select a Asset Flag");
                    chkDspManual.Checked = false;
                    return;
                }

                if (ViewState["CustomerID"] == null || Convert.ToString(ViewState["CustomerID"]) == "0")
                {
                    Utility.FunShowAlertMsg(this, Convert.ToString(rfvcmbCustomer.ErrorMessage));
                    chkDspManual.Checked = false;
                    return;
                }
            }
            chkDspUpload.Checked = false;
            tblUpload.Style["display"] = "none";
            tblManual.Style["display"] = (chkDspManual.Checked == true) ? "block" : "none";
            FunPriClrDspInvtl();
            FunPriClearMapDtls(2);
            FunPriClearUploadDtls();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void chkDspUpload_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDspUpload.Checked == true)
            {
                if (Convert.ToInt32(ddlEntitytype.SelectedValue) == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select Buyer Type");
                    chkDspUpload.Checked = false;
                    return;
                }

                if (Convert.ToInt32(ddlAssetFlag.SelectedValue) == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select a Asset Flag");
                    chkDspUpload.Checked = false;
                    return;
                }

                if (ViewState["CustomerID"] == null || Convert.ToString(ViewState["CustomerID"]) == "0")
                {
                    Utility.FunShowAlertMsg(this, Convert.ToString(rfvcmbCustomer.ErrorMessage));
                    chkDspUpload.Checked = false;
                    return;
                }
            }

            chkDspManual.Checked = false;
            tblManual.Style["display"] = "none";
            tblUpload.Style["display"] = (chkDspUpload.Checked == true) ? "block" : "none";
            btnValidate.Enabled = btnMoveUpload.Enabled = btnException.Enabled = false;
            FunPriClrDspInvtl();
            FunPriClearMapDtls(2);
            FunPriClearUploadDtls();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region

    protected void lnkDspRemoveAll_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearDict();

            dictParam.Add("@Option", "13");
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));

            Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);

            FunPriBindGrid(2, null);
            ucPageDispose.Visible = btnExportDspDtl.Enabled = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void lnkDspRemove_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvDisPosedDtl", strSelectID);

            Label lblInvoiceID = (Label)grvDisPosedDtl.Rows[_iRowIdx].FindControl("lblDspInvoiceID");

            FunPriClearDict();

            dictParam.Add("@Option", "11");
            dictParam.Add("@Invoice_ID", Convert.ToString(lblInvoiceID.Text));
            dictParam.Add("@Mode", (Convert.ToString(strMode) == "" || Convert.ToString(strMode) == "M") ? "C" : Convert.ToString(strMode));
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));

            Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);

            //FunPriMoveSaleDtls();
            FunPriBindSaleDtl();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    #endregion

    #endregion

    #region "Methods"

    public void ExportToExcel()
    {
        try
        {
            Guid objGuid;
            objGuid = Guid.NewGuid();
            DataTable dtPO = new DataTable();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Customer_Id", ddlmapsrchLessee.SelectedValue);
            if (ddlmapSrchTranche.SelectedValue != "0")
                dictParam.Add("@Tranche_ID", ddlmapSrchTranche.SelectedValue);
            if (ddlmapSrchAsset.SelectedValue != "0")
                dictParam.Add("@Asset_Category_ID", ddlmapSrchAsset.SelectedValue);
            if (ddlmapSrchAssetType.SelectedValue != "0")
                dictParam.Add("@Asset_Type_ID", ddlmapSrchAssetType.SelectedValue);
            if (ddlmapSrchAssetSubType.SelectedValue != "0")
                dictParam.Add("@Asset_SubType_ID", ddlmapSrchAssetSubType.SelectedValue);
            if (ddlLATNo.SelectedValue != "0")
                dictParam.Add("@LAS_ID", ddlLATNo.SelectedValue);
            dictParam.Add("@User_Id", intUserId.ToString());
            dictParam.Add("@LASDate", Convert.ToString(Utility.StringToDate(txtLASDate.Text)));
            dictParam.Add("@Asset_Flag", ddlAssetFlag.SelectedValue);
            dtPO = Utility.GetDefaultData("S3G_LAD_GetTrnsfrInvDtlForExport", dictParam);

            string filename = "LAS Dispose Template.xls";

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
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Export LAS Dispose Template");

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    private void FunPriLoadLov()
    {
        try
        {
            FunPriClearDict();

            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            dictParam.Add("@Lob_ID", Convert.ToString(3));

            DataSet dsLOV = Utility.GetDataset("S3G_LAD_GETLASLookup", dictParam);

            ddlLOB.BindDataTable(dsLOV.Tables[0], new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
                ddlLOB.ClearDropDownList();
            }

            ddlEntitytype.FillDataTable(dsLOV.Tables[1], "ID", "Name", true);

            ddllastype.FillDataTable(dsLOV.Tables[2], "Lookup_Code", "Lookup_Description", true);

            //if (dsLOV.Tables[4].Rows.Count > 0)
            //{
            //    ddlBranch.SelectedValue = Convert.ToString(dsLOV.Tables[4].Rows[0]["ID"]);
            //    ddlBranch.SelectedText = Convert.ToString(dsLOV.Tables[4].Rows[0]["Name"]);
            //}

            FunPriLoadDocumentPath(dsLOV.Tables[3]);
            ViewState["ColumnList"] = dsLOV.Tables[5];
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadDocumentPath(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                ViewState["Docpath"] = Convert.ToString(dt.Rows[0]["Document_Path"]);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Define Document Path Setup");
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriInitiateLOVCode()
    {
        try
        {
            Label lblCustomerCode = (Label)ucCustomerAddress.FindControl("lblCustomerCode");
            Label lblCustomerName = (Label)ucCustomerAddress.FindControl("lblCustomerName");
            lblCustomerCode.Text = "Buyer Code";
            lblCustomerName.Text = "Buyer Name";
            if (Convert.ToInt32(ddlEntitytype.SelectedValue) == 6)
            {
                //pnlLesseeInfo.GroupingText = "Vendor Information";
                ucCustomerCodeLov.strLOV_Code = "ENVENDOR";
                //lblCustomerCode.Text = "Entity Code";
                //lblCustomerName.Text = "Entity Name";
                //rfvcmbCustomer.ErrorMessage = "Select a Vendor";
            }
            else if (Convert.ToInt32(ddlEntitytype.SelectedValue) == 7)
            {
                //pnlLesseeInfo.GroupingText = "Sundry Creditors Information";
                ucCustomerCodeLov.strLOV_Code = "ENSUNDRY";
                //lblCustomerCode.Text = "Entity Code";
                //lblCustomerName.Text = "Entity Name";
                //rfvcmbCustomer.ErrorMessage = "Select a Sundry Creditors";
            }
            else if (Convert.ToInt32(ddlEntitytype.SelectedValue) == 8)
            {
                //pnlLesseeInfo.GroupingText = "Dealer Information";
                ucCustomerCodeLov.strLOV_Code = "ENDEALER";
                //lblCustomerCode.Text = "Entity Code";
                //lblCustomerName.Text = "Entity Name";
                //rfvcmbCustomer.ErrorMessage = "Select a Dealer";
            }
            else if (Convert.ToInt32(ddlEntitytype.SelectedValue) == 10)
            {
                //pnlLesseeInfo.GroupingText = "Employee Information";
                ucCustomerCodeLov.strLOV_Code = "ENEMP";
                //lblCustomerCode.Text = "Entity Code";
                //lblCustomerName.Text = "Entity Name";
                //rfvcmbCustomer.ErrorMessage = "Select a Employee";
            }
            else if (Convert.ToInt32(ddlEntitytype.SelectedValue) == 14)
            {
                //pnlLesseeInfo.GroupingText = "Other Debtors Information";
                ucCustomerCodeLov.strLOV_Code = "ENOTHRDBT";
                //lblCustomerCode.Text = "Entity Code";
                //lblCustomerName.Text = "Entity Name";
                //rfvcmbCustomer.ErrorMessage = "Select a Other Debtors";
            }
            else
            {
                //pnlLesseeInfo.GroupingText = "Lessee Information";
                ucCustomerCodeLov.strLOV_Code = "CMD";
                //lblCustomerCode.Text = "Customer Code";
                //lblCustomerName.Text = "Customer Name";
                //rfvcmbCustomer.ErrorMessage = "Select a Customer";
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnblDsblPnl()
    {
        try
        {
            if (Convert.ToString(strMode) == "C")
            {
                ddlTPAN.Clear();
                txtTAcClosure.Text = string.Empty;
                ddlEntitytype.SelectedValue = "0";
            }
            if (Convert.ToInt32(ddllastype.SelectedValue) == 2)
            {
                //trTransfer.Visible = ddlTPAN.IsMandatory = true;
                ddlEntitytype.Enabled = pnlLesseeInfo.Visible = rfvEntityType.Enabled = rfvDueDate.Enabled = false;
                ViewState["CustomerID"] = 0;
                lblEntityType.CssClass = "styleDisplayLabel";
                ucDisposedPaging.Visible = txtAggregateAmt.Visible = btnAggregateAmt.Visible = lblAggregate.Visible = chkDspManual.Checked =
                chkDspUpload.Checked = divDspDtl.Visible = false;
                txtLessee.Visible = trTransfer.Visible = true;
                pnlMapInvoiceDtls.Style["display"] = tblManual.Style["display"] = tblUpload.Style["display"] = "none";
            }
            else if (Convert.ToInt32(ddllastype.SelectedValue) == 3)
            {
                //trTransfer.Visible = ddlTPAN.IsMandatory = true;
                pnlRSDtls.Visible = trTransfer.Visible = txtLessee.Visible = chkDspManual.Checked = chkDspUpload.Checked = false;
                //trDispose.Style["display"] = "block";
                ddlEntitytype.Enabled = pnlLesseeInfo.Visible = rfvEntityType.Enabled =
                txtAggregateAmt.Visible = btnAggregateAmt.Visible = lblAggregate.Visible = rfvDueDate.Enabled = true;
                ViewState["CustomerID"] = 0;
                lblEntityType.CssClass = "styleReqFieldLabel";
                pnlMapInvoiceDtls.Style["display"] = "block";
                tblManual.Style["display"] = tblUpload.Style["display"] = "none";
            }
            else
            {
                pnlRSDtls.Visible = trTransfer.Visible = txtLessee.Visible =
                ddlEntitytype.Enabled = pnlLesseeInfo.Visible = rfvEntityType.Enabled =
                txtAggregateAmt.Visible = btnAggregateAmt.Visible = lblAggregate.Visible = ucDisposedPaging.Visible = chkDspManual.Checked = chkDspUpload.Checked = false;
                ViewState["CustomerID"] = 0;
                lblEntityType.CssClass = "styleDisplayLabel";
                pnlMapInvoiceDtls.Style["display"] = tblManual.Style["display"] = tblUpload.Style["display"] = "none";
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnblDsblCtrl()
    {
        try
        {
            if (PageMode == PageModes.Modify) // Modify
            {
                txtTrancheName.Visible = lblTrancheName.Visible = false;
                btnSave.Enabled = btnClear.Visible = lblCode.Visible = ucCustomPaging.Visible = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                ddlBranch.ReadOnly = ddlTPAN.ReadOnly = true;
                ucCustomerCodeLov.Visible = txtLessee.Enabled =
                ddlEntitytype.Enabled = rfvEntityType.Enabled = false;
                //trDispose.Style["display"] = "none";
                FunPriLoadLASDetails();
                if (Convert.ToInt32(ddllastype.SelectedValue) == 3)
                {
                    pnlRSDtls.Visible = false;
                    pnlMapInvoiceDtls.Visible = pnlLesseeInfo.Visible =
                    lblAggregate.Visible = txtAggregateAmt.Visible = btnAggregateAmt.Visible =
                    grvDisPosedDtl.Visible = ucPageDispose.Visible = divDspDtl.Visible = true;
                    chkDspManual.Checked = true;
                    chkDspManual.Enabled = chkDspUpload.Checked = ucDisposedPaging.Visible = GridViewContainer.Visible =
                    btnSelectAll.Enabled = btnMove.Enabled = btnMapInvUpdate.Enabled = btnViewTot.Enabled = chkDspUpload.Visible =
                    btnExport.Visible = rfvLeseeName.Enabled = lblLesseeName.Visible = txtLessee.Visible =
                    btnPrintAnnexures.Enabled = btnPrintSale.Enabled = false;
                    tblUpload.Style["display"] = "none";
                    tblManual.Style["display"] = (chkDspManual.Checked == true) ? "block" : "none";
                    lblEntityType.CssClass = "styleReqFieldLabel";
                }
                else
                {
                    pnlMapInvoiceDtls.Visible = pnlLesseeInfo.Visible =
                    lblAggregate.Visible = txtAggregateAmt.Visible = btnAggregateAmt.Visible =
                    grvDisPosedDtl.Visible = ucPageDispose.Visible = divDspDtl.Visible = false;
                }
                // This is to block selecting the NEW LOB & BRANCH & LAS combination in Modify Mode
                ddlLOB.ClearDropDownList();
                ddllastype.ClearDropDownList();
                ddlEntitytype.ClearDropDownList();
                txtLASDate.ReadOnly = btnSave.Enabled = true;
                CalendarLASDate.Enabled = false;
                ddlAssetFlag.ClearDropDownList();
            }
            else if (PageMode == PageModes.Query) // Query 
            {
                txtTrancheName.Visible = lblTrancheName.Visible = false;
                btnSave.Enabled = btnClear.Visible = lblCode.Visible = ucCustomPaging.Visible = ucPageDispose.Visible =
                btnExportDspDtl.Enabled = btnAggregateAmt.Enabled = btnAggregateAmt.Enabled = txtAggregateAmt.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                ddlBranch.ReadOnly = ddlTPAN.ReadOnly = true;
                ucCustomerCodeLov.Visible = txtLessee.Enabled = btnMoveTransferDtl.Visible = false;
                FunPriLoadLASDetails();
                FunPriEnblDsblPnl();
                // This is to block selecting the NEW LOB & BRANCH & LAS combination in Query Mode
                if (grvDisPosedDtl != null)
                {
                    grvDisPosedDtl.Columns[grvDisPosedDtl.Columns.Count - 1].Visible = false;
                    grvDisPosedDtl.Enabled = false;
                }

                if (gvRSDtls != null)
                    gvRSDtls.Columns[0].Visible = false;
                if (gvTrancheDtl != null)
                    gvTrancheDtl.Columns[0].Visible = false;

                ddlLOB.ClearDropDownList();
                ddllastype.ClearDropDownList();
                ddlEntitytype.ClearDropDownList();
                pnlMapInvoiceDtls.Style["display"] = "none";
                //trDispose.Style["display"] = "none";
                txtLASDate.ReadOnly = true;
                CalendarLASDate.Enabled = false;
                btnPrintAnnexures.Enabled = btnPrintSale.Enabled = (Convert.ToInt32(ddllastype.SelectedValue) == 3) ? true : false;
                ddlAssetFlag.ClearDropDownList();
            }
            else
            {
                txtTrancheName.Visible = lblTrancheName.Visible = btnClear.Visible = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                txtLASDate.Text = DateTime.Now.ToString(DateFormate);
                txtAggregateAmt.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, false, false, "Aggregate Amount");
                btnPrintSale.Enabled = btnPrintAnnexures.Enabled = false; //By Siva.K on 28APR2015 For Report 
                pnlLesseeInfo.Visible = pnlLeaseGrid.Visible = ucCustomPaging.Visible = ddlEntitytype.Enabled = pnlRSDtls.Visible =
                ucPageDispose.Visible = btnExportDspDtl.Enabled = false;
                pnlMapInvoiceDtls.Style["display"] = "none";
                //trDispose.Style["display"] = "none";
                txtLessee.Enabled = true;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriGetInvoiceDtl()
    {
        try
        {
            FunPriClearDict();

            dictParam.Add("@Option", "1");
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@Pa_Sa_Ref_ID", Convert.ToString(ddlTPAN.SelectedValue));

            DataSet dsTransfer = Utility.GetDataset("S3G_LAD_GETLASCMNLST", dictParam);
            if (dsTransfer.Tables[0].Rows.Count > 0)
                txtTAcClosure.Text = Convert.ToString(dsTransfer.Tables[0].Rows[0]["Closure_Date"]);

            if (Convert.ToInt32(ddllastype.SelectedValue) == 3)
            {
                dictParam.Clear();
                dictParam.Add("@Option", "2");
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                dictParam.Add("@Pa_Sa_Ref_ID", Convert.ToString(ddlTPAN.SelectedValue));

                dsTransfer = Utility.GetDataset("S3G_LAD_GetLASPanumInvDtl", dictParam);
                if (dsTransfer != null && dsTransfer.Tables[0].Rows.Count > 0)
                {
                    pnlLeaseGrid.Visible = ucPageDispose.Visible = btnExportDspDtl.Enabled = true;
                    FunPriBindGrid(2, dsTransfer.Tables[0]);
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "No Invoice Details found");
                    pnlLeaseGrid.Visible = ucPageDispose.Visible = btnExportDspDtl.Enabled = false;
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriGetTotal()
    {
        try
        {
            ViewState["InvTotal"] = null;
            if (gvMapInvoiceDtl != null && gvMapInvoiceDtl.Rows.Count > 0)
            {
                Int32 iChkCnt = 0;
                for (Int32 i = 0; i < gvMapInvoiceDtl.Rows.Count; i++)
                {
                    CheckBox chkSelectInv = (CheckBox)gvMapInvoiceDtl.Rows[i].FindControl("ChkSelectInvoice");
                    if (chkSelectInv.Checked == true)
                    {
                        iChkCnt++;
                    }
                }

                StringBuilder strBuild = new StringBuilder();
                strBuild.Append("<Root>");
                for (Int32 i = 0; i < gvMapInvoiceDtl.Rows.Count; i++)
                {
                    CheckBox chkSelectInv = (CheckBox)gvMapInvoiceDtl.Rows[i].FindControl("ChkSelectInvoice");
                    TextBox txtSaleStateID = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtSellingStateID");
                    TextBox txtSaleStateDesc = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtmapSellingState");
                    TextBox txtTaxType = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtmapTaxType");
                    CheckBox cbxCForm = (CheckBox)gvMapInvoiceDtl.Rows[i].FindControl("cbxmapCForm");
                    TextBox txtCFormNo = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtmapCFormNo");
                    TextBox txtBuyerBranch = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtMapBuyerBranch");
                    TextBox txtSaleQty = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtMapSaleQty");
                    TextBox txtSalePrice = (TextBox)gvMapInvoiceDtl.Rows[i].FindControl("txtMapSalePrice");
                    Label lblPaSaRefID = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapPaSaRefID");
                    Label lblInvoiceNo = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapInvoiceNo");
                    Label lblInvoiceID = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapInvoiceID");
                    Label lblRVAmount = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapRVAmt");

                    strBuild.Append("<Details INVOICE_ID = '" + Convert.ToString(lblInvoiceID.Text) + "'");
                    strBuild.Append(" PASA_REF_ID = '" + Convert.ToString(lblPaSaRefID.Text) + "'");
                    strBuild.Append(" SLAE_QTY = '" + Convert.ToString(txtSaleQty.Text) + "'");
                    strBuild.Append(" SALE_PRICE = '" + ((Convert.ToString(txtSalePrice.Text) == "") ? "0" : Convert.ToString(txtSalePrice.Text)) + "'");
                    strBuild.Append(" SALE_STATE_ID = '" + Convert.ToString(txtSaleStateID.Text) + "'");
                    strBuild.Append(" SALE_STATE_DESC = '" + Convert.ToString(txtSaleStateDesc.Text) + "'");
                    strBuild.Append(" TAX_TYPE = '" + Convert.ToString(txtTaxType.Text) + "'");
                    strBuild.Append(" CFORMAPPL = '" + ((cbxCForm.Checked == true) ? "1" : "0") + "'");
                    strBuild.Append(" CFORMNO = '" + Convert.ToString(txtCFormNo.Text) + "'");
                    strBuild.Append(" BUYER_BRANCH = '" + Convert.ToString(txtBuyerBranch.Text) + "'");
                    strBuild.Append(" IS_CHECKED = '" + ((chkSelectInv.Checked == true) ? "1" : "0") + "'");
                    strBuild.Append(" RV_AMT = '" + Convert.ToString(lblRVAmount.Text) + "'");
                    strBuild.Append(" />");
                }
                strBuild.Append("</Root>");
                FunPriClearDict();

                dictParam.Add("@Option", "17");
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                dictParam.Add("@User_ID", Convert.ToString(intUserId));
                dictParam.Add("@Mode", (Convert.ToString(strMode) == "" || Convert.ToString(strMode) == "M") ? "C" : Convert.ToString(strMode));
                dictParam.Add("@XML_Invoice", Convert.ToString(strBuild));
                DataTable dtTotal = new DataTable();
                dtTotal = Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);

                if (dtTotal != null && dtTotal.Rows.Count > 0)
                {
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFAvlblInvAmt")).Text = dtTotal.Rows[0]["Avlbl_Inv_Amt"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFAvlblQty")).Text = dtTotal.Rows[0]["Avlb_Qty"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFSalePrice")).Text = dtTotal.Rows[0]["Sale_Price"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFRVAmt")).Text = dtTotal.Rows[0]["RV_Amount"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFTotal")).Text = dtTotal.Rows[0]["Sale_Qty"].ToString();
                }

                ViewState["InvTotal"] = dtTotal;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriGetDisposedDtl()
    {
        try
        {
            FunPriClearDict();

            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            dictParam.Add("@Option", "1");
            DataTable dtDispose = Utility.GetDefaultData("S3G_LAD_GETLASDisposeDtl", dictParam);
            ViewState["DisposedDtl"] = dtDispose;
            FunPriBindGrid(2, dtDispose);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriGetCustomerAddress(Int64 CustomerID)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", intCompanyId.ToString());
            dictParam.Add("@ID", Convert.ToString(CustomerID));
            if (Convert.ToInt32(ddlEntitytype.SelectedValue) == 1)
            {
                dictParam.Add("@TypeID", "144");                //Customer Details                
            }
            else
            {
                dictParam.Add("@TypeID", "145");                //Entity Details
            }

            DataTable dtCustomer = Utility.GetDefaultData("S3G_LOANAD_GETCustomerorEntityDetails", dictParam);
            ucCustomerAddress.ClearCustomerDetails();
            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Text = txtCustomerCode.Text = Convert.ToString(dtCustomer.Rows[0]["Code"]);
                ucCustomerAddress.SetCustomerDetails(Convert.ToString(dtCustomer.Rows[0]["Code"]),
                        Convert.ToString(dtCustomer.Rows[0]["Address1"]) + "\n" +
                        ((Convert.ToString(dtCustomer.Rows[0]["Address2"]) == "") ? "" : Convert.ToString(dtCustomer.Rows[0]["Address2"]) + "\n") +
                Convert.ToString(dtCustomer.Rows[0]["city"]) + "\n" +
                Convert.ToString(dtCustomer.Rows[0]["state"]) + "\n" +
                Convert.ToString(dtCustomer.Rows[0]["country"]) + "\n" +
                Convert.ToString(dtCustomer.Rows[0]["pincode"]), Convert.ToString(dtCustomer.Rows[0]["Name"]), Convert.ToString(dtCustomer.Rows[0]["Telephone"]),
                Convert.ToString(dtCustomer.Rows[0]["mobile"]),
                Convert.ToString(dtCustomer.Rows[0]["email"]), Convert.ToString(dtCustomer.Rows[0]["website"]));
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriBindGrid(Int32 intOption, DataTable dt)
    {
        try
        {
            if (intOption == 1)                     //Invoice Transfer Details
            {
                grvTransferDtl.DataSource = dt;
                grvTransferDtl.DataBind();
            }
            else if (intOption == 2)                 //Disposed Details
            {
                //ViewState["DisposedDtl"] = dt;
                grvDisPosedDtl.DataSource = dt;
                grvDisPosedDtl.DataBind();
                if (dt != null && grvDisPosedDtl.HeaderRow != null)
                {
                    CheckBox chkSelectAllInvoice = (CheckBox)grvDisPosedDtl.HeaderRow.FindControl("chkSelectAllInvoice");
                    chkSelectAllInvoice.Checked = blnSlctAll;
                }
            }
            else if (intOption == 3)                //RS Tranfer details
            {
                gvRSDtls.DataSource = dt;
                gvRSDtls.DataBind();
            }
            else if (intOption == 4)                //Tranche Tranfer details
            {
                gvTrancheDtl.DataSource = dt;
                gvTrancheDtl.DataBind();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearCustomerDetails()
    {
        try
        {
            ucCustomerAddress.ClearCustomerDetails();
            ViewState["CustomerID"] = 0;
            txtCustomerCode.Text = "";
            ucCustomerCodeLov.FunPubClearControlValue();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearGrid()
    {
        try
        {
            FunPriBindGrid(1, null);
            FunPriBindGrid(2, null);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearUploadDtls()
    {
        try
        {
            lblCurrentPath.Text = lblExcelCurrentPath.Text = lblUploadErrorMsg.InnerHtml = string.Empty;
            hyplnkView.Enabled = btnUpload.Enabled = false;
            ViewState["FilePath"] = null;
            hdnSelectedPath.Dispose();
            chkSelect.Dispose();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private bool FunPriValidateExcel(DataSet ds)
    {
        bool blnRslt = true;
        try
        {
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                blnRslt = true;
            }
            else
            {
                blnRslt = false;
                Utility.FunShowAlertMsg(obj_Page, "Unable to Show the details, No records found");
                lblExcelCurrentPath.Text = string.Empty;
            }
        }
        catch (Exception objException)
        {
            blnRslt = false;
        }
        return blnRslt;
    }

    private bool FunPriValidateData()
    {
        bool blnRslt = true;
        try
        {
            FunPriFormXML();
            if (ViewState["DisposedDtl"] != null)
            {
                DataTable dtDispose = (DataTable)ViewState["DisposedDtl"];
                //DataRow[] drDispose = dtDispose.Select("IS_Checked = 1");
                //if (drDispose.Length == 0)
                //{
                //    blnRslt = false;
                //    Utility.FunShowAlertMsg(this, "Select atleast one Invoice Detail");
                //}

                if (dtDispose.Rows.Count == 0)
                {
                    blnRslt = false;
                    Utility.FunShowAlertMsg(this, "Select atleast one Invoice Detail");
                }

                DataRow[] drDispose = dtDispose.Select("Sale_Price = 0");
                if (drDispose.Length > 0)
                {
                    blnRslt = false;
                    Utility.FunShowAlertMsg(this, "Sale Price should not be 0");
                }

                drDispose = dtDispose.Select("Dsp_Quantity = 0");
                if (drDispose.Length > 0)
                {
                    blnRslt = false;
                    Utility.FunShowAlertMsg(this, "Saleable Quantity should not be 0");
                }
            }
            else
            {
                blnRslt = false;
                Utility.FunShowAlertMsg(this, "No Invoice detail found");
            }
        }
        catch (Exception objException)
        {
            blnRslt = false;
        }
        return blnRslt;
    }

    private static DataSet FunPriImportExcelXLS(string FileName, bool hasHeaders)
    {
        string HDR = hasHeaders ? "Yes" : "No";
        string strConn;
        if (FileName.Substring(FileName.LastIndexOf('.')).ToLower() == ".xlsx")
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        else
        {
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
        }
        DataSet Dtoutput = new DataSet();
        DataSet output = new DataSet();
        DataTable dtExcel = new DataTable();
        try
        {
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string sheet = schemaRow["TABLE_NAME"].ToString();
                    if (!sheet.EndsWith("_"))
                    {
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);
                        cmd.CommandType = CommandType.Text;
                        DataTable outputTable = new DataTable(sheet);
                        output.Tables.Add(outputTable);
                        new OleDbDataAdapter(cmd).Fill(outputTable);
                    }
                }
                conn.Close();

                output.Tables[0].Columns[0].ColumnName = "AssetCategory";
                output.Tables[0].Columns[1].ColumnName = "AssetType";
                output.Tables[0].Columns[2].ColumnName = "AssetSubType";
                output.Tables[0].Columns[3].ColumnName = "InvoiceNumber";
                output.Tables[0].Columns[4].ColumnName = "TaxPercentage";
                output.Tables[0].Columns[5].ColumnName = "SalePrice";


                string strFilter = "((AssetCategory = ' ' OR AssetCategory IS NULL)";
                strFilter = strFilter + " OR (AssetType = ' ' OR AssetType IS NULL)";
                strFilter = strFilter + " OR (AssetSubType = ' ' OR AssetSubType IS NULL)";
                strFilter = strFilter + " OR (InvoiceNumber = ' ' OR InvoiceNumber IS NULL)";
                strFilter = strFilter + " OR (TaxPercentage IS NULL)";
                strFilter = strFilter + " OR (SalePrice IS NULL))";

                DataRow[] drChequeRtn = output.Tables[0].Select(strFilter);

                foreach (DataRow drCredit in drChequeRtn)
                    drCredit.Delete();

                DataColumn dtClmn = new DataColumn("Uploaded_Date", typeof(DateTime));
                dtClmn.DefaultValue = System.DateTime.Now;
                output.Tables[0].Columns.Add(dtClmn);
                DataColumn dtClmn2 = new DataColumn("user_ID", typeof(System.Int32));
                dtClmn2.DefaultValue = Convert.ToInt32(obj_Page.intUserId);
                output.Tables[0].Columns.Add(dtClmn2);
                output.Tables[0].AcceptChanges();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
            Utility.FunShowAlertMsg(obj_Page, "Invalid Format");
            throw objException;
        }
        return output;
    }

    private void FunPriSaveDisposeDtl(DataTable dt)
    {
        try
        {
            FunPriClearDict();

            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            dictParam.Add("@Option", "2");
            DataTable dtDlt = Utility.GetDefaultData("S3G_LAD_GETLASDisposeDtl", dictParam);

            //string conn = WebConfigurationManager.AppSettings["BulkCopy"].ToString();
            string conn = FunPriGetConnectionString();
            SqlBulkCopy sbcd = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default);
            sbcd.DestinationTableName = "S3G_Tmp_LASDisposeDtl";
            sbcd.ColumnMappings.Add("Vendor code", "Vendor_Code");
            sbcd.ColumnMappings.Add("Vendor state", "Vendor_State");
            sbcd.ColumnMappings.Add("RS No", "RS_No");
            sbcd.ColumnMappings.Add("PI Number", "PI_Number");
            sbcd.ColumnMappings.Add("PI Date", "PI_Date");
            sbcd.ColumnMappings.Add("VI Number", "VI_Number");
            sbcd.ColumnMappings.Add("VI Date", "VI_Date");
            sbcd.ColumnMappings.Add("Asset Category", "Asset_Category");
            sbcd.ColumnMappings.Add("Asset Type", "Asset_Type");
            sbcd.ColumnMappings.Add("Asset Sub Type", "Asset_SubType");
            sbcd.ColumnMappings.Add("AS No", "Asset_Serial_No");
            sbcd.ColumnMappings.Add("Sale Quantity", "Sale_Quantity");
            sbcd.ColumnMappings.Add("Sale Price", "Sale_Price");
            sbcd.ColumnMappings.Add("RS State", "RS_State");
            sbcd.ColumnMappings.Add("Selling State", "Selling_State");
            sbcd.ColumnMappings.Add("Buyer Billing Branch ", "Buyer_Billing_Branch");
            sbcd.ColumnMappings.Add("Sale Type for CST", "CForm_Appl");
            sbcd.ColumnMappings.Add("C Form Number", "CForm_Number");
            sbcd.ColumnMappings.Add("Uploaded_Date", "Uploaded_Date");
            sbcd.ColumnMappings.Add("user_ID", "user_ID");
            sbcd.BatchSize = 1000;
            sbcd.WriteToServer(dt);
            FunPriGetDisposedDtl();
        }
        catch (Exception objException)
        {
            Utility.FunShowAlertMsg(this, objException.Message);
            throw objException;
        }
    }

    private string FunPriGetConnectionString()
    {
        string ConnectionString;
        try
        {
            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            string strFileName = (string)AppReader.GetValue("INIFILEPATH", typeof(string));
            if (File.Exists(strFileName))
            {
                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.LoadXml(File.ReadAllText(strFileName).Trim());
                XmlDocument conxmlDoc = xmlDoc;
                ConnectionString = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["connectionString"].Value;
            }
            else
            {
                throw new FileNotFoundException("Configuration file not found");
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return ConnectionString;
    }

    public static string FunPubGetDatabase()
    {
        try
        {
            XmlDocument conxmlDoc = xmlDoc;
            string ConnectionString = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["connectionString"].Value;
            string strDataProvider = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["providerName"].Value;
            string strConType = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["connnectionType"].Value;

            return ConnectionString;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private void FunPriSaveLAS()
    {
        ObjLeaseAssetSaleClient = new AssetMgtServicesReference.AssetMgtServicesClient();
        try
        {
            string strKey = "Save";
            objLASDT = new AssetMgtServices.S3G_LAD_LASDTLDataTable();
            AssetMgtServices.S3G_LAD_LASDTLRow Obj_LASRow = objLASDT.NewS3G_LAD_LASDTLRow();
            Obj_LASRow.Company_ID = Convert.ToInt32(intCompanyId);
            Obj_LASRow.Created_By = Convert.ToInt32(intUserId);
            Obj_LASRow.Customer_ID = (Convert.ToInt32(ddllastype.SelectedValue) == 2) ? Convert.ToInt64(hdnLessee.Value) : Convert.ToInt64(ViewState["CustomerID"]);
            Obj_LASRow.Entity_Type = Convert.ToInt32(ddlEntitytype.SelectedValue);
            Obj_LASRow.LAS_Date = Utility.StringToDate(txtLASDate.Text);
            Obj_LASRow.LAS_ID = intLASID;
            Obj_LASRow.LAS_Number = Convert.ToString(txtLASNo.Text);
            Obj_LASRow.LAS_Type = Convert.ToInt32(ddllastype.SelectedValue);
            Obj_LASRow.Lob_ID = Convert.ToInt32(ddlLOB.SelectedValue);

            if (txtDueDate.Text != "")
                Obj_LASRow.Due_Date = Utility.StringToDate(txtDueDate.Text);
            else
                Obj_LASRow.Due_Date = DateTime.Now;

            if (Convert.ToString(hdnTranche.Value) != "")
                Obj_LASRow.Tranche_ID = Convert.ToInt64(hdnTranche.Value);

            if (Convert.ToInt32(ddllastype.SelectedValue) == 2)
            {
                //Obj_LASRow.RS_ID = Convert.ToInt64(ddlTPAN.SelectedValue);
                //Obj_LASRow.XML_TransferDtl = Utility.FunPubFormXml(grvTransferDtl, true, false);
                Obj_LASRow.XML_PasaRefDtl = FunPriGenerateXML();
            }
            else if (Convert.ToInt32(ddllastype.SelectedValue) == 3)
            {
                Obj_LASRow.Aggregate_Amount = (Convert.ToString(txtAggregateAmt.Text) == "") ? 0 : Convert.ToDouble(txtAggregateAmt.Text);
                if (chkDspUpload.Checked == true)
                    Obj_LASRow.File_Path = Convert.ToString(lblCurrentPath.Text);
                //Obj_LASRow.RS_ID = Convert.ToInt64(ddlTPAN.SelectedValue);
                //DataTable dtDspDtl = (DataTable)ViewState["DisposedDtl"];
                //DataView dvDsp = new DataView(dtDspDtl);
                //dvDsp.RowFilter = "IS_Checked =1";
                //dtDspDtl = dvDsp.ToTable();
                //Obj_LASRow.XML_DisposeDtl = Utility.FunPubFormXml(dtDspDtl, true);
            }
            Obj_LASRow.Location_ID = 0;

            Obj_LASRow.Asset_Flag = Convert.ToInt32(ddlAssetFlag.SelectedValue);

            if (chkTCS.Checked)
                Obj_LASRow.TCS_Not_App = 1;
            else
                Obj_LASRow.TCS_Not_App = 0;

            objLASDT.AddS3G_LAD_LASDTLRow(Obj_LASRow);
            int intErrCode;
            string strLAS = string.Empty;
            intErrCode = ObjLeaseAssetSaleClient.FunPubCreateLASDetails(out strLAS, SerMode, ClsPubSerialize.Serialize(objLASDT, SerMode));
            if (intErrCode == 0)
            {
                if (intLASID > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + ddllastype.SelectedItem.Text + " details updated successfully - " + txtLASNo.Text + "');" + strRedirectPageView, true);
                    //Code Added by Ganapathy on 19/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                }
                else
                {
                    string strAlert = "";
                    if (Convert.ToInt32(ddllastype.SelectedValue) == 2)
                    {
                        strAlert = "Leased Asset Sale - Transfer details saved successfully - " + strLAS;
                        strAlert += @"\n\nWould you like to add one more Lease Asset Sale?";
                    }
                    else
                    {
                        strAlert = "Leased Asset Sale - Disposed details saved successfully - " + strLAS;
                        strAlert += @"\n\nWould you like to add one more Lease Asset Sale?";
                    }
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 19/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            else if (intErrCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                return;
            }
            else if (intErrCode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                return;
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.LoanAd_LaAsstsale_9);
                return;
            }
            else if (intErrCode == 4)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.LoanAd_LaAsstsale_15);
                return;
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Unable to Save");
                return;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        finally
        {
            ObjLeaseAssetSaleClient.Close();
        }
    }

    private void FunPriLoadLASDetails()
    {
        try
        {
            Dictionary<string, string> dictLAS = new Dictionary<string, string>();
            dictLAS.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictLAS.Add("@Usr_ID", Convert.ToString(intUserId));
            dictLAS.Add("@LAS_ID", Convert.ToString(intLASID));
            dictLAS.Add("@Mode", (Convert.ToString(strMode) == "" || Convert.ToString(strMode) == "M") ? Convert.ToString(strMode) : "C");
            DataSet dsLAS = Utility.GetDataset("S3G_LAD_GetLASDtl", dictLAS);

            if (dsLAS != null && dsLAS.Tables.Count > 0)
            {
                ddlLOB.SelectedValue = Convert.ToString(dsLAS.Tables[0].Rows[0]["Lob_ID"]);
                ddllastype.SelectedValue = Convert.ToString(dsLAS.Tables[0].Rows[0]["LAS_Type"]);
                if (ddllastype.SelectedValue == "3")
                {
                    btnPrintSale.Enabled = btnPrintAnnexures.Enabled = true; //By Siva.K on 28APR2015 For Report 
                }
                txtLASDate.Text = Convert.ToString(dsLAS.Tables[0].Rows[0]["LAS_Date"]);
                txtLASNo.Text = Convert.ToString(dsLAS.Tables[0].Rows[0]["LAS_Number"]);
                txtLASStatus.Text = Convert.ToString(dsLAS.Tables[0].Rows[0]["LAS_Status"]);
                ddlEntitytype.SelectedValue = Convert.ToString(dsLAS.Tables[0].Rows[0]["Entity_Type"]);
                //ddlTPAN.SelectedValue = Convert.ToString(dsLAS.Tables[0].Rows[0]["PaSaRefID"]);
                //ddlTPAN.SelectedText = Convert.ToString(dsLAS.Tables[0].Rows[0]["Panum"]);
                //txtTAcClosure.Text = Convert.ToString(dsLAS.Tables[1].Rows[0]["Closure_Date"]);

                txtDueDate.Text = Convert.ToString(dsLAS.Tables[0].Rows[0]["Due_Date"]);

                if (Convert.ToInt32(ddllastype.SelectedValue) == 2)
                {
                    //ddlTrancheName.SelectedValue = Convert.ToString(dsLAS.Tables[0].Rows[0]["Tranche_ID"]);
                    //ddlTrancheName.SelectedText = Convert.ToString(dsLAS.Tables[0].Rows[0]["Tranche_Name"]);
                    //ddlLesseeName.SelectedValue = Convert.ToString(dsLAS.Tables[0].Rows[0]["Customer_ID"]);
                    //ddlLesseeName.SelectedText = Convert.ToString(dsLAS.Tables[0].Rows[0]["Lessee_Name"]);
                    hdnLessee.Value = Convert.ToString(dsLAS.Tables[0].Rows[0]["Customer_ID"]);
                    txtLessee.Text = Convert.ToString(dsLAS.Tables[0].Rows[0]["Lessee_Name"]);
                    hdnTranche.Value = Convert.ToString(dsLAS.Tables[0].Rows[0]["Tranche_ID"]);
                    txtTrancheName.Text = Convert.ToString(dsLAS.Tables[0].Rows[0]["Tranche_Name"]);
                    pnlRSDtls.Visible = true;
                    FunPriBindGrid(4, dsLAS.Tables[1]);
                    FunPriBindGrid(3, dsLAS.Tables[2]);
                    FunPriBindGrid();
                }

                if (Convert.ToInt32(ddllastype.SelectedValue) == 3)
                {
                    txtAggregateAmt.Text = Convert.ToString(dsLAS.Tables[0].Rows[0]["Aggregate_Amount"]);
                    FunPriGetCustomerAddress(Convert.ToInt64(dsLAS.Tables[0].Rows[0]["Customer_ID"]));
                    ViewState["CustomerID"] = Convert.ToInt64(dsLAS.Tables[0].Rows[0]["Customer_ID"]);
                    ddlAssetFlag.SelectedValue = Convert.ToString(dsLAS.Tables[0].Rows[0]["Asset_Flag"]);
                    //FunPriBindGrid(2, dsLAS.Tables[1]);
                    FunPriBindSaleDtl();
                }

                chkTCS.Checked = dsLAS.Tables[0].Rows[0]["TCS_Not_App"].ToString() == "1" ? true : false;
            }

        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriFormXML()
    {
        try
        {
            DataTable dtDisposedDtl = ((DataTable)ViewState["DisposedDtl"]).Clone();
            DataView dvDsp = new DataView(dtDisposedDtl);
            dtDisposedDtl = dvDsp.ToTable("selected", false, "Invoice_ID", "Avlb_Quantity", "Dsp_Quantity", "Sale_Price", "RV_Amt"
                , "Profit_Loss", "VAT_CST_Perc", "Addtl_VAT_CST_Perc", "VAT_CST_Amt", "Addtl_VAT_CST_Amt");
            //dtDisposedDtl.Columns.Remove("Vendor_Code");
            //dtDisposedDtl.Columns.Remove("Vendor_Name");
            //dtDisposedDtl.Columns.Remove("Invoice_No");
            //dtDisposedDtl.Columns.Remove("Invoice_Date");
            //dtDisposedDtl.Columns.Remove("Asset_Category");
            //dtDisposedDtl.Columns.Remove("Asset_Type");
            //dtDisposedDtl.Columns.Remove("Asset_SubType");
            //dtDisposedDtl.Columns.Remove("Asset_SerialNo");
            //dtDisposedDtl.Columns.Remove("RS_No");
            //dtDisposedDtl.Columns.Remove("RS_State");

            for (Int32 i = 0; i < grvDisPosedDtl.Rows.Count; i++)
            {
                Label lblDspInvoiceID = (Label)grvDisPosedDtl.Rows[i].FindControl("lblDspInvoiceID");
                Label lblAvlbQty = (Label)grvDisPosedDtl.Rows[i].FindControl("lblAvlbQty");
                Label lblDspVATPerc = (Label)grvDisPosedDtl.Rows[i].FindControl("lblDspVATPerc");
                Label lblDspVatAmt = (Label)grvDisPosedDtl.Rows[i].FindControl("lblDspVatAmt");
                Label lblDspAddVatPerc = (Label)grvDisPosedDtl.Rows[i].FindControl("lblDspAddVatPerc");
                Label lblDspAddVatAmt = (Label)grvDisPosedDtl.Rows[i].FindControl("lblDspAddVatAmt");
                Label lblDspRSValue = (Label)grvDisPosedDtl.Rows[i].FindControl("lblDspRSValue");
                Label lblDspProfitLoss = (Label)grvDisPosedDtl.Rows[i].FindControl("lblDspProfitLoss");
                CheckBox chkDspSelect = (CheckBox)grvDisPosedDtl.Rows[i].FindControl("chkDspSelect");
                TextBox txtDspQty = (TextBox)grvDisPosedDtl.Rows[i].FindControl("txtDspQty");
                TextBox txtDspSaleAmount = (TextBox)grvDisPosedDtl.Rows[i].FindControl("txtDspSaleAmount");

                //if (chkDspSelect.Checked == true)
                //{
                DataRow drDsp = dtDisposedDtl.NewRow();
                drDsp["Invoice_ID"] = Convert.ToString(lblDspInvoiceID.Text);
                drDsp["Avlb_Quantity"] = Convert.ToInt32(lblAvlbQty.Text);
                drDsp["Dsp_Quantity"] = Convert.ToInt32(txtDspQty.Text);
                drDsp["Sale_Price"] = Convert.ToDouble(txtDspSaleAmount.Text);
                drDsp["RV_Amt"] = Convert.ToDouble(lblDspRSValue.Text);
                drDsp["Profit_Loss"] = Convert.ToDouble(lblDspProfitLoss.Text);
                //drDsp["Is_Checked"] = (chkDspSelect.Checked == true) ? 1 : 0;
                drDsp["VAT_CST_Perc"] = Convert.ToDouble(lblDspVATPerc.Text);
                drDsp["Addtl_VAT_CST_Perc"] = Convert.ToDouble(lblDspAddVatPerc.Text);
                drDsp["VAT_CST_Amt"] = Convert.ToDouble(lblDspVatAmt.Text);
                drDsp["Addtl_VAT_CST_Amt"] = Convert.ToDouble(lblDspAddVatAmt.Text);

                dtDisposedDtl.Rows.Add(drDsp);
                //}
            }
            ViewState["DisposedDtl"] = dtDisposedDtl;

        }
        catch (Exception objExcemption)
        {
            throw objExcemption;
        }
    }

    #region Page Methods

    private void FunPriBindGrid()
    {
        try
        {
            pnlLeaseGrid.Visible = ucCustomPaging.Visible = true;
            lblPagingErrorMessage.InnerText = "";
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@XML_PaSaRefID", FunPriGenerateXML());
            Procparam.Add("@Option", "1");
            if (intLASID != 0 && blnGo == false)
                Procparam.Add("@LAS_ID", Convert.ToString(intLASID));

            grvTransferDtl.BindGridView("S3G_LAD_GETLASTransferDtl", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvTransferDtl.Rows[0].Visible = grvTransferDtl.FooterRow.Visible = btnExportDspDtl.Enabled = false;
            }

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            if (!bIsNewRow)
            {
                grvTransferDtl.FooterRow.Visible = btnExportDspDtl.Enabled = true;
                FunPriCalcTrnsfrTtl();
            }

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblPagingErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblPagingErrorMessage.InnerText = ex.Message;
        }
    }

    private void FunPriBindDspDtl()
    {
        try
        {
            //if (Convert.ToString(hdnIsMapInvoiceChanged.Value) == "1")
            //{
            //    Utility.FunShowAlertMsg(this, "Certains Details are changed.kindly click Update before Move");
            //    return;
            //}
            lblDspErrorMsg.InnerText = "";
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjDspPaging.ProCompany_ID = intCompanyId;
            ObjDspPaging.ProUser_ID = intUserId;
            ObjDspPaging.ProTotalRecords = intTotalRecords;
            ObjDspPaging.ProCurrentPage = ProPageNumRWDsp;
            ObjDspPaging.ProPageSize = ProPageSizeRWDsp;
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            if (chkDspManual.Checked == true)
            {
                if (Convert.ToString(ddlmapsrchLessee.SelectedText) != "" && Convert.ToInt64(ddlmapsrchLessee.SelectedValue) > 0)
                    Procparam.Add("@Customer_ID", Convert.ToString(ddlmapsrchLessee.SelectedValue));
                if (Convert.ToString(ddlmapSrchTranche.SelectedText) != "" && Convert.ToInt64(ddlmapSrchTranche.SelectedValue) > 0)
                    Procparam.Add("@Tranche_ID", Convert.ToString(ddlmapSrchTranche.SelectedValue));
                if (Convert.ToString(ddlmapSrchAsset.SelectedText) != "" && Convert.ToInt64(ddlmapSrchAsset.SelectedValue) > 0)
                    Procparam.Add("@Asset_Category_ID", Convert.ToString(ddlmapSrchAsset.SelectedValue));
                if (Convert.ToString(ddlmapSrchAssetType.SelectedText) != "" && Convert.ToInt64(ddlmapSrchAssetType.SelectedValue) > 0)
                    Procparam.Add("@Asset_Type_ID", Convert.ToString(ddlmapSrchAssetType.SelectedValue));
                if (Convert.ToString(ddlmapSrchAssetSubType.SelectedText) != "" && Convert.ToInt64(ddlmapSrchAssetSubType.SelectedValue) > 0)
                    Procparam.Add("@Asset_SubType_ID", Convert.ToString(ddlmapSrchAssetSubType.SelectedValue));
                if (Convert.ToString(ddlLATNo.SelectedText) != "" && Convert.ToInt64(ddlLATNo.SelectedValue) > 0)
                    Procparam.Add("@LAS_ID", Convert.ToString(ddlLATNo.SelectedValue));
                if (ViewState["Is_New"] != null)
                    Procparam.Add("@Is_New", Convert.ToString(ViewState["Is_New"]));
                if (ViewState["SelectAll"] != null)
                    Procparam.Add("@SelectAll", Convert.ToString(ViewState["SelectAll"]));

                if (Convert.ToString(strMode) == "M")
                {
                    Procparam.Add("@LAS_Modify_ID", Convert.ToString(intLASID));
                }

                Procparam.Add("@Option", "1");
                Procparam.Add("@LASDate", Convert.ToString(Utility.StringToDate(txtLASDate.Text)));
                Procparam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));
                gvMapInvoiceDtl.BindGridView("S3G_LAD_GetTrnsfrInvDtlforDsp", Procparam, out intTotalRecords, ObjDspPaging, out bIsNewRow);
            }
            else if (chkDspUpload.Checked == true)
            {
                Procparam.Add("@Option", "1");
                if (ViewState["Is_New"] != null)
                    Procparam.Add("@Is_New", Convert.ToString(ViewState["Is_New"]));
                if (ViewState["SelectAll"] != null)
                    Procparam.Add("@SelectAll", Convert.ToString(ViewState["SelectAll"]));
                Procparam.Add("@LASDate", Convert.ToString(Utility.StringToDate(txtLASDate.Text)));

                Procparam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));

                gvMapInvoiceDtl.BindGridView("S3G_LAD_GETLASDisposeDtl", Procparam, out intTotalRecords, ObjDspPaging, out bIsNewRow);
            }
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                gvMapInvoiceDtl.Rows[0].Visible = ucDisposedPaging.Visible = btnMove.Enabled = btnSelectAll.Enabled = btnMapInvUpdate.Enabled =
                    btnViewTot.Enabled = GridViewContainer.Visible = false;
            }
            else
            {
                btnMove.Enabled = btnSelectAll.Enabled = btnMapInvUpdate.Enabled = btnViewTot.Enabled = gvMapInvoiceDtl.Visible =
                    GridViewContainer.Visible = true;
                btnMoveAll.Enabled = btnMoveAll.Visible = chkDspUpload.Checked;

                if (ViewState["InvTotal"] != null && ((DataTable)ViewState["InvTotal"]).Rows.Count > 0)
                {
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFAvlblInvAmt")).Text = ((DataTable)ViewState["InvTotal"]).Rows[0]["Avlbl_Inv_Amt"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFAvlblQty")).Text = ((DataTable)ViewState["InvTotal"]).Rows[0]["Avlb_Qty"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFSalePrice")).Text = ((DataTable)ViewState["InvTotal"]).Rows[0]["Sale_Price"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFRVAmt")).Text = ((DataTable)ViewState["InvTotal"]).Rows[0]["RV_Amount"].ToString();
                    ((Label)gvMapInvoiceDtl.FooterRow.FindControl("lblFTotal")).Text = ((DataTable)ViewState["InvTotal"]).Rows[0]["Sale_Qty"].ToString();
                }
            }
            ucDisposedPaging.Visible = true;
            ucDisposedPaging.Navigation(intTotalRecords, ProPageNumRWDsp, ProPageSizeRWDsp);
            ucDisposedPaging.setPageSize(ProPageSizeRWDsp);
            ViewState["Is_New"] = ViewState["SelectAll"] = 0;

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblDspErrorMsg.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblDspErrorMsg.InnerText = ex.Message;
        }

    }

    private string FunPriGenerateXML()
    {
        string strXML = "<Root>";
        try
        {
            if (gvRSDtls != null)
            {
                for (Int32 i = 0; i < gvRSDtls.Rows.Count; i++)
                {
                    Label lblPasaRefID = (Label)gvRSDtls.Rows[i].FindControl("lblgdPaSaRefID");
                    CheckBox cbxSelectRS = (CheckBox)gvRSDtls.Rows[i].FindControl("cbxSelectRS");
                    if (cbxSelectRS.Checked == true)
                        strXML = strXML + " <Details Pa_Sa_Ref_ID = '" + Convert.ToString(lblPasaRefID.Text) + "' />";
                    else if (strMode == "Q")
                        strXML = strXML + " <Details Pa_Sa_Ref_ID = '" + Convert.ToString(lblPasaRefID.Text) + "' />";
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        strXML = strXML + "</Root>";
        return strXML;
    }

    #endregion

    private void FunPriGetTrasferRSDtl()
    {
        try
        {
            FunPriClearTnsfrGrid();
            Dictionary<string, string> procparam = new Dictionary<string, string>();
            procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            procparam.Add("@User_ID", Convert.ToString(intUserId));
            //if (Convert.ToInt64(ddlLesseeName.SelectedValue) > 0 && Convert.ToString(ddlLesseeName.SelectedText) != "")
            //    procparam.Add("@Customer_ID", Convert.ToString(ddlLesseeName.SelectedValue));

            //procparam.Add("@Tranche_ID", Convert.ToString(ddlTrancheName.SelectedValue));

            if (Convert.ToString(hdnLessee.Value) != "")
                procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
            if (Convert.ToString(hdnTranche.Value) != "")
                procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
            procparam.Add("@LASDate", Convert.ToString(Utility.StringToDate(txtLASDate.Text)));
            procparam.Add("@Option", "4");

            DataSet dsRSDtl = Utility.GetDataset("S3G_LAD_GETLASCMNLST", procparam);
            if (dsRSDtl != null && Convert.ToInt32(dsRSDtl.Tables[1].Rows.Count) > 0)
            {
                pnlRSDtls.Visible = true;
                FunPriBindGrid(4, dsRSDtl.Tables[0]);
                FunPriBindGrid(3, dsRSDtl.Tables[1]);
                btnMoveTransferDtl.Visible = true;
            }
            else
            {
                btnMoveTransferDtl.Visible = false;
                Utility.FunShowAlertMsg(this, "No Rental Schedules Found");
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearTransferDtls()
    {
        try
        {
            txtLessee.Text = txtTrancheName.Text = hdnLessee.Value = hdnTranche.Value = "";
            lblLesseeName.Visible = txtLessee.Visible = pnlRSDtls.Visible = false;
            FunPriClearTnsfrGrid();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriCalcTrnsfrTtl()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@XML_PaSaRefID", FunPriGenerateXML());
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Option", "2");

            if (intLASID != 0)
                Procparam.Add("@LAS_ID", Convert.ToString(intLASID));

            DataTable dtTtl = Utility.GetDefaultData("S3G_LAD_GETLASTransferDtl", Procparam);
            Label lblTtlInv = (Label)grvTransferDtl.FooterRow.FindControl("lblFtrInvAmt");
            Label lblTtlLBT = (Label)grvTransferDtl.FooterRow.FindControl("lblFtrLBTAmt");
            Label lblTtlPT = (Label)grvTransferDtl.FooterRow.FindControl("lblFtrPTAmt");
            Label lblTtlET = (Label)grvTransferDtl.FooterRow.FindControl("lblFtrETAmt");
            Label lblTtlRC = (Label)grvTransferDtl.FooterRow.FindControl("lblFtrRCAmt");
            Label lblTtlSchdAmt = (Label)grvTransferDtl.FooterRow.FindControl("lblFtrSchdAmt");
            Label lblFtrCapitalizeAmt = (Label)grvTransferDtl.FooterRow.FindControl("lblFtrCapitalizeAmt");
            Label lblFtrRVAmt = (Label)grvTransferDtl.FooterRow.FindControl("lblFtrRVAmt");
            if (dtTtl != null && dtTtl.Rows.Count > 0)
            {
                lblTtlInv.Text = Convert.ToString(dtTtl.Rows[0]["Ttl_Inv"]);
                lblTtlLBT.Text = Convert.ToString(dtTtl.Rows[0]["Ttl_LBT"]);
                lblTtlPT.Text = Convert.ToString(dtTtl.Rows[0]["Ttl_PT"]);
                lblTtlET.Text = Convert.ToString(dtTtl.Rows[0]["Ttl_ET"]);
                lblTtlRC.Text = Convert.ToString(dtTtl.Rows[0]["Ttl_RC"]);
                lblTtlSchdAmt.Text = Convert.ToString(dtTtl.Rows[0]["Ttl_SchdAmt"]);
                lblFtrCapitalizeAmt.Text = Convert.ToString(dtTtl.Rows[0]["Ttl_CapitalAmt"]);
                lblFtrRVAmt.Text = Convert.ToString(dtTtl.Rows[0]["Ttl_RVAmt"]);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearMapDtls(Int32 intOption)
    {
        try
        {
            txtAggregateAmt.Text = string.Empty;
            ddlmapSrchAsset.Clear();
            ddlmapSrchAssetSubType.Clear();
            ddlmapSrchAssetType.Clear();
            ddlmapsrchLessee.Clear();
            ddlmapSrchTranche.Clear();
            ddlLATNo.Clear();
            gvMapInvoiceDtl.DataSource = null;
            gvMapInvoiceDtl.DataBind();
            ucDisposedPaging.Visible = false;
            btnViewTot.Enabled = btnMove.Enabled = btnSelectAll.Enabled = btnMoveAll.Enabled = btnUpload.Enabled = btnMapInvUpdate.Enabled = false;
            ViewState["InvTotal"] = null;
            FunPriClearDict();
            dictParam.Add("@Option", (intOption == 1) ? "12" : "13");
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriMoveSaleDtls()
    {
        try
        {
            FunPriClearDict();

            dictParam.Add("@Option", "3");
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            dictParam.Add("@Aggregate_Amt", (Convert.ToString(txtAggregateAmt.Text) == "") ? "0" : Convert.ToString(txtAggregateAmt.Text));
            dictParam.Add("@XML_SaleDtl", FunPriFormSlaeInvXML());
            dictParam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));
            DataTable dtInv = Utility.GetDefaultData("S3G_LAD_GetLASPanumInvDtl", dictParam);
            FunPriBindGrid(2, dtInv);

            FunPriClearMapDtls(1);
        }
        catch (Exception objExecption)
        {
            throw objExecption;
        }
    }

    private string FunPriFormSlaeInvXML()
    {
        string strXML = "<Root>";
        try
        {
            if (grvDisPosedDtl != null)
            {
                for (Int32 i = 0; i < grvDisPosedDtl.Rows.Count; i++)
                {
                    Label lblInvoiceID = (Label)grvDisPosedDtl.Rows[i].FindControl("lblDspInvoiceID");
                    TextBox txtQty = (TextBox)grvDisPosedDtl.Rows[i].FindControl("txtDspQty");
                    TextBox txtSaleAmount = (TextBox)grvDisPosedDtl.Rows[i].FindControl("txtDspSaleAmount");

                    strXML = strXML + " <Details Invoice_ID = '" + Convert.ToString(lblInvoiceID.Text) + "'";
                    strXML = strXML + " Sale_Qty = '" + Convert.ToString(txtQty.Text) + "'";
                    strXML = strXML + " Sale_Amt = '" + Convert.ToString(txtSaleAmount.Text) + "' />";
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        strXML = strXML + "</Root>";
        return strXML;
    }

    private void FunPriClearTnsfrGrid()
    {
        try
        {
            pnlRSDtls.Visible = pnlLASTransfer.Visible = ucCustomPaging.Visible = pnlLeaseGrid.Visible = false;
            FunPriBindGrid(1, null);
            FunPriBindGrid(3, null);
            FunPriBindGrid(4, null);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriBindSaleDtl()
    {
        try
        {
            pnlLeaseGrid.Visible = ucPageDispose.Visible = true;
            lblSalePgingErrMsg.InnerText = "";
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRWSale;
            ObjPaging.ProPageSize = ProPageSizeRWSale;
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            if (Convert.ToString(strMode) == "C" || Convert.ToString(strMode) == "M")
            {
                Procparam.Add("@Option", "3");
                Procparam.Add("@Aggregate_Amt", (Convert.ToString(txtAggregateAmt.Text) == "") ? "0" : Convert.ToString(txtAggregateAmt.Text));
                Procparam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));
                grvDisPosedDtl.BindGridView("S3G_LAD_GetLASPanumInvDtl", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            }
            else
            {
                Procparam.Add("@LAS_ID", Convert.ToString(intLASID));
                Procparam.Add("@Asset_Flag", Convert.ToString(ddlAssetFlag.SelectedValue));
                grvDisPosedDtl.BindGridView("S3G_LAD_GetLASDSPDTL_Query", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            }

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvDisPosedDtl.Rows[0].Visible = grvDisPosedDtl.FooterRow.Visible = btnExportDspDtl.Enabled = false;
            }

            ucPageDispose.Navigation(intTotalRecords, ProPageNumRWSale, ProPageSizeRWSale);
            ucPageDispose.setPageSize(ProPageSizeRWSale);

            if (!bIsNewRow)
            {
                grvDisPosedDtl.FooterRow.Visible = btnExportDspDtl.Enabled = true;
                DataTable dtDsp = ((DataView)grvDisPosedDtl.DataSource).ToTable();

                Label lblTtlSaleAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrSaleAmt");
                Label lblTtlVatAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrVATAmt");
                Label lblTtlAddtVatAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrAddtVATAmt");
                Label lblTtlRVAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrRVAmt");
                Label lblTtlPLAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrPLAmt");

                Label lblFAvlbl_Inv_Amt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblFAvlbl_Inv_Amt");
                Label lblFAvlbQty = (Label)grvDisPosedDtl.FooterRow.FindControl("lblFAvlbQty");
                Label lblFDspSaleQty = (Label)grvDisPosedDtl.FooterRow.FindControl("lblFDspSaleQty");

                lblTtlSaleAmt.Text = Convert.ToString(dtDsp.Rows[0]["TtlSale_Price"]);
                lblTtlVatAmt.Text = Convert.ToString(dtDsp.Rows[0]["TtlVAT_Amount"]);
                lblTtlAddtVatAmt.Text = Convert.ToString(dtDsp.Rows[0]["TtlAdd_Vat_Amount"]);
                lblTtlRVAmt.Text = Convert.ToString(dtDsp.Rows[0]["TtlRVAmont"]);
                lblTtlPLAmt.Text = Convert.ToString(dtDsp.Rows[0]["TtlProfitLoss"]);

                lblFAvlbl_Inv_Amt.Text = Convert.ToString(dtDsp.Rows[0]["TtlAvlbl_Inv_Amt"]);
                lblFAvlbQty.Text = Convert.ToString(dtDsp.Rows[0]["TtlAvlb_Quantity"]);
                lblFDspSaleQty.Text = Convert.ToString(dtDsp.Rows[0]["TtlDsp_Quantity"]);

                //FunPriCalcDspAmt();
            }

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblSalePgingErrMsg.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblSalePgingErrMsg.InnerText = ex.Message;
        }
    }

    private void FunPriCalcDspAmt()
    {
        try
        {
            FunPriClearDict();
            dictParam.Add("@Option", "18");
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            dictParam.Add("@Mode", (Convert.ToString(strMode) == "" || Convert.ToString(strMode) == "M") ? "C" : Convert.ToString(strMode));

            DataTable dtDsp = Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);

            if (dtDsp != null && dtDsp.Rows.Count > 0)
            {
                Label lblTtlSaleAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrSaleAmt");
                Label lblTtlVatAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrVATAmt");
                Label lblTtlAddtVatAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrAddtVATAmt");
                Label lblTtlRVAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrRVAmt");
                Label lblTtlPLAmt = (Label)grvDisPosedDtl.FooterRow.FindControl("lblDspFtrPLAmt");

                lblTtlSaleAmt.Text = Convert.ToString(dtDsp.Rows[0]["Sale_Price"]);
                lblTtlVatAmt.Text = Convert.ToString(dtDsp.Rows[0]["VAT_Amount"]);
                lblTtlAddtVatAmt.Text = Convert.ToString(dtDsp.Rows[0]["ADD_VAT_Amount"]);
                lblTtlRVAmt.Text = Convert.ToString(dtDsp.Rows[0]["RV_Amount"]);
                lblTtlPLAmt.Text = Convert.ToString(dtDsp.Rows[0]["Profit_Loss"]);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriFileUpload(string FileNameFormat, string filepath, string Extension)
    {
        try
        {
            FunProImportExcelData_To_DBTable(filepath, Extension, "Yes");
            if (Flag == 1)
            {
                btnValidate.Enabled = true; btnUpload.Enabled = false;
                strAlert = strAlert.Replace("__ALERT__", "File uploaded successfully");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else
            {
                btnValidate.Enabled = false;
                strAlert = strAlert.Replace("__ALERT__", "File upload Failed");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected int FunProImportExcelData_To_DBTable(string FilePath, string Extension, string isHDR)
    {
        string strErrorMsg = "Please Correct The Validations <br/>";
        try
        {
            DataTable dtXLData = new DataTable();
            DataTable dtValidateXLData = new DataTable();
            DataTable ObjDtColumnList = (DataTable)ViewState["ColumnList"];

            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"]
                             .ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"]
                              .ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();

            cmdExcel.Connection = connExcel;
            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            SheetName = FunProGetSheetName(dtExcelSchema);
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "] WHERE [" + ObjDtColumnList.Rows[0][1].ToString() + "] IS NOT NULL";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dtXLData);

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "] WHERE [" + ObjDtColumnList.Rows[0][1].ToString() + "] IS NULL";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dtValidateXLData);

            connExcel.Close();

            if (dtValidateXLData.Rows.Count >= 0 && dtXLData.Rows.Count == 0)
            {
                strErrorMsg += ObjDtColumnList.Rows[0][1].ToString() + "  is Blank ;";
                lblUploadErrorMsg.InnerHtml = strErrorMsg;
                dtValidateXLData.Dispose();
                Flag = 2;
            }

            //dtXLData.AsEnumerable().ToList()
            // .ForEach(row =>
            // {
            //     var cellList = row.ItemArray.ToList();
            //     row.ItemArray = cellList.Select(x => x.ToString().Trim()).ToArray();
            // });

            if (Flag == 0)
                Flag = FunProValidateExcelColumnHeaderDetails(dtXLData, ObjDtColumnList, FilePath);

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            if (objException.Message.Contains("No value given for one or more required parameters."))
            {
                strErrorMsg = "Please Correct The Validations";
                lblUploadErrorMsg.InnerHtml = strErrorMsg + "Uploaded File Contains Empty Rows and Columns";
                lblUploadErrorMsg.InnerHtml = objException.Message;
            }
            else
            {
                strErrorMsg = "Please Correct The Validations";
                lblUploadErrorMsg.InnerHtml = strErrorMsg + "Uploaded File is in Invalid Format";
                lblUploadErrorMsg.InnerHtml = objException.Message;
            }
        }
        return Flag;
    }

    //method added to get the valid sheet name
    protected string FunProGetSheetName(DataTable dtExcelSchema)
    {
        try
        {
            foreach (DataRow row in dtExcelSchema.Rows)
            {
                if (!(row["TABLE_NAME"].ToString().Contains("FilterDatabase")
                    || row["TABLE_NAME"].ToString().EndsWith("_")))
                {
                    SheetName = row["TABLE_NAME"].ToString();
                    return SheetName;
                }
            }
            return SheetName;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected int FunProValidateExcelColumnHeaderDetails(DataTable XLData, DataTable DBXLcolumnHeaderNames, string FilePath)
    {
        StringBuilder StrIncorrectColumn = new StringBuilder();
        StringBuilder StrColumnLength = new StringBuilder();
        if (XLData.Columns.Count >= DBXLcolumnHeaderNames.Select("Is_ExcelColumn = 1").Length)
        {

            foreach (DataRow ObjDR in DBXLcolumnHeaderNames.Select("Is_ExcelColumn = 1"))
            {
                string StrExcelColumn = "";
                StrExcelColumn = Convert.ToString(ObjDR["Excel_ColumnName"]).Trim().ToUpper();
                if (!XLData.Columns.Contains(StrExcelColumn))
                {
                    if (StrIncorrectColumn.Length == 0)
                        StrIncorrectColumn.Append("Unmatched Column(s) : ");

                    StrIncorrectColumn.Append(ObjDR["Excel_ColumnName"].ToString() + "; <BR/>");
                }
                else
                {
                    int maxlength = 0;
                    int intColNo = XLData.Columns.IndexOf(StrExcelColumn);
                    XLData.Rows.OfType<DataRow>().ToList()
                        .ForEach(ss =>
                        {
                            maxlength = Convert.ToString(ss.ItemArray[intColNo]).Length > maxlength ?
                                Convert.ToString(ss.ItemArray[intColNo]).Length : maxlength;
                        });

                    if (!String.IsNullOrEmpty(ObjDR["Column_Length"].ToString()) && Convert.ToInt32(ObjDR["Column_Length"].ToString()) < maxlength)
                    {
                        if (StrColumnLength.Length == 0)
                            StrColumnLength.Append("Column Length Exceed : ");

                        StrColumnLength.Append(ObjDR["Excel_ColumnName"].ToString() +
                            "; Valid Length - " + ObjDR["Column_Length"].ToString() +
                            "; Available Length - " + maxlength + " <BR/>");
                    }
                }
            }
            if (StrIncorrectColumn.Length == 0 && StrColumnLength.Length == 0)
            {
                FunPriClearTempUpload();
                FunProBindDefaultColumns(XLData);
                string conn = FunPriGetConnectionString();
                SqlBulkCopy sbcd = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default);
                sbcd.DestinationTableName = "S3G_Tmp_LASDisposeDtl";
                foreach (DataRow row in DBXLcolumnHeaderNames.Rows)
                {
                    sbcd.ColumnMappings.Add(row["Excel_ColumnName"].ToString(), row["Db_ColumnName"].ToString());
                }
                sbcd.BatchSize = 1000;
                sbcd.WriteToServer(XLData);
                XLData.Clear();
                Flag = 1;
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
                lblUploadErrorMsg.InnerHtml = "File contains invalid column / data." + "<br/>" + StrIncorrectColumn.ToString() + "<br/>" + StrColumnLength.ToString();
            }
        }
        else
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid file For Selected Template...');", true);
        }
        return Flag;
    }

    protected void FunProBindDefaultColumns(DataTable XL)
    {
        try
        {
            Int32 count = 0;
            XL.Columns.Add(new DataColumn("Company_ID", typeof(Int32)));
            XL.Columns.Add(new DataColumn("Status_ID", typeof(Int32)));
            XL.Columns.Add(new DataColumn("USRID", typeof(Int32)));
            XL.Columns.Add(new DataColumn("Uploaded Date", typeof(DateTime)));
            foreach (DataRow row in XL.Rows)
            {
                row["Company_ID"] = Convert.ToString(intCompanyId);
                row["Status_ID"] = 1;
                row["USRID"] = Convert.ToString(intUserId);
                row["Uploaded Date"] = Convert.ToDateTime(DateTime.Now);
                count = count + 1;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearTempUpload()
    {
        try
        {
            FunPriClearDict();
            dictParam.Add("@Option", "19");
            dictParam.Add("@User_ID", Convert.ToString(intUserId));

            Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearDict()
    {
        try
        {
            if (dictParam != null)
                dictParam.Clear();
            else
                dictParam = new Dictionary<string, string>();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClrDspInvtl()
    {
        try
        {
            grvDisPosedDtl.DataSource = null;
            grvDisPosedDtl.DataBind();
            ucPageDispose.Visible = btnExportDspDtl.Enabled = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    #endregion

    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
            textBox.Attributes.Add("onpaste", "return false");
        }
    }

    #region "WEB METHODS"

    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.CompanyId));
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@Prefix", prefixText);
        //Commented and Modified on 03Jan2015 for OPC Customization starts
        //Procparam.Add("@Type", "3");
        Procparam.Add("@Type", (Convert.ToInt32(obj_Page.ddllastype.SelectedValue) == 2) ? "3" : "4");
        //Commented and Modified on 03Jan2015 for OPC Customization Ends
        Procparam.Add("@Is_Closed", "2");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPLASLA_LAS_AGT", Procparam));
        return suggetions.ToArray();
    }

    private void LoadLANs(int FLAG)
    {
        //{
        //    if (SelectedLASType == 1)
        //        ddlSLANNo.Items.Clear();
        //    else if (SelectedLASType == 2)
        //        ddlTLANNo.Items.Clear();
        //    else if (SelectedLASType == 3)
        //        ddlDLANNo.Items.Clear();

        //    Dictionary<string, string> ProcParams = new Dictionary<string, string>();
        //    if (SelectedLASType == 1 && ddlPAN.Items.Count > 1)
        //        ProcParams.Add("@PANum", ddlPAN.SelectedItem.Text.Trim());
        //    else if (SelectedLASType == 2 && ddlTPAN.Items.Count > 1)
        //        ProcParams.Add("@PANum", ddlTPAN.SelectedItem.Text.Trim());

        //    if (SelectedLASType == 1 && ddlSAN.Items.Count > 1)
        //        ProcParams.Add("@SANum", ddlSAN.SelectedItem.Text.Trim());
        //    else if (SelectedLASType == 2 && ddlTSAN.Items.Count > 1)
        //        ProcParams.Add("@SANum", ddlTSAN.SelectedItem.Text.Trim());
        //    else if (SelectedLASType == 2 && ddlTSAN.SelectedValue == "0")
        //        ProcParams.Add("@SANum", ddlTPAN.SelectedItem.Text.Trim() + "DUMMY");



        //    ProcParams.Add("@Company_ID", CompanyId);
        //    ProcParams.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        //    ProcParams.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));

        //    ProcParams.Add("@FLAG", FLAG.ToString());
        //    if (SelectedLASType == 1)
        //        ddlSLANNo.BindDataTable(SPNames.S3G_LOANAD_GetLeaseAssets, ProcParams, true, new string[] { "Asset_Id", "Lease_Asset_No" });
        //    else if (SelectedLASType == 2)
        //        ddlTLANNo.BindDataTable(SPNames.S3G_LOANAD_GetLeaseAssets, ProcParams, true, new string[] { "Asset_Id", "Lease_Asset_No" });
        //    else if (SelectedLASType == 3)
        //        ddlDLANNo.BindDataTable(SPNames.S3G_LOANAD_GetLeaseAssets, ProcParams, true, new string[] { "Asset_Id", "Lease_Asset_No" });

        //}
        //ddlDLANNo.Clear();
        //ddlTLANNo.Clear();

    }
    [System.Web.Services.WebMethod]
    public static string[] GetDocNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        //if (obj_Page.SelectedLASType == 1 && obj_Page.ddlPAN.SelectedValue != "0")
        //    Procparam.Add("@PANum", obj_Page.ddlPAN.SelectedText.Trim());
        //else if (obj_Page.SelectedLASType == 2 && obj_Page.ddlTPAN.SelectedValue != "0")
        //    Procparam.Add("@PANum", obj_Page.ddlTPAN.SelectedText.Trim());

        //else if (obj_Page.SelectedLASType == 2 && obj_Page.ddlTSAN.Items.Count > 1)
        //    Procparam.Add("@SANum", obj_Page.ddlTSAN.SelectedItem.Text.Trim());
        //else if (obj_Page.SelectedLASType == 2 && obj_Page.ddlTSAN.SelectedValue == "0")
        //    Procparam.Add("@SANum", obj_Page.ddlTPAN.SelectedText.Trim() + "DUMMY");

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.CompanyId));
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@Prefix", prefixText);

        Procparam.Add("@FLAG", obj_Page.hdnFlag.Value);
        //if (obj_Page.SelectedLASType == 1)
        //   suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetLeaseAssets_AGT",Procparam));
        //else if (obj_Page.SelectedLASType == 2)
        //    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetLeaseAssets_AGT", Procparam));
        //else if (obj_Page.SelectedLASType == 3)
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetLeaseAssets_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        //Procparam.Clear();
        //Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        //Procparam.Add("@Type", "GEN");
        //Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        //Procparam.Add("@Program_Id", "62");
        //Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        //Procparam.Add("@Is_Active", "1");
        //Procparam.Add("@PrefixText", prefixText);
        //suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        Procparam.Add("@option", "22");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyId));
        Procparam.Add("@PrefixText", Convert.ToString(prefixText));
        if (Convert.ToInt32(obj_Page.ddlEntitytype.SelectedValue) == 1)
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ViewState["CustomerID"]));
        if (Convert.ToInt32(obj_Page.ddlEntitytype.SelectedValue) > 1)
            Procparam.Add("@Vendor_ID", Convert.ToString(obj_Page.ViewState["CustomerID"]));

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Option", "3");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Option", "2");

        //if (Convert.ToInt64(obj_Page.ddlLesseeName.SelectedValue) > 0 && Convert.ToString(obj_Page.ddlLesseeName.SelectedText) != "")
        //    Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ddlLesseeName.SelectedValue));

        if (Convert.ToString(obj_Page.hdnLessee.Value) != "")
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.hdnLessee.Value));

        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendorList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Option", "8");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Option", "5");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetTypeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Option", "6");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetSubTypeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Option", "7");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLATNoList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Option", "9");
        if (Convert.ToInt64(obj_Page.ddlmapsrchLessee.SelectedValue) > 0 && Convert.ToString(obj_Page.ddlmapsrchLessee.SelectedText) != "")
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ddlmapsrchLessee.SelectedValue));
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSrchTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Option", "14");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }
    #endregion

    #region "Hide"

    //protected void chkDspAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox chkDspAll = (CheckBox)gvMapInvoiceDtl.HeaderRow.FindControl("chkDspAll");
    //        string strXML = "<Root>";

    //        for (Int32 i = 0; i < gvMapInvoiceDtl.Rows.Count; i++)
    //        {
    //            Label lblmapInvoiceID = (Label)gvMapInvoiceDtl.Rows[i].FindControl("lblmapInvoiceID");
    //            CheckBox ChkSelectInvoice = (CheckBox)gvMapInvoiceDtl.Rows[i].FindControl("ChkSelectInvoice");
    //            ChkSelectInvoice.Checked = chkDspAll.Checked;
    //            strXML = strXML + " <Details INVOICE_ID = '" + Convert.ToString(lblmapInvoiceID.Text) + "' />";
    //        }

    //        strXML = strXML + "</Root>";

    //        if (dictParam != null)
    //            dictParam.Clear();
    //        else
    //            dictParam = new Dictionary<string, string>();

    //        dictParam.Add("@Option", "16");
    //        dictParam.Add("@Is_Checked", (chkDspAll.Checked == true) ? "1" : "0");
    //        dictParam.Add("@XML_Invoice", Convert.ToString(strXML));
    //        dictParam.Add("@Mode", (Convert.ToString(strMode) == "") ? "C" : Convert.ToString(strMode));
    //        dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
    //        dictParam.Add("@User_ID", Convert.ToString(intUserId));
    //        Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", dictParam);
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    #endregion
    private void FunPubGetQrCode(string ReferenceNumber)
    {
        try
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ReferenceNumber, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);

            qrCodeImage.Save(Server.MapPath(".") + "\\PDF Files\\LASQRCode.png");
        }
        catch (Exception ex)
        {

        }
    }
}
