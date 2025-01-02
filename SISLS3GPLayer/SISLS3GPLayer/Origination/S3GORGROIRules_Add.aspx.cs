#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Origination
/// Screen Name         :   S3GORGROIRules_Add
/// Created By          :   Suresh P
/// Created Date        :   22-Jun-2010
/// Purpose             :   To Add ROI Rules details
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
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

public partial class Origination_S3GORGROIRules : ApplyThemeForProject
{

    #region Intialization

    RuleCardMgtServicesClient ObjROIRuleMasterClient = null;
    RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable ObjROIRuleMasterDataTable = new RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable();
    RuleCardMgtServices.S3G_ORG_ROI_RulesRow ObjROIRuleMasterRow = null;

    SerializationMode SerMode = SerializationMode.Binary;
    UserInfo ObjUserInfo = new UserInfo();
    Dictionary<string, string> ObjDictParams;

    int intROIRuleMasterID = 0;
    int intErrCode = 0;
    static string strPageName = "ROI Rules";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    string strRedirectPage = "~/Origination/S3GORGROIRules_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGROIRules_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGROIRules_Add.aspx';";

    #endregion

    #region Page Methods
    /// <summary>
    /// Enable or disable the validation of the controls
    /// </summary>
    /// <param name="blnFlag"></param>
    /// <param name="intModeID"></param>
    private void FunPriValidationDefault(bool blnFlag, int intModeID)
    {
        try
        {
            rfvLineOfBusiness.Enabled = blnFlag;
            rfvProduct.Enabled = blnFlag;
            rfvRateType.Enabled = blnFlag;
            rfvROIRuleNumber.Enabled = blnFlag;
            regROIRuleNumber.Enabled = blnFlag;
            rfvRatePattern.Enabled = blnFlag;
            rfvTime.Enabled = blnFlag;
            rfvFrequency.Enabled = blnFlag;
            rfvRepaymentMode.Enabled = blnFlag;
            rfvRate.Enabled = blnFlag;
            rfvIRRRest.Enabled = blnFlag;
            rfvIntrestCalculation.Enabled = blnFlag;
            rfvIntrestLevy.Enabled = blnFlag;
            //rfvRecoveryPatternYear1.Enabled = blnFlag;
            //rfvRecoveryPatternYear2.Enabled = blnFlag;
            //rfvRecoveryPatternYear3.Enabled = blnFlag;
            //rfvRecoveryPatternYearRest.Enabled = blnFlag;
            rfvInsurance.Enabled = blnFlag;
            rfvResidualValue.Enabled = blnFlag;
            rfvMargin.Enabled = blnFlag;
            rfvMarginPercentage.Enabled = blnFlag;


            lblLineOfBusiness.Attributes.Add("class", "");
            lblModelDescription.Attributes.Add("class", "");
            lblRateType.Attributes.Add("class", "");
            lblROIRuleNumber.Attributes.Add("class", "");
            lblRatePattern.Attributes.Add("class", "");
            lblTime.Attributes.Add("class", "");
            lblFrequency.Attributes.Add("class", "");
            lblRepaymentMode.Attributes.Add("class", "");
            lblRate.Attributes.Add("class", "");
            lblIRRRest.Attributes.Add("class", "");
            lblIntrestCalculation.Attributes.Add("class", "");
            lblIntrestLevy.Attributes.Add("class", "");
            lblRecoveryPatternYear1.Attributes.Add("class", "");
            lblInsurance.Attributes.Add("class", "");
            lblResidualValue.Attributes.Add("class", "");
            lblMargin.Attributes.Add("class", "");
            lblMarginPercentage.Attributes.Add("class", "");
            if (blnFlag)
            {
                lblLineOfBusiness.Attributes.Add("class", "styleReqFieldLabel");
                lblModelDescription.Attributes.Add("class", "styleReqFieldLabel");
                lblRateType.Attributes.Add("class", "styleReqFieldLabel");
                lblROIRuleNumber.Attributes.Add("class", "styleReqFieldLabel");
                lblRatePattern.Attributes.Add("class", "styleReqFieldLabel");
                lblTime.Attributes.Add("class", "styleReqFieldLabel");
                lblFrequency.Attributes.Add("class", "styleReqFieldLabel");
                lblRepaymentMode.Attributes.Add("class", "styleReqFieldLabel");
                lblRate.Attributes.Add("class", "styleReqFieldLabel");
                lblIRRRest.Attributes.Add("class", "styleReqFieldLabel");
                lblIntrestCalculation.Attributes.Add("class", "styleReqFieldLabel");
                lblIntrestLevy.Attributes.Add("class", "styleReqFieldLabel");
                lblRecoveryPatternYear1.Attributes.Add("class", "styleReqFieldLabel");
                lblInsurance.Attributes.Add("class", "styleReqFieldLabel");
                lblResidualValue.Attributes.Add("class", "styleReqFieldLabel");
                lblMargin.Attributes.Add("class", "styleReqFieldLabel");
                lblMarginPercentage.Attributes.Add("class", "styleReqFieldLabel");
            }
            if ((intModeID == 1) || (intModeID == -1))
            {
                /// Get Roi Rule Card Details
                FunPriGetROIRuleMasterDetails();
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
    /// <summary>
    /// Enable or disable the Controls
    /// </summary>
    /// <param name="intModeID"></param>
    /// <param name="blnIsReadOnly"></param>
    private void FunPriControlEnable(int intModeID, bool blnIsReadOnly)
    {
        try
        {
            string strProperty = "readonly";
            ///Set readonly property to the controls
            if (blnIsReadOnly)
            {
                ddlLineofBusiness.ClearDropDownList();
                txtModelDescription.Attributes.Add(strProperty, strProperty);
                ddlRateType.ClearDropDownList();
                txtROIRuleNumber.Attributes.Add(strProperty, strProperty);
                ddlRatePattern.ClearDropDownList();
                ddlTime.ClearDropDownList();
                ddlFrequency.ClearDropDownList();
                ddlRepaymentMode.ClearDropDownList();
                txtRate.Attributes.Add(strProperty, strProperty);
                ddlIRRRest.ClearDropDownList();
                ddlIntrestCalculation.ClearDropDownList();
                ddlIntrestLevy.ClearDropDownList();
                txtRecoveryPatternYear1.Attributes.Add(strProperty, strProperty);
                txtRecoveryPatternYear2.Attributes.Add(strProperty, strProperty);
                txtRecoveryPatternYear3.Attributes.Add(strProperty, strProperty);
                txtRecoveryPatternYearRest.Attributes.Add(strProperty, strProperty);
                //added by saranya
                ddlInsurance.Enabled = true;
                //end
                ddlInsurance.ClearDropDownList();
                ddlResidualValue.ClearDropDownList();
                ddlMargin.ClearDropDownList();
                txtMarginPercentage.Attributes.Add(strProperty, strProperty);
            }
            else
            {
                txtModelDescription.Attributes.Remove(strProperty);
                txtROIRuleNumber.Attributes.Remove(strProperty);
                txtRate.Attributes.Remove(strProperty);
                txtRecoveryPatternYear1.Attributes.Remove(strProperty);
                txtRecoveryPatternYear2.Attributes.Remove(strProperty);
                txtRecoveryPatternYear3.Attributes.Remove(strProperty);
                txtRecoveryPatternYearRest.Attributes.Remove(strProperty);
                txtMarginPercentage.Attributes.Remove(strProperty);
            }


            bool blnFlag = (intModeID > 0) ? false : true;
            ///Set Enable or disable to the controls
            ddlLineofBusiness.Enabled = blnFlag;
            txtModelDescription.Enabled = blnFlag;
            ddlRateType.Enabled = blnFlag;
            txtROIRuleNumber.Enabled = blnFlag;
            ddlRatePattern.Enabled = blnFlag;
            ddlTime.Enabled = blnFlag;
            ddlFrequency.Enabled = blnFlag;
            ddlRepaymentMode.Enabled = blnFlag;
            txtRate.Enabled = blnFlag;
            ddlIRRRest.Enabled = blnFlag;
            ddlIntrestCalculation.Enabled = blnFlag;
            ddlIntrestLevy.Enabled = blnFlag;
            txtRecoveryPatternYear1.Enabled = blnFlag;
            txtRecoveryPatternYear2.Enabled = blnFlag;
            txtRecoveryPatternYear3.Enabled = blnFlag;
            txtRecoveryPatternYearRest.Enabled = blnFlag;
            ddlInsurance.Enabled = blnFlag;
            ddlResidualValue.Enabled = blnFlag;
            ddlMargin.Enabled = blnFlag;
            txtMarginPercentage.Enabled = blnFlag;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;

        }
    }
    /// <summary>
    /// Page Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FunPriSetMaxLength()
    {
        try
        {
            S3GSession obSession = new S3GSession();
            txtRecoveryPatternYear1.SetPercentagePrefixSuffix(3, 2, true, false, "Recovery Pattern Year1");
            txtRecoveryPatternYear2.SetPercentagePrefixSuffix(3, 2, true, false, "Recovery Pattern Year2");
            txtRecoveryPatternYear3.SetPercentagePrefixSuffix(3, 2, true, false, "Recovery Pattern Year3");
            txtRecoveryPatternYearRest.SetPercentagePrefixSuffix(3, 2, true, false, "Recovery Pattern Rest Year");
            txtMarginPercentage.SetPercentagePrefixSuffix(2, 2, true, false, "Margin %");
            if (ddlLineofBusiness.SelectedIndex > 0)
            {
                string[] lob = (ddlLineofBusiness.SelectedItem.Text.ToLower()).Split('-');
                if ((lob[0].ToString()).Trim() == "ol" || (lob[0].ToString()).Trim() == "fl")
                {
                    if (ddlRatePattern.SelectedValue == "3" || ddlRatePattern.SelectedValue == "4" || ddlRatePattern.SelectedValue == "5")
                    {
                        txtRate.SetPercentagePrefixSuffix(5, 4, false, false, "Rate");
                        //txtRate.SetPercentagePrefixSuffix(obSession.ProGpsPrefixRW, obSession.ProGpsSuffixRW, false, false, "Rate");
                        txtRecoveryPatternYear1.SetPercentagePrefixSuffix(5, 4, true, false, "Recovery Pattern Year1");
                        txtRecoveryPatternYear2.SetPercentagePrefixSuffix(5, 4, true, false, "Recovery Pattern Year2");
                        txtRecoveryPatternYear3.SetPercentagePrefixSuffix(5, 4, true, false, "Recovery Pattern Year3");
                        txtRecoveryPatternYearRest.SetPercentagePrefixSuffix(5, 4, true, false, "Recovery Pattern Rest Year");
                    }
                    else
                    {
                        txtRate.SetPercentagePrefixSuffix(2, 2, false, false, "Rate");
                    }
                }
                else
                {
                    txtRate.SetPercentagePrefixSuffix(2, 2, false, false, "Rate");
                }
            }
            else
            {
                txtRate.SetPercentagePrefixSuffix(2, 2, false, false, "Rate");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    /// <summary>
    /// This is used to implement User Authorization
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode
                    {
                        FunPriValidationDefault(true, intModeID);

                        txtRecoveryPatternYear1.Text =
                        txtRecoveryPatternYear2.Text =
                        txtRecoveryPatternYear3.Text =
                        txtRecoveryPatternYearRest.Text = "";//cannot be 0

                        txtRecoveryPatternYear1.Enabled =
                        txtRecoveryPatternYear2.Enabled =
                        txtRecoveryPatternYear3.Enabled =
                        txtRecoveryPatternYearRest.Enabled = false;

                        //txtRate.Text = "0.0000";
                        FunPriSetControlAttributes();
                        FunPriRateTypeDropdown(false);
                        //FunPriRateTypeMandotary(false);
                        FunPriFillRateTypeDropdown(false, false);
                        FunPriRatePatternDropdown(false, false, false, false, false, false);
                        FunPriTimeDropdown(false, false, false, false);
                        FunPriFrequencyDropdown(false, false, false, false, false, false, false);
                        FunPriRepaymentMode(false, false, false, false, false);
                        FunPriEnableIRRRest(false);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(false, false, false, false, false);
                        //FunPriIntrestLevyDropdown(false, false, false, false, false);
                        FunPriIntrestCalculationDropdown(false, false, false, false, false, false, false, false);
                        FunPriIntrestLevyDropdown(false, false, false, false, false, false, false, false);
                        //end
                        FunPriInterestCalculationMandotary(false);
                        FunPriRecoveryPatternMandotary(false);
                        FunPriInsuranceDropdown(false);
                        FunPriResidualValueDropdown(false);
                        FunPriMarginDropdown(false, true);

                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                        chkActive.Checked = true;
                        btnClear.Enabled = true;
                        chkActive.Enabled = false;
                        ddlLineofBusiness.Focus();
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
                        btnClear.Enabled = false;
                        chkActive.Enabled = true;
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
                        // FunPriValidationDefault(false, intModeID);
                        FunPriValidationDefault(false, 1);
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                        chkActive.Enabled = false;
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

    #region Page Load
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
                    intROIRuleMasterID = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }

            if (!IsPostBack)
            {

                FunPriLoadLOB_LIST();

                chkActive.Checked = false;
                //User Authorization Code starts
                if ((intROIRuleMasterID > 0) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if ((intROIRuleMasterID > 0) && (strMode == "Q")) // Query // Modify
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
                //User Authorization Code end
            }

            FunPriSetControlAttributes();
            FunPriSetMaxLength();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;

        }
    }
    #endregion

    #region Page User Defined Methods
    /// <summary>
    /// Set Control Attributes
    /// </summary>
    private void FunPriSetControlAttributes()
    {
        try
        {
            txtROIRuleNumber.Attributes.Add("onblur", "Trim('" + txtROIRuleNumber.ClientID + "');");
            txtModelDescription.Attributes.Add("onblur", "Trim('" + txtModelDescription.ClientID + "');");
            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            S3GSession S3GSession = new S3GSession();
            intGPSPrefix = S3GSession.ProGpsPrefixRW;
            intGPSSuffix = S3GSession.ProGpsSuffixRW;
            txtMarginPercentage.SetDecimalPrefixSuffix(2, 0, true, false, "Margin %");
            txtRecoveryPatternYear1.SetDecimalPrefixSuffix(2, 0, true, false, "Recovery Pattern Year 1");
            txtRecoveryPatternYear2.SetDecimalPrefixSuffix(2, 0, true, false, "Recovery Pattern Year 2");
            txtRecoveryPatternYear3.SetDecimalPrefixSuffix(2, 0, true, false, "Recovery Pattern Year 3");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriRateTypeMandotary(bool blnFlag)
    {
        try
        {
            rfvRateType.Enabled = blnFlag;
            lblRateType.Attributes.Add("class", "");

            rfvROIRuleNumber.Enabled = blnFlag;
            regROIRuleNumber.Enabled = blnFlag;
            lblROIRuleNumber.Attributes.Add("class", "");

            if (blnFlag)
            {
                lblRateType.Attributes.Add("class", "styleReqFieldLabel");
                lblROIRuleNumber.Attributes.Add("class", "styleReqFieldLabel");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriInterestCalculationMandotary(bool blnFlag)
    {
        try
        {
            rfvIntrestCalculation.Enabled = blnFlag;
            rfvIntrestLevy.Enabled = blnFlag;
            lblIntrestCalculation.Attributes.Add("class", "");
            lblIntrestLevy.Attributes.Add("class", "");

            if (blnFlag)
            {
                lblIntrestCalculation.Attributes.Add("class", "styleReqFieldLabel");
                lblIntrestLevy.Attributes.Add("class", "styleReqFieldLabel");

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriRecoveryPatternMandotary(bool blnFlag)
    {
        try
        {
            //rfvRecoveryPatternYear1.Enabled = blnFlag;
            lblRecoveryPatternYear1.Attributes.Add("class", "");
            if (blnFlag)
            {
                lblRecoveryPatternYear1.Attributes.Add("class", "styleReqFieldLabel");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriRateTypeDropdown(bool blnFlag)
    {
        try
        {
            //txtROIRuleNumber.Enabled = blnFlag;
            txtROIRuleNumber.Text = "";


            //ddlRateType.Enabled = blnFlag;
            ddlRateType.SelectedValue = "0";

            //rfvRateType.Enabled = blnFlag;
            //rfvROIRuleNumber.Enabled = blnFlag;
            rfvTime.Enabled = blnFlag;
            rfvFrequency.Enabled = blnFlag;
            //rfvIRRRest.Enabled = blnFlag;

            //rfvIntrestCalculation.Enabled = blnFlag;
            //rfvIntrestLevy.Enabled = blnFlag;

            rfvInsurance.Enabled = blnFlag;
            //rfvResidualValue.Enabled = blnFlag;


            //lblRateType.Attributes.Add("class", "");
            //lblROIRuleNumber.Attributes.Add("class", "");
            lblTime.Attributes.Add("class", "");
            lblFrequency.Attributes.Add("class", "");
            //lblIRRRest.Attributes.Add("class", "");

            //lblIntrestCalculation.Attributes.Add("class", "");
            //lblIntrestLevy.Attributes.Add("class", "");

            lblInsurance.Attributes.Add("class", "");
            //lblResidualValue.Attributes.Add("class", "");

            if (blnFlag)
            {
                //lblRateType.Attributes.Add("class", "styleReqFieldLabel");
                //lblROIRuleNumber.Attributes.Add("class", "styleReqFieldLabel");
                lblTime.Attributes.Add("class", "styleReqFieldLabel");
                lblFrequency.Attributes.Add("class", "styleReqFieldLabel");
                //lblIRRRest.Attributes.Add("class", "styleReqFieldLabel");

                //lblIntrestCalculation.Attributes.Add("class", "styleReqFieldLabel");
                //lblIntrestLevy.Attributes.Add("class", "styleReqFieldLabel");

                lblInsurance.Attributes.Add("class", "styleReqFieldLabel");
                //lblResidualValue.Attributes.Add("class", "styleReqFieldLabel");

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriFillRateTypeDropdown(bool blnFixed, bool blnFloating)
    {
        try
        {
            ddlRateType.Items.FindByValue("1").Enabled = blnFixed;
            ddlRateType.Items.FindByValue("2").Enabled = blnFloating;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriRatePatternDropdown(bool blnRate, bool blnIRR, bool blnPTF, bool blnPLF, bool blnPMF, bool blnPrincipal)
    {
        try
        {
            ddlRatePattern.Items.FindByValue("1").Enabled = blnRate;
            ddlRatePattern.Items.FindByValue("2").Enabled = blnIRR;
            ddlRatePattern.Items.FindByValue("3").Enabled = blnPTF;
            ddlRatePattern.Items.FindByValue("4").Enabled = blnPLF;
            ddlRatePattern.Items.FindByValue("5").Enabled = blnPMF;
            ddlRatePattern.Items.FindByValue("6").Enabled = blnPrincipal;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriTimeDropdown(bool blnADV, bool blnARR, bool blnADR, bool blnARF)
    {
        try
        {
            ddlTime.Enabled = true;
            ddlTime.Items.FindByValue("1").Enabled = blnADV;
            ddlTime.Items.FindByValue("2").Enabled = blnARR;
            ddlTime.Items.FindByValue("3").Enabled = blnADR;
            ddlTime.Items.FindByValue("4").Enabled = blnARF;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriFrequencyDropdown(bool blnWeekly, bool blnFortNightly, bool blnMonthly, bool blnBiMonthly, bool blnQuarterly, bool blnHalfYearly, bool blnAnnual)
    {
        try
        {
            ddlFrequency.Enabled = true;
            ddlFrequency.Items.FindByValue("2").Enabled = blnWeekly;
            ddlFrequency.Items.FindByValue("3").Enabled = blnFortNightly;
            ddlFrequency.Items.FindByValue("4").Enabled = blnMonthly;
            ddlFrequency.Items.FindByValue("5").Enabled = blnBiMonthly;
            ddlFrequency.Items.FindByValue("6").Enabled = blnQuarterly;
            ddlFrequency.Items.FindByValue("7").Enabled = blnHalfYearly;
            ddlFrequency.Items.FindByValue("8").Enabled = blnAnnual;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriRepaymentMode(bool blnEI, bool blnSAP, bool blnSF, bool blnProduct, bool blnTL)
    {
        try
        {
            ddlRepaymentMode.Items.FindByValue("1").Enabled = false;
            ddlRepaymentMode.Items.FindByValue("2").Enabled = false;
            ddlRepaymentMode.Items.FindByValue("3").Enabled = false;
            ddlRepaymentMode.Items.FindByValue("4").Enabled = blnProduct;
            ddlRepaymentMode.Items.FindByValue("5").Enabled = blnTL;
            ddlRatePattern.SelectedValue = "0";
            /*
            ddlRepaymentMode.Items.FindByValue("1").Enabled = blnEI;
            ddlRepaymentMode.Items.FindByValue("2").Enabled = blnSAP;
            ddlRepaymentMode.Items.FindByValue("3").Enabled = blnSF;
            ddlRepaymentMode.Items.FindByValue("4").Enabled = blnProduct;
            ddlRepaymentMode.Items.FindByValue("5").Enabled = blnTL;
            */
            /*ddlRepaymentMode.Items.FindByValue("1").Enabled = blnEI;
            if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 2)
            {
                ddlRepaymentMode.Items.FindByValue("2").Enabled = false;
                ddlRepaymentMode.Items.FindByValue("3").Enabled = false;
            }
            else
            {
                ddlRepaymentMode.Items.FindByValue("2").Enabled = blnSAP;
                ddlRepaymentMode.Items.FindByValue("3").Enabled = blnSF;
            }
            ddlRepaymentMode.Items.FindByValue("4").Enabled = blnProduct;
            ddlRepaymentMode.Items.FindByValue("5").Enabled = blnTL;
            */
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void Funrepaymode(bool blnEI, bool blnSAP, bool blnSF, bool blnProduct, bool blnTL)
    {
        try
        {
            ddlRepaymentMode.Items.FindByValue("1").Enabled = blnEI;
            ddlRepaymentMode.Items.FindByValue("2").Enabled = blnSAP;
            ddlRepaymentMode.Items.FindByValue("3").Enabled = blnSF;
            ddlRepaymentMode.Items.FindByValue("4").Enabled = blnProduct;
            ddlRepaymentMode.Items.FindByValue("5").Enabled = blnTL;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunprisetMandatoryRate(bool Blnflag)
    {
        try
        {
            rfvRate.Enabled = Blnflag;
            if (Blnflag)
                lblRate.Attributes.Add("class", "styleReqFieldLabel");
            else
                lblRate.Attributes.Add("class", "");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }

    }

    protected void ddlRatePattern_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //ddlRepaymentMode.Items.FindByValue("1").Enabled = true;
            //if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
            //{
            //    ddlRepaymentMode.Items.FindByValue("2").Enabled = true;
            //    ddlRepaymentMode.Items.FindByValue("3").Enabled = true;
            //}
            //else
            //{
            //    ddlRepaymentMode.Items.FindByValue("2").Enabled = false;
            //    ddlRepaymentMode.Items.FindByValue("3").Enabled = false;
            //}
            txtRate.Text = "";
            txtRecoveryPatternYear1.Text = "";
            txtRecoveryPatternYear2.Text = "";
            txtRecoveryPatternYear3.Text = "";
            txtRecoveryPatternYearRest.Text = "";

            string strType = ddlLineofBusiness.SelectedItem.Text.Split('-')[0].Trim();
            if ((ddlRatePattern.SelectedValue != "1" && ddlRatePattern.SelectedValue != "2") && ddlRepaymentMode.SelectedValue == "3")
                FunprisetMandatoryRate(false);
            else
                FunprisetMandatoryRate(true);
            switch (strType.ToLower())
            {
                case "hp"://Hire Purchase
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(true, true, true, false, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 2)//IRR
                        Funrepaymode(true, true, false, false, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(true, false, false, false, false);
                    break;
                case "ln"://Loan
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(true, true, true, false, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(true, false, false, false, false);
                    break;
                case "pl"://Property Loan
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(true, true, true, false, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(true, false, false, false, false);
                    break;
                case "fl"://Financial Lease
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(true, true, true, false, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        //Funrepaymode(true, false, true, false, false);
                        Funrepaymode(true, true, true, false, false);//modified on 17-11-2011 by saran based on the observation raised by RS.
                    else
                        Funrepaymode(true, false, false, false, false);
                    break;
                case "ol"://Operating Lease
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(true, true, true, false, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        //Funrepaymode(true, false, true, false, false);
                        Funrepaymode(true, true, true, false, false);//modified on 2-12-2011 by saran based on the observation raised by RS.
                    else
                        Funrepaymode(true, false, false, false, false);
                    break;
                case "ft"://Factoring
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(false, false, false, true, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(false, false, false, false, false);
                    break;
                case "tl"://Term Loan 
                    /*if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(false, false, false, false, true);
                    else
                        Funrepaymode(false, false, false, false, false);*/
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(true, true, true, false, true);//modified by kali on 29/nov/2012 as per the instruction givrn by Bashyam sir
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 6)//principal
                        Funrepaymode(true, true,false , false, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(true, false, false, false, false);
                    break;
            
                case "te"://Term Loan Extendable
                    /* if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                         Funrepaymode(false, false, false, false, true);//modified by kali on 29/nov/2012 as per the instruction givrn by Bashyam sir
                     else
                         Funrepaymode(false, false, false, false, false);*/
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(true, true, true, false, true);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 6)//principal
                        Funrepaymode(true, true, false, false, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(true, false, false, false, false);
                    break;

                case "wc"://Working Capital
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(false, false, false, true, false);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(false, false, false, false, false);
                    break;
                case "vf"://Vendor Finance
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(false, false, false, false, true);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(false, false, false, false, false);
                    break;
                case "cf"://Channel Finance
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(false, false, false, false, true);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(false, false, false, false, false);
                    break;
                case "sf"://Security Finance
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(false, false, false, false, true);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(false, false, false, false, false);
                    break;
                case "of"://Commodity Finance
                    if (Convert.ToInt32(ddlRatePattern.SelectedValue) == 1)
                        Funrepaymode(false, false, false, false, true);
                    else if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 2)
                        Funrepaymode(true, false, true, false, false);
                    else
                        Funrepaymode(false, false, false, false, false);
                    break;
                default:

                    break;

            }
            //ddlTime.Focus();
            ddlRatePattern.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriEnableIRRRest(bool blnFlag)
    {
        try
        {
            ddlIRRRest.SelectedValue = "0";

            rfvIRRRest.Enabled = blnFlag;
            lblIRRRest.Attributes.Add("class", "");
            if (blnFlag)
            {
                lblIRRRest.Attributes.Add("class", "styleReqFieldLabel");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriIntrestCalculationDropdown(bool blnDaily, bool blnWeekly, bool blnFortNightly, bool blnMonthly, bool blnBiMonthly, bool blnQuarterly, bool blnHalfYearly, bool blnAnnual)
    {
        try
        {
            ddlIntrestCalculation.Items.FindByValue("1").Enabled = blnDaily;
            ddlIntrestCalculation.Items.FindByValue("2").Enabled = blnWeekly;
            ddlIntrestCalculation.Items.FindByValue("3").Enabled = blnFortNightly;
            ddlIntrestCalculation.Items.FindByValue("4").Enabled = blnMonthly;
            ddlIntrestCalculation.Items.FindByValue("5").Enabled = blnBiMonthly;
            ddlIntrestCalculation.Items.FindByValue("6").Enabled = blnQuarterly;
            ddlIntrestCalculation.Items.FindByValue("7").Enabled = blnHalfYearly;
            ddlIntrestCalculation.Items.FindByValue("8").Enabled = blnAnnual;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriIntrestLevyDropdown(bool blnDaily, bool blnWeekly, bool blnFortNightly, bool blnMonthly, bool blnBiMonthly, bool blnQuarterly, bool blnHalfYearly, bool blnAnnual)
    {
        try
        {
            ddlIntrestLevy.Items.FindByValue("1").Enabled = blnDaily;
            ddlIntrestLevy.Items.FindByValue("2").Enabled = blnWeekly;
            ddlIntrestLevy.Items.FindByValue("3").Enabled = blnFortNightly;
            ddlIntrestLevy.Items.FindByValue("4").Enabled = blnMonthly;
            ddlIntrestLevy.Items.FindByValue("5").Enabled = blnBiMonthly;
            ddlIntrestLevy.Items.FindByValue("6").Enabled = blnQuarterly;
            ddlIntrestLevy.Items.FindByValue("7").Enabled = blnHalfYearly;
            ddlIntrestLevy.Items.FindByValue("8").Enabled = blnAnnual;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }

    }

    private void FunPriInsuranceDropdown(bool blnFlag)
    {
        try
        {
            //ddlInsurance.Enabled = blnFlag;
            //ADDED
            ddlInsurance.Enabled = true;
            //END
            ddlInsurance.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriResidualValueDropdown(bool blnFlag)
    {
        try
        {
            //ddlResidualValue.Enabled = blnFlag;
            ddlResidualValue.SelectedValue = "0";

            rfvResidualValue.Enabled = blnFlag;

            lblResidualValue.Attributes.Add("class", "");
            if (blnFlag)
            {
                lblResidualValue.Attributes.Add("class", "styleReqFieldLabel");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriMarginDropdown(bool blnApplicable, bool blnNotApplicable)
    {
        try
        {
            ddlMargin.Items.FindByValue("1").Enabled = blnApplicable;
            ddlMargin.Items.FindByValue("0").Enabled = blnNotApplicable;
            ddlMargin.SelectedValue = "0";

            rfvMargin.Enabled = blnApplicable;
            lblMargin.Attributes.Add("class", "");

            //txtMarginPercentage.Enabled = blnApplicable;
            //rfvMarginPercentage.Enabled = blnApplicable;
            //lblMarginPercentage.Attributes.Add("class", "");

            txtMarginPercentage.Text = "";
            txtMarginPercentage.Enabled = false;
            rfvMarginPercentage.Enabled = false;
            lblMarginPercentage.Attributes.Add("class", "");
            if (blnApplicable)
            {
                lblMargin.Attributes.Add("class", "styleReqFieldLabel");
                //lblMarginPercentage.Attributes.Add("class", "styleReqFieldLabel");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriMarginPercentage(int intMarginID)
    {
        try
        {
            txtMarginPercentage.Text = "";
            txtMarginPercentage.Enabled = false;
            rfvMarginPercentage.Enabled = false;
            lblMarginPercentage.Attributes.Add("class", "");
            if (intMarginID == 1)
            {
                txtMarginPercentage.Enabled = true;
                rfvMarginPercentage.Enabled = true;
                lblMarginPercentage.Attributes.Add("class", "styleReqFieldLabel");
                //txtMarginPercentage.Focus();
                ddlMargin.Focus();
                //lblMarginPercentage.Attributes.Add("class", "");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }

    }
    /// <summary>
    /// LOB Change Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLineofBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriRateTypeDropdown(false);
            //FunPriRateTypeMandotary(false);
            FunPriFillRateTypeDropdown(false, false);
            FunPriRatePatternDropdown(false, false, false, false, false, false);
            FunPriTimeDropdown(false, false, false, false);
            FunPriFrequencyDropdown(false, false, false, false, false, false, false);
            FunPriRepaymentMode(false, false, false, false, false);
            FunPriEnableIRRRest(false);
            //Commented by Saranya
            //FunPriIntrestCalculationDropdown(false, false, false, false, false);
            FunPriIntrestCalculationDropdown(false, false, false, false, false, false, false, false);
            //end
            FunPriInterestCalculationMandotary(false);
            FunPriRecoveryPatternMandotary(false);
            txtRecoveryPatternYear1.Enabled =
        txtRecoveryPatternYear2.Enabled =
        txtRecoveryPatternYear3.Enabled =
        txtRecoveryPatternYearRest.Enabled = false;
            //Commented by Saranya
            //FunPriIntrestLevyDropdown(false, false, false, false, false);
            FunPriIntrestLevyDropdown(false, false, false, false, false, false, false, false);
            //end
            FunPriInsuranceDropdown(false);
            FunPriResidualValueDropdown(false);
            FunPriMarginDropdown(false, true);
            txtModelDescription.Text =
            txtRate.Text = "";
            txtRecoveryPatternYear1.Text =
            txtRecoveryPatternYear2.Text =
            txtRecoveryPatternYear3.Text =
            txtRecoveryPatternYearRest.Text = "";//cannot be 0

            ddlRateType.SelectedValue = "0";
            ddlRatePattern.SelectedValue = "0";
            ddlTime.SelectedValue = "0";
            ddlFrequency.SelectedValue = "0";
            ddlRepaymentMode.SelectedValue = "0";
            ddlIRRRest.SelectedValue = "0";
            ddlIntrestCalculation.SelectedValue = "0";
            ddlIntrestLevy.SelectedValue = "0";
            ddlInsurance.Enabled = true;
            ddlInsurance.SelectedValue = "0";
            ddlResidualValue.SelectedValue = "0";
            ddlMargin.SelectedValue = "0";
            ddlResidualValue.Enabled = false;
            string strType = ddlLineofBusiness.SelectedItem.Text.Split('-')[0].Trim();

            switch (strType.ToLower())
            {
                case "hp":  //Hire Purchase
                    {
                        FunPriRateTypeDropdown(true);

                        //FunPriRateTypeMandotary(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRepaymentMode(true, true, true, false, false);
                        FunPriRatePatternDropdown(true, true, false, false, false, false);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);

                        FunPriEnableIRRRest(true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriInsuranceDropdown(true);
                        FunPriMarginDropdown(true, true);
                        break;
                    }
                case "ln": //Loan
                    {

                        FunPriRateTypeDropdown(true);

                        //FunPriRateTypeMandotary(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, true, false, false, false, true);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        FunPriRepaymentMode(true, true, true, false, false);
                        FunPriEnableIRRRest(true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriInsuranceDropdown(true);
                        FunPriMarginDropdown(true, true);
                        break;
                    }

                case "pl": //Loan
                    {

                        FunPriRateTypeDropdown(true);

                        //FunPriRateTypeMandotary(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, true, false, false, false, true);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        FunPriRepaymentMode(true, true, true, false, false);
                        FunPriEnableIRRRest(true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriInsuranceDropdown(true);
                        FunPriMarginDropdown(true, true);
                        break;
                    }


                case "fl":  //Financial Leasing
                    {
                        FunPriRateTypeDropdown(true);

                        //FunPriRateTypeMandotary(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, true, true, true, true, true);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        FunPriRepaymentMode(true, true, true, false, false);
                        FunPriEnableIRRRest(true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriInsuranceDropdown(true);
                        FunPriResidualValueDropdown(true);
                        ddlResidualValue.Enabled = true;
                        break;
                    }
                case "ol":  //Operating Lease
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, true, true, true, true, true);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        FunPriRepaymentMode(true, true, true, false, false);
                        FunPriEnableIRRRest(true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriInsuranceDropdown(true);
                        FunPriResidualValueDropdown(true);
                        ddlResidualValue.Enabled = true;
                        break;
                    }
                case "ft":  //Factoring
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRepaymentMode(false, false, false, true, false);
                        FunPriRatePatternDropdown(true, false, false, false, false, false);


                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, true, true, true);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, true, true, true);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(true);

                        FunPriMarginDropdown(true, true);
                        //ADDED by Saranya
                        ddlInsurance.SelectedIndex = 0;
                        ddlInsurance.Enabled = false;
                        //END
                        //added by saranya on 3-Mar-2012 for Bug Id 5538 point 1.
                        ddlFrequency.SelectedIndex = 0;
                        ddlFrequency.Enabled = false;
                        ddlTime.SelectedIndex = 0;
                        ddlTime.Enabled = false;
                        break;
                    }
                case "tl":  //Term Loan
                    {
                        /* FunPriFillRateTypeDropdown(true, true);
                         FunPriRatePatternDropdown(true, false, false, false, false);
                         FunPriRepaymentMode(false, false, false, false, true);
                         FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                         FunPriTimeDropdown(true, true, true, true);
                         FunPriIntrestCalculationDropdown(true, true, true, true, true);
                         FunPriIntrestLevyDropdown(true, true, true, true, true);
                         FunPriInterestCalculationMandotary(true);
                         FunPriRateTypeDropdown(true);
                         break;*/

                        //Method uncommented to make Time and Frequency mandatory for TL & TLE - Kuppusamy.B - 02-Apr-2012
                        FunPriRateTypeDropdown(true);

                        //FunPriRateTypeMandotary(true);
                        FunPriFillRateTypeDropdown(true, true);
                        //code modified to Load Product For TL & TLE - Kuppusamy.B - 31-Mar-2012
                        FunPriRepaymentMode(true, true, true, false, true);
                        //FunPriRepaymentMode(true, true, true, true, false);
                        FunPriRatePatternDropdown(true, true, false, false, false, true);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);

                        FunPriEnableIRRRest(true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriInsuranceDropdown(true);
                        FunPriMarginDropdown(true, true);
                        break;
                    }
              
                case "te":  //Term Loan Extentible
                    {
                        /* FunPriFillRateTypeDropdown(true, true);
                         FunPriRatePatternDropdown(true, false, false, false, false);
                         //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                         //FunPriIntrestLevyDropdown(true, true, false, false, false);
                         FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                         FunPriTimeDropdown(true, true, true, true);
                         FunPriIntrestCalculationDropdown(true, true, true, true, true);
                         FunPriIntrestLevyDropdown(true, true, true, true, true);
                         FunPriInterestCalculationMandotary(true);
                         FunPriRateTypeDropdown(true);*/

                        //Method uncommented to make Time and Frequency mandatory for TL & TLE - Kuppusamy.B - 02-Apr-2012
                        FunPriRateTypeDropdown(true);

                        //FunPriRateTypeMandotary(true);
                        FunPriFillRateTypeDropdown(true, true);
                        //code modified to Load Product For TL & TLE - Kuppusamy.B - 31-Mar-2012
                        FunPriRepaymentMode(true, true, true, false, true);
                        //FunPriRepaymentMode(true, true, true, true, false);
                        FunPriRatePatternDropdown(true, true, false, false, false, true);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);

                        FunPriEnableIRRRest(true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriInsuranceDropdown(true);
                        FunPriMarginDropdown(true, true);
                        break;
                    }
                case "wc":  //Working Capital
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false, false);
                        FunPriRepaymentMode(false, false, false, true, false);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, true, true, true);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, true, true, true);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(true);

                        FunPriMarginDropdown(true, true);
                        //ADDED by Saranya
                        ddlInsurance.SelectedIndex = 0;
                        ddlInsurance.Enabled = false;
                        //END
                        //added by saranya on 3-Mar-2012 for Bug Id 5538 point 1.
                        ddlFrequency.SelectedIndex = 0;
                        ddlFrequency.Enabled = false;
                        ddlTime.SelectedIndex = 0;
                        ddlTime.Enabled = false;
                        //End Here
                        break;
                    }
                case "vf":  //Vendor Finance
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false, false);

                        FunPriTimeDropdown(true, true, false, false);
                        FunPriFrequencyDropdown(false, false, true, false, true, true, false);
                        FunPriRepaymentMode(false, false, false, false, true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriMarginDropdown(true, true);
                        break;
                    }
                case "cf":  //Channel Finance
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false, false);

                        FunPriTimeDropdown(true, true, false, false);
                        FunPriFrequencyDropdown(false, false, true, false, true, true, false);
                        FunPriRepaymentMode(false, false, false, false, true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(true, true, false, false, false);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(true, true, false, false, false);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(false);//aug-11

                        FunPriMarginDropdown(true, true);
                        break;
                    }

                case "sf":  //Security Finance
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false, false);

                        FunPriRepaymentMode(false, false, false, false, true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(false, false, true, true, true);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(false, false, true, true, true);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(true);

                        FunPriMarginDropdown(true, true);
                        break;
                    }
                case "of":  //Commodity Finance
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false, false);

                        FunPriRepaymentMode(false, false, false, false, true);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(false, false, true, true, true);
                        FunPriIntrestCalculationDropdown(true, true, true, true, true, true, true, true);
                        //FunPriIntrestLevyDropdown(false, false, true, true, true);
                        FunPriIntrestLevyDropdown(true, true, true, true, true, true, true, true);
                        FunPriInterestCalculationMandotary(true);

                        FunPriMarginDropdown(true, true);
                        break;
                    }
                default:
                    {
                        //FunPriRateTypeMandotary(false);
                        FunPriFillRateTypeDropdown(false, false);
                        FunPriRatePatternDropdown(true, false, false, false, false, false);

                        FunPriRepaymentMode(false, false, false, false, false);
                        FunPriEnableIRRRest(false);
                        //Commented by Saranya
                        //FunPriIntrestCalculationDropdown(false, false, false, false, false);
                        FunPriIntrestCalculationDropdown(false, false, false, false, false, false, false, false);

                        //Commented by Saranya
                        //FunPriIntrestLevyDropdown(false, false, false, false, false);
                        FunPriIntrestLevyDropdown(false, false, false, false, false, false, false, false);
                        //end
                        FunPriInterestCalculationMandotary(false);
                        FunPriRecoveryPatternMandotary(false);
                        txtRecoveryPatternYear1.Enabled =
                        txtRecoveryPatternYear2.Enabled =
                        txtRecoveryPatternYear3.Enabled =
                        txtRecoveryPatternYearRest.Enabled = false;
                        ddlResidualValue.Enabled = false;
                        break;
                    }
            }
            ddlLineofBusiness.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// RateType SelectedIndex Changed 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            regROIRuleNumber.Enabled = false;
            if (Convert.ToInt32(ddlRateType.SelectedValue) == 1)
            {
                txtROIRuleNumber.Text = "RRA";
                regROIRuleNumber.ValidationExpression = @"RRA[0-4][0-9][0-9]|RRA500";
                regROIRuleNumber.ErrorMessage = "ROI rule number should be in RRA[001-500]";
                regROIRuleNumber.Enabled = true;
                //txtROIRuleNumber.Focus();
            }
            else if (Convert.ToInt32(ddlRateType.SelectedValue) == 2)
            {
                txtROIRuleNumber.Text = "RRB";
                regROIRuleNumber.ValidationExpression = @"RRB[5-9][0-9][0-9]";
                regROIRuleNumber.ErrorMessage = "ROI rule number should be in RRB[501-999]";
                regROIRuleNumber.Enabled = true;
                //txtROIRuleNumber.Focus();
            }
            else
            {
                txtROIRuleNumber.Text = "";
                regROIRuleNumber.ValidationExpression = "";
                regROIRuleNumber.ErrorMessage = "";
            }
            ddlRateType.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    /// <summary>
    /// RepaymentMode dropdown selectedindex
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRepaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriRecoveryPatternMandotary(false);
            txtRecoveryPatternYear1.Text =
            txtRecoveryPatternYear2.Text =
            txtRecoveryPatternYear3.Text =
            txtRecoveryPatternYearRest.Text = "";//cannot be 0
            txtRecoveryPatternYear1.Enabled = false;

            string[] lob = (ddlLineofBusiness.SelectedItem.Text.ToLower()).Split('-');

            //added by saranya on 13-Feb-2012 to Disabled the Frequency DDL if Lob and Repayment Mode is TE  and Product
            // if (ddlLineofBusiness.SelectedItem.Text.ToUpper().StartsWith("TE") && ddlRepaymentMode.SelectedItem.Text.StartsWith("Pro"))

            //Code modified by Kuppusamy.B - 16-April-2012 - bug_ID - 6129
            if (((lob[0].ToString()).Trim() == "te" || (lob[0].ToString()).Trim() == "tl"  || (lob[0].ToString()).Trim() == "ft") && (ddlRepaymentMode.SelectedItem.Text.StartsWith("Pro") || ddlRepaymentMode.SelectedIndex == 0))
            {
                ddlFrequency.SelectedIndex = 0;
                ddlFrequency.Enabled = false;
                ddlTime.SelectedIndex = 0;
                ddlTime.Enabled = false;

                lblTime.Attributes.Add("class", "styleDisplayLabel");
                lblFrequency.Attributes.Add("class", "styleDisplayLabel");

                rfvTime.Enabled = false;
                rfvFrequency.Enabled = false;
            }
            else
            {
                ddlFrequency.Enabled = true;
                ddlTime.Enabled = true;

                lblTime.Attributes.Add("class", "styleReqFieldLabel");
                lblFrequency.Attributes.Add("class", "styleReqFieldLabel");

                rfvTime.Enabled = true;
                rfvFrequency.Enabled = true;
            }
            //end here
            if (Convert.ToInt32(ddlRepaymentMode.SelectedValue) == 3)
            {
                if (ddlRatePattern.SelectedValue != "1" && ddlRatePattern.SelectedValue != "2")
                    FunprisetMandatoryRate(false);
                else
                    FunprisetMandatoryRate(true);
                FunPriRecoveryPatternMandotary(true);
                txtRecoveryPatternYear1.Enabled =
                txtRecoveryPatternYear2.Enabled =
                txtRecoveryPatternYear3.Enabled =
                txtRecoveryPatternYearRest.Enabled = true;
            }
            //txtRate.Focus();
            ddlRepaymentMode.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }
    /// <summary>
    /// Margin dropdown selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlMargin_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriMarginPercentage(Convert.ToInt32(ddlMargin.SelectedValue));
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
        string strROIRuleType = string.Empty;
        ObjROIRuleMasterClient = new RuleCardMgtServicesClient();
        if (intROIRuleMasterID == 0)
        {
            if (!txtROIRuleNumber.Text.Equals(""))
            {
                ///// Check the entered rule number must starts with (RRA OR RRB)
                //strROIRuleType = txtROIRuleNumber.Text.Trim().Substring(0, 3);
                //strROIRuleType = strROIRuleType.ToUpper().Replace("RRA", "").Replace("RRB", "");
                //if (strROIRuleType.Length > 0)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "ROI rule number should be starts with RRA or RRB");
                //    return;
                //}
                ///// Check the entered rule number prefix (RRA/RRB) and selected RateType must match
                //strROIRuleType = txtROIRuleNumber.Text.Trim().Substring(0, 3);
                //strROIRuleType = strROIRuleType.ToUpper().Replace("RRA", "1").Replace("RRB", "2");
                //if (Convert.ToInt32(strROIRuleType) != Convert.ToInt32(ddlRateType.SelectedValue))
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Entered ROI rule number should be starts with RRA or RRB");
                //    return;
                //}


                if (txtROIRuleNumber.Text.Trim().Substring(0, 3) == "RRA")
                {
                    string strNumber = txtROIRuleNumber.Text.Trim().Substring(3);
                    if (strNumber.Length > 0)
                    {
                        int intNumber = Convert.ToInt32(txtROIRuleNumber.Text.Trim().Substring(3));
                        if (intNumber < 1 || intNumber > 500)
                        {
                            Utility.FunShowAlertMsg(this.Page, "ROI rule number should be in 001 to 500");
                            return;
                        }
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this.Page, "ROI rule number should be in 001 to 500");
                        return;
                    }
                }
                else if (txtROIRuleNumber.Text.Trim().Substring(0, 3) == "RRB")
                {
                    string strNumber = txtROIRuleNumber.Text.Trim().Substring(3);
                    if (strNumber.Length > 0)
                    {
                        int intNumber = Convert.ToInt32(txtROIRuleNumber.Text.Trim().Substring(3));
                        if (intNumber < 501 || intNumber > 999)
                        {
                            Utility.FunShowAlertMsg(this.Page, "ROI rule number should be in 501 to 999");
                            return;
                        }
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this.Page, "ROI rule number should be in 501 to 999");
                        return;
                    }
                }
            }

            /*if (txtRecoveryPatternYear1.Text == "" && ddlRepaymentMode.SelectedValue == "3")
            {
                Utility.FunShowAlertMsg(this.Page, "Recovery Pattern1 should not be empty");
                txtRecoveryPatternYear1.Focus();
                return;
            }
            else if (txtRecoveryPatternYear2.Text == "" && ddlRepaymentMode.SelectedValue == "3")
            {
                Utility.FunShowAlertMsg(this.Page, "Recovery Pattern2 should not be empty");
                txtRecoveryPatternYear2.Focus();
                return;
            }
            else if (txtRecoveryPatternYear3.Text == "" && ddlRepaymentMode.SelectedValue == "3")
            {
                Utility.FunShowAlertMsg(this.Page, "Recovery Pattern3 should not be empty");
                txtRecoveryPatternYear3.Focus();
                return;
            }
            else if (txtRecoveryPatternYearRest.Text == "" && ddlRepaymentMode.SelectedValue == "3")
            {
                Utility.FunShowAlertMsg(this.Page, "Recovery PatternRest should not be empty");
                txtRecoveryPatternYearRest.Focus();
                return;
            }
            if (ddlRepaymentMode.SelectedValue == "3")
            {
                decimal Total = 0;
                Total = Convert.ToDecimal(txtRecoveryPatternYear1.Text) + Convert.ToDecimal(txtRecoveryPatternYear2.Text) + Convert.ToDecimal(txtRecoveryPatternYear3.Text) + Convert.ToDecimal(txtRecoveryPatternYearRest.Text);
                if (Total > 100)
                {
                    if (txtRecoveryPatternYear2.Text != "" || txtRecoveryPatternYear2.Text != "0.00")
                    {
                        txtRecoveryPatternYear2.Enabled = true;
                    }
                    if (txtRecoveryPatternYear3.Text != "" || txtRecoveryPatternYear3.Text != "0.00")
                    {
                        txtRecoveryPatternYear3.Enabled = true;
                    }
                    Utility.FunShowAlertMsg(this.Page, "Recovery Pattern should not exceed 100%");
                    txtRecoveryPatternYear1.Focus();
                    return;
                }
                else if (Total > 100 && Total != 0)
                {
                    if (txtRecoveryPatternYear2.Text != "" || txtRecoveryPatternYear2.Text != "0.00")
                    {
                        txtRecoveryPatternYear2.Enabled = true;
                    }
                    if (txtRecoveryPatternYear3.Text != "" || txtRecoveryPatternYear3.Text != "0.00")
                    {
                        txtRecoveryPatternYear3.Enabled = true;
                    }
                    Utility.FunShowAlertMsg(this.Page, "Recovery Pattern should be equal to 100%");
                    txtRecoveryPatternYear1.Focus();
                    return;
                }
            }*/

            #region "Recovery Pattern Validations"
            if (ddlRepaymentMode.SelectedValue == "3")
            {
                if (ddlRatePattern.SelectedValue == "1" || ddlRatePattern.SelectedValue == "2")//Recovery Pattern should be a percentage
                {
                    decimal Total = 0;
                    if (!string.IsNullOrEmpty(txtRecoveryPatternYear1.Text))
                        Total += Convert.ToDecimal(txtRecoveryPatternYear1.Text);
                    if (!string.IsNullOrEmpty(txtRecoveryPatternYear2.Text))
                        Total += Convert.ToDecimal(txtRecoveryPatternYear2.Text);
                    if (!string.IsNullOrEmpty(txtRecoveryPatternYear3.Text))
                        Total += Convert.ToDecimal(txtRecoveryPatternYear3.Text);
                    if (!string.IsNullOrEmpty(txtRecoveryPatternYearRest.Text))
                        Total += Convert.ToDecimal(txtRecoveryPatternYearRest.Text);
                    if (Total != 100)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Recovery Pattern should be 100%");
                        txtRecoveryPatternYear1.Focus();
                        return;
                    }
                }
                else//Recovery Pattern should be a amount
                {
                    decimal Total = 0;
                    if (!string.IsNullOrEmpty(txtRecoveryPatternYear1.Text))
                        Total += Convert.ToDecimal(txtRecoveryPatternYear1.Text);
                    if (!string.IsNullOrEmpty(txtRecoveryPatternYear2.Text))
                        Total += Convert.ToDecimal(txtRecoveryPatternYear2.Text);
                    if (!string.IsNullOrEmpty(txtRecoveryPatternYear3.Text))
                        Total += Convert.ToDecimal(txtRecoveryPatternYear3.Text);
                    if (!string.IsNullOrEmpty(txtRecoveryPatternYearRest.Text))
                        Total += Convert.ToDecimal(txtRecoveryPatternYearRest.Text);
                    if (Total < 1)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Enter Recovery Pattern years");
                        txtRecoveryPatternYear1.Focus();
                        return;
                    }
                }

                if (string.IsNullOrEmpty(txtRecoveryPatternYear1.Text))//Recovery pattern 1.
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter Recovery Pattern year 1");
                    txtRecoveryPatternYear1.Focus();
                    return;
                }
                if ((string.IsNullOrEmpty(txtRecoveryPatternYear2.Text)) &&
                    (!string.IsNullOrEmpty(txtRecoveryPatternYear1.Text)) &&
                    (!string.IsNullOrEmpty(txtRecoveryPatternYear3.Text)))
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter Recovery Pattern year 2");//Recovery pattern 2.
                    txtRecoveryPatternYear2.Focus();
                    return;
                }
                if ((string.IsNullOrEmpty(txtRecoveryPatternYear3.Text)) &&
                    (!string.IsNullOrEmpty(txtRecoveryPatternYear2.Text)) &&
                    (!string.IsNullOrEmpty(txtRecoveryPatternYearRest.Text)))
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter Recovery Pattern year 3");//Recovery pattern 3.
                    txtRecoveryPatternYear3.Focus();
                    return;
                }
                if ((!string.IsNullOrEmpty(txtRecoveryPatternYearRest.Text)) &&
                    (string.IsNullOrEmpty(txtRecoveryPatternYear3.Text)))
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter Recovery Pattern year 3");//Recovery pattern Rest.
                    txtRecoveryPatternYear3.Focus();
                    return;
                }
            }
            #endregion

        }
        try
        {
            ObjROIRuleMasterRow = ObjROIRuleMasterDataTable.NewS3G_ORG_ROI_RulesRow();
            ObjROIRuleMasterRow.ROI_Rules_ID = intROIRuleMasterID;
            if (txtSerialNumber.Text != "")
            {
                ObjROIRuleMasterRow.Serial_Number = Convert.ToInt64(txtSerialNumber.Text);
            }
            ObjROIRuleMasterRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
            ObjROIRuleMasterRow.LOB_ID = Convert.ToInt32(ddlLineofBusiness.SelectedValue);
            ObjROIRuleMasterRow.Model_Description = txtModelDescription.Text;

            if (Convert.ToInt32(ddlRateType.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Rate_Type = Convert.ToInt32(ddlRateType.Text);
            }
            if (txtROIRuleNumber.Text != "")
            {
                ObjROIRuleMasterRow.ROI_Rule_Number = txtROIRuleNumber.Text;
            }

            if (Convert.ToInt32(ddlRatePattern.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Return_Pattern = Convert.ToInt32(ddlRatePattern.Text);
            }
            if (Convert.ToInt32(ddlTime.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Time_Value = Convert.ToInt32(ddlTime.Text);
            }

            if (Convert.ToInt32(ddlFrequency.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Frequency = Convert.ToInt32(ddlFrequency.Text);
            }
            if (Convert.ToInt32(ddlRepaymentMode.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Repayment_Mode = Convert.ToInt32(ddlRepaymentMode.Text);
            }
            if (!string.IsNullOrEmpty(txtRate.Text))
                ObjROIRuleMasterRow.Rate = Convert.ToDecimal(txtRate.Text);
            ObjROIRuleMasterRow.IRR_Rest = Convert.ToInt32(ddlIRRRest.SelectedValue.ToString());

            if (Convert.ToInt32(ddlIntrestCalculation.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Interest_Calculation = Convert.ToInt32(ddlIntrestCalculation.Text);
            }
            if (Convert.ToInt32(ddlIntrestLevy.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Interest_Levy = Convert.ToInt32(ddlIntrestLevy.Text);
            }

            if (txtRecoveryPatternYear1.Text != "")
            {
                ObjROIRuleMasterRow.Recovery_Pattern_Year1 = Convert.ToDecimal(txtRecoveryPatternYear1.Text);
            }
            if (txtRecoveryPatternYear2.Text != "")
            {
                ObjROIRuleMasterRow.Recovery_Pattern_Year2 = Convert.ToDecimal(txtRecoveryPatternYear2.Text);
            }
            if (txtRecoveryPatternYear3.Text != "")
            {
                ObjROIRuleMasterRow.Recovery_Pattern_Year3 = Convert.ToDecimal(txtRecoveryPatternYear3.Text);
            }
            if (txtRecoveryPatternYearRest.Text != "")
            {
                ObjROIRuleMasterRow.Recovery_Pattern_Rest = Convert.ToDecimal(txtRecoveryPatternYearRest.Text);
            }
            if (Convert.ToInt32(ddlInsurance.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Insurance = Convert.ToInt32(ddlInsurance.Text);
            }
            //if (Convert.ToInt32(ddlResidualValue.SelectedValue) > 0)
            //{
            ObjROIRuleMasterRow.Residual_Value = Convert.ToBoolean(Convert.ToInt32(ddlResidualValue.SelectedValue));
            //}
            if (Convert.ToInt32(ddlMargin.SelectedValue) > 0)
            {
                ObjROIRuleMasterRow.Margin = Convert.ToBoolean(Convert.ToInt32(ddlMargin.SelectedValue));
            }
            if (txtMarginPercentage.Text != "")
            {
                ObjROIRuleMasterRow.Margin_Percentage = Convert.ToDecimal(txtMarginPercentage.Text);
            }
            ObjROIRuleMasterRow.Is_Active = chkActive.Checked;
            ObjROIRuleMasterRow.Created_By = ObjUserInfo.ProUserIdRW;
            ObjROIRuleMasterRow.Modified_By = ObjUserInfo.ProUserIdRW;
            ObjROIRuleMasterDataTable.AddS3G_ORG_ROI_RulesRow(ObjROIRuleMasterRow);


            //ObjROIRuleMasterClient = new RuleCardMgtServicesClient();

            if (intROIRuleMasterID > 0)
            {
                intErrCode = ObjROIRuleMasterClient.FunPubModifyROIRulesInt(intROIRuleMasterID, chkActive.Checked, ObjUserInfo.ProUserIdRW);
            }
            else if (intROIRuleMasterID == 0)
            {
                intErrCode = ObjROIRuleMasterClient.FunPubCreateROIRulesInt(SerMode, ClsPubSerialize.Serialize(ObjROIRuleMasterDataTable, SerMode));
            }
            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intROIRuleMasterID > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "ROI rule updated successfully");
                }
                else
                {
                    strAlert = "ROI rule added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
            }
            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "The entered ROI rule number already exists.");
                strRedirectPageView = "";
            }

            else if (intErrCode == 3)
            {
                strAlert = strAlert.Replace("__ALERT__", "The entered Model Description already exists.");
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
        }
        catch (FaultException<RuleCardMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjROIRuleMasterClient.Close();
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
            ddlLineofBusiness.SelectedValue = "0";
            txtModelDescription.Text = "";
            txtRate.Text = "";
            ddlResidualValue.Enabled = false;
            /*
        
            ddlRateType.SelectedValue = "0";
            txtROIRuleNumber.Text = "";
            ddlRatePattern.SelectedValue = "0";
            ddlTime.SelectedValue = "0";
            ddlFrequency.SelectedValue = "0";
            ddlRepaymentMode.SelectedValue = "0";
            txtRate.Text = "";
            ddlIRRRest.SelectedValue = "0";
            ddlIntrestCalculation.SelectedValue = "0";
            ddlIntrestLevy.SelectedValue = "0";

      
            FunPriRecoveryPatternMandotary(false);
            ddlInsurance.SelectedValue = "0";
            ddlResidualValue.SelectedValue = "0";
            ddlMargin.SelectedValue = "0";
            FunPriMarginPercentage(Convert.ToInt32(ddlMargin.SelectedValue));
            //txtMarginPercentage.Text = "";
            */
            FunPriRateTypeDropdown(false);
            //FunPriRateTypeMandotary(false);
            FunPriFillRateTypeDropdown(false, false);
            FunPriRatePatternDropdown(false, false, false, false, false, false);

            FunPriTimeDropdown(false, false, false, false);
            ddlFrequency.Enabled = true;
            ddlTime.Enabled = true;

            FunPriFrequencyDropdown(false, false, false, false, false, false, false);
            FunPriRepaymentMode(false, false, false, false, false);
            FunPriEnableIRRRest(false);
            FunPriIntrestCalculationDropdown(false, false, false, false, false, false, false, false);
            FunPriInterestCalculationMandotary(false);
            txtRecoveryPatternYear1.Text =
            txtRecoveryPatternYear2.Text =
            txtRecoveryPatternYear3.Text =
            txtRecoveryPatternYearRest.Text = "";//cannot be 0
            txtRecoveryPatternYear2.Enabled = false;
            txtRecoveryPatternYear3.Enabled = false;
            txtRecoveryPatternYearRest.Enabled = false;
            FunPriRecoveryPatternMandotary(false);
            txtRecoveryPatternYear1.Enabled = false;
            FunPriIntrestLevyDropdown(false, false, false, false, false, false, false, false);
            FunPriInsuranceDropdown(false);
            FunPriResidualValueDropdown(false);
            FunPriMarginDropdown(false, true);
            ddlLineofBusiness.Focus();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    #region User Defined Methods

    #region Dropdown
    /// <summary>
    /// Load LOB
    /// </summary>
    private void FunPriLoadLOB_LIST()
    {
        try
        {

            ObjDictParams = new Dictionary<string, string>();
            if (PageMode == PageModes.Create)
            {
                ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                if (intROIRuleMasterID == 0)
                {
                    ObjDictParams.Add("@Is_Active", "1");
                }
                ObjDictParams.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
                ObjDictParams.Add("@Program_ID", "27");
                ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, ObjDictParams, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            }

            ObjDictParams = new Dictionary<string, string>();
            DataTable dtDefault = Utility.GetDefaultData("S3G_ORG_GetROIRulesLookUp", ObjDictParams);
            FunProLoadCombos(dtDefault);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunProLoadCombos(DataTable dtDefault)
    {
        DataTable dtDefaultNew;
        DataView dvSearchView;
        if (dtDefault != null)
        {
            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_RATE_TYPE'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();
            ddlRateType.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();

            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_RETURN_PATTERN'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();
            ddlRatePattern.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();

            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_TIME_VALUE'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();
            ddlTime.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();


            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_FREQUENCY'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();
            ddlFrequency.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();
            ddlFrequency.Items.FindByValue("1").Enabled = false;

            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_REPAYMENT_MODE'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();
            ddlRepaymentMode.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();

            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_IRR_REST'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();
            ddlIRRRest.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();


            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_FREQUENCY'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();
            ddlIntrestCalculation.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();
            ddlIntrestCalculation.Items.FindByValue("5").Enabled = false;
            ddlIntrestCalculation.Items.FindByValue("7").Enabled = false;
            ddlIntrestCalculation.Items.FindByValue("8").Enabled = false;

            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_FREQUENCY'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();
            ddlIntrestLevy.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();
            ddlIntrestLevy.Items.FindByValue("5").Enabled = false;
            ddlIntrestLevy.Items.FindByValue("7").Enabled = false;
            ddlIntrestLevy.Items.FindByValue("8").Enabled = false;

            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_INSURANCE'";
            dvSearchView.Sort = "Name Asc";
            dtDefaultNew = dvSearchView.ToTable();


            //added by saranya
            ddlInsurance.Enabled = true;
            //end
            ddlInsurance.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();

            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_RESIDUAL_VALUE'";
            dtDefaultNew = dvSearchView.ToTable();
            ddlResidualValue.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();
            ddlResidualValue.Items.RemoveAt(0);

            dvSearchView = new DataView(dtDefault); ;
            dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_MARGIN'";
            dtDefaultNew = dvSearchView.ToTable();
            ddlMargin.FillDataTable(dtDefaultNew, "Value", "Name");
            dvSearchView.Dispose();
            ddlMargin.Items.RemoveAt(0);
            //ddlRatePattern.DataSource = dvSearchView;
            //ddlRatePattern.DataBind();
        }
    }

    #endregion

    /// <summary>
    /// Get ROI Rule Master Details 
    /// </summary>
    private void FunPriGetROIRuleMasterDetails()
    {
        ObjROIRuleMasterClient = new RuleCardMgtServicesClient();
        try
        {
            byte[] byteLOBDetails = ObjROIRuleMasterClient.FunPubQueryROIRules(intROIRuleMasterID);
            ObjROIRuleMasterDataTable = (RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, SerMode, typeof(RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable));
            ObjROIRuleMasterRow = ObjROIRuleMasterDataTable.Rows[0] as RuleCardMgtServices.S3G_ORG_ROI_RulesRow;

            txtSerialNumber.Text = ObjROIRuleMasterRow.IsSerial_NumberNull() ? "" : ObjROIRuleMasterRow.Serial_Number.ToString();

            System.Web.UI.WebControls.ListItem lst;

            lst = new System.Web.UI.WebControls.ListItem(ObjROIRuleMasterRow.LOB.ToString(), ObjROIRuleMasterRow.LOB_ID.ToString());
            ddlLineofBusiness.Items.Add(lst);
            ddlLineofBusiness.SelectedValue = ObjROIRuleMasterRow.LOB_ID.ToString();
            txtModelDescription.Text = ObjROIRuleMasterRow.Model_Description.ToString();

            ddlRateType.SelectedValue = ObjROIRuleMasterRow.IsRate_TypeNull() ? "0" : ObjROIRuleMasterRow.Rate_Type.ToString();
            txtROIRuleNumber.Text = ObjROIRuleMasterRow.IsROI_Rule_NumberNull() ? "" : ObjROIRuleMasterRow.ROI_Rule_Number.ToString();

            ddlRatePattern.SelectedValue = ObjROIRuleMasterRow.Return_Pattern.ToString();
            ddlTime.SelectedValue = ObjROIRuleMasterRow.IsTime_ValueNull() ? "0" : ObjROIRuleMasterRow.Time_Value.ToString();

            ddlFrequency.SelectedValue = ObjROIRuleMasterRow.IsFrequencyNull() ? "0" : ObjROIRuleMasterRow.Frequency.ToString();
            ddlRepaymentMode.SelectedValue = ObjROIRuleMasterRow.Repayment_Mode.ToString();

            string[] lob = (ddlLineofBusiness.SelectedItem.Text.ToLower()).Split('-');
            if ((lob[0].ToString()).Trim() == "ol" || (lob[0].ToString()).Trim() == "fl")
            {
                if (ddlRatePattern.SelectedItem.Text.Contains("PLF") || ddlRatePattern.SelectedItem.Text.Contains("PMF") || ddlRatePattern.SelectedItem.Text.Contains("PTF"))
                {
                    txtRate.Text = ObjROIRuleMasterRow.IsRateNull() ? "" : Convert.ToDecimal(ObjROIRuleMasterRow.Rate.ToString()).ToString(Funsetsuffix(true));
                }
                else
                {
                    txtRate.Text = ObjROIRuleMasterRow.IsRateNull() ? "" : Convert.ToDecimal(ObjROIRuleMasterRow.Rate.ToString()).ToString(Funsetsuffix(false));
                }
            }
            else
            {
                txtRate.Text = ObjROIRuleMasterRow.IsRateNull() ? "" : Convert.ToDecimal(ObjROIRuleMasterRow.Rate.ToString()).ToString(Funsetsuffix(false));
            }
            ddlIRRRest.SelectedValue = ObjROIRuleMasterRow.IsIRR_RestNull() ? "0" : ObjROIRuleMasterRow.IRR_Rest.ToString();

            ddlIntrestCalculation.SelectedValue = ObjROIRuleMasterRow.IsInterest_CalculationNull() ? "0" : ObjROIRuleMasterRow.Interest_Calculation.ToString();
            ddlIntrestLevy.SelectedValue = ObjROIRuleMasterRow.IsInterest_LevyNull() ? "0" : ObjROIRuleMasterRow.Interest_Levy.ToString();

            txtRecoveryPatternYear1.Text = ObjROIRuleMasterRow.IsRecovery_Pattern_Year1Null() ? "" : ObjROIRuleMasterRow.Recovery_Pattern_Year1.ToString();
            txtRecoveryPatternYear2.Text = ObjROIRuleMasterRow.IsRecovery_Pattern_Year2Null() ? "" : ObjROIRuleMasterRow.Recovery_Pattern_Year2.ToString();
            txtRecoveryPatternYear3.Text = ObjROIRuleMasterRow.IsRecovery_Pattern_Year3Null() ? "" : ObjROIRuleMasterRow.Recovery_Pattern_Year3.ToString();
            txtRecoveryPatternYearRest.Text = ObjROIRuleMasterRow.IsRecovery_Pattern_RestNull() ? "" : ObjROIRuleMasterRow.Recovery_Pattern_Rest.ToString();

            ddlInsurance.SelectedValue = ObjROIRuleMasterRow.IsInsuranceNull() ? "0" : ObjROIRuleMasterRow.Insurance.ToString();
            if (ObjROIRuleMasterRow.Residual_Value.ToString() == "True")
                ddlResidualValue.SelectedValue = "1";
            else
                ddlResidualValue.SelectedValue = "0";
            ddlMargin.SelectedValue = ObjROIRuleMasterRow.IsMarginNull() ? "0" : ObjROIRuleMasterRow.Margin.ToString();
            txtMarginPercentage.Text = ObjROIRuleMasterRow.IsMargin_PercentageNull() ? "" : ObjROIRuleMasterRow.Margin_Percentage.ToString();
            S3GSession ObjS3GSession = new S3GSession();
            if (string.IsNullOrEmpty(txtMarginPercentage.Text) == false)
            {
                txtMarginPercentage.Text = (txtMarginPercentage.Text.Split('.').Length == 2) ? txtMarginPercentage.Text : txtMarginPercentage.Text + "." + Math.Pow(10, 2).ToString().Trim('1');
            }

            if (string.IsNullOrEmpty(txtRecoveryPatternYear1.Text) == false)
            {
                txtRecoveryPatternYear1.Text = (txtRecoveryPatternYear1.Text.Split('.').Length == 2) ? txtRecoveryPatternYear1.Text : txtRecoveryPatternYear1.Text + "." + Math.Pow(10, 2).ToString().Trim('1');
            }

            if (string.IsNullOrEmpty(txtRecoveryPatternYear2.Text) == false)
            {
                txtRecoveryPatternYear2.Text = (txtRecoveryPatternYear2.Text.Split('.').Length == 2) ? txtRecoveryPatternYear2.Text : txtRecoveryPatternYear2.Text + "." + Math.Pow(10, 2).ToString().Trim('1');
            }
            if (string.IsNullOrEmpty(txtRecoveryPatternYear3.Text) == false)
            {
                txtRecoveryPatternYear3.Text = (txtRecoveryPatternYear3.Text.Split('.').Length == 2) ? txtRecoveryPatternYear3.Text : txtRecoveryPatternYear3.Text + "." + Math.Pow(10, 2).ToString().Trim('1');
            }

            if (string.IsNullOrEmpty(txtRecoveryPatternYearRest.Text) == false)
            {
                txtRecoveryPatternYearRest.Text = (txtRecoveryPatternYearRest.Text.Split('.').Length == 2) ? txtRecoveryPatternYearRest.Text : txtRecoveryPatternYearRest.Text + "." + Math.Pow(10, 2).ToString().Trim('1');
            }

            if (string.IsNullOrEmpty(txtRate.Text) == false)
            {
                if (ddlLineofBusiness.SelectedIndex > 0)
                {
                    //string[] lob = (ddlLineofBusiness.SelectedItem.Text.ToLower()).Split('-');
                    //if ((lob[0].ToString()).Trim() == "ol" || (lob[0].ToString()).Trim() == "fl")
                    //{
                    //    if (ddlRatePattern.SelectedValue == "3" || ddlRatePattern.SelectedValue == "4" || ddlRatePattern.SelectedValue == "5")
                    //    {
                    //        //txtRate.SetPercentagePrefixSuffix(5, 4, false, false, "Rate");
                    //        //txtRate.Text = txtRate.ToString(
                    //        txtRecoveryPatternYear1.SetPercentagePrefixSuffix(5, 4, true, false, "Recovery Pattern Year1");
                    //        txtRecoveryPatternYear2.SetPercentagePrefixSuffix(5, 4, true, false, "Recovery Pattern Year2");
                    //        txtRecoveryPatternYear3.SetPercentagePrefixSuffix(5, 4, true, false, "Recovery Pattern Year3");
                    //        txtRecoveryPatternYearRest.SetPercentagePrefixSuffix(5, 4, true, false,"Recovery Pattern Rest Year");
                    //    }
                    //    else
                    //    {
                    //        txtRate.SetPercentagePrefixSuffix(2, 4, false, false, "Rate");
                    //    }
                    //}
                    //else
                    //{
                    //    txtRate.SetPercentagePrefixSuffix(2, 4, false, false, "Rate");
                    //}
                }
                else
                {
                    txtRate.SetPercentagePrefixSuffix(2, 4, false, false, "Rate");
                }

            }

            //Utility.SetDecimalPrefixSuffix(2, 0, true, false, "Controls label");
            chkActive.Checked = ObjROIRuleMasterRow.Is_Active;
        }
        catch (FaultException<RuleCardMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjROIRuleMasterClient.Close();
        }
    }

    private string Funsetsuffix(bool EnableGPS)
    {

        int suffix = 1;
        string strformat = "0.";
        if (EnableGPS == true)
        {
            S3GSession ObjS3GSession = new S3GSession();
            suffix = ObjS3GSession.ProGpsSuffixRW;

            for (int i = 1; i <= suffix; i++)
            {
                strformat += "0";
            }
        }
        else
        {
            suffix = 2;
            for (int i = 1; i <= suffix; i++)
            {
                strformat += "0";
            }
        }
        return strformat;
    }


    #endregion

    # region "Recovery calculation"


    protected void txtRecoveryPatternYear1_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtRecoveryPatternYear1.Text == "" || Convert.ToDecimal(txtRecoveryPatternYear1.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "Recovery pattern 1 cannot be empty or 0");
                return;
            }
            else
            {
                funcalculate(1);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void txtRecoveryPatternYear2_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtRecoveryPatternYear2.Text == "" || Convert.ToDecimal(txtRecoveryPatternYear2.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "Recovery pattern 2 cannot be empty or 0");
                return;
            }
            else
            {
                funcalculate(2);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }


    protected void txtRecoveryPatternYear3_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtRecoveryPatternYear3.Text == "" || Convert.ToDecimal(txtRecoveryPatternYear3.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "Recovery pattern 3 cannot be empty or 0");
                return;
            }
            else
            {
                funcalculate(3);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }



    private void funcalculate(int ID)
    {
        try
        {
            decimal Total, RemainTotal = 0;
            decimal RP1 = Convert.ToDecimal(txtRecoveryPatternYear1.Text);
            decimal RP2 = Convert.ToDecimal(txtRecoveryPatternYear2.Text);
            decimal RP3 = Convert.ToDecimal(txtRecoveryPatternYear3.Text);
            decimal RPRest = Convert.ToDecimal(txtRecoveryPatternYearRest.Text);
            Total = RP1 + RP2 + RP3 + RPRest;
            RemainTotal = 100 - Total;
            if (Total > 100)
            {
                Utility.FunShowAlertMsg(this, "Total Recovery pattern should not exceed 100%");
                if (ID == 1)
                    txtRecoveryPatternYear1.Focus();
                else if (ID == 2)
                    txtRecoveryPatternYear2.Focus();
                else
                    txtRecoveryPatternYear3.Focus();
                return;
            }
            switch (ID)
            {
                case 1:
                    if (RP2 == 0)
                    {
                        txtRecoveryPatternYear2.Enabled = true;
                        txtRecoveryPatternYear2.Text = RemainTotal.ToString();
                    }
                    else
                    {
                        txtRecoveryPatternYear2.Text = (RP2 + RemainTotal).ToString();
                    }
                    break;
                case 2:
                    if (RP3 == 0)
                    {
                        txtRecoveryPatternYear3.Enabled = true;
                        txtRecoveryPatternYear3.Text = RemainTotal.ToString();
                    }
                    else
                    {
                        txtRecoveryPatternYear3.Text = (RP3 + RemainTotal).ToString();
                    }
                    break;
                case 3:
                    if (RPRest == 0)
                    {
                        txtRecoveryPatternYearRest.Text = RemainTotal.ToString();
                    }
                    else
                    {
                        txtRecoveryPatternYearRest.Text = (RPRest + RemainTotal).ToString();
                    }
                    break;
                default:
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




}
