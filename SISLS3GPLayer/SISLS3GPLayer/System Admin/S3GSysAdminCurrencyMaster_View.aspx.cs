
/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminCurrencyMaster_View
/// Created By      :   Kaliraj K
/// Created Date    :   11-MAY-2010
/// Purpose         :   To view currency master details
/// Modified By      :   Kaliraj K
/// Created Date    :   26-Jul-2010
/// Purpose         :   To implement user authorization

using System;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Text;
using System.Configuration;
using System.Web.Security;


public partial class S3GSysAdminCurrencyMaster_View : ApplyThemeForProject
{
    #region Intialization
    AccountMgtServicesReference.AccountMgtServicesClient objCurrencyMasterClient;
    AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable ObjS3G_SYSAD_CurrencyMaster_ViewDataTable = new AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable();
    AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable ObjS3G_SYSAD_CurrencyMasterDataTable = new AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable();
    S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrency;
    string strRedirectPage = "~/System Admin/S3GSysAdminCurrencyMaster_Add.aspx";
    string strSearchColName;
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

    //User Authorization variable declaration
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    //Declaration end

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

        //User Authorization
        ObjUserInfo = new UserInfo();
        intCompanyID = Convert.ToInt32(ObjUserInfo.ProCompanyIdRW);
        intUserID = Convert.ToInt32(ObjUserInfo.ProUserIdRW);

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        //bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        //Authorization Code end

        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);

            FunPriBindGrid();

            //User Authorization
            if (!bIsActive)
            {
                grvCurrencyMaster.Columns[1].Visible = false;
                grvCurrencyMaster.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }
            if (!bModify)
            {
                grvCurrencyMaster.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvCurrencyMaster.Columns[0].Visible = false;
            }
            if (!bCreate)
            {
                btnCreate.Enabled = false;
            }
            //Authorization Code end
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// This method is used to display currency details
    /// </summary>
    private void FunPriBindGrid()
    {
        objCurrencyMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            AccountMgtServices.S3G_SYSAD_CurrencyMasterRow ObjCurrencyMasterRow;
            ObjS3G_SYSAD_CurrencyMasterDataTable = new AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable();
            ObjCurrencyMasterRow = ObjS3G_SYSAD_CurrencyMasterDataTable.NewS3G_SYSAD_CurrencyMasterRow();
            ObjCurrencyMasterRow.Company_ID = intCompanyID;
            ObjCurrencyMasterRow.Currency_ID = 0;

            //Paging Properties set
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            //Paging code end

            ObjS3G_SYSAD_CurrencyMasterDataTable.AddS3G_SYSAD_CurrencyMasterRow(ObjCurrencyMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteCurrencyDetails = objCurrencyMasterClient.FunPubQueryCurrencyPaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_CurrencyMasterDataTable, SerMode), ObjPaging);

            AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrency = (AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteCurrencyDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable));

            DataView dvCurrency = dtCurrency.DefaultView;

            //Paging Config

            FunPriGetSearchValue();

            //This is to show grid header
            bool bIsNewRow = false;
            if (dvCurrency.Count == 0)
            {
                dvCurrency.AddNew();
                bIsNewRow = true;
            }

            grvCurrencyMaster.DataSource = dvCurrency;
            grvCurrencyMaster.DataBind();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvCurrencyMaster.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            objCurrencyMasterClient.Close();
        }

    }

    #endregion


    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        if (grvCurrencyMaster.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
            strSearchVal2 = ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
            //strSearchVal3 = ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();
            //strSearchVal4 = ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
            //strSearchVal5 = ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (grvCurrencyMaster.HeaderRow != null)
        {
            ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
            //((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            //((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            //((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvCurrencyMaster.HeaderRow != null)
        {
            ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
            //((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            //((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            //((TextBox)grvCurrencyMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
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
                strSearchVal += "Currency_Code like '%" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and Currency_Name like '%" + strSearchVal2 + "%'";
            }
            //if (strSearchVal3 != "")
            //{
            //    strSearchVal += " and Currency_Name like '" + strSearchVal3 + "%'";
            //}
            //if (strSearchVal4 != "")
            //{
            //    strSearchVal += " and Currency_Name like '" + strSearchVal4 + "%'";
            //}
            //if (strSearchVal5 != "")
            //{
            //    strSearchVal += " and Currency_Name like '" + strSearchVal5 + "%'";
            //}

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvCurrencyMaster.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

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
                case "lnkbtnCurrencyCode":
                    strSortColName = "Currency_Code";
                    break;
                case "lnkbtnCurrencyName":
                    strSortColName = "Currency_Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvCurrencyMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvCurrencyMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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
        Response.Redirect(strRedirectPage,false);
    }

    protected void grvCurrencyMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");

            //User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }
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

    protected void grvCurrencyMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPage + "?qsCurrencyId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPage + "?qsCurrencyId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;

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

    public string FunPriGetURL(string strId)
    {
        return (strRedirectPage + "?qsCurrencyId=" + strId + "&mode=Q");
    }

    #endregion



}
