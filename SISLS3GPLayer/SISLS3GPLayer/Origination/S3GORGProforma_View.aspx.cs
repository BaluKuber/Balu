
/// Module Name     :   System Admin
/// Screen Name     :   S3GORGProforma_View.aspx
/// Created By      :   Kaliraj K
/// Created Date    :   14-Jul-2010
/// Purpose         :   To view proforma details

using System;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Collections;
using System.Collections.Generic;


public partial class S3GORGProforma_View : ApplyThemeForProject
{
    #region Paging Config

    string strRedirectPage = "~/Origination/S3GORGProforma_Add.aspx";
    int intNoofSearch = 4;
    string[] arrSortCol = new string[] { "LOB_Name", "Branch_Name", "Vendor_Name", "Proforma_No", "" };
    string strProcName = "S3G_ORG_GetProforma_Paging";
    Dictionary<string, string> Procparam = null;

    ArrayList arrSearchVal = new ArrayList(1);
    int intUserID = 0;
    int intCompanyID = 0;
    bool bModify = true;
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
        try
        {
            ProPageNumRW = intPageNum;
            ProPageSizeRW = intPageSize;
            FunPriBindGrid();
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    #endregion

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

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

            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            bModify = ObjUserInfo.ProModifyRW;

            if (!IsPostBack)
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                FunPriBindGrid();
                //if (!bModify)
                //{
                //    grvPaging.Columns[0].Visible = false;
                //}
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.InnerText = ae.Message.ToString();
            throw ae;           
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
            Procparam.Add("@Workflow_Sequence_ID", "0");

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
    #endregion

    #region Paging and Searching Methods For Grid

    
   private void FunPriGetSearchValue()
    {
        try
        {
            arrSearchVal = grvPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private void FunPriClearSearchValue()
    {
        try
        {
            grvPaging.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private void FunPriSetSearchValue()
    {
        try
        {
            grvPaging.FunPriSetSearchValue(arrSearchVal);
        }
        catch (Exception e)
        {
            throw e;
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
                    strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '" + arrSearchVal[iCount].ToString() + "%'";
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
                if (lnkbtnSearch.ID == "lnkbtnSort" + (iCount+1).ToString())
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

    #region Page Events

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ae)
        {
            throw ae;
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
        catch (Exception ae)
        {
            throw ae;
        }
    }

    #endregion



}
