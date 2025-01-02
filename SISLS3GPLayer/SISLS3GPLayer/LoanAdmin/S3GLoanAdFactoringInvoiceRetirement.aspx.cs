#region Page Header

/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Factoring Invoice Retirement
/// Created By			:  Bhuvana.C Thangam M. 
/// Created Date		: 26-April-2013
/// Purpose	            : To fetch Factoring Invoice Retirement Details
/// Modified By         : Shibu 
/// Modified Date       : 28-Sep-2013
/// Purpose	            : Performance Tuning DDL to Change AutoSuggestion An Avoid Unwanted DDL Load Events
///

#endregion

#region Namespaces

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

#endregion

public partial class LoanAdmin_S3GLoanAdFactoringInvoiceRetirement : ApplyThemeForProject
{
    #region Declaration

    LoanAdminMgtServicesReference.LoanAdminMgtServicesClient ObjFactoringInvoiceClient;
    LoanAdminMgtServices.S3G_LOANAD_FT_RetirementDataTable ObjS3G_LOANAD_FT_RetirementDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;

    int intFILID = 0;
    int intErrCode = 0;
    int intUserID = 0;
    int intCompanyID = 0;

    int i;
    bool status;
    string s, strFILNo, strInvoiceNo, strPartyName;
    static string strPageName = "Factoring Invoice Retirement";

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    DataTable dtApprovals = new DataTable();
    Dictionary<string, string> dictParam = null;

    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=FLR";
    string strKey = "Insert";
    string strXMLFILDet = string.Empty;
    decimal totalScore = 0;
    decimal totalInvoice = 0;
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdFactoringInvoiceRetirement.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=FLR';";
    S3GSession ObjS3GSession;
    public static LoanAdmin_S3GLoanAdFactoringInvoiceRetirement obj_Page;
    #endregion

    #region Page Loading

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CECFILDATE.Format = strDateFormat;
            ddlLOB.AddItemToolTip();
           // ddlBranch.AddItemToolTip();
            ddlPAN.AddItemToolTip();
            ddlSAN.AddItemToolTip();
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                { intFILID = Convert.ToInt32(fromTicket.Name); }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end            
            lblErrorMessage.Text = "";
            if (!IsPostBack)
            {
                bool CanModify = true;
                btnGo.Enabled = false;
               
              

                if (PageMode == PageModes.Create)
                {
                    FunProBindBranchPANSAN();
                    txtInvoiceLoadValue.SetDecimalPrefixSuffix(10, 2, true, false, "Invoice Load Value");
                }
                //User Authorization            
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if ((intFILID > 0) && (strMode == "M"))// Modify
                {
                    CanModify = FunProGetFactoringInvoiceDetails();
                    FunProDisableControls(1);
                }
                else if ((intFILID > 0) && (strMode == "Q")) // Query 
                {
                    CanModify = FunProGetFactoringInvoiceDetails();
                    FunProDisableControls(-1);
                }
                else  //Create Mode
                {
                    FunProDisableControls(0);
                }
                if (ddlLOB.Items.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "The Factoring Line of Business is not Activated.", strRedirectPage);
                }
                ddlBranch.Focus();
            }
            if (GRVFIL.FooterRow != null)
            {
                AjaxControlToolkit.CalendarExtender CEXInvDt = (AjaxControlToolkit.CalendarExtender)GRVFIL.FooterRow.FindControl("CalendarExtender1F");
                if (CEXInvDt != null)
                {
                    CEXInvDt.Format = strDateFormat;
                }
                TextBox txtInvoiceAmountF = (TextBox)GRVFIL.FooterRow.FindControl("txtInvoiceAmountF");
                txtInvoiceAmountF.SetDecimalPrefixSuffix(10, 2, true, false, "Invoice Amount");
                TextBox txtInvDateF = (TextBox)GRVFIL.FooterRow.FindControl("txtInvoiceDateF");
                if (txtInvDateF != null)
                {
                    txtInvDateF.Attributes.Add("readonly", "readonly");
                }
            }


        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region Page Events

    #region Save Clear Cancel Go

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjFactoringInvoiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();

        try
        {

            strFILNo = "";
            ObjS3G_LOANAD_FT_RetirementDataTable = new LoanAdminMgtServices.S3G_LOANAD_FT_RetirementDataTable();
            LoanAdminMgtServices.S3G_LOANAD_FT_RetirementRow objS3G_LOANAD_FT_RetirementDataRow;
            objS3G_LOANAD_FT_RetirementDataRow = ObjS3G_LOANAD_FT_RetirementDataTable.NewS3G_LOANAD_FT_RetirementRow();

            objS3G_LOANAD_FT_RetirementDataRow.Company_ID = intCompanyID;
            objS3G_LOANAD_FT_RetirementDataRow.FT_Retirement_ID = intFILID;
            objS3G_LOANAD_FT_RetirementDataRow.XML_Data = ((DataTable)ViewState["MappTable"]).FunPubFormXml();
            objS3G_LOANAD_FT_RetirementDataRow.User_ID = intUserID;
            objS3G_LOANAD_FT_RetirementDataRow.FIL_ID = Convert.ToInt32(ddlFactdocno.SelectedValue);
            objS3G_LOANAD_FT_RetirementDataRow.Created_On = DateTime.Now;

            ObjS3G_LOANAD_FT_RetirementDataTable.AddS3G_LOANAD_FT_RetirementRow(objS3G_LOANAD_FT_RetirementDataRow);

            intErrCode = ObjFactoringInvoiceClient.FunPubCreateFactoringRetirement(out strFILNo, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_FT_RetirementDataTable, SerMode));

            switch (intErrCode)
            {
                case 0:
                    {
                        if (intFILID > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Factoring Retirement details updated successfully");
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        else
                        {
                            txtFILNo.Text = strFILNo;

                            strAlert = "Factoring Retirement details added successfully-" + strFILNo;
                            strAlert += @"\n\nWould you like to add one more Factoring Retirement?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END

                            lblErrorMessage.Text = string.Empty;
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }
                    break;
                case 2: Utility.FunShowAlertMsg(this.Page, "Document Number Not yet Defined");
                    break;
                case 5: Utility.FunShowAlertMsg(this.Page, "Document Number exceeded");
                    break;
                case 6: Utility.FunShowAlertMsg(this.Page, "Invoice Details exist for the given combination");
                    break;
                case 50: Utility.FunShowAlertMsg(this.Page, "Error in saving details");
                    break;
                default: break;
            }
            //dictParam = new Dictionary<string, string>();
            //dictParam.Add("@Company_ID", intCompanyID.ToString());
            //dictParam.Add("@XML_Data", ((DataTable)ViewState["MappTable"]).FunPubFormXml());
            //dictParam.Add("@User_ID", intUserID.ToString());
            //dictParam.Add("@FIL_ID", ddlFactdocno.SelectedValue);

            //Utility.GetDefaultData("S3G_LOANAD_InsertFactoringRetirement", dictParam);

            //strAlert = strAlert.Replace("__ALERT__", "Factoring Retirement details updated successfully");
            //strRedirectPageAdd = "window.location.href='../Common/HomePage.aspx';";

            //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);

        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
        finally
        {
            //if (ObjFactoringInvoiceClient != null)
            //{
            ObjFactoringInvoiceClient.Close();
            //}
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunProClearControls();
            //if (ddlBranch.Items.Count > 0)
            //{ ddlBranch.SelectedIndex = 0; }
            ddlBranch.Clear();
            if (ddlPAN.Items.Count > 0)
            { ddlPAN.Items.Clear(); }
            if (ddlSAN.Items.Count > 0)
            {
                ddlSAN.Items.Clear();
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Clear the Controls";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Redirect the Page";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {

            //string s=ViewState["AccountDate"].ToString();
            //string s1 = Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat);

            //if (Convert.ToDateTime(Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat)) >
            //    Convert.ToDateTime(txtFILDate.Text))

            if (!FunPriValidateFromEndDate(ViewState["AccountDate"].ToString(), txtFILDate.Text))
            {
                cvFactoring.ErrorMessage = "FIL date should be greater than or equal to Account creation date (" + Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat) + ")";
                cvFactoring.IsValid = false;
                return;
            }

            FunProSetInitialRow();

            GRVFIL.Visible = true;

            pnlInvDtl.Visible = true;
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    /// <summary>
    /// This method will validate the from and to date - entered by the user.
    /// </summary>
    private bool FunPriValidateFromEndDate(string strFromDate, string strToDate)
    {
        DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
        dtformat.ShortDatePattern = "MM/dd/yy";

        //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
        if (Utility.StringToDate(strFromDate) > Utility.StringToDate(strToDate)) // start date should be less than or equal to the enddate
        {
            return false;
        }
        return true;
    }


    #endregion

    #region Gridview Events


    #endregion

    #region DDL Events

    protected void ddlPAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearControls(true);
            FunProGetCustomerDetails();
            FunProGetFactDetails();

            FunProLoadSubAccNo();

            string IsExist = "";
            IsExist = FunProGetColumnValue(ddlPAN, "Exist", (DataTable)ViewState["PANumDt"]);
            if (IsExist != "" && IsExist == "1")
            {
                Utility.FunShowAlertMsg(this.Page, "FIL already exists for this Account");
                btnSave.Enabled = false;
                btnGo.Enabled = false;
            }
            else
            {
                if (bCreate)
                {
                    btnSave.Enabled = true;
                }

                btnGo.Enabled = true;
            }

            ddlPAN.Focus();
            ddlPAN.ToolTip = ddlPAN.SelectedItem.Text;
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearControls(true);
            ddlSAN.Items.Clear();
            //FunProLoadPrimeAccNo();
            ddlBranch.Focus();
            ddlBranch.ToolTip = ddlBranch.SelectedText;
            ddlFactdocno.Items.Clear();
            FunProLoadFactdocno();
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ddlSAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearControls(false);
            FunProGetFactDetails();
            FunProGetAccountDetails(true);
            ddlSAN.Focus();
            ddlSAN.ToolTip = ddlSAN.SelectedItem.Text;
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #endregion

    #region User Defined Functions

    protected void FunProClearControls(bool CanClearCustomer)
    {
        try
        {
            txtStatus.Text =
            txtMargin.Text =
            txtCreditLimit.Text =
            txtCreditAvailable.Text =
            txtOutStandingAmount.Text =
            txtInvoiceLoadValue.Text = "";

            //pnlInvDtl.Visible = false;

            if (CanClearCustomer)
            {
                S3GCustomerAddress1.ClearCustomerDetails();
            }

            btnGo.Enabled = false;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to Clear the value");
        }
    }

    protected void FunProBindBranchPANSAN()
    {
        try
        {
            // LOB
            FunPriLoadLob();




            if (ddlLOB.Items.Count > 0)
            {
                ddlLOB.Items.RemoveAt(0);
                //ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
            }

            //Branch
            FunPriLoadBranch();

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to LOB and Branch");
        }
    }

    private void FunPriLoadBranch()
    {
        //Removed By Shibu DDL Changed to AutoSuggestion
        //dictParam = new Dictionary<string, string>();
        //dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //if (PageMode == PageModes.Create)
        //{
        //    dictParam.Add("@Is_Active", "1");
        //}
        //dictParam.Add("@User_ID", intUserID.ToString());
        //dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
        //dictParam.Add("@Program_ID", "57");
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
        //ddlBranch.AddItemToolTip();
        //ddlBranch.ToolTip = ddlBranch.SelectedItem.Text;
    }

    private void FunPriLoadLob()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        if (PageMode == PageModes.Create)
        {
            dictParam.Add("@Is_Active", "1");
        }
        dictParam.Add("@User_ID", intUserID.ToString());
        dictParam.Add("@Program_ID", "57");
        ddlLOB.BindDataTable(SPNames.S3G_LOANAD_GetFactoringLOB, dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        ddlLOB.AddItemToolTip();
    }

    protected void FunProGenerateFactoringXMLDet()
    {
        try
        {
            strXMLFILDet = GRVFIL.FunPubFormXml();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to generate XML.");
        }
    }

    protected void FunProDisableControls(int mode)
    {
        try
        {
            switch (mode)
            {
                case 0:

                    Dictionary<string, string> dictFacInvoice = new Dictionary<string, string>();
                    dictFacInvoice.Add("@Company_ID", Convert.ToString(intCompanyID));
                    DataSet dsFactoringInvoiceDetails = new DataSet();
                    dsFactoringInvoiceDetails = Utility.GetDataset("S3G_LOANAD_GetFactoringDetails", dictFacInvoice);
                    if (string.IsNullOrEmpty(dsFactoringInvoiceDetails.Tables[1].Rows[0][0].ToString()))
                    {
                        Utility.FunShowAlertMsg(this, "Factoring Invoice gap days is not defined in the Global Parameter Setup", strRedirectPage);
                    }

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    //pnlInvDtl.Visible = false;

                    txtFILDate.Attributes.Add("readonly", "readonly");
                    break;
                case 1:
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (ddlPAN.Items.Count != 0)
                    {
                        ddlPAN.ClearDropDownList();
                    }
                    txtFILDate.ReadOnly = txtMargin.ReadOnly = txtCreditAvailable.ReadOnly = txtCreditDays.ReadOnly = txtInvoiceLoadValue.ReadOnly = txtCreditLimit.ReadOnly = txtOutStandingAmount.ReadOnly = true;
                    ddlBranch.ReadOnly = true;
                    if (ddlSAN.Items.Count != 0)
                    {
                        ddlSAN.ClearDropDownList();
                    }
                    imgFILDate.Visible = CECFILDATE.Enabled = false;
                    btnClear.Enabled = false;
                    txtFILDate.Attributes.Add("readonly", "readonly");
                    btnGo.Enabled = false;
                    break;
                case -1:
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    //ddlBranch.ClearDropDownList();
                    ddlBranch.ReadOnly = true;
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    btnGo.Enabled = false;
                    grvMapping.Columns[grvMapping.Columns.Count - 1].Visible = false;
                    txtFILDate.Attributes.Add("readonly", "readonly");
                    txtMargin.ReadOnly = txtCreditAvailable.ReadOnly = txtCreditDays.ReadOnly = txtInvoiceLoadValue.ReadOnly = txtCreditLimit.ReadOnly = txtOutStandingAmount.ReadOnly = true;
                    imgFILDate.Visible = CECFILDATE.Enabled = false;
                    btnGo.Enabled = false;
                    if (grvMapping.FooterRow != null)
                    {
                        grvMapping.FooterRow.Visible = false;
                    }
                    break;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to disable controls.");
        }
    }

    protected void FunProSetInitialMappingRow()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SNo");
            dt.Columns.Add("FT_Retirement_Dtl_ID");
            dt.Columns.Add("Factoring_Inv_Load_Details_ID");
            dt.Columns.Add("Invoiceno");
            dt.Columns.Add("UnreceivedAmount");
            dt.Columns.Add("ReceiptNO");
            dt.Columns.Add("Receipt_Details_ID");
            dt.Columns.Add("ReceiptDate");
            dt.Columns.Add("AccountDescription");
            dt.Columns.Add("ReceiptAmount");
            dt.Columns.Add("UnusedAmount");
            dt.Columns.Add("App_Amount");
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            grvMapping.DataSource = dt;
            grvMapping.DataBind();
            grvMapping.Rows[0].Visible = false;
            dt.Rows[0].Delete();
            ViewState["MappTable"] = dt;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to initiate the row");
        }
    }

    protected void FunProSetInitialRow()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SNo");
            dt.Columns.Add("Invoiceno");
            dt.Columns.Add("InvoiceDate");
            dt.Columns.Add("PartyName");
            dt.Columns.Add("MaturityDate");
            dt.Columns.Add("InvoiceAmount");
            dt.Columns.Add("EligibleAmount");
            dr = dt.NewRow();
            dr["SNo"] = 1;
            dr["Invoiceno"] = string.Empty;
            dr["InvoiceDate"] = string.Empty;
            dr["PartyName"] = string.Empty;
            dr["MaturityDate"] = string.Empty;
            dr["InvoiceAmount"] = string.Empty;
            dr["EligibleAmount"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["currenttable"] = dt;
            GRVFIL.DataSource = dt;
            GRVFIL.DataBind();
            GRVFIL.Rows[0].Visible = false;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to initiate the row");
        }
    }

    protected void FunProGetCustomerDetails()
    {
        try
        {
            Dictionary<string, string> dictFacInvoice = new Dictionary<string, string>();
            dictFacInvoice.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictFacInvoice.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictFacInvoice.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            dictFacInvoice.Add("@PANum", Convert.ToString(ddlPAN.SelectedValue));
            DataTable dtFactoringInvoiceDetails = Utility.GetDefaultData("S3G_LOANAD_GetCustomerDetailsByPAN", dictFacInvoice);
            if (dtFactoringInvoiceDetails.Rows.Count >= 1)
            {
                DataRow dtRow = dtFactoringInvoiceDetails.Rows[0];
                hidcuscode.Value = S3GCustomerAddress1.CustomerId = dtRow["Customer_ID"].ToString();

                S3GCustomerAddress1.SetCustomerDetails(dtRow, true);
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to get Customer details");
        }
    }

    protected bool FunProGetFactoringInvoiceDetails()
    {
        try
        {
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (strMode == "M" || strMode == "Q")
            {
                dictParam.Add("@FIL_ID", Convert.ToString(intFILID));
                DS = Utility.GetDataset("S3G_LOANAD_GetFactoringRetirement_Modify", dictParam);
            }
            else
            {
                dictParam.Add("@FIL_ID", Convert.ToString(ddlFactdocno.SelectedValue));
                DS = Utility.GetDataset("S3G_LOANAD_GetFactoringRetirement", dictParam);
            }

            if (DS.Tables[0].Rows.Count >= 1)
            {
                int intSuffix = 0;
                if (ObjS3GSession.ProGpsSuffixRW > 2)
                {
                    intSuffix = 2;
                }
                else
                {
                    intSuffix = ObjS3GSession.ProGpsSuffixRW;
                }

                txtFILNo.Text = DS.Tables[0].Rows[0]["Fil_No"].ToString();
                DateTime Date = Convert.ToDateTime(DS.Tables[0].Rows[0]["Fil_Date"]);
                txtFILDate.Text = Date.ToString(strDateFormat);
                if (strMode == "M" || strMode == "Q")
                {
                    ddlLOB.Items.Add(new ListItem(DS.Tables[0].Rows[0]["LOB_Name"].ToString(), DS.Tables[0].Rows[0]["LOB_ID"].ToString()));
                }
                ddlLOB.SelectedValue = DS.Tables[0].Rows[0]["LOB_ID"].ToString();
                ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
                //ddlBranch.SelectedValue = DS.Tables[0].Rows[0]["Branch_ID"].ToString();
                ddlBranch.SelectedText = DS.Tables[0].Rows[0]["Location_Name"].ToString();
                ddlBranch.SelectedValue = DS.Tables[0].Rows[0]["Location_ID"].ToString();
                ddlBranch.ToolTip = ddlBranch.SelectedText;

                txtPAN.Text = DS.Tables[0].Rows[0]["PANum"].ToString();
                txtSAN.Text = DS.Tables[0].Rows[0]["SANum"].ToString();
                txtCreditDays.Text = DS.Tables[0].Rows[0]["Credit_Days"].ToString();
                txtInvoiceLoadValue.Text = DS.Tables[0].Rows[0]["Invoice_Load_Value"].ToString();
                txtStatus.Text = DS.Tables[0].Rows[0]["Status"].ToString();
                txtMargin.Text = DS.Tables[0].Rows[0]["Margin"].ToString();
                txtOutStandingAmount.Text = DS.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                txtCreditLimit.Text = DS.Tables[0].Rows[0]["Credit_Limit"].ToString();

                if (Convert.ToDecimal(DS.Tables[0].Rows[0]["CreditAvailable"].ToString()) >
                    Convert.ToDecimal(DS.Tables[0].Rows[0]["Credit_Limit"].ToString()))
                {
                    txtCreditAvailable.Text = Convert.ToString(Convert.ToDecimal(DS.Tables[0].Rows[0]["Credit_Limit"].ToString()) -
                        Convert.ToDecimal(DS.Tables[0].Rows[0]["Used_Invoice"].ToString()));
                }
                else
                {
                    txtCreditAvailable.Text = Convert.ToString(Convert.ToDecimal(DS.Tables[0].Rows[0]["CreditAvailable"].ToString()) -
                        Convert.ToDecimal(DS.Tables[0].Rows[0]["Used_Invoice"].ToString()));
                }

                if (Convert.ToDecimal(txtCreditAvailable.Text) < 0)
                {
                    txtCreditAvailable.Text = "0";
                }

                txtCreditAvailable.Text = Math.Round(Convert.ToDecimal(txtCreditAvailable.Text), intSuffix).ToString();
                txtOutStandingAmount.Text = Math.Round(Convert.ToDecimal(txtOutStandingAmount.Text), intSuffix).ToString();

                if (strMode == "M" || strMode == "Q")
                {
                    ListItem lst = new ListItem(DS.Tables[0].Rows[0]["Fil_No"].ToString(), DS.Tables[0].Rows[0]["Factoring_Inv_Load_ID"].ToString());
                    ddlFactdocno.Items.Add(lst);
                    ddlFactdocno.SelectedValue = DS.Tables[0].Rows[0]["Factoring_Inv_Load_ID"].ToString();
                }

                //else if (Convert.ToDecimal(DS.Tables[0].Rows[0]["CreditAvailable"].ToString()) > Convert.ToDecimal(txtCreditLimit.Text))
                //{
                //    txtCreditAvailable.Text = txtCreditLimit.Text;
                //}
                //else
                //{
                //    txtCreditAvailable.Text = DS.Tables[0].Rows[0]["CreditAvailable"].ToString();
                //}
                hidcuscode.Value = S3GCustomerAddress1.CustomerId = DS.Tables[0].Rows[0]["Customer_ID"].ToString();
                S3GCustomerAddress1.SetCustomerDetails(DS.Tables[0].Rows[0], true);
            }
            if (DS.Tables[1].Rows.Count >= 1)
            {
                GRVFIL.Visible = true;
                GRVFIL.DataSource = DS.Tables[1];
                GRVFIL.DataBind();
                ViewState["currenttable"] = DS.Tables[1];
            }
            else
            {
                GRVFIL.DataSource = null;
                GRVFIL.DataBind();
                ViewState["currenttable"] = null;
            }

            if (strMode == "M" || strMode == "Q")
            {
                if (DS.Tables[2].Rows.Count >= 1)
                {
                    grvMapping.DataSource = DS.Tables[2];
                    grvMapping.DataBind();
                    ViewState["MappTable"] = DS.Tables[2];
                }
                else
                {
                    FunProSetInitialMappingRow();
                }
            }
            else
            {
                FunProSetInitialMappingRow();
            }

            return Convert.ToBoolean(DS.Tables[0].Rows[0]["Can_Modify"].ToString());
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to get Factoring Details");
        }
    }

    protected void FunProLoadFactdocno()
    {
        DataSet DS = new DataSet();
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
        dictParam.Add("@User_ID", Convert.ToString(intUserID));

        ddlFactdocno.BindDataTable("S3G_LOANAD_GetFactoringDocNo", dictParam, new string[] { "Factoring_Inv_Load_ID", "Fil_No" });


    }



    protected void FunProLoadPrimeAccNo()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            dictParam.Add("@Type", "Type6");
            dictParam.Add("@Param1", "1");
            dictParam.Add("@Is_Active", "1");

            if (PageMode == PageModes.Create)
            {
                ddlPAN.BindDataTable("S3G_LOANAD_GetPLASLA_List_for_FIL", dictParam, new string[] { "PANum", "PANum" });
            }
            else
            {
                ddlPAN.BindDataTable("S3G_LOANAD_GetPLASLA_List", dictParam, new string[] { "PANum", "PANum" });
            }
            ddlPAN.AddItemToolTip();
            ddlPAN.ToolTip = ddlPAN.SelectedItem.Text;
            ViewState["PANumDt"] = (DataTable)ddlPAN.DataSource;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to load Prime Account Number");
        }
    }

    protected void FunProGetAccountDetails(bool FromSLA)
    {
        try
        {
            DataSet dtSet = new DataSet();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", intCompanyID.ToString());
            if (FromSLA)
            {
                dictParam.Add("@SANum", ddlSAN.SelectedValue.ToString());
            }
            else
            {
                dictParam.Add("@SANum", ddlPAN.SelectedValue.ToString() + "DUMMY");
            }
            dictParam.Add("@PANum", ddlPAN.SelectedValue);

            dtSet = Utility.GetTableValues(SPNames.S3G_LOANAD_GetPASARefID, dictParam);
            if (dtSet.Tables[0].Rows.Count > 0)
            {
                ViewState["AccountDate"] = dtSet.Tables[0].Rows[0]["Creation_Date"].ToString();
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to get Account details");
        }
    }

    protected void FunProLoadSubAccNo()
    {
        try
        {
            //Sub A/c Number
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
            dictParam.Add("@Location_ID", ddlBranch.SelectedValue);
            dictParam.Add("@Param2", ddlPAN.SelectedItem.Text);
            dictParam.Add("@Type", "Type6");
            dictParam.Add("@Param1", "2");

            DataSet Dset = new DataSet();

            if (PageMode == PageModes.Create)
            {
                Dset = Utility.GetDataset("S3G_LOANAD_GetPLASLA_List_for_FIL", dictParam);
                ddlSAN.BindDataTable(Dset.Tables[0], new string[] { "SANum", "SANum" });
            }
            else
            {
                Dset = Utility.GetDataset("S3G_LOANAD_GetPLASLA_List", dictParam);
                ddlSAN.BindDataTable(Dset.Tables[0], new string[] { "SANum", "SANum" });
            }
            ddlSAN.AddItemToolTip();
            string IsBaseMLA = "";
            if (PageMode == PageModes.Create && Dset != null)
            {
                IsBaseMLA = Convert.ToString(Dset.Tables[1].Rows[0]["MLA_Applicable"]);

                if (IsBaseMLA != "" && IsBaseMLA == "1")
                {
                    if (ddlSAN.Items.Count == 1)
                    {
                        FunProClearControls(true);
                        ddlPAN.SelectedIndex = -1;
                        Utility.FunShowAlertMsg(this.Page, "Sub account not exists. Cannot proceed further.");
                        return;
                    }
                    lblsubAccno.CssClass = "styleReqFieldLabel";
                    RFVSLA.Enabled = RFVSLA1.Enabled = true;
                }
                else
                {
                    lblsubAccno.CssClass = "styleDisplayLabel";
                    RFVSLA.Enabled = RFVSLA1.Enabled = false;
                    FunProGetAccountDetails(false);
                }
            }
            ddlSAN.ToolTip = ddlSAN.SelectedItem.Text;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to load Sub Account Number");
        }
    }

    protected void FunProGetFactDetails()
    {
        try
        {
            s = "";
            Dictionary<string, string> dictFacInvoice = new Dictionary<string, string>();
            dictFacInvoice.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictFacInvoice.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictFacInvoice.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            dictFacInvoice.Add("@PANum", Convert.ToString(ddlPAN.SelectedValue));
            if (ddlSAN.Items.Count == 0 || ddlSAN.SelectedValue == "0")
            {
                dictFacInvoice.Add("@SANum", ddlPAN.SelectedValue + "DUMMY");
            }
            else
            {
                dictFacInvoice.Add("@SANum", Convert.ToString(ddlSAN.SelectedValue));
            }
            DataSet dsFactoringInvoiceDetails = new DataSet();
            dsFactoringInvoiceDetails = Utility.GetDataset("S3G_LOANAD_GetFactoringDetails", dictFacInvoice);

            DataTable dtFactoringInvoiceDetails = dsFactoringInvoiceDetails.Tables[0];
            if (dtFactoringInvoiceDetails.Rows.Count >= 1)
            {
                DataRow dtRow = dtFactoringInvoiceDetails.Rows[0];
                txtStatus.Text = dtRow["Status"].ToString();
                txtMargin.Text = dtRow["Margin"].ToString() != "" ? dtRow["Margin"].ToString() : "0";
                txtOutStandingAmount.Text = dtRow["OutStandingAmount"].ToString();
                txtCreditLimit.Text = dtRow["Credit_Limit"].ToString();
                if (Convert.ToDecimal(dtRow["CreditAvailable"].ToString()) < 0)
                {
                    txtCreditAvailable.Text = "0";
                }
                else if (Convert.ToDecimal(dtRow["CreditAvailable"].ToString()) > Convert.ToDecimal(txtCreditLimit.Text))
                {
                    txtCreditAvailable.Text = txtCreditLimit.Text;
                }
                else
                {
                    txtCreditAvailable.Text = dtRow["CreditAvailable"].ToString();
                }
                //txtCreditAvailable.Text = Math.Round(Convert.ToDecimal(dtRow["CreditAvailable"].ToString()), 3).ToString();
                txtCreditDays.Text = dsFactoringInvoiceDetails.Tables[1].Rows[0][0].ToString();
                btnGo.Enabled = true;
            }
            else
            {
                btnGo.Enabled = false;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to load get Factoring Details");
        }
    }

    protected void FunProClearControls()
    {
        try
        {
            txtPAN.Text = txtSAN.Text =
            txtFILNo.Text = txtFILDate.Text = txtInvoiceLoadValue.Text = txtMargin.Text = txtOutStandingAmount.Text = txtStatus.Text = txtCreditLimit.Text = txtCreditDays.Text = txtCreditAvailable.Text = lblErrorMessage.Text = String.Empty;
            S3GCustomerAddress1.ClearCustomerDetails();
            GRVFIL.Visible = false;
            grvMapping.DataSource = GRVFIL.DataSource = null;
            grvMapping.DataBind();
            GRVFIL.DataBind();

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to clear the values");
        }
    }

    protected string FunProGetColumnValue(DropDownList MyDLL, string strColumnName, DataTable Dt)
    {
        try
        {
            if (Dt != null)
            {
                DataRow[] DRows = Dt.Select(Convert.ToString(MyDLL.DataValueField) + " like '" + Convert.ToString(MyDLL.SelectedValue) + "%'");

                foreach (DataRow dr in DRows)
                {
                    return Convert.ToString(dr[strColumnName]);
                }
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlFactdocno_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            //FunProIntializeData();
            //FunPriInitialRow();
            if (ddlFactdocno.SelectedValue == "0")
            {
                FunProClearControls();
                return;
            }

            FunProGetFactoringInvoiceDetails();

        }
        catch (Exception objException)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }

    }


    #endregion

    protected void GRVFIL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblInvoiceDate = (Label)e.Row.FindControl("lblInvoiceDate");
            Label lblMaturityDate = (Label)e.Row.FindControl("lblMaturityDate");

            if (lblInvoiceDate.Text != "")
            {
                lblInvoiceDate.Text = DateTime.Parse(lblInvoiceDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            }
            if (lblMaturityDate.Text != "")
            {
                lblMaturityDate.Text = DateTime.Parse(lblMaturityDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            }
        }
    }

    protected void grvMapping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                DataTable dtMappTable = (DataTable)ViewState["MappTable"];

                DropDownList ddlReceiptNo = (DropDownList)grvMapping.FooterRow.FindControl("ddlReceiptNo");
                Label lblReceiptDateF = (Label)grvMapping.FooterRow.FindControl("lblReceiptDateF");
                Label lblReceiptAmountF = (Label)grvMapping.FooterRow.FindControl("lblReceiptAmountF");
                Label lblUnusedAmountF = (Label)grvMapping.FooterRow.FindControl("lblUnusedAmountF");
                DropDownList ddlInvoiceNo = (DropDownList)grvMapping.FooterRow.FindControl("ddlInvoiceNo");
                Label lblUnreceivedAmountF = (Label)grvMapping.FooterRow.FindControl("lblUnreceivedAmountF");
                TextBox txtApprAmountF = (TextBox)grvMapping.FooterRow.FindControl("txtApprAmountF");

                if (Convert.ToDecimal(lblUnreceivedAmountF.Text) < Convert.ToDecimal(txtApprAmountF.Text))
                {
                    cvFactoring.ErrorMessage = "Appropriation Amount should not exceed Unreceived amount";
                    cvFactoring.IsValid = false;
                    return;
                }
                if (Convert.ToDecimal(lblUnusedAmountF.Text) < Convert.ToDecimal(txtApprAmountF.Text))
                {
                    cvFactoring.ErrorMessage = "Appropriation Amount should not exceed Receipt amount";
                    cvFactoring.IsValid = false;
                    return;
                }


                dr = dtMappTable.NewRow();
                dr["SNo"] = dtMappTable.Rows.Count + 1;

                dr["FT_Retirement_Dtl_ID"] = "0";
                dr["Factoring_Inv_Load_Details_ID"] = ddlInvoiceNo.SelectedValue;
                dr["Invoiceno"] = ddlInvoiceNo.SelectedItem.Text;
                dr["UnreceivedAmount"] = lblUnreceivedAmountF.Text;
                dr["ReceiptNO"] = ddlReceiptNo.SelectedItem.Text;
                dr["Receipt_Details_ID"] = ddlReceiptNo.SelectedValue;
                dr["ReceiptDate"] = lblReceiptDateF.Text;
                dr["ReceiptAmount"] = lblReceiptAmountF.Text;
                dr["UnusedAmount"] = lblUnusedAmountF.Text;
                dr["App_Amount"] = txtApprAmountF.Text;

                dtMappTable.Rows.Add(dr);
                grvMapping.DataSource = dtMappTable;
                grvMapping.DataBind();
                ViewState["MappTable"] = dtMappTable;
            }
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void grvMapping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlInvoiceNo = (DropDownList)e.Row.FindControl("ddlInvoiceNo");
            DropDownList ddlReceiptNo = (DropDownList)e.Row.FindControl("ddlReceiptNo");

            DataTable dt = ((DataTable)ViewState["currenttable"]).Copy();
            DataRow[] dRows = dt.Select("BalanceAmount > 0");

            DataTable dtDDL = dt.Clone();
            dRows.CopyToDataTable<DataRow>(dtDDL, LoadOption.OverwriteChanges);
            ddlInvoiceNo.BindDataTable(dt, new string[] { "Factoring_Inv_Load_Details_ID", "InvoiceNO" });

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@FIL_ID", ddlFactdocno.SelectedValue);
            ddlReceiptNo.BindDataTable("S3G_LOANAD_GetRetirementReceipt", dictParam, new string[] { "Receipt_Proc_Details_ID", "Receipt_No" });

            TextBox txtApprAmountF = (TextBox)e.Row.FindControl("txtApprAmountF");
            txtApprAmountF.SetDecimalPrefixSuffix(10, 4, true, "Amount");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFT_Retirement_Dtl_ID = (Label)e.Row.FindControl("lblFT_Retirement_Dtl_ID");
            Label lblUnreceivedAmount = (Label)e.Row.FindControl("lblUnreceivedAmount");
            Label lblUnusedAmount = (Label)e.Row.FindControl("lblUnusedAmount");
            LinkButton btnRemove = (LinkButton)e.Row.FindControl("btnRemove");

            if (lblFT_Retirement_Dtl_ID.Text != "0")
            {
                btnRemove.Visible = false;
                lblUnusedAmount.Text = lblUnreceivedAmount.Text = "";
            }
        }
    }

    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlInvoiceNo = (DropDownList)grvMapping.FooterRow.FindControl("ddlInvoiceNo");
        Label lblUnreceivedAmountF = (Label)grvMapping.FooterRow.FindControl("lblUnreceivedAmountF");
        DataTable dt = ((DataTable)ViewState["currenttable"]).Copy();
        DataRow[] dRows = dt.Select("Factoring_Inv_Load_Details_ID = " + ddlInvoiceNo.SelectedValue);

        decimal decEligibleAmt = 0, decReceivedAmt = 0, decApprAmount = 0; ;

        DataTable dtMap = ((DataTable)ViewState["MappTable"]).Copy();
        DataRow[] dRowsMap = dtMap.Select("Factoring_Inv_Load_Details_ID = " + ddlInvoiceNo.SelectedValue + " and FT_Retirement_Dtl_ID = 0");

        if (dRowsMap.Length > 0)
        {
            if (dtMap != null && dtMap.Rows.Count > 0)
            {
                dtMap.Columns.Add("tmpApp_Amount", typeof(decimal), "Convert(App_Amount, 'System.Decimal')");
                decApprAmount = Convert.ToDecimal(dtMap.Compute("SUM(tmpApp_Amount)", "Factoring_Inv_Load_Details_ID = " + ddlInvoiceNo.SelectedValue + " and FT_Retirement_Dtl_ID = 0"));
            }
        }

        if (dRows.Length > 0)
        {
            DataTable dtDDL = dt.Clone();
            dRows.CopyToDataTable<DataRow>(dtDDL, LoadOption.OverwriteChanges);
            decEligibleAmt = Convert.ToDecimal(dtDDL.Rows[0]["EligibleAmount"].ToString());
            decReceivedAmt = Convert.ToDecimal(dtDDL.Rows[0]["ReceivedAmount"].ToString());
        }

        lblUnreceivedAmountF.Text = (decEligibleAmt - decReceivedAmt - decApprAmount).ToString(Utility.SetSuffix(4));
    }

    protected void ddlReceiptNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlReceiptNo = (DropDownList)grvMapping.FooterRow.FindControl("ddlReceiptNo");
        Label lblReceiptDateF = (Label)grvMapping.FooterRow.FindControl("lblReceiptDateF");
        Label lblReceiptAmountF = (Label)grvMapping.FooterRow.FindControl("lblReceiptAmountF");
        Label lblUnusedAmountF = (Label)grvMapping.FooterRow.FindControl("lblUnusedAmountF");

        DataTable dt = ((DataTable)ViewState["MappTable"]).Copy();

        DataRow[] dRows = dt.Select("Receipt_Details_ID = " + ddlReceiptNo.SelectedValue + " and FT_Retirement_Dtl_ID = 0");

        decimal decApprAmount = 0;
        if (dRows.Length > 0)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("tmpApp_Amount", typeof(decimal), "Convert(App_Amount, 'System.Decimal')");
                decApprAmount = Convert.ToDecimal(dt.Compute("SUM(tmpApp_Amount)", "Receipt_Details_ID = " + ddlReceiptNo.SelectedValue + " and FT_Retirement_Dtl_ID = 0"));
            }
        }

        lblReceiptDateF.Text = lblReceiptAmountF.Text = lblUnusedAmountF.Text = string.Empty;
        if (ddlReceiptNo.SelectedValue != "0")
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@FIL_ID", ddlFactdocno.SelectedValue);
            dictParam.Add("@Grid_Amount", decApprAmount.ToString());
            dictParam.Add("@Receipt_Proc_Details_ID", ddlReceiptNo.SelectedValue);

            DataTable dtRcpt = Utility.GetDefaultData("S3G_LOANAD_GetRetirementReceipt", dictParam);

            lblReceiptDateF.Text = Convert.ToDateTime(dtRcpt.Rows[0]["Receipt_Date"].ToString()).ToString(strDateFormat);
            lblReceiptAmountF.Text = Convert.ToDecimal(dtRcpt.Rows[0]["Transaction_Amount"].ToString()).ToString(Utility.SetSuffix(4));
            lblUnusedAmountF.Text = Convert.ToDecimal(dtRcpt.Rows[0]["BalanceAmount"].ToString()).ToString(Utility.SetSuffix(4));
        }


    }

    protected void grvMapping_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtDelete;
        dtDelete = (DataTable)ViewState["MappTable"];
        dtDelete.Rows.RemoveAt(e.RowIndex);

        grvMapping.DataSource = dtDelete;
        grvMapping.DataBind();
        ViewState["MappTable"] = dtDelete;
        if (dtDelete.Rows.Count == 0)
        {
            FunProSetInitialMappingRow();
        }
    }

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
        Procparam.Add("@Program_Id", "238");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }
}