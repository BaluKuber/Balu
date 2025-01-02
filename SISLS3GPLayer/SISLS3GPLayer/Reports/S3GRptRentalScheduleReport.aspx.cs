#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Rental Schedule Report
/// Created By          :   Chandru K
/// Created Date        :   06-Jul-2015
/// Purpose             :   To Get the Rental Schedule Report.
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


public partial class Reports_S3GRptRentalScheduleReport : ApplyThemeForProject
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
    string strPageName = "Rental Schedule Report";
    DataTable dtTable = new DataTable();
    public static Reports_S3GRptRentalScheduleReport obj_Page;
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
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Rental Schedule Report.";
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

            CalendarExtender1.Format = CalendarExtender2.Format = CalendarExtender3.Format = CalendarExtender4.Format = ceRSCreatedOnStart.Format = ceRSCreatedOnEnd.Format = strDateFormat;
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");

            if (!IsPostBack)
            {
                ucCustomPaging.Visible = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
              throw new ApplicationException("Unable to Load Rental Schedule Report");
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
            if (string.IsNullOrEmpty(txtStartDate.Text) && string.IsNullOrEmpty(txtEndDate.Text) &&
                string.IsNullOrEmpty(txtRSActivationFrom.Text) && string.IsNullOrEmpty(txtRSActivationTo.Text) &&
                string.IsNullOrEmpty(ddlLesseeName.SelectedText) && string.IsNullOrEmpty(ddlTrancheName.SelectedText) &&
                string.IsNullOrEmpty(txtRSCreatedOnStart.Text) && string.IsNullOrEmpty(txtRSCreatedOnEnd.Text))
            {
                Utility.FunShowAlertMsg(this, "Select The RS Date or RS Activation Date Or RS Created On Date or Lessee Name or Tranche Name");
                grvPDC.DataSource = null;
                grvPDC.DataBind();
                pnlPDC.Attributes.Add("Style", "display:none");
                return;
            }

            if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            {
                if (Utility.StringToDate(txtStartDate.Text) > Utility.StringToDate(txtEndDate.Text)) // start date should be less than or equal to the enddate
                {
                    Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to the From Date");
                    txtEndDate.Text = "";
                    grvPDC.DataSource = null;
                    grvPDC.DataBind();
                    pnlPDC.Attributes.Add("Style", "display:none"); 
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtRSActivationFrom.Text) && !string.IsNullOrEmpty(txtRSActivationTo.Text))
            {

                if (Utility.StringToDate(txtRSActivationFrom.Text) > Utility.StringToDate(txtRSActivationTo.Text)) // start date should be less than or equal to the enddate
                {
                    Utility.FunShowAlertMsg(this, "RS Activation To Date should be greater than or equal to the RS Activation From Date");
                    txtRSActivationTo.Text = "";
                    grvPDC.DataSource = null;
                    grvPDC.DataBind();
                    pnlPDC.Attributes.Add("Style", "display:none");
                    return;
                }
            }

            Procparam = new Dictionary<string, string>();
           
            if (!string.IsNullOrEmpty(txtStartDate.Text))
            {
                Procparam.Add("@From_Date", Utility.StringToDate(txtStartDate.Text).ToString());
            }
            if (!string.IsNullOrEmpty(txtEndDate.Text))
            {
                Procparam.Add("@To_Date", Utility.StringToDate(txtEndDate.Text).ToString());
            }

            if (!string.IsNullOrEmpty(txtRSActivationFrom.Text))
            {
                Procparam.Add("@RSActivation_From_Date", Utility.StringToDate(txtRSActivationFrom.Text).ToString());
            }
            if (!string.IsNullOrEmpty(txtRSActivationTo.Text))
            {
                Procparam.Add("@RSActivation_To_Date", Utility.StringToDate(txtRSActivationTo.Text).ToString());
            }

            if (!string.IsNullOrEmpty(txtRSCreatedOnStart.Text))
            {
                Procparam.Add("@RSCreatedOn_From_Date", Utility.StringToDate(txtRSCreatedOnStart.Text).ToString());
            }
            if (!string.IsNullOrEmpty(txtRSCreatedOnEnd.Text))
            {
                Procparam.Add("@RSCreatedOn_To_Date", Utility.StringToDate(txtRSCreatedOnEnd.Text).ToString());
            }

            if (ddlLesseeName.SelectedValue != "0")
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            if (ddlFunderName.SelectedValue != "0")
                Procparam.Add("@Funder_Id", ddlFunderName.SelectedValue);
            if (ddlFundingStatus.SelectedValue != "0")
                Procparam.Add("@Funding_Status", ddlFundingStatus.SelectedValue);
            if (ddlBookingStatus.SelectedValue != "0")
                Procparam.Add("@Booking_Status", ddlBookingStatus.SelectedValue);

            if (ddlLocation.SelectedValue != "0")
                Procparam.Add("@Location_Id", ddlLocation.SelectedValue);

            if (ddlTrancheName.SelectedValue != "0")
                Procparam.Add("@Tranche_Id", ddlTrancheName.SelectedValue);

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            bool bIsNewRow = false;
            grvPDC.BindGridView("S3G_RPT_GetRentalScheduleReport", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
         
            if (bIsNewRow)
            {
                grvPDC.Rows[0].Visible = btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            pnlPDC.Attributes.Add("Style", "display:block"); 

            grvPDC.Visible = ucCustomPaging.Visible = true;
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
            txtStartDate.Text = txtEndDate.Text = txtRSActivationFrom.Text = txtRSActivationTo.Text = txtRSCreatedOnStart.Text = txtRSCreatedOnEnd.Text = String.Empty;
            ddlLesseeName.Clear();
            ddlFunderName.Clear();
            ddlTrancheName.Clear();
            ddlFundingStatus.SelectedIndex = 0;
            ddlBookingStatus.SelectedIndex = 0;
            grvPDC.DataSource = null;
            grvPDC.DataBind();
            grvPDC.Visible = ucCustomPaging.Visible = btnExport.Visible = false;
            pnlPDC.Attributes.Add("Style", "display:none"); 
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
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            
            if (!string.IsNullOrEmpty(txtStartDate.Text))
            {
                Procparam.Add("@From_Date", Utility.StringToDate(txtStartDate.Text).ToString());
            }
            if (!string.IsNullOrEmpty(txtEndDate.Text))
            {
                Procparam.Add("@To_Date", Utility.StringToDate(txtEndDate.Text).ToString());
            }

            if (!string.IsNullOrEmpty(txtRSActivationFrom.Text))
            {
                Procparam.Add("@RSActivation_From_Date", Utility.StringToDate(txtRSActivationFrom.Text).ToString());
            }
            if (!string.IsNullOrEmpty(txtRSActivationTo.Text))
            {
                Procparam.Add("@RSActivation_To_Date", Utility.StringToDate(txtRSActivationTo.Text).ToString());
            }

            if (!string.IsNullOrEmpty(txtRSCreatedOnStart.Text))
            {
                Procparam.Add("@RSCreatedOn_From_Date", Utility.StringToDate(txtRSCreatedOnStart.Text).ToString());
            }
            if (!string.IsNullOrEmpty(txtRSCreatedOnEnd.Text))
            {
                Procparam.Add("@RSCreatedOn_To_Date", Utility.StringToDate(txtRSCreatedOnEnd.Text).ToString());
            }

            if (ddlLesseeName.SelectedValue != "0")
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            if (ddlFunderName.SelectedValue != "0")
                Procparam.Add("@Funder_Id", ddlFunderName.SelectedValue);
            if (ddlFundingStatus.SelectedValue != "0")
                Procparam.Add("@Funding_Status", ddlFundingStatus.SelectedValue);
            if (ddlBookingStatus.SelectedValue != "0")
                Procparam.Add("@Booking_Status", ddlBookingStatus.SelectedValue);

            if (ddlLocation.SelectedValue != "0")
                Procparam.Add("@Location_Id", ddlLocation.SelectedValue);

            if (ddlTrancheName.SelectedValue != "0")
                Procparam.Add("@Tranche_Id", ddlTrancheName.SelectedValue);

            Procparam.Add("@IsExport", "1");

            dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_RPT_GetRentalScheduleReport", Procparam);

            GridView Grv = new GridView();
       
            Grv.DataSource = dtTable;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 18px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=RentalScheduleReport.xls";
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
                row["Column1"] = "Rental Schedule Report";
                dtHeader.Rows.Add(row);

                if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
                {
                    row = dtHeader.NewRow();
                    row["Column1"] = "For the Period " + txtStartDate.Text + " to " + txtEndDate.Text;
                    dtHeader.Rows.Add(row);
                }

                if (!string.IsNullOrEmpty(txtRSActivationFrom.Text) && !string.IsNullOrEmpty(txtRSActivationTo.Text))
                {
                    row = dtHeader.NewRow();
                    row["Column1"] = "For the RS Activation Period " + txtRSActivationFrom.Text + " to " + txtRSActivationTo.Text;
                    dtHeader.Rows.Add(row);
                }

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 11;
                grv1.Rows[1].Cells[0].ColumnSpan = 11;
                //grv1.Rows[2].Cells[0].ColumnSpan = 11;
                //grv1.Rows[3].Cells[0].ColumnSpan = 11;

                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                //grv1.Rows[2].HorizontalAlign = HorizontalAlign.Center;
                //grv1.Rows[3].HorizontalAlign = HorizontalAlign.Center;

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
    public static string[] GetTrancheNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        //dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTranche_AGT", dictparam));
        return suggetions.ToArray();
    }

    #endregion
}
