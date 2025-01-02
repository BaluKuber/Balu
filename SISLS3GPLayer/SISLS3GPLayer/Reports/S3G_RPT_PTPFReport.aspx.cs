/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   PTPF Report
/// Created By          :   Sivasubramanain.K
/// Created Date        :   27-APR-2015
/// Purpose             :   This is the Report screen for searching PTPF Details
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

public partial class Reports_S3G_RPT_PTPFReport : ApplyThemeForProject
{
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    PagingValues ObjPaging = new PagingValues();
    public static Reports_S3G_RPT_PTPFReport obj_Page;
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
            BindPTPFDetailsReport();
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
                txtDateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtDateFrom.ClientID + "','" + strDateFormat + "',true,  false);");
                txtDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtDateTo.ClientID + "','" + strDateFormat + "',true,  false);");
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

   
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if ((!string.IsNullOrEmpty(txtDateFrom.Text)) && (!string.IsNullOrEmpty(txtDateTo.Text)))
            {
                if ((Utility.StringToDate(txtDateFrom.Text)) > (Utility.StringToDate(txtDateTo.Text)))
                {
                    if (hidDate.Value.ToUpper() == "STARTDATE")
                    {
                        Utility.FunShowAlertMsg(this, "From Date should be lesser than the To Date");
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "To Date should be greater than the From Date");
                    }
                    return;
                }
            }

            BindPTPFDetailsReport();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public void BindPTPFDetailsReport()
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
            if (ddlRentalScheduleNo.SelectedText.Trim() != "")
            {
                Procparam.Add("@RS_No", ddlRentalScheduleNo.SelectedValue);
            }
            if (ddlNoteNo.SelectedText.Trim() != "")
            {
                Procparam.Add("@Note_Header_ID", ddlNoteNo.SelectedValue);
            }

           // divAssetInsuranceStatus.Visible = true;
            PnlPTPF.Visible = true;
            grvPTPFReport.Visible = true;
            grvPTPFReport.BindGridView("[S3G_RPT_Get_PTPFDetails]", Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            ucCustomPaging.Visible = true;
            if(intTotalRecords>0)
            btnDownload.Visible = true;

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
            ddlRentalScheduleNo.SelectedValue = "0";
            ddlRentalScheduleNo.SelectedText = string.Empty;
            ddlNoteNo.SelectedValue = "0";
            ddlNoteNo.SelectedText = string.Empty;
            //divAssetInsuranceStatus.Visible = false;
            grvPTPFReport.Visible = false;
            ucCustomPaging.Visible = false;
            btnDownload.Visible = false;
            PnlPTPF.Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
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
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
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
            if (ddlRentalScheduleNo.SelectedText.Trim() != "")
            {
                Procparam.Add("@RS_No", ddlRentalScheduleNo.SelectedValue);
            }
            if (ddlNoteNo.SelectedText.Trim() != "")
            {
                Procparam.Add("@Note_Header_ID", ddlNoteNo.SelectedValue);
            }

            dtGetdata = Utility.GetDefaultData("[S3G_RPT_Get_PTPFDetails]", Procparam);
            //int colspan = 0;
            //try
            //{
            //    for (int col_count = 10; col_count < dtGetdata.Columns.Count; col_count++)
            //    {
            //        try
            //        {
            //            string strHeader = dtGetdata.Columns[col_count].ColumnName;
            //            if (strHeader.Contains("_PR_"))
            //            {
            //                dtGetdata.Columns[col_count].ColumnName = strHeader.Replace("_PR_", "");
            //                //colspan += 1;
            //            }
            //            else if (strHeader.Contains("_PA_"))
            //                dtGetdata.Columns[col_count].ColumnName = strHeader.Replace("_PA_", "");
            //            else if (strHeader.Contains("_SR_"))
            //                dtGetdata.Columns[col_count].ColumnName = strHeader.Replace("_SR_", "");
            //            else if (strHeader.Contains("_SA_"))
            //            {
            //                dtGetdata.Columns[col_count].ColumnName = strHeader.Replace("_SA_", "");
            //                colspan += 1;
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //    }
            //    dtGetdata.AcceptChanges();
            //}
            //catch (Exception ex)
            //{

            //}
          
            GridView Grv = new GridView();
            Grv.RowDataBound += new GridViewRowEventHandler(this.Grv_RowDataBound);
            

            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;
          
            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=PTPF_Report.xls";
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
                row["Column1"] = "PTPF Report";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtDateFrom.Text == string.Empty)
                    row["Column1"] = "From Date : ";
                else
                    row["Column1"] = "From Date : " + txtDateFrom.Text;

                if (txtDateTo.Text == string.Empty)
                    row["Column2"] = "To Date : ";
                else
                    row["Column2"] = "To Date : " + txtDateTo.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlCustName.SelectedValue == "0")
                    row["Column1"] = "Lessee Name : --All--";
                else
                    row["Column1"] = "Lessee Name : " + ddlCustName.SelectedText;

                if (ddlRentalScheduleNo.SelectedValue == "0")
                    row["Column2"] = "RentalScheduleNo : --All--";
                else
                    row["Column2"] = "RentalScheduleNo : " + ddlRentalScheduleNo.SelectedText;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlNoteNo.SelectedValue == "0")
                    row["Column1"] = "Note Creation No : --All--";
                else
                    row["Column1"] = "Note Creation No : " + ddlNoteNo.SelectedText;
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

                GridView grvHeader = new GridView();
                DataTable dtGroupHeader = new DataTable();
                dtGroupHeader.Columns.Add("EmptyValue");
                dtGroupHeader.Columns.Add("Primaryrental");
                dtGroupHeader.Columns.Add("PrimaryAMF");
                //dtGroupHeader.Columns.Add("Secondaryrental");
                //dtGroupHeader.Columns.Add("SecondaryAMF");

                DataRow rowHeader = dtGroupHeader.NewRow();
                rowHeader["EmptyValue"] = " ";
                rowHeader["Primaryrental"] = "Rental- PTPF (From Asset category)";
                rowHeader["PrimaryAMF"] = "AMF- PTPF (From Asset category)";
                //rowHeader["Secondaryrental"] = "Secondary Rental";
                //rowHeader["SecondaryAMF"] = "Secondary AMF";
                dtGroupHeader.Rows.Add(rowHeader);

                grvHeader.DataSource = dtGroupHeader;
                grvHeader.DataBind();
                //Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                grvHeader.HeaderRow.Visible = false;
                //grvHeader.GridLines = GridLines.None;
                //int FirstColspan = 0;
                //FirstColspan = dtGetdata.Columns.Count - (colspan * 4);
                //if (FirstColspan < 0)
                //    FirstColspan = 0;
                int Cmncolspan = Convert.ToInt32(ViewState["Cmncolspan"]);
                int colspan = Convert.ToInt32(ViewState["colspan"]);
                grvHeader.Rows[0].Cells[0].ColumnSpan = Cmncolspan;
                grvHeader.Rows[0].Cells[1].ColumnSpan = colspan;
                grvHeader.Rows[0].Cells[2].ColumnSpan = colspan;
                //grvHeader.Rows[0].Cells[3].ColumnSpan = colspan;
                //grvHeader.Rows[0].Cells[4].ColumnSpan = colspan;

                grvHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                grvHeader.Font.Bold = true;
                grvHeader.ForeColor = System.Drawing.Color.DarkBlue;
                grvHeader.BackColor = System.Drawing.Color.FromName("#ebf0f7");

                grvHeader.Font.Name = "calibri";
                grvHeader.Font.Size = 10;
                grvHeader.RenderControl(htw);

                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Title, "alert('No Records Found!.');", true);
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

    protected void grvPTPFReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)                 // if header - then set the style dynamically.
        {

            for (int col_count = 1; col_count < e.Row.Cells.Count; col_count++)
            {
                string strHeader = e.Row.Cells[col_count].Text;
                if(strHeader.Contains("_PR_"))
                    e.Row.Cells[col_count].Text = strHeader.Replace("_PR_", "");
                else if (strHeader.Contains("_PA_"))
                    e.Row.Cells[col_count].Text = strHeader.Replace("_PA_", "");
              
            }
        }
    }

      protected void grvPTPFReport_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colspan = 0;
            int Cmncolspan = 0;
            for (int col_count = 0; col_count < e.Row.Cells.Count; col_count++)
            {
                string strHeader = e.Row.Cells[col_count].Text; 
                if (strHeader.Contains("_PR_"))
                {
                }
                else if (strHeader.Contains("_PA_"))
                {
                      colspan += 1;
                }
                              
                else
                {
                    Cmncolspan += 1;
                }
               
            }
            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert); 

            TableCell HeaderCell2 = new TableCell();
            HeaderCell2.Text = "";
            HeaderCell2.ColumnSpan = Cmncolspan;
            HeaderRow.Cells.Add(HeaderCell2);

            HeaderCell2 = new TableCell();
            HeaderCell2.Text = "Rental- PTPF (From Asset category)";
            HeaderCell2.ColumnSpan = colspan;
           // HeaderCell2.BackColor = System.Drawing.Color.Gold;
            HeaderRow.Cells.Add(HeaderCell2);

            HeaderCell2 = new TableCell();
            HeaderCell2.Text = "AMF- PTPF (From Asset category)";
            HeaderCell2.ColumnSpan = colspan;
            HeaderRow.Cells.Add(HeaderCell2);
            
            //HeaderCell2 = new TableCell();
            //HeaderCell2.Text = "Secondary Rental";
            //HeaderCell2.ColumnSpan = colspan;
            //HeaderRow.Cells.Add(HeaderCell2);

            //HeaderCell2 = new TableCell();
            //HeaderCell2.Text = "Secondary AMF";
            //HeaderCell2.ColumnSpan = colspan;
         
            HeaderRow.Cells.Add(HeaderCell2);
 
            grvPTPFReport.Controls[0].Controls.AddAt(0, HeaderRow);
            HeaderRow.Attributes.Add("class", "styleGridHeader");
            ViewState["Cmncolspan"] = Cmncolspan;
            ViewState["colspan"] = colspan;
       
        }
    }


      protected void Grv_RowDataBound(object sender, GridViewRowEventArgs e)
      {
          if (e.Row.RowType == DataControlRowType.Header)                 // if header - then set the style dynamically.
          {

              for (int col_count = 1; col_count < e.Row.Cells.Count; col_count++)
              {
                  string strHeader = e.Row.Cells[col_count].Text;
                  if (strHeader.Contains("_PR_"))
                      e.Row.Cells[col_count].Text = strHeader.Replace("_PR_", "");
                  else if (strHeader.Contains("_PA_"))
                      e.Row.Cells[col_count].Text = strHeader.Replace("_PA_", "");
                 
              }
          }
      }

    
    
    #region ServiceMethod
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
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCustomers", Procparam), false);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }
    [System.Web.Services.WebMethod]
    public static string[] GetRentalScheduleNo(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            DataTable dtCommon = new DataTable();
            DataSet Ds = new DataSet();
            //if (System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"] != null)
            //{
            //    Procparam.Add("@StartDate", System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"].ToString());
            //}
            //if (System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"] != null)
            //{
            //    Procparam.Add("@EndDate", System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"].ToString());
            //}
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
            Procparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAccountsActivated_AGT", Procparam));
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }
    [System.Web.Services.WebMethod]
    public static string[] GetNoteNo(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            DataTable dtCommon = new DataTable();
            DataSet Ds = new DataSet();
            //if (System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"] != null)
            //{
            //    Procparam.Add("@StartDate", System.Web.HttpContext.Current.Session["StartDateAutoSuggestValue"].ToString());
            //}
            //if (System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"] != null)
            //{
            //    Procparam.Add("@EndDate", System.Web.HttpContext.Current.Session["EndDateAutoSuggestValue"].ToString());
            //}
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
            Procparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_NoteNumber_AGT", Procparam));
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }
    #endregion 

}