/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved

/// <Program Summary>
/// Module Name               : Loan Admin 
/// Screen Name               : Bill Generation
/// Created By                : Prabhu.K
/// Created Date              : 23-Dec-2010
/// Purpose                   : 
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    :

/// <Program Summary>


#region NameSpaces
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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
#endregion

public partial class LoanAdmin_S3GLoanAdCompensation : ApplyThemeForProject
{
    #region Variable declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> objProcedureParameter = null;
    int intErrorCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerializationMode = SerializationMode.Binary;
    public string strDateFormat = string.Empty;
    static string strPageName = "Bill Generation";
    int intBillingId;
    private string strDocPath = "";
    string strErrorMessagePrefix = @"Please correct the following validation(s): </br></br>   ";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3GLoanAdTransLander.aspx?Code=LCC";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdCompensation.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=LCC';";

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    #endregion

    #region Events

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txtScheduleDate.Attributes.Add("readonly", "true");
            FunPrilLoadPage();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Billing Details";
            cvBilling.IsValid = false;
        }
    }






    #endregion

    #region Button Events

    #region Common Control

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriClearPage();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = "Unable to Clear the data";
            cvBilling.IsValid = false;
        }
    }
    protected void btnBillPDF_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DocPath"].ToString() != null)
            {
                string strMessage = "Generated Bills are available in Server (" + ViewState["DocPath"].ToString() + " Directory)";
                Utility.FunShowAlertMsg(this, strMessage);
                return;
            }
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = "Unable to Clear the data";
            cvBilling.IsValid = false;
        }
    }


    // Code added by Santhosh.S on 15.07.2013 to generate PDF report
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@CC_Header_Id", intBillingId.ToString());
            objProcedureParameter.Add("@Lob_Id", ddlLOB.SelectedValue.ToString());
            DataSet dsCCReport = Utility.GetDataset("S3G_LOANAD_Get_CCDetails_Report", objProcedureParameter);

            //dtPrint = dsCCReport.Tables[0];

            DataTable dtCCPrint = dsCCReport.Tables[0].DefaultView.ToTable(true, "PANum"); // "ACCOUNT_NO" for "PANum"
            foreach (DataRow DRaCC in dtCCPrint.Rows)
            {
                DataRow[] DRAccDtls = dsCCReport.Tables[0].Select(("PANum='" + DRaCC["PANum"].ToString() + "'")); //, "CashFlow");
                DataTable dt = dsCCReport.Tables[0].Clone();

                foreach (DataRow DR in DRAccDtls)
                {
                    dt.ImportRow(DR);
                }

                //string strBranchName = dt.Rows[0]["Location"].ToString();
                //string strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                string strCCNo = dt.Rows[0]["Doc_no"].ToString();
                string strAccountNo = dt.Rows[0]["PANum"].ToString();
                string strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                string strDocumentPath =dt.Rows[0]["Document_Path"].ToString();
                ViewState["DocPath"] = strDocumentPath;
                string strnewFile = strDocumentPath + "\\CCNo-" + strCCNo.Replace("/", "").ToString();
                decimal decGrandTotal = 0;
                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {
                    decGrandTotal = Convert.ToDecimal(dt.Rows[idx]["AMOUNT"].ToString()); // +Convert.ToDecimal(dt.Rows[idx]["VAT"].ToString()) + Convert.ToDecimal(dt.Rows[idx]["Service_TAX"].ToString());
                }


                //strnewFile += "\\" + strAccountNo;

                if (!Directory.Exists(strnewFile))
                {
                    Directory.CreateDirectory(strnewFile);
                }

                strnewFile += "\\" + strAccountNo.Replace("/", "").ToString();

                strnewFile += ".pdf";
                FileInfo fl = new FileInfo(strnewFile);
                if (fl.Exists == true)
                {
                    fl.Delete();
                }

                string ReportPath = "";

                ReportPath = "CCDetails.rpt";

                ReportDocument rptd = new ReportDocument();
                rptd.Load(Server.MapPath(ReportPath));
                //TextObject TxtSub = (TextObject)rptd.ReportDefinition.ReportObjects["TxtSubject"];
                //TxtSub.Text = "Sub : Bill for Due of INR." + decGrandTotal.ToString() + " falling on " + dSet.Tables[0].Rows[0]["INSTALLMENT_DATE"].ToString();
                rptd.SetDataSource(dt);
                rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);
                btnBillPDF_Click(sender, e);
                //string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strnewFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }

        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            //if (rbtnSchedule.SelectedValue == "0")
            //{
            //    if (string.IsNullOrEmpty(txtScheduleDate.Text))
            //    {
            //        Utility.FunShowAlertMsg(this, "Select a Date for scheduling");
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(txtScheduleTime.Text))
            //    {
            //        Utility.FunShowAlertMsg(this, "Enter the Time for scheduling");
            //        return;
            //    }


            //    if (Utility.CompareDates(txtScheduleDate.Text, DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat)) == 1)
            //    {
            //        Utility.FunShowAlertMsg(this, "Schedule Date should be greater than or equal to Current Date");
            //        return;
            //    }
            //    if (txtScheduleTime.Text != "")
            //    {
            //        DateTime st = DateTime.Parse((Utility.StringToDate(txtScheduleDate.Text).ToString()));
            //        DateTime et = DateTime.Now;
            //        TimeSpan span = new TimeSpan();
            //        span = et.Subtract(st);
            //        if ((span.Days) == 0)
            //        {
            //            DateTime tm = DateTime.Parse(txtScheduleTime.Text);
            //            span = tm.Subtract(et);
            //            if (span.Hours == 0 && span.Minutes < 4)
            //            {
            //                Utility.FunShowAlertMsg(this, "Schduled Time should be greater than or equal to 5 minutes for current time");
            //                return;
            //            }
            //        }
            //    }
            //}
            FunPriSaveRecord();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }

    }




    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClosePage();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }


    #endregion

    #region Page Specific

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Utility.StringToDate(txtMonthYear.Text).Month != Utility.StringToDate(txtStartDate.Text).Month)
            //{
            //    Utility.FunShowAlertMsg(this, "Start Date Month should be equal to Month/Year");
            //    gvBranchWise.DataSource = null;
            //    gvBranchWise.DataBind();

            //    return;
            //}
            //if(Utility.StringToDate(txtMonthYear.Text).Month != Utility.StringToDate(txtEndDate.Text).Month)
            //{
            //    Utility.FunShowAlertMsg(this, "End Date Month should be equal to Month/Year");
            //    gvBranchWise.DataSource = null;
            //    gvBranchWise.DataBind();

            //    return;
            //}
            //if (Utility.CompareDates(txtStartDate.Text, txtEndDate.Text) != 1)
            //{
            //    Utility.FunShowAlertMsg(this, "End Date should be greater than Start Date");
            //    gvBranchWise.DataSource = null;
            //    gvBranchWise.DataBind();

            //    return;
            //}
            //if (ddlFrequency.SelectedItem.Text.ToUpper() == "MONTHLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text).Day - Utility.StringToDate(txtStartDate.Text).Day) < 28)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than or equal to 28 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "WEEKLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text).Day - Utility.StringToDate(txtStartDate.Text).Day) != 7)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 7 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "FORTNIGHTLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text).Day - Utility.StringToDate(txtStartDate.Text).Day) != 15)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 15 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "BI MONTHLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text).Day - Utility.StringToDate(txtStartDate.Text).Day) != 60)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 60 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "QUARTERLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text).Day - Utility.StringToDate(txtStartDate.Text).Day) < 90)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 90 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "HALF YEARLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text).Day - Utility.StringToDate(txtStartDate.Text).Day) < 181)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 181 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "ANNUALLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text).Day - Utility.StringToDate(txtStartDate.Text).Day) < 365)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 365 days");
            //        return;
            //    }
            //}
            //if (rbtnSchedule.SelectedValue == "0")
            //{
            //    if (string.IsNullOrEmpty(txtScheduleDate.Text))
            //    {
            //        Utility.FunShowAlertMsg(this, "Select a Date for scheduling");
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(txtScheduleTime.Text))
            //    {
            //        Utility.FunShowAlertMsg(this, "Enter the Time for scheduling");
            //        return;
            //    }


            //    if (Utility.CompareDates(txtScheduleDate.Text, DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat)) == 1)
            //    {
            //        Utility.FunShowAlertMsg(this, "Schedule Date should be greater than or equal to Current Date");
            //        return;
            //    }
            //    if (txtScheduleTime.Text != "")
            //    {
            //        DateTime st = DateTime.Parse((Utility.StringToDate(txtScheduleDate.Text).ToString()));
            //        DateTime et = DateTime.Now;
            //        TimeSpan span = new TimeSpan();
            //        span = et.Subtract(st);
            //        if ((span.Days) == 0)
            //        {
            //            DateTime tm = DateTime.Parse(txtScheduleTime.Text);
            //            span = tm.Subtract(et);
            //            if (span.Hours == 0 && span.Minutes < 4)
            //            {
            //                Utility.FunShowAlertMsg(this, "Schduled Time should be greater than or equal to 5 minutes for current time");
            //                return;
            //            }
            //        }
            //    }
            //}

            string strMonthYear = "";
            if (Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month).Length == 1)
            {
                strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + "0" + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
            }
            else
            {
                strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
            }
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@MonthYear", strMonthYear);
            DataTable dtMonthLock = Utility.GetDefaultData("s3g_loanad_CheckPrevMonthLock", objProcedureParameter);
            if (dtMonthLock.Rows.Count > 0)
            {
                if (dtMonthLock.Rows[0][0].ToString() == "True")
                {
                    Utility.FunShowAlertMsg(this, "Month/Year already Locked");
                    return;
                }
            }

            FunPriLoadBranchDetails(strMonthYear);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Location Details";
            cvBilling.IsValid = false;
        }

    }

    protected void rbtnSchedule_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtScheduleDate.Text = txtScheduleTime.Text = "";
        if (rbtnSchedule.SelectedValue == "0")
        {
            txtScheduleDate.Enabled = txtScheduleTime.Enabled = calScheduleDate.Enabled = true;
            REVScheduleTime.Enabled = true;
            // REVScheduleTime.Enabled = RFVScheduleDate.Enabled = RFVScheduleTime.Enabled = true;
        }
        else
        {
            txtScheduleDate.Enabled = txtScheduleTime.Enabled = calScheduleDate.Enabled = false;
            REVScheduleTime.Enabled = false;
            // REVScheduleTime.Enabled = RFVScheduleDate.Enabled = RFVScheduleTime.Enabled = false;
        }

    }

    #endregion

    #endregion

    #region Grid Events
    protected void gvBranchWise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Request.QueryString["qsMode"] != null)
            {
                if (Request.QueryString["qsMode"].ToString() == "Q")
                {
                    CheckBox chkSelectAllBranch = e.Row.FindControl("chkSelectAllBranch") as CheckBox;
                    chkSelectAllBranch.Checked = true;
                    chkSelectAllBranch.Enabled = false;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelectBranch = e.Row.FindControl("chkSelectBranch") as CheckBox;
            TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
            Label lblLastFinMonth = e.Row.FindControl("lblLastFinMonth") as Label;
            if (Request.QueryString["qsMode"] != null)
            {
                if (Request.QueryString["qsMode"].ToString() == "Q")
                {
                    chkSelectBranch.Checked = true;
                    chkSelectBranch.Enabled = false;
                    txtRemarks.ReadOnly = true;
                }
            }
            if (string.IsNullOrEmpty(lblLastFinMonth.Text))
            {
                string strMonthYear = "";
                if (Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month).Length == 1)
                {
                    strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + "0" + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
                }
                else
                {
                    strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
                }
                lblLastFinMonth.Text = strMonthYear;
            }

            CheckBox chkSelectAllBranch = gvBranchWise.HeaderRow.FindControl("chkSelectAllBranch") as CheckBox;
            if (chkSelectBranch != null)
            {

                chkSelectBranch.Attributes.Add("onclick", "javascript:fnSelectBranch(" + chkSelectBranch.ClientID + "," + chkSelectAllBranch.ClientID + ");");
            }
        }

    }
    #endregion

    #endregion

    #region Methods

    #region Local Methods

    private void FunPrilLoadPage()
    {
        S3GSession ObjS3GSession = new S3GSession();
        try
        {

            txtMonthYear.Attributes.Add("readonly", "true");
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            txtStartDate.Attributes.Add("readonly", "true");
            txtEndDate.Attributes.Add("readonly", "true");
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            calEndDate.Format = strDateFormat;
            calStartDate.Format = strDateFormat;
            calScheduleDate.Format = strDateFormat;
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                intBillingId = Convert.ToInt32(fromTicket.Name);
            }
            if (!IsPostBack)
            {
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (Request.QueryString["qsMode"] == "Q")
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriLoadLOV();
                    FunPriDisableControls(0);
                    btnPrint.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Billing Related Details");
        }
        finally
        {
            ObjS3GSession = null;
        }
    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFrequency.SelectedIndex = 0;
        txtStartDate.Text = txtEndDate.Text = txtScheduleDate.Text = txtScheduleTime.Text = txtMonthYear.Text = "";
        pnlBranch.Visible = false;
        gvBranchWise.DataSource = null;
        gvBranchWise.DataBind();

        btnSave.Enabled = false;

        if (txtMonthYear.Text == "")
        {
            txtMonthYear.Text =
            DateTime.Today.ToString("MMM") + "-" + DateTime.Today.Year;
        }
    }

    private void FunPriSaveRecord()
    {
        LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objServiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();

        //        ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient objServiceClient = new ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient();
        BillingEntity objBillingEntity = new BillingEntity();
        try
        {
            CheckBox chkSelectAll = gvBranchWise.HeaderRow.FindControl("chkSelectAllBranch") as CheckBox;
            int intSelectedBranchCount = 0;
            if (!chkSelectAll.Checked)
            {
                foreach (GridViewRow grBranch in gvBranchWise.Rows)
                {
                    if (grBranch.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkSelect = grBranch.FindControl("chkSelectBranch") as CheckBox;
                        if (chkSelect.Checked)
                        {
                            intSelectedBranchCount += 1;
                        }
                    }
                }
                if (intSelectedBranchCount == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atleast one Location for Pre EMI calculation");
                    return;
                }
            }
            string strMonthYear = "";
            if (Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month).Length == 1)
            {
                strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + "0" + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
            }
            else
            {
                strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
            }
            //s3g_loanad_CheckPrevMonthLock

            bool blRetVal = true;
            string strLocations = string.Empty;

            foreach (GridViewRow grvRow in gvBranchWise.Rows)
            {
                CheckBox chkSelect = grvRow.FindControl("chkSelectBranch") as CheckBox;
                Label lblLastFinMonth = grvRow.FindControl("lblLastFinMonth") as Label;

                if (chkSelect.Checked && strMonthYear != lblLastFinMonth.Text)
                {
                    strLocations = strLocations + grvRow.Cells[2].Text.ToString() + " - " + lblLastFinMonth.Text + @"\t\n";
                    blRetVal = false;
                    chkSelect.Checked = false;
                    chkSelectAll.Checked = false;
                }
            }
            if (!blRetVal)
            {
                Utility.FunShowAlertMsg(this, "Pre EMI not calculated for previous month(s):" + @"\n\n" + strLocations);
                return;
            }


            string strXmlBranchDetails = FunPriFormXml(gvBranchWise, true);
            //objProcedureParameter = new Dictionary<string, string>();
            //objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            //objProcedureParameter.Add("@XmlBranchDetails", strXmlBranchDetails);
            //objProcedureParameter.Add("@MonthYear", strMonthYear);
            //DataTable dtMonthLock = Utility.GetDefaultData("s3g_loanad_CheckPrevMonthLock", objProcedureParameter);
            //bool blnIsUnLockBranch = false;

            //foreach (DataRow drBranchMonth in dtMonthLock.Rows)
            //{
            //    if (drBranchMonth["Month_Lock"].ToString() == "False")
            //    {
            //        blnIsUnLockBranch = true;
            //    }
            //}
            //if (blnIsUnLockBranch || dtMonthLock.Rows.Count == 0)
            //{
            //    Utility.FunShowAlertMsg(this, "Month/Year should be locked for selected Location(s) ");
            //    return;
            //}
            objBillingEntity.intCompanyId = intCompanyID;
            objBillingEntity.intLobId = Convert.ToInt32(ddlLOB.SelectedValue);
            objBillingEntity.intFrequency = Convert.ToInt32(ddlFrequency.SelectedValue);
            objBillingEntity.intBranchId = Convert.ToInt32(ddlFrequency.SelectedValue);

            objBillingEntity.lngMonthYear = Convert.ToInt64(strMonthYear);
            objBillingEntity.intUserId = intUserID;
            objBillingEntity.dtStartDate = Utility.StringToDate(txtStartDate.Text);
            objBillingEntity.dtEndDate = Utility.StringToDate(txtEndDate.Text);
            objBillingEntity.dtBillingDate = Utility.StringToDate(DateTime.Now.ToString());
            txtScheduleDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
            objBillingEntity.strScheduleTime = DateTime.Now.AddMinutes(5.0).ToShortTimeString();
            //if (rbtnSchedule.SelectedValue == "0")
            //{
            //    objBillingEntity.strScheduleTime = txtScheduleTime.Text;
            //}
            //else
            //{
            //    txtScheduleDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
            //    objBillingEntity.strScheduleTime = DateTime.Now.AddMinutes(5.0).ToShortTimeString();

            //}
            objBillingEntity.dtScheduleDate = Utility.StringToDate(txtScheduleDate.Text);
            objBillingEntity.strXmlBranchDetails = strXmlBranchDetails;
            //objBillingEntity.strXmlControlDataDetails = gvControlDataSheet.FunPubFormXml(true);
            string strJournalMessage = "";
            intErrorCode = objServiceClient.FunPubCreateCCInt(out strJournalMessage, objBillingEntity);
            //intErrorCode = objServiceClient.FunPubCreateBillingInt(out strJournalMessage, objBillingEntity);
            if (intErrorCode == 0)
            {
                strAlert = "Pre EMI Charges calculated successfully";
                strAlert += @"\n\nWould you like to calculate one more Pre EMI Calculation?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else if (intErrorCode == -1)
            {
                Utility.FunShowAlertMsg(this, "Document Number not defined for " + strJournalMessage);
                return;
            }
            else if (intErrorCode == -2)
            {
                Utility.FunShowAlertMsg(this, "Document Number exceeding for " + strJournalMessage);
                return;
            }
            else if (intErrorCode == 53)
            {
                Utility.FunShowAlertMsg(this.Page, strJournalMessage);
                return;
            }
            else
            {
                if (intErrorCode == 50)
                    Utility.FunShowValidationMsg(this.Page, "", intErrorCode);
                else
                    Utility.FunShowValidationMsg(this.Page, "CLN_BILL", intErrorCode, strJournalMessage, false);
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Insert Pre EMI Details");
        }
        finally
        {
            objServiceClient.Close();
            objBillingEntity = null;
        }
    }

    private bool FunPriCheckPrevMonth()
    {
        bool blRetVal = true;
        string strLocations = string.Empty;
        string strMonthYear = "";
        if (Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month).Length == 1)
        {
            strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + "0" + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
        }
        else
        {
            strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
        }

        CheckBox chkSelectAll = gvBranchWise.HeaderRow.FindControl("chkSelectAllBranch") as CheckBox;

        foreach (GridViewRow grvRow in gvBranchWise.Rows)
        {
            CheckBox chkSelect = grvRow.FindControl("chkSelectBranch") as CheckBox;
            Label lblLastFinMonth = grvRow.FindControl("lblLastFinMonth") as Label;

            if (chkSelect.Checked && strMonthYear != lblLastFinMonth.Text)
            {
                strLocations = strLocations + grvRow.Cells[2].Text.ToString() + " - " + lblLastFinMonth.Text + Environment.NewLine;
                blRetVal = false;
                chkSelect.Checked = false;
                chkSelectAll.Checked = false;
            }
        }
        if (!blRetVal)
        {
            Utility.FunShowAlertMsg(this, "Pre EMI not calculated for previous month(s):" + Environment.NewLine + strLocations);
        }

        return blRetVal;
    }

    private string FunPriFormXml(GridView grvXml, bool IsNeedUpperCase)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {
            CheckBox chkSelect = grvRow.FindControl("chkSelectBranch") as CheckBox;
            CheckBox chkSelectAll = grvXml.HeaderRow.FindControl("chkSelectAllBranch") as CheckBox;
            bool blnIsRowSelect = false;
            if ((!chkSelectAll.Checked && chkSelect.Checked) || chkSelectAll.Checked)
            {
                blnIsRowSelect = true;
            }
            intcolcount = 0;
            if (blnIsRowSelect)
            {
                strbXml.Append(" <Details ");
                for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
                {

                    if (grvXml.Columns[intcolcount].HeaderText != "")
                    {
                        strColValue = grvRow.Cells[intcolcount].Text;
                        if (strColValue == "")
                        {
                            if (grvRow.Cells[intcolcount].Controls.Count > 0)
                            {
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                {
                                    strColValue = ((TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                {
                                    strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                {
                                    strColValue = ((Label)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                            }
                            if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                            {
                                if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                                {
                                    strColValue = Utility.StringToDate(strColValue).ToString();
                                }
                            }

                            if (strColValue.Trim() == "")
                            {
                                strColValue = string.Empty;
                            }
                        }
                        if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                        {
                            if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                            {
                                strColValue = Utility.StringToDate(strColValue).ToString();
                            }
                        }
                        if (strColValue.Trim() == "")
                        {
                            strColValue = string.Empty;
                        }
                        // If Numeric BoundColumn has empty (SPACE &nbsp; value ) at the same time that field is a nullable column in DB 
                        // Avoid adding that column to XML to insert the null value
                        if (strColValue.Trim() == "&nbsp;")
                        {
                            continue;
                        }
                        strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                        strColValue = strColValue.Replace("'", "\"");
                        if (IsNeedUpperCase)
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToUpper().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                        else
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToLower().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                    }

                }
                strbXml.Append(" /> ");
            }
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    private void FunPriClosePage()
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Cancel this process");
        }
    }

    private void FunPriClearPage()
    {
        try
        {
            ddlLOB.SelectedIndex = ddlFrequency.SelectedIndex = 0;
            txtStartDate.Text = txtEndDate.Text = txtScheduleDate.Text = txtScheduleTime.Text = txtMonthYear.Text = "";
            pnlBranch.Visible = false;
            gvBranchWise.DataSource = null;
            gvBranchWise.DataBind();
            btnSave.Enabled = false;
            btnPrint.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Clear the Data");
        }
    }

    private void FunPriLoadLOV()
    {
        try
        {
            if (objProcedureParameter != null)
                objProcedureParameter.Clear();
            else
                objProcedureParameter = new Dictionary<string, string>();

            objProcedureParameter.Add("@OPTION", "1");
            objProcedureParameter.Add("@COMPANYID", Convert.ToString(intCompanyID));
            objProcedureParameter.Add("@USERID", Convert.ToString(intUserID));
            objProcedureParameter.Add("@ProgramId", "213");
            if (Request.QueryString["qsMode"] == "Q")
            {
                objProcedureParameter.Add("@TYPE", "Q");
            }
            ddlLOB.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            objProcedureParameter.Clear();
            objProcedureParameter.Add("@OPTION", "35");
            objProcedureParameter.Add("@TYPE", S3G_Statu_Lookup.ORG_ROI_RULES_FREQUENCY.ToString());
            ddlFrequency.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "FREQUENCY_ID", "FREQUENCY_CODE" });

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Line of Business / Frequency");
        }
    }

    private void FunPriLoadFrequency()
    {
        try
        {
            if (objProcedureParameter != null)
                objProcedureParameter.Clear();
            else
                objProcedureParameter = new Dictionary<string, string>();
            
            objProcedureParameter.Clear();
            objProcedureParameter.Add("@OPTION", "35");
            objProcedureParameter.Add("@TYPE", S3G_Statu_Lookup.ORG_ROI_RULES_FREQUENCY.ToString());
            ddlFrequency.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "FREQUENCY_ID", "FREQUENCY_CODE" });

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Frequency");
        }
    }

    private void FunPriLoadBranchDetails(string strMonthYear)
    {
        try
        {
            DataTable dtBranchwise = new DataTable();
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@USER_ID", intUserID.ToString());
            objProcedureParameter.Add("@COMPANY_ID", intCompanyID.ToString());
            objProcedureParameter.Add("@LOB_ID", ddlLOB.SelectedValue);
            objProcedureParameter.Add("@START_DATE", Utility.StringToDate(txtStartDate.Text).ToString());
            objProcedureParameter.Add("@END_DATE", Utility.StringToDate(txtEndDate.Text).ToString());
            objProcedureParameter.Add("@Fin_Month", strMonthYear);
            DataSet dsBranchwise = Utility.GetDataset("S3G_LOANAD_Get_CC_Location", objProcedureParameter);
            gvBranchWise.DataSource = dsBranchwise.Tables[0];
            gvBranchWise.DataBind();
            pnlBranch.Visible = true;
            if (gvBranchWise.Rows.Count > 0)
            {
                btnSave.Enabled = true;
                btnPrint.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                btnPrint.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Location Details");
        }
    }

    #endregion

    #region Common Methods

    #region "User Authorization"

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;                        
                    }
                    break;
                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = false;
                    btnPrint.Enabled = true;
                    FunPriLoadBillingDetails();
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }


                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Toggling the Controls based on Mode");
        }
    }

    private void FunPriLoadBillingDetails()
    {
        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
        objProcedureParameter.Add("@CC_Header_Id", intBillingId.ToString());
        DataSet dsBilling = Utility.GetDataset("S3G_LOANAD_Get_CCDetails", objProcedureParameter);
      //  FunPriLoadLOV();
        ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dsBilling.Tables[0].Rows[0]["Lob_Name"].ToString(),dsBilling.Tables[0].Rows[0]["Lob_Id"].ToString()));
        ddlLOB.ToolTip = dsBilling.Tables[0].Rows[0]["Lob_Name"].ToString();
        //ddlLOB.ClearDropDownList();
        FunPriLoadFrequency();
        ddlFrequency.SelectedValue = dsBilling.Tables[0].Rows[0]["Frequency_Type"].ToString();
        ddlFrequency.ClearDropDownList();
        txtMonthYear.Text = dsBilling.Tables[0].Rows[0]["Month_Year"].ToString();
        calMonthYear.Enabled = false;
        txtStartDate.Text = dsBilling.Tables[0].Rows[0]["StartDate"].ToString();
        calStartDate.Enabled = false;
        txtEndDate.Text = dsBilling.Tables[0].Rows[0]["EndDate"].ToString();
        calEndDate.Enabled = false;
        btnGo1.Visible = false;
        rbtnSchedule.Enabled = false;
        calScheduleDate.Enabled = false;
        //txtScheduleDate.Text = dsBilling.Tables[0].Rows[0]["ScheduleDate"].ToString();
        txtScheduleTime.Attributes.Add("readonly", "true");
        //txtScheduleTime.Text = dsBilling.Tables[0].Rows[0]["ScheduleTime"].ToString();
        //txtScheduleTime.Text = Convert.ToDateTime(txtScheduleTime.Text).ToShortTimeString();

        if (dsBilling.Tables[1].Rows.Count > 0)
        {
            pnlBranch.Visible = true;
            gvBranchWise.DataSource = dsBilling.Tables[1];
            gvBranchWise.DataBind();
            btnPrint.Enabled = true;
        }
        else
        {
            pnlBranch.Visible = false;
            gvBranchWise.DataSource = null;
            gvBranchWise.DataBind();
            btnPrint.Enabled = false;
        }
        btnClear.Enabled = false;
    }
    ////Code end
    #endregion

    #endregion
    #endregion

}
