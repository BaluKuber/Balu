using S3GBusEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoanAdmin_S3GLoanAd_RSChargeMain_View : ApplyThemeForProject
{
    #region Intialization

    string strRedirectPage = "~/LoanAdmin/S3GLoanAd_RSChargeMain_Add.aspx";
    string strDateFormat = string.Empty;

    string strSortColName;

    int intCompanyID = 0;
    int intUserId = 0;

    UserInfo ObjUserInfo = null;

    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;    
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;

    int intUserID = 0;

    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bMakerChecker = false;
    bool bIsActive = false;

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

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

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
        intUserId = ObjUserInfo.ProUserIdRW;
        lblErrorMessage.InnerText = "";

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
            FunPriBindGrid();

            if (!bIsActive)
            {
                grvRSChargeMaintenance.Columns[1].Visible = false;
                grvRSChargeMaintenance.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }

            if (!bModify)
            {
                grvRSChargeMaintenance.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvRSChargeMaintenance.Columns[0].Visible = false;
            }
            if (!bCreate)
            {
                btnCreate.Enabled = false;
            }
        }
    }

    #endregion

    #region Page Methods

    public string ShowDate(string dt)
    {
        if (!dt.Equals(""))
        {
            return DateTime.Parse(dt, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
        }
        else
            return "";
    }

    /// <summary>
    /// This method is used to display TaxExemption details
    /// </summary>

    private void FunPriBindGrid()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            #region Paging Properties set

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            #endregion //Paging Properties end

            #region Paging Config
            FunPriGetSearchValue();

            #region To Show Grid Header
            bool bIsNewRow = false;

            grvRSChargeMaintenance.BindGridView("S3G_LAD_Get_RSCM_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            #endregion

            #region To Hide First Row if grid is empty

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvRSChargeMaintenance.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            #endregion
            #endregion Paging Config End

        }
        catch (FaultException<OrgMasterMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {

        }
    }

    #endregion

    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>
    /// 
    private void FunPriGetSearchValue()
    {
        if (grvRSChargeMaintenance.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim().Replace("'", "''");            
            strSearchVal3 = ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim().Replace("'", "''");
            strSearchVal4 = ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim().Replace("'", "''");
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (grvRSChargeMaintenance.HeaderRow != null)
        {
            ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";            
            ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvRSChargeMaintenance.HeaderRow != null)
        {
            ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;            
            ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
        }
    }

    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>

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
                strSearchVal += " and RSCM_CODE like '%" + strSearchVal1 + "%'";
            }           
            if (strSearchVal3 != "")
            {
                strSearchVal += " and Customer_Name like '%" + strSearchVal3 + "%'";
            }
            if (strSearchVal4 != "")
            {
                strSearchVal += " and TH.Tranche_Name like '%" + strSearchVal4 + "%'";
            }
            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvRSChargeMaintenance.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
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
        if (strColumn == "RSCM_Code")
            strColumn = "RSCM_Code";        
        if (strColumn == "Customer_Name")
            strColumn = "Customer_Name";
        if (strColumn == "Tranche_Name")
            strColumn = "Tranche_Name";
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
                case "lnkbtnRSCMCode":
                    strSortColName = "RSCM_Code";
                    break;                
                case "lnkbtnSortCustomer_Name":
                    strSortColName = "Customer_Name";
                    break;
                case "lnkbtnSortTranche":
                    strSortColName = "Tranche_Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvRSChargeMaintenance.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {
                ((ImageButton)grvRSChargeMaintenance.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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

    #endregion

    #region Page Events

    #region  BUTTON EVENTS

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

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
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region GRID EVENTS

    protected void grvRSChargeMaintenance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPage + "?qsRSCMID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPage + "?qsRSCMID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;
        }
    }

    protected void grvRSChargeMaintenance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            if (lblUserID.Text != "")
            {
                //Modified by saranya 10-Feb-2012 to validate user based on user level and Maker Checker

                if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text), true)))
                {
                    imgbtnEdit.Enabled = true;
                }
                else
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
        }
    }

    #endregion

    #endregion
}