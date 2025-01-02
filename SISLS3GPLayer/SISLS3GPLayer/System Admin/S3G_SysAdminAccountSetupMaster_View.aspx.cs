#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Account Setup View
/// Created By			: Kannan RC
/// Created Date		: 07-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 07-May-2010   
/// <Program Summary>
#endregion
#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;
#endregion

public partial class System_Admin_S3G_SysAdminAccountSetupMaster_View : ApplyThemeForProject
{
    #region Intialization
    AccountMgtServicesReference.AccountMgtServicesClient objAccount_MasterClient;
    int intErrCode = 0;
    int intAccountSetupId = 0;
    int intCompanyID;
    int intUserID;
    string qsMode;
    string strSearchColName;
    UserInfo ObjUserInfo = null;
    string strRedirectPageAdd = "~/System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx";
    bool bModify = false;
    bool bQuery = false;
    bool bCreate = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    PagedDataSource pdsPagerDataSource;
    DataView dvSearchView;
    private Int32 intCount;
    string strSortColName;
    string strqsTab = string.Empty;
    #endregion
    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    Dictionary<string, string> Procparam = null;
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

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Paging Config

        ProPageNumRW = 1;
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


        if (Request.QueryString["qsMode"] != null)
            qsMode = Request.QueryString["qsMode"];

        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        /// <summary>
        ///  Set Tab  Index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        if (tcAcctSetup.ActiveTabIndex == 0)
        {
            strqsTab = "?qsTab=Asset";
        }
        else if (tcAcctSetup.ActiveTabIndex == 1)
        {
            strqsTab = "?qsTab=Liability";
        }
        else if (tcAcctSetup.ActiveTabIndex == 2)
        {
            strqsTab = "?qsTab=Income";
        }
        else if (tcAcctSetup.ActiveTabIndex == 3)
        {
            strqsTab = "?qsTab=Express";
        }

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
        //tcAcctSetup.ActiveTabIndex = 0;
        if (!IsPostBack)
        {
            FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            #region RoleChecker
            /// <summary>
            ///  using User Role Check
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            if (!bIsActive)
            {
                GridView1.Columns[2].Visible = false;
                GridView1.Columns[1].Visible = false;
                btnCreate.Enabled = false;
            }
            if (!bModify)
            {
                GridView1.Columns[2].Visible = false;
            }
            if (!bQuery)
            {
                GridView1.Columns[1].Visible = false;
            }
            if (!bCreate)
            {
                btnCreate.Enabled = false;
            }
            #endregion
            if (qsMode == "Ass")
            {
                tcAcctSetup.ActiveTabIndex = 0;
                FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);

            }
            else if (qsMode == "Lib")
            {
                tcAcctSetup.ActiveTabIndex = 1;
                FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            }
            else if (qsMode == "Inc")
            {
                tcAcctSetup.ActiveTabIndex = 2;
                FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            }
            else if (qsMode == "Exp")
            {
                tcAcctSetup.ActiveTabIndex = 3;
                FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            }

        }
    }

    #region Get
    /// <summary>
    /// Get Accout setup details for all tabs
    /// </summary>
    /// <param name="intTabID"></param>
    private void FunPriGetAssetDetails_dummy(int intTabID)
    {
        try

        {
         
           
            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;


            /* Code Modified by Nataraj Y For Bug Fixing */
            FunPriGetSearchValue();

            bool bIsNewRow = false;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Account_Setup_ID", "0");
            Procparam.Add("@Account_Tab_ID", intTabID.ToString());

            GridView1.BindGridView("S3G_Get__Account_Setup_Details_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
           
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                GridView1.Rows[0].Visible = false;
            }
            /* Code Modified by Nataraj Y For Bug Fixing Ends here */
            /*Commented by Nataraj Y Begins*/
           // AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable ObS3G_AccountDetails = new AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable();
           // AccountMgtServices.S3G_SYSAD_AccountDetailsRow ObjAcctDetailsRow = null;
           // ObjAcctDetailsRow = ObS3G_AccountDetails.NewS3G_SYSAD_AccountDetailsRow();
           // ObjAcctDetailsRow.Account_Tab_ID = intTabID;
           // ObjAcctDetailsRow.Account_Setup_ID = intAccountSetupId;
           // ObjAcctDetailsRow.Company_ID = intCompanyID;
           // ObS3G_AccountDetails.AddS3G_SYSAD_AccountDetailsRow(ObjAcctDetailsRow);
           // objAccount_MasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
           // SerializationMode SerMode = SerializationMode.Binary;
           // byte[] byteAcctDetails = null;
           // byteAcctDetails = objAccount_MasterClient.FunPubGetAccountDetailsPaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObS3G_AccountDetails, SerMode), ObjPaging);
                      
           // AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable dtAcctDetails = (AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable)ClsPubSerialize.DeSerialize(byteAcctDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable));
           //dtAcctDetails=Utility.GetDefaultData(
            //dtAcctDetails = (DataTable)ClsPubSerialize.DeSerialize(byteAcctDetails, SerializationMode.Binary, typeof(DataTable));
            //DataView dvAccountSetup = dtAcctDetails.DefaultView;

            //Paging Config

            //FunPriGetSearchValue();

            //This is to show grid header
            //bool bIsNewRow = false;
            //if (dvAccountSetup.Count == 0)
            //{
            //    //dvEscalation
            //    dvAccountSetup.AddNew();
            //    bIsNewRow = true;
            //}

            //GridView1.DataSource = dtAcctDetails;
            //GridView1.DataBind();

            ////This is to hide first row if grid is empty
            //if (bIsNewRow)
            //{
            //    GridView1.Rows[0].Visible = false;
            //}
            /*Commented by Nataraj Y Ends*/
            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End



        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    #endregion



    protected void btnAssetCreate_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsMode=Asset");
        //Response.Redirect(strRedirectPageAdd);
    }
    protected void btnLiabilityCreate_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsMode=Liability");
        // Response.Redirect(strRedirectPageAdd);
    }
    protected void btnICreate_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsMode=Income");
        // Response.Redirect(strRedirectPageAdd);
    }
    protected void btnECreate_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsMode=Express");
        //  Response.Redirect(strRedirectPageAdd);
    }
    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        if (GridView1.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
            strSearchVal2 = ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
            strSearchVal3 = ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();
            //strSearchVal4 = ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
            //strSearchVal5 = ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (GridView1.HeaderRow != null)
        {
            ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
            ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            //((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            //((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (GridView1.HeaderRow != null)
        {
            ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
            ((TextBox)GridView1.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            //((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            //((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
        }
    }


    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>

    //public bool CheckBool(string str)
    //{
    //    return ((str == "True") ? true : false);

    //}
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
                strSearchVal += "(LM.LOB_CODE +' - '+ LM.LOB_NAME) like '%" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and (UserLocM.Location_CODE +' - '+ UserLocM.LocationCat_Description) like '%" + strSearchVal2 + "%'";

            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and (SE.Account_Flag +' - '+ SE.Account_Code_Desc) like '%" + strSearchVal3 + "%'";
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            //FunGetEscalationDetails();
            FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)GridView1.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

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
        if (strColumn == "LOB")
            strColumn = "LM.LOB_CODE +' - '+ LM.LOB_NAME";
        if (strColumn == "Location")
            strColumn = "UserLocM.Location_CODE +' - '+ UserLocM.LocationCat_Description";

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
                case "lnkbtnSortLOB":
                    strSortColName = "LOB";
                    break;
                case "lnkbtnSortBranch":
                    strSortColName = "UserLocM.Location_Code";
                    break;
                case "lnkbtnSortAccount":
                    strSortColName = "Account_Flag";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            //FunGetEscalationDetails();
            FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);

            if (strDirection == "ASC")
            {
                ((ImageButton)GridView1.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)GridView1.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion


    #region Active Tab
    /// <summary>
    /// Active Tab set based on mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void tcAcctSetup_ActiveTabChanged(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
        FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);

        if (tcAcctSetup.ActiveTabIndex == 0)
        {
            strqsTab = "?qsTab=Asset";
        }
        else if (tcAcctSetup.ActiveTabIndex == 1)
        {
            strqsTab = "?qsTab=Liability";
        }
        else if (tcAcctSetup.ActiveTabIndex == 2)
        {
            strqsTab = "?qsTab=Income";
        }
        else if (tcAcctSetup.ActiveTabIndex == 3)
        {
            strqsTab = "?qsTab=Express";
        }

    }
    #endregion
    /// <summary>
    /// Account setup ID Passed in modify and query Mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        strqsTab = strqsTab.Replace("?", "&");
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsAccount_Setup_ID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M" + strqsTab);
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsAccount_Setup_ID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q" + strqsTab);
                break;

        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("CbxActive");
            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }


            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnModify");
            ImageButton imgbtnquery = (ImageButton)e.Row.FindControl("imgbtnQuery");
            if (lblUserID.Text != "")
            {
                //Modified by saranya 10-Feb-2012 to validate user based on user level and Maker Checker 
               // if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
                if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text),true)))
                {
                    imgbtnEdit.Enabled = true;
                }
                else
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";

                    //Authorization code end

                }
            }
            //else
           // {
                //imgbtnEdit.Enabled = false;
                //imgbtnEdit.CssClass = "styleGridEditDisabled";
                //imgbtnquery.Enabled = false;
                //imgbtnEdit.CssClass = "styleGridQueryDisabled";

           // }

        }

    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0)
        {
            GridView1.UseAccessibleHeader = true;
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
    /// <summary>
    /// On Click Create Account Setup details in particular Tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreate_Click(object sender, EventArgs e)
    {

        if (tcAcctSetup.ActiveTabIndex == 0)
        {
            strqsTab = "?qsTab=Asset";
        }
        else if (tcAcctSetup.ActiveTabIndex == 1)
        {
            strqsTab = "?qsTab=Liability";
        }
        else if (tcAcctSetup.ActiveTabIndex == 2)
        {
            strqsTab = "?qsTab=Income";
        }
        else if (tcAcctSetup.ActiveTabIndex == 3)
        {
            strqsTab = "?qsTab=Express";
        }
        Response.Redirect("~/System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx" + strqsTab);

    }
    /// <summary>
    /// On click show all Account Setup details records using grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            if (tcAcctSetup.ActiveTabIndex == 0)
            {


                ProPageNumRW = 1;
                hdnSearch.Value = "";
                hdnOrderBy.Value = "";
                FunPriClearSearchValue();
                FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            }
            if (tcAcctSetup.ActiveTabIndex == 1)
            {


                ProPageNumRW = 1;
                hdnSearch.Value = "";
                hdnOrderBy.Value = "";
                FunPriClearSearchValue();
                FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            }

            if (tcAcctSetup.ActiveTabIndex == 2)
            {


                ProPageNumRW = 1;
                hdnSearch.Value = "";
                hdnOrderBy.Value = "";
                FunPriClearSearchValue();
                FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            }

            if (tcAcctSetup.ActiveTabIndex == 3)
            {

                ProPageNumRW = 1;
                hdnSearch.Value = "";
                hdnOrderBy.Value = "";
                FunPriClearSearchValue();
                FunPriGetAssetDetails_dummy(tcAcctSetup.ActiveTabIndex + 1);
            }





        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

}
