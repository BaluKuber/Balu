#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Escalation Details Creation
/// Created By			: Kannan RC
/// Created Date		: 15-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 12-July-2010   
/// Reason              : Add Role Access setup 
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;
using S3GBusEntity;
#endregion

public partial class System_Admin_S3G_SysAdminEscalationMaster_Add : ApplyThemeForProject
{
    #region Intialization
    //AccountMgtServicesReference.AccountMgtServicesClient ObjAccountMasterClient;
    //CompanyMgtServicesReference.CompanyMgtServicesClient objLOB_MasterClient;
    UserMgtServicesReference.UserMgtServicesClient ObjUserManageClient;
    int intErrCode = 0;
    int intEscalationId = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int inthdUserid;
    
    string strMode;
    bool bClearList = false;
    string strRedirectPageMC;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminEscalationMaster_View.aspx'";
    CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable ObjS3G_LOBMasterListDataTable = new CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable();
    UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable ObjS3G_RoleCodeMasterListDataTable = new UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable();
    UserMgtServices.S3G_SYSAD_RoleUser_ListDataTable ObjS3G_RoleUserMasterListDataTable = new UserMgtServices.S3G_SYSAD_RoleUser_ListDataTable();
    UserMgtServices.S3G_SYSAD_EscalationMasterDataTable ObjS3G_EscalationMasterDataTable = new UserMgtServices.S3G_SYSAD_EscalationMasterDataTable();
    UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable ObjS3G_EscalationMasterQueryDataTable = new UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["qsEscalId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsEscalId"));
            strMode = Request.QueryString.Get("qsMode");
            if (fromTicket != null)
            {
                intEscalationId = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }

        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        //bIsActive = ObjUserInfo.ProIsActiveRW;

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        if (!IsPostBack)
        {

            FunPriGetLOBList();
            //  FunPriGetRoleCodeList();
            #region MakerChecker
            /// <summary>
            /// This method used for Role Access
            /// </summary>
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if (((intEscalationId > 0)) && (strMode == "M"))
            {
                FunPriDisableControls(1);
            }
            else if (((intEscalationId > 0)) && (strMode == "Q"))
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriDisableControls(0);
            }
            #endregion
        }

       // FunPriSetMaxLength();
    }


    private void FunPriSetMaxLength()
    {
        txtNLDays.SetDecimalPrefixSuffix(2, 2, true, false, "NL Days");
        txtHLDays.SetDecimalPrefixSuffix(2, 2, true, false, "NL Days");
    }


    #region Dropdowmlist
    /// <summary>
    /// Bind dropdownlist for LOB,ROLECODE,USER
    /// </summary>
    private void FunPriGetLOBList()
    {
        
        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", intCompanyID.ToString());
        if (intEscalationId == 0)
        {
            dictParam.Add("@Is_Active", "1");
        }
        //Commented By Saranya based on Sudarsan Observation on 01/Feb/2012
        //if (intEscalationId == 0)
        //{
        //    dictParam.Add("@User_ID", intUserID.ToString());
        //}
        //end

        //Code needed to load the LOB's for particular user - based on User Access - Kuppusamy.B - 22-March-2012
        if (intEscalationId == 0)
        {
            dictParam.Add("@User_ID", intUserID.ToString());
        }

        dictParam.Add("@Program_ID","11"); //added later
        ddlLOB.BindDataTable("S3G_Get_LOB_LIST", dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        ddlLOB.AddItemToolTip();
    }


    private void FunPriLoadRolecode(string strLOBID)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        if (intEscalationId == 0)
        {
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", intUserID.ToString());
        }
        if (intUserID != 0)
            Procparam.Add("@LOB_ID", strLOBID);
        ////Added by Saranya to get the workflow programs.
        DataSet DSRoleCode = Utility.GetDataset(SPNames.SYS_RoleCodeMaster, Procparam);
        DataRow[] dr = DSRoleCode.Tables[0].Select("PROGRAM_ID IN(31,34,35,37,38,41,42,43,46,47,51,52,54,56,60,71,75,79,80)", "Program_Role", DataViewRowState.CurrentRows);
        DataTable DTRoleCode = dr.CopyToDataTable();
        //End Here
        ddlRoleCode.BindDataTable(DTRoleCode, new string[] { "Role_Code_ID", "Program_Role" });
        ddlRoleCode.AddItemToolTip();
    }

    protected void ddlRoleCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunGetUserDetails(Convert.ToInt32(ddlRoleCode.SelectedValue));
        AddTooltipForControls();
    }

    //Added by Saranya to validate the HL and NL Minis.
    protected void txtNLDays_TextChanged(object sender, EventArgs e)
    {
       
        int Mins;
        if ((!string.IsNullOrEmpty(txtNLDays.Text)) && txtNLDays.Text.Contains(".") && (txtNLDays.Text != "__.__") && (txtNLDays.Text != "00.") && (txtNLDays.Text != "0."))
        {
            Mins = Convert.ToInt32(txtNLDays.Text.Split('.')[1].ToString());
            if (Mins > 59)
            {
                strAlert = strAlert.Replace("__ALERT__", "Enter minutes less than 60");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                lblErrorMessage.Text = string.Empty;
                txtNLDays.Text = ""; //txtNLDays.Text.Split('.')[0].ToString();
                return;
            }
            else
            {
                CbxNLSms.Enabled = true;
                CbxNLEmail.Enabled = true;
                CbxNLcc1.Enabled = true;
            }
        }
        if (string.IsNullOrEmpty(txtNLDays.Text))
        {
            CbxNLSms.Enabled = false;
            CbxNLEmail.Enabled = false;
            CbxNLcc1.Enabled = false;
            CbxNLSms.Checked = false;
            CbxNLEmail.Checked = false;
            CbxNLcc1.Checked = false;
        }

        AddTooltipForControls();
        
    }

    protected void txtHLDays_TextChanged(object sender, EventArgs e)
    {
        int Mins;
        if ((!string.IsNullOrEmpty(txtHLDays.Text)) && txtHLDays.Text.Contains(".") && (txtHLDays.Text != "__.__") && (txtHLDays.Text != "00.") && (txtHLDays.Text != "0."))
        {

            Mins = Convert.ToInt32(txtHLDays.Text.Split('.')[1].ToString());
            if (Mins > 59)
            {
                strAlert = strAlert.Replace("__ALERT__", "Enter minutes less than 60");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                lblErrorMessage.Text = string.Empty;
                //txtHLDays.Text = txtHLDays.Text.Split('.')[0].ToString();
                txtHLDays.Text = "";
                return;
            }
            else
            {
                CbxHLSms.Enabled = true;
                CbxHLEmail.Enabled = true;
                CbxHLcc1.Enabled = true;
                CbxHLcc2.Enabled = true;
            }
        }

        if (string.IsNullOrEmpty(txtHLDays.Text))
        {
            CbxHLSms.Enabled = false;
            CbxHLEmail.Enabled = false;
            CbxHLcc1.Enabled = false;
            CbxHLcc2.Enabled = false;
            CbxHLSms.Checked = false;
            CbxHLEmail.Checked = false;
            CbxHLcc1.Checked = false;
            CbxHLcc2.Checked = false;
        }

        AddTooltipForControls();

    }

    //End Here
    private void FunGetUserDetails(int intRoleCodeID)
    {
        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", intCompanyID.ToString());
        dictParam.Add("@Role_Code_ID", intRoleCodeID.ToString());
        //Added by saranya to check LOB access for User
        if (ddlLOB.SelectedIndex > 0)
        {
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
        }
        if (intEscalationId == 0)
        {
            dictParam.Add("@Is_Active", "1");
        }
        

        ddlUser.BindDataTable("S3G_Get_RoleUser_List", dictParam, new string[] { "User_ID", "Users" });
        ddlUser.AddItemToolTip();
   }


    #endregion

    #region Create Escalation
    /// <summary>
    /// Create Escalation details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjUserManageClient = new UserMgtServicesReference.UserMgtServicesClient();

        try
        {
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminEscalationMaster_View.aspx';";
            string strRedirectPageAdd = "window.location.href='../System Admin/S3G_SysAdminEscalationMaster_Add.aspx';";
            if (ViewState["UserNotinRole"] != null)
            {
                strAlert = strAlert.Replace("__ALERT__", @"Selected User not assigned to this Role Code / \n Reporting Level not assigned to this User");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                lblErrorMessage.Text = string.Empty;
                return;
            }
            //added by saranya based on sudarsan observation to Validate NL Hours and HL Hours
            if (Convert.ToString(txtHLDays.Text) == string.Empty && Convert.ToString(txtNLDays.Text) == string.Empty)
            {
                strAlert = strAlert.Replace("__ALERT__", "Enter NL Hours/HL Hours.");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                lblErrorMessage.Text = string.Empty;
                return;
            }
            if (Convert.ToString(txtNLDays.Text) != string.Empty)
            {
                if (Convert.ToDecimal(txtNLDays.Text) == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "NL Hours should not be 0.");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
                if (!CbxNLSms.Checked && !CbxNLEmail.Checked && !CbxNLcc1.Checked)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Select NL SMS/NL EMail/NL CC1.");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
            }
            if (Convert.ToString(txtHLDays.Text) != string.Empty)
            {
                if (Convert.ToDecimal(txtHLDays.Text) == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "HL Hours should not be 0.");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
                if (!CbxHLSms.Checked && !CbxHLEmail.Checked && !CbxHLcc1.Checked && !CbxHLcc2.Checked)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Select HL SMS/HL EMail/HL CC1/HL CC2.");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
            }

            
            //end here

            CbxActive.Enabled = false;

            UserMgtServices.S3G_SYSAD_EscalationMasterRow ObjEscalationMasterRow;
            ObjEscalationMasterRow = ObjS3G_EscalationMasterDataTable.NewS3G_SYSAD_EscalationMasterRow();
            ObjEscalationMasterRow.Company_ID = intCompanyID;
            ObjEscalationMasterRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjEscalationMasterRow.Created_By = intUserID;
            ObjEscalationMasterRow.Created_On = DateTime.Now;
            ObjEscalationMasterRow.Modified_By = intUserID;
            ObjEscalationMasterRow.Modified_On = DateTime.Now;
            ObjEscalationMasterRow.Escalation_ID = intEscalationId;
            ObjEscalationMasterRow.Role_Code_ID = Convert.ToInt32(ddlRoleCode.SelectedValue);
            ObjEscalationMasterRow.User_ID = Convert.ToInt32(ddlUser.SelectedValue);
            if (Convert.ToString(txtNLDays.Text) != string.Empty)
            {
                ObjEscalationMasterRow.NL_Days = Convert.ToDecimal(txtNLDays.Text);
                ObjEscalationMasterRow.NL_SMS = CbxNLSms.Checked;
                ObjEscalationMasterRow.NL_Email = CbxNLEmail.Checked;
                ObjEscalationMasterRow.NL_CC = CbxNLcc1.Checked;
            }
          
            if (Convert.ToString(txtHLDays.Text) != string.Empty)
            {
                ObjEscalationMasterRow.HL_Days = Convert.ToDecimal(txtHLDays.Text);
                ObjEscalationMasterRow.HL_SMS = CbxHLSms.Checked;
                ObjEscalationMasterRow.HL_Email = CbxHLEmail.Checked;
                ObjEscalationMasterRow.HL_CC1 = CbxHLcc1.Checked;
                ObjEscalationMasterRow.HL_CC2 = CbxHLcc2.Checked;
            }

            ObjEscalationMasterRow.Is_Active = CbxActive.Checked;

            ObjS3G_EscalationMasterDataTable.AddS3G_SYSAD_EscalationMasterRow(ObjEscalationMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;

            if (intEscalationId > 0)
            {
                if (FunCheckLobisvalid(ddlLOB.SelectedValue))
                {
                    intErrCode = ObjUserManageClient.FunPubModifyEscalationMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_EscalationMasterDataTable, SerMode));
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Selected Line of Business rights not assigned");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    return;
                }
            }
            else
            {
                intErrCode = ObjUserManageClient.FunPubCreateEscalationMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_EscalationMasterDataTable, SerMode));
            }
            
            if (intErrCode == 0)
            {
                if (intEscalationId > 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Escalation details updated successfully");
                }
                else
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strAlert = "Escalation details added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    CbxActive.Checked = true;
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "This combination of escalation details already exists");
                strRedirectPageView = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjUserManageClient.Close();
        }
    }
    #endregion

    #region Get Escalation
    /// <summary>
    /// Get escaltion details using modify mode and query mode
    /// </summary>
    private void FunGetEscalationDetatils()
    {

        ObjUserManageClient = new UserMgtServicesReference.UserMgtServicesClient();

        try
        {

            UserMgtServices.S3G_SYSAD_Get_EscalationDetailsRow ObjEscalationMasterRow;
            ObjEscalationMasterRow = ObjS3G_EscalationMasterQueryDataTable.NewS3G_SYSAD_Get_EscalationDetailsRow();
            ObjEscalationMasterRow.Escalation_ID = intEscalationId;
            ObjEscalationMasterRow.Company_ID = intCompanyID;
            ObjS3G_EscalationMasterQueryDataTable.AddS3G_SYSAD_Get_EscalationDetailsRow(ObjEscalationMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteEscalationDetails = ObjUserManageClient.FunPubQueryEscalationMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_EscalationMasterQueryDataTable, SerMode));
            ObjS3G_EscalationMasterQueryDataTable = (UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable)ClsPubSerialize.DeSerialize(byteEscalationDetails, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable));

            ddlLOB.SelectedValue = ObjS3G_EscalationMasterQueryDataTable.Rows[0]["LOB_ID"].ToString();
            FunPriLoadRolecode(ddlLOB.SelectedValue);
            ddlRoleCode.SelectedValue = ObjS3G_EscalationMasterQueryDataTable.Rows[0]["Role_Code_ID"].ToString();

            FunGetUserDetails(Convert.ToInt32(ddlRoleCode.SelectedValue));
            ddlUser.SelectedValue = ObjS3G_EscalationMasterQueryDataTable.Rows[0]["USER_ID"].ToString();
            if (ddlUser.SelectedValue == "0")
            {
                ViewState["UserNotinRole"] = ObjS3G_EscalationMasterQueryDataTable.Rows[0]["USER_ID"].ToString();
                ListItem lstUser = new ListItem(ObjS3G_EscalationMasterQueryDataTable.Rows[0]["USERS"].ToString(), ObjS3G_EscalationMasterQueryDataTable.Rows[0]["USER_ID"].ToString());
                ddlUser.Items.Clear();
                ddlUser.Items.Add(lstUser);
            }
            else
            {
                ViewState.Remove("UserNotinRole");
            }
            hdnID.Value = ObjS3G_EscalationMasterQueryDataTable.Rows[0]["USER_ID"].ToString();
            txtNLDays.Text = ObjS3G_EscalationMasterQueryDataTable.Rows[0]["NL_Days"].ToString();
            if (ObjS3G_EscalationMasterQueryDataTable.Rows[0]["NL_SMS"].ToString() == "True")
                CbxNLSms.Checked = true;
            else
                CbxNLSms.Checked = false;

            if (ObjS3G_EscalationMasterQueryDataTable.Rows[0]["NL_Email"].ToString() == "True")
                CbxNLEmail.Checked = true;
            else
                CbxNLEmail.Checked = false;
            if (ObjS3G_EscalationMasterQueryDataTable.Rows[0]["NL_CC"].ToString() == "True")
                CbxNLcc1.Checked = true;
            else
                CbxNLcc1.Checked = false;
            txtHLDays.Text = ObjS3G_EscalationMasterQueryDataTable.Rows[0]["HL_Days"].ToString();
            if (ObjS3G_EscalationMasterQueryDataTable.Rows[0]["HL_SMS"].ToString() == "True")
                CbxHLSms.Checked = true;
            else
                CbxHLSms.Checked = false;
            if (ObjS3G_EscalationMasterQueryDataTable.Rows[0]["HL_Email"].ToString() == "True")
                CbxHLEmail.Checked = true;
            else
                CbxHLEmail.Checked = false;
            if (ObjS3G_EscalationMasterQueryDataTable.Rows[0]["HL_CC1"].ToString() == "True")
                CbxHLcc1.Checked = true;
            else
                CbxHLcc1.Checked = false;
            if (ObjS3G_EscalationMasterQueryDataTable.Rows[0]["HL_CC2"].ToString() == "True")
                CbxHLcc2.Checked = true;
            else
                CbxHLcc2.Checked = false;

            if (ObjS3G_EscalationMasterQueryDataTable.Rows[0]["Is_Active"].ToString() == "True")
                CbxActive.Checked = true;
            else
                CbxActive.Checked = false;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjUserManageClient.Close();

        }
    }


    #endregion

    /// <summary>
    /// Redirect to view page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../System Admin/S3G_SysAdminEscalationMaster_View.aspx");
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ObjUserManageClient = new UserMgtServicesReference.UserMgtServicesClient();

        try
        {
            string strAlert = "alert('__ALERT__');";
            string strKey = "Insert";
            string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminEscalationMaster_View.aspx';";


            UserMgtServices.S3G_SYSAD_Get_EscalationDetailsRow ObjEscalationMasterRow;
            ObjEscalationMasterRow = ObjS3G_EscalationMasterQueryDataTable.NewS3G_SYSAD_Get_EscalationDetailsRow();
            ObjEscalationMasterRow.Escalation_ID = intEscalationId;

            ObjUserManageClient.FunPubDeleteEscalationDetails(intEscalationId);
            if (intErrCode == 0)
            {
                if (intEscalationId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Escalation details Deleted successfully");
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
        }
        catch (FaultException<DocMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjUserManageClient.Close();
        }

    }
    /// <summary>
    /// Clear the all user enterable date
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlLOB.SelectedIndex = 0;
        ddlRoleCode.SelectedIndex = 0;
        ddlUser.SelectedIndex = 0;
        txtNLDays.Text = "";
        txtHLDays.Text = "";
        CbxNLSms.Checked =CbxNLSms.Enabled= false;
        CbxNLEmail.Checked = CbxNLEmail.Enabled = false;
        CbxNLcc1.Checked = CbxNLcc1.Enabled = false;
        CbxHLSms.Checked = CbxHLSms.Enabled = false;
        CbxHLEmail.Checked = CbxHLEmail.Enabled = false;
        CbxHLcc2.Checked = CbxHLcc2.Enabled = false;
        CbxHLcc1.Checked = CbxHLcc1.Enabled = false;

    }


    /// <summary>
    /// This method used for Role Access 
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                CbxActive.Checked = true;
                CbxActive.Enabled = false;
                btnDelete.Visible = false;
                CbxHLcc1.Enabled = false;
                CbxHLcc2.Enabled = false;
                CbxHLEmail.Enabled = false;
                CbxHLSms.Enabled = false;
                CbxNLcc1.Enabled = false;
                CbxNLEmail.Enabled = false;
                CbxNLSms.Enabled = false; 
                break;

            case 1: // Modify Mode
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                FunGetEscalationDetatils();
                CbxActive.Enabled = true;
                btnClear.Enabled = false;
                rfvLOB.Visible = false;
                rfvRolecode.Visible = false;
                rfvUser.Visible = false;
                ddlLOB.Enabled = false;
                
                ddlRoleCode.Enabled = false;
                ddlUser.Enabled = false;
                btnDelete.Visible = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (string.IsNullOrEmpty(txtHLDays.Text))
                {
                    CbxHLcc1.Enabled = false;
                    CbxHLcc2.Enabled = false;
                    CbxHLEmail.Enabled = false;
                    CbxHLSms.Enabled = false;
                }
                else
                {
                    CbxHLcc1.Enabled = true;
                    CbxHLcc2.Enabled = true;
                    CbxHLEmail.Enabled = true;
                    CbxHLSms.Enabled = true;
                }
                if (string.IsNullOrEmpty(txtNLDays.Text))
                {
                    CbxNLcc1.Enabled = false;
                    CbxNLEmail.Enabled = false;
                    CbxNLSms.Enabled = false;
                }
                else
                {
                    CbxNLcc1.Enabled = true;
                    CbxNLEmail.Enabled = true;
                    CbxNLSms.Enabled = true;
                }
                break;

            case -1:// Query Mode
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                FunGetEscalationDetatils();
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                txtHLDays.ReadOnly = true;
                txtNLDays.ReadOnly = true;
                CbxActive.Enabled = false;
                btnDelete.Visible = false;
                CbxHLcc1.Enabled = false;
                CbxHLcc2.Enabled = false;
                CbxHLEmail.Enabled = false;
                CbxHLSms.Enabled = false;
                CbxNLcc1.Enabled = false;
                CbxNLEmail.Enabled = false;
                CbxNLSms.Enabled = false;
                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    ddlRoleCode.ClearDropDownList();
                    ddlUser.ClearDropDownList();
                }

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                break;
        }




    }

    private bool FunCheckLobisvalid(string strLobId)
    {
        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm.Add("@Company_ID", intCompanyID.ToString());
        Procparm.Add("@User_Id", intUserID.ToString());
        Procparm.Add("@LOB_ID", strLobId);
        Procparm.Add("@Program_ID", "11");
                if (intEscalationId == 0)
        {
            Procparm.Add("@Is_Active", "1");
        }
        Procparm.Add("@Is_UserLobActive", "1");
        DataTable dtBool = Utility.GetDefaultData("S3G_GetUserLobMapping", Procparm);
        if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
            return true;
        else
            return false;

    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

        FunPriLoadRolecode(ddlLOB.SelectedValue);
        AddTooltipForControls();

    }

    private void AddTooltipForControls()
    {
        ddlLOB.AddItemToolTip();
        ddlRoleCode.AddItemToolTip();
        ddlUser.AddItemToolTip();
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        AddTooltipForControls();
    }
}