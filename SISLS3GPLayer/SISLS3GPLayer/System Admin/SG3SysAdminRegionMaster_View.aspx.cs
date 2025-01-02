#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Region and Branch Details
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

// lblErrorMessageRegion added by S.Kanan - for Bug Id 693
public partial class System_Admin_SG3SysAdminRegionMaster_View : ApplyThemeForProject
{
    #region Intialization
    CompanyMgtServicesReference.CompanyMgtServicesClient objRegionMasterClient;
    CompanyMgtServicesReference.CompanyMgtServicesClient objBranchMasterClient;
    S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable ObjS3G_RegionMasterViewDataTable = new CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable();
    S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable ObjS3G_BranchMasterViewDataTable = new CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable();
    string strSearchColName;
    PagedDataSource pdsPagerDataSource;
    DataView dvSearchView;
    private Int32 intCount;
    string strSortColName;
    string qsMode;
    string qsTab;
    string strRedirectPageAdd = "~/System Admin/SG3SysAdminRegionMaster_Add.aspx";
    string strRedirectPageBranchAdd = "~/System Admin/SG3SysAdminRegionMaster_Add.aspx";
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;
    #endregion


    #region Paging Config - For Region
    /// <summary>
    /// page config used for grid search and sorting
    /// </summary>
    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
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
        FunGetRegionDetails();
    }
    #endregion


    #region Paging Config - For Branch

    string strSearchVal1Branch = string.Empty;
    string strSearchVal2Branch = string.Empty;
    string strSearchVal3Branch = string.Empty;
    string strSearchVal4Branch = string.Empty;
    string strSearchVal5Branch = string.Empty;

    PagingValues ObjPagingBranch = new PagingValues();

    public delegate void PageAssignValueBranch(int ProPageNumRWBranch, int intPageSizeBranch);

    public int ProPageNumRWBranch
    {
        get;
        set;
    }

    public int ProPageSizeRWBranch
    {
        get;
        set;

    }

    protected void AssignValueBranch(int intPageNum, int intPageSize)
    {
        ProPageNumRWBranch = intPageNum;
        ProPageSizeRWBranch = intPageSize;
        FunGetBranchDetails();
        tcRegBranch.ActiveTabIndex = 1;
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        #region Paging Config - for region

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

        #region Paging Config - for branch

        ProPageNumRWBranch = 1;
        TextBox txtPageSizeBranch = (TextBox)ucCustomPagingBranch.FindControl("txtPageSize");
        if (txtPageSizeBranch.Text != "")
            ProPageSizeRWBranch = Convert.ToInt32(txtPageSizeBranch.Text);
        else
            ProPageSizeRWBranch = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

        PageAssignValueBranch objBranch = new PageAssignValueBranch(this.AssignValueBranch);
        ucCustomPagingBranch.callback = objBranch;
        ucCustomPagingBranch.ProPageSizeRW = ProPageNumRWBranch;
        ucCustomPagingBranch.ProPageSizeRW = ProPageSizeRWBranch;

        #endregion

        if (Request.QueryString["qsTab"] != null)
            qsTab = Request.QueryString["qsTab"];
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
        tcRegBranch.ActiveTabIndex = 0;
        if (!IsPostBack)
        {

            FunGetRegionDetails();
            FunGetBranchDetails();
            //if (tcRegBranch.ActiveTabIndex == 0)
            /// <summary>
            /// This method used for Role Access
            /// </summary>
            if (!bIsActive)
            {
                gvRegionDetails.Columns[2].Visible = false;
                gvBranch.Columns[2].Visible = false;
                gvRegionDetails.Columns[1].Visible = false;
                gvBranch.Columns[1].Visible = false;
                btnRegionCreate.Enabled = false;
                btnBranchCreate.Enabled = false;
            }
            if (!bModify)
            {
                gvRegionDetails.Columns[2].Visible = false;
                gvBranch.Columns[2].Visible = false;
            }            
            if (!bQuery)
            {
                gvRegionDetails.Columns[1].Visible = false;
                gvBranch.Columns[1].Visible = false;
            }
            if (!bCreate)
            {
                btnRegionCreate.Enabled = false;
                btnBranchCreate.Enabled = false;
            }


            if (qsTab == "reg")
            {
                tcRegBranch.ActiveTabIndex = 0;
            }
            else if (qsTab == "ban")
            {
                tcRegBranch.ActiveTabIndex = 1;
            }
        }

    }

    #region Page Methods


    /// <summary>
    /// To set search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (gvRegionDetails.HeaderRow != null)
        {
            ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
        }
    }




    /// <summary>
    /// To set search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValueB()
    {
        if (gvBranch.HeaderRow != null)
        {
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal1;
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal2;
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal3;
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch6")).Text = strSearchVal4;
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch7")).Text = strSearchVal5;

        }
    }

    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>

    private void FunPriGetSearchValue()
    {
        if (gvRegionDetails.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
            strSearchVal2 = ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
        }
    }

    private void FunPriGetSearchValueB()
    {
        if (gvBranch.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();
            strSearchVal2 = ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
            strSearchVal3 = ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
            strSearchVal4 = ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch6")).Text.Trim();
            strSearchVal5 = ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch7")).Text.Trim();
        }
    }


    /// <summary>
    /// this method used for header search
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
                strSearchVal += "Region_Code like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and Region_Name like '" + strSearchVal2 + "%'";
            }
            if ((strSearchVal.Length > 4) && (strSearchVal.Substring(0, 4) == " and"))
            {
                strSearchVal = strSearchVal.Replace(" and ", "");
            }
            hdnSearch.Value = strSearchVal;
            FunGetRegionDetails();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)gvRegionDetails.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /*  protected void FunProHeaderSearch(object sender, EventArgs e)
      {
          string strSearchVal = String.Empty;
          string strSearchVal1 = String.Empty;
          string strSearchVal2 = String.Empty;
          string strSearchVal3 = String.Empty;
          string strSearchVal4 = String.Empty;
          string strSearchVal5 = String.Empty;
          lblErrorMessage.Text = "";
          lblErrorMessageRegion.Text = "";
          TextBox txtboxSearch;
          try
          {
              txtboxSearch = ((TextBox)sender);
              ViewState["txtSearchValue"] = txtboxSearch.Text.Trim();
              ProCurrentPageRW = 0;
              strSearchVal1 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch1")).Text.Trim();
              strSearchVal2 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch2")).Text.Trim();

              if (strSearchVal1 != "" || strSearchVal1 != String.Empty)
              {
                  strSearchColName = "Region_Code";
                  strSearchVal += "Region_Code like '" + strSearchVal1 + "%'";
              }
              if (strSearchVal2 != "" || strSearchVal2 != String.Empty)
              {
                  strSearchColName = "Region_Name";
                  strSearchVal += "and Region_Name like '" + strSearchVal2 + "%'";
              }


              if (ViewState["SearchCurrencyGrid"] != null)
              {
                  DataView dtRegionView = FuncPriFilterGrid((DataTable)ViewState["SearchCurrencyGrid"], strSearchColName, strSearchVal);
                  if (dtRegionView.Count > 0)
                  {

                      gvRegionDetails.DataSource = FunPriGetPagedSource(dtRegionView);
                      gvRegionDetails.DataBind();
                      //Setting this Session Value since we need to Show only Searched Records
                      Session["AfterCurrencySearch"] = "Yes";
                  }
                  else
                  {
                      //lblErrorMessage.Text = "No records found";
                      FunPriErrorMessage("No Records Found");
                  }

              }
              ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
              ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
              if (txtboxSearch.Text != "")
                  ((TextBox)gvRegionDetails.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
              tcRegBranch.ActiveTabIndex = 0;
          }
          catch (Exception ex)
          {
              //lblErrorMessage.Text = "No Records Found";
              FunPriErrorMessage("No Records Found");
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
          }
      } */



    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (gvRegionDetails.HeaderRow != null)
        {
            ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            ((TextBox)gvRegionDetails.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            //((TextBox)grvLOB.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
        }
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            lblErrorMessage.Text = "";
            lblErrorMessageRegion.Text = "";
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunGetRegionDetails();
            //if (ViewState["SearchCurrencyGrid"] != null)
            //{
            //    DataView dvRegion = ((DataTable)ViewState["SearchCurrencyGrid"]).DefaultView;

            //    gvRegionDetails.DataSource = FunPriGetPagedSource(dvRegion);
            //    gvRegionDetails.DataBind();
            //    //Setting this Session Value since we need to Show all data it will also be set to no in grid load
            //    Session["AfterCurrencySearch"] = "No";
            //}
            tcRegBranch.ActiveTabIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }



    /// <summary>
    /// This method is used to display Region details
    /// </summary>
    private void FunGetRegionDetails()
    {
        objRegionMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewRow ObjRegionMasterRow;
            ObjRegionMasterRow = ObjS3G_RegionMasterViewDataTable.NewS3G_SYSAD_RegionMaster_ViewRow();
            ObjRegionMasterRow.Region_ID = 0;
            ObjRegionMasterRow.Company_ID = intCompanyID;

            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            ObjS3G_RegionMasterViewDataTable.AddS3G_SYSAD_RegionMaster_ViewRow(ObjRegionMasterRow);
            //objRegionMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteRegionDetails = objRegionMasterClient.FunPubQueryRegion(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_RegionMasterViewDataTable, SerMode), ObjPaging);
            CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable dtRegion = (CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteRegionDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable));
            DataView dvRegion = dtRegion.DefaultView;

            //Paging Config

            FunPriGetSearchValue();
            bool bIsNewRow = false;
            if (dvRegion.Count == 0)
            {

                dvRegion.AddNew();
                bIsNewRow = true;
            }


            //if (this.ViewState["strSortExpression"] != null)
            //{
            //    dvRegion.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
            //}

            gvRegionDetails.DataSource = dvRegion;// FunPriTrimdt(dvRegion.ToTable()); // FunPriGetPagedSource(dvRegion);
            gvRegionDetails.DataBind();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                intTotalRecords = 0;
                gvRegionDetails.Rows[0].Visible = false;
            }



            //ViewState["SearchCurrencyGrid"] = dtRegion;
            // Session["SearchCurrencyDataView"] = null;
            //Session["AfterCurrencySearch"] = "No";
            //gvRegionDetails.HeaderStyle.CssClass = "styleGridHeader";


            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            FunPriErrorMessage(objFaultExp.Detail.ProReasonRW);
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
            FunPriErrorMessage(ex.Message);
        }
        finally
        {
            
                objRegionMasterClient.Close();
            
        }
    }

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
    /// Get Branch details
    /// </summary>
    private void FunGetBranchDetails()
    {
        objBranchMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewRow ObjBranchMasterRow;
            ObjBranchMasterRow = ObjS3G_BranchMasterViewDataTable.NewS3G_SYSAD_BranchDetails_ViewRow();
            ObjBranchMasterRow.Branch_ID = 0;
            ObjBranchMasterRow.Company_ID = intCompanyID;

            ObjPagingBranch.ProCompany_ID = intCompanyID;
            ObjPagingBranch.ProUser_ID = intUserID;
            ObjPagingBranch.ProTotalRecords = intTotalRecords;
            ObjPagingBranch.ProCurrentPage = ProPageNumRWBranch;
            ObjPagingBranch.ProPageSize = ProPageSizeRWBranch;
            ObjPagingBranch.ProSearchValue = hdnSearchBranch.Value;
            ObjPagingBranch.ProOrderBy = hdnOrderByBranch.Value;


            ObjS3G_BranchMasterViewDataTable.AddS3G_SYSAD_BranchDetails_ViewRow(ObjBranchMasterRow);
            //objBranchMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteBranchDetails = objBranchMasterClient.FunPubQueryBranch(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchMasterViewDataTable, SerMode), ObjPagingBranch);
            // byte[] byteBranchDetails = objBranchMasterClient.FunPubQueryBranch(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchMasterViewDataTable, SerMode), ObjPagingBranch);
            CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable dtBranch = (CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable)ClsPubSerialize.DeSerialize(byteBranchDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable));
            DataView dvBranch = dtBranch.DefaultView;

            FunPriGetSearchValueB();
            //This is to show grid header
            bool bIsNewRow = false;
            if (dvBranch.Count == 0)
            {
                dvBranch.AddNew();
                bIsNewRow = true;
            }


            //if (this.ViewState["strSortExpression"] != null)
            //{
            //    dvBranch.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
            //}

            //gvBranch.DataSource = FunPriBGetPagedSource(dvBranch);
            gvBranch.DataSource = dtBranch;
            gvBranch.DataBind();
            if (bIsNewRow)
            {
                intTotalRecords = 0;
                gvBranch.Rows[0].Visible = false;
            }
            ViewState["SearchBCurrencyGrid"] = dtBranch;
            Session["SearchBCurrencyDataView"] = null;
            Session["AfterBCurrencySearch"] = "No";
            gvBranch.HeaderStyle.CssClass = "styleGridHeader";

            FunPriSetSearchValueB();
            //intTotalRecords = dtBranch.Rows.Count;
            ucCustomPagingBranch.Navigation(intTotalRecords, ProPageNumRWBranch, ProPageSizeRWBranch);
            ucCustomPagingBranch.setPageSize(ProPageSizeRWBranch);
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            FunPriErrorMessage(objFaultExp.Detail.ProReasonRW);
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
            FunPriErrorMessage(ex.Message);
        }
        finally
        {
            
                objBranchMasterClient.Close();
            
        }
    }
    #endregion

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/SG3SysAdminRegionMaster_Add.aspx?qsTab=region");
    }
    protected void gvRegionDetails_DataBound(object sender, EventArgs e)
    {
        if (gvRegionDetails.Rows.Count > 0)
        {
            gvRegionDetails.UseAccessibleHeader = true;
            gvRegionDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
            gvRegionDetails.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
    protected void gvRegionDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblRegionActive = (Label)e.Row.FindControl("lblRegionActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("CbxRegionActive");
            if (lblRegionActive.Text == "True")
            {
                chkAct.Checked = true;
            }

            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnModify");
            if (lblUserID.Text != "")
            {
                if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
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
        }
    }



    protected void btnBranchCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/SG3SysAdminRegionMaster_Add.aspx?qsTab=branch");
    }
    protected void gvBranch_DataBound(object sender, EventArgs e)
    {
        if (gvBranch.Rows.Count > 0)
        {
            gvBranch.UseAccessibleHeader = true;
            gvBranch.HeaderRow.TableSection = TableRowSection.TableHeader;
            gvBranch.FooterRow.TableSection = TableRowSection.TableFooter;
        }


    }
    protected void gvBranch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBranchActive = (Label)e.Row.FindControl("lblBranchActive");
            CheckBox chkBranchAct = (CheckBox)e.Row.FindControl("CbxBranchActive");
            if (lblBranchActive.Text == "True")
            {
                chkBranchAct.Checked = true;
            }

            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnBModify");
            if (lblUserID.Text != "")
            {
                if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
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

        }

    }

    #region Sorting and Pageing
    #region RegionMaster
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





    private string FunPriBGetSortDirectionStr(string strColumn)
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
            strSortExpression = hdnSortExpressionBranch.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirectionBranch.Value != null) && (hdnSortDirectionBranch.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
            // Save new values in hidden control.
            hdnSortDirectionBranch.Value = strSortDirection;
            hdnSortExpressionBranch.Value = strColumn;
            strOrderBy = " " + strColumn + " " + strSortDirection;
            hdnOrderByBranch.Value = strOrderBy;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        return strSortDirection;

    }

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
        }
        return strSortDirection;

    }
    /*private string FunPriGetSortDirectionStr(string column)
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
    }*/

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
            gvRegionDetails.DataSource = FunPriGetPagedSource(dvSearchView);
            gvRegionDetails.DataBind();

        }
    }





    private PagedDataSource FunPriGetPagedSource(DataView dvData)
    {
        pdsPagerDataSource = new PagedDataSource();
        pdsPagerDataSource.DataSource = dvData;
        pdsPagerDataSource.AllowPaging = true;
        pdsPagerDataSource.PageSize = 10;
        pdsPagerDataSource.CurrentPageIndex = ProCurrentPageRW;
        //imgbtnNext.Enabled = !pdsPagerDataSource.IsLastPage;
        //imgbtnPrev.Enabled = !pdsPagerDataSource.IsFirstPage;
        //imgbtnLast.Enabled = !pdsPagerDataSource.IsLastPage;
        //imgbtnFirst.Enabled = !pdsPagerDataSource.IsFirstPage;
        if (pdsPagerDataSource.PageCount == 0)
        { ViewState["TotalPageCount"] = 0; }
        else { ViewState["TotalPageCount"] = pdsPagerDataSource.PageCount - 1; }
        //FunPridoPaging();
        Session["SearchCurrencyDataView"] = dvData;
        return pdsPagerDataSource;
    }


    // added by S.Kannan
    private void FunPriErrorMessage(string errormsg)
    {
        if (tcRegBranch.ActiveTabIndex == 1)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = errormsg;
            lblErrorMessageRegion.Visible = false;
            lblErrorMessageRegion.Text = "";
        }
        if (tcRegBranch.ActiveTabIndex == 0)
        {

            lblErrorMessage.Visible = false;
            lblErrorMessage.Text = "";
            lblErrorMessageRegion.Visible = true;
            lblErrorMessageRegion.Text = errormsg;
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
                FuncPriBindCurrencyGrid();
                tcRegBranch.ActiveTabIndex = 0;
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
                ((Label)gvRegionDetails.BottomPagerRow.FindControl("lblPagerSearch")).Text = " of  " + ((int)ViewState["TotalPageCount"] + 1).ToString().Trim();
                ((TextBox)gvRegionDetails.BottomPagerRow.FindControl("txtPagerSearch")).Text = lnkbtnPage.Text;
            }
            tcRegBranch.ActiveTabIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }






    #endregion

    #region BranchMaster

    /// <summary>
    /// Using header searching
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FunProBHeaderSearch(object sender, EventArgs e)
    {
        string strSearchVal = String.Empty;
        strSearchVal1 = String.Empty;
        strSearchVal2 = String.Empty;
        strSearchVal3 = String.Empty;
        strSearchVal4 = String.Empty;
        strSearchVal5 = String.Empty;

        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValueB();
            //Replace the corresponding fields needs to search in sqlserver
            if (strSearchVal1 != "")
            {
                strSearchVal += "Branch_Code like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and Branch_Name like '" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and Region_Code like '" + strSearchVal3 + "%'";
            }
            if (strSearchVal4 != "")
            {
                strSearchVal += " and Region_Name like '" + strSearchVal4 + "%'";
            }
            if (strSearchVal5 != "")
            {
                strSearchVal += " and State_Name like '" + strSearchVal5 + "%'";
            }
            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            //if ((strSearchVal.Length > 4) && (strSearchVal.Substring(0, 4) == " and"))
            //{
            //    strSearchVal = strSearchVal.Replace(" and ", "");
            //}
            hdnSearchBranch.Value = strSearchVal;
            FunGetBranchDetails();
            FunPriSetSearchValueB();
            if (txtboxSearch.Text != "")
                ((TextBox)gvBranch.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
            tcRegBranch.ActiveTabIndex = 1;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    //protected void FunProBHeaderSearch(object sender, EventArgs e)
    //{
    //    string strSearchVal = String.Empty;
    //    string strSearchVal1 = String.Empty;
    //    string strSearchVal2 = String.Empty;
    //    string strSearchVal3 = String.Empty;
    //    string strSearchVal4 = String.Empty;
    //    string strSearchVal5 = String.Empty;
    //    lblErrorMessage.Text = "";
    //    lblErrorMessageRegion.Text = "";
    //    TextBox txtboxSearch;
    //    try
    //    {
    //        tcRegBranch.ActiveTabIndex = 1;
    //        txtboxSearch = ((TextBox)sender);
    //        ViewState["txtSearchValue"] = txtboxSearch.Text.Trim();
    //        ProCurrentBPageRW = 0;
    //        strSearchVal1 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch3")).Text.Trim();
    //        strSearchVal2 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch4")).Text.Trim();
    //        strSearchVal3 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch5")).Text.Trim();
    //        strSearchVal4 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch6")).Text.Trim();
    //        strSearchVal5 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeaderSearch7")).Text.Trim();

    //        if (strSearchVal1 != "" || strSearchVal1 != String.Empty)
    //        {
    //            strSearchColName = "Branch_Code";
    //            strSearchVal += "Branch_Code like '" + strSearchVal1 + "%'";
    //        }
    //        if (strSearchVal2 != "" || strSearchVal2 != String.Empty)
    //        {
    //            strSearchColName = "Branch_Name";
    //            strSearchVal += "and Branch_Name like '" + strSearchVal2 + "%'";
    //        }
    //        if (strSearchVal3 != "" || strSearchVal3 != String.Empty)
    //        {
    //            strSearchColName = "Region_Code";
    //            strSearchVal += "Region_Code like '" + strSearchVal3 + "%'";
    //        }
    //        if (strSearchVal4 != "" || strSearchVal4 != String.Empty)
    //        {
    //            strSearchColName = "Region_Name";
    //            strSearchVal += "and Region_Name like '" + strSearchVal4 + "%'";
    //        }
    //        if (strSearchVal5 != "" || strSearchVal5 != String.Empty)
    //        {
    //            strSearchColName = "State_Name";
    //            strSearchVal += "and State_Name like '" + strSearchVal5 + "%'";
    //        }


    //        if (ViewState["SearchBCurrencyGrid"] != null)
    //        {
    //            DataView dtProductView = FuncPriBFilterGrid((DataTable)ViewState["SearchBCurrencyGrid"], strSearchColName, strSearchVal);
    //            if (dtProductView.Count > 0)
    //            {

    //                gvBranch.DataSource = FunPriBGetPagedSource(dtProductView);
    //                gvBranch.DataBind();
    //                //Setting this Session Value since we need to Show only Searched Records
    //                Session["AfterBCurrencySearch"] = "Yes";
    //            }
    //            else
    //            {
    //                //lblErrorMessage.Text = "No records found";
    //               // FunPriErrorMessage("No records found");
    //                FunGetBranchDetails();
    //            }

    //        }
    //        ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal1;
    //        ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal2;
    //        ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal3;
    //        ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch6")).Text = strSearchVal4;
    //        ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch7")).Text = strSearchVal5;
    //        if (txtboxSearch.Text != "")
    //            ((TextBox)gvBranch.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

    //    }
    //    catch (Exception ex)
    //    {
    //        //lblErrorMessage.Text = "No Records Found";
    //        FunPriErrorMessage("No Records Found");
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    /// <summary>
    /// Using sorting
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        var imgbtnSearch = string.Empty;
        // ImageButton imgbtnSearch = (ImageButton)sender;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");
            switch (lnkbtnSearch.ID)
            {
                case "lnkbtnRegionCode":
                    strSortColName = "Region_Code";
                    break;
                case "lnkbtnRegionName":
                    strSortColName = "Region_Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunGetRegionDetails();
            if (strDirection == "ASC")
            {
                ((ImageButton)gvRegionDetails.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)gvRegionDetails.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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
            //    gvRegionDetails.DataSource = FunPriGetPagedSource(dvSearchView);
            //    gvRegionDetails.DataBind();


            //    if (strDirection == "ASC")
            //    {
            //        ((ImageButton)gvRegionDetails.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            //        //((ImageButton)gvRegionDetails.HeaderRow.FindControl(imgbtnSearch.ID)).ImageUrl = "~/Images/ArrowUp.gif";
            //    }
            //    else
            //    {
            //        ((ImageButton)gvRegionDetails.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            //        //((ImageButton)gvRegionDetails.HeaderRow.FindControl(imgbtnSearch.ID)).ImageUrl = "~/Images/ArrowDown.gif";
            //    }
            //}
            tcRegBranch.ActiveTabIndex = 0;
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


    /// <summary>
    /// using branch sorting 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FunProBSortingColumn(object sender, EventArgs e)
    {
        //ImageButton imgbtnSearch = (ImageButton)sender;

        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");
            switch (lnkbtnSearch.ID)
            {
                case "lnkbtnBranchCode":
                    strSortColName = "Branch_Code";
                    break;
                case "lnkbtnBranchName":
                    strSortColName = "Branch_Name";
                    break;
                case "lnkbtnRegionCodeBTab":
                    strSortColName = "Region_Code";
                    break;
                case "lnkbtnRegionNameBTab":
                    strSortColName = "Region_Name";
                    break;
                case "lnkbtnStateCode":
                    strSortColName = "State_Name";
                    break;
            }

            string strDirection = FunPriBGetSortDirectionStr(strSortColName);
            FunGetBranchDetails();

            if (strDirection == "ASC")
            {
                ((ImageButton)gvBranch.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)gvBranch.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }

            tcRegBranch.ActiveTabIndex = 1;
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
    /// <summary>
    /// using branch fillter in grid
    /// </summary>
    /// <param name="dtTable"></param>
    /// <param name="strColumnName"></param>
    /// <param name="strSearchExp"></param>
    /// <returns></returns>
    private DataView FuncPriBFilterGrid(DataTable dtTable, string strColumnName, string strSearchExp)
    {
        if (dtTable != null || dtTable.Rows.Count > 0)
        {
            if (Session["SearchBCurrencyDataView"] == null)
            {
                dvSearchView = new DataView(dtTable);
            }
            else
            {
                dvSearchView = (DataView)Session["SearchBCurrencyDataView"];

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
        Session["SearchBCurrencyDataView"] = dvSearchView;
        return dvSearchView;
    }

    public int ProCurrentBPageRW
    {
        get
        {
            if (this.ViewState["CurrentBPageRW"] == null)
                return 0;
            else
                return Convert.ToInt32(this.ViewState["CurrentBPageRW"].ToString());
        }
        set
        {
            this.ViewState["CurrentBPageRW"] = value;
        }
    }
    /// <summary>
    /// Property Used for Page Count
    /// </summary>
    private Int32 ProPageBCountRW
    {

        get { return intCount; }
        set { intCount = value; }
    }
    private void FuncPriBindBCurrencyGrid()
    {
        if (ViewState["SearchBCurrencyGrid"] != null)
        {
            //Session AfterCurrencySearch Value is checked since we need to Show only Searched Records or All Records if Value is NO 
            if (Session["SearchBCurrencyDataView"] == null || Session["AfterBCurrencySearch"].ToString() == "No")
            {
                dvSearchView = ((DataTable)ViewState["SearchBCurrencyGrid"]).DefaultView;
            }
            else
            {
                dvSearchView = (DataView)Session["SearchBCurrencyDataView"];

            }
            if (this.ViewState["strSortExpression"] != null)
            {
                dvSearchView.Sort = this.ViewState["strSortExpression"].ToString() + " " + this.ViewState["strSortDirection"].ToString();
            }
            gvBranch.DataSource = FunPriBGetPagedSource(dvSearchView);
            gvBranch.DataBind();

        }
    }
    private PagedDataSource FunPriBGetPagedSource(DataView dvData)
    {
        pdsPagerDataSource = new PagedDataSource();
        pdsPagerDataSource.DataSource = dvData;
        pdsPagerDataSource.AllowPaging = true;
        pdsPagerDataSource.PageSize = 10;
        pdsPagerDataSource.CurrentPageIndex = ProCurrentBPageRW;
        //imgbtnBranchNext.Enabled = !pdsPagerDataSource.IsLastPage;
        //imgbtnBranchPrev.Enabled = !pdsPagerDataSource.IsFirstPage;
        //imgbtnBranchLast.Enabled = !pdsPagerDataSource.IsLastPage;
        //imgbtnBranchFirst.Enabled = !pdsPagerDataSource.IsFirstPage;
        if (pdsPagerDataSource.PageCount == 0)
        { ViewState["TotalPageBCount"] = 0; }
        else { ViewState["TotalPageCount"] = pdsPagerDataSource.PageCount - 1; }
        //FunBPridoPaging();
        Session["SearchBCurrencyDataView"] = dvData;
        return pdsPagerDataSource;
    }


    #endregion

    #endregion


    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValueB()
    {
        if (gvBranch.HeaderRow != null)
        {
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch6")).Text = "";
            ((TextBox)gvBranch.HeaderRow.FindControl("txtHeaderSearch7")).Text = "";
        }
    }
    /// <summary>
    /// show the records in click events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBranchShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearchBranch.Value = "";
            hdnOrderByBranch.Value = "";
            FunPriClearSearchValueB();
            FunGetBranchDetails();
            tcRegBranch.ActiveTabIndex = 1;
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

    /// <summary>
    /// using disply in first page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnBranchFirst_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            ProCurrentBPageRW = 0;
            ViewState["dtPaging"] = null;
            FuncPriBindBCurrencyGrid();
            tcRegBranch.ActiveTabIndex = 1;
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// using disply in preview page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnBranchPrev_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            ProCurrentBPageRW -= 1;
            ProPageBCountRW = Convert.ToInt32(ViewState["dlstBranchPager_ItemDataBound"]);
            ViewState["imgbtnBranchPrev_Click"] = true;
            FuncPriBindBCurrencyGrid();
            tcRegBranch.ActiveTabIndex = 1;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// using disply in last page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnBranchLast_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            ProCurrentBPageRW -= 1;
            ProPageBCountRW = Convert.ToInt32(ViewState["dlstBranchPager_ItemDataBound"]);
            ViewState["imgbtnBranchLast_Click"] = true;
            FuncPriBindBCurrencyGrid();
            tcRegBranch.ActiveTabIndex = 1;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    /// <summary>
    /// using disply in next page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnBranchNext_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            ProCurrentBPageRW += 1;
            ProPageBCountRW = Convert.ToInt32(ViewState["dlstBranchPager_ItemDataBound"]);
            ViewState["imgbtnBranchNext_Click"] = true;
            FuncPriBindBCurrencyGrid();
            tcRegBranch.ActiveTabIndex = 1;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }


    protected void dlstBranchPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("lnkbtnBranchPaging"))
            {
                ProPageBCountRW = Convert.ToInt32(e.CommandArgument);
                ProCurrentBPageRW = Convert.ToInt32(e.CommandArgument.ToString());
                FuncPriBindBCurrencyGrid();
                tcRegBranch.ActiveTabIndex = 1;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void dlstBranchPager_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        System.Web.UI.WebControls.LinkButton lnkbtnBPage;

        try
        {
            lnkbtnBPage = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkbtnBranchPaging");
            if (lnkbtnBPage.Text == Convert.ToString(ProCurrentBPageRW + 1))
            {
                lnkbtnBPage.Enabled = false;
                ViewState["dlstBranchPager_ItemDataBound"] = Convert.ToInt32(((Label)e.Item.FindControl("lblCount")).Text.Trim());
                ((Label)gvBranch.BottomPagerRow.FindControl("lblPagerSearch")).Text = " of  " + ((int)ViewState["TotalBPageCount"] + 1).ToString().Trim();
                ((TextBox)gvBranch.BottomPagerRow.FindControl("txtPagerSearch")).Text = lnkbtnBPage.Text;
            }
            tcRegBranch.ActiveTabIndex = 1;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void tcRegBranch_ActiveTabChanged(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
        lblErrorMessageRegion.Text = "";
    }

    /// <summary>
    /// passing the value region id 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvRegionDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string qsTab = "&qsTab=region";
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsRegion_ID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M" + qsTab);
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsRegion_ID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q" + qsTab);
                break;

        }

    }
    /// <summary>
    ///  passing the value branch id 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvBranch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string qsTab = "&qsTab=branch";
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageBranchAdd + "?qsBranch_ID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M" + qsTab);
                break;
            case "query":
                Response.Redirect(strRedirectPageBranchAdd + "?qsBranch_ID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q" + qsTab);
                break;

        }

    }
}
