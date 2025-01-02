/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminRoleCenterMaster_Add
/// Created By      :   Ramesh M
/// Created Date    :   21-May-2010
/// Purpose         :   To insert and update Role Center master details
/// Last updated By      :   Gunasekar.K
/// Last updated Date    :   12-Oct-2010
/// Purpose         :   To filter the Programs based on the module selection 
#region Namespaces
using System;
using System.Web.Security;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Data;
using System.Text;
using S3GBusEntity;
using System.Configuration;
#endregion
public partial class System_Admin_S3GSysAdminRoleCenterMaster_Add : ApplyThemeForProject
{
    #region Initialization
    int intErrCode = 0;
    int intRoleCodeId = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    int intflag = 0;
    int inthdUserid;
    string strRedirectPageMC;
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bClearList = false;
    bool bMakerChecker = false;
    UserMgtServicesReference.UserMgtServicesClient ObjRoleCenterMasterClient;
    UserMgtServices.S3G_SYSAD_ModuleMasterDataTable ObjS3G_ModuleMaster_DataTable = new UserMgtServices.S3G_SYSAD_ModuleMasterDataTable();
    UserMgtServices.S3G_SYSAD_ProgramMasterDataTable ObjS3G_ProgramMaster_DataTable = new UserMgtServices.S3G_SYSAD_ProgramMasterDataTable();
    UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable ObjS3G_RoleCodeMaster_DataTable = new UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable();
    UserMgtServices.S3G_SYSAD_RoleCenterMaster_ListDataTable ObjS3G_RoleCenterMasterList_DataTable = new UserMgtServices.S3G_SYSAD_RoleCenterMaster_ListDataTable();

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "../System Admin/S3GSysAdminRoleCenterMaster_View.aspx";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminRoleCenterMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminRoleCenterMaster_Add.aspx';";
    UserInfo ObjUserInfo = new UserInfo();
    /// <summary>
    /// Page Load Event
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (Request.QueryString["qsRCId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsRCId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intRoleCodeId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            bDelete = ObjUserInfo.ProDeleteRW;

            FunSetComboBoxAttributes(cmbRoleCenterCode, "City", "1");

            RoleCenterMasterDetailsAdd.Visible = true;

            if (!IsPostBack)
            {

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (((intRoleCodeId > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);

                }
                else if (((intRoleCodeId > 0)) && (strMode == "Q"))
                {

                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }
    /// <summary>
    /// Get Role Code Details 
    /// </summary>

    void LoadModuleDropDownList()
    {
        try
        {
            //ObjRoleCenterMasterClient = new UserMgtServicesReference.UserMgtServicesClient();
            UserMgtServices.S3G_SYSAD_ModuleMasterRow ObjModuleMasterRow;
            ObjModuleMasterRow = ObjS3G_ModuleMaster_DataTable.NewS3G_SYSAD_ModuleMasterRow();
            ObjModuleMasterRow.Module_ID = 0;
            ObjS3G_ModuleMaster_DataTable.AddS3G_SYSAD_ModuleMasterRow(ObjModuleMasterRow);
            SerializationMode SerMode = new SerializationMode();
            byte[] bytesModuleMasterDetails = ObjRoleCenterMasterClient.FunPubQueryModuleMasterList(SerMode, ClsPubSerialize.Serialize(ObjS3G_ModuleMaster_DataTable, SerMode));
            ObjS3G_ModuleMaster_DataTable = (UserMgtServices.S3G_SYSAD_ModuleMasterDataTable)ClsPubSerialize.DeSerialize(bytesModuleMasterDetails, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_ProgramMasterDataTable));
            ddlModule.FillDataTable(ObjS3G_ModuleMaster_DataTable, ObjS3G_ModuleMaster_DataTable.Module_IDColumn.ColumnName, ObjS3G_ModuleMaster_DataTable.Module_CodeColumn.ColumnName);
            ddlModule.AddItemToolTip();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    void LoadProgramDropDownList(string ModuleCode)
    {
        try
        {
            if (strMode != string.Empty)
            {
                SerializationMode SerMode = new SerializationMode();
                UserMgtServices.S3G_SYSAD_ProgramMasterRow ObjProgramMasterRow;
                ObjProgramMasterRow = ObjS3G_ProgramMaster_DataTable.NewS3G_SYSAD_ProgramMasterRow();
                ObjProgramMasterRow.Program_ID = 0;
                ObjS3G_ProgramMaster_DataTable.AddS3G_SYSAD_ProgramMasterRow(ObjProgramMasterRow);
                byte[] bytesProgramMasterDetails = ObjRoleCenterMasterClient.FunPubQueryProgramMasterList(SerMode, ClsPubSerialize.Serialize(ObjS3G_ProgramMaster_DataTable, SerMode), ModuleCode);
                ObjS3G_ProgramMaster_DataTable = (UserMgtServices.S3G_SYSAD_ProgramMasterDataTable)ClsPubSerialize.DeSerialize(bytesProgramMasterDetails, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_ProgramMasterDataTable));
                ddlProgram.FillDataTable(ObjS3G_ProgramMaster_DataTable, ObjS3G_ProgramMaster_DataTable.Program_IDColumn.ColumnName, ObjS3G_ProgramMaster_DataTable.Program_CodeColumn.ColumnName);
                ddlProgram.AddItemToolTip();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private void FunGetRoleCenterMasterDetails()
    {
        ObjRoleCenterMasterClient = new UserMgtServicesReference.UserMgtServicesClient();
        try
        {
            //Module Master List
            LoadModuleDropDownList();
            SerializationMode SerMode = new SerializationMode();
            //Bind Role center code
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Is_Active", "1");
            cmbRoleCenterCode.BindDataTable(SPNames.SYS_RoleCenterCode_List, Procparam, new string[] { "Role_Center_id", "ROLE_CENTER_CODE" });

            if (intRoleCodeId > 0)
            {
                UserMgtServices.S3G_SYSAD_RoleCodeMasterRow ObjRoleCodeMasaterRow;
                ObjRoleCodeMasaterRow = ObjS3G_RoleCodeMaster_DataTable.NewS3G_SYSAD_RoleCodeMasterRow();
                ObjRoleCodeMasaterRow.Role_Center_ID = intRoleCodeId;
                ObjS3G_RoleCodeMaster_DataTable.AddS3G_SYSAD_RoleCodeMasterRow(ObjRoleCodeMasaterRow);
                byte[] bytesRoleCenterMaster = ObjRoleCenterMasterClient.FunPubQueryRoleCodeMasterDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_RoleCodeMaster_DataTable, SerMode));
                ObjS3G_RoleCodeMaster_DataTable = (UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesRoleCenterMaster, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
                if (ObjS3G_RoleCodeMaster_DataTable.Rows.Count > 0)
                {
                    cmbRoleCenterCode.SelectedValue = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["Role_Center_ID"].ToString().Trim();
                    txtRoleCenterName.Text = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["Role_Center_Name"].ToString();
                    ddlModule.SelectedValue = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["MODULE_ID"].ToString();
                    LoadProgramDropDownList(ddlModule.SelectedItem.Text.Split(new char[] { '-' }).GetValue(0).ToString());
                    ddlProgram.SelectedValue = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["PROGRAM_ID"].ToString();
                    txtRoleCode.Text = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["ROLE_CODE"].ToString();
                    //LoadWFPrograms();
                    //ddlWFProgram.SelectedValue= ObjS3G_RoleCodeMaster_DataTable.Rows[0]["WFProgram_Id"].ToString();
                    txtWFProgram.Text = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["WFProgram_Id"].ToString();
                    hdnID.Value = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["Created_By"].ToString();
                    if (ObjS3G_RoleCodeMaster_DataTable.Rows[0]["IS_ACTIVE"].ToString() == "True")
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;

                    ddlModule.Enabled = false;
                    cmbRoleCenterCode.Enabled = false;
                    ddlProgram.Enabled = false;
                    //ddlWFProgram.Enabled = false;
                    txtWFProgram.Enabled = false;
                    txtRoleCenterName.Enabled = false;
                    txtRoleCode.Enabled = false;
                }
                else
                {
                    // Utility.FunShowAlertMsg(this, "User does not have rights to the Role Code"); 
                    //Response.Redirect("~/System Admin/S3GSysAdminRoleCenterMaster_View.aspx");
                    strAlert = strAlert.Replace("__ALERT__", "User does not have rights to the Role Code");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
        finally
        {
            ObjRoleCenterMasterClient.Close();
        }
    }
    #endregion

    #region Create and Modify
    /// <summary>
    /// Create and Modify Role Code Details
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjRoleCenterMasterClient = new UserMgtServicesReference.UserMgtServicesClient();
        try
        {
            if (txtWFProgram.Text.Trim().Length > 0)
            {
                int WKProgram;
                if (int.TryParse(txtWFProgram.Text, out WKProgram))
                {
                    if (Convert.ToInt32(txtWFProgram.Text.Trim()) < 200 || Convert.ToInt32(txtWFProgram.Text.Trim()) > 700)
                    {
                        Utility.FunShowAlertMsg(this.Page, "WorkFlow Program number should be greater than or equal to 200 and less than or equal to 700");
                        return;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "WorkFlow Program number should be integer.");
                    return;
                }
            }

            UserMgtServices.S3G_SYSAD_RoleCodeMasterRow ObjRoleCodeMasterRow;
            ObjRoleCodeMasterRow = ObjS3G_RoleCodeMaster_DataTable.NewS3G_SYSAD_RoleCodeMasterRow();
            ObjRoleCodeMasterRow.Role_Code_ID = intRoleCodeId;
            //ObjRoleCodeMasterRow.Role_Center_ID=Convert.ToInt32(cmbRoleCenterCode.SelectedValue.Text.ToString().Trim());
            ObjRoleCodeMasterRow.Role_Center_Code = cmbRoleCenterCode.SelectedItem.Text.ToString().Trim();
            ObjRoleCodeMasterRow.Role_Center_Name = txtRoleCenterName.Text.Trim();
            ObjRoleCodeMasterRow.Company_ID = intCompanyID;
            ObjRoleCodeMasterRow.Module_ID = Convert.ToInt32(ddlModule.SelectedItem.Value);
            ObjRoleCodeMasterRow.Program_ID = Convert.ToInt32(ddlProgram.SelectedItem.Value);
            //if(ddlWFProgram.Items.Count>1)
            //ObjRoleCodeMasterRow.WFProgram_Id= Convert.ToInt32(ddlWFProgram.SelectedValue);
            if (txtWFProgram.Text != string.Empty)
                ObjRoleCodeMasterRow.WFProgram_Id = Convert.ToInt32(txtWFProgram.Text);
            ObjRoleCodeMasterRow.Role_Code = txtRoleCode.Text.Trim().Split(new char[] { '-' }).GetValue(0).ToString();
            if (chkActive.Checked)
                ObjRoleCodeMasterRow.Is_Active = true;
            else
                ObjRoleCodeMasterRow.Is_Active = false; ;
            ObjRoleCodeMasterRow.Created_By = intUserID;
            ObjRoleCodeMasterRow.Modified_By = intUserID;
            ObjS3G_RoleCodeMaster_DataTable.AddS3G_SYSAD_RoleCodeMasterRow(ObjRoleCodeMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            //if (txtRoleCode.Text.Trim().Length!=5)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Role Center Details Added Failed Due to Insufficient RoleCode Digit.');", true);
            //    return;
            //}
            if (intRoleCodeId > 0)
            {
                intErrCode = ObjRoleCenterMasterClient.FunPubModifyRoleCodeMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_RoleCodeMaster_DataTable, SerMode));
            }
            else
            {
                intErrCode = ObjRoleCenterMasterClient.FunPubCreateRoleCodeMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_RoleCodeMaster_DataTable, SerMode));
            }
            if (intErrCode == 0)
            {
                txtRoleCenterName.Text = string.Empty;
                ddlModule.SelectedIndex = 0;
                ddlProgram.SelectedIndex = 0;
                txtRoleCode.Text = string.Empty;
                RoleCenterMasterDetailsAdd.Visible = false;
            }
            if (intErrCode == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Role code already exists');", true);
                RoleCenterMasterDetailsAdd.Visible = false;
            }
            else if (intErrCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Role center name already exists');", true);
            }
            else if (intErrCode == 3)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Role Center details already exists');", true);
            }
            else if (intErrCode == 4)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('WorkFlow Program already exists');", true);
            }
            else if (intErrCode == 50)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Unable to add Role center details');", true);
            }
            else
            {
                if (intRoleCodeId > 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Role center details updated successfully');window.location.href='../System Admin/S3GSysAdminRoleCenterMaster_View.aspx';", true);
                }
                else
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strAlert = "Role Center details added successfully";
                    strAlert += @"\n\nWould you like to add one more Role Center?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
        finally
        {
            ObjRoleCenterMasterClient.Close();
        }
    }
    #endregion

    #region Role Code Logic
    /// <summary>
    /// Role Code Mapped
    /// </summary>
    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObjRoleCenterMasterClient = new UserMgtServicesReference.UserMgtServicesClient();
        try
        {
            string sModuleCode = string.Empty;
            if (ddlModule.SelectedIndex == 0)
            {
                txtRoleCode.Text = string.Empty;
            }
            if (ddlModule.SelectedIndex > 0)
            {

                txtRoleCode.Text = string.Empty;
                if (!(string.IsNullOrEmpty(cmbRoleCenterCode.SelectedItem.Text.ToString().Trim())))
                {
                    txtRoleCode.Text = cmbRoleCenterCode.SelectedItem.Text.Trim().Substring(0, 1);
                }
                if (ddlModule.SelectedIndex > 0)
                    txtRoleCode.Text += ddlModule.SelectedItem.Text.Trim().Substring(0, 1);

                sModuleCode = ddlModule.SelectedItem.Text.Trim().Substring(0, 1);
                //-- Added by Guna on 12-Oct-2010 To add the Module code 
                //Program Master List                
                SerializationMode SerMode = new SerializationMode();
                UserMgtServices.S3G_SYSAD_ProgramMasterRow ObjProgramMasterRow;
                ObjProgramMasterRow = ObjS3G_ProgramMaster_DataTable.NewS3G_SYSAD_ProgramMasterRow();
                ObjProgramMasterRow.Program_ID = 0;
                ObjS3G_ProgramMaster_DataTable.AddS3G_SYSAD_ProgramMasterRow(ObjProgramMasterRow);
                byte[] bytesProgramMasterDetails = ObjRoleCenterMasterClient.FunPubQueryProgramMasterList(SerMode, ClsPubSerialize.Serialize(ObjS3G_ProgramMaster_DataTable, SerMode), sModuleCode);
                ObjS3G_ProgramMaster_DataTable = (UserMgtServices.S3G_SYSAD_ProgramMasterDataTable)ClsPubSerialize.DeSerialize(bytesProgramMasterDetails, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_ProgramMasterDataTable));
                ddlProgram.FillDataTable(ObjS3G_ProgramMaster_DataTable, ObjS3G_ProgramMaster_DataTable.Program_IDColumn.ColumnName, ObjS3G_ProgramMaster_DataTable.Program_CodeColumn.ColumnName);
                ddlProgram.AddItemToolTip();
                if (ddlProgram.SelectedIndex > 0)
                    txtRoleCode.Text += ddlProgram.SelectedItem.Text.Trim().Substring(0, 3);
                //--Ends Here

            }
            else
            {
                ddlProgram.Items.Clear();
            }
            ddlModule.AddItemToolTip();
            //ddlModule.ToolTip = ddlModule.SelectedItem.Text;
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
            throw ae;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
        }
        finally
        {
            ObjRoleCenterMasterClient.Close();
        }
    }
    /// <summary>
    /// Role Code Mapped
    /// </summary>
    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProgram.SelectedIndex == 0)
            {
                txtRoleCode.Text = string.Empty;
            }
            if (ddlProgram.SelectedIndex > 0)
            {
                LoadRoleCode();

                //LoadWFPrograms();
            }
            ddlProgram.AddItemToolTip();
            ddlProgram.ToolTip = ddlProgram.SelectedItem.Text;
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }

    private void LoadRoleCode()
    {
        try
        {
            txtRoleCode.Text = string.Empty;
            if (!(string.IsNullOrEmpty(cmbRoleCenterCode.SelectedItem.Text.ToString().Trim())))
            {
                txtRoleCode.Text = cmbRoleCenterCode.SelectedItem.Text.Trim().Substring(0, 1);
            }
            if (ddlModule.SelectedIndex > 0)
                txtRoleCode.Text += ddlModule.SelectedItem.Text.Trim().Substring(0, 1);
            //if (ddlProgram.SelectedIndex > 0 && ddlWFProgram.SelectedIndex <= 0)
            if (ddlProgram.SelectedIndex > 0 && txtWFProgram.Text == string.Empty)
                txtRoleCode.Text += ddlProgram.SelectedItem.Text.Trim().Substring(0, 3);
            else if (txtWFProgram.Text != string.Empty)
            {
                //if (Convert.ToInt64(txtWFProgram.Text) < 200)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "WorkFlow Program number should be greater than or equal to 200 and less than or equal to 700");
                //    return;
                //}
                //else if (Convert.ToInt64(txtWFProgram.Text) > 700)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "WorkFlow Program number should be greater than or equal to 200 and less than or equal to 700");
                //    return;
                //}
                if (txtWFProgram.Text.Trim().Length > 0)
                {
                    int WKProgram;
                    if (int.TryParse(txtWFProgram.Text, out WKProgram))
                    {
                        if (Convert.ToInt32(txtWFProgram.Text.Trim()) < 200 || Convert.ToInt32(txtWFProgram.Text.Trim()) > 700)
                        {
                            Utility.FunShowAlertMsg(this.Page, "WorkFlow Program number should be greater than or equal to 200 and less than or equal to 700");
                            return;
                        }
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this.Page, "WorkFlow Program number should be integer.");
                        return;
                    }
                }

                txtRoleCode.Text += txtWFProgram.Text.Trim();
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }

    //private void LoadWFPrograms()
    //{
    //    // Bind Workflow Program ID
    //    Dictionary<string, string> paramSP = new Dictionary<string, string>();
    //    paramSP.Add("@ProgramId", ddlProgram.SelectedValue);
    //    ddlWFProgram.BindDataTable("S3G_SYSAD_GetWFProgramsByProgramId", paramSP, new string[] { "Workflow_Prg_ID", "Workflow_Prg_ID" });
    //}
    /// <summary>
    /// Navigation to View Page
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/System Admin/S3GSysAdminRoleCenterMaster_View.aspx");
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }

    #endregion

    #region Delete
    /// <summary>
    /// Delete Role Code
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ObjRoleCenterMasterClient = new UserMgtServicesReference.UserMgtServicesClient();
        try
        {
            UserMgtServices.S3G_SYSAD_RoleCodeMasterRow ObjRoleCodeRow;
            ObjRoleCodeRow = ObjS3G_RoleCodeMaster_DataTable.NewS3G_SYSAD_RoleCodeMasterRow();
            ObjRoleCodeRow.Role_Code_ID = intRoleCodeId;
            ObjS3G_RoleCodeMaster_DataTable.AddS3G_SYSAD_RoleCodeMasterRow(ObjRoleCodeRow);
            SerializationMode Sermode = SerializationMode.Binary;
            intErrCode = ObjRoleCenterMasterClient.FunPubDeleteRoleCenterMaster(Sermode, ClsPubSerialize.Serialize(ObjS3G_RoleCodeMaster_DataTable, Sermode));
            if (intErrCode == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Role Code details successfully deleted');window.location.href='../System Admin/S3GSysAdminRoleCenterMaster_View.aspx';", true);
            else if (intErrCode == 1)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Role Code cannot be deleted since it has been used in transaction');window.location.href='../System Admin/S3GSysAdminRoleCenterMaster_View.aspx';", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Deleted Failed');window.location.href='../System Admin/S3GSysAdminRoleCenterMaster_View.aspx';", true);
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
        finally
        {
            ObjRoleCenterMasterClient.Close();
        }
    }
    #endregion

    /// <summary>
    /// Clear Role Code Details
    /// </summary>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            if (intRoleCodeId == 0)
            {
                if (ddlProgram.Items.Count > 1)
                    ddlProgram.SelectedIndex = 0;
                if (ddlModule.Items.Count > 1)
                    ddlModule.SelectedIndex = 0;
                //if (ddlWFProgram.Items.Count > 1)
                //    ddlWFProgram.SelectedIndex = 0;
                if (cmbRoleCenterCode.Items.Count > 1)
                    cmbRoleCenterCode.SelectedIndex = 0;
                txtWFProgram.Text = txtRoleCenterName.Text = txtRoleCode.Text = string.Empty;

            }
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }
    /// <summary>
    /// Get Role Centre Name based on Role Centre Code 
    /// </summary>
    protected void cmbRoleCenterCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObjRoleCenterMasterClient = new UserMgtServicesReference.UserMgtServicesClient();
        try
        {
            txtRoleCenterName.Text = "";
            txtRoleCode.Text = "";
            ddlModule.SelectedIndex = 0;
            txtRoleCenterName.Enabled = true;
            txtWFProgram.Text = "";
            if (ddlProgram.Items.Count > 0)
                ddlProgram.SelectedIndex = 0;
            ddlProgram.Items.Clear();
            if (!(string.IsNullOrEmpty(cmbRoleCenterCode.SelectedItem.Text.ToString().Trim())))
            {
                txtRoleCode.Text = cmbRoleCenterCode.SelectedItem.Text.ToString().Trim().Substring(0, 1);
                UserMgtServices.S3G_SYSAD_RoleCodeMasterRow ObjRoleCodeMasaterRow;
                ObjRoleCodeMasaterRow = ObjS3G_RoleCodeMaster_DataTable.NewS3G_SYSAD_RoleCodeMasterRow();
                ObjRoleCodeMasaterRow.Company_ID = intCompanyID;
                ObjRoleCodeMasaterRow.Role_Center_Code = cmbRoleCenterCode.SelectedItem.Text.ToString().Trim().Substring(0, 1);
                ObjS3G_RoleCodeMaster_DataTable.AddS3G_SYSAD_RoleCodeMasterRow(ObjRoleCodeMasaterRow);
                SerializationMode Sermode = SerializationMode.Binary;
                byte[] bytesRoleCenterCodeChk = ObjRoleCenterMasterClient.FunPubCheckRoleCodeMasterDetails(Sermode, ClsPubSerialize.Serialize(ObjS3G_RoleCodeMaster_DataTable, Sermode));
                ObjS3G_RoleCodeMaster_DataTable = (UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesRoleCenterCodeChk, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
                if (ObjS3G_RoleCodeMaster_DataTable.Rows.Count > 0)
                {
                    txtRoleCenterName.Text = string.Empty;
                    txtRoleCenterName.Text = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["Role_Center_Name"].ToString();
                    txtRoleCenterName.Enabled = false;
                    ddlModule.Focus();
                    ddlModule.AddItemToolTip();
                }
                else
                    txtRoleCenterName.Focus();
            }
            else
            {
                ddlProgram.Items.Clear();
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
        finally
        {
            ObjRoleCenterMasterClient.Close();
        }
    }
    /// <summary>
    /// Get Role Centre Name based on Role Centre Code 
    /// </summary>
    protected void cmbRoleCenterCode_ItemInserted(object sender, AjaxControlToolkit.ComboBoxItemInsertEventArgs e)
    {
        ObjRoleCenterMasterClient = new UserMgtServicesReference.UserMgtServicesClient();
        try
        {
            txtRoleCenterName.Text = "";
            txtRoleCode.Text = "";
            ddlModule.SelectedIndex = 0;
            if (ddlProgram.Items.Count > 0)
                ddlProgram.SelectedIndex = 0;
            ddlProgram.Items.Clear();
            txtWFProgram.Text = "";
            txtRoleCenterName.Text = "";
            txtRoleCenterName.Enabled = true;
            if (!(string.IsNullOrEmpty(cmbRoleCenterCode.SelectedItem.Text.ToString().Trim())))
            {
                txtRoleCode.Text = cmbRoleCenterCode.SelectedItem.Text.ToString().Trim().Substring(0, 1);
                UserMgtServices.S3G_SYSAD_RoleCodeMasterRow ObjRoleCodeMasaterRow;
                ObjRoleCodeMasaterRow = ObjS3G_RoleCodeMaster_DataTable.NewS3G_SYSAD_RoleCodeMasterRow();
                ObjRoleCodeMasaterRow.Company_ID = intCompanyID;
                ObjRoleCodeMasaterRow.Role_Center_Code = cmbRoleCenterCode.SelectedItem.Text.ToString().Trim().Substring(0, 1);
                ObjS3G_RoleCodeMaster_DataTable.AddS3G_SYSAD_RoleCodeMasterRow(ObjRoleCodeMasaterRow);
                SerializationMode Sermode = SerializationMode.Binary;
                byte[] bytesRoleCenterCodeChk = ObjRoleCenterMasterClient.FunPubCheckRoleCodeMasterDetails(Sermode, ClsPubSerialize.Serialize(ObjS3G_RoleCodeMaster_DataTable, Sermode));
                ObjS3G_RoleCodeMaster_DataTable = (UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesRoleCenterCodeChk, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
                if (ObjS3G_RoleCodeMaster_DataTable.Rows.Count > 0)
                {

                    txtRoleCenterName.Text = string.Empty;
                    txtRoleCenterName.Text = ObjS3G_RoleCodeMaster_DataTable.Rows[0]["Role_Center_Name"].ToString();
                    txtRoleCenterName.Enabled = false;
                    ddlModule.Focus();

                }
                else
                    txtRoleCenterName.Focus();
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
        finally
        {
            ObjRoleCenterMasterClient.Close();
        }
    }

    #region ValueDisable
    /// <summary>
    /// Access Rights
    /// </summary>
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
                    FunGetRoleCenterMasterDetails();
                    if (ddlProgram.Items.Count == 0)
                        ddlProgram.Items.Add(new ListItem("--Select--", "--Select--"));
                    chkActive.Enabled = false;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    btnDelete.Visible = false;
                    break;

                case 1: // Modify Mode
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }

                    FunGetRoleCenterMasterDetails();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    btnClear.Enabled = false;
                    if (bDelete)
                        btnDelete.Visible = true;
                    break;

                case -1:// Query Mode

                    FunGetRoleCenterMasterDetails();
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPageView);
                    }

                    btnDelete.Visible = false;
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    chkActive.Enabled = false;
                    cmbRoleCenterCode.Enabled = false;
                    ddlModule.Enabled = true;
                    ddlProgram.Enabled = true;
                    //if (ddlWFProgram.Items.Count > 1)
                    //{
                    //    ddlWFProgram.ClearDropDownList();
                    //    ddlWFProgram.Enabled = true;
                    //}
                    txtWFProgram.ReadOnly = true;
                    if (txtWFProgram.Text != string.Empty)
                        txtWFProgram.Enabled = true;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    if (bClearList)
                    {
                        if (ddlProgram.Items.Count > 0)
                            ddlProgram.ClearDropDownList();
                        if (ddlModule.Items.Count > 0)
                            ddlModule.ClearDropDownList();
                    }
                    ListItem lit;
                    lit = new ListItem(cmbRoleCenterCode.SelectedItem.Text, cmbRoleCenterCode.SelectedItem.Value);
                    cmbRoleCenterCode.Items.Clear();
                    cmbRoleCenterCode.Items.Add(lit);



                    lit = new ListItem(cmbRoleCenterCode.SelectedItem.Text, cmbRoleCenterCode.SelectedItem.Value);
                    if (cmbRoleCenterCode.Items.Count > 0)
                        cmbRoleCenterCode.Items.Clear();
                    cmbRoleCenterCode.Items.Add(lit);

                    txtRoleCenterName.ReadOnly = true;
                    txtRoleCenterName.Enabled = true;
                    txtRoleCode.ReadOnly = true;
                    txtRoleCode.Enabled = true;
                    break;
            }
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }
    #endregion

    //protected void ddlWFProgram_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LoadRoleCode();
    //    //txtRoleCode.Text += "-"+ ddlWFProgram.SelectedItem.Text;
    //}
    protected void ddlModule_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtWFProgram_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadRoleCode();
        }
        catch (Exception ae)
        {
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }

    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeypress", "maxlengthfortxt('" + maxLength + "');fnAvoidZero('true');");
            //textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
        }
    }
}
