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

public partial class Reports_S3g_RPT__BTNReport : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    string intCustomerId;
    int intUserId;
    bool Is_Active;
    int Active;
    int intProgramId = 536;
    public string strDateFormat;
    string Flag = string.Empty;
    //decimal decAltFC;
    Dictionary<string, string> Procparam;
    string strPageName = "BTN Report";
    DataTable dtTable = new DataTable();
    public static Reports_S3g_RPT__BTNReport obj_Page;
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

            /*     if (txtStartDate.Text == "" && txtenddate.Text == "")
                 {
                     Utility.FunShowAlertMsg(this, "Enter the BTN / DC start Date and BTN / DC End Date");
                     txtStartDate.Focus();
                     return;
                 }

                 if (txtStartDate.Text == "")
                 {
                     Utility.FunShowAlertMsg(this, "Enter the BTN / DC Start Date");
                     txtStartDate.Focus();
                     return;
                 }

                 if (txtenddate.Text == "")
                 {
                     Utility.FunShowAlertMsg(this, "Enter the BTN / DC End Date");
                     txtStartDate.Focus();
                     return;
                 }
             */

            Procparam = new Dictionary<string, string>();
            if (txtStartDate.Text != "")
                Procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());

            if (txtenddate.Text != "")
                Procparam.Add("@EndDate", Utility.StringToDate(txtenddate.Text).ToString());


            if (ddlCustomerName.SelectedValue != "0" && ddlCustomerName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", ddlCustomerName.SelectedValue);
            else if (ddlCustomerName.SelectedValue == "0" && ddlCustomerName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", "0");

            if (ucshipto.SelectedValue != "0" && ucshipto.SelectedText != String.Empty)
                Procparam.Add("@location_id", ucshipto.SelectedText);
            else if (ucshipto.SelectedValue == "0" && ucshipto.SelectedText != String.Empty)
                Procparam.Add("@location_id", "0");

            if (ddlDocumentNumber.SelectedValue != "0" && ddlDocumentNumber.SelectedText != String.Empty)
                Procparam.Add("@DocumentNumber", ddlDocumentNumber.SelectedText);
            else if (ddlDocumentNumber.SelectedValue == "0" && ddlDocumentNumber.SelectedText != String.Empty)
                Procparam.Add("@DocumentNumber", "0");

            if (ddlBTNNo.SelectedValue != "0" && ddlBTNNo.SelectedText != String.Empty)
                Procparam.Add("@BTN_Number", ddlBTNNo.SelectedText);
            else if (ddlBTNNo.SelectedValue == "0" && ddlBTNNo.SelectedText != String.Empty)
                Procparam.Add("@BTN_Number", "0");

            if (ddlTParentRSNumber.SelectedValue != "0" && ddlTParentRSNumber.SelectedText != String.Empty)
                Procparam.Add("@RSNo_Parent", ddlTParentRSNumber.SelectedValue);
            else if (ddlTParentRSNumber.SelectedValue == "0" && ddlTParentRSNumber.SelectedText != String.Empty)
                Procparam.Add("@RSNo_Parent", "0");

            if (ddlNewRSNumber.SelectedValue != "0" && ddlNewRSNumber.SelectedText != String.Empty)
                Procparam.Add("@RSNo_New", ddlNewRSNumber.SelectedValue);
            else if (ddlNewRSNumber.SelectedValue == "0" && ddlNewRSNumber.SelectedText != String.Empty)
                Procparam.Add("@RSNo_New", "0");

            if (ddlStatus.SelectedValue != "0" && ddlStatus.SelectedItem.Text != String.Empty)
                Procparam.Add("@Status", ddlStatus.SelectedValue);
            else if (ddlStatus.SelectedValue == "0" && ddlStatus.SelectedItem.Text != String.Empty)
                Procparam.Add("@Status", "0");

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            bool bIsNewRow = false;
            grvGST.BindGridView("S3G_RPT_GetBTNReport", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

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
            Response.Redirect("../Reports/S3g_RPT__BTNReport.aspx", false);
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

            /*
                        if (txtStartDate.Text == "")
                        {
                            Utility.FunShowAlertMsg(this, "Enter the BTN / DC Date range");
                            txtStartDate.Focus();
                            return;
                        }
                        if (txtenddate.Text == "")
                        {
                            Utility.FunShowAlertMsg(this, "Enter the BTN / DC End Date");
                            txtStartDate.Focus();
                            return;
                        }

            */
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            if (txtStartDate.Text != "")
                Procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());

            if (txtenddate.Text != "")
                Procparam.Add("@EndDate", Utility.StringToDate(txtenddate.Text).ToString());


            if (ddlCustomerName.SelectedValue != "0" && ddlCustomerName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", ddlCustomerName.SelectedValue);
            else if (ddlCustomerName.SelectedValue == "0" && ddlCustomerName.SelectedText != String.Empty)
                Procparam.Add("@Customer_Id", "0");

            if (ucshipto.SelectedValue != "0" && ucshipto.SelectedText != String.Empty)
                Procparam.Add("@location_id", ucshipto.SelectedValue);
            else if (ucshipto.SelectedValue == "0" && ucshipto.SelectedText != String.Empty)
                Procparam.Add("@location_id", "0");

            if (ddlDocumentNumber.SelectedValue != "0" && ddlDocumentNumber.SelectedText != String.Empty)
                Procparam.Add("@DocumentNumber", ddlDocumentNumber.SelectedValue);
            else if (ddlDocumentNumber.SelectedValue == "0" && ddlDocumentNumber.SelectedText != String.Empty)
                Procparam.Add("@DocumentNumber", "0");

            if (ddlBTNNo.SelectedValue != "0" && ddlBTNNo.SelectedText != String.Empty)
                Procparam.Add("@BTN_Number", ddlBTNNo.SelectedValue);
            else if (ddlBTNNo.SelectedValue == "0" && ddlBTNNo.SelectedText != String.Empty)
                Procparam.Add("@BTN_Number", "0");

            if (ddlTParentRSNumber.SelectedValue != "0" && ddlTParentRSNumber.SelectedText != String.Empty)
                Procparam.Add("@RSNo_Parent", ddlTParentRSNumber.SelectedValue);
            else if (ddlTParentRSNumber.SelectedValue == "0" && ddlTParentRSNumber.SelectedText != String.Empty)
                Procparam.Add("@RSNo_Parent", "0");

            if (ddlNewRSNumber.SelectedValue != "0" && ddlNewRSNumber.SelectedText != String.Empty)
                Procparam.Add("@RSNo_New", ddlNewRSNumber.SelectedValue);
            else if (ddlNewRSNumber.SelectedValue == "0" && ddlNewRSNumber.SelectedText != String.Empty)
                Procparam.Add("@RSNo_New", "0");
            if (ddlStatus.SelectedValue != "0" && ddlStatus.SelectedItem.Text != String.Empty)
                Procparam.Add("@Status", ddlStatus.SelectedValue);
            else if (ddlStatus.SelectedValue == "0" && ddlStatus.SelectedItem.Text != String.Empty)
                Procparam.Add("@Status", "0");

            Procparam.Add("@IsExport", "1");

            dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_RPT_GetBTNReport", Procparam);

            string filename = "BTN Report" + ".xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = dtTable;
            dgGrid.DataBind();
            dgGrid.RenderControl(hw);
            //Write the HTML back to the browser.            
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            this.EnableViewState = false;
            Response.Write(tw.ToString());
            Response.End();

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
    public static string[] GetBuyerName(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        dictparam.Add("@Program_Id", "326");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LoanAd_GetCustomer_AGT", dictparam));
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
    public static string[] GetRSNODetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_WBT_GetPANUMNO", dictparam));
        return suggetions.ToArray();
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

    public static string[] GetBTNNO(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        dictparam.Add("@Option", "1");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_get_BTN_No", dictparam));
        return suggetions.ToArray();
    }
    [System.Web.Services.WebMethod]
    public static string[] GetDocumentNo(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        dictparam.Add("@Option", "2");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_get_BTN_No", dictparam));
        return suggetions.ToArray();
    }
    [System.Web.Services.WebMethod]

    public static string[] GetNewRSNO(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        dictparam.Add("@Option", "3");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_get_BTN_No", dictparam));
        return suggetions.ToArray();
    }
    [System.Web.Services.WebMethod]
    public static string[] GetShiptoAddress(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        dictparam.Add("@Option", "4");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_get_BTN_No", dictparam));
        return suggetions.ToArray();
    }
    /*  protected void txtDateTo_TextChanged(object sender, EventArgs e)
      {
          try
          {
              CheckDate();
          }
          catch (Exception ex)
          {
              //lblErrorMessage.Text = ex.Message;
          }
      }*/


    /* private void CheckDate()
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
     */

    #endregion
}