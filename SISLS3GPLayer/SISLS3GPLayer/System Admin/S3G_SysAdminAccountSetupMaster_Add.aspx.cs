#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Account Setup Creation
/// Created By			: Kannan RC
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 12-July-2010   
/// Reason              : Add Role Access setup 
/// 
/// Last Updated By		: Gunasekar.K
/// Last Updated Date   : 15-OCt-2010   
/// Reason              : To address the bugs - 1752,1753 
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
using System.Text;
#endregion
public partial class System_Admin_S3G_SysAdminAccountSetupMaster_Add : ApplyThemeForProject
{

    #region Intialization
    CompanyMgtServicesReference.CompanyMgtServicesClient objProduct_MasterClient;
    AccountMgtServicesReference.AccountMgtServicesClient ObjAccountMasterClient;
    CompanyMgtServicesReference.CompanyMgtServicesClient objBranchList_MasterClient;
    int intErrCode = 0;
    int intCompanyID;
    int intUserID;
    bool bClearList = false;
    int AccountCodeId;
    int inthdUserid;
    int Count;
    string strRedirectPageMC;
    string qsTab;
    static string strMode;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx'";
    //string strRedirectPageAdd = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx'";

    //CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable ObjS3G_LOBMasterListDataTable = new CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable();
    //CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable ObjS3G_BranchList = new CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable();
    AccountMgtServices.S3G_SYSAD_AccountCodeDescDataTable ObjS3G_AccountDesc = new AccountMgtServices.S3G_SYSAD_AccountCodeDescDataTable();
    AccountMgtServices.S3G_SYSAD_AccountSetupMasterDataTable ObjS3G_AccountSetupDetails = new AccountMgtServices.S3G_SYSAD_AccountSetupMasterDataTable();
    AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable ObjS3G_AccountMasterDetails = new AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["qsAccount_Setup_ID"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsAccount_Setup_ID"));
                //AccountCodeId = Convert.ToInt32(fromTicket.Name);
                // strMode = Request.QueryString["qsMode"];
                if (fromTicket != null)
                {
                    AccountCodeId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }


            }
            if (Request.QueryString["qsMode"] != null)
            {
                strMode = Request.QueryString["qsMode"];
            }

            if (Request.QueryString["qsTab"] != null)
                qsTab = Request.QueryString["qsTab"];
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyID.ToString();
            System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = intUserID.ToString();
            System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = qsTab;



            //if (.SelectedValue != "")
            //{
            //    System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = ComboBoxLOBSearch.SelectedValue;
            //    System.Web.HttpContext.Current.Session["LOBAutoSuggestText"] = ComboBoxLOBSearch.SelectedItem.Text;
            //}

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            #region Tab
            if (qsTab != null)
            {
                switch (qsTab)
                {
                    case "Asset":
                        tbLib.Enabled = false;
                        tbIncome.Enabled = false;
                        tbExpress.Enabled = false;
                        FunsetIntegratedCtrls("1");
                        System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = 1;
                        if (!string.IsNullOrEmpty(hdnGLCodeSearchIDA.Value))
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = hdnGLCodeSearchIDA.Value;
                        else
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = null;
                        break;
                    case "Liability":
                        tbAsset.Enabled = false;
                        tbIncome.Enabled = false;
                        tbExpress.Enabled = false;
                        FunsetIntegratedCtrls("2");
                        System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = 2;
                        if (!string.IsNullOrEmpty(hdnGLCodeSearchIDL.Value))
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = hdnGLCodeSearchIDL.Value;
                        else
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = null;
                        break;
                    case "Income":
                        tbLib.Enabled = false;
                        tbAsset.Enabled = false;
                        tbExpress.Enabled = false;
                        FunsetIntegratedCtrls("3");
                        System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = 3;
                        if (!string.IsNullOrEmpty(hdnGLCodeSearchIDI.Value))
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = hdnGLCodeSearchIDI.Value;
                        else
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = null;
                        break;
                    case "Express":
                        tbAsset.Enabled = false;
                        tbIncome.Enabled = false;
                        tbLib.Enabled = false;
                        FunsetIntegratedCtrls("4");
                        System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = 4;
                        if (!string.IsNullOrEmpty(hdnGLCodeSearchIDE.Value))
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = hdnGLCodeSearchIDE.Value;
                        else
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = null;
                        break;
                }
            }
            else
            {
                switch (strMode)
                {
                    case "Asset":
                        tbLib.Enabled = false;
                        tbIncome.Enabled = false;
                        tbExpress.Enabled = false;
                        FunsetIntegratedCtrls("1");
                        System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = 1;
                        if (!string.IsNullOrEmpty(hdnGLCodeSearchIDA.Value))
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = hdnGLCodeSearchIDA.Value;
                        else
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = null;
                        break;
                    case "Liability":
                        tbAsset.Enabled = false;
                        tbIncome.Enabled = false;
                        tbExpress.Enabled = false;
                        FunsetIntegratedCtrls("2");
                        System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = 2;
                        if (!string.IsNullOrEmpty(hdnGLCodeSearchIDL.Value))
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = hdnGLCodeSearchIDL.Value;
                        else
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = null;
                        break;
                    case "Income":
                        tbLib.Enabled = false;
                        tbAsset.Enabled = false;
                        tbExpress.Enabled = false;
                        FunsetIntegratedCtrls("3");
                        System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = 3;
                        if (!string.IsNullOrEmpty(hdnGLCodeSearchIDI.Value))
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = hdnGLCodeSearchIDI.Value;
                        else
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = null;
                        break;
                    case "Express":
                        tbAsset.Enabled = false;
                        tbIncome.Enabled = false;
                        tbLib.Enabled = false;
                        FunsetIntegratedCtrls("4");
                        System.Web.HttpContext.Current.Session["AutoSuggestqsTab"] = 4;
                        if (!string.IsNullOrEmpty(hdnGLCodeSearchIDE.Value))
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = hdnGLCodeSearchIDE.Value;
                        else
                            System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] = null;
                        break;
                }
            }

            #endregion

            if (!IsPostBack)
            {
                FunPriGetBranchList();
                FunPriGetLOBList();
                FunPriGetAccountDesc(1, gvAsset);
                FunPriGetAccountDesc(2, gvLiability);
                FunPriGetAccountDesc(3, gvIncome);
                FunPriGetAccountDesc(4, gvExpress);

                #region Mode
                int intMode;

                /// <summary>
                /// This method is used to set Role access
                /// /// </summary>
                if (((AccountCodeId > 0)) && (strMode == "M"))
                {
                    intMode = 1;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunPriDisableControls(intMode, qsTab);

                }
                else if (((AccountCodeId > 0)) && (strMode == "Q"))
                {
                    intMode = -1;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FunPriDisableControls(intMode, qsTab);
                }
                else
                {
                    intMode = 0;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    FunPriDisableControls(intMode, qsTab);
                }
                #endregion
            }


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #region ddl bind
    /// <summary>
    /// Get dropdownlist data binded for Branch,LOB,
    /// </summary>
    private void FunPriGetBranchList()
    {
        try
        {
            //CompanyMgtServices.S3G_SYSAD_Branch_ListRow ObjBranchListRow;
            //ObjBranchListRow = ObjS3G_BranchList.NewS3G_SYSAD_Branch_ListRow();
            ////ObjBranchListRow.Region_Id = 1;
            //ObjBranchListRow.Company_ID = Convert.ToInt32(Session["Company_ID"]);
            //ObjS3G_BranchList.AddS3G_SYSAD_Branch_ListRow(ObjBranchListRow);
            //objBranchList_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] bytesBranchListDetails = objBranchList_MasterClient.FunPubBranch_List(SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchList, SerMode));
            ////ObjS3G_BranchList = (CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable)ClsPubSerialize.DeSerialize(bytesBranchListDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable));
            //ddlBranch.FillDataTable(ObjS3G_BranchList, ObjS3G_BranchList.Branch_IDColumn.ColumnName.Trim(), ObjS3G_BranchList.BranchColumn.ColumnName.Trim());
            //ddlLBranch.FillDataTable(ObjS3G_BranchList, ObjS3G_BranchList.Branch_IDColumn.ColumnName.Trim(), ObjS3G_BranchList.BranchColumn.ColumnName.Trim());
            //ddlIBranch.FillDataTable(ObjS3G_BranchList, ObjS3G_BranchList.Branch_IDColumn.ColumnName.Trim(), ObjS3G_BranchList.BranchColumn.ColumnName.Trim());
            //ddlEBranch.FillDataTable(ObjS3G_BranchList, ObjS3G_BranchList.Branch_IDColumn.ColumnName.Trim(), ObjS3G_BranchList.BranchColumn.ColumnName.Trim());
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@Program_ID", "9");
            //if (AccountCodeId == 0)
            //{
            dictParam.Add("@Is_Active", "1");
            dictParam.Add("@User_ID", intUserID.ToString());
            if (ddlLOB.SelectedIndex != 0)
                dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            else if (ddlELOB.SelectedIndex != 0)
                dictParam.Add("@Lob_Id", ddlELOB.SelectedValue);
            else if (ddlILob.SelectedIndex != 0)
                dictParam.Add("@Lob_Id", ddlILob.SelectedValue);
            else if (ddlLLOB.SelectedIndex != 0)
                dictParam.Add("@Lob_Id", ddlLLOB.SelectedValue);
            //}
            ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            ddlLBranch.BindDataTable(SPNames.BranchMaster_LIST, dictParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            ddlIBranch.BindDataTable(SPNames.BranchMaster_LIST, dictParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            ddlEBranch.BindDataTable(SPNames.BranchMaster_LIST, dictParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriGetLOBList()
    {
        try
        {
            //CompanyMgtServices.S3G_SYSAD_LOBMasterRow ObjLOBMasterRow;
            //ObjLOBMasterRow = ObjS3G_LOBMasterListDataTable.NewS3G_SYSAD_LOBMasterRow();
            //ObjLOBMasterRow.Company_ID = Convert.ToInt32(Session["Company_ID"]);
            //ObjLOBMasterRow.Is_Active = true;
            //ObjS3G_LOBMasterListDataTable.AddS3G_SYSAD_LOBMasterRow(ObjLOBMasterRow);

            //objProduct_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            //SerializationMode SerMode = SerializationMode.Binary;

            //byte[] bytesLOBListDetails = objProduct_MasterClient.FunPubQueryLOB_LIST(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMasterListDataTable, SerMode));
            //ObjS3G_LOBMasterListDataTable = (CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable)ClsPubSerialize.DeSerialize(bytesLOBListDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable));
            //ddlLOB.FillDataTable(ObjS3G_LOBMasterListDataTable, ObjS3G_LOBMasterListDataTable.LOB_IDColumn.ColumnName, ObjS3G_LOBMasterListDataTable.LOBCode_LOBNameColumn.ColumnName);
            //ddlLLOB.FillDataTable(ObjS3G_LOBMasterListDataTable, ObjS3G_LOBMasterListDataTable.LOB_IDColumn.ColumnName, ObjS3G_LOBMasterListDataTable.LOBCode_LOBNameColumn.ColumnName);
            //ddlILob.FillDataTable(ObjS3G_LOBMasterListDataTable, ObjS3G_LOBMasterListDataTable.LOB_IDColumn.ColumnName, ObjS3G_LOBMasterListDataTable.LOBCode_LOBNameColumn.ColumnName);
            //ddlELOB.FillDataTable(ObjS3G_LOBMasterListDataTable, ObjS3G_LOBMasterListDataTable.LOB_IDColumn.ColumnName, ObjS3G_LOBMasterListDataTable.LOBCode_LOBNameColumn.ColumnName);
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@Program_ID", "9");
            dictParam.Add("@User_ID", intUserID.ToString());
            dictParam.Add("@Is_Active", "1");
            //if (AccountCodeId == 0)
            //{

            //}
            ddlLOB.BindDataTable("S3G_Get_LOB_LIST", dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLLOB.BindDataTable("S3G_Get_LOB_LIST", dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlILob.BindDataTable("S3G_Get_LOB_LIST", dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlELOB.BindDataTable("S3G_Get_LOB_LIST", dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region Get Account Desc
    /// <summary>
    /// This Method Get Account description in particluar Tab
    /// </summary>
    /// <param name="AccountTabId"></param>
    /// <param name="gv"></param>
    private void FunPriGetAccountDesc(int AccountTabId, GridView gv)
    {
        try
        {
            //AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable ObS3G_AccountDesc = new AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable();
            //AccountMgtServices.S3G_SYSAD_Account_Code_DetailsRow ObjAcctDescMasterRow = null;
            //ObjAcctDescMasterRow = ObS3G_AccountDesc.NewS3G_SYSAD_Account_Code_DetailsRow();
            //ObjAcctDescMasterRow.Account_Tab_ID = AccountTabId;
            //ObS3G_AccountDesc.AddS3G_SYSAD_Account_Code_DetailsRow(ObjAcctDescMasterRow);
            //ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] byteAcctDescDetails = ObjAccountMasterClient.FunPubAcctDesc(SerMode, ClsPubSerialize.Serialize(ObS3G_AccountDesc, SerMode));
            //AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable dtAcctDesc = (AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable)ClsPubSerialize.DeSerialize(byteAcctDescDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable));
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Account_Tab_ID", AccountTabId.ToString());
            
            DataTable dtAcctDesc = Utility.GetDefaultData("S3G_Get_Account_Code_Details", dictParam);
            gv.DataSource = dtAcctDesc;
            gv.DataBind();

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    #endregion

    #region All Tab ACID
    /// <summary>
    /// Get Account Code Id
    /// </summary>
    /// <returns></returns>
    string GetSelectedAccountCodeIdA()
    {
        StringBuilder AccountCodeID = new StringBuilder();
        try
        {
            for (int i = 0; i < gvAsset.Rows.Count; i++)
            {
                if (gvAsset.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (((CheckBox)gvAsset.Rows[i].FindControl("CbxAsset")).Checked)
                        AccountCodeID.Append(gvAsset.DataKeys[i]["Account_Code_Desc_ID"].ToString().Trim());
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return AccountCodeID.ToString();
    }

    string GetSelectedAccountCodeIdL()
    {
        StringBuilder AccountCodeID = new StringBuilder();
        try
        {
            for (int i = 0; i < gvLiability.Rows.Count; i++)
            {
                if (gvLiability.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (((CheckBox)gvLiability.Rows[i].FindControl("CbxLibility")).Checked)
                        AccountCodeID.Append(gvLiability.DataKeys[i]["Account_Code_Desc_ID"].ToString().Trim());
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return AccountCodeID.ToString();
    }


    string GetSelectedAccountCodeIdI()
    {
        StringBuilder AccountCodeID = new StringBuilder();
        try
        {
            for (int i = 0; i < gvIncome.Rows.Count; i++)
            {
                if (gvIncome.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (((CheckBox)gvIncome.Rows[i].FindControl("CbxIncome")).Checked)
                        AccountCodeID.Append(gvIncome.DataKeys[i]["Account_Code_Desc_ID"].ToString().Trim());
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return AccountCodeID.ToString();
    }


    string GetSelectedAccountCodeIdE()
    {
        StringBuilder AccountCodeID = new StringBuilder();
        try
        {
            for (int i = 0; i < gvExpress.Rows.Count; i++)
            {
                if (gvExpress.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (((CheckBox)gvExpress.Rows[i].FindControl("CbxExpress")).Checked)
                        AccountCodeID.Append(gvExpress.DataKeys[i]["Account_Code_Desc_ID"].ToString().Trim());
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return AccountCodeID.ToString();
    }
    #endregion

    /// <summary>
    /// This methode used Create new account setup details in Asset 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAssetSave_Click(object sender, EventArgs e)
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx?qsMode=Ass';";
            string strRedirectPageAdd = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsTab=Asset';";

            int count = 0;
            for (int i = 0; i < gvAsset.Rows.Count; i++)
            {
                if (((CheckBox)gvAsset.Rows[i].FindControl("CbxAsset")).Checked)
                {
                    count++;
                    if (count > 1)
                    {
                        cvAsset.ErrorMessage = "Only one Account Description selected";
                        cvAsset.IsValid = false;
                        return;
                    }
                }
            }
            int counts = 0;
            foreach (GridViewRow grv in gvAsset.Rows)
            {
                if (((CheckBox)grv.FindControl("CbxAsset")).Checked)
                {
                    counts++;
                }

            }
            if (counts == 0)
            {
                cvAsset.ErrorMessage = "Select atleast one Account Description";
                cvAsset.IsValid = false;
                return;
            }

            string strGLCode = string.Empty, strSLCode = string.Empty;

            if (trA.Visible)
            {
                strGLCode = txtGLCodeSearchA.Text; strSLCode = txtSLCodeSearchA.Text;
            }
            else if (trOldA.Visible)
            {
                strGLCode = txtGLAC.Text; strSLCode = txtSLAC.Text;
            }

            if (!strGLCode.CheckZeroValidate())
            {
                //--Added by Guna on 15th-Oct-2010 to address the bug - 1752
                Utility.FunShowAlertMsg(this.Page, "GL Code Should not be 0");
                //Utility.FunShowAlertMsg(this.Page, "Cannot allow 0 value");                
                //--Ends Here 
                return;
            }

            //--Added by Guna on 15th-Oct-2010 to address the bug - 1753
            if (!strSLCode.CheckZeroValidate())
            {
                Utility.FunShowAlertMsg(this.Page, "SL Code Should not be 0");
                return;
            }
            //--Ends Here 

            AccountMgtServices.S3G_SYSAD_AccountSetupMasterRow ObjAcSetupMasterRow;
            ObjAcSetupMasterRow = ObjS3G_AccountSetupDetails.NewS3G_SYSAD_AccountSetupMasterRow();
            ObjAcSetupMasterRow.Company_ID = intCompanyID;
            ObjAcSetupMasterRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjAcSetupMasterRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjAcSetupMasterRow.Account_Tab_ID = 1;
            ObjAcSetupMasterRow.Account_Code_Desc_ID = Convert.ToInt32(GetSelectedAccountCodeIdA());
            ObjAcSetupMasterRow.Account_Setup_ID = AccountCodeId;
            ObjAcSetupMasterRow.GL_Code = strGLCode.Trim();
            ObjAcSetupMasterRow.SL_Code = strSLCode.Trim();
            ObjAcSetupMasterRow.Is_Active = CbxActive.Checked;
            ObjAcSetupMasterRow.Created_By = intUserID;
            ObjAcSetupMasterRow.Created_On = DateTime.Now;
            ObjAcSetupMasterRow.Modified_By = intUserID;
            ObjAcSetupMasterRow.Modified_On = DateTime.Now;
            if (Convert.ToInt32(ddlLOB.SelectedValue) == 0 && Convert.ToInt32(ddlBranch.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "C";
            }
            else if (Convert.ToInt32(ddlBranch.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "L";
            }
            else if (Convert.ToInt32(ddlLOB.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "B";
            }
            if (Convert.ToInt32(ddlLOB.SelectedValue) != 0 && Convert.ToInt32(ddlBranch.SelectedValue) != 0)
            {
                ObjAcSetupMasterRow.Level = "A";
            }
            ObjS3G_AccountSetupDetails.AddS3G_SYSAD_AccountSetupMasterRow(ObjAcSetupMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            if (AccountCodeId > 0)
            {
                if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
                {
                    if (FunCheckLobisvalid(ddlLOB.SelectedValue))
                    {
                        intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
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
                    intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                }


                if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
                {
                    if (FunCheckBranchisvalid(ddlBranch.SelectedValue))
                    {
                        intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Selected Branch rights not assigned");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        return;
                    }
                }
                else
                {
                    intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                }


            }
            else
            {
                intErrCode = ObjAccountMasterClient.FunPubCreateAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
            }
            switch (intErrCode)
            {
                case 0:

                    if (AccountCodeId > 0)
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnAssetSave.Enabled = false;
                        //End here
                        strKey = "Modify";
                        strAlert = strAlert.Replace("__ALERT__", "Account setup details updated successfully");
                    }
                    else
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnAssetSave.Enabled = false;
                        //End here
                        strAlert = "Account Setup added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        CbxIActive.Checked = true;
                    }
                    break;
                case 1:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code already exists!");
                    strRedirectPageView = "";
                    break;
                case 2:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has mapped with other SL Code cannot be defined as individual GL Code!");
                    strRedirectPageView = "";
                    break;
                case 3:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has defined as individual GL Code cannot be Combined with SL Code!");
                    strRedirectPageView = "";
                    break;
                case 4:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code,SL Code Combination already exists!");
                    strRedirectPageView = "";
                    break;
                case 5:

                    strAlert = strAlert.Replace("__ALERT__", "SL Code mapped to a different GL Code!");
                    strRedirectPageView = "";
                    break;
                case 6:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has defined as individual GL Code cannot be Combined with SL Code!");
                    strRedirectPageView = "";
                    break;

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
            vsExpress.Visible = false;
            vsIncome.Visible = false;
            vsLiaibily.Visible = false;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }
    /// <summary>
    /// This methode used Create new account setup details in Libaility
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLiailitySave_Click(object sender, EventArgs e)
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx?qsMode=Lib';";
            string strRedirectPageAdd = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsTab=Liability';";

            int count = 0;
            for (int i = 0; i < gvLiability.Rows.Count; i++)
            {
                if (((CheckBox)gvLiability.Rows[i].FindControl("CbxLibility")).Checked)
                {
                    count++;
                    if (count > 1)
                    {

                        cvLib.ErrorMessage = "Only one Account Description selected";
                        cvLib.IsValid = false;
                        return;
                    }
                }
            }
            int counts = 0;
            foreach (GridViewRow grv in gvLiability.Rows)
            {
                if (((CheckBox)grv.FindControl("CbxLibility")).Checked)
                {
                    counts++;
                }

            }
            if (counts == 0)
            {
                cvLib.ErrorMessage = "Select atleast one Account Description";
                cvLib.IsValid = false;
                return;

            }


            string strGLCode = string.Empty, strSLCode = string.Empty;

            if (trL.Visible)
            {
                strGLCode = txtGLCodeSearchL.Text; strSLCode = txtSLCodeSearchL.Text;
            }
            else if (trOldL.Visible)
            {
                strGLCode = txtLGLAC.Text; strSLCode = txtLSLAC.Text;
            }

            if ((strGLCode == "0") || (strGLCode == "00") || (strGLCode == "000") || (strGLCode == "0000") || (strGLCode == "00000") || (strGLCode == "000000"))
            {
                Utility.FunShowAlertMsg(this.Page, "Cannot allow 0 value");
                return;
            }
            AccountMgtServices.S3G_SYSAD_AccountSetupMasterRow ObjAcSetupMasterRow;
            ObjAcSetupMasterRow = ObjS3G_AccountSetupDetails.NewS3G_SYSAD_AccountSetupMasterRow();
            ObjAcSetupMasterRow.Company_ID = intCompanyID;
            ObjAcSetupMasterRow.LOB_ID = Convert.ToInt32(ddlLLOB.SelectedValue);
            ObjAcSetupMasterRow.Branch_ID = Convert.ToInt32(ddlLBranch.SelectedValue);
            ObjAcSetupMasterRow.Account_Tab_ID = 2;
            ObjAcSetupMasterRow.Account_Code_Desc_ID = Convert.ToInt32(GetSelectedAccountCodeIdL());
            //Convert.ToInt32(hdnSelectedValueL.Value);
            ObjAcSetupMasterRow.Account_Setup_ID = AccountCodeId;
            ObjAcSetupMasterRow.GL_Code = strGLCode;
            ObjAcSetupMasterRow.SL_Code = strSLCode;
            ObjAcSetupMasterRow.Is_Active = CbxLActive.Checked;
            ObjAcSetupMasterRow.Created_By = intUserID;
            ObjAcSetupMasterRow.Created_On = DateTime.Now;
            ObjAcSetupMasterRow.Modified_By = intUserID;
            ObjAcSetupMasterRow.Modified_On = DateTime.Now;
            if (Convert.ToInt32(ddlLLOB.SelectedValue) == 0 && Convert.ToInt32(ddlLBranch.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "C";
            }
            else if (Convert.ToInt32(ddlLBranch.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "L";
            }
            else if (Convert.ToInt32(ddlLLOB.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "B";
            }
            if (Convert.ToInt32(ddlLLOB.SelectedValue) != 0 && Convert.ToInt32(ddlLBranch.SelectedValue) != 0)
            {
                ObjAcSetupMasterRow.Level = "A";
            }
            ObjS3G_AccountSetupDetails.AddS3G_SYSAD_AccountSetupMasterRow(ObjAcSetupMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            if (AccountCodeId > 0)
            {
                if (Convert.ToInt32(ddlLLOB.SelectedValue) > 0)
                {
                    if (FunCheckLobisvalid(ddlLLOB.SelectedValue))
                    {

                        intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
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
                    intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                }


                if (Convert.ToInt32(ddlLBranch.SelectedValue) > 0)
                {
                    if (FunCheckBranchisvalid(ddlLBranch.SelectedValue))
                    {
                        intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Selected Branch rights not assigned");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        return;
                    }
                }
                else
                {
                    intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                }
            }
            else
            {
                intErrCode = ObjAccountMasterClient.FunPubCreateAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
            }
            switch (intErrCode)
            {
                case 0:

                    if (AccountCodeId > 0)
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnLiailitySave.Enabled = false;
                        //End here
                        strKey = "Modify";
                        strAlert = strAlert.Replace("__ALERT__", "Account setup details updated successfully");
                    }
                    else
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnLiailitySave.Enabled = false;
                        //End here
                        strAlert = "Account Setup added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        CbxIActive.Checked = true;
                    }
                    break;
                case 1:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code already exists!");
                    strRedirectPageView = "";
                    break;
                case 2:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has mapped with other SL Code cannot be defined as individual GL Code!");
                    strRedirectPageView = "";
                    break;
                case 3:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has defined as individual GL Code cannot be Combined with SL Code!");
                    strRedirectPageView = "";
                    break;
                case 4:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code,SL Code Combination already exists!");
                    strRedirectPageView = "";
                    break;
                case 5:

                    strAlert = strAlert.Replace("__ALERT__", "SL Code mapped to a different GL Code!");
                    strRedirectPageView = "";
                    break;

            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
            vsExpress.Visible = false;
            vsIncome.Visible = false;
            vsAsset.Visible = false;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }
    /// <summary>
    /// This methode used Create new account setup details in Income
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnIncomeSave_Click(object sender, EventArgs e)
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx?qsMode=Inc';";
            string strRedirectPageAdd = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsTab=Income';";

            int count = 0;
            for (int i = 0; i < gvIncome.Rows.Count; i++)
            {
                if (((CheckBox)gvIncome.Rows[i].FindControl("CbxIncome")).Checked)
                {
                    count++;
                    if (count > 1)
                    {

                        cvIncome.ErrorMessage = "Only one Account Description selected";
                        cvIncome.IsValid = false;
                        return;
                    }
                }
            }
            int counts = 0;
            foreach (GridViewRow grv in gvIncome.Rows)
            {
                if (((CheckBox)grv.FindControl("CbxIncome")).Checked)
                {
                    counts++;
                }

            }
            if (counts == 0)
            {
                cvIncome.ErrorMessage = "Select atleast one Account Description";
                cvIncome.IsValid = false;
                return;
            }

            string strGLCode = string.Empty, strSLCode = string.Empty;

            if (trI.Visible)
            {
                strGLCode = txtGLCodeSearchI.Text; strSLCode = txtSLCodeSearchI.Text;
            }
            else if (trOldI.Visible)
            {
                strGLCode = txtIGLAC.Text; strSLCode = txtISLAC.Text;
            }

            if ((strGLCode == "0") || (strGLCode == "00") || (strGLCode == "000") || (strGLCode == "0000") || (strGLCode == "00000") || (strGLCode == "000000"))
            {
                Utility.FunShowAlertMsg(this.Page, "Cannot allow 0 value");
                return;
            }
            AccountMgtServices.S3G_SYSAD_AccountSetupMasterRow ObjAcSetupMasterRow;
            ObjAcSetupMasterRow = ObjS3G_AccountSetupDetails.NewS3G_SYSAD_AccountSetupMasterRow();
            ObjAcSetupMasterRow.Company_ID = intCompanyID;
            ObjAcSetupMasterRow.LOB_ID = Convert.ToInt32(ddlILob.SelectedValue);
            ObjAcSetupMasterRow.Branch_ID = Convert.ToInt32(ddlIBranch.SelectedValue);
            ObjAcSetupMasterRow.Account_Tab_ID = 3;
            ObjAcSetupMasterRow.Account_Code_Desc_ID = Convert.ToInt32(GetSelectedAccountCodeIdI());
            //Convert.ToInt32(hdnSelectedValueI.Value);
            ObjAcSetupMasterRow.Account_Setup_ID = AccountCodeId;
            ObjAcSetupMasterRow.GL_Code = strGLCode;
            ObjAcSetupMasterRow.SL_Code = strSLCode;
            ObjAcSetupMasterRow.Is_Active = CbxIActive.Checked;
            ObjAcSetupMasterRow.Created_By = intUserID;
            ObjAcSetupMasterRow.Created_On = DateTime.Now;
            ObjAcSetupMasterRow.Modified_By = intUserID;
            ObjAcSetupMasterRow.Modified_On = DateTime.Now;
            if (Convert.ToInt32(ddlILob.SelectedValue) == 0 && Convert.ToInt32(ddlIBranch.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "C";
            }
            else if (Convert.ToInt32(ddlIBranch.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "L";
            }
            else if (Convert.ToInt32(ddlILob.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "B";
            }
            if (Convert.ToInt32(ddlILob.SelectedValue) != 0 && Convert.ToInt32(ddlIBranch.SelectedValue) != 0)
            {
                ObjAcSetupMasterRow.Level = "A";
            }
            ObjS3G_AccountSetupDetails.AddS3G_SYSAD_AccountSetupMasterRow(ObjAcSetupMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            if (AccountCodeId > 0)
            {
                if (Convert.ToInt32(ddlILob.SelectedValue) > 0)
                {
                    if (FunCheckLobisvalid(ddlILob.SelectedValue))
                    {
                        intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
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
                    intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                }

                if (Convert.ToInt32(ddlIBranch.SelectedValue) > 0)
                {
                    if (FunCheckBranchisvalid(ddlIBranch.SelectedValue))
                    {
                        intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Selected Branch rights not assigned");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        return;
                    }
                }
                else
                {
                    intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                }
            }
            else
            {
                intErrCode = ObjAccountMasterClient.FunPubCreateAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
            }
            switch (intErrCode)
            {
                case 0:

                    if (AccountCodeId > 0)
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnIncomeSave.Enabled = false;
                        //End here
                        strKey = "Modify";
                        strAlert = strAlert.Replace("__ALERT__", "Account setup details updated successfully");
                    }
                    else
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnIncomeSave.Enabled = false;
                        //End here
                        strAlert = "Account Setup added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        CbxIActive.Checked = true;
                    }
                    break;
                case 1:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code already exists!");
                    strRedirectPageView = "";
                    break;
                case 2:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has mapped with other SL Code cannot be defined as individual GL Code!");
                    strRedirectPageView = "";
                    break;
                case 3:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has defined as individual GL Code cannot be Combined with SL Code!");
                    strRedirectPageView = "";
                    break;
                case 4:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code,SL Code Combination already exists!");
                    strRedirectPageView = "";
                    break;
                case 5:

                    strAlert = strAlert.Replace("__ALERT__", "SL Code mapped to a different GL Code!");
                    strRedirectPageView = "";
                    break;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
            vsExpress.Visible = false;
            vsLiaibily.Visible = false;
            vsAsset.Visible = false;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }
    /// <summary>
    /// This methode used Create new account setup details in Expenses
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExpressSave_Click(object sender, EventArgs e)
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx?qsMode=Exp';";
            string strRedirectPageAdd = "window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsTab=Express';";

            int count = 0;
            for (int i = 0; i < gvExpress.Rows.Count; i++)
            {
                if (((CheckBox)gvExpress.Rows[i].FindControl("CbxExpress")).Checked)
                {
                    count++;
                    if (count > 1)
                    {
                        cvExp.ErrorMessage = "Only one Account Description selected";
                        cvExp.IsValid = false;
                        return;

                    }
                }
            }
            int counts = 0;
            foreach (GridViewRow grv in gvExpress.Rows)
            {
                if (((CheckBox)grv.FindControl("CbxExpress")).Checked)
                {
                    counts++;
                }

            }
            if (counts == 0)
            {
                cvExp.ErrorMessage = "Select atleast one Account Description";
                cvExp.IsValid = false;
                return;

            }

            string strGLCode = string.Empty, strSLCode = string.Empty;

            if (trE.Visible)
            {
                strGLCode = txtGLCodeSearchE.Text; strSLCode = txtSLCodeSearchE.Text;
            }
            else if (trOldE.Visible)
            {
                strGLCode = txtEGLAC.Text; strSLCode = txtESLAC.Text;
            }


            if ((strGLCode == "0") || (strGLCode == "00") || (strGLCode == "000") || (strGLCode == "0000") || (strGLCode == "00000") || (strGLCode == "000000"))
            {
                Utility.FunShowAlertMsg(this.Page, "Cannot allow 0 value");
                return;
            }

            AccountMgtServices.S3G_SYSAD_AccountSetupMasterRow ObjAcSetupMasterRow;
            ObjAcSetupMasterRow = ObjS3G_AccountSetupDetails.NewS3G_SYSAD_AccountSetupMasterRow();
            ObjAcSetupMasterRow.Company_ID = intCompanyID;
            ObjAcSetupMasterRow.LOB_ID = Convert.ToInt32(ddlELOB.SelectedValue);
            ObjAcSetupMasterRow.Branch_ID = Convert.ToInt32(ddlEBranch.SelectedValue);
            ObjAcSetupMasterRow.Account_Tab_ID = 4;
            ObjAcSetupMasterRow.Account_Code_Desc_ID = Convert.ToInt32(GetSelectedAccountCodeIdE());
            //Convert.ToInt32(hdnSelectedValueE.Value);
            ObjAcSetupMasterRow.Account_Setup_ID = AccountCodeId;
            ObjAcSetupMasterRow.GL_Code = strGLCode;
            ObjAcSetupMasterRow.SL_Code = strSLCode;
            ObjAcSetupMasterRow.Is_Active = CbxEActive.Checked;
            ObjAcSetupMasterRow.Created_By = intUserID;
            ObjAcSetupMasterRow.Created_On = DateTime.Now;
            ObjAcSetupMasterRow.Modified_By = intUserID;
            ObjAcSetupMasterRow.Modified_On = DateTime.Now;
            if (Convert.ToInt32(ddlELOB.SelectedValue) == 0 && Convert.ToInt32(ddlEBranch.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "C";
            }
            else if (Convert.ToInt32(ddlEBranch.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "L";
            }
            else if (Convert.ToInt32(ddlELOB.SelectedValue) == 0)
            {
                ObjAcSetupMasterRow.Level = "B";
            }
            if (Convert.ToInt32(ddlELOB.SelectedValue) != 0 && Convert.ToInt32(ddlEBranch.SelectedValue) != 0)
            {
                ObjAcSetupMasterRow.Level = "A";
            }
            ObjS3G_AccountSetupDetails.AddS3G_SYSAD_AccountSetupMasterRow(ObjAcSetupMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            if (AccountCodeId > 0)
            {
                if (Convert.ToInt32(ddlELOB.SelectedValue) > 0)
                {
                    if (FunCheckLobisvalid(ddlELOB.SelectedValue))
                    {

                        intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
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
                    intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                }


                if (Convert.ToInt32(ddlEBranch.SelectedValue) > 0)
                {
                    if (FunCheckBranchisvalid(ddlEBranch.SelectedValue))
                    {
                        intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Selected Branch rights not assigned");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        return;
                    }
                }
                else
                {
                    intErrCode = ObjAccountMasterClient.FunPubModifyAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
                }
            }
            else
            {
                intErrCode = ObjAccountMasterClient.FunPubCreateAcctSetup(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountSetupDetails, SerMode));
            }

            switch (intErrCode)
            {
                case 0:

                    if (AccountCodeId > 0)
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnExpressSave.Enabled = false;
                        //End here
                        strKey = "Modify";
                        strAlert = strAlert.Replace("__ALERT__", "Account setup details updated successfully");
                    }
                    else
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnExpressSave.Enabled = false;
                        //End here
                        strAlert = "Account Setup added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        CbxIActive.Checked = true;
                    }
                    break;
                case 1:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code already exists!");
                    strRedirectPageView = "";
                    break;
                case 2:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has mapped with other SL Code cannot be defined as individual GL Code!");
                    strRedirectPageView = "";
                    break;
                case 3:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code has defined as individual GL Code cannot be Combined with SL Code!");
                    strRedirectPageView = "";
                    break;
                case 4:

                    strAlert = strAlert.Replace("__ALERT__", "GL Code,SL Code Combination already exists!");
                    strRedirectPageView = "";
                    break;
                case 5:

                    strAlert = strAlert.Replace("__ALERT__", "SL Code mapped to a different GL Code!");
                    strRedirectPageView = "";
                    break;

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
            vsIncome.Visible = false;
            vsLiaibily.Visible = false;
            vsAsset.Visible = false;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }


    #region Insert AccountDesc
    /// <summary>
    /// Insert Account Desc and flag for Asset tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkAAdd_Click(object sender, EventArgs e)
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            TextBox txtDesc = gvAsset.FooterRow.FindControl("txtADesc") as TextBox;
            TextBox txtflags = gvAsset.FooterRow.FindControl("txtFlag") as TextBox;
            if (txtDesc.Text.Length == 0 & txtflags.Text.Length == 0)
            {
                cvAsset.ErrorMessage = "Enter the Account Description and Flag ";
                cvAsset.IsValid = false;
                return;
            }
            if (txtDesc.Text.Length == 0)
            {
                cvAsset.ErrorMessage = "Enter the Account Description ";
                cvAsset.IsValid = false;
                return;
            }
            if (txtflags.Text.Length == 0)
            {
                cvAsset.ErrorMessage = "Enter the Account Flag ";
                cvAsset.IsValid = false;
                return;
            }

            AccountMgtServices.S3G_SYSAD_AccountCodeDescRow ObjAcctDescMasterRow;
            ObjAcctDescMasterRow = ObjS3G_AccountDesc.NewS3G_SYSAD_AccountCodeDescRow();
            ObjAcctDescMasterRow.Account_Code_Desc = txtDesc.Text.ToString();
            ObjAcctDescMasterRow.Account_Flag = txtflags.Text.ToString();
            
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            
            dictParam.Add("@Account_Flag", txtflags.Text.ToString());

            DataTable dtAccFlag = Utility.GetDefaultData("S3G_Get_AccountFlag", dictParam);

            Count = Convert.ToInt32(dtAccFlag.Rows[0][0].ToString());

            if (Count > 0)
            {
                Utility.FunShowAlertMsg(this, "Flag already exists");
                return;
            }

            

            ObjAcctDescMasterRow.Account_Tab_ID = 1;
            ObjS3G_AccountDesc.AddS3G_SYSAD_AccountCodeDescRow(ObjAcctDescMasterRow);


            SerializationMode SerMode = SerializationMode.Binary;
            intErrCode = ObjAccountMasterClient.FunPubCreateAcctDesc(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountDesc, SerMode));
            if (intErrCode == 1)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Description details already exist');", true);
            else if (intErrCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Flag already exist');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Description details added successfully');window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsTab=Asset';", true);
                CbxActive.Checked = true;
            }
            lblErrorMessage.Text = string.Empty;
            FunPriGetAccountDesc(1, gvAsset);
            tbLib.Enabled = false;
            tbIncome.Enabled = false;
            tbExpress.Enabled = false;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }
    /// <summary>
    /// Insert Account Desc and flag for Libailty tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkLAdd_Click(object sender, EventArgs e)
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            TextBox txtDesc = gvLiability.FooterRow.FindControl("txtLDesc") as TextBox;
            TextBox txtflag = gvLiability.FooterRow.FindControl("txtLFlag") as TextBox;
            if (txtDesc.Text.Length == 0 & txtflag.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Description and flag ";
                //return;

                cvLib.ErrorMessage = "Enter the Account Description and Flag ";
                cvLib.IsValid = false;
                return;
            }
            if (txtDesc.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Description ";
                //return;
                cvLib.ErrorMessage = "Enter the Account Description ";
                cvLib.IsValid = false;
                return;
            }
            if (txtflag.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Flag ";
                //return;
                cvLib.ErrorMessage = "Enter the Account Flag ";
                cvLib.IsValid = false;
                return;
            }
            AccountMgtServices.S3G_SYSAD_AccountCodeDescRow ObjAcctDescMasterRow;
            ObjAcctDescMasterRow = ObjS3G_AccountDesc.NewS3G_SYSAD_AccountCodeDescRow();
            ObjAcctDescMasterRow.Account_Code_Desc = txtDesc.Text.ToString();
            ObjAcctDescMasterRow.Account_Flag = txtflag.Text.ToString();
            ObjAcctDescMasterRow.Account_Tab_ID = 2;
            ObjS3G_AccountDesc.AddS3G_SYSAD_AccountCodeDescRow(ObjAcctDescMasterRow);

            Dictionary<string, string> dictParam = new Dictionary<string, string>();

            dictParam.Add("@Account_Flag", txtflag.Text.ToString());

            DataTable dtAccFlag = Utility.GetDefaultData("S3G_Get_AccountFlag", dictParam);

            Count = Convert.ToInt32(dtAccFlag.Rows[0][0].ToString());

            if (Count > 0)
            {
                Utility.FunShowAlertMsg(this, "Flag already exists");
                return;
            }

            SerializationMode SerMode = SerializationMode.Binary;
            intErrCode = ObjAccountMasterClient.FunPubCreateAcctDesc(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountDesc, SerMode));
            if (intErrCode == 1)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Description already exist');", true);
            else if (intErrCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Flag already exist');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Description details added successfully');window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsTab=Liability';", true);
            }
            lblErrorMessage.Text = string.Empty;
            FunPriGetAccountDesc(2, gvLiability);
            tbAsset.Enabled = false;
            tbIncome.Enabled = false;
            tbExpress.Enabled = false;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }

    /// <summary>
    /// Insert Account Desc and flag for Income tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkIAdd_Click(object sender, EventArgs e)
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            TextBox txtDesc = gvIncome.FooterRow.FindControl("txtIDesc") as TextBox;
            TextBox txtflag = gvIncome.FooterRow.FindControl("txtIFlag") as TextBox;
            if (txtDesc.Text.Length == 0 & txtflag.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Description and flag ";
                //return;
                cvIncome.ErrorMessage = "Enter the Account Description and Flag ";
                cvIncome.IsValid = false;
                return;
            }
            if (txtDesc.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Description ";
                //return;
                cvIncome.ErrorMessage = "Enter the Account Description ";
                cvIncome.IsValid = false;
                return;
            }
            if (txtflag.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Flag ";
                //return;
                cvIncome.ErrorMessage = "Enter the Account Flag";
                cvIncome.IsValid = false;
                return;
            }
            AccountMgtServices.S3G_SYSAD_AccountCodeDescRow ObjAcctDescMasterRow;
            ObjAcctDescMasterRow = ObjS3G_AccountDesc.NewS3G_SYSAD_AccountCodeDescRow();
            ObjAcctDescMasterRow.Account_Code_Desc = txtDesc.Text.ToString();
            ObjAcctDescMasterRow.Account_Flag = txtflag.Text.ToString();
            ObjAcctDescMasterRow.Account_Tab_ID = 3;
            ObjS3G_AccountDesc.AddS3G_SYSAD_AccountCodeDescRow(ObjAcctDescMasterRow);


            Dictionary<string, string> dictParam = new Dictionary<string, string>();

            dictParam.Add("@Account_Flag", txtflag.Text.ToString());

            DataTable dtAccFlag = Utility.GetDefaultData("S3G_Get_AccountFlag", dictParam);

            Count = Convert.ToInt32(dtAccFlag.Rows[0][0].ToString());

            if (Count > 0)
            {
                Utility.FunShowAlertMsg(this, "Flag already exists");
                return;
            }


            SerializationMode SerMode = SerializationMode.Binary;
            intErrCode = ObjAccountMasterClient.FunPubCreateAcctDesc(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountDesc, SerMode));
            if (intErrCode == 1)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Description already exist');", true);
            else if (intErrCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Flag already exist');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Description details added successfully');window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsTab=Income';", true);
            }
            lblErrorMessage.Text = string.Empty;
            FunPriGetAccountDesc(3, gvIncome);
            tbAsset.Enabled = false;
            tbLib.Enabled = false;
            tbExpress.Enabled = false;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }

    /// <summary>
    /// Insert Account Desc and flag for Expenses tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkEAdd_Click(object sender, EventArgs e)
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            TextBox txtDesc = gvExpress.FooterRow.FindControl("txtEDesc") as TextBox;
            TextBox txtflag = gvExpress.FooterRow.FindControl("txtEFlag") as TextBox;
            if (txtDesc.Text.Length == 0 & txtflag.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Description and flag ";
                //return;
                cvExp.ErrorMessage = "Enter the Account Description and Flag ";
                cvExp.IsValid = false;
                return;
            }
            if (txtDesc.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Description ";
                //return;
                cvExp.ErrorMessage = "Enter the Account Description ";
                cvExp.IsValid = false;
                return;
            }
            if (txtflag.Text.Length == 0)
            {
                //lblErrorMessage.Text = "Enter the Account Flag ";
                //return;
                cvExp.ErrorMessage = "Enter the Account Flag ";
                cvExp.IsValid = false;
                return;
            }
            AccountMgtServices.S3G_SYSAD_AccountCodeDescRow ObjAcctDescMasterRow;
            ObjAcctDescMasterRow = ObjS3G_AccountDesc.NewS3G_SYSAD_AccountCodeDescRow();
            ObjAcctDescMasterRow.Account_Code_Desc = txtDesc.Text.ToString();
            ObjAcctDescMasterRow.Account_Flag = txtflag.Text.ToString();
            ObjAcctDescMasterRow.Account_Tab_ID = 4;
            ObjS3G_AccountDesc.AddS3G_SYSAD_AccountCodeDescRow(ObjAcctDescMasterRow);


            Dictionary<string, string> dictParam = new Dictionary<string, string>();

            dictParam.Add("@Account_Flag", txtflag.Text.ToString());

            DataTable dtAccFlag = Utility.GetDefaultData("S3G_Get_AccountFlag", dictParam);

            Count = Convert.ToInt32(dtAccFlag.Rows[0][0].ToString());

            if (Count > 0)
            {
                Utility.FunShowAlertMsg(this, "Flag already exists");
                return;
            }


            SerializationMode SerMode = SerializationMode.Binary;
            intErrCode = ObjAccountMasterClient.FunPubCreateAcctDesc(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountDesc, SerMode));
            if (intErrCode == 1)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Description already exist');", true);
            else if (intErrCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Flag already exist');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Description details added successfully');window.location.href='../System Admin/S3G_SysAdminAccountSetupMaster_Add.aspx?qsTab=Express';", true);
            }
            lblErrorMessage.Text = string.Empty;
            FunPriGetAccountDesc(4, gvExpress);
            tbAsset.Enabled = false;
            tbLib.Enabled = false;
            tbIncome.Enabled = false;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }


    #endregion

    #region Get Account Setup
    /// <summary>
    /// This Method used Get Asset details in modify mode
    /// </summary>
    private void FunGetAssetDetatils()
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            AccountMgtServices.S3G_SYSAD_AccountDetailsRow ObjAccountSetupMasterRow;
            ObjAccountSetupMasterRow = ObjS3G_AccountMasterDetails.NewS3G_SYSAD_AccountDetailsRow();
            ObjAccountSetupMasterRow.Account_Setup_ID = AccountCodeId;
            ObjAccountSetupMasterRow.Account_Tab_ID = 1;
            ObjAccountSetupMasterRow.Company_ID = intCompanyID;
            ObjS3G_AccountMasterDetails.AddS3G_SYSAD_AccountDetailsRow(ObjAccountSetupMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteAccSetUpDetails = ObjAccountMasterClient.FunPubGetAccountDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountMasterDetails, SerMode));
            ObjS3G_AccountMasterDetails = (AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable)ClsPubSerialize.DeSerialize(byteAccSetUpDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable));
            if (ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString() != "" || ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString() != "0")
            {
                //Added by Thangam M on 20/Mar/2012 to load inactive LOB's
                ListItem ListLOB = new ListItem(Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB"]), Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"]), true);
                ListLOB.Selected = false;
                if (ddlLOB.Items.FindByValue(Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"])) == null)
                {
                    ddlLOB.Items.Add(ListLOB);
                }
                ddlLOB.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString();
            }

            FunPriGetBranchList();

            if (ObjS3G_AccountMasterDetails.Rows[0]["Location_ID"].ToString() != "" || ObjS3G_AccountMasterDetails.Rows[0]["Location_ID"].ToString() != "0")
                ddlBranch.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["Location_ID"].ToString();
            if (trA.Visible)
            {
                txtGLCodeSearchA.Text = ObjS3G_AccountMasterDetails.Rows[0]["GL_CODE"].ToString();
                txtSLCodeSearchA.Text = ObjS3G_AccountMasterDetails.Rows[0]["SL_CODE"].ToString();
            }
            else if (trOldA.Visible)
            {
                txtGLAC.Text = ObjS3G_AccountMasterDetails.Rows[0]["GL_CODE"].ToString();
                txtSLAC.Text = ObjS3G_AccountMasterDetails.Rows[0]["SL_CODE"].ToString();
            }

            hdnID.Value = ObjS3G_AccountMasterDetails.Rows[0]["Created_By"].ToString();
            for (int i = 0; i < gvAsset.Rows.Count; i++)
            {
                if (gvAsset.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    int Account_Code_Desc_ID = Convert.ToInt32(gvAsset.DataKeys[i]["Account_Code_Desc_ID"].ToString());
                    if (Account_Code_Desc_ID == Convert.ToInt32(ObjS3G_AccountMasterDetails.Rows[0]["Account_Code_Desc_ID"].ToString()))
                    {

                        CheckBox Cbx = (CheckBox)gvAsset.Rows[i].FindControl("CbxAsset");
                        Cbx.Checked = true;
                    }
                }
            }
            //   ObjS3G_AccountMasterDetails.Rows[0]["Account_Code_Desc_ID"].ToString();

            if (ObjS3G_AccountMasterDetails.Rows[0]["Is_Active"].ToString() == "True")
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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }

    /// <summary>
    /// This Method used Get Libailty details in modify mode
    /// </summary>
    private void FunGetLIBDetatils()
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            AccountMgtServices.S3G_SYSAD_AccountDetailsRow ObjAccountSetupMasterRow;
            ObjAccountSetupMasterRow = ObjS3G_AccountMasterDetails.NewS3G_SYSAD_AccountDetailsRow();
            ObjAccountSetupMasterRow.Account_Setup_ID = AccountCodeId;
            ObjAccountSetupMasterRow.Account_Tab_ID = 2;
            ObjAccountSetupMasterRow.Company_ID = intCompanyID;
            ObjS3G_AccountMasterDetails.AddS3G_SYSAD_AccountDetailsRow(ObjAccountSetupMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteAccSetUpDetails = ObjAccountMasterClient.FunPubGetAccountDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountMasterDetails, SerMode));
            ObjS3G_AccountMasterDetails = (AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable)ClsPubSerialize.DeSerialize(byteAccSetUpDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable));

            if (ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString() != "" || ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString() != "0")
            {
                //Added by Thangam M on 20/Mar/2012 to load inactive LOB's
                ListItem ListLOB = new ListItem(Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB"]), Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"]), true);
                ListLOB.Selected = false;
                if (ddlLLOB.Items.FindByValue(Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"])) == null)
                {
                    ddlLLOB.Items.Add(ListLOB);
                }
                ddlLLOB.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString();
            }

            FunPriGetBranchList();

            //ddlLLOB.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString();
            ddlLBranch.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["Location_ID"].ToString();
            if (trL.Visible)
            {
                txtGLCodeSearchL.Text = ObjS3G_AccountMasterDetails.Rows[0]["GL_CODE"].ToString();
                txtSLCodeSearchL.Text = ObjS3G_AccountMasterDetails.Rows[0]["SL_CODE"].ToString();
            }
            else if (trOldL.Visible)
            {
                txtLGLAC.Text = ObjS3G_AccountMasterDetails.Rows[0]["GL_CODE"].ToString();
                txtLSLAC.Text = ObjS3G_AccountMasterDetails.Rows[0]["SL_CODE"].ToString();
            }
            
            hdnID.Value = ObjS3G_AccountMasterDetails.Rows[0]["Created_By"].ToString();
            for (int i = 0; i < gvLiability.Rows.Count; i++)
            {
                if (gvLiability.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    int Account_Code_Desc_ID = Convert.ToInt32(gvLiability.DataKeys[i]["Account_Code_Desc_ID"].ToString());
                    if (Account_Code_Desc_ID == Convert.ToInt32(ObjS3G_AccountMasterDetails.Rows[0]["Account_Code_Desc_ID"].ToString()))
                    {

                        CheckBox Cbx = (CheckBox)gvLiability.Rows[i].FindControl("CbxLibility");
                        Cbx.Checked = true;
                    }
                }
            }

            if (ObjS3G_AccountMasterDetails.Rows[0]["Is_Active"].ToString() == "True")
                CbxLActive.Checked = true;
            else
                CbxLActive.Checked = false;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }
    /// <summary>
    /// This Method used Get Income details in modify mode
    /// </summary>
    private void FunGetINCDetatils()
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            AccountMgtServices.S3G_SYSAD_AccountDetailsRow ObjAccountSetupMasterRow;
            ObjAccountSetupMasterRow = ObjS3G_AccountMasterDetails.NewS3G_SYSAD_AccountDetailsRow();
            ObjAccountSetupMasterRow.Account_Setup_ID = AccountCodeId;
            ObjAccountSetupMasterRow.Account_Tab_ID = 3;
            ObjAccountSetupMasterRow.Company_ID = intCompanyID;
            ObjS3G_AccountMasterDetails.AddS3G_SYSAD_AccountDetailsRow(ObjAccountSetupMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteAccSetUpDetails = ObjAccountMasterClient.FunPubGetAccountDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountMasterDetails, SerMode));
            ObjS3G_AccountMasterDetails = (AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable)ClsPubSerialize.DeSerialize(byteAccSetUpDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable));

            if (ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString() != "" || ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString() != "0")
            {
                //Added by Thangam M on 20/Mar/2012 to load inactive LOB's
                ListItem ListLOB = new ListItem(Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB"]), Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"]), true);
                ListLOB.Selected = false;
                if (ddlILob.Items.FindByValue(Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"])) == null)
                {
                    ddlILob.Items.Add(ListLOB);
                }
                ddlILob.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString();
            }

            FunPriGetBranchList();

            //ddlILob.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString();
            ddlIBranch.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["Location_ID"].ToString();
            if (trI.Visible)
            {
                txtGLCodeSearchI.Text = ObjS3G_AccountMasterDetails.Rows[0]["GL_CODE"].ToString();
                txtSLCodeSearchI.Text = ObjS3G_AccountMasterDetails.Rows[0]["SL_CODE"].ToString();
            }
            else if (trOldI.Visible)
            {
                txtIGLAC.Text = ObjS3G_AccountMasterDetails.Rows[0]["GL_CODE"].ToString();
                txtISLAC.Text = ObjS3G_AccountMasterDetails.Rows[0]["SL_CODE"].ToString();
            }
            
            hdnID.Value = ObjS3G_AccountMasterDetails.Rows[0]["Created_By"].ToString();
            for (int i = 0; i < gvIncome.Rows.Count; i++)
            {
                if (gvIncome.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    int Account_Code_Desc_ID = Convert.ToInt32(gvIncome.DataKeys[i]["Account_Code_Desc_ID"].ToString());
                    if (Account_Code_Desc_ID == Convert.ToInt32(ObjS3G_AccountMasterDetails.Rows[0]["Account_Code_Desc_ID"].ToString()))
                    {

                        CheckBox Cbx = (CheckBox)gvIncome.Rows[i].FindControl("CbxIncome");
                        Cbx.Checked = true;
                    }
                }
            }

            if (ObjS3G_AccountMasterDetails.Rows[0]["Is_Active"].ToString() == "True")
                CbxIActive.Checked = true;
            else
                CbxIActive.Checked = false;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }
    }
    /// <summary>
    /// This Method used Get Expenses details in modify mode
    /// </summary>
    private void FunGetEXPDetatils()
    {
        ObjAccountMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            AccountMgtServices.S3G_SYSAD_AccountDetailsRow ObjAccountSetupMasterRow;
            ObjAccountSetupMasterRow = ObjS3G_AccountMasterDetails.NewS3G_SYSAD_AccountDetailsRow();
            ObjAccountSetupMasterRow.Account_Setup_ID = AccountCodeId;
            ObjAccountSetupMasterRow.Account_Tab_ID = 4;
            ObjAccountSetupMasterRow.Company_ID = intCompanyID;
            ObjS3G_AccountMasterDetails.AddS3G_SYSAD_AccountDetailsRow(ObjAccountSetupMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteAccSetUpDetails = ObjAccountMasterClient.FunPubGetAccountDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_AccountMasterDetails, SerMode));
            ObjS3G_AccountMasterDetails = (AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable)ClsPubSerialize.DeSerialize(byteAccSetUpDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable));

            if (ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString() != "" || ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString() != "0")
            {
                //Added by Thangam M on 20/Mar/2012 to load inactive LOB's
                ListItem ListLOB = new ListItem(Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB"]), Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"]), true);
                ListLOB.Selected = false;
                if (ddlELOB.Items.FindByValue(Convert.ToString(ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"])) == null)
                {
                    ddlELOB.Items.Add(ListLOB);
                }
                ddlELOB.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString();
            }

            FunPriGetBranchList();

            //ddlELOB.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["LOB_ID"].ToString();
            ddlEBranch.SelectedValue = ObjS3G_AccountMasterDetails.Rows[0]["Location_ID"].ToString();
            if (trE.Visible)
            {
                txtGLCodeSearchE.Text = ObjS3G_AccountMasterDetails.Rows[0]["GL_CODE"].ToString();
                txtSLCodeSearchE.Text = ObjS3G_AccountMasterDetails.Rows[0]["SL_CODE"].ToString();
            }
            else if (trOldI.Visible)
            {
                txtEGLAC.Text = ObjS3G_AccountMasterDetails.Rows[0]["GL_CODE"].ToString();
                txtESLAC.Text = ObjS3G_AccountMasterDetails.Rows[0]["SL_CODE"].ToString();
            }
           
            hdnID.Value = ObjS3G_AccountMasterDetails.Rows[0]["Created_By"].ToString();
            for (int i = 0; i < gvExpress.Rows.Count; i++)
            {
                if (gvExpress.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    int Account_Code_Desc_ID = Convert.ToInt32(gvExpress.DataKeys[i]["Account_Code_Desc_ID"].ToString());
                    if (Account_Code_Desc_ID == Convert.ToInt32(ObjS3G_AccountMasterDetails.Rows[0]["Account_Code_Desc_ID"].ToString()))
                    {

                        CheckBox Cbx = (CheckBox)gvExpress.Rows[i].FindControl("CbxExpress");
                        Cbx.Checked = true;
                    }
                }
            }

            if (ObjS3G_AccountMasterDetails.Rows[0]["Is_Active"].ToString() == "True")
                CbxEActive.Checked = true;
            else
                CbxEActive.Checked = false;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjAccountMasterClient.Close();
        }

    }


    #endregion

    /// <summary>
    /// Onclick Redirect to view page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAssetCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx?qsMode=Ass");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnLiailityCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx?qsMode=Lib");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnIncomeCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx?qsMode=Inc");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnExpressCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../System Admin/S3G_SysAdminAccountSetupMaster_View.aspx?qsMode=Exp");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #region Clear
    /// <summary>
    /// On click clear the all enterable fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAssetClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtGLAC.Text = "";
            txtSLAC.Text = "";
            ddlBranch.SelectedIndex = 0;
            ddlLOB.SelectedIndex = 0;
            CbxActive.Checked = true;
            for (int i = 0; i < gvAsset.Rows.Count; i++)
            {
                if (gvAsset.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    CheckBox Cbx = (CheckBox)gvAsset.Rows[i].FindControl("CbxAsset");
                    Cbx.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    protected void btnLiaibityClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtLGLAC.Text = "";
            txtLSLAC.Text = "";
            ddlLLOB.SelectedIndex = 0;
            ddlLBranch.SelectedIndex = 0;
            CbxLActive.Checked = true;
            for (int i = 0; i < gvLiability.Rows.Count; i++)
            {
                if (gvLiability.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    CheckBox Cbx = (CheckBox)gvLiability.Rows[i].FindControl("CbxLibility");
                    Cbx.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnInComeClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtIGLAC.Text = "";
            txtISLAC.Text = "";
            ddlIBranch.SelectedIndex = 0;
            ddlILob.SelectedIndex = 0;
            CbxIActive.Checked = true;
            for (int i = 0; i < gvIncome.Rows.Count; i++)
            {
                if (gvIncome.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    CheckBox Cbx = (CheckBox)gvIncome.Rows[i].FindControl("CbxIncome");
                    Cbx.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnExpressClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtEGLAC.Text = "";
            txtESLAC.Text = "";
            ddlEBranch.SelectedIndex = 0;
            ddlELOB.SelectedIndex = 0;
            CbxEActive.Checked = true;
            for (int i = 0; i < gvExpress.Rows.Count; i++)
            {
                if (gvExpress.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    CheckBox Cbx = (CheckBox)gvExpress.Rows[i].FindControl("CbxExpress");
                    Cbx.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion
    protected void tcAcctCreation_ActiveTabChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMessage.Text = "";
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void gvAsset_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gvAsset = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("CbxAsset");
                chkBoxSelect.Attributes["onclick"] = string.Format
                                          (
                                             "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                             gvAsset.ClientID
                                         );


            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    protected void gvLiability_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gvLiability = (GridView)sender;


            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("CbxLibility");
                chkBoxSelect.Attributes["onclick"] = string.Format
                                          (
                                             "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                             gvLiability.ClientID
                                         );


            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void gvIncome_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gvIncome = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("CbxIncome");
                chkBoxSelect.Attributes["onclick"] = string.Format
                                          (
                                             "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                             gvIncome.ClientID
                                         );


            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    protected void gvExpress_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gvExpress = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("CbxExpress");
                chkBoxSelect.Attributes["onclick"] = string.Format
                                          (
                                             "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                             gvExpress.ClientID
                                         );


            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }


    #region FieldDisable
    /// <summary>
    /// This Methode used in Role Access
    /// </summary>
    /// <param name="intModeId"></param>
    private void FunPriAssetTabControlStatus(int intModeId)
    {
        try
        {
            if (intModeId != 0)
            {
                FunGetAssetDetatils();
            }

            if (intModeId == 0)
            {
                if (!bCreate)
                {
                    btnAssetSave.Enabled = false;
                }
                CbxActive.Checked = true;
                CbxActive.Enabled = false;
                tbLib.Enabled = false;
                tbIncome.Enabled = false;
                tbExpress.Enabled = false;
                rfvLGLAC.Visible = false;
                rfvIGLAC.Visible = false;
                rfvEGLAC.Visible = false;
            }
            else if (intModeId == 1)
            {
                if (!bModify)
                {
                    btnAssetSave.Enabled = false;
                }
                CbxActive.Enabled = true;
                tbLib.Enabled = false;
                tbIncome.Enabled = false;
                tbExpress.Enabled = false;
                rfvLGLAC.Visible = false;
                rfvIGLAC.Visible = false;
                rfvEGLAC.Visible = false;
                btnAssetClear.Enabled = false;
            }
            else if (intModeId == -1)
            {
                ListItem lit;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                txtGLAC.ReadOnly = true;
                txtSLAC.ReadOnly = true;
                txtSLCodeSearchA.ReadOnly = txtGLCodeSearchA.ReadOnly = true;
                CbxActive.Enabled = false;
                btnAssetClear.Enabled = false;
                btnAssetSave.Enabled = false;
                gvAsset.FooterRow.Visible = false;
                lit = new ListItem(ddlLOB.SelectedItem.Text, ddlLOB.SelectedItem.Value);
                ddlLOB.Items.Clear();
                ddlLOB.Items.Add(lit);
                lit = new ListItem(ddlBranch.SelectedItem.Text, ddlBranch.SelectedItem.Value);
                ddlBranch.Items.Clear();
                ddlBranch.Items.Add(lit);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLiabilityTabControlStatus(int intModeId)
    {
        try
        {
            if (intModeId != 0)
            {
                FunGetLIBDetatils();
            }

            if (intModeId == 0)
            {
                if (!bCreate)
                {
                    btnLiailitySave.Enabled = false;
                }
                CbxLActive.Checked = true;
                CbxLActive.Enabled = false;
                tbAsset.Enabled = false;
                tbIncome.Enabled = false;
                tbExpress.Enabled = false;
                rfvGLAC.Visible = false;
                rfvIGLAC.Visible = false;
                rfvEGLAC.Visible = false;
                tcAcctCreation.ActiveTabIndex = 1;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

            }
            else if (intModeId == 1)
            {
                if (!bModify)
                {
                    btnLiailitySave.Enabled = false;
                }
                btnLiaibityClear.Enabled = false;
                CbxLActive.Enabled = true;
                tbAsset.Enabled = false;
                tbIncome.Enabled = false;
                tbExpress.Enabled = false;
                rfvGLAC.Visible = false;
                rfvIGLAC.Visible = false;
                rfvEGLAC.Visible = false;
                tcAcctCreation.ActiveTabIndex = 1;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
            }
            else if (intModeId == -1)
            {
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                ListItem lit;
                CbxLActive.Enabled = true;
                txtLGLAC.ReadOnly = true;
                txtLSLAC.ReadOnly = true;
                txtSLCodeSearchL.ReadOnly = txtGLCodeSearchL.ReadOnly = true;
                CbxLActive.Enabled = false;
                btnLiaibityClear.Enabled = false;
                btnLiailitySave.Enabled = false;
                gvLiability.FooterRow.Visible = false;
                lit = new ListItem(ddlLLOB.SelectedItem.Text, ddlLLOB.SelectedItem.Value);
                ddlLLOB.Items.Clear();
                ddlLLOB.Items.Add(lit);
                lit = new ListItem(ddlLBranch.SelectedItem.Text, ddlLBranch.SelectedItem.Value);
                ddlLBranch.Items.Clear();
                ddlLBranch.Items.Add(lit);
                tcAcctCreation.ActiveTabIndex = 1;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriIncomeTabControlStatus(int intModeId)
    {
        try
        {

            if (intModeId != 0)
            {
                FunGetINCDetatils();
            }

            if (intModeId == 0)
            {
                if (!bCreate)
                {
                    btnIncomeSave.Enabled = false;
                }
                tcAcctCreation.ActiveTabIndex = 2;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                CbxIActive.Checked = true;
                CbxIActive.Enabled = false;
                tbLib.Enabled = false;
                tbAsset.Enabled = false;
                tbExpress.Enabled = false;
                rfvGLAC.Visible = false;
                rfvLGLAC.Visible = false;
                rfvEGLAC.Visible = false;

            }
            else if (intModeId == 1)
            {
                if (!bModify)
                {
                    btnIncomeSave.Enabled = false;
                }
                tcAcctCreation.ActiveTabIndex = 2;
                CbxIActive.Enabled = true;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnInComeClear.Enabled = false;
                tbLib.Enabled = false;
                tbAsset.Enabled = false;
                tbExpress.Enabled = false;
                rfvGLAC.Visible = false;
                rfvLGLAC.Visible = false;
                rfvEGLAC.Visible = false;
            }
            else if (intModeId == -1)
            {
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                ListItem lit;
                txtIGLAC.ReadOnly = true;
                txtISLAC.ReadOnly = true;
                txtSLCodeSearchI.ReadOnly = txtGLCodeSearchI.ReadOnly = true;
                CbxIActive.Enabled = false;
                btnInComeClear.Enabled = false;
                btnIncomeSave.Enabled = false;
                gvIncome.FooterRow.Visible = false;
                lit = new ListItem(ddlILob.SelectedItem.Text, ddlILob.SelectedItem.Value);
                ddlILob.Items.Clear();
                ddlILob.Items.Add(lit);
                lit = new ListItem(ddlIBranch.SelectedItem.Text, ddlIBranch.SelectedItem.Value);
                ddlIBranch.Items.Clear();
                ddlIBranch.Items.Add(lit);
                tcAcctCreation.ActiveTabIndex = 2;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    private void FunPriExpenseTabControlStatus(int intModeId)
    {
        try
        {
            if (intModeId != 0)
            {
                FunGetEXPDetatils();
            }

            if (intModeId == 0)
            {
                if (!bCreate)
                {
                    btnExpressSave.Enabled = false;
                }
                tcAcctCreation.ActiveTabIndex = 3;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                CbxEActive.Checked = true;
                CbxEActive.Enabled = false;
                tbLib.Enabled = false;
                tbAsset.Enabled = false;
                tbIncome.Enabled = false;
                rfvGLAC.Visible = false;
                rfvLGLAC.Visible = false;
                rfvIGLAC.Visible = false;

            }
            else if (intModeId == 1)
            {
                if (!bModify)
                {
                    btnExpressSave.Enabled = false;
                }
                tcAcctCreation.ActiveTabIndex = 3;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnExpressClear.Enabled = false;
                CbxEActive.Enabled = true;
                tbLib.Enabled = false;
                tbAsset.Enabled = false;
                tbIncome.Enabled = false;
                rfvGLAC.Visible = false;
                rfvLGLAC.Visible = false;
                rfvIGLAC.Visible = false;
            }
            else if (intModeId == -1)
            {
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                ListItem lit;
                txtEGLAC.ReadOnly = true;
                txtESLAC.ReadOnly = true;
                txtSLCodeSearchE.ReadOnly = txtGLCodeSearchE.ReadOnly = true;
                CbxEActive.Enabled = false;
                btnExpressSave.Enabled = false;
                btnExpressClear.Enabled = false;
                gvExpress.FooterRow.Visible = false;
                lit = new ListItem(ddlELOB.SelectedItem.Text, ddlELOB.SelectedItem.Value);
                ddlELOB.Items.Clear();
                ddlELOB.Items.Add(lit);
                lit = new ListItem(ddlEBranch.SelectedItem.Text, ddlEBranch.SelectedItem.Value);
                ddlEBranch.Items.Clear();
                ddlEBranch.Items.Add(lit);
                tcAcctCreation.ActiveTabIndex = 3;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriDisableControls(int intModeID, string qsTab)
    {
        try
        {
            switch (qsTab)
            {
                case "Asset":
                    {
                        FunPriAssetTabControlStatus(intModeID);
                        break;
                    }
                case "Liability":
                    {
                        FunPriLiabilityTabControlStatus(intModeID);
                        break;
                    }
                case "Income":
                    {
                        FunPriIncomeTabControlStatus(intModeID);
                        break;
                    }
                case "Express":
                    {
                        FunPriExpenseTabControlStatus(intModeID);
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion
    #region databound
    protected void gvAsset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox Chebox = (CheckBox)e.Row.FindControl("CbxAsset");
                if (!bModify && strMode == "Q")
                {
                    Chebox.Enabled = false;
                }
                if (strMode == "Q")
                {
                    Chebox.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void gvLiability_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox Chebox = (CheckBox)e.Row.FindControl("CbxLibility");
                if (!bModify && strMode == "Q")
                {
                    Chebox.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void gvIncome_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox Chebox = (CheckBox)e.Row.FindControl("CbxIncome");
                if (!bModify && strMode == "Q")
                {
                    Chebox.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void gvExpress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox Chebox = (CheckBox)e.Row.FindControl("CbxExpress");
                if (!bModify && strMode == "Q")
                {
                    Chebox.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    private bool FunCheckLobisvalid(string strLobId)
    {
        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm.Add("@Company_ID", intCompanyID.ToString());
        Procparm.Add("@User_Id", intUserID.ToString());
        Procparm.Add("@LOB_ID", strLobId);
        Procparm.Add("@Program_ID", "9");
        if (AccountCodeId == 0)
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

    private bool FunCheckBranchisvalid(string strBranchId)
    {
        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm.Add("@Company_ID", intCompanyID.ToString());
        Procparm.Add("@User_Id", intUserID.ToString());
        Procparm.Add("@Location_ID", strBranchId);
        if (AccountCodeId == 0)
        {
            Procparm.Add("@Is_Active", "1");
        }
        Procparm.Add("@Program_ID", "9");
        Procparm.Add("@Is_UserLobActive", "1");
        DataTable dtBool = Utility.GetDefaultData("S3G_GetUserBranchMapping", Procparm);
        if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
            return true;
        else
            return false;
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriGetBranchList();
    }

    #region "INTEGRATED FA pART"
    [System.Web.Services.WebMethod]
    public static string[] GetGLCodeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@Type", System.Web.HttpContext.Current.Session["AutoSuggestqsTab"].ToString());
        if (System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"] != null)
        {
            Procparam.Add("@GL_Code", System.Web.HttpContext.Current.Session["AutoSuggestGLCodeSearch"].ToString());
        }
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("FAS3G_SYSAD_GET_GLCode", Procparam));
        return suggetions.ToArray();
    }

    //ASSET
    protected void txtGLCodeSearchA_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtSLCodeSearchA.Text = string.Empty;
            string strhdnValue = hdnGLCodeSearchIDA.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtGLCodeSearchA.Text = string.Empty;
                hdnGLCodeSearchIDA.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Account Setup");
        }
    }

    protected void txtSLCodeSearchA_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnSLCodeSearchIDA.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtSLCodeSearchA.Text = string.Empty;
                hdnSLCodeSearchIDA.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Account Setup");
        }
    }
    //LIABILITY
    protected void txtGLCodeSearchL_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtSLCodeSearchL.Text = string.Empty;
            string strhdnValue = hdnGLCodeSearchIDL.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtGLCodeSearchL.Text = string.Empty;
                hdnGLCodeSearchIDL.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Account Setup");
        }
    }

    protected void txtSLCodeSearchL_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnSLCodeSearchIDL.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtSLCodeSearchL.Text = string.Empty;
                hdnSLCodeSearchIDL.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Account Setup");
        }
    }
    //INCOME
    protected void txtGLCodeSearchI_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtSLCodeSearchI.Text = string.Empty;
            string strhdnValue = hdnGLCodeSearchIDI.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtGLCodeSearchI.Text = string.Empty;
                hdnGLCodeSearchIDI.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Account Setup");
        }
    }

    protected void txtSLCodeSearchI_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnSLCodeSearchIDI.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtSLCodeSearchI.Text = string.Empty;
                hdnSLCodeSearchIDI.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Account Setup");
        }
    }
    //EXPENSE
    protected void txtGLCodeSearchE_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtSLCodeSearchE.Text = string.Empty;
            string strhdnValue = hdnGLCodeSearchIDE.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtGLCodeSearchE.Text = string.Empty;
                hdnGLCodeSearchIDE.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Account Setup");
        }
    }

    protected void txtSLCodeSearchE_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnSLCodeSearchIDE.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtSLCodeSearchE.Text = string.Empty;
                hdnSLCodeSearchIDE.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Account Setup");
        }
    }



    private void FunsetIntegratedCtrls(string strtab)
    {

        Dictionary<string, string> Procparam = new Dictionary<string, string>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        DataTable dt = Utility.GetDefaultData("FAS3G_GET_GPS_INTEGRATED", Procparam);
        bool blnflag = false;
        if (dt.Rows.Count > 0)
        {
            blnflag = true;
        }

        switch (strtab)
        {
            case "1":
                trA.Visible =
                    RFVtxtGLCodeSearchA.Enabled = blnflag;
                trOldA.Visible = !blnflag;

                break;
            case "2":
                trL.Visible =
                    RFVtxtGLCodeSearchL.Enabled = blnflag;
                trOldL.Visible = !blnflag;
                break;
            case "3":
                trI.Visible =
                    RFVtxtGLCodeSearchI.Enabled = blnflag;
                trOldI.Visible = !blnflag;
                break;
            case "4":
                trE.Visible =
                    RFVtxtGLCodeSearchE.Enabled = blnflag;
                trOldE.Visible = !blnflag;
                break;
        }
    }


    #endregion
}