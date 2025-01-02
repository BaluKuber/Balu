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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using QRCoder;
using System.Drawing;
using System.IO.Compression;
#endregion


public partial class LoanAdmin_S3G_LAD_StockTransfer_View : ApplyThemeForProject
{
    # region Programs Code

    #endregion
    //Add by chandru
    static bool strRbtnJournal = false;
    string DownFile = "";
    string strScipt1 = "";
    //Add by chandru
    #region Common Variables
    int intCreate = 0;                                                         // intCreate = 1 then display the create button, else invisible
    int intQuery = 0;                                                          // intQuery = 1 then display the Query button, else invisible
    int intModify = 0;                                                         // intModify = 1 then display the Modify button, else invisible
    int intMultipleDNC = 0;                                                    // Allow the user to select the DNC dynamically.
    int intDNCOption = 0;                                                      // Allow the user to select the further option depend on the DNC - eg: approved,unapproved etc...
    string strNote_id = "";
    int nDigi_Flag = 0;
    string SignedFile = "";
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
    public static LoanAdmin_S3G_LAD_StockTransfer_View obj_Page;

    //ReportDocument rpd = new ReportDocument();

    Dictionary<string, string> dictparam;

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
            //RFVComboLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
            //RFVComboBranch.ErrorMessage = ValidationMsgs.S3G_ValMsg_Branch;

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

            btnPrint.Visible = false;

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
        strProgramId = "319";
        FunPubIsVisible(true, true, false);
        FunPubIsMandatory(false, false, false, false);
        this.Title = "S3G - Stock Transfer";
        lblHeading.Text = "Stock Transfer View";
        strRedirectPage = "~/LoanAdmin/S3G_LAD_StockTransfer.aspx";
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
        //RFVComboBranch.Enabled = isBranchMandatory;
        //RFVComboLOB.Enabled = isLOBMandatory;
        // To change the Label style to Non mandatory
        lblLOBSearch.CssClass = (isLOBMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel";
        lblBranchSearch.CssClass = (isBranchMandatory) ? "styleReqFieldLabel" : "styleDisplayLabel"; ;
    }

    private void FunPubIsMandatory(bool isLOBMandatory, bool isBranchMandatory, bool isStartDateMandatory, bool isEndDateMandatory)
    {
        // To make the LOB and Branch Non-Mandatory
        //RFVComboBranch.Enabled = (isBranchMandatory) ? true : false;
        //RFVComboLOB.Enabled = (isLOBMandatory) ? true : false;
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
        //RFVComboBranch.Enabled = isBranch;
        //RFVComboLOB.Enabled = isLOB;
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
                    ComboBoxLOBSearch.SelectedValue = "3";
                    ComboBoxLOBSearch.ClearDropDownList();

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
                Procparam.Add("@Panum", hdnCommonID.Value);
            }

            if (txtTrancheName.Text != string.Empty)
            {
                Procparam.Add("@Invoice_No", hdnTranche.Value);
            }

            if (txtDocumentNumber.Text != string.Empty)
            {
                Procparam.Add("@DocumentNumber", hdnDocNo.Value);
            }

            Procparam.Add("@ProgramCode", ProgramCode);

            if (ddlTransferType.SelectedValue != "0")
                Procparam.Add("@Transfer_Type", ddlTransferType.SelectedValue);
            // Added for RS status code starts
            if (ddlStatus.SelectedValue != "0")
                Procparam.Add("@RS_Status", ddlStatus.SelectedValue);
            //Added for RS status code ends 
            
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
            btnPrint.Visible = false;
            pnlPrintDetails.Visible = false;
        }
        else
        {
            btnPrint.Visible = true;
            //pnlPrintDetails.Visible = true;
            btnExport.Visible = true;
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
        ucCustomPaging.Visible = true;


    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            if (txtLessee.Text != string.Empty)
            {
                dictparam.Add("@Customer_ID", hdnLessee.Value);
            }

            if (txtDocumentNumber.Text != string.Empty)
            {
                dictparam.Add("@Stock_Hdr_ID", hdnDocNo.Value);
            }

            if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
                dictparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());

            if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
                dictparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

            dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@USER_ID", Convert.ToString(intUserID));

            if (ddlTransferType.SelectedValue != "0")
                dictparam.Add("@Transfer_Type", ddlTransferType.SelectedValue);

            DataTable dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_LAD_Get_Stock_Export", dictparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtTable;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            GridView grv1 = new GridView();
            DataTable dtHeader = new DataTable();
            dtHeader.Columns.Add("Column1");
            dtHeader.Columns.Add("Column2");

            DataRow row = dtHeader.NewRow();
            row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();
            row["Column1"] = "Stock Transfer Report";
            dtHeader.Rows.Add(row);
            row = dtHeader.NewRow();
            dtHeader.Rows.Add(row);
            grv1.DataSource = dtHeader;
            grv1.DataBind();
            grv1.HeaderRow.Visible = false;
            grv1.GridLines = GridLines.None;

            grv1.Rows[0].Cells[0].ColumnSpan = 12;
            grv1.Rows[1].Cells[0].ColumnSpan = 12;
            grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
            grv1.Font.Bold = true;
            grv1.ForeColor = System.Drawing.Color.DarkBlue;
            grv1.Font.Name = "calibri";
            grv1.Font.Size = 10;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=StockTransferReport.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grv1.RenderControl(htw);
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            for (int i_cellVal = 2; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                e.Row.Cells[i_cellVal].CssClass = "styleGridHeader";
            }

        }

        e.Row.Cells[4].Visible = false;
        e.Row.Cells[10].Visible = false;
        e.Row.Cells[11].Visible = false;


        if (e.Row.RowType == DataControlRowType.DataRow)                 // if header - then set the style dynamically.
        {
            //CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectRS");
            //chkSelectAccount.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvTransLander.ClientID + "','chkSelectAllRS','chkSelectRS');");

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

            case "modify":
                Response.Redirect(strRedirectPage + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
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
            txtLessee.Text = txtTrancheName.Text = txtDocumentNumber.Text = string.Empty;
            hdnLessee.Value = hdnDocNo.Value = hdnTranche.Value = string.Empty;
            btnPrint.Visible = false;
            pnlPrintDetails.Visible = false;

            ProPageNumRW = 1;                                                           // to set the default page number
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));
            txtPageSize.Text = Convert.ToString(ProPageSizeRW);
            PageAssignValue obj = new PageAssignValue(this.AssignValue);

            btnExport.Visible = false;
            ddlTransferType.SelectedValue = "0";
        }
        catch (Exception ex)
        {

        }
    }

    //added for print
    private string FunPriInterimDetails(string str, DataSet dsHeader)
    {
        try
        {
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string row = "";
            string newtr = String.Empty;

            DataRow dr = dsHeader.Tables[0].NewRow();
            foreach (DataColumn dcol in dsHeader.Tables[0].Columns)
            {
                dr = dsHeader.Tables[0].Rows[0];
                string ColName1 = string.Empty;
                ColName1 = "~" + dcol.ColumnName + "~";
                if (str.Contains(ColName1))
                    str = str.Replace(ColName1, dr[dcol].ToString());
            }

            startTag = "<tr id='row5'>";
            endTag = "</tr>";
            startIndex = str.IndexOf(startTag) + startTag.Length;
            endIndex = str.IndexOf(endTag, startIndex);
            row = str.Substring(startIndex, endIndex - startIndex);
            for (int i = 0; i < dsHeader.Tables[1].Rows.Count; i++)
            {
                string tr = row;
                tr = "<tr id='row5'>" + tr + "</tr>";
                foreach (DataColumn dcol in dsHeader.Tables[1].Columns)
                {
                    dr = dsHeader.Tables[1].Rows[i];
                    string ColName1 = string.Empty;
                    ColName1 = "~" + dcol.ColumnName + "~";
                    if (tr.Contains(ColName1))
                        tr = tr.Replace(ColName1, dr[dcol].ToString());
                }
                newtr = newtr + " " + tr;
            }
            string replacetr = row;
            replacetr = "<tr id='row5'>" + replacetr + "</tr>";
            str = str.Replace(replacetr, newtr);

            //startTag = "<tr id='row5'>";
            //endTag = "</tr>";
            //startIndex = str.IndexOf(startTag) + startTag.Length;
            //endIndex = str.IndexOf(endTag, startIndex);
            //row = str.Substring(startIndex, endIndex - startIndex);
            //newtr = String.Empty;
            //for (int i = 0; i < dsHeader.Tables[2].Rows.Count; i++)
            //{
            //    string tr = row;
            //    tr = "<tr id='row5'>" + tr + "</tr>";
            //    foreach (DataColumn dcol in dsHeader.Tables[2].Columns)
            //    {
            //        dr = dsHeader.Tables[2].Rows[i];
            //        string ColName1 = string.Empty;
            //        ColName1 = "~" + dcol.ColumnName + "~";
            //        if (tr.Contains(ColName1))
            //            tr = tr.Replace(ColName1, dr[dcol].ToString());
            //    }
            //    newtr = newtr + " " + tr;
            //}
            //replacetr = row;
            //replacetr = "<tr id='row5'>" + replacetr + "</tr>";
            //str = str.Replace(replacetr, newtr);
            return str;
        }
        catch (Exception ex)
        {
            throw ex;
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

    //protected void btnPrint_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        object fileFormat = null;
    //        object file = null;
    //        object oMissing = System.Reflection.Missing.Value;
    //        object readOnly = false;
    //        object oFalse = false;
    //        var filepaths = new List<string>();
    //        string strnewFile = String.Empty;
    //        string strnewFile1 = String.Empty;
    //        int uni = 1;
    //        System.Data.DataTable dt = new System.Data.DataTable();
    //        Dictionary<string, string> Procparam;
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_Id", CompanyId.ToString());
    //        Procparam.Add("@Lob_Id", "0");
    //        Procparam.Add("@Location_ID", "0");
    //        Procparam.Add("@Template_Type_Code", "13");
    //        dt = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);
    //        if (dt.Rows.Count == 0)
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "Template not defined in template master");
    //            return;
    //        }
    //        String strHTML = String.Empty;
    //        String FormattedstrHTML = String.Empty;
    //        // FormattedstrHTML = PDFPageSetup.FormatHTML(dt.Rows[0]["Template_Content"].ToString());
    //        FormattedstrHTML = dt.Rows[0]["Template_Content"].ToString();
    //        string strContent = "<TABLE><TR><TD> " + FormattedstrHTML + "</TD></TR></TABLE>";
    //        DataSet dsInterim = new DataSet();
    //        if (dictparam != null)
    //            dictparam.Clear();
    //        else
    //            dictparam = new Dictionary<string, string>();

    //        if (txtLessee.Text != string.Empty)
    //        {
    //            dictparam.Add("@Customer_ID", hdnLessee.Value);
    //        }

    //        if (txtDocumentNumberSearch.Text != string.Empty)
    //        {
    //            dictparam.Add("@PA_SA_Ref_ID", hdnCommonID.Value);
    //        }

    //        if (txtTrancheName.Text != string.Empty)
    //        {
    //            dictparam.Add("@Tranche_ID", hdnTranche.Value);
    //        }

    //        if (txtDocumentNumber.Text != string.Empty)
    //        {
    //            dictparam.Add("@DocumentNumber", hdnDocNo.Value);
    //        }

    //        if (!(string.IsNullOrEmpty(txtStartDateSearch.Text)))
    //            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDateSearch.Text).ToString());

    //        if (!(string.IsNullOrEmpty(txtEndDateSearch.Text)))
    //            Procparam.Add("@EndDate", Utility.StringToDate(txtEndDateSearch.Text).ToString());

    //        dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
    //        dictparam.Add("@USER_ID", Convert.ToString(intUserID));
    //        dsInterim = Utility.GetDataset("S3G_FUNDMGT_GETPrintInterim", dictparam);
    //        dsInterim.Tables[0].Columns.Add("Grand_Total");

    //        //if (dsInterim.Tables[0].Rows.Count != 0)
    //        //{
    //        //    for (int i = 0; i < dsInterim.Tables[0].Rows.Count; i++)
    //        //    {
    //        //        
    //        //    }
    //        //}
    //        if (dsInterim.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dsInterim.Tables[0].Rows.Count; i++)
    //            {
    //                string strhtmlFile = (Server.MapPath(".") + "\\PDF Files\\" + "Interim_Html" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
    //                strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + "SInterim_" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc");
    //                strHTML = strContent;

    //                DataSet dsHeader = new DataSet();

    //                DataTable dtheader = new DataTable();
    //                DataTable dtdetail = new DataTable();

    //                DataView dvheader = new DataView(dsInterim.Tables[0]);

    //                //Header
    //                dvheader.RowFilter = "PASA_Ref_id = " + Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]);
    //                dsHeader.Tables.Add(dvheader.ToTable());

    //                //Detail
    //                dvheader = new DataView(dsInterim.Tables[1]);
    //                dvheader.RowFilter = "PA_SA_Ref_ID = " + Convert.ToString(dsInterim.Tables[0].Rows[i]["PASA_Ref_id"]);
    //                dsHeader.Tables.Add(dvheader.ToTable());

    //                //dsHeader.Tables[0].Rows[0]["Grand_Total"] = Convert.ToDecimal((dsHeader.Tables[1].Compute("sum(Total)", ""))).ToString(Funsetsuffix());

    //                //dsHeader.Tables[0].Rows[0]["amount1"] = Utility.GetAmountInWords(Convert.ToDecimal(dsHeader.Tables[0].Rows[0]["Grand_Total"]));

    //                //strHTML = FunPriInterimDetails(strHTML, dsHeader);
    //                if (strHTML.Contains("~InvoiceTable~"))
    //                {
    //                    strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dsHeader.Tables[1]);
    //                }
    //                strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);

    //                string strImagePath = String.Empty;
    //                if (strHTML.Contains("~CompanyLogo~"))
    //                {
    //                    strImagePath = Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png");
    //                    strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strImagePath, strHTML);
    //                }
    //                if (strHTML.Contains("~InvoiceSignStamp~"))
    //                {
    //                    strImagePath = Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png");
    //                    strHTML = PDFPageSetup.FunPubBindImages("~InvoiceSignStamp~", strImagePath, strHTML);
    //                }
    //                if (strHTML.Contains("~POSignStamp~"))
    //                {
    //                    strImagePath = Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png");
    //                    strHTML = PDFPageSetup.FunPubBindImages("~POSignStamp~", strImagePath, strHTML);
    //                }

    //                try
    //                {
    //                    if (File.Exists(strhtmlFile) == true)
    //                    {
    //                        File.Delete(strhtmlFile);
    //                    }
    //                    File.WriteAllText(strhtmlFile, strHTML);
    //                    file = strhtmlFile;

    //                    Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
    //                    Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
    //                    oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

    //                    oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
    //                        , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

    //                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
    //                    file = strnewFile;

    //                    //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
    //                    //oDoc.ActiveWindow.Selection.TypeText(" \t ");
    //                    //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
    //                    //Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
    //                    //Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
    //                    //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
    //                    //oDoc.ActiveWindow.Selection.TypeText(" / ");
    //                    //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

    //                    Microsoft.Office.Interop.Word.Range rng = null;
    //                    string img = string.Empty;

    //                    //if (oDoc.InlineShapes.Count >= 1)
    //                    //{
    //                    //    img = Server.MapPath("../Images/login/s3g_logo.png");
    //                    //    rng = oDoc.InlineShapes[1].Range;
    //                    //    rng.Delete();
    //                    //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
    //                    //}

    //                    //if (oDoc.InlineShapes.Count == 2)
    //                    //{
    //                    //    img = Server.MapPath("../Images/login/posign.png");
    //                    //    rng = oDoc.InlineShapes[1].Range;
    //                    //    rng.Delete();
    //                    //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
    //                    //}
    //                    //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
    //                    ////oDoc.ActiveWindow.Selection.TypeText(" \t ");
    //                    //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
    //                    //Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
    //                    //Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
    //                    //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
    //                    //oDoc.ActiveWindow.Selection.TypeText(" / ");
    //                    //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);
    //                    oDoc = PDFPageSetup.SetWordProperties(oDoc);
    //                    oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
    //                        , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

    //                    oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
    //                    oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
    //                    if (File.Exists(strhtmlFile) == true)
    //                    {
    //                        File.Delete(strhtmlFile);
    //                    }
    //                }
    //                catch (Exception objException)
    //                {

    //                }
    //                filepaths.Add(strnewFile);
    //            }
    //        }
    //        else
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "Select atleast one Record");
    //        }
    //        uni++;
    //        if (filepaths.Count > 1)
    //        {
    //            if (ddlPrintType.SelectedValue == "P")
    //            {
    //                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
    //                string[] filesToMerge = filepaths.ToArray();
    //                string path = (Server.MapPath(".") + "\\PDF Files\\" + "MInterim_" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
    //                file = path;
    //                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
    //                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add();
    //                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;
    //                int temp = 0;
    //                foreach (string file1 in filesToMerge)
    //                {
    //                    temp++;
    //                    Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(file1);
    //                    PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
    //                    CurrentDocument.Range().Copy();
    //                    selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
    //                    if (temp != filesToMerge.Length)
    //                        selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);
    //                }

    //                wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
    //                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
    //                wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
    //                wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

    //                for (int i = 0; i < filesToMerge.Length; i++)
    //                {
    //                    if (File.Exists(filesToMerge[i]) == true)
    //                    {
    //                        File.Delete(filesToMerge[i]);
    //                    }
    //                }

    //                Response.AppendHeader("content-disposition", "attachment; filename=Interim.pdf");
    //                Response.ContentType = "application/pdf";
    //                Response.WriteFile(path);
    //            }
    //            else if (ddlPrintType.SelectedValue == "W")
    //            {
    //                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
    //                string[] filesToMerge = filepaths.ToArray();
    //                string path = (Server.MapPath(".") + "\\PDF Files\\" + "MInterim_" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc");
    //                file = path;
    //                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
    //                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
    //                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;
    //                int temp = 0;
    //                foreach (string file1 in filesToMerge)
    //                {
    //                    temp++;
    //                    Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(file1);
    //                    PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
    //                    CurrentDocument.Range().Copy();
    //                    selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
    //                    if (temp != filesToMerge.Length)
    //                        selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);
    //                }
    //                wordDocument.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
    //                wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
    //                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
    //                wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
    //                wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

    //                for (int i = 0; i < filesToMerge.Length; i++)
    //                {
    //                    if (File.Exists(filesToMerge[i]) == true)
    //                    {
    //                        File.Delete(filesToMerge[i]);
    //                    }
    //                }

    //                Response.AppendHeader("content-disposition", "attachment; filename=Interim.doc");
    //                Response.ContentType = "application/vnd.ms-word";
    //                Response.WriteFile(path);
    //            }
    //        }
    //        else
    //        {
    //            if (ddlPrintType.SelectedValue == "P")
    //            {
    //                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
    //                string path = (Server.MapPath(".") + "\\PDF Files\\" + "SInterim_" + UserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
    //                file = path;
    //                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
    //                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
    //                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;

    //                Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(strnewFile);
    //                PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
    //                CurrentDocument.Range().Copy();
    //                selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);

    //                wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
    //                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
    //                wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
    //                wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

    //                if (File.Exists(strnewFile) == true)
    //                {
    //                    File.Delete(strnewFile);
    //                }

    //                Response.AppendHeader("content-disposition", "attachment; filename=Interim.pdf");
    //                Response.ContentType = "application/pdf";
    //                Response.WriteFile(path);
    //            }
    //            else if (ddlPrintType.SelectedValue == "W")
    //            {
    //                Response.AppendHeader("content-disposition", "attachment; filename=Interim.doc");
    //                Response.ContentType = "application/vnd.ms-word";
    //                Response.WriteFile(strnewFile);
    //            }

    //        }
    //    }
    //    catch (System.IO.IOException ex)
    //    {
    //        Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print Interim");

    //    }
    //}

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string strHTML = string.Empty;
            string strHTML1 = string.Empty;

            if (ddlTransferType.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Select the Transfer Type");
                return;
            }

            foreach (GridViewRow grv in grvTransLander.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = grv.FindControl("chkSelectRS") as CheckBox;

                    if (chkSelect.Checked)
                    {
                        ImageButton imgQuery = grv.FindControl("imgbtnQuery") as ImageButton;

                        strNote_id = imgQuery.CommandArgument;
                    }
                }
            }

            if (ddlTransferType.SelectedValue == "1" & Convert.ToUInt32(strNote_id) > 635)
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyID, 0, 0, 58, Convert.ToString(strNote_id));
            else if (ddlTransferType.SelectedValue == "1" & Convert.ToUInt32(strNote_id) <= 635)
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyID, 0, 0, 63, Convert.ToString(strNote_id));
            else
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyID, 0, 0, 61, Convert.ToString(strNote_id));

            if (strHTML == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined");
                return;
            }

            strHTML1 = strHTML;

            if (grvTransLander.Rows.Count > 0)
            {
                CheckBox chkSelectAll = grvTransLander.HeaderRow.FindControl("chkSelectAllRS") as CheckBox;
                int intSelectedRentalCount = 0;

                if (!chkSelectAll.Checked)
                {
                    foreach (GridViewRow grv in grvTransLander.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = grv.FindControl("chkSelectRS") as CheckBox;

                            if (chkSelect.Checked)
                            {
                                intSelectedRentalCount += 1;

                                ImageButton imgQuery = grv.FindControl("imgbtnQuery") as ImageButton;

                                strNote_id = imgQuery.CommandArgument;

                                string FileName = PDFPageSetup.FunPubGetFileName(Convert.ToString(strNote_id) + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss"));

                                string FilePath = Server.MapPath(".") + "\\PDF Files\\";
                                //string DownFile = FilePath + FileName + ".pdf";

                                strHTML = strHTML1;

                                SaveDocument(strHTML, Convert.ToString(strNote_id), FilePath, FileName);
                            }
                        }
                    }
                    if (intSelectedRentalCount == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atleast one record for Invoice Print");
                        return;
                    }
                }
                else
                {
                    foreach (GridViewRow grv in grvTransLander.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = grv.FindControl("chkSelectRS") as CheckBox;

                            if (chkSelect.Checked)
                            {
                                intSelectedRentalCount += 1;

                                ImageButton imgQuery = grv.FindControl("imgbtnQuery") as ImageButton;

                                strNote_id = imgQuery.CommandArgument;

                                string FileName = PDFPageSetup.FunPubGetFileName(Convert.ToString(strNote_id) + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss"));

                                string FilePath = Server.MapPath(".") + "\\PDF Files\\";

                                strHTML = strHTML1;

                                SaveDocument(strHTML, Convert.ToString(strNote_id), FilePath, FileName);
                            }
                        }
                    }
                }

                if (nDigi_Flag == 1)
                {
                    string filePath = Server.MapPath(".") + "\\PDF Files\\BTN\\Signed.zip";

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

                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(SignedFile, false, 0);
                    strScipt1 = "";
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
                else
                {
                    strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + DownFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, "");
        }
    }

    protected void SaveDocument(string strHTML, string ReferenceNumber, string FilePath, string FileName)
    {
        try
        {
            string strQRCode = string.Empty;
            
            DataSet dsPrintDetails = new DataSet();

            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            dictparam.Add("@Stock_Hdr_ID", Convert.ToString(strNote_id));
            dictparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@User_ID", Convert.ToString(intUserID));
            dsPrintDetails = Utility.GetDataset("S3G_LAD_Get_Stock_Print", dictparam);

            //if (ddlTransferType.SelectedValue == "1" && String.IsNullOrEmpty(dsPrintDetails.Tables[0].Rows[0]["QRCode"].ToString()))
            //{
            //    Utility.FunShowAlertMsg(this, "Please upload IRN details to generate the invoice.");
            //    return;
            //}

            if (dsPrintDetails.Tables[0].Rows[0]["Digi_Sign_Enable"].ToString() == "1")
                nDigi_Flag = 1;

            if (strHTML.Contains("~InvoiceTable1~"))
            {
                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dsPrintDetails.Tables[1]/*Invoice Breakup*/);
            }

            FunPubGetQrCode(Convert.ToString(dsPrintDetails.Tables[0].Rows[0]["QRCode"]));

            strQRCode = Server.MapPath(".") + "\\PDF Files\\BTNQRCode.png";

            strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsPrintDetails.Tables[0]/*HDR*/);

            List<string> listImageName = new List<string>();
            listImageName.Add("~CompanyLogo~");
            listImageName.Add("~InvoiceSignStamp~");
            //listImageName.Add("~POSignStamp~");
            listImageName.Add("~BTNQRCode~");
            List<string> listImagePath = new List<string>();

            if (Convert.ToInt32(ReferenceNumber) > 1126)
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
            }
            else
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
            }

            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
            //listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));
            listImagePath.Add(strQRCode);

            strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
            string text = "";
            if (Convert.ToInt32(ReferenceNumber) > 1132)
            {
                text = "\nRegd. Office: Ground Floor, Block No B, 809, EGA Trade Centre, Poonamallee High Road, Kilpauk, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            }
            else
            { 
                text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069. ";
            }

            if (Convert.ToString(dsPrintDetails.Tables[0].Rows[0]["Invoice_No"]).Trim() != string.Empty)
            {
                FileName = Convert.ToString(dsPrintDetails.Tables[0].Rows[0]["Invoice_No"]).Replace("/", "_");
            }

            if (nDigi_Flag == 1)
            {
                FunPubSaveDocument(strHTML, FilePath, FileName, "P", text);
                //S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                //ObjPDFSign.DigiPDFSign(FilePath + SignedFile + ".pdf", DownFile, "RIGHT");
            }
            else
                FunPubSaveDocument(strHTML, FilePath, FileName, "P", text);

            SignedFile = "Signed" + intUserID.ToString();

            SignedFile = Server.MapPath(".") + "\\PDF Files\\BTN\\Signed\\";

            if (!System.IO.Directory.Exists(SignedFile))
            {
                System.IO.Directory.CreateDirectory(SignedFile);
            }

            DownFile = FilePath + FileName + ".pdf";

            if (nDigi_Flag == 1)
            {
                S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                ObjPDFSign.DigiPDFSign(DownFile, SignedFile + Path.GetFileName(DownFile), "RIGHT");
            }
            else
            {
                //filepaths.Add(FilePath + FileName + ".pdf");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, "");
        }
    }

    private void FunPubGetQrCode(string ReferenceNumber)
    {
        try
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ReferenceNumber, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);

            qrCodeImage.Save(Server.MapPath(".") + "\\PDF Files\\BTNQRCode.png");
        }
        catch (Exception ex)
        {

        }
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, string FooterNote)
    {
        string strhtmlFile = FilePath + "\\" + FileName + ".html";
        string strwordFile = FilePath + "\\" + FileName + ".doc";
        string strpdfFile = FilePath + "\\" + FileName + ".pdf";
        object file = strhtmlFile;
        object oMissing = System.Reflection.Missing.Value;
        object readOnly = false;
        object oFalse = false;
        object fileFormat = null;

        Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
        Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            if (File.Exists(strwordFile) == true)
            {
                File.Delete(strwordFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            //if (Utility.StringToDate(obj_Page.txtDocumentDate.Text) < Utility.StringToDate("30-Jun-2020"))
            //{
            //    string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
            //    oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            //    oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            //    oDoc.ActiveWindow.Selection.Font.Size = 7;
            //    oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            //    oDoc.ActiveWindow.Selection.TypeText(textDisc);
            //}

            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" / ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            if (FooterNote != "")
            {
                oDoc.ActiveWindow.Selection.Font.Size = 9;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(FooterNote);
            }
            else
            {
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
            }

            string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            if (DocumentType == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
            }

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);
        }
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



    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceNoList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Option", "18");
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
        Procparam.Add("@Option", "17");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    // Added on 19Jun215 Ends here


    //protected void btnRentalCancel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (grvRental.Rows.Count > 0)
    //        {
    //            CheckBox chkSelectAll = grvRental.HeaderRow.FindControl("chkAll") as CheckBox;
    //            int intSelectedRentalCount = 0;
    //            if (!chkSelectAll.Checked)
    //            {
    //                foreach (GridViewRow grv in grvRental.Rows)
    //                {
    //                    if (grv.RowType == DataControlRowType.DataRow)
    //                    {
    //                        CheckBox chkSelect = grv.FindControl("chkSelectAccount") as CheckBox;
    //                        if (chkSelect.Checked)
    //                        {
    //                            intSelectedRentalCount += 1;
    //                        }
    //                    }
    //                }
    //                if (intSelectedRentalCount == 0)
    //                {
    //                    Utility.FunShowAlertMsg(this, "Select atleast one record for Invoice Cancellation");
    //                    return;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Utility.FunShowAlertMsg(this, "Select atleast one record for Invoice Cancellation");
    //            return;
    //        }

    //        Procparam = new Dictionary<string, string>();

    //        Procparam.Add("@Company_id", intCompanyID.ToString());
    //        Procparam.Add("@User_ID", intUserID.ToString());
    //        Procparam.Add("@XmlAccDetails", FunPriFormXml(grvRental, true));
    //        DataTable dtdata = new DataTable();
    //        dtdata = Utility.GetDefaultData("S3G_LAD_CancelInTerim", Procparam);
    //        //intErrorCode = objServiceClient.FunPubCreateBillingInt(out strJournalMessage, objBillingEntity);
    //        if (dtdata.Rows.Count > 0)
    //        {
    //            Utility.FunShowAlertMsg(this, "Invoices cancelled successfully");
    //            FetchInvoiceAccountData();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    private string FunPriFormXml(GridView grvXml, bool IsNeedUpperCase)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {
            CheckBox chkSelect = grvRow.FindControl("chkSelectAccount") as CheckBox;
            CheckBox chkSelectAll = grvXml.HeaderRow.FindControl("chkAll") as CheckBox;
            bool blnIsRowSelect = false;
            if ((!chkSelectAll.Checked && chkSelect.Checked) || chkSelectAll.Checked)
            {
                blnIsRowSelect = true;
            }
            intcolcount = 0;
            if (blnIsRowSelect)
            {
                strbXml.Append(" <Details ");
                for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
                {

                    if (grvXml.Columns[intcolcount].HeaderText != "")
                    {
                        strColValue = grvRow.Cells[intcolcount].Text;
                        if (strColValue == "")
                        {
                            if (grvRow.Cells[intcolcount].Controls.Count > 0)
                            {
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                {
                                    strColValue = ((TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                {
                                    strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                {
                                    strColValue = ((Label)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                            }
                            if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                            {
                                if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                                {
                                    strColValue = Utility.StringToDate(strColValue).ToString();
                                }
                            }

                            if (strColValue.Trim() == "")
                            {
                                strColValue = string.Empty;
                            }
                        }
                        if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                        {
                            if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                            {
                                strColValue = Utility.StringToDate(strColValue).ToString();
                            }
                        }
                        if (strColValue.Trim() == "")
                        {
                            strColValue = string.Empty;
                        }
                        // If Numeric BoundColumn has empty (SPACE &nbsp; value ) at the same time that field is a nullable column in DB 
                        // Avoid adding that column to XML to insert the null value
                        if (strColValue.Trim() == "&nbsp;")
                        {
                            continue;
                        }
                        strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                        strColValue = strColValue.Replace("'", "\"");
                        if (IsNeedUpperCase)
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToUpper().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                        else
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToLower().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                    }

                }
                strbXml.Append(" /> ");
            }
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }
}