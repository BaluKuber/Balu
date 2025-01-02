#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Doc Path Details
/// Created By			: Nataraj Y
/// Created Date		: 28-May-2010
/// Purpose	            : Development
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 29-Jun-2010
/// Reason              : Bug Fixing
/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;

public partial class System_Admin_S3GSysAdminDocPath_View : ApplyThemeForProject
{
    #region Intialization
    string strRedirectPageAdd = "~/System Admin/S3GSysAdminDocPath_Add.aspx";
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
    bool bCreate = false;
    bool bQuery = false;
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
        try
        {
            ProPageNumRW = intPageNum;
            ProPageSizeRW = intPageSize;
            FunPriBindGrid();
        }
        catch (Exception e)
        {
            lblErrorMessage.InnerText = e.Message;
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
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            #endregion

            if (!IsPostBack)
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                FunPriBindGrid();
                if (!bIsActive)// || ObjUserInfo.ProUserLevelIdRW != 5)
                {
                    //grvDocPathMaster.Columns[1].Visible = false;
                    //grvDocPathMaster.Columns[0].Visible = false;
                    btnCreate.Enabled = false;
                    return;
                }

                if (!bModify)
                {
                    grvDocPathMaster.Columns[1].Visible = false;
                }
                if (!bQuery)
                {
                    grvDocPathMaster.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                }
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }
    #endregion


    #region Page Methods

    /// <summary>
    /// This method is used to display Company details
    /// </summary>
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

            //DataTable dtDocPath;
            //objDocMgtServicesClient = new DocMgtServicesReference.DocMgtServicesClient();
            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] byteDocPathDetails = objDocMgtServicesClient.FunPubQueryDocPathPaging(out  intTotalRecords, ObjPaging);
            //dtDocPath = (DataTable)ClsPubSerialize.DeSerialize(byteDocPathDetails, SerMode, typeof(DataTable));
            //DataView dvDocPath = dtDocPath.DefaultView;

            //Paging Config

            FunPriGetSearchValue();
            lblErrorMessage.InnerText = string.Empty;
            //This is to show grid header
            bool bIsNewRow = false;
            //if (dvDocPath.Count == 0)
            //{
            //    dvDocPath.AddNew();
            //    dvDocPath[0]["is_active"] = false;
            //    bIsNewRow = true;

            //}

            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            grvDocPathMaster.BindGridView("S3G_Get_DocumentationPath_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //grvDocPathMaster.DataSource = dvDocPath;
            //grvDocPathMaster.DataBind();like %'


            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvDocPathMaster.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);

            //if (intTotalRecords < ProPageSizeRW)
            //    ucCustomPaging.setPageSize(intTotalRecords);
            //else
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
    }

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        try
        {
            if (grvDocPathMaster.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
                strSearchVal2 = ((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
                //strSearchVal3 = ((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();

            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        try
        {
            if (grvDocPathMaster.HeaderRow != null)
            {
                ((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
                ((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
                //((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";

            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        try
        {
            if (grvDocPathMaster.HeaderRow != null)
            {
                ((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
                ((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
                //((TextBox)grvDocPathMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
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
            if (strSearchVal1 != "")
            {
                strSearchVal += grvDocPathMaster.Columns[2].SortExpression + " like '%" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and " + grvDocPathMaster.Columns[3].SortExpression + " like '%" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and " + grvDocPathMaster.Columns[4].SortExpression + " like '%" + strSearchVal3 + "%'";
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }



            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvDocPathMaster.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = "No Records Found";
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
            lblErrorMessage.InnerText = ex.Message;
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
                case "lnkbtnSortLOB":
                    strSortColName = "LOB_Name";
                    break;
                case "lnkbtnSortRole":
                    strSortColName = "Role_Code";
                    break;
                case "lnkbtnSortFlagDesc":
                    strSortColName = "PM.Document_Flag";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();
            if (strDirection == "ASC")
            {
                ((ImageButton)grvDocPathMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvDocPathMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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
            Response.Redirect(strRedirectPageAdd);
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
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
            ProPageSizeRW = 10;
            FunPriClearSearchValue();
            FunPriBindGrid();
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvDocPathMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            switch (e.CommandName.ToLower())
            {
                case "modify":
                    Response.Redirect(strRedirectPageAdd + "?qsDocPathId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                    break;
                case "query":
                    Response.Redirect(strRedirectPageAdd + "?qsDocPathId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                    break;
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }
    protected void grvDocPathMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }
    #endregion
}
