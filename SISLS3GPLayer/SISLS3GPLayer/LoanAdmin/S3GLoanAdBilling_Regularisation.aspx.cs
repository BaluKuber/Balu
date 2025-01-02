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
using System.ServiceProcess;
using System.Diagnostics;
using Microsoft.Win32;
using System.Drawing.Printing;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
#endregion

public partial class LoanAdmin_S3GClnReBilling : ApplyThemeForProject
{
    #region Variable declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> objProcedureParameter = null;
    int intErrorCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerializationMode = SerializationMode.Binary;
    public static LoanAdmin_S3GClnReBilling obj_Page = null;
    public string strDateFormat = string.Empty;
    static string strPageName = "Bill Generation";
    int intBillingId;
    private string strDocPath = "";
    string strErrorMessagePrefix = @"Please correct the following validation(s): </br></br>   ";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3GLoanAdTransLander.aspx?Code=RBILL";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdBilling_Regularisation.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=RBILL';";
    DataSet dsexcel = new DataSet();
    System.Data.DataTable dttarget;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    System.Data.DataTable dtdebit = new System.Data.DataTable();
    string[] sConsolidate_Name = null;
    //Code end
    #endregion

    #region Events

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //txtScheduleDate.Attributes.Add("readonly", "true");
            obj_Page = this;
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
            FunGetBillPDF();

            //if (ViewState["DocPath"] != null)
            //{
            //    string strMessage = "Generated Bills are available in Server (" + ViewState["DocPath"].ToString() + " Directory)";
            //    Utility.FunShowAlertMsg(this, strMessage);
            //    return;
            //}
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = "Unable to Clear the data";
            cvBilling.IsValid = false;
        }
    }


    // Code added by Santhosh.S on 10.07.2013 to export Gridview data to Excel Sheet
    protected void btnXLPorting_Click(object sender, EventArgs e)
    {
        try
        {
            FunProExport(grvAccounts, "Accounts");
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
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
            //Utility.FunShowAlertMsg(this, "Service is not running. Start the service and schedule");
            //return;


            bool chkServiceStatus = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ServiceStatus"));

            if (chkServiceStatus)
            {
                ServiceController sc = new ServiceController("SISLS3GWSBilling");
                if ((sc.Status.Equals(ServiceControllerStatus.Stopped)) ||
                    (sc.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    // Start the service if the current status is stopped.    sc.Start();    
                    Utility.FunShowAlertMsg(this, "Service is not running. Contact S3G Admin to start the service");
                    return;
                }
            }
            if (rbtnSchedule.SelectedValue == "0")
            {
                if (string.IsNullOrEmpty(txtScheduleDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "Select a Date for scheduling");
                    return;
                }
                if (string.IsNullOrEmpty(txtScheduleTime.Text))
                {
                    Utility.FunShowAlertMsg(this, "Enter the Time for scheduling");
                    return;
                }


                if (Utility.CompareDates(txtScheduleDate.Text, DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat)) == 1)
                {
                    Utility.FunShowAlertMsg(this, "Schedule Date should be greater than or equal to Current Date");
                    return;
                }
                if (txtScheduleTime.Text != "")
                {
                    DateTime st = DateTime.Parse((Utility.StringToDate(txtScheduleDate.Text).ToString()));
                    DateTime et = DateTime.Now;
                    TimeSpan span = new TimeSpan();
                    span = et.Subtract(st);
                    if ((span.Days) == 0)
                    {
                        DateTime tm = DateTime.Parse(txtScheduleTime.Text);
                        span = tm.Subtract(et);
                        if (span.Hours == 0 && span.Minutes < 4)
                        {
                            Utility.FunShowAlertMsg(this, "Schduled Time should be greater than or equal to 5 minutes for current time");
                            return;
                        }
                    }
                }
            }

           

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
           



            //if (ddlFrequency.SelectedItem.Text.ToUpper() == "MONTHLY")
            //{
            //    if (Utility.StringToDate(txtMonthYear.Text).Month != Utility.StringToDate(txtStartDate.Text).Month)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date Month should be equal to Month/Year");
            //        gvBranchWise.DataSource = null;
            //        gvBranchWise.DataBind();
            //        gvControlDataSheet.DataSource = null;
            //        gvControlDataSheet.DataBind();
            //        return;
            //    }
            //    if (Utility.StringToDate(txtMonthYear.Text).Month != Utility.StringToDate(txtEndDate.Text).Month)
            //    {
            //        Utility.FunShowAlertMsg(this, "End Date Month should be equal to Month/Year");
            //        gvBranchWise.DataSource = null;
            //        gvBranchWise.DataBind();
            //        gvControlDataSheet.DataSource = null;
            //        gvControlDataSheet.DataBind();
            //        return;
            //    }
            //}
            //if (Utility.CompareDates(txtStartDate.Text, txtEndDate.Text) != 1)
            //{
            //    Utility.FunShowAlertMsg(this, "End Date should be greater than Start Date");
            //    gvBranchWise.DataSource = null;
            //    gvBranchWise.DataBind();
            //    gvControlDataSheet.DataSource = null;
            //    gvControlDataSheet.DataBind();
            //    return;
            //}

            //if (ddlFrequency.SelectedItem.Text.ToUpper() == "MONTHLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays < 27)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than or equal to 28 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "WEEKLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays != 7)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 7 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "FORTNIGHTLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays != 15)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 15 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "BI MONTHLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays != 60)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 60 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "QUARTERLY")
            //{

            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays < 90)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 90 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "HALF YEARLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays < 181)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 181 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "ANNUALLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays < 365)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 365 days");
            //        return;
            //    }
            //}
            if (rbtnSchedule.SelectedValue == "0")
            {
                if (string.IsNullOrEmpty(txtScheduleDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "Select a Date for scheduling");
                    return;
                }
                if (string.IsNullOrEmpty(txtScheduleTime.Text))
                {
                    Utility.FunShowAlertMsg(this, "Enter the Time for scheduling");
                    return;
                }


                if (Utility.CompareDates(txtScheduleDate.Text, DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat)) == 1)
                {
                    Utility.FunShowAlertMsg(this, "Schedule Date should be greater than or equal to Current Date");
                    return;
                }
                if (txtScheduleTime.Text != "")
                {
                    DateTime st = DateTime.Parse((Utility.StringToDate(txtScheduleDate.Text).ToString()));
                    DateTime et = DateTime.Now;
                    TimeSpan span = new TimeSpan();
                    span = et.Subtract(st);
                    if ((span.Days) == 0)
                    {
                        DateTime tm = DateTime.Parse(txtScheduleTime.Text);
                        span = tm.Subtract(et);
                        if (span.Hours == 0 && span.Minutes < 4)
                        {
                            Utility.FunShowAlertMsg(this, "Schduled Time should be greater than or equal to 5 minutes for current time");
                            return;
                        }
                    }
                }
            }
            if (txtMonthYear.Text != "")
            {
               // txtBillMonthYear.Text = txtDataMonthYear.Text = txtMonthYear.Text;
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
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@MonthYear", strMonthYear);
            System.Data.DataTable dtMonthLock = Utility.GetDefaultData("s3g_loanad_CheckPrevMonthLock", objProcedureParameter);
            if (dtMonthLock.Rows.Count > 0)
            {
                if (dtMonthLock.Rows[0][0].ToString() == "True")
                {
                    Utility.FunShowAlertMsg(this, "Month/Year already Locked");
                    return;
                }
            }

           // txtDataFrequency.Text = txtBillFrequency.Text = ddlFrequency.SelectedItem.Text;
           // txtDataLOB.Text = txtBillLOB.Text = ddlLOB.SelectedItem.Text;
            FunPriLoadBranchDetails();
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
                    System.Web.UI.WebControls.CheckBox chkSelectAllBranch = e.Row.FindControl("chkSelectAllBranch") as System.Web.UI.WebControls.CheckBox;
                    chkSelectAllBranch.Checked = true;
                    chkSelectAllBranch.Enabled = false;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            System.Web.UI.WebControls.CheckBox chkSelectBranch = e.Row.FindControl("chkSelectBranch") as System.Web.UI.WebControls.CheckBox;
            System.Web.UI.WebControls.TextBox txtRemarks = e.Row.FindControl("txtRemarks") as System.Web.UI.WebControls.TextBox;
            if (Request.QueryString["qsMode"] != null)
            {
                if (Request.QueryString["qsMode"].ToString() == "Q")
                {
                    chkSelectBranch.Checked = true;
                    chkSelectBranch.Enabled = false;
                    txtRemarks.ReadOnly = true;
                }
            }
            System.Web.UI.WebControls.CheckBox chkSelectAllBranch = gvBranchWise.HeaderRow.FindControl("chkSelectAllBranch") as System.Web.UI.WebControls.CheckBox;
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
            //txtStartDate.Attributes.Add("readonly", "true");
            //txtEndDate.Attributes.Add("readonly", "true");
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            //calEndDate.Format = strDateFormat;
            //calStartDate.Format = strDateFormat;
            calScheduleDate.Format = strDateFormat;
            txtScheduleDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',false,  false);");
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                intBillingId = Convert.ToInt32(fromTicket.Name);
            }
            if (txtMonthYear.Text == "")
            {
               // txtBillMonthYear.Text = 
                //    txtMonthYear.Text =
                //txtDataMonthYear.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.Year;
            }
            if (!IsPostBack)
            {
                tcBilling.ActiveTabIndex = 0;

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (Request.QueryString["qsMode"] == "Q")
                {
                    FunPriDisableControls(-1);
                    gvControlDataSheet.Columns[4].Visible = true;
                }
                else
                {
                    FunPriLoadLOV();
                    tcBilling.Tabs[1].Enabled = tcBilling.Tabs[2].Enabled = false;
                    //btnBillPDF.Enabled = false;
                    FunPriDisableControls(0);
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
       // ddlFrequency.SelectedIndex = 0;
        txtStartDate.Text = txtEndDate.Text = txtScheduleDate.Text = txtScheduleTime.Text = txtDataLOB.Text = txtMonthYear.Text = "";
        //txtBillFrequency.Text = txtBillLOB.Text = txtDataFrequency.Text = "";
        //pnlBranch.Visible = false;
        //gvBranchWise.DataSource = null;
        //gvBranchWise.DataBind();
        pnlControlData.Visible = false;
        gvControlDataSheet.DataSource = null;
        gvControlDataSheet.DataBind();
        btnSave.Enabled = false;
        if (txtMonthYear.Text == "")
        {
            //txtBillMonthYear.Text =
                txtMonthYear.Text =
            txtDataMonthYear.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.Year;
        }
        if (ddlLOB.SelectedIndex > 0)
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@LobId", ddlLOB.SelectedValue);
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            System.Data.DataTable dtDocPath = Utility.GetDefaultData("s3g_LoanAd_GetBillingDocPath", objProcedureParameter);
            if (dtDocPath.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Define the Document Path for Billing in Document Path Setup");
                return;
            }
            else
            {
                ViewState["DocPath"] = strDocPath = dtDocPath.Rows[0]["Document_Path"].ToString();
            }

           
        }
        
        
    }

   

    private void FunGetBillPDF()
    {
        LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objServiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        BillingEntity objBillingEntity = new BillingEntity();
        try
        {
            objBillingEntity.intCompanyId = intCompanyID;
            objBillingEntity.dtScheduleDate = DateTime.Now;
            objBillingEntity.strScheduleTime = DateTime.Now.AddMinutes(5.0).ToShortTimeString();
            objBillingEntity.intFrequency = intBillingId;
            objBillingEntity.intUserId = intUserID;
            intErrorCode = objServiceClient.FunPubGetPDF(objBillingEntity);
            if (intErrorCode == 0)
            {
                if (ViewState["DocPath"] != null)
                {
                    string strMessage = "Generated Bills are available in Server after " + objBillingEntity.strScheduleTime + "(" + ViewState["DocPath"].ToString() + " Directory)";
                    Utility.FunShowAlertMsg(this, strMessage);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Get Bill Details");
        }
        finally
        {
            objServiceClient.Close();
            objBillingEntity = null;
        }
    }


    private void FunPriSaveRecord()
    {
        LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objServiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();

        //        ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient objServiceClient = new ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient();
        BillingEntity objBillingEntity = new BillingEntity();
        try
        {


            System.Web.UI.WebControls.CheckBox chkSelectAll = gvBranchWise.HeaderRow.FindControl("chkSelectAllBranch") as System.Web.UI.WebControls.CheckBox;
            int intSelectedBranchCount = 0;
            if (!chkSelectAll.Checked)
            {
                foreach (GridViewRow grBranch in gvBranchWise.Rows)
                {
                    if (grBranch.RowType == DataControlRowType.DataRow)
                    {
                        System.Web.UI.WebControls.CheckBox chkSelect = grBranch.FindControl("chkSelectBranch") as System.Web.UI.WebControls.CheckBox;
                        if (chkSelect.Checked)
                        {
                            intSelectedBranchCount += 1;
                        }
                    }
                }
                if (intSelectedBranchCount == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atleast one Location for Bill Generation");
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
            string strXmlBranchDetails = FunPriFormXml(gvBranchWise, true);
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@XmlBranchDetails", strXmlBranchDetails);
            objProcedureParameter.Add("@MonthYear", strMonthYear);
            System.Data.DataTable dtMonthLock = Utility.GetDefaultData("s3g_loanad_CheckPrevMonthLock", objProcedureParameter);
            bool blnIsUnLockBranch = false;

            foreach (DataRow drBranchMonth in dtMonthLock.Rows)
            {
                if (drBranchMonth["Month_Lock"].ToString() == "False")
                {
                    blnIsUnLockBranch = true;
                }
            }
            if (blnIsUnLockBranch || dtMonthLock.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, Utility.StringToDate(txtMonthYear.Text).AddMonths(-3).ToString("MMM")+" -"+ Utility.StringToDate(txtMonthYear.Text).ToString("yyyy") +" should be locked");
                return;
            }

            // Added By C.Aswinkrishna on 7-Mar-2016 Start//
            string strLocCode = string.Empty;

            Dictionary<string, string> dictparam = new Dictionary<string, string>();

            for (int i = 0; i < gvBranchWise.Rows.Count; i++)
            {
                if (((CheckBox)gvBranchWise.Rows[i].FindControl("chkSelectBranch")).Checked)
                {
                    ViewState["Location_Code"] = strLocCode += ((Label)gvBranchWise.Rows[i].FindControl("lblBranchId")).Text + ",";

                }
            }

            string str = "01" + txtMonthYear.Text;
            DateTime dt = Utility.StringToDate(str);
            string strresult = string.Empty;
            dictparam.Add("@Codes", ViewState["Location_Code"].ToString().Substring(0, ViewState["Location_Code"].ToString().LastIndexOf(",")));
            // Bug Fixing - 5913 - Start
            //dictparam.Add("@month", dt.Year.ToString() + (((dt.Month - 1).ToString().Length < 2 ? ("0" + (Convert.ToInt32(dt.Month) - 1).ToString()) : ((Convert.ToInt32(dt.Month) - 1).ToString()))));
            if (Convert.ToInt32(dt.Month) == 1 )
                dictparam.Add("@month", (dt.Year - 1).ToString() + ("12"));
            else
                dictparam.Add("@month", dt.Year.ToString() + (((dt.Month - 1).ToString().Length < 2 ? ("0" + (Convert.ToInt32(dt.Month) - 1).ToString()) : ((Convert.ToInt32(dt.Month) - 1).ToString()))));
            // Bug Fixing - 5913 - End
            DataTable dtcheck = Utility.GetDefaultData("S3G_LOANAD_AccountsForBilling", dictparam);

            foreach (DataRow dr in dtcheck.Rows)
            {
                strresult += dr["Location"];
            }


            //if (dtcheck.Rows.Count > 0)
            //{
            //    Utility.FunShowAlertMsg(this, ("Demand not run for previous month for " + strresult.Substring(0, strresult.Length - 1)));

            //    return;
            //}

            // Added By C.Aswinkrishna on 7-Mar-2016 End//

            objBillingEntity.intCompanyId = intCompanyID;
            objBillingEntity.intLobId = Convert.ToInt32(ddlLOB.SelectedValue);
            objBillingEntity.intFrequency = 0;
            objBillingEntity.intBranchId = 0;

            objBillingEntity.lngMonthYear = Convert.ToInt64(strMonthYear);
            objBillingEntity.intUserId = intUserID;
            objBillingEntity.Is_Regular = "R";
            objBillingEntity.dtStartDate = Utility.StringToDate(txtStartDate.Text);
            objBillingEntity.dtEndDate = Utility.StringToDate(txtEndDate.Text);
            objBillingEntity.dtBillingDate = Utility.StringToDate(DateTime.Now.ToString());
            if (rbtnSchedule.SelectedValue == "0")
            {
                objBillingEntity.strScheduleTime = txtScheduleTime.Text;
            }
            else
            {
                txtScheduleDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
                objBillingEntity.strScheduleTime = DateTime.Now.AddMinutes(5.0).ToShortTimeString();

            }
            objBillingEntity.dtScheduleDate = Utility.StringToDate(txtScheduleDate.Text);
            objBillingEntity.strXmlBranchDetails = strXmlBranchDetails;
            objBillingEntity.strXmlControlDataDetails = gvControlDataSheet.FunPubFormXml(true);
            //objBillingEntity.strXmlCashFlowDetails = FunPriFormCFXml(grvCashFlow, true); //grvCashFlow.FunPubFormXml(true);
            string strJournalMessage = "";
            intErrorCode = objServiceClient.FunPubCreateBillingInt(out strJournalMessage, objBillingEntity);
            //intErrorCode = objServiceClient.FunPubCreateBillingInt(out strJournalMessage, objBillingEntity);
            if (intErrorCode == 0)
            {
                strAlert = "Billing Generated successfully";
                strAlert += @"\n\nWould you like to generate one more Bill ?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else if (intErrorCode == 53)
            {
                Utility.FunShowValidationMsg(this.Page, "", intErrorCode);
                return;
            }
            else if (intErrorCode == 13)
            {
                Utility.FunShowAlertMsg(this, "Month is not open in GPS");
                return;
            }
            else if (intErrorCode == 20)
            {
                Utility.FunShowAlertMsg(this, "Cash Flow Master Not defined");
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
            throw new ApplicationException("Due to Data Problem,Unable to Insert Billing Details");
        }
        finally
        {
            objServiceClient.Close();
            objBillingEntity = null;
        }
    }
    private string FunPriFormXml(GridView grvXml, bool IsNeedUpperCase)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {
            System.Web.UI.WebControls.CheckBox chkSelect = grvRow.FindControl("chkSelectBranch") as System.Web.UI.WebControls.CheckBox;
            System.Web.UI.WebControls.CheckBox chkSelectAll = grvXml.HeaderRow.FindControl("chkSelectAllBranch") as System.Web.UI.WebControls.CheckBox;
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
                                    strColValue = ((System.Web.UI.WebControls.TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                {
                                    strColValue = ((System.Web.UI.WebControls.CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                {
                                    strColValue = ((System.Web.UI.WebControls.Label)grvRow.Cells[intcolcount].Controls[1]).Text;
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

    private string FunPriFormCFXml(GridView grvXml, bool IsNeedUpperCase)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {
            System.Web.UI.WebControls.CheckBox chkSelect = grvRow.FindControl("chkSelectCF") as System.Web.UI.WebControls.CheckBox;
            System.Web.UI.WebControls.CheckBox chkSelectAll = grvXml.HeaderRow.FindControl("chkSelectAllCF") as System.Web.UI.WebControls.CheckBox;
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
                                    strColValue = ((System.Web.UI.WebControls.TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                {
                                    strColValue = ((System.Web.UI.WebControls.CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                {
                                    strColValue = ((System.Web.UI.WebControls.Label)grvRow.Cells[intcolcount].Controls[1]).Text;
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
            Response.Redirect(strRedirectPage, false);
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
            ddlLOB.SelectedIndex =  0;
            txtStartDate.Text = txtEndDate.Text = txtScheduleDate.Text = txtScheduleTime.Text = txtDataLOB.Text = txtMonthYear.Text = "";
           // txtBillFrequency.Text = txtBillLOB.Text = txtDataFrequency.Text = "";
           // pnlBranch.Visible = false;
            gvBranchWise.DataSource = null;
            gvBranchWise.DataBind();
            pnlControlData.Visible = false;
            gvControlDataSheet.DataSource = null;
            gvControlDataSheet.DataBind();
            btnSave.Enabled = false;
            if (txtMonthYear.Text == "")
            {
               // txtBillMonthYear.Text = 
                    txtMonthYear.Text =
                txtDataMonthYear.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.Year;
            }
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
            objProcedureParameter.Add("@ProgramId", "95");
            if (Request.QueryString["qsMode"] == "Q")
            {
                objProcedureParameter.Add("@TYPE", "Q");
            }
            ddlLOB.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.SelectedIndex = 1;
            ddlLOB.ClearDropDownList();
            objProcedureParameter.Clear();
            objProcedureParameter.Add("@OPTION", "2");
            objProcedureParameter.Add("@TYPE", S3G_Statu_Lookup.ORG_ROI_RULES_FREQUENCY.ToString());
            //ddlFrequency.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "FREQUENCY_ID", "FREQUENCY_CODE" });

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
            objProcedureParameter.Clear();
            objProcedureParameter.Add("@OPTION", "2");
            objProcedureParameter.Add("@TYPE", S3G_Statu_Lookup.ORG_ROI_RULES_FREQUENCY.ToString());
            //ddlFrequency.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "FREQUENCY_ID", "FREQUENCY_CODE" });
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Line of Business / Frequency");
        }
    }





    private void FunPriLoadBranchDetails()
    {
        try
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

            System.Data.DataTable dtBranchwise = new System.Data.DataTable();
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@USERID", intUserID.ToString());
            objProcedureParameter.Add("@OPTION", "1");
            objProcedureParameter.Add("@COMPANY_ID", intCompanyID.ToString());
            objProcedureParameter.Add("@LOBID", ddlLOB.SelectedValue);
            objProcedureParameter.Add("@STARTDATE", Utility.StringToDate(txtStartDate.Text).ToString());
            objProcedureParameter.Add("@ENDDATE", Utility.StringToDate(txtEndDate.Text).ToString());
            objProcedureParameter.Add("@month", strMonthYear);
            objProcedureParameter.Add("@is_Regular", "Y");
            //objProcedureParameter.Add("@type", Utility.StringToDate(txtEndDate.Text).ToString());
            DataSet dsBranchwise = Utility.GetDataset("S3G_CLN_LOADBILLINGBRANCH", objProcedureParameter);
            //if (dsBranchwise.Tables[0] != null && dsBranchwise.Tables[0].Rows.Count>0 && dsBranchwise.Tables[0].Rows[0]["count"].ToString() == "0")
            //{
            //    Utility.FunShowAlertMsg(this, "Demand should be run for previous month");
            //    return;
            //}
            gvBranchWise.DataSource = dsBranchwise.Tables[0];
            gvBranchWise.DataBind();
            Session["dttarget"] = dsBranchwise.Tables[0];
            FunPriFindTotal();
            pnlBranch.Visible = true;
            if (gvBranchWise.Rows.Count > 0)
            {
                btnSave.Enabled = true;

            }
            else
            {
                btnSave.Enabled = false;
            }
            pnlControlData.Visible = true;  
            //gvControlDataSheet.DataSource = dsBranchwise.Tables[1];
            //gvControlDataSheet.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Location Details");
        }
    }
    private string Funsetsuffix()
    {
        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    private void FunPriFindTotal()
    {
        try
        {
            if (Session["dttarget"] != null)
                dttarget = (System.Data.DataTable)Session["dttarget"];
            if (dttarget.Rows.Count > 0)
            {

                Label lbltotaccountcount = (Label)(gvBranchWise).FooterRow.FindControl("lbltotaccountcount") as Label;
                lbltotaccountcount.Text = Convert.ToDecimal(dttarget.Compute("sum(ACCOUNTCOUNT)", "")).ToString();

                Label lbltotopcamount = (Label)(gvBranchWise).FooterRow.FindControl("lbltotopcamount") as Label;
                lbltotopcamount.Text = Convert.ToDecimal(dttarget.Compute("sum(DEBITAMOUNT1)", "")).ToString();

                Label lbltotFunderamt = (Label)(gvBranchWise).FooterRow.FindControl("lbltotFunderamt") as Label;
                lbltotFunderamt.Text = Convert.ToDecimal(dttarget.Compute("sum(funder_amt1)", "")).ToString();

            }
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = "Due to Data Problem,Unable to Move Invoices.";
            cvBilling.IsValid = false;
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
                    //btnXLPorting.Enabled = false;

                    break;
                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = false;
                    btnGonw.Visible = false;
                    FunPriLoadBillingDetails();
                    txtStartDate.ReadOnly = txtEndDate.ReadOnly = txtScheduleDate.ReadOnly = true;
                    txtScheduleDate.Attributes.Remove("onblur");
                    txtStartDate.Attributes.Remove("onblur");
                    txtEndDate.Attributes.Remove("onblur");
                    //btnXLPorting.Enabled = false;
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
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
        objProcedureParameter.Add("@BillingId", intBillingId.ToString());
        DataSet dsBilling = Utility.GetDataset("s3g_loanad_RGetBillingDetails", objProcedureParameter);
        //FunPriLoadLOV();
        ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dsBilling.Tables[0].Rows[0]["Lob_Name"].ToString(),dsBilling.Tables[0].Rows[0]["Lob_Id"].ToString()));
        ddlLOB.ToolTip = dsBilling.Tables[0].Rows[0]["Lob_Name"].ToString();
        // ddlLOB.ClearDropDownList();
        //FunProLoadCahsFlows();
       
        //ddlFrequency.Items.Add(new System.Web.UI.WebControls.ListItem(dsBilling.Tables[0].Rows[0]["FREQUENCY_Name"].ToString(), dsBilling.Tables[0].Rows[0]["Frequency_Type"].ToString()));
        //ddlFrequency.ToolTip = dsBilling.Tables[0].Rows[0]["FREQUENCY_Name"].ToString();
        FunPriLoadFrequency();

        //ddlFrequency.SelectedValue = dsBilling.Tables[0].Rows[0]["Frequency_Type"].ToString();
        //ddlFrequency.ClearDropDownList();

        txtMonthYear.Text = dsBilling.Tables[0].Rows[0]["Month_Year"].ToString();
        calMonthYear.Enabled = false;
        //txtBillMonthYear.Text = 
            txtDataMonthYear.Text = txtMonthYear.Text;
        txtStartDate.Text = dsBilling.Tables[0].Rows[0]["StartDate"].ToString();
        //calStartDate.Enabled = false;
        txtEndDate.Text = dsBilling.Tables[0].Rows[0]["EndDate"].ToString();
        //calEndDate.Enabled = false;
        btnGonw.Visible = false;
        rbtnSchedule.Enabled = false;
        calScheduleDate.Enabled = false;
        txtScheduleDate.Text = dsBilling.Tables[0].Rows[0]["ScheduleDate"].ToString();
        txtScheduleTime.Attributes.Add("readonly", "true");
        txtScheduleTime.Text = dsBilling.Tables[0].Rows[0]["ScheduleTime"].ToString();
        txtScheduleTime.Text = Convert.ToDateTime(txtScheduleTime.Text).ToShortTimeString();
        //objProcedureParameter = new Dictionary<string, string>();
        //objProcedureParameter.Add("@LobId", ddlLOB.SelectedValue);
        //objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
        //DataTable dtDocPath = Utility.GetDefaultData("s3g_LoanAd_GetBillingDocPath", objProcedureParameter);
        if (dsBilling.Tables[4].Rows.Count > 0)
        {
            ViewState["DocPath"] = strDocPath = dsBilling.Tables[4].Rows[0]["Document_Path"].ToString();
        }
        if (dsBilling.Tables[1].Rows.Count > 0)
        {
            pnlBranch.Visible = true;
            gvBranchWise.DataSource = dsBilling.Tables[1];
            gvBranchWise.DataBind();
            Session["dttarget"] = dsBilling.Tables[1];
            if (Session["dttarget"] != null)
                dttarget = (System.Data.DataTable)Session["dttarget"];
            if (dttarget.Rows.Count > 0)
            {
                Label lbltotaccountcount = (Label)(gvBranchWise).FooterRow.FindControl("lbltotaccountcount") as Label;
                lbltotaccountcount.Text = Convert.ToDecimal(dttarget.Compute("sum(ACCOUNTCOUNT)", "")).ToString();

                Label lbltotopcamount = (Label)(gvBranchWise).FooterRow.FindControl("lbltotopcamount") as Label;
                lbltotopcamount.Text = Convert.ToDecimal(dttarget.Compute("sum(DEBITAMOUNT1)", "")).ToString(Funsetsuffix());

                Label lbltotFunderamt = (Label)(gvBranchWise).FooterRow.FindControl("lbltotFunderamt") as Label;
                lbltotFunderamt.Text = Convert.ToDecimal(dttarget.Compute("sum(funder_amt1)", "")).ToString(Funsetsuffix());

            }
        }
        else
        {
            pnlBranch.Visible = false;
            gvBranchWise.DataSource = null;
            gvBranchWise.DataBind();
        }
       // txtBillFrequency.Text = txtDataFrequency.Text = ddlFrequency.SelectedItem.Text;
       // txtDataLOB.Text = txtBillLOB.Text = ddlLOB.SelectedItem.Text;
        if (dsBilling.Tables[2].Rows.Count > 0)
        {
            pnlControlData.Visible = true;
            gvControlDataSheet.DataSource = dsBilling.Tables[2];
            gvControlDataSheet.DataBind();
        }
        else
        {
            pnlControlData.Visible = false;
            gvControlDataSheet.DataSource = null;
            gvControlDataSheet.DataBind();
        }
       // btnBillPDF.Enabled = true;
        btnClear.Enabled = false;
    }
    ////Code end
    #endregion

    #endregion
    #endregion

    protected bool FunProCheckLOB()
    {
        if (ddlLOB.SelectedItem.Text.StartsWith("TE") || ddlLOB.SelectedItem.Text.StartsWith("TL") || ddlLOB.SelectedItem.Text.StartsWith("WC"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void gvControlDataSheet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@Billing_Control_Id", e.CommandArgument.ToString());
        System.Data.DataTable dtAccounts = Utility.GetDefaultData("S3G_LOANAD_GetBillingAccounts", objProcedureParameter);

        if (dtAccounts.Rows.Count == 0)
        {
            DataRow dRow = dtAccounts.NewRow();
            dtAccounts.Rows.Add(dRow);

            grvAccounts.DataSource = dtAccounts;
            grvAccounts.DataBind();

            grvAccounts.Rows[0].Visible = false;
            btnXLPorting.Enabled = false;
        }
        else
        {
            //btnXLPorting.Enabled = true;
            grvAccounts.DataSource = dtAccounts;
            grvAccounts.DataBind();
            btnXLPorting.Enabled = true;
        }




    }


    // Code added by Santhosh.S on 10.07.2013 to export Gridview data to Excel Sheet
    protected void FunProExport(GridView Grv, string FileName)
    {
        try
        {
            //Type ExcellType = Type.GetTypeFromProgID("Excel.Application");
            //if (ExcellType == null)
            //{
            //Utility.FunShowAlertMsg(this, "Cannot export file. MS-Excel is not istalled in this System.");
            //return;
            //}
            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=" + FileName + ".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
                
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Export into Excel");
        }
    }

    protected void btnFetch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlcust.SelectedValue.ToString() == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Select Customer");
                return;
            }
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@BillingId", intBillingId.ToString());
            objProcedureParameter.Add("@customer_id", ddlcust.SelectedValue);
            dtdebit = Utility.GetDefaultData("S3g_loanad_bill_Debit", objProcedureParameter);
            if (dtdebit.Rows.Count > 0)
            {
                ViewState["dtdebit"] = dtdebit;
                pnlrs.Visible = true;
                divacc.Style.Add("display", "block");
                grvRental.DataSource = dtdebit;
                grvRental.DataBind();
                btnExcel.Visible = true;
                btncredit.Visible = true;
                btnDebitPrint.Visible = true;
                btnCreditPrint.Visible = true;

            }
            else
            {
                pnlrs.Visible = true;
                divacc.Style.Add("display", "block");
                grvRental.EmptyDataText = "No records Found";
                grvRental.DataBind();
                btnExcel.Visible = false;
                btncredit.Visible = false;
                btnDebitPrint.Visible = false;
                btnCreditPrint.Visible = false;
            }
            //dttranche = dsaccounts.Tables[1];
            //ViewState["dttranche"] = dttranche;
            //pnlTranche.Visible = true;
            //divtranche.Style.Add("display", "block");
            //grvtranche.DataSource = dttranche;
            //grvtranche.DataBind();


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Location Details";
            cvBilling.IsValid = false;
        }

    }
  

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@BillingId", intBillingId.ToString());
            objProcedureParameter.Add("@customer_id", ddlcust.SelectedValue);
            objProcedureParameter.Add("@option", "1");
            dsexcel = Utility.GetDataset("S3g_loanad_bill_Debit", objProcedureParameter);
            if (dsexcel.Tables[0].Rows.Count > 0)
            {
                GridView Grv = new GridView();
                Grv.DataSource = dsexcel.Tables[0];
                Grv.DataBind();
                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;

                if (Grv.Rows.Count > 0)
                {
                    string attachment = "attachment; filename=Sale Incoice Register For AMF.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.xlsx";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    GridView grv1 = new GridView();
                    System.Data.DataTable dtHeader = new System.Data.DataTable();
                    dtHeader.Columns.Add("Column1");

                    DataRow row = dtHeader.NewRow();
                    row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                    dtHeader.Rows.Add(row);



                    row = dtHeader.NewRow();
                    dtHeader.Rows.Add(row);
                    grv1.DataSource = dtHeader;
                    grv1.DataBind();

                    grv1.HeaderRow.Visible = false;
                    grv1.GridLines = GridLines.None;

                    grv1.Rows[0].Cells[0].ColumnSpan = 9;


                    grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;

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
            else
            {
                Utility.FunShowAlertMsg(this.Page, "No records to export");
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Location Details";
            cvBilling.IsValid = false;
        }

    }

    protected void btncredit_Click(object sender, EventArgs e)
    {
        try
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@BillingId", intBillingId.ToString());
            objProcedureParameter.Add("@customer_id", ddlcust.SelectedValue);
            objProcedureParameter.Add("@option", "1");
            dsexcel = Utility.GetDataset("S3g_loanad_bill_Debit", objProcedureParameter);
            if (dsexcel.Tables[1].Rows.Count > 0)
            {
                GridView Grv = new GridView();
                Grv.DataSource = dsexcel.Tables[1];
                Grv.DataBind();
                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;

                if (Grv.Rows.Count > 0)
                {
                    string attachment = "attachment; filename=Sale Incoice Register For AMF.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.xlsx";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    GridView grv1 = new GridView();
                    System.Data.DataTable dtHeader = new System.Data.DataTable();
                    dtHeader.Columns.Add("Column1");

                    DataRow row = dtHeader.NewRow();
                    row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                    dtHeader.Rows.Add(row);





                    row = dtHeader.NewRow();
                    dtHeader.Rows.Add(row);
                    grv1.DataSource = dtHeader;
                    grv1.DataBind();

                    grv1.HeaderRow.Visible = false;
                    grv1.GridLines = GridLines.None;

                    grv1.Rows[0].Cells[0].ColumnSpan = 9;


                    grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;

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
            else
            {
                Utility.FunShowAlertMsg(this.Page, "No records to export");
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Location Details";
            cvBilling.IsValid = false;
        }

    }
    protected void btnDebitPrint_Click(object sender, EventArgs e)
    {
        try
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@BillingId", intBillingId.ToString());
            objProcedureParameter.Add("@customer_id", ddlcust.SelectedValue);
            objProcedureParameter.Add("@option", "1");
            dsexcel = Utility.GetDataset("S3g_loanad_bill_Debit", objProcedureParameter);
            if (dsexcel.Tables[2].Rows.Count > 0)
            {
                Guid objGuid;
                objGuid = Guid.NewGuid();
                //ReportDocument rpd = new ReportDocument();

                //rpd.Load(Server.MapPath("Debit.rpt"));

                //rpd.SetDataSource(dsexcel.Tables[2]);
                

                string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + objGuid.ToString() + ".pdf";

                string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

                if (!(System.IO.Directory.Exists(strFolder)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strFolder);

                }
                //if (File.Exists(strFileName) == true)
                //{
                //    File.Delete(strFileName);
                //}

                //Document doc = new Document();

                //PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strFileName, FileMode.Create));

                string strScipt = "";
                //rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                strScipt = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strFileName.Replace(@"\", "/") +  "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);


            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "No records to export");
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Location Details";
            cvBilling.IsValid = false;
        }

    }
    protected void btnCreditPrint_Click(object sender, EventArgs e)
    {
        try
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@BillingId", intBillingId.ToString());
            objProcedureParameter.Add("@customer_id", ddlcust.SelectedValue);
            objProcedureParameter.Add("@option", "1");
            dsexcel = Utility.GetDataset("S3g_loanad_bill_Debit", objProcedureParameter);
            if (dsexcel.Tables[3].Rows.Count > 0)
            {
                Guid objGuid;
                objGuid = Guid.NewGuid();
                //ReportDocument rpd = new ReportDocument();

                //rpd.Load(Server.MapPath("Debit.rpt"));

                //rpd.SetDataSource(dsexcel.Tables[3]);


                string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + objGuid.ToString() + ".pdf";

                string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

                if (!(System.IO.Directory.Exists(strFolder)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strFolder);

                }
                //if (File.Exists(strFileName) == true)
                //{
                //    File.Delete(strFileName);
                //}

                //Document doc = new Document();

                //PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strFileName, FileMode.Create));

                string strScipt = "";
                //rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                strScipt = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);


            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "No records to export");
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Location Details";
            cvBilling.IsValid = false;
        }

    }


    //protected void Print_click(System.Data.DataSet dsprint, string[] sConsolidate_Name)
    //{
    //    ApplicationClass excelApplicationClass = new ApplicationClass();
    //    _Workbook finalWorkbook = null;
    //    Workbook workBook = null;
    //    Workbooks workbooks = null;
    //    Worksheet workSheet = null;
    //    Worksheet newWorksheet = null;

    //    System.Data.DataTable dt = new System.Data.DataTable();
    //    string FilePath = CreateExcel().ToString();
    //    try
    //    {
    //        //if (OS_Consolidate_File.ToString() == "")
    //        //{
    //        System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
    //        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
    //        workbooks = excelApplicationClass.Workbooks;

    //        finalWorkbook = excelApplicationClass.Workbooks.Open(FilePath, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
    //            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

    //        for (int i = 0; i < sConsolidate_Name.Length; i++)
    //        {

    //            int countWorkSheet = finalWorkbook.Worksheets.Count;

    //            //  finalWorkbook = books.Add();     
    //            newWorksheet = (Worksheet)finalWorkbook.Sheets[i + 1];

    //            newWorksheet.Name = sConsolidate_Name[i];

    //            //Open the source WorkBook

    //            newWorksheet = GenerateWorkSheet(newWorksheet, dsprint.Tables[i]);

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        if (finalWorkbook != null)
    //        {
    //            finalWorkbook.Close(true, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
    //        }

    //        if (workBook != null)
    //            workBook.Close(true, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

    //        if (workSheet != null)
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

    //        workSheet = null;

    //        if (workBook != null)
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);

    //        if (finalWorkbook != null)
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(finalWorkbook);

    //        workBook = null;

    //        if (excelApplicationClass != null)
    //        {
    //            excelApplicationClass.Quit();
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApplicationClass);
    //            excelApplicationClass = null;
    //        }
    //    }
    //    try
    //    {
    //        Response.Clear();
    //        Response.AppendHeader("content-disposition", "attachment; filename=" + FilePath + "");
    //        Response.ContentType = "application/octet-stream";
    //        Response.WriteFile(FilePath);
    //        Response.End();

    //    }
    //    catch (Exception ex)
    //    {
    //        //    ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
    //        throw ex;
    //    }
    //}

    //private string CreateExcel()
    //{
    //    Application app = null;
    //    Workbooks books = null;
    //    Workbook book = null;
    //    Sheets sheets = null;
    //    Worksheet sheet = null;
    //    Range range = null;
    //    string sFilePath;
    //    try
    //    {
    //        app = new Application();
    //        books = app.Workbooks;

    //        book = app.Workbooks.Add(System.Reflection.Missing.Value);

    //        //book.Worksheets.Add(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
    //        //book.Worksheets.Add(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
    //        ////book = books.Add(System.Reflection.Missing.Value);
    //        //app.Workbooks.Add(3);
    //        sFilePath = Server.MapPath(".") + "\\PDF Files\\DN" + System.DateTime.Today.ToString("yyyyMMdd") + DateTime.Now.Millisecond + ".xls";
    //        book.SaveAs(sFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12,
    //System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false,
    //Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, System.Reflection.Missing.Value,
    //System.Reflection.Missing.Value, System.Reflection.Missing.Value);
    //        book.Close(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
    //        app.Quit();
    //    }
    //    finally
    //    {
    //        if (range != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
    //        if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
    //        if (sheets != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
    //        if (book != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
    //        if (books != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
    //        if (app != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
    //    }
    //    return sFilePath;
    //}

    //protected Worksheet GenerateWorkSheet(Worksheet worksheet, System.Data.DataTable dt)
    //{
    //    try
    //    {
    //        Range range;
    //        long totalCount = dt.Rows.Count;
    //        long rowRead = 0;
    //        float percent = 0;
    //        for (int i = 0; i < dt.Columns.Count; i++)
    //        {
    //            worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
    //            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
    //            range.Interior.ColorIndex = 41;
    //            range.Font.Bold = true;
    //            range.Font.ColorIndex = 2;

    //        }

    //        for (int r = 0; r < dt.Rows.Count; r++)
    //        {
    //            for (int i = 0; i < dt.Columns.Count; i++)
    //            {
    //                worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
    //            }
    //            rowRead++;
    //            percent = ((float)(100 * rowRead)) / totalCount;
    //        }

    //        return worksheet;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    //finally
    //    //{
    //    //    if (workbook != null)
    //    //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
    //    //    worksheet = null;
    //    //    if (workbook != null)
    //    //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
    //    //    workbook = null;
    //    //}
    //}

    [System.Web.Services.WebMethod]
    public static string[] GetCustList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@OPTION", "1");
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));
        return suggetions.ToArray();
    }


    // Code added by C.Aswinkrishna on 5-Feb-2016 Start //
    protected void txtMonthYear_TextChanged(object sender, EventArgs e)
    {

        string str = "01" + txtMonthYear.Text;
        DateTime dt = Utility.StringToDate(str);

        if (DateTime.Now.Month < dt.Month && (DateTime.Now.Year <= dt.Year || dt.Year>DateTime.Now.Year ))
        {
            Utility.FunShowAlertMsg(this, "Selected Month should not be a future month");
            txtMonthYear.Text = txtStartDate.Text = txtEndDate.Text = string.Empty;
            pnlBranch.Visible = false;
            return;
        }

        DateTime dtstartdate = new DateTime(Convert.ToInt32(((TextBox)sender).Text.Substring(((TextBox)sender).Text.IndexOf('-') + 1)), dt.Month, 1);

        txtStartDate.Text = dtstartdate.ToString("dd-MMM-yyyy");

        DateTime dtenddate = dtstartdate.AddMonths(1).AddDays(-1);

        txtEndDate.Text = dtenddate.ToString("dd-MMM-yyyy");
    }

    // Code added by C.Aswinkrishna on 5-Feb-2016 End //
}
