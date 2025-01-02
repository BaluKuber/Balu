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
using System.Collections.Generic;
using S3GBusEntity;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.IO;
using System.Text;

public partial class Reports_S3G_Rpt_EInvoiceReport : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    string intCustomerId;
    int intUserId;
    bool Is_Active;
    int Active;
    int intProgramId = 310;
    public string strDateFormat;
    string Flag = string.Empty;
    //decimal decAltFC;
    Dictionary<string, string> Procparam;
    string strPageName = "E Invoice Report";
    DataTable dtTable = new DataTable();
    public static Reports_S3G_Rpt_EInvoiceReport obj_Page;
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

    #region Page Load

    /// <summary>
    /// This event is handled for load the page
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            FunPriLoadPage();
            if (!IsPostBack)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Option", "3");
                ddlInvoice.BindDataTable("S3G_Rpt_Get_InvType", Procparam, new string[] { "Lookup_value", "Lookup_Name" });
                ddlInvoice.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load E Invoice Report";
            CVRepaymentSchedule.IsValid = false;
            throw ex;
        }
    }

    #endregion

    # region Page Methods
    /// <summary>
    /// This Method is called when page is Loding
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            Session["AccountingCurrency"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            // Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            Session["Date"] = 0;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            intUserId = ObjUserInfo.ProUserIdRW;

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

            CalendarExtender1.Format = strDateFormat;
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',false,  false);");
            CalendarExtender2.Format = strDateFormat;
            txtenddate.Attributes.Add("onblur", "fnDoDate(this,'" + txtenddate.ClientID + "','" + strDateFormat + "',false,  false);");

            if (!IsPostBack)
            {
                ucCustomPaging.Visible = false;
                ListItem liSelect = new ListItem("--All--", "0");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load E Invoice Report page");
        }
    }



    /// <summary>
    /// This Method is called after Clicking the Ok button.
    /// To Load the Repayment Details in Grid.
    /// </summary>
    /// <param name="PANum"></param>
    /// <param name="SANum"></param>
    private void FunPriLoadReport()
    {
        try
        {
            if (txtStartDate.Text == "" && txtenddate.Text == "" && ddlLesseeName.SelectedValue == "0" && ddlLocation.SelectedValue == "0" && ddlTrancheNo.SelectedValue == "0" && ddlRSNo.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Enter the From Date and To Date");
                txtStartDate.Focus();
                return;
            }

            if (txtStartDate.Text == "" && txtenddate.Text != "")
            {
                Utility.FunShowAlertMsg(this, "Enter the From Date");
                txtStartDate.Focus();
                return;
            }

            if (txtStartDate.Text != "" && txtenddate.Text == "")
            {
                Utility.FunShowAlertMsg(this, "Enter the To Date");
                txtenddate.Focus();
                return;
            }

            Procparam = new Dictionary<string, string>();
            if (txtStartDate.Text != "")
                Procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());
            if (txtenddate.Text != "")
                Procparam.Add("@Enddate", Utility.StringToDate(txtenddate.Text).ToString());

            //if (txtStartDate.Text != "")
            //    Procparam.Add("@StartDate", Utility.StringToDate("01/01/2020").ToString());
            //if (txtenddate.Text != "")
            //    Procparam.Add("@Enddate", Utility.StringToDate("01/01/2020").ToString());

            if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            else if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", "0");

            if (ddlLocation.SelectedValue != "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", ddlLocation.SelectedValue);
            else if (ddlLocation.SelectedValue == "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", "0");

            if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText != String.Empty)
                Procparam.Add("@Tranche_No", ddlTrancheNo.SelectedValue);
            else if (ddlTrancheNo.SelectedValue == "0" && ddlTrancheNo.SelectedText != String.Empty)
                Procparam.Add("@Tranche_No", "0");

            if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@RSNo", ddlRSNo.SelectedValue);
            else if (ddlRSNo.SelectedValue == "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@RSNo", "0");

            if (ddlInvoice.SelectedValue != "0")
                Procparam.Add("@Invoice_Type", ddlInvoice.SelectedValue);
            if (ddlStatus.SelectedValue != "0")
                Procparam.Add("@Invoice_Status", ddlStatus.SelectedValue);

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            bool bIsNewRow = false;
            grvGST.BindGridView("S3G_RPT_GetGSTReport_New", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            if (bIsNewRow)
            {
                grvGST.Rows[0].Visible = btnExportExcel.Visible = btnExport.Visible = false;
            }
            else
                btnExportExcel.Visible = btnExport.Visible = true;

            grvGST.Visible = ucCustomPaging.Visible = pnlVAT.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion

    #region Page Events


    #region Button ( Ok / Clear / Print)

    protected void btnOk_Click(object sender, EventArgs e)
    {
        FunPriLoadReport();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtStartDate.Text = String.Empty;
            ddlLesseeName.Clear();
            ddlLocation.Clear();
            ddlTrancheNo.Clear();
            ddlRSNo.Clear();
            ddlInvoice.SelectedValue = ddlStatus.SelectedValue = "0";
            txtenddate.Text = string.Empty;
            grvGST.DataSource = null;
            grvGST.DataBind();
            grvGST.Visible = ucCustomPaging.Visible = pnlVAT.Visible = btnExport.Visible = btnExportExcel.Visible = false;
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Error in Clear.";
            CVRepaymentSchedule.IsValid = false;
        }
    }

    protected void txtDateFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtenddate.Text = String.Empty;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
        }
    }


    protected void txtDateTo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CheckDate();
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
        }
    }

    private void CheckDate()
    {
        if (txtStartDate.Text != String.Empty && txtenddate.Text != String.Empty)
        {
            int intErrCount = 0;
            intErrCount = Utility.CompareDates(txtStartDate.Text, txtenddate.Text);
            if (intErrCount == -1)
            {
                txtenddate.Text = String.Empty;
                Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to From Date ");
                txtenddate.Focus();
                return;
            }
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtStartDate.Text == "" && txtenddate.Text == "" && ddlLesseeName.SelectedValue == "0" && ddlLocation.SelectedValue == "0" && ddlTrancheNo.SelectedValue == "0" && ddlRSNo.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Enter the From Date and To Date");
                txtStartDate.Focus();
                return;
            }

            if (txtStartDate.Text == "" && txtenddate.Text != "")
            {
                Utility.FunShowAlertMsg(this, "Enter the From Date");
                txtStartDate.Focus();
                return;
            }

            if (txtStartDate.Text != "" && txtenddate.Text == "")
            {
                Utility.FunShowAlertMsg(this, "Enter the To Date");
                txtenddate.Focus();
                return;
            }

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            if (txtStartDate.Text != "")
                Procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());
            if (txtenddate.Text != "")
                Procparam.Add("@Enddate", Utility.StringToDate(txtenddate.Text).ToString());

            if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            else if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", "0");

            if (ddlLocation.SelectedValue != "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", ddlLocation.SelectedValue);
            else if (ddlLocation.SelectedValue == "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", "0");

            if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText != String.Empty)
                Procparam.Add("@Tranche_No", ddlTrancheNo.SelectedValue);
            else if (ddlTrancheNo.SelectedValue == "0" && ddlTrancheNo.SelectedText != String.Empty)
                Procparam.Add("@Tranche_No", "0");

            if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@RSNo", ddlRSNo.SelectedValue);
            else if (ddlRSNo.SelectedValue == "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@RSNo", "0");

            if (ddlInvoice.SelectedValue != "0")
                Procparam.Add("@Invoice_Type", ddlInvoice.SelectedValue);
            if (ddlStatus.SelectedValue != "0")
                Procparam.Add("@Invoice_Status", ddlStatus.SelectedValue);

            Procparam.Add("@IsExport", "1");

            dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_RPT_GetGSTReport_New", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtTable;
            Grv.DataBind();
            //Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            //Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                //string attachment = "attachment; filename=GSTReport.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.xlsx";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(sw);

                //opc047 start to fix performance issue if recs are more than 1500
                // to fix request timeout issue
                //string csv = string.Empty;
                string filePath = Server.MapPath(".") + "\\CSV Files\\E-Invoice\\E_Invoice_Report_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".csv"; ;
                StreamWriter sw = new StreamWriter(filePath, false);
                //opc047 end
                foreach (DataColumn column in dtTable.Columns)
                {
                    //Add the Header row for CSV file.
                    //csv += column.ColumnName + ',';
                    sw.Write(column.ColumnName + ',');
                }

                //Add new line.
                //opc047 start
                //csv += "\r\n";
                sw.Write(sw.NewLine);
                //opc047 end

                foreach (DataRow row in dtTable.Rows)
                {
                    foreach (DataColumn column in dtTable.Columns)
                    {
                        //Add the Data rows.
                        //if (column.ColumnName.ToString() == "COMPANY_STATECODE" || column.ColumnName.ToString() == "LEDGER_CODE" || column.ColumnName.ToString() == "TRAN_CUST_STATE" || column.ColumnName.ToString() == "POS")
                        //    csv += "\t" + row[column.ColumnName].ToString().Replace(",", ";") + ',';
                        //else
                        //csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                        sw.Write(row[column.ColumnName].ToString().Replace(",", ";") + ',');
                    }

                    //Add new line.
                    //csv += "\r\n";
                    sw.Write(sw.NewLine);
              }
                sw.Close();
                //Download the CSV file.
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=E_Invoice_Report.csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                //Response.Output.Write(csv.ToString());
                Response.WriteFile(filePath);
                Response.Flush();
                Response.End();

                //Grv.RenderControl(htw);
                //Response.Write(sw.ToString());
                //Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtStartDate.Text == "" && txtenddate.Text == "" && ddlLesseeName.SelectedValue == "0" && ddlLocation.SelectedValue == "0" && ddlTrancheNo.SelectedValue == "0" && ddlRSNo.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Enter the From Date and To Date");
                txtStartDate.Focus();
                return;
            }

            if (txtStartDate.Text == "" && txtenddate.Text != "")
            {
                Utility.FunShowAlertMsg(this, "Enter the From Date");
                txtStartDate.Focus();
                return;
            }

            if (txtStartDate.Text != "" && txtenddate.Text == "")
            {
                Utility.FunShowAlertMsg(this, "Enter the To Date");
                txtenddate.Focus();
                return;
            }

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            if (txtStartDate.Text != "")
                Procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());
            if (txtenddate.Text != "")
                Procparam.Add("@Enddate", Utility.StringToDate(txtenddate.Text).ToString());

            if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            else if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", "0");

            if (ddlLocation.SelectedValue != "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", ddlLocation.SelectedValue);
            else if (ddlLocation.SelectedValue == "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", "0");

            if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText != String.Empty)
                Procparam.Add("@Tranche_No", ddlTrancheNo.SelectedValue);
            else if (ddlTrancheNo.SelectedValue == "0" && ddlTrancheNo.SelectedText != String.Empty)
                Procparam.Add("@Tranche_No", "0");

            if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@RSNo", ddlRSNo.SelectedValue);
            else if (ddlRSNo.SelectedValue == "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@RSNo", "0");

            if (ddlInvoice.SelectedValue != "0")
                Procparam.Add("@Invoice_Type", ddlInvoice.SelectedValue);
            if (ddlStatus.SelectedValue != "0")
                Procparam.Add("@Invoice_Status", ddlStatus.SelectedValue);

            Procparam.Add("@IsExport", "1");

            dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_RPT_GetGSTReport_New", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtTable;
            Grv.DataBind();
            //Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            //Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=E Invoice Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

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

    public override void VerifyRenderingInServerForm(Control control)
    {

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



    protected void grvGST_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
        {


            for (int i_cellVal = 1; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                try
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
                catch (Exception ex)
                {
                    //continue;
                }
            }




        }
        //if (ProgramCodeToCompare == strOperatingLeaseExpenses)
        //{ e.Row.Cells[1].Visible = false; }
        //else
        //{ e.Row.Cells[1].Visible = true; }


    }

    #endregion

    [System.Web.Services.WebMethod]
    public static string[] GetLesseeNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceNo(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        dictparam.Add("@Option", "13");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetOldInvoiceNo(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        dictparam.Add("@Option", "16");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "318");
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheNo(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@Program_ID", "321");
        if (obj_Page.ddlLesseeName.SelectedValue != "0" && obj_Page.ddlLesseeName.SelectedText != String.Empty)
            dictparam.Add("@Customer_ID", obj_Page.ddlLesseeName.SelectedValue.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetTrancheDetails", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetRSNo(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@Program_ID", "321");
        if (obj_Page.ddlLesseeName.SelectedValue != "0" && obj_Page.ddlLesseeName.SelectedText != String.Empty)
            dictparam.Add("@Cust_ID", obj_Page.ddlLesseeName.SelectedValue.ToString());
        if (obj_Page.ddlTrancheNo.SelectedValue != "0" && obj_Page.ddlTrancheNo.SelectedText != String.Empty)
            dictparam.Add("@tranche_id", obj_Page.ddlTrancheNo.SelectedValue.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_WBT_GetRSNO", dictparam));
        return suggetions.ToArray();
    }

    #endregion
}