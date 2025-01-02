/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminCustomerAlert_View
/// Created By      :   Ramesh M
/// Created Date    :   24-May-2010
/// Purpose         :   To View CustomerAlert details
#region Namespaces
using System;
using System.Web.Security;
using System.Configuration;
using System.Data;
using System.ServiceModel;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Collections.Generic;
#endregion

public partial class System_Admin_S3GSysAdminCustomerAlert_View : ApplyThemeForProject
{

    string strRedirectPage = "~/System Admin/S3GSysAdminCustomerAlert_Add.aspx";
    CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable ObjCustomerAlert_DataTable = new CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable();
    CompanyMgtServicesReference.CompanyMgtServicesClient ObjCustomerAlertClient;

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    string strSearchVal6 = string.Empty;
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
    /// <summary>
    ///Assign Page Number
    /// </summary
    public int ProPageNumRW
    {
        get;
        set;
    }
    /// <summary>
    ///Assign Page Saie
    /// </summary
    public int ProPageSizeRW
    {
        get;
        set;

    }
    /// <summary>
    ///Assign Page value
    /// </summary
    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;
            ProPageSizeRW = intPageSize;
            FunPriBindGrid();
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    #region PageLoad
    /// <summary>
    ///Page Load Events
    /// </summary
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
            bDelete = ObjUserInfo.ProDeleteRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            if (!IsPostBack)
            {

                FunPriBindGrid();
                if (!bIsActive)
                {
                    grvCustomerAlert.Columns[1].Visible = false;
                    grvCustomerAlert.Columns[0].Visible = false;
                    btnCreate.Enabled = false;
                    return;
                }

                if (!bModify)
                {
                    grvCustomerAlert.Columns[1].Visible = false;
                }
                if (!bQuery)
                {
                    grvCustomerAlert.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                }

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    /// <summary>
    ///Bind Customer Alert Details
    /// </summary
    private void FunPriBindGrid()
    {
        ObjCustomerAlertClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            CompanyMgtServices.S3G_SYSAD_CustomerAlertRow ObjCustomerAlertRow;
            ObjCustomerAlertRow = ObjCustomerAlert_DataTable.NewS3G_SYSAD_CustomerAlertRow();
            ObjCustomerAlert_DataTable.AddS3G_SYSAD_CustomerAlertRow(ObjCustomerAlertRow);
            SerializationMode SerMode = SerializationMode.Binary;

            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            //byte[] byteCustomerAlertDetails = ObjCustomerAlertClient.FunPubQueryCustomerAlertPaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjCustomerAlert_DataTable, SerMode), ObjPaging);
            //ObjCustomerAlert_DataTable = (CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable)ClsPubSerialize.DeSerialize(byteCustomerAlertDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable));
            //DataView dvCustomrAlert = ObjCustomerAlert_DataTable.DefaultView;
            FunPriGetSearchValue();

            //This is to show grid header
            bool bIsNewRow = false;
            //if (dvCustomrAlert.Count == 0)
            //{
            //    dvCustomrAlert.AddNew();
            //    bIsNewRow = true;
            //}

            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            grvCustomerAlert.BindGridView("S3G_Get_CustomerAlert_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);


            //grvCustomerAlert.DataSource = dvCustomrAlert;
            //grvCustomerAlert.DataBind();
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvCustomerAlert.Rows[0].Visible = false;
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjCustomerAlertClient.Close();
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
            if (grvCustomerAlert.HeaderRow != null)
            {
                ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderEntity_Type")).Text = "";
                //((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderEventType")).Text = "";
                ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderRoleCode")).Text = "";
                ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderLOBCode")).Text = "";
                //((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderTargetEmail")).Text = "";
                //((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderTargetSMS")).Text = "";
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        try
        {
            if (grvCustomerAlert.HeaderRow != null)
            {
                ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderEntity_Type")).Text = strSearchVal1;
                //((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderEventType")).Text = strSearchVal2;
                ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderRoleCode")).Text = strSearchVal3;
                ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderLOBCode")).Text = strSearchVal4;
                //((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderTargetEmail")).Text = strSearchVal5;
                //((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderTargetSMS")).Text = strSearchVal6;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
                strSearchVal += " EnT.Entity_Type_Name like '%" + strSearchVal1 + "%'";
            }
            //if (strSearchVal2 != "")
            //{
            //    strSearchVal += " and EvT.Event_Type_Name like '%" + strSearchVal2 + "%'";
            //}
            if (strSearchVal3 != "")
            {
                //strSearchVal += " and RC.Role_Code + ' - ' + Program_Name like '%" + strSearchVal3 + "%'";
                strSearchVal += " and Program_Name + ' - ' + RC.Role_Code like '%" + strSearchVal3 + "%'";
            }
            if (strSearchVal4 != "")
            {
                strSearchVal += " and LOB.LOB_Name like '%" + strSearchVal4 + "%'";
            }
            if (strSearchVal5 != "")
            {
                strSearchVal += " and EnT3.Entity_Type_Name like '" + strSearchVal5 + "%'";
            }
            if (strSearchVal6 != "")
            {
                strSearchVal += " and EnT2.Entity_Type_Name like '" + strSearchVal6 + "%'";
            }


            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvCustomerAlert.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
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
            lblErrorMessage.InnerText = ex.Message;
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
                case "lnkbtnEntityType":
                    strSortColName = "EnT.Entity_Type_Name";
                    break;
                //case "lnkbtnEventType":
                //    strSortColName = "EvT.Event_Type_Name";
                //    break;
                case "lnkbtnRoleCode":
                    strSortColName = "RC.Role_Code";
                    break;
                case "lnkbtnLOBCode":
                    strSortColName = "LOB.LOB_Name";
                    break;
                //case "lnkbtnTargetEmail":
                //    strSortColName = "EnT3.Entity_Type_Name";
                //    break;
                //case "lnkbtnTargetSMS":
                //    strSortColName = "EnT2.Entity_Type_Name";
                //    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvCustomerAlert.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvCustomerAlert.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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

    protected void grvCustomerAlert_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (grvCustomerAlert.Rows.Count > 0)
            {
                grvCustomerAlert.UseAccessibleHeader = true;
                grvCustomerAlert.HeaderRow.TableSection = TableRowSection.TableHeader;
                grvCustomerAlert.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvCustomerAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if(e.Row.DataItem(""))
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblActive = (Label)e.Row.FindControl("lblActive");
                CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
                Label lblEmail = (Label)e.Row.FindControl("lblEmail");
                CheckBox chkEmail = (CheckBox)e.Row.FindControl("chkEmail");
                Label lblSMS = (Label)e.Row.FindControl("lblSMS");
                CheckBox chkSMS = (CheckBox)e.Row.FindControl("chkSMS");

                if (lblActive.Text == "True")
                {
                    chkAct.Checked = true;
                }
                if (lblEmail.Text == "True")
                {
                    chkEmail.Checked = true;
                }
                if (lblSMS.Text == "True")
                {
                    chkSMS.Checked = true;
                }

                //grvCustomerAlert.RowHeaderColumn.
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
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvCustomerAlert_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            switch (e.CommandName.ToLower())
            {
                case "modify":
                    Response.Redirect("~/System Admin/S3GSysAdminCustomerAlert_Add.aspx?qsCAId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                    break;
                case "query":
                    Response.Redirect("~/System Admin/S3GSysAdminCustomerAlert_Add.aspx?qsCAId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                    break;

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
            if (grvCustomerAlert.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderEntity_Type")).Text.Trim();
                //strSearchVal2 = ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderEventType")).Text.Trim();
                strSearchVal3 = ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderRoleCode")).Text.Trim();
                strSearchVal4 = ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderLOBCode")).Text.Trim();
                //strSearchVal5 = ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderTargetEmail")).Text.Trim();
                //strSearchVal6 = ((TextBox)grvCustomerAlert.HeaderRow.FindControl("txtHeaderTargetSMS")).Text.Trim();
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public string FunPriGetURL(string strId)
    {
        return (strRedirectPage + "?CAID=" + strId + "&mode=Q");
    }

    #endregion
}

