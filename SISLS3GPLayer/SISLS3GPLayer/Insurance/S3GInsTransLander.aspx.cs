#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Insurance
/// Screen Name         :   Transaction Lander
/// Created By          :   Vijaya Kumar
/// Created Date        :   12-April-2011
/// Purpose             :   This is the landing screen for all the other Insurance Screens
/// Last Updated By		:   Bhuvaneswari.C
/// Last Updated Date   :   04/12/2011
/// Reason              :   NULL
/// <Program Summary>
#endregion

#region How to use this
/*
   1)	Search for the word "Add Here" and add your code respectively  there...
 * 2)   Use the same stored procedure S3G_CLN_TransLander, the return table should have the first column named "ID" - to use it as the RowCommandValue
            SP return table Rule
                1)  First Column should be "ID" - which will be used as a row command
                2)  Second Column should be "Created_By" - which will be the created_By column
                3)  third should be the "User_Level_ID" - which will be the createdBy user's level id.            
            The second and third should was used to check the user authorization.
            Take latest code from - App_Code\Utility.cs
 * 3)  Add your page Program ID as a parameter (@DocumentNumber)
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
using S3GBusEntity.Insurance;
using Resources;
#endregion

public partial class S3GInsTranslander : ApplyThemeForProject
{
    # region Programs Code
    string ProgramCodeToCompare = "";                                           // this is to hold the Program Code of your web page
    public string strQueryString = "";
    // Add here - Add your Program Code Here - refer to the SQL table Program Master.
    const string strAssetInsuranceEntry = "AINE";
    const string strAssetInsuranceClaim = "AINC";
    public static S3GInsTranslander ojb_TransLander = null;

    static bool IsApprovalNeed;
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
    string strProcName = "S3G_Ins_Translander";                             // this is the Stored procedure to get call                     
    string ProgramCode;                                                         // To maintain the ProgramID
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
    string[] strLOBCodeToFilter;                                       // give the selective lob code 
    string strDNC_SP;
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
            FunPriUIVisibility();
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
            IsApprovalNeed = false;
            RFVComboLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
            //RFVComboBranch.ErrorMessage = ValidationMsgs.S3G_ValMsg_Branch;

            ViewState["ProgramCode"] = ProgramCode;
            IsQueryStringChanged = false;

            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',false,  false);");
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',false,  false);");

            //txtEndDateSearch.Attributes.Add("readonly", "readonly");                // making the end date textbox readonly
            //txtStartDateSearch.Attributes.Add("readonly", "readonly");              // making the start date textbox readonly
            FunProLoadCombos();                                                     // loading the combos - LOB and Branch
            grvTransLander.Visible =
            ucCustomPaging.Visible = false;
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);           // setting the Page Title
            //if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.Items.Count > 0)    // to set to the default position
            //    ComboBoxLOBSearch.SelectedIndex = 0;
            //if (ComboBoxBranchSearch != null && ComboBoxBranchSearch.Items.Count > 0)    // to set to the default position
            //    ComboBoxBranchSearch.SelectedIndex = 0;
            ComboBoxBranchSearch.Clear();

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



        }
        #endregion
        ViewState["EnquiryorCustomer"] = string.Empty;

        if (ComboBoxBranchSearch.SelectedValue.ToString() != "0")
            ucCustomerCodeLov.strBranchID = ComboBoxBranchSearch.SelectedValue.ToString();
        if (ComboBoxLOBSearch.SelectedValue.ToString() != "0")
            ucCustomerCodeLov.strLOBID = ComboBoxLOBSearch.SelectedValue.ToString();
        ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;

        //TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        //txt.Attributes.Add("onfocus", "fnLoadCustomer()");

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

            case strAssetInsuranceEntry:
                FunPriEnableActionButtons(true, true, true, false, false, false);
                break;
            case strAssetInsuranceClaim:
                FunPriEnableActionButtons(true, true, true, false, false, false);
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
            ProgramCode = (Request.QueryString.Get("Code"));
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
            // lblProgramIDSearch.Visible = cmbDocumentNumberSearch.Visible = false;

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
            // lblProgramIDSearch.Visible = cmbDocumentNumberSearch.Visible = true;
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
        btnCreate.Visible = true;

        //cmbCustomerCode.Visible = false;
        ucCustomerCodeLov.Visible = false;
        lblCustomerCode.Visible = false;

        lblEndDateSearch.Text = "End Date";// Declared in common for all other screens
        lblStartDateSearch.Text = "Start Date";


        switch (ProgramCode)
        {

            case strAssetInsuranceEntry:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strAssetInsuranceEntry;
                this.Title = "S3G - Asset Insurance Entry";
                lblProgramIDSearch.Text = "AINSE Number";                                         // This is to display on the Document Number Label                
                strRedirectPage = "~/Insurance/S3GInsAssetInsuranceEntry.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                strDNC_SP = "S3G_INS_GetInsuranceEntry_AGT";
                break;
            case strAssetInsuranceClaim:
                FunPubIsVisible(true, true, true);
                FunPubIsMandatory(false, false, false, false);
                ProgramCodeToCompare = strAssetInsuranceClaim;
                this.Title = "S3G - Asset Insurance Claim";
                lblProgramIDSearch.Text = "AICN Number";                                         // This is to display on the Document Number Label                
                strRedirectPage = "~/Insurance/S3GInsAssetInsuranceClaim.aspx";        // page to redirect to the page in edit mode
                strRedirectCreatePage = strRedirectPage + "?qsMode=C";        // page to redirect to the page in edit mode                
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                strDNC_SP = "S3G_INS_GetInsuranceClaim_AGT";
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

                ListItem liSelect = new ListItem("--Select--", "0");
                ComboBoxLOBSearch.Items.Insert(0, liSelect);

            }

        }

    }


    /// <summary>
    /// To set the LOB and Branch Mandatory/NonMandatory
    /// </summary>
    /// <param name="isLOBMandatory">true to set the LOB DDL Mandatory</param>
    /// <param name="isBranchMandatory">true to set the Branch DDL Mandatory</param>
    private void FunPubIsMandatory(bool isLOBMandatory, bool isBranchMandatory, bool isStartDateMandatory, bool isEndDateMandatory)
    {
        // To make the LOB and Branch Non-Mandatory
        //RFVComboBranch.Enabled = isBranchMandatory;
        RFVComboLOB.Enabled = isLOBMandatory;
        RFVStartDate.Enabled = isStartDateMandatory;
        RFVEndDate.Enabled = isEndDateMandatory;
        // To change the Label style to Non mandatory
        lblLOBSearch.CssClass = (isLOBMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel";
        lblBranchSearch.CssClass = (isBranchMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel"; ;
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
        //RFVComboBranch.Enabled = isBranch;
        RFVComboLOB.Enabled = isLOB;
        // To change the Label style to Non mandatory
        lblLOBSearch.CssClass = (isLOB) ? "styleReqFieldLabel" : "styleDisplayLabel";
        ComboBoxLOBSearch.Visible = lblLOBSearch.Visible = isLOB;

        lblBranchSearch.CssClass = (isBranch) ? "styleReqFieldLabel" : "styleDisplayLabel"; ;
        ComboBoxBranchSearch.Visible = lblBranchSearch.Visible = isBranch;

        lblProgramIDSearch.Visible = cmbDocumentNumberSearch.Visible = lblCustomerName.Visible = ucCustomer.Visible =
        lblTrancheName.Visible = ucTrancheName.Visible = lblRSNo.Visible = ucRSNo.Visible = isMultipleDNC;
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

        if (Procparam != null)
            Procparam.Clear();

        if (ProgramCode == strAssetInsuranceEntry || ProgramCode == strAssetInsuranceClaim)
        {            
            if(ProgramCode==strAssetInsuranceEntry)
            {
                Procparam.Add("@Program_ID", Convert.ToString(131));
            }
            else if (ProgramCode == strAssetInsuranceClaim)
            {
                Procparam.Add("@Program_ID", Convert.ToString(141));
            }
            //Procparam.Add("@OPTION", "6");
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@USER_ID", Convert.ToString(intUserID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@FilterOption", "'OL'");
            //ComboBoxLOBSearch.BindDataTable("S3G_INS_LOADLOV", Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ComboBoxLOBSearch.BindDataTable("S3G_Get_LOB_LIST", Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ComboBoxLOBSearch.Items.Count == 2)
            {
                ComboBoxLOBSearch.SelectedIndex = 1;
            }
        }
        else
        {

            // LOB ComboBoxLOBSearch
            if (ProgramCode == strAssetInsuranceEntry)
            {
                Procparam.Add("@Program_ID", Convert.ToString(131));
            }
            else if (ProgramCode == strAssetInsuranceClaim)
            {
                Procparam.Add("@Program_ID", Convert.ToString(141));
            }
            
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Is_Active", "1");
            ComboBoxLOBSearch.BindDataTable(SPNames.LOBMaster, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        }

        // branch
        //Procparam.Clear();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //Procparam.Add("@User_ID", Convert.ToString(intUserID));
        //Procparam.Add("@Is_Active", "1");
        //if (ProgramCode == strAssetInsuranceEntry)
        //{
        //    Procparam.Add("@Program_ID", Convert.ToString(131));
        //}
        //else if (ProgramCode == strAssetInsuranceClaim)
        //{
        //    Procparam.Add("@Program_ID", Convert.ToString(141));
        //}
        //ComboBoxBranchSearch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location_Code","Location_Name" });

        // Loading Multiple DNC

        FunPriLoadMultiDNCCombo();

        FunPriLoadDNCCombo();

        switch (ProgramCode)
        {
            case "1": // temp., remove this while adding your first case....
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

        // Add Here - your case statement blow if needed
        // Document Number Combo - Before add your code check if the common Function was available.       
        switch (ProgramCode)
        {
            case strAssetInsuranceEntry:
                FunPriLoadCombo_AssetInsuranceEntry();
                break;
            case strAssetInsuranceClaim:
                FunPriLoadCombo_AssetInsuranceClaim();
                break;
            default:
                // to do: disable the page 
                break;
        }
    }




    

   

    /// <summary>
    ///  Load the Document Number Combo Box with Appropriate Number
    /// </summary>
    //private void FunPriLoadCombo_AppropriateNumber()
    //{
    //    //FunPri_LOB_Branch();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_CLN_GetAppropriateNumber, Procparam, true, "-- Select --", new string[] { "ID", "Appropriation_ID" });
    //}

   

    
    /// <summary>
    ///  Load the Document Number Combo Box with Asset Insurance Number
    /// </summary>
    private void FunPriLoadCombo_AssetInsuranceEntry()
    {
        if (ComboBoxLOBSearch.SelectedIndex > 0)
        {
            Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));            
        }
        if (ComboBoxBranchSearch.SelectedValue != "0")
            Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
        //cmbDocumentNumberSearch.BindDataTable("S3G_INS_AssetInsuranceNumber", Procparam, true, "-- Select --", new string[] { "ID", "AINSE_No" });
    }
    private void FunPriLoadCombo_AssetInsuranceClaim()
    {
        if (ComboBoxLOBSearch.SelectedIndex > 0)
        {
            Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));            
        }
        if (ComboBoxBranchSearch.SelectedValue != "0")
            Procparam.Add("@Location_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
            Procparam.Add("@stDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
            Procparam.Add("@endDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
        //cmbDocumentNumberSearch.BindDataTable("S3G_INS_AssetInsuranceClaimNumber", Procparam, true, "-- Select --", new string[] { "ID", "ICN_No" });
    }


   
   


  
    

    
    /// <summary>
    /// Bind the Landing Grid.
    /// </summary>
    private void FunPriBindGrid()
    {
        try
        {
            FunPriAddCommonParameters();                                                        // Adding the Common parameters to the dictionary            
            // Passing DNC ddl value to the SP
            if (!string.IsNullOrEmpty(cmbDocumentNumberSearch.SelectedText) && cmbDocumentNumberSearch.SelectedValue != "0")  // General document no of your page (ex)FIR Number,Enquiry No,App Processing No,
            {
                Procparam.Add("@DocumentNumber", cmbDocumentNumberSearch.SelectedValue);
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
            if ((!string.IsNullOrEmpty(cmbDocumentNumberSearch.SelectedText) && cmbDocumentNumberSearch.SelectedValue != "0") && IsApprovalNeed)   // using for approval screen purpose
            {
                Procparam.Add("@Task_Number_ID", cmbDocumentNumberSearch.SelectedValue);
            }


            // ADDED FOR ASSET INSURANCE ENTRY PLT CHANGES BY SWARNA ON 12TH APR 2015

            if (ucRSNo.Visible==true)   // using for approval screen purpose
            {
                if (ucCustomer.SelectedValue == "0" && ucCustomer.SelectedText != "")
                    Procparam.Add("@Customer_ID", "-1");
                else if (ucCustomer.SelectedValue != "0" && ucCustomer.SelectedText == "")
                    Procparam.Add("@Customer_ID", "0");
                else if (ucCustomer.SelectedValue != "0" && ucCustomer.SelectedText != "")
                    Procparam.Add("@Customer_ID", ucCustomer.SelectedValue.ToString());

                if (ucTrancheName.SelectedValue == "0" && ucTrancheName.SelectedText != "")
                    Procparam.Add("@Tranche_ID", "-1");
                else if (ucTrancheName.SelectedValue != "0" && ucTrancheName.SelectedText == "")
                    Procparam.Add("@Tranche_ID", "0");
                else if (ucTrancheName.SelectedValue != "0" && ucTrancheName.SelectedText != "")
                    Procparam.Add("@Tranche_ID", ucTrancheName.SelectedValue.ToString());

                if (ucRSNo.SelectedValue == "0" && ucRSNo.SelectedText != "")
                    Procparam.Add("@PA_SA_REF_ID", "-1");
                else if (ucRSNo.SelectedValue != "0" && ucRSNo.SelectedText == "")
                    Procparam.Add("@PA_SA_REF_ID", "0");
                else if (ucRSNo.SelectedValue != "0" && ucRSNo.SelectedText != "")
                    Procparam.Add("@PA_SA_REF_ID", ucRSNo.SelectedValue.ToString());
            }

            Procparam.Add("@ProgramCode", ProgramCodeToCompare.ToString());                     // using for approval screen purpose


            //Add here - add your extra SP parameters - if required... in the below switch case (also Add the same to the common SP - with your program Code Commented).
            switch (ProgramCode)
            {
                default:
                    break;
            }

            FunPriBindGridWithFooter();                                                                   // Binding the grid.

        }
        catch (FaultException<UserMgtServicesReference.IUserMgtServices> objFaultExp)
        {
            // lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// Will bind the grid view 
    /// </summary>
    private void FunPriBindGridWithFooter()
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
        if (IsApprovalNeed)
        {
            grvTransLander.Columns[1].Visible = false;
        }
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

        Procparam = new Dictionary<string, string>();
        if (Procparam != null)
        {
            Procparam.Clear();
        }
        if (ComboBoxLOBSearch.SelectedIndex > 0)
        {
            Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue.ToString());
        }
        if (ComboBoxBranchSearch.SelectedValue != "0" && !string.IsNullOrEmpty(ComboBoxBranchSearch.SelectedText))
        {
            Procparam.Add("@Branch", ComboBoxBranchSearch.SelectedValue.ToString());
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
        try                                                         // casting - to use proper align       
        {
            Int32 tempint = Convert.ToInt32(Convert.ToDecimal(val));                   // Try int     
            return 1;
        }
        catch (Exception ex)
        {
            try
            {
                DateTime tempdatetime = Convert.ToDateTime(val);    // try datetime
                return 2;
            }
            catch (Exception ex1)
            {
                return 3;                                           // try String
            }
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
                Utility.FunShowAlertMsg(this, "Start Date should be lesser than or equal to the End Date");
                return;
            }
        }
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           ((string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtEndDateSearch.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        }
        if (((string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
            (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtStartDateSearch.Text = txtEndDateSearch.Text;

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

            //if (ProgramCode == strECSProcess)
            //    e.Row.Cells[1].Text = "Authorize";
        }
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow) // to hide the "ID" column
        {


            e.Row.Cells[3].Visible = false;                             // ID Column - always invisible
            e.Row.Cells[4].Visible = false;                             // User ID Column - always invisible
            e.Row.Cells[5].Visible = false;                             // User Level ID Column - always invisible

        }
        if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
        {

            #region User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

            ImageButton imgbtnQuery = (ImageButton)e.Row.FindControl("imgbtnQuery");
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

            //if ((ProgramCode == strAssetInsuranceClaim || ProgramCode == strAssetInsuranceEntry) && lblUserID.Text == ObjUserInfo.ProUserIdRW.ToString())
            //{
            //    imgbtnEdit.Enabled = false;
            //    imgbtnEdit.CssClass = "styleGridEditDisabled";
            //}
            //else
            //{
            //    imgbtnEdit.Enabled = true;
            //    imgbtnEdit.CssClass = "styleGridEdit";
            //}
            #endregion

            //if (ProgramCode == strAccountCreation)
            //{
            //    strQueryString = "AppProcessId =" + e.Row.Cells[3].Text + "&" +
            //    "CompanyId=" + e.Row.Cells[7].Text + "&" + "PANum=" + e.Row.Cells[8].Text + "&" + "SANum=" + e.Row.Cells[9].Text;
            //}
            for (int i_cellVal = 2; i_cellVal < e.Row.Cells.Count; i_cellVal++)
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
                        case 3:  // string - do nothing - left align(default)
                            e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Left;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //continue;
                }
            }
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
        //if (ProgramCode == strAccountCreation)
        //{
        //    strQueryString = Convert.ToString(ViewState["AccountCreation"]);
        //}

        Session.Remove("Land_CustomerDT");

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
        if (ComboBoxBranchSearch.SelectedValue != "0")
        {
            if (strTargetURL.Contains('?'))
                strTargetURL += "&Branch=" + ComboBoxBranchSearch.SelectedValue.ToString();
            else
                strTargetURL += "?Branch=" + ComboBoxBranchSearch.SelectedValue.ToString();
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
        if (string.IsNullOrEmpty(strRedirectCreatePage))
        {
            Utility.FunShowAlertMsg(this, "Target page not found");
            return;
        }

        Session.Remove("Land_CustomerDT");

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
            cmbDocumentNumberSearch.Clear();
            ucCustomer.Clear();
            ucTrancheName.Clear();
            ucRSNo.Clear();
            ComboBoxLOBSearch.SelectedIndex = 0;
            ComboBoxBranchSearch.Clear();
            ViewState["Land_CustomerID"] = "";
            ucCustomerCodeLov.FunPubClearControlValue();
            FunPriLoadDNCCombo();
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

            cmbDocumentNumberSearch.Visible =
            lblProgramIDSearch.Visible = true;
            lblProgramIDSearch.Text = ddlMultipleDNC.SelectedItem.ToString();
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
        if (ddlMultipleDNC.SelectedIndex == 0)
        {

            cmbDocumentNumberSearch.Visible =
            lblProgramIDSearch.Visible = false;
        }
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
        ddlMultipleDNC_SelectedIndexChanged(sender, e);
        //FunPubLoadLOBBasedLocation();// to Load Multiple DNC
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

    #region Customer Search
    /// <summary>
    /// To load the Customer according to this field get change 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    /// 

    //protected void cmbCustomerCode_TextChanged(object sender, EventArgs e)
    //{
    //    GetCustomerList(cmbCustomerCode.Text, 20);
    //    ViewState["Land_CustomerID"] = "";

    //    if (cmbCustomerCode.Text != "")
    //    {
    //        DataTable dtCustomer = new DataTable();
    //        dtCustomer = (DataTable)System.Web.HttpContext.Current.Session["Land_CustomerDT"];
    //        if (dtCustomer != null)
    //        {
    //            string filterExpression = "Cust_Code like '" + cmbCustomerCode.Text + "'";
    //            DataRow[] dtSuggestions = dtCustomer.Select(filterExpression);


    //            foreach (DataRow dr in dtSuggestions)
    //            {
    //                ViewState["Land_CustomerID"] = dr["Customer_ID"].ToString();
    //                cmbCustomerCode.Text = dr["Code"].ToString();
    //            }
    //        }
    //    }
    //}

    //[System.Web.Services.WebMethod]
    //public static string[] GetCustomerList(String prefixText, int count)
    //{
    //    List<String> suggetions = null;
    //    DataTable dtCustomer = new DataTable();
    //    dtCustomer = (DataTable)System.Web.HttpContext.Current.Session["Land_CustomerDT"];
    //    suggetions = GetSuggestions(prefixText, count, dtCustomer);
    //    return suggetions.ToArray();
    //}

    //protected static List<String> GetSuggestions(string key, int count, DataTable dt1)
    //{
    //    List<String> suggestions = new List<string>();
    //    try
    //    {
    //        string filterExpression = "Cust_Code like '%" + key + "%'";
    //        DataRow[] dtSuggestions = dt1.Select(filterExpression);
    //        foreach (DataRow dr in dtSuggestions)
    //        {
    //            string suggestion = dr["Cust_Code"].ToString();
    //            suggestions.Add(suggestion);
    //        }
    //    }
    //    catch (Exception objException)
    //    {
    //        //return suggestions;
    //        //  ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
    //        //   lblErrorMessage.Text = Resources.LocalizationResources.CustomerTypeChangeError;
    //    }
    //    return suggestions;
    //}

    #endregion

    protected void txtStartDateSearch_TextChanged(object sender, EventArgs e)
    {
        FunPriLoadDNCCombo();
    }
    protected void txtEndDateSearch_TextChanged(object sender, EventArgs e)
    {
        FunPriLoadDNCCombo();
    }
    //public void FunPubLoadLOBBasedLocation()
    //{
    //    Procparam = new Dictionary<string, string>();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@User_ID", Convert.ToString(intUserID));
    //    if (ProgramCode == strAssetInsuranceEntry)
    //    {
    //        Procparam.Add("@Program_ID", Convert.ToString(131));
    //    }
    //    else if (ProgramCode == strAssetInsuranceClaim)
    //    {
    //        Procparam.Add("@Program_ID", Convert.ToString(141));
    //    }
    //    Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue.ToString());
    //    Procparam.Add("@Is_Active", "1");
    //    ComboBoxBranchSearch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location_Code", "Location_Name" });
    //}
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", ojb_TransLander.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", ojb_TransLander.intUserID.ToString());
        if (ojb_TransLander.ProgramCode == strAssetInsuranceEntry)
        {
            Procparam.Add("@Program_ID", Convert.ToString(131));
        }
        else 
        {
            Procparam.Add("@Program_ID", Convert.ToString(141));                      
        }        
        Procparam.Add("@Lob_Id", ojb_TransLander.ComboBoxLOBSearch.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }


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
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCUSTOMERS", Procparam), false);
        return suggetions.ToArray();
    }



    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", ojb_TransLander.intCompanyID.ToString());
        Procparam.Add("@User_ID", ojb_TransLander.intUserID.ToString());
        Procparam.Add("@Program_Id", "131");
        if (ojb_TransLander.ComboBoxBranchSearch.SelectedValue != "0")
            Procparam.Add("@Location_ID", ojb_TransLander.ComboBoxBranchSearch.SelectedValue);
        if (ojb_TransLander.ucCustomer.SelectedValue != "0")
            Procparam.Add("@Customer_ID", ojb_TransLander.ucCustomer.SelectedValue);
        if (ojb_TransLander.ucTrancheName.SelectedValue != "0")
            Procparam.Add("@Tranche_Header_Id", ojb_TransLander.ucTrancheName.SelectedValue);
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRS_AGT", Procparam));
        return suggetions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetTrancheName(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", ojb_TransLander.intCompanyID.ToString());
        Procparam.Add("@User_ID", ojb_TransLander.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        //suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCUSTOMERS", Procparam), false);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTrancheAGT", Procparam));
        return suggetions.ToArray();
    }



    
    [System.Web.Services.WebMethod]
    public static string[] GetDocumentNumber(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        if (ojb_TransLander.ComboBoxLOBSearch.SelectedValue != "0")
        {
            Procparam.Add("@LOB_ID", ojb_TransLander.ComboBoxLOBSearch.SelectedValue);
        }
        if (ojb_TransLander.ComboBoxBranchSearch.SelectedValue != "0")
        {
            Procparam.Add("@Location_ID", ojb_TransLander.ComboBoxBranchSearch.SelectedValue);
        }
        if (ojb_TransLander.txtStartDateSearch.Text != "")
        {
            Procparam.Add("@StartDate", ojb_TransLander.txtStartDateSearch.Text);
        }
        if (ojb_TransLander.txtEndDateSearch.Text != "")
        {
            Procparam.Add("@EndDate", ojb_TransLander.txtEndDateSearch.Text);
        }
        Procparam.Add("@Company_ID", ojb_TransLander.intCompanyID.ToString());

        Procparam.Add("@User_ID", ojb_TransLander.intUserID.ToString());
        //Procparam.Add("@Program_Id", ojb_TransLander.PrgIDToCompare.ToString());


        Procparam.Add("@PrefixText", prefixText);
        //suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_CLT_GetCapture_Val_AGT", Procparam));
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(ojb_TransLander.strDNC_SP, Procparam));


        return suggetions.ToArray();
    }
}

