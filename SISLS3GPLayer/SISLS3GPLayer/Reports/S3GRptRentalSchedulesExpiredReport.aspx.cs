#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Rental Schedules Expired Report
/// Created By          :   Chandru K
/// Created Date        :   30-Jan-2015
/// Purpose             :   To Get the Rental Schedules Expired.
/// <Program Summary>
#endregion

#region Namespaces
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
#endregion


public partial class Reports_S3GRptRentalSchedulesExpiredReport : ApplyThemeForProject
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
    string strPageName = "Rental Schedules Expired Report";
    DataTable dtTable = new DataTable();
    public static Reports_S3GRptRentalSchedulesExpiredReport obj_Page;
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
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Repayment Schedule Page.";
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
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

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
            CalendarExtender2.Format = strDateFormat;
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',false,  false);"); ;
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',false,  false);"); ;

            if (!IsPostBack)
            {
                FunPriLoadStatus();
                ucCustomPaging.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Repayment Schedule page");
        }
    }

    private void FunPriLoadStatus()
    {
        Dictionary<string, string> dictassetrgstr = new Dictionary<string, string>();
        try
        {
            dictassetrgstr.Add("@Company_ID", intCompanyId.ToString());
            dictassetrgstr.Add("@User_ID", intUserId.ToString());
            dictassetrgstr.Add("@Rpt_Name", "RSExpired");
            ddlAssetStatus.BindMemoDataTable("S3G_RPT_GetAssetRgstrRptStatus", dictassetrgstr, new string[] { "Value", "Name" });
        }
        catch (Exception ex)
        {
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
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@StratDate", Utility.StringToDate(txtStartDate.Text).ToString());
            Procparam.Add("@EndDate", Utility.StringToDate(txtEndDate.Text).ToString());
            if (ddlLesseeName.SelectedValue != "0")
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            if (ddlClosureStatus.SelectedValue != "0")
                Procparam.Add("@Closure_Status", ddlClosureStatus.SelectedValue);
            if (ddlAssetStatus.SelectedValue != "0")
                Procparam.Add("@Asset_Status", ddlAssetStatus.SelectedValue);

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            bool bIsNewRow = false;
            grvRSExpired.BindGridView("S3G_RPT_GetRentalSchedulesExpired", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvRSExpired.Rows[0].Visible = btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            grvRSExpired.Visible = ucCustomPaging.Visible =  true;
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

    protected void grvRSExpired_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Just set the Column Name that you wish to hide based on your requirements
        foreach (TableCell cell in e.Row.Cells)
        {
            BoundField field = (BoundField)((DataControlFieldCell)cell).ContainingField;
            if (field.DataField == "PA_SA_REF_ID")
            {
                field.Visible = false;
            }
        }
    }

    protected void grvRSExpired_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i_cellVal = 1; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                try
                {
                    
                    if (!string.IsNullOrEmpty(e.Row.Cells[i_cellVal].Text))
                    {
                        Int32 type = 0;    

                        type = FunPriTypeCast(e.Row.Cells[i_cellVal].Text);

                      
                        switch (type)
                        {
                            case 1: 
                                e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Right;
                                break;
                            case 3: 
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

    #region Button ( Ok / Clear / Print)

    protected void btnOk_Click(object sender, EventArgs e)
    {
        int intErrCount = 0;
        if (txtStartDate.Text != String.Empty && txtEndDate.Text != String.Empty)
        {
            intErrCount = Utility.CompareDates(txtStartDate.Text, txtEndDate.Text);
            if (intErrCount == -1)
            {
                txtEndDate.Text = String.Empty;
                Utility.FunShowAlertMsg(this, "Expiry Date Range To Should Be Greater Than Expiry Date Range From");
                txtEndDate.Focus();
                return;
            }
        }

        try
        {
            FunPriLoadReport();
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Rental Schedule Expired Report Details Grid.";
            CVRepaymentSchedule.IsValid = false;
        }
    }

    /// <summary>
    /// To clear the fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtStartDate.Text = txtEndDate.Text = String.Empty;
            ddlLesseeName.Clear();
            ddlAssetStatus.SelectedIndex = ddlClosureStatus.SelectedIndex = 0;
            grvRSExpired.DataSource = null;
            grvRSExpired.DataBind();
            grvRSExpired.Visible = ucCustomPaging.Visible =  btnExport.Visible = false;
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Error in Clear.";
            CVRepaymentSchedule.IsValid = false;
        }
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_Id", intCompanyId.ToString());
        Procparam.Add("@StratDate", Utility.StringToDate(txtStartDate.Text).ToString());
        Procparam.Add("@EndDate", Utility.StringToDate(txtEndDate.Text).ToString());
        if (ddlLesseeName.SelectedValue != "0")
            Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
        if (ddlClosureStatus.SelectedValue != "0")
            Procparam.Add("@Closure_Status", ddlClosureStatus.SelectedValue);
        if (ddlAssetStatus.SelectedValue != "0")
            Procparam.Add("@Asset_Status", ddlAssetStatus.SelectedValue);
        Procparam.Add("@IsExport", "1");

        dtTable = new DataTable();
        dtTable = Utility.GetDefaultData("S3G_RPT_GetRentalSchedulesExpired", Procparam);
        dtTable.Columns.Remove("PA_SA_REF_ID");
        dtTable.Columns["Lessee’s end user Name"].ColumnName = "Lessee's end user Name";
        dtTable.AcceptChanges();

        GridView Grv = new GridView();

        Grv.DataSource = dtTable;
        Grv.DataBind();
        Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
        Grv.ForeColor = System.Drawing.Color.DarkBlue;

        if (Grv.Rows.Count > 0)
        {
            string attachment = "attachment; filename=RentalSchedulesExpired.xls";
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
            row["Column1"] = "Rental Schedules Expired Report";
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();
            row["Column1"] = " Expiry Date Range From " + txtStartDate.Text;
            row["Column2"] = " Expiry Date Range To " + txtEndDate.Text;
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();            

            if (ddlLesseeName.SelectedValue == "0")
                row["Column1"] = "Lessee Name : All";
            else
                row["Column1"] = "Lessee Name : " + ddlLesseeName.SelectedText;

            if (ddlClosureStatus.SelectedValue == "0")
                row["Column2"] = "Closure Status : All";
            else
                row["Column2"] = " Closure Status : " + ddlClosureStatus.SelectedItem.Text;
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();
            if (ddlAssetStatus.SelectedValue == "0")
                row["Column1"] = " Asset Status : All";
            else
                row["Column1"] = " Asset Status  : " + ddlAssetStatus.SelectedItem.Text;            
            dtHeader.Rows.Add(row);            

            row = dtHeader.NewRow();
            dtHeader.Rows.Add(row);
            grv1.DataSource = dtHeader;
            grv1.DataBind();

            grv1.HeaderRow.Visible = false;
            grv1.GridLines = GridLines.None;

            grv1.Rows[0].Cells[0].ColumnSpan = 18;
            grv1.Rows[1].Cells[0].ColumnSpan = 18;
            grv1.Rows[2].Cells[0].ColumnSpan = 5;
            grv1.Rows[2].Cells[1].ColumnSpan = 5;
            grv1.Rows[3].Cells[0].ColumnSpan = 5;
            grv1.Rows[3].Cells[1].ColumnSpan = 5;
            grv1.Rows[4].Cells[0].ColumnSpan = 5;

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

    public override void VerifyRenderingInServerForm(Control control)
    {

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

    #endregion
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        if (txtEndDate.Text != "")
        {
            if ((Utility.StringToDate(txtEndDate.Text)) >= DateTime.UtcNow.Date)
            {
                Utility.FunShowAlertMsg(this, "Expiry to date should be less than current date");
                txtEndDate.Clear();
                txtEndDate.Focus();
                return;
            }
        }
    }
}
