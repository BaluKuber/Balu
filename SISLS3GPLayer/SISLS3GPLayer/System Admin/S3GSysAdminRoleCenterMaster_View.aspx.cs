/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminRoleCenterMaster_View
/// Created By      :   Ramesh M
/// Created Date    :   20-May-2010
/// Purpose         :   To view product master details
#region Namespaces
using System;
using System.Web.Security;
using System.Data;
using System.ServiceModel;
using System.Web.UI.WebControls;
using System.Configuration;
using S3GBusEntity;
#endregion
public partial class System_Admin_S3GSysAdminRoleCenterMaster_View : ApplyThemeForProject
{
    #region Initialization
    UserMgtServicesReference.UserMgtServicesClient ObjRoleCenterMasterClient;
    string strRedirectPage = "~/System Admin/S3GSysAdminRoleCenterMaster_Add.aspx";
    UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable ObjS3G_RoleCodeMasterDataTable = new UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable();
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;
    #region PageLoad
    /// <summary>
    ///Page Load Event
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
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
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            if (!IsPostBack)
            {
                FunPriBindGrid();

                if (!bIsActive)
                {
                    grvRoleCenterMaster.Columns[1].Visible = false;
                    grvRoleCenterMaster.Columns[0].Visible = false;
                    btnCreate.Enabled = false;
                    return;
                }
                if (!bModify)
                {
                    grvRoleCenterMaster.Columns[1].Visible = false;
                }

                if (!bQuery)
                {
                    grvRoleCenterMaster.Columns[0].Visible = false;
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
    /// <summary>
    ///Bind Role Code Details
    /// </summary>
    private void FunPriBindGrid()
    {
        ObjRoleCenterMasterClient = new UserMgtServicesReference.UserMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            UserMgtServices.S3G_SYSAD_RoleCodeMasterRow ObjRoleCodeMasterRow;
            ObjRoleCodeMasterRow = ObjS3G_RoleCodeMasterDataTable.NewS3G_SYSAD_RoleCodeMasterRow();
            ObjS3G_RoleCodeMasterDataTable.AddS3G_SYSAD_RoleCodeMasterRow(ObjRoleCodeMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            byte[] byteRolCenterMasterDetails = ObjRoleCenterMasterClient.FunPubQueryRoleCodeMasterPaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_RoleCodeMasterDataTable, SerMode), ObjPaging);
            UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable dtRoleCode = (UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(byteRolCenterMasterDetails, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
            DataView dvRoleCenter = dtRoleCode.DefaultView;
            //Paging Config

            FunPriGetSearchValue();

            //This is to show grid header
            bool bIsNewRow = false;
            if (dvRoleCenter.Count == 0)
            {
                dvRoleCenter.AddNew();
                bIsNewRow = true;
            }

            grvRoleCenterMaster.DataSource = dvRoleCenter;
            grvRoleCenterMaster.DataBind();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvRoleCenterMaster.Rows[0].Visible = false;
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
        finally
        {
            ObjRoleCenterMasterClient.Close();
        }
    }
    /// <summary>
    /// This method is used to display product details
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
        try
        {
            if (grvRoleCenterMaster.HeaderRow != null)
            {
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderRoleCenterCode")).Text = "";
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderRoleCode")).Text = "";
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderModuleCode")).Text = "";
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderProgramCode")).Text = "";
                //((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
                //((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
            }
        }
        catch (Exception e)
        {
            lblErrorMessage.InnerText = e.Message;
            throw e;
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        try
        {
            if (grvRoleCenterMaster.HeaderRow != null)
            {
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderRoleCenterCode")).Text = strSearchVal1;
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderRoleCode")).Text = strSearchVal2;
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderModuleCode")).Text = strSearchVal3;
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderProgramCode")).Text = strSearchVal4;
                //((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
                //((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
            }
        }
        catch (Exception e)
        {
            lblErrorMessage.InnerText = e.Message;
            throw e;
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
                strSearchVal += "Role_Center_Name like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and Role_Code like '" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and Module_Name like '" + strSearchVal3 + "%'";
            }
            if (strSearchVal4 != "")
            {
                strSearchVal += " and Program_Name like '" + strSearchVal4 + "%'";
            }
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
                ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            throw ex;
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
            throw ex;
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
                case "lnkbtnRoleCenterCode":
                    strSortColName = "Role_Center_Name";
                    break;
                case "lnkbtnRoleCode":
                    strSortColName = "Role_Code";
                    break;
                case "lnkbtnModuleCode":
                    strSortColName = "Module_Name";
                    break;
                case "lnkbtnProgramCode":
                    strSortColName = "Program_Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvRoleCenterMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvRoleCenterMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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
    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    PagingValues ObjPaging = new PagingValues();

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    /// <summary>
    ///Assign Page Number
    /// </summary>
    public int ProPageNumRW
    {
        get;
        set;
    }
    /// <summary>
    ///Assign Page Size
    /// </summary>
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

    #endregion

    #region Page Events

    protected void grvRoleCenterMaster_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (grvRoleCenterMaster.Rows.Count > 0)
            {
                grvRoleCenterMaster.UseAccessibleHeader = true;
                grvRoleCenterMaster.HeaderRow.TableSection = TableRowSection.TableHeader;
                grvRoleCenterMaster.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }
    protected void grvRoleCenterMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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

                //grvRoleCenterMaster.RowHeaderColumn.

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
            throw ae;
            lblErrorMessage.InnerText = ae.Message;
        }
    }
    protected void grvRoleCenterMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            switch (e.CommandName.ToLower())
            {
                case "modify":
                    Response.Redirect("~/System Admin/S3GSysAdminRoleCenterMaster_Add.aspx?qsRCId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                    break;
                case "query":
                    Response.Redirect("~/System Admin/S3GSysAdminRoleCenterMaster_Add.aspx?qsRCId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                    break;
            }
        }
        catch (Exception ae)
        {
            throw ae;
            lblErrorMessage.InnerText = ae.Message;
        }
    }
    /// <summary>
    ///Navigation Create Mode
    /// </summary>
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
    /// <summary>
    ///Show All Role Code Details
    /// </summary>
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

    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        try
        {
            if (grvRoleCenterMaster.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderRoleCenterCode")).Text.Trim();
                strSearchVal2 = ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderRoleCode")).Text.Trim();
                strSearchVal3 = ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderModuleCode")).Text.Trim();
                strSearchVal4 = ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderProgramCode")).Text.Trim();
                //strSearchVal4 = ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
                //strSearchVal5 = ((TextBox)grvRoleCenterMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public string FunPriGetURL(string strId)
    {
        try
        {
            return (strRedirectPage + "?RCid=" + strId + "&mode=Q");
        }
        catch (Exception ae)
        {
            throw ae;
            lblErrorMessage.InnerText = ae.Message;
        }
    }

    #endregion


}