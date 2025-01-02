#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Fund Management 
/// Screen Name			: Funder Limit Management Approval
/// Created Date		: 13-Oct-2014
/// Purpose	            : 
#endregion

#region Namespaces
using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Collections;
using System.Configuration;
using System.ServiceModel;
using System.Collections.Generic;
#endregion

public partial class Fund_Management_S3G_FUNDMGT_FunderApproval_View : ApplyThemeForProject
{
    #region Paging Config

    string strRedirectPage = "~/Fund Management/S3G_FUNDMGT_FUNDERAPPROVAL_ADD.aspx";
    int intNoofSearch = 2;
    string[] arrSortCol = new string[] { "Customer_Name", "Funder_Name", "" };
    string strProcName = "S3G_FUNDMGT_GetFunderApprovalDetails_Paging";
    Dictionary<string, string> Procparam = null;

    ArrayList arrSearchVal = new ArrayList(1);
    int intUserID = 0;
    int intCompanyID = 0;
    PagingValues ObjPaging = new PagingValues();

    //User Authorization variable declaration
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    //Declaration end

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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            #region Paging Config
            arrSearchVal = new ArrayList(intNoofSearch);
            ProPageNumRW = 1;

            //User Authorization
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = false;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;

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

            if (!IsPostBack)
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                FunPriBindGrid();
            }
            //User Authorization
            if (!bIsActive)
            {
                grvPaging.Columns[1].Visible = false;
                grvPaging.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }
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
            FunPubSetIndex(1);
        }
        catch (Exception objException)
        {
            lblErrorMessage.InnerText = objException.Message;
        }
    }

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

                lblUserID.Text = (Convert.ToString(lblUserID.Text) == "") ? "0" : Convert.ToString(lblUserID.Text);

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
        catch (Exception objException)
        {
            lblErrorMessage.InnerText = objException.Message;
        }
    }

    protected void grvCustomerMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToLower() == "modify")
            {
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
                Response.Redirect(strRedirectPage + "?qsFunderApprovalId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
            }
            if (e.CommandName.ToLower() == "query")
            {
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
                Response.Redirect(strRedirectPage + "?qsFunderApprovalId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
            }
        }
        catch (Exception objException)
        {
            lblErrorMessage.InnerText = objException.Message;
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage + "?qsMode=C", false);
        }
        catch (Exception objException)
        {
            lblErrorMessage.InnerText = objException.Message;
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

    #endregion

    #region Page Methods

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
            //Procparam.Add("@Workflow_Sequence_ID", "0");

            grvPaging.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvPaging.Rows[0].Visible = false;
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

    }

    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        try
        {
            arrSearchVal = grvPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearSearchValue()
    {
        try
        {
            grvPaging.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriSetSearchValue()
    {
        try
        {
            grvPaging.FunPriSetSearchValue(arrSearchVal);
        }
        catch (Exception objException)
        {
            throw objException;
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

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvPaging.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #endregion
}