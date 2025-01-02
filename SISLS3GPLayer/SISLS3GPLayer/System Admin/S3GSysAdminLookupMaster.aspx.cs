/// Module Name        :   System Admin
/// Screen Name        :   S3GSysAdminLookupMaster.aspx
/// Created By         :   Santhosh S
/// Created Date       :   25-July-2013
/// Purpose            :   To insert look up master details
/// Modified By        :   
/// Modified Date      :   

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Configuration;

public partial class System_Admin_S3GSysAdminLookupMaster : ApplyThemeForProject
{
    #region Variable declaration

    CompanyMgtServicesReference.CompanyMgtServicesClient objManualLookupClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
    S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ManualLookupDataTable ObjS3G_SYS_ManualLookupDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;


    //User Authorization
    UserInfo ObjUserInfo = new UserInfo();
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    //Code end

    int intErrCode = 0;
    int intCompanyID = 0;
    int intUserID = 0;

    Dictionary<string, string> Procparam = null;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "../System Admin/S3GSysAdminLookupMaster.aspx";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminLookupMaster.aspx';";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminLookupMaster.aspx';";
    static string strPageName = "Lookup Master";
    DataTable dtManualLookup = null;
    DataTable dtManualLookupModify = null;

    string strMode = string.Empty;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #region User Defined Functions

    private void FunPriLoadPage()
    {
        try
        {
            //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            S3GSession ObjS3GSession = new S3GSession();

            ////User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

            if (!IsPostBack)
            {
                pnlDetail.Visible = false;
                LoadModuleDropDownList();

                //btnSave.Enabled = true;
                if (!bCreate)
                    imgbtnAdd.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion

    /// <summary>
    /// To bind Lookup Type
    /// </summary>

    private void FunBindLookupType(int intProgram_ID)
    {
        try
        {
            ddlLookupTypeDesc.Enabled = true;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Program_ID", intProgram_ID.ToString());
            ddlLookupTypeDesc.BindDataTable("S3G_Get_ManualLookupDesc", Procparam, new string[] { "Lookup_Def_ID", "Lookup_Desc" }); // prc_GetManualLookupDesc
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = "Unable to load Lookup Type";
        }
    }

    #region Button Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        objManualLookupClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

        try
        {
            if (ViewState["NewLookup"] == null)
            {
                Utility.FunShowAlertMsg(this.Page, "Add atleast one Lookup Value");
                return;
            }
           

            ObjS3G_SYS_ManualLookupDataTable = new S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ManualLookupDataTable();
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ManualLookupRow ObjManualLookupRow;
            ObjManualLookupRow = ObjS3G_SYS_ManualLookupDataTable.NewS3G_SYSAD_ManualLookupRow();
            ObjManualLookupRow.Company_ID = intCompanyID; // 1
            ObjManualLookupRow.Lookup_Desc = ddlLookupTypeDesc.SelectedItem.Text;
            ObjManualLookupRow.Lookup_Def_ID = Convert.ToInt32(ddlLookupTypeDesc.SelectedValue);
            ObjManualLookupRow.Lookup_Name = Utility.FunPubFormXml((DataTable)ViewState["NewLookup"]);
            ObjManualLookupRow.Created_By = intUserID; // 2
            ObjManualLookupRow.Is_Active = true;
            //ObjManualLookupRow.Company_ID = intCompanyID; // 1
            //ObjManualLookupRow.Lookup_Desc = ddlLookupTypeDesc.SelectedItem.Text;
            //ObjManualLookupRow.Lookup_Def_ID = Convert.ToInt32(ddlLookupTypeDesc.SelectedValue);
            //ObjManualLookupRow.Lookup_Name = txtLookupName.Text;
            ////Utility.FunPubFormXml((DataTable)ViewState["NewLookup"]); 
            //ObjManualLookupRow.Created_By = intUserID; // 2
            //ObjManualLookupRow.Is_Active = true;
            //ObjManualLookupRow.Modified_By = intUserID;
            //ObjManualLookupRow.Lookup_Type = txtLookupName.Text;
            //ObjManualLookupRow.Table_Name = "";

            ObjS3G_SYS_ManualLookupDataTable.AddS3G_SYSAD_ManualLookupRow(ObjManualLookupRow);
            SerializationMode SerMode = SerializationMode.Binary;

            //objManualLookupClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            intErrCode = objManualLookupClient.FunPubCreateManualLookup(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYS_ManualLookupDataTable, SerMode));

            if (intErrCode == 0)
            {
                //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                strAlert = "Lookup details added successfully";
                strAlert += @"\n\nWould you like to add one more Lookup?";

                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;

            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Lookup Name already exist");
            }
            lblErrorMessage.Text = string.Empty;

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objManualLookupClient.Close();
        }
    }
    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProgram.SelectedItem.Text == "--Select--")
            {
                FunPubResetControls();
                if (ddlLookupTypeDesc.SelectedItem.Text != "--Select--")
                {
                    FunBindLookupType(Convert.ToInt32(ddlProgram.SelectedValue));
                    ddlLookupTypeDesc.ClearDropDownList();
                    //ddlLookupTypeDesc.Enabled = false;
                    return;
                }
            }
            if (ddlModule.SelectedItem.Text != "--Select--")
            {
                pnlDetail.Visible = false;
                gvLookupMaster.DataSource = null;
                gvLookupMaster.DataBind();
                FunBindLookupType(Convert.ToInt32(ddlProgram.SelectedValue));
                ddlProgram.AddItemToolTip();
                ddlProgram.ToolTip = ddlProgram.SelectedItem.Text;
            }
            else
            {
                pnlDetail.Visible = false;
                ddlProgram.SelectedValue = "0";
                ddlModule.SelectedValue = "0";
                ddlModule.ClearDropDownList();
                ddlProgram.ClearDropDownList();
                btnSave.Enabled = false;
            }

        }
        catch (Exception ae)
        {
            throw ae;
        }
    }
    void LoadModuleDropDownList()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@SysAdmin", intUserID.ToString());
            Procparam.Add("@MODULE_CODE", null);
            Procparam.Add("@MODULE_ID", "0");
            Procparam.Add("@LookUp", "M");
            ddlModule.BindDataTable("S3G_Get_Program_ManualLookup", Procparam, new string[] { "MODULE_ID", "MODULE_CODE" });
            ddlModule.AddItemToolTip();

        }
        catch (Exception e)
        {
            throw e;
        }
    }
    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlModule.SelectedValue) == 0)
        {
            if (Convert.ToInt32(ddlProgram.SelectedValue) > 0)
            {
                ddlProgram.ClearDropDownList();
            }
            if (Convert.ToInt32(ddlProgram.SelectedValue) == 0)
            {
                ddlProgram.ClearDropDownList();
            }
            if (Convert.ToInt32(ddlLookupTypeDesc.SelectedValue) > 0)
            {
                FunBindLookupType(Convert.ToInt32(ddlProgram.SelectedValue));
                ddlLookupTypeDesc.ClearDropDownList();
            }
        }
        if (ddlModule.SelectedIndex > 0)
        {
            LoadProgramDropDownList(ddlModule.SelectedItem.Text);
            btnClear.Enabled = true;
            pnlDetail.Visible = false;
            gvLookupMaster.DataSource = null;
            gvLookupMaster.DataBind();
            btnSave.Enabled = false;
            if (ddlLookupTypeDesc.SelectedValue.ToString() != "")
            {
                ddlLookupTypeDesc.ClearSelection();
            }
        }
        else
        {
            LoadModuleDropDownList();
            if (ddlModule.SelectedIndex == 0)
            {
                ddlProgram.ClearSelection();
                ddlProgram.SelectedItem.Text = "--Select--";
            }
            FunPubResetControls();
            if (ddlProgram.SelectedIndex > 0)
                ddlProgram.ClearDropDownList();

        }
    }
    void FunPubResetControls()
    {
        btnClear.Enabled = true;
        pnlDetail.Visible = false;
        gvLookupMaster.DataSource = null;
        gvLookupMaster.DataBind();
        //if (ddlLookupTypeDesc.SelectedValue!="0")       
        //    ddlLookupTypeDesc.ClearDropDownList();            
        btnSave.Enabled = false;
    }
    void LoadProgramDropDownList(string ModuleCode)
    {
        try
        {
            string sModuleCode = ddlModule.SelectedItem.Text.Trim().Substring(0, 1);
            if (ModuleCode != string.Empty)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@SysAdmin", intUserID.ToString());
                Procparam.Add("@MODULE_CODE", sModuleCode);
                Procparam.Add("@MODULE_ID", "0");
                Procparam.Add("@LookUp", "P");
                ddlProgram.BindDataTable("S3G_Get_Program_ManualLookup", Procparam, new string[] { "PROGRAM_ID", "PROGRAM_CODE" });
                ddlProgram.AddItemToolTip();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (ddlLookupTypeDesc.SelectedIndex != null)
            ddlLookupTypeDesc.SelectedIndex = -1;
        ddlLookupTypeDesc.ClearDropDownList();
        ddlLookupTypeDesc.ClearSelection();

        txtLookupName.Text = string.Empty;
        gvLookupMaster.DataSource = null;
        gvLookupMaster.DataBind();
        pnlDetail.Visible = false;
        btnSave.Enabled = false;
        ViewState["LookupMaster"] = null;
        if (ddlModule.SelectedIndex != null)
        {
            ddlModule.SelectedIndex = 0;
        }
        if (ddlProgram.SelectedIndex != null)
        {
            ddlProgram.SelectedIndex = 0;
            ddlProgram.ClearDropDownList();
        }
    }

    //protected void btnCancel_Click(object sender, EventArgs e)
    //{
    //    string strRedirectPageAdd = "window.location.href='../Common/HomePage.aspx';";
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShWFMsg", strRedirectPageAdd, true);
    //}
    #endregion


    protected void ddlLookupTypeDesc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLookupTypeDesc.SelectedIndex > 0)
        {
            Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Lookup_Def_ID", ddlLookupTypeDesc.SelectedValue.ToString());
            Procparam.Add("@Lookup_Desc", ddlLookupTypeDesc.SelectedItem.Text.ToString());
            Procparam.Add("@Created_By", intUserID.ToString());

            dtManualLookup = Utility.GetDefaultData("S3G_Get_Manual_Lookup", Procparam);
            if (dtManualLookup != null)
            {
                pnlDetail.Visible = true;
                btnClear.Enabled = true;
                    //btnSave.Enabled = true;
                gvLookupMaster.DataSource = dtManualLookup;
                gvLookupMaster.DataBind();
                ViewState["LookupMaster"] = dtManualLookup;
            }
        }
        else
        {
            pnlDetail.Visible = false;
            btnClear.Enabled = true;
            btnSave.Enabled = false;
            txtLookupName.Text = string.Empty;
            ViewState["LookupMaster"] = null;
            gvLookupMaster.DataSource = null;
            gvLookupMaster.DataBind();
        }


    }

    protected void gvLookupMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Button btnUpdate = (Button)sender;
            //GridViewRow grvRow = (GridViewRow)btnUpdate.Parent.Parent;
            Label lblID = (Label)e.Row.FindControl("lblID");
            Label lblLookupCategory = (Label)e.Row.FindControl("lblLookupCategory");
            TextBox txtLookupName = (TextBox)e.Row.FindControl("txtLookupName");
            Label lblLookupDesc = (Label)e.Row.FindControl("lblLookupDesc");
            Label lblTableName = (Label)e.Row.FindControl("lblTableName");
            Button btnupd = (Button)e.Row.FindControl("btnUpdate");
            if (!bModify || lblID.Text.ToString() == string.Empty)
            {
                btnupd.Enabled = false;
            }
            else
            {
                btnupd.Enabled = true;
            }
        }
    }

    public void btnUpdate_Click(object sender, EventArgs e)
    {

        dtManualLookupModify = (DataTable)ViewState["LookupMaster"];
        Button btnUpdate = (Button)sender;
        GridViewRow grvRow = (GridViewRow)btnUpdate.Parent.Parent;
        Label lblID = (Label)grvRow.FindControl("lblID");
        Label lblLookupCategory = (Label)grvRow.FindControl("lblLookupCategory");
        TextBox txtLookupName = (TextBox)grvRow.FindControl("txtLookupName");
        Label lblLookupDesc = (Label)grvRow.FindControl("lblLookupDesc");
        Label lblTableName = (Label)grvRow.FindControl("lblTableName");

        if (dtManualLookupModify.Rows.Count > 0)
        {
            if (lblID.Text.ToString() == string.Empty)
            {
                Utility.FunShowAlertMsg(this, "Save the Lookup details, Then Update it");
                return;
            }
        }
        if (txtLookupName.Text.ToString()==string.Empty)
        {
            Utility.FunShowAlertMsg(this, "Lookup Value cannot be empty.");
            txtLookupName.Focus();
            return;
        }

        try
        {
            objManualLookupClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            ObjS3G_SYS_ManualLookupDataTable = new S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ManualLookupDataTable();
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ManualLookupRow ObjManualLookupRow;
            ObjManualLookupRow = ObjS3G_SYS_ManualLookupDataTable.NewS3G_SYSAD_ManualLookupRow();
            ObjManualLookupRow.Company_ID = intCompanyID; // 1
            ObjManualLookupRow.ID = Convert.ToInt32(lblID.Text.ToString());
            ObjManualLookupRow.Lookup_Type = lblLookupCategory.Text;
            ObjManualLookupRow.Lookup_Name = txtLookupName.Text;
            ObjManualLookupRow.Table_Name = lblTableName.Text;
            ObjManualLookupRow.Created_By = intUserID; // 2
            //ObjManualLookupRow.Modified_By = intUserID;
            //ObjManualLookupRow.Lookup_Type = txtLookupName.Text;
            //ObjManualLookupRow.Table_Name = "";

            ObjS3G_SYS_ManualLookupDataTable.AddS3G_SYSAD_ManualLookupRow(ObjManualLookupRow);
            SerializationMode SerMode = SerializationMode.Binary;

            //objManualLookupClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            intErrCode = objManualLookupClient.FunPubModifyManualLookup(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYS_ManualLookupDataTable, SerMode));

            if (intErrCode == 0)
            {
                //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                Utility.FunShowAlertMsg(this, "Lookup details updated successfully");


                //strRedirectPageView = "";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;

            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Lookup Name already exist");
            }
            lblErrorMessage.Text = string.Empty;

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objManualLookupClient.Close();
        }
    }

    protected void imgbtnAdd_Click(object sender, EventArgs e)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Lookup_Def_ID", ddlLookupTypeDesc.SelectedValue.ToString());
        Procparam.Add("@Lookup_Desc", txtLookupName.Text.Trim());
        DataTable dtCheck = new DataTable();
        dtCheck = Utility.GetDefaultData("S3G_Chk_Manual_Lookup_Exist", Procparam);

        if ((Convert.ToInt32(dtCheck.Rows[0][0]) > 0))
        {
            Utility.FunShowAlertMsg(this.Page, "Lookup Value already exist");
        }
        else
        {
            dtManualLookup = new DataTable();
            dtManualLookup = (DataTable)ViewState["LookupMaster"];
            DataTable dtNewLookup = new DataTable();
            dtNewLookup.Columns.Add("Name");
            dtNewLookup.Columns.Add("Lookup_Type");
           
            if (ViewState["NewLookup"] != null)
                dtNewLookup = (DataTable)ViewState["NewLookup"];
           
            if (txtLookupName.Text.Trim().ToString() == string.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, "Lookup Value cannot be empty");
                return;
            }

            foreach (DataRow dataRow in dtNewLookup.Rows)
            {
                if (dataRow["Name"].ToString() == txtLookupName.Text.Trim())
                {
                    Utility.FunShowAlertMsg(this.Page, "Lookup Value already exist");
                    return;
                }
            }           

            DataRow row = dtNewLookup.NewRow();
            row["Name"] = txtLookupName.Text.Trim();
            row["Lookup_Type"] = ddlRCUType.SelectedValue;
            dtNewLookup.Rows.InsertAt(row, 0);
            ViewState["NewLookup"] = dtNewLookup;

            row = dtManualLookup.NewRow();
            row["Name"] = txtLookupName.Text.Trim();
            row["Lookup_Type"] = ddlRCUType.SelectedValue;
            dtManualLookup.Rows.InsertAt(row, 0);

            gvLookupMaster.DataSource = ViewState["LookupMaster"] = dtManualLookup;
            gvLookupMaster.DataBind();
            txtLookupName.Text = string.Empty;            
            rfvLookupName.Enabled = false;
            btnSave.Enabled = true;
        }

    }

    protected void ddlRCUType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLookupTypeDesc.SelectedIndex > 0 && ddlRCUType.SelectedIndex > 0)
        {
            Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Lookup_Def_ID", ddlLookupTypeDesc.SelectedValue.ToString());
            Procparam.Add("@Lookup_Desc", ddlLookupTypeDesc.SelectedItem.Text.ToString());
            Procparam.Add("@RCUType", ddlRCUType.SelectedValue.ToString());
            Procparam.Add("@Created_By", intUserID.ToString());
            dtManualLookup = Utility.GetDefaultData("S3G_Get_Manual_Lookup", Procparam);
        }

        gvLookupMaster.DataSource = dtManualLookup;
        gvLookupMaster.DataBind();
        ViewState["LookupMaster"] = dtManualLookup;

    }


    protected void gvLookupMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
}