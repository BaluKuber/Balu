#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   System Admin
/// Screen Name         :   S3GSysAdminLOBMaster_View
/// Created By          :   Suresh P
/// Created Date        :   07-MAY-2010
/// Purpose             :   To view lob master details
/// Last Updated By		:   Suresh P
/// Last Updated Date   :   13-MAY-2010
/// Reason              :   To add Grid Search / Paging Functionality  
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

public partial class S3GSysAdminLOBMaster_View : ApplyThemeForProject
{
    #region Intialization
    string strRedirectPage = "~/System Admin/S3GSysAdminLOBMaster_Add.aspx";
    CompanyMgtServicesReference.CompanyMgtServicesClient ObjLOBMasterClient;
    CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable ObjS3G_LOBMaster_CUDataTable = new CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable();
    #endregion


    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;
    string strRedirectPageAdd = "~/System Admin/S3GSysAdminLOBMaster_Add.aspx";
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
        FunPriBindGrid();
    }
    #endregion

    string strSearchColName;
    PagedDataSource pdsPagerDataSource;
    DataView dvSearchView;
    private Int32 intCount;
    string strSortColName;



    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErrorMessage.InnerText = "";
        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);


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

        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        if (!IsPostBack)
        {
            FunPriBindGrid();
            if (!bIsActive)
            {
                grvLOBMaster.Columns[1].Visible = false;
                grvLOBMaster.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }           
            if (!bModify)
            {
                  grvLOBMaster.Columns[1].Visible = false;
            }
              
                if (!bQuery)
                {
                    grvLOBMaster.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                }
            

        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// This method is used to display LOB details
    /// </summary>
    private void FunPriBindGrid()
    {
        ObjLOBMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            CompanyMgtServices.S3G_SYSAD_LOBPagingMasterDataTable ObjS3G_LOBMaster_CUDataTable = new CompanyMgtServices.S3G_SYSAD_LOBPagingMasterDataTable();
            CompanyMgtServices.S3G_SYSAD_LOBPagingMasterRow ObjLOBMasterRow = ObjS3G_LOBMaster_CUDataTable.NewS3G_SYSAD_LOBPagingMasterRow();

            //Paging Properties set
            ObjLOBMasterRow.LOB_ID = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            //Paging code end

            ObjS3G_LOBMaster_CUDataTable.AddS3G_SYSAD_LOBPagingMasterRow(ObjLOBMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;

            byte[] byteLOBDetails = ObjLOBMasterClient.FunPubQueryLOBPaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMaster_CUDataTable, SerMode), ObjPaging);

            CompanyMgtServices.S3G_SYSAD_LOBPagingMasterDataTable dtLOB = (CompanyMgtServices.S3G_SYSAD_LOBPagingMasterDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_LOBPagingMasterDataTable));
            DataView dvLOB = dtLOB.DefaultView;

            //Paging Config

            FunPriGetSearchValue();

            //This is to show grid header
            bool bIsNewRow = false;
            if (dvLOB.Count == 0)
            {
                dvLOB.AddNew();
                bIsNewRow = true;
            }

            grvLOBMaster.DataSource = dvLOB;
            grvLOBMaster.DataBind();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvLOBMaster.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            ObjLOBMasterClient.Close();
        }
    }
    //private void FunPriBindGrid()
    //{
    //    try
    //    {
    //        CompanyMgtServices.S3G_SYSAD_LOBMaster_CURow ObjLOBMasterRow = ObjS3G_LOBMaster_CUDataTable.NewS3G_SYSAD_LOBMaster_CURow();
    //        ObjLOBMasterRow.Company_ID = Convert.ToInt32(Session["Company_ID"]);
    //        ObjS3G_LOBMaster_CUDataTable.AddS3G_SYSAD_LOBMaster_CURow(ObjLOBMasterRow);

    //        ObjLOBMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
    //        SerializationMode SerMode = SerializationMode.Binary;
    //        byte[] byteLOBDetails = ObjLOBMasterClient.FunPubQueryLOB(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMaster_CUDataTable, SerMode));

    //        CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable dtLOB = (CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable));
    //        DataView dvLOB = dtLOB.DefaultView;
    //        if (this.ViewState["strSortExpression"] != null)
    //        {
    //            dvLOB.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
    //        }

    //        grvLOBMaster.DataSource = FunPriGetPagedSource(dvLOB);
    //        grvLOBMaster.DataBind();
    //        ViewState["SearchLOBGrid"] = dtLOB;
    //        Session["SearchLOBDataView"] = null;
    //        //Setting this Session Value since we need to Show all data it will also be set to no in Show all Click Event
    //        Session["AfterLOBSearch"] = "No";

    //        grvLOBMaster.HeaderStyle.CssClass = "styleGridHeader";
    //        ObjLOBMasterClient.Close();
    //    }
    //    catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.InnerText = ex.Message;
    //    }
    //}


    /// <summary>
    /// This Method is Used to Set Page Numbers for the Grid View
    /// </summary>
    //private void FunPridoPaging()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("PageIndex");
    //    dt.Columns.Add("PageText");

    //    for (int i = 0; i < pdsPagerDataSource.PageCount; i++)
    //    {
    //        DataRow dr = dt.NewRow();
    //        dr[0] = i;
    //        dr[1] = i + 1;
    //        dt.Rows.Add(dr);
    //    }

    //    dlstPager.DataSource = dt;
    //    dlstPager.DataBind();
    //}
    #endregion

    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>
    private void FunPriGetSearchValue()
    {
        if (grvLOBMaster.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
            strSearchVal2 = ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
            //strSearchVal3 = ((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();
            //strSearchVal4 = ((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
            //strSearchVal5 = ((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
        }
    }
    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (grvLOBMaster.HeaderRow != null)
        {
            ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvLOBMaster.HeaderRow != null)
        {
            ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
        }
    }
    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>
    protected void FunProHeaderSearch(object sender, EventArgs e)
    {
        string strSearchVal = String.Empty;
        //string strSearchVal1 = String.Empty;
        //string strSearchVal2 = String.Empty;
        //string strSearchVal3 = String.Empty;
        //string strSearchVal4 = String.Empty;
        //string strSearchVal5 = String.Empty;
        //lblErrorMessage.InnerText = "";
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            if (strSearchVal1 != "")
            {
                strSearchVal += "LOB_Code like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and LOB_Name like '" + strSearchVal2 + "%'";
            }

            /*if (strSearchVal1 != "" || strSearchVal1 != String.Empty)
            {
                strSearchColName = "LOB_Code";
                strSearchVal += "LOB_Code like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "" || strSearchVal2 != String.Empty)
            {
                strSearchColName = "LOB_Name";
                strSearchVal += "and LOB_Name like '" + strSearchVal2 + "%'";
            }*/

            /*if (strSearchVal3 != "" || strSearchVal3 != String.Empty)
            {
                strSearchColName = "Constitutional_Status";
                strSearchVal += "and Constitutional_Status like '" + strSearchVal3 + "%'";
            }*/
            if ((strSearchVal.Length > 4) && (strSearchVal.Substring(0, 4) == " and"))
            {
                strSearchVal = strSearchVal.Replace(" and ", "");
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvLOBMaster.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = "No Records Found";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    /// <summary>
    /// Gets the Sort Direction of the Column in the Grid View Using View State
    /// </summary>
    /// <param name="column"> Colunm Name is Passed</param>
    /// <returns>Sort Direction as a String </returns>
    private string FunPriGetSortDirectionStr(string strColumn)
    {
        string strSortDirection = string.Empty;
        string strSortExpression = string.Empty;
        // By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;
        strSortDirection = "DESC";

        try
        {
            // Retrieve the last column that was sorted.
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
    /// Will Perform Sorting On Colunm base upon the image id calling the function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                case "lnkbtnSortCode":
                    strSortColName = "LOB_Code";
                    break;
                case "lnkbtnSortName":
                    strSortColName = "LOB_Name";
                    break;
            }
            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvLOBMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvLOBMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }


    /// <summary>
    /// Retruns a dataview by applying Row filer to it
    /// </summary>
    /// <param name="dtTable"></param>
    /// <param name="strColumnName"></param>
    /// <param name="strSearchExp"></param>
    /// <returns></returns>
    //private DataView FuncPriFilterGrid(DataTable dtTable, string strColumnName, string strSearchExp)
    //{
    //    if (dtTable != null || dtTable.Rows.Count > 0)
    //    {
    //        if (Session["SearchLOBDataView"] == null)
    //        {
    //            dvSearchView = new DataView(dtTable);
    //        }
    //        else
    //        {
    //            dvSearchView = (DataView)Session["SearchLOBDataView"];

    //        }

    //        if (strSearchExp != "")
    //        {
    //            if (strSearchExp.StartsWith("and"))
    //            {
    //                strSearchExp = strSearchExp.Substring(3);
    //            }
    //            dvSearchView.RowFilter = strSearchExp;

    //        }
    //    }
    //    Session["SearchLOBDataView"] = dvSearchView;
    //    return dvSearchView;
    //}  
    /// <summary>
    /// Property Used to Find Current Page
    /// </summary>
    //public int ProCurrentPageRW
    //{
    //    get
    //    {
    //        if (this.ViewState["CurrentPageRW"] == null)
    //            return 0;
    //        else
    //            return Convert.ToInt32(this.ViewState["CurrentPageRW"].ToString());
    //    }
    //    set
    //    {
    //        this.ViewState["CurrentPageRW"] = value;
    //    }
    //}
    /// <summary>
    /// Property Used for Page Count
    /// </summary>
    //private Int32 ProPageCountRW
    //{

    //    get { return intCount; }
    //    set { intCount = value; }
    //}
    //private int ProTotalPageCount
    //{
    //    get
    //    {
    //        if (this.ViewState["TotalPageCount"] == null)
    //        {
    //            return 0;
    //        }
    //        else
    //        {
    //            return Convert.ToInt32(ViewState["TotalPageCount"]);
    //        }
    //    }
    //    set
    //    {
    //        this.ViewState["TotalPageCount"] = value;
    //    }
    //}
    /// <summary>
    /// Loads the grid from the ViewState/Session Value After loading from Db
    /// </summary>
    //private void FuncPriBindComapnyGrid()
    //{
    //    if (ViewState["SearchLOBGrid"] != null)
    //    {
    //        //Session AfterLOBSearch Value is checked since we need to Show only Searched Records or All Records if Value is NO 
    //        if (Session["SearchLOBDataView"] == null || Session["AfterLOBSearch"].ToString() == "No")
    //        {
    //            dvSearchView = ((DataTable)ViewState["SearchLOBGrid"]).DefaultView;
    //        }
    //        else
    //        {
    //            dvSearchView = (DataView)Session["SearchLOBDataView"];

    //        }
    //        if (this.ViewState["strSortExpression"] != null)
    //        {
    //            dvSearchView.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
    //        }
    //        grvLOBMaster.DataSource = FunPriGetPagedSource(dvSearchView);
    //        grvLOBMaster.DataBind();

    //    }
    //}
    /// <summary>
    /// Returns a Paged DataSource Based on the dataview given a input parameter
    /// </summary>
    /// <param name="dvData">DataView</param>
    /// <returns>PagedDataSource</returns>
    //private PagedDataSource FunPriGetPagedSource(DataView dvData)
    //{
    //    pdsPagerDataSource = new PagedDataSource();
    //    pdsPagerDataSource.DataSource = dvData;
    //    pdsPagerDataSource.AllowPaging = true;
    //    pdsPagerDataSource.PageSize = 10;
    //    pdsPagerDataSource.CurrentPageIndex = ProCurrentPageRW;
    //    //imgbtnNext.Enabled = !pdsPagerDataSource.IsLastPage;
    //    //imgbtnPrev.Enabled = !pdsPagerDataSource.IsFirstPage;
    //    //imgbtnLast.Enabled = !pdsPagerDataSource.IsLastPage;
    //    //imgbtnFirst.Enabled = !pdsPagerDataSource.IsFirstPage;
    //    if (pdsPagerDataSource.PageCount == 0)
    //    {
    //        //ViewState["TotalPageCount"] = 0;
    //        ProTotalPageCount = 0;
    //    }
    //    else
    //    {
    //        //ViewState["TotalPageCount"] = pdsPagerDataSource.PageCount - 1;
    //        ProTotalPageCount = pdsPagerDataSource.PageCount - 1;
    //    }
    //    //FunPridoPaging();
    //    Session["SearchLOBDataView"] = dvData;
    //    return pdsPagerDataSource;
    //}


    #endregion

    #region Page Events

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminLOBMaster_Add.aspx");
    }
    //protected void btnShowAll_Click(object sender, EventArgs e)
    //{
    //    try
    //    {


    //        lblErrorMessage.InnerText = "";
    //        if (ViewState["SearchLOBGrid"] != null)
    //        {
    //            DataView dtLOBView = ((DataTable)ViewState["SearchLOBGrid"]).DefaultView;

    //            //if (ViewState["SearchLOBGrid"] != null)

    //            //DataView dtLOBView = FuncPriFilterGrid((DataTable)ViewState["SearchLOBGrid"], strSearchColName, strSearchVal);


    //            grvLOBMaster.DataSource = FunPriGetPagedSource(dtLOBView);
    //            grvLOBMaster.DataBind();
    //            //Setting this Session Value since we need to Show all data it will also be set to no in grid load
    //            Session["AfterLOBSearch"] = "No";

    //            //Session["AfterCompanySearch"] = "No";

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunPriBindGrid();
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    /// <summary>
    /// GridView Row DataBound Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvLOBMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
            chkAct.Checked = false;
            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }


            //User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            if (lblUserID.Text != "")
            {
                //Modified by saranya 10-Feb-2012 to validate user based on user level and Maker Checker
                //if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
                if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text),true)))
                {
                    imgbtnEdit.Enabled = true;
                }
                else
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
            //Authorization code end
        }
    }

    public string FunPriGetURL(string strId)
    {
        return (strRedirectPage + "?qsLOBID=" + strId + "&mode=Q");
    }


    ///// <summary>
    ///// Gridview DataBound Event
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void grvLOBMaster_DataBound(object sender, EventArgs e)
    //{
    //    if (grvLOBMaster.Rows.Count > 0)
    //    {
    //        grvLOBMaster.UseAccessibleHeader = true;
    //        grvLOBMaster.HeaderRow.TableSection = TableRowSection.TableHeader;
    //        grvLOBMaster.FooterRow.TableSection = TableRowSection.TableFooter;
    //    }
    //}
    //protected void imgbtnFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    //{
    //    try
    //    {

    //        ProCurrentPageRW = 0;
    //        ViewState["dtPaging"] = null;
    //        FuncPriBindComapnyGrid();
    //    }
    //    catch (Exception ex)
    //    {

    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    //protected void imgbtnLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    //{
    //    try
    //    {

    //        //ProCurrentPageRW = (int)ViewState["TotalPageCount"];
    //        ProCurrentPageRW = ProTotalPageCount;
    //        ViewState["dtPaging"] = null;
    //        ViewState["imgbtnLast_Click"] = true;
    //        FuncPriBindComapnyGrid();
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    //protected void imgbtnPrev_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    //{
    //    try
    //    {

    //        ProCurrentPageRW -= 1;
    //        ProPageCountRW = Convert.ToInt32(ViewState["dlstPager_ItemDataBound"]);
    //        ViewState["imgbtnPrev_Click"] = true;
    //        FuncPriBindComapnyGrid();
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    //protected void imgbtnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    //{
    //    try
    //    {

    //        ProCurrentPageRW += 1;
    //        ProPageCountRW = Convert.ToInt32(ViewState["dlstPager_ItemDataBound"]);
    //        ViewState["imgbtnNext_Click"] = true;
    //        FuncPriBindComapnyGrid();
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    //protected void dlstPager_ItemCommand(object source, DataListCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.Equals("lnkbtnPaging"))
    //        {
    //            ProPageCountRW = Convert.ToInt32(e.CommandArgument);
    //            ProCurrentPageRW = Convert.ToInt32(e.CommandArgument.ToString());
    //            FuncPriBindComapnyGrid();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    //protected void dlstPager_ItemDataBound(object sender, DataListItemEventArgs e)
    //{
    //    System.Web.UI.WebControls.LinkButton lnkbtnPage;

    //    try
    //    {
    //        lnkbtnPage = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkbtnPaging");
    //        if (lnkbtnPage.Text == Convert.ToString(ProCurrentPageRW + 1))
    //        {
    //            lnkbtnPage.Enabled = false;
    //            ViewState["dlstPager_ItemDataBound"] = Convert.ToInt32(((Label)e.Item.FindControl("lblCount")).Text.Trim());
    //            //((Label)grvLOBMaster.BottomPagerRow.FindControl("lblPagerSearch")).Text = " of  " + ((int)ViewState["TotalPageCount"] + 1).ToString().Trim();
    //            ((Label)grvLOBMaster.BottomPagerRow.FindControl("lblPagerSearch")).Text = " of  " + ProTotalPageCount + 1;
    //            ((TextBox)grvLOBMaster.BottomPagerRow.FindControl("txtPagerSearch")).Text = lnkbtnPage.Text;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}


    #endregion



    protected void grvLOBMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsLobID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsLobID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;

        }

    }
}
