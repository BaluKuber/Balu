using S3GBusEntity;
using S3GBusEntity.Origination;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Origination_S3G_ORG_TaxExemptionMaster_View : ApplyThemeForProject
{
    #region Intialization
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objTaxExemptionMasterClient;
    OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable ObjS3G_ORG_TaxExemptionMasterDataTable = new OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable();
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable dtTaxExemption;
    SerializationMode SerMode = SerializationMode.Binary;
    string strRedirectPage = "~/Origination/S3G_ORG_TaxExemptionMaster_Add.aspx";
    string strSearchColName;
    string strDateFormat = string.Empty;
    PagedDataSource pdsPagerDataSource;
    DataView dvSearchView;
    private Int32 intCount;
    string strSortColName;
    int intCompanyID = 0;
    int intUserId = 0;
    UserInfo ObjUserInfo = null;
    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    //string strSearchVal5 = string.Empty;
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
                grvTaxExemption.Columns[1].Visible = false;
                grvTaxExemption.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }

            if (!bModify)
            {
                grvTaxExemption.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvTaxExemption.Columns[0].Visible = false;
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

            grvTaxExemption.BindGridView("S3G_ORG_Get_TaxExmp_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            #endregion

            #region To Hide First Row if grid is empty

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvTaxExemption.Rows[0].Visible = false;
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

    private void FunPriGetSearchValue()
    {
        if (grvTaxExemption.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim().Replace("'", "''");
            strSearchVal2 = ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim().Replace("'", "''");
            strSearchVal3 = ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim().Replace("'", "''");
            strSearchVal4 = ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim().Replace("'", "''");
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (grvTaxExemption.HeaderRow != null)
        {
            ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
            ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvTaxExemption.HeaderRow != null)
        {
            ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
            ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            ((TextBox)grvTaxExemption.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
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
                strSearchVal += " and Tax_Code like '%" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and (CM.Customer_Code + ' - ' + CM.Customer_Name) like '%" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and TAN like '%" + strSearchVal3 + "%'";
            }
            if (strSearchVal4 != "")
            {
                strSearchVal += " and Tax_Law_Section like '%" + strSearchVal4 + "%'";
            }
            //if (strSearchVal5 != "")
            //{
            //    strSearchVal += " and Effective_From = '" + strSearchVal5 + "%'";
            //}
            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvTaxExemption.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
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
        if (strColumn == "Tax_Code")
            strColumn = "Tax_Code";
        if (strColumn == "Customer_Name")
            strColumn = "Customer_Name";
        if (strColumn == "TAN")
            strColumn = "TAN";
        if (strColumn == "Tax_Law_Section")
            strColumn = "Tax_Law_Section";
        if (strColumn == "Effective_From")
            strColumn = "Effective_From";
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
                case "lnkbtnTax_Exemption_Code":
                    strSortColName = "Tax_Code";
                    break;
                case "lnkbtnLessee":
                    strSortColName = "Customer_Name";
                    break;
                case "lnkbtnSortTAN":
                    strSortColName = "TAN";
                    break;
                case "lnkbtnSection":
                    strSortColName = "Tax_Law_Section";
                    break;
                case "lnkbtnFrom_Date":
                    strSortColName = "Effective_From";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvTaxExemption.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {
                ((ImageButton)grvTaxExemption.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

    protected void grvTaxExemption_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPage + "?qsTaxId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPage + "?qsTaxId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;
        }
    }

    protected void grvTaxExemption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
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
}