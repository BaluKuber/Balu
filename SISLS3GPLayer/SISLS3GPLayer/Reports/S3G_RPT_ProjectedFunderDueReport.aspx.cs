using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using S3GBusEntity;
using System.IO;

public partial class Reports_S3G_RPT_ProjectedFunderDueReport : ApplyThemeForProject
{

    #region Common Variable declaration

    int intCompanyID, intUserID = 0;
    string strMode = string.Empty;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    string strDateFormat = string.Empty;
    string strPageName = "Projected Funder Due Report";
    public static Reports_S3G_RPT_ProjectedFunderDueReport obj_Page;

    string[] sConsolidate_Name = null;

    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    S3GSession ObjS3GSession = new S3GSession();

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
        FunPriLoadReport();
    }

    #endregion

    #region "EVENTS"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            FunPriLoadPage();
        }
        catch (Exception objException)
        {
            CVFunderRpt.ErrorMessage = "Due to Data Problem, Unable to Load Projected Funder Due Report.";
            CVFunderRpt.IsValid = false;
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #region "BUTTON EVENTS"

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            bool blnChk = true;
            if (Convert.ToString(ddlLesseeName.SelectedText) != "" && Convert.ToInt64(ddlLesseeName.SelectedValue) == 0)
            {
                Utility.FunShowAlertMsg(this, "Invalid Lessee Name");
                ddlLesseeName.Clear();
                blnChk = false;
            }
            else if (Convert.ToString(ddlFunderName.SelectedText) != "" && Convert.ToInt64(ddlFunderName.SelectedValue) == 0)
            {
                Utility.FunShowAlertMsg(this, "Invalid Funder Name");
                ddlFunderName.Clear();
                blnChk = false;
            }
            else if (Convert.ToString(ddlTranchNumber.SelectedText) != "" && Convert.ToInt64(ddlTranchNumber.SelectedValue) == 0)
            {
                Utility.FunShowAlertMsg(this, "Invalid Note Number");
                ddlTranchNumber.Clear();
                blnChk = false;
            }

            if (blnChk == false)
            {
                FunPriClearGrid();
                return;
            }

            FunPriLoadReport();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLesseeName.Clear();
            ddlFunderName.Clear();
            ddlTranchNumber.Clear();
            FunPriClearGrid();
            txtFromDate.Text = DateTime.Now.ToString(strDateFormat);
            txtToDate.Text = "";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtGetdata = FunPriGetDetails();

            GridView Grv = new GridView();

            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=ProjectedFunderDueReport.xls";
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
                if (txtToDate.Text == "")
                    row["Column1"] = "Projected Funder Due Report" + " As On Date : " + Convert.ToString(txtFromDate.Text);
                else
                    row["Column1"] = "Projected Funder Due Report" + " For the Period " + txtFromDate.Text + " to " + txtToDate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Lessee Name : " + Convert.ToString(ddlLesseeName.SelectedText);
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Funder Name : " + Convert.ToString(ddlFunderName.SelectedText);
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Tranche Number : " + Convert.ToString(ddlTranchNumber.SelectedText);
                dtHeader.Rows.Add(row);

                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 10;
                grv1.Rows[1].Cells[0].ColumnSpan = 10;
                grv1.Rows[2].Cells[0].ColumnSpan = 4;
                grv1.Rows[3].Cells[0].ColumnSpan = 4;
                grv1.Rows[4].Cells[0].ColumnSpan = 4;

                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[2].HorizontalAlign = grv1.Rows[3].HorizontalAlign = grv1.Rows[4].HorizontalAlign = grv1.Rows[4].HorizontalAlign = HorizontalAlign.Left;

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
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "GridView Events"

    protected void grvFunderOsDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
            {
                for (int i_cellVal = 1; i_cellVal < e.Row.Cells.Count; i_cellVal++)
                {
                    // if (!string.IsNullOrEmpty(e.Row.Cells[i_cellVal].Text) && e.Row.Cells[i_cellVal].Text.Contains("/"))
                    if (!string.IsNullOrEmpty(e.Row.Cells[i_cellVal].Text))
                    {
                        Int32 type = 0;       // 1 = int, 2 = datetime, 3 = string

                        type = FunPriTypeCast(e.Row.Cells[i_cellVal].Text);

                        // cell alignment
                        switch (type)
                        {
                            case 1:  // int - right to left
                                e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Right;
                                break;
                            case 3:  // string - do nothing - left align(default)
                                e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Left;
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #endregion

    #region "Methods"

    private void FunPriLoadPage()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            Session["AccountingCurrency"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";

            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            intUserID = ObjUserInfo.ProUserIdRW;

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

            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString(strDateFormat);
                txtFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
                ceDate.Format = ceToDate.Format = strDateFormat;
                pnlRptDetails.Visible = ucCustomPaging.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Projected Funder Due Report page");
        }
    }

    private Int32 FunPriTypeCast(string val)
    {
        try                                                         // casting - to use proper align       
        {
            Int32 tempint = Convert.ToInt32(Convert.ToDecimal(val));                   // Try int     
            return 1;
        }
        catch (Exception ex)
        {
            return 3;        // try String
        }
    }

    private void FunPriLoadReport()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            Procparam.Add("@IsExport", "0");
            Procparam.Add("@From_Date", Utility.StringToDate(txtFromDate.Text).ToString());

            if (txtToDate.Text != "")
                Procparam.Add("@To_Date", Utility.StringToDate(txtToDate.Text).ToString());

            if (Convert.ToString(ddlFunderName.SelectedText) != "" && Convert.ToInt64(ddlFunderName.SelectedValue) > 0)
                Procparam.Add("@Funder_ID", Convert.ToString(ddlFunderName.SelectedValue));

            if (Convert.ToString(ddlLesseeName.SelectedText) != "" && Convert.ToInt64(ddlLesseeName.SelectedValue) > 0)
                Procparam.Add("@Customer_ID", Convert.ToString(ddlLesseeName.SelectedValue));

            if (Convert.ToString(ddlTranchNumber.SelectedText) != "" && Convert.ToInt64(ddlTranchNumber.SelectedValue) > 0)
                Procparam.Add("@Tranche_ID", Convert.ToString(ddlTranchNumber.SelectedValue));

            bool bIsNewRow = false;
            grvFunderOsDtl.BindGridView("S3G_RPT_ProjectedFunderDueReport", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            if (bIsNewRow)
            {
                grvFunderOsDtl.Rows[0].Visible = btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            grvFunderOsDtl.Visible = ucCustomPaging.Visible = pnlRptDetails.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private DataTable FunPriGetDetails()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@IsExport", "1");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));

            Procparam.Add("@From_Date", Utility.StringToDate(txtFromDate.Text).ToString());

            if (txtToDate.Text != "")
                Procparam.Add("@To_Date", Utility.StringToDate(txtToDate.Text).ToString());

            if (Convert.ToString(ddlFunderName.SelectedText) != "" && Convert.ToInt64(ddlFunderName.SelectedValue) > 0)
                Procparam.Add("@Funder_ID", Convert.ToString(ddlFunderName.SelectedValue));

            if (Convert.ToString(ddlLesseeName.SelectedText) != "" && Convert.ToInt64(ddlLesseeName.SelectedValue) > 0)
                Procparam.Add("@Customer_ID", Convert.ToString(ddlLesseeName.SelectedValue));

            if (Convert.ToString(ddlTranchNumber.SelectedText) != "" && Convert.ToInt64(ddlTranchNumber.SelectedValue) > 0)
                Procparam.Add("@Tranche_ID", Convert.ToString(ddlTranchNumber.SelectedValue));

            DataTable dtLmt = Utility.GetDefaultData("S3G_RPT_ProjectedFunderDueReport", Procparam);
            return dtLmt;
        }
        catch (Exception objException)
        {
            throw objException;
            return null;
        }
    }

    private void FunPriClearGrid()
    {
        try
        {
            grvFunderOsDtl.DataSource = null;
            grvFunderOsDtl.DataBind();
            ucCustomPaging.Visible = pnlRptDetails.Visible = btnExport.Visible = false;
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }
    #endregion

    #region "WEB METHODS"

    [System.Web.Services.WebMethod]
    public static string[] GetLesseeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Prefix_Text", prefixText);
        Procparam.Add("@Option", "1");

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetLmtTrckLKP", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetFunderList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Prefix_Text", prefixText);
        Procparam.Add("@Option", "2");

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetLmtTrckLKP", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Prefix_Text", prefixText);
        Procparam.Add("@Option", "5");

        if (Convert.ToString(obj_Page.ddlFunderName.SelectedText) != "" && Convert.ToInt64(obj_Page.ddlFunderName.SelectedValue) > 0)
            Procparam.Add("@Funder_ID", Convert.ToString(obj_Page.ddlFunderName.SelectedValue));

        if (Convert.ToString(obj_Page.ddlLesseeName.SelectedText) != "" && Convert.ToInt64(obj_Page.ddlLesseeName.SelectedValue) > 0)
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ddlLesseeName.SelectedValue));

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetLmtTrckLKP", Procparam));

        return suggetions.ToArray();
    }
    #endregion
}