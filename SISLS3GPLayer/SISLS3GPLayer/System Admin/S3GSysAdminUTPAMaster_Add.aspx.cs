#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   System Admin
/// Screen Name         :   S3GSysAdminUTPAMaster Add/ Modify
/// Created By          :   Suresh P
/// Created Date        :   11-MAY-2010
/// Purpose             :   To Create or Modify UTPA Master details
/// 
/// Last Updated By		:   Gunasekar.K
/// Last Updated Date   :   19-OCT-2010
/// Reason              :   To fix the Bugs - 1776,1773,1769
/// <Program Summary>
#endregion
#region Namespaces
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Configuration;
using System.Data;

#endregion
public partial class System_Admin_S3GSysAdminUTPAMaster_Add : ApplyThemeForProject
{
    #region Initialization
    int intUTPAID = 0;

    int intErrCode = 0;
    int intCompany_ID;
    string strMode = "";
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bClearList = false;
    bool bMakerChecker = false;
    int inthdUserid;
    string strRedirectPageMC;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminUTPAMaster_View.aspx'";

    SerializationMode SerMode;
    UserInfo uin = null;
    Dictionary<string, string> dictLOB;

    //UTPA Master Services
    TPAMgtServicesReference.TPAMgtServicesClient ObjUTPAMasterClient;

    //UTPAMasterTable AND Row
    TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable ObjS3G_UTPADataTable = new TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable();
    TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjUTPAMasterRow;

    //UTPAProgramAccessTable AND Row
    TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable ObjProgramAccessDataTable = new TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable();
    TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow ObjUTPAProgramAccessRow;

    /*Password Policy variable declaration - BCA Enhancement - Kuppu - Sep -17*/
    string strUpperCaseNeed;
    string strNumberNeed;
    string strSpecCharNeed;
    int intPWDLength;
    //string strRedirectPage = "../LoginPage.aspx";
    Boolean boolLoginPage;
    int intGPSPwdItrCount;
    string strNewPassword;
    DataSet dsChagePWD = new DataSet();
    Dictionary<string, string> ProcPWDparam = null;
    int intErrorCode;
    string strPWDDefaultString;

    //Get CompanyMgtServices Methods
    //CompanyMgtServicesReference.CompanyMgtServicesClient ObjCompanyMasterClient;
    //UserMgtServicesReference.UserMgtServicesClient ObjUserManageClient;


    //CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable ObjS3G_RegionCodeDataTable = new CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable();
    //CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable ObjS3G_BranchListDataTable = new CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable();
    //CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable ObjS3G_LOBListDataTable = new CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable();
    //UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable ObjS3G_RoleCodeMasterListDataTable = new UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable();

    #endregion

    #region Page Events


    #region properties
    private int _UTPAID;
    public int UTPAID
    {
        get
        {
            return _UTPAID;
        }
        set
        {
            _UTPAID = value;
        }
    }


    #endregion

    /// <summary>
    /// Page Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        uin = new UserInfo();
        intCompany_ID = uin.ProCompanyIdRW;
        //btnProgramAccess.Enabled = false;
        //string strPassword = txtPassword.Text;
        txtPassword.Attributes.Add("value", txtPassword.Text);
        //Added by saranya
        txtLoginCode.Attributes.CssStyle.Add("text-transform", "uppercase");
        //End
        ChkAdd.Attributes.Add("onclick", "fnSelect('" + ChkAdd.ClientID + "', '" + ChkModify.ClientID + "', '" + ChkQuery.ClientID + "', '" + ChkDelete.ClientID + "', '" + ChkSelectAll.ClientID + "')");
        ChkModify.Attributes.Add("onclick", "fnSelect('" + ChkAdd.ClientID + "', '" + ChkModify.ClientID + "', '" + ChkQuery.ClientID + "', '" + ChkDelete.ClientID + "', '" + ChkSelectAll.ClientID + "')");
        ChkQuery.Attributes.Add("onclick", "fnSelect('" + ChkAdd.ClientID + "', '" + ChkModify.ClientID + "', '" + ChkQuery.ClientID + "', '" + ChkDelete.ClientID + "', '" + ChkSelectAll.ClientID + "')");
        ChkDelete.Attributes.Add("onclick", "fnSelect('" + ChkAdd.ClientID + "', '" + ChkModify.ClientID + "', '" + ChkQuery.ClientID + "', '" + ChkDelete.ClientID + "', '" + ChkSelectAll.ClientID + "')");
        ChkSelectAll.Attributes.Add("onclick", "fnSelectAll('" + ChkAdd.ClientID + "', '" + ChkModify.ClientID + "', '" + ChkQuery.ClientID + "', '" + ChkDelete.ClientID + "', '" + ChkSelectAll.ClientID + "')");

        FunPriTrimTextBoxValues();

        /*Password Policy variable declaration - BCA Enhancement - Kuppu - Sep -17*/
        ProcPWDparam = new Dictionary<string, string>();
        ProcPWDparam.Add("@Company_ID", intCompany_ID.ToString());
        DataSet ds = Utility.GetDataset("S3G_SYSAD_Get_GPS_PWD_Values", ProcPWDparam);

        if (ds.Tables[0].Rows.Count > 0)
        {
            strUpperCaseNeed = ds.Tables[0].Rows[0]["UpperCase_Char"].ToString();
            strNumberNeed = ds.Tables[0].Rows[0]["Numeral_Char"].ToString();
            strSpecCharNeed = ds.Tables[0].Rows[0]["Spec_Char"].ToString();
            intPWDLength = Convert.ToInt32(ds.Tables[0].Rows[0]["Min_pwd_length"].ToString());
        }

        strPWDDefaultString = "New Password should contains atleast ";

        if (strUpperCaseNeed == "True")
        {
            strPWDDefaultString = strPWDDefaultString + "one uppercase alphabet[A-Z];";
        }

        if (strNumberNeed == "True")
        {
            strPWDDefaultString = strPWDDefaultString + " one numeral[0-9];";
        }

        if (strSpecCharNeed == "True")
        {
            strPWDDefaultString = strPWDDefaultString + " one special character[!,@,#,$,..];";
        }

        if (strUpperCaseNeed == "False" && strNumberNeed == "False" && strSpecCharNeed == "False")
        {
            strPWDDefaultString = "Enter a password contains atleast one uppercase alphabet[A-Z]; a numeral[0-9] ; a special character[!,@,#,$,..];";
        }

        if (intPWDLength > 0)
        {
            strPWDDefaultString = strPWDDefaultString + " Length should be minimum " + intPWDLength + " digits.";
        }
        else
        {
            strPWDDefaultString = "Enter a password contains atleast one uppercase alphabet[A-Z]; a numeral[0-9] ; a special character[!,@,#,$,..];";
        }

        lblPWDString.Text = strPWDDefaultString;

        //btnProgramAccess.Enabled = false;
        //btnSave.Enabled = false;


        if (Request.QueryString["qsUTPAID"] != null)
        {

            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsUTPAID"));
            strMode = Request.QueryString.Get("qsMode");
            if (fromTicket != null)
            {
                intUTPAID = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }


        // if (Request.QueryString["qsRegionID"] != null)
        // intRegionID = Convert.ToInt32(Request.QueryString["qsRegionID"]);

        //if (Request.QueryString["qsBranchID"] != null)
        // intBranchID = Convert.ToInt32(Request.QueryString["qsBranchID"]);

        this.UTPAID = intUTPAID;

        TabContainer1.ActiveTabIndex = 0;
        //if (((this.UTPAID > 0)) && (strMode == "M"))
        //{
        //    btnProgramAccess.Enabled = true;
        //    TabContainer1.ActiveTabIndex = 1;
        //    FunPriDisableControls(1);
        //}
        UserInfo ObjUserInfo = new UserInfo();
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        if (!IsPostBack)
        {
            chkActive.Checked = false;
            //btnClearProgramAccess.Enabled = false;

            ddlBranchCode.FillDataTable(null, null, null);
            //FunPriLoadRegion_LIST();
            FunPriLoadBranch_LIST();
            FunPriLoadLOB_LIST();
            FunPriLoadRoleCode_LIST();
            //FunPriLoadGLCode_LIST();
            FunPriLoadUTPAType_LIST();

            chkActive.Checked = false;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if (((intUTPAID > 0)) && (strMode == "M"))
            {
                FunPriDisableControls(1);
                //btnProgramAccess.Enabled = true;
            }
            else if (((intUTPAID > 0)) && (strMode == "Q"))
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriDisableControls(0);
            }
        }
        S3GSession ObjS3GSession = new S3GSession();
        if (ObjS3GSession.ProPINCodeDigitsRW > 0)
        {
            txtPincode.MaxLength = ObjS3GSession.ProPINCodeDigitsRW;
            if (ObjS3GSession.ProPINCodeTypeRW.ToUpper() == "ALPHA NUMERIC")
            {
                ftePIN.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
            }
            else
            {
                ftePIN.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                //ftePIN.FilterType = AjaxControlToolkit.FilterTypes.Custom;
            }
        }

        if (((intUTPAID  > 0)) && (strMode  == "Q"))
        {
            txtPassword.Attributes.Add("value", "*************");
        }

        if (strMode == "M" || strMode == "Q")
        {
            txtPassword.Attributes.Add("value", "*************");
        }
    }

    /// <summary>
    /// Enable or disable the validation controls
    /// </summary>
    /// <param name="blnFlag"></param>
    private void FunPriValidationDefault(bool blnFlag)
    {
        rfvRegionCode.Enabled = blnFlag;
        //rfvBranch.Enabled = blnFlag;
        //rfvUTPAType.Enabled = blnFlag;
        //rfvUTPACode.Enabled = blnFlag;
        rfvUTPAName.Enabled = blnFlag;
        rfvAddress1.Enabled = blnFlag;
        rfvCity.Enabled = blnFlag;
        rfvState.Enabled = blnFlag;
        rfvCountry.Enabled = blnFlag;
        rfvPincode.Enabled = blnFlag;
        rfvGLCode.Enabled = blnFlag;
        //tdUTPACODE.Visible = blnFlag;
        lblRegion.Attributes.Add("class", "");
        lblBranch.Attributes.Add("class", "");
        lblUTPAType.Attributes.Add("class", "");
        lblUTPACode.Attributes.Add("class", "");
        lblUTPAName.Attributes.Add("class", "");
        lblAddress1.Attributes.Add("class", "");
        lblCity.Attributes.Add("class", "");
        lblState.Attributes.Add("class", "");
        lblCountry.Attributes.Add("class", "");
        lblPincode.Attributes.Add("class", "");
        lblGLCode.Attributes.Add("class", "");
        if (blnFlag)
        {
            lblRegion.Attributes.Add("class", "styleReqFieldLabel");
            lblBranch.Attributes.Add("class", "styleReqFieldLabel");
            lblUTPAType.Attributes.Add("class", "styleReqFieldLabel");
            lblUTPACode.Attributes.Add("class", "styleReqFieldLabel");
            lblUTPAName.Attributes.Add("class", "styleReqFieldLabel");
            lblAddress1.Attributes.Add("class", "styleReqFieldLabel");
            lblCity.Attributes.Add("class", "styleReqFieldLabel");
            lblState.Attributes.Add("class", "styleReqFieldLabel");
            lblCountry.Attributes.Add("class", "styleReqFieldLabel");
            lblPincode.Attributes.Add("class", "styleReqFieldLabel");
            lblGLCode.Attributes.Add("class", "styleReqFieldLabel");
        }
    }

    /// <summary>
    /// Remove Leading and Trailing Spaces
    /// </summary>
    private void FunPriTrimTextBoxValues()
    {
        try
        {
            txtUTPAName.Attributes.Add("onblur", "Trim('" + txtUTPAName.ClientID + "');");

            txtAddress1.Attributes.Add("onblur", "Trim('" + txtAddress1.ClientID + "');");
            txtAddress2.Attributes.Add("onblur", "Trim('" + txtAddress2.ClientID + "');");

            txtCity.Attributes.Add("onblur", "Trim('" + txtCity.ClientID + "');");
            txtState.Attributes.Add("onblur", "Trim('" + txtState.ClientID + "');");
            txtCountry.Attributes.Add("onblur", "Trim('" + txtCountry.ClientID + "');");
            txtPincode.Attributes.Add("onblur", "Trim('" + txtPincode.ClientID + "');");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// Load Branch List based on the Selected Region
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RegionCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadBranch_LIST();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Save_Click(object sender, EventArgs e)
    {

        ObjUTPAMasterClient = new TPAMgtServicesReference.TPAMgtServicesClient();

        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminUTPAMaster_View.aspx';";
        string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminUTPAMaster_Add.aspx__qs__';";

        lblErrorMessage.Text = "";

        //if (txtUTPACode.Text.Trim().Length < 4)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Enter a valid UTPA Code');", true);
        //    return;
        //}
        //if (txtUTPACode.Text.Trim().HasSpecialCharacter(false, false))
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('PEnter a valid UTPA Code');", true);
        //    return;
        //}

        ////if (!txtPassword.Text.IsValidPassword())
        ////{
        ////    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Enter a valid password');", true);
        ////    return;
        ////}
        //if (txtPincode.Text.Length < txtPincode.MaxLength)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Enter a valid PIN code');", true);
        //    return;

        //}

        #region Password Policy
        if (chkResetPWD.Checked && txtPassword.Text.Trim() != "")
        {
            if (txtPassword.Text.Length < intPWDLength)
            {
                Utility.FunShowAlertMsg(this.Page, "Password length should be minimum " + intPWDLength + " digits");
                return;
            }

            if (strUpperCaseNeed == "True" && !pwdHasCapitals(txtPassword.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this.Page, "Password should contain atleast one uppercase alphabet[A-Z]");
                return;
            }

            if (strNumberNeed == "True" && !pwdHasNumbers(txtPassword.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this.Page, "Password should contain atleast one numeral[0-9]");
                return;
            }

            if (strSpecCharNeed == "True" && !pwdHasSpecChar(txtPassword.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this.Page, "Password should contain atleast one special character[!,@,#,$,..]");
                return;
            }


            strNewPassword = ClsPubCommCrypto.EncryptText(txtPassword.Text.Trim());

            if (ProcPWDparam != null)
            {
                ProcPWDparam.Clear();
            }
            ProcPWDparam.Add("@UTPA_ID", intUTPAID.ToString());
            ProcPWDparam.Add("@Password", strNewPassword);
            ProcPWDparam.Add("@Company_ID", intCompany_ID.ToString());

            dsChagePWD = Utility.GetDataset("S3G_SYSAD_UTPA_Upd_ChangePassword", ProcPWDparam);

            if (dsChagePWD.Tables[1].Rows.Count > 0)
            {
                intGPSPwdItrCount = Convert.ToInt32(dsChagePWD.Tables[1].Rows[0]["PWD_Itr_Count"].ToString());
            }

            if (dsChagePWD.Tables[0].Rows.Count > 0)
            {
                intErrorCode = Convert.ToInt32(dsChagePWD.Tables[0].Rows[0]["ErrorCode"].ToString());
                if (intErrorCode == 1)
                {
                    Utility.FunShowAlertMsg(this.Page, "You cannot enter previous " + intGPSPwdItrCount + " passwords.");
                    txtPassword.Enabled = true;
                    txtPassword.Text = "";
                    txtPassword.Text = string.Empty;
                    return;
                }
            }
        }

        #endregion Password Policy

        if (txtUTPAName.Text.Substring(0, 1).Equals(" "))
        {
            txtUTPAName.Text = txtUTPAName.Text.Trim();
        }

        if (txtCity.Text.Substring(0, 1).Equals(" "))
        {
            txtCity.Text = txtCity.Text.Trim();
        }

        if (txtState.Text.Substring(0, 1).Equals(" "))
        {
            txtState.Text = txtCity.Text.Trim();
        }


        int intInsertUTPAID;

        try
        {
            uin = new UserInfo();


            //TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjUTPAMasterRow;
            ObjUTPAMasterRow = ObjS3G_UTPADataTable.NewS3G_SYSAD_UTPAMasterRow();
            ObjUTPAMasterRow.UTPA_ID = intUTPAID;
            ObjUTPAMasterRow.Company_ID = intCompany_ID;
            ObjUTPAMasterRow.Region_ID = 0;// Convert.ToInt32(ddlRegionCode.SelectedValue);
            ObjUTPAMasterRow.Branch_ID = Convert.ToInt32(ddlBranchCode.SelectedValue);
            ObjUTPAMasterRow.UTPA_Type_ID = Convert.ToInt32(ddlUTPAType.SelectedValue);
            //ObjUTPAMasterRow.UTPA_Code = txtUTPACode.Text;
            ObjUTPAMasterRow.UTPA_Code = ddlUTPACode.SelectedItem.ToString();
            ObjUTPAMasterRow.Entity_ID = Convert.ToInt32(ddlUTPACode.SelectedValue);
            ObjUTPAMasterRow.UTPA_Name = txtUTPAName.Text;
            ObjUTPAMasterRow.UTPA_Login_Code = txtLoginCode.Text;
            ObjUTPAMasterRow.Address1 = txtAddress1.Text;
            ObjUTPAMasterRow.Address2 = txtAddress2.Text;
            ObjUTPAMasterRow.City = txtCity.Text;
            ObjUTPAMasterRow.State = txtState.Text;
            ObjUTPAMasterRow.GL_Code = txtGLCODE.Text;
            //ObjUTPAMasterRow.GL_Code = ddlGLCode.SelectedValue;
            if (intUTPAID > 0 && !chkResetPWD.Checked)
            {
                ObjUTPAMasterRow.Password = "0";
            }
            else
            {
                ObjUTPAMasterRow.Password = ClsPubCommCrypto.EncryptText(txtPassword.Text.Trim());
            }
            ObjUTPAMasterRow.Country = txtCountry.Text;
            ObjUTPAMasterRow.Pincode = txtPincode.Text;
            ObjUTPAMasterRow.Is_Active = chkActive.Checked;
            ObjUTPAMasterRow.Created_By = uin.ProUserIdRW;
            ObjUTPAMasterRow.Modified_By = uin.ProUserIdRW;
            ObjS3G_UTPADataTable.AddS3G_SYSAD_UTPAMasterRow(ObjUTPAMasterRow);


            SerMode = SerializationMode.Binary;

            if (intUTPAID > 0)
            {
                intErrCode = ObjUTPAMasterClient.FunPubModifyUTPA(SerMode, ClsPubSerialize.Serialize(ObjS3G_UTPADataTable, SerMode));
            }
            else if (intUTPAID == 0)
            {
                intErrCode = ObjUTPAMasterClient.FunPubCreateUTPA(out intInsertUTPAID, SerMode, ClsPubSerialize.Serialize(ObjS3G_UTPADataTable, SerMode));
                this.UTPAID = intInsertUTPAID;
            }

            if (intErrCode == 0)
            {
                if (intUTPAID > 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "UTPA details updated successfully");
                }
                else
                {
                    if (this.UTPAID > 0)
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here
                        string strAlert1 = "UTPA details added successfully";
                        //strAlert = strAlert.Replace("__ALERT__", strAlert1);

                        strAlert1 += @"\n\nSet Program Access for this UTPA!";
                        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(this.UTPAID.ToString(), false, 0);
                        strRedirectPageView = strRedirectPageAdd.Replace("__qs__", "?qsUTPAID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                        TabContainer1.ActiveTabIndex = 1;
                        strAlert = strAlert.Replace("__ALERT__", strAlert1);
                    }
                }
            }
            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "UTPA Code already exists in the system. Kindly enter a new UTPA Code");
                strRedirectPageView = "";
            }
            //ObjUTPAMasterClient.Close();

            strAlert = strAlert.Replace("__ALERT__", "");
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
            ObjUTPAMasterClient.Close();
        }
    }

    #region Methods to check GPS Password Policy values

    private bool pwdHasCapitals(string text)
    {
        foreach (char c in text)
        {
            if (char.IsUpper(c))
                return true;
        }
        return false;
    }

    private bool pwdHasNumbers(string text)
    {
        foreach (char c in text)
        {
            if (char.IsNumber(c))
                return true;
        }
        return false;
    }

    private bool pwdHasSpecChar(string text)
    {
        foreach (char c in text)
        {
            if (!char.IsLetterOrDigit(c))
                return true;
        }
        return false;
    }

    # endregion Methods to check GPS values


    /// <summary>
    /// Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminUTPAMaster_View.aspx");
    }


    protected void BtnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriClearControls()
    {
        try
        {
            ddlBranchCode.SelectedIndex = 0;
            //ddlGLCode.SelectedIndex = 0;
            txtGLCODE.Text = "";
            ddlLOB.SelectedIndex = 0;
            ddlUTPACode.SelectedIndex = 0;
            //ddlRegionCode.SelectedIndex = 0;
            ddlRoleCode.SelectedIndex = 0;
            ddlUTPAType.SelectedIndex = 0;
            txtAddress1.Text = txtAddress2.Text = txtCity.Text = txtCountry.Text =
            txtPassword.Text = txtPincode.Text = txtState.Text = txtUTPAName.Text = txtLoginCode.Text = string.Empty;
            txtPassword.Attributes.Add("value", "");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    /// <summary>
    /// Save the Program Access details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ProgramAccessSave_Click(object sender, EventArgs e)
    {
        ObjUTPAMasterClient = new TPAMgtServicesReference.TPAMgtServicesClient();


        if (Convert.ToInt32(ddlLOB.SelectedValue) == 0)
        {
            //lblErrorMessage_Program.Text = "Please select a Line of Business in the list";
            //lblErrorMessage_Program.Attributes.Add("style", "color:red");
            TabContainer1.ActiveTabIndex = 1;
            return;
        }

        if (Convert.ToInt32(ddlRoleCode.SelectedValue) == 0)
        {
            //lblErrorMessage_Program.Text = "Please select a RoleCode in the list";
            //lblErrorMessage_Program.Attributes.Add("style", "color:red");
            TabContainer1.ActiveTabIndex = 1;
            return;
        }
        if (!ChkAdd.Checked && !ChkModify.Checked && !ChkDelete.Checked && !ChkQuery.Checked)
        {
            lblErrorMessage_Program.Text = "Select atleast one access rights";
            TabContainer1.ActiveTabIndex = 1;
            return;
        }
        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminUTPAMaster_View.aspx';";
        string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminUTPAMaster_Add.aspx__qs__';";

        try
        {
            ObjUTPAProgramAccessRow = ObjProgramAccessDataTable.NewS3G_SYSAD_UTPAProgramAccessRow();
            ObjUTPAProgramAccessRow.UTPA_ID = intUTPAID;
            ObjUTPAProgramAccessRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjUTPAProgramAccessRow.Role_Code_ID = Convert.ToInt32(ddlRoleCode.SelectedValue);
            ObjUTPAProgramAccessRow.Add_Access = ChkAdd.Checked;
            ObjUTPAProgramAccessRow.Modify_Access = ChkModify.Checked;
            ObjUTPAProgramAccessRow.Delete_Access = ChkDelete.Checked;
            ObjUTPAProgramAccessRow.Query = ChkQuery.Checked;
            ObjUTPAProgramAccessRow.Is_Active = true;
            ObjProgramAccessDataTable.AddS3G_SYSAD_UTPAProgramAccessRow(ObjUTPAProgramAccessRow);

            SerMode = SerializationMode.Binary;


            int intErrCodeProgramAccess = 0;
            if (ddlLOB.Enabled == true && ddlRoleCode.Enabled == true)
            {
                intErrCodeProgramAccess = ObjUTPAMasterClient.FunPubCreateUTPAProgramAccess(SerMode, ClsPubSerialize.Serialize(ObjProgramAccessDataTable, SerMode));
            }
            else if (ddlLOB.Enabled == false && ddlRoleCode.Enabled == false)
            {
                intErrCodeProgramAccess = ObjUTPAMasterClient.FunPubModifyUTPAProgramAccess(SerMode, ClsPubSerialize.Serialize(ObjProgramAccessDataTable, SerMode));
            }

            if (intErrCodeProgramAccess == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "Program Access details added successfully");
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(intUTPAID.ToString(), false, 0);
                strRedirectPageView = strRedirectPageAdd.Replace("__qs__", "?qsUTPAID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                TabContainer1.ActiveTabIndex = 1;
            }
            else if (intErrCodeProgramAccess == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Selected Combination already exists for this UTPA code.");
                strRedirectPageView = "";
            }
            //ObjUTPAMasterClient.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
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
            ObjUTPAMasterClient.Close();
        }

    }

    /// <summary>
    /// Delete Program Access Grid Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProgramAccess_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ObjUTPAMasterClient = new TPAMgtServicesReference.TPAMgtServicesClient();

        try
        {
            string strAlert = "alert('__ALERT__');";
            //--Added by guna on 19th-Oct-2010 to address the bug 1773
            string strRedirectPageAdd;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(intUTPAID.ToString(), false, 0);
            //--Ends Here

            int intDeleteID = Convert.ToInt32(gvProgramAccess.DataKeys[e.RowIndex]["UTPA_Prog_Access_ID"]);

            int intErrCodeProgramAccess = 0;

            intErrCodeProgramAccess = ObjUTPAMasterClient.FunPubDeleteUTPAProgramAccess(intDeleteID);



            if (intErrCodeProgramAccess > 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "Unable to Delete the Program Access details");
            }
            else
            {
                //--Added by guna on 19th-Oct-2010 to address the bug 1773
                ////Response.Redirect(strRedirectPageAdd + "?qsDNCId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                strRedirectPageAdd = "../System Admin/S3GSysAdminUTPAMaster_Add.aspx?qsUTPAID=";
                Response.Redirect(strRedirectPageAdd + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                //--Ends Here

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strAlert, true);
            // ObjUTPAMasterClient.Close();

        }
        catch (FaultException<TPAMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjUTPAMasterClient.Close();
        }

    }

    /// <summary>
    /// Modify Program Access Grid Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProgramAccess_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {

            int intUTPAProgramAccessID = Convert.ToInt32(gvProgramAccess.DataKeys[e.NewEditIndex]["UTPA_Prog_Access_ID"]);
            //ObjUTPAProgramAccessRow = ObjProgramAccessDataTable.NewS3G_SYSAD_UTPAProgramAccessRow();
            //ObjUTPAProgramAccessRow.UTPA_ID = intUTPAID;
            //ObjUTPAProgramAccessRow.UTPA_Prog_Access_ID = intUTPAProgramAccessID;
            //ObjProgramAccessDataTable.AddS3G_SYSAD_UTPAProgramAccessRow(ObjUTPAProgramAccessRow);

            //ObjUTPAMasterClient = new TPAMgtServicesReference.TPAMgtServicesClient();

            //SerMode = SerializationMode.Binary;
            //byte[] byteUTPAProgramAccessDetails = ObjUTPAMasterClient.FunPubQueryUTPAProgramAccess(SerMode, ClsPubSerialize.Serialize(ObjProgramAccessDataTable, SerMode));
            //ObjProgramAccessDataTable = new TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable();
            Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
            dictProcParam.Add("@UTPA_ID", intUTPAID.ToString());
            dictProcParam.Add("@UTPA_Prog_Access_ID", intUTPAProgramAccessID.ToString());

            DataTable dtProgramAccess = Utility.GetDefaultData("S3G_Get_UTPAProgramAccess_Details", dictProcParam);
            //dt.ImportRow(ObjProgramAccessDataTable.Rows[0]);
            // dt = (TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable)ClsPubSerialize.DeSerialize(byteUTPAProgramAccessDetails, SerMode, typeof(DataTable));

            DataRow ObjUTPAProgramRow = dtProgramAccess.Rows[0] as DataRow;

            //ddlLOB.SelectedValue = ObjUTPAProgramRow["LOB_ID"].ToString();
            Boolean blob;
            Boolean blobranch;
            // blob = FunCheckLobisvalid(ddlLOB.SelectedValue);
            blob = FunCheckLobisvalid(ObjUTPAProgramRow["LOB_ID"].ToString());
            blobranch = FunCheckBranchisvalid(ddlBranchCode.SelectedValue);
            if (blob == false)
            {
                strAlert = strAlert.Replace("__ALERT__", "Selected Line of Business rights not assigned");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                return;
            }

            if (blobranch == false)
            {
                strAlert = strAlert.Replace("__ALERT__", "Selected Location rights not assigned");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                return;
            }


            //--Added by guna on 19th-Oct-2010 to address the Bug - 1769
            FunPriLoadRoleCodeForUpdate_LIST();
            //--Ends Here
            ddlLOB.SelectedValue = ObjUTPAProgramRow["LOB_ID"].ToString();
            ddlRoleCode.SelectedValue = ObjUTPAProgramRow["Role_Code_ID"].ToString();
            ChkAdd.Checked = Convert.ToBoolean(ObjUTPAProgramRow["Add_Access"]);
            ChkModify.Checked = Convert.ToBoolean(ObjUTPAProgramRow["Modify_Access"]);
            ChkQuery.Checked = Convert.ToBoolean(ObjUTPAProgramRow["Query"]);
            ChkDelete.Checked = Convert.ToBoolean(ObjUTPAProgramRow["Delete_Access"]);
            ChkSelectAll.Checked = (ChkAdd.Checked && ChkModify.Checked && ChkQuery.Checked && ChkDelete.Checked);
            ObjProgramAccessDataTable = null;
            //ObjUTPAMasterClient.Close();

            ddlLOB.Enabled = false;
            ddlRoleCode.Enabled = false;
            btnClearProgramAccess.Enabled = true;
            TabContainer1.ActiveTabIndex = 1;
        }
        catch (FaultException<TPAMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// Clear the Selected Program Access Grid Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ProgramAccessClear_Click(object sender, EventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(intUTPAID.ToString(), false, 0);
        ddlLOB.Enabled = true;
        ddlRoleCode.Enabled = true;
        btnClearProgramAccess.Enabled = false;
        Response.Redirect("../System Admin/S3GSysAdminUTPAMaster_Add.aspx?qsUTPAID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=" + Request.QueryString.Get("qsMode"));
    }
    #endregion

    #region User Defined Methods
    #region Dropdown
    /// <summary>
    /// Load Region List
    /// </summary>
    private void FunPriLoadRegion_LIST()
    {
        try
        {
            uin = new UserInfo();

            dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());

            if (intUTPAID == 0)
            {
                dictLOB.Add("@Is_Active", "1");
                dictLOB.Add("@User_Id", uin.ProUserIdRW.ToString());
            }
            //--Added by Guna on 19th-Oct-2010 to fix the Bug 1776
            //ddlRegionCode.BindDataTable("S3G_Get_Region_CodeForUTPA", dictLOB, new string[] { "Region_Id", "Region" });
            ////ddlRegionCode.BindDataTable("S3G_Get_Region_Code", dictLOB, new string[] { "Region_Id", "Region" });
            //--Ends Here


            /*
            CompanyMgtServices.S3G_SYSAD_RegionCodeRow ObjRegionMasterRow;
            ObjRegionMasterRow = ObjS3G_RegionCodeDataTable.NewS3G_SYSAD_RegionCodeRow();
            ObjRegionMasterRow.Company_ID = intCompany_ID;
            ObjS3G_RegionCodeDataTable.AddS3G_SYSAD_RegionCodeRow(ObjRegionMasterRow);

            ObjCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

            SerMode = SerializationMode.Binary;
            byte[] bytesRegionListDetails = ObjCompanyMasterClient.FunPubRegionCode_LIST(SerMode, ClsPubSerialize.Serialize(ObjS3G_RegionCodeDataTable, SerMode));
            ObjS3G_RegionCodeDataTable = (CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable)ClsPubSerialize.DeSerialize(bytesRegionListDetails, SerMode, typeof(CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable));

            ddlRegionCode.FillDataTable(ObjS3G_RegionCodeDataTable, ObjS3G_RegionCodeDataTable.Region_IDColumn.ColumnName, ObjS3G_RegionCodeDataTable.RegionColumn.ColumnName);
            ObjCompanyMasterClient.Close();
             */
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    /// <summary>
    /// Load Branch 
    /// </summary>
    /// <param name="intRegionID">Region</param>
    //private void FunPriLoadBranch_LIST(int intRegionID)
    private void FunPriLoadBranch_LIST()
    {
        try
        {
            uin = new UserInfo();

            dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());
            //  dictLOB.Add("@Region_id", intRegionID.ToString());
            dictLOB.Add("@Program_ID", "8");
            if (intUTPAID == 0)
            {
                dictLOB.Add("@Is_Active", "1");
                dictLOB.Add("@User_Id", uin.ProUserIdRW.ToString());
            }
            ddlBranchCode.BindDataTable("S3G_Get_Branch_List", dictLOB, new string[] { "Location_ID", "Location" });



            /*
            CompanyMgtServices.S3G_SYSAD_Branch_ListRow ObjUTPAMasterRow;
            ObjUTPAMasterRow = ObjS3G_BranchListDataTable.NewS3G_SYSAD_Branch_ListRow();
            ObjUTPAMasterRow.Region_ID = intRegionID;
            ObjS3G_BranchListDataTable.AddS3G_SYSAD_Branch_ListRow(ObjUTPAMasterRow);

            ObjCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

            SerMode = SerializationMode.Binary;
            byte[] bytesLOBListDetails = ObjCompanyMasterClient.FunPubBranch_List(SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchListDataTable, SerMode));
            ObjS3G_BranchListDataTable = (CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable)ClsPubSerialize.DeSerialize(bytesLOBListDetails, SerMode, typeof(CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable));

            ddlBranchCode.FillDataTable(ObjS3G_BranchListDataTable, ObjS3G_BranchListDataTable.Branch_IDColumn.ColumnName, ObjS3G_BranchListDataTable.BranchColumn.ColumnName);
            ObjCompanyMasterClient.Close();
             */
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlBranchCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadLOB_LIST();
    }
    /// <summary>
    /// Load LOB LIST
    /// </summary>
    private void FunPriLoadLOB_LIST()
    {
        try
        {
            uin = new UserInfo();

            dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());
            //if (intUTPAID == 0)
            if (strMode != "Q")
            {
                dictLOB.Add("@Is_Active", "1");

            }
            dictLOB.Add("@User_ID", uin.ProUserIdRW.ToString());
            if (ddlBranchCode.SelectedIndex != 0)
                dictLOB.Add("@Location_ID", ddlBranchCode.SelectedValue);
            dictLOB.Add("@Program_ID", "8");

            ddlLOB.BindDataTable("S3G_Get_LOBList_UTPA", dictLOB, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });


            /*
            CompanyMgtServices.S3G_SYSAD_LOBMasterRow ObjLOBMasterRow1;
            ObjLOBMasterRow1 = ObjS3G_LOBListDataTable.NewS3G_SYSAD_LOBMasterRow();
            ObjLOBMasterRow1.Company_ID = intCompany_ID;
            ObjLOBMasterRow1.Is_Active = true;
            ObjS3G_LOBListDataTable.AddS3G_SYSAD_LOBMasterRow(ObjLOBMasterRow1);

            ObjCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

            SerMode = SerializationMode.Binary;
            byte[] bytesLOBListDetails = ObjCompanyMasterClient.FunPubQueryLOB_LIST(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBListDataTable, SerMode));
            ObjS3G_LOBListDataTable = (CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable)ClsPubSerialize.DeSerialize(bytesLOBListDetails, SerMode, typeof(CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable));

            ddlLOB.FillDataTable(ObjS3G_LOBListDataTable, ObjS3G_LOBListDataTable.LOB_IDColumn.ColumnName, ObjS3G_LOBListDataTable.LOBCode_LOBNameColumn.ColumnName);
            ObjCompanyMasterClient.Close();
            */
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    /// <summary>
    /// Load all GL Account Code
    /// </summary>
    //private void FunPriLoadGLCode_LIST()
    //{
    //    uin = new UserInfo();
    //    Dictionary<string, string> dictLOB = new Dictionary<string, string>();
    //    dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());
    //    dictLOB.Add("@Is_Active", "1");
    //    ddlGLCode.BindDataTable("S3G_Get_GLCode_LIST", dictLOB, new string[] { "GL_CODE", "GL_CODE" });
    //}

    /// <summary>
    /// Load RoleCode List
    /// </summary>
    private void FunPriLoadRoleCode_LIST()
    {
        try
        {
            uin = new UserInfo();
            Dictionary<string, string> dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());
            //--Added by guna to address the bug 1769
            ////if (intUTPAID == 0)
            ////{
            ////    dictLOB.Add("@Is_Active", "1");
            ////}
            dictLOB.Add("@Is_Active", "1");


            //-- Ends here

            ddlRoleCode.BindDataTable("S3G_Get_RoleCode_List", dictLOB, new string[] { "Role_Code_ID", "Role_Code" });


            /*UserMgtServices.S3G_SYSAD_RoleCode_ListRow ObjRoleCodeMasterRow;
            ObjRoleCodeMasterRow = ObjS3G_RoleCodeMasterListDataTable.NewS3G_SYSAD_RoleCode_ListRow();
            ObjRoleCodeMasterRow.Company_ID = intCompany_ID;
            ObjS3G_RoleCodeMasterListDataTable.AddS3G_SYSAD_RoleCode_ListRow(ObjRoleCodeMasterRow);

            ObjUserManageClient = new UserMgtServicesReference.UserMgtServicesClient();

            SerMode = SerializationMode.Binary;
            byte[] bytesRoleCodeListDetails = ObjUserManageClient.FunPubRoleCodeList(SerMode, ClsPubSerialize.Serialize(ObjS3G_RoleCodeMasterListDataTable, SerMode));
            ObjS3G_RoleCodeMasterListDataTable = (UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable)ClsPubSerialize.DeSerialize(bytesRoleCodeListDetails, SerMode, typeof(UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable));

            ddlRoleCode.FillDataTable(ObjS3G_RoleCodeMasterListDataTable, ObjS3G_RoleCodeMasterListDataTable.Role_Code_IDColumn.ColumnName, ObjS3G_RoleCodeMasterListDataTable.Role_CodeColumn.ColumnName);
            ObjUserManageClient.Close();
             */
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }


    }


    //--Added by guna on 19th-Oct-2010 to address the Bug - 1769
    private void FunPriLoadRoleCodeForUpdate_LIST()
    {
        try
        {
            ddlRoleCode.Items.Clear();
            uin = new UserInfo();
            Dictionary<string, string> dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());
            ddlRoleCode.BindDataTable("S3G_Get_RoleCode_List", dictLOB, new string[] { "Role_Code_ID", "Role_Code" });
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }
    //--Ends Here

    /// <summary>
    /// Load All UTPA Type LiSt
    /// </summary>
    private void FunPriLoadUTPAType_LIST()
    {
        try
        {
            dictLOB = new Dictionary<string, string>();
            ddlUTPAType.BindDataTable("S3G_Get_UTPA_TypeMaster", dictLOB, new string[] { "Entity_Type_ID", "Entity_Type_Name" });
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    /// <summary>
    /// Get UTPA Details and Program Access Details
    /// </summary>
    private void FunPriGetUTPADetails()
    {
        ObjUTPAMasterClient = new TPAMgtServicesReference.TPAMgtServicesClient();

        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompany_ID.ToString());
            Procparam.Add("@UTPA_ID", intUTPAID.ToString());
            DataTable dtUTPA = Utility.GetDefaultData("S3G_Get_UTPA_Details", Procparam);

            if (dtUTPA != null && dtUTPA.Rows.Count > 0)
            {

                lblErrorMessage.Text = "";
                //ObjS3G_UTPADataTable = new TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable();
                //ObjUTPAMasterRow = ObjS3G_UTPADataTable.NewS3G_SYSAD_UTPAMasterRow();
                //ObjUTPAMasterRow.Company_ID = intCompany_ID;
                //ObjUTPAMasterRow.UTPA_ID = intUTPAID;
                //ObjS3G_UTPADataTable.AddS3G_SYSAD_UTPAMasterRow(ObjUTPAMasterRow);

                //SerMode = SerializationMode.Binary;
                //byte[] byteLOBDetails = ObjUTPAMasterClient.FunPubQueryUTPA(SerMode, ClsPubSerialize.Serialize(ObjS3G_UTPADataTable, SerMode));
                //ObjS3G_UTPADataTable = (TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, SerMode, typeof(TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable));

                /*--------------------------------------------*/
                /*----------------UTPA Details----------------*/
                /*--------------------------------------------*/

                //ObjUTPAMasterRow = ObjS3G_UTPADataTable.Rows[0] as TPAMgtServices.S3G_SYSAD_UTPAMasterRow;
                ////ddlRegionCode.SelectedValue = ObjUTPAMasterRow.Region_ID.ToString();
                ////FunPriLoadBranch_LIST(Convert.ToInt32(ObjUTPAMasterRow.Region_ID));
                //FunPriLoadBranch_LIST();
                //ddlBranchCode.SelectedValue = ObjUTPAMasterRow.Branch_ID.ToString();
                //FunPriLoadLOB_LIST();
                //ddlUTPAType.SelectedValue = ObjUTPAMasterRow.UTPA_Type_ID.ToString();
                ////txtUTPACode.Text = ObjUTPAMasterRow.UTPA_Code.ToString();
                ////txtUTPAName.Text = ObjUTPAMasterRow.UTPA_Name.ToString();
                //FunPriLoadUTPA_List(Convert.ToInt32(ddlUTPAType.SelectedValue));
                //ddlUTPACode.SelectedValue = ObjUTPAMasterRow.Entity_ID.ToString();
                //txtUTPAName.Text = ObjUTPAMasterRow.UTPA_Name.ToString();
                //txtLoginCode.Text = ObjUTPAMasterRow.UTPA_Login_Code.ToString();
                //txtPassword.Attributes.Add("value", ClsPubCommCrypto.DecryptText(ObjUTPAMasterRow.Password.ToString()));
                //txtAddress1.Text = ObjUTPAMasterRow.Address1.ToString();
                //txtAddress2.Text = ObjUTPAMasterRow.Address2.ToString();
                //txtCity.Text = ObjUTPAMasterRow.City.ToString();
                //txtState.Text = ObjUTPAMasterRow.State.ToString();
                //txtCountry.Text = ObjUTPAMasterRow.Country.ToString();
                //txtPincode.Text = ObjUTPAMasterRow.Pincode.ToString();
                ////ddlGLCode.SelectedValue = ObjUTPAMasterRow.GL_Code.ToString();
                //txtGLCODE.Text = ObjUTPAMasterRow.GL_Code.ToString();
                //chkActive.Checked = ObjUTPAMasterRow.Is_Active;
                //hdnID.Value = ObjUTPAMasterRow.Created_By.ToString();

                //Commented and Added by Thangam M on 16/Apr/2012
                DataRow DRow = dtUTPA.Rows[0];
                FunPriLoadBranch_LIST();
                ddlBranchCode.SelectedValue = DRow["Branch_ID"].ToString();
                FunPriLoadLOB_LIST();
                ddlUTPAType.SelectedValue = DRow["UTPA_Type_ID"].ToString();
                FunPriLoadUTPA_List(Convert.ToInt32(ddlUTPAType.SelectedValue));
                ddlUTPACode.SelectedValue = DRow["Entity_ID"].ToString();
                txtUTPAName.Text = DRow["UTPA_Name"].ToString();
                txtLoginCode.Text = DRow["UTPA_Login_Code"].ToString();
                txtPassword.Attributes.Add("value", ClsPubCommCrypto.DecryptText(DRow["Password"].ToString()));
                txtAddress1.Text = DRow["Address1"].ToString();
                txtAddress2.Text = DRow["Address2"].ToString();
                txtCity.Text = DRow["City"].ToString();
                txtState.Text = DRow["State"].ToString();
                txtCountry.Text = DRow["Country"].ToString();
                txtPincode.Text = DRow["Pincode"].ToString();
                txtGLCODE.Text = DRow["GL_Code"].ToString();
                if (DRow["Is_Active"].ToString() == "1" || DRow["Is_Active"].ToString() == "True")
                    chkActive.Checked = true;
                else
                    chkActive.Checked = false;
                hdnID.Value = DRow["Created_By"].ToString();
                //End here

                ddlRegionCode.Enabled = false;
                ddlBranchCode.Enabled = false;
                ddlUTPAType.Enabled = false;
                //txtUTPACode.Enabled = false;
                //txtUTPAName.Enabled = false;
                //txtAddress1.Enabled = false;
                //txtAddress2.Enabled = false;
                //txtCity.Enabled = false;
                //txtState.Enabled = false;
                //txtCountry.Enabled = false;
                //txtPincode.Enabled = false;


                /*-----------------------------------------------------------*/
                /*----------------UTPA Program Access Details----------------*/
                /*-----------------------------------------------------------*/
                //ObjUTPAProgramAccessRow = ObjProgramAccessDataTable.NewS3G_SYSAD_UTPAProgramAccessRow();
                //ObjUTPAProgramAccessRow.UTPA_ID = intUTPAID;
                //ObjProgramAccessDataTable.AddS3G_SYSAD_UTPAProgramAccessRow(ObjUTPAProgramAccessRow);

                //ObjUTPAMasterClient = new TPAMgtServicesReference.TPAMgtServicesClient();

                //SerMode = SerializationMode.Binary;
                //byte[] byteUTPAProgramAccessDetails = ObjUTPAMasterClient.FunPubQueryUTPAProgramAccess(SerMode, ClsPubSerialize.Serialize(ObjProgramAccessDataTable, SerMode));
                //ObjProgramAccessDataTable = new TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable();
                //ObjProgramAccessDataTable = (TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable)ClsPubSerialize.DeSerialize(byteUTPAProgramAccessDetails, SerMode, typeof(TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable));

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@UTPA_ID", intUTPAID.ToString());
                DataTable dtProgramAccess = Utility.GetDefaultData("S3G_Get_UTPAProgramAccess_Details", Procparam);
                gvProgramAccess.DataSource = dtProgramAccess;
                gvProgramAccess.DataBind();
                //ObjProgramAccessDataTable = null;
                //ObjUTPAMasterClient.Close();
            }
        }
        catch (FaultException<TPAMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {

            //ObjUTPAMasterClient.Close();
        }

    }

    /// <summary>
    /// Gets Name of the LineofBusiness.
    /// </summary>
    /// <param name="strLOB_ID"></param>
    /// <returns></returns>
    public string GetLineofBusinessName(string strLOB_ID)
    {
        return ddlLOB.Items.FindByValue(strLOB_ID).Text;
    }
    #endregion

    #region ValueDisable
    /// <summary>
    /// Disable Controls based on the User Roles
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
                chkActive.Checked = true;
                //Changed By Thangam M on 19/Mar/2012 to fix bug id - 6064
                FunPriValidationDefault(false);
                BtnClear.Enabled = true;
                //TabContainer1.Tabs[1].Enabled = false;
                btnProgramAccess.Enabled = false;
                chkActive.Enabled = false;
                TabContainer1.ActiveTabIndex = 0;
                break;

            case 1: // Modify Mode
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                FunPriGetUTPADetails();
                FunPriValidationDefault(false);
                TabContainer1.ActiveTabIndex = 1;
                BtnClear.Enabled = false;
                //TabContainer1.Tabs[1].Enabled = true;
                chkActive.Enabled = true;
                ddlUTPACode.Enabled = false;
                txtLoginCode.ReadOnly = true;
                btnProgramAccess.Enabled = true;
                txtPassword.Attributes.Add("value", "*************");
                txtPassword.Enabled = false;
                break;

            case -1:// Query Mode
                FunPriGetUTPADetails();
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                BtnClear.Enabled = false;
                btnSave.Enabled = false;
                btnProgramAccess.Enabled = false;
                gvProgramAccess.Columns[8].Visible = false;
                gvProgramAccess.Columns[9].Visible = false;
                ChkAdd.Enabled = false;
                ChkDelete.Enabled = false;
                ChkModify.Enabled = false;
                ChkQuery.Enabled = false;
                ChkSelectAll.Enabled = false;
                ddlLOB.Enabled = false;
                ddlRoleCode.Enabled = false;
                chkActive.Enabled = false;
                TabContainer1.ActiveTabIndex = 0;
                //txtUTPACode.ReadOnly = true;
                ddlUTPACode.ClearDropDownList();
                txtUTPAName.ReadOnly = true;
                txtLoginCode.ReadOnly = true;
                txtPassword.ReadOnly = true;
                chkResetPWD.Enabled = false;
                txtAddress1.ReadOnly = true;
                txtAddress2.ReadOnly = true;
                txtCity.ReadOnly = true;
                txtState.ReadOnly = true;
                txtCountry.ReadOnly = true;
                txtPincode.ReadOnly = true;
                ddlRegionCode.Enabled = true;
                ddlBranchCode.Enabled = true;
                ddlUTPAType.Enabled = true;
                txtUTPAName.Enabled = true;
                txtAddress1.Enabled = true;
                txtAddress2.Enabled = true;
                txtCity.Enabled = true;
                txtState.Enabled = true;
                txtCountry.Enabled = true;
                txtPincode.Enabled = true;
                btnClearProgramAccess.Enabled = false;
                /*Commented for bug fixing - GL code not avail in Query mode*/
                /*Bug_ID- 5573 - Kuppusamy.B - 21-Feb-2012*/
                //txtGLCODE.Text = "";
                txtGLCODE.Enabled = true;

                if (bClearList)
                {
                    //ddlRegionCode.ClearDropDownList();
                    ddlBranchCode.ClearDropDownList();
                    ddlUTPAType.ClearDropDownList();


                }

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                break;
        }

    }
    #endregion

    protected void chkResetPWD_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkResetPWD.Checked)
            {
                txtPassword.Enabled = true;
                txtPassword.Text = string.Empty;
                txtPassword.Text = "";
                txtPassword.Clear();
            }
            else
            {
                txtPassword.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void gvProgramAccess_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    //Label lblRegionActive = (Label)e.Row.FindControl("lblRegionActive");
        //    //CheckBox chkAct = (CheckBox)e.Row.FindControl("CbxRegionActive");
        //    //if (lblRegionActive.Text == "True")
        //    //{
        //    //    chkAct.Checked = true;
        //    //}
        //    if (!bModify)
        //    {
        //        gvProgramAccess.Columns[8].Visible = false;
        //        gvProgramAccess.Columns[9].Visible = false;
        //    }
        //} 


    }

    private bool FunCheckLobisvalid(string strLobId)
    {
        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm.Add("@Company_ID", intCompany_ID.ToString());
        Procparm.Add("@User_Id", uin.ProUserIdRW.ToString());
        Procparm.Add("@LOB_ID", strLobId);
        if (ddlBranchCode.SelectedIndex != 0)
            Procparm.Add("@Location_ID", ddlBranchCode.SelectedValue);
        Procparm.Add("@Program_ID", "8");
        if (intUTPAID == 0)
        {
            Procparm.Add("@Is_Active", "1");
        }
        Procparm.Add("@Is_UserLobActive", "1");
        //DataTable dtBool = Utility.GetDefaultData("S3G_GetUserLobMapping", Procparm);[S3G_GetUserLobMappingUTPA]
        DataTable dtBool = Utility.GetDefaultData("S3G_GetUserLobMappingUTPA", Procparm);
        if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
            return true;
        else
            return false;

    }

    private bool FunCheckBranchisvalid(string strBranchId)
    {
        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm.Add("@Company_ID", intCompany_ID.ToString());
        Procparm.Add("@User_Id", uin.ProUserIdRW.ToString());
        Procparm.Add("@Location_ID", strBranchId);
        Procparm.Add("@Program_ID", "8");
        if (intUTPAID == 0)
        {
            Procparm.Add("@Is_Active", "1");
        }

        Procparm.Add("@Is_UserLobActive", "1");
        DataTable dtBool = Utility.GetDefaultData("S3G_GetUserBranchMapping", Procparm);
        if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
            return true;
        else
            return false;

    }

    protected void ddlUTPAType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClearControls();
        if (Convert.ToInt32(ddlUTPAType.SelectedValue) > 0)
        {
            FunPriLoadUTPA_List(Convert.ToInt32(ddlUTPAType.SelectedValue));
        }

        if (Convert.ToInt32(ddlUTPAType.SelectedValue) == 0)
        {
            ddlUTPACode.Items.Clear();
        }

    }

    private void FunPriLoadUTPA_List(int intType)
    {
        try
        {
            if (intType == 1)               // Customer
            {
                dictLOB = new Dictionary<string, string>();
                dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());
                dictLOB.Add("@UTPA_Type", "1");
                ddlUTPACode.BindDataTable("S3G_Get_UTPA_CustomerMaster", dictLOB, new string[] { "Customer_ID", "Customer_Code" });
            }
            else                            //Entity
            {
                dictLOB = new Dictionary<string, string>();
                dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());
                dictLOB.Add("@UTPA_Type", "0");
                dictLOB.Add("@UTPA_TypeCode", ddlUTPAType.SelectedValue.ToString());
                ddlUTPACode.BindDataTable("S3G_Get_UTPA_CustomerMaster", dictLOB, new string[] { "Entity_ID", "Entity_Code" });
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }


    private void FunPriLoadName_List()
    {
        try
        {
            Dictionary<string, string> dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", uin.ProCompanyIdRW.ToString());
            dictLOB.Add("@UTPA_ID", ddlUTPACode.SelectedValue.ToString());
            if (ddlUTPAType.SelectedValue != "1")
            {
                dictLOB.Add("@UTPA_Type", "0");
                dictLOB.Add("@UTPA_TypeCode", ddlUTPAType.SelectedValue.ToString());
            }
            else
            {
                dictLOB.Add("@UTPA_Type", "1");
            }

            DataTable dtCustDetails = Utility.GetDefaultData("S3G_Get_UTPA_CustomerMaster", dictLOB);

            if (dtCustDetails.Rows.Count > 0)
            {
                DataRow dtRow = dtCustDetails.Rows[0];
                txtUTPAName.Text = dtRow["Name"].ToString();
                txtAddress1.Text = dtRow["Address1"].ToString();
                txtAddress2.Text = dtRow["Address2"].ToString();
                txtCity.Text = dtRow["City"].ToString();
                txtState.Text = dtRow["State"].ToString();
                txtCountry.Text = dtRow["Country"].ToString();
                txtPincode.Text = dtRow["Pincode"].ToString();
                txtGLCODE.Text = dtRow["GL_Code"].ToString();
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlUTPACode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClearControls();
        if (Convert.ToInt32(ddlUTPACode.SelectedValue) > 0)
        {
            FunPriLoadName_List();
        }
        if (Convert.ToInt32(ddlUTPAType.SelectedValue) == 0)
        {
            ddlUTPACode.Items.Clear();
        }
    }

    protected void FunClearControls()
    {
        txtPassword.Text =
            txtUTPAName.Text =
            txtLoginCode.Text =
            txtAddress1.Text =
            txtAddress2.Text =
            txtCity.Text =
            txtState.Text =
            txtCountry.Text =
            txtPincode.Text =
            txtPassword.Text =
            txtGLCODE.Text = "";

        //code to clear Password textbox on ddlUTPACode_SelectedIndexChanged
        txtPassword.Attributes.Add("value", string.Empty);

    }
}

