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

public partial class Reports_S3G_RPT_LimitsTracker : ApplyThemeForProject
{

    #region Common Variable declaration

    int intCompanyID, intUserID = 0;
    string strMode = string.Empty;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    string strDateFormat = string.Empty;
    string strPageName = "Limits Tracker Report";
    public static Reports_S3G_RPT_LimitsTracker obj_Page;

    string[] sConsolidate_Name = null;

    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    S3GSession ObjS3GSession = new S3GSession();

    string[] arrSortCol = new string[] { "PODTL.PO_Number" };
    int intNoofSearch = 1;
    ArrayList arrSearchVal = new ArrayList(1);
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

    #region "EVENTS"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #region "BUTTON EVENTS"

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(txtLastDrawnDate.Text) != "" && Convert.ToString(txtEndDate.Text) != "")
            {
                Int32 intDiff = Convert.ToInt32((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtLastDrawnDate.Text)).TotalDays);
                if (intDiff < 0)
                {
                    Utility.FunShowAlertMsg(this, "Last Draw Down Start Date should be less than or equal to Last Draw Down End Date");
                    return;
                }
            }

            if (Convert.ToString(txtsanctnfromDate.Text) != "" && Convert.ToString(txtsanctntoDate.Text) != "")
            {
                Int32 intDiff = Convert.ToInt32((Utility.StringToDate(txtsanctntoDate.Text) - Utility.StringToDate(txtsanctnfromDate.Text)).TotalDays);
                if (intDiff < 0)
                {
                    Utility.FunShowAlertMsg(this, "Sanction To Date should be less than or equal to Sanction From Date");
                    return;
                }
            }
            FunPriBindGrid();
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
            FunPriClear();
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
            FunPriExportExcel();
        }
        catch (Exception objException)
        {
        }
    }

    #endregion

    #endregion

    #region "METHODS"

    private void FunPriLoadPage()
    {
        try
        {
            ObjS3GSession = new S3GSession();
            obj_Page = this;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            #region Paging Config
            arrSearchVal = new ArrayList(intNoofSearch);
            ProPageNumRW = 1;

            System.Web.UI.WebControls.TextBox txtPageSize = (System.Web.UI.WebControls.TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

            #endregion

            if (!IsPostBack)
            {
                ceLastDrawnDate.Format = ceEndDate.Format = ceSanctnFrmDate.Format = ceSanctnToDate.Format=strDateFormat;
                txtLastDrawnDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtLastDrawnDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtsanctnfromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtsanctnfromDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtsanctntoDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtsanctntoDate.ClientID + "','" + strDateFormat + "',false,  false);");
                pnlRptDetails.Visible = false;
                FunPriLoadLOV();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadLOV()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Option", "3");
            Procparam.Add("@User_ID", Convert.ToString(intUserID));

            System.Data.DataTable dtLov = Utility.GetDefaultData("S3G_RPT_GetLmtTrckLKP", Procparam);

            ddlLmtAvailability.FillDataTable(dtLov, "ID", "Description", true);
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }

    private void FunPriClear()
    {
        try
        {
            ddlFunderName.Clear();
            ddlLesseeName.Clear();
            ddlLmtAvailability.SelectedValue = "0";
            txtLastDrawnDate.Text = txtEndDate.Text = string.Empty;
            txtsanctnfromDate.Text = txtsanctntoDate.Text = string.Empty;
            grvLmtTrackDtls.DataSource = null;
            grvLmtTrackDtls.DataBind();
            pnlRptDetails.Visible = btnExport.Visible = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    public void FunPriExportExcel()
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
                string attachment = "attachment; filename=Limit_Tracker_Report.xls";
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
                row["Column1"] = "Limit Tracker Report";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtLastDrawnDate.Text == string.Empty)
                    row["Column1"] = "Last Draw Down Start Date : ";
                else
                    row["Column1"] = "Last Draw Down Start Date : " + txtLastDrawnDate.Text;

                if (txtEndDate.Text == string.Empty)
                    row["Column2"] = "Last Draw Down End Date : ";
                else
                    row["Column2"] = "Last Draw Down End Date : " + txtEndDate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtsanctnfromDate.Text == string.Empty)
                    row["Column1"] = "Sanction From Date : ";
                else
                    row["Column1"] = "Sanction From Date : " + txtsanctnfromDate.Text;

                if (txtsanctntoDate.Text == string.Empty)
                    row["Column2"] = "Sanction To Date : ";
                else
                    row["Column2"] = "Sanction To Date : " + txtsanctntoDate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlLesseeName.SelectedValue == "0")
                    row["Column1"] = "Lessee Name : --All--";
                else
                    row["Column1"] = "Lessee Name : " + ddlLesseeName.SelectedText;

                if (ddlFunderName.SelectedValue == "0")
                    row["Column2"] = "Funder Name : --All--";
                else
                    row["Column2"] = "Funder Name : " + ddlFunderName.SelectedText;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Limit Availability : " + ddlLmtAvailability.SelectedItem.Text;
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
                grv1.Rows[2].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[3].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[4].HorizontalAlign = HorizontalAlign.Left;

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

    #region Page Methods

    private void FunPriBindGrid()
    {
        try
        {
            lblPagingErrorMessage.InnerText = "";
            pnlRptDetails.Visible = true;
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "1");
            if (Convert.ToInt64(ddlFunderName.SelectedValue) > 0 && Convert.ToString(ddlFunderName.SelectedText) != "")
            {
                Procparam.Add("@Funder_ID", Convert.ToString(ddlFunderName.SelectedValue));
            }

            if (Convert.ToInt64(ddlLesseeName.SelectedValue) > 0 && Convert.ToString(ddlLesseeName.SelectedText) != "")
            {
                Procparam.Add("@Customer_ID", Convert.ToString(ddlLesseeName.SelectedValue));
            }

            if (Convert.ToString(txtLastDrawnDate.Text) != "")
            {
                Procparam.Add("@Expiry_Date", Convert.ToString(Utility.StringToDate(txtLastDrawnDate.Text)));
            }

            if (Convert.ToString(txtEndDate.Text) != "")
            {
                Procparam.Add("@Expiry_End_Date", Convert.ToString(Utility.StringToDate(txtEndDate.Text)));
            }

            if (Convert.ToString(txtsanctnfromDate.Text) != "")
            {
                Procparam.Add("@Sanction_From_Date", Convert.ToString(Utility.StringToDate(txtsanctnfromDate.Text)));
            }

            if (Convert.ToString(txtsanctntoDate.Text) != "")
            {
                Procparam.Add("@Sanction_To_Date", Convert.ToString(Utility.StringToDate(txtsanctntoDate.Text)));
            }

            if (Convert.ToInt64(ddlLmtAvailability.SelectedValue) > 0)
            {
                Procparam.Add("@Limit_Status", Convert.ToString(ddlLmtAvailability.SelectedValue));
            }


            grvLmtTrackDtls.BindGridView("SG_RPT_LmtTrackerDetails", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            //System.Data.DataTable dtLmtDtl = ((DataView)grvLmtTrackDtls.DataSource).ToTable();
            //ViewState["LmtTrackerDtl"] = dtLmtDtl;

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvLmtTrackDtls.Rows[0].Visible = false;
            }

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            btnExport.Visible = (Convert.ToInt64(intTotalRecords) > 0) ? true : false;

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblPagingErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblPagingErrorMessage.InnerText = ex.Message;
        }

    }

    private System.Data.DataTable FunPriGetDetails()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "2");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (Convert.ToInt64(ddlFunderName.SelectedValue) > 0 && Convert.ToString(ddlFunderName.SelectedText) != "")
            {
                Procparam.Add("@Funder_ID", Convert.ToString(ddlFunderName.SelectedValue));
            }

            if (Convert.ToInt64(ddlLesseeName.SelectedValue) > 0 && Convert.ToString(ddlLesseeName.SelectedText) != "")
            {
                Procparam.Add("@Customer_ID", Convert.ToString(ddlLesseeName.SelectedValue));
            }

            if (Convert.ToString(txtLastDrawnDate.Text) != "")
            {
                Procparam.Add("@Expiry_Date", Convert.ToString(Utility.StringToDate(txtLastDrawnDate.Text)));
            }

            if (Convert.ToString(txtEndDate.Text) != "")
            {
                Procparam.Add("@Expiry_End_Date", Convert.ToString(Utility.StringToDate(txtEndDate.Text)));
            }

            if (Convert.ToString(txtsanctnfromDate.Text) != "")
            {
                Procparam.Add("@Sanction_From_Date", Convert.ToString(Utility.StringToDate(txtsanctnfromDate.Text)));
            }

            if (Convert.ToString(txtsanctntoDate.Text) != "")
            {
                Procparam.Add("@Sanction_To_Date", Convert.ToString(Utility.StringToDate(txtsanctntoDate.Text)));
            }

            if (Convert.ToInt64(ddlLmtAvailability.SelectedValue) > 0)
            {
                Procparam.Add("@Limit_Status", Convert.ToString(ddlLmtAvailability.SelectedValue));
            }

            System.Data.DataTable dtLmt = Utility.GetDefaultData("SG_RPT_LmtTrackerDetails", Procparam);
            return dtLmt;
        }
        catch (Exception objException)
        {
            throw objException;
            return null;
        }
    }

    #endregion

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

    #endregion


}