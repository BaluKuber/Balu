
#region Page Header
//Module Name      :   LoanAdmin
//Screen Name      :   S3GLoanAdManualJournal_Add.aspx
//Created By       :   Kaliraj K
//Created Date     :   06-SEP-2010
//Purpose          :   To insert and update Manual Journal details

#endregion

using System;
using System.Web.UI;
using System.Data;
using System.Text;
using S3GBusEntity.LoanAdmin;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Resources;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class S3GLoanAdManualJournal_Add : ApplyThemeForProject
{
    #region initialization

    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjMJVClient;
    LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalDataTable ObjMJVClientDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;
    ClsSystemJournal ObjSysJournal = new ClsSystemJournal();

    int intErrCode = 0;
    int intMJVID = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int intRowId = 0;
    int intProgramId = 77;
    Decimal decDebit = 0;
    Decimal decCredit = 0;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    //Code end

    DataSet dsMJV = new DataSet();
    Dictionary<string, string> Procparam = null;

    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=MAJ";
    string strKey = "Insert";
    string strXMLMJVDetails = string.Empty;
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strMJVNumber = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdManualJournal_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=MAJ';";
    DataTable dtMJV = new DataTable();
    public static S3GLoanAdManualJournal_Add obj_Page;
    bool blnMLAApplicable = false;

    #endregion

    #region PageLoad
    protected new void Page_PreInit(object sender, EventArgs e) //transaction screen page init
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        //Add by chandru on 23/04/2012
        System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyID.ToString();
        //Add by chandru on 23/04/2012
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        txtMJVValueDate.Attributes.Add("readonly", "readonly");
        CalendarExtender1.Format = strDateFormat;

        txtMJVDate.Attributes.Add("readonly", "readonly");
        CalendarExtender2.Format = strDateFormat;

        if (Request.QueryString["Popup"] != null)
            btnCancel.Enabled = false;
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));

            if (fromTicket != null)
            {
                intMJVID = Convert.ToInt32(fromTicket.Name);
                //hdnMJVID.Value = intMJVID.ToString();
                strMode = Request.QueryString.Get("qsMode");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        //intMJVID = Convert.ToInt32(hdnMJVID.Value);
        //intMJVID = 5;
        //strMode = "M";

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end
        lblErrorMessage.Text = "";

        if (!IsPostBack)
        {
            //Validation Msgs from Resource File
            rfvLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
            //rfvBranch.ErrorMessage = ValidationMsgs.S3G_ValMsg_Branch;
            txtMJVDate.Text = DateTime.Now.ToString(strDateFormat);
            txtMJVValueDate.Text = DateTime.Now.ToString(strDateFormat);
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));


            if ((intMJVID > 0) && (strMode == "M"))
            {
                btnSave.Enabled = true;
                if (txtTally.Text == "0.00")
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;

                FunGetMJVDetails();
                FunPriDisableControls(1);
                FunPriBindMLAEntityType();
                if (grvManualJournal.FooterRow != null)
                {
                    Button btnAdd = (Button)grvManualJournal.FooterRow.FindControl("btnAdd");
                    btnAdd.Enabled = true;
                }


                // FunPriBindMLAEntityType();
            }
            else if ((intMJVID > 0) && (strMode == "Q"))
            {
                FunGetMJVDetails();
                FunPriDisableControls(-1);

            }
            else
            {
                FunPriBindLOBBranch();
                FunPriDisableControls(0);
                txtTotDebit.Text = Convert.ToDecimal("0").ToString(); //Convert.ToDecimal("0").ToString(Utility.SetSuffix());
                txtTotCredit.Text = Convert.ToDecimal("0").ToString(); //Convert.ToDecimal("0").ToString(Utility.SetSuffix());
            }
            //FunBindPostingFlag();
        }
        #region Common Session Values
        //Add by chandru on 23/04/2012
        if (ddlLOB.SelectedValue != "")
        {
            System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = ddlLOB.SelectedValue;
            System.Web.HttpContext.Current.Session["LOBAutoSuggestText"] = ddlLOB.SelectedItem.Text;
        }
        else
        {
            System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = null;
        }
        //Add by chandru on 23/04/2012
        #endregion
        //FunPubAddSummary(true);

    }

    #endregion

    #region Page Events

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    protected void btnViewRS_Click(object sender, EventArgs e)
    {
        if (ddlTranche.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this, "Select a Tranche");
            return;
        }

        if (ViewState["RSDetails"] != null)
        {
            grvRS.DataSource = (DataTable)ViewState["RSDetails"];
            grvRS.DataBind();
            moeRS.Show();
        }
    }
    
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        //////grvPrintManualJournal.ShowHeader = false;
        //////grvPrintManualJournal.ShowFooter = false;
        //////grvPrintManualJournal.AllowSorting = false;
        //////grvPrintManualJournal.AllowPaging = false;

        strProcName = "S3G_LoanAd_GetMJVDetails";
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@User_ID", intUserID.ToString());
        Procparam.Add("@MJV_No", intMJVID.ToString());
        dsMJV = Utility.GetTableValues(strProcName, Procparam);
        dsMJV.Tables[0].TableName = "DataTable1";
        dsMJV.Tables[1].TableName = "S3G_LoanAd_GetMJVDetails";
        /*
        DataTable dtExport = new DataTable();
        dtExport = dsMJV.Tables[1];
        //dtExport = (DataTable)ViewState["MJVDetails"];
        dtExport.Columns.Remove("EntityTypeValue");
        dtExport.Columns.Remove("PostingFlag");
        dtExport.Columns.Remove("Accounting_Flag");
        dtExport.Columns.Remove("Occurance_ID");
        dtExport.Columns.Remove("GLAcc");
        dtExport.Columns.Remove("Description");
        //dtExport.Columns.Remove("MJVRow_ID");
        dtExport.Columns["Dim2"].ColumnName = "Dim 2";
        if (ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.LastIndexOf("-")).Trim() != "OL")
        {
            dtExport.Columns.Remove("Dim 2");
        }
        dtExport.Columns["MJVRow_ID"].ColumnName = "Sl.No.";
        dtExport.Columns["MLA"].ColumnName = "Prime A/c No.";
        dtExport.Columns["SLA"].ColumnName = "Sub A/c No.";
        dtExport.Columns["EntityType"].ColumnName = "Entity Type";
        dtExport.Columns["PostingFlagDesc"].ColumnName = "Transaction Flag";
        dtExport.Columns["SLAcc"].ColumnName = "SL A/c";
        dtExport.Columns["GL_Description"].ColumnName = "GL A/c";
        grvPrintManualJournal.DataSource = dtExport;
        grvPrintManualJournal.DataBind();

        /////////grvManualJournal.FooterRow.Visible = false;
        ////////////grvManualJournal.ShowFooter = false;
        ////////////grvManualJournal.Columns[10].Visible=false;

        string attachment = "attachment; filename=MJV.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.xls";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        grvPrintManualJournal.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
        */
        Guid objGuid;
        objGuid = Guid.NewGuid();
        ReportDocument rpd = new ReportDocument();
        rpd.Load(Server.MapPath("rptMJV.rpt"));

        rpd.SetDataSource(dsMJV);
        //rpd.Subreports[0].SetDataSource(dset.Tables[0]);

        string strFileName = Server.MapPath(".") + "\\PDF Files\\MJV\\" + txtMJVNo.Text.Replace("/", "") + objGuid.ToString() + ".pdf";

        string strFolder = Server.MapPath(".") + "\\PDF Files\\MJV";

        //if (strFlag == "P")
        //{
        //    var outputStream = new MemoryStream();
        //    var pdfReader = new PdfReader(strFileName);
        //    var pdfStamper = new PdfStamper(pdfReader, outputStream);
        //    //Add the auto-print javascript
        //    var writer = pdfStamper.Writer;
        //    writer.AddJavaScript(GetAutoPrintJs());
        //    pdfStamper.Close();
        //    var content = outputStream.ToArray();
        //    outputStream.Close();
        //    Response.ContentType = "application/pdf";
        //    Response.BinaryWrite(content);
        //    Response.End();
        //    outputStream.Close();
        //    outputStream.Dispose();
        //}


        if (!(System.IO.Directory.Exists(strFolder)))
        {
            DirectoryInfo di = Directory.CreateDirectory(strFolder);

        }
        if (System.IO.Directory.Exists(strFolder))
        {
            string[] files = System.IO.Directory.GetFiles(strFolder);
            foreach (string s in files)
            {
                File.Delete(s);
            }

        }

        string strScipt = "";
        rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName); //ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "DeliveryInstruction");
        strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);


    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        FunPubAddSummary(false);
        ObjMJVClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();

        try
        {

            TimeSpan diff = Utility.StringToDate(txtMJVValueDate.Text).Subtract(Utility.StringToDate(txtMJVDate.Text));
            if (Math.Abs(diff.Days) > 30)
            {
                rfvCompareMJVDate.IsValid = false;
                return;
            }

            dtMJV = (DataTable)ViewState["MJVDetails"];

            if ((dtMJV.Rows[0]["EntityTypeValue"].ToString() == "") || (dtMJV.Rows[0]["EntityTypeValue"].ToString() == "0"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + ValidationMsgs.S3G_ValMsg_AddAtleastOneRec + "');", true);
                return;
            }

            ObjMJVClientDataTable = new LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalDataTable();
            LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalRow ObjMJVRow;

            ObjMJVRow = ObjMJVClientDataTable.NewS3G_LOANAD_ManualJournalRow();

            ObjMJVRow.MJV_ID = intMJVID;
            ObjMJVRow.Company_ID = intCompanyID;
            ObjMJVRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjMJVRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjMJVRow.CancelStatus = 0;
            //Here Approved Date Column as considered as MJV Date
            ObjMJVRow.Approved_Date = Utility.StringToDate(txtMJVDate.Text);
            ObjMJVRow.Value_Date = Utility.StringToDate(txtMJVValueDate.Text);
            ObjMJVRow.Created_By = intUserID;

            FunPriGenerateMJVXMLDet();

            ObjMJVRow.XMLMJVDetails = strXMLMJVDetails;

            if (grvInvoiceDetail != null && grvInvoiceDetail.Rows.Count > 0)
                ObjMJVRow.XML_InvoiceDtls = Utility.FunPubFormXml(grvInvoiceDetail, true, false);

            if (grvSalesInvoiceDetail != null && grvSalesInvoiceDetail.Rows.Count > 0)
                ObjMJVRow.XML_SalesInvoiceDtls = Utility.FunPubFormXml(grvSalesInvoiceDetail, true, false);

            if (ddlTranche.SelectedValue != "0" && ddlTranche.SelectedValue != "")
                ObjMJVRow.Tranche_Header_ID = Convert.ToInt32(ddlTranche.SelectedValue);

            if (ViewState["RSDetails"] != null && ((DataTable)ViewState["RSDetails"]).Rows.Count > 0)
                ObjMJVRow.XML_AccDtls = Utility.FunPubFormXml((DataTable)ViewState["RSDetails"], true);
            
            ObjMJVClientDataTable.AddS3G_LOANAD_ManualJournalRow(ObjMJVRow);

            //ObjMJVClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
            intErrCode = ObjMJVClient.FunPubCreateManualJournal(out strMJVNumber, SerMode, ClsPubSerialize.Serialize(ObjMJVClientDataTable, SerMode));

            if (intErrCode == 0)
            {
                if (intMJVID > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Manual Journal " + ValidationMsgs.S3G_ValMsg_Update, strRedirectPage);
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    return;
                }
                else
                {
                    //Utility.FunShowAlertMsg(this.Page, "Manual Journal " + ValidationMsgs.S3G_ValMsg_Save);

                    strAlert = "Manual Journal " + strMJVNumber + " " + ValidationMsgs.S3G_ValMsg_Save;
                    strAlert += @"\n" + ValidationMsgs.S3G_ValMsg_Next + " Manual Journal?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    return;
                }

            }
            else
            {
                if ((intErrCode == -1) || (intErrCode == -2) || (intErrCode == 50))
                    Utility.FunShowValidationMsg(this.Page, "", intErrCode);
                else
                    Utility.FunShowValidationMsg(this.Page, "LOANAD_MJV", intErrCode);
                return;
            }

            lblErrorMessage.Text = string.Empty;

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            // if (ObjMJVClient != null)
            ObjMJVClient.Close();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage, false);
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnCancelMJV_Click(object sender, EventArgs e)
    {
        ObjMJVClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();

        try
        {
            ObjMJVClientDataTable = new LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalDataTable();
            LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalRow ObjMJVRow;

            ObjMJVRow = ObjMJVClientDataTable.NewS3G_LOANAD_ManualJournalRow();
            ObjMJVRow.MJV_ID = intMJVID;
            ObjMJVRow.CancelStatus = 1;
            ObjMJVRow.Created_By = intUserID;
            ObjMJVRow.Company_ID = intCompanyID;
            ObjMJVRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjMJVRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjMJVRow.Approved_Date = Utility.StringToDate(txtMJVDate.Text);
            ObjMJVRow.Value_Date = Utility.StringToDate(txtMJVValueDate.Text);
            ObjMJVRow.XMLMJVDetails = "";

            ObjMJVClientDataTable.AddS3G_LOANAD_ManualJournalRow(ObjMJVRow);

            //ObjMJVClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
            intErrCode = ObjMJVClient.FunPubCreateManualJournal(out strMJVNumber, SerMode, ClsPubSerialize.Serialize(ObjMJVClientDataTable, SerMode));
            if (intErrCode == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Manual Journal Cancelled Successfully", strRedirectPage);
                return;
            }
            else
            {
                Utility.FunShowValidationMsg(this.Page, "LOANAD_MJV", intErrCode);
                return;
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //if (ObjMJVClient != null)
            ObjMJVClient.Close();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            string str = "'2','3','4'";
            FunPriClear(bClearList);
            FunPriBindGrid();
            FunPriDim2Manupulation();
            FunPriBindInvoiceGrid(null, 1);
            FunPriBindInvoiceGrid(null, 2);
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


    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

        Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //if (intMJVID == 0)
        //    Procparam.Add("@Is_Active", "1");
        //Procparam.Add("@Program_ID", intProgramId.ToString());
        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
        //Procparam.Add("@User_ID", intUserID.ToString());
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });

        //ddlBranch.SelectedIndex = 0;
        txtMJVValueDate.Text = DateTime.Now.ToString(strDateFormat);
        txtTally.Text = "";
        txtTotDebit.Text = Convert.ToDecimal("0").ToString();   //Convert.ToDecimal("0").ToString(Utility.SetSuffix());
        txtTotCredit.Text = Convert.ToDecimal("0").ToString(); //Convert.ToDecimal("0").ToString(Utility.SetSuffix());
        FunPriBindGrid();
        FunPriDim2Manupulation();
        btnSave.Enabled = false;
    }


    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtMJVValueDate.Text = DateTime.Now.ToString(strDateFormat);
        txtTally.Text = "";
        txtTotDebit.Text = Convert.ToDecimal("0").ToString(); //Convert.ToDecimal("0").ToString(Utility.SetSuffix());
        txtTotCredit.Text = Convert.ToDecimal("0").ToString(); //Convert.ToDecimal("0").ToString(Utility.SetSuffix());

        FunPriBindGrid();
        btnSave.Enabled = false;
    }

    protected void ddlTranche_SelectedIndexChanged(object sender, EventArgs e)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Tranche_Header_Id", ddlTranche.SelectedValue);
        DataTable dt = Utility.GetDefaultData("S3G_CLN_GetPANum_Tranche", Procparam);
        DataColumn col = new DataColumn("IsSelect", typeof(Int32));
        col.DefaultValue = 1;
        dt.Columns.Add(col);

        ViewState["RSDetails"] = dt;

        FunPriBindGrid();
        FunPriBindInvoiceGrid(null, 1);

        if (ddlTranche.SelectedValue != "0")
        {
            UserControls_S3GAutoSuggest ddlMLA = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");
            ddlMLA.Enabled = false;
        }
    }


    /*protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddlMLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlMLA");
        TextBox txtMLASearch = (TextBox)grvManualJournal.FooterRow.FindControl("txtMLASearch");

        DropDownList ddlSLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlSLA");
        //Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //Procparam.Add("@PANum", ddlMLA.SelectedValue);
        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);

        //if ((Utility.GetTableScalarValue("S3G_LoanAd_CheckValidPANum", Procparam)) == "0")
        //{
        //    ddlSLA.Items.Clear();
        //    hdnAccValid.Value = "0";
        //    txtMLA.Focus();
        //    Utility.FunShowAlertMsg(this.Page, "Prime Account Number Not Found");
        //    return;
        //}
        //hdnAccValid.Value = "1";

        DropDownList ddlDim2 = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlDim2");
        DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");

        RequiredFieldValidator rfvSAN = (RequiredFieldValidator)grvManualJournal.FooterRow.FindControl("rfvSLA");
        FunBindSLADim2(txtMLASearch, ddlSLA, ddlDim2, rfvSAN);
        if (ddlEntityType.SelectedValue == "1")//if customer then change selected value to -1
        {
            ddlEntityType.SelectedIndex = 0;
        }

        FunPubAddSummary(false);        
    }*/

    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Modified by chandru
        //DropDownList ddlMLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlMLA");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");
        //Modified by chandru
        DropDownList ddlSLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlSLA");
        DropDownList ddlDim2 = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlDim2");
        FunPriBindDim2(txtMLASearch, ddlSLA, ddlDim2);

        FunPubAddSummary(false);
    }

    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");

        //DropDownList ddlGLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        //DropDownList ddlSLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlSLAcc");
        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("ddlSLAcc");
        //DropDownList ddlMLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlMLA");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");
        DropDownList ddlPostingFlag = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlPostingFlag");
        RequiredFieldValidator rfvPostingFlag = (RequiredFieldValidator)grvManualJournal.FooterRow.FindControl("rfvPostingFlag");

        //Code added by saran on 16-Dec-2011 as per the observation start
        //RequiredFieldValidator rfvMLA = (RequiredFieldValidator)grvManualJournal.FooterRow.FindControl("rfvMLA");
        RequiredFieldValidator rfvSLA = (RequiredFieldValidator)grvManualJournal.FooterRow.FindControl("rfvSLA");
        //Code added by saran on 16-Dec-2011 as per the observation end
        Label lblDesc = (Label)grvManualJournal.FooterRow.FindControl("lblDescF");
        //ddlSLAcc.Items.Clear();
        lblDesc.Text = "";
        ddlGLAcc.SelectedValue = ddlSLAcc.SelectedValue = "0";
        ddlGLAcc.SelectedText = ddlSLAcc.SelectedText = string.Empty;
        //FunBindGLAcc(ddlEntityType, ddlGLAcc, ddlPostingFlag, txtMLASearch.Text);
        if (ddlEntityType.SelectedValue == "1")//Customer
        {
            ddlPostingFlag.Enabled = true;
            rfvPostingFlag.Enabled = true;
            //grvManualJournal.HeaderRow.Cells[4].Visible = true;
            //grvManualJournal.FooterRow.Cells[4].Visible = true;
            //Code added by saran on 16-Dec-2011 as per the observation start
            //rfvMLA.Enabled = true;
            //if (ddlSLAcc.Items.Count > 1)
            //{
            //    rfvSLA.Enabled = true;
            //}
            //Code added by saran on 16-Dec-2011 as per the observation end

            //getting customer code 

            if (!string.IsNullOrEmpty(txtMLASearch.SelectedText) || ddlTranche.SelectedValue != "0")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_Id", intCompanyID.ToString());
                if (ddlTranche.SelectedValue != "0")
                    Procparam.Add("@Tranche_Header_ID", ddlTranche.SelectedValue);
                if (txtMLASearch.SelectedValue != "0")
                    Procparam.Add("@PANum", txtMLASearch.SelectedValue.ToString());
                DataTable dt = Utility.GetDefaultData("S3G_ORG_GETCUST_GLSL", Procparam);
                if (dt.Rows.Count > 0)
                {
                    ddlGLAcc.SelectedValue = dt.Rows[0]["GL_Code"].ToString();
                    ddlGLAcc.SelectedText = dt.Rows[0]["GL_Desc"].ToString();
                    ddlSLAcc.SelectedValue = dt.Rows[0]["SL_Code"].ToString();
                    ddlSLAcc.SelectedText = dt.Rows[0]["SL_Code"].ToString();
                }
            }

        }
        else
        {
            ddlPostingFlag.SelectedIndex = 0;
            ddlPostingFlag.Enabled = false;
            rfvPostingFlag.Enabled = false;
            //grvManualJournal.HeaderRow.Cells[4].Visible = false;
            //grvManualJournal.FooterRow.Cells[4].Visible = false;

            //Code added by saran on 16-Dec-2011 as per the observation start
            //rfvMLA.Enabled = 
            rfvSLA.Enabled = false;
            //Code added by saran on 16-Dec-2011 as per the observation end


        }

        FunPubAddSummary(false);

    }

    protected void ddlGLAcc_SelectedIndexChanged(object sender, EventArgs e)
    {
        /* DropDownList ddlGLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlGLAcc");
         DropDownList ddlSLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlSLAcc");
         DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");
         //DropDownList ddlMLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlMLA");
         TextBox txtMLASearch = (TextBox)grvManualJournal.FooterRow.FindControl("txtMLASearch");
         RequiredFieldValidator rfvSLAcc = (RequiredFieldValidator)grvManualJournal.FooterRow.FindControl("rfvSLAcc");
         Label lblDesc = (Label)grvManualJournal.FooterRow.FindControl("lblDescF");

        

         if (ddlSLAcc.Items.Count > 1)
         {
             lblDesc.Text = "";
             rfvSLAcc.Enabled = true;
             ddlSLAcc.Enabled = true;
         }
         else
         {
             FunPriGetDesc(ddlGLAcc, ddlSLAcc, lblDesc, ddlEntityType.SelectedValue);
             rfvSLAcc.Enabled = false;
             ddlSLAcc.Enabled = false;
         }*/

        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("ddlSLAcc");
        DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");
        FunBindSLAcc(ddlEntityType, ddlGLAcc, ddlSLAcc, txtMLASearch.SelectedValue);

        FunPubAddSummary(false);
    }

    protected void ddlSLAcc_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlGLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        DropDownList ddlSLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlSLAcc");
        DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");
        Label lblDesc = (Label)grvManualJournal.FooterRow.FindControl("lblDescF");
        FunPriGetDesc(ddlGLAcc, ddlSLAcc, lblDesc, ddlEntityType.SelectedValue);

        FunPubAddSummary(false);
    }

    /*protected void ddlMLAhdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlMLAhdr = (DropDownList)sender;
        GridViewRow grvRow = (GridViewRow)ddlMLAhdr.Parent.Parent;

        //DropDownList ddlMLA = (DropDownList)grvRow.FindControl("ddlMLAhdr");
        TextBox txtMLASearchhdr = (TextBox)grvRow.FindControl("txtMLASearchhdr");
        DropDownList ddlSLA = (DropDownList)grvRow.FindControl("ddlSLAhdr");

        ////if (txtMLA.Text != "")
        ////{
        ////    Procparam = new Dictionary<string, string>();
        ////    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        ////    Procparam.Add("@PANum", ddlMLA.Text.Trim());
        ////    Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);

        ////    if ((Utility.GetTableScalarValue("S3G_LoanAd_CheckValidPANum", Procparam)) == "0")
        ////    {
        ////        ddlSLA.Items.Clear();
        ////        hdnAccValid.Value = "0";
        ////        ddlMLA.Focus();
        ////        Utility.FunShowAlertMsg(this.Page, "Prime Account Number Not Found");
        ////        return;
        ////    }
        ////    hdnAccValid.Value = "1";
        ////}

        DropDownList ddlDim2 = (DropDownList)grvRow.FindControl("ddlDim2Hdr");
        DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
        RequiredFieldValidator rfvSAN = (RequiredFieldValidator)grvRow.FindControl("rfvSLAhdr");
        FunBindSLADim2(txtMLASearchhdr, ddlSLA, ddlDim2, rfvSAN);

        if (ddlEntityType.SelectedValue == "1")//if customer then change selected value to -1
        {
            ddlEntityType.SelectedIndex = 0;
        }
    }*/

    protected void ddlSLAhdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlSLAhdr = (DropDownList)sender;
        GridViewRow grvRow = (GridViewRow)ddlSLAhdr.Parent.Parent;

        //TextBox txtMLA = (TextBox)grvRow.FindControl("txtMLAhdr");
        //DropDownList ddlMLA = (DropDownList)grvRow.FindControl("ddlMLAhdr");
        UserControls_S3GAutoSuggest txtMLASearchhdr = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearchhdr");
        DropDownList ddlSLA = (DropDownList)grvRow.FindControl("ddlSLAhdr");
        DropDownList ddlDim2 = (DropDownList)grvRow.FindControl("ddlDim2Hdr");
        FunPriBindDim2(txtMLASearchhdr, ddlSLA, ddlDim2);
    }
    //To Bind GLAcc list
    protected void ddlEntityTypeHdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlEntityhdr = (DropDownList)sender;
        GridViewRow grvRow = (GridViewRow)ddlEntityhdr.Parent.Parent;

        DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
        //DropDownList ddlMLA = (DropDownList)grvRow.FindControl("ddlMLAhdr");
        //TextBox txtMLASearchhdr = (TextBox)grvRow.FindControl("txtMLASearchhdr");

        //DropDownList ddlGLAcc = (DropDownList)grvRow.FindControl("ddlGLAccHdr");
        //DropDownList ddlSLAcc = (DropDownList)grvRow.FindControl("ddlSLAccHdr");

        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlGLAccHdr");
        UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlSLAccHdr");

        Label lblDesc = (Label)grvRow.FindControl("lblDescHdr");

        DropDownList ddlPostingFlag = (DropDownList)grvRow.FindControl("ddlPostingFlagHdr");
        RequiredFieldValidator rfvPostingFlag = (RequiredFieldValidator)grvRow.FindControl("rfvPostingFlagHdr");

        //ddlSLAcc.Items.Clear();
        lblDesc.Text = "";
        ddlGLAcc.SelectedValue = ddlSLAcc.SelectedValue = "0";
        ddlGLAcc.SelectedText = ddlSLAcc.SelectedText = string.Empty;
        //FunBindGLAcc(ddlEntityType, ddlGLAcc, ddlPostingFlag, txtMLASearchhdr.Text);

        UserControls_S3GAutoSuggest txtMLASearchhdr = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtMLASearchhdr");

        if (!string.IsNullOrEmpty(txtMLASearchhdr.SelectedText) && ddlEntityType.SelectedValue == "1")
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyID.ToString());
            if (ddlTranche.SelectedValue != "0")
                Procparam.Add("@Tranche_Header_ID", ddlTranche.SelectedValue);
            Procparam.Add("@PANum", txtMLASearchhdr.SelectedValue.ToString());
            DataTable dt = Utility.GetDefaultData("S3G_ORG_GETCUST_GLSL", Procparam);
            if (dt.Rows.Count > 0)
            {
                ddlGLAcc.SelectedValue = dt.Rows[0]["GL_Code"].ToString();
                ddlGLAcc.SelectedText = dt.Rows[0]["GL_Desc"].ToString();
                ddlSLAcc.SelectedValue = dt.Rows[0]["SL_Code"].ToString();
                ddlSLAcc.SelectedText = dt.Rows[0]["SL_Code"].ToString();
            }
        }


        if (ddlEntityType.SelectedValue == "1")
        {
            ddlPostingFlag.Enabled = true;
            rfvPostingFlag.Enabled = true;
            //grvManualJournal.Columns[4].Visible = true;
        }
        else
        {
            ddlPostingFlag.Enabled = false;
            rfvPostingFlag.Enabled = false;
            ddlPostingFlag.SelectedIndex = 0;
            //grvManualJournal.Columns[4].Visible = false;
        }


    }

    //To Bind SLAcc list
    protected void ddlGLAccHdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddlMLAhdr = (DropDownList)sender;
        ////TextBox ddlMLAhdr = (TextBox)sender;
        //GridViewRow grvRow = (GridViewRow)ddlMLAhdr.Parent.Parent;

        ////DropDownList ddlGLAcc = (DropDownList)grvRow.FindControl("ddlGLAccHdr");
        ////DropDownList ddlSLAcc = (DropDownList)grvRow.FindControl("ddlSLAccHdr");
        //UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlGLAccHdr");
        //UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlSLAccHdr");
        //DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
        ////DropDownList ddlMLA = (DropDownList)grvRow.FindControl("ddlMLAhdr");
        //TextBox txtMLASearch = (TextBox)grvRow.FindControl("txtMLASearchhdr");
        //RequiredFieldValidator rfvSLAcc = (RequiredFieldValidator)grvRow.FindControl("rfvSLAccHdr");
        //Label lblDesc = (Label)grvRow.FindControl("lblDescHdr");

        /*FunBindSLAcc(ddlEntityType, ddlGLAcc, ddlSLAcc, txtMLASearch.Text);

        if (ddlSLAcc.Items.Count > 1)
        {
            lblDesc.Text = "";
            rfvSLAcc.Enabled = true;
            ddlSLAcc.Enabled = true;
        }
        else
        {
            FunPriGetDesc(ddlGLAcc, ddlSLAcc, lblDesc, ddlEntityType.SelectedValue);
            rfvSLAcc.Enabled = false;
            ddlSLAcc.Enabled = false;
        }*/

        GridViewRow grvRow = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent);
        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlGLAccHdr");
        UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlSLAccHdr");
        DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
        UserControls_S3GAutoSuggest txtMLASearchhdr = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtMLASearchhdr");
        FunBindSLAcc(ddlEntityType, ddlGLAcc, ddlSLAcc, txtMLASearchhdr.SelectedValue);


    }

    protected void ddlSLAccHdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlMLAhdr = (DropDownList)sender;
        GridViewRow grvRow = (GridViewRow)ddlMLAhdr.Parent.Parent;

        //DropDownList ddlGLAcc = (DropDownList)grvRow.FindControl("ddlGLAccHdr");
        //DropDownList ddlSLAcc = (DropDownList)grvRow.FindControl("ddlSLAccHdr");
        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlGLAccHdr");
        UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlSLAccHdr");
        DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
        Label lblDesc = (Label)grvRow.FindControl("lblDescHdr");
        //FunPriGetDesc(ddlGLAcc, ddlSLAcc, lblDesc, ddlEntityType.SelectedValue);
    }

    //Code added by saran on 16-Dec-2011 as per the observation start


    //To bind GL Code as per the Transaction flag.
    protected void ddlPostingFlag_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");
        //DropDownList ddlGLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        //DropDownList ddlMLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlMLA");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");
        DropDownList ddlPostingFlag = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlPostingFlag");
        //FunBindGLAcc(ddlEntityType, ddlGLAcc, ddlPostingFlag, txtMLASearch.Text);

        FunPubAddSummary(false);
    }

    //To bind GL Code as per the Transaction flag.
    protected void ddlPostingFlagHdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddlPostingFlagHdr = (DropDownList)sender;
        //GridViewRow grvRow = (GridViewRow)ddlPostingFlagHdr.Parent.Parent;

        //DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
        ////DropDownList ddlMLA = (DropDownList)grvRow.FindControl("ddlMLAhdr");
        //TextBox txtMLASearch = (TextBox)grvRow.FindControl("txtMLASearchhdr");
        //DropDownList ddlGLAcc = (DropDownList)grvRow.FindControl("ddlGLAccHdr");
        //DropDownList ddlPostingFlag = (DropDownList)grvRow.FindControl("ddlPostingFlagHdr");
        //FunBindGLAcc(ddlEntityType, ddlGLAcc, ddlPostingFlag, txtMLASearch.Text);


    }

    //Code added by saran on 16-Dec-2011 as per the observation end
    protected void grvManualJournal_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtDebit = (TextBox)e.Row.FindControl("txtDebit");
            TextBox txtCredit = (TextBox)e.Row.FindControl("txtCredit");
            Button btnGridSave = (Button)e.Row.FindControl("btnAdd");
            RequiredFieldValidator rfvCredit = (RequiredFieldValidator)e.Row.FindControl("rfvCredit");
            RequiredFieldValidator rfvDebit = (RequiredFieldValidator)e.Row.FindControl("rfvDebit");

            S3GSession ObjS3GSession = new S3GSession();

            txtDebit.SetDecimalPrefixSuffix(8, ObjS3GSession.ProGpsSuffixRW, true, "Debit value");
            txtCredit.SetDecimalPrefixSuffix(8, ObjS3GSession.ProGpsSuffixRW, true, "Credit value");

            ////if (strMode == "M")
            ////{
            ////    if (btnGridSave.Text == "Update")
            ////    {
            ////        btnGridSave.Enabled = true;
            ////    }
            ////    else if (btnGridSave.Text == "Add")
            ////    {
            ////        btnGridSave.Enabled = true;
            ////    }
            ////    else
            ////    {
            ////        btnGridSave.Enabled = false;
            ////    }
            ////}

            txtDebit.Attributes.Add("onkeyup", "fnDiableCredit('" + txtDebit.ClientID + "','" + txtCredit.ClientID + "','D')");
            txtCredit.Attributes.Add("onkeyup", "fnDiableCredit('" + txtDebit.ClientID + "','" + txtCredit.ClientID + "','C')");

            if ((txtDebit.Text != "") && (Convert.ToDecimal(txtDebit.Text) == 0))
            {
                txtDebit.Text = "";

            }
            if ((txtCredit.Text != "") && (Convert.ToDecimal(txtCredit.Text) == 0))
            {
                txtCredit.Text = "";

            }
            btnGridSave.Attributes.Add("onclick", "return FnValidate('" + txtDebit.ClientID + "','" + txtCredit.ClientID + "','" + rfvCredit.ClientID + "','" + rfvDebit.ClientID + "')");
        }

        //////intGridRowCount=0;
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            Label lblDebit = (Label)e.Row.FindControl("lblDebit");
            Label lblCredit = (Label)e.Row.FindControl("lblCredit");
            if (lblDebit != null)
            {
                if ((lblDebit.Text != "") && (Convert.ToDecimal(lblDebit.Text) == 0))
                    lblDebit.Text = "";
            }
            if (lblCredit != null)
            {
                if ((lblCredit.Text != "") && (Convert.ToDecimal(lblCredit.Text) == 0))
                    lblCredit.Text = "";
            }

        }
        //intGridRowCount++;
    }

    protected void grvPoInvoiceDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("chkSelectAll");
                chkSelectAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvPoInvoiceDetails.ClientID + "',this,'chkSelect');");
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
     

    ////private void FunPriDebit()
    ////{
    ////    TextBox txtDebit = (TextBox)grvManualJournal.FooterRow.FindControl("txtDebit");
    ////    TextBox txtCredit = (TextBox)grvManualJournal.FooterRow.FindControl("txtCredit");
    ////    txtDebit.Attributes.Add("onblur", "fnDiableCredit('" + txtDebit.ClientID + "','" + txtCredit.ClientID + "')");
    ////    txtCredit.Attributes.Add("onblur", "fnDiableCredit('" + txtDebit.ClientID + "','" + txtCredit.ClientID + "')");
    ////}

    protected void lnkbtnRemove_Click(object sender, EventArgs e)
    {
        dtMJV = (DataTable)ViewState["MJVDetails"];
        LinkButton lnkbtnRemove = (LinkButton)sender;
        GridViewRow grvRow = (GridViewRow)lnkbtnRemove.Parent.Parent;

        if (strMode == "M")
        {
            if (dtMJV.Rows.Count == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "There should be atleast one row in the grid");
                //FunPriBindGrid();
                return;
            }
        }
        dtMJV.Rows.RemoveAt(grvRow.RowIndex);
        if (dtMJV.Rows.Count == 0)
        {
            FunPriBindGrid();
        }
        else
        {
            FunPriBindGridDetails(dtMJV);
            //grvManualJournal.DataSource = dtMJV;
            //grvManualJournal.DataBind();
            FunPriBindMLAEntityType();
        }

        FunPriCalTotal();
        ViewState["MJVDetails"] = dtMJV;
    }

    protected void grvManualJournal_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            grvManualJournal.EditIndex = e.NewEditIndex;
            int intRowId = Convert.ToInt32(grvManualJournal.DataKeys[e.NewEditIndex].Value.ToString()) - 1;

            dtMJV = (DataTable)ViewState["MJVDetails"];
            FunPriBindGridDetails(dtMJV);

            GridViewRow grvRow = grvManualJournal.Rows[intRowId];

            //Modified by Chandru
            //DropDownList ddlMLA = (DropDownList)grvRow.FindControl("ddlMLAhdr");            
            UserControls_S3GAutoSuggest txtMLASearchhdr = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtMLASearchhdr");
            //Modified by Chandru
            DropDownList ddlSLA = (DropDownList)grvRow.FindControl("ddlSLAhdr");
            DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
            //DropDownList ddlGLAcc = (DropDownList)grvRow.FindControl("ddlGLAccHdr");
            //DropDownList ddlSLAcc = (DropDownList)grvRow.FindControl("ddlSLAccHdr");
            UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlGLAccHdr");
            UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlSLAccHdr");
            DropDownList ddlDim2 = (DropDownList)grvRow.FindControl("ddlDim2Hdr");
            DropDownList ddlPostingFlag = (DropDownList)grvRow.FindControl("ddlPostingFlagHdr");
            TextBox txtDebit = (TextBox)grvRow.FindControl("txtDebitHdr");
            TextBox txtCredit = (TextBox)grvRow.FindControl("txtCreditHdr");
            TextBox txtRemarks = (TextBox)grvRow.FindControl("txtRemarksHdr");
            Label lblDesc = (Label)grvRow.FindControl("lblDescHdr");

            LinkButton btnGridSave = (LinkButton)grvRow.FindControl("lnkUpdate");

            //RequiredFieldValidator rfvMLA = (RequiredFieldValidator)grvRow.FindControl("rfvMLAhdr");
            if (!string.IsNullOrEmpty(ddlEntityType.SelectedValue.Trim()))
            {
                if (ddlEntityType.SelectedValue.Trim() != "50")
                {
                    if (ddlEntityType.SelectedValue == "1" && (string.IsNullOrEmpty(txtMLASearchhdr.SelectedText.Trim().ToString())))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Enter and Select the Rental Schedule No.");
                        return;
                    }
                }
            }

            RequiredFieldValidator rfvSLA = (RequiredFieldValidator)grvRow.FindControl("rfvSLAhdr");
            RequiredFieldValidator rfvSLAcc = (RequiredFieldValidator)grvRow.FindControl("rfvSLAccHdr");
            RequiredFieldValidator rfvCredit = (RequiredFieldValidator)grvRow.FindControl("rfvCreditHdr");
            RequiredFieldValidator rfvDebit = (RequiredFieldValidator)grvRow.FindControl("rfvDebitHdr");
            RequiredFieldValidator rfvDim2 = (RequiredFieldValidator)grvRow.FindControl("rfvDim2Hdr");
            RequiredFieldValidator rfvPostingFlag = (RequiredFieldValidator)grvRow.FindControl("rfvPostingFlagHdr");

            FunPriBindMLAHeaderEntityType(ddlEntityType, ddlPostingFlag);

            //Modified by Chandru
            //ddlMLA.SelectedValue = dtMJV.Rows[intRowId]["MLA"].ToString();

            //else
            //{
            //    txtMLASearchhdr.SelectedValue = ' ';
            //    txtMLASearchhdr.SelectedText=
            //}
            //Modified by Chandru
            ddlEntityType.SelectedValue = dtMJV.Rows[intRowId]["EntityTypeValue"].ToString();
            //if (ddlEntityType.SelectedValue != "50")
            //{            
            txtMLASearchhdr.SelectedValue = dtMJV.Rows[intRowId]["MLA"].ToString();
            txtMLASearchhdr.SelectedText = dtMJV.Rows[intRowId]["MLA_Desc"].ToString();

            //Modified by Chandru
            //FunBindSLADim2(ddlMLA, ddlSLA, ddlDim2, rfvSLA);

            //Commented for OPC Start

            ////FunBindSLADim2(txtMLASearchhdr, ddlSLA, ddlDim2, rfvSLA);

            //End

            //Modified by Chandru

            ddlSLA.SelectedValue = dtMJV.Rows[intRowId]["SLA"].ToString();

            if (ddlSLA.Items.Count > 1)
            {
                FunPriBindDim2(txtMLASearchhdr, ddlSLA, ddlDim2);
            }
            if (ddlDim2.Items.Count > 1)
            {
                ddlDim2.SelectedValue = dtMJV.Rows[intRowId]["Dim2"].ToString();
                rfvDim2.Enabled = true;
            }
            else
            {
                rfvDim2.Enabled = false;
            }
            //}

            //FunBindGLAcc(ddlEntityType, ddlGLAcc, ddlPostingFlag, txtMLASearchhdr.Text);

            ddlGLAcc.SelectedValue = dtMJV.Rows[intRowId]["GLAcc"].ToString();
            ddlGLAcc.SelectedText = dtMJV.Rows[intRowId]["GL_Description"].ToString();

            FunBindSLAcc(ddlEntityType, ddlGLAcc, ddlSLAcc, txtMLASearchhdr.SelectedValue);

            ddlSLAcc.SelectedValue = dtMJV.Rows[intRowId]["SLAcc"].ToString();
            ddlSLAcc.SelectedText = dtMJV.Rows[intRowId]["SLAcc"].ToString();

            ddlPostingFlag.SelectedValue = dtMJV.Rows[intRowId]["PostingFlag"].ToString();
            //FunBindPostingFlag(ddlPostingFlag);
            if (ddlEntityType.SelectedValue == "1" && !string.IsNullOrEmpty(ddlEntityType.SelectedValue.Trim()))
            {
                ddlPostingFlag.Enabled = true;
                rfvPostingFlag.Enabled = true;
                //code added by saran on 16-Dec-2011 as per the observation start
                //if (ddlSLAcc.Items.Count > 1)
                //{
                //    rfvSLAcc.Enabled = true;
                //}

                //rfvMLA.Enabled = true;
                //code added by saran on 16-Dec-2011 as per the observation end
            }
            else
            {
                ddlPostingFlag.Enabled = false;
                rfvPostingFlag.Enabled = false;
                //code added by saran on 16-Dec-2011 as per the observation start
                rfvSLA.Enabled = false;
                //rfvMLA.Enabled = false;
                //code added by saran on 16-Dec-2011 as per the observation end
            }

            //if (ddlSLAcc.Items.Count > 1)
            //{
            //    ddlSLAcc.Enabled = true;
            //    rfvSLAcc.Enabled = true;
            //}
            //else
            //{
            //    ddlSLAcc.Enabled = false;
            //    rfvSLAcc.Enabled = false;
            //}

            txtDebit.SetDecimalPrefixSuffix(8, 2, true, "Debit value");
            txtCredit.SetDecimalPrefixSuffix(8, 2, true, "Credit value");

            txtDebit.Attributes.Add("onkeyup", "fnDiableCredit('" + txtDebit.ClientID + "','" + txtCredit.ClientID + "','D')");
            txtCredit.Attributes.Add("onkeyup", "fnDiableCredit('" + txtDebit.ClientID + "','" + txtCredit.ClientID + "','C')");
            btnGridSave.Attributes.Add("onclick", "return FnValidate('" + txtDebit.ClientID + "','" + txtCredit.ClientID + "','" + rfvCredit.ClientID + "','" + rfvDebit.ClientID + "')");

            txtDebit.Text = dtMJV.Rows[intRowId]["Debit"].ToString();

            txtDebit.Text = txtDebit.Text == "0" ? "" : txtDebit.Text;

            txtCredit.Text = dtMJV.Rows[intRowId]["Credit"].ToString();
            txtCredit.Text = txtCredit.Text == "0" ? "" : txtCredit.Text;

            txtRemarks.Text = dtMJV.Rows[intRowId]["Remarks"].ToString();
            lblDesc.Text = dtMJV.Rows[intRowId]["GL_Description"].ToString();


            grvManualJournal.FooterRow.Visible = false;
            //Modified By Thangam M on 12/Apr/2012 to fix UAT - MJV_014
            btnSave.Enabled = false;
            //End here

            FunPubAddSummary(false);

            if (ddlTranche.SelectedValue != "0")
            {
                txtMLASearchhdr.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvManualJournal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string strFilterQuery;
            intRowId = e.RowIndex;



            GridViewRow grvRow = grvManualJournal.Rows[intRowId];

            //Modified by Chandru
            //DropDownList ddlMLA = (DropDownList)grvRow.FindControl("ddlMLAhdr");
            UserControls_S3GAutoSuggest txtMLASearchhdr = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtMLASearchhdr");
            //Modified by Chandru

            DropDownList ddlSLA = (DropDownList)grvRow.FindControl("ddlSLAhdr");
            DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
            //DropDownList ddlGLAcc = (DropDownList)grvRow.FindControl("ddlGLAccHdr");
            //DropDownList ddlSLAcc = (DropDownList)grvRow.FindControl("ddlSLAccHdr");
            UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlGLAccHdr");
            UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvRow.FindControl("ddlSLAccHdr");
            DropDownList ddlDim2 = (DropDownList)grvRow.FindControl("ddlDim2Hdr");
            DropDownList ddlPostingFlag = (DropDownList)grvRow.FindControl("ddlPostingFlagHdr");
            TextBox txtDebit = (TextBox)grvRow.FindControl("txtDebitHdr");
            TextBox txtCredit = (TextBox)grvRow.FindControl("txtCreditHdr");
            TextBox txtRemarks = (TextBox)grvRow.FindControl("txtRemarksHdr");
            Label lblDesc = (Label)grvRow.FindControl("lblDescHdr");

            LinkButton btnGridSave = (LinkButton)grvRow.FindControl("lnkUpdate");

            //RequiredFieldValidator rfvSLA = (RequiredFieldValidator)grvRow.FindControl("rfvSLAhdr");
            //RequiredFieldValidator rfvSLAcc = (RequiredFieldValidator)grvRow.FindControl("rfvSLAccHdr");
            //RequiredFieldValidator rfvCredit = (RequiredFieldValidator)grvRow.FindControl("rfvCreditHdr");
            //RequiredFieldValidator rfvDebit = (RequiredFieldValidator)grvRow.FindControl("rfvDebitHdr");

            if ((txtDebit.Text.Trim() != "") && (txtCredit.Text.Trim() != ""))
            {
                Utility.FunShowAlertMsg(this.Page, "Either debit or credit should be empty");
                return;
            }

            dtMJV = (DataTable)ViewState["MJVDetails"];

            DataRow[] drCheck = null;
            //string strDim2=string.Empty;

            string strSLA = ddlSLA.SelectedValue == "0" ? "" : ddlSLA.SelectedValue;
            string strSLAcc = ddlSLAcc.SelectedValue == "0" ? "" : ddlSLAcc.SelectedValue;
            //string strPostingFlag = ddlPostingFlag.SelectedValue == "0" ? "" : ddlPostingFlag.SelectedValue;
            if (!string.IsNullOrEmpty(ddlEntityType.SelectedValue.Trim()))
            {
                if (ddlEntityType.SelectedValue != "50")
                {
                    if (ddlEntityType.SelectedValue == "1")
                    {
                        if (string.IsNullOrEmpty(txtMLASearchhdr.SelectedText.ToString().Trim()))
                        {
                            Utility.FunShowAlertMsg(this.Page, "Enter and Select Rental Schedule No.");
                            txtMLASearchhdr.Focus();
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlGLAcc.SelectedText.Trim()))
                        {
                            Utility.FunShowAlertMsg(this.Page, "Enter and Select GL A/c");
                            txtMLASearchhdr.Focus();
                            return;
                        }
                    }
                    // strFilterQuery = "MLA='" + txtMLASearchhdr.SelectedValue.ToString() + "' AND EntityTypeValue='" + ddlEntityType.SelectedValue + "'";
                }
            }
            if (!string.IsNullOrEmpty(txtMLASearchhdr.SelectedText.ToString().Trim()))
            {
                strFilterQuery = "MLA='" + txtMLASearchhdr.SelectedValue.ToString() + "' AND EntityTypeValue='" + ddlEntityType.SelectedValue + "'";
            }
            else
            {
                txtMLASearchhdr.SelectedValue = string.Empty;
                strFilterQuery = "MLA='" + txtMLASearchhdr.SelectedValue.ToString() + "' AND EntityTypeValue='" + ddlEntityType.SelectedValue + "'";
            }

            strFilterQuery = strFilterQuery + " AND GLAcc='" + ddlGLAcc.SelectedValue + "'";
            if (strSLA != "")
            {
                strFilterQuery = strFilterQuery + " AND SLA='" + strSLA + "'";
            }
            if (strSLAcc != "")
            {
                strFilterQuery = strFilterQuery + " AND SLAcc='" + strSLAcc + "'";
            }

            //Commenetd for OPC Starts

            //if (Utility.FunGetValueBySplit(ddlLOB.SelectedItem.Text) == "OL")
            //{
            //    strFilterQuery = strFilterQuery + " AND Dim2='" + ddlDim2.SelectedValue + "'";
            //}

            //End

            strFilterQuery = strFilterQuery + " AND MJVRow_ID <> '" + (intRowId + 1).ToString() + "'";
            drCheck = dtMJV.Select(strFilterQuery);
            if (drCheck.Length > 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Same record already exists in the grid");
                return;
            }

            //Modified by Chandru
            //dtMJV.Rows[intRowId]["MLA"] = ddlMLA.SelectedValue == "0" ? "" : ddlMLA.SelectedValue;

            //Modified by Chandru            
            dtMJV.Rows[intRowId]["SLA"] = ddlSLA.SelectedValue == "0" ? "" : ddlSLA.SelectedValue;
            dtMJV.Rows[intRowId]["EntityTypeValue"] = ddlEntityType.SelectedValue;
            dtMJV.Rows[intRowId]["EntityType"] = ddlEntityType.SelectedItem.Text;
            if (ddlDim2.SelectedItem != null)
            {
                dtMJV.Rows[intRowId]["Dim2"] = ddlDim2.SelectedItem.Value;
            }
            dtMJV.Rows[intRowId]["GLAcc"] = ddlGLAcc.SelectedValue;
            dtMJV.Rows[intRowId]["GL_Description"] = ddlGLAcc.SelectedText.Trim();
            dtMJV.Rows[intRowId]["Description"] = lblDesc.Text;
            dtMJV.Rows[intRowId]["SLAcc"] = ddlSLAcc.SelectedValue == "0" ? "" : ddlSLAcc.SelectedValue;
            dtMJV.Rows[intRowId]["SLAcc_Desc"] = ddlSLAcc.SelectedValue == "0" ? "" : ddlSLAcc.SelectedText;
            dtMJV.Rows[intRowId]["Debit"] = txtDebit.Text == "" ? "0" : txtDebit.Text;
            dtMJV.Rows[intRowId]["Credit"] = txtCredit.Text == "" ? "0" : txtCredit.Text;
            dtMJV.Rows[intRowId]["Remarks"] = txtRemarks.Text;
            dtMJV.Rows[intRowId]["PostingFlag"] = ddlPostingFlag.SelectedValue;
            dtMJV.Rows[intRowId]["PostingFlagDesc"] = ddlPostingFlag.SelectedValue == "0" ? "" : ddlPostingFlag.SelectedItem.Text;
            dtMJV.Rows[intRowId]["MLA"] = txtMLASearchhdr.SelectedValue.ToString();
            dtMJV.Rows[intRowId]["MLA_Desc"] = txtMLASearchhdr.SelectedText.Trim();

            grvManualJournal.EditIndex = -1;
            FunPriBindGridDetails(dtMJV);

            FunPriCalTotal();
            grvManualJournal.FooterRow.Visible = true;
            FunPriBindMLAEntityType();
            ViewState["MJVDetails"] = dtMJV;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvManualJournal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grvManualJournal.EditIndex = -1;

            dtMJV = (DataTable)ViewState["MJVDetails"];
            //grvManualJournal.DataSource = dtMJV;
            //grvManualJournal.DataBind();
            FunPriBindGridDetails(dtMJV);
            FunPriBindMLAEntityType();
            grvManualJournal.FooterRow.Visible = true;
            FunPriCalTotal();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvManualJournal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        grvManualJournal.EditIndex = -1;
        dtMJV = (DataTable)ViewState["MJVDetails"];

        if (strMode == "M")
        {
            if (dtMJV.Rows.Count == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "There should be atleast one row in the grid");
                return;
            }
        }
        dtMJV.Rows.RemoveAt(e.RowIndex);
        if (dtMJV.Rows.Count == 0)
        {
            FunPriBindGrid();
        }
        else
        {
            //Reset Row_ID in DataTable
            for (int intCount = 0; intCount < dtMJV.Rows.Count; intCount++)
            {
                dtMJV.Rows[intCount]["MJVRow_ID"] = (intCount + 1).ToString();
            }
            FunPriBindGridDetails(dtMJV);
            //grvManualJournal.DataSource = dtMJV;
            //grvManualJournal.DataBind();


            FunPriBindMLAEntityType();
        }

        FunPriCalTotal();
        ViewState["MJVDetails"] = dtMJV;
        grvManualJournal.FooterRow.Visible = true;
    }

    protected void grvManualJournal_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        //Modified by chandru
        //DropDownList ddlMLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlMLA");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");

        //Modified by chandru

        DropDownList ddlSLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlSLA");
        DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");
        DropDownList ddlDim2 = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlDim2");
        //DropDownList ddlGLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        //DropDownList ddlSLAcc = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlSLAcc");
        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        UserControls_S3GAutoSuggest ddlSLAcc = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("ddlSLAcc");
        DropDownList ddlPostingFlag = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlPostingFlag");
        TextBox txtDebit = (TextBox)grvManualJournal.FooterRow.FindControl("txtDebit");
        TextBox txtCredit = (TextBox)grvManualJournal.FooterRow.FindControl("txtCredit");
        TextBox txtRemarks = (TextBox)grvManualJournal.FooterRow.FindControl("txtRemarks");
        Label lblDesc = (Label)grvManualJournal.FooterRow.FindControl("lblDescF");
        Button btnAdd = (Button)grvManualJournal.FooterRow.FindControl("btnAdd");
        RequiredFieldValidator rfvDim2 = (RequiredFieldValidator)grvManualJournal.FooterRow.FindControl("rfvDim2");
        rfvDim2.Enabled = false;
        if (!string.IsNullOrEmpty(ddlEntityType.SelectedValue.Trim()))
        {
            if (ddlEntityType.SelectedValue.Trim() != "50")
            {
                if (ddlEntityType.SelectedValue.Trim() == "1")
                {
                    if (string.IsNullOrEmpty(txtMLASearch.SelectedText.ToString().Trim()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Enter and Select the Rental Schedule No.");
                        txtMLASearch.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(ddlGLAcc.SelectedText.Trim().ToString()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Enter and Select the GL A/c");
                        ddlGLAcc.Focus();
                        return;
                    }

                }
            }
        }

        DataRow dr;

        if (hdnAccValid.Value == "0")
        {
            Utility.FunShowAlertMsg(this.Page, "Rental Schedule No Not Found");
            txtMLASearch.Focus();
            return;
        }

        if ((txtDebit.Text.Trim() != "") && (txtCredit.Text.Trim() != ""))
        {
            Utility.FunShowAlertMsg(this.Page, "Either debit or credit should be empty");
            return;
        }

        if (e.CommandName == "AddNew")
        {
            string strFilterQuery;
            dtMJV = (DataTable)ViewState["MJVDetails"];

            DataRow[] drCheck = null;
            string strSLA = ddlSLA.SelectedValue == "0" ? "" : ddlSLA.SelectedValue;
            string strSLAcc = ddlSLAcc.SelectedValue == "0" ? "" : ddlSLAcc.SelectedValue;
            //string strPostingFlag = ddlPostingFlag.SelectedValue == "0" ? "" : ddlPostingFlag.SelectedValue;
            if (txtMLASearch.SelectedText.Trim() != string.Empty)
            {
                strFilterQuery = "MLA='" + txtMLASearch.SelectedValue.ToString() + "' AND EntityTypeValue='" + ddlEntityType.SelectedValue + "'";
            }
            else
            {
                txtMLASearch.SelectedValue = string.Empty;
                strFilterQuery = "MLA='" + txtMLASearch.SelectedValue.ToString() + "' AND EntityTypeValue='" + ddlEntityType.SelectedValue + "'";
            }
            strFilterQuery = strFilterQuery + " AND GLAcc='" + ddlGLAcc.SelectedValue + "'";
            if (strSLA != "")
            {
                strFilterQuery = strFilterQuery + " AND SLA='" + strSLA + "'";
            }
            if (strSLAcc != "")
            {
                strFilterQuery = strFilterQuery + " AND SLAcc='" + strSLAcc + "'";
            }

            if (ddlPostingFlag.SelectedValue != string.Empty)
            {
                strFilterQuery = strFilterQuery + " AND PostingFlag='" + ddlPostingFlag.SelectedValue.ToString() + "'";
            }
            else
            {
                ddlPostingFlag.SelectedValue = string.Empty;
                strFilterQuery = strFilterQuery + " AND PostingFlag='" + ddlPostingFlag.SelectedValue.ToString() + "'";
            }
            

            //Commented for OPC
            //if (Utility.FunGetValueBySplit(ddlLOB.SelectedItem.Text) == "OL")
            //{
            //    rfvDim2.Enabled = true;
            //    strFilterQuery = strFilterQuery + " AND Dim2='" + ((Convert.ToInt32(ddlDim2.SelectedIndex) > 0) ? ddlDim2.SelectedValue : "") + "'";
            //}

            if (btnAdd.Text == "Add")
            {
                if (dtMJV.Rows[0]["EntityTypeValue"].ToString() == "" || dtMJV.Rows[0]["EntityTypeValue"].ToString() == "0")
                {
                    dtMJV.Rows[0].Delete();
                }

                drCheck = dtMJV.Select(strFilterQuery);

                if (drCheck.Length > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Same record already exists in the grid");
                    return;
                }
                dr = dtMJV.NewRow();

                dr["MJVRow_ID"] = Convert.ToString(dtMJV.Rows.Count + 1);
                dr["Occurance_ID"] = "0";

                if (txtMLASearch.SelectedValue != "0" && !string.IsNullOrEmpty(txtMLASearch.SelectedText.Trim()))
                {
                    dr["MLA"] = txtMLASearch.SelectedValue.ToString();
                    dr["MLA_Desc"] = txtMLASearch.SelectedText.Trim();
                }
                else
                {
                    dr["MLA"] =
                    dr["MLA_Desc"] = string.Empty;
                }

                dr["SLA"] = ddlSLA.SelectedValue == "0" ? "" : ddlSLA.SelectedValue;
                dr["EntityTypeValue"] = ddlEntityType.SelectedValue;
                dr["EntityType"] = ddlEntityType.SelectedItem.Text;
                if (ddlDim2.SelectedIndex > 0)
                    dr["Dim2"] = ddlDim2.SelectedItem.Value;
                dr["GLAcc"] = ddlGLAcc.SelectedValue;
                dr["GL_Description"] = ddlGLAcc.SelectedText;
                dr["SLAcc"] = ddlSLAcc.SelectedValue == "0" ? "" : ddlSLAcc.SelectedValue;
                dr["SLAcc_Desc"] = ddlSLAcc.SelectedValue == "0" ? "" : ddlSLAcc.SelectedText.Trim();
                dr["PostingFlag"] = ddlPostingFlag.SelectedValue;
                dr["PostingFlagDesc"] = ddlPostingFlag.SelectedValue == "0" ? "" : ddlPostingFlag.SelectedItem.Text;

                if (((txtDebit.Text.Trim()).ToString().Length) > 0) dr["Debit"] = txtDebit.Text;
                if (((txtCredit.Text.Trim()).ToString().Length) > 0) dr["Credit"] = txtCredit.Text;
                dr["Remarks"] = txtRemarks.Text;
                dtMJV.Rows.Add(dr);
            }

            FunPriBindGridDetails(dtMJV);

            FunPriCalTotal();

            FunPriBindMLAEntityType();
            ViewState["MJVDetails"] = dtMJV;
        }

    }
    #endregion

    #region Page Methods

    /// <summary>
    /// to bind LOB and Product details
    /// </summary>

    private void FunPriCalTotal()
    {

        for (int i = 0; i < dtMJV.Rows.Count; i++)
        {
            if (dtMJV.Rows[i]["Debit"].ToString() != "")
                decDebit += Convert.ToDecimal(dtMJV.Rows[i]["Debit"]);
            if (dtMJV.Rows[i]["Credit"].ToString() != "")
                decCredit += Convert.ToDecimal(dtMJV.Rows[i]["Credit"]);
        }

        //txtTotDebit.Text = decDebit.ToString();
        //txtTotCredit.Text = decCredit.ToString();

        txtTotDebit.Text = decDebit.ToString();//Convert.ToDecimal(decDebit.ToString()).ToString(Utility.SetSuffix());
        txtTotCredit.Text = decCredit.ToString();// Convert.ToDecimal(decCredit.ToString()).ToString(Utility.SetSuffix());

        txtTally.Text = (decDebit - decCredit).ToString();
        if (Convert.ToDecimal(txtTally.Text) == 0)
            btnSave.Enabled = true;
        else
            btnSave.Enabled = false;

        FunPubAddSummary(false);

    }

    private void FunPriGetDesc(DropDownList ddlGLAcc, DropDownList ddlSLAcc, Label lblDesc, string strEnityType)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
            Procparam.Add("@GL_Code", ddlGLAcc.SelectedValue);
            Procparam.Add("@EntityType", strEnityType);
            if (ddlSLAcc.Items.Count > 1)
            {
                Procparam.Add("@SL_Code", ddlSLAcc.SelectedValue);
            }
            lblDesc.Text = Utility.GetTableScalarValue("S3G_LoanAd_GetGLDescription", Procparam);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriBindLOBBranch()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Program_ID", intProgramId.ToString());
            if (intMJVID == 0)
                Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", intUserID.ToString());
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            if (ddlLOB.Items != null && ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
                ddlLOB.ClearDropDownList();
            }

            if (Request.QueryString.Get("qsMode") != "C")
            {
                //Branch Removed By Shibu DDL Changed To Autosuggestion Control 
                //Procparam = new Dictionary<string, string>();
                //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                //if (intMJVID == 0)
                //    Procparam.Add("@Is_Active", "1");
                //Procparam.Add("@Program_ID", intProgramId.ToString());
                //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
                //Procparam.Add("@User_ID", intUserID.ToString());
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            }
            FunPriBindGrid();

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    //Modified by chandru
    //private void FunPriBindDim2(DropDownList ddlMLA, DropDownList ddlSLA, DropDownList ddlDim2)
    private void FunPriBindDim2(UserControls_S3GAutoSuggest txtMLASearch, DropDownList ddlSLA, DropDownList ddlDim2)
    {
        try
        {
            if (ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.LastIndexOf("-")).Trim() == "OL")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                //Procparam.Add("@PANUM", ddlMLA.SelectedValue);
                if (!string.IsNullOrEmpty(txtMLASearch.SelectedText.Trim()))
                    Procparam.Add("@PANUM", txtMLASearch.SelectedValue);
                if (ddlSLA.SelectedIndex > 0)
                {
                    Procparam.Add("@SANUM", ddlSLA.SelectedValue);
                }
                ddlDim2.BindDataTable("S3G_LOANAD_GetAssetDetails_List", Procparam, new string[] { "Asset_Number", "Asset_Code" });
            }
            else
            {
                ddlDim2.Items.Insert(0, new ListItem("0", "0"));
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriBindGrid()
    {

        dtMJV = new DataTable();
        DataRow dr;

        dtMJV.Columns.Add("MJVRow_ID");
        dtMJV.Columns.Add("Occurance_ID");
        dtMJV.Columns.Add("MLA");
        dtMJV.Columns.Add("MLA_Desc");
        dtMJV.Columns.Add("SLA");
        dtMJV.Columns.Add("EntityType");
        dtMJV.Columns.Add("EntityTypeValue");
        dtMJV.Columns.Add("Dim2");
        dtMJV.Columns.Add("GLAcc");
        dtMJV.Columns.Add("Description");
        dtMJV.Columns.Add("GL_Description");
        dtMJV.Columns.Add("SLAcc");
        dtMJV.Columns.Add("SLAcc_Desc");
        dtMJV.Columns.Add("Debit");
        dtMJV.Columns.Add("Credit");
        dtMJV.Columns.Add("Remarks");
        dtMJV.Columns.Add("PostingFlag");
        dtMJV.Columns.Add("PostingFlagDesc");

        dr = dtMJV.NewRow();
        dtMJV.Rows.Add(dr);

        if (intMJVID == 0)
        {
            ViewState["MJVDetails"] = dtMJV;

            //grvManualJournal.DataSource = dtMJV;
            //grvManualJournal.DataBind();
            FunPriBindGridDetails(dtMJV);

            grvManualJournal.Rows[0].Visible = false;

        }

        FunPriBindMLAEntityType();


    }

    private void FunPriBindGridDetails(DataTable dtMJV)
    {
        if ((dtMJV.Rows[0]["EntityTypeValue"].ToString() == "") || (dtMJV.Rows[0]["EntityTypeValue"].ToString() == "0"))
        {
            grvManualJournal.Columns[0].Visible = true;
            //grvManualJournal.HeaderRow.Cells[0].Visible = true;
            //grvManualJournal.FooterRow.Cells[0].Visible = true;
        }
        else
        {
            grvManualJournal.Columns[0].Visible = false;
            //grvManualJournal.HeaderRow.Cells[0].Visible = false;
            //grvManualJournal.FooterRow.Cells[0].Visible = false;
        }
        grvManualJournal.DataSource = dtMJV;
        grvManualJournal.DataBind();

        if (ddlTranche.SelectedValue != "0")
        {
            UserControls_S3GAutoSuggest ddlMLA = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");
            ddlMLA.Enabled = false;
        }

        /*
    string strFilterQuery = " PostingFlag <> '0'";
    DataRow[] drCheck = null;
    drCheck = dtMJV.Select(strFilterQuery);
    if (drCheck.Length > 0)
    {
        grvManualJournal.HeaderRow.Cells[4].Visible = true;
    }
    else
    {
        grvManualJournal.HeaderRow.Cells[4].Visible = false;
    }
    grvManualJournal.FooterRow.Cells[4].Visible=false;
    */
    }

    public void FunPubAddSummary(bool FromPage)
    {
        //if (grvManualJournal.FooterRow != null && grvManualJournal.FooterRow.Visible) 
        //{
        //GridViewRow FRow = grvManualJournal.FooterRow;

        //Changed by Thangam for UAT bug fixing OL
        GridViewRow FRow = grvManualJournal.FooterRow;
        int intCells = FRow.Cells.Count;

        GridViewRow FNRow;
        FNRow = new GridViewRow(FRow.RowIndex + 1, -1, FRow.RowType, FRow.RowState);

        if (grvManualJournal.Columns[0].Visible && ddlLOB.SelectedItem.Text.Contains("OL"))
        {
            intCells = 11;
        }
        else if (grvManualJournal.Columns[0].Visible)
        {
            intCells = 10;
        }
        else
        {
            intCells = 9;
        }

        for (int i = 0; i <= intCells - 1; i++)
        {
            TableCell clNew = new TableCell();
            //if (!grvManualJournal.Columns[i].Visible)
            //{
            //    clNew.Visible = false;
            //}               
            FNRow.Cells.Add(clNew);
        }

        if (!FromPage)
        {
            if (grvManualJournal.Columns[0].Visible && ddlLOB.SelectedItem.Text.Contains("OL"))
            {
                FNRow.Cells[6].Attributes.Add("align", "right");
                FNRow.Cells[7].Attributes.Add("align", "right");

                FNRow.Cells[5].Text = "Total";
                FNRow.Cells[6].Text = txtTotDebit.Text;
                FNRow.Cells[7].Text = txtTotCredit.Text;
            }
            else if (grvManualJournal.Columns[0].Visible || ddlLOB.SelectedItem.Text.Contains("OL"))
            {
                FNRow.Cells[5].Attributes.Add("align", "right");
                FNRow.Cells[6].Attributes.Add("align", "right");

                FNRow.Cells[4].Text = "Total";
                FNRow.Cells[5].Text = txtTotDebit.Text;
                FNRow.Cells[6].Text = txtTotCredit.Text;
            }
            else
            {
                FNRow.Cells[6].Attributes.Add("align", "right");
                FNRow.Cells[7].Attributes.Add("align", "right");

                FNRow.Cells[5].Text = "Total";
                FNRow.Cells[6].Text = txtTotDebit.Text;
                FNRow.Cells[7].Text = txtTotCredit.Text;
            }
        }

        FNRow.Attributes.Add("border", "none");
        ((System.Web.UI.WebControls.Table)grvManualJournal.Controls[0]).Rows.Add(FNRow);
        ViewState["Summary"] = "1";
        //}
    }

    private void FunPriBindMLAEntityType()
    {
        try
        {
            if (grvManualJournal.FooterRow != null)
            {
                //DropDownList ddlMLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlMLA");
                DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");
                DropDownList ddlPostingFlag = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlPostingFlag");
                //FunBindMLA(ddlMLA);
                FunBindEntityType(ddlEntityType);
                FunBindPostingFlag(ddlPostingFlag);
            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriBindMLAHeaderEntityType(DropDownList ddlEntityType, DropDownList ddlPostingFlag)
    {
        try
        {
            //FunBindMLA(ddlMLA);
            FunBindEntityType(ddlEntityType);
            FunBindPostingFlag(ddlPostingFlag);

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /*private void FunBindMLA(DropDownList ddlMLA)
    {
        try
        {
            //MLA Acc
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Type", "1");
            Procparam.Add("@Is_Closed", "1");
            Procparam.Add("@ParamPA_Status", "6,7,26,45,55");
            Procparam.Add("@ParamSA_Status", "6,7,26,45,55");
            //Procparam.Add("@Is_Activated", "1");
            //Procparam.Add("@ParamPA_Status", "3,11,13,42,46,6,7");

            //dtMLA = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetPLASLA_List, Procparam);
            //ddlMLA.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_AIE, Procparam, new string[] { "PANum", "PANum" });
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }*/

    private void FunBindPostingFlag(DropDownList ddlPostingFlag)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            ddlPostingFlag.BindDataTable("S3G_LOANAD_GetPostingFlag", Procparam, new string[] { "CashFlowFlag_Code", "CashFlowFlag_Desc" });
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    //Modified by chandru
    //private void FunBindSLADim2(DropDownList ddlMLA, DropDownList ddlSLA, DropDownList ddlDim2, RequiredFieldValidator rfvSAN)    
    private void FunBindSLADim2(UserControls_S3GAutoSuggest txtMLASearch, DropDownList ddlSLA, DropDownList ddlDim2, RequiredFieldValidator rfvSAN)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Type", "2");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            //Procparam.Add("@PANum", ddlMLA.SelectedValue);
            Procparam.Add("@PANum", txtMLASearch.SelectedValue.ToString());
            Procparam.Add("@Is_Closed", "1");
            Procparam.Add("@ParamPA_Status", "6,7,26,45,55");
            Procparam.Add("@ParamSA_Status", "6,7,26,45,55");
            Procparam.Add("@Program_ID", intProgramId.ToString());
            ddlSLA.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_AIE, Procparam, new string[] { "SANum", "SANum" });
            if (ddlSLA.Items.Count > 1)
            {
                ddlSLA.Enabled = true;
                rfvSAN.Enabled = true;
            }
            else
            {
                ddlSLA.Enabled = false;
                rfvSAN.Enabled = false;
                FunPriBindDim2(txtMLASearch, ddlSLA, ddlDim2);
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    //Modified by chandru


    private void FunBindGLAcc(DropDownList ddlEntityType, DropDownList ddlGLAcc, DropDownList ddlTaransaction_Flag, string strMLAValue)
    {
        try
        {
            //GL Acc
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            if (Convert.ToInt32(ddlEntityType.SelectedValue) != 10)
            {
                Procparam.Add("@Entity_Type", ddlEntityType.SelectedValue);
            }

            Procparam.Add("@IS_Active", "1");

            if (ddlTranche.SelectedValue != "0")
                Procparam.Add("@Tranche_Header_ID", ddlTranche.SelectedValue);

            if (ddlEntityType.SelectedValue == "1")
            {
                if (ddlTaransaction_Flag.SelectedIndex > 0)
                    Procparam.Add("@Transaction_Flag", ddlTaransaction_Flag.SelectedValue);
                Procparam.Add("@PANUM", strMLAValue);
                ddlGLAcc.BindDataTable("S3G_LOANAD_GetCustomerGLSLAcc_List", Procparam, new string[] { "ID", "Name" });
                //ddlGLAcc.BindDataTable(SPNames.S3G_LOANAD_GetGLSLAcc_List, Procparam, new string[] { "ID", "Name" });
            }
            else
            {
                //ddlGLAcc.BindDataTable(SPNames.S3G_LOANAD_GetGLSLAcc_List, Procparam, new string[] { "ID", "Name" });
                ddlGLAcc.BindDataTable("S3G_LOANAD_GetCustomerGLSLAcc_List", Procparam, new string[] { "ID", "Name" });
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunBindSLAcc(DropDownList ddlEntityType, UserControls_S3GAutoSuggest ddlGLAcc, UserControls_S3GAutoSuggest ddlSLAcc, string strMLAValue)
    {
        try
        {
            //SLA Acc
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Entity_Type", ddlEntityType.SelectedValue);
            Procparam.Add("@GL_Code", ddlGLAcc.SelectedValue);
            Procparam.Add("@PrefixText", "");
            if (ddlEntityType.SelectedValue == "1")
            {
                //strMLAValue = strMLAValue.Substring(0, strMLAValue.Trim().ToString().LastIndexOf("-") - 1).ToString();
                Procparam.Add("@PANum", strMLAValue);
            }
            if (ddlTranche.SelectedValue != "0")
                Procparam.Add("@Tranche_Header_ID", ddlTranche.SelectedValue);
            DataTable dt = Utility.GetDefaultData("S3G_LOANAD_GetCustomerGLSLAcc_List", Procparam);    //ddlSLAcc.BindDataTable(SPNames.S3G_LOANAD_GetGLSLAcc_List, Procparam, new string[] { "ID", "Name" });

            if (dt.Rows.Count > 0)
            {
                ddlSLAcc.IsMandatory = true;
                ddlSLAcc.Enabled = true;

                if (dt.Rows.Count == 1)
                {
                    ddlSLAcc.SelectedValue = dt.Rows[0][0].ToString();
                    ddlSLAcc.SelectedText = dt.Rows[0][1].ToString();
                }
            }
            else
            {
                ddlSLAcc.IsMandatory = false;
                ddlSLAcc.SelectedValue = "0";
                ddlSLAcc.SelectedText = string.Empty;
                ddlSLAcc.Enabled = false;
            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunBindEntityType(DropDownList ddlEntityType)
    {
        try
        {
            //Entity Type
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LookupType_Code", "22");
            ddlEntityType.BindDataTable("S3G_LOANAD_GetMJVLookUpValues", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            if (ddlEntityType.Items.Contains(ddlEntityType.Items.FindByValue("10")))
                ddlEntityType.Items.Remove(ddlEntityType.Items.FindByValue("10"));

            if (ddlTranche.SelectedValue != "0")
                ddlEntityType.Items.Remove(ddlEntityType.Items.FindByValue("1"));
        
            //dtEntity = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetLookUpValues, Procparam);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetMJVDetails()
    {
        try
        {
            FunPriBindGrid();
            if (intMJVID > 0)
            {

                strProcName = "S3G_LoanAd_GetMJVDetails";
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@MJV_No", intMJVID.ToString());
                dsMJV = Utility.GetTableValues(strProcName, Procparam);

                ddlLOB.Items.Add(new ListItem(dsMJV.Tables[0].Rows[0]["LOB_Name"].ToString(), dsMJV.Tables[0].Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = dsMJV.Tables[0].Rows[0]["LOB_Name"].ToString();

                ddlBranch.SelectedText = dsMJV.Tables[0].Rows[0]["Branch"].ToString();
                ddlBranch.SelectedValue = dsMJV.Tables[0].Rows[0]["Location_Id"].ToString();
                ddlBranch.ToolTip = dsMJV.Tables[0].Rows[0]["Branch"].ToString();

                txtMJVDate.Text = Convert.ToString(dsMJV.Tables[0].Rows[0]["MJVDate"]);

                txtMJVNo.Text = dsMJV.Tables[0].Rows[0]["MJVNo"].ToString();
                txtMJVStatus.Text = dsMJV.Tables[0].Rows[0]["MJVStatus"].ToString();

                txtMJVValueDate.Text = Convert.ToString(dsMJV.Tables[0].Rows[0]["MJVValueDate"]);
                hdnStatus.Value = dsMJV.Tables[0].Rows[0]["StatusCode"].ToString();

                if (dsMJV.Tables[0].Rows[0]["Tranche_Header_ID"] != null)
                {
                    ddlTranche.SelectedValue = dsMJV.Tables[0].Rows[0]["Tranche_Header_ID"].ToString();
                    ddlTranche.SelectedText = dsMJV.Tables[0].Rows[0]["Tranche_Name"].ToString();
                }

                FunPriBindGridDetails(dsMJV.Tables[1]);
                //grvManualJournal.DataSource = dsMJV.Tables[1];
                //grvManualJournal.DataBind();

                ViewState["MJVDetails"] = dsMJV.Tables[1];
                dtMJV = dsMJV.Tables[1];
                FunPriCalTotal();

                FunPriBindInvoiceGrid(dsMJV.Tables[2], 1);
                FunPriBindInvoiceGrid(dsMJV.Tables[3], 2);

                ViewState["RSDetails"] = dsMJV.Tables[4];

                if (Convert.ToString(txtMJVStatus.Text) == "Cancelled" || Convert.ToString(txtMJVStatus.Text) == "Approved")
                {                    
                    btnMapInvoice.Enabled = false;
                    grvInvoiceDetail.Columns[grvInvoiceDetail.Columns.Count - 1].Visible = false;

                    btnMapSalesInvoice.Enabled = false;
                    grvSalesInvoiceDetail.Columns[grvSalesInvoiceDetail.Columns.Count - 1].Visible = false;
                }
            }
            FunPriDim2Manupulation();

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            dsMJV.Dispose();
        }
    }


    private void FunPriClear(bool bClearAll)
    {
        ddlLOB.SelectedIndex = ddlDrCr.SelectedIndex = 0;
        //ddlBranch.SelectedIndex = 0;
        ddlBranch.Clear();
        ddlTranche.Clear();
        txtMJVValueDate.Text = DateTime.Now.ToString(strDateFormat);
        txtTally.Text = "";
        txtTotDebit.Text = Convert.ToDecimal("0").ToString(); //Convert.ToDecimal("0").ToString(Utility.SetSuffix());
        txtTotCredit.Text = Convert.ToDecimal("0").ToString(); //Convert.ToDecimal("0").ToString(Utility.SetSuffix());
        grvManualJournal.DataSource = null;
        grvManualJournal.DataBind();
        btnSave.Enabled = false;
        ddlBranch.Clear();
        lblErrorMessage.Text = string.Empty;
        ViewState["RSDetails"] = null;
    }

    private void FunPriDim2Manupulation()
    {
        try
        {

            if (grvManualJournal.FooterRow != null)
            {
                RequiredFieldValidator rfvDim2 = (RequiredFieldValidator)grvManualJournal.FooterRow.FindControl("rfvDim2");
                //if (ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.LastIndexOf("-")).Trim() == "OL")
                //{
                //    rfvDim2.Enabled = true;
                //    grvManualJournal.Columns[5].Visible = true;
                //}
                //else
                //{
                //    rfvDim2.Enabled = false;
                //    grvManualJournal.Columns[5].Visible = false;
                //}
                rfvDim2.Enabled = false;
                grvManualJournal.Columns[5].Visible = false;
            }
        }

        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriGenerateMJVXMLDet()
    {
        try
        {
            //strXMLMJVDetails = grvManualJournal.FunPubFormXml(enumGridType.TemplateGrid).Replace("PrimeA/cNo.", "MLA").Replace("SubA/cNo.", "SLA");
            strXMLMJVDetails = dtMJV.FunPubFormXml();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    ////This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                if (!bCreate)
                {
                    btnSave.Enabled = false;

                }

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                ddlLOB.Enabled = false;
                ddlBranch.Enabled = false;
                ddlTranche.Enabled = false;
                btnClear.Enabled = false;

                if (hdnStatus.Value == "3")
                {
                    btnPrint.Enabled = true;
                }

                if ((hdnStatus.Value != "1"))
                {
                    btnSave.Enabled = false;
                    grvManualJournal.Columns[13].Visible = false;
                    if (grvManualJournal.FooterRow != null)
                    {
                        grvManualJournal.FooterRow.Visible = false;
                    }
                    CalendarExtender2.Enabled = false;
                    CalendarExtender1.Enabled = false;
                }
                if (hdnStatus.Value == "1")
                {
                    btnCancelMJV.Enabled = true;
                }
                txtMJVValueDate.Enabled = false;
                txtMJVDate.Enabled = false;
                txtMJVValueDate.ReadOnly = true;
                txtMJVDate.ReadOnly = true;
                CalendarExtender2.Enabled = false;
                CalendarExtender1.Enabled = false;
                break;

            case -1:// Query Mode


                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }

                if (bClearList)
                {
                    // ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();

                }

                ddlLOB.Enabled = true;
                ddlBranch.Enabled = true;
                ddlBranch.ReadOnly = true;
                ddlTranche.ReadOnly = true;
                ddlDrCr.ClearDropDownList();
                txtMJVValueDate.Enabled = true;
                txtMJVDate.Enabled = true;
                txtMJVStatus.Enabled = true;
                txtMJVNo.Enabled = true;

                txtMJVValueDate.ReadOnly = true;
                txtMJVDate.ReadOnly = true;
                txtMJVStatus.ReadOnly = true;
                txtMJVNo.ReadOnly = true;
                CalendarExtender2.Enabled = false;
                CalendarExtender1.Enabled = false;
                imgMJVDate.Visible = false;
                imgInvoiceDate.Visible = false;
                btnClear.Enabled = false;
                btnSave.Enabled = btnMapInvoice.Enabled = btnMapSalesInvoice.Enabled = false;
                tdAlign.Style.Add("width", "46px");
                if (hdnStatus.Value == "3")
                {
                    btnPrint.Enabled = true;
                }
                if (grvManualJournal.FooterRow != null)
                {
                    Button btnGridSave = (Button)grvManualJournal.FooterRow.FindControl("btnAdd");
                    btnGridSave.Enabled = false;
                    grvManualJournal.FooterRow.Visible = false;
                    //grvManualJournal.Columns[11].Visible = false;
                    grvManualJournal.Columns[12].Visible = false;
                    grvManualJournal.Columns[13].Visible = false;

                }
                if (grvInvoiceDetail != null)
                {
                    grvInvoiceDetail.Columns[grvInvoiceDetail.Columns.Count - 1].Visible = false;
                }
                if (grvSalesInvoiceDetail != null)
                {
                    grvSalesInvoiceDetail.Columns[grvSalesInvoiceDetail.Columns.Count - 1].Visible = false;
                }
                break;
        }

    }

    ////Code end

    //public static string FunPubManualJournalXMLGenerate(ClsSystemJournal ObjSysJournal)
    //{
    //    StringBuilder strbSysJournal = new StringBuilder();
    //    strbSysJournal.Append("<Root> <Details ");
    //    strbSysJournal.Append("Ref_No='" + ObjSysJournal.Reference_Number + "' ");
    //    strbSysJournal.Append("Sub_Ref_No='" + ObjSysJournal.Sub_Reference_Number + "' ");
    //    strbSysJournal.Append("Occur_No='" + ObjSysJournal.Occurrence_No + "' ");
    //    strbSysJournal.Append("Txn_Amt='" + ObjSysJournal.Txn_Amount + "' ");
    //    strbSysJournal.Append("GLAcc_No='" + ObjSysJournal.GL_Account_Number + "' ");
    //    strbSysJournal.Append("SubGLAcc_No='" + ObjSysJournal.Sub_GL_Account_Number + "' ");
    //    strbSysJournal.Append("Acc_Flag='" + ObjSysJournal.Accounting_Flag + "' ");
    //    strbSysJournal.Append("Dim2_Code='" + ObjSysJournal.Global_Dimension2_Code + "' ");
    //    strbSysJournal.Append("Dim2_No='" + ObjSysJournal.Global_Dimension2_No + "' ");
    //    strbSysJournal.Append(" /> ");
    //    return strbSysJournal.ToString();
    //}


    //Add by chandru on 23/04/2012
    #region CommonWebmethod
    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetMLAList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] != null)
        {
            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
                Procparam.Add("@LOB_ID", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
        }
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@Type", "1");
        Procparam.Add("@Is_Closed", "1");

        // Code Changed for Call Id : 4763 CR_057
        //Procparam.Add("@ParamPA_Status", "6,7,26,45,55");
        //Procparam.Add("@ParamSA_Status", "6,7,26,45,55");
        Procparam.Add("@ParamPA_Status", "26,45,55");
        Procparam.Add("@ParamSA_Status", "26,45,55");

        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPLASLA_AIE_AGT", Procparam));
        return suggetions.ToArray();

    }
    protected void txtMLASearch_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");
            if (txtMLASearch.SelectedText.ToString().Trim() == string.Empty)
            {
                txtMLASearch.SelectedValue = string.Empty;
                txtMLASearch.Focus();
                return;
            }
            //string strhdnValue = hdnCommonID.Value;
            //if (strhdnValue == "-1" || strhdnValue == "")
            //{
            //    txtMLASearch.SelectedValue = "";
            //    hdnCommonID.Value = string.Empty;
            //    txtMLASearch.Focus();
            //    return;
            //}
            DropDownList ddlSLA = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlSLA");
            DropDownList ddlDim2 = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlDim2");
            DropDownList ddlEntityType = (DropDownList)grvManualJournal.FooterRow.FindControl("ddlEntityType");
            RequiredFieldValidator rfvSAN = (RequiredFieldValidator)grvManualJournal.FooterRow.FindControl("rfvSLA");
            FunBindSLADim2(txtMLASearch, ddlSLA, ddlDim2, rfvSAN);
            if (ddlEntityType.SelectedValue == "1")//if customer then change selected value to -1
            {
                ddlEntityType.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "ManualJournal");
        }
    }
    protected void txtMLASearchhdr_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            UserControls_S3GAutoSuggest txtMLASearchhdr = (UserControls_S3GAutoSuggest)sender;
            GridViewRow grvRow = (GridViewRow)txtMLASearchhdr.Parent.Parent;
            UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)grvManualJournal.FooterRow.FindControl("txtMLASearch");

            //string strhdnValue = hdnCommonID.Value;
            if (txtMLASearch.SelectedText.ToString().Trim() == string.Empty)
            {
                txtMLASearchhdr.SelectedText = string.Empty;
                //hdnCommonID.Value = string.Empty;
                txtMLASearchhdr.Focus();
                return;
            }
            //DropDownList ddlMLA = (DropDownList)grvRow.FindControl("ddlMLAhdr");
            DropDownList ddlSLA = (DropDownList)grvRow.FindControl("ddlSLAhdr");
            DropDownList ddlDim2 = (DropDownList)grvRow.FindControl("ddlDim2Hdr");
            DropDownList ddlEntityType = (DropDownList)grvRow.FindControl("ddlEntityTypeHdr");
            RequiredFieldValidator rfvSAN = (RequiredFieldValidator)grvRow.FindControl("rfvSLAhdr");
            FunBindSLADim2(txtMLASearch, ddlSLA, ddlDim2, rfvSAN);
            if (ddlEntityType.SelectedValue == "1")//if customer then change selected value to -1
            {
                ddlEntityType.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "ManualJournal");
        }
    }


    #endregion

    // Added By Shibu 24-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "077");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetGLAccF(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam = new Dictionary<string, string>();
        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)obj_Page.grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        DropDownList ddlEntityType = (DropDownList)obj_Page.grvManualJournal.FooterRow.FindControl("ddlEntityType");
        DropDownList ddlTaransaction_Flag = (DropDownList)obj_Page.grvManualJournal.FooterRow.FindControl("ddlPostingFlag");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)obj_Page.grvManualJournal.FooterRow.FindControl("txtMLASearch");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID.ToString()));
        Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
        if (Convert.ToInt32(ddlEntityType.SelectedValue) != 10)
        {
            Procparam.Add("@Entity_Type", ddlEntityType.SelectedValue);
        }

        Procparam.Add("@IS_Active", "1");
        if (ddlEntityType.SelectedValue == "1")
        {
            if (ddlTaransaction_Flag.SelectedIndex > 0)
                Procparam.Add("@Transaction_Flag", ddlTaransaction_Flag.SelectedValue);
            if (!string.IsNullOrEmpty(txtMLASearch.SelectedText.Trim()))
                Procparam.Add("@PANUM", txtMLASearch.SelectedValue.ToString());
        }
        Procparam.Add("@PrefixText", prefixText);
        if (obj_Page.ddlTranche.SelectedValue != "0")
            Procparam.Add("@Tranche_Header_ID", obj_Page.ddlTranche.SelectedValue);

        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetCustomerGLSLAcc_List", Procparam));

        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetSLAccF(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam = new Dictionary<string, string>();
        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)obj_Page.grvManualJournal.FooterRow.FindControl("ddlGLAcc");
        DropDownList ddlEntityType = (DropDownList)obj_Page.grvManualJournal.FooterRow.FindControl("ddlEntityType");
        DropDownList ddlTaransaction_Flag = (DropDownList)obj_Page.grvManualJournal.FooterRow.FindControl("ddlPostingFlag");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)obj_Page.grvManualJournal.FooterRow.FindControl("txtMLASearch");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID.ToString()));
        Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
        if (Convert.ToInt32(ddlEntityType.SelectedValue) != 10)
        {
            Procparam.Add("@Entity_Type", ddlEntityType.SelectedValue);
        }
        Procparam.Add("@GL_Code", ddlGLAcc.SelectedValue);

        Procparam.Add("@IS_Active", "1");
        if (ddlEntityType.SelectedValue == "1")
        {
            if (ddlTaransaction_Flag.SelectedIndex > 0)
                Procparam.Add("@Transaction_Flag", ddlTaransaction_Flag.SelectedValue);
            if (!string.IsNullOrEmpty(txtMLASearch.SelectedText.Trim()))
                Procparam.Add("@PANUM", txtMLASearch.SelectedValue.ToString());
        }
        Procparam.Add("@PrefixText", prefixText);
        if (obj_Page.ddlTranche.SelectedValue != "0")
            Procparam.Add("@Tranche_Header_ID", obj_Page.ddlTranche.SelectedValue);

        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetCustomerGLSLAcc_List", Procparam));

        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetGLAccE(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam = new Dictionary<string, string>();
        int intRowindex = obj_Page.grvManualJournal.EditIndex;
        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)obj_Page.grvManualJournal.Rows[intRowindex].FindControl("ddlGLAccHdr");
        DropDownList ddlEntityType = (DropDownList)obj_Page.grvManualJournal.Rows[intRowindex].FindControl("ddlEntityTypeHdr");
        DropDownList ddlTaransaction_Flag = (DropDownList)obj_Page.grvManualJournal.Rows[intRowindex].FindControl("ddlPostingFlagHdr");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)obj_Page.grvManualJournal.Rows[intRowindex].FindControl("txtMLASearchhdr");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID.ToString()));
        Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
        if (Convert.ToInt32(ddlEntityType.SelectedValue) != 10)
        {
            Procparam.Add("@Entity_Type", ddlEntityType.SelectedValue);
        }

        Procparam.Add("@IS_Active", "1");
        if (ddlEntityType.SelectedValue == "1")
        {
            if (ddlTaransaction_Flag.SelectedIndex > 0)
                Procparam.Add("@Transaction_Flag", ddlTaransaction_Flag.SelectedValue);
            if (!string.IsNullOrEmpty(txtMLASearch.SelectedText.Trim()))
                Procparam.Add("@PANUM", txtMLASearch.SelectedValue.ToString());
        }
        Procparam.Add("@PrefixText", prefixText);

        if (obj_Page.ddlTranche.SelectedValue != "0")
            Procparam.Add("@Tranche_Header_ID", obj_Page.ddlTranche.SelectedValue);

        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetCustomerGLSLAcc_List", Procparam));

        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetSLAccE(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam = new Dictionary<string, string>();
        int intRowindex = obj_Page.grvManualJournal.EditIndex;
        UserControls_S3GAutoSuggest ddlGLAcc = (UserControls_S3GAutoSuggest)obj_Page.grvManualJournal.Rows[intRowindex].FindControl("ddlGLAccHdr");
        DropDownList ddlEntityType = (DropDownList)obj_Page.grvManualJournal.Rows[intRowindex].FindControl("ddlEntityTypeHdr");
        DropDownList ddlTaransaction_Flag = (DropDownList)obj_Page.grvManualJournal.Rows[intRowindex].FindControl("ddlPostingFlagHdr");
        UserControls_S3GAutoSuggest txtMLASearch = (UserControls_S3GAutoSuggest)obj_Page.grvManualJournal.Rows[intRowindex].FindControl("txtMLASearchhdr");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID.ToString()));
        Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
        if (Convert.ToInt32(ddlEntityType.SelectedValue) != 10)
        {
            Procparam.Add("@Entity_Type", ddlEntityType.SelectedValue);
        }
        Procparam.Add("@GL_Code", ddlGLAcc.SelectedValue);

        Procparam.Add("@IS_Active", "1");
        if (ddlEntityType.SelectedValue == "1")
        {
            if (ddlTaransaction_Flag.SelectedIndex > 0)
                Procparam.Add("@Transaction_Flag", ddlTaransaction_Flag.SelectedValue);
            if (!string.IsNullOrEmpty(txtMLASearch.SelectedText.Trim()))
                Procparam.Add("@PANUM", txtMLASearch.SelectedValue.ToString());
        }
        Procparam.Add("@PrefixText", prefixText);
        if (obj_Page.ddlTranche.SelectedValue != "0")
            Procparam.Add("@Tranche_Header_ID", obj_Page.ddlTranche.SelectedValue);

        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetCustomerGLSLAcc_List", Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTranche_AGT", Procparam));

        return suggestions.ToArray();
    }

    #endregion

    protected void btnMapInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAccount = (DataTable)ViewState["MJVDetails"];
            string strFilter = "MLA <> " + "''" + " and EntityTypeValue = 1";

            DataRow[] drInvoice = dtAccount.Select(strFilter);

            if (ddlTranche.SelectedValue == "0" && drInvoice.Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one RS Details against Customer as Entity Type");
                return;
            }

            string strXML_Invoice = "";
            if (ddlTranche.SelectedValue != "0" && ViewState["RSDetails"] != null)
            {
                if (ddlDrCr.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this, "Select Dr. or Cr.");
                    return;
                }

                dtAccount = (DataTable)ViewState["RSDetails"];
                strXML_Invoice = "<Root> ";
                foreach (DataRow row in dtAccount.Rows)
                {
                    if (row["IsSelect"].ToString() == "1")
                        strXML_Invoice += "<Details MLA= '" + row["PANum"].ToString() + "' />";
                }
                strXML_Invoice += " </Root>";
            }
            else
            {
                strXML_Invoice = Utility.FunPubFormXml(dtAccount, true);
            }

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@XML_Invoice", Convert.ToString(strXML_Invoice));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            if (grvInvoiceDetail != null && grvInvoiceDetail.Rows.Count > 0)
                Procparam.Add("@Is_New", "2");
            else
                Procparam.Add("@Is_New", "1");
            DataTable dtInvoice = Utility.GetDefaultData("S3G_Loanad_GetMJVInvocieDetails", Procparam);
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                grvPoInvoiceDetails.DataSource = dtInvoice;
                grvPoInvoiceDetails.DataBind();
                moePoInvoiceDtls.Show();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Invoices found");
            }
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    protected void btnMapSalesInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAccount = (DataTable)ViewState["MJVDetails"];
            string strFilter = "MLA <> " + "''" + " and EntityTypeValue = 1";

            DataRow[] drInvoice = dtAccount.Select(strFilter);
            if (drInvoice.Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one RS Details against Customer as Entity Type");
                return;
            }

            string strXML_Invoice = Utility.FunPubFormXml(dtAccount, true);

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@XML_Invoice", Convert.ToString(strXML_Invoice));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            if (grvInvoiceDetail != null && grvInvoiceDetail.Rows.Count > 0)
                Procparam.Add("@Is_New", "2");
            else
                Procparam.Add("@Is_New", "1");
            DataTable dtInvoice = Utility.GetDefaultData("S3G_Loanad_GetMJVSalesInvocieDetails", Procparam);
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                grvSalesInvoice.DataSource = dtInvoice;
                grvSalesInvoice.DataBind();
                moeSalesInvoiceDtls.Show();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Invoices found");
            }
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    private void FunPriBindInvoiceGrid(DataTable dtInvoice, int intOption)
    {
        try
        {
            if (intOption == 1)
            {
                grvInvoiceDetail.DataSource = dtInvoice;
                grvInvoiceDetail.DataBind();

                if (dtInvoice != null)
                    pnlInvoiceDetails.Visible = (Convert.ToUInt32(dtInvoice.Rows.Count) > 0) ? true : false;
                else
                    pnlInvoiceDetails.Visible = false;
            }
            else if (intOption == 2)
            {
                grvSalesInvoiceDetail.DataSource = dtInvoice;
                grvSalesInvoiceDetail.DataBind();
                if (dtInvoice != null)
                    pnlSalesInvoiceDetails.Visible = (Convert.ToUInt32(dtInvoice.Rows.Count) > 0) ? true : false;
                else
                    pnlSalesInvoiceDetails.Visible = false;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriGetInvoiceDetail()
    {
        try
        {
            DataTable dtAccount = (DataTable)ViewState["MJVDetails"];
            string strFilter = "MLA <> " + "''" + " and EntityTypeValue = 1";

            DataRow[] drInvoice = dtAccount.Select(strFilter);
            if (drInvoice.Length > 0)
            {
                string strXML_Invoice = Utility.FunPubFormXml(dtAccount, true);

                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@XML_Invoice", Convert.ToString(strXML_Invoice));
                DataTable dtInvoice = Utility.GetDefaultData("S3G_Loanad_GetMJVInvocieDetails", Procparam);
                FunPriBindInvoiceGrid(dtInvoice, 1);
            }
            else
            {
                FunPriBindInvoiceGrid(null, 1);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }


    protected void imgPopupClose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moePoInvoiceDtls.Hide();
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    protected void imgSalesPopupClose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moeSalesInvoiceDtls.Hide();
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    protected void imgRS_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moeRS.Hide();
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    protected void btnAddRS_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
             dt = (DataTable)ViewState["RSDetails"];
            foreach (GridViewRow row in grvRS.Rows)
            {
                if (((CheckBox)row.FindControl("chkSelect")).Checked)
                    dt.Rows[row.RowIndex]["IsSelect"] = 1;
                else
                    dt.Rows[row.RowIndex]["IsSelect"] = 0;
            }
            dt.AcceptChanges();
            ViewState["RSDetails"] = dt;
            moeRS.Hide();
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    protected void btnAddPOInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            int intSelectedCnt = 0;
            decimal decDrCr = 0;

            foreach (GridViewRow grInvoice in grvPoInvoiceDetails.Rows)
            {
                if (grInvoice.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = grInvoice.FindControl("chkSelect") as CheckBox;
                    if (chkSelect.Checked)
                    {
                        intSelectedCnt += 1;
                        decDrCr += Convert.ToDecimal(((Label)grInvoice.FindControl("lblNet_Payable")).Text);
                    }
                }
            }
            if (intSelectedCnt == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast one Invoice Details");
                return;
            }

            string strXMLInvDetails = Utility.FunPubFormXml(grvPoInvoiceDetails, true, false);
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "1");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            if (grvInvoiceDetail != null && grvInvoiceDetail.Rows.Count > 0)
                Procparam.Add("@Is_New", "2");
            else
                Procparam.Add("@Is_New", "1");
            Procparam.Add("@XMLInvoiceDtl", strXMLInvDetails);

            DataTable dtInvoice = Utility.GetDefaultData("S3G_LAD_InsertMJVInvoiceDtl", Procparam);
            FunPriBindInvoiceGrid(dtInvoice, 1);

            if (ddlTranche.SelectedValue != "0")
            {
                TextBox txtDebit = (TextBox)grvManualJournal.FooterRow.FindControl("txtDebit");
                TextBox txtCredit = (TextBox)grvManualJournal.FooterRow.FindControl("txtCredit");

                if (ddlDrCr.SelectedValue == "1")
                    txtDebit.Text = txtDebit.Text != "" ? (Convert.ToDecimal(txtDebit.Text) + decDrCr).ToString() : decDrCr.ToString();
                else if (ddlDrCr.SelectedValue == "2")
                    txtCredit.Text = txtCredit.Text != "" ? (Convert.ToDecimal(txtCredit.Text) + decDrCr).ToString() : decDrCr.ToString();
            }

            moePoInvoiceDtls.Hide();
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    protected void btnAddSalesInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            int intSelectedCnt = 0;
            foreach (GridViewRow grInvoice in grvSalesInvoice.Rows)
            {
                if (grInvoice.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = grInvoice.FindControl("chkSelect") as CheckBox;
                    if (chkSelect.Checked)
                    {
                        intSelectedCnt += 1;
                    }
                }
            }
            if (intSelectedCnt == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast one Invoice Details");
                return;
            }

            string strXMLInvDetails = Utility.FunPubFormXml(grvSalesInvoice, true, false);
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "1");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            if (grvSalesInvoice != null && grvSalesInvoice.Rows.Count > 0)
                Procparam.Add("@Is_New", "2");
            else
                Procparam.Add("@Is_New", "1");
            Procparam.Add("@XMLInvoiceDtl", strXMLInvDetails);

            DataTable dtInvoice = Utility.GetDefaultData("S3G_LAD_InsertMJVSalesInvoiceDtl", Procparam);
            FunPriBindInvoiceGrid(dtInvoice, 2);
            moeSalesInvoiceDtls.Hide();
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    protected void lnkRemoveInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvInvoiceDetail", strSelectID);

            if (ddlTranche.SelectedValue != "0")
            {
                TextBox txtDebit = (TextBox)grvManualJournal.FooterRow.FindControl("txtDebit");
                TextBox txtCredit = (TextBox)grvManualJournal.FooterRow.FindControl("txtCredit");
                Label lblNet_Payable = (Label)grvInvoiceDetail.Rows[_iRowIdx].FindControl("lblNet_Payable");

                if (ddlDrCr.SelectedValue == "1" && txtDebit.Text != "")
                    txtDebit.Text = (Convert.ToDecimal(txtDebit.Text) - Convert.ToDecimal(lblNet_Payable.Text)).ToString();
                else if (ddlDrCr.SelectedValue == "2" && txtCredit.Text != "")
                    txtCredit.Text = (Convert.ToDecimal(txtCredit.Text) - Convert.ToDecimal(lblNet_Payable.Text)).ToString();
            }

            Label lblRSInvoicID = (Label)grvInvoiceDetail.Rows[_iRowIdx].FindControl("lblAccountInvoiceID");
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "2");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Invoice_ID", Convert.ToString(lblRSInvoicID.Text));

            DataTable dtInvoice = Utility.GetDefaultData("S3G_LAD_InsertMJVInvoiceDtl", Procparam);
            FunPriBindInvoiceGrid(dtInvoice, 1);
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }

    protected void lnkRemoveSalesInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvSalesInvoiceDetail", strSelectID);

            Label lblSalesInvoiceID = (Label)grvSalesInvoiceDetail.Rows[_iRowIdx].FindControl("lblSalesInvoiceID");
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "2");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Invoice_ID", Convert.ToString(lblSalesInvoiceID.Text));

            DataTable dtInvoice = Utility.GetDefaultData("S3G_LAD_InsertMJVSalesInvoiceDtl", Procparam);
            FunPriBindInvoiceGrid(dtInvoice, 2);
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
        }
    }
}



