

#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Loan Admin
/// Screen Name         :   Transaction Lander
/// Created By          :   S.Kannan
/// Created Date        :   08-Aug-2010
/// Purpose             :   This is the landing screen for all the other LoanAdmin Screens
/// Last Updated By		:   Chandra Sekhar BS
/// Last Updated Date   :   17-Sep-2013
/// Reason              :   Auto Suggest for Account Creation
/// <Program Summary> 
#endregion

#region How to use this
/*
   1)	Search for the word "Add Here" and add your code respectively  there...
   2)   Use the same stored procedure S3G_LOANAD_TransLander, the return table should have the first column named "ID" - to use it as the RowCommandValue
            SP return table Rule
                1)  First Column should be "ID" - which will be used as a row command
                2)  Second Column should be "Created_By" - which will be the created_By column
                3)  third should be the "User_Level_ID" - which will be the createdBy user's level id.            
            The second and third should was used to check the user authorization.
            Take latest code from - App_Code\Utility.cs
   3)  Add your page Program ID as a parameter (@DocumentNumber)
   4)  If you want to send any other special parameters to your SP, then you can send it – through Dictionary – ProcParam. 
   5)  Also add it – to the Common SP – Commented with your program ID
   6)  The Query String can accept 6 Parameters, 
        a) Create – (Optional)
        b) Query – (Optional)
        c) Modify – (Optional)
        d) MultipleDNC - (Optional)  - If the user wants to select the Document Number Control dynamically.
        e) DNCOption - (Optional) - If the Enduser have more than one option for the selected DNC - eg: approved,unapproved - etc.
        f) Code – (Mandatory)
            ex: 
                S3GORGTransLander.aspx?Code=FEIR	
                S3GORGTransLander.aspx?Code=SNQ&Create=0&Query=1&Modify=0
                S3GORGTransLander.aspx?Code=SNQ&Query=0
                S3GORGTransLander.aspx?Code=SNQ& Modify=0
                S3GORGTransLander.aspx?Code=CRPT&MultipleDNC=1&DNCOption=1
   7) If you use the Query string "MultipleDNC" then you want to pass the parameter "@MultipleDNC_ID" to "[S3G_ORG_Get_TransLander]" SP
   8) If you use the Query string "DNCOption" then you want to pass the parameter "@MultipleOption_ID" to "[S3G_ORG_Get_TransLander]" SP 

  */
#endregion

# region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Globalization;
using System.Collections;
using System.Resources;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Web.Security;
using S3GBusEntity.Origination;
using Resources;
#endregion

#region Class LoanAdmin_S3GORGTransLander
public partial class LoanAdmin_S3gLoanAdTransLander : ApplyThemeForProject
{
    # region Programs Code
    string ProgramCodeToCompare = "";                                           // this is to hold the Program Code of your web page
    public string strQueryString = "";
    // Add here - Add your Program Code Here - refer to the SQL table Program Master.
    const string strPaymentRequest = "PARE";                        // Program Code for Payment Request
    const string strPaymentApproval = "PAAP";                        // Program Code for Payment Request
    const string strLeaseAssetSaleApproval = "LAA";                        // Program Code for Payment Request
    const string strAccountclosureApproval = "AAP";                        // Program Code for Payment Request
    const string strTopupApproval = "TUA";                        // Program Code for Payment Request
    const string strSpecificRevisionApproval = "SRA";                        // Program Code for Payment Request
    const string strManualJournalApproval = "MJA";                        // Program Code for Payment Request
    const string strPrematureClosureApproval = "PCA";                        // Program Code for Payment Request
    const string strConsolidationApproval = "CONA";                        // Program Code for Payment Request
    const string strSplitApproval = "ASPA";                        // Program Code for Payment Request
    const string strAssetVerification = "ASV";   // Program Code for Asset Verification
    const string strAccountActivation = "ACAC";   // Program Code for Account Activation
    const string strAccountBulkActivation = "ACBAC";   // Program Code for Account Activation
    const string strDeliveryIns = "DEI";   // Program Code for Delivery Instruction
    const string strFactoringInvoice = "FIL";   // Program Code for Factoring Invoice Loading
    const string strFactoringInvoiceRetirement = "FLR";  // Program Code for Factoring Invoice Retirement
    const string strAssetIdentification = "ASI"; // Asset identification Program Code
    const string strInvoiceVendor = "IVE";   // Program Code for Invoice Vendor Loading

    const string strAssetAcquisition = "ASA";                   // Program Code for Asset AssetAcquisition
    const string strMonthClosure = "LADMoCL";                      // Program Code for Month Closure
    const string strBilling = "CLNBILL";
    const string strRBilling = "RBILL";

    const string strLeaseAssetSale = "LAS";
    const string strGlobalParmSetup = "GPS";   // Program Code for Global Parameter Setup
    const string strOverDueInterest = "COI"; // Program Code for Over Due Interest

    //Code added and commented by saran on 26-Jul-2013 for BW start
    const string strOverDueInterestBW = "BWODI"; // Program Code for Over Due Interest BW
    //Code added and commented by saran on 26-Jul-2013 for BW end
    const string strOperaingDepreciation = "OLD"; // Program code for Operating Lease Depreciation
    const string strBulkRevision = "BURE"; // Program code for Bulk revision
    const string strOperatingLeaseExpenses = "OLE";   // Program Code for Operating Lease Expenses
    const string strNocTermination = "NOCT";
    const string strAccountConsolidation = "ACON";   // Program Code for Account Consoldation
    const string strAccountSplit = "ACSP";   // Program Code for Account Split
    const string strCashflowMntlyBking = "CMB";   // Program Code for Cashflow Monthly Booking
    const string strAccountClosure = "ACCO"; // Program Code for Account Closure
    const string strMJV = "MAJ"; // Program Code for Account Closure
    const string strSysJournal = "SYS"; // Program Code for Account Closure
    const string strSpecificRevision = "ASR"; // account specific revision.
    const string strTLEWC = "TUP"; // account TLEWC.
    const string strTLGL = "TLGL"; // account TLEWC.
    const string strIncomeRecognition = "CIR";   // PDC Module
    const string strAccountCreation = "ACR";//Account Creation
    const string strAccountCreationBW = "BWACR";//Account Creation
    const string strPreMatureClosure = "PRC"; // Program code for Premature Closure
    const string strPDTransaction = "PDT"; // Program code for Premature Closure
    const string strCompensation = "LCC"; // Program code for Compensation Calculation
    const string strLoanEndUseApproval = "LEUA";
    const string strCRDRNote = "DCN"; // Program Code for Credit Debit Note 
    const string strCRDRNoteApproval = "DCAP"; // Program Code for Debit Credit Note Approval  
    const string strInterim = "INTB"; // Program Code for Interim Billing  
    // added by Ramesh -For approval
    static bool IsApprovalNeed;
    string ProgramCode = "";
    // Program Code for Enquiry Customer Appraisal

    static string userUTPA;
    #endregion
    //Add by chandru
    static bool strRbtnJournal = false;
    //Add by chandru
    #region Common Variables
    int intCreate = 0;                                                         // intCreate = 1 then display the create button, else invisible
    int intQuery = 0;                                                          // intQuery = 1 then display the Query button, else invisible
    int intModify = 0;                                                         // intModify = 1 then display the Modify button, else invisible
    int intMultipleDNC = 0;                                                    // Allow the user to select the DNC dynamically.
    int intDNCOption = 0;                                                      // Allow the user to select the further option depend on the DNC - eg: approved,unapproved etc...
    Dictionary<int, string> dictMultipleDNC = null;                             // collection for Multiple DNC - DDL
    Dictionary<int, string> dictDNCOption = null;                               // Collection for DNCOption.
    string strProcName = "S3G_LOANAD_TransLander";                             // this is the Stored procedure to get call                     
    // To maintain the ProgramID
    UserInfo ObjUserInfo;                                                       // to maintain the user information      
    public string strDateFormat;                                                // to maintain the standard format
    string strRedirectPage = "";                                                // page to redirect to the page in query mode
    string strRedirectCreatePage = "";                                          // page to redirect to the page in Create mode
    Dictionary<string, string> Procparam = null;                                // Dictionary to send our procedure's Parameters
    int intUserID = 0;                                                          // user who signed in
    int intCompanyID = 0;                                                       // conpany of the user who signed in   
    int intUserLevelID = 0; 
    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    bool isQueryColumnVisible;                                                  // To change the Query Column visibility - depend on the user autherization 
    bool isEditColumnVisible;                                                  // To change the Edit Column visibility - depend on the user autherization 
    string[] strLOBCodeToFilter;
    public string strProgramId = "";
    public static LoanAdmin_S3gLoanAdTransLander obj_Page;
    // give the selective lob code 

    #region  User Authorization
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    #endregion


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

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        #region Application Standard Date Format
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;                              // to get the standard date format of the Application
        CalendarExtenderEndDateSearch.Format = strDateFormat;                       // assigning the first textbox with the End date
        CalendarExtenderStartDateSearch.Format = strDateFormat;                     // assigning the first textbox with the start date
        #endregion
        #region Common Session Values
        //Add by chandru on 23/04/2012
        if (ComboBoxLOBSearch.SelectedValue != "")
        {
            System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = ComboBoxLOBSearch.SelectedValue;
            System.Web.HttpContext.Current.Session["LOBAutoSuggestText"] = ComboBoxLOBSearch.SelectedItem.Text;
        }
        else
        {
            System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = null;
        }

        //if (ComboBoxBranchSearch.SelectedValue != "")
        //    System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"] = ComboBoxBranchSearch.SelectedValue;
        //else
        //    System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"] = null;
        if (hdnBranchID.Value != string.Empty)
            System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"] = hdnBranchID.Value;
        else
            System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"] = null;

        if (txtStartDateSearch.Text != string.Empty)
            System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"] = Utility.StringToDate(txtStartDateSearch.Text).ToString();
        else
            System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"] = null;

        if (txtEndDateSearch.Text != string.Empty)
            System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"] = Utility.StringToDate(txtEndDateSearch.Text).ToString();
        else
            System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"] = null;

        //if (ddlLessee.SelectedText != string.Empty)
        //    System.Web.HttpContext.Current.Session["CustomerAutoSuggestValue"] = ddlLessee.SelectedValue;
        //else
        //    System.Web.HttpContext.Current.Session["CustomerAutoSuggestValue"] = null;
        if (rdoSystem.Checked)
            strRbtnJournal = true;
        else
            strRbtnJournal = false;
        //Add by chandru on 23/04/2012

        #endregion

        #region Grid Paging Config
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
        #endregion

        # region User Information
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;                                  // current user's company ID.
        intUserID = ObjUserInfo.ProUserIdRW;                                        // current user's ID
        intUserLevelID = ObjUserInfo.ProUserLevelIdRW; 

        //Add by chandru on 23/04/2012
        System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyID.ToString();
        System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = intUserID.ToString();
        //Add by chandru on 23/04/2012

        #region  User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        #endregion
        #endregion

        #region Initialize page
        bool IsQueryStringChanged = false;
        if (!(string.IsNullOrEmpty(Request.QueryString["Code"])))                   // reading the query string
        {
            // to do  : want to decrypt this code in the URL
            FunPriGetQueryStrings();
            FunPriTransactionActionButtons();
            InitPage();
            System.Web.HttpContext.Current.Session["ProgramId"] = strProgramId;
            System.Web.HttpContext.Current.Session["ProgramCode"] = ProgramCode;
            FunPriUIVisibility();

            //Code added for Asset Verification - UTPA Fucntionality - Kuppu - Aug-20-2012
            //Starts
            switch (ProgramCode)
            {
                case strAssetVerification:
                    if (ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "UTPA")
                    {
                        Session["UTPA"] = ObjUserInfo.ProUserTypeRW.ToString().ToUpper();
                        userUTPA = Session["UTPA"].ToString();
                        btnCreate.Enabled = false;
                    }
                    if (!bModify)
                    {
                        grvTransLander.Columns[1].Visible = false;
                    }
                    if (!bQuery)
                    {
                        grvTransLander.Columns[0].Visible = false;
                    }
                    break;
            }
            //Ends

            if (ViewState["ProgramCode"] == null)                                    // Added viewstate for the program code - to refresh the page - when the query string of the URL varys. - It will assign the program code to the view state - for the very first time the page loads.
                ViewState["ProgramCode"] = ProgramCode;
            else                                                                    // If the program code in the URL changes then - it mean the user clicked on some other menu - so it need to refresh the page accordingly.
            {
                if (string.Compare(ViewState["ProgramCode"].ToString(), ProgramCode) != 0)
                {
                    IsQueryStringChanged = true;                                    // If the page changed from the current page.
                }
            }


        }
        #endregion

        #region !IsPostBack or QueryString changed.
        if ((!IsPostBack) || (IsQueryStringChanged))                                // refresh the page even if the query string of the URL varys - it mean the user navigates to some other page.
        {
            IsApprovalNeed = false;
            RFVComboLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
            RFVComboBranch.ErrorMessage = ValidationMsgs.S3G_ValMsg_Branch;

            ViewState["ProgramCode"] = ProgramCode;
            IsQueryStringChanged = false;
            //txtEndDateSearch.Attributes.Add("readonly", "readonly");                // making the end date textbox readonly
            //txtStartDateSearch.Attributes.Add("readonly", "readonly");              // making the start date textbox readonly
            if (ProgramCode == "CLNBILL")
            {
                txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',false,  false);");
                txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',false,  false);");


            }
            else
            {
                txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
                txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");

                CalendarExtenderStartDateSearch.OnClientDateSelectionChanged = "checkDate_NextSystemDate";
                CalendarExtenderEndDateSearch.OnClientDateSelectionChanged = "checkDate_NextSystemDate";
            }
            FunProLoadCombos();                                                     // loading the combos - LOB and Branch
            grvTransLander.Visible =
            ucCustomPaging.Visible = false;
            //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);           // setting the Page Title
            if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.Items.Count > 0)    // to set to the default position
                ComboBoxLOBSearch.SelectedIndex = 0;
            //if (ComboBoxBranchSearch != null && ComboBoxBranchSearch.Items.Count > 0)    // to set to the default position
            //    ComboBoxBranchSearch.SelectedIndex = 0;
            txtBranchSearch.Text = string.Empty;
            hdnBranchID.Value = string.Empty;
            //Added by Chandru
            txtDocumentNumberSearch.Text = string.Empty;
            hdnCommonID.Value = string.Empty;
            //Added by Chandru

            //Added by Siva.K
            txtOldInvoice.Text = hdnOldInvoice.Value = txtInvoice.Text = hdnInvoice.Value = txtDCNInvoice.Text = hdnDCNInvoice.Value = txtODIInvoice.Text = 
                hdnODIInvoice.Value = txtLessee.Text = hdnLessee.Value = txtTrancheName.Text = hdnTranche.Value = string.Empty;
            ddlType.SelectedValue = "0";
            //Added by Chandru
            
            #region  User Authorization
            if (btnCreate.Enabled)                                                  // if the user can view the create button - depends on the query string
            {

                //User Authorization
                if (!bIsActive)
                {
                    grvTransLander.Columns[1].Visible = false;
                    grvTransLander.Columns[0].Visible = false;
                    btnCreate.Enabled = false;
                    return;
                }
                if (!bModify)
                {
                    grvTransLander.Columns[1].Visible = false;
                    intModify = 0;
                }
                if (!bQuery)
                {
                    grvTransLander.Columns[0].Visible = false;
                    intQuery = 0;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                    intCreate = 0;
                }
                //Authorization Code end

                switch (ProgramCode)
                {
                    case strLeaseAssetSaleApproval:
                    case strConsolidationApproval:
                    case strSplitApproval:
                    case strAccountclosureApproval:
                    case strSpecificRevisionApproval:
                    case strManualJournalApproval:
                    case strPaymentApproval:
                    case strTopupApproval:
                    case strCRDRNoteApproval:
                        btnCreate.Text = btnCreate.ToolTip = "Approve";
                        break;
                    case strPrematureClosureApproval:
                        btnCreate.Text = btnCreate.ToolTip = "Approve";
                        break;
                    case strAccountActivation:
                        btnCreate.Visible = false;
                        btnCreate.Text = btnCreate.ToolTip = "Activate";
                        break;
                    case strAccountBulkActivation:
                        btnCreate.Text = btnCreate.ToolTip = "Activate";
                        break;
                    default:
                        btnCreate.Text = btnCreate.ToolTip = "Create";
                        break;

                }
            }

            #endregion
            txtEndDateSearch.Text = txtStartDateSearch.Text = "";


        }
        #endregion
        ViewState["EnquiryorCustomer"] = string.Empty;

        //Added by Thangam M on 28-Sep-2013 for Row lock
        if (Session["RemoveLock"] != null)
        {
            Utility.FunPriRemoveLockedRow(intUserID, "0", "0");
            Session.Remove("RemoveLock");
        }
        //End here

    }
    #endregion

    #region UserDefined Events
    /// <summary>
    /// To change the visibility - according to the Query String
    /// </summary>
    private void FunPriUIVisibility()
    {
        btnCreate.Enabled = (intCreate == 0) ? false : true;
    }

    /// <summary>
    /// To Enable/Disable your transaction page Action Buttons
    /// </summary>
    private void FunPriTransactionActionButtons()
    {

        // Add here
        switch (ProgramCode)
        {
            case strCRDRNoteApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;

            case strCRDRNote:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strPaymentRequest:
                FunPriEnableActionButtons(true, true, true, false, true, false);
                break;
            case strRBilling:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strInterim:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strOverDueInterest: //Source modified by Tamilselvan.S on 12/01/2011
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            //Code added and commented by saran on 26-Jul-2013 for BW start
            case strOverDueInterestBW:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            //Code added and commented by saran on 26-Jul-2013 for BW start

            case strLeaseAssetSaleApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strPaymentApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strAccountclosureApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strTopupApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strSpecificRevisionApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strManualJournalApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strPrematureClosureApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strConsolidationApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strSplitApproval:
                FunPriEnableActionButtons(true, true, true, false, false, true);
                break;
            case strAssetVerification:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strAccountActivation:
                if (ObjUserInfo.ProUserLevelIdRW >= 3)
                    FunPriEnableActionButtons(true, true, true, false, false, false);
                else
                    FunPriEnableActionButtons(true, true, false, false, false, false);
                break;
            case strAccountBulkActivation:
                if (ObjUserInfo.ProUserLevelIdRW >= 3)
                    FunPriEnableActionButtons(true, true, true, false, false, false);
                else
                    FunPriEnableActionButtons(true, true, false, false, false, false);
                break;

            case strDeliveryIns:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strFactoringInvoice:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;

            case strFactoringInvoiceRetirement:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strInvoiceVendor:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strAssetIdentification:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;


            case strAssetAcquisition:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strMonthClosure:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;


            case strLeaseAssetSale:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strGlobalParmSetup:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strOperaingDepreciation:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strBulkRevision:
                FunPriEnableActionButtons(true, true, false, false, false, false);
                break;
            case strOperatingLeaseExpenses:
                FunPriEnableActionButtons(true, true, false, false, false, false);
                break;
            case strNocTermination:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strAccountConsolidation:
                if (ObjUserInfo.ProUserLevelIdRW >= 3)
                    FunPriEnableActionButtons(true, true, true, false, false, false);
                else
                    FunPriEnableActionButtons(false, true, false, false, false, false);
                break;
            case strAccountSplit:
                if (ObjUserInfo.ProUserLevelIdRW >= 3)
                    FunPriEnableActionButtons(true, true, true, false, false, false);
                else if (ObjUserInfo.ProUserLevelIdRW == 2)
                    FunPriEnableActionButtons(true, true, true, false, false, false);
                else
                    FunPriEnableActionButtons(false, true, false, false, false, false);
                break;
            case strCashflowMntlyBking:
                FunPriEnableActionButtons(true, true, false, false, false, false);
                break;
            case strAccountClosure:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strMJV:
                if (rdoManual.Checked)
                {
                    FunPriEnableActionButtons(true, true, true, false, false, false);
                }
                else
                {
                    FunPriEnableActionButtons(false, true, false, false, false, false);
                }
                break;
            case strSpecificRevision:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strTLEWC:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strTLGL:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strAccountCreation:
            case strAccountCreationBW:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strBilling:
                FunPriEnableActionButtons(true, true, false, false, false, false);
                break;
            case strPreMatureClosure:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strIncomeRecognition:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case (strPDTransaction):
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case (strCompensation):
                FunPriEnableActionButtons(true, true, false, false, false, false);
                break;
            case strLoanEndUseApproval:
                FunPriEnableActionButtons(true, true, false, false, false, false);
                break;
            default:
                FunPriEnableActionButtons(false, false, false, false, false, false);
                break;



        }
    }

    /// <summary>
    /// This method will decide to make the Action Buttons Enable/Disable - Depending to your transaction Page.
    /// </summary>
    /// <param name="blnCreate">true to enable create button</param>
    /// <param name="blnQuery">true to enable Query Mode</param>
    /// <param name="blnModify">true to enable Modify Mode</param>
    /// <param name="blnMultipleDNC">true to enable Multiple DNC</param>
    /// <param name="blnDNCOption">true to maintain the DNC Option</param>
    private void FunPriEnableActionButtons(bool blnCreate, bool blnQuery, bool blnModify, bool blnMultipleDNC, bool blnDNCOption, bool isApprovalNeed)
    {

        intCreate = (bCreate) ? Convert.ToInt32(blnCreate) : 0; // checking user access
        intModify = (bModify) ? Convert.ToInt32(blnModify) : 0; // checking user access
        intQuery = (bQuery) ? Convert.ToInt32(blnQuery) : 0; // checking user access

        intMultipleDNC = Convert.ToInt32(blnMultipleDNC);
        intDNCOption = Convert.ToInt32(blnDNCOption);
        IsApprovalNeed = isApprovalNeed;
    }



    /// <summary>
    /// To read the values from the querystring
    /// </summary>
    private void FunPriGetQueryStrings()
    {
        //Modified by chandru on 23/04/2012
        /*if (Request.QueryString.Get("Code") != null)
            ProgramCode = (Request.QueryString.Get("Code"));*/
        if (Request.QueryString.Get("Code") != null)
        {
            ProgramCode = (Request.QueryString.Get("Code"));
            System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"] = ProgramCode;
        }
        //Modified by chandru on 23/04/2012
    }


    /// <summary>
    /// This is an optional dropdown box - if the user want to 
    /// display multiple DNC - then he can make use of this method.
    /// </summary>
    private void FunPriLoadMultiDNCCombo()
    {
        if (intMultipleDNC == 1)
        {
            FunPriMakeMultipleDNCVisible(lblMultipleDNC, ddlMultipleDNC, true);
            // lblAutosuggestProgramIDSearch.Visible = cmbDocumentNumberSearch.Visible = false;

            // Add here case statement here - to load the Multiple DNC DropDown.
            switch (ProgramCode)
            {
                //// Sample
                //case strCreditParameterApproval:
                //    FunpriBindMultipleDNC(new string[] { "-- Select --", "Customer", "Enquiry" }, ddlMultipleDNC);
                //    break;
                case strPaymentRequest:
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Option", "22");
                    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    DataTable dtPaymentMode = Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam);
                    ddlMultipleDNC.BindDataTable(dtPaymentMode, new string[] { "Lookup_Code", "Lookup_Description", });
                    break;
            }
        }
        else
        {
            // lblAutosuggestProgramIDSearch.Visible = cmbDocumentNumberSearch.Visible = true;
            FunPriMakeMultipleDNCVisible(lblMultipleDNC, ddlMultipleDNC, false);
        }

        if (intDNCOption == 1)
        {
            dictDNCOption = new Dictionary<int, string>();
            // Add here case statement here - to load the Multiple DNC option.
            switch (ProgramCode)
            {
                // Sample
                //case strCreditParameterTransactionCode:
                //    FunpriBindMultipleDNC(new string[] { "-- Select --", "Approved", "Unapproved" }, ddlDNCOption);
                //    break;               
            }

            FunPriMakeMultipleDNCVisible(lblDNCOption, ddlDNCOption, true);
        }
        else
        {
            FunPriMakeMultipleDNCVisible(lblDNCOption, ddlDNCOption, false);
        }

    }

    /// <summary>
    /// This is to load the specified Dropdown list box
    /// </summary>
    /// <param name="transactions"> strings to load the dropdown </param>
    /// <param name="ddl"> target to load the dropdown list boxes </param>
    /// <param name="dict"> dictionary as a source to load the dropdown </param>
    private void FunpriBindMultipleDNC(string[] transactions, DropDownList ddl)
    {
        Dictionary<int, string> dict = new Dictionary<int, string>();
        ddl.Items.Clear();
        for (int i_value = 0; i_value < transactions.Length; i_value++)
        {
            dict.Add(i_value, transactions[i_value].ToString());
        }

        ddl.DataSource = dict;
        ddl.DataTextField = "Value";
        ddl.DataValueField = "Key";
        ddl.DataBind();
    }


    /// <summary>
    /// To change the Multiple DNC option Visibility.
    /// </summary>
    /// <param name="lMultipleDNC"> Label to change its visibility </param>
    /// <param name="dDNCOption"> dropdown to change its visibility </param>
    /// <param name="blnMakeVisible">boolean - to set the visibility</param>
    private void FunPriMakeMultipleDNCVisible(Label lMultipleDNC, DropDownList dDNCOption, bool blnMakeVisible)
    {
        dDNCOption.Visible =
        lMultipleDNC.Visible = blnMakeVisible;
    }

    /// <summary>
    /// This method will Initialize the page depend on the document Number Code passed.
    /// </summary>
    protected void InitPage()
    {
        // Add here - your case condition - with respect to you program Code 
        // only If you're not passing the MultipleDNC Query String.
        // if you want to pass the LOB and branch as a query string - then make a call to FunPriQueryString();
        tdManual.Visible = false;
        switch (ProgramCode)
        {
            case strCRDRNoteApproval://Added By Vinodha M For New Screen Debit Credit Note Approval
                strProgramId = "300";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Page.Title = "S3G - " + FunPubGetPageTitles(enumPageTitle.PageTitle);
                strRedirectPage = "~/LoanAdmin/S3GLoanAdCreditDebitNoteApproval.aspx";
                lblAutosuggestProgramIDSearch.Text = "Debit Credit Approval No.";
                lblHeading.Text = "Debit Credit Note Approval";// FunPubGetPageTitles(enumPageTitle.Details);
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                rdoManual.Visible = false;
                ProgramCodeToCompare = strCRDRNoteApproval;
                rdoSystem.Visible = false;
                lblLesseeNameSrch.Visible = txtLessee.Visible = true;
                lblLesseeNameSrch.Text = "Lessee/Funder/Vendor Name";
                lblRSNumber.Visible = txtRSNumber.Visible = true;
                break;
            case strCRDRNote://Added By Sathish R For New Screen Credit Debit Note 
                strProgramId = "298";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Page.Title = "S3G - " + FunPubGetPageTitles(enumPageTitle.PageTitle);
                strRedirectPage = "~/LoanAdmin/S3GDebitCreditNote_Add.aspx";
                lblAutosuggestProgramIDSearch.Text = "Debit Credit No.";
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                strRedirectCreatePage = strRedirectPage;
                rdoManual.Visible = false;
                ProgramCodeToCompare = strCRDRNote;
                rdoSystem.Visible = false;
                //START By Siva.K 24JUN2015 Lessee
                lblLesseeNameSrch.Visible = txtLessee.Visible = true;
                lblLesseeNameSrch.Text = "Lessee/Funder/Vendor Name";
                //END By Siva.K 24JUN2015 
                lblRSNumber.Visible = txtRSNumber.Visible = true;
                lblDCNInvoice.Visible = txtDCNInvoice.Visible = true;
                lblOldInvoice.Visible = txtOldInvoice.Visible = true;
                lblType.Visible = ddlType.Visible = true;
                break;

            case strPaymentRequest:
                strProgramId = "54";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strPaymentRequest;
                this.Title = "S3G - Payment Request";
                lblHeading.Text = " Payment Request - Details";
                //Modified by chandru                
                lblAutosuggestProgramIDSearch.Text = "Payment Request No.";
                //Modified by chandru
                strRedirectPage = "~/LoanAdmin/S3gLoanAdPaymentRequest.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode  
                //lblLesseeNameSrch.Visible = ddlLessee.Visible = true;
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 22MAY2015 Lessee
                lblMultipleDNC.Text = "Payment Mode";
                // Added for Call Id : 3474 CR_057
                lblLesseeNameSrch.Text = "Lessee/Funder/Vendor Name";
                lblRSNumber.Visible = txtRSNumber.Visible = lblTrancheName.Visible = txtTrancheName.Visible = true;
                break;
            case strOverDueInterest:
                strProgramId = "72";
                FunPubIsVisible(true, true, false);
                //Source modified by Tamilselvan.S on 18/01/2011 for LOB and Branch as NonMandatory
                FunPubIsMandatory(false, false, false, false);
                //Source modified by Tamilselvan.S on 12/01/2011
                ProgramCodeToCompare = strOverDueInterest;
                this.Title = "S3G - Over Due Interest";
                lblAutosuggestProgramIDSearch.Text = "Over Due No.";                                         // This is to display on the Document Number Label                
                strRedirectPage = "~/LoanAdmin/S3gLoanAdOverDueInterest.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 24JUN2015 Lessee
                lblODIInvoice.Visible = txtODIInvoice.Visible = true;
                // Added for Call Id : 3474 CR_057
                lblRSNumber.Visible = txtRSNumber.Visible = lblTrancheName.Visible = txtTrancheName.Visible = true;
                break;
            //Code added and commented by saran on 26-Jul-2013 for BW start
            case strOverDueInterestBW:
                strProgramId = "250";
                FunPubIsVisible(true, true, false);
                //Source modified by Tamilselvan.S on 18/01/2011 for LOB and Branch as NonMandatory
                FunPubIsMandatory(false, false, false, false);
                //Source modified by Tamilselvan.S on 12/01/2011 
                ProgramCodeToCompare = strOverDueInterestBW;
                this.Title = "S3G - Over Due Interest";
                lblAutosuggestProgramIDSearch.Text = "Over Due No.";                                         // This is to display on the Document Number Label                
                strRedirectPage = "~/LoanAdmin/S3gLoanAdOverDueInterestBW.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                break;
            //Code added and commented by saran on 26-Jul-2013 for BW end
            case strLeaseAssetSaleApproval:
                strProgramId = "63";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strLeaseAssetSaleApproval;
                this.Title = "S3G - Lease Asset Sale Approval";
                lblHeading.Text = "Lease Asset Sale Approval - Details";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Lease Asset Sale No.";                                         // This is to display on the Document Number Label                
                lblAutosuggestProgramIDSearch.Text = "Lease Asset Sale No.";
                //Modified by chandru
                lblLesseeNameSrch.Text = "Lessee/Vendor Name";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdLeaseAssetSaleApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 11JUN2015 Lessee
                lblRSNumber.Visible = txtRSNumber.Visible = lblTrancheName.Visible = txtTrancheName.Visible = true; // Added for Call Id : 3474 CR_057
                break;
            case strPaymentApproval:
                strProgramId = "56";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strPaymentApproval;
                this.Title = "S3G - Payment Request Approval";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Payment Request No.";                                       // This is to display on the Document Number Label                
                lblAutosuggestProgramIDSearch.Text = "Payment Request No.";
                //Modified by chandru
                lblHeading.Text = "Payment Approval - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdPaymentApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                // Added for Call Id : 3474 CR_057
                lblLesseeNameSrch.Visible = txtLessee.Visible = true;
                lblLesseeNameSrch.Text = "Lessee/Funder/Vendor";
                lblRSNumber.Visible = txtRSNumber.Visible = lblTrancheName.Visible = txtTrancheName.Visible = true;
                break;
            case strTopupApproval:
                strProgramId = "71";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strTopupApproval;
                this.Title = "S3G - Top Up Approval";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "TLE/WC No";                                         // This is to display on the Document Number Label                
                lblAutosuggestProgramIDSearch.Text = "TLE/WC No";
                //Modified by chandru
                lblHeading.Text = "Top Up Approval - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdTLEWCTopupApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                break;
            case strSpecificRevisionApproval:
                strProgramId = "78";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strSpecificRevisionApproval;
                this.Title = "S3G - Revision Specific Approval";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Account Revision No";
                lblAutosuggestProgramIDSearch.Text = "Account Revision No";
                //Modified by chandru
                lblHeading.Text = "Revision Specific Approval - Details";// This is to display on the Document Number Label                
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountRevisionSpecificApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                break;
            case strManualJournalApproval:
                strProgramId = "81";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strManualJournalApproval;
                this.Title = "S3G - Manual Journal Approval";
                lblAutosuggestProgramIDSearch.Text = "MJV Number";
                lblHeading.Text = "Manual Journal Approval - Details";// This is to display on the Document Number Label                
                strRedirectPage = "~/LoanAdmin/S3GLoanAdManualJournalApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                break;
            case strPrematureClosureApproval:
                strProgramId = "86";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strPrematureClosureApproval;
                this.Title = "S3G - Premature Closure Approval";
                lblHeading.Text = "Premature Closure Approval - Details";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "PMC No";                                         // This is to display on the Document Number Label                
                lblAutosuggestProgramIDSearch.Text = "PMC No";
                //Modified by chandru
                strRedirectPage = "~/LoanAdmin/S3GLoanAdPreMatureClosureApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 15JUN2015 Lessee 
                lblRSNumber.Visible = txtRSNumber.Visible = lblTrancheName.Visible = txtTrancheName.Visible = true; // Added for Call Id : 3474 CR_057
                break;
            case strConsolidationApproval:
                strProgramId = "87";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strConsolidationApproval;
                this.Title = "S3G - Consolidation Approval";
                lblAutosuggestProgramIDSearch.Text = "Consolidation No";
                lblHeading.Text = "Consolidation Approval - Details";// This is to display on the Document Number Label                
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountConsolidationApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                break;
            case strSplitApproval:
                strProgramId = "88";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strSplitApproval;
                this.Title = "S3G - Split Approval";
                lblAutosuggestProgramIDSearch.Text = "Split No";
                lblHeading.Text = "Split Approval - Details";// This is to display on the Document Number Label                
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountSplitApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; 
                lblRSNumber.Visible = txtRSNumber.Visible = true;
                break;
            case strAccountclosureApproval:
                strProgramId = "69";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strAccountclosureApproval;
                lblHeading.Text = "Rental schedule Closure Approval - Details";
                this.Title = "S3G - Rental schedule Closure Approval";
                lblAutosuggestProgramIDSearch.Text = "Rental schedule Closure No.";                                         // This is to display on the Document Number Label                
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountClosureApproval_Add.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 15JUN2015 Lessee
                lblTrancheName.Visible = txtTrancheName.Visible = true; // Added For Call Id : 2696
                lblRSNumber.Visible = txtRSNumber.Visible = true; 
                break;
            case strAssetVerification:
                strProgramId = "52";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Asset Verification";
                ProgramCodeToCompare = strAssetVerification;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Asset Verification Number";
                lblAutosuggestProgramIDSearch.Text = "Asset Verification Number";
                //Modified by chandru
                lblHeading.Text = "Asset Verification - Details";
                strRedirectPage = "~/LoanAdmin/S3GLOANADAssetVerification_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;
            case strAccountActivation:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                strProgramId = "75";
                ProgramCodeToCompare = strAccountActivation;
                this.Title = "S3G - Rental Schedule Activation";
                lblHeading.Text = "Rental Schedule Activation - Details";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Account Activation Number";
                lblAutosuggestProgramIDSearch.Text = "Rental Schedule Number";
                //Modified by chandru
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountActivation_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                //lblLesseeNameSrch.Visible = ddlLessee.Visible = true; //By Siva.K On 20/05/2015
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 22MAY2015 Lessee
                break;
            case strAccountBulkActivation:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                strProgramId = "334";
                ProgramCodeToCompare = strAccountBulkActivation;
                this.Title = "S3G - Rental Schedule Bulk Activation";
                lblHeading.Text = "Rental Schedule Bulk Activation - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdRSActivation_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                //lblLesseeNameSrch.Visible = ddlLessee.Visible = true; //By Siva.K On 20/05/2015
                lblLesseeNameSrch.Visible = txtLessee.Visible = lblTrancheName.Visible = txtTrancheName.Visible = true; //By Siva.K 22MAY2015 Lessee
                lblAutosuggestProgramIDSearch.Visible = false;
                panAutoSuggest.Attributes.Add("style", "display:none");
                break;
            case strDeliveryIns:
                strProgramId = "55";//Added By Kuppusamy.B On 26/09/2011
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strDeliveryIns;
                this.Title = "S3G - Delivery Instruction / Lease Purchase Order";
                lblHeading.Text = "Delivery Instruction / Lease Purchase Order - Details";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "DI./LPO.No.";
                lblAutosuggestProgramIDSearch.Text = "DI./LPO.No.";
                //Modified by chandru
                strRedirectPage = "~/LoanAdmin/S3GLoanAdDeliveryInstructionLPO_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strTLEWC:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false); //Modified By Tamilselvan.S on 10/02/2011
                ProgramCodeToCompare = strTLEWC;
                strProgramId = "79";  //Added By Tamilselvan.S On 26/09/2011
                this.Title = "S3G - TE / WC TopUP";
                lblHeading.Text = "TE / WC TopUP - Details";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "TLE/WC.No.";
                lblAutosuggestProgramIDSearch.Text = "TLE/WC.No.";
                //Modified by chandru
                strRedirectPage = "~/LoanAdmin/S3GLoanAdTLEWCTopup_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;
            case strTLGL:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false); //Modified By Tamilselvan.S on 10/02/2011
                ProgramCodeToCompare = strTLGL;
                strProgramId = "220";  //Added By Tamilselvan.S On 26/09/2011
                this.Title = "S3G - Term Loan Topup - Gold Loan";
                lblHeading.Text = "Term Loan Topup - Gold Loan - Details";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "TLE/WC.No.";
                lblAutosuggestProgramIDSearch.Text = "TL Topup";
                //Modified by chandru
                strRedirectPage = "~/LoanAdmin/S3GLoanAdTLEWCTopup_Add_GL.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;


            case strGlobalParmSetup:
                //FunPubIsVisible(true, false, false);
                //FunPubIsMandatory(true, false);
                //ProgramCodeToCompare = strGlobalParmSetup;
                //this.Title = "S3G - Global Parameter Setup";
                //lblHeading.Text = "Global Parameter Setup";
                ////lblAutosuggestProgramIDSearch.Text = "DI./LPO.No.";
                //strRedirectPage = "~/LoanAdmin/S3GLoanAdGlobalParameterSetup_Add.aspx";
                //strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strFactoringInvoice:
                strProgramId = "57";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Factoring Invoice Loading";
                ProgramCodeToCompare = strFactoringInvoice;
                lblAutosuggestProgramIDSearch.Text = "Factoring Invoice Number";
                lblHeading.Text = "Factoring Invoice Loading - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdFactoringInvoiceLoading.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strFactoringInvoiceRetirement:
                strProgramId = "238";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Factoring Invoice Retirement";
                ProgramCodeToCompare = strFactoringInvoiceRetirement;
                lblAutosuggestProgramIDSearch.Text = "Factoring Invoice Retirement Number";
                lblHeading.Text = "Factoring Invoice Retirement - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdFactoringInvoiceRetirement.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strInvoiceVendor:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strInvoiceVendor;
                strProgramId = "58";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Invoice Number";
                lblAutosuggestProgramIDSearch.Text = "Invoice Number";
                //Modified by chandru
                this.Page.Title = "S3G - " + FunPubGetPageTitles(enumPageTitle.PageTitle);
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                strRedirectPage = "~/LoanAdmin/S3GLoanAdInvoiceVendor_Add.aspx";
                strRedirectCreatePage = strRedirectPage;
                break;

            case strAssetIdentification:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                strProgramId = "60";
                this.Title = "S3G - Asset Identification";
                ProgramCodeToCompare = strAssetIdentification;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Asset Identification Number";
                lblAutosuggestProgramIDSearch.Text = "Asset Identification Number";
                //Modified by chandru
                lblHeading.Text = "Asset Identification - Details";
                strRedirectPage = "~/LoanAdmin/S3GLOANADAssetIdentificationEntry_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;



            case strAssetAcquisition:
                strProgramId = "66";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strAssetAcquisition;
                this.Title = "S3G - Asset Acquisition";
                lblHeading.Text = "Asset Acquisition - Details";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Asset Acquisition No";
                lblAutosuggestProgramIDSearch.Text = "Asset Acquisition No";
                //Modified by chandru
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAssetAcquisition_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;
            case strMonthClosure:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strMonthClosure;
                this.Title = "S3G - Month Closure";
                lblHeading.Text = "Month Closure - Details";
                lblAutosuggestProgramIDSearch.Text = "Closure Month";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdMonthClosure.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strLeaseAssetSale:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                strProgramId = "62";
                this.Title = "S3G - Lease Asset Sale";
                ProgramCodeToCompare = strLeaseAssetSale;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Lease Asset Sale Number";
                lblAutosuggestProgramIDSearch.Text = "Lease Asset Sale Number";
                //Modified by chandru
                lblLesseeNameSrch.Text = "Lessee/Vendor Name";
                lblHeading.Text = "Lease Asset Sale - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAd_LeaseAssetSale.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 11JUN2015 Lessee
                lblInvoice.Visible = txtInvoice.Visible = true;
                lblRSNumber.Visible = txtRSNumber.Visible = lblTrancheName.Visible = txtTrancheName.Visible = true; // Added for Call Id : 3474 CR_057
                break;

            case strOperaingDepreciation:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                strProgramId = "64";
                this.Title = "S3G - Operating Lease Depreciation";
                ProgramCodeToCompare = strOperaingDepreciation;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Depreciation Number";
                lblAutosuggestProgramIDSearch.Text = "Depreciation Number";
                //Modified by chandru
                lblHeading.Text = "Operating Lease Depreciation";
                strRedirectPage = "~/LoanAdmin/S3GLOANADOperatingLeaseDepreciation_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strBulkRevision:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Bulk Revision";
                ProgramCodeToCompare = strBulkRevision;
                strProgramId = "67";  //Added By Tamilselvan.S On 26/09/2011
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Bulk Revision Number";
                lblAutosuggestProgramIDSearch.Text = "Bulk Revision Number";
                //Modified by chandru
                lblHeading.Text = "Bulk Revision Number";
                strRedirectPage = "~/LoanAdmin/S3gLoanAdBulkRevision_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strOperatingLeaseExpenses:
                strProgramId = "68";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Operating Lease Expenses";
                ProgramCodeToCompare = strOperatingLeaseExpenses;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Operating Lease Expenses Number";
                lblAutosuggestProgramIDSearch.Text = "Operating Lease Expenses Number";
                //Modified by chandru
                lblHeading.Text = "Operating Lease Expenses - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdOperatingLeaseExpenses.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strNocTermination:
                strProgramId = "76";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - NOC Termination";
                ProgramCodeToCompare = strNocTermination;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "NOC Number";
                lblAutosuggestProgramIDSearch.Text = "NOC Number";
                //Modified by chandru
                lblHeading.Text = "NOC Termination - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdNocTermination.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;
            case strAccountConsolidation:
                strProgramId = "70";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Account Consolidation";
                ProgramCodeToCompare = strAccountConsolidation;
                lblAutosuggestProgramIDSearch.Text = "Account Consolidation Number";
                lblHeading.Text = "Account Consolidation - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountConsolidation.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;
            case strAccountSplit:
                strProgramId = "83";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Rental Schedule Split";
                ProgramCodeToCompare = strAccountSplit;
                lblAutosuggestProgramIDSearch.Text = "Rental Schedule Split Number";
                lblHeading.Text = "Rental Schedule Split - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountSplit.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 15JUN2015 Lessee 
                lblRSNumber.Visible = txtRSNumber.Visible = true;
                break;
            case strCashflowMntlyBking:
                strProgramId = "84";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Cashflow Monthly Booking";
                ProgramCodeToCompare = strCashflowMntlyBking;
                lblAutosuggestProgramIDSearch.Text = "Cashflow Monthly Booking Number";
                lblHeading.Text = "Cashflow Monthly Booking - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdCashFlowMnthBk.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;
            case strAccountClosure:
                strProgramId = "73";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Rental Schedule Closure";
                ProgramCodeToCompare = strAccountClosure;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Account Closure Number";
                lblAutosuggestProgramIDSearch.Text = "Rental Schedule Closure Number";
                //Modified by chandru
                lblHeading.Text = "Rental Schedule Closure - Details";
                strRedirectPage = "~/LoanAdmin/S3GLOANADAccountClosure_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 15JUN2015 Lessee
                lblTrancheName.Visible = txtTrancheName.Visible = true; // Added For Call Id : 2696
                lblRSNumber.Visible = txtRSNumber.Visible = true;
                break;
            case strMJV:
                strProgramId = "77";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                tdManual.Visible = true;
                this.Page.Title = "S3G - " + FunPubGetPageTitles(enumPageTitle.PageTitle);
                if (rdoManual.Checked)
                {
                    ProgramCodeToCompare = strMJV;
                    strRedirectPage = "~/LoanAdmin/S3GLoanAdManualJournal_Add.aspx";
                }
                else
                {
                    ProgramCodeToCompare = strSysJournal;
                    strRedirectPage = "~/LoanAdmin/S3GLoanAdSysJournal.aspx";
                }
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Journal Number";
                lblAutosuggestProgramIDSearch.Text = "Journal Number";
                //Modified by chandru
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                strRedirectCreatePage = strRedirectPage;
                break;
            case strSpecificRevision:
                strProgramId = "74";
                FunPubIsVisible(true, true, true);
                this.Page.Title = "S3G-Account Specific Revision";
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strSpecificRevision;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Specific Revision";
                lblAutosuggestProgramIDSearch.Text = "Specific Revision";
                //Modified by chandru
                lblHeading.Text = "Account Specific Revision";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountSpecificRevision.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;


            //FunPubIsVisible(true, true, true);
            //FunPubIsMandatory(false, false, false, false);
            //strProgramId = "62";
            //this.Title = "S3G - Lease Asset Sale";
            //ProgramCodeToCompare = strLeaseAssetSale;
            //lblAutosuggestProgramIDSearch.Text = "Lease Asset Sale Number";
            //lblHeading.Text = "Lease Asset Sale - Details";
            //strRedirectPage = "~/LoanAdmin/S3GLoanAd_LeaseAssetSale.aspx";
            //strRedirectCreatePage = strRedirectPage + "?qsMode=C";
            //break;
            case strAccountCreation:
                strProgramId = "80";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Account Creation";
                ProgramCodeToCompare = strAccountCreation;
                lblHeading.Text = "Rental Schedule Creation - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountCreation.aspx";
                lblAutosuggestProgramIDSearch.Text = "Rental Schedule Number";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                //lblLesseeNameSrch.Visible = ddlLessee.Visible = true;
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 22MAY2015 Lessee
                break;

            case strAccountCreationBW:
                strProgramId = "253";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Account Creation";
                ProgramCodeToCompare = strAccountCreation;
                lblHeading.Text = "Account Creation - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountCreationBW.aspx";
                lblAutosuggestProgramIDSearch.Text = "Account Creation Number";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;

            case strBilling:
                strProgramId = "95";
                FunPubIsVisible(true, true, false);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Bill Generation";
                ProgramCodeToCompare = strBilling;
                lblHeading.Text = "Bill Generation - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdBilling.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;


            case strInterim:
                strProgramId = "323";
                FunPubIsVisible(true, true, false);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - Interim Billing";
                ProgramCodeToCompare = strInterim;
                lblHeading.Text = "Interim Billing";
                strRedirectPage = "~/LoanAdmin/S3G_Loanad_InterimBilling.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;
            case strRBilling:
                strProgramId = "295";
                FunPubIsVisible(true, true, false);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G -Regularisation Bill Generation";
                ProgramCodeToCompare = strRBilling;
                lblHeading.Text = "Regularisation Bill Generation - Details";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdBilling_Regularisation.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                break;
            case strPreMatureClosure:
                strProgramId = "85";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                this.Title = "S3G - PreMature Closure";
                ProgramCodeToCompare = strPreMatureClosure;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "PreMature Closure Number";
                lblAutosuggestProgramIDSearch.Text = "PreMature Closure Number";
                //Modified by chandru
                lblHeading.Text = "PreMature Closure - Details";
                strRedirectPage = "~/LoanAdmin/S3GLOANADPreMatureClosure_Add.aspx";
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                lblLesseeNameSrch.Visible = txtLessee.Visible = true; //By Siva.K 15JUN2015 Lessee
                // Added for Call Id : 3474 CR_057
                lblRSNumber.Visible = txtRSNumber.Visible =  lblTrancheName.Visible = txtTrancheName.Visible = true;
                lblInvoice.Visible = txtInvoice.Visible = true;
              break;

            case strIncomeRecognition:
                strProgramId = "114";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strIncomeRecognition;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Frequency Type";
                lblAutosuggestProgramIDSearch.Text = "Frequency Type";
                //Modified by chandru
                this.Page.Title = "S3G - " + FunPubGetPageTitles(enumPageTitle.PageTitle);
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                strRedirectPage = "~/LoanAdmin/S3GLoanAdIncomeRecognition.aspx";
                strRedirectCreatePage = strRedirectPage;
                break;

            case strPDTransaction:
                strProgramId = "185";
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strPDTransaction;
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "PDD Trans. Number";
                lblAutosuggestProgramIDSearch.Text = "PDD Trans. Number";
                //Modified by chandru
                this.Page.Title = "S3G - " + FunPubGetPageTitles(enumPageTitle.PageTitle);
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                strRedirectPage = "~/LoanAdmin/S3GLoanAdPDDTransaction_Add.aspx";
                strRedirectCreatePage = strRedirectPage;
                break;

            case strCompensation:
                strProgramId = "213";
                FunPubIsVisible(true, true, false);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strCompensation;
                this.Page.Title = "S3G - " + FunPubGetPageTitles(enumPageTitle.PageTitle);
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                strRedirectPage = "~/LoanAdmin/S3GLoanAdCompensation.aspx";
                strRedirectCreatePage = strRedirectPage;
                break;
            //Added for Loan End Use Approval
            case strLoanEndUseApproval:
                strProgramId = "260";//Loan End Use Approval
                FunPubIsVisible(true, true, false);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strLoanEndUseApproval;
                this.Title = "S3G - Loan End Use Approval";
                lblHeading.Text = " Loan End Use Approval - Details";
                lblAutosuggestProgramIDSearch.Text = "Prime Account Number";
                strRedirectPage = "~/LoanAdmin/S3GLoanAdLoanEndUseApproval.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode  
                break;
        }

    }


    private void FunPriFilterAndLoadLOB()
    {

        //if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.DataSource != null)
        //{
        //    DataTable dtLOB = (DataTable)ComboBoxLOBSearch.DataSource;
        //    if (dtLOB != null && dtLOB.Rows.Count > 0)
        //    {
        //        StringBuilder strddlItem = new StringBuilder();
        //        for (int i_lob = 0; i_lob < strLOBCodeToFilter.Length; i_lob++)
        //        {
        //            strddlItem.Append(" LOB_Code like '" + strLOBCodeToFilter[i_lob] + "' or ");
        //        }
        //        strddlItem.Append(" LOB_Code like '" + strLOBCodeToFilter[0] + "'");

        //        dtLOB.DefaultView.RowFilter = strddlItem.ToString();

        //        ComboBoxLOBSearch.Items.Clear();

        //        //dtLOB.Columns.Add("DataText", typeof(string), "LOB_Code+'  -  '+LOB_Name");

        //        ComboBoxLOBSearch.DataValueField = "LOB_ID";
        //        ComboBoxLOBSearch.DataTextField = "DataText";

        //        ComboBoxLOBSearch.DataSource = dtLOB;

        //        ComboBoxLOBSearch.DataBind();

        //        ListItem liSelect = new ListItem("-- Select --", "0");
        //        ComboBoxLOBSearch.Items.Insert(0, liSelect);
        //        //ComboBoxLOBSearch.SelectedValue = "3";
        //    }

        //}

    }


    /// <summary>
    /// To set the LOB and Branch Mandatory/NonMandatory
    /// </summary>
    /// <param name="isLOBMandatory">true to set the LOB DDL Mandatory</param>
    /// <param name="isBranchMandatory">true to set the Branch DDL Mandatory</param>
    private void FunPubIsMandatory(bool isLOBMandatory, bool isBranchMandatory)
    {
        // To make the LOB and Branch Non-Mandatory
        RFVComboBranch.Enabled = isBranchMandatory;
        RFVComboLOB.Enabled = isLOBMandatory;
        // To change the Label style to Non mandatory
        lblLOBSearch.CssClass = (isLOBMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel";
        lblBranchSearch.CssClass = (isBranchMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel"; ;
    }

    private void FunPubIsMandatory(bool isLOBMandatory, bool isBranchMandatory, bool isStartDateMandatory, bool isEndDateMandatory)
    {
        // To make the LOB and Branch Non-Mandatory
        RFVComboBranch.Enabled = (isBranchMandatory) ? true : false;
        RFVComboLOB.Enabled = (isLOBMandatory) ? true : false;
        RFVStartDate.Enabled = (isStartDateMandatory) ? true : false;
        RFVEndDate.Enabled = (isEndDateMandatory) ? true : false;
        // To change the Label style to Non mandatory
        lblLOBSearch.CssClass = (isLOBMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel";
        lblBranchSearch.CssClass = (isBranchMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel";
        lblStartDateSearch.CssClass = (isStartDateMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel";
        lblEndDateSearch.CssClass = (isEndDateMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel";
    }
    /// <summary>
    /// To set the LOB and Branch visible/NonVisible
    /// </summary>
    /// <param name="isLOBMandatory">true to set the LOB DDL Mandatory</param>
    /// <param name="isBranchMandatory">true to set the Branch DDL Mandatory</param>
    private void FunPubIsVisible(bool isLOB, bool isBranch, bool isMultipleDNC)
    {
        // To make the LOB and Branch Non-Mandatory
        RFVComboBranch.Enabled = isBranch;
        RFVComboLOB.Enabled = isLOB;
        // To change the Label style to Non mandatory
        lblLOBSearch.CssClass = (isLOB) ? "styleReqFieldLabel" : "styleDisplayLabel";
        ComboBoxLOBSearch.Visible = lblLOBSearch.Visible = isLOB;

        lblBranchSearch.CssClass = (isBranch) ? "styleReqFieldLabel" : "styleDisplayLabel"; ;
        //ComboBoxBranchSearch.Visible = lblBranchSearch.Visible = isBranch;
        txtBranchSearch.Visible = lblBranchSearch.Visible = isBranch;
        //lblAutosuggestProgramIDSearch.Visible = cmbDocumentNumberSearch.Visible = isMultipleDNC;
        lblAutosuggestProgramIDSearch.Visible = txtDocumentNumberSearch.Visible = isMultipleDNC;

    }

    /// <summary>
    /// To Bind the Landing Grid
    /// </summary>
    /// <param name="intPageNum"> Current Page Number of the grid </param>
    /// <param name="intPageSize"> Current Page size of the grid </param>
    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;              // To set the page Number
        ProPageSizeRW = intPageSize;            // To set the page size    
        FunPriBindGrid();                       // Binding the Landing grid
    }

    /// <summary>
    /// This is tp load the combo(s) in the page.
    /// </summary>
    protected void FunProLoadCombos()
    {
        Procparam = new Dictionary<string, string>();

        switch (ObjUserInfo.ProUserTypeRW)
        {
            case "UTPA":
                {
                    // LOB ComboBoxLOBSearch For UTPA Login
                    Procparam.Clear();
                    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    Procparam.Add("@UTPA_ID", intUserID.ToString());
                    Procparam.Add("@Program_ID", strProgramId);
                    ComboBoxLOBSearch.BindDataTable("S3G_Get_UTPA_LOB_LIST", Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                    //ComboBoxLOBSearch.SelectedValue = "3";
                    // Branch ComboBoxBranchSearch For UTPA Login
                    //Procparam.Clear();
                    //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    //Procparam.Add("@UTPA_ID", Convert.ToString(intUserID));
                    ////Procparam.Add("@Is_Active", "1");
                    //Procparam.Add("@Program_ID", strProgramId);
                    //Procparam.Add("@LOB_ID", "0");
                    //ComboBoxBranchSearch.BindDataTable("S3G_Get_UTPA_Branch_List", Procparam, true, "-- Select --", new string[] { "Location_Id", "Location" });

                }
                break;
            default:
                {

                    // LOB ComboBoxLOBSearch
                    if (Procparam != null)
                        Procparam.Clear();
                    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    Procparam.Add("@User_ID", Convert.ToString(intUserID));
                    Procparam.Add("@Program_Id", strProgramId);
                    //Procparam.Add("@Is_Active", "1");
                    if (strTLEWC == ProgramCode || strTopupApproval == ProgramCode)  ///Added by Tamilselvan.S on 15/02/2011 for fetching the TEWC Topup
                    {
                        Procparam.Add("@FilterOption", "'TE','WC'");
                    }
                    if (strTLGL == ProgramCode)  ///Added by Tamilselvan.S on 15/02/2011 for fetching the TEWC Topup
                    {
                        Procparam.Add("@FilterOption", "'TL'");
                    }
                    if (ProgramCode == strAssetVerification)  ///Added by Ganapathy Subramanian.G on 03/07/2012 for fetching the Asset VErification LOB's begin
                    {
                        Procparam.Add("@FilterOption", "'HP','OL','LN','FL'");
                    }
                    //END
                    if (ProgramCode == strBilling)  ///Added by Ganapathy Subramanian.G on 21/01/2013 for fetching the Billing LOB's begin
                    {
                        Procparam.Add("@FilterOption", "'HP','OL','LN','FL','TE','TL'");
                    }
                    //END
                    if (ProgramCode == strDeliveryIns)  ///Added by Manikandan. R on 16/02/2011 for fetching the Delivery Instruction
                    {
                        Procparam.Add("@FilterOption", "'HP','OL','LN','FL'");
                    }

                    if (ProgramCode == strFactoringInvoice)  ///Added by Manikandan. R on 16/02/2011 for fetching the Factoring Invoice Loading 
                    {
                        Procparam.Add("@FilterOption", "'FT'");
                    }

                    if (ProgramCode == strFactoringInvoiceRetirement)  ///Added by Bhuvana on 26/05/2013 for fetching the Factoring Invoice Retirement 
                    {
                        Procparam.Add("@FilterOption", "'FT'");
                    }

                    if (ProgramCode == strSpecificRevision)  ///Added by Srivatsan. R on 16/02/2012 for fetching the Specific Revision LOBs
                    {
                        Procparam.Add("@FilterOption", "'FL','HP','LN','OL','TE','TL'");
                    }

                    if (ProgramCode == strInvoiceVendor)
                    {
                        Procparam.Add("@FilterType", "0");
                        Procparam.Add("@FilterCode", "'TL','TE','FT','WC'");
                        ComboBoxLOBSearch.BindDataTable(SPNames.S3G_ORG_GetSpecificLOBList, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                    }
                    else if (ProgramCode == strPreMatureClosure) /// Added by Kuppusamy.B - 30/07/2012 - To fetch only  - "'HP','LN','OL','TL','TE'" - LOB's for Pre_Mature_Closure
                    {
                        Procparam.Add("@FilterType", "1");
                        Procparam.Add("@FilterCode", "'HP','LN','OL','TL','TE'");
                        ComboBoxLOBSearch.BindDataTable(SPNames.S3G_ORG_GetSpecificLOBList, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                    }
                    else
                    {
                        ComboBoxLOBSearch.BindDataTable(SPNames.LOBMaster, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                    }
                    //ComboBoxLOBSearch.SelectedValue = "3";
                    // branch
                    //Procparam.Clear();
                    //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    //Procparam.Add("@Program_Id", strProgramId);
                    //Procparam.Add("@User_ID", Convert.ToString(intUserID));
                    ////Procparam.Add("@Is_Active", "1");
                    //ComboBoxBranchSearch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });

                }
                break;
        }

        if (ComboBoxLOBSearch.Items.Count == 2)
        {
            ComboBoxLOBSearch.SelectedIndex = 1;
            ComboBoxLOBSearch.ClearDropDownList();
        }
        // Loading Multiple DNC
        FunPriLoadMultiDNCCombo();

        FunPriLoadDNCCombo();

        switch (ProgramCode)
        {
            case "OLE": // Remove LOB Except Operating Lease-OL
                // Operating Lease Expenses
                strLOBCodeToFilter = new string[] { "OL" };
                FunPriFilterAndLoadLOB();
                break;
            case "NOCT": // Remove LOB Except Hire Purchase-HP,LOAN-LN,Finance Lease-FL,Operating Lease-OL,Term LOAN-TL,Term LOAN Extensible-TE
                //NOC Termination
                strLOBCodeToFilter = new string[] { "HP", "LN", "FL", "OL", "TL", "TE" };
                FunPriFilterAndLoadLOB();
                break;

            case "LAA":
                strLOBCodeToFilter = new string[] { "FL", "OL" };
                FunPriFilterAndLoadLOB();
                break;

            case "LATUA":
                strLOBCodeToFilter = new string[] { "TE", "WC" };
                FunPriFilterAndLoadLOB();
                break;

            case "FIL":
                //Factoring Invoice Loading
                strLOBCodeToFilter = new string[] { "FT" };
                FunPriFilterAndLoadLOB();
                break;

            case "FLR":
                //Factoring Invoice Retirement
                strLOBCodeToFilter = new string[] { "FT" };
                FunPriFilterAndLoadLOB();
                break;

            case "LAS":
                //Lease Asset Sale
                strLOBCodeToFilter = new string[] { "FL", "OL" };
                FunPriFilterAndLoadLOB();
                break;
            case "DEI":
                //Delivery Instruction
                strLOBCodeToFilter = new string[] { "HP", "LN", "FL", "OL" };
                FunPriFilterAndLoadLOB();
                break;
            case "LA7CO": // Remove LOB Except Hire Purchase-HP,LOAN-LN,Finance Lease-FL,Operating Lease-OL,Term LOAN-TL,Term LOAN Extensible-TE
            case "LA8SP":
            case "LCAPP":
            case "LSAPP":

                strLOBCodeToFilter = new string[] { "HP", "LN", "FL" };
                FunPriFilterAndLoadLOB();
                break;
            case "BURE":
                strLOBCodeToFilter = new string[] { "HP", "LN", "FL", "OL" };// Remove LOB Except Hire Purchase-HP,LOAN-LN,Finance Lease-FL,Operating Lease-OL
                FunPriFilterAndLoadLOB();
                break;
            case "COI":
                strLOBCodeToFilter = new string[] { "HP", "LN", "FL", "OL", "FT", "TL", "TE" };// Load LOB Except Working Capital
                FunPriFilterAndLoadLOB();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Will load the DNC Combo
    /// </summary>
    private void FunPriLoadDNCCombo()
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        if (ProgramCode == strPaymentRequest)
        {
            Procparam.Add("@Option", "17");
            DataTable dtStatus = Utility.GetDefaultData("S3G_Get_PaymentCMNLST", Procparam);
            ddlDNCOption.FillDataTable(dtStatus, "PaymentStatusID", "PaymentStatus", true);
        }

        //// Add Here - your case statement blow if needed
        //// Document Number Combo - Before add your code check if the common Function was available.       
        //switch (ProgramCode)
        //{
        //    case strPaymentRequest:                         // Payment Request
        //        FunPriLoadCombo_PaymentRequestNumber();
        //        break;
        //    case strPaymentApproval:                         // Payment Request
        //        FunPriLoadCombo_PaymentRequestApproval();
        //        break;
        //    case strLeaseAssetSaleApproval:                         // Payment Request
        //        FunPriLoadCombo_LeaseAssetSaleApproval();
        //        break;
        //    case strAccountclosureApproval:                         // Payment Request
        //        FunPriLoadCombo_AccountClosureApproval();
        //        break;
        //    case strTopupApproval:                         // Payment Request
        //        FunPriLoadCombo_TopUpApproval();
        //        break;
        //    case strSpecificRevisionApproval:                         // Payment Request
        //        FunPriLoadCombo_RevisionSpecificApproval();
        //        break;
        //    case strManualJournalApproval:                         // Payment Request
        //        FunPriLoadCombo_ManualJournalApproval();
        //        break;
        //    case strPrematureClosureApproval:                         // Payment Request
        //        FunPriLoadCombo_PrematureClosureApproval();
        //        break;
        //    case strConsolidationApproval:                         // Payment Request
        //        FunPriLoadCombo_ConsolidationApproval();
        //        break;
        //    case strSplitApproval:                         // Payment Request
        //        FunPriLoadCombo_SplitApproval();
        //        break;
        //    case strAssetVerification:
        //        FunPriLoadCombo_AssetsVerified();    // Loading the verified assets
        //        break;
        //    case strAccountActivation:
        //        FunPriLoadCombo_AccountActivation();    //Loading the Account Number
        //        break;
        //    case strTLEWC:
        //        FunPriLoadCombo_TLEWC();    //Loading the DI/LPO No
        //        break;
        //    case strDeliveryIns:
        //        FunPriLoadCombo_DeliveryIns();    //Loading the DI/LPO No
        //        break;

        //    case strFactoringInvoice:
        //        FunPriLoadCombo_FILNo();    // Loading the Factoring Invoice  Number                
        //        break;
        //    case strInvoiceVendor: //InvoiceVendor
        //        FunPriLoadCombo_InvoiceVendorNo();
        //        break;

        //    case strAssetIdentification:
        //        FunPriLoadCombo_AssetIdentified();
        //        break;


        //    case strAssetAcquisition:
        //        FunPriLoadCombo_AssetAcquisition();
        //        break;
        //    case strMonthClosure:
        //        FunPriLoadCombo_MonthClosure();

        //        break;

        //    case strLeaseAssetSale:
        //        FunPriLoadCombo_LASNUM();    // Loading the Leae Asset Sale No
        //        break;
        //    case strOperaingDepreciation:
        //        FunPriLoadCombo_DepreciatedSJVNumbers();
        //        break;
        //    case strAccountConsolidation:
        //        FunPriLoadCombo_AccountConsolidationNumbers();
        //        break;
        //    case strAccountSplit:
        //        FunPriLoadCombo_AccountSplitNumbers();
        //        break;
        //    case strCashflowMntlyBking:
        //        FunPriLoadCombo_CashFlowMntlyBkingNumbers();
        //        break;
        //    case strNocTermination:
        //        FunPriLoadCombo_NocTerminationNumbers();
        //        break;
        //    case strOperatingLeaseExpenses:
        //        FunPriLoadCombo_OperatingLeaseExpensesNumbers();
        //        break;

        //    case strAccountClosure:
        //        FunPriLoadCombo_AccountsClosured();
        //        break;
        //    case strMJV:

        //        FunPriLoadCombo_ManualJournal();
        //        break;
        //    case strSpecificRevision:
        //        FunPriLoadCombo_SpecificRevision();
        //        break;
        //    case strBulkRevision:
        //        FunPriLoadCombo_BulkRevisionNumber();
        //        break;
        //    case strPreMatureClosure:
        //        FunPriLoadCombo_PreAccountsClosured();
        //        break;
        //    case strIncomeRecognition:
        //        FunPriLoadCombo_FrequencyType();
        //        break;
        //    case strPDTransaction:
        //        FunPriLoadCombo_PDDT();
        //        break;
        //    default:
        //        // to do: disable the page 
        //        break;

    }

    # region Document Number controls to load the Combo Box
    /// <summary>
    ///  Load the Document Number Combo Box with Bulk Revision Number
    /// </summary>
    //private void FunPriLoadCombo_BulkRevisionNumber()
    //{ 
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetBulkRevisionNo, Procparam, true, "-- Select --", new string[] { "ID", "Bulk_Revision_Number" });
    //}
    /// <summary>
    ///  Load the Document Number Combo Box with Payment Request Number
    /// </summary>
    //private void FunPriLoadCombo_PaymentRequestNumber()
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));

    //    Procparam.Add("@Program_Id", strProgramId);

    //    if (txtStartDateSearch.Text != "")
    //    {
    //        Procparam.Add("@Fromdate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    }
    //    if (txtEndDateSearch.Text != "")
    //    {
    //        Procparam.Add("@Todate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    }
    //    cmbDocumentNumberSearch.BindDataTable("S3G_LOANAD_GetPaymentRequestNoCombo", Procparam, true, "-- Select --", new string[] { "ID", "Payment_Request_No" });
    //}
    //private void FunPriLoadCombo_PaymentRequestApproval()   //Payment Approval
    //{

    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(10));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "Payment_Request_No");
    //        Ds = null;
    //    }
    //}
    //private void FunPriLoadCombo_LeaseAssetSaleApproval()   //Lease Asset Sale Approval
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(12));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "LeaseAssetSales_No");
    //        Ds = null;
    //    }
    //}
    //private void FunPriLoadCombo_AccountClosureApproval()   //Lease Asset Sale Approval
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(15));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //        Ds = null;
    //    }
    //}
    //private void FunPriLoadCombo_TopupApproval()   //Top up Approval
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(14));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "TLE_WC_No");
    //        Ds = null;
    //    }
    //}
    //private void FunPriLoadCombo_RevisionSpecificApproval()   //Revision Specific Approval
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(28));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //        Ds = null;
    //    }
    //}
    //private void FunPriLoadCombo_ManualJournalApproval()   //Manual Journal Approval
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(29));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //        Ds = null;
    //    }
    //}
    //private void FunPriLoadCombo_PrematureClosureApproval()   //Premature Closure Approval
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(13));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //        Ds = null;
    //    }
    //}
    //private void FunPriLoadCombo_ConsolidationApproval()   //Consolidation Approval
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(30));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //        Ds = null;
    //    }
    //}
    //private void FunPriLoadCombo_SplitApproval()   //Split Approval
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@Task_Type", Convert.ToString(31));
    //    if (ComboBoxLOBSearch.SelectedIndex > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        DataSet Ds = new DataSet();
    //        Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //        cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //        Ds = null;
    //    }
    //}
    /// <summary>
    ///  Load the Document Number Combo Box with Customer Code
    /// </summary>
    //private void FunPriLoadCombo_CustomerCode()
    //{
    //    FunPri_LOB_Branch();
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_Get_Customer_Code, Procparam, true, "-- Select --", new string[] { "ID", "Customer_Code", "Customer_Name" });
    //}
    /// <summary>
    ///  Load the Document Number Combo Box with Enquiry Number
    /// </summary>
    //private void FunPriLoadCombo_EnquiryNo()
    //{
    //    FunPri_LOB_Branch();
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_Get_Enquiry_No, Procparam, true, "-- Select --", new string[] { "ID", "Enquiry_No" });
    //}
    /// <summary>
    /// Will Load the Document Number Combo Box with Sanction Number
    /// </summary>
    //private void FunPriLoadCombo_SanctionNo()
    //{
    //    FunPri_LOB_Branch();
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_ORG_GetSanctionNo, Procparam, true, "-- Select --", new string[] { "ID", "Santion_No" });
    //}
    //private void FunPriLoadCombo_AssetsVerified()
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    if (ComboBoxLOBSearch.SelectedItem.Text.ToUpper().Contains("OPERAT"))
    //    {
    //        Procparam.Add("@Is_OL", "1");
    //    }
    //    else if (ComboBoxLOBSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@Is_OL", "0");
    //    }
    //    //Change Made on 09-Feb-2011 For Loading Asset verification number
    //    //Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    //Procparam.Add("@Branch_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    //Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetAssetsVerified, Procparam, true, "--Select--", new string[] { "ID", "Asset_Verification_No" });
    //}

    //private void FunPriLoadCombo_DeliveryIns()
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    }
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    }
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //    {
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    }
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //    {
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    }
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_DeliveryIns, Procparam, true, "--Select--", new string[] { "DeliveryInstruction_No", "DeliveryInstruction_No" });
    //}
    //private void FunPriLoadCombo_TLEWC()
    //{
    //    FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetTLEWC, Procparam, true, "--Select--", new string[] { "ID", "TLE_WC_No" });
    //}
    //private void FunPriLoadCombo_AccountsClosured()
    //{
    //    //throw new NotImplementedException();

    //    if (ComboBoxLOBSearch.SelectedValue != "0") Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedValue != "0") Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));

    //    Procparam.Add("@Program_Id", strProgramId);
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetAccountsClosured, Procparam, true, "--Select--", new string[] { "ID", "Closure_No" });
    //}
    //private void FunPriLoadCombo_PreAccountsClosured()
    //{
    //    //throw new NotImplementedException();
    //    if (ComboBoxLOBSearch.SelectedValue != "0") Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedValue != "0") Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Program_Id", strProgramId);
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetPreClosedAccounts, Procparam, true, "--Select--", new string[] { "ID", "Closure_No" });
    //}

    //private void FunPriLoadCombo_FrequencyType()
    //{
    //    Procparam = new Dictionary<string, string>();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@LookupType_Code", "8");
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, Procparam, true, "--Select--", new string[] { "Lookup_Code", "Lookup_Description" });
    //}

    //private void FunPriLoadCombo_ManualJournal()
    //{
    //    //throw new NotImplementedException();

    //    Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    if (rdoSystem.Checked)
    //    {
    //        Procparam.Add("@Journal_Type", "SYS");
    //    }
    //    else
    //    {
    //        Procparam.Add("@Journal_Type", "MANUAL");
    //    }
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetMJV_List, Procparam, true, "--Select--", new string[] { "ID", "MJVNO" });
    //}
    //private void FunPriLoadCombo_SpecificRevision()
    //{
    //    //throw new NotImplementedException();

    //    // Add by Narasimha Rao 5th Nov 2011. To load Specific Revision No in Translander Page. -- (Checking the condition if value == 0)
    //    if (ComboBoxLOBSearch.SelectedValue != "0") Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedValue != "0") Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));

    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetSpecificRevisionNo, Procparam, true, "--Select--", new string[] { "Account_Revision_Number", "Account_Revision_Number" });
    //}
    /// <summary>
    /// Load the Account Activation ID 
    /// </summary>
    //private void FunPriLoadCombo_AccountActivation()
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    }
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    }
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //    {
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    }
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //    {
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    }
    //    Procparam.Add("@Company_ID", intCompanyID.ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetAccountsActivated, Procparam, true, "--Select--", new string[] { "ID", "Account_Activation_Number" });
    //}

    //private void FunPriLoadCombo_PDDT()
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    }
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    }
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //    {
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    }
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //    {
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    }
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable("S3G_LOANAD_TransLander_DDL_PDDTransaction", Procparam, true, "--Select--", new string[] { "ID", "PDDT_Number" });

    //}

    //private void FunPriLoadCombo_AssetIdentified()
    //{

    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    }
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    }
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //    {
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    }
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //    {
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    }
    //    if (ComboBoxLOBSearch.SelectedItem.Text.ToUpper().Contains("OPERAT"))
    //    {
    //        Procparam.Add("@Is_OL", "1");
    //    }
    //    else if (ComboBoxLOBSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@Is_OL", "0");
    //    }
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetAssetsIdentified, Procparam, true, "--Select--", new string[] { "Asset_Identification_No", "Asset_Identification_No" });
    //}

    //private void FunPriLoadCombo_FILNo()
    //{
    //    //FunPri_LOB_Branch();
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    }
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    }
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //    {
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    }
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //    {
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    }
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetFILNOTransLand, Procparam, true, "--Select--", new string[] { "Factoring_Inv_Load_ID", "Fil_No" });
    //}

    //private void FunPriLoadCombo_MonthClosure()
    //{

    //    int intMaxMonth = 0;
    //    string strFinancialYear = "";
    //    strFinancialYear = DateTime.Now.AddYears(intMaxMonth).Year.ToString() + "-" + DateTime.Now.AddYears(intMaxMonth + 1).Year.ToString();
    //    cmbDocumentNumberSearch.FillFinancialMonth(strFinancialYear);

    //}
    /// <summary>
    ///  Load the Asset Acquisition ID 
    /// </summary>
    //private void FunPriLoadCombo_AssetAcquisition()
    //{
    //    Procparam.Clear();
    //    strLOBCodeToFilter = new string[] { "OL" };
    //    FunPriFilterAndLoadLOB();

    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    }
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //    {
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    }
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //    {
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    }
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //    {
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    }
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetAssetAcquisitionID, Procparam, true, "--Select--", new string[] { "ID", "Acquisition_No" });
    //}
    //private void FunPriLoadCombo_InvoiceVendorNo()
    //{
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    cmbDocumentNumberSearch.BindDataTable("S3G_LOANAD_GetInvoiceVendorNo", Procparam, true, "-- Select --", new string[] { "Invoice_ID", "Vendor_Invoice_No" });
    //}
    //private void FunPriLoadCombo_LASNUM()
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetLASNUMTransLand, Procparam, true, "--Select--", new string[] { "ID", "LASNO" });
    //}
    //private void FunPriLoadCombo_DepreciatedSJVNumbers()
    //{
    //    // Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    strLOBCodeToFilter = new string[] { "OL" };
    //    FunPriFilterAndLoadLOB();
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetDepreciatedSJV, Procparam, true, "--Select--", new string[] { "ID", "Depreciation_ID" });
    //}
    /// <summary>
    /// 
    /// </summary>
    //private void FunPriLoadCombo_AccountConsolidationNumbers()
    //{
    //    Procparam.Clear();
    //    Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    Procparam.Add("@Option", "2");
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetAccCons_Split_List, Procparam, true, "--Select--", new string[] { "Consolidation_No", "Consolidation_No" });
    //}
    //private void FunPriLoadCombo_AccountSplitNumbers()
    //{
    //    Procparam.Clear();
    //    Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    Procparam.Add("@Option", "5");
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetAccCons_Split_List, Procparam, true, "--Select--", new string[] { "Account_Split_No", "Account_Split_No" });
    //}
    //private void FunPriLoadCombo_CashFlowMntlyBkingNumbers()
    //{
    //    Procparam.Clear();
    //    //Procparam.Add("@Branch_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    Procparam.Add("@Option", "2");
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetCashMntlyBk_List, Procparam, true, "--Select--", new string[] { "Monthly_Bk_Id", "Monthly_bk_no" });
    //}
    //private void FunPriLoadCombo_NocTerminationNumbers()
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetNocTerminationID, Procparam, true, "--Select--", new string[] { "ID", "NOCNo" });
    //}
    //private void FunPriLoadCombo_OperatingLeaseExpensesNumbers()
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetOLENUMTransLand, Procparam, true, "--Select--", new string[] { "ID", "OLENO" });
    //}
    //private void FunPriLoadCombo_LeaseAssetSaleApproval() //Lease Asset sale Approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "12");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "LeaseAssetSales_No");
    //    Ds = null;

    //}
    //private void FunPriLoadCombo_TopUpApproval() //Top up approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "14");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "TLE_WC_NO");
    //    Ds = null;

    //}
    //private void FunPriLoadCombo_ConsolidationApproval()//consolidation Approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "30");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "Consolidation_No");
    //    Ds = null;

    //}
    //private void FunPriLoadCombo_SplitApproval()//split Approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "31");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //    Ds = null;

    //}
    //private void FunPriLoadCombo_ManualJournalApproval()   //Manual Journal Approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "29");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "Manual_Journal_Voucher_No");
    //    Ds = null;

    //}
    //private void FunPriLoadCombo_AccountClosureApproval()   //Account Closure Approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "15");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //    Ds = null;


    //}
    //private void FunPriLoadCombo_PrematureClosureApproval()   //Premature Closure Approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "13");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //    Ds = null;
    //}
    //private void FunPriLoadCombo_RevisionSpecificApproval()   //Revision Specific Approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "28");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "ID");
    //    Ds = null;

    //}
    //private void FunPriLoadCombo_PaymentRequestApproval()   //Payment Approval
    //{
    //    if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //    if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //        Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //    if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    Procparam.Add("@Task_Type", "10");
    //    DataSet Ds = new DataSet();
    //    Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, Procparam);
    //    cmbDocumentNumberSearch.FillDataTable(Ds.Tables[2], "ID", "Payment_Request_No");
    //    Ds = null;


    //}
    /// <summary>
    /// To load the DNC according to the Lob and branch selected.
    /// </summary>
    private void FunPri_LOB_Branch()
    {
        //if (ComboBoxBranchSearch != null && ComboBoxBranchSearch.SelectedIndex > 0)
        //{
        //    Procparam.Add("@Location_ID", ComboBoxBranchSearch.SelectedValue.ToString());
        //}
        if (hdnBranchID.Value != null && hdnBranchID.Value != string.Empty)
        {
            Procparam.Add("@Location_ID", hdnBranchID.Value);
        }
        if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.SelectedIndex > 0)
        {
            Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue.ToString());
        }

        //commected by chandru
        //if (cmbDocumentNumberSearch != null)
        //cmbDocumentNumberSearch.Items.Clear();
        //commected by chandru
    }
    #endregion

    /// <summary>
    /// Bind the Landing Grid.
    /// </summary>
    private void FunPriBindGrid()
    {
        try
        {
            FunPriAddCommonParameters();                                                        // Adding the Common parameters to the dictionary            
            // Passing DNC ddl value to the SP
            //Modified by chandru on 23/04/2012
            //if (cmbDocumentNumberSearch != null && cmbDocumentNumberSearch.SelectedIndex > 0)   // General document no of your page (ex)FIR Number,Enquiry No,App Processing No,
            if (txtDocumentNumberSearch.Text != string.Empty)   // General document no of your page (ex)FIR Number,Enquiry No,App Processing No,
            {
                //Procparam.Add("@DocumentNumber", cmbDocumentNumberSearch.SelectedValue);
                Procparam.Add("@DocumentNumber", hdnCommonID.Value);
                //Modified by chandru on 23/04/2012
            }
            // Passing Multiple DNC ddl value to the SP
            if (ddlMultipleDNC != null && ddlMultipleDNC.Visible && ddlMultipleDNC.SelectedIndex > 0)
            {
                Procparam.Add("@MultipleDNC_ID", ddlMultipleDNC.SelectedValue);
            }
            // Passing Multiple DNC option ddl value to the SP
            if (ddlDNCOption != null && ddlDNCOption.Visible && ddlDNCOption.SelectedIndex > 0)
            {
                Procparam.Add("@MultipleOption_ID", ddlDNCOption.SelectedValue);
            }
            //Modified by chandru on 23/04/2012
            //if ((cmbDocumentNumberSearch != null && cmbDocumentNumberSearch.SelectedIndex > 0) && IsApprovalNeed)   // using for approval screen purpose
            if (txtDocumentNumberSearch.Text != string.Empty && IsApprovalNeed)
            {
                //Procparam.Add("@Task_Number_ID", cmbDocumentNumberSearch.SelectedValue);
                Procparam.Add("@Task_Number_ID", hdnCommonID.Value);
            }
            //Modified by chandru on 23/04/2012
            Procparam.Add("@ProgramCode", ProgramCodeToCompare.ToString()); // using for approval screen purpose

            //Parameter added for UTPA Access - Asset vreification - Kuppu- Aug-27 
            if (ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "UTPA")
            {
                Procparam.Add("@User_Type", ObjUserInfo.ProUserTypeRW.ToString());
            }

            bool colModify = true;//This is to hide column grid
            bool colQuery = true;

            //Add here - add your extra SP parameters - if required... in the below switch case (also Add the same to the common SP - with your program Code Commented).
            switch (ProgramCode)
            {
                //    //sample 
                //case strFieldInformationReportCode:
                //    break;
                //case strAssetVerification:
                //    break;
                //case strAssetIdentification:
                //    break;
                //case strOperaingDepreciation:
                //    break;
                //case strAccountClosure:
                //    break;
                //case strAccountActivation:
                //    break;
                // Added for Call Id : 3474 CD_057
                case strCRDRNote:
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    if (Convert.ToString(txtDCNInvoice.Text) != "")
                        Procparam.Add("@Invoice_No", Convert.ToString(hdnDCNInvoice.Value));
                    if (Convert.ToString(txtOldInvoice.Text) != "")
                        Procparam.Add("@OldInvoice_No", Convert.ToString(hdnOldInvoice.Value));
                    if (Convert.ToString(ddlType.SelectedValue) != "0")
                        Procparam.Add("@Tran_Type", ddlType.SelectedValue);
                    break;
                case strCRDRNoteApproval:
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;
                case strOverDueInterest:
                    if (Convert.ToString(txtTrancheName.Text) != "")
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    if (Convert.ToString(txtODIInvoice.Text) != "")
                        Procparam.Add("@Invoice_No", Convert.ToString(hdnODIInvoice.Value));
                    break;
                case strAccountClosure:
                    Procparam.Add("@ClosureType", "1");
                    if (Convert.ToString(txtLessee.Text) != "") //By Siva.K 15JUN2015 Lessee
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    if (Convert.ToString(txtTrancheName.Text) != "") // Added For Call Id : 2696
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;
                case strAccountclosureApproval:  //By Siva.K 15JUN2015 Lessee
                    if (Convert.ToString(txtLessee.Text) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    if (Convert.ToString(txtTrancheName.Text) != "") // Added For Call Id : 2696
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;
                case strPreMatureClosure:
                    Procparam.Add("@ClosureType", "2");
                    if (Convert.ToString(txtLessee.Text) != "") //By Siva.K 15JUN2015 Lessee
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    // Added for Call Id : 3474 CR_057
                    if (Convert.ToString(txtTrancheName.Text) != "") 
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    if (Convert.ToString(txtInvoice.Text) != "")
                        Procparam.Add("@Invoice_No", Convert.ToString(hdnInvoice.Value));
                    break;
                case strOperatingLeaseExpenses:
                    colModify = false;
                    break;
                case strMJV:
                    if (rdoSystem.Checked)
                    {
                        colModify = false;
                    }
                    break;

                case strAssetVerification:
                    break;
                case strAssetIdentification:
                    if (ComboBoxLOBSearch.SelectedItem.Text.ToUpper().Contains("OPERAT"))
                    {
                        Procparam.Add("@Is_OL", "1");
                    }
                    else if (ComboBoxLOBSearch.SelectedIndex > 0)
                    {
                        Procparam.Add("@Is_OL", "0");
                    }
                    break;
                case strPaymentRequest:
                    if (Convert.ToString(txtTrancheName.Text) != "")
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;
                case strPaymentApproval:
                     if (Convert.ToString(txtTrancheName.Text) != "")
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;
                case strAccountCreation:
                    if (Convert.ToString(txtLessee.Text) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    break;
                case strAccountActivation:
                    if (Convert.ToString(txtLessee.Text) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    break;
                case strAccountBulkActivation:
                    if (Convert.ToString(txtLessee.Text) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    if (Convert.ToString(txtTrancheName.Text) != "")
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    break;
                case strLeaseAssetSale:
                    //if (Convert.ToString(txtLessee.Text) != "")
                    //    Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    // Added for Call Id : 3474 CD_057
                    if (Convert.ToString(txtTrancheName.Text) != "")
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    if (Convert.ToString(txtInvoice.Text) != "")
                        Procparam.Add("@Invoice_No", Convert.ToString(hdnInvoice.Value));
                    break;
                case strLeaseAssetSaleApproval:
                    //if (Convert.ToString(txtLessee.Text) != "")
                    //    Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    // Added for Call Id : 3474 CD_057
                    if (Convert.ToString(txtTrancheName.Text) != "")
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;
                case strPrematureClosureApproval: // By Siva.K on 15JUN2015 Include lessee in Translander
                    if (Convert.ToString(txtLessee.Text) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    // Added for Call Id : 3474 CD_057
                    if (Convert.ToString(txtTrancheName.Text) != "")
                        Procparam.Add("@Tranche_ID", Convert.ToString(hdnTranche.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;
                case strAccountSplit: // By Siva.K on 15JUN2015 Include lessee in Translander
                    if (Convert.ToString(txtLessee.Text) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;
                case strSplitApproval: // By Siva.K on 15JUN2015 Include lessee in Translander
                    if (Convert.ToString(txtLessee.Text) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(hdnLessee.Value));
                    if (Convert.ToString(txtRSNumber.Text) != "")
                        Procparam.Add("@PA_SA_Ref_ID", Convert.ToString(hdnRSNumber.Value));
                    break;

                //case strPaymentRequest:
                //    if (Convert.ToString(ddlLessee.SelectedText) != "")
                //        Procparam.Add("@Customer_ID", Convert.ToString(ddlLessee.SelectedValue));
                //    break;
                //case strAccountCreation:
                //    if (Convert.ToString(ddlLessee.SelectedText) != "")
                //        Procparam.Add("@Customer_ID", Convert.ToString(ddlLessee.SelectedValue));
                //    break;
                //case strAccountActivation:
                //    if (Convert.ToString(ddlLessee.SelectedText) != "")
                //        Procparam.Add("@Customer_ID", Convert.ToString(ddlLessee.SelectedValue));
                //    break;
            }

            if (!bModify)
                colModify = false;

            FunPriBindGridWithFooter(colModify, colQuery);                                                                   // Binding the grid.

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

    /// <summary>
    /// Will bind the grid view 
    /// </summary>
    private void FunPriBindGridWithFooter(bool colModify, bool colQuery)
    {
        int intTotalRecords = 0;
        bool bIsNewRow = false;
        grvTransLander.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        //This is to hide first row if grid is empty
        if (bIsNewRow)
        {
            grvTransLander.Rows[0].Visible = false;
        }
        //if (ProgramCodeToCompare == strPaymentApproval || ProgramCodeToCompare == strLeaseAssetSaleApproval || ProgramCodeToCompare == strAccountclosureApproval || ProgramCodeToCompare == strTopupApproval || ProgramCodeToCompare == strSpecificRevisionApproval || ProgramCodeToCompare == strManualJournalApproval || ProgramCodeToCompare == strPrematureClosureApproval || ProgramCodeToCompare == strConsolidationApproval || ProgramCodeToCompare == strSplitApproval)
        if ((IsApprovalNeed) || (!colModify))
        {
            grvTransLander.Columns[1].Visible = false;
        }
        else
        {
            grvTransLander.Columns[1].Visible = true;
        }
        //if (ProgramCode == strDeliveryIns)
        //{
        //    grvTransLander.Columns[1].Visible = false;
        //}
        ucCustomPaging.Visible = true;
        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucCustomPaging.setPageSize(ProPageSizeRW);
        lblErrorMessage.Text = "";
    }

    /// <summary>
    /// Will add the common parameters to the Dictionary - to pass it to the Common SP.
    /// </summary>
    private void FunPriAddCommonParameters()
    {
        //Paging Properties set  
        int intTotalRecords = 0;
        ObjPaging.ProCompany_ID = intCompanyID;
        ObjPaging.ProUser_ID = intUserID;
        ObjPaging.ProTotalRecords = intTotalRecords;
        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        if (strProgramId == "298" || strProgramId == "300" || strProgramId == "72" || strProgramId == "54" || strProgramId == "56") //By Siva.K 24JUN2015 Lessee
            ObjPaging.ProSearchValue = Convert.ToString(txtLessee.Text);
        else if (strProgramId == "62" || strProgramId == "63")
            ObjPaging.ProSearchValue = hdnLessee.Value;
        else
            ObjPaging.ProSearchValue = hdnSearch.Value;

        ObjPaging.ProOrderBy = hdnOrderBy.Value;

        //ObjPaging.ProProgram_ID = Convert.ToInt32(strProgramId);

        Procparam = new Dictionary<string, string>();
        if (Procparam != null)
        {
            Procparam.Clear();
        }
        if (ComboBoxLOBSearch.SelectedIndex > 0)
        {
            Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue.ToString());
        }
        //if (ComboBoxBranchSearch.SelectedIndex > 0)
        //{
        //    Procparam.Add("@Location", ComboBoxBranchSearch.SelectedValue.ToString());
        //}
        if (txtBranchSearch.Text != string.Empty)    // General document no of your page (ex)FIR Number,Enquiry No,App Processing No,
        {
            Procparam.Add("@Location", hdnBranchID.Value.ToString());
        }
        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());


        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
            Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    }

    /// <summary>
    /// This is the get the datatype of the string passed
    /// </summary>
    /// <param name="val">string</param>
    /// <returns>   
    ///             1 for int, 
    ///             2 for datetime, 
    ///             3 for string
    /// </returns>
    private Int32 FunPriTypeCast(string val)
    {
        //try                                                         // casting - to use proper align       
        //{
        //    Int32 tempint = Convert.ToInt32(Convert.ToDecimal(val));                   // Try int     
        //    return 1;
        //}
        //catch (Exception ex)
        //{
        //    try
        //    {
        //        DateTime tempdatetime = Convert.ToDateTime(val);    // try datetime
        //        return 2;
        //    }
        //    catch (Exception ex1)
        //    {
        //        return 3;                                           // try String
        //    }
        //}
        return 3;

    }

    /// <summary>
    /// This method will validate the from and to date - entered by the user.
    /// </summary>
    private void FunPriValidateFromEndDate()
    {
        DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
        dtformat.ShortDatePattern = "MM/dd/yy";

        //if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()))) &&
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtStartDateSearch.Text) > Utility.StringToDate(txtEndDateSearch.Text)) // start date should be less than or equal to the enddate
            {
                //Utility.FunShowAlertMsg(this, "Start Date should be lesser than or equal to the End Date");
                Utility.FunShowAlertMsg(this, "End date should be greater than or equal to start date");
                FunPriBindGrid();
                return;
            }
        }
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           ((string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtEndDateSearch.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        }
        //if (((string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
        //    (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))
        //{
        //    txtStartDateSearch.Text = txtEndDateSearch.Text;

        //}

        FunPriBindGrid();
    }
    #endregion

    #region Control Events
    /// <summary>
    /// Will bind the grid and validate the from and to date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        #region  User Authorization
        if (!bIsActive)
        {
            isEditColumnVisible =
            isQueryColumnVisible = false;
        }
        if ((!bModify) || (intModify == 0))
        {
            isEditColumnVisible = false;

        }
        if ((!bQuery) || (intQuery == 0))
        {
            isQueryColumnVisible = false;
        }
        #endregion

        grvTransLander.Visible = true;
        FunPriValidateFromEndDate();
    }

    /// <summary>
    /// Will set the Grid Style and Alignment of the string dynamically depend on the data types of the cell
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvTransLander_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)                 // if header - then set the style dynamically.
        {
            for (int i_cellVal = 2; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                e.Row.Cells[i_cellVal].CssClass = "styleGridHeader";
            }
        }
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow) // to hide the "ID" column
        {


            e.Row.Cells[3].Visible = false;                             // ID Column - always invisible
            e.Row.Cells[4].Visible = false;                             // User ID Column - always invisible
            e.Row.Cells[5].Visible = false;                             // User Level ID Column - always invisible
            if (ProgramCode == strBilling || ProgramCode == strBulkRevision || ProgramCode == strCompensation || ProgramCode == strRBilling)
            {
                e.Row.Cells[1].Visible = false;
            }
            //Code added and commented by saran on 26-Jul-2013 for BW start
            //if (ProgramCode == strOverDueInterest || ProgramCode == strOverDueInterestBW)
            //{
            //    e.Row.Cells[11].Visible = false;
            //}
            //Code added and commented by saran on 26-Jul-2013 for BW end
            if (ProgramCode == strInterim)
            {
                e.Row.Cells[1].Visible = false;
            }
            //if (ProgramCode == strPreMatureClosure)
            //{
            //    e.Row.Cells[11].Visible = false;

            //}
        }
        if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
        {

            #region User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

            ImageButton imgbtnQuery = (ImageButton)e.Row.FindControl("imgbtnQuery");

            if (ObjUserInfo.ProUserTypeRW.ToUpper() == "USER")
            {
                if ((intModify != 0) && ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text)))))
                {
                    imgbtnEdit.Enabled = true;
                }
                else
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
                if (intQuery != 0)
                {
                    imgbtnQuery.Enabled = true;
                }
                else
                {
                    imgbtnQuery.Enabled = false;
                    imgbtnQuery.CssClass = "styleGridQueryDisabled";
                }
            }
            else if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                if ((intModify != 0) && bModify)
                {
                    imgbtnEdit.Enabled = true;
                }
                else
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
                if (intQuery != 0 && bQuery)
                {
                    imgbtnQuery.Enabled = true;
                }
                else
                {
                    imgbtnQuery.Enabled = false;
                    imgbtnQuery.CssClass = "styleGridQueryDisabled";
                }
            }


            //if (imgbtnEdit != null)
            //{
            //    if ((intModify != 0) && ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text)))))
            //    {
            //        imgbtnEdit.Enabled = true;
            //    }
            //    else
            //    {
            //        imgbtnEdit.Enabled = false;
            //        imgbtnEdit.CssClass = "styleGridEditDisabled";
            //    }
            //}
            //if (imgbtnQuery != null)
            //{
            //    if (intQuery != 0)
            //    {
            //        imgbtnQuery.Enabled = true;
            //    }
            //    else
            //    {
            //        imgbtnQuery.Enabled = false;
            //        imgbtnQuery.CssClass = "styleGridQueryDisabled";
            //    }
            //}

            #endregion

            if (ProgramCode == strCRDRNote)
            {
                if (e.Row.Cells[14].Text.ToUpper() != "PENDING")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
            if (ProgramCode == strInterim)
            {
                e.Row.Cells[1].Visible = false;

            }
            //if (ProgramCode == strPreMatureClosure)
            //{
            //    e.Row.Cells[11].Visible = false;

            //}

            if (ProgramCode == strAccountCreation)
            {
                //strQueryString = "AppProcessId =" + e.Row.Cells[3].Text + "&" +
                //"CompanyId=" + e.Row.Cells[7].Text + "&" + "PANum=" + e.Row.Cells[8].Text + "&" + "SANum=" + e.Row.Cells[9].Text;

                /*UAT Issues - changed by Prabhu.K on 29-Nov-2011 (User can modify only in configured status accounts) */
                if (!e.Row.Cells[12].Text.ToUpper().Contains("CONFIGURED") && !e.Row.Cells[12].Text.ToUpper().Contains("ACTIVATED") && !e.Row.Cells[12].Text.ToUpper().Contains("ACTIVE"))
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }

                //if (e.Row.Cells[12].Text.ToUpper() == "SPLIT CONFIGURED")
                //{
                //    imgbtnEdit.Enabled = false;
                //    imgbtnEdit.CssClass = "styleGridEditDisabled";
                //}

                if (e.Row.Cells[12].Text.ToUpper().Contains("ACTIVATED") && intUserLevelID <= 2)
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }

            }
            if (ProgramCode == strSpecificRevision) // Added by Rajendran 5/11/2011
            {
                if (e.Row.Cells[12].Text == "Rejected" || e.Row.Cells[12].Text == "Cancelled")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }

            }
            if ((ProgramCode == strTLEWC) || (ProgramCode == strTLGL)) // Added by Tamilselvan on 16/06/2011
            {
                if (e.Row.Cells[8].Text == "Rejected" || e.Row.Cells[8].Text == "Cancelled"
                    || e.Row.Cells[8].Text == "Approved" || e.Row.Cells[8].Text == "Under Process")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }

            if (ProgramCode == strAccountClosure || ProgramCode == strPreMatureClosure)
            {
                if (e.Row.Cells[11].Text.Trim() == "Cancelled" || e.Row.Cells[11].Text.Trim() == "Approved")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
            //Code added and commented by saran on 26-Jul-2013 for BW start
            if (ProgramCode == strOverDueInterest || ProgramCode == strOverDueInterestBW)
            {
                //e.Row.Cells[11].Visible = false;
                if (e.Row.Cells[12].Text.Trim().ToUpper() != "R")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
            for (int i_cellVal = 2; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                try
                {

                    // if (!string.IsNullOrEmpty(e.Row.Cells[i_cellVal].Text) && e.Row.Cells[i_cellVal].Text.Contains("/"))
                    if (!string.IsNullOrEmpty(e.Row.Cells[i_cellVal].Text))
                    {
                        Int32 type = 0;       // 1 = int, 2 = datetime, 3 = string

                        type = FunPriTypeCast(e.Row.Cells[i_cellVal].Text);

                        // cell alignment
                        switch (type)
                        {
                            case 1:  // int - right to left
                                e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Right;
                                break;
                            case 2:  // datetime - trim to code standard
                                e.Row.Cells[i_cellVal].Text = DateTime.Parse(e.Row.Cells[i_cellVal].Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                                break;
                            case 3:  // string - do nothing - left align(default)
                                e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Left;
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //continue;
                }
            }

            if (ProgramCode == strPaymentRequest) // Added by M.Saran on 30-07-2011.
            {
                //if (e.Row.Cells[10] != null && e.Row.Cells[10].Text != string.Empty)
                //{
                //    e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                //}
                //if (e.Row.Cells[11].Text == "Rejected" || e.Row.Cells[11].Text == "Cancelled")
                //{
                //    imgbtnEdit.Enabled = false;
                //    imgbtnEdit.CssClass = "styleGridEditDisabled";
                //}
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                imgbtnEdit.Enabled = (e.Row.Cells[11].Text == "Pending") ? true : false;
                imgbtnEdit.CssClass = (e.Row.Cells[11].Text == "Pending") ? "styleGridEdit" : "styleGridEditDisabled";
            }
            if (ProgramCode == strSpecificRevision) // Added by Narasimha Rao on 27 Jan 2012.
            {
                if (e.Row.Cells[7] != null && e.Row.Cells[7].Text != string.Empty)
                {
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                }
                if (e.Row.Cells[8] != null && e.Row.Cells[8].Text != string.Empty)
                {
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                }
                if (e.Row.Cells[9] != null && e.Row.Cells[9].Text != string.Empty)
                {
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            if (ProgramCode == strAccountConsolidation)    // Added by Narasimha Rao On 27 Jan 2012
            {
                if (e.Row.Cells[9] != null && e.Row.Cells[9].Text != string.Empty)
                {
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            if (ProgramCode == strAccountSplit)    // Added by Narasimha Rao On 27 Jan 2012
            {
                if (e.Row.Cells[8] != null && e.Row.Cells[8].Text != string.Empty)
                {
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                }
                if (e.Row.Cells[9].Text == "Split Cancelled")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
            //Added on 28Jan2016 Starts Here
            if (ProgramCode == strLeaseAssetSale)
            {
                if (Convert.ToString(e.Row.Cells[10].Text) == "Pending")
                {
                    imgbtnEdit.Enabled = true;
                }
                else
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }

            //Added on 28Jan2016 Ends Here
        }
        //if (ProgramCodeToCompare == strOperatingLeaseExpenses)
        //{ e.Row.Cells[1].Visible = false; }
        //else
        //{ e.Row.Cells[1].Visible = true; }


    }




    /// <summary>
    /// It will redirect the page from LAnding to Document Number Page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvTransLander_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);

        if (strRedirectPage.Contains('?'))
            strRedirectPage += "&";
        else
            strRedirectPage += "?";

        switch (e.CommandName.ToLower())
        {
            case "modify":
                //Added by Thangam M on 28-Sep-2013 for Row lock
                string strUserRowLocked = Utility.FunPriCheckRowConcurrency(intUserID, ProgramCodeToCompare, e.CommandArgument.ToString(), Session.SessionID);
                if (strUserRowLocked != "0")
                {
                    Utility.FunShowAlertMsg(this, strUserRowLocked);
                    return;
                }

                Session["RemoveLock"] = "1";
                Response.Redirect(strRedirectPage + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPage + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;

        }

    }


    /// <summary>
    /// This is to add the LOB and Branch as a Query String to your - redirect page
    /// </summary>
    private string FunPriAddLOBBranchQueryString(string strTargetURL)
    {
        //if (ComboBoxBranchSearch != null && ComboBoxBranchSearch.Items.Count > 0 && ComboBoxBranchSearch.SelectedIndex > 0)
        //{
        //    if (strTargetURL.Contains('?'))
        //        strTargetURL += "&Branch=" + ComboBoxBranchSearch.SelectedValue.ToString();
        //    else
        //        strTargetURL += "?Branch=" + ComboBoxBranchSearch.SelectedValue.ToString();
        //}
        if (hdnBranchID.Value != null && hdnBranchID.Value != string.Empty)
        {
            if (strTargetURL.Contains('?'))
                strTargetURL += "&Branch=" + hdnBranchID.Value;
            else
                strTargetURL += "?Branch=" + hdnBranchID.Value;
        }
        if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.Items.Count > 0 && ComboBoxLOBSearch.SelectedIndex > 0)
            if (strTargetURL.Contains('?'))
                strTargetURL += "&LOB=" + ComboBoxLOBSearch.SelectedValue.ToString();
            else
                strTargetURL += "?LOB=" + ComboBoxLOBSearch.SelectedValue.ToString();
        return strTargetURL;
    }



    /// <summary>
    /// Will call create Page - depend on the Program ID passed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Session["ApplicationAssetDetails"] = null;
        Session["AccountAssetCustomer"] = null;
        if (string.IsNullOrEmpty(strRedirectCreatePage))
        {
            Utility.FunShowAlertMsg(this, "Target page not found");
            return;
        }


        // to get the S3G_LOANAD_GetProgram_Ref_No
        #region To maintain Program_Ref_No in session
        DataTable dt_Program_Ref_No = Utility.FunGetProgramDetailsByProgramCode(ProgramCode);
        if (dt_Program_Ref_No != null && dt_Program_Ref_No.Rows.Count > 0)
        {
            Session["Program_Ref_No"] = dt_Program_Ref_No.Rows[0]["Program_Ref_No"].ToString();
        }
        else
        {
            Session["Program_Ref_No"] = null;
        }
        #endregion

        if (ProgramCodeToCompare == "ACBAC")
            strRedirectCreatePage = "~/LoanAdmin/S3GLoanAdRSActivation_Add.aspx?qsMode=C";

        Response.Redirect(strRedirectCreatePage);
    }

    /// <summary>
    /// Will clear the controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddlMultipleDNC != null && ddlMultipleDNC.Visible && ddlMultipleDNC.Items.Count > 0)
                ddlMultipleDNC.SelectedIndex = 0;
            if (ddlDNCOption != null && ddlDNCOption.Visible && ddlDNCOption.Items.Count > 0)
                ddlDNCOption.SelectedIndex = 0;

            grvTransLander.DataSource = null;
            grvTransLander.Visible = false;
            ucCustomPaging.Visible = false;
            lblErrorMessage.Text =
            txtStartDateSearch.Text =
            txtEndDateSearch.Text = "";
            System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"] = null;
            System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"] = null;
            //if (cmbDocumentNumberSearch.Items.Count > 1)
            //    cmbDocumentNumberSearch.SelectedIndex = -1;

            ComboBoxLOBSearch.SelectedIndex = 0;
            //ComboBoxBranchSearch.SelectedIndex = 0;
            txtBranchSearch.Text = string.Empty;
            hdnBranchID.Value = string.Empty;
            FunPriLoadDNCCombo();
            txtDocumentNumberSearch.Text = string.Empty;
            hdnCommonID.Value = string.Empty;
            //ddlLessee.Clear();
            txtLessee.Text = string.Empty;  //By Siva.K 22MAY2015 Lessee
            hdnLessee.Value = string.Empty;
            txtOldInvoice.Text = hdnOldInvoice.Value = txtInvoice.Text = hdnInvoice.Value = txtDCNInvoice.Text = hdnDCNInvoice.Value = txtODIInvoice.Text = hdnODIInvoice.Value = 
            txtRSNumber.Text = hdnRSNumber.Value = txtTrancheName.Text = hdnTranche.Value = string.Empty;  // Call Id : 2696
            ddlType.SelectedValue = "0";
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// If there is more than one Document Number - then use this DDL
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlMultipleDNC_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMultipleDNC.SelectedIndex > 0)
        {

            //cmbDocumentNumberSearch.Visible =
            //lblAutosuggestProgramIDSearch.Visible = true;
            //lblAutosuggestProgramIDSearch.Text = ddlMultipleDNC.SelectedItem.ToString();
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            Procparam.Clear();



            // Add here case statement here - to load document number, and document number label.
            // Add your code here if you are passing the MultipleDNC Query String.
            switch (ProgramCode)
            {
                //    //sample 
                //case strCreditParameterTransactionCode:
                //    ProgramCodeToCompare = strCreditParameterTransactionCode;
                //    strRedirectPage = "~/Origination/S3G_ORG_CreditParameterTransaction.aspx";     // page to redirect to the page                                                                  
                //    Procparam.Add("@Company_ID", intCompanyID.ToString());
                //    switch (Convert.ToInt32(ddlMultipleDNC.SelectedValue))
                //    {

                //        case 1:         // Customer Code                           
                //            FunPriLoadCombo_CustomerCode_CPT();
                //            break;
                //        case 2:         // Enquiry Number                            
                //            FunPriLoadCombo_EnquiryNo_CPT();
                //            break;
                //    }
                //    break;               

            }
        }
        //if (ddlMultipleDNC.SelectedIndex == 0)
        //{

        //    cmbDocumentNumberSearch.Visible =
        //    lblAutosuggestProgramIDSearch.Visible = false;
        //}
    }

    /// <summary>
    /// If there is any specific option releated to the Multiple DNC - then use this method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDNCOption_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// To load the DNC combo according to this field get change 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ComboBoxLOBSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadDNCCombo();                               // to load DNC
        ddlMultipleDNC_SelectedIndexChanged(sender, e);     // to Load Multiple DNC


        //switch (ObjUserInfo.ProUserTypeRW)
        //{
        //    case "UTPA":
        //        Procparam.Clear();
        //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //        Procparam.Add("@UTPA_ID", Convert.ToString(intUserID));
        //        //Procparam.Add("@Is_Active", "1");
        //        Procparam.Add("@Program_ID", strProgramId);
        //        Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue);
        //        ComboBoxBranchSearch.BindDataTable("S3G_Get_UTPA_Branch_List", Procparam, true, "-- Select --", new string[] { "Location_Id", "Location" });
        //        break;
        //    default:
        //        // branch 
        //        Procparam.Clear();
        //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
        //        //Procparam.Add("@Is_Active", "1");
        //        if (ComboBoxLOBSearch.SelectedIndex > 0)
        //            Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue);
        //        Procparam.Add("@Program_Id", strProgramId);
        //        ComboBoxBranchSearch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });
        //        break;
        //}
    }

    protected void Journal_OnCheckedChanged(object sender, EventArgs e)
    {
        FunPriLoadDNCCombo();                               // to load DNC
        grvTransLander.DataSource = null;
        grvTransLander.DataBind();
        ucCustomPaging.Visible = false;
    }
    /// <summary>
    /// To load the DNC combo according to this field get change 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void ComboBoxBranchSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ProgramCode)
        {
            //    // sample
            //case strCreditGuideTransaction:
            //    break;
            default:
                FunPriLoadDNCCombo();                               // To load DNC
                ddlMultipleDNC_SelectedIndexChanged(sender, e);     // To load Multiple DNC    
                break;

        }
    }
    #endregion

    private void FunPriQueryString()
    {
        // Add here: if you want to pass the LOB and Branch as  a query string then - use the following case
        // pass you raw - URL string.
        switch (ProgramCode)
        {
            //    // sample 
            //case strApplicationApproval:
            //    strRedirectPage = FunPriAddLOBBranchQueryString("~/Origination/S3GORGApplicationApproval_Add.aspx");
            //    break;
            default:
                break;
        }
    }

    protected void txtStartDateSearch_TextChanged(object sender, EventArgs e)
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        switch (ProgramCode)
        {
            //case strNocTermination:
            //    FunPriLoadCombo_NocTerminationNumbers();
            //    break;
            //case strOperatingLeaseExpenses:
            //    FunPriLoadCombo_OperatingLeaseExpensesNumbers();
            //    break;
            //case strLeaseAssetSaleApproval:
            //    FunPriLoadCombo_LeaseAssetSaleApproval();
            //    break;
            //case strTopupApproval:
            //    FunPriLoadCombo_TopUpApproval();
            //    break;
            //case strConsolidationApproval:
            //    FunPriLoadCombo_ConsolidationApproval();
            //    break;
            //case strSplitApproval:
            //    FunPriLoadCombo_SplitApproval();
            //    break;
            //case strManualJournalApproval:
            //    FunPriLoadCombo_ManualJournalApproval();
            //    break;
            //case strAccountclosureApproval:
            //    FunPriLoadCombo_AccountClosureApproval();
            //    break;
            //case strPrematureClosureApproval:
            //    FunPriLoadCombo_PrematureClosureApproval();
            //    break;
            //case strPaymentApproval:
            //    FunPriLoadCombo_PaymentRequestApproval();
            //    break;
            //case strSpecificRevisionApproval:
            //    FunPriLoadCombo_RevisionSpecificApproval();
            //    break;
            //case strPaymentRequest:
            //    FunPriLoadCombo_PaymentRequestNumber();
            //    break;
            //case strLeaseAssetSale:
            //    FunPriLoadCombo_LASNUM();
            //    break;
            //case strOperaingDepreciation:
            //    FunPriLoadCombo_DepreciatedSJVNumbers();
            //    break;
            default:
                FunPriLoadDNCCombo();
                break;
        }

    }
    protected void txtEndDateSearch_TextChanged(object sender, EventArgs e)
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        switch (ProgramCode)
        {
            //case strNocTermination:
            //    FunPriLoadCombo_NocTerminationNumbers();
            //    break;
            //case strOperatingLeaseExpenses:
            //    FunPriLoadCombo_OperatingLeaseExpensesNumbers();
            //    break;
            //case strInvoiceVendor:
            //    FunPriLoadCombo_InvoiceVendorNo();
            //    break;
            //case strLeaseAssetSaleApproval:
            //    FunPriLoadCombo_LeaseAssetSaleApproval();
            //    break;
            //case strTopupApproval:
            //    FunPriLoadCombo_TopUpApproval();
            //    break;
            //case strConsolidationApproval:
            //    FunPriLoadCombo_ConsolidationApproval();
            //    break;
            //case strSplitApproval:
            //    FunPriLoadCombo_SplitApproval();
            //    break;
            //case strManualJournalApproval:
            //    FunPriLoadCombo_ManualJournalApproval();
            //    break;
            //case strAccountclosureApproval:
            //    FunPriLoadCombo_AccountClosureApproval();
            //    break;
            //case strPrematureClosureApproval:
            //    FunPriLoadCombo_PrematureClosureApproval();
            //    break;
            //case strPaymentApproval:
            //    FunPriLoadCombo_PaymentRequestApproval();
            //    break;
            //case strSpecificRevisionApproval:
            //    FunPriLoadCombo_RevisionSpecificApproval();
            //    break;
            //case strPaymentRequest:
            //    FunPriLoadCombo_PaymentRequestNumber();
            //    break;
            //case strLeaseAssetSale:
            //    FunPriLoadCombo_LASNUM();
            //    break;
            //case strOperaingDepreciation:
            //    FunPriLoadCombo_DepreciatedSJVNumbers();
            //    break;
            default:
                FunPriLoadDNCCombo();
                break;
        }
    }

    //Add by chandru on 23/04/2012
    #region CommonWebmethod
    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetCommonList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] != null)
        {
            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
                Procparam.Add("@LOB_ID", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
        }
        if (System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"] != null)
        {
            if (System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"].ToString() != "0")
                Procparam.Add("@Location_ID", System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"].ToString());
        }
        if (System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"] != null)
        {
            Procparam.Add("@StartDate", System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"].ToString());
        }
        if (System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"] != null)
        {
            Procparam.Add("@EndDate", System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"].ToString());
        }
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
        Procparam.Add("@PrefixText", prefixText);
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strPDTransaction)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_TransLander_DDL_PDDTransaction_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAssetVerification)
        {
            //if (System.Web.HttpContext.Current.Session["UTPA"].ToString().ToUpper() == "UTPA")
            if (userUTPA == "UTPA")
            {
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LA_GetAssetVerification_For_UTPA", Procparam));
            }
            else
            {
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAssetsVerified_AGT", Procparam));
            }
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAccountActivation)
        {
            if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "")
                //Procparam.Add("@CustomerID", System.Web.HttpContext.Current.Session["CustomerAutoSuggestValue"].ToString());
                Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAccountsActivated_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAssetIdentification)
        {
            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestText"].ToString() == "OPERAT")
                Procparam.Add("@Is_OL", "1");
            else
                Procparam.Add("@Is_OL", "0");

            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAssetsIdentified_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strPaymentRequest)
        {
            Procparam.Add("@Program_Id", "54");
            Procparam.Remove("@User_ID");
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPaymentRequestNoCombo_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strDeliveryIns)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetDeliveryInsLPO_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strLeaseAssetSale)
        {
            //if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "") //By Siva.K on 17JUN2015 Include Lessee
            //    Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetLASNUMTransLand_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strLeaseAssetSaleApproval)
        {
            Procparam.Add("@Task_Type", "12");
            //if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "") //By Siva.K on 17JUN2015 Include Lessee
            //    Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            Ds = Utility.GetDataset("S3G_LOANAD_GetApprovalReqDetail_AGT", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[2]);
            Ds = null;
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strPaymentApproval)
        {
            Procparam.Add("@Task_Type", "10");
            Ds = Utility.GetDataset("S3G_LOANAD_GetApprovalReqDetail_AGT", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[2]);
            Ds = null;
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAccountClosure)
        {
            Procparam.Add("@Program_Id", "73");
            if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "") //By Siva.K on 17JUN2015 Include Lessee
                Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetClosedAccounts_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strTopupApproval)
        {
            Procparam.Add("@Task_Type", "14");
            Ds = Utility.GetDataset("S3G_LOANAD_GetApprovalReqDetail_AGT", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[2]);
            Ds = null;
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strTLEWC)
        {
            Procparam.Remove("@StartDate");
            Procparam.Remove("@EndDate");
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetTLEWC_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strTLGL)
        {
            Procparam.Remove("@StartDate");
            Procparam.Remove("@EndDate");
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetTLEWC_GL_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strPreMatureClosure)
        {
            Procparam.Add("@Program_Id", "85");
            if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "") //By Siva.K on 17JUN2015 Include Lessee
                Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPreClosedAccounts_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strPrematureClosureApproval)
        {
            Procparam.Add("@Task_Type", "13");
            if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "") //By Siva.K on 17JUN2015 Include Lessee
                Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            Ds = Utility.GetDataset("S3G_LOANAD_GetApprovalReqDetail_AGT", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[2]);
            Ds = null;
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAccountclosureApproval)
        {
            Procparam.Add("@Task_Type", "15");
            if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "") //By Siva.K on 17JUN2015 Include Lessee
                Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            Ds = Utility.GetDataset("S3G_LOANAD_GetApprovalReqDetail_AGT", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[2]);
            Ds = null;
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strNocTermination)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetNocTerminationID_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strBulkRevision)
        {
            Procparam.Remove("@StartDate");
            Procparam.Remove("@EndDate");
            Procparam.Remove("@User_ID");
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetBulkRevisionNo_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAssetAcquisition)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAssetAcquisitionID_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strOperaingDepreciation)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetDepreciatedSJV_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strInvoiceVendor)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetInvoiceVendorNo_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strOperatingLeaseExpenses)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetOLENUMTransLand_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strSpecificRevision)
        {
            Procparam.Remove("@StartDate");
            Procparam.Remove("@EndDate");
            Procparam.Remove("@User_ID");
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetSpecificRevisionNo_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strSpecificRevisionApproval)
        {
            Procparam.Add("@Task_Type", "28");
            Ds = Utility.GetDataset("S3G_LOANAD_GetApprovalReqDetail_AGT", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[2]);
            Ds = null;
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strManualJournalApproval)
        {
            Procparam.Add("@Task_Type", "29");
            Ds = Utility.GetDataset("S3G_LOANAD_GetApprovalReqDetail_AGT", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[2]);
            Ds = null;
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strMJV)
        {
            if (strRbtnJournal)
            {
                Procparam.Add("@Journal_Type", "SYS");
            }
            else
            {
                Procparam.Add("@Journal_Type", "MANUAL");
            }

            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetMJV_List_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strIncomeRecognition)
        {
            Procparam.Remove("@LOB_ID");
            Procparam.Remove("@Location_ID");
            Procparam.Remove("@User_ID");
            Procparam.Remove("@StartDate");
            Procparam.Remove("@EndDate");
            Procparam.Add("@LookupType_Code", "8");
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetLookUpValues_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAccountCreation)
        {

            if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "")
                //Procparam.Add("@CustomerID", System.Web.HttpContext.Current.Session["CustomerAutoSuggestValue"].ToString());
                Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAccountsCreated_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAccountCreationBW)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAccountsCreated_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strFactoringInvoice)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_TransLander_FactoringInvoice_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strFactoringInvoiceRetirement)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_TransLander_FactoringInvoice_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAccountSplit || System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strSplitApproval)
        {
            if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "")
                Procparam.Add("@CustomerID", obj_Page.hdnLessee.Value);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_TransLander_AccountSplit_AGT", Procparam));
        }
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strAccountConsolidation)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_TransLander_AccountConsolidation_AGT", Procparam));
        }
        //Added for Loan End Use Approval
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strLoanEndUseApproval)
        {
            Procparam.Add("@Program_Id", "260");
            Procparam.Remove("@User_ID");
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetLoanEndUseNoCombo_AGT", Procparam));
        }

        //Added by vinodha for Credit Debit note
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strCRDRNote)
        {
            Procparam.Add("@Task_Type", "60");
            Procparam.Add("@Program_Id", "298");
            Ds = Utility.GetDataset("S3G_DCN_GET_DOCDET", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[0]);
            Ds = null;
        }

        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strCRDRNoteApproval)
        {
            Procparam.Add("@Task_Type", "60");
            Procparam.Add("@Program_Id", "300");
            Ds = Utility.GetDataset("S3G_DCN_GET_DOCDET", Procparam);
            suggetions = Utility.GetSuggestions(Ds.Tables[0]);
            Ds = null;
        }

        return suggetions.ToArray();
    }
    protected void txtDocumentNumberSearch_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnCommonID.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtDocumentNumberSearch.Text = string.Empty;
                hdnCommonID.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    protected void txtRSNumber_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnRSNumber.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtRSNumber.Text = string.Empty;
                hdnRSNumber.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    // Created By: Anbuvel
    // Created Date: 09-01-2012
    // Descrition: To Bind Location Value

    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@Program_ID", System.Web.HttpContext.Current.Session["ProgramId"].ToString());
        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] != null)
        {
            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
                Procparam.Add("@LOB_ID", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
            else
                Procparam.Add("@LOB_ID", "0");
        }
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));
        return suggetions.ToArray();
    }
    protected void txtBranchSearch_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnBranchID.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtBranchSearch.Text = string.Empty;
                hdnBranchID.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander - Loan Admin");
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLesseeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        //START By Siva.K 24JUN2015 Bring Lessee,SUNDRY CREDITORS,GENERAL,ENTITY For DebitCreditNote
        string strSp_Name = "S3G_GETCustomers";
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strCRDRNote || System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strCRDRNoteApproval
            || System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strPaymentRequest || System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strPaymentApproval)
            strSp_Name = "S3G_LAD_GET_ALL_CUSTENTITY_DTLS";
        else if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strOverDueInterest)
            strSp_Name = "S3G_LoanAd_GetCustomer_AGT";
        else if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strLeaseAssetSale || System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strLeaseAssetSaleApproval)
        {
            Procparam.Add("@Program_Id", "62");
            strSp_Name = "S3G_LoanAd_GetCustomer_AGT";
        }
            //END By Siva.K 24JUN2015 
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(strSp_Name, Procparam));
        return suggetions.ToArray();
    }


    // Created By: Siva.K
    // Created Date: 22-05-2015
    // Descrition: To Bind Lessee Value
    protected void txtLessee_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnLessee.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtLessee.Text = string.Empty;
                hdnLessee.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
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

    protected void txtODIInvoice_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnODIInvoice.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtODIInvoice.Text = string.Empty;
                hdnODIInvoice.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    protected void txtDCNInvoice_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnDCNInvoice.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtDCNInvoice.Text = string.Empty;
                hdnDCNInvoice.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    protected void txtOldInvoice_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnOldInvoice.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtOldInvoice.Text = string.Empty;
                hdnOldInvoice.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    #endregion

    // Added on 19Jun215 starts here
    protected void txtTrancheName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnTranche.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtTrancheName.Text = string.Empty;
                hdnTranche.Value = string.Empty;
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

        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strOverDueInterest)
            Procparam.Add("@Option", "6");
        else if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strCRDRNote)
            Procparam.Add("@Option", "7");
        else if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strPreMatureClosure)
            Procparam.Add("@Option", "10");
        else if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strLeaseAssetSale)
            Procparam.Add("@Option", "11");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetOldInvoiceNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        if (System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"].ToString() == strCRDRNote)
            Procparam.Add("@Option", "16");

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        //if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "")
        //    Procparam.Add("@Customer_ID", obj_Page.hdnLessee.Value);
        Procparam.Add("@Option", "3");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetRSList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        //if (obj_Page.hdnLessee.Value != "0" && obj_Page.txtLessee.Text != "")
        //    Procparam.Add("@Customer_ID", obj_Page.hdnLessee.Value);
        Procparam.Add("@Option", "4");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    // Added on 19Jun215 Ends here
}
#endregion
