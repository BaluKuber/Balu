#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Loan Admin
/// Screen Name         :   Payment Request
/// Created By          :   S.Kannan
/// Created Date        :   27-July-2010 
/// Purpose             :   To raise the payment request
/// Last Updated By		:   M.Saran 
/// Last Updated Date   :   NULL
/// Reason              :   To raise the payment request
/// Last Updated By		:   Thalaiselvam N
/// Last Updated Date   :   03-Sep-2011
/// Reason              :   Encrypted Password Validation

/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using S3GBusEntity.Origination;
using System.IO;
using System.Web.Security;
using S3GBusEntity.LoanAdmin;

using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using S3GBusEntity.Reports;
using System.Collections;

#endregion

public partial class Loan_Admin_S3GLoanAdminPaymentRequest : ApplyThemeForProject
{
    #region "Variables"

    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjLoanAdminAccMgtServicesClient;
    LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable ObjPaymentDetailsDt = new LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable();

    // to get the user details
    UserInfo userInfo = new UserInfo();

    Dictionary<string, string> Procparam = null;
    int intUserID = 0;
    int intCompanyID = 0;
    string strDateFormat;
    const string strRedirectOnCancel = "S3gLoanAdTransLander.aspx?Code=PARE";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PARE';";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3gLoanAdPaymentRequest.aspx?qsMode=C';";
    static string strPageName = "Payment Request";
    static bool blnChkSlctAll;

    #region  User Authorization
    bool bCreate = false;
    bool bModify = false;
    bool bDelete = false;
    bool bQuery = false;
    bool bIsActive = false;
    #endregion

    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    DataTable dtPaymenttable = new DataTable();

    // to hold the query string
    string strQsMode = string.Empty;
    string strRequestID = string.Empty;
    public static Loan_Admin_S3GLoanAdminPaymentRequest obj_Page;
    ClsSystemJournal ObjSysJournal = new ClsSystemJournal();

    #region "Page Variables"

    string[] arrSortCol = new string[] { "PODTL.PO_Number" };
    int intNoofSearch = 1;

    ArrayList arrSearchVal = new ArrayList(1);
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

    public int ProPageSizeRW1
    {
        get;
        set;
    }

    public int ProPageNumRW1
    { get; set; }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGrid();
    }

    // Code Added By C.Aswinkrishna on 8-Feeb-2016 Start //

    protected void AssignValue1(int intPageNum, int intPageSize)
    {
        ProPageNumRW1 = intPageNum;
        ProPageSizeRW1 = intPageSize;
        btnAddPOInvoice_Click(new object(), new EventArgs());
    }

    // Code Added By C.Aswinkrishna on 8-Feeb-2016 End //

    #endregion

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            #region Application Standard Date Format

            strDateFormat = ObjS3GSession.ProDateFormatRW;                              // to get the standard date format of the Application
            CalendarExtenderValueDate.Format = strDateFormat;
            CalendarExtenderPaymentRequestDate.Format = strDateFormat;
            CalendarExtenderInstrumentDate.Format = strDateFormat;


            #endregion

            #region User Information

            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            #endregion

            #region  User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            #endregion

            if (Request.QueryString["Popup"] != null)
                btnCancel.Enabled = false;

            GetQueryString();

            obj_Page = this;
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
            TextBox txtName = ucCustomerCodeLov.FindControl("txtName") as TextBox;
            txtName.Attributes.Add("onfocus", "fnLoadCustomer()");
            ucCustomerAddress.ShowCustomerCode = false;
            // for password popup
            ModalPopupExtenderPassword.Enabled = false;

            if (ddlPayTo.SelectedValue == "12")
            {
                ucCustomerCodeLov.DispalyContent = UserControls_LOBMasterView.enumContentType.Name;
            }

            #region Paging Config

            arrSearchVal = new ArrayList(intNoofSearch);
            ProPageNumRW = 1;
            ProPageNumRW1 = 1;

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

            // Code Added By C.Aswinkrishna start on 8-Feb-2016 //

            TextBox txtPageSize1 = (TextBox)Pagenavigator1.FindControl("txtPageSize");
            TextBox txtGotoPage1 = (TextBox)Pagenavigator1.FindControl("txtGotoPage");
            if (txtPageSize1.Text != "")
                ProPageSizeRW1 = Convert.ToInt32(txtPageSize1.Text);
            else
                ProPageSizeRW1 = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            if (txtGotoPage1.Text != "")
                ProPageNumRW1 = Convert.ToInt32(txtGotoPage1.Text);

            PageAssignValue obj1 = new PageAssignValue(this.AssignValue1);
            Pagenavigator1.callback1 = obj1;
            Pagenavigator1.ProPageNumRW = ProPageNumRW1;
            Pagenavigator1.ProPageSizeRW = ProPageSizeRW1;

            // Code Added By C.Aswinkrishna End on 8-Feb-2016 //

            #endregion

            if (!IsPostBack)
            {
                ViewState["Sum"] = 0;
                ceInvoiceEndDate.Format = ceInvoiceStartDate.Format = strDateFormat;
                txtInvoiceStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoiceStartDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtInvoiceEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoiceEndDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtInstrumentDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInstrumentDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtPaymentRequestDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInstrumentDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtPaymentRequestDate.Text = DateTime.Now.ToString(strDateFormat);
                //txtValueDate.Enabled = false;
                txtValueDate.Enabled = false;
                txtValueDate.Text = DateTime.Now.ToString(strDateFormat);

                FunPriLoadLOV();

                // to change the view - depend on the query string
                FunPriFormActToMode();

            }
            FunPriSetMaxLength();
            // WF Initializtion 
            ProgramCode = "054";
            // WORK FLOW IMPLEMENTATION
            if (PageMode == PageModes.WorkFlow)
            {
                try
                {
                    ViewState["PageMode"] = PageModes.WorkFlow;
                    PreparePageForWorkFlowLoad();
                    if (ViewState["strRequestID"] != null)
                        strRequestID = ViewState["strRequestID"].ToString();
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLog.CustomErrorRoutine(ex);
                    Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
                }
            }
            FunLoadPayToDetails();
        }
        catch (TimeoutException tex)
        {
            Utility.FunShowAlertMsg(this, tex.ToString());
        }
    }

    #region "EVENTS"

    #region "DropDownList Event"

    protected void ddlPayMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlPayMode.SelectedValue == "3")
            {
                TabPanelPBD.Enabled = false;
            }
            else
            {
                TabPanelPBD.Enabled = true;
                Int32 iPayMode = Convert.ToInt32(ddlPayMode.SelectedValue);
                lblInstrumentDate.Text = (iPayMode == 5 || iPayMode == 6) ? "Transfer Date" : "Instrument Date";
                lblFavouringName.CssClass = (iPayMode == 5 || iPayMode == 6) ? "styleDisplayLabel" : "styleReqFieldLabel";
                if (ddlPayMode.SelectedValue == "2")
                {
                    RBLCompanyCashorBankAcct.Visible = RBLCompanyCashorBankAcct.Enabled = true;
                    txtInstrumentNumber.ReadOnly = false;
                    lblInstrumentNumber.CssClass = "styleReqFieldLabel";
                }
                else
                {
                    RBLCompanyCashorBankAcct.Enabled = RBLCompanyCashorBankAcct.Visible = false;
                    txtInstrumentNumber.ReadOnly = true;
                    lblInstrumentNumber.CssClass = "styleDisplayLabel";
                }
                FunPriLoadBankCodes();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "1");
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));
            DataTable dtPayTo = Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam);
            FunPriFillDropdown(dtPayTo, ddlPayTo, "Lookup_Code", "Lookup_Description", true);
            ddlPayTo.Enabled = true;
            btnShow.Enabled = PanelPayType.Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) > 0 && Convert.ToInt32(ddlPaymentType.SelectedValue) < 5) ? true : false;

            ddlReceiptFrom.Clear();
            ddlTranche.Clear();
            ddlReceiptFrom.ReadOnly = ddlTranche.ReadOnly = (Convert.ToInt32(ddlPaymentType.SelectedValue) == 3 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 5) ? false : true;
            ddlReceiptFrom.IsMandatory = (Convert.ToInt32(ddlPaymentType.SelectedValue) == 3 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 5) ? true : false;
            FunPriBindGridDtls(2, null);
            //FunPriEnableDisableAdjsumentGrid();
            //tpInvoiceLoading.Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) > 0 && Convert.ToInt32(ddlPaymentType.SelectedValue) < 5) ? true : false;
            ddlInvoiceSortBy.Items.FindByValue("3").Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4) ? false : true;
            //ddlInvoiceSortBy.Items.FindByValue("4").Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4) ? false : true;
            //ddlInvoiceSortBy.Items.FindByValue("5").Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4) ? false : true;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void ddlPayTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunGridBlank();
            FunPriClearCustomerDtls();
            PnlCustEntityInformation.Enabled = ucCustomerCodeLov.ButtonEnabled = rfvcmbCustomer.Enabled = true;
            chkAccountBased.Enabled = false;//this is applicable only for general
            Int32 intPaymentType = Convert.ToInt32(ddlPaymentType.SelectedValue);
            chkAccountBased.SelectedValue = ((intPaymentType > 0 && intPaymentType < 6) || intPaymentType == 8) ? "1" : "0";
            FunProToggleCustomerAddress(true);
            rfvcmbCustomer.ErrorMessage = "Select a Customer/Entity";
            //PanelPaymentAdjustment.Visible = true;
            if (ddlPayTo != null && ddlPayTo.SelectedIndex > 0)
            {
                FunPriLoadPaytypeingrid(Convert.ToString(ddlPayTo.SelectedValue));
                FunPriLoadGLcodes();
                if (ddlPayTo.SelectedValue == "50")
                {
                    PnlCustEntityInformation.Enabled = ucCustomerCodeLov.ButtonEnabled =
                    rfvcmbCustomer.Enabled = false;
                    if (chkAccountBased.SelectedValue == "1")
                    {
                        FunDisableGLSLCodeGrid(true);
                        FundisableGridValidation(false);
                    }
                    else
                    {
                        FunDisableGLSLCodeGrid(false);
                        FundisableGridValidation(true);
                    }
                    chkAccountBased.Enabled = true;
                }
                else if (ddlPayTo.SelectedValue == "1")
                {
                    ucCustomerAddress.Caption = "Lessee";
                    FunDisableGLSLCodeGrid(false);
                    FundisableGridValidation(true);
                }
                else if (ddlPayTo.SelectedValue == "11")
                {
                    ucCustomerAddress.Caption = "Insurance Company";
                    rfvcmbCustomer.ErrorMessage = "Select a Insurance Company";
                    lblCode.Text = "Insurance Company Code";
                    FunDisableGLSLCodeGrid(false);
                    FundisableGridValidation(true);
                }
                else if (ddlPayTo.SelectedValue == "12")  // Others
                {
                    if (ddlLOB.SelectedItem.Text.Contains("FT"))
                    {
                        chkAccountBased.Enabled = true;
                    }
                    ucCustomerAddress.Caption = "Party";
                    rfvcmbCustomer.ErrorMessage = "Select a Party";
                    lblCode.Text = "Party Name";

                    FunDisableGLSLCodeGrid(false);
                    FundisableGridValidation(true);
                }
                else if (Convert.ToInt32(ddlPayTo.SelectedValue) == 13)
                {
                    ucCustomerAddress.Caption = "Funder";
                    rfvcmbCustomer.ErrorMessage = "Select a Funder";
                    lblCode.Text = "Funder Name";
                }
                else
                {
                    ucCustomerAddress.Caption = "Entity";
                    FunDisableGLSLCodeGrid(false);
                    FundisableGridValidation(true);
                }
            }
            else
            {
                FunProToggleCustomerAddress(false);
            }

            FunPriEnblDsblGrid();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void ddlFooterPrimeAccountNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProPANumSelectedIndexChange();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void ddlFooterFlowType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");
            Label lblFooterActualAmount = (Label)grvPaymentDetails.FooterRow.FindControl("lblFooterActualAmount");
            if (ddlFooterFlowType.SelectedIndex > 0)
            {
                if (ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue == "1")
                {
                    FunDisableGLSLCodeGrid(true);
                }
                else
                {
                    FunDisableGLSLCodeGrid(false);
                }
                ddlFooterGL_Code.SelectedIndex = -1;
                ddlFooterSL_Code.Clear();
                txtFooterDescription.Text = txtFooterAmount.Text = "";
                if (ddlFooterFlowType.SelectedValue == "202")
                {
                    if (ddlFooterSubAccountNumber.Items.Count > 1)
                    {
                        if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                        {
                            grvPaymentDetails.Columns[4].Visible = true;
                            FunPriLoadRefDocno();
                        }
                        else
                        {
                            ddlFooterFlowType.SelectedIndex = -1;
                            Utility.FunShowAlertMsg(this, "Select the sub account number");
                            return;
                        }
                    }
                    else
                    {
                        grvPaymentDetails.Columns[4].Visible = true;
                        FunPriLoadRefDocno();
                        //FunPriLoadDim2("Asset");
                    }
                }
                else if (ddlFooterFlowType.SelectedValue == "215")
                {
                    grvPaymentDetails.Columns[4].Visible = true;
                    UserControls_S3GAutoSuggest ddlPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                    ddlPrimeAccountNumber.IsMandatory = false;
                    FunPriLoadRefDocnoIVE();
                }
                else
                {
                    UserControls_S3GAutoSuggest ddlPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                    ddlPrimeAccountNumber.IsMandatory = true;
                    if (ddlFooterSubAccountNumber.Items.Count > 1)
                    {
                        if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                        {
                            if (ddlPayTo.SelectedValue == "11")//This is for insurance Payment 
                            {
                                FunPriloadGLSLCodeInsurance();
                            }
                            else // All other payments 
                            {
                                FunPriLoadSLcodes("PD", "");
                                FunPriLoadGLSLcodes();//payment details
                            }
                        }
                        else
                        {
                            ddlFooterFlowType.SelectedIndex = -1;
                            Utility.FunShowAlertMsg(this, "Select the sub account number");
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlPayTo.SelectedValue) == 13 && Convert.ToInt32(ddlPaymentType.SelectedValue) == 5)
                        {
                            if (Procparam != null)
                                Procparam.Clear();
                            else
                                Procparam = new Dictionary<string, string>();
                            Procparam.Add("@Option", "7");
                            Procparam.Add("@Company_Id", Convert.ToString(intCompanyID));
                            Procparam.Add("@Customer_ID", Convert.ToString(ddlReceiptFrom.SelectedValue));
                            Procparam.Add("@Funder_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                            Procparam.Add("@PA_SA_REF_ID", Convert.ToString(ddlPrimeAccountNumber.SelectedValue));
                            Procparam.Add("@CashFlow_ID", Convert.ToString(ddlFooterFlowType.SelectedValue));

                            DataTable dtCash = Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam);
                            if (dtCash.Rows.Count > 0)
                            {
                                ddlFooterGL_Code.Items.Add(Convert.ToString(dtCash.Rows[0]["GL_Code"]));
                                ddlFooterSL_Code.SelectedValue = Convert.ToString(dtCash.Rows[0]["SL_Code"]);
                                ddlFooterSL_Code.SelectedText = Convert.ToString(dtCash.Rows[0]["SL_Code"]);
                                txtFooterAmount.Text = lblFooterActualAmount.Text = Convert.ToString(dtCash.Rows[0]["Amount"]);
                            }
                            else
                            {
                                Utility.FunShowAlertMsg(this, "Receipt yet to be collect for this combination");
                                ddlFooterFlowType.SelectedValue = "0";
                                return;
                            }
                        }
                        else if (Convert.ToInt32(ddlPayTo.SelectedValue) == 13 && Convert.ToInt32(ddlPaymentType.SelectedValue) == 8)
                        {
                            if (Procparam != null)
                                Procparam.Clear();
                            else
                                Procparam = new Dictionary<string, string>();
                            Procparam.Add("@Option", "10");
                            Procparam.Add("@Company_Id", Convert.ToString(intCompanyID));
                            Procparam.Add("@Funder_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                            Procparam.Add("@Note_ID", Convert.ToString(ddlPrimeAccountNumber.SelectedValue));
                            Procparam.Add("@CashFlow_ID", Convert.ToString(ddlFooterFlowType.SelectedValue));

                            DataTable dtCash = Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam);
                            if (dtCash.Rows.Count > 0)
                            {
                                if (Convert.ToDouble(dtCash.Rows[0]["Amount"]) == 0)
                                {
                                    Utility.FunShowAlertMsg(this, "No Pendings available against this Pay Type");
                                    ddlFooterFlowType.SelectedValue = "0";
                                    return;
                                }
                                ddlFooterGL_Code.Items.Add(Convert.ToString(dtCash.Rows[0]["GL_Code"]));
                                ddlFooterGL_Code.SelectedValue = Convert.ToString(dtCash.Rows[0]["GL_Code"]);
                                ddlFooterSL_Code.SelectedValue = Convert.ToString(dtCash.Rows[0]["SL_Code"]);
                                ddlFooterSL_Code.SelectedText = Convert.ToString(dtCash.Rows[0]["SL_Code"]);
                                txtFooterAmount.Text = lblFooterActualAmount.Text = Convert.ToString(dtCash.Rows[0]["Amount"]);
                            }
                        }
                        else if (ddlPayTo.SelectedValue == "11")//This is for insurance Payment 
                        {
                            FunPriloadGLSLCodeInsurance();

                        }
                        else // All other payments 
                        {
                            FunPriLoadSLcodes("PD", "");
                            FunPriLoadGLSLcodes();//payment details
                        }
                    }
                }
            }
            else
            {
                ddlFooterGL_Code.SelectedIndex = -1;
                ddlFooterSL_Code.Clear();
                txtFooterDescription.Text = txtFooterAmount.Text = "";
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void ddlbankname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearBankDetails();
            FunPriLoadAcctNumbers();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void ddlNoteNo_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            UserControls_S3GAutoSuggest ddlfndrTrancheNo = (UserControls_S3GAutoSuggest)grvFunderReceipt.FooterRow.FindControl("ddlfndrTrancheNo");
            ddlfndrTrancheNo.Clear();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void ddlFndrCFDesc_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserControls_S3GAutoSuggest ddlFndrGLCode = (UserControls_S3GAutoSuggest)grvFunderReceipt.FooterRow.FindControl("ddlFndrGLCode");
        UserControls_S3GAutoSuggest ddlFndrSLCode = (UserControls_S3GAutoSuggest)grvFunderReceipt.FooterRow.FindControl("ddlFndrSLCode");
        DropDownList ddlFndrCFDesc = (DropDownList)grvFunderReceipt.FooterRow.FindControl("ddlFndrCFDesc");
        ddlFndrGLCode.Clear();
        ddlFndrSLCode.Clear();
        ddlFndrGLCode.Enabled = ddlFndrSLCode.Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) == 8) ? false : true;

        if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 8 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 5)
        {
            ddlFndrGLCode.Enabled = ddlFndrSLCode.Enabled = false;

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "18");
            Procparam.Add("@Funder_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            Procparam.Add("@CashFlow_ID", ddlFndrCFDesc.SelectedValue);

            DataTable dtFndrGL = Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam);
            if (dtFndrGL.Rows.Count > 0)
            {
                ddlFndrGLCode.SelectedText = ddlFndrGLCode.SelectedValue = Convert.ToString(dtFndrGL.Rows[0]["GL_Code"]);
                ddlFndrSLCode.SelectedText = ddlFndrSLCode.SelectedValue = Convert.ToString(dtFndrGL.Rows[0]["SL_Code"]);
            }
        }
    }

    protected void ddlInvoiceSortBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlInvSrchTxt.Clear();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    #endregion

    #region "Button Events"

    protected void btnCreateCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            FunGridBlank();
            FunPriEnblDsblGrid();
            if (ddlPayTo.SelectedIndex > 0)
            {
                HiddenField HdnId = ucCustomerCodeLov.FindControl("hdnID") as HiddenField;
                if (HdnId != null)
                {
                    ViewState["hdnCustorEntityID"] = HdnId.Value;
                }
                FunPriLoadCustomerEntityDtls(HdnId.Value);
                if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 5 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 8)
                {
                    ViewState["FunderPmtDtl"] = (DataTable)ViewState["DefaultFunderPmtDtl"];
                    FunPriBindFunderGrid((DataTable)ViewState["FunderPmtDtl"]);
                }
                else
                {
                    FunPriLoadGLcodes();
                }
                FunPriLoadCustomerBank();

                //Added on 20Apr2015 for Bug Fixing - Pay Type Not flow
                if (Convert.ToInt32(ddlPayTo.SelectedValue) > 0)
                {
                    FunPriLoadPaytypeingrid(Convert.ToString(ddlPayTo.SelectedValue));
                    FunPriLoadGLcodes();
                    if (ddlPayTo.SelectedValue == "1")
                    {
                        FunDisableGLSLCodeGrid(false);
                        FundisableGridValidation(true);
                    }
                    else if (ddlPayTo.SelectedValue == "11")
                    {
                        FunDisableGLSLCodeGrid(false);
                        FundisableGridValidation(true);
                    }
                    else if (ddlPayTo.SelectedValue == "12")  // Others
                    {
                        if (ddlLOB.SelectedItem.Text.Contains("FT"))
                        {
                            chkAccountBased.Enabled = true;
                        }
                        FunDisableGLSLCodeGrid(false);
                        FundisableGridValidation(true);
                    }
                    else
                    {
                        FunDisableGLSLCodeGrid(false);
                        FundisableGridValidation(true);
                    }
                }
                //End
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }

    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            txtInvoiceStartDate.Text = txtInvoiceEndDate.Text = txtInvSrchText.Text = "";
            ddlInvoiceSortBy.SelectedValue = "0";
            ddlInvSrchTxt.Clear();
            FunPriBindGridDtls(1, null);
            btnAddPOInvoice.Enabled = false;
            ucCustomPaging.Navigation(0, ProPageNumRW, ProPageSizeRW);
            moePoInvoiceDtls.Show();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void btnInvoiceGo_Click(object sender, EventArgs e)
    {
        try
        {
            bool blnRslt = true;
            if (Convert.ToString(txtInvoiceStartDate.Text) != "" && Convert.ToString(txtInvoiceEndDate.Text) != "")
            {
                Int32 intDiff = Convert.ToInt32((Utility.StringToDate(txtInvoiceEndDate.Text) - Utility.StringToDate(txtInvoiceStartDate.Text)).TotalDays);
                if (intDiff < 0)
                {
                    Utility.FunShowAlertMsg(this, "Invoice Start Date should be less than or equal to Invoice End Date");
                    blnRslt = false;
                }
            }

            if (Convert.ToInt32(ddlInvoiceSortBy.SelectedValue) > 0 && Convert.ToString(ddlInvSrchTxt.SelectedValue) == "")
            {
                Utility.FunShowAlertMsg(this, "Enter Search Text");
                blnRslt = false;
            }

            if (Convert.ToInt32(ddlInvoiceSortBy.SelectedValue) == 0 && Convert.ToString(txtInvSrchText.Text) != "")
            {
                Utility.FunShowAlertMsg(this, "Select the Search Type");
                blnRslt = false;
            }

            if (Convert.ToString(txtInvoiceStartDate.Text) != "" && Convert.ToString(txtInvoiceEndDate.Text) == "")
            {
                Utility.FunShowAlertMsg(this, "Enter Invoice End Date");
                blnRslt = false;
            }

            if (Convert.ToString(txtInvoiceEndDate.Text) != "" && Convert.ToString(txtInvoiceStartDate.Text) == "")
            {
                Utility.FunShowAlertMsg(this, "Enter Invoice Start Date");
                blnRslt = false;
            }

            if (blnRslt == false)
            {
                grvLoadInvoiceDtl.DataSource = null;
                grvLoadInvoiceDtl.DataBind();
                btnAddPOInvoice.Enabled = false;
                return;
            }
            blnChkSlctAll = false;
            ProPageNumRW = 1;
            FunPriBindGrid();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void btnAddPOInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            btnremoveall.Visible = true;
            lblPagingErrorMessage.InnerText = "";
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = ViewState["TotRec"] == null ? intTotalRecords : Convert.ToInt32(ViewState["TotRec"]);
            ObjPaging.ProCurrentPage = ProPageNumRW1;
            ObjPaging.ProPageSize = ProPageSizeRW1;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            FunPriGetSearchValue();



            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", Convert.ToString(4));
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));
            FunPriAddCmnParam();
            Procparam.Add("@Is_Added", "1");    //Added on 15Jun2015 for OPC Req

            Procparam.Add("@CurrentPage", ProPageNumRW1.ToString());
            Procparam.Add("@PageSize", ProPageSizeRW1.ToString());
            Procparam.Add("@SearchValue", hdnSearch.Value);
            Procparam.Add("@OrderBy", hdnOrderBy.Value);
            Procparam.Add("@TotalRecords", ViewState["TotRec"] == null ? intTotalRecords.ToString() : Convert.ToInt32(ViewState["TotRec"]).ToString());
            DataSet dsInvoice = Utility.GetDataset("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);
            ViewState["TotRec"] = dsInvoice.Tables[3].Rows[0]["@TotalRecords"].ToString();
            if (dsInvoice.Tables[0].Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast one Invoice for payment");
                lblPaymentDetailsTotal.Text = lbltotalPaymentAdjust.Text = "0";
                txtDocAmount.Text = Math.Round(Convert.ToDecimal(lbltotalPaymentAdjust.Text), 0).ToString();
                return;
            }

            ViewState["PaymentInvoiceDetails"] = dsInvoice.Tables[0];

            FunPriBindGridDtls(2, dsInvoice.Tables[0]);

            ViewState["grvPaymentDetails"] = dsInvoice.Tables[1];
            FunPriBindGridDtls(3, dsInvoice.Tables[1]);
            FunPriDisplaySmryGridTotal(dsInvoice.Tables[2]);
            grvPaymentDetails.FooterRow.Visible = grvPaymentDetails.Columns[9].Visible = false;

            moePoInvoiceDtls.Hide();

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
            // this is to save the payment request.
            #region Save Payment request

            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 5 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 8)
            {
                if (ViewState["FunderPmtDtl"] == null || Convert.ToInt64(((DataTable)ViewState["FunderPmtDtl"]).Rows[0]["Note_ID"]) == 0)
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one Payment details");
                    return;
                }
                if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 5)
                {
                    if (FunPriCheckGLSLSum() == false)
                    {
                        Utility.FunShowAlertMsg(this, "Payment Amount should not exceed Received Amount");
                        return;
                    }
                }
            }
            else
            {
                DataTable dtPmtDtls = new DataTable();

                if (ViewState["grvPaymentDetails"] != null)
                {
                    dtPmtDtls = (DataTable)ViewState["grvPaymentDetails"];
                }
                if (dtPmtDtls.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one Payment details");
                    return;
                }
                else if (Convert.ToString(dtPmtDtls.Rows[0]["PANum"]) == "-1" && (dtPmtDtls.Rows.Count == 1))
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one Payment details");
                    return;
                }
                // To check payment should be greater than 0.
                if (Convert.ToDecimal(lbltotalPaymentAdjust.Text) <= 0)
                {
                    Utility.FunShowAlertMsg(this, "Payment Total amount should be greater than zero");
                    return;
                }
            }
            if (FuncalculateDocAmount())
            {
                string strbMissedValues = string.Empty;

                LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDataTable PaymentRequestDataTable = new LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDataTable();
                LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestRow PaymentRequestDataRow = PaymentRequestDataTable.NewS3G_LOANAD_PaymentRequestRow();

                PaymentRequestDataRow.Company_ID = intCompanyID;
                PaymentRequestDataRow.LOB_ID =
                    (ddlLOB != null && Convert.ToInt32(ddlLOB.SelectedValue) > 0) ?
                        Convert.ToInt32(ddlLOB.SelectedValue) : -1;

                PaymentRequestDataRow.Branch_ID =
                    (ddlBranch != null && Convert.ToInt32(ddlBranch.SelectedValue) > 0) ?
                        Convert.ToInt32(ddlBranch.SelectedValue) : -1;


                PaymentRequestDataRow.Payment_Request_No = string.Empty;// to send Null - in create mode
                PaymentRequestDataRow.Request_No = -1;//Primary Key
                PaymentRequestDataRow.Payment_Request_Date = Utility.StringToDate(txtPaymentRequestDate.Text);
                PaymentRequestDataRow.Value_Date = Utility.StringToDate(txtPaymentRequestDate.Text);
                PaymentRequestDataRow.Pay_Mode_Code =
                   (ddlPayMode != null && ddlPayMode.SelectedIndex > 0) ?
                       Convert.ToInt32(ddlPayMode.SelectedValue) : -1;

                PaymentRequestDataRow.Currency_ID =
                   (ddlCurrencyCode != null && ddlCurrencyCode.SelectedIndex > 0) ?
                       Convert.ToInt32(ddlCurrencyCode.SelectedValue) : -1;

                if (strQsMode == "C")
                {
                    PaymentRequestDataRow.Mode = 0;
                    PaymentRequestDataRow.Requestno = 0;
                }
                else if (PageMode == PageModes.WorkFlow)
                {
                    if (!string.IsNullOrEmpty(strRequestID))
                    {
                        if (Convert.ToInt32(strRequestID) > 0)
                        {
                            PaymentRequestDataRow.Mode = 1;
                            PaymentRequestDataRow.Requestno = Convert.ToInt64(strRequestID);
                        }
                    }
                    else
                    {
                        PaymentRequestDataRow.Mode = 0;
                        PaymentRequestDataRow.Requestno = 0;
                    }
                }
                else
                {
                    PaymentRequestDataRow.Mode = 1;
                    PaymentRequestDataRow.Requestno = Convert.ToInt64(strRequestID);
                }

                PaymentRequestDataRow.Exchange_Rate_ID = -1;

                PaymentRequestDataRow.Pay_Amount = Convert.ToDecimal(txtDocAmount.Text);

                if (ddlPayTo != null && ddlPayTo.SelectedIndex > 0)
                {
                    PaymentRequestDataRow.Pay_To_Type_Code = Convert.ToInt32(ddlPayTo.SelectedValue);
                    PaymentRequestDataRow.Pay_To_Code = Convert.ToInt32(ddlPayTo.SelectedValue);
                }
                else
                {
                    PaymentRequestDataRow.Pay_To_Type_Code = -1;
                    PaymentRequestDataRow.Pay_To_Code = -1;
                }
                if (ddlPayTo.SelectedValue != "50" && ddlPayTo.SelectedValue != "1" && ddlPayTo.SelectedValue != "13")
                {
                    if (ViewState["hdnCustorEntityID"] != null && ViewState["hdnCustorEntityID"].ToString() != string.Empty)
                    {
                        PaymentRequestDataRow.Vendor_Code = Convert.ToInt64(ViewState["hdnCustorEntityID"].ToString());
                    }
                    else
                    {
                        PaymentRequestDataRow.Vendor_Code = -1;
                    }
                }
                else
                    PaymentRequestDataRow.Vendor_Code = -1;
                if (ddlPayTo.SelectedValue == "1")
                {
                    PaymentRequestDataRow.Customer_ID = Convert.ToInt64(ViewState["hdnCustorEntityID"].ToString());
                }
                else
                {
                    PaymentRequestDataRow.Customer_ID = -1;
                }
                if (Convert.ToInt32(ddlPayTo.SelectedValue) == 13)
                {
                    PaymentRequestDataRow.Funder_ID = Convert.ToInt64(ViewState["hdnCustorEntityID"].ToString());
                }
                if (ddlPayTo.SelectedValue == "50")
                {
                    PaymentRequestDataRow.Pay_To_Name = "General";
                    PaymentRequestDataRow.Pay_To_Address = "General";
                }
                else
                {
                    PaymentRequestDataRow.Pay_To_Name = ucCustomerAddress.CustomerName;
                    PaymentRequestDataRow.Pay_To_Address = ucCustomerAddress.CustomerAddress;
                }

                PaymentRequestDataRow.Pmt_Voucher_status = false;  // check this

                PaymentRequestDataRow.Cancelled_Date = Convert.ToDateTime("1/1/1700"); // will handle in SP
                if (ViewState["IsVoucher_Print"] != null)
                {
                    PaymentRequestDataRow.IsVoucher_Print = (string)ViewState["IsVoucher_Print"];
                }
                else
                {
                    PaymentRequestDataRow.IsVoucher_Print = "N";
                }

                if (ViewState["IsCheque_Print"] != null)
                {
                    PaymentRequestDataRow.IsCheque_Print = (string)ViewState["IsCheque_Print"];
                }

                else
                {
                    PaymentRequestDataRow.IsCheque_Print = "N";
                }
                if (ddlPayeeBankAccount.Items.Count > 0)
                {
                    PaymentRequestDataRow.Payee_Bank_ID = Convert.ToInt32(ddlPayeeBankAccount.SelectedValue);
                }
                else
                {
                    PaymentRequestDataRow.Payee_Bank_ID = 0;
                }
                PaymentRequestDataRow.Account_Based = chkAccountBased.SelectedValue;
                PaymentRequestDataRow.Created_By = intUserID;
                PaymentRequestDataRow.Created_On = Convert.ToDateTime("1/1/1700"); // will handle in SP
                PaymentRequestDataRow.Modified_By = -1; // Since it is insert mode we dont want to handle here
                PaymentRequestDataRow.Modified_Date = Convert.ToDateTime("1/1/1700"); // will handle in SP
                PaymentRequestDataRow.TXN_ID = -1; // 
                grvPaymentDetails.Columns[3].HeaderText = "Cashflow ID";

                if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 5 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 8)
                {
                    PaymentRequestDataRow.XML_FunderPmtDtl = Utility.FunPubFormXml((DataTable)ViewState["FunderPmtDtl"], true);
                }
                else
                {
                    if (grvPaymentDetails.Rows.Count > 0)
                    {
                        PaymentRequestDataRow.XML_PaymentDetails = grvPaymentDetails.FunPubFormXml();
                    }
                    else
                        PaymentRequestDataRow.XML_PaymentDetails = "<Root></Root>";
                }
                if (ViewState["grvPaymentAdjust"] != null)
                {
                    if (((DataTable)ViewState["grvPaymentAdjust"]).Rows.Count > 0 && ((DataTable)ViewState["grvPaymentAdjust"]).Rows[0]["AddOrLess"].ToString() != "")
                    {
                        PaymentRequestDataRow.XML_PaymentAdjustment = grvPaymentAdjustment.FunPubFormXml();
                    }
                    else
                    {
                        PaymentRequestDataRow.XML_PaymentAdjustment = "<Root></Root>";
                    }
                }
                else
                {
                    PaymentRequestDataRow.XML_PaymentAdjustment = "<Root></Root>";
                }

                //Observation raised by Sudharsan. updated dated on 14-Nov-2011. by saran
                if (ddlAcctNumber.SelectedIndex >= 0)
                    PaymentRequestDataRow.Bank_ID = Convert.ToInt32(ddlAcctNumber.SelectedValue);
                else
                    PaymentRequestDataRow.Bank_ID = -1;
                if (ddlPayMode.SelectedValue == "1")
                {
                    PaymentRequestDataRow.Instrument_Type = "C";

                }
                else if (ddlPayMode.SelectedValue == "2")
                {
                    PaymentRequestDataRow.Instrument_Type = "D";

                }
                if (txtInstrumentNumber.Text != string.Empty)
                    PaymentRequestDataRow.Instrument_No = txtInstrumentNumber.Text;

                PaymentRequestDataRow.Instrument_Status = true;
                if (txtInstrumentDate.Text != string.Empty)
                    PaymentRequestDataRow.Instrument_Date = Utility.StringToDate(txtInstrumentDate.Text).ToString();

                if (txtRemarks.Text != string.Empty)
                    PaymentRequestDataRow.Remarks = txtRemarks.Text;

                PaymentRequestDataRow.PaymentGateWayRefNo = Convert.ToString(txtPmtGatewayRefNo.Text);
                PaymentRequestDataRow.Favouring_Name = Convert.ToString(txtFavouringName.Text);

                if (ddlPayMode.SelectedValue == "1" && Convert.ToString(ViewState["IsCheque_Print"]) != "P")
                    PaymentRequestDataRow.Is_Update_Req = 1;
                else
                    PaymentRequestDataRow.Is_Update_Req = 0;

                //update if it is cheque or dd
                if (ddlPayMode.SelectedValue == "1" || ddlPayMode.SelectedValue == "2")
                {
                    PaymentRequestDataRow.Is_Instrument = 1;
                }
                else
                {
                    PaymentRequestDataRow.Is_Instrument = 0;
                }
                PaymentRequestDataRow.Pay_Type_Code = Convert.ToInt32(ddlPaymentType.SelectedValue);
                PaymentRequestDataRow.Payment_From = Convert.ToInt64(ddlReceiptFrom.SelectedValue);
                PaymentRequestDataRow.Tranche_ID = Convert.ToInt64(ddlTranche.SelectedValue);
                if (grvPaymentInvoiceDtl != null && grvPaymentInvoiceDtl.Rows.Count > 0)
                {
                    PaymentRequestDataRow.XML_POInvoiceDtls = grvPaymentInvoiceDtl.FunPubFormXml(true);
                }

                PaymentRequestDataTable.AddS3G_LOANAD_PaymentRequestRow(PaymentRequestDataRow);

                ObjLoanAdminAccMgtServicesClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
                SerializationMode SMode = SerializationMode.Binary;

                string paynum = string.Empty;
                int request_no = 0;
                int errCode = ObjLoanAdminAccMgtServicesClient.FunPubCreateOrModifyPaymentRequest(out paynum, out request_no, SMode, ClsPubSerialize.Serialize(PaymentRequestDataTable, SMode));
                //int errCode =99;

                if (errCode == 1 || errCode == 2 || errCode == 0)
                {
                    // this is to save the payment request details.
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, paynum, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";

                        }
                        catch (Exception ex)
                        {
                            strAlert = " Work Flow is not Assigned";
                        }
                        ShowWFAlertMessage(paynum, ProgramCode, strAlert);
                        return;
                    }
                    else if (strQsMode == "C")
                    {

                        // FORCE PULL IMPLEMENTATION KR
                        DataTable WFFP = new DataTable();

                        if (CheckForForcePullOperation(ProgramCode, "", null, "L", CompanyId, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), "", "", int.Parse(dtrForce["PRODUCTID"].ToString()));
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        strAlert = "Payment Request \"" + paynum + "\"  saved successfully";
                        strAlert += @"\n\nWould you like to raise one more payment request?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Payment Request Number " + paynum + " generated successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PARE';", true);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Payment Request Modified successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PARE';", true);
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                        return;
                    }
                }
                else if (errCode == 90)
                {
                    Utility.FunShowAlertMsg(this, "Document sequence number not defined for payment request");
                }
                else if (errCode == 91)
                {
                    Utility.FunShowAlertMsg(this, "Document sequence number exceeded for payment request");
                }
                else if (errCode == 99)
                {
                    Utility.FunShowAlertMsg(this, "Calling");
                }
                else if (errCode == 100)
                {
                    Utility.FunShowAlertMsg(this, "Payment Date should be in Open Month");
                }
                else if (errCode == 101)
                {
                    Utility.FunShowAlertMsg(this, "Payment Date should not be Less than Latest Payment Date ");
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Unable to generate Payment");
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Payment Total amount should be equal to Document Amount");
            }

            #endregion
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
        finally
        {
            if (ObjLoanAdminAccMgtServicesClient != null)
                ObjLoanAdminAccMgtServicesClient.Close();
        }
    }

    protected void btnFooterFunderAdd_Click(object sender, EventArgs e)
    {
        try
        {
            UserControls_S3GAutoSuggest ddlNoteNo = (UserControls_S3GAutoSuggest)grvFunderReceipt.FooterRow.FindControl("ddlNoteNo");
            UserControls_S3GAutoSuggest ddlfndrTrancheNo = (UserControls_S3GAutoSuggest)grvFunderReceipt.FooterRow.FindControl("ddlfndrTrancheNo");
            UserControls_S3GAutoSuggest ddlFndrGLCode = (UserControls_S3GAutoSuggest)grvFunderReceipt.FooterRow.FindControl("ddlFndrGLCode");
            UserControls_S3GAutoSuggest ddlFndrSLCode = (UserControls_S3GAutoSuggest)grvFunderReceipt.FooterRow.FindControl("ddlFndrSLCode");
            DropDownList ddlFndrCFDesc = (DropDownList)grvFunderReceipt.FooterRow.FindControl("ddlFndrCFDesc");
            TextBox txtFooterFunderAmount = (TextBox)grvFunderReceipt.FooterRow.FindControl("txtFooterFunderAmount");
            Label lblFooterFndrCFFlagID = (Label)grvFunderReceipt.FooterRow.FindControl("lblFooterFndrCFFlagID");

            DataTable paydt = (DataTable)ViewState["grvPaymentDetails"];
            if (paydt.Rows[0]["PANUM"].ToString() == "-1")
            {
                ViewState["grvPaymentDetails"] = null; // To remove the payment empty grid when its funder payment
            }

            if (Convert.ToString(ddlFndrCFDesc.SelectedValue) == "")
            {
                Utility.FunShowAlertMsg(this, "Define cashflow master for Funder PV Amount against Payment Request");
                return;
            }

            DataTable dtFndrRcpt = (ViewState["FunderPmtDtl"] != null) ? (DataTable)ViewState["FunderPmtDtl"] : (DataTable)ViewState["DefaultFunderPmtDtl"];
            if (dtFndrRcpt != null && dtFndrRcpt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtFndrRcpt.Rows[0]["Note_ID"]) == 0)
                {
                    dtFndrRcpt.Rows[0].Delete();
                    dtFndrRcpt.AcceptChanges();
                }
            }

            if (dtFndrRcpt.Rows.Count > 0)
            {
                string strFilter = "Note_ID = " + Convert.ToInt32(ddlNoteNo.SelectedValue);
                strFilter = strFilter + " and Tranche_ID = 0";
                DataRow[] drDuplicate = dtFndrRcpt.Select(strFilter);

                if (drDuplicate.Length > 0)
                {
                    Utility.FunShowAlertMsg(this, "Entered combinatin already exists");
                    return;
                }

                if (Convert.ToInt32(ddlfndrTrancheNo.SelectedValue) == 0)
                {
                    strFilter = "Note_ID = " + Convert.ToInt32(ddlNoteNo.SelectedValue);
                    drDuplicate = dtFndrRcpt.Select(strFilter);

                    if (drDuplicate.Length > 0)
                    {
                        Utility.FunShowAlertMsg(this, "Entered combinatin already exists");
                        return;
                    }
                }

                strFilter = "Note_ID = " + Convert.ToInt32(ddlNoteNo.SelectedValue);
                strFilter = strFilter + " and Tranche_ID = " + Convert.ToInt32(ddlfndrTrancheNo.SelectedValue);
                drDuplicate = dtFndrRcpt.Select(strFilter);

                if (drDuplicate.Length > 0)
                {
                    Utility.FunShowAlertMsg(this, "Entered combinatin already exists");
                    return;
                }
            }

            DataRow drFndr = dtFndrRcpt.NewRow();

            drFndr["Note_ID"] = Convert.ToInt32(ddlNoteNo.SelectedValue);
            drFndr["Note_No"] = Convert.ToString(ddlNoteNo.SelectedText);
            drFndr["Tranche_ID"] = Convert.ToInt32(ddlfndrTrancheNo.SelectedValue);
            drFndr["Tranche_Name"] = Convert.ToString(ddlfndrTrancheNo.SelectedText);
            drFndr["CashFlow_ID"] = Convert.ToInt32(ddlFndrCFDesc.SelectedValue);
            drFndr["CashFlow_Desc"] = Convert.ToString(ddlFndrCFDesc.SelectedItem.Text);
            drFndr["GL_Code_Desc"] = Convert.ToString(ddlFndrGLCode.SelectedValue);

            drFndr["SL_Code_Desc"] = Convert.ToString(ddlFndrSLCode.SelectedValue);
            drFndr["CashFlow_Flag_ID"] = Convert.ToInt32(lblFooterFndrCFFlagID.Text);
            drFndr["Amount"] = (Convert.ToString(txtFooterFunderAmount.Text) == "") ? 0 : Convert.ToDouble(txtFooterFunderAmount.Text);
            //drInstallment["IsAddLessExist"] = false;
            dtFndrRcpt.Rows.Add(drFndr);

            ViewState["FunderPmtDtl"] = dtFndrRcpt;
            FunPriBindFunderGrid(dtFndrRcpt);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnCoveringLetter_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriDecimalCommaSeperator(100000);
            String htmlText = FunPriGenerateCoveringPDF();
            string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + txtPaymentRequestNo.Text.Replace('/', '_') + "_CoveringLetter" + ".pdf");
            string strFileName = "/LoanAdmin/PDF Files/" + txtPaymentRequestNo.Text.Replace("/", "_").Replace(" ", "").Replace(":", "") + "_CoveringLetter" + ".pdf";
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));
            doc.AddCreator("Sundaram Infotech Solutions Limited");
            doc.AddTitle("Covering Letter_" + txtPaymentRequestNo.Text.Replace('/', '_'));
            doc.Open();
            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null).Cast<IElement>().ToList();
            for (int k = 0; k < htmlarraylist.Count; k++)
            { doc.Add((IElement)htmlarraylist[k]); }
            doc.AddAuthor("S3G Team");
            doc.Close();
            //System.Diagnostics.Process.Start(strnewFile);
            string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (DirectoryNotFoundException dr)
        {
            Utility.FunShowAlertMsg(this, "The Target Directory was not found in the server to generate the PDF file");
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, " Error in creating a PDF file");
        }
    }

    protected void btnPrintVoucher_Click(object sender, EventArgs e)
    {
        try
        {
            FunOpenPDF();

            //if (ddlPayMode.SelectedValue == "2")
            //{
            //    /*if ((!(Convert.ToInt32(ddlbankname.SelectedValue) > 0)) && (RBLCompanyCashorBankAcct.SelectedValue == "1"))
            //    {
            //        Utility.FunShowAlertMsg(this, "Select the Bank Name");
            //        return;
            //    }
            //    else if ((txtGLCode.Text == "" || txtGLCode.Text == string.Empty) && (RBLCompanyCashorBankAcct.SelectedValue == "1"))
            //    {
            //        Utility.FunShowAlertMsg(this, "Enter the GL Code");
            //        return;
            //    }
            //    //else if ((txtSLCode.Text == "" || txtSLCode.Text == string.Empty) && (RBLCompanyCashorBankAcct.SelectedValue == "1"))
            //    //{
            //    //    Utility.FunShowAlertMsg(this, "Enter the SL Code");
            //    //    return;
            //    //}
            //    else if (txtInstrumentNumber.Text == "" || txtInstrumentNumber.Text == string.Empty)
            //    {
            //        Utility.FunShowAlertMsg(this, "Enter the Instrument Number");
            //        return;
            //    }
            //    else if (txtInstrumentDate.Text == "" || txtInstrumentDate.Text == string.Empty)
            //    {
            //        Utility.FunShowAlertMsg(this, "Select the Instrument Date");
            //        return;
            //    }*/
            //}
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnPrintCheque_Click(object sender, EventArgs e)
    {
        //string s =Utility.FunPubSetCommaSeperator(txtRemarks.Text, "INR");
        //Utility.FunShowAlertMsg(this,s);
        ////  txtRemarks.Text = string.Format(System.Globalization.CultureInfo.GetCultureInfo("ta-IN").NumberFormat,
        ////"{0:c}", 4454544.5554);
        //return;

        try
        {
            switch ((string)ViewState["IsCheque_Print"])
            {
                case "N":
                    ViewState["IsCheque_Print"] = "P";

                    //if (FunPriInsertInstrumentdetails() == 0)
                    //{
                    FunPriGeneratePdfVoucher("Payment Request - Cheque", "Cheque");
                    FunPriUpdateStatus();
                    FunPriLoadBankdetailsPmtReq();
                    btnPrintCheque.Text = "Reprint Cheque";
                    //btnCoveringLetter.Enabled = true;
                    FunPriSetRemarksMandatory(true);
                    if (ddlbankname.Items.Count > 0)
                        ddlbankname.ClearDropDownList();
                    if (ddlAcctNumber.Items.Count > 0)
                        ddlAcctNumber.ClearDropDownList();
                    //Utility.FunShowAlertMsg(this, "Cheque was printed successfully");
                    //}
                    //else
                    //{
                    //    ViewState["IsCheque_Print"] = "N";
                    //    Utility.FunShowAlertMsg(this, "Error in generating Cheque");

                    //}

                    break;
                case "P":
                case "D":

                    ViewState["IsCheque_Print"] = "D";
                    if (FunPriInsertInstrumentdetails() == 0)
                    {
                        FunPriGeneratePdfVoucher("Payment Request - Cheque", "Cheque");
                        FunPriUpdateStatus();
                        FunPriLoadBankdetailsPmtReq();
                        //Utility.FunShowAlertMsg(this, " Cheque was Reprinted successfully");
                    }
                    else
                    {
                        ViewState["IsCheque_Print"] = "D";
                        Utility.FunShowAlertMsg(this, "Error in Regenerating Cheque");

                    }
                    break;

                //FunPriGeneratePdfVoucher("Payment Request - Cheque (Duplicate Copy)");
                //Utility.FunShowAlertMsg(this, "Duplicate copy of the Cheque was printed successfully");
                //break;

            }
            FunPriToSetChequeStatus(Convert.ToString(ViewState["IsCheque_Print"]));
            //FunPriGeneratePdfVoucher("Payment Request - Cheque", "Cheque");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "Image Events"

    protected void imgPopupClose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moePoInvoiceDtls.Hide();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void imgbtnDeleteInvoice_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string strSelectID = ((ImageButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvPaymentInvoiceDtl", strSelectID);
            Label lblPoDtlID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPoID");
            Label lblgdinvPIID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPIID");
            Label lblgdinvVIID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvVIID");

            if (lblgdinvPIID.Text == "")
                lblgdinvPIID.Text = "0";
            if (lblgdinvVIID.Text == "")
                lblgdinvVIID.Text = "0";

            FunPriInsertDeleteInvoiceDtl(2, "", Convert.ToInt64(lblPoDtlID.Text), Convert.ToInt64(lblgdinvPIID.Text), Convert.ToInt64(lblgdinvVIID.Text));
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    #endregion

    #region "Checkbox Events"

    protected void chkSelectIndicator_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((CheckBox)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvLoadInvoiceDtl", strSelectID);
            Label lblPoDtlID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPoID");
            Label lblgdinvPIID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPIID");
            Label lblgdinvVIID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblVendorInvoiceID");
            CheckBox chkSelectIndicator = (CheckBox)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("chkSelectIndicator");
            TextBox txtPayableAmount = (TextBox)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("txtPayableAmount");
            Label lbTotalAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblTotalAmount");
            Label lblHoldAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblHoldAmount");
            Label lblPaidAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPaidAmount");
            Label lblAdvanceAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblAdvanceAmount");
            double dblBalance;

            if (lblgdinvPIID.Text == "")
                lblgdinvPIID.Text = "0";
            if (lblgdinvVIID.Text == "")
                lblgdinvVIID.Text = "0";

            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1)
            {
                dblBalance = Convert.ToDouble(lbTotalAmount.Text) - Convert.ToDouble(lblPaidAmount.Text);
            }
            else
            {
                dblBalance = Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblPaidAmount.Text);
            }

            if (chkSelectIndicator.Checked == true && Convert.ToDouble(txtPayableAmount.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "Payable Amount should be Greater than 0");
                chkSelectIndicator.Checked = false;
                return;
            }


            if (Convert.ToDouble(txtPayableAmount.Text) > dblBalance)
            {
                Utility.FunShowAlertMsg(this, "Amount to be disbursed should be less than or equal to Balance Amount");
                txtPayableAmount.Text = Convert.ToString(dblBalance);
            }

            DataTable dtInvoiceDtl = (DataTable)ViewState["POInvoiceDetails"];
            DataView dvInvoice = new DataView(dtInvoiceDtl);
            dvInvoice.RowFilter = "PO_Det_ID = " + Convert.ToInt64(lblPoDtlID.Text);
            dtInvoiceDtl = dvInvoice.ToTable();

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", (chkSelectIndicator.Checked == true) ? "1" : "2");     //1 - Insert  2 - Delete
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));

            if (chkSelectIndicator.Checked == true)
            {
                Procparam.Add("@XML_InvoiceDetails", Utility.FunPubFormXml(dtInvoiceDtl, true));
                FunPriAddCmnParam();
            }
            else
            {
                Procparam.Add("@Po_Det_ID", Convert.ToString(lblPoDtlID.Text));
            }

            Procparam.Add("@PI_Det_ID", Convert.ToString(lblgdinvPIID.Text));
            Procparam.Add("@VI_Det_ID", Convert.ToString(lblgdinvVIID.Text));

            Procparam.Add("@Is_Added", "0");    //Added on 15Jun2015 for OPC Req

            DataSet dsInvoice = Utility.GetDataset("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);

            if (dsInvoice != null && dsInvoice.Tables.Count > 1)
                FunPriDisplayGridTotal(dsInvoice.Tables[2]);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void chkSelectAllInvoices_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSelectAll = (CheckBox)grvLoadInvoiceDtl.HeaderRow.FindControl("chkSelectAllInvoices");
            DataTable dtInvoiceDtl = (DataTable)ViewState["POInvoiceDetails"];
            blnChkSlctAll = chkSelectAll.Checked;

            if (chkSelectAll.Checked == false)
            {
                DataView dvInv = new DataView(dtInvoiceDtl);
                dtInvoiceDtl = dvInv.ToTable(true, "PO_DET_ID");
                dtInvoiceDtl.AcceptChanges();
            }

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", (chkSelectAll.Checked == true) ? "1" : "5");     //1 - Insert  2 - Delete
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));
            Procparam.Add("@XML_InvoiceDetails", Utility.FunPubFormXml(dtInvoiceDtl, true));
            FunPriAddCmnParam();

            Procparam.Add("@Is_Added", "0");    //Added on 15Jun2015 for OPC Req

            DataSet dsInvoice = Utility.GetDataset("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);
            //FunPriBindGrid();
            FunPriBindGrid1();

            // Commented By C.Aswinkrishna on 9-Feb-2015 Start //

            //if (dsInvoice != null && dsInvoice.Tables.Count > 1)
            //    FunPriDisplayGridTotal(dsInvoice.Tables[2]);

            // Commented By C.Aswinkrishna on 9-Feb-2015 End //
        }
        catch (Exception ObjException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ObjException);
        }
    }

    #endregion

    #region "TextBox Events"

    protected void txtPayableAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((TextBox)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvLoadInvoiceDtl", strSelectID);
            Label lblPoDtlID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPoID");
            Label lblgdinvPIID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPIID");
            Label lblgdinvVIID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblVendorInvoiceID");
            CheckBox chkSelectIndicator = (CheckBox)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("chkSelectIndicator");
            TextBox txtPayableAmount = (TextBox)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("txtPayableAmount");
            Label lblHoldAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblHoldAmount");
            TextBox txtRetentionAmt = (TextBox)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("txtRetentionAmt");
            Label lblPaidAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPaidAmount");
            Label lbTotalAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblTotalAmount");
            Label lblAdvanceAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblAdvanceAmount");
            Label lblAdvancePaidAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblAdvancePaidAmount");
            double dblBalance;

            if (lblgdinvPIID.Text == "")
                lblgdinvPIID.Text = "0";
            if (lblgdinvVIID.Text == "")
                lblgdinvVIID.Text = "0";

            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1)
            {
                dblBalance = Convert.ToDouble(lbTotalAmount.Text) - (Convert.ToDouble(txtRetentionAmt.Text) + Convert.ToDouble(lblPaidAmount.Text));
                dblBalance = dblBalance - (Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblAdvancePaidAmount.Text));
            }
            else
            {
                dblBalance = Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblPaidAmount.Text);
            }

            if (Convert.ToString(txtPayableAmount.Text) == "")
            {
                Utility.FunShowAlertMsg(this, "Amount to be Disbursed should not be blank");
                txtPayableAmount.Text = Convert.ToString(dblBalance);
            }

            if (Convert.ToDouble(txtPayableAmount.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "Amount to be Disbursed should be greater than 0");
                txtPayableAmount.Text = Convert.ToString(dblBalance);
            }

            if (Convert.ToDouble(txtPayableAmount.Text) > dblBalance)
            {
                Utility.FunShowAlertMsg(this, "Amount to be disbursed should be less than or equal to Balance Amount");
                txtPayableAmount.Text = Convert.ToString(dblBalance);
            }

            if (lblgdinvPIID.Text == "")
                lblgdinvPIID.Text = "0";
            if (lblgdinvVIID.Text == "")
                lblgdinvVIID.Text = "0";
            if (chkSelectIndicator.Checked == true)
            {
                DataSet dsInvoice = FunPriUpdateInvDtl(Convert.ToString(lblPoDtlID.Text), Convert.ToDouble(txtPayableAmount.Text), Convert.ToDouble(txtRetentionAmt.Text), 0, Convert.ToString(lblgdinvPIID.Text), Convert.ToString(lblgdinvVIID.Text));
                if (dsInvoice.Tables.Count > 1)
                    FunPriDisplayGridTotal(dsInvoice.Tables[2]);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void txtgdinvPayableAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((TextBox)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvPaymentInvoiceDtl", strSelectID);
            Label lblPoDtlID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPoID");
            Label lblgdinvPIID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPIID");
            Label lblgdinvVIID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvVIID");
            Label lblReceiptAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvHoldAmount");
            Label lblPaidAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPaidAmount");
            TextBox txtgdinvPayableAmount = (TextBox)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("txtgdinvPayableAmount");
            Label lblgdinvPayableAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPayableAmount");
            TextBox txtgdinvRetentionAmt = (TextBox)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("txtgdinvRetentionAmt");
            Label lbTotalAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvTotalAmount");
            Label lblAdvanceAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvAdvanceAmount");
            Label lblAdvancePaidAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvAdvancePaidAmount");
            TextBox txtRetentionAmt = (TextBox)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("txtgdinvRetentionAmt");
            double dblBalance;

            if (lblgdinvPIID.Text == "")
                lblgdinvPIID.Text = "0";
            if (lblgdinvVIID.Text == "")
                lblgdinvVIID.Text = "0";

            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1)
            {
                dblBalance = Convert.ToDouble(lbTotalAmount.Text) - (Convert.ToDouble(txtRetentionAmt.Text) + Convert.ToDouble(lblPaidAmount.Text));
                dblBalance = dblBalance - (Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblAdvancePaidAmount.Text));
            }
            else
            {
                dblBalance = Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblPaidAmount.Text);
            }
            if (Convert.ToString(txtgdinvPayableAmount.Text) == "" || Convert.ToDouble(txtgdinvPayableAmount.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "Payable Amount should not be blank and 0");
                txtgdinvPayableAmount.Text = Convert.ToString(lblgdinvPayableAmount.Text);
                return;
            }
            if (Convert.ToDouble(txtgdinvPayableAmount.Text) > dblBalance)
            {
                Utility.FunShowAlertMsg(this, "Payable Amount should not be Greater than Balance Amount");
                txtgdinvPayableAmount.Text = Convert.ToString(lblgdinvPayableAmount.Text);
                return;
            }

            DataSet dsInvoice = FunPriUpdateInvDtl(Convert.ToString(lblPoDtlID.Text), Convert.ToDouble(txtgdinvPayableAmount.Text), Convert.ToDouble(txtgdinvRetentionAmt.Text), 1, Convert.ToString(lblgdinvPIID.Text), Convert.ToString(lblgdinvVIID.Text));
            ViewState["PaymentInvoiceDetails"] = dsInvoice.Tables[0];
            FunPriBindGridDtls(2, dsInvoice.Tables[0]);

            ViewState["grvPaymentDetails"] = dsInvoice.Tables[1];
            FunPriBindGridDtls(3, dsInvoice.Tables[1]);

            if (dsInvoice.Tables.Count > 1)
                FunPriDisplaySmryGridTotal(dsInvoice.Tables[2]);

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void txtRetentionAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((TextBox)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvLoadInvoiceDtl", strSelectID);
            Label lblPoDtlID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPoID");
            Label lblgdinvPIID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPIID");
            Label lblgdinvVIID = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblVendorInvoiceID");
            CheckBox chkSelectIndicator = (CheckBox)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("chkSelectIndicator");
            TextBox txtPayableAmount = (TextBox)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("txtPayableAmount");
            Label lblHoldAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblHoldAmount");
            TextBox txtRetentionAmt = (TextBox)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("txtRetentionAmt");
            Label lblPaidAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblPaidAmount");
            Label lbTotalAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblTotalAmount");
            Label lblAdvanceAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblAdvanceAmount");
            Label lblAdvancePaidAmount = (Label)grvLoadInvoiceDtl.Rows[_iRowIdx].FindControl("lblAdvancePaidAmount");
            double dblBalance;

            if (lblgdinvPIID.Text == "")
                lblgdinvPIID.Text = "0";
            if (lblgdinvVIID.Text == "")
                lblgdinvVIID.Text = "0";

            if (Convert.ToString(txtRetentionAmt.Text) == "")
            {
                Utility.FunShowAlertMsg(this, "Retention Amount should not be Blank");
                txtRetentionAmt.Text = Convert.ToString(lblHoldAmount.Text);
            }

            if (Convert.ToDouble(txtRetentionAmt.Text) > Convert.ToDouble(lblHoldAmount.Text))
            {
                Utility.FunShowAlertMsg(this, "Retention Amount should not exceed than Actual Retention Amount (i.e)" + Convert.ToString(lblHoldAmount.Text));
                dblBalance = Convert.ToDouble(lbTotalAmount.Text) - (Convert.ToDouble(lblHoldAmount.Text) + Convert.ToDouble(lblPaidAmount.Text));
                dblBalance = dblBalance - (Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblAdvancePaidAmount.Text));
                txtPayableAmount.Text = Convert.ToString(dblBalance);
                txtRetentionAmt.Text = Convert.ToString(lblHoldAmount.Text);
            }
            else
            {
                dblBalance = (Convert.ToDouble(lbTotalAmount.Text) + (Convert.ToDouble(lblHoldAmount.Text) - Convert.ToDouble(txtRetentionAmt.Text))) - (Convert.ToDouble(lblHoldAmount.Text) + Convert.ToDouble(lblPaidAmount.Text));
                dblBalance = dblBalance - (Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblAdvancePaidAmount.Text));
                txtPayableAmount.Text = Convert.ToString(dblBalance);
            }

            if (chkSelectIndicator.Checked == true)
            {
                DataSet dsInvoice = FunPriUpdateInvDtl(Convert.ToString(lblPoDtlID.Text), Convert.ToDouble(txtPayableAmount.Text), Convert.ToDouble(txtRetentionAmt.Text), 0, Convert.ToString(lblgdinvPIID.Text), Convert.ToString(lblgdinvVIID.Text));
                if (dsInvoice.Tables.Count > 1)
                    FunPriDisplayGridTotal(dsInvoice.Tables[2]);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void txtgdinvRetentionAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((TextBox)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvPaymentInvoiceDtl", strSelectID);
            Label lblPoDtlID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPoID");
            Label lblgdinvPIID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPIID");
            Label lblgdinvVIID = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvVIID");
            Label lblActualRetentionAmt = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvHoldAmount");
            Label lblPaidAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPaidAmount");
            TextBox txtgdinvPayableAmount = (TextBox)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("txtgdinvPayableAmount");
            TextBox txtgdinvRetentionAmt = (TextBox)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("txtgdinvRetentionAmt");
            Label lblgdinvPayableAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvPayableAmount");
            Label lblgdinvEnteredRetntionAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvEnteredRetntionAmount");
            Label lbTotalAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvTotalAmount");
            Label lblAdvanceAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvAdvanceAmount");
            Label lblAdvancePaidAmount = (Label)grvPaymentInvoiceDtl.Rows[_iRowIdx].FindControl("lblgdinvAdvancePaidAmount");
            double dblBalance;

            if (lblgdinvPIID.Text == "")
                lblgdinvPIID.Text = "0";
            if (lblgdinvVIID.Text == "")
                lblgdinvVIID.Text = "0";

            if (Convert.ToString(txtgdinvRetentionAmt.Text) == "")
            {
                Utility.FunShowAlertMsg(this, "Retention Amount should not be Blank");
                txtgdinvRetentionAmt.Text = Convert.ToString(lblgdinvEnteredRetntionAmount.Text);
                return;
            }

            if (Convert.ToDouble(txtgdinvRetentionAmt.Text) > Convert.ToDouble(lblActualRetentionAmt.Text))
            {
                Utility.FunShowAlertMsg(this, "Retention Amount should not exceed than Actual Retention Amount (i.e)" + Convert.ToString(lblActualRetentionAmt.Text));
                dblBalance = Convert.ToDouble(lbTotalAmount.Text) - (Convert.ToDouble(lblgdinvEnteredRetntionAmount.Text) + Convert.ToDouble(lblPaidAmount.Text));
                dblBalance = dblBalance - (Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblAdvancePaidAmount.Text));
                txtgdinvPayableAmount.Text = Convert.ToString(dblBalance);
                txtgdinvRetentionAmt.Text = Convert.ToString(lblgdinvEnteredRetntionAmount.Text);
            }
            else
            {
                dblBalance = (Convert.ToDouble(lbTotalAmount.Text) + (Convert.ToDouble(lblActualRetentionAmt.Text) - Convert.ToDouble(txtgdinvRetentionAmt.Text))) - (Convert.ToDouble(lblActualRetentionAmt.Text) + Convert.ToDouble(lblPaidAmount.Text));
                dblBalance = dblBalance - (Convert.ToDouble(lblAdvanceAmount.Text) - Convert.ToDouble(lblAdvancePaidAmount.Text));
                txtgdinvPayableAmount.Text = Convert.ToString(dblBalance);
            }

            DataSet dsInvoice = FunPriUpdateInvDtl(Convert.ToString(lblPoDtlID.Text), Convert.ToDouble(txtgdinvPayableAmount.Text), Convert.ToDouble(txtgdinvRetentionAmt.Text), 1, Convert.ToString(lblgdinvPIID.Text), Convert.ToString(lblgdinvVIID.Text));
            ViewState["PaymentInvoiceDetails"] = dsInvoice.Tables[0];
            FunPriBindGridDtls(2, dsInvoice.Tables[0]);

            ViewState["grvPaymentDetails"] = dsInvoice.Tables[1];
            FunPriBindGridDtls(3, dsInvoice.Tables[1]);

            if (Convert.ToInt32(dsInvoice.Tables[1].Rows.Count) == 0)
            {
                lblPaymentDetailsTotal.Text = lbltotalPaymentAdjust.Text = "0.00";
                txtDocAmount.Text = Math.Round(Convert.ToDecimal(lbltotalPaymentAdjust.Text), 0).ToString();
            }

            if (dsInvoice.Tables.Count > 1)
                FunPriDisplaySmryGridTotal(dsInvoice.Tables[2]);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    #endregion

    #region "Link Button Events"

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        try
        {
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = ((DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber"));
            DropDownList ddlFooterFlowType = ((DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType"));
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterDescription = ((TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription"));
            TextBox txtFooterAmount = ((TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount"));
            Label lblFooterActualAmount = ((Label)grvPaymentDetails.FooterRow.FindControl("lblFooterActualAmount"));

            if (!Is_DuplicatePayment(ddlFooterPrimeAccountNumber.SelectedValue, ddlFooterSubAccountNumber.SelectedValue, ddlFooterFlowType.SelectedValue, ddlFooterGL_Code.SelectedValue, ddlFooterSL_Code.SelectedValue))
            {
                if (ViewState["grvPaymentDetails"] != null)
                    dtPaymenttable = (DataTable)ViewState["grvPaymentDetails"];

                DataRow dtPaymentRow = dtPaymenttable.NewRow();
                if (ddlFooterSubAccountNumber != null && ddlFooterSubAccountNumber.Items.Count > 1)
                {
                    if (ddlFooterSubAccountNumber.SelectedIndex < 1)
                    {
                        Utility.FunShowAlertMsg(this, "Select the sub account number.");
                        return;
                    }
                }
                if (ddlPayTo.SelectedValue == "50")
                {
                    bool chkAcctbased = false;
                    chkAcctbased = FunPrichkAcctbasedorgeneral(ddlFooterPrimeAccountNumber.SelectedValue, ddlFooterSubAccountNumber.SelectedValue, ddlFooterFlowType.SelectedValue);
                    if (chkAcctbased)
                    {
                        Utility.FunShowAlertMsg(this, "Payment should made either through account or pay type.");
                        return;
                    }
                }
                if (ddlFooterSL_Code.AvailableRecords > 1 && ddlFooterSL_Code.SelectedValue == "0" && ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue == "1")
                {
                    Utility.FunShowAlertMsg(this, "select the sl code.");
                    return;
                }
                if (ddlFooterFlowType.SelectedIndex > 0 && ddlPayTo.SelectedValue != "50")
                {
                    if (Convert.ToDecimal(txtFooterAmount.Text) > Convert.ToDecimal(lblFooterActualAmount.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Amount should not exceed Rs." + lblFooterActualAmount.Text);
                        txtFooterAmount.Text = lblFooterActualAmount.Text;
                        return;
                    }
                }
                if (ddlFooterFlowType.SelectedValue == "215")
                {
                    if (Convert.ToDecimal(txtFooterAmount.Text) > Convert.ToDecimal(lblFooterActualAmount.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Amount should not exceed Rs." + lblFooterActualAmount.Text);
                        txtFooterAmount.Text = lblFooterActualAmount.Text;
                        return;
                    }
                }

                dtPaymentRow["Pa_Sa_Ref_ID"] = (Convert.ToInt64(ddlFooterPrimeAccountNumber.SelectedValue) > 0) ? Convert.ToInt64(ddlFooterPrimeAccountNumber.SelectedValue) : 0;
                dtPaymentRow["Panum"] = (Convert.ToInt64(ddlFooterPrimeAccountNumber.SelectedValue) > 0) ? Convert.ToString(ddlFooterPrimeAccountNumber.SelectedText) : null;
                dtPaymentRow["Sanum"] = "";// Convert.ToString(ddlFooterSubAccountNumber.SelectedItem.Text);
                dtPaymentRow["CashFlow_ID"] = (Convert.ToInt32(ddlFooterFlowType.SelectedValue) > 0) ? Convert.ToInt32(ddlFooterFlowType.SelectedValue) : 0;
                dtPaymentRow["CashFlow_Description"] = (Convert.ToInt32(ddlFooterFlowType.SelectedValue) > 0) ? Convert.ToString(ddlFooterFlowType.SelectedItem.Text) : null;
                dtPaymentRow["GL_Code"] = (Convert.ToString(ddlFooterGL_Code.SelectedValue) != "") ? Convert.ToString(ddlFooterGL_Code.SelectedItem.Text) : null;
                dtPaymentRow["SL_Code"] = (Convert.ToString(ddlFooterSL_Code.SelectedValue) != "") ? Convert.ToString(ddlFooterSL_Code.SelectedText) : null; ;
                dtPaymentRow["Remarks"] = Convert.ToString(txtFooterDescription.Text);
                dtPaymentRow["Amount"] = (Convert.ToString(txtFooterAmount.Text) != "") ? Convert.ToDouble(txtFooterAmount.Text) : 0;
                dtPaymentRow["ActualAmount"] = (Convert.ToString(lblFooterActualAmount.Text) != "") ? Convert.ToDouble(lblFooterActualAmount.Text) : 0;
                dtPaymentRow["CashFLow_Flag_ID"] = 0;
                dtPaymenttable.Rows.Add(dtPaymentRow);

                if (Convert.ToString(dtPaymenttable.Rows[0]["PANum"]) == "-1")
                {
                    dtPaymenttable.Rows.RemoveAt(0);
                }
                ViewState["grvPaymentDetails"] = dtPaymenttable;
                FunPriBindGridDtls(3, dtPaymenttable);
                if (ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue == "1")
                {
                    FunDisableGLSLCodeGrid(true);
                    FundisableGridValidation(false);
                }
                else
                {
                    FunDisableGLSLCodeGrid(false);
                    FundisableGridValidation(true);
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Payment cannot be duplicated.");
                return;
            }
            //FunPriLoadPANUM();
            FunPriLoadGLcodes();
            FunPriLoadPaytypeingrid(Convert.ToString(ddlPayTo.SelectedValue));
            if (ddlPayTo.SelectedValue == "50")
            {
                if (chkAccountBased.SelectedValue == "1")
                {
                    FunDisableGLSLCodeGrid(true);
                    FunProAcctBasedCtrlsValidation(false);
                }
                else
                {
                    FunDisableGLSLCodeGrid(false);
                    FunProAcctBasedCtrlsValidation(true);
                }
            }
            else
                FunDisableGLSLCodeGrid(false);

            //Added by Sathiyanathan on 30-DEC-2013
            //Bind Inflow for adjustment

            #region "HIDE"
            /*
            if (Convert.ToString(ddlFooterPrimeAccountNumber.SelectedValue) != "0")
            {
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();

                Procparam.Add("@PANUM", Convert.ToString(ddlFooterPrimeAccountNumber.SelectedValue));
                Procparam.Add("@Company_Id", intCompanyID.ToString());
                DataTable dtableLessDtl = Utility.GetDefaultData("S3G_LOANAD_GET_PAYMENT_INFLOWFLAG", Procparam);
                if (dtableLessDtl != null && dtableLessDtl.Rows.Count > 0)
                {
                    if (Convert.ToString(dtableLessDtl.Rows[0]["AddOrLess"]) == "1")
                    {
                        Utility.FunShowAlertMsg(this, "Cash Flow not yet Defined");
                        return;
                    }

                    DataTable dtableExistLessDtl = new DataTable();
                    dtableExistLessDtl = (DataTable)ViewState["grvPaymentAdjust"];

                    if (Convert.ToString(dtableExistLessDtl.Rows[0]["AddOrLess"]) == "")
                    {
                        dtableExistLessDtl.Rows.RemoveAt(0);
                    }

                    for (int i = 0; i < dtableLessDtl.Rows.Count; i++)
                    {
                        DataRow dr = dtableExistLessDtl.NewRow();
                        dr["AddOrLess"] = Convert.ToString(dtableLessDtl.Rows[i]["AddOrLess"]);
                        dr["PANum"] = Convert.ToString(dtableLessDtl.Rows[i]["PANum"]);
                        dr["SANum"] = Convert.ToString(dtableLessDtl.Rows[i]["SANum"]);
                        dr["PayType"] = Convert.ToString(dtableLessDtl.Rows[i]["PayType"]);
                        dr["PayTypeID"] = Convert.ToString(dtableLessDtl.Rows[i]["PayTypeID"]);
                        dr["GL_Code"] = Convert.ToString(dtableLessDtl.Rows[i]["GL_Code"]);
                        dr["SL_Code"] = Convert.ToString(dtableLessDtl.Rows[i]["SL_Code"]);
                        dr["Remarks"] = Convert.ToString(dtableLessDtl.Rows[i]["Remarks"]);
                        dr["Amount"] = Convert.ToString(dtableLessDtl.Rows[i]["Amount"]);

                        dtableExistLessDtl.Rows.Add(dr);
                    }

                    ViewState["PaymentAdjustment"] = dtableExistLessDtl;

                    ViewState["grvPaymentAdjust"] = dtableExistLessDtl;
                    grvPaymentAdjustment.DataSource = dtableExistLessDtl;
                    grvPaymentAdjustment.DataBind();
                    FunPriLoadPANUM();
                    FunPriLoadGLcodes();
                }
            }
            */
            #endregion

            //End Here
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }

    }

    protected void lnkgvFunderRemove_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvFunderReceipt", strSelectID);
            DataTable dtPmt = (DataTable)ViewState["FunderPmtDtl"];
            dtPmt.Rows.RemoveAt(_iRowIdx);
            dtPmt.AcceptChanges();
            if (dtPmt.Rows.Count == 0)
                dtPmt = (DataTable)ViewState["DefaultFunderPmtDtl"];

            ViewState["FunderPmtDtl"] = dtPmt;
            FunPriBindFunderGrid(dtPmt);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "GridView Events"

    protected void grvPaymentDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dt_PaymentDetails = (DataTable)ViewState["grvPaymentDetails"];
            if (dt_PaymentDetails != null && dt_PaymentDetails.Rows.Count > 0)
            {
                string strPanum = Convert.ToString(dt_PaymentDetails.Rows[e.RowIndex]["PANUM"]);
                dt_PaymentDetails.Rows.RemoveAt(e.RowIndex);
                if (dt_PaymentDetails.Rows.Count == 0)
                {
                    fundisablepaymentdtlsgrid();
                    FunPriLoadPaytypeingrid(ddlPayTo.SelectedValue);
                }
                else
                {
                    ViewState["grvPaymentDetails"] = dt_PaymentDetails;
                    FunPriBindGridDtls(3, dt_PaymentDetails);
                    FunPriLoadPANUM();
                    FunPriLoadGLcodes();
                    grvPaymentDetails.FooterRow.Visible = true;
                    FunPriLoadPaytypeingrid(ddlPayTo.SelectedValue);
                }
                if (ddlPayTo.SelectedValue == "50")
                {
                    if (chkAccountBased.SelectedValue == "1")
                    {
                        FunDisableGLSLCodeGrid(true);
                        FundisableGridValidation(false);
                        FunProAcctBasedCtrlsValidation(false);
                    }
                    else
                    {
                        FunDisableGLSLCodeGrid(false);
                        FundisableGridValidation(true);
                        FunProAcctBasedCtrlsValidation(true);
                    }
                }
                else
                {
                    FunDisableGLSLCodeGrid(false);
                    FundisableGridValidation(true);
                }
            }
            else
            {
                if (grvPaymentDetails != null)
                {
                    if (grvPaymentDetails.Rows.Count <= 1)
                    {
                        Utility.FunShowAlertMsg(this, "Should not allowed to remove Payment details,atleast one record needed");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, "Unable to delete.");
            return;
        }

    }

    protected void grvPaymentInvoiceDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtgdinvPayableAmount = (TextBox)e.Row.FindControl("txtgdinvPayableAmount");
                TextBox txtgdinvRetentionAmt = (TextBox)e.Row.FindControl("txtgdinvRetentionAmt");
                txtgdinvPayableAmount.SetDecimalPrefixSuffix(13, 2, false, false, "Payable Amount");
                txtgdinvRetentionAmt.SetDecimalPrefixSuffix(13, 2, false, false, "Retention Amount");
                if (strQsMode == "Q" || Convert.ToInt32(ddlPaymentStatus.SelectedValue) >= 3)
                {
                    txtgdinvPayableAmount.ReadOnly = txtgdinvRetentionAmt.ReadOnly = true;
                }
                if (Convert.ToInt32(ddlPaymentType.SelectedValue) > 1)
                {
                    txtgdinvRetentionAmt.ReadOnly = true;
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    protected void grvPaymentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)                 // if header - then set the style dynamically.
            {
                lblPaymentDetailsTotal.Text = "0";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
            {
                //lblFlowType
                Label lblFlowType = (Label)e.Row.FindControl("lblCashflow_Description");
                TextBox txtAmount = (TextBox)e.Row.FindControl("lblAmount");
                if ((!(string.IsNullOrEmpty(lblFlowType.Text))) && (!(string.IsNullOrEmpty(txtAmount.Text))))
                {
                    lblPaymentDetailsTotal.Text = (Convert.ToDecimal(lblPaymentDetailsTotal.Text) + Convert.ToDecimal(txtAmount.Text)).ToString();
                }
                if (ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue != "1")
                {
                    txtAmount.ReadOnly = true;
                }
                else if (Convert.ToInt32(ddlPaymentType.SelectedValue) > 0 && Convert.ToInt32(ddlPaymentType.SelectedValue) < 5)
                {
                    txtAmount.ReadOnly = true;
                }
                else if (ddlPayTo.SelectedValue == "11")
                {
                    // --code commented and added by saran in 01-Aug-2014 Insurance start 

                    //lblAmount.ReadOnly = true;
                    // --code commented and added by saran in 01-Aug-2014 Insurance start 

                }
                else
                {
                    txtAmount.ReadOnly = false;
                }
            }

            FunPriCalcSumAmountDetails();
            FunPriCalcSumAmount();


            if (string.Compare(strQsMode, "Q") == 0 || Convert.ToInt32(ddlPaymentStatus.SelectedValue) >= 3)
            {
                LinkButton lnkRemove = (LinkButton)e.Row.FindControl("lnkRemove");
                if (lnkRemove != null)
                    lnkRemove.Enabled = false;
                TextBox lblAmount = (TextBox)e.Row.FindControl("lblAmount");
                if (lblAmount != null)
                    lblAmount.ReadOnly = true;
                TextBox lblDescription = (TextBox)e.Row.FindControl("lblDescription");
                if (lblDescription != null)
                    lblDescription.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void grvFunderReceipt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                UserControls_S3GAutoSuggest ddlFndrGLCode = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlFndrGLCode");
                UserControls_S3GAutoSuggest ddlFndrSLCode = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlFndrSLCode");
                DropDownList ddlFndrCFDesc = (DropDownList)e.Row.FindControl("ddlFndrCFDesc");
                TextBox txtFooterFunderAmount = (TextBox)e.Row.FindControl("txtFooterFunderAmount");
                Label lblFooterFndrCFFlagID = (Label)e.Row.FindControl("lblFooterFndrCFFlagID");
                txtFooterFunderAmount.SetDecimalPrefixSuffix(13, 2, true, false);
                DataTable dtCashFlow = (DataTable)ViewState["FunderCashFlowDesc"];

                if (dtCashFlow != null && dtCashFlow.Rows.Count > 0)
                {
                    ddlFndrCFDesc.FillDataTable(dtCashFlow, "CashFlow_ID", "CashFlowFlag_Desc", true);
                    lblFooterFndrCFFlagID.Text = Convert.ToString(dtCashFlow.Rows[0]["CashFlow_Flag_ID"]);
                }
                ddlFndrGLCode.Clear();
                ddlFndrSLCode.Clear();
                ddlFndrGLCode.Enabled = ddlFndrSLCode.Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) == 8) ? false : true;

                if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 8)
                {
                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();

                    Procparam.Add("@Option", "18");
                    Procparam.Add("@Funder_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                    DataTable dtFndrGL = Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam);
                    //if (dtFndrGL.Rows.Count > 0)
                    //{
                    //    ddlFndrGLCode.SelectedText = ddlFndrGLCode.SelectedValue = Convert.ToString(dtFndrGL.Rows[0]["GL_Code"]);
                    //    ddlFndrSLCode.SelectedText = ddlFndrSLCode.SelectedValue = Convert.ToString(dtFndrGL.Rows[0]["SL_Code"]);
                    //}
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void grvLoadInvoiceDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtPayableAmount = (TextBox)e.Row.FindControl("txtPayableAmount");
                TextBox txtRetentionAmt = (TextBox)e.Row.FindControl("txtRetentionAmt");
                txtRetentionAmt.SetDecimalPrefixSuffix(13, 2, false, false, "Retention Amount");
                //txtPayableAmount.SetDecimalPrefixSuffix(13, 2, false, false, "Amount to be disbursed");
                txtRetentionAmt.ReadOnly = (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1) ? false : true;
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("chkSelectAllInvoices");
                chkSelectAll.Checked = blnChkSlctAll;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #endregion

    #region "METHODS"

    private void FunPriLoadLOV()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Is_Active", "1");

            DataSet dsLov = Utility.GetDataset("S3G_Loanad_GetPaymentLookup", Procparam);
            if (dsLov != null)
            {
                ddlLOB.BindDataTable(dsLov.Tables[0], new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                if (ddlLOB.Items.Count == 2)
                {
                    ddlLOB.SelectedIndex = 1;
                    ddlLOB.ClearDropDownList();
                }

                FunPriFillDropdown(dsLov.Tables[1], ddlPaymentStatus, "PaymentStatusID", "PaymentStatus", true);

                FunPriFillDropdown(dsLov.Tables[2], ddlPayMode, "Lookup_Code", "Lookup_Description", true);

                //FunPriFillDropdown(dsLov.Tables[3], ddlPayTo, "Lookup_Code", "Lookup_Description", true);

                FunPriFillDropdown(dsLov.Tables[4], ddlChequeStatus, "ChequeStatusID", "ChequeStatus", true);

                ViewState["grvPaymentDetails"] = dsLov.Tables[5];
                ViewState["DefaultPaymentDetails"] = dsLov.Tables[5];
                dtPaymenttable = dsLov.Tables[5].Clone();

                FunPriFillDropdown(dsLov.Tables[6], ddlPaymentType, "ID", "Name", true);

                FunPriLoadCurrencyCode();

                ViewState["FunderPmtDtl"] = dsLov.Tables[7];
                ViewState["DefaultFunderPmtDtl"] = dsLov.Tables[7];

                ViewState["FunderCashFlowDesc"] = dsLov.Tables[8];
                FunPriFillDropdown(dsLov.Tables[9], ddlInvoiceSortBy, "ID", "Name", true);

                if (dsLov.Tables[11].Rows.Count > 0)
                {
                    ddlBranch.SelectedValue = Convert.ToString(dsLov.Tables[11].Rows[0]["ID"]);
                    ddlBranch.SelectedText = Convert.ToString(dsLov.Tables[11].Rows[0]["Name"]);
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    public void FunLoadPayToDetails()
    {
        try
        {
            if (ddlLOB.SelectedIndex > 0)
                ucCustomerCodeLov.strLOBID = ddlLOB.SelectedValue;
            if (ddlPayTo.SelectedIndex > 0)
            {
                switch (ddlPayTo.SelectedValue)
                {
                    case "1":
                        ucCustomerCodeLov.strLOV_Code = "CMD";
                        break;
                    case "2":
                        ucCustomerCodeLov.strLOV_Code = "ENDSA";
                        break;
                    case "3":
                        ucCustomerCodeLov.strLOV_Code = "ENFIA";
                        break;
                    case "4":
                        ucCustomerCodeLov.strLOV_Code = "ENDMA";
                        break;
                    case "5":
                        ucCustomerCodeLov.strLOV_Code = "ENDBTCOLL";
                        break;
                    case "6":
                        ucCustomerCodeLov.strLOV_Code = "ENVENDOR";
                        break;
                    case "7":
                        ucCustomerCodeLov.strLOV_Code = "ENSUNDRY";
                        break;
                    case "8":
                        ucCustomerCodeLov.strLOV_Code = "ENDEALER";
                        break;
                    case "9":
                        ucCustomerCodeLov.strLOV_Code = "ENBROK";
                        break;
                    case "10":
                        ucCustomerCodeLov.strLOV_Code = "ENEMP";
                        break;
                    case "11":
                        ucCustomerCodeLov.strLOV_Code = "ENINS";
                        break;
                    case "12":
                        ucCustomerCodeLov.strLOV_Code = "FACT";
                        ucCustomerCodeLov.DispalyContent = UserControls_LOBMasterView.enumContentType.Name;
                        break;
                    case "13":
                        ucCustomerCodeLov.strLOV_Code = "FLM";
                        ucCustomerCodeLov.DispalyContent = UserControls_LOBMasterView.enumContentType.Name;
                        break;
                    default:
                        ucCustomerCodeLov.strLOV_Code = "CMD";
                        break;
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadCustomerEntityDtls(string HdnId)
    {
        try
        {
            ViewState["hdnCustorEntityID"] = HdnId;
            if (ddlPayTo.SelectedValue == "50")// For general Pay To
            {
                PnlCustEntityInformation.Enabled = false;
                txtCustomerCode.Visible = false;
            }
            else
            {
                if (ddlLOB.SelectedItem.Text.Contains("FT") && ddlPayTo.SelectedValue == "12")
                {
                    TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    TextBox txtCustomerName = (TextBox)ucCustomerAddress.FindControl("txtCustomerName");
                    txtCustomerCode.Text = txtCustomerName.Text = txtName.Text;
                }
                else
                {
                    DataTable dtCustEntityDtls = new DataTable();
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", intCompanyID.ToString());
                    Procparam.Add("@ID", HdnId);
                    if (ddlPayTo.SelectedValue == "1")// For Customer Pay To
                    {
                        Procparam.Add("@TypeID", "144");
                    }
                    else if (ddlPayTo.SelectedValue == "13")// For Funder Pay To
                    {
                        Procparam.Add("@TypeID", "150");
                    }
                    else//For All other Entity Types
                    {
                        Procparam.Add("@TypeID", "145");
                    }
                    dtCustEntityDtls = Utility.GetDefaultData("S3G_LOANAD_GETCustomerorEntityDetails", Procparam);
                    if (dtCustEntityDtls.Rows.Count > 0)
                    {
                        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                        txtName.Text = txtCustomerCode.Text = dtCustEntityDtls.Rows[0]["Code"].ToString();

                        ucCustomerAddress.SetCustomerDetails(dtCustEntityDtls.Rows[0]["Code"].ToString(),
                                dtCustEntityDtls.Rows[0]["Address1"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["Address2"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["city"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["state"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["country"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["pincode"].ToString(), dtCustEntityDtls.Rows[0]["Name"].ToString(),
                        dtCustEntityDtls.Rows[0]["Telephone"].ToString(),
                        dtCustEntityDtls.Rows[0]["Mobile"].ToString(),
                        dtCustEntityDtls.Rows[0]["email"].ToString(), dtCustEntityDtls.Rows[0]["website"].ToString());
                        ViewState["Address"] = dtCustEntityDtls;
                    }
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    //Bank Codes
    private void FunPriLoadBankCodes()
    {
        try
        {
            FunPriClearBankDetails();
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Option", "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            //if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            //    Procparam.Add("@LobID", ddlLOB.SelectedValue);
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
                Procparam.Add("@LocationID", ddlBranch.SelectedValue);
            ddlbankname.BindDataTable("S3G_ORG_GetPmtReqBankdetails", Procparam, new string[] { "SYS_BANK_CODE", "BANK_NAME" });

        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadCustomerBank()
    {
        try
        {
            FunPriClearBankDetails();
            Int32 intPayTo = Convert.ToInt32(ddlPayTo.SelectedValue);
            if ((intPayTo == 1 || intPayTo == 3 || intPayTo == 6 || intPayTo == 8 || intPayTo == 9 || intPayTo == 13) && Convert.ToString(ViewState["hdnCustorEntityID"]) != "")
            {
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_Id", Convert.ToString(intCompanyID));
                Procparam.Add("@User_ID", Convert.ToString(intUserID));
                Procparam.Add("@Option", "8");
                if (intPayTo == 1)          //Customer
                {
                    Procparam.Add("@Option1", "3");
                    Procparam.Add("@Customer_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                }
                else if (intPayTo == 13)    //Funder
                {
                    Procparam.Add("@Option1", "2");
                    Procparam.Add("@Funder_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                }
                else if (intPayTo == 6 || intPayTo == 8 || intPayTo == 9)
                {
                    Procparam.Add("@Option1", "1");
                    Procparam.Add("@vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                }

                DataSet dsCstBank = Utility.GetDataset("S3G_Get_PaymentCMNLST", Procparam);
                FunPriFillDropdown(dsCstBank.Tables[0], ddlPayeeBankAccount, "ID", "Name", true);
                if (dsCstBank.Tables[1].Rows.Count > 0)
                {
                    ddlPayeeBankAccount.SelectedValue = Convert.ToString(dsCstBank.Tables[1].Rows[0]["ID"]);
                }
                //ddlPayeeBankAccount.BindDataTable("S3G_Get_PaymentCMNLST", Procparam, new string[] { "ID", "Name" });
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadAcctNumbers()
    {
        try
        {
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Option", "2");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@SYS_BANK_CODE", ddlbankname.SelectedValue);
            DataSet DS = Utility.GetDataset("S3G_ORG_GetPmtReqBankdetails", Procparam);
            if (DS.Tables[0].Rows.Count > 0)
            {
                if (DS.Tables[0].Rows[0]["ErrorCode"].ToString() == "54")
                {
                    Utility.FunShowAlertMsg(this, "Selected bank GLCode is not in Account setup master.");
                    ddlbankname.SelectedIndex = -1;
                    return;
                }
            }
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Option", "4");
            Procparam.Add("@SYS_BANK_CODE", ddlbankname.SelectedValue);
            Procparam.Add("@LocationID", ddlBranch.SelectedValue);
            ddlAcctNumber.BindDataTable("S3G_ORG_GetPmtReqBankdetails", Procparam, new string[] { "BankMaster_Details_ID", "ACCT_NUMBER" });
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// To change the form depending on the Mode set by the Query String.
    /// </summary>
    private void FunPriFormActToMode()
    {
        try
        {
            switch (Convert.ToChar(strQsMode))
            {
                case 'Q':   // query Mode
                    lblPaymentRequest.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FunPriLoadFormControls();
                    FunloadGridPayments();
                    QueryView();
                    FunPriEnblDsblGrid();
                    if (grvFunderReceipt != null && grvFunderReceipt.FooterRow != null)
                    {
                        grvFunderReceipt.FooterRow.Visible = false;
                        grvFunderReceipt.Columns[grvFunderReceipt.Columns.Count - 1].Visible = false;
                    }
                    Int32 iPayMode = Convert.ToInt32(ddlPayMode.SelectedValue);
                    lblInstrumentDate.Text = (iPayMode == 5 || iPayMode == 6) ? "Transfer Date" : "Instrument Date";
                    lblFavouringName.CssClass = (iPayMode == 5 || iPayMode == 6) ? "styleDisplayLabel" : "styleReqFieldLabel";
                    lblInstrumentNumber.CssClass = (iPayMode == 2) ? "styleReqFieldLabel" : "styleDisplayLabel";
                    break;
                case 'M':   // modify Mode
                    // FunPriLockControls(true);
                    lblPaymentRequest.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunPriLoadFormControls();
                    FunloadGridPayments();
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                    //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                    Procparam.Add("@Location_Id", ddlBranch.SelectedValue);
                    Procparam.Add("@Closure_Date", Utility.StringToDate(txtPaymentRequestDate.Text).ToString());

                    //DataTable chkmonthendclosure = new DataTable();
                    //chkmonthendclosure = Utility.GetDefaultData("S3G_LOANAD_ValidateMonthClosure", Procparam);

                    string chkmonthendclosure = Utility.ValidateMonthClosure("S3G_LOANAD_ValidateMonthClosure", Procparam);

                    if (chkmonthendclosure == "5")
                    {
                        Utility.FunShowAlertMsg(this, "Modification is not allowed for a closed month");
                        QueryView();
                    }
                    else
                    {
                        ModifyMode();
                    }
                    FunPriEnblDsblGrid();
                    //  ddlPayMode.ClearDropDownList();

                    if (Convert.ToInt32(ddlPaymentStatus.SelectedValue) > 2)
                    {
                        if (grvFunderReceipt != null && grvFunderReceipt.FooterRow != null)
                        {
                            grvFunderReceipt.FooterRow.Visible = false;
                            grvFunderReceipt.Columns[grvFunderReceipt.Columns.Count - 1].Visible = false;
                        }
                    }
                    iPayMode = Convert.ToInt32(ddlPayMode.SelectedValue);
                    lblInstrumentDate.Text = (iPayMode == 5 || iPayMode == 6) ? "Transfer Date" : "Instrument Date";
                    lblFavouringName.CssClass = (iPayMode == 5 || iPayMode == 6) ? "styleDisplayLabel" : "styleReqFieldLabel";
                    lblInstrumentNumber.CssClass = (iPayMode == 2) ? "styleReqFieldLabel" : "styleDisplayLabel";
                    if (!bDelete)
                        btnCancelPayment.Enabled = false;
                    break;
                case 'C':   // create mode
                    lblPaymentRequest.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    ddlPaymentStatus.SelectedValue = "1";
                    /* PanelPaymentAdjustment.Visible =
                     PanelPaymentDetails.Visible = false;*/

                    //         grvPaymentAdjustment.FooterRow.Visible = false;
                    PnlCustEntityInformation.Enabled = true;
                    ucCustomerCodeLov.ButtonEnabled = false;
                    btnPrintVoucher.Visible =
                    btnPrintCheque.Visible =
                        //btnCoveringLetter.Visible =
                    btnCancelPayment.Visible = false;
                    FunLoadPaymentAdjustment();
                    FunPriLoadGLcodes();
                    if (ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue == "1")
                    {
                        FunDisableGLSLCodeGrid(true);
                        FundisableGridValidation(false);
                    }
                    else
                    {
                        FunDisableGLSLCodeGrid(false);
                        FundisableGridValidation(true);
                    }
                    /*To bind Grid "No Records Found In Page Load- Call ID - 3514 - Added by Vinodha M on March 23,2016*/
                    FunPriBindGridDtls(2, null);
                    /*To bind Grid "No Records Found In Page Load- Call ID - 3514 - Added by Vinodha M on March 23,2016*/

                    //TabPanelPBD.Enabled = false;

                    break;
                default:
                    break;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadFormControls()
    {
        try
        {
            if (Procparam != null)
            {
                Procparam.Clear();
            }
            else
            {
                Procparam = new Dictionary<string, string>();
            }

            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Request_No", strRequestID);
            FunPriLoadFormControls(Utility.GetDefaultData(SPNames.S3G_LoanAd_GetPaymentRequest, Procparam));
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadFormControls(DataTable dtPaymentRequest)
    {
        try
        {
            if (dtPaymentRequest != null && dtPaymentRequest.Rows.Count > 0)
            {
                ddlLOB.Items.Clear();
                ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dtPaymentRequest.Rows[0]["LOB_NAME"].ToString(), dtPaymentRequest.Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = dtPaymentRequest.Rows[0]["LOB_NAME"].ToString(); // paytotypecode in DB

                ddlBranch.SelectedValue = dtPaymentRequest.Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = dtPaymentRequest.Rows[0]["Location_Name"].ToString();
                ddlBranch.ToolTip = dtPaymentRequest.Rows[0]["Location_Name"].ToString();

                txtPaymentRequestNo.Text = dtPaymentRequest.Rows[0]["Payment_Request_No"].ToString();
                txtPaymentRequestDate.Text = Convert.ToString(dtPaymentRequest.Rows[0]["Payment_Request_Date"]);
                txtValueDate.Text = Convert.ToString(dtPaymentRequest.Rows[0]["Value_Date"]);
                ddlPaymentType.SelectedValue = Convert.ToString(dtPaymentRequest.Rows[0]["Pay_Type_Code"]);
                ddlReceiptFrom.SelectedValue = Convert.ToString(dtPaymentRequest.Rows[0]["Receipt_From_ID"]);
                ddlReceiptFrom.SelectedText = Convert.ToString(dtPaymentRequest.Rows[0]["Receipt_From_Description"]);
                ddlTranche.SelectedValue = Convert.ToString(dtPaymentRequest.Rows[0]["Tranche_ID"]);
                ddlTranche.SelectedText = Convert.ToString(dtPaymentRequest.Rows[0]["Tranche_Name"]);

                ddlPayMode.SelectedValue = Convert.ToString(dtPaymentRequest.Rows[0]["Pay_Mode_Code"]);
                ddlPayMode.ToolTip = dtPaymentRequest.Rows[0]["Payment_Mode"].ToString(); // paytotypecode in DB
                FunPriLoadBankCodes();
                //ddlPayMode.SelectedValue = dtPaymentRequest.Rows[0]["Pay_Mode_Code"].ToString();

                if (ddlPayMode.SelectedValue != "3")
                {
                    TabPanelPBD.Enabled = true;
                }
                else
                {
                    TabPanelPBD.Enabled = false;
                }
                chkAccountBased.SelectedValue = dtPaymentRequest.Rows[0]["Account_Based"].ToString();
                ddlCurrencyCode.SelectedValue = dtPaymentRequest.Rows[0]["Currency_ID"].ToString();

                //txtDocAmount.Text = Convert.ToDecimal(dtPaymentRequest.Rows[0]["Pay_Amount"].ToString()).ToString("0.00"); // doc Amount
                txtDocAmount.Text = dtPaymentRequest.Rows[0]["Pay_Amount"].ToString();

                ddlPayTo.Items.Add(new System.Web.UI.WebControls.ListItem(dtPaymentRequest.Rows[0]["Pay_To"].ToString(), dtPaymentRequest.Rows[0]["Pay_To_Type_Code"].ToString()));
                ddlPayTo.ToolTip = dtPaymentRequest.Rows[0]["Pay_To"].ToString(); // paytotypecode in DB

                //ddlPaymentStatus.SelectedValue = dtPaymentRequest.Rows[0]["Pmt_Voucher_status"].ToString();
                ddlPaymentStatus.Items.Clear();
                ddlPaymentStatus.Items.Add(new System.Web.UI.WebControls.ListItem(dtPaymentRequest.Rows[0]["Payment_Status"].ToString(), dtPaymentRequest.Rows[0]["Pmt_Voucher_status"].ToString()));
                ddlPaymentStatus.ToolTip = dtPaymentRequest.Rows[0]["Pay_To"].ToString(); // paytotypecode in DB


                ViewState["IsVoucher_Print"] = dtPaymentRequest.Rows[0]["IsVoucher_Print"].ToString();  // N- not printed, P - Printed, D - Duplicate

                ViewState["IsCheque_Print"] = dtPaymentRequest.Rows[0]["IsCheque_Print"].ToString();    // N- not printed, P - Printed, D - Duplicate


                // ddlPaymentStatus.SelectedValue = dtPaymentRequest.Rows[0]["Status"].ToString();
                //For Instrument 
                if (dtPaymentRequest.Rows[0]["Bank_ID"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["Bank_ID"]) != string.Empty && Convert.ToInt32(dtPaymentRequest.Rows[0]["Bank_ID"]) > 0)
                {
                    ddlbankname.SelectedValue = dtPaymentRequest.Rows[0]["Bank_ID"].ToString();
                    FunPriLoadAcctNumbers();
                }
                if (dtPaymentRequest.Rows[0]["GL_ACCOUNT"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["GL_ACCOUNT"]) != string.Empty)
                    txtGLCode.Text = dtPaymentRequest.Rows[0]["GL_ACCOUNT"].ToString();
                if (dtPaymentRequest.Rows[0]["SL_ACCOUNT"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["SL_ACCOUNT"]) != string.Empty)
                    txtSLCode.Text = dtPaymentRequest.Rows[0]["SL_ACCOUNT"].ToString();
                if (dtPaymentRequest.Rows[0]["Instrument_No"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["Instrument_No"]) != string.Empty)
                    txtInstrumentNumber.Text = dtPaymentRequest.Rows[0]["Instrument_No"].ToString();
                if (dtPaymentRequest.Rows[0]["Instrument_Date"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["Instrument_Date"]) != string.Empty)
                    txtInstrumentDate.Text = Convert.ToString(dtPaymentRequest.Rows[0]["Instrument_Date"]);
                if (dtPaymentRequest.Rows[0]["branchname"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["branchname"]) != string.Empty)
                    ViewState["BankBranch"] = dtPaymentRequest.Rows[0]["branchname"].ToString();
                if (dtPaymentRequest.Rows[0]["ACCT_NUMBER"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["ACCT_NUMBER"]) != string.Empty)
                    ViewState["AccountNumber"] = dtPaymentRequest.Rows[0]["ACCT_NUMBER"].ToString();
                if (dtPaymentRequest.Rows[0]["BankMaster_Details_ID"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["BankMaster_Details_ID"]) != string.Empty)
                    ddlAcctNumber.SelectedValue = dtPaymentRequest.Rows[0]["BankMaster_Details_ID"].ToString();
                if (dtPaymentRequest.Rows[0]["branchname"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["branchname"]) != string.Empty)
                    txtBankbranch.Text = dtPaymentRequest.Rows[0]["branchname"].ToString();
                if (dtPaymentRequest.Rows[0]["IFSC_CODE"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["IFSC_CODE"]) != string.Empty)
                    txtIFSC_Code.Text = dtPaymentRequest.Rows[0]["IFSC_CODE"].ToString();
                if (dtPaymentRequest.Rows[0]["Remarks"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["Remarks"]) != string.Empty)
                    txtRemarks.Text = dtPaymentRequest.Rows[0]["Remarks"].ToString();
                if (dtPaymentRequest.Rows[0]["Payment_Gateway_Ref_No"] != null && Convert.ToString(dtPaymentRequest.Rows[0]["Payment_Gateway_Ref_No"]) != string.Empty)
                    txtPmtGatewayRefNo.Text = dtPaymentRequest.Rows[0]["Payment_Gateway_Ref_No"].ToString();
                //For RadiobuttonSelection
                txtRemarks.Text = dtPaymentRequest.Rows[0]["Remarks1"].ToString();
                if (ddlPayMode.SelectedValue == "1")
                {
                    if (dtPaymentRequest.Rows[0]["IsCheque_Print"].ToString() != null && dtPaymentRequest.Rows[0]["IsVoucher_Print"].ToString() != string.Empty)
                    {
                        if (Convert.ToString(dtPaymentRequest.Rows[0]["IsCheque_Print"]).Trim() != "N")
                        {
                            btnPrintCheque.Text = "Reprint Cheque";
                            FunPriSetRemarksMandatory(true);
                        }
                        else
                        {
                            FunPriSetRemarksMandatory(false);
                            btnPrintCheque.Text = "Print Cheque";
                        }
                        //For Cheque status
                        FunPriToSetChequeStatus(dtPaymentRequest.Rows[0]["IsCheque_Print"].ToString());
                        //FunPriLoadBankCodes();
                    }
                }
                FunPriGetExchangeRate();

                if (Convert.ToInt32(ddlPaymentStatus.SelectedValue) == 2 || Convert.ToInt32(ddlPaymentStatus.SelectedValue) == 4 || Convert.ToInt32(ddlPaymentStatus.SelectedValue) == 5)
                {
                    FunPriLockControls(false);
                }

                if (!(string.IsNullOrEmpty(dtPaymentRequest.Rows[0]["Customer_ID"].ToString())))
                {
                    PnlCustEntityInformation.Enabled = true;
                    ucCustomerCodeLov.ButtonEnabled = true;
                    ucCustomerAddress.Caption = "Customer";
                    FunPriLoadCustomerEntityDtls(dtPaymentRequest.Rows[0]["Customer_ID"].ToString());
                }
                else if (Convert.ToString(dtPaymentRequest.Rows[0]["Funder_ID"]) != "")
                {
                    PnlCustEntityInformation.Enabled = true;
                    ucCustomerCodeLov.ButtonEnabled = true;
                    ucCustomerAddress.Caption = "Funder";
                    FunPriLoadCustomerEntityDtls(dtPaymentRequest.Rows[0]["Funder_ID"].ToString());
                }
                else
                {
                    PnlCustEntityInformation.Enabled = true;
                    ucCustomerCodeLov.ButtonEnabled = true;
                    ucCustomerAddress.Caption = "Entity";
                    FunPriLoadCustomerEntityDtls(dtPaymentRequest.Rows[0]["Vendor_Code"].ToString());
                }

                if (ddlPayTo.SelectedIndex > 0)
                {
                    if (ddlPayTo.SelectedValue == "11")
                    {
                        ucCustomerAddress.Caption = "Insurance Company";
                        lblCode.Text = "Insurance Company Code";
                    }

                }
                // Added for Bank details cannot modify one time if it is created....as per bashyam sir on 13/10/2011

                if (ddlPayMode.SelectedIndex > 0)//for cheque
                {
                    if (ddlPayMode.SelectedValue == "1" && Convert.ToInt32(ddlPaymentStatus.SelectedValue) > 2)
                    {
                        if (dtPaymentRequest.Rows[0]["IsCheque_Print"].ToString() != null && dtPaymentRequest.Rows[0]["IsCheque_Print"].ToString() != "N")
                        {
                            ddlbankname.ClearDropDownList();
                            ddlAcctNumber.ClearDropDownList();
                            //ImageInstrumentDate.Visible = false;
                        }
                    }
                }
                if (Convert.ToInt32(dtPaymentRequest.Rows[0]["Payee_Bank_ID"]) > 0)
                {
                    FunPriFillDropdown(dtPaymentRequest, ddlPayeeBankAccount, "Payee_Bank_ID", "Payee_Bank_Name", false);
                }

                txtFavouringName.Text = Convert.ToString(dtPaymentRequest.Rows[0]["Favour_Name"]);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunloadGridPayments()
    {
        try
        {
            int intTotalRecords = 0;
            DataSet Dsgrid = new DataSet();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Request_No", strRequestID);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@User_ID", Convert.ToString(intUserID));

            if ((ddlPayTo.SelectedValue == "1" || ddlPayTo.SelectedValue == "12") && ddlLOB.SelectedItem.Text.Contains("FT"))
            {
                Dsgrid = Utility.GetDataset("S3G_LoanAd_GetFactPayDetails", Procparam);
                TextBox txtCustomerName = (TextBox)ucCustomerAddress.FindControl("txtCustomerName");
                TextBox txtCusAddress = (TextBox)ucCustomerAddress.FindControl("txtCusAddress");
                txtCustomerName.Text = Dsgrid.Tables[0].Rows[0]["Pay_To_Name"].ToString();
                txtCusAddress.Text = Dsgrid.Tables[0].Rows[0]["Pay_To_Address"].ToString();

                if (ddlPayTo.SelectedValue == "1")
                    grvPaymentDetails.Columns[3].HeaderText = "Party";
                if (ddlPayTo.SelectedValue == "12")
                    grvPaymentDetails.Columns[3].HeaderText = "Cusotmer";

                grvPaymentDetails.Columns[6].Visible = false;
                grvPaymentDetails.Columns[7].Visible = true;
            }
            else
            {
                Procparam.Add("@CurrentPage", ProPageNumRW1.ToString());
                Procparam.Add("@PageSize", ProPageSizeRW1.ToString());
                Procparam.Add("@SearchValue", hdnSearch.Value);
                Procparam.Add("@OrderBy", hdnOrderBy.Value);
                Procparam.Add("@TotalRecords", ViewState["TotRec"] == null ? intTotalRecords.ToString() : Convert.ToInt32(ViewState["TotRec"]).ToString());
                Dsgrid = Utility.GetDataset("S3G_LoanAd_GetPaymentRequestGriddetailsMOD", Procparam);
                ViewState["TotRec"] = Dsgrid.Tables[3].Rows[0]["@TotalRecords"].ToString();
            }

            if (Dsgrid.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Dsgrid.Tables[0].Rows[0]["Payment_Type"]) == 5 || Convert.ToInt32(Dsgrid.Tables[0].Rows[0]["Payment_Type"]) == 8)
                {
                    ViewState["FunderPmtDtl"] = Dsgrid.Tables[0];
                    FunPriBindFunderGrid((DataTable)ViewState["FunderPmtDtl"]);
                }
                else
                {
                    ViewState["grvPaymentDetails"] = Dsgrid.Tables[0];
                    FunPriBindGridDtls(3, Dsgrid.Tables[0]);
                    grvPaymentDetails.FooterRow.Visible = false;
                    //Commented for OPC
                    //foreach (DataRow dr in Dsgrid.Tables[0].Rows)
                    //{
                    //    if (dr["RefDocNo"].ToString() != string.Empty)
                    //    {
                    //        grvPaymentDetails.Columns[4].Visible = true;
                    //    }
                    //}
                }
            }
            if (Dsgrid.Tables[1].Rows.Count > 0)
            {
                grvPaymentAdjustment.DataSource = Dsgrid.Tables[1];
                grvPaymentAdjustment.DataBind();
                ViewState["grvPaymentAdjust"] = Dsgrid.Tables[1];
                grvPaymentAdjustment.FooterRow.Visible = false;
            }
            if (Dsgrid.Tables.Count > 2)
            {
                if (Dsgrid.Tables[2].Rows.Count > 0)
                {
                    ViewState["PaymentInvoiceDetails"] = Dsgrid.Tables[2];
                    FunPriBindGridDtls(2, Dsgrid.Tables[2]);
                    Procparam.Clear();
                    Procparam.Add("@User_ID", Convert.ToString(intUserID));
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@Is_Added", "1");    //Added on 15Jun2015 for OPC Req
                    DataTable dtSmry = Utility.GetDefaultData("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);
                    if (dtSmry != null && dtSmry.Rows.Count > 0)
                        FunPriDisplaySmryGridTotal(dtSmry);
                }
                else
                {
                    FunPriSetSearchValue();
                    Pagenavigator1.Navigation(Convert.ToInt32(ViewState["TotRec"]), ProPageNumRW1, ProPageSizeRW1);
                    Pagenavigator1.setPageSize(ProPageSizeRW1);
                }
            }
            //FunPriLoadPaytypeingrid(Convert.ToString(ddlPayTo.SelectedValue));
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriBindFunderGrid(DataTable dt)
    {
        try
        {
            grvFunderReceipt.DataSource = dt;
            grvFunderReceipt.DataBind();
            grvFunderReceipt.Rows[0].Visible = (Convert.ToInt64(dt.Rows[0]["Note_ID"]) > 0) ? true : false;
            FunPriCalcSumAmountDetails();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriFillDropdown(DataTable dtLov, DropDownList ddlObj, string strValueField, string strDisplayField, bool blnRequest)
    {
        try
        {
            if (ddlObj != null && ddlObj.Items.Count > 0)
            {
                ddlObj.Items.Clear();
            }

            ddlObj.DataSource = dtLov;
            ddlObj.DataValueField = strValueField;
            ddlObj.DataTextField = strDisplayField;
            ddlObj.DataBind();
            if (blnRequest == true)
            {
                ddlObj.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearCustomerDtls()
    {
        try
        {
            TextBox txtCode = ucCustomerCodeLov.FindControl("txtName") as TextBox;
            txtCode.Text = txtCustomerCode.Text = "";
            ucCustomerAddress.ClearCustomerDetails();
            ViewState["hdnCustorEntityID"] = null;
            lblCode.Text = "Lessee/Entity";
            ddlPayeeBankAccount.Items.Clear();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearBankDetails()
    {
        try
        {
            txtInstrumentNumber.Text = txtIFSC_Code.Text = txtBankbranch.Text = txtGLCode.Text =
            txtSLCode.Text = txtInstrumentNumber.Text = txtInstrumentDate.Text = txtPmtGatewayRefNo.Text =
            txtRemarks.Text = txtFavouringName.Text = "";

            ddlAcctNumber.Items.Clear();
        }
        catch (Exception objException)
        {
            throw objException;

        }
    }

    private void FunClearPaymentdtlgridfooter()
    {
        try
        {
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");
            ddlFooterSL_Code.Clear();
            ddlFooterGL_Code.SelectedIndex = ddlFooterFlowType.SelectedIndex = -1;
            txtFooterDescription.Text = txtFooterAmount.Text = "";
            if (ddlFooterSubAccountNumber != null && ddlFooterSubAccountNumber.Items.Count > 0)
            {
                ddlFooterSubAccountNumber.Items.Clear();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunProToggleCustomerAddress(bool blEnable)
    {
        try
        {
            TextBox txtCusAddress = (TextBox)ucCustomerAddress.FindControl("txtCusAddress");
            TextBox txtPhone = (TextBox)ucCustomerAddress.FindControl("txtPhone");
            TextBox txtMobile = (TextBox)ucCustomerAddress.FindControl("txtMobile");
            TextBox txtEmail = (TextBox)ucCustomerAddress.FindControl("txtEmail");
            TextBox txtWebSite = (TextBox)ucCustomerAddress.FindControl("txtWebSite");
            txtCusAddress.ReadOnly = txtPhone.ReadOnly = txtMobile.ReadOnly =
                txtEmail.ReadOnly = txtWebSite.ReadOnly = !blEnable;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FundisableGridValidation(bool blnflag)
    {
        try
        {
            if (grvPaymentDetails.FooterRow != null)
            {
                //RequiredFieldValidator RFVddlPrimeAccountNumber = (RequiredFieldValidator)grvPaymentDetails.FooterRow.FindControl("RFVddlPrimeAccountNumber");
                RequiredFieldValidator RFVddlFooterFlowType = (RequiredFieldValidator)grvPaymentDetails.FooterRow.FindControl("RFVddlFooterFlowType");
                UserControls_S3GAutoSuggest ddlPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                ddlPrimeAccountNumber.IsMandatory =
                    //RFVddlPrimeAccountNumber.Enabled =
                    RFVddlFooterFlowType.Enabled = blnflag;
            }
            if (grvPaymentAdjustment.FooterRow != null)
            {
                //RequiredFieldValidator RFVddlPrimeAccountNumberA = (RequiredFieldValidator)grvPaymentAdjustment.FooterRow.FindControl("RFVddlPrimeAccountNumber");
                UserControls_S3GAutoSuggest ddlPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");

                RequiredFieldValidator RFVddlFooterFlowTypeA = (RequiredFieldValidator)grvPaymentAdjustment.FooterRow.FindControl("RFVddlFooterFlowType");
                //ddlPrimeAccountNumber.IsMandatory =
                //RFVddlPrimeAccountNumberA.Enabled = 
                RFVddlFooterFlowTypeA.Enabled = blnflag;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunDisableGLSLCodeGrid(bool blnflag)
    {
        try
        {
            if (grvPaymentDetails.FooterRow != null)
            {
                DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
                UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
                //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
                TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");

                ddlFooterGL_Code.Enabled = ddlFooterSL_Code.Enabled = blnflag;
                if (ddlPayTo.SelectedValue == "50")
                {
                    txtFooterAmount.ReadOnly = true;
                    if (chkAccountBased.SelectedValue == "1")
                        txtFooterAmount.ReadOnly = false;
                }
                else if (ddlPayTo.SelectedValue == "11")
                {
                    // --code commented and added by saran in 01-Aug-2014 Insurance start 

                    //txtFooterAmount.ReadOnly = true;
                    // --code commented and added by saran in 01-Aug-2014 Insurance end 

                }
                else
                {
                    txtFooterAmount.ReadOnly = false;
                }

            }
            if (grvPaymentAdjustment.FooterRow != null)
            {
                DropDownList ddlFooterGL_CodeA = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterGL_Code");
                UserControls_S3GAutoSuggest ddlFooterSL_CodeA = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
                //DropDownList ddlFooterSL_CodeA = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
                ddlFooterGL_CodeA.Enabled = ddlFooterSL_CodeA.Enabled = blnflag;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriBindGridDtls(Int32 intOption, DataTable dtObj)
    {
        try
        {
            if (intOption == 1)                     //Bind Load Po Invoice Details Grid
            {
                grvLoadInvoiceDtl.DataSource = dtObj;
                grvLoadInvoiceDtl.DataBind();
            }
            else if (intOption == 2)                     //Bind Load Payment Invoice Details Grid
            {
                grvPaymentInvoiceDtl.DataSource = dtObj;
                grvPaymentInvoiceDtl.DataBind();

                FunPriSetSearchValue();

                Pagenavigator1.Navigation(Convert.ToInt32(ViewState["TotRec"]), ProPageNumRW1, ProPageSizeRW1);
                Pagenavigator1.setPageSize(ProPageSizeRW1);
            }
            else if (intOption == 3)                     //Bind Load Payment Grid
            {
                grvPaymentDetails.DataSource = dtObj;
                grvPaymentDetails.DataBind();
            }



        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunProPANumSelectedIndexChange()
    {
        try
        {
            if (grvPaymentDetails.FooterRow != null)
            {
                UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
                FunClearPaymentdtlgridfooter();

                DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
                if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 8)
                {
                    ddlFooterSubAccountNumber.Items.Clear();
                }
                else
                {
                    ddlFooterSubAccountNumber.Items.Insert(0, Convert.ToString(ddlFooterPrimeAccountNumber.SelectedText) + "Dummy");
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private bool FunPrichkAcctbasedorgeneral(string primeacctno, string subacctno, string flowtype)
    {
        try
        {
            bool Rtrn = false;
            DataTable dtpmtdtls = new DataTable();
            if (ViewState["grvPaymentDetails"] != null)
                dtpmtdtls = (DataTable)ViewState["grvPaymentDetails"];
            if (primeacctno == "0")
                primeacctno = "";
            if (subacctno == "0")
                subacctno = "";
            if (flowtype == "0")
                flowtype = "";

            if (dtpmtdtls.Rows.Count == 0 || (dtpmtdtls.Rows.Count == 1 && Convert.ToString(dtpmtdtls.Rows[0]["CashFlow_Description"]) == "" && Convert.ToString(dtpmtdtls.Rows[0]["GL_Code"]) == ""))
            {
                Rtrn = false;
            }
            else
            {
                if (!((Convert.ToString(dtpmtdtls.Rows[0]["PANum"]).Length > 0 && primeacctno.Length > 0) ||
                   (Convert.ToString(dtpmtdtls.Rows[0]["PANum"]).Length == 0 && primeacctno.Length == 0)))
                {
                    Rtrn = true;
                }

            }
            return Rtrn;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private bool FunPriCheckGLSLSum()
    {
        bool blnRslt = true;
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Int32 _intGLOption = 1;

            Procparam.Add("@XML_FunderPmtDtl", Utility.FunPubFormXml((DataTable)ViewState["FunderPmtDtl"], true));
            Procparam.Add("@Payment_ID", Convert.ToString(strRequestID));
            if (Convert.ToString(strRequestID) != "")
            {
                if (Convert.ToInt64(strRequestID) > 0)
                {
                    _intGLOption = 2;
                }
            }
            Procparam.Add("@Option", Convert.ToString(_intGLOption));

            DataTable dtPmt = Utility.GetDefaultData("S3G_LAD_CheckPmtGLSLSum", Procparam);
            if (Convert.ToInt32(dtPmt.Rows[0]["Error_Code"]) == 1)
            {
                blnRslt = false;
            }
        }
        catch (Exception objException)
        {
            blnRslt = false;
            throw objException;
        }
        return blnRslt;
    }

    private void FunPriLoadPaytypeingrid(string strPayto)
    {
        try
        {
            string strProcName = "S3G_LOANAD_GetPaymentTypedetails";
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ol")
            {
                Procparam.Add("@LOB_Code", "OL");
            }
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            DropDownList ddlFooterPayType = ((DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType"));
            switch (strPayto.ToLower())
            {
                case "50":
                    Procparam.Add("@Option", "1");
                    if (grvPaymentDetails.FooterRow != null)
                    {
                        ddlFooterFlowType.BindDataTable(strProcName, Procparam, new string[] { "ID", "Name" });
                    }
                    if (grvPaymentAdjustment.FooterRow != null)
                    {
                        ddlFooterPayType.BindDataTable(strProcName, Procparam, new string[] { "ID", "Name" });
                    }
                    break;
                case "1":
                    Procparam.Add("@Option", "2");
                    if (grvPaymentDetails.FooterRow != null)
                    {
                        ddlFooterFlowType.BindDataTable(strProcName, Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });
                    }
                    if (grvPaymentAdjustment.FooterRow != null)
                    {
                        Procparam.Add("@Adjustmentvalue", "1");
                        ddlFooterPayType.BindDataTable(strProcName, Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });
                    }
                    break;
                case "11":
                    Procparam.Add("@Option", "5");
                    if (grvPaymentDetails.FooterRow != null)
                    {
                        ddlFooterFlowType.BindDataTable(strProcName, Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });
                    }
                    if (grvPaymentAdjustment.FooterRow != null)
                    {
                        Procparam.Add("@Adjustmentvalue", "1");
                        ddlFooterPayType.BindDataTable(strProcName, Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });
                    }
                    break;
                case "13":              //Funder
                    Procparam.Add("@Option", "6");
                    if (grvPaymentDetails.FooterRow != null)
                    {
                        ddlFooterFlowType.BindDataTable(strProcName, Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });
                    }
                    if (grvPaymentAdjustment.FooterRow != null)
                    {
                        Procparam.Add("@Adjustmentvalue", "1");
                        ddlFooterPayType.BindDataTable(strProcName, Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });
                    }
                    break;
                default:
                    Procparam.Add("@Option", "3");

                    if (grvPaymentDetails.FooterRow != null)
                    {
                        ddlFooterFlowType.BindDataTable(strProcName, Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });
                    }
                    if (grvPaymentAdjustment.FooterRow != null)
                    {
                        Procparam.Add("@Adjustmentvalue", "1");
                        ddlFooterPayType.BindDataTable(strProcName, Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });
                    }
                    break;

            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriInsertDeleteInvoiceDtl(Int32 intOption, string strXMLInvoiceDtl, Int64 intPODtlID, Int64 intgdinvPIID, Int64 intgdinvVIID)
    {
        try
        {
            int intTotalRecords = 0;
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", Convert.ToString(intOption));
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));
            Procparam.Add("@CurrentPage", ProPageNumRW1.ToString());
            Procparam.Add("@PageSize", ProPageSizeRW1.ToString());
            Procparam.Add("@SearchValue", hdnSearch.Value);
            Procparam.Add("@OrderBy", hdnOrderBy.Value);
            Procparam.Add("@TotalRecords", ViewState["TotRec"] == null ? intTotalRecords.ToString() : Convert.ToInt32(ViewState["TotRec"]).ToString());
            if (intOption == 2)
            {
                Procparam.Add("@Po_Det_ID", Convert.ToString(intPODtlID));
                //Added for Call Id : 5513
                if (intgdinvPIID != 0 && intgdinvPIID != null)
                    Procparam.Add("@PI_Det_ID", Convert.ToString(intgdinvPIID));
                if (intgdinvVIID != 0 && intgdinvVIID != null)
                    Procparam.Add("@VI_Det_ID", Convert.ToString(intgdinvVIID));
            }
            if (intOption == 1)
                Procparam.Add("@XML_InvoiceDetails", Convert.ToString(strXMLInvoiceDtl));
            FunPriAddCmnParam();

            Procparam.Add("@Is_Added", "1");    //Added on 15Jun2015 for OPC Req

            DataSet dsInvoice = Utility.GetDataset("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);

            ViewState["PaymentInvoiceDetails"] = dsInvoice.Tables[0];
            if (dsInvoice.Tables[0] != null && dsInvoice.Tables[0].Rows.Count > 0)
            {
                ViewState["TotRec"] = dsInvoice.Tables[3].Rows[0]["@TotalRecords"].ToString();
            }
            else
            {
                btnremoveall.Visible = false;
                ViewState["TotRec"] = null;
            }
            FunPriBindGridDtls(2, dsInvoice.Tables[0]);

            ViewState["grvPaymentDetails"] = dsInvoice.Tables[1];
            FunPriBindGridDtls(3, dsInvoice.Tables[1]);
            grvPaymentDetails.FooterRow.Visible = false;
            grvPaymentDetails.Columns[9].Visible = false;

            if (dsInvoice.Tables[0].Rows.Count == 0)
            {
                lblPaymentDetailsTotal.Text = lbltotalPaymentAdjust.Text = "0";
                txtDocAmount.Text = Math.Round(Convert.ToDecimal(lbltotalPaymentAdjust.Text), 0).ToString();
            }

            if (dsInvoice != null && dsInvoice.Tables.Count > 1)
                FunPriDisplaySmryGridTotal(dsInvoice.Tables[2]);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void fundisablepaymentdtlsgrid()
    {
        try
        {
            ViewState["grvPaymentDetails"] = (DataTable)ViewState["DefaultPaymentDetails"];
            FunPriBindGridDtls(3, (DataTable)ViewState["DefaultPaymentDetails"]);
            grvPaymentDetails.Rows[0].Visible = false;
            FunpriBlank();
            FunPriLoadPANUM();
            FunPriLoadGLcodes();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnblDsblGrid()
    {
        try
        {
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 5 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 8)
            {
                grvFunderReceipt.Visible = true;
                grvPaymentDetails.Visible = false;
            }
            else
            {
                grvFunderReceipt.Visible = false;
                grvPaymentDetails.Visible = true;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private bool FuncalculateDocAmount()
    {
        bool boolflag = true;
        try
        {
            if (Convert.ToDecimal(lbltotalPaymentAdjust.Text) > 0)
            {
                if (Math.Round(Convert.ToDecimal(lbltotalPaymentAdjust.Text), 0) != Convert.ToDecimal(txtDocAmount.Text))
                    boolflag = false;
            }
            else if (Convert.ToDecimal(lblPaymentDetailsTotal.Text) > 0)
            {
                if (Math.Round(Convert.ToDecimal(lblPaymentDetailsTotal.Text), 0) != Convert.ToDecimal(txtDocAmount.Text))
                    boolflag = false;
            }
        }
        catch (Exception objException)
        {
            boolflag = false;
        }
        return boolflag;
    }

    private void fundisablecontrols(string qsmode)
    {
        try
        {
            PnlCustEntityInformation.Enabled = ucCustomerCodeLov.ButtonEnabled = (Convert.ToString(ddlPayTo.SelectedValue) != "50") ? false : true;
            switch (qsmode)
            {
                case "Q":
                    PanelPayType.Enabled = ddlPaymentType.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancelPayment.Visible = false;
                    // ddlPayMode.ClearDropDownList();
                    ddlCurrencyCode.ClearDropDownList();
                    chkAccountBased.ClearDropDownList();
                    CalendarExtenderPaymentRequestDate.Enabled =
                    CalendarExtenderValueDate.Enabled = false;
                    //imgPaymentRequestDate.Visible = false;
                    txtDocAmount.ReadOnly = true;
                    if (PanelPaymentAdjustment != null && PanelPaymentAdjustment.Visible)
                    {
                        //ddlAddOrLess.ClearDropDownList();
                        //ddlPaytypeadjust.ClearDropDownList();
                    }
                    PnlCustEntityInformation.Enabled = false;
                    ucCustomerCodeLov.ButtonEnabled = false;
                    if (grvPaymentDetails != null)
                    {
                        grvPaymentDetails.Columns[grvPaymentDetails.Columns.Count - 1].Visible = false;
                        for (int rowcount = 0; rowcount < grvPaymentDetails.Rows.Count; rowcount++)
                        {
                            TextBox lblDescription = (TextBox)grvPaymentDetails.Rows[rowcount].FindControl("lblDescription");
                            TextBox lblAmount = (TextBox)grvPaymentDetails.Rows[rowcount].FindControl("lblAmount");
                            lblDescription.ReadOnly = true;
                            lblAmount.ReadOnly = true;

                        }
                    }
                    if (grvPaymentAdjustment != null)
                    {
                        grvPaymentAdjustment.Columns[grvPaymentAdjustment.Columns.Count - 1].Visible = false;
                    }
                    if (grvPaymentInvoiceDtl != null)
                    {
                        grvPaymentInvoiceDtl.Columns[grvPaymentInvoiceDtl.Columns.Count - 1].Visible = false;
                    }
                    break;
                case "M":
                    //CalendarExtenderValueDate.Enabled =
                    CalendarExtenderPaymentRequestDate.Enabled = false;
                    if (PanelPaymentAdjustment != null && PanelPaymentAdjustment.Visible)
                    {
                        //ddlAddOrLess.ClearDropDownList();
                        //ddlPaytypeadjust.ClearDropDownList();
                    }
                    PnlCustEntityInformation.Enabled = false;
                    ucCustomerCodeLov.ButtonEnabled = false;
                    txtInstrumentDate.Attributes.Add("readonly", "readonly");
                    btnShow.Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) > 0 && Convert.ToInt32(ddlPaymentType.SelectedValue) < 5) ? true : false;


                    grvPaymentDetails.FooterRow.Visible =
                    grvPaymentDetails.Columns[9].Visible = (Convert.ToInt32(ddlPaymentType.SelectedValue) > 0 && Convert.ToInt32(ddlPaymentType.SelectedValue) < 5) ? false : true;

                    break;
                default:
                    //TabPanelPBD.Enabled = false;

                    break;

            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnableDisableAdjsumentGrid()
    {
        try
        {
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 6 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 7)
            {
                grvPaymentAdjustment.Enabled = true;
            }
            else
            {
                grvPaymentAdjustment.Enabled = false;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriCalcSumAmountDetails()
    {
        try
        {
            decimal _SumAmt = 0;
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 5 || Convert.ToInt32(ddlPaymentType.SelectedValue) == 8)
            {
                _SumAmt = Convert.ToDecimal(((DataTable)ViewState["FunderPmtDtl"]).Compute("sum(Amount)", "Amount >=0"));
            }
            else
            {
                if (grvPaymentDetails != null)
                {
                    foreach (GridViewRow grvPaymentDetailsRow in grvPaymentDetails.Rows)
                    {
                        TextBox lblAmount = (TextBox)grvPaymentDetailsRow.FindControl("lblAmount");

                        if (!(string.IsNullOrEmpty(lblAmount.Text)))
                            _SumAmt += (Convert.ToDecimal(lblAmount.Text));

                    }
                }
            }
            lblPaymentDetailsTotal.Text = Convert.ToString(_SumAmt);
            lbltotalPaymentAdjust.Text = Convert.ToString(_SumAmt);
            txtDocAmount.Text = Math.Round(Convert.ToDecimal(lbltotalPaymentAdjust.Text), 0).ToString();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriCalcSumAmount()
    {
        try
        {
            if (grvPaymentAdjustment != null)
            {
                decimal sum = 0;
                decimal Total = 0;
                foreach (GridViewRow grvPaymentDetailsRow in grvPaymentAdjustment.Rows)
                {
                    Label lblAddOrLess = (Label)grvPaymentDetailsRow.FindControl("lblAddOrLess");
                    Label lblAmount = (Label)grvPaymentDetailsRow.FindControl("lblAmount");


                    if (!(string.IsNullOrEmpty(lblAmount.Text)))
                        sum = (Convert.ToDecimal(lblAmount.Text));
                    if (lblAddOrLess.Text.ToLower() == "add")
                    {
                        Total += sum;
                    }
                    else if (lblAddOrLess.Text.ToLower() == "less")
                    {
                        Total -= sum;
                    }
                    lbltotalPaymentAdjust.Text = (Convert.ToDecimal(lblPaymentDetailsTotal.Text) + Total).ToString();
                    txtDocAmount.Text = Math.Round(Convert.ToDecimal(lbltotalPaymentAdjust.Text), 0).ToString();
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnblDsblINVGridClmn()
    {
        try
        {
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1)
            {
                grvLoadInvoiceDtl.Columns[13].Visible = false;
            }
            else
            {
                grvLoadInvoiceDtl.Columns[13].Visible = true;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLockControls(bool blnLock)
    {
        try
        {
            // other controls (other than textboxes)
            ddlLOB.Enabled =
            ddlBranch.Enabled =

            imgValueDate.Visible =

            ddlPayMode.Enabled =

            chkAccountBased.Enabled =
            ddlPayTo.Enabled =
            btnSave.Enabled =
            btnCancelPayment.Enabled =
            blnLock;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunOpenPDF()
    {
        string strHTML = FunPriGeneratePDF();

        string objGuid = System.Guid.NewGuid().ToString();
        string strnewFile = (Server.MapPath(".") + "\\PDF Files\\PmtVoucher_" + strRequestID + ".pdf");
        string strnewFile1 = "/LoanAdmin/PDF Files/" + "PmtVoucher_" + strRequestID + ".pdf";

        try
        {
            if (File.Exists(strnewFile) == true)
            {
                File.Delete(strnewFile);
            }
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));
            doc.AddCreator(ObjUserInfo.ProCompanyNameRW.ToString());
            doc.AddTitle("PmtVoucher_" + strRequestID);
            doc.Open();
            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(strHTML), null).Cast<IElement>().ToList();
            for (int k = 0; k < htmlarraylist.Count; k++)
            { doc.Add((IElement)htmlarraylist[k]); }
            doc.AddAuthor("S3G Team");
            doc.Close();
            //System.Diagnostics.Process.Start(strnewFile);
            string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strnewFile1 + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception objException)
        {
            throw objException;
            System.Diagnostics.Process.Start(strnewFile);
        }
    }

    private string FunPriGeneratePDF()
    {
        StringBuilder strHTML = new StringBuilder();
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Payment_ID", strRequestID);
            Procparam.Add("@Option", "1");
            DataSet dsPmtVchr = Utility.GetDataset("S3G_LAD_PrintPmtVchr_OPC", Procparam);

            DataTable dtCmpdtl = dsPmtVchr.Tables[0];
            DataTable dtGridTbl = dsPmtVchr.Tables[2];
            DataTable dtApproveddtl = dsPmtVchr.Tables[4];
            DataRow drHDR = dtCmpdtl.Rows[0];
            strHTML.Append(" <font size=\"1\"  color=\"black\" face=\"verdana\">" +
                 "<table width=\"85%\">" +
                     "<tr>" +
                        "<td width=\"100%\" align=\"right\">" +
                //"<b>" + Convert.ToString(drHDR["Duplicate"]) + "</b>" +
                         "<b>" + " " + "</b>" +
                         "</td>" +
                    "</tr>" +
                "</table>" +
                "<table width=\"85%\">" +
                "<tr>" +
                "<td width=\"100%\">" +
                    "<table border=\"0\" width=\"100%\">" +
                    "<tr>" +
                        "<td align=\"center\">" +
                           "<b>" + drHDR["COMPANY_NAME"].ToString() + "</b>" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td align=\"center\">" +
                            "<b>" + drHDR["Address"] + "</b>" +
                            "<br />" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align=\"center\">" +
                        "<b> ");

            dtCmpdtl = dsPmtVchr.Tables[1];
            drHDR = dtCmpdtl.Rows[0];            
            string country = dtCmpdtl.Rows[0]["Country"].ToString();           

            string countryval = country.Split('-').Last();
            if (countryval == " ")
            {
                country = country.Substring(0, country.LastIndexOf(" - "));
            }
            
            strHTML.Append("</b>" +
                "</td>" +
            "</tr>" +
        "</table>" +
    "</td>" +
"</tr>" +
"<tr>" +
    "<td>" +
        "<table width=\"100%\">" +
            "<tr>" +
                "<td align=\"left\" valign=\"top\">" +
                  "<table align=\"left\" valign=\"top\" cellpadding=\"0\" cellspacing=\"0\">" +
                     "<tr>" +
                        "<td>" +
                            drHDR["Payee_Name"].ToString() +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>" +
                            drHDR["Address"].ToString() +
                        "</td>" +
                   " </tr>" +
                    "<tr>" +
                        "<td>" +
                            drHDR["City"].ToString() +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>" +
                            drHDR["State"].ToString() +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>" +
                        country+
                            //drHDR["Country"].ToString() +
                        "</td>" +
                    "</tr>" +
                    "</table>" +
                  "</td>" +
                  "<td>" +
                   "<table width=\"100%\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\">" +
                //      "<tr>" +
                //             "<td align=\"left\">" +
                //                "Location Name" +
                //             "</td>" +
                //             "<td align=\"center\">" +
                //                ":" +
                //             "</td>" +
                //             "<td align=\"left\">" + //drHDR["LOCATION_CODE"].ToString() + " - " + 
                //               drHDR["Location_Desc"].ToString() +
                //             "</td>" +
                //     "</tr>" +
                //      "<tr>" +
                //        "<td align=\"left\">" +
                //            "Line of Business" +
                //        "</td>" +
                //        "<td align=\"center\">" +
                //           " :" +
                //        "</td>" +
                //        "<td align=\"left\">" +
                //           drHDR["LOB_Name"].ToString() +
                //        "</td>" +
                //"</tr>" +
                     "<tr>" +
                           "<td align=\"left\">" +
                             "Voucher Number" +
                           "</td>" +
                           "<td align=\"center\">" +
                             ":" +
                           "</td>" +
                           "<td align=\"left\">" +
                            drHDR["Voucher_No"].ToString() +
                           "</td>" +
                    "</tr>" +
                    "<tr>" +
                          "<td align=\"left\">" +
                            "Date" +
                          "</td>" +
                          "<td align=\"center\">" +
                            ":" +
                          "</td>" +
                          "<td align=\"left\">" +
                            drHDR["Payment_Date"].ToString() +
                          "</td>" +
                  "</tr>" +

                           "</table>" +
                       " </td>" +
                      "</tr>" +
                    "</table>" +
                  " </td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table width=\"100%\">" +
                            "<tr>" +
                                "<td>" +
                                    "Payment in favour of M/s./Mr./Mrs./Ms. " + drHDR["Payee_Name"].ToString() +
                                    " a sum of Rs." + drHDR["Payment_Amount"].ToString() + " (Rupees " +
                                    Convert.ToDecimal(drHDR["Payment_Amount"].ToString()).GetAmountInWords() + ") " +
                                    " by " + drHDR["Payment_Mode_Desc"].ToString());

            if (!string.IsNullOrEmpty(drHDR["INSTRUMENT_NO"].ToString()))
            {
                strHTML.Append(" number " + drHDR["INSTRUMENT_NO"].ToString());
            }
            if (!string.IsNullOrEmpty(drHDR["INSTRUMENT_DATE"].ToString()))
            {
                strHTML.Append(" dated " + drHDR["INSTRUMENT_DATE"].ToString());// + 
                if (ddlPayMode.SelectedItem.Text == "NEFT")
                {
                    strHTML.Append(" drawn from ");
                }
                else
                {
                    strHTML.Append(" drawn on ");
                }

                //+ 
                strHTML.Append(drHDR["DRAWEE_BANK_Name"].ToString() +
                " " + drHDR["DRAWEE_BANK_LOCATION"].ToString());
            }
            strHTML.Append(" towards payment as given below." +

                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>");

            string sno = "";
            string RS_no = "";
            string particular = "";
            string tranche = "";
            string invoice = "";
            string DB_no = "";
            string inv_dt = "";
            string inv_amt = "";
            string tds = "";
            string wct = "";
            string ret = "";
            string ADv_paid = "";
            string tot_amt = "";

            switch ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString()))
            {
                case "1"://Payment to Vendor based on invoices (Direct payment by OPC)
                    sno = "Width=3%";
                    RS_no = "Width=7%";
                    particular = "Width=13%";
                    tranche = "Width=8%";
                    invoice = "Width=8%";
                    DB_no = "Width=6%";
                    inv_dt = "Width=10%";
                    inv_amt = "Width=10%";
                    tds = "Width=5%";
                    wct = "Width=5%";
                    ret = "Width=5%";
                    ADv_paid = "Width=10%";
                    tot_amt = "Width=10%";
                    break;
                case "3"://Payment to Vendor after getting Lessee Purchase advance
                    sno = "Width=3%";
                    RS_no = "Width=8%";
                    particular = "Width=15%";
                    invoice = "Width=11%";
                    DB_no = "Width=8%";
                    inv_dt = "Width=10%";
                    inv_amt = "Width=10";
                    tds = "Width=5%";
                    wct = "Width=5%";
                    ret = "Width=5%";
                    ADv_paid = "Width=10%";
                    tot_amt = "Width=10%";
                    break;
                case "4"://Payment to Lessee after getting Funder amount (Refund of advance)
                    sno = "Width=3%";
                    RS_no = "Width=7%";
                    particular = "Width=13%";
                    tranche = "Width=8%";
                    invoice = "Width=8%";
                    DB_no = "Width=6%";
                    inv_dt = "Width=10%";
                    inv_amt = "Width=10%";
                    tds = "Width=5%";
                    wct = "Width=5%";
                    ret = "Width=5%";
                    ADv_paid = "Width=10%";
                    tot_amt = "Width=10%";
                    break;

                case "5"://Payment to Funder after getting Lessee amount (Lease Rental)
                    sno = "Width=5%";
                    RS_no = "Width=12%";
                    particular = "Width=25%";
                    tranche = "Width=17%";
                    invoice = "Width=17%";
                    ADv_paid = "Width=12%";
                    tot_amt = "Width=12%";
                    break;

                case "6"://General Payment (GL based)
                    sno = "Width=2%";
                    particular = "Width=62%";
                    tot_amt = "Width=30%";
                    break;
                case "7"://Entity Payment
                    sno = "Width=%";
                    particular = "Width=62%";
                    tot_amt = "Width=30%";
                    break;

                case "8"://Payment to Funder Against Note
                    sno = "Width=5%";
                    RS_no = "Width=10%";
                    particular = "Width=45%";
                    tranche = "Width=20%";
                    ADv_paid = "Width=10%";
                    tot_amt = "Width=10%";
                    break;

            }



            strHTML.Append(
                "<tr>" +
                "<td>" +
                "<table width=\"130%\" style=\"border-color: Black;\" border=\"0.5\" cellpadding=\"0\" cellspacing=\"0\">" +
                    "<tr>" +
                       "<td align=\"center\" " + sno + ">" +
                            "<font face=\"verdana\" size=\"1\"><b>Sl.No </b></font>" +
                        "</td>");// +
            if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")))
            {
                strHTML.Append("<td align=\"center\" " + RS_no + ">" +
                    "<font face=\"verdana\" size=\"1\"><b>Rental Schedule No</b></font>" +
                "</td>");//+
            }
            strHTML.Append("<td align=\"center\" " + particular + ">" +
                "<font face=\"verdana\" size=\"1\"><b>Particulars</b></font>" + /* Changed by PSK on 19-Jan-2011 - For UAT Issue No.RCT_006 */
            "</td>");// +
            if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("3")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7"))
                && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")))
            {
                strHTML.Append("<td align=\"center\" " + tranche + ">" +
                    "<font face=\"verdana\" size=\"1\"><b>Tranche No</b></font>" +
                "</td>");// +
            }
            if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7"))
                && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")))
            {
                strHTML.Append("<td align=\"center\" " + invoice + ">" +
                  "<font face=\"verdana\" size=\"1\"><b>Invoice No</b></font>" +
              "</td>");// +
            }
            if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7"))
                && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")))
            {
                strHTML.Append("<td align=\"center\" " + DB_no + ">" +
                    "<font face=\"verdana\" size=\"1\"><b>Debit Note Number</b></font>" +
                "</td>");// +
            }
            if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7"))
                && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")))
            {
                strHTML.Append("<td align=\"center\" " + inv_dt + ">" +
                    "<font face=\"verdana\" size=\"1\"><b>Invoice Date</b></font>" +
                "</td>");// +
            }
            if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7"))
                && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")))
            {
                strHTML.Append("<td align=\"center\" " + inv_amt + ">" +
                    "<font face=\"verdana\" size=\"1\"><b>Invoice Amount</b></font>" +
                "</td>");// +
            }
            if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7"))
                && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")))
            {
                strHTML.Append("<td align=\"center\" " + tds + ">" +
                     "<font face=\"verdana\" size=\"1\"><b>TDS</b></font>" +
                  "</td>");//+
            }
            if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7"))
                 && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")))
            {
                strHTML.Append("<td align=\"center\" " + wct + ">" +
                   "<font face=\"verdana\" size=\"1\"><b>WCT</b></font>" +
               "</td>" +
                "<td align=\"center\" >" +
                   "<font face=\"verdana\" size=\"1\"><b>Retention</b></font>" +
               "</td>");// +
            }
            if (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6") && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")))
            {
                strHTML.Append("<td align=\"center\" " + ADv_paid + ">" +
                   "<font face=\"verdana\" size=\"1\"><b>Advance Paid Amount</b></font>" +
               "</td>"); //+
            }
            strHTML.Append("<td align=\"center\" " + tot_amt + ">" +
                            "<font face=\"verdana\" size=\"1\"><b> Total Amount Paid</b></font>" +
                        "</td>" +
                    "</tr>");

            for (int i = 0; i < dtGridTbl.Rows.Count; i++)
            {

                strHTML.Append("<tr>" +
             "<td align=\"center\" " + sno + ">" +
                    //"<td align=\"center\"width=\"1\">" +
                (i + 1).ToString() +
                "</td>");// +
                if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")))
                {
                    strHTML.Append("<td align=\"center\" " + RS_no + ">" +
                    dtGridTbl.Rows[i]["Panum"].ToString() +
                    "</td>");// +
                }
                strHTML.Append("<td align=\"center\" " + particular + ">" +
                dtGridTbl.Rows[i]["AccountDescription"].ToString() +
                "</td>");// +                
                if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("3")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")))
                {
                    strHTML.Append("<td align=\"center\" " + tranche + ">" +
                    dtGridTbl.Rows[i]["Tranche_no"].ToString() +
                    "</td>");// +
                }
                if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")))
                {
                    strHTML.Append("<td align=\"center\" " + invoice + ">" +
                    dtGridTbl.Rows[i]["Invoice_no"].ToString() +
                    "</td>");// +
                }
                if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6"))
                    && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")))
                {
                    strHTML.Append("<td align=\"center\" " + DB_no + ">" +
                    dtGridTbl.Rows[i]["Note_no"].ToString() +
                    "</td>");// +
                }
                if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6"))
                    && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")))
                {
                    strHTML.Append("<td align=\"center\" " + inv_dt + ">" +
                    dtGridTbl.Rows[i]["INv_date"].ToString() +
                    "</td>");// +
                }
                if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6"))
                    && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")))
                {
                    strHTML.Append("<td align=\"right\" " + inv_amt + ">" +
                   dtGridTbl.Rows[i]["Inv_amt"].ToString() +
                   "</td>");// +
                }
                if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6"))
                    && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")))
                {
                    strHTML.Append("<td align=\"right\" " + tds + ">" +
                   dtGridTbl.Rows[i]["Tds_amt"].ToString() +
                   "</td>");// +
                }
                if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("5")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6")) && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7"))
                    && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("8")))
                {
                    strHTML.Append("<td align=\"right\" " + wct + ">" +
                    dtGridTbl.Rows[i]["wct_amt"].ToString() +
                    "</td>" +
                     "<td align=\"right\" " + ret + ")>" +
                    dtGridTbl.Rows[i]["Rtention"].ToString() +
                    "</td>"); //+
                }

                if (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("6") && (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() != ("7")))
                {
                    strHTML.Append("<td align=\"right\" " + ADv_paid + ">" +
                    dtGridTbl.Rows[i]["Adv_pay_amt"].ToString() +
                    "</td>");// +
                }
                strHTML.Append("<td align=\"right\" " + tot_amt + ">" +
                dtGridTbl.Rows[i]["Transaction_Amount"].ToString() +
                "</td>" +
                "</tr>");
            }

            DataTable dtGridTblAddLess = dsPmtVchr.Tables[3];

            if (dtGridTblAddLess.Rows.Count > 0)
            {
                strHTML.Append(
                  "</table>" +
                    "</td>" +
                    "</tr>");
                strHTML.Append(
                        "<tr>" +
      "<td>Add/Less Details : </td></tr>" +

                      "<tr>" +
      "<td>" +
          "<table width=\"100%\" style=\"border-color: Black;\" border=\"0.5\" cellpadding=\"4\" cellspacing=\"0\">" +
           "<tr>" +
                  "<td align=\"center\" >" +
                      "<font face=\"verdana\" size=\"1\"><b>Sl.No </b></font>" +
                  "</td>" +
                    "<td align=\"center\" >" +
                      "<font face=\"verdana\" size=\"1\"><b>Add or Less</b></font>" +
                  "</td>" +
                  "<td align=\"center\">" +
                      "<font face=\"verdana\" size=\"1\"><b>Rental Schedule No</b></font>" +
                  "</td>" +
                  "<td align=\"center\" colspan=\"2\">" +
                       "<font face=\"verdana\" size=\"1\"><b>Particulars</b></font>" +
                  "</td>" +
                  "<td align=\"center\">" +
                      "<font face=\"verdana\" size=\"1\"><b>Amount</b></font>" +
                  "</td>" +
                      "</tr>");

                for (int i = 0; i < dtGridTblAddLess.Rows.Count; i++)
                {

                    strHTML.Append("<tr>" +
                       "<td align=\"center\">" +
                           (i + 1).ToString() +
                       "</td>" +
                        "<td>" +
                           dtGridTblAddLess.Rows[i]["Add_Less_Desc"].ToString() +
                       "</td>" +
                       "<td>" +
                           dtGridTblAddLess.Rows[i]["Panum"].ToString() +
                       "</td>" +
                       "<td colspan=\"2\">" +
                           dtGridTblAddLess.Rows[i]["AccountDescription"].ToString() +
                       "</td>" +
                       "<td align=\"right\">" +
                           dtGridTblAddLess.Rows[i]["Transaction_Amount"].ToString() +
                       "</td>" +
                   "</tr>");
                }
                strHTML.Append(
                    "<tr>" +
                        "<td>" +
                        "</td>" +
                        "<td>" +
                        "</td>" +
                        "<td>" +
                        "</td>" +
                    // "<td>" +
                    //"</td>" +
                        "<td colspan=\"2\">" +
                            "Total Amount" +
                        "</td>" +
                        "<td align=\"right\">" +
                            drHDR["Payment_Amount"].ToString() +
                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>");

            }
            else
            {
                if (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() == ("8"))
                {
                    strHTML.Append(
                    "<tr>" +


                    "<td align=\"right\"colspan=\"5\">" +
                    "Total Amount " +
                    "</td>" +
                    "<td align=\"right\">" +
                    drHDR["Payment_Amount"].ToString() +
                    "</td>" +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>");
                }
                else if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() == ("7")))
                {
                    strHTML.Append(
                    "<tr>" +
                        "<td width=\"1\">" +
                        "</td>" +
                    "<td align=\"right\">" +
                    "Total Amount" +
                    "</td>" +
                    "<td align=\"right\">" +
                    drHDR["Payment_Amount"].ToString() +
                    "</td>" +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>");
                }
                else if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() == ("6")))
                {
                    strHTML.Append(
                    "<tr>" +
                        "<td width=\"1\">" +
                        "</td>" +
                    "<td align=\"right\">" +
                    "Total Amount" +
                    "</td>" +
                    "<td align=\"right\">" +
                    drHDR["Payment_Amount"].ToString() +
                    "</td>" +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>");
                }
                else if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() == ("5")))
                {
                    strHTML.Append(
                    "<tr>" +
                    "<td align=\"right\"colspan=\"6\">" +
                    "Total Amount" +
                    "</td>" +
                    "<td align=\"right\">" +
                    drHDR["Payment_Amount"].ToString() +
                    "</td>" +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>");
                }
                else if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() == ("4")) || (dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() == ("1")))
                {
                    strHTML.Append(
                    "<tr>" +

                    "<td align=\"right\"colspan=\"12\">" +
                    "Total Amount" +
                    "</td>" +
                    "<td align=\"right\">" +
                    drHDR["Payment_Amount"].ToString() +
                    "</td>" +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>");
                }

                else if ((dtGridTbl.Rows[0]["Pay_Type_Code"].ToString() == ("3")))
                {
                    strHTML.Append(
                    "<tr>" +
                    "<td align=\"right\"colspan=\"11\">" +
                    "Total Amount" +
                    "</td>" +
                    "<td align=\"right\">" +
                    drHDR["Payment_Amount"].ToString() +
                    "</td>" +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>");
                }

            }

            strHTML.Append(
           "<tr>" +
               "<td>" +
                   "<table width=\"100%\">" +

                   "<tr>" +
                 "<td width=\"1\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +


              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +

              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
             "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
             "</tr>" +
             "<tr>" +
                 "<td width=\"300%\" colspan=\"2\">" +
                 "<table><tr><td>Prepared by</td><td>Approved by</td><td>Received payment</td></tr><tr><td>" + (dtGridTbl.Rows[0]["Prepared_By"].ToString()) + "</td><td>" + dtApproveddtl.Rows[0]["Approved_by"].ToString() + "</td><td></td></tr></table>" +
                 "</td>" +
             "</tr>" +
             "<tr>" +
                 "<td width=\"100%\" colspan=\"2\"></td>" +
             "</tr>" +
             // "<tr>" +
             //    "<td width=\"25%\" colspan=\"2\" align=\"left\">" + ObjUserInfo.ProUserNameRW.ToString() + "</td>" +
             //"</tr>" +
              "<tr>" +
                 "<td width=\"100%\" colspan=\"2\">" + DateTime.Now.ToString(strDateFormat) + "  " + DateTime.Now.ToLongTimeString() + "</td>" +
             "</tr>" +
             "</table>" +
               "</td>" +
           "</tr>" +
       "</table> </font>");
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return strHTML.ToString();
    }

    private string FunPriGenerateCoveringPDF()
    {
        StringBuilder strHTML = new StringBuilder();
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Payment_ID", strRequestID);
            Procparam.Add("@Option", "1");
            DataSet dsPmtVchr = Utility.GetDataset("S3G_LAD_PrintPmtVchr_OPC", Procparam);

            DataTable dtCmpdtl = dsPmtVchr.Tables[0];

            DataRow drHDR = dtCmpdtl.Rows[0];
            strHTML.Append(" <font size=\"1\"  color=\"black\" face=\"verdana\">" +
                 "<table width=\"85%\">" +
                     "<tr>" +
                        "<td width=\"100%\" align=\"right\">" +
                //"<b>" + Convert.ToString(drHDR["Duplicate"]) + "</b>" +
                         "<b>" + " " + "</b>" +
                         "</td>" +
                    "</tr>" +
                "</table>" +
                "<table width=\"85%\">" +
                "<tr>" +
                "<td width=\"100%\">" +
                    "<table border=\"0\" width=\"100%\">" +
                    "<tr>" +
                        "<td align=\"center\">" +
                           "<b>" + drHDR["COMPANY_NAME"].ToString() + "</b>" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td align=\"center\">" +
                            "<b>" + drHDR["Address"] + "</b>" +
                            "<br />" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align=\"center\">" +
                        "<b> ");

            dtCmpdtl = dsPmtVchr.Tables[1];
            drHDR = dtCmpdtl.Rows[0];

            strHTML.Append("</b>" +
                "</td>" +
            "</tr>" +
        "</table>" +
    "</td>" +
"</tr>" +
"<tr>" +
    "<td>" +
        "<table width=\"100%\">" +
            "<tr>" +
                "<td align=\"left\" valign=\"top\">" +
                  "<table align=\"left\" valign=\"top\" cellpadding=\"0\" cellspacing=\"0\">" +
                     "<tr>" +
                        "<td>" +
                            drHDR["Payee_Name"].ToString() +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>" +
                            drHDR["Address"].ToString() +
                        "</td>" +
                   " </tr>" +
                    "<tr>" +
                        "<td>" +
                            drHDR["City"].ToString() +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>" +
                            drHDR["State"].ToString() +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>" +
                            drHDR["Country"].ToString() +
                        "</td>" +
                    "</tr>" +
                    "</table>" +
                  "</td>" +
                  "<td>" +
                   "<table width=\"100%\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\">" +
                     "<tr>" +
                           "<td align=\"left\">" +
                             "Voucher Number" +
                           "</td>" +
                           "<td align=\"center\">" +
                             ":" +
                           "</td>" +
                           "<td align=\"left\">" +
                            drHDR["Voucher_No"].ToString() +
                           "</td>" +
                    "</tr>" +
                    "<tr>" +
                          "<td align=\"left\">" +
                            "Date" +
                          "</td>" +
                          "<td align=\"center\">" +
                            ":" +
                          "</td>" +
                          "<td align=\"left\">" +
                            drHDR["Payment_Date"].ToString() +
                          "</td>" +
                  "</tr>" +

                           "</table>" +
                       " </td>" +
                      "</tr>" +
                    "</table>" +
                  " </td>" +
                "</tr>" +
                "<tr>" +
                    "<td>" +
                        "<table width=\"100%\">" +
                            "<tr>" +
                                "<td>" +
                                    "It is our pleasure in enclosing our <b>" + Convert.ToString(drHDR["Payment_Mode_Desc"]) +
                                    " number - " + Convert.ToString(drHDR["INSTRUMENT_NO"]) + "</b> dated <b>" + Convert.ToString(drHDR["INSTRUMENT_DATE"]) +
                                    "</b> drawn on " + Convert.ToString(drHDR["DRAWEE_BANK_Name"]) + " " + Convert.ToString(drHDR["DRAWEE_BANK_Location"]) +
                                    " for INR <b>" + Convert.ToString(drHDR["Payment_Amount"]) + "</b> (Indian Rupees " +
                                    Convert.ToDecimal(drHDR["Payment_Amount"].ToString()).GetAmountInWords() + ") " +
                                    " towards payment as given below.");

            strHTML.Append("</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>");


            strHTML.Append(
                "<tr>" +
                "<td>" +
                "<table width=\"100%\" style=\"border-color: Black;\" border=\"1\" cellpadding=\"4\" cellspacing=\"0\">" +
                    "<tr>" +
                        "<td align=\"center\" >" +
                            "<font face=\"verdana\" size=\"1\"><b>Sl.No </b></font>" +
                        "</td>" +
                        "<td align=\"center\">" +
                            "<font face=\"verdana\" size=\"1\"><b>Rental Schedule No</b></font>" +
                        "</td>" +
                        "<td align=\"center\" colspan=\"2\">" +
                            "<font face=\"verdana\" size=\"1\"><b>Particulars</b></font>" + /* Changed by PSK on 19-Jan-2011 - For UAT Issue No.RCT_006 */
                        "</td>" +
                        "<td align=\"center\">" +
                            "<font face=\"verdana\" size=\"1\"><b>Amount</b></font>" +
                        "</td>" +
                    "</tr>");

            DataTable dtGridTbl = dsPmtVchr.Tables[2];

            for (int i = 0; i < dtGridTbl.Rows.Count; i++)
            {

                strHTML.Append("<tr>" +
                "<td align=\"center\">" +
                (i + 1).ToString() +
                "</td>" +
                "<td>" +
                dtGridTbl.Rows[i]["Panum"].ToString() +
                "</td>" +
                "<td colspan=\"2\">" +
                dtGridTbl.Rows[i]["AccountDescription"].ToString() +
                "</td>" +
                "<td align=\"right\">" +
                dtGridTbl.Rows[i]["Transaction_Amount"].ToString() +
                "</td>" +
                "</tr>");
            }

            DataTable dtGridTblAddLess = dsPmtVchr.Tables[3];

            if (dtGridTblAddLess.Rows.Count > 0)
            {
                strHTML.Append(
                  "</table>" +
                    "</td>" +
                    "</tr>");
                strHTML.Append(
                        "<tr>" +
      "<td>Add/Less Details : </td></tr>" +

                      "<tr>" +
      "<td>" +
          "<table width=\"100%\" style=\"border-color: Black;\" border=\"0.5\" cellpadding=\"4\" cellspacing=\"0\">" +
           "<tr>" +
                  "<td align=\"center\" >" +
                      "<font face=\"verdana\" size=\"1\"><b>Sl.No </b></font>" +
                  "</td>" +
                    "<td align=\"center\" >" +
                      "<font face=\"verdana\" size=\"1\"><b>Add or Less</b></font>" +
                  "</td>" +
                  "<td align=\"center\">" +
                      "<font face=\"verdana\" size=\"1\"><b>Rental Schedule No</b></font>" +
                  "</td>" +
                  "<td align=\"center\" colspan=\"2\">" +
                       "<font face=\"verdana\" size=\"1\"><b>Particulars</b></font>" +
                  "</td>" +
                  "<td align=\"center\">" +
                      "<font face=\"verdana\" size=\"1\"><b>Amount</b></font>" +
                  "</td>" +
                      "</tr>");

                for (int i = 0; i < dtGridTblAddLess.Rows.Count; i++)
                {

                    strHTML.Append("<tr>" +
                       "<td align=\"center\">" +
                           (i + 1).ToString() +
                       "</td>" +
                        "<td>" +
                           dtGridTblAddLess.Rows[i]["Add_Less_Desc"].ToString() +
                       "</td>" +
                       "<td>" +
                           dtGridTblAddLess.Rows[i]["Panum"].ToString() +
                       "</td>" +
                       "<td colspan=\"2\">" +
                           dtGridTblAddLess.Rows[i]["AccountDescription"].ToString() +
                       "</td>" +
                       "<td align=\"right\">" +
                           dtGridTblAddLess.Rows[i]["Transaction_Amount"].ToString() +
                       "</td>" +
                   "</tr>");
                }
                strHTML.Append(
                    "<tr>" +
                        "<td>" +
                        "</td>" +
                        "<td>" +
                        "</td>" +
                        "<td>" +
                        "</td>" +
                    // "<td>" +
                    //"</td>" +
                        "<td colspan=\"2\">" +
                            "<b>Total Amount</b>" +
                        "</td>" +
                        "<td align=\"right\"><b>" +
                            drHDR["Payment_Amount"].ToString() +
                        "</b></td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>");

            }
            else
            {
                strHTML.Append(
                "<tr>" +
                    "<td>" +
                    "</td>" +
                    "<td>" +
                    "</td>" +
                    //"<td>" +
                    //"</td>" +
                    "<td colspan=\"2\">" +
                        "<b>Total Payment Amount</b>" +
                    "</td>" +
                    "<td align=\"right\"><b>" +
                        drHDR["Payment_Amount"].ToString() +
                    "</b></td>" +
                "</tr>" +
            "</table>" +
        "</td>" +
    "</tr>");
            }

            strHTML.Append(
           "<tr>" +
               "<td>" +
                   "<table width=\"100%\">" +
                 "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
         "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
         "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
          "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
          "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
          "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
          "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
          "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
          "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
          "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\" style=\"padding-left:25px;\">For <b>" + ObjUserInfo.ProCompanyNameRW + "</b>" + "</td>" +
        "</tr>" +

        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
       "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\">&nbsp;</td>" +
        "</tr>" +
        "<tr>" +
        "<td width=\"100%\" colspan=\"2\" style=\"padding-left:25px;\"><b>Authorised Signatory</b></td>" +
        "</tr>" +
         "<tr>" +
          "<td width=\"25%\" colspan=\"2\" align=\"left\">" + ObjUserInfo.ProUserNameRW.ToString() + "</td>" +
      "</tr>" +

       "<tr>" +
          "<td width=\"100%\" colspan=\"2\">" + DateTime.Now.ToString(strDateFormat) + "  " + DateTime.Now.ToLongTimeString() + "</td>" +
      "</tr>" +
        "</table>" +
               "</td>" +
           "</tr>" +
       "</table> </font>");
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return strHTML.ToString();
    }
    
    private DataSet FunPriUpdateInvDtl(string strPoDetlID, double dblPayableAmt, double dblRetentionAmt, Int32 intIsAdded, string strPIDetlID, string strVIDetlID)
    {
        DataSet dsInvoice = null;
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "3");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));
            Procparam.Add("@Po_Det_ID", Convert.ToString(strPoDetlID));
            Procparam.Add("@PI_Det_ID", Convert.ToString(strPIDetlID));
            Procparam.Add("@VI_Det_ID", Convert.ToString(strVIDetlID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@PaymentRequest_ID", (intIsAdded == 0) ? "0" : Convert.ToString(strRequestID));
            Procparam.Add("@Payable_Amount", Convert.ToString(dblPayableAmt));
            Procparam.Add("@Retention_Amount", Convert.ToString(dblRetentionAmt));
            Procparam.Add("@Is_Added", Convert.ToString(intIsAdded));

            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 3)
            {
                Procparam.Add("@Customer_ID", Convert.ToString(ddlReceiptFrom.SelectedValue));
                Procparam.Add("@Vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            }
            else if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1)
            {
                Procparam.Add("@Vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            }
            else if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4)
            {
                Procparam.Add("@Customer_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            }

            dsInvoice = Utility.GetDataset("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
        return dsInvoice;
    }

    private void FunPriAddCmnParam()
    {
        try
        {
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 3)
            {
                Procparam.Add("@Customer_ID", Convert.ToString(ddlReceiptFrom.SelectedValue));
                Procparam.Add("@Vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            }
            else if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1)
            {
                Procparam.Add("@Vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            }
            else if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4)
            {
                Procparam.Add("@Funder_ID", Convert.ToString(ddlReceiptFrom.SelectedValue));
                Procparam.Add("@Customer_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            }
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }

    #region Page Methods

    private void FunPriBindGrid()
    {
        try
        {
            lblPagingErrorMessage.InnerText = "";
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            FunPriGetSearchValue();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));
            Procparam.Add("@FilterType", Convert.ToString(ddlInvoiceSortBy.SelectedValue));
            if (Convert.ToString(ddlInvSrchTxt.SelectedText) != "")
                Procparam.Add("@FilterText", Convert.ToString(ddlInvSrchTxt.SelectedValue));
            if (Convert.ToString(txtInvoiceStartDate.Text) != "" && Convert.ToString(txtInvoiceEndDate.Text) != "")
            {
                Procparam.Add("@From_Date", Convert.ToString(Utility.StringToDate(txtInvoiceStartDate.Text)));
                Procparam.Add("@To_Date", Convert.ToString(Utility.StringToDate(txtInvoiceEndDate.Text)));
            }
            if (ViewState["grvPaymentDetails"] != null)
            {
                DataTable dtInvoice = (DataTable)ViewState["grvPaymentDetails"];
                if (dtInvoice.Rows.Count == 0 || Convert.ToString(dtInvoice.Rows[0]["Panum"]) == "-1")
                {
                    Procparam.Add("@Option1", "1");
                }
            }
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 3)
            {
                Procparam.Add("@Option", "1");
                Procparam.Add("@Customer_ID", Convert.ToString(ddlReceiptFrom.SelectedValue));
                Procparam.Add("@Vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            }
            else if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1)
            {
                Procparam.Add("@Option", "2");
                Procparam.Add("@Vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                if (Convert.ToString(ddlReceiptFrom.SelectedValue) != "" && Convert.ToInt32(ddlReceiptFrom.SelectedValue) > 0)
                    Procparam.Add("@Customer_ID", Convert.ToString(ddlReceiptFrom.SelectedValue));
            }
            else if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4)
            {
                Procparam.Add("@Customer_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                Procparam.Add("@Option", "3");
            }

            grvLoadInvoiceDtl.BindGridView("S3G_Loanad_GetPaymentInvoiceDtl", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            DataTable dtPOInvoiceDtls = ((DataView)grvLoadInvoiceDtl.DataSource).ToTable();
            ViewState["POInvoiceDetails"] = dtPOInvoiceDtls;
            btnAddPOInvoice.Enabled = true;
            //FunPriEnblDsblINVGridClmn();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvLoadInvoiceDtl.Rows[0].Visible = btnAddPOInvoice.Enabled = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

            Procparam.Clear();
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Is_Added", "0");    //Added on 15Jun2015 for OPC Req
            DataTable dtSmry = Utility.GetDefaultData("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);
            if (dtSmry != null && dtSmry.Rows.Count > 0)
                FunPriDisplayGridTotal(dtSmry);

            DataRow[] drCheck = dtPOInvoiceDtls.Select("Chk_Selected = 0");
            CheckBox chkSelectAllInvoices = (CheckBox)grvLoadInvoiceDtl.HeaderRow.FindControl("chkSelectAllInvoices");
            chkSelectAllInvoices.Checked = (drCheck.Length > 0) ? false : true;
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

    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        try
        {
            arrSearchVal = grvLoadInvoiceDtl.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearSearchValue()
    {
        try
        {
            grvLoadInvoiceDtl.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriSetSearchValue()
    {
        try
        {
            grvLoadInvoiceDtl.FunPriSetSearchValue(arrSearchVal);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {
        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                }
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvLoadInvoiceDtl.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

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

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (lnkbtnSearch.ID == "lnkbtnSort" + (iCount + 1).ToString())
                {
                    strSortColName = arrSortCol[iCount].ToString();
                    break;
                }
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvLoadInvoiceDtl.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvLoadInvoiceDtl.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblPagingErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblPagingErrorMessage.InnerText = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #endregion

    //Added on 15Jun2015 Starts here

    private void FunPriDisplayGridTotal(DataTable dt)
    {
        try
        {
            if (dt != null && dt.Rows.Count > 0 && grvLoadInvoiceDtl != null)
            {
                grvLoadInvoiceDtl.ShowFooter = grvLoadInvoiceDtl.FooterRow.Visible = true;
                Label lblFtrTtlInvoice = (Label)grvLoadInvoiceDtl.FooterRow.FindControl("lblFtrTtlInvoice");
                Label lblFtrTtlTDS = (Label)grvLoadInvoiceDtl.FooterRow.FindControl("lblFtrTtlTDS");
                Label lblFtrTtlWCT = (Label)grvLoadInvoiceDtl.FooterRow.FindControl("lblFtrTtlWCT");
                Label lblFtrTtlCenvat = (Label)grvLoadInvoiceDtl.FooterRow.FindControl("lblFtrTtlCenvat");
                Label lblFtrTtlRetension = (Label)grvLoadInvoiceDtl.FooterRow.FindControl("lblFtrTtlRetension");
                Label lblFtrTtlAmount = (Label)grvLoadInvoiceDtl.FooterRow.FindControl("lblFtrTtlAmount");
                Label lblFtrTtlDisposed = (Label)grvLoadInvoiceDtl.FooterRow.FindControl("lblFtrTtlDisposed");

                lblFtrTtlInvoice.Text = Convert.ToString(dt.Rows[0]["Invoice_Amount"]);
                lblFtrTtlTDS.Text = Convert.ToString(dt.Rows[0]["TDS_Amount"]);
                lblFtrTtlWCT.Text = Convert.ToString(dt.Rows[0]["WCT_Amount"]);
                lblFtrTtlCenvat.Text = Convert.ToString(dt.Rows[0]["Cenvat_Amount"]);
                lblFtrTtlRetension.Text = Convert.ToString(dt.Rows[0]["Retention_Amount"]);
                lblFtrTtlAmount.Text = Convert.ToString(dt.Rows[0]["Total_Amount"]);
                lblFtrTtlDisposed.Text = Convert.ToString(dt.Rows[0]["Payable_Amount"]);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriDisplaySmryGridTotal(DataTable dt)
    {
        try
        {
            if (dt != null && dt.Rows.Count > 0 && grvPaymentInvoiceDtl != null)
            {
                grvPaymentInvoiceDtl.ShowFooter = grvPaymentInvoiceDtl.FooterRow.Visible = true;
                Label lblFtrTtlInvoice = (Label)grvPaymentInvoiceDtl.FooterRow.FindControl("lblgdFtrTtlInvoice");
                Label lblFtrTtlTDS = (Label)grvPaymentInvoiceDtl.FooterRow.FindControl("lblgdFtrTtlTDS");
                Label lblFtrTtlWCT = (Label)grvPaymentInvoiceDtl.FooterRow.FindControl("lblgdFtrTtlWCT");
                Label lblFtrTtlCenvat = (Label)grvPaymentInvoiceDtl.FooterRow.FindControl("lblgdFtrTtlCenvat");
                Label lblFtrTtlRetension = (Label)grvPaymentInvoiceDtl.FooterRow.FindControl("lblgdFtrTtlRetention");
                Label lblFtrTtlAmount = (Label)grvPaymentInvoiceDtl.FooterRow.FindControl("lblgdFtrTtlAmount");
                Label lblFtrTtlDisposed = (Label)grvPaymentInvoiceDtl.FooterRow.FindControl("lblgdFtrTtlPayable");

                lblFtrTtlInvoice.Text = Convert.ToString(dt.Rows[0]["Invoice_Amount"]);
                lblFtrTtlTDS.Text = Convert.ToString(dt.Rows[0]["TDS_Amount"]);
                lblFtrTtlWCT.Text = Convert.ToString(dt.Rows[0]["WCT_Amount"]);
                lblFtrTtlCenvat.Text = Convert.ToString(dt.Rows[0]["Cenvat_Amount"]);
                lblFtrTtlRetension.Text = Convert.ToString(dt.Rows[0]["Retention_Amount"]);
                lblFtrTtlAmount.Text = Convert.ToString(dt.Rows[0]["Total_Amount"]);
                lblFtrTtlDisposed.Text = Convert.ToString(dt.Rows[0]["Payable_Amount"]);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    //Added on 15Jun2015 Ends here

    #endregion

    #region Workflow Methods
    /// <summary>
    /// Workflow Function
    /// </summary>
    private void PreparePageForWorkFlowLoad()
    {
        try
        {
            if (!IsPostBack)
            {
                WorkFlowSession WFSessionValues = new WorkFlowSession();
                // Get The IDVALUE from Document Sequence #

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@DocumentNo", WFSessionValues.WorkFlowDocumentNo);
                Procparam.Add("@Company_Id", intCompanyID.ToString());
                DataTable dt = new DataTable();
                dt = Utility.GetDefaultData("S3G_WORKFLOW_PAYMENTREQ_MOD", Procparam);
                if (dt.Rows.Count > 0)//If Exist i.e Modify Mode
                {
                    strRequestID = dt.Rows[0]["Doc_ID"].ToString();
                    ViewState["strRequestID"] = strRequestID;
                    FunPriLoadFormControls();
                    FunloadGridPayments();
                    ModifyMode();
                }
                else
                {
                    DataTable HeaderValues = GetHeaderDetailsFromPANUMandSANUM(WFSessionValues.PANUM, ProgramCode, WFSessionValues.SANUM);
                    ddlLOB.SelectedValue = WFSessionValues.LOBId.ToString();
                    ddlBranch.SelectedValue = WFSessionValues.BranchID.ToString();
                    if (HeaderValues.Rows.Count > 0)
                    {
                        grvPaymentDetails.DataSource = HeaderValues;
                        grvPaymentDetails.DataBind();
                        ViewState["grvPaymentDetails"] = HeaderValues;
                        grvPaymentDetails.Columns[grvPaymentDetails.Columns.Count - 1].Visible = false;
                        grvPaymentDetails.FooterRow.Visible = false;
                        ddlPayTo.SelectedValue = HeaderValues.Rows[0]["PayTo"].ToString();
                        FunLoadPaymentAdjustment();
                        FunPriLoadCustomerEntityDtls(HeaderValues.Rows[0]["Cashflow_entity_code"].ToString());
                        ViewState["hdnCustorEntityID"] = HeaderValues.Rows[0]["Cashflow_entity_code"].ToString();
                        if (HeaderValues.Rows[0]["Cashflow_entity_type"].ToString() == "145")
                        {
                            ucCustomerAddress.Caption = "Entity";
                            FunPriLoadPaytypeingrid("8");
                        }
                        else
                        {
                            ucCustomerAddress.Caption = "Customer";
                            FunPriLoadPaytypeingrid("1");
                        }
                        FunPriDisableCtrlsWF();
                        if (!string.IsNullOrEmpty(lbltotalPaymentAdjust.Text))
                        {
                            txtDocAmount.Text = Math.Round(Convert.ToDecimal(lbltotalPaymentAdjust.Text), 0).ToString();
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
    #endregion

    private void FunPriDisableCtrlsWF()
    {
        try
        {
            ddlPaymentStatus.SelectedValue = "1";
            TabPanelPBD.Enabled =
            btnPrintVoucher.Visible =
            btnPrintCheque.Visible =
                //btnCoveringLetter.Visible =
            btnCancelPayment.Visible =
            PanelPayType.Enabled =
                // ddlLOB.ClearDropDownList();
                //ddlBranch.Clear();
            PnlCustEntityInformation.Enabled = false;
            ucCustomerCodeLov.ButtonEnabled = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriSetMaxLength()
    {
        try
        {
            //txtDocAmount.CheckGPSLength(true);
            txtDocAmount.SetDecimalPrefixSuffix(10, 2, true, false, "Document Amount");
            if (grvPaymentDetails != null)
            {
                if (grvPaymentDetails.FooterRow != null)
                {
                    TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount") as TextBox;
                    txtFooterAmount.CheckGPSLength(true, "Amount");
                }
            }
            if (grvPaymentAdjustment != null)
            {
                if (grvPaymentAdjustment.FooterRow != null)
                {
                    TextBox txtFooterAmount = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterAmount") as TextBox;
                    txtFooterAmount.CheckGPSLength(true, "Amount");
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }

    }

    private void FunLockControlsInQueryMode()
    {
        try
        {
            if (grvPaymentAdjustment.FooterRow != null)
                grvPaymentAdjustment.FooterRow.Visible = false;
            if (grvPaymentDetails.FooterRow != null)
                grvPaymentDetails.FooterRow.Visible = false;
            ddlCurrencyCode.Enabled = false;
            PnlCustEntityInformation.Enabled = false;
            ucCustomerCodeLov.ButtonEnabled = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunLoadPaymentAdjustment()
    {
        if (strQsMode == "C")
            FunpriBlank();
        /*
        if ((string.Compare(strQsMode, "Q") == 0) || (string.Compare(strQsMode, "M") == 0))
        {
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Request_No", strRequestID);
            DataTable dtAdjustment = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetPaymentAdjustments, Procparam);
            ViewState["PaymentAdjustment"] = grvPaymentAdjustment.DataSource = dtAdjustment;
            grvPaymentAdjustment.DataBind();
            //FunLoadPaymentAdjustment();
            if (dtAdjustment == null || dtAdjustment.Rows.Count == 0)

                FunpriBlank();

            lbltotalPaymentAdjust.Text = Convert.ToDecimal((lblPaymentDetailsTotal.Text)).ToString();

            for (int i = 0; i < dtAdjustment.Rows.Count; i++)
            {
                if (string.Compare(dtAdjustment.Rows[i]["AddOrLess"].ToString(), "Add") == 0)
                {
                    lbltotalPaymentAdjust.Text = (Convert.ToDecimal((lbltotalPaymentAdjust.Text)) +
                        Convert.ToDecimal(dtAdjustment.Rows[i]["Amount"].ToString())).ToString();
                }
                else
                {
                    lbltotalPaymentAdjust.Text = (Convert.ToDecimal((lbltotalPaymentAdjust.Text)) -
                        Convert.ToDecimal(dtAdjustment.Rows[i]["Amount"].ToString())).ToString();
                }
            }
        }
        else
        {
        FunpriBlank();
           
        }*/

    }

    private void FunpriBlank()
    {
        try
        {
            DataColumn AddOrLess = new DataColumn("AddOrLess", System.Type.GetType("System.String"));
            DataColumn PayType = new DataColumn("PayType", System.Type.GetType("System.String"));
            DataColumn PayTypeID = new DataColumn("PayTypeID", System.Type.GetType("System.String"));
            DataColumn SANum = new DataColumn("SANum", System.Type.GetType("System.String"));
            DataColumn PANum = new DataColumn("PANum", System.Type.GetType("System.String"));
            DataColumn GL_Code = new DataColumn("GL_Code", System.Type.GetType("System.String"));
            DataColumn SL_Code = new DataColumn("SL_Code", System.Type.GetType("System.String"));
            DataColumn Amount = new DataColumn("Amount", System.Type.GetType("System.String"));
            DataColumn Remarks = new DataColumn("Remarks", System.Type.GetType("System.String"));

            DataTable dt = new DataTable();
            dt.Columns.Add(AddOrLess);
            dt.Columns.Add(PayType);
            dt.Columns.Add(PayTypeID);
            dt.Columns.Add(PANum);
            dt.Columns.Add(SANum);
            dt.Columns.Add(GL_Code);
            dt.Columns.Add(SL_Code);
            dt.Columns.Add(Amount);
            dt.Columns.Add(Remarks);

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            grvPaymentAdjustment.DataSource = dt;
            grvPaymentAdjustment.DataBind();
            //FunLoadPaymentAdjustment();
            grvPaymentAdjustment.Rows[0].Visible = false;

            ViewState["PaymentAdjustment"] = dt;
            ViewState["grvPaymentAdjust"] = dt;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void ModifyMode()
    {
        try
        {
            //btnCoveringLetter.Visible =
            btnCancelPayment.Visible = true;

            ddlReceiptFrom.Enabled = ddlTranche.Enabled = btnShow.Enabled = ddlPaymentType.Enabled = ddlPayMode.Enabled =
            lblChequeStatus.Visible = btnPrintVoucher.Enabled = rfvcmbCustomer.Enabled =
            RBLCompanyCashorBankAcct.Visible = ddlChequeStatus.Visible = ddlLOB.Enabled = ddlBranch.Enabled = false;

            //funshowPaymentdetailsgrid(strQsMode);

            if (Convert.ToInt32(ddlPaymentStatus.SelectedValue) >= 2)
                fundisablecontrols("Q");
            else
                fundisablecontrols("M");

            if (ddlPaymentStatus.SelectedValue == "3")   // if (dtPaymentRequest.Rows[0]["Pmt_Voucher_status"] == 3)  -- then enable the user to print the voucher
                btnPrintVoucher.Enabled = true;

            if (ddlPayMode.SelectedValue == "1")
            {
                if (ViewState["IsVoucher_Print"] != null)
                {
                    if (Convert.ToString(ViewState["IsVoucher_Print"]) == "P" || Convert.ToString(ViewState["IsVoucher_Print"]) == "D")  // enble btncheque only if the voucher was printed.
                    {
                        btnPrintCheque.Enabled = true;
                        //btnCoveringLetter.Enabled = 
                    }
                    else
                    {
                        btnPrintCheque.Enabled = false;
                        //btnCoveringLetter.Enabled =
                    }
                }
                ddlChequeStatus.Visible = lblChequeStatus.Visible = true;
            }

            //if (ddlPaymentStatus.SelectedValue == "3")
            //{
            if (ddlPayMode.SelectedValue == "1" || ddlPayMode.SelectedValue == "2")//Enabled the payment bank details tab if the payment is approved and the pay mode is cheque or DD
            {

                FunEnableDisableInstrumentTabs(true);
                if (ddlPayMode.SelectedValue == "2") //Instrument Number is User Enterable for DD
                {
                    txtInstrumentNumber.ReadOnly = false;
                    btnPrintCheque.Visible = false;

                }
                else
                {
                    txtInstrumentNumber.ReadOnly = true;
                    btnPrintCheque.Visible = true;
                }
            }
            //}
            /*if (ddlPayMode.SelectedValue == "2")
            {
                RBLCompanyCashorBankAcct.Visible = true;


                if (ViewState["IsVoucher_Print"] != null)
                {
                    if (Convert.ToString(ViewState["IsVoucher_Print"]) == "P" || Convert.ToString(ViewState["IsVoucher_Print"]) == "D")
                    {
                        if (Convert.ToInt32(ddlbankname.SelectedValue) > 0)//Radio button selection only in front end
                        {
                            RBLCompanyCashorBankAcct.SelectedValue = "1";
                            rfvbankname.Enabled = rfvtxtGLCode.Enabled =
                                rfvbanknameC.Enabled =
                                rfvtxtGLCodeC.Enabled =
                                RFVAcctNumber.Enabled =
                                RFVAcctNumberC.Enabled =
                                RFVIFSC_CodeC.Enabled =
                                RFVIFSC_Code.Enabled =
                                true;

                        }
                        else
                        {
                            RBLCompanyCashorBankAcct.SelectedValue = "0";
                            rfvbanknameC.Enabled =
                            rfvtxtGLCodeC.Enabled =
                            rfvbankname.Enabled = rfvtxtGLCode.Enabled =
                                                            RFVAcctNumber.Enabled =
                                RFVAcctNumberC.Enabled =
                                RFVIFSC_CodeC.Enabled =
                                RFVIFSC_Code.Enabled =

                            false;
                        }
                        btnCoveringLetter.Enabled = true;
                        ImageInstrumentDate.Visible = false;
                        FunPriPayDtstabDDValidation(false);
                    }
                    else
                    {
                        btnCoveringLetter.Enabled = false;
                        FunPriPayDtstabDDValidation(true);
                    }
                }
            }*/
            if (ddlPayMode.SelectedValue == "2")
            {
                RBLCompanyCashorBankAcct.Visible = true;
                if (Convert.ToInt32(ddlbankname.SelectedValue) > 0)//Radio button selection only in front end
                {
                    RBLCompanyCashorBankAcct.SelectedValue = "1";
                }
                else
                {
                    RBLCompanyCashorBankAcct.SelectedValue = "0";
                }
                if (txtInstrumentNumber.Text != string.Empty)
                {
                    RBLCompanyCashorBankAcct.Enabled = false;
                    ddlbankname.ClearDropDownList();
                    ddlAcctNumber.ClearDropDownList();
                    ImageInstrumentDate.Visible = false;
                    //txtInstrumentNumber.ReadOnly = true;
                }
                else
                {
                    RBLCompanyCashorBankAcct.Enabled = true;
                    ImageInstrumentDate.Visible = true;
                }
            }


            if (ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue == "1")
            {
                FunDisableGLSLCodeGrid(true);
                FundisableGridValidation(false);
            }
            else
            {
                FunDisableGLSLCodeGrid(false);
                FundisableGridValidation(true);
            }
            FunChkDocdateReprintcheque();

            if (Convert.ToInt32(ddlPaymentStatus.SelectedValue) < 3)
            {
                btnPrintCheque.Visible = btnPrintVoucher.Visible = false;
                //btnCoveringLetter.Visible = 
            }

            if (Convert.ToInt32(ddlPaymentStatus.SelectedValue) > 2)
            {
                if (ddlAcctNumber.Items.Count > 0)
                {
                    ddlbankname.ClearDropDownList();
                    ddlAcctNumber.ClearDropDownList();
                }
                RBLCompanyCashorBankAcct.Enabled = false;
            }
            //tpInvoiceLoading.Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) > 0 && Convert.ToInt32(ddlPaymentType.SelectedValue) < 5) ? true : false;
        }
        catch (Exception ex)
        {
        }
    }

    private void QueryView()
    {
        try
        {
            if (ddlPaymentStatus.SelectedItem.Text == "Pending" || ddlPaymentStatus.SelectedItem.Text == "Under Process")//To Check Status to Enable Print button
            {
                btnPrintVoucher.Enabled = false;
            }
            //btnPrintVoucher.Visible =
            btnPrintCheque.Visible =
                //btnCoveringLetter.Visible =
            btnCancelPayment.Visible = false;
            RBLCompanyCashorBankAcct.Visible = false;
            //btnCalculate.Visible = false;

            //btnPrintVoucher.Visible = (Convert.ToInt32(ddlPaymentStatus.SelectedValue) == 3) ? true : false;

            FunPriLockControls(false);
            FunLockControlsInQueryMode();
            ddlChequeStatus.Visible = true;
            lblChequeStatus.Visible = true;
            //funshowPaymentdetailsgrid(strQsMode);

            fundisablecontrols("Q");
            //tpInvoiceLoading.Enabled = (Convert.ToInt32(ddlPaymentType.SelectedValue) > 0 && Convert.ToInt32(ddlPaymentType.SelectedValue) < 5) ? true : false;

            if (ddlPayMode.SelectedValue == "1" || ddlPayMode.SelectedValue == "2" || ddlPayMode.SelectedValue == "4" || ddlPayMode.SelectedValue == "5" || ddlPayMode.SelectedValue == "6")
            {
                TabPanelPBD.Enabled = true;
                txtInstrumentNumber.ReadOnly = txtFavouringName.ReadOnly = txtRemarks.ReadOnly = txtPmtGatewayRefNo.ReadOnly =
                txtInstrumentDate.ReadOnly = true;
                CalendarExtenderInstrumentDate.Enabled = false;
                ddlbankname.ClearDropDownList();
                if (ddlAcctNumber.Items.Count > 0)
                    ddlAcctNumber.ClearDropDownList();

                if (ddlPayMode.SelectedValue == "1")
                {
                    ddlChequeStatus.Visible = true;
                    lblChequeStatus.Visible = true;
                }
                else
                {
                    ddlChequeStatus.Visible = false;
                    lblChequeStatus.Visible = false;
                }
                if (ViewState["IsVoucher_Print"] != null)
                {
                    if (Convert.ToString(ViewState["IsVoucher_Print"]) == "P" || Convert.ToString(ViewState["IsVoucher_Print"]) == "D")  // enble btncheque only if the voucher was printed.
                    {
                        //btnCoveringLetter.Visible = true;
                    }
                }
            }
            else
                TabPanelPBD.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunChkDocdateReprintcheque()
    {
        if (DateTime.Now > Utility.StringToDate(txtPaymentRequestDate.Text).AddMonths(1))
        {
            btnPrintCheque.Enabled = false;
            if (btnPrintCheque.Text == "Print Cheque")
            {
                //btnCoveringLetter.Enabled = false;
            }
            else
            {
                //btnCoveringLetter.Enabled = true;
            }
        }
    }

    private void FunEnableDisableInstrumentTabs(bool blnflag)
    {
        try
        {
            TabPanelPBD.Enabled =
            rfvbankname.Enabled =
            rfvbanknameC.Enabled =
            rfvtxtGLCodeC.Enabled =
            rfvtxtGLCode.Enabled =
            RFVAcctNumber.Enabled =
            RFVAcctNumberC.Enabled =
            RFVIFSC_CodeC.Enabled =
            RFVIFSC_Code.Enabled =
            rfvtxtInstrumentNumber.Enabled =
            rfvtxtInstrumentDate.Enabled = blnflag;
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }

    private void FunPriToSetChequeStatus(string chequeStatus)
    {
        switch (chequeStatus)
        {
            case "N":
                ddlChequeStatus.SelectedValue = "1";
                break;
            case "P":
                ddlChequeStatus.SelectedValue = "2";
                break;
            case "D":
                ddlChequeStatus.SelectedValue = "3";
                break;
            default:
                ddlChequeStatus.SelectedValue = "1";
                break;
        }
    }

    private void GetQueryString()
    {
        if (Request.QueryString.Get("qsMode") != null)
        {
            if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
            {
                ViewState["IsVoucher_Print"] =
                ViewState["IsCheque_Print"] = "N";
                btnPrintCheque.Enabled =
                btnPrintVoucher.Enabled = true;
                strQsMode = "Q";
            }
            else if (string.Compare("M", Request.QueryString.Get("qsMode")) == 0)
            {
                strQsMode = "M";
            }
            else if (string.Compare("C", Request.QueryString.Get("qsMode")) == 0)
            {
                strQsMode = "C";
            }
            else
            {
                strQsMode = "D";//Dummy
            }
        }
        if (Request.QueryString.Get("qsViewId") != null)  // to get the Request ID
        {
            FormsAuthenticationTicket TicketViewID = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            strRequestID = TicketViewID.Name;
        }
    }

    //SLCode
    private void FunPriLoadSLcodes(string GridType, string GLCODE)
    {
        //DropDownList ddlFooterSL_Code = new DropDownList();
        UserControls_S3GAutoSuggest ddlFooterSL_Code = new UserControls_S3GAutoSuggest();
        if (Procparam == null)
            Procparam = new Dictionary<string, string>();
        else
            Procparam.Clear();
        //Procparam.Add("@Option", "12");
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        if (GLCODE != "" || GLCODE != string.Empty)
            Procparam.Add("@GLCode", GLCODE);
        switch (GridType)
        {
            case "PD":
                ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
                break;
            case "PA":
                ddlFooterSL_Code = ((UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code"));
                break;
        }

        DataSet Dset = Utility.GetDataset("S3G_LOANAD_GetPaymentRequestgridDetails_AGT", Procparam);
        ddlFooterSL_Code.Clear();
        ddlFooterSL_Code.AvailableRecords = Convert.ToInt32(Dset.Tables[1].Rows[0]["Cnt"].ToString());
        //ddlFooterSL_Code.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "SL_Code", "SL_Code" });
    }

    //GL Code
    private void FunPriLoadGLcodes()
    {
        try
        {
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) > 5)
            {
                if (Procparam == null)
                    Procparam = new Dictionary<string, string>();
                else
                    Procparam.Clear();
                Procparam.Add("@Option", "12");
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                if (chkAccountBased.SelectedValue == "1")
                    Procparam.Add("@OptionPay_To", "1");
                if (grvPaymentDetails.FooterRow != null)
                {
                    DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
                    ddlFooterGL_Code.BindDataTable("S3G_Get_PaymentCMNLST", Procparam, new string[] { "GL_Code", "GL_Desc" });
                }
                if (grvPaymentAdjustment.FooterRow != null)
                {
                    DropDownList ddlFooterGL_Code = ((DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterGL_Code"));
                    ddlFooterGL_Code.BindDataTable("S3G_Get_PaymentCMNLST", Procparam, new string[] { "GL_Code", "GL_Desc" });
                }
            }
        }
        catch (Exception objException)
        {
        }
    }

    /// <summary>
    /// this is to add tooltip to your dropdown box 
    /// </summary>
    /// <param name="dt_PaymentEntityPANSANDetails"></param>

    private void FunPriAddToolTip(DropDownList ddl)
    {
        if (ddl != null && ddl.Items.Count > 0)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                ddl.Items[i].Attributes.Add("title", ddl.Items[i].Text);
            }
        }
    }

    protected void txtInstrumentDate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string Keyvalue = S3GBusEntity.ClsPubConfigReader.FunPubReadConfig("InstrumentDate");
            if ((Utility.StringToDate(txtInstrumentDate.Text) < (DateTime.Now.AddDays(-Convert.ToInt32(Keyvalue)))) ||
                (Utility.StringToDate(txtInstrumentDate.Text) > (DateTime.Now.AddDays(Convert.ToInt32(Keyvalue)))))
            {
                Utility.FunShowAlertMsg(this, "Instrument Date cannot be lesser or greater than " + Keyvalue + " days");
                txtInstrumentDate.Text = "";
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void Desc_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            Label lblActualAmount = (Label)grvPaymentDetails.Rows[gvRow.RowIndex].FindControl("lblActualAmount");
            Label lblPrimeAccountNumber = (Label)grvPaymentDetails.Rows[gvRow.RowIndex].FindControl("lblPrimeAccountNumber");
            Label lblSubAccountNumber = (Label)grvPaymentDetails.Rows[gvRow.RowIndex].FindControl("lblSubAccountNumber");
            Label lblPayTypeID = (Label)grvPaymentDetails.Rows[gvRow.RowIndex].FindControl("lblCashFlow_ID");
            TextBox lblAmount = (TextBox)grvPaymentDetails.Rows[gvRow.RowIndex].FindControl("lblAmount");
            TextBox lblDescription = (TextBox)grvPaymentDetails.Rows[gvRow.RowIndex].FindControl("lblDescription");

            if (Convert.ToDecimal(lblAmount.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "Amount cannot be zero");
                lblAmount.Text = lblActualAmount.Text;
                return;
            }
            if ((lblPrimeAccountNumber.Text != "" || lblPrimeAccountNumber.Text != string.Empty))
            {
                if (Convert.ToDecimal(lblAmount.Text) > Convert.ToDecimal(lblActualAmount.Text))
                {
                    Utility.FunShowAlertMsg(this, "Amount should not exceed Rs." + lblActualAmount.Text);
                    lblAmount.Text = lblActualAmount.Text;
                    return;
                }
            }
            if (lblPayTypeID.Text == "215")
            {
                if (Convert.ToDecimal(lblAmount.Text) > Convert.ToDecimal(lblActualAmount.Text))
                {
                    Utility.FunShowAlertMsg(this, "Amount should not exceed Rs. " + lblActualAmount.Text);
                    lblAmount.Text = lblActualAmount.Text;
                    return;
                }
            }
            FunPriCalcSumAmountDetails();
            FunPriCalcSumAmount();
            Funupdatedatatable();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }

    }

    protected void RBLCompanyCashorBankAcct_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RBLCompanyCashorBankAcct.SelectedValue == "0")
        {
            rfvbankname.Enabled =
            rfvtxtGLCode.Enabled =
            rfvbanknameC.Enabled =
            RFVAcctNumber.Enabled =
            RFVIFSC_Code.Enabled =
             RFVAcctNumberC.Enabled =
            RFVIFSC_CodeC.Enabled =
            rfvtxtGLCodeC.Enabled = false;
            lblBankName.CssClass = "styleDisplayLabel";
            lblGLcode.CssClass = "styleDisplayLabel";
            lblAcctnumber.CssClass = "styleDisplayLabel";
            lblIFSC_Code.CssClass = "styleDisplayLabel";
            txtInstrumentDate.Text = txtInstrumentNumber.Text = txtGLCode.Text = txtSLCode.Text = txtBankbranch.Text =
                txtIFSC_Code.Text = "";
            ddlbankname.SelectedIndex = -1;
            ddlAcctNumber.SelectedIndex = -1;
        }
        else
        {
            rfvbankname.Enabled =
            rfvtxtGLCode.Enabled =
            rfvbanknameC.Enabled =
           RFVAcctNumber.Enabled =
            RFVIFSC_Code.Enabled =
             RFVAcctNumberC.Enabled =
            RFVIFSC_CodeC.Enabled =
            rfvtxtGLCodeC.Enabled = true;
            lblBankName.CssClass = "styleReqFieldLabel";
            lblGLcode.CssClass = "styleReqFieldLabel";
            lblAcctnumber.CssClass = "styleReqFieldLabel";
            lblIFSC_Code.CssClass = "styleReqFieldLabel";
            txtInstrumentDate.Text = txtInstrumentNumber.Text = txtGLCode.Text = txtSLCode.Text = txtBankbranch.Text =
                txtIFSC_Code.Text = "";
            ddlbankname.SelectedIndex = -1;
            ddlAcctNumber.SelectedIndex = -1;
        }
    }

    protected void ddlAcctNumber_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (Procparam == null)
            Procparam = new Dictionary<string, string>();
        else
            Procparam.Clear();
        Procparam.Add("@Option", "5");
        Procparam.Add("@SYS_BANK_CODE", ddlAcctNumber.SelectedValue);
        DataSet DS = Utility.GetDataset("S3G_ORG_GetPmtReqBankdetails", Procparam);

        if (DS.Tables[0].Rows.Count > 0)
        {
            txtIFSC_Code.Text = DS.Tables[0].Rows[0]["IFSC_CODE"].ToString();
            txtBankbranch.Text = DS.Tables[0].Rows[0]["branchname"].ToString();
            txtGLCode.Text = DS.Tables[0].Rows[0]["GL_ACCOUNT"].ToString();
            txtSLCode.Text = DS.Tables[0].Rows[0]["SL_ACCOUNT"].ToString();
            if (ddlPaymentStatus.SelectedValue == "3")
            {
                if (ddlPayMode.SelectedValue == "1")
                    txtInstrumentNumber.Text = DS.Tables[0].Rows[0]["LAST_USED_NUMBER"].ToString();
                txtInstrumentDate.Text = DateTime.Now.ToString(strDateFormat);
            }
            txtRemarks.Text = "";
            txtPmtGatewayRefNo.Text = "";
            ViewState["BankBranch"] = DS.Tables[0].Rows[0]["branchname"].ToString();
            ViewState["AccountNumber"] = DS.Tables[0].Rows[0]["ACCT_NUMBER"].ToString();

        }
    }

    private void FunPriLoadChequeStatus()
    {
        try
        {
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Option", "14");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            ddlChequeStatus.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "ChequeStatusID", "ChequeStatus" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadBankdetailsPmtReq()
    {
        try
        {
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Option", "3");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Request_No", strRequestID);
            DataSet DS = Utility.GetDataset("S3G_ORG_GetPmtReqBankdetails", Procparam);
            if (DS.Tables[0].Rows.Count > 0)
            {
                txtGLCode.Text = DS.Tables[0].Rows[0]["GL_ACCOUNT"].ToString();
                txtSLCode.Text = DS.Tables[0].Rows[0]["SL_ACCOUNT"].ToString();
                //if (ddlPayMode.SelectedValue == "1")
                txtInstrumentNumber.Text = DS.Tables[0].Rows[0]["Instrument_No"].ToString();
                txtInstrumentDate.Text = Convert.ToDateTime(DS.Tables[0].Rows[0]["Instrument_Date"].ToString()).ToString(strDateFormat);
                txtRemarks.Text = "";
                txtPmtGatewayRefNo.Text = "";
                txtFavouringName.Text = Convert.ToString(DS.Tables[0].Rows[0]["Favouring_Name"]);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw;
        }
    }

    private void FunPriLoadBankdetails()
    {
        try
        {
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Option", "2");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@SYS_BANK_CODE", ddlbankname.SelectedValue);
            DataSet DS = Utility.GetDataset("S3G_ORG_GetPmtReqBankdetails", Procparam);
            if (DS.Tables[1].Rows.Count > 0)
            {
                if (DS.Tables[1].Rows[0]["ErrorCode"].ToString() == "54")
                {
                    Utility.FunShowAlertMsg(this, "Selected bank GLCode is not in Account setup master.");
                    return;
                }
            }
            /*if (DS.Tables[0].Rows.Count > 0)
             {
                 txtGLCode.Text = DS.Tables[0].Rows[0]["GL_ACCOUNT"].ToString();
                 txtSLCode.Text = DS.Tables[0].Rows[0]["SL_ACCOUNT"].ToString();
                 if (ddlPayMode.SelectedValue == "1")
                     txtInstrumentNumber.Text = DS.Tables[0].Rows[0]["LAST_USED_NUMBER"].ToString();
                 txtRemarks.Text = "";
                 ViewState["BankBranch"] = DS.Tables[0].Rows[0]["branchname"].ToString();
                 ViewState["AccountNumber"] = DS.Tables[0].Rows[0]["ACCT_NUMBER"].ToString();
             }*/
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadCurrencyCode()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", intCompanyID.ToString());

            if (ddlCurrencyCode != null)
            {
                DataTable dtDefaultCurrency = Utility.GetDefaultData(SPNames.S3G_Get_Currency_Details_ByCompanyID, Procparam);
                if (dtDefaultCurrency != null && dtDefaultCurrency.Rows.Count > 0)
                {
                    ViewState["DefaultCurrencyID"] = dtDefaultCurrency.Rows[0]["Currency_ID"];
                    ddlCurrencyCode.SelectedValue = dtDefaultCurrency.Rows[0]["Currency_ID"].ToString();
                }

                Procparam.Add("@Currency_ID", "0");
                Procparam.Add("@IS_Active", "1");
                ddlCurrencyCode.BindDataTable("S3G_Get_Currency_Details", Procparam, new string[] { "Currency_ID", "Currency_Code", "Currency_Name" });
                Procparam.Clear();
                ViewState["CurrencyExchangeRate"] = Utility.GetDefaultData(SPNames.S3G_Get_ExchangeRate_Details, Procparam);

                if (ViewState["DefaultCurrencyID"] == null || string.IsNullOrEmpty(ViewState["DefaultCurrencyID"].ToString()))
                {
                    ViewState["DefaultCurrencyID"] = -1;
                }
                else
                {
                    txtCurrencyValue.Text = "";
                }
                FunPriGetExchangeRate();

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }

    }

    protected void ddlCurrencyCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriGetExchangeRate();
    }

    private void FunPriGetExchangeRate()
    {

        DataTable dtCurrencyExchangeRate = (DataTable)ViewState["CurrencyExchangeRate"];

        if (ddlCurrencyCode != null && ddlCurrencyCode.SelectedIndex > 0 && ddlCurrencyCode.Items.Count > 2
            && Convert.ToInt32(ddlCurrencyCode.SelectedValue) != Convert.ToInt32(ViewState["DefaultCurrencyID"]))
        {
            dtCurrencyExchangeRate.DefaultView.RowFilter = "Exchange_Currency_ID = " + ddlCurrencyCode.SelectedValue;
            dtCurrencyExchangeRate = dtCurrencyExchangeRate.DefaultView.ToTable();

            if (dtCurrencyExchangeRate != null && dtCurrencyExchangeRate.Rows.Count > 0)
            {
                txtCurrencyValue.Text = Convert.ToDecimal(dtCurrencyExchangeRate.Rows[0]["Exchange_Value"].ToString()).ToString();
            }
            else
            {
                if (ddlCurrencyCode.SelectedValue != "53")
                {
                    txtCurrencyValue.Text = "";
                    Utility.FunShowAlertMsg(this, "No Exchange Rate value found");
                    ddlCurrencyCode.SelectedIndex = -1;
                }
            }

        }
        else
        {
            txtCurrencyValue.Text = "";
        }
        defaultCurrency.Visible = false;
        // same currency code
        if (Convert.ToInt32(ddlCurrencyCode.SelectedValue) == Convert.ToInt32(ViewState["DefaultCurrencyID"]))
        {
            defaultCurrency.Visible = true;
        }

    }

    private void FunPriLoadBranch()
    {
        // branch
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //Procparam.Add("@User_ID", Convert.ToString(intUserID));
            //Procparam.Add("@Program_Id", "54");
            //if (ddlLOB.SelectedIndex > 0)
            //    Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
            //Procparam.Add("@Is_Active", "1");
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Branch_ID", "Branch" });
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_Id", "Location_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }

    }

    private void FunPriLoadPANUM()
    {
        try
        {
            if (ddlPayTo.SelectedValue == "11")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Option", "1");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                // --code commented and added by saran in 01-Aug-2014 Insurance start 

                //if (ddlLOB.SelectedIndex > 0)
                // --code commented and added by saran in 01-Aug-2014 Insurance end 
                Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
                if (ddlBranch.SelectedValue != "0")
                {
                    //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                    Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                }
                if (ViewState["hdnCustorEntityID"] != null && ViewState["hdnCustorEntityID"].ToString() != string.Empty)
                {

                    Procparam.Add("@Ins_Company_Id", ViewState["hdnCustorEntityID"].ToString());
                }
                if (grvPaymentDetails != null && grvPaymentDetails.FooterRow != null)
                {
                    //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                    //ddlFooterPrimeAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestInsuranceDetails", Procparam, new string[] { "PANum", "PANum" });
                }
                if (grvPaymentAdjustment != null && grvPaymentAdjustment.FooterRow != null)
                {
                    //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                    //ddlFooterPrimeAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestInsuranceDetails", Procparam, new string[] { "PANum", "PANum" });
                }


            }
            else
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Option", "1");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                // --code commented and added by saran in 01-Aug-2014 Insurance start 

                //if (ddlLOB.SelectedIndex > 0)
                // --code commented and added by saran in 01-Aug-2014 Insurance end 
                Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
                if (ddlBranch.SelectedValue != "0")
                {
                    //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                    Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                }
                if (ddlPayTo.SelectedIndex > 0)
                {
                    if (ddlPayTo.SelectedValue == "1")
                        Procparam.Add("@Payto", "144");
                    else if (ddlPayTo.SelectedValue == "50")
                        Procparam.Add("@OptionPay_To", "1");
                    else
                        Procparam.Add("@Payto", "145");

                }
                if (ViewState["hdnCustorEntityID"] != null && ViewState["hdnCustorEntityID"].ToString() != string.Empty)
                {

                    Procparam.Add("@EntityId", ViewState["hdnCustorEntityID"].ToString());
                }
                if (ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ft")
                {
                    Procparam.Add("@LOB_Code", "FT");
                }
                if (ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ol")
                {
                    Procparam.Add("@LOB_Code", "OL");
                }
                if (grvPaymentDetails != null && grvPaymentDetails.FooterRow != null)
                {
                    //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                    //ddlFooterPrimeAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "PANum", "PANum" });

                    //s3g to sfl - kuppu - july 12 -- starts
                    DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
                    UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
                    //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
                    TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
                    TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");

                    ddlFooterFlowType.SelectedIndex = 0;
                    //ddlFooterSL_Code.SelectedIndex = 0;
                    ddlFooterSL_Code.Clear();
                    txtFooterDescription.Text = string.Empty;
                    txtFooterAmount.Text = string.Empty;

                    // --ends--

                }
                if (grvPaymentAdjustment != null && grvPaymentAdjustment.FooterRow != null)
                {
                    //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                    //ddlFooterPrimeAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "PANum", "PANum" });

                    //s3g to sfl - kuppu - july 12 -- starts
                    DropDownList ddlFooterAddOrLess = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterAddOrLess");
                    DropDownList ddlFooterPayType = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType");
                    TextBox txtFooterDescription = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterDescription");
                    TextBox txtFooterAmount = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterAmount");

                    ddlFooterAddOrLess.SelectedIndex = 0;
                    ddlFooterPayType.SelectedIndex = 0;
                    txtFooterDescription.Text = string.Empty;
                    txtFooterAmount.Text = string.Empty;
                    // --ends--
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadRefDocnoIVE()
    {
        try
        {
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "16");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));

            if (ddlBranch.SelectedValue != "0")
            {
                //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            }
            if (ViewState["hdnCustorEntityID"] != null && ViewState["hdnCustorEntityID"].ToString() != string.Empty)
            {
                Procparam.Add("@EntityId", ViewState["hdnCustorEntityID"].ToString());
            }

            DropDownList ddlFooterRefDocNo = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterRefDocNo");
            ddlFooterRefDocNo.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "Ref_No", "Ref_No" });
            if (ddlFooterRefDocNo.Items.Count < 2)
            {
                ddlFooterFlowType.SelectedIndex = -1;
                grvPaymentDetails.Columns[4].Visible = false;

                //RequiredFieldValidator RFVddlPrimeAccountNumber = (RequiredFieldValidator)grvPaymentDetails.FooterRow.FindControl("RFVddlPrimeAccountNumber");
                //RFVddlPrimeAccountNumber.Enabled = true;

                UserControls_S3GAutoSuggest ddlPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                ddlPrimeAccountNumber.IsMandatory = true;

                Utility.FunShowAlertMsg(this, "Payment not made for the selected Customer/Entity");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadRefDocno()
    {
        try
        {
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "7");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            // --code commented and added by saran in 01-Aug-2014 Insurance start 

            //if (ddlLOB.SelectedIndex > 0)
            // --code commented and added by saran in 01-Aug-2014 Insurance end 
            Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
            if (ddlBranch.SelectedValue != "0")
            {
                //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            }
            Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
            if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                Procparam.Add("@SANum", ddlFooterSubAccountNumber.SelectedValue);
            if (ViewState["hdnCustorEntityID"] != null && ViewState["hdnCustorEntityID"].ToString() != string.Empty)
            {
                Procparam.Add("@EntityId", ViewState["hdnCustorEntityID"].ToString());
            }
            if (ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ol")
            {
                Procparam.Add("@LOB_Code", "OL");
            }
            DropDownList ddlFooterRefDocNo = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterRefDocNo");
            ddlFooterRefDocNo.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "Ref_No", "Ref_No" });
            if (ddlFooterRefDocNo.Items.Count < 2)
            {
                ddlFooterFlowType.SelectedIndex = -1;
                grvPaymentDetails.Columns[4].Visible = false;
                Utility.FunShowAlertMsg(this, "Payment already made / not applicable for the selected Prime Account Number");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriloadDim2Insurance()
    {
        try
        {
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterDIM2 = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterDIM2");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "3");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //if (ddlFooterPrimeAccountNumber.SelectedIndex > 0)
            Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
            if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                Procparam.Add("@SANum", ddlFooterSubAccountNumber.SelectedValue);
            if (ViewState["hdnCustorEntityID"] != null && ViewState["hdnCustorEntityID"].ToString() != string.Empty)
            {
                Procparam.Add("@Ins_Company_Id", ViewState["hdnCustorEntityID"].ToString());
            }
            if (txtValueDate.Text.Length > 0)//Validate the policy from date  with value date in payment request
                Procparam.Add("@From_Date", Utility.StringToDate(txtValueDate.Text).ToString());
            ddlFooterDIM2.BindDataTable("S3G_LOANAD_GetPaymentRequestInsuranceDetails", Procparam, new string[] { "ASSET_ID", "Asset_Code", "Asset_Description" });

            // // --code commented and added by saran in 01-Aug-2014 Insurance start 

            RequiredFieldValidator RFVddlFooterDIM2 = (RequiredFieldValidator)grvPaymentDetails.FooterRow.FindControl("RFVddlFooterDIM2");
            if (ddlFooterDIM2.Items.Count <= 1)
                RFVddlFooterDIM2.Enabled = false;
            else
                RFVddlFooterDIM2.Enabled = true;
            // --code commented and added by saran in 01-Aug-2014 Insurance end 

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadDim2(string Option)
    {
        try
        {
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterRefDocNo = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterRefDocNo");

            Procparam = new Dictionary<string, string>();
            if (Option == "5")
            {
                Procparam.Add("@Option", Option);
                Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
            }
            else if (Option == "6")
            {
                Procparam.Add("@Option", Option);
                Procparam.Add("@SANum", ddlFooterSubAccountNumber.SelectedValue);
                Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
            }
            else if (Option == "Asset")
            {
                Procparam.Add("@Option", "9");
                if (ddlFooterRefDocNo.SelectedIndex > 0)
                    Procparam.Add("@Ref_No", ddlFooterRefDocNo.SelectedValue);
                if (ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ol")
                {
                    Procparam.Add("@LOB_Code", "OL");
                }
            }
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (grvPaymentDetails != null && grvPaymentDetails.FooterRow != null)
            {
                DropDownList ddlFooterDIM2 = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterDIM2");
                if (Option == "Asset")
                {
                    ddlFooterDIM2.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "ASSET_ID", "Asset_Code", "Asset_Description" });
                }
                else
                {
                    ddlFooterDIM2.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "ASSET_ID", "Lease_Asset_No", "Asset_Description" });
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadSANUM(string Payreqgrid)
    {
        try
        {
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
            //DropDownList ddlFooterPrimeAccountNumberA = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumberA = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumberA = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSubAccountNumber");
            Procparam = new Dictionary<string, string>();

            if (ddlPayTo.SelectedValue == "50")
            {
                Procparam.Add("@Option", "2");
                Procparam.Add("@OptionPay_To", "1");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                if (Payreqgrid == "PD")
                {
                    //if (ddlFooterPrimeAccountNumber.SelectedIndex > 0)
                    Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
                    ddlFooterSubAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "SANum", "SANum" });
                }
                else
                {
                    //if (ddlFooterPrimeAccountNumberA.SelectedIndex > 0)
                    Procparam.Add("@PANum", ddlFooterPrimeAccountNumberA.SelectedValue);
                    ddlFooterSubAccountNumberA.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "SANum", "SANum" });
                }

            }
            else
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                Procparam.Add("@Type", "2");
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                Procparam.Add("@Is_Closed", "1");
                Procparam.Add("@ParamSA_Status", "0,6,7,26,45,47,55");
                if (Payreqgrid == "PD")
                {
                    //if (ddlFooterPrimeAccountNumber.SelectedIndex > 0)
                    //Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedItem.Text);
                    Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
                    ddlFooterSubAccountNumber.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "SANum", "SANum" });
                }
                else
                {
                    //if (ddlFooterPrimeAccountNumberA.SelectedIndex > 0)
                    //Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedItem.Text);
                    Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
                    ddlFooterSubAccountNumberA.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "SANum", "SANum" });
                }


            }
            if (ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ol")
            {
                if (ddlFooterSubAccountNumber.Items.Count < 2)
                {
                    FunPriLoadDim2("5");
                }

            }
            if (ddlPayTo.SelectedValue == "11")
            {
                FunPriloadDim2Insurance();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }

    }

    /// <summary>
    /// This method is used to populate GLcode for Insurance "Pay TO"
    /// </summary>
    private void FunPriloadGLSLCodeInsurance()
    {
        try
        {
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            //UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            //DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
            //DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            //DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
            ////DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            //UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            //TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
            //TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");
            //Label lblFooterActualAmount = (Label)grvPaymentDetails.FooterRow.FindControl("lblFooterActualAmount");

            // --code commented and added by saran in 01-Aug-2014 Insurance start 

            DropDownList ddlFooterDIM2 = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterDIM2");
            if (ddlFooterDIM2 != null)
            {
                if (ddlFooterDIM2.Items.Count <= 1)
                {
                    FunPriLoadSLcodes("PD", "");
                    FunPriLoadGLSLcodes();//payment details
                }
            }
            FunPriLoadSLcodes("PD", "");
            FunPriLoadGLSLcodes();//payment details
            // --code commented and added by saran in 01-Aug-2014 Insurance end 

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadGLSLcodes()
    {
        try
        {

            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
            //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");
            Label lblFooterActualAmount = (Label)grvPaymentDetails.FooterRow.FindControl("lblFooterActualAmount");


            DataSet Ds = FunGetGLSLForPayType(ddlFooterPrimeAccountNumber, ddlFooterSubAccountNumber, ddlFooterFlowType);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                ddlFooterGL_Code.SelectedValue = Ds.Tables[0].Rows[0]["GL_Code"].ToString();
                if (Convert.ToString(Ds.Tables[0].Rows[0]["SL_Code"]) != string.Empty && Convert.ToString(Ds.Tables[0].Rows[0]["SL_Code"]) != "")
                {
                    ddlFooterSL_Code.SelectedValue = Ds.Tables[0].Rows[0]["SL_Code"].ToString();
                    ddlFooterSL_Code.SelectedText = Ds.Tables[0].Rows[0]["SL_Code"].ToString();
                }
                txtFooterDescription.Text = Ds.Tables[0].Rows[0]["Description"].ToString();

                txtFooterAmount.Text = Ds.Tables[0].Rows[0]["Amount"].ToString();
                lblFooterActualAmount.Text = Ds.Tables[0].Rows[0]["Amount"].ToString();

                ddlFooterGL_Code.Enabled = ddlFooterSL_Code.Enabled = false;
            }
            else
            {

                if (Ds.Tables[1].Rows.Count > 0)
                {
                    if (Convert.ToInt32(Ds.Tables[1].Rows[0]["ErrorCode"].ToString()) == 1)
                    {
                        Utility.FunShowAlertMsg(this, "GL Code is not defined for the selected Pay Type");
                        ddlFooterFlowType.SelectedIndex = -1;
                        ddlFooterGL_Code.SelectedIndex = -1;
                        return;
                    }
                    else if (Convert.ToInt32(Ds.Tables[1].Rows[0]["ErrorCode"].ToString()) == 2)
                    {
                        Utility.FunShowAlertMsg(this, "Payment already made for this account");
                        ddlFooterFlowType.SelectedIndex = -1;
                        ddlFooterGL_Code.SelectedIndex = -1;
                        return;
                    }
                    else if (Convert.ToInt32(Ds.Tables[1].Rows[0]["ErrorCode"].ToString()) == 3)
                    {
                        Utility.FunShowAlertMsg(this, "Payment not made for this account");
                        ddlFooterFlowType.SelectedIndex = -1;
                        ddlFooterGL_Code.SelectedIndex = -1;
                        //if (ddlFooterPrimeAccountNumber.SelectedIndex > 0)
                        //    ddlFooterPrimeAccountNumber.SelectedIndex = -1;
                        ddlFooterPrimeAccountNumber.SelectedText = string.Empty;
                        ddlFooterPrimeAccountNumber.SelectedValue = "0";
                        if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                        {
                            ddlFooterSubAccountNumber.SelectedIndex = -1;
                            ddlFooterSubAccountNumber.ClearDropDownList();
                        }
                        return;
                    }
                    else if (Convert.ToInt32(Ds.Tables[1].Rows[0]["ErrorCode"].ToString()) == 4)
                    {
                        Utility.FunShowAlertMsg(this, "Payment not defined for the selected Customer/Entity");
                        ddlFooterFlowType.SelectedIndex = -1;
                        ddlFooterGL_Code.SelectedIndex = -1;
                        //if (ddlFooterPrimeAccountNumber.SelectedIndex > 0)
                        //    ddlFooterPrimeAccountNumber.SelectedIndex = -1;
                        ddlFooterPrimeAccountNumber.SelectedText = string.Empty;
                        ddlFooterPrimeAccountNumber.SelectedValue = "0";
                        if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                        {
                            ddlFooterSubAccountNumber.SelectedIndex = -1;
                            ddlFooterSubAccountNumber.ClearDropDownList();
                        }
                        return;
                    }
                }


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadGLSLcodesPaymentadjustment()
    {
        try
        {

            DropDownList ddlFooterAddOrLess = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterAddOrLess");
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType");
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterGL_Code");
            //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterDescription = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterAmount");
            Procparam = new Dictionary<string, string>();
            if (Convert.ToInt32(ddlFooterAddOrLess.SelectedValue) > 0)
            {
                Procparam.Add("@Option", "15");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@FlowTypeID", ddlFooterFlowType.SelectedValue);
                Procparam.Add("@FlowDescription", ddlFooterFlowType.SelectedItem.Text);
                Procparam.Add("@AddorLess", ddlFooterAddOrLess.SelectedValue);
                Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
                //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                if (ddlPayTo.SelectedValue == "50")
                    Procparam.Add("@OptionPay_To", "1");

                DataTable Dtaddorless = new DataTable();
                Dtaddorless = Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam);
                if (Dtaddorless.Rows.Count > 0)
                {
                    ddlFooterGL_Code.FillDataTable(Dtaddorless, "GL_Code", "GL_Code", false);
                    ddlFooterGL_Code.SelectedValue = Dtaddorless.Rows[0]["GL_Code"].ToString();
                    if (Convert.ToString(Dtaddorless.Rows[0]["SL_Code"]) != string.Empty && Convert.ToString(Dtaddorless.Rows[0]["SL_Code"]) != "")
                    {
                        ddlFooterSL_Code.SelectedValue = Dtaddorless.Rows[0]["SL_Code"].ToString();
                        ddlFooterSL_Code.SelectedText = Dtaddorless.Rows[0]["SL_Code"].ToString();
                    }
                    txtFooterDescription.Text = Dtaddorless.Rows[0]["Description"].ToString();
                }
                else
                {
                    ddlFooterFlowType.SelectedIndex = -1;
                    Utility.FunShowAlertMsg(this, "GL Code is not defined for the selected Pay Type");
                    return;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "select Add or Less");
                return;
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private DataSet FunGetGLSLForPayType(UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber, DropDownList ddlFooterSubAccountNumber, DropDownList ddlFooterFlowType)
    {
        DataSet Ds = new DataSet();
        try
        {
            Procparam = new Dictionary<string, string>();
            if (ddlFooterFlowType.SelectedIndex > 0)
            {
                //if ((ddlFooterFlowType.SelectedValue == "64" || ddlFooterFlowType.SelectedValue == "66" || ddlFooterFlowType.SelectedValue == "68" || ddlFooterFlowType.SelectedValue == "69") && ddlPayTo.SelectedValue != "50")
                //{
                //    Procparam.Add("@Option", "4");
                //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                //    Procparam.Add("@FlowTypeID", ddlFooterFlowType.SelectedValue);
                //    Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
                //    //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                //    Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                //}
                //else
                //{
                Procparam.Add("@Option", "3");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                // --code commented and added by saran in 01-Aug-2014 Insurance start 

                //if (ddlLOB.SelectedIndex > 0)
                // --code commented and added by saran in 01-Aug-2014 Insurance end 
                Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
                //if (ddlFooterPrimeAccountNumber.SelectedIndex > 0)
                Procparam.Add("@PANum", (Convert.ToInt64(ddlFooterPrimeAccountNumber.SelectedValue) > 0) ? Convert.ToString(ddlFooterPrimeAccountNumber.SelectedText) : "");
                //Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
                if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                    Procparam.Add("@SANum", ddlFooterSubAccountNumber.SelectedValue);
                Procparam.Add("@FlowTypeID", ddlFooterFlowType.SelectedValue);
                if (ddlPayTo.SelectedValue == "1")
                    Procparam.Add("@Payto", "144");
                else if (ddlPayTo.SelectedValue == "50")
                {
                    if (ChkContractRef.Checked)
                        Procparam.Add("@OptionPay_To", "2");//General with contract ref
                    else
                        Procparam.Add("@OptionPay_To", "1");
                }
                // --code commented and added by saran in 01-Aug-2014 Insurance start 

                else if (ddlPayTo.SelectedValue == "11")
                    Procparam.Add("@Payto", "236");
                // --code commented and added by saran in 01-Aug-2014 Insurance end 

                else
                    Procparam.Add("@Payto", "145");
                if (ViewState["hdnCustorEntityID"] != null && ViewState["hdnCustorEntityID"].ToString() != string.Empty)
                {

                    Procparam.Add("@EntityId", ViewState["hdnCustorEntityID"].ToString());
                }
                //}
                Ds = Utility.GetDataset("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam);

            }
        }
        catch (Exception objException)
        {
            Ds = null;
        }

        return Ds;
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

        FunClearForm();
        ddlBranch.Clear();
        FunPriLoadBranch();
        if (ddlLOB.SelectedIndex > 0)
        {
            ddlPayTo.Enabled = true;
            if (ddlLOB.SelectedItem.Text.Contains("FT"))
            {
                grvPaymentDetails.Columns[3].HeaderText = "Party";
                grvPaymentDetails.Columns[6].Visible = false;
                grvPaymentDetails.Columns[7].Visible = true;
            }
            else
            {
                grvPaymentDetails.Columns[3].HeaderText = "Cashflow ID";
                grvPaymentDetails.Columns[6].Visible = true;
                grvPaymentDetails.Columns[7].Visible = false;
            }

        }
        else
        {
            ddlPayTo.Enabled = false;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClearForm();
    }

    protected void btnCancelPass_Click(object sender, EventArgs e)
    {
        chkAccountBased.SelectedValue = "0";
    }

    protected void btnPassword_Click(object sender, EventArgs e)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {

            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
            {
                Utility.FunShowAlertMsg(this, "Incorrect Password");
                chkAccountBased.SelectedValue = "0";
            }
            else   // correct password
            {
                if (ddlPayTo.SelectedValue == "50")
                {
                    FunProAcctBasedCtrlsValidation(false);
                    FunPriLoadGLcodes();
                }
                else
                {
                    FunProAcctBasedCtrlsValidation(true);
                    FunPriLoadGLcodes();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjS3GAdminServices.Close();
        }
    }

    protected void FunProAcctBasedCtrlsValidation(bool blnflaG)
    {
        try
        {
            PanelPayType.Enabled = blnflaG;
            ChkContractRef.Enabled = false;
            if (ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue == "1")
            {
                FunDisableGLSLCodeGrid(true);
                FundisableGridValidation(false);
                ChkContractRef.Enabled = true;
            }
            else
            {
                FunDisableGLSLCodeGrid(false);
                FundisableGridValidation(true);
            }
            FunSetPASANumCtrls();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunSetPASANumCtrls()
    {
        try
        {
            bool blnflaG;
            if (ChkContractRef.Checked)//Contract ref
                blnflaG = true;
            else
                blnflaG = false;
            if (grvPaymentDetails.FooterRow != null)
            {
                DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber");
                DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
                UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");

                ddlFooterPrimeAccountNumber.Enabled = ddlFooterSubAccountNumber.Enabled = ddlFooterFlowType.Enabled = blnflaG;
            }
            if (grvPaymentAdjustment.FooterRow != null)
            {
                DropDownList ddlFooterPayType = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType");
                UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
                DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSubAccountNumber");
                ddlFooterPayType.Enabled = ddlFooterPrimeAccountNumber.Enabled = ddlFooterSubAccountNumber.Enabled = blnflaG;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void ChkContractRef_CheckedChanged(object sender, EventArgs e)
    {
        FunSetPASANumCtrls();
    }

    protected void chkAccountBased_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (chkAccountBased.SelectedValue == "1")
        {
            lblErrorMessagePass.Text = string.Empty;
            if (ModalPopupExtenderPassword.Enabled == false)
            {
                ModalPopupExtenderPassword.Enabled = true;

            }

            ModalPopupExtenderPassword.Show();
        }
        else //If Account based is no
        {
            fundisablepaymentdtlsgrid();
            FunProAcctBasedCtrlsValidation(true);
            FunPriLoadGLcodes();
            FunPriLoadPaytypeingrid(Convert.ToString(ddlPayTo.SelectedValue));
        }
    }

    private bool IsUserValid(string pass)
    {
        CreditMgtServicesReference.CreditMgtServicesClient ObjMgtCreditMgtClient;
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            string userName = userInfo.ProUserNameRW;

            CreditMgtServices.S3G_ORG_UserIsValidRow ObjUserIsValidRow;
            CreditMgtServices.S3G_ORG_UserIsValidDataTable ObjUserIsValidDataTable = new CreditMgtServices.S3G_ORG_UserIsValidDataTable();


            ObjUserIsValidRow = ObjUserIsValidDataTable.NewS3G_ORG_UserIsValidRow();

            ObjUserIsValidRow.User_Name = userName;
            ObjUserIsValidRow.Password = pass;

            ObjUserIsValidDataTable.AddS3G_ORG_UserIsValidRow(ObjUserIsValidRow);

            SerializationMode SerMode = new SerializationMode();

            byte[] bytesUserValid = ObjMgtCreditMgtClient.FunPubQueryUserIsValid(SerMode, ClsPubSerialize.Serialize(ObjUserIsValidDataTable, SerMode));
            DataTable dt_User = (CreditMgtServices.S3G_ORG_UserIsValidDataTable)ClsPubSerialize.DeSerialize(bytesUserValid, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_UserIsValidDataTable));

            if (dt_User != null && dt_User.Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();// Changed by Manikandan. R to close object
            // ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect(strRedirectOnCancel);
    }

    private void FunClearPaymentAdjustgridfooter()
    {
        //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
        UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
        DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
        DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
        //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
        UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
        TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
        TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");
        txtFooterDescription.Text = txtFooterAmount.Text = "";
        ddlFooterGL_Code.SelectedIndex = ddlFooterFlowType.SelectedIndex = -1;
        //ddlFooterSL_Code.SelectedIndex = 
        ddlFooterSL_Code.Clear();
    }

    private void FunClearForm()
    {
        try
        {

            ddlPayTo.SelectedIndex =
                //ddlCurrencyCode.SelectedIndex=
                //ddlPaytypeadjust.SelectedIndex= -1;
            ddlPayMode.SelectedIndex = -1;
            txtCurrencyValue.Text =
                txtDocAmount.Text =
                //txtValueDate.Text = 
                "";
            //FunPriClearEntityCodeControls();
            PnlCustEntityInformation.Enabled = true;

            //FunPriClearEntityCodeControls();
            //FunPriClearCustomerCodeControls();
            FunGridBlank();
            PnlCustEntityInformation.Enabled = true;
            ucCustomerCodeLov.ButtonEnabled = false;
            chkAccountBased.SelectedValue = "0";
            chkAccountBased.Enabled = false;

            FunPriClearCustomerDtls();
            if (ViewState["DefaultCurrencyID"] != null && ViewState["DefaultCurrencyID"].ToString() != "-1")
            {
                ddlCurrencyCode.SelectedValue = ViewState["DefaultCurrencyID"].ToString();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunGridBlank()
    {
        try
        {
            if (grvPaymentAdjustment != null)
                FunpriBlank();
            if (grvPaymentAdjustment.FooterRow != null)
                grvPaymentAdjustment.FooterRow.Visible = true;

            if (grvPaymentDetails != null)
            {
                fundisablepaymentdtlsgrid();
                if (grvPaymentDetails.FooterRow != null && Convert.ToInt32(ddlPaymentType.SelectedValue) > 4)
                {
                    grvPaymentDetails.FooterRow.Visible = true;
                    grvPaymentDetails.Columns[9].Visible = true;
                }
                else
                {
                    grvPaymentDetails.FooterRow.Visible = false;
                }
            }

            if (grvFunderReceipt != null)
            {
                ViewState["FunderPmtDtl"] = (DataTable)ViewState["DefaultFunderPmtDtl"];
                FunPriBindFunderGrid((DataTable)ViewState["DefaultFunderPmtDtl"]);
            }

            grvFunderReceipt.Visible = grvPaymentDetails.Visible = false;
            FunPriBindGridDtls(2, null);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void Funupdatedatatable()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["grvPaymentDetails"];
            if (dt.Rows.Count > 0)
            {
                if (grvPaymentDetails != null)
                    for (int tableRow = 0; tableRow < grvPaymentDetails.Rows.Count; tableRow++)
                    {
                        dt.Rows[tableRow].BeginEdit();
                        TextBox txtEntered_Amount = (TextBox)grvPaymentDetails.Rows[tableRow].FindControl("lblAmount");
                        TextBox lblDescription = (TextBox)grvPaymentDetails.Rows[tableRow].FindControl("lblDescription");
                        dt.Rows[tableRow]["Amount"] = txtEntered_Amount.Text.ToString();
                        dt.Rows[tableRow]["Description"] = lblDescription.Text.ToString();
                        dt.Rows[tableRow].EndEdit();
                        dt.AcceptChanges();
                    }
                ViewState["grvPaymentDetails"] = dt;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void FunProLoadFTAccounts(string strCustomer)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_Id", intCompanyID.ToString());
        Procparam.Add("@Option", ddlPayTo.SelectedValue.ToString());
        Procparam.Add("@Lob_Id", ddlLOB.SelectedValue.ToString());
        Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
        Procparam.Add("@Customer", strCustomer);

        if (grvPaymentDetails != null && grvPaymentDetails.FooterRow != null)
        {
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            //UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            //ddlFooterPrimeAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestFTAcc", Procparam, new string[] { "PANum", "PANum" });

            //s3g to sfl - kuppu - july 12 -- starts
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");

            ddlFooterFlowType.SelectedIndex = 0;
            //ddlFooterSL_Code.SelectedIndex = 0;
            ddlFooterSL_Code.Clear();
            txtFooterDescription.Text = string.Empty;
            txtFooterAmount.Text = string.Empty;

            // --ends--

        }
        if (grvPaymentAdjustment != null && grvPaymentAdjustment.FooterRow != null)
        {
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            //ddlFooterPrimeAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestFTAcc", Procparam, new string[] { "PANum", "PANum" });

            //s3g to sfl - kuppu - july 12 -- starts
            DropDownList ddlFooterAddOrLess = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterAddOrLess");
            DropDownList ddlFooterPayType = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType");
            TextBox txtFooterDescription = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterAmount");

            ddlFooterAddOrLess.SelectedIndex = 0;
            ddlFooterPayType.SelectedIndex = 0;
            txtFooterDescription.Text = string.Empty;
            txtFooterAmount.Text = string.Empty;
            // --ends--
        }
    }

    protected void lnkAddAdjust_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtPaymentdetails = new DataTable();
            DataTable dtpaymentfunderdet = new DataTable();
            dtPaymentdetails = (DataTable)ViewState["grvPaymentDetails"];
            dtpaymentfunderdet = (DataTable)ViewState["FunderPmtDtl"];

            if (dtPaymentdetails != null)
            {
                if (dtPaymentdetails.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one record for payment details");
                    return;
                }
                else if (dtPaymentdetails.Rows.Count == 1 && (dtPaymentdetails.Rows[0]["PANum"].ToString() == "-1"))
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one record for payment details");
                    return;
                }
            }
            //else
            //{
            //    Utility.FunShowAlertMsg(this, "Add atleast one record for payment details");
            //    return;
            //}

            if (dtpaymentfunderdet != null && (ddlPaymentType.SelectedValue.ToString() == "5" || ddlPaymentType.SelectedValue.ToString() == "8"))
            {
                if (dtpaymentfunderdet.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one record for payment details");
                    return;
                }
                else if (dtpaymentfunderdet.Rows.Count == 1 && (dtpaymentfunderdet.Rows[0]["Note_ID"].ToString() == "0"))
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one record for payment details");
                    return;
                }
            }
            //else
            //{
            //    Utility.FunShowAlertMsg(this, "Add atleast one record for payment details");
            //    return;
            //}

            DropDownList ddlFooterAddOrLess = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterAddOrLess");
            DropDownList ddlFooterSubAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterPayType = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType");
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterGL_Code");
            //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterDescription = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterAmount");
            DataTable dtPmtAdjust = new DataTable();
            if (ViewState["grvPaymentAdjust"] != null)
            {
                dtPmtAdjust = (DataTable)ViewState["grvPaymentAdjust"];
            }

            //if (ddlFooterSubAccountNumber != null && ddlFooterPrimeAccountNumber.SelectedIndex > 0 && ddlFooterSubAccountNumber.Items.Count > 1)
            if (ddlFooterSubAccountNumber != null && ddlFooterSubAccountNumber.Items.Count > 1)
            {
                if (ddlFooterSubAccountNumber.SelectedIndex < 1)
                {
                    Utility.FunShowAlertMsg(this, "Select the sub account number.");
                    return;
                }
            }

            //To do
            //if (ddlFooterSL_Code.Items.Count > 1 && ddlFooterSL_Code.SelectedIndex == 0 && ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue == "1")
            //{
            //    Utility.FunShowAlertMsg(this, "Select the SL Code.");
            //    return;
            //}

            if (ddlFooterSL_Code.AvailableRecords > 1 && ddlFooterSL_Code.SelectedValue == "0" && ddlPayTo.SelectedValue == "50" && chkAccountBased.SelectedValue == "1")
            {
                Utility.FunShowAlertMsg(this, "select the sl code.");
                return;
            }

            DataRow dtPaymentRow = dtPmtAdjust.NewRow();

            dtPaymentRow["AddOrLess"] = ddlFooterAddOrLess.SelectedItem.Text;
            if (ddlFooterPayType.SelectedIndex > 0)
            {
                dtPaymentRow["PayType"] = ddlFooterPayType.SelectedItem.Text;
                dtPaymentRow["PayTypeID"] = ddlFooterPayType.SelectedItem.Value;
            }
            if (ddlFooterPrimeAccountNumber.SelectedValue != "0")
                dtPaymentRow["PANum"] = ddlFooterPrimeAccountNumber.SelectedValue;
            if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                dtPaymentRow["SANum"] = ddlFooterSubAccountNumber.SelectedItem.Value;
            if (ddlFooterGL_Code.SelectedValue != null) //Changed here for GLcode Add issue to grid
                dtPaymentRow["GL_Code"] = ddlFooterGL_Code.SelectedValue;
            //if (ddlFooterSL_Code.SelectedIndex > 0)
            if (ddlFooterSL_Code.SelectedValue != "0")
                dtPaymentRow["SL_Code"] = ddlFooterSL_Code.SelectedValue;
            dtPaymentRow["Remarks"] = txtFooterDescription.Text;
            dtPaymentRow["Amount"] = txtFooterAmount.Text;

            dtPmtAdjust.Rows.Add(dtPaymentRow);

            if (dtPmtAdjust.Rows[0]["AddOrLess"].ToString() == "")
            {
                dtPmtAdjust.Rows.RemoveAt(0);
            }

            grvPaymentAdjustment.DataSource = dtPmtAdjust;
            ViewState["grvPaymentAdjust"] = dtPmtAdjust;
            grvPaymentAdjustment.DataBind();

            FunPriLoadPANUM();
            FunPriLoadGLcodes();
            FunPriLoadPaytypeingrid(Convert.ToString(ddlPayTo.SelectedValue));

            if (ddlPayTo.SelectedValue == "50")
            {

                if (chkAccountBased.SelectedValue == "1")
                {
                    FunDisableGLSLCodeGrid(true);
                    FundisableGridValidation(false);
                    FunProAcctBasedCtrlsValidation(false);
                }
                else
                {
                    FunDisableGLSLCodeGrid(false);
                    FundisableGridValidation(true);
                    FunProAcctBasedCtrlsValidation(true);
                }
            }
            else
            {
                FunDisableGLSLCodeGrid(false);
                FundisableGridValidation(true);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }


    }

    protected void grvPaymentAdjustment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtPmtAdjust = new DataTable();
            if (ViewState["grvPaymentAdjust"] != null)
            {
                dtPmtAdjust = (DataTable)ViewState["grvPaymentAdjust"];
            }
            dtPmtAdjust.Rows.RemoveAt(e.RowIndex);
            if (dtPmtAdjust.Rows.Count == 0)
            {
                FunpriBlank();
            }
            else
            {
                grvPaymentAdjustment.DataSource = dtPmtAdjust;
                ViewState["grvPaymentAdjust"] = dtPmtAdjust;
                grvPaymentAdjustment.DataBind();
            }
            FunPriLoadPANUM();
            FunPriLoadGLcodes();
            FunPriLoadPaytypeingrid(Convert.ToString(ddlPayTo.SelectedValue));
            if (ddlPayTo.SelectedValue == "50")
            {

                if (chkAccountBased.SelectedValue == "1")
                {
                    FunDisableGLSLCodeGrid(true);
                    FundisableGridValidation(false);
                    FunProAcctBasedCtrlsValidation(false);
                }
                else
                {
                    FunDisableGLSLCodeGrid(false);
                    FundisableGridValidation(true);
                    FunProAcctBasedCtrlsValidation(true);
                }
            }
            else
            {
                FunDisableGLSLCodeGrid(false);
                FundisableGridValidation(true);
            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, "Unable to delete.");
        }


    }

    protected void grvPaymentAdjustment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriCalcSumAmountDetails();
            FunPriCalcSumAmount();

            if (string.Compare(strQsMode, "Q") == 0)
            {
                LinkButton lnkRemove = (LinkButton)e.Row.FindControl("lnkRemove");
                if (lnkRemove != null)
                    lnkRemove.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }

    }

    private bool Is_DuplicatePayment(string PrimeAccountNumber, string SubAccountNumber, string FlowType, string GL_code, string SL_code)
    {
        try
        {
            Boolean Rtrn = false;
            DataTable dtpmtdtls = new DataTable();
            if (ViewState["grvPaymentDetails"] != null)
                dtpmtdtls = (DataTable)ViewState["grvPaymentDetails"];
            if (PrimeAccountNumber == "0")
                PrimeAccountNumber = "";
            if (SubAccountNumber == "0")
                SubAccountNumber = "";
            if (FlowType == "0")
                FlowType = "";
            if (GL_code == "0")
                GL_code = "";
            if (SL_code == "0")
                SL_code = "";
            if (dtpmtdtls.Rows.Count == 0)
            {
                Rtrn = false;
            }
            else
            {
                for (int rowcount = 0; rowcount < dtpmtdtls.Rows.Count; rowcount++)
                {
                    if ((PrimeAccountNumber == Convert.ToString(dtpmtdtls.Rows[rowcount]["PA_SA_REF_ID"])) &&
                        (SubAccountNumber == Convert.ToString(dtpmtdtls.Rows[rowcount]["SANum"])) &&
                        (FlowType == Convert.ToString(dtpmtdtls.Rows[rowcount]["CashFlow_ID"])) &&
                        (GL_code == Convert.ToString(dtpmtdtls.Rows[rowcount]["GL_Code"])) &&
                        (SL_code == Convert.ToString(dtpmtdtls.Rows[rowcount]["SL_Code"])))
                    {
                        Rtrn = true;
                    }
                }

            }
            return Rtrn;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void ddlFooterDIM2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlFooterDIM2 = (DropDownList)sender;
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            //DropDownList ddlFooterPrimeAccountNumber = ((DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber"));
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumber = ((DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSubAccountNumber"));
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
            //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");
            Label lblFooterActualAmount = (Label)grvPaymentDetails.FooterRow.FindControl("lblFooterActualAmount");
            DataSet Ds = new DataSet();

            //ddlFooterSL_Code.SelectedIndex = 
            ddlFooterSL_Code.Clear();
            ddlFooterGL_Code.SelectedIndex = -1;
            txtFooterDescription.Text = txtFooterAmount.Text = "";
            if (ddlFooterDIM2.SelectedIndex > 0)
            {
                if (ddlPayTo.SelectedValue == "11")
                {
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Option", "4");
                    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    if (ddlFooterFlowType.SelectedIndex > 0)
                        Procparam.Add("@FlowTypeID", ddlFooterFlowType.SelectedValue);
                    if (ddlFooterDIM2.SelectedIndex > 0)
                        Procparam.Add("@Asset_Ins_Det_Id", ddlFooterDIM2.SelectedValue);
                    Ds = Utility.GetDataset("S3G_LOANAD_GetPaymentRequestInsuranceDetails", Procparam);
                }
                else
                {
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Option", "13");
                    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    //if (ddlFooterPrimeAccountNumber.SelectedIndex > 0)
                    Procparam.Add("@PANum", ddlFooterPrimeAccountNumber.SelectedValue);
                    if (ddlFooterSubAccountNumber.SelectedIndex > 0)
                        Procparam.Add("@SANum", ddlFooterSubAccountNumber.SelectedValue);
                    Procparam.Add("@FlowTypeID", ddlFooterFlowType.SelectedValue);
                    if (ddlPayTo.SelectedValue == "1")
                        Procparam.Add("@Payto", "144");
                    else if (ddlPayTo.SelectedValue == "50")
                        Procparam.Add("@OptionPay_To", "1");
                    else
                        Procparam.Add("@Payto", "145");
                    if (ViewState["hdnCustorEntityID"] != null && ViewState["hdnCustorEntityID"].ToString() != string.Empty)
                    {
                        Procparam.Add("@EntityId", ViewState["hdnCustorEntityID"].ToString());
                    }
                    Procparam.Add("@Asset_ID", ddlFooterDIM2.SelectedValue);
                    Ds = Utility.GetDataset("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam);
                }
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    ddlFooterGL_Code.SelectedValue = Ds.Tables[0].Rows[0]["GL_Code"].ToString();
                    ddlFooterSL_Code.SelectedValue = Ds.Tables[0].Rows[0]["SL_Code"].ToString();
                    txtFooterDescription.Text = Ds.Tables[0].Rows[0]["Description"].ToString();
                    txtFooterAmount.Text = Ds.Tables[0].Rows[0]["Amount"].ToString();
                    lblFooterActualAmount.Text = Ds.Tables[0].Rows[0]["ActualAmount"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void ddlFooterRefDocNo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadSLcodes("PD", "");
            DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
            DropDownList ddlFooterRefDocNo = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterRefDocNo");
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
            //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
            DropDownList ddlFooterDIM2 = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterDIM2");
            TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");
            Label lblFooterActualAmount = (Label)grvPaymentDetails.FooterRow.FindControl("lblFooterActualAmount");
            //FunPriLoadDim2("Asset");
            //ddlFooterSL_Code.SelectedIndex = 
            ddlFooterSL_Code.Clear();
            ddlFooterGL_Code.SelectedIndex = ddlFooterDIM2.SelectedIndex = -1;
            txtFooterDescription.Text = txtFooterAmount.Text = "";

            if (ddlFooterRefDocNo.SelectedIndex > 0)
            {
                Procparam = new Dictionary<string, string>();
                if (ddlFooterFlowType.SelectedValue == "202")
                    Procparam.Add("@Option", "8");
                else if (ddlFooterFlowType.SelectedValue == "215")
                    Procparam.Add("@Option", "17");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                // --code commented and added by saran in 01-Aug-2014 Insurance start 

                //if (ddlLOB.SelectedIndex > 0)
                // --code commented and added by saran in 01-Aug-2014 Insurance end 
                Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
                if (ddlBranch.SelectedValue != "0")
                {
                    //   Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                    Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                }
                if (ddlFooterRefDocNo.SelectedIndex > 0)
                    Procparam.Add("@Ref_No", ddlFooterRefDocNo.SelectedValue);
                DataSet Ds = new DataSet();
                Ds = Utility.GetDataset("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    ddlFooterGL_Code.SelectedValue = Ds.Tables[0].Rows[0]["GL_Code"].ToString();
                    ddlFooterSL_Code.SelectedValue = Ds.Tables[0].Rows[0]["SL_Code"].ToString();
                    txtFooterDescription.Text = Ds.Tables[0].Rows[0]["Description"].ToString();
                    txtFooterAmount.Text = Ds.Tables[0].Rows[0]["Amount"].ToString();
                    lblFooterActualAmount.Text = Ds.Tables[0].Rows[0]["ActualAmount"].ToString();
                }
                if (ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ol")
                {
                    if (Ds.Tables.Count > 1)
                        if (Ds.Tables[1].Rows.Count > 0)
                        {
                            FunPriLoadDim2("Asset");
                            ddlFooterDIM2.SelectedValue = Ds.Tables[1].Rows[0]["ASSET_ID"].ToString();
                            ddlFooterDIM2.ClearDropDownList();
                        }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void ddlFooterAddOrLess_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlFooterAddOrLess = (DropDownList)sender;
            DropDownList ddlFooterPayType = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType");
            //DropDownList ddlFooterPrimeAccountNumberA = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumberA = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            DropDownList ddlFooterSubAccountNumberA = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterGL_Code");
            //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterAmount = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterAmount");
            TextBox txtFooterDescription = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterDescription");
            ddlFooterPrimeAccountNumberA.SelectedText = string.Empty;
            ddlFooterPrimeAccountNumberA.SelectedValue = "0";
            //ddlFooterPrimeAccountNumberA.SelectedIndex =
            ddlFooterSubAccountNumberA.SelectedIndex = ddlFooterPayType.SelectedIndex =
        ddlFooterGL_Code.SelectedIndex = -1;
            //ddlFooterSL_Code.SelectedIndex = -1;
            ddlFooterSL_Code.Clear();
            txtFooterAmount.Text = txtFooterDescription.Text = "";

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "4");
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@AddorLess", ddlFooterAddOrLess.SelectedValue);
            if (ddlPayTo.SelectedValue == "50")
                Procparam.Add("@OptionPay_To", "1");
            ddlFooterPayType.BindDataTable("S3G_LOANAD_GetPaymentTypedetails", Procparam, new string[] { "CashFlow_ID", "CashFlowFlag_Desc" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void ddlFooterPayType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlFooterPayType = (DropDownList)sender;
            DropDownList ddlFooterSubAccountNumberA = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSubAccountNumber");
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterGL_Code");
            //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
            UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterSL_Code");
            TextBox txtFooterAmount = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterAmount");
            TextBox txtFooterDescription = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterDescription");
            ddlFooterGL_Code.SelectedIndex = -1;
            //ddlFooterSL_Code.SelectedIndex = -1;
            txtFooterAmount.Text = txtFooterDescription.Text = "";
            if (ddlFooterPayType.SelectedIndex > 0)
            {
                if (ddlFooterSubAccountNumberA.Items.Count > 1)
                {
                    if (ddlFooterSubAccountNumberA.SelectedIndex > 0)
                    {
                        FunPriLoadSLcodes("PD", "");
                        //FunPriLoadGLSLcodes();//Adjustment
                        FunPriLoadGLSLcodesPaymentadjustment();
                    }
                    else
                    {
                        ddlFooterPayType.SelectedIndex = -1;
                        Utility.FunShowAlertMsg(this, "Select the sub account number");
                        return;
                    }
                }
                else
                {
                    FunPriLoadSLcodes("PD", "");
                    //FunPriLoadGLSLcodes();//Adjustment
                    FunPriLoadGLSLcodesPaymentadjustment();
                }

            }
            else
            {
                ddlFooterGL_Code.SelectedIndex = -1;
                //ddlFooterSL_Code.SelectedIndex = -1;
                ddlFooterSL_Code.Clear();
                txtFooterAmount.Text = txtFooterDescription.Text = "";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    //Payment Details
    protected void ddlFooterGL_Code_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlFooterGL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
            if (ddlFooterGL_Code.SelectedIndex > 0)
                FunPriLoadSLcodes("PD", ddlFooterGL_Code.SelectedValue);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    //Payment Adjustment
    protected void ddlFooterGL_CodeA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlFooterGL_Code = (DropDownList)sender;
            DropDownList ddlFooterPayType = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType");
            TextBox txtFooterDescription = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterDescription");
            TextBox txtFooterAmount = (TextBox)grvPaymentAdjustment.FooterRow.FindControl("txtFooterAmount");

            ddlFooterPayType.SelectedIndex = -1;
            txtFooterAmount.Text = txtFooterDescription.Text = "";
            if (ddlFooterGL_Code.SelectedIndex > 0)
                FunPriLoadSLcodes("PA", ddlFooterGL_Code.SelectedValue);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }

    }

    //Payment Details
    protected void ddlFooterPrimeAccountNumberA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
        UserControls_S3GAutoSuggest ddlFooterPrimeAccountNumber = (UserControls_S3GAutoSuggest)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
        DropDownList ddlFooterPayType = (DropDownList)grvPaymentAdjustment.FooterRow.FindControl("ddlFooterPayType");
        FunClearPaymentAdjustgridfooter();
        //if (ddlFooterPrimeAccountNumber != null && ddlFooterPrimeAccountNumber.SelectedIndex > 0
        if (ddlFooterPrimeAccountNumber != null && ddlFooterPrimeAccountNumber.SelectedValue != "0")
        {
            FunPriLoadSANUM("PA");
            ddlFooterPayType.SelectedIndex = -1;
        }
        else
            ddlFooterPayType.SelectedIndex = -1;
    }

    protected void btnCancelPayment_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtPaymentRequestNo.Text))
            {
                Utility.FunShowAlertMsg(this, "Select the Requested number to cancel");
            }
            else
            {
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();

                Procparam.Add("@Payment_Request_No", txtPaymentRequestNo.Text);
                Procparam.Add("@ErrorCode", "-1");
                Procparam.Add("@UserID", intUserID.ToString());
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                DataTable dt = Utility.GetDefaultData(SPNames.S3G_LOANAD_CancelPaymentRequest, Procparam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (!(string.IsNullOrEmpty(dt.Rows[0]["ErrorCode"].ToString())))
                    {
                        if ((Convert.ToInt32(dt.Rows[0]["ErrorCode"].ToString())) == 1)
                        {
                            //Utility.FunShowAlertMsg(this, "Payment Request Cancelled successfully");
                            //ddlPaymentStatus.SelectedValue = "5";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Payment Request cancelled successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PARE';", true);
                            return;
                        }
                        else
                        {
                            Utility.FunShowAlertMsg(this, "Error in cancelling this Payment Request");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, "Error in cancelling this Payment Request");
        }

    }

    private int FunPriInsertInstrumentdetails()
    {
        ObjLoanAdminAccMgtServicesClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
        try
        {
            int ErrorCode = 1;
            LoanAdminAccMgtServices.S3G_LOANAD_InstrumentdetailsDataTable InstrumentdetailsDataTable = new LoanAdminAccMgtServices.S3G_LOANAD_InstrumentdetailsDataTable();
            LoanAdminAccMgtServices.S3G_LOANAD_InstrumentdetailsRow InstrumentdetailsRow = InstrumentdetailsDataTable.NewS3G_LOANAD_InstrumentdetailsRow();
            InstrumentdetailsDataTable.NewS3G_LOANAD_InstrumentdetailsRow();

            InstrumentdetailsRow.Company_ID = intCompanyID;
            if (Convert.ToInt32(ddlAcctNumber.SelectedValue) > 0 && ddlPayMode.SelectedValue == "1")
            {
                InstrumentdetailsRow.Bank_ID = Convert.ToInt32(ddlAcctNumber.SelectedValue);
            }
            else if (Convert.ToInt32(ddlbankname.SelectedValue) > 0 && ddlPayMode.SelectedValue == "2")
            {
                InstrumentdetailsRow.Bank_ID = Convert.ToInt32(ddlbankname.SelectedValue);
            }
            else
            {
                InstrumentdetailsRow.Bank_ID = 0;
            }
            if (ddlPayMode.SelectedValue == "1")
            {
                InstrumentdetailsRow.Instrument_Type = "C";

            }
            else if (ddlPayMode.SelectedValue == "2")
            {
                InstrumentdetailsRow.Instrument_Type = "D";

            }
            InstrumentdetailsRow.Instrument_No = txtInstrumentNumber.Text;
            InstrumentdetailsRow.Instrument_Status = true;
            InstrumentdetailsRow.Instrument_Amount = Convert.ToDecimal(txtDocAmount.Text);
            InstrumentdetailsRow.Instrument_Date = Utility.StringToDate(txtInstrumentDate.Text);
            InstrumentdetailsRow.Remarks = txtRemarks.Text;
            InstrumentdetailsRow.Created_By = intUserID;
            InstrumentdetailsRow.Payment_Request_ID = Convert.ToInt32(strRequestID);
            InstrumentdetailsRow.Favouring_Name = Convert.ToString(txtFavouringName.Text);
            if (ddlPayMode.SelectedValue == "1" && Convert.ToString(ViewState["IsCheque_Print"]) != "P")
                InstrumentdetailsRow.Is_Update_Req = 1;
            else
                InstrumentdetailsRow.Is_Update_Req = 0;
            InstrumentdetailsDataTable.AddS3G_LOANAD_InstrumentdetailsRow(InstrumentdetailsRow);


            SerializationMode SMode = SerializationMode.Binary;

            ErrorCode = ObjLoanAdminAccMgtServicesClient.FunPubInsPaymentRequestInstrument(SMode, ClsPubSerialize.Serialize(InstrumentdetailsDataTable, SMode));
            return ErrorCode;

        }
        catch (Exception ex)
        {
            return 1;
        }
        finally
        {

            ObjLoanAdminAccMgtServicesClient.Close();
        }
    }

    //private string FunPriSetCommaSeperator(string DecValue, string strCurrencyCode)
    //{
    //    string Strvalue = "";
    //    string Strrevvalue = "";
    //    string[] strArrValue = new string[100];
    //    if (!string.IsNullOrEmpty(DecValue))
    //    {
    //        switch(strCurrencyCode.ToUpper())
    //        {
    //            case "INR"://Indian Currency
    //                Strvalue=string.Format(System.Globalization.CultureInfo.GetCultureInfo("hi-IN").NumberFormat,
    //                "{0:c}", Convert.ToDecimal(DecValue)).Split(' ')[1].ToString();
    //                break;
    //            default://Us,Uk,Ero etc Currency
    //                Strvalue= string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-us").NumberFormat,
    //                "{0:c}", Convert.ToDecimal(DecValue)).Substring(1).Trim();
    //                break;
    //        }

    //    }
    //     return Strvalue;

    //}

    private void FunPriSetRemarksMandatory(bool BlnFlag)
    {
        RFVRemarks.Enabled = BlnFlag;
        if (BlnFlag)
        {
            lblRemarks.CssClass = "styleReqFieldLabel";
        }
        else
        {
            lblRemarks.CssClass = "styleDisplayLabel";
        }
    }

    private void FunPriPayDtstabDDValidation(bool Blnflag)
    {
        try
        {
            RBLCompanyCashorBankAcct.Enabled =
            ddlbankname.Enabled =
                txtGLCode.Enabled =
                txtSLCode.Enabled =
                //txtInstrumentDate.Enabled =
                txtRemarks.Enabled =
                ddlAcctNumber.Enabled =
                txtInstrumentNumber.Enabled = Blnflag;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriUpdateStatus()
    {
        try
        {
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Voucher_Status", ViewState["IsVoucher_Print"].ToString());
            Procparam.Add("@Cheque_Status", ViewState["IsCheque_Print"].ToString());
            Procparam.Add("@Payment_Request_No", txtPaymentRequestNo.Text);
            Utility.GetDefaultData(SPNames.S3G_LOANAD_UpdatePaymentRequestIsPrint, Procparam);
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, "Error in updating the status");
        }
    }

    private DataTable FunGetCompanyAddress()
    {
        DataTable dtcompanyaddress = new DataTable();
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", userInfo.ProCompanyIdRW.ToString());
            dtcompanyaddress = Utility.GetDefaultData("S3G_Get_Company_Details", Procparam);
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return dtcompanyaddress;
    }

    private void FunPriDecimalCommaSeperator(decimal DecValue)
    {
        //INR,USD,SGD,IQD,EUR,LKR
        string strValue = ""; string stractvalue = "";
        if (DecValue > 0)
        {
            string[] strArrValue = new string[50];
            strValue = DecValue.ToString();
            if (strValue.Contains('.'))
            {
                strValue = strValue.Split('.')[0].ToString();
            }

            char[] chrArrvalue = strValue.ToCharArray(0, strValue.Length);



        }

    }

    private string GetHTMLTextVoucher(string strcopy)
    {
        string strchequevalue = "";

        strchequevalue += "<font size=\"2\"  color=\"Black\" face=\"verdana\">" +
                "<table width=\"94%\" height=\"10%\" border=\"1\"><tr><td>" +
                " <table align=\"center\" width=\"100%\" border=\"0\">" +
                " <tr>" +
                " <td align=\"left\" valign=\"top\">" +
                "<font size=\"5\"  color=\"Blue\" face=\"verdana\">" + ddlbankname.SelectedItem.Text + " </font>" +
                "</td></tr>" +
                " <tr>" +
                " <td align=\"Right\" valign=\"top\"> Date :<u>" + txtInstrumentDate.Text + "</u>" +
                  "</td></tr>" +

                  " <tr>" +
            //" <td align=\"left\" valign=\"top\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;PAY :  <u>" + ucCustomerAddress.CustomerName + "</u>" +
                  " <td align=\"left\" valign=\"top\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;PAY :  <u>" + Convert.ToString(txtFavouringName.Text) + "</u>" +
                  "</td></tr>" +
                  " <tr>" +
                  " <td align=\"left\" valign=\"top\"> _______________________________________________________________________ or BEARER" +
                  "</td></tr>" +
                  " <tr>" +
                  " <td align=\"left\" valign=\"top\">RUPEES :  <u>" + Convert.ToDecimal(lbltotalPaymentAdjust.Text).GetAmountInWords() + "</u>" +
                  "</td></tr>" +
                   " <tr>" +
                  " <td align=\"left\" valign=\"top\">" +
                          " <table align=\"left\" width=\"100%\">" +
                          " <tr>" +
                          " <td width=\"70%\">" +
                            " <table align=\"left\" border=\"0\" width=\"100%\">" +
                              "<tr><td align=\"left\" >_________________________________________</td></tr>" +
                            "</table>" +
                          " </td>" +
                          " <td width=\"30%\" align=\"left\">" +
                          " <table align=\"left\" border=\"0\" width=\"100%\" >" +
                              "<tr>" +
                                  "<td>" +
                                      "<table border=\"1\">" +
                                              " <tr>" +
                                                  " <td>" +
                                                      "Rs." + lbltotalPaymentAdjust.Text +
                                                  "</td>" +
                                                "</tr>" +
                                      "</table>" +
                                  "</td><td></td>" +
                              "</tr>" +
                          "</table>" +
                          "</td></tr>" +
                          "</table>" +
                  "</td></tr>" +
                  " <tr align=\"left\">" +
                      " <td align=\"left\">" +
                          " <table align=\"left\" width=\"100%\" height=\"10px\" border=\"0\">" +
                              "<tr>" +
                                  "<td>" +
                                      "<table align=\"left\" width=\"100%\" border=\"1\">" +
                                           "<tr><td align=\"left\">Acc.No</td><td colspan=\"2\">";
        if (ViewState["AccountNumber"] != null)
            strchequevalue += ViewState["AccountNumber"].ToString();
        strchequevalue += "</td></tr></table>" +

                                "</td>" +
                               "<td></td><td>For " + userInfo.ProCompanyNameRW + "</td>" +
                            " </tr>" +
                        " </table></td>" +
                        "<td>" +
                            "<table><tr><td></td></tr></table>" +
                        "</td>" +
                        "</tr>" +
                        " <tr>" +
" <td align=\"left\" valign=\"top\">" +
"<font size=\"1\"  color=\"Black\" face=\"verdana\">" + ddlbankname.SelectedItem.Text;
        if (ViewState["BankBranch"] != null)
            strchequevalue += " , " + ViewState["BankBranch"].ToString();

        strchequevalue += "</font>" +
        "</td></tr>" +
        " <tr>" +
        " <td align=\"center\" valign=\"top\">" +
        "<table><tr><td></td><td></td><td>Authorised Signature</td></tr></table>" +
        "</td></tr>" +
        " <tr>" +
        " <td  align=\"center\">" +
        "<font size=\"1\"  color=\"Black\" face=\"Courier\">\"" + txtInstrumentNumber.Text + "\"</font>" +
        "</td></tr>" +
        "</table></td></tr></table></font>";

        return strchequevalue;
    }

    private void FunPriGeneratePdfVoucher(string strCopy, string Type)
    {
        try
        {
            String htmlText = GetHTMLTextVoucher(strCopy);
            string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + txtPaymentRequestNo.Text.Replace('/', '_') + "_" + Type + ".pdf");
            string strFileName = "/LoanAdmin/PDF Files/" + txtPaymentRequestNo.Text.Replace("/", "_").Replace(" ", "").Replace(":", "") + "_" + Type + ".pdf";
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));
            doc.AddCreator(ObjUserInfo.ProCompanyNameRW.ToString());
            doc.AddTitle("Voucher_" + strCopy + "_" + txtPaymentRequestNo.Text.Replace('/', '_'));
            doc.Open();
            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null).Cast<IElement>().ToList();
            for (int k = 0; k < htmlarraylist.Count; k++)
            { doc.Add((IElement)htmlarraylist[k]); }
            doc.AddAuthor("S3G Team");
            doc.Close();
            //System.Diagnostics.Process.Start(strnewFile);
            string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (DirectoryNotFoundException dr)
        {
            Utility.FunShowAlertMsg(this, "The Target Directory was not found in the Server to generate the PDF file");
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, " Error in creating a PDF file");
        }
    }

    #region "WEBMETHODS"

    [System.Web.Services.WebMethod]
    public static string[] GetSLCodes(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        DropDownList ddlFooterGL_Code = (DropDownList)obj_Page.grvPaymentDetails.FooterRow.FindControl("ddlFooterGL_Code");
        Procparam.Add("@GLCode", ddlFooterGL_Code.SelectedValue);
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestgridDetails_AGT", Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSLCodesA(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        DropDownList ddlFooterGL_Code = (DropDownList)obj_Page.grvPaymentAdjustment.FooterRow.FindControl("ddlFooterGL_Code");
        Procparam.Add("@GLCode", ddlFooterGL_Code.SelectedValue);
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestgridDetails_AGT", Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPANum(String prefixText, int count)
    {

        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();

        if (Convert.ToInt32(obj_Page.ddlPaymentType.SelectedValue) == 5)
        {
            Procparam.Add("@Option", "6");
            Procparam.Add("@PrefixText", Convert.ToString(prefixText));
            Procparam.Add("@Funder_ID", Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]));
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ddlReceiptFrom.SelectedValue));
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));
        }
        else if (Convert.ToInt32(obj_Page.ddlPaymentType.SelectedValue) == 8)
        {
            Procparam.Add("@Option", "9");
            Procparam.Add("@PrefixText", Convert.ToString(prefixText));
            Procparam.Add("@Funder_ID", Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]));
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));
        }
        else
        {
            Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
            // --code commented and added by saran in 01-Aug-2014 Insurance start 

            //if (obj_Page.ddlLOB.SelectedIndex > 0)
            // --code commented and added by saran in 01-Aug-2014 Insurance end 

            Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
            if (obj_Page.ddlBranch.SelectedValue != "0")
            {
                Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
            }

            Procparam.Add("@Option", "1");
            Procparam.Add("@PrefixText", prefixText);

            if (obj_Page.ddlPayTo.SelectedValue == "11")
            {
                if (obj_Page.ViewState["hdnCustorEntityID"] != null && obj_Page.ViewState["hdnCustorEntityID"].ToString() != string.Empty)
                {

                    Procparam.Add("@Ins_Company_Id", obj_Page.ViewState["hdnCustorEntityID"].ToString());
                }
                suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestInsuranceDetails", Procparam));
            }
            else
            {
                if (Convert.ToInt32(obj_Page.ddlPayTo.SelectedValue) > 0)
                {
                    if (obj_Page.ddlPayTo.SelectedValue == "1")
                        Procparam.Add("@Payto", "144");
                    else if (obj_Page.ddlPayTo.SelectedValue == "50")
                        Procparam.Add("@OptionPay_To", "1");
                    else if (obj_Page.ddlPayTo.SelectedValue == "13")
                        Procparam.Add("@OptionPay_To", "150");
                    else
                        Procparam.Add("@Payto", "145");
                }

                if (obj_Page.ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ol")
                {
                    Procparam.Add("@LOB_Code", "OL");
                }

                if (obj_Page.ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ft")
                {
                    Procparam.Add("@LOB_Code", "FT");
                    if (obj_Page.ddlLOB.SelectedItem.Text.Contains("FT") && (obj_Page.ddlPayTo.SelectedValue == "1" || obj_Page.ddlPayTo.SelectedValue == "12"))
                    {
                        if (obj_Page.ViewState["hdnCustorEntityID"] != null && obj_Page.ViewState["hdnCustorEntityID"].ToString() != string.Empty)
                        {

                            Procparam.Add("@Customer", obj_Page.ViewState["hdnCustorEntityID"].ToString());
                            suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestFTAcc", Procparam));
                        }
                    }
                }
                else
                {
                    if (obj_Page.ViewState["hdnCustorEntityID"] != null && obj_Page.ViewState["hdnCustorEntityID"].ToString() != string.Empty)
                    {
                        Procparam.Add("@EntityId", obj_Page.ViewState["hdnCustorEntityID"].ToString());
                    }
                    suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam));
                }
            }
        }
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPANumA(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        // --code commented and added by saran in 01-Aug-2014 Insurance start 

        //if (obj_Page.ddlLOB.SelectedIndex > 0)
        // --code commented and added by saran in 01-Aug-2014 Insurance end 
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        if (obj_Page.ddlBranch.SelectedValue != "0")
        {
            //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
        }

        Procparam.Add("@Option", "1");
        Procparam.Add("@PrefixText", prefixText);

        if (obj_Page.ddlPayTo.SelectedValue == "11")
        {

            if (obj_Page.ViewState["hdnCustorEntityID"] != null && obj_Page.ViewState["hdnCustorEntityID"].ToString() != string.Empty)
            {

                Procparam.Add("@Ins_Company_Id", obj_Page.ViewState["hdnCustorEntityID"].ToString());
            }
            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)obj_Page.grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            //ddlFooterPrimeAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestInsuranceDetails", Procparam, new string[] { "PANum", "PANum" });
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestInsuranceDetails", Procparam));


        }
        else
        {

            if (obj_Page.ddlPayTo.SelectedIndex > 0)
            {
                if (obj_Page.ddlPayTo.SelectedValue == "1")
                    Procparam.Add("@Payto", "144");
                else if (obj_Page.ddlPayTo.SelectedValue == "50")
                    Procparam.Add("@OptionPay_To", "1");
                else
                    Procparam.Add("@Payto", "145");

            }

            if (obj_Page.ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ft")
            {
                Procparam.Add("@LOB_Code", "FT");
            }
            if (obj_Page.ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].ToString().Trim() == "ol")
            {
                Procparam.Add("@LOB_Code", "OL");
            }

            if (obj_Page.ViewState["hdnCustorEntityID"] != null && obj_Page.ViewState["hdnCustorEntityID"].ToString() != string.Empty)
            {

                Procparam.Add("@EntityId", obj_Page.ViewState["hdnCustorEntityID"].ToString());
            }


            //DropDownList ddlFooterPrimeAccountNumber = (DropDownList)obj_Page.grvPaymentDetails.FooterRow.FindControl("ddlFooterPrimeAccountNumber");
            //ddlFooterPrimeAccountNumber.BindDataTable("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam, new string[] { "PANum", "PANum" });
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestgridDetails", Procparam));

            /*     //s3g to sfl - kuppu - july 12 -- starts
                 DropDownList ddlFooterFlowType = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterFlowType");
                 UserControls_S3GAutoSuggest ddlFooterSL_Code = (UserControls_S3GAutoSuggest)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
                 //DropDownList ddlFooterSL_Code = (DropDownList)grvPaymentDetails.FooterRow.FindControl("ddlFooterSL_Code");
                 TextBox txtFooterDescription = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterDescription");
                 TextBox txtFooterAmount = (TextBox)grvPaymentDetails.FooterRow.FindControl("txtFooterAmount");

                 ddlFooterFlowType.SelectedIndex = 0;
                 //ddlFooterSL_Code.SelectedIndex = 0;
                 ddlFooterSL_Code.Clear();
                 txtFooterDescription.Text = string.Empty;
                 txtFooterAmount.Text = string.Empty;

                 // --ends--*/


        }
        return suggestions.ToArray();
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
        Procparam.Add("@Program_Id", obj_Page.ProgramCode);
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetReceiptList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        if (Convert.ToInt32(obj_Page.ddlPaymentType.SelectedValue) == 1 || Convert.ToInt32(obj_Page.ddlPaymentType.SelectedValue) == 3)
        {
            Procparam.Add("@Option", "2");          //Lessee
            Procparam.Add("@Vendor_ID", Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]));
        }
        else if (Convert.ToInt32(obj_Page.ddlPaymentType.SelectedValue) == 4)
        {
            Procparam.Add("@Option", "3");          //Funder
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]));
        }
        else if (Convert.ToInt32(obj_Page.ddlPaymentType.SelectedValue) == 5)
        {
            Procparam.Add("@Option", "4");          //Lessee
            Procparam.Add("@Funder_ID", Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]));
        }
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Option", "5");          //Tranche List
        Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ddlReceiptFrom.SelectedValue));
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));

        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetGLCodeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.CompanyId.ToString());
        Procparam.Add("@Lob_ID", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@OPTION", "15");
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSLCodeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Funder_ID", (obj_Page.ViewState["hdnCustorEntityID"] != null) ? Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]) : "0");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Option", "16");
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetNoteList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@OPTION", "13");
        Procparam.Add("@Funder_ID", (obj_Page.ViewState["hdnCustorEntityID"] != null) ? Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]) : "0");
        Procparam.Add("@Lob_Id", Convert.ToString(obj_Page.ddlLOB.SelectedValue));

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetNoteTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@OPTION", "14");
        Procparam.Add("@Funder_ID", (obj_Page.ViewState["hdnCustorEntityID"] != null) ? Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]) : "0");
        Procparam.Add("@Lob_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));

        if (obj_Page.grvFunderReceipt.FooterRow != null)
        {
            UserControls_S3GAutoSuggest ddlNoteNo = (UserControls_S3GAutoSuggest)obj_Page.grvFunderReceipt.FooterRow.FindControl("ddlNoteNo");
            Procparam.Add("@Note_ID", Convert.ToString(ddlNoteNo.SelectedValue));
        }

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetInvSrchLst(String prefixText, int count)
    {
        if (Convert.ToInt32(obj_Page.ddlInvoiceSortBy.SelectedValue) > 0)
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();

            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
            Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
            Procparam.Add("@PrefixText", prefixText);
            Procparam.Add("@SearchType", Convert.ToString(obj_Page.ddlInvoiceSortBy.SelectedValue));
            if (Convert.ToInt32(obj_Page.ddlPaymentType.SelectedValue) == 1 || Convert.ToInt32(obj_Page.ddlPaymentType.SelectedValue) == 3)
            {
                Procparam.Add("@OPTION", "20");
                Procparam.Add("@Vendor_ID", (obj_Page.ViewState["hdnCustorEntityID"] != null) ? Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]) : "0");
            }
            else
            {
                Procparam.Add("@OPTION", "21");
                Procparam.Add("@Customer_ID", (obj_Page.ViewState["hdnCustorEntityID"] != null) ? Convert.ToString(obj_Page.ViewState["hdnCustorEntityID"]) : "0");
            }

            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam));

            return suggetions.ToArray();
        }
        else
        {
            return null;
        }
    }


    #endregion

    /*
        protected void btncrtVoucher_Click(object sender, EventArgs e)
        {
            try
            {
                ObjLoanAdminAccMgtServicesClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
                byte[] bytePaymentVoucher = ObjLoanAdminAccMgtServicesClient.FunPubPaymentvoucher(intCompanyID, strRequestID);
                List<ClsPubPaymentvoucher> PaymentVoucherDetails = (List<ClsPubPaymentvoucher>)DeSeriliaze(bytePaymentVoucher);
                Session["PaymentVoucherDetails"] = PaymentVoucherDetails;
            
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
            }
        }
        */

    //Added by Sathiyanathan on 30-Dec-2013
    //Delete Records in Adjustment grid based on PANUM
    protected void FunProDeleteAdjsDtls(string strPanum)
    {
        try
        {
            if (ViewState["grvPaymentAdjust"] != null)
            {
                DataTable dtgrvPaymentAdjust = (DataTable)ViewState["grvPaymentAdjust"];
                DataRow[] drpaymtadj = dtgrvPaymentAdjust.Select("PANUM='" + strPanum + "'");
                if (drpaymtadj.Length > 0)
                {
                    foreach (DataRow dr in drpaymtadj)
                    {
                        dr.Delete();
                    }
                }
                ViewState["grvPaymentAdjust"] = dtgrvPaymentAdjust;
                grvPaymentAdjustment.DataSource = dtgrvPaymentAdjust;
                grvPaymentAdjustment.DataBind();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    //END Here

        protected void btnremoveall_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            /*To bind Grid "No Records Found- Call ID - 3514 - Added by Vinodha M on March 23,2016*/
            ViewState["TotRec"] = null;
            FunPriBindGridDtls(2, null);
            Pagenavigator1.Visible = true;
            //grvPaymentInvoiceDtl.DataSource = null;
            //grvPaymentInvoiceDtl.DataBind();
            //Pagenavigator1.Visible = false;
            /*To bind Grid "No Records Found- Call ID - 3514 - Added by Vinodha M on March 23,2016*/
            btnremoveall.Visible = false;
            Procparam.Add("@Option", "6");     //1 - Insert  2 - Delete
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));

            DataTable dtremove = Utility.GetDefaultData("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);

            if (dtremove.Rows[0]["@Option"].ToString() == "100")
            {
                ((TextBox)grvPaymentDetails.Rows[0].FindControl("lblAmount")).Text = "0";
                lblPaymentDetailsTotal.Text = lbltotalPaymentAdjust.Text = "0";
                txtDocAmount.Text = Math.Round(Convert.ToDecimal(lbltotalPaymentAdjust.Text), 0.00).ToString();
                Utility.FunShowAlertMsg(this, "Invoices have been removed successfully");
                return;
            }




        }



    private void FunPriBindGrid1()
    {
        try
        {
            lblPagingErrorMessage.InnerText = "";
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            FunPriGetSearchValue();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Payment_Type", Convert.ToString(ddlPaymentType.SelectedValue));
            Procparam.Add("@FilterType", Convert.ToString(ddlInvoiceSortBy.SelectedValue));
            if (Convert.ToString(ddlInvSrchTxt.SelectedText) != "")
                Procparam.Add("@FilterText", Convert.ToString(ddlInvSrchTxt.SelectedValue));
            if (Convert.ToString(txtInvoiceStartDate.Text) != "" && Convert.ToString(txtInvoiceEndDate.Text) != "")
            {
                Procparam.Add("@From_Date", Convert.ToString(Utility.StringToDate(txtInvoiceStartDate.Text)));
                Procparam.Add("@To_Date", Convert.ToString(Utility.StringToDate(txtInvoiceEndDate.Text)));
            }
            if (ViewState["grvPaymentDetails"] != null)
            {
                DataTable dtInvoice = (DataTable)ViewState["grvPaymentDetails"];
                if (dtInvoice.Rows.Count == 0 || Convert.ToString(dtInvoice.Rows[0]["Panum"]) == "-1")
                {
                    Procparam.Add("@Option1", "1");
                }
            }
            if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 3)
            {
                Procparam.Add("@Option", "1");
                Procparam.Add("@Customer_ID", Convert.ToString(ddlReceiptFrom.SelectedValue));
                Procparam.Add("@Vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
            }
            else if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 1)
            {
                Procparam.Add("@Option", "2");
                Procparam.Add("@Vendor_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                if (Convert.ToString(ddlReceiptFrom.SelectedValue) != "" && Convert.ToInt32(ddlReceiptFrom.SelectedValue) > 0)
                    Procparam.Add("@Customer_ID", Convert.ToString(ddlReceiptFrom.SelectedValue));
            }
            else if (Convert.ToInt32(ddlPaymentType.SelectedValue) == 4)
            {
                Procparam.Add("@Customer_ID", Convert.ToString(ViewState["hdnCustorEntityID"]));
                Procparam.Add("@Option", "3");
            }
            if (((CheckBox)grvLoadInvoiceDtl.HeaderRow.FindControl("chkSelectAllInvoices")).Checked)
            {
                Procparam.Add("@selectall", 9.ToString());
            }

            grvLoadInvoiceDtl.BindGridView("S3G_Loanad_GetPaymentInvoiceDtl", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            DataTable dtPOInvoiceDtls = ((DataView)grvLoadInvoiceDtl.DataSource).ToTable();
            ViewState["POInvoiceDetails"] = dtPOInvoiceDtls;
            btnAddPOInvoice.Enabled = true;
            //FunPriEnblDsblINVGridClmn();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvLoadInvoiceDtl.Rows[0].Visible = btnAddPOInvoice.Enabled = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

            Procparam.Clear();
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Is_Added", "0");    //Added on 15Jun2015 for OPC Req
            DataTable dtSmry = Utility.GetDefaultData("S3G_Loanad_InsertTmpPaymentInvDtl", Procparam);
            if (dtSmry != null && dtSmry.Rows.Count > 0)
                FunPriDisplayGridTotal(dtSmry);

            DataRow[] drCheck = dtPOInvoiceDtls.Select("Chk_Selected = 0");
            CheckBox chkSelectAllInvoices = (CheckBox)grvLoadInvoiceDtl.HeaderRow.FindControl("chkSelectAllInvoices");
            chkSelectAllInvoices.Checked = (drCheck.Length > 0) ? false : true;
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
}
