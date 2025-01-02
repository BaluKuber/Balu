/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminUserManagement_Add.aspx
/// Created By      :   Kaliraj K
/// Created Date    :   13-May-2010
/// Purpose         :   To insert and update user details
/// Modified By      :   Kaliraj K
/// Created Date    :   13-May-2010
/// Purpose         :   To insert and update user details

using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Text;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class S3GSysAdminUserManagement_Add : ApplyThemeForProject
{

    #region Intialization
    UserMgtServicesReference.UserMgtServicesClient objUserManagementClient = new UserMgtServicesReference.UserMgtServicesClient();
    int intErrCode = 0;
    int intUserId = -1;
    int intCreatedBy = 0;
    int inthdUserid;
    string strRedirectPageMC;
    string strDateFormat = string.Empty;
    UserMgtServices.S3G_SYSAD_UserManagementDataTable ObjS3G_SYSAD_UserManagementDataTable = new UserMgtServices.S3G_SYSAD_UserManagementDataTable();
    UserMgtServices.S3G_SYSAD_UserMaster_ListDataTable ObjS3G_SYSAD_UserMasterDataTable = new UserMgtServices.S3G_SYSAD_UserMaster_ListDataTable();
    SerializationMode SerMode = SerializationMode.Binary;

    public static S3GSysAdminUserManagement_Add obj_Page;

    string strRedirectPage = "../System Admin/S3GSysAdminUserManagement_View.aspx";
    string strRegionDet = "<Root><Details Region_ID='0' /></Root>";
    DataSet dsBranchRole = new DataSet();
    DataTable dtBranch = null;
    int intCompanyID = 0;
    string strModeCode = string.Empty;
    Dictionary<string, string> Procparam = null;
    StringBuilder strbRegion = new StringBuilder();
    StringBuilder strbSelLocation = new StringBuilder();
    StringBuilder strbSelRoleCode = new StringBuilder();
    int intLOBID = 0;
    bool booChecked = true;
    UserInfo ObjUserInfo = null;
    string strMode = "C";
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bClearList = false;
    bool bMakerChecker = false;
    bool bChecked = true;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminUserManagement_Add.aspx';";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminUserManagement_View.aspx';";
    static string strPageName = "User Management";

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

    #endregion

    #region PageLoad

    /// <summary>
    /// Loadingf region and lob details on page load 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            obj_Page = this;
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intCreatedBy = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender1.Format = strDateFormat;

            //intCompanyID = Convert.ToInt32(Session["Company_ID"]);
            //intCreatedBy = Convert.ToInt32(Session["userid"]);

            /*Password Policy variable declaration - BCA Enhancement - Kuppu - Sep -17*/
            ProcPWDparam = new Dictionary<string, string>();
            ProcPWDparam.Add("@Company_ID", intCompanyID.ToString());
            DataSet ds = Utility.GetDataset("S3G_SYSAD_Get_GPS_PWD_Values", ProcPWDparam);

            if (ds.Tables[0].Rows.Count > 0)
            {
                strUpperCaseNeed = ds.Tables[0].Rows[0]["UpperCase_Char"].ToString();
                strNumberNeed = ds.Tables[0].Rows[0]["Numeral_Char"].ToString();
                strSpecCharNeed = ds.Tables[0].Rows[0]["Spec_Char"].ToString();
                intPWDLength = Convert.ToInt32(ds.Tables[0].Rows[0]["Min_pwd_length"].ToString());
            }

            strPWDDefaultString = "New Password should contains atleast ";

            if (strUpperCaseNeed == "True" || strUpperCaseNeed == "1")
            {
                strPWDDefaultString = strPWDDefaultString + "one uppercase alphabet[A-Z];";
            }

            if (strNumberNeed == "True" || strNumberNeed == "1")
            {
                strPWDDefaultString = strPWDDefaultString + " one numeral[0-9];";
            }

            if (strSpecCharNeed == "True" || strSpecCharNeed == "1")
            {
                strPWDDefaultString = strPWDDefaultString + " one special character[!,@,#,$,..];";
            }

            if (strUpperCaseNeed == "False" || strUpperCaseNeed == "0" && strNumberNeed == "False" || strNumberNeed == "0" && strSpecCharNeed == "False" || strSpecCharNeed == "0")
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

            StringBuilder strRegion = new StringBuilder();
            txtPassword.Attributes.Add("value", txtPassword.Text);
            txtUserCode.Attributes.CssStyle.Add("text-transform", "uppercase");

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            if (Request.QueryString["qsUserId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsUserId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intUserId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }

            if (!IsPostBack)
            {
                trLOBMapping.Visible = false;

                //btnSaveBranch.Attributes.Add("onclick", "if(!fnIsCheckboxChecked('" + grvRegionDet.ClientID + "','chkSelRegion','Region'))  ");
                //btnSaveBranch.Attributes.Add("onclick", "alert(document.getElementById('hdnRegionVal').value); if(document.getElementById('hdnRegionVal').value=='0') return fnIsCheckboxChecked('" + grvBranchDet.ClientID + "','chkSelBranch','Branch');");
                //txtDateofJoining.Attributes.Add("readonly", "readonly");
                txtDateofJoining.Attributes.Add("onblur", "fnDoDate(this,'" + txtDateofJoining.ClientID + "','" + strDateFormat + "',true,  false);");
                FunBindUserCode();
                FunPriloadDesignation();
                FunPriloadUserGroup();
                FunPriloadLOB();
                FunPriloadLocationTreeview();
                FunGetUserDetails();
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (((intUserId > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if (((intUserId > 0)) && (strMode == "Q"))
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);

                }
                //treeview.CollapseAll();
            }
            if ((ddlLevel.SelectedValue == "4") || ((ddlLevel.SelectedValue == "5")))
            {
                ddlReportingLevel.IsMandatory = false;
                ddlHigherLevel.IsMandatory = false;
                //rfvReportingLevel.Enabled = false;
                //rfvHigherLevel.Enabled = false;
            }
            else
            {
                //rfvReportingLevel.Enabled = false;
                //rfvHigherLevel.Enabled = false;
                ddlHigherLevel.IsMandatory = false;
                ddlReportingLevel.IsMandatory = false;
            }

            if (((intUserId > 0)) && (strMode == "Q"))
            {
                txtPassword.Attributes.Add("value", "*************");
            }

            treeview.Attributes.Add("onclick", "postBackByObject()");
            if (strMode == "M" || strMode == "Q")
                txtPassword.Attributes.Add("value", "*************");

            //        treeview.Attributes["onclick"] = ClientScript..GetPostBackCl
            //(this.treeview, "Select$" + e.ToString());

        }
        catch (Exception ex)
        {
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            //lblErrorMessage.Text = ex.Message;
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
    }



    #endregion

    #region Page Events


    /// <summary>
    /// This is used to clear data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            fnPubClear();
            foreach (TreeNode treeviewnode in treeview.Nodes)
                FunNodeClearCheck(treeviewnode); //to clear Treeview

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }

        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }

    }

    private void fnPubClear()
    {
        try
        {
            txtUserCode.Text =
            txtUserName.Text =
            txtMobileNo.Text =
            txtEMailId.Text = string.Empty;
            txtPassword.Attributes.Add("value", "");
            txtDateofJoining.Text = DateTime.Now.ToString(strDateFormat);
            ddlLevel.SelectedIndex = 0;
            //Bug Fixing by Palani Kumar.A on 09/01/2014
            if (txtDesignation.SelectedValue != "0")
            {
                txtDesignation.SelectedValue = "0"; ;
            }
            if (ddlLevel.SelectedValue != "-1")
            {
                ddlLevel.SelectedValue = "-1";
            }

            //ddlReportingLevel.SelectedIndex = 0;
            //ddlHigherLevel.SelectedIndex = 0;
            ddlReportingLevel.Clear();
            ddlHigherLevel.Clear();

            ddlLOB.SelectedIndex = -1;
            chkCopyProfile.Checked = false;
            //ddlCopyProfile.SelectedIndex = 0;
            ddlCopyProfile.Clear();
            chkActive.Checked = true;
            //FunBindUserCode();
            FunGetUserDetails();
            //grvBranchDet.DataSource = null;
            //grvBranchDet.DataBind();

            grvRoleCode.DataSource = null;
            grvRoleCode.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected void treeview_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (strMode == "Q")
            {
                treeview.ExpandAll();
                treeview.Enabled = false; //Bug Fixing on 09/01/2013
            }
            else
            {
                treeview.Enabled = true;
                treeview.CollapseAll();
                FunPriCheckLocation();
            }
        }
        else
        {
            treeview.ExpandAll();
        }
    }

    private void FunPriCheckLocation()
    {
        foreach (TreeNode treeviewnode in treeview.Nodes)
        {
            if (ViewState["Locationss"] != null)
                FunNodeCheckMod(treeviewnode, (DataTable)ViewState["Locationss"]); //to check locations in Treeview
        }
    }


    private void FunNodeloop(TreeNode e)
    {
        try
        {
            for (int i = 0; i < e.ChildNodes.Count; i++)
            {
                //FunNodeGetValue(e.ChildNodes[i]);
                //if (e.ChildNodes[i].Checked)
                //{
                FunPubAssignValue(e.ChildNodes[i].Value.Split('-')[0].ToString(), e.ChildNodes[i].Value.Split('-')[1].ToString());
                if (e.ChildNodes[i].ChildNodes.Count == 0)
                    FunPubAssignValue(e.ChildNodes[i]);
                //}
                if (e.ChildNodes[i].ChildNodes.Count > 0)
                    FunNodeloop(e.ChildNodes[i]);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    /// <summary>
    /// This method is used to clear all checked items in a treeview.
    /// </summary>
    /// <param name="e"></param>
    private void FunNodeGetValue(TreeNode tn)
    {
        try
        {
            int levels = Convert.ToInt32(tn.Value.Split('-')[0].ToString());
            string str1 = tn.Parent.Value;
            string str2 = tn.Parent.Parent.Value;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    public void FunPubAssignValue(string Level, string value)
    {
        if (ViewState["DtMappingLevel"] == null || ((DataTable)ViewState["DtMappingLevel"]).Columns.Count == 0)
        { FunPubCreateTable(); }
        DataTable dt = ((DataTable)ViewState["DtMappingLevel"]);
        DataRow drow = dt.NewRow();
        drow["M" + Level] = value;
        dt.Rows.Add(drow);
        ViewState["DtMappingLevel"] = dt;
    }

    Dictionary<string, string> dictValue = new Dictionary<string, string>();
    public void FunPubAssignValue(TreeNode tn)
    {
        if (ViewState["DtMappingLevel"] == null || ((DataTable)ViewState["DtMappingLevel"]).Columns.Count == 0)
        { FunPubCreateTable(); }
        DataTable dt = ((DataTable)ViewState["DtMappingLevel"]);
        string strCurrentValue = string.Empty;
        int intLevels = Convert.ToInt32(tn.Value.Split('-')[0].ToString());
        string strnewValue = string.Empty;
        for (int i = 0; i < intLevels; i++)
        {
            if (i == 0 && tn.Checked)
            {
                //dictValue.Add(tn.Value, tn.Value);
                strCurrentValue = tn.Value;
            }
            else if (i == 1 && tn.Parent.Checked)
            {
                //dictValue.Add(tn.Parent.Value, tn.Parent.Value);
                strCurrentValue = tn.Parent.Value + "|" + strCurrentValue;
            }
            else if (i == 2 && tn.Parent.Parent.Checked)
            {
                //dictValue.Add(tn.Parent.Parent.Value, tn.Parent.Parent.Value);
                strCurrentValue = tn.Parent.Parent.Value + "|" + strCurrentValue;
            }
            else if (i == 3 && tn.Parent.Parent.Parent.Checked)
            {
                //dictValue.Add(tn.Parent.Parent.Parent.Value, tn.Parent.Parent.Parent.Value);
                strCurrentValue = tn.Parent.Parent.Parent.Value + "|" + strCurrentValue;
            }
            else if (i == 4 && tn.Parent.Parent.Parent.Parent.Checked)
            {
                //dictValue.Add(tn.Parent.Parent.Parent.Parent.Value, tn.Parent.Parent.Parent.Parent.Value);
                strCurrentValue = tn.Parent.Parent.Parent.Parent.Value + "|" + strCurrentValue;
            }
        }

        if (!dictValue.ContainsKey(strCurrentValue))
            dictValue.Add(strCurrentValue, strCurrentValue);

        ViewState["DtMappingLevel"] = dt;
    }

    //public void FunPubAssignValue(string strLevel, string strvalue, string strLMode, string? strFstValue)
    //{
    //    if (ViewState["DtMappingLevel"] == null || ((DataTable)ViewState["DtMappingLevel"]).Columns.Count == 0)
    //    { FunPubCreateTable(); }
    //    DataTable dt = ((DataTable)ViewState["DtMappingLevel"]);
    //    DataRow[] dr;
    //    if (strLMode == "New")
    //    {
    //        DataRow drow = dt.NewRow();
    //        drow["M" + Level] = value;
    //        dt.Rows.Add(drow);
    //    }
    //    else
    //    {
    //        dr = dt.Select("M1='" + strFstValue + "'");
    //        dr[0]["M" + strLevel] = strvalue;
    //        dt.AcceptChanges();
    //    }
    //    ViewState["DtMappingLevel"] = dt;
    //}

    public void FunPubCreateTable()
    {
        DataTable dt = new DataTable();
        for (int i = 1; Convert.ToInt32(ViewState["TVMAxLevel"].ToString()) >= i; i++)
        {
            dt.Columns.Add("M" + i);
        }
        ViewState["DtMappingLevel"] = dt;
    }

    /// <summary>
    /// This is used to save user (role access,Branch and lob mapping) details 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            /*Code commented to  implement password policy - kuppu - sep 17 - BCA Enhancement*/
            //if ((chkResetPwd.Checked) && (!Utility.IsValidPassword(txtPassword.Text)))
            //{
            //    txtPassword.Enabled = true;
            //    Utility.FunShowAlertMsg(this.Page, "Enter valid password");
            //    return;
            //}
            if (chkResetPwd.Checked)
            {
                txtPassword.Enabled = true;
            }

            #region Password Policy
            if (chkResetPwd.Checked && txtPassword.Text.Trim() != "")
            {
                if (txtPassword.Text.Length < intPWDLength)
                {
                    Utility.FunShowAlertMsg(this.Page, "New Password length should be minimum " + intPWDLength + " digits");
                    return;
                }

                if (strUpperCaseNeed == "True" && !pwdHasCapitals(txtPassword.Text.Trim()))
                {
                    Utility.FunShowAlertMsg(this.Page, "New Password should contain atleast one uppercase alphabet[A-Z]");
                    return;
                }

                if (strNumberNeed == "True" && !pwdHasNumbers(txtPassword.Text.Trim()))
                {
                    Utility.FunShowAlertMsg(this.Page, "New Password should contain atleast one numeral[0-9]");
                    return;
                }

                if (strSpecCharNeed == "True" && !pwdHasSpecChar(txtPassword.Text.Trim()))
                {
                    Utility.FunShowAlertMsg(this.Page, "New Password should contain atleast one special character[!,@,#,$,..]");
                    return;
                }

                strNewPassword = ClsPubCommCrypto.EncryptText(txtPassword.Text.Trim());

                if (ProcPWDparam != null)
                {
                    ProcPWDparam.Clear();
                }
                ProcPWDparam.Add("@User_ID", intUserId.ToString());
                ProcPWDparam.Add("@Password", strNewPassword);
                ProcPWDparam.Add("@Company_ID", intCompanyID.ToString());

                dsChagePWD = Utility.GetDataset("S3G_SYSAD_Update_ChangePassword", ProcPWDparam);

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
                        chkResetPwd.Checked = false;
                        txtPassword.Text = "";
                        return;
                    }
                }
            }
            #endregion Password Policy

            if (!string.IsNullOrEmpty(ddlReportingLevel.SelectedValue) && !string.IsNullOrEmpty(ddlReportingLevel.SelectedValue))
            {
                if (ddlReportingLevel.SelectedValue != "0" && ddlHigherLevel.SelectedValue != "0" && ddlReportingLevel.SelectedValue == ddlHigherLevel.SelectedValue)
                {
                    Utility.FunShowAlertMsg(this.Page, "Reporting level and Higher level cannot be the same user");//Error Code 502
                    return;
                }
            }
            if (ddldeletion.Visible && ddldeletion.SelectedIndex > 0)
            {
                if (ddldeletion.SelectedValue == "1")
                {
                    bool istreechecked = false;
                    foreach (TreeNode node in treeview.CheckedNodes)
                    {
                        istreechecked = true;
                    }
                    if (!istreechecked)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Select atleast one Location");
                        return;
                    }
                }
            }

            if (ddlModification.Visible && ddlModification.SelectedIndex > 0)
            {
                if (ddlModification.SelectedValue == "2")
                {
                    bool istreecheckedG = false;
                    bool istreecheckedR = false;
                    foreach (TreeNode node in treeview.CheckedNodes)
                    {
                        istreecheckedG = true;
                    }
                    if (tbBranchAccess.Enabled && treeview.Enabled && !istreecheckedG)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Select atleast one Location");
                        return;
                    }
                    //For roles
                    foreach (GridViewRow gvnode in grvRoleCode.Rows)
                    {
                        CheckBox chkSelRoleCode = (CheckBox)gvnode.FindControl("chkSelRoleCode");
                        if (chkSelRoleCode.Checked)
                            istreecheckedR = true;
                    }
                    if (tbBranchAccess.Enabled && grvRoleCode.Enabled && !istreecheckedR)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Select atleast one Role Code");
                        return;
                    }
                }
            }

            if (ddlUserGroup.SelectedItem.Text.Length > 20)
            {
                Utility.FunShowAlertMsg(this.Page, "User Group Length Should be 20");
                return;
            }

            UserMgtServices.S3G_SYSAD_UserManagementRow ObjUserManagementRow;

            ObjUserManagementRow = ObjS3G_SYSAD_UserManagementDataTable.NewS3G_SYSAD_UserManagementRow();
            ObjUserManagementRow.Company_ID = intCompanyID;
            ObjUserManagementRow.User_Code = txtUserCode.Text.Trim();
            ObjUserManagementRow.User_Name = txtUserName.Text.Trim();

            if ((intUserId > 0) && (!chkResetPwd.Checked))
            {
                ObjUserManagementRow.Password = "0";
            }
            else
            {
                ObjUserManagementRow.Password = ClsPubCommCrypto.EncryptText(txtPassword.Text.Trim());
            }

            ObjUserManagementRow.Email_ID = txtEMailId.Text.Trim();
            ObjUserManagementRow.DOJ = Utility.StringToDate(txtDateofJoining.Text.Trim());
            ObjUserManagementRow.Designation = txtDesignation.SelectedValue.Trim();
            ObjUserManagementRow.Mobile_No = txtMobileNo.Text.Trim();
            ObjUserManagementRow.CreatedBy = intCreatedBy;
            ObjUserManagementRow.Is_Active = chkActive.Checked;
            ObjUserManagementRow.User_Level_ID = Convert.ToInt32(ddlLevel.SelectedValue.Trim());

            if (!string.IsNullOrEmpty(ddlReportingLevel.SelectedValue))
                ObjUserManagementRow.Reporting_Next_level = Convert.ToInt32(ddlReportingLevel.SelectedValue);
            else
                ObjUserManagementRow.Reporting_Next_level = 0;
            if (!string.IsNullOrEmpty(ddlHigherLevel.SelectedValue))
                ObjUserManagementRow.Reporting_Higher_level = Convert.ToInt32(ddlHigherLevel.SelectedValue);
            else
                ObjUserManagementRow.Reporting_Higher_level = 0;
            ObjUserManagementRow.Is_Active = chkActive.Checked;
            ObjUserManagementRow.User_ID = intUserId;

            if (!string.IsNullOrEmpty(ddlCopyProfile.SelectedValue))
                ObjUserManagementRow.CopyProfile_User_ID = Convert.ToInt32(ddlCopyProfile.SelectedValue);
            else
                ObjUserManagementRow.CopyProfile_User_ID = 0;


            ObjUserManagementRow.Mode = 1;

            int n;
            bool isNumeric = int.TryParse(ddlUserGroup.SelectedValue, out n);
            if (isNumeric == true)
            {
                if (Convert.ToInt32(ddlUserGroup.SelectedValue) > 0)
                    ObjUserManagementRow.User_Group = Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            else
            {
                ObjUserManagementRow.User_Group = 0;
                ObjUserManagementRow.User_Group_Name = ddlUserGroup.SelectedItem.Text;
            }

            FunGetBranchRoleSelectedDet();
            ObjUserManagementRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue.Trim());
            ObjUserManagementRow.XMLLocationDet = strbSelLocation.ToString().Trim();
            ObjUserManagementRow.XMLRoleCodeDet = strbSelRoleCode.ToString().Trim();
            if (ddlAction.SelectedIndex > 0)
            {
                if (ddlModification.SelectedIndex > 0)//Access Modification value-2
                    ObjUserManagementRow.DeletionLobLocation = Convert.ToInt16(ddlModification.SelectedValue.Trim());
                else if (ddldeletion.SelectedIndex > 0)//Deletion value-0,1
                    ObjUserManagementRow.DeletionLobLocation = Convert.ToInt16(ddldeletion.SelectedValue.Trim());
                else
                    ObjUserManagementRow.DeletionLobLocation = -1;
            }
            else
            {
                ObjUserManagementRow.DeletionLobLocation = -1;
            }

            if (ddlCopyLOb.SelectedIndex > 0)
                ObjUserManagementRow.Copy_Lob_profile = Convert.ToInt32(ddlCopyLOb.SelectedValue.Trim());
            else
                ObjUserManagementRow.Copy_Lob_profile = 0;
            ObjS3G_SYSAD_UserManagementDataTable.AddS3G_SYSAD_UserManagementRow(ObjUserManagementRow);

            intErrCode = objUserManagementClient.FunPubCreateUser(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_UserManagementDataTable, SerMode));
            switch (intErrCode)
            {
                case 0:

                    if (intUserId > 0)
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here
                        string strFileName = Server.MapPath(@"~\Data\UserManagement\" + "_" + Session.SessionID.ToString() + ".xml");
                        if (File.Exists(strFileName))
                        {
                            File.Delete(strFileName);
                        }

                        Utility.FunShowAlertMsg(this.Page, "User details updated successfully", strRedirectPage);
                    }
                    else
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here
                        strAlert = "User details added successfully";
                        strAlert += @"\n\nWould you like to add one more user?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                    }
                    break;
                case 1:
                    Utility.FunShowAlertMsg(this.Page, "User Code already exist");
                    break;
                case 2:
                    Utility.FunShowAlertMsg(this.Page, "User Name already exist");
                    break;
                case 3:
                    Utility.FunShowAlertMsg(this.Page, "EMail ID already exist");
                    break;
                //case 4:
                //    Utility.FunShowAlertMsg(this.Page, 
                //        "Date of Joining should be greater than or equal to the Date of Incorporation/Company Start date");
                //    break;
                case 5:
                    Utility.FunShowAlertMsg(this.Page, "Mobile number already exists");
                    break;
                ////case 6:
                ////    Utility.FunShowAlertMsg(this.Page, "Previous password and new password cannot be same");

                ////    break;
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + "window.location.href='" + strRedirectPage + "';", true);
            lblErrorMessage.Text = string.Empty;


        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;

        }
        finally
        {
            if (objUserManagementClient != null)
                objUserManagementClient.Close();
        }
    }

    /// <summary>
    /// This is used to redirect page 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage, false);
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
    }

    /// <summary>
    /// This is used to tick selected checkbox when records bind in grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    //protected void grvLOB_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (intUserId > 0)
    //        {
    //            if (e.Row.RowType == DataControlRowType.DataRow)
    //            {
    //                Label lblAssigned = (Label)e.Row.FindControl("lblAssigned");
    //                CheckBox chkSelLOB = (CheckBox)e.Row.FindControl("chkSelLOB");
    //                if (lblAssigned.Text == "1")
    //                {
    //                    chkSelLOB.Checked = true;
    //                    hdnLOBPresent.Value = "1";
    //                }
    //                else
    //                    chkSelLOB.Enabled = false;
    //                if (!bModify && strMode == "Q")
    //                {
    //                    chkSelLOB.Enabled = false;
    //                }
    //                if ((strMode == "Q") && (booChecked))
    //                {
    //                    chkSelLOB.Checked = true;
    //                    booChecked = false;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        CVUsermanagement.ErrorMessage = ex.Message;
    //        CVUsermanagement.IsValid = false;
    //    }


    //}

    /// <summary>
    /// This is used to tick selected checkbox when records bind in grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void grvRoleCode_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAssigned = (Label)e.Row.FindControl("lblAssigned");
                CheckBox chkSelRoleCode = (CheckBox)e.Row.FindControl("chkSelRoleCode");

                if (lblAssigned.Text == "1")
                {
                    chkSelRoleCode.Checked = true;
                }
                if (!bModify && strMode == "Q")
                {
                    chkSelRoleCode.Enabled = false;

                }
                chkSelRoleCode.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvRoleCode.ClientID + "','chkAll','chkSelRoleCode');");
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAllRole");
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvRoleCode.ClientID + "',this,'chkSelRoleCode');");
                //chkAll.Checked = true;
            }

        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }



    }

    /// <summary>
    /// /// This is used to tick selected checkbox when records bind in grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void grvRegion_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        Label lblAssigned = (Label)e.Row.FindControl("lblAssigned");
    //        CheckBox chkSelRegion = (CheckBox)e.Row.FindControl("chkSelRegion");
    //        //CheckBox chkSelRegion = (CheckBox)e.Row.FindControl("chkAll");
    //        CheckBox chkall = grvRegionDet.HeaderRow.FindControl("chkAll") as CheckBox;

    //        if (lblAssigned.Text == "1")
    //        {
    //            chkSelRegion.Checked = true;
    //        }
    //        if (!bModify && strMode == "Q")
    //        {
    //            chkSelRegion.Enabled = false;
    //            chkall.Enabled = false;
    //        }

    //    }


    //}

    /// <summary>
    /// This is used to tick selected checkbox when records bind in grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    //protected void grvBranchDet_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        Label lblAssigned = (Label)e.Row.FindControl("lblAssigned");
    //        CheckBox chkSelBranch = (CheckBox)e.Row.FindControl("chkSelBranch");
    //        HtmlInputCheckBox chkall = grvBranchDet.HeaderRow.FindControl("chkAll") as HtmlInputCheckBox;
    //        //CheckBox chkall = grvBranchDet.HeaderRow.FindControl("chkAll") as CheckBox;
    //        if (lblAssigned.Text == "1")
    //        {
    //            chkSelBranch.Checked = true;
    //        }
    //        if (!bModify && strMode == "Q")
    //        {
    //            chkSelBranch.Enabled = false;
    //            //  chkall.Attributes.Add("readonly", "readonly");
    //        }
    //        //fnGridUnSelect('<%=grvBranchDet.ClientID%>',this,'<%=chkAll.ClientID%>
    //        chkSelBranch.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvBranchDet.ClientID + "','chkAll','chkSelBranch');");
    //    }
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        //CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAllRole");
    //        CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
    //        chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvBranchDet.ClientID + "',this,'chkSelBranch');");
    //        //chkAll.Checked = true;
    //    }
    //}

    protected void btnRemoveLOBMapping_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOBMapping.Items.Count == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Cannot be deleted.Atleast there should be one Line of Business");
                return;
            }

            objUserManagementClient.FunPubUpdateLOBMapping(intUserId, Convert.ToInt32(ddlLOBMapping.SelectedValue));
            //FunPriBindUserLOBMapping();
            FunGetUserDetails();
            Utility.FunShowAlertMsg(this.Page, "Line of Business mapping deleted successfully");
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
        finally
        {
            if (objUserManagementClient != null)
                objUserManagementClient.Close();
        }

    }


    protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //chkCopyLobprofile.Enabled =            
            btnSave.Text = "Save";
            //ddlLOB.Enabled =
        treeview.Enabled =
        grvRoleCode.Enabled = true;
            lblDelMod.Visible = false;
            if (ddlAction.SelectedIndex > 0)
            {
                if (ddlAction.SelectedValue == "0")//Deletion
                {
                    ddldeletion.Visible = true;
                    ddlModification.SelectedIndex = -1;
                    ddlModification.Visible = false;
                    btnSave.Text = "Delete";
                    chkCopyLobprofile.Checked = false;
                    chkCopyLobprofile.Enabled = false;
                    ddlCopyLOb.SelectedIndex = 0;
                    ddlCopyLOb.Enabled = false;
                    
                    FunPriloadLOB();
                }
                else//Modification
                {
                    ddldeletion.Visible = false;
                    ddldeletion.SelectedIndex = -1;
                    ddlModification.Visible = true;
                    chkCopyLobprofile.Enabled = true;

                    FunPriloadLOB();
                }
                lblDelMod.Visible = true;
                lblDelMod.Text = ddlAction.SelectedItem.Text;
            }
            else
            {
                ddldeletion.Visible = false;
                ddlModification.Visible = false;
                ddlModification.SelectedIndex =
                ddldeletion.SelectedIndex = -1;
                grvRoleCode.Enabled = false;

                chkCopyLobprofile.Checked = false;
                chkCopyLobprofile.Enabled = false;
                ddlCopyLOb.SelectedIndex = 0;
                ddlCopyLOb.Enabled = false;

            }

            if (ddlLOB.SelectedIndex > 0)
                FungetLocationRoleLOB(Convert.ToInt32(ddlLOB.SelectedValue));
            else
                FungetLocationRoleLOB(-1);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
    }


    protected void ddlModification_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlModification.SelectedIndex > 0)
            {
                if (ddlModification.SelectedItem.Text.ToUpper() == "USER DETAILS")
                {
                    RfvddlLOB.Enabled = false;
                    ddlLOB.SelectedIndex = 0;
                    ddlLOB.Enabled = false;
                    grvRoleCode.Enabled = false;
                    treeview.Enabled = false;
                    chkCopyLobprofile.Checked = false;
                    chkCopyLobprofile.Enabled = false;
                    ddlCopyLOb.SelectedIndex = 0;
                    ddlCopyLOb.Enabled = false;
                    RFVCopyLOb.Enabled = false;
                }
                else
                {
                    RfvddlLOB.Enabled = true;
                    grvRoleCode.Enabled = true;
                    treeview.Enabled = true;
                    ddlLOB.Enabled = true;
                    chkCopyLobprofile.Enabled = true;
                }
                FungetLocationRoleLOB(-1);
            }
            else
            {
                ddlLOB.Enabled = false;
                if (ddlLOB.SelectedIndex > 0)
                    FungetLocationRoleLOB(Convert.ToInt32(ddlLOB.SelectedValue));
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
    }

    protected void ddldeletion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = 0;

            if (ddldeletion.SelectedValue == "0")//LOB
            {
                treeview.Enabled =
                chkCopyLobprofile.Enabled =
                grvRoleCode.Enabled = false;
                ddlLOB.Enabled = true;
                //RfvddlLOB.Enabled = true;
            }
            else if (ddldeletion.SelectedValue == "1")//Location
            {
                //RfvddlLOB.Enabled =
                //ddlLOB.Enabled =
                grvRoleCode.Enabled =
                chkCopyLobprofile.Enabled = false;
                ddlLOB.SelectedIndex = 0;
                treeview.Enabled = true;
                //RfvddlLOB.Enabled = true;
            }
            else
            {
                ddlLOB.Enabled =
                treeview.Enabled =
                chkCopyLobprofile.Enabled =
                grvRoleCode.Enabled = true;
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
    }

    /// <summary>
    /// This method is used to fetch role code as per the LOB ID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLOB_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int intLOB_ID = 0;

            //if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            //    intLOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            if (ddlLOB.SelectedIndex > 0)
                intLOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);

            FungetLocationRoleLOB(intLOB_ID);//this method is used to 
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
        finally
        {
            if (objUserManagementClient != null)
                objUserManagementClient.Close();
        }

    }

    private void FungetLocationRoleLOB(int intLOB_ID)
    {
        try
        {
            foreach (TreeNode treeviewnode in treeview.Nodes)
                FunNodeClearCheck(treeviewnode); //to clear Treeview
            DataTable dtRoleCode = null;



            byte[] byteBranchRoleDetails = objUserManagementClient.FunPubQueryUserRoleDetails(intCompanyID, intUserId, intLOB_ID, strMode);
            dtRoleCode = (DataTable)ClsPubSerialize.DeSerialize(byteBranchRoleDetails, SerializationMode.Binary, typeof(DataTable));
            grvRoleCode.DataSource = dtRoleCode;
            grvRoleCode.DataBind();


            byteBranchRoleDetails = objUserManagementClient.FunPubQueryUserLocationDetails(intCompanyID, intUserId, intLOB_ID, strMode);
            dtBranch = (DataTable)ClsPubSerialize.DeSerialize(byteBranchRoleDetails, SerializationMode.Binary, typeof(DataTable));
            ViewState["Locationss"] = dtBranch;
            FunPriCheckLocation();
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
        finally
        {
            if (objUserManagementClient != null)
                objUserManagementClient.Close();
        }

    }


    protected void ddlCopyLOb_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //validation for LOB with Copy profile LOB
        if (ddlLOB.SelectedValue == ddlCopyLOb.SelectedValue)
        {
            ddlCopyLOb.SelectedIndex = -1;
            Utility.FunShowAlertMsg(this, "Line of Business already defined");
            return;
        }

        int intCpyLobId = 0;
        if (ddlCopyLOb.SelectedIndex > 0)
            intCpyLobId = Convert.ToInt32(ddlCopyLOb.SelectedValue);
        FungetLocationRoleLOB(intCpyLobId);
        if (ddlCopyLOb.SelectedIndex > 0)
        {
            treeview.Enabled =
            grvRoleCode.Enabled = false;
        }
        else
        {
            treeview.Enabled =
            grvRoleCode.Enabled = true;
        }

    }


    /// <summary>
    /// This is used to fetch role details for selected LOB
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    //protected void chkSelLOB_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataTable dtRoleCode = null;
    //        int intLOB_ID = 0;
    //        CheckBox chkLOB = null;
    //        foreach (GridViewRow grvData in grvLOB.Rows)
    //        {
    //            chkLOB = ((CheckBox)grvData.FindControl("chkSelLOB"));
    //            if (chkLOB.Checked)
    //            {
    //                intLOB_ID = Convert.ToInt32(((Label)grvData.FindControl("lblLOBID")).Text);
    //            }
    //            else
    //                chkLOB.Enabled = false;
    //        }

    //        if (intLOB_ID == 0)
    //        {
    //            foreach (GridViewRow grvData in grvLOB.Rows)
    //            {
    //                chkLOB = ((CheckBox)grvData.FindControl("chkSelLOB"));
    //                chkLOB.Enabled = true;
    //            }
    //        }

    //        byte[] byteBranchRoleDetails = objUserManagementClient.FunPubQueryUserRoleDetails(intCompanyID, intUserId, intLOB_ID, strMode);
    //        dtRoleCode = (DataTable)ClsPubSerialize.DeSerialize(byteBranchRoleDetails, SerializationMode.Binary, typeof(DataTable));
    //        grvRoleCode.DataSource = dtRoleCode;
    //        grvRoleCode.DataBind();
    //    }
    //    catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
    //    }
    //    catch (Exception ex)
    //    {
    //        CVUsermanagement.ErrorMessage = ex.Message;
    //        CVUsermanagement.IsValid = false;
    //    }
    //    finally
    //    {
    //        if (objUserManagementClient != null)
    //            objUserManagementClient.Close();
    //    }
    //}

    /// <summary>
    /// This is used to maintain checkbox when all checkbox is selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    //protected void User_ActiveTabChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (intUserId > 0)
    //        {
    //            if (tcUserMgmt.ActiveTabIndex == 2)
    //            {
    //                trLOBMapping.Visible = true;
    //            }
    //            else
    //            {
    //                trLOBMapping.Visible = false;
    //            }
    //            if (strMode == "Q")
    //            {
    //                trLOBMapping.Visible = false;

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        CVUsermanagement.ErrorMessage = ex.Message;
    //        CVUsermanagement.IsValid = false;
    //    }

    //}

    //protected void chkSelAllRegion_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (strMode != "Q")
    //        {
    //            strbRegion.Append("<Root>");
    //            string strRegionID = string.Empty;
    //            CheckBox chkRegion = null;
    //            if (!((CheckBox)grvRegionDet.HeaderRow.FindControl("chkAll")).Checked)
    //            {
    //                foreach (GridViewRow grvData in grvRegionDet.Rows)
    //                {
    //                    chkRegion = ((CheckBox)grvData.FindControl("chkSelRegion"));
    //                    chkRegion.Checked = false;
    //                }
    //                //grvBranchDet.DataSource = dtBranch;
    //                //grvBranchDet.DataBind();
    //                return;
    //            }
    //            foreach (GridViewRow grvData in grvRegionDet.Rows)
    //            {
    //                chkRegion = ((CheckBox)grvData.FindControl("chkSelRegion"));
    //                chkRegion.Checked = true;
    //                strRegionID = ((Label)grvData.FindControl("lblRegionID")).Text;
    //                strbRegion.Append(" <Details Region_ID='" + strRegionID + "' /> ");
    //            }
    //            strbRegion.Append("</Root>");
    //            byte[] byteBranchRoleDetails = objUserManagementClient.FunPubQueryUserBranchDetails(intCompanyID, intUserId, strbRegion.ToString(), strMode);
    //            dtBranch = (DataTable)ClsPubSerialize.DeSerialize(byteBranchRoleDetails, SerializationMode.Binary, typeof(DataTable));
    //            //grvBranchDet.DataSource = dtBranch;
    //            //grvBranchDet.DataBind();
    //            dtBranch.Dispose();
    //            dtBranch = null;
    //        }
    //    }

    //    catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //    finally
    //    {
    //        objUserManagementClient.Close();
    //    }
    //}

    /// <summary>
    /// This is used to get branch details based on region
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    //protected void chkSelRegion_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (strMode != "Q")
    //        {
    //            strbRegion = new StringBuilder();
    //            strbRegion.Append("<Root>");
    //            string strRegionID = string.Empty;
    //            CheckBox chkRegion = null;
    //            bool bCheckAll = true;
    //            foreach (GridViewRow grvData in grvRegionDet.Rows)
    //            {
    //                chkRegion = ((CheckBox)grvData.FindControl("chkSelRegion"));
    //                if (chkRegion.Checked)
    //                {
    //                    strRegionID = ((Label)grvData.FindControl("lblRegionID")).Text;
    //                    strbRegion.Append(" <Details Region_ID='" + strRegionID + "' /> ");

    //                }
    //                else
    //                    bCheckAll = false;

    //            }
    //            if (!bCheckAll)
    //                ((CheckBox)grvRegionDet.HeaderRow.FindControl("chkAll")).Checked = false;
    //            else
    //                ((CheckBox)grvRegionDet.HeaderRow.FindControl("chkAll")).Checked = true;
    //            strbRegion.Append("</Root>");
    //            byte[] byteBranchRoleDetails = objUserManagementClient.FunPubQueryUserBranchDetails(intCompanyID, intUserId, strbRegion.ToString(), strMode);
    //            dtBranch = (DataTable)ClsPubSerialize.DeSerialize(byteBranchRoleDetails, SerializationMode.Binary, typeof(DataTable));
    //            //grvBranchDet.DataSource = dtBranch;
    //            //grvBranchDet.DataBind();
    //            dtBranch.Dispose();
    //            dtBranch = null;
    //        }
    //    }
    //    catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //    finally
    //    {
    //        objUserManagementClient.Close();
    //    }


    //}

    /// <summary>
    /// This is used to copy profile from the selected Code
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void ddlCopyProfile_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCopyProfile.SelectedValue == "0")
            {
                ddlCopyProfile.Enabled = false;
                chkCopyProfile.Checked = false;
                RfvddlLOB.Enabled = true;
                FunGetUserDetails();
                grvRoleCode.DataSource = null;
                grvRoleCode.DataBind();
                txtDesignation.SelectedIndex = -1;
                ddlLevel.SelectedIndex = 0;
                //ddlReportingLevel.SelectedIndex = 0;
                //ddlHigherLevel.SelectedIndex = 0;
                ddlReportingLevel.Clear();
                ddlHigherLevel.Clear();
                tbBranchAccess.Enabled = true;
                return;

            }
            RfvddlLOB.Enabled = false;
            ddlCopyProfile.Enabled = true;

            intUserId = Convert.ToInt32(ddlCopyProfile.SelectedValue);

            FunGetUserDetails();
            if ((ddlLevel.SelectedValue == "4") || ((ddlLevel.SelectedValue == "5")))
            {
                //rfvReportingLevel.Enabled = false;
                ddlReportingLevel.IsMandatory = false;
                //rfvHigherLevel.Enabled = false;
                ddlHigherLevel.IsMandatory = false;
            }
            else
            {
                //rfvReportingLevel.Enabled = false;
                ddlReportingLevel.IsMandatory = false;
                //rfvHigherLevel.Enabled = false;
                ddlHigherLevel.IsMandatory = false;
            }
            tbBranchAccess.Enabled = false;

        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }

        finally
        {
            if (objUserManagementClient != null)
                objUserManagementClient.Close();
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetUserDetails()
    {
        try
        {
            UserMgtServices.S3G_SYSAD_UserManagementRow ObjUserManagementRow;
            ObjUserManagementRow = ObjS3G_SYSAD_UserManagementDataTable.NewS3G_SYSAD_UserManagementRow();

            ObjUserManagementRow.Company_ID = intCompanyID;
            ObjUserManagementRow.User_ID = intUserId;

            ObjS3G_SYSAD_UserManagementDataTable.AddS3G_SYSAD_UserManagementRow(ObjUserManagementRow);
            if (ddlCopyProfile.SelectedValue != "0")
            {
                strMode = "M";
            }
            byte[] byteBranchRoleDetails = objUserManagementClient.FunPubQueryBranchRoleDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_UserManagementDataTable, SerMode), strMode);
            dsBranchRole = (DataSet)ClsPubSerialize.DeSerialize(byteBranchRoleDetails, SerializationMode.Binary, typeof(DataSet));

            if (intUserId > 0)
            {
                byte[] byteUserDetails = objUserManagementClient.FunPubQueryUser(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_UserManagementDataTable, SerMode));

                ObjS3G_SYSAD_UserManagementDataTable = (UserMgtServices.S3G_SYSAD_UserManagementDataTable)ClsPubSerialize.DeSerialize(byteUserDetails, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_UserManagementDataTable));
                hdnID.Value = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Created_By"].ToString();
                if (ddlCopyProfile.SelectedValue == "0")
                {
                    txtUserCode.Text = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["User_Code"].ToString();
                    txtUserName.Text = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["User_Name"].ToString();
                    //txtPassword.Text = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Password"].ToString();
                    //txtPassword.Attributes.Add("value", ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Password"].ToString());
                    txtMobileNo.Text = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Mobile_No"].ToString();
                    txtEMailId.Text = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Email_ID"].ToString();
                    txtDateofJoining.Text = DateTime.Parse(ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["DOJ"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                    //ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["DOJ"].ToString();
                }
                else
                {
                    //txtUserCode.Text = "";
                    //txtUserName.Text = "";
                    //txtMobileNo.Text = "";
                    //txtEMailId.Text = "";
                    //txtDateofJoining.Text = "";
                    txtDateofJoining.Text = DateTime.Now.ToString(strDateFormat);
                }
                txtDesignation.SelectedValue = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Designation"].ToString();
                ddlLevel.SelectedValue = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["User_Level_ID"].ToString();
                ddlReportingLevel.SelectedValue = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Reporting_Next_level"].ToString();
                ddlHigherLevel.SelectedValue = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Reporting_Higher_level"].ToString();

                ddlReportingLevel.SelectedText = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Reporting_Next_Name"].ToString();
                ddlHigherLevel.SelectedText = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Reporting_High_Name"].ToString();

                if (!string.IsNullOrEmpty(ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["User_Group_Id"].ToString()))
                    ddlUserGroup.SelectedValue = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["User_Group_Id"].ToString();

                chkActive.Checked = ObjS3G_SYSAD_UserManagementDataTable.Rows[0]["Is_Active"].ToString() == "True" ? true : false;


                //Line of Business
                if (dsBranchRole.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsBranchRole.Tables[0].Rows[0]["LOB_ID"].ToString()))
                        ddlLOB.SelectedValue = dsBranchRole.Tables[0].Rows[0]["LOB_ID"].ToString();
                }
                if (dsBranchRole.Tables[1].Rows.Count > 0)
                {
                    ViewState["Locationss"] = dsBranchRole.Tables[1];

                }
                if (dsBranchRole.Tables[2].Rows.Count > 0)
                {
                    grvRoleCode.DataSource = dsBranchRole.Tables[2];
                    grvRoleCode.DataBind();
                    FunPriChkAllRecords("grvRoleCode");
                }
                if (dsBranchRole.Tables[3].Rows.Count > 0)
                {
                    ddlCopyLOb.FillDataTable(dsBranchRole.Tables[3], "LOB_ID", "LOB");
                }
            }
            else
            {
                //grvRegionDet.DataSource = dsBranchRole.Tables[0];
                //grvRegionDet.DataBind();
                //FunPriChkAllRecords("grvRegionDet");

                //grvLOB.DataSource = dsBranchRole.Tables[1];
                //grvLOB.DataBind();

            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
        finally
        {
            dsBranchRole.Dispose();
            dsBranchRole = null;

        }
    }

    private void FunPriChkAllRecords(string gridName)
    {
        try
        {
            bool booCheckedAll = true;
            switch (gridName)
            {
                //case "grvRegionDet":
                //    CheckBox chkRegionAll = grvRegionDet.HeaderRow.FindControl("chkAll") as CheckBox;
                //    CheckBox chkRegion = null;
                //    foreach (GridViewRow grvData in grvRegionDet.Rows)
                //    {
                //        chkRegion = ((CheckBox)grvData.FindControl("chkSelRegion"));
                //        if (!chkRegion.Checked)
                //        {
                //            booCheckedAll = false;
                //            break;
                //        }
                //    }

                //    if (booCheckedAll)
                //    {
                //        chkRegionAll.Checked = true;
                //    }
                //    break;


                //case "grvBranchDet":
                //    CheckBox chkallBranch = grvBranchDet.HeaderRow.FindControl("chkAll") as CheckBox;
                //    CheckBox chkBranch = null;
                //    foreach (GridViewRow grvData in grvBranchDet.Rows)
                //    {
                //        chkBranch = ((CheckBox)grvData.FindControl("chkSelBranch"));
                //        if (!chkBranch.Checked)
                //        {
                //            booCheckedAll = false;
                //            break;
                //        }
                //    }

                //    if (booCheckedAll)
                //    {
                //        chkallBranch.Checked = true;
                //    }
                //    break;


                case "grvRoleCode":
                    CheckBox chkallRoleCode = grvRoleCode.HeaderRow.FindControl("chkAll") as CheckBox;
                    CheckBox chkSelRoleCode = null;

                    foreach (GridViewRow grvData in grvRoleCode.Rows)
                    {
                        chkSelRoleCode = ((CheckBox)grvData.FindControl("chkSelRoleCode"));
                        if (!chkSelRoleCode.Checked)
                        {
                            booCheckedAll = false;
                            break;
                        }

                    }

                    if (booCheckedAll)
                    {
                        chkallRoleCode.Checked = true;
                    }
                    break;

                //case "grvLOB":

                //    if (strMode == "Q")
                //    {
                //        int iCountQuery = 0;
                //        CheckBox chkSelLOB = null;
                //        foreach (GridViewRow grvData in grvLOB.Rows)
                //        {
                //            chkSelLOB = ((CheckBox)grvData.FindControl("chkSelLOB"));
                //            if (iCountQuery > 0)
                //            {
                //                chkSelLOB.Enabled = false;
                //            }
                //            iCountQuery++;
                //        }
                //    }
                //    break;

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriloadLocationTreeview()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            DataSet ds = new DataSet();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            ds = Utility.GetDataset("S3G_SYSAD_GETLOCATIONTREEVIEW", Procparam);
            ds.DataSetName = "Menus";
            ds.Tables[0].TableName = "Menu";
            //ViewState["TVMAxLevel"] = ds.Tables[0].Compute("max(Hierarchy_Type)", "Hierarchy_Type>0");
            DataRelation relation = new DataRelation("ParentChild",
            ds.Tables[0].Columns["Location_ID"],
            ds.Tables[0].Columns["pid"], true);
            relation.Nested = true;
            ds.Relations.Add(relation);

            XmlDataSource1.Data = ds.GetXml();
            XmlDataSource1.DataBind();



        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    //
    protected void treeview_OnTreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        try
        {
            FunNodeCheckChildnode(e.Node);
            FunPriLocationbasedRoles();
        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }

    }

    private void FunPriLocationbasedRoles()
    {
        string strSelLocations = "";
        foreach (TreeNode node in treeview.CheckedNodes)
        {
            strSelLocations += node.Value + ",";
        }
        if (strSelLocations.Length > 0)
            strSelLocations = strSelLocations.Remove(strSelLocations.Length - 1);
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_Id", intCompanyID.ToString());
        Procparam.Add("@User_ID", intUserId.ToString());
        if (ddlCopyLOb.SelectedIndex > 0)
            Procparam.Add("@LOB_ID", ddlCopyLOb.SelectedValue);
        else if (ddlLOB.SelectedIndex > 0)
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        Procparam.Add("@Location_ID", strSelLocations);
        Procparam.Add("@Strmode", strMode);
        DataTable dttble = new DataTable();
        dttble = Utility.GetDefaultData("S3G_SYSAD_Get_User_RoleLocationDtls", Procparam);
        //FunNodeCheckParentnode(e.Node);
        if (dttble.Rows.Count > 0)
        {
            grvRoleCode.DataSource = dttble;
            grvRoleCode.DataBind();
            FunPriChkAllRecords("grvRoleCode");
        }
        else
        {
            grvRoleCode.DataSource = null;
            grvRoleCode.DataBind();
            FunPriChkAllRecords("grvRoleCode");
        }
    }

    private void FunNodeCheckParentnode(TreeNode e)
    {
        try
        {
            //int checkcount = 0;
            //for (int i = 0; i < e.Parent.ChildNodes.Count; i++)
            //{
            //     if (e.Parent.ChildNodes[i].Checked)
            //        checkcount++;

            //}
            //if (checkcount > 0)
            //{
            //    e.Parent.Checked = true;
            //}
            //else
            //{
            e.Parent.Checked = false;
            //}
            if (e.Parent.Parent != null)
            {
                FunNodeCheckParentnode(e.Parent);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    /// This method is used to clear all checked items in a treeview.
    /// </summary>
    /// <param name="e"></param>
    private void FunNodeClearCheck(TreeNode e)
    {
        try
        {
            for (int i = 0; i < e.ChildNodes.Count; i++)
            {
                e.ChildNodes[i].Checked = false;
                if (e.ChildNodes[i].ChildNodes.Count > 0)
                    FunNodeClearCheck(e.ChildNodes[i]);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }






    private void FunNodeCheckMod(TreeNode e, DataTable dtLocation)
    {
        try
        {
            for (int i = 0; i < e.ChildNodes.Count; i++)
            {

                foreach (DataRow dr in dtLocation.Rows)
                {
                    if (dr["Location_Id"].ToString() == e.ChildNodes[i].Value)
                        e.ChildNodes[i].Checked = true;

                }
                if (e.ChildNodes[i].ChildNodes.Count > 0)
                    FunNodeCheckMod(e.ChildNodes[i], dtLocation);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunNodeCheckChildnode(TreeNode e)
    {
        try
        {
            for (int i = 0; i < e.ChildNodes.Count; i++)
            {
                e.ChildNodes[i].Checked = e.Checked;
                if (e.ChildNodes[i].ChildNodes.Count > 0)
                    FunNodeCheckChildnode(e.ChildNodes[i]);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }


    }

    private void FunPriLoadDescription()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            DataTable dtlocationview = Utility.GetDefaultData("S3G_SYSAD_GETLOCATIONTREEVIEW", Procparam);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriloadLOB()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            //string strProcName = SPNames.SYS_UserLOBMapping;
            Procparam = new Dictionary<string, string>();
            //if (intUserId > 0)
            //    Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Program_ID", "14");
            Procparam.Add("@Is_Active", "1");
            if (PageMode == PageModes.Query || (PageMode == PageModes.Modify && ddlAction.SelectedItem.Text == "Deletion"))
            {
                //New Stored procedure call - To load LOB based on User Rights - Kuppusamy.B - April-03-2012
                string strProcName = "S3G_Get_UserLOBMapping_UserManagement";
                Procparam.Add("@User_ID", intUserId.ToString());
                ddlLOB.BindDataTable(strProcName, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            }
            else
            {
                ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This is used to bind user code in ;
    /// </summary>
    private void FunBindUserCode()
    {
        try
        {
            UserMgtServices.S3G_SYSAD_UserMaster_ListRow ObjUserMasterRow;
            ObjUserMasterRow = ObjS3G_SYSAD_UserMasterDataTable.NewS3G_SYSAD_UserMaster_ListRow();
            ObjUserMasterRow.Company_ID = intCompanyID;
            ObjUserMasterRow.User_ID = intUserId;
            ObjS3G_SYSAD_UserMasterDataTable.AddS3G_SYSAD_UserMaster_ListRow(ObjUserMasterRow);
            byte[] byteUserDetails = objUserManagementClient.FunPubQueryUserMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_UserMasterDataTable, SerMode));
            ObjS3G_SYSAD_UserMasterDataTable = (UserMgtServices.S3G_SYSAD_UserMaster_ListDataTable)ClsPubSerialize.DeSerialize(byteUserDetails, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_UserMaster_ListDataTable));
            //ddlReportingLevel.FillDataTable(ObjS3G_SYSAD_UserMasterDataTable, "User_ID", "User_Name");
            // ddlHigherLevel.FillDataTable(ObjS3G_SYSAD_UserMasterDataTable, "User_ID", "User_Name");
            //Current user can not be reporting level user or higher level Removing current user id
            //ddlHigherLevel.Items.Remove(ddlHigherLevel.Items.FindByValue(intUserId.ToString()));
            //ddlReportingLevel.Items.Remove(ddlReportingLevel.Items.FindByValue(intUserId.ToString()));
            //
            //ddlCopyProfile.FillDataTable(ObjS3G_SYSAD_UserMasterDataTable, "User_ID", "User_Code");



        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    // changed by bhuvana for performance on September 13th 2013//
    [System.Web.Services.WebMethod]
    public static string[] GetUsers(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_UserCode_Details_AGT", Procparam));

        return suggetions.ToArray();
    }

    //end here//

    private void FunPriBindUserLOBMapping()
    {
        try
        {
            //This is to bind user LOB mapping
            /*  Procparam = new Dictionary<string, string>();
              string strProcName = SPNames.SYS_UserLOBMapping;
              Procparam = new Dictionary<string, string>();
              Procparam.Add("@User_ID", intUserId.ToString());
              ddlLOBMapping.BindDataTable(strProcName, Procparam, new string[] { "LOB_ID", "LOB" });*/
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This is used to get branch and role details based on region and user
    /// </summary>

    private void FunGetBranchRoleSelectedDet()
    {
        try
        {
            CheckBox chkBranch = null;
            string strLocationID = string.Empty;
            string strRegionID = string.Empty;

            strbSelLocation.Append("<Root>");
            //foreach (GridViewRow grvData in grvBranchDet.Rows)
            //{
            //    chkBranch = ((CheckBox)grvData.FindControl("chkSelBranch"));
            //    if (chkBranch.Checked)
            //    {
            //        strBranchID = ((Label)grvData.FindControl("lblBranchID")).Text;
            //        strRegionID = ((Label)grvData.FindControl("lblRegionID")).Text;
            //        strbSelLocation.Append(" <Details Branch_ID='" + strBranchID + "' Region_ID='" + strRegionID + "' /> ");
            //    }
            //}
            foreach (TreeNode node in treeview.CheckedNodes)
            {
                strLocationID = node.Value;
                strbSelLocation.Append(" <Details Location_ID='" + strLocationID + "' /> ");
            }
            strbSelLocation.Append("</Root>");

            CheckBox chkRoleCode = null;
            string strRoleCenterID = string.Empty;


            strbSelRoleCode.Append("<Root>");
            foreach (GridViewRow grvData in grvRoleCode.Rows)
            {
                chkRoleCode = ((CheckBox)grvData.FindControl("chkSelRoleCode"));
                if (chkRoleCode.Checked)
                {
                    strRoleCenterID = ((Label)grvData.FindControl("lblRoleCenterID")).Text;
                    strbSelRoleCode.Append(" <Details RoleCode_ID='" + strRoleCenterID + "' /> ");
                }
            }
            strbSelRoleCode.Append("</Root>");

            CheckBox chkLOB = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
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

    #endregion


    protected void chkCopyProfile_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            /* if (chkCopyProfile.Checked)
             {
                 ddlCopyProfile.Enabled = true;
             }
             else
             {
                 ddlCopyProfile.SelectedIndex = 0;
                 ddlCopyProfile.Enabled = false;
                 FunGetUserDetails();
                 grvRoleCode.DataSource = null;
                 grvRoleCode.DataBind();
                 txtDesignation.Text = "";
                 ddlLevel.SelectedIndex = 0;
                 ddlReportingLevel.SelectedIndex = 0;
                 ddlHigherLevel.SelectedIndex = 0;

             }*/
            //if (ddlCopyProfile.SelectedIndex > 0)
            //    ddlCopyProfile.SelectedIndex = 0;
            ddlCopyProfile.Clear();
            ddlCopyProfile.Enabled = chkCopyProfile.Checked;
            if (!chkCopyProfile.Checked)
            {
                grvRoleCode.DataSource = null;
                grvRoleCode.DataBind();
               // txtDesignation.Text = "";
                txtDesignation.SelectedIndex = -1; //By Siva.K 24MAR2015 Exception throwed when uncheck the check box.
                ddlLevel.SelectedIndex = 0;
                //ddlReportingLevel.SelectedIndex = 0;
                //ddlHigherLevel.SelectedIndex = 0;
                ddlReportingLevel.Clear();
                ddlHigherLevel.Clear();
                //RFVCopyProfile.Enabled = false;
                ddlCopyProfile.IsMandatory = false;
            }
            else
            {
                ddlCopyProfile.IsMandatory = true;
            }
            //RFVCopyProfile.Enabled = true;

        }
        catch (Exception ex)
        {
            CVUsermanagement.ErrorMessage = ex.Message;
            CVUsermanagement.IsValid = false;
        }
        finally
        {
            objUserManagementClient.Close();
        }

    }

    protected void chkCopyLobprofile_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCopyLOb.Enabled = chkCopyLobprofile.Checked;
            ddlCopyLOb.SelectedIndex = -1;
            if (chkCopyLobprofile.Checked)
                RFVCopyLOb.Enabled = true;
            else
            {
                RFVCopyLOb.Enabled = false;
                grvRoleCode.Enabled = true;
                treeview.Enabled = true;
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
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    //FunPriBindUserLOBMapping();
                    txtDateofJoining.Text = DateTime.Now.ToString(strDateFormat);
                    chkActive.Enabled = false;
                    chkResetPwd.Checked = true;
                    chkCopyLobprofile.Enabled = ddlCopyLOb.Enabled = lblLOBCpyProfile.Enabled = false;
                    lblAction.Visible = ddlAction.Visible = false;
                    break;

                case 1: // Modify Mode
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    txtUserCode.Enabled = false;
                    //Code Added on 20-May-2015 - Username allow edittable except for SYSADM User - Start//
                    string strusercodee;
                    strusercodee = txtUserCode.Text;
                    if (strusercodee == "SYSADM")
                    {
                        txtUserName.Enabled = false;
                    }
                    else
                    {
                        txtUserName.Enabled = true;
                    }
                    //Code Added on 20-May-2015 - Username allow edittable except for SYSADM User - End//
                    txtDateofJoining.Enabled = true;
                    btnClear.Enabled = false;
                    txtPassword.Attributes.Add("value", "*************");
                    //txtPassword.Text = "*************";
                    txtPassword.Enabled = false;
                    ddlCopyProfile.Enabled =ddlLOB.Enabled= false;
                    chkResetPwd.Checked = false;
                    divReset.Style.Add("display", "Block");
                    chkCopyProfile.Enabled = false;
                    CalendarExtender1.Enabled = true;
                    //chkCopyLobprofile.Enabled =
                    //ddlCopyLOb.Enabled = 
                    lblLOBCpyProfile.Enabled = true;
                    //txtPassword.Attributes.Add("onfocus", "clearPwd(););");
                    //document.getElementById('<%=txtPassword.ClientID%>').value=''
                    //txtPassword.Attributes.Add("onfocus", "alert('kkr');");
                    //FunPriBindUserLOBMapping();
                    lblAction.Visible = ddlAction.Visible = true;
                    grvRoleCode.Enabled = false;
                    RFVddlAction.Enabled = true;
                    if (!bDelete)
                        ddlAction.Items.RemoveAt(1);
                    break;

                case -1:// Query Mode   
                    FunPriBindUserLOBMapping();
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPageView);
                    }
                    ddlCopyProfile.Enabled = true;

                    if (bClearList)
                    {
                        //ddlCopyProfile.ClearDropDownList();
                        ddlCopyProfile.ReadOnly = true;
                        ddlLevel.ClearDropDownList();
                        // ddlReportingLevel.ClearDropDownList();
                        ddlReportingLevel.ReadOnly = true;

                        // ddlHigherLevel.ClearDropDownList();
                        ddlHigherLevel.ReadOnly = true;
                        txtDesignation.ClearDropDownList();
                    }

                    //lit = new ListItem(ddlLOBMapping.SelectedItem.Text, ddlLOBMapping.SelectedItem.Value);
                    //ddlLOBMapping.Items.Clear();
                    //ddlLOBMapping.Items.Add(lit);
                    chkActive.Enabled = false;
                    //txtDesignation.Enabled = false;
                    txtEMailId.ReadOnly = true;
                    txtMobileNo.ReadOnly = true;
                    txtUserCode.ReadOnly = true;
                    txtUserName.ReadOnly = true;
                    txtDateofJoining.ReadOnly = true;
                    txtDateofJoining.Attributes.Remove("onblur");
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    btnRemoveLOB.Enabled = false;
                    txtPassword.Attributes.Add("value", "*************");
                    txtPassword.ReadOnly = true;
                    //chkResetPwd.Checked = true;

                    chkResetPwd.Disabled = true;
                    divReset.Style.Add("display", "Block");
                    chkCopyProfile.Enabled = false;
                    ddlCopyProfile.Enabled = false;
                    CalendarExtender1.Enabled = false;
                    chkCopyLobprofile.Enabled = ddlCopyLOb.Enabled = lblLOBCpyProfile.Enabled = false;
                    lblAction.Visible = ddlAction.Visible = false;
                    //txtPassword.Attributes.Add("onfocus", "clearPwd(););");
                    //document.getElementById('<%=txtPassword.ClientID%>').value=''
                    //txtPassword.Attributes.Add("onfocus", "alert('kkr');");                
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    ////grvRegionDet.Columns[1].Visible=false;
                    //grvRegionDet.Columns[1].Visible = false;
                    //grvBranchDet.Columns[2].Visible = false;
                    //grvRoleCode.Columns[2].Visible = false;
                    grvRoleCode.Columns[2].Visible = false;

                    ddlUserGroup.ClearDropDownList();

                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriloadDesignation()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            txtDesignation.BindDataTable("S3G_SA_GET_DESIGN", Procparam, new string[] { "DESIGNATION_ID", "DESIGNATION_NAME" });
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    private void FunPriloadUserGroup()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyID.ToString());
            if (intUserId <= 0)//Create mode
            {
                Procparam.Add("@Is_Active", "1");
            }
            DataTable dt = Utility.GetDefaultData("S3G_SYSAD_Get_UserGroup", Procparam);
            ddlUserGroup.FillDataTable(dt, "GROUP_ID", "GROUP_NAME", true);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


}
