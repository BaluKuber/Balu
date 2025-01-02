#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Origination
/// Screen Name         :   S3GORGPaymentRuleCard_View
/// Created By          :   Suresh P
/// Created Date        :   22-Jun-2010
/// Purpose             :   To view PaymentRuleCard details
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI.WebControls;
using S3GBusEntity;
#endregion

public partial class S3GORGPaymentRuleCard_View : ApplyThemeForProject
{
    #region Paging Config
    string strRedirectPage = "~/Origination/S3GORGPaymentRuleCard_Add.aspx";
    int intNoofSearch;
    string[] arrSortCol = new string[] { "LOB.LOB_CODE + ' - ' +  LOB.LOB_NAME", "Name", "Payment_Rule_Number" };
    string strProcName = "S3G_ORG_GetPaymentRuleCard_Paging";
    Dictionary<string, string> Procparam = null;
    static string strPageName = "Payment Rule Card";
    ArrayList arrSearchVal = new ArrayList(1);
    int intUserID = 0;
    int intCompanyID = 0;

    //User Authorization variable declaration
    UserInfo ObjUserInfo = new UserInfo();
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

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            intNoofSearch = arrSortCol.Length;
            arrSearchVal = new ArrayList(intNoofSearch);

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
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            //Authorization Code end

            lblErrorMessage.InnerText = "";
            if (!IsPostBack)
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                FunPriBindGrid();

                //User Authorization
                if (!bIsActive)
                {
                    grvPaging.Columns[1].Visible = false;
                    grvPaging.Columns[0].Visible = false;
                    btnCreate.Enabled = false;
                    return;
                }
                else
                {
                    if (!bModify)
                    {
                        grvPaging.Columns[1].Visible = false;
                    }
                    if (!bQuery)
                    {
                        grvPaging.Columns[0].Visible = false;
                    }
                    if (!bCreate)
                    {
                        btnCreate.Enabled = false;
                    }
                    //Authorization Code end
                }
            }
        }

        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
    }
    #endregion

    #region Page Methods
    /// <summary>
    /// This method is used to display LOB details
    /// </summary>
    private void FunPriBindGrid()
    {
        try
        {

            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            FunPriGetSearchValue();

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Payment_RuleCard_ID", "-1");

            grvPaging.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvPaging.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            lblErrorMessage.InnerText = "";
            //Paging Config End
        }
        catch (FaultException<RuleCardMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp, strPageName);
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
    }
    #endregion

    #region Paging and Searching Methods For Grid

    private void FunPriGetSearchValue()
    {
        try
        {
            arrSearchVal = grvPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    private void FunPriClearSearchValue()
    {
        try
        {
            grvPaging.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    private void FunPriSetSearchValue()
    {
        try
        {
            grvPaging.FunPriSetSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {
        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                }
            }

            //if (strSearchVal.StartsWith(" and "))
            //{
            //    strSearchVal = strSearchVal.Remove(0, 5);
            //}

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvPaging.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.InnerText = ex.Message;
        }
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
            // Retrieve the last strColumn that wasorted.
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
            lblErrorMessage.InnerText = ex.Message;
        }
        return strSortDirection;
    }

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (lnkbtnSearch.ID == "lnkbtnSort" + (iCount + 1).ToString())
                {
                    strSortColName = arrSortCol[iCount].ToString();
                    break;
                }
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
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
    protected void grvPaging_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //User Authorization
                Label lblUserID = (Label)e.Row.FindControl("lblUserID");
                Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

                if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
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
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
    public bool FunPriCheckBool(string strActive)
    {

        return ((strActive == "True") ? true : false);

    }
    protected void grvPaging_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            switch (e.CommandName.ToLower())
            {
                case "modify":
                    Response.Redirect(strRedirectPage + "?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                    break;
                case "query":
                    Response.Redirect(strRedirectPage + "?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                    break;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion



}
