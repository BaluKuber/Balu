﻿#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Debtor's Ageing Report
/// Created By          :   Chandru K
/// Created Date        :   04-Feb-2015
/// Purpose             :   To Get the Debtor's Ageing Report.
/// 
/// Modified By          :  Swarna S
/// Modified Date        :   15-Jun-2015
/// Purpose             :   obs fix
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

public partial class Reports_S3GRptDebtorAgingReport : ApplyThemeForProject
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
    string strPageName = "Debtor's Ageing Report";      //          TC_026 -- Senthilkumar P -- 08-Apr-2015
    DataTable dtTable = new DataTable();
    public static Reports_S3GRptDebtorAgingReport obj_Page;
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
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load " + strPageName;        //           TC_026 -- Senthilkumar P -- 08-Apr-2015
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
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");

            if (!IsPostBack)
            {
                ucCustomPaging.Visible = false;
                ListItem liSelect = new ListItem("--All--", "0");
                ddlTranche.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
              //throw new ApplicationException("Unable to Load Repayment Schedule page");         //           TC_026 -- Senthilkumar P -- 08-Apr-2015
              throw ex; //           TC_026 -- Senthilkumar P -- 08-Apr-2015
        }
    }

    private void FunPriTranche()
    {
        Dictionary<string, string> dictassetrgstr = new Dictionary<string, string>();
        try
        {
            dictassetrgstr.Add("@Company_ID", intCompanyId.ToString());
            dictassetrgstr.Add("@Customer_ID", ddlLesseeName.SelectedValue);
            ddlTranche.BindMemoDataTable("S3G_CLN_GetTranche_ODI", dictassetrgstr, new string[] { "Tranche_Header_Id", "Tranche_Name" });
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
            Procparam.Add("@Date", Utility.StringToDate(txtStartDate.Text).ToString());


            if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != "")
                Procparam.Add("@Customer_Id", "-1");
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText == "")
                Procparam.Add("@Customer_Id", "0");
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != "")
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue.ToString());


            //if (ddlLesseeName.SelectedValue != "0")
            //    Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            if (ddlTranche.SelectedValue != "0")
                Procparam.Add("@Tranche_Id", ddlTranche.SelectedValue);

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            bool bIsNewRow = false;
            grvPDC.BindGridView("S3G_RPT_GetDebtorAgingReport", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
         
            if (bIsNewRow)
            {
                grvPDC.Rows[0].Visible = btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            grvPDC.Visible = ucCustomPaging.Visible = pnlPDC.Visible = true;
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
            ddlTranche.Items.Clear();
            ListItem liSelect = new ListItem("--All--", "0");
            ddlTranche.Items.Insert(0, liSelect);
            grvPDC.DataSource = null;
            grvPDC.DataBind();
            grvPDC.Visible = ucCustomPaging.Visible = pnlPDC.Visible = btnExport.Visible = false;
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Error in Clear.";
            CVRepaymentSchedule.IsValid = false;
        }
    }

    protected void ddlLesseeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriTranche();
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = ex.Message;
            CVRepaymentSchedule.IsValid = false;
        }
    }

    protected void grvPDC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[7].HorizontalAlign = e.Row.Cells[8].HorizontalAlign = e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[10].HorizontalAlign = e.Row.Cells[11].HorizontalAlign = e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        catch (Exception exp)
        {
            CVRepaymentSchedule.ErrorMessage = exp.Message;
            CVRepaymentSchedule.IsValid = false;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            Procparam.Add("@Date", Utility.StringToDate(txtStartDate.Text).ToString());

            if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != "")
                Procparam.Add("@Customer_Id", "-1");
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText == "")
                Procparam.Add("@Customer_Id", "0");
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != "")
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue.ToString());

     
            //if (ddlLesseeName.SelectedValue != "0")
            //    Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            if (ddlTranche.SelectedValue != "0")
                Procparam.Add("@Tranche_Id", ddlTranche.SelectedValue);

            Procparam.Add("@IsExport", "1");

            dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_RPT_GetDebtorAgingReport", Procparam);

            GridView Grv = new GridView();
       
            Grv.DataSource = dtTable;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=DebtorAgeingReport.xls";              //          TC_026 -- Senthilkumar P -- 08-Apr-2015
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Debtor's Ageing Report";                   //          TC_026 -- Senthilkumar P -- 08-Apr-2015
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "As on " + txtStartDate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 14;
                grv1.Rows[1].Cells[0].ColumnSpan = 14;
                grv1.Rows[2].Cells[0].ColumnSpan = 14;
                //grv1.Rows[3].Cells[0].ColumnSpan = 9;
                //grv1.Rows[4].Cells[0].ColumnSpan = 9;
                
                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[2].HorizontalAlign = HorizontalAlign.Center;
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

    [System.Web.Services.WebMethod]
    public static string[] GetFunderNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetFunderName", dictparam));
        return suggetions.ToArray();
    }

    #endregion
}
