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

public partial class Reports_S3G_RPT_Inv_Summary :ApplyThemeForProject
{
  string strPageName = "Invoice Summary Report";
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    PagingValues ObjPaging = new PagingValues();
    public static Reports_S3G_RPT_Inv_Summary obj_Page;
    string strDateFormat = string.Empty;
    DataTable dtGetdata = new DataTable();
    DataTable dtGriddata = new DataTable();
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
            BindPurchaseInvoiceDetailsReport();
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
            CalendarExtenderInvoicePostingDateFrom.Format = strDateFormat;
            CalendarExtenderInvoicePostingDateTo.Format = strDateFormat;
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
                txtInvoicePostingDateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoicePostingDateFrom.ClientID + "','" + strDateFormat + "',true,  false);");
                txtInvoicePostingDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoicePostingDateTo.ClientID + "','" + strDateFormat + "',true,  false);");
                ucCustomPaging.Visible = false;
                //myDivForPanelScroll.Visible = false;
                myDivForPanelScroll.Style.Add("display","none");
                PnPurchaseInvoiceDetails.Style.Add("display", "none");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }   

    public void BindPurchaseInvoiceDetailsReport()
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

            if (ddlCustName.SelectedText.Trim() != string.Empty)
            {
                Procparam.Add("@Customer_Id", ddlCustName.SelectedValue);
            }

            if (ddlState.SelectedText.Trim() != string.Empty)
            {
                Procparam.Add("@Location_Id", ddlState.SelectedValue);
            }

            if (ddlTranche.SelectedText.Trim() != string.Empty)
            {
                Procparam.Add("@Tranche_Id", ddlTranche.SelectedValue);
            }

            if (txtInvoicePostingDateFrom.Text != string.Empty)
            {
                Procparam.Add("@From_Date", Convert.ToDateTime(Utility.StringToDate(txtInvoicePostingDateFrom.Text)).ToString("yyyy/MM/dd"));
            }

            if (txtInvoicePostingDateTo.Text != string.Empty)
            {
                Procparam.Add("@To_Date", Convert.ToDateTime(Utility.StringToDate(txtInvoicePostingDateTo.Text)).ToString("yyyy/MM/dd"));
            }

            Procparam.Add("@Account_From", ddlAccountFrom.SelectedValue);
            Procparam.Add("@Account_To", ddlAccountTo.SelectedValue);

            //Procparam.Add("@company_id", intCompanyId.ToString());

            //PnPurchaseInvoiceDetails.Visible = true;
            PnPurchaseInvoiceDetails.Style.Add("display", "block");
            myDivForPanelScroll.Style.Add("display", "block");
            //myDivForPanelScroll.Visible = true;
            grvPurchaseInvoiceDetailsReport.Visible = true;

            dtGriddata = Utility.GetGridData("S3G_RPT_InvSummary", Procparam, out intTotalRecords, ObjPaging);

            if (dtGriddata.Rows.Count > 0)
            {
                grvPurchaseInvoiceDetailsReport.DataSource = dtGriddata;
                grvPurchaseInvoiceDetailsReport.DataBind();
                ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
                ucCustomPaging.setPageSize(ProPageSizeRW);
                ucCustomPaging.Visible = true;
                btnPrint.Visible = true;
            }
            else
            {
                // PnlMRADetails.Visible = true;
                grvPurchaseInvoiceDetailsReport.DataSource = null;
                grvPurchaseInvoiceDetailsReport.DataBind();
                grvPurchaseInvoiceDetailsReport.EmptyDataText = "No Records Found";
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
           BindPurchaseInvoiceDetailsReport();
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
            Clear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void Clear()
    {
        try
        {
            ddlCustName.SelectedValue = string.Empty;
            ddlCustName.SelectedText = string.Empty;

            ddlTranche.SelectedValue = string.Empty;
            ddlTranche.SelectedText = string.Empty;
          
            txtInvoicePostingDateFrom.Text = string.Empty;
            txtInvoicePostingDateTo.Text = string.Empty;
         
            grvPurchaseInvoiceDetailsReport.Visible = false;
            ucCustomPaging.Visible = btnPrint.Visible = false;
            //PnPurchaseInvoiceDetails.Visible = false;
            PnPurchaseInvoiceDetails.Style.Add("display", "none");
            //myDivForPanelScroll.Visible = false;
            myDivForPanelScroll.Style.Add("display", "none");
            ddlAccountFrom.SelectedValue = string.Empty;

            ddlAccountTo.SelectedText = string.Empty;
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
            ExportExcel();
          //  Clear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void ExportExcel()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (ddlCustName.SelectedText.Trim() != string.Empty)
            {
                Procparam.Add("@Customer_Id", ddlCustName.SelectedValue);
            }

            if (txtInvoicePostingDateFrom.Text != string.Empty)
            {
                Procparam.Add("@From_Date", Convert.ToDateTime(Utility.StringToDate(txtInvoicePostingDateFrom.Text)).ToString("yyyy/MM/dd"));
            }

            if (txtInvoicePostingDateTo.Text != string.Empty)
            {
                Procparam.Add("@To_Date", Convert.ToDateTime(Utility.StringToDate(txtInvoicePostingDateTo.Text)).ToString("yyyy/MM/dd"));
            }
            
            if (ddlTranche.SelectedText.Trim() != string.Empty)
            {
                Procparam.Add("@Tranche_Id", ddlTranche.SelectedValue);
            }

            if (ddlState.SelectedText.Trim() != string.Empty)
            {
                Procparam.Add("@Location_Id", ddlState.SelectedValue);
            }

            Procparam.Add("@company_id", intCompanyId.ToString());

            Procparam.Add("@Account_From", ddlAccountFrom.SelectedValue);
            Procparam.Add("@Account_To", ddlAccountTo.SelectedValue);

            Procparam.Add("@IsExport", "1");
            dtGetdata = Utility.GetDefaultData("S3G_RPT_InvSummary", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Invoice Summary.xls";
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
                row["Column1"] = "Invoice Summary";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlCustName.SelectedValue == "0")
                    row["Column1"] = "Customer Name : All";
                else
                    row["Column1"] = "Customer Name : " + ddlCustName.SelectedText;
                dtHeader.Rows.Add(row);
                
                row = dtHeader.NewRow();
                if (txtInvoicePostingDateFrom.Text == string.Empty)
                    row["Column1"] = "From Date : ";
                else
                    row["Column1"] = "From Date : " + txtInvoicePostingDateFrom.Text;

                if (txtInvoicePostingDateTo.Text == string.Empty)
                    row["Column2"] = "To Date : ";
                else
                    row["Column2"] = "To Date : " + txtInvoicePostingDateTo.Text;
                dtHeader.Rows.Add(row);

            

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 8;
                grv1.Rows[1].Cells[0].ColumnSpan = 8;
                //grv1.Rows[2].Cells[0].ColumnSpan = 5;
                //grv1.Rows[3].Cells[0].ColumnSpan = 5;
                //grv1.Rows[4].Cells[0].ColumnSpan = 5;
                //grv1.Rows[5].Cells[0].ColumnSpan = 5;


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
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void txtInvoicePostingDateFrom_TextChanged(object sender, EventArgs e)
    {
        hidInvoicePostingDateDate.Value = "STARTDATE";
    }
    protected void txtInvoicePostingDateTo_TextChanged(object sender, EventArgs e)
    {
        hidInvoicePostingDateDate.Value = "ENDDATE";
        //if (txtInvoicePostingDateTo.Text != String.Empty)
        //{
        //    if (Utility.StringToDate(txtInvoicePostingDateTo.Text) > System.DateTime.Today)
        //    {
        //        txtInvoicePostingDateTo.Text = String.Empty;
        //        Utility.FunShowAlertMsg(this.Page, "Invoice Posting To Date cannot be greater than system Date ");
        //        return;
        //    }
        //}
        //if (txtInvoicePostingDateFrom.Text != String.Empty && txtInvoicePostingDateTo.Text != String.Empty)
        //{
        //    int ErrorCount = Utility.CompareDates(txtInvoicePostingDateFrom.Text, txtInvoicePostingDateTo.Text);
        //    if (ErrorCount == -1)
        //    {
        //        txtInvoicePostingDateTo.Text = String.Empty;
        //        Utility.FunShowAlertMsg(this.Page, "Invoice Posting To Date cannot be lesser than Invoice Posting From Date ");
        //        return;
        //    }
        //}
    }
    
    protected void grvPurchaseInvoiceDetailsReport_RowDataBound(object sender, GridViewRowEventArgs e)
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

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheName(String prefixText, int count)
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
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTranche_AGT", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }



    [System.Web.Services.WebMethod]
    public static string[] FunPriBindAccountFrom(String prefixText, int count)
    {
        List<String> suggestions = new List<String>();
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
            Procparam.Add("@customer_Id", Convert.ToString(System.Web.HttpContext.Current.Session["ddlCustName"]));
            Procparam.Add("@PrefixText", prefixText);
           
            Procparam.Add("@Option", "1");
            DataTable dt = Utility.GetDefaultData("FA_RPT_GET_AccountFrom", Procparam);
            if (dt.Rows.Count > 0 || prefixText.ToUpper() == "ALL")
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                dr["id"] = "-2";
                dr["Name"] = "ALL";
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            suggestions = Utility.GetSuggestions(dt);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] FunPriBindAccountTo(String prefixText, int count)
    {
        List<String> suggestions = new List<String>();
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
            Procparam.Add("@customer_Id", Convert.ToString(System.Web.HttpContext.Current.Session["ddlCustName"]));
            Procparam.Add("@Pasa_Id", Convert.ToString(System.Web.HttpContext.Current.Session["ddlAccountFrom"]));
            Procparam.Add("@PrefixText", prefixText);

            Procparam.Add("@Option", "2");
            DataTable dt = Utility.GetDefaultData("FA_RPT_GET_AccountFrom", Procparam);
            if (dt.Rows.Count > 0 || prefixText.ToUpper() == "ALL")
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                dr["id"] = "-2";
                dr["Name"] = "ALL";
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            suggestions = Utility.GetSuggestions(dt);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {

        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        
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


    protected void ddlCustName_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            System.Web.HttpContext.Current.Session["ddlCustName"] = (ddlCustName.SelectedValue == "-2") ? "-1" : ddlCustName.SelectedValue;
            ddlAccountFrom.SelectedValue = "-2";
            ddlAccountFrom.SelectedText = "--Select--";
            ddlAccountTo.SelectedValue = "-2";
            ddlAccountTo.SelectedText = "--Select--";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlTranche_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            System.Web.HttpContext.Current.Session["ddlTranche"] = (ddlTranche.SelectedValue == "-2") ? "-1" : ddlTranche.SelectedValue;
            ddlAccountFrom.SelectedValue = "-2";
            ddlAccountFrom.SelectedText = "--Select--";
            ddlAccountTo.SelectedValue = "-2";
            ddlAccountTo.SelectedText = "--Select--";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlAccountFrom_Item_Selected(object Sender, EventArgs e)
    {
        ddlAccountTo.SelectedValue = ddlAccountFrom.SelectedValue;
        ddlAccountTo.SelectedText = ddlAccountFrom.SelectedText;
        System.Web.HttpContext.Current.Session["ddlAccountFrom"] = (ddlAccountFrom.SelectedValue == "0") ? "0" : ddlAccountFrom.SelectedValue;
    }


   
}