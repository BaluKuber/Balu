#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Company Creation
/// Created By			: Nataraj Y
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 10-May-2010
/// Reason              : System Admin Module Developement
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 11-May-2010
/// Reason              : System Admin Module Developement
/// <Program Summary>
#endregion

using System;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Collections;

public partial class System_Admin_S3GSysAdminCompanyMaster_View : ApplyThemeForProject
{
    #region Intialization
    CompanyMgtServicesReference.CompanyMgtServicesClient objCompanyMasterClient;
    S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable ObjS3G_CompanyMaster_ViewDataTable = new CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable();
    S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable dtCompany;
    string strSearchColName;
    PagedDataSource pdsPagerDataSource;
    DataView dvSearchView;
    private Int32 intCount;
    string strSortColName;
    ImageButton imgbtnSearch;
    LinkButton lnkbtnSearch;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            FunPriGetCompanyDetails();

        }
    }
    #endregion

    #region Page Methods

    /// <summary>
    /// This method is used to display Company details
    /// </summary>
    private void FunPriGetCompanyDetails()
    {
        try
        {

            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewRow ObjCompanyMasterRow;
            ObjCompanyMasterRow = ObjS3G_CompanyMaster_ViewDataTable.NewS3G_SYSAD_CompanyMaster_ViewRow();
            ObjCompanyMasterRow.Company_ID = 0;
            ObjS3G_CompanyMaster_ViewDataTable.AddS3G_SYSAD_CompanyMaster_ViewRow(ObjCompanyMasterRow);
            objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteCompanyDetails = objCompanyMasterClient.FunPubQueryCompany(SerMode, ClsPubSerialize.Serialize(ObjS3G_CompanyMaster_ViewDataTable, SerMode));
            dtCompany = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteCompanyDetails, SerializationMode.Binary, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable));
            DataView dvCompany = dtCompany.DefaultView;
            dvCompany.Sort = "Company_Code" + " " + "ASC";
            ViewState["strSortDirection"] = "ASC";
            grvCompanyMaster.DataSource = FunPriGetPagedSource(dvCompany);
            grvCompanyMaster.DataBind();
            ViewState["SearchCmpnyGrid"] = dtCompany;
            Session["SearchCmpnyDataView"] = null;
            //Setting this Session Value since we need to Show all data it will also be set to no in Show all Click Event
            Session["AfterCompanySearch"] = "No";
            grvCompanyMaster.HeaderStyle.CssClass = "styleGridHeader";
            lblErrorMessage.InnerText = string.Empty;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
    }
    /// <summary>
    /// This Method is Used to Set Page Numbers for the Grid View
    /// </summary>
    private void FunPridoPaging()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PageIndex");
        dt.Columns.Add("PageText");

        for (int i = 0; i < pdsPagerDataSource.PageCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr[0] = i;
            dr[1] = i + 1;
            dt.Rows.Add(dr);
        }

        dlstPager.DataSource = dt;
        dlstPager.DataBind();
    }
    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>
    protected void FunProHeaderSearch(object sender, EventArgs e)
    {
        string strSearchVal = String.Empty;
        string strSearchVal1 = String.Empty;
        string strSearchVal2 = String.Empty;
        string strSearchVal3 = String.Empty;
        string strSearchVal4 = String.Empty;
        string strSearchVal5 = String.Empty;
        lblErrorMessage.InnerText = "";
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);
            ViewState["txtSearchValue"] = txtboxSearch.Text.Trim();
            ProCurrentPageRW = 0;
            strSearchVal1 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderCompanyCode")).Text.Trim();
            strSearchVal2 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderCompanyName")).Text.Trim();
            strSearchVal3 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderStatus")).Text.Trim();
            if (strSearchVal1 != "" || strSearchVal1 != String.Empty)
            {
                strSearchColName = "Company_Code";
                strSearchVal += "Company_Code like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "" || strSearchVal2 != String.Empty)
            {
                strSearchColName = "Company_Name";
                strSearchVal += "and Company_Name like '" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "" || strSearchVal3 != String.Empty)
            {
                strSearchColName = "Constitutional_Status";
                strSearchVal += "and Constitutional_Status like '" + strSearchVal3 + "%'";
            }


            if (ViewState["SearchCmpnyGrid"] != null)
            {
                DataView dtCompanyView = FuncPriFilterGrid((DataTable)ViewState["SearchCmpnyGrid"], strSearchColName, strSearchVal);
                if (dtCompanyView.Count > 0)
                {
                   
                    grvCompanyMaster.DataSource = FunPriGetPagedSource(dtCompanyView);
                    grvCompanyMaster.DataBind();
                    //Setting this Session Value since we need to Show only Searched Records
                    Session["AfterCompanySearch"] = "Yes";
                }
                else
                {
                    lblErrorMessage.InnerText = "No records found";
                }
               
            }
            ((TextBox)grvCompanyMaster.HeaderRow.FindControl("txtHeaderCompanyCode")).Text = strSearchVal1;
            ((TextBox)grvCompanyMaster.HeaderRow.FindControl("txtHeaderCompanyName")).Text = strSearchVal2;
            ((TextBox)grvCompanyMaster.HeaderRow.FindControl("txtHeaderStatus")).Text = strSearchVal3;
            if(txtboxSearch.Text!="")
            ((TextBox)grvCompanyMaster.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
           
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
    private string FunPriGetSortDirectionStr(string column)
    {
        // By default, set the sort direction to ascending.
        string strSortDirection = "DESC";
        string strSortExpression;

        try
        {
            // Retrieve the last column that was sorted.
            strSortExpression = ViewState["strSortExpression"] as string;
            if (strSortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (strSortExpression == column)
                {
                    string lastDirection = ViewState["strSortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "DESC"))
                    {
                        strSortDirection = "ASC";
                    }
                    else
                    {
                        strSortDirection = "DESC";
                    }
                }
            }
            // Save new values in ViewState.
            ViewState["strSortDirection"] = strSortDirection;
            ViewState["strSortExpression"] = column;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        return strSortDirection;
    }
    /// <summary>
    /// Retruns a dataview by applying Row filer to it
    /// </summary>
    /// <param name="dtTable"></param>
    /// <param name="strColumnName"></param>
    /// <param name="strSearchExp"></param>
    /// <returns></returns>
    private DataView FuncPriFilterGrid(DataTable dtTable, string strColumnName, string strSearchExp)
    {
        if (dtTable != null || dtTable.Rows.Count > 0)
        {
            if (Session["SearchCmpnyDataView"] == null)
            {
                dvSearchView = new DataView(dtTable);
            }
            else
            {
                dvSearchView = (DataView)Session["SearchCmpnyDataView"];

            }

            if (strSearchExp != "")
            {
                if (strSearchExp.StartsWith("and"))
                {
                    strSearchExp = strSearchExp.Substring(3);
                }
                dvSearchView.RowFilter = strSearchExp ;

            }
        }
        Session["SearchCmpnyDataView"] = dvSearchView;
        return dvSearchView;
    }
    /// <summary>
    /// Property Used to Find Current Page
    /// </summary>
    public int ProCurrentPageRW
    {
        get
        {
            if (this.ViewState["CurrentPageRW"] == null)
                return 0;
            else
                return Convert.ToInt32(this.ViewState["CurrentPageRW"].ToString());
        }
        set
        {
            this.ViewState["CurrentPageRW"] = value;
        }
    }
    /// <summary>
    /// Property Used for Page Count
    /// </summary>
    private Int32 ProPageCountRW
    {

        get { return intCount; }
        set { intCount = value; }
    }
    /// <summary>
    /// Loads the grid from the ViewState/Session Value After loading from Db
    /// </summary>
    private void FuncPriBindComapnyGrid()
    {
        if (ViewState["SearchCmpnyGrid"] != null)
        {
            //Session AfterCompanySearch Value is checked since we need to Show only Searched Records or All Records if Value is NO 
            if (Session["SearchCmpnyDataView"] == null || Session["AfterCompanySearch"].ToString()=="No")
            {
                dvSearchView = ((DataTable)ViewState["SearchCmpnyGrid"]).DefaultView;
            }
            else
            {
                dvSearchView = (DataView)Session["SearchCmpnyDataView"];

            }
            if (this.ViewState["strSortExpression"] != null)
            {
                dvSearchView.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
            }
            grvCompanyMaster.DataSource = FunPriGetPagedSource(dvSearchView);
            grvCompanyMaster.DataBind();
          
        }
    }
    /// <summary>
    /// Returns a Paged DataSource Based on the dataview given a input parameter
    /// </summary>
    /// <param name="dvData">DataView</param>
    /// <returns>PagedDataSource</returns>
    private PagedDataSource FunPriGetPagedSource(DataView dvData)
    {
        pdsPagerDataSource = new PagedDataSource();
        pdsPagerDataSource.DataSource = dvData;
        pdsPagerDataSource.AllowPaging = true;
        pdsPagerDataSource.PageSize = 10;
        pdsPagerDataSource.CurrentPageIndex = ProCurrentPageRW;
        imgbtnNext.Enabled = !pdsPagerDataSource.IsLastPage;
        imgbtnPrev.Enabled = !pdsPagerDataSource.IsFirstPage;
        imgbtnLast.Enabled = !pdsPagerDataSource.IsLastPage;
        imgbtnFirst.Enabled = !pdsPagerDataSource.IsFirstPage;
        if (pdsPagerDataSource.PageCount == 0)
        { ViewState["TotalPageCount"] = 0; }
        else { ViewState["TotalPageCount"] = pdsPagerDataSource.PageCount - 1; }
        FunPridoPaging();
        Session["SearchCmpnyDataView"] = dvData;
        return pdsPagerDataSource;
    }
    /// <summary>
    /// Will Perform Sorting On Colunm base upon the image id calling the function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        
        string strTypeName=sender.GetType().Name;
        if (strTypeName == "ImageButton")
        {
            imgbtnSearch = (ImageButton)sender;

            switch (imgbtnSearch.ID)
            {
                case "imgbtnSortCode":
                    strSortColName = "Company_Code";
                    break;
                case "imgbtnSortName":
                    strSortColName = "Company_Name";
                    break;
                case "imgbtnSortStatus":
                    strSortColName = "Constitutional_Status";
                    break;
            }
        }
        
        string strDirection = FunPriGetSortDirectionStr(strSortColName);
        
        if (ViewState["SearchCmpnyGrid"] != null)
        {
            //Session AfterCompanySearch Value is checked since we need to Show only Searched Records or All Records if Value is NO 
            if (Session["SearchCmpnyDataView"] == null || Session["AfterCompanySearch"].ToString() == "No")
            {
                dvSearchView = ((DataTable)ViewState["SearchCmpnyGrid"]).DefaultView;
            }
            else
            {
                dvSearchView = (DataView)Session["SearchCmpnyDataView"];

            }
            dvSearchView.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
            grvCompanyMaster.DataSource = FunPriGetPagedSource(dvSearchView);
            grvCompanyMaster.DataBind();
            if (strDirection == "ASC")
            {
                ((ImageButton)grvCompanyMaster.HeaderRow.FindControl(imgbtnSearch.ID)).ImageUrl = "~/Images/ArrowUp.gif";
            }
            else
            {

                ((ImageButton)grvCompanyMaster.HeaderRow.FindControl(imgbtnSearch.ID)).ImageUrl = "~/Images/ArrowDown.gif";
            }
        }
    }

    #endregion

    #region Page Events
    protected void grvCompanyMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }

        }
    }

    protected void grvCompanyMaster_DataBound(object sender, EventArgs e)
    {
        if (grvCompanyMaster.Rows.Count > 0)
        {
            grvCompanyMaster.UseAccessibleHeader = true;
            grvCompanyMaster.HeaderRow.TableSection = TableRowSection.TableHeader;
            grvCompanyMaster.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminCompanyMaster_Add.aspx");
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMessage.InnerText = "";
            if (ViewState["SearchCmpnyGrid"] != null)
            {
                DataView dvCompany = ((DataTable)ViewState["SearchCmpnyGrid"]).DefaultView;

                grvCompanyMaster.DataSource = FunPriGetPagedSource(dvCompany);
                grvCompanyMaster.DataBind();
                //Setting this Session Value since we need to Show all data it will also be set to no in grid load
                Session["AfterCompanySearch"] = "No";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        try
        {

            ProCurrentPageRW = 0;
            ViewState["dtPaging"] = null;
            FuncPriBindComapnyGrid();
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        try
        {

            ProCurrentPageRW = (int)ViewState["TotalPageCount"];
            ViewState["dtPaging"] = null;
            ViewState["imgbtnLast_Click"] = true;
            FuncPriBindComapnyGrid();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnPrev_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        try
        {

            ProCurrentPageRW -= 1;
            ProPageCountRW = Convert.ToInt32(ViewState["dlstPager_ItemDataBound"]);
            ViewState["imgbtnPrev_Click"] = true;
            FuncPriBindComapnyGrid();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        try
        {

            ProCurrentPageRW += 1;
            ProPageCountRW = Convert.ToInt32(ViewState["dlstPager_ItemDataBound"]);
            ViewState["imgbtnNext_Click"] = true;
            FuncPriBindComapnyGrid();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void dlstPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("lnkbtnPaging"))
            {
                ProPageCountRW = Convert.ToInt32(e.CommandArgument);
                ProCurrentPageRW = Convert.ToInt32(e.CommandArgument.ToString());
                FuncPriBindComapnyGrid();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void dlstPager_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        System.Web.UI.WebControls.LinkButton lnkbtnPage;

        try
        {
            lnkbtnPage = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkbtnPaging");
            if (lnkbtnPage.Text == Convert.ToString(ProCurrentPageRW + 1))
            {
                lnkbtnPage.Enabled = false;
                ViewState["dlstPager_ItemDataBound"] = Convert.ToInt32(((Label)e.Item.FindControl("lblCount")).Text.Trim());
                ((Label)grvCompanyMaster.BottomPagerRow.FindControl("lblPagerSearch")).Text = " of  " + ((int)ViewState["TotalPageCount"] + 1).ToString().Trim();
                ((TextBox)grvCompanyMaster.BottomPagerRow.FindControl("txtPagerSearch")).Text = lnkbtnPage.Text;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

}
