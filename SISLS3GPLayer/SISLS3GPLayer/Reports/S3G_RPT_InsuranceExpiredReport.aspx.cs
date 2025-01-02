/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Asset Insurance Status Report
/// Created By          :   Swarna S
/// Created Date        :   14-Sep-2015
/// Purpose             :   This is the Report screen for searching Insurance Expired Details
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


public partial class Reports_S3G_RPT_InsuranceExpiredReport : ApplyThemeForProject
{
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    PagingValues ObjPaging = new PagingValues();
    public static Reports_S3G_RPT_InsuranceExpiredReport obj_Page;
    string strDateFormat = string.Empty;
    DataTable dtGetdata = new DataTable();
    int intUserId;
    int intCompanyId;

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
            BindAssetInsuranceDetailsReport();
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
            CalendarExtenderDateFrom.Format = strDateFormat;
            CalendarExtenderDateTo.Format = strDateFormat;
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
            intUserId = ObjUserInfo.ProUserIdRW;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;

            if (!IsPostBack)
            {                
                //divAssetInsuranceStatus.Visible = false;
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
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCustomers", Procparam));
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetFunderName(String prefixText, int count)
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
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETFunders", Procparam));
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if ((string.IsNullOrEmpty(txtDateFrom.Text)) && (string.IsNullOrEmpty(txtDateTo.Text)))
            {
                Utility.FunShowAlertMsg(this, "Select date range");
                return;
            }

            if ((!string.IsNullOrEmpty(txtDateFrom.Text)) && (!string.IsNullOrEmpty(txtDateTo.Text)))
            {
                if ((Utility.StringToDate(txtDateFrom.Text)) > (Utility.StringToDate(txtDateTo.Text)))
                {
                    if (hidDate.Value.ToUpper() == "STARTDATE")
                    {
                        Utility.FunShowAlertMsg(this, "Expiry From Date should be lesser than the Expiry To Date");
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Expiry To Date should be greater than the Expiry From Date");
                    }
                    return;
                }
            }

            BindAssetInsuranceDetailsReport();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public void BindAssetInsuranceDetailsReport()
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

            if (txtDateFrom.Text.Trim() != string.Empty)
            {
                Procparam.Add("@DateFrom", Utility.StringToDate(txtDateFrom.Text).ToString("yyyy/MM/dd"));
            }
            if (txtDateTo.Text.Trim() != string.Empty)
            {
                Procparam.Add("@DateTo", Utility.StringToDate(txtDateTo.Text).ToString("yyyy/MM/dd"));
            }
            if (ddlCustName.SelectedText.Trim() != "")
            {
                Procparam.Add("@CustomerId", ddlCustName.SelectedValue);
            }
            if (ddlFunderName.SelectedText.Trim() != "")
            {
                Procparam.Add("@FunderId", ddlFunderName.SelectedValue);
            }

            //Procparam.Add("@StatusId", ddlStatus.SelectedValue);

            // divAssetInsuranceStatus.Visible = true;
            PnlAssetInsuranceStatus.Visible = true;
            grvAssetInsuranceStatusReport.Visible = true;
            grvAssetInsuranceStatusReport.BindGridView("[S3G_RPT_InsuranceExpiredReport]", Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            ucCustomPaging.Visible = true;
            btnPrint.Visible = true;

            if (blnIsNewRow)
            {
                grvAssetInsuranceStatusReport.Rows[0].Visible = false;
/*added by vinodha m on sep 25,2015 - to disable export button when grid have no records*/                
                btnPrint.Visible = false;
/*added by vinodha m on sep 25,2015 - to disable export button when grid have no records*/                
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        try
        {
            txtDateFrom.Text = string.Empty;
            txtDateTo.Text = string.Empty;
            ddlCustName.SelectedValue = "0";
            ddlCustName.SelectedText = string.Empty;
            ddlFunderName.SelectedValue = "0";
            ddlFunderName.SelectedText = string.Empty;
            //ddlStatus.SelectedValue = "0";
            //divAssetInsuranceStatus.Visible = false;
            grvAssetInsuranceStatusReport.Visible = false;
            ucCustomPaging.Visible = false;
            btnPrint.Visible = false;
            PnlAssetInsuranceStatus.Visible = false;
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
            ExcelExport();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public void ExcelExport()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", "1");

            if (txtDateFrom.Text.Trim() != string.Empty)
            {
                Procparam.Add("@DateFrom", Utility.StringToDate(txtDateFrom.Text).ToString("yyyy/MM/dd"));
            }
            if (txtDateTo.Text.Trim() != string.Empty)
            {
                Procparam.Add("@DateTo", Utility.StringToDate(txtDateTo.Text).ToString("yyyy/MM/dd"));
            }
            if (ddlCustName.SelectedText.Trim() != "")
            {
                Procparam.Add("@CustomerId", ddlCustName.SelectedValue);
            }

/*added by vinodha m on sep 25,2015 - missed input parameter*/            
            if (ddlFunderName.SelectedText.Trim() != "")
            {
                Procparam.Add("@FunderId", ddlFunderName.SelectedValue);
            }
/*added by vinodha m on sep 25,2015 - missed input parameter*/            

            Procparam.Add("@IsExport", "1");
            dtGetdata = Utility.GetDefaultData("[S3G_RPT_InsuranceExpiredReport]", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtGetdata;
            Grv.DataBind();


            if (Grv.Rows.Count > 0)
            {
                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;

                string attachment = "attachment; filename=InsuranceExpiry_Report.xls";
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
                row["Column1"] = "Insurance Expiry Report";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtDateFrom.Text == string.Empty)
                    row["Column1"] = "Expiry From Date : ";
                else
                    row["Column1"] = "Expiry From Date : " + txtDateFrom.Text;

                if (txtDateTo.Text == string.Empty)
                    row["Column2"] = "Expiry To Date : ";
                else
                    row["Column2"] = "Expiry To Date : " + txtDateTo.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlCustName.SelectedValue == "0")
                    row["Column1"] = "Lessee Name : All";
                else
                    row["Column1"] = "Lessee Name : " + ddlCustName.SelectedText;

                if (ddlFunderName.SelectedValue == "0")
                    row["Column2"] = "Funder Name : All";
                else
                    row["Column2"] = "Funder Name : " + ddlFunderName.SelectedText;
                dtHeader.Rows.Add(row);

                //row = dtHeader.NewRow();
                //if (ddlStatus.SelectedValue == "0")
                //    row["Column1"] = "Status : All";
                //else
                //    row["Column1"] = "Status : " + ddlStatus.SelectedItem.Text;
                //dtHeader.Rows.Add(row);

                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 10;
                grv1.Rows[1].Cells[0].ColumnSpan = 10;
                //grv1.Rows[2].Cells[0].ColumnSpan = 4;
                //grv1.Rows[3].Cells[0].ColumnSpan = 4;
                //grv1.Rows[4].Cells[0].ColumnSpan = 4;


                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                //grv1.Rows[2].HorizontalAlign = HorizontalAlign.Left;
                //grv1.Rows[3].HorizontalAlign = HorizontalAlign.Left;
                //grv1.Rows[4].HorizontalAlign = HorizontalAlign.Left;


                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;
                grv1.RenderControl(htw);
                Grv.RenderControl(htw);

                ddlFunderName.SelectedValue = "0";
                ddlCustName.SelectedValue = "0";
                //ddlStatus.SelectedValue = "0";

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
        hidDate.Value = "STARTDATE";
    }
    protected void txtDateTo_TextChanged(object sender, EventArgs e)
    {
        hidDate.Value = "ENDDATE";
    }
}