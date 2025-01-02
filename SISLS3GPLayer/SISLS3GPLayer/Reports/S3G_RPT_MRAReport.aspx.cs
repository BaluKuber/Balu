/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   MRA Report
/// Created By          :   Sampath B
/// Created Date        :   06-Jan-2015
/// Purpose             :   This is the Report screen for searching Master Rental Agreement Details
/// Last Updated By		:   
/// Last Updated Date   :   
/// <Program Summary>

using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.IO;
using System.Configuration;
using S3GBusEntity.Collection;
using System.Globalization;
using System.Web.Services;
using System.Text;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ServiceProcess;
using System.Diagnostics;
using Microsoft.Win32;
using System.Drawing.Printing;

public partial class Reports_Report_S3G_RPT_MRAReport : ApplyThemeForProject
{
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    PagingValues ObjPaging = new PagingValues();
    string strDateFormat = string.Empty;
    public static Reports_Report_S3G_RPT_MRAReport obj_Page;
    int intUserId;
    int intCompanyId;
    DataTable dtGetdata = new DataTable();
    DataTable dtGriddata = new DataTable();

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
        try
        {
            ProPageNumRW = intPageNum;
            ProPageSizeRW = intPageSize;
            BindGridMRAReportDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        try
        {
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtenderMRACreationFrom.Format = strDateFormat;
            CalendarExtenderMRACreationTo.Format = strDateFormat;
            CalendarExtenderMRADateFrom.Format = strDateFormat;
            CalendarExtenderMRADateTo.Format = strDateFormat;
            ProPageNumRW = 1;
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
            {
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            }
            else
            {
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));
            }
            PageAssignValue Obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = Obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            if (!IsPostBack)
            {
                txtMRACreationFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtMRACreationFrom.ClientID + "','" + strDateFormat + "',true,  false);");
                txtMRACreationTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtMRACreationTo.ClientID + "','" + strDateFormat + "',true,  false);");
                txtMRADateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtMRADateFrom.ClientID + "','" + strDateFormat + "',true,  false);");
                txttxtMRADateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txttxtMRADateTo.ClientID + "','" + strDateFormat + "',true,  false);");
                ucCustomPaging.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    public void BindGridMRAReportDetails()
    {
        try
        {
            int intTotalRecords = 0;
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            ObjPaging.ProTotalRecords = intTotalRecords;
            bool blnIsNewRow = false;

            if (txtMRACreationFrom.Text.Trim() != string.Empty)
            {
                Procparam.Add("@MRACreationFromDate", Convert.ToDateTime(Utility.StringToDate(txtMRACreationFrom.Text)).ToString("yyyy/MM/dd"));
            }
            if (txtMRACreationTo.Text.Trim() != string.Empty)
            {
                Procparam.Add("@MRACreationToDate", Convert.ToDateTime(Utility.StringToDate(txtMRACreationTo.Text)).ToString("yyyy/MM/dd"));
            }
            if (txtMRADateFrom.Text.Trim() != string.Empty)
            {
                Procparam.Add("@MRADateFrom", Convert.ToDateTime(Utility.StringToDate(txtMRADateFrom.Text)).ToString("yyyy/MM/dd"));
            }
            if (txttxtMRADateTo.Text.Trim() != string.Empty)
            {
                Procparam.Add("@MRADateTo", Convert.ToDateTime(Utility.StringToDate(txttxtMRADateTo.Text)).ToString("yyyy/MM/dd"));
            }
            if (ddlCustName.SelectedText.Trim() != "")
            {
                Procparam.Add("@CustomerId", ddlCustName.SelectedValue);
            }
            if (ddlMRAStatus.SelectedIndex > 0)
            {
                Procparam.Add("@MRAStatusId", ddlMRAStatus.SelectedValue);
            }

            PnlMRADetails.Visible = true;
            grvMRAReport.Visible = true;

            dtGriddata = Utility.GetGridData("[S3G_RPT_GetMRAReportDetails]", Procparam, out intTotalRecords, ObjPaging);
          //  grvMRAReport.BindGridView("[S3G_RPT_GetMRAReportDetails]", Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);

            if (dtGriddata.Rows.Count > 0)
            {
                grvMRAReport.DataSource = dtGriddata;
                grvMRAReport.DataBind();
                ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
                ucCustomPaging.setPageSize(ProPageSizeRW);
                ucCustomPaging.Visible = true;
                btnPrint.Visible = true;
            }
            else
            {
               // PnlMRADetails.Visible = true;
                grvMRAReport.DataSource = null;
                grvMRAReport.DataBind();
                grvMRAReport.EmptyDataText = "No Records Found";
                ucCustomPaging.Visible = false;
                btnPrint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtMRACreationFrom.Text == string.Empty && txtMRACreationTo.Text == string.Empty && txtMRADateFrom.Text == string.Empty
                && txttxtMRADateTo.Text == string.Empty)
            {
                Utility.FunShowAlertMsg(this, "Select Creation date range (or) MRA date range");
                PnlMRADetails.Visible = false;
                ucCustomPaging.Visible = false;
                btnPrint.Visible = false;
                return;
            }

            if ((!string.IsNullOrEmpty(txtMRACreationFrom.Text)) && (!string.IsNullOrEmpty(txtMRACreationTo.Text)))
            {
                if ((Utility.StringToDate(txtMRACreationFrom.Text)) > (Utility.StringToDate(txtMRACreationTo.Text)))
                {
                    if (hidDate.Value.ToUpper() == "STARTDATE")
                    {
                        Utility.FunShowAlertMsg(this, "Creation From date should be lesser than (or) equal to the Creation To Date");
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Creation To date should be greater than (or) equal to the Creation From Date");
                    }
                    PnlMRADetails.Visible = false;
                    ucCustomPaging.Visible = false;
                    btnPrint.Visible = false;
                    return;
                }
            }
            if ((!string.IsNullOrEmpty(txtMRADateFrom.Text)) && (!string.IsNullOrEmpty(txttxtMRADateTo.Text)))
            {
                if ((Utility.StringToDate(txtMRADateFrom.Text)) > (Utility.StringToDate(txttxtMRADateTo.Text)))
                {
                    if (hidMRADate.Value.ToUpper() == "STARTDATE")
                    {
                        Utility.FunShowAlertMsg(this, "MRA From date should be lesser than (or) equal to the MRA To Date");
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "MRA To date should be greater than (or) equal to the MRA From Date");
                    }
                    PnlMRADetails.Visible = false;
                    ucCustomPaging.Visible = false;
                    btnPrint.Visible = false;
                    return;
                }
            }

            BindGridMRAReportDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
  
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            if (txtMRACreationFrom.Text.Trim() != string.Empty)
            {
                Procparam.Add("@MRACreationFromDate", Convert.ToDateTime(Utility.StringToDate(txtMRACreationFrom.Text)).ToString("yyyy/MM/dd"));
            }
            if (txtMRACreationTo.Text.Trim() != string.Empty)
            {
                Procparam.Add("@MRACreationToDate", Convert.ToDateTime(Utility.StringToDate(txtMRACreationTo.Text)).ToString("yyyy/MM/dd"));
            }
            if (txtMRADateFrom.Text.Trim() != string.Empty)
            {
                Procparam.Add("@MRADateFrom", Convert.ToDateTime(Utility.StringToDate(txtMRADateFrom.Text)).ToString("yyyy/MM/dd"));
            }
            if (txttxtMRADateTo.Text.Trim() != string.Empty)
            {
                Procparam.Add("@MRADateTo", Convert.ToDateTime(Utility.StringToDate(txttxtMRADateTo.Text)).ToString("yyyy/MM/dd"));
            }
            if (ddlCustName.SelectedText.Trim() != "")
            {
                Procparam.Add("@CustomerId", ddlCustName.SelectedValue);
            }
            if (ddlMRAStatus.SelectedIndex > 0)
            {
                Procparam.Add("@MRAStatusId", ddlMRAStatus.SelectedValue);
            }

            Procparam.Add("@IsExport", "1");
            dtGetdata = Utility.GetDefaultData("[S3G_RPT_GetMRAReportDetails]", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=MRA_Report.xls";
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
                row["Column1"] = "MRA Report";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtMRACreationFrom.Text==string.Empty)
                    row["Column1"] = "Creation From Date : ";
                else
                    row["Column1"] = "Creation From Date : " + txtMRACreationFrom.Text;

                if (txtMRACreationTo.Text == string.Empty)
                    row["Column2"] = "Creation To Date : ";
                else
                    row["Column2"] = "Creation To Date : " + txtMRACreationTo.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtMRADateFrom.Text == string.Empty)
                    row["Column1"] = "MRA From Date : ";
                else
                    row["Column1"] = "MRA From Date : " + txtMRADateFrom.Text;

                if (txttxtMRADateTo.Text == string.Empty)
                    row["Column2"] = "MRA To Date : ";
                else
                    row["Column2"] = "MRA To Date : " + txttxtMRADateTo.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlCustName.SelectedValue == "0")
                    row["Column1"] = "Lessee Name : --All--";
                else
                    row["Column1"] = "Lessee Name : " + ddlCustName.SelectedText;

                if (ddlMRAStatus.SelectedValue == "0")
                    row["Column2"] = "MRA Status : --All--";
                else
                    row["Column2"] = "MRA Status : " + ddlMRAStatus.SelectedItem.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
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
 

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerName(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            DataTable dtCommon = new DataTable();
            DataSet Ds = new DataSet();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCustomers", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        try
        {
            txtMRACreationFrom.Text = string.Empty;
            txtMRACreationTo.Text = string.Empty;
            txtMRADateFrom.Text = string.Empty;
            txttxtMRADateTo.Text = string.Empty;
            ddlCustName.SelectedValue = string.Empty;
            ddlCustName.SelectedText = string.Empty;
            ddlMRAStatus.SelectedIndex = 0;
            grvMRAReport.Visible = false;
            ucCustomPaging.Visible = false;
            btnPrint.Visible = false;
            PnlMRADetails.Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void txtMRADateFrom_TextChanged(object sender, EventArgs e)
    {
        hidMRADate.Value = "STARTDATE";
    }
    protected void txttxtMRADateTo_TextChanged(object sender, EventArgs e)
    {
        hidMRADate.Value = "ENDDATE";
    }
    protected void txtMRACreationFrom_TextChanged(object sender, EventArgs e)
    {
        hidDate.Value = "STARTDATE";
    }
    protected void txtMRACreationTo_TextChanged(object sender, EventArgs e)
    {
        hidDate.Value = "ENDDATE";
    }
}