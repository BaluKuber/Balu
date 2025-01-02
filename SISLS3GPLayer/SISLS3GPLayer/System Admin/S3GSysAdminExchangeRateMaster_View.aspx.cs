#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   System Admin
/// Screen Name         :   S3GSysAdminExchangeMaster_View
/// Created By          :   Suresh P
/// Created Date        :   22-MAY-2010
/// Purpose             :   To view Exchange master details
/// Last Updated By		:   Suresh P
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.ServiceModel;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Data;
using AccountMgtServicesReference;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Web.Security;
#endregion

public partial class S3GSysAdminExchangeMaster_View : ApplyThemeForProject
{
    #region Intialization
    string strRedirectPage = "~/System Admin/S3GSysAdminLOBMaster_Add.aspx";
    AccountMgtServicesClient ObjLOBMasterClient = null;
    AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable ObjS3G_LOBMaster_CUDataTable = new AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable();

    string strRedirectPageAdd = "~/System Admin/S3GSysAdminExchangeRateMaster_Add.aspx";

    #endregion

    #region Paging Config
    string strDateFormat;

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

    //string strSearchColName;
    //PagedDataSource pdsPagerDataSource;
    //DataView dvSearchView;
    //private Int32 intCount;
    //string strSortColName;

    private string strDefaultCurrency;
    public string DefaultCurrency
    {
        get
        {
            return strDefaultCurrency;
        }
        set
        {
            strDefaultCurrency = value;
        }
    }



    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        lblErrorMessage.InnerText = "";
        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);

        //grvLOBMaster.Columns[6].ItemStyle.dat
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
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
        bDelete = ObjUserInfo.ProDeleteRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

        this.DefaultCurrency = ObjS3GSession.ProCurrencyCodeRW + " - " + ObjS3GSession.ProCurrencyNameRW;
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
    public string ShowDate(string dt)
    {
        if (!dt.Equals(""))
        {
            return DateTime.Parse(dt, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
        }
        else
            return "";
    }

    public string ShowTime(string dt)
    {
        if (!dt.Equals(""))
        {
            //return Convert.ToDateTime(Eval("Created_On")).ToString("hh:mm:ss tt")
            return DateTime.Parse(dt, CultureInfo.CurrentCulture.DateTimeFormat).ToString("hh:mm:ss tt");
        }
        else
            return "";
    }

    /// <summary>
    /// This method is used to display ExchangeRateMaster details
    /// </summary>
    private void FunPriBindGrid()
    {
        ObjLOBMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {

            int intTotalRecords = 0;

            AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow ObjLOBMasterRow = ObjS3G_LOBMaster_CUDataTable.NewS3G_SYSAD_ExchangeRateMasterRow();

            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            ObjS3G_LOBMaster_CUDataTable.AddS3G_SYSAD_ExchangeRateMasterRow(ObjLOBMasterRow);


            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteLOBDetails = ObjLOBMasterClient.FunPubQueryExchangeRatePaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMaster_CUDataTable, SerMode), ObjPaging);

            AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable dtLOB = (AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, SerMode, typeof(AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable));
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
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
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
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();

            //Replace the corresponding fields needs to search in sqlserver
            if (strSearchVal1 != "")
            {
                //strSearchVal += "Exchange_Currency_Name like '" + strSearchVal1 + "%'";
                strSearchVal += "UPPER(CM.Currency_Code) + ' - ' + UPPER(CM.Currency_Name) LIKE '" + strSearchVal1 + "%'";

            }
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
                    //strSortColName = " Exchange_Currency_Name ";
                    strSortColName = "(UPPER(Currency_Code) + ' - ' + UPPER(Currency_Name))";

                    break;
                /*case "lnkbtnSortName":
                    strSortColName = "UTPA_Name";
                    break;*/
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
    protected string DefaultCurrencyDisplay()
    {
        return this.DefaultCurrency;
    }

    #endregion

    #region Page Events

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminExchangeRateMaster_Add.aspx");
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


    protected void grvLOBMaster_DataBound(object sender, EventArgs e)
    {
        if (grvLOBMaster.Rows.Count > 0)
        {
            grvLOBMaster.UseAccessibleHeader = true;
            grvLOBMaster.HeaderRow.TableSection = TableRowSection.TableHeader;
            grvLOBMaster.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }

    public string SetExcSuffix(int suffix)
    {
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
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
            //Label lblActive = (Label)e.Row.FindControl("lblActive");
            //CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
            //if (lblActive.Text == "True")
            //{
            //chkAct.Checked = true;
            //}

            //User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            Label lblPrecision = (Label)e.Row.FindControl("lblPrecision");
            
            Label lblExchangeValue = (Label)e.Row.FindControl("lblLOBName");
            lblExchangeValue.Text = Convert.ToDecimal(string.IsNullOrEmpty(lblExchangeValue.Text) ? "0" : lblExchangeValue.Text).ToString(SetExcSuffix(Convert.ToInt32(string.IsNullOrEmpty(lblPrecision.Text) ? "0" : lblPrecision.Text.Trim())));

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

    protected void grvLOBMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string qsTab = "&qsTab=region";
        qsTab = "";
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsER=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M" + qsTab);
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsER=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q" + qsTab);
                break;

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strId"></param>
    /// <returns></returns>
    public string FunPriGetURL(string strId)
    {
        return (strRedirectPage + "?qsLOBID=" + strId + "&mode=Q");
    }

    #endregion




}
