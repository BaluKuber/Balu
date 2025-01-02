

#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Orgination
/// Screen Name         :   Transaction Lander
/// Created By          :   S.Kannan
/// Created Date        :   30-July-2010
/// Purpose             :   This is the landing screen for all the other Orgination Screens
/// Last Updated By		:   Chandra Sekhar BS
/// Last Updated Date   :   17-Sep-2013
/// Reason              :   Auto Suggest 
/// <Program Summary>
#endregion

#region How to use this
/*
   1)	Search for the word "Add Here" and add your code respectively  there...
   2)   Use the same stored procedure S3G_ORG_Get_TransLander, the return table should have the first column named "ID" - to use it as the RowCommandValue
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
using System.Diagnostics;
using S3GBusEntity.Origination;
using S3GBusEntity;

#endregion

#region Class Origination_S3GORGTransLander
public partial class Origination_S3GORGTransLander : ApplyThemeForProject
{
    # region Programs Code
    string ProgramCodeToCompare = "";                                           // this is to hold the Program Code of your web page


    // Add here - Add your Program Code Here - refer to the SQL table Program Master.
    const string strFieldInformationReportCode = "FEIR";                        // Program Code for Field Information Report
    const string strLegalclearanceReportCode = "LCLRR";                        // Program Code for Legal Clearance Report
    const string strTechclearanceReportCode = "TCLRR";                         // Program code for technical clearance report
    const string strSanctionNumberCode = "SNQ";                                 // Program Code for Sanction Number
    const string strCreditParameterTransactionCode = "CRPT";                    // Program Code for Credit Parameter Transaction
    const string strProformaReportCode = "PROF";                               // Program Code for proforma Report
    const string strEnquiryAssignment = "ENA";                                  // Program Code for Enquiry Assignment
    const string strCreditParameterApproval = "CPA";                                  // Program Code for Enquiry Assignment
    const string strEnquiryCustomerAppraisal = "ENCA";
    const string strPRDDTransaction = "PRT";
    const string strPricingApprovalReportCode = "PRAP";
    const string strApplicationApproval = "APAP";                           //Program Code for Application Approval
    const string strEnquiryResponse = "ENRES";                          // Program Code for Enquiry Response
    const string strApplicationProcess = "APPP";// Program Code for Application Processing
    const string strApplicationProcessBW = "BWAPP";  // Program Code for Application Processing BellWether
    const string strCreditGuideTransaction = "OCGT";                          // Program Code for Crdit Guide Transaction
    const string strPricing = "ORPRC";
    const string strPricingBW = "BWPRC";
    const string strTargetMaster = "TRG";                                      //Program code for Target Master
    const string strApplicationProcessGL = "APG";                                 //Program code for Application Processing - Gold loan
    const string strCRM = "CRM";//program code for CRM
    public string strProgramId = "0";
    const string strMRA = "MRA";                                                //program code for MRA Creation
    const string strMRAApproval = "MAPP";                                        //program code for MRA Approval
    const string strPurchaseOrder = "PURORD";                                        //program code for Proforma Invoice
    const string strProformaInvoice = "PRFINV";
    const string strVendorInvoice = "VNINV";
    string strPO_Type = "0";
    // Program Code for Enquiry Customer Appraisal
    #endregion

    #region Common Variables
    int intCreate = -1;                                                         // intCreate = 1 then display the create button, else invisible
    int intQuery = -1;                                                          // intQuery = 1 then display the Query button, else invisible
    int intModify = -1;                                                         // intModify = 1 then display the Modify button, else invisible
    int intMultipleDNC = -1;                                                    // Allow the user to select the DNC dynamically.
    int intDNCOption = -1;                                                      // Allow the user to select the further option depend on the DNC - eg: approved,unapproved etc...
    Dictionary<int, string> dictMultipleDNC = null;                             // collection for Multiple DNC - DDL
    Dictionary<int, string> dictDNCOption = null;                               // Collection for DNCOption.
    string strProcName = "S3G_ORG_Get_TransLander";                             // this is the Stored procedure to get call                     
    string ProgramCode = "";                                                         // To maintain the ProgramID
    UserInfo ObjUserInfo;                                                       // to maintain the user information      
    public string strDateFormat;                                                // to maintain the standard format
    string strRedirectPage = "";                                                // page to redirect to the page in query mode
    string strRedirectCreatePage = "";                                          // page to redirect to the page in Create mode
    Dictionary<string, string> Procparam = null;                                // Dictionary to send our procedure's Parameters
    int intUserID = 0;                                                          // user who signed in
    int intCompanyID = 0;                                                       // conpany of the user who signed in   
    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    bool isQueryColumnVisible;                                                  // To change the Query Column visibility - depend on the user autherization 
    bool isEditColumnVisible;                                                  // To change the Edit Column visibility - depend on the user autherization 
    static string strPageName = "Origination TransLander";
    public static Origination_S3GORGTransLander ojb_TransLander = null;

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
        ojb_TransLander = this;

        #region Application Standard Date Format
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;                              // to get the standard date format of the Application
        CalendarExtenderEndDateSearch.Format = strDateFormat;                       // assigning the first textbox with the End date
        CalendarExtenderStartDateSearch.Format = strDateFormat;                     // assigning the first textbox with the start date
        #endregion

        #region Common Session Values


        if (ComboBoxLOBSearch.SelectedValue != string.Empty)
        {
            System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = ComboBoxLOBSearch.SelectedValue;
            System.Web.HttpContext.Current.Session["LOBAutoSuggestText"] = ComboBoxLOBSearch.SelectedItem.Text;
        }
        else
        {
            System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = null;
        }
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
        System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyID.ToString();
        System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = intUserID.ToString();
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
            btnCreate.Text = "Create";
            FunPriGetQueryStrings();
            InitPage();
            System.Web.HttpContext.Current.Session["ProgramId"] = strProgramId;
            //FunPriUIVisibility();

            switch (ProgramCode)
            {
                case strFieldInformationReportCode:
                    if (ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "UTPA")
                    {
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
                case strLegalclearanceReportCode:
                    if (ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "UTPA")
                    {
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
                case strTechclearanceReportCode:
                    if (ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "UTPA")
                    {
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

            if (ViewState["ProgramCode"] == null)                                   // Added viewstate for the program code - to refresh the page - when the query string of the URL varys. - It will assign the program code to the view state - for the very first time the page loads.
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
            ViewState["ProgramCode"] = ProgramCode;
            IsQueryStringChanged = false;
            //txtEndDateSearch.Attributes.Add("readonly", "readonly");                // making the end date textbox readonly
            //txtStartDateSearch.Attributes.Add("readonly", "readonly");              // making the start date textbox readonly
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
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

            #region  User Authorization
            if (btnCreate.Enabled)                                                  // if the user can view the create button - depends on the query string
            {
                //if (!bIsActive)                                                     // checking the authorization
                //{
                //    btnCreate.Enabled = false;
                //}
                //else if (!bCreate)
                //{
                //    btnCreate.Enabled = false;
                //}

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
                }
                if (!bQuery)
                {
                    grvTransLander.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                }
                //Authorization Code end

            }
            #endregion

            CalendarExtenderEndDateSearch.Enabled =
                    imgEndDateSearch.Visible =
                    CalendarExtenderStartDateSearch.Enabled =
                    imgStartDateSearch.Visible = true;

            txtEndDateSearch.Text = txtStartDateSearch.Text = "";

            //if (ProgramCode == "PURORD")
            //{
            //    //lblLOBSearch.Visible = ComboBoxLOBSearch.Visible = false;
            //    lblBranchSearch.Text = "Load Sequence No.";
            //}
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
    /// To read the values from the querystring
    /// </summary>
    private void FunPriGetQueryStrings()
    {
        if (Request.QueryString.Get("Code") != null)
            ProgramCode = (Request.QueryString.Get("Code"));
        if (Request.QueryString.Get("Create") != null)
            intCreate = Convert.ToInt32(Request.QueryString.Get("Create"));
        if (Request.QueryString.Get("Query") != null)
            intQuery = Convert.ToInt32(Request.QueryString.Get("Query"));
        if (Request.QueryString.Get("Modify") != null)
            intModify = Convert.ToInt32(Request.QueryString.Get("Modify"));
        if (Request.QueryString.Get("MultipleDNC") != null)
            intMultipleDNC = Convert.ToInt32(Request.QueryString.Get("MultipleDNC"));
        if (Request.QueryString.Get("DNCOption") != null)
            intDNCOption = Convert.ToInt32(Request.QueryString.Get("DNCOption"));
    }


    /// <summary>
    /// This is an optional dropdown box - if the user want to 
    /// display multiple DNC - then he can make use of this method.
    /// </summary>
    private void FunPriLoadMultiDNCCombo()
    {
        try
        {

            if (intMultipleDNC == 1)
            {
                FunPriMakeMultipleDNCVisible(lblMultipleDNC, ddlMultipleDNC, true);
                //lblProgramIDSearch.Visible = ddlDocumentNumb.Visible = false;

                // Add here case statement here - to load the Multiple DNC DropDown.
                switch (ProgramCode)
                {
                    case strEnquiryCustomerAppraisal:
                    case strCreditParameterTransactionCode:
                    case strCreditParameterApproval:
                        FunpriBindMultipleDNC(new string[] { "-- Select --", "Enquiry", "Pricing", "Application", "Customer" }, ddlMultipleDNC);
                        break;
                    case strCreditGuideTransaction:
                        FunpriBindMultipleDNC(new string[] { "-- Select --", "Enquiry", "Pricing", "Application", "Customer" }, ddlMultipleDNC);
                        break;
                }
            }
            else
            {
                //if (ProgramCode == strCRM)
                //    lblProgramIDSearch.Visible = ddlDocumentNumb.Visible = false;
                //else
                //    lblProgramIDSearch.Visible = ddlDocumentNumb.Visible = true;
                FunPriMakeMultipleDNCVisible(lblMultipleDNC, ddlMultipleDNC, false);
            }
            /*if ((intMultipleDNC == -1) && (string.Compare(ProgramCode, strEnquiryResponse) == 0))
            {

                FunPriMakeMultipleDNCVisible(lblMultipleDNC, ddlMultipleDNC, false);

                lblProgramIDSearch.Visible = ddlDocumentNumb.Visible = true;
            }*/


            if (intDNCOption == 1)
            {
                dictDNCOption = new Dictionary<int, string>();
                // Add here case statement here - to load the Multiple DNC option.
                switch (ProgramCode)
                {
                    case strCreditParameterTransactionCode:
                        FunpriBindMultipleDNC(new string[] { "-- Select --", "Approved", "Unapproved" }, ddlDNCOption);
                        break;
                    case strCreditParameterApproval:
                        ddlDNCOption.AutoPostBack = true;
                        FunpriBindMultipleDNC(new string[] { "-- Select --", "Unapproved", "All" }, ddlDNCOption);
                        break;
                }

                FunPriMakeMultipleDNCVisible(lblDNCOption, ddlDNCOption, true);
            }
            else
            {
                FunPriMakeMultipleDNCVisible(lblDNCOption, ddlDNCOption, false);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
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
        try
        {

            //if (intMultipleDNC != 1)
            //{
            lblBranchSearch.Text = "Location";
            txtBranchSearch.Enabled =

ddlDNCOption.Enabled =
ddlDocumentNumb.Enabled = true;
            switch (ProgramCode)
            {
                case strFieldInformationReportCode:
                    strProgramId = "33";
                    FunPubIsMandatory(false, false, false, false);
                    ProgramCodeToCompare = strFieldInformationReportCode;
                    lblHeading.Text = "FIR Transaction";
                    this.Title = "S3G - FIR Number";
                    lblProgramIDSearch.Text = "FIR Number";                                         // This is to display on the Document Number Label                
                    strRedirectPage = "~/Origination/S3GOrgFieldInformationReport_Add.aspx";        // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode
                    break;
                case strLegalclearanceReportCode:
                    strProgramId = "273";
                    FunPubIsMandatory(false, false, false, false);
                    ProgramCodeToCompare = strLegalclearanceReportCode;
                    lblHeading.Text = "Legal Clearance";
                    this.Title = "S3G - Legal Clearance Number";
                    lblProgramIDSearch.Text = "Legal Clearance Number";                                         // This is to display on the Document Number Label                
                    strRedirectPage = "~/Origination/S3G_ORG_Legal_Clearance.aspx";        // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode
                    break;
                case strTechclearanceReportCode:
                    strProgramId = "274";
                    FunPubIsMandatory(false, false, false, false);
                    ProgramCodeToCompare = strTechclearanceReportCode;
                    lblHeading.Text = "Tech Clearance";
                    this.Title = "S3G - Tech Clearance Number";
                    lblProgramIDSearch.Text = "Tech Clearance Number";                                         // This is to display on the Document Number Label                
                    strRedirectPage = "~/Origination/S3G_ORG_Tech_Clearance.aspx";        // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode
                    break;
                case strSanctionNumberCode:
                    strProgramId = "36";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Sanction Number Query";
                    ProgramCodeToCompare = strSanctionNumberCode;
                    lblHeading.Text = "Sanction Number Query - Details";
                    lblProgramIDSearch.Text = "Sanction Number";                                    // This is to display on the Document Number Label            
                    strRedirectPage = "~/Origination/S3G_ORG_SanctionNumberQuery.aspx";             // page to redirect to the page in edit mode
                    break;
                //Code end
                case strProformaReportCode:
                    strProgramId = "44";
                    FunPubIsMandatory(false, false, false, false);
                    ProgramCodeToCompare = strProformaReportCode;
                    this.Title = "S3G - Proforma Number";
                    lblProgramIDSearch.Text = "Proforma Number";                                    // This is to display on the Document Number Label
                    lblHeading.Text = "Proforma - Details";
                    strRedirectPage = "~/Origination/S3GORGProforma_Add.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage;
                    break;

                case strPRDDTransaction:
                    //ProgramCode = "PRT";
                    strProgramId = "41";
                    FunPubIsMandatory(false, false, false, false);
                    ProgramCodeToCompare = strPRDDTransaction;
                    this.Title = "S3G - PRDDT Number";
                    lblProgramIDSearch.Text = "PRDDT Number";                                    // This is to display on the Document Number Label
                    RFVComboLOB.Visible = false;
                    lblHeading.Text = "Pre Disbursement Documents Transaction ";
                    RFVComboBranch.Visible = false;
                    strRedirectPage = "~/Origination/S3GOrgPDDTMaster_Add.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage;
                    break;

                case strCreditParameterTransactionCode:
                    //ProgramCode = "CRPT";
                    strProgramId = "35";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Credit Parameter Transaction";
                    ProgramCodeToCompare = strCreditParameterTransactionCode;
                    lblMultipleDNC.Text = "Document Type";
                    lblProgramIDSearch.Text = "Document Number";
                    lblHeading.Text = "Credit Parameter Transaction - Details";
                    //lblProgramIDSearch.Text = "Credit Parameter Transaction No.";
                    strRedirectPage = "~/Origination/S3G_ORG_CreditParameterTransaction.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = "~/Origination/S3G_ORG_CreditParameterTransaction.aspx?qsMode=C";
                    break;

                case strEnquiryAssignment:
                    strProgramId = "34";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Enquiry Assignment";
                    ProgramCodeToCompare = strEnquiryAssignment;
                    lblProgramIDSearch.Text = "Enquiry Number";
                    lblHeading.Text = "Enquiry Assignment - Details";
                    strRedirectPage = "~/Origination/S3G_ORG_EnquiryAssignment.aspx";
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    break;
                case strCreditParameterApproval:
                    this.Title = "S3G - Credit Parameter Approval";
                    FunPubIsMandatory(false, false, false, false);
                    lblHeading.Text = "Credit Parameter Approval - Details";
                    //lblProgramIDSearch.Text = "Credit Parameter Approval Number";
                    strProgramId = "51";
                    lblMultipleDNC.Text = "Document Type";
                    lblProgramIDSearch.Text = "Document Number";
                    ProgramCodeToCompare = strCreditParameterApproval;
                    strRedirectPage = "~/Origination/S3GOrgCreditParameterApproval.aspx";
                    strRedirectCreatePage = "~/Origination/S3GOrgCreditParameterApproval.aspx?qsMode=C";
                    // strRedirectCreatePage = strRedirectPage + "?qsMode=C" + "&qsViewId=" + FormsAuthentication.Encrypt(TicketEnquiryNo);                    
                    break;
                case strEnquiryCustomerAppraisal:
                    strProgramId = "47";
                    this.Title = "S3G - Enquiry / Customer Appraisal";
                    ProgramCodeToCompare = strEnquiryCustomerAppraisal;
                    lblHeading.Text = "Enquiry / Customer Appraisal - Details";
                    FunPubIsMandatory(false, false, false, false);

                    lblMultipleDNC.Text = "Document Type";
                    lblProgramIDSearch.Text = "Document Number";

                    //lblProgramIDSearch.Text = "Enquiry/Customer";

                    strRedirectPage = "~/Origination/S3GORGEnquiryAppraisal_Add.aspx?qsTransactionType=" + ddlMultipleDNC.SelectedValue;
                    //strRedirectCreatePage = "~/Origination/S3GORGEnquiryAppraisal_View.aspx?qsMode=C&qsTransactionType=" + ddlMultipleDNC.SelectedValue;
                    strRedirectCreatePage = "~/Origination/S3GORGEnquiryAppraisal_Add.aspx?qsMode=C"; //&qsTransactionType=" + ddlMultipleDNC.SelectedValue;
                    break;
                case strPricingApprovalReportCode:
                    strProgramId = "31";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Pricing Approval";
                    ProgramCodeToCompare = strPricingApprovalReportCode;
                    lblHeading.Text = "Pricing Approval - Details";
                    lblProgramIDSearch.Text = "Business Offer Number";                                    // This is to display on the Document Number Label
                    lblHeading.Text = "Pricing Approval - Details";
                    strRedirectPage = "~/Origination/S3GORGPricingApproval_Add.aspx?qsMode=C";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage;
                    btnCreate.Text = "Approve";
                    lblCustomerName.Visible = true;
                    ddlCustomerName.Visible = true;
                    break;
                case strApplicationApproval:
                    strProgramId = "37";
                    FunPubIsMandatory(false, false, false, false);
                    ProgramCodeToCompare = strApplicationApproval;
                    this.Title = "S3G - Application Number";
                    lblHeading.Text = "Application Approval - Details";
                    lblProgramIDSearch.Text = "Application Number";
                    strRedirectPage = "~/Origination/S3GORGApplicationApproval_Add.aspx";
                    strRedirectCreatePage = "~/Origination/S3GORGApplicationApproval_Add.aspx?qsMode=C";
                    FunPriQueryString();
                    btnCreate.Text = "Approve";
                    break;

                case strEnquiryResponse:
                    strProgramId = "46";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Enquiry Response";
                    ProgramCodeToCompare = strEnquiryResponse;
                    lblProgramIDSearch.Text = "Enquiry Number";
                    lblHeading.Text = "Enquiry Response - Details";
                    //lblProgramIDSearch.Visible = false;
                    ddlDocumentNumb.Visible = true;
                    strRedirectPage = "~/Origination/S3GORGEnquiryResponse.aspx";                      // page to redirect to the page in edit mode
                    //strRedirectCreatePage = strRedirectPage;
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";

                    break;

                case strApplicationProcess:
                    strProgramId = "38";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Application Processing";
                    ProgramCodeToCompare = strApplicationProcess;
                    lblHeading.Text = "Application Processing - Details";
                    lblProgramIDSearch.Text = "Application No.";
                    lblProgramIDSearch.Visible = true;
                    ddlDocumentNumb.Visible = true;
                    strRedirectPage = "~/Origination/S3G_ORG_ApplicationProcessing.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    break;
                case strApplicationProcessBW:
                    strProgramId = "252";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Application Processing";
                    ProgramCodeToCompare = strApplicationProcessBW;
                    lblHeading.Text = "Application Processing - Details";
                    lblProgramIDSearch.Text = "Application No.";
                    lblProgramIDSearch.Visible = true;
                    ddlDocumentNumb.Visible = true;
                    strRedirectPage = "~/Origination/S3G_ORG_ApplicationProcessingBW.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    break;
                case strApplicationProcessGL:
                    strProgramId = "216";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Application Processing";
                    ProgramCodeToCompare = strApplicationProcessGL;
                    lblHeading.Text = "Application Processing - Details";
                    lblProgramIDSearch.Text = "Account No.";
                    lblProgramIDSearch.Visible = true;
                    ddlDocumentNumb.Visible = true;
                    strRedirectPage = "~/Origination/S3G_ORG_ApplicationProcessingGL.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    break;

                case strCreditGuideTransaction:
                    //ProgramCode = "OCGT";
                    strProgramId = "43";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Credit Guide Transaction";
                    lblHeading.Text = "Credit Guide Transaction - Details";

                    //ddlDocumentNumb.Visible = false;
                    lblBranchSearch.Text = "Location";
                    RFVComboBranch.ErrorMessage = "Select Constitution";
                    ProgramCodeToCompare = strCreditGuideTransaction;
                    lblMultipleDNC.Text = "Document Type";
                    lblProgramIDSearch.Text = "Document Number";
                    strRedirectPage = "~/Origination/S3GORGCreditGuideTransaction_Add.aspx";                      // page to redirect to the page in edit mode
                    //strRedirectCreatePage = strRedirectPage;
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    break;

                case strPricing:
                    strProgramId = "42";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Proposal Number";
                    ProgramCodeToCompare = strPricing;
                    lblHeading.Text = "Proposal - Details";
                    lblProgramIDSearch.Text = "Proposal Number";
                    strRedirectPage = "~/Origination/S3G_ORG_Proposal.aspx";                      // page to redirect to the page in edit mode                
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    lblStartDateSearch.Text = "Proposal Period From";
                    lblEndDateSearch.Text = "Proposal Period To";
                    ddlDocumentNumb.Visible = true;
                    lblCustomerName.Visible = true;
                    ddlCustomerName.Visible = true;
                    break;

                //Added for BW by saran
                case strPricingBW:
                    strProgramId = "251";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Business Offer Number";
                    ProgramCodeToCompare = strPricingBW;
                    lblHeading.Text = "Pricing - Details";
                    lblProgramIDSearch.Text = "Pricing Offer Number";
                    strRedirectPage = "~/Origination/S3gOrgPricingBW_Add.aspx";                      // page to redirect to the page in edit mode                
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    ddlDocumentNumb.Visible = true;
                    break;

                case strTargetMaster:
                    strProgramId = "208";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Business Target Master";
                    ProgramCodeToCompare = strTargetMaster;
                    lblHeading.Text = "Target Master - Details";
                    lblProgramIDSearch.Text = "Business Target Number";
                    strRedirectPage = "~/Origination/S3GORGTargetMaster_Add.aspx";                      // page to redirect to the page in edit mode                
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    ddlDocumentNumb.Visible = true;
                    break;

                case strCRM:
                    strProgramId = "241";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G-CRM";
                    ProgramCodeToCompare = strCRM;
                    lblHeading.Text = "CRM-Details";
                    lblProgramIDSearch.Text = "";
                    strRedirectPage = "~/Origination/S3GOrgCRM.aspx";
                    strRedirectCreatePage = strRedirectPage;// +"?qsMode=C";
                    ddlDocumentNumb.Visible = false;
                    FunPriMakeMultipleDNCVisible(lblDNCOption, ddlDNCOption, false);
                    ComboBoxLOBSearch.Enabled = txtBranchSearch.Enabled = ddlDNCOption.Enabled = ddlDocumentNumb.Enabled = false;
                    lblProgramIDSearch.Visible =
                    ddlDocumentNumb.Visible = false;
                    break;

                //Added on 17Sep2014 starts here
                case strMRA:
                    strProgramId = "281";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - MRA";
                    ProgramCodeToCompare = strMRA;
                    lblHeading.Text = "MRA Creation- Details";
                    lblProgramIDSearch.Text = "Lessee Name";
                    strRedirectPage = "~/Origination/S3G_ORG_MRA_ADD.aspx";                      // page to redirect to the page in edit mode                
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    ddlDocumentNumb.Visible = true;
                    txtBranchSearch.Visible = lblBranchSearch.Visible = false;
                    //lblBranchSearch.Text = "MRA Number";
                    break;
                //Added on 17Sep2014 Ends here

                //Added on 19Sep2014 starts here
                case strMRAApproval:
                    strProgramId = "282";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - MRA Approval";
                    ProgramCodeToCompare = strMRAApproval;
                    lblHeading.Text = "MRA Approval- Details";
                    lblProgramIDSearch.Text = "Lessee Name";
                    strRedirectPage = "~/Origination/S3G_ORG_MRA_Approval.aspx";                      // page to redirect to the page in edit mode                
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    ddlDocumentNumb.Visible = true;
                    txtBranchSearch.Visible = lblBranchSearch.Visible = false;
                    //lblBranchSearch.Text = "MRA Number";
                    lblStartDateSearch.Text = "MRA Period From";
                    lblEndDateSearch.Text = "MRA Period To";
                    btnCreate.Text = "Approve";
                    break;
                //Added on 19Sep2014 Ends here
                case strPurchaseOrder:
                    strProgramId = "278";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Purchase Order";
                    ProgramCodeToCompare = strPurchaseOrder;
                    lblHeading.Text = "Purchase Order - Details";
                    lblBranchSearch.Text = "Load Sequence No.";
                    lblProgramIDSearch.Text = "Purchase Order No.";
                    lblProgramIDSearch.Visible = true;
                    ddlDocumentNumb.Visible = true;
                    ComboBoxLOBSearch.Enabled = false;
                    strRedirectPage = "~/Origination/S3G_ORG_PurchaseOrder_Add.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    lblCustomerName.Visible = true;
                    ddlCustomerName.Visible = true;
                    break;
                case strProformaInvoice:
                    strProgramId = "290";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Proforma Invoice";
                    ProgramCodeToCompare = strProformaInvoice;
                    lblHeading.Text = "Proforma Invoice - Details";
                    lblBranchSearch.Text = "Load Sequence No.";
                    lblProgramIDSearch.Text = "Proforma Invoice No.";
                    lblProgramIDSearch.Visible = true;
                    ddlDocumentNumb.Visible = true;
                    ComboBoxLOBSearch.Enabled = false;
                    strRedirectPage = "~/Origination/S3G_ORG_ProformaInvoice_Add.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    break;
                case strVendorInvoice:
                    strProgramId = "293";
                    FunPubIsMandatory(false, false, false, false);
                    this.Title = "S3G - Vendor Invoice";
                    ProgramCodeToCompare = strVendorInvoice;
                    lblHeading.Text = "Vendor Invoice - Details";
                    lblBranchSearch.Text = "Load Sequence No.";
                    lblProgramIDSearch.Text = "Vendor Invoice No.";
                    lblProgramIDSearch.Visible = true;
                    ddlDocumentNumb.Visible = true;
                    ComboBoxLOBSearch.Enabled = false;
                    strRedirectPage = "~/Origination/S3G_ORG_VendorInvoice_Add.aspx";                      // page to redirect to the page in edit mode
                    strRedirectCreatePage = strRedirectPage + "?qsMode=C";
                    lblCustomerName.Visible = ddlCustomerName.Visible = true; //By Siva.K 17JUN2015 Lessee
                    break;


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        // }
    }



    /// <summary>
    /// To set the LOB and Branch Mandatory/NonMandatory
    /// </summary>
    /// <param name="isLOBMandatory">true to set the LOB DDL Mandatory</param>
    /// <param name="isBranchMandatory">true to set the Branch DDL Mandatory</param>
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
    /// To Bind the Landing Grid
    /// </summary>
    /// <param name="intPageNum"> Current Page Number of the grid </param>
    /// <param name="intPageSize"> Current Page size of the grid </param>
    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;              // To set the page Number
            ProPageSizeRW = intPageSize;            // To set the page size    
            FunPriBindGrid();                       // Binding the Landing grid
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    /// <summary>
    /// This is tp load the combo(s) in the page.
    /// </summary>
    protected void FunProLoadCombos()
    {
        try
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
                        Procparam.Add("@User_ID", intUserID.ToString());
                        //Procparam.Add("@Program_Code", ProgramCode);
                        Procparam.Add("@Program_Id", strProgramId);
                        if (ProgramCode == strProformaReportCode)
                        {
                            Procparam.Add("@FilterType", "0");
                            //Procparam.Add("@FilterCode", "'OL','TL','TE','FT','WC'");
                            //code modified to get specific LOb for "Proforma" - Kuppusamy.B - 16-Feb-2012
                            //start
                            //Procparam.Add("@Is_Active", "1");
                            Procparam.Add("@FilterCode", "'FT','WC'");
                            //end

                            ComboBoxLOBSearch.BindDataTable(SPNames.S3G_ORG_GetSpecificLOBList, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                        }
                        else if (ProgramCode == strPurchaseOrder)
                        {
                            //Procparam.Add("@Is_Active", "1");
                            //Procparam.Add("@Is_Translander", "True");
                            ComboBoxLOBSearch.BindDataTable(SPNames.LOBMaster, Procparam, false, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                        }
                        else
                        {
                            //Procparam.Add("@Is_Active", "1");
                            //Procparam.Add("@Is_Translander", "True");
                            ComboBoxLOBSearch.BindDataTable(SPNames.LOBMaster, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                        }

                        // branch 
                        //Procparam.Clear();
                        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                        //Procparam.Add("@User_ID", Convert.ToString(intUserID));
                        ////Procparam.Add("@Is_Active", "1");
                        ////Procparam.Add("@Program_Code", ProgramCode);
                        //Procparam.Add("@Program_Id", strProgramId);
                        //Procparam.Add("@LOB_ID", ComboBoxBranchSearch.SelectedValue);
                        //ComboBoxBranchSearch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });
                    }
                    break;
            }
            // Loading Multiple DNC
            if (ComboBoxLOBSearch.Items.Count == 2)
            {
                ComboBoxLOBSearch.SelectedIndex = 1;
                ComboBoxLOBSearch.ClearDropDownList();
            }

            FunPriLoadMultiDNCCombo();

            //FunPriLoadDNCCombo();//Modified by saran to load combo at page loa
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    /// <summary>
    /// Will load the DNC Combo
    /// </summary>
    //private void FunPriLoadDNCCombo()
    //{
    //    try
    //    {
    //        if (Procparam != null)
    //            Procparam.Clear();
    //        else
    //            Procparam = new Dictionary<string, string>();

    //        // Add Here - your case statement blow if needed
    //        // Document Number Combo - Before add your code check if the common Function was available.       
    //        switch (ProgramCode)
    //        {
    //            case strFieldInformationReportCode:                         // Field Information Report                
    //                FunPriLoadCombo_FIRNo();
    //                break;
    //            case strSanctionNumberCode:                                 // Sanction Number
    //                FunPriLoadCombo_SanctionNo();
    //                break;
    //            case strProformaReportCode:                                 // Proforma
    //                FunPriLoadCombo_ProformaNo();
    //                break;
    //            case strCreditParameterTransactionCode:
    //                FunPriLoadCombo_CreditParameterTransaction();
    //                break;
    //            case strEnquiryAssignment:
    //                FunPriLoadCombo_AssignedEnquiry();                        // Loading the Enquiry Numbers to Assigned
    //                break;
    //            case strPRDDTransaction:
    //                FunPriLoadCombo_PRDDTNo();                // Loading the Appraised Enquiry No & Customer
    //                break;
    //            case strPricingApprovalReportCode:
    //                FunPriLoadCombo_PricingBusinessOfferNo();
    //                break;
    //            case strApplicationApproval:
    //                FunPriLoadCombo_ApplicationNumber();                     // Loading the Application Numbers 
    //                break;
    //            //case strCreditGuideTransaction:
    //            //   FunPriLoadComboCreditGuideTransaction();                     // Loading the Application Numbers 
    //            //   break;
    //            case strEnquiryResponse:                                    // Loading the Enquiry Reaponse Numbers 
    //                FunPriLoadCombo_EnquiryResponseNo();
    //                break;
    //            case strPricing:
    //                FunPriLoadCombo_PricingNo();                     // Loading the Application Numbers 
    //                break;
    //            //Added by saran for BW
    //            case strPricingBW:
    //                FunPriLoadCombo_PricingNo();                     // Loading the Application Numbers 
    //                break;

    //            case strApplicationProcess:                                    // Loading the Enquiry Reaponse Numbers 
    //            case strApplicationProcessBW:
    //                FunPriLoadCombo_ApplicationProcessNo();
    //                break;
    //            case strApplicationProcessGL:                                    // Loading the Application Number
    //                FunPriLoadCombo_ApplicationProcessGLNo();
    //                break;
    //            case strTargetMaster:
    //                FunPriLoadCombo_TargetNo();
    //                break;
    //            case strCRM:
    //                FunPriLoadCombo_TargetNo();
    //                break;
    //            default:
    //                // to do: disable the page 
    //                break;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    //private void FunPriLoadDNCCombo()
    //{
    //    try
    //    {
    //        if (Procparam != null)
    //            Procparam.Clear();
    //        else
    //            Procparam = new Dictionary<string, string>();

    //        // Add Here - your case statement blow if needed
    //        // Document Number Combo - Before add your code check if the common Function was available.       
    //        switch (ProgramCode)
    //        {
    //            case strFieldInformationReportCode:                         // Field Information Report                
    //                break;
    //            case strSanctionNumberCode:                                 // Sanction Number
    //                break;
    //            case strProformaReportCode:                                 // Proforma
    //                break;
    //            case strCreditParameterTransactionCode:
    //                break;
    //            case strEnquiryAssignment:
    //                break;
    //            case strPRDDTransaction:
    //                break;
    //            case strPricingApprovalReportCode:
    //                break;
    //            case strApplicationApproval:
    //                break;
    //            //case strCreditGuideTransaction:
    //            //   FunPriLoadComboCreditGuideTransaction();                     // Loading the Application Numbers 
    //            //   break;
    //            case strEnquiryResponse:                                    // Loading the Enquiry Reaponse Numbers 
    //                break;
    //            case strPricing:
    //                break;
    //            //Added by saran for BW
    //            case strPricingBW:
    //                break;

    //            case strApplicationProcess:                                    // Loading the Enquiry Reaponse Numbers 
    //            case strApplicationProcessBW:
    //                break;
    //            case strApplicationProcessGL:                                    // Loading the Application Number
    //                break;
    //            case strTargetMaster:
    //                break;
    //            case strCRM:
    //                break;
    //            default:
    //                // to do: disable the page 
    //                break;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    //private void FunPriLoadCombo_ApplicationProcessNo()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        Procparam.Add("@CompanyId", Convert.ToString(intCompanyID));
    //        ddlDocumentNumb.BindDataTable("s3g_org_LoadApplicationNo", Procparam, true, "-- Select --", new string[] { "ID", "Application_Number" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    //private void FunPriLoadCombo_ApplicationProcessGLNo()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        Procparam.Add("@CompanyId", Convert.ToString(intCompanyID));
    //        ddlDocumentNumb.BindDataTable("s3g_org_LoadApplicationNoGL", Procparam, true, "-- Select --", new string[] { "ID", "PANUM" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


    # region Document Number controls to load the Combo Box
    /// <summary>
    ///  Load the Document Number Combo Box with Customer Code
    /// </summary>
    //private void FunPriLoadCombo_CustomerCode()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_Get_Customer_Code, Procparam, true, "-- Select --", new string[] { "ID", "Customer_Code", "Customer_Name" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


    /// <summary>
    ///  Load the Document Number Combo Box with Customer Code
    /// </summary>
    //private void FunPriLoadCombo_CustomerCode_CPA()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_Get_Customer_Code_CPA, Procparam, true, "-- Select --", new string[] { "ID", "Customer_Code", "Customer_Name" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    //private void FunPriLoadCombo_CustomerCode_CGT()
    //{
    //    try
    //    {
    //        //FunPriMakeMultipleDNCVisible(lblProgramIDSearch, ddlDocumentNumb, true);
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable("S3G_Get_Customer_Code_CGT", Procparam, true, "-- Select --", new string[] { "ID", "Customer_Code", "Customer_Name" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    /// <summary>
    ///  Load the Document Number Combo Box with Customer Code
    /// </summary>
    //private void FunPriLoadCombo_CustomerCode_CPT()
    //{
    //    try
    //    {
    //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        Procparam.Add("@Program_ID", strProgramId);
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_Get_Customer_Code_CPT, Procparam, true, "-- Select --", new string[] { "ID", "Customer_Code", "Customer_Name" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    /// <summary>
    ///  Load the Credit Parameter Transaction Number Combo Box 
    /// </summary>
    //private void FunPriLoadCombo_CreditParameterTransaction()
    //{
    //    try
    //    {
    //        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        //Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_GetCreditParameterTransactionNo, Procparam, true, "-- Select --", new string[] { "ID", "CreditParamNumber" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    /// <summary>
    ///  Load the Document Number Combo Box with pricing offer number
    /// </summary>
    //private void FunPriLoadCombo_PricingBusinessOfferNo()
    //{
    //    try
    //    {
    //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_Get_PricingApprovalOffer_No, Procparam, true, "-- Select --", new string[] { "ID", "Offer_No" });
    //        //lblMultipleDNC.Text = "Business Offer Number";
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }

    //}
    /// <summary>
    ///  Load the Document Number Combo Box with Target Number
    /// </summary>
    //private void FunPriLoadCombo_TargetNo()
    //{
    //    try
    //    {
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //        //Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //        if (txtBranchSearch.Text != string.Empty) Procparam.Add("@Location_ID", Convert.ToString(hdnBranchID.Value));
    //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_Get_Target_Number, Procparam, true, "-- Select --", new string[] { "ID", "Business_Target_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


    /// <summary>
    ///  Load the Document Number Combo Box with Enquiry Number
    /// </summary>
    //private void FunPriLoadCombo_EnquiryNo()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_Get_Enquiry_No, Procparam, true, "-- Select --", new string[] { "ID", "Enquiry_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}



    /// <summary>
    ///  Load the Document Number Combo Box with Enquiry Number
    /// </summary>
    //private void FunPriLoadCombo_AssignedEnquiry()
    //{
    //    try
    //    {
    //        //FunPri_LOB_Branch();
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //        //Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //        if (txtBranchSearch.Text != string.Empty) Procparam.Add("@Location_ID", Convert.ToString(hdnBranchID.Value));
    //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_Get_Router_Enquiry, Procparam, true, "-- Select --", new string[] { "ID", "Enquiry_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


    /// <summary>
    ///  Load the Document Number Combo Box with Enquiry Number
    /// </summary>
    //private void FunPriLoadCombo_EnquiryNo_CPA()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_Get_Enquiry_No_CPA, Procparam, true, "-- Select --", new string[] { "ID", "Enquiry_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    //private void FunPriLoadCombo_ApplicationNo_CPA()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable("S3G_Get_Application_No_CPA", Procparam, true, "-- Select --", new string[] { "ID", "Number" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    //private void FunPriLoadCombo_PricingNo_CPA()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable("S3G_Get_Pricing_No_CPA", Procparam, true, "-- Select --", new string[] { "ID", "Number" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    //private void FunPriLoadCombo_EnquiryNo_CGT()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable("S3G_Get_EnquiryNo_CGT", Procparam, true, "-- Select --", new string[] { "ID", "EnquiryNo" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    //private void FunPriLoadCombo_PricingNo_CGT()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable("S3G_Get_PricingNo_CGT", Procparam, true, "-- Select --", new string[] { "ID", "PricingNo" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    //private void FunPriLoadCombo_ApplicationNo_CGT()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        ddlDocumentNumb.BindDataTable("S3G_Get_ApplicationNo_CGT", Procparam, true, "-- Select --", new string[] { "ID", "ApplicationNo" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    /// <summary>
    ///  Load the Document Number Combo Box with Enquiry Number
    /// </summary>
    //private void FunPriLoadCombo_EnquiryNo_CPT()
    //{
    //    try
    //    {
    //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        Procparam.Add("@Program_ID", strProgramId);

    //        FunPri_LOB_Branch();

    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_Get_Enquiry_No_CPT, Procparam, true, "-- Select --", new string[] { "ID", "Enquiry_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


    //private void FunPriLoadCombo_PricingNo_CPT()
    //{
    //    try
    //    {
    //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        Procparam.Add("@Program_ID", strProgramId);

    //        FunPri_LOB_Branch();

    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

    //        ddlDocumentNumb.BindDataTable("S3G_Get_Pricing_No_CPT", Procparam, true, "-- Select --", new string[] { "ID", "Business_Offer_Number" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    //private void FunPriLoadCombo_ApplicationNo_CPT()
    //{
    //    try
    //    {
    //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        Procparam.Add("@Program_ID", strProgramId);

    //        FunPri_LOB_Branch();

    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

    //        ddlDocumentNumb.BindDataTable("S3G_Get_Application_No_CPT", Procparam, true, "-- Select --", new string[] { "ID", "Application_Number" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    /// <summary>
    /// Will Load the Document Number Combo Box with Sanction Number
    /// </summary>

    //private void FunPriLoadCombo_AppraisedEnquiry()         //Enquiry
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_Get_Appraised_Enquiry_Customer, Procparam, true, "--Select--", new string[] { "ID", "Enquiry_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }

    //}

    //private void FunPriLoadCombo_AppraisedPricing()         //Pricing
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        ddlDocumentNumb.BindDataTable("S3G_ORG_Get_Appraised_Pricing", Procparam, true, "--Select--", new string[] { "ID", "Pricing_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    //private void FunPriLoadCombo_AppraisedApplication()         //Application
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        ddlDocumentNumb.BindDataTable("S3G_ORG_Get_Appraised_Application", Procparam, true, "--Select--", new string[] { "ID", "App_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }

    //}

    /// <summary>
    /// Will Load the Document Number Combo Box with Appraised Customers
    /// </summary>

    //private void FunPriLoadCombo_AppraisedCustomer()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_Get_Appraised_Customer, Procparam, true, "--Select--", new string[] { "ID", "CustomerName" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


    /// <summary>
    /// Will Load the Document Number Combo Box with Sanction Number
    /// </summary>
    //private void FunPriLoadCombo_SanctionNo()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        Procparam.Add("@User_ID", intUserID.ToString());
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_GetSanctionNo, Procparam, true, "-- Select --", new string[] { "ID", "Santion_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


    /// <summary>
    /// Will Load the Document Number Combo Box with FIR Number
    /// </summary>
    //private void FunPriLoadCombo_FIRNo()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        Procparam.Add("@User_ID", intUserID.ToString());
    //        Procparam.Add("@Program_ID", strProgramId);
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        if (ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "USER")
    //        {
    //            ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_GetFIRNo, Procparam, true, "-- Select --", new string[] { "ID", "FIR_No" });
    //        }
    //        else
    //        {
    //            ddlDocumentNumb.BindDataTable("S3G_ORG_GetFIRNo_For_UTPA", Procparam, true, "-- Select --", new string[] { "ID", "FIR_No" });
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    /// <summary>
    /// Will Load the Document Number Number Combo Box with Application Approval Number
    /// </summary>
    //private void FunPriLoadCombo_ApplicationNumber()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_GetApplicationNoForTL, Procparam, true, "-- Select --", new string[] { "ID", "Application_Number" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    //private void FunPriLoadComboCreditGuideTransaction()  //Credit Guide transaction Ramesh
    //{
    //    try
    //    {
    //        //ComboBoxBranchSearch.Items.Clear();
    //        if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        {
    //            //ComboBoxBranchSearch.AutoPostBack = false;
    //            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //            //Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue);
    //            //Procparam.Add("@Is_Active", "true");
    //            //ComboBoxBranchSearch.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, Procparam, true, "-- Select --", new string[] { "Constitution_ID", "ConstitutionName" });

    //            //Bind Product 
    //            Dictionary<string, string> Procparam1 = new Dictionary<string, string>();
    //            Procparam1.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue);
    //            Procparam1.Add("@Company_ID", Convert.ToString(intCompanyID));
    //            ddlDocumentNumb.BindDataTable(SPNames.SYS_ProductMaster, Procparam1, true, "-- Select --", new string[] { "Product_ID", "Product_Code", "Product_Name" });
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    /// <summary>
    /// Function to load Pricng's business offer number
    /// </summary>
    //private void FunPriLoadCombo_PricingNo()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        Procparam.Add("@Option", "15");
    //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        //if (ComboBoxLOBSearch.SelectedIndex > 0)
    //        //{
    //        //    Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue);
    //        //}
    //        //if (ComboBoxBranchSearch.SelectedIndex > 0)
    //        //{
    //        //    Procparam.Add("@Branch_ID", ComboBoxBranchSearch.SelectedValue);
    //        //}
    //        if (txtStartDateSearch.Text != "")
    //        {
    //            Procparam.Add("@Fromdate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        }
    //        if (txtEndDateSearch.Text != "")
    //        {
    //            Procparam.Add("@Todate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        }
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_GetPricing_List, Procparam, true, "-- Select --", new string[] { "Pricing_ID", "Business_Offer_Number" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


    /// <summary>
    /// Function to load Enquiry Response number
    /// </summary>
    //private void FunPriLoadCombo_EnquiryResponseNo()
    //{
    //    try
    //    {
    //        FunPri_LOB_Branch();
    //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        //Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        if (txtStartDateSearch.Text != "")
    //        {
    //            Procparam.Add("@Fromdate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        }
    //        if (txtEndDateSearch.Text != "")
    //        {
    //            Procparam.Add("@Todate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        }
    //        ddlDocumentNumb.BindDataTable("S3G_ORG_GetEnquiryResponseNocombo", Procparam, true, "-- Select --", new string[] { "Enquiry_Response_ID", "Enquiry_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    /// <summary>
    /// Will Load the Document Number Combo Box with Proforma Number
    /// </summary>
    //private void FunPriLoadCombo_ProformaNo()
    //{
    //    try
    //    {
    //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
    //        //Procparam.Add("@Location", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
    //        if (txtBranchSearch.Text != string.Empty) Procparam.Add("@Location", Convert.ToString(hdnBranchID.Value));
    //        if (txtStartDateSearch.Text != "")
    //            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (txtEndDateSearch.Text != "")
    //            Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_GetProformaNo, Procparam, true, "-- Select --", new string[] { "Proforma_ID", "Proforma_No" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    //private void FunPriLoadCombo_PRDDTNo()
    //{
    //    try
    //    {
    //        Procparam.Clear();
    //        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //        Procparam.Add("@Program_Id", strProgramId);
    //        FunPri_LOB_Branch();
    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

    //        ddlDocumentNumb.BindDataTable(SPNames.S3G_ORG_PRDDT, Procparam, true, "-- Select --", new string[] { "PreDisbursement_Doc_Tran_ID", "PRDDC_Number" });
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    /// <summary>
    /// To load the DNC according to the Lob and branch selected.
    /// </summary>
    //private void FunPri_LOB_Branch()
    //{
    //    try
    //    {
    //        //if (ComboBoxBranchSearch != null && ComboBoxBranchSearch.SelectedIndex > 0)
    //        //{
    //        //    Procparam.Add("@Location_Id", ComboBoxBranchSearch.SelectedValue.ToString());
    //        //}
    //        if (hdnBranchID.Value != null && hdnBranchID.Value != string.Empty)
    //        {
    //            Procparam.Add("@Location_Id", hdnBranchID.Value.ToString());
    //        }
    //        if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.SelectedIndex > 0)
    //        {
    //            Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue.ToString());
    //        }
    //        //if (ddlDNCOption != null && ddlDNCOption.Visible && ddlDNCOption.SelectedIndex > 0)
    //        //{
    //        //    Procparam.Add("@MultipleDNC_ID", ddlDNCOption.SelectedValue.ToString());
    //        //}
    //        //if (ddlMultipleDNC != null && ddlMultipleDNC.Visible && ddlMultipleDNC.SelectedIndex > 0)
    //        //{
    //        //    Procparam.Add("@MultipleOption_ID", ddlMultipleDNC.SelectedValue.ToString());
    //        //}
    //        if (ddlDocumentNumb != null)
    //            ddlDocumentNumb.Items.Clear();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}


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
            if (ddlDocumentNumb != null && ddlDocumentNumb.SelectedText != "")   // General document no of your page (ex)FIR Number,Enquiry No,App Processing No,
            {
                if (ProgramCode == strCreditGuideTransaction || ProgramCode == strCreditParameterTransactionCode)
                {
                    if (ddlMultipleDNC.SelectedIndex != 0)
                        Procparam.Add("@DocumentNumber", ddlDocumentNumb.SelectedValue);
                }
                else
                {
                    Procparam.Add("@DocumentNumber", ddlDocumentNumb.SelectedValue);
                }
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
            // Start Extra credit guide Transaction
            //if (ComboBoxBranchSearch.SelectedIndex > 0)
            //{
            //    Procparam.Add("@Branch", ComboBoxBranchSearch.SelectedValue.ToString());
            //}
            if (ddlDocumentNumb.SelectedText != "")
            {
                Procparam.Add("@Product_ID", ddlDocumentNumb.SelectedValue.ToString());
            }
            // End Extra credit guide Transaction
            Procparam.Add("@ProgramCode", ProgramCodeToCompare.ToString());                     // Sending the Program Code - to the SP

            if (ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "UTPA")
            {
                Procparam.Add("@User_Type", ObjUserInfo.ProUserTypeRW.ToString());
            }

            if (ddlCustomerName.Visible == true && lblCustomerName.Visible == true)
            {
                if (ddlCustomerName.SelectedValue == "0" && ddlCustomerName.SelectedText != "")
                    Procparam.Add("@Customer_ID", "-1");
                else if (ddlCustomerName.SelectedValue != "0" && ddlCustomerName.SelectedText != "")
                    Procparam.Add("@Customer_ID", ddlCustomerName.SelectedValue.ToString());
            }
            //Add here - add your extra SP parameters - if required... in the below switch case (also Add the same to the common SP - with your program Code Commented).
            switch (ProgramCode)
            {
                case strFieldInformationReportCode:
                    break;
                case strLegalclearanceReportCode:
                    break;
                case strTechclearanceReportCode:
                    break;
                case strSanctionNumberCode:
                    break;
                case strProformaReportCode:
                    break;
                case strEnquiryAssignment:            // Enquiry Number Assined Grid
                    break;
                case strEnquiryCustomerAppraisal:     // Force Appraised Customer / Enquiry
                    break;
                case strPRDDTransaction:
                    break;
                case strPricingApprovalReportCode:
                    break;
                case strTargetMaster:
                    break;
                case strCRM:
                    break;

                // case strCreditGuideTransaction:
                //ComboBoxBranchSearch.AutoPostBack = false;


            }

            FunPriBindGridWithFooter();                                                                   // Binding the grid.

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// Will bind the grid view 
    /// </summary>
    private void FunPriBindGridWithFooter()
    {
        try
        {
            int intTotalRecords = 0;
            bool bIsNewRow = false;

            grvTransLander.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            if (ProgramCodeToCompare == strApplicationApproval || ProgramCodeToCompare == strPricingApprovalReportCode)
                grvTransLander.Columns[1].Visible = false;
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvTransLander.Rows[0].Visible = false;
            }

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            lblErrorMessage.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
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
        //    Procparam.Add("@Branch", ComboBoxBranchSearch.SelectedValue.ToString());
        //}
        if (txtBranchSearch.Text != string.Empty)    // General document no of your page (ex)FIR Number,Enquiry No,App Processing No,
        {
            Procparam.Add("@Branch", hdnBranchID.Value.ToString());
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
        try
        {
            // Modified : Rajendran 
            int returnValue = 0;
            decimal decX = 0;
            //Added by Kali K on 15-DEC-2010 for date time display based on GPS
            DateTime tempdatetime;
            if (DateTime.TryParse(val, out tempdatetime))
                return 2;
            if (val is int)
                returnValue = 1;
            if (Decimal.TryParse(val, out decX))
                returnValue = 1;
            ////else if (val is DateTime)
            ////    returnValue = 2;
            else if (val is string)
                returnValue = 3;
            return returnValue;
        }
        catch (Exception)
        {
            return 0;
        }
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
                if (hidDate.Value.ToUpper() == "START")
                    Utility.FunShowAlertMsg(this, "Start date should be lesser than the End Date");
                else
                    Utility.FunShowAlertMsg(this, "End date should be greater than the Start Date");
                //Added By Thangam M on 23/Mar/2012
                grvTransLander.DataSource = null;
                grvTransLander.DataBind();
                ucCustomPaging.Visible = false;
                return;
            }
        }
        // else if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString())))
        //       &&
        //       ((string.IsNullOrEmpty(DateTime.Parse(txtEndDateSearch.Text, dtformat).ToString()))))                               // start date is not empty and end date is empty
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           ((string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtEndDateSearch.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        }
        //else if (((string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString())))
        //      &&
        //    (!(string.IsNullOrEmpty(DateTime.Parse(txtEndDateSearch.Text, dtformat).ToString()))))                              // end date is not empty and start date is empty
        if (((string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            // Modified : Thangam //
            //txtStartDateSearch.Text = txtEndDateSearch.Text;

        }

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
        if ((!bQuery) || (intModify == 0))
        {
            isQueryColumnVisible = false;
        }
        #endregion
        Session["DocumentNo"] = null;
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

            //e.Row.Cells[1].Visible = (intQuery == 0) ? false : true;
            //e.Row.Cells[2].Visible = (intModify == 0) ? false : true;

            e.Row.Cells[3].Visible = false;                             // ID Column - always invisible
            e.Row.Cells[4].Visible = false;                             // User ID Column - always invisible
            e.Row.Cells[5].Visible = false;                             // User Level ID Column - always invisible
            if (ProgramCodeToCompare == strPurchaseOrder)
            {
                e.Row.Cells[6].Visible = false;
                //added by vinodha.m to restrict the users to update the mapped po(with PI/VI)-ends
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                //added by vinodha.m to restrict the users to update the mapped po (with PI/VI)-ends
            }
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

            #endregion
            //to disable Modify if Pricing and application is approved,rejected,Cancelled 

            if (ProgramCode == strApplicationProcess || ProgramCode == strApplicationProcessGL || ProgramCode == strApplicationProcessBW)
            {
                if (e.Row.Cells[12].Text == "Approved" || e.Row.Cells[12].Text == "Rejected")//|| e.Row.Cells[12].Text == "Cancelled")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
            else if (ProgramCode == strPricing || ProgramCode == strPricingBW)
            {
                if (e.Row.Cells[11].Text == "Approved" || e.Row.Cells[11].Text == "Rejected")//|| e.Row.Cells[12].Text == "Cancelled")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }

            if (ProgramCode == strPurchaseOrder)
            {
                //added by vinodha.m to restrict the users to update the mapped po(with PI/VI)-Starts
                if (e.Row.Cells[11].Text == "Cancelled" || e.Row.Cells[12].Text == "1" || e.Row.Cells[12].Text == "2")//1-PO mapped with VI,2-PO mapped with PI
                //added by vinodha.m to restrict the users to update the mapped po(with PI/VI)-ends
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
                //added by vinodha.m to restrict the users to update the mapped po with PI/VI-ends
            }
            if (ProgramCode == strProformaInvoice || ProgramCode == strVendorInvoice)
            {
                if (e.Row.Cells[11].Text == "Cancelled")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }

            for (int i_cellVal = 3; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                try
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
                        //case 3:  // string - do nothing - left align(default)
                        //    e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Left;
                        //    break;
                    }
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
                }
            }
        }
        if (ProgramCodeToCompare == strEnquiryAssignment || ProgramCodeToCompare == strSanctionNumberCode || ProgramCodeToCompare == strCreditParameterApproval)
        {
            e.Row.Cells[1].Visible = false;
        }
        else
        {
            e.Row.Cells[1].Visible = true;
        }


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
        if (ProgramCodeToCompare == strCRM)
        {
            Session["InitiateNumber"] = e.CommandArgument;
            Response.Redirect("~/Origination/S3GOrgCRM.aspx");
        }

        if (ProgramCodeToCompare == strPurchaseOrder)
        {
            GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            Session["VendorGroup"] = gvr.Cells[6].Text;
            strPO_Type = gvr.Cells[14].Text;
        }
        if (ProgramCodeToCompare == strProformaInvoice)
        {
            GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            Session["PI_Number"] = gvr.Cells[8].Text;
            Session["PO_Number"] = gvr.Cells[7].Text;
        }
        if (ProgramCodeToCompare == strVendorInvoice)
        {
            GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            Session["VI_Number"] = gvr.Cells[8].Text;
            Session["PO_Number"] = gvr.Cells[7].Text;
        }
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
                Response.Redirect(strRedirectPage + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M&POType=" + strPO_Type);
               
                break;
            case "query":
                Response.Redirect(strRedirectPage + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&POType=" + strPO_Type);
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
        //        strTargetURL += "&Location=" + ComboBoxBranchSearch.SelectedValue.ToString();
        //    else
        //        strTargetURL += "?Location=" + ComboBoxBranchSearch.SelectedValue.ToString();
        //}
        if (hdnBranchID.Value != null && hdnBranchID.Value != string.Empty)
        {
            if (strTargetURL.Contains('?'))
                strTargetURL += "&Location=" + hdnBranchID.Value;
            else
                strTargetURL += "?Location=" + hdnBranchID.Value;
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
        try
        {
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
            Session["DocumentNo"] = null;
            Session["EnqNewCustomerId"] = null;
            if (ProgramCode == "APPP")
            {
                Session["PricingAssetDetails"] = null;
                Session["AssetCustomer"] = null;
            }

            Response.Redirect(strRedirectCreatePage);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
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
            //if (ddlDocumentNumb.Items.Count > 0)
            //{
            //    ddlDocumentNumb.ClearDropDownList();
            //    ddlDocumentNumb.SelectedItem.Text = "-- Select --";
            //}
            ddlDocumentNumb.SelectedText = "";
            grvTransLander.DataSource = null;
            grvTransLander.Visible = false;
            ucCustomPaging.Visible = false;
            ddlCustomerName.Clear();
            ddlCustomerName.SelectedText = "--Select--";
            lblErrorMessage.Text =
            txtStartDateSearch.Text =
            txtEndDateSearch.Text = "";
            ComboBoxLOBSearch.SelectedIndex = -1;
            txtBranchSearch.Text = string.Empty;
            hdnBranchID.Value = string.Empty;
            //ComboBoxBranchSearch.SelectedIndex = -1;
            //FunPriLoadDNCCombo();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    /// <summary>
    /// If there is more than one Document Number - then use this DDL
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void ddlMultipleDNC_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        if (ddlMultipleDNC.SelectedIndex > 0)
    //        {
    //            ddlDocumentNumb.Visible =
    //            lblProgramIDSearch.Visible = true;
    //            lblProgramIDSearch.Text = ddlMultipleDNC.SelectedItem.ToString();
    //            if (Procparam == null)
    //                Procparam = new Dictionary<string, string>();
    //            Procparam.Clear();



    //            // Add here case statement here - to load document number, and document number label.
    //            // Add your code here if you are passing the MultipleDNC Query String.
    //            switch (ProgramCode)
    //            {
    //                case strCreditParameterTransactionCode:
    //                    ProgramCodeToCompare = strCreditParameterTransactionCode;
    //                    strRedirectPage = "~/Origination/S3G_ORG_CreditParameterTransaction.aspx";     // page to redirect to the page                                                                  
    //                    Procparam.Add("@Company_ID", intCompanyID.ToString());
    //                    switch (Convert.ToInt32(ddlMultipleDNC.SelectedValue))
    //                    {

    //                        case 1:     // Enquiry Number                            
    //                            FunPriLoadCombo_EnquiryNo_CPT();
    //                            break;
    //                        case 2:         // Pricing
    //                            FunPriLoadCombo_PricingNo_CPT();
    //                            break;
    //                        case 3:         // Application                           
    //                            FunPriLoadCombo_ApplicationNo_CPT();
    //                            break;
    //                        case 4:         // Customer Code                           
    //                            FunPriLoadCombo_CustomerCode_CPT();
    //                            break;
    //                    }
    //                    break;
    //                case strCreditParameterApproval:
    //                    //ProgramCodeToCompare = strCreditParameterApproval;
    //                    LoadCPARecords();
    //                    break;

    //                case strEnquiryAssignment:
    //                    FunPriLoadCombo_AssignedEnquiry();
    //                    break;
    //                case strCreditGuideTransaction:
    //                    LoadCreditGuideTransRecords();
    //                    break;

    //                case strEnquiryCustomerAppraisal:
    //                    strRedirectPage = "";  // Page to redirect during edit & query
    //                    Procparam.Add("@Company_ID", intCompanyID.ToString());
    //                    switch (ddlMultipleDNC.SelectedValue)
    //                    {
    //                        case "1": //Enquiry
    //                            FunPriLoadCombo_AppraisedEnquiry(); // Appraised Customer
    //                            break;
    //                        case "2": //Pricing
    //                            FunPriLoadCombo_AppraisedPricing(); // Appraised Enquiry Customer
    //                            break;

    //                        case "3": //Application
    //                            FunPriLoadCombo_AppraisedApplication(); // Appraised Enquiry Customer
    //                            break;
    //                        case "4": //Customer
    //                            FunPriLoadCombo_AppraisedCustomer(); // Appraised Enquiry Customer
    //                            break;
    //                    }
    //                    break;

    //            }
    //        }
    //        if (ddlMultipleDNC.SelectedIndex == 0)
    //        {
    //            ddlDocumentNumb.Visible =
    //            lblProgramIDSearch.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    protected void ddlMultipleDNC_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlMultipleDNC.SelectedIndex > 0)
            {
                ddlDocumentNumb.Enabled =
                lblProgramIDSearch.Enabled = true;
                lblProgramIDSearch.Text = ddlMultipleDNC.SelectedItem.ToString();
            }
            if (ddlMultipleDNC.SelectedIndex == 0)
            {
                ddlDocumentNumb.Enabled =
                lblProgramIDSearch.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    //private void LoadCPARecords()
    //{
    //    try
    //    {
    //        strRedirectPage = "~/Origination/S3GOrgCreditParameterApproval.aspx";     // page to redirect to the page                                                                  
    //        Procparam.Add("@Company_ID", intCompanyID.ToString());
    //        if (txtStartDateSearch.Text.Trim().Length > 0 && txtEndDateSearch.Text.Trim().Length > 0)
    //        {
    //            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //            Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        }
    //        Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
    //        if (ddlDNCOption.SelectedIndex > 0)
    //        {
    //            DataSet dstStatus = new DataSet();
    //            DataTable dtTable = new DataTable("Status");
    //            if (!dtTable.Columns.Contains("StatusId"))
    //                dtTable.Columns.Add("StatusId", typeof(int));
    //            if (ddlDNCOption.SelectedItem.Text.ToLower().Equals("unapproved"))
    //            {
    //                Procparam.Add("@StatusValues", "5,8");
    //            }
    //            else if (ddlDNCOption.SelectedItem.Text.ToLower().Equals("all"))
    //            {
    //                Procparam.Add("@StatusValues", "5,8,6");
    //            }
    //            //dstStatus.Tables.Add(dtTable.Copy());                     

    //        }
    //        switch (ddlMultipleDNC.SelectedValue)
    //        {
    //            case "4":         // Customer Code                           
    //                FunPriLoadCombo_CustomerCode_CPA();
    //                break;
    //            case "1":         // Enquiry Number                            
    //                FunPriLoadCombo_EnquiryNo_CPA();
    //                break;
    //            case "2":         // Pricing                        
    //                FunPriLoadCombo_PricingNo_CPA();
    //                break;
    //            case "3":         // Application Number                             
    //                FunPriLoadCombo_ApplicationNo_CPA();
    //                break;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    //private void LoadCreditGuideTransRecords()
    //{
    //    try
    //    {
    //        strRedirectPage = "~/Origination/S3GORGCreditGuideTransaction_Add.aspx";     // page to redirect to the page                                                                  
    //        Procparam.Add("@Company_ID", intCompanyID.ToString());
    //        lblProgramIDSearch.Text = ddlMultipleDNC.SelectedItem.Text;
    //        Procparam.Add("@User_ID", intUserID.ToString());
    //        Procparam.Add("@Program_Id", strProgramId);

    //        if (txtStartDateSearch.Text.Trim().Length > 0 && txtEndDateSearch.Text.Trim().Length > 0)
    //        {
    //            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
    //            Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
    //        }
    //        switch (ddlMultipleDNC.SelectedValue)
    //        {
    //            case "1":         // Enquiry Number
    //                FunPriLoadCombo_EnquiryNo_CGT();
    //                break;
    //            case "2":         // Pricing                        
    //                FunPriLoadCombo_PricingNo_CGT();
    //                break;
    //            case "3":         // Application Number                             
    //                FunPriLoadCombo_ApplicationNo_CGT();
    //                break;
    //            case "4":         // Customer                        
    //                FunPriLoadCombo_CustomerCode_CGT();
    //                break;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }

    //}
    /// <summary>
    /// If there is any specific option releated to the Multiple DNC - then use this method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDNCOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlDNCOption.SelectedIndex > 0)
            //{
            //    if (Procparam == null)
            //        Procparam = new Dictionary<string, string>();
            //    Procparam.Clear();

            //    switch (ProgramCode)
            //    {
            //        case strCreditParameterApproval:
            //            LoadCPARecords();
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    /// <summary>
    /// To load the DNC combo according to this field get change 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ComboBoxLOBSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlDocumentNumb.SelectedText = "";
            ddlDocumentNumb.SelectedValue = "";

            //FunPriLoadDNCCombo();                               // to load DNC
            ddlMultipleDNC_SelectedIndexChanged(sender, e);     // to Load Multiple DNC

            CalendarExtenderEndDateSearch.Enabled =
            imgEndDateSearch.Visible =
            CalendarExtenderStartDateSearch.Enabled =
            imgStartDateSearch.Visible = true;

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
            //        Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue);
            //        Procparam.Add("@Program_Id", strProgramId);
            //        ComboBoxBranchSearch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });
            //        break;
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }


    /// <summary>
    /// To load the DNC combo according to this field get change 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    //protected void ComboBoxBranchSearch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        switch (ProgramCode)
    //        {
    //            //case strCreditGuideTransaction:
    //            //    break;
    //            default:
    //                FunPriLoadDNCCombo();                               // To load DNC
    //                ddlMultipleDNC_SelectedIndexChanged(sender, e);     // To load Multiple DNC    
    //                break;
    //        }
    //        CalendarExtenderEndDateSearch.Enabled =
    //        imgEndDateSearch.Visible =
    //        CalendarExtenderStartDateSearch.Enabled =
    //        imgStartDateSearch.Visible = true;
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    #endregion

    private void FunPriQueryString()
    {
        // Add here: if you want to pass the LOB and Branch as  a query string then - use the following case
        // pass you raw - URL string.
        switch (ProgramCode)
        {
            case strApplicationApproval:
                strRedirectPage = FunPriAddLOBBranchQueryString("~/Origination/S3GORGApplicationApproval_Add.aspx");
                break;
        }
    }

    protected void cmbDocumentNumberSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //switch (ProgramCode)
        //{
        //    case strSanctionNumberCode:
        //        if (ddlDocumentNumb.SelectedIndex > 0)
        //        {
        //            txtEndDateSearch.Text = txtStartDateSearch.Text = "";
        //            CalendarExtenderEndDateSearch.Enabled =
        //            imgEndDateSearch.Visible =
        //            CalendarExtenderStartDateSearch.Enabled =
        //            imgStartDateSearch.Visible = false;

        //        }
        //        else
        //        {
        //            CalendarExtenderEndDateSearch.Enabled =
        //            imgEndDateSearch.Visible =
        //            CalendarExtenderStartDateSearch.Enabled =
        //            imgStartDateSearch.Visible = true;
        //        }
        //        break;
        //}
    }
    protected void txtStartDateSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            hidDate.Value = "Start";

            //if (Procparam == null)
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Clear();

            //switch (ProgramCode)
            //{
            //    case strCreditParameterTransactionCode:
            //        ProgramCodeToCompare = strCreditParameterTransactionCode;
            //        strRedirectPage = "~/Origination/S3G_ORG_CreditParameterTransaction.aspx";     // page to redirect to the page                                                                  
            //        Procparam.Add("@Company_ID", intCompanyID.ToString());
            //        switch (Convert.ToInt32(ddlMultipleDNC.SelectedValue))
            //        {

            //            case 1:     // Enquiry Number                            
            //                FunPriLoadCombo_EnquiryNo_CPT();
            //                break;
            //            case 2:         // Pricing
            //                FunPriLoadCombo_PricingNo_CPT();
            //                break;
            //            case 3:         // Application                           
            //                FunPriLoadCombo_ApplicationNo_CPT();
            //                break;
            //            case 4:         // Customer Code                           
            //                FunPriLoadCombo_CustomerCode_CPT();
            //                break;
            //        }
            //        break;
            //    case strSanctionNumberCode:
            //        FunPriLoadDNCCombo();
            //        break;
            //    case strFieldInformationReportCode:
            //        FunPriLoadCombo_FIRNo();
            //        break;
            //    case strCreditGuideTransaction:
            //        LoadCreditGuideTransRecords();
            //        break;
            //    case strPricingApprovalReportCode:
            //        FunPriLoadCombo_PricingBusinessOfferNo();
            //        break;
            //    case strApplicationApproval:
            //        FunPriLoadCombo_ApplicationNumber();
            //        break;
            //    case strEnquiryResponse:
            //        FunPriLoadCombo_EnquiryResponseNo();
            //        break;
            //    case strEnquiryAssignment:
            //        FunPriLoadCombo_AssignedEnquiry();                        // Loading the Enquiry Numbers to Assigned
            //        break;
            //    case strTargetMaster:
            //        FunPriLoadCombo_TargetNo();
            //        break;
            //    default:
            //        FunPriLoadDNCCombo();
            //        break;
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void txtEndDateSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            hidDate.Value = "End";

            //if (Procparam == null)
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Clear();

            //switch (ProgramCode)
            //{
            //    case strCreditParameterTransactionCode:
            //        ProgramCodeToCompare = strCreditParameterTransactionCode;
            //        strRedirectPage = "~/Origination/S3G_ORG_CreditParameterTransaction.aspx";     // page to redirect to the page                                                                  
            //        Procparam.Add("@Company_ID", intCompanyID.ToString());
            //        switch (Convert.ToInt32(ddlMultipleDNC.SelectedValue))
            //        {

            //            case 1:     // Enquiry Number                            
            //                FunPriLoadCombo_EnquiryNo_CPT();
            //                break;
            //            case 2:         // Pricing
            //                FunPriLoadCombo_PricingNo_CPT();
            //                break;
            //            case 3:         // Application                           
            //                FunPriLoadCombo_ApplicationNo_CPT();
            //                break;
            //            case 4:         // Customer Code                           
            //                FunPriLoadCombo_CustomerCode_CPT();
            //                break;
            //        }
            //        break;
            //    case strSanctionNumberCode:
            //        FunPriLoadDNCCombo();
            //        break;
            //    case strFieldInformationReportCode:
            //        FunPriLoadCombo_FIRNo();
            //        break;
            //    case strEnquiryAssignment:
            //        FunPriLoadCombo_AssignedEnquiry();                        // Loading the Enquiry Numbers to Assigned
            //        break;
            //    case strCreditParameterApproval:
            //        LoadCPARecords();
            //        break;
            //    case strCreditGuideTransaction:
            //        LoadCreditGuideTransRecords();
            //        break;
            //    case strPricingApprovalReportCode:
            //        FunPriLoadCombo_PricingBusinessOfferNo();
            //        break;
            //    case strApplicationApproval:
            //        FunPriLoadCombo_ApplicationNumber();                     // Loading the Application Numbers 
            //        break;
            //    case strEnquiryResponse:
            //        FunPriLoadCombo_EnquiryResponseNo();
            //        break;
            //    case strTargetMaster:
            //        FunPriLoadCombo_TargetNo();
            //        break;
            //    case strCRM:
            //        FunPriLoadCombo_TargetNo();
            //        break;
            //    case strEnquiryCustomerAppraisal:
            //        strRedirectPage = "";  // Page to redirect during edit & query
            //        Procparam.Add("@Company_ID", intCompanyID.ToString());
            //        switch (ddlMultipleDNC.SelectedValue)
            //        {
            //            //case "1":
            //            //    FunPriLoadCombo_AppraisedCustomer(); // Appraised Customer
            //            //    break;
            //            //case "2":
            //            //    FunPriLoadCombo_AppraisedEnquiry(); // Appraised Enquiry Customer
            //            //    break;


            //            case "1": //Enquiry
            //                FunPriLoadCombo_AppraisedEnquiry(); // Appraised Customer
            //                break;
            //            case "2": //Pricing
            //                FunPriLoadCombo_AppraisedPricing(); // Appraised Enquiry Customer
            //                break;

            //            case "3": //Application
            //                FunPriLoadCombo_AppraisedApplication(); // Appraised Enquiry Customer
            //                break;
            //            case "4": //Customer
            //                FunPriLoadCombo_AppraisedCustomer(); // Appraised Enquiry Customer
            //                break;
            //        }
            //        break;
            //    //LoadCPARecords();
            //    //break;
            //    default:
            //        FunPriLoadDNCCombo();
            //        break;
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander - Origination");
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
    /// 

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerName(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", ojb_TransLander.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCustomers", Procparam), true);
        return suggetions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        if (ojb_TransLander.ProgramCode == "PURORD" || ojb_TransLander.ProgramCode == "PRFINV" || ojb_TransLander.ProgramCode == "VNINV")
        {
            Procparam.Clear();
            if (ojb_TransLander.txtStartDateSearch.Text != "")
                Procparam.Add("@StartDate", Utility.StringToDate(ojb_TransLander.txtStartDateSearch.Text).ToString());
            if (ojb_TransLander.txtEndDateSearch.Text != "")
                Procparam.Add("@EndDate", Utility.StringToDate(ojb_TransLander.txtEndDateSearch.Text).ToString());
            Procparam.Add("@Company_ID", Convert.ToString(ojb_TransLander.intCompanyID));
            Procparam.Add("@SearchText", prefixText);
            Procparam.Add("@ProgramCode", ojb_TransLander.ProgramCode);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_GetPO_LSQNo_AGT", Procparam));
        }
        else
        {
            switch (ojb_TransLander.ObjUserInfo.ProUserTypeRW.ToString().ToUpper())
            {
                case "UTPA":
                    {
                        Procparam.Clear();
                        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
                        Procparam.Add("@Type", "UTPA");
                        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
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
                    }
                    break;
                default:
                    {
                        Procparam.Clear();
                        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
                        Procparam.Add("@Type", "GEN");
                        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
                        Procparam.Add("@Program_Id", System.Web.HttpContext.Current.Session["ProgramId"].ToString());
                        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] != null)
                        {
                            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
                                Procparam.Add("@LOB_ID", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
                            else
                                Procparam.Add("@LOB_ID", "0");
                        }
                        Procparam.Add("@PrefixText", prefixText);
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));
                    }
                    break;
            }
        }
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetDocumentNumberList(String prefixText, int count)
    {
        List<String> suggetions = new List<String>();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@SearchText", prefixText);
        if (ojb_TransLander.hdnBranchID.Value != null && ojb_TransLander.hdnBranchID.Value != string.Empty)
        {
            Procparam.Add("@Location_Id", ojb_TransLander.hdnBranchID.Value.ToString());
        }
        if (ojb_TransLander.ComboBoxLOBSearch != null && ojb_TransLander.ComboBoxLOBSearch.SelectedIndex > 0)
        {
            Procparam.Add("@LOB_ID", ojb_TransLander.ComboBoxLOBSearch.SelectedValue.ToString());
        }
        if (ojb_TransLander.txtStartDateSearch.Text != "")
            Procparam.Add("@StartDate", Utility.StringToDate(ojb_TransLander.txtStartDateSearch.Text).ToString());
        if (ojb_TransLander.txtEndDateSearch.Text != "")
            Procparam.Add("@EndDate", Utility.StringToDate(ojb_TransLander.txtEndDateSearch.Text).ToString());
        Procparam.Add("@Company_ID", Convert.ToString(ojb_TransLander.intCompanyID));
        Procparam.Add("@User_ID", ojb_TransLander.intUserID.ToString());
        switch (ojb_TransLander.ProgramCode)
        {
            case strFieldInformationReportCode:
                Procparam.Add("@Program_ID", ojb_TransLander.strProgramId);
                if (ojb_TransLander.ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "USER")
                {
                    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetFIRNo_AGT", Procparam));
                }
                else
                {
                    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetFIRNo_For_UTPA_AGT", Procparam));
                }
                break;
            case strLegalclearanceReportCode:
                Procparam.Add("@Program_ID", ojb_TransLander.strProgramId);
                if (ojb_TransLander.ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "USER")
                {
                    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetlegalNo_AGT", Procparam));
                }
                else
                {
                    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetlegalNo_For_UTPA_AGT", Procparam));
                }
                break;
            case strTechclearanceReportCode:
                Procparam.Add("@Program_ID", ojb_TransLander.strProgramId);
                if (ojb_TransLander.ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "USER")
                {
                    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetTechNo_AGT", Procparam));
                }
                else
                {
                    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetTechNo_For_UTPA_AGT", Procparam));
                }

                break;
            case strSanctionNumberCode:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetSanctionNo_AGT", Procparam));
                break;
            case strProformaReportCode:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetProformaNo_AGT", Procparam));
                break;
            //case strCreditParameterTransactionCode:
            //     suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_ORG_GetCreditParameterTransactionNo, Procparam));
            //    break;
            case strEnquiryAssignment:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_Get_Router_Enquiry_AGT", Procparam));
                break;
            case strPRDDTransaction:
                Procparam.Add("@Program_Id", ojb_TransLander.strProgramId);
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetPRDDTNo_AGT", Procparam));
                break;
            case strPricingApprovalReportCode:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOAD_PROPOSALAPPR_CUSTOMER", Procparam));
                break;
            case strApplicationApproval:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetApplicationNoForTL_AGT", Procparam));
                break;
            case strEnquiryResponse:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEnquiryResponseNocombo_AGT", Procparam));
                break;
            case strPricing:
                Procparam.Add("@Option", "15");
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetPricing_List_AGT", Procparam));
                break;
            case strPricingBW:
                Procparam.Add("@Option", "15");
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetPricing_List_AGT", Procparam));
                break;

            case strApplicationProcess:
            case strApplicationProcessBW:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("s3g_org_LoadApplicationNo_AGT", Procparam));
                break;
            case strApplicationProcessGL:                                    // Loading the Application Number
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("s3g_org_LoadApplicationNoGL_AGT", Procparam));
                break;
            case strTargetMaster:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_Get_Target_Number_AGT", Procparam));
                break;
            case strCRM:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_Get_Target_Number_AGT", Procparam));
                break;
            case strMRA:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetMRA_List_AGT", Procparam));
                break;
            case strMRAApproval:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetMRA_List_AGT", Procparam));
                break;
            case strPurchaseOrder:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_LoadPurchaseOrderNo_AGT_Trans", Procparam));
                break;
            case strProformaInvoice:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_LoadProformaInvoiceNo_AGT", Procparam));
                break;
            case strVendorInvoice:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_LoadVendorInvoiceNo_AGT", Procparam));
                break;
            case strCreditParameterTransactionCode:
                //ProgramCodeToCompare = strCreditParameterTransactionCode;
                //strRedirectPage = "~/Origination/S3G_ORG_CreditParameterTransaction.aspx";     // page to redirect to the page                                                                  
                Procparam.Add("@Program_ID", ojb_TransLander.strProgramId);
                switch (Convert.ToInt32(ojb_TransLander.ddlMultipleDNC.SelectedValue))
                {

                    case 1:     // Enquiry Number                            
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Enquiry_No_CPT_AGT", Procparam));
                        break;
                    case 2:
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Pricing_No_CPT_AGT", Procparam));
                        break;
                    case 3:         // Application                           
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Application_No_CPT_AGT", Procparam));
                        break;
                    case 4:         // Customer Code                           

                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Customer_Code_CPT_AGT", Procparam));
                        break;
                }
                break;
            case strCreditParameterApproval:

                if (ojb_TransLander.ddlDNCOption.SelectedIndex > 0)
                {
                    DataSet dstStatus = new DataSet();
                    DataTable dtTable = new DataTable("Status");
                    if (!dtTable.Columns.Contains("StatusId"))
                        dtTable.Columns.Add("StatusId", typeof(int));
                    if (ojb_TransLander.ddlDNCOption.SelectedItem.Text.ToLower().Equals("unapproved"))
                    {
                        Procparam.Add("@StatusValues", "5,8");
                    }
                    else if (ojb_TransLander.ddlDNCOption.SelectedItem.Text.ToLower().Equals("all"))
                    {
                        Procparam.Add("@StatusValues", "5,8,6");
                    }
                }
                switch (ojb_TransLander.ddlMultipleDNC.SelectedValue)
                {

                    case "1":         // Enquiry Number                            
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Enquiry_No_CPA_AGT", Procparam));
                        break;
                    case "2":         // Pricing                        
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Pricing_No_CPA_AGT", Procparam));
                        break;
                    case "3":         // Application Number                             
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Application_No_CPA_AGT", Procparam));
                        break;
                    case "4":         // Customer Code                           
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Customer_Code_CPA_AGT", Procparam));
                        break;
                }
                break;


            case strCreditGuideTransaction:
                Procparam.Add("@Program_ID", ojb_TransLander.strProgramId);
                switch (ojb_TransLander.ddlMultipleDNC.SelectedValue)
                {
                    case "1":         // Enquiry Number

                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_EnquiryNo_CGT_AGT", Procparam));
                        break;
                    case "2":         // Pricing                        
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_PricingNo_CGT_AGT", Procparam));
                        break;
                    case "3":         // Application Number                             
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_ApplicationNo_CGT_AGT", Procparam));
                        break;
                    case "4":         // Customer                        
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Customer_Code_CGT_AGT", Procparam));
                        break;
                }
                break;

            case strEnquiryCustomerAppraisal:
                switch (ojb_TransLander.ddlMultipleDNC.SelectedValue)
                {
                    case "1": //Enquiry
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_Get_Appraised_Enquiry_Customer_AGT", Procparam));
                        break;
                    case "2": //Pricing
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_Get_Appraised_Pricing_AGT", Procparam));
                        break;
                    case "3": //Application
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_Get_Appraised_Application_AGT", Procparam));
                        break;
                    case "4": //Customer
                        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_Get_Appraised_Customer_AGT", Procparam));
                        break;
                }
                break;
            default:
                // to do: disable the page 
                break;
        }
        return suggetions.ToArray();

    }
}
#endregion
