#region Namespaces
using System;
using System.Web.Security;
using System.Data;
using System.ServiceModel;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Configuration;
using System.Collections.Generic;

#endregion

public partial class Origination_S3G_Org_HSN_View : ApplyThemeForProject
{
    #region Intialization
    string strRedirectPage = "~/Origination/S3G_Org_HSN_Add.aspx";
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    
    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    bool bModify = false;
    bool bQuery = false;
    bool bCreate = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;
    PagingValues ObjPaging = new PagingValues();

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    /// <summary>
    ///Assign Page Number
    /// </summary
    public int ProPageNumRW
    {
        get;
        set;
    }
    /// <summary>
    ///Assign Page Size
    /// </summary
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
    /// <summary>
    ///Page Load Events
    /// </summary
    protected void Page_Load(object sender, EventArgs e)
    
    {
        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
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
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        if (!IsPostBack)
        {
            FunPriBindGrid();
            if (!bIsActive)
            {
                grvHSNMaster.Columns[1].Visible = false;
                grvHSNMaster.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }

            if (!bModify)
            {
                grvHSNMaster.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvHSNMaster.Columns[0].Visible = false;
            }
            if (!bCreate)
            {
                btnCreate.Enabled = false;
            }




        }
    }
    #endregion

    #region Page Load Methods
    /// <summary>
    ///Bind HSN Details
    /// </summary
    private void FunPriBindGrid()
    {
        try
        {
            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            bool blnIsNewRow = false;
            FunPriGetSearchValue();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
              grvHSNMaster.BindGridView("S3G_Get_HSN_Paging", Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
               
           


            lblErrorMessage.InnerText = string.Empty;
            //This is to show grid header

            //This is to hide first row if grid is empty
            if (blnIsNewRow)
            {
                if (grvHSNMaster.Rows.Count >= 1)
                {
                    grvHSNMaster.Rows[0].Visible = false;
                }
               
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);


            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.InnerText = ex.Message;
        }

    }

    /// <summary>
    /// This method is used to display HSN details
    /// </summary>
    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>



    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (grvHSNMaster.HeaderRow != null)
        {
            ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderHSNCode")).Text = "";
            ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderHSNName")).Text = "";
            //((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderLOBName")).Text = "";
            //((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            //((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvHSNMaster.HeaderRow != null)
        {
            ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderHSNCode")).Text = strSearchVal1;
            ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderHSNName")).Text = strSearchVal2;
            //  ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderLOBName")).Text = strSearchVal3;
            //((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            //((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
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
                strSearchVal += " HSN_Code like '%" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and HSN_Desc like '%" + strSearchVal2 + "%'";
            }
           
            //if (strSearchVal4 != "")
            //{
            //    strSearchVal += " and LOB_Name like '" + strSearchVal4 + "%'";
            //}
            //if (strSearchVal5 != "")
            //{
            //    strSearchVal += " and LOB_Name like '" + strSearchVal5 + "%'";
            //}

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }
            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvHSNMaster.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

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
                case "lnkbtnHSNCode":
                    strSortColName = "HSN_Code";
                    break;
                case "lnkbtnHSNName":
                    strSortColName = "HSN_Desc";
                    break;
              
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvHSNMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvHSNMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
       
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion


    #endregion

    #region Page Events

    protected void grvHSNMaster_DataBound(object sender, EventArgs e)
    {
        if (grvHSNMaster.Rows.Count > 0)
        {
            grvHSNMaster.UseAccessibleHeader = true;
            grvHSNMaster.HeaderRow.TableSection = TableRowSection.TableHeader;
            grvHSNMaster.FooterRow.TableSection = TableRowSection.TableFooter;
        }

    }
    protected void grvHSNMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if(e.Row.DataItem(""))
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }

            //grvHSNMaster.RowHeaderColumn.

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
            //Authorization code end   


        }

    }
    protected void grvHSNMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect("~/Origination/S3G_Org_HSN_Add.aspx?qsPId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M", false);
                break;
            case "query":
                Response.Redirect("~/Origination/S3G_Org_HSN_Add.aspx?qsPId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q", false);
                break;

        }
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

    protected void btnShow_Click(object sender, EventArgs e)
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
            lblErrorMessage.InnerText = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion
    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        if (grvHSNMaster.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderHSNCode")).Text.Trim();
            strSearchVal2 = ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderHSNName")).Text.Trim();
            //strSearchVal3 = ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderLOBName")).Text.Trim();
            //strSearchVal4 = ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
            //strSearchVal5 = ((TextBox)grvHSNMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
        }
    }

    public string FunPriGetURL(string strId)
    {
        return (strRedirectPage + "?pid=" + strId + "&mode=Q");
    }

    #endregion

}