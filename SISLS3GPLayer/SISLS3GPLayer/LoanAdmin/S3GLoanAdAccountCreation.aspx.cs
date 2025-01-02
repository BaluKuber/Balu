
/// Module Name     :   Loan Admin
/// Screen Name     :   Account Creation
/// Created By      :   Prabhu.K
/// Created Date    :   04-09-2010
/// Purpose         :   To Insert , Update and Query
/// Modified By     :   Thangam M
/// Modified Date   :   23/Dec/2013
/// Purpose         :   To make entity dropdown as auto complete for in/out flow grids

using System;
using System.Data;
using S3GBusEntity;
using System.Web.UI;
using System.ServiceModel;
using System.Globalization;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Configuration;
using S3GBusEntity.Reports;
using System.IO;
using System.Linq;
using System.Data.Common;
using System.Diagnostics;
using System.Text;



public partial class S3GLoanAdAccountCreation : ApplyThemeForProject
{

    #region Variable declaration
    double intTotalamount = 0;
    string XMLCommon = string.Empty;
    string _Add = "1", _Edit = "2", strDocumentDate = "";
    int intCompanyId = 0, intUserId = 0, _SlNo = 0, _Program_ID = 0, intAccountCreationId, intResult;
    int intApplicationProcessId = 0;
    string strPANum, strSANum = "";
    int intCustEmailRows = 0;
    string strConsNumber, strSplitNum, strSplitRefNo, strSplitDate;
    public string strDateFormat;
    bool PaintBG = true;
    Dictionary<string, string> Procparam;
    Dictionary<string, string> objProcedureParameter;
    System.Data.DataTable DtAlertDetails = new System.Data.DataTable();
    System.Data.DataTable DtFollowUp = new System.Data.DataTable();
    System.Data.DataTable DtCashFlow = new System.Data.DataTable();
    System.Data.DataTable DtCashFlowOut = new System.Data.DataTable();
    System.Data.DataTable DtRepayGrid = new System.Data.DataTable();
    System.Data.DataTable DtRepaySummary = new System.Data.DataTable();
    System.Data.DataTable dtsum = new System.Data.DataTable();
    System.Data.DataTable dtDeliveryAddress = new System.Data.DataTable();
    System.Data.DataTable DtRepayGridIRR = new System.Data.DataTable();
    System.Data.DataTable DtRepayGridIRRPri = new System.Data.DataTable();
    System.Data.DataTable DtRepayGridIRRSec = new System.Data.DataTable();
    System.Data.DataTable dtCashflows = new System.Data.DataTable();
    System.Data.DataTable dtInvoicesACAT = new System.Data.DataTable();
    System.Data.DataTable dtInvoicesACATSummary = new System.Data.DataTable();

    System.Data.DataTable dtRentalDetails = new System.Data.DataTable();
    System.Data.DataTable dtReWriteAmount = new System.Data.DataTable();
    System.Data.DataTable dtmovetotal = new System.Data.DataTable();
    System.Data.DataTable dtPrimaryGrid = new System.Data.DataTable();
    System.Data.DataTable dtSecondaryGrid = new System.Data.DataTable();
    System.Data.DataTable dtEUCDetails = new System.Data.DataTable();
    System.Data.DataTable dtInvoiceGrid = new System.Data.DataTable();
    System.Data.DataTable dtSummaryInvoiceGrid = new System.Data.DataTable();

    static string strPageName = "Rental Schedule Creation";
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    // ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjAProcessSave = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;
    //ReportAccountsMgtServicesReference.ReportAccountsMgtServicesClient objSerClient;


    string strRedirectPage = "~/LoanAdmin/S3GLoanAdTransLander.aspx?Code=ACR";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strNewWin1 = "window.showModalDialog('../LoanAdmin/S3G_ORG_AccountHomeLoanAsset.aspx";
    string strNewWin = " window.showModalDialog('../LoanAdmin/S3GLoanAdAccountAssetDetails.aspx";
    string NewWinAttributes = "', 'Asset Details', 'dialogwidth:750px;dialogHeight:450px;');";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=ACR';";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    int intsales_type;
    //5093 start
    HiddenField vatleasing;
    HiddenField cstleasing;
    //5093 end
    public static S3GLoanAdAccountCreation obj_PageValue;


    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    //int intUserID = 0;


    bool bIsActive = false;

    #region "Popup paging"
    PagingValues ObjPaging = new PagingValues();

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    /// <summary>
    ///Assign Page Number
    /// </summary
    public int ProPageNumRW
    {
        get;
        set;
    }
    /// <summary>
    ///Assign Page Size
    /// </summary
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

    #region "Summary paging"

    PagingValues ObjPagingSummary = new PagingValues();

    public delegate void PageAssignValueSummary(int ProPageNumRWSummary, int intPageSizeSummary);
    /// <summary>
    ///Assign Page Number
    /// </summary
    public int ProPageNumRWSummary
    {
        get;
        set;
    }
    /// <summary>
    ///Assign Page Size
    /// </summary
    public int ProPageSizeRWSummary
    {
        get;
        set;

    }

    protected void AssignValueSummary(int intPageNum, int intPageSize)
    {
        ProPageNumRWSummary = intPageNum;
        ProPageSizeRWSummary = intPageSize;
        FunPriBindGridSummary();
    }

    #endregion



    #endregion

    //Code end
    #endregion

    #region Page Event

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //By Siva.K For Data Load Problem in Multi User
            Session["intAccountCreationId"] = null;
            Session["strSplitNum"] = null;
            Session["strConsNumber"] = null;
            // END  Siva.K For Data Load Problem in Multi User
            obj_PageValue = this;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarAccountDate.Format = strDateFormat;
            CalendarFirstinstdate.Format = strDateFormat;
            CalendarFirstinstDuedate.Format = strDateFormat;
            CalendarCommencedate.Format = strDateFormat;
            CalendarSigndate.Format = strDateFormat;

            txtAccountDate.Attributes.Add("readonly", "true");

            #region Paging Config
            ProPageNumRW = 1;
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize")); //999;

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

            //Summary paging
            ProPageNumRWSummary = 1;
            TextBox txtPageSizeSummary = (TextBox)ucCustomPagingSummary.FindControl("txtPageSize");
            if (txtPageSizeSummary.Text != "")
                ProPageSizeRWSummary = Convert.ToInt32(txtPageSizeSummary.Text);
            else
                ProPageSizeRWSummary = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize")); //999;

            PageAssignValueSummary objSummary = new PageAssignValueSummary(this.AssignValueSummary);
            ucCustomPagingSummary.callback = objSummary;
            ucCustomPagingSummary.ProPageNumRW = ProPageNumRWSummary;
            ucCustomPagingSummary.ProPageSizeRW = ProPageSizeRWSummary;
            #endregion

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            FormsAuthenticationTicket fromTicket;
            //Code end

            #region " WF INITIATION"
            ProgramCode = "080";


            #endregion

            if (Request.QueryString["qsViewId"] != null)
            {
                fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    string[] strFromTicket = fromTicket.Name.Split('~');
                    if (strFromTicket.Length == 5)
                    {
                        intApplicationProcessId = Convert.ToInt32(strFromTicket[1].ToString());
                        intCompanyId = Convert.ToInt32(strFromTicket[2].ToString());
                        strPANum = strFromTicket[3].ToString();
                        if (strFromTicket[4].ToString() != strPANum + "Dummy")
                            strSANum = strFromTicket[4].ToString();
                    }
                    intAccountCreationId = Convert.ToInt32(strFromTicket[0].ToString());
                    Session["intAccountCreationId"] = intAccountCreationId;//By Siva.K For Data Load Problem in Multi User
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Rental Schedule Creation Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];

            txtFinanceAmount.CheckGPSLength(true);
            TxtAutoExtnRental.CheckGPSLength(true);
            if (Request.QueryString["qsAccConNo"] != null)
            {
                fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsAccConNo"]);
                strConsNumber = fromTicket.Name;
                txtFinanceAmount.ReadOnly = true;
                //rfvTenureType.Enabled = true;
                //rfvTenure.Enabled = true;
                CalendarAccountDate.Enabled = false;
                btnConfigure.Enabled = false;
                Session["strConsNumber"] = strConsNumber;//By Siva.K For Data Load Problem in Multi User
            }
            if (Request.QueryString["qsAccSplitNo"] != null)
            {
                fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsAccSplitNo"]);
                strSplitNum = fromTicket.Name;
                strSplitRefNo = Request.QueryString["qsRefNo"].ToString();

                txtFinanceAmount.ReadOnly = true;
                btnConfigure.Enabled = false;
                Session["strSplitNum"] = strSplitNum;//By Siva.K For Data Load Problem in Multi User
            }

            if (!IsPostBack)
            {
                ViewState["strSplitDate"] = null;
                FunLoadDDLControls();//For proposal 
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                FunPriLoadStatusDDL();
                FillDLL_MainTab();
                //5093 start
                //vatleasing = 0;
                //cstleasing = 0;
                //5093 end
                ViewState["intsales_type"] = "2";
                FunProGetIRRDetails();
                //FunProLoadAddressCombos();
                if (intAccountCreationId == 0 && PageMode != PageModes.WorkFlow)  // TO RESTRICT THE WORK FROM BEING EXECUTED
                {
                    txtAccountDate.Text = DateTime.Now.Date.ToString(strDateFormat);
                    txtfirstinstdate.Text = DateTime.Now.Date.ToString(strDateFormat);
                    TxtFirstInstallDue.Text = DateTime.Now.Date.ToString(strDateFormat);
                    TxtCommenceDate.Text = DateTime.Now.Date.ToString(strDateFormat);
                    TxtRSSignDate.Text = DateTime.Now.Date.ToString(strDateFormat);
                    if (Request.QueryString["qsAccConNo"] == null)
                    {
                        if (Request.QueryString["qsAccSplitNo"] == null)
                        {
                            CallStartup();
                            btnCalcel.Enabled = true;
                            btnClear.Enabled = true;
                        }
                        else
                        {
                            //FunPriGetApplicationDetails(0);
                            //FunPriBindAlertDLL(_Add);
                            //FunPriBindFollowupDLL(_Add);
                            //FunPriBindMoratoriumDLL(_Add);
                            btnCalcel.Enabled = false;
                            btnClear.Enabled = false;
                        }
                    }
                    else
                    {
                        //FunPriGetApplicationDetails(0);
                        FunPriBindRepaymentDLL(_Add);
                        //FunPriBindROIDLL(_Add);
                        FunPriBindGuarantorDLL(_Add);
                        //FunPriBindAlertDLL(_Add);
                        //FunPriBindFollowupDLL(_Add);
                        //FunPriBindMoratoriumDLL(_Add);
                        btnCalcel.Enabled = false;
                        btnClear.Enabled = false;
                    }
                    FunPriDisableControls(0);
                }
                else if (intAccountCreationId > 0)
                {
                    if (strMode == "M")
                    {
                        FunPriDisableControls(1);
                    }
                    else
                    {
                        FunPriDisableControls(-1);
                    }

                }
                else if (PageMode == PageModes.WorkFlow && !IsPostBack)
                {
                    try
                    {
                        PreparePageForWorkFlowLoad();
                        //btnCalcel.Enabled = false;
                        btnClear.Enabled = false;

                        ViewState["PageMode"] = PageModes.WorkFlow;
                    }
                    catch (Exception ex)
                    {
                        ClsPubCommErrorLog.CustomErrorRoutine(ex);
                        Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
                    }
                }
                //FunPriSetRateLength();
                /* Initialized For Security Deposit CashFlow on Dec 23,2015 */
                Session["strSecuDepstDate"] = null;
                /* Initialized For Security Deposit CashFlow on Dec 23,2015 */
            }
            txtAccountIRR_Repay.Attributes.Add("readonly", "readonly");
            txtAccountingIRR.Attributes.Add("readonly", "readonly");
            txtBusinessIRR.Attributes.Add("readonly", "readonly");
            txtBusinessIRR_Repay.Attributes.Add("readonly", "readonly");
            txtCompanyIRR.Attributes.Add("readonly", "readonly");
            txtCompanyIRR_Repay.Attributes.Add("readonly", "readonly");
            //txtRepaymentTime.Attributes.Add("readonly", "true");
            if (gvRepaymentDetails.FooterRow != null)
            {
                if (strSplitNum == null)
                {
                    TextBox txtPerInstall = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
                    txtPerInstall.CheckGPSLength(false, "Per Installment Amount");

                    TextBox txtBreakPer = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;
                    txtBreakPer.SetDecimalPrefixSuffix(2, 2, false, "Break up Percentage");
                }
            }

            //Code Added by Saranya for Customer Changes
            //if (gvGuarantor.FooterRow != null)
            //{
            //    UserControls_LOBMasterView ucCustomerLov = gvGuarantor.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
            //    DropDownList ddlGuarantortype_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
            //    if (ddlGuarantortype_GuarantorTab1.SelectedIndex > 0)
            //    {
            //        if (ddlGuarantortype_GuarantorTab1.SelectedItem.Text.StartsWith("G") && ddlGuarantortype_GuarantorTab1.SelectedItem.Text.EndsWith("1"))
            //        {
            //            ucCustomerLov.strLOV_Code = "GCMD";
            //        }
            //        else if (ddlGuarantortype_GuarantorTab1.SelectedItem.Text.Contains("G") && ddlGuarantortype_GuarantorTab1.SelectedItem.Text.Contains("2"))
            //        {
            //            ucCustomerLov.strLOV_Code = "PCMD";
            //        }
            //        else
            //        {
            //            //For Co-Applicant
            //            ucCustomerLov.strLOV_Code = "COAP";
            //        }

            //    }
            //    else
            //    {
            //        ucCustomerLov.strLOV_Code = "CMD";
            //    }
            //    ucCustomerLov.strControlID = ucCustomerLov.ClientID;
            //}

            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;

            TextBox txtRsNo = (TextBox)UcCtlRsno.FindControl("txtName");
            txtRsNo.Attributes.Add("onfocus", "fnLoadRsNo()");
            UcCtlRsno.strControlID = UcCtlRsno.ClientID; // By Siva.K on 03JUN2015

            //*********START************** By Siva.K For Data Load Problem in Multi User ***********//

            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyId.ToString();
            System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = intUserId.ToString();

            if (ddlLOB.SelectedValue != "0")
            {
                System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = ddlLOB.SelectedValue;
                System.Web.HttpContext.Current.Session["LOBAutoSuggestText"] = ddlLOB.SelectedItem.Text;
            }
            else
            {
                System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = null;
            }

            if (ddlBranchList.SelectedValue != "0")
            {
                System.Web.HttpContext.Current.Session["LocationAutoSuggestValue"] = ddlBranchList.SelectedValue;
                System.Web.HttpContext.Current.Session["LocationAutoSuggestText"] = ddlBranchList.SelectedText;
            }
            else
            {
                System.Web.HttpContext.Current.Session["LocationAutoSuggestValue"] = null;
            }

            System.Web.HttpContext.Current.Session["AutoSuggestUserLevelID"] = ObjUserInfo.ProUserLevelIdRW.ToString();
            if (ViewState["Customer_Id"] != null)
                System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = Convert.ToString(ViewState["Customer_Id"]);
            else
                System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = null;


            //*********END************** By Siva.K For Data Load Problem in Multi User ***********//

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Load Rental Schedule Details";
            cv_TabMainPage.IsValid = false;
        }
    }


    #region Workflow Methods
    /// <summary>
    /// Workflow Function
    /// </summary>
    private void PreparePageForWorkFlowLoad()
    {
        ContractMgtServicesReference.ContractMgtServicesClient objContractMgtserviceClient = new ContractMgtServicesReference.ContractMgtServicesClient();

        try
        {

            if (!IsPostBack)
            {
                WorkFlowSession WFSessionValues = new WorkFlowSession();
                // Get The IDVALUE from Document Sequence #
                DataRow HeaderValues = GetHeaderDetailsFromDOCNo(WFSessionValues.WorkFlowDocumentNo, ProgramCode);
                //FunPriLoadLOBBranchDDL();
                FillDLL_MainTab();
                FunPriLoadApplicationNoDDL(int.Parse(HeaderValues["Location_ID"].ToString()), int.Parse(HeaderValues["LOB_ID"].ToString()));
                ddlLOB.SelectedValue = HeaderValues["LOB_ID"].ToString();
                ddlBranchList.SelectedValue = HeaderValues["Location_ID"].ToString();
                //ddlBranchList.ClearDropDownList();
                ddlApplicationReferenceNo.SelectedValue = HeaderValues["UniqueId"].ToString();
                //ddlApplicationReferenceNo.ClearDropDownList();
                FunProGetIRRDetails();
                byte[] ObjPricingDataTable = objContractMgtserviceClient.FunPubGetMLASLAApplicable(Convert.ToInt32(ddlLOB.SelectedValue), intCompanyId);
                System.Data.DataSet dsMLASLAApplicable = (System.Data.DataSet)ClsPubSerialize.DeSerialize(ObjPricingDataTable, SerializationMode.Binary, typeof(System.Data.DataSet));
                if (dsMLASLAApplicable.Tables.Count > 0)
                {
                    if (dsMLASLAApplicable.Tables[0].Rows.Count > 0)
                    {
                        string strMLASLAApplicable = dsMLASLAApplicable.Tables[0].Rows[0][0].ToString();
                        ViewState["strMLASLAApplicable"] = strMLASLAApplicable;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Define the Rental Schedule Details in System Admin Global Parameter");
                    return;
                }
                txtAccountDate.Text = DateTime.Now.Date.ToString(strDateFormat);
                //FunPriGetApplicationDetails(int.Parse(HeaderValues["UniqueId"].ToString()));
                //if (ddlLOB.SelectedIndex > 0) ddlLOB.ClearDropDownList();
                //Removed By Shibu 17-Sep-2013
                //if (ddlBranchList.SelectedIndex > 0) ddlBranchList.ClearDropDownList();
                if (ddlBranchList.SelectedValue != "0") ddlBranchList.Clear();
                //if (ddlApplicationReferenceNo.SelectedIndex > 0) ddlApplicationReferenceNo.ClearDropDownList();

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objContractMgtserviceClient.Close();
        }


    }
    #endregion

    /*
    /// <summary>
    /// Event for Pre-Initialize the Components  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    
    protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["IsFromAccount"] != null)
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
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
            if (Request.QueryString["IsFromAccount"] != null)
            {
                throw objException;
            }
            else
            {
                cv_TabMainPage.ErrorMessage = "Unable to Initialise the Controls in Account";
                cv_TabMainPage.IsValid = false;
            }
        }
    }
    */
    #region "User Authorization"

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
                btnAcccountCancel.Visible = false;
                if (strSplitNum != null)
                {
                    //if (gvAssetDetails.Rows.Count > 0)
                    //{
                    //    gvAssetDetails.Columns[7].Visible = false;
                    //}
                }
                if (strSplitNum != null)
                {
                    FunPriLoadAccountDetails_Split();
                    FunGenerateSchedule();
                }
                chkSecondary.Visible = lblReport_Format.Visible = btnPrint.Visible = ddlReportType.Visible = false;
                break;

            case 1: // Modify Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                ucCustomerCodeLov.ButtonEnabled = false;
                UcCtlRsno.ButtonEnabled = false; //By Siva.K on 09JUN2015
                chkSecondary.Visible = lblReport_Format.Visible = btnPrint.Visible = ddlReportType.Visible = true;
                //FunPriGetApplicationDetails(intApplicationProcessId);
                FunPriLoadAccountDetails(intAccountCreationId);
                btnAcccountCancel.Visible = true;
                if (txtStatus.Text.StartsWith("6 ") || txtStatus.Text.StartsWith("7 ") || txtStatus.Text.StartsWith("0") || txtStatus.Text.StartsWith("3") || 
                    txtStatus.Text.StartsWith("11") || txtStatus.Text.StartsWith("13") || txtStatus.Text.StartsWith("46 "))
                {
                    chkIsSecondary.Enabled = chkApplicable.Enabled = chkApplicableSec.Enabled = false;
                    btnConfigure.Enabled = btnSave.Enabled = false;
                    btnAcccountCancel.Visible = false;
                    ddlRepaymentMode.ClearDropDownList();
                    //txtRepaymentTime.ReadOnly = txtLastODICalcDate.ReadOnly =
                    //txtFBDate.ReadOnly = 
                    //    //txtAdvanceInstallments.ReadOnly = true;
                    //chkDORequired.Enabled = false;
                    grvConsDocuments.Enabled = false;
                    if (gvInflow.Rows.Count > 0)
                    {
                        gvInflow.FooterRow.Visible = gvInflow.Columns[9].Visible = false;
                    }
                    gvOutFlow.FooterRow.Visible = gvOutFlow.Columns[9].Visible =
                    gvRepaymentDetails.FooterRow.Visible = gvRepaymentDetails.Columns[8].Visible = false;
                    //gvAlert.FooterRow.Visible = gvAlert.Columns[6].Visible =
                    //gvFollowUp.FooterRow.Visible = gvFollowUp.Columns[9].Visible = false;
                    btnCalIRR.Enabled = btnReset.Enabled = false;

                    //txtSLACustomerCode.Attributes.Add("readonly", "true");
                    //txtSLAUserName.Attributes.Add("readonly", "true");
                    //txtComAddress1.Attributes.Add("readonly", "true");
                    //txtCOmAddress2.Attributes.Add("readonly", "true");
                    //txtComState.Enabled = txtComCountry.Enabled = txtComCity.Enabled = false;
                    //txtComPincode.Attributes.Add("readonly", "true");
                    //txtComMobile.Attributes.Add("readonly", "true");
                    //txtComPhone.Attributes.Add("readonly", "true");
                    //txtComEMail.Attributes.Add("readonly", "true");
                    //txtComWebsite.Attributes.Add("readonly", "true");
                }
                if (btnAcccountCancel.Visible)
                {
                    // txtBusinessIRR_Repay.Text = ""; COMMENTED ON 4-11-2011 FOR SAVING ACCOUNT CREATION IN MODIFY MODE.
                }
                tcAccountCreation.ActiveTabIndex = 0;
                btnClear.Enabled = false;
                ddlLOB.ClearDropDownList();
                //Removed By Shibu 17-Sep-2013
                //ddlBranchList.ClearDropDownList();
                //ddlBranchList.Clear();
                //ddlBranchList.ReadOnly = true;
                //ddlApplicationReferenceNo.ClearDropDownList();
                //ddlApplicationReferenceNo.Clear();
                // Modified By : Anbuvel.T,Date:11-Jan-2016, Description : OPC_CR_025(CL_904 & 905) Proposal Number Enabled Option Done.
                if (txtStatus.Text.StartsWith("2 "))
                {
                    ddlApplicationReferenceNo.ReadOnly = false;
                }
                else
                {
                    ddlApplicationReferenceNo.ReadOnly = true;
                }
                if (ddlPaymentRuleList.Items.Count > 0)
                {
                    ddlPaymentRuleList.ClearDropDownList();
                }
                // SalePerson DDL Removed By Shibu 17-Sep-2013
                //ddlSalePersonCodeList.ClearDropDownList();
                //ddlSalePersonCodeList.ReadOnly = true;
                //By Siva.K on 16JUN2015 for  below field can be modify
                //ddlAccountManager1.ReadOnly = ddlAccountManager2.ReadOnly = ddlRegionalManager.ReadOnly = true; siva
                txtTenure.ReadOnly = txtFinanceAmount.ReadOnly = true;
                //txtMarginMoneyAmount_Cashflow.ReadOnly = true;
                //txtMarginMoneyAmount_Cashflow.Attributes.Add("readonly", "true");
                //txtMarginMoneyPer_Cashflow.Attributes.Add("readonly", "true");
                //txtResidualAmt_Cashflow.Attributes.Add("readonly", "true");
                //txtResidualValue_Cashflow.Attributes.Add("readonly", "true");
                //if (txtStatus.Text.StartsWith("12"))
                //{
                //    txtSLACustomerCode.Attributes.Add("readonly", "true");
                //    txtSLAUserName.Attributes.Add("readonly", "true");
                //    txtComAddress1.Attributes.Add("readonly", "true");
                //    txtCOmAddress2.Attributes.Add("readonly", "true");
                //    txtComState.Enabled = txtComCountry.Enabled = txtComCity.Enabled = false;
                //    txtComPincode.Attributes.Add("readonly", "true");
                //    txtComMobile.Attributes.Add("readonly", "true");
                //    txtComPhone.Attributes.Add("readonly", "true");
                //    txtComEMail.Attributes.Add("readonly", "true");
                //    txtComWebsite.Attributes.Add("readonly", "true");
                //}
                if (gvGuarantor.Rows.Count > 0)
                {
                    gvGuarantor.FooterRow.Visible = gvGuarantor.Columns[6].Visible = false;
                }
                //if (gvMoratorium.Rows.Count > 0)
                //{
                //    gvMoratorium.FooterRow.Visible = gvMoratorium.Columns[4].Visible = false;
                //}
                //if (gvAssetDetails.Rows.Count > 0)
                //{
                //    gvAssetDetails.Columns[7].Visible = false;
                //}
                ////added by saranya
                //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") && ddl_Repayment_Mode.SelectedItem.Text.StartsWith("Pro")))
                //{
                //    tcAccountCreation.Tabs[2].Enabled = false;
                //}
                ////end
                btnPrint.Enabled = true;
                //if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper() != "PRODUCT"))
                //{
                //    grvRepayStructure.Columns[5].Visible = false;
                //    //grvRepayStructure.Columns[6].Visible = true;
                //    //grvRepayStructure.Columns[7].Visible = true;
                //}
                //else
                //{
                //    grvRepayStructure.Columns[5].Visible = true;
                //    grvRepayStructure.Columns[6].Visible = false;
                //    grvRepayStructure.Columns[7].Visible = false;
                //}
                //if (ddlDeliveryState.Items.Count > 0)
                //{
                //    ddlDeliveryState.ClearDropDownList();
                //}
                if (!bDelete)
                    btnAcccountCancel.Visible = false;
                ddlLientContract.Enabled = false;

                if (txtStatus.Text.StartsWith("3 ") || txtStatus.Text.StartsWith("46 "))
                    goto QMode;

                break;


            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnSave.Enabled = false;
                ucCustomerCodeLov.ButtonEnabled = false;
                UcCtlRsno.ButtonEnabled = false; //By Siva.K on 09JUN2015
                chkSecondary.Visible = lblReport_Format.Visible = btnPrint.Visible = ddlReportType.Visible = true;
                //FunPriGetApplicationDetails(intApplicationProcessId);
                FunPriLoadAccountDetails(intAccountCreationId);

            QMode:

                tcAccountCreation.ActiveTabIndex = 0;

                CalendarFirstinstdate.Enabled = false;
                CalendarFirstinstDuedate.Enabled = false;
                CalendarCommencedate.Enabled = false;
                CalendarSigndate.Enabled = false;
                ddlDeliveryState.ClearDropDownList();
                ddlBillingState.ClearDropDownList();
                chkimport.Enabled = chkSEZ.Enabled = chkWithIGST.Enabled = false;
                if (grvRentalDetails != null)
                    grvRentalDetails.Columns[grvRentalDetails.Columns.Count - 1].Visible = false;

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }
                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    //Removed By Shibu 17-Sep-2013
                    //ddlBranchList.ClearDropDownList();
                    //ddlBranchList.Clear();
                    ddlBranchList.ReadOnly = true;
                    //ddlApplicationReferenceNo.ClearDropDownList();
                    //ddlApplicationReferenceNo.Clear();
                    ddlApplicationReferenceNo.ReadOnly = true;
                    // SalePerson DDL Removed By Shibu 17-Sep-2013
                    //ddlSalePersonCodeList.ClearDropDownList();
                    //ddlSalePersonCodeList.Clear();
                    //ddlSalePersonCodeList.ReadOnly = true;
                    ddlTenureType.ClearDropDownList();
                    //ddlROIRuleList.ClearDropDownList();
                    if (ddlPaymentRuleList.Items.Count > 0)
                    {
                        ddlPaymentRuleList.ClearDropDownList();
                    }

                    ddlRepaymentMode.ClearDropDownList();

                }

                //txtComState.ReadOnly = txtComCity.ReadOnly = txtComCountry.ReadOnly = 

                //txtComState.DropDownStyle = txtComCity.DropDownStyle = txtComCountry.DropDownStyle =
                //     AjaxControlToolkit.ComboBoxStyle.DropDownList;
                //txtComCity.ClearDropDownList();
                //txtComState.ClearDropDownList();
                //txtComCountry.ClearDropDownList();

                //txtTenure.ReadOnly = txtFinanceAmount.ReadOnly = txtMarginMoneyAmount_Cashflow.ReadOnly =
                //txtMarginMoneyPer_Cashflow.ReadOnly = txtResidualAmt_Cashflow.ReadOnly = txtResidualValue_Cashflow.ReadOnly =
                //txtFBDate.ReadOnly = txtAdvanceInstallments.ReadOnly = txtLastODICalcDate.ReadOnly =
                //txtSLACustomerCode.ReadOnly = txtSLAUserName.ReadOnly = txtComAddress1.ReadOnly =
                //txtCOmAddress2.ReadOnly = txtRepaymentTime.ReadOnly =
                //txtComPincode.ReadOnly = true;
                //chkDORequired.Enabled =
                TxtAutoExtnRental.ReadOnly = true;
                CalendarAccountDate.Enabled = false;
                grvConsDocuments.Enabled = false;
                if (gvInflow.Rows.Count > 0)
                {
                    gvInflow.FooterRow.Visible = gvInflow.Columns[9].Visible = false;
                }
                gvOutFlow.FooterRow.Visible = false;
                gvOutFlow.Columns[9].Visible = false;
                if (gvRepaymentDetails.FooterRow != null)
                    gvRepaymentDetails.FooterRow.Visible = false;
                gvRepaymentDetails.Columns[8].Visible = false;
                if (gvGuarantor.Rows.Count > 0)
                {
                    gvGuarantor.FooterRow.Visible = gvGuarantor.Columns[6].Visible = false;
                }
                //gvAlert.FooterRow.Visible = gvAlert.Columns[6].Visible =
                //gvFollowUp.FooterRow.Visible = gvFollowUp.Columns[9].Visible = false;
                //if (gvMoratorium.Rows.Count > 0)
                //{
                //    gvMoratorium.FooterRow.Visible = gvMoratorium.Columns[4].Visible = false;
                //}

                btnCalIRR.Enabled = btnReset.Enabled = btnConfigure.Enabled = btnClear.Enabled = false;
                btnAcccountCancel.Visible = false;

                btnPrint.Enabled = true;

                chkimport.Enabled = chkSEZ.Enabled = chkSEZA1.Enabled = chkcstwith.Enabled = chkCSTDeal.Enabled = false;
                ddlSEZZone.Enabled = false;
                txtCFormNo.Enabled = false;
                chkAmfsold.Enabled = chkVATSold.Enabled = chkServiceTaxSold.Enabled = chkApplicable.Enabled = chkApplicableSec.Enabled = chkFullRental.Enabled = false;

                pnlAddtionalTax.Enabled = false;
                //ddlSalesTax.ClearDropDownList();
                txtCFormNo.ReadOnly = true;
                ddlSEZZone.Enabled = false;
                btnInvoices.Visible = false;
                //txtAddress2DA.ReadOnly = txtCityDA.ReadOnly =
                //    txtCountryDA.ReadOnly = txtPinCodeDA.ReadOnly = 
                //ddlState.ClearDropDownList();
                btnAddEUC.Visible = btnModifyEUC.Visible = btnhClearEUC.Visible = false;

                //ddlAssetCategoryEUC.Enabled = false;
                txtCustomerName_EUC.ReadOnly = txtEmailId_EUC.ReadOnly = txtRemarks_EUC.ReadOnly = true;

                //grvSummaryInvoices.Columns[grvSummaryInvoices.Columns.Count - 1].Visible = false;
                ((LinkButton)grvSummaryInvoices.Controls[0].Controls[0].FindControl("lnRemoveAll")).Visible = false;
                foreach (RepeaterItem item in grvSummaryInvoices.Items)
                {
                    LinkButton LkButton = (LinkButton)item.FindControl("lnRemoveRepayment");
                    LkButton.Visible = false;
                }
                pnlPrimaryGrid.Enabled = false;
                pnlSecondaryGrid.Enabled = false;
                PnlExtension.Enabled = false;
                if (intModeID != 1)
                {
                    ddlAccountManager1.ReadOnly = ddlAccountManager2.ReadOnly = ddlRegionalManager.ReadOnly = true;
                    ddlDeliveryType.ClearDropDownList();
                    txtAddress1DA.ReadOnly = txtTelephoneDA.ReadOnly = txtMobileDA.ReadOnly = txtGSTIN.ReadOnly = txtLabel.ReadOnly = txtAddress.ReadOnly = txtPin.ReadOnly = true;
                }

                grvEUC.Columns[grvEUC.Columns.Count - 1].Visible = false;

                ddlLientContract.Enabled = false;
                break;

        }

    }

    #endregion

    private void CallStartup()
    {
        try
        {
            //FunPriBindROIDLL(_Add);
            //FunPriBindAlertDLL(_Add);
            //FunPriBindFollowupDLL(_Add);
            FunPriBindInflowDLL(_Add);
            FunPriBindOutflowDLL(_Add);
            FunPriBindRepaymentDLL(_Add);
            FunPriBindGuarantorDLL(_Add);
            //FunPriBindMoratoriumDLL(_Add);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion

    #region Page Methods

    private void ClearForms()
    {
        try
        {
            txtAccountDate.Text = DateTime.Now.Date.ToString(strDateFormat);
            //By Siva.k on 10JUN2015 Clear the Dates
            txtfirstinstdate.Text = DateTime.Now.Date.ToString(strDateFormat);
            TxtFirstInstallDue.Text = DateTime.Now.Date.ToString(strDateFormat);
            TxtCommenceDate.Text = DateTime.Now.Date.ToString(strDateFormat);
            TxtRSSignDate.Text = DateTime.Now.Date.ToString(strDateFormat);
            //Removed By Shibu 17-Sep-2013
            //ddlBranchList.SelectedIndex = 0;
            //ddlBranchList.Clear();
            //ddlLOB.SelectedIndex = 0;
            FunPriLoadStatusDDL();

            ClearGrid(grvConsDocuments);
            S3GCustomerAddress1.ClearCustomerDetails();
            //ClearGrid(gvAssetDetails);
            txtAddress1.Text = txtAddress2.Text =
           txtCity.Text = txtState.Text = txtCountry.Text = txtPincode.Text = txtBusinessIRR.Text = txtCompanyIRR.Text = txtAccountingIRR.Text =
           txtFinanceAmount.Text = txtTenure.Text = TxtAutoExtnRental.Text = string.Empty;


            /* Offer Terms */
            ClearGrid(gvPaymentRuleDetails);
            ListItem lstSelect = new ListItem("--Select--", "0");
            //ddlROIRuleList.Items.Clear();
            //ddlROIRuleList.Items.Add(lstSelect);
            ddlPaymentRuleList.Items.Clear();
            ddlPaymentRuleList.Items.Add(lstSelect);


            // chk_lblResidual_Value.Checked = false;
            // chk_lblMargin.Checked = false;
            // txt_Model_Description.Text = txt_ROI_Rule_Number.Text = txt_Recovery_Pattern_Year1.Text =
            //txt_Recovery_Pattern_Year2.Text = txt_Recovery_Pattern_Year3.Text = txt_Recovery_Pattern_Rest.Text =
            //txt_Margin_Percentage.Text = txtResidualValue_Cashflow.Text = txtResidualAmt_Cashflow.Text = txtMarginMoneyPer_Cashflow.Text =
            //txtMarginMoneyAmount_Cashflow.Text = string.Empty;
            //Load_ROI_Rule();

            /* Repayment Details */
            txtAccountIRR_Repay.Text = txtCompanyIRR_Repay.Text = txtBusinessIRR_Repay.Text = string.Empty;
            ClearGrid(gvRepaymentDetails);

            /* Guarantee / Invoice Details */
            ClearGrid(gvCollateralDetails);
            ClearGrid(gvGuarantor);
            ClearGrid(gvInvoiceDetails);

            /* Alerts */
            //ClearGrid(gvAlert);

            /* Cahs Flows */
            //ClearGrid(gvFollowUp);
            //txtApplication_Followup.Text = txtOfferNo_Followup.Text = txtCustNameAdd_Followup.Text = txtEnquiryDate_Followup.Text =
            //txtEnquiry_Followup.Text = txtBranch_Followup.Text = txtLOB_Followup.Text = string.Empty;



            /*Moratorium Details*/
            //ClearGrid(gvMoratorium);
            CallStartup();
            //ddl_Rate_Type.SelectedIndex = 0;
            //ddl_Return_Pattern.SelectedIndex = 0;
            //ddl_Time_Value.SelectedIndex = 0;
            //ddl_Frequency.SelectedIndex = 0;
            //ddl_Repayment_Mode.SelectedIndex = 0;
            //ddl_Rate_Type.SelectedIndex = 0;
            //ddl_IRR_Rest.SelectedIndex = 0;
            //ddl_Interest_Levy.SelectedIndex = 0;
            //ddl_Interest_Calculation.SelectedIndex = 0;
            //ddl_Insurance.SelectedIndex = 0;
            //txtRate.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriClearLOBBranchBased()
    {
        try
        {
            txtAccountDate.Text = DateTime.Now.Date.ToString(strDateFormat);
            //ddlSalePersonCodeList.SelectedValue = "0";
            ddlAccountManager1.SelectedValue = ddlAccountManager2.SelectedValue = ddlRegionalManager.SelectedValue = "0";
            ddlStatus.SelectedIndex = 0;
            //ddlTenureType.SelectedIndex = 0;
            txtPrimeAccountNo.Text =
                //txtSubAccountNo.Text =
            txtStatus.Text = txtProductCode.Text =
                //txtApplicationDate.Text =
            txtCustomerCode.Text = txtCustomerName.Text = txtAddress1.Text = txtAddress2.Text = txtCity.Text =
            txtState.Text = txtCountry.Text = txtPincode.Text = txtBusinessIRR.Text = txtAccountingIRR.Text =
            txtCompanyIRR.Text = txtFinanceAmount.Text = TxtAutoExtnRental.Text = txtTenure.Text = txtConstitutionCode.Text = string.Empty;
            //S3GCustomerAddress1.ClearCustomerDetails();
            //TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            //txt.Text = string.Empty;
            //ClearGrid(gvAssetDetails);
            ClearGrid(grvConsDocuments);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void ClearGrid(GridView Gv)
    {
        try
        {
            Gv.Dispose();
            Gv.DataSource = null;
            Gv.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void SetDDLFirstitem(DropDownList DDl)
    {
        try
        {
            DDl.SelectedValue = "-1";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void SetDDLFirstitem(AjaxControlToolkit.ComboBox DDl)
    {
        try
        {
            DDl.SelectedValue = "-1";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private bool DisableValueField(string str)
    {
        string[] strsp = new string[2];
        strsp = str.Split('-');

        if (strsp[0].ToString() != "CID")
            return false;
        else
            return true;

    }

    private void SetWhiteSpaceDLL(DropDownList ObjDLL)
    {
        try
        {
            if (ObjDLL.Items.Count == 0)
            {
                ListItem liSelect = new ListItem("", "-1");
                ObjDLL.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    #endregion

    #region Main Tab



    #region Methods

    private void FunPriLoadLOBBranchDDL()
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            string strProcedureName = "S3g_Org_GetCustomerLookup";
            Procparam.Add("@Option", "5");
            Procparam.Add("@Param1", intCompanyId.ToString());
            Procparam.Add("@Param2", intUserId.ToString());
            Procparam.Add("@Param3", "80");
            Procparam.Add("@Param4", "1");
            //ddlLOB.BindDataTable(strProcedureName, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.BindDataTable(strProcedureName, Procparam, false, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            FunPriBindInflowDLL(_Add);
            FunPriBindOutflowDLL(_Add);
            FunPriBindRepaymentDLL(_Add);
            FunPrisetPLRCLR_Rate(ddlLOB.SelectedValue);
            FunPriLoadRepaymentMode();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Line of Business / Locations");
        }
    }

    // Added By Shibu 17-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        //Procparam.Add("@User_ID", obj_PageValue.intUserId.ToString());
        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
        Procparam.Add("@Program_Id", "80");
        //Procparam.Add("@Lob_Id", obj_PageValue.ddlLOB.SelectedValue);
        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] != null)
        {
            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
                Procparam.Add("@Lob_Id", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
        }

        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    // Added By Shibu 17-Sep-2013 Sales Personal List (Auto Suggestion)
    [System.Web.Services.WebMethod]
    public static string[] GetSalePersonList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_Get_User_List, Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetProposalNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Clear();
        ////string strProcedureName = "S3G_LOANAD_LoadAccountApplicationNo";
        //Procparam.Add("@CompanyId", obj_PageValue.intCompanyId.ToString());
        // Procparam.Add("@LocationId", obj_PageValue.ddlBranchList.SelectedValue);
        //Procparam.Add("@LobId", obj_PageValue.ddlLOB.SelectedValue.ToString());
        //Procparam.Add("@User_Level", obj_PageValue.ObjUserInfo.ProUserLevelIdRW.ToString());
        //if (obj_PageValue.ViewState["Customer_Id"] != null)
        //    Procparam.Add("@Customer_Id", obj_PageValue.ViewState["Customer_Id"].ToString());
        Procparam.Add("@CompanyId", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@LocationId", Convert.ToString(System.Web.HttpContext.Current.Session["LocationAutoSuggestValue"]));
        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] != null)
        {
            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
                Procparam.Add("@LobId", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
        }

        Procparam.Add("@User_Level", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestUserLevelID"]));
        if (System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] != null)
        {
            if (System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"].ToString() != "0")
                Procparam.Add("@Customer_Id", System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"].ToString());
        }

        //if (obj_PageValue.intAccountCreationId > 0 || obj_PageValue.strSplitNum != null || obj_PageValue.strConsNumber != null)
        if (Convert.ToInt32(System.Web.HttpContext.Current.Session["intAccountCreationId"]) > 0 || obj_PageValue.strSplitNum != null || obj_PageValue.strConsNumber != null)
        {
            Procparam.Add("@Mode", "Modify");
        }
        Procparam.Add("@Prefix", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_LOAD_PROPOSAL_NO_ACC", Procparam));

        return suggestions.ToArray();
    }

    public DataTable GetdtProposalNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        // List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Clear();
        //string strProcedureName = "S3G_LOANAD_LoadAccountApplicationNo";
        Procparam.Add("@CompanyId", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@LocationId", obj_PageValue.ddlBranchList.SelectedValue);
        Procparam.Add("@LobId", obj_PageValue.ddlLOB.SelectedValue.ToString());
        Procparam.Add("@User_Level", obj_PageValue.ObjUserInfo.ProUserLevelIdRW.ToString());
        if (obj_PageValue.ViewState["Customer_Id"] != null)
            Procparam.Add("@Customer_Id", obj_PageValue.ViewState["Customer_Id"].ToString());
        if (obj_PageValue.intAccountCreationId > 0 || obj_PageValue.strSplitNum != null || obj_PageValue.strConsNumber != null)
        {
            Procparam.Add("@Mode", "Modify");
        }
        Procparam.Add("@Prefix", prefixText);
        DataTable dt = (Utility.GetDefaultData("S3G_LAD_LOAD_PROPOSAL_NO_ACC", Procparam));

        return dt;
    }



    private void FunPriLoadRepaymentMode()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            string strProcedureName = "S3g_LoanAd_LoadRepaymentDDL";
            Procparam.Add("@MasterId", "63");
            //if (ddl_Time_Value.SelectedValue == "1" || ddl_Time_Value.SelectedValue == "2")
            //if (hdnTimeValue.Value == "1" || hdnTimeValue.Value == "2")
            //    Procparam.Add("@Is_FBD", "1");

            ddlRepaymentMode.BindDataTable(strProcedureName, Procparam, new string[] { "RepaymentModeId", "RepaymentMode" });
            if (strMode == "C")
                ddlRepaymentMode.SelectedValue = "1";

            //ddlRepaymentMode.BindDataTable(strProcedureName, Procparam, new string[] { "RepaymentModeId", "RepaymentMode" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Repayment Mode in Rental Schedule Details");
        }
    }
    private void FunPriLoadStatusDDL()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            string strProcedureName = "S3G_LOANAD_GetLookUpValues";
            Procparam.Add("@LookupType_Code", "25");
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            ddlStatus.BindDataTable(strProcedureName, Procparam, false, new string[] { "Lookup_Code", "Lookup_Code", "Lookup_Description" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Status");
        }
    }
    private void FunPriLoadApplicationNoDDL(int intBranchId, int intLobId)
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //string strProcedureName = "S3G_LOANAD_LoadAccountApplicationNo";
            string strProcedureName = "S3G_LAD_LOAD_PROPOSAL_NO_ACC";
            Procparam.Add("@CompanyId", intCompanyId.ToString());
            Procparam.Add("@LocationId", intBranchId.ToString());
            Procparam.Add("@LobId", intLobId.ToString());
            if (intAccountCreationId > 0 || strSplitNum != null || strConsNumber != null)
            {
                Procparam.Add("@Mode", "Modify");
            }
            //ddlApplicationReferenceNo.BindDataTable(strProcedureName, Procparam, new string[] { "Application_Process_Id", "Application_Number" });
            //ddlApplicationReferenceNo.BindDataTable(strProcedureName, Procparam, new string[] { "Pricing_ID", "Pricing_No" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Application Reference No");
        }
    }

    private void FillDLL_MainTab()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            if (!IsPostBack)
            {
                FunPriLoadLOBBranchDDL();

                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                // SalePerson Removed By Shibu 17-Sep-2013
                //ObjStatus.Option = 35;
                //ObjStatus.Param1 = intCompanyId.ToString();
                //Utility.FillDLL(ddlSalePersonCodeList, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

                ObjStatus.Option = 1;
                ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
                Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                ddlTenureType.SelectedValue = "134";

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }
    private void Load_CostitutionDocsList_in_Grid()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            if (intAccountCreationId == 0)
            {
                if (ddlApplicationReferenceNo.SelectedValue != "0")
                {
                    ObjStatus.Option = 37;
                    ObjStatus.Param1 = ddlApplicationReferenceNo.SelectedValue;
                }
                else
                {
                    ObjStatus.Option = 59;
                    ObjStatus.Param1 = hdnConstitutionId.Value;
                }
            }
            else if (intAccountCreationId > 0)
            {
                ObjStatus.Option = 60;
                ObjStatus.Param1 = ddlApplicationReferenceNo.SelectedValue;
            }

            grvConsDocuments.DataSource = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            grvConsDocuments.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }
    #region To be Commented

    private System.Data.DataTable GenerateRepaymentSchedule(Int32 Margine, Int32 FacilityAmount, decimal InterestPercentage, Int32 iTenure)
    {
        try
        {
            decimal FinanceAmount = 0;
            decimal InterestAmount = 0m;
            decimal TotalReceive = 0;
            decimal MonthIns = 0;
            int Tenure = iTenure;
            FinanceAmount = FacilityAmount - Margine;
            InterestAmount = (decimal)FinanceAmount * Tenure / 12 * InterestPercentage / 100;
            TotalReceive = (decimal)FinanceAmount + InterestAmount;
            MonthIns = TotalReceive / iTenure;
            string strInsDate = "";
            DateTime dateMonthlyInsDate = DateTime.Now.Date;

            DateTime locDateInsDate = dateMonthlyInsDate;
            DateTime locDateInsDate1 = dateMonthlyInsDate;
            dateMonthlyInsDate = dateMonthlyInsDate.AddMonths(1);

            System.Data.DataTable dtRepay = new System.Data.DataTable();
            DataColumn dc1 = new DataColumn("MonthlyInstalmentDate");
            DataColumn dc2 = new DataColumn("Amount");
            DataColumn dc3 = new DataColumn("SLNo");

            dtRepay.Columns.Add(dc1);
            dtRepay.Columns.Add(dc2);
            dtRepay.Columns.Add(dc3);

            DataRow dr;

            for (int i = 0; i < Tenure; i++)
            {
                if (locDateInsDate.Day == 31)
                {
                    DateTime loc = LastDate(dateMonthlyInsDate);
                    strInsDate = loc.ToString(strDateFormat);
                    dr = dtRepay.NewRow();
                    dr[0] = strInsDate;
                    dr[1] = MonthIns;
                    dtRepay.Rows.Add(dr);
                    dateMonthlyInsDate = dateMonthlyInsDate.AddMonths(1);
                }

                else
                {
                    strInsDate = dateMonthlyInsDate.ToString(strDateFormat);
                    dr = dtRepay.NewRow();
                    dr[0] = strInsDate;
                    dr[1] = MonthIns;
                    dtRepay.Rows.Add(dr);

                    if (dateMonthlyInsDate.Month != 2)
                        dateMonthlyInsDate = dateMonthlyInsDate.AddMonths(1);
                    else
                    {
                        dateMonthlyInsDate = locDateInsDate1.AddMonths(i + 2);
                    }
                }
            }
            return dtRepay;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DateTime LastDate(DateTime dt)
    {
        try
        {
            DateTime dtTo = dt;
            dtTo = dt.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));

            return dtTo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #endregion

    #region********** Events ***********

    //protected void gvAssetDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        if (gvAssetDetails.Rows.Count == 1)
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Atleast one asset required')", true);
    //            return;
    //        }
    //        LinkButton lnkSelect = (LinkButton)((GridView)sender).Rows[e.RowIndex].FindControl("lnkAssetSerialNo");
    //        System.Data.DataTable dtAssetDetails = (System.Data.DataTable)Session["ApplicationAssetDetails"];
    //        DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + lnkSelect.Text);
    //        drAsset[0].Delete();
    //        dtAssetDetails.AcceptChanges();
    //        DataRow[] drSerialAsset = dtAssetDetails.Select("SlNo > " + lnkSelect.Text);
    //        foreach (DataRow dr in drSerialAsset)
    //        {
    //            dr["SlNo"] = Convert.ToInt32(dr["SlNo"]) - 1;
    //            dr.AcceptChanges();
    //        }
    //        Session["ApplicationAssetDetails"] = dtAssetDetails;
    //        gvAssetDetails.DataSource = (System.Data.DataTable)Session["ApplicationAssetDetails"];
    //        gvAssetDetails.DataBind();
    //        FunPriIRRReset();

    //        txtFinanceAmount.Text = "";
    //        FunPriBindInflowDLL(_Add);
    //        FunPriBindOutflowDLL(_Add);
    //        lblTotalOutFlowAmount.Text = "";
    //        // FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedValue);
    //        FunPrisetPLRCLR_Rate(ddlLOB.SelectedValue);

    //        FunChangeResidualvalue();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Remove Asset";
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}

    //private void FunChangeResidualvalue()
    //{
    //    if (txtResidualValue_Cashflow.Text != string.Empty)
    //    {
    //        decimal Amount = (decimal)((System.Data.DataTable)Session["ApplicationAssetDetails"]).Compute("Sum(Finance_Amount)", "Asset_Code <>''");
    //        if (Amount > 0)
    //        {
    //            txtResidualAmt_Cashflow.Text =
    //                 Math.Round(((Convert.ToDecimal(txtResidualValue_Cashflow.Text) * Amount) / 100), 0).ToString();
    //        }
    //    }

    //}

    //protected void gvAssetDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (ViewState["IsBaseMLA"] != null)
    //        {
    //            if (ViewState["IsBaseMLA"].ToString() == "1" && (txtPrimeAccountNo.Text == "" || (txtPrimeAccountNo.Text != "" && txtStatus.Text.StartsWith("10"))))
    //            {
    //                if (gvAssetDetails.Rows.Count > 0)
    //                {
    //                    gvAssetDetails.Columns[7].Visible = false;
    //                }
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            LinkButton LnkSelect = (LinkButton)e.Row.FindControl("lnkAssetSerialNo");
    //            string strNewPurchase = "";
    //            if (ddlLOB.SelectedValue != "0" || ddlLOB.SelectedValue != "")
    //            {
    //                if (ddlLOB.SelectedItem != null)//Added by Nataraj for Bug id 3702 part 3
    //                {
    //                    if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
    //                    {
    //                        strNewPurchase = "Yes";
    //                    }
    //                    else
    //                    {
    //                        strNewPurchase = "No";
    //                    }
    //                }
    //                else
    //                {
    //                    strNewPurchase = "No";
    //                }
    //            }
    //            else
    //            {
    //                strNewPurchase = "No";
    //            }
    //            if (intAccountCreationId > 0 || strSplitNum != null)
    //            {
    //                LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=M" + NewWinAttributes);
    //            }
    //            else
    //            {
    //                if (ViewState["IsBaseMLA"] != null)
    //                {
    //                    if (ViewState["IsBaseMLA"].ToString() == "1" && (txtPrimeAccountNo.Text == "" || (txtPrimeAccountNo.Text != "" && txtStatus.Text.StartsWith("10"))))
    //                    {
    //                        if (strNewPurchase == "Yes")
    //                        {
    //                            LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsLOB=O" + NewWinAttributes);
    //                        }
    //                        else
    //                        {
    //                            LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=M" + NewWinAttributes);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
    //                    }
    //                }
    //                else
    //                {
    //                    LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
    //                }

    //            }
    //            if (!string.IsNullOrEmpty(e.Row.Cells[5].Text) && e.Row.Cells[5].Text != "&nbsp;")
    //                e.Row.Cells[5].Text = DateTime.Parse(e.Row.Cells[5].Text, CultureInfo.CurrentCulture).ToString(strDateFormat);
    //            LinkButton lnkView = (LinkButton)e.Row.FindControl("lnkView");
    //            Label lblProformaId = e.Row.FindControl("lblProformaId") as Label;
    //            if (lnkView != null && lblProformaId != null)
    //            {
    //                if (!string.IsNullOrEmpty(lblProformaId.Text))
    //                {
    //                    lnkView.Enabled = true;
    //                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblProformaId.Text, false, 0);
    //                    lnkView.Attributes.Add("onclick", "window.open('../Origination/S3GOrgProforma_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsFromAccount=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;");
    //                }
    //                else
    //                {
    //                    lnkView.Enabled = false;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Load Asset Details";
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}

    protected void gvInvoiceDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblInvoiceReferNo = e.Row.FindControl("lblInvoiceReferNo") as Label;
                LinkButton lbtnViewInvoice = e.Row.FindControl("lbtnViewInvoice") as LinkButton;
                if (lbtnViewInvoice != null && lblInvoiceReferNo != null)
                {
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblInvoiceReferNo.Text, false, 0);
                    //string myURL="window.open('../LoanAdmin/S3GLoanAdInvoiceVendor_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsFromAccount=Yes', '_self','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;";
                    string myURL = "window.showModalDialog('../LoanAdmin/S3GLoanAdInvoiceVendor_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsFromAccount=Yes','#1','dialogHeight: 600; dialogWidth: 950;dialogTop: 190px;  dialogLeft: 220px; edge: Raised; center: No;help: No; resizable: No; status: No;')";
                    lbtnViewInvoice.Attributes.Add("onclick", myURL);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Load Invoice Details";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void gvInvoiceDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //try
        //{
        //    System.Data.DataTable dtInvoiceDetails = (System.Data.DataTable)ViewState["InvoiceDetails"];
        //    if (dtInvoiceDetails.Rows.Count > 0)
        //    {
        //        dtInvoiceDetails.Rows.RemoveAt(e.RowIndex);
        //        gvInvoiceDetails.DataSource = dtInvoiceDetails;
        //        gvInvoiceDetails.DataBind();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        //    cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Remove Alert";
        //    cv_TabMainPage.IsValid = false;

        //}
    }

    protected void ddlBranchList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlBranchList.SelectedValue.ToString() != "0" && ddlLOB.SelectedIndex > 0)

            if (strMode == "M")
            {
                ddlApplicationReferenceNo.Clear();
            }
            else
            {
                if (string.IsNullOrEmpty(strSplitNum))
                {
                    if (ddlBranchList.SelectedValue.ToString() != "0")
                    {
                        FunPriLoadApplicationNoDDL(Convert.ToInt32(ddlBranchList.SelectedValue), Convert.ToInt32(ddlLOB.SelectedValue));
                    }

                    //FunPriClearLOBBranchBased();
                    //By Siva.K on 18JUN2015
                    TextBox txtRsNo = (TextBox)UcCtlRsno.FindControl("txtName");

                    if (txtRsNo.Text != string.Empty)
                    {
                        DataTable dtProposalNo = GetdtProposalNo(ddlApplicationReferenceNo.SelectedText, 0);
                        if (dtProposalNo.Rows.Count == 0)
                            FunPubClear(true);
                        else if (ddlApplicationReferenceNo.SelectedText == "")
                        {
                            btnLoadRS_OnClick(sender, e);
                        }
                    }
                    else
                        FunPriClearLOBBranchBased();
                }
                //By Siva.K on 18JUN2015 VAT and CST not Changed
                if (ViewState["Parent_Location_ID"] != null)
                {
                    FunPriLoadLocationID(Convert.ToInt32(ddlBranchList.SelectedValue));
                    ddlDeliveryState_SelectedIndexChange(null, null);
                }
            }
            ddlBillingState.SelectedValue = ddlBillingState.Items.FindByText(ddlBranchList.SelectedText).Value;
            ddlBillingState.Enabled = true;
            ddlDeliveryState.Enabled = true;
           
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Load Location Related Details";
            cv_TabMainPage.IsValid = false;
        }
    }
    //protected void ddl_Time_Value_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        if (ddl_Time_Value.SelectedIndex > 0)
    //        {
    //            txtRepaymentTime.Text = ddl_Time_Value.SelectedItem.Text;
    //            txtAdvanceInstallments.Text = "";


    //            if (ddl_Time_Value.SelectedValue == "1" || ddl_Time_Value.SelectedValue == "2")
    //            {
    //                txtFBDate.Text = "";
    //                txtFBDate.Enabled = false;
    //                rfvFBDate.Enabled = false;
    //                rvtxtFBDate.Enabled = false;
    //            }
    //            else
    //            {
    //                txtFBDate.Enabled = true;
    //                rfvFBDate.Enabled = true;
    //                rvtxtFBDate.Enabled = true;
    //            }
    //            if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
    //            {

    //                txtAdvanceInstallments.ReadOnly = true;
    //            }
    //            else
    //            {

    //                txtAdvanceInstallments.ReadOnly = false;
    //            }
    //        }
    //        FunPriLoadRepaymentMode();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Load Location Related Details";
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}



    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        ContractMgtServicesReference.ContractMgtServicesClient objContractMgtserviceClient = new ContractMgtServicesReference.ContractMgtServicesClient();

        try
        {
            if (ddlLOB.SelectedIndex > 0)
            {
                //Removed By Shibu 17-Sep-2013
                //Dictionary<string, string> Procparam = new Dictionary<string, string>();
                //string strProcedureName = "S3g_Org_GetCustomerLookup";
                //Procparam.Add("@Option", "4");
                //Procparam.Add("@Param1", intCompanyId.ToString());
                //Procparam.Add("@Param2", intUserId.ToString());
                //Procparam.Add("@Param3", "80");  // changed from 38 to 80 since it programid for account creating is 80. By Rao on 9FEB2012.
                //Procparam.Add("@Param4", ddlLOB.SelectedValue);
                //Procparam.Add("@Param5", "1");
                //ddlBranchList.BindDataTable(strProcedureName, Procparam, new string[] { "Location_Id", "Location" });
                //ddlBranchList.Clear();
                //ddlApplicationReferenceNo.Items.Clear();

                //ddlTenureType.Items.Clear();
                //S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                //ObjStatus.Option = 1;
                //ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
                //Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

                FunPriBindInflowDLL(_Add);
                FunPriBindOutflowDLL(_Add);
                FunPriBindRepaymentDLL(_Add);

                byte[] ObjPricingDataTable = objContractMgtserviceClient.FunPubGetMLASLAApplicable(Convert.ToInt32(ddlLOB.SelectedValue), intCompanyId);
                System.Data.DataSet dsMLASLAApplicable = (System.Data.DataSet)ClsPubSerialize.DeSerialize(ObjPricingDataTable, SerializationMode.Binary, typeof(System.Data.DataSet));
                if (dsMLASLAApplicable.Tables.Count > 0)
                {
                    if (dsMLASLAApplicable.Tables[0].Rows.Count > 0)
                    {
                        string strMLASLAApplicable = dsMLASLAApplicable.Tables[0].Rows[0][0].ToString();
                        ViewState["strMLASLAApplicable"] = strMLASLAApplicable;
                        //    if (strMLASLAApplicable == "False")
                        //    {
                        //        tcAccountCreation.Tabs[7].Visible = false;
                        //    }
                        //    else
                        //    {
                        //        tcAccountCreation.Tabs[7].Visible = true;
                        //    }
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Define the Rental Schedule Details in System Admin Global Parameter");
                    return;
                }
                FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedItem.Value);
                //Removed By Shibu 17-Sep-2013
                //if (ddlBranchList.SelectedIndex > 0)
                //{
                //    FunPriLoadApplicationNoDDL(Convert.ToInt32(ddlBranchList.SelectedValue), Convert.ToInt32(ddlLOB.SelectedValue));
                //}
                //if (ddlBranchList.SelectedValue != "0")
                //{
                //    FunPriLoadApplicationNoDDL(Convert.ToInt32(ddlBranchList.SelectedValue), Convert.ToInt32(ddlLOB.SelectedValue));
                //}
                //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACTORING"))
                /* if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACTORING"))
                 {
                     TabContainerMainTab.Tabs[1].Visible = false;
                 }
                 else
                 {
                     TabContainerMainTab.Tabs[1].Visible = true;
                 }
                 //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACTORING"))
                 if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACTORING"))
                 {
                     chkDORequired.Enabled = false;
                     TabContainerMainTab.Tabs[1].Visible = false;
                 }
                 else
                 {
                     chkDORequired.Enabled = true;
                     TabContainerMainTab.Tabs[1].Visible = true;
                 }*/
                FunPriLoadRepaymentMode();
                FunPriClearLOBBranchBased();

                // obj_PageValue = this;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Load Line of Business Related Details";
            cv_TabMainPage.IsValid = false;
        }
        finally
        {
            objContractMgtserviceClient.Close();
        }
    }
    private void FunPriSetDOCondition()
    {
        if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
        {
            //chkDORequired.Enabled = false;
        }
        else
        {
            //chkDORequired.Enabled = true;
        }
    }
    private void FunPriLOBBasedvalidations(string strLobName, string strLobId)
    {
        try
        {
            //if (intAccountCreationId == 0)
            //{
            //    Procparam = new Dictionary<string, string>();
            //    Procparam.Add("@Option", "7");
            //    Procparam.Add("@Is_Active", "1");
            //    Procparam.Add("@Company_ID", intCompanyId.ToString());
            //    Procparam.Add("@LOB_ID", strLobId);
            //    ddlROIRuleList.BindDataTable(SPNames.S3G_ORG_GetPricing_List, Procparam, new string[] { "ROI_Rules_ID", "ROI_Rule_Number", "Model_Description" });
            //}
            //else
            //{
            //    Procparam = new Dictionary<string, string>();
            //    Procparam.Add("@Is_Active", "1");
            //    Procparam.Add("@Company_ID", intCompanyId.ToString());
            //    Procparam.Add("@LOB_ID", strLobId);
            //}
            FunPrisetPLRCLR_Rate(strLobId);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load ROI Rule");
        }
    }


    private void FunPrisetPLRCLR_Rate(string strLobId)
    {
        try
        {
            System.Data.DataTable dtPLR = (System.Data.DataTable)ViewState["IRRDetails"];
            if (dtPLR != null)
            {
                dtPLR.DefaultView.RowFilter = "LOB_ID = " + strLobId;
                dtPLR = dtPLR.DefaultView.ToTable();
                if (dtPLR.Rows.Count > 0)
                {
                    hdnCTR.Value = dtPLR.Rows[0]["Corporate_Tax_Rate"].ToString();
                    hdnPLR.Value = dtPLR.Rows[0]["Prime_Lending_Rate"].ToString();
                    ViewState["hdnRoundOff"] = dtPLR.Rows[0]["Roundoff"].ToString();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="strType"></param>
    protected void btnCalIRR_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["ErroCode"] != null && Convert.ToString(ViewState["ErroCode"]) == "3")
            {
                Utility.FunShowAlertMsg(this, "Does not match CST leasing criteria, please check.");
                return;
            }

            System.Data.DataTable dtRepaymentStructure = new System.Data.DataTable();
            decimal decResidualAmount = 0;
            decimal decFinanceAmount = 0;
            int inttenure = 0;
            if (!string.IsNullOrEmpty(txtResidualAmt_Cashflow.Text))
                decResidualAmount = Convert.ToDecimal(txtResidualAmt_Cashflow.Text);
            if (!string.IsNullOrEmpty(txtFinanceAmount.Text))
                decFinanceAmount = Convert.ToDecimal(txtFinanceAmount.Text);
            if (!string.IsNullOrEmpty(txtTenure.Text))
                inttenure = Convert.ToInt32(txtTenure.Text);
            //FunCalculateIRROPC(out dtRepaymentStructure, decTotalRV_Amount, decTotalSchedule_Amount, inttenure, dtstartdate.ToString());
            //FunCalculateIRROPC(out dtRepaymentStructure, decResidualAmount, decFinanceAmount, inttenure, Utility.StringToDate(txtAccountDate.Text).ToString());
            FunCalculateIRROPC(out dtRepaymentStructure, decResidualAmount, decFinanceAmount, decFinanceAmount, inttenure, Utility.StringToDate(txtfirstinstdate.Text).ToString(), 0, 0, 0, 0);
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            if (strSplitNum == null && strConsNumber == null)//For Consolidation & Split
            {
                cv_TabMainPage.ErrorMessage = "Incorrect Cash flow details,Unable to Calculate the IRR";
            }
            else
            {
                cv_TabMainPage.ErrorMessage = "Cannot calculate IRR,Re-enter Cashflow details";
            }
            cv_TabMainPage.IsValid = false;
        }
    }


    #region Protected Methods



    private void FunCalculateIRROPC(out System.Data.DataTable dtRepaymentStructure, decimal decTotalRV_Amount, decimal decTotalSchedule_Amount, decimal decSchedule_Amount_SD, int inttenure, string strStartDte, decimal decTotalRVPri_Amount, decimal decTotalRVSec_Amount, int intPritenure, int intSectenure)
    {
        dtRepaymentStructure = null;
        try
        {
            string strFrequency = hdnRentfrequency.Value;
            string strTimeValue = hdnTimeValue.Value;
            System.Data.DataTable dtMoratorium = null;
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();

            ClsRepaymentStructure objRepaymentStructurePri = new ClsRepaymentStructure();

            ClsRepaymentStructure objRepaymentStructureSec = new ClsRepaymentStructure();
            DataTable dtRepaymentStructurePri;
            DataTable dtRepaymentStructureSec;
            decimal decTotalAmount = 0;
            decimal decIRRActualAmount = 0;

            if (ViewState["DtRepayGrid"] != null)
                DtRepayGrid = (System.Data.DataTable)ViewState["DtRepayGrid"];

            if (ViewState["DtRepayGridIRR"] != null)
                DtRepayGridIRR = (System.Data.DataTable)ViewState["DtRepayGridIRR"];

            //if (ViewState["DtRepayGridIRRPri"] != null)
            //    DtRepayGridIRRPri = (System.Data.DataTable)ViewState["DtRepayGridIRRPri"];
            //if (ViewState["DtRepayGridIRRSec"] != null)
            //    DtRepayGridIRRSec = (System.Data.DataTable)ViewState["DtRepayGridIRRSec"];
            if (DtRepayGrid.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one Repayment Details");
                return;
            }

            //  Code Added By Chandru K On 11 Mar 2015

            if ((ddlProposalType.SelectedValue == "5" || ddlProposalType.SelectedValue == "4" || ddlProposalType.SelectedValue == "3") && ViewState["Balance_Principal"] != null && ViewState["Balance_Principal"].ToString() != "")
            {
                if (txtAddInvAmt.Text != "" && Convert.ToDecimal(ViewState["Balance_Principal"].ToString()) > 0)
                    decTotalSchedule_Amount = Convert.ToDecimal(ViewState["Balance_Principal"].ToString()); //+ Convert.ToDecimal(txtAddInvAmt.Text);
                else
                    decTotalSchedule_Amount = Convert.ToDecimal(ViewState["Balance_Principal"].ToString());
            }

            // Code End

            //condition for checking start date and first due date gaps should be within the first frequency

            //commented by vinodha m for CR 3560

            //if (TxtFirstInstallDue.Text != txtfirstinstdate.Text)
            //{
            //    if (!string.IsNullOrEmpty(TxtFirstInstallDue.Text) && !string.IsNullOrEmpty(txtfirstinstdate.Text))
            //    {
            //        DateTime dtFirstEnddate = FunPubGetNextDate(strFrequency, Utility.StringToDate(txtfirstinstdate.Text), 1);
            //        if (Utility.StringToDate(TxtFirstInstallDue.Text) > dtFirstEnddate)
            //        {
            //            Utility.FunShowAlertMsg(this, "First Rental Due Date should not be greater than First Rental End Date");
            //            return;
            //        }
            //    }
            //}

            decimal DecRoundOff;
            if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                DecRoundOff = Convert.ToDecimal(ViewState["hdnRoundOff"]);
            else
                DecRoundOff = 2;


            System.Data.DataTable dtOutflow = ((System.Data.DataTable)ViewState["DtCashFlowOut"]);

            try
            {
                DataRow[] drOutflowOL = dtOutflow.Select("CashFlow_Flag_ID=41");
                if (drOutflowOL.Length > 0)
                {
                    foreach (DataRow dr in drOutflowOL)
                        dr.Delete();
                    dtOutflow.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
            }
            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                DataSet dsOutlfow = (DataSet)ViewState["OutflowDDL"];
                DataRow drOutflow = dtOutflow.NewRow();
                drOutflow["Date"] = Utility.StringToDate(strStartDte);

                drOutflow["CashOutFlow"] = "Ol Lease Amount";
                //drOutflow["EntityID"] = hdnCustomerId.Value;
                drOutflow["EntityID"] = ViewState["Customer_Id"].ToString();
                drOutflow["Entity"] = S3GCustomerAddress1.CustomerName;
                drOutflow["OutflowFromId"] = "144";
                drOutflow["OutflowFrom"] = "Customer";

                drOutflow["Amount"] = decTotalSchedule_Amount;

                drOutflow["CashOutFlowID"] = "-1";
                drOutflow["Accounting_IRR"] = true;
                drOutflow["Business_IRR"] = true;
                drOutflow["Company_IRR"] = true;
                drOutflow["CashFlow_Flag_ID"] = "41";
                dtOutflow.Rows.Add(drOutflow);
            }
            ViewState["DtCashFlowOut"] = dtOutflow;

            decimal decBreakPercent = 0;
            double douAccountingIRR = 0;
            double douBusinessIRR = 0;
            double douCompanyIRR = 0;

            double douDumpAccountingIRR = 0;
            double douDumpBusinessIRR = 0;
            double douDumpCompanyIRR = 0;
            //System.Data.DataTable dtRepaymentStructure = new System.Data.DataTable();

            System.Data.DataTable dtRepayDetails = new System.Data.DataTable();
            //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
            System.Data.DataTable dtRepayDetailsOthers = new System.Data.DataTable();
            //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end

            //Checking if other than normal payment , start date should be last payment date.
            //string strStartDte = txtAccountDate.Text;

            DateTime dtDocFBDate = Utility.StringToDate(strStartDte);
            int intDeffered = 0;

            //string strTimeValue = "1";//adv/Arr
            string strIRR_Rest = "1";//monthly
            //string strFrequency = "4";//monthly
            string strRepayMode = "1";//EMI

            if ((ddlProposalType.SelectedValue == "5" || ddlProposalType.SelectedValue == "4" || ddlProposalType.SelectedValue == "3") && decTotalSchedule_Amount == 0 && decTotalRV_Amount != 0)
            {
                Utility.FunShowAlertMsg(this, "Opening Principal is zero. RV should also be zero");
                return;
            }

            DataTable dtInvoicesSummary = new DataTable();

            if (ViewState["dtInvoicesACATSummary"] != null)
            {
                dtInvoicesSummary = (System.Data.DataTable)ViewState["dtInvoicesACATSummary"];
            }

            if ((decTotalRV_Amount == 0 && decTotalSchedule_Amount == 0) || (Math.Round(decTotalRV_Amount, 0) == Math.Round(decTotalSchedule_Amount, 0)))
            {
                System.Data.DataTable dtRepaymentStructure1 = new System.Data.DataTable();
                dtRepaymentStructure1.Columns.Add("NoofDays", typeof(int));
                dtRepaymentStructure1.Columns.Add("InstallmentNo", typeof(int));
                dtRepaymentStructure1.Columns.Add("FromDate", typeof(DateTime));
                dtRepaymentStructure1.Columns.Add("ToDate", typeof(DateTime));
                dtRepaymentStructure1.Columns.Add("From_Date");
                dtRepaymentStructure1.Columns.Add("To_Date");
                dtRepaymentStructure1.Columns.Add("Installment_Date");
                dtRepaymentStructure1.Columns.Add("InstallmentDate", typeof(DateTime));
                dtRepaymentStructure1.Columns.Add("Amount", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("TransactionType");
                dtRepaymentStructure1.Columns.Add("PrevBalance", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("CurrBalance", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("Slno", typeof(int));
                dtRepaymentStructure1.Columns.Add("InstallmentAmount", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("Charge", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("PrincipalAmount", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("IsMor");
                dtRepaymentStructure1.Columns.Add("Insurance", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("Others", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("Tax", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("ServiceTax", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("AMF", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("AMF_Principal", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("AMF_Charges", typeof(decimal));
                //opc058 start
                dtRepaymentStructure1.Columns.Add("Rebate_Discount", typeof(decimal));
                dtRepaymentStructure1.Columns.Add("Addi_Rebate_Discount", typeof(decimal));
                //opc058 end
                //opc060 start
                dtRepaymentStructure1.Columns.Add("Cess_Amount", typeof(decimal));
                //opc060 end
                foreach (DataRow Irow in dtInvoicesSummary.Rows)
                {
                    DataRow dtRow = dtRepaymentStructure1.NewRow();

                    dtRow["InstallmentNo"] = Irow["Install_No"].ToString();
                    dtRow["InstallmentDate"] = Irow["Install_date"].ToString();
                    dtRow["Installment_Date"] = Convert.ToDateTime(Irow["Install_date"]).ToString(strDateFormat);
                    dtRow["InstallmentAmount"] = Irow["Interest"].ToString();
                    dtRow["PrincipalAmount"] = 0.00;
                    dtRow["Charge"] = Irow["Interest"].ToString();
                    dtRow["Tax"] = Irow["Sales_Tax"].ToString();
                    dtRow["ServiceTax"] = Irow["Service_Tax"].ToString();
                    dtRow["CurrBalance"] = 0.00;
                    //opc058 start
                    dtRow["Rebate_Discount"] = Irow["Rebate_Discount"].ToString();
                    dtRow["Addi_Rebate_Discount"] = Irow["Addi_Rebate_Discount"].ToString();
                    //opc058 end
                    //opc060 start
                    dtRow["Cess_Amount"] = Irow["Cess_Amount"].ToString();
                    //opc060 end
                    //dtRow["AMF_Principal"] = 0.00;
                    //dtRow["AMF_Charges"] = Irow["AMF"].ToString();
                    dtRepaymentStructure1.Rows.Add(dtRow);
                }
                dtRepaymentStructure1.TableName = "Repayment Structure";
                dtRepaymentStructure1.AcceptChanges();
                dtRepaymentStructure = dtRepaymentStructure1.Copy();

            }
            else
            {
                try
                {
                    //strStartDte = "5/25/2017";
                    objRepaymentStructure.FunPubCalculateIRR(strStartDte, hdnPLR.Value, strFrequency, strDateFormat, decTotalRV_Amount.ToString(),
                        txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                        , out dtRepaymentStructure
                        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                        //, out dtRepayDetailsOthers
                        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                        , DtRepayGridIRR, ((System.Data.DataTable)ViewState["DtCashFlow"]).Clone(), (System.Data.DataTable)ViewState["DtCashFlowOut"]
                        , decTotalSchedule_Amount.ToString(), "", ddlReturnPattern.SelectedValue
                        , "", ddlTenureType.SelectedItem.Text, inttenure.ToString(), strIRR_Rest,
                        strTimeValue, ddlLOB.SelectedItem.Text, strRepayMode, "", dtMoratorium, chkApplicable.Checked, "NIL", 0);

                    //opc058 start
                    for (int i = 0; i < dtInvoicesSummary.Rows.Count; i++)
                    {
                        DataRow[] drRebate = dtRepaymentStructure.Select("InstallmentNo=" + Convert.ToString(dtInvoicesSummary.Rows[i]["Install_No"]) + "");
                        DataRow[] drCess = dtRepaymentStructure.Select("InstallmentNo=" + Convert.ToString(dtInvoicesSummary.Rows[i]["Install_No"]) + "");
                        if (drRebate.Length > 0)
                        {
                            if (dtInvoicesSummary.Rows[i]["Rebate_Discount"].ToString() != "")
                            {
                                drRebate[0]["Rebate_Discount"] = Convert.ToDecimal(dtInvoicesSummary.Rows[i]["Rebate_Discount"]);
                                dtRepaymentStructure.AcceptChanges();
                            }
                            if (dtInvoicesSummary.Rows[i]["Addi_Rebate_Discount"].ToString() != "")
                            {
                                drRebate[0]["Addi_Rebate_Discount"] = Convert.ToDecimal(dtInvoicesSummary.Rows[i]["Addi_Rebate_Discount"]);
                                dtRepaymentStructure.AcceptChanges();
                            }
                            if (dtInvoicesSummary.Rows[i]["Cess_Amount"].ToString() != "")
                            {
                                drCess[0]["Cess_Amount"] = Convert.ToDecimal(dtInvoicesSummary.Rows[i]["Cess_Amount"]);
                                dtRepaymentStructure.AcceptChanges();
                            }
                        }
                    }
                    //opc058 start

                    //Changes  [Request ID :##5708##] : FW: OPC_CR_56 Start
                    if (chkIsSecondary.Checked)
                    {
                        objRepaymentStructurePri.FunPubCalculateIRR(strStartDte, hdnPLR.Value, strFrequency, strDateFormat, decTotalRVPri_Amount.ToString(),
                            txtResidualValue_Cashflow.Text, out douDumpAccountingIRR, out douDumpBusinessIRR, out douDumpCompanyIRR
                            , out dtRepaymentStructurePri
                            , DtRepayGridIRR, ((System.Data.DataTable)ViewState["DtCashFlow"]).Clone(), (System.Data.DataTable)ViewState["DtCashFlowOut"]
                            , decTotalSchedule_Amount.ToString(), "", ddlReturnPattern.SelectedValue
                            , "", ddlTenureType.SelectedItem.Text, inttenure.ToString(), strIRR_Rest,
                            strTimeValue, ddlLOB.SelectedItem.Text, strRepayMode, "", dtMoratorium, chkApplicable.Checked, "PRIMARY", intPritenure);
                        // To get Start date for Last Installment Date for Primary Structure
                        string strSecStartDate = Convert.ToString(dtRepaymentStructurePri.Rows[dtRepaymentStructurePri.Rows.Count - 1]["FromDate"]);

                        objRepaymentStructureSec.FunPubCalculateIRR(strSecStartDate, hdnPLR.Value, strFrequency, strDateFormat, decTotalRVSec_Amount.ToString(),
                            txtResidualValue_Cashflow.Text, out douDumpAccountingIRR, out douDumpBusinessIRR, out douDumpCompanyIRR
                            , out dtRepaymentStructureSec
                            , DtRepayGridIRR, ((System.Data.DataTable)ViewState["DtCashFlow"]).Clone(), (System.Data.DataTable)ViewState["DtCashFlowOut"]
                            , decTotalRVPri_Amount.ToString(), "", ddlReturnPattern.SelectedValue
                            , "", ddlTenureType.SelectedItem.Text, inttenure.ToString(), strIRR_Rest,
                            strTimeValue, ddlLOB.SelectedItem.Text, strRepayMode, "", dtMoratorium, chkApplicable.Checked, "SECONDARY", intPritenure);
                        foreach (DataColumn col in dtRepaymentStructure.Columns)
                            col.ReadOnly = false;
                        foreach (DataColumn col in dtRepaymentStructurePri.Columns)
                            col.ReadOnly = false;
                        foreach (DataColumn col in dtRepaymentStructureSec.Columns)
                            col.ReadOnly = false;
                        for (int i = 0; i < dtRepaymentStructurePri.Rows.Count; i++)
                        {
                            DataRow[] drPriUpd = dtRepaymentStructure.Select("InstallmentNo=" + Convert.ToString(dtRepaymentStructurePri.Rows[i]["InstallmentNo"]) + "");
                            if (drPriUpd.Length > 0)
                            {
                                drPriUpd[0]["PrincipalAmount"] = Convert.ToDecimal(dtRepaymentStructurePri.Rows[i]["PrincipalAmount"]);
                                drPriUpd[0]["Charge"] = Convert.ToDecimal(dtRepaymentStructurePri.Rows[i]["Charge"]);
                                drPriUpd[0]["CurrBalance"] = Convert.ToDecimal(dtRepaymentStructurePri.Rows[i]["CurrBalance"]);
                                dtRepaymentStructure.AcceptChanges();
                            }
                        }
                        for (int i = 0; i < dtRepaymentStructureSec.Rows.Count; i++)
                        {
                            DataRow[] drSecUpd = dtRepaymentStructure.Select("InstallmentNo=" + Convert.ToString(dtRepaymentStructureSec.Rows[i]["InstallmentNo"]) + "");
                            if (drSecUpd.Length > 0)
                            {
                                drSecUpd[0]["PrincipalAmount"] = Convert.ToDecimal(dtRepaymentStructureSec.Rows[i]["PrincipalAmount"]);
                                drSecUpd[0]["Charge"] = Convert.ToDecimal(dtRepaymentStructureSec.Rows[i]["Charge"]);
                                drSecUpd[0]["CurrBalance"] = Convert.ToDecimal(dtRepaymentStructureSec.Rows[i]["CurrBalance"]);
                                dtRepaymentStructure.AcceptChanges();
                            }
                        }
                        
                    }
                    //Changes  [Request ID :##5708##] : FW: OPC_CR_56 End

                }
                catch (Exception ex)
                {
                    FunLoadDummyRepayment(strFrequency);
                    ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                    throw ex;
                }

            }
            dtRepaymentStructure.Columns["Charge"].ColumnName = "FinanceCharges";

            ViewState["RepaymentStructure"] = dtRepaymentStructure;

            //code added by saran on 30-Dec-2014 start

            if (dtRepaymentStructure.Rows.Count > 0)
            {

                //if (dtRepaymentStructure.Rows[0]["ToDate"].ToString() != dtRepaymentStructure.Rows[0]["InstallmentDate"].ToString())//to date is not eqal to installment date
                //if (TxtFirstInstallDue.Text != txtfirstinstdate.Text)
                //{
                //DateTime dtstartTodate = Utility.StringToDate(dtRepaymentStructure.Rows[0]["FromDate"].ToString());
                DateTime dtstartTodate = Utility.StringToDate(txtfirstinstdate.Text);
                DateTime dtstartStartTodate = dtstartTodate;
                for (int i = 0; i < dtRepaymentStructure.Rows.Count; i++)
                {
                    DateTime dtstartEndTodate = dtstartTodate;

                    if (chkApplicable.Checked || chkApplicableSec.Checked)
                    {
                        dtRepaymentStructure.Rows[i]["FromDate"] = Convert.ToDateTime(dtInvoicesSummary.Rows[i]["Strat_Date"]);
                        dtRepaymentStructure.Rows[i]["From_Date"] = Convert.ToDateTime(dtInvoicesSummary.Rows[i]["Strat_Date"]).ToString(strDateFormat);
                        dtRepaymentStructure.Rows[i]["ToDate"] = Convert.ToDateTime(dtInvoicesSummary.Rows[i]["End_Date"]);
                        dtRepaymentStructure.Rows[i]["To_Date"] = Convert.ToDateTime(dtInvoicesSummary.Rows[i]["End_Date"]).ToString(strDateFormat);
                        dtRepaymentStructure.Rows[i]["InstallmentDate"] = Convert.ToDateTime(dtInvoicesSummary.Rows[i]["Install_date"]);
                        dtRepaymentStructure.Rows[i]["Installment_Date"] = Convert.ToDateTime(dtInvoicesSummary.Rows[i]["Install_date"]).ToString(strDateFormat);
                    }
                    else
                    {
                        dtstartEndTodate = FunPubGetNextDate(strFrequency, dtstartTodate, i + 1);
                        dtRepaymentStructure.Rows[i]["FromDate"] = dtstartStartTodate;
                        dtRepaymentStructure.Rows[i]["From_Date"] = dtstartStartTodate.ToString(strDateFormat);
                        dtRepaymentStructure.Rows[i]["ToDate"] = dtstartEndTodate.AddDays(-1);
                        dtRepaymentStructure.Rows[i]["To_Date"] = dtstartEndTodate.AddDays(-1).ToString(strDateFormat);
                        dtstartStartTodate = FunPubGetNextDate(strFrequency, dtstartTodate, i + 1);
                    }
                    dtRepaymentStructure.Rows[i]["NoofDays"] = (Convert.ToDateTime(dtRepaymentStructure.Rows[i]["ToDate"].ToString()) - Convert.ToDateTime(dtRepaymentStructure.Rows[i]["FromDate"].ToString())).TotalDays + 1;

                }
                //}
                //FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1);
            }
            //code added by saran on 30-Dec-2014 end

            //code added by saran on 3-Jul-2014 Sales Tax start

            if (dtRepaymentStructure.Rows.Count > 0)
            {
                System.Data.DataTable dtRepaymentStructureST = null;
                FunCalculateSalesTax(dtRepaymentStructure, out dtRepaymentStructureST);
                ViewState["RepaymentStructure"] = dtRepaymentStructureST;
                //foreach (DataRow dr in dtRepaymentStructureST.Rows)
                //{

                //    dr["InstallmentAmount"] = Convert.ToDecimal(Convert.ToDecimal(dr["InstallmentAmount"]).ToString(Funsetsuffix()));
                //    dr["FinanceCharges"] = Convert.ToDecimal(Convert.ToDecimal(dr["FinanceCharges"]).ToString(Funsetsuffix()));
                //    //dr["PrincipalAmount"] = Convert.ToDecimal(Convert.ToDecimal(dr["PrincipalAmount"]).ToString(Funsetsuffix()));
                //    if (dr["Tax"].ToString() != "")
                //        dr["Tax"] = Convert.ToDecimal(Convert.ToDecimal(dr["Tax"]).ToString(Funsetsuffix()));
                //    if (dr["AMF"].ToString() != "")
                //        dr["AMF"] = Convert.ToDecimal(Convert.ToDecimal(dr["AMF"]).ToString(Funsetsuffix()));
                //    if (dr["ServiceTax"].ToString() != "")
                //        dr["ServiceTax"] = Convert.ToDecimal(Convert.ToDecimal(dr["ServiceTax"]).ToString(Funsetsuffix()));
                //    if (dr["Insurance"].ToString() != "")
                //        dr["Insurance"] = Convert.ToDecimal(Convert.ToDecimal(dr["Insurance"]).ToString(Funsetsuffix()));
                //    if (dr["Others"].ToString() != "")
                //        dr["Others"] = Convert.ToDecimal(Convert.ToDecimal(dr["Others"]).ToString(Funsetsuffix()));
                //    dtRepaymentStructureST.AcceptChanges();
                //}

                DataTable dtRepaymentStructureSTCopy = dtRepaymentStructureST.Copy();
                if (dtRepaymentStructureSTCopy.Rows.Count > 0)
                {

                    foreach (DataRow dr in dtRepaymentStructureSTCopy.Rows)
                    {
                        if (dr["InstallmentAmount"].ToString() != "")
                            dr["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dr["InstallmentAmount"].ToString()));
                        if (dr["Tax"].ToString() != "")
                            dr["Tax"] = Math.Round(Convert.ToDecimal(dr["Tax"].ToString()));
                        if (dr["ServiceTax"].ToString() != "")
                            dr["ServiceTax"] = Math.Round(Convert.ToDecimal(dr["ServiceTax"].ToString()));
                        if (dr["Insurance"].ToString() != "")
                            dr["Insurance"] = Math.Round(Convert.ToDecimal(dr["Insurance"].ToString()));
                        if (dr["Others"].ToString() != "")
                            dr["Others"] = Math.Round(Convert.ToDecimal(dr["Others"].ToString()));
                    }

                    ViewState["InstallmentAmount_Total"] = dtRepaymentStructureSTCopy.Compute("sum(InstallmentAmount)", "");
                    ViewState["AMF_Total"] = dtRepaymentStructureSTCopy.Compute("sum(AMF)", "");
                    ViewState["TAX_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Tax)", "");
                    ViewState["ServiceTax_Total"] = dtRepaymentStructureSTCopy.Compute("sum(ServiceTax)", "");
                    if (dtRepaymentStructureSTCopy.Rows[0]["Insurance"].ToString() != "")
                        ViewState["Insurance_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Insurance)", "");
                    if (dtRepaymentStructureSTCopy.Rows[0]["Others"].ToString() != "")
                        ViewState["Others_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Others)", "");
                }
                //grvRepayStructure.DataSource = dtRepaymentStructureST;
                //grvRepayStructure.DataBind();
            }

            DataTable dtRepaymentStructureNew = dtRepaymentStructure.Copy();
            //code added by saran in 3-Jul-2014 Sales Tax end
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];

            //foreach (DataRow dr in dtRepaymentStructure.Rows)
            //{
            //     DataRow[] dr1 = DtRepayGrid.Select(dr["InstallmentNo"].ToString() + ">=FROMINSTALL and "+dr["InstallmentNo"].ToString()+"<=TOINSTALL and CashFlow_Flag_ID=23");

            //     foreach (DataRow drinst in dr1)
            //     {
            //         dr["installmentamount"] = drinst["PerInstall"];

            //         dtRepaymentStructure.AcceptChanges();
            //     }
            //     DataRow[] dr2 = DtRepayGrid.Select(dr["InstallmentNo"].ToString() + ">=FROMINSTALL and " + dr["InstallmentNo"].ToString() + "<=TOINSTALL and CashFlow_Flag_ID=105");

            //     foreach (DataRow drinst in dr2)
            //     {
            //         dr["AMF"] = drinst["PerInstall"];

            //         dtRepaymentStructure.AcceptChanges();
            //     }

            //}
            //code added To Amort AMF principal and AMF Interest start

            if (strSplitDate == null && ViewState["strSplitDate"] != null)
                strSplitDate = ViewState["strSplitDate"].ToString();

            if (!String.IsNullOrEmpty(strSplitDate))
            {
                if (dtRepaymentStructure.Select("InstallmentDate > '" + Utility.StringToDate(strSplitDate) + "'").Count() > 0)
                    dtRepaymentStructure = dtRepaymentStructure.Select("InstallmentDate > '" + Utility.StringToDate(strSplitDate) + "'").CopyToDataTable();
            }

            foreach (DataRow dr in dtRepaymentStructure.Rows)
            {
                decimal decInstallAmount = 0, decPrincipalAmount = 0, decAMFInstallAmount = 0, decAMFPrincipalAmount = 0, decACtInstallAmount = 0, decActAmfAmount = 0;
                decimal decInstPrincipalAmount = 0;
                DataRow[] dr1 = DtRepayGrid.Select(dr["InstallmentNo"].ToString() + ">=FROMINSTALL and " + dr["InstallmentNo"].ToString() + "<=TOINSTALL and CashFlow_Flag_ID=23");

                foreach (DataRow drinst in dr1)
                {
                    decACtInstallAmount = Convert.ToDecimal(drinst["PerInstall"].ToString());
                }

                DataRow[] dr2 = DtRepayGrid.Select(dr["InstallmentNo"].ToString() + ">=FROMINSTALL and " + dr["InstallmentNo"].ToString() + "<=TOINSTALL and CashFlow_Flag_ID=105");

                foreach (DataRow drinst in dr2)
                {
                    decActAmfAmount = Convert.ToDecimal(drinst["PerInstall"].ToString());
                }

                if (!string.IsNullOrEmpty(dr["InstallmentAmount"].ToString()))
                    decInstallAmount = Convert.ToDecimal(dr["InstallmentAmount"].ToString());
                if (!string.IsNullOrEmpty(dr["PrincipalAmount"].ToString()))
                    decPrincipalAmount = Convert.ToDecimal(dr["PrincipalAmount"].ToString());
                //if (!string.IsNullOrEmpty(dr["AMF"].ToString()))
                //    decAMFInstallAmount = Convert.ToDecimal(dr["AMF"].ToString());

                if (decInstallAmount != 0)
                {
                    decInstPrincipalAmount = decACtInstallAmount * (decPrincipalAmount / decInstallAmount);
                    decAMFPrincipalAmount = decActAmfAmount * (decPrincipalAmount / decInstallAmount);
                }
                else
                {
                    decInstPrincipalAmount = decAMFPrincipalAmount = 0;
                }

                foreach (DataColumn col in dtRepaymentStructure.Columns)
                    col.ReadOnly = false;


                dr["AMF_Principal"] = decAMFPrincipalAmount.ToString();
                dr["AMF_Charges"] = (decActAmfAmount - decAMFPrincipalAmount).ToString();
                dr["PrincipalAmount"] = decInstPrincipalAmount.ToString();
                dr["FinanceCharges"] = (decACtInstallAmount - decInstPrincipalAmount).ToString();
                dr["installmentamount"] = decACtInstallAmount.ToString();
                dr["AMF"] = decActAmfAmount.ToString();
                dr["InstallmentAmount"] = Convert.ToDecimal(Convert.ToDecimal(dr["InstallmentAmount"]).ToString());
                dr["FinanceCharges"] = Convert.ToDecimal(Convert.ToDecimal(dr["FinanceCharges"]).ToString());
                // dr["PrincipalAmount"] = Convert.ToDecimal(Convert.ToDecimal(dr["PrincipalAmount"]).ToString(Funsetsuffix()));
                if (!string.IsNullOrEmpty(dr["Tax"].ToString()))
                    dr["Tax"] = Convert.ToDecimal(Convert.ToDecimal(dr["Tax"]).ToString());
                if (!string.IsNullOrEmpty(dr["AMF"].ToString()))
                    dr["AMF"] = Convert.ToDecimal(Convert.ToDecimal(dr["AMF"]).ToString());
                if (!string.IsNullOrEmpty(dr["ServiceTax"].ToString()))
                    dr["ServiceTax"] = Convert.ToDecimal(Convert.ToDecimal(dr["ServiceTax"]).ToString());
                if (!string.IsNullOrEmpty(dr["Insurance"].ToString()))
                    dr["Insurance"] = Convert.ToDecimal(Convert.ToDecimal(dr["Insurance"]).ToString());
                if (!string.IsNullOrEmpty(dr["Others"].ToString()))
                    dr["Others"] = Convert.ToDecimal(Convert.ToDecimal(dr["Others"]).ToString());

                if (!string.IsNullOrEmpty(dr["AMF_Principal"].ToString()))
                    dr["AMF_Principal"] = Convert.ToDecimal(Convert.ToDecimal(dr["AMF_Principal"]).ToString());
                if (!string.IsNullOrEmpty(dr["AMF_Charges"].ToString()))
                    dr["AMF_Charges"] = Convert.ToDecimal(Convert.ToDecimal(dr["AMF_Charges"]).ToString());
                dtRepaymentStructure.AcceptChanges();


            }
            //code added To Amort AMF principal and AMF Interest end
            ViewState["RepaymentStructure"] = dtRepaymentStructure;
            grvRepayStructure.DataSource = dtRepaymentStructure;
            grvRepayStructure.DataBind();

            txtAccountIRR_Repay.Text = douAccountingIRR.ToString("0.0000");
            txtAccountingIRR.Text = douAccountingIRR.ToString("0.0000");

            txtBusinessIRR_Repay.Text = douBusinessIRR.ToString("0.0000");
            txtBusinessIRR.Text = douBusinessIRR.ToString("0.0000");

            txtCompanyIRR_Repay.Text = douCompanyIRR.ToString("0.0000");
            txtCompanyIRR.Text = douCompanyIRR.ToString("0.0000");

            if (DtRepayGrid.Rows.Count > 0)
            {
                //FunPriShowRepaymetDetails((decimal)DtRepayGrid.Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID = 23"));
                if (ViewState["InstallmentAmount_Total"] != null)
                    FunPriShowRepaymetDetails(Convert.ToDecimal(ViewState["InstallmentAmount_Total"]));
                else
                    FunPriShowRepaymetDetails((decimal)DtRepayGrid.Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID = 23"));
            }
            else
            {
                FunPriShowRepaymetDetails(decTotalSchedule_Amount + FunPriGetInterestAmount());
            }
            //DataTable DtRepayGridp = ((DataTable)ViewState["DtRepayGrid"]).Copy();
            //DataRow dr1;
            //dr1 = DtRepayGridp.NewRow();
            //dr1["slno"] = "1";
            //dr1["CashFlow"] = "Tax";
            //dr1["TotalPeriodInstall"] = ViewState["TAX_Total"];
            //DtRepayGridp.Rows.Add(dr1);

            //dr1 = DtRepayGridp.NewRow();
            //dr1["slno"] = "4";
            //dr1["CashFlow"] = "ServiceTax";
            //dr1["TotalPeriodInstall"] = ViewState["ServiceTax_Total"];
            //DtRepayGridp.Rows.Add(dr1);


            ////if (ViewState["Insurance_Total"].ToString() != "")
            ////{
            ////    dr1 = DtRepayGridp.NewRow();
            ////    dr1["slno"] = "5";
            ////    dr1["CashFlow"] = "Insurance";
            ////    dr1["TotalPeriodInstall"] = ViewState["Insurance_Total"];
            ////    DtRepayGridp.Rows.Add(dr1);
            ////}

            ////if (ViewState["Others_Total"].ToString() != "")
            ////{
            ////    dr1 = DtRepayGridp.NewRow();
            ////    dr1["slno"] = "6";
            ////    dr1["CashFlow"] = "Others";
            ////    dr1["TotalPeriodInstall"] = ViewState["Others_Total"];
            ////    DtRepayGridp.Rows.Add(dr1);
            ////}
            ////DataRow[] datainst = DtRepayGridp.Select("CashFlow_Flag_ID = 23", "");
            ////if (datainst.Length > 0)
            ////{
            ////    foreach (DataRow drrow in datainst)
            ////    {
            ////        drrow["slno"] = "1";
            ////        drrow["TotalPeriodInstall"] = ViewState["InstallmentAmount_Total"];
            ////        drrow.AcceptChanges();
            ////    }
            ////}
            ////DataRow[] dataAMF = DtRepayGridp.Select("CashFlow_Flag_ID = 105", "");
            ////if (dataAMF.Length > 0)
            ////{
            ////    foreach (DataRow drrow in dataAMF)
            ////    {
            ////        drrow["slno"] = "2";
            ////        drrow["TotalPeriodInstall"] = ViewState["AMF_Total"];
            ////        drrow.AcceptChanges();
            ////    }
            ////}
            //foreach (DataRow dr in DtRepayGridp.Rows)
            //{
            //    dr["TotalPeriodInstall"] = Convert.ToDecimal(Convert.ToDecimal(dr["TotalPeriodInstall"]).ToString(Funsetsuffix()));
            //}
            //DataView dv = DtRepayGridp.DefaultView;
            //dv.Sort = "slno asc";
            //DtRepayGridp = dv.ToTable();
            //ViewState["DtRepayGrid"] = DtRepayGrid;
            FunPriCalculateSummary(((DataTable)ViewState["DtRepayGrid"]), "cashflow", "TotalPeriodInstall");
            FunPriGenerateNewRepaymentRow();
            //if (ddl_Repayment_Mode.SelectedValue != "2")
            //{
            Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
            if (lblCashFlowId.Text != "23" && lblCashFlowId.Text != "105")
            {
                ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
            }
            //}
            //else
            //{
            //    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
            //}


            /*Security Deposit has been calculated automatically for OPC */
            FunPriLoadSecurityCashflows(decTotalSchedule_Amount, decSchedule_Amount_SD);
            FunPriLoadOneTimeProcessingFeeCashflows();
            FunPriLoadProcessingFeeCashflows();
            FunPriCalculateServiceTax();            //Added on 31Mar2015

            /*UMFC has been calculated automatically for other than Product & TermLoan Return Pattern 
            (Also applicable to HP,FL,LN,TE,TL) Updated on 28th Oct 2010*/
            //if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL") && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            //{
            //    FunPriInsertUMFC();
            //}
            //if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper() != "PRODUCT"))
            //{
            //    grvRepayStructure.Columns[5].Visible = false;
            //    //grvRepayStructure.Columns[6].Visible = true;
            //    //grvRepayStructure.Columns[7].Visible = true;
            //}
            //else
            //{
            //    grvRepayStructure.Columns[5].Visible = true;
            //    grvRepayStructure.Columns[6].Visible = false;
            //    grvRepayStructure.Columns[7].Visible = false;
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected void FunLoadDummyRepayment(string strFrequency)
    {
        DataTable dtInvoicesSummary = new DataTable();
        System.Data.DataTable dtRepaymentStructure1 = new System.Data.DataTable();
        dtRepaymentStructure1.Columns.Add("NoofDays", typeof(int));
        dtRepaymentStructure1.Columns.Add("InstallmentNo", typeof(int));
        dtRepaymentStructure1.Columns.Add("FromDate", typeof(DateTime));
        dtRepaymentStructure1.Columns.Add("ToDate", typeof(DateTime));
        dtRepaymentStructure1.Columns.Add("From_Date");
        dtRepaymentStructure1.Columns.Add("To_Date");
        dtRepaymentStructure1.Columns.Add("Installment_Date");
        dtRepaymentStructure1.Columns.Add("InstallmentDate", typeof(DateTime));
        dtRepaymentStructure1.Columns.Add("Amount", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("TransactionType");
        dtRepaymentStructure1.Columns.Add("PrevBalance", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("CurrBalance", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("Slno", typeof(int));
        dtRepaymentStructure1.Columns.Add("InstallmentAmount", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("Charge", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("PrincipalAmount", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("IsMor");
        dtRepaymentStructure1.Columns.Add("Insurance", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("Others", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("Rebate_Discount", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("Addi_Rebate_Discount", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("Cess_Amount", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("Tax", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("ServiceTax", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("AMF", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("AMF_Principal", typeof(decimal));
        dtRepaymentStructure1.Columns.Add("AMF_Charges", typeof(decimal));

        if (ViewState["dtInvoicesACATSummary"] != null)
        {
            dtInvoicesSummary = (System.Data.DataTable)ViewState["dtInvoicesACATSummary"];

            DateTime dtstartTodate = Utility.StringToDate(txtfirstinstdate.Text);
            DateTime dtstartStartTodate = Utility.StringToDate(txtfirstinstdate.Text);

            int i = 0;
            foreach (DataRow Irow in dtInvoicesSummary.Rows)
            {
                DataRow dtRow = dtRepaymentStructure1.NewRow();

                dtRow["InstallmentNo"] = Irow["Install_No"].ToString();
                dtRow["InstallmentDate"] = Irow["Install_date"].ToString();
                dtRow["Installment_Date"] = Convert.ToDateTime(Irow["Install_date"]).ToString(strDateFormat);
                dtRow["InstallmentAmount"] = Irow["Interest"].ToString();
                dtRow["PrincipalAmount"] = 0.00;
                dtRow["Charge"] = Irow["Interest"].ToString();
                dtRow["Tax"] = Irow["Sales_Tax"].ToString();
                dtRow["AMF"] = Irow["AMF"].ToString();
                dtRow["ServiceTax"] = Irow["Service_Tax"].ToString();
                dtRow["CurrBalance"] = 0.00;

                DateTime dtstartEndTodate = dtstartTodate;
                dtstartEndTodate = FunPubGetNextDate(strFrequency, dtstartTodate, i + 1);

                dtRow["FromDate"] = dtstartStartTodate;
                dtRow["From_Date"] = dtstartStartTodate.ToString(strDateFormat);
                dtRow["ToDate"] = dtstartEndTodate.AddDays(-1);
                dtRow["To_Date"] = dtstartEndTodate.AddDays(-1).ToString(strDateFormat);
                dtstartStartTodate = FunPubGetNextDate(strFrequency, dtstartTodate, i + 1);
                dtRow["NoofDays"] = (Convert.ToDateTime(dtRow["ToDate"].ToString()) - Convert.ToDateTime(dtRow["FromDate"].ToString())).TotalDays + 1;
                dtRepaymentStructure1.Rows.Add(dtRow);
                i++;
            }

            grvRepayStructure.DataSource = dtRepaymentStructure1;
            grvRepayStructure.DataBind();
        }
    }
    private DateTime FunPubGetNextDate(string strFrequency, DateTime dtFromDate, int intNoInstalment)
    {
        DateTime dtToDate;
        switch (strFrequency.ToLower())
        {
            //Weekly
            case "2":
                intNoInstalment = intNoInstalment * 7;
                dtToDate = dtFromDate.AddDays(intNoInstalment);
                break;
            //Fortnightly
            case "3":
                intNoInstalment = intNoInstalment * 15;
                dtToDate = dtFromDate.AddDays(intNoInstalment);
                break;
            //Monthly
            case "4":
                dtToDate = dtFromDate.AddMonths(intNoInstalment);
                break;
            //bi monthly
            case "5":
                intNoInstalment = intNoInstalment * 2;
                dtToDate = dtFromDate.AddMonths(intNoInstalment);
                break;
            //quarterly
            case "6":
                intNoInstalment = intNoInstalment * 3;
                dtToDate = dtFromDate.AddMonths(intNoInstalment);
                break;
            // half yearly
            case "7":
                intNoInstalment = intNoInstalment * 6;
                dtToDate = dtFromDate.AddMonths(intNoInstalment);
                break;
            //annually
            case "8":
                intNoInstalment = intNoInstalment * 12;
                dtToDate = dtFromDate.AddMonths(intNoInstalment);
                break;
            //daily
            case "0":
                dtToDate = dtFromDate.AddDays(intNoInstalment);
                break;
            //daily
            case "1":
                dtToDate = dtFromDate.AddDays(intNoInstalment);
                break;
            default:
                dtToDate = dtFromDate.AddMonths(intNoInstalment);
                break;
        }


        /*   DateTime lastDayOfCurrentMonth = new DateTime(dtToDate.Year, dtToDate.Month,
                     DateTime.DaysInMonth(dtToDate.Year, dtToDate.Month));

           if (lastDayOfCurrentMonth != dtToDate)
           {
               if (dtFromDate.Day >= dtToDate.Day)
               {
                   if (dtFromDate.Day > lastDayOfCurrentMonth.Day)
                   dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + lastDayOfCurrentMonth.Day.ToString() + "/" + dtToDate.Year.ToString());
                   else
                       dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + dtFromDate.Day.ToString() + "/" + dtToDate.Year.ToString());
               }
           }*/
        dtToDate = FunPubMonthEndDate(dtFromDate, dtToDate);

        return dtToDate;
    }

    private DateTime FunPubMonthEndDate(DateTime dtFromdate, DateTime dtTodate)
    {
        DateTime dtOutDate = dtTodate;
        DateTime lastDayOfFromMonth = new DateTime(dtFromdate.Year, dtFromdate.Month,
                  DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month));
        if (dtFromdate == lastDayOfFromMonth)//if start date is month end
        {
            DateTime lastDayOfToMonth = new DateTime(dtTodate.Year, dtTodate.Month,
                  DateTime.DaysInMonth(dtTodate.Year, dtTodate.Month));
            if (dtTodate != lastDayOfToMonth)//if to date is different then arrive month end
            {
                dtOutDate = DateTime.Parse(dtTodate.Month.ToString() + "/" + lastDayOfToMonth.Day.ToString() + "/" + dtTodate.Year.ToString());
            }

        }

        return dtOutDate;
    }
    /// <summary>
    /// Get IRR Details From Global Paramater Setup
    /// </summary>
    protected void FunProGetIRRDetails()
    {
        try
        {
            System.Data.DataTable dtIRRDetails = Utility.FunPubGetGlobalIRRDetails(intCompanyId, null);
            ViewState["IRRDetails"] = dtIRRDetails;
            if (dtIRRDetails.Rows.Count > 0)
            {
                if (dtIRRDetails.Rows[0]["IsIRRApplicable"].ToString() == "True")
                {
                    txtAccountingIRR.Visible =
                    lblAccountingIRR.Visible =
                    txtCompanyIRR.Visible =
                    lblCompanyIRR.Visible =
                    txtCompanyIRR_Repay.Visible =
                    lblCompanyIRR_Repay.Visible =
                    rfvCompanyIRR.Enabled =
                    txtAccountIRR_Repay.Visible =
                    lblAccountIRR_Repay.Visible =
                    rfvAccountingIRR.Enabled = true;
                }
                else
                {
                    //txtAccountingIRR.Visible = false;
                    //lblAccountingIRR.Visible = false;
                    txtCompanyIRR.Visible =
                    lblCompanyIRR.Visible =
                    txtCompanyIRR_Repay.Visible =
                    lblCompanyIRR_Repay.Visible =
                    rfvCompanyIRR.Enabled =
                    txtAccountIRR_Repay.Visible =
                    lblAccountIRR_Repay.Visible =
                    rfvAccountingIRR.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load IRR Related Details");
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearTempInvoiceDtl();        //Added on 06Jul2015
            // wf cancel
            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = null;
            System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = null;
            System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = null;
            System.Web.HttpContext.Current.Session["LocationAutoSuggestValue"] = null;
            System.Web.HttpContext.Current.Session["AutoSuggestUserLevelID"] = null;
            System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = null;

            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else
                FunPriClosePage();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to Cancel this Process";
            cv_TabMainPage.IsValid = false;
        }
    }

    private void FunPriClosePage()
    {
        if (Request.QueryString["IsFromAccount"] != null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
        }
        else
        {
            Response.Redirect(strRedirectPage, false);
        }
    }

    protected void btnAcccountCancel_Click(object sender, EventArgs e)
    {

        ContractMgtServicesReference.ContractMgtServicesClient objContractMgtServicesClient = new ContractMgtServicesReference.ContractMgtServicesClient();
        try
        {
            AccountCreationEntity objAccountCreationEntity = new AccountCreationEntity();
            objAccountCreationEntity.intCompanyId = intCompanyId;
            objAccountCreationEntity.strPANumber = txtPrimeAccountNo.Text;
            //objAccountCreationEntity.strSANum = txtSubAccountNo.Text;
            objAccountCreationEntity.intUserId = intUserId;
            int intResult = objContractMgtServicesClient.FunPubUpdateAccountStatus(objAccountCreationEntity);
            if (intResult == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "Rental Schedule cancelled successfully");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Due to Data Problem,Unable to Cancel the Rental Schedule");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Cancel the Rental Schedule";
            cv_TabMainPage.IsValid = false;
        }
        finally
        {
            objContractMgtServicesClient.Close();
        }
    }
    #endregion

    #endregion

    #region Offer Tab

    //private void FunPriBindROIDLL(string Mode)
    //{
    //    try
    //    {
    //        if (Mode == _Add)
    //        {
    //            Dictionary<string, string> objParameters = new Dictionary<string, string>();
    //            System.Data.DataSet dsROILov = Utility.GetDataset("s3g_org_loadROILov", objParameters);
    //            ddl_Rate_Type.BindDataTable(dsROILov.Tables[0]);
    //            ddl_Return_Pattern.BindDataTable(dsROILov.Tables[1]);
    //            ddl_Time_Value.BindDataTable(dsROILov.Tables[2]);
    //            ddl_Frequency.BindDataTable(dsROILov.Tables[3]);
    //            ddl_Repayment_Mode.BindDataTable(dsROILov.Tables[4]);
    //            ddl_IRR_Rest.BindDataTable(dsROILov.Tables[5]);
    //            ddl_Interest_Calculation.BindDataTable(dsROILov.Tables[3]);
    //            ddl_Interest_Levy.BindDataTable(dsROILov.Tables[3]);
    //            ddl_Insurance.BindDataTable(dsROILov.Tables[6]);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw new ApplicationException("Due to Data Problem, Unable to Load ROI Rule Details");
    //    }


    //}

    //private void Load_ROI_Rule()
    //{
    //    ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
    //    try
    //    {
    //        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
    //        System.Data.DataTable ObjDTROI = new System.Data.DataTable();
    //        if (!string.IsNullOrEmpty(strConsNumber)) // while consolidating, ROI Rule Details from ROI Rule Master Data
    //        {
    //            ObjStatus.Option = 40;
    //            ObjStatus.Param1 = ddlROIRuleList.SelectedValue;
    //            ObjDTROI = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
    //            ViewState["ROIRules"] = ObjDTROI;
    //        }
    //        else // ROI Rule Details from Application/Account
    //        {
    //            ObjDTROI = (System.Data.DataTable)ViewState["ROIRules"];
    //        }
    //        if (ObjDTROI.Rows.Count > 0)
    //        {
    //            txt_Model_Description.Text = ObjDTROI.Rows[0]["Model_Description"].ToString();
    //            txt_ROI_Rule_Number.Text = ObjDTROI.Rows[0]["ROI_Rule_Number"].ToString();
    //            txt_Recovery_Pattern_Year1.Text = ObjDTROI.Rows[0]["Recovery_Pattern_Year1"].ToString();
    //            txt_Recovery_Pattern_Year2.Text = ObjDTROI.Rows[0]["Recovery_Pattern_Year2"].ToString();
    //            txt_Recovery_Pattern_Year3.Text = ObjDTROI.Rows[0]["Recovery_Pattern_Year3"].ToString();
    //            txt_Recovery_Pattern_Rest.Text = ObjDTROI.Rows[0]["Recovery_Pattern_Rest"].ToString();
    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Rate"], tr_lblRate, txtRate);
    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Rate_Type"], tr_lblRate_Type, ddl_Rate_Type);
    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Return_Pattern"], tr_lblReturn_Pattern, ddl_Return_Pattern);
    //            if (ObjDTROI.Rows[0]["Time_Value"].ToString() == "-1")
    //            {
    //                rfvTimeValue.Enabled = false;
    //            }
    //            else
    //            {
    //                rfvTimeValue.Enabled = true;
    //            }
    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Time_Value"], tr_lblTime_Value, ddl_Time_Value);

    //            if (ObjDTROI.Rows[0]["Time_Value"].ToString() == "3" || ObjDTROI.Rows[0]["Time_Value"].ToString() == "4")
    //            {
    //                txtFBDate.Enabled = true;
    //                rfvFBDate.Enabled = true;
    //                rvtxtFBDate.Enabled = true;
    //            }
    //            else
    //            {
    //                txtFBDate.Text = "";
    //                txtFBDate.Enabled = false;
    //                rfvFBDate.Enabled = false;
    //                rvtxtFBDate.Enabled = false;
    //            }
    //            if (ObjDTROI.Rows[0]["Frequency"].ToString() == "-1")
    //            {
    //                rfvFrequency.Enabled = false;
    //            }
    //            else
    //            {
    //                rfvFrequency.Enabled = true;
    //            }
    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Frequency"], tr_lblFrequency, ddl_Frequency);
    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Repayment_Mode"], tr_lblRepayment_Mode, ddl_Repayment_Mode);

    //            Show_ROI_Forms(ObjDTROI.Rows[0]["IRR_Rest"], tr_lblIRR_Rest, ddl_IRR_Rest);
    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Interest_Calculation"], tr_lblInterest_Calculation, ddl_Interest_Calculation);
    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Interest_Levy"], tr_lblInterest_Levy, ddl_Interest_Levy);

    //            Show_ROI_Forms(ObjDTROI.Rows[0]["Insurance"], tr_lblInsurance, ddl_Insurance);
    //            if (ObjDTROI.Rows[0]["Residual_Value"].ToString() == "0")
    //            {
    //                tr_lblResidual_Value.Visible = false;
    //            }
    //            else
    //            {
    //                Show_ROI_Forms(ObjDTROI.Rows[0]["Residual_Value"], tr_lblResidual_Value, chk_lblResidual_Value);
    //            }
    //            if (ObjDTROI.Rows[0]["Margin"].ToString() == "0")
    //            {
    //                tr_lblMargin.Visible = false;
    //            }
    //            else
    //            {
    //                Show_ROI_Forms(ObjDTROI.Rows[0]["Margin"], tr_lblMargin, chk_lblMargin);

    //            }
    //            if (ObjDTROI.Rows[0]["Margin_Percentage"].ToString() == "" || ObjDTROI.Rows[0]["Margin_Percentage"].ToString().StartsWith("0"))
    //            {
    //                if (ObjDTROI.Rows[0]["Margin"].ToString() == "0")
    //                {
    //                    tr_lblMargin_Percentage.Visible = false;
    //                }
    //                else
    //                {
    //                    Show_ROI_Forms(ObjDTROI.Rows[0]["Margin_Percentage"], tr_lblMargin_Percentage, txt_Margin_Percentage);
    //                }
    //            }
    //            else
    //            {
    //                Show_ROI_Forms(ObjDTROI.Rows[0]["Margin_Percentage"], tr_lblMargin_Percentage, txt_Margin_Percentage);
    //            }
    //            if (ObjDTROI.Rows[0]["Margin"].ToString() == "1")
    //            {
    //                txtMarginMoneyPer_Cashflow.Attributes.Add("readonly", "false");
    //                txtMarginMoneyAmount_Cashflow.Attributes.Add("readonly", "false");
    //                //txtMarginMoneyPer_Cashflow.ReadOnly = true;
    //                //txtMarginMoneyAmount_Cashflow.ReadOnly = true;
    //                rfvMarginPercent.Enabled = true;
    //            }
    //            else
    //            {
    //                txtMarginMoneyPer_Cashflow.Text = "";
    //                //txtMarginMoneyPer_Cashflow.ReadOnly = true;
    //                //txtMarginMoneyAmount_Cashflow.ReadOnly = true;
    //                rfvMarginPercent.Enabled = false;
    //                txtMarginMoneyPer_Cashflow.Attributes.Add("readonly", "true");
    //                txtMarginMoneyAmount_Cashflow.Attributes.Add("readonly", "true");
    //            }
    //            if (ObjDTROI.Rows[0]["Residual_Value"].ToString() == "1")
    //            {
    //                if (txtResidualAmt_Cashflow.Text == "" && txtResidualValue_Cashflow.Text == "")
    //                {
    //                    rfvResidualValue.Enabled = true;
    //                }
    //                else
    //                {
    //                    rfvResidualValue.Enabled = false;
    //                }
    //                txtResidualAmt_Cashflow.ReadOnly = false;
    //                txtResidualValue_Cashflow.ReadOnly = false;

    //            }
    //            else
    //            {
    //                rfvResidualValue.Enabled = false;
    //                txtResidualAmt_Cashflow.Attributes.Add("readonly", "true");
    //                txtResidualValue_Cashflow.Attributes.Add("readonly", "true");
    //            }
    //            if (ddl_Time_Value.SelectedIndex > 0)
    //            {
    //                txtRepaymentTime.Text = ddl_Time_Value.SelectedItem.Text;
    //            }
    //            if (strMode == "C" || strMode == "")//Empty it only in Create Mode.
    //                txtAdvanceInstallments.Text = "";

    //            if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
    //            {

    //                txtAdvanceInstallments.Attributes.Add("readonly", "readonly"); ;
    //            }
    //            else
    //            {

    //                txtAdvanceInstallments.Attributes.Remove("readonly");

    //            }


    //            if (!string.IsNullOrEmpty(ObjDTROI.Rows[0]["IRR_Rate"].ToString()))
    //            {
    //                ViewState["decRate"] = ObjDTROI.Rows[0]["IRR_Rate"].ToString();
    //            }
    //            //hide the IRR panel visibility for WC,FT,TLE(Product) method as per Malolan raised on 23-feb-2012 start by saran
    //            FunPriDisableIRRPanel();
    //            //hide the IRR panel visibility for WC,FT,TLE(Product) method as per Malolan raised on 23-feb-2012 end by saran

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw new ApplicationException("Unable to Load ROI Rule Details");
    //    }
    //    finally
    //    {
    //        ObjCustomerService.Close();
    //    }
    //}

    //hide the IRR panel visibility for WC,FT,TLE(Product) method as per Malolan raised on 23-feb-2012 start by saran
    //private void FunPriDisableIRRPanel()
    //{
    //    if (ddl_Repayment_Mode.SelectedIndex > 0)
    //    {
    //        if (Convert.ToInt32(ddl_Repayment_Mode.SelectedValue) > 0)
    //            if (Convert.ToInt32(ddl_Repayment_Mode.SelectedValue) >= 4)
    //                Panel8.Visible = false;
    //    }
    //}
    ////hide the IRR panel visibility for WC,FT,TLE(Product) method as per Malolan raised on 23-feb-2012 end by saran

    //private decimal FunPriGetMarginAmout()
    //{
    //    decimal decMarginAmount = 0;
    //    try
    //    {
    //        //if (txtMarginMoneyPer_Cashflow.Text != "")
    //        //{
    //        //    decMarginAmount = (Convert.ToDecimal(txtFinanceAmount.Text) * (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) / 100));
    //        //}
    //        //else
    //        //{
    //        //    decMarginAmount = 0;
    //        //}
    //        if (Session["ApplicationAssetDetails"] != null)
    //        {
    //            decimal dcmTotalAssetValue = Convert.ToDecimal(((System.Data.DataTable)Session["ApplicationAssetDetails"]).Compute("Sum(TotalAssetValue)", "Noof_Units > 0"));

    //            decMarginAmount = (dcmTotalAssetValue * (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) / 100));
    //        }
    //        else
    //        {
    //            decMarginAmount = (Convert.ToDecimal(txtFinanceAmount.Text) * (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) / 100));
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //    }
    //    return decMarginAmount;
    //}

    private void Load_Payment_Rule()
    {
        try
        {
            System.Data.DataTable ObjDTPayment = new System.Data.DataTable();

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Rules_ID", ddlPaymentRuleList.SelectedItem.Value);
            Procparam.Add("@Option", "10");
            ObjDTPayment = Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, Procparam);

            System.Data.DataTable ObjDTPaymentGen = new System.Data.DataTable();
            DataColumn dc1 = new DataColumn("FieldName");
            DataColumn dc2 = new DataColumn("FieldValue");
            ObjDTPaymentGen.Columns.Add(dc1);
            ObjDTPaymentGen.Columns.Add(dc2);
            ViewState["PaymentRules"] = ObjDTPaymentGen;
            for (int i = 0; i < ObjDTPayment.Columns.Count; i++)
            {
                if (ObjDTPayment.Rows[0][i].ToString() != string.Empty)
                {
                    DataRow dr = ObjDTPaymentGen.NewRow();
                    dr[0] = ObjDTPayment.Columns[i].ColumnName.Replace("_", " ");
                    if (ObjDTPayment.Rows.Count > 0) dr[1] = ObjDTPayment.Rows[0][i].ToString();
                    else dr[1] = string.Empty;
                    ObjDTPaymentGen.Rows.Add(dr);
                }
            }
            gvPaymentRuleDetails.DataSource = ObjDTPaymentGen;
            gvPaymentRuleDetails.DataBind();
            FunPriLoadPaymentTo();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Payment Rule Details");
        }

    }
    private void FunPriLoadPaymentTo()
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Rules_ID", ddlPaymentRuleList.SelectedItem.Value);
            Procparam.Add("@Option", "10");
            System.Data.DataTable ObjDTPayment = Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, Procparam);
            string vendor = ObjDTPayment.Rows[0]["Entity_Type"].ToString().ToLower();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();


            DropDownList ddlEntityName_InFlowFrom = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
            System.Data.DataTable dtCashFlowFrom = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            switch (vendor)
            {
                case "vendor":
                    dtCashFlowFrom.Rows.RemoveAt(0);
                    break;
                case "customer":
                    dtCashFlowFrom.Rows.RemoveAt(1);
                    break;
            }
            ViewState["vendor"] = vendor;
            ViewState["CashFlowTo"] = dtCashFlowFrom;
            Utility.FillDLL(ddlEntityName_InFlowFrom, dtCashFlowFrom, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("unable to Load Payment To");
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }
    //private void FunPriUpdateROIRule()
    //{
    //    try
    //    {
    //        System.Data.DataTable ObjDTROI = (System.Data.DataTable)ViewState["ROIRules"];
    //        ObjDTROI.Rows[0]["Model_Description"] = txt_Model_Description.Text;
    //        ObjDTROI.Rows[0]["Rate_Type"] = ddl_Rate_Type.SelectedValue;
    //        ObjDTROI.Rows[0]["ROI_Rule_Number"] = txt_ROI_Rule_Number.Text;
    //        ObjDTROI.Rows[0]["Return_Pattern"] = ddl_Return_Pattern.SelectedValue;
    //        ObjDTROI.Rows[0]["Time_Value"] = ddl_Time_Value.SelectedValue;
    //        ObjDTROI.Rows[0]["Frequency"] = ddl_Frequency.SelectedValue;
    //        ObjDTROI.Rows[0]["Repayment_Mode"] = ddl_Repayment_Mode.SelectedValue;
    //        ObjDTROI.Rows[0]["Rate"] = txtRate.Text;
    //        ObjDTROI.Rows[0]["IRR_Rest"] = ddl_IRR_Rest.SelectedValue;
    //        ObjDTROI.Rows[0]["Interest_Calculation"] = ddl_Interest_Calculation.SelectedValue;
    //        ObjDTROI.Rows[0]["Interest_Levy"] = ddl_Interest_Levy.SelectedValue;
    //        ObjDTROI.Rows[0]["Recovery_Pattern_Year1"] = txt_Recovery_Pattern_Year1.Text;
    //        ObjDTROI.Rows[0]["Recovery_Pattern_Year2"] = txt_Recovery_Pattern_Year2.Text;
    //        ObjDTROI.Rows[0]["Recovery_Pattern_Year3"] = txt_Recovery_Pattern_Year3.Text;
    //        ObjDTROI.Rows[0]["Recovery_Pattern_Rest"] = txt_Recovery_Pattern_Rest.Text;
    //        ObjDTROI.Rows[0]["Insurance"] = ddl_Insurance.SelectedValue;
    //        ObjDTROI.Rows[0]["Residual_Value"] = chk_lblResidual_Value.Checked;
    //        ObjDTROI.Rows[0]["Margin"] = chk_lblMargin.Checked;
    //        ObjDTROI.Rows[0]["Margin_Percentage"] = txt_Margin_Percentage.Text == "" ? 0 : Convert.ToDecimal(txt_Margin_Percentage.Text);
    //        ObjDTROI.Rows[0].AcceptChanges();
    //        ViewState["ROIRules"] = ObjDTROI;
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw new ApplicationException("Due to Data Problem, Unable to Update the ROI Rule Details");
    //    }
    //}

    //private void Show_ROI_Forms(Object Data, System.Web.UI.HtmlControls.HtmlTableRow rRow, Object ObjCtl)
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(Convert.ToString(Data)))
    //        {
    //            rRow.Visible = true;
    //            if (ObjCtl.GetType().Name == "TextBox")
    //            {
    //                ((TextBox)ObjCtl).Text = Convert.ToString(Data);
    //            }
    //            if (ObjCtl.GetType().Name == "DropDownList")
    //            {
    //                DropDownList DDL = new DropDownList();
    //                DDL = ((DropDownList)ObjCtl);
    //                if (DDL.Items.Count > 0)
    //                    DDL.SelectedValue = Convert.ToString(Data);
    //                if (Convert.ToString(Data) == "0" || Convert.ToString(Data) == "-1")
    //                {
    //                    rRow.Visible = false;
    //                }
    //            }
    //            if (ObjCtl.GetType().Name == "CheckBox")
    //            {
    //                ((CheckBox)ObjCtl).Checked = Convert.ToBoolean(Data);
    //            }

    //            if (ddlROIRuleList.SelectedItem.Text.ToUpper().Contains("RRA"))//ROI Rule number selected in drop down
    //            {
    //                ((WebControl)ObjCtl).Enabled = false;
    //            }
    //            else
    //            {
    //                if (ObjCtl != null)
    //                {

    //                    if (((WebControl)ObjCtl).ID.Contains("ddl_Time_Value") || ((WebControl)ObjCtl).ID.Contains("ddl_Frequency"))
    //                    {
    //                        //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") || ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM"))
    //                        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK"))
    //                        {
    //                            rRow.Visible = false;
    //                        }
    //                        else
    //                        {
    //                            ((WebControl)ObjCtl).Enabled = true;
    //                        }
    //                    }
    //                    else if (!((WebControl)ObjCtl).ID.Contains("txtRate") && !((WebControl)ObjCtl).ID.Contains("ddl_Insurance") && !((WebControl)ObjCtl).ID.Contains("txt_Margin_Percentage"))
    //                    {
    //                        ((WebControl)ObjCtl).Enabled = false;
    //                    }
    //                    else
    //                    {
    //                        if (!txtStatus.Text.ToUpper().Contains("ACT") || !txtStatus.Text.ToUpper().Contains("CLO") || !txtStatus.Text.ToUpper().Contains("CAN"))
    //                        {
    //                            ((WebControl)ObjCtl).Enabled = true;
    //                        }
    //                        else
    //                        {
    //                            ((WebControl)ObjCtl).Enabled = false;
    //                        }
    //                    }
    //                }
    //            }

    //        }

    //        else
    //            rRow.Visible = false;

    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}

    #region Cash In Flows Grid

    private void FunPriBindInflowDLL(string Mode)
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {


            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyId.ToString());
            if (ddlLOB.SelectedValue != "0")
            {
                objParameters.Add("@LobId", ddlLOB.SelectedValue);
            }
            System.Data.DataSet dsInflow = Utility.GetDataset("s3g_org_loadInflowLov", objParameters);
            ViewState["InflowDDL"] = dsInflow;
            if (Mode == _Add)
            {
                gvInflow.DataSource = null;
                gvInflow.DataBind();

                DtCashFlow = new System.Data.DataTable();
                DtCashFlow.Columns.Add("Date");
                DtCashFlow.Columns.Add("CashInFlowID");
                DtCashFlow.Columns.Add("CashInFlow");
                DtCashFlow.Columns.Add("EntityID");
                DtCashFlow.Columns.Add("Entity");
                DtCashFlow.Columns.Add("InflowFromId");
                DtCashFlow.Columns.Add("InflowFrom");
                DtCashFlow.Columns.Add("Remarks");
                DtCashFlow.Columns.Add("Amount", typeof(decimal));
                DtCashFlow.Columns.Add("Accounting_IRR");
                DtCashFlow.Columns.Add("Business_IRR");
                DtCashFlow.Columns.Add("Company_IRR");
                DtCashFlow.Columns.Add("CashFlow_Flag_ID", typeof(int));
                DtCashFlow.Columns.Add("CashFlowID", typeof(int));

                DataRow dr = DtCashFlow.NewRow();
                dr["Date"] = "01/01/1900";
                dr["CashInFlowID"] = "";
                dr["CashInFlow"] = "";
                dr["EntityID"] = "";
                dr["Entity"] = "";
                dr["InflowFromId"] = "";
                dr["InflowFrom"] = "";
                dr["Amount"] = 0;
                dr["Accounting_IRR"] = "";
                dr["Business_IRR"] = "";
                dr["Company_IRR"] = "";
                dr["CashFlow_Flag_ID"] = 0;
                dr["CashFlowID"] = 0;
                DtCashFlow.Rows.Add(dr);

            }
            if (Mode == _Edit)
            {
                if ((System.Data.DataTable)ViewState["DtCashFlow"] != null)
                    DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];

            }

            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();
            if (Mode == _Add)
            {
                DtCashFlow.Rows.Clear();
                ViewState["DtCashFlow"] = DtCashFlow;
                DtCashFlow.Dispose();
                gvInflow.Rows[0].Cells.Clear();
                gvInflow.Rows[0].Visible = false;

            }
            FunPriGenerateNewInflowRow();


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriGenerateNewInflowRow()
    {
        try
        {
            DropDownList ddlInflowDesc = gvInflow.FooterRow.FindControl("ddlInflowDesc") as DropDownList;
            //DropDownList ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;

            Utility.FillDLL(ddlInflowDesc, ((System.Data.DataSet)ViewState["InflowDDL"]).Tables[2], true);
            //Utility.FillDLL(ddlEntityName_InFlow, ((System.Data.DataSet)ViewState["InflowDDL"]).Tables[1], true);
            Utility.FillDLL(ddlEntityName_InFlowFrom, ((System.Data.DataSet)ViewState["InflowDDL"]).Tables[0], true);
            ddlEntityName_InFlowFrom.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Generate New Inflow");
        }
    }

    protected void CashInflow_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
            TextBox txtDate_GridInflow1 = gvInflow.FooterRow.FindControl("txtDate_GridInflow") as TextBox;
            DropDownList ddlInflowDesc1 = gvInflow.FooterRow.FindControl("ddlInflowDesc") as DropDownList;
            //DropDownList ddlEntityName_InFlow1 = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow1 = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;
            TextBox txtAmount_Inflow1 = gvInflow.FooterRow.FindControl("txtAmount_Inflow") as TextBox;
            TextBox txtFooterRemarks = gvInflow.FooterRow.FindControl("txtFooterRemarks") as TextBox;

            string[] strArrayIds = ddlInflowDesc1.SelectedValue.Split(',');

            DataRow[] drOutflowOL = DtCashFlow.Select(" Date = '" + Utility.StringToDate(txtDate_GridInflow1.Text) +
                "' and CashFlow_Flag_ID = " + strArrayIds[4] + " and InflowFromId = " + ddlEntityName_InFlowFrom.SelectedValue +
                " and EntityID = " + ddlEntityName_InFlow1.SelectedValue);
            if (drOutflowOL.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Inflow", "alert('Cash flow cannot be repeated for the same date with same Customer/Entity');", true);
                return;
            }


            DataRow dr = DtCashFlow.NewRow();
            //DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["Date"] };
            dr["Date"] = Utility.StringToDate(txtDate_GridInflow1.Text);
            dr["CashInFlowID"] = strArrayIds[0];
            dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
            dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
            dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
            dr["CashFlow_Flag_ID"] = strArrayIds[4];
            dr["CashInFlow"] = ddlInflowDesc1.SelectedItem;
            dr["EntityID"] = ddlEntityName_InFlow1.SelectedValue;
            dr["Entity"] = ddlEntityName_InFlow1.SelectedText;
            dr["InflowFromId"] = ddlEntityName_InFlowFrom.SelectedValue;
            dr["InflowFrom"] = ddlEntityName_InFlowFrom.SelectedItem;
            dr["Amount"] = txtAmount_Inflow1.Text;
            dr["Remarks"] = txtFooterRemarks.Text;
            //DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["Date"], DtCashFlow.Columns["CashInFlowID"], 
            //DtCashFlow.Columns["InflowFromId"], DtCashFlow.Columns["EntityID"] };
            DtCashFlow.Rows.Add(dr);

            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();

            ViewState["DtCashFlow"] = DtCashFlow;

            FunPriCalculateServiceTax(Convert.ToInt32( strArrayIds[0]), Convert.ToDecimal(txtAmount_Inflow1.Text), ddlEntityName_InFlow1.SelectedValue, ddlEntityName_InFlow1.SelectedText
        , txtDate_GridInflow1.Text, ddlEntityName_InFlowFrom.SelectedValue, ddlEntityName_InFlowFrom.SelectedItem.Text);

            FunPriGenerateNewInflowRow();
            txtAccountingIRR.Text =
            txtAccountIRR_Repay.Text =
            txtBusinessIRR.Text =
            txtBusinessIRR_Repay.Text =
            txtCompanyIRR.Text =
            txtCompanyIRR_Repay.Text = "";
            //FunPriIRRReset();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            if (ex.Message.Contains("Column 'Date, CashInFlowID, InflowFromID, EntityID' is constrained to be unique"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Inflow", "alert('Cash flow cannot be repeated for the same date with same Customer/Entity');", true);
            }
            else
            {
                cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Add Inflow";
                cv_TabMainPage.IsValid = false;
            }
        }
    }

    private void FunPriCalculateServiceTax(Int32 intCashflowID, decimal decCashflowAmount, string strEntityID, string strEntityFrom
        , string strDate, string strInflowFromID, string strInflowFrom)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "4");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Param1", Convert.ToString(intCashflowID));
            Procparam.Add("@Param2", Convert.ToString(Utility.StringToDate(strDate)));
            Procparam.Add("@Param4", Convert.ToString(decCashflowAmount));
            Procparam.Add("@Param3", Convert.ToString(ddlBranchList.SelectedValue));
            Procparam.Add("@Param5", Convert.ToString(ddlBillingState.SelectedValue));

            DataSet dsST = Utility.GetDataset("S3G_GET_DefaultCMNLST", Procparam);

            if (dsST != null && dsST.Tables.Count > 0 && dsST.Tables[0].Rows.Count > 0)
            {
                DtCashFlow = (DataTable)ViewState["DtCashFlow"];
                DtCashFlow.PrimaryKey = null;

                DataRow dr;

                for (int i = 0; i < dsST.Tables[0].Rows.Count; i++)
                {
                    dr = DtCashFlow.NewRow();
                    dr["Date"] = Utility.StringToDate(strDate);
                    dr["CashInFlowID"] = Convert.ToString(dsST.Tables[0].Rows[i]["CashInFlowID"]);
                    dr["Accounting_IRR"] = Convert.ToBoolean(dsST.Tables[0].Rows[i]["Accounting_IRR"]);
                    dr["Business_IRR"] = Convert.ToBoolean(dsST.Tables[0].Rows[i]["Business_IRR"]);
                    dr["Company_IRR"] = Convert.ToBoolean(dsST.Tables[0].Rows[i]["Company_IRR"]);
                    dr["CashFlow_Flag_ID"] = Convert.ToString(dsST.Tables[0].Rows[0]["CashFlow_Flag_ID"]);
                    dr["CashInFlow"] = Convert.ToString(dsST.Tables[0].Rows[i]["CashInFlow"]);
                    dr["EntityID"] = Convert.ToString(strEntityID);
                    dr["Entity"] = Convert.ToString(strEntityFrom);
                    dr["InflowFromId"] = Convert.ToString(strInflowFromID);
                    dr["InflowFrom"] = Convert.ToString(strInflowFrom);
                    dr["Amount"] = Convert.ToString(dsST.Tables[0].Rows[i]["Amount"]);
                    dr["CashFlowID"] = Convert.ToString(dsST.Tables[0].Rows[i]["Parent_CashFlow_Flag_ID"]);    //Added on 31-Jan-2017

                    DtCashFlow.Rows.Add(dr);
                }

                gvInflow.DataSource = DtCashFlow;
                gvInflow.DataBind();

                ViewState["DtCashFlow"] = DtCashFlow;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }


    protected void gvInflow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
            if (DtCashFlow.Rows.Count > 0)
            {
                //FunPriIRRReset();
                txtAccountingIRR.Text =
                txtAccountIRR_Repay.Text =
                txtBusinessIRR.Text =
                txtBusinessIRR_Repay.Text =
                txtCompanyIRR.Text =
                txtCompanyIRR_Repay.Text = "";
                DtCashFlow.Rows.RemoveAt(e.RowIndex);
                ViewState["DtCashFlow"] = DtCashFlow;
                if (DtCashFlow.Rows.Count == 0)
                {
                    FunPriBindInflowDLL(_Add);
                }
                else
                {
                    gvInflow.DataSource = DtCashFlow;
                    gvInflow.DataBind();
                    FunPriGenerateNewInflowRow();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Remove Inflow";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void gvInflow_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            /* Initialized For Security Deposit CashFlow on Dec 18,2015 */
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TextBox txtDate_GrdInflowSecuDept = e.Row.FindControl("txtDate_GrdInflowSecuDept") as TextBox;
                //txtDate_GrdInflowSecuDept.Attributes.Add("readonly", "readonly");
                Label lblCashFlow_Flag_ID = e.Row.FindControl("lblCashFlow_Flag_ID") as Label;
                AjaxControlToolkit.CalendarExtender CalendarExtenderSecuDept_InflowDate = e.Row.FindControl("CalendarExtenderSecuDept_InflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSecuDept_InflowDate.Format = ObjS3GSession.ProDateFormatRW;
            }
            /* Initialized For Security Deposit CashFlow on Dec 18,2015 */

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridInflow1 = e.Row.FindControl("txtDate_GridInflow") as TextBox;
                txtDate_GridInflow1.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_InflowDate1 = e.Row.FindControl("CalendarExtenderSD_InflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_InflowDate1.Format = ObjS3GSession.ProDateFormatRW;
                TextBox txtAmount_Inflow = e.Row.FindControl("txtAmount_Inflow") as TextBox;
                txtAmount_Inflow.SetDecimalPrefixSuffix(10, 0, true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Set the Date format in Inflow Date";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void gvInflow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            /* Initialized For Security Deposit CashFlow on Dec 18,2015 */
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDate_GrdInflowSecuDept = e.Row.FindControl("txtDate_GrdInflowSecuDept") as TextBox;
                txtDate_GrdInflowSecuDept.Attributes.Add("onblur", "fnDoDate(this,'" + txtDate_GrdInflowSecuDept.ClientID + "','" + strDateFormat + "',false,  false);");
                Label lblCashFlow_Flag_ID = e.Row.FindControl("lblCashFlow_Flag_ID") as Label;
                AjaxControlToolkit.CalendarExtender CalendarExtenderSecuDept_InflowDate = e.Row.FindControl("CalendarExtenderSecuDept_InflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSecuDept_InflowDate.Format = ObjS3GSession.ProDateFormatRW;
                if (strMode != "Q")
                {
                    if (lblCashFlow_Flag_ID.Text != String.Empty && lblCashFlow_Flag_ID.Text != "0")
                    {
                        if (lblCashFlow_Flag_ID.Text == "30")
                        {
                            CalendarExtenderSecuDept_InflowDate.Enabled = true;
                            txtDate_GrdInflowSecuDept.ReadOnly = false;
                        }
                        else
                        {
                            CalendarExtenderSecuDept_InflowDate.Enabled = false;
                            txtDate_GrdInflowSecuDept.ReadOnly = true;
                        }
                    }
                }
            }
            /* Initialized For Security Deposit CashFlow on Dec 18,2015 */
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Set the Date format in Inflow Date";
            cv_TabMainPage.IsValid = false;
        }
    }
    protected void TxtId_TextChanged(object sender, EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
        TextBox txt = (TextBox)currentRow.FindControl("TxtId");
        Int32 count = Convert.ToInt32(txt.Text);
        txt.Text = Convert.ToString(count + 10);

    }

    protected void txtDate_GrdInflowSecuDept_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            TextBox txtDate_GrdInflowSecuDept = (TextBox)currentRow.FindControl("txtDate_GrdInflowSecuDept");
            if (txtDate_GrdInflowSecuDept.Text != String.Empty)
            {
                Session["strSecuDepstDate"] = txtDate_GrdInflowSecuDept.Text;
                DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
                DataRow[] drCashflow = DtCashFlow.Select("CashFlow_Flag_ID = 30");
                if (drCashflow.Length > 0)
                {
                    drCashflow[0]["Date"] = Utility.StringToDate(txtDate_GrdInflowSecuDept.Text);
                    DtCashFlow.AcceptChanges();
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "InFlow Date should not be Empty");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to Change InFlow Date";
            cv_TabMainPage.IsValid = false;
        }
    }


    protected void ddlEntityName_InFlowFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;
            if (ddlEntityName_InFlowFrom.SelectedItem.Text.ToUpper() == "CUSTOMER")
            {
                if (S3GCustomerAddress1.CustomerName != string.Empty)
                {
                    //ddlEntityName_InFlow.Items.Clear();
                    //ListItem lstItem = new ListItem(S3GCustomerAddress1.CustomerCode + " - " + S3GCustomerAddress1.CustomerName, hdnCustomerId.Value);
                    //ddlEntityName_InFlow.Items.Add(lstItem);

                    ddlEntityName_InFlow.Clear();
                    //ddlEntityName_InFlow.SelectedValue = hdnCustomerId.Value; 
                    ddlEntityName_InFlow.SelectedValue = ViewState["Customer_Id"].ToString();
                    ddlEntityName_InFlow.SelectedText = S3GCustomerAddress1.CustomerCode + " - " + S3GCustomerAddress1.CustomerName;
                    ddlEntityName_InFlow.ReadOnly = true;
                }
            }
            else
            {
                ddlEntityName_InFlow.ReadOnly = false;
                ddlEntityName_InFlow.Clear();

                //ddlEntityName_InFlow.BindDataTable(((System.Data.DataSet)ViewState["InflowDDL"]).Tables[1]);

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to Load Customer/Entity Name";
            cv_TabMainPage.IsValid = false;
        }


    }

    #endregion



    #region Cash Out Flow Grid

    private void FunPriBindOutflowDLL(string Mode)
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyId.ToString());
            if (ddlLOB.SelectedValue != "0")
            {
                objParameters.Add("@LobId", ddlLOB.SelectedValue);
            }
            System.Data.DataSet dsInflow = Utility.GetDataset("s3g_org_loadOutflowLov", objParameters);
            ViewState["OutflowDDL"] = dsInflow;

            if (Mode == _Add)
            {
                //Code modified by Nataraj Y
                DtCashFlowOut = new System.Data.DataTable();
                DtCashFlowOut.Columns.Add("Date");
                DtCashFlowOut.Columns.Add("CashOutFlowID");
                DtCashFlowOut.Columns.Add("CashOutFlow");
                DtCashFlowOut.Columns.Add("EntityID");
                DtCashFlowOut.Columns.Add("Entity");
                DtCashFlowOut.Columns.Add("OutflowFromId");
                DtCashFlowOut.Columns.Add("OutflowFrom");
                DtCashFlowOut.Columns.Add("Amount");
                DtCashFlowOut.Columns.Add("Remarks");
                DtCashFlowOut.Columns.Add("Accounting_IRR");
                DtCashFlowOut.Columns.Add("Business_IRR");
                DtCashFlowOut.Columns.Add("Company_IRR");
                DtCashFlowOut.Columns.Add("CashFlow_Flag_ID", typeof(int));
                DtCashFlowOut.Columns["Amount"].DataType = typeof(decimal);
                //DtCashFlowOut.PrimaryKey = new DataColumn[] { DtCashFlowOut.Columns["CashOutFlowID"], DtCashFlowOut.Columns["Date"], DtCashFlowOut.Columns["EntityID"] };
                DataRow dr_out = DtCashFlowOut.NewRow();
                dr_out["Date"] = "01/01/1900";
                dr_out["CashOutFlowID"] = "";
                dr_out["CashOutFlow"] = "";
                dr_out["EntityID"] = "";
                dr_out["Entity"] = "";
                dr_out["OutflowFromId"] = "";
                dr_out["OutflowFrom"] = "";
                dr_out["Amount"] = "0";
                dr_out["Accounting_IRR"] = "";
                dr_out["Business_IRR"] = "";
                dr_out["Company_IRR"] = "";
                dr_out["CashFlow_Flag_ID"] = 0;
                DtCashFlowOut.Rows.Add(dr_out);

            }
            if (Mode == _Edit)
            {
                if ((System.Data.DataTable)ViewState["DtCashFlowOut"] != null)
                    DtCashFlowOut = (System.Data.DataTable)ViewState["DtCashFlowOut"];
            }
            gvOutFlow.DataSource = DtCashFlowOut;
            gvOutFlow.DataBind();

            if (Mode == _Add)
            {

                DtCashFlowOut.Rows.Clear();
                ViewState["DtCashFlowOut"] = DtCashFlowOut;
                DtCashFlowOut.Dispose();

                gvOutFlow.Rows[0].Cells.Clear();
                gvOutFlow.Rows[0].Visible = false;
            }
            FunPriGenerateNewOutflowRow();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Outflow Details");

        }
        finally
        {

            ObjCustomerService.Close();
        }
    }

    private void FunPriGenerateNewOutflowRow()
    {
        try
        {
            DropDownList ddlInflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            //DropDownList ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlEntityName_InFlowFrom = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;

            Utility.FillDLL(ddlInflowDesc, ((System.Data.DataSet)ViewState["OutflowDDL"]).Tables[2], true);

            Utility.FillDLL(ddlInflowDesc, ((System.Data.DataSet)ViewState["OutflowDDL"]).Tables[2], true);
            if (ViewState["OutflowDDL"] != null)
            {
                System.Data.DataTable dtCashFlowFrom = ((System.Data.DataSet)ViewState["OutflowDDL"]).Tables[0];
                string vendor = (string)ViewState["vendor"];
                // --code commented and added by saran in 01-Aug-2014 Insurance start 
                //if (dtCashFlowFrom.Rows.Count > 1)
                if (dtCashFlowFrom.Rows.Count > 2)
                // --code commented and added by saran in 01-Aug-2014 Insurance end 
                {
                    switch (vendor)
                    {
                        case "vendor":
                            dtCashFlowFrom.Rows.RemoveAt(0);
                            break;
                        case "customer":
                            dtCashFlowFrom.Rows.RemoveAt(1);
                            break;
                    }
                    ((System.Data.DataSet)ViewState["OutflowDDL"]).Merge(dtCashFlowFrom);
                }
                Utility.FillDLL(ddlEntityName_InFlowFrom, dtCashFlowFrom, true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Generate New Outflow");
        }
    }

    protected void gvOutFlow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtCashFlowOut = (System.Data.DataTable)ViewState["DtCashFlowOut"];
            if (DtCashFlowOut.Rows.Count > 0)
            {

                DataRow[] drOutflowOL = DtCashFlowOut.Select("CashFlow_Flag_ID=41");
                if (drOutflowOL.Length > 0)
                {
                    foreach (DataRow der in drOutflowOL)
                        der.Delete();
                    DtCashFlowOut.AcceptChanges();
                }

                DtCashFlowOut.Rows.RemoveAt(e.RowIndex);
                ViewState["DtCashFlowOut"] = DtCashFlowOut;
                if (DtCashFlowOut.Rows.Count == 0)
                {
                    FunPriBindOutflowDLL(_Add);
                    lblTotalOutFlowAmount.Text = "0";
                    FunPriIRRReset();
                }
                else
                {
                    FunProBindCashFlow();
                    FunPriIRRReset();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Remove Outflow";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void CashOutflow_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            System.Data.DataTable dtAcctype = ((System.Data.DataTable)ViewState["PaymentRules"]);
            if (dtAcctype != null && dtAcctype.Rows.Count > 0)
            {
                DtCashFlowOut = (System.Data.DataTable)ViewState["DtCashFlowOut"];

                TextBox txtDate_GridOutflow = gvOutFlow.FooterRow.FindControl("txtDate_GridOutflow") as TextBox;
                DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
                DropDownList ddlPaymentto_OutFlow = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
                //DropDownList ddlEntityName_OutFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
                UserControls_S3GAutoSuggest ddlEntityName_OutFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as UserControls_S3GAutoSuggest;
                TextBox txtAmount_Outflow = gvOutFlow.FooterRow.FindControl("txtAmount_Outflow") as TextBox;
                TextBox txtFooterRemarks = gvOutFlow.FooterRow.FindControl("txtFooterRemarks") as TextBox;
                DataRow dr = DtCashFlowOut.NewRow();
                dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";

                if (Utility.CompareDates(txtAccountDate.Text, txtDate_GridOutflow.Text) == -1)
                {
                    Utility.FunShowAlertMsg(this, "Outflow date cannot be less than Rental Schedule date");
                    return;
                }

                //DtCashFlowOut.PrimaryKey = new DataColumn[] { DtCashFlowOut.Columns["Date"], DtCashFlowOut.Columns["CashOutFlowID"], 
                //DtCashFlowOut.Columns["OutflowFromId"], DtCashFlowOut.Columns["EntityID"] };
                dr["Date"] = Utility.StringToDate(txtDate_GridOutflow.Text);
                string[] strArrayIds = ddlOutflowDesc.SelectedValue.Split(',');

                DataRow[] drOutflowOL = DtCashFlowOut.Select(" Date = '" + Utility.StringToDate(txtDate_GridOutflow.Text) +
                    "' and CashFlow_Flag_ID = " + strArrayIds[4] + " and OutflowFromId = " + ddlPaymentto_OutFlow.SelectedValue +
                    " and EntityID = " + ddlEntityName_OutFlow.SelectedValue);
                if (drOutflowOL.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Inflow", "alert('Cash flow cannot be repeated for the same date with same Customer/Entity');", true);
                    return;
                }


                if (strArrayIds[4] == "41")
                {
                    if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Trade Advance" || dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Normal Payment")
                    {
                        if (Utility.StringToDate(txtAccountDate.Text) != Utility.StringToDate(txtDate_GridOutflow.Text))
                        {
                            Utility.FunShowAlertMsg(this, "Outflow date should be equal to Rental Schedule date for Normal Payment/Trade Advance");
                            return;
                        }
                    }
                    if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Deferred Payment")
                    {
                        if (Utility.CompareDates(txtAccountDate.Text, txtDate_GridOutflow.Text) == 0)
                        {
                            Utility.FunShowAlertMsg(this, "Outflow date should be greater than Rental Schedule date for Deferred Payment");
                            return;
                        }
                    }
                }
                dr["CashOutFlowID"] = strArrayIds[0];
                dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
                dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
                dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
                dr["CashFlow_Flag_ID"] = strArrayIds[4];
                dr["CashOutFlow"] = ddlOutflowDesc.SelectedItem;
                dr["OutflowFrom"] = ddlPaymentto_OutFlow.SelectedItem;
                dr["OutflowFromId"] = ddlPaymentto_OutFlow.SelectedValue;
                dr["EntityID"] = ddlEntityName_OutFlow.SelectedValue;
                dr["Entity"] = ddlEntityName_OutFlow.SelectedText;
                dr["Amount"] = txtAmount_Outflow.Text;
                dr["Remarks"] = txtFooterRemarks.Text;
                DtCashFlowOut.Rows.Add(dr);
                ViewState["DtCashFlowOut"] = DtCashFlowOut;

                FunProBindCashFlow();
                FunPriIRRReset();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);

            if (ex.Message.Contains("Column 'Date, CashOutFlowID' is constrained to be unique"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Outflow", "alert('Cash flow cannot be repeated for the same date');", true);
            }
            else if (ex.Message.Contains("Column 'Date, CashOutFlowID, OutflowFromId, EntityID' is constrained to be unique"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Outflow", "alert('Cash flow cannot be repeated for the same date with same Customer/Entity');", true);
            }
            else
            {
                cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Add Outflow";
                cv_TabMainPage.IsValid = false;
            }
        }

    }

    protected void FunProBindCashFlow()
    {
        try
        {
            DtCashFlowOut = (System.Data.DataTable)ViewState["DtCashFlowOut"];
            DataTable dtOutflowNew = DtCashFlowOut.Copy();
            try
            {

                DataRow[] drOutflowOL = dtOutflowNew.Select("CashFlow_Flag_ID=41");
                if (drOutflowOL.Length > 0)
                {
                    foreach (DataRow der in drOutflowOL)
                        der.Delete();
                    dtOutflowNew.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
            }
            lblTotalOutFlowAmount.Text = dtOutflowNew.Compute("sum(Amount)", "CashOutFlowID > 0").ToString();
            gvOutFlow.DataSource = dtOutflowNew;
            gvOutFlow.DataBind();
            FunPriGenerateNewOutflowRow();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void ddlPaymentto_OutFlow_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlPaymentto_OutFlow = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
            TextBox txtAmount_Outflow = gvOutFlow.FooterRow.FindControl("txtAmount_Outflow") as TextBox;

            ddlEntityName_InFlow.Clear();
            ddlEntityName_InFlow.ReadOnly = true;

            if (ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "CUSTOMER")
            {
                if (S3GCustomerAddress1.CustomerName != string.Empty) //SelectedIndex > 0)
                {
                    //ddlEntityName_InFlow.Items.Clear();
                    //ListItem lstItem = new ListItem(S3GCustomerAddress1.CustomerName, hdnCustomerId.Value);
                    //ddlEntityName_InFlow.Items.Add(lstItem);
                    //ddlEntityName_InFlow.SelectedValue = hdnCustomerId.Value;
                    ddlEntityName_InFlow.SelectedValue = ViewState["Customer_Id"].ToString();
                    ddlEntityName_InFlow.SelectedText = S3GCustomerAddress1.CustomerCode + " - " + S3GCustomerAddress1.CustomerName;
                }


            }
            // --code commented and added by saran in 01-Aug-2014 Insurance start  

            else if (ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "ENTITY" || ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "INSURANCE COMPANY")
            {
                Procparam = new Dictionary<string, string>();
                DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
                string[] strArrayIds = ddlOutflowDesc.SelectedValue.Split(',');
                Procparam.Add("@Option", "11");
                Procparam.Add("@Company_ID", intCompanyId.ToString());

                // --code commented and added by saran in 01-Aug-2014 Insurance start  
                if (ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "INSURANCE COMPANY")
                    ddlEntityName_InFlow.ServiceMethod = "GetVendorsInsurance";
                else
                    ddlEntityName_InFlow.ServiceMethod = "GetVendors";

                // --code commented and added by saran in 01-Aug-2014 Insurance end  
                ddlEntityName_InFlow.ReadOnly = false;
                ddlEntityName_InFlow.Clear();

                if (strArrayIds.Length >= 4)
                {
                    if (strArrayIds[4].ToString() == "41")
                    {
                        Procparam.Add("@ID", "1");
                        if (Session["ApplicationAssetDetails"] != null)
                        {
                            if (((System.Data.DataTable)Session["ApplicationAssetDetails"]).Rows.Count > 0)
                            {
                                DataRow[] dr = ((System.Data.DataTable)Session["ApplicationAssetDetails"]).Select("Pay_To_ID=137");//Entity
                                if (dr != null)
                                {
                                    if (dr.Length > 0)
                                    {
                                        ddlEntityName_InFlow.ServiceMethod = "GetAssetVendors";
                                        //ddlEntityName_InFlow.FillDataTable(dr.CopyToDataTable(), "Entity_ID", "Entity_Code");
                                        if (dr.Length == 1)
                                        {
                                            //ddlEntityName_InFlow.SelectedIndex = 1;
                                            ddlEntityName_InFlow.ReadOnly = true;
                                            ddlEntityName_InFlow.SelectedValue = dr[0]["Entity_ID"].ToString();
                                            ddlEntityName_InFlow.SelectedText = dr[0]["Entity_Code"].ToString();

                                            txtAmount_Outflow.Text = txtFinanceAmount.Text;
                                        }
                                        return;
                                    }
                                }

                            }
                        }

                    }
                }
                //ddlEntityName_InFlow.BindDataTable(SPNames.S3G_ORG_GetPricing_List, Procparam, true, new string[] { "Entity_ID", "Entity_Code", "Entity_Name" });

            }
            //Code Added by saran for UAT Fix in round 4 on 18-Jul-2012 end 
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Load Customer/Entity Name";
            cv_TabMainPage.IsValid = false;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Entity_Type", "8");
        Procparam.Add("@PrefixText", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));

        return suggetions.ToArray();

    }


    // --code commented and added by saran in 01-Aug-2014 Insurance start  
    [System.Web.Services.WebMethod]
    public static string[] GetVendorsInsurance(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Entity_Type", "11");
        Procparam.Add("@PrefixText", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));

        return suggetions.ToArray();

    }
    // --code commented and added by saran in 01-Aug-2014 Insurance end  

    [System.Web.Services.WebMethod]
    public static string[] GetAssetVendors(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            System.Data.DataTable dtCommon = new System.Data.DataTable();
            System.Data.DataSet Ds = new System.Data.DataSet();

            System.Data.DataTable ApplicationAssetDetails = (System.Data.DataTable)obj_PageValue.Session["ApplicationAssetDetails"];
            DataRow[] dr = ApplicationAssetDetails.Select("Pay_To_ID=137 and Entity_Code like '%" + prefixText + "%'");

            System.Data.DataTable dtAssetEntity = new System.Data.DataTable();
            if (dr.Length == 0)
                dtAssetEntity = ApplicationAssetDetails.Clone();
            else
                dtAssetEntity = (dr.CopyToDataTable().Copy());

            dtAssetEntity.Columns["Entity_ID"].ColumnName = "ID";
            dtAssetEntity.Columns["Entity_Code"].ColumnName = "Name";

            suggetions = Utility.GetSuggestions(dtAssetEntity, true);

            return suggetions.ToArray();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void gvOutFlow_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridOutflow = e.Row.FindControl("txtDate_GridOutflow") as TextBox;
                txtDate_GridOutflow.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_OutflowDate = e.Row.FindControl("CalendarExtenderSD_OutflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_OutflowDate.Format = ObjS3GSession.ProDateFormatRW;
                TextBox txtAmount_Outflow = e.Row.FindControl("txtAmount_Outflow") as TextBox;
                txtAmount_Outflow.SetDecimalPrefixSuffix(10, 0, true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Set the Date format in Outflow Date";
            cv_TabMainPage.IsValid = false;
        }
    }

    #endregion

    //protected void ddlROIRuleList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlROIRuleList.SelectedIndex > 0)
    //        {
    //            Load_ROI_Rule();
    //            if (txtMarginMoneyPer_Cashflow.Text != "")
    //            {
    //                txtMarginMoneyAmount_Cashflow.Text = FunPriGetMarginAmout().ToString();
    //            }
    //            FunPriBindRepaymentDLL(_Add);
    //            FunPriIRRReset();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Load ROI Rule Details";
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}

    protected void ddlPaymentRuleList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlPaymentRuleList.SelectedIndex > 0)
            {
                Load_Payment_Rule();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Load Payment Rule Details";
            cv_TabMainPage.IsValid = false;
        }
    }

    #endregion

    #region Repayment Details Tab

    private void FunPriBindRepaymentDLL(string Mode)
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            if (Mode == _Add)
            {
                gvRepaymentDetails.DataSource = null;
                gvRepaymentDetails.DataBind();
                ObjStatus.Option = 52;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            }
            if (Mode == _Edit)
            {
                if ((System.Data.DataTable)ViewState["DtRepayGrid"] != null)
                    DtRepayGrid = (System.Data.DataTable)ViewState["DtRepayGrid"];

            }
            gvRepaymentDetails.DataSource = DtRepayGrid;
            gvRepaymentDetails.DataBind();
            if (Mode == _Add)
            {
                DtRepayGrid.Rows.Clear();
                ViewState["DtRepayGrid"] = DtRepayGrid;
                gvRepaymentDetails.Rows[0].Cells.Clear();
                gvRepaymentDetails.Rows[0].Visible = false;
            }
            FunPriGenerateNewRepaymentRow();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriGenerateNewRepaymentRow()
    {
        try
        {
            DropDownList ddlRepaymentCashFlow_RepayTab = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            Utility.FillDLL(ddlRepaymentCashFlow_RepayTab, ((System.Data.DataSet)ViewState["InflowDDL"]).Tables[3], true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Cashflow Description in Repayment");
        }
    }

    protected void btnAddRepayment_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtRepayGrid = (System.Data.DataTable)ViewState["DtRepayGrid"];
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            DateTime dtNextFromdate; DateTime dtStartdate;
            DropDownList ddlRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            TextBox txtAmountRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtAmountRepaymentCashFlow_RepayTab") as TextBox;
            TextBox txtPerInstallmentAmount_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            TextBox txtBreakup_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;
            TextBox txtFromInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            TextBox txtToInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToInstallment_RepayTab") as TextBox;
            TextBox txtfromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            TextBox txtToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToDate_RepayTab") as TextBox;
            string[] strIds = ddlRepaymentCashFlow_RepayTab1.SelectedValue.ToString().Split(',');
            if (DtRepayGrid.Rows.Count > 0)
            {
                //if (ddlLOB.SelectedItem.Text.Contains("TL") || ddlLOB.SelectedItem.Text.Contains("TE"))
                //{
                //    objRepaymentStructure.FunPubGetNextRepaydateTL(DtRepayGrid, ddl_Frequency.SelectedValue, ddlRepaymentCashFlow_RepayTab1.SelectedValue);
                //}
                //else
                //{
                //objRepaymentStructure.FunPubGetNextRepaydate(DtRepayGrid, ddl_Frequency.SelectedValue);
                objRepaymentStructure.FunPubGetNextRepaydate(DtRepayGrid, hdnRentfrequency.Value);
                //}
                if (txtfromdate_RepayTab1.Text != "")
                {
                    if (Utility.StringToDate(txtfromdate_RepayTab1.Text) < objRepaymentStructure.dtNextDate)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlapped');", true);
                        return;
                    }
                    /*Changed by Prabhu.K on 23-Nov-2011 for UAT Issue*/
                    else if (Utility.StringToDate(txtfromdate_RepayTab1.Text) != objRepaymentStructure.dtNextDate)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Date should be " + objRepaymentStructure.dtNextDate.ToString(strDateFormat) + "');", true);
                        return;
                    }
                }
            }
            System.Data.DataTable dtMoratorium = new DataTable();
            if (ViewState["dtMoratorium"] != null)
            {
                dtMoratorium = (System.Data.DataTable)ViewState["dtMoratorium"];
            }
            if (dtMoratorium.Rows.Count > 0 && strIds[4] == "23")
            {
                DataRow[] drMoratoriumRangeExist = dtMoratorium.Select("ToDate >= #" + Utility.StringToDate(txtfromdate_RepayTab1.Text) + "#");
                if (drMoratoriumRangeExist.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Selected Installment Period Exist in Moratorium');", true);
                    return;
                }
            }
            else
            {
                if (DtRepayGrid.Rows.Count > 0)
                {
                    DataRow[] drRepayDetail = null;
                    drRepayDetail = DtRepayGrid.Select(" CASHFLOW_FLAG_ID = " + strIds[4] +
                        " and (( " + txtFromInstallment_RepayTab1.Text.Trim() + " >= FROMINSTALL " +
                        " and " + txtFromInstallment_RepayTab1.Text.Trim() + " <= TOINSTALL ) or " +
                        " ( " + txtToInstallment_RepayTab1.Text.Trim() + " >= FROMINSTALL and " +
                        txtToInstallment_RepayTab1.Text.Trim() + " <= TOINSTALL) or " +
                        " ( FROMINSTALL >= " + txtFromInstallment_RepayTab1.Text.Trim() +
                        " and FROMINSTALL <= " + txtToInstallment_RepayTab1.Text.Trim() + " ))");
                    if (drRepayDetail.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlaped');", true);
                        txtToInstallment_RepayTab1.Focus();
                        return;
                    }
                }
            }
            Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
            //objMethodParameters.Add("REPAYMODE", ddl_Repayment_Mode.SelectedItem.Text.ToString());
            objMethodParameters.Add("REPAYMODE", "1");//Default EMI
            objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text.ToString());
            objMethodParameters.Add("CashFlow", ddlRepaymentCashFlow_RepayTab1.SelectedItem.Text);
            objMethodParameters.Add("CashFlowId", ddlRepaymentCashFlow_RepayTab1.SelectedValue);
            objMethodParameters.Add("PerInstall", txtPerInstallmentAmount_RepayTab1.Text);
            objMethodParameters.Add("Breakup", txtBreakup_RepayTab1.Text);
            objMethodParameters.Add("FromInstall", txtFromInstallment_RepayTab1.Text);
            objMethodParameters.Add("ToInstall", txtToInstallment_RepayTab1.Text);
            objMethodParameters.Add("FromDate", txtfromdate_RepayTab1.Text);
            //objMethodParameters.Add("Frequency", ddl_Frequency.SelectedValue);
            objMethodParameters.Add("Frequency", hdnRentfrequency.Value);
            objMethodParameters.Add("TenureType", ddlTenureType.SelectedItem.Text);
            objMethodParameters.Add("Tenure", txtTenure.Text);
            //objMethodParameters.Add("DocumentDate", txtAccountDate.Text);
            objMethodParameters.Add("DocumentDate", TxtFirstInstallDue.Text);
            objMethodParameters.Add("Time_Value", hdnTimeValue.Value);
            string strErrorMessage = "";
            if (ddlLOB.SelectedItem.Text.Contains("TL"))
            {
                //objMethodParameters.Add("repayMode_id", ddl_Repayment_Mode.SelectedValue);
                objMethodParameters.Add("repayMode_id", "1");//Default EMI
                //objMethodParameters.Add("Levy", ddl_Interest_Levy.SelectedItem.Value);

                //Checking if other than normal payment , start date should be last payment date.
                //if (ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
                if (ddlReturnPattern.SelectedValue == "6")
                {
                    System.Data.DataTable dtAcctype = ((System.Data.DataTable)ViewState["PaymentRules"]);
                    dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
                    string strAcctType = dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper();

                    if (strAcctType == "PROJECT FINANCE" || strAcctType == "DEFERRED PAYMENT" || strAcctType == "DEFERRED STRUCTURED")
                    {
                        DtCashFlowOut = (System.Data.DataTable)ViewState["DtCashFlowOut"];
                        if (DtCashFlowOut.Rows.Count > 0)
                        {
                            DataRow drOutFlw = DtCashFlowOut.Select("CashFlow_Flag_ID=41").Last();
                            if (drOutFlw != null)
                            {
                                objMethodParameters.Remove("DocumentDate");
                                objMethodParameters.Add("DocumentDate", drOutFlw["Date"].ToString());
                                dtStartdate = Utility.StringToDate(drOutFlw["Date"].ToString());
                            }
                        }

                    }
                }
                objRepaymentStructure.FunPubAddRepaymentforTL(out dtNextFromdate, out strErrorMessage, out DtRepayGrid, DtRepayGrid, objMethodParameters);
            }
            else
            {
                objRepaymentStructure.FunPubAddRepayment(out dtNextFromdate, out strErrorMessage, out DtRepayGrid, DtRepayGrid, objMethodParameters);
            }

            if (strErrorMessage != "")
            {
                Utility.FunShowAlertMsg(this, strErrorMessage);
                if (DtRepayGrid != null)
                    DtRepayGrid.Rows.Remove(DtRepayGrid.Rows[DtRepayGrid.Rows.Count - 1]);
                return;
            }
            if (strIds[4] == "23")
            {
                decimal decIRRActualAmount = 0;
                decimal decTotalAmount = 0;

                decimal DecRoundOff;
                if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                    DecRoundOff = Convert.ToDecimal(ViewState["hdnRoundOff"]);
                else
                    DecRoundOff = 2;

                //if (!objRepaymentStructure.FunPubValidateTotalAmount(DtRepayGrid, txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue, txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "1", DecRoundOff))
                if (!objRepaymentStructure.FunPubValidateTotalAmount(DtRepayGrid, txtFinanceAmount.Text, "", ddlReturnPattern.SelectedValue, "", ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "1", DecRoundOff))
                {
                    Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                    DtRepayGrid.Rows.Remove(DtRepayGrid.Rows[DtRepayGrid.Rows.Count - 1]);
                    return;
                }

            }


            gvRepaymentDetails.DataSource = DtRepayGrid;
            gvRepaymentDetails.DataBind();

            TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
            txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
            TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //if (ddl_Rate_Type.SelectedItem.Text == "Floating" && string.IsNullOrEmpty(txtFBDate.Text))
            //{
            //    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
            //    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
            //}
            //else
            //{
            //    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
            //    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
            //}
            ViewState["DtRepayGrid"] = DtRepayGrid;

            if (ViewState["DtRepayGrid_TL"] != null)
            {
                System.Data.DataTable DtRepayGrid_TL = (System.Data.DataTable)ViewState["DtRepayGrid_TL"];
                DataRow drow = DtRepayGrid_TL.NewRow();

                for (int i = 0; i <= DtRepayGrid_TL.Columns.Count - 1; i++)
                {
                    drow[i] = DtRepayGrid.Rows[DtRepayGrid.Rows.Count - 1][i].ToString();
                }

                DtRepayGrid_TL.Rows.Add(drow);
                ViewState["DtRepayGrid_TL"] = DtRepayGrid_TL;
            }
            FunPriGenerateNewRepaymentRow();
            FunPriIRRReset();
            FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Add Repayment";
            cv_TabMainPage.IsValid = false;
        }
    }

    private bool FunPriValidateTotalAmount(out decimal decActualAmount, out decimal decTotalAmount, string strOption)
    {
        try
        {
            if (strOption != "3")
            {
                //decTotalAmount = FunPriGetAmountFinanced() + Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(ddlTenureType.SelectedItem.Text, FunPriGetAmountFinanced(), Convert.ToDecimal(txtRate.Text), Convert.ToInt32(txtTenure.Text)), 0);
                decTotalAmount = FunPriGetAmountFinanced() + Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(ddlTenureType.SelectedItem.Text, FunPriGetAmountFinanced(), Convert.ToDecimal(0), Convert.ToInt32(txtTenure.Text)), 0);
            }
            else
            {
                decTotalAmount = FunPriGetAmountFinanced();
            }
            decActualAmount = 0;
            if (((System.Data.DataTable)ViewState["DtRepayGrid"]).Rows.Count <= 0)
            {
                if (strConsNumber == null && strSplitNum == null)//For Consolidation & Split
                {
                    cv_TabMainPage.ErrorMessage = "Correct the following validation(s): <br/><br/>  Add atleast one row Repayment details";
                }
                else
                {
                    cv_TabMainPage.ErrorMessage = "Add atleast one row Repayment details";
                }

                cv_TabMainPage.IsValid = false;
                return false;
            }
            DtRepayGrid = (System.Data.DataTable)ViewState["DtRepayGrid"];
            foreach (DataRow drRepyrow in DtRepayGrid.Rows)
            {
                decActualAmount += (Convert.ToDecimal(drRepyrow["TotalPeriodInstall"].ToString()));
            }
            if (strOption == "1")
            {
                if (decActualAmount > decTotalAmount)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (strOption == "2")
            {
                if (decActualAmount == decTotalAmount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (strOption == "3")
            {
                if (decActualAmount >= decTotalAmount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Total Amount");
        }

    }

    private bool FunPriValidateTenurePeriod(DateTime dtStartDate, DateTime dtEndDate)
    {
        DateTime dateInterval = new DateTime();

        switch (ddlTenureType.SelectedItem.Text.ToLower())
        {
            case "months":
                dateInterval = dtStartDate.AddMonths(Convert.ToInt32(txtTenure.Text));
                break;
            case "weeks":

                int intAddweeks = Convert.ToInt32(txtTenure.Text) * 7;
                dateInterval = dtStartDate.AddDays(intAddweeks);
                break;
            case "days":
                dateInterval = dtStartDate.AddDays(Convert.ToInt32(txtTenure.Text));
                break;
        }
        if (dtEndDate > dateInterval)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool FunPriValidateTenurePeriod(int intActualTenurePeriod)
    {
        if (intActualTenurePeriod == Convert.ToInt32(txtTenure.Text))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void FunPriCalculateSummary(System.Data.DataTable objDataTable, string strGroupByField, string strSumField)
    {
        try
        {

            DataTable DtRepayGridp = objDataTable.Copy();
            DataRow dr1;
            dr1 = DtRepayGridp.NewRow();
            dr1["slno"] = "1";
            dr1["CashFlow"] = "Tax";
            dr1["TotalPeriodInstall"] = ViewState["TAX_Total"];
            DtRepayGridp.Rows.Add(dr1);

            if (Convert.ToDecimal(ViewState["ServiceTax_Total"]) > 0)
            {
                dr1 = DtRepayGridp.NewRow();
                dr1["slno"] = "4";
                dr1["CashFlow"] = "ServiceTax";
                dr1["TotalPeriodInstall"] = ViewState["ServiceTax_Total"];
                DtRepayGridp.Rows.Add(dr1);
            }


            foreach (DataRow dr in DtRepayGridp.Rows)
            {
                decimal intinstall = 0;

                //dr["TotalPeriodInstall"] = Convert.ToDecimal(Convert.ToDecimal(dr["TotalPeriodInstall"]).ToString(Funsetsuffix()));
                //dr["TotalPeriodInstall"] = Convert.ToDecimal(Math.Floor(Convert.ToDecimal(dr["TotalPeriodInstall"])).ToString(Funsetsuffix()));
                if (!string.IsNullOrEmpty(dr["PerInstall"].ToString()))
                {
                    intinstall = Convert.ToDecimal(dr["ToInstall"]) - Convert.ToDecimal(dr["FromInstall"]) + 1;
                    dr["TotalPeriodInstall"] = Convert.ToDecimal((Math.Round(Convert.ToDecimal(dr["PerInstall"])) * intinstall).ToString(Funsetsuffix()));
                }
                else
                    dr["TotalPeriodInstall"] = Convert.ToDecimal(Math.Round(Convert.ToDecimal(dr["TotalPeriodInstall"])).ToString(Funsetsuffix()));
            }
            DataView dv = DtRepayGridp.DefaultView;
            dv.Sort = "slno asc";
            DtRepayGridp = dv.ToTable();

            System.Data.DataTable dtSummaryDetails = Utility.FunPriCalculateSumAmount(DtRepayGridp, strGroupByField, strSumField);
            gvRepaymentSummary.DataSource = dtSummaryDetails;
            gvRepaymentSummary.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Calculate Repayment Summary");
        }

    }

    protected void gvRepaymentDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtRepayGrid = (System.Data.DataTable)ViewState["DtRepayGrid"];
            if (ViewState["DtRepayGrid_TL"] != null)
            {
                System.Data.DataTable DtRepayGrid_TL = (System.Data.DataTable)ViewState["DtRepayGrid_TL"];
                if (DtRepayGrid_TL.Rows.Count > 0)
                {
                    DtRepayGrid_TL.Rows.RemoveAt(DtRepayGrid_TL.Rows.Count - 1);
                }
            }
            if (DtRepayGrid.Rows.Count > 0)
            {
                DtRepayGrid.Rows.RemoveAt(e.RowIndex);

                if (DtRepayGrid.Rows.Count == 0)
                {
                    FunPriBindRepaymentDLL(_Add);
                    gvRepaymentSummary.DataSource = null;
                    gvRepaymentSummary.DataBind();
                }
                else
                {
                    gvRepaymentDetails.DataSource = DtRepayGrid;
                    gvRepaymentDetails.DataBind();
                    FunPriGenerateNewRepaymentRow();
                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
                    TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
                    Label lblTODate_ReapyTab_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblTODate_ReapyTab");
                    DateTime dtTodate = Utility.StringToDate(lblTODate_ReapyTab_Upd.Text);
                    //DateTime dtNextFromdate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedItem.Text, dtTodate);
                    DateTime dtNextFromdate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate("monthly", dtTodate);//Timebeing

                    txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    FunPriCalculateSummary(DtRepayGrid, "cashflow", "TotalPeriodInstall");
                    //if (ddl_Repayment_Mode.SelectedValue != "2")
                    //{
                    Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
                    if (lblCashFlowId.Text != "23" && lblCashFlowId.Text != "105")
                    {
                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                    }
                    //}
                    //else
                    //{
                    //    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                    //}
                }
            }
            FunPriIRRReset();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Remove Repayment";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void gvRepaymentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                _SlNo += 1;
                e.Row.Cells[0].Text = _SlNo.ToString();
                if (Request.QueryString["qsMode"] != null)
                {
                    if (Request.QueryString["qsMode"].ToString() == "Q")
                    {
                        AjaxControlToolkit.CalendarExtender calext_FromDate = e.Row.FindControl("calext_FromDate") as AjaxControlToolkit.CalendarExtender;
                        calext_FromDate.Enabled = false;
                    }
                }


                //TO show round off value in grid note: for front end purpose
                Label lblPerInstallmentAmount_RepayTab = e.Row.FindControl("lblPerInstallmentAmount_RepayTab") as Label;
                lblPerInstallmentAmount_RepayTab.Text = Math.Round(Convert.ToDecimal(lblPerInstallmentAmount_RepayTab.Text)).ToString(Funsetsuffix());



            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void grvRepayStructure_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblInstallmentAmount = e.Row.FindControl("lblInstallmentAmount") as Label;
                Label lblTax = e.Row.FindControl("lblTax") as Label;
                Label lblAMF = e.Row.FindControl("lblAMF") as Label;
                Label lblServiceTax = e.Row.FindControl("lblServiceTax") as Label;
                Label lblInsurance = e.Row.FindControl("lblInsurance") as Label;
                Label lblOthers = e.Row.FindControl("lblOthers") as Label;
                if (!string.IsNullOrEmpty(lblInstallmentAmount.Text))
                    lblInstallmentAmount.Text = Math.Round(Convert.ToDecimal(lblInstallmentAmount.Text), MidpointRounding.AwayFromZero).ToString(Funsetsuffix());
                if (!string.IsNullOrEmpty(lblTax.Text))
                    lblTax.Text = Math.Round(Convert.ToDecimal(lblTax.Text), MidpointRounding.AwayFromZero).ToString(Funsetsuffix());


                if (!string.IsNullOrEmpty(lblAMF.Text))
                    if (Convert.ToDecimal(lblAMF.Text) > 0)
                        lblAMF.Text = Math.Round(Convert.ToDecimal(lblAMF.Text), MidpointRounding.AwayFromZero).ToString(Funsetsuffix());
                    else
                        lblAMF.Text = string.Empty;

                if (!string.IsNullOrEmpty(lblServiceTax.Text))
                    lblServiceTax.Text = Math.Round(Convert.ToDecimal(lblServiceTax.Text), MidpointRounding.AwayFromZero).ToString(Funsetsuffix());
                else
                    lblServiceTax.Text = string.Empty;
                if (!string.IsNullOrEmpty(lblInsurance.Text))
                    lblInsurance.Text = Math.Round(Convert.ToDecimal(lblInsurance.Text), MidpointRounding.AwayFromZero).ToString(Funsetsuffix());
                if (!string.IsNullOrEmpty(lblOthers.Text))
                    lblOthers.Text = Math.Round(Convert.ToDecimal(lblOthers.Text), MidpointRounding.AwayFromZero).ToString(Funsetsuffix());



            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected void gvRepaymentDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtToDate_RepayTab = e.Row.FindControl("txtToDate_RepayTab") as TextBox;
            txtToDate_RepayTab.Attributes.Add("readonly", "readonly");
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab = e.Row.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_ToDate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;

            TextBox txtfromdate_RepayTab = e.Row.FindControl("txtfromdate_RepayTab") as TextBox;
            txtfromdate_RepayTab.Attributes.Add("readonly", "readonly");
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab = e.Row.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_fromdate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            AjaxControlToolkit.CalendarExtender calext_FromDate = e.Row.FindControl("calext_FromDate") as AjaxControlToolkit.CalendarExtender;
            calext_FromDate.Format = ObjS3GSession.ProDateFormatRW;
        }
    }



    #endregion

    //#region Alert Tab

    //private void FunPriBindAlertDLL(string Mode)
    //{
    //    try
    //    {
    //        Dictionary<string, string> objParameters = new Dictionary<string, string>();
    //        objParameters.Add("@CompanyId", intCompanyId.ToString());
    //        System.Data.DataSet dsAlert = Utility.GetDataset("s3g_org_loadAlertLov", objParameters);

    //        DataRow[] dr = dsAlert.Tables[0].Select("ID in(141,219,220,221,222,223,224,225)");
    //        System.Data.DataTable dtAlert = dr.CopyToDataTable();
    //        ViewState["AlertDDL"] = dtAlert;
    //        ViewState["AlertUser"] = dsAlert;
    //        //End Here
    //        if (Mode == _Add)
    //        {

    //            System.Data.DataTable ObjDT = new System.Data.DataTable();
    //            ObjDT.Columns.Add("Type");
    //            ObjDT.Columns.Add("TypeID");
    //            ObjDT.Columns.Add("UserContact");
    //            ObjDT.Columns.Add("UserContactID");
    //            ObjDT.Columns.Add("EMail");
    //            ObjDT.Columns["Email"].DataType = typeof(Boolean);
    //            ObjDT.Columns.Add("SMS");
    //            ObjDT.Columns["SMS"].DataType = typeof(Boolean);
    //            DataRow dr_Alert = ObjDT.NewRow();
    //            dr_Alert["Type"] = "";
    //            dr_Alert["TypeID"] = "";
    //            dr_Alert["UserContact"] = "";
    //            dr_Alert["UserContactID"] = "";
    //            dr_Alert["EMail"] = "False";
    //            dr_Alert["SMS"] = "False";
    //            ObjDT.Rows.Add(dr_Alert);

    //            gvAlert.DataSource = ObjDT;
    //            gvAlert.DataBind();

    //            ObjDT.Rows.Clear();
    //            ViewState["DtAlertDetails"] = ObjDT;

    //            gvAlert.Rows[0].Cells.Clear();
    //            gvAlert.Rows[0].Visible = false;
    //            FunPriGenerateNewAlertRow();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }

    //}

    //private void FunPriGenerateNewAlertRow()
    //{
    //    try
    //    {
    //        DropDownList ObjddlType_AlertTab = gvAlert.FooterRow.FindControl("ddlType_AlertTab") as DropDownList;
    //        //Removed By Shibu 18-Sep-2013
    //        //  DropDownList ObjddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as DropDownList;
    //        UserControls_S3GAutoSuggest ddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as UserControls_S3GAutoSuggest;
    //        Utility.FillDLL(ObjddlType_AlertTab, ((System.Data.DataTable)ViewState["AlertDDL"]), true);
    //        //Utility.FillDLL(ObjddlContact_AlertTab, ((System.Data.DataSet)ViewState["AlertUser"]).Tables[1], true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}

    //protected void Alert_AddRow_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DtAlertDetails = (System.Data.DataTable)ViewState["DtAlertDetails"];

    //        DropDownList ddlAlert_Type = gvAlert.FooterRow.FindControl("ddlType_AlertTab") as DropDownList;
    //        //Removed By Shibu 18-Sep-2013
    //        // DropDownList ddlAlert_ContactList = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as DropDownList;
    //        UserControls_S3GAutoSuggest ddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as UserControls_S3GAutoSuggest;
    //        CheckBox ChkAlertEmail = gvAlert.FooterRow.FindControl("ChkEmail") as CheckBox;
    //        CheckBox ChkAlertSMS = gvAlert.FooterRow.FindControl("ChkSMS") as CheckBox;

    //        if (ChkAlertEmail.Checked || ChkAlertSMS.Checked)
    //        {

    //            //For Duplication
    //            if (DtAlertDetails.Rows.Count > 0)
    //            {
    //                DataRow[] drAlertDetails = null;
    //                drAlertDetails = DtAlertDetails.Select(" TypeId = " + ddlAlert_Type.SelectedValue + " and UserContactId=" + ddlContact_AlertTab.SelectedValue + "");
    //                if (drAlertDetails.Count() > 0)
    //                {
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Combination already exists');", true);
    //                    return;
    //                }
    //            }


    //            DataRow dr = DtAlertDetails.NewRow();

    //            dr["TypeId"] = ddlAlert_Type.SelectedValue;
    //            dr["Type"] = ddlAlert_Type.SelectedItem;
    //            dr["UserContactId"] = ddlContact_AlertTab.SelectedValue.ToString();
    //            dr["UserContact"] = ddlContact_AlertTab.SelectedText;
    //            dr["EMail"] = ChkAlertEmail.Checked;
    //            dr["SMS"] = ChkAlertSMS.Checked;

    //            DtAlertDetails.Rows.Add(dr);

    //            gvAlert.DataSource = DtAlertDetails;
    //            gvAlert.DataBind();
    //            //gvAlert0.DataSource = DtAlertDetails;
    //            //gvAlert0.DataBind();
    //            ViewState["DtAlertDetails"] = DtAlertDetails;
    //            FunPriGenerateNewAlertRow();
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Select Email or SMS');", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Add Alert";
    //        cv_TabMainPage.IsValid = false;
    //    }

    //}
    //protected void gvAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        FunPriBindAlertDetails(e);
    //    }
    //    catch (Exception ex)
    //    {
    //        cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}

    //private void FunPriBindAlertDetails(GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        CheckBox ChkAlertEmail = e.Row.FindControl("ChkEmail") as CheckBox;
    //        CheckBox ChkAlertSMS = e.Row.FindControl("ChkSMS") as CheckBox;
    //        ChkAlertEmail.Enabled = ChkAlertSMS.Enabled = false;
    //    }
    //}

    //protected void gvAlert_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        DtAlertDetails = (System.Data.DataTable)ViewState["DtAlertDetails"];
    //        if (DtAlertDetails.Rows.Count > 0)
    //        {
    //            DtAlertDetails.Rows.RemoveAt(e.RowIndex);

    //            if (DtAlertDetails.Rows.Count == 0)
    //            {
    //                FunPriBindAlertDLL(_Add);
    //            }
    //            else
    //            {
    //                gvAlert.DataSource = DtAlertDetails;
    //                gvAlert.DataBind();
    //                FunPriGenerateNewAlertRow();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Remove Alert";
    //        cv_TabMainPage.IsValid = false;

    //    }
    //}

    //#endregion

    //#region Follow Up Tab

    //private void FunPriBindFollowupDLL(string Mode)
    //{
    //    ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
    //    try
    //    {
    //        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
    //        System.Data.DataTable ObjDT = new System.Data.DataTable();
    //        if (Mode == _Add)
    //        {
    //            ObjStatus.Option = 35;
    //            ObjStatus.Param1 = intCompanyId.ToString();
    //            ViewState["UserListFolloup"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

    //            ObjStatus.Option = 47;
    //            ObjStatus.Param1 = null;
    //            ObjDT = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
    //            ObjDT.Columns.Add("FromUserId");
    //            ObjDT.Columns.Add("ToUserId");
    //        }
    //        if (Mode == _Edit)
    //        {
    //            ObjStatus.Option = 35;
    //            ObjStatus.Param1 = intCompanyId.ToString();
    //            ViewState["UserListFolloup"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

    //            if (((System.Data.DataTable)ViewState["DtFollowUp"]) != null)
    //                ObjDT = (System.Data.DataTable)ViewState["DtFollowUp"];

    //        }
    //        gvFollowUp.DataSource = ObjDT;
    //        gvFollowUp.DataBind();

    //        if (Mode == _Add)
    //        {
    //            ObjDT.Rows.Clear();
    //            ViewState["DtFollowUp"] = ObjDT;

    //            gvFollowUp.Rows[0].Cells.Clear();
    //            gvFollowUp.Rows[0].Visible = false;

    //        }
    //        FunPriGenerateNewFollowupRow();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //    finally
    //    {
    //        ObjCustomerService.Close();
    //    }

    //}

    //private void FunPriGenerateNewFollowupRow()
    //{
    //    try
    //    {
    //        if (gvFollowUp.Rows.Count > 0)
    //        {  //Removed By Shibu 18-Sep-2013
    //            //DropDownList ddlfrom_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as DropDownList;
    //            UserControls_S3GAutoSuggest ddlfrom_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as UserControls_S3GAutoSuggest;
    //            //DropDownList ddlTo_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as DropDownList;
    //            UserControls_S3GAutoSuggest ddlTo_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as UserControls_S3GAutoSuggest;
    //            //Utility.FillDLL(ddlfrom_GridFollowup, ((System.Data.DataTable)ViewState["UserListFolloup"]), true);
    //            //Utility.FillDLL(ddlTo_GridFollowup, ((System.Data.DataTable)ViewState["UserListFolloup"]), true);
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}

    //protected void FollowUp_AddRow_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DtFollowUp = (System.Data.DataTable)ViewState["DtFollowUp"];

    //        TextBox txttxtDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtDate_GridFollowup") as TextBox;
    //        //Removed By Shibu 18-Sep-2013
    //        //DropDownList ddlfrom_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as DropDownList;
    //        //DropDownList ddlTo_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as DropDownList;
    //        UserControls_S3GAutoSuggest ddlfrom_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as UserControls_S3GAutoSuggest;
    //        UserControls_S3GAutoSuggest ddlTo_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as UserControls_S3GAutoSuggest;
    //        TextBox txtAction_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtAction_GridFollowup") as TextBox;
    //        TextBox txtActionDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtActionDate_GridFollowup") as TextBox;
    //        TextBox txtCustomerResponse_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtCustomerResponse_GridFollowup") as TextBox;
    //        TextBox txtRemarks_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtRemarks_GridFollowup") as TextBox;
    //        if (Utility.CompareDates(txttxtDate_GridFollowup1.Text, txtActionDate_GridFollowup1.Text) != 1)
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('Action Date should be greater than Date in Followup');", true);
    //            return;
    //        }
    //        if (ddlfrom_GridFollowup1.SelectedValue == ddlTo_GridFollowup1.SelectedValue)
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('From and To UserName should be different');", true);
    //            return;
    //        }
    //        DataRow dr = DtFollowUp.NewRow();
    //        dr["Date"] = Utility.StringToDate(txttxtDate_GridFollowup1.Text);
    //        dr["From"] = ddlfrom_GridFollowup1.SelectedText;
    //        dr["FromUserId"] = ddlfrom_GridFollowup1.SelectedValue;
    //        dr["To"] = ddlTo_GridFollowup1.SelectedText;
    //        dr["ToUserId"] = ddlTo_GridFollowup1.SelectedValue;
    //        dr["Action"] = txtAction_GridFollowup1.Text;
    //        dr["ActionDate"] = Utility.StringToDate(txtActionDate_GridFollowup1.Text);
    //        dr["CustomerResponse"] = txtCustomerResponse_GridFollowup1.Text;
    //        dr["Remarks"] = txtRemarks_GridFollowup1.Text;

    //        DtFollowUp.Rows.Add(dr);

    //        gvFollowUp.DataSource = DtFollowUp;
    //        gvFollowUp.DataBind();

    //        ViewState["DtFollowUp"] = DtFollowUp;
    //        FunPriGenerateNewFollowupRow();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Add Followup";
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}

    //protected void gvFollowUp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        DtFollowUp = (System.Data.DataTable)ViewState["DtFollowUp"];
    //        if (DtFollowUp.Rows.Count > 0)
    //        {
    //            DtFollowUp.Rows.RemoveAt(e.RowIndex);

    //            if (DtFollowUp.Rows.Count == 0)
    //            {
    //                FunPriBindFollowupDLL(_Add);
    //            }
    //            else
    //            {
    //                gvFollowUp.DataSource = DtFollowUp;
    //                gvFollowUp.DataBind();
    //                FunPriGenerateNewFollowupRow();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Remove Followup";
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}

    //protected void gvFollowUp_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        TextBox txtDate_GridFollowup = e.Row.FindControl("txtDate_GridFollowup") as TextBox;
    //        txtDate_GridFollowup.Attributes.Add("readonly", "readonly");
    //        AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupDate = e.Row.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
    //        CalendarExtenderSD_FollowupDate.Format = ObjS3GSession.ProDateFormatRW;

    //        TextBox txtActionDate_GridFollowup = e.Row.FindControl("txtActionDate_GridFollowup") as TextBox;
    //        txtActionDate_GridFollowup.Attributes.Add("readonly", "readonly");
    //        AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupActionDate = e.Row.FindControl("CalendarExtenderSD_FollowupActionDate") as AjaxControlToolkit.CalendarExtender;
    //        CalendarExtenderSD_FollowupActionDate.Format = ObjS3GSession.ProDateFormatRW;
    //    }
    //}
    //#endregion

    #region Document Details


    private void FunGetConstitutionCodeDetails(string intConstitutionID)
    {
        Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@ID", intConstitutionID);
        if (intAccountCreationId == 0)
        {
            Procparam.Add("@Option", "12");
        }
        grvConsDocuments.BindGridView(SPNames.S3G_ORG_GetPricing_List, Procparam);
    }


    protected void grvConsDocs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            e.Row.Cells[0].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtVal = (TextBox)e.Row.FindControl("txtValues");
                TextBox txtRemark = (TextBox)e.Row.FindControl("txtRemark");
                CheckBox chkCollected = (CheckBox)e.Row.FindControl("chkCollected");
                CheckBox chkScanned = (CheckBox)e.Row.FindControl("chkScanned");
                LinkButton lnkScannedReference = (LinkButton)e.Row.FindControl("lnkScannedReference");
                //CheckBox ObjchkIsNeedImageCopy = (CheckBox)e.Row.FindControl("chkIsNeedImageCopy");
                chkScanned.Enabled = !chkScanned.Checked; //if yes then disabled
                chkCollected.Enabled = !chkCollected.Checked;
                lnkScannedReference.Enabled = chkScanned.Checked; // if yes then enabled
                txtRemark.Attributes.Add("onkeypress", "wraptext(" + txtRemark.ClientID + ",30);");
                //ObjchkIsMandatory.Enabled = false;
                //ObjchkIsNeedImageCopy.Enabled = false;

                if (txtVal != null)
                {
                    txtVal.Enabled = DisableValueField(e.Row.Cells[1].Text);
                }
            }
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = ex.Message;
            cv_TabMainPage.IsValid = false;
        }
    }

    #endregion

    #region Guarantor / Invoice / Collateral Tab

    private void FunPriBindGuarantorDLL(string Mode)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyId.ToString());
            System.Data.DataSet dsGuarantor = Utility.GetDataset("s3g_org_loadGuarantorLov", objParameters);
            ViewState["GuarantorDDL"] = dsGuarantor;

            if (Mode == _Add)
            {
                ObjStatus.Option = 53;
                ObjStatus.Param1 = null;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                gvGuarantor.DataSource = DtRepayGrid;
                gvGuarantor.DataBind();

                DtRepayGrid.Rows.Clear();
                ViewState["dtGuarantorGrid"] = DtRepayGrid;

                gvGuarantor.Rows[0].Cells.Clear();
                gvGuarantor.Rows[0].Visible = false;

                FunPriGenerateNewGuarantorRow();



            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriGenerateNewGuarantorRow()
    {
        try
        {
            //DropDownList ddlGuarantortype_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
            //DropDownList ddlChargesequence_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlChargesequence_GuarantorTab") as DropDownList;
            //UserControls_LOBMasterView ucCustomerLov = gvGuarantor.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
            //ucCustomerLov.strControlID = ucCustomerLov.ClientID;
            //Utility.FillDLL(ddlGuarantortype_GuarantorTab1, ((System.Data.DataSet)ViewState["GuarantorDDL"]).Tables[0], true);

            //Utility.FillDLL(ddlChargesequence_GuarantorTab1, ((System.Data.DataSet)ViewState["GuarantorDDL"]).Tables[1], true);



        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlGuarantortype_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlGuarantortype_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
        UserControls_LOBMasterView ucCustomerLov = gvGuarantor.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
        HiddenField hdnCustId = ucCustomerLov.FindControl("hdnID") as HiddenField;
        TextBox txtName = ucCustomerLov.FindControl("txtName") as TextBox;
        if (ddlGuarantortype_GuarantorTab1.SelectedIndex > 0)
        {
            if (ddlGuarantortype_GuarantorTab1.SelectedItem.Text.StartsWith("G") && ddlGuarantortype_GuarantorTab1.SelectedItem.Text.EndsWith("1"))
            {
                ucCustomerLov.strLOV_Code = "GCMD";
            }
            else if (ddlGuarantortype_GuarantorTab1.SelectedItem.Text.Contains("G") && ddlGuarantortype_GuarantorTab1.SelectedItem.Text.Contains("2"))
            {
                ucCustomerLov.strLOV_Code = "PCMD";
            }
            else
            {
                //For Co-Applicant
                ucCustomerLov.strLOV_Code = "COAP";
            }
            ViewState["Type"] = ucCustomerLov.strLOV_Code;
        }

        txtName.Text = string.Empty;

        //ucCustomerLov.strControlID = ucCustomerLov.ClientID;
        //Page_Load(null, null);
    }


    protected void Guarantor_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtRepayGrid = (System.Data.DataTable)ViewState["dtGuarantorGrid"];

            DropDownList ddlGuarantortype_GuarantorTab = gvGuarantor.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
            DropDownList ddlChargesequence_GuarantorTab = gvGuarantor.FooterRow.FindControl("ddlChargesequence_GuarantorTab") as DropDownList;
            TextBox txtGuaranteeamount_GuarantorTab = gvGuarantor.FooterRow.FindControl("txtGuaranteeamount_GuarantorTab_Footer") as TextBox;
            TextBox txtguarantor = gvGuarantor.FooterRow.FindControl("txtguarantor") as TextBox;

            if (txtguarantor.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Guarantor Details", "alert('Select the Guarantor');", true);
                return;
            }


            DataRow dr = DtRepayGrid.NewRow();

            dr["Guarantortype"] = "0";
            dr["Guarantor"] = "0";
            dr["Code"] = txtguarantor.Text;
            dr["Name"] = txtguarantor.Text;
            dr["Amount"] = txtGuaranteeamount_GuarantorTab.Text;
            dr["Charge"] = "0";
            dr["ChargeSequence"] = "0";
            dr["View"] = "View";


            DtRepayGrid.Rows.Add(dr);

            gvGuarantor.DataSource = DtRepayGrid;
            gvGuarantor.DataBind();

            ViewState["dtGuarantorGrid"] = DtRepayGrid;
            FunPriGenerateNewGuarantorRow();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Add Guarantor";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void gvGuarantor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtRepayGrid = (System.Data.DataTable)ViewState["dtGuarantorGrid"];
            if (DtRepayGrid.Rows.Count > 0)
            {
                DtRepayGrid.Rows.RemoveAt(e.RowIndex);

                if (DtRepayGrid.Rows.Count == 0)
                {
                    FunPriBindGuarantorDLL(_Add);
                }
                else
                {
                    gvGuarantor.DataSource = DtRepayGrid;
                    gvGuarantor.DataBind();
                    FunPriGenerateNewGuarantorRow();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Remove Guarantor";
            cv_TabMainPage.IsValid = false;
        }
    }



    protected void gvGuarantor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblGuaranteeID = e.Row.FindControl("lblGuaranteeID") as Label;
                LinkButton lbtnViewCustomer = e.Row.FindControl("lbtnViewCustomer") as LinkButton;
                if (lbtnViewCustomer != null && lblGuaranteeID != null)
                {
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblGuaranteeID.Text, false, 0);
                    lbtnViewCustomer.Attributes.Add("onclick", "window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&qsCustomerId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;");
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtGuaranteeamount_GuarantorTab_Footer = e.Row.FindControl("txtGuaranteeamount_GuarantorTab_Footer") as TextBox;
                txtGuaranteeamount_GuarantorTab_Footer.CheckGPSLength(true, "Guarantee Amount");
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Load Guarantor Details";
            cv_TabMainPage.IsValid = false;
        }
    }



    #endregion



    //#region Moratorium Tab

    //private void FunPriBindMoratoriumDLL(string Mode)
    //{
    //    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
    //    try
    //    {
    //        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
    //        ObjStatus.Option = 1;
    //        ObjStatus.Param1 = "MORATORIUM_TYPE";
    //        ViewState["MoratoriumType"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
    //        if (Mode == _Add)
    //        {
    //            ObjStatus.Option = 54;
    //            ObjStatus.Param1 = null;
    //            DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

    //            gvMoratorium.DataSource = DtRepayGrid;
    //            gvMoratorium.DataBind();

    //            DtRepayGrid.Rows.Clear();
    //            ViewState["dtMoratorium"] = DtRepayGrid;

    //            gvMoratorium.Rows[0].Cells.Clear();
    //            gvMoratorium.Rows[0].Visible = false;

    //            TextBox txtFromdate_MoratoriumTab = gvMoratorium.FooterRow.FindControl("txtFromdate_MoratoriumTab") as TextBox;
    //            TextBox txtTodate_MoratoriumTab = gvMoratorium.FooterRow.FindControl("txtTodate_MoratoriumTab") as TextBox;
    //            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_MoratoriumTab1 = gvMoratorium.FooterRow.FindControl("CalendarExtenderSD_ToDate_MoratoriumTab") as AjaxControlToolkit.CalendarExtender;
    //            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FromDate_MoratoriumTab1 = gvMoratorium.FooterRow.FindControl("CalendarExtenderSD_FromDate_MoratoriumTab") as AjaxControlToolkit.CalendarExtender;

    //            txtFromdate_MoratoriumTab.Attributes.Add("readonly", "readonly");
    //            txtTodate_MoratoriumTab.Attributes.Add("readonly", "readonly");
    //            CalendarExtenderSD_ToDate_MoratoriumTab1.Format = strDateFormat;
    //            CalendarExtenderSD_FromDate_MoratoriumTab1.Format = strDateFormat;

    //            FunPriGenerateNewMorotoriumRow();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //    finally
    //    {
    //        ObjCustomerService.Close();
    //    }
    //}

    //private void FunPriGenerateNewMorotoriumRow()
    //{
    //    try
    //    {
    //        AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_MoratoriumTab1 = gvMoratorium.FooterRow.FindControl("CalendarExtenderSD_ToDate_MoratoriumTab") as AjaxControlToolkit.CalendarExtender;
    //        AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FromDate_MoratoriumTab1 = gvMoratorium.FooterRow.FindControl("CalendarExtenderSD_FromDate_MoratoriumTab") as AjaxControlToolkit.CalendarExtender;
    //        CalendarExtenderSD_ToDate_MoratoriumTab1.Format = strDateFormat;
    //        CalendarExtenderSD_FromDate_MoratoriumTab1.Format = strDateFormat;

    //        DropDownList ddlMoratoriumtype_MoratoriumTab = gvMoratorium.FooterRow.FindControl("ddlMoratoriumtype_MoratoriumTab") as DropDownList;

    //        Utility.FillDLL(ddlMoratoriumtype_MoratoriumTab, ((System.Data.DataTable)ViewState["MoratoriumType"]), true);


    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}

    //private void SetSelectItem_DLL(DropDownList ObjDrop, string str)
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(str))
    //        {
    //            ObjDrop.SelectedValue = str;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //protected void Moratorium_AddRow_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DtRepayGrid = (System.Data.DataTable)ViewState["dtMoratorium"];

    //        DropDownList ddlMoratoriumtype_MoratoriumTab = gvMoratorium.FooterRow.FindControl("ddlMoratoriumtype_MoratoriumTab") as DropDownList;
    //        TextBox txtFromdate_MoratoriumTab = gvMoratorium.FooterRow.FindControl("txtFromdate_MoratoriumTab") as TextBox;
    //        TextBox txtTodate_MoratoriumTab = gvMoratorium.FooterRow.FindControl("txtTodate_MoratoriumTab") as TextBox;

    //        DataRow dr = DtRepayGrid.NewRow();

    //        dr["MoratoriumtypeId"] = ddlMoratoriumtype_MoratoriumTab.SelectedValue;
    //        dr["Moratoriumtype"] = ddlMoratoriumtype_MoratoriumTab.SelectedItem.Text;
    //        dr["Fromdate"] = Utility.StringToDate(txtFromdate_MoratoriumTab.Text);
    //        dr["Todate"] = Utility.StringToDate(txtTodate_MoratoriumTab.Text);
    //        dr["Noofdays"] = (Utility.StringToDate(txtTodate_MoratoriumTab.Text) - Utility.StringToDate(txtFromdate_MoratoriumTab.Text)).TotalDays;

    //        if (Utility.CompareDates(txtFromdate_MoratoriumTab.Text, txtTodate_MoratoriumTab.Text) != 1)
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Moratorium details", "alert('To Date should be greater than from date in Moratorium');", true);
    //            return;
    //        }

    //        DtRepayGrid.Rows.Add(dr);

    //        gvMoratorium.DataSource = DtRepayGrid;
    //        gvMoratorium.DataBind();

    //        ViewState["dtMoratorium"] = DtRepayGrid;
    //        FunPriGenerateNewMorotoriumRow();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Add Moratorium";
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}

    //protected void gvMoratorium_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        DtRepayGrid = (System.Data.DataTable)ViewState["dtMoratorium"];
    //        if (DtRepayGrid.Rows.Count > 0)
    //        {
    //            DtRepayGrid.Rows.RemoveAt(e.RowIndex);

    //            if (DtRepayGrid.Rows.Count == 0)
    //            {
    //                FunPriBindMoratoriumDLL(_Add);
    //            }
    //            else
    //            {
    //                gvMoratorium.DataSource = DtRepayGrid;
    //                gvMoratorium.DataBind();
    //                FunPriGenerateNewMorotoriumRow();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Remove Moratorium";
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}

    //private void Load_Moratorium(string Appid)
    //{
    //    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

    //    try
    //    {
    //        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
    //        ObjStatus.Option = 73;
    //        ObjStatus.Param1 = Appid;
    //        DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
    //        gvMoratorium.DataSource = DtRepayGrid;
    //        gvMoratorium.DataBind();
    //        ViewState["dtMoratorium"] = DtRepayGrid;
    //        FunPriGenerateNewMorotoriumRow();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //    finally
    //    {
    //        ObjCustomerService.Close();
    //    }
    //}

    //#endregion
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriClearTempInvoiceDtl();        //Added on 06Jul2015
            FunPubClear(false);
            /* Initialized For Security Deposit CashFlow on Dec 23,2015 */
            Session["strSecuDepstDate"] = null;
            /* Initialized For Security Deposit CashFlow on Dec 23,2015 */
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunPubClear(bool isCopyRS)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {
            Session["ApplicationAssetDetails"] = null;
            ClearForms();
            lblTotalOutFlowAmount.Text = "0";
            //ddlApplicationReferenceNo.Items.Clear();
            ddlApplicationReferenceNo.Clear();
            //txtApplicationDate.Text =
            txtPrimeAccountNo.Text =
                //txtSubAccountNo.Text = 
            txtStatus.Text = txtMRANo.Text = txtMRADate.Text =
            txtProductCode.Text = txtCustomerCode.Text = txtCustomerName.Text =
            txtConstitutionCode.Text = string.Empty;
            //ddlPaymentRuleList.SelectedIndex = 0;
            gvRepaymentDetails.DataSource = null;
            gvRepaymentDetails.DataBind();
            //txtAccount_Followup.Text = txtRepaymentTime.Text = string.Empty;
            // SalePerson Removed By Shibu 17-Sep-2013
            // ddlSalePersonCodeList.SelectedIndex = 0;
            //ddlSalePersonCodeList.Clear();
            //ddlSalePersonCodeList.SelectedValue = "0";
            //ddlAccountManager1.SelectedValue = ddlAccountManager2.SelectedValue = ddlRegionalManager.SelectedValue = "0";
            ddlAccountManager1.Clear(); ddlAccountManager2.Clear(); ddlRegionalManager.Clear();
            ddlStatus.SelectedIndex = 0;
            FillDLL_MainTab();

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
            //Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
            //ObjCustomerService.Close();
            //ddlTenureType.SelectedIndex = 0;
            gvRepaymentSummary.DataSource = null;
            gvRepaymentSummary.DataBind();
            ViewState["RepaymentStructure"] = null;
            grvRepayStructure.DataSource = null;
            grvRepayStructure.DataBind();
            lblTotalAmount.Text = "Total Amount Repayable : ";
            lblFrequency_Display.Text = "Tenure &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : ";
            lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : ";
            ddlRepaymentMode.SelectedIndex = 0;
            HiddenField hdnCustomerId1 = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            hdnCustomerId1.Value =
            hdnCustomerId.Value = null;

            txtCustomerCode.Text = "";
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Text = "";
            tcAccountCreation.ActiveTabIndex = 0;
            ddlDeliveryState.SelectedIndex = ddlBillingState.SelectedIndex = 0;
            chkPT.Checked =
            chkET.Checked =
            chkRC.Checked =
            chkLBT.Checked =
            chkSEZ.Checked =
            chkSEZA1.Checked =
            chkcstwith.Checked =
            chkApplicable.Checked = /*Added by vinodha m for the call id 3663*/
            chkApplicableSec.Checked =
            chkCSTDeal.Checked =
            chkIsSecondary.Checked =
            chkbxGSTRC.Checked =
            false;

            if (!isCopyRS)
            {
                //Removed By Shibu 17-Sep-2013
                //ddlBranchList.Items.Clear();
                ddlBranchList.Clear();
                //By Siva.K on 09JUN2015 Clear the Rs Details
                HiddenField hdnRsNo = (HiddenField)UcCtlRsno.FindControl("hdnID");
                hdnRsNo.Value = null;
                ViewState["Customer_Id"] = null;
                TextBox txtRsNo = (TextBox)UcCtlRsno.FindControl("txtName");
                txtRsNo.Text = "";
                System.Web.HttpContext.Current.Session["LocationAutoSuggestValue"] = null;
                System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = null;

            }
            Session["intAccountCreationId"] = null;
            Session["strSplitNum"] = null;
            Session["strConsNumber"] = null;
            //grvSummaryInvoices.DataSource = null;
            //grvSummaryInvoices.DataBind();
            //chkDORequired.Checked = false;
            //txtAdvanceInstallments.Text = "";
            ////txtFBDate.Text = "";
            //txtLastODICalcDate.Text = "";
            //txtComState.SelectedValue = txtComCity.SelectedValue = txtComCountry.SelectedValue = "0";
            //txtSLAUserName.Text = txtSLACustomerCode.Text =
            //txtComAddress1.Text = txtCOmAddress2.Text =
            //txtComEMail.Text = txtComMobile.Text =
            //txtComWebsite.Text =
            //txtComPhone.Text = txtComPincode.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to Clear the Details";
            cv_TabMainPage.IsValid = false;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    #region Save Methods

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {

            //if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            //{
            //    if (ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            //    {
            //        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            //        if (((System.Data.DataTable)ViewState["DtCashFlow"]).Rows.Count > 0)
            //        {
            //            decimal decUMFC = (decimal)((System.Data.DataTable)ViewState["DtCashFlow"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 34");
            //        }

            //        else
            //        {
            //            cv_TabMainPage.ErrorMessage = "Recalculate IRR ";
            //            cv_TabMainPage.IsValid = false;
            //            return;
            //        }
            //        //if (decUMFC != FunPriGetInterestAmount())
            //        //{
            //        //    cv_TabMainPage.ErrorMessage = "Unmatured Finance Charges (UMFC) should be equal to Interest";
            //        //    cv_TabMainPage.IsValid = false;

            //        //    return;
            //        //}
            //    }
            //if (((System.Data.DataTable)ViewState["DtCashFlowOut"]).Rows.Count == 0)
            //{
            //    ScriptManagerAlert("Cash Outflow details", "Add atleast one Out flow details");
            //    tcAccountCreation.ActiveTabIndex = 1;
            //    return;
            //}
            //if (FunPriIsDeferredPayment())
            //{
            //    return;
            //}
            //}
            if (ddlRepaymentMode.SelectedItem.Text.ToUpper() == "ECS")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@CompanyId", intCompanyId.ToString());
                //Procparam.Add("@CustomerId", hdnCustomerId.Value);
                Procparam.Add("@CustomerId", ViewState["Customer_Id"].ToString());
                string strCount = Utility.GetTableScalarValue("S3G_LOANAD_GETDEFAULTCOUNT", Procparam);
                if (strCount == "")
                {
                    Utility.FunShowAlertMsg(this, "Define the Bank Details in Customer Master for the selected Customer");
                    return;
                }
                if (strCount == "0")
                {
                    Utility.FunShowAlertMsg(this, "For ECS,Define a Default Rental Schedule in Customer Master for the selected Customer");
                    return;
                }
            }
            /*
                        if (tcAccountCreation.Tabs[7].Visible)
                        {
                            if (string.IsNullOrEmpty(txtSLACustomerCode.Text.Trim()))
                            {
                                cv_TabMainPage.ErrorMessage = "Enter the User Internal Code Reference";
                                cv_TabMainPage.IsValid = false;
                                tcAccountCreation.ActiveTabIndex = 7;
                                return;
                            }
                            if (string.IsNullOrEmpty(txtSLAUserName.Text.Trim()))
                            {
                                cv_TabMainPage.ErrorMessage = "Enter the User Name";
                                cv_TabMainPage.IsValid = false;
                                tcAccountCreation.ActiveTabIndex = 7;
                                return;
                            }
                            if (string.IsNullOrEmpty(txtComAddress1.Text.Trim()))
                            {
                                cv_TabMainPage.ErrorMessage = "Enter the Address1";
                                cv_TabMainPage.IsValid = false;
                                tcAccountCreation.ActiveTabIndex = 7;
                                return;
                            }
                            if (string.IsNullOrEmpty(txtComCity.SelectedItem.Text.Trim()))
                            {
                                cv_TabMainPage.ErrorMessage = "Enter the City";
                                cv_TabMainPage.IsValid = false;
                                tcAccountCreation.ActiveTabIndex = 7;
                                return;
                            }
                            if (string.IsNullOrEmpty(txtComState.SelectedItem.Text.Trim()))
                            {
                                cv_TabMainPage.ErrorMessage = "Enter the State";
                                cv_TabMainPage.IsValid = false;
                                tcAccountCreation.ActiveTabIndex = 7;
                                return;
                            }
                            if (string.IsNullOrEmpty(txtComCountry.SelectedItem.Text.Trim()))
                            {
                                cv_TabMainPage.ErrorMessage = "Enter the Country";
                                cv_TabMainPage.IsValid = false;
                                tcAccountCreation.ActiveTabIndex = 7;
                                return;
                            }
                            if (string.IsNullOrEmpty(txtComPincode.Text.Trim()))
                            {
                                cv_TabMainPage.ErrorMessage = "Enter the Pincode/Zipcode";
                                cv_TabMainPage.IsValid = false;
                                tcAccountCreation.ActiveTabIndex = 7;
                                return;
                            }
                            if (string.IsNullOrEmpty(txtComPhone.Text.Trim()))
                            {
                                cv_TabMainPage.ErrorMessage = "Enter the Phone";
                                cv_TabMainPage.IsValid = false;
                                return;
                            }
                        }*/

            //if (intAccountCreationId == 0)
            //{
            //    /******************************************************To avoid Finanace amount validation for Consolidation and split Begins here******************************************************************/
            //    //if (strConsNumber == null && strSplitNum == null)//Since Out flow amount and finance amount validation Not applicable for Split and consolidation.
            //    //{ Coomented this line on 25/06/2011 as Discussed removed this validation for consolidation and split
            //    DataRow[] drFinanAmtRow = ((System.Data.DataTable)ViewState["DtCashFlowOut"]).Select("CashFlow_Flag_ID = 41");
            //    if (drFinanAmtRow.Length > 0)
            //    {
            //        decimal decToatlFinanceAmt = (decimal)((System.Data.DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");

            //        if (Convert.ToDecimal(txtFinanceAmount.Text) != decToatlFinanceAmt)
            //        {
            //            cv_TabMainPage.ErrorMessage = "Total amount financed in Cashoutflow should be equal to amount financed";
            //            cv_TabMainPage.IsValid = false;
            //            return;
            //        }

            //    }
            //    if (strConsNumber == null && strSplitNum == null)//Since application Finance amount and finance amount validation Not applicable for Split and consolidation.
            //    {
            //        if (ddlStatus.SelectedValue == "11")
            //        {
            //            Dictionary<string, string> ProcParam = new Dictionary<string, string>();
            //            ProcParam.Add("@APPLICATIONPROCESSID", ddlApplicationReferenceNo.SelectedValue);
            //            System.Data.DataSet dsFinanceAmount = Utility.GetDataset("S3G_ORG_GETFINANCEAMOUNT", ProcParam);
            //            if (dsFinanceAmount.Tables[0].Rows.Count > 0)
            //            {
            //                if (dsFinanceAmount.Tables[0].Rows[0][0] != null)
            //                {
            //                    if (!string.IsNullOrEmpty(Convert.ToString(dsFinanceAmount.Tables[0].Rows[0][0])))
            //                    {
            //                        decimal dcmSLAFinanceAmount = Convert.ToDecimal(dsFinanceAmount.Tables[0].Rows[0]["SLAAMOUNT"]);
            //                        decimal dcmApplicationFinanceAmount = Convert.ToDecimal(dsFinanceAmount.Tables[0].Rows[0]["APPLICATIONAMOUNT"]);
            //                        if (dcmSLAFinanceAmount == dcmApplicationFinanceAmount)
            //                        {
            //                            ScriptManagerAlert("Account Creation", "Amount Financed in Application already over.Unable to Create Account");
            //                            return;
            //                        }
            //                        else
            //                        {
            //                            decimal dcmCurrentFinanceAmount = Convert.ToDecimal(txtFinanceAmount.Text);
            //                            dcmSLAFinanceAmount = dcmSLAFinanceAmount + dcmCurrentFinanceAmount;
            //                            if (dcmSLAFinanceAmount > dcmApplicationFinanceAmount)
            //                            {
            //                                ScriptManagerAlert("Account Creation", "Finance Amount should not be greater than Amount Financed in Application");
            //                                return;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else if (string.IsNullOrEmpty(txtStatus.Text) || ddlStatus.SelectedValue == "10" || ddlStatus.SelectedValue == "2")
            //        {
            //            Dictionary<string, string> ProcParam = new Dictionary<string, string>();
            //            ProcParam.Add("@APPLICATIONPROCESSID", ddlApplicationReferenceNo.SelectedValue);
            //            ProcParam.Add("@FINANCEAMOUNT", txtFinanceAmount.Text);
            //            System.Data.DataSet dsFinanceAmount = Utility.GetDataset("S3G_ORG_GETFINANCEAMOUNT", ProcParam);
            //            if (dsFinanceAmount != null)
            //            {
            //                if (dsFinanceAmount.Tables.Count > 0)
            //                {
            //                    if (dsFinanceAmount.Tables[0].Rows.Count > 0)
            //                    {
            //                        if (Convert.ToString(dsFinanceAmount.Tables[0].Rows[0][0]) == "EXCEED")
            //                        {
            //                            ScriptManagerAlert("Account Creation", "Finance Amount should not be greater than Amount Financed in Application");
            //                            return;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    /******************************************************To avoid Finanace amount validation for Consolidation and split ends here******************************************************************/
            //    string strLOBType = ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim();
            //    if (ViewState["dtInvoicesACAT"] != null)
            //    {
            //        dtInvoicesACAT = (System.Data.DataTable)ViewState["dtInvoicesACAT"];
            //    }

            //    if (dtInvoicesACAT.Rows.Count == 0)
            //    {
            //        cv_TabMainPage.ErrorMessage = "Select Invoices No for New Purchase";
            //        cv_TabMainPage.IsValid = false;
            //        return;
            //    }

            //    if (dtInvoicesACAT.Rows.Count > 0)
            //    {
            //        decimal dcmFinanceAmount = Convert.ToDecimal(txtFinanceAmount.Text);
            //        decimal dcmMarginAmount = 0;
            //        if (!string.IsNullOrEmpty(txtMarginMoneyPer_Cashflow.Text))
            //        {
            //            dcmMarginAmount = FunPriGetMarginAmout();
            //        }
            //        if (((System.Data.DataTable)Session["ApplicationAssetDetails"]).Rows.Count > 0)
            //        {
            //            decimal dcmAssetFinanceAmount = (decimal)((System.Data.DataTable)Session["ApplicationAssetDetails"]).Compute("Sum(Finance_Amount)", "Noof_Units > 0");
            //            decimal dcmAssetMarginAmount = Convert.ToDecimal(((System.Data.DataTable)Session["ApplicationAssetDetails"]).Compute("Sum(Margin_Amount)", "Noof_Units > 0"));
            //            if (dcmFinanceAmount != dcmAssetFinanceAmount)
            //            {
            //                cv_TabMainPage.ErrorMessage = "The sum of Finance Amount in AssetDetails should be equal to Finance Amount";
            //                cv_TabMainPage.IsValid = false;
            //                return;
            //            }
            //            else if (dcmMarginAmount > dcmAssetMarginAmount)
            //            {

            //                Utility.FunShowAlertMsg(this, " The sum of Margin Amount in AssetDetails should be greater than or equal to Margin Amount in ROI Rules");
            //                return;
            //            }
            //        }
            //    }
            //    //commented by saranya  
            //    // if (strLOBType != "te" && strLOBType != "tl" && strLOBType != "wc" && strLOBType != "ft")
            //    if (strLOBType != "tl" && strLOBType != "wc" && strLOBType != "ft")
            //    {
            //        if (strSplitNum == null && strConsNumber == null)
            //        {
            //            if (gvAssetDetails.Rows.Count > 0)
            //            {
            //                objProcedureParameter = new Dictionary<string, string>();
            //                objProcedureParameter.Add("@CompanyId", intCompanyId.ToString());
            //                objProcedureParameter.Add("@Panum", txtPrimeAccountNo.Text);
            //                objProcedureParameter.Add("@Sanum", txtSubAccountNo.Text);
            //                objProcedureParameter.Add("@XmlAssetDetails", gvAssetDetails.FunPubFormXml(true));
            //                objProcedureParameter.Add("@ApplicationProcessId", ddlApplicationReferenceNo.SelectedValue);
            //                System.Data.DataSet dsAccountAssetCountDetails = Utility.GetDataset("s3g_loanad_checkaccountassetcount", objProcedureParameter);
            //                if (dsAccountAssetCountDetails != null)
            //                {
            //                    foreach (DataRow drCurrentAssetCount in dsAccountAssetCountDetails.Tables[0].Rows)
            //                    {
            //                        int intCurrentAssetCount = Convert.ToInt32(drCurrentAssetCount["UNITCOUNT"]);
            //                        DataRow[] dsMLAAssetCount = dsAccountAssetCountDetails.Tables[1].Select("ASSETCODE = '" + Convert.ToString(drCurrentAssetCount["ASSETCODE"]) + "'");
            //                        if (dsMLAAssetCount.Length > 0)
            //                        {
            //                            int intMLAAssetCount = Convert.ToInt32(dsMLAAssetCount[0]["UNITCOUNT"]);
            //                            DataRow[] dsSLAAssetCount = dsAccountAssetCountDetails.Tables[2].Select("ASSETCODE = '" + Convert.ToString(drCurrentAssetCount["ASSETCODE"]) + "'");
            //                            if (dsSLAAssetCount.Length > 0)
            //                            {
            //                                int intSLAAssetCount = Convert.ToInt32(dsSLAAssetCount[0]["UNITCOUNT"]);
            //                                int intExistCount = intMLAAssetCount - intSLAAssetCount;
            //                                if (intCurrentAssetCount > intExistCount)
            //                                {
            //                                    cv_TabMainPage.ErrorMessage = "Unit Count Exceeded in Asset Details";
            //                                    cv_TabMainPage.IsValid = false;
            //                                    return;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                int intExistCount = intCurrentAssetCount - intMLAAssetCount;
            //                                if (intExistCount > 0)
            //                                {
            //                                    cv_TabMainPage.ErrorMessage = "Unit Count Exceeded in Asset Details";
            //                                    cv_TabMainPage.IsValid = false;
            //                                    return;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //if (tcAccountCreation.Tabs[6].Visible)
            //{
            //    if (ddlRepaymentMode.SelectedValue == "0")
            //    {
            //        cv_TabMainPage.ErrorMessage = "Select an Installment Repayment Mode";
            //        cv_TabMainPage.IsValid = false;
            //        return;
            //    }
            //    //if (ddlRepaymentMode.SelectedItem.Text.ToUpper().StartsWith("ECS"))
            //    //{
            //    //    if (string.IsNullOrEmpty(txtFBDate.Text) && txtFBDate.Enabled == true)
            //    //    {
            //    //        cv_TabMainPage.ErrorMessage = "Enter the FB Day";
            //    //        cv_TabMainPage.IsValid = false;
            //    //        return;
            //    //    }
            //    //}
            //}


            if (((System.Data.DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
            {
                ScriptManagerAlert("Repay details", "Add atleast one Repay details");
                tcAccountCreation.ActiveTabIndex = 2;
                return;
            }
            if (txtAccountIRR_Repay.Text == string.Empty || txtBusinessIRR_Repay.Text == string.Empty || txtCompanyIRR_Repay.Text == string.Empty)
            {
                ScriptManagerAlert("Repay details", "Recalculate IRR");
                tcAccountCreation.ActiveTabIndex = 2;
                //cv_TabMainPage.ErrorMessage = "Recalculate IRR";
                //cv_TabMainPage.IsValid = false;
                //tcAccountCreation.ActiveTabIndex = 2;
                return;

            }

            if (ddlRepaymentMode.SelectedValue == "0")
            {
                ScriptManagerAlert("Repay details", "Select the Installment Repayment Mode");
                tcAccountCreation.ActiveTabIndex = 2;
                return;
            }

            //for map invoices condition start
            if (ddlProposalType.SelectedValue == "4")//Rewrite
            {
                // Code Changed by Chandru K On 27 July 2016 For BU Id - 4495

                //dtRentalDetails = (DataTable)ViewState["dtRentalDetails"];

                //DataRow[] drREntal = dtRentalDetails.Select("PA_SA_REF_ID > 0");
                //if (drREntal.Length == 0)
                //{
                //    ScriptManagerAlert("Repay details", "Add atleast one Rental schedule");
                //    tcAccountCreation.ActiveTabIndex = 1;
                //    return;
                //}

                Label lblTotInvoice_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotInvoice_Amount") as Label;

                if (lblTotInvoice_Amount.Text == "")
                {
                    ScriptManagerAlert("Repay details", "Add atleast one Invoice");
                    tcAccountCreation.ActiveTabIndex = 1;
                    return;
                }

                // Code End
            }
            //By Siva.K on 19JUN2015 Validation State Mandatory for Billing Address
            if (ddlDeliveryType.SelectedItem.Text.ToUpper() == "BILLING ADDRESS" && ddlCust_Address.SelectedValue == "0")
            {
                ScriptManagerAlert("Delivery Address", "Select the State for the Billing Address");
                tcAccountCreation.ActiveTabIndex = 4;
                return;
            }

            string strValidDate = "";
            ViewState["ProposalOfferDate"] = txtOfferDate.Text;
            //DateTime.Parse(dsm.Tables[0].Rows[0]["Collateral_Storage_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            if (Utility.StringToDate(txtMRADate.Text) > Utility.StringToDate(txtOfferDate.Text))
            {
                strValidDate = txtMRADate.Text;
            }
            else
            {
                strValidDate = txtOfferDate.Text;
            }
            TextBox txtVerifyDate = new TextBox();
            txtVerifyDate.Text = strValidDate;
            if (Utility.StringToDate(txtAccountDate.Text) < Utility.StringToDate(txtVerifyDate.Text))
            {
                Utility.FunShowAlertMsg(this, "Account Date should be greater than Proposal date/MRA date");
                txtAccountDate.Text = "";
                TabMainPage.Focus();
                txtAccountDate.Focus();
                return;
            }

            if (ddlDeliveryType.SelectedValue == "3" && String.IsNullOrEmpty(txtGSTIN.Text))
            {
                Utility.FunShowAlertMsg(this, "Enter the GSTIN");
                txtGSTIN.Focus();
                return;
            }

            if (ddlRentalTDSSec.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "select Rental TDS Section in Invoice Details");
                return;
            }

            //for map invoices condition end

            FunPriSaveAccountCreation();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = ex.Message;
            cv_TabMainPage.IsValid = false;
        }

    }


    private void FunPriSaveAccountCreation()
    {
        ContractMgtServicesReference.ContractMgtServicesClient objAccountCreationClient = new ContractMgtServicesReference.ContractMgtServicesClient();
        try
        {
            string strPASANumber = string.Empty;
            S3GBusEntity.AccountCreationEntity objAccountCreationEntity = new AccountCreationEntity();

            objAccountCreationEntity.XmlCustEmailDetails = FunCustEmailBuilder();

            if (intCustEmailRows > 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Selected E-Mail Alert cannot be more than one");
                return;
            }

            #region Header
            objAccountCreationEntity.strPANumber = txtPrimeAccountNo.Text;
            objAccountCreationEntity.dtCreationDate = Utility.StringToDate(txtAccountDate.Text);
            objAccountCreationEntity.Instdate = Utility.StringToDate(txtfirstinstdate.Text);
            objAccountCreationEntity.RSCommencedate = Utility.StringToDate(TxtCommenceDate.Text);
            if (TxtRSSignDate.Text != "")
                objAccountCreationEntity.RSSigndate = Utility.StringToDate(TxtRSSignDate.Text);
            objAccountCreationEntity.InstDuedate = Utility.StringToDate(TxtFirstInstallDue.Text);
            objAccountCreationEntity.intCompanyId = intCompanyId;
            objAccountCreationEntity.intLobId = Convert.ToInt32(ddlLOB.SelectedValue);
            objAccountCreationEntity.intBranchId = Convert.ToInt32(ddlBranchList.SelectedValue);

            objAccountCreationEntity.Tranch_Header_ID = 0;

            if (intAccountCreationId > 0)
            {
                objAccountCreationEntity.intAccountCreationId = intAccountCreationId;
                objAccountCreationEntity.Tranch_Header_ID = 1;
                //objAccountCreationEntity.strSANum = txtSubAccountNo.Text;
            }
            if (intAccountCreationId == 0)
            {
                if ((Convert.ToString(ddlLientContract.SelectedValue) != "") && (Convert.ToString(ddlLientContract.SelectedValue) != "0"))
                    objAccountCreationEntity.intLien_Reference_ID = Convert.ToInt32(ddlLientContract.SelectedValue);
            }
            //if (intAccountCreationId == 0)
            //{
            if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                objAccountCreationEntity.strSAInternal_code_Ref = txtPrimeAccountNo.Text;//RS_No for create mode to select invoices
            else
                objAccountCreationEntity.strSAInternal_code_Ref = ddlApplicationReferenceNo.SelectedText;
            //objAccountCreationEntity.strSAInternal_code_Ref = ddlApplicationReferenceNo.SelectedItem.Text;//RS_No for create mode to select invoices
            objAccountCreationEntity.intProductId = Convert.ToInt32(0);

            if (strConsNumber == null && strSplitNum == null)
            {

                objAccountCreationEntity.intApplicationProcessId = Convert.ToInt32(ddlApplicationReferenceNo.SelectedValue);

            }
            else
            {
                objAccountCreationEntity.intApplicationProcessId = Convert.ToInt32(ddlApplicationReferenceNo.SelectedValue);
                if (strConsNumber != null)
                {
                    objAccountCreationEntity.strConsSplitNo = strConsNumber;
                }
                else
                {
                    objAccountCreationEntity.strConsSplitNo = strSplitNum;
                    objAccountCreationEntity.strSplit_RefNo = strSplitRefNo;
                }
            }

            if (string.IsNullOrEmpty(txtPrimeAccountNo.Text))
            {
                if (Convert.ToString(ViewState["strMLASLAApplicable"]) == "True")
                {
                    objAccountCreationEntity.intPAStatusCode = 10;
                }
                else
                {
                    objAccountCreationEntity.intPAStatusCode = 2;
                }
            }
            else
            {
                objAccountCreationEntity.intPAStatusCode = Convert.ToInt32(ddlStatus.SelectedValue);
            }

            if (!String.IsNullOrEmpty(strSplitNum))
            {
                //objAccountCreationEntity.strPANumber = "";
                objAccountCreationEntity.intPAStatusCode = 49;
            }

            //objAccountCreationEntity.intCustomerId = Convert.ToInt32(hdnCustomerId.Value);
            objAccountCreationEntity.intCustomerId = Convert.ToInt32(ViewState["Customer_Id"].ToString());
            objAccountCreationEntity.intPAStatusTypeCode = 25;
            objAccountCreationEntity.intTxnId = 1;
            objAccountCreationEntity.intLeaseType = 1;
            objAccountCreationEntity.intUserId = intUserId;
            objAccountCreationEntity.dcmFinanceAmount = Convert.ToDecimal(txtFinanceAmount.Text);
            //if (!string.IsNullOrEmpty(txtMarginMoneyPer_Cashflow.Text))
            //    objAccountCreationEntity.dcmOfferMargin = Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text);
            //if (!string.IsNullOrEmpty(txtMarginMoneyAmount_Cashflow.Text))
            //    objAccountCreationEntity.dcmOfferMarginAmount = Convert.ToDecimal(txtMarginMoneyAmount_Cashflow.Text);
            if (!string.IsNullOrEmpty(txtResidualValue_Cashflow.Text))
                objAccountCreationEntity.dcmOfferResidualValue = Convert.ToDecimal(txtResidualValue_Cashflow.Text);
            if (!string.IsNullOrEmpty(txtResidualAmt_Cashflow.Text))
                objAccountCreationEntity.dcmOfferResidualValueAmount = Convert.ToDecimal(txtResidualAmt_Cashflow.Text);
            if (!string.IsNullOrEmpty(hdnConstitutionId.Value))
                objAccountCreationEntity.intConstitutionId = Convert.ToInt32(hdnConstitutionId.Value);

            //For OPC Start
            if (!string.IsNullOrEmpty(ddlRegionalManager.SelectedValue) && !string.IsNullOrEmpty(ddlRegionalManager.SelectedText))
                objAccountCreationEntity.intSalesPersonId = Convert.ToInt32(ddlRegionalManager.SelectedValue);
            objAccountCreationEntity.Billing_Address = Convert.ToInt32(ddlDeliveryType.SelectedValue);
            if (!string.IsNullOrEmpty(ddlCust_Address.SelectedValue))
                objAccountCreationEntity.Cust_Address_ID = Convert.ToInt32(ddlCust_Address.SelectedValue);
            //By siva.K on 15JUN2015 Value not cleared eventhough data get Clear
            if (!string.IsNullOrEmpty(ddlAccountManager1.SelectedText) && !string.IsNullOrEmpty(ddlAccountManager1.SelectedValue))
                objAccountCreationEntity.Acc_Mngr1 = Convert.ToInt32(ddlAccountManager1.SelectedValue);
            if (!string.IsNullOrEmpty(ddlAccountManager2.SelectedText) && !string.IsNullOrEmpty(ddlAccountManager2.SelectedValue))
                objAccountCreationEntity.Acc_Mngr2 = Convert.ToInt32(ddlAccountManager2.SelectedValue);
            objAccountCreationEntity.Delivery_State = Convert.ToInt32(ddlDeliveryState.SelectedValue);
            objAccountCreationEntity.Billing_State = Convert.ToInt32(ddlBillingState.SelectedValue);
            objAccountCreationEntity.Proposal_Type = Convert.ToInt32(ddlProposalType.SelectedValue);
            if (RBLAdvanceRent.SelectedValue == "1")//Yes
                objAccountCreationEntity.Adv_Rent_Sec_Dep = Convert.ToInt32(RBLAdvanceRent.SelectedValue);

            objAccountCreationEntity.Secu_Deposit_Type = Convert.ToInt32(ddlSecuritydeposit.SelectedValue);
            if (!string.IsNullOrEmpty(txtSecDepAdvRent.Text))
                objAccountCreationEntity.AR_SD_Amount = Convert.ToDecimal(txtSecDepAdvRent.Text);
            objAccountCreationEntity.ReturnPattern = Convert.ToInt32(ddlReturnPattern.SelectedValue);
            objAccountCreationEntity.Seco_Term_Applicability = Convert.ToInt32(RBLSecondaryTerm.SelectedValue);
            if (!string.IsNullOrEmpty(txtOneTimeFee.Text))
                objAccountCreationEntity.One_Time_Fee = Convert.ToDecimal(txtOneTimeFee.Text);
            objAccountCreationEntity.Repayment_Mode = Convert.ToInt32(RBLStructuredEI.SelectedValue);
            if (!string.IsNullOrEmpty(txtProcessingFee.Text))
                objAccountCreationEntity.Processing_Fee_Per = Convert.ToDecimal(txtProcessingFee.Text);
            objAccountCreationEntity.VAT_Rebate_Applicability = Convert.ToInt32(RBLVATRebate.SelectedValue);
            objAccountCreationEntity.Remarks = txtRemarks.Text;
            intsales_type = Convert.ToInt32(ViewState["intsales_type"].ToString());
            objAccountCreationEntity.RS_type = intsales_type;
            objAccountCreationEntity.AMF_Sold = Convert.ToInt16(chkAmfsold.Checked);
            objAccountCreationEntity.VAT_Sold = Convert.ToInt16(chkVATSold.Checked);
            objAccountCreationEntity.ServiceTax_Sold = Convert.ToInt16(chkServiceTaxSold.Checked);


            //if (!string.IsNullOrEmpty(ddlSalesTax.Text))
            //{
            //    if (ddlSalesTax.Text == "2")
            //    {
            //        if (chkcstwith.Checked)
            //            objAccountCreationEntity.RS_type = 6;
            //        else
            //            objAccountCreationEntity.RS_type = Convert.ToInt32(ddlSalesTax.Text);//sales tax
            //    }
            //}
            objAccountCreationEntity.Cform_Number = txtCFormNo.Text;
            if (!string.IsNullOrEmpty(ddlSEZZone.SelectedValue))
                objAccountCreationEntity.SEZ_Zone = ddlSEZZone.SelectedValue;

            objAccountCreationEntity.ITC_Req = Convert.ToInt16(chkITC.Checked);
            
            objAccountCreationEntity.SEZA1 = Convert.ToInt16(chkSEZA1.Checked);

            if(chkWithIGST.Checked)
                objAccountCreationEntity.WithIGST = Convert.ToInt32(chkWithIGST.Checked);

            //if (ViewState["dtInvoicesACAT"] != null)
            //    dtInvoicesACAT = (System.Data.DataTable)ViewState["dtInvoicesACAT"];
            //objAccountCreationEntity.XMLInvoicesACAT = dtInvoicesACAT.FunPubFormXml(true);

            if (ViewState["dtEUCDetails"] != null)
            {
                dtEUCDetails = (System.Data.DataTable)ViewState["dtEUCDetails"];

                if (!string.IsNullOrEmpty(dtEUCDetails.Rows[0]["CustomerName"].ToString()))
                {
                    objAccountCreationEntity.XmlROIDetails = dtEUCDetails.FunPubFormXml(true);
                }
            }
            //Primary/Secondary
            System.Data.DataTable dt = new System.Data.DataTable();
            if (ViewState["dtPrimaryGrid"] != null)
            {
                dt = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
                //START By siva on 14MA2015 for fixed rate 
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToString(row["RentRate"]) == "")
                    {
                        row["RentRate"] = 0;
                    }
                    if (Convert.ToString(row["AMFRent"]) == "")
                        row["AMFRent"] = 0;
                    if (Convert.ToString(row["FixedRent"]) == "")
                        row["FixedRent"] = 0;
                    if (Convert.ToString(row["FixedAMF"]) == "")
                        row["FixedAMF"] = 0;

                }
                dt.AcceptChanges();
                //ENF By siva on 14MA2015 for fixed rate 
                objAccountCreationEntity.Sec_Tenure = Convert.ToInt32(24);//need to remove

                objAccountCreationEntity.XmlAssetDetails = dt.FunPubFormXml(true);

            }

            //objAccountCreationEntity.XMLInvoicesACATSummary  	=
            string strTaxXML = string.Empty;
            strTaxXML += "<Root>";
            if (chkLBT.Checked)
                strTaxXML += " <Details TAX_TYPE_ID='5' TAX_AMOUNT='0' IS_CONSIDER='1' />";
            if (chkPT.Checked)
                strTaxXML += " <Details TAX_TYPE_ID='1' TAX_AMOUNT='0' IS_CONSIDER='1' />";
            if (chkET.Checked)
                strTaxXML += " <Details TAX_TYPE_ID='3' TAX_AMOUNT='0' IS_CONSIDER='1' />";
            if (chkRC.Checked)
                strTaxXML += " <Details TAX_TYPE_ID='2' TAX_AMOUNT='0' IS_CONSIDER='1' />";
            if (chkbxGSTRC.Checked)
                strTaxXML += " <Details TAX_TYPE_ID='6' TAX_AMOUNT='0' IS_CONSIDER='1' />";
            strTaxXML += "</Root>";

            if (!string.IsNullOrEmpty(strTaxXML))
                objAccountCreationEntity.XMLTaxdetails = strTaxXML;//

            objAccountCreationEntity.strSA_User_Address1 = txtAddress1DA.Text;
            objAccountCreationEntity.strSA_User_Address2 = null;
            objAccountCreationEntity.strSA_User_City = null;
            objAccountCreationEntity.strSA_User_State = null;
            objAccountCreationEntity.strSA_User_Country = null;
            objAccountCreationEntity.strSA_User_Pincode = null;
            objAccountCreationEntity.strSA_User_Phone = txtTelephoneDA.Text;
            objAccountCreationEntity.strSA_User_Mobile = txtMobileDA.Text;
            objAccountCreationEntity.strSA_User_Pin = txtPin.Text;

            //}
            #endregion

            #region Details

            #region Constitution
            XMLCommon = grvConsDocuments.FunPubFormXml(true);
            XMLCommon = XMLCommon.Replace("VALUES=''", "VALUES=' '");
            objAccountCreationEntity.XmlConstitutionDocDetails = XMLCommon;
            #endregion

            #region ROI Details
            //FunPriUpdateROIRuleDecRate();
            //objAccountCreationEntity.XmlROIDetails = ((System.Data.DataTable)ViewState["ROIRules"]).FunPubFormXml(true);
            #endregion

            #region PASA Account

            //objAccountCreationEntity.intROIRuleID = Convert.ToInt32(ddlROIRuleList.SelectedValue);
            //if (!string.IsNullOrEmpty(txtLastODICalcDate.Text))
            //    objAccountCreationEntity.dtLastODICalcDate = Utility.StringToDate(txtLastODICalcDate.Text);
            //if (!string.IsNullOrEmpty(txtAdvanceInstallments.Text))
            //    objAccountCreationEntity.intAdvanceInstallments = Convert.ToInt32(txtAdvanceInstallments.Text);
            //if (!string.IsNullOrEmpty(txtFBDate.Text))
            //    objAccountCreationEntity.intFBDate = Convert.ToInt32(txtFBDate.Text);



            objAccountCreationEntity.blnIsDORequired = false;
            if (!string.IsNullOrEmpty(txtAccountingIRR.Text))
                objAccountCreationEntity.dcmAccountingIRR = Convert.ToDecimal(txtAccountingIRR.Text);
            if (!string.IsNullOrEmpty(txtBusinessIRR.Text))
                objAccountCreationEntity.dcmBusinessIRR = Convert.ToDecimal(txtBusinessIRR.Text);
            if (!string.IsNullOrEmpty(txtCompanyIRR.Text))
                objAccountCreationEntity.dcmCompanyIRR = Convert.ToDecimal(txtCompanyIRR.Text);

            if (ddlPaymentRuleList.SelectedIndex != -1)
            {
                objAccountCreationEntity.intPaymentRuleId = Convert.ToInt32(ddlPaymentRuleList.SelectedValue);
            }
            else
            {
                objAccountCreationEntity.intPaymentRuleId = 0;
            }
            if (!string.IsNullOrEmpty(TxtAutoExtnRental.Text))
                objAccountCreationEntity.dcmAutoRentAmount = Convert.ToDecimal(TxtAutoExtnRental.Text);
            objAccountCreationEntity.dcmLoanAmount = Convert.ToDecimal(txtFinanceAmount.Text);
            objAccountCreationEntity.intRepaymentTypecode = 63;
            objAccountCreationEntity.intRepaymentTimeTypeCode = 64;
            objAccountCreationEntity.intTenure = Convert.ToInt32(txtTenure.Text);
            objAccountCreationEntity.intTenureTypeCode = 17;
            objAccountCreationEntity.intTenureCode = Convert.ToInt32(ddlTenureType.SelectedValue);
            //objAccountCreationEntity.intRepaymentCode = Convert.ToInt32(ddlRepaymentMode.SelectedValue);
            //objAccountCreationEntity.intRepaymentTimeCode = Convert.ToInt32(ddl_Time_Value.SelectedValue);
            objAccountCreationEntity.intRepaymentCode = Convert.ToInt32(ddlRepaymentMode.SelectedValue);
            if (!string.IsNullOrEmpty(hdnTimeValue.Value))
                objAccountCreationEntity.intRepaymentTimeCode = Convert.ToInt32(hdnTimeValue.Value);
            if (!string.IsNullOrEmpty(hdnRentfrequency.Value))
                objAccountCreationEntity.intROIRuleID = Convert.ToInt32(hdnRentfrequency.Value);//Rent frequency


            #endregion

            #region Guarantor
            string strXMLGuarantor = ((System.Data.DataTable)ViewState["dtGuarantorGrid"]).FunPubFormXml(true);
            objAccountCreationEntity.XmlGuarantorDetails = strXMLGuarantor;
            #endregion

            if (intAccountCreationId == 0)
            {


                #region AssetDetails
                //XMLCommon = ((System.Data.DataTable)Session["ApplicationAssetDetails"]).FunPubFormXml(true);
                //XMLCommon = XMLCommon.Replace("PAYTO='&nbsp;'", "");
                //XMLCommon = XMLCommon.Replace("ENTITYNAME='&nbsp;'", "");
                //XMLCommon = XMLCommon.Replace("SLNO=''", "");
                //XMLCommon = XMLCommon.Replace("''", "'0'");
                //objAccountCreationEntity.XmlAssetDetails = XMLCommon;
                //if (ViewState["dtInvoicesACAT"] != null)
                //    dtInvoicesACAT = (System.Data.DataTable)ViewState["dtInvoicesACAT"];
                //objAccountCreationEntity.XMLInvoicesACAT = dtInvoicesACAT.FunPubFormXml(true);

                #endregion

                #region Moratorium
                //if (((System.Data.DataTable)ViewState["dtMoratorium"]).Rows.Count > 0)
                //{
                //    XMLCommon = ((System.Data.DataTable)ViewState["dtMoratorium"]).FunPubFormXml(true);
                //    objAccountCreationEntity.XmlMoratoriumDetails = XMLCommon;
                //}
                #endregion
            }
            #region Inflow
            if (((System.Data.DataTable)ViewState["DtCashFlow"]).Rows.Count > 0)
            {
                DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
                DataRow[] drCashflow = DtCashFlow.Select("CashFlow_Flag_ID = 30");
                if (drCashflow.Length > 0)
                {
                    if (drCashflow[0]["Date"] == String.Empty)
                    {
                        Utility.FunShowAlertMsg(this.Page, "InFlow Date should not be Empty");
                        return;
                    }
                }
                XMLCommon = ((System.Data.DataTable)ViewState["DtCashFlow"]).FunPubFormXml(true);
                objAccountCreationEntity.XmlCashInflowDetails = XMLCommon;
            }
            #endregion

            #region OutFlow
            XMLCommon = ((System.Data.DataTable)ViewState["DtCashFlowOut"]).FunPubFormXml(true);
            objAccountCreationEntity.XmlOutFlowDetails = XMLCommon;
            #endregion

            #region Repayment
            objAccountCreationEntity.XmlRepaymentDetails = ((System.Data.DataTable)ViewState["DtRepayGrid"]).FunPubFormXml(true);
            if (grvRepayStructure.Rows.Count > 0)
            {
                XMLCommon = ((System.Data.DataTable)ViewState["RepaymentStructure"]).FunPubFormXml(true);
                objAccountCreationEntity.XmlRepaymentStructure = XMLCommon;
            }


            ////Added by saran on 3-Jul-2014 for CR_SISSL12E046_018 start
            //System.Data.DataTable dtRepayDetailsOthers = new System.Data.DataTable();
            //if (ViewState["dtRepayDetailsOthers"] != null)
            //{
            //    dtRepayDetailsOthers = (System.Data.DataTable)ViewState["dtRepayDetailsOthers"];
            //}
            //if (dtRepayDetailsOthers.Rows.Count > 0)
            //{

            //    objAccountCreationEntity.XMLRepayDetailsOthers = dtRepayDetailsOthers.FunPubFormXml(true);
            //}

            ////Added by saran on 3-Jul-2014 for CR_SISSL12E046_018 end


            #endregion

            //#region ALERT
            //XMLCommon = ((System.Data.DataTable)ViewState["DtAlertDetails"]).FunPubFormXml(true);
            //objAccountCreationEntity.XmlAlertDetails = XMLCommon;
            //#endregion

            //#region Followup
            //objAccountCreationEntity.XmlFollowDetails = ((System.Data.DataTable)ViewState["DtFollowUp"]).FunPubFormXml(true);
            //#endregion

            objAccountCreationEntity.XmlInvoiceDetails = gvInvoiceDetails.FunPubFormXml(true);

            //For Rental Schedule Numbers
            if (ViewState["dtRentalDetails"] != null)
                dtRentalDetails = (System.Data.DataTable)ViewState["dtRentalDetails"];

            if (dtRentalDetails.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtRentalDetails.Rows[0]["PA_SA_REF_ID"].ToString()))
                    objAccountCreationEntity.XMLRepayDetailsOthers = dtRentalDetails.FunPubFormXml(true);
            }

            if (!String.IsNullOrEmpty(txtAddInvAmt.Text))
                objAccountCreationEntity.dcmReset_Amount = Convert.ToDecimal(txtAddInvAmt.Text);

            if (ViewState["ReWriteAmount"] != null)
            {
                dtReWriteAmount = (System.Data.DataTable)ViewState["ReWriteAmount"];
                objAccountCreationEntity.XMLReWriteAmount = dtReWriteAmount.FunPubFormXml(true);
            }

            if (ViewState["RWInvoiceAmount"] != null)
            {
                dtReWriteAmount = (System.Data.DataTable)ViewState["RWInvoiceAmount"];
                objAccountCreationEntity.XMLRWInvoiceAmount = dtReWriteAmount.FunPubFormXml(true);
            }

            /*Added by vinodha m for the call id 3663 on may 20,2016*/
            objAccountCreationEntity.Is_StdFreq_Applicable = Convert.ToInt32(chkApplicable.Checked);

            //Added by Chandru K for Call Ref ID - 4154 and 4203 on  11 July,2016
            objAccountCreationEntity.Is_StdFreq_Applicable_Sec = Convert.ToInt32(chkApplicableSec.Checked);

            //Added by Chandru K for Call Ref ID - 12219 on  12 July,2018
            objAccountCreationEntity.Is_FullRental = Convert.ToInt32(chkFullRental.Checked);

            //Added by Chandru K for Call Ref ID 4154, CR_56 on  06 Feb, 2017
            objAccountCreationEntity.IsSep_Amort = Convert.ToInt32(chkIsSecondary.Checked);

            //5093 start
            // objAccountCreationEntity.CST_Deal = Convert.ToInt32(chkCSTDeal.Checked);

            if (ViewState["cstleasing"] != null)
                objAccountCreationEntity.CST_Deal = Convert.ToInt32(ViewState["cstleasing"].ToString());
            else
                objAccountCreationEntity.CST_Deal = 0;
            if (ViewState["vatleasing"] != null)
                objAccountCreationEntity.VAT_Leasing = Convert.ToInt32(ViewState["vatleasing"].ToString());
            else
                objAccountCreationEntity.VAT_Leasing = 0;
            //5093 end

            if (!String.IsNullOrEmpty(txtGSTIN.Text))
                objAccountCreationEntity.GSTIN = txtGSTIN.Text;

            if (!String.IsNullOrEmpty(txtLabel.Text))
                objAccountCreationEntity.Lable = txtLabel.Text;

            if (!String.IsNullOrEmpty(txtAddress.Text))
                objAccountCreationEntity.Address = txtAddress.Text;

            objAccountCreationEntity.CST_Deal = Convert.ToInt16(chkITC_Cap.Checked);

            objAccountCreationEntity.Rental_TDS_Sec = (ddlRentalTDSSec.SelectedValue);

            
            #endregion
            if (intAccountCreationId == 0)
            {
                intResult = objAccountCreationClient.FunPubInsertAccountCreationInt(out strPASANumber, objAccountCreationEntity);
                //intResult = 0;//for checking workflow
            }

            else
            {
                //intResult = objAccountCreationClient.FunPubModifyAccountCreationInt(objAccountCreationEntity);
                intResult = objAccountCreationClient.FunPubInsertAccountCreationInt(out strPASANumber, objAccountCreationEntity);
            }
            if (intResult == 0)
            {
                if (intAccountCreationId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Rental Schedule Details updated successfully");
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                }
                else
                {
                    ViewState["strPASANumber"] = strPASANumber;
                    //WorkFlowSave();  

                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler) 
                    {

                        WorkFlowSession WFValues = new WorkFlowSession();
                        int intWorkflowStatus = 0;
                        WFValues.LastDocumentNo = strPASANumber;
                        // To insert SA Reference KR 
                        if (TabSLADetails.Visible == true)
                            WFValues.SANUM = strPASANumber;
                        else
                        {
                            WFValues.PANUM = strPASANumber;
                            WFValues.SANUM = strPASANumber + "DUMMY";
                        }
                        try
                        {
                            intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strPASANumber, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            strAlert = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }

                        //strRedirectPageView = strRedirectHomePage;
                        ShowWFAlertMessage(strPASANumber, ProgramCode, strAlert);
                        return;
                    }
                    else if (Request.QueryString["IsFromAccount"] == null)
                    {
                        strAlert = "Rental Schedule " + strPASANumber + " created successfully";
                        strAlert += @"\n\nWould you like to create one more Rental Schedule?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END

                    }
                    else
                    {
                        // FORCE PULL IMPLEMENTATION KR change this to right else case
                        System.Data.DataTable WFFP = new System.Data.DataTable();
                        if (CheckForForcePullOperation(null, ddlApplicationReferenceNo.SelectedText, ProgramCode, null, null, "O", CompanyId, null, null, ddlLOB.SelectedValue, txtProductCode.Text.Trim(), out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), strPASANumber, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), 0);
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is not assinged";
                            }
                        }

                        if (strSplitNum != "" && strSplitRefNo == "1")
                        {
                            strAlert = "alert('Rental Schedule " + strPASANumber + " created successfully";
                            strAlert += @"\n\nPlease modify the Parent RS, To complete the Split.');";
                        }
                        else if (strSplitNum != "" && strSplitRefNo == "2")
                        {
                            strAlert = "alert('Rental Schedule " + strPASANumber + " modified successfully');";
                        }
                        else
                        {
                            strAlert = "alert('Rental Schedule " + strPASANumber + " created successfully');";
                        }

                        strAlert += "window.close();window.opener.location.reload();";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    }


                }
                Session.Remove("ApplicationAssetDetails");
                Session.Remove("AccountAssetCustomer");
            }
            else if (intResult == -1)
            {
                if (intAccountCreationId == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                }
                strRedirectPageView = "";
            }
            else if (intResult == -2)
            {
                if (intAccountCreationId == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                }
                strRedirectPageView = "";
            }
            else if (intResult == 100)
            {
                strAlert = strAlert.Replace("__ALERT__", "Enter Rental Schedule Number.");
                strRedirectPageView = "";
            }
            else if (intResult == 101)
            {
                strAlert = strAlert.Replace("__ALERT__", "Schedule Number should be start with Customer Short Name.");
                strRedirectPageView = "";
            }
            else if (intResult == 102)
            {
                strAlert = strAlert.Replace("__ALERT__", "Last 4 digit should be numeric in Schedule Number.");
                strRedirectPageView = "";
            }
            else if (intResult == 103)
            {
                strAlert = strAlert.Replace("__ALERT__", "Schedule Number already Exists.");
                strRedirectPageView = "";
            }
            else if (intResult == 104)
            {
                strAlert = strAlert.Replace("__ALERT__", "Maturity Date of Lien Schedule must be within Maturity Date of Parent Rental Schedule.");
                strRedirectPageView = "";
            }
            else if (intResult == 159)//For RW/RW sale or lease back 
            {
                strAlert = strAlert.Replace("__ALERT__", "Map atleast one existing rental schedule.");
                strRedirectPageView = "";
            }
            else if (intResult == 160)//For RW/RW sale or lease back 
            {
                strAlert = strAlert.Replace("__ALERT__", "Map atleast one Invoices.");
                strRedirectPageView = "";
            }
            else if (intResult == 161)//For balance amount<utilised
            {
                //if (intAccountCreationId == 0)
                //{
                strAlert = strAlert.Replace("__ALERT__", "Rental Schedule Amount should not exceed Balance amount");
                //}
                strRedirectPageView = "";
            }
            else if (intResult == 162)
            {
                strAlert = strAlert.Replace("__ALERT__", "GSTIN is not avilable in corporate address.");
                strRedirectPageView = "";
            }
            else if (intResult == 163)
            {
                strAlert = strAlert.Replace("__ALERT__", "GSTIN is not avilable in billing address.");
                strRedirectPageView = "";
            }
            else if (intResult == 173)
            {
                strAlert = strAlert.Replace("__ALERT__", "Delivery Address State can not differ from Billing State");
                strRedirectPageView = "";
            }
            //added by swarna on 13th Aug as per changes by Thalai
            else if (intResult == 777)//For prop amount>utilised
            {
                //if (intAccountCreationId == 0)
                //{
                strAlert = strAlert.Replace("__ALERT__", "Rental Schedule Amount should not exceed Proposal amount");
                //}
                strRedirectPageView = "";
            }
            else
            {
                if (intAccountCreationId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Due to Data Problem,Unable to modifying Rental Schedule Details");
                }
                else
                {

                    strAlert = strAlert.Replace("__ALERT__", "Due to Data Problem,Unable to creating an Rental Schedule ");

                }
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objAccountCreationClient.Close();
        }
    }


    #region " WORK FLOW SAVE"
    private void WorkFlowSave()
    {
        WorkFlowSession WFValues = new WorkFlowSession();
        System.Data.DataTable dtWorkFlow;
        int WFProgramId = 0;
        try
        {
            if (isWorkFlowTraveler) // WORK FLOW TRAVELLER 
            {
                int intWorkflowStatus = UpdateWorkFlowTasks(intCompanyId.ToString(), intUserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, 0);
            }
            else if (CheckForWorkFlowConfiguration(ProgramCode, WFLOBId, WFProductId, out WFProgramId, out dtWorkFlow) > 0) // WORK FLOW STARTER
            {
                int intWorkflowStatus = InsertWorkFlowTasks(intCompanyId.ToString(), intUserId.ToString(), WFLOBId, WFBranchId, ViewState["strPASANumber"].ToString(), WFProgramId, WFValues.ProductId, 0);
            }
            strAlert = "";
        }
        catch (Exception ex)
        {
            strAlert = "Work Flow is Not assigned";
        }
        // Navigate to Next WF Page
        ShowWFAlertMessage(ViewState["strPASANumber"].ToString(), WFValues.WorkFlowProgramId.ToString(), strAlert);

    }
    /* WorkFlow Properties */
    private int WFLOBId { get { return int.Parse(ddlLOB.SelectedValue); } }
    private int WFBranchId { get { return int.Parse(ddlBranchList.SelectedValue); } }
    private int WFProductId { get { return int.Parse(hdnProductId.Value); } }

    #endregion

    private void ScriptManagerAlert(string Title, string AlertMsg)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Title, "alert('" + AlertMsg + "');", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvRepaymentDetails.Rows.Count > 0)
            {
                FunPriBindRepaymentDLL(_Add);
                FunPriIRRReset();
                ViewState["RepaymentStructure"] = null;
                grvRepayStructure.DataSource = null;
                grvRepayStructure.DataBind();
                gvRepaymentSummary.ClearGrid();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Reset the Repayment & IRR Details";
            cv_TabMainPage.IsValid = false;
        }
    }
    protected void FunCalculateIRR(object sender, EventArgs e)
    {
        try
        {
            decimal decActualAmount, decTotalAmount = 0;
            string strType;
            string stroption;
            RepaymentType rePayType = new RepaymentType();
            strType = ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim();
            switch (strType.ToLower())
            {
                case "te":
                case "tl":
                    rePayType = RepaymentType.TLE;
                    //tenure = 1;
                    break;
                case "ft":
                    rePayType = RepaymentType.FC;
                    // tenure = 1;
                    break;
                case "wc":
                    rePayType = RepaymentType.WC;
                    //tenure = 1;
                    break;
                default:
                    rePayType = RepaymentType.EMI;
                    break;
            }
            //if (ddl_Return_Pattern.SelectedItem.Text == "PTF (Per thousand frequency)" || ddl_Return_Pattern.SelectedItem.Text == "PLF (Per lakh frequency)" || ddl_Return_Pattern.SelectedItem.Text == "PMF (Per million frequency)")
            if (ddlReturnPattern.SelectedItem.Text == "PTF (Per thousand frequency)" || ddlReturnPattern.SelectedItem.Text == "PLF (Per lakh frequency)" || ddlReturnPattern.SelectedItem.Text == "PMF (Per million frequency)")
            {
                stroption = "3";
            }
            else
            {
                stroption = "2";
            }
            if (FunPriValidateTotalAmount(out decActualAmount, out decTotalAmount, stroption))
            {
                //int intToInstallment =Convert.ToInt32(DtRepayGrid.Rows[DtRepayGrid.Rows.Count - 1]["ToInstall"].ToString());


                //if (FunPriValidateTenurePeriod(intToInstallment))
                //{
                CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();

                System.Data.DataTable dtRepaymentTab = (System.Data.DataTable)ViewState["DtRepayGrid"];
                System.Data.DataTable dtCashInflow = (System.Data.DataTable)ViewState["DtCashFlow"];
                System.Data.DataTable dtCashOutflow = (System.Data.DataTable)ViewState["DtCashFlowOut"];
                double decResultIrr = 0;
                decimal decPLR = 0;
                if (hdnPLR.Value != "")
                {
                    decPLR = Convert.ToDecimal(hdnPLR.Value);
                }
                string strIrrRest = string.Empty;
                string strTimeval = string.Empty;
                //switch (ddl_IRR_Rest.SelectedItem.Text.ToLower())
                //{
                //    case "day wise irr":
                //        strIrrRest = "daily";
                //        break;
                //    case "month wise irr":
                //        strIrrRest = "monthly";
                //        break;
                //    default:
                //        strIrrRest = "daily";
                //        break;

                //}
                strIrrRest = "monthly";
                //switch (ddl_Time_Value.SelectedItem.Text.ToLower())
                switch (hdnTimeValue.Value.ToLower())
                {
                    case "adv(advance)":
                    case "adf(advance fbd)":
                        strTimeval = "advance";

                        break;
                    case "arr(arrears)":
                    case "arf(arrears fbd)":
                        strTimeval = "arrears";
                        break;
                    default:
                        strTimeval = "advance";
                        break;
                }
                // txtRepaymentTime.Text = strTimeval;
                decimal decRate = 0;
                double docRate = 0;
                //switch (ddl_Return_Pattern.SelectedItem.Text)
                switch (ddlReturnPattern.SelectedItem.Text)
                {

                    //case "IRR (Internal Rate of Return)":
                    //ObjBusinessLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), Convert.ToDouble(txtRate.Text), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out docRate, Convert.ToDecimal(10.05), decPLR);
                    //decRate = Convert.ToDecimal(docRate);
                    //break;
                    default:
                        //decRate = Convert.ToDecimal(txtRate.Text);
                        break;
                }
                //Function for Calculating IRR Called
                decimal? decResvalue = null;
                decimal? decResAmt = null;
                if (txtResidualAmt_Cashflow.Text != "")
                {
                    decResAmt = Convert.ToDecimal(txtResidualAmt_Cashflow.Text);
                }
                if (txtResidualValue_Cashflow.Text != "")
                {
                    decResvalue = Convert.ToDecimal(txtResidualValue_Cashflow.Text);
                }
                //ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtAccountDate.Text), decResvalue, decResAmt, rePayType);


                txtAccountIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtAccountingIRR.Text = decResultIrr.ToString("0.0000");

                //ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Business_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtAccountDate.Text), decResvalue, decResAmt, rePayType);
                txtBusinessIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtBusinessIRR.Text = decResultIrr.ToString("0.0000");

                //ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Company_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtAccountDate.Text), decResvalue, decResAmt, rePayType);
                txtCompanyIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtCompanyIRR.Text = decResultIrr.ToString("0.0000");


                // Utility.FunShowAlertMsg(this, decResultIrr.ToString("0.0000"));
                //}
                //else
                //{
                //    Utility.FunShowAlertMsg(this, "Tenure period should be equal to "+ txtTenure.Text +" "+ddlTenureType.SelectedItem.Text);
                //    return;
                //}
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + ex.Message.Replace("'", " ").Replace(";", " ") + "');", true);
        }
    }
    private decimal FunPriGetAmountFinanced()
    {
        try
        {
            decimal decFinanaceAmt;
            decFinanaceAmt = Convert.ToDecimal(txtFinanceAmount.Text);// -FunPriGetMarginAmout();
            return Math.Round(decFinanaceAmt, 0);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to getting finance amount");
        }
    }
    //protected void txt_Margin_Percentage_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunPriAssignMarginAmount();
    //    }
    //    catch (Exception ex)
    //    {
    //        cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
    //        cv_TabMainPage.IsValid = false;
    //    }
    //}
    //private void FunPriAssignMarginAmount()
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(txt_Margin_Percentage.Text))
    //        {
    //            txtMarginMoneyPer_Cashflow.Text = txt_Margin_Percentage.Text;
    //            txtMarginMoneyPer_Cashflow.ReadOnly = true;
    //            txtMarginMoneyAmount_Cashflow.ReadOnly = true;
    //            txtMarginMoneyAmount_Cashflow.Text = FunPriGetMarginAmout().ToString();

    //        }
    //        else
    //        {
    //            txtMarginMoneyPer_Cashflow.Text = txtMarginMoneyAmount_Cashflow.Text = "";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw new ApplicationException("Unable to Assign the Margin Amount");
    //    }
    //}
    private void FunPriIRRReset()
    {
        try
        {
            txtAccountingIRR.Text =
            txtAccountIRR_Repay.Text =
            txtBusinessIRR.Text =
            txtBusinessIRR_Repay.Text =
            txtCompanyIRR.Text =
            txtCompanyIRR_Repay.Text = "";
            if (ViewState["DtCashFlow"] != null)
            {
                System.Data.DataTable DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
                if (DtCashFlow.Rows.Count > 0)
                {
                    DataRow[] drUMFC = null;
                    if (DtCashFlow.Columns.Contains("CashFlow_ID"))
                    {
                        drUMFC = DtCashFlow.Select("CashFlow_ID = 34");

                    }
                    else
                    {
                        drUMFC = DtCashFlow.Select("CashFlow_Flag_ID = 34");
                    }
                    if (drUMFC.Length > 0)
                    {
                        drUMFC[0].Delete();
                        DtCashFlow.AcceptChanges();
                        ViewState["DtCashFlow"] = DtCashFlow;
                        if (DtCashFlow.Rows.Count > 0)
                        {
                            FunPriBindInflowDLL(_Edit);
                        }
                        else
                        {
                            FunPriBindInflowDLL(_Add);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private System.Data.DataSet FunPriGetAccountConsolidationDetails(string strConsNumber)
    {

        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Add("@ConsNo", strConsNumber);
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Option", "1");
        System.Data.DataTable dtConPanSANum = Utility.GetDefaultData("S3G_LOANAD_GetAccConsDetails", Procparam);
        System.Data.DataSet dsConsolidationDetails = new System.Data.DataSet();
        if (dtConPanSANum.Rows.Count > 0)
        {
            Procparam.Clear();
            Procparam.Add("@ConsNo", strConsNumber);
            Procparam.Add("@PANum", dtConPanSANum.Rows[0]["PA_Number"].ToString());
            ViewState["PANUM"] = dtConPanSANum.Rows[0]["PA_Number"].ToString();
            Procparam.Add("@SANum", dtConPanSANum.Rows[0]["SA_Number"].ToString());
            Procparam.Add("@CompanyId", intCompanyId.ToString());
            txtFinanceAmount.Text = Math.Round(Convert.ToDecimal(dtConPanSANum.Rows[0]["Finance_Amount"]), 0).ToString();
            dsConsolidationDetails = Utility.GetDataset("S3G_LoanAd_GetAccountDetails_Consolidation", Procparam);
        }
        return dsConsolidationDetails;
    }

    private System.Data.DataSet FunPriGetAccountSplitDetails(string strSplitNumber, string strRefNo)
    {

        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Add("@SplitNo", strSplitNumber);
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@SplitRefNo", strRefNo);
        Procparam.Add("@Option", "1");
        System.Data.DataSet dsSplitPANSANum = Utility.GetDataset("S3G_LOANAD_GetAccSplitDetails", Procparam);
        System.Data.DataSet dsSplitDetails = new System.Data.DataSet();
        if (dsSplitPANSANum.Tables[0].Rows.Count > 0)
        {
            Procparam.Clear();
            Procparam.Add("@PANum", dsSplitPANSANum.Tables[0].Rows[0]["PANum"].ToString());
            ViewState["PANUM"] = dsSplitPANSANum.Tables[0].Rows[0]["PANum"].ToString();
            Procparam.Add("@SANum", dsSplitPANSANum.Tables[0].Rows[0]["SANum"].ToString());
            ViewState["SANUM"] = dsSplitPANSANum.Tables[0].Rows[0]["SANum"].ToString();
            Procparam.Add("@Split_Ref_Number", strRefNo);
            Procparam.Add("@CompanyId", intCompanyId.ToString());
            Procparam.Add("@Split_No", strSplitNumber);
            txtFinanceAmount.Text = Math.Round(Convert.ToDecimal(dsSplitPANSANum.Tables[1].Rows[0]["Finance_Amount"]), 0).ToString();
            dsSplitDetails = Utility.GetDataset("S3G_LOANAD_GetAccountDetails_Split", Procparam);
        }
        return dsSplitDetails;
    }

    private bool FunPriCheckMLA(string PANUM)
    {
        string[] strPanum = PANUM.Split(',');

        if (strPanum.Length > 1)
            return true;
        else
            return false;
    }
    #region Button Events
    protected void btnGenerateRepay_Click(object sender, EventArgs e)
    {
        try
        {
            FunGenerateSchedule();
            //ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            //if (objRepaymentStructure.FunPubGetCashFlowDetails(intCompanyId, Convert.ToInt32(ddlLOB.SelectedValue)).Rows.Count == 0)
            //{
            //    Utility.FunShowAlertMsg(this, "Define Installment Flag in Cashflow Master for selected Line of Business");
            //    return;
            //}
            ////Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
            //strDocumentDate = txtAccountDate.Text;
            //if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            //{
            //    if (gvAssetDetails != null)
            //    {
            //        System.Data.DataTable dtAssetDtls = new System.Data.DataTable();
            //        if (Session["ApplicationAssetDetails"] != null)
            //            dtAssetDtls = (System.Data.DataTable)Session["ApplicationAssetDetails"];
            //        if (dtAssetDtls.Rows.Count > 0)
            //        {
            //            if (!string.IsNullOrEmpty(dtAssetDtls.Compute("Max(Required_FromDate)", "Noof_Units > 0").ToString()))
            //                strDocumentDate = dtAssetDtls.Compute("Max(Required_FromDate)", "Noof_Units > 0").ToString();
            //        }

            //    }
            //}
            ////Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

            //if (!string.IsNullOrEmpty(txtFBDate.Text))
            //{
            //    DateTime dtDocDate = Utility.StringToDate(strDocumentDate);
            //    DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
            //    dtformat.ShortDatePattern = "MM/dd/yy";

            //    //Changed by Thangam M on 16/Jan/2013 to fix FBD issue as per Application Process
            //    //string strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
            //    string strFBDate = "";
            //    try
            //    {
            //        strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
            //    }
            //    catch (Exception ex)
            //    {
            //        DateTime lastDayOfCurrentMonth = new DateTime(dtDocDate.Year, dtDocDate.Month,
            //                                            DateTime.DaysInMonth(dtDocDate.Year, dtDocDate.Month));
            //        strFBDate = lastDayOfCurrentMonth.ToString(strDateFormat);
            //    }

            //    //string strFBDate = txtFBDate.Text + "/" + dtDocDate.Month + "/" + dtDocDate.Year;
            //    GenerateRepaymentSchedule(objRepaymentStructure, Utility.StringToDate(strFBDate));
            //}
            //else
            //{
            //    GenerateRepaymentSchedule(objRepaymentStructure, Utility.StringToDate(strDocumentDate));
            //}
            ///*UMFC has been calculated automatically for other than Product & TermLoan Return Pattern 
            //(Also applicable to HP,FL,LN,TE,TL) Updated on 28th Oct 2010*/
            //if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL") && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            //{
            //    FunPriInsertUMFC();
            //}

        }
        catch (Exception ex)
        {

            if (ex.Message.Contains("cannot calculate IRR"))
            {
                if (strSplitNum == null && strConsNumber == null)//For Consolidation & Split
                {
                    cv_TabMainPage.ErrorMessage = "Incorrect cashflow details,cannot calculate IRR";
                }
                else
                {
                    cv_TabMainPage.ErrorMessage = "Cannot calculate IRR,Re-enter Cashflow details";
                }
            }
            else
            {
                cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Calculate IRR/ Generate Repayment Structure";
            }
            cv_TabMainPage.IsValid = false;
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    private void FunPriInsertUMFC()
    {
        try
        {
            DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
            DataRow[] drCashflow = DtCashFlow.Select("CashFlow_Flag_ID = 34");
            if (drCashflow.Length > 0)
            {
                drCashflow[0].Delete();
                DtCashFlow.AcceptChanges();
            }
            System.Data.DataSet dsUMFC = (System.Data.DataSet)ViewState["InflowDDL"];
            DataRow dr = DtCashFlow.NewRow();
            //DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["Date"], DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["InflowFromId"], DtCashFlow.Columns["EntityID"] };
            //dr["Date"] = DateTime.Today.ToString();
            //dr["Date"] = Utility.StringToDate(txtAccountDate.Text);
            dr["Date"] = Utility.StringToDate(txtfirstinstdate.Text);
            string[] strArrayIds = null;
            string cashflowdesc = "";
            foreach (DataRow drOut in dsUMFC.Tables[2].Rows)
            {
                string[] strCashflow = drOut["CashFlow_ID"].ToString().Split(',');
                if (strCashflow[4].ToString() == "34")
                {
                    strArrayIds = strCashflow;
                    cashflowdesc = drOut["CashFlow_Description"].ToString();
                }
            }
            if (strArrayIds == null)
            {
                Utility.FunShowAlertMsg(this, "Define the Cashflow for UMFC in Cashflow Master");
                return;
            }
            dr["CashInFlowID"] = strArrayIds[0];
            dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
            dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
            dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
            dr["CashFlow_Flag_ID"] = strArrayIds[4];
            dr["CashInFlow"] = cashflowdesc;
            //dr["EntityID"] = hdnCustomerId.Value;
            dr["EntityID"] = ViewState["Customer_Id"].ToString();
            dr["Entity"] = S3GCustomerAddress1.CustomerName;
            dr["InflowFromId"] = "144";
            dr["InflowFrom"] = "Customer";
            //if (ddl_Repayment_Mode.SelectedValue == "2")
            //{
            //    dr["Amount"] = FunPriGetStructureAdhocInterestAmount().ToString();
            //}
            //else
            //{
            dr["Amount"] = FunPriGetInterestAmount().ToString();
            //}

            DtCashFlow.Rows.Add(dr);

            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();

            ViewState["DtCashFlow"] = DtCashFlow;
            FunPriGenerateNewInflowRow();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    //private decimal FunPriGetStructureAdhocInterestAmount()
    //{
    //    decimal decFinAmount = FunPriGetAmountFinanced();
    //    decimal decRate = 0;

    //    switch (ddl_Return_Pattern.SelectedValue)
    //    {
    //        case "1":
    //            decRate = Convert.ToDecimal(txtRate.Text);
    //            break;
    //        case "2":
    //            if (ViewState["decRate"] != null)
    //            {
    //                decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
    //            }
    //            break;

    //    }
    //    string strLOB = ddlLOB.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower();
    //    switch (strLOB)
    //    {
    //        case "tl":
    //        case "te":
    //            if (ddl_Repayment_Mode.SelectedValue == "5")
    //            {
    //                decRate = 0;
    //            }
    //            break;
    //        case "ft":
    //        case "wc":
    //            decRate = 0;
    //            break;
    //    }

    //    return Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(ddlTenureType.SelectedItem.Text.ToLower(), decFinAmount, decRate, int.Parse(txtTenure.Text)), 0);
    //}




    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }


    protected void btnConfigure_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                if (((System.Data.DataTable)ViewState["DtCashFlowOut"]).Rows.Count == 0)
                {
                    cv_TabMainPage.ErrorMessage = "Define an OfferTerms Related Details";
                    cv_TabMainPage.IsValid = false;
                    tcAccountCreation.ActiveTabIndex = 0;
                    return;
                }
                FunPriCalculateIRR();
            }
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Configure the IRR Details";
            cv_TabMainPage.IsValid = false;
        }
    }
    #endregion

    private decimal FunPriGetInterestAmount()
    {
        decimal decFinAmount = FunPriGetAmountFinanced();
        decimal decUMFC = 0;
        if (!string.IsNullOrEmpty(lblTotalAmount.Text))
        {
            string strTotalAmount = (lblTotalAmount.Text.Split(':').Length > 1) ? lblTotalAmount.Text.Split(':')[1].Trim() : "";
            if (strTotalAmount != "")
            {
                decimal decTotalRepayable = Convert.ToDecimal(strTotalAmount);
                decUMFC = decTotalRepayable - decFinAmount;
            }
        }
        return decUMFC;
        //decimal decRate = 0;

        //switch (ddl_Return_Pattern.SelectedValue)
        //{
        //    case "1":
        //        decRate = Convert.ToDecimal(txtRate.Text);
        //        break;
        //    case "2":
        //        if (ViewState["decRate"] != null)
        //        {
        //            decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
        //        }
        //        break;
        //}
        //string strLOB = ddlLOB.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower();
        //switch (strLOB)
        //{
        //    case "tl":
        //    case "te":
        //        if (ddl_Repayment_Mode.SelectedValue == "5")
        //        {
        //            decRate = 0;
        //        }
        //        break;
        //    case "ft":
        //    case "wc":
        //        decRate = 0;
        //        break;
        //}

        //return Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(ddlTenureType.SelectedItem.Text.ToLower(), decFinAmount, decRate, int.Parse(txtTenure.Text)), 0);
    }


    //protected void GenerateRepaymentSchedule(ClsRepaymentStructure objRepaymentStructure, DateTime dtStartDate)
    //{
    //    try
    //    {
    //        System.Data.DataSet dsRepayGrid = new System.Data.DataSet();
    //        System.Data.DataTable dtRepayDetails = new System.Data.DataTable();
    //        //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
    //        System.Data.DataTable dtRepayDetailsOthers = new System.Data.DataTable();
    //        //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end
    //        System.Data.DataTable dtMoratorium = null;
    //        Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
    //        objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text);
    //        objMethodParameters.Add("Tenure", txtTenure.Text);
    //        objMethodParameters.Add("TenureType", ddlTenureType.SelectedItem.Text);

    //        if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
    //        {
    //            System.Data.DataTable dsAssetDetails = (System.Data.DataTable)Session["ApplicationAssetDetails"];
    //            decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
    //            objMethodParameters.Add("FinanceAmount", dcmTotalAssetValue.ToString());
    //        }
    //        else
    //        {
    //            objMethodParameters.Add("FinanceAmount", txtFinanceAmount.Text);
    //        }
    //        /*objMethodParameters.Add("ReturnPattern", ddl_Return_Pattern.SelectedValue);
    //        objMethodParameters.Add("MarginPercentage", txtMarginMoneyPer_Cashflow.Text);
    //         */
    //        objMethodParameters.Add("ReturnPattern", ddlReturnPattern.SelectedValue);
    //        objMethodParameters.Add("MarginPercentage", "");
    //        //objMethodParameters.Add("Rate", txtRate.Text);
    //        //objMethodParameters.Add("TimeValue", ddl_Time_Value.SelectedValue);
    //        //objMethodParameters.Add("RepaymentMode", ddl_Repayment_Mode.SelectedValue);
    //        objMethodParameters.Add("TimeValue", hdnTimeValue.Value);
    //        objMethodParameters.Add("RepaymentMode", "1");//EMI
    //        objMethodParameters.Add("CompanyId", intCompanyId.ToString());
    //        objMethodParameters.Add("LobId", ddlLOB.SelectedValue);
    //        //objMethodParameters.Add("DocumentDate", (txtAccountDate.Text == "") ? txtApplicationDate.Text : txtAccountDate.Text);
    //        objMethodParameters.Add("DocumentDate", (txtAccountDate.Text == "") ? txtOfferDate.Text : txtAccountDate.Text);
    //        objMethodParameters.Add("Frequency", ddl_Frequency.SelectedValue);
    //        //objMethodParameters.Add("RecoveryYear1", txt_Recovery_Pattern_Year1.Text);
    //        //objMethodParameters.Add("RecoveryYear2", txt_Recovery_Pattern_Year2.Text);
    //        //objMethodParameters.Add("RecoveryYear3", txt_Recovery_Pattern_Year3.Text);
    //        //objMethodParameters.Add("RecoveryYear4", txt_Recovery_Pattern_Rest.Text);
    //        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE") && ddlRepaymentMode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
    //        {
    //            objMethodParameters.Add("PrincipalMethod", "1");
    //        }
    //        else
    //        {
    //            objMethodParameters.Add("PrincipalMethod", "0");
    //        }

    //        if (ViewState["hdnRoundOff"] != null)
    //        {
    //            if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
    //                objMethodParameters.Add("Roundoff", ViewState["hdnRoundOff"].ToString());
    //            else
    //                objMethodParameters.Add("Roundoff", "2");
    //        }
    //        else
    //        {
    //            objMethodParameters.Add("Roundoff", "2");
    //        }
    //        System.Data.DataTable dtOutflow = ((System.Data.DataTable)ViewState["DtCashFlowOut"]).Clone();
    //        if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
    //        {
    //            System.Data.DataSet dsOutlfow = (DataSet)ViewState["OutflowDDL"];
    //            DataRow drOutflow = dtOutflow.NewRow();
    //            drOutflow["Date"] = Utility.StringToDate(txtAccountDate.Text);

    //            drOutflow["CashOutFlow"] = "Ol Lease Amount";
    //            //drOutflow["EntityID"] = hdnCustomerId.Value;
    //            drOutflow["EntityID"] = ViewState["Customer_Id"].ToString();
    //            drOutflow["Entity"] = S3GCustomerAddress1.CustomerName;
    //            drOutflow["OutflowFromId"] = "144";
    //            drOutflow["OutflowFrom"] = "Customer";
    //            System.Data.DataTable dsAssetDetails = (System.Data.DataTable)Session["ApplicationAssetDetails"];
    //            decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
    //            drOutflow["Amount"] = dcmTotalAssetValue;

    //            drOutflow["CashOutFlowID"] = "-1";
    //            drOutflow["Accounting_IRR"] = true;
    //            drOutflow["Business_IRR"] = true;
    //            drOutflow["Company_IRR"] = true;
    //            drOutflow["CashFlow_Flag_ID"] = "41";
    //            dtOutflow.Rows.Add(drOutflow);
    //        }

    //        //For TL
    //        ViewState["DtRepayGrid_TL"] = null;

    //        //Checking if other than normal payment , start date should be last payment date.
    //        if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
    //        {
    //            System.Data.DataTable dtAcctype = ((System.Data.DataTable)ViewState["PaymentRules"]);
    //            dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
    //            string strAcctType = dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper();

    //            if (strAcctType == "PROJECT FINANCE" || strAcctType == "DEFERRED PAYMENT" || strAcctType == "DEFERRED STRUCTURED")
    //            {
    //                DtCashFlowOut = (System.Data.DataTable)ViewState["DtCashFlowOut"];
    //                if (DtCashFlowOut.Rows.Count > 0)
    //                {
    //                    DataRow drOutFlw = DtCashFlowOut.Select("CashFlow_Flag_ID=41").Last();
    //                    if (drOutFlw != null)
    //                    {
    //                        objMethodParameters.Remove("DocumentDate");
    //                        objMethodParameters.Add("DocumentDate", drOutFlw["Date"].ToString());
    //                        dtStartDate = Utility.StringToDate(drOutFlw["Date"].ToString());

    //                        if (!string.IsNullOrEmpty(txtFBDate.Text))
    //                        {
    //                            //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
    //                            //DateTime dtDocDate = Utility.StringToDate(txtDate.Text);
    //                            DateTime dtDocDate = dtStartDate;
    //                            //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

    //                            DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
    //                            dtformat.ShortDatePattern = "MM/dd/yy";
    //                            string strFBDate = "";
    //                            try
    //                            {
    //                                strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
    //                            }
    //                            catch (Exception ex)
    //                            {
    //                                DateTime lastDayOfCurrentMonth = new DateTime(dtDocDate.Year, dtDocDate.Month,
    //                                                                    DateTime.DaysInMonth(dtDocDate.Year, dtDocDate.Month));
    //                                strFBDate = lastDayOfCurrentMonth.ToString(strDateFormat);
    //                            }
    //                            //string strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
    //                            dtStartDate = Utility.StringToDate(strFBDate);
    //                        }

    //                    }
    //                }

    //            }
    //        }






    //        if (ddl_Return_Pattern.SelectedValue == "2")
    //        {
    //            if (txtResidualAmt_Cashflow.Text.Trim() != "" && txtResidualAmt_Cashflow.Text.Trim() != "0")
    //            {
    //                objMethodParameters.Add("decResidualAmount", txtResidualAmt_Cashflow.Text);
    //            }
    //            if (txtResidualValue_Cashflow.Text.Trim() != "" && txtResidualValue_Cashflow.Text.Trim() != "0")
    //            {
    //                objMethodParameters.Add("decResidualValue", txtResidualValue_Cashflow.TemplateSourceDirectory);
    //            }
    //            switch (ddl_IRR_Rest.SelectedValue)
    //            {
    //                case "1":
    //                    objMethodParameters.Add("strIRRrest", "daily");
    //                    break;
    //                case "2":
    //                    objMethodParameters.Add("strIRRrest", "monthly");
    //                    break;
    //                default:
    //                    objMethodParameters.Add("strIRRrest", "daily");
    //                    break;

    //            }

    //            objMethodParameters.Add("decLimit", "0.10");
    //            decimal decRateOut = 0;
    //            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
    //            {
    //                dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, (System.Data.DataTable)ViewState["DtCashFlow"], dtOutflow, objMethodParameters, dtMoratorium, out decRateOut);
    //            }
    //            else
    //            {
    //                dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, (System.Data.DataTable)ViewState["DtCashFlow"], (System.Data.DataTable)ViewState["DtCashFlowOut"], objMethodParameters, dtMoratorium, out decRateOut);
    //            }
    //            ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);
    //        }
    //        else
    //        {
    //            dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, objMethodParameters, dtMoratorium);
    //        }

    //        decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text);
    //        if (dsRepayGrid == null)
    //        {
    //            /* It Calculates and displays the Repayment Details for ST-ADHOC */
    //            grvRepayStructure.DataSource = null;
    //            grvRepayStructure.DataBind();
    //            FunPriShowRepaymetDetails(decFinAmount + FunPriGetStructureAdhocInterestAmount());
    //            gvRepaymentDetails.FooterRow.Visible = true;
    //            btnReset.Enabled = true;
    //            return;
    //        }
    //        if (dsRepayGrid.Tables[0].Rows.Count > 0)
    //        {
    //            gvRepaymentDetails.DataSource = dsRepayGrid.Tables[0];
    //            gvRepaymentDetails.DataBind();
    //            ViewState["DtRepayGrid"] = dsRepayGrid.Tables[0];
    //            if (ddl_Rate_Type.SelectedItem.Text == "Floating" && string.IsNullOrEmpty(txtFBDate.Text))
    //            {
    //                ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
    //                ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
    //            }
    //            else
    //            {
    //                ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
    //                ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
    //            }
    //            btnReset.Enabled = false;
    //            FunPriCalculateSummary(dsRepayGrid.Tables[0], "CashFlow", "TotalPeriodInstall");
    //            //decimal decBreakPercent = ((decimal)((System.Data.DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
    //            decimal decBreakPercent;// = ((decimal)((System.Data.DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
    //            if (!((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))))
    //            {
    //                decBreakPercent = ((decimal)((System.Data.DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
    //            }
    //            else
    //            {
    //                DataRow[] dr = (((System.Data.DataTable)ViewState["DtRepayGrid"])).Select("CashFlow_Flag_ID IN(35,91)");
    //                if (dr.Length == 0)
    //                {
    //                    decBreakPercent = ((decimal)((System.Data.DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
    //                }
    //                else
    //                {
    //                    decBreakPercent = ((decimal)((System.Data.DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID in(91,35)"));
    //                }


    //            }
    //            if (decBreakPercent != 0)
    //            {
    //                if (decBreakPercent != 100)
    //                {
    //                    Utility.FunShowAlertMsg(this, "Total break up percentage should be equal to 100%");
    //                    return;
    //                }
    //            }
    //            double douAccountingIRR = 0;
    //            double douBusinessIRR = 0;
    //            double douCompanyIRR = 0;
    //            System.Data.DataTable dtRepaymentStructure = new System.Data.DataTable();

    //            try
    //            {

    //                string strStartDte = txtAccountDate.Text;
    //                DateTime dtDocFBDate = Utility.StringToDate(strStartDte);
    //                int intDeffered = 0;
    //                if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
    //                {
    //                    System.Data.DataTable dtAcctype = ((System.Data.DataTable)ViewState["PaymentRules"]);
    //                    dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
    //                    string strAcctType = dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper();

    //                    if (strAcctType == "PROJECT FINANCE" || strAcctType == "DEFERRED PAYMENT" || strAcctType == "DEFERRED STRUCTURED")
    //                    {
    //                        intDeffered = 1;//Defferred Payment
    //                        DtCashFlowOut = (System.Data.DataTable)ViewState["DtCashFlowOut"];
    //                        if (DtCashFlowOut.Rows.Count > 0)
    //                        {
    //                            DataRow drOutFlw = DtCashFlowOut.Select("CashFlow_Flag_ID=41").Last();
    //                            if (drOutFlw != null)
    //                            {
    //                                strStartDte = drOutFlw["Date"].ToString();




    //                            }
    //                        }

    //                    }
    //                }
    //                if (!string.IsNullOrEmpty(txtFBDate.Text))
    //                {
    //                    //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
    //                    //DateTime dtDocDate = Utility.StringToDate(txtDate.Text);
    //                    DateTime dtDocDate = Utility.StringToDate(strStartDte);
    //                    //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

    //                    DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
    //                    dtformat.ShortDatePattern = "MM/dd/yy";
    //                    string strFBDate = "";
    //                    try
    //                    {
    //                        strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        DateTime lastDayOfCurrentMonth = new DateTime(dtDocDate.Year, dtDocDate.Month,
    //                                                            DateTime.DaysInMonth(dtDocDate.Year, dtDocDate.Month));
    //                        strFBDate = lastDayOfCurrentMonth.ToString(strDateFormat);
    //                    }
    //                    //string strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
    //                    dtDocFBDate = Utility.StringToDate(strFBDate);
    //                }


    //                if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
    //                {
    //                    System.Data.DataTable dsAssetDetails = (System.Data.DataTable)Session["ApplicationAssetDetails"];
    //                    decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
    //                    objRepaymentStructure.FunPubCalculateIRR((txtAccountDate.Text == "") ? txtApplicationDate.Text : txtAccountDate.Text, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualAmt_Cashflow.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
    //                        , out dtRepaymentStructure
    //                        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
    //                            , out dtRepayDetailsOthers
    //                        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
    //                        , (System.Data.DataTable)ViewState["DtRepayGrid"], (System.Data.DataTable)ViewState["DtCashFlow"], dtOutflow
    //                        , dcmTotalAssetValue.ToString(), txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
    //                        , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
    //                        ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium);
    //                }
    //                else
    //                {
    //                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
    //                    {
    //                        objRepaymentStructure.FunPubCalculateIRR(strStartDte, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualAmt_Cashflow.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
    //                            , out dtRepaymentStructure, out dtRepayDetails
    //                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
    //                        , out dtRepayDetailsOthers
    //                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
    //                            , (System.Data.DataTable)ViewState["DtRepayGrid"], (System.Data.DataTable)ViewState["DtCashFlow"], (System.Data.DataTable)ViewState["DtCashFlowOut"]
    //                            , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
    //                            , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
    //                            ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium, ddl_Interest_Levy.SelectedValue.ToString(), intDeffered, dtDocFBDate.ToString());

    //                        _SlNo = 0;
    //                        gvRepaymentDetails.DataSource = dtRepayDetails;
    //                        gvRepaymentDetails.DataBind();
    //                        ViewState["DtRepayGrid_TL"] = ((System.Data.DataTable)ViewState["DtRepayGrid"]).Copy();
    //                        ViewState["DtRepayGrid"] = dtRepayDetails;
    //                    }
    //                    else
    //                    {

    //                        objRepaymentStructure.FunPubCalculateIRR((txtAccountDate.Text == "") ? txtApplicationDate.Text : txtAccountDate.Text, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualAmt_Cashflow.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
    //                         , out dtRepaymentStructure
    //                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
    //                                , out dtRepayDetailsOthers
    //                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end                     
    //                         , (System.Data.DataTable)ViewState["DtRepayGrid"], (System.Data.DataTable)ViewState["DtCashFlow"], (System.Data.DataTable)ViewState["DtCashFlowOut"]
    //                         , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
    //                         , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
    //                         ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium);
    //                    }
    //                }
    //                dtRepaymentStructure.Columns["Charge"].ColumnName = "FinanceCharges";
    //                ViewState["RepaymentStructure"] = dtRepaymentStructure;
    //                grvRepayStructure.DataSource = dtRepaymentStructure;
    //                grvRepayStructure.DataBind();

    //                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
    //                if (dtRepayDetailsOthers != null)
    //                    ViewState["dtRepayDetailsOthers"] = dtRepayDetailsOthers;
    //                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end

    //                txtAccountIRR_Repay.Text = douAccountingIRR.ToString("0.0000");
    //                txtAccountingIRR.Text = douAccountingIRR.ToString("0.0000");

    //                txtBusinessIRR_Repay.Text = douBusinessIRR.ToString("0.0000");
    //                txtBusinessIRR.Text = douBusinessIRR.ToString("0.0000");

    //                txtCompanyIRR_Repay.Text = douCompanyIRR.ToString("0.0000");
    //                txtCompanyIRR.Text = douCompanyIRR.ToString("0.0000");


    //            }
    //            catch (Exception Ex1)
    //            {
    //                ClsPubCommErrorLog.CustomErrorRoutine(Ex1, strPageName);
    //                throw Ex1;
    //            }
    //        }
    //        else
    //        {
    //            gvRepaymentDetails.FooterRow.Visible = true;
    //            btnReset.Enabled = true;
    //            btnCalIRR.Enabled = true;
    //            ViewState["RepaymentStructure"] = null;
    //            grvRepayStructure.DataSource = null;
    //            grvRepayStructure.DataBind();
    //        }
    //        //decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text);
    //        if (dsRepayGrid.Tables[0].Rows.Count > 0)
    //        {
    //            FunPriShowRepaymetDetails((decimal)dsRepayGrid.Tables[0].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
    //        }
    //        else
    //        {
    //            FunPriShowRepaymetDetails(decFinAmount + FunPriGetInterestAmount());
    //        }

    //        FunPriGenerateNewRepaymentRow();
    //        FunPriUpdateROIRule();
    //        if (ddl_Repayment_Mode.SelectedValue != "2")
    //        {
    //            Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
    //            if (lblCashFlowId.Text != "23")
    //            {
    //                ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
    //            }
    //        }
    //        else
    //        {
    //            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}



    private void FunPriShowRepaymetDetails(decimal decAmountRepayble)
    {

        if (txtTenure.Text != "" || txtTenure.Text != string.Empty)
        {
            lblTotalAmount.Text = "Total Amount Repayable : " + decAmountRepayble.ToString(Funsetsuffix());
            lblFrequency_Display.Text = "Tenure &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + txtTenure.Text + " " + ddlTenureType.SelectedItem.Text;
            //if (txtRate.Text.Trim() != "")
            //{
            //    if (ddl_Return_Pattern.SelectedValue == "2")
            //    {
            //        if (ViewState["decRate"] != null)
            //        {
            //            lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + ViewState["decRate"].ToString();
            //        }
            //    }
            //    else
            //    {
            //        lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + txtRate.Text;
            //    }
            //}

        }

    }
    private void FunPriCalculateIRR()
    {
        try
        {
            decimal decActualAmount, decTotalAmount = 0;
            string strType;
            string stroption;
            RepaymentType rePayType = new RepaymentType();
            strType = ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim();
            switch (strType.ToLower())
            {
                case "te":
                case "tl":
                    rePayType = RepaymentType.TLE;
                    //tenure = 1;
                    break;
                case "ft":
                    rePayType = RepaymentType.FC;
                    // tenure = 1;
                    break;
                case "wc":
                    rePayType = RepaymentType.WC;
                    //tenure = 1;
                    break;
                default:
                    rePayType = RepaymentType.EMI;
                    break;
            }
            //if (ddl_Return_Pattern.SelectedItem.Text == "PTF (Per thousand frequency)" || ddl_Return_Pattern.SelectedItem.Text == "PLF (Per lakh frequency)" || ddl_Return_Pattern.SelectedItem.Text == "PMF (Per million frequency)")
            if (ddlReturnPattern.SelectedItem.Text == "PTF (Per thousand frequency)" || ddlReturnPattern.SelectedItem.Text == "PLF (Per lakh frequency)" || ddlReturnPattern.SelectedItem.Text == "PMF (Per million frequency)")
            {
                stroption = "3";
            }
            else
            {
                stroption = "2";
            }
            if (FunPriValidateTotalAmount(out decActualAmount, out decTotalAmount, stroption))
            {
                CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();

                System.Data.DataTable dtRepaymentTab = (System.Data.DataTable)ViewState["DtRepayGrid"];
                System.Data.DataTable dtCashInflow = (System.Data.DataTable)ViewState["DtCashFlow"];
                System.Data.DataTable dtCashOutflow = (System.Data.DataTable)ViewState["DtCashFlowOut"];
                double decResultIrr = 0;
                decimal decPLR = 0;
                if (hdnPLR.Value != "")
                {
                    decPLR = Convert.ToDecimal(hdnPLR.Value);
                }
                string strIrrRest = string.Empty;
                string strTimeval = string.Empty;
                //switch (ddl_IRR_Rest.SelectedItem.Text.ToLower())
                //{
                //    case "day wise irr":
                //        strIrrRest = "daily";
                //        break;
                //    case "month wise irr":
                //        strIrrRest = "monthly";
                //        break;
                //    default:
                //        strIrrRest = "daily";
                //        break;

                //}
                strIrrRest = "monthly";
                //switch (ddl_Time_Value.SelectedItem.Text.ToLower())
                switch (hdnTimeValue.Value.ToLower())
                {
                    case "adv(advance)":
                    case "adf(advance fbd)":
                        strTimeval = "advance";
                        break;
                    case "arr(arrears)":
                    case "arf(arrears fbd)":
                        strTimeval = "arrears";
                        break;
                    default:
                        strTimeval = "advance";
                        break;
                }

                decimal decRate = 0;
                double docRate = 0;
                //switch (ddl_Return_Pattern.SelectedItem.Text)
                //switch (ddlReturnPattern.SelectedItem.Text)
                //{

                //    case "IRR (Internal Rate of Return)":
                //        ObjBusinessLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), Convert.ToDouble(txtRate.Text), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out docRate, Convert.ToDecimal(10.05), decPLR);
                //        decRate = Convert.ToDecimal(docRate);
                //        break;
                //    default:
                //        decRate = Convert.ToDecimal(txtRate.Text);
                //        break;
                //}
                //Function for Calculating IRR Called
                decimal? decResvalue = null;
                decimal? decResAmt = null;
                if (txtResidualAmt_Cashflow.Text != "")
                {
                    decResAmt = Convert.ToDecimal(txtResidualAmt_Cashflow.Text);
                }
                if (txtResidualValue_Cashflow.Text != "")
                {
                    decResvalue = Convert.ToDecimal(txtResidualValue_Cashflow.Text);
                }
                //                ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtAccountDate.Text), decResvalue, decResAmt, rePayType);


                txtAccountIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtAccountingIRR.Text = decResultIrr.ToString("0.0000");

                // ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Business_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtAccountDate.Text), decResvalue, decResAmt, rePayType);
                txtBusinessIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtBusinessIRR.Text = decResultIrr.ToString("0.0000");

                // ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Company_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtAccountDate.Text), decResvalue, decResAmt, rePayType);
                txtCompanyIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtCompanyIRR.Text = decResultIrr.ToString("0.0000");
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            //For Consolidation & Split
            if (strSplitNum == null && strConsNumber == null)
            {
                throw new ApplicationException("Incorrect cashflow details,cannot calculate IRR");
            }
            else
            {
                throw new ApplicationException("Cannot calculate IRR,Re-enter Cashflow details");
            }
        }
    }

    protected void ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlCashFlowDesc = sender as DropDownList;
            if (ddlCashFlowDesc.SelectedIndex > 0)
                FunPriDoCashflowBasedValidation(ddlCashFlowDesc);
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to fetching values based on cash flow details";
            cv_TabMainPage.IsValid = false;
        }

    }

    private void FunPriDoCashflowBasedValidation(DropDownList ddlCashFlowDesc)
    {
        try
        {

            string[] strvalues = ddlCashFlowDesc.SelectedValue.Split(',');
            TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            TextBox txtPerInstallmentAmount_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            TextBox txtBreakup_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;


            if (!ddlLOB.SelectedItem.Text.Contains("TL"))
            {
                if (strvalues[4].ToString() != "23")
                {
                    txtFromInstallment_RepayTab1_upd.Attributes.Remove("readonly");
                    txtFromInstallment_RepayTab1_upd.ReadOnly = false;
                    CalendarExtenderSD_ToDate_RepayTab.Enabled = false;
                    CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                    txtfromdate_RepayTab1_Upd.Text = "";
                    txtBreakup_RepayTab1.Text = "";
                    txtBreakup_RepayTab1.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    //if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
                    if (hdnTimeValue.Value == "2" || hdnTimeValue.Value == "4")
                    {
                        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                        objRepaymentStructure.dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(
                            //ddl_Frequency.SelectedValue
                            hdnRentfrequency.Value
                            , Utility.StringToDate(DateTime.Now.ToString(strDateFormat)));
                        if (gvRepaymentDetails.Rows.Count > 0 && txtfromdate_RepayTab1_Upd.Text == "")  // 24 Jan 2012 By Rao. Fixed Observation- From Date Overlapping issue while selecting cashflow. 
                            txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    }
                    else
                    {
                        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                        objRepaymentStructure.FunPubGetNextRepaydate((System.Data.DataTable)ViewState["DtRepayGrid"], hdnRentfrequency.Value);
                        //ddl_Frequency.SelectedValue
                        txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(objRepaymentStructure.intNextInstall + 1);
                        txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    }

                    //if (ddl_Rate_Type.SelectedItem.Text.Trim().ToUpper() == "FLOATING")
                    //{
                    //    if (((System.Data.DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
                    //    {
                    //        CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                    //        txtfromdate_RepayTab1_Upd.ReadOnly = false;
                    //    }
                    //    else
                    //    {
                    //        CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                    //        txtfromdate_RepayTab1_Upd.ReadOnly = true;
                    //    }
                    //}
                    //else
                    //{
                    CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                    txtfromdate_RepayTab1_Upd.ReadOnly = true;
                    //}

                    txtFromInstallment_RepayTab1_upd.Attributes.Add("readonly", "readonly");
                    txtBreakup_RepayTab1.Attributes.Remove("readonly");
                    txtFromInstallment_RepayTab1_upd.ReadOnly = true;

                    CalendarExtenderSD_ToDate_RepayTab.Enabled = true;
                    CalendarExtenderSD_ToDate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;


                    CalendarExtenderSD_fromdate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;
                }

            }
            else
            {
                if (strvalues[4].ToString() != "91")
                {
                    txtFromInstallment_RepayTab1_upd.Attributes.Remove("readonly");
                    txtFromInstallment_RepayTab1_upd.ReadOnly = false;
                    CalendarExtenderSD_ToDate_RepayTab.Enabled = false;
                    CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                    txtfromdate_RepayTab1_Upd.Text = "";
                    txtBreakup_RepayTab1.Text = "";
                    txtBreakup_RepayTab1.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    //if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
                    if (hdnTimeValue.Value == "2" || hdnTimeValue.Value == "4")
                    {
                        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                        objRepaymentStructure.dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(hdnRentfrequency.Value
                            //ddl_Frequency.SelectedValue
                            , Utility.StringToDate(DateTime.Now.ToString(strDateFormat)));
                        if (gvRepaymentDetails.Rows.Count > 0 && txtfromdate_RepayTab1_Upd.Text == "")  // 24 Jan 2012 By Rao. Fixed Observation- From Date Overlapping issue while selecting cashflow. 
                            txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    }
                    else
                    {
                        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                        objRepaymentStructure.FunPubGetNextRepaydateTL((System.Data.DataTable)ViewState["DtRepayGrid"], hdnRentfrequency.Value
                            //ddl_Frequency.SelectedValue
                            , strvalues[4].ToString());
                        txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(objRepaymentStructure.intNextInstall + 1);
                        txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    }

                    //if (ddl_Rate_Type.SelectedItem.Text.Trim().ToUpper() == "FLOATING")
                    //{
                    //    if (((System.Data.DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
                    //    {
                    //        CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                    //        txtfromdate_RepayTab1_Upd.ReadOnly = false;
                    //    }
                    //    else
                    //    {
                    //        CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                    //        txtfromdate_RepayTab1_Upd.ReadOnly = true;
                    //    }
                    //}
                    //else
                    //{
                    CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                    txtfromdate_RepayTab1_Upd.ReadOnly = false;
                    //}
                }
                //txtFromInstallment_RepayTab1_upd.Attributes.Add("readonly", "readonly");
                //txtBreakup_RepayTab1.Attributes.Remove("readonly");
                //txtFromInstallment_RepayTab1_upd.ReadOnly = true;

                CalendarExtenderSD_ToDate_RepayTab.Enabled = true;
                CalendarExtenderSD_ToDate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;


                CalendarExtenderSD_fromdate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw new ApplicationException(ex.Message);
        }
    }
    protected void txRepaymentFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtBoxFromdate = (TextBox)sender;
            //if (Utility.CompareDates(txtAccountDate.Text, txtBoxFromdate.Text) == -1)
            if (Utility.CompareDates(txtfirstinstdate.Text, txtBoxFromdate.Text) == -1)
            {
                Utility.FunShowAlertMsg(this, "From Date should be greater than or equal to Rental Schedule Date");
                return;
            }
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            if (objRepaymentStructure.FunPubGetCashFlowDetails(intCompanyId, Convert.ToInt32(ddlLOB.SelectedValue)).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Define Installment Flag in Cashflow Master for selected Line of Business");
                return;
            }
            FunPriIRRReset();
            //strDocumentDate = txtBoxFromdate.Text;
            //GenerateRepaymentSchedule(objRepaymentStructure, Utility.StringToDate(txtBoxFromdate.Text));
            /*UMFC has been calculated automatically for other than Product & TermLoan Return Pattern 
            (Also applicable to HP,FL,LN,TE,TL) Updated on 28th Oct 2010*/
            //if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL") && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            //{
            //    FunPriInsertUMFC();
            //}

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Generate Repayment Schedule";
            cv_TabMainPage.IsValid = false;
        }
    }
    /*
    protected void txtResidualValue_Cashflow_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtResidualValue_Cashflow.Text != "")
            {
                rfvResidualValue.Enabled = false;
                txtResidualAmt_Cashflow.ReadOnly = true;
            }
            else
            {
                rfvResidualValue.Enabled = true;
                txtResidualAmt_Cashflow.ReadOnly = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void txtResidualAmt_Cashflow_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtResidualAmt_Cashflow.Text != "")
            {
                rfvResidualValue.Enabled = false;
                txtResidualValue_Cashflow.ReadOnly = true;
            }
            else
            {
                rfvResidualValue.Enabled = true;
                txtResidualValue_Cashflow.ReadOnly = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    */

    //  protected void txtResidualValue_Cashflow_TextChanged(object sender, EventArgs e)
    //  {
    //      try
    //      {
    //          //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 start

    //          if (txtResidualValue_Cashflow.Text.Trim() != "")
    //          {
    //              //rfvResidualValue.Enabled = false;
    //              //txtResidualAmt_Cashflow.ReadOnly = true;
    //              if (txtFinanceAmount.Text != "")
    //              {
    //                  txtResidualAmt_Cashflow.Text =
    //                       Math.Round(((Convert.ToDecimal(txtResidualValue_Cashflow.Text) * Convert.ToDecimal(txtFinanceAmount.Text)) / 100), 0).ToString();
    //              }

    //          }
    //          else
    //          {
    //              //rfvResidualValue.Enabled = true;
    //              //txtResidualAmt_Cashflow.ReadOnly = false;
    //          }
    //          //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 end
    //          txtResidualValue_Cashflow.Focus();

    //      }
    //      catch (Exception ex)
    //      {
    //          cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
    //          cv_TabMainPage.IsValid = false;
    //      }
    //  }
    //protected void txtResidualAmt_Cashflow_TextChanged(object sender, EventArgs e)
    //  {
    //      try
    //      {
    //          //FunPriToggleResidualAmountBased();
    //          //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 start
    //          if (txtResidualAmt_Cashflow.Text.Trim() != "")
    //          {
    //              if (Convert.ToDecimal(txtResidualAmt_Cashflow.Text.Trim()) >
    //                  Convert.ToDecimal(txtFinanceAmount.Text.Trim())
    //                  && (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL")))
    //              {
    //                  Utility.FunShowAlertMsg(this, "Residual amount should be less than or equal to Finance amount");
    //                  txtResidualAmt_Cashflow.Text = "";
    //                  txtResidualValue_Cashflow.Text = "";
    //                  txtResidualAmt_Cashflow.Focus();
    //              }
    //              else
    //              {
    //                  rfvResidualValue.Enabled = false;
    //                  txtResidualValue_Cashflow.ReadOnly = true;
    //                  txtResidualValue_Cashflow.Text = "";
    //                  //txtResidualValue.Text = txtResidualAmt_Cashflow.Text;
    //              }
    //          }
    //          else
    //          {
    //              txtResidualAmt_Cashflow.Text = "";
    //              rfvResidualValue.Enabled = true;
    //              txtResidualValue_Cashflow.ReadOnly = false;
    //          }
    //          //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 end

    //      }
    //      catch (Exception ex)
    //      {
    //          cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
    //          cv_TabMainPage.IsValid = false;
    //      }
    //  }


    protected void txtAccountDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DtCashFlow"] != null)
            {
                System.Data.DataTable DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
                if (DtCashFlow.Rows.Count > 0)
                {
                    DataRow[] drUMFC = null;
                    if (DtCashFlow.Columns.Contains("CashFlow_ID"))
                    {
                        drUMFC = DtCashFlow.Select("CashFlow_ID = 34");

                    }
                    else
                    {
                        drUMFC = DtCashFlow.Select("CashFlow_Flag_ID = 34");
                    }
                    if (drUMFC.Length > 0)
                    {
                        drUMFC[0].Delete();
                        DtCashFlow.AcceptChanges();
                        ViewState["DtCashFlow"] = DtCashFlow;
                        if (DtCashFlow.Rows.Count > 0)
                        {
                            FunPriBindInflowDLL(_Edit);
                        }
                        else
                        {
                            FunPriBindInflowDLL(_Add);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }


    private void FunPriLoadAccountDetails(int intAccountCreationId)
    {
        DataSet dsApplicationDetails = new DataSet();
        //S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        ////Code Added by Ganapathy on 08/06/2012
        //OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        //END
        try
        {

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CompanyId", intCompanyId.ToString());
            Procparam.Add("@AccountCreationId", intAccountCreationId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            //Added by Sathiyanathan on 24Jun2015 for OPC starts here
            Procparam.Add("@Mode", Convert.ToString(strMode));
            Procparam.Add("@SessionID", Convert.ToString(Request.Cookies["CookSession_ID"].Value));
            //Added by Sathiyanathan on 24Jun2015 for OPC ends here
            dsApplicationDetails = Utility.GetDataset("S3G_LOANAD_GetRSdtls_ACC", Procparam);

            if (dsApplicationDetails != null)
            {
                //if (dsApplicationDetails.Tables[0].Rows.Count == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Activate the Account for selected Application Reference No')", true);
                //    return;
                //}
                ListItem lstItem;
                System.Data.DataTable dtHeader = new System.Data.DataTable();
                System.Data.DataTable dtPASAdtls = new System.Data.DataTable();


                dtHeader = dsApplicationDetails.Tables[0].Copy();
                dtPASAdtls = dsApplicationDetails.Tables[14].Copy();

                //delivery Customer Address
                if (dsApplicationDetails.Tables[16].Rows.Count > 0)
                    ViewState["DeliveryAddress"] = dsApplicationDetails.Tables[16].Copy();

                #region MainPage Tab


                ddlLOB.SelectedValue = Convert.ToString(dtHeader.Rows[0]["LOB_Id"]);
                ddlBranchList.SelectedText = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Name"]);
                ddlBranchList.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Id"]);
                //opc102 start

                //Utility.FillDataTable(ddlRentalTDSSec, dsApplicationDetails.Tables[19], "Tax_Law_Section", "Tax_Law_Section");
                String strTDSSection = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Rental_TDS_Sec"]);
                if (strTDSSection != "")
                {
                    ddlRentalTDSSec.Items.Insert(0, new ListItem(strTDSSection, strTDSSection));
                }
                //opc102 end

                System.Web.HttpContext.Current.Session["LocationAutoSuggestValue"] = ddlBranchList.SelectedValue;
                ddlBranchList.ToolTip = Convert.ToString(dtHeader.Rows[0]["Location_Name"]);
                FunPriLoadStatusDDL();
                if (dtHeader.Columns.Contains("PA_Status_Code"))
                {
                    if (Convert.ToString(dtHeader.Rows[0]["PA_Status_Code"]) != "")
                    {
                        ddlStatus.SelectedValue = Convert.ToString(dtHeader.Rows[0]["PA_Status_Code"]);
                        txtStatus.Text = ddlStatus.SelectedItem.Text;
                        if (ddlStatus.SelectedValue == "11")
                        {
                            txtStatus.Text = ddlStatus.SelectedItem.Text;
                            //tcAccountCreation.Tabs[7].Visible = true;
                        }
                    }
                }

                //lstItem = new ListItem(dtHeader.Rows[0]["Pricing_No"].ToString(), dtHeader.Rows[0]["Pricing_ID"].ToString());
                //ddlApplicationReferenceNo.Items.Add(lstItem);
                ddlApplicationReferenceNo.SelectedText = dtHeader.Rows[0]["Pricing_No"].ToString();
                ddlApplicationReferenceNo.SelectedValue = Convert.ToString(dtHeader.Rows[0]["Pricing_ID"]);
                txtAccountDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["AccountDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtfirstinstdate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["FirstInst_StartDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtFirstInstallDue.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["firstinstdate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtCommenceDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["Commencement_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtRSSignDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["Signon_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                ViewState["Customer_Id"] = hdnCustomerId.Value = Convert.ToString(dtHeader.Rows[0]["Customer_Id"]);
                System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = Convert.ToString(dtHeader.Rows[0]["Customer_Id"]); //By Siva.K For Data Load Problem in Multi User
                System.Data.DataTable dtcustomerAddress = dsApplicationDetails.Tables[15].Copy();

                if (dtcustomerAddress.Rows.Count > 0)
                {
                    TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    txtName.Text = txtCustomerCode.Text = dtcustomerAddress.Rows[0]["Customer_Code"].ToString();
                    S3GCustomerAddress1.SetCustomerDetails(dtcustomerAddress.Rows[0]["Customer_Code"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_Address1"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_Address2"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_city"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_state"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_country"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_pincode"].ToString(),
                    dtcustomerAddress.Rows[0]["Customer_Name"].ToString(),
                    dtcustomerAddress.Rows[0]["Comm_Telephone"].ToString(),
                    dtcustomerAddress.Rows[0]["Comm_mobile"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_email"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_website"].ToString());

                    Session["AccountAssetCustomer"] = hdnCustomerId.Value + ";" + S3GCustomerAddress1.CustomerName;
                }

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["PANum"].ToString()))
                    txtPrimeAccountNo.Text = Convert.ToString(dtHeader.Rows[0]["PANum"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["MRA_No"].ToString()))
                    txtMRANo.Text = Convert.ToString(dtHeader.Rows[0]["MRA_No"]);
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["MRA_Effective_Date"].ToString()))
                    txtMRADate.Text = Convert.ToString(dtHeader.Rows[0]["MRA_Effective_Date"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Delivery_State"].ToString()))
                    ddlDeliveryState.SelectedValue = Convert.ToString(dtHeader.Rows[0]["Delivery_State"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Billing_State"].ToString()))
                    ddlBillingState.SelectedValue = Convert.ToString(dtHeader.Rows[0]["Billing_State"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Finance_Amount"].ToString()))
                    txtFinanceAmount.Text = Math.Round(Convert.ToDecimal(dtHeader.Rows[0]["Finance_Amount"]), 0).ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["AutoExtnRent"].ToString()))
                {
                    if (Convert.ToDecimal(dtHeader.Rows[0]["AutoExtnRent"].ToString()) > 0)
                        TxtAutoExtnRental.Text = Math.Round(Convert.ToDecimal(dtHeader.Rows[0]["AutoExtnRent"]), 0).ToString();
                }

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["LeadRefNo"].ToString()))
                    txtLeadRefNo.Text = dtHeader.Rows[0]["LeadRefNo"].ToString();

                //txtFinanceAmount.Text = Convert.ToString(dtHeader.Rows[0]["Finance_Amount"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Acc_Mngr1_Name"].ToString()))
                    ddlAccountManager1.SelectedText = dtHeader.Rows[0]["Acc_Mngr1_Name"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Acc_Mngr1"].ToString()))
                    ddlAccountManager1.SelectedValue = dtHeader.Rows[0]["Acc_Mngr1"].ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Acc_Mngr2_Name"].ToString()))
                    ddlAccountManager2.SelectedText = dtHeader.Rows[0]["Acc_Mngr2_Name"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Acc_Mngr2"].ToString()))
                    ddlAccountManager2.SelectedValue = dtHeader.Rows[0]["Acc_Mngr2"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Sales_Person_Name"].ToString()))
                    ddlRegionalManager.SelectedText = dtHeader.Rows[0]["Sales_Person_Name"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Sales_Person_ID"].ToString()))
                    ddlRegionalManager.SelectedValue = dtHeader.Rows[0]["Sales_Person_ID"].ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Offer_Residual_Value_Amount"].ToString()))
                    txtResidualAmt_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Residual_Value_Amount"]);
                //txtResidualAmt_Cashflow.Text = (txtResidualAmt_Cashflow.Text == "0") ? "" : txtResidualAmt_Cashflow.Text;
                //txtResidualValue_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Residual_Value"]);
                //txtResidualValue_Cashflow.Text = (txtResidualValue_Cashflow.Text.StartsWith("0")) ? "" : txtResidualValue_Cashflow.Text;

                hdnProductId.Value = "0";
                txtConstitutionCode.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Constitution"]);
                hdnConstitutionId.Value = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Constitution_ID"]);

                /*Added by vinodha m for the call id 3663 on may 23,2016*/

                if (dsApplicationDetails.Tables[0].Rows[0]["Is_SFP_Applicable"].ToString() != String.Empty)
                    chkApplicable.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[0].Rows[0]["Is_SFP_Applicable"]);

                /* Added by Chandru K for the call id 4154 and 4203 on 11 July,2016 */

                if (dsApplicationDetails.Tables[0].Rows[0]["Is_SFP_Applicable_Sec"].ToString() != String.Empty)
                    chkApplicableSec.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[0].Rows[0]["Is_SFP_Applicable_Sec"]);

                if (dsApplicationDetails.Tables[0].Rows[0]["Is_FullRental"].ToString() != String.Empty)
                    chkFullRental.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[0].Rows[0]["Is_FullRental"]);

                //5093 start
                //if (dsApplicationDetails.Tables[0].Rows[0]["CST_Deal"].ToString() != String.Empty)
                //    chkCSTDeal.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[0].Rows[0]["CST_Deal"]);


                FunPrisalesControls(Convert.ToInt32(dtHeader.Rows[0]["RS_type"].ToString()),
                    dsApplicationDetails.Tables[0].Rows[0]["CST_Deal"].ToString(),
                    dsApplicationDetails.Tables[0].Rows[0]["vat_lease"].ToString());
                ViewState["Parent_Location_ID"] = dtHeader.Rows[0]["Parent_Location_ID"].ToString();
                
                /*if (ViewState["Parent_Location_ID"] != null)
                {
                    string strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();
                    //uncommented by Gomathi for VAT Leasing start
                    if (strParent_Location_ID == ddlDeliveryState.SelectedValue)
                    {
                        ddlSalesTax.Text = "VAT";
                        //ddlSalesTax.Enabled = false;
                        ViewState["intsales_type"] = "1";
                        chkcstwith.Enabled = false;
                        chkcstwith.Checked = false;

                        chkCSTDeal.Checked = false;
                        chkCSTDeal.Enabled = true;
                    }
                    else
                    {
                        ddlSalesTax.Text = "CST";
                        //ddlSalesTax.Enabled = false;
                        ViewState["intsales_type"] = "2";
                        chkcstwith.Enabled = true;
                        chkcstwith.Checked = false;
                        chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                    }



                }
                if (Convert.ToInt32(dsApplicationDetails.Tables[0].Rows[0]["CST_Deal"].ToString()) == 1)
                {
                    ddlSalesTax.Text = "CST(CST Leasing)";
                    //ddlSalesTax.Enabled = false;
                    ViewState["intsales_type"] = "2";
                    chkcstwith.Enabled = true;
                    chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                    ViewState["cstleasing"] = "1";
                }
                if (Convert.ToInt32(dsApplicationDetails.Tables[0].Rows[0]["vat_lease"].ToString()) == 1)
                {
                    ddlSalesTax.Text = "VAT(VAT Leasing)";
                    //ddlSalesTax.Enabled = false;
                    ViewState["intsales_type"] = "1";
                    chkcstwith.Enabled = false;
                    chkcstwith.Checked = false;
                    chkCSTDeal.Enabled = true;
                    chkCSTDeal.Checked = false;
                    ViewState["vatleasing"] = "1";

                }




                //5093 end
                */
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Billing_Address"].ToString()))
                    ddlDeliveryType.SelectedValue = dtHeader.Rows[0]["Billing_Address"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Cust_Address_ID"].ToString()))
                {
                    if (dtHeader.Rows[0]["Billing_Address"].ToString() == "2")
                    {
                        if (ViewState["DeliveryAddress"] != null)
                        {
                            DataTable dtDeliveryAddress = (DataTable)ViewState["DeliveryAddress"];
                            DataRow[] dtDel = dtDeliveryAddress.Select("Address_Type=2");
                            if (dtDel.Length > 0)
                            {
                                Utility.FillDataTable(ddlCust_Address, dtDel.CopyToDataTable(), "State", "State_Name");
                            }
                            ddlCust_Address.SelectedValue = dtHeader.Rows[0]["Cust_Address_ID"].ToString();
                        }
                    }
                }
                funclearDeliveryAddress();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Address1"].ToString()))
                    txtAddress1DA.Text = dtHeader.Rows[0]["Address1"].ToString();
                //if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Address2"].ToString()))
                //    txtAddress2DA.Text = dtHeader.Rows[0]["Address2"].ToString();
                //if (!string.IsNullOrEmpty(dtHeader.Rows[0]["City"].ToString()))
                //    txtCityDA.Text = dtHeader.Rows[0]["City"].ToString();
                //if (!string.IsNullOrEmpty(dtHeader.Rows[0]["State"].ToString()))
                //    ddlState.SelectedValue = dtHeader.Rows[0]["State"].ToString();
                //if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Country"].ToString()))
                //    txtCountryDA.Text = dtHeader.Rows[0]["Country"].ToString();
                //if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Pincode"].ToString()))
                //    txtPinCodeDA.Text = dtHeader.Rows[0]["Pincode"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Telephone"].ToString()))
                    txtTelephoneDA.Text = dtHeader.Rows[0]["Telephone"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Mobile"].ToString()))
                    txtMobileDA.Text = dtHeader.Rows[0]["Mobile"].ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Pin"].ToString()))
                    txtPin.Text = dtHeader.Rows[0]["Pin"].ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["GSTIN"].ToString()))
                    txtGSTIN.Text = dtHeader.Rows[0]["GSTIN"].ToString();

                txtLabel.Text = dtHeader.Rows[0]["Label"].ToString();
                txtAddress.Text = dtHeader.Rows[0]["Address"].ToString();

                //To be check
                //if (!string.IsNullOrEmpty(dtHeader.Rows[0]["RS_type"].ToString()))
                //    ddlSalesTax.Text = dtHeader.Rows[0]["RS_type"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Cform_Number"].ToString()))
                    txtCFormNo.Text = dtHeader.Rows[0]["Cform_Number"].ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["SEZ_Zone"].ToString()))
                    ddlSEZZone.SelectedValue = dtHeader.Rows[0]["SEZ_Zone"].ToString();
                if (dtHeader.Rows[0]["SEZA1"].ToString() == "1")
                {
                    chkSEZA1.Enabled = chkSEZA1.Checked = true;

                }

                if (dtHeader.Rows[0]["WithIGST"].ToString() == "1")
                {
                    chkWithIGST.Enabled = chkWithIGST.Checked = true;
                }

                if (dtHeader.Rows[0]["ITC_Not_Rental"].ToString() == "1")
                    chkITC.Checked = true;
                if (dtHeader.Rows[0]["ITC_Not_Cap"].ToString() == "1")
                    chkITC_Cap.Checked = true;
                if (dtHeader.Rows[0]["AMF_sold"].ToString() == "1")
                    chkAmfsold.Checked = true;
                if (dtHeader.Rows[0]["VAT_Sold"].ToString() == "1")
                    chkVATSold.Checked = true;
                if (dtHeader.Rows[0]["ServiceTax_Sold"].ToString() == "1")
                    chkServiceTaxSold.Checked = true;




                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Payment_Rule_ID"].ToString()))
                {
                    ListItem lstItem1;
                    lstItem1 = new ListItem(dtHeader.Rows[0]["Payment_Rule_No"].ToString(), dtHeader.Rows[0]["Payment_Rule_ID"].ToString());
                    ddlPaymentRuleList.Items.Add(lstItem1);
                    ddlPaymentRuleList.SelectedValue = dtHeader.Rows[0]["Payment_Rule_ID"].ToString();
                    Load_Payment_Rule();

                }
                else
                {
                    System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                    ddlPaymentRuleList.Items.Insert(0, liSelect1);
                }

                #region "Proposal details

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Pricing_No"].ToString()))
                    txtProposalNumber.Text = dtHeader.Rows[0]["Pricing_No"].ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Offer_Date"].ToString()))
                    txtOfferDate.Text = dtHeader.Rows[0]["Offer_Date"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Facility_Amount"].ToString()))
                    txtTotalFacilityAmount.Text = dtHeader.Rows[0]["Facility_Amount"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Offer_Valid_Till"].ToString()))
                    txtOfferValidTill.Text = dtHeader.Rows[0]["Offer_Valid_Till"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Proposal_Type"].ToString()))
                    ddlProposalType.SelectedValue = dtHeader.Rows[0]["Proposal_Type"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Adv_Rent_Sec_Dep"].ToString()))
                    RBLAdvanceRent.SelectedValue = dtHeader.Rows[0]["Adv_Rent_Sec_Dep"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Seco_Term_Applicability"].ToString()))
                    RBLSecondaryTerm.SelectedValue = dtHeader.Rows[0]["Seco_Term_Applicability"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Secu_Deposit_Type"].ToString()))
                    ddlSecuritydeposit.SelectedValue = dtHeader.Rows[0]["Secu_Deposit_Type"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["AR_SD_Amount"].ToString()))
                    txtSecDepAdvRent.Text = dtHeader.Rows[0]["AR_SD_Amount"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["ReturnPattern"].ToString()))
                    ddlReturnPattern.SelectedValue = dtHeader.Rows[0]["ReturnPattern"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["One_Time_Fee"].ToString()))
                    txtOneTimeFee.Text = dtHeader.Rows[0]["One_Time_Fee"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Processing_Fee_Per"].ToString()))
                    txtProcessingFee.Text = dtHeader.Rows[0]["Processing_Fee_Per"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Repayment_Mode"].ToString()))
                    RBLStructuredEI.SelectedValue = dtHeader.Rows[0]["Repayment_Mode"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["VAT_Rebate_Applicability"].ToString()))
                    RBLVATRebate.SelectedValue = dtHeader.Rows[0]["VAT_Rebate_Applicability"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Remarks"].ToString()))
                    txtRemarks.Text = dtHeader.Rows[0]["Remarks"].ToString();

                #endregion

                //PASA start
                ddlTenureType.ClearDropDownList();
                if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Tenure"].ToString()))
                    txtTenure.Text = Convert.ToString(dtPASAdtls.Rows[0]["Tenure"]);
                if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Tenure_Code"].ToString()))
                {
                    //ObjStatus.Option = 1;
                    //ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
                    //Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                    ddlTenureType.SelectedValue = Convert.ToString(dtPASAdtls.Rows[0]["Tenure_Code"]);
                    //ddlTenureType.ClearDropDownList();
                }
                if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Business_IRR"].ToString()))
                    txtBusinessIRR.Text = dtPASAdtls.Rows[0]["Business_IRR"].ToString();
                if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Company_IRR"].ToString()))
                    txtCompanyIRR.Text = dtPASAdtls.Rows[0]["Company_IRR"].ToString();

                hdnRentfrequency.Value = dtPASAdtls.Rows[0]["ROI_Rule_ID"].ToString();
                hdnTimeValue.Value = dtPASAdtls.Rows[0]["Repayment_time_code"].ToString();
                string strTimeval = string.Empty;
                switch (hdnTimeValue.Value.ToLower())
                {
                    case "adv(advance)":
                    case "adf(advance fbd)":
                        strTimeval = "advance";

                        break;
                    case "arr(arrears)":
                    case "arf(arrears fbd)":
                        strTimeval = "arrears";
                        break;
                    default:
                        strTimeval = "advance";
                        break;
                }
                // txtRepaymentTime.Text = strTimeval;

                ddlStatus.SelectedValue = Convert.ToString(dtPASAdtls.Rows[0]["SA_Status_Code"]);
                txtStatus.Text = ddlStatus.SelectedItem.Text;
                //txtAdvanceInstallments.Text = Convert.ToString(dtPASAdtls.Rows[0]["Advance_Installments"]);
                //txtAdvanceInstallments.Text = (txtAdvanceInstallments.Text == "0") ? "" : txtAdvanceInstallments.Text;
                ddlRepaymentMode.SelectedValue = Convert.ToString(dtPASAdtls.Rows[0]["Repayment_Code"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtPASAdtls.Rows[0]["Last_ODI_Date"])))
                //{
                //    txtLastODICalcDate.Text = DateTime.Parse(Convert.ToString(dtPASAdtls.Rows[0]["Last_ODI_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                //}
                //if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Is_delivery_Order_Require"].ToString()))
                //{
                //    chkDORequired.Checked = Convert.ToBoolean(dtPASAdtls.Rows[0]["Is_delivery_Order_Require"]);
                //}
                //PASA end


                #region "Constitutional Documents"
                grvConsDocuments.DataSource = dsApplicationDetails.Tables[9];
                grvConsDocuments.DataBind();

                #endregion

                #endregion

                #region OfferTerms Tab

                FunPriBindInflowDLL(_Add);
                FunPriBindOutflowDLL(_Add);
                FunPriBindRepaymentDLL(_Add);

                ViewState["DtCashFlow"] = dsApplicationDetails.Tables[3];
                if (dsApplicationDetails.Tables[3].Rows.Count > 0)
                {
                    if (intAccountCreationId == 0)
                    {
                        FunPriBindInflowDLL(_Add);
                    }
                    else
                    {
                        FunPriBindInflowDLL(_Edit);
                    }
                    gvInflow.DataSource = dsApplicationDetails.Tables[3];
                    gvInflow.DataBind();
                    FunPriGenerateNewInflowRow();
                    ViewState["DtCashFlow"] = dsApplicationDetails.Tables[3];
                }
                else
                {
                    FunPriBindInflowDLL(_Add);
                }

                ViewState["DtCashFlowOut"] = dsApplicationDetails.Tables[4];
                if (dsApplicationDetails.Tables[4].Rows.Count > 0)
                {
                    if (intAccountCreationId == 0)
                    {
                        FunPriBindOutflowDLL(_Add);
                    }
                    else
                    {
                        FunPriBindOutflowDLL(_Edit);
                    }
                    gvOutFlow.DataSource = dsApplicationDetails.Tables[4];
                    gvOutFlow.DataBind();
                    FunPriGenerateNewOutflowRow();
                    lblTotalOutFlowAmount.Text = dsApplicationDetails.Tables[4].Compute("sum(Amount)", "CashOutFlowID > 0").ToString();
                }
                else
                {
                    FunPriBindOutflowDLL(_Add);
                }

                System.Data.DataTable dt = dsApplicationDetails.Tables[1].Copy();
                if (dt.Rows.Count > 0)
                {
                    ViewState["dtPrimaryGrid"] = dt;
                    DataRow[] drprim = dt.Select("Offer_Type=1");
                    DataRow[] drApp = dt.Select("RentFrequencyID In(6,7,8)");
                    if (drprim.Length > 0)
                    {
                        dtPrimaryGrid = drprim.CopyToDataTable();
                        FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                        if (drApp.Length > 0)
                            chkApplicable.Enabled = true;
                    }
                    DataRow[] drsec = dt.Select("Offer_Type=2");
                    if (drsec.Length > 0)
                    {
                        dtSecondaryGrid = drsec.CopyToDataTable();
                        FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                        if (drApp.Length > 0)
                            chkApplicableSec.Enabled = true;

                    }

                    FunAddremoveAssetCategoryDDL();

                }

                if (dsApplicationDetails.Tables[2].Rows.Count > 0)
                {
                    ViewState["dtEUCDetails"] = dtEUCDetails = dsApplicationDetails.Tables[2].Copy();
                    FunFillgrid(grvEUC, dtEUCDetails);
                    //For end use Asset category

                }
                else
                {
                    FunPriSetEmptyEUCtbl();
                }

                if (dsApplicationDetails.Tables[17].Rows.Count > 0)
                {
                    ViewState["dtRentalDetails"] = dtRentalDetails = dsApplicationDetails.Tables[17].Copy();
                    FunFillgrid(grvRentalDetails, dtRentalDetails);
                }
                else
                {
                    FunPriEmptyRentalDetails();
                }

                if (dsApplicationDetails.Tables[18].Rows.Count > 0)
                {
                    ViewState["dtInvoicesACATSummary"] = dtInvoicesACATSummary = dsApplicationDetails.Tables[18].Copy();

                }

                #region "Purchase Tax Part"
                System.Data.DataTable dtTaxDetails = dsApplicationDetails.Tables[8].Copy();
                foreach (DataRow drrow in dtTaxDetails.Rows)
                {
                    switch (drrow["Tax_Type_ID"].ToString())
                    {
                        case "5"://LBT
                            chkLBT.Checked = true;
                            break;
                        case "1"://Purchase Tax
                            chkPT.Checked = true;
                            break;
                        case "2"://Reverse Charge Tax
                            chkRC.Checked = true;
                            break;
                        case "3"://Entry Tax
                            chkET.Checked = true;
                            break;
                        case "6"://GST Reverse Charge Tax
                            chkbxGSTRC.Checked = true;
                            break;
                    }
                }

                #endregion

                #endregion
                FunPriBindGridSummary();
                FunPriGetMappedAmount();
                FunPriSetProposalType();
                if (RBLSecondaryTerm.SelectedValue == "1")
                {
                    if (dtHeader.Rows[0]["IsSep_Amort"].ToString() == "1")
                        chkIsSecondary.Checked = true;

                    if (strMode == "M")
                    {
                        chkIsSecondary.Enabled = true;
                        FunPriIsSecondaryChanges();
                    }
                    else
                        chkIsSecondary.Enabled = false;
                }
                else
                    chkIsSecondary.Checked = false;
                #region Repayment Tab

                DtRepayGrid = dsApplicationDetails.Tables[5].Copy();

                if (DtRepayGrid.Rows.Count > 0)
                {
                    _SlNo = 0;
                    ViewState["DtRepayGrid"] = DtRepayGrid;

                    foreach (DataRow dr in DtRepayGrid.Rows)
                    {
                        dr["Amount"] = Convert.ToDecimal(Convert.ToDecimal(dr["Amount"]).ToString(Funsetsuffix()));
                        dr["PerInstall"] = Convert.ToDecimal(Convert.ToDecimal(dr["PerInstall"]).ToString(Funsetsuffix()));
                        DtRepayGrid.AcceptChanges();
                    }

                    gvRepaymentDetails.DataSource = DtRepayGrid;
                    gvRepaymentDetails.DataBind();
                    FunPriGenerateNewRepaymentRow();
                    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
                    //if (intApplicationId > 0)
                    //{
                    txtAccountIRR_Repay.Text = txtAccountingIRR.Text = dtPASAdtls.Rows[0]["Accounting_IRR"].ToString();
                    txtBusinessIRR_Repay.Text = txtBusinessIRR.Text = dtPASAdtls.Rows[0]["Business_IRR"].ToString();
                    txtCompanyIRR_Repay.Text = txtCompanyIRR.Text = dtPASAdtls.Rows[0]["Company_IRR"].ToString();
                    Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
                    if (lblCashFlowId.Text != "23" && lblCashFlowId.Text != "105")
                    {
                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                    }
                    else
                    {
                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = false;
                    }
                    //FunPriCalculateSummary(DtRepayGrid, "cashflow", "TotalPeriodInstall");



                    //FunPriShowRepaymetDetails((decimal)DtRepayGrid.Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));

                }

                #endregion


                #region Guarantor Tab


                System.Data.DataTable dtGuarantorGrid = dsApplicationDetails.Tables[11].Copy();
                if (dtGuarantorGrid.Rows.Count > 0)
                {
                    ViewState["dtGuarantorGrid"] = dtGuarantorGrid;
                    if (intAccountCreationId == 0)
                    {
                        FunPriBindGuarantorDLL(_Add);
                    }
                    else
                    {
                        FunPriBindGuarantorDLL(_Edit);
                    }
                    gvGuarantor.DataSource = dtGuarantorGrid;
                    gvGuarantor.DataBind();
                    ViewState["dtGuarantorGrid"] = dtGuarantorGrid;
                    ViewState["GuarantorDetails"] = dtGuarantorGrid;
                    FunPriGenerateNewGuarantorRow();
                }
                else
                {
                    FunPriBindGuarantorDLL(_Add);
                }

                #endregion

                //#region Alerts Tab
                //DtAlertDetails = dsApplicationDetails.Tables[10].Copy();
                //if (DtAlertDetails.Rows.Count == 0)
                //{
                //    FunPriBindAlertDLL(_Add);
                //}
                //else
                //{
                //    ViewState["DtAlertDetails"] = DtAlertDetails;
                //    if (intAccountCreationId == 0)
                //    {

                //        FunPriBindAlertDLL(_Add);
                //    }
                //    else
                //    {
                //        FunPriBindAlertDLL(_Edit);
                //    }
                //    //gvAlert.DataSource = DtAlertDetails;
                //    //gvAlert.DataBind();
                //    //FunPriGenerateNewAlertRow();
                //    ViewState["DtAlertDetails"] = DtAlertDetails;
                //}
                //#endregion

                //#region Followup Tab

                //if (dsApplicationDetails.Tables[12].Rows.Count > 0)
                //{
                //    txtAccount_Followup.Text = txtPrimeAccountNo.Text;
                //    txtEnquiry_Followup.Text = dsApplicationDetails.Tables[12].Rows[0]["Enquiry_Number"].ToString();
                //    txtOfferNo_Followup.Text = dsApplicationDetails.Tables[12].Rows[0]["Offer_Number"].ToString();
                //    txtApplication_Followup.Text = dsApplicationDetails.Tables[12].Rows[0]["Application_Number"].ToString();
                //    //if (!string.IsNullOrEmpty(dsApplicationDetails.Tables[12].Rows[0]["Date"].ToString()))
                //    //    txtEnquiryDate_Followup.Text = Convert.ToDateTime(dsApplicationDetails.Tables[12].Rows[0]["Date"].ToString()).ToString(strDateFormat);
                //    txtCustNameAdd_Followup.Text = S3GCustomerAddress1.CustomerName + "," + "\n" + S3GCustomerAddress1.CustomerAddress;
                //}
                //if (dsApplicationDetails.Tables[13].Rows.Count == 0)
                //{
                //    FunPriBindFollowupDLL(_Add);
                //}
                //else
                //{
                //    ViewState["DtFollowUp"] = dsApplicationDetails.Tables[13];
                //    if (intAccountCreationId == 0)
                //    {
                //        FunPriBindFollowupDLL(_Add);
                //    }
                //    else
                //    {
                //        /*ADDED BY NATARAJ Y for issues in follow up loading by 27/6/2011*/
                //        if (dsApplicationDetails.Tables[13].Rows.Count > 0)
                //        {
                //            FunPriBindFollowupDLL(_Edit);
                //        }
                //        else
                //        {
                //            FunPriBindFollowupDLL(_Add);
                //        }
                //    }
                //    gvFollowUp.DataSource = dsApplicationDetails.Tables[13];
                //    gvFollowUp.DataBind();
                //    FunPriGenerateNewFollowupRow();
                //    ViewState["DtFollowUp"] = dsApplicationDetails.Tables[13];
                //}
                //#endregion


                //#region Invoice
                //if (dsApplicationDetails.Tables.Count > 15)
                //{
                //    if (dsApplicationDetails.Tables[15].Rows.Count > 0 && dsApplicationDetails.Tables[15].Rows[0].ItemArray[0].ToString() != "")
                //    {
                //        gvInvoiceDetails.DataSource = dsApplicationDetails.Tables[15];
                //        ViewState["InvoiceDetails"] = dsApplicationDetails.Tables[15];
                //        gvInvoiceDetails.DataBind();
                //    }
                //}
                //#endregion

                #region Repayment Structure
                System.Data.DataTable dtRepaystructure = dsApplicationDetails.Tables[6].Copy();

                if (dtRepaystructure.Rows.Count > 0 && dtRepaystructure.Rows[0].ItemArray[0].ToString() != "")
                {
                    ViewState["RepaymentStructure"] = dtRepaystructure;
                    foreach (DataRow dr in dtRepaystructure.Rows)
                    {
                        dr["InstallmentAmount"] = Convert.ToDecimal(Convert.ToDecimal(dr["InstallmentAmount"]).ToString());
                        dr["FinanceCharges"] = Convert.ToDecimal(Convert.ToDecimal(dr["FinanceCharges"]).ToString());
                        // dr["PrincipalAmount"] = Convert.ToDecimal(Convert.ToDecimal(dr["PrincipalAmount"]).ToString(Funsetsuffix()));
                        if (dr["Tax"].ToString() != "")
                            dr["Tax"] = Convert.ToDecimal(Convert.ToDecimal(dr["Tax"]).ToString());
                        if (dr["AMF"].ToString() != "")
                            dr["AMF"] = Convert.ToDecimal(Convert.ToDecimal(dr["AMF"]).ToString());
                        if (dr["ServiceTax"].ToString() != "")
                            dr["ServiceTax"] = Convert.ToDecimal(Convert.ToDecimal(dr["ServiceTax"]).ToString());
                        if (dr["Insurance"].ToString() != "")
                            dr["Insurance"] = Convert.ToDecimal(Convert.ToDecimal(dr["Insurance"]).ToString());
                        if (dr["Others"].ToString() != "")
                            dr["Others"] = Convert.ToDecimal(Convert.ToDecimal(dr["Others"]).ToString());
                        if (dr["AMF_Principal"].ToString() != "")
                            dr["AMF_Principal"] = Convert.ToDecimal(Convert.ToDecimal(dr["AMF_Principal"]).ToString());
                        if (dr["AMF_Charges"].ToString() != "")
                            dr["AMF_Charges"] = Convert.ToDecimal(Convert.ToDecimal(dr["AMF_Charges"]).ToString());
                        if (dr["Rebate_Discount"].ToString() != "")
                            dr["Rebate_Discount"] = Convert.ToDecimal(Convert.ToDecimal(dr["Rebate_Discount"]).ToString());
                        if (dr["Addi_Rebate_Discount"].ToString() != "")
                            dr["Addi_Rebate_Discount"] = Convert.ToDecimal(Convert.ToDecimal(dr["Addi_Rebate_Discount"]).ToString());
                        if (dr["Cess_Amount"].ToString() != "")
                            dr["Cess_Amount"] = Convert.ToDecimal(Convert.ToDecimal(dr["Cess_Amount"]).ToString());
                        dtRepaystructure.AcceptChanges();
                    }

                    grvRepayStructure.DataSource = dtRepaystructure;
                    grvRepayStructure.DataBind();


                    //opc042 start
                    if (dsApplicationDetails.Tables[0].Rows[0]["state_wise_billing"].ToString() == "1")
                    {
                        grvCustEmail.DataSource = dsApplicationDetails.Tables[20];
                        grvCustEmail.DataBind();
                    }
                    //opc042 end

                    DataTable dtRepaymentStructureSTCopy = dtRepaystructure.Copy();
                    if (dtRepaymentStructureSTCopy.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtRepaymentStructureSTCopy.Rows)
                        {
                            if (dr["InstallmentAmount"].ToString() != "")
                                dr["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dr["InstallmentAmount"].ToString()));
                            if (dr["Tax"].ToString() != "")
                                dr["Tax"] = Math.Round(Convert.ToDecimal(dr["Tax"].ToString()));
                            if (dr["ServiceTax"].ToString() != "")
                                dr["ServiceTax"] = Math.Round(Convert.ToDecimal(dr["ServiceTax"].ToString()));
                            if (dr["Insurance"].ToString() != "")
                                dr["Insurance"] = Math.Round(Convert.ToDecimal(dr["Insurance"].ToString()));
                            if (dr["Others"].ToString() != "")
                                dr["Others"] = Math.Round(Convert.ToDecimal(dr["Others"].ToString()));
                            if (dr["AMF"].ToString() != "")
                                dr["AMF"] = Math.Round(Convert.ToDecimal(dr["AMF"].ToString()));
                        }

                        ViewState["InstallmentAmount_Total"] = dtRepaymentStructureSTCopy.Compute("sum(InstallmentAmount)", "");
                        ViewState["AMF_Total"] = dtRepaymentStructureSTCopy.Compute("sum(AMF)", "");
                        ViewState["TAX_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Tax)", "");
                        ViewState["ServiceTax_Total"] = dtRepaymentStructureSTCopy.Compute("sum(ServiceTax)", "");
                        if (dtRepaymentStructureSTCopy.Rows[0]["Insurance"].ToString() != "") ;
                        ViewState["Insurance_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Insurance)", "");
                        if (dtRepaymentStructureSTCopy.Rows[0]["Others"].ToString() != "") ;
                        ViewState["Others_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Others)", "");
                    }
                }

                //DataTable DtRepayGridp = ((DataTable)ViewState["DtRepayGrid"]).Copy();
                //DataRow dr1;
                //dr1 = DtRepayGridp.NewRow();
                //dr1["slno"] = "1";
                //dr1["CashFlow"] = "Tax";
                //dr1["TotalPeriodInstall"] = ViewState["TAX_Total"];
                //DtRepayGridp.Rows.Add(dr1);

                //dr1 = DtRepayGridp.NewRow();
                //dr1["slno"] = "4";
                //dr1["CashFlow"] = "ServiceTax";
                //dr1["TotalPeriodInstall"] = ViewState["ServiceTax_Total"];
                //DtRepayGridp.Rows.Add(dr1);


                ////if (ViewState["Insurance_Total"].ToString() != "")
                ////{
                ////    dr1 = DtRepayGridp.NewRow();
                ////    dr1["slno"] = "5";
                ////    dr1["CashFlow"] = "Insurance";
                ////    dr1["TotalPeriodInstall"] = ViewState["Insurance_Total"];
                ////    DtRepayGridp.Rows.Add(dr1);
                ////}

                ////if (ViewState["Others_Total"].ToString() != "")
                ////{
                ////    dr1 = DtRepayGridp.NewRow();
                ////    dr1["slno"] = "6";
                ////    dr1["CashFlow"] = "Others";
                ////    dr1["TotalPeriodInstall"] = ViewState["Others_Total"];
                ////    DtRepayGridp.Rows.Add(dr1);
                ////}
                ////DataRow[] datainst = DtRepayGridp.Select("CashFlow_Flag_ID = 23", "");
                ////if (datainst.Length > 0)
                ////{
                ////    foreach (DataRow drrow in datainst)
                ////    {
                ////        drrow["TotalPeriodInstall"] = ViewState["InstallmentAmount_Total"];
                ////        drrow.AcceptChanges();
                ////    }
                ////}
                ////DataRow[] dataAMF = DtRepayGridp.Select("CashFlow_Flag_ID = 105", "");
                ////if (dataAMF.Length > 0)
                ////{
                ////    foreach (DataRow drrow in dataAMF)
                ////    {
                ////        drrow["TotalPeriodInstall"] = ViewState["AMF_Total"];
                ////        drrow.AcceptChanges();
                ////    }
                ////}
                //foreach (DataRow dr in DtRepayGridp.Rows)
                //{
                //    dr["TotalPeriodInstall"] = Convert.ToDecimal(Convert.ToDecimal(dr["TotalPeriodInstall"]).ToString(Funsetsuffix()));
                //}

                //DataView dv = DtRepayGridp.DefaultView;
                //dv.Sort = "slno asc";
                //DtRepayGridp = dv.ToTable();
                //ViewState["DtRepayGrid"] = DtRepayGrid;

                FunPriCalculateSummary(((DataTable)ViewState["DtRepayGrid"]), "cashflow", "TotalPeriodInstall");


                if (ViewState["InstallmentAmount_Total"] != null)
                    FunPriShowRepaymetDetails(Convert.ToDecimal(ViewState["InstallmentAmount_Total"]) + ((ViewState["AMF_Total"] != null) ? Convert.ToDecimal(ViewState["AMF_Total"]) : 0));
                else
                    FunPriShowRepaymetDetails((decimal)DtRepayGrid.Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID = 23"));

                #endregion

                #region "Prime Account tab"

                FunPriLoadRepaymentMode();
                //if (dsApplicationDetails.Tables[4].Rows.Count > 0)
                //{
                //    Load_Payment_Rule();
                //}
                //txtLOB_Followup.Text = ddlLOB.SelectedItem.Text;
                //txtBranch_Followup.Text = ddlBranchList.SelectedText;
                //txtApplication_Followup.Text = ddlApplicationReferenceNo.SelectedText;
                #endregion

                //if (intAccountCreationId == 0 && txtAccountDate.Text != DateTime.Now.Date.ToString(strDateFormat))
                //{
                //    txtBusinessIRR_Repay.Text = "";
                //}

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Reset_Amount"].ToString()))
                    txtAddInvAmt.Text = Convert.ToString(dtHeader.Rows[0]["Reset_Amount"]);

                if (dtHeader.Rows[0]["Lien_Account_PASA_ID"].ToString() != string.Empty)
                {
                    ddlLientContract.SelectedValue = Convert.ToString(dtHeader.Rows[0]["Lien_Account_PASA_ID"]);
                    ddlLientContract.SelectedText = Convert.ToString(dtHeader.Rows[0]["Lien_Account"]);
                }

                //5093 start

                //if (ViewState["Parent_Location_ID"] != null)
                //{
                //    if (ViewState["Parent_Location_ID"].ToString() == ddlDeliveryState.SelectedValue)
                //        chkCSTDeal.Enabled = true;
                //}

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            if (intAccountCreationId == 0)
            {
                throw new ApplicationException("Due to Data Problem, Unable to Load Application Details");
            }
            else
            {
                throw new ApplicationException("Due to Data Problem, Unable to Load Rental Schedule Details");
            }
        }
        //Code Added By Ganapathy on 08/06/2012 BEGIN
        finally
        {
            ObjCustomerService.Close();
        }
        //END
    }

    private void FunPriLoadAccountDetails_Split()
    {
        DataSet dsApplicationDetails = new DataSet();
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CompanyId", intCompanyId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Split_No", strSplitNum);
            //Added by Sathiyanathan on 24Jun2015 for OPC starts here
            Procparam.Add("@Mode", Convert.ToString(strMode));
            Procparam.Add("@SessionID", Convert.ToString(Request.Cookies["CookSession_ID"].Value));
            //Added by Sathiyanathan on 24Jun2015 for OPC ends here
           
            Procparam.Add("@SplitRefNo", strSplitRefNo);

            dsApplicationDetails = Utility.GetDataset("S3G_LOANAD_GetRSdtls_Split", Procparam);

            if (dsApplicationDetails != null)
            {

                ListItem lstItem;
                System.Data.DataTable dtHeader = new System.Data.DataTable();
                System.Data.DataTable dtPASAdtls = new System.Data.DataTable();


                dtHeader = dsApplicationDetails.Tables[0].Copy();
                dtPASAdtls = dsApplicationDetails.Tables[14].Copy();

                //delivery Customer Address
                if (dsApplicationDetails.Tables[16].Rows.Count > 0)
                    ViewState["DeliveryAddress"] = dsApplicationDetails.Tables[16].Copy();

                #region MainPage Tab

                ddlLOB.SelectedValue = Convert.ToString(dtHeader.Rows[0]["LOB_Id"]);
                ddlBranchList.SelectedText = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Name"]);
                ddlBranchList.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Id"]);
                ddlBranchList.ToolTip = Convert.ToString(dtHeader.Rows[0]["Location_Name"]);
                System.Web.HttpContext.Current.Session["LocationAutoSuggestValue"] = ddlBranchList.SelectedValue; //By Siva.K For Data Load Problem in Multi User
                //ddlBranchList.ReadOnly = true;
                FunPriLoadStatusDDL();
                if (dtHeader.Columns.Contains("PA_Status_Code"))
                {
                    if (Convert.ToString(dtHeader.Rows[0]["PA_Status_Code"]) != "")
                    {
                        ddlStatus.SelectedValue = Convert.ToString(dtHeader.Rows[0]["PA_Status_Code"]);
                        txtStatus.Text = ddlStatus.SelectedItem.Text;
                        if (ddlStatus.SelectedValue == "11")
                        {
                            txtStatus.Text = ddlStatus.SelectedItem.Text;
                            //tcAccountCreation.Tabs[7].Visible = true;
                        }
                    }
                }

                ViewState["strSplitDate"] = strSplitDate = Convert.ToString(dtHeader.Rows[0]["Split_Date"]);

                ddlApplicationReferenceNo.SelectedText = dtHeader.Rows[0]["Pricing_No"].ToString();
                ddlApplicationReferenceNo.SelectedValue = Convert.ToString(dtHeader.Rows[0]["Pricing_ID"]);
                ddlApplicationReferenceNo.ReadOnly = true;
                txtAccountDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["AccountDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtfirstinstdate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["FirstInst_StartDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtFirstInstallDue.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["firstinstdate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtCommenceDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["Commencement_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtRSSignDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["Signon_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                ViewState["Customer_Id"] = hdnCustomerId.Value = Convert.ToString(dtHeader.Rows[0]["Customer_Id"]);
                //By Siva.K For Data Load Problem in Multi User
                System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = Convert.ToString(dtHeader.Rows[0]["Customer_Id"]);
                CalendarFirstinstdate.Enabled = CalendarAccountDate.Enabled = CalendarFirstinstDuedate.Enabled = CalendarCommencedate.Enabled = CalendarSigndate.Enabled = false;
                System.Data.DataTable dtcustomerAddress = dsApplicationDetails.Tables[15].Copy();

                if (dtcustomerAddress.Rows.Count > 0)
                {
                    TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    txtName.Text = txtCustomerCode.Text = dtcustomerAddress.Rows[0]["Customer_Code"].ToString();
                    S3GCustomerAddress1.SetCustomerDetails(dtcustomerAddress.Rows[0]["Customer_Code"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_Address1"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_Address2"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_city"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_state"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_country"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_pincode"].ToString(),
                    dtcustomerAddress.Rows[0]["Customer_Name"].ToString(),
                    dtcustomerAddress.Rows[0]["Comm_Telephone"].ToString(),
                    dtcustomerAddress.Rows[0]["Comm_mobile"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_email"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_website"].ToString());

                    Session["AccountAssetCustomer"] = hdnCustomerId.Value + ";" + S3GCustomerAddress1.CustomerName;
                    ucCustomerCodeLov.ButtonEnabled = false;
                }

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["PANum"].ToString()))
                    txtPrimeAccountNo.Text = Convert.ToString(dtHeader.Rows[0]["PANum"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["MRA_No"].ToString()))
                    txtMRANo.Text = Convert.ToString(dtHeader.Rows[0]["MRA_No"]);
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["MRA_Effective_Date"].ToString()))
                    txtMRADate.Text = Convert.ToString(dtHeader.Rows[0]["MRA_Effective_Date"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Delivery_State"].ToString()))
                {
                    ddlDeliveryState.SelectedValue = Convert.ToString(dtHeader.Rows[0]["Delivery_State"]);
                    ddlDeliveryState.ClearDropDownList();
                }

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Billing_State"].ToString()))
                    ddlBillingState.SelectedValue = Convert.ToString(dtHeader.Rows[0]["Billing_State"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Finance_Amount"].ToString()))
                    txtFinanceAmount.Text = Math.Round(Convert.ToDecimal(dtHeader.Rows[0]["Finance_Amount"]), 0).ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["AutoExtnRent"].ToString()))
                    TxtAutoExtnRental.Text = Math.Round(Convert.ToDecimal(dtHeader.Rows[0]["AutoExtnRent"]), 0).ToString();

                //txtFinanceAmount.Text = Convert.ToString(dtHeader.Rows[0]["Finance_Amount"]);

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Acc_Mngr1_Name"].ToString()))
                    ddlAccountManager1.SelectedText = dtHeader.Rows[0]["Acc_Mngr1_Name"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Acc_Mngr1"].ToString()))
                    ddlAccountManager1.SelectedValue = dtHeader.Rows[0]["Acc_Mngr1"].ToString();
                ddlAccountManager1.ReadOnly = true;
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Acc_Mngr2_Name"].ToString()))
                    ddlAccountManager2.SelectedText = dtHeader.Rows[0]["Acc_Mngr2_Name"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Acc_Mngr2"].ToString()))
                    ddlAccountManager2.SelectedValue = dtHeader.Rows[0]["Acc_Mngr2"].ToString();
                ddlAccountManager2.ReadOnly = true;
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Sales_Person_Name"].ToString()))
                    ddlRegionalManager.SelectedText = dtHeader.Rows[0]["Sales_Person_Name"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Sales_Person_ID"].ToString()))
                    ddlRegionalManager.SelectedValue = dtHeader.Rows[0]["Sales_Person_ID"].ToString();
                ddlRegionalManager.ReadOnly = true;

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Offer_Residual_Value_Amount"].ToString()))
                    txtResidualAmt_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Residual_Value_Amount"]);

                hdnProductId.Value = "0";
                txtConstitutionCode.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Constitution"]);
                hdnConstitutionId.Value = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Constitution_ID"]);

                /* Added by Chandru K for the call id 4154 and 4203 on 11 July,2016 */
                if (dsApplicationDetails.Tables[0].Rows[0]["Is_SFP_Applicable"].ToString() != String.Empty)
                    chkApplicable.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[0].Rows[0]["Is_SFP_Applicable"]);

                if (dsApplicationDetails.Tables[0].Rows[0]["Is_SFP_Applicable_Sec"].ToString() != String.Empty)
                    chkApplicableSec.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[0].Rows[0]["Is_SFP_Applicable_Sec"]);

                if (dsApplicationDetails.Tables[0].Rows[0]["Is_FullRental"].ToString() != String.Empty)
                    chkFullRental.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[0].Rows[0]["Is_FullRental"]);

                //5093 start
                //if (dsApplicationDetails.Tables[0].Rows[0]["CST_Deal"].ToString() != String.Empty)
                //    chkCSTDeal.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[0].Rows[0]["CST_Deal"]);
                //5093 end

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Billing_Address"].ToString()))
                    ddlDeliveryType.SelectedValue = dtHeader.Rows[0]["Billing_Address"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Cust_Address_ID"].ToString()))
                {
                    if (dtHeader.Rows[0]["Cust_Address_ID"].ToString() != "0")
                    {
                        if (ViewState["DeliveryAddress"] != null)
                        {
                            DataTable dtDeliveryAddress = (DataTable)ViewState["DeliveryAddress"];
                            DataRow[] dtDel = dtDeliveryAddress.Select("Address_Type=2");
                            if (dtDel.Length > 0)
                            {
                                Utility.FillDataTable(ddlCust_Address, dtDel.CopyToDataTable(), "State", "State_Name");
                            }
                            ddlCust_Address.SelectedValue = dtHeader.Rows[0]["Cust_Address_ID"].ToString();
                        }
                    }
                }
                funclearDeliveryAddress();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Address1"].ToString()))
                    txtAddress1DA.Text = dtHeader.Rows[0]["Address1"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Telephone"].ToString()))
                    txtTelephoneDA.Text = dtHeader.Rows[0]["Telephone"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Mobile"].ToString()))
                    txtMobileDA.Text = dtHeader.Rows[0]["Mobile"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Pin"].ToString()))
                    txtPin.Text = dtHeader.Rows[0]["Pin"].ToString();
                FunPrisalesControls(Convert.ToInt32(dtHeader.Rows[0]["RS_type"].ToString()), 
                    dtHeader.Rows[0]["CST_Deal"].ToString(),
                    dtHeader.Rows[0]["vat_lease"].ToString());
                
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["GSTIN"].ToString()))
                    txtGSTIN.Text = dtHeader.Rows[0]["GSTIN"].ToString();

                txtLabel.Text = dtHeader.Rows[0]["Label"].ToString();
                txtAddress.Text = dtHeader.Rows[0]["Address"].ToString();

                //To be check
                //if (!string.IsNullOrEmpty(dtHeader.Rows[0]["RS_type"].ToString()))
                //    ddlSalesTax.Text = dtHeader.Rows[0]["RS_type"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Cform_Number"].ToString()))
                    txtCFormNo.Text = dtHeader.Rows[0]["Cform_Number"].ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["SEZ_Zone"].ToString()))
                    ddlSEZZone.SelectedValue = dtHeader.Rows[0]["SEZ_Zone"].ToString();

                if (dtHeader.Rows[0]["SEZA1"].ToString() == "1")
                {
                    chkSEZA1.Checked = true;
                }

                if (dtHeader.Rows[0]["ITC_Not_Rental"].ToString() == "1")
                    chkITC.Checked = true;
                if (dtHeader.Rows[0]["AMF_sold"].ToString() == "1")
                    chkAmfsold.Checked = true;
                if (dtHeader.Rows[0]["VAT_Sold"].ToString() == "1")
                    chkVATSold.Checked = true;
                if (dtHeader.Rows[0]["ServiceTax_Sold"].ToString() == "1")
                    chkServiceTaxSold.Checked = true;


                ViewState["Parent_Location_ID"] = dtHeader.Rows[0]["Parent_Location_ID"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Payment_Rule_ID"].ToString()))
                {
                    ListItem lstItem1;
                    lstItem1 = new ListItem(dtHeader.Rows[0]["Payment_Rule_No"].ToString(), dtHeader.Rows[0]["Payment_Rule_ID"].ToString());
                    ddlPaymentRuleList.Items.Add(lstItem1);
                    ddlPaymentRuleList.SelectedValue = dtHeader.Rows[0]["Payment_Rule_ID"].ToString();
                    Load_Payment_Rule();

                }
                else
                {
                    System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                    ddlPaymentRuleList.Items.Insert(0, liSelect1);
                }

                #region "Proposal details

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Pricing_No"].ToString()))
                    txtProposalNumber.Text = dtHeader.Rows[0]["Pricing_No"].ToString();

                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Offer_Date"].ToString()))
                    txtOfferDate.Text = dtHeader.Rows[0]["Offer_Date"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Facility_Amount"].ToString()))
                    txtTotalFacilityAmount.Text = dtHeader.Rows[0]["Facility_Amount"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Offer_Valid_Till"].ToString()))
                    txtOfferValidTill.Text = dtHeader.Rows[0]["Offer_Valid_Till"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Proposal_Type"].ToString()))
                    ddlProposalType.SelectedValue = dtHeader.Rows[0]["Proposal_Type"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Adv_Rent_Sec_Dep"].ToString()))
                    RBLAdvanceRent.SelectedValue = dtHeader.Rows[0]["Adv_Rent_Sec_Dep"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Seco_Term_Applicability"].ToString()))
                    RBLSecondaryTerm.SelectedValue = dtHeader.Rows[0]["Seco_Term_Applicability"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Secu_Deposit_Type"].ToString()))
                    ddlSecuritydeposit.SelectedValue = dtHeader.Rows[0]["Secu_Deposit_Type"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["AR_SD_Amount"].ToString()))
                    txtSecDepAdvRent.Text = dtHeader.Rows[0]["AR_SD_Amount"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["ReturnPattern"].ToString()))
                    ddlReturnPattern.SelectedValue = dtHeader.Rows[0]["ReturnPattern"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["One_Time_Fee"].ToString()))
                    txtOneTimeFee.Text = dtHeader.Rows[0]["One_Time_Fee"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Processing_Fee_Per"].ToString()))
                    txtProcessingFee.Text = dtHeader.Rows[0]["Processing_Fee_Per"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Repayment_Mode"].ToString()))
                    RBLStructuredEI.SelectedValue = dtHeader.Rows[0]["Repayment_Mode"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["VAT_Rebate_Applicability"].ToString()))
                    RBLVATRebate.SelectedValue = dtHeader.Rows[0]["VAT_Rebate_Applicability"].ToString();
                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Remarks"].ToString()))
                    txtRemarks.Text = dtHeader.Rows[0]["Remarks"].ToString();

                #endregion

                //PASA start
                ddlTenureType.ClearDropDownList();
                if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Tenure"].ToString()))
                    txtTenure.Text = Convert.ToString(dtPASAdtls.Rows[0]["Tenure"]);

                if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Tenure_Code"].ToString()))
                {
                    ddlTenureType.SelectedValue = Convert.ToString(dtPASAdtls.Rows[0]["Tenure_Code"]);
                }
                if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Business_IRR"].ToString()))
                    txtBusinessIRR.Text = dtPASAdtls.Rows[0]["Business_IRR"].ToString();
                if (!string.IsNullOrEmpty(dtPASAdtls.Rows[0]["Company_IRR"].ToString()))
                    txtCompanyIRR.Text = dtPASAdtls.Rows[0]["Company_IRR"].ToString();

                hdnRentfrequency.Value = dtPASAdtls.Rows[0]["ROI_Rule_ID"].ToString();
                hdnTimeValue.Value = dtPASAdtls.Rows[0]["Repayment_time_code"].ToString();
                string strTimeval = string.Empty;
                switch (hdnTimeValue.Value.ToLower())
                {
                    case "adv(advance)":
                    case "adf(advance fbd)":
                        strTimeval = "advance";

                        break;
                    case "arr(arrears)":
                    case "arf(arrears fbd)":
                        strTimeval = "arrears";
                        break;
                    default:
                        strTimeval = "advance";
                        break;
                }
                // txtRepaymentTime.Text = strTimeval;

                ddlStatus.SelectedValue = Convert.ToString(dtPASAdtls.Rows[0]["SA_Status_Code"]);
                txtStatus.Text = ddlStatus.SelectedItem.Text;

                ddlRepaymentMode.SelectedValue = Convert.ToString(dtPASAdtls.Rows[0]["Repayment_Code"]);

                #region "Constitutional Documents"
                grvConsDocuments.DataSource = dsApplicationDetails.Tables[9];
                grvConsDocuments.DataBind();

                #endregion

                #endregion


                #region OfferTerms Tab

                //FunPriBindInflowDLL(_Add);
                //FunPriBindOutflowDLL(_Add);
                FunPriBindRepaymentDLL(_Add);

                //ViewState["DtCashFlow"] = dsApplicationDetails.Tables[3];
                //if (dsApplicationDetails.Tables[3].Rows.Count > 0)
                //{
                //    if (intAccountCreationId == 0)
                //    {
                //        FunPriBindInflowDLL(_Add);
                //    }
                //    else
                //    {
                //        FunPriBindInflowDLL(_Edit);
                //    }
                //    gvInflow.DataSource = dsApplicationDetails.Tables[3];
                //    gvInflow.DataBind();
                //    FunPriGenerateNewInflowRow();
                //    ViewState["DtCashFlow"] = dsApplicationDetails.Tables[3];
                //}
                //else
                //{
                //    FunPriBindInflowDLL(_Add);
                //}

                //ViewState["DtCashFlowOut"] = dsApplicationDetails.Tables[4];
                //if (dsApplicationDetails.Tables[4].Rows.Count > 0)
                //{
                //    if (intAccountCreationId == 0)
                //    {
                //        FunPriBindOutflowDLL(_Add);
                //    }
                //    else
                //    {
                //        FunPriBindOutflowDLL(_Edit);
                //    }
                //    gvOutFlow.DataSource = dsApplicationDetails.Tables[4];
                //    gvOutFlow.DataBind();
                //    FunPriGenerateNewOutflowRow();
                //    lblTotalOutFlowAmount.Text = dsApplicationDetails.Tables[4].Compute("sum(Amount)", "CashOutFlowID > 0").ToString();
                //}
                //else
                //{
                //    FunPriBindOutflowDLL(_Add);
                //}

                System.Data.DataTable dt = dsApplicationDetails.Tables[1].Copy();
                if (dt.Rows.Count > 0)
                {
                    ViewState["dtPrimaryGrid"] = dt;
                    DataRow[] drprim = dt.Select("Offer_Type=1");
                    if (drprim.Length > 0)
                    {
                        dtPrimaryGrid = drprim.CopyToDataTable();
                        FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                    }
                    DataRow[] drsec = dt.Select("Offer_Type=2");
                    if (drsec.Length > 0)
                    {
                        dtSecondaryGrid = drsec.CopyToDataTable();
                        FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                    }
                    FunAddremoveAssetCategoryDDL();
                }
                grvPrimaryGrid.Enabled = grvSecondaryGrid.Enabled = false;
                if (RBLSecondaryTerm.SelectedValue == "1")
                    chkIsSecondary.Checked = true;
                else
                    chkIsSecondary.Checked = false;

                if (dsApplicationDetails.Tables[2].Rows.Count > 0)
                {
                    ViewState["dtEUCDetails"] = dtEUCDetails = dsApplicationDetails.Tables[2].Copy();
                    FunFillgrid(grvEUC, dtEUCDetails);
                    //For end use Asset category

                }
                else
                {
                    FunPriSetEmptyEUCtbl();
                }


                if (dsApplicationDetails.Tables[17].Rows.Count > 0)
                {
                    ViewState["dtRentalDetails"] = dtRentalDetails = dsApplicationDetails.Tables[17].Copy();
                    FunFillgrid(grvRentalDetails, dtRentalDetails);
                }
                else
                {
                    FunPriEmptyRentalDetails();
                }


                #region "Purchase Tax Part"
                System.Data.DataTable dtTaxDetails = dsApplicationDetails.Tables[8].Copy();
                foreach (DataRow drrow in dtTaxDetails.Rows)
                {
                    switch (drrow["Tax_Type_ID"].ToString())
                    {
                        case "5"://LBT
                            chkLBT.Checked = true;
                            break;
                        case "1"://Purchase Tax
                            chkPT.Checked = true;
                            break;
                        case "2"://Reverse Charge Tax
                            chkRC.Checked = true;
                            break;
                        case "3"://Entry Tax
                            chkET.Checked = true;
                            break;
                        case "6"://GST Reverse Charge Tax
                            chkbxGSTRC.Checked = true;
                            break;
                    }
                }

                #endregion

                #endregion
                FunPriBindGridSummary();
                FunPriGetMappedAmount();
                FunPriSetProposalType();


                #region Repayment Tab

                DtRepayGrid = dsApplicationDetails.Tables[5].Copy();


                if (DtRepayGrid.Rows.Count > 0)
                {
                    _SlNo = 0;
                    ViewState["DtRepayGrid"] = DtRepayGrid;
                    gvRepaymentDetails.DataSource = DtRepayGrid;
                    gvRepaymentDetails.DataBind();
                    FunPriGenerateNewRepaymentRow();
                    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
                    //if (intApplicationId > 0)
                    //{
                    txtAccountIRR_Repay.Text = txtAccountingIRR.Text = dtPASAdtls.Rows[0]["Accounting_IRR"].ToString();
                    txtBusinessIRR_Repay.Text = txtBusinessIRR.Text = dtPASAdtls.Rows[0]["Business_IRR"].ToString();
                    txtCompanyIRR_Repay.Text = txtCompanyIRR.Text = dtPASAdtls.Rows[0]["Company_IRR"].ToString();
                    Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
                    if (lblCashFlowId.Text != "23" && lblCashFlowId.Text != "105")
                    {
                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                    }
                    else
                    {
                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = false;
                    }
                    //FunPriCalculateSummary(DtRepayGrid, "cashflow", "TotalPeriodInstall");
                    //FunPriShowRepaymetDetails((decimal)DtRepayGrid.Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));

                }

                #endregion


                #region Guarantor Tab


                System.Data.DataTable dtGuarantorGrid = dsApplicationDetails.Tables[11].Copy();
                if (dtGuarantorGrid.Rows.Count > 0)
                {
                    ViewState["dtGuarantorGrid"] = dtGuarantorGrid;
                    if (intAccountCreationId == 0)
                    {
                        FunPriBindGuarantorDLL(_Add);
                    }
                    else
                    {
                        FunPriBindGuarantorDLL(_Edit);
                    }
                    gvGuarantor.DataSource = dtGuarantorGrid;
                    gvGuarantor.DataBind();
                    ViewState["dtGuarantorGrid"] = dtGuarantorGrid;
                    ViewState["GuarantorDetails"] = dtGuarantorGrid;
                    FunPriGenerateNewGuarantorRow();
                }
                else
                {
                    FunPriBindGuarantorDLL(_Add);
                }

                #endregion


                #region Repayment Structure
                System.Data.DataTable dtRepaystructure = dsApplicationDetails.Tables[6].Copy();

                if (dtRepaystructure.Rows.Count > 0 && dtRepaystructure.Rows[0].ItemArray[0].ToString() != "")
                {
                    ViewState["RepaymentStructure"] = dtRepaystructure;
                    foreach (DataRow dr in dtRepaystructure.Rows)
                    {
                        dr["InstallmentAmount"] = Convert.ToDecimal(Convert.ToDecimal(dr["InstallmentAmount"]).ToString(Funsetsuffix()));
                        dr["FinanceCharges"] = Convert.ToDecimal(Convert.ToDecimal(dr["FinanceCharges"]).ToString(Funsetsuffix()));
                        // dr["PrincipalAmount"] = Convert.ToDecimal(Convert.ToDecimal(dr["PrincipalAmount"]).ToString(Funsetsuffix()));
                        if (dr["Tax"].ToString() != "")
                            dr["Tax"] = Convert.ToDecimal(Convert.ToDecimal(dr["Tax"]).ToString(Funsetsuffix()));
                        if (dr["AMF"].ToString() != "")
                            dr["AMF"] = Convert.ToDecimal(Convert.ToDecimal(dr["AMF"]).ToString(Funsetsuffix()));
                        if (dr["ServiceTax"].ToString() != "")
                            dr["ServiceTax"] = Convert.ToDecimal(Convert.ToDecimal(dr["ServiceTax"]).ToString(Funsetsuffix()));
                        if (dr["Insurance"].ToString() != "")
                            dr["Insurance"] = Convert.ToDecimal(Convert.ToDecimal(dr["Insurance"]).ToString(Funsetsuffix()));
                        if (dr["Others"].ToString() != "")
                            dr["Others"] = Convert.ToDecimal(Convert.ToDecimal(dr["Others"]).ToString(Funsetsuffix()));
                        dtRepaystructure.AcceptChanges();
                    }
                    //ViewState["InstallmentAmount_Total"] = dtRepaystructure.Compute("sum(InstallmentAmount)", "");
                    //ViewState["AMF_Total"] = dtRepaystructure.Compute("sum(AMF)", "");
                    //ViewState["TAX_Total"] = dtRepaystructure.Compute("sum(Tax)", "");
                    //ViewState["ServiceTax_Total"] = dtRepaystructure.Compute("sum(ServiceTax)", "");
                    //if (dtRepaystructure.Rows[0]["Insurance"].ToString() != "") ;
                    //ViewState["Insurance_Total"] = dtRepaystructure.Compute("sum(Insurance)", "");
                    //if (dtRepaystructure.Rows[0]["Others"].ToString() != "") ;
                    //ViewState["Others_Total"] = dtRepaystructure.Compute("sum(Others)", "");
                    grvRepayStructure.DataSource = dtRepaystructure;
                    grvRepayStructure.DataBind();

                    //opc042 start
                    if (dsApplicationDetails.Tables[0].Rows[0]["state_wise_billing"].ToString() == "1")
                    {
                        grvCustEmail.DataSource = dsApplicationDetails.Tables[19];
                        grvCustEmail.DataBind();
                    }
                    //opc042 end

                    DataTable dtRepaymentStructureSTCopy = dtRepaystructure.Copy();
                    if (dtRepaymentStructureSTCopy.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtRepaymentStructureSTCopy.Rows)
                        {
                            if (dr["Tax"].ToString() != "")
                                dr["Tax"] = Math.Round(Convert.ToDecimal(dr["Tax"].ToString()));
                            if (dr["ServiceTax"].ToString() != "")
                                dr["ServiceTax"] = Math.Round(Convert.ToDecimal(dr["ServiceTax"].ToString()));
                            if (dr["Insurance"].ToString() != "")
                                dr["Insurance"] = Math.Round(Convert.ToDecimal(dr["Insurance"].ToString()));
                            if (dr["Others"].ToString() != "")
                                dr["Others"] = Math.Round(Convert.ToDecimal(dr["Others"].ToString()));
                        }

                        ViewState["InstallmentAmount_Total"] = dtRepaymentStructureSTCopy.Compute("sum(InstallmentAmount)", "");
                        ViewState["AMF_Total"] = dtRepaymentStructureSTCopy.Compute("sum(AMF)", "");
                        ViewState["TAX_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Tax)", "");
                        ViewState["ServiceTax_Total"] = dtRepaymentStructureSTCopy.Compute("sum(ServiceTax)", "");
                        if (dtRepaymentStructureSTCopy.Rows[0]["Insurance"].ToString() != "") ;
                        ViewState["Insurance_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Insurance)", "");
                        if (dtRepaymentStructureSTCopy.Rows[0]["Others"].ToString() != "") ;
                        ViewState["Others_Total"] = dtRepaymentStructureSTCopy.Compute("sum(Others)", "");
                    }

                }

                //DataTable DtRepayGridp = ((DataTable)ViewState["DtRepayGrid"]).Copy();
                //DataRow dr1;
                //dr1 = DtRepayGridp.NewRow();
                //dr1["slno"] = "1";
                //dr1["CashFlow"] = "Tax";
                //dr1["TotalPeriodInstall"] = ViewState["TAX_Total"];
                //DtRepayGridp.Rows.Add(dr1);

                //dr1 = DtRepayGridp.NewRow();
                //dr1["slno"] = "4";
                //dr1["CashFlow"] = "ServiceTax";
                //dr1["TotalPeriodInstall"] = ViewState["ServiceTax_Total"];
                //DtRepayGridp.Rows.Add(dr1);


                ////if (ViewState["Insurance_Total"].ToString() != "")
                ////{
                ////    dr1 = DtRepayGridp.NewRow();
                ////    dr1["slno"] = "5";
                ////    dr1["CashFlow"] = "Insurance";
                ////    dr1["TotalPeriodInstall"] = ViewState["Insurance_Total"];
                ////    DtRepayGridp.Rows.Add(dr1);
                ////}

                ////if (ViewState["Others_Total"].ToString() != "")
                ////{
                ////    dr1 = DtRepayGridp.NewRow();
                ////    dr1["slno"] = "6";
                ////    dr1["CashFlow"] = "Others";
                ////    dr1["TotalPeriodInstall"] = ViewState["Others_Total"];
                ////    DtRepayGridp.Rows.Add(dr1);
                ////}
                ////DataRow[] datainst = DtRepayGridp.Select("CashFlow_Flag_ID = 23", "");
                ////if (datainst.Length > 0)
                ////{
                ////    foreach (DataRow drrow in datainst)
                ////    {
                ////        drrow["TotalPeriodInstall"] = ViewState["InstallmentAmount_Total"];
                ////        drrow.AcceptChanges();
                ////    }
                ////}
                ////DataRow[] dataAMF = DtRepayGridp.Select("CashFlow_Flag_ID = 105", "");
                ////if (dataAMF.Length > 0)
                ////{
                ////    foreach (DataRow drrow in dataAMF)
                ////    {
                ////        drrow["TotalPeriodInstall"] = ViewState["AMF_Total"];
                ////        drrow.AcceptChanges();
                ////    }
                ////}
                //foreach (DataRow dr in DtRepayGridp.Rows)
                //{
                //    dr["TotalPeriodInstall"] = Convert.ToDecimal(Convert.ToDecimal(dr["TotalPeriodInstall"]).ToString(Funsetsuffix()));
                //}

                //DataView dv = DtRepayGridp.DefaultView;
                //dv.Sort = "slno asc";
                //DtRepayGridp = dv.ToTable();
                //ViewState["DtRepayGrid"] = DtRepayGrid;

                FunPriCalculateSummary(((DataTable)ViewState["DtRepayGrid"]), "cashflow", "TotalPeriodInstall");

                if (ViewState["InstallmentAmount_Total"] != null)
                    FunPriShowRepaymetDetails(Convert.ToDecimal(ViewState["InstallmentAmount_Total"]) + ((ViewState["AMF_Total"] != null) ? Convert.ToDecimal(ViewState["AMF_Total"]) : 0));
                else
                    FunPriShowRepaymetDetails((decimal)DtRepayGrid.Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID = 23"));

                #endregion

                #region "Prime Account tab"

                FunPriLoadRepaymentMode();
                //if (dsApplicationDetails.Tables[4].Rows.Count > 0)
                //{
                //    Load_Payment_Rule();
                //}
                //txtLOB_Followup.Text = ddlLOB.SelectedItem.Text;
                //txtBranch_Followup.Text = ddlBranchList.SelectedText;
                //txtApplication_Followup.Text = ddlApplicationReferenceNo.SelectedText;
                #endregion

                //if (intAccountCreationId == 0 && txtAccountDate.Text != DateTime.Now.Date.ToString(strDateFormat))
                //{
                //    txtBusinessIRR_Repay.Text = "";
                //}

                chkimport.Enabled = chkSEZ.Enabled = chkSEZA1.Enabled = chkcstwith.Enabled = chkCSTDeal.Enabled = false;
                ddlSEZZone.ClearDropDownList();
                pnlAddtionalTax.Enabled = false;
                TxtAutoExtnRental.Attributes.Add("readonly", "readonly");
                btnInvoices.Enabled = btnCalculateInvoices.Enabled = false;
                pnlCashflowInfo.Enabled = gvRepaymentDetails.Enabled = btnCalIRR.Enabled = btnReset.Enabled = false;
                ddlRepaymentMode.ClearDropDownList();
                gvGuarantor.Enabled = pnlEUCDtls.Enabled = false;
                chkAmfsold.Enabled = chkVATSold.Enabled = chkServiceTaxSold.Enabled = false;
                //grvSummaryInvoices.Columns[grvSummaryInvoices.Columns.Count - 1].Visible = false;
                UcCtlRsno.ButtonEnabled = false;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            if (intAccountCreationId == 0)
            {
                throw new ApplicationException("Due to Data Problem, Unable to Load Application Details");
            }
            else
            {
                throw new ApplicationException("Due to Data Problem, Unable to Load Rental Schedule Details");
            }
        }
        //Code Added By Ganapathy on 08/06/2012 BEGIN
        finally
        {
            ObjCustomerService.Close();
        }
        //END
    }

    //By Siva.K on 08JUN2015

    private void FunPriCopyAccountDetails(string sAccountCreationId)
    {
        DataSet dsApplicationDetails = new DataSet();

        try
        {

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CompanyId", intCompanyId.ToString());
            Procparam.Add("@PANum", sAccountCreationId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            dsApplicationDetails = Utility.GetDataset("S3G_LOANAD_GetRSdtls_CreateNewRS", Procparam);

            if (dsApplicationDetails != null)
            {

                ListItem lstItem;
                System.Data.DataTable dtHeader = new System.Data.DataTable();
                //System.Data.DataTable dtPASAdtls = new System.Data.DataTable();
                dtHeader = dsApplicationDetails.Tables[0].Copy();
                #region MainPage Tab

                FunPriLoadStatusDDL();
                ddlBranchList.Clear();
                ddlApplicationReferenceNo.SelectedText = dtHeader.Rows[0]["Pricing_No"].ToString();
                ddlApplicationReferenceNo.SelectedValue = Convert.ToString(dtHeader.Rows[0]["Pricing_ID"]);
                if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedValue))
                {
                    FunPriLoadProposalDtls(ddlApplicationReferenceNo.SelectedValue);
                }


                txtAccountDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["AccountDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtfirstinstdate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["FirstInst_StartDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtFirstInstallDue.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["firstinstdate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtCommenceDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["Commencement_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                TxtRSSignDate.Text = DateTime.Parse(Convert.ToString(dtHeader.Rows[0]["Signon_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                ViewState["Customer_Id"] = hdnCustomerId.Value = Convert.ToString(dtHeader.Rows[0]["Customer_Id"]);
                //By Siva.K For Data Load Problem in Multi User
                System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = Convert.ToString(dtHeader.Rows[0]["Customer_Id"]);

                #endregion

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            if (intAccountCreationId == 0)
            {
                throw new ApplicationException("Due to Data Problem, Unable to Load Application Details");
            }
            else
            {
                throw new ApplicationException("Due to Data Problem, Unable to Load Rental Schedule Details");
            }
        }

        finally
        {
            // ObjCustomerService.Close();
        }

    }

    //private void FunPriGetApplicationDetails(int intApplicationId)
    //{
    //    DataSet dsApplicationDetails = new DataSet();
    //    S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
    //    //Code Added by Ganapathy on 08/06/2012
    //    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
    //    //END
    //    try
    //    {

    //        if (intApplicationId == 0)
    //        {
    //            if (strConsNumber != null)
    //            {
    //                dsApplicationDetails = FunPriGetAccountConsolidationDetails(strConsNumber);
    //            }
    //            else if (strSplitNum != null)
    //            {
    //                dsApplicationDetails = FunPriGetAccountSplitDetails(strSplitNum, strSplitRefNo);
    //            }
    //            rfvApplicationReferenceNo.Enabled = false;
    //            ViewState["ConsolidateApplicationProcessId"] = dsApplicationDetails.Tables[0].Rows[0]["Application_Process_ID"];
    //        }
    //        else
    //        {
    //            Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //            Procparam.Add("@ApplicationProcessId", intApplicationId.ToString());
    //            if (intAccountCreationId > 0)
    //            {
    //                Procparam.Add("@CompanyId", intCompanyId.ToString());
    //                Procparam.Add("@PANum", strPANum);
    //                Procparam.Add("@SANum", (string.IsNullOrEmpty(strSANum)) ? "0" : strSANum);
    //                Procparam.Add("@AccountCreationId", intAccountCreationId.ToString());
    //            }
    //            dsApplicationDetails = Utility.GetDataset("S3G_LoanAd_LoadAccountDetails", Procparam);
    //        }
    //        if (dsApplicationDetails != null)
    //        {
    //            if (dsApplicationDetails.Tables[0].Rows.Count == 0)
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Activate the Account for selected Application Reference No')", true);
    //                return;
    //            }
    //            ListItem lstItem;

    //            #region MainPage Tab
    //            ddlLOB.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["LOB_Id"]);
    //            //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACTORING"))
    //            /*if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACTORING"))
    //            {
    //                chkDORequired.Enabled = false;
    //                TabContainerMainTab.Tabs[1].Visible = false;
    //            }
    //            else
    //            {
    //                chkDORequired.Enabled = true;
    //                TabContainerMainTab.Tabs[1].Visible = true;
    //            }*/
    //            ddlBranchList.SelectedText = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Name"]);
    //            ddlBranchList.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Id"]);
    //            ddlBranchList.ToolTip = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Name"]);
    //            //txtFBDate.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["FBDate"]);
    //            //txtFBDate.Text = (txtFBDate.Text == "0") ? "" : txtFBDate.Text;
    //            if (strConsNumber != null || strSplitNum != null)
    //            {
    //                ddlLOB.ClearDropDownList();
    //                //Removed By Shibu 17-Sep-2013
    //                //ddlBranchList.ClearDropDownList();
    //                // ddlBranchList.Clear(); 
    //                rfvTenure.Enabled = true;
    //                rfvTenureType.Enabled = true;
    //            }
    //            if (intApplicationId == 0)
    //            {
    //                FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedValue);
    //            }

    //            FunPriLoadStatusDDL();
    //            if (dsApplicationDetails.Tables[0].Columns.Contains("PA_Status_Code"))
    //            {
    //                if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PA_Status_Code"]) != "")
    //                {
    //                    ddlStatus.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PA_Status_Code"]);
    //                    txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                    if (ddlStatus.SelectedValue == "11")
    //                    {
    //                        txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                        //tcAccountCreation.Tabs[7].Visible = true;
    //                    }
    //                }
    //            }

    //            if (intAccountCreationId > 0 || strSplitNum != null || strConsNumber != null)
    //            {

    //                FunPriLoadApplicationNoDDL(Convert.ToInt32(ddlBranchList.SelectedValue), Convert.ToInt32(ddlLOB.SelectedValue));
    //                ddlApplicationReferenceNo.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Application_Process_Id"]);
    //                if (strSplitNum != null || strConsNumber != null)
    //                {
    //                    txtApplication_Followup.Text = ddlApplicationReferenceNo.SelectedItem.Text;
    //                    ddlApplicationReferenceNo.ClearDropDownList();
    //                }
    //                txtPrimeAccountNo.Text = strPANum;
    //                //if (strSANum != "")
    //                //{
    //                //    txtSubAccountNo.Text = strSANum;
    //                //    //tcAccountCreation.Tabs[7].Visible = true;
    //                //}
    //                //else
    //                //{
    //                //    //tcAccountCreation.Tabs[7].Visible = false;
    //                //}
    //                //if (strSplitNum == null)
    //                txtAccountDate.Text = DateTime.Parse(Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["AccountDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    //                if (dsApplicationDetails.Tables.Count > 17)
    //                {
    //                    if (dsApplicationDetails.Tables[17].Rows.Count > 0)
    //                    {

    //                        ddlStatus.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["SA_Status_Code"]);
    //                        txtStatus.Text = ddlStatus.SelectedItem.Text;


    //                        txtAdvanceInstallments.Text = Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["Advance_Installments"]);
    //                        txtAdvanceInstallments.Text = (txtAdvanceInstallments.Text == "0") ? "" : txtAdvanceInstallments.Text;
    //                        ddlRepaymentMode.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["Repayment_Code"]);
    //                        if (!string.IsNullOrEmpty(Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["Last_ODI_Date"])))
    //                        {
    //                            txtLastODICalcDate.Text = DateTime.Parse(Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["Last_ODI_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    //                        }
    //                        if (!string.IsNullOrEmpty(dsApplicationDetails.Tables[17].Rows[0]["Is_delivery_Order_Require"].ToString()))
    //                        {
    //                            chkDORequired.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[17].Rows[0]["Is_delivery_Order_Require"]);
    //                        }

    //                    }

    //                }
    //            }

    //            //FunPriLoadApplicationNoDDL(Convert.ToInt32(ddlBranchList.SelectedValue), Convert.ToInt32(ddlLOB.SelectedValue));
    //            //ddlApplicationReferenceNo.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Application_Process_Id"]);
    //            if (strSplitNum != null)
    //            {
    //                ddlApplicationReferenceNo.ClearDropDownList();
    //            }
    //            //txtApplicationDate.Text = DateTime.Parse(Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    //            if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["MLA_Number"]) != "0")
    //            {
    //                txtPrimeAccountNo.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["MLA_Number"]);
    //                if (ViewState["strMLASLAApplicable"] != null)
    //                {
    //                    if (ViewState["strMLASLAApplicable"].ToString().ToUpper() == "FALSE")
    //                    {
    //                        ddlStatus.SelectedValue = "2";
    //                    }
    //                    else
    //                    {
    //                        ddlStatus.SelectedValue = "10";
    //                    }
    //                }
    //                txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                //tcAccountCreation.Tabs[7].Visible = false;
    //            }
    //            if (dsApplicationDetails.Tables[0].Columns.Contains("PA_Status_Code"))
    //            {
    //                if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PA_Status_Code"]) != "")
    //                {
    //                    ddlStatus.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PA_Status_Code"]);
    //                    txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                    if (ddlStatus.SelectedValue == "11")
    //                    {
    //                        txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                        //tcAccountCreation.Tabs[7].Visible = true;
    //                    }
    //                }
    //            }
    //            if (dsApplicationDetails.Tables[0].Columns.Contains("MLA_Applicable"))
    //            {
    //                if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["MLA_Applicable"]) == "0")
    //                {
    //                    //tcAccountCreation.Tabs[7].Visible = false;
    //                }
    //            }

    //            if (dsApplicationDetails.Tables[0].Columns.Contains("PANum"))
    //            {
    //                if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PANum"]) != "")
    //                {
    //                    txtPrimeAccountNo.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PANum"]);
    //                }
    //            }
    //            if (intAccountCreationId > 0)
    //            {
    //                if (dsApplicationDetails.Tables.Count > 17)
    //                {
    //                    if (dsApplicationDetails.Tables[17].Rows.Count > 0)
    //                    {
    //                        FunPriLoadStatusDDL();
    //                        ddlStatus.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["SA_Status_Code"]);
    //                        txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                    }

    //                }
    //            }
    //            if (intApplicationId == 0)
    //            {
    //                if (strSplitNum != null)
    //                {
    //                    if ((string)ViewState["SANUM"] == (string)ViewState["PANUM"] + "DUMMY")
    //                    {
    //                        txtPrimeAccountNo.Text = "";
    //                    }
    //                }
    //                if (FunPriCheckMLA((string)ViewState["PANUM"]))
    //                {
    //                    txtPrimeAccountNo.Text = "";
    //                }

    //            }
    //            hdnCustomerId.Value = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Customer_Id"]);
    //            S3GCustomerAddress1.SetCustomerDetails(dsApplicationDetails.Tables[0].Rows[0], true);
    //            Session["AccountAssetCustomer"] = hdnCustomerId.Value + ";" + S3GCustomerAddress1.CustomerName;
    //            txtAddress1.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_address1"]);
    //            txtAddress2.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_address2"]);
    //            txtState.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_state"]);
    //            txtPincode.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_pincode"]);
    //            txtCity.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_city"]);
    //            txtCountry.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_country"]);


    //            hdnProductId.Value = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Product_Id"]);
    //            txtProductCode.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Product"]);
    //            if (intApplicationId > 0 || strSplitNum != null)
    //            {
    //                if (intApplicationId > 0)
    //                {
    //                    txtAccountingIRR.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Accounting_IRR"]);
    //                    txtCompanyIRR.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Company_IRR"]);
    //                    txtBusinessIRR.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Business_IRR"]);
    //                    if (intAccountCreationId == 0)
    //                    {
    //                        txtFinanceAmount.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Finance_Amount"]);
    //                    }
    //                    else
    //                    {
    //                        txtFinanceAmount.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Loan_Amount"]);
    //                    }

    //                }
    //                txtTenure.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Tenure"]);
    //                //Added by Ganapathy on 08/06/2012 BEGIN
    //                ObjStatus.Option = 1;
    //                ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
    //                Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
    //                //END
    //                ddlTenureType.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Tenure_Type"]);
    //                txtTenure.ReadOnly = true;
    //                ddlTenureType.ClearDropDownList();
    //            }
    //            else
    //            {
    //                txtTenure.ReadOnly = false;
    //            }

    //            ddlSalePersonCodeList.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Sales_Person_Id"]);
    //            ddlSalePersonCodeList.SelectedText = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Sales_Person_Name"]);
    //            txtConstitutionCode.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Constitution"]);
    //            hdnConstitutionId.Value = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Constitution_Id"]);


    //            if (strConsNumber == null)
    //            {
    //                txtResidualAmt_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Residual_Value"]);
    //                txtResidualAmt_Cashflow.Text = (txtResidualAmt_Cashflow.Text == "0") ? "" : txtResidualAmt_Cashflow.Text;
    //                txtResidualValue_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Residual_Value"]);
    //                txtResidualValue_Cashflow.Text = (txtResidualValue_Cashflow.Text.StartsWith("0")) ? "" : txtResidualValue_Cashflow.Text;

    //                txtMarginMoneyAmount_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Margin_Amount"]);
    //                txtMarginMoneyAmount_Cashflow.Text = (txtMarginMoneyAmount_Cashflow.Text.StartsWith("0")) ? "" : txtMarginMoneyAmount_Cashflow.Text;
    //                txtMarginMoneyPer_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Margin"]);
    //                txtMarginMoneyPer_Cashflow.Text = (txtMarginMoneyPer_Cashflow.Text.StartsWith("0")) ? "" : txtMarginMoneyPer_Cashflow.Text;
    //            }

    //            grvConsDocuments.DataSource = dsApplicationDetails.Tables[14];
    //            grvConsDocuments.DataBind();
    //            #endregion

    //            #region OfferTerms Tab
    //            if (intApplicationId > 0 || strSplitNum != null)
    //            {
    //                ddlROIRuleList.Items.Clear();
    //                lstItem = new ListItem(dsApplicationDetails.Tables[3].Rows[0]["ROI_Number"].ToString(), dsApplicationDetails.Tables[3].Rows[0]["ROI_Rules_ID"].ToString());
    //                ddlROIRuleList.Items.Add(lstItem);
    //                ddlROIRuleList.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[3].Rows[0]["ROI_Rules_ID"]);
    //                ViewState["ROIRules"] = dsApplicationDetails.Tables[3];
    //                FunPriBindROIDLL(_Add);
    //                Load_ROI_Rule();
    //            }
    //            if (strSplitNum == null)
    //            {

    //                ddlROIRuleList.ClearDropDownList();

    //            }
    //            if (strConsNumber != null)
    //            {
    //                ddlLOB.ClearDropDownList();
    //                //Removed By Shibu 17-Sep-2013
    //                //ddlBranchList.ClearDropDownList();
    //                ddlBranchList.Clear();
    //                FunPriBindInflowDLL(_Add);
    //                FunPriBindOutflowDLL(_Add);
    //                FunPriBindRepaymentDLL(_Add);
    //                FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedValue);
    //                FunPriBindROIDLL(_Add);
    //                Load_ROI_Rule();
    //            }
    //            ddlPaymentRuleList.Items.Clear();
    //            if (dsApplicationDetails.Tables[4].Rows.Count > 0)
    //            {
    //                lstItem = new ListItem(dsApplicationDetails.Tables[4].Rows[0]["Payment_Rule_Number"].ToString(), dsApplicationDetails.Tables[4].Rows[0]["Payment_RuleCard_ID"].ToString());
    //                ddlPaymentRuleList.Items.Add(lstItem);
    //            }
    //            FunPriSetRateLength();

    //            ViewState["DtCashFlow"] = dsApplicationDetails.Tables[1];
    //            if (dsApplicationDetails.Tables[1].Rows.Count > 0)
    //            {
    //                if (intAccountCreationId == 0)
    //                {
    //                    FunPriBindInflowDLL(_Add);
    //                }
    //                else
    //                {
    //                    FunPriBindInflowDLL(_Edit);
    //                }
    //                gvInflow.DataSource = dsApplicationDetails.Tables[1];
    //                gvInflow.DataBind();
    //                FunPriGenerateNewInflowRow();
    //                ViewState["DtCashFlow"] = dsApplicationDetails.Tables[1];
    //            }
    //            else
    //            {
    //                FunPriBindInflowDLL(_Add);
    //            }
    //            ////added by saranya
    //            //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") && ddl_Repayment_Mode.SelectedItem.Text.StartsWith("Pro")))
    //            //{
    //            //    Button btnAdd = gvInflow.FooterRow.FindControl("btnAdd") as Button;
    //            //    btnAdd.Enabled = false;
    //            //}
    //            //end
    //            ViewState["DtCashFlowOut"] = dsApplicationDetails.Tables[2];
    //            if (dsApplicationDetails.Tables[2].Rows.Count > 0)
    //            {
    //                if (intAccountCreationId == 0)
    //                {
    //                    FunPriBindOutflowDLL(_Add);
    //                }
    //                else
    //                {
    //                    FunPriBindOutflowDLL(_Edit);
    //                }
    //                gvOutFlow.DataSource = dsApplicationDetails.Tables[2];
    //                gvOutFlow.DataBind();
    //                FunPriGenerateNewOutflowRow();
    //            }
    //            else
    //            {
    //                FunPriBindOutflowDLL(_Add);
    //            }


    //            ViewState["DtCashFlowOut"] = dsApplicationDetails.Tables[2];
    //            lblTotalOutFlowAmount.Text = dsApplicationDetails.Tables[9].Rows[0].ItemArray[0].ToString();
    //            #endregion

    //            FunPriLoadRepaymentMode();
    //            if (dsApplicationDetails.Tables[4].Rows.Count > 0)
    //            {
    //                Load_Payment_Rule();
    //            }
    //            txtLOB_Followup.Text = ddlLOB.SelectedItem.Text;
    //            //  txtBranch_Followup.Text = ddlBranchList.SelectedItem.Text;
    //            txtBranch_Followup.Text = ddlBranchList.SelectedText;
    //            txtApplication_Followup.Text = ddlApplicationReferenceNo.SelectedItem.Text;
    //            if (intApplicationId > 0 || (strSplitRefNo != null))
    //            {

    //                #region Repayment Tab

    //                ViewState["DtRepayGrid"] = dsApplicationDetails.Tables[5];
    //                if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
    //                {
    //                    ViewState["DtRepayGrid_TL"] = dsApplicationDetails.Tables[5];
    //                }
    //                if (intAccountCreationId == 0)
    //                {
    //                    //FunPriBindRepaymentDLL(_Add);
    //                }
    //                else
    //                {
    //                    if (dsApplicationDetails.Tables[5].Rows.Count > 0)
    //                    {
    //                        //FunPriBindRepaymentDLL(_Edit);
    //                    }
    //                }
    //                if (dsApplicationDetails.Tables[5].Rows.Count > 0)
    //                {
    //                    gvRepaymentDetails.DataSource = dsApplicationDetails.Tables[5];
    //                    gvRepaymentDetails.DataBind();
    //                    FunPriGenerateNewRepaymentRow();
    //                    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
    //                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
    //                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
    //                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
    //                    if (intApplicationId > 0)
    //                    {
    //                        txtAccountIRR_Repay.Text = txtAccountingIRR.Text = dsApplicationDetails.Tables[0].Rows[0]["Accounting_IRR"].ToString();
    //                        txtBusinessIRR_Repay.Text = txtBusinessIRR.Text = dsApplicationDetails.Tables[0].Rows[0]["Business_IRR"].ToString();
    //                        txtCompanyIRR_Repay.Text = txtCompanyIRR.Text = dsApplicationDetails.Tables[0].Rows[0]["Company_IRR"].ToString();
    //                    }
    //                    if (ddl_Repayment_Mode.SelectedValue != "2")
    //                    {
    //                        Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
    //                        if (lblCashFlowId.Text != "23")
    //                        {
    //                            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
    //                        }
    //                        else
    //                        {
    //                            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
    //                    }
    //                    ViewState["DtRepayGrid"] = dsApplicationDetails.Tables[5];

    //                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
    //                    {
    //                        ViewState["DtRepayGrid_TL"] = dsApplicationDetails.Tables[5];
    //                    }

    //                    if (strConsNumber != null || strSplitNum != null)
    //                    {
    //                        FunPriCalculateSummary(dsApplicationDetails.Tables[5], "CashFlow", "TotalPeriodInstall");
    //                    }
    //                    else
    //                    {
    //                        gvRepaymentSummary.DataSource = dsApplicationDetails.Tables[11];
    //                        gvRepaymentSummary.DataBind();
    //                    }
    //                    if (ddl_Rate_Type.Visible && ddl_Rate_Type.SelectedItem.Text == "Floating" && string.IsNullOrEmpty(txtFBDate.Text))
    //                    {
    //                        ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
    //                        ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
    //                    }
    //                    else
    //                    {
    //                        ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
    //                        ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
    //                    }
    //                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))))
    //                    {
    //                        DataRow[] dr = dsApplicationDetails.Tables[5].Select("CashFlow_Flag_ID IN(35,91)");
    //                        if (dr.Length == 0)
    //                        {
    //                            FunPriShowRepaymetDetails((decimal)dsApplicationDetails.Tables[5].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
    //                        }
    //                        else
    //                        {
    //                            FunPriShowRepaymetDetails((decimal)dsApplicationDetails.Tables[5].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =91"));
    //                        }

    //                    }
    //                    else
    //                    {
    //                        FunPriShowRepaymetDetails((decimal)dsApplicationDetails.Tables[5].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
    //                    }
    //                }

    //                #endregion


    //                #region Guarantor Tab


    //                ViewState["dtGuarantorGrid"] = dsApplicationDetails.Tables[12];
    //                if (dsApplicationDetails.Tables[12].Rows.Count > 0)
    //                {
    //                    if (intAccountCreationId == 0)
    //                    {

    //                        FunPriBindGuarantorDLL(_Add);
    //                    }
    //                    else
    //                    {
    //                        FunPriBindGuarantorDLL(_Edit);
    //                    }
    //                    gvGuarantor.DataSource = dsApplicationDetails.Tables[12];
    //                    gvGuarantor.DataBind();
    //                    ViewState["dtGuarantorGrid"] = dsApplicationDetails.Tables[12];
    //                    ViewState["GuarantorDetails"] = dsApplicationDetails.Tables[12];
    //                    FunPriGenerateNewGuarantorRow();
    //                }
    //                else
    //                {
    //                    FunPriBindGuarantorDLL(_Add);
    //                }

    //                #endregion

    //                if (intApplicationId > 0)
    //                {

    //                    #region Alerts Tab

    //                    if (dsApplicationDetails.Tables[6].Rows.Count == 0)
    //                    {
    //                        FunPriBindAlertDLL(_Add);
    //                    }
    //                    else
    //                    {
    //                        ViewState["DtAlertDetails"] = dsApplicationDetails.Tables[6];
    //                        if (intAccountCreationId == 0)
    //                        {

    //                            FunPriBindAlertDLL(_Add);
    //                        }
    //                        else
    //                        {
    //                            FunPriBindAlertDLL(_Edit);
    //                        }
    //                        gvAlert.DataSource = dsApplicationDetails.Tables[6];
    //                        gvAlert.DataBind();
    //                        FunPriGenerateNewAlertRow();
    //                        ViewState["DtAlertDetails"] = dsApplicationDetails.Tables[6];
    //                    }
    //                    #endregion

    //                    #region Followup Tab

    //                    if (dsApplicationDetails.Tables[7].Rows.Count > 0)
    //                    {
    //                        txtAccount_Followup.Text = txtPrimeAccountNo.Text;
    //                        txtEnquiry_Followup.Text = dsApplicationDetails.Tables[7].Rows[0]["Enquiry_Number"].ToString();
    //                        txtOfferNo_Followup.Text = dsApplicationDetails.Tables[7].Rows[0]["Offer_Number"].ToString();
    //                        txtApplication_Followup.Text = dsApplicationDetails.Tables[7].Rows[0]["Application_Number"].ToString();
    //                        if (!string.IsNullOrEmpty(dsApplicationDetails.Tables[7].Rows[0]["Date"].ToString()))
    //                            txtEnquiryDate_Followup.Text = Convert.ToDateTime(dsApplicationDetails.Tables[7].Rows[0]["Date"].ToString()).ToString(strDateFormat);
    //                        txtCustNameAdd_Followup.Text = S3GCustomerAddress1.CustomerName + "," + "\n" + txtAddress1.Text + " " + txtAddress2.Text + "," + "\n" + txtCity.Text
    //                                                       + "-" + txtPincode.Text + "\n" + txtState.Text + "\n" + txtCountry.Text;
    //                    }
    //                    if (dsApplicationDetails.Tables[8].Rows.Count == 0)
    //                    {
    //                        FunPriBindFollowupDLL(_Add);
    //                    }
    //                    else
    //                    {
    //                        ViewState["DtFollowUp"] = dsApplicationDetails.Tables[8];
    //                        if (intAccountCreationId == 0)
    //                        {
    //                            FunPriBindFollowupDLL(_Add);
    //                        }
    //                        else
    //                        {
    //                            /*ADDED BY NATARAJ Y for issues in follow up loading by 27/6/2011*/
    //                            if (dsApplicationDetails.Tables[8].Rows.Count > 0)
    //                            {
    //                                FunPriBindFollowupDLL(_Edit);
    //                            }
    //                            else
    //                            {
    //                                FunPriBindFollowupDLL(_Add);
    //                            }
    //                        }
    //                        gvFollowUp.DataSource = dsApplicationDetails.Tables[8];
    //                        gvFollowUp.DataBind();
    //                        FunPriGenerateNewFollowupRow();
    //                        ViewState["DtFollowUp"] = dsApplicationDetails.Tables[8];
    //                    }
    //                    #endregion

    //                    #region Moratorium Tab
    //                    if (Request.QueryString["qsAccConNo"] == null)
    //                    {
    //                        if (ddl_Time_Value.SelectedIndex > 0)
    //                        {
    //                            txtRepaymentTime.Text = ddl_Time_Value.SelectedItem.Text;
    //                        }
    //                    }
    //                    if (ddl_Time_Value.SelectedValue == "3" || ddl_Time_Value.SelectedValue == "4")
    //                    {
    //                        txtFBDate.Enabled = true;
    //                        rvtxtFBDate.Enabled = true;
    //                        rvtxtFBDate.Enabled = true;
    //                    }
    //                    else
    //                    {
    //                        txtFBDate.Enabled = false;
    //                        rvtxtFBDate.Enabled = false;
    //                        rvtxtFBDate.Enabled = false;
    //                        txtFBDate.Text = "";
    //                    }
    //                    if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
    //                    {

    //                        txtAdvanceInstallments.Attributes.Add("readonly", "readonly");
    //                    }
    //                    else
    //                    {

    //                        txtAdvanceInstallments.Attributes.Remove("readonly");
    //                    }

    //                    ViewState["dtMoratorium"] = dsApplicationDetails.Tables[13];
    //                    if (dsApplicationDetails.Tables[13].Rows.Count > 0)
    //                    {
    //                        if (intAccountCreationId == 0)
    //                        {
    //                            FunPriBindMoratoriumDLL(_Add);
    //                        }
    //                        else
    //                        {
    //                            FunPriBindMoratoriumDLL(_Edit);
    //                        }
    //                        gvMoratorium.DataSource = dsApplicationDetails.Tables[13];
    //                        gvMoratorium.DataBind();
    //                        ViewState["dtMoratorium"] = dsApplicationDetails.Tables[13];
    //                        FunPriGenerateNewMorotoriumRow();
    //                        ViewState["dtMoratorium"] = dsApplicationDetails.Tables[13];
    //                    }
    //                    else
    //                    {
    //                        FunPriBindMoratoriumDLL(_Add);
    //                    }
    //                    #endregion
    //                }
    //                else
    //                {
    //                    if (strSplitNum != null)
    //                    {
    //                        #region Moratorium Tab
    //                        if (Request.QueryString["qsAccConNo"] == null)
    //                        {
    //                            if (ddl_Time_Value.SelectedIndex > 0)
    //                            {
    //                                txtRepaymentTime.Text = ddl_Time_Value.SelectedItem.Text;
    //                            }
    //                        }
    //                        if (ddl_Time_Value.SelectedValue == "3" || ddl_Time_Value.SelectedValue == "4")
    //                        {
    //                            txtFBDate.Enabled = true;
    //                            rvtxtFBDate.Enabled = true;
    //                            rvtxtFBDate.Enabled = true;
    //                        }
    //                        else
    //                        {
    //                            txtFBDate.Enabled = false;
    //                            rvtxtFBDate.Enabled = false;
    //                            rvtxtFBDate.Enabled = false;
    //                            txtFBDate.Text = "";
    //                        }
    //                        if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
    //                        {

    //                            txtAdvanceInstallments.Attributes.Add("readonly", "readonly");
    //                        }
    //                        else
    //                        {

    //                            txtAdvanceInstallments.Attributes.Remove("readonly");
    //                        }

    //                        #endregion
    //                    }
    //                }

    //            }
    //            #region Asset Tab
    //            if (intAccountCreationId == 0)
    //            {
    //                if (strSplitNum == null && strConsNumber == null)//For Consolidation & Split
    //                {
    //                    string strBaseMLA = dsApplicationDetails.Tables[0].Rows[0]["MLA_Applicable"].ToString();
    //                    ViewState["IsBaseMLA"] = strBaseMLA;
    //                    if (strBaseMLA == "1" && (txtPrimeAccountNo.Text == "" || (txtPrimeAccountNo.Text != "" && txtStatus.Text.StartsWith("10"))))
    //                    {
    //                        txtFinanceAmount.ReadOnly = true;
    //                    }
    //                }
    //            }

    //            Session["ApplicationAssetDetails"] = dsApplicationDetails.Tables[10];
    //            gvAssetDetails.DataSource = dsApplicationDetails.Tables[10];
    //            gvAssetDetails.DataBind();
    //            System.Data.DataTable dtTable = (System.Data.DataTable)Session["ApplicationAssetDetails"];


    //            if (intAccountCreationId == 0)
    //            {
    //                if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL") && dsApplicationDetails.Tables[17].Rows.Count > 0)
    //                {
    //                    Session["ApplicationloanAssetDetails"] = dsApplicationDetails.Tables[17];
    //                    grvloanasset.DataSource = dsApplicationDetails.Tables[17];
    //                    grvloanasset.DataBind();
    //                    System.Data.DataTable dtTable1 = (System.Data.DataTable)Session["ApplicationloanAssetDetails"];
    //                }
    //            }
    //            else
    //            {
    //                if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL") && dsApplicationDetails.Tables[20].Rows.Count > 0)
    //                {
    //                    Session["ApplicationloanAssetDetails"] = dsApplicationDetails.Tables[20];
    //                    grvloanasset.DataSource = dsApplicationDetails.Tables[20];
    //                    grvloanasset.DataBind();
    //                    System.Data.DataTable dtTable1 = (System.Data.DataTable)Session["ApplicationloanAssetDetails"];
    //                }
    //            }
    //            #endregion

    //            #region Invoice
    //            if (dsApplicationDetails.Tables.Count > 15)
    //            {
    //                if (dsApplicationDetails.Tables[15].Rows.Count > 0 && dsApplicationDetails.Tables[15].Rows[0].ItemArray[0].ToString() != "")
    //                {
    //                    gvInvoiceDetails.DataSource = dsApplicationDetails.Tables[15];
    //                    ViewState["InvoiceDetails"] = dsApplicationDetails.Tables[15];
    //                    gvInvoiceDetails.DataBind();
    //                }
    //            }
    //            #endregion

    //            #region Repayment Structure
    //            if (dsApplicationDetails.Tables.Count > 16)
    //            {
    //                if (dsApplicationDetails.Tables[16].Rows.Count > 0 && dsApplicationDetails.Tables[16].Rows[0].ItemArray[0].ToString() != "")
    //                {
    //                    ViewState["RepaymentStructure"] = dsApplicationDetails.Tables[16];
    //                    grvRepayStructure.DataSource = dsApplicationDetails.Tables[16];
    //                    grvRepayStructure.DataBind();
    //                }

    //                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
    //                if (intAccountCreationId > 0)
    //                {
    //                    if (dsApplicationDetails.Tables[19].Rows.Count > 0)
    //                        ViewState["dtRepayDetailsOthers"] = dsApplicationDetails.Tables[19];
    //                }
    //                else
    //                {
    //                    if (dsApplicationDetails.Tables[18].Rows.Count > 0)
    //                        ViewState["dtRepayDetailsOthers"] = dsApplicationDetails.Tables[18];
    //                }
    //                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end
    //            }
    //            #endregion

    //            #region SubAccount User

    //            if (intAccountCreationId > 0)
    //            {
    //                /*tcAccountCreation.Tabs[7].Visible = !string.IsNullOrEmpty(txtSubAccountNo.Text);
    //                if (!string.IsNullOrEmpty(txtSubAccountNo.Text))
    //                {
    //                    if (dsApplicationDetails.Tables[18].Rows.Count > 0)
    //                    {
    //                        txtSLACustomerCode.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_Internal_Code_Ref"].ToString();
    //                        txtSLAUserName.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Name"].ToString();
    //                        txtComAddress1.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Address1"].ToString();
    //                        txtCOmAddress2.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Address2"].ToString();
    //                        txtComCity.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_City"].ToString();
    //                        txtComState.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_State"].ToString();
    //                        txtComCountry.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Country"].ToString();
    //                        txtComPincode.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Pincode"].ToString();
    //                        txtComPhone.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Phone"].ToString();
    //                        txtComEMail.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Email"].ToString();
    //                        txtComWebsite.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Website"].ToString();

    //                    }
    //                }*/
    //            }
    //            #endregion
    //            //if (txtPrimeAccountNo.Text == "" || txtStatus.Text.StartsWith("10") || txtStatus.Text.StartsWith("2"))
    //            //{
    //            //    tcAccountCreation.Tabs[7].Visible = false;
    //            //}
    //            //else if (txtPrimeAccountNo.Text != "" && intAccountCreationId == 0)
    //            //{
    //            //    tcAccountCreation.Tabs[7].Visible = true;
    //            //}

    //            //if (strMode == "C" && ddl_Return_Pattern.Visible && ddl_Return_Pattern.SelectedItem.Text.StartsWith("IRR"))
    //            //{
    //            //    FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedValue);
    //            //    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
    //            //    //strDocumentDate = txtApplicationDate.Text;
    //            //    GenerateRepaymentSchedule(objRepaymentStructure,Utility.StringToDate(txtApplicationDate.Text));
    //            //}
    //            if (intAccountCreationId == 0 && txtAccountDate.Text != DateTime.Now.Date.ToString(strDateFormat))
    //            {
    //                txtBusinessIRR_Repay.Text = "";
    //            }

    //        }
    //        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") ||
    //            ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") ||
    //            ((ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") ||
    //            ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TL")) &&
    //            ddl_Repayment_Mode.SelectedItem.Text.ToUpper().StartsWith("PRO")))
    //        {
    //            tcAccountCreation.Tabs[2].Enabled = false;
    //        }
    //        else
    //            tcAccountCreation.Tabs[2].Enabled = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        if (intAccountCreationId == 0)
    //        {
    //            throw new ApplicationException("Due to Data Problem, Unable to Load Application Details");
    //        }
    //        else
    //        {
    //            throw new ApplicationException("Due to Data Problem, Unable to Load Account Details");
    //        }
    //    }
    //    //Code Added By Ganapathy on 08/06/2012 BEGIN
    //    finally
    //    {
    //        ObjCustomerService.Close();
    //    }
    //    //END
    //}



    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeypress", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
        }
    }


    //protected void FunProLoadAddressCombos()
    //{
    //    try
    //    {
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        if (intCompanyId > 0)
    //        {
    //            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
    //        }
    //        System.Data.DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);

    //        System.Data.DataTable dtSource = new System.Data.DataTable();
    //        if (dtAddr.Select("Category = 1").Length > 0)
    //        {
    //            dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
    //        }
    //        else
    //        {
    //            dtSource = FunProAddAddrColumns(dtSource);
    //        }
    //        txtComCity.FillDataTable(dtSource, "Name", "Name", false);

    //        dtSource = new System.Data.DataTable();
    //        if (dtAddr.Select("Category = 2").Length > 0)
    //        {
    //            dtSource = dtAddr.Select("Category = 2").CopyToDataTable();
    //        }
    //        else
    //        {
    //            dtSource = FunProAddAddrColumns(dtSource);
    //        }
    //        txtComState.FillDataTable(dtSource, "Name", "Name", false);

    //        dtSource = new System.Data.DataTable();
    //        if (dtAddr.Select("Category = 3").Length > 0)
    //        {
    //            dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
    //        }
    //        else
    //        {
    //            dtSource = FunProAddAddrColumns(dtSource);
    //        }
    //        txtComCountry.FillDataTable(dtSource, "Name", "Name", false);

    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    protected System.Data.DataTable FunProAddAddrColumns(System.Data.DataTable dt)
    {
        dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("Category");

        return dt;
    }
    //private void FunPriUpdateROIRuleDecRate()//Added on 3/11/2011 by saran for UAT raised mail modify mode not allowing to save forr IRR to flat rate
    //{
    //    System.Data.DataTable ObjDTROI = new System.Data.DataTable(); ;
    //    ObjDTROI = (System.Data.DataTable)ViewState["ROIRules"];
    //    decimal decRate = 0;
    //    switch (ddl_Return_Pattern.SelectedValue)
    //    {

    //        case "1":
    //            decRate = Convert.ToDecimal(txtRate.Text);
    //            break;
    //        case "2":
    //            //ObjCommonBusLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, Convert.ToDecimal(txtFacilityAmt.Text), Convert.ToDouble(9.6365), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decRate, Convert.ToDecimal(10.05), decPLR);
    //            if (ViewState["decRate"] != null)
    //            {
    //                decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
    //            }//Hard Coded for testing IRR
    //            break;
    //    }
    //    ObjDTROI.Rows[0]["IRR_Rate"] = decRate;
    //    ObjDTROI.Rows[0].AcceptChanges();
    //    ViewState["ROIRules"] = ObjDTROI;
    //}

    private bool FunPriIsDeferredPayment()
    {
        bool blnResult = false;
        System.Data.DataTable dtAcctype = ((System.Data.DataTable)ViewState["PaymentRules"]);
        dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
        System.Data.DataTable dtOutflow = (System.Data.DataTable)ViewState["DtCashFlowOut"];
        if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Deferred Payment")
        {
            DataRow[] drOutflowDate = dtOutflow.Select("CashFlow_Flag_ID = 41 and Date < #" + Utility.StringToDate(txtAccountDate.Text) + "#");
            if (drOutflowDate.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Outflow date should be greater than Rental Schedule date for Deferred Payment");
                blnResult = true;
            }
        }
        if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Trade Advance" || dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Normal Payment")
        {
            DataRow[] drOutflowDate = dtOutflow.Select("CashFlow_Flag_ID = 41 and Date <> #" + Utility.StringToDate(txtAccountDate.Text) + "#");
            if (drOutflowDate.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Outflow date should be equal to Rental Schedule date for Normal Payment/Trade Advance");
                blnResult = true;
            }
        }
        return blnResult;
    }

    protected void grvloanasset_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //if (grvloanasset.Rows.Count == 1)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Atleast one asset required')", true);
            //    return;
            //}
            //LinkButton lnkSelect = (LinkButton)((GridView)sender).Rows[e.RowIndex].FindControl("lnkAssetSerialNo");
            //System.Data.DataTable dtAssetDetails = (System.Data.DataTable)Session["ApplicationloanAssetDetails"];
            //DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + lnkSelect.Text);
            //drAsset[0].Delete();
            //dtAssetDetails.AcceptChanges();
            //DataRow[] drSerialAsset = dtAssetDetails.Select("SlNo > " + lnkSelect.Text);
            //foreach (DataRow dr in drSerialAsset)
            //{
            //    dr["SlNo"] = Convert.ToInt32(dr["SlNo"]) - 1;
            //    dr.AcceptChanges();
            //}
            //Session["ApplicationloanAssetDetails"] = dtAssetDetails;
            //grvloanasset.DataSource = (System.Data.DataTable)Session["ApplicationloanAssetDetails"];
            //grvloanasset.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Remove Asset";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void grvloanasset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    LinkButton LnkSelect = (LinkButton)e.Row.FindControl("lnkAssetSerialNo");
            //    string strNewPurchase = "";
            //    if (ddlLOB.SelectedValue != "0" || ddlLOB.SelectedValue != "")
            //    {
            //        if (ddlLOB.SelectedItem != null)//Added by Nataraj for Bug id 3702 part 3
            //        {
            //            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
            //            {
            //                strNewPurchase = "Yes";
            //            }
            //            else
            //            {
            //                strNewPurchase = "No";
            //            }
            //        }
            //        else
            //        {
            //            strNewPurchase = "No";
            //        }
            //    }
            //    else
            //    {
            //        strNewPurchase = "No";
            //    }
            //    if (intAccountCreationId > 0 || strSplitNum != null)
            //    {
            //        LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=M" + NewWinAttributes);
            //    }
            //    else
            //    {
            //        if (ViewState["IsBaseMLA"] != null)
            //        {
            //            if (ViewState["IsBaseMLA"].ToString() == "1" && (txtPrimeAccountNo.Text == "" || (txtPrimeAccountNo.Text != "" && txtStatus.Text.StartsWith("10"))))
            //            {
            //                if (strNewPurchase == "Yes")
            //                {
            //                    LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsLOB=O" + NewWinAttributes);
            //                }
            //                else
            //                {
            //                    LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=M" + NewWinAttributes);
            //                }
            //            }
            //            else
            //            {
            //                LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
            //            }
            //        }
            //        else
            //        {
            //            LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
            //        }

            //    }

            //}
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cv_TabMainPage.IsValid = false;
        }
    }

    #region "OPC"

    protected void btnDEVModalCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderApprover.Hide();
    }

    protected void btnDEVModalMove_Click(object sender, EventArgs e)
    {
        //ViewState["dtInvoiceGrid"] = dtInvoiceGrid;
        try
        {

            FunPriMoveInvoices();

        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to Move invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void btnDEVModalMoveAll_Click(object sender, EventArgs e)
    {
        //ViewState["dtInvoiceGrid"] = dtInvoiceGrid;
        try
        {
            FunPriBindGridTotal();

            FunPriMoveInvoices();

        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to Move invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void btnInvoices_Click(object sender, EventArgs e)
    {
        ddlFilterType.SelectedIndex = 0;
        txtFilterValue.Text = "";
        ViewState["dtInvoiceGrid"] = null;
        string strXML = string.Empty;

        strXML = FunPriGetAssetCatXML();


        if (string.IsNullOrEmpty(strXML))
        {
            Utility.FunShowAlertMsg(this, "select atleast one asset category to map invoices");
            return;
        }

        if (ddlRentalTDSSec.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this, "select Rental TDS Section");
            return;
        }

        //To check same tenure,rent frequency,Adv/Arr
        DataTable dtUniqRecords = new DataTable();
        if (ViewState["dtPrimaryGrid"] != null)
            dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];
        DataRow[] drprimary = dtPrimaryGrid.Select("Selected = 1 and Offer_Type = 1");
        if (drprimary.Length > 0)
        {
            string[] TobeDistinct = { "Tenure", "RentFrequencyID", "AdvanceArrearID" };
            dtUniqRecords = drprimary.CopyToDataTable().DefaultView.ToTable(true, TobeDistinct);
            if (dtUniqRecords.Rows.Count > 1)
            {
                Utility.FunShowAlertMsg(this, "Select the same Tenure/Rent Frequency/Adv or Arr combination in Primary grid");
                return;
            }
        }

        DataRow[] drScondary = dtPrimaryGrid.Select("Selected = 1 and Offer_Type = 2");
        if (drScondary.Length > 0)
        {
            string[] TobeDistinct = { "Tenure", "RentFrequencyID", "AdvanceArrearID" };
            dtUniqRecords = drScondary.CopyToDataTable().DefaultView.ToTable(true, TobeDistinct);
            if (dtUniqRecords.Rows.Count > 1)
            {
                Utility.FunShowAlertMsg(this, "Select the same Tenure/Rent Frequency/Adv or Arr combination in Secondary grid");
                return;
            }
        }


        //var distinctRows = (from DataRow dRow in dtPrimaryGrid.Rows
        //            select new col1=dRow["dataColumn1"],col2=dRow["dataColumn2"]}).Distinct();

        //if (!(chkLBT.Checked || chkPT.Checked || chkET.Checked || chkRC.Checked))
        //{
        //    Utility.FunShowAlertMsg(this, "select atleast one Additional tax to map invoices");
        //    return;
        //}

        ViewState["dtmovetotal"] = null;
        ViewState["strXML"] = strXML;
        FunPriBindGrid();
        ModalPopupExtenderApprover.Show();
    }

    private string FunPriGetAssetCatXML()
    {
        /*  string strXML = string.Empty;
          foreach (GridViewRow grv in grvPrimaryGrid.Rows)
          {
              CheckBox chkSelectPG = (CheckBox)grv.FindControl("chkSelectPG");
              Label lblAssetCategoryIDPG = (Label)grv.FindControl("lblAssetCategoryIDPG");
              TextBox txtResidualPerPG = (TextBox)grv.FindControl("txtResidualPerPG");
              if (chkSelectPG.Checked)
              {
                  strXML += "," + lblAssetCategoryIDPG.Text;
              }

          }
          foreach (GridViewRow grv in grvSecondaryGrid.Rows)
          {
              CheckBox chkSelectPG = (CheckBox)grv.FindControl("chkSelectSG");
              Label lblAssetCategoryIDPG = (Label)grv.FindControl("lblAssetCategoryIDSG");
              TextBox txtResidualPerG = (TextBox)grv.FindControl("txtResidualPerSG");
              if (chkSelectPG.Checked)
              {
                  strXML += "," + lblAssetCategoryIDPG.Text;
              }

          }
          if (strXML.Contains(","))
              strXML = strXML.Substring(1, strXML.Length - 1);
          return strXML;*/

        string strXML = string.Empty;
        if (ViewState["dtPrimaryGrid"] != null)
            dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];
        //START By siva on 14MA2015 for fixed rate 
        foreach (DataRow row in dtPrimaryGrid.Rows)
        {
            if (Convert.ToString(row["RentRate"]) == "")
            {
                row["RentRate"] = 0;
            }
            if (Convert.ToString(row["AMFRent"]) == "")
                row["AMFRent"] = 0;
            if (Convert.ToString(row["FixedRent"]) == "")
                row["FixedRent"] = 0;
            if (Convert.ToString(row["FixedAMF"]) == "")
                row["FixedAMF"] = 0;

        }
        //END By siva on 14MA2015 for fixed rate 
        dtPrimaryGrid.AcceptChanges();
        //Commented by gomathi based on NOV-17 2014 mail 
        DataRow[] drprimary = dtPrimaryGrid.Select("Selected=1");
        if (drprimary.Length > 0)
        {
            strXML = "<Root></Root>";
            dtPrimaryGrid = drprimary.CopyToDataTable();
            strXML = dtPrimaryGrid.FunPubFormXml(true);
        }
        return strXML;
    }

    protected void btnCalculateInvoices_Click(object sender, EventArgs e)
    {
        try
        {
            FunGenerateSchedule();
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to generate IRR";
            cv_TabMainPage.IsValid = false;

        }

    }

    private void FunGenerateSchedule()
    {
        try
        {
            string strPANumber = string.Empty;
            System.Data.DataTable dtRepaymentStructure = null;
            dtCashflows = null;
            dtInvoicesACAT = null;
            dtInvoicesACATSummary = null;
            ViewState["ErroCode"] = null;

            if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                strPANumber = txtPrimeAccountNo.Text;
            else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                strPANumber = ddlApplicationReferenceNo.SelectedText;

            string strXML = string.Empty;
            strXML = FunPriGetAssetCatXML();

            if (string.IsNullOrEmpty(strXML))
            {
                Utility.FunShowAlertMsg(this, "select atleast one asset category to map invoices");
                return;
            }

            if (ddlRentalTDSSec.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "select Rental TDS Section in Invoice Details");
                return;
            }

            ViewState["strXML"] = strXML;

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@PAnum", strPANumber);
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Pricing_Id", ddlApplicationReferenceNo.SelectedValue);
            Procparam.Add("@Delivery_State", ViewState["Parent_Location_ID"].ToString());
            Procparam.Add("@Billing_State", ddlBillingState.SelectedValue);
            Procparam.Add("@Tenure_Type", ddlTenureType.SelectedValue);
            //if (!string.IsNullOrEmpty(ddlSalesTax.Text))
            Procparam.Add("@SalesTax_Type", ViewState["intsales_type"].ToString());
            if (!string.IsNullOrEmpty(ddlSEZZone.SelectedValue))
                Procparam.Add("@SEZZone_Type", ddlSEZZone.SelectedValue);
            //if (!string.IsNullOrEmpty(txtAccountDate.Text))
            //    Procparam.Add("@RS_Date", Utility.StringToDate(txtAccountDate.Text).ToString());
            //if (!string.IsNullOrEmpty(txtfirstinstdate.Text))
            //    Procparam.Add("@Firstinst_Date", Utility.StringToDate(txtfirstinstdate.Text).ToString());
            if (!string.IsNullOrEmpty(txtfirstinstdate.Text))
                Procparam.Add("@RS_Date", Utility.StringToDate(txtfirstinstdate.Text).ToString());
            if (!string.IsNullOrEmpty(TxtFirstInstallDue.Text))
                Procparam.Add("@Firstinst_Date", Utility.StringToDate(TxtFirstInstallDue.Text).ToString());
            /**/
            string strPurTax = string.Empty;
            if (chkLBT.Checked)
                strPurTax += "+ISNULL(LBT_Amount,0)";
            if (chkPT.Checked)
                strPurTax += "+ISNULL(Purchase_Tax,0)";
            if (chkET.Checked)
                strPurTax += "+ISNULL(Entry_Tax_Amount,0)";
            if (chkRC.Checked)
                strPurTax += "+ISNULL(Reverse_Charge_Tax,0)";
            if (chkbxGSTRC.Checked)
                strPurTax += "+ISNULL(RC_SGST_Amt,0) + ISNULL(RC_CGST_Amt,0) + ISNULL(RC_IGST_Amt,0) + ISNULL(RC_SAC_SGST_Amt,0) + ISNULL(RC_SAC_CGST_Amt,0) + ISNULL(RC_SAC_IGST_Amt,0)";
            if (!string.IsNullOrEmpty(strPurTax))
                Procparam.Add("@PurTax", strPurTax);
            if (chkSEZA1.Checked)
                Procparam.Add("@SEZA1", "1");

            if (chkITC.Checked)
                Procparam.Add("@ITC_Req", "1");

            if (chkITC_Cap.Checked)
                Procparam.Add("@ITC_Cap", "1");

            //if (chkcstwith.Checked)
            //    Procparam.Add("@CST_C", "1");

            if (ViewState["strXML"] != null)
                Procparam.Add("@XMLAssetCategory", ViewState["strXML"].ToString());

            if (txtAddInvAmt.Text != "")
                Procparam.Add("@Reset_Amount", txtAddInvAmt.Text);
            else
                Procparam.Add("@Reset_Amount", "0");
            Procparam.Add("@Applicable", chkApplicable.Checked.ToString());
            Procparam.Add("@Applicable_Sec", chkApplicableSec.Checked.ToString());
            Procparam.Add("@FullRental", chkFullRental.Checked.ToString());
            Procparam.Add("@CSTDeal", chkCSTDeal.Checked.ToString());
            Procparam.Add("@IsSep_Amort", chkIsSecondary.Checked.ToString());

            if (chkWithIGST.Checked)
                Procparam.Add("@WithIGST", "1");

            if (strSplitNum != "")
                Procparam.Add("@Split_No", strSplitNum);

            if (strSplitRefNo != "")
                Procparam.Add("@SplitRefNo", strSplitRefNo);

            DataSet ds = Utility.GetDataset("S3G_GET_Invoices_RS_ACC", Procparam);

            System.Data.DataTable dtErrorCode = ds.Tables[0].Copy();
            if (dtErrorCode.Rows.Count > 0)
            {
                int intErroCode = Convert.ToInt32(dtErrorCode.Rows[0]["ErrorCode"]);
                if (intErroCode == 1)//Cashflow not defined
                {
                    Utility.FunShowAlertMsg(this, "Cashflow not defined for Installment/AMF.");
                    return;
                }
                else if (intErroCode == 2)//Tax guide not defined
                {
                    Utility.FunShowAlertMsg(this, "Tax guide not defined.");
                    return;
                }
                else if (intErroCode == 3)
                {
                    Utility.FunShowAlertMsg(this, "Does not match CST leasing criteria, please check.");
                    ViewState["ErroCode"] = "3";
                    return;
                }
                else if (intErroCode == 4)
                {
                    Utility.FunShowAlertMsg(this, "Secondary RV% should be lesser than or equal to Primary RV%.");
                    return;
                }
            }

            if (ds.Tables.Count > 1)
            {

                dtCashflows = ds.Tables[1].Copy();
                //dtInvoicesACAT = ds.Tables[2].Copy();
                dtInvoicesACATSummary = ds.Tables[2].Copy();

                //ViewState["dtInvoicesACAT"] = dtInvoicesACAT;
                //ViewState["dtInvoicesACATSummary"] = dtInvoicesACATSummary;

                ViewState["dtInvoicesACATSummary"] = dtInvoicesACATSummary;

                int intTenure = Convert.ToInt32(dtErrorCode.Rows[0]["Tenure"]);
                int intFrequency = Convert.ToInt32(dtErrorCode.Rows[0]["Frequency"]);
                int intTime_Value = Convert.ToInt32(dtErrorCode.Rows[0]["Time_Value"]);
                decimal decTotalSch_Amount = Convert.ToDecimal(dtErrorCode.Rows[0]["TotalSch_Amount"]);
                decimal decTotalRV_Amount = Convert.ToDecimal(dtErrorCode.Rows[0]["RV_Amount"]);
                decimal decFinance_Amount = Convert.ToDecimal(dtErrorCode.Rows[0]["Finance_Amount"]);
                decimal decSchedule_Amount_SD = Convert.ToDecimal(dtErrorCode.Rows[0]["Schedule_Amount_SD"]);

                decimal decTotalRVPri_Amount = Convert.ToDecimal(dtErrorCode.Rows[0]["RV_PriAmount"]);
                decimal decTotalRVSec_Amount = Convert.ToDecimal(dtErrorCode.Rows[0]["RV_SecAmount"]);
                int intPriTenure = Convert.ToInt32(dtErrorCode.Rows[0]["PriTenure"]);
                int intSecTenure = Convert.ToInt32(dtErrorCode.Rows[0]["SecTenure"]);

                if (ddlProposalType.SelectedValue == "3" || ddlProposalType.SelectedValue == "4" || ddlProposalType.SelectedValue == "5")
                {
                    ViewState["Balance_Principal"] = ds.Tables[3].Rows[0]["Balance_Principal"].ToString();
                    ViewState["ReWriteAmount"] = (DataTable)ds.Tables[4];
                    ViewState["RWInvoiceAmount"] = (DataTable)ds.Tables[5];
                }

                FunCalculateRepayment(dtCashflows, dtInvoicesACATSummary, Utility.StringToDate(txtfirstinstdate.Text), decTotalSch_Amount, decTotalRV_Amount,
                   intTenure, intFrequency, intTime_Value, decFinance_Amount, decSchedule_Amount_SD,
                   out DtRepayGrid, out dtRepaymentStructure, decTotalRVPri_Amount, decTotalRVSec_Amount, intPriTenure, intSecTenure);
            }

            // Added For Call Id : 4154 CR_56
            FunPriBindGridSummary();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("cannot calculate IRR"))
            {
                if (strSplitNum == null && strConsNumber == null)//For Consolidation & Split
                {
                    CV_IRR.ErrorMessage = "Incorrect cashflow details,cannot calculate IRR";
                }
                else
                {
                    CV_IRR.ErrorMessage = "Cannot calculate IRR,Re-enter Cashflow details";
                }
            }
            else
            {
                CV_IRR.ErrorMessage = "Due to Data Problem, Unable to Calculate IRR/ Generate Repayment Structure";
            }
            CV_IRR.IsValid = false;
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }


    private void FunCalculateSalesTax(System.Data.DataTable dtRepaymentStructure, out System.Data.DataTable dtRepaymentStructureST)
    {
        try
        {
            if (ViewState["dtInvoicesACATSummary"] != null)
                dtInvoicesACATSummary = (System.Data.DataTable)ViewState["dtInvoicesACATSummary"];

            foreach (DataRow dr in dtRepaymentStructure.Rows)
            {
                decimal decinstallmentAmount = Convert.ToDecimal(dr["InstallmentAmount"].ToString());

                int intinstallmentNo = Convert.ToInt32(dr["InstallmentNo"].ToString());
                decimal dectotTaxAmount = 0; decimal dectotSTAmount = 0;

                try
                {
                    dectotTaxAmount = (decimal)dtInvoicesACATSummary.Compute("sum(Sales_Tax)", "Install_No=" + intinstallmentNo.ToString());
                    //if (!chkSEZA1.Checked)//If A1 checked no need to caclulate service tax 
                    dectotSTAmount = (decimal)dtInvoicesACATSummary.Compute("sum(Service_Tax)", "Install_No=" + intinstallmentNo.ToString());
                }
                catch (Exception eX)
                {
                }

                if (dectotTaxAmount > 0)
                    dr["Tax"] = Convert.ToDecimal(dectotTaxAmount.ToString("0.00"));
                else
                    dr["Tax"] = 0.00;

                if (dectotSTAmount > 0)
                    dr["ServiceTax"] = Convert.ToDecimal(dectotSTAmount.ToString("0.00"));
                else
                    dr["ServiceTax"] = 0.00;
            }
            dtRepaymentStructure.AcceptChanges();


        }
        catch (Exception ex)
        {

        }
        dtRepaymentStructureST = dtRepaymentStructure.Copy();
    }


    private void FunCalculateRepayment(System.Data.DataTable dtCashflows, System.Data.DataTable dtINvoicesSummary, DateTime dtstartdate, decimal decTotalSchedule_Amount, decimal decTotalRV_Amount, int inttenure, int intRenFrequency, int inttimeValue
        , decimal decFinance_Amount, decimal decSchedule_Amount_SD, out System.Data.DataTable DtRepayGrid, out System.Data.DataTable dtRepaymentStructure, decimal decTotalRVPri_Amount, decimal decTotalRVSec_Amount, int intPritenure, int intSectenure)
    {
        DtRepayGrid = null;
        dtRepaymentStructure = null;
        try
        {
            hdnRentfrequency.Value = intRenFrequency.ToString();
            hdnTimeValue.Value = inttimeValue.ToString();

            if (dtINvoicesSummary.Rows.Count > 0)
            {
                decimal decTotalInterest_Amount = 0;
                decimal decTotalAMF_Amount = 0;

                decTotalInterest_Amount = (decimal)dtINvoicesSummary.Compute("sum(Interest)", "");
                decTotalAMF_Amount = (decimal)dtINvoicesSummary.Compute("sum(AMF)", "");
                DateTime dtFromDate = Utility.StringToDate(dtINvoicesSummary.Rows[0]["Install_date"].ToString());
                DateTime dtToDate = Utility.StringToDate(dtINvoicesSummary.Rows[dtINvoicesSummary.Rows.Count - 1]["Install_date"].ToString());

                //Commented and added on 09Jul2015 Starts here
                //txtFinanceAmount.Text = Math.Round(decTotalSchedule_Amount, 0).ToString();
                txtFinanceAmount.Text = Math.Round(decFinance_Amount, 0).ToString();
                //Commented and added on 09Jul2015 Ends here

                txtTenure.Text = inttenure.ToString();
                txtResidualAmt_Cashflow.Text = decTotalRV_Amount.ToString();
                if (ViewState["DtRepayGrid"] != null)
                    DtRepayGrid = ((System.Data.DataTable)ViewState["DtRepayGrid"]).Clone();

                int SlNo = 0;

                FunPriGetDtRepayGrid(dtCashflows, dtINvoicesSummary, "Interest", 0);//0--installment
                if (decTotalAMF_Amount > 0)
                    FunPriGetDtRepayGrid(dtCashflows, dtINvoicesSummary, "AMF", 1);//AMF

                /* var queryInterest = (from row in dtINvoicesSummary.AsEnumerable()
                                      group row by new
                                      {
                                          InstallmentAmount = row.Field<decimal>("Interest"),
                                      } into grp

                                      select new
                                      {
                                          PerInstall = grp.Key.InstallmentAmount,
                                          Amount = grp.Select(r => r.Field<decimal>("Interest")),
                                          Breakup = 0,
                                          FromInstall = grp.Min(r => r.Field<int>("Install_No")),
                                          ToInstall = grp.Max(r => r.Field<int>("Install_No")),
                                          TotalPeriodInstall = grp.Sum(r => Convert.ToInt32(r.Field<decimal>("Interest"))),
                                          FromDate = grp.Min(r => r.Field<DateTime>("Install_date")),
                                          ToDate = grp.Max(r => r.Field<DateTime>("Install_date"))
                                      }).ToList();


                 var queryAMF = (from row in dtINvoicesSummary.AsEnumerable()
                                 group row by new
                                 {
                                     InstallmentAmount = row.Field<decimal>("AMF"),
                                 } into grp

                                 select new
                                 {
                                     PerInstall = grp.Key.InstallmentAmount,
                                     Amount = grp.Select(r => r.Field<decimal>("AMF")),
                                     Breakup = 0,
                                     FromInstall = grp.Min(r => r.Field<int>("Install_No")),
                                     ToInstall = grp.Max(r => r.Field<int>("Install_No")),
                                     TotalPeriodInstall = grp.Sum(r => Convert.ToInt32(r.Field<decimal>("AMF"))),
                                     FromDate = grp.Min(r => r.Field<DateTime>("Install_date")),
                                     ToDate = grp.Max(r => r.Field<DateTime>("Install_date"))

                                 }).ToList();

                 foreach (var x in queryInterest)
                 {
                     SlNo++;
                     drRepayRow = DtRepayGrid.NewRow();
                     drRepayRow["slno"] = SlNo.ToString();
                     drRepayRow["Amount"] = Convert.ToDecimal(x.Amount.ToArray()[x.Amount.ToArray().Length - 1].ToString());
                     drRepayRow["PerInstall"] = x.PerInstall;
                     drRepayRow["Breakup"] = Convert.ToDecimal(x.Breakup);
                     drRepayRow["FromInstall"] = x.FromInstall.ToString();
                     drRepayRow["ToInstall"] = x.ToInstall.ToString();
                     drRepayRow["FromDate"] = x.FromDate.ToString();
                     drRepayRow["ToDate"] = x.ToDate.ToString();
                     drRepayRow["TotalPeriodInstall"] = x.TotalPeriodInstall;
                     drRepayRow["CashFlow"] = dtCashflows.Rows[0]["CashFlow_Description"].ToString();
                     drRepayRow["CashFlowId"] = dtCashflows.Rows[0]["CashFlow_ID"].ToString();
                     drRepayRow["Accounting_IRR"] = dtCashflows.Rows[0]["Accounting_IRR"].ToString();
                     drRepayRow["Business_IRR"] = dtCashflows.Rows[0]["Business_IRR"].ToString();
                     drRepayRow["Company_IRR"] = dtCashflows.Rows[0]["Company_IRR"].ToString();
                     drRepayRow["CashFlow_Flag_ID"] = Convert.ToInt32(dtCashflows.Rows[0]["CashFlow_Flag_ID"]);
                     DtRepayGrid.Rows.Add(drRepayRow);
                 }
                 foreach (var x in queryAMF)
                 {
                     SlNo++;
                     drRepayRow = DtRepayGrid.NewRow();
                     drRepayRow["slno"] = SlNo.ToString();
                     drRepayRow["Amount"] = Convert.ToDecimal(x.Amount.ToArray()[x.Amount.ToArray().Length - 1].ToString());
                     drRepayRow["PerInstall"] = x.PerInstall;
                     drRepayRow["Breakup"] = Convert.ToDecimal(x.Breakup);
                     drRepayRow["FromInstall"] = x.FromInstall.ToString();
                     drRepayRow["ToInstall"] = x.ToInstall.ToString();
                     drRepayRow["FromDate"] = x.FromDate.ToString();
                     drRepayRow["ToDate"] = x.ToDate.ToString();
                     drRepayRow["TotalPeriodInstall"] = x.TotalPeriodInstall;
                     drRepayRow["CashFlow"] = dtCashflows.Rows[1]["CashFlow_Description"].ToString();
                     drRepayRow["CashFlowId"] = dtCashflows.Rows[1]["CashFlow_ID"].ToString();
                     drRepayRow["Accounting_IRR"] = dtCashflows.Rows[1]["Accounting_IRR"].ToString();
                     drRepayRow["Business_IRR"] = dtCashflows.Rows[1]["Business_IRR"].ToString();
                     drRepayRow["Company_IRR"] = dtCashflows.Rows[1]["Company_IRR"].ToString();
                     drRepayRow["CashFlow_Flag_ID"] = Convert.ToInt32(dtCashflows.Rows[1]["CashFlow_Flag_ID"]);
                     DtRepayGrid.Rows.Add(drRepayRow);
                 }

                 */

                ViewState["DtRepayGrid"] = DtRepayGrid;
                decimal decamfamount = 0;
                DataRow[] dramf = DtRepayGrid.Select("CashFlow_Flag_Id=105");
                if (dramf.Length > 0)
                {
                    decamfamount = (decimal)DtRepayGrid.Compute("sum(TotalPeriodInstall)", "CashFlow_Flag_ID =105");
                }

                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();

                DtRepayGridIRR = DtRepayGrid.Copy();

                // Code changed by Chandru K On 16-Mar-2016

                //DataRow[] drr = DtRepayGridIRR.Select("CashFlow_Flag_Id=105");

                //foreach (DataRow dr in drr)
                //{
                //    decimal amf_amount = Convert.ToDecimal(dr["amount"].ToString());
                //    decimal peramf_amount = Convert.ToDecimal(dr["PerInstall"].ToString());
                //    string from_instal = dr["FromInstall"].ToString();
                //    DataRow[] dr1 = DtRepayGridIRR.Select("FromInstall = " + from_instal + " and CashFlow_Flag_Id=23");
                //    foreach (DataRow drinst in dr1)
                //    {
                //        decimal total_value = amf_amount + Convert.ToDecimal(drinst["amount"].ToString());
                //        decimal pertotal_value = peramf_amount + Convert.ToDecimal(drinst["PerInstall"].ToString());
                //        drinst["amount"] = total_value.ToString();
                //        drinst["PerInstall"] = pertotal_value.ToString();
                //        DtRepayGridIRR.Rows.Remove(dr);
                //        DtRepayGridIRR.AcceptChanges();
                //    }

                //}
                //ViewState["DtRepayGridIRR"] = DtRepayGridIRR;

                //decTotalSchedule_Amount = decTotalSchedule_Amount + decamfamount;

                DataTable DtIRR = new DataTable();
                DataTable DtIRRPri = new DataTable();
                DataTable DtIRRSec = new DataTable();
                DtIRR = DtRepayGridIRR.Clone();
                DtIRRPri = DtRepayGridIRR.Clone();
                DtIRRSec = DtRepayGridIRR.Clone();

                DataRow[] rw = DtRepayGridIRR.Select("CashFlow_Flag_Id=23");

                decimal totalAmt1 = Convert.ToDecimal(dtINvoicesSummary.Rows[0]["Interest"].ToString()) + Convert.ToDecimal(dtINvoicesSummary.Rows[0]["AMF"].ToString());
                rw[0]["FromInstall"] = dtINvoicesSummary.Rows[0]["Install_No"].ToString();
                rw[0]["FromDate"] = dtINvoicesSummary.Rows[0]["Install_date"].ToString();
                rw[0]["amount"] = totalAmt1.ToString();
                rw[0]["PerInstall"] = totalAmt1.ToString();

                DtIRR.ImportRow(rw[0]);

                decimal totalIntAmt = 0;
                int icount = 0;

                for (int i = 0; i < dtINvoicesSummary.Rows.Count; i++)
                {
                    decimal totalAmt2 = Convert.ToDecimal(dtINvoicesSummary.Rows[i]["Interest"].ToString()) + Convert.ToDecimal(dtINvoicesSummary.Rows[i]["AMF"].ToString());
                    totalIntAmt = totalIntAmt + totalAmt1;
                    if (totalAmt1 != totalAmt2)
                    {
                        DtIRR.Rows[icount]["ToInstall"] = dtINvoicesSummary.Rows[i - 1]["Install_No"].ToString();
                        DtIRR.Rows[icount]["ToDate"] = dtINvoicesSummary.Rows[i - 1]["Install_date"].ToString();
                        DtIRR.Rows[icount]["TotalPeriodInstall"] = totalIntAmt.ToString();
                        DtIRR.AcceptChanges();

                        icount = icount + 1;

                        rw[0]["FromInstall"] = dtINvoicesSummary.Rows[i]["Install_No"].ToString();
                        rw[0]["FromDate"] = dtINvoicesSummary.Rows[i]["Install_date"].ToString();
                        rw[0]["amount"] = totalAmt2.ToString();
                        rw[0]["PerInstall"] = totalAmt2.ToString();

                        DtIRR.ImportRow(rw[0]);

                        totalAmt1 = totalAmt2;
                        totalIntAmt = 0;
                    }
                }

                DtIRR.Rows[icount]["ToInstall"] = dtINvoicesSummary.Rows[dtINvoicesSummary.Rows.Count - 1]["Install_No"].ToString();
                DtIRR.Rows[icount]["ToDate"] = dtINvoicesSummary.Rows[dtINvoicesSummary.Rows.Count - 1]["Install_date"].ToString();
                DtIRR.Rows[icount]["TotalPeriodInstall"] = totalIntAmt.ToString();
                DtIRR.AcceptChanges();

                ViewState["DtRepayGridIRR"] = DtIRR;

                // End Code Chandru K On 16-Mar-2016

                // Change  [Request ID :##5708##] : FW: OPC_CR_56 Start
                if (chkIsSecondary.Checked)
                {
                    // Primary Amort Split
                    DataRow[] rwPri = DtRepayGridIRR.Select("CashFlow_Flag_Id=23");
                    decimal totalPriAmt = Convert.ToDecimal(dtINvoicesSummary.Rows[0]["Interest"].ToString()) + Convert.ToDecimal(dtINvoicesSummary.Rows[0]["AMF"].ToString());
                    rwPri[0]["FromInstall"] = dtINvoicesSummary.Rows[0]["Install_No"].ToString();
                    rwPri[0]["FromDate"] = dtINvoicesSummary.Rows[0]["Install_date"].ToString();
                    rwPri[0]["amount"] = totalPriAmt.ToString();
                    rwPri[0]["PerInstall"] = totalPriAmt.ToString();
                    DtIRRPri.ImportRow(rw[0]);
                    DtIRRPri.Rows[icount]["ToInstall"] = dtINvoicesSummary.Rows[intPritenure - 1]["Install_No"].ToString();
                    DtIRRPri.Rows[icount]["ToDate"] = dtINvoicesSummary.Rows[intPritenure - 1]["Install_date"].ToString();
                    DtIRRPri.Rows[icount]["TotalPeriodInstall"] = (totalPriAmt * intPritenure).ToString();
                    DtIRRPri.AcceptChanges();
                    ViewState["DtRepayGridIRRPri"] = DtIRRPri;
                    // Secondary Amort Split
                    DataRow[] rwSec = DtRepayGridIRR.Select("CashFlow_Flag_Id=23");
                    decimal totalSecAmt = Convert.ToDecimal(dtINvoicesSummary.Rows[intPritenure]["Interest"].ToString()) + Convert.ToDecimal(dtINvoicesSummary.Rows[0]["AMF"].ToString());
                    rwSec[0]["FromInstall"] = dtINvoicesSummary.Rows[intPritenure]["Install_No"].ToString();
                    rwSec[0]["FromDate"] = dtINvoicesSummary.Rows[intPritenure]["Install_date"].ToString();
                    rwSec[0]["amount"] = totalSecAmt.ToString();
                    rwSec[0]["PerInstall"] = totalSecAmt.ToString();
                    DtIRRSec.ImportRow(rw[0]);
                    DtIRRSec.Rows[icount]["ToInstall"] = dtINvoicesSummary.Rows[intSectenure - 1]["Install_No"].ToString();
                    DtIRRSec.Rows[icount]["ToDate"] = dtINvoicesSummary.Rows[intSectenure - 1]["Install_date"].ToString();
                    DtIRRSec.Rows[icount]["TotalPeriodInstall"] = (totalSecAmt * (intSectenure - intPritenure)).ToString();
                    DtIRRSec.AcceptChanges();
                    ViewState["DtRepayGridIRRSec"] = DtIRRSec;
                }
                // Change  [Request ID :##5708##] : FW: OPC_CR_56 End

                FunCalculateIRROPC(out dtRepaymentStructure, decTotalRV_Amount, decTotalSchedule_Amount, decSchedule_Amount_SD, inttenure, dtstartdate.ToString(), decTotalRVPri_Amount, decTotalRVSec_Amount, intPritenure, intSectenure);

            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("cannot calculate IRR"))
            {
                if (strSplitNum == null && strConsNumber == null)//For Consolidation & Split
                {
                    CV_IRR.ErrorMessage = "Incorrect cashflow details,cannot calculate IRR";
                }
                else
                {
                    CV_IRR.ErrorMessage = "Cannot calculate IRR,Re-enter Cashflow details";
                }
            }
            else
            {
                CV_IRR.ErrorMessage = "Due to Data Problem, Unable to Calculate IRR/ Generate Repayment Structure";
            }
            CV_IRR.IsValid = false;
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriGetDtRepayGrid(System.Data.DataTable dtCashflows, System.Data.DataTable dtINvoicesSummary, string strColumnName, int intType)
    {
        try
        {
            DataRow drRepayRow;


            int counter = 1;
            int iCounter = 1;
            decimal iCurAmt = 0;
            int iToPeriod = 0;
            decimal iPrvAmt = 0;
            int iFromPeriod = 0;

            foreach (DataRow grvReapyRow in dtINvoicesSummary.Rows)
            {
                if (iCounter == counter)
                {
                    int i = 1;
                    foreach (DataRow grvNewReapyRow in dtINvoicesSummary.Rows)
                    {

                        if (iCounter == 1)
                        {
                            //if(!Int64.TryParse(grvNewReapyRow["InstallmentAmount"].ToString(),out iCurAmt))
                            iCurAmt = Convert.ToDecimal(Convert.ToDecimal(grvNewReapyRow[strColumnName]).ToString());
                            iPrvAmt = iCurAmt;
                            iFromPeriod = 1;
                            iToPeriod = 1;
                        }
                        else
                        {
                            if (iCounter == i)
                            {
                                //if (!Int64.TryParse(grvNewReapyRow["InstallmentAmount"].ToString(), out iCurAmt))
                                iCurAmt = Convert.ToDecimal(Convert.ToDecimal(grvNewReapyRow[strColumnName]).ToString());
                                if (iCurAmt != iPrvAmt)
                                {
                                    goto L1;
                                }
                                else
                                {
                                    iToPeriod = iToPeriod + 1;
                                }
                            }
                            else
                            {
                                goto L2;
                            }
                        }
                        iCounter = iCounter + 1;
                    L2: ++i;
                    }

                L1: drRepayRow = DtRepayGrid.NewRow();
                    drRepayRow["slno"] = "1";
                    drRepayRow["Amount"] = Convert.ToDecimal(Convert.ToDecimal(dtINvoicesSummary.Rows[0][strColumnName]).ToString());
                    //if (GPSRoundOff == 0)
                    //{
                    //    drRepayRow["PerInstall"] = Math.Round(iPrvAmt, 2);
                    //}
                    //else
                    //{
                    drRepayRow["PerInstall"] = iPrvAmt;
                    //}
                    drRepayRow["Breakup"] = 0;
                    drRepayRow["FromInstall"] = iFromPeriod;
                    drRepayRow["ToInstall"] = iToPeriod;
                    if (dtINvoicesSummary.Rows.Count > 1)
                    {
                        if (counter < dtINvoicesSummary.Rows.Count)
                        {
                            //if (dtRepaymentDetails.Rows[counter]["FromDate"].ToString() == dtRepaymentDetails.Rows[counter - 1]["ToDate"].ToString())
                            drRepayRow["FromDate"] = dtINvoicesSummary.Rows[counter - 1]["Install_date"].ToString();
                            //else //for moratorium
                            //    drRepayRow["FromDate"] = dtRepaymentDetails.Rows[counter - 1]["FromDate"].ToString();
                        }
                        else if (counter == dtINvoicesSummary.Rows.Count)
                            drRepayRow["FromDate"] = dtINvoicesSummary.Rows[counter - 2]["Install_date"].ToString();
                    }
                    else
                    {
                        drRepayRow["FromDate"] = dtINvoicesSummary.Rows[0]["Install_date"].ToString();
                    }

                    drRepayRow["ToDate"] = dtINvoicesSummary.Rows[iCounter - 2]["Install_date"].ToString();
                    //drRepayRow["TotalPeriodInstall"] = dtRepaymentDetails.Rows[counter]["Amount"].ToString();
                    int intTotalInstall = iToPeriod - iFromPeriod + 1;
                    //if (GPSRoundOff == 0)
                    //{
                    //    drRepayRow["TotalPeriodInstall"] = Math.Round(iPrvAmt, 2) * intTotalInstall;
                    //}
                    //else
                    //{
                    drRepayRow["TotalPeriodInstall"] = iPrvAmt * intTotalInstall;
                    //}

                    drRepayRow["CashFlow"] = dtCashflows.Rows[intType]["CashFlow_Description"].ToString();
                    drRepayRow["CashFlowId"] = dtCashflows.Rows[intType]["CashFlow_ID"].ToString();
                    drRepayRow["Accounting_IRR"] = dtCashflows.Rows[intType]["Accounting_IRR"].ToString();
                    drRepayRow["Business_IRR"] = dtCashflows.Rows[intType]["Business_IRR"].ToString();
                    drRepayRow["Company_IRR"] = dtCashflows.Rows[intType]["Company_IRR"].ToString();
                    drRepayRow["CashFlow_Flag_ID"] = Convert.ToInt32(dtCashflows.Rows[intType]["CashFlow_Flag_ID"]);
                    DtRepayGrid.Rows.Add(drRepayRow);
                    iPrvAmt = iCurAmt;
                    iFromPeriod = iCounter;
                    iToPeriod = iCounter - 1;
                }
                ++counter;
            }
            ViewState["DtRepayGrid"] = DtRepayGrid;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("cannot calculate IRR"))
            {
                if (strSplitNum == null && strConsNumber == null)//For Consolidation & Split
                {
                    CV_IRR.ErrorMessage = "Incorrect cashflow details,cannot calculate IRR";
                }
                else
                {
                    CV_IRR.ErrorMessage = "Cannot calculate IRR,Re-enter Cashflow details";
                }
            }
            else
            {
                CV_IRR.ErrorMessage = "Due to Data Problem, Unable to Calculate IRR/ Generate Repayment Structure";
            }
            CV_IRR.IsValid = false;
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }



    protected void btnGoInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGrid();
        }
        catch (Exception ex)
        {
            cvPopUP.ErrorMessage = "Due to Data Problem, Unable to Load the Invoices.";
            cvPopUP.IsValid = false;
        }
    }


    protected void grvInvoices_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //CheckBox chkSelected = (CheckBox)e.Row.FindControl("chkSelected");
                //Label lblInvoice_Type = (Label)e.Row.FindControl("lblInvoice_Type");
                //Label lblInvoiceId = (Label)e.Row.FindControl("lblInvoiceId");

                CheckBox chkSelected = ((CheckBox)e.Item.FindControl("chkSelected"));
                Label lblInvoice_Type = ((Label)e.Item.FindControl("lblInvoice_Type"));
                Label lblInvoiceId = ((Label)e.Item.FindControl("lblInvoiceId"));

                if (ViewState["dtInvoiceGrid"] != null)
                    dtInvoiceGrid = (System.Data.DataTable)ViewState["dtInvoiceGrid"];
                if (dtInvoiceGrid.Rows.Count > 0)
                {
                    DataRow[] drInvoiceGrid = dtInvoiceGrid.Select("Invoice_Type='" + lblInvoice_Type.Text + "' and Invoice_Id=" + lblInvoiceId.Text + "");
                    if (drInvoiceGrid.Length > 0)
                    {
                        chkSelected.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cv_TabMainPage.IsValid = false;
        }
    }



    protected void ddlApplicationReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedValue))
            {
                FunPriLoadProposalDtls(ddlApplicationReferenceNo.SelectedValue);
            }
            if (strMode != "M")
            {
                //opc0195 start
                ddlDeliveryState.SelectedValue = "0";
                //opc0195 end
            }
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Due to Data Problem, Unable to Load the Rental Schedule Creation.";
            cv_TabMainPage.IsValid = false;
        }

    }


    /// <summary>
    /// this method is used to move invoices from pop up to summary grid
    /// </summary>
    private void FunPriMoveInvoices()
    {
        ContractMgtServicesReference.ContractMgtServicesClient objContractMgtServicesClient = new ContractMgtServicesReference.ContractMgtServicesClient();
        try
        {
            txtBusinessIRR_Repay.Text = string.Empty;
            if (ViewState["dtmovetotal"] != null)
            {
                dtInvoiceGrid = (System.Data.DataTable)ViewState["dtmovetotal"];
            }
            if (ViewState["dtInvoiceGrid"] != null)
                dtInvoiceGrid = (System.Data.DataTable)ViewState["dtInvoiceGrid"];
            if (dtInvoiceGrid.Rows.Count > 0)
            {
                string strPANumber = string.Empty;
                if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                    strPANumber = txtPrimeAccountNo.Text;
                else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                    strPANumber = ddlApplicationReferenceNo.SelectedText;
                string strInvoiceXML = dtInvoiceGrid.FunPubFormXml(true);
                AccountCreationEntity objAccountCreationEntity = new AccountCreationEntity();
                objAccountCreationEntity.intCompanyId = intCompanyId;
                objAccountCreationEntity.strPANumber = strPANumber;
                //This is used to send purchase tax applicable
                string strPurTax = string.Empty;
                if (chkLBT.Checked)
                    strPurTax += "+ISNULL(LBT_Amount,0)";
                if (chkPT.Checked)
                    strPurTax += "+ISNULL(Purchase_Tax,0)";
                if (chkET.Checked)
                    strPurTax += "+ISNULL(Entry_Tax_Amount,0)";
                if (chkRC.Checked)
                    strPurTax += "+ISNULL(Reverse_Charge_Tax,0)";
                if (chkbxGSTRC.Checked)
                    strPurTax += "+ISNULL(RC_SGST_Amt,0) + ISNULL(RC_CGST_Amt,0) + ISNULL(RC_IGST_Amt,0) + ISNULL(RC_SAC_SGST_Amt,0) + ISNULL(RC_SAC_CGST_Amt,0) + ISNULL(RC_SAC_IGST_Amt,0)";
                //if (chkITC.Checked)
                //    strPurTax += "+ISNULL(-ITC_Value,0)";

                if (!string.IsNullOrEmpty(strPurTax))
                {
                    if (strPurTax.Contains("+"))
                        strPurTax = strPurTax.Substring(1, strPurTax.Length - 1);
                    objAccountCreationEntity.strSANum = strPurTax;
                }
                if (!string.IsNullOrEmpty(strInvoiceXML))
                    objAccountCreationEntity.XmlInvoiceDetails = strInvoiceXML;
                else
                    objAccountCreationEntity.XmlInvoiceDetails = "<Root></Root>";

                if (chkITC.Checked)
                    objAccountCreationEntity.ITC_Req = 1;
                else
                    objAccountCreationEntity.ITC_Req = 0;

                objAccountCreationEntity.intUserId = intUserId;
                objAccountCreationEntity.Delivery_State = Convert.ToInt32(ddlDeliveryState.SelectedValue);
                //Added by Sathiyanathan on 24Jun2015 for OPC starts here
                objAccountCreationEntity.strMode = Convert.ToString(strMode);
                objAccountCreationEntity.strSession = Convert.ToString(Request.Cookies["CookSession_ID"].Value);
                //Added by Sathiyanathan on 24Jun2015 for OPC ends here
                //Added by Sathiyanathan on 04Jul2015 for OPC starts here
                objAccountCreationEntity.Proposal_Type = Convert.ToInt32(ddlProposalType.SelectedValue);
                //Added by Sathiyanathan on 04Jul2015 for OPC ends here
                int intResult = objContractMgtServicesClient.FunPubMoveTempInvoices(objAccountCreationEntity);
                if (intResult == 0)
                {
                    //Utility.FunShowAlertMsg(this, "Invoices Moved sucessfully.");
                    ViewState["dtInvoiceGrid"] = null;


                    //DataSet ds=Utility.GetDefaultData
                    //FunPriBindGridSummary();

                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Due to Data Problem,Unable to Move Invoices.");
                    return;
                }
            }
            FunPriBindGridSummary();

            FunPriGetMappedAmount();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Move Invoices.";
            cv_TabMainPage.IsValid = false;
        }
        finally
        {
            if (objContractMgtServicesClient != null)
                objContractMgtServicesClient.Close();
        }
        ModalPopupExtenderApprover.Hide();
    }

    private void FunPriClearPRoposalDtls()
    {
        try
        {
            S3GCustomerAddress1.ClearCustomerDetails();
            txtProposalNumber.Text = txtOfferDate.Text = txtProcessingFee.Text = txtRemarks.Text = txtMRANo.Text = txtMRADate.Text =
       txtOneTimeFee.Text = txtTotalFacilityAmount.Text = txtSecDepAdvRent.Text = txtOfferValidTill.Text = string.Empty;
            lblRoundNo.Text = "0";
            ddlProposalType.SelectedIndex = -1;
            ddlSecuritydeposit.SelectedIndex = -1;
            ddlReturnPattern.SelectedIndex = -1;
            ViewState["dtPrimaryGrid"] = null;
            grvPrimaryGrid.DataSource = null;
            grvPrimaryGrid.DataBind();
            ViewState["dtSecondaryGrid"] = null;
            grvSecondaryGrid.DataSource = null;
            grvSecondaryGrid.DataBind();
            FunPriClearEUC();
            ViewState["dtEUCDetails"] = null;
            grvEUC.DataSource = null;
            grvEUC.DataBind();
            ddlAccountManager1.SelectedValue = ddlAccountManager2.SelectedValue = ddlRegionalManager.SelectedValue = "0";
            ddlAccountManager1.Clear(); ddlAccountManager2.Clear(); ddlRegionalManager.Clear();
            ddlPaymentRuleList.Items.Clear();
            chkPT.Checked =
            chkET.Checked =
            chkRC.Checked =
            chkbxGSTRC.Checked =
            chkLBT.Checked =
            chkSEZ.Checked =
            chkSEZA1.Checked =
            chkcstwith.Checked = chkCSTDeal.Checked = chkIsSecondary.Checked = false;
        }
        catch (Exception ex)
        {

        }
    }

    private void FunPriLoadRV_SeparateSecAmort()
    {
        try
        {

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Pricing_Id", ddlApplicationReferenceNo.SelectedValue);
            Procparam.Add("@location_id", ddlBranchList.SelectedValue);
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            if (chkIsSecondary.Checked)
                Procparam.Add("@IsSep_Amort", "1");
            DataTable dtRV = Utility.GetDefaultData("S3G_LAD_Get_RV_SeparateSecAmort", Procparam);

            DataRow[] drprim = dtRV.Select("Offer_Type=1");
            
            if (drprim.Length > 0)
            {
                dtPrimaryGrid = drprim.CopyToDataTable();
                FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
            }

            DataRow[] drsec = dtRV.Select("Offer_Type=2");
            if (drsec.Length > 0)
            {
                dtSecondaryGrid = drsec.CopyToDataTable();
                FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
            }

            ViewState["dtPrimaryGrid"] = dtRV;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadProposalDtls(string strPricingId)
    {
        try
        {

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Pricing_Id", strPricingId);
            Procparam.Add("@location_id", ddlBranchList.SelectedValue);
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            DataSet DS = Utility.GetDataset("S3G_ORG_Get_Proposal_Dtls_ACC", Procparam);

            System.Data.DataTable dtProposal = DS.Tables[0].Copy();
            System.Data.DataTable dtcustomerAddress = DS.Tables[3].Copy();

            if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Offer_Valid_Till"].ToString()))
            {
                txtOfferValidTill.Text = dtProposal.Rows[0]["Offer_Valid_Till"].ToString();
                if (Utility.StringToDate(DateTime.Now.Date.ToString(strDateFormat)) > Utility.StringToDate(txtOfferValidTill.Text))
                {
                    ddlApplicationReferenceNo.SelectedText = "";
                    ddlApplicationReferenceNo.SelectedValue = "0";
                    Utility.FunShowAlertMsg(this, "Proposal Valid Till date is expired");
                    return;
                }
            }
            //Clear Controls
            FunPriClearPRoposalDtls();
            #region "Header Part"
            if (dtProposal.Rows.Count > 0)
            {
                ViewState["Customer_Id"] = hdnCustomerId.Value = Convert.ToString(dtProposal.Rows[0]["Customer_Id"]);
                System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = Convert.ToString(dtProposal.Rows[0]["Customer_Id"]);//By Siva.K For Data Load Problem in Multi User
                if (dtcustomerAddress.Rows.Count > 0)
                {
                    TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    txtName.Text = txtCustomerCode.Text = dtcustomerAddress.Rows[0]["Customer_Code"].ToString();

                    S3GCustomerAddress1.SetCustomerDetails(dtcustomerAddress.Rows[0]["Customer_Code"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_Address1"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_Address2"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_city"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_state"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_country"].ToString() + "\n" +
                    dtcustomerAddress.Rows[0]["comm_pincode"].ToString(),
                    dtcustomerAddress.Rows[0]["Customer_Name"].ToString(),
                    dtcustomerAddress.Rows[0]["Comm_Telephone"].ToString(),
                    dtcustomerAddress.Rows[0]["Comm_mobile"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_email"].ToString(),
                    dtcustomerAddress.Rows[0]["comm_website"].ToString());

                }



                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Business_Offer_Number"].ToString()))
                    txtProposalNumber.Text = dtProposal.Rows[0]["Business_Offer_Number"].ToString();

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Offer_Date"].ToString()))
                    txtOfferDate.Text = dtProposal.Rows[0]["Offer_Date"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Facility_Amount"].ToString()))
                    txtTotalFacilityAmount.Text = dtProposal.Rows[0]["Facility_Amount"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Offer_Valid_Till"].ToString()))
                    txtOfferValidTill.Text = dtProposal.Rows[0]["Offer_Valid_Till"].ToString();
                //ddlBranchList.SelectedValue = dtProposal.Rows[0]["Location_ID"].ToString();
                //ddlBranchList.SelectedText = dtProposal.Rows[0]["Location"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Proposal_Type"].ToString()))
                    ddlProposalType.SelectedValue = dtProposal.Rows[0]["Proposal_Type"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Adv_Rent_Applicability"].ToString()))
                    RBLAdvanceRent.SelectedValue = dtProposal.Rows[0]["Adv_Rent_Applicability"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Seco_Term_Applicability"].ToString()))
                    RBLSecondaryTerm.SelectedValue = dtProposal.Rows[0]["Seco_Term_Applicability"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Secu_Deposit_Type"].ToString()))
                    ddlSecuritydeposit.SelectedValue = dtProposal.Rows[0]["Secu_Deposit_Type"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Secu_Rent_Amount"].ToString()))
                    txtSecDepAdvRent.Text = dtProposal.Rows[0]["Secu_Rent_Amount"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["ReturnPattern"].ToString()))
                    ddlReturnPattern.SelectedValue = dtProposal.Rows[0]["ReturnPattern"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["One_Time_Fee"].ToString()))
                    txtOneTimeFee.Text = dtProposal.Rows[0]["One_Time_Fee"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Processing_Fee_Per"].ToString()))
                    txtProcessingFee.Text = dtProposal.Rows[0]["Processing_Fee_Per"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Repayment_Mode"].ToString()))
                    RBLStructuredEI.SelectedValue = dtProposal.Rows[0]["Repayment_Mode"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["VAT_Rebate_Applicability"].ToString()))
                    RBLVATRebate.SelectedValue = dtProposal.Rows[0]["VAT_Rebate_Applicability"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Remarks"].ToString()))
                    txtRemarks.Text = dtProposal.Rows[0]["Remarks"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Round_No"].ToString()))
                    lblRoundNo.Text = dtProposal.Rows[0]["Round_No"].ToString();
                else
                    lblRoundNo.Text = "0";

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["LeadRefNo"].ToString()))
                    txtLeadRefNo.Text = dtProposal.Rows[0]["LeadRefNo"].ToString();

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["PROCESSING_FEE_RATE"].ToString()))
                    txtProcessingfeeRate.Text = dtProposal.Rows[0]["PROCESSING_FEE_RATE"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["ONE_TIME_RATE"].ToString()))
                    txtOneTimeRate.Text = dtProposal.Rows[0]["ONE_TIME_RATE"].ToString();

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["One_Time_BasedOn"].ToString()))
                    ddlAmtBasedOn.SelectedValue = dtProposal.Rows[0]["One_Time_BasedOn"].ToString();

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["PROCESSING_AMT_BASED_ON"].ToString()))
                    ddlProcessingBasedOn.Text = dtProposal.Rows[0]["PROCESSING_AMT_BASED_ON"].ToString();

                ViewState["Status_ID"] = dtProposal.Rows[0]["Status_ID"].ToString();
                ViewState["Parent_Location_ID"] = dtProposal.Rows[0]["Parent_Location_ID"].ToString();

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Payment_Rule_ID"].ToString()))
                {
                    ListItem lstItem;
                    lstItem = new ListItem(dtProposal.Rows[0]["Payment_Rule_No"].ToString(), dtProposal.Rows[0]["Payment_Rule_ID"].ToString());
                    ddlPaymentRuleList.Items.Add(lstItem);
                    ddlPaymentRuleList.SelectedValue = dtProposal.Rows[0]["Payment_Rule_ID"].ToString();
                    Load_Payment_Rule();

                }
                else
                {
                    System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                    ddlPaymentRuleList.Items.Insert(0, liSelect1);
                }

                FunPriSetProposalType();

                //MRA start
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["MRA_No"].ToString()))
                    txtMRANo.Text = dtProposal.Rows[0]["MRA_No"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["MRA_Effective_Date"].ToString()))
                    txtMRADate.Text = dtProposal.Rows[0]["MRA_Effective_Date"].ToString();

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["AcountMgr1"].ToString()))
                    ddlAccountManager1.SelectedText = dtProposal.Rows[0]["AcountMgr1"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["AcountMgrId1"].ToString()))
                    ddlAccountManager1.SelectedValue = dtProposal.Rows[0]["AcountMgrId1"].ToString();

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["AcountMgr2"].ToString()))
                    ddlAccountManager2.SelectedText = dtProposal.Rows[0]["AcountMgr2"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["AcountMgrId2"].ToString()))
                    ddlAccountManager2.SelectedValue = dtProposal.Rows[0]["AcountMgrId2"].ToString();

                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["RegionalMgr3"].ToString()))
                    ddlRegionalManager.SelectedText = dtProposal.Rows[0]["RegionalMgr3"].ToString();
                if (!string.IsNullOrEmpty(dtProposal.Rows[0]["RegionalMgrId3"].ToString()))
                    ddlRegionalManager.SelectedValue = dtProposal.Rows[0]["RegionalMgrId3"].ToString();
                //MRA end


            }
            #endregion

            if (DS.Tables[1].Rows.Count > 0)
            {
                System.Data.DataTable dt = DS.Tables[1].Copy();
                ViewState["dtPrimaryGrid"] = dt;
                DataRow[] drprim = dt.Select("Offer_Type=1");
                DataRow[] drApp = dt.Select("RentFrequencyID In(6,7,8)");
                if (drprim.Length > 0)
                {
                    dtPrimaryGrid = drprim.CopyToDataTable();
                    FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                    if (drApp.Length > 0)
                        chkApplicable.Enabled = true;
                }
                chkIsSecondary.Enabled = false;
                DataRow[] drsec = dt.Select("Offer_Type=2");
                if (drsec.Length > 0)
                {
                    dtSecondaryGrid = drsec.CopyToDataTable();
                    FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                    if (drApp.Length > 0)
                        chkApplicableSec.Enabled = true;

                }
                FunAddremoveAssetCategoryDDL();
            }
            if (RBLSecondaryTerm.SelectedValue == "1")
            {
                chkIsSecondary.Enabled = true;
            }
            if (DS.Tables[2].Rows.Count > 0)
            {
                ViewState["dtEUCDetails"] = dtEUCDetails = DS.Tables[2].Copy();
                FunFillgrid(grvEUC, dtEUCDetails);
                //For end use Asset category

            }
            else
            {
                FunPriSetEmptyEUCtbl();
            }

            if (DS.Tables[4].Rows.Count > 0)
            {
                //ViewState["dtEUCDetails"] = dtEUCDetails = DS.Tables[3].Copy();
                FunFillgrid(grvConsDocuments, DS.Tables[4].Copy());
            }

            if (DS.Tables[5].Rows.Count > 0)
            {
                //ViewState["dtEUCDetails"] = dtEUCDetails = DS.Tables[3].Copy();
                ViewState["DeliveryAddress"] = DS.Tables[5].Copy();

                if (ViewState["DeliveryAddress"] != null)
                    dtDeliveryAddress = (System.Data.DataTable)ViewState["DeliveryAddress"];

                if (Convert.ToInt32(DS.Tables[6].Rows[0]["Default_Address_Type"]) == 1)
                {
                    DataRow[] dt = dtDeliveryAddress.Select("Address_Type=1");
                    if (dt.Length > 0)
                    {
                        FunPriLoadDeliverAddress(dt.CopyToDataTable());
                    }
                }
                else
                {
                    ddlDeliveryType.SelectedValue = Convert.ToString(DS.Tables[6].Rows[0]["Default_Address_Type"]);
                    ddlDeliveryType_SelectedIndexChange(null, null);
                }
            }
            // Modified By : Anbuvel.T,Date:11-Jan-2016, Description : OPC_CR_025(CL_904 & 905) Proposal Number Enabled Option Done. Bug Fixing
            if (strMode == "M" && txtStatus.Text.StartsWith("2 "))
            {
                txtTenure.Text = string.Empty;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@PAnum", txtPrimeAccountNo.Text.Trim());
                Procparam.Add("@Pricing_Id", strPricingId);
                Procparam.Add("@User_ID", Convert.ToString(intUserId));
                DataTable dt = Utility.GetDefaultData("S3G_RS_Reset_Invoice", Procparam);
                txtBusinessIRR_Repay.Text = string.Empty;
            }
            else
            {
                FunPriEmptyRentalDetails();
            }

            FunPriBindGridSummary();
            FunPriGetMappedAmount();

            ddlDeliveryState_SelectedIndexChange(null, null);
            string strValidDate = "";
            ViewState["ProposalOfferDate"] = txtOfferDate.Text;

            if (txtMRADate.Text == "")
                txtMRADate.Text = txtOfferDate.Text;

            if (Utility.StringToDate(txtMRADate.Text) > Utility.StringToDate(txtOfferDate.Text))
            {
                strValidDate = txtMRADate.Text;
            }
            else
            {
                strValidDate = txtOfferDate.Text;
            }
            TextBox txtValidateDate = new TextBox();
            txtValidateDate.Text = strValidDate;
            if (Utility.StringToDate(txtAccountDate.Text) < Utility.StringToDate(txtValidateDate.Text))
            {
                Utility.FunShowAlertMsg(this, "Account Date should be greater than Proposal date/MRA date");
                TabMainPage.Focus();
                txtAccountDate.Focus();
                txtAccountDate.Text = "";
                return;
            }
            //to load inflow or outflow for security deposits
            //FunPriLoadSecurityCashflows();
            Utility.FillDataTable(ddlRentalTDSSec, DS.Tables[7], "Tax_Law_Section", "Tax_Law_Section");

            if (DS.Tables[0].Rows[0]["state_wise_billing"].ToString() == "1")
            {
                //opc042 start
                grvCustEmail.DataSource = DS.Tables[8];
                grvCustEmail.DataBind();
                //opc042 end
            }

            if (DS.Tables[0].Rows[0]["Is_Manual_Num"].ToString() == "1")
            {
                txtPrimeAccountNo.ReadOnly = false;
            }
            else
            {
                txtPrimeAccountNo.ReadOnly = true;
            }

            if (DS.Tables[0].Rows[0]["Rental_Based_On"].ToString() == "1")
            {
                chkITC.Checked = true;
                chkITC.Enabled = false;
            }
            else
            {
                chkITC.Checked = false;
                chkITC.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //By Siva.K on 18JUN2015 Get the Location Category ID
    private void FunPriLoadLocationID(int LocationID)
    {
        try
        {

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@location_id", Convert.ToString(LocationID));

            //dtTable = Utility.GetGridData(SPNames.S3G_CLN_GetChequeDetailsforAuthorization, Procparam, out intTotalRecords, ObjPaging);
            DataTable dt = Utility.GetDefaultData("S3G_LAD_GET_LOC_CATEGORY", Procparam);
            if (dt.Rows.Count > 0)
            {
                ViewState["Parent_Location_ID"] = dt.Rows[0]["Location_Category_ID"].ToString();
            }
            //DataSet DS = Utility.GetDataset("S3G_ORG_Get_Proposal_Dtls_ACC", Procparam);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriSetProposalType()
    {
        try
        {
            btnInvoices.Visible = true;
            pnlSecondaryGrid.Enabled = true;
            grvRentalDetails.Enabled = true;
            txtAddInvAmt.Text = "";
            switch (ddlProposalType.SelectedValue)
            {
                case "1"://New Lease
                    grvRentalDetails.Enabled = false;
                    pnlAddtionalTax.Enabled = true;
                    break;
                case "2"://Sale and Lease
                    grvRentalDetails.Enabled = false;
                    pnlAddtionalTax.Enabled = true;
                    break;
                case "3"://Extension
                    pnlSecondaryGrid.Enabled = true;
                    btnInvoices.Visible = false;
                    pnlAddtionalTax.Enabled = false;
                    break;
                case "4"://RW New
                    txtAddInvAmt.Enabled = true;
                    //pnlAddtionalTax.Enabled = false;
                    chkLBT.Enabled = chkbxGSTRC.Enabled = chkPT.Enabled = chkITC_Cap.Enabled = 
                        chkET.Enabled = chkRC.Enabled = chkITC.Enabled = false;
                    ddlRentalTDSSec.Enabled = true;
                    break;
                case "5"://RW Sale and Lease
                    txtAddInvAmt.Enabled = true;
                    //pnlAddtionalTax.Enabled = false;
                    chkLBT.Enabled = chkbxGSTRC.Enabled = chkPT.Enabled = chkITC_Cap.Enabled =
                       chkET.Enabled = chkRC.Enabled = chkITC.Enabled = false;
                    ddlRentalTDSSec.Enabled = true;
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void FunAddremoveAssetCategoryDDL()
    {
        try
        {
            string strTextItem = "NAME"; string strValueItem = "ID";
            System.Data.DataTable dtAssetCategorytbl = new System.Data.DataTable();
            if (ViewState["dtPrimaryGrid"] != null)
                dtPrimaryGrid = (System.Data.DataTable)ViewState["dtPrimaryGrid"];

            if (!dtAssetCategorytbl.Columns.Contains("AssetCategory"))
                dtAssetCategorytbl.Columns.Add("NAME");
            if (!dtAssetCategorytbl.Columns.Contains("AssetCategory_ID"))
                dtAssetCategorytbl.Columns.Add("ID");
            System.Data.DataTable dtPriSecondary = new System.Data.DataTable();

            dtPriSecondary = dtPrimaryGrid.Copy();
            if (dtPriSecondary.Rows.Count > 0)
            {
                System.Data.DataTable dtUniqRecords = dtPriSecondary.DefaultView.ToTable(true, new string[] { "AssetCategory", "AssetCategory_ID" });

                foreach (DataRow dr in dtUniqRecords.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["AssetCategory_ID"].ToString()))
                    {
                        DataRow drAssetCategory;
                        drAssetCategory = dtAssetCategorytbl.NewRow();
                        if (!string.IsNullOrEmpty(dr["AssetCategory_ID"].ToString()))
                            drAssetCategory[strValueItem] = dr["AssetCategory_ID"];
                        if (!string.IsNullOrEmpty(dr["AssetCategory"].ToString()))
                            drAssetCategory[strTextItem] = dr["AssetCategory"];
                        dtAssetCategorytbl.Rows.Add(drAssetCategory);
                    }
                }
            }

            if (dtAssetCategorytbl.Rows.Count > 0)
            {
                ViewState["dtAssetCategorytbl"] = dtAssetCategorytbl;

                //Fill dropdown End Use Customer Details
                //ddlAssetCategoryEUC.DataTextField = strTextItem;
                //ddlAssetCategoryEUC.DataValueField = strValueItem;
                //ddlAssetCategoryEUC.DataSource = dtAssetCategorytbl;
                //ddlAssetCategoryEUC.DataBind();
                //System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                //ddlAssetCategoryEUC.Items.Insert(0, liSelect1);
                //ddlAssetCategoryEUC.AddItemToolTipValue();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }


    /// <summary>
    /// This method is used to load/Bind the grid for the given datatable.
    /// </summary>
    /// <param name="grv"></param>
    /// <param name="dtEntityBankdetails"></param>
    /// 
    private void FunFillgrid(GridView grv, System.Data.DataTable dtbl)
    {
        try
        {
            grv.DataSource = dtbl;
            grv.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //To Load Proposal Type,Return Pattern and Security Deposits
    /// <summary>
    /// This method is used to Load the Proposal Type,Return Pattern and Security Deposits to a dropdown from stored Procedure using BindDatatable option.
    /// </summary>
    private void FunLoadDDLControls()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Option", "1");
            DataSet ds = Utility.GetDataset("S3G_ORG_RentalLukup", Procparam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)//Proposal Type
                {
                    ddlProposalType.FillDataTable(ds.Tables[0], "Value", "Name");
                }
                if (ds.Tables[1].Rows.Count > 0)//SECURITY_DEPOSIT
                {
                    ddlSecuritydeposit.FillDataTable(ds.Tables[1], "Value", "Name");
                }
                if (ds.Tables[2].Rows.Count > 0)//RETURN_PATTERN
                {
                    ddlReturnPattern.FillDataTable(ds.Tables[2], "Value", "Name", false);
                    ddlReturnPattern.SelectedValue = "3";//default PTF
                }
                if (ds.Tables[3].Rows.Count > 0)//TIME_VALUE
                {
                    ViewState["TIME_VALUE"] = ds.Tables[3].Copy();
                }
                if (ds.Tables[4].Rows.Count > 0)//FREQUENCY
                {
                    ViewState["FREQUENCY"] = ds.Tables[4].Copy();
                }

                //if (ds.Tables[5].Rows.Count > 0)//Sales Tax
                //{
                //    ddlSalesTax.FillDataTable(ds.Tables[5], "Value", "Name", true);
                //}

                if (ds.Tables[6].Rows.Count > 0)//Invoice Filter
                {
                    ddlFilterType.FillDataTable(ds.Tables[6], "Value", "Name", true);
                }
                if (ds.Tables[7].Rows.Count > 0)//Delivery Type
                {
                    ddlDeliveryType.FillDataTable(ds.Tables[7], "Value", "Name", false);


                }
                if (ds.Tables[8].Rows.Count > 0)//Delivery State
                {
                    ddlDeliveryState.FillDataTable(ds.Tables[8], "Id", "Value", false);
                    ddlBillingState.FillDataTable(ds.Tables[8], "Id", "Value", false);
                }

                if (ds.Tables[9].Rows.Count > 0)//Delivery State
                {
                    ddlSEZZone.FillDataTable(ds.Tables[9], "Value", "Name", true);
                }
                if (ds.Tables[10].Rows.Count > 0)//ddlReportType
                {
                    ddlReportType.FillDataTable(ds.Tables[10], "Value", "Name", true);
                }

                //Added on 31Mar2015 starts here
                if (ds.Tables[11].Rows.Count > 0)
                {
                    ViewState["ServiceTax"] = Convert.ToString(ds.Tables[11].Rows[0]["ST_Perc"]);
                }
                //Added on 31Mar2015 ends here

                //Added on 01Apr2015 starts here
                if (ds.Tables[12].Rows.Count > 0)
                {
                    ViewState["ServiceTax_Cashflows"] = ds.Tables[12];
                }
                //Added on 01Apr2015 ends here

                //Added on 03Jul2015 starts here
                if (ds.Tables[13].Rows.Count > 0)
                {
                    ViewState["Addtl_Srv_Tax"] = Convert.ToString(ds.Tables[13].Rows[0]["Addtl_Tax"]);
                }
                //Added on 03Jul2015 ends here
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    #region "EUC Dtls Code"

    /// <summary>
    /// This user defined function is used to add the End use Customer details in grid.
    /// </summary>
    private void FunPriAddEUCDtls()
    {
        try
        {
            DataRow drEmptyRow;
            dtEUCDetails = (System.Data.DataTable)ViewState["dtEUCDetails"];

            if (dtEUCDetails.Rows.Count > 0)
            {
                if (dtEUCDetails.Rows[0]["CustomerName"].ToString() == "")
                {
                    dtEUCDetails.Rows[0].Delete();
                }
            }

            ////checking if already exist
            //foreach (DataRow dr in dtEUCDetails.Rows)
            //{
            //    if (dr["AssetCategory_ID"].ToString().Trim() == ddlAssetCategoryEUC.SelectedItem.Text.Trim())
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "Asset Category already Exists");
            //        return;
            //    }
            //}


            drEmptyRow = dtEUCDetails.NewRow();
            drEmptyRow["SlNo"] = dtEUCDetails.Rows.Count + 1;
            //if (ddlAssetCategoryEUC.SelectedIndex > 0)
            //{
            //    drEmptyRow["AssetCategory"] = ddlAssetCategoryEUC.SelectedItem.Text.Trim();
            //    drEmptyRow["AssetCategory_ID"] = ddlAssetCategoryEUC.SelectedValue.Trim();
            //}

            if (!string.IsNullOrEmpty(txtCustomerName_EUC.Text.Trim()))
                drEmptyRow["CustomerName"] = txtCustomerName_EUC.Text.Trim();
            else
                drEmptyRow["CustomerName"] = DBNull.Value;

            if (!string.IsNullOrEmpty(txtEmailId_EUC.Text.Trim()))
                drEmptyRow["EmailId"] = txtEmailId_EUC.Text.Trim();
            else
                drEmptyRow["EmailId"] = DBNull.Value;

            if (!string.IsNullOrEmpty(txtRemarks_EUC.Text.Trim()))
                drEmptyRow["Remarks"] = txtRemarks_EUC.Text.Trim();
            else
                drEmptyRow["Remarks"] = DBNull.Value;

            dtEUCDetails.Rows.Add(drEmptyRow);
            ViewState["dtEUCDetails"] = dtEUCDetails;
            FunFillgrid(grvEUC, dtEUCDetails);

            FunPriClearEUC();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to modify the End use Customer details in grid.
    /// </summary>
    private void FunPriModifyEUCdtls()
    {
        try
        {
            dtEUCDetails = (System.Data.DataTable)ViewState["dtEUCDetails"];

            DataRow drow = dtEUCDetails.Rows[Convert.ToInt32(lblEUCSlNo.Text) - 1];
            drow.BeginEdit();
            drow["SlNo"] = lblEUCSlNo.Text;
            //if (ddlAssetCategoryEUC.SelectedIndex > 0)
            //{
            //    drow["AssetCategory"] = ddlAssetCategoryEUC.SelectedItem.Text.Trim();
            //    drow["AssetCategory_ID"] = ddlAssetCategoryEUC.SelectedValue;
            //}
            //else
            //{
            //    drow["AssetCategory"] = drow["AssetCategory_ID"] = DBNull.Value;
            //}

            if (!string.IsNullOrEmpty(txtCustomerName_EUC.Text))
                drow["CustomerName"] = txtCustomerName_EUC.Text;
            //if (!string.IsNullOrEmpty(txtEmailId_EUC.Text))
            drow["EmailId"] = txtEmailId_EUC.Text;
            if (!string.IsNullOrEmpty(txtRemarks_EUC.Text))
                drow["Remarks"] = txtRemarks_EUC.Text;

            drow.EndEdit();
            ViewState["dtEUCDetails"] = dtEUCDetails;
            FunFillgrid(grvEUC, dtEUCDetails);
            FunPriClearEUC();
            btnAddEUC.Enabled = btnhClearEUC.Enabled = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    ///  This user defined function is used to select the End use Customer details from grid to controls.
    /// </summary>
    /// <param name="sender"></param>
    private void FunPriRBSelectIndexChange(object sender)
    {
        try
        {
            btnModifyEUC.Enabled = true;
            btnAddEUC.Enabled = btnhClearEUC.Enabled = false;

            int intRowIndex = Utility.FunPubGetGridRowID("grvEUC", ((RadioButton)sender).ClientID);
            dtEUCDetails = (System.Data.DataTable)ViewState["dtEUCDetails"];

            FunPriResetRdButton(grvEUC, intRowIndex);

            DataRow drow = dtEUCDetails.Rows[intRowIndex];
            lblEUCSlNo.Text = drow["SlNo"].ToString();
            //if (!string.IsNullOrEmpty(drow["AssetCategory_ID"].ToString()))
            //    ddlAssetCategoryEUC.SelectedValue = drow["AssetCategory_ID"].ToString();
            txtCustomerName_EUC.Text = drow["CustomerName"].ToString();
            txtEmailId_EUC.Text = drow["EmailId"].ToString();
            txtRemarks_EUC.Text = drow["Remarks"].ToString();
            if (PageMode == PageModes.Query)
            {
                btnModifyEUC.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to delete the End use Customer details from grid.
    /// </summary>
    /// <param name="intRowIndex"></param>
    private void FunPriRemoveEUCDetails(int intRowIndex)
    {
        try
        {
            dtEUCDetails = (System.Data.DataTable)ViewState["dtEUCDetails"];
            dtEUCDetails.Rows.RemoveAt(intRowIndex);
            if (dtEUCDetails.Rows.Count == 0)
            {
                FunPriSetEmptyEUCtbl();
            }
            else
            {
                FunFillgrid(grvEUC, dtEUCDetails);
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to clear the End use Customer details controls .
    /// </summary>
    private void FunPriClearEUC()
    {
        try
        {
            // ddlAssetCategoryEUC.SelectedIndex = -1;
            txtCustomerName_EUC.Text = txtEmailId_EUC.Text = txtRemarks_EUC.Text = string.Empty;
            lblEUCSlNo.Text = string.Empty;
            btnModifyEUC.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to default empty datatable for the End use Customer details .
    /// </summary>
    private void FunPriSetEmptyEUCtbl()
    {
        try
        {
            DataRow drEmptyRow;
            dtEUCDetails = new System.Data.DataTable();
            dtEUCDetails.Columns.Add("SlNo");
            //dtEUCDetails.Columns.Add("AssetCategory");
            //dtEUCDetails.Columns.Add("AssetCategory_ID");
            dtEUCDetails.Columns.Add("CustomerName");
            dtEUCDetails.Columns.Add("EmailId");
            dtEUCDetails.Columns.Add("Remarks");
            drEmptyRow = dtEUCDetails.NewRow();
            dtEUCDetails.Rows.Add(drEmptyRow);
            ViewState["dtEUCDetails"] = dtEUCDetails;
            FunFillgrid(grvEUC, dtEUCDetails);
            grvEUC.Rows[0].Visible = false;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to reset all select option from the end use customer grid
    /// </summary>
    /// <param name="grv"></param>
    /// <param name="intRowIndex"></param>
    private void FunPriResetRdButton(GridView grv, int intRowIndex)
    {
        try
        {
            for (int i = 0; i <= grv.Rows.Count - 1; i++)
            {
                if (i != intRowIndex)
                {
                    RadioButton rdSelect = grv.Rows[i].FindControl("RBSelect") as RadioButton;
                    rdSelect.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerName_EUCList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        //Procparam.Add("@Customer_Id", obj_PageValue.ViewState["Customer_Id"].ToString());
        if (System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] != null)
        {
            if (System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"].ToString() != "0")
                Procparam.Add("@Customer_Id", System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"].ToString());
        }
        Procparam.Add("@PrefixText", prefixText.Trim());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetCustomerName_EUCList", Procparam), false);
        return suggetions.ToArray();
    }



    #region "EUC Dtls Code"

    /// <summary>
    /// This event is used to delete the End use customer records in grid level 
    /// it will call user defined function FunPriRemoveEUCDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvEUC_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveEUCDetails(e.RowIndex);
            FunPriClearEUC();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + "Unable to delete End use Customer Details";
            cv_TabMainPage.IsValid = false;
        }
    }

    /// <summary>
    /// This event will fire once we need to add the entered End use details in grid using user defined function FunPriAddEUCDtls.
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddEUC_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriAddEUCDtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + "Unable to Add End use Customer Details";
            cv_TabMainPage.IsValid = false;
        }

    }

    /// <summary>
    /// This event is used to modify the selected End use customer records using user defind function FunPriModifyEUCdtls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnModifyEUC_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriModifyEUCdtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + "Unable to Modify End use Customer Details";
            cv_TabMainPage.IsValid = false;
        }
    }

    /// <summary>
    /// This event is used to clear the end use customer controls using user defind function FunPriClearEUC
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClearInt_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearEUC();
            btnAddEUC.Enabled = true;
            btnModifyEUC.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + "Unable to Clear End use Customer Details";
            cv_TabMainPage.IsValid = false;
        }
    }

    /// <summary>
    /// This event is used to select the End use Customer list for Display using user defined function FunPriRBSelectIndexChange
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RBSelectInt_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriRBSelectIndexChange(sender);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + "Unable to Select End use Customer Details";
            cv_TabMainPage.IsValid = false;
        }

    }

    #endregion


    protected void chkSelected_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (CheckBox)sender;
            RepeaterItem gvRow = (RepeaterItem)chk.Parent;
            Label lblInvoice_Type = (Label)gvRow.FindControl("lblInvoice_Type");
            Label lblInvoiceId = (Label)gvRow.FindControl("lblInvoiceId");
            if (ViewState["dtInvoiceGrid"] != null)
                dtInvoiceGrid = (System.Data.DataTable)ViewState["dtInvoiceGrid"];
            else
            {
                if (!dtInvoiceGrid.Columns.Contains("Invoice_Type"))
                    dtInvoiceGrid.Columns.Add("Invoice_Type");
                if (!dtInvoiceGrid.Columns.Contains("Invoice_Id"))
                    dtInvoiceGrid.Columns.Add("Invoice_Id");

            }
            if (chk.Checked)
            {
                DataRow drInvoiceGrid = dtInvoiceGrid.NewRow();
                drInvoiceGrid["Invoice_Type"] = lblInvoice_Type.Text;
                drInvoiceGrid["Invoice_Id"] = lblInvoiceId.Text;
                dtInvoiceGrid.Rows.Add(drInvoiceGrid);
            }
            else
            {
                DataRow[] drInvoiceGrid = dtInvoiceGrid.Select("Invoice_Type='" + lblInvoice_Type.Text + "' and Invoice_Id=" + lblInvoiceId.Text + "");
                if (drInvoiceGrid.Length > 0)
                {
                    drInvoiceGrid[0].Delete();
                }
                dtInvoiceGrid.AcceptChanges();
            }
            ViewState["dtInvoiceGrid"] = dtInvoiceGrid;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select Invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void chkSelectedH_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkHdr = (CheckBox)sender;

            foreach (RepeaterItem item in grvInvoices.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelected");
                Label lblInvoice_Type = (Label)item.FindControl("lblInvoice_Type");
                Label lblInvoiceId = (Label)item.FindControl("lblInvoiceId");
                if (ViewState["dtInvoiceGrid"] != null)
                    dtInvoiceGrid = (System.Data.DataTable)ViewState["dtInvoiceGrid"];
                else
                {
                    if (!dtInvoiceGrid.Columns.Contains("Invoice_Type"))
                        dtInvoiceGrid.Columns.Add("Invoice_Type");
                    if (!dtInvoiceGrid.Columns.Contains("Invoice_Id"))
                        dtInvoiceGrid.Columns.Add("Invoice_Id");

                }

                if (chkHdr.Checked)
                    chk.Checked = true;
                else
                    chk.Checked = false;

                if (chk.Checked)
                {
                    DataRow drInvoiceGrid = dtInvoiceGrid.NewRow();
                    drInvoiceGrid["Invoice_Type"] = lblInvoice_Type.Text;
                    drInvoiceGrid["Invoice_Id"] = lblInvoiceId.Text;
                    dtInvoiceGrid.Rows.Add(drInvoiceGrid);
                }
                else
                {
                    DataRow[] drInvoiceGrid = dtInvoiceGrid.Select("Invoice_Type='" + lblInvoice_Type.Text + "' and Invoice_Id=" + lblInvoiceId.Text + "");
                    if (drInvoiceGrid.Length > 0)
                    {
                        drInvoiceGrid[0].Delete();
                    }
                    dtInvoiceGrid.AcceptChanges();
                }
            }
            ViewState["dtInvoiceGrid"] = dtInvoiceGrid;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select Invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void chkSelectPG_checkedChange(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSelectPG = (CheckBox)sender;
            int gRowIndex = Utility.FunPubGetGridRowID("grvPrimaryGrid", ((CheckBox)sender).ClientID);
            Label lblAssetCategoryIDPG = (Label)grvPrimaryGrid.Rows[gRowIndex].FindControl("lblAssetCategoryIDPG");
            Label lblTenurePG = (Label)grvPrimaryGrid.Rows[gRowIndex].FindControl("lblTenurePG");

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Tenure", lblTenurePG.Text);
            Procparam.Add("@Asset_Category_Id", lblAssetCategoryIDPG.Text);
            System.Data.DataTable dtRVMatrix = Utility.GetDefaultData("S3G_LOANAD_Validate_RV_Matrix", Procparam);

            if (dtRVMatrix.Rows[0]["Is_RV_Matrix_Defined"].ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('RV Matrix is not defined.');", true);
                chkSelectPG.Checked = false;
                return;
            }

            //By Siva.k on 02JUL2015 Remove the Residual Per
            /*
            TextBox txtResidualPerPG = (TextBox)grvPrimaryGrid.Rows[gRowIndex].FindControl("txtResidualPerPG");
            if (txtResidualPerPG.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('RV Matrix not defined.');", true);
                chkSelectPG.Checked = false;
                return;
            }   //END By Siva.k on 02JUL2015 Remove the Residual Per
            */
            if (!string.IsNullOrEmpty(lblAssetCategoryIDPG.Text))
            {
                //if (grvSummaryInvoices.Rows.Count > 0 && ((DataTable)ViewState["dtRentalDetails"]).Rows.Count > 0 && ((DataTable)ViewState["dtRentalDetails"]).Rows[0]["PA_SA_REF_ID"].ToString() != "")
                //{
                //    DataTable dtRent = (DataTable)ViewState["dtRentalDetails"];
                //    foreach (GridViewRow row in grvSummaryInvoices.Rows)
                //    {
                //        Label lblPA_SA_REF_ID = (Label)row.FindControl("lblPA_SA_REF_ID");
                //        DataRow[] dRow = dtRent.Select("PA_SA_REF_ID=" + lblPA_SA_REF_ID.Text);

                //        if (dRow.Length > 0)
                //        {
                //            Utility.FunShowAlertMsg(this.Page, "Remove the Rental Schedule Number " + dRow[0]["PANum"].ToString() + " in Extension/Rewrite Grid.");
                //            chkSelectPG.Checked = true;
                //            return;
                //        }
                //    }
                //}

                System.Data.DataTable dt = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
                DataRow[] drr = dt.Select("AssetCategory_ID = " + lblAssetCategoryIDPG.Text);
                foreach (DataRow dr in drr)
                {
                    if (chkSelectPG.Checked)
                        dr["Selected"] = "1";
                    else
                        dr["Selected"] = "0";
                }
                dtPrimaryGrid.AcceptChanges();

                ViewState["dtPrimaryGrid"] = dt;
                DataRow[] drprim = dt.Select("Offer_Type=1");
                if (drprim.Length > 0)
                {
                    dtPrimaryGrid = drprim.CopyToDataTable();
                    FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                    FunprisumgrvPrimaryGrid(); //By Siva.K on 19JUN2015 updated the Total after changing Residual %
                }
                DataRow[] drsec = dt.Select("Offer_Type=2");
                if (drsec.Length > 0)
                {
                    dtSecondaryGrid = drsec.CopyToDataTable();
                    FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                }

                //unmap the invoices 
                if (!chkSelectPG.Checked)//Unchecked
                {
                    if (!string.IsNullOrEmpty(lblAssetCategoryIDPG.Text))
                    {
                        string strPANumber = string.Empty;
                        Procparam = new Dictionary<string, string>();
                        if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                            strPANumber = txtPrimeAccountNo.Text;
                        else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                            strPANumber = ddlApplicationReferenceNo.SelectedText;
                        Procparam.Add("@PAnum", strPANumber);
                        Procparam.Add("@User_Id", intUserId.ToString());
                        Procparam.Add("@Asset_Category_Id", lblAssetCategoryIDPG.Text);
                        System.Data.DataTable dtMAp = Utility.GetDefaultData("S3G_RS_Remove_MappedInvoices", Procparam);

                        FunPriBindGridSummary();
                        FunPriGetMappedAmount();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select asset category.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void chkSelectSG_checkedChange(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSelectPG = (CheckBox)sender;
            int gRowIndex = Utility.FunPubGetGridRowID("grvSecondaryGrid", ((CheckBox)sender).ClientID);
            Label lblAssetCategoryIDPG = (Label)grvSecondaryGrid.Rows[gRowIndex].FindControl("lblAssetCategoryIDSG");
            //By Siva.k on 02JUL2015 Remove the Residual Per
            TextBox txtResidualPerSG = (TextBox)grvSecondaryGrid.Rows[gRowIndex].FindControl("txtResidualPerSG");
            if (txtResidualPerSG.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('RV Matrix not defined.');", true);
                chkSelectPG.Checked = false;
                return;
            }   //END By Siva.k on 02JUL2015 Remove the Residual Per

            if (!string.IsNullOrEmpty(lblAssetCategoryIDPG.Text))
            {
                System.Data.DataTable dt = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
                DataRow[] drr = dt.Select("AssetCategory_ID = " + lblAssetCategoryIDPG.Text);
                foreach (DataRow dr in drr)
                {
                    if (chkSelectPG.Checked)
                        dr["Selected"] = "1";
                    else
                        dr["Selected"] = "0";
                }
                dtPrimaryGrid.AcceptChanges();

                ViewState["dtPrimaryGrid"] = dt;
                DataRow[] drprim = dt.Select("Offer_Type=1");
                if (drprim.Length > 0)
                {
                    dtPrimaryGrid = drprim.CopyToDataTable();
                    FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                }
                DataRow[] drsec = dt.Select("Offer_Type=2");
                if (drsec.Length > 0)
                {
                    dtSecondaryGrid = drsec.CopyToDataTable();
                    FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select asset category.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void txtResidualPerPG_textChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtResidualPerPG = (TextBox)sender;
            int gRowIndex = Utility.FunPubGetGridRowID("grvPrimaryGrid", ((TextBox)sender).ClientID);
            Label lblAssetCategoryIDPG = (Label)grvPrimaryGrid.Rows[gRowIndex].FindControl("lblAssetCategoryIDPG");

            //check it shoud not exceed 100%

            if (!string.IsNullOrEmpty(txtResidualPerPG.Text))
            {
                if (Convert.ToDecimal(txtResidualPerPG.Text) > 100)
                {
                    Utility.FunShowAlertMsg(this.Page, "Residual Per% should not exceed 100%");
                    txtResidualPerPG.Text = string.Empty;
                    return;
                }
            }


            if (!string.IsNullOrEmpty(lblAssetCategoryIDPG.Text))
            {
                System.Data.DataTable dt = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
                DataRow[] drr = dt.Select("AssetCategory_ID = " + lblAssetCategoryIDPG.Text + " and Offer_Type=1");
                foreach (DataRow dr in drr)
                {
                    if (!string.IsNullOrEmpty(txtResidualPerPG.Text))
                    {
                        dr["ResidualPer"] = Convert.ToDecimal(txtResidualPerPG.Text);
                        //Commented and added on 09Jul2015 Starts here
                        //dr["RV_amount"] = Convert.ToDecimal(dr["ResidualPer"].ToString()) * Convert.ToDecimal(dr["MappedAmount"].ToString()) / 100;
                        dr["RV_amount"] = Math.Round(Convert.ToDecimal(dr["ResidualPer"].ToString()) * Math.Round(Convert.ToDecimal(dr["Capitalised_Amount"].ToString()), 4) / 100, 4);
                        //Commented and added on 09Jul2015 Ends here
                    }
                    else
                    {
                        dr["ResidualPer"] = 0;
                        dr["RV_amount"] = 0;
                    }
                }
                dtPrimaryGrid.AcceptChanges();

                ViewState["dtPrimaryGrid"] = dt;
                DataRow[] drprim = dt.Select("Offer_Type=1");
                if (drprim.Length > 0)
                {
                    dtPrimaryGrid = drprim.CopyToDataTable();
                    ViewState["dtPrimaryGridsum"] = dtPrimaryGrid;
                    FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                    FunprisumgrvPrimaryGrid(); //By Siva.K on 19JUN2015 updated the Total after changing Residual %
                }
                //DataRow[] drsec = dt.Select("Offer_Type=2");
                //if (drsec.Length > 0)
                //{
                //    dtSecondaryGrid = drsec.CopyToDataTable();
                //    FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                //}

                //By Siva.k on 02JUL2015 Remove the Residual Per
                if (grvSecondaryGrid.Rows.Count > 0 && chkIsSecondary.Checked == false)
                    Set_ResidualPer_Secondary(Convert.ToInt32(lblAssetCategoryIDPG.Text), Convert.ToDecimal(txtResidualPerPG.Text));
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select asset category.";
            cv_TabMainPage.IsValid = false;
        }
    }

    private void Primary_Secondary_Update()
    {
        if (grvPrimaryGrid.Rows.Count > 0)
        {
            for (int pri = 0; pri < grvPrimaryGrid.Rows.Count; pri++)
            {
                Label lblAssetCategoryIDPG = (Label)grvPrimaryGrid.Rows[pri].FindControl("lblAssetCategoryIDPG");
                TextBox txtResidualPerPG = (TextBox)grvPrimaryGrid.Rows[pri].FindControl("txtResidualPerPG");
                if (!string.IsNullOrEmpty(lblAssetCategoryIDPG.Text))
                {
                    System.Data.DataTable dt = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
                    DataRow[] drr = dt.Select("AssetCategory_ID = " + lblAssetCategoryIDPG.Text + " and Offer_Type=1");
                    foreach (DataRow dr in drr)
                    {
                        if (!string.IsNullOrEmpty(txtResidualPerPG.Text))
                        {
                            dr["ResidualPer"] = Convert.ToDecimal(txtResidualPerPG.Text);
                            dr["RV_amount"] = Math.Round(Convert.ToDecimal(dr["ResidualPer"].ToString()) * Math.Round(Convert.ToDecimal(dr["Capitalised_Amount"].ToString()), 4) / 100, 4);
                        }
                        else
                        {
                            dr["ResidualPer"] = 0;
                            dr["RV_amount"] = 0;
                        }
                    }
                    dtPrimaryGrid.AcceptChanges();

                    ViewState["dtPrimaryGrid"] = dt;
                    DataRow[] drprim = dt.Select("Offer_Type=1");
                    if (drprim.Length > 0)
                    {
                        dtPrimaryGrid = drprim.CopyToDataTable();
                        ViewState["dtPrimaryGridsum"] = dtPrimaryGrid;
                        FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                        FunprisumgrvPrimaryGrid(); //By Siva.K on 19JUN2015 updated the Total after changing Residual %
                    }
                    if (grvSecondaryGrid.Rows.Count > 0 && chkIsSecondary.Checked == false)
                        Set_ResidualPer_Secondary(Convert.ToInt32(lblAssetCategoryIDPG.Text), Convert.ToDecimal(txtResidualPerPG.Text));
                }
            }
        }
    }
    protected void txtResidualPerSG_textChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtResidualPerSG = (TextBox)sender;
            int gRowIndex = Utility.FunPubGetGridRowID("grvSecondaryGrid", ((TextBox)sender).ClientID);
            Label lblAssetCategoryIDPG = (Label)grvSecondaryGrid.Rows[gRowIndex].FindControl("lblAssetCategoryIDSG");


            //check it shoud not exceed 100%
            if (!string.IsNullOrEmpty(txtResidualPerSG.Text))
            {
                if (Convert.ToDecimal(txtResidualPerSG.Text) > 100)
                {
                    Utility.FunShowAlertMsg(this.Page, "Residual Per% should not exceed 100%");
                    txtResidualPerSG.Text = string.Empty;
                    return;
                }
            }

            if (!string.IsNullOrEmpty(lblAssetCategoryIDPG.Text))
            {
                System.Data.DataTable dt = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
                DataRow[] drr = dt.Select("AssetCategory_ID = " + lblAssetCategoryIDPG.Text + " and Offer_Type=2");
                foreach (DataRow dr in drr)
                {
                    if (!string.IsNullOrEmpty(txtResidualPerSG.Text))
                    {
                        dr["ResidualPer"] = Convert.ToDecimal(txtResidualPerSG.Text);
                        dr["RV_amount"] = Convert.ToDecimal(dr["ResidualPer"].ToString()) * Convert.ToDecimal(dr["MappedAmount"].ToString()) / 100;
                    }
                    else
                    {
                        dr["ResidualPer"] = 0;
                        dr["RV_amount"] = 0;
                    }
                }
                dt.AcceptChanges();

                ViewState["dtPrimaryGrid"] = dt;
                //DataRow[] drprim = dt.Select("Offer_Type=1");
                //if (drprim.Length > 0)
                //{
                //    dtPrimaryGrid = drprim.CopyToDataTable();
                //    FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                //}
                DataRow[] drsec = dt.Select("Offer_Type=2");
                if (drsec.Length > 0)
                {
                    dtSecondaryGrid = drsec.CopyToDataTable();
                    ViewState["dtSecondaryGridsum"] = dtSecondaryGrid;
                    FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                    FunprisumgrvSecondaryGrid(); //By Siva.K on 02JUL2015 updated the Total after changing Residual %
                }

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select asset category.";
            cv_TabMainPage.IsValid = false;
        }
    }

    //By Siva.k on 02JUL2015 Remove the Residual Per
    protected void Set_ResidualPer_Secondary(int AssetCategory_ID, decimal Recidual_per)
    {
        try
        {

            System.Data.DataTable dt = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
            DataRow[] drr = dt.Select("AssetCategory_ID = " + AssetCategory_ID + " and Offer_Type=2");
            foreach (DataRow dr in drr)
            {
                if (Recidual_per != 0)
                {
                    dr["ResidualPer"] = Convert.ToDecimal(Recidual_per);
                    dr["RV_amount"] = Convert.ToDecimal(dr["ResidualPer"].ToString()) * Convert.ToDecimal(dr["MappedAmount"].ToString()) / 100;
                }
                else
                {
                    dr["ResidualPer"] = 0;
                    dr["RV_amount"] = 0;
                }
            }
            dt.AcceptChanges();

            ViewState["dtPrimaryGrid"] = dt;

            DataRow[] drsec = dt.Select("Offer_Type=2");
            if (drsec.Length > 0)
            {
                dtSecondaryGrid = drsec.CopyToDataTable();
                ViewState["dtSecondaryGridsum"] = dtSecondaryGrid;
                FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                FunprisumgrvSecondaryGrid(); //By Siva.K on 02JUL2015 updated the Total after changing Residual %
            }

        }

        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void grvPrimaryGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSelected = (Label)e.Row.FindControl("lblSelected");
                CheckBox chkSelectPG = (CheckBox)e.Row.FindControl("chkSelectPG");
                TextBox txtResidualPerPG = (TextBox)e.Row.FindControl("txtResidualPerPG");
                chkSelectPG.Checked = false;
                if (lblSelected.Text == "1")
                    chkSelectPG.Checked = true;

                //txtResidualPerPG.SetPercentagePrefixSuffix(13, 5, false, false, "Residual Per%");

                txtResidualPerPG.Attributes.Add("onblur", "funChkDecimial(this,'" + 10 + "','" + 6 + "','" + "Residual Per%" + "',false);");


            }
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void grvSecondaryGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSelected = (Label)e.Row.FindControl("lblSelected");
                CheckBox chkSelectSG = (CheckBox)e.Row.FindControl("chkSelectSG");
                TextBox txtResidualPerSG = (TextBox)e.Row.FindControl("txtResidualPerSG");
                chkSelectSG.Checked = false;
                if (lblSelected.Text == "1")
                {
                    chkSelectSG.Checked = true;
                }
                //txtResidualPerSG.SetPercentagePrefixSuffix(13, 5, false, false, "Residual Per%");
                txtResidualPerSG.Attributes.Add("onblur", "funChkDecimial(this,'" + 10 + "','" + 5 + "','" + "Residual Per%" + "',false);");
            }
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cv_TabMainPage.IsValid = false;
        }
    }


    protected void grvSummaryInvoices_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {



            //Label lbltemp_Inv_Id = (Label)grvSummaryInvoices.Rows[e.RowIndex].FindControl("lbltemp_Inv_Id");
            //if (!string.IsNullOrEmpty(lbltemp_Inv_Id.Text))
            //    ObjPagingSummary.ProSearchValue = lbltemp_Inv_Id.Text;
            FunPriBindGridSummary();
            FunPriGetMappedAmount();
            txtBusinessIRR_Repay.Text = string.Empty;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable To delete Due to Data Problem";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void grvPrimaryGrid_OnRowCommand(Object Sender, RepeaterCommandEventArgs e)
    {
        try
        {
            Label lbltemp_Inv_Id = (Label)e.Item.FindControl("lbltemp_Inv_Id");
            if (!string.IsNullOrEmpty(lbltemp_Inv_Id.Text))
                ObjPagingSummary.ProSearchValue = lbltemp_Inv_Id.Text;
            FunPriBindGridSummary();
            FunPriGetMappedAmount();
            txtBusinessIRR_Repay.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable To delete Due to Data Problem";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void btnRemoveAll_click(object sender, EventArgs e)
    {
        try
        {
            ObjPagingSummary.ProSearchValue = "0";
            FunPriBindGridSummary();
            FunPriGetMappedAmount();
            txtBusinessIRR_Repay.Text = string.Empty;
            ddlRentalTDSSec.Items.Clear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + "Unable to Add End use Customer Details";
            cv_TabMainPage.IsValid = false;
        }

    }
    #region Paging and Searching Methods For Grid



    private void FunPriBindGrid()
    {
        //objProductMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            System.Data.DataTable dtInvoicess = new System.Data.DataTable();
            bool bIsNewRow = false;
            string strPANumber = string.Empty;
            string strProcName = "S3G_GET_Invoice_PIVI_ACC";
            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = txtFilterValue.Text; //Ivoice_Filter
            ObjPaging.ProOrderBy = hdnOrderBy.Value;


            Procparam = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                strPANumber = txtPrimeAccountNo.Text;
            else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                strPANumber = ddlApplicationReferenceNo.SelectedText;
            Procparam.Add("@PAnum", strPANumber);
            Procparam.Add("@Customer_ID", ViewState["Customer_Id"].ToString());
            Procparam.Add("@Search_Type", ddlFilterType.SelectedValue);
            Procparam.Add("@Pricing_Id", ddlApplicationReferenceNo.SelectedValue);
            Procparam.Add("@Proposal_Type", ddlProposalType.SelectedValue);
            Procparam.Add("@itc_value", chkITC.Checked.ToString());
            Procparam.Add("@option", "1");
            Procparam.Add("@Delivery_State", ddlDeliveryState.SelectedValue);
            if (ViewState["strXML"] != null)
                if (!string.IsNullOrEmpty(ViewState["strXML"].ToString()))
                    Procparam.Add("@XMLAssetCategory", ViewState["strXML"].ToString());
            //Paging code end

            grvInvoices.BindRepeater(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            //if (bIsNewRow)
            //{
            //    grvInvoices.Rows[0].Visible = false;
            //}
            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            //ucCustomPaging.setPageSize(ProPageSizeRW);
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            txtPageSize.Text = Convert.ToString(ProPageSizeRW);

            /*
                        if (ViewState["dtInvoiceGrid"] != null)
                            dtInvoicess = (System.Data.DataTable)ViewState["dtInvoiceGrid"];
                        System.Data.DataTable dtInv = dtInvoicess.Clone();
                        intTotalRecords = dtInvoicess.Rows.Count;
                        if (dtInvoicess.Rows.Count > 0)
                        {
                            int intfrom = (ProPageNumRW - 1) * ProPageSizeRW + 1;
                            int intto = (ProPageNumRW) * ProPageSizeRW;
                            string strexpression = " slno >= " + intfrom + " and  slno <=" + intto + " ";
                            DataRow[] drInvoices = dtInvoicess.Select(hdnSearch.Value);
                            intTotalRecords = drInvoices.Length;
                            if (drInvoices.Length > 0)
                            {
                                var rows = drInvoices.CopyToDataTable().AsEnumerable().Skip(intfrom-1).Take(intto-(intfrom-1));
                                //DataRow[] drrInv = drInvoices.CopyToDataTable().Select(strexpression);
                                dtInv = rows.CopyToDataTable();
                    
                            }

                        }

                        DataView dvInvoicess = dtInv.DefaultView;



                        //Paging Config

                        FunPriGetSearchValue();

                        //This is to show grid header
                        bool bIsNewRow = false;
                        if (dvInvoicess.Count == 0)
                        {
                            dvInvoicess.AddNew();
                            bIsNewRow = true;
                        }

                        grvInvoices.DataSource = dvInvoicess;
                        grvInvoices.DataBind();

                        //This is to hide first row if grid is empty
                        if (bIsNewRow)
                        {
                            grvInvoices.Rows[0].Visible = false;
                        }

                        FunPriSetSearchValue();

                        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
                        ucCustomPaging.setPageSize(ProPageSizeRW);
                        */
            //Paging Config End
            //ModalPopupExtenderApprover.Show();
        }


        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
            cv_TabMainPage.ErrorMessage = objFaultExp.Detail.ProReasonRW;
            cv_TabMainPage.IsValid = false;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.InnerText = ex.Message;
            cv_TabMainPage.ErrorMessage = ex.Message;
            cv_TabMainPage.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    private void FunPriBindGridTotal()
    {
        //objProductMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            System.Data.DataTable dtInvoicess = new System.Data.DataTable();
            bool bIsNewRow = false;
            string strPANumber = string.Empty;
            string strProcName = "S3G_GET_Invoice_PIVI_ACC_Total";
            int intTotalRecords = 0;
            //ObjPaging.ProCompany_ID = intCompanyId;
            //ObjPaging.ProUser_ID = intUserId;
            //ObjPaging.ProTotalRecords = intTotalRecords;
            //ObjPaging.ProCurrentPage = ProPageNumRW;
            //ObjPaging.ProPageSize = ProPageSizeRW;
            //ObjPaging.ProSearchValue = txtFilterValue.Text; //Ivoice_Filter
            //ObjPaging.ProOrderBy = hdnOrderBy.Value;


            Procparam = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                strPANumber = txtPrimeAccountNo.Text;
            else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                strPANumber = ddlApplicationReferenceNo.SelectedText;
            Procparam.Add("@PAnum", strPANumber);
            Procparam.Add("@Customer_ID", ViewState["Customer_Id"].ToString());
            Procparam.Add("@Search_Type", ddlFilterType.SelectedValue);
            Procparam.Add("@SearchValue", txtFilterValue.Text);
            Procparam.Add("@Pricing_Id", ddlApplicationReferenceNo.SelectedValue);
            Procparam.Add("@Proposal_Type", ddlProposalType.SelectedValue);
            Procparam.Add("@itc_value", chkITC.Checked.ToString());
            Procparam.Add("@option", "2");
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            if (ViewState["strXML"] != null)
                if (!string.IsNullOrEmpty(ViewState["strXML"].ToString()))
                    Procparam.Add("@XMLAssetCategory", ViewState["strXML"].ToString());
            dtmovetotal = Utility.GetDefaultData("S3G_GET_Invoice_PIVI_ACC_Total", Procparam);
            ViewState["dtmovetotal"] = dtmovetotal;
        }


        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
            cv_TabMainPage.ErrorMessage = objFaultExp.Detail.ProReasonRW;
            cv_TabMainPage.IsValid = false;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.InnerText = ex.Message;
            cv_TabMainPage.ErrorMessage = ex.Message;
            cv_TabMainPage.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    private void FunPriLoadSecurityCashflows(decimal Capitalisation_Amount, decimal decSchedule_Amount_SD)
    {
        try
        {
            decimal decFinanceAmount = 0, decSecDep_Per = 0, decTotSDAmount = 0;
            if (!string.IsNullOrEmpty(txtSecDepAdvRent.Text))
                decSecDep_Per = Convert.ToDecimal(txtSecDepAdvRent.Text);
            if (!string.IsNullOrEmpty(txtFinanceAmount.Text))
                decFinanceAmount = Convert.ToDecimal(txtFinanceAmount.Text);

            DateTime dtstartdate = Utility.StringToDate(TxtFirstInstallDue.Text);
            DateTime dtenddate = Utility.StringToDate(TxtFirstInstallDue.Text);
            DataTable dtRepaymentStructure = null;
            if (ViewState["RepaymentStructure"] != null)
                dtRepaymentStructure = (DataTable)ViewState["RepaymentStructure"];


            if (dtRepaymentStructure.Rows.Count > 0)
            {
                dtstartdate = Utility.StringToDate(dtRepaymentStructure.Rows[0]["InstallmentDate"].ToString());
                //Commented on 06Mar2015 starts here
                //changes made as per OPC UAT Observation Pt No 174
                //dtenddate = Utility.StringToDate(dtRepaymentStructure.Rows[dtRepaymentStructure.Rows.Count - 1]["InstallmentDate"].ToString());
                dtenddate = Utility.StringToDate(dtRepaymentStructure.Rows[dtRepaymentStructure.Rows.Count - 1]["To_Date"].ToString());
                //Commented on 06Mar2015 ends here
            }

            string StrCashflow = "";

            //Changed for CR No - OPC_CR_061
            if (ddlSecuritydeposit.SelectedValue == "2")//Adjustable of Security Deposit
            {
                StrCashflow = "30";
                decTotSDAmount = decSchedule_Amount_SD * (decSecDep_Per / 100);
            }
            else if (ddlSecuritydeposit.SelectedValue == "3")//Refund of Security Deposit
            {
                StrCashflow = "30";
                //decTotSDAmount = Capitalisation_Amount * (decSecDep_Per / 100); //Changed for Call ID - 4500
                decTotSDAmount = decSchedule_Amount_SD * (decSecDep_Per / 100); //Changed for Call ID - 4500
            }
            else if (ddlSecuritydeposit.SelectedValue == "1" && RBLAdvanceRent.SelectedValue == "1") //Adv Rental
            {
                StrCashflow = "171";
                //decTotSDAmount = decFinanceAmount * (decSecDep_Per / 100);//Changed for Call ID - 4500
                decTotSDAmount = Convert.ToDecimal(dtRepaymentStructure.Rows[0]["InstallmentAmount"].ToString()) * decSecDep_Per;
            }

            //if (ddlSecuritydeposit.SelectedValue == "2" || ddlSecuritydeposit.SelectedValue == "3")//Adjustable/Refund
            if (StrCashflow != "")
            {
                DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
                DataRow[] drCashflow = DtCashFlow.Select("CashFlow_Flag_ID = " + StrCashflow);
                if (drCashflow.Length > 0)
                {
                    drCashflow[0].Delete();
                    DtCashFlow.AcceptChanges();
                }
                System.Data.DataSet dsSD = (System.Data.DataSet)ViewState["InflowDDL"];
                DataRow dr = DtCashFlow.NewRow();
                //DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["Date"], DtCashFlow.Columns["CashInFlowID"], 
                //    DtCashFlow.Columns["InflowFromId"], DtCashFlow.Columns["EntityID"], DtCashFlow.Columns["CashFlow_Flag_ID"] };
                //dr["Date"] = DateTime.Today.ToString();

                /* Initialized For Security Deposit CashFlow on Dec 18,2015 */
                if (StrCashflow == "30")//Security Deposit
                {
                    if (Session["strSecuDepstDate"] != null)
                    {
                        dtstartdate = Utility.StringToDate(Session["strSecuDepstDate"].ToString());
                        Session["strSecuDepstDate"] = null;
                    }
                }
                /* Initialized For Security Deposit CashFlow on Dec 18,2015 */

                dr["Date"] = dtstartdate;
                string[] strArrayIds = null;
                string cashflowdesc = "";
                foreach (DataRow drOut in dsSD.Tables[2].Rows)
                {
                    string[] strCashflow = drOut["CashFlow_ID"].ToString().Split(',');
                    if (strCashflow[4].ToString() == StrCashflow)
                    {
                        strArrayIds = strCashflow;
                        cashflowdesc = drOut["CashFlow_Description"].ToString();
                    }
                }
                if (strArrayIds == null)
                {
                    Utility.FunShowAlertMsg(this, "Define the Cashflow for " + ((Convert.ToString(StrCashflow) == "30") ? "Security Deposit" : "Advance Rental") + " in Cashflow Master");
                    return;
                }

                if (decTotSDAmount > 0 && string.IsNullOrEmpty(strSplitNum))
                {
                    dr["CashInFlowID"] = strArrayIds[0];
                    dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
                    dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
                    dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
                    dr["CashFlow_Flag_ID"] = strArrayIds[4];
                    dr["CashInFlow"] = cashflowdesc;
                    //dr["EntityID"] = hdnCustomerId.Value;
                    dr["EntityID"] = ViewState["Customer_Id"].ToString();
                    dr["Entity"] = S3GCustomerAddress1.CustomerName;
                    dr["InflowFromId"] = "144";
                    dr["InflowFrom"] = "Customer";
                    //if (ddl_Repayment_Mode.SelectedValue == "2")
                    //{
                    //    dr["Amount"] = FunPriGetStructureAdhocInterestAmount().ToString();
                    //}
                    //else
                    //{

                    //}
                    dr["Amount"] = Convert.ToDecimal(decTotSDAmount.ToString(Funsetsuffix()));

                    DtCashFlow.Rows.Add(dr);

                    gvInflow.DataSource = DtCashFlow;
                    gvInflow.DataBind();

                    ViewState["DtCashFlow"] = DtCashFlow;
                    FunPriGenerateNewInflowRow();
                }


                //For outflow
                //if (ddlSecuritydeposit.SelectedValue == "3")
                //{
                //    System.Data.DataTable dtOutflow = ((System.Data.DataTable)ViewState["DtCashFlowOut"]);

                //    try
                //    {
                //        DataRow[] drOutflowOL = dtOutflow.Select("CashFlow_Flag_ID=30");
                //        if (drOutflowOL.Length > 0)
                //        {
                //            foreach (DataRow der in drOutflowOL)
                //                der.Delete();
                //            dtOutflow.AcceptChanges();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //    if (decTotSDAmount > 0)
                //    {
                //        DataSet dsOutlfow = (DataSet)ViewState["OutflowDDL"];

                //        foreach (DataRow drOut in dsOutlfow.Tables[2].Rows)
                //        {
                //            string[] strCashflow = drOut["CashFlow_ID"].ToString().Split(',');
                //            if (strCashflow[4].ToString() == "30")
                //            {
                //                strArrayIds = strCashflow;
                //                cashflowdesc = drOut["CashFlow_Description"].ToString();
                //            }
                //        }
                //        if (strArrayIds == null)
                //        {
                //            Utility.FunShowAlertMsg(this, "Define the Cashflow for Security Deposit in Cashflow Master");
                //            return;
                //        }


                //        DataRow drOutflow = dtOutflow.NewRow();
                //        drOutflow["Date"] = dtenddate;

                //        drOutflow["CashOutFlow"] = cashflowdesc;
                //        drOutflow["EntityID"] = ViewState["Customer_Id"].ToString();
                //        drOutflow["Entity"] = S3GCustomerAddress1.CustomerName;
                //        drOutflow["OutflowFromId"] = "144";
                //        drOutflow["OutflowFrom"] = "Customer";


                //        drOutflow["Amount"] = Convert.ToDecimal(decTotSDAmount.ToString(Funsetsuffix()));

                //        drOutflow["CashOutFlowID"] = strArrayIds[0];
                //        drOutflow["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
                //        drOutflow["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
                //        drOutflow["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
                //        drOutflow["CashFlow_Flag_ID"] = strArrayIds[4];
                //        dtOutflow.Rows.Add(drOutflow);
                //        ViewState["DtCashFlowOut"] = dtOutflow;

                //        FunProBindCashFlow();
                //    }

                //}


            }
        }
        catch (Exception ex)
        {
        }
    }

    private void FunPriLoadOneTimeProcessingFeeCashflows()
    {
        try
        {
            decimal decFinanceAmount = 0, decOneTimeFee = 0, decTotSDAmount = 0;
            if (!string.IsNullOrEmpty(txtOneTimeFee.Text))
                decOneTimeFee = Convert.ToDecimal(txtOneTimeFee.Text);

            if (Convert.ToString(txtOneTimeRate.Text) != "" && grvSummaryInvoices != null && grvSummaryInvoices.FooterTemplate != null)
            {
                if (ddlAmtBasedOn.SelectedValue == "1")
                {
                    Label lblInvoiceAmt = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotInvoice_Amount") as Label;
                    //(Label)grvSummaryInvoices.FooterRow.FindControl("lblTotInvoice_Amount");
                    decOneTimeFee = Convert.ToDecimal(lblInvoiceAmt.Text) * (Convert.ToDecimal(txtOneTimeRate.Text) / 100);
                }
                else
                {
                    Label lblScheduleAmount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotSchedule_Amount") as Label;
                    //(Label)grvSummaryInvoices.FooterRow.FindControl("lblTotSchedule_Amount");
                    decOneTimeFee = Convert.ToDecimal(lblScheduleAmount.Text) * (Convert.ToDecimal(txtOneTimeRate.Text) / 100);
                }
            }

            DateTime dtstartdate = Utility.StringToDate(TxtFirstInstallDue.Text);
            DateTime dtenddate = Utility.StringToDate(TxtFirstInstallDue.Text);
            DataTable dtRepaymentStructure = null;
            if (ViewState["RepaymentStructure"] != null)
                dtRepaymentStructure = (DataTable)ViewState["RepaymentStructure"];


            if (dtRepaymentStructure.Rows.Count > 0)
            {
                dtstartdate = Utility.StringToDate(dtRepaymentStructure.Rows[0]["InstallmentDate"].ToString());
                dtenddate = Utility.StringToDate(dtRepaymentStructure.Rows[dtRepaymentStructure.Rows.Count - 1]["InstallmentDate"].ToString());
            }


            if (decOneTimeFee > 0 && string.IsNullOrEmpty(strSplitNum))
            {
                DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
                DataRow[] drCashflow = DtCashFlow.Select("CashFlow_Flag_ID = 143");
                if (drCashflow.Length > 0)
                {
                    drCashflow[0].Delete();
                    DtCashFlow.AcceptChanges();
                }
                System.Data.DataSet dsSD = (System.Data.DataSet)ViewState["InflowDDL"];
                DataRow dr = DtCashFlow.NewRow();
                //DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["Date"], DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["InflowFromId"], DtCashFlow.Columns["EntityID"], DtCashFlow.Columns["CashFlow_Flag_ID"] };
                //dr["Date"] = DateTime.Today.ToString();
                dr["Date"] = dtstartdate;
                string[] strArrayIds = null;
                string cashflowdesc = "";
                foreach (DataRow drOut in dsSD.Tables[2].Rows)
                {
                    string[] strCashflow = drOut["CashFlow_ID"].ToString().Split(',');
                    if (strCashflow[4].ToString() == "143")
                    {
                        strArrayIds = strCashflow;
                        cashflowdesc = drOut["CashFlow_Description"].ToString();
                    }
                }
                if (strArrayIds == null)
                {
                    Utility.FunShowAlertMsg(this, "Define the Cashflow for One Time Fee in Cashflow Master");
                    return;
                }




                dr["CashInFlowID"] = strArrayIds[0];
                dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
                dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
                dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
                dr["CashFlow_Flag_ID"] = strArrayIds[4];
                dr["CashInFlow"] = cashflowdesc;
                //dr["EntityID"] = hdnCustomerId.Value;
                dr["EntityID"] = ViewState["Customer_Id"].ToString();
                dr["Entity"] = S3GCustomerAddress1.CustomerName;
                dr["InflowFromId"] = "144";
                dr["InflowFrom"] = "Customer";
                //if (ddl_Repayment_Mode.SelectedValue == "2")
                //{
                //    dr["Amount"] = FunPriGetStructureAdhocInterestAmount().ToString();
                //}
                //else
                //{

                //}
                dr["Amount"] = Convert.ToDecimal(decOneTimeFee.ToString(Funsetsuffix()));

                DtCashFlow.Rows.Add(dr);

                gvInflow.DataSource = DtCashFlow;
                gvInflow.DataBind();

                ViewState["DtCashFlow"] = DtCashFlow;
                FunPriGenerateNewInflowRow();

            }
        }
        catch (Exception ex)
        {
        }
    }

    private void FunPriLoadProcessingFeeCashflows()
    {
        try
        {
            decimal decFinanceAmount = 0, decOneTimeFee = 0, decTotSDAmount = 0;
            if (!string.IsNullOrEmpty(txtProcessingFee.Text))
                decOneTimeFee = Convert.ToDecimal(txtProcessingFee.Text);

            if (Convert.ToString(txtProcessingfeeRate.Text) != "" && grvSummaryInvoices != null && grvSummaryInvoices.FooterTemplate != null)
            {
                if (ddlProcessingBasedOn.SelectedValue == "1")
                {
                    Label lblInvoiceAmt = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotInvoice_Amount") as Label;
                    //(Label)grvSummaryInvoices.FooterRow.FindControl("lblTotInvoice_Amount");
                    decOneTimeFee = Convert.ToDecimal(lblInvoiceAmt.Text) * (Convert.ToDecimal(txtProcessingfeeRate.Text) / 100);
                }
                else
                {
                    Label lblScheduleAmount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotSchedule_Amount") as Label;
                    //(Label)grvSummaryInvoices.FooterRow.FindControl("lblTotSchedule_Amount");
                    decOneTimeFee = Convert.ToDecimal(lblScheduleAmount.Text) * (Convert.ToDecimal(txtProcessingfeeRate.Text) / 100);
                }
            }

            DateTime dtstartdate = Utility.StringToDate(TxtFirstInstallDue.Text);
            DateTime dtenddate = Utility.StringToDate(TxtFirstInstallDue.Text);
            DataTable dtRepaymentStructure = null;
            if (ViewState["RepaymentStructure"] != null)
                dtRepaymentStructure = (DataTable)ViewState["RepaymentStructure"];


            if (dtRepaymentStructure.Rows.Count > 0)
            {
                dtstartdate = Utility.StringToDate(dtRepaymentStructure.Rows[0]["InstallmentDate"].ToString());
                dtenddate = Utility.StringToDate(dtRepaymentStructure.Rows[dtRepaymentStructure.Rows.Count - 1]["InstallmentDate"].ToString());
            }


            if (decOneTimeFee > 0 && string.IsNullOrEmpty(strSplitNum))
            {
                DtCashFlow = (System.Data.DataTable)ViewState["DtCashFlow"];
                DataRow[] drCashflow = DtCashFlow.Select("CashFlow_Flag_ID = 144");
                if (drCashflow.Length > 0)
                {
                    drCashflow[0].Delete();
                    DtCashFlow.AcceptChanges();
                }
                System.Data.DataSet dsSD = (System.Data.DataSet)ViewState["InflowDDL"];
                DataRow dr = DtCashFlow.NewRow();
                //DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["Date"], DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["InflowFromId"], DtCashFlow.Columns["EntityID"], DtCashFlow.Columns["CashFlow_Flag_ID"] };
                //dr["Date"] = DateTime.Today.ToString();
                dr["Date"] = dtstartdate;
                string[] strArrayIds = null;
                string cashflowdesc = "";
                foreach (DataRow drOut in dsSD.Tables[2].Rows)
                {
                    string[] strCashflow = drOut["CashFlow_ID"].ToString().Split(',');
                    if (strCashflow[4].ToString() == "144")
                    {
                        strArrayIds = strCashflow;
                        cashflowdesc = drOut["CashFlow_Description"].ToString();
                    }
                }
                if (strArrayIds == null)
                {
                    Utility.FunShowAlertMsg(this, "Define the Cashflow for Processing Fee in Cashflow Master");
                    return;
                }

                dr["CashInFlowID"] = strArrayIds[0];
                dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
                dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
                dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
                dr["CashFlow_Flag_ID"] = strArrayIds[4];
                dr["CashInFlow"] = cashflowdesc;
                //dr["EntityID"] = hdnCustomerId.Value;
                dr["EntityID"] = ViewState["Customer_Id"].ToString();
                dr["Entity"] = S3GCustomerAddress1.CustomerName;
                dr["InflowFromId"] = "144";
                dr["InflowFrom"] = "Customer";
                //if (ddl_Repayment_Mode.SelectedValue == "2")
                //{
                //    dr["Amount"] = FunPriGetStructureAdhocInterestAmount().ToString();
                //}
                //else
                //{

                //}
                dr["Amount"] = Convert.ToDecimal(decOneTimeFee.ToString(Funsetsuffix()));

                DtCashFlow.Rows.Add(dr);
                gvInflow.DataSource = DtCashFlow;
                gvInflow.DataBind();

                ViewState["DtCashFlow"] = DtCashFlow;
                FunPriGenerateNewInflowRow();
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void FunPriGetMappedAmount()
    {
        //objProductMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            string strPANumber = string.Empty;
            Procparam = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                strPANumber = txtPrimeAccountNo.Text;
            else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                strPANumber = ddlApplicationReferenceNo.SelectedText;
            Procparam.Add("@PAnum", strPANumber);
            Procparam.Add("@User_Id", intUserId.ToString());
            System.Data.DataTable dt = Utility.GetDefaultData("S3G_RS_GetMappedAmt_ACC", Procparam);
            System.Data.DataTable dt1 = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {

                    dr["MappedAmount"] = 0;
                    dr["RV_amount"] = 0;
                    dr["Capitalised_Amount"] = 0;

                    DataRow[] drr = dt.Select("Asset_Category_ID=" + dr["AssetCategory_ID"].ToString() + " OR 0 = " + dr["AssetCategory_ID"].ToString());
                    if (drr.Length > 0)
                    {
                        decimal decamount = (decimal)dt.Compute("sum(Schedule_Amount)", "Asset_Category_ID = " + dr["AssetCategory_ID"].ToString() + " OR 0 = " + dr["AssetCategory_ID"].ToString());
                        decimal decAddamount = (decimal)dt.Compute("sum(Additional_Amount)", "Asset_Category_ID=" + dr["AssetCategory_ID"].ToString() + " OR 0 =" + dr["AssetCategory_ID"].ToString());
                        decimal decCapitalized_Amount = Convert.ToDecimal(drr[0]["Capitalised_Amount"]);//Added on 09Jul2015

                        dr["MappedAmount"] = Math.Round(decamount);
                        if (dr["ResidualPer"].ToString() != "") // By Siva.K on 03JUL2015 for Error handling when ResidualPer is null
                        {
                            //dr["RV_amount"] = (Convert.ToDecimal(dr["ResidualPer"].ToString()) * Math.Round(decamount)) / 100;
                            dr["RV_amount"] = Math.Round((Convert.ToDecimal(dr["ResidualPer"].ToString()) * Math.Round(decCapitalized_Amount, 4)) / 100, 4);//Added on 09Jul2015
                        }
                        dr["Selected"] = "1"; // By Siva.K on 03JUL2015 for Select the Check box for Asset Category 
                        dr["Capitalised_Amount"] = Convert.ToDecimal(drr[0]["Capitalised_Amount"]); //Added on 09Jul2015
                    }
                }
                dt1.AcceptChanges();
            }
            else
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    dr["MappedAmount"] = 0;
                    dr["RV_amount"] = 0;
                }
                dt1.AcceptChanges();

            }
            ViewState["dtPrimaryGrid"] = dt1;
            DataRow[] drprim = dt1.Select("Offer_Type=1");
            if (drprim.Length > 0)
            {
                dtPrimaryGrid = drprim.CopyToDataTable();
                ViewState["dtPrimaryGridsum"] = dtPrimaryGrid;
                FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                FunprisumgrvPrimaryGrid();
            }
            DataRow[] drsec = dt1.Select("Offer_Type=2");
            if (drsec.Length > 0)
            {
                dtSecondaryGrid = drsec.CopyToDataTable();
                ViewState["dtSecondaryGridsum"] = dtSecondaryGrid;
                FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                FunprisumgrvSecondaryGrid();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }


    private void FunPriBindGridSummary()
    {
        //objProductMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            System.Data.DataTable dtInvoicess = new System.Data.DataTable();
            bool bIsNewRow = false;
            string strProcName = "S3G_GET_Invoice_Summary_ACC";
            int intTotalRecords = 0;
            ObjPagingSummary.ProCompany_ID = intCompanyId;
            ObjPagingSummary.ProUser_ID = intUserId;
            ObjPagingSummary.ProTotalRecords = intTotalRecords;
            ObjPagingSummary.ProCurrentPage = ProPageNumRWSummary;
            ObjPagingSummary.ProPageSize = ProPageSizeRWSummary;
            //ObjPagingSummary.ProSearchValue = txtFilterValue.Text; //taxes appliacable
            ObjPagingSummary.ProOrderBy = hdnOrderBy.Value;
            string strPANumber = string.Empty;
            Procparam = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                strPANumber = txtPrimeAccountNo.Text;
            else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                strPANumber = ddlApplicationReferenceNo.SelectedText;
            Procparam.Add("@PAnum", strPANumber);
            string strPurTax = string.Empty;
            if (chkLBT.Checked)
                strPurTax += "+ISNULL(LBT_Amount,0)";
            if (chkPT.Checked)
                strPurTax += "+ISNULL(Purchase_Tax,0)";
            if (chkET.Checked)
                strPurTax += "+ISNULL(Entry_Tax_Amount,0)";
            if (chkRC.Checked)
                strPurTax += "+ISNULL(Reverse_Charge_Tax,0)";
            if (chkbxGSTRC.Checked)
                strPurTax += "+ISNULL(RC_SGST_Amt,0) + ISNULL(RC_CGST_Amt,0) + ISNULL(RC_IGST_Amt,0) + ISNULL(RC_SAC_SGST_Amt,0) + ISNULL(RC_SAC_CGST_Amt,0) + ISNULL(RC_SAC_IGST_Amt,0)";
            //if (chkITC.Checked)
            //    strPurTax += "+ISNULL(-ITC_Value,0)";

            if (chkITC.Checked)
                Procparam.Add("@ITC_Req", "1");

            if (chkITC_Cap.Checked)
                Procparam.Add("@ITC_Cap", "1");

            if (!string.IsNullOrEmpty(strPurTax))
            {
                if (strPurTax.Contains("+"))
                    strPurTax = strPurTax.Substring(1, strPurTax.Length - 1);
                Procparam.Add("@sanum", strPurTax);
            }

            //Paging code end
            if (!string.IsNullOrEmpty(strSplitNum))
                Procparam.Add("@Split_No", strSplitNum);
            if (!string.IsNullOrEmpty(strSplitRefNo))
                Procparam.Add("@Option", strSplitRefNo);
            Procparam.Add("@RS_State", ddlBranchList.SelectedValue);
            if (ViewState["vatleasing"] != null)
                Procparam.Add("@VATLeasing", ViewState["vatleasing"].ToString());
            else
                Procparam.Add("@VATLeasing", "0");

            Procparam.Add("@Mode", Convert.ToString(strMode));

            //grvSummaryInvoices.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPagingSummary, out bIsNewRow);

            grvSummaryInvoices.BindRepeater(strProcName, Procparam, out intTotalRecords, ObjPagingSummary, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                foreach (RepeaterItem item in grvSummaryInvoices.Items)
                {
                    LinkButton LkButton = (LinkButton)item.FindControl("lnRemoveRepayment");
                    LkButton.Visible = false;
                }
                //grvSummaryInvoices.Rows[0].Visible = false;
            }
            ucCustomPagingSummary.Visible = true;
            ucCustomPagingSummary.Navigation(intTotalRecords, ProPageNumRWSummary, ProPageSizeRWSummary);
            //ucCustomPagingSummary.setPageSize(ProPageSizeRWSummary);
            TextBox txtPageSizeSummary = (TextBox)ucCustomPagingSummary.FindControl("txtPageSize");
            txtPageSizeSummary.Text = Convert.ToString(ProPageSizeRWSummary);
            FunPriSumgrvSummaryInvoices();

            /*
                        if (ViewState["dtInvoiceGrid"] != null)
                            dtInvoicess = (System.Data.DataTable)ViewState["dtInvoiceGrid"];
                        System.Data.DataTable dtInv = dtInvoicess.Clone();
                        intTotalRecords = dtInvoicess.Rows.Count;
                        if (dtInvoicess.Rows.Count > 0)
                        {
                            int intfrom = (ProPageNumRW - 1) * ProPageSizeRW + 1;
                            int intto = (ProPageNumRW) * ProPageSizeRW;
                            string strexpression = " slno >= " + intfrom + " and  slno <=" + intto + " ";
                            DataRow[] drInvoices = dtInvoicess.Select(hdnSearch.Value);
                            intTotalRecords = drInvoices.Length;
                            if (drInvoices.Length > 0)
                            {
                                var rows = drInvoices.CopyToDataTable().AsEnumerable().Skip(intfrom-1).Take(intto-(intfrom-1));
                                //DataRow[] drrInv = drInvoices.CopyToDataTable().Select(strexpression);
                                dtInv = rows.CopyToDataTable();
                    
                            }

                        }

                        DataView dvInvoicess = dtInv.DefaultView;



                        //Paging Config

                        FunPriGetSearchValue();

                        //This is to show grid header
                        bool bIsNewRow = false;
                        if (dvInvoicess.Count == 0)
                        {
                            dvInvoicess.AddNew();
                            bIsNewRow = true;
                        }

                        grvInvoices.DataSource = dvInvoicess;
                        grvInvoices.DataBind();

                        //This is to hide first row if grid is empty
                        if (bIsNewRow)
                        {
                            grvInvoices.Rows[0].Visible = false;
                        }

                        FunPriSetSearchValue();

                        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
                        ucCustomPaging.setPageSize(ProPageSizeRW);
                        */
            //Paging Config End

        }


        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
            cv_TabMainPage.ErrorMessage = objFaultExp.Detail.ProReasonRW;
            cv_TabMainPage.IsValid = false;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.InnerText = ex.Message;
            cv_TabMainPage.ErrorMessage = ex.Message;
            cv_TabMainPage.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }



    private void funclearDeliveryAddress()
    {
        try
        {
            bool blnflag = false;
            txtAddress1DA.Text = txtMobileDA.Text = txtTelephoneDA.Text = txtLabel.Text = txtAddress.Text = txtPin.Text = string.Empty;

            //txtAddress2DA.Text = txtCityDA.Text = txtCountryDA.Text =
            //    txtPinCodeDA.Text = ddlState.SelectedIndex = -1;

            if (ddlDeliveryType.SelectedValue == "1" || ddlDeliveryType.SelectedValue == "2")
            {
                blnflag = true;
            }
            txtAddress1DA.ReadOnly = txtMobileDA.ReadOnly = txtTelephoneDA.ReadOnly = txtPin.ReadOnly = blnflag;
            //txtAddress2DA.ReadOnly = txtCityDA.ReadOnly = txtCountryDA.ReadOnly =
            //    txtPinCodeDA.ReadOnly =
            //rfvtxtAddress1DA.Enabled = !blnflag;
            //rfvtxtCityDA.Enabled = rfvtxtCountryDA.Enabled = rfvtxtPinCodeDA.Enabled = rfvddlState.Enabled =

            //ddlState.Enabled 
            //rfvddlCust_Address.Enabled = false;
            //if (ddlDeliveryType.SelectedValue == "2")//Billing
            //{
            //    rfvddlCust_Address.Enabled = true;
            //}

        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlDeliveryType_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            funclearDeliveryAddress();
            if (ddlCust_Address.Items.Count > 0)
                ddlCust_Address.Items.Clear();
            if (ddlDeliveryType.SelectedValue == "1" || ddlDeliveryType.SelectedValue == "2")//Billing/Corporate
            {
                System.Data.DataTable dtDeliveryAddress = null;
                if (ViewState["DeliveryAddress"] != null)
                    dtDeliveryAddress = (System.Data.DataTable)ViewState["DeliveryAddress"];

                if (dtDeliveryAddress != null)
                {
                    if (dtDeliveryAddress.Rows.Count > 0)
                    {
                        if (ddlDeliveryType.SelectedValue == "2")//Billing address
                        {
                            //rfvddlCust_Address.Enabled = true;
                            DataRow[] dt = dtDeliveryAddress.Select("Address_Type=2");
                            if (dt.Length > 0)
                            {
                                Utility.FillDataTable(ddlCust_Address, dt.CopyToDataTable(), "State", "State_Name");
                            }
                        }
                        else//Corprate Address
                        {
                            DataRow[] dt = dtDeliveryAddress.Select("Address_Type=1");
                            if (dt.Length > 0)
                            {
                                FunPriLoadDeliverAddress(dt.CopyToDataTable());
                            }
                        }
                    }
                }


            }

        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to get delivery address";
            cv_TabMainPage.IsValid = false;
        }
    }


    //protected void ddlSalesTax_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        rfvddlSEZZone.Enabled = false;
    //        if (ddlSalesTax.Text == "1" || ddlSalesTax.Text == "2")//VAT //CST
    //        {
    //            if (ViewState["Parent_Location_ID"] != null)
    //            {
    //                string strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();
    //                if (strParent_Location_ID == ddlDeliveryState.SelectedValue)
    //                {
    //                    if (ddlSalesTax.Text == "2")
    //                    {
    //                        Utility.FunShowAlertMsg(this, "Sales tax should be VAT");
    //                        ddlSalesTax.SelectedIndex = -1;
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    if (ddlSalesTax.Text == "1")
    //                    {
    //                        Utility.FunShowAlertMsg(this, "Sales tax should be CST");
    //                        ddlSalesTax.SelectedIndex = -1;
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (ddlSalesTax.Text == "4")//SEZ
    //                rfvddlSEZZone.Enabled = true;
    //        }
    //        if (ddlSalesTax.Text == "2") //CST
    //        {
    //            chkcstwith.Enabled = true;
    //            //chkcstwithout.Enabled = true;
    //        }
    //        else
    //        {
    //            chkcstwith.Enabled = false;
    //            //chkcstwithout.Enabled = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    protected void ddlCust_Address_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            funclearDeliveryAddress();
            if (ddlDeliveryType.SelectedValue == "2")//Billing/Corporate
            {

                if (ViewState["DeliveryAddress"] != null)
                    dtDeliveryAddress = (System.Data.DataTable)ViewState["DeliveryAddress"];

                if (dtDeliveryAddress != null)
                {
                    if (dtDeliveryAddress.Rows.Count > 0)
                    {
                        DataRow[] dt = dtDeliveryAddress.Select("Address_Type=2 and State=" + ddlCust_Address.SelectedValue);
                        if (dt.Length > 0)
                        {
                            FunPriLoadDeliverAddress(dt.CopyToDataTable());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to get delivery address";
            cv_TabMainPage.IsValid = false;
        }
    }


    private void FunPriLoadDeliverAddress(System.Data.DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["Address1"].ToString()))
                    txtAddress1DA.Text = dt.Rows[0]["Address1"].ToString();
                //if (!string.IsNullOrEmpty(dt.Rows[0]["Address2"].ToString()))
                //    txtAddress2DA.Text = dt.Rows[0]["Address2"].ToString();
                //if (!string.IsNullOrEmpty(dt.Rows[0]["City"].ToString()))
                //    txtCityDA.Text = dt.Rows[0]["City"].ToString();
                //if (!string.IsNullOrEmpty(dt.Rows[0]["State"].ToString()))
                //    ddlState.SelectedValue = dt.Rows[0]["State"].ToString();//state
                //if (!string.IsNullOrEmpty(dt.Rows[0]["Country"].ToString()))
                //    txtCountryDA.Text = dt.Rows[0]["Country"].ToString();
                //if (!string.IsNullOrEmpty(dt.Rows[0]["Pincode"].ToString()))
                //    txtPinCodeDA.Text = dt.Rows[0]["Pincode"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Mobile"].ToString()))
                    txtMobileDA.Text = dt.Rows[0]["Mobile"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Telephone"].ToString()))
                    txtTelephoneDA.Text = dt.Rows[0]["Telephone"].ToString();
                //if (!string.IsNullOrEmpty(dt.Rows[0]["Pin"].ToString()))
                //    txtPin.Text = dt.Rows[0]["Pin"].ToString();
            }
        }
        catch (Exception ex)
        {
        }
    }


    //private void FunPriGetApplicationDetails(int intApplicationId)
    //{
    //    DataSet dsApplicationDetails = new DataSet();
    //    S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
    //    //Code Added by Ganapathy on 08/06/2012
    //    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
    //    //END
    //    try
    //    {

    //        if (intApplicationId == 0)
    //        {
    //            if (strConsNumber != null)
    //            {
    //                dsApplicationDetails = FunPriGetAccountConsolidationDetails(strConsNumber);
    //            }
    //            else if (strSplitNum != null)
    //            {
    //                dsApplicationDetails = FunPriGetAccountSplitDetails(strSplitNum, strSplitRefNo);
    //            }
    //            rfvApplicationReferenceNo.Enabled = false;
    //            ViewState["ConsolidateApplicationProcessId"] = dsApplicationDetails.Tables[0].Rows[0]["Application_Process_ID"];
    //        }
    //        else
    //        {
    //            Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //            Procparam.Add("@ApplicationProcessId", intApplicationId.ToString());
    //            if (intAccountCreationId > 0)
    //            {
    //                Procparam.Add("@CompanyId", intCompanyId.ToString());
    //                Procparam.Add("@PANum", strPANum);
    //                Procparam.Add("@SANum", (string.IsNullOrEmpty(strSANum)) ? "0" : strSANum);
    //                Procparam.Add("@AccountCreationId", intAccountCreationId.ToString());
    //            }
    //            dsApplicationDetails = Utility.GetDataset("S3G_LoanAd_LoadAccountDetails", Procparam);
    //        }
    //        if (dsApplicationDetails != null)
    //        {
    //            if (dsApplicationDetails.Tables[0].Rows.Count == 0)
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Activate the Account for selected Application Reference No')", true);
    //                return;
    //            }
    //            ListItem lstItem;

    //            #region MainPage Tab
    //            ddlLOB.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["LOB_Id"]);
    //            //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACTORING"))
    //            /*if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACTORING"))
    //            {
    //                chkDORequired.Enabled = false;
    //                TabContainerMainTab.Tabs[1].Visible = false;
    //            }
    //            else
    //            {
    //                chkDORequired.Enabled = true;
    //                TabContainerMainTab.Tabs[1].Visible = true;
    //            }*/
    //            ddlBranchList.SelectedText = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Name"]);
    //            ddlBranchList.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Id"]);
    //            ddlBranchList.ToolTip = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Location_Name"]);
    //            //txtFBDate.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["FBDate"]);
    //            //txtFBDate.Text = (txtFBDate.Text == "0") ? "" : txtFBDate.Text;
    //            if (strConsNumber != null || strSplitNum != null)
    //            {
    //                ddlLOB.ClearDropDownList();
    //                //Removed By Shibu 17-Sep-2013
    //                //ddlBranchList.ClearDropDownList();
    //                // ddlBranchList.Clear(); 
    //                rfvTenure.Enabled = true;
    //                rfvTenureType.Enabled = true;
    //            }
    //            if (intApplicationId == 0)
    //            {
    //                FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedValue);
    //            }

    //            FunPriLoadStatusDDL();
    //            if (dsApplicationDetails.Tables[0].Columns.Contains("PA_Status_Code"))
    //            {
    //                if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PA_Status_Code"]) != "")
    //                {
    //                    ddlStatus.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PA_Status_Code"]);
    //                    txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                    if (ddlStatus.SelectedValue == "11")
    //                    {
    //                        txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                        //tcAccountCreation.Tabs[7].Visible = true;
    //                    }
    //                }
    //            }

    //            if (intAccountCreationId > 0 || strSplitNum != null || strConsNumber != null)
    //            {

    //                FunPriLoadApplicationNoDDL(Convert.ToInt32(ddlBranchList.SelectedValue), Convert.ToInt32(ddlLOB.SelectedValue));
    //                ddlApplicationReferenceNo.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Application_Process_Id"]);
    //                if (strSplitNum != null || strConsNumber != null)
    //                {
    //                    txtApplication_Followup.Text = ddlApplicationReferenceNo.SelectedItem.Text;
    //                    ddlApplicationReferenceNo.ClearDropDownList();
    //                }
    //                txtPrimeAccountNo.Text = strPANum;
    //                //if (strSANum != "")
    //                //{
    //                //    txtSubAccountNo.Text = strSANum;
    //                //    //tcAccountCreation.Tabs[7].Visible = true;
    //                //}
    //                //else
    //                //{
    //                //    //tcAccountCreation.Tabs[7].Visible = false;
    //                //}
    //                //if (strSplitNum == null)
    //                txtAccountDate.Text = DateTime.Parse(Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["AccountDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    //                if (dsApplicationDetails.Tables.Count > 17)
    //                {
    //                    if (dsApplicationDetails.Tables[17].Rows.Count > 0)
    //                    {

    //                        ddlStatus.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["SA_Status_Code"]);
    //                        txtStatus.Text = ddlStatus.SelectedItem.Text;


    //                        txtAdvanceInstallments.Text = Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["Advance_Installments"]);
    //                        txtAdvanceInstallments.Text = (txtAdvanceInstallments.Text == "0") ? "" : txtAdvanceInstallments.Text;
    //                        ddlRepaymentMode.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["Repayment_Code"]);
    //                        if (!string.IsNullOrEmpty(Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["Last_ODI_Date"])))
    //                        {
    //                            txtLastODICalcDate.Text = DateTime.Parse(Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["Last_ODI_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    //                        }
    //                        if (!string.IsNullOrEmpty(dsApplicationDetails.Tables[17].Rows[0]["Is_delivery_Order_Require"].ToString()))
    //                        {
    //                            chkDORequired.Checked = Convert.ToBoolean(dsApplicationDetails.Tables[17].Rows[0]["Is_delivery_Order_Require"]);
    //                        }

    //                    }

    //                }
    //            }

    //            //FunPriLoadApplicationNoDDL(Convert.ToInt32(ddlBranchList.SelectedValue), Convert.ToInt32(ddlLOB.SelectedValue));
    //            //ddlApplicationReferenceNo.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Application_Process_Id"]);
    //            if (strSplitNum != null)
    //            {
    //                ddlApplicationReferenceNo.ClearDropDownList();
    //            }
    //            //txtApplicationDate.Text = DateTime.Parse(Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    //            if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["MLA_Number"]) != "0")
    //            {
    //                txtPrimeAccountNo.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["MLA_Number"]);
    //                if (ViewState["strMLASLAApplicable"] != null)
    //                {
    //                    if (ViewState["strMLASLAApplicable"].ToString().ToUpper() == "FALSE")
    //                    {
    //                        ddlStatus.SelectedValue = "2";
    //                    }
    //                    else
    //                    {
    //                        ddlStatus.SelectedValue = "10";
    //                    }
    //                }
    //                txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                //tcAccountCreation.Tabs[7].Visible = false;
    //            }
    //            if (dsApplicationDetails.Tables[0].Columns.Contains("PA_Status_Code"))
    //            {
    //                if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PA_Status_Code"]) != "")
    //                {
    //                    ddlStatus.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PA_Status_Code"]);
    //                    txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                    if (ddlStatus.SelectedValue == "11")
    //                    {
    //                        txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                        //tcAccountCreation.Tabs[7].Visible = true;
    //                    }
    //                }
    //            }
    //            if (dsApplicationDetails.Tables[0].Columns.Contains("MLA_Applicable"))
    //            {
    //                if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["MLA_Applicable"]) == "0")
    //                {
    //                    //tcAccountCreation.Tabs[7].Visible = false;
    //                }
    //            }

    //            if (dsApplicationDetails.Tables[0].Columns.Contains("PANum"))
    //            {
    //                if (Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PANum"]) != "")
    //                {
    //                    txtPrimeAccountNo.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["PANum"]);
    //                }
    //            }
    //            if (intAccountCreationId > 0)
    //            {
    //                if (dsApplicationDetails.Tables.Count > 17)
    //                {
    //                    if (dsApplicationDetails.Tables[17].Rows.Count > 0)
    //                    {
    //                        FunPriLoadStatusDDL();
    //                        ddlStatus.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[17].Rows[0]["SA_Status_Code"]);
    //                        txtStatus.Text = ddlStatus.SelectedItem.Text;
    //                    }

    //                }
    //            }
    //            if (intApplicationId == 0)
    //            {
    //                if (strSplitNum != null)
    //                {
    //                    if ((string)ViewState["SANUM"] == (string)ViewState["PANUM"] + "DUMMY")
    //                    {
    //                        txtPrimeAccountNo.Text = "";
    //                    }
    //                }
    //                if (FunPriCheckMLA((string)ViewState["PANUM"]))
    //                {
    //                    txtPrimeAccountNo.Text = "";
    //                }

    //            }
    //            hdnCustomerId.Value = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Customer_Id"]);
    //            S3GCustomerAddress1.SetCustomerDetails(dsApplicationDetails.Tables[0].Rows[0], true);
    //            Session["AccountAssetCustomer"] = hdnCustomerId.Value + ";" + S3GCustomerAddress1.CustomerName;
    //            txtAddress1.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_address1"]);
    //            txtAddress2.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_address2"]);
    //            txtState.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_state"]);
    //            txtPincode.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_pincode"]);
    //            txtCity.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_city"]);
    //            txtCountry.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["comm_country"]);


    //            hdnProductId.Value = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Product_Id"]);
    //            txtProductCode.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Product"]);
    //            if (intApplicationId > 0 || strSplitNum != null)
    //            {
    //                if (intApplicationId > 0)
    //                {
    //                    txtAccountingIRR.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Accounting_IRR"]);
    //                    txtCompanyIRR.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Company_IRR"]);
    //                    txtBusinessIRR.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Business_IRR"]);
    //                    if (intAccountCreationId == 0)
    //                    {
    //                        txtFinanceAmount.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Finance_Amount"]);
    //                    }
    //                    else
    //                    {
    //                        txtFinanceAmount.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Loan_Amount"]);
    //                    }

    //                }
    //                txtTenure.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Tenure"]);
    //                //Added by Ganapathy on 08/06/2012 BEGIN
    //                ObjStatus.Option = 1;
    //                ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
    //                Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
    //                //END
    //                ddlTenureType.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Tenure_Type"]);
    //                txtTenure.ReadOnly = true;
    //                ddlTenureType.ClearDropDownList();
    //            }
    //            else
    //            {
    //                txtTenure.ReadOnly = false;
    //            }

    //            ddlSalePersonCodeList.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Sales_Person_Id"]);
    //            ddlSalePersonCodeList.SelectedText = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Sales_Person_Name"]);
    //            txtConstitutionCode.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Constitution"]);
    //            hdnConstitutionId.Value = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Constitution_Id"]);


    //            if (strConsNumber == null)
    //            {
    //                txtResidualAmt_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Residual_Value"]);
    //                txtResidualAmt_Cashflow.Text = (txtResidualAmt_Cashflow.Text == "0") ? "" : txtResidualAmt_Cashflow.Text;
    //                txtResidualValue_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Residual_Value"]);
    //                txtResidualValue_Cashflow.Text = (txtResidualValue_Cashflow.Text.StartsWith("0")) ? "" : txtResidualValue_Cashflow.Text;

    //                txtMarginMoneyAmount_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Margin_Amount"]);
    //                txtMarginMoneyAmount_Cashflow.Text = (txtMarginMoneyAmount_Cashflow.Text.StartsWith("0")) ? "" : txtMarginMoneyAmount_Cashflow.Text;
    //                txtMarginMoneyPer_Cashflow.Text = Convert.ToString(dsApplicationDetails.Tables[0].Rows[0]["Offer_Margin"]);
    //                txtMarginMoneyPer_Cashflow.Text = (txtMarginMoneyPer_Cashflow.Text.StartsWith("0")) ? "" : txtMarginMoneyPer_Cashflow.Text;
    //            }

    //            grvConsDocuments.DataSource = dsApplicationDetails.Tables[14];
    //            grvConsDocuments.DataBind();
    //            #endregion

    //            #region OfferTerms Tab
    //            if (intApplicationId > 0 || strSplitNum != null)
    //            {
    //                ddlROIRuleList.Items.Clear();
    //                lstItem = new ListItem(dsApplicationDetails.Tables[3].Rows[0]["ROI_Number"].ToString(), dsApplicationDetails.Tables[3].Rows[0]["ROI_Rules_ID"].ToString());
    //                ddlROIRuleList.Items.Add(lstItem);
    //                ddlROIRuleList.SelectedValue = Convert.ToString(dsApplicationDetails.Tables[3].Rows[0]["ROI_Rules_ID"]);
    //                ViewState["ROIRules"] = dsApplicationDetails.Tables[3];
    //                FunPriBindROIDLL(_Add);
    //                Load_ROI_Rule();
    //            }
    //            if (strSplitNum == null)
    //            {

    //                ddlROIRuleList.ClearDropDownList();

    //            }
    //            if (strConsNumber != null)
    //            {
    //                ddlLOB.ClearDropDownList();
    //                //Removed By Shibu 17-Sep-2013
    //                //ddlBranchList.ClearDropDownList();
    //                ddlBranchList.Clear();
    //                FunPriBindInflowDLL(_Add);
    //                FunPriBindOutflowDLL(_Add);
    //                FunPriBindRepaymentDLL(_Add);
    //                FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedValue);
    //                FunPriBindROIDLL(_Add);
    //                Load_ROI_Rule();
    //            }
    //            ddlPaymentRuleList.Items.Clear();
    //            if (dsApplicationDetails.Tables[4].Rows.Count > 0)
    //            {
    //                lstItem = new ListItem(dsApplicationDetails.Tables[4].Rows[0]["Payment_Rule_Number"].ToString(), dsApplicationDetails.Tables[4].Rows[0]["Payment_RuleCard_ID"].ToString());
    //                ddlPaymentRuleList.Items.Add(lstItem);
    //            }
    //            FunPriSetRateLength();

    //            ViewState["DtCashFlow"] = dsApplicationDetails.Tables[1];
    //            if (dsApplicationDetails.Tables[1].Rows.Count > 0)
    //            {
    //                if (intAccountCreationId == 0)
    //                {
    //                    FunPriBindInflowDLL(_Add);
    //                }
    //                else
    //                {
    //                    FunPriBindInflowDLL(_Edit);
    //                }
    //                gvInflow.DataSource = dsApplicationDetails.Tables[1];
    //                gvInflow.DataBind();
    //                FunPriGenerateNewInflowRow();
    //                ViewState["DtCashFlow"] = dsApplicationDetails.Tables[1];
    //            }
    //            else
    //            {
    //                FunPriBindInflowDLL(_Add);
    //            }
    //            ////added by saranya
    //            //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") && ddl_Repayment_Mode.SelectedItem.Text.StartsWith("Pro")))
    //            //{
    //            //    Button btnAdd = gvInflow.FooterRow.FindControl("btnAdd") as Button;
    //            //    btnAdd.Enabled = false;
    //            //}
    //            //end
    //            ViewState["DtCashFlowOut"] = dsApplicationDetails.Tables[2];
    //            if (dsApplicationDetails.Tables[2].Rows.Count > 0)
    //            {
    //                if (intAccountCreationId == 0)
    //                {
    //                    FunPriBindOutflowDLL(_Add);
    //                }
    //                else
    //                {
    //                    FunPriBindOutflowDLL(_Edit);
    //                }
    //                gvOutFlow.DataSource = dsApplicationDetails.Tables[2];
    //                gvOutFlow.DataBind();
    //                FunPriGenerateNewOutflowRow();
    //            }
    //            else
    //            {
    //                FunPriBindOutflowDLL(_Add);
    //            }


    //            ViewState["DtCashFlowOut"] = dsApplicationDetails.Tables[2];
    //            lblTotalOutFlowAmount.Text = dsApplicationDetails.Tables[9].Rows[0].ItemArray[0].ToString();
    //            #endregion

    //            FunPriLoadRepaymentMode();
    //            if (dsApplicationDetails.Tables[4].Rows.Count > 0)
    //            {
    //                Load_Payment_Rule();
    //            }
    //            txtLOB_Followup.Text = ddlLOB.SelectedItem.Text;
    //            //  txtBranch_Followup.Text = ddlBranchList.SelectedItem.Text;
    //            txtBranch_Followup.Text = ddlBranchList.SelectedText;
    //            txtApplication_Followup.Text = ddlApplicationReferenceNo.SelectedItem.Text;
    //            if (intApplicationId > 0 || (strSplitRefNo != null))
    //            {

    //                #region Repayment Tab

    //                ViewState["DtRepayGrid"] = dsApplicationDetails.Tables[5];
    //                if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
    //                {
    //                    ViewState["DtRepayGrid_TL"] = dsApplicationDetails.Tables[5];
    //                }
    //                if (intAccountCreationId == 0)
    //                {
    //                    //FunPriBindRepaymentDLL(_Add);
    //                }
    //                else
    //                {
    //                    if (dsApplicationDetails.Tables[5].Rows.Count > 0)
    //                    {
    //                        //FunPriBindRepaymentDLL(_Edit);
    //                    }
    //                }
    //                if (dsApplicationDetails.Tables[5].Rows.Count > 0)
    //                {
    //                    gvRepaymentDetails.DataSource = dsApplicationDetails.Tables[5];
    //                    gvRepaymentDetails.DataBind();
    //                    FunPriGenerateNewRepaymentRow();
    //                    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
    //                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
    //                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
    //                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
    //                    if (intApplicationId > 0)
    //                    {
    //                        txtAccountIRR_Repay.Text = txtAccountingIRR.Text = dsApplicationDetails.Tables[0].Rows[0]["Accounting_IRR"].ToString();
    //                        txtBusinessIRR_Repay.Text = txtBusinessIRR.Text = dsApplicationDetails.Tables[0].Rows[0]["Business_IRR"].ToString();
    //                        txtCompanyIRR_Repay.Text = txtCompanyIRR.Text = dsApplicationDetails.Tables[0].Rows[0]["Company_IRR"].ToString();
    //                    }
    //                    if (ddl_Repayment_Mode.SelectedValue != "2")
    //                    {
    //                        Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
    //                        if (lblCashFlowId.Text != "23")
    //                        {
    //                            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
    //                        }
    //                        else
    //                        {
    //                            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
    //                    }
    //                    ViewState["DtRepayGrid"] = dsApplicationDetails.Tables[5];

    //                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
    //                    {
    //                        ViewState["DtRepayGrid_TL"] = dsApplicationDetails.Tables[5];
    //                    }

    //                    if (strConsNumber != null || strSplitNum != null)
    //                    {
    //                        FunPriCalculateSummary(dsApplicationDetails.Tables[5], "CashFlow", "TotalPeriodInstall");
    //                    }
    //                    else
    //                    {
    //                        gvRepaymentSummary.DataSource = dsApplicationDetails.Tables[11];
    //                        gvRepaymentSummary.DataBind();
    //                    }
    //                    if (ddl_Rate_Type.Visible && ddl_Rate_Type.SelectedItem.Text == "Floating" && string.IsNullOrEmpty(txtFBDate.Text))
    //                    {
    //                        ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
    //                        ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
    //                    }
    //                    else
    //                    {
    //                        ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
    //                        ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
    //                    }
    //                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))))
    //                    {
    //                        DataRow[] dr = dsApplicationDetails.Tables[5].Select("CashFlow_Flag_ID IN(35,91)");
    //                        if (dr.Length == 0)
    //                        {
    //                            FunPriShowRepaymetDetails((decimal)dsApplicationDetails.Tables[5].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
    //                        }
    //                        else
    //                        {
    //                            FunPriShowRepaymetDetails((decimal)dsApplicationDetails.Tables[5].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =91"));
    //                        }

    //                    }
    //                    else
    //                    {
    //                        FunPriShowRepaymetDetails((decimal)dsApplicationDetails.Tables[5].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
    //                    }
    //                }

    //                #endregion


    //                #region Guarantor Tab


    //                ViewState["dtGuarantorGrid"] = dsApplicationDetails.Tables[12];
    //                if (dsApplicationDetails.Tables[12].Rows.Count > 0)
    //                {
    //                    if (intAccountCreationId == 0)
    //                    {

    //                        FunPriBindGuarantorDLL(_Add);
    //                    }
    //                    else
    //                    {
    //                        FunPriBindGuarantorDLL(_Edit);
    //                    }
    //                    gvGuarantor.DataSource = dsApplicationDetails.Tables[12];
    //                    gvGuarantor.DataBind();
    //                    ViewState["dtGuarantorGrid"] = dsApplicationDetails.Tables[12];
    //                    ViewState["GuarantorDetails"] = dsApplicationDetails.Tables[12];
    //                    FunPriGenerateNewGuarantorRow();
    //                }
    //                else
    //                {
    //                    FunPriBindGuarantorDLL(_Add);
    //                }

    //                #endregion

    //                if (intApplicationId > 0)
    //                {

    //                    #region Alerts Tab

    //                    if (dsApplicationDetails.Tables[6].Rows.Count == 0)
    //                    {
    //                        FunPriBindAlertDLL(_Add);
    //                    }
    //                    else
    //                    {
    //                        ViewState["DtAlertDetails"] = dsApplicationDetails.Tables[6];
    //                        if (intAccountCreationId == 0)
    //                        {

    //                            FunPriBindAlertDLL(_Add);
    //                        }
    //                        else
    //                        {
    //                            FunPriBindAlertDLL(_Edit);
    //                        }
    //                        gvAlert.DataSource = dsApplicationDetails.Tables[6];
    //                        gvAlert.DataBind();
    //                        FunPriGenerateNewAlertRow();
    //                        ViewState["DtAlertDetails"] = dsApplicationDetails.Tables[6];
    //                    }
    //                    #endregion

    //                    #region Followup Tab

    //                    if (dsApplicationDetails.Tables[7].Rows.Count > 0)
    //                    {
    //                        txtAccount_Followup.Text = txtPrimeAccountNo.Text;
    //                        txtEnquiry_Followup.Text = dsApplicationDetails.Tables[7].Rows[0]["Enquiry_Number"].ToString();
    //                        txtOfferNo_Followup.Text = dsApplicationDetails.Tables[7].Rows[0]["Offer_Number"].ToString();
    //                        txtApplication_Followup.Text = dsApplicationDetails.Tables[7].Rows[0]["Application_Number"].ToString();
    //                        if (!string.IsNullOrEmpty(dsApplicationDetails.Tables[7].Rows[0]["Date"].ToString()))
    //                            txtEnquiryDate_Followup.Text = Convert.ToDateTime(dsApplicationDetails.Tables[7].Rows[0]["Date"].ToString()).ToString(strDateFormat);
    //                        txtCustNameAdd_Followup.Text = S3GCustomerAddress1.CustomerName + "," + "\n" + txtAddress1.Text + " " + txtAddress2.Text + "," + "\n" + txtCity.Text
    //                                                       + "-" + txtPincode.Text + "\n" + txtState.Text + "\n" + txtCountry.Text;
    //                    }
    //                    if (dsApplicationDetails.Tables[8].Rows.Count == 0)
    //                    {
    //                        FunPriBindFollowupDLL(_Add);
    //                    }
    //                    else
    //                    {
    //                        ViewState["DtFollowUp"] = dsApplicationDetails.Tables[8];
    //                        if (intAccountCreationId == 0)
    //                        {
    //                            FunPriBindFollowupDLL(_Add);
    //                        }
    //                        else
    //                        {
    //                            /*ADDED BY NATARAJ Y for issues in follow up loading by 27/6/2011*/
    //                            if (dsApplicationDetails.Tables[8].Rows.Count > 0)
    //                            {
    //                                FunPriBindFollowupDLL(_Edit);
    //                            }
    //                            else
    //                            {
    //                                FunPriBindFollowupDLL(_Add);
    //                            }
    //                        }
    //                        gvFollowUp.DataSource = dsApplicationDetails.Tables[8];
    //                        gvFollowUp.DataBind();
    //                        FunPriGenerateNewFollowupRow();
    //                        ViewState["DtFollowUp"] = dsApplicationDetails.Tables[8];
    //                    }
    //                    #endregion

    //                    #region Moratorium Tab
    //                    if (Request.QueryString["qsAccConNo"] == null)
    //                    {
    //                        if (ddl_Time_Value.SelectedIndex > 0)
    //                        {
    //                            txtRepaymentTime.Text = ddl_Time_Value.SelectedItem.Text;
    //                        }
    //                    }
    //                    if (ddl_Time_Value.SelectedValue == "3" || ddl_Time_Value.SelectedValue == "4")
    //                    {
    //                        txtFBDate.Enabled = true;
    //                        rvtxtFBDate.Enabled = true;
    //                        rvtxtFBDate.Enabled = true;
    //                    }
    //                    else
    //                    {
    //                        txtFBDate.Enabled = false;
    //                        rvtxtFBDate.Enabled = false;
    //                        rvtxtFBDate.Enabled = false;
    //                        txtFBDate.Text = "";
    //                    }
    //                    if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
    //                    {

    //                        txtAdvanceInstallments.Attributes.Add("readonly", "readonly");
    //                    }
    //                    else
    //                    {

    //                        txtAdvanceInstallments.Attributes.Remove("readonly");
    //                    }

    //                    ViewState["dtMoratorium"] = dsApplicationDetails.Tables[13];
    //                    if (dsApplicationDetails.Tables[13].Rows.Count > 0)
    //                    {
    //                        if (intAccountCreationId == 0)
    //                        {
    //                            FunPriBindMoratoriumDLL(_Add);
    //                        }
    //                        else
    //                        {
    //                            FunPriBindMoratoriumDLL(_Edit);
    //                        }
    //                        gvMoratorium.DataSource = dsApplicationDetails.Tables[13];
    //                        gvMoratorium.DataBind();
    //                        ViewState["dtMoratorium"] = dsApplicationDetails.Tables[13];
    //                        FunPriGenerateNewMorotoriumRow();
    //                        ViewState["dtMoratorium"] = dsApplicationDetails.Tables[13];
    //                    }
    //                    else
    //                    {
    //                        FunPriBindMoratoriumDLL(_Add);
    //                    }
    //                    #endregion
    //                }
    //                else
    //                {
    //                    if (strSplitNum != null)
    //                    {
    //                        #region Moratorium Tab
    //                        if (Request.QueryString["qsAccConNo"] == null)
    //                        {
    //                            if (ddl_Time_Value.SelectedIndex > 0)
    //                            {
    //                                txtRepaymentTime.Text = ddl_Time_Value.SelectedItem.Text;
    //                            }
    //                        }
    //                        if (ddl_Time_Value.SelectedValue == "3" || ddl_Time_Value.SelectedValue == "4")
    //                        {
    //                            txtFBDate.Enabled = true;
    //                            rvtxtFBDate.Enabled = true;
    //                            rvtxtFBDate.Enabled = true;
    //                        }
    //                        else
    //                        {
    //                            txtFBDate.Enabled = false;
    //                            rvtxtFBDate.Enabled = false;
    //                            rvtxtFBDate.Enabled = false;
    //                            txtFBDate.Text = "";
    //                        }
    //                        if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
    //                        {

    //                            txtAdvanceInstallments.Attributes.Add("readonly", "readonly");
    //                        }
    //                        else
    //                        {

    //                            txtAdvanceInstallments.Attributes.Remove("readonly");
    //                        }

    //                        #endregion
    //                    }
    //                }

    //            }
    //            #region Asset Tab
    //            if (intAccountCreationId == 0)
    //            {
    //                if (strSplitNum == null && strConsNumber == null)//For Consolidation & Split
    //                {
    //                    string strBaseMLA = dsApplicationDetails.Tables[0].Rows[0]["MLA_Applicable"].ToString();
    //                    ViewState["IsBaseMLA"] = strBaseMLA;
    //                    if (strBaseMLA == "1" && (txtPrimeAccountNo.Text == "" || (txtPrimeAccountNo.Text != "" && txtStatus.Text.StartsWith("10"))))
    //                    {
    //                        txtFinanceAmount.ReadOnly = true;
    //                    }
    //                }
    //            }

    //            Session["ApplicationAssetDetails"] = dsApplicationDetails.Tables[10];
    //            gvAssetDetails.DataSource = dsApplicationDetails.Tables[10];
    //            gvAssetDetails.DataBind();
    //            System.Data.DataTable dtTable = (System.Data.DataTable)Session["ApplicationAssetDetails"];


    //            if (intAccountCreationId == 0)
    //            {
    //                if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL") && dsApplicationDetails.Tables[17].Rows.Count > 0)
    //                {
    //                    Session["ApplicationloanAssetDetails"] = dsApplicationDetails.Tables[17];
    //                    grvloanasset.DataSource = dsApplicationDetails.Tables[17];
    //                    grvloanasset.DataBind();
    //                    System.Data.DataTable dtTable1 = (System.Data.DataTable)Session["ApplicationloanAssetDetails"];
    //                }
    //            }
    //            else
    //            {
    //                if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL") && dsApplicationDetails.Tables[20].Rows.Count > 0)
    //                {
    //                    Session["ApplicationloanAssetDetails"] = dsApplicationDetails.Tables[20];
    //                    grvloanasset.DataSource = dsApplicationDetails.Tables[20];
    //                    grvloanasset.DataBind();
    //                    System.Data.DataTable dtTable1 = (System.Data.DataTable)Session["ApplicationloanAssetDetails"];
    //                }
    //            }
    //            #endregion

    //            #region Invoice
    //            if (dsApplicationDetails.Tables.Count > 15)
    //            {
    //                if (dsApplicationDetails.Tables[15].Rows.Count > 0 && dsApplicationDetails.Tables[15].Rows[0].ItemArray[0].ToString() != "")
    //                {
    //                    gvInvoiceDetails.DataSource = dsApplicationDetails.Tables[15];
    //                    ViewState["InvoiceDetails"] = dsApplicationDetails.Tables[15];
    //                    gvInvoiceDetails.DataBind();
    //                }
    //            }
    //            #endregion

    //            #region Repayment Structure
    //            if (dsApplicationDetails.Tables.Count > 16)
    //            {
    //                if (dsApplicationDetails.Tables[16].Rows.Count > 0 && dsApplicationDetails.Tables[16].Rows[0].ItemArray[0].ToString() != "")
    //                {
    //                    ViewState["RepaymentStructure"] = dsApplicationDetails.Tables[16];
    //                    grvRepayStructure.DataSource = dsApplicationDetails.Tables[16];
    //                    grvRepayStructure.DataBind();
    //                }

    //                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
    //                if (intAccountCreationId > 0)
    //                {
    //                    if (dsApplicationDetails.Tables[19].Rows.Count > 0)
    //                        ViewState["dtRepayDetailsOthers"] = dsApplicationDetails.Tables[19];
    //                }
    //                else
    //                {
    //                    if (dsApplicationDetails.Tables[18].Rows.Count > 0)
    //                        ViewState["dtRepayDetailsOthers"] = dsApplicationDetails.Tables[18];
    //                }
    //                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end
    //            }
    //            #endregion

    //            #region SubAccount User

    //            if (intAccountCreationId > 0)
    //            {
    //                /*tcAccountCreation.Tabs[7].Visible = !string.IsNullOrEmpty(txtSubAccountNo.Text);
    //                if (!string.IsNullOrEmpty(txtSubAccountNo.Text))
    //                {
    //                    if (dsApplicationDetails.Tables[18].Rows.Count > 0)
    //                    {
    //                        txtSLACustomerCode.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_Internal_Code_Ref"].ToString();
    //                        txtSLAUserName.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Name"].ToString();
    //                        txtComAddress1.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Address1"].ToString();
    //                        txtCOmAddress2.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Address2"].ToString();
    //                        txtComCity.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_City"].ToString();
    //                        txtComState.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_State"].ToString();
    //                        txtComCountry.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Country"].ToString();
    //                        txtComPincode.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Pincode"].ToString();
    //                        txtComPhone.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Phone"].ToString();
    //                        txtComEMail.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Email"].ToString();
    //                        txtComWebsite.Text = dsApplicationDetails.Tables[18].Rows[0]["SA_User_Website"].ToString();

    //                    }
    //                }*/
    //            }
    //            #endregion
    //            //if (txtPrimeAccountNo.Text == "" || txtStatus.Text.StartsWith("10") || txtStatus.Text.StartsWith("2"))
    //            //{
    //            //    tcAccountCreation.Tabs[7].Visible = false;
    //            //}
    //            //else if (txtPrimeAccountNo.Text != "" && intAccountCreationId == 0)
    //            //{
    //            //    tcAccountCreation.Tabs[7].Visible = true;
    //            //}

    //            //if (strMode == "C" && ddl_Return_Pattern.Visible && ddl_Return_Pattern.SelectedItem.Text.StartsWith("IRR"))
    //            //{
    //            //    FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedValue);
    //            //    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
    //            //    //strDocumentDate = txtApplicationDate.Text;
    //            //    GenerateRepaymentSchedule(objRepaymentStructure,Utility.StringToDate(txtApplicationDate.Text));
    //            //}
    //            if (intAccountCreationId == 0 && txtAccountDate.Text != DateTime.Now.Date.ToString(strDateFormat))
    //            {
    //                txtBusinessIRR_Repay.Text = "";
    //            }

    //        }
    //        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") ||
    //            ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") ||
    //            ((ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") ||
    //            ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TL")) &&
    //            ddl_Repayment_Mode.SelectedItem.Text.ToUpper().StartsWith("PRO")))
    //        {
    //            tcAccountCreation.Tabs[2].Enabled = false;
    //        }
    //        else
    //            tcAccountCreation.Tabs[2].Enabled = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        if (intAccountCreationId == 0)
    //        {
    //            throw new ApplicationException("Due to Data Problem, Unable to Load Application Details");
    //        }
    //        else
    //        {
    //            throw new ApplicationException("Due to Data Problem, Unable to Load Account Details");
    //        }
    //    }
    //    //Code Added By Ganapathy on 08/06/2012 BEGIN
    //    finally
    //    {
    //        ObjCustomerService.Close();
    //    }
    //    //END
    //}





    /*
        /// <summary>
        /// To Get search value to display value after sorting or paging changed
        /// </summary>



        /// <summary>
        /// To clear value after show all is clicked
        /// </summary>
        private void FunPriClearSearchValue()
        {
            if (grvInvoices.HeaderRow != null)
            {
                ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderVendor_Name")).Text = "";
                ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderAssetCategory")).Text = "";
                ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderInvoice_Type")).Text = "";
                ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderInvoice_No")).Text = "";
                //((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
            }
        }
        /// <summary>
        /// Tos et search value after sorting or paging changed
        /// </summary>
        private void FunPriSetSearchValue()
        {
            if (grvInvoices.HeaderRow != null)
            {
                ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderVendor_Name")).Text = strSearchVal1;
                ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderAssetCategory")).Text = strSearchVal2;
                ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderInvoice_Type")).Text = strSearchVal3;
                ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderInvoice_No")).Text = strSearchVal4;
                //((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
            }
        }

        private void FunPriGetSearchValue()
        {
            if (grvInvoices.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderVendor_Name")).Text.Trim();
                strSearchVal2 = ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderAssetCategory")).Text.Trim();
                strSearchVal3 = ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderInvoice_Type")).Text.Trim();
                strSearchVal4 = ((TextBox)grvInvoices.HeaderRow.FindControl("txtHeaderInvoice_No")).Text.Trim();
                //strSearchVal5 = ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
            }
        }


        /// <summary>
        /// To Search in Grid view Gets the text box as sender and gets its text
        /// </summary>
        /// <param name="sender">Text box in gridview</param>
        /// <param name="e"></param>

        protected void FunProHeaderSearch(object sender, EventArgs e)
        {

            string strSearchVal = string.Empty;
            TextBox txtboxSearch;
            try
            {
                txtboxSearch = ((TextBox)sender);
                FunPriGetSearchValue();
                //Replace the corresponding fields needs to search in sqlserver
                if (strSearchVal1 != "")
                {
                    strSearchVal += "Vendor_Name like '%" + strSearchVal1 + "%'";
                }
                if (strSearchVal2 != "")
                {
                    strSearchVal += " and AssetCategory like '%" + strSearchVal2 + "%'";
                }
                if (strSearchVal3 != "")
                {
                    strSearchVal += " and Invoice_Type like '%" + strSearchVal3 + "%'";
                }
                if (strSearchVal4 != "")
                {
                    strSearchVal += " and Invoice_No like '%" + strSearchVal4 + "%'";
                }
                //if (strSearchVal5 != "")
                //{
                //    strSearchVal += " and LOB_Name like '" + strSearchVal5 + "%'";
                //}

                if (strSearchVal.StartsWith(" and "))
                {
                    strSearchVal = strSearchVal.Remove(0, 5);
                }
                hdnSearch.Value = strSearchVal;
                //FunPriBindGrid();
                FunPriSetSearchValue();
                if (txtboxSearch.Text != "")
                    ((TextBox)grvInvoices.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            }
        }

        /// <summary>
        /// Gets the Sort Direction of the strColumn in the Grid View Using hidden control
        /// </summary>
        /// <param name="strColumn"> Colunm Name is Passed</param>
        /// <returns>Sort Direction as a String </returns>

        private string FunPriGetSortDirectionStr(string strColumn)
        {
            string strSortDirection = string.Empty;
            string strSortExpression = string.Empty;
            // By default, set the sort direction to ascending.
            string strOrderBy = string.Empty;
            strSortDirection = "DESC";
            try
            {
                // Retrieve the last strColumn that was sorted.
                // Check if the same strColumn is being sorted.
                // Otherwise, the default value can be returned.
                strSortExpression = hdnSortExpression.Value;
                if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
                {
                    strSortDirection = "ASC";
                }
                // Save new values in hidden control.
                hdnSortDirection.Value = strSortDirection;
                hdnSortExpression.Value = strColumn;
                strOrderBy = " " + strColumn + " " + strSortDirection;
                hdnOrderBy.Value = strOrderBy;
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            }
            return strSortDirection;
        }

        /// <summary>
        /// Will Perform Sorting On Colunm base upon the link button id calling the function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected void FunProSortingColumn(object sender, EventArgs e)
        {
            var imgbtnSearch = string.Empty;
            try
            {
                LinkButton lnkbtnSearch = (LinkButton)sender;
                string strSortColName = string.Empty;
                //To identify image button which needs to get chnanged
                imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");
                switch (lnkbtnSearch.ID)
                {
                    case "lnkbtnVendor_Name":
                        strSortColName = "Vendor_Name";
                        break;
                    case "lnkbtnAssetCategory":
                        strSortColName = "AssetCategory";
                        break;
                    case "lnkbtnInvoice_Type":
                        strSortColName = "Invoice_Type";
                        break;
                    case "lnkbtnInvoice_No":
                        strSortColName = "Invoice_No";
                        break;
                }

                string strDirection = FunPriGetSortDirectionStr(strSortColName);
                //FunPriBindGrid();

                if (strDirection == "ASC")
                {
                    ((ImageButton)grvInvoices.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
                }
                else
                {

                    ((ImageButton)grvInvoices.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
                }
            }
            catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
            {
                //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
                cv_TabMainPage.ErrorMessage = objFaultExp.Detail.ProReasonRW;
                cv_TabMainPage.IsValid = false;
            }
            catch (Exception ex)
            {
                //lblErrorMessage.InnerText = ex.Message;
                cv_TabMainPage.ErrorMessage = ex.Message;
                cv_TabMainPage.IsValid = false;
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            }
        }
        */
    #endregion

    #endregion

    #region"Rental Details"

    /// <summary>
    /// This event is used call at the time of primary grid insert in grvPrimaryGrid 
    /// using user defined funcition FunPriAddgrvPrimaryGridDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvRentalDetails_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                FunPriAddgrvRentalDetails();
            }
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable To add Due to Data Problem";
            cv_TabMainPage.IsValid = false;
        }
    }

    /// <summary>
    /// This event is used call at the time of primary grid delete in grvPrimaryGrid 
    /// using user defined funcition FunPriRemovegrvPrimaryGridDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvRentalDetails_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemovegrvRentalDetails(e.RowIndex);

        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable To delete Due to Data Problem";
            cv_TabMainPage.IsValid = false;
        }
    }

    private void FunPriEmptyRentalDetails()
    {
        try
        {
            dtRentalDetails = new System.Data.DataTable();


            dtRentalDetails.Columns.Add("PA_SA_REF_ID");
            dtRentalDetails.Columns.Add("PANum");
            dtRentalDetails.Columns.Add("Tranche");
            DataRow drRentalDetails = dtRentalDetails.NewRow();
            dtRentalDetails.Rows.Add(drRentalDetails);

            ViewState["dtRentalDetails"] = dtRentalDetails;
            grvRentalDetails.DataSource = dtRentalDetails;
            grvRentalDetails.DataBind();
            grvRentalDetails.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    ///This user defined function is used to add primary grid details into the grid.
    /// </summary>
    private void FunPriAddgrvRentalDetails()
    {
        try
        {
            UserControls_S3GAutoSuggest ddlPANum = (UserControls_S3GAutoSuggest)grvRentalDetails.FooterRow.FindControl("ddlPANum");

            DataRow drRow;
            dtRentalDetails = (System.Data.DataTable)ViewState["dtRentalDetails"];
            if (dtRentalDetails.Rows.Count > 0)
            {
                if (dtRentalDetails.Rows[0]["PA_SA_REF_ID"].ToString() == "")
                {
                    dtRentalDetails.Rows[0].Delete();
                }
            }

            if (ddlProposalType.SelectedValue == "3")//Extension
            {
                if (dtRentalDetails.Rows.Count > 0)
                {
                    Utility.FunShowAlertMsg(this, "Only one Rental Schedule Number should be selected");
                    ddlPANum.Focus();
                    return;
                }
            }

            dtPrimaryGrid = (System.Data.DataTable)ViewState["dtPrimaryGrid"];
            if (dtPrimaryGrid.Rows.Count > 0)
            {
                DataRow[] dr = dtPrimaryGrid.Select("Selected=1");
                if (dr.Length == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atleast one asset category.");
                    return;
                }
            }

            //Duplicate validation for equated

            if (dtRentalDetails.Rows.Count > 0)
            {
                DataRow[] drPrimaryGrid1 = null;
                drPrimaryGrid1 = dtRentalDetails.Select("PA_SA_REF_ID = " + ddlPANum.SelectedValue + "");
                if (drPrimaryGrid1.Count() > 0)
                {
                    Utility.FunShowAlertMsg(this, "Rental Schedule Number already added");
                    ddlPANum.Focus();
                    return;
                }
            }

            //Validate Given RS no asset category and selected asset category matched or not
            string strXML = string.Empty;
            strXML = FunPriGetAssetCatXML();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@PA_SA_REF_ID", ddlPANum.SelectedValue);
            Procparam.Add("@XMLAssetCategory", strXML);
            if (ViewState["intsales_type"] != null)
                Procparam.Add("@RS_Type", ViewState["intsales_type"].ToString());
            
            DataSet ds = Utility.GetDataset("S3G_LOANAD_VALIDATE_MapInvoices_ACC", Procparam);

            DataTable dt = ds.Tables[0];

            int intErrorcode = 0;

            if (dt.Rows.Count > 0)
            {
                intErrorcode = Convert.ToInt32(dt.Rows[0]["ErrorCode"].ToString());
                if (intErrorcode == 1)
                {
                    Utility.FunShowAlertMsg(this, "Assset Category Mismatch.");
                    return;
                }
                if (intErrorcode == 2)
                {
                    Utility.FunShowAlertMsg(this, "Sales Tax Type Mismatch.");
                    return;
                }
            }

            drRow = dtRentalDetails.NewRow();
            drRow["PA_SA_REF_ID"] = ddlPANum.SelectedValue;
            drRow["PANum"] = ddlPANum.SelectedText;
            drRow["Tranche"] = ds.Tables[1].Rows[0]["Tranche_Name"].ToString();
            dtRentalDetails.Rows.Add(drRow);
            ViewState["dtRentalDetails"] = dtRentalDetails;

            // Code Added for Call Id : 4154 CR_56
            if (chkIsSecondary.Checked)
                FunPriRV_RW_Acc();

            FunFillgrid(grvRentalDetails, dtRentalDetails);
            FunPriMapRentalInvoices();
            FunPriGetMappedAmount();
            txtBusinessIRR_Repay.Text = string.Empty;
            //ViewState["Balance_Principal"] = ds.Tables[1].Rows[0]["Balance_Principal"].ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    // Code Added for Call Id : 4154 CR_56 Strart

    private void FunPriRV_RW_Acc()
    {
        try
        {
            string strXML = string.Empty;
            strXML = FunPriGetAssetCatXML();

            dtRentalDetails = (System.Data.DataTable)ViewState["dtRentalDetails"];

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Pricing_ID", ddlApplicationReferenceNo.SelectedValue);
            Procparam.Add("@RentalDetails", dtRentalDetails.FunPubFormXml());
            Procparam.Add("@XMLAssetCategory", strXML);
            DataTable dtRVRW = Utility.GetDefaultData("S3G_LAD_Get_RV_RW_Acc", Procparam);

            if (ViewState["dtPrimaryGrid"] != null)
            {
                DataTable dtRV = ((DataTable)ViewState["dtPrimaryGrid"]);
                DataRow[] drRVRWP = dtRVRW.Select("Offer_Type=1");

                for (int pri = 0; pri < drRVRWP.Length; pri++)
                {
                    DataRow[] drRV = dtRV.Select("AssetCategory_ID = " + drRVRWP[pri]["AssetCategory_ID"].ToString() + " and Offer_Type=1");
                    foreach (DataRow dr in drRV)
                    {
                        dr["ResidualPer"] = Convert.ToDecimal(drRVRWP[pri]["ResidualPer"].ToString());
                    }
                    dtRV.AcceptChanges();
                }

                DataRow[] drRVRWS = dtRVRW.Select("Offer_Type=2");

                for (int pri = 0; pri < drRVRWS.Length; pri++)
                {
                    DataRow[] drRV = dtRV.Select("AssetCategory_ID = " + drRVRWS[pri]["AssetCategory_ID"].ToString() + " and Offer_Type=2");
                    foreach (DataRow dr in drRV)
                    {
                        dr["ResidualPer"] = Convert.ToDecimal(drRVRWS[pri]["ResidualPer"].ToString());
                    }
                    dtRV.AcceptChanges();
                }

                ViewState["dtPrimaryGrid"] = dtRV;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    // Call Id : 4154 CR_56 End

    /// <summary>
    ////This user defined function is used to remove primary grid details into the grid.
    /// </summary>
    private void FunPriRemovegrvRentalDetails(int intRowIndex)
    {
        try
        {
            dtRentalDetails = (System.Data.DataTable)ViewState["dtRentalDetails"];
            dtRentalDetails.Rows.RemoveAt(intRowIndex);
            if (dtRentalDetails.Rows.Count == 0)
            {
                FunPriEmptyRentalDetails();
            }
            else
            {

                FunFillgrid(grvRentalDetails, dtRentalDetails);
                ViewState["dtRentalDetails"] = dtRentalDetails;

            }
            FunPriMapRentalInvoices();
            FunPriGetMappedAmount();
            txtAddInvAmt.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }


    private void FunPriMapRentalInvoices()
    {
        try
        {
            string strErrorCode = "0";
            string strPANumber = string.Empty;
            dtRentalDetails = (System.Data.DataTable)ViewState["dtRentalDetails"];
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                strPANumber = txtPrimeAccountNo.Text;
            else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                strPANumber = ddlApplicationReferenceNo.SelectedText;
            Procparam.Add("@PANum", strPANumber);
            Procparam.Add("@Type", ddlProposalType.SelectedValue);
            string strPurTax = string.Empty;
            if (chkLBT.Checked)
                strPurTax += "+ISNULL(LBT_Amount,0)";
            if (chkPT.Checked)
                strPurTax += "+ISNULL(Purchase_Tax,0)";
            if (chkET.Checked)
                strPurTax += "+ISNULL(Entry_Tax_Amount,0)";
            if (chkRC.Checked)
                strPurTax += "+ISNULL(Reverse_Charge_Tax,0)";
            if (chkbxGSTRC.Checked)
                strPurTax += "+ISNULL(RC_SGST_Amt,0) + ISNULL(RC_CGST_Amt,0) + ISNULL(RC_IGST_Amt,0) + ISNULL(RC_SAC_SGST_Amt,0) + ISNULL(RC_SAC_CGST_Amt,0) + ISNULL(RC_SAC_IGST_Amt,0)";

            if (!string.IsNullOrEmpty(strPurTax))
            {
                if (strPurTax.Contains("+"))
                    strPurTax = strPurTax.Substring(1, strPurTax.Length - 1);
                Procparam.Add("@PurTax", strPurTax);
            }
            Procparam.Add("@User_Id", intUserId.ToString());
            Procparam.Add("@XMLPANum", dtRentalDetails.FunPubFormXml(true));
            //Added by Sathiyanathan on 24Jun2015 for OPC starts here
            Procparam.Add("@Mode", Convert.ToString(strMode));
            Procparam.Add("@SessionID", Convert.ToString(Request.Cookies["CookSession_ID"].Value));
            //Added by Sathiyanathan on 24Jun2015 for OPC ends here
            Procparam.Add("@New_PANum", txtPrimeAccountNo.Text);
            strErrorCode = Utility.GetTableScalarValue("S3G_LOANAD_MoveExistingInvoices_ACC", Procparam);
            if (strErrorCode == "0")
            {
                FunPriBindGridSummary();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }


    [System.Web.Services.WebMethod]
    public static string[] GetPANumList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Type", "1");
        //Procparam.Add("@Customer_Id", obj_PageValue.ViewState["Customer_Id"].ToString());
        if (System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] != null)
        {
            if (System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"].ToString() != "0")
                Procparam.Add("@Customer_Id", System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"].ToString());
        }
        Procparam.Add("@Prefix", prefixText);
        //CODE ADDED FOR THE CALL 4186
        Procparam.Add("@AccountCreation_ID", obj_PageValue.intAccountCreationId.ToString());
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GET_RSNO_ACC", Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLienAccount(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Type", "2");
        Procparam.Add("@Prefix", prefixText);
     
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GET_RSNO_ACC", Procparam));

        return suggestions.ToArray();
    }

    protected void btnPrint_click1(object sender, EventArgs e)
    {
        try
        {
            ReportAccountsMgtServicesReference.ReportAccountsMgtServicesClient objSerClient = new ReportAccountsMgtServicesReference.ReportAccountsMgtServicesClient();
            //Repay Details
            //byte[] byterepayDetails = objSerClient.FunPubGetRepayDetails(intCompanyId, txtPrimeAccountNo.Text, txtSubAccountNo.Text, "R");
            byte[] byterepayDetails = objSerClient.FunPubGetRepayDetails(intCompanyId, txtPrimeAccountNo.Text, "", "R");

            List<ClsPubRepayDetails> repayDetails = (List<ClsPubRepayDetails>)DeSeriliaze(byterepayDetails);
            //Asset Details
            //byte[] byteassetDetails = objSerClient.FunPubGetAssestDetails(intCompanyId, txtPrimeAccountNo.Text, txtSubAccountNo.Text);
            byte[] byteassetDetails = objSerClient.FunPubGetAssestDetails(intCompanyId, txtPrimeAccountNo.Text, "");
            List<ClsPubAssestDetails> assetDetails = (List<ClsPubAssestDetails>)DeSeriliaze(byteassetDetails);
            //Header Details
            ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
            objHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            // objHeader.Branch = ddlBranchList.SelectedItem.Text;
            objHeader.Branch = ddlBranchList.SelectedText;
            objHeader.PANum = txtPrimeAccountNo.Text;
            //if (txtSubAccountNo.Text != string.Empty)
            //    objHeader.SANum = txtSubAccountNo.Text;
            //else
            objHeader.SANum = "--";
            if (txtProductCode.Text.Contains("-"))
                objHeader.Product = txtProductCode.Text.Split('-')[1].ToString();
            else
                objHeader.Product = txtProductCode.Text;

            //Customer Details
            ClsPubCustomer ObjCustomer = new ClsPubCustomer();
            ObjCustomer.Address = S3GCustomerAddress1.CustomerName + " (" + S3GCustomerAddress1.CustomerCode + ")\n" + S3GCustomerAddress1.CustomerAddress;

            Session["AccountingCurrency"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            Session["Header"] = objHeader;
            Session["CustomerInfo"] = ObjCustomer;
            Session["FinAmt"] = txtFinanceAmount.Text;
            //Session["Terms"] = txtTenure.Text + "/" + ddlTenureType.SelectedItem.Text + "/" + ddl_Time_Value.SelectedItem.Text + "/" + txtRate.Text + "(" + ddl_Return_Pattern.SelectedItem.Text + ")/" + ddlRepaymentMode.SelectedItem.Text;
            Session["Terms"] = txtTenure.Text + "/" + ddlTenureType.SelectedItem.Text + "/" + hdnTimeValue.Value + "/" + "" + "(" + ddlReturnPattern.SelectedItem.Text + ")/" + "";
            Session["IRR"] = txtBusinessIRR.Text;
            Session["Repay"] = repayDetails;
            Session["Asset"] = assetDetails;
            Session["IsAssetPrintOff"] = "0";
            Session["Heading"] = "Repayment Structure";
            Session["Type"] = "R";

            string strScipt = "window.open('../Reports/S3GRepaymentScheduleReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Repay", strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Print the Repayment Details";
            cv_TabMainPage.IsValid = false;
        }


    }

    protected void btnPrint_click(object sender, EventArgs e)
    {
        try
        {

            if (ddlReportType.SelectedIndex > 0)
            {

                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                Procparam.Add("@AccountCreation_Id", intAccountCreationId.ToString());
                Procparam.Add("@CustomerID", ViewState["Customer_Id"].ToString());
                Procparam.Add("@TypeID", ddlReportType.SelectedValue);
                if (chkSecondary.Checked)
                    Procparam.Add("@Sec", "1");
                //Procparam.Add("@Acc_XMl", dtTranche.FunPubFormXml());
                //Procparam.Add("@customer_id", ddlcust.SelectedValue);
                //Procparam.Add("@funder_id", ddlfund.SelectedValue);
                // Procparam.Add("@Note_id", strNote_id.ToString());

                //START By Siva.K on 16APR2015 Validation for No records in Primary/Secondary
                if (ddlReportType.SelectedValue == "1" || ddlReportType.SelectedValue == "2" || ddlReportType.SelectedValue == "3")
                {
                    System.Data.DataSet dsprint = Utility.GetDataset("S3G_LOANAD_GET_RPT_RSDtls", Procparam);
                    if (dsprint.Tables[1].Rows.Count == 0)
                    {
                        Utility.FunShowAlertMsg(this, "No Records Found!.");
                        return;
                    }
                }
                //END By Siva.K on 16APR2015 Validation for No records in Primary/Secondary

                Session["ProcParam"] = Procparam;
                Session["Format_Type"] = ddlReportType.SelectedValue;//Format_Type
                string strScipt = "window.open('../LoanAdmin/S3G_RPT_RS_ReportPage.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Note Creation", strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Select Report Format");
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Print the Repayment Details";
            cv_TabMainPage.IsValid = false;
        }


    }


    /// <summary>
    /// This method is used to load the customer address based on the customer control customer id
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPriGetCustomerAddress(int CustomerID)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CustomerID", CustomerID.ToString());
            Procparam.Add("@CompanyID", intCompanyId.ToString());
            System.Data.DataTable dtCustomer = Utility.GetDefaultData("S3G_Get_Exist_Customer_Details_Enquiry_Updation", Procparam);
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Text = txtCustomerCode.Text = dtCustomer.Rows[0]["Customer_Code"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(dtCustomer.Rows[0]["Customer_Code"].ToString(),
                    dtCustomer.Rows[0]["comm_Address1"].ToString() + "\n" +
             dtCustomer.Rows[0]["comm_Address2"].ToString() + "\n" +
            dtCustomer.Rows[0]["comm_city"].ToString() + "\n" +
            dtCustomer.Rows[0]["comm_state"].ToString() + "\n" +
            dtCustomer.Rows[0]["comm_country"].ToString() + "\n" +
            dtCustomer.Rows[0]["comm_pincode"].ToString(), dtCustomer.Rows[0]["Customer_Name"].ToString(), dtCustomer.Rows[0]["Comm_Telephone"].ToString(),
            dtCustomer.Rows[0]["Comm_mobile"].ToString(),
            dtCustomer.Rows[0]["comm_email"].ToString(), dtCustomer.Rows[0]["comm_website"].ToString());

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// this event is used to load customer address using user defined function FunPriGetCustomerAddress by passing customer Id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            ddlApplicationReferenceNo.Clear();
            FunPriClearPRoposalDtls();
            HiddenField hdnCustomerId1 = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId1 != null && Convert.ToString(hdnCustomerId1.Value) != "")
            {
                if (hdnCustomerId1.Value != "")
                {
                    ViewState["Customer_Id"] = hdnCustomerId.Value = hdnCustomerId1.Value;
                    FunPriGetCustomerAddress(Convert.ToInt32(hdnCustomerId1.Value));
                    //*********START************** By Siva.K For Data Load Problem in Multi User ***********//
                    if (ViewState["Customer_Id"] != null)
                        System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = Convert.ToString(ViewState["Customer_Id"]);
                    else
                        System.Web.HttpContext.Current.Session["AutoSuggestCustomerID"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + "Unable to get customer details";
            cv_TabMainPage.IsValid = false;
        }
    }


    protected void btnLoadRS_OnClick(object sender, EventArgs e)
    {
        try
        {
            //ddlApplicationReferenceNo.Clear();
            // FunPriClearPRoposalDtls();
            HiddenField hdnRsNo = (HiddenField)UcCtlRsno.FindControl("hdnID");
            if (hdnRsNo != null && Convert.ToString(hdnRsNo.Value) != "")
            {
                if (hdnRsNo.Value != "")
                {
                    ViewState["RSNO"] = hdnRsNo.Value;
                    TextBox txtName = (TextBox)UcCtlRsno.FindControl("txtName");
                    FunPriCopyAccountDetails(txtName.Text);
                    //FunPriGetCustomerAddress(Convert.ToInt32(hdnRsNo.Value));
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = strErrorMessagePrefix + "Unable to get RS details";
            cv_TabMainPage.IsValid = false;
        }
    }




    #endregion


    #endregion

    protected void chkcstwith_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkcstwith.Checked)
            {
                //chkcstwithout.Checked = false;
                txtCFormNo.Enabled = true;
                ViewState["intsales_type"] = "6";
                chkSEZ.Checked = false;
                ddlSEZZone.SelectedIndex = 0;
                ddlSEZZone.Enabled = false;
                chkimport.Checked = false;
                chkSEZA1.Checked = false;
                chkSEZA1.Enabled = false;

            }
            else
            {
                txtCFormNo.Text = "";
                txtCFormNo.Enabled = false;
                ViewState["intsales_type"] = "2";
            }

            string strParent_Location_ID = "";

            if (ViewState["Parent_Location_ID"] != null && ddlBillingState.SelectedValue != "0")
            {
                strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();

                if (strParent_Location_ID == ddlBillingState.SelectedValue)
                {
                    ddlSalesTax.Text = "SGST&CGST";
                    ViewState["intsales_type"] = "13";
                    chkcstwith.Enabled = false;
                    chkcstwith.Checked = false;

                    chkCSTDeal.Checked = false;
                    chkCSTDeal.Enabled = true;
                    ViewState["vatleasing"] = 0;

                }
                else
                {
                    ddlSalesTax.Text = "IGST";

                    ViewState["intsales_type"] = "15";
                    chkcstwith.Enabled = true;
                    chkcstwith.Checked = false;
                    chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select Invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void chkIsSecondary_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadRV_SeparateSecAmort();
            FunPriIsSecondaryChanges();
        }
        catch
        {

        }
    }

    protected void chkCSTDeal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["vatleasing"] = "0";
            ViewState["cstleasing"] = "0";
            string strParent_Location_ID = "";
            if (chkCSTDeal.Checked)
            {
                if (ViewState["Parent_Location_ID"] != null)
                {
                    strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();
                    //uncommented by Gomathi for VAT Leasing start
                    if (strParent_Location_ID == ddlDeliveryState.SelectedValue)
                    {
                        ddlSalesTax.Text = "CST(CST Leasing)";
                        ViewState["intsales_type"] = "2";
                        chkcstwith.Enabled = true;
                        ViewState["cstleasing"] = "1";
                    }
                    else
                    {
                        ddlSalesTax.Text = "CST";
                        ViewState["intsales_type"] = "2";
                        chkcstwith.Enabled = true;
                        ViewState["cstleasing"] = "0";

                    }
                }
            }

            else
            {
                if (ViewState["Parent_Location_ID"] != null)
                {
                    strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();
                    //uncommented by Gomathi for VAT Leasing start
                    if (strParent_Location_ID == ddlDeliveryState.SelectedValue)
                    {
                        ddlSalesTax.Text = "VAT";
                        ViewState["intsales_type"] = "1";
                        chkcstwith.Enabled = false;
                        chkcstwith.Checked = false;
                        ViewState["vatleasing"] = "0";
                    }
                    else
                    {
                        ddlSalesTax.Text = "VAT(VAT Leasing)";
                        ViewState["intsales_type"] = "1";
                        chkcstwith.Enabled = false;
                        chkcstwith.Checked = false;
                        ViewState["vatleasing"] = "1";
                    }
                }
            }

            if (ViewState["Parent_Location_ID"] != null && ddlBillingState.SelectedValue != "0")
            {
                strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();

                if (strParent_Location_ID == ddlBillingState.SelectedValue)
                {
                    ddlSalesTax.Text = "SGST&CGST";
                    ViewState["intsales_type"] = "13";
                    chkcstwith.Enabled = false;
                    chkcstwith.Checked = false;

                    chkCSTDeal.Checked = false;
                    chkCSTDeal.Enabled = true;
                    ViewState["vatleasing"] = 0;

                }
                else
                {
                    ddlSalesTax.Text = "IGST";

                    ViewState["intsales_type"] = "15";
                    chkcstwith.Enabled = true;
                    chkcstwith.Checked = false;
                    chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                }
            }

            FunPriEmptyRentalDetails();
            ObjPagingSummary.ProSearchValue = "0";
            FunPriBindGridSummary();
            //else
            //{
            //    ddlDeliveryState_SelectedIndexChange(null, null);
            //}

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select Invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void chkimport_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strParent_Location_ID = "";

            if (chkimport.Checked)
            {
                chkSEZ.Checked = false;
                ddlSEZZone.SelectedIndex = 0;
                ddlSEZZone.Enabled = false;
                ddlSalesTax.Text = "None";
                ViewState["intsales_type"] = "3";
                chkcstwith.Checked = chkCSTDeal.Checked = false;
                chkSEZA1.Enabled = true;
                chkcstwith.Enabled = chkCSTDeal.Enabled = false;
                txtCFormNo.Text = "";
                txtCFormNo.Enabled = false;
            }
            else
            {
                if (ViewState["Parent_Location_ID"] != null)
                {
                    chkSEZA1.Checked = false;
                    chkSEZA1.Enabled = false;
                    strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();
                    //if (strParent_Location_ID == ddlDeliveryState.SelectedValue)
                    //{
                    //    ddlSalesTax.Text = "VAT";
                    //    //ddlSalesTax.Enabled = false;
                    //    ViewState["intsales_type"] = "1";
                    //    chkcstwith.Enabled = false;
                    //    chkCSTDeal.Enabled = true;
                    //}
                    //else
                    //{
                    //    ddlSalesTax.Text = "CST";
                    //    //ddlSalesTax.Enabled = false;
                    //    ViewState["intsales_type"] = "2";
                    //    chkcstwith.Enabled = true;
                    //    chkCSTDeal.Enabled = false;
                    //}
                    ddlSalesTax.Text = "VAT";
                    //ddlSalesTax.Enabled = false;
                    ViewState["intsales_type"] = "1";
                    chkcstwith.Enabled = false;
                    chkCSTDeal.Enabled = true;
                }

            }

            if (ViewState["Parent_Location_ID"] != null && ddlBillingState.SelectedValue != "0")
            {
                strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();

                if (strParent_Location_ID == ddlBillingState.SelectedValue)
                {
                    ddlSalesTax.Text = "SGST&CGST";
                    ViewState["intsales_type"] = "13";
                    chkcstwith.Enabled = false;
                    chkcstwith.Checked = false;

                    chkCSTDeal.Checked = false;
                    chkCSTDeal.Enabled = true;
                    ViewState["vatleasing"] = 0;

                }
                else
                {
                    ddlSalesTax.Text = "IGST";

                    ViewState["intsales_type"] = "15";
                    chkcstwith.Enabled = true;
                    chkcstwith.Checked = false;
                    chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select Invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void chkLBT_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGridSummary();
            FunPriGetMappedAmount();
            FunprisumgrvPrimaryGrid();
            FunprisumgrvSecondaryGrid();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select Invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void chkbxGSTRC_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGridSummary();
            FunPriGetMappedAmount();
            FunprisumgrvPrimaryGrid();
            FunprisumgrvSecondaryGrid();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select Invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void ddlDeliveryState_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            ViewState["vatleasing"] = 0;
            ViewState["cstleasing"] = 0;
            //opc195 start
            if (ddlBranchList.SelectedText != "" && (ddlBranchList.SelectedText != ddlDeliveryState.SelectedItem.Text)
                && (ddlDeliveryState.SelectedItem.Text != "All"))
            {
                DataSet dsCheck = new DataSet();
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Pricing_ID", ddlApplicationReferenceNo.SelectedValue);
                Procparam.Add("@Delivery_Loc_ID", ddlDeliveryState.SelectedValue);
                dsCheck = Utility.GetDataset("S3G_GET_Check_Delivery_State_GST", Procparam);
                if (dsCheck.Tables[0].Rows[0]["Is_GST"].ToString() == "1")
                {
                    Utility.FunShowAlertMsg(this, "RS state can not differ from Delivery State");
                    ddlDeliveryState.SelectedValue = "0";
                    return;
                }
            }
            //opc0195 end

            if (ViewState["Parent_Location_ID"] != null)
            {
                string strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();
                //uncommented by Gomathi for VAT Leasing start
                if (strParent_Location_ID == ddlDeliveryState.SelectedValue)
                {
                    ddlSalesTax.Text = "VAT";
                    //ddlSalesTax.Enabled = false;
                    ViewState["intsales_type"] = "1";
                    chkcstwith.Enabled = false;
                    chkcstwith.Checked = false;

                    chkCSTDeal.Checked = false;
                    chkCSTDeal.Enabled = true;
                    ViewState["vatleasing"] = 0;

                }
                else
                {
                    ddlSalesTax.Text = "CST";
                    //ddlSalesTax.Enabled = false;
                    ViewState["intsales_type"] = "2";
                    chkcstwith.Enabled = true;
                    chkcstwith.Checked = false;
                    chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                }
                //uncommented by Gomathi for VAT Leasing end
                // Added for Call Id :5093
                if (string.IsNullOrEmpty(strSplitNum))
                {
                    FunPriEmptyRentalDetails();
                    ObjPagingSummary.ProSearchValue = "0";
                    FunPriBindGridSummary();
                }
                // End Call Id :5093
                //commented by Gomathi for VAT Leasing start
                //ddlSalesTax.Text = "VAT";
                ////ddlSalesTax.Enabled = false;
                //ViewState["intsales_type"] = "1";
                //chkcstwith.Enabled = false;
                //chkCSTDeal.Enabled = true;
                //commented by Gomathi for VAT Leasing end

                if (ddlBillingState.SelectedValue != "0")
                    ddlBillingState_SelectedIndexChange(null, null);
            }

            if (ddlBillingState.SelectedItem.Text != ddlDeliveryState.SelectedItem.Text)
            {
                hdnGST.Value = "1";
                //strAlert = "Delivery State and Billing States are different";
                //strAlert += @"\n\nDo you want to proceed?";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "if(confirm('" + strAlert + "')) { document.getElementById('" + btnTemp.ClientID + "').click(); } else {}", true);
            }
            else
            {
                hdnGST.Value = "0";
            }

            
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to get delivery address";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void ddlBillingState_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            ViewState["vatleasing"] = 0;
            ViewState["cstleasing"] = 0;

            if (ViewState["Parent_Location_ID"] != null)
            {
                string strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();

                if (strParent_Location_ID == ddlBillingState.SelectedValue)
                {
                    ddlSalesTax.Text = "SGST&CGST";
                    ViewState["intsales_type"] = "13";
                    chkcstwith.Enabled = false;
                    chkcstwith.Checked = false;

                    chkCSTDeal.Checked = false;
                    chkCSTDeal.Enabled = true;
                    ViewState["vatleasing"] = 0;

                }
                else
                {
                    ddlSalesTax.Text = "IGST";
                    
                    ViewState["intsales_type"] = "15";
                    chkcstwith.Enabled = true;
                    chkcstwith.Checked = false;
                    chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                }
                //Changed By Chandru for Daman Diu and Dadra
                if ((strParent_Location_ID == "189" || strParent_Location_ID == "190") && 
                    (ddlBillingState.SelectedValue == "189" || ddlBillingState.SelectedValue == "190"))
                {
                    ddlSalesTax.Text = "SGST&CGST";
                    ViewState["intsales_type"] = "13";
                    chkcstwith.Enabled = false;
                    chkcstwith.Checked = false;

                    chkCSTDeal.Checked = false;
                    chkCSTDeal.Enabled = true;
                    ViewState["vatleasing"] = 0;
                }
            }
            if (ddlBillingState.SelectedItem.Text != ddlDeliveryState.SelectedItem.Text)
            {
                hdnGST.Value = "1";
                //strAlert = "Delivery State and Billing States are different";
                //strAlert += @"\n\nDo you want to proceed?";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "if(confirm('" + strAlert + "')) { document.getElementById('" + btnTemp.ClientID + "').click(); } else {}", true);
            }
            else
            {
                hdnGST.Value = "0";
            }
        }
        catch (Exception objException)
        {
            cv_TabMainPage.ErrorMessage = "Unable to get Billing address";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void chkSEZ_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkSEZ.Checked)
            {
                chkimport.Checked = false;
                chkcstwith.Checked = chkCSTDeal.Checked = false;
                ddlSalesTax.Text = "None";
                ViewState["intsales_type"] = "4";
                ddlSEZZone.Enabled = true;
                chkSEZA1.Enabled = true;
                chkcstwith.Enabled = chkCSTDeal.Enabled = false;
                txtCFormNo.Text = "";
                txtCFormNo.Enabled = false;

                chkWithIGST.Enabled = true;
                chkWithIGST.Checked = true;
            }
            else
            {
                chkSEZA1.Checked = false;
                chkSEZA1.Enabled = false;
                ddlSEZZone.Enabled = false;
                ddlSEZZone.SelectedIndex = 0;

                chkWithIGST.Enabled = false;
                chkWithIGST.Checked = false;

                if (ViewState["Parent_Location_ID"] != null)
                {
                    string strParent_Location_ID = ViewState["Parent_Location_ID"].ToString();
                    //if (strParent_Location_ID == ddlDeliveryState.SelectedValue)
                    //{
                    //    ddlSalesTax.Text = "VAT";
                    //    //ddlSalesTax.Enabled = false;
                    //    ViewState["intsales_type"] = "1";
                    //    chkCSTDeal.Enabled = true;
                    //}
                    //else
                    //{
                    //    ddlSalesTax.Text = "CST";
                    //    //ddlSalesTax.Enabled = false;
                    //    chkcstwith.Enabled = true;
                    //    chkCSTDeal.Enabled = false;
                    //    ViewState["intsales_type"] = "2";
                    //}
                    ddlSalesTax.Text = "VAT";
                    //ddlSalesTax.Enabled = false;
                    ViewState["intsales_type"] = "1";
                    chkcstwith.Enabled = false;
                    chkCSTDeal.Enabled = true;
                }

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Unable to select Invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    private void FunPrisalesControls(int intModeID, String StrCST_Lease, String StrVAT_Lease)
    {
        // Bug ID  5847 - Start
        ViewState["cstleasing"] = "0";
        ViewState["vatleasing"] = "0";

        switch (intModeID)
        {
            case 1: // VAT
                if (StrVAT_Lease == "1")
                {
                    ddlSalesTax.Text = "VAT(VAT Leasing)";
                    ViewState["vatleasing"] = "1";
                }
                else
                    ddlSalesTax.Text = "VAT";

                chkcstwith.Enabled = false;
                ViewState["intsales_type"] = "1";
                ddlSEZZone.Enabled = false;
                chkCSTDeal.Enabled = true;
                chkCSTDeal.Checked = false;
                chkCSTDeal.Enabled = true;
                break;

            case 2: // CST
                if (StrCST_Lease == "1")
                {
                    ddlSalesTax.Text = "(CST Leasing)";
                    ViewState["cstleasing"] = "1";
                }
                else
                    ddlSalesTax.Text = "CST";

                chkcstwith.Enabled = true;
                ViewState["intsales_type"] = "2";
                ddlSEZZone.Enabled = false;
                chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                break;

            case 3://Import
                ddlSalesTax.Text = "None";
                chkcstwith.Enabled = false;
                ViewState["intsales_type"] = "3";
                chkimport.Checked = true;
                ddlSEZZone.Enabled = false;
                chkSEZA1.Enabled = true;
                break;
            case 4://SEZ
                ddlSalesTax.Text = "None";
                chkcstwith.Enabled = false;
                ViewState["intsales_type"] = "4";
                chkSEZ.Checked = true;
                ddlSEZZone.Enabled = true;
                chkWithIGST.Enabled = chkSEZA1.Enabled = true;
                break;
            case 6://CST - C
                if (StrCST_Lease == "1")
                {
                    ddlSalesTax.Text = "(CST Leasing)";
                    ViewState["cstleasing"] = "1";
                }
                else
                    ddlSalesTax.Text = "CST";
                chkcstwith.Enabled = true;
                ViewState["intsales_type"] = "6";
                chkcstwith.Checked = true;
                ddlSEZZone.Enabled = false;
                chkCSTDeal.Enabled = chkCSTDeal.Checked = true;
                break;
            // Bug ID  5847 - End
            case 13: // SGST & CGST
                ddlSalesTax.Text = "SGST&CGST";
                ViewState["cstleasing"] = "0";
                ViewState["intsales_type"] = "13";
                break;
            case 15: // IGST
                ddlSalesTax.Text = "IGST";
                ViewState["cstleasing"] = "0";
                ViewState["intsales_type"] = "15";
                break;
        }

    }

    private void FunPriIsSecondaryChanges()
    {
        if (chkIsSecondary.Checked)
        {
            pnlSecondaryGrid.Enabled = true;
            grvSecondaryGrid.Enabled = true;
            if (grvSecondaryGrid.Rows.Count > 0)
            {
                for (int gr = 0; gr < grvSecondaryGrid.Rows.Count; gr++)
                {
                    CheckBox chkSelectSG = (CheckBox)grvSecondaryGrid.Rows[gr].FindControl("chkSelectSG");
                    TextBox txtResidualPerSG = (TextBox)grvSecondaryGrid.Rows[gr].FindControl("txtResidualPerSG");
                    chkSelectSG.Enabled = false;
                    txtResidualPerSG.Enabled = true;
                }
            }
        }
        else
        {
            pnlSecondaryGrid.Enabled = true;
            grvSecondaryGrid.Enabled = true;
        }
        if (strMode == "M")
        {
            Primary_Secondary_Update();
        }
    }

    private void FunPriSumgrvSummaryInvoices()
    {
        try
        {
            string strpanum = string.Empty;
            System.Data.DataSet dsSum = new System.Data.DataSet();
            System.Data.DataTable dtsum = new System.Data.DataTable();
            Procparam = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                strpanum = txtPrimeAccountNo.Text;
            else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                strpanum = ddlApplicationReferenceNo.SelectedText;
            Procparam.Add("@PAnum", strpanum);
            Procparam.Add("@company_id", intCompanyId.ToString());
            Procparam.Add("@user_id", intUserId.ToString());
            if (chkITC.Checked)
                Procparam.Add("@ITC_Req", "1");
            dsSum = Utility.GetDataset("S3G_GET_Invoice_Summary_ACC_sum", Procparam);
            dtsum = dsSum.Tables[0];
            if (dtsum.Rows.Count > 0)
            {
                ///To insert TDS Section OPC102 start
                ////if (dsSum.Tables[1].Rows.Count > 0)
                ////{
                ////    if (ddlRentalTDSSec.Items.Count == 0)
                ////    {
                ////        Utility.FillDLL(ddlRentalTDSSec, dsSum.Tables[1], true);

                ////        //if (ddlRentalTDSSec.Items.Count == 2)
                ////        //    ddlRentalTDSSec.SelectedIndex = 1;
                ////    }
                ////}
                /// OPC102 end


                Label lblTot_Qty = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTot_Qty") as Label;
                lblTot_Qty.Text = dtsum.Rows[0]["Tot_Quantity"].ToString();

                //Label lblTotInvoice_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTotInvoice_Amount") as Label;
                Label lblTotInvoice_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotInvoice_Amount") as Label;
                lblTotInvoice_Amount.Text = dtsum.Rows[0]["INV_sum"].ToString();

                //Label lblTotAdditional_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTotAdditional_Amount") as Label;
                Label lblTotAdditional_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotAdditional_Amount") as Label;
                lblTotAdditional_Amount.Text = dtsum.Rows[0]["Additional_Amount"].ToString();

                Label lblTotCrNote = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotCrNote") as Label;
                lblTotCrNote.Text = dtsum.Rows[0]["Credit_Note_Amount"].ToString();

                //Label lblTotSchedule_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTotSchedule_Amount") as Label;
                Label lblTotSchedule_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotSchedule_Amount") as Label;
                lblTotSchedule_Amount.Text = dtsum.Rows[0]["sched_sum"].ToString();

                //Label lblTotlblLBT_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTotlblLBT_Amount") as Label;
                Label lblTotlblLBT_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotlblLBT_Amount") as Label;
                lblTotlblLBT_Amount.Text = dtsum.Rows[0]["LBT_Amount"].ToString();

                //Label lblTotPurchase_Tax = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTotPurchase_Tax") as Label;
                Label lblTotPurchase_Tax = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotPurchase_Tax") as Label;
                lblTotPurchase_Tax.Text = dtsum.Rows[0]["Purchase_Tax"].ToString();

                //Label lblTotlblEntry_Tax_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTotlblEntry_Tax_Amount") as Label;
                Label lblTotlblEntry_Tax_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotlblEntry_Tax_Amount") as Label;
                lblTotlblEntry_Tax_Amount.Text = dtsum.Rows[0]["Entry_Tax_Amount"].ToString();

                //Label lblTotlblReverse_Charge_Tax = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTotlblReverse_Charge_Tax") as Label;
                Label lblTotlblReverse_Charge_Tax = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotlblReverse_Charge_Tax") as Label;
                lblTotlblReverse_Charge_Tax.Text = dtsum.Rows[0]["Reverse_Charge_Tax"].ToString();

                //Label TotlblBase_Inv_Amt_Mat = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblBase_Inv_Amt_Mat") as Label;
                Label TotlblBase_Inv_Amt_Mat = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblBase_Inv_Amt_Mat") as Label;
                TotlblBase_Inv_Amt_Mat.Text = dtsum.Rows[0]["Inv_Amt_Material"].ToString();

                //Label TotlblBase_Inv_Amt_lbr = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblBase_Inv_Amt_lbr") as Label;
                Label TotlblBase_Inv_Amt_lbr = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblBase_Inv_Amt_lbr") as Label;
                TotlblBase_Inv_Amt_lbr.Text = dtsum.Rows[0]["Inv_Amt_Labour"].ToString();

                //Label TotlblCST = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblCST") as Label;
                Label TotlblCST = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblCST") as Label;
                TotlblCST.Text = dtsum.Rows[0]["CST"].ToString();

                //Label TotlblVAT = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblVAT") as Label;
                Label TotlblVAT = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblVAT") as Label;
                TotlblVAT.Text = dtsum.Rows[0]["VAT"].ToString();

                //Label TotlblExcise_Duty_CVD = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblExcise_Duty_CVD") as Label;
                Label TotlblExcise_Duty_CVD = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblExcise_Duty_CVD") as Label;
                TotlblExcise_Duty_CVD.Text = dtsum.Rows[0]["Excise_Duty_CVD"].ToString();

                //Label TotlblService_Tax_Amt = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblService_Tax_Amt") as Label;
                Label TotlblService_Tax_Amt = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblService_Tax_Amt") as Label;
                TotlblService_Tax_Amt.Text = dtsum.Rows[0]["Service_Tax_Amt"].ToString();

                //Label lblTtlRtntionAmount = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTtlRtntionAmount") as Label;
                Label lblTtlRtntionAmount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTtlRtntionAmount") as Label;
                lblTtlRtntionAmount.Text = dtsum.Rows[0]["Retention_Amount"].ToString();

                //Label Totlblothers = (Label)(grvSummaryInvoices).FooterRow.FindControl("Totlblothers") as Label;
                Label Totlblothers = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("Totlblothers") as Label;
                Totlblothers.Text = dtsum.Rows[0]["Others"].ToString();

                //Label TotlblTotal_SCH_Amt = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblTotal_SCH_Amt") as Label;
                Label TotlblTotal_SCH_Amt = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblTotal_SCH_Amt") as Label;
                TotlblTotal_SCH_Amt.Text = dtsum.Rows[0]["Total_SCH_Amt"].ToString();

                //Label TotlblAsset_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblAsset_Amount") as Label;
                Label TotlblAsset_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblAsset_Amount") as Label;
                TotlblAsset_Amount.Text = dtsum.Rows[0]["Asset_Amount"].ToString();

                //Label TotlblBase_Amt_VAT = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblBase_Amt_VAT") as Label;
                Label TotlblBase_Amt_VAT = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblBase_Amt_VAT") as Label;
                TotlblBase_Amt_VAT.Text = dtsum.Rows[0]["Base_Amt_VAT"].ToString();

                //Label lblTtlSmryITC_Value = (Label)(grvSummaryInvoices).FooterRow.FindControl("lblTtlSmryITC_Value") as Label;
                Label lblTtlSmryITC_Value = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTtlSmryITC_Value") as Label;
                lblTtlSmryITC_Value.Text = dtsum.Rows[0]["ITC_Value"].ToString();

                //Label TotlblTDS = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblTDS") as Label;
                Label TotlblTDS = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblTDS") as Label;
                TotlblTDS.Text = dtsum.Rows[0]["TDS_Amount"].ToString();

                //Label TotlblWCT_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblWCT_Amount") as Label;
                Label TotlblWCT_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblWCT_Amount") as Label;
                TotlblWCT_Amount.Text = dtsum.Rows[0]["WCT_Amount"].ToString();

                //Label TotlblCENVAT_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblCENVAT_Amount") as Label;
                Label TotlblCENVAT_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblCENVAT_Amount") as Label;
                TotlblCENVAT_Amount.Text = dtsum.Rows[0]["CENVAT_Amount"].ToString();
                //Label TotlblTotal_Deduction = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblTotal_Deduction") as Label;
                Label TotlblTotal_Deduction = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblTotal_Deduction") as Label;
                TotlblTotal_Deduction.Text = dtsum.Rows[0]["Total_Deduction"].ToString();

                //Label TotlblNet_Payable_Amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("TotlblNet_Payable_Amount") as Label;
                Label TotlblNet_Payable_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblNet_Payable_Amount") as Label;
                TotlblNet_Payable_Amount.Text = dtsum.Rows[0]["Net_Payable_Amount"].ToString();

                //Label Totlblcapitalised_amount = (Label)(grvSummaryInvoices).FooterRow.FindControl("Totlblcapitalised_amount") as Label;
                Label Totlblcapitalised_amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("Totlblcapitalised_amount") as Label;
                Totlblcapitalised_amount.Text = dtsum.Rows[0]["capitalised_amount"].ToString();

                Label TotllblCur_Capitalised_Amount = grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("TotllblCur_Capitalised_Amount") as Label;
                TotllblCur_Capitalised_Amount.Text = dtsum.Rows[0]["Cur_Capitalised_Amount"].ToString();

                Label lblFHSNSGST = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNSGST");
                Label lblFHSNCGST = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNCGST");
                Label lblFHSNIGST = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNIGST");
                Label lblFRebateDiscount = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFRebateDiscount");
                Label lblFCessAmount = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFCessAmount");
                Label lblFSACSGST = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACSGST");
                Label lblFSACCGST = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACCGST");
                Label lblFSACIGST = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACIGST");
                Label lblFHSNSGSTITC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNSGSTITC_Value");
                Label lblFHSNCGSTITC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNCGSTITC_Value");
                Label lblFHSNIGSTITC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNIGSTITC_Value");
                Label lblFSACSGSTITC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACSGSTITC_Value");
                Label lblFSACCGSTITC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACCGSTITC_Value");
                Label lblFSACIGSTITC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACIGSTITC_Value");
                Label lblFHSNSGSTRC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNSGSTRC_Value");
                Label lblFHSNCGSTRC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNCGSTRC_Value");
                Label lblFHSNIGSTRC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFHSNIGSTRC_Value");
                Label lblFSACSGSTRC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACSGSTRC_Value");
                Label lblFSACCGSTRC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACCGSTRC_Value");
                Label lblFSACIGSTRC_Value = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblFSACIGSTRC_Value");
                Label lblTCS_Amount = (Label)grvSummaryInvoices.Controls[grvSummaryInvoices.Controls.Count - 1].Controls[0].FindControl("lblTCS_Amount");

                lblFHSNSGST.Text = Convert.ToString(dtsum.Rows[0]["SGST"]);
                lblFHSNCGST.Text = Convert.ToString(dtsum.Rows[0]["CGST"]);
                lblFHSNIGST.Text = Convert.ToString(dtsum.Rows[0]["IGST"]);
                lblFRebateDiscount.Text = Convert.ToString(dtsum.Rows[0]["Rebate_Discount"]);
                lblFCessAmount.Text = Convert.ToString(dtsum.Rows[0]["Cess_Amount"]);
                lblFSACSGST.Text = Convert.ToString(dtsum.Rows[0]["SAC_SGST"]);
                lblFSACCGST.Text = Convert.ToString(dtsum.Rows[0]["SAC_CGST"]);
                lblFSACIGST.Text = Convert.ToString(dtsum.Rows[0]["SAC_IGST"]);
                lblFHSNSGSTITC_Value.Text = Convert.ToString(dtsum.Rows[0]["ITC_SGST_Amt"]);
                lblFHSNCGSTITC_Value.Text = Convert.ToString(dtsum.Rows[0]["ITC_CGST_Amt"]);
                lblFHSNIGSTITC_Value.Text = Convert.ToString(dtsum.Rows[0]["ITC_IGST_Amt"]);
                lblFSACSGSTITC_Value.Text = Convert.ToString(dtsum.Rows[0]["ITC_SAC_SGST_Amt"]);
                lblFSACCGSTITC_Value.Text = Convert.ToString(dtsum.Rows[0]["ITC_SAC_CGST_Amt"]);
                lblFSACIGSTITC_Value.Text = Convert.ToString(dtsum.Rows[0]["ITC_SAC_IGST_Amt"]);
                lblFHSNSGSTRC_Value.Text = Convert.ToString(dtsum.Rows[0]["RC_SGST_Amt"]);
                lblFHSNCGSTRC_Value.Text = Convert.ToString(dtsum.Rows[0]["RC_CGST_Amt"]);
                lblFHSNIGSTRC_Value.Text = Convert.ToString(dtsum.Rows[0]["RC_IGST_Amt"]);
                lblFSACSGSTRC_Value.Text = Convert.ToString(dtsum.Rows[0]["RC_SAC_SGST_Amt"]);
                lblFSACCGSTRC_Value.Text = Convert.ToString(dtsum.Rows[0]["RC_SAC_CGST_Amt"]);
                lblFSACIGSTRC_Value.Text = Convert.ToString(dtsum.Rows[0]["RC_SAC_IGST_Amt"]);
                lblTCS_Amount.Text = Convert.ToString(dtsum.Rows[0]["TCS_Amount"]);
            }
        }
        catch(Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunprisumgrvPrimaryGrid()
    {

        try
        {
            System.Data.DataTable dtPrimaryGridsum = (DataTable)ViewState["dtPrimaryGridsum"];
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@OPTION", "42");
            Procparam.Add("@Pricing_ID", ddlApplicationReferenceNo.SelectedValue);
            dtsum.Clear();
            dtsum = Utility.GetDefaultData("S3G_CLN_LOADLOV", Procparam);

            Label lblTotFacilityAmountPG = (Label)(grvPrimaryGrid).FooterRow.FindControl("lblTotFacilityAmountPG") as Label;
            lblTotFacilityAmountPG.Text = dtPrimaryGridsum.Compute("sum(FacilityAmount)", "").ToString();

            Label lblTotUtilizedAmountPG = (Label)(grvPrimaryGrid).FooterRow.FindControl("lblTotUtilizedAmountPG") as Label;
            lblTotUtilizedAmountPG.Text = dtPrimaryGridsum.Compute("sum(UtilizedAmount)", "").ToString();

            Label lblTotbalanceAmountPG = (Label)(grvPrimaryGrid).FooterRow.FindControl("lblTotbalanceAmountPG") as Label;
            lblTotbalanceAmountPG.Text = dtPrimaryGridsum.Compute("sum(balance_amount)", "").ToString();

            Label lblTotMappedAmountPG = (Label)(grvPrimaryGrid).FooterRow.FindControl("lblTotMappedAmountPG") as Label;
            lblTotMappedAmountPG.Text = dtPrimaryGridsum.Compute("sum(MappedAmount)", "").ToString();

            Label lblTotRVamountPG = (Label)(grvPrimaryGrid).FooterRow.FindControl("lblTotRVamountPG") as Label;
            lblTotRVamountPG.Text = dtPrimaryGridsum.Compute("sum(RV_amount)", "").ToString();

            ViewState["GetAssetValue"] = dtsum.Rows[0][0].ToString();
            if (dtsum.Rows[0][0].ToString() != null)
            {
                if (dtsum.Rows[0][0].ToString() == "0")
                {
                    lblTotFacilityAmountPG.Visible = false;
                    lblTotUtilizedAmountPG.Visible = false;
                    lblTotbalanceAmountPG.Visible = false;
                }
                else
                {
                    lblTotFacilityAmountPG.Visible = true;
                    lblTotUtilizedAmountPG.Visible = true;
                    lblTotbalanceAmountPG.Visible = true;
                }
            }
        }
        catch(Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunprisumgrvSecondaryGrid()
    {
        try
        {
            System.Data.DataTable dtSecondaryGridsum = (DataTable)ViewState["dtSecondaryGridsum"];

            if (grvSecondaryGrid.Rows.Count > 0)
            {
                Label lblTotFacilityAmountSG = (Label)(grvSecondaryGrid).FooterRow.FindControl("lblTotFacilityAmountSG") as Label;
                lblTotFacilityAmountSG.Text = dtSecondaryGridsum.Compute("sum(FacilityAmount)", "").ToString();

                Label lblTotUtilizedAmountSG = (Label)(grvSecondaryGrid).FooterRow.FindControl("lblTotUtilizedAmountSG") as Label;
                lblTotUtilizedAmountSG.Text = dtSecondaryGridsum.Compute("sum(UtilizedAmount)", "").ToString();

                Label lblTotbalanceAmountSG = (Label)(grvSecondaryGrid).FooterRow.FindControl("lblTotbalanceAmountSG") as Label;
                lblTotbalanceAmountSG.Text = dtSecondaryGridsum.Compute("sum(balance_amount)", "").ToString();

                Label lblTotMappedAmountSG = (Label)(grvSecondaryGrid).FooterRow.FindControl("lblTotMappedAmountSG") as Label;
                lblTotMappedAmountSG.Text = dtSecondaryGridsum.Compute("sum(MappedAmount)", "").ToString();

                Label lblTotRVamountSG = (Label)(grvSecondaryGrid).FooterRow.FindControl("lblTotRVamountSG") as Label;
                lblTotRVamountSG.Text = dtSecondaryGridsum.Compute("sum(RV_amount)", "").ToString();

                if (ViewState["GetAssetValue"] != null)
                {
                    if (ViewState["GetAssetValue"].ToString() == "0")
                    {
                        lblTotFacilityAmountSG.Visible = false;
                        lblTotUtilizedAmountSG.Visible = false;
                        lblTotbalanceAmountSG.Visible = false;
                    }
                    else
                    {
                        lblTotFacilityAmountSG.Visible = true;
                        lblTotUtilizedAmountSG.Visible = true;
                        lblTotbalanceAmountSG.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void btnDevModalTotal_Click(object sender, EventArgs e)
    {
        //ViewState["dtInvoiceGrid"] = dtInvoiceGrid;
        try
        {

            FunPriFindTotal();

        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to Move invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }


    private void FunPriFindTotal()
    {
        try
        {
            if (ViewState["dtInvoiceGrid"] != null)
                dtInvoiceGrid = (System.Data.DataTable)ViewState["dtInvoiceGrid"];
            if (dtInvoiceGrid.Rows.Count > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                Procparam.Add("@User_ID", intUserId.ToString());
                Procparam.Add("@Proposal_Type", ddlProposalType.SelectedValue);
                Procparam.Add("@xml_Invoice", dtInvoiceGrid.FunPubFormXml(true));
                dtsum = Utility.GetDefaultData("S3G_GET_Invoice_PIVI_ACC_sum", Procparam);
            }
            else
            {
                string strPANumber = string.Empty;
                Procparam = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(txtPrimeAccountNo.Text))
                    strPANumber = txtPrimeAccountNo.Text;
                else if (!string.IsNullOrEmpty(ddlApplicationReferenceNo.SelectedText))
                    strPANumber = ddlApplicationReferenceNo.SelectedText;
                Procparam.Add("@PAnum", strPANumber);
                Procparam.Add("@Customer_ID", ViewState["Customer_Id"].ToString());
                Procparam.Add("@Search_Type", ddlFilterType.SelectedValue);
                Procparam.Add("@Pricing_Id", ddlApplicationReferenceNo.SelectedValue);
                Procparam.Add("@Proposal_Type", ddlProposalType.SelectedValue);
                Procparam.Add("@itc_value", chkITC.Checked.ToString());
                Procparam.Add("@option", "3");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
                Procparam.Add("@User_ID", Convert.ToString(intUserId));
                Procparam.Add("@Delivery_State", ddlDeliveryState.SelectedValue);
                if (ViewState["strXML"] != null)
                    if (!string.IsNullOrEmpty(ViewState["strXML"].ToString()))
                        Procparam.Add("@XMLAssetCategory", ViewState["strXML"].ToString());

                dtsum = Utility.GetDefaultData("S3G_GET_Invoice_PIVI_ACC", Procparam);
            }

            if (dtsum != null && dtsum.Rows.Count > 0)
            {
                //Label TotlblInvoice_Amount = (Label)(grvInvoices).FooterRow.FindControl("TotlblInvoice_Amount") as Label;
                Label TotlblInvoice_Amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblInvoice_Amount") as Label;
                TotlblInvoice_Amount.Text = Convert.ToDecimal(dtsum.Compute("sum(Invoice_Amount)", "")).ToString(Funsetsuffix());

                //Label TotlblLBT_Amount = (Label)(grvInvoices).FooterRow.FindControl("TotlblLBT_Amount") as Label;
                Label TotlblLBT_Amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblLBT_Amount") as Label;
                TotlblLBT_Amount.Text = Convert.ToDecimal(dtsum.Compute("sum(LBT_Amount)", "")).ToString();

                //Label TotlblPurchase_Tax = (Label)(grvInvoices).FooterRow.FindControl("TotlblPurchase_Tax") as Label;
                Label TotlblPurchase_Tax = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblPurchase_Tax") as Label;
                TotlblPurchase_Tax.Text = Convert.ToDecimal(dtsum.Compute("sum(Purchase_Tax)", "")).ToString();

                //Label TotlblEntry_Tax_Amount = (Label)(grvInvoices).FooterRow.FindControl("TotlblEntry_Tax_Amount") as Label;
                Label TotlblEntry_Tax_Amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblEntry_Tax_Amount") as Label;
                TotlblEntry_Tax_Amount.Text = Convert.ToDecimal(dtsum.Compute("sum(Entry_Tax_Amount)", "")).ToString();

                //Label TotlblReverse_Charge_Tax = (Label)(grvInvoices).FooterRow.FindControl("TotlblReverse_Charge_Tax") as Label;
                Label TotlblReverse_Charge_Tax = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblReverse_Charge_Tax") as Label;
                TotlblReverse_Charge_Tax.Text = Convert.ToDecimal(dtsum.Compute("sum(Reverse_Charge_Tax)", "")).ToString();

                //Label TotlblBase_Inv_Amt_Mat = (Label)(grvInvoices).FooterRow.FindControl("TotlblBase_Inv_Amt_Mat") as Label;
                Label TotlblBase_Inv_Amt_Mat = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblBase_Inv_Amt_Mat") as Label;
                TotlblBase_Inv_Amt_Mat.Text = Convert.ToDecimal(dtsum.Compute("sum(Base_Inv_Amt_Mat)", "")).ToString();

                //Label TotlblBase_Inv_Amt_lbr = (Label)(grvInvoices).FooterRow.FindControl("TotlblBase_Inv_Amt_lbr") as Label;
                Label TotlblBase_Inv_Amt_lbr = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblBase_Inv_Amt_lbr") as Label;
                TotlblBase_Inv_Amt_lbr.Text = Convert.ToDecimal(dtsum.Compute("sum(Base_Inv_Amt_lbr)", "")).ToString();

                //Label TotlblVAT = (Label)(grvInvoices).FooterRow.FindControl("TotlblVAT") as Label;
                Label TotlblVAT = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblVAT") as Label;
                TotlblVAT.Text = Convert.ToDecimal(dtsum.Compute("sum(VAT)", "")).ToString();

                //Label TotlblExcise_Duty_CVD = (Label)(grvInvoices).FooterRow.FindControl("TotlblExcise_Duty_CVD") as Label;
                Label TotlblExcise_Duty_CVD = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblExcise_Duty_CVD") as Label;
                TotlblExcise_Duty_CVD.Text = Convert.ToDecimal(dtsum.Compute("sum(Excise_Duty_CVD)", "")).ToString();

                //Label TotlblService_Tax_Amt = (Label)(grvInvoices).FooterRow.FindControl("TotlblService_Tax_Amt") as Label;
                Label TotlblService_Tax_Amt = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblService_Tax_Amt") as Label;
                TotlblService_Tax_Amt.Text = Convert.ToDecimal(dtsum.Compute("sum(Service_Tax_Amt)", "")).ToString();

                //Label Totlblothers = (Label)(grvInvoices).FooterRow.FindControl("Totlblothers") as Label;
                Label Totlblothers = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("Totlblothers") as Label;
                Totlblothers.Text = Convert.ToDecimal(dtsum.Compute("sum(others)", "")).ToString();

                //Label TotlblTotal_Bill_Amount = (Label)(grvInvoices).FooterRow.FindControl("TotlblTotal_Bill_Amount") as Label;
                Label TotlblTotal_Bill_Amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblTotal_Bill_Amount") as Label;
                TotlblTotal_Bill_Amount.Text = Convert.ToDecimal(dtsum.Compute("sum(Total_Bill_Amount)", "")).ToString();

                Label lblTotCrNote = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotCrNote") as Label;
                lblTotCrNote.Text = Convert.ToDecimal(dtsum.Compute("sum(Credit_Note_Amount)", "")).ToString();

                //Label TotlblTotal_SCH_Amt = (Label)(grvInvoices).FooterRow.FindControl("TotlblTotal_SCH_Amt") as Label;
                Label TotlblTotal_SCH_Amt = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblTotal_SCH_Amt") as Label;
                TotlblTotal_SCH_Amt.Text = Convert.ToDecimal(dtsum.Compute("sum(Total_SCH_Amt)", "")).ToString();

                //Label TotlblAsset_Amount = (Label)(grvInvoices).FooterRow.FindControl("TotlblAsset_Amount") as Label;
                Label TotlblAsset_Amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblAsset_Amount") as Label;
                TotlblAsset_Amount.Text = Convert.ToDecimal(dtsum.Compute("sum(Asset_Amount)", "")).ToString();

                //Label TotlblBase_Amt_VAT = (Label)(grvInvoices).FooterRow.FindControl("TotlblBase_Amt_VAT") as Label;
                Label TotlblBase_Amt_VAT = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblBase_Amt_VAT") as Label;
                TotlblBase_Amt_VAT.Text = Convert.ToDecimal(dtsum.Compute("sum(Base_Amt_VAT)", "")).ToString();

                //Label TotlblITC_Value = (Label)(grvInvoices).FooterRow.FindControl("TotlblITC_Value") as Label;
                Label TotlblITC_Value = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblITC_Value") as Label;
                TotlblITC_Value.Text = Convert.ToDecimal(dtsum.Compute("sum(ITC_Value)", "")).ToString();

                //Label TotlblTDS = (Label)(grvInvoices).FooterRow.FindControl("TotlblTDS") as Label;
                Label TotlblTDS = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblTDS") as Label;
                TotlblTDS.Text = Convert.ToDecimal(dtsum.Compute("sum(TDS)", "")).ToString();

                //Label TotlblWCT_Amount = (Label)(grvInvoices).FooterRow.FindControl("TotlblWCT_Amount") as Label;
                Label TotlblWCT_Amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblWCT_Amount") as Label;
                TotlblWCT_Amount.Text = Convert.ToDecimal(dtsum.Compute("sum(WCT_Amount)", "")).ToString();

                //Label TotlblCENVAT_Amount = (Label)(grvInvoices).FooterRow.FindControl("TotlblCENVAT_Amount") as Label;
                Label TotlblCENVAT_Amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblCENVAT_Amount") as Label;
                TotlblCENVAT_Amount.Text = Convert.ToDecimal(dtsum.Compute("sum(CENVAT_Amount)", "")).ToString();

                //Label TotlblTotal_Deduction = (Label)(grvInvoices).FooterRow.FindControl("TotlblTotal_Deduction") as Label;
                Label TotlblTotal_Deduction = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblTotal_Deduction") as Label;
                TotlblTotal_Deduction.Text = Convert.ToDecimal(dtsum.Compute("sum(Total_Deduction)", "")).ToString();

                //Label TotlblNet_Payable_Amount = (Label)(grvInvoices).FooterRow.FindControl("TotlblNet_Payable_Amount") as Label;
                Label TotlblNet_Payable_Amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblNet_Payable_Amount") as Label;
                TotlblNet_Payable_Amount.Text = Convert.ToDecimal(dtsum.Compute("sum(Net_Payable_Amount)", "")).ToString();

                //Label Totlblcapitalised_amount = (Label)(grvInvoices).FooterRow.FindControl("Totlblcapitalised_amount") as Label;
                Label Totlblcapitalised_amount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("Totlblcapitalised_amount") as Label;
                Totlblcapitalised_amount.Text = Convert.ToDecimal(dtsum.Compute("sum(capitalisation_amount)", "")).ToString(Funsetsuffix());

                //Label TotlblCST = (Label)(grvInvoices).FooterRow.FindControl("TotlblCST") as Label;
                Label TotlblCST = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("TotlblCST") as Label;
                TotlblCST.Text = Convert.ToDecimal(dtsum.Compute("sum(CST)", "")).ToString();

                //Label TtlRtntionAmount = (Label)(grvInvoices).FooterRow.FindControl("lblTotalRetention") as Label;
                Label TtlRtntionAmount = grvInvoices.Controls[grvInvoices.Controls.Count - 1].Controls[0].FindControl("lblTotalRetention") as Label;
                TtlRtntionAmount.Text = Convert.ToDecimal(dtsum.Compute("sum(Retention_Amount)", "")).ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabMainPage.ErrorMessage = "Due to Data Problem,Unable to Move Invoices.";
            cv_TabMainPage.IsValid = false;
        }


    }

    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    //Added on 31Mar2015 Starts here

    private void FunPriCalculateServiceTax()
    {
        try
        {
            DataTable dtcashInflow = (DataTable)ViewState["DtCashFlow"];

            DataRow[] drCashflow = dtcashInflow.Select("CashFlow_Flag_ID = 83");

            foreach (DataRow drDelete in drCashflow)
            {
                drDelete.Delete();
            }
            dtcashInflow.AcceptChanges();
            DtCashFlow = dtcashInflow;

            Int32 iRwCnt = dtcashInflow.Rows.Count;

            if (iRwCnt > 0)
            {
                string[] strArrayIds = null;
                string cashflowdesc = string.Empty;
                string strCashFlowID = string.Empty;
                System.Data.DataSet dsSD = (System.Data.DataSet)ViewState["InflowDDL"];
                foreach (DataRow drOut in dsSD.Tables[2].Rows)
                {
                    string[] strCashflow = drOut["CashFlow_ID"].ToString().Split(',');
                    if (strCashflow[4].ToString() == "83")
                    {
                        strArrayIds = strCashflow;
                        cashflowdesc = drOut["CashFlow_Description"].ToString();
                        strCashFlowID = strCashflow[0];
                    }
                }
                if (strArrayIds == null)
                {
                    Utility.FunShowAlertMsg(this, "Define the Cashflow for Service Tax against RS Activation in Cashflow Master");
                    return;
                }

                for (int i = 0; i < iRwCnt; i++)
                {
                    if (ViewState["ServiceTax_Cashflows"] == null)
                        return;
                    DataTable dtSTCF = (DataTable)ViewState["ServiceTax_Cashflows"];

                    DataRow[] drCheck = dtSTCF.Select("CashFlow_Flag_ID = " + Convert.ToString(dtcashInflow.Rows[i]["CashFlow_Flag_ID"]));
                    if (drCheck.Length > 0)
                    {
                        DataRow dr = DtCashFlow.NewRow();
                        //DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["Date"], DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["InflowFromId"], DtCashFlow.Columns["EntityID"] };
                        //DtCashFlow.PrimaryKey = null;
                        //dr["Date"] = DateTime.Today.ToString();
                        if (Utility.StringToDate(dtcashInflow.Rows[i]["Date"].ToString()) < Utility.StringToDate("1/7/2017"))
                        {
                            dr["Date"] = dtcashInflow.Rows[i]["Date"];
                            dr["CashInFlowID"] = Convert.ToString(strCashFlowID);
                            dr["Accounting_IRR"] = dtcashInflow.Rows[i]["Accounting_IRR"];
                            dr["Business_IRR"] = dtcashInflow.Rows[i]["Business_IRR"];
                            dr["Company_IRR"] = dtcashInflow.Rows[i]["Company_IRR"];
                            dr["CashFlow_Flag_ID"] = "83";
                            dr["CashFlowID"] = Convert.ToString(dtcashInflow.Rows[i]["CashFlow_Flag_ID"]);
                            dr["CashInFlow"] = cashflowdesc;
                            dr["EntityID"] = dtcashInflow.Rows[i]["EntityID"];
                            dr["Entity"] = dtcashInflow.Rows[i]["Entity"];
                            dr["InflowFromId"] = dtcashInflow.Rows[i]["InflowFromId"];
                            dr["InflowFrom"] = dtcashInflow.Rows[i]["InflowFrom"];
                            dr["Amount"] = ((Convert.ToDecimal(dtcashInflow.Rows[i]["Amount"]) * Convert.ToDecimal(ViewState["ServiceTax"]) / 100)
                                + (Convert.ToDecimal(dtcashInflow.Rows[i]["Amount"]) * Convert.ToDecimal(ViewState["Addtl_Srv_Tax"]) / 100)).ToString(Funsetsuffix());

                            DtCashFlow.Rows.InsertAt(dr, i + 1);
                            iRwCnt = iRwCnt + 1;
                        }
                        

                    }
                }
                gvInflow.DataSource = DtCashFlow;
                gvInflow.DataBind();

                ViewState["DtCashFlow"] = DtCashFlow;
                FunPriGenerateNewInflowRow();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    //Added on 31Mar2015 Ends here
    protected void btnExportRepay_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["RepaymentStructure"] != null)
            {
                FunPriExportExcel();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Repayment Structure not yet generated");
            }
        }
        catch (Exception ObjException)
        {

        }
    }

   
        public void FunPriExportExcel()
    {
        try
        {
            DataTable dtGetdata = (DataTable)ViewState["RepaymentStructure"];
            DataView dvData = new DataView(dtGetdata);
            dtGetdata = dvData.ToTable("Selected", false, "InstallmentNo", "From_Date", "To_Date", "Installment_Date", "noofdays", "InstallmentAmount",
                "Tax", "AMF", "ServiceTax", "Insurance", "Others", "Cess_Amount","Rebate_Discount", "Addi_Rebate_Discount");
            dtGetdata.Columns["InstallmentNo"].ColumnName = "Installment No";
            dtGetdata.Columns["From_Date"].ColumnName = "From Date";
            dtGetdata.Columns["To_Date"].ColumnName = "To Date";
            dtGetdata.Columns["Installment_Date"].ColumnName = "Installment Date";
            dtGetdata.Columns["noofdays"].ColumnName = "No of Days";
            dtGetdata.Columns["InstallmentAmount"].ColumnName = "Rental Amount";
            dtGetdata.Columns["Tax"].ColumnName = "Tax";
            dtGetdata.Columns["AMF"].ColumnName = "AMF";
            dtGetdata.Columns["ServiceTax"].ColumnName = "Service Tax";
            dtGetdata.Columns["Cess_Amount"].ColumnName = "Cess Amount";
            dtGetdata.Columns["Rebate_Discount"].ColumnName = "Rebate Discount";
            dtGetdata.Columns["Addi_Rebate_Discount"].ColumnName = "Additional Rebate Discount";

            GridView Grv = new GridView();

            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Account_Repayment_Structure.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Repayment Structure";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();

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
        catch (Exception ObjException)
        {
        }
    }

    //Added on 06Jul2015 Starts here
    private void FunPriClearTempInvoiceDtl()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            Procparam.Add("@Option", "1");
            Procparam.Add("@Panum", ((Convert.ToString(strMode) == "C") ? Convert.ToString(ddlApplicationReferenceNo.SelectedText) : Convert.ToString(txtPrimeAccountNo.Text)));
            Utility.GetDefaultData("S3G_LAD_RSCmnLst", Procparam);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }
    //Added on 06Jul2015 Ends here

    #region Customer Email Det
    //opc042 start
    protected void grvCustEmail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblEmailAlert = e.Row.FindControl("lblEmail_Alert") as Label;
                CheckBox ChkAlertEmail = e.Row.FindControl("chkEmailAlert") as CheckBox;
                if (lblEmailAlert.Text == "1")
                {
                    ChkAlertEmail.Checked = true;
                }
                else
                {
                    ChkAlertEmail.Checked = false;
                }
                //ChkAlertEmail.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string FunCustEmailBuilder()
    {
        StringBuilder strCustEmailBuilder = new StringBuilder();
        strCustEmailBuilder.Append("<ROOT>");

        try
        {
            foreach (GridViewRow grvCust in grvCustEmail.Rows)
            {
                CheckBox chkEmailAlert = (CheckBox)grvCust.FindControl("chkEmailAlert");

                if (chkEmailAlert.Checked)
                {
                    intCustEmailRows = intCustEmailRows + 1;
                    strCustEmailBuilder.Append(" <DETAILS ");
                    strCustEmailBuilder.Append("Cust_Email_Det_ID='" + ((Label)grvCust.FindControl("lblCustEmailDetID")).Text + "'");
                    strCustEmailBuilder.Append(" ");
                    strCustEmailBuilder.Append("/>");
                }
            }
            strCustEmailBuilder.Append("</ROOT>");
            // return strbXml.ToString();
            return strCustEmailBuilder.ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //opc042 end
    #endregion
}

