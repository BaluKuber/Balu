/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   CST Purchase Details Report
/// Created By          :   Sampath B
/// Created Date        :   16-Feb-2015
/// Purpose             :   This is the Report screen for searching CST Purchase Details
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

public partial class Reports_S3G_RPT_CSTPurchaseDetailsReport : ApplyThemeForProject
{
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    PagingValues ObjPaging = new PagingValues();
    string strDateFormat = string.Empty;
    DataTable dtGetdata = new DataTable();
    public static Reports_S3G_RPT_CSTPurchaseDetailsReport obj_Page;
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
            BindCSTPurchaseDetailsReport();
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
            CalendarExtenderPostingDateFrom.Format = strDateFormat;
            CalendarExtenderPostingDateTo.Format = strDateFormat;
            CalendarExtenderInvoiceDateFrom.Format = strDateFormat;
            CalendarExtenderInvoiceDateTo.Format = strDateFormat;
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
                txtPostingDateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtPostingDateFrom.ClientID + "','" + strDateFormat + "',true,  false);");
                txtPostingDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtPostingDateTo.ClientID + "','" + strDateFormat + "',true,  false);");
                txtInvoiceDateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoiceDateFrom.ClientID + "','" + strDateFormat + "',true,  false);");
                txtInvoiceDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoiceDateTo.ClientID + "','" + strDateFormat + "',true,  false);");
                //   divCFormSalesDetails.Visible = false;
                ucCustomPaging.Visible = false;
                //ddlCFormNumber.Enabled = false;
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
    public static string[] GetLesseeName(String prefixText, int count)
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

    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceNo(String prefixText, int count)
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
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("[S3G_GET_SaleInvoiceNo]", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCFormNo(String prefixText, int count)
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
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("[S3G_GET_CFORM_NOS]", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetStateDetails(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> dictparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            dictparam.Clear();
            dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
            dictparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetStateName", dictparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }
    [System.Web.Services.WebMethod]
    public static string[] GetVendorName(String prefixText, int count)
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
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("[S3G_GETVENDORS]", Procparam), true);
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
            if ((txtPostingDateFrom.Text == string.Empty) && (txtPostingDateTo.Text == string.Empty)
                && (txtInvoiceDateFrom.Text == string.Empty) && (txtInvoiceDateTo.Text == string.Empty))
            {
                Utility.FunShowAlertMsg(this, "Select Posting date range (or) Invoice date range");
                return;
            }

            if ((txtPostingDateFrom.Text != string.Empty) && (txtPostingDateTo.Text != string.Empty))
            {
                if (Utility.StringToDate(txtPostingDateFrom.Text) > Utility.StringToDate(txtPostingDateTo.Text))
                {
                    Utility.FunShowAlertMsg(this, "Posting From date cannot be greater than To date");
                    return;
                }  
            }

            if (((txtPostingDateFrom.Text != string.Empty) && (txtPostingDateTo.Text == string.Empty)) || ((txtPostingDateFrom.Text == string.Empty) && (txtPostingDateTo.Text != string.Empty)))
            {
                Utility.FunShowAlertMsg(this, "Select a valid posting date range");
                return;
            }


            if (((txtInvoiceDateFrom.Text != string.Empty) && (txtInvoiceDateTo.Text == string.Empty)) || ((txtInvoiceDateFrom.Text == string.Empty) && (txtInvoiceDateTo.Text != string.Empty)))
            {
                Utility.FunShowAlertMsg(this, "Select a valid invoice date range");
                return;
            }

            if ((txtInvoiceDateFrom.Text != string.Empty) && (txtInvoiceDateTo.Text != string.Empty))
            {
                if (Utility.StringToDate(txtInvoiceDateFrom.Text) > Utility.StringToDate(txtInvoiceDateTo.Text))
                {
                    Utility.FunShowAlertMsg(this, "Invoice From date cannot be greater than To date");
                    return;
                }

            }

            BindCSTPurchaseDetailsReport();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    public void BindCSTPurchaseDetailsReport()
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

            if (txtPostingDateFrom.Text != string.Empty)
            {
                Procparam.Add("@PostingDateFrom", Convert.ToDateTime(Utility.StringToDate(txtPostingDateFrom.Text)).ToString("yyyy/MM/dd"));
            }

            if (txtPostingDateTo.Text != string.Empty)
            {
                Procparam.Add("@PostingDateTo", Convert.ToDateTime(Utility.StringToDate(txtPostingDateTo.Text)).ToString("yyyy/MM/dd"));
            }

            if (txtInvoiceDateFrom.Text != string.Empty)
            {
                Procparam.Add("@InvoiceDateFrom", Convert.ToDateTime(Utility.StringToDate(txtInvoiceDateFrom.Text)).ToString("yyyy/MM/dd"));
            }

            if (txtInvoiceDateTo.Text != string.Empty)
            {
                Procparam.Add("@InvoiceDateTo", Convert.ToDateTime(Utility.StringToDate(txtInvoiceDateTo.Text)).ToString("yyyy/MM/dd"));
            }


            if (ddlCustName.SelectedValue == "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@CustomerId", "-1");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText == "")
                Procparam.Add("@CustomerId", "0");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@CustomerId", ddlCustName.SelectedValue.ToString());

            //if (ddlCustName.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@CustomerId", ddlCustName.SelectedValue);
            //}

            if (ddlInvoiceNo.SelectedValue == "0" && ddlInvoiceNo.SelectedText != "")
                Procparam.Add("@InvoiceNo", "-1");
            else if (ddlInvoiceNo.SelectedValue != "0" && ddlInvoiceNo.SelectedText == "")
                Procparam.Add("@InvoiceNo", "0");
            else if (ddlInvoiceNo.SelectedValue != "0" && ddlInvoiceNo.SelectedText != "")
                Procparam.Add("@InvoiceNo", ddlInvoiceNo.SelectedValue.ToString());

            //if (ddlInvoiceNo.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@InvoiceNo", ddlInvoiceNo.SelectedValue);
            //}

            if (ddlCFormNumber.SelectedValue == "0" && ddlCFormNumber.SelectedText != "")
                Procparam.Add("@CFormNo", "-1");
            else if (ddlCFormNumber.SelectedValue != "0" && ddlCFormNumber.SelectedText == "")
                Procparam.Add("@CFormNo", "0");
            else if (ddlCFormNumber.SelectedValue != "0" && ddlCFormNumber.SelectedText != "")
                Procparam.Add("@CFormNo", ddlCFormNumber.SelectedValue.ToString());
            //if (ddlCFormNumber.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@CFormNo", ddlCFormNumber.SelectedText);
            //}

            if (ddlStateName.SelectedValue == "0" && ddlStateName.SelectedText != "")
                Procparam.Add("@StateId", "-1");
            else if (ddlStateName.SelectedValue != "0" && ddlStateName.SelectedText == "")
                Procparam.Add("@StateId", "0");
            else if (ddlStateName.SelectedValue != "0" && ddlStateName.SelectedText != "")
                Procparam.Add("@StateId", ddlStateName.SelectedValue.ToString());

            //if (ddlStateName.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@StateId", ddlStateName.SelectedValue);
            //}

            if (ddlCForm.SelectedValue != "0")
            {
                Procparam.Add("@CForm", ddlCForm.SelectedValue);
            }

            if (ddlVendorName.SelectedValue == "0" && ddlVendorName.SelectedText != "")
                Procparam.Add("@VendorId", "-1");
            else if (ddlVendorName.SelectedValue != "0" && ddlVendorName.SelectedText == "")
                Procparam.Add("@VendorId", "0");
            else if (ddlVendorName.SelectedValue != "0" && ddlVendorName.SelectedText != "")
                Procparam.Add("@VendorId", ddlVendorName.SelectedValue.ToString());

            //if (ddlVendorName.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@VendorId", ddlVendorName.SelectedValue);
            //}

            PnlCSTPurchaseDetails.Visible = true;
            grvCSTPurchaseDetailsReport.Visible = true;
            //grvCFormSalesReport.HeaderRow.Attributes.Add("Style", "text-Align");
            grvCSTPurchaseDetailsReport.BindGridView("[S3G_RPT_GetCSTPurchaseDetailsReport]", Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            ucCustomPaging.Visible = true;
            btnPrint.Visible = true;
            if (blnIsNewRow)
            {
                grvCSTPurchaseDetailsReport.Rows[0].Visible = false;
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
            txtPostingDateFrom.Text = string.Empty;
            txtPostingDateTo.Text = string.Empty;
            txtInvoiceDateFrom.Text = string.Empty;
            txtInvoiceDateTo.Text = string.Empty;
            ddlCustName.SelectedText = string.Empty;
            ddlCustName.SelectedValue = "0";
            ddlInvoiceNo.SelectedText = string.Empty;
            ddlInvoiceNo.SelectedValue = "0";
            ddlStateName.SelectedText = string.Empty;
            ddlStateName.SelectedValue = "0";
            ddlCForm.SelectedValue = "0";
            ddlCFormNumber.SelectedText = string.Empty;
            ddlCFormNumber.SelectedValue = "0";
            grvCSTPurchaseDetailsReport.Visible = false;
            //   divCFormSalesDetails.Visible = false;
            ucCustomPaging.Visible = false;
            //ddlCFormNumber.Enabled = false;
            btnPrint.Visible = false;
            PnlCSTPurchaseDetails.Visible = false;
            ddlVendorName.SelectedText = string.Empty;
            ddlVendorName.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvCSTPurchaseDetailsReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Style.Add("text-Align", "center");
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    string str = e.Row.Cells[i].Text.ToString();
                    decimal Num;
                    bool isNum = decimal.TryParse(str, out Num);
                    if (isNum)
                    {
                        e.Row.Cells[i].Style.Add("text-Align", "right");
                    }
                    else
                    {
                        e.Row.Cells[i].Style.Add("text-Align", "Left");
                        if (str.Length >= 11)
                        {
                            e.Row.Cells[i].Style.Add("width", "200px");
                        }
                        else if (str.Length >= 5 && str.Length < 10)
                        {
                            e.Row.Cells[i].Style.Add("width", "150px");
                        }
                    }
                    if (str.Contains(".000"))
                    {
                        e.Row.Cells[i].Style.Add("text-Align", "right");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }

    protected void ExportExcel()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());

            if (txtPostingDateFrom.Text != string.Empty)
            {
                Procparam.Add("@PostingDateFrom", Convert.ToDateTime(Utility.StringToDate(txtPostingDateFrom.Text)).ToString("yyyy/MM/dd"));
            }

            if (txtPostingDateTo.Text != string.Empty)
            {
                Procparam.Add("@PostingDateTo", Convert.ToDateTime(Utility.StringToDate(txtPostingDateTo.Text)).ToString("yyyy/MM/dd"));
            }

            if (txtInvoiceDateFrom.Text != string.Empty)
            {
                Procparam.Add("@InvoiceDateFrom", Convert.ToDateTime(Utility.StringToDate(txtInvoiceDateFrom.Text)).ToString("yyyy/MM/dd"));
            }

            if (txtInvoiceDateTo.Text != string.Empty)
            {
                Procparam.Add("@InvoiceDateTo", Convert.ToDateTime(Utility.StringToDate(txtInvoiceDateTo.Text)).ToString("yyyy/MM/dd"));
            }



            //if (txtPostingDateFrom.Text != string.Empty)
            //{
            //    Procparam.Add("@PostingDateFrom", Convert.ToDateTime(Utility.StringToDate(txtPostingDateFrom.Text)).ToString("yyyy/MM/dd"));
            //}

            //if (txtPostingDateTo.Text != string.Empty)
            //{
            //    Procparam.Add("@PostingDateTo", Convert.ToDateTime(Utility.StringToDate(txtPostingDateTo.Text)).ToString("yyyy/MM/dd"));
            //}

            //if (txtInvoiceDateFrom.Text != string.Empty)
            //{
            //    Procparam.Add("@InvoiceDateFrom", Convert.ToDateTime(Utility.StringToDate(txtInvoiceDateFrom.Text)).ToString("yyyy/MM/dd"));
            //}

            //if (txtInvoiceDateTo.Text != string.Empty)
            //{
            //    Procparam.Add("@InvoiceDateTo", Convert.ToDateTime(Utility.StringToDate(txtInvoiceDateTo.Text)).ToString("yyyy/MM/dd"));
            //}



            if (ddlCustName.SelectedValue == "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@CustomerId", "-1");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText == "")
                Procparam.Add("@CustomerId", "0");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@CustomerId", ddlCustName.SelectedValue.ToString());

            //if (ddlCustName.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@CustomerId", ddlCustName.SelectedValue);
            //}

            if (ddlInvoiceNo.SelectedValue == "0" && ddlInvoiceNo.SelectedText != "")
                Procparam.Add("@InvoiceNo", "-1");
            else if (ddlInvoiceNo.SelectedValue != "0" && ddlInvoiceNo.SelectedText == "")
                Procparam.Add("@InvoiceNo", "0");
            else if (ddlInvoiceNo.SelectedValue != "0" && ddlInvoiceNo.SelectedText != "")
                Procparam.Add("@InvoiceNo", ddlInvoiceNo.SelectedValue.ToString());

            //if (ddlInvoiceNo.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@InvoiceNo", ddlInvoiceNo.SelectedValue);
            //}

            if (ddlCFormNumber.SelectedValue == "0" && ddlCFormNumber.SelectedText != "")
                Procparam.Add("@CFormNo", "-1");
            else if (ddlCFormNumber.SelectedValue != "0" && ddlCFormNumber.SelectedText == "")
                Procparam.Add("@CFormNo", "0");
            else if (ddlCFormNumber.SelectedValue != "0" && ddlCFormNumber.SelectedText != "")
                Procparam.Add("@CFormNo", ddlCFormNumber.SelectedValue.ToString());
            //if (ddlCFormNumber.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@CFormNo", ddlCFormNumber.SelectedText);
            //}

            if (ddlStateName.SelectedValue == "0" && ddlStateName.SelectedText != "")
                Procparam.Add("@StateId", "-1");
            else if (ddlStateName.SelectedValue != "0" && ddlStateName.SelectedText == "")
                Procparam.Add("@StateId", "0");
            else if (ddlStateName.SelectedValue != "0" && ddlStateName.SelectedText != "")
                Procparam.Add("@StateId", ddlStateName.SelectedValue.ToString());

            //if (ddlStateName.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@StateId", ddlStateName.SelectedValue);
            //}

            if (ddlCForm.SelectedValue != "0")
            {
                Procparam.Add("@CForm", ddlCForm.SelectedValue);
            }

            if (ddlVendorName.SelectedValue == "0" && ddlVendorName.SelectedText != "")
                Procparam.Add("@VendorId", "-1");
            else if (ddlVendorName.SelectedValue != "0" && ddlVendorName.SelectedText == "")
                Procparam.Add("@VendorId", "0");
            else if (ddlVendorName.SelectedValue != "0" && ddlVendorName.SelectedText != "")
                Procparam.Add("@VendorId", ddlVendorName.SelectedValue.ToString());

            //if (ddlVendorName.SelectedText.Trim() != string.Empty)
            //{
            //    Procparam.Add("@VendorId", ddlVendorName.SelectedValue);
            //}
            Procparam.Add("@IsExport", "1");
            dtGetdata = Utility.GetDefaultData("[S3G_RPT_GetCSTPurchaseDetailsReport]", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            

            if (Grv.Rows.Count > 0)
            {
                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;
                string attachment = "attachment; filename=CSTPurchaseDetails_Report.xls";
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
                row["Column1"] = "CST Purchase Details Report";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtPostingDateFrom.Text == string.Empty)
                    row["Column1"] = "From Posting Date : ";
                else
                    row["Column1"] = "From Posting Date : " + txtPostingDateFrom.Text;

                if (txtPostingDateTo.Text == string.Empty)
                    row["Column2"] = "To Posting Date : ";
                else
                    row["Column2"] = "To Posting Date : " + txtPostingDateTo.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtInvoiceDateFrom.Text == string.Empty)
                    row["Column1"] = "From Invoice Date : ";
                else
                    row["Column1"] = "From Invoice Date : " + txtInvoiceDateFrom.Text;

                if (txtInvoiceDateTo.Text == string.Empty)
                    row["Column2"] = "To Invoice Date : ";
                else
                    row["Column2"] = "To Invoice Date : " + txtInvoiceDateTo.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlStateName.SelectedValue == "0")
                    row["Column1"] = "State : All";
                else
                    row["Column1"] = "State : " + ddlStateName.SelectedText;

                if (ddlCustName.SelectedValue == "0")
                    row["Column2"] = "Lessee Name : All";
                else
                    row["Column2"] = "Lessee Name : " + ddlCustName.SelectedText;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlVendorName.SelectedValue == "0")
                    row["Column1"] = "Vendor Name : All";
                else
                    row["Column1"] = "Vendor Name : " + ddlVendorName.SelectedText;
                if (ddlInvoiceNo.SelectedValue == "0")
                    row["Column2"] = "Invoice Number : All";
                else
                    row["Column2"] = "Invoice Number : " + ddlInvoiceNo.SelectedText;

                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlCForm.SelectedValue == "0")
                    row["Column1"] = "C Form : All";
                else
                    row["Column1"] = "C Form : " + ddlCForm.SelectedItem.Text;
                if (ddlCFormNumber.SelectedValue == "0")
                    row["Column2"] = "C Form Number : All";
                else
                    row["Column2"] = "C Form Number : " + ddlCFormNumber.SelectedText;

                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 14;
                grv1.Rows[1].Cells[0].ColumnSpan = 14;
                //grv1.Rows[2].Cells[0].ColumnSpan = 4;
                //grv1.Rows[3].Cells[0].ColumnSpan = 4;
                //grv1.Rows[4].Cells[0].ColumnSpan = 4;
                //grv1.Rows[5].Cells[0].ColumnSpan = 4;
                //grv1.Rows[6].Cells[0].ColumnSpan = 4;


                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                //grv1.Rows[2].HorizontalAlign = HorizontalAlign.Left;
                //grv1.Rows[3].HorizontalAlign = HorizontalAlign.Left;
                //grv1.Rows[4].HorizontalAlign = HorizontalAlign.Left;
                //grv1.Rows[5].HorizontalAlign = HorizontalAlign.Left;
                //grv1.Rows[6].HorizontalAlign = HorizontalAlign.Left;


                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;
                grv1.RenderControl(htw);
                Grv.RenderControl(htw);


                ddlStateName.SelectedValue = "0";
                ddlCustName.SelectedValue = "0";
                ddlVendorName.SelectedValue = "0";
                ddlInvoiceNo.SelectedValue = "0";
                ddlCForm.SelectedValue = "0";
                ddlCFormNumber.SelectedValue = "0";


                Response.Write(sw.ToString());
                Response.End();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No data to export");
                //txtPostingDateFrom.Text = "";
                //txtPostingDateTo.Text = "";
                //txtInvoiceDateFrom.Text = "";
                //txtInvoiceDateTo.Text = "";
                //ddlStateName.SelectedText = "--All--";
                //ddlCustName.SelectedText = "--All--";
                //ddlVendorName.SelectedText = "--All--";
                //ddlInvoiceNo.SelectedText = "--All--";
                //ddlCForm.SelectedIndex = 0;
                //ddlCFormNumber.SelectedText = "--All--";

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
}