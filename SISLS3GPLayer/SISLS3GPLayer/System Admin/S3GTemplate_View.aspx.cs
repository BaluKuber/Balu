
#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         : System Admin  
/// Screen Name         : Template Creation View Page
/// Created By          : M.Saran
/// Created Date        : 28-Jul-2012 
/// <Program Summary>
#endregion



#region [Namespaces]

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ServiceModel;
using S3GBusEntity;
using System.Collections.Generic;

#endregion [Namespaces]


public partial class System_Admin_S3GTemplate_View : ApplyThemeForProject
{

    string strRedirectPage = "~/System Admin/S3GTemplate_Add.aspx";
    #region [Paging Config]


    int intNoofSearch = 4;
    //string[] arrSortCol = new string[] { "DebtCollector_Code", "LOB_Code", "Region_Code", "Branch_Code" };
    string[] arrSortCol = new string[] { "LOB_Name", "LocationCat_Description", "Tmp_Doc_No", "Lookup_Description" };
    string strProcName = "S3G_SYSAD_TMPLTE_PAGING";
    Dictionary<string, string> Procparam = null;

    ArrayList arrSearchVal = new ArrayList(4);
    int intUserID = 0;
    int intCompanyID = 0;
    //User Authorization variable declaration
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
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

    #endregion [Paging Config]



    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriLoadPage();
    }

    #region [Grid Event's]

    protected void grvTemplatePaging_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPage + "?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M", false);
                break;
            case "query":
                Response.Redirect(strRedirectPage + "?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q", false);
                break;
        }

    }

    protected void grvTemplatePaging_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkActive = (CheckBox)e.Row.FindControl("chkIsActive");
            if (lblActive.Text.ToLower() == "true")
            {
                chkActive.Checked = true;
            }
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

    #endregion [Grid Event's]

    #region [Button Event's]

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
            cvErrormsg.ErrorMessage = ex.Message;
            cvErrormsg.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion [Button Event's]



    #region [Function's]


    private void FunPriLoadPage()
    {
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        #region [Paging Config]

        arrSearchVal = new ArrayList(intNoofSearch);
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
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

        #endregion  [Paging Config]

        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            FunPriBindGrid();
            //User Authorization
            if (!bIsActive)
            {
                grvTemplatePaging.Columns[1].Visible = false;
                grvTemplatePaging.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }
            if (!bModify)
            {
                grvTemplatePaging.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvTemplatePaging.Columns[0].Visible = false;
            }
            if (!bCreate)
            {
                btnCreate.Enabled = false;
            }
            //Authorization Code end
        }
    }


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

            grvTemplatePaging.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvTemplatePaging.Rows[0].Visible = false;
            }
            FunPriSetSearchValue();
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            cvErrormsg.ErrorMessage = objFaultExp.Detail.ProReasonRW;
            cvErrormsg.IsValid = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvErrormsg.ErrorMessage = ex.Message;
            cvErrormsg.IsValid = false;
        }
    }

    #region [Paging and Searching Methods For Grid]

    private void FunPriGetSearchValue()
    {
        arrSearchVal = grvTemplatePaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        grvTemplatePaging.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        grvTemplatePaging.FunPriSetSearchValue(arrSearchVal);
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
                ((TextBox)grvTemplatePaging.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
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
                ((ImageButton)grvTemplatePaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {
                ((ImageButton)grvTemplatePaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            cvErrormsg.ErrorMessage = objFaultExp.Detail.ProReasonRW;
            cvErrormsg.IsValid = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvErrormsg.ErrorMessage = ex.Message;
            cvErrormsg.IsValid = false;
        }
    }

    #endregion [Paging and Searching Methods For Grid]

    #endregion [Function's]




}
