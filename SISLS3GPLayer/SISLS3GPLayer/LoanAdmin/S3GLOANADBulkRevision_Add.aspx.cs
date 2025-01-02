#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Loan Admin
/// Screen Name         :   Transaction Lander
/// Created By          :   Y.Nataraj
/// Created Date        :   12-Oct-2010
/// Purpose             :   Bulk Revision Screen
/// Last Updated By		:   Nataraj Y
/// Last Updated Date   :   29-Dec-2010
/// Reason              :   NULL
/// <Program Summary>
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
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
using S3GBusEntity;
using System.Globalization;
using S3GBusEntity.LoanAdmin;
using System.ServiceModel;
using System.ServiceProcess;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Text;
using System.Net.Mail;

public partial class LoanAdmin_S3GLOANADBulkRevision_Add : ApplyThemeForProject
{
    #region Variable declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    Dictionary<string, string> Procparam;
    //int intCompany_Id;
    //int intUserId;
    int intResult;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bClearList = false;
    //public string strDateFormat;
    static string strMode;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=BURE';";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3gLoanAdBulkRevision_Add.aspx?qsMode=C';";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=BURE";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    ContractMgtServicesReference.ContractMgtServicesClient ObjBulkRevisionClient;
    ContractMgtServices.S3G_LOANAD_BulkRevisionDataTable ObjS3G_LOANAD_BulkRevisionDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        CalendarExtender1.Format = DateFormate;
        CalendarExtender2.Format = DateFormate;
        txtRevisionDate.Attributes.Add("readonly", "readonly");
        txtScheduleDate.Attributes.Add("readonly", "readonly");
        txtRevisionEffectiveDate.Attributes.Add("readonly", "readonly");
        txtRevisionNumber.Attributes.Add("readonly", "readonly");
        txtRevisionImpactPercent.Attributes.Add("readonly", "readonly");
        if (!IsPostBack)
        {
            intRecordsCount = 0;
            txtInterveningPeriodIntrest.Text = "";
            txtRevisionDate.Text = DateTime.Now.ToString(DateFormate);
            txtRevisionEffectiveDate.Text = DateTime.Now.ToString(DateFormate);
            if (PageMode == PageModes.Create)
            FunPriLoadCombos();
            //grvBranchDetails.Visible = false;
            //pnlBranchDetails.Visible = false;
            btnSave.Enabled = false;
            btnProcess.Visible = false;
            FunPriBulkRevControlStatus();
            btnEmail.Enabled = btnPDF.Enabled = false;
            if (PageMode != PageModes.Create)
                FunPriGetBulkRevisionDtls(PageIdValue);
            FunPubSetHeaderCheckBox();
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        FunPriLoadBranch();
        grvBranchDetails.Visible = true;
        pnlBranchDetails.Visible = true;
        btnSave.Enabled = true;
    }

    /// <summary>
    /// Method to load combos
    /// </summary>
    private void FunPriLoadCombos()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@User_Id", UserId);
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@FilterOption", "'HP','LN','FL','OL'");
        Procparam.Add("@Program_ID", "67");
        ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });


    }

    private void FunPriLoadBranch()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Param1", "1");
        Procparam.Add("@Param2", ddlLOB.SelectedValue);
        Procparam.Add("@Param3", Utility.StringToDate(txtRevisionEffectiveDate.Text).ToString());
        Procparam.Add("@Param4", UserId);
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@option", "7");
        DataTable dtBranchDetails = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetBulkRevison_List, Procparam);
        intRecordsCount = 0;
        grvBranchDetails.DataSource = dtBranchDetails;
        grvBranchDetails.DataBind();
        grvBranchDetails.Columns[2].Visible = false;
        grvBranchDetails.Columns[3].Visible = false;
        grvBranchDetails.Columns[4].Visible = false;

    }
    protected void grvBranchDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "View")
        {
            FunPriShowAccountsGrid(Convert.ToString(e.CommandArgument), Convert.ToString(ddlLOB.SelectedValue));
        }
    }

    /// <summary>
    ///Method to get Account details 
    /// </summary>
    /// <param name="strBranchId"></param>
    /// <param name="strLobId"></param>
    /// <returns></returns>
    private DataTable FunPriGetAccountDetails(string strBranchId, string strLobId)
    {

        ViewState["strBranchId"] = strBranchId;
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Param1", strBranchId);
        Procparam.Add("@Param2", strLobId);
        Procparam.Add("@Param3", Utility.StringToDate(txtRevisionEffectiveDate.Text).ToString());
        Procparam.Add("@Company_ID", CompanyId);
        if (PageMode == PageModes.Create)
            Procparam.Add("@Option", "1");
        else
            Procparam.Add("@Option", "8");
        Procparam.Add("@Param4", PageIdValue);

        DataTable dtAccDetails = Utility.GetDefaultData("S3G_LOANAD_GetBulkRevison_List", Procparam);
        return dtAccDetails;
    }

    /// <summary>
    /// Method to load account in modal Popup
    /// </summary>
    /// <param name="strBranchId"></param>
    /// <param name="strLobId"></param>
    private void FunPriShowAccountsGrid(string strBranchId, string strLobId)
    {
        DataTable dtAccDetails;
        dtAccDetails = FunPriGetAccountDetails(strBranchId, strLobId);

        if (dtAccDetails.Rows.Count > 0)
        {
            grvAccountDetails.DataSource = dtAccDetails;
            grvAccountDetails.DataBind();


            ModalPopupExtenderAccountDetails.Show();
        }
        else
        {
            Utility.FunShowAlertMsg(this, "No Accounts for selected location");
            ModalPopupExtenderAccountDetails.Hide();
        }
    }

    protected void grvAccountDetails_PageIndexChanged(object sender, EventArgs e)
    {
        //grvAccountDetails.PageIndex = grvAccountDetails.PageIndex + 1;
        ModalPopupExtenderAccountDetails.Show();
    }

    protected void grvAccountDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvAccountDetails.PageIndex = e.NewPageIndex;
        FunPriShowAccountsGrid((string)ViewState["strBranchId"], ddlLOB.SelectedValue);

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveRecord();

        }
        catch (Exception ex)
        {

            CvBulkRevision.ErrorMessage = "Unable to save Bulk revision";
            CvBulkRevision.IsValid = false;
        }
    }

    /// <summary>
    /// Created by Tamilselvan.S
    /// Created Date 15/11/2011
    /// For Email 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEmail_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubEmailSent(true);
        }
        catch (Exception ex)
        {

            CvBulkRevision.ErrorMessage = "Invalid EMail ID. Mail not sent.";
            CvBulkRevision.IsValid = false;
        }
    }

    /// <summary>
    /// Created by Tamilselvan.S
    /// Created Date 15/11/2011
    /// For PDF 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPDF_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubPrintToGenerateBulkRevision(true);
        }
        catch (Exception ex)
        {

            CvBulkRevision.ErrorMessage = "Unable to Print Bulk revision details";
            CvBulkRevision.IsValid = false;
        }
    }

    /// <summary>
    /// Method to save bulk revision
    /// </summary>
    private void FunPriSaveRecord()
    {
        try
        {
            string strBulkRevNo;

            DateTime dtScheduleDate = txtScheduleDate.Text == "" ? DateTime.Now : Utility.StringToDate(txtScheduleDate.Text);

            DateTime dtCurrentTime = DateTime.Now;

            TimeSpan span = new TimeSpan();

            span = dtCurrentTime.Subtract(dtScheduleDate);

            if ((span.Days) == 0)
            {
                if (txtScheduleTime.Text != "")
                {
                    DateTime dtScheduleTime = DateTime.Parse(txtScheduleTime.Text);
                    span = dtScheduleTime.Subtract(dtCurrentTime);
                    if (span.Hours == 0 && span.Minutes < 5)
                    {
                        Utility.FunShowAlertMsg(this, "Schedule Time should be greater than 5 minutes from current time if the schedule date is current date.");
                        return;
                    }
                }

            }




            ObjS3G_LOANAD_BulkRevisionDataTable = new ContractMgtServices.S3G_LOANAD_BulkRevisionDataTable();
            ContractMgtServices.S3G_LOANAD_BulkRevisionRow ObjBulkRevisionRow;
            ObjBulkRevisionRow = ObjS3G_LOANAD_BulkRevisionDataTable.NewS3G_LOANAD_BulkRevisionRow();
            ObjBulkRevisionRow.Bulk_Revision_ID = 0; // save mode
            ObjBulkRevisionRow.Company_ID = Convert.ToInt32(CompanyId);
            ObjBulkRevisionRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjBulkRevisionRow.Intervening_Interest = txtInterveningPeriodIntrest.Text == "Yes" ? "1" : "2";// ddlInterveningPeriodIntrest.SelectedValue;
            ObjBulkRevisionRow.Bulk_Revision_Date = Utility.StringToDate(txtRevisionDate.Text);
            ObjBulkRevisionRow.Bulk_Revision_Effective_Date = Utility.StringToDate(txtRevisionEffectiveDate.Text);
            if (rbtnSchedule.SelectedValue == "0")
            {
                // TimeSpan span=new TimeSpan(txtScheduleTime.Text.Split(':'
                ObjBulkRevisionRow.Schedule_Date = Utility.StringToDate(txtScheduleDate.Text);
                ObjBulkRevisionRow.Schedule_Time = txtScheduleTime.Text;
            }
            else
            {
                ObjBulkRevisionRow.Schedule_Date = dtCurrentTime;
                ObjBulkRevisionRow.Schedule_Time = dtCurrentTime.AddMinutes(5.0).ToShortTimeString();
            }

            ObjBulkRevisionRow.Changed_Interest_Rate = Convert.ToDecimal(txtRevisionImpactPercent.Text);
            ObjBulkRevisionRow.Created_By = Convert.ToInt32(UserId);
            ObjBulkRevisionRow.Modified_By = Convert.ToInt32(UserId);

            ObjBulkRevisionRow.XMLBulkRevisionDetails = grvBranchDetails.FunPubFormXml(true, true);

            ObjS3G_LOANAD_BulkRevisionDataTable.AddS3G_LOANAD_BulkRevisionRow(ObjBulkRevisionRow);
            ObjBulkRevisionClient = new ContractMgtServicesReference.ContractMgtServicesClient();
            intResult = ObjBulkRevisionClient.FunPubCreateBulkRevisionDetails(out strBulkRevNo, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_BulkRevisionDataTable, SerMode));
            bool blnRedirect = true;
            //strBulkRevNo = "dsklhgfds";
            switch (intResult)
            {
                case 0:
                    strAlert = "Bulk Revision no " + strBulkRevNo + " Created sucessfully.";
                    strAlert += @"\n\nWould you like to Continue?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    txtRevisionNumber.Text = strBulkRevNo;
                    break;

                case -1:
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    strRedirectPageView = "";
                    break;
                case -2:
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    strRedirectPageView = "";
                    break;
                case 2:
                    strAlert = strAlert.Replace("__ALERT__", "Bulk revision under progress for selected combination.");
                    strRedirectPageView = "";
                    break;
                case 3:
                    strAlert = strAlert.Replace("__ALERT__", "Bulk revision alreay done for selected location.");
                    strRedirectPageView = "";
                    break;
                case 4:
                    strAlert = strAlert.Replace("__ALERT__", "No location has been selected for bulk revision.");
                    strRedirectPageView = "";
                    break;
                case 5:
                    strAlert = strAlert.Replace("__ALERT__", "Bulk revision already effected for the selected location(s) for the given effective date.");
                    strRedirectPageView = "";
                    break;
                case 6:
                    strAlert = strAlert.Replace("__ALERT__", "Bulk revision already scheduled for the selected location(es) for the given scheduled date.");
                    strRedirectPageView = "";
                    break;
                case 50:
                    strAlert = strAlert.Replace("__ALERT__", "Error in creating Revision.");
                    strRedirectPageView = "";
                    break;
                //Code added by Kali on 16-Mar-2011 to Display Journal Validation Msg
                default:
                    Utility.FunShowValidationMsg(this.Page, "LOANAD_BURE", intResult, strBulkRevNo, false);
                    strRedirectPageView = "";
                    blnRedirect = false;
                    break;
            }
            if (blnRedirect)
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception(ex.Message);
        }
        finally
        {
            if (ObjBulkRevisionClient != null)
                ObjBulkRevisionClient.Close();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        FunPriResetControls("2");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect(strRedirectPage,false);
    }

    /// <summary>
    /// Methosd to get Global Parmeter Details
    /// </summary>
    private void FunPriGetGlobalParameterDetails()
    {
        try
        {
            FunPriResetControls("1");
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Param1", ddlLOB.SelectedValue);
            Procparam.Add("@Company_ID", CompanyId);
            Procparam.Add("@Option", "2");
            DataSet dsGlobalParamdetails = Utility.GetDataset("S3G_LOANAD_GetBulkRevison_List", Procparam);
            if (dsGlobalParamdetails != null)
            {
                string strInterveningInterest = dsGlobalParamdetails.Tables[1].Rows[0]["Parameter_Value"].ToString();
                if (strInterveningInterest != "")
                {
                    //ddlInterveningPeriodIntrest.SelectedValue = strInterveningInterest;
                    if (strInterveningInterest == "1")
                        txtInterveningPeriodIntrest.Text = "Yes";
                    else
                        txtInterveningPeriodIntrest.Text = "No";
                    //ddlInterveningPeriodIntrest.Enabled = false;
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Intervening Period Interest not defined in global parameter setup");

                    //Utility.FunShowAlertMsg(this, "Intervening Period Interest not defined in global parameter setup" + strRedirectPageView);
                    ddlLOB.SelectedIndex = 0;
                    //return;
                }
                decimal decRevImpact = Convert.ToDecimal(dsGlobalParamdetails.Tables[0].Rows[0]["Parameter_Value"].ToString());
                if (decRevImpact != 0)
                {
                    txtRevisionImpactPercent.Text = decRevImpact.ToString();
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", @"Revision Impact % is not defined in global parameter setup \r\n for selected Line of Business");
                    // Utility.FunShowAlertMsg(this, "Revision Impact not defined in global parameter setup for selected Line of Business" + strRedirectPageView);
                    ddlLOB.SelectedIndex = 0;
                    txtInterveningPeriodIntrest.Text = "";
                    //return;
                }
                string strGapdays = dsGlobalParamdetails.Tables[2].Rows[0]["Parameter_Value"].ToString();
                if (strGapdays != "")
                {
                    hdnGapdays.Value = strGapdays;
                }
                else
                {
                    //Utility.FunShowAlertMsg(this, "Revision Gap Days not defined in global parameter setup" + strRedirectPageView);
                    strAlert = strAlert.Replace("__ALERT__", "Revision Gap Days not defined in global parameter setup");
                    ddlLOB.SelectedIndex = 0;
                    //return;
                }
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Global Parameter setup not defined for Bulk Revision");

                //Utility.FunShowAlertMsg(this, "Global Parameter Setup Not defined for Bulk Revision" + strRedirectPageView);
                ddlLOB.SelectedIndex = 0;
                // return;
            }
            if (strAlert != "alert('__ALERT__');")
            {
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else
                return;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException(ex.Message);
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriResetControls("1");
            if (ddlLOB.SelectedIndex > 0)
            {
                FunPriGetGlobalParameterDetails();
                FunPriLoadBranch();
            }
        }
        catch (Exception ex)
        {
            CvBulkRevision.ErrorMessage = "Unable load Globalparameter details";
            CvBulkRevision.IsValid = false;
        }
    }

    /// <summary>
    /// Method to Reset Controls
    /// </summary>
    /// <param name="strMode"></param>
    private void FunPriResetControls(string strMode)
    {
        switch (strMode)
        {
            case "1":
                txtRevisionImpactPercent.Text = "";
                //ddlInterveningPeriodIntrest.SelectedIndex = 0;
                pnlBranchDetails.Visible = false;
                txtScheduleDate.Text = "";
                txtScheduleTime.Text = "";
                txtRevisionEffectiveDate.Text = DateTime.Now.ToString(DateFormate);
                txtInterveningPeriodIntrest.Text = "";
                btnClear.Enabled = true;
                break;
            case "2":
                txtRevisionImpactPercent.Text = "";
                //ddlInterveningPeriodIntrest.SelectedIndex = 0;
                ddlLOB.SelectedIndex = 0;
                btnSave.Enabled = false;
                //txtRevisionDate.Text = "";
                txtRevisionEffectiveDate.Text = DateTime.Now.ToString(DateFormate);
                pnlBranchDetails.Visible = false;
                txtScheduleDate.Text = "";
                txtScheduleTime.Text = "";
                txtInterveningPeriodIntrest.Text = "";
                break;
        }
    }

    private void FunPriGetBulkRevisionDtls(string strBulkRevNumber)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Param1", strBulkRevNumber);
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@Option", "3");
        DataSet dsBulkRevDetails = Utility.GetDataset(SPNames.S3G_LOANAD_GetBulkRevison_List, Procparam);
        btnEmail.Enabled = btnPDF.Enabled = false;
        if (dsBulkRevDetails.Tables.Count > 0)
        {
            if ((dsBulkRevDetails.Tables[0].Rows[0]["Status"]).ToString() == "Processed")
            { btnEmail.Enabled = btnPDF.Enabled = true; }

            if (dsBulkRevDetails.Tables[0].Rows.Count > 0)
            {
                DataRow drBulkRevRow = dsBulkRevDetails.Tables[0].Rows[0];
                ddlLOB.Items.Add(new ListItem(drBulkRevRow["LOB_Name"].ToString(), drBulkRevRow["LOB_ID"].ToString()));
                ddlLOB.ToolTip = drBulkRevRow["LOB_Name"].ToString();
                //ddlLOB.ClearDropDownList();
                //ddlInterveningPeriodIntrest.SelectedValue = drBulkRevRow["Intervening_Interest"].ToString();
                txtInterveningPeriodIntrest.Text = Convert.ToString(drBulkRevRow["Intervening_Interest"]) == "1" ? "Yes" : "No";
                //ddlInterveningPeriodIntrest.ClearDropDownList();
                txtRevisionImpactPercent.Text = drBulkRevRow["Changed_Interest_Rate"].ToString();
                txtRevisionDate.Text = drBulkRevRow["Revision_Date"].ToString();
                txtRevisionEffectiveDate.Text = drBulkRevRow["Effective_Date"].ToString();
                txtRevisionNumber.Text = drBulkRevRow["Bulk_Revision_Number"].ToString();
            }
            if (dsBulkRevDetails.Tables[1].Rows.Count > 0)
            {
                grvBranchDetails.DataSource = dsBulkRevDetails.Tables[1];
                grvBranchDetails.DataBind();
                grvBranchDetails.Columns[2].Visible = false;
                grvBranchDetails.Columns[3].Visible = true;
                grvBranchDetails.Columns[4].Visible = true;
                // grvBranchDetails.Enabled = false;
            }
            if (dsBulkRevDetails.Tables[2].Rows.Count > 0)
            {
                DataRow drScheduleRow = dsBulkRevDetails.Tables[2].Rows[0];
                txtScheduleDate.Text = drScheduleRow["Schedule_Date"].ToString();
                txtScheduleTime.Text = drScheduleRow["Schedule_Time"].ToString();

            }
        }

    }
    /// <summary>
    /// Event for Checkbox in grid
    /// </summary>u
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((CheckBox)sender).ClientID;
        if (((CheckBox)sender).Checked)
        {
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvBranchDetails_")).Replace("grvBranchDetails_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            string strBrachId = ((Label)grvBranchDetails.Rows[gRowIndex].FindControl("lblBranchId")).Text;
            DataTable dtAccountDetails = FunPriGetAccountDetails(strBrachId, ddlLOB.SelectedValue);
            if (dtAccountDetails.Rows.Count <= 0)
            {
                Utility.FunShowAlertMsg(this, "No Accounts for selected location");
                ((CheckBox)sender).Checked = false;
            }
            if (((CheckBox)sender).Checked == true)
                intRecordsCount += 1;
        }
        else
            intRecordsCount -= 1;
        FunPubSetHeaderCheckBox();
    }

    /// <summary>
    /// Event for Processing bulk revision
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriStartBulkRevisionServices();

        }
        catch (Exception ex)
        {
            if (ex.Message == "Service S3gWinServices was not found on computer")
            {
                CvBulkRevision.ErrorMessage = ex.Message;
                CvBulkRevision.IsValid = false;

            }
            else
            {

                CvBulkRevision.ErrorMessage = "Unable to start the services";
                CvBulkRevision.IsValid = false;
            }
        }
    }

    /// <summary>
    /// Method to start bulk revision Process
    /// </summary>
    private void FunPriStartBulkRevisionServices()
    {
        try
        {
            ServiceController serviceWinService = new ServiceController("S3gWinServices");
            string[] strArgs = new string[5];
            strArgs[0] = CompanyId;
            if (serviceWinService.Status != ServiceControllerStatus.Running)
            {
                serviceWinService.Start(strArgs);
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException(ex.Message);
        }
    }

    private void FunPriBulkRevControlStatus()
    {
        switch (PageMode)
        {
            case PageModes.Create: // Create Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                grvBranchDetails.Visible = false;
                pnlBranchDetails.Visible = false;
                break;
            case PageModes.Modify://Modify

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                //grvBranchDetails.Enabled = false;
                btnGo.Enabled = false;
                rbtnSchedule.Enabled = false;
                grvBranchDetails.Visible = true;
                txtScheduleTime.ReadOnly = true;
                break;
            case PageModes.Query://Query  
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                //grvBranchDetails.Enabled = false;
                btnGo.Enabled = false;
                rbtnSchedule.Enabled = false;
                CalendarExtender1.Enabled = false;
                CalendarExtender2.Enabled = false;
                imgRevisionEffectiveDate.Visible = false;
                imgScheduleDate.Visible = false;
                grvBranchDetails.Visible = true;
                pnlBranchDetails.Visible = true;
                txtScheduleTime.ReadOnly = true;
                break;
        }
    }
    protected void rbtnSchedule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnSchedule.SelectedValue == "0")
        {
            txtScheduleTime.Enabled = true;
            rfvScheduleTime.Enabled = true;
            revScheduleTime.Enabled = true;
            rfvScheduleDate.Enabled = true;
            CalendarExtender2.Enabled = true;

        }
        else
        {
            txtScheduleTime.Enabled = false;
            txtScheduleTime.Text = "";
            txtScheduleDate.Text = "";
            rfvScheduleTime.Enabled = false;
            rfvScheduleDate.Enabled = false;
            CalendarExtender2.Enabled = false;
            revScheduleTime.Enabled = false;
        }
    }

    public static int intRecordsCount = 0;
    protected void grvBranchDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CheckBox chk = (CheckBox)e.Row.FindControl("chkCheckBox");
            //if (chk.Checked)
            //    e.Row.Enabled = false;
            if (PageMode == PageModes.Query)
            {
                e.Row.Cells[0].Enabled = false;
            }
            if (((CheckBox)e.Row.FindControl("chkCheckBox")).Checked)
                intRecordsCount += 1;
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            if (PageMode == PageModes.Query)
            {
                e.Row.Cells[0].Enabled = false;
            }
        }
    }

    protected void chkCheckBoxHdr_CheckedChanged(object sender, EventArgs e)
    {
        int intRowCount = 0;
        foreach (GridViewRow grv in grvBranchDetails.Rows)
        {
            CheckBox chkSelect = (CheckBox)grv.FindControl("chkCheckBox");
            if (((CheckBox)sender).Checked)
            {

                chkSelect.Checked = true;
                string strFieldAtt = ((CheckBox)chkSelect).ClientID;

                string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvBranchDetails_")).Replace("grvBranchDetails_ctl", "");
                int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
                gRowIndex = gRowIndex - 2;
                string strBrachId = ((Label)grvBranchDetails.Rows[gRowIndex].FindControl("lblBranchId")).Text;
                DataTable dtAccountDetails = FunPriGetAccountDetails(strBrachId, ddlLOB.SelectedValue);
                if (dtAccountDetails.Rows.Count <= 0)
                {
                    intRowCount++;
                    chkSelect.Checked = false;
                }
            }
            else
            {
                chkSelect.Checked = false;
            }
        }
        if (intRowCount > 0)
        {
            ((CheckBox)sender).Checked = false;
        }
    }

    string strnewFile = string.Empty;
    string strToMailID = string.Empty;
    string strFromMailID = string.Empty;
    string strCCMailID = string.Empty;
    /// <summary>
    /// Created by Tamilselvan.S
    /// Created Date 15/11/2011
    /// For Email and PDF
    /// </summary>
    /// <param name="blnPrint"></param>
    public void FunPubPrintToGenerateBulkRevision(bool blnPrint)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@BulkRevision_No", PageIdValue);
        Procparam.Add("@Company_ID", CompanyId);
        DataTable dsBulkRevDetails = Utility.GetDataset("S3G_LNA_BulkRevision_EmailPDF", Procparam).Tables[0];
        ReportDocument rptBulkRevision = new ReportDocument();
        Guid objGuid;
        objGuid = Guid.NewGuid();
        if (dsBulkRevDetails.Rows.Count > 0)
        {
            rptBulkRevision.Load(Server.MapPath("BulkRevision.rpt"));
            rptBulkRevision.SetDataSource(dsBulkRevDetails);
            strToMailID = dsBulkRevDetails.Rows[0]["Email_ID"].ToString();
            strFromMailID = dsBulkRevDetails.Rows[0]["Com_Email_ID"].ToString();

            //MailAddress ccCopy = new MailAddress();

            if (dsBulkRevDetails.Rows[0]["NextLevel_EmailID"] != DBNull.Value && !string.IsNullOrEmpty(dsBulkRevDetails.Rows[0]["NextLevel_EmailID"].ToString()))
                strCCMailID = dsBulkRevDetails.Rows[0]["NextLevel_EmailID"].ToString();

            if (dsBulkRevDetails.Rows[0]["HigherLevel_EmailID"] != DBNull.Value && !string.IsNullOrEmpty(dsBulkRevDetails.Rows[0]["HigherLevel_EmailID"].ToString()))
                strCCMailID += "," + dsBulkRevDetails.Rows[0]["HigherLevel_EmailID"].ToString();

            DirectoryInfo df = new DirectoryInfo(Convert.ToString(Server.MapPath(".") + "\\PDF Files"));
            if (!df.Exists)
            {
                df.Create();
            }
           
            strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + objGuid + "Bulk Revision Statistics.pdf");

            rptBulkRevision.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);
        }
        string strScipt = "";

        if (blnPrint)
        {
            strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strnewFile.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }

    }

    public void FunPubSetHeaderCheckBox()
    {
        if (grvBranchDetails.Rows.Count > 0 && intRecordsCount == grvBranchDetails.Rows.Count)
        {
            if (grvBranchDetails.HeaderRow != null)
                ((CheckBox)grvBranchDetails.HeaderRow.FindControl("chkCheckBoxHdr")).Checked = true;
        }
        else
        {
            if (grvBranchDetails.HeaderRow != null)
                ((CheckBox)grvBranchDetails.HeaderRow.FindControl("chkCheckBoxHdr")).Checked = false;
        }
    }

    #region [Email Sent]

    public void FunPubEmailSent(bool booMail)
    {
        if (strnewFile == "")
            FunPubPrintToGenerateBulkRevision(false);

        if (booMail && strToMailID == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Customer does not have an Email Id');", true);
            return;
        }
        Dictionary<string, string> dictMail = new Dictionary<string, string>();
        //dictMail.Add("FromMail", "s3g@sundaraminfotech.in");
        dictMail.Add("FromMail", strFromMailID);
        dictMail.Add("ToMail", strToMailID);
        dictMail.Add("ToCC", strCCMailID);
        dictMail.Add("Subject", "Bulk Revision Control Statistics");
        ArrayList arrMailAttachement = new ArrayList();
        if (strnewFile != "")
        {
            arrMailAttachement.Add(strnewFile);
        }

        try
        {
            Utility.FunPubSentMail(dictMail, arrMailAttachement, new StringBuilder());
            Utility.FunShowAlertMsg(this, "Mail sent successfully.");
        }
        catch (Exception objException)
        {
            if (objException.Message.Contains("Mailbox unavailable"))
            {
                Utility.FunShowAlertMsg(this, "E-Mail not sent.Kindly contact the administrator");
            }
            return;
        }


    }

    #endregion [Email Sent]
}
