
#region Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved

/// <Program Summary>
/// Module Name               : Reports
/// Screen Name               : CIBIL Report Generation
/// Created By                : Manikandan. R
/// Created Date              : 22-Oct-2011
/// Purpose                   : 
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    :

/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections;
#endregion

public partial class Reports_S3GRPTCIBILReportGeneration : ApplyThemeForProject
{
    #region Variable declaration
    int intCompanyId, intUserId = 0;
    Dictionary<string, string> Procparam = null;
    int intErrorCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerializationMode = SerializationMode.Binary;
    string strDateFormat = string.Empty;
    string strCIB = string.Empty;
    static string strPageName = "CIBIL Report Generation";
    int intCIBILId;
   
    string strErrorMessagePrefix = @"Please correct the following validation(s): </br></br>   ";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Reports/S3G_RPT_CIBILGeneration_View.aspx";
    string strRedirectPageAdd = "window.location.href='../Reports/S3GRPTCIBILReportGeneration.aspx';";
    string strRedirectPageView = "window.location.href='../Reports/S3G_RPT_CIBILGeneration_View.aspx';";
    StringBuilder strbuilder = new StringBuilder();
    string ERRORCODE = string.Empty;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
   
    #endregion
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        S3GSession ObjS3GSession = new S3GSession();
        try
        {
            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            txtPresentDate.Text = DateTime.Now.ToShortDateString();
            //txtCutoffDate.Text = DateTime.Today.ToString("MM") + "/" + DateTime.Today.Year;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            txtSheduledDate.Attributes.Add("readonly", "true");
            bCreate = ObjUserInfo.ProCreateRW;
            CalendarExtender2.Format = strDateFormat;
            txtSheduledDate.Attributes.Add("readonly", "readonly");
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strCIB = fromTicket.Name;

                int IsAdd = 1;

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (fromTicket != null)
                {
                    intCIBILId = Convert.ToInt32(fromTicket.Name);
                    strMode = Request.QueryString.Get("qsMode");
                    
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
            if (!IsPostBack)
            {
                rdoSheduleLater.Checked = true;
                rdoSheduleLater.Checked = false;
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));


                if ((intCIBILId > 0) && (PageMode == PageModes.Query))
                {

                    PopulateCIBILData(intCIBILId);
                    FunPriDisableControls(1);
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
            throw new ApplicationException("Unable to Load CIBIL Generation Related Details");
        }
        finally
        {
            ObjS3GSession = null;
        }
    }
    #region Disable Contraol Function
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                //  lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                if (!bCreate)
                {
                    btnGenerate.Enabled = false;

                }
                txtStatus.Visible = lblStatus.Visible = false;
                lnkExcel.Visible = lnkText.Visible = false;

                break;

            case 1: // Query Mode

                //  lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                if (!bModify)
                {
                    btnGenerate.Enabled = false;
                }
                txtCutoffDate.ReadOnly = true;
                txtPath.ReadOnly = true;
                rdoSheduleLater.Enabled = false;
                rdoSheduleNow.Enabled = false;
                txtSheduledDate.ReadOnly = true;
                txtHour.ReadOnly = true;
                txtMins.ReadOnly = true;
                txtHour.Enabled = txtMins.Enabled = false;
                ddlAMPM.ClearDropDownList();
                btnGenerate.Enabled = false;
                btnClear.Enabled = false;
                calendar1.Enabled = false;
                //calendar1.EnabledOnClient = true;
                
                CalendarExtender2.Enabled = false;
                txtStatus.Visible = lblStatus.Visible = true;
                txtStatus.ReadOnly = true;
                string strPageLoad = "PageLoad";
                break;
           
        }
    }
    #endregion

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            FunProGenerate(true);
           
        }
        catch (Exception ex)
        {
            cvCIBIL.ErrorMessage = "Unable to generate CIBIL Report";
            cvCIBIL.IsValid = true;
            return;
            
        }
        
    }

    protected void FunProGenerate(bool FromSave)
    {
        string Rws = "";
        string strFolder = "";
        string strReport_Date = "";
        DateTime dtRepoDate;

        // GIVING FILE PATH
        string StrFilePathName = (txtCutoffDate.Text.Split('/')[0].ToString()) + (txtCutoffDate.Text.Split('/')[1].ToString());
        //Concadinating Report_Date
        int strCutMont = Convert.ToInt32(txtCutoffDate.Text.Split('/')[0].ToString());
        int strCutYr = Convert.ToInt32(txtCutoffDate.Text.Split('/')[1].ToString());
        int EOD = FunPubGetEndDayOfMonth(strCutYr, strCutMont);
        strReport_Date = Convert.ToString(EOD.ToString() + '/' + strCutMont.ToString() + '/' + strCutYr.ToString());
        // Split for the Month
        string repMonth = (txtCutoffDate.Text.Split('/')[1].ToString()) + (txtCutoffDate.Text.Split('/')[0].ToString());
        dtRepoDate = (Utility.StringToDate(strReport_Date));
        string repTime = (txtSheduledDate.Text.ToString() + ' ' + txtHour.Text.ToString() + ':' + txtMins.Text.ToString() + ' ' + ddlAMPM.SelectedItem.Text.ToString());
        string strShDate = (txtSheduledDate.Text.ToString() + ' ' + txtHour.Text.ToString() + ':' + txtMins.Text.ToString() + ' ' + ddlAMPM.SelectedItem.Text.ToString());
        // TO Check Date
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Report_Date", dtRepoDate.ToString());
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        DataSet dsCheck = Utility.GetDataset("S3G_RPT_CheckDate", Procparam);
        if (dsCheck.Tables[0].Rows.Count > 0)
        {
            Utility.FunShowAlertMsg(this, "Already CIBIL Record generated greater than the given Cutoff Month/Year");
            txtCutoffDate.Text = "";
            txtPath.Text = "";
            txtSheduledDate.Text = "";
            rdoSheduleLater.Checked = false;
            rdoSheduleNow.Checked = false;
            txtSheduledDate.Text = "";
            txtHour.Text = "12";
            txtMins.Text = "00";
            ddlAMPM.SelectedValue = "AM";
            ddlAMPM.SelectedItem.Text = "AM";
            txtHour.Enabled = txtMins.Enabled = ddlAMPM.Enabled = true;
            CalendarExtender2.Enabled = true;
            return;
        }
        else if (dsCheck.Tables[1].Rows.Count > 0)
        {
            strAlert = "Already CIBIL File Generated for the Selected Cutoff Month/Year";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + strAlert + "');" + strRedirectPageView, true);
            return;
    
        }
        else if (dsCheck.Tables[2].Rows.Count > 0)
        {
            strAlert = "Already CIBIL File Initiated for the Selected Cutoff Month/Year";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + strAlert + "');" + strRedirectPageView, true);
            return;
        }
        // To Check Member Code is availble         

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@intCompany_ID", intCompanyId.ToString());
        DataTable dtCheckMemCode = Utility.GetDefaultData("S3G_RPT_CheckMemberCode", Procparam);
        if (dtCheckMemCode.Rows.Count <= 0)
        {
            Utility.FunShowAlertMsg(this, "The Member Code and Short Member Code is not available cannot proceed further");
            return;
        }

        // Fetching of Data

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Cutoff_Month", repMonth.ToString());
        Procparam.Add("@User_ID", intUserId.ToString());
        Procparam.Add("@Sheduled_Date", strShDate);
        Procparam.Add("@Sheduled_Time", repTime.ToString());
        Procparam.Add("@Document_Path", txtPath.Text.ToString());
        Procparam.Add("@Report_Date", dtRepoDate.ToString());
        if(!FromSave)
        Procparam.Add("@Need_Generate", intCIBILId.ToString());
        DataSet DSet = Utility.GetDataset("S3G_RPT_InsertCIBIL", Procparam);
        if (!FromSave)
        {
            strAlert = "File Generation Initiated";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + strAlert + "');" + strRedirectPageView, true);
        }
        else
        {
            strAlert = "Data Generation Initiated";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + strAlert + "');" + strRedirectPageView, true);
        }
        //return;
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
         try
        {
            string strexcels;
            strexcels = "excels";
            FunFileGeneration(strexcels, "0", "0");
        }
        catch (Exception ex)
        {
            cvCIBIL.ErrorMessage = "Unable Download File";
            cvCIBIL.IsValid = true;
            return;
        }
    }
    protected void lnkText_Click(object sender, EventArgs e)
    {
        try
        {
            string strtext;
            strtext = "text";
            FunFileGeneration(strtext, "0", "0");
        }
        catch (Exception ex)
        {
            cvCIBIL.ErrorMessage = "Unable Download File";
            cvCIBIL.IsValid = true;
            return;
        }
    }

    public void FunFileGeneration(string str, string intDownLoad_XL, string intDownLoad_Txt)
    {
        string StrFilePathName;
        // Fetching StrFilePathName
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@CIBILMaster_Id", intCIBILId.ToString());
        DataSet dsPath = Utility.GetDataset("S3G_RPT_getCIBILFilePath", Procparam);

        DataTable dtPath = dsPath.Tables[0];
        DataRow dRowPath = dtPath.Rows[0];
        StrFilePathName = dRowPath["FILEPATH"].ToString(); ;



        // Fetching File Name
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@CIBILMaster_Id", intCIBILId.ToString());
        DataSet dsFile = Utility.GetDataset("S3G_RPT_getCIBILFiles", Procparam);

        DataTable dtFile = dsFile.Tables[0];
        DataRow dRowFile = dtFile.Rows[0];
        string strFileName = dRowFile["FILENAMES"].ToString();
        string strPath = "\\Reports\\" + StrFilePathName + "\\TEXT\\";

        string strFullPath = Server.MapPath(".") + "\\" + StrFilePathName + "\\TEXT\\";

        //string strPath = Server.MapPath(".") + "\\" + StrFilePathName + "\\TEXT\\";
        if (str == "PageLoad")
        {
            string FileName = strFullPath + strFileName + ".txt";
            string FileNameExcel = strFullPath + strFileName + ".xls";
            if (File.Exists(FileNameExcel) && intDownLoad_XL == "1")
            {
                lnkExcel.Visible = true;
            }
            else
            {
                lnkExcel.Visible = false;
            }

            if (File.Exists(FileName) && intDownLoad_Txt == "1")
            {
                lnkText.Visible = true;
            }
            else
            {
                lnkText.Visible = false;
                btnGenerateFile.Enabled = true;
            }

            return;            
        }
        if (!string.IsNullOrEmpty(strFileName) && !string.IsNullOrEmpty(StrFilePathName))
        {
            strPath = strPath.Replace("\\", "/").Trim();

            Response.Clear();
            if (str == "text")
            {
                //Response.AppendHeader("content-disposition", "attachment; filename=" + strPath + strFileName + ".txt");
                //Response.ContentType = "application/octet-stream";
                //Response.WriteFile(strPath + strFileName + ".txt");
                //Response.End();

                Response.Clear();
                Response.AppendHeader("content-disposition", "attachment; filename=" + strFileName + ".txt");
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(".." + strPath + strFileName + ".txt");
                Response.End();

                //string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strPath + strFileName + ".txt', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

            }
            else
            {
                
                Response.AppendHeader("content-disposition", "attachment; filename=" + strFileName + ".xls");
                Response.ContentType = "application/vnd.xls";
                Response.WriteFile(".." + strPath + strFileName + ".xls");
                Response.End();

                //string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strPath + strFileName + ".xls', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

                //Response.AppendHeader("content-disposition", "attachment; filename=" + strPath + strFileName + ".xls");
                //Response.ContentType = "application/octet-stream";
                //Response.WriteFile(strPath + strFileName + ".xls");
            }
            
            //string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strPath + strFileName + ".txt', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        else
        {
            Utility.FunShowAlertMsg(this, "Unable to download File");
            return;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        rdoSheduleNow.Checked = false;
        rdoSheduleLater.Checked = false;
        txtPath.Text = "";
        txtCutoffDate.Text = "";
        txtSheduledDate.Text = "";
        txtHour.Text = "12";
        txtMins.Text = "00";
        ddlAMPM.SelectedValue = "AM";
        ddlAMPM.SelectedItem.Text = "AM";
        txtHour.Enabled = txtMins.Enabled = ddlAMPM.Enabled = true;
        CalendarExtender2.Enabled = true;
        grvInvalid.DataSource = "";
        grvInvalid.DataBind();
       

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }
    protected void rdoSheduleNow_CheckedChanged(object sender, EventArgs e)
    {
        txtSheduledDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
        CalendarExtender2.Enabled = false;
        txtHour.Text = DateTime.Now.ToString("hh");
        txtMins.Text = DateTime.Now.ToString("mm");
        ddlAMPM.SelectedValue = DateTime.Now.ToString("tt");
        txtHour.Enabled = false;
        txtMins.Enabled = false;
        ddlAMPM.Enabled = false;
        rdoSheduleLater.Checked = false;

    }
    protected void rdoSheduleLater_CheckedChanged(object sender, EventArgs e)
    {
        txtSheduledDate.Text = "";
        CalendarExtender2.Enabled = true;
        txtHour.Text = "12";
        txtMins.Text = "00";
        ddlAMPM.SelectedValue = "AM";
        ddlAMPM.SelectedItem.Text = "AM";
        txtHour.Enabled = true;
        txtMins.Enabled = true;
        ddlAMPM.Enabled = true;
        rdoSheduleNow.Checked = false;


    }
    protected void txtCutoffDate_TextChanged(object sender, EventArgs e)
    {
        rdoSheduleLater.Checked = false;
        rdoSheduleNow.Checked = false;
        txtHour.Text = "12";
        txtMins.Text = "00";
        ddlAMPM.SelectedValue = "AM";
        txtHour.Enabled = txtMins.Enabled = ddlAMPM.Enabled = true;
        int strCutMonth;
        int strCutyear;
        int strPMonth;
        int strPDay;
        int strPYear;
        int strDate;
        int EOD;
        txtSheduledDate.Text = "";
        string StrFilePathName = (txtCutoffDate.Text.Split('/')[0].ToString()) + (txtCutoffDate.Text.Split('/')[1].ToString());
        if (txtCutoffDate.Text != "") 
        {
            strCutMonth = Convert.ToInt32(txtCutoffDate.Text.Split('/')[0].ToString());
            strCutyear = Convert.ToInt32(txtCutoffDate.Text.Split('/')[1].ToString());
            strPDay = Convert.ToInt32(DateTime.Now.Day);
            EOD = FunPubGetEndDayOfMonth(strCutyear, strCutMonth);

            if ((strCutyear >= Convert.ToInt32(DateTime.Now.Year)))
            {
                if (strCutMonth >= Convert.ToInt32(DateTime.Now.Month))
                {
                    if (strPDay < EOD)
                    {
                        Utility.FunShowAlertMsg(this, "CIBIL Cut off Month/Year Cannot be Created greater or Equal Current Month/Year");
                        txtCutoffDate.Text = "";
                        txtPath.Text = "";
                        return;
                    }
                }

            }
           txtPath.Text = Server.MapPath(".") + "\\" + StrFilePathName + "\\TEXT";
        }
    }
    protected void txtSheduledDate_TextChanged(object sender, EventArgs e)
    {
        int strCutMonth;
        int strCutyear = 0;
        int strSMonth,PMonth ;
        int strSDays;
        int strSYear;
        if ((txtCutoffDate.Text != "") && (txtSheduledDate.Text != ""))
        {
            strCutMonth = Convert.ToInt32(txtCutoffDate.Text.Split('/')[0].ToString());
            strCutyear = Convert.ToInt32(txtCutoffDate.Text.Split('/')[1].ToString());
            strSMonth = (Utility.StringToDate(txtSheduledDate.Text)).Month;
            strSYear = (Utility.StringToDate(txtSheduledDate.Text)).Year;
            strSDays = (Utility.StringToDate(txtSheduledDate.Text)).Day;
            int EOD = FunPubGetEndDayOfMonth(strSYear,strSMonth);
            if (strSYear <= Convert.ToInt32(strCutyear))
            {
                if (strSMonth <= Convert.ToInt32(strCutMonth))
                {
                    if (strSDays < EOD)
                    {
                        Utility.FunShowAlertMsg(this, "Sheduled Month/Year Cannot be Lesser or Equal to Cuttoff Month/Year");
                        txtSheduledDate.Text = "";
                        txtPath.Text = "";
                        return;
                    }
                }
               
            }
        }
    }
    public int FunPubGetEndDayOfMonth(int Year, int Month)
    {
        try
        {
            return (new DateTime(Year, (int)Month, DateTime.DaysInMonth(Year, (int)Month), 23, 59, 59, 999).Day);
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
    }

    #endregion

    public void PopulateCIBILData(int CIBIL_ID)
    {
        DateTime AMPM;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@intCIBIL_ID", intCIBILId.ToString());
        DataTable dtCIB = Utility.GetDefaultData("S3G_RPT_GetCIBILData", Procparam);
        DataRow dtRow = dtCIB.Rows[0];
        txtPath.Text = dtRow["Document_Path"].ToString();
        txtSheduledDate.Text = DateTime.Parse(dtRow["Schedule_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
        lblShedule.Visible = false;
        lblSheduleLater.Visible = false;
        lblSheduleNow.Visible = false;
        rdoSheduleLater.Visible = rdoSheduleNow.Visible = false;
        DateTime getTime;
        txtCutoffDate.Text = (dtRow["Months"].ToString() + '/' + dtRow["Years"].ToString());
        if (dtRow["Hours"].ToString().Length == 1)
        {
            txtHour.Text = "0" + dtRow["Hours"].ToString();
           
        }
        else
        {
            txtHour.Text = dtRow["Hours"].ToString();
            if (Convert.ToInt32(txtHour.Text) > 12)
            {
                txtHour.Text = (Convert.ToInt32(txtHour.Text) - 12).ToString();
                if (txtHour.Text.Length == 1)
                {
                    txtHour.Text = ("0" + txtHour.Text).ToString();
                }
            }
        }
        if (dtRow["Mins"].ToString().Length == 1)
        {
            txtMins.Text = "0" + dtRow["Mins"].ToString();
        }
        else
        {
            txtMins.Text = dtRow["Mins"].ToString();
            
        }
        AMPM = (Convert.ToDateTime(dtRow["Schedule_Date"].ToString()));
        ddlAMPM.SelectedValue = AMPM.ToString("tt");
        txtStatus.Text = dtRow["Status"].ToString(); 
        lnkExcel.Visible = true;
          //if (txtStatus.Text == "C")
          //{
               
          //}
          //else
          //{
          //    lnkText.Visible = lnkExcel.Visible = false;
          //}

        FunFileGeneration("PageLoad", dtRow["DownLoad_XL"].ToString(), dtRow["DownLoad_TXT"].ToString());
        if (dtRow["File_Status"].ToString() == "N")
        {
            btnGenerateFile.Enabled = false;
        }
    }

    protected void btnGenerateFile_Click(object sender, EventArgs e)
    {
        try
        {
            FunProGenerate(false);
            strAlert = "File Generation Initiated";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + strAlert + "');" + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            cvCIBIL.ErrorMessage = "Unable to generate CIBIL Report";
            cvCIBIL.IsValid = true;
            return;

        }
    }
}