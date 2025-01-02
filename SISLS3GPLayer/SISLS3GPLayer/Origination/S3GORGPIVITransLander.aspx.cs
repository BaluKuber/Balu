#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Orgination
/// Screen Name         :   PI and VI Transaction Lander
/// Created By          :   Chandru K
/// Created Date        :   13-Aug-2015
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
using LoanAdminMgtServicesReference;

#endregion

#region Class Origination_S3GORGTransLander
public partial class Origination_S3GORGPIVITransLander : ApplyThemeForProject
{
    # region Programs Code
    string ProgramCodeToCompare = "";                                           // this is to hold the Program Code of your web page
    // Add here - Add your Program Code Here - refer to the SQL table Program Master.
    const string strProformaInvoice = "PRFINV";
    const string strVendorInvoice = "VNINV";
    public string strProgramId = "0";
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
    public static Origination_S3GORGPIVITransLander ojb_TransLander = null;

    #region  User Authorization
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
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
        bDelete = ObjUserInfo.ProDeleteRW;
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
                if (!bDelete)
                {
                    btnCancel.Enabled = false;
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
            }
            else
            {
               
                FunPriMakeMultipleDNCVisible(lblMultipleDNC, ddlMultipleDNC, false);
            }

            if (intDNCOption == 1)
            {
                dictDNCOption = new Dictionary<int, string>();
               
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

            lblBranchSearch.Text = "Location";
            txtBranchSearch.Enabled = ddlDNCOption.Enabled = ddlDocumentNumb.Enabled = true;
            switch (ProgramCode)
            {
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
                    lblCustomerName.Visible = ddlCustomerName.Visible = true;
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
                    lblVendor.Visible = ddlVendor.Visible = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
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
                        
                        ComboBoxLOBSearch.BindDataTable(SPNames.LOBMaster, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
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

    private void FunPriBindGrid()
    {
        try
        {
            FunPriAddCommonParameters();                                                        // Adding the Common parameters to the dictionary            
            // Passing DNC ddl value to the SP
            if (ddlDocumentNumb != null && ddlDocumentNumb.SelectedText != "")   // General document no of your page (ex)FIR Number,Enquiry No,App Processing No,
            {
                Procparam.Add("@DocumentNumber", ddlDocumentNumb.SelectedText);
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

            if (ddlVendor.Visible == true && lblVendor.Visible == true)
            {
                if (ddlVendor.SelectedValue == "0" && ddlVendor.SelectedText != "")
                    Procparam.Add("@Vendor_ID", "-1");
                else if (ddlVendor.SelectedValue != "0" && ddlVendor.SelectedText != "")
                    Procparam.Add("@Vendor_ID", ddlVendor.SelectedValue.ToString());
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
            if (bDelete)
                btnCancel.Enabled = true;

            grvTransLander.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            if (bIsNewRow)
            {
                grvTransLander.Rows[0].Visible = false;
                btnCancel.Enabled = false;
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

    protected void btnCancelPO_Click(object sender, EventArgs e)
    {
        try
        {
            int ErrorCode = 0;
            int intOption = 2;
            string strPONumber = "";
            string strPONumberOut = "";
            foreach (GridViewRow grvRow in grvTransLander.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    strPONumber += ((Label)grvRow.FindControl("lblPIN")).Text + ",";
                }
            }
            LoanAdminMgtServicesClient objLoanAdmin_MasterClient;
            objLoanAdmin_MasterClient = new LoanAdminMgtServicesClient();

            if (strPONumber != "")
            {
                if (strProgramId == "290")
                    intOption = 2;
                else if (strProgramId == "293")
                    intOption = 3;

                int intResult = objLoanAdmin_MasterClient.FunPubCancelMultiplePO(out ErrorCode, out strPONumberOut, strPONumber, intOption,"");
                if (intResult == 0)
                {
                    string strRedi = "../Origination/S3GORGPIVITransLander.aspx?Code=" + ProgramCode;
                    Utility.FunShowAlertMsg(this.Page, "Invoice Cancelled successfully", strRedi);
                    return;
                }
                if (intResult == 12)
                {
                    Utility.FunShowAlertMsg(this.Page, "Already Invoice has been Cancelled.");
                }
                if (intResult == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, "Invoice already mapped with RS, Unable to Cancel - " + strPONumberOut);
                }
                if (intResult == 6)
                {
                    Utility.FunShowAlertMsg(this.Page, "Payment made for the selected invoice, hence unable to cancel it - " + strPONumberOut);
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one Invoice");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
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
            if (ProgramCode == strVendorInvoice)
            {
                Label lblPI = (Label)e.Row.FindControl("lblPI");
                Label lblPID = (Label)e.Row.FindControl("lblPID");
                lblPI.Text = "VI Number";
                lblPID.Text = "VI Date";
            }
            CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
            chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvTransLander.ClientID + "',this,'chkSelected');");
        }
        
        if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
        {

            #region User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            Label lblPIStatus = (Label)e.Row.FindControl("lblPIStatus");
            Label lblPIMappingStatus = (Label)e.Row.FindControl("lblPIMappingStatus");
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

            #endregion
            //to disable Modify if Pricing and application is approved,rejected,Cancelled 
            if (ProgramCode == strProformaInvoice || ProgramCode == strVendorInvoice)
            {
                if (lblPIStatus.Text == "Cancelled")
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }

                if (lblPIMappingStatus.Text == "1")//1-PI/VI MAPPED WITH RS,0-PI/VI NOT MAPPED WITH RS
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
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
       
        GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
        Label lblPON = (Label)gvr.FindControl("lblPON");
        Label lblPIN = (Label)gvr.FindControl("lblPIN");
        Session["PO_Number"] = lblPON.Text;
        if (ProgramCodeToCompare == strProformaInvoice)
            Session["PI_Number"] = lblPIN.Text;
        if (ProgramCodeToCompare == strVendorInvoice)
            Session["VI_Number"] = lblPIN.Text;
       
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
            ddlVendor.Clear();
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
    
    protected void ddlDNCOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           
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
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    #endregion

    private void FunPriQueryString()
    {
        // Add here: if you want to pass the LOB and Branch as  a query string then - use the following case
        // pass you raw - URL string.
    }

    protected void cmbDocumentNumberSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void txtStartDateSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            hidDate.Value = "Start";
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
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", ojb_TransLander.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));
        return suggestions.ToArray();
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
            
            case strProformaInvoice:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_LoadProformaInvoiceNo_AGT", Procparam));
                break;
            case strVendorInvoice:
                suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_LoadVendorInvoiceNo_AGT", Procparam));
                break;
            default:
                // to do: disable the page 
                break;
        }
        return suggetions.ToArray();

    }
}
#endregion
