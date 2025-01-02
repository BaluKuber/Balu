/// Module Name     :   Origination
/// Screen Name     :   S3GOrgAuthorizationRuleCard_Add
/// Created By      :   Ramesh M
/// Created Date    :   28-May-2010
/// Purpose         :   To insert and update Authorization Rule card details
///
/// Modified By     :   Thangam M
/// Date            :   17-sep-2010 
/// Purpose         :   Bug Fixation
///
/// 


#region Namespaces
using System;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
#endregion

public partial class Origination_S3GOrgAuthorizationRuleCard_Add : ApplyThemeForProject
{
    #region Initialization
    int intErrCode = 0;
    int intAuthorizationId = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    int intRowcnt = 0;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    //Code end 
    static string strPageName = "Authorization Rule Card";
    string strRedirectPage = "../Origination/S3GOrgAuthorizationRuleCard_View.aspx";
    DataSet DsAuthorizatioRulecard = new DataSet();
    StringBuilder strRulecardDetail;
    UserInfo ObjUserInfo = new UserInfo();
    DataTable dtRuleCardDetails = new DataTable();
    SerializationMode SerMode = SerializationMode.Binary;
    RuleCardMgtServicesReference.RuleCardMgtServicesClient ObjAuthorizationRuleCardClient;
    CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable ObjS3G_LOBMasterListDataTable = new CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable();
    S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable ObjS3G_ProductMaster_ViewDataTable_DAL = new CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable();
    UserMgtServices.S3G_SYSAD_ProgramMasterDataTable ObjS3G_ProgramMaster_DataTable = new UserMgtServices.S3G_SYSAD_ProgramMasterDataTable();
    S3GBusEntity.Origination.RuleCardMgtServices.S3G_Status_LookUPDataTable ObjS3G_TransactionType_DataTable = new S3GBusEntity.Origination.RuleCardMgtServices.S3G_Status_LookUPDataTable();
    S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ConstitutionMasterDataTable ObjS3G_Constitution_DataTable = new S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ConstitutionMasterDataTable();
    S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable ObjS3G_AuthorizationRuleCard_DataTable = new S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable();

    public static Origination_S3GOrgAuthorizationRuleCard_Add obj_Page;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            bClearList = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("ClearListValues"));

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            if (Request.QueryString["qsARC_ID"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsARC_ID"));
                intAuthorizationId = Convert.ToInt32(fromTicket.Name);
                strMode = Request.QueryString.Get("qsMode").ToString();
            }
            if (!IsPostBack)
            {
                //Added to load the footer control initially.. (For auto suggest)
                DataTable dtTable = FunProInitializePopup("0");
                grvApprover.DataSource = dtTable;
                grvApprover.DataBind();
                dtTable.Rows[0].Delete();
                grvApprover.Rows[0].Visible = false;

                if (intAuthorizationId > 0)
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunPubBindMasterDetails();
                    btnClear.Enabled = false;
                }
                else
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    FunPubBindMasterDetails();
                    ChkActive.Enabled = false;
                }
                //User Authorization

                if ((intAuthorizationId > 0) && (strMode == "M"))
                {
                    FunPriDisableControls(1);

                }
                else if ((intAuthorizationId > 0) && (strMode == "Q")) // Query // Modify
                {
                    FunPriDisableControls(-1);

                }
                else
                {
                    FunPriDisableControls(0);
                }

                //Code end
            }
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    #region Page Methods

    //This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }

                    TextBox txtFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
                    txtFromAmount.Text = "1";
                    txtFromAmount.Enabled = false;

                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    btnClear.Enabled = false;

                    DisableMasterControls();
                    hdnGrvCnt.Value = grvAuthorizationrulecardDetail.Rows.Count.ToString();
                    for (int i = 0; i <= grvAuthorizationrulecardDetail.Rows.Count - 1; i++)
                    {
                        LinkButton btnRemove = (LinkButton)grvAuthorizationrulecardDetail.Rows[i].FindControl("btnRemove");
                        TextBox ToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtToAmount");
                        TextBox FromAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtFromAmount");
                        //TextBox Totalapproval = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txttotalapproval");
                        //TextBox Level4approvals = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtlevel4approvals");
                        //code is commented by saran for UAT Fix on 25-Apr-2012 start
                            // btnRemove.Enabled = false;
                        //code is commented by saran for UAT Fix on 25-Apr-2012 end

                        //if (ToAmount.Text != null)
                        //{
                        //    FromAmount.ReadOnly = true;
                        //    Totalapproval.ReadOnly = true;
                        //    Level4approvals.ReadOnly = true;
                        //    ToAmount.ReadOnly = true;
                        //}
                        //Totalapproval.ReadOnly = true;
                    }

                    // Modified By : Thangam M ---



                    //---

                    break;

                case -1:// Query Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }

                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    ChkActive.Enabled = false;

                    if (bClearList)
                    {

                        if (ddlLOB.Items.Count > 0)
                            ddlLOB.ClearDropDownList();
                        if (ddlConstitution.Items.Count > 0)
                            ddlConstitution.ClearDropDownList();
                        //if (ddlproduct.Items.Count > 0)
                        //    ddlproduct.ClearDropDownList();
                        if (ddlprogram.Items.Count > 0)
                            ddlprogram.ClearDropDownList();
                        // ddlTransacType.ClearDropDownList();
                    }

                    // Modified By : Thangam M ---

                    DisableMasterControls();

                    for (int i = 0; i <= grvAuthorizationrulecardDetail.Rows.Count - 1; i++)
                    {
                        TextBox FromAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtFromAmount");
                        TextBox ToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtToAmount");
                        //TextBox Totalapproval = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txttotalapproval");
                        //TextBox Level4approvals = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtlevel4approvals");                        
                        LinkButton Remove = (LinkButton)grvAuthorizationrulecardDetail.Rows[i].FindControl("btnRemove");

                        FromAmount.ReadOnly = ToAmount.ReadOnly 
                            //= Totalapproval.ReadOnly 
                            //= Level4approvals.ReadOnly 
                            = true;
                        Remove.Enabled = false;
                    }

                    ChkActive.Enabled = false;
                    btnDEVModalAdd.Enabled = false;
                    //---
                    //Code added to make Remove colunm visible false in query Mode
                    grvAuthorizationrulecardDetail.FooterRow.Visible = false;
                    grvAuthorizationrulecardDetail.Columns[4].Visible = false;                    
                    //Made Remove colunm visible false in query Mode ends here

                    break;
            }
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }

    }

    // Added By : Thangam M ---

    public void DisableMasterControls()
    {
        try
        {
            ddlLOB.Enabled = false;
            ddlConstitution.Enabled = false;
            ddlproduct.Enabled = false;
            ddlTransacType.Enabled = false;
            ddlprogram.Enabled = false;
            ddlEntityType.Enabled = false;
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    //-----

    //Code end
    public void FunPubBindMasterDetails()
    {
        ObjAuthorizationRuleCardClient = new RuleCardMgtServicesReference.RuleCardMgtServicesClient();
        try
        {
            if (intAuthorizationId == 0)
            {
                DataRow dr;
                dtRuleCardDetails.Columns.Add("SNo");
                dtRuleCardDetails.Columns.Add("From_Amount");
                dtRuleCardDetails.Columns.Add("To_Amount");
                //dtRuleCardDetails.Columns.Add("Total_Approvals");
                //dtRuleCardDetails.Columns.Add("Level4_Approvals");
                //// Code modified for Oracle Table Column name change - Kuppusamy.B - 22-Feb-2012
                //dtRuleCardDetails.Columns.Add("Authorization_Rule_Card_Detail_ID");
                dtRuleCardDetails.Columns.Add("AUTH_RULE_CARD_DET_ID");
                dtRuleCardDetails.Columns.Add("RowStatus");

                dr = dtRuleCardDetails.NewRow();
                dr["SNo"] = 0;
                dtRuleCardDetails.Rows.Add(dr);
                ViewState["RuleCardDetail"] = dtRuleCardDetails;
                grvAuthorizationrulecardDetail.DataSource = dtRuleCardDetails;
                grvAuthorizationrulecardDetail.DataBind();
                grvAuthorizationrulecardDetail.Rows[0].Visible = false;
                ViewState["RuleCardDetail"] = dtRuleCardDetails;

                //Bind LOB
                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@User_Id", Convert.ToString(intUserID));
                Procparam.Add("@Is_Active", "1");
                //@Program_ID - Parameter added for Usermgt 
                Procparam.Add("@Program_ID", "23");

                ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
                ddlLOB.AddItemToolTip();

                Dictionary<string, string> Procparam4 = new Dictionary<string, string>();
                Procparam4.Add("@PROGRM_ID", "0");
                ddlprogram.BindDataTable("S3G_Get_ProgramMasterForORG_Details", Procparam4, new string[] { "PROGRAM_ID", "PROGRAM_CODE" });
                ddlprogram.AddItemToolTip();

                Dictionary<string, string> Procparam1 = new Dictionary<string, string>();
                Procparam1.Add("@TransactionType", "Transaction_Type");
                ddlTransacType.BindDataTable("S3G_ORG_GetTransactionType", Procparam1, new string[] { "ID", "Name" });
            }

            //ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID","LOB_NAME" });


            //Program
            //ObjProgramClient = new UserMgtServicesReference.UserMgtServicesClient();
            //UserMgtServices.S3G_SYSAD_ProgramMasterRow ObjProgramMasterRow;
            //ObjProgramMasterRow = ObjS3G_ProgramMaster_DataTable.NewS3G_SYSAD_ProgramMasterRow();
            //ObjProgramMasterRow.Program_ID = 0;
            //ObjS3G_ProgramMaster_DataTable.AddS3G_SYSAD_ProgramMasterRow(ObjProgramMasterRow);
            //byte[] bytesProgramMasterDetails = ObjProgramClient.FunPubQueryProgramMasterList(SerMode, ClsPubSerialize.Serialize(ObjS3G_ProgramMaster_DataTable, SerMode), "");
            //ObjS3G_ProgramMaster_DataTable = (UserMgtServices.S3G_SYSAD_ProgramMasterDataTable)ClsPubSerialize.DeSerialize(bytesProgramMasterDetails, SerializationMode.Binary, typeof(UserMgtServices.S3G_SYSAD_ProgramMasterDataTable));
            //ddlprogram.FillDataTable(ObjS3G_ProgramMaster_DataTable, ObjS3G_ProgramMaster_DataTable.Program_IDColumn.ColumnName, ObjS3G_ProgramMaster_DataTable.Program_CodeColumn.ColumnName);
            //ObjProgramClient.Close();

            

            if (intAuthorizationId > 0)
            {
                S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardRow ObjRuleCardRow;
                ObjRuleCardRow = ObjS3G_AuthorizationRuleCard_DataTable.NewS3G_ORG_AuthorizationRuleCardRow();
                ObjRuleCardRow.Company_ID = intCompanyID;
                ObjRuleCardRow.Authorization_Rule_Card_ID = intAuthorizationId;
                ObjS3G_AuthorizationRuleCard_DataTable.AddS3G_ORG_AuthorizationRuleCardRow(ObjRuleCardRow);
                SerializationMode Sermode = SerializationMode.Binary;
                //ObjAuthorizationRuleCardClient = new RuleCardMgtServicesReference.RuleCardMgtServicesClient();
                byte[] bytesAuthorizationRuleCard = ObjAuthorizationRuleCardClient.FunPubQueryAuthorizationRuleCard(Sermode, ClsPubSerialize.Serialize(ObjS3G_AuthorizationRuleCard_DataTable, Sermode));
                DsAuthorizatioRulecard = (DataSet)ClsPubSerialize.DeSerialize(bytesAuthorizationRuleCard, SerializationMode.Binary, typeof(DataSet));
                if (DsAuthorizatioRulecard.Tables[0].Rows.Count > 0)
                {
                    //Bind Product Based On LOB

                    //objCompanyServicesClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
                    //CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewRow ObjProductRow;
                    //ObjProductRow = ObjS3G_ProductMaster_ViewDataTable_DAL.NewS3G_SYSAD_ProductMaster_ViewRow();
                    //ObjProductRow.Is_Active = true;
                    //ObjProductRow.LOB_ID = Convert.ToInt32(DsAuthorizatioRulecard.Tables[0].Rows[0]["LOB_ID"].ToString());
                    //ObjProductRow.Company_ID = intCompanyID;
                    //ObjS3G_ProductMaster_ViewDataTable_DAL.AddS3G_SYSAD_ProductMaster_ViewRow(ObjProductRow);
                    //byte[] bytesProductDetails = objCompanyServicesClient.FunPubQueryProduct(SerMode, ClsPubSerialize.Serialize(ObjS3G_ProductMaster_ViewDataTable_DAL, SerMode));
                    //ObjS3G_ProductMaster_ViewDataTable_DAL = (CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable)ClsPubSerialize.DeSerialize(bytesProductDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable));
                    //ddlproduct.FillDataTable(ObjS3G_ProductMaster_ViewDataTable_DAL, ObjS3G_ProductMaster_ViewDataTable_DAL.Product_IDColumn.ColumnName, ObjS3G_ProductMaster_ViewDataTable_DAL.Product_Code_NameColumn.ColumnName);
                    //objCompanyServicesClient.Close();
                    //Constitution Code
                    //Procparam = new Dictionary<string, string>();
                    //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    //Procparam.Add("@LOB_ID", DsAuthorizatioRulecard.Tables[0].Rows[0]["LOB_ID"].ToString().Trim());
                    //Procparam.Add("@Is_Active", "true");
                    //ddlConstitution.BindDataTable("S3G_ORG_GetConstitutionMasterforLOB", Procparam, new string[] { "Constitution_ID", "ConstitutionName" });
                    ddlLOB.Items.Clear();
                    if (!string.IsNullOrEmpty(DsAuthorizatioRulecard.Tables[0].Rows[0]["LOB_ID"].ToString().Trim()))
                    {
                        ddlLOB.FillDataTable(DsAuthorizatioRulecard.Tables[0], "LOB_ID", "LOB_Code_Name");
                        ddlLOB.SelectedValue = DsAuthorizatioRulecard.Tables[0].Rows[0]["LOB_ID"].ToString().Trim();
                    }
                    ddlConstitution.Items.Clear();
                    ddlConstitution.FillDataTable(DsAuthorizatioRulecard.Tables[0], "Constitution_ID", "Constitution_Code_Name");
                    if (!string.IsNullOrEmpty(DsAuthorizatioRulecard.Tables[0].Rows[0]["Constitution_ID"].ToString().Trim()))
                    {
                        ddlConstitution.SelectedValue = DsAuthorizatioRulecard.Tables[0].Rows[0]["Constitution_ID"].ToString().Trim();
                    }

                    //ddlproduct.Items.Clear();
                    //ddlproduct.FillDataTable(DsAuthorizatioRulecard.Tables[0], "Product_ID", "Product_Code_Name");
                    //if (!string.IsNullOrEmpty(DsAuthorizatioRulecard.Tables[0].Rows[0]["Product_ID"].ToString()))
                    //{
                    //    ddlproduct.SelectedValue = DsAuthorizatioRulecard.Tables[0].Rows[0]["Product_ID"].ToString();
                    //}
                    
                    // ddlTransacType.SelectedValue = DsAuthorizatioRulecard.Tables[0].Rows[0]["Transaction_Type_ID"].ToString().Trim();

                    ListItem lst = new ListItem(DsAuthorizatioRulecard.Tables[0].Rows[0]["Program_Code_Name"].ToString().Trim(), DsAuthorizatioRulecard.Tables[0].Rows[0]["Program_Id"].ToString().Trim());
                    ddlprogram.Items.Add(lst);
                    ddlprogram.SelectedValue = DsAuthorizatioRulecard.Tables[0].Rows[0]["Program_Id"].ToString().Trim();
                    ddlEntityType.SelectedValue = DsAuthorizatioRulecard.Tables[0].Rows[0]["Entity_Type_ID"].ToString().Trim();
                    ChkActive.Checked = DsAuthorizatioRulecard.Tables[0].Rows[0]["Is_Active"].ToString().Trim() == "True" ? true : false;
                    if (DsAuthorizatioRulecard != null)
                    {
                        if (DsAuthorizatioRulecard.Tables[1].Rows.Count > 0)
                        {
                            dtRuleCardDetails = DsAuthorizatioRulecard.Tables[1];
                            grvAuthorizationrulecardDetail.DataSource = dtRuleCardDetails;
                            grvAuthorizationrulecardDetail.DataBind();
                            ViewState["RuleCardDetail"] = dtRuleCardDetails;
                            btnReset.Visible = false;
                        }

                        if (DsAuthorizatioRulecard.Tables[2].Rows.Count > 0)
                        {
                            ViewState["dtTempAuthApprover"] = DsAuthorizatioRulecard.Tables[2];
                        }
                    }
                }
                //ObjAuthorizationRuleCardClient.Close();
            }
        }
        catch (FaultException<RuleCardMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void AddToolTipForcontrols()
    {
        ddlLOB.AddItemToolTip();
        ddlConstitution.AddItemToolTip();
        //ddlproduct.AddItemToolTip();
        ddlprogram.AddItemToolTip();
    }


    #endregion

    #region Page Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjAuthorizationRuleCardClient = new RuleCardMgtServicesReference.RuleCardMgtServicesClient();
        try
        {
            Session["ToAmount"] = null;
            strRulecardDetail = new StringBuilder();
            if (intAuthorizationId > 0)
            {
                decimal FromAmount = 0;
                decimal ToAmount = 0;
                if (grvAuthorizationrulecardDetail.Rows.Count <= 1)
                {
                    if (grvAuthorizationrulecardDetail.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Add atleast one Authorization Rule Card detail');", true);
                        return;
                    }
                    TextBox txtToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[0].FindControl("txtToAmount");
                    if (String.IsNullOrEmpty(txtToAmount.Text.Trim()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Add atleast one Authorization Rule Card detail');", true);
                        txtToAmount.Focus();
                        return;
                    }
                }
                strRulecardDetail.Append("<Root>");
                foreach (GridViewRow grvRulecard in grvAuthorizationrulecardDetail.Rows)
                {
                    Label lblRuleCardDetailID = (Label)grvRulecard.FindControl("lblRulecardDetailID");
                    TextBox txtAddFromAmount = (TextBox)grvRulecard.FindControl("txtFromAmount");
                    if (txtAddFromAmount.Text.Trim() == string.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter From Amount');", true);
                        txtAddFromAmount.Focus();
                        return;

                    }
                    if (Session["ToAmount"] != null)
                    {
                        ToAmount = Convert.ToDecimal(Session["ToAmount"]) + Convert.ToInt32("1");
                        FromAmount = Convert.ToDecimal(txtAddFromAmount.Text.Trim());
                        if (ToAmount != FromAmount)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From/To Amount should not overlap');", true);
                            txtAddFromAmount.Text = "";
                            txtAddFromAmount.Focus();
                            return;
                        }
                    }
                    TextBox txtAddToAmount = (TextBox)grvRulecard.FindControl("txtToAmount");
                    if (txtAddToAmount.Text.Trim() == string.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter To Amount');", true);
                        txtAddToAmount.Focus();
                        return;

                    }
                    //TextBox txtAddtotalapprovals = (TextBox)grvRulecard.FindControl("txttotalapproval");

                    //if (txtAddtotalapprovals.Text.Trim() == string.Empty)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Total Approval(s)');", true);
                    //    txtAddtotalapprovals.Focus();
                    //    return;

                    //}
                    //else if (txtAddtotalapprovals.Text.Trim() == "0")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter  valid Total Approval(s)');", true);
                    //    txtAddtotalapprovals.Focus();
                    //    return;
                    //}
                    //TextBox txtlevel4approvals = (TextBox)grvRulecard.FindControl("txtlevel4approvals");

                    //if (txtlevel4approvals.Text.Trim() == string.Empty)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Level 4 Approval(s)');", true);
                    //    txtlevel4approvals.Focus();
                    //    return;

                    //}
                    //else if (txtlevel4approvals.Text.Trim() == "0")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter valid Level 4 Approval(s)');", true);
                    //    txtlevel4approvals.Focus();
                    //    return;
                    //}


               //     TextBox txtAddlevel4approvals = (TextBox)grvRulecard.FindControl("txtlevel4approvals");
               //     if ((!(string.IsNullOrEmpty(txtAddtotalapprovals.Text.Trim())))
               //&&
               //(!(string.IsNullOrEmpty(txtAddlevel4approvals.Text.Trim()))))
               //     {
               //         if (Convert.ToInt32(txtAddlevel4approvals.Text.Trim()) >
               //             Convert.ToInt32(txtAddtotalapprovals.Text.Trim()))
               //         {
               //             ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Level 4/5 Approvals should not be greater than Total Approvals');", true);
               //             txtAddlevel4approvals.Focus();
               //             return;
               //         }
               //     }
                    //if (txtAddlevel4approvals.Text.Trim() == string.Empty)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Level4 Approvals');", true);
                    //    txtAddlevel4approvals.Focus();
                    //    return;

                    //}
                    //Label lblEntityName = (Label)grvRulecard.FindControl("lblEntityType");
                    Label lblSNo = (Label)grvRulecard.FindControl("lblSNo");
                    strRulecardDetail.Append(" <RulecardDetail From_Amount='" + txtAddFromAmount.Text.Trim() + "'");
                    strRulecardDetail.Append("  SNo='" + lblSNo.Text.Trim() + "'");
                    //// Code modified for Oracle Table Column name change - Kuppusamy.B - 22-Feb-2012
                    //strRulecardDetail.Append("  Authorization_Rule_Card_Detail_ID='" + lblRuleCardDetailID.Text.Trim() + "'");
                    strRulecardDetail.Append("  AUTH_RULE_CARD_DET_ID='" + lblRuleCardDetailID.Text.Trim() + "'");
                    
                    strRulecardDetail.Append("  To_Amount='" + txtAddToAmount.Text.Trim() + "'/>");
                    //strRulecardDetail.Append("  Total_Approvals='" + txtAddtotalapprovals.Text.Trim() + "'");
                    //strRulecardDetail.Append("  Entity_Name='" + ((string.IsNullOrEmpty(lblEntityName.Text.Trim())) ? "" : lblEntityName.Text.Trim()) + "'/>");

                    FromAmount = Convert.ToDecimal(txtAddFromAmount.Text.Trim());
                    ToAmount = Convert.ToDecimal(txtAddToAmount.Text.Trim());

                    //--- Modified By     M ---

                    if (FromAmount == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Amount cannot be Zero');", true);
                        txtAddFromAmount.Focus();
                        return;
                    }

                    //---

                    if (FromAmount >= ToAmount)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To Amount Should be greater than From Amount');", true);
                        txtAddToAmount.Text = "";
                        txtAddToAmount.Focus();
                        return;
                    }
                    else
                        Session["ToAmount"] = ToAmount.ToString();

                }
                strRulecardDetail.Append("</Root>");
            }
            else
            {
                decimal FromAmount = 0;
                decimal ToAmount = 0;
                if (grvAuthorizationrulecardDetail.Rows.Count <= 1)
                {
                    if (grvAuthorizationrulecardDetail.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Add atleast one Authorization Rule Card detail');", true);
                        return;
                    }
                    TextBox txtToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[0].FindControl("txtToAmount");
                    if (String.IsNullOrEmpty(txtToAmount.Text.Trim()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert1", "alert('Add atleast one Authorization Rule Card detail');", true);
                        txtToAmount.Focus();
                        return;
                    }
                }
                strRulecardDetail.Append("<Root>");
                for (int i = 0; i < grvAuthorizationrulecardDetail.Rows.Count; i++)
                {
                    TextBox txtAddFromAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtFromAmount");
                    if (txtAddFromAmount.Text.Trim() == string.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter From Amount');", true);
                        txtAddFromAmount.Focus();
                        return;
                    }
                    if (Session["ToAmount"] != null)
                    {
                        ToAmount = Convert.ToDecimal(Session["ToAmount"]) + Convert.ToInt32("1");
                        FromAmount = Convert.ToDecimal(txtAddFromAmount.Text.Trim());
                        if (ToAmount != FromAmount)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From/To Amount should not overlap');", true);
                            txtAddFromAmount.Focus();
                            return;
                        }
                    }
                    TextBox txtAddToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtToAmount");
                    if (txtAddToAmount.Text.Trim() == string.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter To Amount');", true);
                        txtAddToAmount.Focus();
                        return;

                    }
                    //TextBox txtAddtotalapprovals = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txttotalapproval");
                    //if (txtAddtotalapprovals.Text.Trim() == string.Empty)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Total Approval(s)');", true);
                    //    txtAddtotalapprovals.Focus();
                    //    return;

                    //}
                    //else if (txtAddtotalapprovals.Text.Trim() == "0")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter valid Total Approval(s)');", true);
                    //    txtAddtotalapprovals.Focus();
                    //    return;
                    //}
                    //TextBox txtAddlevel4approvals = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtlevel4approvals");

               //     if ((!(string.IsNullOrEmpty(txtAddtotalapprovals.Text.Trim())))
               //&&
               //(!(string.IsNullOrEmpty(txtAddlevel4approvals.Text.Trim()))))
               //     {
               //         if (Convert.ToInt32(txtAddlevel4approvals.Text.Trim()) >
               //             Convert.ToInt32(txtAddtotalapprovals.Text.Trim()))
               //         {
               //             ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Level 4/5 Approvals should not be greater than Total Approvals');", true);
               //             txtAddlevel4approvals.Focus();
               //             return;
               //         }
               //     }

                    //if (txtAddlevel4approvals.Text.Trim() == string.Empty)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Level 4 Approval(s)');", true);
                    //    txtAddlevel4approvals.Focus();
                    //    return;

                    //}
                    //else if (txtAddlevel4approvals.Text.Trim() == "0")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter valid Level 4 Approval(s)');", true);
                    //    txtAddlevel4approvals.Focus();
                    //    return;
                    //}
                    //Label lblEntityName = (Label)grvAuthorizationrulecardDetail.Rows[i].FindControl("lblEntityType");
                    Label lblSNo = (Label)grvAuthorizationrulecardDetail.Rows[i].FindControl("lblSNo");
                    strRulecardDetail.Append(" <RulecardDetail From_Amount='" + txtAddFromAmount.Text.Trim() + "'");
                    strRulecardDetail.Append("  SNo='" + lblSNo.Text.Trim() + "'");
                    strRulecardDetail.Append("  To_Amount='" + txtAddToAmount.Text.Trim() + "'/>");
                    //strRulecardDetail.Append("  Total_Approvals='" + txtAddtotalapprovals.Text.Trim() + "'");
                    ////strRulecardDetail.Append("  Level4_Approvals='" + ((string.IsNullOrEmpty(txtAddlevel4approvals.Text.Trim())) ? "" : txtAddlevel4approvals.Text.Trim()) + "'/>");
                    //strRulecardDetail.Append("  Entity_Name='" + ((string.IsNullOrEmpty(lblEntityName.Text.Trim())) ? "" : lblEntityName.Text.Trim()) + "'/>");

                    FromAmount = Convert.ToDecimal(txtAddFromAmount.Text.Trim());
                    ToAmount = Convert.ToDecimal(txtAddToAmount.Text.Trim());
                    if (FromAmount >= ToAmount)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Should be greater than From Amount');", true);
                        txtAddToAmount.Focus();
                        return;
                    }
                    else
                        Session["ToAmount"] = ToAmount.ToString();
                }
                strRulecardDetail.Append("</Root>");
            }
            if (ddlLOB.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Line of Business');", true);
                return;
            }
            //if (ddlTransacType.SelectedIndex == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Transaction Type');", true);
            //    return;
            //}
            //if (ddlprogram.SelectedValue== "0" || ddlproduct.SelectedValue=="" )
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Program');", true);
            //    return;
            //}
            //ObjAuthorizationRuleCardClient = new RuleCardMgtServicesReference.RuleCardMgtServicesClient();
            S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardRow ObjAuthorizationRuleCardRow;
            ObjAuthorizationRuleCardRow = ObjS3G_AuthorizationRuleCard_DataTable.NewS3G_ORG_AuthorizationRuleCardRow();
            ObjAuthorizationRuleCardRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedItem.Value.Trim());
            if (ddlConstitution != null && ddlConstitution.SelectedIndex > 0)
                ObjAuthorizationRuleCardRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedItem.Value.Trim());
            else
                ObjAuthorizationRuleCardRow.Constitution_ID = 0;
            ObjAuthorizationRuleCardRow.Company_ID = intCompanyID;
            ObjAuthorizationRuleCardRow.Authorization_Rule_Card_ID = intAuthorizationId;
            //if (ddlproduct != null && ddlproduct.SelectedIndex > 0)
            //    ObjAuthorizationRuleCardRow.Product_ID = Convert.ToInt32(ddlproduct.SelectedItem.Value.Trim().ToString());
            //else
                ObjAuthorizationRuleCardRow.Product_ID = 0;


            ObjAuthorizationRuleCardRow.Transaction_Type = 0;
            ObjAuthorizationRuleCardRow.Program_Id = Convert.ToInt32(ddlprogram.SelectedItem.Value.Trim());
            ObjAuthorizationRuleCardRow.XMLRulecardDetail = strRulecardDetail.ToString();
            ObjAuthorizationRuleCardRow.XMLRuleCardApprovals = ((DataTable)ViewState["dtTempAuthApprover"]).FunPubFormXml();
            ObjAuthorizationRuleCardRow.Is_Active = ChkActive.Checked ? true : false;
            ObjAuthorizationRuleCardRow.Created_By = intUserID;
            ObjAuthorizationRuleCardRow.Modified_By = intUserID;
            ObjAuthorizationRuleCardRow.Entity_Type = ddlEntityType.SelectedValue;
            ObjS3G_AuthorizationRuleCard_DataTable.AddS3G_ORG_AuthorizationRuleCardRow(ObjAuthorizationRuleCardRow);
            SerializationMode SerMode = SerializationMode.Binary;
            if (intAuthorizationId > 0)
                intErrCode = ObjAuthorizationRuleCardClient.FunPubModifyAuthorizationRuleCard(SerMode, ClsPubSerialize.Serialize(ObjS3G_AuthorizationRuleCard_DataTable, SerMode));
            else
                intErrCode = ObjAuthorizationRuleCardClient.FunPubCreateAuthorizationRuleCard(SerMode, ClsPubSerialize.Serialize(ObjS3G_AuthorizationRuleCard_DataTable, SerMode));
            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intAuthorizationId > 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorization Rule Card details updated successfully.');window.location.href='../Origination/S3GOrgAuthorizationRuleCard_View.aspx';", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorization Rule Card details added successfully.');window.location.href='../Origination/S3GOrgAuthorizationRuleCard_View.aspx';", true);

            }
            else if (intErrCode == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('This combination already exist');", true);
                return;
            }
            else if (intErrCode == 20)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Unable to save the Authorization Rule Card details');", true);
                return;
            }
                //Code Added by saran on 25-Apr-2012 for UAT Fix start 
            else if (intErrCode == 25)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorize all pending transactions before changing the rule card.');", true);
                return;
            }
            //Code Added by saran on 25-Apr-2012 for UAT Fix end 


        }
        catch (FaultException<RuleCardMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjAuthorizationRuleCardClient.Close();
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Origination/S3GOrgAuthorizationRuleCard_View.aspx");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedIndex > 0)
            {
                //bind Constitution
                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);//changed 29/11/2010 ramesh
                Procparam.Add("@Is_Active", "true");
                //Commented by saranya on 14-Feb-2012 to remove the Constitution Code
                //ddlConstitution.BindDataTable("S3G_ORG_GetConstitutionMasterforLOB", Procparam, new string[] { "Constitution_ID", "ConstitutionName" });
                ddlConstitution.BindDataTable("S3G_ORG_GetConstitutionMasterforLOB", Procparam, new string[] { "Constitution_ID", "Constitution_Name" });
                ddlConstitution.AddItemToolTip();

                ////Bind Product 
                //Dictionary<string, string> Procparam1 = new Dictionary<string, string>();
                //Procparam1.Add("@LOB_ID", ddlLOB.SelectedValue);
                //Procparam1.Add("@Company_ID", Convert.ToString(intCompanyID));
                //Procparam1.Add("@Is_Active", "true");//changed 29/11/2010 ramesh
                ////Commented by saranya on 14-Feb-2012 to remove the Product Code
                ////ddlproduct.BindDataTable(SPNames.SYS_ProductMaster, Procparam1, new string[] { "Product_ID", "Product_Code", "Product_Name" });
                //ddlproduct.BindDataTable(SPNames.SYS_ProductMaster, Procparam1, new string[] { "Product_ID","Product_Name" });
                //ddlproduct.AddItemToolTip();

                //lblErrorMessage.Text = dtRuleCardDetails.Rows.Count.ToString();
            }
            if (ddlLOB.SelectedIndex == 0)
            {
                ddlConstitution.SelectedIndex = 0;
                ddlproduct.SelectedIndex = 0;
            }

            AddToolTipForcontrols();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvAuthorizationrulecardDetail_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                if (dtRuleCardDetails.Rows.Count > 0 && intAuthorizationId > 0)
                {
                    TextBox txtAddFromAmount = (TextBox)e.Row.FindControl("txtAddFromAmount");
                    txtAddFromAmount.Text = dtRuleCardDetails.Rows[dtRuleCardDetails.Rows.Count - 1]["To_Amount"].ToString();
                    if (!string.IsNullOrEmpty(txtAddFromAmount.Text))
                        txtAddFromAmount.Text = (Convert.ToDecimal(txtAddFromAmount.Text) + 1).ToString();
                }

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtFromAmount = (TextBox)e.Row.FindControl("txtFromAmount");
                if (intRowcnt == 0)
                    txtFromAmount.Enabled = false;
                else
                    txtFromAmount.Enabled = true;
                intRowcnt++;
                //// LinkButton btnRemove = (LinkButton)e.Row.FindControl("btnRemove");
                //// btnRemove.Enabled = true;
                // if (dtRuleCardDetails.Rows.Count > 0 && intAuthorizationId > 0)
                // {
                //     TextBox txtAddFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
                //     TextBox txtToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[dtRuleCardDetails.Rows.Count-1].FindControl("txtToAmount");
                //     txtAddFromAmount.Text = (Convert.ToDouble(txtToAmount.Text.Trim()) + 1).ToString();
                // }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (dtRuleCardDetails.Rows.Count != 0)
                {
                    if (dtRuleCardDetails.Rows[0]["From_Amount"].ToString() == "")
                    {
                        dtRuleCardDetails = (DataTable)ViewState["RuleCardDetail"];
                        dtRuleCardDetails.Rows[0].Delete();
                        ViewState["RuleCardDetail"] = dtRuleCardDetails;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRowStatus = (Label)e.Row.FindControl("lblRowStatus");
                TextBox txtFromAmnt = (TextBox)e.Row.FindControl("txtFromAmount");
                TextBox txtToAmnt = (TextBox)e.Row.FindControl("txtToAmount");
                //TextBox txtTotApp = (TextBox)e.Row.FindControl("txttotalapproval");
                //TextBox txtlev4app = (TextBox)e.Row.FindControl("txtlevel4approvals");

                //Code commented by saran on 25-Apr-2012 start
               /* if (lblRowStatus.Text == "O")
                {
                   txtFromAmnt.ReadOnly = true;
                   txtToAmnt.ReadOnly = true;
                   txtTotApp.ReadOnly = true;
                   txtlev4app.ReadOnly = true;
                }
                else
                {
                    txtFromAmnt.ReadOnly = false;
                    txtToAmnt.ReadOnly = false;
                    txtTotApp.ReadOnly = false;
                    txtlev4app.ReadOnly = false;
                }*/
                //Code commented by saran on 25-Apr-2012 end
            }            
            
            AddToolTipForcontrols();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvAuthorizationrulecardDetail_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                Label lbl = (Label)grvAuthorizationrulecardDetail.Rows[grvAuthorizationrulecardDetail.Rows.Count - 1].FindControl("lblSNo");
                int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
                TextBox txtAddFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
                TextBox txtAddToAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddToAmount");
                //TextBox txtAddtotalapprovals = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddtotalapprovals");
                //DropDownList ddlEntityType = (DropDownList)grvAuthorizationrulecardDetail.FooterRow.FindControl("ddlEntityType");
                //TextBox txtAddlevel4approvals = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddlevel4approvals");
                
                //if ((!(string.IsNullOrEmpty(txtAddtotalapprovals.Text)))
                //    &&
                //    (!(string.IsNullOrEmpty(txtAddlevel4approvals.Text))))
                //{
                //    if (Convert.ToInt32(txtAddlevel4approvals.Text.Trim()) >
                //        Convert.ToInt32(txtAddtotalapprovals.Text.Trim()))
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Level 4/5 Approvals should not be greater than Total Approvals');", true);
                //        txtAddlevel4approvals.Focus();
                //        return;
                //    }
                //}
                if (txtAddFromAmount.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter From amount');", true);
                    txtAddFromAmount.Focus();
                    return;

                }
                if (txtAddToAmount.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter To amount');", true);
                    txtAddToAmount.Focus();
                    return;

                }
                //else if (txtAddtotalapprovals.Text.Trim() == string.Empty)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Total Approval(s)');", true);
                //    txtAddtotalapprovals.Focus();
                //    return;

                //}
                //else if (txtAddtotalapprovals.Text.Trim() == "0")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter valid Total Approval(s)');", true);
                //    txtAddtotalapprovals.Focus();
                //    return;
                //}
                if (grvAuthorizationrulecardDetail.Rows.Count == 1 && ViewState["dtTempAuthApprover"] == null)
                {
                    Utility.FunShowAlertMsg(this, "Add Atleast one Approver");
                    return;
                }
                if (((DataTable)(ViewState["dtTempAuthApprover"])) != null)
                {
                    DataRow[] dArray = ((DataTable)(ViewState["dtTempAuthApprover"])).Select("SNo='" + intSNo + "' and Location<>'' and ApprovPerson<>''");
                    if (dArray.Length ==0)
                    {
                        Utility.FunShowAlertMsg(this, "Add Atleast one Approver");
                        return;
                    }
                }       
                //else if (txtAddlevel4approvals.Text.Trim() == string.Empty)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Level 4 Approval(s)');", true);
                //    txtAddlevel4approvals.Focus();
                //    return;

                //}
                ////Edited by saran based on the Test cases TC_114 (1077)
                //else if (txtAddlevel4approvals.Text.Trim() == "0")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter valid Level 4 Approval(s)');", true);
                //    txtAddlevel4approvals.Focus();
                //    return;

                //}
                
                    decimal dcToAmount = 0;
                    TextBox txtToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[grvAuthorizationrulecardDetail.Rows.Count - 1].FindControl("txtToAmount");
                    if (!string.IsNullOrEmpty(txtToAmount.Text.Trim()))
                    {
                        dcToAmount = Convert.ToDecimal(txtToAmount.Text.Trim()) + 1;
                        if (dcToAmount != Convert.ToDecimal(txtAddFromAmount.Text.Trim()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From/To Amount should not overlap');", true);
                            txtAddFromAmount.Focus();
                            return;
                        }
                    }

                    //if (ViewState["RuleCardDetail"] != null && (((DataTable)ViewState["RuleCardDetail"]).Rows.Count==1 || ((DataTable)ViewState["RuleCardDetail"]).Rows.Count>1))
                    //{
                    //    string strEntityName = ((DataTable)ViewState["RuleCardDetail"]).Rows[0]["Level4_Approvals"].ToString();
                    //    if (ddlEntityType.SelectedItem.Text != strEntityName)
                    //    {
                    //        Utility.FunShowAlertMsg(this, "Entity type should be same for all the approvals");
                    //        return;
                    //    }
                    //}

                    dtRuleCardDetails = (DataTable)ViewState["RuleCardDetail"];
                    dr = dtRuleCardDetails.NewRow();
                    dr["SNo"] = Convert.ToInt32(lbl.Text) + 1;
                    dr["From_Amount"] = txtAddFromAmount.Text.Trim();
                    dr["To_Amount"] = txtAddToAmount.Text.Trim();
                    //dr["Total_Approvals"] = txtAddtotalapprovals.Text.Trim();
                    //if (!string.IsNullOrEmpty(txtAddlevel4approvals.Text.Trim()))
                    //{
                    //    dr["Level4_Approvals"] = txtAddlevel4approvals.Text.Trim();
                    //}
                    //else
                    //{
                    //dr["Level4_Approvals"] = ddlEntityType.SelectedItem.Text;
                    //}
                    dtRuleCardDetails.Rows.Add(dr);
                    grvAuthorizationrulecardDetail.DataSource = dtRuleCardDetails;
                    grvAuthorizationrulecardDetail.DataBind();
                    ViewState["RuleCardDetail"] = dtRuleCardDetails;
                    TextBox txtaddFromAmt1 = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
                    txtaddFromAmt1.Text = Convert.ToString(Convert.ToDecimal(txtAddToAmount.Text.Trim()) + Convert.ToInt32("1"));

                    if ((intAuthorizationId > 0) && (strMode == "M"))
                    {
                        for (int i = 0; i <= grvAuthorizationrulecardDetail.Rows.Count - 1; i++)
                        {
                            Label lblSNo = (Label)grvAuthorizationrulecardDetail.Rows[i].FindControl("lblSNo");
                            TextBox fromval = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtFromAmount");
                            TextBox totval = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtToAmount");
                            //TextBox totApp = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txttotalapproval");
                            //TextBox levApp = (TextBox)grvAuthorizationrulecardDetail.Rows[i].FindControl("txtlevel4approvals");
                            LinkButton btnRemove = (LinkButton)grvAuthorizationrulecardDetail.Rows[i].FindControl("btnRemove");

                            if (i < Convert.ToInt32(hdnGrvCnt.Value))
                            {
                                btnRemove.Enabled = false;
                            }
                            else
                            {
                                btnRemove.Enabled = true;
                            }
                        }
                    }
                    if (strMode.Trim() == "")
                    {
                        for (int i = 0; i <= grvAuthorizationrulecardDetail.Rows.Count - 1; i++)
                        {
                            LinkButton btnRemove = (LinkButton)grvAuthorizationrulecardDetail.Rows[i].FindControl("btnRemove");
                            if (i < Convert.ToInt32(grvAuthorizationrulecardDetail.Rows.Count - 1))
                                btnRemove.Enabled = false;
                            else
                                btnRemove.Enabled = true;
                        }
                    }
                    ViewState["dtTempAuthApproverPopUp"] = null;
            }
            AddToolTipForcontrols();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            dtRuleCardDetails.Clear();
            ViewState["RuleCardDetail"] = null;
            DataRow dr;
            dtRuleCardDetails.Columns.Add("From_Amount");
            dtRuleCardDetails.Columns.Add("To_Amount");
            //dtRuleCardDetails.Columns.Add("Total_Approvals");
            //dtRuleCardDetails.Columns.Add("Level4_Approvals");

            //// Code modified for Oracle Table Column name change - Kuppusamy.B - 22-Feb-2012
            //dtRuleCardDetails.Columns.Add("Authorization_Rule_Card_Detail_ID");
            dtRuleCardDetails.Columns.Add("AUTH_RULE_CARD_DET_ID");

            dtRuleCardDetails.Columns.Add("RowStatus");

            dr = dtRuleCardDetails.NewRow();
            dtRuleCardDetails.Rows.Add(dr);
            ViewState["RuleCardDetail"] = dtRuleCardDetails;
            grvAuthorizationrulecardDetail.DataSource = dtRuleCardDetails;
            grvAuthorizationrulecardDetail.DataBind();
            grvAuthorizationrulecardDetail.Rows[0].Visible = false;
            ViewState["RuleCardDetail"] = dtRuleCardDetails;

            TextBox txtAddFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
            TextBox txtAddToAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddToAmount");
            //TextBox txtAddtotalapprovals = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddtotalapprovals");
            //TextBox txtAddlevel4approvals = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddlevel4approvals");
            txtAddFromAmount.Text = string.Empty;
            txtAddToAmount.Text = string.Empty;
            //txtAddtotalapprovals.Text = string.Empty;
            //txtAddlevel4approvals.Text = string.Empty;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = 0;
            ddlConstitution.SelectedIndex = 0;
            //ddlproduct.SelectedIndex = 0;
            //ddlTransacType.SelectedIndex = 0;
            ddlprogram.SelectedIndex = 0;
            ddlEntityType.SelectedIndex = 0;
            dtRuleCardDetails.Clear();
            ViewState["RuleCardDetail"] = null;
            ViewState["dtTempAuthApprover"] = null;
            ViewState["dtTempAuthApproverPopUp"] = null;
            DataRow dr;
            dtRuleCardDetails.Columns.Add("SNo");
            dtRuleCardDetails.Columns.Add("From_Amount");
            dtRuleCardDetails.Columns.Add("To_Amount");
            //dtRuleCardDetails.Columns.Add("Total_Approvals");
            //dtRuleCardDetails.Columns.Add("Level4_Approvals");

            //// Code modified for Oracle Table Column name change - Kuppusamy.B - 22-Feb-2012
            //dtRuleCardDetails.Columns.Add("Authorization_Rule_Card_Detail_ID");
            dtRuleCardDetails.Columns.Add("AUTH_RULE_CARD_DET_ID");

            dtRuleCardDetails.Columns.Add("RowStatus");

            dr = dtRuleCardDetails.NewRow();
            dr["SNo"] = 0;
            dtRuleCardDetails.Rows.Add(dr);
            ViewState["RuleCardDetail"] = dtRuleCardDetails;
            grvAuthorizationrulecardDetail.DataSource = dtRuleCardDetails;
            grvAuthorizationrulecardDetail.DataBind();
            grvAuthorizationrulecardDetail.Rows[0].Visible = false;
            ViewState["RuleCardDetail"] = dtRuleCardDetails;

            TextBox txtAddFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
            txtAddFromAmount.Text = "1";
            txtAddFromAmount.Enabled = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvAuthorizationrulecardDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["RuleCardDetail"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            Label lblSNo = (Label)grvAuthorizationrulecardDetail.Rows[e.RowIndex].FindControl("lblSNo");
            DataRow[] dArr = ((DataTable)ViewState["dtTempAuthApprover"]).Select("SNo='" + lblSNo.Text + "'");
            if (dArr.Length > 0)
            {
                foreach (DataRow dr in dArr)
                {
                    ((DataTable)ViewState["dtTempAuthApprover"]).Rows.Remove(dr);
                }
            }
            if (dtDelete.Rows.Count <= 0)
            {
                dtDelete = null;
                dtRuleCardDetails.Clear();
                ViewState["RuleCardDetail"] = null;
                DataRow dr;
                dtRuleCardDetails.Columns.Add("SNo");
                dtRuleCardDetails.Columns.Add("From_Amount");
                dtRuleCardDetails.Columns.Add("To_Amount");
                //dtRuleCardDetails.Columns.Add("Total_Approvals");
                //dtRuleCardDetails.Columns.Add("Level4_Approvals");

                //// Code modified for Oracle Table Column name change - Kuppusamy.B - 22-Feb-2012
                //dtRuleCardDetails.Columns.Add("Authorization_Rule_Card_Detail_ID");
                dtRuleCardDetails.Columns.Add("AUTH_RULE_CARD_DET_ID");
                
                dtRuleCardDetails.Columns.Add("RowStatus");

                dr = dtRuleCardDetails.NewRow();
                dr["SNo"] = 0;
                dtRuleCardDetails.Rows.Add(dr);
                ViewState["RuleCardDetail"] = dtRuleCardDetails;
                grvAuthorizationrulecardDetail.DataSource = dtRuleCardDetails;
                grvAuthorizationrulecardDetail.DataBind();
                grvAuthorizationrulecardDetail.Rows[0].Visible = false;
                ViewState["RuleCardDetail"] = dtRuleCardDetails;

                TextBox txtFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
                txtFromAmount.Text = "1";
                txtFromAmount.Enabled = false;

                return;
            }




            grvAuthorizationrulecardDetail.DataSource = dtDelete;
            grvAuthorizationrulecardDetail.DataBind();
            if (strMode.Trim() == "M")
            {
                for (int i = 0; i <= grvAuthorizationrulecardDetail.Rows.Count - 1; i++)
                {
                    LinkButton btnRemove = (LinkButton)grvAuthorizationrulecardDetail.Rows[i].FindControl("btnRemove");
                    if (i < Convert.ToInt32(hdnGrvCnt.Value))
                    {
                        btnRemove.Enabled = false;
                        TextBox txtAddFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
                        TextBox txtToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[grvAuthorizationrulecardDetail.Rows.Count - 1].FindControl("txtToAmount");
                        txtAddFromAmount.Text = (Convert.ToDouble(txtToAmount.Text.Trim()) + 1).ToString();
                    }
                    else
                    {
                        btnRemove.Enabled = true;

                        TextBox txtAddFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
                        TextBox txtToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[grvAuthorizationrulecardDetail.Rows.Count - 1].FindControl("txtToAmount");
                        txtAddFromAmount.Text = (Convert.ToDouble(txtToAmount.Text.Trim()) + 1).ToString();
                    }
                }
            }
            else
            {
                for (int i = 0; i <= grvAuthorizationrulecardDetail.Rows.Count - 1; i++)
                {
                    LinkButton btnRemove = (LinkButton)grvAuthorizationrulecardDetail.Rows[i].FindControl("btnRemove");
                    if (i < Convert.ToInt32(grvAuthorizationrulecardDetail.Rows.Count - 1))
                    {
                        btnRemove.Enabled = false;
                    }
                    else
                    {
                        btnRemove.Enabled = true;

                        TextBox txtAddFromAmount = (TextBox)grvAuthorizationrulecardDetail.FooterRow.FindControl("txtAddFromAmount");
                        TextBox txtToAmount = (TextBox)grvAuthorizationrulecardDetail.Rows[grvAuthorizationrulecardDetail.Rows.Count - 1].FindControl("txtToAmount");
                        txtAddFromAmount.Text = (Convert.ToDouble(txtToAmount.Text.Trim()) + 1).ToString();
                    }
                }
            }
            ViewState["RuleCardDetail"] = dtDelete;
            AddToolTipForcontrols();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }   
    

    protected void grvApprover_DataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");
            UserControls_S3GAutoSuggest ddlLocation = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlLocation");
            DropDownList ddlApprovPerson = (DropDownList)e.Row.FindControl("ddlApprovPerson");
            //RequiredFieldValidator rfvLocation = (RequiredFieldValidator)e.Row.FindControl("rfvLocation");
            //RequiredFieldValidator rfvApprovalPerson = (RequiredFieldValidator)e.Row.FindControl("rfvApprovalPerson");

            if (ddlEntityType.SelectedValue == "1" || ddlEntityType.SelectedValue == "2")
            {
                grvApprover.Columns[2].Visible = false;
                //rfvLocation.Enabled = false;
                ddlLocation.IsMandatory = false;
                grvApprover.Columns[2].FooterStyle.Width = 0;
                //ddlLocation.Width = 0;
               
                //rfvApprovalPerson.Enabled = false;
            }
            else
            {
                grvApprover.Columns[2].Visible = true;
                //rfvLocation.Enabled = true;
               
                ddlLocation.IsMandatory = true;
                PnlApprover.Width = 500;
                //rfvApprovalPerson.Enabled = true;
            }
            //FunPriBindLocation(ddlLocation); 
            //procparam = new Dictionary<string, string>();
            //procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //if (PageMode == PageModes.Create)
            //{
            //    procparam.Add("@Is_Active", "1");
            //}
            //procparam.Add("@User_ID", intUserID.ToString());
            //procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
            //procparam.Add("@Program_ID", ddlprogram.SelectedValue);
            //ddlLocation.BindDataTable(SPNames.BranchMaster_LIST, procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //ddlLocation.AddItemToolTip();
            //ddlLocation.ToolTip = ddlLocation.SelectedItem.Text;

            //procparam = new Dictionary<string, string>();

            //procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //procparam.Add("@Entity_Type_ID",ddlEntityType.SelectedValue);            
            //ddlApprovPerson.BindDataTable("S3G_ORG_GetEntityType",procparam,new string[] { "ID", "Name" });
            FunPriInitializeFooterControls(e);            
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
            if (strMode == "Q")
            {
                btnDelete.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = true;
            }
        }
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", obj_Page.ddlprogram.SelectedValue);
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    private void FunPriInitializeFooterControls(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Dictionary<string, string> procparam;
            //DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");
            UserControls_S3GAutoSuggest ddlLocation = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlLocation");
            DropDownList ddlApprovPerson = (DropDownList)e.Row.FindControl("ddlApprovPerson");
            if (ddlEntityType.SelectedValue == "3")//If User
            {
                //procparam = new Dictionary<string, string>();
                //procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                //if (PageMode == PageModes.Create)
                //{
                //    procparam.Add("@Is_Active", "1");
                //}
                //procparam.Add("@User_ID", intUserID.ToString());
                //procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
                //procparam.Add("@Program_ID", ddlprogram.SelectedValue);
                //ddlLocation.BindDataTable(SPNames.BranchMaster_LIST, procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
                //ddlLocation.AddItemToolTip();
                //ddlLocation.ToolTip = ddlLocation.SelectedItem.Text;
            }
            else
            {
                FunsetApproverPerson(ddlLocation, ddlApprovPerson);
            }

        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserControls_S3GAutoSuggest ddlLoc = (UserControls_S3GAutoSuggest)grvApprover.FooterRow.FindControl("ddlLocation");
        DropDownList ddlApprovPerson = (DropDownList)grvApprover.FooterRow.FindControl("ddlApprovPerson");

        FunsetApproverPerson(ddlLoc, ddlApprovPerson);
    }

    private void FunsetApproverPerson(UserControls_S3GAutoSuggest ddlLoc, DropDownList ddlAPPPer)
    {
        if (PageMode != PageModes.Query)
        {
            Dictionary<string, string> procparam = new Dictionary<string, string>();
            if (ViewState["dtTempAuthApproverPopUp"] != null && ((DataTable)ViewState["dtTempAuthApproverPopUp"]).Rows.Count >= 1)
            {
                int intEntityID = Convert.ToInt32(((DataTable)ViewState["dtTempAuthApproverPopUp"]).Rows[(((DataTable)ViewState["dtTempAuthApproverPopUp"])).Rows.Count - 1]["ApprovPersonID"]);
                if (ddlEntityType.SelectedValue == "1")
                    procparam.Add("@Entity_ID", Convert.ToString(intEntityID));
            }
            procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            procparam.Add("@Entity_Type_ID", ddlEntityType.SelectedValue);
            if (ddlLoc.SelectedValue != "0")
                procparam.Add("@Location_Id", ddlLoc.SelectedValue);
            procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
            ddlAPPPer.BindDataTable("S3G_ORG_GetEntityType", procparam, new string[] { "ID", "Name" });
        }
    }

    protected DataTable FunProInitializePopup(string SNo)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns.Add("SequenceNumber");           
        dt.Columns.Add("ApprovPerson");
        dt.Columns.Add("ApprovPersonID");
        dt.Columns.Add("Location");
        dt.Columns.Add("LocationID"); 

        DataRow dRow = dt.NewRow();
        dRow["SNo"] = SNo;
        dRow["SequenceNumber"] = 0;
        dRow["ApprovPerson"] = string.Empty;
        dRow["ApprovPersonID"] = 0;
        dRow["LocationID"] = 0;
        dRow["Location"] = string.Empty;

        dt.Rows.Add(dRow);

        return dt;
        //grvApprover.DataSource = dt;
        //grvApprover.DataBind();
        //grvApprover.Rows[0].Visible = false;
        //dt.Rows.RemoveAt(0);
        //ViewState["dtTempAuthApprover"] = dt;        
    }

    protected void btnDEVModalCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderApprover.Hide();
    }

    protected void btnFApprover_Click(object sender, EventArgs e)
    {       
        DataTable dt = new DataTable();
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;               
        string strSLNo = "";        
        Label lblSNo = (Label)grvAuthorizationrulecardDetail.Rows[grvAuthorizationrulecardDetail.Rows.Count - 1].FindControl("lblSNo");
        strSLNo = (Convert.ToInt32(lblSNo.Text) + 1).ToString();
        if (ViewState["dtTempAuthApprover"] == null)
        {
            dt = FunProInitializePopup(strSLNo);
            grvApprover.DataSource = dt;            
            grvApprover.DataBind();
            grvApprover.Rows[0].Visible = false;
            dt.Rows.RemoveAt(0);
            ViewState["dtTempAuthApproverPopUp"] = ViewState["dtTempAuthApprover"] = dt;                      
        }
        else
        {
            DataTable dt1 = (DataTable)ViewState["dtTempAuthApprover"];
            DataTable dtApprove = new DataTable();
            DataRow[] dRow = dt1.Select("SNo ='" + strSLNo + "'");
            if (dRow.Length > 0)
            {
                dtApprove = dt1.Clone();
                ViewState["dtTempAuthApproverPopUp"] = dtApprove = dt1.Select("SNo ='" + strSLNo + "'").CopyToDataTable();
                grvApprover.DataSource = dtApprove;
                grvApprover.DataBind();
            }
            else
            {                
                dt1 = FunProInitializePopup(strSLNo);
                grvApprover.DataSource = dt1;
                grvApprover.DataBind();
                grvApprover.Rows[0].Visible = false;
                dt1.Rows.RemoveAt(0);
                ViewState["dtTempAuthApproverPopUp"] = dt1;
            }
        }       
        ModalPopupExtenderApprover.Show();
    }

    protected void btnIApprover_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable dtApprove = new DataTable();
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
        GridView grv = (GridView)gvRow.Parent.Parent;
        dt = (DataTable)ViewState["dtTempAuthApprover"];
        //RequiredFieldValidator rfvLocation = (RequiredFieldValidator)grvApprover.FooterRow.FindControl("rfvLocation");
        //RequiredFieldValidator rfvApprovalPerson = (RequiredFieldValidator)grvApprover.FooterRow.FindControl("rfvApprovalPerson");
        //if (ddlEntityType.SelectedValue == "1" || ddlEntityType.SelectedValue == "2")
        //{
        //    grvApprover.Columns[2].Visible = false;
        //    rfvLocation.Enabled = false;
        //    rfvApprovalPerson.Enabled = false;
        //}
        //else
        //{
        //    grvApprover.Columns[2].Visible = true;
        //    rfvLocation.Enabled = true;
        //    rfvApprovalPerson.Enabled = true;
        //}
        Label lblSNo = (Label)gvRow.FindControl("lblSNo");
        int intSNo = Convert.ToInt32(lblSNo.Text);
        DataRow[] drAuthApprover = dt.Select("SNo='" + intSNo + "'");
        if (drAuthApprover.Length > 0)
        {
            ViewState["dtTempAuthApproverPopUp"] = dtApprove = dt.Select("SNo='" + intSNo + "'").CopyToDataTable();
        }
        grvApprover.DataSource = dtApprove;
        grvApprover.DataBind();
        if (strMode == "Q")
        {
            grvApprover.FooterRow.Visible = false;
        }
        ModalPopupExtenderApprover.Show();        
    }

    //protected void ddlApprovalEntity_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DropDownList ddlApprovalEntity = (DropDownList)sender;
    //    int intRowIndex = Utility.FunPubGetGridRowID("grvApprover", ((DropDownList)sender).ClientID.ToString());
    //    DropDownList ddlApprovPerson = (DropDownList)grvApprover.Rows[intRowIndex].FindControl("ddlApprovPerson"); 
    //    Dictionary<string, string> procparam = new Dictionary<string, string>();
    //    procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    procparam.Add("@Entity_Type_ID", ddlApprovalEntity.SelectedValue);
    //    ddlApprovPerson.BindDataTable("S3G_ORG_GetEntityType", procparam, new string[] { "ID", "Name" });
    //}

    protected void btnDEVModalAdd_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        DataTable dtModal = new DataTable();
        DataTable dtMain = new DataTable();
        dtModal = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        if (dtModal.Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this, "Add Atlest on Approval Details");
            return;
        }
        //foreach (GridViewRow GvRow in grvApprover.Rows)
        //{
        //    DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
        //    DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
        //    Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");
        //    //if (ddlApprovalEntity.Items.Count > 1 && ddlApprovalEntity.SelectedValue == "0")
        //    //{
        //    //    Utility.FunShowAlertMsg(this, "Select Approval Entity");
        //    //    ddlApprovalEntity.Focus();
        //    //    return;
        //    //}
        //    if (ddlApprovPerson.Items.Count > 1 && ddlApprovPerson.SelectedValue == "0")
        //    {
        //        Utility.FunShowAlertMsg(this, "Select Approval Authority");
        //        ddlApprovPerson.Focus();
        //        return;
        //    }
        //    //string strApprovalEntity = ddlApprovalEntity.SelectedValue;
        //    //string strApprovalPerson = ddlApprovPerson.SelectedValue;
        //    //for (int inti = 0; inti <= grvApprover.Rows.Count-1; inti++)
        //    //{
        //    //    Label lblSequenceNumber1 = (Label)grvApprover.Rows[inti].FindControl("lblSequenceNumber");
        //    //    DropDownList ddlApprovalEntity1 = (DropDownList)grvApprover.Rows[inti].FindControl("ddlApprovalEntity");
        //    //    DropDownList ddlApprovPerson1 = (DropDownList)grvApprover.Rows[inti].FindControl("ddlApprovPerson");
        //    //    if (lblSequenceNumber.Text != lblSequenceNumber1.Text && ddlApprovalEntity1.SelectedValue == strApprovalEntity && ddlApprovPerson1.SelectedValue == strApprovalPerson)
        //    //    {
        //    //        Utility.FunShowAlertMsg(this, "Selected combination already exist");
        //    //        ddlApprovPerson1.Focus();
        //    //        return;
        //    //    }
        //    //}
        //}
        dtMain = (DataTable)ViewState["dtTempAuthApprover"];

        int intMasterSeq = Convert.ToInt32(((Label)grvApprover.Rows[0].FindControl("LblSNo")).Text);
        /*Write Coding To delete Existing record*/

        DataRow[] Drow = dtMain.Select("SNo='" + intMasterSeq + "'");
        foreach (DataRow dr in Drow)
        {
            dtMain.Rows.Remove(dr);
        }

        foreach (GridViewRow GvRow in grvApprover.Rows)
        {
            Label lblSNo=(Label)GvRow.FindControl("lblSNo");            
            Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");
            Label lblLocation = (Label)GvRow.FindControl("lblLocation");
            Label lblLocationID = (Label)GvRow.FindControl("lblLocationID");
            Label lblApprovalPerson = (Label)GvRow.FindControl("lblApprovalPerson");
            Label ApprovPersonID = (Label)GvRow.FindControl("ApprovPersonID");
            DataRow dRow = dtMain.NewRow();

            dRow["SNo"] = intMasterSeq;
            dRow["SequenceNumber"] = lblSequenceNumber.Text;
            dRow["ApprovPerson"] = lblApprovalPerson.Text;
            dRow["ApprovPersonID"] = ApprovPersonID.Text;
            dRow["LocationID"] = lblLocationID.Text;
            dRow["Location"] = lblLocation.Text;
            dtMain.Rows.Add(dRow);
        }
        ViewState["dtTempAuthApprover"] = dtMain;
        ViewState["dtTempAuthApproverPopUp"] = null;
        ModalPopupExtenderApprover.Hide();
    }

    //protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DropDownList ddlEntityType = (DropDownList)sender;
    //    if (ViewState["dtTempAuthApprover"] != null)
    //    {
    //        int intRowCount = ((DataTable)ViewState["dtTempAuthApprover"]).Rows.Count;
            
    //        //ViewState["dtTempAuthApprover"] = FunProInitializePopup();
    //        DataRow[] dArr = ((DataTable)ViewState["dtTempAuthApprover"]).Select("SNo='" + intRowCount + 1 + "'");
    //        if (dArr.Length > 0)
    //        {
    //            foreach (DataRow dr in dArr)
    //            {
    //                ((DataTable)ViewState["dtTempAuthApprover"]).Rows.Remove(dr);
    //                ((DataTable)ViewState["dtTempAuthApprover"]).AcceptChanges();
    //            }
    //        }
    //    }
    //}


    protected void btnDetails_Click(object sender, EventArgs e)
    {
        Label lblSequenceNumber = (Label)grvApprover.FooterRow.FindControl("lblSequenceNumber");
        //DropDownList ddlLocation = (DropDownList)grvApprover.FooterRow.FindControl("ddlLocation");
        UserControls_S3GAutoSuggest ddlLocation = (UserControls_S3GAutoSuggest)grvApprover.FooterRow.FindControl("ddlLocation");
        DropDownList ddlApprovPerson = (DropDownList)grvApprover.FooterRow.FindControl("ddlApprovPerson");
        int intSNo=Convert.ToInt32(((Label)grvApprover.Rows[0].FindControl("LblSNo")).Text);
        DataTable dtModal = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        
        if (dtModal == null)
        {
            dtModal = (((DataTable)ViewState["dtTempAuthApprover"]).Select("SNo='" + intSNo + "'")).CopyToDataTable();
        }
        DataRow dRow = dtModal.NewRow();
        dRow["SNo"] = Convert.ToInt32(((Label)grvApprover.Rows[0].FindControl("LblSNo")).Text);
        if (ddlEntityType.SelectedValue == "3")
        {
            DataRow[] dArr = (dtModal.Select("SNo='" + intSNo + "' and LocationID='" + ddlLocation.SelectedValue + "' and ApprovPersonID='" + ddlApprovPerson.SelectedValue + "'"));
            if (dArr.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "The Selected user is already added for this location");
                return;
            }
        }
       
        dRow["ApprovPersonID"] = ddlApprovPerson.SelectedValue;
        dRow["LocationID"] = ddlLocation.SelectedValue;
        dRow["ApprovPerson"] = ddlApprovPerson.SelectedItem.Text;
        if (ddlLocation.SelectedValue != "0")
        {
            DataRow[] DRowArr = dtModal.Select("LocationID='" + ddlLocation.SelectedValue + "'");
            if (DRowArr.Length > 0)
            {
                dRow["SequenceNumber"] = DRowArr.Length + 1;
            }
            else
            {
                dRow["SequenceNumber"] = 1;
            }
            dRow["Location"] = Convert.ToString(ddlLocation.SelectedText);
        }
        else
        {
            if (dtModal.Rows.Count > 0)
            {
                dRow["SequenceNumber"] = dtModal.Rows.Count + 1;
            }
            else
            {
                dRow["SequenceNumber"] = 1;
            }
            dRow["Location"] = "0";
        }
        dtModal.Rows.Add(dRow);

        grvApprover.DataSource = dtModal;
        grvApprover.DataBind();
        ViewState["dtTempAuthApproverPopUp"] = dtModal;
    }
    

    protected void grvApprover_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtTable = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        Label lblSNo = (Label)grvApprover.Rows[e.RowIndex].FindControl("lblSNo");
        LinkButton btnDelete = (LinkButton)grvApprover.Rows[e.RowIndex].FindControl("btnDelete");
        int intRowIndex = Utility.FunPubGetGridRowID("grvApprover", btnDelete.ClientID.ToString());
        //DropDownList ddlApprovPerson = (DropDownList)grvApprover.Rows[intRowIndex].FindControl("ddlApprovPerson");
        //Label lblSequenceNumber = (Label)grvApprover.Rows[intRowIndex].FindControl("lblSequenceNumber");
        //Label lblSNo = (Label)grvApprover.Rows[intRowIndex].FindControl("lblSNo");
        Label lblLocationID = (Label)grvApprover.Rows[e.RowIndex].FindControl("lblLocationID");
        //DataRow[] DRowArr = dtTable.Select("SNo='" + lblSNo.Text + "' and LocationID='" + lblLocationID.Text + "' and SequenceNumber>'"+lblSequenceNumber.Text+"'");
        dtTable.Rows[e.RowIndex].Delete();
        DataRow[] dArr = dtTable.Select("LocationID='" + lblLocationID.Text + "'");
        if (dArr.Length > 0)
        {
            DataRow[] dt = dtTable.Select("LocationID='"+lblLocationID.Text+"'");
            int intSequenceNumber;
            intSequenceNumber = 1;
            foreach (DataRow dr in dt)
            {
                dr["SequenceNumber"] = intSequenceNumber;
                intSequenceNumber++;
            }                   
        }
        dtTable.AcceptChanges();
        if (dtTable.Rows.Count == 0)
        {
            dtTable = FunProInitializePopup(lblSNo.Text);
            grvApprover.DataSource = dtTable;
            grvApprover.DataBind();
            dtTable.Rows[0].Delete();
            ViewState["dtTempAuthApproverPopUp"] = dtTable;
            grvApprover.Rows[0].Visible = false;
        }
        else
        {
            ViewState["dtTempAuthApproverPopUp"] = dtTable;
            grvApprover.DataSource = dtTable;
            grvApprover.DataBind();
        }
        //if (DRowArr.Length > 0)
        //{
        //    foreach(DataRow dr in DRowArr)
        //    {
        //        int intSeq = Convert.ToInt32(lblSequenceNumber.Text);
        //        dtTable.Rows[
        //    }
        //}
    }

    #endregion
}
