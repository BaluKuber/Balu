#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Document Number Control View
/// Created By			: Kannan RC
/// Created Date		: 13-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 12-July-2010   
/// Reason              : Add Role Access setup 
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

public partial class System_Admin_S3G_SysAdminDocumentNumberControl_View : ApplyThemeForProject
{
    #region Intialization
    DocMgtServicesReference.DocMgtServicesClient ObjDNCClient;
    S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable ObjS3G_DNCDetailsViewDataTable = new DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable();
    int intDocumentSeqId = 0;
    string strSearchColName;
    string strRedirectPageAdd = "~/System Admin/S3G_SysAdminDocumentNumberControl_Add.aspx";
    PagedDataSource pdsPagerDataSource;
    DataView dvSearchView;
    private Int32 intCount;
    string strSortColName;
    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    bool bModify = false;
    bool bQuery = false;
    bool bCreate = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;
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
        FunGetDNCDetails();
    }
    #endregion

    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            bModify = ObjUserInfo.ProModifyRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            bQuery = ObjUserInfo.ProViewRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
            if (!IsPostBack)
            {
                FunGetDNCDetails();
                if (!bIsActive)
                {
                    gvDNC.Columns[2].Visible = false;
                    gvDNC.Columns[1].Visible = false;
                    btnCreate.Enabled = false;
                    return;
                }

                if (!bModify)
                {
                    gvDNC.Columns[2].Visible = false;
                }
                if (!bQuery)
                {
                    gvDNC.Columns[1].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                }

            }
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;

        }

    }
    #endregion

    #region Page Methods

    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void  FunPriSetSearchValue()
    {
        try
        {
            if (gvDNC.HeaderRow != null)
            {
                ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
                ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
                ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    /// <summary>
    ///  This method used Get DNC Details
    /// </summary>
    private void FunGetDNCDetails()
    {
        ObjDNCClient = new DocMgtServicesReference.DocMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            DocMgtServices.S3G_SYSAD_Get_DNCDetailsRow ObjDNCDetailsRow;
            ObjDNCDetailsRow = ObjS3G_DNCDetailsViewDataTable.NewS3G_SYSAD_Get_DNCDetailsRow();
            ObjDNCDetailsRow.Doc_Number_Seq_ID = intDocumentSeqId;
            ObjDNCDetailsRow.Company_ID = intCompanyID;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            ObjPaging.ProProgram_ID = 10;

            ObjS3G_DNCDetailsViewDataTable.AddS3G_SYSAD_Get_DNCDetailsRow(ObjDNCDetailsRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteDNCDetails = ObjDNCClient.FunPubGetDNCDetails_View(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_DNCDetailsViewDataTable, SerMode), ObjPaging);
            DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable dtDNC = (DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable)ClsPubSerialize.DeSerialize(byteDNCDetails, SerializationMode.Binary, typeof(DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable));
            DataView dvDNC = dtDNC.DefaultView;

            //Paging Config

            FunPriGetSearchValue();
            bool bIsNewRow = false;
            if (dvDNC.Count == 0)
            {

                dvDNC.AddNew();
                bIsNewRow = true;
            }


            //if (this.ViewState["strSortExpression"] != null)
            //{
            //    dvDNC.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
            //}

            gvDNC.DataSource = dvDNC;// FunPriTrimdt(dvDNC.ToTable());//FunPriGetPagedSource(dvDNC);
            gvDNC.DataBind();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                intTotalRecords = 0;
                gvDNC.Rows[0].Visible = false;
            }

            //ViewState["SearchCurrencyGrid"] = dtDNC;
            //Session["SearchCurrencyDataView"] = null;
            //Session["AfterCurrencySearch"] = "No";
            //gvDNC.HeaderStyle.CssClass = "styleGridHeader";

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);


        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //if(ObjDNCClient!=null)
            ObjDNCClient.Close();
        }
    }
    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        try
        {
            if (gvDNC.HeaderRow != null)
            {
                ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
                ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
                ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
                //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
                //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    #region Pageing and sorting
    private void FunPridoPaging()
    {
        try
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
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }

        //dlstPager.DataSource = dt;
        //dlstPager.DataBind();
    }

    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>
    private void FunPriGetSearchValue()
    {
        try
        {
            if (gvDNC.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
                strSearchVal2 = ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
                strSearchVal3 = ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();
                //strSearchVal4 = ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
                //strSearchVal5 = ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// This method used header Searching
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FunProHeaderSearch(object sender, EventArgs e)
    {
        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);
            FunPriGetSearchValue();
            if (strSearchVal1 != "")
            {
                strSearchVal += "(LM.LOB_CODE +' - '+ LM.LOB_NAME)  like '%" + strSearchVal1 + "%'";
                // strSearchVal += "LM.LOB_CODE   like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                //strSearchVal += " and (DSN.Location_Code +' - '+ LOC.LocationCat_Description) like '%" + strSearchVal2 + "%'";
                strSearchVal += " and (LOC.LocationCat_Description) like '%" + strSearchVal2 + "%'";
                //strSearchVal += " and LM.Branch_CODE like '" + strSearchVal2 + "%'";

            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and (DT.Document_Type_Code + ' - ' + DT.Document_Type_Desc) like '%" + strSearchVal3 + "%'";
                //strSearchVal += " and LM.Document_Type_Code  like '" + strSearchVal3 + "%'";
            }
            //commented and added by Kali on 01-mar-2012
            /*
            if ((strSearchVal.Length > 4) && (strSearchVal.Substring(0, 4) == " and"))
            {
                strSearchVal = strSearchVal.Replace(" and ", "");
            }*/

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunGetDNCDetails();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)gvDNC.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    /*   protected void FunProHeaderSearch(object sender, EventArgs e)
       {
           string strSearchVal = String.Empty;
           string strSearchVal1 = String.Empty;
           string strSearchVal2 = String.Empty;
           string strSearchVal3 = String.Empty;
           lblErrorMessage.Text = "";
           TextBox txtboxSearch;
           try
           {
               txtboxSearch = ((TextBox)sender);
               ViewState["txtSearchValue"] = txtboxSearch.Text.Trim();
               ProCurrentPageRW = 0;
               strSearchVal1 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch1")).Text.Trim();
               strSearchVal2 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch2")).Text.Trim();
               strSearchVal3 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch3")).Text.Trim();

               if (strSearchVal1 != "" || strSearchVal1 != String.Empty)
               {
                   strSearchColName = "LOB";
                   strSearchVal += "LOB like '" + strSearchVal1 + "%'";
               }
               if (strSearchVal2 != "" || strSearchVal2 != String.Empty)
               {
                   strSearchColName = "BRANCH";
                   strSearchVal += "and BRANCH like '" + strSearchVal2 + "%'";
               }
               if (strSearchVal3 != "" || strSearchVal3 != String.Empty)
               {
                   strSearchColName = "DESCS";
                   strSearchVal += "and  DESCS like '" + strSearchVal3 + "%'";
               }


               if (ViewState["SearchCurrencyGrid"] != null)
               {
                   DataView dtDNCView = FuncPriFilterGrid((DataTable)ViewState["SearchCurrencyGrid"], strSearchColName, strSearchVal);
                   if (dtDNCView.Count > 0)
                   {

                       gvDNC.DataSource = dtDNCView;// FunPriGetPagedSource(dtDNCView);
                       gvDNC.DataBind();
                       //Setting this Session Value since we need to Show only Searched Records
                       Session["AfterCurrencySearch"] = "Yes";
                   }
                   else
                   {
                       //lblErrorMessage.Text = "No records found";
                       FunGetDNCDetails();
                   }

               }
               ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderLOB")).Text = strSearchVal1;
               ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderBranch")).Text = strSearchVal2;
               ((TextBox)gvDNC.HeaderRow.FindControl("txtHeaderDesc")).Text = strSearchVal3;
               if (txtboxSearch.Text != "")
                   ((TextBox)gvDNC.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

           }
           catch (Exception ex)
           {
               //lblErrorMessage.Text = "No Records Found";
               FunGetDNCDetails();
                 ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
           }
       }*/

    /// <summary>
    /// This method using Sorting
    /// </summary>
    /// <param name="strColumn"></param>
    /// <returns></returns>
    private string FunPriGetSortDirectionStr(string strColumn)
    {

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
            lblErrorMessage.Text = ex.Message;
        }
        return strSortDirection;

        //////////////////////////////////////////////
        //// By default, set the sort direction to ascending.

        //if (column == "LOB")
        //    column = "LM.LOB_CODE +' - '+ LM.LOB_NAME";
        //if (column == "BRANCH")
        //    column = "BM.Branch_CODE +' - '+ BM.BRANCH_NAME";
        //if (column == "DESCS")
        //    column = "DT.Document_Type_Code + ' - ' + DT.Document_Type_Desc";


        //string strSortDirection = "DESC";
        //string strSortExpression;

        //try
        //{
        //    // Retrieve the last column that was sorted.
        //    strSortExpression = ViewState["strSortExpression"] as string;
        //    if (strSortExpression != null)
        //    {
        //        // Check if the same column is being sorted.
        //        // Otherwise, the default value can be returned.
        //        if (strSortExpression == column)
        //        {
        //            string lastDirection = ViewState["strSortDirection"] as string;
        //            if ((lastDirection != null) && (lastDirection == "DESC"))
        //            {
        //                strSortDirection = "ASC";
        //            }
        //        }
        //    }
        //    // Save new values in ViewState.
        //    ViewState["strSortDirection"] = strSortDirection;
        //    ViewState["strSortExpression"] = column;
        //}
        //catch (Exception ex)
        //{
        //      ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        //}
        //return strSortDirection;
    }

    /// <summary>
    /// This methode used filtering using the grid
    /// </summary>
    /// <param name="dtTable"></param>
    /// <param name="strColumnName"></param>
    /// <param name="strSearchExp"></param>
    /// <returns></returns>
    private DataView FuncPriFilterGrid(DataTable dtTable, string strColumnName, string strSearchExp)
    {

        if (dtTable != null || dtTable.Rows.Count > 0)
        {
            if (Session["SearchCurrencyDataView"] == null)
            {
                dvSearchView = new DataView(dtTable);
            }
            else
            {
                dvSearchView = (DataView)Session["SearchCurrencyDataView"];

            }

            if (strSearchExp != "")
            {
                if (strSearchExp.StartsWith("and"))
                {
                    strSearchExp = strSearchExp.Substring(3);
                }
                dvSearchView.RowFilter = strSearchExp;

            }
        }
        Session["SearchCurrencyDataView"] = dvSearchView;
        return dvSearchView;

    }

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


    private void FuncPriBindCurrencyGrid()
    {
        try
        {
            if (ViewState["SearchCurrencyGrid"] != null)
            {
                //Session AfterCurrencySearch Value is checked since we need to Show only Searched Records or All Records if Value is NO 
                if (Session["SearchCurrencyDataView"] == null || Session["AfterCurrencySearch"].ToString() == "No")
                {
                    dvSearchView = ((DataTable)ViewState["SearchCurrencyGrid"]).DefaultView;
                }
                else
                {
                    dvSearchView = (DataView)Session["SearchCurrencyDataView"];

                }
                if (this.ViewState["strSortExpression"] != null)
                {
                    dvSearchView.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
                }
                gvDNC.DataSource = dvSearchView;// FunPriGetPagedSource(dvSearchView);
                gvDNC.DataBind();

            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }


    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        try
        {
            var imgbtnSearch = string.Empty;
            //ImageButton imgbtnSearch = (ImageButton)sender;
            LinkButton lnkbtnSearch = (LinkButton)sender;
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            switch (lnkbtnSearch.ID)
            {
                case "lnkbtnLOB":
                    strSortColName = "LM.LOB_CODE +' - '+ LM.LOB_NAME";
                    break;
                case "lnkbtnRegionCode":
                    //code changed for Bug Fixing_ID_6002_Kuppusamy.B - 05-Mar-2012
                    //strSortColName = "DSN.Location_Code +' - '+ LOC.LocationCat_Description";
                    strSortColName = "LOC.LocationCat_Description";
                    break;
                case "lnkbtnUser":
                    strSortColName = "DT.Document_Type_Code + ' - ' + DT.Document_Type_Desc";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);


            FunGetDNCDetails();

            if (strDirection == "ASC")
            {
                ((ImageButton)gvDNC.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)gvDNC.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }

            //if (ViewState["SearchCurrencyGrid"] != null)
            //{
            //    //Session AfterCurrencySearch Value is checked since we need to Show only Searched Records or All Records if Value is NO 
            //    if (Session["SearchCurrencyDataView"] == null || Session["AfterCurrencySearch"].ToString() == "No")
            //    {
            //        dvSearchView = ((DataTable)ViewState["SearchCurrencyGrid"]).DefaultView;
            //    }
            //    else
            //    {
            //        dvSearchView = (DataView)Session["SearchCurrencyDataView"];

            //    }
            //    dvSearchView.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
            //    gvDNC.DataSource = dvSearchView;// FunPriGetPagedSource(dvSearchView);
            //    gvDNC.DataBind();
            //    if (strDirection == "ASC")
            //    {
            //       // ((ImageButton)gvDNC.HeaderRow.FindControl(imgbtnSearch.ID)).ImageUrl = "~/Images/ArrowUp.gif";
            //        ((ImageButton)gvDNC.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";

            //    }
            //    else
            //    {

            //        //((ImageButton)gvDNC.HeaderRow.FindControl(imgbtnSearch.ID)).ImageUrl = "~/Images/ArrowDown.gif";
            //        ((ImageButton)gvDNC.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";

            //    }
            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    //private PagedDataSource FunPriGetPagedSource(DataView dvData)
    //{
    //pdsPagerDataSource = new PagedDataSource();
    //pdsPagerDataSource.DataSource = dvData;
    //pdsPagerDataSource.AllowPaging = true;
    ////pdsPagerDataSource.PageSize = 10;
    //pdsPagerDataSource.CurrentPageIndex = ProCurrentPageRW;
    ////imgbtnDNCNext.Enabled = !pdsPagerDataSource.IsLastPage;
    ////imgbtnDNCPrev.Enabled = !pdsPagerDataSource.IsFirstPage;
    ////imgbtnDNCLast.Enabled = !pdsPagerDataSource.IsLastPage;
    ////imgbtnDNCFirst.Enabled = !pdsPagerDataSource.IsFirstPage;
    //if (pdsPagerDataSource.PageCount == 0)
    //{ ViewState["TotalPageCount"] = 0; }
    //else { ViewState["TotalPageCount"] = pdsPagerDataSource.PageCount - 1; }
    //FunPridoPaging();
    //Session["SearchCurrencyDataView"] = dvData;
    //return pdsPagerDataSource;
    // }




    protected void dlstPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("lnkbtnDNCPaging"))
            {
                ProPageCountRW = Convert.ToInt32(e.CommandArgument);
                ProCurrentPageRW = Convert.ToInt32(e.CommandArgument.ToString());
                FuncPriBindCurrencyGrid();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }


    protected void dlstPager_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        System.Web.UI.WebControls.LinkButton lnkbtnPage;

        try
        {
            lnkbtnPage = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkbtnDNCPaging");
            if (lnkbtnPage.Text == Convert.ToString(ProCurrentPageRW + 1))
            {
                lnkbtnPage.Enabled = false;
                ViewState["dlstPager_ItemDataBound"] = Convert.ToInt32(((Label)e.Item.FindControl("lblCount")).Text.Trim());
                ((Label)gvDNC.BottomPagerRow.FindControl("lblPagerSearch")).Text = " of  " + ((int)ViewState["TotalPageCount"] + 1).ToString().Trim();
                ((TextBox)gvDNC.BottomPagerRow.FindControl("txtPagerSearch")).Text = lnkbtnPage.Text;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }



    #endregion

    #region Page Events
    private DataTable FunPriTrimdt(DataTable dt)
    {
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            if (!(string.IsNullOrEmpty(hdnOrderBy.Value)))
            {
                dt.DefaultView.Sort = hdnOrderBy.Value;
                dt = dt.DefaultView.ToTable();
            }
            for (int i_dt = 0; i_dt < dt.Rows.Count; i_dt++)
            {
                dt.Rows[i_dt]["RowNumber"] = (i_dt + 1);
            }
        }
        return dt;
    }

    /// <summary>
    /// Onclick create new DNC 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/System Admin/S3G_SysAdminDocumentNumberControl_Add.aspx");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void gvDNC_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gvDNC.Rows.Count > 0)
            {
                gvDNC.UseAccessibleHeader = true;
                gvDNC.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvDNC.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void gvDNC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblDNCActive = (Label)e.Row.FindControl("lblDNCActive");
                CheckBox chkAct = (CheckBox)e.Row.FindControl("CbxDNCActive");
                if (lblDNCActive.Text == "True")
                {
                    chkAct.Checked = true;
                }

                //User Authorization
                Label lblUserID = (Label)e.Row.FindControl("lblUserID");
                Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnModify");
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
                    //Authorization code end   
                }


            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }


    /// <summary>
    /// Show all the records in click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            lblErrorMessage.Text = "";

            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunGetDNCDetails();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }


    /// <summary>
    /// Passing DNC ID for Modify mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDNC_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            switch (e.CommandName.ToLower())
            {
                case "modify":
                    Response.Redirect(strRedirectPageAdd + "?qsDNCId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                    break;
                case "query":
                    Response.Redirect(strRedirectPageAdd + "?qsDNCId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                    break;

            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion
}
