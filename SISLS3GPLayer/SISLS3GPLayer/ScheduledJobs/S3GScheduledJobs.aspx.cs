
#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Scheduled Jobs
/// Screen Name         :   Scheduled Jobs 
/// Created By          :   Muni Kavitha    
/// Created Date        :   
/// Purpose             :   To save Scheduled Jobs details
/// Created By          :  Prabhu.K 
/// Created Date        :  20-dec-2011 
/// Purpose             :  To Develop the Scheduled Jobs as per new requirement
/// <Program Summary>
#endregion


#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Web.Security;
using System.IO;
using System.Xml;
using S3GBusEntity.ScheduledJobs;
using System.Globalization;
using Resources;
#endregion


public partial class ScheduledJobs_S3GScheduledJobs : ApplyThemeForProject
{
    #region Common Variable declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> Procparam = null;
    string strApprLogicID = "0";
    int intErrCode = 0;
    int intScheduleJobId = 0; 
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode SerMode = SerializationMode.Binary;
    ScheduledJobMgtServices.S3G_SYSAD_ScheduledJobsDataTable objScheduledJobsDataTable = null;
    ScheduledJobMgtServices.S3G_SYSAD_ScheduledJobsRow objScheduledJobsRow = null;
    ScheduledJobMgtServicesReference.ScheduledJobMgtServicesClient objScheduleMgtServiceClient = null;
    S3GSession ObjS3GSession = null;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "S3GScheduledJobs.aspx";
    string strRedirectPageView = "S3GScheduledJobsView.aspx";

    string strRedirectPageAdd = "window.location.href='../Others/S3GScheduledJobs.aspx';";
    string strRedirectPageView1 = "window.location.href='../Others/S3GScheduledJobsView.aspx';";

    string strDateFormat = string.Empty;
    //string strcode = "";
    static string strPageName = "Scheduled Jobs";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    string strSch_ID = "";
 
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            cvScheduledJobs.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad + this.Page.Header.Title;
            cvScheduledJobs.IsValid = false;
        }

    }


    private void FunPriLoadPage()
    {

        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPubSetIndex(1);
            if (Request.QueryString["qsMode"] != null)
            {
                strMode = Request.QueryString["qsMode"].ToString();
            }
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket formTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (formTicket != null)
                {
                    strSch_ID = formTicket.Name;
                }
            }


            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            calDocDate.Format = strDateFormat;
            txtStartDate.Attributes.Add("readonly", "true");
            if (!IsPostBack)
            {
                if ((strSch_ID != "") && (strMode == "Q")) // Query 
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Schedule);
                    btnClear.Enabled = true;
                    FunPriLoadLov();
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    
                    break;
                }
           
            case -1:// Query Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }
                    FunGetScheduledJobsDetails();
                    chkActive.Enabled = chkHoliday.Enabled = false;
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    ddlJobNature.ClearDropDownList();
                    ddlFrequency.ClearDropDownList();
                    ddlLocation.ClearDropDownList();
                    ddlJob.ClearDropDownList();
                    txtJobDescription.ReadOnly = 
                    txtRemarks.ReadOnly = txtMinutes.ReadOnly = txtStartTime.ReadOnly =true ;
                    break;
                }
        }
    }

    private void FunPriLoadLov()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Add("@Option", "1");
        Procparam.Add("@CompanyID", intCompanyID.ToString());
        ddlFrequency.BindDataTable("S3G_SCH_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
        Procparam.Clear();
        Procparam.Add("@Option", "2");
        Procparam.Add("@CompanyID", intCompanyID.ToString());
        ddlJobNature.BindDataTable("S3G_SCH_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

        Procparam.Clear();
        Procparam.Add("@Option", "3");
        Procparam.Add("@CompanyID", intCompanyID.ToString());
        ddlJob.BindDataTable("S3G_SCH_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
        Procparam.Clear();
        if (intScheduleJobId == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }
        Procparam.Add("@User_Id", intUserID.ToString());
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Program_Id", "205");
        ddlLocation.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
    }

    private void FunGetScheduledJobsDetails()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@Schedule_ID", (strSch_ID));
            DataTable dtScheduleDetails = Utility.GetDefaultData("S3G_Sysad_GetScheduleDetails", Procparam);
            FunPriLoadLov();
            if (dtScheduleDetails.Rows.Count > 0)
            {
                ddlLocation.SelectedValue = dtScheduleDetails.Rows[0]["Location"].ToString();
                txtJobDescription.Text = dtScheduleDetails.Rows[0]["JobDescription"].ToString();
                ddlJobNature.SelectedValue = dtScheduleDetails.Rows[0]["JobNature"].ToString();
                ddlJob.SelectedValue = dtScheduleDetails.Rows[0]["ScheduleJob"].ToString();
                txtStartDate.Text = dtScheduleDetails.Rows[0]["StartDate"].ToString();
                txtStartTime.Text = dtScheduleDetails.Rows[0]["StartTime"].ToString();
                txtMinutes.Text = dtScheduleDetails.Rows[0]["Time_In_Mins"].ToString();
                ddlFrequency.SelectedValue = dtScheduleDetails.Rows[0]["Frequency"].ToString();
                chkHoliday.Checked = Convert.ToBoolean(dtScheduleDetails.Rows[0]["Holiday"]);
                txtRemarks.Text = dtScheduleDetails.Rows[0]["Remarks"].ToString();
                chkActive.Checked = Convert.ToBoolean(dtScheduleDetails.Rows[0]["Is_Active"]);
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
     

    protected void ddlJobNature_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
          
        }
        catch (Exception ex)
        {
            cvScheduledJobs.ErrorMessage = ex.ToString();
            cvScheduledJobs.IsValid = false;
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        objScheduleMgtServiceClient =new ScheduledJobMgtServicesReference.ScheduledJobMgtServicesClient();
        try
        {
            objScheduledJobsDataTable = new ScheduledJobMgtServices.S3G_SYSAD_ScheduledJobsDataTable();
            objScheduledJobsRow = objScheduledJobsDataTable.NewS3G_SYSAD_ScheduledJobsRow();
            objScheduledJobsRow.Location_Code = ddlLocation.SelectedValue;
            objScheduledJobsRow.JobDescription = txtJobDescription.Text;
            objScheduledJobsRow.JobNature = Convert.ToInt32(ddlJobNature.SelectedValue);
            objScheduledJobsRow.Schedule_Job = Convert.ToInt32(ddlJob.SelectedValue);
            objScheduledJobsRow.Frequency = Convert.ToInt32(ddlFrequency.SelectedValue);
            if (ddlFrequency.SelectedValue == "1")
            {
                objScheduledJobsRow.Time_In_Mins = Convert.ToInt32(txtMinutes.Text);
            }
            objScheduledJobsRow.StartDateTime = Utility.StringToDate(txtStartDate.Text + " " + txtStartTime.Text);
            objScheduledJobsRow.Holiday = chkHoliday.Checked;
            objScheduledJobsRow.Remarks = txtRemarks.Text;
            objScheduledJobsRow.Created_By = intUserID;
            objScheduledJobsRow.Company_ID = intCompanyID;
            objScheduledJobsRow.Is_Active = chkActive.Checked;
            objScheduledJobsDataTable.AddS3G_SYSAD_ScheduledJobsRow(objScheduledJobsRow);
            byte[] byteobjScheduledJobsDataTable = ClsPubSerialize.Serialize(objScheduledJobsDataTable, SerMode);
            intErrCode = objScheduleMgtServiceClient.FunPubCreateScheduledJobs(out strSch_ID, SerMode, byteobjScheduledJobsDataTable);
            if (intErrCode == 0)
            {
                strAlert = "Job Scheduled Successfully";
                strAlert += @"\n\nWould you like to Schedule one more Job?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView1 + "}";
                
            }
            else

            {
                strAlert = strAlert.Replace("__ALERT__", "Due to Data Problem,Unable to schedule the Job");
            }
            strRedirectPageView1 = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView1, true);

        }
        catch (Exception ex)
        {

            cvScheduledJobs.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_Save + this.Page.Header.Title;
            cvScheduledJobs.IsValid = false;
        }

    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlJobNature.SelectedValue = "0";
        ddlFrequency.SelectedValue = "0";
        ddlLocation.SelectedValue = "0";
        ddlJob.SelectedValue = "0";
        txtStartDate.Text = txtJobDescription.Text = 
        txtRemarks.Text = txtStartTime.Text = txtMinutes.Text = "";
        chkHoliday.Checked = chkActive.Checked = false;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPageView);
    }
      
   
}
