using System.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.IO;
using System.Configuration;
using S3GBusEntity.LoanAdmin;
using System.Globalization;
using System.Web.Services;
using System.Text;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ServiceProcess;
using System.Diagnostics;
using Microsoft.Win32;
using System.Linq;
using System.Drawing.Printing;
using System.IO.Compression;


public partial class LoanAdmin_S3G_Loanad_CashFlowPrint : ApplyThemeForProject
{
    public static LoanAdmin_S3G_Loanad_CashFlowPrint obj_Page;
    ContractMgtServicesReference.ContractMgtServicesClient objCashFlwPrt_Client;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LoanAd_CashFlowPrintDataTable objS3G_LoanAd_CashFlowPrintDataTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LoanAd_CashFlowPrintRow objS3G_LoanAd_CashFlowPrintRow = null;
    int intUserId;
    int intCompanyId;
    int intErrorCode = 0;
    long intCashFlwId;
    int strCashFlw_id;
    SerializationMode ObjSerMode = SerializationMode.Binary;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    DataTable dt = new DataTable();
    DataTable dtaccounts = new DataTable();
    string strDateFormat = string.Empty;
    Dictionary<string, string> Procparam;
    string strKey = "Generate";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3G_Loanad_CashFlowPrint.aspx";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3G_Loanad_CashFlowPrint.aspx';";

    #region Paging Config
    PagingValues ObjPaging = new PagingValues();
    int intNoofSearch = 5;
    ArrayList arrSearchVal = new ArrayList(1);
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

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Paging Config
        arrSearchVal = new ArrayList(intNoofSearch);
        ProPageNumRW = 1;
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        if (txtPageSize.Text != "")
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        else
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

        PageAssignValue obj = new PageAssignValue(this.AssignValue);
        ucCustomPaging.callback = obj;
        ucCustomPaging.ProPageNumRW = ProPageNumRW;
        ucCustomPaging.ProPageSizeRW = ProPageSizeRW;
        #endregion

        obj_Page = this;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        CalendarExtenderPostingDateFrom.Format = strDateFormat;                       // assigning the first textbox with the End date
        CalendarExtenderPostingDateTo.Format = strDateFormat;                       // assigning the first textbox with the start date
        CalExtenderDueDate.Format = strDateFormat;
        /* Changed Date Control start - 30-Nov-2012 */
        //txtStartDate.Attributes.Add("readonly", "readonly");
        //txtEndDate.Attributes.Add("readonly", "readonly");
        txtPostingDateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtPostingDateFrom.ClientID + "','" + strDateFormat + "',true,  false);");
        txtPostingDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtPostingDateTo.ClientID + "','" + strDateFormat + "',true,  false);");
        txtDueDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDueDate.ClientID + "','" + strDateFormat + "',null,null);");
        //txtPostingDateFrom.Attributes.Add("readonly", "readonly");
        //txtPostingDateTo.Attributes.Add("readonly", "readonly");
        if (!IsPostBack)
        {
            Session["AutoSuggestCompanyID"] = intCompanyId.ToString();
            pnlrs.Visible = false;
            FunPriLoadCashFlow();
            drpStatus_SelectedIndexChanged(sender, e);
        }
    }

    #region [Button Click Events]
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        objCashFlwPrt_Client = new ContractMgtServicesReference.ContractMgtServicesClient();
        dtaccounts = (DataTable)ViewState["dtAccounts"];
        DataTable dtAccSt;
        string TrancheName;
        dtAccSt = new DataTable();
        dtAccSt.Columns.Add("TrancheName");
        //dtAccSt.Columns.Add("CashFlowID");
        int Check = 0;
        if (dtaccounts.Rows.Count > 0)
        {
            foreach (GridViewRow gvrow in grvCashflowPrnt.Rows)
            {
                CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount");
                Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");
                Label lblStatus = (Label)gvrow.Cells[0].FindControl("lblStatus");
                Label lblTranche = (Label)gvrow.Cells[0].FindControl("lblTranche");
                if (chkSelectAccount.Checked)
                {
                    DataRow dr = dtAccSt.NewRow();
                    TrancheName = lblRS_Number.Text.ToString();
                    dr["TrancheName"] = TrancheName;
                    //dr["CashFlowID"] = lblCashFlowID.Text.ToString();
                    dtAccSt.Rows.Add(dr);
                    dtAccSt.AcceptChanges();
                }

                if ((chkSelectAccount.Checked && lblTranche.Text == "0")&&(drpStatus.SelectedValue=="1"))
                {
                    Utility.FunShowAlertMsg(this, "Tranche is not created, please create Tranche to proceed");
                    return;
                }

                if ((chkSelectAccount.Checked && lblStatus.Text == "Processed") || (chkSelectAccount.Checked && lblStatus.Text == "Under Processing"))
                {
                    Utility.FunShowAlertMsg(this, "Already Processed/Processing account(s) cannot be generated");
                    return;
                }
                if (chkSelectAccount.Checked && lblStatus.Text == "Pending")
                {
                    Check += 1;
                    DataRow dr = dtAccSt.NewRow();
                    TrancheName = lblRS_Number.Text.ToString();
                    dr["TrancheName"] = TrancheName;
                    //dr["CashFlowID"] = lblCashFlowID.Text.ToString();
                    dtAccSt.Rows.Add(dr);
                    dtAccSt.AcceptChanges();
                }
            }
            ViewState["dtAccSt"] = dtAccSt;
            if (grvCashflowPrnt.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "No account(s) to generate");
                return;
            }
            if (Check == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast one account to generate");
                return;
            }
        }

        objS3G_LoanAd_CashFlowPrintDataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LoanAd_CashFlowPrintDataTable();
        objS3G_LoanAd_CashFlowPrintRow = objS3G_LoanAd_CashFlowPrintDataTable.NewS3G_LoanAd_CashFlowPrintRow();
        objS3G_LoanAd_CashFlowPrintRow.Company_ID = intCompanyId;
        objS3G_LoanAd_CashFlowPrintRow.User_Id = intUserId;

        if (txtPostingDateFrom.Text != "")
            objS3G_LoanAd_CashFlowPrintRow.StartDate = Utility.StringToDate(txtPostingDateFrom.Text);
        if (txtPostingDateTo.Text != "")
            objS3G_LoanAd_CashFlowPrintRow.EndDate = Utility.StringToDate(txtPostingDateTo.Text);

        objS3G_LoanAd_CashFlowPrintRow.Due_Date = Utility.StringToDate(txtDueDate.Text);

        objS3G_LoanAd_CashFlowPrintRow.XmlAccCashFlow = dtAccSt.FunPubFormXml();
        objS3G_LoanAd_CashFlowPrintDataTable.AddS3G_LoanAd_CashFlowPrintRow(objS3G_LoanAd_CashFlowPrintRow);

        string strErrorMsg = string.Empty;

        int iErrorCode = objCashFlwPrt_Client.FunPubGenerateCashFlowPrint(out intCashFlwId, out strErrorMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LoanAd_CashFlowPrintDataTable, ObjSerMode));
        switch (iErrorCode)
        {
            case 0:
                strAlert = "Other Cash Flow Print generated successfully";
                strAlert += @"\n\nWould you like to generate one more Cash Flow?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
                strRedirectPage = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                break;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtPostingDateFrom.Clear();
        txtPostingDateTo.Clear();
        drpStatus.SelectedValue = "-1";
        ddlCustName.Clear();
        ddlRSNo.Clear();
        ddlTrancheNo.Clear();
        ddlLocation.Clear();
        drpStatus_SelectedIndexChanged(sender, e);
        pnlrs.Visible = false;
        txtInvoice.Text = txtDueDate.Text = hdnInvoice.Value = String.Empty;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in grvCashflowPrnt.Rows)
        {
            CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount");
            Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");
            Label lblStatus = (Label)gvrow.Cells[0].FindControl("lblStatus");
            if ((chkSelectAccount.Checked && lblStatus.Text == "Pending") || (chkSelectAccount.Checked && lblStatus.Text == "Under Processing"))
            {
                Utility.FunShowAlertMsg(this, "Pending/Processing account(s) cannot be printed");
                return;
            }
        }
        try
        {
            GetPDFFiles();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
            dt = Utility.GetDefaultData("S3G_LAD_GETDOCPATH", Procparam);
            ViewState["strnewFile"] = dt.Rows[0]["Document_Path"].ToString();

            Procparam = new Dictionary<string, string>();

            if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText != "")
                Procparam.Add("@PA_SA_REF_ID", ddlRSNo.SelectedValue.ToString());

            if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText != "")
            {
                dt = Utility.GetDefaultData("S3G_LAD_Tranche_Exists", Procparam);
                if (dt.Rows[0]["Tranche_Exist"].ToString() == "0")
                {
                    Utility.FunShowAlertMsg(this, "Tranche is not created, please create Tranche to proceed");
                    return;
                }
            }

            FunPriBindGrid();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region [Functions Defined]
    protected void FunPriLoadCashFlow()
    {
        DataSet ds = new DataSet();
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_Id", intCompanyId.ToString());
        Procparam.Add("@LOB_ID", "3");
        ds = Utility.GetDataset("S3G_LAD_CASHFLOWFLAGS", Procparam);
        MultiSelect.DataSource = ds.Tables[0];
        MultiSelect.DataTextField = "TEXT";
        MultiSelect.DataValueField = "ID";
        MultiSelect.DataBind();
    }

    private void GetPDFFiles()
    {
        try
        {
            int nCnt = 0;
            dtaccounts = (DataTable)ViewState["dtaccounts"];
            DataTable dtprint;
            int nDigi_Flag = 0;
            string panum;
            dtprint = new DataTable();
            dtprint.Columns.Add("tranche_name");
            if (dtaccounts.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in grvCashflowPrnt.Rows)
                {
                    CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount");
                    Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");
                    Label lblDigi_Flag = (Label)gvrow.Cells[0].FindControl("lblDigi_Flag");
                    if (chkSelectAccount.Checked)
                    {
                        nCnt += 1;
                        DataRow dr = dtprint.NewRow();
                        panum = lblRS_Number.Text.ToString();

                        if (lblDigi_Flag.Text == "1")
                            nDigi_Flag = 1;

                        dr["tranche_name"] = panum;
                        dtprint.Rows.Add(dr);
                        dtprint.AcceptChanges();
                    }
                }
                if (nCnt == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atlease one account to print");
                    return;
                }
                ViewState["dtprint"] = dtprint;
            }
            DataTable dtN = (DataTable)ViewState["dtprint"];
            string strnewfile = ViewState["strnewFile"].ToString();
            List<String> list = new List<String>();
            string GetCashFlow = MultiSelect.SelectedValue.ToString();

            foreach (DataRow drow in dtN.Rows)
            {
                if (MultiSelect.SelectedText != "--Select--")
                {
                    string[] GetIndCashFlow = GetCashFlow.Split(',');
                    for (int i = 0; i <= GetIndCashFlow.Length - 1; i++)
                    {
                        string strtranche = "/" + drow[0].ToString() + "_" + GetIndCashFlow[i].ToString() + ".pdf";
                        string GetFilePath = strnewfile + strtranche;
                        //strnewfile = strnewfile + strtranche;
                        FileInfo fl = new FileInfo(GetFilePath);
                        if (fl.Exists == true)
                        {
                            list.Add(GetFilePath);
                        }
                    }
                }
                else
                {
                    if (ddlLocation.SelectedValue != "0")
                    {
                        string[] str = Directory.GetFiles(ViewState["strnewFile"].ToString(), "*_" + drow[0].ToString() + "*");

                        for (int i = 0; i <= str.Length - 1; i++)
                        {
                            list.Add(str[i].ToString());
                        }
                    }
                    else
                    {
                        string[] str = Directory.GetFiles(ViewState["strnewFile"].ToString(), drow[0].ToString() + "*");

                        for (int i = 0; i <= str.Length - 1; i++)
                        {
                            list.Add(str[i].ToString());
                        }
                    }
                }
            }
            string[] GetAllFiles = list.ToArray();
            if (Convert.ToInt32(GetAllFiles.Length.ToString()) > 0)
            {
                CombineMultiplePDFs(GetAllFiles, strnewfile, nDigi_Flag, "RIGHT");
            }
        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }
    }

    private void GetPDFTrnacheFiles(string strtranche, int nDigi_Flag)
    {
        try
        {
            string strnewfile = ViewState["strnewFile"].ToString();
            strnewfile = strnewfile.Replace("\\", "/");

            string outfile = string.Empty;
            if (MultiSelect.SelectedText == "--Select--")
            {
                string[] str = Directory.GetFiles(strnewfile, "*" + strtranche + "*");
                if (Convert.ToInt32(str.Length.ToString()) != 0)
                {
                    CombineMultiplePDFs(str, strnewfile, nDigi_Flag, "RIGHT");
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                    return;
                }
                //else
                //{
                //    strtranche = "/" + strtranche + ".pdf";
                //    strnewfile = strnewfile + strtranche;
                //    FileInfo fl = new FileInfo(strnewfile);
                //    if (fl.Exists == true)
                //    {
                //        string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strnewfile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
                //    }
                //    else
                //    {
                //        Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                //        return;
                //    }
                //}
            }
            else
            {
                string GetCashFlow = MultiSelect.SelectedValue.ToString();
                string[] GetIndCashFlow = GetCashFlow.Split(',');
                List<String> list = new List<String>();
                for (int i = 0; i <= GetIndCashFlow.Length - 1; i++)
                {
                    string GetTranche = "";
                    string GetNewFile = "";
                    list.Add(strnewfile + "/" + strtranche + "_" + GetIndCashFlow[i].ToString() + ".pdf");
                    GetTranche = "/" + strtranche + "_" + GetIndCashFlow[i].ToString() + ".pdf";
                    GetNewFile = strnewfile + GetTranche;

                    FileInfo fl = new FileInfo(GetNewFile);
                    if (fl.Exists != true)
                    {
                        list.Remove(strnewfile + "/" + strtranche + "_" + GetIndCashFlow[i].ToString() + ".pdf");
                    }
                }
                string[] str = list.ToArray();
                CombineMultiplePDFs(str, strnewfile, nDigi_Flag, "RIGHT");
            }
        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }
    }

    public void CombineMultiplePDFs(string[] fileNames, string outFilePath, int Digi_Flag, string DigiPosition)
    {
        if (Digi_Flag == 1)
        {
            string InFile = outFilePath + "/Combine.pdf";

            if (!System.IO.Directory.Exists(outFilePath + "/Signed"))
            {
                System.IO.Directory.CreateDirectory(outFilePath + "/Signed");
            }

            string filePath = outFilePath + "/Signed.zip";

            //before creation of compressed folder,deleting it if exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();

            foreach (string fileName in fileNames)
            {

                string SignedFile = outFilePath + "/Signed/" + Path.GetFileName(fileName);
                //Digital signature is not required for SD Invoice
                if (fileName.Contains("_30.pdf"))
                {
                    File.Copy(fileName, SignedFile,true);
                }
                else
                {
                    ObjPDFSign.DigiPDFSign(fileName, SignedFile, "RIGHT");
                }
            }

            if (!File.Exists(filePath))
            {
                //creating a zip file from one folder to another folder
                ZipFile.CreateFromDirectory(outFilePath + "/Signed", filePath);

                //Delete The excel file which is created

                string[] str = Directory.GetFiles(outFilePath + "/Signed");
                foreach (string fileName in str)
                {
                    File.Delete(fileName);
                }

                if (Directory.Exists(outFilePath + "/Signed"))
                {
                    Directory.Delete(outFilePath + "/Signed");
                }
            }

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(filePath, false, 0);
            string strScipt1 = "";

            strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + filePath.Replace("\\", "/") +
                "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ZIP", strScipt1, true);

        }
        else
        {
            // step 1: creation of a document-object
            Document document = new Document();

            string InFile = outFilePath + "/Combine.pdf";
            string SignedFile = outFilePath + "/Signed.pdf";

            // step 2: we create a writer that listens to the document
            PdfCopy writer = new PdfCopy(document, new FileStream(InFile, FileMode.Create));
            if (writer == null)
            {
                return;
            }

            // step 3: we open the document
            document.Open();

            foreach (string fileName in fileNames)
            {
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(fileName);
                reader.ConsolidateNamedDestinations();

                // step 4: we add content
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }

                PRAcroForm form = reader.AcroForm;
                if (form != null)
                {
                    writer.CopyAcroForm(reader);
                }
                reader.Close();
            }

            // step 5: we close the document and writer
            writer.Close();
            document.Close();

            S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
            if (Digi_Flag == 1)
                ObjPDFSign.DigiPDFSign(InFile, SignedFile, "RIGHT");
            else
                SignedFile = InFile;

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
    }

    #region [Page Methods]
    private void FunPriBindGrid()
    {
        try
        {
            DataSet ds = new DataSet();
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            FunPriGetSearchValue();

            Procparam = new Dictionary<string, string>();
            if (txtPostingDateFrom.Text != "")
                Procparam.Add("@STARTDATE", Utility.StringToDate(txtPostingDateFrom.Text).ToString());
            if (txtPostingDateTo.Text != "")
                Procparam.Add("@ENDDATE", Utility.StringToDate(txtPostingDateTo.Text).ToString());
            Procparam.Add("@Status_ID", drpStatus.SelectedValue.ToString());

            if (ddlCustName.SelectedValue == "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@Customer_ID", "-1");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText == "")
                Procparam.Add("@Customer_ID", "0");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@Customer_ID", ddlCustName.SelectedValue.ToString());

            if (ddlRSNo.SelectedValue == "0" && ddlRSNo.SelectedText != "")
                Procparam.Add("@PA_SA_REF_ID", "-1");
            else if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText == "")
                Procparam.Add("@PA_SA_REF_ID", "0");
            else if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText != "")
                Procparam.Add("@PA_SA_REF_ID", ddlRSNo.SelectedValue.ToString());

            if (ddlTrancheNo.SelectedValue == "0" && ddlTrancheNo.SelectedText != "")
                Procparam.Add("@Tranche_ID", "-1");
            else if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText == "")
                Procparam.Add("@Tranche_ID", "0");
            else if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText != "")
                Procparam.Add("@Tranche_ID", ddlTrancheNo.SelectedValue.ToString());

            if (hdnInvoice.Value != "")
                Procparam.Add("@Invoice_No", hdnInvoice.Value);

            if (ddlLocation.SelectedValue != "0" && ddlLocation.SelectedText != "")
                Procparam.Add("@Location_Id", ddlLocation.SelectedValue.ToString());

            //if (drpStatus.SelectedValue == "0")
            //{
            //    string GetCashFlow = MultiSelect.SelectedValue.ToString();
            //    string[] GetIndCashFlow = GetCashFlow.Split(',');
            //    string strGetCashflow="";
            //    for (int i = 0; i <= GetIndCashFlow.Length - 1; i++)
            //    {
            //        strGetCashflow = strGetCashflow + "," + GetIndCashFlow[i].ToString();
            //    }

            //    Procparam.Add("@CashFlow_ID", strGetCashflow);
            //}
            //else
            //{
            //    Procparam.Add("@CashFlow_ID", "");
            //}

            grvCashflowPrnt.BindGridView("S3G_LOANAD_CASHFLWPRT_RSNO", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvCashflowPrnt.Rows[0].Visible = false;
            }
            DataView dv = (DataView)grvCashflowPrnt.DataSource;
            dt = dv.ToTable();
            ViewState["dtaccounts"] = dt;
            if (grvCashflowPrnt.Rows.Count > 0)
            {
                ViewState["dtAccounts"] = dt;
                pnlrs.Visible = true;
                divacc.Style.Add("display", "block");
                int nCnt = 0;
                foreach (GridViewRow gvrow in grvCashflowPrnt.Rows)
                {
                    Label lblStatus = (Label)gvrow.Cells[1].FindControl("lblStatus");
                    CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[2].FindControl("chkSelectAccount");
                    CheckBox chkAll = (CheckBox)grvCashflowPrnt.HeaderRow.FindControl("chkAll");
                    ImageButton imgbtnPrint = (ImageButton)gvrow.Cells[3].FindControl("imgbtnPrint");

                    if (lblStatus.Text == "Processed" || lblStatus.Text == "Under Processing")
                    {
                        chkSelectAccount.Checked = true;
                        nCnt += 1;
                    }
                    if (nCnt == grvCashflowPrnt.Rows.Count)
                    {
                        chkAll.Checked = true;
                        //btnGenerate.Visible = false;
                    }
                    else
                    {
                        chkAll.Checked = false;
                        //btnGenerate.Visible = true;
                    }
                }
            }
            else
            {
                pnlrs.Visible = true;
                grvCashflowPrnt.EmptyDataText = "No records Found";
                grvCashflowPrnt.DataBind();
            }
            FunPriSetSearchValue();
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            lblErrorMessage.Text = "";
            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    #region [Paging Methods For Grid]
    private void FunPriGetSearchValue()
    {
        arrSearchVal = grvCashflowPrnt.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        grvCashflowPrnt.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        grvCashflowPrnt.FunPriSetSearchValue(arrSearchVal);
    }
    #endregion
    #endregion

    #region [Web Methods]
    [System.Web.Services.WebMethod]
    public static string[] GetCustomerName(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            DataTable dtCommon = new DataTable();
            DataSet Ds = new DataSet();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCustomers", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetTrancheDetails", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetScheduleNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@LOB_ID", "3");
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_OTHERCASHFLOW_GETSCHEDULENO", Procparam));
        return suggetions.ToArray();
    }
    #endregion

    #region [Tool Events]
    protected void grvCashflowPrnt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgbtnPrint = (ImageButton)e.Row.FindControl("imgbtnPrint");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                if (lblStatus.Text != "Processed")
                {
                    imgbtnPrint.ImageUrl = "~/Images/pdf_disabled.png";
                    imgbtnPrint.CssClass = "styleGridEdit";
                    imgbtnPrint.Enabled = false;
                }
                else
                {  //if (drpStatus.SelectedValue == "0")
                    //{
                    //    string GetCashFlow = MultiSelect.SelectedValue.ToString();
                    //    string[] GetIndCashFlow = GetCashFlow.Split(',');
                    //    string strGetCashflow="";
                    //    for (int i = 0; i <= GetIndCashFlow.Length - 1; i++)
                    //    {
                    //        strGetCashflow = strGetCashflow + "," + GetIndCashFlow[i].ToString();
                    //    }

                    //    Procparam.Add("@CashFlow_ID", strGetCashflow);
                    //}
                    //else
                    //{
                    //    Procparam.Add("@CashFlow_ID", "");
                    //}
                    string GetCashFlow = MultiSelect.SelectedValue.ToString();
                    string[] GetIndCashFlow = GetCashFlow.Split(',');
                    string strGetCashflow = "";
                    for (int i = 0; i <= GetIndCashFlow.Length - 1; i++)
                    {
                        if (strGetCashflow == "")
                        {
                            strGetCashflow = GetIndCashFlow[i].ToString();
                        }
                        else
                        {
                            strGetCashflow = strGetCashflow + "," + GetIndCashFlow[i].ToString();
                        }
                    }

                    if (GetCashFlow != "0")
                    {
                        Label lblRS_Number = (Label)e.Row.FindControl("lblRS_Number");
                        Procparam = new Dictionary<string, string>();
                        Procparam.Add("@RS_Number", lblRS_Number.Text);
                        Procparam.Add("@Company_ID", intCompanyId.ToString());
                        Procparam.Add("@CashFlow_ID", strGetCashflow);
                        dt = Utility.GetDefaultData("S3G_LOANAD_GetCashflowFlag_Cashflowprint", Procparam);
                        if (dt.Rows[dt.Rows.Count - 1][0].ToString() == "TRUE")
                        {
                            imgbtnPrint.ImageUrl = "~/Images/pdf.png";
                            imgbtnPrint.CssClass = "stylePDFEnabled";
                            imgbtnPrint.Height = System.Web.UI.WebControls.Unit.Pixel(16);
                            imgbtnPrint.Width = System.Web.UI.WebControls.Unit.Pixel(16);
                            imgbtnPrint.Enabled = true;
                        }
                        else
                        {
                            imgbtnPrint.ImageUrl = "~/Images/pdf_disabled.png";
                            imgbtnPrint.CssClass = "stylePDFDisabled";
                            imgbtnPrint.Enabled = false;
                        }
                    }
                    else
                    {
                        imgbtnPrint.ImageUrl = "~/Images/pdf.png";
                        imgbtnPrint.CssClass = "stylePDFEnabled";
                        imgbtnPrint.Height = System.Web.UI.WebControls.Unit.Pixel(16);
                        imgbtnPrint.Width = System.Web.UI.WebControls.Unit.Pixel(16);
                        imgbtnPrint.Enabled = true;

                    }
                }
                CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectAccount");
                chkSelectAccount.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvCashflowPrnt.ClientID + "','chkAll','chkSelectAccount');");

            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvCashflowPrnt.ClientID + "',this,'chkSelectAccount');");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grvCashflowPrnt_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        GridViewRow Objrow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);

        Label lblDigi_Flag = (Label)Objrow.Cells[0].FindControl("lblDigi_Flag");
        int nDigi_Flag = 0;

        if (lblDigi_Flag.Text == "1")
            nDigi_Flag = 1;
        
        switch (e.CommandName.ToLower())
        {
            case "print":
                GetPDFTrnacheFiles(e.CommandArgument.ToString(), nDigi_Flag);
                break;
        }
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpStatus.SelectedValue == "0")
        {
            MultiSelect.TEnabled = true;
        }
        else
        {
            MultiSelect.TEnabled = false;
        }
        MultiSelect.DisableCheck = "0";
        FunPriLoadCashFlow();
        MultiSelect.SelectedText = "--Select--";
    }

    protected void txtPostingDateFrom_TextChanged(object sender, EventArgs e)
    {
        if ((!(string.IsNullOrEmpty(txtPostingDateFrom.Text))) &&
           (!(string.IsNullOrEmpty(txtPostingDateTo.Text)))) // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtPostingDateFrom.Text) > Utility.StringToDate(txtPostingDateTo.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "From Date should be lesser than or equal to the To Date");
                txtPostingDateTo.Text = "";
                return;
            }
        }
    }

    protected void txtPostingDateTo_TextChanged(object sender, EventArgs e)
    {
        if ((!(string.IsNullOrEmpty(txtPostingDateFrom.Text))) &&
           (!(string.IsNullOrEmpty(txtPostingDateTo.Text))))  // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtPostingDateFrom.Text) > Utility.StringToDate(txtPostingDateTo.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to the From Date");
                txtPostingDateTo.Text = "";
                return;
            }
        }
    }

    protected void txtInvoice_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnInvoice.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtInvoice.Text = string.Empty;
                hdnInvoice.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@Option", "8");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@option", "22");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyId.ToString()));
        Procparam.Add("@PrefixText", Convert.ToString(prefixText));
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }
    #endregion
}