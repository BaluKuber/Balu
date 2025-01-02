
/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminTaxGuide_View
/// Created By      :   Kaliraj K
/// Created Date    :   27-MAY-2010
/// Purpose         :   To view tax guide details

using System;
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


public partial class S3GSysAdminTaxGuide_View : ApplyThemeForProject
{
    #region Intialization
    AccountMgtServicesReference.AccountMgtServicesClient objTaxGuideClient;
    //AccountMgtServicesReference.AccountMgtServicesClient objTaxGuideClient=new AccountMgtServicesReference.AccountMgtServicesClient();
    AccountMgtServices.S3G_SYSAD_TaxGuideDataTable ObjS3G_SYSAD_TaxGuideDataTable = new AccountMgtServices.S3G_SYSAD_TaxGuideDataTable();
    S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable dtTaxGuide;
    SerializationMode SerMode = SerializationMode.Binary;
    string strRedirectPage = "~/System Admin/S3GSysAdminTaxGuide_Add.aspx";
    string strSearchColName;
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
    string strSearchVal5 = string.Empty;
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
        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
            FunPriBindGrid();

            if (!bIsActive)
            {
                grvTaxGuide.Columns[1].Visible = false;
                grvTaxGuide.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }

            if (!bModify)
            {
                grvTaxGuide.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvTaxGuide.Columns[0].Visible = false;
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
    /// This method is used to display TaxGuide details
    /// </summary>

    private void FunPriBindGrid()
    {
        AccountMgtServicesReference.AccountMgtServicesClient objTaxGuideClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            AccountMgtServices.S3G_SYSAD_TaxGuideRow ObjTaxGuideRow;
            ObjTaxGuideRow = ObjS3G_SYSAD_TaxGuideDataTable.NewS3G_SYSAD_TaxGuideRow();
            ObjTaxGuideRow.Company_ID = intCompanyID;
            ObjTaxGuideRow.Tax_Code_ID = 0;

            //Paging Properties set

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = " USER1.USER_ID=" + ObjUserInfo.ProUserIdRW.ToString() + hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            //Paging Properties end


            ObjS3G_SYSAD_TaxGuideDataTable.AddS3G_SYSAD_TaxGuideRow(ObjTaxGuideRow);
            byte[] byteTaxGuideDetails = objTaxGuideClient.FunPubQueryTaxGuidePaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_TaxGuideDataTable, SerMode), ObjPaging);

            AccountMgtServices.S3G_SYSAD_TaxGuideDataTable dtTaxGuide = (AccountMgtServices.S3G_SYSAD_TaxGuideDataTable)ClsPubSerialize.DeSerialize(byteTaxGuideDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_TaxGuideDataTable));
            DataView dvTaxGuide = dtTaxGuide.DefaultView;
            //Paging Config

            FunPriGetSearchValue();

            //This is to show grid header
            bool bIsNewRow = false;
            if (dvTaxGuide.Count == 0)
            {
                dvTaxGuide.AddNew();
                bIsNewRow = true;
            }

            grvTaxGuide.DataSource = dvTaxGuide;
            grvTaxGuide.DataBind();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvTaxGuide.Rows[0].Visible = false;
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
            objTaxGuideClient.Close();
        }
    }

    #endregion

    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        if (grvTaxGuide.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim().Replace("'", "''");
            strSearchVal2 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim().Replace("'", "''");
            strSearchVal3 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim().Replace("'", "''");
            strSearchVal4 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim().Replace("'", "''");
            strSearchVal5 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim().Replace("'", "''");
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (grvTaxGuide.HeaderRow != null)
        {
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvTaxGuide.HeaderRow != null)
        {
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
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
                strSearchVal += " and LC.LocationCat_Description like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and Tax_Code like '" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and LKUP.NAME like '" + strSearchVal3 + "%'";
            }
            if (strSearchVal4 != "")
            {
                if (strSearchVal4.ToLower().Contains("purchase"))
                    strSearchVal += " and TGH.Tax_Class=1 ";
                if (strSearchVal4.ToLower().Contains("sales"))
                    strSearchVal += " and TGH.Tax_Class=2 ";
            }
            if (strSearchVal5 != "")
            {
                strSearchVal += " and LOB.LOB_Name like '" + strSearchVal5 + "%'";
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvTaxGuide.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
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
        if (strColumn == "State_Name")
            strColumn = "State_Name";
        if (strColumn == "Tax_Code")
            strColumn = "Tax_Code";
        if (strColumn == "TaxType")
            strColumn = "TaxType";
        if (strColumn == "TaxClass")
            strColumn = "TaxClass";
        if (strColumn == "LOB_Name")
            strColumn = "LOB_Name";
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
                case "lnkbtnstate":
                    strSortColName = "State_Name";
                    break;
                case "lnkbtnSortCode":
                    strSortColName = "Tax_Code";
                    break;
                case "lnkbtnSortTaxType":
                    strSortColName = "TaxType";
                    break;
                case "lnkbtnTaxClass":
                    // if (strSortColName.ToLower().Contains("purchase"))
                    strSortColName = "TaxClass";
                    //if (strSortColName.ToLower().Contains("sales"))
                    //strSortColName = " and TGH.Tax_Class=2 ";                    
                    break;
                case "lnkbtnSortName":
                    strSortColName = "LOB_Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvTaxGuide.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvTaxGuide.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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

    protected void grvTaxGuide_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPage + "?qsTaxCodeId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPage + "?qsTaxCodeId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;
        }
    }

    protected void grvTaxGuide_RowDataBound(object sender, GridViewRowEventArgs e)
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
                //if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
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
