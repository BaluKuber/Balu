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
using S3GBusEntity.FundManagement;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.IO;

public partial class Fund_Management_S3G_FundMgt_GrossMarginCalculation : ApplyThemeForProject
{
    #region Variable Declaration

    FunderMgtServiceReference.FundMgtServiceClient objFundMgtServiceClient;
    FundMgtServices.S3G_Fundmgt_GrossMarginDataTable objGrossMarginDatatable;
    FundMgtServices.S3G_Fundmgt_GrossMarginRow objGrossMarginRow;

    SerializationMode ObjSerMode = SerializationMode.Binary;

    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    string intCustomerId;
    string strMode = string.Empty;
    int intUserId;
    int GrossMargin_Id;
    bool Is_Active;
    int Active;
    int intProgramId = 536;
    public string strDateFormat;
    string Flag = string.Empty;
    //decimal decAltFC;
    Dictionary<string, string> Procparam;
    string strPageName = "Gross Margin Calculation";
    DataTable dtTable = new DataTable();
    public static Fund_Management_S3G_FundMgt_GrossMarginCalculation obj_Page;
    PagingValues ObjPaging = new PagingValues();
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    string strRedirectPage = "~/Fund Management/S3G_Fund_Translander.aspx?Code=GMC";

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Fund Management/S3G_FundMgt_GrossMarginCalculation.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../Fund Management/S3G_Fund_Translander.aspx?Code=GMC';";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";

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
        FunPriLoadGrossMarginCal();
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
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Gross Margin Report";
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

            GrossMargin_Id = 0;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strMode = Request.QueryString.Get("qsMode");
                GrossMargin_Id = Convert.ToInt32(fromTicket.Name);
            }

            strMode = Request.QueryString["qsMode"];

            //TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            //if (txtPageSize.Text != "")
            //    ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            //else
            //    ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            //PageAssignValue obj = new PageAssignValue(this.AssignValue);
            //ucCustomPaging.callback = obj;
            //ucCustomPaging.ProPageNumRW = ProPageNumRW;
            //ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

            //CalendarExtender1.Format = strDateFormat;
            //txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',false,  false);");

            //CalendarExtender2.Format = strDateFormat;
            //txtenddate.Attributes.Add("onblur", "fnDoDate(this,'" + txtenddate.ClientID + "','" + strDateFormat + "',false,  false);");

            if (!IsPostBack)
            {
                if (strMode == "M")                     //Modify Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunPriLoadGrossMargineDtls();
                    btnReCal.Visible = btnExport.Visible = true;
                }
                else if (strMode == "Q")                //Query Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FunPriLoadGrossMargineDtls();
                    txtDiscountingRate.ReadOnly = true;
                    btnExport.Visible = true;
                    btnOk.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load GST Report page");
        }
    }

    private void FunPriLoadGrossMargineDtls()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
            Procparam.Add("@GrossMargin_Id", Convert.ToString(GrossMargin_Id));

            DataSet dsTable = new DataSet();

            dsTable = Utility.GetDataset("S3G_FundMgt_GetGrossMarginDet", Procparam);
            dtTable = new DataTable();
            dtTable = dsTable.Tables[1];

            ViewState["dtTranche"] = dtTable;

            grvGST.DataSource = dtTable;
            grvGST.DataBind();

            if (dsTable.Tables[0].Rows.Count > 0)
            {
                ddlTrancheNumber.ReadOnly = true;
                ddlTrancheNumber.SelectedValue = dsTable.Tables[0].Rows[0]["Tranche_Header_id"].ToString();
                ddlTrancheNumber.SelectedText = dsTable.Tables[0].Rows[0]["Tranche_Name"].ToString();
                txtInvoiceValue.Text = dsTable.Tables[0].Rows[0]["Invoice_Value"].ToString();
                txtITCAvailable.Text = dsTable.Tables[0].Rows[0]["ITC_Available"].ToString();
                txtDiscountingRate.Text = dsTable.Tables[0].Rows[0]["Discounting_Rate"].ToString();
                txtGrossMarginNBR.Text = dsTable.Tables[0].Rows[0]["Gross_Margin_NBR"].ToString();
                txtRSValue.Text = dsTable.Tables[0].Rows[0]["RS_Value"].ToString();
                txtRebateDiscountValue.Text = dsTable.Tables[0].Rows[0]["Rebate_Discount_Value"].ToString();
                txtSecurityDeposit.Text = dsTable.Tables[0].Rows[0]["Security_Deposit"].ToString();
                txtRentalStartDate.Text = dsTable.Tables[0].Rows[0]["Rental_Start_Date"].ToString();
                txtRentalEndDate.Text = dsTable.Tables[0].Rows[0]["Rental_End_date"].ToString();
                txtEOTGuaranteedValue.Text = dsTable.Tables[0].Rows[0]["EOT_Guaranteed_Value"].ToString();
            }
            pnlVAT.Visible = true;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// This Method is called after Clicking the Ok button.
    /// To Load the Repayment Details in Grid.
    /// </summary>
    /// <param name="PANum"></param>
    /// <param name="SANum"></param>
    private void FunPriLoadGrossMarginCal()
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

            Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
            //if (txtStartDate.Text != "")
            //    Procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());

            //if (txtenddate.Text != "")
            //    Procparam.Add("@EndDate", Utility.StringToDate(txtenddate.Text).ToString());

            if (ddlTrancheNumber.SelectedValue != "0" && ddlTrancheNumber.SelectedText != String.Empty)
                Procparam.Add("@Tranche_Id", ddlTrancheNumber.SelectedValue);
            else if (ddlTrancheNumber.SelectedValue == "0" && ddlTrancheNumber.SelectedText != String.Empty)
                Procparam.Add("@Tranche_Id", "0");

            if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@PA_SA_Ref_ID", ddlRSNo.SelectedValue);
            else if (ddlRSNo.SelectedValue == "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@PA_SA_Ref_ID", "0");

            if (txtDiscountingRate.Text != "")
                Procparam.Add("@Discounting_Rate", txtDiscountingRate.Text);

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            //bool bIsNewRow = false;
            //grvGST.BindGridView("S3G_RPT_GetGrossMarginReport", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            DataSet dsTable = new DataSet();

            dsTable = Utility.GetDataset("S3G_FundMgt_GetGrossMarginCalculation", Procparam);
            dtTable = new DataTable();
            dtTable = dsTable.Tables[1];

            ViewState["dtTranche"] = dtTable;

            grvGST.DataSource = dtTable;
            grvGST.DataBind();

            if (dsTable.Tables[0].Rows.Count > 0)
            {
                txtInvoiceValue.Text = dsTable.Tables[0].Rows[0]["Invoice_Value"].ToString();
                txtITCAvailable.Text = dsTable.Tables[0].Rows[0]["ITC_Available"].ToString();
                txtDiscountingRate.Text = dsTable.Tables[0].Rows[0]["Discounting_Rate"].ToString();
                txtGrossMarginNBR.Text = dsTable.Tables[0].Rows[0]["Gross_Margin_NBR"].ToString();
                txtRSValue.Text = dsTable.Tables[0].Rows[0]["RS_Value"].ToString();
                txtRebateDiscountValue.Text = dsTable.Tables[0].Rows[0]["Rebate_Discount_Value"].ToString();
                txtSecurityDeposit.Text = dsTable.Tables[0].Rows[0]["Security_Deposit"].ToString();
                txtRentalStartDate.Text = dsTable.Tables[0].Rows[0]["Rental_Start_Date"].ToString();
                txtRentalEndDate.Text = dsTable.Tables[0].Rows[0]["Rental_End_date"].ToString();
                txtEOTGuaranteedValue.Text = dsTable.Tables[0].Rows[0]["EOT_Guaranteed_Value"].ToString();
            }

            if (dtTable.Rows.Count == 0)
            {
                grvGST.Rows[0].Visible = btnExport.Visible = false;
            }
            else
            {
                btnExport.Visible = false;
                grvGST.Visible = pnlVAT.Visible = true;
            }

            //ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            //ucCustomPaging.setPageSize(ProPageSizeRW);


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void ddlTrancheNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTrancheNumber.SelectedValue != "0" || ddlRSNo.SelectedValue != "0")
            {
                FunPriLoadGrossMarginCal();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    protected void txtDiscountingRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTrancheNumber.SelectedValue != "0" || ddlRSNo.SelectedValue != "0")
            {
                FunPriLoadGrossMarginCal();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    protected void btnReCal_Click(object sender, EventArgs e)
    {
        if (ddlTrancheNumber.SelectedValue != "0" || ddlRSNo.SelectedValue != "0")
        {
            FunPriLoadGrossMarginCal();
        }
    }

    #endregion

    #region Page Events


    #region Button ( Save / Clear / Print)

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objGrossMarginDatatable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_Fundmgt_GrossMarginDataTable();
            objGrossMarginRow = objGrossMarginDatatable.NewS3G_Fundmgt_GrossMarginRow();
            objGrossMarginRow.Company_id = intCompanyId.ToString();
            objGrossMarginRow.User_id = intUserId.ToString();
            objGrossMarginRow.GrossMargin_Id = GrossMargin_Id;
            objGrossMarginRow.Tranche_Header_Id = Convert.ToInt32(ddlTrancheNumber.SelectedValue);
            objGrossMarginRow.PA_SA_REF_ID = Convert.ToInt32(ddlRSNo.SelectedValue);
            objGrossMarginRow.Invoice_Value = Convert.ToDecimal(txtInvoiceValue.Text);
            objGrossMarginRow.RS_Value = Convert.ToDecimal(txtRSValue.Text);
            objGrossMarginRow.GrossMargin_Date = Utility.StringToDate(txtRentalStartDate.Text.ToString()).ToString();
            objGrossMarginRow.Rental_Start_Date = Utility.StringToDate(txtRentalStartDate.Text.ToString()).ToString();
            objGrossMarginRow.Rental_End_date = Utility.StringToDate(txtRentalEndDate.Text.ToString()).ToString();
            objGrossMarginRow.ITC_Available = Convert.ToDecimal(txtITCAvailable.Text);
            objGrossMarginRow.Rebate_Discount_Value = Convert.ToDecimal(txtRebateDiscountValue.Text);
            objGrossMarginRow.Discounting_Rate = Convert.ToDecimal(txtDiscountingRate.Text);
            objGrossMarginRow.Security_Deposit = Convert.ToDecimal(txtSecurityDeposit.Text);
            objGrossMarginRow.EOT_Guaranteed_Value = Convert.ToDecimal(txtEOTGuaranteedValue.Text);
            objGrossMarginRow.Gross_Margin = Convert.ToDecimal(txtGrossMarginNBR.Text);

            if (ViewState["dtTranche"] != null && ((DataTable)ViewState["dtTranche"]).Rows.Count > 0)
            {
                objGrossMarginRow.XML_GrossMargin = Utility.FunPubFormXml((DataTable)ViewState["dtTranche"]);
            }
            else
            {
                objGrossMarginRow.XML_GrossMargin = null;
            }

            objGrossMarginDatatable.AddS3G_Fundmgt_GrossMarginRow(objGrossMarginRow);

            objFundMgtServiceClient = new FunderMgtServiceReference.FundMgtServiceClient();
            string strErrorMsg = string.Empty;
            Int64 intGrossMargin_Id = 0;
            string StrGrossMargin_Number = string.Empty;
            int iErrorCode = objFundMgtServiceClient.FunPubCreateOrModifyGrossMarginCalculation(out intGrossMargin_Id, out strErrorMsg, out StrGrossMargin_Number, ObjSerMode, ClsPubSerialize.Serialize(objGrossMarginDatatable, ObjSerMode));
            switch (iErrorCode)
            {
                case 0:
                    btnOk.Enabled = false;
                    if (GrossMargin_Id == 0)
                    {
                        strAlert = "Gross Margin Calculation " + StrGrossMargin_Number + " Created Successfully";
                        strAlert += @"\n\nWould you like to Create one more Gross Margin Calculation?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPage = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Gross Margin Calculation Updated Successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        btnOk.Enabled = false;
                    }
                    break;
                case 1:
                    Utility.FunShowAlertMsg(this.Page, "Document Number Not Defined");
                    break;
                case 3:
                    Utility.FunShowAlertMsg(this.Page, "Gross Margin Calculation already saved for selected Tranche");
                    break;
                case 50:
                    Utility.FunShowAlertMsg(this, "Error in Saving Details");
                    break;
            }
        }
        catch (Exception objException)
        {
            CVRepaymentSchedule.ErrorMessage = objException.Message;
            CVRepaymentSchedule.IsValid = false;
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        //Response.Redirect(strRedirectPageAdd);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
            }
            else
            {
                Response.Redirect(strRedirectPage);
            }
        }
        catch (Exception objException)
        {
            //cvTranche.ErrorMessage = objException.Message;
            //cvTranche.IsValid = false;
        }
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            Procparam.Add("@GrossMargin_Id", Convert.ToString(GrossMargin_Id));

            dtTable = new DataTable();

            DataSet dsTable = new DataSet();

            dsTable = Utility.GetDataset("S3G_RPT_GetGrossMarginExport", Procparam);

            dtTable = dsTable.Tables[1];

            string filename = "Gross Margin Report" + ".xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            GridView dgGrid = new GridView();

            GridView grv1 = new GridView();
            DataTable dtHeader = new DataTable();
            dtHeader.Columns.Add("Column1");
            dtHeader.Columns.Add("Column2");
            dtHeader.Columns.Add("Column3");
            dtHeader.Columns.Add("Column4");
            dtHeader.Columns.Add("Column5");
            dtHeader.Columns.Add("Column6");
            dtHeader.Columns.Add("Column7");
            dtHeader.Columns.Add("Column8");
            dtHeader.Columns.Add("Column9");
            dtHeader.Columns.Add("Column10");
            dtHeader.Columns.Add("Column11");

            DataRow row = dtHeader.NewRow();
            row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();
            row["Column1"] = "Gross Margin Report";
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();
            row["Column1"] = "Invoice Value";
            row["Column2"] = dsTable.Tables[0].Rows[0]["Invoice_Value"].ToString();
            row["Column4"] = "ITC Available";
            row["Column5"] = dsTable.Tables[0].Rows[0]["ITC_Available"].ToString();
            row["Column7"] = "Discounting Rate";
            row["Column8"] = dsTable.Tables[0].Rows[0]["Discounting_Rate"].ToString();
            row["Column10"] = "Gross Margin / NBR";
            row["Column11"] = dsTable.Tables[0].Rows[0]["Gross_Margin_NBR"].ToString();
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();
            row["Column1"] = "RS Value";
            row["Column2"] = dsTable.Tables[0].Rows[0]["RS_Value"].ToString();
            row["Column4"] = "Rebate Discount Value";
            row["Column5"] = dsTable.Tables[0].Rows[0]["Rebate_Discount_Value"].ToString();
            row["Column7"] = "Security Deposit";
            row["Column8"] = dsTable.Tables[0].Rows[0]["Security_Deposit"].ToString();
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();
            row["Column1"] = "Rental Start Date";
            row["Column2"] = dsTable.Tables[0].Rows[0]["Rental_Start_Date"].ToString();
            row["Column4"] = "Rental End Date";
            row["Column5"] = dsTable.Tables[0].Rows[0]["Rental_End_date"].ToString();
            row["Column7"] = "EOT Guaranteed Value";
            row["Column8"] = dsTable.Tables[0].Rows[0]["EOT_Guaranteed_Value"].ToString();
            dtHeader.Rows.Add(row);

            row = dtHeader.NewRow();
            dtHeader.Rows.Add(row);

            grv1.DataSource = dtHeader;
            grv1.DataBind();
            grv1.HeaderRow.Visible = false;
            grv1.GridLines = GridLines.None;
            grv1.Rows[0].Cells[0].ColumnSpan = 14;
            grv1.Rows[1].Cells[0].ColumnSpan = 14;
            grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
            grv1.Font.Bold = true;
            grv1.ForeColor = System.Drawing.Color.DarkBlue;
            grv1.Font.Name = "calibri";
            grv1.Font.Size = 10;
            grv1.RenderControl(hw);

            dgGrid.DataSource = dtTable;
            dgGrid.DataBind();

            dgGrid.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 20px; font-weight: bold;");
            dgGrid.ForeColor = System.Drawing.Color.DarkBlue;

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

    //RS NO 
    [System.Web.Services.WebMethod]
    public static string[] GetRSNoDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetRSNO", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Program_id", "285");
        Procparam.Add("@Approved", "1");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetMRANumber_AGT", Procparam));
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