#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Origination
/// Screen Name         :   S3GORGPaymentRuleCard_Add
/// Created By          :   Suresh P
/// Created Date        :   22-Jun-2010
/// Purpose             :   To Add PaymentRuleCard details
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// 
/// Last Updated By	    : Thangam M, Manikandan R, Saran M
/// Last Updated Date   : 10-09-2010
/// Reason              : Bug fixing for the follwing Test cases: 
///                        TC_025

/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using RuleCardMgtServicesReference;
using S3GBusEntity;
using S3GBusEntity.Origination;
#endregion

public partial class Origination_S3GORGPaymentRuleCard : ApplyThemeForProject
{
    #region Intialization


    RuleCardMgtServicesClient ObjPaymentRuleCardMasterClient = null;
    RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable ObjS3G_PaymentRuleCardDataTable = null;
    RuleCardMgtServices.S3G_ORG_PaymentRuleCardRow ObjPaymentRuleCardMasterRow = null;

    SerializationMode ObjSerMode = SerializationMode.Binary;
    UserInfo ObjUserInfo = new UserInfo();
    Dictionary<string, string> ObjDict = null;

    int intPaymentRuleCardID = 0;
    int intErrCode = 0;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    string strRedirectPage = "~/Origination/S3GORGPaymentRuleCard_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGPaymentRuleCard_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGPaymentRuleCard_Add.aspx';";
    #endregion

    #region Page Events

    private void FunPriValidationDefault(bool blnFlag, int intModeID)
    {
        try
        {
            rfvLineOfBusiness.Enabled = blnFlag;
            rfvAccountType.Enabled = blnFlag;
            rfvEntityType.Enabled = blnFlag;
            rfvOnTapRefinance.Enabled = blnFlag;

            lblLineOfBusiness.Attributes.Add("class", "");
            lblAccountType.Attributes.Add("class", "");
            lblEntityType.Attributes.Add("class", "");
            lblOnTapRefinance.Attributes.Add("class", "");
            if (blnFlag)
            {
                lblLineOfBusiness.Attributes.Add("class", "styleReqFieldLabel");
                lblAccountType.Attributes.Add("class", "styleReqFieldLabel");
                lblEntityType.Attributes.Add("class", "styleReqFieldLabel");
                lblOnTapRefinance.Attributes.Add("class", "styleReqFieldLabel");
            }
            if ((intModeID == 1) || (intModeID == -1))
            {
                /// Get Payment Rule Card Details
                FunPriGetPaymentRuleCardDetails();
            }
            bool blnIsReadOnly = (intModeID == -1) ? true : false;
            FunPriControlEnable(intModeID, blnIsReadOnly);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriValidationCompensation(bool blnFlag)
    {
        try
        {
            rfvFrequency.Enabled = blnFlag;
            rfvCompensationLevyPattern.Enabled = blnFlag;
            lblCompensationLevyPattern.Attributes.Add("class", "");
            lblFrequency.Attributes.Add("class", "");
            if (blnFlag)
            {
                lblCompensationLevyPattern.Attributes.Add("class", "styleReqFieldLabel");
                lblFrequency.Attributes.Add("class", "styleReqFieldLabel");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriControlEnable(int intModeID, bool blnIsReadOnly)
    {
        try
        {
            string strProperty = "readonly";
            ///Set readonly property to the controls

            if (blnIsReadOnly)
            {
                txtPaymentRuleNumber.Attributes.Add(strProperty, strProperty);
                ddlLOB1.ClearDropDownList();
                ddlProductName.ClearDropDownList();
                ddlAccountType.ClearDropDownList();
                ddlEntityType.ClearDropDownList();

                txtCompensationPercentage.Attributes.Add(strProperty, strProperty);
                ddlCompensationLevyPattern.ClearDropDownList();
                ddlFrequency.ClearDropDownList();
                ddlOnTapRefinance.ClearDropDownList();
            }
            else
            {
                txtPaymentRuleNumber.Attributes.Remove(strProperty);
                txtCompensationPercentage.Attributes.Remove(strProperty);
            }

            bool blnFlag = (intModeID > 0) ? false : true;
            txtPaymentRuleNumber.Enabled = false;
            ddlLOB1.Enabled = blnFlag;
            ddlProductName.Enabled = blnFlag;
            ddlAccountType.Enabled = blnFlag;
            ddlEntityType.Enabled = blnFlag;
            txtCompensationPercentage.Enabled = blnFlag;
            ddlCompensationLevyPattern.Enabled = blnFlag;
            ddlFrequency.Enabled = blnFlag;
            ddlOnTapRefinance.Enabled = blnFlag;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intPaymentRuleCardID = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }
            txtCompensationPercentage.SetPercentagePrefixSuffix(2, 2, true, false, "Compensation %");
            if (!IsPostBack)
            {
                if (PageMode == PageModes.Create)
                {
                    FunPriLoadLOB_LIST();
                    FunPriLoadProduct_LIST();
                }
                chkActive.Checked = false;
                //User Authorization Code starts
                if ((intPaymentRuleCardID > 0) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if ((intPaymentRuleCardID > 0) && (strMode == "Q")) // Query // Modify
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
                //User Authorization Code end
                //  txtCompensationPercentage.SetDecimalPrefixSuffix(2, 4, true,"Compensation Percentage %");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    //This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode
                    {
                        FunPriValidationDefault(true, intModeID);

                        EntityTypeDropdown(false, false, false);
                        FunPriCompensationPercentage(false);
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                        chkActive.Checked = true;
                        chkActive.Enabled = false;
                        btnClear.Enabled = true;
                        ddlLOB1.Focus();
                        if (!bCreate)
                        {
                            btnSave.Enabled = false;
                        }
                        break;
                    }
                case 1: // Modify Mode
                    {

                        FunPriValidationDefault(false, intModeID);
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                        chkActive.Enabled = true;
                        btnClear.Enabled = false;
                        if (!bModify)
                        {
                            btnSave.Enabled = false;
                        }
                        break;
                    }
                case -1:// Query Mode
                    {
                        if (!bQuery)
                        {
                            Response.Redirect(strRedirectPage,false);
                        }
                        FunPriValidationDefault(false, intModeID);
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                        chkActive.Enabled = false;
                        txtCompensationPercentage.Enabled = false;
                        btnClear.Enabled = false;
                        btnSave.Enabled = false;
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region Button (Save / Clear / Cancel)
    /// <summary>
    /// Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
        ObjPaymentRuleCardMasterClient = new RuleCardMgtServicesClient();

        try
        {
            ObjS3G_PaymentRuleCardDataTable = new RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable();
            ObjPaymentRuleCardMasterRow = ObjS3G_PaymentRuleCardDataTable.NewS3G_ORG_PaymentRuleCardRow();
            ObjPaymentRuleCardMasterRow.Payment_RuleCard_ID = intPaymentRuleCardID;
            ObjPaymentRuleCardMasterRow.Payment_Rule_Number = txtPaymentRuleNumber.Text;
            ObjPaymentRuleCardMasterRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
            ObjPaymentRuleCardMasterRow.LOB_ID = Convert.ToInt32(ddlLOB1.SelectedValue);
            ObjPaymentRuleCardMasterRow.Product_ID = Convert.ToInt32(ddlProductName.SelectedValue);
            ObjPaymentRuleCardMasterRow.AccountType_ID = Convert.ToInt32(ddlAccountType.SelectedValue);
            ObjPaymentRuleCardMasterRow.Entity_ID = Convert.ToInt32(ddlEntityType.SelectedValue);
            if (txtCompensationPercentage.Text != "")
                ObjPaymentRuleCardMasterRow.Compensation_Percentage = Convert.ToDecimal(txtCompensationPercentage.Text);
            ObjPaymentRuleCardMasterRow.Compensation_Levy_Pattern = ddlCompensationLevyPattern.Text;
            ObjPaymentRuleCardMasterRow.Levy_Frequency = Convert.ToDecimal(ddlFrequency.Text);
            ObjPaymentRuleCardMasterRow.Is_OnTap_Refinance = Convert.ToBoolean(Convert.ToInt32(ddlOnTapRefinance.Text));
            ObjPaymentRuleCardMasterRow.Is_Active = chkActive.Checked;
            ObjPaymentRuleCardMasterRow.Created_By = ObjUserInfo.ProUserIdRW;
            ObjPaymentRuleCardMasterRow.Modified_By = ObjUserInfo.ProUserIdRW;
            ObjS3G_PaymentRuleCardDataTable.AddS3G_ORG_PaymentRuleCardRow(ObjPaymentRuleCardMasterRow);

            //ObjPaymentRuleCardMasterClient = new RuleCardMgtServicesClient();

            if (intPaymentRuleCardID > 0)
            {
                intErrCode = ObjPaymentRuleCardMasterClient.FunPubModifyPaymentRuleCardInt(intPaymentRuleCardID, chkActive.Checked, ObjUserInfo.ProUserIdRW);
            }
            else if (intPaymentRuleCardID == 0)
            {
                //if (txtCompensationPercentage.Enabled == true)
                //{
                //    if (txtCompensationPercentage.Text.Trim() == "0" || string.IsNullOrEmpty(txtCompensationPercentage.Text.Trim()))
                //    {
                //        Utility.FunShowAlertMsg(this, "Compensation% cannot be zero or empty");
                //        return;
                //    }
                //}
                intErrCode = ObjPaymentRuleCardMasterClient.FunPubCreatePaymentRuleCardInt(ObjSerMode, ClsPubSerialize.Serialize(ObjS3G_PaymentRuleCardDataTable, ObjSerMode));
                //intErrCode = 0;
            }
            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intPaymentRuleCardID > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Payment Rule Card details updated successfully");
                }
                else
                {
                    strAlert = "Payment Rule Card details added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
            }
            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Payment Rule Card Number already exists. Enter a new Payment Rule Card Number");
                strRedirectPageView = "";
            }
            else if (intErrCode == 3)
            {
                strAlert = strAlert.Replace("__ALERT__", "Payment Rule Card already exists for the selected combination. ");
                strRedirectPageView = "";
            }
            else if (intErrCode == 4)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number Control not defined");
                strRedirectPageView = "";
            }
            else if (intErrCode == 50)
            {
                strAlert = strAlert.Replace("__ALERT__", "Error in inserting Payment Rule Card");
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
        }
        catch (FaultException<RuleCardMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjPaymentRuleCardMasterClient.Close();
        }


    }
    /// <summary>
    /// Clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtPaymentRuleNumber.Text = "";
            ddlLOB1.SelectedValue = "0";
            ddlProductName.SelectedIndex = 0;
            ddlProductName.ClearDropDownList();
            ddlAccountType.SelectedValue = "0";
            EntityTypeDropdown(false, false, false);
            FunPriCompensationPercentage(false);
            //txtCompensationPercentage.Text = "";
            //ddlCompensationLevyPattern.SelectedValue = "0";
            //ddlFrequency.SelectedValue = "0";
            FunPriCompensationPercentage(false);
            ddlOnTapRefinance.SelectedIndex = 0;
            ddlLOB1.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    /// <summary>
    /// Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    #region User Defined Methods

    /// <summary>
    /// Load LOB
    /// </summary>
    private void FunPriLoadLOB_LIST()
    {
        try
        {
            ObjDict = new Dictionary<string, string>();
            ObjDict.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            ObjDict.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            ObjDict.Add("@Program_ID", "28");

            if (intPaymentRuleCardID == 0)
            {
                ObjDict.Add("@Is_Active", "1");
                ObjDict.Add("@Is_UserLobActive", "1");
            }
            ddlLOB1.BindDataTable(SPNames.LOBMaster, ObjDict, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });

            ObjDict = new Dictionary<string, string>();
            DataTable dtDefault = Utility.GetDefaultData("S3G_ORG_GetPaymentRuleCardLookUp", ObjDict);
            DataTable dtDefaultNew;
            DataView dvSearchView;
            if (dtDefault != null)
            {
                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_PAYMENT_RULECARD_AccountType'";
                dvSearchView.Sort = "Name Asc";
                dtDefaultNew = dvSearchView.ToTable();

                ddlAccountType.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_PAYMENT_RULECARD_EntityType'";
                dtDefaultNew = dvSearchView.ToTable();
                ddlEntityType.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_PAYMENT_RULECARD_CompensationLevyPattern'";
                dtDefaultNew = dvSearchView.ToTable();
                ddlCompensationLevyPattern.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_PAYMENT_RULECARD_LevyFrequency'";
                dtDefaultNew = dvSearchView.ToTable();
                ddlFrequency.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_PAYMENT_RULECARD_OnTapRefinance'";
                dtDefaultNew = dvSearchView.ToTable();
                ddlOnTapRefinance.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();
                ddlOnTapRefinance.Items.RemoveAt(0);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    /// <summary>
    /// Load RoleCode
    /// </summary>
    private void FunPriLoadProduct_LIST()
    {
        try
        {
            ObjDict = new Dictionary<string, string>();
            //ObjDict.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());

            //if (ddlLOB1.SelectedValue != 0)
            ObjDict.Add("@LOB_ID", ddlLOB1.SelectedValue.ToString());
            //else
            //ObjDict.Add("@LOB_ID", ddlLOB1.SelectedValue.ToString());
            //else  
            if (intPaymentRuleCardID == 0)
            {
                ObjDict.Add("@Is_Active", "1");
            }
            //Commented by Saranya on 16-Feb-2012 to remove the Product Code in LOV
            //ddlProductName.BindDataTable("S3G_ORG_GetLOBProductList", ObjDict, new string[] { "Product_ID", "Product_Code" });
            ddlProductName.BindDataTable("S3G_ORG_GetLOBProductList", ObjDict, new string[] { "Product_ID", "Product_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void EntityTypeDropdown(bool blnVendor, bool blnCustomer, bool blnVendorOthers)
    {
        try
        {
            ddlEntityType.Items.FindByValue("1").Enabled = blnVendor;
            ddlEntityType.Items.FindByValue("2").Enabled = blnCustomer;
            ddlEntityType.Items.FindByValue("3").Enabled = blnVendorOthers;
            ddlEntityType.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriCompensationPercentage(bool blnFlag)
    {
        try
        {
            txtCompensationPercentage.Enabled = blnFlag;
            txtCompensationPercentage.Text = "";
            if (blnFlag == true)
            {
                lblCompensationPercentage.Attributes.Add("class", "styleReqFieldLabel");
            }
            else
            {
                lblCompensationPercentage.Attributes.Add("class", "styleMandatoryLabel");
            }
            rfvCompensationPercentage.Enabled = blnFlag;
            ddlCompensationLevyPattern.Items.FindByValue("1").Enabled = blnFlag;
            ddlCompensationLevyPattern.Items.FindByValue("2").Enabled = blnFlag;

            ddlCompensationLevyPattern.SelectedValue = "0";
            ddlFrequency.SelectedValue = "0";
            //ddlCompensationLevyPattern.Enabled = blnFlag;
            //ddlFrequency.Enabled = blnFlag;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #region Load PaymentRuleCard
    /// <summary>
    /// Get PaymentRuleCard Details 
    /// </summary>
    private void FunPriGetPaymentRuleCardDetails()
    {
        //ObjPaymentRuleCardMasterClient = new RuleCardMgtServicesClient();
        try
        {
            ObjDict = new Dictionary<string, string>();
            ObjDict.Add("@Payment_RuleCard_ID", intPaymentRuleCardID.ToString());
            DataTable dtModify = Utility.GetDefaultData("S3G_ORG_GetPaymentRuleCard", ObjDict);

            //byte[] byteLOBDetails = ObjPaymentRuleCardMasterClient.FunPubQueryPaymentRuleCard(intPaymentRuleCardID);
            //ObjS3G_PaymentRuleCardDataTable = (RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, ObjSerMode, typeof(RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable));
            //ObjPaymentRuleCardMasterRow = ObjS3G_PaymentRuleCardDataTable.Rows[0] as RuleCardMgtServices.S3G_ORG_PaymentRuleCardRow;
            txtPaymentRuleNumber.Text = dtModify.Rows[0]["Payment_Rule_Number"].ToString(); //ObjPaymentRuleCardMasterRow.Payment_Rule_Number.ToString();
            //FunPriLoadLOB_LIST();

            System.Web.UI.WebControls.ListItem lst;

            lst = new System.Web.UI.WebControls.ListItem(dtModify.Rows[0]["LOB"].ToString(), dtModify.Rows[0]["LOB_ID"].ToString());
            ddlLOB1.Items.Add(lst);
            ddlLOB1.SelectedValue = dtModify.Rows[0]["LOB_ID"].ToString(); // ObjPaymentRuleCardMasterRow.LOB_ID.ToString();
            //ddlLOB1.Attributes.Add("LOB_ID", ddlLOB1.SelectedValue);
            //FunPriLoadProduct_LIST();

            lst = new System.Web.UI.WebControls.ListItem(dtModify.Rows[0]["Product_Name"].ToString(), dtModify.Rows[0]["Product_ID"].ToString());
            ddlProductName.Items.Add(lst);

            ddlProductName.SelectedValue = dtModify.Rows[0]["Product_ID"].ToString();//ObjPaymentRuleCardMasterRow.Product_ID.ToString();

            lst = new System.Web.UI.WebControls.ListItem(dtModify.Rows[0]["AccountType"].ToString(), dtModify.Rows[0]["AccountType_ID"].ToString());
            ddlAccountType.Items.Add(lst);

            ddlAccountType.SelectedValue = dtModify.Rows[0]["AccountType_ID"].ToString(); // ObjPaymentRuleCardMasterRow.AccountType_ID.ToString();

            lst = new System.Web.UI.WebControls.ListItem(dtModify.Rows[0]["Entity_Type"].ToString(), dtModify.Rows[0]["Entity_ID"].ToString());
            ddlEntityType.Items.Add(lst);

            ddlEntityType.SelectedValue = dtModify.Rows[0]["Entity_ID"].ToString(); // ObjPaymentRuleCardMasterRow.Entity_ID.ToString();
            txtCompensationPercentage.Text = dtModify.Rows[0]["Compensation_Percentage"].ToString();//ObjPaymentRuleCardMasterRow.IsCompensation_PercentageNull() ? "" : ObjPaymentRuleCardMasterRow.Compensation_Percentage.ToString();
            if (string.IsNullOrEmpty(txtCompensationPercentage.Text) == false)
            {
                txtCompensationPercentage.Text = (txtCompensationPercentage.Text.Split('.').Length == 2) ? txtCompensationPercentage.Text : txtCompensationPercentage.Text + "." + Math.Pow(10, 4).ToString().Trim('1');
            }

            lst = new System.Web.UI.WebControls.ListItem(dtModify.Rows[0]["Compensation_Levy"].ToString(), dtModify.Rows[0]["Compensation_Levy_Pattern"].ToString());
            ddlCompensationLevyPattern.Items.Add(lst);

            ddlCompensationLevyPattern.SelectedValue = dtModify.Rows[0]["Compensation_Levy_Pattern"].ToString();//ObjPaymentRuleCardMasterRow.Compensation_Levy_Pattern.ToString();

            lst = new System.Web.UI.WebControls.ListItem(dtModify.Rows[0]["Levy_Freq_Text"].ToString(), dtModify.Rows[0]["Levy_Frequency"].ToString());
            ddlFrequency.Items.Add(lst);

            ddlFrequency.SelectedValue = dtModify.Rows[0]["Levy_Frequency"].ToString(); // ObjPaymentRuleCardMasterRow.Levy_Frequency.ToString();

            lst = new System.Web.UI.WebControls.ListItem(dtModify.Rows[0]["OnTap_Ref_Text"].ToString(), Convert.ToInt32(Convert.ToBoolean(dtModify.Rows[0]["Is_OnTap_Refinance"].ToString())).ToString());
            ddlOnTapRefinance.Items.Add(lst);

            ddlOnTapRefinance.SelectedValue = Convert.ToInt32(Convert.ToBoolean(dtModify.Rows[0]["Is_OnTap_Refinance"].ToString())).ToString(); // Convert.ToInt32(ObjPaymentRuleCardMasterRow.Is_OnTap_Refinance).ToString();
            if (dtModify.Rows[0]["Is_Active"].ToString().ToUpper() == "TRUE")
                chkActive.Checked = true;
            else
                chkActive.Checked = false;
        }
        catch (FaultException<RuleCardMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            // ObjPaymentRuleCardMasterClient.Close();
        }
    }

    #endregion

    #endregion

    #region Page Events

    private void FrequencyDropdown(bool blnWeekly, bool blnFortNightly, bool blnMonthly, bool blnQuarterly, bool blnHalfYearly, bool blnAnnual)
    {
        try
        {
            ddlFrequency.Items.FindByValue("1").Enabled = blnWeekly;
            ddlFrequency.Items.FindByValue("2").Enabled = blnFortNightly;
            ddlFrequency.Items.FindByValue("3").Enabled = blnMonthly;
            ddlFrequency.Items.FindByValue("4").Enabled = blnQuarterly;
            ddlFrequency.Items.FindByValue("5").Enabled = blnHalfYearly;
            ddlFrequency.Items.FindByValue("6").Enabled = blnAnnual;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void Compensation_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            int intPrefix = 2;
            int intSuffix = 4;
            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            S3GSession S3GSession = new S3GSession();
            intGPSPrefix = S3GSession.ProGpsPrefixRW;
            intGPSSuffix = S3GSession.ProGpsSuffixRW;

            if (intPrefix > intGPSPrefix)
            {
                intPrefix = intGPSPrefix;
            }
            if (intSuffix > intGPSSuffix)
            {
                intSuffix = intGPSSuffix;
            }

            //if ((txtCompensationPercentage.Text).Contains("."))
            //{
            //    string[] str = (txtCompensationPercentage.Text).Split('.');
            //    if (str[1] == null || str[1].Length == 0)
            //    {
            //        Utility.FunShowAlertMsg(this, "Enter valid decimal");
            //        txtCompensationPercentage.Text = "";
            //        txtCompensationPercentage.Focus();
            //        return;
            //    }

            //}
            ////if (txtCompensationPercentage.Text == "" || Convert.ToDecimal(txtCompensationPercentage.Text) == 0)
            ////{
            ////    Utility.FunShowAlertMsg(this, "Compensation% cannot be zero or empty");
            ////    txtCompensationPercentage.Text = "";
            ////    txtCompensationPercentage.Focus();
            ////    return;
            ////}
            //else
            //{
            //    string[] str = (txtCompensationPercentage.Text).Split('.');
            //    if (str[0].Length > intPrefix)
            //    {
            //        Utility.FunShowAlertMsg(this, "Compensation % precision should not exceed " + intPrefix.ToString() + " digits");
            //        txtCompensationPercentage.Text = "";
            //        //txtCompensationPercentage.Focus();
            //        return;
            //    }
            //}

            if (txtCompensationPercentage.Text == "")
                FunPriValidationCompensation(false);
            else
                FunPriValidationCompensation(true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlLOB1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (txtCompensationPercentage.Text == "")
                FunPriValidationCompensation(false);
            else
                FunPriValidationCompensation(true);
            FunPriLoadProduct_LIST();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void AccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            EntityTypeDropdown(false, false, false);
            FunPriCompensationPercentage(false);
            if (txtCompensationPercentage.Text == "")
                FunPriValidationCompensation(false);
            else
                FunPriValidationCompensation(true);
            // FunPriValidationCompensation(false);
            string strType = ddlAccountType.SelectedItem.Value;

            switch (strType.ToLower())
            {
                case "4":
                    {
                        EntityTypeDropdown(true, false, true);
                        FunPriCompensationPercentage(true);
                        break;
                    }
                case "6":
                    {
                        EntityTypeDropdown(false, true, false);
                        break;
                    }

                default:
                    {
                        EntityTypeDropdown(true, false, true);
                        FunPriCompensationPercentage(false);
                        break;
                    }
            }

            //changed on 9-11-2011 for observation raised by malolan with refereence to the mail dated on 9-11-2011. modified by saran
            //Payment Rule card if the LOB is Loan(Asset Optional)TL,TLE and WC (where is no asset)in the entity type the customer should flow from back end – You can fix it for TL, TLE and WC. But for Loan, there can be asset based finance also. In such scenarios, payment will be either to a customer or to an entity.

            //Commanded by Thangam M based on 08/Nov/2012 mail , this should happen in application and no here
            //if (ddlLOB1.SelectedIndex > 0)
            //{
            //    if (ddlLOB1.SelectedItem.Text.Split('-')[0].Trim().ToUpper().ToString() == "LN")
            //    {
            //        EntityTypeDropdown(false, true, false);
            //    }
            //}
            //Thanagm M end here





        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion
}
