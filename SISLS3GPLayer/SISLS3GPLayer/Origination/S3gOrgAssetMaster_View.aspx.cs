#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Asset Master
/// Created By			: Chandrasekar K
/// Created Date		: 25-Sep-2014
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Data;
using System.Security;
using System.Web.Security;
using System.Configuration;
using System.ServiceModel;
using System.IO;
using System.IO.Compression;
#endregion

public partial class Origination_S3g_OrgAssetMaster_View : ApplyThemeForProject
{
    #region Intialization
    // OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    string strRedirectPageAdd = "~/Origination/S3gOrgAssetMaster_Add.aspx";
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
        FunPriBindGrid(TabContainerAM.ActiveTabIndex + 1);
    }

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
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

        //if (TabAssetCat.ToString() == 1)
        //{
        //    btnExportToExcel.Visible = true;
        //}

        #endregion
        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
            FunPriBindGrid(TabContainerAM.ActiveTabIndex + 1);
            //User Authorization

            if (!bIsActive)
            {
                grvAssetCategoryCodes.Columns[1].Visible = false;
                grvAssetCategoryCodes.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }

            if (!bModify)
            {
                grvAssetCategoryCodes.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvAssetCategoryCodes.Columns[0].Visible = false;
            }
            if (!bCreate)
            {
                btnCreate.Enabled = false;
            }
            //Authorization Code end

        }


    }
    #endregion

    //#region Page Events     

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (TabContainerAM.ActiveTabIndex == 0)
        {
            Response.Redirect("~/Origination/S3GOrgAssetMaster_Add.aspx?qsType=1&qsMode=C");
        }
        else if (TabContainerAM.ActiveTabIndex == 1)
        {
            Response.Redirect("~/Origination/S3GOrgAssetMaster_Add.aspx?qsType=2&qsMode=C");
        }
        else
        {
            Response.Redirect("~/Origination/S3GOrgAssetMaster_Add.aspx?qsType=3&qsMode=C");
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunPriBindGrid(TabContainerAM.ActiveTabIndex + 1);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvAssetClassCodes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsAssetCatId=" + FormsAuthentication.Encrypt(Ticket) + "&qsType=" + (TabContainerAM.ActiveTabIndex + 1) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsAssetCatId=" + FormsAuthentication.Encrypt(Ticket) + "&qsType=" + (TabContainerAM.ActiveTabIndex + 1) + "&qsMode=Q");
                break;
        }
    }

    protected void tccategoryCodes_ActiveTabChanged(object sender, EventArgs e)
    {
        FunPriClearSearchValue();
        hdnSearch.Value = "";
        hdnOrderBy.Value = "";
        FunPriBindGrid(TabContainerAM.ActiveTabIndex + 1);
    }

    //#endregion

    //#region Page Methods

    //protected string FuncProConactFiledsStr(string strField1, string strField2)
    //{
    //    return strField1 + "," + strField2;
    //}

    /// <summary>
    /// This method is used to display Company details
    /// </summary>
    private void FunPriBindGrid(int strCategoryType)
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

            if (strCategoryType == 1)
            {
                btnExportToExcel.Visible = true;
            }

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            try
            {
                Procparam.Add("@Category_Type", strCategoryType.ToString());
                grvAssetCategoryCodes.BindGridView("S3G_ORG_GetAssetMaster_Paging", Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            }

            //Paging Config

            lblErrorMessage.InnerText = string.Empty;
            //This is to show grid header

            //This is to hide first row if grid is empty
            if (blnIsNewRow)
            {
                if (grvAssetCategoryCodes.Rows.Count >= 1)
                {
                    grvAssetCategoryCodes.Rows[0].Visible = false;
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
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        if (grvAssetCategoryCodes.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
            strSearchVal2 = ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (grvAssetCategoryCodes.HeaderRow != null)
        {
            ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";

        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvAssetCategoryCodes.HeaderRow != null)
        {
            ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
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

                strSearchVal += grvAssetCategoryCodes.Columns[2].SortExpression + " like '%" + strSearchVal1 + "%'";

            }
            if (strSearchVal2 != "")
            {

                strSearchVal += " and " + grvAssetCategoryCodes.Columns[3].SortExpression + " like '%" + strSearchVal2 + "%'";

            }
            if (strSearchVal3 != "")
            {

                strSearchVal += " and " + grvAssetCategoryCodes.Columns[4].SortExpression + " like '%" + strSearchVal3 + "%'";

            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;

            FunPriBindGrid(TabContainerAM.ActiveTabIndex + 1);

            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                //((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
                txtboxSearch.Focus();

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
                case "lnkbtnSort1":
                    strSortColName = "Category_Code";
                    break;
                case "lnkbtnSort2":
                    strSortColName = "Category_Description";
                    break;
                case "lnkbtnSort3":
                    strSortColName = "Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);

            FunPriBindGrid(TabContainerAM.ActiveTabIndex + 1);

            if (strDirection == "ASC")
            {
                ((ImageButton)grvAssetCategoryCodes.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {
                ((ImageButton)grvAssetCategoryCodes.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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

    //#endregion


    protected void grvAssetCategoryCodes_RowDataBound(object sender, GridViewRowEventArgs e)
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

    /* Added by Sampath for Customer Details Export On 27-Feb-2015 */

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
            //string attachment = string.Empty;
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            dtGetdata = Utility.GetDefaultData("[S3G_AssetCategoryDetails]", Procparam);

            GridView Grv = new GridView();
            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");
                dtHeader.Columns.Add("Column2");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();

                row["Column1"] = "Asset Master Report";

                dtHeader.Rows.Add(row);
                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();
                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 7;
                grv1.Rows[1].Cells[0].ColumnSpan = 7;
                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;

                string path = Server.MapPath("~/AssetReport/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(path + "Asset_Report.xls"))
                {
                    File.Delete(path + "Asset_Report.xls");
                }

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StreamWriter writer = File.AppendText(path + "Asset_Report.xls");
                        grv1.RenderControl(hw);
                        Grv.RenderControl(hw);
                        writer.WriteLine(sw.ToString());
                        writer.Close();
                    }
                }

                //Map the path where the zip file is to be stored
                string DestinationPath = Server.MapPath("~/AssetDetails/");

                //creating the directory when it is not existed
                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                //concatenation of the path and name
                string filePath = DestinationPath + "Asset_Report.zip";

                //before creation of compressed folder,deleting it if exists
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                //checking the path is available or not
                if (!File.Exists(filePath))
                {
                    //creating a zip file from one folder to another folder
                    ZipFile.CreateFromDirectory(path, filePath);

                    //Delete The excel file which is created
                    if (File.Exists(path + "Asset_Report.xls"))
                    {
                        File.Delete(path + "Asset_Report.xls");
                    }
                    //Delete The folder where the excel file is created
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path);
                    }

                    //download compressed file                    
                    FileInfo file = new FileInfo(filePath);

                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + "Asset_Report.zip");                    
                    Response.ContentType = "application/x-zip-compressed";
                    Response.WriteFile(filePath);                    
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
}
