#region PageHeader
/// © 2013 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Users Login Details
/// Screen Name         :   S3GSysAdminUserLoginDetails
/// Created By          :   Chandu 
/// Created Date        :   30-July-2013
/// Purpose             :   Logged On User Details
/// <Program Summary>
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Configuration;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
public partial class System_Admin_S3GSysAdminUserLoginDetails : ApplyThemeForProject
{
    #region [Intialization]

    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;

    #endregion
    #region Declarations


    static string strPageName = "Logged On User Details";
    int intCompanyID, intUserID,intAccessLevel = 0;
    Dictionary<String, String> ParamsDict = new Dictionary<String, String>();


    #endregion
    #region Delcarations For Paging

    PagingValues ObjPaging = new PagingValues();

    PagingValues TransObjPaging = new PagingValues();

    public delegate void TransPageAssignValue(int TransProPageNumRW, int TransProPageSizeRW);
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
    public int TransProPageNumRW
    {
        get;
        set;
    }
    public int TransProPageSizeRW
    {
        get;
        set;

    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;

        FunPriLoadGridUsers();



    }
    protected void TransPageAssignValueSet(int TransintPageNum, int TransintPageSize)
    {
        TransProPageNumRW = TransintPageNum;
        TransProPageSizeRW = TransintPageSize;

        FunPriLoadTransGrid();

    }
    #endregion
    #region Paging Config
    private void FunPriPageLoadCommon()
    {
      

        ProPageNumRW = 1;
        TransProPageNumRW = 1;

        PageAssignValue obj = new PageAssignValue(this.AssignValue);
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        if (txtPageSize.Text != "")
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        else
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

        ucCustomPaging.callback = obj;
        ucCustomPaging.ProPageNumRW = ProPageNumRW;
        ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

        //Second Pager

        TransPageAssignValue objTrans = new TransPageAssignValue(this.TransPageAssignValueSet);
        TextBox TranstxtPageSize = (TextBox)UCCustomTransPaging.FindControl("txtPageSize");
        if (TranstxtPageSize.Text != "")
            TransProPageSizeRW = Convert.ToInt32(TranstxtPageSize.Text);
        else
            TransProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

        UCCustomTransPaging.callback = objTrans;
        UCCustomTransPaging.ProPageNumRW = TransProPageNumRW;
        UCCustomTransPaging.ProPageSizeRW = TransProPageSizeRW;

       

        
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriSetSessionVariables();
        FunPriPageLoadCommon();
      
        if (!IsPostBack)
        {
            if (intAccessLevel != 5)
                BtnDisconnect.Visible = false;
            PnlUserTrans.Attributes.Add("style", "display:none;");
            PriFunLoadLocationDropDown();
            FunPriActiveUserCount();
            FunPriLoadGridUsers();
            Session["UserListLocationID"] = ddlLocation.SelectedValue;
        }
    }
    private void FunPriSetSessionVariables() {
        try
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.PageTitle);
            ObjS3GSession = new S3GSession();
            ObjUserInfo = new UserInfo();
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            if (intAccessLevel == 0)
                intAccessLevel = ObjUserInfo.ProUserLevelIdRW;
        }
        catch (Exception ex)
        {
            ObjS3GSession = null;
            ObjUserInfo = null;
            //  ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjS3GSession = null;
            ObjUserInfo = null;
        }
    }
    private void FunPriActiveUserCount()
    {
        ParamsDict.Clear();
        ParamsDict.Add("@LocationCode", ddlLocation.SelectedValue.ToString());
        DataTable dt = new DataTable();
        //int ActiveUserCount;
        //dt = Utility.GetGridData("S3G_SYSAD_GET_ACTIVEUSERCOUNT", ParamsDict, out ActiveUserCount, ObjPaging);
        dt = Utility.GetDefaultData("S3G_SYSAD_GET_ACTIVEUSERCOUNT", ParamsDict);
        
        txtUserCount.Text = dt.Rows.Count.ToString();
        
    }
    protected void ddlLocation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Session["UserListLocationID"] = ddlLocation.SelectedValue;
        hdnShowAll.Value = "";
        FunPriActiveUserCount();
        FunPriLoadGridUsers();
    }
    /// <summary>
    /// Location Drop Down Bind
    /// </summary>
    private void PriFunLoadLocationDropDown()
    {
        ParamsDict.Clear();
        try
        {
            ParamsDict.Add("@User_ID", Convert.ToString(intUserID));
            ddlLocation.BindDataTable("S3G_SA_GET_LocationList", ParamsDict,true,"All",new string[] { "Location_ID", "Location" });
        }
        catch (Exception ex)
        {
            //   ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }
    private void FunPriLoadGridUsers()
    {
        ParamsDict.Clear();
        ParamsDict.Add("@LocationCode", ddlLocation.SelectedValue);
        if (RBList.SelectedValue != "")
            ParamsDict.Add("@ShowAll", "Yes");
        FunPriBindGrid();
        if (GrdUsers.Rows.Count > 0)
        {
            BtnExportToExcel.Enabled = true;
            BtnDisconnect.Enabled = true;
            BtnShowAll.Enabled = true;
        }
        //if (ddlLocation.SelectedIndex != 0)
        //{
        //    BtnExportToExcel.Enabled = true;
        //  //Was Cut from Here
        //}
        //else
        //{
        //    BtnExportToExcel.Enabled = false;
        //    BtnDisconnect.Enabled = false;
        //    BtnShowAll.Enabled = false; 
        //    GrdUsers.DataSource = "";
        //    GrdUsers.DataBind();
        //    GrdUsersExcel.DataSource = "";
        //    GrdUsersExcel.DataBind();

        //    ucCustomPaging.Navigation(0, 1, 10);
        //    ucCustomPaging.setPageSize(10);
        //}
    }
    /// <summary>
    /// User List Bind
    /// </summary>
    #region Grid Events
    protected void GrdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "History")
        {
            GridViewRow Gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int RowIndex = Gvr.RowIndex;


            txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            hdnUserId.Value = e.CommandArgument.ToString();
            HdnCompanyId.Value = GrdUsers.DataKeys[RowIndex]["Company_ID"].ToString();

            PnlUserTrans.GroupingText = GrdUsers.DataKeys[RowIndex]["User_Name"].ToString() + " Login Transaction Details";


            ddlLocation.Enabled = false;
            BtnRefresh.Enabled = false;
            PnlDetails.Attributes.Add("style", "display:none;");
            PnlUserTrans.Attributes.Add("style", "display:block;margin-left: 120px; margin-top: 15px;");
            UCCustomTransPaging.Visible = true;
            FunPriLoadTransGrid();

        }
    }
    protected void GrdUsersExcel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String Is_LoggedIn = GrdUsersExcel.DataKeys[e.Row.DataItemIndex]["Is_LoggedIn"].ToString();
            String Is_CurrentlyActive = GrdUsersExcel.DataKeys[e.Row.DataItemIndex]["Is_CurrentlyActive"].ToString();
            if (Is_CurrentlyActive.ToString() == "1")
                e.Row.Cells[4].Text = "";
        }
    }
    protected void GrdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String Is_LoggedIn = GrdUsers.DataKeys[e.Row.DataItemIndex]["Is_LoggedIn"].ToString();
            String Is_CurrentlyActive = GrdUsers.DataKeys[e.Row.DataItemIndex]["Is_CurrentlyActive"].ToString();
            CheckBox ChkDisconnect = (CheckBox)e.Row.FindControl("ChkDisconnect");
            if (Is_LoggedIn.ToString() == "Yes")
                ChkDisconnect.Checked = true;
            if (Is_CurrentlyActive.ToString() == "1")
                e.Row.Cells[4].Text = "";
            
            ObjUserInfo = new UserInfo();
            String User_ID = GrdUsers.DataKeys[e.Row.DataItemIndex]["User_ID"].ToString();
            if (Convert.ToString(ObjUserInfo.ProUserIdRW) == User_ID)
            {
                ChkDisconnect.Enabled = false;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            TextBox txtUserNameSearch = (TextBox)e.Row.FindControl("txtUserNameSearch");
            txtUserNameSearch.Text = hdnUserName.Value;

        }
    }
    #endregion
    #region GridLoads
    private void FunPriBindGrid()
    {
        try
        {
            //Paging Properties set

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnUserName.Value;// hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            bool bIsNewRow = false;
            //Paging code end

            GrdUsers.BindGridView("S3G_SYSAD_Get_UserList", ParamsDict, out intTotalRecords, ObjPaging, out bIsNewRow);


            //this has to be changed
            //GrdUsersExcel.DataSource = GrdUsers.DataSource;
            //GrdUsersExcel.DataBind();



            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                GrdUsers.Rows[0].Visible = false;
            }

            if (GrdUsers.Rows.Count > 0 && bIsNewRow == false)
            {
                intTotalRecords = Convert.ToInt32(GrdUsers.DataKeys[0]["Pagecounts"].ToString());
            }
            else {
                intTotalRecords = 0;
            }
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
        }
        catch (Exception ex)
        {
            // ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private Dictionary<String,String>  FunPriTransParameters() {
        
        DateTime FromD = DateTime.Now, ToD = DateTime.Now;
        ParamsDict.Clear();
        ParamsDict.Add("@TransUSER_ID", Convert.ToString(hdnUserId.Value.ToString()));
        ParamsDict.Add("@TransCOMPANY_ID", Convert.ToString(HdnCompanyId.Value.ToString()));
        try
        {
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                FromD = Convert.ToDateTime(txtFromDate.Text.ToString());
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                ToD = Convert.ToDateTime(txtToDate.Text.ToString());
            }
        }
        catch (Exception ex)
        {
            FromD = DateTime.Now;
            ToD = DateTime.Now;
            txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            Utility.FunShowAlertMsg(this, "Please Enter a Valid Date");
        }
        ParamsDict.Add("@To_Date", ToD.ToString("MM-dd-yyyy"));
        ParamsDict.Add("@From_Date", FromD.ToString("MM-dd-yyyy"));

        return ParamsDict;
    }
    private void FunPriLoadTransGrid()
    {
        if (hdnUserId.Value != "")
        {


            ParamsDict = FunPriTransParameters();

            //int CompanyID = ObjUserInfo.ProCompanyIdRW;

            int intTotalRecords = 0;
            TransObjPaging.ProCompany_ID = intCompanyID;
            TransObjPaging.ProUser_ID = intUserID;
            TransObjPaging.ProTotalRecords = intTotalRecords;
            TransObjPaging.ProCurrentPage = TransProPageNumRW;
            TransObjPaging.ProPageSize = TransProPageSizeRW;
            TransObjPaging.ProSearchValue = hdnTransSearch.Value;
            TransObjPaging.ProOrderBy = hdnTransOrderBy.Value;

            bool bIsNewRow = false;

            GrdTransDetails.BindGridView("S3G_SYSAD_GET_USERTRANLIST", ParamsDict, out intTotalRecords, TransObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                GrdTransDetails.Rows[0].Visible = false;
            }

            if (GrdTransDetails.Rows.Count  > 0 && bIsNewRow == false)
            {
                intTotalRecords = Convert.ToInt32(GrdTransDetails.DataKeys[0]["Pagecounts"].ToString());
            }
            else
            {
                intTotalRecords = 0;
            }

            UCCustomTransPaging.Navigation(intTotalRecords, TransProPageNumRW, TransProPageSizeRW);
            UCCustomTransPaging.setPageSize(TransProPageSizeRW);

            txtFromDate.Text = Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyyy");
            txtToDate.Text = Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyyy");
        }
        else
        {
            GrdTransDetails.DataSource = "";
            GrdTransDetails.DataBind();
            UCCustomTransPaging.Navigation(0, 1, 10);
            UCCustomTransPaging.setPageSize(10);
        }
    }
    #endregion
    #region Export
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    private void ExportToExcel(GridView Grd, String FileName)
    {
        string attachment = "attachment; filename=" + FileName + ".xls";
        Response.ClearContent();

        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.xls";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        if (Grd.Rows.Count > 0)
        {
            Grd.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
        else
        {
            Utility.FunShowAlertMsg(Up, "No Rows to Export to Excel");
        }
    }
    protected void BtnExportToExcel_Click(object sender, EventArgs e)
    {
        ParamsDict.Clear();
        ParamsDict.Add("@LocationCode", ddlLocation.SelectedValue);
        ParamsDict.Add("@SEARCHVALUE", hdnSearch.Value);
        ParamsDict.Add("@ORDERBY", hdnOrderBy.Value);
        ParamsDict.Add("@USER_ID", intUserID.ToString());
        ParamsDict.Add("@COMPANY_ID", intCompanyID.ToString());

        if (RBList.SelectedValue != "")
            ParamsDict.Add("@ShowAll", "Yes");
        
        DataTable dt = Utility.GetDefaultData("S3G_SYSAD_GET_USERLIST_Excel", ParamsDict);
        GrdUsersExcel.DataSource = dt;
        GrdUsersExcel.DataBind();

        ExportToExcel(GrdUsersExcel, "OnlineUsers " + DateTime.Now);
    }
    protected void BtnExportTrans_Click(object sender, EventArgs e)
    {
        ParamsDict = FunPriTransParameters();
        ParamsDict.Add("@SEARCHVALUE", hdnSearch.Value);
        ParamsDict.Add("@ORDERBY", hdnOrderBy.Value);
        ParamsDict.Add("@USER_ID", intUserID.ToString());
        ParamsDict.Add("@COMPANY_ID", intCompanyID.ToString());

        DataTable dt = Utility.GetDefaultData("S3G_SYSAD_GET_TRANLIST_Excel", ParamsDict);

        GrdTransDetailsExcel.DataSource = dt;
        GrdTransDetailsExcel.DataBind();
        ExportToExcel(GrdTransDetailsExcel, PnlUserTrans.GroupingText.ToString() + " " + DateTime.Now.ToString());
    }
    #endregion
    #region Button Clicks
   
    protected void BtnDisconnect_Click(object sender, EventArgs e)
    {
        String UserIDs = "";
        for (int i = 0; i < GrdUsers.Rows.Count; i++)
        {
            String Is_LoggedIn = GrdUsers.DataKeys[i]["Is_LoggedIn"].ToString();
            if (Is_LoggedIn == "Yes")
            {
                CheckBox ChkDisconnect = (CheckBox)GrdUsers.Rows[i].FindControl("ChkDisconnect");
                if (ChkDisconnect.Checked == false)
                {
                    String User_ID = GrdUsers.DataKeys[i]["User_ID"].ToString();
                    if (UserIDs == "")
                    {
                        UserIDs = User_ID;
                        FunPriDeleteXmlFile(User_ID);
                    }
                    else
                    {
                        UserIDs = UserIDs + "," + User_ID;
                        FunPriDeleteXmlFile(User_ID);
                    }
                }
            }
        }
        if (UserIDs != "")
        {
            S3GAdminServicesReference.S3GAdminServicesClient ObjWebServiceClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            ObjUserInfo = new UserInfo();
            try
            {
                int intErrorCode = ObjWebServiceClient.FunPubUpdateLogOutFlags(ObjUserInfo.ProCompanyIdRW, ObjUserInfo.ProUserIdRW, "", UserIDs,4);
                FunPriLoadGridUsers();
                FunPriActiveUserCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ObjWebServiceClient.Close();
            }
        }
        else
        {
            Utility.FunShowAlertMsg(Up, "Please UnCheck the Users from the list to disconnect!");
        }
    }
    private void FunPriDeleteXmlFile(String UserID) {
        String strPath = Server.MapPath(@"~\Data\UserManagement\" + UserID + "_Disc.xml");
        if (!File.Exists(strPath))
        {
            File.Create(strPath);
        }
    }
    protected void BtnCloseTransPanel_Click(object sender, EventArgs e)
    {
        PnlDetails.Attributes.Add("style", "display:block;");
        PnlUserTrans.Attributes.Add("style", "display:none;");
        ddlLocation.Enabled = true;
        BtnRefresh.Enabled = true;

        hdnUserId.Value = "";
        FunPriLoadGridUsers();
        FunPriActiveUserCount();
    }
    protected void btnSearchTrans_Click(object sender, EventArgs e)
    {
        FunPriLoadTransGrid();
    }
    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        FunPriLoadGridUsers();
        FunPriActiveUserCount();
    }
    protected void BtnShowAll_Click(object sender, EventArgs e) {
        hdnUserName.Value = "";
        //ParamsDict.Clear();
        //ParamsDict.Add("@LocationCode", ddlLocation.SelectedValue);
        //ParamsDict.Add("@ShowAll", "Yes");
        hdnShowAll.Value = "Yes";
        FunPriActiveUserCount();
        FunPriLoadGridUsers();
    }
    #endregion
    protected void RBList_SelectedIndexChanged(object sender, EventArgs e) {
        FunPriActiveUserCount();
        FunPriLoadGridUsers();
    }
    #region WerbService
    [System.Web.Services.WebMethod]
    public static string[] GetUserNameList(String prefixText, int count)
    {
        UserInfo ObjInfo = new UserInfo();
        String ddlLocation = Convert.ToString(System.Web.HttpContext.Current.Session["UserListLocationID"].ToString());
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(ObjInfo.ProCompanyIdRW));
        Procparam.Add("@Location_Code", ddlLocation);
        Procparam.Add("@User_Name", prefixText.ToString().Replace("'", ""));
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("GETUSERSFORLOCATION", Procparam));
        return suggetions.ToArray();
    }
    protected void txtUserNameSearch_OnTextChanged(object sender, EventArgs e)
    {
        TextBox txtUserNameSearch = ((TextBox)sender);
        try
        {
            string strhdnValue = hdnUserName.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtUserNameSearch.Text = string.Empty;
                hdnUserName.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            //   ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion
    /// <summary>
    /// Web Serivce for Getting UserNames
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    protected void Timer_Tick(object sender, EventArgs e)
    {
        FunPriActiveUserCount();
        FunPriLoadGridUsers();
    }
}