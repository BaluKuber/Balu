#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   System Admin
/// Screen Name         :   S3GSysAdminUTPAMaster_View
/// Created By          :   Suresh P
/// Created Date        :   07-MAY-2010
/// Purpose             :   To view UTPA master details
/// Last Updated By		:   Suresh P
/// Last Updated Date   :   13-MAY-2010
/// Reason              :   To add Grid Search / Paging Functionality  
/// <Program Summary>
#endregion

#region Namespaces
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
#endregion

public partial class S3GSysAdminUTPAMaster_View : ApplyThemeForProject
{
    #region Intialization
    string strRedirectPageAdd = "~/System Admin/S3GSysAdminUTPAMaster_Add.aspx";
    SerializationMode SerMode;
    #endregion
    #region Paging Config
    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
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
    #endregion

    //string strSearchColName;
    //PagedDataSource pdsPagerDataSource;
    //DataView dvSearchView;
    //private Int32 intCount;
    //string strSortColName;
    //int intCompany_ID = 0;

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErrorMessage.InnerText = "";
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
                grvLOBMaster.Columns[1].Visible = false;
                grvLOBMaster.Columns[0].Visible = false;
                btnCreate.Enabled = false;
            }
            else
            {
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
    }
    #endregion

    #region Page Methods
    /// <summary>
    /// This method is used to display UTPA details
    /// </summary>
    private void FunPriBindGrid()
    {

        //TPAMgtServicesReference.TPAMgtServicesClient ObjUTPAMasterClient;
        //ObjUTPAMasterClient = new TPAMgtServicesReference.TPAMgtServicesClient();

        TPAMgtServicesReference.TPAMgtServicesClient ObjUTPAMasterClient = new TPAMgtServicesReference.TPAMgtServicesClient();

        try
        {
            int intTotalRecords = 0;

            TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable ObjS3G_UTPADataTable = new TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable();
            TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjLOBMasterRow = ObjS3G_UTPADataTable.NewS3G_SYSAD_UTPAMasterRow();
            ObjLOBMasterRow.Company_ID = intCompanyID;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            ObjS3G_UTPADataTable.AddS3G_SYSAD_UTPAMasterRow(ObjLOBMasterRow);

            SerMode = SerializationMode.Binary;

            byte[] byteLOBDetails = ObjUTPAMasterClient.FunPubQueryUTPAPaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_UTPADataTable, SerMode), ObjPaging);

            TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable dtLOB = (TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, SerializationMode.Binary, typeof(TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable));
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
            ObjUTPAMasterClient.Close();
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
            strSearchVal2 = ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
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
            ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
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
            ((TextBox)grvLOBMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
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
                strSearchVal += "UTPA_Code like '%" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and UTPA_Name like '%" + strSearchVal2 + "%'";
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
        string strSortExpression = string.Empty;    //// By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;
        strSortDirection = "DESC";

        try
        {
            // Retrieve the last column that was sorted.
            strSortExpression = hdnSortExpression.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
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
                    strSortColName = "UTPA_Code";
                    break;
                case "lnkbtnSortName":
                    strSortColName = "UTPA_Name";
                    break;
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
    #endregion

    #region Page Events
    /// <summary>
    /// Create
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminUTPAMaster_Add.aspx");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <summary>
    /// GridView Row DataBound Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvLOBMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }

            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }
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
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strUTPA_ID"></param>
    /// <param name="strRegion_ID"></param>
    /// <param name="strBranch_ID"></param>
    /// <returns></returns>
    //protected string RedirectUrl(string strUTPA_ID, string strRegion_ID, string strBranch_ID)
    //{
    //    return "~/System Admin/S3GSysAdminUTPAMaster_Add.aspx?qsUTPAID=" + strUTPA_ID + "";
    //}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strId"></param>
    /// <returns></returns>
    //public string FunPriGetURL(string strId)
    //{
    //    return (strRedirectPage + "?qsUTPAID=" + strId + "&mode=Q");
    //}
    #endregion
    protected void grvLOBMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsUTPAID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsUTPAID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;

        }

    }
}
