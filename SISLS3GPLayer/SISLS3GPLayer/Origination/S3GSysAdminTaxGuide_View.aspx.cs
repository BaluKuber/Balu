
/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminTaxGuide_View
/// Created By      :   Vinodha M
/// Created Date    :   24-SEP-2014
/// Purpose         :   To view tax guide details

using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Text;
using System.Configuration;
using System.Web.Security;
using System.IO;
using System.IO.Compression;


public partial class S3GSysAdminTaxGuide_View : ApplyThemeForProject
{
    #region Intialization

    string strRedirectPage = "~/Origination/S3GSysAdminTaxGuide_Add.aspx";
    int intCompanyID = 0;
    int intUserId = 0;
    UserInfo ObjUserInfo = null;

    public static S3GSysAdminTaxGuide_View ojb_TransLander = null;
    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal6 = string.Empty;
    string strSearchVal7 = string.Empty;
    string strSearchVal8 = string.Empty;
    string strSearchEffDt = string.Empty;
    string strSearchVal9 = string.Empty;
    int intUserID = 0;

    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bMakerChecker = false;
    bool bIsActive = false;
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

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        ojb_TransLander = this;
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


        #endregion

        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        lblErrorMessage.InnerText = "";

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;

        S3GSession ObjS3GSession = new S3GSession();
        string strDateFormat = ObjS3GSession.ProDateFormatRW;                              // to get the standard date format of the Application
        CalendarEffectiveDateSearch.Format = strDateFormat;

        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
            FunPubFillAssetCategoryDetails();// by Siva
            grvTaxGuide.Visible = false;
            ucCustomPaging.Visible = false;
            //FunPriBindGrid();
            txtEffectiveDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEffectiveDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            if (!bIsActive)
            {
                grvTaxGuide.Columns[1].Visible = false;
                grvTaxGuide.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }

            if (!bModify)
            {
                grvTaxGuide.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvTaxGuide.Columns[0].Visible = false;
            }
            if (!bCreate)
            {
                btnCreate.Enabled = false;
            }
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// This method is used to display TaxGuide details
    /// </summary>

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    private void FunPriBindGrid()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            #region Paging Properties set

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            #endregion //Paging Properties end

            #region Paging Config
            FunPriGetSearchValue();

            #region To Show Grid Header
            bool bIsNewRow = false;

            grvTaxGuide.BindGridView("S3G_Get_TaxGuide_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            #endregion

            #region To Hide First Row if grid is empty

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvTaxGuide.Rows[0].Visible = false;
            }

            // FunPriSetSearchValue();

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            #endregion
            #endregion Paging Config End
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {

        }
    }

    #endregion

    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        //if (grvTaxGuide.HeaderRow != null)
        //{
        //    strSearchVal1 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim().Replace("'", "''");            
        //    strSearchVal3 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim().Replace("'", "''");
        //    strSearchVal4 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim().Replace("'", "''");            
        //    strSearchVal6 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch6")).Text.Trim().Replace("'", "''");
        //    strSearchVal7 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch7")).Text.Trim().Replace("'", "''");
        //    strSearchVal8 = ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch8")).Text.Trim().Replace("'", "''");
        //}
        if (txtState.SelectedText != "")
            strSearchVal1 = txtState.SelectedText.Replace("'", "''");
        if (ddlTaxType.SelectedIndex > 0)
            strSearchVal3 = ddlTaxType.SelectedItem.Text.Replace("'", "''");
        if (ddlTaxClass.SelectedIndex > 0)
            strSearchVal4 = ddlTaxClass.SelectedItem.Text.Replace("'", "''");
        if (ddlAssetCategory.SelectedIndex > 0)
            strSearchVal6 = ddlAssetCategory.SelectedItem.Text.Replace("'", "''");
        if (txtAssetType.SelectedText != "")
            strSearchVal7 = txtAssetType.SelectedText.Replace("'", "''");
        if (ddlReverseCharge.SelectedIndex > 0)
            strSearchVal8 = ddlReverseCharge.SelectedItem.Text.Replace("'", "''");
        if (txtEffectiveDateSearch.Text != "")
            //strSearchEffDt = txtEffectiveDateSearch.Text;
            strSearchEffDt = Utility.StringToDate(txtEffectiveDateSearch.Text).ToString();
        if (txtHSNCode.SelectedText != "")
            strSearchVal9 = txtHSNCode.SelectedText.Replace("'", "''");
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        //txtState.Text = string.Empty;
        txtState.SelectedText = "";
        txtState.SelectedValue = "";
        ddlTaxType.SelectedIndex = -1;
        ddlTaxClass.SelectedIndex = -1;
        ddlAssetCategory.SelectedIndex = -1;
        //txtAssetType.Text = string.Empty;
        txtAssetType.SelectedText = "";
        txtAssetType.SelectedValue = "";
        txtHSNCode.Clear();
        ddlReverseCharge.SelectedIndex = -1;
        txtEffectiveDateSearch.Text = string.Empty;
        //if (grvTaxGuide.HeaderRow != null)
        //{
        //    ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";            
        //    ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
        //    ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";            
        //    ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch6")).Text = "";
        //    ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch7")).Text = "";
        //                ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch8")).Text = "";
        //}
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvTaxGuide.HeaderRow != null)
        {
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch6")).Text = strSearchVal6;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch7")).Text = strSearchVal7;
            ((TextBox)grvTaxGuide.HeaderRow.FindControl("txtHeaderSearch8")).Text = strSearchVal8;
        }
    }

    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>

    protected void FunProHeaderSearch()
    {

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            //object sender, EventArgs e
            //txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver
            if (strSearchVal1 != "")
            {
                //strSearchVal += " and State_Name like '%" + strSearchVal1 + "%'";
                strSearchVal += " and State_Name = '" + strSearchVal1 + "'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " And tbl.TaxType = '" + strSearchVal3 + "'";
            }
            if (strSearchVal4 != "")
            {
                strSearchVal += " And tbl.TaxClass = '" + strSearchVal4 + "'";
            }
            if (strSearchVal6 != "")
            {
                strSearchVal += " and Asset_Category = '" + strSearchVal6 + "'";
            }
            if (strSearchVal7 != "")
            {
                strSearchVal += " and Asset_Type = '" + strSearchVal7 + "'";
                //strSearchVal += " and Asset_Type like '%" + strSearchVal7 + "%'";
            }
            if (strSearchVal8 != "")
            {
                strSearchVal += " and REV_CHARGE_DESC = '" + strSearchVal8 + "'";
            }
            if (strSearchEffDt != "")
            {
                strSearchVal += " and Effective_From = '" + strSearchEffDt + "'";
            }
            if (strSearchVal9 != "")
            {
                strSearchVal += " and HSN_Code = '" + strSearchVal9 + "'";
            }
            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            //FunPriSetSearchValue();
            //if (txtboxSearch.Text != "")
            //    ((TextBox)grvTaxGuide.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
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
        if (strColumn == "State_Name")
            strColumn = "State_Name";
        if (strColumn == "TaxType")
            strColumn = "TaxType";
        if (strColumn == "TaxClass")
            strColumn = "TaxClass";
        if (strColumn == "Asset_Category")
            strColumn = "Asset_Category";
        if (strColumn == "Asset_Type")
            strColumn = "Asset_Type";
        if (strColumn == "Effective_From")
            strColumn = "Effective_From";
        if (strColumn == "REV_CHARGE_DESC")
            strColumn = "REV_CHARGE_DESC";
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
                case "lnkbtnstate":
                    strSortColName = "State_Name";
                    break;
                case "lnkbtnSortTaxType":
                    strSortColName = "TaxType";
                    break;
                case "lnkbtnTaxClass":
                    strSortColName = "TaxClass";
                    break;
                case "lnkbtnAsset_Category":
                    strSortColName = "Asset_Category";
                    break;
                case "lnkbtnAsset_Type":
                    strSortColName = "Asset_Type";
                    break;
                case "lnkbtnEffFrom":
                    strSortColName = "Effective_From";
                    break;
                case "lnkbtnRev_Charge_typ":
                    strSortColName = "REV_CHARGE_DESC";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvTaxGuide.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {
                ((ImageButton)grvTaxGuide.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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
        Response.Redirect(strRedirectPage, false);
    }

    protected void grvTaxGuide_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPage + "?qsTaxCodeId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPage + "?qsTaxCodeId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;
        }
    }

    protected void grvTaxGuide_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }

            ////User Authorization
            //Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            //Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            //ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            //if (lblUserID.Text != "")
            //{
            //    //Modified by saranya 10-Feb-2012 to validate user based on user level and Maker Checker
            //    //if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
            //    if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text), true)))
            //    {
            //        imgbtnEdit.Enabled = true;
            //    }
            //    else
            //    {
            //        imgbtnEdit.Enabled = false;
            //        imgbtnEdit.CssClass = "styleGridEditDisabled";
            //    }
            //}
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            //FunPriBindGrid();
            grvTaxGuide.DataSource = null;
            grvTaxGuide.Visible = false;
            ucCustomPaging.Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion


    /* Added by Sampath for Tax Guide Full Details Export On 25-Feb-2015 */


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
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            if (ddlTaxClass.SelectedValue != "0")
                Procparam.Add("@Tax_Class", ddlTaxClass.SelectedValue);
            if (ddlTaxType.SelectedValue != "0")
                Procparam.Add("@Tax_Type_ID", ddlTaxType.SelectedValue);
            if (txtEffectiveDateSearch.Text != "")
                Procparam.Add("@Effective_From", Utility.StringToDate(txtEffectiveDateSearch.Text).ToString());

            dtGetdata = Utility.GetDefaultData("[S3G_TaxGuideMasterDetails_NEW]", Procparam);

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
                row["Column1"] = "TaxGuide Report";
                dtHeader.Rows.Add(row);
                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();
                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 17;
                grv1.Rows[1].Cells[0].ColumnSpan = 17;
                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;

                string path = Server.MapPath("~/TaxGuidereport/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(path + "TaxGuidereport.xls"))
                {
                    File.Delete(path + "TaxGuidereport.xls");
                }

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StreamWriter writer = File.AppendText(path + "TaxGuidereport.xls");
                        grv1.RenderControl(hw);
                        Grv.RenderControl(hw);
                        writer.WriteLine(sw.ToString());
                        writer.Close();
                    }
                }

                //Map the path where the zip file is to be stored
                string DestinationPath = Server.MapPath("~/TaxGuideDetails/");

                //creating the directory when it is not existed
                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                //concatenation of the path and name
                string filePath = DestinationPath + "TaxGuidereport.zip";

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
                    if (File.Exists(path + "TaxGuidereport.xls"))
                    {
                        File.Delete(path + "TaxGuidereport.xls");
                    }
                    //Delete The folder where the excel file is created
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path);
                    }

                    //download compressed file                    
                    FileInfo file = new FileInfo(filePath);

                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + "TaxGuidereport.zip");
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

    #region Search Fields-Tax Gude Master

    /// <summary>
    /// Will bind the grid and validate the from and to date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        #region  User Authorization
        //if (!bIsActive)
        //{
        //    isEditColumnVisible =
        //    isQueryColumnVisible = false;
        //}
        //if ((!bModify) || (intModify == 0))
        //{
        //    isEditColumnVisible = false;

        //}
        //if ((!bQuery) || (intModify == 0))
        //{
        //    isQueryColumnVisible = false;
        //}
        #endregion
        Session["DocumentNo"] = null;
        grvTaxGuide.Visible = true;
        //FunPriValidateFromEndDate();
        FunProHeaderSearch();


    }


    protected void ddlTaxClass_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string TaxClass = ddlTaxClass.SelectedItem.Text;
            FunPubFillTaxTypeDetails(TaxClass);

            FunPubFillReverseCharge_ZoneDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            //lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlTaxType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubFillReverseCharge_ZoneDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            //lblErrorMessage.Text = ex.Message;
        }
    }

    //protected void ddlExDGLCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UserControls_S3GAutoSuggest ddl_stateF = (UserControls_S3GAutoSuggest)obj_Page.gvcashextended.FooterRow.FindControl("ddl_stateF");
    //    ddl_stateF.Focus();
    //}

    //tax Type
    protected void FunPubFillTaxTypeDetails(string TaxClass)
    {
        try
        {
            Dictionary<string, string> dictParam;
            DataSet dsInitTaxTypeDetails;
            int taxtype = 0;
            if (ddlTaxClass.SelectedValue != "0")
            {
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Company_ID", intCompanyID.ToString());
                dictParam.Add("@User_ID", intUserId.ToString());
                dictParam.Add("@TaxClass", TaxClass);
                dsInitTaxTypeDetails = Utility.GetDataset("GetTaxTypeDetailsByTaxClass", dictParam);
                ddlTaxType.Enabled = true;
                ddlTaxType.BindDataTable(dsInitTaxTypeDetails.Tables[0], new string[] { "Value", "Name" });
            }
            else
            {
                ddlTaxType.Items.Clear();
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlTaxType.Items.Insert(0, liSelect);
            }
            //if (ddlTaxType.SelectedIndex > 0)
            //    taxtype = Convert.ToInt32(ddlTaxType.SelectedValue);
            //FunPriFillAssetHistory(taxclass, taxtype, ddlleasestate.SelectedValue, Asset_Type_ID);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            //lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    //Asset  Type  

    /// <summary>
    /// GetAssettypeList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetAssettypeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        //Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(ojb_TransLander.intCompanyID));
        //Procparam.Add("@User_ID", Convert.ToString(ojb_TransLander.intUserId));
        Procparam.Add("@Option", "2");
        Procparam.Add("@PrefixText", prefixText);
        // Procparam.Add("@ProgramCode", ojb_TransLander.ProgramCode);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GETTAXGUIDE_TRANSVALUES", Procparam));


        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetHSNCode(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
       
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(ojb_TransLander.intCompanyID));
        Procparam.Add("@Option", "4");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GETTAXGUIDE_TRANSVALUES", Procparam));


        return suggetions.ToArray();
    }

    protected void FunPubFillAssetCategoryDetails()
    {
        try
        {
            Dictionary<string, string> Procparam;
            DataSet dsInitTaxTypeDetails;
            int taxtype = 0;
            DataSet dsInitLoadDetails;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            // Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Option", "1");
            dsInitLoadDetails = Utility.GetDataset("S3G_ORG_GETTAXGUIDE_TRANSVALUES", Procparam);
            ddlAssetCategory.BindDataTable(dsInitLoadDetails.Tables[0], new string[] { "Id", "Name" });
            //ddlAssetType.BindDataTable(dsInitLoadDetails.Tables[5], new string[] { "AT_ID", "AT_DESC" });
            //ddlAssetType.Items.RemoveAt(0);
            //ddlAssetType.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("ALL", "0")));
            //ddlAssetType.Focus();


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            //lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    /// <summary>
    /// GetAssettypeList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetStateList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        //Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(ojb_TransLander.intCompanyID));
        //Procparam.Add("@User_ID", Convert.ToString(ojb_TransLander.intUserId));
        Procparam.Add("@Option", "3");
        Procparam.Add("@PrefixText", prefixText);
        // Procparam.Add("@ProgramCode", ojb_TransLander.ProgramCode);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GETTAXGUIDE_TRANSVALUES", Procparam));


        return suggetions.ToArray();
    }


    //rEVERSE cHARGE -Zone
    protected void FunPubFillReverseCharge_ZoneDetails()
    {
        try
        {
            if (ddlTaxClass.SelectedValue != "0" && ddlTaxType.SelectedValue != "0")
            {
                Dictionary<string, string> dictParam;
                DataSet dsInitLoadDetails;
                int taxtype = 0;
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Company_ID", intCompanyID.ToString());
                dictParam.Add("@User_ID", intUserId.ToString());
                //dictParam.Add("@TaxClass", TaxClass);
                dsInitLoadDetails = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", dictParam);
                ddlReverseCharge.Enabled = true;
                if (ddlTaxClass.SelectedValue == "1" && ddlTaxType.SelectedValue == "2")//2-Reverse Charge Type
                {
                    ddlReverseCharge.BindDataTable(dsInitLoadDetails.Tables[1], new string[] { "Value", "Name" });
                    if (ddlReverseCharge.Items.Count > 0)
                    {
                        ddlReverseCharge.Items.RemoveAt(0);
                        //ddlReverseCharge.Items.Insert(0, "ALL");
                        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                        ddlReverseCharge.Items.Insert(0, liSelect);
                    }
                }
                else if (ddlTaxClass.SelectedValue == "2" && ddlTaxType.SelectedValue == "4")//4-Zone
                {
                    ddlReverseCharge.BindDataTable(dsInitLoadDetails.Tables[2], new string[] { "Value", "Name" });
                }
                else
                {
                    ddlReverseCharge.Items.Clear();
                    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                    ddlReverseCharge.Items.Insert(0, liSelect);
                }
            }
            else
            {
                ddlReverseCharge.Items.Clear();
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlReverseCharge.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            //lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }


    #endregion
}
