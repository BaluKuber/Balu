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

public partial class Reports_S3G_Rpt_EOTReport : ApplyThemeForProject
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
    string strPageName = "GST Report";
    DataTable dtTable = new DataTable();
    public static Reports_S3G_Rpt_EOTReport obj_Page;
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

            }
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load GST Report";
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
            throw new ApplicationException("Unable to Load GST Report page");
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

            if (txtStartDate.Text == "" && txtenddate.Text == "" && ddlLesseeName.SelectedValue == "0")
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
                Procparam.Add("@EndDate", Utility.StringToDate(txtenddate.Text).ToString());

            if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            else if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", "0");

            //if (ddlFunderName.SelectedValue != "0" && ddlFunderName.SelectedText != String.Empty)
            //    Procparam.Add("@Funder_Id", ddlFunderName.SelectedValue);
            //else if (ddlFunderName.SelectedValue == "0" && ddlFunderName.SelectedText != String.Empty)
            //    Procparam.Add("@Funder_Id", "0");

            Procparam.Add("@Type", ddlType.SelectedValue);

            Procparam.Add("@Status", ddlStatus.SelectedValue);

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            bool bIsNewRow = false;
            grvGST.BindGridView("S3G_RPT_GetEOTReport", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            if (bIsNewRow)
            {
                grvGST.Rows[0].Visible = btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

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
            txtStartDate.Text = txtenddate.Text = String.Empty;
            ddlLesseeName.Clear();
            //ddlFunderName.Clear();
            ddlType.SelectedValue = ddlStatus.SelectedValue = "0";
            grvGST.DataSource = null;
            grvGST.DataBind();
            grvGST.Visible = ucCustomPaging.Visible = pnlVAT.Visible = btnExport.Visible = false;
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Error in Clear.";
            CVRepaymentSchedule.IsValid = false;
        }
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtStartDate.Text == "" && txtenddate.Text == "" && ddlLesseeName.SelectedValue == "0")
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
                Procparam.Add("@EndDate", Utility.StringToDate(txtenddate.Text).ToString());

            if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            else if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", "0");

            //if (ddlFunderName.SelectedValue != "0" && ddlFunderName.SelectedText != String.Empty)
            //    Procparam.Add("@Funder_Id", ddlFunderName.SelectedValue);
            //else if (ddlFunderName.SelectedValue == "0" && ddlFunderName.SelectedText != String.Empty)
            //    Procparam.Add("@Funder_Id", "0");

            Procparam.Add("@Type", ddlType.SelectedValue);

            Procparam.Add("@Status", ddlStatus.SelectedValue);

            Procparam.Add("@IsExport", "1");

            dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_RPT_GetEOTReport", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtTable;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=EOTReport.xls";
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
                if (txtStartDate.Text == "" && txtenddate.Text == "")
                    row["Column1"] = "EOT Report";
                else
                    row["Column1"] = "EOT Report For the Period " + txtStartDate.Text + " to " + txtenddate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != String.Empty)
                    row["Column1"] = "Lessee Name: " + ddlLesseeName.SelectedText;
                else
                    row["Column1"] = "Lessee Name: --All--";

                if (ddlType.SelectedValue != "0")
                    row["Column2"] = "Type: " + ddlType.SelectedItem.Text;
                else
                    row["Column2"] = "Type: --All--";

                dtHeader.Rows.Add(row);

                //row = dtHeader.NewRow();

                //if (ddlFunderName.SelectedValue != "0" && ddlFunderName.SelectedText != String.Empty)
                //    row["Column1"] = "Funder Name:" + ddlFunderName.SelectedText;
                //else
                //    row["Column1"] = "Funder Name: --All--";

                //row["Column2"] = "";

                //dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 9;
                grv1.Rows[1].Cells[0].ColumnSpan = 9;
                grv1.Rows[2].Cells[0].ColumnSpan = 9;
                //grv1.Rows[3].Cells[0].ColumnSpan = 9;
               
                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[2].HorizontalAlign = HorizontalAlign.Left;
                //grv1.Rows[3].HorizontalAlign = HorizontalAlign.Left;

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

    //[System.Web.Services.WebMethod]
    //public static string[] GetVendors(String prefixText, int count)
    //{
    //    Dictionary<string, string> Procparam;
    //    Procparam = new Dictionary<string, string>();
    //    List<String> suggetions = new List<String>();
    //    Procparam.Clear();
    //    Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
    //    Procparam.Add("@OPTION", "4");
    //    Procparam.Add("@Prefix", prefixText);
    //    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));

    //    return suggetions.ToArray();
    //}

    #endregion
}