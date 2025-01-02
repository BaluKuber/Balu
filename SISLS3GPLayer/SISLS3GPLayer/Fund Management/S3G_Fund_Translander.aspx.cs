#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Fund Management
/// Screen Name         :   Transaction Lander
/// Created By          :  Jeyagomathi M
/// Created Date        :   08-Aug-2010
/// Purpose             :   This is the landing screen for all the other Fund management Screens
/// 
/// 
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

public partial class Fund_Management_S3G_Fund_Translander : ApplyThemeForProject
{
    # region Programs Code
    string ProgramCodeToCompare = "";
    public string strQueryString = "";
    const string strNoteCreation = "NOTE";                        // Program Code for Note Creation
    const string strNoteCreationApproval = "APNOT";
    const string strGrossMarginCalculation = "GMC";
    public static Fund_Management_S3G_Fund_Translander obj_Page;

    static bool IsApprovalNeed;
    string ProgramCode = "";


    static string userUTPA;
    #endregion

    static bool strRbtnJournal = false;

    #region Common Variables
    int intCreate = 0;                                                         // intCreate = 1 then display the create button, else invisible
    int intQuery = 0;                                                          // intQuery = 1 then display the Query button, else invisible
    int intModify = 0;                                                         // intModify = 1 then display the Modify button, else invisible
    int intMultipleDNC = 0;                                                    // Allow the user to select the DNC dynamically.
    int intDNCOption = 0;                                                      // Allow the user to select the further option depend on the DNC - eg: approved,unapproved etc...
    Dictionary<int, string> dictMultipleDNC = null;                             // collection for Multiple DNC - DDL
    Dictionary<int, string> dictDNCOption = null;                               // Collection for DNCOption.
    string strProcName = "S3G_Fund_TransLander";                             // this is the Stored procedure to get call                     
    // To maintain the ProgramID
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
    string[] strLOBCodeToFilter;
    public string strProgramId = "";
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
            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            FunProLoadCombos();                                                     // loading the combos - LOB and Branch
            grvTransLander.Visible =
            ucCustomPaging.Visible = false;
            //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);           // setting the Page Title
            if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.Items.Count > 0)    // to set to the default position
                ComboBoxLOBSearch.SelectedIndex = 1;
            //if (ComboBoxBranchSearch != null && ComboBoxBranchSearch.Items.Count > 0)    // to set to the default position
            //    ComboBoxBranchSearch.SelectedIndex = 0;
            txtBranchSearch.Text = string.Empty;
            hdnBranchID.Value = string.Empty;
            //Added by Chandru
            txtDocumentNumberSearch.Text = string.Empty;
            hdnCommonID.Value = string.Empty;
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

                    case strNoteCreationApproval:
                        btnCreate.Text = btnCreate.ToolTip = "Approve";
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
            case strNoteCreation:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strNoteCreationApproval:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strGrossMarginCalculation:
                FunPriEnableActionButtons(true, true, true, false, false, false);
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

        if (Request.QueryString.Get("Code") != null)
        {
            ProgramCode = (Request.QueryString.Get("Code"));
            System.Web.HttpContext.Current.Session["AutoSuggestProgramCode"] = ProgramCode;
        }

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


        switch (ProgramCode)
        {
            case strNoteCreation:
                strProgramId = "289";
                FunPubIsVisible(true, true, true, false);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strNoteCreation;
                this.Title = "S3G - Note Creation";
                lblHeading.Text = " Note Creation - Details";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Payment Request No.";                                         // This is to display on the Document Number Label                
                lblAutosuggestProgramIDSearch.Text = "Note Creation No.";
                //Modified by chandru
                strRedirectPage = "~/Fund Management/S3G_FundMgt_NoteCreation.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode              
                lblLesseeName.Visible = ddlCustomer.Visible = true;
                break;
            case strNoteCreationApproval:
                strProgramId = "290";
                FunPubIsVisible(true, true, true, false);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strNoteCreationApproval;
                this.Title = "S3G - Note Creation Approval";
                lblHeading.Text = " Note Creation - Approval";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Payment Request No.";                                         // This is to display on the Document Number Label                
                lblAutosuggestProgramIDSearch.Text = "Note Creation No.";
                //Modified by chandru
                strRedirectPage = "~/Fund Management/S3G_FundMgt_NoteCreationApproval.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode            
                lblLesseeName.Visible = ddlCustomer.Visible = true;
                break;
            case strGrossMarginCalculation:
                strProgramId = "537";
                FunPubIsVisible(true, false, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strGrossMarginCalculation;
                this.Title = "S3G - Gross Margin Calculation";
                lblHeading.Text = "Gross Margin Calculation";
                //Modified by chandru
                //lblAutosuggestProgramIDSearch.Text = "Payment Request No.";                                         // This is to display on the Document Number Label                
                lblAutosuggestProgramIDSearch.Text = "Gross Margin Calculation No.";
                //Modified by chandru
                strRedirectPage = "~/Fund Management/S3G_FundMgt_GrossMarginCalculation.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode              
                lblLesseeName.Visible = ddlCustomer.Visible = true;
                lblAutosuggestProgramIDSearch.Visible = txtDocumentNumberSearch.Visible = false;
                break;
        }

    }


    private void FunPriFilterAndLoadLOB()
    {

        if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.DataSource != null)
        {
            DataTable dtLOB = (DataTable)ComboBoxLOBSearch.DataSource;
            if (dtLOB != null && dtLOB.Rows.Count > 0)
            {
                StringBuilder strddlItem = new StringBuilder();
                for (int i_lob = 0; i_lob < strLOBCodeToFilter.Length; i_lob++)
                {
                    strddlItem.Append(" LOB_Code like '" + strLOBCodeToFilter[i_lob] + "' or ");
                }
                strddlItem.Append(" LOB_Code like '" + strLOBCodeToFilter[0] + "'");

                dtLOB.DefaultView.RowFilter = strddlItem.ToString();

                ComboBoxLOBSearch.Items.Clear();

                //dtLOB.Columns.Add("DataText", typeof(string), "LOB_Code+'  -  '+LOB_Name");

                ComboBoxLOBSearch.DataValueField = "LOB_ID";
                ComboBoxLOBSearch.DataTextField = "DataText";

                ComboBoxLOBSearch.DataSource = dtLOB;

                ComboBoxLOBSearch.DataBind();

                //ListItem liSelect = new ListItem("-- Select --", "0");
                //ComboBoxLOBSearch.Items.Insert(0, liSelect);

            }

        }

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
    private void FunPubIsVisible(bool isLOB, bool isBranch, bool isMultipleDNC, bool isRSNo)
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
        txtBranchSearch.Visible = lblBranchSearch.Visible = isBranch;
        lblRSNo.Visible = ddlRSNo.Visible = isRSNo;
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
                    ComboBoxLOBSearch.SelectedIndex = 1;
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
                    ComboBoxLOBSearch.BindDataTable(SPNames.LOBMaster, Procparam, true, "--Select--", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                    ComboBoxLOBSearch.SelectedIndex = 1;
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
        // Loading Multiple DNC
        FunPriLoadMultiDNCCombo();

        FunPriLoadDNCCombo();

        switch (ProgramCode)
        {
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
            Procparam.Add("@ProgramCode", ProgramCodeToCompare.ToString());                     // using for approval screen purpose

            //Parameter added for UTPA Access - Asset vreification - Kuppu- Aug-27 
            if (ObjUserInfo.ProUserTypeRW.ToString().ToUpper() == "UTPA")
            {
                Procparam.Add("@User_Type", ObjUserInfo.ProUserTypeRW.ToString());
            }

            if (Convert.ToInt32(ddlTrancheName.SelectedValue) > 0 && Convert.ToString(ddlTrancheName.SelectedText) != "")
                Procparam.Add("@Tranche_Id", Convert.ToString(ddlTrancheName.SelectedValue));

            switch (ProgramCode)
            {
                case strNoteCreationApproval:
                    if (Convert.ToInt32(ddlCustomer.SelectedValue) > 0 && Convert.ToString(ddlCustomer.SelectedText) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(ddlCustomer.SelectedValue));
                    break;
                case strNoteCreation:
                    if (Convert.ToInt32(ddlCustomer.SelectedValue) > 0 && Convert.ToString(ddlCustomer.SelectedText) != "")
                        Procparam.Add("@Customer_ID", Convert.ToString(ddlCustomer.SelectedValue));
                    break;
            }

            bool colModify = true;//This is to hide column grid
            bool colQuery = true;

            //Add here - add your extra SP parameters - if required... in the below switch case (also Add the same to the common SP - with your program Code Commented).


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

            //Code added and commented by saran on 26-Jul-2013 for BW end

        }
        if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
        {

            #region User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            //Label lblUserID = (Label)e.Row.FindControl("lblUserID");
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

            //Added Tranche name in note translander on sep 10,2015 - vinodha m
            if (ProgramCode == strNoteCreation) 
            {
                if (e.Row.Cells[11].Text == "Rejected")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
            //Added Tranche name in note translander on sep 10,2015 - vinodha m

            if (ProgramCode == strNoteCreationApproval) // Added by Rajendran 5/11/2011
            {
                if (e.Row.Cells[10].Text == "Approved" || e.Row.Cells[10].Text == "Rejected")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
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

            //if (cmbDocumentNumberSearch.Items.Count > 1)
            //    cmbDocumentNumberSearch.SelectedIndex = -1;

            ComboBoxLOBSearch.SelectedIndex = 0;
            //ComboBoxBranchSearch.SelectedIndex = 0;
            txtBranchSearch.Text = string.Empty;
            hdnBranchID.Value = string.Empty;
            FunPriLoadDNCCombo();
            txtDocumentNumberSearch.Text = string.Empty;
            hdnCommonID.Value = string.Empty;
            ddlCustomer.Clear();
            ddlTrancheName.Clear();
            ddlRSNo.Clear();
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
        //if (System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"] != null)
        //{
        //    if (System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"].ToString() != "0")
        //        Procparam.Add("@Location_ID", System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"].ToString());
        //}
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

        switch (obj_Page.ProgramCode)
        {
            case strNoteCreationApproval:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_NoteNumber_AGT", Procparam));
                break;
            default:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_NoteNumber_AGT", Procparam));
                break;
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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander - Fund Management");
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LoanAd_GetCustomer_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Program_id", "285");
        Procparam.Add("@Approved", "1");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetMRANumber_AGT", Procparam));
        return suggetions.ToArray();
    }

    #endregion
}
