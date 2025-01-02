

#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Orgination
/// Screen Name         :   Transaction Lander
/// Created By          :   S.Kannan
/// Created Date        :   30-July-2010
/// Purpose             :   This is the landing screen for all the other Orgination Screens
/// Last Updated By		:   Rajendran
/// Last Updated Date   :   Dec 06 /2010
/// Reason              :   Removed the Old Data Type Check Logic
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
    const string strCRM = "CRM";//program code for CRM
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
    string strProcName = "S3G_ORG_TransLander_CRM";                             // this is the Stored procedure to get call                     
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

        //ddlUserSide.Control_ID = ddlUserSide.ID;
        //ddlTaskStatus.Control_ID = ddlTaskStatus.ID;
        //ddlQueryType.Control_ID = ddlQueryType.ID;

        #region Initialize page
        bool IsQueryStringChanged = false;
        if (!(string.IsNullOrEmpty(Request.QueryString["Code"])))                   // reading the query string
        {
            // to do  : want to decrypt this code in the URL
            btnCreate.Text = "Create";
            FunPriGetQueryStrings();
            InitPage();
            System.Web.HttpContext.Current.Session["ProgramId"] = strProgramId;
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
            txtBranchSearch.Visible = lblBranchSearch.Visible = false;
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
            FunProLoadUserSide();
            popUserName.gvDisplay = "Name";
            #region  User Authorization
            if (btnCreate.Enabled)                                                  // if the user can view the create button - depends on the query string
            {
                //User Authorization

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
        }
        #endregion
        ViewState["EnquiryorCustomer"] = string.Empty;
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
            txtBranchSearch.Enabled = true;
            switch (ProgramCode)
            {
                case strCRM:
                    strProgramId = "241";
                    this.Title = "S3G-CRM";
                    ProgramCodeToCompare = strCRM;
                    lblHeading.Text = "CRM-Details";
                    strRedirectPage = "~/Origination/S3G_ORG_CRM_ADD.aspx";
                    strRedirectCreatePage = strRedirectPage;// +"?qsMode=C";
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        // }
    }


    protected void FunProLoadUserSide()
    {
        DataTable dtUserSide = new DataTable();
        dtUserSide.Columns.Add("ID");
        dtUserSide.Columns.Add("Text");

        DataRow dRow = dtUserSide.NewRow();
        dRow["ID"] = 1;
        dRow["Text"] = "From";
        dtUserSide.Rows.Add(dRow);

        dRow = dtUserSide.NewRow();
        dRow["ID"] = 2;
        dRow["Text"] = "To";
        dtUserSide.Rows.Add(dRow);

        ddlTaskStatus.DataValueField = "ID";
        ddlTaskStatus.DataTextField = "Text";
        ddlUserSide.DataSource = dtUserSide;


        Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        DataSet dsLookUp = Utility.GetDataset("S3G_CLN_GetFollowUp_LookUp", Procparam);

        ddlTaskStatus.DataValueField = "Lookup_Code";
        ddlTaskStatus.DataTextField = "Lookup_Description";
        ddlTaskStatus.DataSource = dsLookUp.Tables[4];

        ddlQueryType.DataValueField = "Lookup_Code";
        ddlQueryType.DataTextField = "Lookup_Description";
        ddlQueryType.DataSource = dsLookUp.Tables[3];
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
            if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.Items.Count == 2)
            {
                ComboBoxLOBSearch.SelectedIndex = 1;
                ComboBoxLOBSearch.ClearDropDownList();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    # region Document Number controls to load the Combo Box


    /// <summary>
    /// To load the DNC according to the Lob and branch selected.
    /// </summary>
    private void FunPri_LOB_Branch()
    {
        try
        {

            if (hdnBranchID.Value != null && hdnBranchID.Value != string.Empty)
            {
                Procparam.Add("@Location_Id", hdnBranchID.Value.ToString());
            }
            if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.SelectedIndex > 0)
            {
                Procparam.Add("@LOB_ID", ComboBoxLOBSearch.SelectedValue.ToString());
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
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

            Procparam.Add("@UserSide", ddlUserSide.SelectedValue);
            Procparam.Add("@FilterUser", popUserName.SelectedValue);
            Procparam.Add("@TaskStatus", ddlTaskStatus.SelectedValue);
            Procparam.Add("@QueryType", ddlQueryType.SelectedValue);

            //Add here - add your extra SP parameters - if required... in the below switch case (also Add the same to the common SP - with your program Code Commented).
            switch (ProgramCode)
            {
                case strCRM:
                    break;
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

        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))                                   // If start and end date is not empty
        {
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

        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           ((string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtEndDateSearch.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

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
        FunProSearch();
    }

    protected void FunProSearch()
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

        pnlSearchMore.Style.Add("display", "none");
    }

    /// <summary>
    /// Will set the Grid Style and Alignment of the string dynamically depend on the data types of the cell
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvTransLander_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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

        switch (e.CommandName.ToLower())
        {
            case "modify":
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
            grvTransLander.DataSource = null;
            grvTransLander.Visible = false;
            ucCustomPaging.Visible = false;
            lblErrorMessage.Text =
            txtStartDateSearch.Text =
            txtEndDateSearch.Text = "";
            ComboBoxLOBSearch.SelectedIndex = -1;
            txtBranchSearch.Text = string.Empty;
            hdnBranchID.Value = string.Empty;

            ddlQueryType.Clear();
            ddlTaskStatus.Clear();
            ddlUserSide.Clear();
            popUserName.Clear();
            txtUserName.Text = "--Select--";
            pnlSearchMore.Style.Add("display", "none");
            FunProLoadUserSide();

            //ComboBoxBranchSearch.SelectedIndex = -1;
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

        switch (System.Web.HttpContext.Current.Session["PageUserType"].ToString())
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
        return suggetions.ToArray();
    }

    protected void btnGetUser_Click(object sender, EventArgs e)
    {
        (popUserName.FindControl("pnlLoadLOV") as Panel).Style.Add("min-width", "300px");
        (popUserName.FindControl("pnlLoadLOV") as Panel).Style.Add("max-width", "10%");
        (popUserName.FindControl("pnlLoadLOV") as Panel).Style.Add("float", "right");

        popUserName.LOVCode = "CRMUM";
        popUserName.gvDisplay = "Name";
        popUserName.ucProcparam = new Dictionary<string, string>();

        popUserName.Show();
    }

    protected void UserSelected_Click(object sender, EventArgs e)
    {
        txtUserName.Text = popUserName.SelectedText;

        if (popUserName.SelectedValue == ObjUserInfo.ProUserIdRW.ToString())
        {
            chkDefault.Checked = true;
        }
        else
        {
            chkDefault.Checked = false;
        }
    }

    protected void btnMore_Click(object sender, EventArgs e)
    {
        //pnlSearchMore.Visible = true;
        pnlSearchMore.Style.Add("display", "block");
    }

    protected void btnSearchMoreOk_Click(object sender, EventArgs e)
    {
        FunProSearch();
    }

    protected void chkDefault_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDefault.Checked)
        {
            popUserName.SelectedValue = ObjUserInfo.ProUserIdRW.ToString();
            txtUserName.Text = ObjUserInfo.ProUserNameRW;
        }
        else
        {
            popUserName.Clear();
            txtUserName.Text = "--Select--";
        }
    }

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            pnlSearchMore.Style.Add("display", "none");
        }
        catch (Exception ObjException)
        {
        }
    }
}
#endregion
