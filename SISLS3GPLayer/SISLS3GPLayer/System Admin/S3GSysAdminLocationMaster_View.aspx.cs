#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin  
/// Screen Name			: Location Master View
/// Created By			: Tamilselvan.S
/// Created Date		: 04 May 2011
/// Modified            : Tamilselvan.S on 15/12/2011 for UAT Malolan Observation Fixing
#endregion

#region [Namespace]
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
using S3GBusEntity;
using System.Collections.Generic;
using System.IO;
#endregion [Namespace]

public partial class System_Admin_S3GSysAdminLocationMaster_View : ApplyThemeForProject
{
    #region Intialization
    string strRedirectPageAdd = "~/System Admin/S3GSysAdminLocationMaster_Add.aspx";
    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    //User Authorization variable declaration
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();

    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    string strTabType = "Cate";
    DataTable dtHierarchy = new DataTable();
    string strErrorMessage = @"Correct the following validation(s):<ul><li> ";
    string strErrorMsgLast = @"</ul></li>";

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
        if (tcLocation.ActiveTabIndex == 0)
        {
            //Added by Tamilselvan.S on 13/09/2011 for Fitching 
            AjaxControlToolkit.TabPanel tpCategory = tcLocation.Tabs[0];
            AjaxControlToolkit.TabContainer tcCategory = (AjaxControlToolkit.TabContainer)tpCategory.FindControl("tcLocationDef");
            FunPriBindGrid(tcLocation.ActiveTab.HeaderText.ToUpper(), tcCategory.ActiveTab.HeaderText);
        }
        else
        {
            FunPriBindGrid("", "");
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //User Authorization
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
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
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            #endregion
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
                    strTabType = Convert.ToString(Request.QueryString["Type"]);
                if (strTabType == "Cate")
                {
                    tcLocation.ActiveTabIndex = 0;
                    btnCreate.Visible = true;
                }
                else
                {
                    tcLocation.ActiveTabIndex = 1;
                    btnCreate.Visible = false;
                }
                //User Authorization
                if (!bIsActive)
                {
                    gvLocationDef.Columns[1].Visible = false;
                    gvLocationDef.Columns[0].Visible = false;
                    btnCreate.Enabled = false;
                    return;
                }

                if (!bModify)
                {
                    gvLocationDef.Columns[1].Visible = false;
                }
                if (!bQuery)
                {
                    gvLocationDef.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                }
                //Authorization Code end
            }
            FunPubLoadActiveHierarchy();
            if (!IsPostBack)
            {
                AjaxControlToolkit.TabPanel tpLocDefTab = (AjaxControlToolkit.TabPanel)tcLocation.FindControl("tpLocationDef");
                AjaxControlToolkit.TabContainer tc = (AjaxControlToolkit.TabContainer)tpLocDefTab.FindControl("tcLocationDef");
                string strType = "";
                if (!string.IsNullOrEmpty(Request.QueryString["Level"]))
                {
                    strType = HttpUtility.UrlDecode(Request.QueryString["Level"].ToString());
                    //foreach (AjaxControlToolkit.TabPanel tp in tc.Tabs)
                    //{
                    //    if (tp.HeaderText == strType)
                    //        break;
                    //    intActiveTab++;
                    //}
                    
                }
                int intActiveTab = 0;
                if (Request.QueryString["Hir"] != null)
                {
                    intActiveTab = Convert.ToInt32(Request.QueryString["Hir"].ToString());
                }
                tcLocationDef.ActiveTabIndex = intActiveTab;
                FunProCateTabChanged(tcLocationDef);

                //if(dtHierarchy.Rows.Count > 0)
                //FunPriBindGrid(tcLocation.ActiveTab.HeaderText.ToUpper(), strType);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvLocation.IsValid = false;
            cvLocation.ErrorMessage = strErrorMessage + "Unable to load the location master" + strErrorMsgLast;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    #region [Button Event's]

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (hdnHierarc.Value == "0")
        {
            Utility.FunShowAlertMsg(this, "Hierarchy Master should be added.");
            return;
        }
        if (tcLocation.ActiveTabIndex == 0)  // Create mode by location Definition
        {
            Response.Redirect("~/System Admin/S3GSysAdminLocationMaster_Add.aspx?Type=Cate&qsMode=C&Hir=" + tcLocationDef.ActiveTabIndex.ToString());
        }
        else     // Create mode by Location Mapping
        {
            Response.Redirect("~/System Admin/S3GSysAdminLocationMaster_Add.aspx?Type=Map&qsMode=C");
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
            ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            AjaxControlToolkit.TabContainer tcLocation = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocation");
            AjaxControlToolkit.TabPanel tpLocDefTab = (AjaxControlToolkit.TabPanel)tcLocation.FindControl("tpLocationDef");
            AjaxControlToolkit.TabContainer tcLocDef = (AjaxControlToolkit.TabContainer)tpLocDefTab.FindControl("tcLocationDef");
            tcLocDef.ActiveTabIndex = 0;
            if(hdnHierarc.Value!="0")
            FunPriBindGrid(tcLocation.ActiveTab.HeaderText.ToUpper(), "");
        }
        catch (Exception ex)
        {
            cvLocation.IsValid = false;
            cvLocation.ErrorMessage = strErrorMessage + "Unable to show all the records" + strErrorMsgLast;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected string FuncProConactFiledsStr(string strField1, string strField2)
    {
        return strField1 + "," + strField2;
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
            if (tcLocation.ActiveTabIndex == 0)
            {
                switch (lnkbtnSearch.ID)
                {
                    case "lnkbtnSort1":
                        strSortColName = "Location_Code";
                        break;
                    case "lnkbtnSort2":
                        strSortColName = "LocationCat_Description";
                        break;
                    case "lnkbtnGSTState":
                        strSortColName = "GST_State_Code";
                        break;
                }
                string strDirection = FunPriGetSortDirectionStr(strSortColName);
                FunPriBindGrid("", FunPubGetSelectedHierarchyType());
                if (strDirection == "ASC")
                    ((ImageButton)gvLocationDef.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
                else
                    ((ImageButton)gvLocationDef.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
            else
            {
                switch (lnkbtnSearch.ID)
                {
                    case "lnkbtnSort1":
                        strSortColName = "LM.Location_Code";
                        break;
                    case "lnkbtnSort2":
                        strSortColName = "Location";
                        break;
                }
                string strDirection = FunPriGetSortDirectionStr(strSortColName);
                FunPriBindGrid("", "");
                if (strDirection == "ASC")
                    ((ImageButton)gvLocationMapping.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
                else
                    ((ImageButton)gvLocationMapping.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvLocation.IsValid = false;
            cvLocation.ErrorMessage = strErrorMessage + ex.Message + strErrorMsgLast;
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
            if (tcLocation.ActiveTabIndex == 0)
            {
                if (strSearchVal1 != "")
                {
                    strSearchVal += gvLocationDef.Columns[2].SortExpression + " like '%" + strSearchVal1 + "%'";
                }
                if (strSearchVal2 != "")
                {
                    strSearchVal += " and " + gvLocationDef.Columns[4].SortExpression + " like '%" + strSearchVal2 + "%'";
                }
                if (strSearchVal3 != "")
                {
                    strSearchVal += " and " + gvLocationDef.Columns[3].SortExpression + " like '%" + strSearchVal3 + "%'";
                }
            }
            else
            {
                if (strSearchVal1 != "")
                {
                    strSearchVal += gvLocationMapping.Columns[2].SortExpression.Replace("Location_Code", "REPLACE(LM.Location_Code,'|','')") + " like '%" + strSearchVal1 + "%'";
                }
                if (strSearchVal2 != "")
                {
                    strSearchVal += " and " + gvLocationMapping.Columns[3].SortExpression.Replace("Location", "Location") + " like '%" + strSearchVal2 + "%'";
                }
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }
            hdnSearch.Value = strSearchVal;
            if (tcLocation.ActiveTabIndex == 0)
            {
                FunPriBindGrid("", FunPubGetSelectedHierarchyType());
            }
            else
                FunPriBindGrid("", "");
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
            {
                txtboxSearch.Focus();
         //       ((TextBox)gvLocationDef.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvLocation.IsValid = false;
            cvLocation.ErrorMessage = strErrorMessage + ex.Message + strErrorMsgLast;
        }
    }

    #endregion [Button Event's]

    #region [Grid View Events]

    protected void gvLocationDef_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsLocCatId=" + FormsAuthentication.Encrypt(Ticket) + "&qsType=" + FunPubGetSelectedHierarchyType() + "&qsMode=M&Type=Cate&Hir=" + tcLocationDef.ActiveTabIndex.ToString());
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsLocCatId=" + FormsAuthentication.Encrypt(Ticket) + "&qsType=" + FunPubGetSelectedHierarchyType() + "&qsMode=Q&Type=Cate&Hir=" + tcLocationDef.ActiveTabIndex.ToString());
                break;
        }
    }

    /// <summary>
    /// GridView Row DataBound Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLocationDef_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gvLocationMapping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsLocMapId=" + FormsAuthentication.Encrypt(Ticket) + "&qsType=" + "" + "&qsMode=M&Type=Map");
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsLocMapId=" + FormsAuthentication.Encrypt(Ticket) + "&qsType=" + "" + "&qsMode=Q&Type=Map");
                break;
        }
    }

    /// <summary>
    /// GridView Row DataBound Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLocationMapping_RowDataBound(object sender, GridViewRowEventArgs e)
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

    #endregion [Grid View Events]

    #region [Function's]

    #region "Page Values Initialization "

    private void SetPageMode(string queryMode)
    {
        switch (queryMode.ToLower())
        {
            case "c":
                PageMode = PageModes.Create;
                break;
            case "q":
                PageMode = PageModes.Query;
                break;
            case "m":
                PageMode = PageModes.Modify;
                break;
            case "d":
                PageMode = PageModes.Delete;
                break;
            case "w":
                PageMode = PageModes.WorkFlow;
                break;
            default:
                break;
        }
    }
    #endregion

    public string FunPubGetSelectedHierarchyType()
    {
        ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
        AjaxControlToolkit.TabContainer tcLocation = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocation");
        AjaxControlToolkit.TabPanel tpLocDefTab = (AjaxControlToolkit.TabPanel)tcLocation.FindControl("tpLocationDef");
        AjaxControlToolkit.TabContainer tcLocDef = (AjaxControlToolkit.TabContainer)tpLocDefTab.FindControl("tcLocationDef");
        return tcLocDef.ActiveTab.HeaderText;
    }

    public void FunPubLoadActiveHierarchy()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", "1");
            dictParam.Add("@IsActive", "1");
            dtHierarchy = Utility.GetDefaultData("S3G_SYSAD_GetHierachyMasterDetails", dictParam);
            if (dtHierarchy.Rows.Count == 0)
            {
                ucCustomPaging.Visible = gvLocationDef.Visible = false;
                hdnHierarc.Value = "0";
                //Utility.FunShowAlertMsg(this, "Hierarchy Master should be added.");
                //btnCreate.Enabled = tcLocation.Tabs[1].Enabled = false;
            }
            else
            {
                //btnCreate.Enabled = true;
                ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
                AjaxControlToolkit.TabContainer tcLocation = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocation");
                AjaxControlToolkit.TabPanel tpLocDefTab = (AjaxControlToolkit.TabPanel)tcLocation.FindControl("tpLocationDef");
                AjaxControlToolkit.TabContainer tcLocDef = (AjaxControlToolkit.TabContainer)tpLocDefTab.FindControl("tcLocationDef");
                AjaxControlToolkit.TabPanel tpLocDef = new AjaxControlToolkit.TabPanel();
                int intCount = 0;
                foreach (DataRow drow in dtHierarchy.Rows)
                {
                    tpLocDef = new AjaxControlToolkit.TabPanel();
                    tpLocDef.ID = "tpLocDef" + Convert.ToString(drow["Location_Description"]);
                    tpLocDef.ToolTip = Convert.ToString(drow["Location_Description"]);
                    tpLocDef.HeaderText = Convert.ToString(drow["Location_Description"]);
                    tcLocDef.Tabs.Add(tpLocDef);
                    intCount++;
                }
                tcLocation.Tabs[1].Enabled = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    protected void tcLocation_ActiveTabChanged(object sender, EventArgs e)
    {
        try
        {
            strSearchVal1 = strSearchVal2 = "";
            hdnSearch.Value = hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            string strHier = string.Empty;
            btnCreate.Visible = true;
            if (tcLocation.ActiveTabIndex == 0)//Added by Tamilselvan.S on 13/09/2011 for Fitching 
            {
                AjaxControlToolkit.TabPanel tpCategory = tcLocation.Tabs[0];
                AjaxControlToolkit.TabContainer tcCategory = (AjaxControlToolkit.TabContainer)tpCategory.FindControl("tcLocationDef");
                strHier = tcCategory.ActiveTab.HeaderText.ToString();
            }
            else
                btnCreate.Visible = false;
            FunPriBindGrid(tcLocation.ActiveTab.HeaderText.ToUpper(), strHier);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void tcLocationDef_ActiveTabChanged(object sender, EventArgs e)
    {
        FunProCateTabChanged(sender);
    }

    protected void FunProCateTabChanged(object sender)
    {
        // FunPriClearSearchValue();
        AjaxControlToolkit.TabContainer tc = ((AjaxControlToolkit.TabContainer)sender);
        if (tc.ActiveTab != null)
        {
            string strType = tc.ActiveTab.HeaderText;
            FunPriBindGrid("", strType);
            /*int intActiveTab = 0;
            foreach (AjaxControlToolkit.TabPanel tp in tc.Tabs)
            {
                if (tp.HeaderText == strType)
                    break;
                intActiveTab++;
            }
            tcLocationDef.ActiveTabIndex = intActiveTab;*/
        }
    }


    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (tcLocation.ActiveTabIndex == 0)
        {
            if (gvLocationDef.HeaderRow != null)
            {
                ((TextBox)gvLocationDef.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
                ((TextBox)gvLocationDef.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
                ((TextBox)gvLocationDef.HeaderRow.FindControl("txtGSTState")).Text = "";
            }
        }
        else
        {
            if (gvLocationMapping.HeaderRow != null)
            {
                ((TextBox)gvLocationMapping.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
                ((TextBox)gvLocationMapping.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
                ((TextBox)gvLocationDef.HeaderRow.FindControl("txtGSTState")).Text = "";
            }
        }
    }

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>
    private void FunPriGetSearchValue()
    {
        if (tcLocation.ActiveTabIndex == 0)
        {
            if (gvLocationDef.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)gvLocationDef.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
                strSearchVal2 = ((TextBox)gvLocationDef.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
                strSearchVal3 = ((TextBox)gvLocationDef.HeaderRow.FindControl("txtGSTState")).Text.Trim();
            }
        }
        else
        {
            if (gvLocationMapping.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)gvLocationMapping.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
                strSearchVal2 = ((TextBox)gvLocationMapping.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
            }
        }
    }

    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (tcLocation.ActiveTabIndex == 0)
        {
            if (gvLocationDef.HeaderRow != null)
            {
                ((TextBox)gvLocationDef.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
                ((TextBox)gvLocationDef.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
                ((TextBox)gvLocationDef.HeaderRow.FindControl("txtGSTState")).Text = strSearchVal3;
            }
        }
        else
        {
            if (gvLocationMapping.HeaderRow != null)
            {
                ((TextBox)gvLocationMapping.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
                ((TextBox)gvLocationMapping.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
            }
        }
    }


    /// <summary>
    /// This method is used to display Company details
    /// </summary>
    private void FunPriBindGrid(string strCategoryType, string strHierarchy)
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

            if (tcLocation.ActiveTabIndex == 0)
            {
                if (strHierarchy != "")
                    Procparam.Add("@Hierarchy_Type", strHierarchy);
                gvLocationDef.BindGridView("S3G_SYSAD_GetLocationCategoryDetails_Paging", Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
            }
            else
            {
                try
                {
                    gvLocationMapping.BindGridView("S3G_SYSAD_GetLocationMappingDetails_Paging", Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
                }
                catch (Exception ex)
                {
                      ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                }
            }
            //Paging Config

            //This is to hide first row if grid is empty
            if (blnIsNewRow)
            {
                if (gvLocationMapping.Rows.Count >= 1)
                {
                    gvLocationMapping.Rows[0].Visible = false;
                }
                if (gvLocationDef.Rows.Count >= 1)
                {
                    gvLocationDef.Rows[0].Visible = false;
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
            throw;
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
            throw;
        }
        return strSortDirection;
    }

    #endregion [Function's]


    /* Added by Sampath for Location Full Details Export On 02-Mar-2015 */

    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ExportExcel();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void ExportExcel()
    {
        try
        {
            DataTable dtGetdata = new DataTable();
            string StrTabIndex = string.Empty;
            string attachment = string.Empty;
            if (tcLocation.TabIndex == 0)
            {
                AjaxControlToolkit.TabPanel tpCategory = tcLocation.Tabs[0];
                AjaxControlToolkit.TabContainer tcCategory = (AjaxControlToolkit.TabContainer)tpCategory.FindControl("tcLocationDef");
                StrTabIndex = (tcCategory.ActiveTabIndex + 1).ToString();
            }
            
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@HierarchyType", StrTabIndex);

            dtGetdata = Utility.GetDefaultData("[S3G_LocationMasterDetails]", Procparam);

            GridView Grv = new GridView();
            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                switch (StrTabIndex)
                {
                    case "1":
                        attachment = "attachment; filename=LocationCorporate_Report.xls";
                        break;
                    case "2":
                        attachment = "attachment; filename=LocationState_Report.xls";
                        break;
                    case "3":
                        attachment = "attachment; filename=LocationBranch_Report.xls";
                        break;
                }
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");
                dtHeader.Columns.Add("Column2");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                switch (StrTabIndex)
                {
                    case "1":
                        row["Column1"] = "Location Corporate Report";
                        break;
                    case "2":
                        row["Column1"] = "Location State Report";
                        break;
                    case "3":
                        row["Column1"] = "Location Branch Report";
                        break;
                }

                dtHeader.Rows.Add(row);
                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();
                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;
                if (StrTabIndex == "2")
                {
                    grv1.Rows[0].Cells[0].ColumnSpan = 14;
                    grv1.Rows[1].Cells[0].ColumnSpan = 14;
                }
                else
                {
                    grv1.Rows[0].Cells[0].ColumnSpan = 3;
                    grv1.Rows[1].Cells[0].ColumnSpan = 3;
                }
                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;
                grv1.RenderControl(htw);
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
}
