//Module Name      :   Loan Admin
//Screen Name      :   S3GLoanAdOverDueInterest.aspx
//Created By       :   Irsathameen K
//Created Date     :   04-Sep-2010
//Purpose          :   To insert and Query Over Due Interest
// Modified By     :   Vijaya Kumar R
// Modified Date   :   29-12-2010
// Purpose         :   To Implement the Missing functionalities. 
#region Namespaces

using System;
using System.Web.UI;
using System.Data;
using System.Text;
using S3GBusEntity.LoanAdmin;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using LoanAdminAccMgtServicesReference;
using System.ServiceProcess;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
#endregion

public partial class LoanAdmin_S3GLoanAdOverDueInterestBW : ApplyThemeForProject
{
    #region Variable declaration

    string intOverdueInterestID = "0";
    int intErrCode = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    string strDate = DateTime.Now.ToString();
    public string strProgramId = "250";

    //User Authorization
    public string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    Dictionary<string, string> ObjDictionary = null;

    LoanAdminAccMgtServicesClient ObjLoanAdminAccMgtServicesClient = null;
    LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable ObjS3G_LOANAD_ODICalculationsDataTable = null;
    LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsRow ObjS3G_LOANAD_ODICalculationsRow = null;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;

    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=BWODI";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strDateFormat = string.Empty;
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";

    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdOverDueInterest.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=BWODI';";
    string strServiceName = "S3gService";
    static string strPageName = "Over Due Interest";

    #endregion

    #region Events

    #region Page Event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
            FunPubSetErrorMessage();

            if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
            {
                txtCurrentODIDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCurrentODIDate.ClientID + "','" + strDateFormat + "',true,  false);");
            }
            else
            {
                txtCurrentODIDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCurrentODIDate.ClientID + "','" + strDateFormat + "',false,  false);");
            }
            txtScheduleDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleDate.ClientID + "','" + strDateFormat + "',false,  true);");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad; // "Due to Data Problem, Unable to Load the Over Due Interest";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        txtCustomerName.Text = hidCustId.Value = "";
        try
        {
            hidCustId.Value = (ucCustomerCode.FindControl("hdnID") as HiddenField).Value;
            TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
            if (txtName.Text.Split('-').Length > 1)
                txtCustomerName.Text = txtName.Text.Split('-')[txtName.Text.Split('-').Length - 1].ToString();

            FunPriPopulatePANum();
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();
    
            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadPrimeACNum;
            cvOverDueInterest.IsValid = false;
        }
    }

    #endregion

    #region Button Events

    #region Common Control Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            #region Validation
            if (strMode != "Q")
            {
                if (!FunPriIsValid())
                {
                    return;
                }
            }
            #endregion
            FunPriSaveOverDueInterest();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_SaveODI; //"Unable to Insert the Over Due Interest";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearPage();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_ClearAll; //"Unable to Clear the data";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    #endregion

    #region Other Button Events

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriRevoke();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_ODIRevoke; //"Unable to Revoke the data";
            cvOverDueInterest.IsValid = false;
        }
    }
    #endregion

    #endregion

    #region DropDown List Events
    protected void ddlPrimeAccountNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtLastODICalculationDate.Text = "";
            FunPriPopulateSANum(ddlPrimeAccountNo.SelectedValue);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadSubACNum; //"Due to Data Problem, Unable to Load the SANum";
            cvOverDueInterest.IsValid = false;
        }
    }

    private void FunBindLastODIDate()
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (ddlBranch.SelectedIndex != 0) ObjDictionary.Add("@Location_ID", ddlBranch.SelectedValue);
            ObjDictionary.Add("@PANum", ddlPrimeAccountNo.SelectedValue);
            if (ddlSubAccountNo.SelectedValue != "0") ObjDictionary.Add("@SANum", ddlSubAccountNo.SelectedValue);
            else ObjDictionary.Add("@SANum", ddlPrimeAccountNo.SelectedValue + "DUMMY");
            DataTable dtLastODIDate = Utility.GetDefaultData("S3G_lOANAD_OVERDUE_LASTODI", ObjDictionary);

            if (dtLastODIDate != null && dtLastODIDate.Rows.Count > 0)
            {
                txtLastODICalculationDate.Text = dtLastODIDate.Rows[0]["LAST_ODI_DATE"].ToString();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlSubAccountNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtLastODICalculationDate.Text = "";
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();
            FunBindLastODIDate();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message; //"Due to Data Problem, Unable to Load the Customer";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriPopulateCustomer();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadCustomer; //"Due to Data Problem, Unable to Load the Customer";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();

        try
        {
            FunPriPopulateCustomer();
            FunPriBindLocation();

            if (ddlLOB.SelectedValue != "0")
            {
                ObjDictionary = new Dictionary<string, string>();
                ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
                ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
                txtODIRate.Text = objS3GAdminServicesClient.FunGetScalarValue("S3G_LOANAD_GetODIRate", ObjDictionary);
            }
            else
            {
                txtODIRate.Text = "";
            }
                        
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadCustomer; // "Due to Data Problem, Unable to Load the Customer";
            cvOverDueInterest.IsValid = false;
        }
        finally
        {
            objS3GAdminServicesClient.Close();
        }
    }
    #endregion

    #region Other Control Events

    protected void ChkHeadODI_CheckedChanged(object sender, EventArgs e)
    {
        string strLocation = "";
        try
        {
            CheckBox ChkHeadODI = (CheckBox)sender;
            foreach (GridViewRow gvRow in grvODI.Rows)
            {
                CheckBox ChkODI = gvRow.FindControl("ChkODI") as CheckBox;
                if (ChkODI.Enabled)
                {
                    if (ChkHeadODI.Checked)
                        ChkODI.Checked = true;
                    else
                        ChkODI.Checked = false;
                }
                if (ChkODI.Checked && (gvRow.FindControl("hidBillStatus") as HiddenField).Value == "0")
                {
                //    if (strLocation == "")
                //        strLocation = "(" + (gvRow.FindControl("txtBranchName") as Label).Text.Split('-')[1].ToString();
                //    else
                //        strLocation = strLocation + "," + (gvRow.FindControl("txtBranchName") as Label).Text.Split('-')[1].ToString();
                     if (strLocation == "")
                        strLocation = "(" + (gvRow.FindControl("txtBranchName") as Label).Text;
                    else
                        strLocation = strLocation + "," + (gvRow.FindControl("txtBranchName") as Label).Text;
                }
               
            }
            if (strLocation != "")
            {
                Utility.FunShowAlertMsg(this, "Billing is not done for " + strLocation + ")");
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ChkODI_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkODI = (CheckBox)sender;
            CheckBox ChkHeadODI = grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox;

            if (ChkODI.Checked)
            {
                HiddenField hidBillStatus = ((CheckBox)sender).Parent.Parent.FindControl("hidBillStatus") as HiddenField;
                if (hidBillStatus.Value.ToUpper() == "0")
                {
                    Utility.FunShowAlertMsg(this, "Billing not done for selected Location");
                }
            }

            if (!ChkODI.Checked) ChkHeadODI.Checked = false;
            else
            {
                ChkHeadODI.Checked = true;
                foreach (GridViewRow gvRow in grvODI.Rows)
                {
                    CheckBox ChkODINew = gvRow.FindControl("ChkODI") as CheckBox;
                    if (!ChkODINew.Checked && ChkODINew.Enabled)
                    {
                        ChkHeadODI.Checked = false;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ChkHeadRevoke_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkHeadRevoke = (CheckBox)sender;
            foreach (GridViewRow gvRow in grvODI.Rows)
            {
                CheckBox ChkRevoke = gvRow.FindControl("ChkRevoke") as CheckBox;
                if (ChkRevoke.Enabled)
                {
                    if (ChkHeadRevoke.Checked)
                        ChkRevoke.Checked = true;
                    else
                        ChkRevoke.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ChkRevoke_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkRevoke = (CheckBox)sender;
            CheckBox ChkHeadRevoke = grvODI.HeaderRow.FindControl("ChkHeadRevoke") as CheckBox;
            if (!ChkRevoke.Checked) ChkHeadRevoke.Checked = false;
            else
            {
                ChkHeadRevoke.Checked = true;
                foreach (GridViewRow gvRow in grvODI.Rows)
                {
                    CheckBox ChkRevokeNew = gvRow.FindControl("ChkRevoke") as CheckBox;
                    if (!ChkRevokeNew.Checked && ChkRevokeNew.Enabled)
                    {
                        ChkHeadRevoke.Checked = false;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void txtCurrentODIDate_TextChanged(object sender, EventArgs e)
    {
        panSchedule.Visible = false;
        grvODI.DataSource = null;
        grvODI.DataBind();
       
    }

    protected void btnCalculateODI_Click(object sender, EventArgs e)
    {
        try
        {
            //panSchedule.Visible = true;
            FunPriCalcualteODI();
           // panSchedule.Height = 100;
            //Panel1.Height = 100;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_CalculateODI;// "Due to Data Problem, Unable to Calulate ODI";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void rblAllSpecific_SelectedIndexChanged(object sender, EventArgs e)
    {
        panSchedule.Visible = false;
        grvODI.DataSource = null;
        grvODI.DataBind();
       
        txtLastODICalculationDate.Text = "";

        try
        {
            Button btnGetLOV = ucCustomerCode.FindControl("btnGetLOV") as Button;
            TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
            txtName.Text = "";
            
            if (rblAllSpecific.SelectedValue.ToUpper() == "ALL")
            {
                btnSave.Enabled = true;
                ddlPrimeAccountNo.Enabled = ddlSubAccountNo.Enabled = false;
                txtScheduleDate.ReadOnly = txtScheduleTime.ReadOnly = false;
                cmbCustomerCode.ReadOnly = true;
                btnGetLOV.Enabled = false;
                if (ddlPrimeAccountNo.Items.Count > 0) ddlPrimeAccountNo.SelectedIndex = 0;
                if (ddlSubAccountNo.Items.Count > 0) ddlSubAccountNo.SelectedIndex = 0;
                cmbCustomerCode.Text = hidCustId.Value = "";
                RFVOLEBranch.Enabled = RFVPrimeAccountNo.Enabled = RFVSubAccountNo.Enabled = false; //RFVCustomerCode.Enabled =
                rbtnSchedule.Enabled = RFVScheduleTime.Enabled = RFVScheduleDate.Enabled = REVScheduleTime.Enabled = CECScheduleDate.Enabled = true;
                txtCustomerName.Text = ""; //Source added by Tamilselvan.S on 27/01/2011
                ddlBranch.Enabled = rfvCALBranch.Enabled = rfvCALPrimeAC.Enabled = rfvCALsubAC.Enabled = false;//= rfvCALcustomerCode.Enabled
                ddlBranch.SelectedIndex = 0;
            }
            else
            {
                //btnSave.Enabled = false;
                ddlPrimeAccountNo.Enabled = ddlSubAccountNo.Enabled = true;
                cmbCustomerCode.ReadOnly = false;
                txtScheduleDate.ReadOnly = txtScheduleTime.ReadOnly = true;
                RFVOLEBranch.Enabled = RFVPrimeAccountNo.Enabled = RFVSubAccountNo.Enabled = true; //RFVCustomerCode.Enabled =
                rbtnSchedule.Enabled = RFVScheduleTime.Enabled = RFVScheduleDate.Enabled = REVScheduleTime.Enabled = CECScheduleDate.Enabled = false;
                txtScheduleDate.Text = txtScheduleTime.Text = "";
                ddlBranch.Enabled = rfvCALBranch.Enabled = rfvCALPrimeAC.Enabled = rfvCALsubAC.Enabled = true;// = rfvCALcustomerCode.Enabled
                btnGetLOV.Enabled = true;
                txtCurrentODIDate.Text = DateTime.Today.ToString(strDateFormat);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadODI;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void cmbCustomerCode_TextChanged(object sender, EventArgs e)
    {
        txtCustomerName.Text = hidCustId.Value = "";
        try
        {
            //Source modified by Tamilselvan.S on 27/01/2011
            if (System.Web.HttpContext.Current.Session["CustomerDT"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["CustomerDT"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string filterExpression = "Customer_Code = '" + cmbCustomerCode.Text + "'";
                    DataRow[] dtSuggestions = dt.Select(filterExpression);
                    if (dtSuggestions.Length > 0)
                    {
                        hidCustId.Value = dtSuggestions[0]["Customer_ID"].ToString();
                        txtCustomerName.Text = dtSuggestions[0]["Customer_Name"].ToString();
                    }
                }
                FunPriPopulatePANum();
                panSchedule.Visible = false;
                grvODI.DataSource = null;
                grvODI.DataBind();
                
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadPrimeACNum;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void grvODI_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField ODIStatus = (HiddenField)e.Row.FindControl("ODIStatus");
                CheckBox ChkODI = (CheckBox)e.Row.FindControl("ChkODI");
                HiddenField hidRevoke = (HiddenField)e.Row.FindControl("hidRevoke");
                CheckBox ChkRevoke = (CheckBox)e.Row.FindControl("ChkRevoke");

                if (ODIStatus.Value == "1")
                {
                    ChkODI.Checked = true;
                    ChkODI.Enabled = false;
                }
                if (hidRevoke.Value == "1")
                {
                    ChkRevoke.Checked = true;
                    ChkRevoke.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void rbtnSchedule_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbtnSchedule.SelectedValue == "0")
            {
                txtScheduleDate.Enabled = txtScheduleTime.Enabled = CECScheduleDate.Enabled = true;
                txtScheduleDate.Text = strDate;
                REVScheduleTime.Enabled = RFVScheduleDate.Enabled = RFVScheduleTime.Enabled = true;
            }
            else
            {
                txtScheduleDate.Text = txtScheduleTime.Text = "";
                txtScheduleDate.Enabled = txtScheduleTime.Enabled = CECScheduleDate.Enabled = false;
                REVScheduleTime.Enabled = RFVScheduleDate.Enabled = RFVScheduleTime.Enabled = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    #endregion

    #endregion

    #region Methods

    private void FunPriRevoke()
    {
        string strODIId = string.Empty;
        foreach (GridViewRow gvRow in grvODI.Rows)
        {
            CheckBox ChkRevoke = (CheckBox)gvRow.FindControl("ChkRevoke");
            HiddenField ODIID = (HiddenField)gvRow.FindControl("ODIID");
            if (ChkRevoke.Checked && ChkRevoke.Enabled)
            {
                if (strODIId == string.Empty)
                    strODIId = ODIID.Value;
                else
                    strODIId = strODIId + "," + ODIID.Value;
            }
        }

        if (String.IsNullOrEmpty(strODIId))
        {
            cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select atleast one location for Revoke ";
            cvOverDueInterest.IsValid = false;
            return;
        }

        ObjLoanAdminAccMgtServicesClient = new LoanAdminAccMgtServicesClient();
        try
        {
            string strResult = ObjLoanAdminAccMgtServicesClient.FunPubRevokeODI(intCompanyID, strODIId, intUserID);
            if (strResult == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + Resources.ValidationMsgs.S3G_SucMsg_RevokeODI + "');" + strRedirectPageView, true);
            }
            else if (strResult == "5")
            {
                Utility.FunShowAlertMsg(this.Page, "Month is locked.Unable to Revoke Over Due Interest");
                return;
            }
            else if (strResult == "6")
            {
                Utility.FunShowAlertMsg(this.Page, "Previous month is not locked.Unable to Revoke Over Due Interest");
                return;
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Error in Revoking");
                return;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_RevokeODI;
            cvOverDueInterest.IsValid = false;
        }
        finally
        {
            if (ObjLoanAdminAccMgtServicesClient != null)
                ObjLoanAdminAccMgtServicesClient.Close();
        }
    }

    private void FunPriCalcualteODI()
    {
        

        TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
        if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
        {
            if (ddlPrimeAccountNo.Items.Count == 0) txtName.Focus();
            if (txtName.Text.Trim() == "")
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select the Customer Code";
                cvOverDueInterest.IsValid = false;
                return;
            }
            if (ddlPrimeAccountNo.Items.Count == 0)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select the Prime Account No";
                cvOverDueInterest.IsValid = false;
                return;
            }
        }
        if (txtCurrentODIDate.Text != "")
        {
            if (Utility.CompareDates(txtCurrentODIDate.Text, strDate) == -1)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalDate_NGT_CD; //" Current ODI Calculation Date should not be greater than Current Date  ";
                cvOverDueInterest.IsValid = false;
                return;
            }
            //if (rblAllSpecific.SelectedValue.ToUpper() == "ALL")
            //{
                if (txtLastODICalculationDate.Text != "")
                {
                    int intDateDiff = Utility.CompareDates(txtCurrentODIDate.Text, txtLastODICalculationDate.Text);
                    if (intDateDiff == 0 || intDateDiff == 1)
                    {
                        cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalDate_GT_LastODI; //" Current ODI Calculation Date should be greater than Last ODI Calculation Date  ";
                        cvOverDueInterest.IsValid = false;
                        return;
                    }
                }
           // }
        }

        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ObjDictionary.Add("@Lob_Id", ddlLOB.SelectedValue);
            ObjDictionary.Add("@Is_Active", "1");

            DataTable dtGPSODI = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterOverdue", ObjDictionary);
            if (dtGPSODI != null && dtGPSODI.Rows.Count > 0)
            {
                if (dtGPSODI.Rows[0]["Parameter_Value"].ToString() == "0")
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Denominator Days can not be zero in Global Parameter Setup";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
                else if (dtGPSODI.Rows[0]["Parameter_Value"].ToString() == "")
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Assign the Denominator Days  in global parameter setup ";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
                else if (dtGPSODI.Rows[0]["Parameter_Value1"].ToString() == "")
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Assign the grace days  in global parameter setup ";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
                else if (dtGPSODI.Rows[0]["Parameter_Value2"].ToString() == "")
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Assign the rate in global parameter setup ";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
            }
            else
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Assign the Denominator Days,Rate and Grace days in global parameter setup ";
                cvOverDueInterest.IsValid = false;
                return;

            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_CalODI;
            cvOverDueInterest.IsValid = false;
        }

        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ObjDictionary.Add("@Lob_Id", ddlLOB.SelectedValue);
            if (ddlBranch.SelectedIndex != 0 && ddlBranch.Items.Count > 0) ObjDictionary.Add("@Location_Id", ddlBranch.SelectedValue);
            if (ddlPrimeAccountNo.SelectedIndex != 0 && ddlPrimeAccountNo.Items.Count > 0) ObjDictionary.Add("@PANum", ddlPrimeAccountNo.SelectedValue);
            if (ddlSubAccountNo.SelectedIndex != 0 && ddlSubAccountNo.Items.Count > 0) ObjDictionary.Add("@SANum", ddlSubAccountNo.SelectedValue);
            if (hidCustId.Value != "") ObjDictionary.Add("@Customer_ID", hidCustId.Value);
            ObjDictionary.Add("@ODIDATE", Utility.StringToDate(txtCurrentODIDate.Text).ToString());
            ObjDictionary.Add("@Type", rblAllSpecific.SelectedValue);
            ObjDictionary.Add("@Mode", "1");
            ObjDictionary.Add("@Is_BW", "1");

            DataSet dsCalCulateODI = Utility.GetDataset("S3G_LOANAD_OVERDUECALCULATION", ObjDictionary);

            if (dsCalCulateODI.Tables.Count == 1)//New Code
            {//New Code
                if(Convert .ToInt32(dsCalCulateODI.Tables[0].Rows[0][0])==0)
                {
                    Utility.FunShowAlertMsg(this.Page, "To Calculate ODI - Current Month Should be Opened and Previous Month Should be Locked");
                    //To Calculate ODI Current Month Should be in Open Status and Previous Month Should be Locked
                    return;
                }
            }
            else
            {
                panSchedule.Visible = true;
                //-----------------------------------
                if (dsCalCulateODI.Tables[1] != null && dsCalCulateODI.Tables[1].Rows.Count > 0)
                {
                    //panSchedule.Visible = true;
                    grvODI.DataSource = dsCalCulateODI.Tables[1];
                    grvODI.DataBind();
                    FunSetHeight();
                    if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
                    {
                        if (dsCalCulateODI.Tables[1].Rows[0]["Last_Calculated_Date"].ToString() != "")
                            txtLastODICalculationDate.Text = DateTime.Parse(dsCalCulateODI.Tables[1].Rows[0]["Last_Calculated_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);// ; ;
                    }
                }
                else
                {
                    //panSchedule.Visible = false;

                    grvODI.DataSource = null;
                    grvODI.EmptyDataText = "No Over Due for the Current ODI Calculation Date";
                    grvODI.DataBind();


                    //if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
                    //{
                    //    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Account Activation Date should be less than ODI Calculation Date";
                    //    cvOverDueInterest.IsValid = false;
                    //    return;
                    //}
                    //-------------------
                    //cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + "Records does not exist ";
                    //cvOverDueInterest.ErrorMessage = "No Over Due for the Current ODI Calculation Date";
                    //cvOverDueInterest.IsValid = false;
                    Panel1.Height = 50;
                    //-------------------
                }
                FunPriCtrlDisable();
                //if (grvODI.Rows.Count == 1)
                //{
                //    Panel1.Height = 50;
                //}
//-----------------------------------
            }//New Code
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_CalODI;
            cvOverDueInterest.IsValid = false;
            //throw new ApplicationException("Unable to calculate the Over Due Interest");
        }
        finally
        {
            ObjDictionary = null;
        }
    }

    protected void FunPriBindLocation()
    {
        //Branch
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (intOverdueInterestID == "0") ObjDictionary.Add("@Is_Active", "1");
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ObjDictionary.Add("@Program_ID", strProgramId);
            if (ddlLOB.SelectedValue != "0" && ddlLOB.SelectedValue != "") ObjDictionary.Add("@LOb_ID", ddlLOB.SelectedValue);
            ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, ObjDictionary, new string[] { "Location_ID", "Location_Code", "Location_Name" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadODI;
            cvOverDueInterest.IsValid = false;
            //throw new ApplicationException("Unable to Load the Over Due Interest");
        }
    }


    protected void FunPriBindBranchLOB()
    {
        //Branch
        try
        {
            //if (ObjDictionary != null)
            //    ObjDictionary.Clear();
            //else
            //    ObjDictionary = new Dictionary<string, string>();

            //ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            //if (intOverdueInterestID == "0") ObjDictionary.Add("@Is_Active", "1");
            //ObjDictionary.Add("@User_ID", intUserID.ToString());
            //ObjDictionary.Add("@Program_ID", strProgramId);
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, ObjDictionary, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //ObjDictionary = null;

            //LOB
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Program_ID", strProgramId);
            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (intOverdueInterestID == "0") ObjDictionary.Add("@Is_Active", "1");
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ddlLOB.BindDataTable(SPNames.S3G_LOANAD_GetOverDueInterestLOB, ObjDictionary, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            FunPriBindLocation();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadODI;
            cvOverDueInterest.IsValid = false;
            //throw new ApplicationException("Unable to Load the Over Due Interest");
        }
    }

    private void FunPriPopulatePANum()
    {
        ddlSubAccountNo.Items.Clear();

        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@Customer_ID", hidCustId.Value);
            ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (intOverdueInterestID == "0") ObjDictionary.Add("@Is_Active", "1");
            if (ddlBranch.SelectedIndex != 0) ObjDictionary.Add("@Location_ID", ddlBranch.SelectedValue);
            ddlPrimeAccountNo.BindDataTable("S3G_lOANAD_OVERDUE_PANUM", ObjDictionary, new string[] { "PANum", "PANum" });
            
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();
            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to Load the Prime Account No");
        }
    }

    private void FunPriPopulateSANum(string strPAN)
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@Customer_ID", hidCustId.Value);
            ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (ddlBranch.SelectedIndex != 0) ObjDictionary.Add("@Location_ID", ddlBranch.SelectedValue);
            ObjDictionary.Add("@Param2", ddlPrimeAccountNo.SelectedValue);
            ObjDictionary.Add("@Type", "Type6");
            if (intOverdueInterestID == "0") ObjDictionary.Add("@Param1", "2");
            else ObjDictionary.Add("@Param1", "3");
            ddlSubAccountNo.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_List, ObjDictionary, new string[] { "SANum", "SANum" });
            
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();
            

            if (ddlSubAccountNo.Items.Count > 1)
                RFVSubAccountNo.Enabled = rfvCALsubAC.Enabled = true;
            else
            {
                RFVSubAccountNo.Enabled = rfvCALsubAC.Enabled = false;
                FunBindLastODIDate();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            // throw new ApplicationException("Unable to Load the Sub Account No");
        }
    }

    private void FunPriPopulateCustomer()
    {
        TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
        txtName.Text = "";
        hidCustId.Value = cmbCustomerCode.Text = txtLastODICalculationDate.Text = "";
        txtCustomerName.Text = ""; //Source added by Tamilselvan.S on 27/01/2011
        ddlPrimeAccountNo.Items.Clear();
        ddlSubAccountNo.Items.Clear();
        panSchedule.Visible = false;
        grvODI.DataSource = null;
        grvODI.DataBind();
        
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            if (ddlBranch.SelectedIndex != 0) ObjDictionary.Add("@Location_ID", ddlBranch.SelectedValue);
            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            //System.Web.HttpContext.Current.Session["CustomerDT"] = Utility.GetDefaultData(SPNames.S3G_CLN_GetCustomer, ObjDictionary);// true, "--All Customers--", new string[] { "Customer_ID", "Customer_Code", "Customer_Name" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to Load the Customer");
        }
    }

    private void FunPriLoadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            ServiceController sc;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            // Change the Date Format
            //Begin
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            strDate = DateTime.Now.ToString(strDateFormat);
            CECCurODICalcDate.Format = CECScheduleDate.Format = strDateFormat;
            //End

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intOverdueInterestID = fromTicket.Name;
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end  

            if (!IsPostBack)
            {
                txtCurrentODIDate.Text = txtScheduleDate.Text = strDate;
                CECCurODICalcDate.Format = CECScheduleDate.Format = strDateFormat;
                FunPriBindBranchLOB();
                //User Authorization            
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                if ((intOverdueInterestID != "0") && (strMode == "M")) // Query // Modify
                {
                    FunPriGetOverDueInterest();
                    FunPriDisableControls(3);
                }
                else if ((intOverdueInterestID != "0") && (strMode == "Q")) // Query // Modify
                {
                    FunPriGetOverDueInterest();
                    FunPriDisableControls(2);
                }
                
            }
            #region "User Control"

            TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
            txtName.Width = System.Web.UI.WebControls.Unit.Pixel(170);
            Button btnGetLOV = ucCustomerCode.FindControl("btnGetLOV") as Button;
            if (rblAllSpecific.SelectedValue.ToUpper() == "ALL" || strMode == "M" || strMode == "Q")
                btnGetLOV.Enabled = false;
            else
                btnGetLOV.Enabled = true;
            txtName.Attributes.Add("ReadOnly", "true");
            txtName.Attributes.Add("onfocus", "fnLoadCustomer();");
           
            ucCustomerCode.strLOBID = ddlLOB.SelectedValue;
            if (ddlBranch.SelectedValue != "0") ucCustomerCode.strBranchID = ddlBranch.SelectedValue;
            ucCustomerCode.strControlID = ucCustomerCode.ClientID.ToString();

            FunSetHeight();
           
            #endregion
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            // throw new ApplicationException("Unable to Load the Over Due Interest");
        }
    }

    private void FunSetHeight()
    {
        int i = grvODI.Rows.Count;
        switch (i)
        {
            case 1:
                Panel1.Height = 62;
                break;
            case 2:
                Panel1.Height = 85;
                break;
            case 3:
                Panel1.Height = 110;
                break;
            case 4:
                Panel1.Height = 132;
                break;
            case 5:
                Panel1.Height = 155;
                break;
            case 6:
                Panel1.Height = 178;
                break;
            default :
                Panel1.Height = 200;
                break;
        }
    }

    private void FunPriGetOverDueInterest()
    {
        try
        {
           
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@ODI_Calculation_ID", intOverdueInterestID.ToString());
            ObjDictionary.Add("@User_ID", intUserID.ToString());

            DataSet dsODI = Utility.GetDataset("S3G_LoanAd_GetOverDueInterest", ObjDictionary);
            if (dsODI.Tables[0] != null && dsODI.Tables[0].Rows.Count > 0)
            {
                ddlLOB.SelectedValue = dsODI.Tables[0].Rows[0]["LOB_ID"].ToString();
                txtCurrentODIDate.Text = DateTime.Parse(dsODI.Tables[0].Rows[0]["ODI_Calculation_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);// ;
                //Added For ODI Rate//
                if (dsODI.Tables[5].Rows.Count >0)
                {
                    txtODIRate.Text = dsODI.Tables[5].Rows[0]["ODI_Intrest_Rate"].ToString();
                    //if(dsODI.Tables[5].Rows[0]["Last_Calculated_Date"].ToString()!=null )
                    //txtLastODICalculationDate.Text = dsODI.Tables[5].Rows[0]["Last_Calculated_Date"].ToString();
                }
               //Added For ODI Rate//
                if (dsODI.Tables[0].Rows[0]["ODI_Calculation_Level"].ToString().ToUpper() == "S")
                {
                    rblAllSpecific.SelectedValue = "Specific";
                    if (dsODI.Tables[1] != null && dsODI.Tables[1].Rows.Count > 0)
                    {
                        txtScheduleDate.Text = "";
                        ddlBranch.SelectedValue = dsODI.Tables[1].Rows[0]["Location_ID"].ToString();
                        cmbCustomerCode.Text = dsODI.Tables[1].Rows[0]["Customer_Code"].ToString();
                        TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
                        txtName.Text = dsODI.Tables[1].Rows[0]["Customer_Code"].ToString();
                        hidCustId.Value = dsODI.Tables[1].Rows[0]["Customer_ID"].ToString();
                        string[] arrCustomerCode = dsODI.Tables[1].Rows[0]["Customer_Code"].ToString().Split('-');
                        if (arrCustomerCode.Length != 0)
                            txtCustomerName.Text = arrCustomerCode[arrCustomerCode.Length - 1].Trim();
                        FunPriPopulatePANum();
                        ddlPrimeAccountNo.SelectedValue = dsODI.Tables[1].Rows[0]["PANum"].ToString();
                        FunPriPopulateSANum(dsODI.Tables[1].Rows[0]["PANum"].ToString());
                        ddlSubAccountNo.SelectedValue = dsODI.Tables[1].Rows[0]["SANum"].ToString();
                        if (dsODI.Tables[1].Rows[0]["Last_Calculated_Date"].ToString() != "") txtLastODICalculationDate.Text = DateTime.Parse(dsODI.Tables[1].Rows[0]["Last_Calculated_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);// ; 
                    }
                }
                if (dsODI.Tables.Count > 2 && dsODI.Tables[2] != null)
                {
                    panSchedule.Visible = true;
                    grvODI.DataSource = dsODI.Tables[2];
                    grvODI.DataBind();
                    FunSetHeight();
                }
                if (dsODI.Tables.Count > 3 && dsODI.Tables[3] != null && dsODI.Tables[3].Rows.Count > 0 && rblAllSpecific.SelectedValue.ToUpper() == "ALL")
                {
                    txtScheduleDate.Text = DateTime.Parse(dsODI.Tables[3].Rows[0]["Schedule_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    txtScheduleTime.Text = Convert.ToDateTime(dsODI.Tables[3].Rows[0]["Schedule_Date"].ToString()).ToShortTimeString();
                }
                FunPriCtrlDisable();
                if (dsODI.Tables.Count > 4 && dsODI.Tables[4] != null && dsODI.Tables[4].Rows.Count > 0)
                {
                    btnRevoke.Visible = true;
                    grvODI.Columns[5].Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to View the Over Due Interest details");
        }
    }

    private void FunPriCtrlDisable()
    {
        try
        {
            int intRevokeCnt = 0, intODICnt = 0;
            foreach (GridViewRow gvRow in grvODI.Rows)
            {
                CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
                CheckBox ChkRevoke = (CheckBox)gvRow.FindControl("ChkRevoke");
                if (!ChkODI.Enabled) intODICnt++;
                if (!ChkRevoke.Enabled) intRevokeCnt++;
            }
            if (intODICnt == grvODI.Rows.Count)
            {
                if (rblAllSpecific.SelectedValue.ToUpper() == "ALL") btnSave.Enabled = false;
                CheckBox ChkHeadODI = (CheckBox)grvODI.HeaderRow.FindControl("ChkHeadODI");
                if (ChkHeadODI != null)
                {
                    ChkHeadODI.Enabled = false;
                    ChkHeadODI.Checked = true;
                }
            }
            else
            {
                //if (rblAllSpecific.SelectedValue.ToUpper() == "ALL") 
                    btnSave.Enabled = true;
            }

            if (intRevokeCnt == grvODI.Rows.Count)
            {
                btnRevoke.Enabled = false;
                CheckBox ChkHeadRevoke = (CheckBox)grvODI.HeaderRow.FindControl("ChkHeadRevoke");
                if (ChkHeadRevoke != null)
                {
                    ChkHeadRevoke.Enabled = false;
                    ChkHeadRevoke.Checked = true;
                }
            }
            else
                btnRevoke.Enabled = true;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriClearPage()
    {
        try
        {
            if (ddlBranch.Items.Count > 0) ddlBranch.SelectedIndex = 0;
            if (ddlPrimeAccountNo.Items.Count > 0) ddlPrimeAccountNo.SelectedIndex = 0;
            if (ddlSubAccountNo.Items.Count > 0) ddlSubAccountNo.SelectedIndex = 0;
            if (ddlLOB.Items.Count > 0) ddlLOB.SelectedIndex = 0;
            //if (ddlCustomerCode.Items.Count > 0)ddlCustomerCode.SelectedIndex = 0;            
            rblAllSpecific.SelectedValue = "All";
            txtODIRate.Text= txtCustomerName.Text = txtScheduleTime.Text = hidCustId.Value = cmbCustomerCode.Text = txtLastODICalculationDate.Text = "";
            txtScheduleDate.Text = txtCurrentODIDate.Text = strDate;
            ddlPrimeAccountNo.Enabled = ddlSubAccountNo.Enabled = RFVOLEBranch.Enabled = false;
            txtScheduleDate.ReadOnly = txtScheduleTime.ReadOnly = ddlBranch.Enabled = false;
            RFVScheduleTime.Enabled = RFVScheduleDate.Enabled = REVScheduleTime.Enabled = false;
            cmbCustomerCode.ReadOnly = CECScheduleDate.Enabled = rbtnSchedule.Enabled = btnSave.Enabled = true;
            rbtnSchedule.SelectedValue = "0";
            rfvCALBranch.Enabled = RFVOLEBranch.Enabled = false; //= RFVCustomerCode.Enabled = rfvCALcustomerCode.Enabled
            RFVPrimeAccountNo.Enabled = rfvCALPrimeAC.Enabled = RFVSubAccountNo.Enabled = rfvCALsubAC.Enabled = false;
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();
            TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
            txtName.Text = "";
            Button btnGetLOV = ucCustomerCode.FindControl("btnGetLOV") as Button;
            if (rblAllSpecific.SelectedValue.ToUpper() == "ALL") btnGetLOV.Enabled = false;
            else btnGetLOV.Enabled = true;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to Clear the Over Due Interest");
        }
    }

    private void FunPriSaveOverDueInterest()
    {
        string strBranchId = string.Empty;
        foreach (GridViewRow gvRow in grvODI.Rows)
        {
            CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
            Label txtBranchId = (Label)gvRow.FindControl("txtBranchId");
            if (ChkODI.Checked && ChkODI.Enabled)
            {
                if (strBranchId == string.Empty)
                    strBranchId = txtBranchId.Text;
                else
                    strBranchId = strBranchId + "," + txtBranchId.Text;
            }
        }

        ObjLoanAdminAccMgtServicesClient = new LoanAdminAccMgtServicesClient();

        string strErrorLog = string.Empty;
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ObjDictionary.Add("@Lob_Id", ddlLOB.SelectedValue);
            ObjDictionary.Add("@ODIDATE", Utility.StringToDate(txtCurrentODIDate.Text).ToString());
            //Added and commented by saran on 30-Jul-2013 start
            ObjDictionary.Add("@Is_BW", "1");
            //Added and commented by saran on 30-Jul-2013 end
            if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
            {
                ObjDictionary.Add("@Location_Id", strBranchId);
                if (ddlPrimeAccountNo.SelectedIndex != 0 && ddlPrimeAccountNo.Items.Count > 0) ObjDictionary.Add("@PANum", ddlPrimeAccountNo.SelectedValue);
                if (ddlSubAccountNo.SelectedIndex != 0 && ddlSubAccountNo.Items.Count > 0) ObjDictionary.Add("@SANum", ddlSubAccountNo.SelectedValue);
                if (hidCustId.Value != "") ObjDictionary.Add("@Customer_ID", hidCustId.Value);
                ObjDictionary.Add("@Type", rblAllSpecific.SelectedValue);
                //Source modified by Tamilselvan.S on 27/01/2011
                int intResult = ObjLoanAdminAccMgtServicesClient.FunPubSaveODICalculations(out strErrorLog, ObjSerMode, ClsPubSerialize.Serialize(ObjDictionary, ObjSerMode));
                if (intResult == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.S3G_ErrMsg_ODIMonthLockLOB); //"Month was locked for the selected LOB.Unable to Save Over Due Interest");
                    return;
                }
                else if (intResult == 6)
                {
                    Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.S3G_ErrMsg_ODIPrevMonthLockLOB);
                    return;
                }
                else if (intResult == 10)
                {
                    Utility.FunShowAlertMsg(this.Page, "ODI - Memo type was not added in the Memo Master");
                    return;
                }
                else if (intResult == 0)
                {
                    strAlert = Resources.ValidationMsgs.S3G_SucMsg_SaveODI; //"Over Due Interest added successfully  ";
                    strAlert += Resources.ValidationMsgs.S3G_ValMsg_Next; //@"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END

                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            else
            {
                ObjDictionary.Add("@Location_ID", strBranchId);
                if (rbtnSchedule.SelectedValue == "0")
                {
                    ObjDictionary.Add("@Schedule_Date", Utility.StringToDate(txtScheduleDate.Text).ToShortDateString());
                    ObjDictionary.Add("@Schedule_Time", txtScheduleTime.Text);
                }
                else
                {
                    ObjDictionary.Add("@Schedule_Date", DateTime.Now.ToShortDateString());
                    ObjDictionary.Add("@Schedule_Time", DateTime.Now.AddMinutes(5.0).ToShortTimeString());
                }
                int intResult = ObjLoanAdminAccMgtServicesClient.FunPubSaveODISchedule(ObjSerMode, ClsPubSerialize.Serialize(ObjDictionary, ObjSerMode));
                if (intResult == 10)
                {
                    Utility.FunShowAlertMsg(this.Page, "ODI - Memo type was not added in the Memo Master");
                    return;
                }
                if (intResult == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, "Month is locked");
                    return;
                }
                else if (intResult == 6)
                {
                    Utility.FunShowAlertMsg(this.Page, "Previous month is not locked ");
                    return;
                }
                else if (intResult == 11)
                {
                    Utility.FunShowAlertMsg(this.Page, "Over Due Interest should be calculated last day of the month");
                    return;
                }
                else if (intResult == 0)
                {
                    strAlert = Resources.ValidationMsgs.S3G_SucMsg_SaveODI;//"Over Due Interest added successfully  ";
                    strAlert += Resources.ValidationMsgs.S3G_ValMsg_Next; //@"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {
            ObjDictionary = null;
            ObjLoanAdminAccMgtServicesClient.Close();
        }
    }

    #region "User Authorization"
    ////This is used to implement User Authorization
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                //TextBox txtname1 = (TextBox)ucCustomerCode.FindControl("txtName");
                //txtname1.Enabled = false;
                //Button btnGetLOV1 = ucCustomerCode.FindControl("btnGetLOV") as Button;
                //btnGetLOV1.Enabled = false;
                break;

            case 3:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnSave.Enabled = btnClear.Enabled = btnCalculateODI.Visible = false;
                ddlLOB.Enabled = ddlBranch.Enabled = imgCurrentODIDate.Visible = imgScheduleDate.Visible = false;
                txtCurrentODIDate.Width = ddlBranch.Width;
                txtCurrentODIDate.Enabled = txtScheduleTime.Enabled = txtScheduleDate.Enabled = false;
                rblAllSpecific.Enabled = rbtnSchedule.Enabled = CECScheduleDate.Enabled = CECCurODICalcDate.Enabled = false;
                Button btnGetLOV = ucCustomerCode.FindControl("btnGetLOV") as Button;
                btnGetLOV.Enabled = false;

                if ((grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox) != null)
                    (grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox).Enabled = false;

                foreach (GridViewRow gvRow in grvODI.Rows)
                {
                    CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
                    ChkODI.Enabled = false;
                }
                grvODI.Columns[3].Visible = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                break;

            case 2:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnRevoke.Enabled = btnSave.Enabled = btnClear.Enabled = btnCalculateODI.Visible = false;
                ddlLOB.Enabled = ddlBranch.Enabled = imgCurrentODIDate.Visible = imgScheduleDate.Visible = false;
                txtCurrentODIDate.Width = ddlBranch.Width;
                txtCurrentODIDate.Enabled = txtScheduleTime.Enabled = txtScheduleDate.Enabled = false;
                rblAllSpecific.Enabled = rbtnSchedule.Enabled = CECScheduleDate.Enabled = CECCurODICalcDate.Enabled = false;

                if ((grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox) != null)
                    (grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox).Enabled = false;
                if ((grvODI.HeaderRow.FindControl("ChkHeadRevoke") as CheckBox) != null)
                    (grvODI.HeaderRow.FindControl("ChkHeadRevoke") as CheckBox).Enabled = false;

                foreach (GridViewRow gvRow in grvODI.Rows)
                {
                    CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
                    CheckBox ChkRevoke = (CheckBox)gvRow.FindControl("ChkRevoke");
                    ChkODI.Enabled = ChkRevoke.Enabled = false;
                }
                grvODI.Columns[3].Visible = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                break;
        }
    }
    ////Code end
    #endregion

    private bool FunPriIsValid()
    {
        bool blnIsValid = true;
        cvOverDueInterest.ErrorMessage = "";
        TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
        if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC" && txtName.Text.Trim() == "")
        {
            cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select the Customer Code";
            cvOverDueInterest.IsValid = false;
            blnIsValid = false;
            return false;
        }
        if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC" && ddlPrimeAccountNo.Items.Count == 0)
        {
            cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select the Prime Account No";
            cvOverDueInterest.IsValid = false;
            blnIsValid = false;
            return false;
        }
        if (txtCurrentODIDate.Text != "")
        {
            if (Utility.CompareDates(txtCurrentODIDate.Text, strDate) == -1)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalDate_NGT_CD;// " Current ODI Calculation Date should not be greater than Current Date  ";
                cvOverDueInterest.IsValid = false;
                blnIsValid = false;
                return false;
            }
            if (txtLastODICalculationDate.Text != "")
            {
                int intDateDiff = Utility.CompareDates(txtCurrentODIDate.Text, txtLastODICalculationDate.Text);
                if (intDateDiff == 0 || intDateDiff == 1)
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalDate_GT_LastODI; // " Current ODI Calculation Date should be greater than Last ODI Calculation Date  ";
                    cvOverDueInterest.IsValid = false;
                    blnIsValid = false;
                    return false;
                }
            }
        }

        if (rblAllSpecific.SelectedValue.ToUpper() == "ALL" && rbtnSchedule.SelectedValue == "0")
        {
            if (Utility.CompareDates(txtScheduleDate.Text, strDate) == 1)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ScheduleDate_GREQl;// " Schedule Date should be greater than or equal to Current Date  ";
                cvOverDueInterest.IsValid = false;
                blnIsValid = false;
                return false;
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
                    if (span.Hours == 0 && span.Minutes <= 5)
                    {
                        cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ScheduleTime_GR_5M;// " Schduled Time should be greater than 5 minutes for current date only ";
                        cvOverDueInterest.IsValid = false;
                        blnIsValid = false;
                        return false;
                    }
                }
            }
        }

        if (grvODI.Rows.Count == 0)
        {
            cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalNotExist; //" Records not exists for Over Due Calculation ";
            cvOverDueInterest.IsValid = false;
            blnIsValid = false;
            return false;
        }
        else
        {
            bool cntChk = false;
            foreach (GridViewRow gvRow in grvODI.Rows)
            {
                CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
                if (ChkODI.Enabled && ChkODI.Checked) cntChk = true;
            }
            if (cntChk == false)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_Atleast_1ODI;// " Please select atleast one branch in the grid for Over Due Calculation ";
                cvOverDueInterest.IsValid = false;
                blnIsValid = false;
                return false;
            }
        }
        return blnIsValid;
    }

    #region GetCustomerCode
    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetCustomerList(String prefixText, int count)
    {
        List<String> suggetions = null;
        DataTable dtCustomer = new DataTable();
        dtCustomer = (DataTable)System.Web.HttpContext.Current.Session["CustomerDT"];
        suggetions = GetSuggestions(prefixText, count, dtCustomer);
        return suggetions.ToArray();
    }
    #endregion

    #region GetSuggestions
    /// <summary>
    /// GetSuggestions
    /// </summary>
    /// <param name="key">Country Names to search</param>
    /// <returns>Country Names Similar to key</returns>
    private static List<String> GetSuggestions(string key, int count, DataTable dt1)
    {
        List<String> suggestions = new List<string>();
        try
        {
            string filterExpression = "Customer_Code like '" + key + "%'";
            DataRow[] dtSuggestions = dt1.Select(filterExpression);
            foreach (DataRow dr in dtSuggestions)
            {
                string suggestion = dr["Customer_Code"].ToString();
                suggestions.Add(suggestion);
            }
        }
        catch (Exception objException)
        {
            return suggestions;
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            //   lblErrorMessage.Text = Resources.LocalizationResources.CustomerTypeChangeError;
        }
        return suggestions;
    }
    #endregion

    #endregion

    #region [Set Error message]
    /// <summary>
    /// Created by Tamilselvan.S 
    /// Created date 27/01/2011
    /// </summary>
    public void FunPubSetErrorMessage()
    {
        RFVLOB.ErrorMessage = rfvCALLOB.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LOB;
        RFVOLEBranch.ErrorMessage = rfvCALBranch.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Branch;
        //RFVCustomerCode.ErrorMessage = rfvCALcustomerCode.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_CustomerCode;//
        RFVPrimeAccountNo.ErrorMessage = rfvCALPrimeAC.ErrorMessage = Resources.ValidationMsgs.CLNPDC_13;
        RFVSubAccountNo.ErrorMessage = rfvCALsubAC.ErrorMessage = Resources.ValidationMsgs.CLNPDC_14;
        RFVCurrentODIDate.ErrorMessage = rfvCALCurrentODIcalculationDate.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_ODICalculationDate;
        RFVScheduleDate.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_ODIScheduleDate;
        RFVScheduleTime.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_ODIScheduleTime;
        REVScheduleTime.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_ODIValidTime;
    }

    #endregion [Set Error message]


}