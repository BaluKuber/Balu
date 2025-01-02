#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Loan Admin
/// Screen Name         :   Transaction Lander
/// Created By          :   Chandrasekar K
/// Created Date        :   27-Feb-2016
/// Purpose             :   
/// <Program Summary> 
#endregion

#region How to use this

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
using System.Web.Services;
using S3GBusEntity.Origination;
using Resources;
using System.IO;
using System.IO.Compression;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using QRCoder;
using System.Drawing;
#endregion

#region Class LoanAdmin_S3G_Loanad_InterimBilling_View

public partial class LoanAdmin_S3G_Loanad_InterimBilling_View : ApplyThemeForProject
{
    # region Programs Code

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
    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    bool isQueryColumnVisible;                                                  // To change the Query Column visibility - depend on the user autherization 
    bool isEditColumnVisible;                                                  // To change the Edit Column visibility - depend on the user autherization 
    string[] strLOBCodeToFilter;
    public string strProgramId = "";
    public static LoanAdmin_S3G_Loanad_InterimBilling_View obj_Page;

    //ReportDocument rpd = new ReportDocument();

    Dictionary<string, string> dictparam;
    int intRegAddress = 0;
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

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (rpd != null)
        //{
        //    rpd.Close();
        //    rpd.Dispose();
        //}
    }

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
            InitPage();
            System.Web.HttpContext.Current.Session["ProgramId"] = strProgramId;
            System.Web.HttpContext.Current.Session["ProgramCode"] = ProgramCode;
            FunPriEnableActionButtons(true, false, false, false, false, false);
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
            RFVComboLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
            RFVComboBranch.ErrorMessage = ValidationMsgs.S3G_ValMsg_Branch;

            ViewState["ProgramCode"] = ProgramCode;
            IsQueryStringChanged = false;

            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");

            CalendarExtenderStartDateSearch.OnClientDateSelectionChanged = "checkDate_NextSystemDate";
            CalendarExtenderEndDateSearch.OnClientDateSelectionChanged = "checkDate_NextSystemDate";

            FunProLoadCombos();
            grvTransLander.Visible =
            ucCustomPaging.Visible = false;

            if (ComboBoxLOBSearch != null && ComboBoxLOBSearch.Items.Count > 0)
                ComboBoxLOBSearch.SelectedIndex = 0;

            txtBranchSearch.Text = string.Empty;
            hdnBranchID.Value = string.Empty;

            txtDocumentNumberSearch.Text = txtDocumentNumber.Text = string.Empty;
            hdnCommonID.Value = hdnDocNo.Value = string.Empty;

            txtLessee.Text = hdnLessee.Value = txtTrancheName.Text = hdnTranche.Value = string.Empty;

            btnPrintRental.Visible = btnPrintAMF.Visible = false;

            #region  User Authorization
            if (btnCreate.Enabled)
            {
                //User Authorization
                if (!bIsActive)
                {
                    btnCreate.Enabled = false;
                    return;
                }
                if (!bModify)
                {
                    intModify = 0;
                }
                if (!bQuery)
                {
                    intQuery = 0;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                    intCreate = 0;
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
        strProgramId = "323";
        FunPubIsVisible(true, true, false);
        FunPubIsMandatory(false, false, false, false);
        this.Title = "S3G - Interim Billing";
        lblHeading.Text = "Interim Billing";
        strRedirectPage = "~/LoanAdmin/S3G_Loanad_InterimBilling.aspx";
        strRedirectCreatePage = strRedirectPage + "?qsMode=C";
    }


    private void FunPriFilterAndLoadLOB()
    {

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
        RFVComboBranch.Enabled = isBranch;
        RFVComboLOB.Enabled = isLOB;
        lblLOBSearch.CssClass = (isLOB) ? "styleReqFieldLabel" : "styleDisplayLabel";
        ComboBoxLOBSearch.Visible = lblLOBSearch.Visible = isLOB;
        lblBranchSearch.CssClass = (isBranch) ? "styleReqFieldLabel" : "styleDisplayLabel"; ;
        //ComboBoxBranchSearch.Visible = lblBranchSearch.Visible = isBranch;
        txtBranchSearch.Visible = lblBranchSearch.Visible = isBranch;
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
                }
                break;
            default:
                {
                    if (Procparam != null)
                        Procparam.Clear();
                    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    Procparam.Add("@User_ID", Convert.ToString(intUserID));
                    Procparam.Add("@Program_Id", strProgramId);
                    ComboBoxLOBSearch.BindDataTable(SPNames.LOBMaster, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

                }
                break;
        }

        if (ComboBoxLOBSearch.Items.Count == 2)
        {
            ComboBoxLOBSearch.SelectedIndex = 1;
            ComboBoxLOBSearch.ClearDropDownList();
        }

    }

    /// <summary>
    /// Bind the Landing Grid.
    /// </summary>
    private void FunPriBindGrid()
    {
        try
        {
            FunPriAddCommonParameters();

            if (txtLessee.Text != string.Empty)
            {
                Procparam.Add("@Customer_ID", hdnLessee.Value);
            }

            if (txtDocumentNumberSearch.Text != string.Empty)
            {
                Procparam.Add("@PA_SA_Ref_ID", hdnCommonID.Value);
            }

            if (txtTrancheName.Text != string.Empty)
            {
                Procparam.Add("@Tranche_ID", hdnTranche.Value);
            }

            if (txtDocumentNumber.Text != string.Empty)
            {
                Procparam.Add("@DocumentNumber", hdnDocNo.Value);
            }

            if (txtInvoice.Text != string.Empty)
            {
                Procparam.Add("@Invoice_No", hdnInvoice.Value);
            }

            Procparam.Add("@ProgramCode", ProgramCode);

            bool colModify = true;//This is to hide column grid
            bool colQuery = true;

            if (!bModify)
                colModify = false;

            FunPriBindGridWithFooter(colModify, colQuery);

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

        if (bIsNewRow)
        {
            grvTransLander.Rows[0].Visible = false;
            btnPrintRental.Visible = btnPrintAMF.Visible = false;
        }
        else
        {
            btnPrintRental.Visible = btnPrintAMF.Visible = true;
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
        if (strProgramId == "298" || strProgramId == "72")
            ObjPaging.ProSearchValue = Convert.ToString(txtLessee.Text);
        else
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

        if (txtBranchSearch.Text != string.Empty)
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
        try                                                         // casting - to use proper align       
        {
            //Changed by tamilselvan.S for Int32 to int64
            Int64 tempint = Convert.ToInt64(Convert.ToDecimal(val));                   // Try int     
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

        e.Row.Cells[0].Visible = false;

        if (e.Row.RowType == DataControlRowType.DataRow)                 // if header - then set the style dynamically.
        {
            for (int i_cellVal = 2; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[i_cellVal].Text))
                    {
                        Int32 type = 0;
                        type = FunPriTypeCast(e.Row.Cells[i_cellVal].Text);
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
            grvTransLander.DataSource = null;
            grvTransLander.Visible = false;
            ucCustomPaging.Visible = false;
            lblErrorMessage.Text =
            txtStartDateSearch.Text =
            txtEndDateSearch.Text = "";
            System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"] = null;
            System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"] = null;
            ComboBoxLOBSearch.SelectedIndex = 0;
            txtBranchSearch.Text = string.Empty;
            hdnBranchID.Value = string.Empty;
            txtDocumentNumberSearch.Text = string.Empty;
            hdnCommonID.Value = string.Empty;
            txtInvoice.Text = txtLessee.Text = txtTrancheName.Text = txtDocumentNumber.Text = string.Empty;
            hdnInvoice.Value = hdnLessee.Value = hdnDocNo.Value = hdnTranche.Value = string.Empty;
            btnPrintRental.Visible = btnPrintAMF.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }


    protected void btnPrintRental_Click(object sender, EventArgs e)
    {
        var outputStream = new MemoryStream();
        try
        {
            FunPriGenerateIIFiles();
            //object fileFormat = null;
            //object file = null;
            //object oMissing = System.Reflection.Missing.Value;
            //object readOnly = false;
            //object oFalse = false;
            //var filepaths = new List<string>();
            //string strnewFile = String.Empty;
            //string strnewFile1 = String.Empty;
            //int uni = 1;
            //System.Data.DataTable dt = new System.Data.DataTable();
            //Dictionary<string, string> Procparam;
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_Id", CompanyId.ToString());
            //Procparam.Add("@Lob_Id", ComboBoxLOBSearch.SelectedValue.ToString());
            //Procparam.Add("@Location_ID", "0");
            //Procparam.Add("@Template_Type_Code", "13");
            //dt = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);
            //if (dt.Rows.Count == 0)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Template not defined in template master");
            //    return;
            //}
            //String strHTML = String.Empty;
            //String FormattedstrHTML = String.Empty;
            //// FormattedstrHTML = PDFPageSetup.FormatHTML(dt.Rows[0]["Template_Content"].ToString());
            //FormattedstrHTML = dt.Rows[0]["Template_Content"].ToString();
            //string strContent = "<TABLE><TR><TD> " + FormattedstrHTML + "</TD></TR></TABLE>";
            //DataSet dsInterim = new DataSet();

            ////if (dtInterim.Compute("sum(Interim_rent1)", "").ToString().StartsWith("0") || dtInterim.Compute("sum(Interim_rent1)", "").ToString() == "")
            ////{
            ////    Utility.FunShowAlertMsg(this.Page, "No records Found");
            ////    return;
            ////}

            //if (dictparam != null)
            //    dictparam.Clear();
            //else
            //    dictparam = new Dictionary<string, string>();

            //if (txtLessee.Text != string.Empty)
            //{
            //    dictparam.Add("@Customer_ID", hdnLessee.Value);
            //}

            //if (txtDocumentNumberSearch.Text != string.Empty)
            //{
            //    dictparam.Add("@PA_SA_Ref_ID", hdnCommonID.Value);
            //}

            //if (txtTrancheName.Text != string.Empty)
            //{
            //    dictparam.Add("@Tranche_ID", hdnTranche.Value);
            //}

            //if (txtDocumentNumber.Text != string.Empty)
            //{
            //    dictparam.Add("@DocumentNumber", hdnDocNo.Value);
            //}

            //if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
            //    Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());

            //if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
            //    Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

            //dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            //dictparam.Add("@USER_ID", Convert.ToString(intUserID));
            //dictparam.Add("@Option", "1");
            //dsInterim = Utility.GetDataset("S3G_FUNDMGT_GETPrintInterim", dictparam);
            ////dsInterim.Tables[0].Columns.Add("Grand_Total");

            ////if (dsInterim.Tables[0].Rows.Count != 0)
            ////{
            ////    for (int i = 0; i < dsInterim.Tables[0].Rows.Count; i++)
            ////    {
            ////        
            ////    }
            ////}
            //if (dsInterim.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < dsInterim.Tables[0].Rows.Count; i++)
            //    {
            //        string strhtmlFile = (Server.MapPath(".") + "\\PDF Files\\" + "Interim_Html" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
            //        strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + "SInterim_" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc");
            //        strHTML = strContent;

            //        DataSet dsHeader = new DataSet();

            //        DataTable dtheader = new DataTable();
            //        DataTable dtdetail = new DataTable();

            //        DataView dvheader = new DataView(dsInterim.Tables[0]);

            //        //Header
            //        dvheader.RowFilter = "PASA_Ref_id = " + Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]);
            //        dsHeader.Tables.Add(dvheader.ToTable());

            //        //Detail
            //        dvheader = new DataView(dsInterim.Tables[1]);
            //        dvheader.RowFilter = "PA_SA_Ref_ID = " + Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]);
            //        dsHeader.Tables.Add(dvheader.ToTable());

            //        //dsHeader.Tables[0].Rows[0]["Grand_Total"] = Convert.ToDecimal((dsHeader.Tables[1].Compute("sum(Total)", ""))).ToString(Funsetsuffix());

            //        //dsHeader.Tables[0].Rows[0]["amount1"] = Utility.GetAmountInWords(Convert.ToDecimal(dsHeader.Tables[0].Rows[0]["Grand_Total"]));

            //        //strHTML = FunPriInterimDetails(strHTML, dsHeader);
            //        if (strHTML.Contains("~InvoiceTable~"))
            //        {
            //            strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dsHeader.Tables[1]);
            //        }
            //        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);

            //        string strImagePath = String.Empty;
            //        if (strHTML.Contains("~CompanyLogo~"))
            //        {
            //            strImagePath = Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png");
            //            strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strImagePath, strHTML);
            //        }
            //        if (strHTML.Contains("~InvoiceSignStamp~"))
            //        {
            //            strImagePath = Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png");
            //            strHTML = PDFPageSetup.FunPubBindImages("~InvoiceSignStamp~", strImagePath, strHTML);
            //        }
            //        if (strHTML.Contains("~POSignStamp~"))
            //        {
            //            strImagePath = Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png");
            //            strHTML = PDFPageSetup.FunPubBindImages("~POSignStamp~", strImagePath, strHTML);
            //        }

            //        try
            //        {
            //            if (File.Exists(strhtmlFile) == true)
            //            {
            //                File.Delete(strhtmlFile);
            //            }
            //            File.WriteAllText(strhtmlFile, strHTML);
            //            file = strhtmlFile;

            //            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            //            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
            //            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            //            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
            //                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            //            fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
            //            file = strnewFile;

            //            //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            //            //oDoc.ActiveWindow.Selection.TypeText(" \t ");
            //            //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //            //Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            //            //Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            //            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            //            //oDoc.ActiveWindow.Selection.TypeText(" / ");
            //            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            //            Microsoft.Office.Interop.Word.Range rng = null;
            //            string img = string.Empty;

            //            //if (oDoc.InlineShapes.Count >= 1)
            //            //{
            //            //    img = Server.MapPath("../Images/login/s3g_logo.png");
            //            //    rng = oDoc.InlineShapes[1].Range;
            //            //    rng.Delete();
            //            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //            //}

            //            //if (oDoc.InlineShapes.Count == 2)
            //            //{
            //            //    img = Server.MapPath("../Images/login/posign.png");
            //            //    rng = oDoc.InlineShapes[1].Range;
            //            //    rng.Delete();
            //            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //            //}
            //            //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            //            ////oDoc.ActiveWindow.Selection.TypeText(" \t ");
            //            //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //            //Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            //            //Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            //            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            //            //oDoc.ActiveWindow.Selection.TypeText(" / ");
            //            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);
            //            oDoc = PDFPageSetup.SetWordProperties(oDoc);
            //            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
            //                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            //            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            //            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            //            if (File.Exists(strhtmlFile) == true)
            //            {
            //                File.Delete(strhtmlFile);
            //            }
            //        }
            //        catch (Exception objException)
            //        {

            //        }
            //        filepaths.Add(strnewFile);
            //    }
            //}
            //else
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Select atleast one Record");
            //}
            //uni++;
            //if (filepaths.Count > 1)
            //{

            //        fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            //        string[] filesToMerge = filepaths.ToArray();
            //        string path = (Server.MapPath(".") + "\\PDF Files\\" + "MInterim_" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
            //        file = path;
            //        Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            //        Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add();
            //        Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;
            //        int temp = 0;
            //        foreach (string file1 in filesToMerge)
            //        {
            //            temp++;
            //            Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(file1);
            //            PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
            //            CurrentDocument.Range().Copy();
            //            selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
            //            if (temp != filesToMerge.Length)
            //                selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);
            //        }

            //        wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
            //            , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            //        wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
            //        wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

            //        for (int i = 0; i < filesToMerge.Length; i++)
            //        {
            //            if (File.Exists(filesToMerge[i]) == true)
            //            {
            //                File.Delete(filesToMerge[i]);
            //            }
            //        }

            //        Response.AppendHeader("content-disposition", "attachment; filename=Interim.pdf");
            //        Response.ContentType = "application/pdf";
            //        Response.WriteFile(path);

            //}
            //else
            //{

            //        fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            //        string path = (Server.MapPath(".") + "\\PDF Files\\" + "SInterim_" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
            //        file = path;
            //        Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            //        Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            //        Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;

            //        Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(strnewFile);
            //        PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
            //        CurrentDocument.Range().Copy();
            //        selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);

            //        wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
            //            , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            //        wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
            //        wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

            //        if (File.Exists(strnewFile) == true)
            //        {
            //            File.Delete(strnewFile);
            //        }

            //        Response.AppendHeader("content-disposition", "attachment; filename=Interim.pdf");
            //        Response.ContentType = "application/pdf";
            //        Response.WriteFile(path);


            //}

            //DataTable dtprint = new DataTable();
            //dtprint.Columns.Add("tranche_name");
            //List<String> list = new List<String>();

            //if (dsInterim.Tables[2].Rows.Count > 0)
            //{
            //    Guid objGuid;
            //    objGuid = Guid.NewGuid();

            //    rpd.Load(Server.MapPath("InterimBill_Cov.rpt"));

            //    DataTable DTTranche = dsInterim.Tables[2].DefaultView.ToTable(true, "Tranche_Name");

            //    for (int i = 0; i < DTTranche.Rows.Count; i++)
            //    {
            //        DataRow dr = dtprint.NewRow();

            //        DataRow DRaCC = DTTranche.Rows[i];

            //        DataRow[] DRAccDtls = dsInterim.Tables[2].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

            //        DataTable dt = dsInterim.Tables[2].Clone();

            //        if (DRAccDtls.Length > 0)
            //            dt = DRAccDtls.CopyToDataTable();

            //        rpd.SetDataSource(dt);

            //        string strFileName = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + DRaCC["Tranche_Name"].ToString() + "_Covering.pdf";

            //        string strFolder = Server.MapPath(".") + "\\PDF Files\\Interim Billing";

            //        if (!(System.IO.Directory.Exists(strFolder)))
            //        {
            //            DirectoryInfo di = Directory.CreateDirectory(strFolder);
            //        }

            //        FileInfo fl = new FileInfo(strFileName);

            //        if (fl.Exists == true)
            //        {
            //            fl.Delete();
            //        }

            //        rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
            //    }
            //}

            //if (dsInterim.Tables[0].Rows.Count > 0)
            //{

            //    DataTable DTAccounts = dsInterim.Tables[0].DefaultView.ToTable(true, "ACCOUNT_NO");
            //    rpd = new ReportDocument();
            //    rpd.Load(Server.MapPath("InterimBill.rpt"));

            //    for (int i = 0; i < DTAccounts.Rows.Count; i++)
            //    {
            //        DataRow DRaCC = DTAccounts.Rows[i];
            //        DataRow[] DRAccDtls = dsInterim.Tables[0].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
            //        DataRow[] DRAccDtls1 = dsInterim.Tables[1].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
            //        DataTable dt = dsInterim.Tables[0].Clone();
            //        DataTable dt1 = dsInterim.Tables[1].Clone();
            //        if (DRAccDtls.Length > 0)
            //            dt = DRAccDtls.CopyToDataTable();
            //        if (DRAccDtls1.Length > 0)
            //            dt1 = DRAccDtls1.CopyToDataTable();

            //        rpd.SetDataSource(dt);
            //        rpd.Subreports["Subreport"].SetDataSource(dt1);

            //        string strFileName = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + DRaCC["ACCOUNT_NO"].ToString() + ".pdf";

            //        FileInfo fl = new FileInfo(strFileName);
            //        if (fl.Exists == true)
            //        {
            //            fl.Delete();
            //        }

            //        rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

            //        fl = new FileInfo(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");

            //        if (i == 0 && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != "" && fl.Exists == true)
            //            list.Add(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");
            //        else if (i > 0 && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != "" && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != dsInterim.Tables[0].Rows[i - 1]["Tranche_Name"].ToString() && fl.Exists == true)
            //            list.Add(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");

            //        fl = new FileInfo(strFileName);

            //        if (fl.Exists == true)
            //        {
            //            list.Add(strFileName);
            //        }
            //    }
            //}

            //string[] GetAllFiles = list.ToArray();
            //if (Convert.ToInt32(GetAllFiles.Length.ToString()) > 0)
            //{
            //    CombineMultiplePDFs(GetAllFiles, Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + "Combine.pdf");
            //}

        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
            outputStream.Close();
            outputStream.Dispose(); //Upto This
        }
    }



    private void FunPriGenerateIIFiles()
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strid = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            int nDigi_Flag = 0;
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string SignedFile = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\Signed.pdf";
            string strQRCode = string.Empty;
            DataTable dt;
            DataTable dtTem;
            Dictionary<string, string> Procparam;

            DataSet dsInterim = new DataSet();

            //if (dtInterim.Compute("sum(Interim_rent1)", "").ToString().StartsWith("0") || dtInterim.Compute("sum(Interim_rent1)", "").ToString() == "")
            //{
            //    Utility.FunShowAlertMsg(this.Page, "No records Found");
            //    return;
            //}

            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            if (txtLessee.Text != string.Empty)
            {
                dictparam.Add("@Customer_ID", hdnLessee.Value);
            }

            if (txtDocumentNumberSearch.Text != string.Empty)
            {
                dictparam.Add("@PA_SA_Ref_ID", hdnCommonID.Value);
            }

            if (txtTrancheName.Text != string.Empty)
            {
                dictparam.Add("@Tranche_ID", hdnTranche.Value);
            }

            if (txtDocumentNumber.Text != string.Empty)
            {
                dictparam.Add("@DocumentNumber", hdnDocNo.Value);
            }

            if (txtInvoice.Text != string.Empty)
            {
                dictparam.Add("@Invoice_No", hdnInvoice.Value);
            }

            if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
                dictparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());

            if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
                dictparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

            dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@USER_ID", Convert.ToString(intUserID));
            dictparam.Add("@Option", "1");

            if (txtBranchSearch.Text != string.Empty)
            {
                dictparam.Add("@Location", hdnBranchID.Value);
            }

            dsInterim = Utility.GetDataset("S3G_FUNDMGT_GETPrintInterim", dictparam);

            if (dsInterim.Tables[2].Rows.Count > 0)//Covering
            {
                DataSet dss = new DataSet();
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_Id", CompanyId.ToString());
                Procparam.Add("@Lob_Id", ComboBoxLOBSearch.SelectedValue.ToString());
                Procparam.Add("@Location_ID", "0");
                //Procparam.Add("@Template_Type_Code", "54");
                if (dsInterim.Tables[0].Rows[0]["Is_Cess"].ToString() == "1")
                {
                    Procparam.Add("@Template_Type_Code", "76");
                }
                else
                {
                    Procparam.Add("@Template_Type_Code", "54");
                }
                
                dtTem = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);
                if (dtTem.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Template not defined in template master");
                    return;
                }
                String strHTML = String.Empty;
                String FormattedstrHTML = String.Empty;
                // FormattedstrHTML = PDFPageSetup.FormatHTML(dt.Rows[0]["Template_Content"].ToString());
                FormattedstrHTML = dtTem.Rows[0]["Template_Content"].ToString();
                string strContent = "<TABLE><TR><TD> " + FormattedstrHTML + "</TD></TR></TABLE>";
                string strFolderNo, strBillNo, strCustomerName, strDocumentPath,
                    strbillperiod, strnewFile, strAcocuntno, strBranchName, strtranche, strnewFile1;

                //ReportDocument rptd = new ReportDocument();
                //string ReportPath = "";
                //ReportPath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\bin\\Debug\\SISLS3GWSBilling.exe", "\\Reports\\RptBillDetails.RPT");
                //ReportPath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\bin\\Debug\\SISLS3GCommonWS.exe", "\\Reports\\Billing.RPT");
                //System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                DataTable DTTranche = dsInterim.Tables[2].DefaultView.ToTable(true, "Tranche_Name");

                //ReportPath = (string)AppReader.GetValue("BillPDFPath", typeof(string));
                //ReportPath += "Rental_Invoice.RPT";
                for (int i = 0; i < DTTranche.Rows.Count; i++)
                //foreach (DataRow DRaCC in DTTranche.Rows)
                {
                    strHTML = dtTem.Rows[0]["Template_Content"].ToString();
                    DataRow DRaCC = DTTranche.Rows[i];
                    // DataRow[] DRAccDtls = dSet.Tables[0].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'", "CashFlow");
                    DataRow[] DRAccDtls = dsInterim.Tables[2].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

                    dt = dsInterim.Tables[2].Clone();
                    if (DRAccDtls.Length > 0)
                        dt = DRAccDtls.CopyToDataTable();
                    //foreach (DataRow DR in DRAccDtls)
                    //{
                    //    dt.ImportRow(DR);
                    //}
                    if (dt.Rows.Count > 0)
                    {
                        //strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                        //strBillNo = dt.Rows[0]["Bill_Number"].ToString();
                        //strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                        //strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
                        //strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
                        //strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
                        strAcocuntno = dt.Rows[0]["Tranche_Name"].ToString();

                        strnewFile = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\";
                        //decimal decGrandTotal = 0;
                        //for (int idx = 0; idx < dt.Rows.Count; idx++)
                        //{
                        //    decGrandTotal = Convert.ToDecimal(dt.Rows[idx]["AMOUNT"].ToString()) + Convert.ToDecimal(dt.Rows[idx]["VAT"].ToString()) + Convert.ToDecimal(dt.Rows[idx]["Service_TAX"].ToString());
                        //}

                        if (i == 0)
                        {

                            if (!Directory.Exists(strnewFile))
                            {
                                Directory.CreateDirectory(strnewFile);
                            }
                        }
                        //strnewFile += "\\" + dSet.Tables[1].Rows[0]["LOCATION"].ToString().Replace("|", "").Replace(" ", "_").ToString();
                        //  if (!Directory.Exists(strnewFile))
                        //  {
                        //      Directory.CreateDirectory(strnewFile);
                        //  }
                        // strnewFile += "\\" + strAcocuntno + "_Covering.pdf";
                        strnewFile1 = strAcocuntno + "_Covering";
                        FileInfo fl = new FileInfo(strnewFile);
                        if (fl.Exists == true)
                        {
                            fl.Delete();
                        }


                        //rptd.Load(ReportPath);
                        ////TextObject TxtSub = (TextObject)rptd.ReportDefinition.ReportObjects["TxtSubject"];
                        ////TxtSub.Text = "Sub : Bill for Due of INR." + decGrandTotal.ToString() + " falling on " + dSet.Tables[0].Rows[0]["INSTALLMENT_DATE"].ToString();
                        //rptd.SetDataSource(dt);
                        ////rptd.Subreports["Subreport"].SetDataSource(dt1);
                        //rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);

                        DataSet dsHeader = new DataSet();
                        dsHeader.Tables.Add(dt);
                        //DataRow[] ObjIGST = dt.Select("IS_POS");
                        //strHTML = FunPriHeadPriDetails(strHTML, dsHeader);
                        if (dt.Rows[0]["IS_POS"].ToString() == "1")
                        {
                            if (strHTML.Contains("~InvoiceTable~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

                            if (strHTML.Contains("~InvoiceTable1~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt);
                            }
                        }
                        else
                        {
                            if (strHTML.Contains("~InvoiceTable1~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

                            if (strHTML.Contains("~InvoiceTable~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt);
                            }
                        }

                        if (dt.Rows[0]["CHK_RTGS"].ToString() == "1")
                        {
                            if (strHTML.Contains("~RTGS_Company~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Company~", strHTML);
                            if (strHTML.Contains("~RTGS_Funder~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Funder~", strHTML);

                            if (strHTML.Contains("~RTGS_Both~"))
                            {
                                strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Both~", strHTML);
                            }
                        }
                        else if (dt.Rows[0]["CHK_RTGS"].ToString() == "2")
                        {
                            if (strHTML.Contains("~RTGS_Both~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Both~", strHTML);
                            if (strHTML.Contains("~RTGS_Funder~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Funder~", strHTML);

                            if (strHTML.Contains("~RTGS_Company~"))
                            {
                                strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Company~", strHTML);
                            }
                        }
                        else if (dt.Rows[0]["CHK_RTGS"].ToString() == "3")
                        {
                            if (strHTML.Contains("~RTGS_Company~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Company~", strHTML);
                            if (strHTML.Contains("~RTGS_Both~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Both~", strHTML);

                            if (strHTML.Contains("~RTGS_Funder~"))
                            {
                                strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Funder~", strHTML);
                            }
                        }

                        FunPubGetQrCode(Convert.ToString(dt.Rows[0]["QrCode"]));

                        strQRCode = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\DCQRCode.png";

                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);
                        List<string> listImageName = new List<string>();
                        listImageName.Add("~CompanyLogo~");
                        listImageName.Add("~InvoiceSignStamp~");
                        listImageName.Add("~POSignStamp~");
                        listImageName.Add("~DCQRCode~");
                        List<string> listImagePath = new List<string>();

                        //listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                        if (Utility.StringToDate(dt.Rows[0]["print_date"].ToString()) >= Utility.StringToDate("06/06/2023"))
                        {
                            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                        }
                        else
                        {
                            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
                        }
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));
                        listImagePath.Add(strQRCode);
                        strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);

                        if (Utility.StringToDate(dt.Rows[0]["print_date"].ToString()) >= Utility.StringToDate("18/07/2023"))
                        {
                            intRegAddress = 1;
                        }
                        else
                        {
                            intRegAddress = 0;
                        }

                        //if (strHTML.Contains("~CompanyLogo~"))
                        //{
                        //    strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strHTML, dsBillingDetails.Tables[0].Rows[billidx]["Company_ID"].ToString());
                        //}
                        //if (strHTML.Contains("~InvoiceSignStamp~"))
                        //{
                        //    strHTML = PDFPageSetup.FunPubBindImages("~InvoiceSignStamp~", strHTML, dsBillingDetails.Tables[0].Rows[billidx]["Company_ID"].ToString());
                        //}
                        //if (strHTML.Contains("~OPCFooter~"))
                        //{
                        //    strHTML = PDFPageSetup.FunPubBindImages("~OPCFooter~", strHTML, dsBillingDetails.Tables[0].Rows[billidx]["Company_ID"].ToString());
                        //}
                        //PDFPageSetup.FunPubSaveDocument(strHTML, strnewFile, strnewFile1, "P");
                        FunPrintWord(strHTML, strnewFile, strnewFile1, "1", intRegAddress);
                    }
                }

                

                for (int i = 0; i < dsInterim.Tables[0].Rows.Count; i++)
                {

                    if (dsInterim.Tables[0].Rows[i]["Digi_Sign_Enable"].ToString() == "1")
                        nDigi_Flag = 1;

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", CompanyId.ToString());
                    Procparam.Add("@Lob_Id", ComboBoxLOBSearch.SelectedValue.ToString());
                    Procparam.Add("@Location_ID", "0");
                    if (dsInterim.Tables[0].Rows[0]["Is_Cess"].ToString() == "1")
                    {
                        Procparam.Add("@Template_Type_Code", "75");
                    }
                    else
                    {
                        Procparam.Add("@Template_Type_Code", "13");
                    }
                    dtTem = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);
                    if (dtTem.Rows.Count == 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Template not defined in template master");
                        return;
                    }
                    strHTML = String.Empty;
                    FormattedstrHTML = String.Empty;
                    // FormattedstrHTML = PDFPageSetup.FormatHTML(dt.Rows[0]["Template_Content"].ToString());
                    strHTML = dtTem.Rows[0]["Template_Content"].ToString();
                    strContent = "<TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";

                    DataSet dsHeader = new DataSet();
                    DataView dvheader = new DataView(dsInterim.Tables[0]);
                    strAcocuntno = dsInterim.Tables[0].Rows[i]["Account_NO"].ToString();
                    //  string FileName = PDFPageSetup.FunPubGetFileName(strid + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                    string DownFile = string.Empty;
                    FilePath = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\";
                    string FileName = strAcocuntno;
                    DownFile = FilePath + FileName + ".pdf";
                    OutputFile = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                    //Header
                    dvheader.RowFilter = "PASA_Ref_id = " + Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]);
                    dsHeader.Tables.Add(dvheader.ToTable());
                    //Detail
                    dvheader = new DataView(dsInterim.Tables[1]);
                    dvheader.RowFilter = "PA_SA_Ref_ID = " + Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]);
                    dsHeader.Tables.Add(dvheader.ToTable());

                    DataRow[] DRAccDtlsSAC = dsInterim.Tables[3].Select("ACCOUNT_NO='" + strAcocuntno + "'");
                    DataTable dtSAC = dsInterim.Tables[3].Clone();
                    if (DRAccDtlsSAC.Length > 0)
                        dtSAC = DRAccDtlsSAC.CopyToDataTable();

                    DataTable dtRCov = dsInterim.Tables[0].Clone();
                    DataTable dt1 = dsInterim.Tables[1].Clone();

                    //DataRow[] DRAccDtls = dsInterim.Tables[0].Select("PASA_Ref_id=" +
                        //Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]));

                    DataRow[] DRAccDtls = dsInterim.Tables[0].Select("ACCOUNT_NO='" + strAcocuntno + "'");

                    DataRow[] DRAccDtls1 = dsInterim.Tables[1].Select("ACCOUNT_NO='" + strAcocuntno + "'");

                    if (DRAccDtls1.Length > 0)
                        dt1 = DRAccDtls1.CopyToDataTable();

                    dtRCov = DRAccDtls.CopyToDataTable();

                    DataRow[] ObjIGSTDR = dt1.Select("InvTbl_IGST_Amount_Dbl > 0");


                    if (ObjIGSTDR.Length > 0)
                    {
                        if (strHTML.Contains("~InvoiceTable~"))
                            strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

                        if (strHTML.Contains("~InvoiceTable1~"))
                        {
                            strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt1);
                        }
                    }
                    else
                    {
                        if (strHTML.Contains("~InvoiceTable1~"))
                            strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

                        if (strHTML.Contains("~InvoiceTable~"))
                        {
                            strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt1);
                        }
                    }

                    if (strHTML.Contains("~SACTable~"))
                    {
                        strHTML = PDFPageSetup.FunPubBindTable("~SACTable~", strHTML, dtSAC);
                    }

                    

                    strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dtRCov);

                    FunPubGetQrCode(Convert.ToString(dsInterim.Tables[0].Rows[i]["QrCode"]));

                    strQRCode = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\DCQRCode.png";

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    listImageName.Add("~DCQRCode~");
                    List<string> listImagePath = new List<string>();
                    //listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    if (Utility.StringToDate(dtRCov.Rows[0]["Invoice_Date"].ToString()) >= Utility.StringToDate("06/06/2023"))
                    {
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    }
                    else
                    {
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
                    }
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));
                    listImagePath.Add(strQRCode);
                    strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                    //PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, "P");

                    if (Utility.StringToDate(dtRCov.Rows[0]["Invoice_Date"].ToString()) >= Utility.StringToDate("18/07/2023"))
                    {
                        intRegAddress = 1;
                    }
                    else
                    {
                        intRegAddress = 0;
                    }

                        if (nDigi_Flag == 1)
                        FunPrintWord(strHTML, FilePath, FileName, "1", intRegAddress);
                    else
                        FunPrintWord(strHTML, FilePath, FileName, "0", intRegAddress);

                    FileInfo fl = new FileInfo(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");

                    //if (i == 0)
                    //    Label1.Text = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf";

                    SignedFile = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\Signed\\";

                    if (!System.IO.Directory.Exists(SignedFile))
                    {
                        System.IO.Directory.CreateDirectory(SignedFile);
                    }

                    if (i == 0 && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != "" && fl.Exists == true)
                    {
                        File.Copy(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf",
                           Server.MapPath(".") + "\\PDF Files\\Interim Billing\\Signed\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf", true);

                        filepaths.Add(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\Signed\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");
                    }
                    else if (i > 0 && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != "" && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != dsInterim.Tables[0].Rows[i - 1]["Tranche_Name"].ToString() && fl.Exists == true)
                        filepaths.Add(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");
                    
                    if (nDigi_Flag == 1)
                    {
                        S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                        ObjPDFSign.DigiPDFSign(DownFile, SignedFile + Path.GetFileName(DownFile), "RIGHT");

                        filepaths.Add(SignedFile + Path.GetFileName(DownFile));
                    }
                    else
                    {
                        filepaths.Add(DownFile);
                    }
                }



                if (filepaths.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                    return;
                }
                else
                {
                    if (nDigi_Flag == 1)
                    {
                        string filePath = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\Signed.zip";

                        //before creation of compressed folder,deleting it if exists
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }


                        if (!File.Exists(filePath))
                        {
                            //creating a zip file from one folder to another folder
                            ZipFile.CreateFromDirectory(SignedFile, filePath);

                            //Delete The excel file which is created

                            string[] str = Directory.GetFiles(SignedFile);
                            foreach (string fileName in str)
                            {
                                File.Delete(fileName);
                            }

                            if (Directory.Exists(SignedFile))
                            {
                                Directory.Delete(SignedFile);
                            }
                        }
                        SignedFile = filePath;
                    }
                    else
                    {
                        FunPriGenerateFiles(filepaths, OutputFile, "P");

                        //S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                        //if (nDigi_Flag == 1)
                        //    ObjPDFSign.DigiPDFSign(OutputFile, SignedFile, "RIGHT");
                        //else
                        SignedFile = OutputFile;

                    }
                    //Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    //Response.ContentType = "application/pdf";
                    //Response.WriteFile(SignedFile);
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(SignedFile, false, 0);
                    string strScipt1 = "";
                    if (SignedFile.Contains("/File.pdf"))
                    {
                        strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + SignedFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    }
                    else
                    {
                        strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + SignedFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);

                }
            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    private void FunPrintWord(string strHTML, string strnewfile, string strnewfile1, string strIsCov, int intRegAddress)
    {
        string strhtmlFile = (strnewfile + "Bill_Html" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
        string strwordFile = string.Empty;
        string strpdfFile = string.Empty;
        string strpdfFileName = string.Empty;

        strpdfFileName = strnewfile1;
        strpdfFile = strnewfile + "\\" + strnewfile1;

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;

            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            Microsoft.Office.Interop.Word.Range rng = null;
            string img = string.Empty;

            //if (oDoc.InlineShapes.Count >= 1)
            //{
            //    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            //    string strFileName = (string)AppReader.GetValue("ImagePath", typeof(string));// @"D:\S3G\SISLS3GPLayer\SISLS3GPLayer\Config.ini";// 
            //    img = strFileName + @"\login\s3g_logo.png";
            //    rng = oDoc.InlineShapes[1].Range;
            //    rng.Delete();
            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //}

            //if (oDoc.InlineShapes.Count == 1)
            //{
            //    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            //    string strFileName = (string)AppReader.GetValue("ImagePath", typeof(string));// @"D:\S3G\SISLS3GPLayer\SISLS3GPLayer\Config.ini";// 
            //    img = strFileName + @"\Billsign.png";
            //    rng = oDoc.InlineShapes[1].Range;
            //    rng.Delete();
            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //}
            object fileFormat = null;


            fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            file = strpdfFile;

            //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            ////oDoc.ActiveWindow.Selection.TypeText(" \t ");
            //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            //Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            //oDoc.ActiveWindow.Selection.TypeText(" / ");
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            if (strIsCov == "0")
            {
                string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oDoc.ActiveWindow.Selection.Font.Size = 7;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(textDisc);
            }

            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" / ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);
            string text = "";
            //string text = "\nRegd. Office: D-16, Nelson Chambers, Nelson Manickam Road, Chennai, Tamil Nadu - 600029.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            if (intRegAddress == 0)
            {
                 text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            }
            if (intRegAddress == 1)
            {
                text = "\nRegd. Office: Ground Floor, Block No B, 809, EGA Trade Centre, Poonamallee High Road, Kilpauk, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            }
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.TypeText(text);
            //System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            //string strFileName = @"D:\S3G_OPC\OPC_Service_LIVE_GST"; //(string)AppReader.GetValue("ImagePath", typeof(string));
            //string footerimagepath = strFileName + @"\TemplateImages\1\OPCFooter.png";
            string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);


        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriGenerateIIFiles_AMF()
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strid = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            int nDigi_Flag = 0;
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string SignedFile = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\Signed.pdf";
            DataTable dt;
            DataTable dtTemp;
            Dictionary<string, string> Procparam;

            DataSet dsInterim = new DataSet();

            //if (dtInterim.Compute("sum(Interim_rent1)", "").ToString().StartsWith("0") || dtInterim.Compute("sum(Interim_rent1)", "").ToString() == "")
            //{
            //    Utility.FunShowAlertMsg(this.Page, "No records Found");
            //    return;
            //}

            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            if (txtLessee.Text != string.Empty)
            {
                dictparam.Add("@Customer_ID", hdnLessee.Value);
            }

            if (txtDocumentNumberSearch.Text != string.Empty)
            {
                dictparam.Add("@PA_SA_Ref_ID", hdnCommonID.Value);
            }

            if (txtTrancheName.Text != string.Empty)
            {
                dictparam.Add("@Tranche_ID", hdnTranche.Value);
            }

            if (txtDocumentNumber.Text != string.Empty)
            {
                dictparam.Add("@DocumentNumber", hdnDocNo.Value);
            }

            if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
                dictparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());

            if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
                dictparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

            dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@USER_ID", Convert.ToString(intUserID));
            dictparam.Add("@Option", "2");
            
            dsInterim = Utility.GetDataset("S3G_FUNDMGT_GETPrintInterim", dictparam);

            if (dsInterim.Tables[2].Rows.Count > 0)//Covering
            {
                DataSet dss = new DataSet();
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_Id", CompanyId.ToString());
                Procparam.Add("@Lob_Id", ComboBoxLOBSearch.SelectedValue.ToString());
                Procparam.Add("@Location_ID", "0");
                Procparam.Add("@Template_Type_Code", "55");
                
                dtTemp = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);
                if (dtTemp.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Template not defined in template master");
                    return;
                }
                String strHTML = String.Empty;
                String FormattedstrHTML = String.Empty;
                // FormattedstrHTML = PDFPageSetup.FormatHTML(dt.Rows[0]["Template_Content"].ToString());
                FormattedstrHTML = dtTemp.Rows[0]["Template_Content"].ToString();
                string strContent = "<TABLE><TR><TD> " + FormattedstrHTML + "</TD></TR></TABLE>";
                string strFolderNo, strBillNo, strCustomerName, strDocumentPath,
                    strbillperiod, strnewFile, strAcocuntno, strBranchName, strtranche, strnewFile1;

                //ReportDocument rptd = new ReportDocument();
                //string ReportPath = "";
                //ReportPath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\bin\\Debug\\SISLS3GWSBilling.exe", "\\Reports\\RptBillDetails.RPT");
                //ReportPath = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\bin\\Debug\\SISLS3GCommonWS.exe", "\\Reports\\Billing.RPT");
                //System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                DataTable DTTranche = dsInterim.Tables[2].DefaultView.ToTable(true, "Tranche_Name");

                //ReportPath = (string)AppReader.GetValue("BillPDFPath", typeof(string));
                //ReportPath += "Rental_Invoice.RPT";
                for (int i = 0; i < DTTranche.Rows.Count; i++)
                //foreach (DataRow DRaCC in DTTranche.Rows)
                {
                    strHTML = dtTemp.Rows[0]["Template_Content"].ToString();
                    DataRow DRaCC = DTTranche.Rows[i];
                    // DataRow[] DRAccDtls = dSet.Tables[0].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'", "CashFlow");
                    DataRow[] DRAccDtls = dsInterim.Tables[2].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

                    dt = dsInterim.Tables[2].Clone();
                    if (DRAccDtls.Length > 0)
                        dt = DRAccDtls.CopyToDataTable();
                    //foreach (DataRow DR in DRAccDtls)
                    //{
                    //    dt.ImportRow(DR);
                    //}
                    if (dt.Rows.Count > 0)
                    {
                        //strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                        //strBillNo = dt.Rows[0]["Bill_Number"].ToString();
                        //strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                        //strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
                        //strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
                        //strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
                        strAcocuntno = dt.Rows[i]["Tranche_Name"].ToString();

                        strnewFile = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\";
                        //decimal decGrandTotal = 0;
                        //for (int idx = 0; idx < dt.Rows.Count; idx++)
                        //{
                        //    decGrandTotal = Convert.ToDecimal(dt.Rows[idx]["AMOUNT"].ToString()) + Convert.ToDecimal(dt.Rows[idx]["VAT"].ToString()) + Convert.ToDecimal(dt.Rows[idx]["Service_TAX"].ToString());
                        //}

                        if (i == 0)
                        {

                            if (!Directory.Exists(strnewFile))
                            {
                                Directory.CreateDirectory(strnewFile);
                            }
                        }
                        //strnewFile += "\\" + dSet.Tables[1].Rows[0]["LOCATION"].ToString().Replace("|", "").Replace(" ", "_").ToString();
                        //  if (!Directory.Exists(strnewFile))
                        //  {
                        //      Directory.CreateDirectory(strnewFile);
                        //  }
                        // strnewFile += "\\" + strAcocuntno + "_Covering.pdf";
                        strnewFile1 = strAcocuntno + "_Covering";
                        FileInfo fl = new FileInfo(strnewFile);
                        if (fl.Exists == true)
                        {
                            fl.Delete();
                        }


                        //rptd.Load(ReportPath);
                        ////TextObject TxtSub = (TextObject)rptd.ReportDefinition.ReportObjects["TxtSubject"];
                        ////TxtSub.Text = "Sub : Bill for Due of INR." + decGrandTotal.ToString() + " falling on " + dSet.Tables[0].Rows[0]["INSTALLMENT_DATE"].ToString();
                        //rptd.SetDataSource(dt);
                        ////rptd.Subreports["Subreport"].SetDataSource(dt1);
                        //rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);

                        DataSet dsHeader = new DataSet();
                        dsHeader.Tables.Add(dt);
                        //DataRow[] ObjIGST = dt.Select("IS_POS");
                        //strHTML = FunPriHeadPriDetails(strHTML, dsHeader);
                        if (dt.Rows[0]["IS_POS"].ToString() == "1")
                        {
                            if (strHTML.Contains("~InvoiceTable~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

                            if (strHTML.Contains("~InvoiceTable1~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt);
                            }
                        }
                        else
                        {
                            if (strHTML.Contains("~InvoiceTable1~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

                            if (strHTML.Contains("~InvoiceTable~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt);
                            }
                        }

                        if (dt.Rows[0]["CHK_RTGS"].ToString() == "1")
                        {
                            if (strHTML.Contains("~RTGS_Company~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Company~", strHTML);
                            if (strHTML.Contains("~RTGS_Funder~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Funder~", strHTML);

                            if (strHTML.Contains("~RTGS_Both~"))
                            {
                                strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Both~", strHTML);
                            }
                        }
                        else if (dt.Rows[0]["CHK_RTGS"].ToString() == "2")
                        {
                            if (strHTML.Contains("~RTGS_Both~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Both~", strHTML);
                            if (strHTML.Contains("~RTGS_Funder~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Funder~", strHTML);

                            if (strHTML.Contains("~RTGS_Company~"))
                            {
                                strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Company~", strHTML);
                            }
                        }
                        else if (dt.Rows[0]["CHK_RTGS"].ToString() == "3")
                        {
                            if (strHTML.Contains("~RTGS_Company~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Company~", strHTML);
                            if (strHTML.Contains("~RTGS_Both~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Both~", strHTML);

                            if (strHTML.Contains("~RTGS_Funder~"))
                            {
                                strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Funder~", strHTML);
                            }
                        }

                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);
                        List<string> listImageName = new List<string>();
                        listImageName.Add("~CompanyLogo~");
                        listImageName.Add("~InvoiceSignStamp~");
                        listImageName.Add("~POSignStamp~");
                        List<string> listImagePath = new List<string>();
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                        strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);

                        //if (strHTML.Contains("~CompanyLogo~"))
                        //{
                        //    strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strHTML, dsBillingDetails.Tables[0].Rows[billidx]["Company_ID"].ToString());
                        //}
                        //if (strHTML.Contains("~InvoiceSignStamp~"))
                        //{
                        //    strHTML = PDFPageSetup.FunPubBindImages("~InvoiceSignStamp~", strHTML, dsBillingDetails.Tables[0].Rows[billidx]["Company_ID"].ToString());
                        //}
                        //if (strHTML.Contains("~OPCFooter~"))
                        //{
                        //    strHTML = PDFPageSetup.FunPubBindImages("~OPCFooter~", strHTML, dsBillingDetails.Tables[0].Rows[billidx]["Company_ID"].ToString());
                        //}
                        //PDFPageSetup.FunPubSaveDocument(strHTML, strnewFile, strnewFile1, "P");
                        FunPrintWord(strHTML, strnewFile, strnewFile1, "1",0);

                    }
                }


                for (int i = 0; i < dsInterim.Tables[0].Rows.Count; i++)
                {

                    if (dsInterim.Tables[0].Rows[i]["Digi_Sign_Enable"].ToString() == "1")
                        nDigi_Flag = 1;

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", CompanyId.ToString());
                    Procparam.Add("@Lob_Id", ComboBoxLOBSearch.SelectedValue.ToString());
                    Procparam.Add("@Location_ID", "0");

                    Procparam.Add("@Template_Type_Code", "56");
                    dtTemp = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);
                    if (dtTemp.Rows.Count == 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Template not defined in template master");
                        return;
                    }
                    strHTML = String.Empty;
                    FormattedstrHTML = String.Empty;
                    // FormattedstrHTML = PDFPageSetup.FormatHTML(dt.Rows[0]["Template_Content"].ToString());
                    strHTML = dtTemp.Rows[0]["Template_Content"].ToString();
                    strContent = "<TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";

                    DataSet dsHeader = new DataSet();
                    DataView dvheader = new DataView(dsInterim.Tables[0]);
                    strAcocuntno = dsInterim.Tables[0].Rows[i]["Account_NO"].ToString();
                    //  string FileName = PDFPageSetup.FunPubGetFileName(strid + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                    string DownFile = string.Empty;
                    FilePath = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\";
                    string FileName = strAcocuntno;
                    DownFile = FilePath + FileName + ".pdf";
                    OutputFile = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                    //Header
                    dvheader.RowFilter = "PASA_Ref_id = " + Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]);
                    dsHeader.Tables.Add(dvheader.ToTable());
                    //Detail
                    dvheader = new DataView(dsInterim.Tables[1]);
                    dvheader.RowFilter = "PA_SA_Ref_ID = " + Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]);
                    dsHeader.Tables.Add(dvheader.ToTable());
                    DataRow[] DRAccDtlsSAC = dsInterim.Tables[3].Select("ACCOUNT_NO='" + strAcocuntno + "'");
                    DataTable dtSAC = dsInterim.Tables[3].Clone();
                    if (DRAccDtlsSAC.Length > 0)
                        dtSAC = DRAccDtlsSAC.CopyToDataTable();

                    DataTable dtACov = dsInterim.Tables[0].Clone();
                    DataTable dt1 = dsInterim.Tables[1].Clone();

                    DataRow[] DRAccDtls = dsInterim.Tables[0].Select("ACCOUNT_NO='" + strAcocuntno + "'");
                    DataRow[] DRAccDtls1 = dsInterim.Tables[1].Select("ACCOUNT_NO='" + strAcocuntno + "'");

                    if (DRAccDtls1.Length > 0)
                        dt1 = DRAccDtls1.CopyToDataTable();

                    dtACov = DRAccDtls.CopyToDataTable();

                    DataRow[] ObjIGSTDR = dt1.Select("AMF_IGST_Amount_Dbl > 0");

                    if (ObjIGSTDR.Length > 0)
                    {
                        if (strHTML.Contains("~InvoiceTable~"))
                            strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

                        if (strHTML.Contains("~InvoiceTable1~"))
                        {
                            strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt1);
                        }
                    }
                    else
                    {
                        if (strHTML.Contains("~InvoiceTable1~"))
                            strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

                        if (strHTML.Contains("~InvoiceTable~"))
                        {
                            strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt1);
                        }
                    }

                    if (strHTML.Contains("~SACTable~"))
                    {
                        strHTML = PDFPageSetup.FunPubBindTable("~SACTable~", strHTML, dtSAC);
                    }


                    strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dtACov);

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    List<string> listImagePath = new List<string>();
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                    strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                    //PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, "P");

                    if (nDigi_Flag == 1)
                        FunPrintWord(strHTML, FilePath, FileName, "1",0);
                    else
                        FunPrintWord(strHTML, FilePath, FileName, "0",0);

                    FileInfo fl = new FileInfo(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");

                    if (i == 0 && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != "" && fl.Exists == true)
                        filepaths.Add(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");
                    else if (i > 0 && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != "" && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != dsInterim.Tables[0].Rows[i - 1]["Tranche_Name"].ToString() && fl.Exists == true)
                        filepaths.Add(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");


                    SignedFile = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\Signed\\";

                    if (!System.IO.Directory.Exists(SignedFile))
                    {
                        System.IO.Directory.CreateDirectory(SignedFile);
                    }


                    if (nDigi_Flag == 1)
                    {
                        S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                        ObjPDFSign.DigiPDFSign(DownFile, SignedFile + Path.GetFileName(DownFile), "RIGHT");

                        filepaths.Add(SignedFile + Path.GetFileName(DownFile));
                    }
                    else
                    {
                        filepaths.Add(DownFile);
                    }

                }



                if (filepaths.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                    return;
                }
                else
                {
                    if (nDigi_Flag == 1)
                    {
                        string filePath = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\Signed.zip";

                        //before creation of compressed folder,deleting it if exists
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }


                        if (!File.Exists(filePath))
                        {
                            //creating a zip file from one folder to another folder
                            ZipFile.CreateFromDirectory(SignedFile, filePath);

                            //Delete The excel file which is created

                            string[] str = Directory.GetFiles(SignedFile);
                            foreach (string fileName in str)
                            {
                                File.Delete(fileName);
                            }

                            if (Directory.Exists(SignedFile))
                            {
                                Directory.Delete(SignedFile);
                            }
                        }
                        SignedFile = filePath;
                    }
                    else
                    {
                        FunPriGenerateFiles(filepaths, OutputFile, "P");

                        //S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                        //if (nDigi_Flag == 1)
                        //    ObjPDFSign.DigiPDFSign(OutputFile, SignedFile, "RIGHT");
                        //else
                        SignedFile = OutputFile;

                    }

                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(SignedFile, false, 0);
                    string strScipt1 = "";
                    if (SignedFile.Contains("/File.pdf"))
                    {
                        strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + SignedFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    }
                    else
                    {
                        strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + SignedFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);

                    //Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    //Response.ContentType = "application/pdf";
                    //Response.WriteFile(SignedFile);


                }
            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }


    private void FunPriGenerateFiles(List<string> filepaths, string OutputFile, string DocumentType)
    {
        try
        {
            object fileFormat = null;
            object file = null;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string[] filesToMerge = filepaths.ToArray();

            if (DocumentType == "P")
            {
                PDFPageSetup.MergePDFs(filesToMerge, OutputFile);

                for (int i = 0; i < filesToMerge.Length; i++)
                {
                    if (File.Exists(filesToMerge[i]) == true)
                    {
                        File.Delete(filesToMerge[i]);
                    }
                }
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = OutputFile;
                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;
                int temp = 0;
                foreach (string file1 in filesToMerge)
                {
                    temp++;
                    Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(file1);
                    PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
                    CurrentDocument.Range().Copy();
                    selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
                    if (temp != filesToMerge.Length)
                        selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);
                }
                wordDocument.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
                wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

                for (int i = 0; i < filesToMerge.Length; i++)
                {
                    if (File.Exists(filesToMerge[i]) == true)
                    {
                        File.Delete(filesToMerge[i]);
                    }
                }
            }

        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print");
        }
    }


    protected void btnPrintAMF_Click(object sender, EventArgs e)
    {

        FunPriGenerateIIFiles_AMF();

        //var outputStream = new MemoryStream();
        ////dtInterim = (DataTable)ViewState["dtInterim"];
        ////decimal decAMF;
        ////decAMF = Convert.ToDecimal(dtInterim.Compute("sum(Interim_rent_amf1)", "").ToString());

        ////if (decAMF == 0)
        ////{
        ////    Utility.FunShowAlertMsg(this.Page, "No records Found");
        ////    return;
        ////}

        //DataSet dsInterim = new DataSet();

        //if (dictparam != null)
        //    dictparam.Clear();
        //else
        //    dictparam = new Dictionary<string, string>();

        //if (txtLessee.Text != string.Empty)
        //{
        //    dictparam.Add("@Customer_ID", hdnLessee.Value);
        //}

        //if (txtDocumentNumberSearch.Text != string.Empty)
        //{
        //    dictparam.Add("@PA_SA_Ref_ID", hdnCommonID.Value);
        //}

        //if (txtTrancheName.Text != string.Empty)
        //{
        //    dictparam.Add("@Tranche_ID", hdnTranche.Value);
        //}

        //if (txtDocumentNumber.Text != string.Empty)
        //{
        //    dictparam.Add("@DocumentNumber", hdnDocNo.Value);
        //}

        //dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
        //dictparam.Add("@USER_ID", Convert.ToString(UserId));
        //dictparam.Add("@Option", "2");
        //dsInterim = Utility.GetDataset("S3G_FUNDMGT_GETPrintInterim", dictparam);

        //decimal decAMF = 0;
        //if (dsInterim.Tables[1].Rows.Count > 0)
        //    decAMF = Convert.ToDecimal(dsInterim.Tables[1].Compute("sum(install_amount)", "").ToString());

        //if (decAMF == 0)
        //{
        //    Utility.FunShowAlertMsg(this.Page, "No records Found");
        //    return;
        //}

        //DataTable dtprint = new DataTable();
        //dtprint.Columns.Add("tranche_name");
        //List<String> list = new List<String>();

        //if (dsInterim.Tables[2].Rows.Count > 0)
        //{
        //    Guid objGuid;
        //    objGuid = Guid.NewGuid();

        //    rpd.Load(Server.MapPath("InterimBill_AMF_Cov.rpt"));

        //    DataTable DTTranche = dsInterim.Tables[2].DefaultView.ToTable(true, "Tranche_Name");

        //    for (int i = 0; i < DTTranche.Rows.Count; i++)
        //    {
        //        DataRow dr = dtprint.NewRow();

        //        DataRow DRaCC = DTTranche.Rows[i];

        //        DataRow[] DRAccDtls = dsInterim.Tables[2].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

        //        DataTable dt = dsInterim.Tables[2].Clone();

        //        if (DRAccDtls.Length > 0)
        //            dt = DRAccDtls.CopyToDataTable();

        //        rpd.SetDataSource(dt);

        //        string strFileName = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + DRaCC["Tranche_Name"].ToString() + "_Covering.pdf";

        //        string strFolder = Server.MapPath(".") + "\\PDF Files\\Interim Billing";

        //        if (!(System.IO.Directory.Exists(strFolder)))
        //        {
        //            DirectoryInfo di = Directory.CreateDirectory(strFolder);
        //        }

        //        FileInfo fl = new FileInfo(strFileName);

        //        if (fl.Exists == true)
        //        {
        //            fl.Delete();
        //        }

        //        rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
        //    }
        //}

        //if (dsInterim.Tables[0].Rows.Count > 0)
        //{
        //    DataTable DTAccounts = dsInterim.Tables[0].DefaultView.ToTable(true, "ACCOUNT_NO");
        //    rpd = new ReportDocument();
        //    rpd.Load(Server.MapPath("InterimBill_AMF.rpt"));

        //    for (int i = 0; i < DTAccounts.Rows.Count; i++)
        //    {
        //        DataRow DRaCC = DTAccounts.Rows[i];
        //        DataRow[] DRAccDtls = dsInterim.Tables[0].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
        //        DataRow[] DRAccDtls1 = dsInterim.Tables[1].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
        //        DataTable dt = dsInterim.Tables[0].Clone();
        //        DataTable dt1 = dsInterim.Tables[1].Clone();
        //        if (DRAccDtls.Length > 0)
        //            dt = DRAccDtls.CopyToDataTable();
        //        if (DRAccDtls1.Length > 0)
        //            dt1 = DRAccDtls1.CopyToDataTable();

        //        rpd.SetDataSource(dt);
        //        rpd.Subreports["Subreport"].SetDataSource(dt1);

        //        string strFileName = Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + DRaCC["ACCOUNT_NO"].ToString() + ".pdf";

        //        FileInfo fl = new FileInfo(strFileName);
        //        if (fl.Exists == true)
        //        {
        //            fl.Delete();
        //        }

        //        rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

        //        fl = new FileInfo(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");

        //        if (i == 0 && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != "" && fl.Exists == true)
        //            list.Add(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");
        //        else if (i > 0 && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != "" && dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() != dsInterim.Tables[0].Rows[i - 1]["Tranche_Name"].ToString() && fl.Exists == true)
        //            list.Add(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + dsInterim.Tables[0].Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf");

        //        fl = new FileInfo(strFileName);

        //        if (fl.Exists == true)
        //        {
        //            list.Add(strFileName);
        //        }
        //    }
        //}

        //string[] GetAllFiles = list.ToArray();
        //if (Convert.ToInt32(GetAllFiles.Length.ToString()) > 0)
        //{
        //    CombineMultiplePDFs(GetAllFiles, Server.MapPath(".") + "\\PDF Files\\Interim Billing\\" + "Combine.pdf");
        //}

        //if (dsInterim.Tables[0].Rows.Count > 0)
        //{
        //    Guid objGuid;
        //    objGuid = Guid.NewGuid();

        //    rpd.Load(Server.MapPath("BillReg_AMF_Cov.rpt"));


        //    rpd.SetDataSource(dsInterim.Tables[2]);
        //    rpd.Subreports["BillReg_AMF.rpt"].SetDataSource(dsInterim.Tables[0]);
        //    rpd.Subreports["BillRegAMF_Sub"].SetDataSource(dsInterim.Tables[1]);


        //    string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + objGuid.ToString() + ".pdf";

        //    string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

        //    if (!(System.IO.Directory.Exists(strFolder)))
        //    {
        //        DirectoryInfo di = Directory.CreateDirectory(strFolder);

        //    }

        //    rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

        //    System.IO.FileStream fs = new System.IO.FileStream(strFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //    byte[] ar = new byte[(int)fs.Length];
        //    fs.Read(ar, 0, (int)fs.Length);
        //    fs.Close();

        //    var pdfReader = new PdfReader(strFileName);
        //    var pdfStamper = new PdfStamper(pdfReader, outputStream);
        //    var writer = pdfStamper.Writer;
        //    pdfStamper.Close();
        //    var content = outputStream.ToArray();
        //    outputStream.Close();
        //    Response.AppendHeader("content-disposition", "attachment;filename=" + dsInterim.Tables[0].Rows[0]["tranche_name"].ToString() + ".pdf");
        //    Response.ContentType = "application/octectstream";
        //    Response.BinaryWrite(content);
        //    Response.End();
        //    outputStream.Close();
        //    outputStream.Dispose();

        //}
    }


    /// <summary>
    /// If there is more than one Document Number - then use this DDL
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlMultipleDNC_SelectedIndexChanged(object sender, EventArgs e)
    {

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
        ddlMultipleDNC_SelectedIndexChanged(sender, e);     // to Load Multiple DNC
    }

    protected void Journal_OnCheckedChanged(object sender, EventArgs e)
    {
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
            default:
                ddlMultipleDNC_SelectedIndexChanged(sender, e);     // To load Multiple DNC    
                break;
        }
    }
    #endregion

    public void CombineMultiplePDFs(string[] fileNames, string outFile)
    {
        // step 1: creation of a document-object
        Document document = new Document();

        // step 2: we create a writer that listens to the document
        PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
        if (writer == null)
        {
            return;
        }

        // step 3: we open the document
        document.Open();

        foreach (string fileName in fileNames)
        {
            // we create a reader for a certain document
            PdfReader reader = new PdfReader(fileName);
            reader.ConsolidateNamedDestinations();

            // step 4: we add content
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                PdfImportedPage page = writer.GetImportedPage(reader, i);
                writer.AddPage(page);
            }

            PRAcroForm form = reader.AcroForm;
            if (form != null)
            {
                writer.CopyAcroForm(reader);
            }
            reader.Close();
        }

        // step 5: we close the document and writer
        writer.Close();
        document.Close();
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(outFile, false, 0);
        string strScipt1 = "";
        if (outFile.Contains("/File.pdf"))
        {
            strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + outFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
        }
        else
        {
            strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + outFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
    }

    private void FunPriQueryString()
    {
        switch (ProgramCode)
        {
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
    }

    protected void txtEndDateSearch_TextChanged(object sender, EventArgs e)
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
    }

    //Add by chandru on 23/04/2012
    #region CommonWebmethod
    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>

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

    protected void txtDocumentNumber_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnDocNo.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtDocumentNumber.Text = string.Empty;
                hdnDocNo.Value = string.Empty;
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
        string strSp_Name = "S3G_GETCustomers";
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

    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@Option", "9");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Option", "3");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetRSList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Option", "4");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetDocumentList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Option", "5");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    private void FunPubGetQrCode(string ReferenceNumber)
    {
        try
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ReferenceNumber, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);

            qrCodeImage.Save(Server.MapPath(".") + "\\PDF Files\\Interim Billing\\DCQRCode.png");
        }
        catch (Exception ex)
        {

        }
    }

    // Added on 19Jun215 Ends here
}
#endregion
