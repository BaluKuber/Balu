/// Module Name      :   System Admin
/// Screen Name      :   S3GSysAdminCustomerAlert_Add
/// Created By       :   Ramesh M
/// Created Date     :   22-May-2010
/// Purpose          :   To insert and update CustomerAlert details
/// 
/// Last updated by  :   Gunasekar.K
/// Last Updated Date:   15-Oct-2010
/// Purpose          :   To address the Bug -1736
#region Namesapces
using System;
using System.Web.Security;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using S3GBusEntity;
#endregion
public partial class System_Admin_S3GSysAdminCustomerAlert_Add : ApplyThemeForProject
{
    #region Initialization
    int intErrCode = 0;
    int intCustomerAlertId = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bClearList = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = new UserInfo();
    UserMgtServicesReference.UserMgtServicesClient ObjRoleCodeclient;
    CompanyMgtServicesReference.CompanyMgtServicesClient ObjCompanyServicesClient;
    CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable ObjCustomerAlert_DataTable = new CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable();
    CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable ObjEntity_DataTable = new CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable();
    CompanyMgtServices.S3G_SYSAD_EventTypeDataTable ObjEvent_DataTable = new CompanyMgtServices.S3G_SYSAD_EventTypeDataTable();
    CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable ObjLOB_DataTable = new CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable();
    UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable ObjRoleCode_Datatable = new UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable();
    int inthdUserid;
    string strRedirectPageMC;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "../System Admin/S3GSysAdminCustomerAlert_View.aspx";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminCustomerAlert_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminCustomerAlert_Add.aspx';";
    /// <summary>
    ///Page Load Events
    /// </summary
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (Request.QueryString["qsCAId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCAId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intCustomerAlertId = Convert.ToInt32(fromTicket.Name);
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
            bDelete = ObjUserInfo.ProDeleteRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            if (!IsPostBack)
            {
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (((intCustomerAlertId > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);

                }
                else if (((intCustomerAlertId > 0)) && (strMode == "Q"))
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
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    /// <summary>
    ///Bind Customer Alert Details
    /// </summary
    private void FunLoadBinding()
    {

        //Bind EntityType Master
        ObjCompanyServicesClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            SerializationMode SerMode = new SerializationMode();
            byte[] bytesEntityTypeMaster = ObjCompanyServicesClient.FunPubQueryEntityTypeMaster(SerMode, ClsPubSerialize.Serialize(ObjEntity_DataTable, SerMode));
            ObjEntity_DataTable = (CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable)ClsPubSerialize.DeSerialize(bytesEntityTypeMaster, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable));
            ddlEntityType.FillDataTable(ObjEntity_DataTable, ObjEntity_DataTable.Entity_Type_IDColumn.ColumnName, ObjEntity_DataTable.Entity_Type_NameColumn.ColumnName);
            ddlEntityType.AddItemToolTip();

            //Bind EventType Master
            //Commented by saranya on 09-Mar-2012 to remove Event Type field based on sudarsan observation
            /*
            byte[] bytesEventTypeMaster = ObjCompanyServicesClient.FunPubQueryEventTypeMaster(SerMode, ClsPubSerialize.Serialize(ObjEvent_DataTable, SerMode));
            ObjEvent_DataTable = (CompanyMgtServices.S3G_SYSAD_EventTypeDataTable)ClsPubSerialize.DeSerialize(bytesEventTypeMaster, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_EventTypeDataTable));
            ddlEventType.FillDataTable(ObjEvent_DataTable, ObjEvent_DataTable.Event_Type_IDColumn.ColumnName, ObjEvent_DataTable.Event_Type_NameColumn.ColumnName);
            ddlEventType.AddItemToolTip();
             */
            //End Here
            /*
            //Bind SMS
            byte[] bytesEntityTypeMasterforSMS = ObjCompanyServicesClient.FunPubQueryEntityTypeMaster(SerMode, ClsPubSerialize.Serialize(ObjEntity_DataTable, SerMode));
            ObjEntity_DataTable = (CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable)ClsPubSerialize.DeSerialize(bytesEntityTypeMasterforSMS, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable));
            ddlSMS.FillDataTable(ObjEntity_DataTable, ObjEntity_DataTable.Entity_Type_IDColumn.ColumnName, ObjEntity_DataTable.Entity_Type_NameColumn.ColumnName);

            //Bind Email
            byte[] bytesEntityTypeMasterforEMAIL = ObjCompanyServicesClient.FunPubQueryEntityTypeMaster(SerMode, ClsPubSerialize.Serialize(ObjEntity_DataTable, SerMode));
            ObjEntity_DataTable = (CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable)ClsPubSerialize.DeSerialize(bytesEntityTypeMasterforEMAIL, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable));
            ddlEmail.FillDataTable(ObjEntity_DataTable, ObjEntity_DataTable.Entity_Type_IDColumn.ColumnName, ObjEntity_DataTable.Entity_Type_NameColumn.ColumnName);
            */

            //Bind LOB
            /*CompanyMgtServices.S3G_SYSAD_LOBMasterRow ObjLOBMasterRow;
            ObjLOBMasterRow = ObjLOB_DataTable.NewS3G_SYSAD_LOBMasterRow();
            ObjLOBMasterRow.Company_ID =intCompanyID;
            ObjLOBMasterRow.Is_Active = true;
            ObjLOB_DataTable.AddS3G_SYSAD_LOBMasterRow(ObjLOBMasterRow);
            ObjCompanyServicesClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            byte[] bytesLOBListDetails = ObjCompanyServicesClient.FunPubQueryLOB_LIST(SerMode, ClsPubSerialize.Serialize(ObjLOB_DataTable, SerMode));
            ObjLOB_DataTable = (CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable)ClsPubSerialize.DeSerialize(bytesLOBListDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable));
            ddlLOB.FillDataTable(ObjLOB_DataTable, ObjLOB_DataTable.LOB_IDColumn.ColumnName, ObjLOB_DataTable.LOBCode_LOBNameColumn.ColumnName);
            */

            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            if (intCustomerAlertId == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }

            //Commented By Saranya based on sudarsan observation

            //--Added by Guna on 15th-Oct-2010 to address the Bud -1736
            //if (strMode != "M" && strMode != "Q")
            //{
            //    Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            //}
            //Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            //--Ends Here 
                        
            Procparam.Add("@Program_ID", "13");
            Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            ddlLOB.BindDataTable("S3G_Get_LOB_LIST", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.AddItemToolTip();
            Procparam = null;


            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //if (intCustomerAlertId == 0)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            //if (intUserID != 0)
            //Procparam.Add("@User_ID", intUserID.ToString());
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            //ddlRoleCode.BindDataTable(SPNames.SYS_RoleCodeMaster, Procparam, new string[] { "Role_Code_ID", "Role_Code" });
            if (intCustomerAlertId > 0)
            {
                CompanyMgtServices.S3G_SYSAD_CustomerAlertRow ObjCustomerAlertRow;
                ObjCustomerAlertRow = ObjCustomerAlert_DataTable.NewS3G_SYSAD_CustomerAlertRow();
                ObjCustomerAlertRow.Customer_Alert_ID = intCustomerAlertId;
                ObjCustomerAlert_DataTable.AddS3G_SYSAD_CustomerAlertRow(ObjCustomerAlertRow);
                byte[] byteCustomerAlert = ObjCompanyServicesClient.FunPubQueryCustomerAlert(SerMode, ClsPubSerialize.Serialize(ObjCustomerAlert_DataTable, SerMode));
                ObjCustomerAlert_DataTable = (CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable)ClsPubSerialize.DeSerialize(byteCustomerAlert, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable));
                hdnID.Value = ObjCustomerAlert_DataTable.Rows[0]["Created_By"].ToString();
                if (ObjCustomerAlert_DataTable.Rows.Count > 0)
                {
                    ddlEntityType.SelectedValue = ObjCustomerAlert_DataTable.Rows[0]["Entity_Type_ID"].ToString();
                    //ddlEventType.SelectedValue = ObjCustomerAlert_DataTable.Rows[0]["Event_Type_ID"].ToString();
                    ddlLOB.SelectedValue = ObjCustomerAlert_DataTable.Rows[0]["LOB_ID"].ToString();
                    FunPriLoadRolecode(ddlLOB.SelectedValue);
                    ddlRoleCode.SelectedValue = ObjCustomerAlert_DataTable.Rows[0]["Role_Code_ID"].ToString();
                    chkSMS.Checked = Convert.ToBoolean( ObjCustomerAlert_DataTable.Rows[0]["Target_SMS"]);
                    chkEMAIL.Checked = Convert.ToBoolean( ObjCustomerAlert_DataTable.Rows[0]["Target_Email"]);
                    if (ObjCustomerAlert_DataTable.Rows[0]["Is_Active"].ToString() == "True")
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;
                    ddlEntityType.Enabled = false;
                    //ddlEventType.Enabled = false;
                    ddlLOB.Enabled = false;

                    if (ddlRoleCode.SelectedValue == "0")
                        ddlRoleCode.Enabled = true;
                    else
                        ddlRoleCode.Enabled = false;
                    
                }
            }

            //ddlEntityType.AddItemToolTip();
            //ddlEventType.AddItemToolTip();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjCompanyServicesClient.Close();
        }
    }
    #endregion

    #region Create CustomerAlert and Update
    /// <summary>
    ///Save And Update Customer Alert Details
    /// </summary
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjCompanyServicesClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            if (chkEMAIL.Checked == false && chkSMS.Checked == false)
            {
                Utility.FunShowAlertMsg(this, "Select Target SMS/Target EMail.");

                return;
            }

            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertRow ObjCustomerAlertRow;
            ObjCustomerAlertRow = ObjCustomerAlert_DataTable.NewS3G_SYSAD_CustomerAlertRow();
            ObjCustomerAlertRow.Customer_Alert_ID = intCustomerAlertId;
            ObjCustomerAlertRow.Company_ID = intCompanyID;
            ObjCustomerAlertRow.Entity_Type_ID = Convert.ToInt32(ddlEntityType.SelectedItem.Value.Trim());
            ObjCustomerAlertRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedItem.Value.Trim());
            ObjCustomerAlertRow.Role_Code_ID = Convert.ToInt32(ddlRoleCode.SelectedItem.Value.Trim());
            ObjCustomerAlertRow.Event_Type_ID = 0;//Convert.ToInt32(ddlEventType.SelectedItem.Value.Trim());
            ObjCustomerAlertRow.Target_Email = chkEMAIL.Checked;
            ObjCustomerAlertRow.Target_SMS = chkSMS.Checked;
            if (chkActive.Checked)
                ObjCustomerAlertRow.Is_Active = true;
            else
                ObjCustomerAlertRow.Is_Active = false;
            ObjCustomerAlertRow.Created_By = intUserID;
            ObjCustomerAlertRow.Modified_By = intUserID;
            ObjCustomerAlert_DataTable.AddS3G_SYSAD_CustomerAlertRow(ObjCustomerAlertRow);
            SerializationMode SerMode = SerializationMode.Binary;

          
            if (intCustomerAlertId > 0)
            {
                if (FunCheckLobisvalid(ddlLOB.SelectedValue))
                {
                    intErrCode = ObjCompanyServicesClient.FunPubModifyCustomerAlert(SerMode, ClsPubSerialize.Serialize(ObjCustomerAlert_DataTable, SerMode));
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
                intErrCode = ObjCompanyServicesClient.FunPubCreateCustomerAlert(SerMode, ClsPubSerialize.Serialize(ObjCustomerAlert_DataTable, SerMode));
            }
            ObjCompanyServicesClient.Close();
           
            if (intErrCode == 1)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The Entered Alert Details Already Exists');", true);
            else if (intErrCode == 2)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The Entered Alert Details Already Exists');", true);
            else if (intErrCode == 3)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The Entered Alert Details Already Exists');", true);
            else if (intErrCode == 50)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Unable to Save the Entered Alert Details');", true);
            else
            {
                if (intCustomerAlertId > 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The Alert Details updated successfully');window.location.href='../System Admin/S3GSysAdminCustomerAlert_View.aspx';", true);
                }

                else
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strAlert = "The Entered Alert Details added successfully";
                    strAlert += @"\n\nWould you like to add one more Alert?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
            }

            lblErrorMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjCompanyServicesClient.Close();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    #region Delete customer Alert
    /// <summary>
    ///Delete Customer Alert Details
    /// </summary
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ObjCompanyServicesClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertRow ObjCustomerAlertRow;
            ObjCustomerAlertRow = ObjCustomerAlert_DataTable.NewS3G_SYSAD_CustomerAlertRow();
            ObjCustomerAlertRow.Customer_Alert_ID = intCustomerAlertId;
            ObjCustomerAlert_DataTable.AddS3G_SYSAD_CustomerAlertRow(ObjCustomerAlertRow);
            ObjCompanyServicesClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;
            intErrCode = ObjCompanyServicesClient.FunPubDeleteCustomerAlert(SerMode, ClsPubSerialize.Serialize(ObjCustomerAlert_DataTable, SerMode));
            ObjCompanyServicesClient.Close();
            if (intErrCode == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Successfully Deleted');window.location.href='../System Admin/S3GSysAdminCustomerAlert_View.aspx';", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Deleted Failed');window.location.href='../System Admin/S3GSysAdminCustomerAlert_View.aspx';", true);

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjCompanyServicesClient.Close();
        }
    }
    #endregion
    /// <summary>
    ///Clear Customer Alert Details
    /// </summary
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            if (intCustomerAlertId == 0)
            {
                ddlEntityType.SelectedIndex = 0;
                ddlLOB.SelectedIndex = 0;
                ddlRoleCode.SelectedIndex = 0;
                //ddlEventType.SelectedIndex = 0;
                chkSMS.Checked = false;
                chkEMAIL.Checked = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #region Disable
    /// <summary>
    ///Access Rights
    /// </summary
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
                    chkActive.Enabled = false;
                    FunLoadBinding();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    break;

                case 1: // Modify Mode
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    FunLoadBinding();
                    btnClear.Enabled = false;
                    chkActive.Enabled = true;
                    //chkSMS.Enabled = false;
                    //chkEMAIL.Enabled = false;
                    if (ddlRoleCode.SelectedIndex == 0)
                    {
                        ddlRoleCode.Enabled = false;
                    }
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    break;

                case -1:// Query Mode
                    FunLoadBinding();
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPageView);
                    }

                    chkActive.Enabled = false;
                    chkEMAIL.Enabled = false;
                    chkSMS.Enabled = false;
                    if (bClearList)
                    {
                        ddlEntityType.ClearDropDownList();
                        //ddlEventType.ClearDropDownList();
                        ddlLOB.ClearDropDownList();
                        ddlRoleCode.ClearDropDownList();
                        
                    }



                    ddlEntityType.Enabled = true;
                    //ddlEventType.Enabled = true;
                    ddlLOB.Enabled = true;
                    ddlRoleCode.Enabled = true;
                    //chkSMS.Enabled = true;
                    //chkEMAIL.Enabled = true;

                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    break;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadRolecode(ddlLOB.SelectedValue);
            ddlLOB.AddItemToolTip();
            //ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
            ddlEntityType.AddItemToolTip();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadRolecode(string strLOBID)
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (intCustomerAlertId == 0)
            {
                Procparam.Add("@Is_Active", "1");
                Procparam.Add("@User_ID", intUserID.ToString());
            }
            if (intUserID != 0)
                //Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@LOB_ID", strLOBID);
            Procparam.Add("@Is_Alert", "1");
            
            //DataSet DSRoleCode = Utility.GetDataset(SPNames.SYS_RoleCodeMaster, Procparam);
          
            //commented and Modified by saranya I based on sudarsan observation to get master programs
            //DataRow[] dr = DSRoleCode.Tables[0].Select("PROGRAM_ID IN(31,37,38,42,95,108,110,111,147,146)", "Program_Role", DataViewRowState.CurrentRows);
            //DataTable DTRoleCode = dr.CopyToDataTable();
            
            DataTable DTRoleCode = Utility.GetDefaultData(SPNames.SYS_RoleCodeMaster, Procparam);
            ddlRoleCode.BindDataTable(DTRoleCode, new string[] { "Role_Code_ID", "Program_Role" });
            ddlRoleCode.AddItemToolTip();
            /*Srivatsan change ends*/
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private bool FunCheckLobisvalid(string strLobId)
    {
        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm.Add("@Company_ID", intCompanyID.ToString());
        Procparm.Add("@User_Id", intUserID.ToString());
        Procparm.Add("@LOB_ID", strLobId);
        
        if (intCustomerAlertId == 0)
        {
            Procparm.Add("@Is_Active", "1");
        }
        Procparm.Add("@Is_UserLobActive", "1");
        Procparm.Add("@Program_ID", "13");
        DataTable dtBool = Utility.GetDefaultData("S3G_GetUserLobMapping", Procparm);
        if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
            return true;
        else
            return false;
    }
}
