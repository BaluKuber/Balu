/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name               : Legal Repossession
/// Screen Name               : Legal Repossession Note
/// Created By                : NARASIMHA RAO.P
/// Created Date              : 22-Apr-2011
/// Purpose                   : 
/// Last Updated By           : NARASIMHA RAO.P
/// Last Updated Date         : 10 -May-2011
/// Reason                    :
/// Last Updated By		      :   Thalaiselvam N
/// Last Updated Date         :   20-Sep-2011
/// Reason                    :   Encrypted Password Validation
/// <Program Summary>
#region "Namespaces"

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using S3GBusEntity;
    using LEGAL = S3GBusEntity.LegalRepossession;
    using LEGALSERVICES = LegalAndRepossessionMgtServicesReference;
    using System.IO;
    using System.Globalization;
    using System.Web.Services;
    using System.ServiceModel;
    using System.Text;

#endregion
public partial class LegalRepossession_S3GLRRepossessionNote_Add : ApplyThemeForProject
{
    #region [Common Variable declaration]
    DataTable dtLRDetails;
    int intCompanyId, intUserId = 0;
    int intLRNId = 0;
    string updateStatus = "U";
    static int intCustomer = 0;

    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
   
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    string strRedirectPage = "~/LegalRepossession/S3gLRTransLander.aspx?Code=GRN";
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRRepossessionNote_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GRN';";

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;

    public static LegalRepossession_S3GLRRepossessionNote_Add obj_Page;
    //Code end

    LEGALSERVICES.LegalAndRepossessionMgtServicesClient objLegalRepossession_Client;

    LEGAL.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteDataTable objS3G_LR_RepossessionNoteDataTable = null;
    LEGAL.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteRow objS3G_LR_RepossessionNoteDataRow = null;
    //AssetMgtServicesReference.AssetMgtServicesClient objAssetIdentification_Client;
   
    #endregion

    #region [Page Load Event]
    protected void Page_Load(object sender, EventArgs e)
    {
        bool isAllowEdit = true;
        try
        {

            S3GSession ObjS3GSession = new S3GSession();
            
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            //txtLegalCutOffDate.Attributes.Add("readonly", "readonly");
            //Date Format
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

            //Code end
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
                    
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intLRNId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid LRN Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            obj_Page = this;

            if (!IsPostBack)
            {
               
                FunToolTip();
                FunPriLoadLOV();
                if (ddlLOB.Items.Count == 2)
                {
                    ddlLOB.SelectedIndex = 1;
                    ddlLOB_SelectedIndexChanged(sender, e);
                }
              
                txtLRDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                txtApprovalDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                
                if (intLRNId > 0)
                {
                    isAllowEdit = FunPubProGetLRNDetails(intCompanyId, intLRNId);
                }
                if (strMode == "Q")
                {
                    FunPriDisableControls(-1);
                }
                else if (strMode == "M")
                {
                    if (isAllowEdit)
                    {
                        FunPriDisableControls(1);
                    }
                    else
                    {
                        FunPriDisableControls(-1);
                    }
                }
                else
                {
                    FunPriDisableControls(0);
                }
               
            }
        }
        catch (Exception objExp)
        {
            //cvLRNote.IsValid = false;
            //cvLRNote.ErrorMessage = "Unable to load Legal Repossession Note Details due to data problem";
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    #region [DropDownListEvents]

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunClearControls(true);
            //ddlBranch.SelectedIndex = 0;
            ViewState["GetDeliquencyData"] = null;
            PopulatePANum();

            ddlLOB.Focus();
            //if (ddlBranch.Items.Count == 2)
            //{
            //    ddlBranch.SelectedIndex = 1;
            //    ddlBranch_SelectedIndexChanged(sender, e);
            //}
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@OPTION", "2");
            Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
            Procparam.Add("@USERID", Convert.ToString(intUserId));
            ddlAction.BindDataTable(SPNames.S3G_LR_LOADLOV, Procparam, new string[] { "LOOKUP_CODE", "LOOKUP_DESCRIPTION" });
            string strLobValue = ddlLOB.SelectedItem.Text;
            if (!strLobValue.Contains("HP") && !strLobValue.Contains("FL") && !strLobValue.Contains("LN") && !strLobValue.Contains("OL"))
            {
                ddlAction.SelectedIndex = 1;
                ddlAction.ClearDropDownList();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClearControls(true);
        ViewState["GetDeliquencyData"] = null;
        //PopulatePANum();
        ddlBranch.Focus();
        //if (ddlMLA.Items.Count == 2)
        //{
        //    ddlMLA.SelectedIndex = 1;
        //    ddlMLA_SelectedIndexChanged(sender, e);
        //}
    }


    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMLA.SelectedValue != "0")
        {
            FunGetDeliquencyData();
            PopulateSANum(ddlMLA.SelectedValue);
            PopulatePANCustomer(ddlMLA.SelectedValue);
            btnAccount.Visible = true;
        }
        else
        {
            ddlSLA.Items.Clear();
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSLA.Items.Insert(0, liSelect);
            S3GCustomerAddress1.ClearCustomerDetails();
        }
        ddlMLA.Focus();
    }
    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSLA.Focus();
        ProcessMappedLRN(true);
    }
    protected void ddlFollowUPEMPID_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFlUpEmpName.Text = GetFollowUpEmpName();
    }
   

    #endregion

    #region [UserDefined Functions]
   
    private void FunPriLoadLOV()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            /********** LOAD LOB *************/
            //Procparam.Add("@OPTION", "1");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            if (strMode == "C")
                Procparam.Add("@Is_Active", "1");
            //Procparam.Add("@FilterOption", "'HP','LN','OL','FL'");
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            Procparam.Add("@Program_ID", "152");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            /********** LOAD BRANCH *************/
            Procparam.Clear();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            if (strMode == "C")
                Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            Procparam.Add("@Program_ID", "152");
            //ddlBranch.BindDataTable(SPNames.S3G_LR_LOADLOV, Procparam, new string[] { "BRANCH_ID", "BRANCH_CODE", "BRANCH_NAME" });
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
            /********** LOAD ACTION *************/
            Procparam.Clear();
            Procparam.Add("@OPTION", "2");
            Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
            Procparam.Add("@USERID", Convert.ToString(intUserId));
            ddlAction.BindDataTable(SPNames.S3G_LR_LOADLOV, Procparam, new string[] { "LOOKUP_CODE", "LOOKUP_DESCRIPTION" });
            Procparam.Clear();

          
            if (strMode == string.Empty || strMode == "C")
            {
                txtLRStatus.Text = "Pending";
            }
            /********** LOAD Approval Status *************/
            Procparam.Add("@OPTION", "4");
            Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
            Procparam.Add("@USERID", Convert.ToString(intUserId));
            ddlStatus.BindDataTable(SPNames.S3G_LR_LOADLOV, Procparam, new string[] { "LOOKUP_CODE", "LOOKUP_DESCRIPTION" });
            Procparam.Clear();

            /********** LOAD FOLLOWUP EMP ID *************/
            Procparam.Add("@OPTION", "5");
            Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
            Procparam.Add("@USERID", Convert.ToString(intUserId));
            //ddlFollowUPEMPID.BindDataTable(SPNames.S3G_LR_LOADLOV, Procparam, new string[] { "USER_ID", "USER_CODE" });
            Procparam.Clear();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Program_Id", "141");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }
    protected void FunClearControls(bool ClearPAN)
    {
        try
        {
            if (ClearPAN)
            {
                ddlMLA.Clear();
            }
            InitializeDropDownList(ddlSLA);

            S3GCustomerAddress1.ClearCustomerDetails();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyId));
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@Prefix", prefixText);
        obj_Page.ViewState["GetDeliquencyData"] = Utility.GetDefaultData("S3G_LR_GetDelinquencyPANumSANum_AGT", Procparam);
        suggetions = Utility.GetSuggestions((DataTable)obj_Page.ViewState["GetDeliquencyData"]);

        return suggetions.ToArray();
    }

    private void PopulatePANum()
    {
        //DataSet dsDeliquencyData;
        //DataTable dtDistinctPANumData;
        //string[] TobeDistinct = { "PANum" };
        //try
        //{
        //    if (ddlLOB.SelectedValue != "0" && ddlBranch.SelectedValue != "0")
        //    {
        //        //****** TO GET DELIQUENCY LIST OF PANnum ******//
        //        ddlMLA.Clear();
        //        FunGetDeliquencyData();
        //        dsDeliquencyData = (DataSet)ViewState["GetDeliquencyData"];
        //        if (dsDeliquencyData.Tables.Count > 0)
        //        {
        //            dtDistinctPANumData = dsDeliquencyData.Tables[0].DefaultView.ToTable(true, TobeDistinct);
        //            ddlMLA.DataValueField = "PANum";
        //            ddlMLA.DataTextField = "PANum";
        //            ddlMLA.DataSource = dtDistinctPANumData;
        //            ddlMLA.DataBind();
        //        }
        //        ddlMLA.Items.Insert(0, "--Select--");
        //    }
        //}
        //catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{
        //    throw objFaultExp;
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}


    }
    public void InitializeDropDownList(DropDownList ddlSource)
    {
        ddlSource.Items.Clear();
        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
        ddlSource.Items.Insert(0, liSelect);
    }
    private void PopulateSANum(string strPAN)
    {
        DataSet dsDeliquencyData;
        DataTable dtSANnum;
        try
        {
            ////Procparam = new Dictionary<string, string>();
            ////Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            ////Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            ////Procparam.Add("@Branch_ID ", Convert.ToString(ddlBranch.SelectedValue));
            ////Procparam.Add("@Type", "2");
            ////Procparam.Add("@PANum", ddlMLA.SelectedItem.Text);

            ////ddlSLA.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "SANum", "SANum" });

            //******** To Fetch Deliquency SubAccounts **************///
            if (ViewState["GetDeliquencyData"] != null && ViewState["GetDeliquencyData"] != string.Empty)
            {
                ddlSLA.Items.Clear();
                dsDeliquencyData = (DataSet)ViewState["GetDeliquencyData"];
                if (dsDeliquencyData.Tables.Count > 0)
                {
                    DataView dv = new DataView(dsDeliquencyData.Tables[0], "PANum = '" + strPAN + "'", "panum", DataViewRowState.CurrentRows);
                    dtSANnum = dv.ToTable("TableSANum", true, new string[] { "SANum" });
                    if (dtSANnum.Rows.Count == 1)
                    {
                        if (dtSANnum.Rows[0]["SANum"].ToString().Trim() == string.Empty)
                        {
                            dtSANnum.Rows.RemoveAt(0);
                        }
                    }
                    ddlSLA.DataValueField = "SANum";
                    ddlSLA.DataTextField = "SANum";
                    ddlSLA.DataSource = dtSANnum.DefaultView;
                    ddlSLA.DataBind();
                }
                ddlSLA.Items.Insert(0, "--Select--");
            }

            if (ddlSLA.Items.Count == 1)
            {
                FunClearControls(false);
                lblSLA.CssClass = "styleDisplayLabel";
                rfvSLA.Enabled = false;
                ddlSLA.Enabled = false;
                ProcessMappedLRN(true);
            }
            else if (ddlSLA.Items.Count == 2)
            {
                ddlSLA.Items.RemoveAt(0);
                ddlSLA.SelectedIndex = 0;
                ddlSLA.Enabled = true;
                ProcessMappedLRN(true);
            }
            else
            {
                lblSLA.CssClass = "styleReqFieldLabel";
                rfvSLA.Enabled = true;
                ddlSLA.Enabled = true;
            }


            
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void PopulatePANCustomer(string strPAN)
    {
        objLegalRepossession_Client = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();

        try
        {
          
            DataTable dtTable = new DataTable();
           
            byte[] bytesLRDetails = objLegalRepossession_Client.FunGetPANumCustomer(strPAN);
            dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesLRDetails, ObjSerMode, typeof(DataTable));
            intCustomer = Convert.ToInt32(dtTable.Rows[0]["Customer_ID"].ToString());
            S3GCustomerAddress1.SetCustomerDetails(dtTable.Rows[0], true);

        }
        catch (Exception ex)
        {
            //Commented by Srivatsan for Removing the error
            //lblErrorMessage.Text = ex.ToString();
        }
        finally
        {
            objLegalRepossession_Client.Close();
        }
    }
   
    private string GetFollowUpEmpName()
    {
        //throw new NotImplementedException();

        //DataTable dtTable = new DataTable();
        //if (Procparam != null)
        //    Procparam.Clear();
        //else
        //    Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        //Procparam.Add("@User_ID", Convert.ToString(intUserId));
        //Procparam.Add("@FollowUp_ID", Convert.ToString(ddlFollowUPEMPID.SelectedValue));

        //dtTable = Utility.GetDefaultData(SPNames.S3G_LR_GetFollowUpUserName, Procparam);
        //if (dtTable != null && dtTable.Rows.Count > 0)
        //    return dtTable.Rows[0]["FollowUp_UserName"].ToString();
        //else
            return string.Empty;
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

        Procparam.Add("@CompanyID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@USERID", obj_Page.intUserId.ToString());
        Procparam.Add("@OPTION", "8");
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LR_LOADLOV", Procparam));

        return suggetions.ToArray();
    }

    //end here//
    private void FunToolTip()
    {
        //throw new NotImplementedException();
        ddlBranch.ToolTip = lblBranch.Text;
        ddlLOB.ToolTip = lblLOB.Text;
        ddlAction.ToolTip = lblAction.Text;
        ddlSLA.ToolTip = lblSLA.Text;
        ddlMLA.ToolTip = lblMLA.Text;
        txtLRNo.ToolTip = lblLRNo.Text;
        txtLRDate.ToolTip = lblLRDate.Text;
        txtActionPoints.ToolTip = lblActionPoints.Text;
        ddlFollowUPEMPID.ToolTip = lblFlUpEmpID.Text;
        txtFlUpEmpName.ToolTip = lblFlUpEmpName.Text;
        txtLRStatus.ToolTip = lblLRStatus.Text;
        
        txtApprovalName.ToolTip = lblApprovalName.Text;
        ddlStatus.ToolTip = lblStatus.Text;
        txtApprovalDate.ToolTip = lblApprovalDate.Text;
        txtPassword.ToolTip = lblPassword.Text;
        txtRemarks.ToolTip = lblRemarks.Text;
        btnSave.ToolTip = btnSave.Text;
        btnClear.ToolTip = btnClear.Text;
        btnCancel.ToolTip = btnCancel.Text;
        
    }
    protected void FunPriDisableControls(int intModeID)
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

                    txtLRDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    tpLRApproval.Visible = false;
                    break;

                case 1: // Modify Mode

                   lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                    //btnRevoke.Visible = true;
                    if (!bModify)
                    {
                        btnSave.Visible = false;
                       // btnRevoke.Enabled = false;
                    }
                    ddlSLA.Enabled = ddlMLA.Enabled =ddlFollowUPEMPID.Enabled= ddlLOB.Enabled = ddlBranch.Enabled = btnClear.Enabled = false;
                    lblSLA.CssClass = "styleDisplayLabel";
                    rfvSLA.Enabled = false;
                    btnSave.Text = "Save";
                    btnSave.ToolTip = "Save";
                    btnAccount.Enabled = true;
                    if (ObjUserInfo.ProUserLevelIdRW == 5 || ObjUserInfo.ProUserLevelIdRW == 4 || ObjUserInfo.ProUserLevelIdRW == 3)
                    {
                        tpLRApproval.Visible = true;
                        pnlApprovalHistory.Visible = true;
                        pnlLRNoteApproval.Visible = true;
                    }
                    else
                        tpLRApproval.Visible = false;
                    
                    break;

                case -1:// Query Mode
                    if (strMode == "Q")
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    else if(strMode == "M")
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                    btnSave.Enabled = btnClear.Enabled = false;
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }

                    if (bClearList)
                    {
                        //ddlBranch.ClearDropDownList();
                        ddlLOB.ClearDropDownList();
                        ddlMLA.ReadOnly = true;
                        ddlSLA.ClearDropDownList();
                        ddlAction.ClearDropDownList();
                        //ddlFollowUPEMPID.ClearDropDownList();
                      
                    }
                    txtActionPoints.ReadOnly = txtFlUpEmpName.ReadOnly = ddlFollowUPEMPID.ReadOnly=ddlBranch.ReadOnly= true;
                    tpLRApproval.Visible = true;
                    pnlApprovalHistory.Visible = true;
                    pnlLRNoteApproval.Visible = false;
                    break;
            }
        }
        catch (Exception ex)
        {
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            //cvLRNote.IsValid = false;  
            //cvLRNote.ErrorMessage = "Unable to set the conrols";
            //throw new ApplicationException("Unable to set the controls");
        }
    }
    private bool FunPubProGetLRNDetails(int intCompanyId, int intLRNId)
    {
        bool isAllowEdit = true;
        objLegalRepossession_Client = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();
        try
        {
            DataSet dsLRNDetails;
           
            byte[] byte_LRNDetails = objLegalRepossession_Client.FunPubQueryLRNDetails(intCompanyId, intLRNId);
            dsLRNDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_LRNDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@CompanyId", Convert.ToString(intCompanyId));
            Procparam.Add("@LRN_ID", Convert.ToString(intLRNId));

            DataSet dsLRNAPPROVALDetails = Utility.GetDataset(SPNames.S3G_LR_getLRApprovalDetails, Procparam);

            //DataSet dsGetApprovalDetails = Utility.GetDataset("",);
            if (dsLRNDetails.Tables.Count > 0 && intCompanyId > 0)
            {
                DataTable dtLRNTable = dsLRNDetails.Tables[0];
                txtLRDate.Text = DateTime.Parse(dtLRNTable.Rows[0]["LRN_Date"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
                ddlLOB.SelectedValue = dtLRNTable.Rows[0]["LOB_ID"].ToString();
                ddlLOB.ClearDropDownList();
                ddlBranch.SelectedValue = dtLRNTable.Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = dtLRNTable.Rows[0]["Location_Name"].ToString();
                //ddlBranch.ClearDropDownList();
                ddlAction.SelectedValue = dtLRNTable.Rows[0]["Action_Code"].ToString();
                //ddlAction.ClearDropDownList();

                ddlMLA.SelectedValue = dtLRNTable.Rows[0]["PANum"].ToString();
                ddlMLA.SelectedText = dtLRNTable.Rows[0]["PANum"].ToString();
                //ddlMLA.Items.Add(new ListItem(dtLRNTable.Rows[0]["PANum"].ToString(), dtLRNTable.Rows[0]["PANum"].ToString()));
                //ddlMLA.SelectedValue = dtLRNTable.Rows[0]["PANum"].ToString();

                if (!dtLRNTable.Rows[0]["SANum"].ToString().Contains("DUMMY"))
                {
                    ddlSLA.Items.Add(new ListItem(dtLRNTable.Rows[0]["SANum"].ToString(), dtLRNTable.Rows[0]["SANum"].ToString()));
                    ddlSLA.SelectedValue = dtLRNTable.Rows[0]["SANum"].ToString();
                }
                else
                {
                    ddlSLA.Items.Add(new ListItem("--Select--", "0"));
                    ddlSLA.SelectedValue = "0";
                }
                if (ddlMLA.SelectedValue != "0")
                {
                    btnAccount.Visible = true;
                }
                txtActionPoints.Text = dtLRNTable.Rows[0]["Action_Point"].ToString();
                ddlFollowUPEMPID.SelectedValue = dtLRNTable.Rows[0]["Followup_Employee_ID"].ToString();
                ddlFollowUPEMPID.SelectedText = dtLRNTable.Rows[0]["FollowUp_Name"].ToString();
                txtFlUpEmpName.Text = GetFollowUpEmpName();
                string lr_status = dtLRNTable.Rows[0]["LR_Status_Code"].ToString();
                txtLRStatus.Text = lr_status == "1" ? "Pending" : (lr_status == "2" ? "Under Process" : (lr_status == "3" ? "Approved" : "Rejected"));
                intCustomer = Convert.ToInt32(dtLRNTable.Rows[0]["Customer_ID"].ToString());
                S3GCustomerAddress1.SetCustomerDetails(dtLRNTable.Rows[0], true);
                TextBox txtCustName = (TextBox)S3GCustomerAddress1.FindControl("txtCustomerName");
                txtLRNo.Text = dtLRNTable.Rows[0]["LRN_NO"].ToString();
                hidLRNNo.Value = dtLRNTable.Rows[0]["LRN_NO"].ToString();
                ViewState["Activated_By"] = dtLRNTable.Rows[0]["Created_By"].ToString();
                txtApprovalName.Text = ObjUserInfo.ProUserNameRW;
                isAllowEdit = AllowApproval(dtLRNTable.Rows[0]["LR_Status_Code"].ToString());
                if (!ddlLOB.SelectedItem.Text.Contains("HP") && !ddlLOB.SelectedItem.Text.Contains("FL") && !ddlLOB.SelectedItem.Text.Contains("LN") && !ddlLOB.SelectedItem.Text.Contains("OL"))
                    ddlAction.ClearDropDownList();
                
                CheckAccountHasAsset();
                //if (dtLRNTable.Rows[0]["Mapped_LRN_No"].ToString() != string.Empty)
                //{
                //    ddlAction.Enabled = false;
                //    lnkBtnViewExistingLRN.Text = dtLRNTable.Rows[0]["Mapped_LRN_No"].ToString();
                //    lblMappedLRNId.Text = dtLRNTable.Rows[0]["Mapped_LRN_ID"].ToString();
                //}
            }
            if (dsLRNAPPROVALDetails.Tables.Count > 0 && intCompanyId > 0)
            {
                if (dsLRNAPPROVALDetails.Tables[0].Rows.Count > 0)
                {
                    gvApprovalHistory.DataSource = dsLRNAPPROVALDetails.Tables[0];
                    gvApprovalHistory.DataBind();
                    if (strMode != "Q")
                    {
                        btnEmail.Visible = true;
                        btnSMS.Visible = true;
                    }
                }
            }
            if (gvApprovalHistory.Rows.Count == 0)
            {
                lblGuarDetails.Visible = true;
            }
            else
            {
                lblGuarDetails.Visible = false;
            }
            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Legal Repossession Note details");
        }
        return isAllowEdit;
    }
    private int ISValidate()
    {
        if (intCustomer == 0)
            return 1;
        else
            return 0;
    }
    private void FunGetDeliquencyData()
    {
        if (ViewState["GetDeliquencyData"] == null || ViewState["GetDeliquencyData"] == string.Empty)
        {
            //string getMonthYear = FunGetMonthYear();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            //Procparam.Add("@DelinqDate", System.DateTime.Now.ToString("dd/MMM/yyyy"));
            //Procparam.Add("@MonthYear", getMonthYear);
            ViewState["GetDeliquencyData"] = Utility.GetDataset(SPNames.S3G_LR_GetDelinquencyPANumSANum, Procparam);
        }
    }
    private string FunGetMonthYear()
    {
        DataSet dsMonthYear = new DataSet();
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@Branch_ID", Convert.ToString(ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        dsMonthYear = Utility.GetDataset(SPNames.S3G_CLN_GetDelinquencyMonth, Procparam);

        if (dsMonthYear.Tables.Count > 0)
        {
            if (dsMonthYear.Tables[0].Rows.Count > 0)
                return dsMonthYear.Tables[0].Rows[0]["Demand_Month"].ToString();
            else
                return string.Empty;
        }

        return string.Empty;
    }
    private bool AllowApproval(string approvalId)
    {
        int approvalcode = int.Parse(approvalId);
        if (approvalcode > 2)
        {
            pnlLRNoteApproval.Enabled = false;
            return false;
        }
        return true;
    }
    private void ProcessMappedLRN(bool showAlert)
    {
        objLegalRepossession_Client = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();

        if (strMode != "Q")
        {
            lnkBtnViewExistingLRN.Text = string.Empty;
            lblMappedLRNId.Text = string.Empty;
            btnSave.Enabled = true;
            int lobId = 0;
            int branchId = 0;
            string paNum = string.Empty;
            string saNum = string.Empty;
            ddlAction.Enabled = true;
            try
            {

                if (ddlLOB.SelectedIndex != -1)
                    lobId = Int32.Parse(ddlLOB.SelectedValue);

                if (ddlBranch.SelectedValue != "0")
                    branchId = Int32.Parse(ddlBranch.SelectedValue);

                if (ddlMLA.SelectedValue != "0")
                    paNum = ddlMLA.SelectedValue;

                if (ddlSLA.SelectedValue != "0")
                    saNum = ddlSLA.SelectedValue;

                else
                    saNum = string.Empty;

                DataTable dtTable = new DataTable();
               
                //byte[] bytesLRDetails = objLegalRepossession_Client.FunPubGetMappedPASADetails(intCompanyId, lobId, branchId, paNum, saNum);
                //dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesLRDetails, ObjSerMode, typeof(DataTable));

                int assetCount = objLegalRepossession_Client.FunPubGetMappedAccountAssetDetails(intCompanyId, lobId, branchId, paNum, saNum);

                if (assetCount == 0)
                {
                        ddlAction.SelectedIndex = 1;
                        ddlAction.Enabled = false;
                }
                #region "Commented Code"
                //else if (dtTable.Rows.Count == 1)
                //{
                //    string LRNNo = dtTable.Rows[0]["LRN_NO"].ToString();
                //    int statusCode = Convert.ToInt32(dtTable.Rows[0]["LR_STATUS_CODE"].ToString());
                //    int actionCode = Convert.ToInt32(dtTable.Rows[0]["ACTION_CODE"].ToString());
                //    lnkBtnViewExistingLRN.Text = LRNNo;
                //    lblMappedLRNId.Text = dtTable.Rows[0]["LRN_ID"].ToString();
                //    if (statusCode == 1 || statusCode == 2)
                //    {
                //        Utility.FunShowAlertMsg(this, "Active LRN [" + LRNNo + "] is mapped with this account.You could not create the LRN.");
                //        btnSave.Enabled = false;
                //        //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, , true);
                //    }
                //    else if (statusCode == 3)
                //    {
                //        if (actionCode == 1)
                //            actionCode = 2;
                //        else if (actionCode == 2)
                //            actionCode = 1;

                //        ddlAction.SelectedIndex = ddlAction.Items.IndexOf(ddlAction.Items.FindByValue(actionCode.ToString()));
                //        ddlAction.Enabled = false;
                //    }
                //}
                //else if (dtTable.Rows.Count == 2)
                //{
                //    Utility.FunShowAlertMsg(this, "LRN have been mapped with this account.You could not create the LRN.");
                //    btnSave.Enabled = false;

                //}
            #endregion
                //TODO: Commented by Vijay
                //if (dtTable.Rows.Count == 0)
                //{
                //    ddlAction.SelectedIndex = 1;
                //    ddlAction.Enabled = false;
                //    return;
                //}

                //foreach (DataRow dr in dtTable.Rows)
                //{
                //    string LRNNo = dr["LRN_NO"].ToString();
                //    int statusCode = Convert.ToInt32(dr["LR_STATUS_CODE"].ToString());
                //    int actionCode = Convert.ToInt32(dr["ACTION_CODE"].ToString());
                //    int legalStatusCode = Convert.ToInt32(dr["LEGAL_STATUS_CODE"].ToString());
                //    int repossessionStatusCode = Convert.ToInt32(dr["REPOSSESSION_STATUS_CODE"].ToString());
                //    if (statusCode == 1 || statusCode == 2 || legalStatusCode != 3 || repossessionStatusCode != 3)
                //    {
                //        Utility.FunShowAlertMsg(this, "Active LRN [" + LRNNo + "] is mapped with this account.You could not create the LRN.");
                //        btnSave.Enabled = false;
                //        return;
                //    }
                //    else if (statusCode == 3 && (legalStatusCode != 3 || repossessionStatusCode != 3))
                //    {
                //        Utility.FunShowAlertMsg(this, "Active LRN [" + LRNNo + "] is mapped with this account.You could not create the LRN.");
                //        btnSave.Enabled = false;
                //        return;
                //    }
                //    else if (statusCode == 3 && legalStatusCode == 3 && repossessionStatusCode == 3)
                //    {
                //        //if (actionCode == 1)
                //        //    actionCode = 2;
                //        //else if (actionCode == 2)
                //        //    actionCode = 1;

                //        //ddlAction.SelectedIndex = ddlAction.Items.IndexOf(ddlAction.Items.FindByValue(actionCode.ToString()));
                //        ddlAction.SelectedValue = "1";
                //        ddlAction.Enabled = false;
                //    }
                //}
            }
       
            catch (Exception ex)
            {

            }
            finally
            {
                objLegalRepossession_Client.Close();
            }
        }
    }
    private bool ISProcessMappedLRN(bool showAlert)
    {
        if (strMode != "Q")
        {
            lnkBtnViewExistingLRN.Text = string.Empty;
            lblMappedLRNId.Text = string.Empty;
            btnSave.Enabled = true;
            int lobId = 0;
            int branchId = 0;
            string paNum = string.Empty;
            string saNum = string.Empty;
            ddlAction.Enabled = true;
            LEGALSERVICES.LegalAndRepossessionMgtServicesClient objLR_Client;
            objLR_Client = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

            try
            {

                if (ddlLOB.SelectedIndex != -1)
                    lobId = Int32.Parse(ddlLOB.SelectedValue);

                if (ddlBranch.SelectedValue != "0")
                    branchId = Int32.Parse(ddlBranch.SelectedValue);

                if (ddlMLA.SelectedValue != "0")
                    paNum = ddlMLA.SelectedValue;

                if (ddlSLA.SelectedValue != "0")
                    saNum = ddlSLA.SelectedValue;

                else
                    saNum = string.Empty;

                DataTable dtTable;
                byte[] bytesLRDetails = objLR_Client.FunPubGetMappedLRNDetails(intCompanyId, lobId, branchId, paNum, saNum);
                dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesLRDetails, ObjSerMode, typeof(DataTable));

                if (dtTable.Rows.Count == 2)
                {
                    Utility.FunShowAlertMsg(this, "LRN have been mapped with this account.You could not create the LRN.");
                    btnSave.Enabled = false;
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objLR_Client != null)
                    objLR_Client.Close();
            }
        }
        return false;
    }
    private void CheckAccountHasAsset()
    {
        int lobId = 0;
        int branchId = 0;
        string paNum = string.Empty;
        string saNum = string.Empty;
        ddlAction.Enabled = true;
        LEGALSERVICES.LegalAndRepossessionMgtServicesClient objLR_Client;
        objLR_Client = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {

            if (ddlLOB.SelectedIndex != -1)
            {
                lobId = Int32.Parse(ddlLOB.SelectedValue);
            }

            if (ddlBranch.SelectedValue != "0")
            {
                branchId = Int32.Parse(ddlBranch.SelectedValue);
            }
            if (ddlMLA.SelectedValue != "0")
            {
                paNum = ddlMLA.SelectedValue;
            }
            if (ddlSLA.SelectedValue != "0")
            {
                saNum = ddlSLA.SelectedValue;
            }
            else
            {
                saNum = string.Empty;
            }
            int assetCount = objLegalRepossession_Client.FunPubGetMappedAccountAssetDetails(intCompanyId, lobId, branchId, paNum, saNum);

            if (assetCount == 0)
            {
                ddlAction.Enabled = false;
            }
        }
        catch { }
        finally
        {
            if (objLR_Client != null)
                objLR_Client.Close();
        }
    }

    #endregion

    #region [Button Events]

    protected void btnSave_Click(object sender, EventArgs e)
    {
                S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices =
                    new S3GAdminServicesReference.S3GAdminServicesClient();                
                objLegalRepossession_Client = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();
                try
                {
                    
                    //if (ISProcessMappedLRN(true))
                    //    return;
                    objS3G_LR_RepossessionNoteDataTable = new LEGAL.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteDataTable();
                    objS3G_LR_RepossessionNoteDataRow = objS3G_LR_RepossessionNoteDataTable.NewS3G_LR_RepossessionNoteRow();
                    if (ISValidate() == 0)
                    {
                        objS3G_LR_RepossessionNoteDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                        objS3G_LR_RepossessionNoteDataRow.Company_ID = intCompanyId;
                        objS3G_LR_RepossessionNoteDataRow.LRN_ID = intLRNId;
                        objS3G_LR_RepossessionNoteDataRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
                        objS3G_LR_RepossessionNoteDataRow.PANum = ddlMLA.SelectedText.Trim();
                        objS3G_LR_RepossessionNoteDataRow.Customer_ID = intCustomer;
                        if (ddlSLA.SelectedValue == "0" || ddlSLA.SelectedValue == string.Empty)
                            objS3G_LR_RepossessionNoteDataRow.SANum = ddlMLA.SelectedValue + "DUMMY";
                        else
                            objS3G_LR_RepossessionNoteDataRow.SANum = ddlSLA.SelectedValue;
                        objS3G_LR_RepossessionNoteDataRow.LRN_Date = DateTime.Now;
                        objS3G_LR_RepossessionNoteDataRow.Action_Code = Convert.ToInt32(ddlAction.SelectedValue);
                        objS3G_LR_RepossessionNoteDataRow.Action_Type_Code = 4; //TYPE_CODE 4 FROM LOOKUPMASTER FOR 'Legal or Repossession Action'
                        objS3G_LR_RepossessionNoteDataRow.LRN_No = txtLRNo.Text == string.Empty ? string.Empty : hidLRNNo.Value;
                        objS3G_LR_RepossessionNoteDataRow.Action_Point = txtActionPoints.Text.Trim();
                        objS3G_LR_RepossessionNoteDataRow.Followup_Employee_ID = Convert.ToInt32(ddlFollowUPEMPID.SelectedValue);
                        objS3G_LR_RepossessionNoteDataRow.LR_Status_Type_Code = 1; //TYPE_CODE 1 FROM LOOKUPMASTER FOR 'Legal and Repossession Approvals'
                        objS3G_LR_RepossessionNoteDataRow.LR_Status_Code = Convert.ToInt32(txtLRStatus.Text == "Pending" ? "1" : (txtLRStatus.Text == "Under Process" ? "2" : (txtLRStatus.Text == "Approved" ? "3" : "4")));
                        //objS3G_LR_RepossessionNoteDataRow.Mapped_LRN_No = lnkBtnViewExistingLRN.Text;
                        //if (lblMappedLRNId.Text == string.Empty)
                        //{
                        //    objS3G_LR_RepossessionNoteDataRow.Mapped_LRN_ID = 0;
                        //}
                        //else
                        //{
                        //    objS3G_LR_RepossessionNoteDataRow.Mapped_LRN_ID = Int32.Parse(lblMappedLRNId.Text);
                        //}
                        objS3G_LR_RepossessionNoteDataRow.Legal_Status_Type_Code = 6;
                        objS3G_LR_RepossessionNoteDataRow.Legal_Status_Code = 1;
                        objS3G_LR_RepossessionNoteDataRow.Repossession_Status_Type_Code = 7;
                        objS3G_LR_RepossessionNoteDataRow.Repossession_Status_Code = 1;

                        objS3G_LR_RepossessionNoteDataRow.Created_By = intUserId;
                        objS3G_LR_RepossessionNoteDataRow.Created_On = DateTime.Now;
                        objS3G_LR_RepossessionNoteDataRow.Modified_By = intUserId;
                        objS3G_LR_RepossessionNoteDataRow.Modified_On = DateTime.Now;

                        objS3G_LR_RepossessionNoteDataRow.Remarks = txtRemarks.Text.Trim();
                        objS3G_LR_RepossessionNoteDataRow.Password = txtPassword.Text.Trim();
                        objS3G_LR_RepossessionNoteDataRow.Approval_Status_Code = Convert.ToInt32(ddlStatus.SelectedValue);
                        objS3G_LR_RepossessionNoteDataRow.Approval_Status_Type_Code = 1;
                        if (txtPassword.Text.Trim() != "" && ddlStatus.SelectedValue == "0")
                        {
                            Utility.FunShowAlertMsg(this, "Please select status to Approve or Reject");
                            return;
                        }
                        if (ddlStatus.SelectedValue != "0" && txtPassword.Text.Trim() == "")
                        {
                            Utility.FunShowAlertMsg(this, "Please enter the Password");
                            return;
                        }
                        if ((txtPassword.Text.Trim() != "") && (ddlStatus.SelectedValue == "3"))
                        {
                            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserId, txtPassword.Text.Trim()) > 0)
                            {
                                Utility.FunShowAlertMsg(this, "Invalid Password");
                                return;
                            }
                            updateStatus = "A";
                        }
                        else if ((txtPassword.Text.Trim() != "") && (ddlStatus.SelectedValue == "4"))
                        {
                            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserId, txtPassword.Text.Trim()) > 0)
                            {
                                Utility.FunShowAlertMsg(this, "Invalid Password");
                                return;
                            }
                            updateStatus = "R";
                        }

                        objS3G_LR_RepossessionNoteDataTable.AddS3G_LR_RepossessionNoteRow(objS3G_LR_RepossessionNoteDataRow);
                        string strLRNO = string.Empty;

                        intErrCode = objLegalRepossession_Client.FunPubCreateRepossessionNote(out strLRNO, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LR_RepossessionNoteDataTable, ObjSerMode));
                        switch (intErrCode)
                        {
                            case 0:

                                if (intLRNId > 0)
                                {
                                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here

                                    if (updateStatus == "U")
                                        strAlert = strAlert.Replace("__ALERT__", "Legal Repossession Note Details updated successfully");
                                    else if (updateStatus == "A")
                                        strAlert = strAlert.Replace("__ALERT__", "Legal Repossession Note Approved successfully");
                                    else if (updateStatus == "R")
                                        strAlert = strAlert.Replace("__ALERT__", "Legal Repossession Note Rejected successfully");

                                }
                                else
                                {
                                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here

                                    strAlert = "LRN NO " + strLRNO + " created successfully";
                                    strAlert += @"\n\nWould you like to create one more Legal Repossession Note?";
                                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                                    strRedirectPageView = string.Empty;
                                }
                                break;

                            case 1:
                                strAlert = strAlert.Replace("__ALERT__", "INVALID PASSWORD");
                                strRedirectPageView = string.Empty;
                                break;
                            case -1:
                                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                                strRedirectPageView = string.Empty;
                                break;
                            case -2:
                                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                                strRedirectPageView = string.Empty;
                                break;
                            case 3:
                                strAlert = strAlert.Replace("__ALERT__", "All Approvals are Over");
                                strRedirectPageView = string.Empty;
                                break;
                            case 4:
                                //strAlert = strAlert.Replace("__ALERT__", "Cut off Date is Exceeded - cannot approve");
                                //strRedirectPageView = "";
                                break;
                            case 5:
                                strAlert = strAlert.Replace("__ALERT__", "The Approvals can done only above 3 level users");
                                strRedirectPageView = string.Empty;
                                break;
                            case 10:
                                strAlert = strAlert.Replace("__ALERT__", "The User had done the Approval Already");
                                strRedirectPageView = string.Empty;
                                break;
                            case 11:
                                strAlert = strAlert.Replace("__ALERT__", "The Created User Cannot Approve Or Reject");
                                strRedirectPageView = string.Empty;
                                break;
                            default:
                                if (intLRNId > 0)
                                {
                                    strAlert = strAlert.Replace("__ALERT__", "Error in updating Legal Repossession Note details");
                                }
                                else
                                {
                                    strAlert = strAlert.Replace("__ALERT__", "Error in adding Legal Repossession Note details");
                                }
                                strRedirectPageView = string.Empty;
                                break;
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }
                    else
                    {
                        if (ISValidate() == 1)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Invalid Customer");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                      ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                    
                }
                finally
                {
                    //if (objLegalRepossession_Client != null)
                        objLegalRepossession_Client.Close();
                    //if (ObjS3GAdminServices != null)
                    ObjS3GAdminServices.Close();
                }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = -1;
            ddlBranch.SelectedValue = "0";
            ddlFollowUPEMPID.Clear();
            ddlMLA.Clear();
            ddlSLA.Items.Clear();
            ddlAction.SelectedIndex = -1;
            ddlLOB.Focus();
            txtLRNo.Text = txtActionPoints.Text = txtFlUpEmpName.Text = string.Empty;
            S3GCustomerAddress1.ClearCustomerDetails();
            btnAccount.Visible = false;
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@OPTION", "2");
            Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
            Procparam.Add("@USERID", Convert.ToString(intUserId));
            ddlAction.BindDataTable(SPNames.S3G_LR_LOADLOV, Procparam, new string[] { "LOOKUP_CODE", "LOOKUP_DESCRIPTION" });
        }
        catch (Exception ex)    
        {
            //custRouterLogic.ErrorMessage = ex.Message;
            //custRouterLogic.IsValid = false;
            ////  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            ////cvLRNote.IsValid = false;
            ////cvLRNote.ErrorMessage = "Unable to clear data";
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
           // custRouterLogic.ErrorMessage = ex.Message;
            //custRouterLogic.IsValid = false;
        }

    }
    protected void lnkBtnViewExistingLRN_Click(object sender, EventArgs e)
    {

    //LegalRepossession/S3GLRRepossessionNote_Add.aspx?qsViewId=504B203C3972A8FF2C8418477935FBF83FD0EA050A24F46650BA708FEDFE9B97C8B338186DF39A4A158C6A2B438951150BA7550D4BA998BE7F9AB014807BD8C7&qsMode=M
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblMappedLRNId.Text, false, 0);
        string pageName = "../LegalRepossession/S3GLRRepossessionNote_Add.aspx?" + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q";
        //string pageName = "../LegalRepossession/S3GLRRepossessionNote_Add.aspx?qsViewId=504B203C3972A8FF2C8418477935FBF83FD0EA050A24F46650BA708FEDFE9B97C8B338186DF39A4A158C6A2B438951150BA7550D4BA998BE7F9AB014807BD8C7&qsMode=M";

        string strScipt = "window.open('" + pageName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "newwindow", strScipt, true);
    }
    protected void btnAccount_Click(object sender, EventArgs e)
    {
        string s = "";
        if (ddlSLA.SelectedValue == "0")
            s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=" + ddlMLA.SelectedText.Trim() + "&qsSANum=" + ddlMLA.SelectedText.Trim() + "DUMMY";

        else
            s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=" + ddlMLA.SelectedText.Trim() + "&qsSANum=" + ddlSLA.SelectedItem.Text.Trim();

        // s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=2011-2012/PRIME/10001&qsSANum=2011-2012/WCSAN/100";

        string strScipt = "window.open('" + s + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "SOA", strScipt, true);

    }

    #endregion

}
