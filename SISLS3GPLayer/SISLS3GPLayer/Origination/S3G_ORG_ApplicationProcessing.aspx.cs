/// Module Name     :   Origination
/// Screen Name     :   Application Processing
/// Created By      :   Narayanan
/// Created Date    :   18-06-2010
/// Purpose         :   To Insert , Update and Query
/// Modified By     :   Nataraj Y
/// Modified Date   :   02-09-2010
/// Purpose         :   Actually To Insert , Update and Query 
/// Modified By     :   Prabhu K
/// Modified Date   :   02-10-2010
/// Purpose         :   To Develop the Missed Functionalities
/// Modified By     :   Prabhu K
/// Modified Date   :   28-10-2011
/// Purpose         :   To Calculate and display the UMFC automatically whereever is applicable
/// Modified By     :   Narasimha Rao P
/// Modified Date   :   25-01-2012
/// Purpose         :   To fix the From Date Overlapping while selecting CashFlow in the Repayment Details.
/// /// Modified By :   Saranya I
/// Modified Date   :   08-02-2012
/// Purpose         :   To add Customer Master Changes and Enabled Asset Tab for Term Loan and  TL Extensible
/// Modified By     :   Thangam M
/// Date            :   17/Sep/2013
/// Purpose         :   Performance related changes
/// Modified By     :   Chandru K
/// Date            :   18/Sep/2013
/// Purpose         :   ISFC Customization


#region NameSpaces
using System;
using System.Data;
using S3GBusEntity;
using System.Web.UI;
using System.ServiceModel;
using System.Globalization;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using S3GBusEntity.Origination;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Data.Common;
using System.Diagnostics;
#endregion

public partial class Origination_S3G_ORG_ApplicationProcessing : ApplyThemeForProject
{

    #region Variable declaration
    static string strMode;
    double dcmTotalamount = 0;
    string strXMLCommon = string.Empty;
    string strAddMode = "1", strEditMode = "2", strDocumentDate = "";
    int intCompanyId = 0, intUserId = 0, intSlNo = 0, intProgramID = 38, intApplicationProcessId = 0, intResult;
    public string strDateFormat;
    string strRRBDate = "";
    int intEnqNewCustomerId = 0;
    bool blnIsPaintBG = true;
    Dictionary<string, string> objProcedureParameter;
    DataTable DtAlertDetails = new DataTable();
    DataTable DtFollowUp = new DataTable();
    DataTable DtCashFlow = new DataTable();
    DataTable DtCashFlowOut = new DataTable();
    DataTable DtRepayGrid = new DataTable();
    DataTable DtRepaySummary = new DataTable();
    DataTable dtInvoiceDetails = new DataTable();
    DataTable dtloanassetdetails=new DataTable();
    static string strPageName = "Application Processing";
    public string strCustomerId = string.Empty;
    public string strCustomerValue = string.Empty;
    public string strCustomerName = string.Empty;
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjAProcessSave;
    //ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjAProcessSave = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;
    Dictionary<string, string> Procparam;
    string strRedirectPage = "~/Origination/S3GORGTransLander.aspx?Code=APPP&Create=1&Modify=1";

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    DataTable dtasset;
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_ApplicationProcessing.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=APPP&Create=1&Modify=1';";
    //User Authorization
    string strNewWin1 = "window.showModalDialog('../Origination/S3G_ORG_Application_Asset_Homeloan.aspx";
   string strNewWin = "window.showModalDialog('../Origination/S3GOrgApplicationAssetDetails.aspx";
    string NewWinAttributes = "', 'Asset', 'toolbar:no;menubar:no;statusbar:no;dialogwidth:750px;dialogHeight:450px;');";
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    public static Origination_S3G_ORG_ApplicationProcessing obj_Page;
    //Code end
    #endregion

    #region  Methods

    #region Local Methods

    #region Asset

    private void FunPriRemoveAsset(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            LinkButton lnkSelect = (LinkButton)((GridView)sender).Rows[e.RowIndex].FindControl("lnkAssetSerialNo");
            DataTable dtAssetDetails = (DataTable)Session["PricingAssetDetails"];
            DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + lnkSelect.Text);
            drAsset[0].Delete();
            dtAssetDetails.AcceptChanges();
            DataRow[] drSerialAsset = dtAssetDetails.Select("SlNo > " + lnkSelect.Text);
            foreach (DataRow dr in drSerialAsset)
            {
                dr["SlNo"] = Convert.ToInt32(dr["SlNo"]) - 1;
                dr.AcceptChanges();
            }
            Session["PricingAssetDetails"] = dtAssetDetails;
            gvAssetDetails.DataSource = (DataTable)Session["PricingAssetDetails"];
            gvAssetDetails.DataBind();
            if (dtAssetDetails.Rows.Count > 0)
            {
                decimal dcmMarginAmount = (decimal)(dtAssetDetails.Compute("Sum(Margin_Amount)", "Noof_Units > 0"));
                txtMarginAmount.Text = (dcmMarginAmount == 0) ? "" : dcmMarginAmount.ToString();
            }
            else
            {
                txtMarginAmount.Text = "";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    //private void FunPriRemoveDeleteloanAsset(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        LinkButton lnkSelect = (LinkButton)((GridView)sender).Rows[e.RowIndex].FindControl("lnkAssetSerialNo");
    //        DataTable dtAssetDetails = (DataTable)Session["PricingloanAssetDetails"];
    //        DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + lnkSelect.Text);
    //        drAsset[0].Delete();
    //        dtAssetDetails.AcceptChanges();
    //        DataRow[] drSerialAsset = dtAssetDetails.Select("SlNo > " + lnkSelect.Text);
    //        foreach (DataRow dr in drSerialAsset)
    //        {
    //            dr["SlNo"] = Convert.ToInt32(dr["SlNo"]) - 1;
    //            dr.AcceptChanges();
    //        }
    //        Session["PricingloanAssetDetails"] = dtAssetDetails;
    //        grvloanasset.DataSource = (DataTable)Session["PricingloanAssetDetails"];
    //        grvloanasset.DataBind();
           
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex);
    //        throw ex;
    //    }
    //}


    private void FunPriBindAssetDetails(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton LnkSelect = (LinkButton)e.Row.FindControl("lnkAssetSerialNo");
                string strNewPurchase = "";
                if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
                {
                    strNewPurchase = "Yes";
                }
                else
                {
                    strNewPurchase = "No";
                }
                LnkSelect.Attributes.Remove("onclick");
                if (intApplicationProcessId > 0)
                {
                    if (ddlBusinessOfferNoList.SelectedValue == "-1")
                        //Condition added to validate pagemode - Bug_ID - 6387 - Kuppusamy.B - May-30-2012
                        if (strMode == "Q")
                        {
                            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                            {
                                
                                LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=" + strMode + NewWinAttributes);
                            }
                            else
                            {
                                LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=" + strMode + NewWinAttributes);
                            }
                        }
                        else
                        {
                            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                            {

                                LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
                            }
                            else
                            {
                                LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
                            }
                        }
                    else
                    {
                        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                        {

                            LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=" + strMode + NewWinAttributes);
                        }
                        else
                        {
                            LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=" + strMode + NewWinAttributes);
                        }
                    }
                }
                else
                {
                    if (ddlBusinessOfferNoList.SelectedIndex == 0)
                    {
                        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                        {
                            LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
                        }
                        else
                        {
                            LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
                        }
                    }
                    else
                    {
                        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                        {

                            LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&FromPricing=Yes" + NewWinAttributes);
                        }
                        else
                        {
                            LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&FromPricing=Yes" + NewWinAttributes);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(e.Row.Cells[5].Text) && e.Row.Cells[5].Text != "&nbsp;")
                    e.Row.Cells[5].Text = DateTime.Parse(e.Row.Cells[5].Text, CultureInfo.CurrentCulture).ToString(strDateFormat);
                LinkButton lnkView = (LinkButton)e.Row.FindControl("lnkView");
                Label lblProformaId = e.Row.FindControl("lblProformaId") as Label;
                if (lnkView != null && lblProformaId != null)
                {
                    if (!string.IsNullOrEmpty(lblProformaId.Text))
                    {
                        lnkView.Enabled = true;
                        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblProformaId.Text, false, 0);
                        lnkView.Attributes.Add("onclick", "window.open('../Origination/S3GOrgProforma_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsFromAccount=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;");
                    }
                    else
                    {
                        lnkView.Enabled = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriBindLoanAssetDetails(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton LnkSelect = (LinkButton)e.Row.FindControl("lnkAssetSerialNo");
                string strNewPurchase = "";
                if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
                {
                    strNewPurchase = "Yes";
                }
                else
                {
                    strNewPurchase = "No";
                }
                LnkSelect.Attributes.Remove("onclick");
                if (intApplicationProcessId > 0)
                {
                    if (ddlBusinessOfferNoList.SelectedValue == "-1")
                        //Condition added to validate pagemode - Bug_ID - 6387 - Kuppusamy.B - May-30-2012
                        if (strMode == "Q")
                        {
                            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                            {

                                LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=" + strMode + NewWinAttributes);
                            }
                            else
                            {
                                LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=" + strMode + NewWinAttributes);
                            }
                        }
                        else
                        {
                            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                            {

                                LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
                            }
                            else
                            {
                                LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
                            }
                        }
                    else
                    {
                        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                        {

                            LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=" + strMode + NewWinAttributes);
                        }
                        else
                        {
                            LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&qsMode=" + strMode + NewWinAttributes);
                        }
                    }
                }
                else
                {
                    if (ddlBusinessOfferNoList.SelectedIndex == 0)
                    {
                        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                        {
                            LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
                        }
                        else
                        {
                            LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + NewWinAttributes);
                        }
                    }
                    else
                    {
                        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
                        {

                            LnkSelect.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&FromPricing=Yes" + NewWinAttributes);
                        }
                        else
                        {
                            LnkSelect.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + "&qsRowID=" + LnkSelect.Text + "&FromPricing=Yes" + NewWinAttributes);
                        }
                    }
                }


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }


    #endregion

    #region ROI - Payment Rule

    private void FunPriFetchPaymentDetails()
    {
        try
        {
            div8.Visible = true;
            hdnPayment.Value = ddlPaymentRuleList.SelectedValue;
            FunPriLoadPaymentRuleDetails();
            if (ddlPaymentRuleList.SelectedItem.Text != "--Select--")
                txtPaymentCardMLA.Text = ddlPaymentRuleList.SelectedItem.Text;
            if (hdnPayment.Value != "")
            {
                FunPriFillRepaymentDLL(strAddMode);
                FunPriFillOutflowDLL(strAddMode);
                lblTotalOutFlowAmount.Text = "0";
                FunPriIRRReset();
                ViewState["RepaymentStructure"] = null;
                grvRepayStructure.DataSource = null;
                grvRepayStructure.DataBind();
                gvRepaymentSummary.ClearGrid();
            }
        }
        catch (Exception ex)
        {
            div8.Visible = false;
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Payment Rule Details");
        }
    }
    private decimal FunPriGetInterestAmount()
    {
        decimal decFinAmount = FunPriGetAmountFinanced();
        decimal decUMFC = 0;
        if (!string.IsNullOrEmpty(lblTotalAmount.Text))
        {
            string strTotalAmount = (lblTotalAmount.Text.Split(':').Length > 1) ? lblTotalAmount.Text.Split(':')[1].Trim() : "";
            if (strTotalAmount != "")
            {
                decimal decTotalRepayable = Convert.ToDecimal(strTotalAmount);
                decUMFC = decTotalRepayable - decFinAmount;
            }
        }
        return decUMFC;



    }

    private decimal FunPriGetStructureAdhocInterestAmount()
    {
        decimal decFinAmount = FunPriGetAmountFinanced();
        decimal decRate = 0;
        decRate = Convert.ToDecimal(txtRate.Text);
        switch (ddl_Return_Pattern.SelectedValue)
        {
            case "1":
                decRate = Convert.ToDecimal(txtRate.Text);
                break;
            case "2":
                if (ViewState["decRate"] != null)
                {
                    decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
                }
                break;
            case "3": //RepaymentType.PMPT:
                return Math.Round(((decFinAmount / 1000) * decRate *
                    int.Parse(txtTenure.Text)) - decFinAmount, 0);
                break;
            case "4": //RepaymentType.PMPL:
                return Math.Round(((decFinAmount / 100000) * decRate *
                    int.Parse(txtTenure.Text)) - decFinAmount, 0);
                break;
            case "5": //RepaymentType.PMPM:
                return Math.Round(((decFinAmount / 1000000) * decRate *
                    int.Parse(txtTenure.Text)) - decFinAmount, 0);
                break;
            default:
                decRate = Convert.ToDecimal(txtRate.Text);
                break;
        }
        string strLOB = ddlLOB.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower();
        switch (strLOB)
        {
            case "tl":
            case "te":
                if (ddl_Repayment_Mode.SelectedValue == "5")
                {
                    decRate = 0;
                }
                break;
            case "ft":
            case "wc":
                decRate = 0;
                break;
        }

        return Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(ddlTenureType.SelectedItem.Text.ToLower(), decFinAmount, decRate, int.Parse(txtTenure.Text)), 0);
    }

    private void FunPriFetchROIDetails()
    {
        try
        {
            if (ddlROIRuleList.SelectedIndex > 0)
            {
                div7.Visible = true;
                hdnROIRule.Value = ddlROIRuleList.SelectedValue;
                FunPriFillROIDLL(strAddMode);
                FunPriLoadROIRuleDetails(strAddMode);

                FunPriSetRateLength();
                txtROIMLA.Text = ddlROIRuleList.SelectedItem.Text;
                if (hdnROIRule.Value != "" || div7.Visible == false)
                {

                    FunPriFillInflowDLL(strAddMode);
                    FunPriFillOutflowDLL(strAddMode);
                    FunPriFillRepaymentDLL(strAddMode);
                    FunPriFillAlertDLL(strAddMode);
                    FunPriFillFollowupDLL(strAddMode);
                    FunPriFillGuarantorDLL();
                    FunPriFillMoratoriumDLL();
                    FunPriIRRReset();
                    ViewState["RepaymentStructure"] = null;
                    grvRepayStructure.DataSource = null;
                    grvRepayStructure.DataBind();
                    ViewState["decRate"] = null;
                    gvRepaymentSummary.ClearGrid();
                }

                //added by saranya
                if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") ||
                    ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") ||
                    ((ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") ||
                    ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TL")) &&
                    ddl_Repayment_Mode.SelectedItem.Text.ToUpper().StartsWith("PRO")))
                {
                    Button btnAdd = gvInflow.FooterRow.FindControl("btnAdd") as Button;
                    btnAdd.Enabled = false;
                    // TabContainerAP.Tabs[2].Enabled = false;

                }
                //end
            }
            else
            {
                if (intApplicationProcessId == 0)
                {
                    txt_ROI_Rule_Number.Text = "";
                    FunPriFillROIDLL(strAddMode);
                    txt_Model_Description.Text = "";
                    txtRate.Text = "";
                    txtRate.Enabled = false;
                    txt_Recovery_Pattern_Year1.Text = "";
                    txt_Recovery_Pattern_Year2.Text = "";
                    txt_Recovery_Pattern_Year3.Text = "";
                    txt_Recovery_Pattern_Rest.Text = "";
                    //txtCollateralTypeRate.Text = "";
                    //txtIRRRate.Text = "";
                    hdnROIRule.Value = "";
                }
            }
        }
        catch (Exception ex)
        {
            div7.Visible = false;
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load ROI Rule Details");
        }
    }
    private void FunPriSetRateLength()
    {
        if (ddl_Return_Pattern.SelectedValue == "3" || ddl_Return_Pattern.SelectedValue == "4" || ddl_Return_Pattern.SelectedValue == "5")
        {
            txtRate.SetDecimalPrefixSuffix(5, 4, false, "Rate");
        }
        else
        {
            txtRate.SetPercentagePrefixSuffix(2, 4, false, true, "Rate");
        }

    }
    private void FunPriFillROIDLL(string Mode)
    {
        try
        {
            if (Mode == strAddMode)
            {
                Dictionary<string, string> objParameters = new Dictionary<string, string>();
                DataSet dsROILov = Utility.GetDataset("s3g_org_loadROILov", objParameters);
                ddl_Rate_Type.BindDataTable(dsROILov.Tables[0]);
                ddl_Return_Pattern.BindDataTable(dsROILov.Tables[1]);
                ddl_Time_Value.BindDataTable(dsROILov.Tables[2]);
                ddl_Frequency.BindDataTable(dsROILov.Tables[3]);
                ddl_Repayment_Mode.BindDataTable(dsROILov.Tables[4]);
                ddl_IRR_Rest.BindDataTable(dsROILov.Tables[5]);
                ddl_Interest_Calculation.BindDataTable(dsROILov.Tables[3]);
                ddl_Interest_Levy.BindDataTable(dsROILov.Tables[3]);
                ddl_Insurance.BindDataTable(dsROILov.Tables[6]);
                ViewState["MLAROIDetails"] = dsROILov;
            }
        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Fill DLL for ROI Rule");
        }
        //finally
        //{
        //    if (ObjCustomerService != null)
        //        ObjCustomerService.Close();
        //}

    }

    private void FunPriLoadROIRuleDetails(string Mode)
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            //ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            DataTable ObjDTROI = new DataTable();
            if (Mode == strAddMode)
            {
                ObjStatus.Option = 40;
                ObjStatus.Param1 = ddlROIRuleList.SelectedValue;
                //ObjStatus.Param2 = ddlProductCodeList.SelectedValue;
                //ObjStatus.Param3 = hdnConstitutionId.Value;
                ObjDTROI = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                ViewState["ROIRules"] = ObjDTROI;
            }
            if (Mode == strEditMode)
            {
                ObjDTROI = (DataTable)ViewState["ROIRules"];

            }

            string strRoirValue;
            if (hdnROIRule.Value != "")
            {
                strRoirValue = hdnROIRule.Value;
            }
            else
            {
                strRoirValue = "0";
            }

            switch (ObjDTROI.Rows[0]["Repayment_Mode"].ToString())
            {
                case "3":
                    Dictionary<int, decimal> dictRecovery = new Dictionary<int, decimal>();
                    if (Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year1"].ToString()) != 0)
                        dictRecovery.Add(1, Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year1"].ToString()));

                    if (Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year2"].ToString()) != 0)
                        dictRecovery.Add(2, Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year2"].ToString()));

                    if (Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year3"].ToString()) != 0)
                        dictRecovery.Add(3, Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year3"].ToString()));

                    if (Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Rest"].ToString()) != 0)
                        dictRecovery.Add(4, Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Rest"].ToString()));

                    int inMax = dictRecovery.Keys.Max();
                    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                    int intNoofYears = objRepaymentStructure.FunPubGetNoofYearsFromTenure(ddlTenureType.SelectedItem.Text, txtTenure.Text);
                    if (inMax != 4)
                    {
                        if (inMax != intNoofYears)
                        {
                            Utility.FunShowAlertMsg(this, "Tenure and Recovery Pattern are not matching");
                            div7.Visible = false;
                            ddlROIRuleList.SelectedValue = strRoirValue;
                            hdnROIRule.Value = "";
                            return;
                        }

                    }
                    else
                    {
                        if (intNoofYears < 4)
                        {
                            Utility.FunShowAlertMsg(this, "Tenure and Recovery Pattern are not matching");
                            div7.Visible = false;
                            ddlROIRuleList.SelectedValue = strRoirValue;
                            hdnROIRule.Value = "";
                            return;
                        }
                    }
                    break;

            }

            if (Convert.ToInt32(ObjDTROI.Rows[0]["Return_Pattern"].ToString()) > 2)
            {
                if (ddlTenureType.SelectedItem.Text.Trim().ToUpper() != "MONTHS")
                {
                    Utility.FunShowAlertMsg(this, "Tenure type should be months for PTF/PLF/PMF");
                    div7.Visible = false;
                    ddlROIRuleList.SelectedValue = strRoirValue;
                    hdnROIRule.Value = "";
                    return;
                }
            }


            //Dec rate 
            if (!string.IsNullOrEmpty(ObjDTROI.Rows[0]["IRR_Rate"].ToString()))
            {
                ViewState["decRate"] = ObjDTROI.Rows[0]["IRR_Rate"].ToString();
            }


            if (ObjDTROI.Rows.Count > 0)
            {
                txt_Model_Description.Text = ObjDTROI.Rows[0]["Model_Description"].ToString();
                txt_ROI_Rule_Number.Text = ObjDTROI.Rows[0]["ROI_Rule_Number"].ToString();
                txt_Recovery_Pattern_Year1.Text = ObjDTROI.Rows[0]["Recovery_Pattern_Year1"].ToString();
                txt_Recovery_Pattern_Year2.Text = ObjDTROI.Rows[0]["Recovery_Pattern_Year2"].ToString();
                txt_Recovery_Pattern_Year3.Text = ObjDTROI.Rows[0]["Recovery_Pattern_Year3"].ToString();
                txt_Recovery_Pattern_Rest.Text = ObjDTROI.Rows[0]["Recovery_Pattern_Rest"].ToString();
                
                FunPriShowROIControls(ObjDTROI.Rows[0]["Rate_Type"], tr_lblRate_Type, ddl_Rate_Type);
                FunPriShowROIControls(ObjDTROI.Rows[0]["Return_Pattern"], tr_lblReturn_Pattern, ddl_Return_Pattern);
                if (ObjDTROI.Rows[0]["Time_Value"].ToString() == "-1")
                {
                    rfvTimeValue.Enabled = false;
                }
                else
                {
                    rfvTimeValue.Enabled = true;
                }
                FunPriShowROIControls(ObjDTROI.Rows[0]["Time_Value"], tr_lblTime_Value, ddl_Time_Value);
                if (intApplicationProcessId == 0)
                {
                    txtFBDate.Text = "";
                }
                if (ObjDTROI.Rows[0]["Time_Value"].ToString() == "3" || ObjDTROI.Rows[0]["Time_Value"].ToString() == "4")
                {
                    rfvFBDate.Enabled = true;
                    txtFBDate.Enabled = true;
                    rngFBDate.Enabled = true;


                }
                else
                {
                    txtFBDate.Enabled = false;
                    rfvFBDate.Enabled = false;
                    rngFBDate.Enabled = false;
                }
                if (ObjDTROI.Rows[0]["Frequency"].ToString() == "-1")
                {
                    rfvFrequency.Enabled = false;
                }
                else
                {
                    rfvFrequency.Enabled = true;
                }
                FunPriShowROIControls(ObjDTROI.Rows[0]["Frequency"], tr_lblFrequency, ddl_Frequency);
                FunPriShowROIControls(ObjDTROI.Rows[0]["Repayment_Mode"], tr_lblRepayment_Mode, ddl_Repayment_Mode);
                FunPriShowROIControls(ObjDTROI.Rows[0]["Rate"], tr_lblRate, txtRate);
                FunPriShowROIControls(ObjDTROI.Rows[0]["IRR_Rest"], tr_lblIRR_Rest, ddl_IRR_Rest);
                FunPriShowROIControls(ObjDTROI.Rows[0]["Interest_Calculation"], tr_lblInterest_Calculation, ddl_Interest_Calculation);
                FunPriShowROIControls(ObjDTROI.Rows[0]["Interest_Levy"], tr_lblInterest_Levy, ddl_Interest_Levy);
                FunPriShowROIControls(ObjDTROI.Rows[0]["Insurance"], tr_lblInsurance, ddl_Insurance);

                //Chandru K On 19-Sep-2013 For ISFC
                //txtCollateralTypeRate.Text = ObjDTROI.Rows[0]["Collateral_Type_Rate"].ToString();
                //FunPriShowROIControls(ObjDTROI.Rows[0]["IRR_Rate"], tr_lblCollateralTypeRate, txtRate);

                //if (txtCollateralTypeRate.Text != "" && txtIRRRate.Text != "")
                //    txtRate.Text = Convert.ToString(Convert.ToDecimal(txtIRRRate.Text) + Convert.ToDecimal(txtCollateralTypeRate.Text));

                if (ObjDTROI.Rows[0]["Residual_Value"].ToString() == "0")
                {
                    tr_lblResidual_Value.Visible = false;
                }
                else
                {
                    FunPriShowROIControls(ObjDTROI.Rows[0]["Residual_Value"], tr_lblResidual_Value, chk_lblResidual_Value);
                }
                if (ObjDTROI.Rows[0]["Margin"].ToString() == "0")
                {
                    tr_lblMargin.Visible = false;
                }
                else
                {
                    FunPriShowROIControls(ObjDTROI.Rows[0]["Margin"], tr_lblMargin, chk_lblMargin);

                    if (ObjDTROI.Rows[0]["Margin_Percentage"].ToString() == "")
                    {
                        tr_lblMargin_Percentage.Visible = false;
                    }
                    else
                    {
                        FunPriShowROIControls(ObjDTROI.Rows[0]["Margin_Percentage"], tr_lblMargin_Percentage, txt_Margin_Percentage);
                    }
                }
                if (ObjDTROI.Rows[0]["Margin"].ToString() == "1")
                {
                    FunPriShowROIControls(ObjDTROI.Rows[0]["Margin_Percentage"], tr_lblMargin_Percentage, txt_Margin_Percentage);
                    txtMarginMoneyPer_Cashflow.Text = ObjDTROI.Rows[0]["Margin_Percentage"].ToString();
                    txtMarginMoneyAmount_Cashflow.Text = FunPriGetMarginAmout().ToString();
                    txtMarginMoneyPer_Cashflow.ReadOnly = true;
                    txtMarginMoneyAmount_Cashflow.ReadOnly = true;
                    rfvMarginPercent.Enabled = true;

                }
                else
                {
                    txt_Margin_Percentage.Text = "";
                    txtMarginMoneyPer_Cashflow.Text = "";
                    txtMarginMoneyAmount_Cashflow.Text = "";
                    txtMarginMoneyPer_Cashflow.ReadOnly = true;
                    txtMarginMoneyAmount_Cashflow.ReadOnly = true;
                    rfvMarginPercent.Enabled = false;
                }

                if (ObjDTROI.Rows[0]["Residual_Value"].ToString() == "1" && txtResidualValue.Text.Trim() == "")
                {
                    rfvResidualValue.Enabled = true;
                    txtResidualAmt_Cashflow.ReadOnly = false;
                    txtResidualValue_Cashflow.ReadOnly = false;
                }
                else if (ObjDTROI.Rows[0]["Residual_Value"].ToString() == "1" && txtResidualValue.Text.Trim() != "")
                {
                    rfvResidualValue.Enabled = false;
                    txtResidualAmt_Cashflow.ReadOnly = false;
                    txtResidualValue_Cashflow.ReadOnly = false;
                    txtResidualAmt_Cashflow.Text = txtResidualValue.Text;
                }
                else
                {
                    txtResidualAmt_Cashflow.Text = "";
                    txtResidualValue_Cashflow.Text = "";
                    txtResidualValue.Text = "";
                    rfvResidualValue.Enabled = false;
                    txtResidualAmt_Cashflow.ReadOnly = true;
                    txtResidualValue_Cashflow.ReadOnly = true;
                }
                FunPriLoadROIMLA();
                //hide the IRR panel visibility for WC,FT,TLE(Product) method as per Malolan raised on 23-feb-2012 start by saran

                FunPriDisableIRRPanel();
                //hide the IRR panel visibility for WC,FT,TLE(Product) method as per Malolan raised on 23-feb-2012 end by saran

            }

        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the ROI Rule");
        }
        finally
        {
            //if (ObjCustomerService != null)
            ObjCustomerService.Close();
        }
    }
    //hide the IRR panel visibility for WC,FT,TLE(Product) method as per Malolan raised on 23-feb-2012 start by saran

    private void FunPriDisableIRRPanel()
    {
        if (ddl_Repayment_Mode.SelectedIndex > 0)
        {
            if (Convert.ToInt32(ddl_Repayment_Mode.SelectedValue) > 0)
                if (Convert.ToInt32(ddl_Repayment_Mode.SelectedValue) >= 4)
                    pnlIRRDetails.Visible = false;
        }
    }
    //hide the IRR panel visibility for WC,FT,TLE(Product) method as per Malolan raised on 23-feb-2012 end by saran

    private void FunPriToggleResidualAmountBased()
    {
        try
        {
            if (txtResidualAmt_Cashflow.Text != "")
            {
                rfvResidualValue.Enabled = false;
                txtResidualAmt_Cashflow.ReadOnly = false;
                txtResidualValue_Cashflow.ReadOnly = true;

            }
            else
            {
                rfvResidualValue.Enabled = true;
                txtResidualValue_Cashflow.ReadOnly = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Toggle Residual Amount Related Details");
        }
    }

    private void FunPriToggleResidualPercentageBased()
    {
        try
        {
            if (txtResidualValue_Cashflow.Text != "")
            {
                rfvResidualValue.Enabled = false;
                txtResidualAmt_Cashflow.ReadOnly = true;
                txtResidualValue.Text = "";
            }
            else
            {
                rfvResidualValue.Enabled = true;
                txtResidualAmt_Cashflow.ReadOnly = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Toggle Residual % Related Details");
        }
    }

    private void FunPriLoadPaymentRuleDetails()
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            DataTable ObjDTPayment = new DataTable();

            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@Is_Active", "1");
            objProcedureParameter.Add("@Rules_ID", ddlPaymentRuleList.SelectedItem.Value);
            objProcedureParameter.Add("@Option", "10");
            ObjDTPayment = Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, objProcedureParameter);
            string vendor = ObjDTPayment.Rows[0]["Entity_Type"].ToString().ToLower();

            //ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();
            DataTable dtCashFlowFrom = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            switch (vendor)
            {
                case "vendor":
                    dtCashFlowFrom.Rows.RemoveAt(0);
                    break;
                case "customer":
                    dtCashFlowFrom.Rows.RemoveAt(1);
                    break;
            }
            ViewState["vendor"] = vendor;
            if (intApplicationProcessId == 0)
            {
                DropDownList ddlEntityName_InFlowFrom = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
                ((DataSet)ViewState["OutflowDDL"]).Merge(dtCashFlowFrom);
                Utility.FillDLL(ddlEntityName_InFlowFrom, dtCashFlowFrom, true);
            }
            FunPriBindPaymentRule(ObjDTPayment);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the Payment Rule");
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }
    private void FunPriBindPaymentRule(DataTable ObjDTPayment)
    {
        DataTable ObjDTPaymentGen = new DataTable();
        DataColumn dc1 = new DataColumn("FieldName");
        DataColumn dc2 = new DataColumn("FieldValue");
        ObjDTPaymentGen.Columns.Add(dc1);
        ObjDTPaymentGen.Columns.Add(dc2);
        try
        {
            ViewState["PaymentRules"] = ObjDTPaymentGen;
            for (int i = 0; i < ObjDTPayment.Columns.Count; i++)
            {
                if (ObjDTPayment.Rows[0][i].ToString() != string.Empty)
                {
                    DataRow dr = ObjDTPaymentGen.NewRow();
                    dr[0] = ObjDTPayment.Columns[i].ColumnName.Replace("_", " ");
                    if (ObjDTPayment.Rows.Count > 0) dr[1] = ObjDTPayment.Rows[0][i].ToString();
                    else dr[1] = string.Empty;
                    ObjDTPaymentGen.Rows.Add(dr);
                }
            }
            gvPaymentRuleDetails.DataSource = ObjDTPaymentGen;
            gvPaymentRuleDetails.DataBind();
            if (TabContainerAP.Tabs[6].Visible)
            {
                gv_MLARepayRuleCard.DataSource = ObjDTPaymentGen;
                gv_MLARepayRuleCard.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Payment Rule Details");
        }
    }
    private void FunPriLoadROIMLA()
    {
        try
        {
            DataTable ObjDTROI = (DataTable)ViewState["ROIRules"];


            DataTable ObjDTPaymentGen = new DataTable();
            DataColumn dc1 = new DataColumn("FieldName");
            DataColumn dc2 = new DataColumn("FieldValue");
            ObjDTPaymentGen.Columns.Add(dc1);
            ObjDTPaymentGen.Columns.Add(dc2);
            DataSet dsROILov = ViewState["MLAROIDetails"] as DataSet;
            for (int i = 0; i < ObjDTROI.Columns.Count; i++)
            {
                if (ObjDTROI.Rows[0][i].ToString() != string.Empty)
                {
                    if (!ObjDTROI.Columns[i].ColumnName.Contains("Serial") && !ObjDTROI.Columns[i].ColumnName.Contains("ID"))
                    {
                        DataRow dr = ObjDTPaymentGen.NewRow();
                        dr[0] = ObjDTROI.Columns[i].ColumnName.Replace("_", " ");
                        if (dsROILov.Tables.Count > 0)
                        {
                            if (ObjDTROI.Rows.Count > 0)
                            {
                                if (dr[0].ToString() == "Rate Type")
                                {
                                    DataRow[] drRate = dsROILov.Tables[0].Select("Value = " + ObjDTROI.Rows[0][i].ToString());
                                    if (drRate.Length > 0)
                                        dr[1] = drRate[0]["Name"];
                                }
                                else if (dr[0].ToString() == "Return Pattern")
                                {
                                    DataRow[] drRate = dsROILov.Tables[1].Select("Value = " + ObjDTROI.Rows[0][i].ToString());
                                    if (drRate.Length > 0)
                                        dr[1] = drRate[0]["Name"];
                                }
                                else if (dr[0].ToString() == "Time Value")
                                {
                                    DataRow[] drRate = dsROILov.Tables[2].Select("Value = " + ObjDTROI.Rows[0][i].ToString());
                                    if (drRate.Length > 0)
                                        dr[1] = drRate[0]["Name"];
                                }
                                else if (dr[0].ToString() == "Frequency" || dr[0].ToString() == "Interest Calculation" || dr[0].ToString() == "Interest Levy")
                                {
                                    DataRow[] drRate = dsROILov.Tables[3].Select("Value = " + ObjDTROI.Rows[0][i].ToString());
                                    if (drRate.Length > 0)
                                        dr[1] = drRate[0]["Name"];
                                }
                                else if (dr[0].ToString() == "Repayment Mode")
                                {
                                    DataRow[] drRate = dsROILov.Tables[4].Select("Value = " + ObjDTROI.Rows[0][i].ToString());
                                    if (drRate.Length > 0)
                                        dr[1] = drRate[0]["Name"];
                                }
                                else if (dr[0].ToString() == "IRR Rest")
                                {
                                    DataRow[] drRate = dsROILov.Tables[5].Select("Value = " + ObjDTROI.Rows[0][i].ToString());
                                    if (drRate.Length > 0)
                                        dr[1] = drRate[0]["Name"];
                                }
                                else if (dr[0].ToString() == "Insurance")
                                {
                                    DataRow[] drRate = dsROILov.Tables[6].Select("Value = " + ObjDTROI.Rows[0][i].ToString());
                                    if (drRate.Length > 0)
                                        dr[1] = drRate[0]["Name"];
                                }
                                else if (dr[0].ToString() == "Residual Value")
                                {
                                    if (ObjDTROI.Rows[0][i].ToString() == "0")
                                        dr[1] = "Not Applicable";
                                    else
                                        dr[1] = "Applicable";
                                }
                                else
                                {
                                    dr[1] = ObjDTROI.Rows[0][i].ToString();
                                }
                            }
                            else
                            {
                                dr[1] = string.Empty;
                            }

                        }
                        ObjDTPaymentGen.Rows.Add(dr);
                    }
                }
            }
            if (TabContainerAP.Tabs[6].Visible)
            {
                gv_MLAROI.DataSource = ObjDTPaymentGen;
                gv_MLAROI.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the ROI Rule in Prime/Sub Account Details");
        }

    }

    private void FunPriUpdateROIRule()
    {
        DataTable ObjDTROI;
        try
        {
            ObjDTROI = (DataTable)ViewState["ROIRules"];
            ObjDTROI.Rows[0]["Model_Description"] = txt_Model_Description.Text;
            ObjDTROI.Rows[0]["Rate_Type"] = ddl_Rate_Type.SelectedValue;
            ObjDTROI.Rows[0]["ROI_Rule_Number"] = txt_ROI_Rule_Number.Text;
            ObjDTROI.Rows[0]["Return_Pattern"] = ddl_Return_Pattern.SelectedValue;
            ObjDTROI.Rows[0]["Time_Value"] = ddl_Time_Value.SelectedValue;
            ObjDTROI.Rows[0]["Frequency"] = ddl_Frequency.SelectedValue;
            ObjDTROI.Rows[0]["Repayment_Mode"] = ddl_Repayment_Mode.SelectedValue;
            ObjDTROI.Rows[0]["Rate"] = txtRate.Text;
            //ObjDTROI.Rows[0]["Collateral_Type_Rate"] = txtCollateralTypeRate.Text;
            ObjDTROI.Rows[0]["IRR_Rest"] = ddl_IRR_Rest.SelectedValue;
            ObjDTROI.Rows[0]["Interest_Calculation"] = ddl_Interest_Calculation.SelectedValue;
            ObjDTROI.Rows[0]["Interest_Levy"] = ddl_Interest_Levy.SelectedValue;
            ObjDTROI.Rows[0]["Recovery_Pattern_Year1"] = txt_Recovery_Pattern_Year1.Text;
            ObjDTROI.Rows[0]["Recovery_Pattern_Year2"] = txt_Recovery_Pattern_Year2.Text;
            ObjDTROI.Rows[0]["Recovery_Pattern_Year3"] = txt_Recovery_Pattern_Year3.Text;
            ObjDTROI.Rows[0]["Recovery_Pattern_Rest"] = txt_Recovery_Pattern_Rest.Text;
            ObjDTROI.Rows[0]["Insurance"] = ddl_Insurance.SelectedValue;
            ObjDTROI.Rows[0]["Residual_Value"] = chk_lblResidual_Value.Checked;
            ObjDTROI.Rows[0]["Margin"] = chk_lblMargin.Checked;
            ObjDTROI.Rows[0]["Margin_Percentage"] = txt_Margin_Percentage.Text == "" ? 0 : Convert.ToDecimal(txt_Margin_Percentage.Text);
            ObjDTROI.Rows[0].AcceptChanges();
            ViewState["ROIRules"] = ObjDTROI;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriShowROIControls(Object Data, System.Web.UI.HtmlControls.HtmlTableRow rRow, Object ObjCtl)
    {
        try
        {

            if (!string.IsNullOrEmpty(Convert.ToString(Data)))
            {
                rRow.Visible = true;

                if (ObjCtl.GetType().Name == "TextBox")
                {
                    ((TextBox)ObjCtl).Text = Convert.ToString(Data);
                }
                if (ObjCtl.GetType().Name == "DropDownList")
                {
                    DropDownList DDL = new DropDownList();
                    DDL = ((DropDownList)ObjCtl);
                    if (DDL.Items.Count > 0)
                        DDL.SelectedValue = Convert.ToString(Data);
                    if (Convert.ToString(Data) == "0" || Convert.ToString(Data) == "-1")
                    {
                        rRow.Visible = false;
                    }
                }
                if (ObjCtl.GetType().Name == "CheckBox")
                {
                    ((CheckBox)ObjCtl).Checked = Convert.ToBoolean(Data);
                }

                if (ddlROIRuleList.SelectedItem.Text.ToUpper().Contains("RRA"))//ROI Rule number selected in drop down
                {
                    ((WebControl)ObjCtl).Enabled = false;

                }
                else
                {
                    if (ObjCtl != null)
                    {
                        if (!((WebControl)ObjCtl).ID.Contains("ddl_Time_Value") && !((WebControl)ObjCtl).ID.Contains("ddl_Frequency") && !((WebControl)ObjCtl).ID.Contains("txtRate") && !((WebControl)ObjCtl).ID.Contains("ddl_Insurance") && !((WebControl)ObjCtl).ID.Contains("txt_Margin_Percentage"))
                        {
                            ((WebControl)ObjCtl).Enabled = false;

                        }
                        else
                        {
                            if (txtStatus.Text.ToUpper() == "PENDING")
                            {
                                ((WebControl)ObjCtl).Enabled = true;

                            }
                            else
                            {
                                ((WebControl)ObjCtl).Enabled = false;

                            }
                        }
                    }
                }

            }
            else
                rRow.Visible = false;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    #endregion

    #region Cash In Flows

    private void FunPriFillInflowDLL(string Mode)
    {

        try
        {

            //ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyId.ToString());
            if (ddlLOB.SelectedValue != "0")
            {
                objParameters.Add("@LobId", ddlLOB.SelectedValue);
            }
            DataSet dsInflow = Utility.GetDataset("s3g_org_loadInflowLov", objParameters);
            ViewState["InflowDDL"] = dsInflow;
            if (Mode == strAddMode)
            {
                if (txtFinanceAmount.Text != "")
                {
                    objParameters.Add("@Finance_Amount", txtFinanceAmount.Text);
                    objParameters.Add("@Product_Id", ddlProductCodeList.SelectedValue);
                    objParameters.Add("@EntityID", hdnCustID.Value);
                    objParameters.Add("@Entity", S3GCustomerAddress1.CustomerName);
                    DataTable dtAutoCashFlow = new DataTable();
                    DataTable dtAutoCashFlowtemp = new DataTable();
                    dtAutoCashFlow = Utility.GetDefaultData("S3G_ORG_LoadInflow", objParameters);
                    ViewState["dtAutoCashFlow"] = dtAutoCashFlow;

                    if (dtAutoCashFlow != null)
                    {
                        if (dtAutoCashFlow.Columns["CashFlow_Flag_ID"].DataType.Name == "Decimal")
                        {
                            dtAutoCashFlowtemp = dtAutoCashFlow.Clone();
                            dtAutoCashFlowtemp.Columns["CashFlow_Flag_ID"].DataType = typeof(int);

                            DataRow drAutoCashFlowtemp = dtAutoCashFlowtemp.NewRow();
                            foreach (DataRow drdtAutoCashFlow in dtAutoCashFlow.Rows)
                            {
                                foreach (DataColumn dc in dtAutoCashFlow.Columns)
                                {
                                    if (dc.ColumnName == "Amount")
                                        drAutoCashFlowtemp[dc.ColumnName] = Convert.ToDecimal(drdtAutoCashFlow[dc.ColumnName].ToString());
                                    else if (dc.ColumnName == "CashFlow_Flag_ID")
                                        drAutoCashFlowtemp[dc.ColumnName] = Convert.ToInt16(drdtAutoCashFlow[dc.ColumnName].ToString());
                                    else
                                        drAutoCashFlowtemp[dc.ColumnName] = drdtAutoCashFlow[dc.ColumnName].ToString();
                                }
                            }
                            ViewState["dtAutoCashFlow"] = dtAutoCashFlowtemp;
                        }

                    }
                }
                gvInflow.DataSource = null;
                gvInflow.DataBind();

                DtCashFlow = new DataTable();
                DtCashFlow.Columns.Add("Date");
                DtCashFlow.Columns.Add("CashInFlowID");
                DtCashFlow.Columns.Add("CashInFlow");
                DtCashFlow.Columns.Add("EntityID");
                DtCashFlow.Columns.Add("Entity");
                DtCashFlow.Columns.Add("InflowFromId");
                DtCashFlow.Columns.Add("InflowFrom");
                DtCashFlow.Columns.Add("Amount", typeof(decimal));
                DtCashFlow.Columns.Add("Accounting_IRR");
                DtCashFlow.Columns.Add("Business_IRR");
                DtCashFlow.Columns.Add("Company_IRR");
                DtCashFlow.Columns.Add("CashFlow_Flag_ID", typeof(int));

                DataRow dr = DtCashFlow.NewRow();
                dr["Date"] = "01/01/1900";
                dr["CashInFlowID"] = "";
                dr["CashInFlow"] = "";
                dr["EntityID"] = "";
                dr["Entity"] = "";
                dr["InflowFromId"] = "";
                dr["InflowFrom"] = "";
                dr["Amount"] = 0;
                dr["Accounting_IRR"] = "";
                dr["Business_IRR"] = "";
                dr["Company_IRR"] = "";
                dr["CashFlow_Flag_ID"] = 0;
                DtCashFlow.Rows.Add(dr);

            }
            if (Mode == strEditMode)
            {
                if ((DataTable)ViewState["DtCashFlow"] != null)
                    DtCashFlow = (DataTable)ViewState["DtCashFlow"];

            }

            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();
            if (Mode == strAddMode)
            {
                DtCashFlow.Rows.Clear();
                ViewState["DtCashFlow"] = DtCashFlow;
                DtCashFlow.Dispose();
                gvInflow.Rows[0].Cells.Clear();
                gvInflow.Rows[0].Visible = false;

            }
            FunPriGenerateNewInflow();


        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        //finally
        //{
        //    if (ObjCustomerService != null)
        //        ObjCustomerService.Close();
        //}
    }

    private void FunPriGenerateNewInflow()
    {
        try
        {
            DropDownList ddlInflowDesc = gvInflow.FooterRow.FindControl("ddlInflowDesc") as DropDownList;
            //DropDownList ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;

            Utility.FillDLL(ddlInflowDesc, ((DataSet)ViewState["InflowDDL"]).Tables[2], true);
            //Utility.FillDLL(ddlEntityName_InFlow, ((DataSet)ViewState["InflowDDL"]).Tables[1], true);
            Utility.FillDLL(ddlEntityName_InFlowFrom, ((DataSet)ViewState["InflowDDL"]).Tables[0], true);
            ddlEntityName_InFlowFrom.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriInsertUMFC()
    {
        try
        {
            DtCashFlow = (DataTable)ViewState["DtCashFlow"];
            DataSet dsUMFC = (DataSet)ViewState["InflowDDL"];
            DataRow dr = DtCashFlow.NewRow();
            DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["Date"] };
            dr["Date"] = DateTime.Today.ToString();
            string[] strArrayIds = null;
            string cashflowdesc = "";
            foreach (DataRow drOut in dsUMFC.Tables[2].Rows)
            {
                string[] strCashflow = drOut["CashFlow_ID"].ToString().Split(',');
                if (strCashflow[4].ToString() == "34")
                {
                    strArrayIds = strCashflow;
                    cashflowdesc = drOut["CashFlow_Description"].ToString();
                }
            }
            if (strArrayIds == null)
            {
                Utility.FunShowAlertMsg(this, "Define the Cashflow for UMFC in Cashflow Master");
                return;
            }
            dr["CashInFlowID"] = strArrayIds[0];
            dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
            dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
            dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
            dr["CashFlow_Flag_ID"] = strArrayIds[4];
            dr["CashInFlow"] = cashflowdesc;
            dr["EntityID"] = hdnCustID.Value;
            dr["Entity"] = S3GCustomerAddress1.CustomerName;
            dr["InflowFromId"] = "144";
            dr["InflowFrom"] = "Customer";
            if (ddl_Repayment_Mode.SelectedValue == "2")
            {
                //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 start

                if (Convert.ToInt32(ddl_Return_Pattern.SelectedValue) > 2)
                {
                    dr["Amount"] = FunPriGetInterestAmount().ToString();
                }
                else
                {
                    dr["Amount"] = FunPriGetStructureAdhocInterestAmount().ToString();
                }
                //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 end

            }
            else
            {
                dr["Amount"] = FunPriGetInterestAmount().ToString();
            }

            DtCashFlow.Rows.Add(dr);

            if (strMode == "C" && ViewState["dtAutoCashFlow"] != null)
                DtCashFlow.Merge((DataTable)ViewState["dtAutoCashFlow"]);

            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();

            ViewState["DtCashFlow"] = DtCashFlow;
            FunPriGenerateNewInflow();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriInsertInflow()
    {
        try
        {
            DtCashFlow = (DataTable)ViewState["DtCashFlow"];

            TextBox txtDate_GridInflow1 = gvInflow.FooterRow.FindControl("txtDate_GridInflow") as TextBox;
            DropDownList ddlInflowDesc1 = gvInflow.FooterRow.FindControl("ddlInflowDesc") as DropDownList;
            //DropDownList ddlEntityName_InFlow1 = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow1 = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;
            TextBox txtAmount_Inflow1 = gvInflow.FooterRow.FindControl("txtAmount_Inflow") as TextBox;

            string[] strArrayIds = ddlInflowDesc1.SelectedValue.Split(',');

            if (DtCashFlow.Rows.Count > 0)
            {
                DataRow[] drDupCashFlow = DtCashFlow.Select(" Date ='"
                    + Utility.StringToDate(txtDate_GridInflow1.Text)
                    + "' and CashFlow_Flag_ID = " + strArrayIds[4]
                    + " and InflowFromId = " + ddlEntityName_InFlowFrom.SelectedValue
                    + " and EntityID = " + ddlEntityName_InFlow1.SelectedValue);

                if (drDupCashFlow.Count() > 0)
                {
                    Utility.FunShowAlertMsg(this, "Cannot add duplicate Cash inflow");
                    return;
                }
            }

            DataRow dr = DtCashFlow.NewRow();
            //DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["Date"] };
            dr["Date"] = Utility.StringToDate(txtDate_GridInflow1.Text);
            dr["CashInFlowID"] = strArrayIds[0];
            dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
            dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
            dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
            dr["CashFlow_Flag_ID"] = strArrayIds[4];
            dr["CashInFlow"] = ddlInflowDesc1.SelectedItem;

            dr["EntityID"] = ddlEntityName_InFlow1.SelectedValue;
            dr["Entity"] = ddlEntityName_InFlow1.SelectedText;
            dr["InflowFromId"] = ddlEntityName_InFlowFrom.SelectedValue;
            DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["Date"], DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["InflowFromId"], DtCashFlow.Columns["EntityID"] };
            dr["InflowFrom"] = ddlEntityName_InFlowFrom.SelectedItem;
            dr["Amount"] = txtAmount_Inflow1.Text;

            DtCashFlow.Rows.Add(dr);

            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();

            ViewState["DtCashFlow"] = DtCashFlow;
            FunPriGenerateNewInflow();

            /* Clear the IRR for Generating RepayStructure. If we calls FunPriIRRREset() here, It clears UMFC also. */
            txtAccountingIRR.Text = "";
            txtAccountIRR_Repay.Text = "";
            txtBusinessIRR.Text = "";
            txtBusinessIRR_Repay.Text = "";
            txtCompanyIRR.Text = "";
            txtCompanyIRR_Repay.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriRemoveInflow(GridViewDeleteEventArgs e)
    {
        try
        {
            DtCashFlow = (DataTable)ViewState["DtCashFlow"];
            if (DtCashFlow.Rows.Count > 0)
            {
                //FunPriIRRReset();
                txtAccountingIRR.Text = "";
                txtAccountIRR_Repay.Text = "";
                txtBusinessIRR.Text = "";
                txtBusinessIRR_Repay.Text = "";
                txtCompanyIRR.Text = "";
                txtCompanyIRR_Repay.Text = "";
                DtCashFlow.Rows.RemoveAt(e.RowIndex);
                ViewState["DtCashFlow"] = DtCashFlow;
                if (DtCashFlow.Rows.Count == 0)
                {
                    FunPriFillInflowDLL(strAddMode);
                }
                else
                {
                    gvInflow.DataSource = DtCashFlow;
                    gvInflow.DataBind();
                    FunPriGenerateNewInflow();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Remove Inflow");
        }
    }

    private void FunPriAssignInflowDateFormat(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridInflow1 = e.Row.FindControl("txtDate_GridInflow") as TextBox;
                txtDate_GridInflow1.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_InflowDate1 = e.Row.FindControl("CalendarExtenderSD_InflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_InflowDate1.Format = ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Assign the DateFormat in Inflow");
        }
    }

    private void FunPriLoadInflowCustomerEntityDLL()
    {
        try
        {
            //DropDownList ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;

            if (ddlEntityName_InFlowFrom.SelectedItem.Text.ToUpper() == "CUSTOMER")
            {
                if (S3GCustomerAddress1.CustomerName != string.Empty)
                {
                    //ddlEntityName_InFlow.Items.Clear();
                    //ListItem lstItem = new ListItem(S3GCustomerAddress1.CustomerName, hdnCustID.Value);
                    //ddlEntityName_InFlow.Items.Add(lstItem);

                    ddlEntityName_InFlow.Clear();
                    ddlEntityName_InFlow.SelectedValue = hdnCustID.Value;
                    ddlEntityName_InFlow.SelectedText = S3GCustomerAddress1.CustomerName;
                    ddlEntityName_InFlow.ReadOnly = true;

                }
            }
            else
            {
                ddlEntityName_InFlow.ReadOnly = false;
                ddlEntityName_InFlow.Clear();

                //ddlEntityName_InFlow.BindDataTable(((DataSet)ViewState["InflowDDL"]).Tables[1]);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Customer/Entity Name");
        }
    }

    #endregion

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Entity_Type", "8");
        Procparam.Add("@PrefixText", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));

        return suggetions.ToArray();

    }

    // --code commented and added by saran in 01-Aug-2014 Insurance start  
    [System.Web.Services.WebMethod]
    public static string[] GetVendorsInsurance(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Entity_Type", "11");
        Procparam.Add("@PrefixText", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));

        return suggetions.ToArray();

    }
    // --code commented and added by saran in 01-Aug-2014 Insurance end  

    [System.Web.Services.WebMethod]
    public static string[] GetAssetVendors(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            DataTable dtCommon = new DataTable();
            DataSet Ds = new DataSet();

            DataTable PricingAssetDetails = (DataTable)obj_Page.Session["PricingAssetDetails"];
            DataRow[] dr = PricingAssetDetails.Select("Pay_To_ID=137 and Entity_Code like '%" + prefixText + "%'");

            DataTable dtAssetEntity = new DataTable();
            if (dr.Length == 0)
                dtAssetEntity = PricingAssetDetails.Clone();
            else
                dtAssetEntity = (dr.CopyToDataTable().Copy());

            dtAssetEntity.Columns["Entity_ID"].ColumnName = "ID";
            dtAssetEntity.Columns["Entity_Code"].ColumnName = "Name";

            suggetions = Utility.GetSuggestions(dtAssetEntity, true);

            return suggetions.ToArray();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    #region Cash Out Flow
    private void FunPriFillOutflowDLL(string Mode)
    {
        try
        {
            //ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyId.ToString());
            if (ddlLOB.SelectedValue != "0")
            {
                objParameters.Add("@LobId", ddlLOB.SelectedValue);
            }
            DataSet dsInflow = Utility.GetDataset("s3g_org_loadOutflowLov", objParameters);
            ViewState["OutflowDDL"] = dsInflow;

            if (Mode == strAddMode)
            {
                //Code modified by Nataraj Y
                DtCashFlowOut = new DataTable();
                DtCashFlowOut.Columns.Add("Date");
                DtCashFlowOut.Columns.Add("CashOutFlowID");
                DtCashFlowOut.Columns.Add("CashOutFlow");
                DtCashFlowOut.Columns.Add("EntityID");
                DtCashFlowOut.Columns.Add("Entity");
                DtCashFlowOut.Columns.Add("OutflowFromId");
                DtCashFlowOut.Columns.Add("OutflowFrom");
                DtCashFlowOut.Columns.Add("Amount");
                DtCashFlowOut.Columns.Add("Accounting_IRR");
                DtCashFlowOut.Columns.Add("Business_IRR");
                DtCashFlowOut.Columns.Add("Company_IRR");
                DtCashFlowOut.Columns.Add("CashFlow_Flag_ID", typeof(int));
                DtCashFlowOut.Columns["Amount"].DataType = typeof(decimal);
                DtCashFlowOut.PrimaryKey = new DataColumn[] { DtCashFlowOut.Columns["CashOutFlowID"], DtCashFlowOut.Columns["Date"], DtCashFlowOut.Columns["EntityID"] };
                DataRow dr_out = DtCashFlowOut.NewRow();
                dr_out["Date"] = "01/01/1900";
                dr_out["CashOutFlowID"] = "";
                dr_out["CashOutFlow"] = "";
                dr_out["EntityID"] = "";
                dr_out["Entity"] = "";
                dr_out["OutflowFromId"] = "";
                dr_out["OutflowFrom"] = "";
                dr_out["Amount"] = "0";
                dr_out["Accounting_IRR"] = "";
                dr_out["Business_IRR"] = "";
                dr_out["Company_IRR"] = "";
                dr_out["CashFlow_Flag_ID"] = 0;
                DtCashFlowOut.Rows.Add(dr_out);

            }
            if (Mode == strEditMode)
            {
                if ((DataTable)ViewState["DtCashFlowOut"] != null)
                    DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
            }
            gvOutFlow.DataSource = DtCashFlowOut;
            gvOutFlow.DataBind();

            if (Mode == strAddMode)
            {

                DtCashFlowOut.Rows.Clear();
                ViewState["DtCashFlowOut"] = DtCashFlowOut;
                DtCashFlowOut.Dispose();

                gvOutFlow.Rows[0].Cells.Clear();
                gvOutFlow.Rows[0].Visible = false;
            }
            FunPriGenerateNewOutflow();
            if (((DataTable)ViewState["DtCashFlowOut"]).Rows.Count > 0)
            {
                lblTotalOutFlowAmount.Text = ((DataTable)ViewState["DtCashFlowOut"]).
                    Compute("sum(Amount)", "CashOutFlowID > 0").ToString();
            }
            else
                lblTotalOutFlowAmount.Text = "0";
        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        //finally
        //{
        //    if (ObjCustomerService != null)
        //        ObjCustomerService.Close();
        //}
    }

    private void FunPriGenerateNewOutflow()
    {
        try
        {
            DropDownList ddlInflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            //DropDownList ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlEntityName_InFlowFrom = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;

            Utility.FillDLL(ddlInflowDesc, ((DataSet)ViewState["OutflowDDL"]).Tables[2], true);
            if (ViewState["OutflowDDL"] != null)
            {
                DataTable dtCashFlowFrom = ((DataSet)ViewState["OutflowDDL"]).Tables[0];
                string vendor = (string)ViewState["vendor"];
                
                // --code commented and added by saran in 01-Aug-2014 Insurance start 
                //if (dtCashFlowFrom.Rows.Count > 1)
                if (dtCashFlowFrom.Rows.Count > 2)
                // --code commented and added by saran in 01-Aug-2014 Insurance end 

                {
                    switch (vendor)
                    {
                        case "vendor":
                            dtCashFlowFrom.Rows.RemoveAt(0);
                            break;
                        case "customer":
                            dtCashFlowFrom.Rows.RemoveAt(1);
                            break;
                    }
                    ((DataSet)ViewState["OutflowDDL"]).Merge(dtCashFlowFrom);
                }
                Utility.FillDLL(ddlEntityName_InFlowFrom, dtCashFlowFrom, true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriRemoveOutflow(GridViewDeleteEventArgs e)
    {
        try
        {
            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
            if (DtCashFlowOut.Rows.Count > 0)
            {
                DtCashFlowOut.Rows.RemoveAt(e.RowIndex);
                ViewState["DtCashFlowOut"] = DtCashFlowOut;
                if (DtCashFlowOut.Rows.Count == 0)
                {
                    FunPriFillOutflowDLL(strAddMode);
                    lblTotalOutFlowAmount.Text = "0";
                    FunPriIRRReset();
                }
                else
                {

                    FunProBindCashFlow();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Remove Outflow");
        }
    }

    private void FunPriInsertOutflow(DataTable dtAcctype)
    {
        try
        {

            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];

            TextBox txtDate_GridOutflow = gvOutFlow.FooterRow.FindControl("txtDate_GridOutflow") as TextBox;
            DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            DropDownList ddlPaymentto_OutFlow = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
            //DropDownList ddlEntityName_OutFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_OutFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as UserControls_S3GAutoSuggest;
            TextBox txtAmount_Outflow = gvOutFlow.FooterRow.FindControl("txtAmount_Outflow") as TextBox;

            DataRow dr = DtCashFlowOut.NewRow();

            dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
            if (Utility.CompareDates(txtDate.Text, txtDate_GridOutflow.Text) == -1)
            {
                Utility.FunShowAlertMsg(this, "Outflow date cannot be less than Application date");
                return;
            }

            DtCashFlowOut.PrimaryKey = new DataColumn[] { DtCashFlowOut.Columns["Date"], DtCashFlowOut.Columns["CashOutFlowID"], DtCashFlowOut.Columns["OutflowFromId"], DtCashFlowOut.Columns["EntityID"] };
            dr["Date"] = Utility.StringToDate(txtDate_GridOutflow.Text);
            string[] strArrayIds = ddlOutflowDesc.SelectedValue.Split(',');
            if (strArrayIds[4] == "41")
            {
                if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().ToUpper() == "DEFERRED PAYMENT")
                {
                    if (Utility.CompareDates(txtDate.Text, txtDate_GridOutflow.Text) == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Outflow date should be greater than Application date for Deferred Payment");
                        return;
                    }

                    if (((DataTable)ViewState["DtCashFlowOut"]).Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(((DataTable)ViewState["DtCashFlowOut"]).
                            Compute("Count(CashFlow_Flag_ID)", "CashFlow_Flag_ID = 41 and " +
                            " Date <> #" + Utility.StringToDate(txtDate_GridOutflow.Text.Trim()) + "#"))))
                        {
                            Int32 IntTotalOutflow = (Int32)((DataTable)ViewState["DtCashFlowOut"]).
                                Compute("Count(CashFlow_Flag_ID)", "CashFlow_Flag_ID = 41 and " +
                                " Date <> #" + Utility.StringToDate(txtDate_GridOutflow.Text.Trim()) + "#");
                            if (IntTotalOutflow >= 1)
                            {
                                Utility.FunShowAlertMsg(this, "Finance amount Outflow date should " +
                                    "be the same for all entities (" +
                                    DateTime.Parse(((DataTable)ViewState["DtCashFlowOut"]).Rows[0]["Date"].ToString(),
                                    CultureInfo.CurrentCulture).ToString(strDateFormat) + ")");
                                return;
                            }
                        }
                    }
                }

                if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Trade Advance" || dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Normal Payment")
                {
                    if (Utility.StringToDate(txtDate.Text) != Utility.StringToDate(txtDate_GridOutflow.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Outflow date should be equal to Application date for Normal Payment/Trade Advance");
                        return;
                    }
                }
            }

            if (DtCashFlowOut.Rows.Count > 0)
            {
                DataRow[] drDupCashFlow = DtCashFlowOut.Select(" Date ='"
                    + Utility.StringToDate(txtDate_GridOutflow.Text)
                    + "' and CashFlow_Flag_ID = " + strArrayIds[4]
                    + " and OutflowFromId = " + ddlPaymentto_OutFlow.SelectedValue
                    + " and EntityID = " + ddlEntityName_OutFlow.SelectedValue);

                if (drDupCashFlow.Count() > 0)
                {
                    Utility.FunShowAlertMsg(this, "Cannot add duplicate Cash outflow");
                    return;
                }
            }

            dr["CashOutFlowID"] = strArrayIds[0];
            dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
            dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
            dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
            dr["CashFlow_Flag_ID"] = strArrayIds[4];
            dr["CashOutFlow"] = ddlOutflowDesc.SelectedItem;
            dr["OutflowFrom"] = ddlPaymentto_OutFlow.SelectedItem;
            dr["OutflowFromId"] = ddlPaymentto_OutFlow.SelectedValue;
            dr["EntityID"] = ddlEntityName_OutFlow.SelectedValue;
            dr["Entity"] = ddlEntityName_OutFlow.SelectedText;
            dr["Amount"] = txtAmount_Outflow.Text;
            DtCashFlowOut.Rows.Add(dr);
            ViewState["DtCashFlowOut"] = DtCashFlowOut;


            FunProBindCashFlow();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunProBindCashFlow()
    {
        try
        {
            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
            lblTotalOutFlowAmount.Text = DtCashFlowOut.Compute("sum(Amount)", "CashOutFlowID > 0").ToString();
            //foreach (DataRow drrow in DtCashFlowOut.Rows)
            //{
            //    string strOutFlowAmount = drrow["Amount"].ToString();
            //    if (strOutFlowAmount != "")
            //    {
            //        dcmTotalamount = dcmTotalamount + Convert.ToDouble(strOutFlowAmount);
            //        lblTotalOutFlowAmount.Text = dcmTotalamount.ToString();
            //    }
            //}
            gvOutFlow.DataSource = DtCashFlowOut;
            gvOutFlow.DataBind();
            FunPriGenerateNewOutflow();
            FunPriIRRReset();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadOutflowCustomerEntity()
    {
        try
        {
            //DropDownList ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            UserControls_S3GAutoSuggest ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as UserControls_S3GAutoSuggest;
            DropDownList ddlPaymentto_OutFlow = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
            //Code Added by saran for UAT Fix in round 4 on 18-Jul-2012 start 
            TextBox txtAmount_Outflow = gvOutFlow.FooterRow.FindControl("txtAmount_Outflow") as TextBox;
            //Code Added by saran for UAT Fix in round 4 on 18-Jul-2012 end 
            ddlEntityName_InFlow.Clear();
            ddlEntityName_InFlow.ReadOnly = true;

            if (ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "CUSTOMER")
            {
                if (S3GCustomerAddress1.CustomerName != string.Empty) //SelectedIndex > 0)
                {
                    //ddlEntityName_InFlow.Items.Clear();
                    //ListItem lstItem = new ListItem(S3GCustomerAddress1.CustomerName, hdnCustID.Value);
                    //ddlEntityName_InFlow.Items.Add(lstItem);
                    ddlEntityName_InFlow.SelectedValue = hdnCustID.Value;
                    ddlEntityName_InFlow.SelectedText = S3GCustomerAddress1.CustomerName;
                    
                }
            }
            // --code commented and added by saran in 01-Aug-2014 Insurance start  

            else if (ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "ENTITY" || ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "INSURANCE COMPANY")
            {

                // --code commented and added by saran in 01-Aug-2014 Insurance start  
                DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
                string[] strArrayIds = ddlOutflowDesc.SelectedValue.Split(',');
                DataSet dsDealer = new DataSet();
                // --code commented and added by saran in 01-Aug-2014 Insurance start  
                if (ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "INSURANCE COMPANY")
                    ddlEntityName_InFlow.ServiceMethod = "GetVendorsInsurance";
                else
                    ddlEntityName_InFlow.ServiceMethod = "GetVendors";

                // --code commented and added by saran in 01-Aug-2014 Insurance end  
                ddlEntityName_InFlow.ReadOnly = false;
                ddlEntityName_InFlow.Clear();
                txtAmount_Outflow.Text = "";

                if (strArrayIds.Length >= 4)
                {
                    if (strArrayIds[4].ToString() == "41")
                    {
                        //Code Added by saran for UAT Fix in round 4 on 18-Jul-2012 start 
                        if (Session["PricingAssetDetails"] != null)
                        {
                            if (((DataTable)Session["PricingAssetDetails"]).Rows.Count > 0)
                            {
                                DataRow[] dr = ((DataTable)Session["PricingAssetDetails"]).Select("Pay_To_ID=137");//Entity
                                if (dr != null)
                                {
                                    if (dr.Length > 0)
                                    {
                                        string strEntity_Ids = "0";
                                        for (int i = 0; i < dr.Length; i++)
                                            strEntity_Ids += "," + dr[i]["Entity_ID"].ToString();

                                        DataRow[] drEntity = ((DataSet)ViewState["OutflowDDL"]).Tables[1].Select("Entity_ID in (" + strEntity_Ids + ")");
                                        if (drEntity != null)
                                        {
                                            if (drEntity.Length > 0)
                                            {
                                                ddlEntityName_InFlow.ServiceMethod = "GetAssetVendors";
                                                //ddlEntityName_InFlow.BindDataTable(drEntity.CopyToDataTable());

                                                if (drEntity.Length == 1)
                                                {
                                                    //ddlEntityName_InFlow.SelectedIndex = 1;
                                                    ddlEntityName_InFlow.ReadOnly = true;
                                                    ddlEntityName_InFlow.SelectedValue = drEntity[0]["Entity_ID"].ToString();
                                                    ddlEntityName_InFlow.SelectedText = drEntity[0]["Entity_Code"].ToString();

                                                    txtAmount_Outflow.Text = txtFinanceAmount.Text;
                                                }
                                                return;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        //Code Added by saran for UAT Fix in round 4 on 18-Jul-2012 end 


                        DataRow[] drDealer = ((DataSet)ViewState["OutflowDDL"]).Tables[1].Select("Entity_Type_Name = 'Dealer'");
                        dsDealer.Merge(drDealer);
                    }
                    else
                    {
                        dsDealer.Merge(((DataSet)ViewState["OutflowDDL"]).Tables[1]);
                    }
                }
                //ddlEntityName_InFlow.BindDataTable(dsDealer.Tables[0]);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Customer/Entity Name");
        }
    }

    private void FunPriAssignOutflowDateFormat(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridOutflow = e.Row.FindControl("txtDate_GridOutflow") as TextBox;
                txtDate_GridOutflow.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_OutflowDate = e.Row.FindControl("CalendarExtenderSD_OutflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_OutflowDate.Format = ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Assign DateFormat in Outflow");
        }
    }

    #endregion

    #region Repayment Details

    private void FunPriFillRepaymentDLL(string Mode)
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            //ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            if (Mode == strAddMode)
            {
                gvRepaymentDetails.DataSource = null;
                gvRepaymentDetails.DataBind();
                ObjStatus.Option = 52;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


            }
            if (Mode == strEditMode)
            {
                if ((DataTable)ViewState["DtRepayGrid"] != null)
                    DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];

            }

            gvRepaymentDetails.DataSource = DtRepayGrid;
            gvRepaymentDetails.DataBind();

            if (Mode == strAddMode)
            {
                DtRepayGrid.Rows.Clear();
                ViewState["DtRepayGrid"] = DtRepayGrid;
                gvRepaymentDetails.Rows[0].Cells.Clear();
                gvRepaymentDetails.Rows[0].Visible = false;
            }

            FunPriGenerateNewRepayment();

        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            //if (ObjCustomerService != null)
            ObjCustomerService.Close();
        }
    }

    private void FunPriGenerateNewRepayment()
    {
        try
        {
            DropDownList ddlRepaymentCashFlow_RepayTab = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            Utility.FillDLL(ddlRepaymentCashFlow_RepayTab, ((DataSet)ViewState["InflowDDL"]).Tables[3], true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Cashflow Description in Repayment");
        }
    }


    private void FunPriInsertRepayment()
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            DateTime dtNextFromdate; DateTime dtStartdate;
            DropDownList ddlRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            TextBox txtAmountRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtAmountRepaymentCashFlow_RepayTab") as TextBox;
            TextBox txtPerInstallmentAmount_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            TextBox txtBreakup_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;
            TextBox txtFromInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            TextBox txtToInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToInstallment_RepayTab") as TextBox;
            TextBox txtfromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            TextBox txtToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToDate_RepayTab") as TextBox;
            string[] strIds = ddlRepaymentCashFlow_RepayTab1.SelectedValue.ToString().Split(',');

            if (txtfromdate_RepayTab1.Text.Trim() != "" &&
                Utility.StringToDate(txtfromdate_RepayTab1.Text.Trim()) < DateTime.Now.Date)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Date should be greater than or Equal to System Date');", true);
                return;
            }

            if (Convert.ToInt32(txtToInstallment_RepayTab1.Text) > Convert.ToInt32(txtTenure.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To Installment should not exceed Tenure');", true);
                txtToInstallment_RepayTab1.Focus();
                return;
            }

            if (DtRepayGrid.Rows.Count > 0)
            {
                if (ddlLOB.SelectedItem.Text.Contains("TL") || ddlLOB.SelectedItem.Text.Contains("TE"))
                {
                    objRepaymentStructure.FunPubGetNextRepaydateTL(DtRepayGrid, ddl_Frequency.SelectedValue, ddlRepaymentCashFlow_RepayTab1.SelectedValue);
                }
                else
                {
                    objRepaymentStructure.FunPubGetNextRepaydate(DtRepayGrid, ddl_Frequency.SelectedValue);
                }
                if (txtfromdate_RepayTab1.Text != "")
                {
                    if (Utility.StringToDate(txtfromdate_RepayTab1.Text) < objRepaymentStructure.dtNextDate.Date)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlapped');", true);
                        return;
                    }
                    /*Changed by Prabhu.K on 23-Nov-2011 for UAT Issue*/
                    else if (Utility.StringToDate(txtfromdate_RepayTab1.Text) != objRepaymentStructure.dtNextDate.Date && (ddlRepaymentCashFlow_RepayTab1.SelectedValue == "91" || ddlRepaymentCashFlow_RepayTab1.SelectedValue == "23"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Date should be " + objRepaymentStructure.dtNextDate.ToString(strDateFormat) + "');", true);
                        return;
                    }
                }
            }

            DataTable dtMoratorium = (DataTable)ViewState["dtMoratorium"];

            if (dtMoratorium.Rows.Count > 0 && strIds[4] == "23")
            {
                DataRow[] drMoratoriumRangeExist = dtMoratorium.Select("ToDate >= #" + Utility.StringToDate(txtfromdate_RepayTab1.Text) + "#");
                if (drMoratoriumRangeExist.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Selected Installment Period Exist in Moratorium');", true);
                    return;
                }
            }
            else
            {
                if (DtRepayGrid.Rows.Count > 0)
                {
                    DataRow[] drRepayDetail = null;
                    drRepayDetail = DtRepayGrid.Select(" CASHFLOW_FLAG_ID = " + strIds[4] +
                        " and (( " + txtFromInstallment_RepayTab1.Text.Trim() + " >= FROMINSTALL " +
                        " and " + txtFromInstallment_RepayTab1.Text.Trim() + " <= TOINSTALL ) or " +
                        " ( " + txtToInstallment_RepayTab1.Text.Trim() + " >= FROMINSTALL and " +
                        txtToInstallment_RepayTab1.Text.Trim() + " <= TOINSTALL) or " +
                        " ( FROMINSTALL >= " + txtFromInstallment_RepayTab1.Text.Trim() +
                        " and FROMINSTALL <= " + txtToInstallment_RepayTab1.Text.Trim() + " ))");
                    if (drRepayDetail.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlaped');", true);
                        txtToInstallment_RepayTab1.Focus();
                        return;
                    }
                }
            }

            Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
            objMethodParameters.Add("REPAYMODE", ddl_Repayment_Mode.SelectedItem.Text.ToString());
            objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text.ToString());
            objMethodParameters.Add("CashFlow", ddlRepaymentCashFlow_RepayTab1.SelectedItem.Text);
            objMethodParameters.Add("CashFlowId", ddlRepaymentCashFlow_RepayTab1.SelectedValue);
            objMethodParameters.Add("PerInstall", txtPerInstallmentAmount_RepayTab1.Text);
            objMethodParameters.Add("Breakup", txtBreakup_RepayTab1.Text);
            objMethodParameters.Add("FromInstall", txtFromInstallment_RepayTab1.Text);
            objMethodParameters.Add("ToInstall", txtToInstallment_RepayTab1.Text);
            objMethodParameters.Add("FromDate", txtfromdate_RepayTab1.Text);
            objMethodParameters.Add("Frequency", ddl_Frequency.SelectedValue);
            objMethodParameters.Add("TenureType", ddlTenureType.SelectedItem.Text);
            objMethodParameters.Add("Tenure", txtTenure.Text);
            objMethodParameters.Add("DocumentDate", txtDate.Text);
            dtStartdate = Utility.StringToDate(txtDate.Text);
            string strErrorMessage = "";
            if (ddlLOB.SelectedItem.Text.Contains("TL") || ddlLOB.SelectedItem.Text.Contains("TE"))
            {
                objMethodParameters.Add("repayMode_id", ddl_Repayment_Mode.SelectedValue);
                objMethodParameters.Add("Levy", ddl_Interest_Levy.SelectedItem.Value);

                //Checking if other than normal payment , start date should be last payment date.
                if (ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
                {
                    DataTable dtAcctype = ((DataTable)ViewState["PaymentRules"]);
                    dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
                    string strAcctType = dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper();

                    if (strAcctType == "PROJECT FINANCE" || strAcctType == "DEFERRED PAYMENT" || strAcctType == "DEFERRED STRUCTURED")
                    {
                        DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
                        if (DtCashFlowOut.Rows.Count > 0)
                        {
                            DataRow drOutFlw = DtCashFlowOut.Select("CashFlow_Flag_ID=41").Last();
                            if (drOutFlw != null)
                            {
                                objMethodParameters.Remove("DocumentDate");
                                objMethodParameters.Add("DocumentDate", drOutFlw["Date"].ToString());
                                dtStartdate = Utility.StringToDate(drOutFlw["Date"].ToString());
                            }
                        }

                    }
                }
                objRepaymentStructure.FunPubAddRepaymentforTL(out dtNextFromdate, out strErrorMessage, out DtRepayGrid, DtRepayGrid, objMethodParameters);
            }
            else
            {
                objRepaymentStructure.FunPubAddRepayment(out dtNextFromdate, out strErrorMessage, out DtRepayGrid, DtRepayGrid, objMethodParameters);
            }

            if (strErrorMessage != "")
            {
                Utility.FunShowAlertMsg(this, strErrorMessage);
                return;
            }
            if (strIds[4] == "23")
            {
                decimal decIRRActualAmount = 0;
                decimal decTotalAmount = 0;

                decimal DecRoundOff;
                if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                    DecRoundOff = Convert.ToDecimal(ViewState["hdnRoundOff"]);
                else
                    DecRoundOff = 2;

                if (!((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))))
                {
                    if (!objRepaymentStructure.FunPubValidateTotalAmount(DtRepayGrid, txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue, txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "1", DecRoundOff))
                    {
                        DtRepayGrid.Rows.RemoveAt(DtRepayGrid.Rows.Count - 1);
                        Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                        return;
                    }
                }
                //else
                //{
                //    int intValidation = objRepaymentStructure.FunPubValidateTotalAmountTL((DataTable)ViewState["DtRepayGrid"], txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue, txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "", DecRoundOff);
                //    if (intValidation == 1)
                //    {
                //        Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                //        return;
                //    }
                //    else if (intValidation == 2)
                //    {
                //        Utility.FunShowAlertMsg(this, "Principal Amount Should be equal to finance amount (" + txtFinanceAmount.Text + ")");
                //        return;
                //    }
                //    else if (intValidation == 3)
                //    {
                //        Utility.FunShowAlertMsg(this, "Total Amount Should be equal to interest (" + (decTotalAmount - Convert.ToDecimal(txtFinanceAmount.Text)).ToString() + ")");
                //        return;
                //    }
                //    else if (intValidation == 6)
                //    {
                //        Utility.FunShowAlertMsg(this, "No Principal and Interest amount entered to calculate");
                //        return;
                //    }
                //    else if (intValidation == 4)
                //    {
                //        Utility.FunShowAlertMsg(this, "No Principal amount entered to calculate");
                //        return;
                //    }
                //    else if (intValidation == 5)
                //    {
                //        Utility.FunShowAlertMsg(this, "No Interest amount entered to calculate");
                //        return;
                //    }
                //}
            }

            gvRepaymentDetails.DataSource = DtRepayGrid;
            gvRepaymentDetails.DataBind();

            TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
            txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
            TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            //if (ddl_Rate_Type.SelectedItem.Text == "Floating" && string.IsNullOrEmpty(txtFBDate.Text))
            //{
            //    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
            //    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
            //}
            //else
            //{
            ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
            ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
            //}

            ViewState["DtRepayGrid"] = DtRepayGrid;

            if (ViewState["DtRepayGrid_TL"] != null)
            {
                DataTable DtRepayGrid_TL = (DataTable)ViewState["DtRepayGrid_TL"];
                DataRow drow = DtRepayGrid_TL.NewRow();

                for (int i = 0; i <= DtRepayGrid_TL.Columns.Count - 1; i++)
                {
                    drow[i] = DtRepayGrid.Rows[DtRepayGrid.Rows.Count - 1][i].ToString();
                }

                DtRepayGrid_TL.Rows.Add(drow);
                ViewState["DtRepayGrid_TL"] = DtRepayGrid_TL;
            }

            FunPriGenerateNewRepayment();
            FunPriIRRReset();
            FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private decimal FunPriGetAmountFinanced()
    {
        try
        {
            decimal decFinanaceAmt;
            decFinanaceAmt = Convert.ToDecimal(txtFinanceAmount.Text);// -FunPriGetMarginAmout();
            return Math.Round(decFinanaceAmt, 0);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error in getting finance amount");
        }
    }


    private void FunPriAssignMarginAmount()
    {
        try
        {
            if (!string.IsNullOrEmpty(txt_Margin_Percentage.Text))
            {
                txtMarginMoneyPer_Cashflow.Text = txt_Margin_Percentage.Text;
                txtMarginMoneyPer_Cashflow.ReadOnly = true;
                txtMarginMoneyAmount_Cashflow.ReadOnly = true;
                txtMarginMoneyAmount_Cashflow.Text = FunPriGetMarginAmout().ToString();

            }
            else
            {
                txtMarginMoneyPer_Cashflow.Text = txtMarginMoneyAmount_Cashflow.Text = "";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Assign the Margin Amount");
        }
    }
    private bool FunPriValidateTotalAmount(out decimal decActualAmount, out decimal decTotalAmount, string strOption)
    {
        try
        {
            if (strOption != "3")
            {
                decTotalAmount = FunPriGetAmountFinanced() + Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(ddlTenureType.SelectedItem.Text, FunPriGetAmountFinanced(), Convert.ToDecimal(txtRate.Text), Convert.ToInt32(txtTenure.Text)), 0);
            }
            else
            {
                decTotalAmount = FunPriGetAmountFinanced();
            }
            decActualAmount = 0;
            if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count <= 0)
            {
                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + " Add atleast one row Repayment details";
                cvApplicationProcessing.IsValid = false;
                return false;
            }
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            foreach (DataRow drRepyrow in DtRepayGrid.Rows)
            {
                decActualAmount += (Convert.ToDecimal(drRepyrow["TotalPeriodInstall"].ToString()));
            }
            if (strOption == "1")
            {
                if (decActualAmount > decTotalAmount)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (strOption == "2")
            {
                if (decActualAmount == decTotalAmount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (strOption == "3")
            {
                if (decActualAmount >= decTotalAmount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Total Amount");
        }

    }

    private bool FunPriValidateTenurePeriod(DateTime dtStartDate, DateTime dtEndDate)
    {
        DateTime dateInterval = new DateTime();
        bool blnIsvalid = true;
        try
        {
            switch (ddlTenureType.SelectedItem.Text.ToLower())
            {
                case "months":
                    dateInterval = dtStartDate.AddMonths(Convert.ToInt32(txtTenure.Text));
                    break;
                case "weeks":

                    int intAddweeks = Convert.ToInt32(txtTenure.Text) * 7;
                    dateInterval = dtStartDate.AddDays(intAddweeks);
                    break;
                case "days":
                    dateInterval = dtStartDate.AddDays(Convert.ToInt32(txtTenure.Text));
                    break;
            }
            if (dtEndDate > dateInterval)
            {
                blnIsvalid = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Tenure Period with Tenure Type");
        }
        return blnIsvalid;
    }

    private bool FunPriValidateTenurePeriod(int intActualTenurePeriod)
    {
        bool blnIsValid = false;
        try
        {
            if (intActualTenurePeriod == Convert.ToInt32(txtTenure.Text))
            {
                blnIsValid = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Tenure with Tenure Type");
        }
        return blnIsValid;
    }

    private void FunPriCalculateSummary(DataTable objDataTable, string strGroupByField, string strSumField)
    {
        try
        {
            DataTable dtSummaryDetails = Utility.FunPriCalculateSumAmount(objDataTable, strGroupByField, strSumField);
            gvRepaymentSummary.DataSource = dtSummaryDetails;
            gvRepaymentSummary.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Calculate Repayment Summary");
        }

    }



    private void FunPriRemoveRepayment(GridViewDeleteEventArgs e)
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            if (ViewState["DtRepayGrid_TL"] != null)
            {
                DataTable DtRepayGrid_TL = (DataTable)ViewState["DtRepayGrid_TL"];
                if (DtRepayGrid_TL.Rows.Count > 0)
                {
                    DtRepayGrid_TL.Rows.RemoveAt(DtRepayGrid_TL.Rows.Count - 1);
                }
            }
            if (DtRepayGrid.Rows.Count > 0)
            {
                DtRepayGrid.Rows.RemoveAt(e.RowIndex);

                if (DtRepayGrid.Rows.Count == 0)
                {
                    FunPriFillRepaymentDLL(strAddMode);
                    gvRepaymentSummary.DataSource = null;
                    gvRepaymentSummary.DataBind();
                }
                else
                {
                    gvRepaymentDetails.DataSource = DtRepayGrid;
                    gvRepaymentDetails.DataBind();
                    FunPriGenerateNewRepayment();
                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
                    TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
                    Label lblTODate_ReapyTab_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblTODate_ReapyTab");
                    DateTime dtTodate = Utility.StringToDate(lblTODate_ReapyTab_Upd.Text);
                    DateTime dtNextFromdate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedItem.Text, dtTodate);
                    txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
                    if (ddl_Repayment_Mode.SelectedValue != "2")
                    {
                        Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
                        if (lblCashFlowId.Text != "23")
                        {
                            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                        }
                    }
                    else
                    {
                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                    }
                }
            }
            grvRepayStructure.DataSource = null;
            grvRepayStructure.DataBind();
            FunPriIRRReset();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Remove Repayment");
        }
    }



    private void FunPriBindRepaymentDetails(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                intSlNo += 1;
                e.Row.Cells[0].Text = intSlNo.ToString();
                if (Request.QueryString["qsMode"] != null)
                {
                    if (Request.QueryString["qsMode"].ToString() == "Q")
                    {
                        AjaxControlToolkit.CalendarExtender calext_FromDate = e.Row.FindControl("calext_FromDate") as AjaxControlToolkit.CalendarExtender;
                        calext_FromDate.Enabled = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Repayment Details");
        }
    }



    private void FunPriAssignRepaymentDateFormat(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtToDate_RepayTab = e.Row.FindControl("txtToDate_RepayTab") as TextBox;
                txtToDate_RepayTab.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab = e.Row.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_ToDate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;

                TextBox txtfromdate_RepayTab = e.Row.FindControl("txtfromdate_RepayTab") as TextBox;
                txtfromdate_RepayTab.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab = e.Row.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_fromdate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender calext_FromDate = e.Row.FindControl("calext_FromDate") as AjaxControlToolkit.CalendarExtender;
                calext_FromDate.Format = ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Assign Date Format in Repayment Details");
        }
    }

    #endregion

    #region Alert

    private void FunPriFillAlertDLL(string Mode)
    {
        try
        {


            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyId.ToString());
            DataSet dsAlert = Utility.GetDataset("s3g_org_loadAlertLov", objParameters);

            //added by saranya on 08-Mar-2012 based on sudarsan observation to add Programs in Type field
            DataRow[] dr = dsAlert.Tables[0].Select("ID in(141,143,218, 220, 222)"); //Changed by Thangam M on 08/Nov/2012
            DataTable dtAlert = dr.CopyToDataTable();
            ViewState["AlertDDL"] = dtAlert;
            ViewState["AlertUser"] = dsAlert;
            //End Here

            if (Mode == strAddMode)
            {

                DataTable ObjDT = new DataTable();


                ObjDT.Columns.Add("Type");
                ObjDT.Columns.Add("TypeID");
                ObjDT.Columns.Add("UserContact");
                ObjDT.Columns.Add("UserContactID");
                ObjDT.Columns.Add("EMail");
                ObjDT.Columns["Email"].DataType = typeof(Boolean);
                ObjDT.Columns.Add("SMS");
                ObjDT.Columns["SMS"].DataType = typeof(Boolean);
                DataRow dr_Alert = ObjDT.NewRow();
                dr_Alert["Type"] = "";
                dr_Alert["TypeID"] = "";
                dr_Alert["UserContact"] = "";
                dr_Alert["UserContactID"] = "";
                dr_Alert["EMail"] = "False";
                dr_Alert["SMS"] = "False";
                ObjDT.Rows.Add(dr_Alert);

                gvAlert.DataSource = ObjDT;
                gvAlert.DataBind();

                ObjDT.Rows.Clear();
                ViewState["DtAlertDetails"] = ObjDT;

                gvAlert.Rows[0].Cells.Clear();
                gvAlert.Rows[0].Visible = false;

            }
            FunPriGenerateNewAlert();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        //finally
        //{
        //    if (ObjCustomerService != null)
        //        ObjCustomerService.Close();
        //}
    }

    private void FunPriGenerateNewAlert()
    {
      
             try
        {
            DropDownList ObjddlType_AlertTab = gvAlert.FooterRow.FindControl("ddlType_AlertTab") as DropDownList;
            //Removed By Shibu 18-Sep-2013
       //  DropDownList ObjddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as DropDownList;
            UserControls_S3GAutoSuggest ddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as UserControls_S3GAutoSuggest;
            Utility.FillDLL(ObjddlType_AlertTab, ((DataTable)ViewState["AlertDDL"]), true);
            //Utility.FillDLL(ObjddlContact_AlertTab, ((DataSet)ViewState["AlertUser"]).Tables[1], true);
        }
      
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the Contact User in Alert");
        }
    }

    private void FunPriInsertAlert()
    {
        try
        {
            DtAlertDetails = (DataTable)ViewState["DtAlertDetails"];

            DropDownList ddlAlert_Type = gvAlert.FooterRow.FindControl("ddlType_AlertTab") as DropDownList;
          //  DropDownList ddlAlert_ContactList = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as DropDownList;
            UserControls_S3GAutoSuggest ddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as UserControls_S3GAutoSuggest;
            CheckBox ChkAlertEmail = gvAlert.FooterRow.FindControl("ChkEmail") as CheckBox;
            CheckBox ChkAlertSMS = gvAlert.FooterRow.FindControl("ChkSMS") as CheckBox;

            if (ChkAlertEmail.Checked || ChkAlertSMS.Checked)
            {
                DataRow dr = DtAlertDetails.NewRow();

                dr["TypeId"] = ddlAlert_Type.SelectedValue;
                dr["Type"] = ddlAlert_Type.SelectedItem;
                dr["UserContactId"] = ddlContact_AlertTab.SelectedValue.ToString();
                dr["UserContact"] = ddlContact_AlertTab.SelectedText;
                dr["EMail"] = ChkAlertEmail.Checked;
                dr["SMS"] = ChkAlertSMS.Checked;

                DtAlertDetails.Rows.Add(dr);

                gvAlert.DataSource = DtAlertDetails;
                gvAlert.DataBind();
                ViewState["DtAlertDetails"] = DtAlertDetails;
                FunPriGenerateNewAlert();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Select Email or SMS');", true);

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Add Alert");
        }
    }

    private void FunPriRemoveAlert(GridViewDeleteEventArgs e)
    {
        try
        {
            DtAlertDetails = (DataTable)ViewState["DtAlertDetails"];
            if (DtAlertDetails.Rows.Count > 0)
            {
                DtAlertDetails.Rows.RemoveAt(e.RowIndex);

                if (DtAlertDetails.Rows.Count == 0)
                {
                    FunPriFillAlertDLL(strAddMode);
                }
                else
                {
                    gvAlert.DataSource = DtAlertDetails;
                    gvAlert.DataBind();
                    FunPriGenerateNewAlert();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Remove Alert Details");
        }
    }

    #endregion

    #region Follow Up

    private void FunPriFillFollowupDLL(string Mode)
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            //ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            DataTable ObjDT = new DataTable();
            bool blIsFooterRow = false;
            if (Mode == strAddMode)
            {
                ObjStatus.Option = 47;
                ObjStatus.Param1 = null;
                ObjDT = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                ObjDT.Columns.Add("FromUserId");
                ObjDT.Columns.Add("ToUserId");
                blIsFooterRow = true;
            }
            if (!blIsFooterRow)
                ObjDT = (DataTable)ViewState["DtFollowUp"];

            gvFollowUp.DataSource = ObjDT;
            gvFollowUp.DataBind();

            if (Mode == strAddMode)
            {
                ObjDT.Rows.Clear();
                ViewState["DtFollowUp"] = ObjDT;
                if (gvFollowUp.Rows.Count > 0)
                {
                    gvFollowUp.Rows[0].Cells.Clear();
                    gvFollowUp.Rows[0].Visible = false;
                }
            }
            FunPriGenerateNewFollowUp();


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            //if (ObjCustomerService != null)
            ObjCustomerService.Close();
        }

    }

    private void FunPriGenerateNewFollowUp()
    {
        try
        {
       //Removed By Shibu 18-Sep-2013
                //DropDownList ddlfrom_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as DropDownList;
                UserControls_S3GAutoSuggest ddlfrom_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as UserControls_S3GAutoSuggest;
                //DropDownList ddlTo_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as DropDownList;
                UserControls_S3GAutoSuggest ddlTo_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as UserControls_S3GAutoSuggest;
                //Utility.FillDLL(ddlfrom_GridFollowup, ((DataTable)ViewState["UserListFolloup"]), true);
                //Utility.FillDLL(ddlTo_GridFollowup, ((DataTable)ViewState["UserListFolloup"]), true);
         

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load From/To UserName in Followup Details");
        }
    }

    private void FunPriInsertFollowup()
    {
        try
        {
            DtFollowUp = (DataTable)ViewState["DtFollowUp"];

            TextBox txttxtDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtDate_GridFollowup") as TextBox;
                     //Removed By Shibu 18-Sep-2013
            //DropDownList ddlfrom_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as DropDownList;
            //DropDownList ddlTo_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as DropDownList;
            UserControls_S3GAutoSuggest ddlfrom_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as UserControls_S3GAutoSuggest;
            UserControls_S3GAutoSuggest ddlTo_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as UserControls_S3GAutoSuggest;


            TextBox txtAction_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtAction_GridFollowup") as TextBox;
            TextBox txtActionDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtActionDate_GridFollowup") as TextBox;
            TextBox txtCustomerResponse_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtCustomerResponse_GridFollowup") as TextBox;
            TextBox txtRemarks_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtRemarks_GridFollowup") as TextBox;
            if (Utility.CompareDates(txttxtDate_GridFollowup1.Text, txtActionDate_GridFollowup1.Text) != 1 && Utility.CompareDates(txttxtDate_GridFollowup1.Text, txtActionDate_GridFollowup1.Text) != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('Action Date should be greater than or Equal to Date in Followup');", true);
                return;
            }
            if (ddlfrom_GridFollowup1.SelectedValue == ddlTo_GridFollowup1.SelectedValue)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('From and To UserName should be different');", true);
                return;
            }


            DataRow dr = DtFollowUp.NewRow();
            dr["Date"] = Utility.StringToDate(txttxtDate_GridFollowup1.Text);
            dr["From"] = ddlfrom_GridFollowup1.SelectedText;
            dr["FromUserId"] = ddlfrom_GridFollowup1.SelectedValue;
            dr["To"] = ddlTo_GridFollowup1.SelectedText;
            dr["ToUserId"] = ddlTo_GridFollowup1.SelectedValue;
            dr["Action"] = txtAction_GridFollowup1.Text;
            dr["ActionDate"] = Utility.StringToDate(txtActionDate_GridFollowup1.Text);
            dr["CustomerResponse"] = txtCustomerResponse_GridFollowup1.Text;
            dr["Remarks"] = txtRemarks_GridFollowup1.Text;

            DtFollowUp.Rows.Add(dr);

            gvFollowUp.DataSource = DtFollowUp;
            gvFollowUp.DataBind();

            ViewState["DtFollowUp"] = DtFollowUp;
            FunPriGenerateNewFollowUp();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Add Followup");
        }
    }

    private void FunPriRemoveFollowup(GridViewDeleteEventArgs e)
    {
        try
        {
            DtFollowUp = (DataTable)ViewState["DtFollowUp"];
            if (DtFollowUp.Rows.Count > 0)
            {
                DtFollowUp.Rows.RemoveAt(e.RowIndex);

                if (DtFollowUp.Rows.Count == 0)
                {
                    FunPriFillFollowupDLL(strAddMode);
                }
                else
                {
                    gvFollowUp.DataSource = DtFollowUp;
                    gvFollowUp.DataBind();
                    FunPriGenerateNewFollowUp();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Remove Followup");
        }
    }

    private void FunPriAssignFollowupDateFormat(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridFollowup = e.Row.FindControl("txtDate_GridFollowup") as TextBox;
                txtDate_GridFollowup.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupDate = e.Row.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_FollowupDate.Format = ObjS3GSession.ProDateFormatRW;

                TextBox txtActionDate_GridFollowup = e.Row.FindControl("txtActionDate_GridFollowup") as TextBox;
                txtActionDate_GridFollowup.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupActionDate = e.Row.FindControl("CalendarExtenderSD_FollowupActionDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_FollowupActionDate.Format = ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Assign Date Format in Followup");
        }

    }

    #endregion

    #region Constitution Document

    private void FunPriLoadConsitutionBasedCustomer(int intCustomerId)
    {
        objProcedureParameter = new Dictionary<string, string>();
        try
        {
            objProcedureParameter.Add("@CompanyId", intCompanyId.ToString());
            objProcedureParameter.Add("@IsActive", "1");
            objProcedureParameter.Add("@CustomerId", intCustomerId.ToString());
            grvConsDocuments.BindGridView("s3g_Org_GetConstitution_Customer", objProcedureParameter);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the Constitution Documents");
        }
    }
    protected void lnkScannedReference_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriShowConsDocImage(sender);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }
    private void FunPriShowConsDocImage(object sender)
    {
        try
        {
            string strFieldAtt = ((LinkButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvConsDocuments", strFieldAtt);
            Label lblPath = grvConsDocuments.Rows[gRowIndex].FindControl("lblDocumentPath") as Label;
            string strFileName = lblPath.Text.Replace("\\", "/").Trim();
            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to View the Document");
        }
    }
    private void FunPriBindConstitutionDocuments(GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtVal = (TextBox)e.Row.FindControl("txtValues");
                CheckBox ObjchkIsMandatory = (CheckBox)e.Row.FindControl("chkIsMandatory");
                CheckBox ObjchkIsNeedImageCopy = (CheckBox)e.Row.FindControl("chkIsNeedImageCopy");

                ObjchkIsMandatory.Enabled = false;
                ObjchkIsNeedImageCopy.Enabled = false;
                CheckBox chkScanned = (CheckBox)e.Row.FindControl("chkScanned");
                CheckBox chkCollect = (CheckBox)e.Row.FindControl("chkCollected");
                LinkButton lnkScannedReference = (LinkButton)e.Row.FindControl("lnkScannedReference");
                //chkScanned.Enabled = !chkScanned.Checked; //if yes then disabled
                //lnkScannedReference.Enabled = chkScanned.Checked; // if yes then enabled

                lnkScannedReference.Enabled = chkScanned.Checked;

                //added by saranya
                chkCollect.Enabled = false;
                chkScanned.Enabled = false;
                //lnkScannedReference.Enabled = false;
                TextBox Remarks = (TextBox)e.Row.FindControl("txtRemark");
                TextBox txtValues = (TextBox)e.Row.FindControl("txtValues");
                Remarks.ReadOnly = true;
                txtValues.ReadOnly = true;
                //end


                if (txtVal != null)
                {
                    txtVal.Enabled = FunPriDisableValueField(e.Row.Cells[1].Text);
                }
                if (strMode == "Q")
                {
                    CheckBox chkCollected = (CheckBox)e.Row.FindControl("chkCollected");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemark");
                    chkCollected.Enabled = chkScanned.Enabled = false;
                    txtVal.ReadOnly = txtRemarks.ReadOnly = true;
                    txtRemarks.ToolTip = txtRemarks.Text;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Constitution Document Details");
        }
    }

    #endregion

    #region Guarantor / Invoice / Collateral

    private void FunPriFillGuarantorDLL()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        try
        {

            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyId.ToString());
            DataSet dsGuarantor = Utility.GetDataset("s3g_org_loadGuarantorLov", objParameters);
            ViewState["GuarantorDDL"] = dsGuarantor;
            if (ViewState["mode"].ToString() == strAddMode)
            {
                ObjStatus.Option = 53;
                ObjStatus.Param1 = null;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                gvGuarantor.DataSource = DtRepayGrid;
                gvGuarantor.DataBind();

                DtRepayGrid.Rows.Clear();
                ViewState["dtGuarantorGrid"] = DtRepayGrid;

                gvGuarantor.Rows[0].Cells.Clear();
                gvGuarantor.Rows[0].Visible = false;

            }
            if (ViewState["mode"].ToString() == strEditMode)
            {
                if (ViewState["dtGuarantorGrid"] != null)
                {
                    DtRepayGrid = (DataTable)ViewState["dtGuarantorGrid"];
                    gvGuarantor.DataSource = DtRepayGrid;
                    gvGuarantor.DataBind();
                }
            }
            FunPriGenerateNewGuarantor();
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjStatus = null;
            ObjCustomerService.Close();
        }
    }

    private void FunPriGenerateNewGuarantor()
    {
        try
        {
            if (gvGuarantor.FooterRow != null)
            {
                DropDownList ddlGuarantortype_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
                //DropDownList ddlCode_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlCode_GuarantorTab") as DropDownList;
                DropDownList ddlChargesequence_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlChargesequence_GuarantorTab") as DropDownList;
                UserControls_LOBMasterView ucCustomerLov = gvGuarantor.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
                ucCustomerLov.strControlID = ucCustomerLov.ClientID;
                Utility.FillDLL(ddlGuarantortype_GuarantorTab1, ((DataSet)ViewState["GuarantorDDL"]).Tables[0], true);
                FunPriSetWhiteSpaceDLL(ddlGuarantortype_GuarantorTab1);

                //ddlCode_GuarantorTab1.BindDataTable((DataTable)Session["CustomerDT"], new string[] {"Customer_Id","Customer_Code","Customer_Name" });

                Utility.FillDLL(ddlChargesequence_GuarantorTab1, ((DataSet)ViewState["GuarantorDDL"]).Tables[1], true);
                FunPriSetWhiteSpaceDLL(ddlChargesequence_GuarantorTab1);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Guarantor Type/Code/Charge Sequence in Guarantor");
        }
    }
    protected void ddlGuarantortype_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Guarantee"] = "G";

        DropDownList ddlGuarantortype_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
        UserControls_LOBMasterView ucCustomerLov = gvGuarantor.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
        if (ddlGuarantortype_GuarantorTab1.SelectedIndex > 0)
        {
            if (ddlGuarantortype_GuarantorTab1.SelectedItem.Text.StartsWith("G") && ddlGuarantortype_GuarantorTab1.SelectedItem.Text.EndsWith("1"))
            {
                ucCustomerLov.strLOV_Code = "GCMD";
            }
            else if (ddlGuarantortype_GuarantorTab1.SelectedItem.Text.Contains("G") && ddlGuarantortype_GuarantorTab1.SelectedItem.Text.Contains("2"))
            {
                ucCustomerLov.strLOV_Code = "PCMD";
            }
            else
            {
                //For Co-Applicant
                ucCustomerLov.strLOV_Code = "COAP";
            }
            ViewState["Type"] = ucCustomerLov.strLOV_Code;

        }
        ucCustomerLov.strControlID = ucCustomerLov.ClientID;
        //Page_Load(null, null);
    }
    private void FunPriInsertGuarantor()
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["dtGuarantorGrid"];

            DropDownList ddlGuarantortype_GuarantorTab = gvGuarantor.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
            //DropDownList ddlCode_GuarantorTab = gvGuarantor.FooterRow.FindControl("ddlCode_GuarantorTab") as DropDownList;
            DropDownList ddlChargesequence_GuarantorTab = gvGuarantor.FooterRow.FindControl("ddlChargesequence_GuarantorTab") as DropDownList;
            TextBox txtGuaranteeamount_GuarantorTab = gvGuarantor.FooterRow.FindControl("txtGuaranteeamount_GuarantorTab_Footer") as TextBox;
            UserControls_LOBMasterView ucCustomerLov = gvGuarantor.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
            HiddenField hdnCustomerId = ucCustomerLov.FindControl("hdnID") as HiddenField;
            TextBox txtName = ucCustomerLov.FindControl("txtName") as TextBox;
            if (txtName.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Guarantor Details", "alert('Select the Guarantor');", true);
                return;
            }
            if (hdnCustID.Value == hdnCustomerId.Value)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Guarantor Details", "alert('Guarantor should be other than Customer');", true);
                return;
            }
            DataRow[] drDuplicateGuarantor = DtRepayGrid.Select("Code = " + hdnCustomerId.Value);
            if (drDuplicateGuarantor.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Guarantor Details", "alert('Guarantor already Exists');", true);
                return;
            }
            DataRow dr = DtRepayGrid.NewRow();

            dr["Guarantortype"] = ddlGuarantortype_GuarantorTab.SelectedValue;
            dr["Guarantor"] = ddlGuarantortype_GuarantorTab.SelectedItem.Text;
            dr["Code"] = hdnCustomerId.Value;
            dr["Name"] = txtName.Text;
            dr["Amount"] = txtGuaranteeamount_GuarantorTab.Text;
            if (ddlChargesequence_GuarantorTab.SelectedIndex > 0)
            {
                dr["Charge"] = ddlChargesequence_GuarantorTab.SelectedValue;
                dr["ChargeSequence"] = ddlChargesequence_GuarantorTab.SelectedItem.Text;
            }
            dr["View"] = "View";

            DtRepayGrid.Rows.Add(dr);

            gvGuarantor.DataSource = DtRepayGrid;
            gvGuarantor.DataBind();

            ViewState["dtGuarantorGrid"] = DtRepayGrid;
            FunPriGenerateNewGuarantor();
            FunPriSetMaxLength_gvGuarantor();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Add Guarantor");
        }
    }

    private void FunPriRemoveGuarantor(GridViewDeleteEventArgs e)
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["dtGuarantorGrid"];
            if (DtRepayGrid.Rows.Count > 0)
            {
                DtRepayGrid.Rows.RemoveAt(e.RowIndex);

                if (DtRepayGrid.Rows.Count == 0)
                {
                    ViewState["mode"] = strAddMode;
                    FunPriFillGuarantorDLL();
                }
                else
                {
                    gvGuarantor.DataSource = DtRepayGrid;
                    gvGuarantor.DataBind();
                    FunPriGenerateNewGuarantor();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Remove Guarantor");
        }
    }

    private static void FunPriBindGuarantorDetails(GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblGuaranteeID = e.Row.FindControl("lblGuaranteeID") as Label;
                LinkButton lbtnViewCustomer = e.Row.FindControl("lbtnViewCustomer") as LinkButton;
                if (lbtnViewCustomer != null && lblGuaranteeID != null)
                {
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblGuaranteeID.Text, false, 0);
                    lbtnViewCustomer.Attributes.Add("onclick", "window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&qsCustomerId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q', 'null','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;");
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtGuaranteeamount_GuarantorTab_Footer = e.Row.FindControl("txtGuaranteeamount_GuarantorTab_Footer") as TextBox;
                txtGuaranteeamount_GuarantorTab_Footer.SetDecimalPrefixSuffix(10, 0, true, "Guarantee Amount");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Guarantor Details");
        }
    }

    private static void FunPriBindInvoiceDetails(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblInvoiceReferNo = e.Row.FindControl("lblInvoiceReferNo") as Label;
                LinkButton lbtnViewInvoice = e.Row.FindControl("lbtnViewInvoice") as LinkButton;
                if (lbtnViewInvoice != null && lblInvoiceReferNo != null)
                {
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblInvoiceReferNo.Text, false, 0);
                    //lbtnViewInvoice.Attributes.Add("onclick", "window.open('../LoanAdmin/S3GLoanAdInvoiceVendor_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsFromAccount=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;");
                    string myURL = "window.showModalDialog('../LoanAdmin/S3GLoanAdInvoiceVendor_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsFromAccount=Yes','#1','dialogHeight: 600; dialogWidth: 950;dialogTop: 190px;  dialogLeft: 220px; edge: Raised; center: No;help: No; resizable: No; status: No;')";
                    lbtnViewInvoice.Attributes.Add("onclick", myURL);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Invoice Details");
        }
    }

    #endregion

    #region MLA / SLA

    private void FunPriShowMLAROIDetails(DataTable ObjDt)
    {
        try
        {
            DataTable ObjDTPaymentGen = new DataTable();
            DataColumn dc1 = new DataColumn("FieldName");
            DataColumn dc2 = new DataColumn("FieldValue");
            ObjDTPaymentGen.Columns.Add(dc1);
            ObjDTPaymentGen.Columns.Add(dc2);

            for (int i = 0; i < ObjDt.Columns.Count; i++)
            {
                DataRow dr = ObjDTPaymentGen.NewRow();
                dr[0] = ObjDt.Columns[i].ColumnName.Replace("_", " ");

                if (ObjDt.Rows.Count > 0)
                    dr[1] = ObjDt.Rows[0][i].ToString();
                else
                    dr[1] = string.Empty;
                ObjDTPaymentGen.Rows.Add(dr);
            }

            gv_MLAROI.DataSource = ObjDTPaymentGen;
            gv_MLAROI.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load ROI Details in Prime Account Tab");

        }
    }

    #endregion

    #region Moratorium

    private void FunPriFillMoratoriumDLL()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 1;
            ObjStatus.Param1 = "MORATORIUM_TYPE";
            ViewState["MoratoriumType"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            if (ViewState["mode"].ToString() == strAddMode)
            {
                ObjStatus.Option = 54;
                ObjStatus.Param1 = null;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                gvMoratorium.DataSource = DtRepayGrid;
                gvMoratorium.DataBind();

                DtRepayGrid.Rows.Clear();
                ViewState["dtMoratorium"] = DtRepayGrid;

                gvMoratorium.Rows[0].Cells.Clear();
                gvMoratorium.Rows[0].Visible = false;


            }
            if (ViewState["mode"].ToString() == strEditMode)
            {
                if (ViewState["dtMoratorium"] != null)
                {
                    DtRepayGrid = (DataTable)ViewState["dtMoratorium"];
                    gvMoratorium.DataSource = DtRepayGrid;
                    gvMoratorium.DataBind();
                }
            }
            TextBox txtFromdate_MoratoriumTab = gvMoratorium.FooterRow.FindControl("txtFromdate_MoratoriumTab") as TextBox;
            TextBox txtTodate_MoratoriumTab = gvMoratorium.FooterRow.FindControl("txtTodate_MoratoriumTab") as TextBox;
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_MoratoriumTab1 = gvMoratorium.FooterRow.FindControl("CalendarExtenderSD_ToDate_MoratoriumTab") as AjaxControlToolkit.CalendarExtender;
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FromDate_MoratoriumTab1 = gvMoratorium.FooterRow.FindControl("CalendarExtenderSD_FromDate_MoratoriumTab") as AjaxControlToolkit.CalendarExtender;

            txtFromdate_MoratoriumTab.Attributes.Add("readonly", "readonly");
            txtTodate_MoratoriumTab.Attributes.Add("readonly", "readonly");
            CalendarExtenderSD_ToDate_MoratoriumTab1.Format = strDateFormat;
            CalendarExtenderSD_FromDate_MoratoriumTab1.Format = strDateFormat;

            FunPriGenerateNewMoratorium();
            //ObjCustomerService.Close();

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objFaultExp);
            //ObjCustomerService.Close();
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            //ObjCustomerService.Close();
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriGenerateNewMoratorium()
    {
        try
        {
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_MoratoriumTab1 = gvMoratorium.FooterRow.FindControl("CalendarExtenderSD_ToDate_MoratoriumTab") as AjaxControlToolkit.CalendarExtender;
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FromDate_MoratoriumTab1 = gvMoratorium.FooterRow.FindControl("CalendarExtenderSD_FromDate_MoratoriumTab") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_ToDate_MoratoriumTab1.Format = strDateFormat;
            CalendarExtenderSD_FromDate_MoratoriumTab1.Format = strDateFormat;

            DropDownList ddlMoratoriumtype_MoratoriumTab = gvMoratorium.FooterRow.FindControl("ddlMoratoriumtype_MoratoriumTab") as DropDownList;
            Utility.FillDLL(ddlMoratoriumtype_MoratoriumTab, ((DataTable)ViewState["MoratoriumType"]), true);
            DtRepayGrid = (DataTable)ViewState["dtMoratorium"];
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Moratorium Type");
        }
    }

    private void FunPriSetDLLSelecteItem(DropDownList ObjDrop, string str)
    {
        try
        {
            if (!string.IsNullOrEmpty(str))
            {
                ObjDrop.SelectedValue = str;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriInsertMoratorium()
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["dtMoratorium"];

            DropDownList ddlMoratoriumtype_MoratoriumTab = gvMoratorium.FooterRow.FindControl("ddlMoratoriumtype_MoratoriumTab") as DropDownList;
            TextBox txtFromdate_MoratoriumTab = gvMoratorium.FooterRow.FindControl("txtFromdate_MoratoriumTab") as TextBox;
            TextBox txtTodate_MoratoriumTab = gvMoratorium.FooterRow.FindControl("txtTodate_MoratoriumTab") as TextBox;

            DataRow dr = DtRepayGrid.NewRow();

            dr["Moratoriumtype"] = ddlMoratoriumtype_MoratoriumTab.SelectedValue;
            dr["Moratorium"] = ddlMoratoriumtype_MoratoriumTab.SelectedItem.Text;
            dr["Fromdate"] = Utility.StringToDate(txtFromdate_MoratoriumTab.Text);
            dr["Todate"] = Utility.StringToDate(txtTodate_MoratoriumTab.Text);
            dr["Noofdays"] = (Utility.StringToDate(txtTodate_MoratoriumTab.Text) - Utility.StringToDate(txtFromdate_MoratoriumTab.Text)).TotalDays;
            if (ddl_Repayment_Mode.SelectedValue == "2")
            {
                if (!ddlLOB.SelectedItem.Text.ToLower().StartsWith("wc") && !ddlLOB.SelectedItem.Text.ToLower().StartsWith("ft"))
                {
                    int intValidRange = FunPriGetMoratoriumDays(Convert.ToInt32(dr["Noofdays"]));
                    if (intValidRange > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Moratorium details", "alert('Moratorium Period should be " + ddl_Frequency.SelectedItem.Text + " basis');", true);
                        return;
                    }
                }
            }

            if (Utility.CompareDates(txtFromdate_MoratoriumTab.Text, txtTodate_MoratoriumTab.Text) != 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Moratorium details", "alert('To Date should be greater than from date in Moratorium');", true);
                return;
            }

            DtRepayGrid.Rows.Add(dr);

            gvMoratorium.DataSource = DtRepayGrid;
            gvMoratorium.DataBind();

            ViewState["dtMoratorium"] = DtRepayGrid;
            FunPriGenerateNewMoratorium();
            if (ddl_Repayment_Mode.SelectedValue == "2")
            {
                FunPriResetIRRDetails();
                TabContainerAP.ActiveTabIndex = 2;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Add Moratorium");
        }
    }
    private int FunPriGetMoratoriumDays(int intNoDays)
    {
        int intNoInstalment = 0;
        switch (ddl_Frequency.SelectedValue)
        {
            //Weekly
            case "2":
                intNoInstalment = intNoDays % 7;

                break;
            //Fortnightly
            case "3":
                intNoInstalment = intNoDays % 15;

                break;
            //Monthly
            case "4":
                intNoInstalment = intNoDays % 30;
                break;
            //bi monthly
            case "5":
                intNoInstalment = intNoDays % 60;
                break;
            //quarterly
            case "6":
                intNoInstalment = intNoDays % 120;
                break;
            // half yearly
            case "7":
                intNoInstalment = intNoDays % 180;
                break;
            //annually
            case "8":
                intNoInstalment = intNoDays % 365;
                break;

        }
        return intNoInstalment;
    }
    private void FunPriRemoveMoratorium(GridViewDeleteEventArgs e)
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["dtMoratorium"];
            if (DtRepayGrid.Rows.Count > 0)
            {
                DtRepayGrid.Rows.RemoveAt(e.RowIndex);

                if (DtRepayGrid.Rows.Count == 0)
                {
                    ViewState["mode"] = strAddMode;
                    FunPriFillMoratoriumDLL();
                }
                else
                {
                    gvMoratorium.DataSource = DtRepayGrid;
                    gvMoratorium.DataBind();
                    FunPriGenerateNewMoratorium();
                }
                if (ddl_Repayment_Mode.SelectedValue == "2")
                {
                    FunPriResetIRRDetails();
                    TabContainerAP.ActiveTabIndex = 2;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Remove Moratorium");
        }
    }

    private void FunPriLoadMoratorium(string Appid)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 73;
            ObjStatus.Param1 = Appid;
            DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            gvMoratorium.DataSource = DtRepayGrid;
            gvMoratorium.DataBind();

            ViewState["dtMoratorium"] = DtRepayGrid;

            FunPriGenerateNewMoratorium();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Moratorium");
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    #endregion

    #region PreDisbursement

    private void FunPriShowPRDD(object sender)
    {
        try
        {
            string strFieldAtt = ((LinkButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvPRDDT", strFieldAtt);
            Label lblPath = gvPRDDT.Rows[gRowIndex].FindControl("lblPath") as Label;
            string strFileName = lblPath.Text.Replace("\\", "/").Trim();
            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to View the Document");
        }
    }
    protected void ddlCollectedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlCollectedBy = sender as DropDownList;
        if (ddlCollectedBy.SelectedIndex > 0)
        {
            int intCurrentRow = ((GridViewRow)ddlCollectedBy.Parent.Parent).RowIndex;
            Label lblCollectedBy = (Label)gvPRDDT.Rows[intCurrentRow].FindControl("lblCollectedBy");
            lblCollectedBy.Text = ddlCollectedBy.SelectedValue;
        }

    }
    protected void ddlScannedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlScannedBy = sender as DropDownList;
        if (ddlScannedBy.SelectedIndex > 0)
        {
            int intCurrentRow = ((GridViewRow)ddlScannedBy.Parent.Parent).RowIndex;
            Label lblScannedBy = (Label)gvPRDDT.Rows[intCurrentRow].FindControl("lblScannedBy");
            lblScannedBy.Text = ddlScannedBy.SelectedValue;
        }
    }

    private void FunPriBindPRDD(GridViewRowEventArgs e)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                UserInfo ObjUserInfo = new UserInfo();
                DropDownList ddlCollectedby = (DropDownList)e.Row.FindControl("ddlCollectedby");
                AjaxControlToolkit.CalendarExtender calCollectedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calCollectedDate");
                AjaxControlToolkit.CalendarExtender calScannedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calScannedDate");
                calScannedDate.Format = calCollectedDate.Format = strDateFormat;
                TextBox txtColletedDate = (TextBox)e.Row.FindControl("txtCollectedDate");
                LinkButton Viewdoct = (LinkButton)e.Row.FindControl("hyplnkView");
                CheckBox Cbx1 = (CheckBox)e.Row.FindControl("CbxCheck");
                DropDownList ddlScannedby = (DropDownList)e.Row.FindControl("ddlScannedby");
                TextBox txtScannedDate = (TextBox)e.Row.FindControl("txtScannedDate");
                TextBox txtUpload = (TextBox)e.Row.FindControl("txOD");
                Label lblPath = e.Row.FindControl("lblPath") as Label;
                //Label lblScanned = e.Row.FindControl("lblScanned") as Label;
                Label myThrobber = e.Row.FindControl("myThrobber") as Label;

                ObjStatus.Option = 35;
                ObjStatus.Param1 = intCompanyId.ToString();
                Utility.FillDLL(ddlCollectedby, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

                ObjStatus.Option = 35;
                ObjStatus.Param1 = intCompanyId.ToString();
                Utility.FillDLL(ddlScannedby, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

                Label lblCollectedBy = e.Row.FindControl("lblCollectedBy") as Label;
                Label lblScannedBy = e.Row.FindControl("lblScannedBy") as Label;
                if (lblCollectedBy.Text != "")
                {
                    ddlCollectedby.SelectedValue = lblCollectedBy.Text;
                }
                if (lblScannedBy.Text != "")
                {
                    ddlScannedby.SelectedValue = lblScannedBy.Text;
                }

                if (ViewState["PRDDDocPath"] != null)
                    txtUpload.Text = ViewState["PRDDDocPath"].ToString();
                AjaxControlToolkit.AsyncFileUpload asyFileUpload = e.Row.FindControl("asyFileUpload") as AjaxControlToolkit.AsyncFileUpload;
                Label lblScanned = e.Row.FindControl("lblScanned") as Label;
                string Path;
                Path = txtUpload.Text.Split('\\')[1].ToString();



                if (lblScanned != null && asyFileUpload != null)
                {

                    if (lblScanned.Text == "False")
                    {
                        myThrobber.Text = "";
                        Viewdoct.Enabled = false;
                        Cbx1.Checked = false;
                        txtScannedDate.Text = "";
                        txtScannedDate.Visible = false; //added
                        calScannedDate.Enabled = false;
                        ddlScannedby.ClearDropDownList();
                        ddlScannedby.Visible = false; //added
                    }
                    else
                    {
                        Cbx1.Checked = true;
                        if (Path == string.Empty)
                        {
                            Viewdoct.Enabled = false;

                        }
                        else
                        {

                            Viewdoct.Enabled = true;
                        }
                    }
                }
                if (txtScannedDate.Text.Contains("1900"))
                {
                    Cbx1.Checked = false;
                    txtScannedDate.Text = "";
                    txtScannedDate.Visible = false; //added
                    ddlScannedby.ClearDropDownList();
                    ddlScannedby.Visible = false; //added
                }

                //if (intApplicationProcessId >= 0)
                //{
                //    Cbx1.Checked = true;
                //}
                ObjUserInfo = null;

                asyFileUpload.Enabled = false;
                ddlCollectedby.ClearDropDownList();
                calCollectedDate.Enabled = false;
                txtColletedDate.ReadOnly = true;

                gvPRDDT.Columns[9].Visible = false;
                ddlScannedby.ClearDropDownList();
                calScannedDate.Enabled = false;
                txtScannedDate.ReadOnly = true;
                Cbx1.Enabled = false;
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                txtRemarks.ReadOnly = true;


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Pre-Disbursement Documents");
        }
        finally
        {
            ObjCustomerService.Close();
            ObjStatus = null;
        }
    }

    #endregion






    private void FunPriLoadPage()
    {
        try
        {

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            FormsAuthenticationTicket fromTicket;
            //Code end
            Session["CustomerNameForAsset"] = "";
            CalendarExtenderED.Format = strDateFormat;

            if (ViewState["hdnRoundOff"] != null && !string.IsNullOrEmpty(ViewState["hdnRoundOff"].ToString()))
            {
                S3GBusEntity.CommonS3GBusLogic.GPSRoundOff = Convert.ToInt32(ViewState["hdnRoundOff"].ToString());
            }

            if (ViewState["Password"] != null)
            {
                txtPassword.Text = ViewState["Password"].ToString();
            }
            if (Request.QueryString["qsViewId"] != null)
            {
                fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intApplicationProcessId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Application Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

                ViewState["mode"] = strEditMode;
            }
            ucCustomerCodeLov._strRegionID = "0";
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];

            if (strMode.ToUpper().Trim() != "Q")
                txtMarginAmount.CheckGPSLength(false, "Margin Amount");

            txtMarginMoneyAmount_Cashflow.CheckGPSLength(false, "Margin Amount");
            txtMarginMoneyPer_Cashflow.SetDecimalPrefixSuffix(2, 4, false, "Margin %");
            txtMarginAmount.Attributes.Add("readonly", "true");
            txtResidualValue.SetDecimalPrefixSuffix(10, 0, false, "Residual Amount");
            txtResidualAmt_Cashflow.SetDecimalPrefixSuffix(10, 0, false, "Residual Amount");
            txtResidualValue_Cashflow.SetPercentagePrefixSuffix(2, 4, false, true, "Residual %");
            //txtResidualValue_Cashflow.CheckGPSLength(false, "Residual %");

            txtCompanyIRR.Attributes.Add("readonly", "true");
            txtBusinessIRR.Attributes.Add("readonly", "true");
            txtAccountingIRR.Attributes.Add("readonly", "true");
            txtCompanyIRR_Repay.Attributes.Add("readonly", "true");
            txtBusinessIRR_Repay.Attributes.Add("readonly", "true");
            txtAccountIRR_Repay.Attributes.Add("readonly", "true");
            txtTenure.SetDecimalPrefixSuffix(3, 0, true, "Tenure");
            txtFinanceAmount.SetDecimalPrefixSuffix(10, 0, true, "Finance Amount");
            txtResidualValue.Attributes.Add("readonly", "true");
            if (Session["EnqNewCustomerId"] != null && string.IsNullOrEmpty(S3GCustomerAddress1.CustomerName))
            {
                intEnqNewCustomerId = Convert.ToInt32(Utility.Load("EnqNewCustomerId", ""));
                hdnCustID.Value = intEnqNewCustomerId.ToString();
                Session["AssetCustomer"] = hdnCustID.Value + ";" + S3GCustomerAddress1.CustomerName;

                //By Thangam M on 03/Oct/2012 to set customer id for new customer
                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                if (hdnCustomerId != null)
                {
                    hdnCustomerId.Value = hdnCustID.Value;
                }
                //End here

                if (intEnqNewCustomerId > 0)
                {
                    FunPubQueryExistCustomerListEnquiryUpdation(intEnqNewCustomerId);
                }
            }
            if (ddlLOB.SelectedIndex.ToString()!="-1" )
            {
                if (intApplicationProcessId == 0 || (intApplicationProcessId > 0 && ddlBusinessOfferNoList.SelectedValue == "-1"))
                {


                    if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL"))
                    {
                        //if (Session["PricingloanAssetDetails"] != null && ddlLOB.Items.Count > 0)
                        //{
                        //    DataTable dtloanassetdetails = (DataTable)Session["PricingloanAssetDetails"];
                        //    grvloanasset.DataSource = dtloanassetdetails;
                        //    grvloanasset.DataBind();
                        //}
                    }

                    else
                    {
                        if (Session["PricingAssetDetails"] != null && ddlLOB.Items.Count > 0)
                        {
                            DataTable dsAssetDetails = (DataTable)Session["PricingAssetDetails"];
                            gvAssetDetails.DataSource = dsAssetDetails;
                            gvAssetDetails.DataBind();
                            FunPriAssignMarginAmount();
                            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
                            {
                                decimal dcmAssetFinAmount = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
                                txtFinanceAmount.Text = (dcmAssetFinAmount == 0) ? "" : dcmAssetFinAmount.ToString();

                            }

                            decimal dcmMarginAmount = (decimal)(dsAssetDetails.Compute("Sum(Margin_Amount)", "Noof_Units > 0"));
                            txtMarginAmount.Text = (dcmMarginAmount == 0) ? "" : dcmMarginAmount.ToString();

                        }
                    }
                }

            }

            if (!IsPostBack)
            {
                Guarantee.Value = "";
                FunPriValidateApplicationStart();
                FunProGetIRRDetails();
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (intApplicationProcessId == 0)
                {
                    FunPriDisableControls(0);
                }
                else if (intApplicationProcessId > 0)
                {
                    if (strMode == "M")
                    {
                        FunPriDisableControls(1);
                    }
                    else if (strMode == "Q")
                    {
                        FunPriDisableControls(-1);
                    }
                }

                if (PageMode == PageModes.WorkFlow)
                {
                    ViewState["PageMode"] = PageModes.WorkFlow;
                }

                // WORK FLOW IMPLEMENTATION
                if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString() && !IsPostBack)
                {
                    PreparePageForWFLoad();
                }

                //Added by Thangam M on 25/Jul/2013 to create Pricing from CRM
                if (Request.QueryString.Get("qsCRMID") != null)
                {
                    FunPrILoadCRMInfo();
                }
                //End here
            }
            FunPriLoadFileNameInPRDDT();
            FunPriSetRateLength();
            FunPriSetMaxLength();
            //Workflow for Modify Mode
            if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())
            {
                if (ViewState["intApplicationProcessId"] != null)
                    intApplicationProcessId = Convert.ToInt32(ViewState["intApplicationProcessId"]);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //Added by Thangam M on 25/Jul/2013 to create Pricing from CRM
    private void FunPrILoadCRMInfo()
    {
        try
        {
            Page.Master.FindControl("SiteMapPath1").Visible = false;
            lblHeading.Text = lblHeading.Text + "  [From CRM]";

            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCRMID"));
            int intCRMID = Convert.ToInt32(fromTicket.Name);

            btnCreateCustomer.Enabled = false;
            Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            HiddenField hdnID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            btnGetLOV.Enabled = false;

            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CRM_ID", intCRMID.ToString());
            objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());

            DataSet dSet = Utility.GetDataset("S3G_CLN_GerCRMForApplication", objProcedureParameter);

            if (dSet != null && dSet.Tables[0].Rows.Count > 0)
            {
                txtName.Text = dSet.Tables[0].Rows[0]["Customer"].ToString();
                hdnCustID.Value = hdnID.Value = dSet.Tables[0].Rows[0]["Customer_ID"].ToString();
                btnLoadCustomer_OnClick(null, null);
                ddlLOB.SelectedValue = dSet.Tables[0].Rows[0]["LOB_ID"].ToString();
                ddlLOB_SelectedIndexChanged(null, null);
                ddlBranchList.SelectedValue = dSet.Tables[0].Rows[0]["LocationID"].ToString();
                ddlBranch_SelectedIndexChanged(null, null);
                txtFinanceAmount.Text = dSet.Tables[0].Rows[0]["Finance_Amount"].ToString();
                txtTenure.Text = dSet.Tables[0].Rows[0]["Tenure"].ToString();
                ddlTenureType.SelectedValue = "134";

                if (dSet.Tables[1].Rows.Count == 0)
                {
                    dSet.Tables[1].Rows.Add();
                    dSet.Tables[1].Columns.Add("AssetValue", typeof(decimal));
                    gvAssetDetails.DataSource = dSet.Tables[1];
                    gvAssetDetails.DataBind();

                    gvAssetDetails.Rows[0].Cells.Clear();
                    gvAssetDetails.Rows[0].Visible = false;
                    gvAssetDetails.Visible = false;
                    dSet.Tables[1].Rows.Clear();
                }
                else
                {
                    gvAssetDetails.DataSource = dSet.Tables[1];
                    gvAssetDetails.DataBind();
                    gvAssetDetails.Visible = true;
                    //gvAssetDetails.Columns[7].Visible = false;
                    Session["PricingAssetDetails"] = ViewState["ObjDTAssetDetails"] = dSet.Tables[1];
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriSetMaxLength_gvOutFlow()
    {
        if (gvOutFlow.FooterRow != null)
        {
            TextBox txtAmountOutflow = gvOutFlow.FooterRow.FindControl("txtAmount_Outflow") as TextBox;
            txtAmountOutflow.SetDecimalPrefixSuffix(10, 0, true, "Outflow Amount");
        }
    }

    private void FunPriSetMaxLength_gvInflow()
    {
        if (gvInflow.FooterRow != null)
        {
            TextBox txtAmountInflow = gvInflow.FooterRow.FindControl("txtAmount_Inflow") as TextBox;
            txtAmountInflow.SetDecimalPrefixSuffix(10, 0, true, "Inflow Amount");
        }
    }

    private void FunPriSetMaxLength_gvRepaymentDetails()
    {
        if (gvRepaymentDetails.FooterRow != null)
        {
            TextBox txtPerInstall = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            txtPerInstall.SetDecimalPrefixSuffix(10, 0, true, "Per Installment Amount");

            TextBox txtBreakPer = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;
            txtBreakPer.SetDecimalPrefixSuffix(2, 2, false, false, "Break up Percentage");

            TextBox ObjtxtFromInstallment = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            ObjtxtFromInstallment.SetDecimalPrefixSuffix(3, 0, true, false, "From Installment");

            TextBox ObjtxtToInstallment = gvRepaymentDetails.FooterRow.FindControl("txtToInstallment_RepayTab") as TextBox;
            ObjtxtToInstallment.SetDecimalPrefixSuffix(3, 0, true, false, "To Installment");
        }
    }

    private void FunPriSetMaxLength()
    {
        /*txtMarginMoneyPer_Cashflow.SetDecimalPrefixSuffix(2, 4, false, false, "Margin %");
        txtMarginMoneyAmount_Cashflow.CheckGPSLength(false, "Margin Amount");

        txtMarginPercentage.SetDecimalPrefixSuffix(2, 4, false, false, "Margin %");
        txtMarginAmountAsset.CheckGPSLength(false, "Margin Amount");

        txtRate.SetDecimalPrefixSuffix(5, 4, false, false, "Rate");

        txt_Margin_Percentage.SetDecimalPrefixSuffix(2, 4, false, false, "Margin %");

        txtResidualValue_Cashflow.SetDecimalPrefixSuffix(2, 2, false, false, "Residual Value");
        txtResidualAmt_Cashflow.CheckGPSLength(false, "Residual Amount");

        txtFacilityAmt.CheckGPSLength(true, "Facility Amount");
        txtUnitValue.SetDecimalPrefixSuffix(10, 2, true, "Unit Value");

        //txtCompanyIRR.SetDecimalPrefixSuffix(10, 4, true);
        //txtCompanyIRR_Repay.SetDecimalPrefixSuffix(10, 4, true);

        //txtBusinessIRR.SetDecimalPrefixSuffix(10, 4, true);
        //txtBusinessIRR_Repay.SetDecimalPrefixSuffix(10, 4, true);

        //txtAccIRR.SetDecimalPrefixSuffix(10, 4, true);
        //txtAccountIRR_Repay.SetDecimalPrefixSuffix(10, 4, true);

        Button btnAdd_OutFlow = gvOutFlow.FooterRow.FindControl("btnAddOut") as Button;
        btnAdd_OutFlow.Attributes.Add("onclick", "FunChkAllFooterValues(" + gvOutFlow.ClientID + ");");

        Button btnAdd_Inflow = gvInflow.FooterRow.FindControl("btnAdd") as Button;
        btnAdd_Inflow.Attributes.Add("onclick", "FunChkAllFooterValues(" + gvInflow.ClientID + ");");*/

        FunPriSetMaxLength_gvOutFlow();
        FunPriSetMaxLength_gvInflow();
        FunPriSetMaxLength_gvRepaymentDetails();
        FunPriSetMaxLength_gvGuarantor();
    }

    private void FunPriSetMaxLength_gvGuarantor()
    {
        if (gvGuarantor.FooterRow != null)
        {
            TextBox txtGuaranteeamount_GuarantorTab3 = gvGuarantor.FooterRow.FindControl("txtGuaranteeamount_GuarantorTab_Footer") as TextBox;
            txtGuaranteeamount_GuarantorTab3.SetDecimalPrefixSuffix(10, 0, true, "Guarantee Amount");
            //txtGuaranteeamount_GuarantorTab3.Attributes.Add("onkeypress", "fnAllowNumbersOnly('true','false','" + txtGuaranteeamount_GuarantorTab3.ClientID + "')");
        }
    }
    private void PreparePageForWFLoad()
    {
        WorkFlowSession WFSessionValues = new WorkFlowSession();

        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@EnquiryNo", WFSessionValues.WorkFlowDocumentNo);
        objProcedureParameter.Add("@CompanyId", intCompanyId.ToString());
        if (!string.IsNullOrEmpty(WFSessionValues.Document_Type.ToString()))
            objProcedureParameter.Add("@Document_Type", WFSessionValues.Document_Type.ToString());
        DataSet DS = new DataSet();
        DS = Utility.GetDataset("S3G_WORKFLOW_LoadApplication", objProcedureParameter);
        if (DS.Tables.Count > 0)
        {
            if (DS.Tables.Count > 1)
            {
                if (DS.Tables[1].Rows.Count > 0)
                {
                    intApplicationProcessId = Convert.ToInt32(DS.Tables[1].Rows[0]["Doc_Id"].ToString());
                    ViewState["intApplicationProcessId"] = intApplicationProcessId;
                    FunPriDisableControls(1);
                }
            }
            else
            {
                if (DS.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DS.Tables[0].Rows[0]["Doc_Id"].ToString()))
                        ddlBusinessOfferNoList.SelectedValue = DS.Tables[0].Rows[0]["Doc_Id"].ToString();
                }
                else//Enquiry or Pricing start point
                {
                    DataTable BusinessOffers = (DataTable)ddlBusinessOfferNoList.DataSource;
                    DataRow[] BusinessOfferRows = BusinessOffers.Select("Business_Offer_Number='" + WFSessionValues.WorkFlowDocumentNo + "'");
                    if (BusinessOfferRows.Length > 0)
                    {
                        ddlBusinessOfferNoList.SelectedValue = BusinessOfferRows[0]["Pricing_ID"].ToString();
                    }
                }
                FunPriToggleOfferNoBased();
            }

        }



        if (ddlLOB.SelectedIndex > 0) ddlLOB.ClearDropDownList();
        //if (ddlBranchList.SelectedIndex > 0) ddlBranchList.ClearDropDownList();
        ddlBranchList.ReadOnly = true;
        if (ddlBusinessOfferNoList.SelectedIndex > 0) ddlBusinessOfferNoList.ClearDropDownList();
        btnClear.Enabled = false;
    }

    private decimal FunPriGetMarginAmout()
    {
        decimal decMarginAmount;


        if (txt_Margin_Percentage.Text != "" && txtFinanceAmount.Text != "")
        {
            //if (txtMarginMoneyAmount_Cashflow.Text != "")
            //{
            //    decMarginAmount = ((Convert.ToDecimal(txtFinanceAmount.Text) + Convert.ToDecimal(txtMarginMoneyAmount_Cashflow.Text)) * (Convert.ToDecimal(txt_Margin_Percentage.Text) / 100));
            //}
            //else
            //{
            if (Session["PricingAssetDetails"] != null)
            {
                decimal dcmTotalAssetValue = Convert.ToDecimal(((DataTable)Session["PricingAssetDetails"]).Compute("Sum(TotalAssetValue)", "Noof_Units > 0"));

                decMarginAmount = (dcmTotalAssetValue * (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) / 100));
            }
            else
            {
                decMarginAmount = (Convert.ToDecimal(txtFinanceAmount.Text) * (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) / 100));
            }
            //}

        }
        else
        {
            decMarginAmount = 0;
        }


        return decMarginAmount;
    }

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
                    ViewState["mode"] = strAddMode;
                    intApplicationProcessId = 0;
                    FunPriSetInitialSettings();
                    FunPriFillMainPageDLL();
                    FunPriInitializeControls();
                    FunProGetIRRDetails();
                    TabContainerMainTab.ActiveTabIndex = 0;
                    btnApplicationCancel.Visible = false;
                    btnPrint.Enabled = false;
                    lblRoundNumber.Visible = lblRoundNo.Visible = false;
                    break;

                case 1: // Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    rfvtxtPassword.Enabled = false;
                    //lblRoundNumber.Visible = lblRoundNo.Visible = true;
                    FunPriLoadApplicationDetails();
                    FunPriSetInitialSettings();
                    if (ddlBusinessOfferNoList.SelectedValue != "-1")
                        ddlROIRuleList.ClearDropDownList();
                    TabContainerAP.ActiveTabIndex = 0;
                    btnClear.Enabled = false;
                    ucCustomerCodeLov.ButtonEnabled = false;
                    if (!txtStatus.Text.ToUpper().Contains("APP") && !txtStatus.Text.ToUpper().Contains("REJ") && !txtStatus.Text.ToUpper().Contains("CAN"))
                    {
                        btnApplicationCancel.Visible = true;
                    }
                    else
                    {
                        btnApplicationCancel.Visible = false;
                    }
                    FunPriToggleModeControls();
                    TabContainerMainTab.ActiveTabIndex = 0;
                    //btnPrint.Enabled = true;
                    rfvtxtPassword.Enabled = false;
                    btnCreateCustomer.Visible = false;
                    rfvBusinessOfferNo.Enabled = false;
                    if (ddlBusinessOfferNoList.SelectedValue != "-1")
                    {
                        if (gvAssetDetails.Rows.Count > 0)
                        {
                            gvAssetDetails.Columns[7].Visible = false;
                        }
                    }
                    else
                    {
                        FunPriAssignAssetLink();
                    }
                    txtMLAFinanceAmount.ReadOnly = true;
                    //added by saranya
                    if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") ||
                        ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") ||
                        ((ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") ||
                        ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TL")) &&
                        ddl_Repayment_Mode.SelectedItem.Text.ToUpper().StartsWith("PRO")))

                        TabContainerAP.Tabs[2].Enabled = false;
                    else
                        TabContainerAP.Tabs[2].Enabled = true;

                    //end

                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper() != "PRODUCT"))
                    {
                        grvRepayStructure.Columns[5].Visible = false;
                        //grvRepayStructure.Columns[6].Visible = true;
                        //grvRepayStructure.Columns[7].Visible = true;
                    }
                    else
                    {
                        grvRepayStructure.Columns[5].Visible = true;
                        grvRepayStructure.Columns[6].Visible = false;
                        grvRepayStructure.Columns[7].Visible = false;
                    }
                    break;


                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = false;
                    ucCustomerCodeLov.ButtonEnabled = false;
                    FunPriLoadApplicationDetails();
                    lblRoundNumber.Visible = lblRoundNo.Visible = false;
                    FunPriSetInitialSettings();
                    TabContainerAP.ActiveTabIndex = 0;
                    TabContainerMainTab.ActiveTabIndex = 0;
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }
                    if (bClearList)
                    {
                        ddlLOB.ClearDropDownList();
                        ddlBranchList.ReadOnly = true;
                        ddlBusinessOfferNoList.ClearDropDownList();
                        //ddlSalePersonCodeList.ClearDropDownList();
                        ddlTenureType.ClearDropDownList();
                        ddlROIRuleList.ClearDropDownList();
                        if (ddlPaymentRuleList.Items.Count > 0)
                        {
                            ddlPaymentRuleList.ClearDropDownList();
                        }

                    }
                    btnFetchPayment.Visible = btnFetchROI.Visible = false;
                    txtCustomerCode.ReadOnly = txtMLAFinanceAmount.ReadOnly = txtFBDate.ReadOnly =
                    txtTenure.ReadOnly = txtFinanceAmount.ReadOnly = txtMarginAmount.ReadOnly = txtResidualValue.ReadOnly = txtMarginMoneyAmount_Cashflow.ReadOnly =
                    txtMarginMoneyPer_Cashflow.ReadOnly = txtResidualAmt_Cashflow.ReadOnly = txtResidualValue_Cashflow.ReadOnly = true;
                    ChkRefinanceContract.Enabled = CalendarExtenderED.Enabled = false;
                    gvInflow.FooterRow.Visible = gvInflow.Columns[8].Visible = false;
                    gvOutFlow.FooterRow.Visible = gvOutFlow.Columns[8].Visible = false;
                    if(!ddlLOB.SelectedItem.Text.ToString().Contains("PL"))
                    {
                                            gvRepaymentDetails.FooterRow.Visible = gvRepaymentDetails.Columns[10].Visible = false;
                    }
                        if (gvGuarantor.Rows.Count > 0)
                    
                    {
                        gvGuarantor.FooterRow.Visible = gvGuarantor.Columns[6].Visible = false;
                    }
                    gvAlert.FooterRow.Visible = gvAlert.Columns[6].Visible = false;
                    gvFollowUp.FooterRow.Visible = gvFollowUp.Columns[9].Visible = false;
                    if (gvMoratorium.Rows.Count > 0)
                    {
                        gvMoratorium.FooterRow.Visible = gvMoratorium.Columns[4].Visible = false;
                    }
                    btnCalIRR.Enabled = btnReset.Enabled = btnClear.Enabled = false;
                    btnApplicationCancel.Visible = btnAddAsset.Visible = false;
                    if (gvAssetDetails.Rows.Count > 0)
                    {
                        gvAssetDetails.Columns[7].Visible = false;
                    }
                    ddlDoyouWant_MLA.ClearDropDownList();
                    btnCreateCustomer.Visible = rfvtxtPassword.Enabled =
                    CalendarExtenderED.Enabled = btnConfigure.Enabled = false;
                    //btnPrint.Enabled = true;
                    btnAddAsset.Visible = false;
                    btnCreateCustomer.Visible = false; txtPassword.ReadOnly =
                    txtMLANo.ReadOnly = txtValidFrom_MLA.ReadOnly = txtValidTo_MLA.ReadOnly = true;

                    ddl_Frequency.Enabled = txt_Margin_Percentage.Enabled = ddl_Time_Value.Enabled =
                    ddl_Insurance.Enabled =
                    txtRate.Enabled = false;
                    //added by saranya
                    if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") ||
                        ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") ||
                        ((ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") ||
                        ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TL")) &&
                        ddl_Repayment_Mode.SelectedItem.Text.ToUpper().StartsWith("PRO")))

                        TabContainerAP.Tabs[2].Enabled = false;
                    else
                        TabContainerAP.Tabs[2].Enabled = true;
                    //end
                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper() != "PRODUCT"))
                    {
                        grvRepayStructure.Columns[5].Visible = false;
                        //grvRepayStructure.Columns[6].Visible = true;
                        //grvRepayStructure.Columns[7].Visible = true;
                    }
                    else
                    {
                        grvRepayStructure.Columns[5].Visible = true;
                        grvRepayStructure.Columns[6].Visible = false;
                        grvRepayStructure.Columns[7].Visible = false;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPubQueryExistCustomerListEnquiryUpdation(int CustomerID)
    {

        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@CustomerID", CustomerID.ToString());
        objProcedureParameter.Add("@CompanyID", intCompanyId.ToString());
        DataSet dsCustomer = Utility.GetDataset("S3G_Get_Exist_Customer_Details_Enquiry_Updation", objProcedureParameter);
        hdnConstitutionId.Value = dsCustomer.Tables[0].Rows[0]["Constitution_Id"].ToString();
        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        txtName.Text = txtCustomerCode.Text = dsCustomer.Tables[0].Rows[0]["Customer_Code"].ToString();
        S3GCustomerAddress1.SetCustomerDetails(dsCustomer.Tables[0].Rows[0]["Customer_Code"].ToString(),
                dsCustomer.Tables[0].Rows[0]["comm_Address1"].ToString() + "\n" +
         dsCustomer.Tables[0].Rows[0]["comm_Address2"].ToString() + "\n" +
        dsCustomer.Tables[0].Rows[0]["comm_city"].ToString() + "\n" +
        dsCustomer.Tables[0].Rows[0]["comm_state"].ToString() + "\n" +
        dsCustomer.Tables[0].Rows[0]["comm_country"].ToString() + "\n" +
        dsCustomer.Tables[0].Rows[0]["comm_pincode"].ToString(), dsCustomer.Tables[0].Rows[0]["Customer_Name"].ToString(), dsCustomer.Tables[0].Rows[0]["Comm_Telephone"].ToString(),
        dsCustomer.Tables[0].Rows[0]["Comm_mobile"].ToString(),
        dsCustomer.Tables[0].Rows[0]["comm_email"].ToString(), dsCustomer.Tables[0].Rows[0]["comm_website"].ToString());
        txtConstitution.Text = dsCustomer.Tables[0].Rows[0]["Constitution"].ToString();

        FunPriLoadConsitutionBasedCustomer(CustomerID);
        Session["AssetCustomer"] = hdnCustID.Value + ";" + S3GCustomerAddress1.CustomerName;

    }

    private void FunPriToggleCustomerCodeBased()
    {
        try
        {
            S3GCustomerAddress1.SetCustomerDetails("", "", "", "", "", "", "");
            Session["AssetCustomer"] = "";
            DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["CustomerDT"];
            if (dt.Rows.Count > 0 && txtCustomerCode.Text != "")
            {

                string[] strSplit = txtCustomerCode.Text.Split('-');
                if (strSplit.Length == 1)
                {
                    Utility.FunShowAlertMsg(this, "Customer Code doesnot Exist");
                    S3GCustomerAddress1.SetCustomerDetails(txtCustomerCode.Text, "", "", "", "", "", "");
                    txtConstitution.Text = "";
                    grvConsDocuments.DataSource = null;
                    grvConsDocuments.DataBind();
                    return;
                }
                strSplit[1] = strSplit[1].ToString() + "-" + strSplit[2].ToString();
                string filterExpression = "Customer_Code = '" + strSplit[1].ToString() + "'";
                DataRow[] dtSuggestions = dt.Select(filterExpression);
                if (dtSuggestions.Length > 0)
                {
                    txtCustomerCode.Text = strSplit[1].ToString();
                    strCustomerId = dtSuggestions[0]["Customer_ID"].ToString();
                    hdnCustID.Value = dtSuggestions[0]["Customer_ID"].ToString();
                    strCustomerValue = dtSuggestions[0]["Customer_Code"].ToString();
                    strCustomerName = dtSuggestions[0]["Customer_Name"].ToString();
                    FunPubQueryExistCustomerListEnquiryUpdation(Convert.ToInt32(strCustomerId));
                    Dictionary<string, string> Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Is_Active", "1");
                    Procparam.Add("@User_Id", intUserId.ToString());
                    Procparam.Add("@Company_ID", intCompanyId.ToString());
                    Procparam.Add("@Consitution_Id", hdnConstitutionId.Value);
                    ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                    Session["AssetCustomer"] = strCustomerId + ";" + strCustomerName;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriInitializeControls()
    {
        try
        {
            FunPriFillROIDLL(strAddMode);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriValidateApplicationStart()
    {
        try
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@COMPANYID", intCompanyId.ToString());
            objProcedureParameter.Add("@PROGRAMID", "38");
            DataSet dsGlobalStartPoint = Utility.GetDataset("S3G_ORG_GETSTARTINGPOINT", objProcedureParameter);
            if (dsGlobalStartPoint.Tables[0].Rows.Count > 0)
            {
                bool blnIsStartUp = Convert.ToBoolean(dsGlobalStartPoint.Tables[0].Rows[0]["APPLICATION_NUMBER"].ToString());
                if (blnIsStartUp)
                {
                    txtCustomerCode.ReadOnly = false;
                    btnCreateCustomer.Visible = true;
                    rfvcmbCustomer.Enabled = true;
                    rfvBusinessOfferNo.Enabled = false;
                    lblCustomerName.CssClass = "styleReqFieldLabel";
                    lblBusinessOfferNo.CssClass = "styleDisplayLabel";
                    if (dsGlobalStartPoint.Tables[1].Rows.Count > 0)
                    {
                        bool blnIsAllowROIModify = Convert.ToBoolean(dsGlobalStartPoint.Tables[1].Rows[0]["IS_PROGRAM"].ToString());
                        ViewState["PricingROIRuleModify"] = blnIsAllowROIModify;
                    }
                }
                else
                {
                    txtCustomerCode.ReadOnly = true;
                    btnCreateCustomer.Visible = false;
                    lblCustomerName.CssClass = "styleDisplayLabel";
                    lblBusinessOfferNo.CssClass = "styleReqFieldLabel";
                    rfvcmbCustomer.Enabled = false;
                    rfvBusinessOfferNo.Enabled = true;
                    if (dsGlobalStartPoint.Tables[1].Rows.Count > 0)
                    {
                        bool blnIsAllowROIModify = Convert.ToBoolean(dsGlobalStartPoint.Tables[1].Rows[0]["IS_PROGRAM"].ToString());
                        ViewState["PricingROIRuleModify"] = blnIsAllowROIModify;
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Define ROI Rule Modification in GlobalParameter setup");
                        return;
                    }
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Define startup-screen in GlobalParameter setup");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriSetInitialSettings()
    {
        try
        {
            txtMLANo.Attributes.Add("readonly", "readonly");
            txtValidFrom_MLA.Attributes.Add("readonly", "readonly");
            //txtValidTo_MLA.Attributes.Add("readonly", "readonly");
            CalendarExtenderED.Format = strDateFormat;
            if (intApplicationProcessId == 0)
            {
                txtDate.Text = DateTime.Now.Date.ToString(strDateFormat);
                txtStatus.Text = "Pending";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriClearForms()
    {
        try
        {
            ViewState["mode"] = strAddMode;

            #region Main Tab

            ChkRefinanceContract.Checked = false;
            FunPriClearGrid(grvConsDocuments);
            FunPriClearGrid(gvPRDDT);
            FunPriClearGrid(gvAssetDetails);
            txtApplicationNo.Text = txtDate.Text = txtStatus.Text =
            txtValidFrom_MLA.Text = txtDate.Text = txtBusinessIRR.Text = txtCompanyIRR.Text = txtAccountingIRR.Text =
            txtFinanceAmount.Text = txtTenure.Text = txtMarginAmount.Text = txtResidualValue.Text = string.Empty;
            #endregion

            #region Offer Terms
            FunPriClearGrid(gvPaymentRuleDetails);

            chk_lblResidual_Value.Checked = false;
            chk_lblMargin.Checked = false;
            txt_Model_Description.Text = txt_ROI_Rule_Number.Text = txt_Recovery_Pattern_Year1.Text =
            txt_Recovery_Pattern_Year2.Text = txt_Recovery_Pattern_Year3.Text = txt_Recovery_Pattern_Rest.Text =
            txt_Margin_Percentage.Text = txtResidualValue_Cashflow.Text = txtResidualAmt_Cashflow.Text = txtMarginMoneyPer_Cashflow.Text =
            txtMarginMoneyAmount_Cashflow.Text = string.Empty;

            FunPriClearGrid(gvOutFlow);
            FunPriClearGrid(gvInflow);

            #endregion

            #region Repayment
            txtAccountIRR_Repay.Text = txtCompanyIRR_Repay.Text = txtBusinessIRR_Repay.Text = string.Empty;
            FunPriClearGrid(gvRepaymentDetails);
            #endregion

            #region GuaranteeInvoice Details
            FunPriClearGrid(gvCollateralDetails);
            FunPriClearGrid(gvGuarantor);
            FunPriClearGrid(gvInvoiceDetails);
            #endregion

            #region Alert
            FunPriClearGrid(gvAlert);
            #endregion

            #region Followup
            FunPriClearGrid(gvFollowUp);
            txtApplication_Followup.Text = txtOfferNo_Followup.Text = txtCustNameAdd_Followup.Text = txtEnquiryDate_Followup.Text =
            txtEnquiry_Followup.Text = txtBranch_Followup.Text = txtLOB_Followup.Text = string.Empty;
            #endregion

            #region MLA/SLA Details
            FunPriClearGrid(gv_MLARepayRuleCard);
            FunPriClearGrid(gv_MLAROI);
            txtPassword.Text = txtMLANo.Text = txtValidTo_MLA.Text = txtROIMLA.Text = txtPaymentCardMLA.Text = string.Empty;
            ddlDoyouWant_MLA.Enabled = true;
            #endregion

            #region Moratorium Details
            FunPriClearGrid(gvMoratorium);
            #endregion

            #region Clear view state
            txtStatus.Text = "Pending";

            intApplicationProcessId = 0;
            #endregion

            FunPriInitializeControls();


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriClearGrid(GridView Gv)
    {
        try
        {
            Gv.Dispose();
            Gv.DataSource = null;
            Gv.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriSetDDLFirstitem(DropDownList DDl)
    {
        try
        {
            DDl.SelectedValue = "-1";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriSetDDLFirstitem(AjaxControlToolkit.ComboBox DDl)
    {
        try
        {
            DDl.SelectedValue = "-1";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private bool FunPriDisableValueField(string str)
    {
        string[] strsp = new string[2];
        strsp = str.Split('-');

        if (strsp[0].ToString() != "CID")
            return false;
        else
            return true;

    }

    private void FunPriSetWhiteSpaceDLL(DropDownList ObjDLL)
    {
        try
        {
            if (ObjDLL.Items.Count == 0)
            {
                ListItem liSelect = new ListItem("", "-1");
                ObjDLL.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriMLASLAApplicable(int intLobId)
    {
        ContractMgtServicesReference.ContractMgtServicesClient objContractMgtserviceClient = new ContractMgtServicesReference.ContractMgtServicesClient();
        try
        {
            byte[] ObjPricingDataTable = objContractMgtserviceClient.FunPubGetMLASLAApplicable(Convert.ToInt32(ddlLOB.SelectedValue), intCompanyId);

            DataSet dsMLASLAApplicable = (DataSet)ClsPubSerialize.DeSerialize(ObjPricingDataTable, SerializationMode.Binary, typeof(DataSet));
            if (dsMLASLAApplicable.Tables.Count > 0)
            {
                if (dsMLASLAApplicable.Tables[0].Rows.Count > 0)
                {
                    string strMLASLAApplicable = dsMLASLAApplicable.Tables[0].Rows[0][0].ToString();
                    ViewState["strMLASLAApplicable"] = strMLASLAApplicable;
                    if (strMLASLAApplicable == "False")
                    {
                        TabContainerAP.Tabs[6].Visible = false;
                        rfvtxtPassword.Enabled = false;
                        rfvtxtValidTo_MLA.Enabled = false;
                    }
                    else
                    {
                        TabContainerAP.Tabs[6].Visible = true;
                        rfvtxtPassword.Enabled = true;
                        rfvtxtValidTo_MLA.Enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objContractMgtserviceClient.Close();
        }
    }

    private void FunPriLOBBasedvalidations(string strLobName, string strLobId, string strTMode, string StrRuleID)
    {
        try
        {

            if (strTMode == strAddMode)
            {
                objProcedureParameter = new Dictionary<string, string>();

                if (strMode.ToUpper().Trim() != "Q")
                    objProcedureParameter.Add("@Is_Active", "1");

                if (strMode.ToUpper().Trim() == "M" && StrRuleID != "")
                    objProcedureParameter.Add("@Rules_ID", StrRuleID);

                objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());
                objProcedureParameter.Add("@LOB_ID", strLobId);
                ddlProductCodeList.BindDataTable(SPNames.SYS_ProductMaster, objProcedureParameter, new string[] { "Product_ID", "Product_Code", "Product_Name" });
                objProcedureParameter.Add("@Option", "7");
                ddlROIRuleList.BindDataTable(SPNames.S3G_ORG_GetPricing_List, objProcedureParameter, new string[] { "ROI_Rules_ID", "ROI_Rule_Number", "Model_Description" });
                txtLOB_Followup.Text = strLobName;
                hdnROIRule.Value = "";
                if (ddlBusinessOfferNoList.SelectedIndex == 0)
                {
                    FunPriLoadOfferNo();
                }
            }
            if (strTMode == strEditMode)
            {
                objProcedureParameter = new Dictionary<string, string>();
                objProcedureParameter.Add("@Is_Active", "1");
                objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());
                objProcedureParameter.Add("@LOB_ID", strLobId);
                objProcedureParameter.Add("@Option", "13");
                ddlConstitutionCodeList.BindDataTable(SPNames.S3G_ORG_GetPricing_List, objProcedureParameter, new string[] { "Constitution_ID", "Constitution_Code", "Constitution_Name" });
                txtLOB_Followup.Text = strLobName;
                if (ddlConstitutionCodeList.Items.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Constitution List", "alert('Define the Constitution for the selected Line of business');", true);
                }
            }

            DataTable dtPLR = (DataTable)ViewState["IRRDetails"];
            dtPLR.DefaultView.RowFilter = "LOB_ID = " + strLobId;
            dtPLR = dtPLR.DefaultView.ToTable();
            if (dtPLR.Rows.Count > 0)
            {
                hdnCTR.Value = dtPLR.Rows[0]["Corporate_Tax_Rate"].ToString();
                hdnPLR.Value = dtPLR.Rows[0]["Prime_Lending_Rate"].ToString();
                ViewState["hdnRoundOff"] = dtPLR.Rows[0]["Roundoff"].ToString();
            }
            FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLOBBasedvalidations(string strType)
    {
        try
        {
            strType = strType.Split('-')[0].Trim();
            switch (strType.ToLower())
            {
                case "ol":  //Operating lease
                    TabContainerAP.Tabs[2].Visible = true;
                    break;


                case "te": //Term loan Extensible
                case "tl": //Term loan
                case "ft": //Factoring 
                    TabContainerAP.Tabs[2].Visible = true;
                    break;
                default://for default case
                    TabContainerAP.Tabs[2].Visible = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriBindPaymentDDL(string StrRuleID)
    {
        try
        {
            objProcedureParameter = new Dictionary<string, string>();
            if (strMode.ToUpper().Trim() != "Q")
                objProcedureParameter.Add("@Is_Active", "1");

            if (strMode.ToUpper().Trim() == "M" && StrRuleID != "")
                objProcedureParameter.Add("@Rules_ID", StrRuleID);

            objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());
            objProcedureParameter.Add("@LOB_ID", ddlLOB.SelectedItem.Value);
            objProcedureParameter.Add("@Product_ID", ddlProductCodeList.SelectedItem.Value);
            objProcedureParameter.Add("@Option", "8");
            ddlPaymentRuleList.BindDataTable(SPNames.S3G_ORG_GetPricing_List, objProcedureParameter, new string[] { "Payment_RuleCard_ID", "Payment_Rule_Number" });
            hdnPayment.Value = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Payment Rule");
        }
    }

    private void FunPubGetPricingDetails(int intPricingId)
    {

        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());
        objProcedureParameter.Add("@Pricing_ID", intPricingId.ToString());
        try
        {
            DataSet ds_PricingDetails = Utility.GetDataset("S3G_ORG_GetPricingDetails_App", objProcedureParameter);
            if (ds_PricingDetails != null)
            {
                if (ds_PricingDetails.Tables[0].Rows[0]["Status_ID"].ToString() != "44")
                {
                    strMode = "Q";
                }
                #region MainPage Tab
                ddlLOB.Items.Clear();
                ddlProductCodeList.Items.Clear();
                //ddlBranchList.Items.Clear();
                ddlBranchList.Clear();
                txtOfferDate.Text = DateTime.Parse(ds_PricingDetails.Tables[0].Rows[0]["Offer_Date"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
                txtEnquiry_Followup.Text = ds_PricingDetails.Tables[0].Rows[0]["Enquiry_No"].ToString();
                txtOfferNo_Followup.Text = ds_PricingDetails.Tables[0].Rows[0]["Business_Offer_Number"].ToString();
                FunPriLoadTenureType();
                txtFinanceAmount.Text = ds_PricingDetails.Tables[0].Rows[0]["Facility_Amount"].ToString();

                txtTenure.Text = ds_PricingDetails.Tables[0].Rows[0]["Tenure"].ToString();
                ddlTenureType.SelectedValue = ds_PricingDetails.Tables[0].Rows[0]["Tenure_Type"].ToString();

                ListItem lstItem;
                lstItem = new ListItem(ds_PricingDetails.Tables[0].Rows[0]["LOB"].ToString(), ds_PricingDetails.Tables[0].Rows[0]["LOB_ID"].ToString());
                ddlLOB.Items.Add(lstItem);
                FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedValue, strAddMode,
                    ds_PricingDetails.Tables[3].Rows[0]["ROI_Rules_ID"].ToString());
                if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
                {
                    rfvddlPaymentRuleList.Enabled = false;
                }
                else
                {
                    rfvddlPaymentRuleList.Enabled = true;
                }
                //lstItem = new ListItem(ds_PricingDetails.Tables[0].Rows[0]["PRODUCT"].ToString(), ds_PricingDetails.Tables[0].Rows[0]["PRODUCT_ID"].ToString());
                //ddlProductCodeList.Items.Add(lstItem);
                ddlProductCodeList.SelectedValue = ds_PricingDetails.Tables[0].Rows[0]["PRODUCT_ID"].ToString();
                ddlProductCodeList.ClearDropDownList();

                //lstItem = new ListItem(ds_PricingDetails.Tables[0].Rows[0]["Location"].ToString(), ds_PricingDetails.Tables[0].Rows[0]["Location_ID"].ToString());
                //ddlBranchList.Items.Add(lstItem);
                ddlBranchList.SelectedText = ds_PricingDetails.Tables[0].Rows[0]["Location"].ToString();
                ddlBranchList.SelectedValue = ds_PricingDetails.Tables[0].Rows[0]["Location_ID"].ToString();

                S3GCustomerAddress1.SetCustomerDetails(ds_PricingDetails.Tables[0].Rows[0]["Customer_Code"].ToString(),
                    ds_PricingDetails.Tables[0].Rows[0]["comm_Address1"].ToString() + "\n" +
                    ds_PricingDetails.Tables[0].Rows[0]["comm_Address2"].ToString() + "\n" +
                    ds_PricingDetails.Tables[0].Rows[0]["comm_city"].ToString() + "\n" +
                    ds_PricingDetails.Tables[0].Rows[0]["comm_state"].ToString() + "\n" +
                    ds_PricingDetails.Tables[0].Rows[0]["comm_country"].ToString() + "\n" +
                    ds_PricingDetails.Tables[0].Rows[0]["comm_pincode"].ToString(), ds_PricingDetails.Tables[0].Rows[0]["Customer_Name"].ToString(),
                    ds_PricingDetails.Tables[0].Rows[0]["Comm_Telephone"].ToString(),
                    ds_PricingDetails.Tables[0].Rows[0]["Comm_mobile"].ToString(),
                    ds_PricingDetails.Tables[0].Rows[0]["comm_email"].ToString(), ds_PricingDetails.Tables[0].Rows[0]["comm_website"].ToString());

                ViewState["GuarantorCustomer"] = hdnCustID.Value = ds_PricingDetails.Tables[0].Rows[0]["Customer_ID"].ToString();

                TextBox txtName = ucCustomerCodeLov.FindControl("txtName") as TextBox;
                txtName.Text = txtCustomerCode.Text = ds_PricingDetails.Tables[0].Rows[0]["Customer_Code"].ToString();
                Session["AssetCustomer"] = hdnCustID.Value + ";" + S3GCustomerAddress1.CustomerName;
                txtAccountIRR_Repay.Text = txtAccountingIRR.Text = ds_PricingDetails.Tables[0].Rows[0]["Accounting_IRR"].ToString();
                txtBusinessIRR.Text = ds_PricingDetails.Tables[0].Rows[0]["Business_IRR"].ToString();
                txtBusinessIRR_Repay.Text = "";
                txtCompanyIRR_Repay.Text = txtCompanyIRR.Text = ds_PricingDetails.Tables[0].Rows[0]["Company_IRR"].ToString();
                FunPriMLASLAApplicable(Convert.ToInt32(ddlLOB.SelectedValue));
                if (ds_PricingDetails.Tables[0].Rows[0]["offer_margin_amount"].ToString() != "0")
                    txtMarginAmount.Text = ds_PricingDetails.Tables[0].Rows[0]["offer_margin_amount"].ToString();
                if (ds_PricingDetails.Tables[0].Rows[0]["Offer_Residual_Value_Amount"].ToString() != "0")
                    txtResidualValue.Text = ds_PricingDetails.Tables[0].Rows[0]["Offer_Residual_Value_Amount"].ToString();
                #endregion

                #region Document Details Tab
                ddlConstitutionCodeList.SelectedValue = ds_PricingDetails.Tables[0].Rows[0]["Constitution_Id"].ToString();
                hdnConstitutionId.Value = ds_PricingDetails.Tables[0].Rows[0]["Constitution_Id"].ToString();
                txtConstitution.Text = ds_PricingDetails.Tables[0].Rows[0]["Consitution"].ToString();
                grvConsDocuments.DataSource = ds_PricingDetails.Tables[12];
                grvConsDocuments.DataBind();

                #endregion

                #region OfferTerms Tab
                //lstItem = new ListItem(ds_PricingDetails.Tables[3].Rows[0]["ROI_Number"].ToString(), ds_PricingDetails.Tables[3].Rows[0]["ROI_Rules_ID"].ToString());
                //ddlROIRuleList.Items.Add(lstItem);
                //ddlROIRuleList.ClearDropDownList();

                ddlROIRuleList.SelectedValue = ds_PricingDetails.Tables[3].Rows[0]["ROI_Rules_ID"].ToString();


                //if(ds_PricingDetails.Tables[3].Rows[0]["Residual_Value"].ToString() != "0" )
                //{
                //    txtResidualValue_Cashflow.Text = ds_PricingDetails.Tables[3].Rows[0]["Residual_Value"].ToString() ;
                //}
                //else
                //{
                //    txtResidualValue_Cashflow.Text = string.Empty;
                //}

                if (!Convert.ToBoolean(ViewState["PricingROIRuleModify"]))
                {
                    ddlROIRuleList.RemoveDropDownList();
                }
                ViewState["ROIRules"] = ds_PricingDetails.Tables[3];
                //lstItem = new ListItem(ds_PricingDetails.Tables[4].Rows[0]["Payment_Rule_Number"].ToString(), ds_PricingDetails.Tables[4].Rows[0]["Payment_RuleCard_ID"].ToString());
                //ddlPaymentRuleList.Items.Add(lstItem);
                if (ds_PricingDetails.Tables[4].Rows.Count > 0)
                {
                    FunPriBindPaymentDDL(ds_PricingDetails.Tables[4].Rows[0]["Payment_RuleCard_ID"].ToString());
                }
                else
                {
                    FunPriBindPaymentDDL("");
                }
                if (ds_PricingDetails.Tables[4].Rows.Count > 0)
                {
                    ddlPaymentRuleList.SelectedValue = ds_PricingDetails.Tables[4].Rows[0]["Payment_RuleCard_ID"].ToString();
                    ddlPaymentRuleList.ClearDropDownList();
                }
                FunPriFillROIDLL(strAddMode);
                FunPriLoadROIRuleDetails(strEditMode);


                if (ds_PricingDetails.Tables[1].Rows.Count > 0)
                {
                    gvInflow.DataSource = ds_PricingDetails.Tables[1];
                    gvInflow.DataBind();
                    ViewState["DtCashFlow"] = ds_PricingDetails.Tables[1];
                    FunPriFillInflowDLL(strEditMode);
                }
                else
                {
                    ViewState["DtCashFlow"] = ds_PricingDetails.Tables[1];
                    FunPriFillInflowDLL(strAddMode);
                }

                if (ds_PricingDetails.Tables[2].Rows.Count > 0)
                {
                    gvOutFlow.DataSource = ds_PricingDetails.Tables[2];
                    gvOutFlow.DataBind();
                    ViewState["DtCashFlowOut"] = ds_PricingDetails.Tables[2];
                    FunPriFillOutflowDLL(strEditMode);
                    lblTotalOutFlowAmount.Text = ds_PricingDetails.Tables[9].Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    ViewState["DtCashFlowOut"] = ds_PricingDetails.Tables[2];
                    FunPriFillOutflowDLL(strAddMode);
                }

                if (ds_PricingDetails.Tables[4].Rows.Count > 0)
                {
                    FunPriLoadPaymentRuleDetails();
                }

                if (ds_PricingDetails.Tables[2].Rows.Count > 0)
                {
                    DataTable dtAcctype = ((DataTable)ViewState["PaymentRules"]);
                    dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";

                    DataRow[] drOutflowDate;

                    drOutflowDate = ds_PricingDetails.Tables[2].Select("CashFlow_Flag_ID = 41 and Date <= #" + Utility.StringToDate(txtDate.Text) + "#");
                    if (drOutflowDate.Length > 0)
                    {
                        if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().ToUpper() == "DEFERRED PAYMENT")
                        {
                            foreach (DataRow drrow in drOutflowDate)
                            {
                                drrow["Date"] = Utility.StringToDate(txtDate.Text).AddDays(1);
                                drrow.AcceptChanges();
                            }
                        }

                        if ((dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper() == "DEFERRED STRUCTURED") ||
                            (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper() == "PROJECT FINANCE") ||
                            (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().ToUpper() == "TRADE ADVANCE") ||
                            (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().ToUpper() == "NORMAL PAYMENT"))
                        {
                            foreach (DataRow drrow in drOutflowDate)
                            {
                                drrow["Date"] = Utility.StringToDate(txtDate.Text);
                                drrow.AcceptChanges();
                            }
                        }
                    }

                    gvOutFlow.DataSource = ds_PricingDetails.Tables[2];
                    gvOutFlow.DataBind();
                    ViewState["DtCashFlowOut"] = ds_PricingDetails.Tables[2];
                    FunPriFillOutflowDLL(strEditMode);
                    lblTotalOutFlowAmount.Text = ds_PricingDetails.Tables[9].Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    ViewState["DtCashFlowOut"] = ds_PricingDetails.Tables[2];
                    FunPriFillOutflowDLL(strAddMode);
                }

                #endregion

                #region Repayment Tab

                //DataRow[] drReapyDet = ds_PricingDetails.Tables[5].Select("FROMINSTALL = 1 and FROMDATE < #" +
                //    Utility.StringToDate(txtDate.Text) + "# and CASHFLOW_FLAG_ID = 23 ");
                //if (drReapyDet.Length > 0)
                //{
                //    foreach (DataRow drrow in drReapyDet)
                //    {
                //        drrow["FROMDATE"] = Utility.StringToDate(txtDate.Text);
                //        drrow.AcceptChanges();
                //    }
                DateTime DtNextInstallmentDate;
                DataRow[] drReapyDet = ds_PricingDetails.Tables[5].Select("FROMINSTALL >= 1");
                int intNoOfInstalment;
                foreach (DataRow drrow in drReapyDet)
                {
                    if (drrow["CASHFLOW_FLAG_ID"].ToString() != "23")
                    {
                        DataRow[] drRow;

                        drRow = ds_PricingDetails.Tables[5].Select("FROMINSTALL <='" + drrow["FROMINSTALL"].ToString() + "' and TOINSTALL >='" + drrow["TOINSTALL"].ToString() + "'");
                        if (drRow.Length == 0)
                        {
                            drRow = DtRepayGrid.Select("FROMINSTALL <='" + drrow["FROMINSTALL"].ToString() + "' or TOINSTALL >='" + drrow["TOINSTALL"].ToString() + "'");
                        }
                        int intNextInstall = Convert.ToInt32(drRow[0]["TOINSTALL"].ToString());
                        int intNoOfInstall = Convert.ToInt32(drrow["FROMINSTALL"].ToString()) - intNextInstall + 1;
                        DateTime dtDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedValue, Convert.ToDateTime(drRow[0]["ToDate"].ToString()), (intNoOfInstall - 1));
                        drrow["FROMDATE"] = dtDate;
                        drrow.AcceptChanges();
                    }

                    if (drrow["CASHFLOW_FLAG_ID"].ToString() == "23")
                    {
                        drrow["FROMDATE"] = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedValue, Utility.StringToDate(txtDate.Text), Convert.ToInt32(drrow["FROMINSTALL"].ToString()) - 1);
                    }
                    intNoOfInstalment = Convert.ToInt32(drrow["TOINSTALL"].ToString()) - Convert.ToInt32(drrow["FROMINSTALL"].ToString()) + 1;
                    DateTime dtTodate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedValue, Utility.StringToDate(drrow["FROMDATE"].ToString()), (intNoOfInstalment - 1));
                    drrow["TODATE"] = dtTodate;
                    DtNextInstallmentDate = dtTodate;
                    drrow.AcceptChanges();

                }

                //}
                gvRepaymentDetails.DataSource = ds_PricingDetails.Tables[5];
                gvRepaymentDetails.DataBind();
                ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
                txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
                ViewState["DtRepayGrid"] = ds_PricingDetails.Tables[5];
                if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
                {
                    ViewState["DtRepayGrid_TL"] = ds_PricingDetails.Tables[5];
                }

                //FunPriFillRepaymentDLL(strEditMode);
                FunPriGenerateNewRepayment();
                gvRepaymentDetails.FooterRow.Visible = true;

                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
                if (ds_PricingDetails.Tables[16].Rows.Count > 0)
                    ViewState["dtRepayDetailsOthers"] = ds_PricingDetails.Tables[16];
                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end

                FunPriCalculateSummary(ds_PricingDetails.Tables[5], "CashFlow", "TotalPeriodInstall");
                FunPriShowRepaymetDetails((decimal)ds_PricingDetails.Tables[5].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
                #endregion

                #region Alerts Tab

                if (ds_PricingDetails.Tables[6].Rows.Count > 0)
                {
                    gvAlert.DataSource = ds_PricingDetails.Tables[6];
                    gvAlert.DataBind();
                    FunPriFillAlertDLL(strEditMode);
                    ViewState["DtAlertDetails"] = ds_PricingDetails.Tables[6];
                }
                else
                {
                    FunPriFillAlertDLL(strAddMode);
                }



                #endregion

                #region Followup Tab
                txtEnquiry_Followup.Text = ds_PricingDetails.Tables[7].Rows[0]["Enquiry_Number"].ToString();
                txtOfferNo_Followup.Text = ds_PricingDetails.Tables[7].Rows[0]["Offer_Number"].ToString();
                txtApplication_Followup.Text = ds_PricingDetails.Tables[7].Rows[0]["Application_Number"].ToString();
                if (ds_PricingDetails.Tables[7].Rows[0]["Date"].ToString() != "" || ds_PricingDetails.Tables[7].Rows[0]["Date"].ToString() != string.Empty)
                    txtEnquiryDate_Followup.Text = Utility.StringToDate(ds_PricingDetails.Tables[7].Rows[0]["Date"].ToString()).ToString(strDateFormat);
                txtCustNameAdd_Followup.Text = S3GCustomerAddress1.CustomerAddress;
                txtBranch_Followup.Text = ds_PricingDetails.Tables[0].Rows[0]["Location"].ToString();
                txtLOB_Followup.Text = ds_PricingDetails.Tables[0].Rows[0]["LOB"].ToString();

                //Code added to populate Residual% from Pricing form For "OL"-BusinessOfferNo selection - Bug_ID - 6382 - Kuppusamy.B - May-31-2012
                txtResidualValue_Cashflow.Text = ds_PricingDetails.Tables[0].Rows[0]["OFFER_RESIDUAL_VALUE"].ToString();
                if (ds_PricingDetails.Tables[8].Rows.Count > 0)
                {
                    ViewState["DtFollowUp"] = ds_PricingDetails.Tables[8];
                    FunPriFillFollowupDLL(strEditMode);
                }
                else
                {
                    FunPriFillFollowupDLL(strAddMode);
                }





                #endregion

                #region Asset Tab
                //Commented by Saranya
                //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM"))
                if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT"))
                {
                    TabContainerMainTab.Tabs[1].Visible = false;
                }
                else
                {
                    TabContainerMainTab.Tabs[1].Visible = true;
                }
                if (TabContainerMainTab.Tabs[1].Visible)
                {
                    gvAssetDetails.Visible = true;
                    ViewState["ObjDTAssetDetails"] = Session["PricingAssetDetails"] = ds_PricingDetails.Tables[10];
                    FunProBindAssetGrid();
                }

                #endregion

                #region Toggle for BusinessOfferNo
                ddlTenureType.ClearDropDownList();
                txtTenure.ReadOnly = true;
                txtMarginAmount.Attributes.Add("readonly", "true");
                txtResidualValue.Attributes.Add("readonly", "true");
                ViewState["mode"] = strAddMode;
                FunPriFillGuarantorDLL();
                FunPriFillMoratoriumDLL();
                #endregion



                #region MLA/SLA
                if (TabContainerAP.Tabs[6].Visible)
                {
                    txtROIMLA.Text = ddlROIRuleList.SelectedItem.Text;
                    if (ddlPaymentRuleList.SelectedItem.Text != "--Select--")
                        txtPaymentCardMLA.Text = ddlPaymentRuleList.SelectedItem.Text;
                    txtMLAFinanceAmount.Text = ds_PricingDetails.Tables[0].Rows[0]["Facility_Amount"].ToString();
                }

                /* This need to be regenerate
                 * if (ds_PricingDetails.Tables[14].Rows.Count > 0)
                {
                    ViewState["RepaymentStructure"] = ds_PricingDetails.Tables[14];
                    grvRepayStructure.DataSource = ds_PricingDetails.Tables[14];
                    grvRepayStructure.DataBind();
                }*/
                grvRepayStructure.DataSource = null;
                grvRepayStructure.DataBind();

                if (ds_PricingDetails.Tables.Count > 14)
                {

                    if (ds_PricingDetails.Tables[15].Rows.Count > 0)
                    {
                        Panel8.Visible = true;
                        gvPRDDT.DataSource = ds_PricingDetails.Tables[15];
                        gvPRDDT.DataBind();
                    }

                }

                #endregion
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to fetch Pricing Details");
        }
    }

    //private bool FunPriIsValidConstitution()
    //{
    //    bool blnIsValid = true;

    //    foreach (GridViewRow gvRow in grvConsDocuments.Rows)
    //    { 
    //        CheckBox chkIsMandatory = (CheckBox)gvRow.FindControl("chkIsMandatory");
    //        CheckBox chkCollected = (CheckBox)gvRow.FindControl("chkCollected");
    //        if (chkCollected != null && chkIsMandatory != null)
    //        {
    //            if (chkIsMandatory.Checked && !chkCollected.Checked)
    //            {
    //                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Choose the Collected in Consitution Document Details";
    //                cvApplicationProcessing.IsValid = false;
    //                TabContainerMainTab.ActiveTabIndex = 0;
    //                return false;
    //            }
    //        }
    //        CheckBox chkIsNeedImageCopy = (CheckBox)gvRow.FindControl("chkIsNeedImageCopy");
    //        CheckBox chkScanned = (CheckBox)gvRow.FindControl("chkScanned");
    //        if (chkScanned != null && chkIsNeedImageCopy != null)
    //        {
    //            if (chkIsNeedImageCopy.Checked && !chkScanned.Checked)
    //            {
    //                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Choose the Scanned in Consitution Document Details";
    //                cvApplicationProcessing.IsValid = false;
    //                TabContainerMainTab.ActiveTabIndex = 0;
    //                return false;
    //            }
    //        }
    //    }
    //    return blnIsValid;
    //}

    private void FunPriLoadLObandBranch(int intUser_id, int intCompany_id)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        try
        {
            if (intApplicationProcessId == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_Id", intUser_id.ToString());
            Procparam.Add("@Company_ID", intCompany_id.ToString());
            Procparam.Add("@Program_Id", "38");
            Procparam.Add("@Consitution_Id", hdnConstitutionId.Value);
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            Procparam.Remove("@Consitution_Id");
            //ddlBranchList.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Line of Business / Location");
        }
    }

    
    private void FunPriSaveRecord()
    {
        ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjAProcessSave = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();

        try
        {
            string strApp_Number = string.Empty;
            S3GBusEntity.ApplicationProcess.ApplicationProcess ObjBusApplicationProcess = new S3GBusEntity.ApplicationProcess.ApplicationProcess();
            
            ObjBusApplicationProcess.Application_Process_ID = Convert.ToInt32(intApplicationProcessId);


            ObjBusApplicationProcess.Application_Number = txtApplicationNo.Text;
            ObjBusApplicationProcess.Date = Utility.StringToDate(txtDate.Text);
            ObjBusApplicationProcess.Status_ID = txtStatus.Text;
            ObjBusApplicationProcess.Branch_ID = Convert.ToInt32(ddlBranchList.SelectedValue);
            if (ddlBusinessOfferNoList.SelectedIndex > 0)
            {
                ObjBusApplicationProcess.Business_Offer_Number = ddlBusinessOfferNoList.SelectedValue;
            }
            if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                ObjBusApplicationProcess.Customer_ID = Convert.ToInt32(hdnCustID.Value);
            }
            ObjBusApplicationProcess.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjBusApplicationProcess.Product_ID = Convert.ToInt32(ddlProductCodeList.SelectedValue);

            if (!string.IsNullOrEmpty(txtBusinessIRR_Repay.Text))
                ObjBusApplicationProcess.Business_IRR = Convert.ToDecimal(txtBusinessIRR_Repay.Text);
            if (!string.IsNullOrEmpty(txtAccountIRR_Repay.Text))
                ObjBusApplicationProcess.Accounting_IRR = Convert.ToDecimal(txtAccountIRR_Repay.Text);
            if (!string.IsNullOrEmpty(txtCompanyIRR_Repay.Text))
                ObjBusApplicationProcess.Company_IRR = Convert.ToDecimal(txtCompanyIRR_Repay.Text);

            if (!string.IsNullOrEmpty(txtFinanceAmount.Text))
                ObjBusApplicationProcess.Finance_Amount = Convert.ToDecimal(txtFinanceAmount.Text);

            ObjBusApplicationProcess.Tenure = Convert.ToDecimal(txtTenure.Text);
            ObjBusApplicationProcess.Tenure_Type = Convert.ToInt32(ddlTenureType.SelectedValue);

            if (!string.IsNullOrEmpty(txtMarginAmount.Text))
                ObjBusApplicationProcess.Margin_Amount = Convert.ToDecimal(txtMarginAmount.Text);

            if (!string.IsNullOrEmpty(txtResidualValue.Text))
                ObjBusApplicationProcess.Residual_Value = Convert.ToDecimal(txtResidualValue.Text);

            ObjBusApplicationProcess.Refinance_Contract = Convert.ToInt32(ChkRefinanceContract.Checked);
            ObjBusApplicationProcess.Sales_Person_ID = Convert.ToInt32(ddlSalePersonCodeList.SelectedValue);
            
            if (TabContainerAP.Tabs[6].Visible)
            {
                if (ViewState["Password"] != null)
                {
                    txtPassword.Text = ViewState["Password"].ToString();
                }
                ObjBusApplicationProcess.MLA_Applicable = Convert.ToInt32(ddlDoyouWant_MLA.SelectedValue);
                ObjBusApplicationProcess.MLA_Validity_To = Utility.StringToDate(txtValidTo_MLA.Text).ToString();
            }
            ObjBusApplicationProcess.Company_ID = intCompanyId;
            ObjBusApplicationProcess.Created_By = intUserId;
            ObjBusApplicationProcess.Constitution_ID = Convert.ToInt32(hdnConstitutionId.Value);
            ObjBusApplicationProcess.Lease_Type = 1;
            ObjBusApplicationProcess.Payment_RuleCard_ID = Convert.ToInt32(ddlPaymentRuleList.SelectedValue);

            if (!string.IsNullOrEmpty(txtMarginMoneyPer_Cashflow.Text))
                ObjBusApplicationProcess.Offer_Margin = Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text);

            if (!string.IsNullOrEmpty(txtMarginMoneyAmount_Cashflow.Text))
                ObjBusApplicationProcess.Offer_Margin_Amount = Convert.ToDecimal(txtMarginMoneyAmount_Cashflow.Text);

            if (!string.IsNullOrEmpty(txtResidualValue_Cashflow.Text))
                ObjBusApplicationProcess.Offer_Residual_Value = Convert.ToDecimal(txtResidualValue_Cashflow.Text);

            if (!string.IsNullOrEmpty(txtResidualAmt_Cashflow.Text))
                ObjBusApplicationProcess.Offer_Residual_Value_Amount = Convert.ToDecimal(txtResidualAmt_Cashflow.Text);

            //string strXMLMortgage = "<Root> <Details ";

            //if (ddlTypeOfMortgage.SelectedIndex > 0)
            //    strXMLMortgage += " Mortgage_Type='" + ddlTypeOfMortgage.SelectedValue + "' ";

            //if (!string.IsNullOrEmpty(txtMortgageFee.Text))
            //    strXMLMortgage += " Mortgage_Fees='" + txtMortgageFee.Text + "' ";
            
            //if (ddlStepDown.SelectedIndex > 0)
            //    strXMLMortgage += " StepDown_RevisionType='" + ddlStepDown.SelectedValue + "' ";

            //strXMLMortgage += " /> </Root>";

            #region Constitution

            strXMLCommon = grvConsDocuments.FunPubFormXml(true);
            strXMLCommon = strXMLCommon.Replace("OPTIONAL/MANDATORY='1'", "");
            strXMLCommon = strXMLCommon.Replace("OPTIONAL/MANDATORY='0'", "");
            strXMLCommon = strXMLCommon.Replace("VALUE=''", "VALUE=' '");
            ObjBusApplicationProcess.XML_Constitution = strXMLCommon;
            strXMLCommon = "<Root></Root>";
            ObjBusApplicationProcess.XML_PDD = strXMLCommon;


            #endregion

            #region Asset
            //if (!ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM") && !ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") && !ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT"))
            if (!ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") && !ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") && !ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
            {
                if (Session["PricingAssetDetails"] != null)
                {
                    if (((DataTable)Session["PricingAssetDetails"]).Rows.Count > 0)
                    {

                        strXMLCommon = Utility.FunPubFormXml((DataTable)Session["PricingAssetDetails"], true);
                        strXMLCommon = strXMLCommon.Replace("SLNO=''", "");
                        strXMLCommon = strXMLCommon.Replace("BOOKDEPRECIATIONPERCENTAGE='&NBSP;'", "BOOKDEPRECIATIONPERCENTAGE='0'");
                        strXMLCommon = strXMLCommon.Replace("BLOCKDEPRECIATIONPERCENTAGE='&NBSP;'", "BLOCKDEPRECIATIONPERCENTAGE='0'");
                        strXMLCommon = strXMLCommon.Replace("''", "'0'");
                        ObjBusApplicationProcess.XML_AssetDetails = strXMLCommon;
                    }
                }

            }
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("PL"))
            {
                if (ViewState["dtasset"] != null)
                {
                    if (((DataTable)ViewState["dtasset"]).Rows.Count > 0)
                    {

                        strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["dtasset"], true);
                        strXMLCommon = strXMLCommon.Replace("SLNO=''", "");
                         strXMLCommon = strXMLCommon.Replace("''", "'0'");
                         ObjBusApplicationProcess.XML_AssetLoanHDR = strXMLCommon;
                    }
                }

                if (Session["dtapplicationasset"] != null)
                {
                    if (((DataTable)Session["dtapplicationasset"]).Rows.Count > 0)
                    {

                        strXMLCommon = Utility.FunPubFormXml((DataTable)Session["dtapplicationasset"], true);
                        strXMLCommon = strXMLCommon.Replace("SLNO=''", "");
                        strXMLCommon = strXMLCommon.Replace("''", "'0'");
                        ObjBusApplicationProcess.XML_AssetLoanDet = strXMLCommon;
                    }
                }
                if (Session["dtdocuments"] != null)
                {
                    if (((DataTable)Session["dtdocuments"]).Rows.Count > 0)
                    {

                        strXMLCommon = Utility.FunPubFormXml((DataTable)Session["dtdocuments"], true);
                        strXMLCommon = strXMLCommon.Replace("SLNO=''", "");
                        strXMLCommon = strXMLCommon.Replace("''", "'0'");
                        ObjBusApplicationProcess.XML_AssetLoandoc = strXMLCommon;
                    }
                }
            }

            #endregion

            #region ROI Rule

            if (!ddlROIRuleList.SelectedItem.Text.ToUpper().Contains("RRA"))
            {
                FunPriUpdateROIRule();
            }

            FunPriUpdateROIRuleDecRate();

            ObjBusApplicationProcess.XML_ROIRULE = ((DataTable)ViewState["ROIRules"]).FunPubFormXml(true);

            #endregion

            #region Inflow
            if (((DataTable)ViewState["DtCashFlow"]).Rows.Count > 0)
            {
                strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["DtCashFlow"], true);
                ObjBusApplicationProcess.XML_Inflow = strXMLCommon;
            }

            else
            {
                strXMLCommon = "<Root></Root>";
                ObjBusApplicationProcess.XML_Inflow = strXMLCommon;
            }
            #endregion

            #region OutFlow
            if (((DataTable)ViewState["DtCashFlowOut"]).Rows.Count > 0)
            {
                strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["DtCashFlowOut"], true);
                ObjBusApplicationProcess.XML_OutFlow = strXMLCommon;
            }

            else
            {
                strXMLCommon = "<Root></Root>";
                ObjBusApplicationProcess.XML_OutFlow = strXMLCommon;
            }
            #endregion

            #region Guarantor
            if (ViewState["dtGuarantorGrid"] != null)
            {
                if (((DataTable)ViewState["dtGuarantorGrid"]).Rows.Count > 0)
                {
                    strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["dtGuarantorGrid"], true);
                    ObjBusApplicationProcess.XML_Guarantor = strXMLCommon;
                }
            }
            #endregion

            #region ALERT
            if (ViewState["DtAlertDetails"] != null)
            {
                if (((DataTable)ViewState["DtAlertDetails"]).Rows.Count > 0)
                {

                    strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["DtAlertDetails"], true);
                    ObjBusApplicationProcess.XML_ALERT = strXMLCommon;
                }
            }
            #endregion

            #region Repayment
            if (ViewState["DtRepayGrid"] != null)
            {
                if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count > 0)
                {

                    strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["DtRepayGrid"], true);
                    ObjBusApplicationProcess.XML_Repayment = strXMLCommon;
                }
            }
            if (grvRepayStructure.Rows.Count > 0)
            {
                strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["RepaymentStructure"], true);
                ObjBusApplicationProcess.XMLRepaymentStructure = strXMLCommon;
            }


            //Added by saran on 3-Jul-2014 for CR_SISSL12E046_018 start
            DataTable dtRepayDetailsOthers = new DataTable();
            if (ViewState["dtRepayDetailsOthers"] != null)
            {
                dtRepayDetailsOthers = (DataTable)ViewState["dtRepayDetailsOthers"];
            }
            if (dtRepayDetailsOthers.Rows.Count > 0)
            {

                ObjBusApplicationProcess.XMLRepayDetailsOthers = dtRepayDetailsOthers.FunPubFormXml(true);
            }

            //Added by saran on 3-Jul-2014 for CR_SISSL12E046_018 end


            #endregion

            #region FollowDetail
            if (ViewState["DtFollowUp"] != null)
            {
                strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["DtFollowUp"], true);
                ObjBusApplicationProcess.XML_FollowDetail = strXMLCommon;
            }
            #endregion

            #region Moratorium
            if (ViewState["dtMoratorium"] != null)
            {
                strXMLCommon = Utility.FunPubFormXml((DataTable)ViewState["dtMoratorium"], true);
                ObjBusApplicationProcess.XML_Moratorium = strXMLCommon;
            }
            #endregion

            if (!string.IsNullOrEmpty(txtFBDate.Text))
            {
                ObjBusApplicationProcess.intFBDate = Convert.ToInt32(txtFBDate.Text);
            }

            intResult = ObjAProcessSave.FunPubCreateApplicationProcessInt(out strApp_Number, ObjBusApplicationProcess);

            if (intResult == 0)
            {
                if (intApplicationProcessId > 0)
                {
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    strAlert = strAlert.Replace("__ALERT__", "Application " + txtApplicationNo.Text + " Modified Successfully");
                    Session.Remove("PricingAssetDetails");
                    Session.Remove("AssetCustomer");
                    strApp_Number = txtApplicationNo.Text;
                    //Utility.FunShowAlertMsg(this, strAlert);
                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler) 
                    {

                        WorkFlowSession WFValues = new WorkFlowSession();
                        int intWorkflowStatus = 0;
                        try
                        {
                            intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strApp_Number, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, WFValues.Document_Type);
                            strAlert = "";

                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not Assigned";
                            int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                        }
                        WFValues.LastDocumentNo = strApp_Number;

                        //strRedirectPageView = strRedirectHomePage;
                        ShowWFAlertMessage(strApp_Number, ProgramCode, strAlert);
                        return;
                    }
                }
                else
                {
                    Session.Remove("PricingAssetDetails");
                    Session.Remove("AssetCustomer");
                    txtApplicationNo.Text = strApp_Number;
                    DataTable dtWorkFlow = new DataTable();
                    int WFProgramId = 0;
                    //if Starting Point is either Enquiry or Pricing

                    //Added by Thangam M on 27/Jul/2012 for CRM
                    if (Request.QueryString.Get("qsCRMID") != null)      // From CRM
                    {
                        btnSave.Enabled = false;

                        FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCRMID"));
                        Session["InitiateNumber"] = fromTicket.Name;
                        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hdnCustID.Value, false, 0);

                        string strCustomerAlert = "alert('Application " + strApp_Number + " Created Successfully');";
                        strCustomerAlert += "window.location.href='../Origination/S3GOrgCRM.aspx?qsCustomer=" + FormsAuthentication.Encrypt(Ticket) + "'";
                        //strCustomerAlert += "window.parent.document.getElementById('ctl00_ContentPlaceHolder1_btnFrameCancel').click()";
                        //strCustomerAlert += "window.opener.location.reload();window.close();";
                        strRedirectPageView = "";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strCustomerAlert, true);
                        return;
                    }

                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler) 
                    {

                        WorkFlowSession WFValues = new WorkFlowSession();
                        int intWorkflowStatus = 0;
                        try
                        {
                            intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strApp_Number, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, WFValues.Document_Type);
                            strAlert = "";

                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not Assigned";
                            int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                        }
                        WFValues.LastDocumentNo = strApp_Number;

                        //strRedirectPageView = strRedirectHomePage;
                        ShowWFAlertMessage(strApp_Number, ProgramCode, strAlert);
                        return;
                    }
                    //if Starting Point is Application
                    else if (CheckForWorkFlowConfiguration(ProgramCode, WFLOBId, WFProductId, out WFProgramId, out dtWorkFlow) > 0)
                    {
                        AssignNewWorkFlowValues(WFProgramId, int.Parse(ProgramCode), strApp_Number, WFBranchId, WFLOBId, WFProductId, "", dtWorkFlow);
                        try
                        {
                            int intWorkflowStatus = InsertWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFLOBId, WFBranchId, strApp_Number, WFProgramId, WFProductId, 3);
                            strAlert = "";

                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not Assigned";

                        }
                        ShowWFAlertMessage(strApp_Number, ProgramCode, strAlert);
                        return;
                    }
                    //if workflow has not been defined for this combination(LOB,Product,Branch)
                    else
                    {
                        DataTable WFFP = new DataTable();
                        string strWFMsg = "";
                        string strBusinessOfferNoList = string.Empty;
                        if (strMode=="C")
                        {
                            if (ddlBusinessOfferNoList.SelectedValue.ToString() != "0")
                            {
                                strBusinessOfferNoList = ddlBusinessOfferNoList.SelectedItem.Text.Substring(0, ddlBusinessOfferNoList.SelectedItem.Text.Trim().ToString().LastIndexOf("-") - 1).ToString();
                            }  
                        }
                        if (strMode == "M")
                        {
                            if (ddlBusinessOfferNoList.SelectedValue.ToString() != "0")
                            {
                                strBusinessOfferNoList = ddlBusinessOfferNoList.SelectedItem.Text;
                            }
                        }
                                               
                        if (CheckForForcePullOperation(null, strBusinessOfferNoList, ProgramCode, null, null, "O", CompanyId, null, null, ddlLOB.SelectedItem.Text, ddlProductCodeList.SelectedItem.Text, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), strApp_Number, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), 3);
                            }
                            catch (Exception ex)
                            {
                                strWFMsg = "WorkFlow not assigned";
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", WFProductId, dtrForce["WFSTATUSID"].ToString());
                            }

                        }

                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strAlert = "Application " + strApp_Number + " Created Successfully";
                        if (strWFMsg != "")
                            strAlert += @"\n'" + strWFMsg + "'";
                        strAlert += @"\n\nWould you like to Create one more Application?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                    }

                }

            }
            else if (intResult == -1)
            {
                if (intApplicationProcessId == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                }
                strRedirectPageView = "";
            }
            else if (intResult == -2)
            {
                if (intApplicationProcessId == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                }
                strRedirectPageView = "";
            }
            else if (intResult == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Finance Amount Exceeds");
                strRedirectPageView = "";
            }
            else if (intResult == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Tenure Exceeds");
                strRedirectPageView = "";
            }
            else
            {
                if (intApplicationProcessId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Due to Data Problem,Unable to Modify an Application");
                }
                else
                {

                    strAlert = strAlert.Replace("__ALERT__", "Due to Data Problem,Unable to Create an Application");

                }
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objFaultExp);
            //if (ObjAProcessSave != null)
            //{
            //    if (ObjAProcessSave.State == CommunicationState.Opened)
            //        ObjAProcessSave.Close();
            //}
            throw objFaultExp;
        }
        catch (ApplicationException ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            //ObjAProcessSave.Close();
            throw ex;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            //ObjAProcessSave.Close();
            throw ex;
        }
        finally
        {
            ObjAProcessSave.Close();
        }


    }

    /* WorkFlow Properties */
    private int WFLOBId { get { return int.Parse(ddlLOB.SelectedValue); } }
    private int WFBranchId { get { return int.Parse(ddlBranchList.SelectedValue); } }
    private int WFProductId { get { return int.Parse(ddlProductCodeList.SelectedValue); } }

    void AssignNewWorkFlowValues(int SelecteDocument, int SelectedProgramId, string SelectedDocumentNo, int BranchID, int LOBId, int ProductId, string LasDocumentNo, DataTable WFSequence)
    {
        WorkFlowSession WFValues = new WorkFlowSession(SelecteDocument, SelectedProgramId, SelectedDocumentNo, BranchID, LOBId, ProductId, LasDocumentNo, 3);
        WFValues.WorkFlowScreens = WFSequence;
    }
    DataTable LoadWorkFlowScreensList(string WFSequenceID)
    {
        //S3G_GEN_GetWorkFlowScreens
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Add("@WFSequence", WFSequenceID);
        DataTable WorkFlowScreens = Utility.GetDefaultData(SPNames.S3G_WORKFLOW_GetWorkFlowScreens, Procparam);
        return WorkFlowScreens;
    }

    private void FunPriShowScriptManager(string Title, string AlertMsg)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Title, "alert('" + AlertMsg + "');", true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriCalculateIRR()
    {
        try
        {
            decimal decActualAmount, decTotalAmount = 0;
            string strType;
            string stroption;
            RepaymentType rePayType = new RepaymentType();
            strType = ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim();
            switch (strType.ToLower())
            {
                case "tl":
                case "te":
                    rePayType = RepaymentType.TLE;
                    //tenure = 1;
                    break;
                case "ft":
                    rePayType = RepaymentType.FC;
                    // tenure = 1;
                    break;
                case "wc":
                    rePayType = RepaymentType.WC;
                    //tenure = 1;
                    break;
                default:
                    rePayType = RepaymentType.EMI;
                    break;
            }
            if (ddl_Return_Pattern.SelectedItem.Text == "PTF (Per thousand frequency)" || ddl_Return_Pattern.SelectedItem.Text == "PLF (Per lakh frequency)" || ddl_Return_Pattern.SelectedItem.Text == "PMF (Per million frequency)")
            {
                stroption = "3";
            }
            else
            {
                stroption = "2";
            }
            if (FunPriValidateTotalAmount(out decActualAmount, out decTotalAmount, stroption))
            {
                CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();

                DataTable dtRepaymentTab = (DataTable)ViewState["DtRepayGrid"];
                DataTable dtCashInflow = (DataTable)ViewState["DtCashFlow"];
                DataTable dtCashOutflow = (DataTable)ViewState["DtCashFlowOut"];
                double decResultIrr = 0;
                decimal decPLR = 0;
                if (hdnPLR.Value != "")
                {
                    decPLR = Convert.ToDecimal(hdnPLR.Value);
                }
                string strIrrRest = string.Empty;
                string strTimeval = string.Empty;
                switch (ddl_IRR_Rest.SelectedItem.Text.ToLower())
                {
                    case "day wise irr":
                        strIrrRest = "daily";
                        break;
                    case "month wise irr":
                        strIrrRest = "monthly";
                        break;
                    default:
                        strIrrRest = "daily";
                        break;

                }
                switch (ddl_Time_Value.SelectedItem.Text.ToLower())
                {
                    case "adv(advance)":
                    case "adf(advance fbd)":
                        strTimeval = "advance";
                        break;
                    case "arr(arrears)":
                    case "arf(arrears fbd)":
                        strTimeval = "arrears";
                        break;
                    default:
                        strTimeval = "advance";
                        break;
                }

                decimal decRate;
                double docRate;
                switch (ddl_Return_Pattern.SelectedItem.Text)
                {

                    case "IRR (Internal Rate of Return)":
                        ObjBusinessLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), Convert.ToDouble(txtRate.Text), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out docRate, Convert.ToDecimal(10.05), decPLR);
                        decRate = Convert.ToDecimal(docRate);
                        break;
                    default:
                        decRate = Convert.ToDecimal(txtRate.Text);
                        break;
                }
                //Function for Calculating IRR Called
                decimal? decResvalue = null;
                decimal? decResAmt = null;
                if (txtResidualAmt_Cashflow.Text != "")
                {
                    decResAmt = Convert.ToDecimal(txtResidualAmt_Cashflow.Text);
                }
                if (txtResidualValue_Cashflow.Text != "")
                {
                    decResvalue = Convert.ToDecimal(txtResidualValue_Cashflow.Text);
                }
                // ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtDate.Text), decResvalue, decResAmt, rePayType);


                txtAccountIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtAccountingIRR.Text = decResultIrr.ToString("0.0000");

                //ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Business_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtDate.Text), decResvalue, decResAmt, rePayType);
                txtBusinessIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtBusinessIRR.Text = decResultIrr.ToString("0.0000");

                //ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, FunPriGetAmountFinanced(), decRate, strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Company_IRR, out decResultIrr, Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(txtDate.Text), decResvalue, decResAmt, rePayType);
                txtCompanyIRR_Repay.Text = decResultIrr.ToString("0.0000");
                txtCompanyIRR.Text = decResultIrr.ToString("0.0000");
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriIRRReset()
    {
        try
        {
            txtAccountingIRR.Text = "";
            txtAccountIRR_Repay.Text = "";
            txtBusinessIRR.Text = "";
            txtBusinessIRR_Repay.Text = "";
            txtCompanyIRR.Text = "";
            txtCompanyIRR_Repay.Text = "";
            if (ViewState["DtCashFlow"] != null)
            {
                DataTable DtCashFlow = (DataTable)ViewState["DtCashFlow"];
                if (DtCashFlow.Rows.Count > 0)
                {
                    DataRow[] drUMFC = null;
                    if (DtCashFlow.Columns.Contains("CashFlow_ID"))
                    {
                        drUMFC = DtCashFlow.Select("CashFlow_ID = 34");
                    }
                    else
                    {
                        drUMFC = DtCashFlow.Select("CashFlow_Flag_ID = 34");
                    }
                    if (drUMFC.Length > 0)
                    {
                        drUMFC[0].Delete();
                        DtCashFlow.AcceptChanges();
                        ViewState["DtCashFlow"] = DtCashFlow;
                        if (DtCashFlow.Rows.Count > 0)
                        {
                            FunPriFillInflowDLL(strEditMode);
                        }
                        else
                        {
                            FunPriFillInflowDLL(strAddMode);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private string FunPriGetScalarValue(DataTable ObjDT)
    {
        try
        {
            if (ObjDT.Rows.Count > 0)
                return Convert.ToString(ObjDT.Rows[0][0]);
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriLoadTenureType()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

        try
        {
            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
            Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriFillMainPageDLL()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

        try
        {
            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
            Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

            //Code added by Chandru K On 18/Sep/2013 For ISFC Customization

            //ObjStatus.Option = 1;
            //ObjStatus.Param1 = "Type_of_Mortgage";
            //Utility.FillDLL(ddlTypeOfMortgage, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
            
            //ObjStatus.Option = 1;
            //ObjStatus.Param1 = "Step_Down_Revision";
            //Utility.FillDLL(ddlStepDown, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

            //End

            //ObjStatus.Option = 35;
            //ObjStatus.Param1 = intCompanyId.ToString();
            //Utility.FillDLL(ddlSalePersonCodeList, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@Company_ID", Convert.ToString(intCompanyId));
            objProcedureParameter.Add("@User_Id", Convert.ToString(intUserId));
            objProcedureParameter.Add("@Program_Id", "38");
            if (intApplicationProcessId == 0)
            {
                objProcedureParameter.Add("@Is_Active", "1");
            }
            ddlLOB.BindDataTable(SPNames.LOBMaster, objProcedureParameter, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            //if (strMode != "C")
            //{
            //    ddlBranchList.BindDataTable(SPNames.BranchMaster_LIST, objProcedureParameter, new string[] { "Location_ID", "Location" });
            //}
            objProcedureParameter = null;

            FunPriLoadOfferNo();
            //ObjCustomerService.Close();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            //ObjCustomerService.Close();
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }

    }
    private void FunPriLoadOfferNo()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        try
        {

            ObjStatus.Option = 33;
            ObjStatus.Param1 = intCompanyId.ToString();
            if (intApplicationProcessId == 0)
            {
                ObjStatus.Param2 = "C";
            }
            else
            {
                ObjStatus.Param2 = "M";
            }

            if (ddlLOB.SelectedIndex > 0)
            {
                ObjStatus.Param3 = ddlLOB.SelectedValue;
            }
            if (ddlBranchList.SelectedValue != "0")
            {
                ObjStatus.Param4 = ddlBranchList.SelectedValue;
            }
            Utility.FillDLL(ddlBusinessOfferNoList, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Business Offer No");
        }
        finally
        {
            ObjCustomerService.Close();
            ObjStatus = null;
        }
    }
    private DateTime FunPriSetLastDate(DateTime dt)
    {
        try
        {
            DateTime dtTo = dt;
            dtTo = dt.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));

            return dtTo;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    private bool FunPriValidateUserPassword()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {
            bool ischeck = false;

            if (intApplicationProcessId == 0)
            {
                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                ObjStatus.Option = 57;
                ObjStatus.Param1 = intUserId.ToString();
                ObjStatus.Param2 = intCompanyId.ToString();
                ObjStatus.Param3 = txtPassword.Text.Trim();
                if (FunPriGetScalarValue(ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus)).ToString() == "1")
                {
                    ischeck = true;
                }
                else
                    ischeck = false;

                //ObjCustomerService.Close();
                return ischeck;
            }
            else
                return true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            //ObjCustomerService.Close();
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private string FunPriValidateEmpty(string Str, bool IsNeedZero)
    {
        try
        {
            string ReturnS = string.Empty;

            if (string.IsNullOrEmpty(Str))
            {
                if (IsNeedZero)
                    ReturnS = "0";
            }
            else
                ReturnS = Str;

            return ReturnS;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadApplicationDetails()
    {
        try
        {
            //<<Performance>>
            //FunPriLoadLObandBranch(intUserId, intCompanyId);
            //FunPriFillMainPageDLL();
            //FunPriLoadTenureType();
            ListItem lstitem;
            string strSPName = "S3G_ORG_GETAPPPROCESSDETAILS";
            Dictionary<string, string> procparam = new Dictionary<string, string>();
            procparam.Add("@ApplicationProcessId", intApplicationProcessId.ToString());
            procparam.Add("@Mode", strMode);
            DataSet dsApplicationProcessDetails = Utility.GetDataset(strSPName, procparam);
            div7.Visible = true;
            div8.Visible = true;
            lblRoundNo.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["RoundNo"]);
            txtApplicationNo.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Application_Number"]);
            txtApplication_Followup.Text = txtApplicationNo.Text;
            txtDate.Text = DateTime.Parse(dsApplicationProcessDetails.Tables[0].Rows[0]["Date"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
            txtStatus.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Status"]);

            //<<Performance>>
            //lstitem = new ListItem(dsApplicationProcessDetails.Tables[0].Rows[0]["LocationCat_Description"].ToString(), dsApplicationProcessDetails.Tables[0].Rows[0]["Location_ID"].ToString());
            //ddlBranchList.Items.Add(lstitem);
            ddlBranchList.SelectedText = dsApplicationProcessDetails.Tables[0].Rows[0]["LocationCat_Description"].ToString();
            ddlBranchList.SelectedValue = dsApplicationProcessDetails.Tables[0].Rows[0]["Location_ID"].ToString();

            ddlBranchList.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Location_ID"]);
            if (Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Pricing_ID"]) != string.Empty)
            {
                lstitem = new ListItem(dsApplicationProcessDetails.Tables[0].Rows[0]["Business_Offer_Number"].ToString(), dsApplicationProcessDetails.Tables[0].Rows[0]["Pricing_ID"].ToString());
                ddlBusinessOfferNoList.Items.Add(lstitem);
            }
            else
            {
                lstitem = new ListItem("--Select--", "-1");
                ddlBusinessOfferNoList.Items.Add(lstitem);
            }

            if (Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Pricing_ID"]) != string.Empty)
            {
                //string strBusinessNo = ddlBusinessOfferNoList.SelectedItem.Text.Substring(0, ddlBusinessOfferNoList.SelectedItem.Text.Trim().ToString().LastIndexOf("-") - 1).ToString();
                ddlBusinessOfferNoList.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Pricing_ID"]);
                txtOfferNo_Followup.Text = ddlBusinessOfferNoList.SelectedItem.Text;
                ddlBusinessOfferNoList.ClearDropDownList();
            }
            TextBox txtName = ucCustomerCodeLov.FindControl("txtName") as TextBox;
            txtName.Text = txtCustomerCode.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Customer_Code"]);
            hdnCustID.Value = dsApplicationProcessDetails.Tables[0].Rows[0]["Customer_Id"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(dsApplicationProcessDetails.Tables[0].Rows[0]["Customer_Code"].ToString(),
                    dsApplicationProcessDetails.Tables[0].Rows[0]["comm_Address1"].ToString() + "\n" +
                    dsApplicationProcessDetails.Tables[0].Rows[0]["comm_Address2"].ToString() + "\n" +
                    dsApplicationProcessDetails.Tables[0].Rows[0]["comm_city"].ToString() + "\n" +
                    dsApplicationProcessDetails.Tables[0].Rows[0]["comm_state"].ToString() + "\n" +
                    dsApplicationProcessDetails.Tables[0].Rows[0]["comm_country"].ToString() + "\n" +
                    dsApplicationProcessDetails.Tables[0].Rows[0]["comm_pincode"].ToString(), dsApplicationProcessDetails.Tables[0].Rows[0]["Customer_Name"].ToString(),
                    dsApplicationProcessDetails.Tables[0].Rows[0]["Comm_Telephone"].ToString(),
                    dsApplicationProcessDetails.Tables[0].Rows[0]["Comm_mobile"].ToString(),
                    dsApplicationProcessDetails.Tables[0].Rows[0]["comm_email"].ToString(), dsApplicationProcessDetails.Tables[0].Rows[0]["comm_website"].ToString());
            ViewState["GuarantorCustomer"] = dsApplicationProcessDetails.Tables[0].Rows[0]["Customer_Id"].ToString();
            Session["AssetCustomer"] = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Customer_Id"]) + ";" + txtName.Text;

            //<<Performance>>

            lstitem = new ListItem(dsApplicationProcessDetails.Tables[0].Rows[0]["LOB_Name"].ToString(), dsApplicationProcessDetails.Tables[0].Rows[0]["LOB_ID"].ToString());
            ddlLOB.Items.Add(lstitem);

            ddlLOB.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["LOB_ID"]);

            //<<Performance>>
            Utility.FillDLL(ddlTenureType, dsApplicationProcessDetails.Tables[18]);

            //Utility.FillDLL(ddlTypeOfMortgage, dsApplicationProcessDetails.Tables[22]);
            //Utility.FillDLL(ddlStepDown, dsApplicationProcessDetails.Tables[23]);

            //if (Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Mortgage_Type"]) != string.Empty)
            //    ddlTypeOfMortgage.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Mortgage_Type"]);
            //txtMortgageFee.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Mortgage_Fees"]);
            //if (Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["StepDown_RevisionType"]) != string.Empty)
            //    ddlStepDown.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["StepDown_RevisionType"]);

            FunPriMLASLAApplicable(Convert.ToInt32(ddlLOB.SelectedValue));
            txtLOB_Followup.Text = ddlLOB.SelectedItem.Text;
            txtBranch_Followup.Text = ddlBranchList.SelectedText;

            lstitem = new ListItem();
            lstitem.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["PRODUCT"]);
            lstitem.Value = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Product_ID"]);
            ddlProductCodeList.Items.Add(lstitem);

               ddlSalePersonCodeList.SelectedText = dsApplicationProcessDetails.Tables[0].Rows[0]["Sales_Person"].ToString();
                ddlSalePersonCodeList.SelectedValue = dsApplicationProcessDetails.Tables[0].Rows[0]["Sales_Person_ID"].ToString();
                ddlSalePersonCodeList.ToolTip = dsApplicationProcessDetails.Tables[0].Rows[0]["Sales_Person"].ToString();
                if (strMode != "M")
                    ddlSalePersonCodeList.ReadOnly = true;
            ddlSalePersonCodeList.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Sales_Person_ID"]);
            lstitem = new ListItem();
            lstitem.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Consitution"]);
            lstitem.Value = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Constitution_ID"]);
            ddlConstitutionCodeList.Items.Add(lstitem);
            txtConstitution.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Consitution"]);
            if (string.IsNullOrEmpty(Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Refinance_Contract"])))
                ChkRefinanceContract.Checked = false;
            else
                ChkRefinanceContract.Checked = Convert.ToBoolean(Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Refinance_Contract"]));
            if (!string.IsNullOrEmpty(Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["FBDate"])))
            {
                txtFBDate.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["FBDate"]);
                rfvFBDate.Enabled = true;
                rngFBDate.Enabled = true;
            }
            else
            {
                txtFBDate.Text = "";
                rfvFBDate.Enabled = false;
                rngFBDate.Enabled = false;
            }
            txtFinanceAmount.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Finance_Amount"]);

            txtBusinessIRR_Repay.Text = txtBusinessIRR.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Business_IRR"]);
            txtCompanyIRR_Repay.Text = txtCompanyIRR.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Company_IRR"]);
            txtAccountIRR_Repay.Text = txtAccountingIRR.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Accounting_IRR"]);
            txtTenure.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Tenure"]);
            ddlTenureType.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Tenure_Type"]);

            txtResidualValue.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Residual_Value"]);
            txtResidualValue.Text = txtResidualValue.Text.StartsWith("0") ? "" : txtResidualValue.Text;

            txtMarginMoneyPer_Cashflow.Text = Convert.ToDecimal(Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Offer_Margin"])).ToString();
            txtMarginMoneyAmount_Cashflow.Text = Convert.ToDecimal(Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Offer_Margin_Amount"])).ToString();

            txtEnquiry_Followup.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Enquiry_Ref_Number"]);
            if (dsApplicationProcessDetails.Tables[0].Rows[0]["Enquiry_Date"] != DBNull.Value)
                txtEnquiryDate_Followup.Text = Convert.ToDateTime(dsApplicationProcessDetails.Tables[0].Rows[0]["Enquiry_Date"]).ToString(strDateFormat);

            txtCustNameAdd_Followup.Text = S3GCustomerAddress1.CustomerName + "\n" + S3GCustomerAddress1.CustomerAddress;
            ddlConstitutionCodeList.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Constitution_ID"]);
            hdnConstitutionId.Value = ddlConstitutionCodeList.SelectedValue;
            ddlProductCodeList.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Product_ID"]);

            if (strMode == "M")
            {
                FunPriBindPaymentDDL(Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Payment_Rule_Card_Id"]));
            }
            else
            {
                lstitem = new ListItem(dsApplicationProcessDetails.Tables[0].Rows[0]["Payment_Rule_Number"].ToString(), dsApplicationProcessDetails.Tables[0].Rows[0]["Payment_Rule_Card_Id"].ToString());
                ddlPaymentRuleList.Items.Add(lstitem);
            }
            ddlPaymentRuleList.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Payment_Rule_Card_Id"]);
            hdnPayment.Value = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Payment_Rule_Card_Id"]);
            if (dsApplicationProcessDetails.Tables[0].Rows[0]["Payment_Rule_Card_Id"].ToString() != "0")
            {
                FunPriLoadPaymentRuleDetails();
            }

            grvConsDocuments.DataSource = dsApplicationProcessDetails.Tables[1];
            grvConsDocuments.DataBind();

            if (dsApplicationProcessDetails.Tables[2].Rows.Count > 0)
            {
                gvAlert.DataSource = dsApplicationProcessDetails.Tables[2];
                gvAlert.DataBind();
                FunPriFillAlertDLL(strEditMode);
                ViewState["DtAlertDetails"] = dsApplicationProcessDetails.Tables[2];
            }
            else
            {
                FunPriFillAlertDLL(strAddMode);
            }

            FunPriCalculateSummary(dsApplicationProcessDetails.Tables[4], "CashFlow", "TotalPeriodInstall");
            ViewState["DtCashFlow"] = dsApplicationProcessDetails.Tables[5];
            if (dsApplicationProcessDetails.Tables[5].Rows.Count > 0)
            {
                FunPriFillInflowDLL(strEditMode);
            }
            else
            {
                FunPriFillInflowDLL(strAddMode);
            }

            ViewState["DtCashFlowOut"] = dsApplicationProcessDetails.Tables[6];
            if (dsApplicationProcessDetails.Tables[6].Rows.Count > 0)
            {
                FunPriFillOutflowDLL(strEditMode);
            }
            else
            {
                FunPriFillOutflowDLL(strAddMode);
            }
            if (!ddlLOB.SelectedItem.Text.ToString().Contains("PL"))
            {
                ViewState["DtRepayGrid"] = dsApplicationProcessDetails.Tables[4];

                FunPriFillRepaymentDLL(strEditMode);
            }
            //gvRepaymentDetails.FooterRow.Visible = false;

            btnReset.Enabled = false;
            /*changed by Prabhu.K on 22-Nov-2011 for UAT Issue in App.Approval*/
            if (TabContainerMainTab.Tabs[1].Visible)
            {
                Session["PricingAssetDetails"] = dsApplicationProcessDetails.Tables[12];
            }
            FunPriFillROIDLL(strAddMode);
            objProcedureParameter = new Dictionary<string, string>();

            if (strMode == "M")
            {
                Utility.BindDataTable(ddlROIRuleList, dsApplicationProcessDetails.Tables[20], new string[] { "ROI_Rules_ID", "ROI_Rule_Number", "Model_Description" });
            }
            else
            {
                lstitem = new ListItem(dsApplicationProcessDetails.Tables[7].Rows[0]["ROI_Number"].ToString(), dsApplicationProcessDetails.Tables[7].Rows[0]["ROI_Rules_Id"].ToString());
                ddlROIRuleList.Items.Add(lstitem);
            }

            //<<Performance>>
            //if (strMode.ToUpper().Trim() != "Q")
            //    objProcedureParameter.Add("@Is_Active", "1");

            //if (strMode.ToUpper().Trim() == "M")
            //    objProcedureParameter.Add("@Rules_ID", Convert.ToString(dsApplicationProcessDetails.Tables[7].Rows[0]["ROI_Rules_Id"]));


            //objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());
            //objProcedureParameter.Add("@LOB_ID", ddlLOB.SelectedValue);
            //objProcedureParameter.Add("@Option", "7");
            //ddlROIRuleList.BindDataTable(SPNames.S3G_ORG_GetPricing_List, objProcedureParameter, new string[] { "ROI_Rules_ID", "ROI_Rule_Number", "Model_Description" });

            ddlROIRuleList.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[7].Rows[0]["ROI_Rules_Id"]);
            hdnROIRule.Value = Convert.ToString(dsApplicationProcessDetails.Tables[7].Rows[0]["ROI_Rules_Id"]);

            ViewState["ROIRules"] = dsApplicationProcessDetails.Tables[7];
            FunPriLoadROIRuleDetails(strEditMode);

            txtResidualValue_Cashflow.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Offer_Residual_Value"]);
            txtResidualValue_Cashflow.Text = txtResidualValue_Cashflow.Text.StartsWith("0") ? "" : txtResidualValue_Cashflow.Text;
            txtResidualAmt_Cashflow.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Residual_Value"]);
            txtResidualAmt_Cashflow.Text = txtResidualAmt_Cashflow.Text.StartsWith("0") ? "" : txtResidualAmt_Cashflow.Text;

            //added by saranya
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") ||
                ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") ||
                ((ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") ||
                ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TL")) &&
                ddl_Repayment_Mode.SelectedItem.Text.ToUpper().StartsWith("PRO")))
            {
                Button btnAdd = gvInflow.FooterRow.FindControl("btnAdd") as Button;
                btnAdd.Enabled = false;
                // TabContainerAP.Tabs[2].Enabled = false;

            }
            //end
            if (dsApplicationProcessDetails.Tables[10].Rows.Count > 0)
            {
                ViewState["DtFollowUp"] = dsApplicationProcessDetails.Tables[10];
                FunPriFillFollowupDLL(strEditMode);
            }
            else
            {
                FunPriFillFollowupDLL(strAddMode);
            }

            txtMarginAmount.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["Margin_Amount"]);
            lblTotalOutFlowAmount.Text = dsApplicationProcessDetails.Tables[11].Rows[0][0].ToString();
            //commented by saranya for TERM LOAN/EXTENSIBLE changes
            // if (ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING") || ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM"))
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING"))
            {
                TabContainerMainTab.Tabs[1].Visible = false;
            }
            else
            {
                TabContainerMainTab.Tabs[1].Visible = true;
            }
            if (TabContainerMainTab.Tabs[1].Visible)
            {
                Session["PricingAssetDetails"] = dsApplicationProcessDetails.Tables[12];
                gvAssetDetails.DataSource = dsApplicationProcessDetails.Tables[12];
                gvAssetDetails.DataBind();
            }
            if (dsApplicationProcessDetails.Tables[14].Rows.Count > 0)
            {
                ViewState["mode"] = strEditMode;
            }
            else
            {
                ViewState["mode"] = strAddMode;
            }
            ViewState["dtMoratorium"] = dsApplicationProcessDetails.Tables[14];
            FunPriFillMoratoriumDLL();


            ViewState["dtGuarantorGrid"] = dsApplicationProcessDetails.Tables[3];
            if (dsApplicationProcessDetails.Tables[3].Rows.Count > 0)
            {
                ViewState["mode"] = strEditMode;
            }
            else
            {
                ViewState["mode"] = strAddMode;
            }
            FunPriFillGuarantorDLL();
            ViewState["InvoiceDetails"] = dsApplicationProcessDetails.Tables[15];
            gvInvoiceDetails.DataSource = dsApplicationProcessDetails.Tables[15];
            gvInvoiceDetails.DataBind();
            if (dsApplicationProcessDetails.Tables[15].Rows.Count > 0)
            {
                pnlInvoiceDetails.Visible = true;
            }
            else
            {
                pnlInvoiceDetails.Visible = false;
            }
            if (dsApplicationProcessDetails.Tables[16].Rows.Count > 0)
            {
                Panel8.Visible = true;
                gvPRDDT.DataSource = dsApplicationProcessDetails.Tables[16];
                gvPRDDT.DataBind();
            }

            if (TabContainerAP.Tabs[6].Visible)
            {
                txtROIMLA.Text = ddlROIRuleList.SelectedItem.Text;
                if (ddlPaymentRuleList.SelectedItem.Text != "--Select--")
                    txtPaymentCardMLA.Text = ddlPaymentRuleList.SelectedItem.Text;
                ddlDoyouWant_MLA.SelectedValue = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["MLA_Applicable"]);
                txtMLANo.Text = Convert.ToString(dsApplicationProcessDetails.Tables[0].Rows[0]["MLA_Number"]);
                txtMLANo.Text = txtMLANo.Text == "0" ? "" : txtMLANo.Text;
                if (dsApplicationProcessDetails.Tables[0].Rows[0]["MLA_Validity_From"] != null)
                {
                    if (dsApplicationProcessDetails.Tables[0].Rows[0]["MLA_Validity_From"].ToString() != "")
                    {
                        txtValidFrom_MLA.Text = DateTime.Parse(dsApplicationProcessDetails.Tables[0].Rows[0]["MLA_Validity_From"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
                    }
                }
                if (dsApplicationProcessDetails.Tables[0].Rows[0]["MLA_Validity_to"].ToString() != "")
                    txtValidTo_MLA.Text = DateTime.Parse(dsApplicationProcessDetails.Tables[0].Rows[0]["MLA_Validity_to"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
                else
                    txtValidTo_MLA.Text = "";

                txtMLAFinanceAmount.Text = txtFinanceAmount.Text;
                rfvtxtPassword.Enabled = false;
            }
            if (ddlBusinessOfferNoList.SelectedValue != "-1")
                ddlPaymentRuleList.ClearDropDownList();
            if (dsApplicationProcessDetails.Tables[17].Rows.Count > 0)
            {
                gvMLADetails.DataSource = dsApplicationProcessDetails.Tables[17];
                gvMLADetails.DataBind();
            }
            if (!ddlLOB.SelectedItem.Text.ToString().Contains("PL"))
            {

                FunPriShowRepaymetDetails((decimal)dsApplicationProcessDetails.Tables[4].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
            }

            //<<Performance>>

            //objProcedureParameter = new Dictionary<string, string>();
            //objProcedureParameter.Add("@ApplicationProcessId", intApplicationProcessId.ToString());
            //DataSet dsRepaymentStructure = Utility.GetDataset("s3g_org_loadRepayStructure", objProcedureParameter);

            if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
            {
                ViewState["DtRepayGrid_TL"] = dsApplicationProcessDetails.Tables[4];
            }

            ViewState["RepaymentStructure"] = dsApplicationProcessDetails.Tables[21];
            grvRepayStructure.DataSource = dsApplicationProcessDetails.Tables[21];
            grvRepayStructure.DataBind();

            //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
            if (dsApplicationProcessDetails.Tables[23].Rows.Count > 0)
                ViewState["dtRepayDetailsOthers"] = dsApplicationProcessDetails.Tables[23];
            //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end

            FunPriSetRateLength();
            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                rfvddlPaymentRuleList.Enabled = false;
            }
            else
            {
                rfvddlPaymentRuleList.Enabled = true;
            }
            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL") && dsApplicationProcessDetails.Tables[22].Rows.Count > 0)
            {
                ViewState["dtasset"] = dsApplicationProcessDetails.Tables[22];
                grvloanassethdr.DataSource = dsApplicationProcessDetails.Tables[22];
                grvloanassethdr.DataBind();
            }
            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL") && dsApplicationProcessDetails.Tables[24].Rows.Count > 0)
            {
                Session["dtapplicationasset"] = dsApplicationProcessDetails.Tables[24];
              
            }
            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL") && dsApplicationProcessDetails.Tables[25].Rows.Count > 0)
            {
                Session["dtdocuments"] = dsApplicationProcessDetails.Tables[25];
              
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriToggleModeControls()
    {
        try
        {
            ddlDoyouWant_MLA.ClearDropDownList();
            ddlLOB.ClearDropDownList();
            //ddlBranchList.ClearDropDownList();
            ddlBranchList.ReadOnly = true;
            ddlBusinessOfferNoList.ClearDropDownList();
            ddlProductCodeList.ClearDropDownList();


            //Enabled this controls when Starting point is application processing - as per Bug id 6194
            if (ddlBusinessOfferNoList.SelectedValue != "-1")
            {
                //ddlSalePersonCodeList.ClearDropDownList();
                ddlTenureType.ClearDropDownList();
                ChkRefinanceContract.Enabled = false;
                txtResidualValue.Attributes.Add("readonly", "true");
                txtMarginAmount.Attributes.Add("readonly", "true");
                txtTenure.ReadOnly = txtFinanceAmount.ReadOnly = true;
                btnAddAsset.Visible = false;
                btnAddAsset.Enabled = false;
            }
            txtResidualAmt_Cashflow.ReadOnly = txtResidualValue_Cashflow.ReadOnly =
            txtMarginMoneyAmount_Cashflow.ReadOnly = txtMarginMoneyPer_Cashflow.ReadOnly =
            txtCustomerCode.ReadOnly = txtPassword.ReadOnly = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriInitializePage()
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private bool FunPriValidatePage()
    {
        bool blnIsValid = true;
        try
        {
            string strLOBType = ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim();

            #region Asset
            //commented by saranya
            // if (strLOBType != "te" && strLOBType != "tl" && strLOBType != "wc" && strLOBType != "ft")
            if (strLOBType != "wc" && strLOBType != "ft")
            {
                if (Session["PricingAssetDetails"] != null)
                {
                    decimal dcmFinanceAmount = Convert.ToDecimal(txtFinanceAmount.Text);
                    decimal dcmMarginAmount = 0;
                    if (!string.IsNullOrEmpty(txtMarginMoneyPer_Cashflow.Text))
                    {
                        dcmMarginAmount = FunPriGetMarginAmout();
                    }
                    if (((DataTable)Session["PricingAssetDetails"]).Rows.Count > 0)
                    {
                        DataRow[] drFilter = ((DataTable)Session["PricingAssetDetails"]).Select("Finance_Amount = 0");
                        if (drFilter.Length > 0)
                        {
                            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + " Enter the Finance Amount in AssetDetails";
                            cvApplicationProcessing.IsValid = false;

                            blnIsValid = false;
                        }
                        else
                        {
                            decimal dcmAssetFinanceAmount = Convert.ToDecimal(((DataTable)Session["PricingAssetDetails"]).Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
                            decimal dcmAssetMarginAmount = Convert.ToDecimal(((DataTable)Session["PricingAssetDetails"]).Compute("Sum(Margin_Amount)", "Noof_Units > 0"));
                            if (dcmFinanceAmount != dcmAssetFinanceAmount)
                            {
                                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + " The sum of Finance Amount in Asset Details should be equal to Finance Amount";
                                cvApplicationProcessing.IsValid = false;
                                blnIsValid = false;
                            }
                            else if (dcmMarginAmount > dcmAssetMarginAmount)
                            {
                                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + " The sum of Margin Amount in Asset Details should be greater than or equal to Margin Amount in ROI Rules";
                                cvApplicationProcessing.IsValid = false;

                                blnIsValid = false;
                            }
                        }

                        //checking cashflow for Discount Absorbed.
                        DataRow[] drDis_Absorbed = ((DataTable)Session["PricingAssetDetails"]).Select("Discount_Absorbed = 1");
                        if (drDis_Absorbed.Length > 0)
                        {
                            DataTable dtCashInflow = new DataTable();
                            if (ViewState["DtCashFlow"] != null)
                            {
                                dtCashInflow = (DataTable)ViewState["DtCashFlow"];
                            }

                            if (dtCashInflow.Rows.Count > 0)
                            {
                                DataRow[] drCashInflow = dtCashInflow.Select("CashFlow_Flag_ID = 94");//Discount Flag

                                if (drCashInflow.Length == 0)
                                {
                                    cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Discount cashflow should be added in Cash Inflow details";
                                    cvApplicationProcessing.IsValid = false;

                                    blnIsValid = false;
                                }
                            }

                        }

                    }

                }
            }
            #endregion

            if (rfvtxtPassword.Enabled == true)
            {
                S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
                if (ObjS3GAdminServices.FunPubPasswordValidation(ObjUserInfo.ProUserIdRW, txtPassword.Text.Trim()) > 0)
                {
                    cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + " Invalid Password. ";
                    cvApplicationProcessing.IsValid = false;
                    blnIsValid = false;
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return blnIsValid;
    }


    private void FunPriCancelApplication()
    {
        ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjAProcessSave = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
        S3GBusEntity.ApplicationProcess.ApplicationProcess objApplicationProcess = new S3GBusEntity.ApplicationProcess.ApplicationProcess();
        try
        {
            objApplicationProcess.Application_Process_ID = intApplicationProcessId;
            int intResult = ObjAProcessSave.FunPubUpdateApplicationStatus(objApplicationProcess);
            if (intResult == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "Application cancelled successfully");
            }
            else
            {

                strAlert = strAlert.Replace("__ALERT__", "Due to Data Problem,Unable to Cancel an Application");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;

        }
        finally
        {
            ObjAProcessSave.Close();
        }
    }

    private void FunPriClearPage()
    {
        try
        {
            FunPriClearForms();
            FunPriFillMainPageDLL();
            FunPriLoadLObandBranch(intUserId, intCompanyId);
            //ddlBranchList.Items.Clear();
            ddlBranchList.Clear();
            ddlProductCodeList.Items.Clear();
            FunPriLoadOfferNo();
            S3GCustomerAddress1.SetCustomerDetails("", "", "", "", "", "", "");
            txtCustomerCode.Text =
            txtConstitution.Text = string.Empty;
            TextBox txtCode = ucCustomerCodeLov.FindControl("txtName") as TextBox;
            txtCode.Text = "";
            Button btnGetLov = ucCustomerCodeLov.FindControl("btnGetLov") as Button;
            btnGetLov.Visible = true;
            Session.Remove("PricingAssetDetails");
            Session.Remove("AssetCustomer");
            ddlSalePersonCodeList.Clear();
            TabContainerAP.ActiveTabIndex = 0;
            FunPriSetInitialSettings();
            btnCreateCustomer.Visible = true;
            btnAddAsset.Visible = true;
            txtTenure.ReadOnly = false;
            pnlIRRDetails.Visible = true;
            FunPriAssignAssetLink();
            HiddenField Hdval = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            Hdval.Value = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriClosePage()
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
            }
            else
            {
                Response.Redirect(strRedirectPage,false);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriResetIRRDetails()
    {
        try
        {
            FunPriFillRepaymentDLL(strAddMode);
            txtBusinessIRR.Text = txtCompanyIRR.Text = txtAccountingIRR.Text =
            txtBusinessIRR_Repay.Text = txtAccountIRR_Repay.Text = txtCompanyIRR_Repay.Text = string.Empty;
            gvRepaymentSummary.DataSource = null;
            gvRepaymentSummary.DataBind();
            ViewState["RepaymentStructure"] = null;
            grvRepayStructure.DataSource = null;
            grvRepayStructure.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
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
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "38");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }


    // Added By Shibu 17-Sep-2013 Sales Personal List (Auto Suggestion)
    [System.Web.Services.WebMethod]
    public static string[] GetSalePersonList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_Get_User_List, Procparam));

        return suggestions.ToArray();
    }


    private void FunPriToggleLOBBased()
    {
        try
        {
           
            ddlSalePersonCodeList.Clear();
            if (ddlLOB.SelectedIndex > 0)
            {
                div7.Visible = false;
                div8.Visible = false;

                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                //Procparam.Add("@Is_Active", "1");
                //Procparam.Add("@User_Id", intUserId.ToString());
                //Procparam.Add("@Company_ID", intCompanyId.ToString());
                //Procparam.Add("@Lob_ID", ddlLOB.SelectedValue);
                //Procparam.Add("@Program_Id", "38");
                //ddlBranchList.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
                ddlBranchList.Clear();

                #region Add Asset
                FunPriAssignAssetLink();
                #endregion
                FunPriMLASLAApplicable(Convert.ToInt32(ddlLOB.SelectedValue));
                FunPriLOBBasedvalidations(ddlLOB.SelectedItem.Text, ddlLOB.SelectedItem.Value, strAddMode, "");
                txtLOB_Followup.Text = ddlLOB.SelectedItem.Text;
                FunPriFillInflowDLL(strAddMode);
                FunPriFillOutflowDLL(strAddMode);
                FunRepayClear("");
                //FunPriFillRepaymentDLL(strAddMode);


                //commented by saranya
                //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("TERM") || ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING"))
                if (ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") || ddlLOB.SelectedItem.Text.ToUpper().Contains("WORKING"))
                {
                    TabContainerMainTab.Tabs[1].Visible = false;
                }
                else
                {
                    TabContainerMainTab.Tabs[1].Visible = true;
                }
                if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
                {
                    rfvddlPaymentRuleList.Enabled = false;

                }
                else
                {
                    rfvddlPaymentRuleList.Enabled = true;

                }
            }
            else
            {
                ddlProductCodeList.Items.Clear();
                ddlConstitutionCodeList.Items.Clear();
                ddlROIRuleList.Items.Clear();
                ddlPaymentRuleList.Items.Clear();
                grvConsDocuments.Visible = false;
                FunPriFillMainPageDLL();
            }
            if (gvAssetDetails.Rows.Count > 0)
            {
                gvAssetDetails.DataSource = null;
                gvAssetDetails.DataBind();
                Session.Remove("PricingAssetDetails");
                if (Session["AssetCustomer"] != null && !string.IsNullOrEmpty(S3GCustomerAddress1.CustomerName))
                {
                    Session["AssetCustomer"] = hdnCustID.Value + ";" + S3GCustomerAddress1.CustomerName;
                }
                else
                {
                    Session.Remove("AssetCustomer");
                }
            }
            if (ddlLOB.SelectedItem.Text.ToString().Contains("PL"))
            {
                ViewState["dtasset"] = null;
                FunPriInitializeAssetGridData();
                Session.Remove("dtapplicationasset");
                Session.Remove("dtdocuments");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriAssignAssetLink()
    {
        string strNewPurchase = "";
        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
        {
            strNewPurchase = "Yes";
        }
        else
        {
            strNewPurchase = "No";
        }
        
        if ((ddlLOB.SelectedIndex > 0 || intApplicationProcessId > 0) && txtCustomerCode.Text != "")
        {
            btnAddAsset.Attributes.Remove("onclick");
            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL"))
            {
                btnAddAsset.Attributes.Add("onclick", strNewWin1 + "?qsMaster=" + strNewPurchase + NewWinAttributes);
            }
            else
            {
                btnAddAsset.Attributes.Add("onclick", strNewWin + "?qsMaster=" + strNewPurchase + NewWinAttributes);
            }
        }
    }

    private void FunPriToggleOfferNoBased()
    {
        try
        {
            Session.Remove("PricingAssetDetails");
            Session.Remove("AssetCustomer");
            pnlIRRDetails.Visible = true;
            ddlSalePersonCodeList.Clear();
            if (ddlBusinessOfferNoList.SelectedIndex == 0)
            {
                TextBox txtName = ucCustomerCodeLov.FindControl("txtName") as TextBox;
                txtName.Text = "";
                FunPriClearForms();
                txtCustomerCode.Text = txtConstitution.Text = string.Empty;
                S3GCustomerAddress1.SetCustomerDetails("", "", "", "", "", "", "");
                txtDate.Text = DateTime.Now.Date.ToString(strDateFormat);
                FunPriLoadLObandBranch(intUserId, intCompanyId);
                FunPriLoadTenureType();
                ddlProductCodeList.Items.Clear();
                TabContainerMainTab.Tabs[1].Visible = true;
                TabContainerAP.Tabs[6].Visible = true;
                txtCustomerCode.ReadOnly = false;
                btnCreateCustomer.Visible = true;
                btnAddAsset.Visible = true;
                txtOfferDate.Text = "";
                txtTenure.ReadOnly = false;
                ddlSalePersonCodeList.Clear();
                Session.Remove("PricingAssetDetails");
                Session.Remove("AssetCustomer");
                Button btnGetLov = ucCustomerCodeLov.FindControl("btnGetLov") as Button;
                btnGetLov.Visible = true;
            }
            else
            {
                FunPubGetPricingDetails(Convert.ToInt32(ddlBusinessOfferNoList.SelectedValue));
                FunPriSetRateLength();
                btnAddAsset.Visible = false;
                Button btnGetLov = ucCustomerCodeLov.FindControl("btnGetLov") as Button;
                btnGetLov.Visible = false;
                gvAssetDetails.Columns[7].Visible = false;
                txtCustomerCode.ReadOnly = true;
                btnCreateCustomer.Visible = false;
            }
            FunPriSetMaxLength();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriToggleProductBased()
    {
        try
        {
            if (ddlProductCodeList.SelectedIndex > 0)
            {
                FunPriBindPaymentDDL("");
            }
            else
            {
                ddlPaymentRuleList.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Get IRR Details From Global Paramater Setup
    /// </summary>
    protected void FunProGetIRRDetails()
    {
        try
        {
            DataTable dtIRRDetails = Utility.FunPubGetGlobalIRRDetails(intCompanyId, null);
            ViewState["IRRDetails"] = dtIRRDetails;
            if (dtIRRDetails.Rows.Count > 0)
            {
                //Added by Thangam on 19-Jun-2012 to solve modify mode round off issue
                ViewState["hdnRoundOff"] = dtIRRDetails.Rows[0]["Roundoff"].ToString();
                S3GBusEntity.CommonS3GBusLogic.GPSRoundOff = Convert.ToInt32(ViewState["hdnRoundOff"].ToString());

                if (dtIRRDetails.Rows[0]["IsIRRApplicable"].ToString() == "True")
                {
                    txtAccountingIRR.Visible = true;
                    lblAccountingIRR.Visible = true;
                    txtCompanyIRR.Visible = true;
                    lblCompanyIRR.Visible = true;

                    txtCompanyIRR_Repay.Visible = true;
                    lblCompanyIRR_Repay.Visible = true;
                    rfvCompanyIRR.Enabled = true;
                    txtAccountIRR_Repay.Visible = true;
                    lblAccountIRR_Repay.Visible = true;
                    rfvAccountingIRR.Enabled = true;
                }
                else
                {
                    txtAccountingIRR.Visible = false;
                    lblAccountingIRR.Visible = false;
                    txtCompanyIRR.Visible = false;
                    lblCompanyIRR.Visible = false;

                    txtCompanyIRR_Repay.Visible = false;
                    lblCompanyIRR_Repay.Visible = false;
                    rfvCompanyIRR.Enabled = false;
                    txtAccountIRR_Repay.Visible = false;
                    lblAccountIRR_Repay.Visible = false;
                    rfvAccountingIRR.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunProBindAssetGrid()
    {
        DataTable ObjDTAssetDetail = new DataTable();
        try
        {
            if (ViewState["ObjDTAssetDetails"] != null)
                ObjDTAssetDetail = (DataTable)ViewState["ObjDTAssetDetails"];
            decimal intAssetamount = 0;
            foreach (DataRow drrow in ObjDTAssetDetail.Rows)
            {
                string unitvalueAmount = drrow["Noof_Units"].ToString();
                if (unitvalueAmount != "&nbsp;")
                {
                    intAssetamount = intAssetamount + Convert.ToInt32(drrow["Noof_Units"]) * Convert.ToDecimal(drrow["Unit_Value"]);

                    //if (intAssetamount > Convert.ToDouble(lblTotalOutFlowAmount.Text))
                    //{
                    //    Utility.FunShowAlertMsg(this, "Total asset amount cannot be greater than total cash outflow amount");
                    //    //showmessage("Total asset amount can't be greater than total cash outflow amount");
                    //    intAssetamount = intAssetamount - Convert.ToInt32(drrow["Noof_Units"]) * Convert.ToDouble(drrow["Unit_Value"]); ;
                    //    ObjDTAssetDetail.Rows.RemoveAt(ObjDTAssetDetail.Rows.Count - 1);
                    //}
                    //lblTotalAssetAmount.Text = intAssetamount.ToString();

                }
            }

            gvAssetDetails.DataSource = ObjDTAssetDetail;
            gvAssetDetails.DataBind();
            gvAssetDetails.Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunCalculateIRR(object sender, EventArgs e)
    {
        try
        {
            FunPriCalculateIRR();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + ex.Message.Replace("'", " ").Replace(";", " ") + "');", true);
        }
    }

    protected void FunPriGenerateRepaymentSchedule(ClsRepaymentStructure objRepaymentStructure, DateTime dtStartDate)
    {
        try
        {
            DataSet dsRepayGrid = new DataSet();
            DataTable dtRepayDetails = new DataTable();
             //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
            DataTable dtRepayDetailsOthers = new DataTable();
            //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end
            DataTable dtMoratorium = null;
            Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
            objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text);
            objMethodParameters.Add("Tenure", txtTenure.Text);
            objMethodParameters.Add("TenureType", ddlTenureType.SelectedItem.Text);

            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                DataTable dsAssetDetails = (DataTable)Session["PricingAssetDetails"];
                decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
                objMethodParameters.Add("FinanceAmount", dcmTotalAssetValue.ToString());
            }
            else
            {
                DataRow[] drFinanAmtRow = ((DataTable)ViewState["DtCashFlowOut"]).Select("CashFlow_Flag_ID = 41");
                if (drFinanAmtRow.Length > 0)
                {
                    decimal decToatlFinanceAmt = (decimal)((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");

                    if (Convert.ToDecimal(txtFinanceAmount.Text) != decToatlFinanceAmt)
                    {
                        Utility.FunShowAlertMsg(this, "Total amount financed in Cashoutflow should be equal to amount financed");
                        FunRepayClear("");
                        return;
                    }
                }

                objMethodParameters.Add("FinanceAmount", txtFinanceAmount.Text);
            }
            objMethodParameters.Add("ReturnPattern", ddl_Return_Pattern.SelectedValue);
            objMethodParameters.Add("MarginPercentage", txtMarginMoneyPer_Cashflow.Text);
            objMethodParameters.Add("Rate", txtRate.Text);
            objMethodParameters.Add("TimeValue", ddl_Time_Value.SelectedValue);
            objMethodParameters.Add("RepaymentMode", ddl_Repayment_Mode.SelectedValue);
            objMethodParameters.Add("CompanyId", intCompanyId.ToString());
            objMethodParameters.Add("LobId", ddlLOB.SelectedValue);
            objMethodParameters.Add("DocumentDate", txtDate.Text);
            objMethodParameters.Add("Frequency", ddl_Frequency.SelectedValue);
            objMethodParameters.Add("RecoveryYear1", txt_Recovery_Pattern_Year1.Text);
            objMethodParameters.Add("RecoveryYear2", txt_Recovery_Pattern_Year2.Text);
            objMethodParameters.Add("RecoveryYear3", txt_Recovery_Pattern_Year3.Text);
            objMethodParameters.Add("RecoveryYear4", txt_Recovery_Pattern_Rest.Text);
            if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
            {
                objMethodParameters.Add("PrincipalMethod", "1");
            }
            else
            {
                objMethodParameters.Add("PrincipalMethod", "0");
            }
            if (ViewState["hdnRoundOff"] != null)
            {
                if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                    objMethodParameters.Add("Roundoff", ViewState["hdnRoundOff"].ToString());
                else
                    objMethodParameters.Add("Roundoff", "2");
            }
            else
            {
                objMethodParameters.Add("Roundoff", "2");
            }
            DataTable dtOutflow = ((DataTable)ViewState["DtCashFlowOut"]).Clone();
            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                DataSet dsOutlfow = (DataSet)ViewState["OutflowDDL"];
                DataRow drOutflow = dtOutflow.NewRow();
                drOutflow["Date"] = Utility.StringToDate(txtDate.Text);
                drOutflow["CashOutFlow"] = "OL Lease Amount";
                drOutflow["EntityID"] = hdnCustID.Value;
                drOutflow["Entity"] = S3GCustomerAddress1.CustomerName;
                drOutflow["OutflowFromId"] = "144";
                drOutflow["OutflowFrom"] = "Customer";
                DataTable dsAssetDetails = (DataTable)Session["PricingAssetDetails"];
                decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
                drOutflow["Amount"] = dcmTotalAssetValue;
                drOutflow["CashOutFlowID"] = "-1";
                drOutflow["Accounting_IRR"] = true;
                drOutflow["Business_IRR"] = true;
                drOutflow["Company_IRR"] = true;
                drOutflow["CashFlow_Flag_ID"] = "41";
                dtOutflow.Rows.Add(drOutflow);
            }

            //For TL
            ViewState["DtRepayGrid_TL"] = null;

            //Checking if other than normal payment , start date should be last payment date.
            if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
            {
                DataTable dtAcctype = ((DataTable)ViewState["PaymentRules"]);
                dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
                string strAcctType = dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper();

                if (strAcctType == "PROJECT FINANCE" || strAcctType == "DEFERRED PAYMENT" || strAcctType == "DEFERRED STRUCTURED")
                {
                    DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
                    if (DtCashFlowOut.Rows.Count > 0)
                    {
                        DataRow drOutFlw = DtCashFlowOut.Select("CashFlow_Flag_ID=41").Last();
                        if (drOutFlw != null)
                        {
                            objMethodParameters.Remove("DocumentDate");
                            objMethodParameters.Add("DocumentDate", drOutFlw["Date"].ToString());
                            dtStartDate = Utility.StringToDate(drOutFlw["Date"].ToString());

                            if (!string.IsNullOrEmpty(txtFBDate.Text))
                            {
                                //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
                                //DateTime dtDocDate = Utility.StringToDate(txtDate.Text);
                                DateTime dtDocDate = dtStartDate;
                                //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

                                DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
                                dtformat.ShortDatePattern = "MM/dd/yy";
                                string strFBDate = "";
                                try
                                {
                                    strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
                                }
                                catch (Exception ex)
                                {
                                    DateTime lastDayOfCurrentMonth = new DateTime(dtDocDate.Year, dtDocDate.Month,
                                                                        DateTime.DaysInMonth(dtDocDate.Year, dtDocDate.Month));
                                    strFBDate = lastDayOfCurrentMonth.ToString(strDateFormat);
                                }
                                //string strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
                                dtStartDate = Utility.StringToDate(strFBDate);
                            }

                        }
                    }

                }
            }



            if (ddl_Return_Pattern.SelectedValue == "2")
            {
                if (txtResidualAmt_Cashflow.Text.Trim() != "" && txtResidualAmt_Cashflow.Text.Trim() != "0")
                {
                    objMethodParameters.Add("decResidualAmount", txtResidualAmt_Cashflow.Text);
                }
                else if (txtResidualValue_Cashflow.Text.Trim() != "" && txtResidualValue_Cashflow.Text.Trim() != "0")
                {
                    /*Code commented and added for bug fixing - Kuppu - Jan-07-2012 - Error on Residual% = 0 - Bug_ID - 6099 */
                    //objMethodParameters.Add("decResidualValue", txtResidualValue_Cashflow.TemplateSourceDirectory);
                    objMethodParameters.Add("decResidualValue", txtResidualValue_Cashflow.Text);
                }
                switch (ddl_IRR_Rest.SelectedValue)
                {
                    case "1":
                        objMethodParameters.Add("strIRRrest", "daily");
                        break;
                    case "2":
                        objMethodParameters.Add("strIRRrest", "monthly");
                        break;
                    default:
                        objMethodParameters.Add("strIRRrest", "daily");
                        break;

                }

                objMethodParameters.Add("decLimit", "0.10");
                decimal decRateOut = 0;

                if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
                {
                    dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, (DataTable)ViewState["DtCashFlow"], dtOutflow, objMethodParameters, dtMoratorium, out decRateOut);
                }
                else
                {
                    dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"], objMethodParameters, dtMoratorium, out decRateOut);
                }
                ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);
            }
            else
            {
                dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, objMethodParameters, dtMoratorium);
            }
            decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text);
            if (dsRepayGrid == null)
            {
                /* It Calculates and displays the Repayment Details for ST-ADHOC */
                grvRepayStructure.DataSource = null;
                grvRepayStructure.DataBind();
                FunPriShowRepaymetDetails(decFinAmount + FunPriGetStructureAdhocInterestAmount());
                gvRepaymentDetails.FooterRow.Visible = true;
                btnReset.Enabled = true;
                return;
            }
            //DtRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(Utility.StringToDate(strDocumentDate), objMethodParameters);
            if (dsRepayGrid.Tables[0].Rows.Count > 0)
            {
                gvRepaymentDetails.DataSource = dsRepayGrid.Tables[0];
                gvRepaymentDetails.DataBind();
                ViewState["DtRepayGrid"] = dsRepayGrid.Tables[0];
                //if (ddl_Rate_Type.SelectedItem.Text == "Floating" && string.IsNullOrEmpty(txtFBDate.Text))
                //{
                //    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
                //    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
                //}
                //else
                //{
                ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
                ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
                //}
                btnReset.Enabled = false;
                FunPriCalculateSummary(dsRepayGrid.Tables[0], "CashFlow", "TotalPeriodInstall");
                decimal decBreakPercent;// = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));

                if (!((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))))
                {
                    decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
                }
                else
                {
                    DataRow[] dr = ((DataTable)ViewState["DtRepayGrid"]).Select("CashFlow_Flag_ID = 23");
                    if (dr.Length > 0)
                    {
                        decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
                    }
                    else
                    {
                        decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID in(35,91)"));
                    }

                }
                if (decBreakPercent != 0)
                {
                    if (decBreakPercent != 100)
                    {
                        Utility.FunShowAlertMsg(this, "Total break up percentage should be equal to 100%");
                        return;
                    }
                }
                double douAccountingIRR = 0;
                double douBusinessIRR = 0;
                double douCompanyIRR = 0;
                DataTable dtRepaymentStructure = new DataTable();

                try
                {

                    string strStartDte = txtDate.Text;
                    DateTime dtDocFBDate = Utility.StringToDate(strStartDte);
                    int intDeffered = 0;
                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
                    {
                        DataTable dtAcctype = ((DataTable)ViewState["PaymentRules"]);
                        dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
                        string strAcctType = dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper();

                        if (strAcctType == "PROJECT FINANCE" || strAcctType == "DEFERRED PAYMENT" || strAcctType == "DEFERRED STRUCTURED")
                        {
                            intDeffered = 1;//Defferred Payment
                            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
                            if (DtCashFlowOut.Rows.Count > 0)
                            {
                                DataRow drOutFlw = DtCashFlowOut.Select("CashFlow_Flag_ID=41").Last();
                                if (drOutFlw != null)
                                {
                                    strStartDte = drOutFlw["Date"].ToString();

                                   


                                }
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(txtFBDate.Text))
                    {
                        //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
                        //DateTime dtDocDate = Utility.StringToDate(txtDate.Text);
                        DateTime dtDocDate = Utility.StringToDate(strStartDte);
                        //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

                        DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
                        dtformat.ShortDatePattern = "MM/dd/yy";
                        string strFBDate = "";
                        try
                        {
                            strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
                        }
                        catch (Exception ex)
                        {
                            DateTime lastDayOfCurrentMonth = new DateTime(dtDocDate.Year, dtDocDate.Month,
                                                                DateTime.DaysInMonth(dtDocDate.Year, dtDocDate.Month));
                            strFBDate = lastDayOfCurrentMonth.ToString(strDateFormat);
                        }
                        //string strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
                        dtDocFBDate = Utility.StringToDate(strFBDate);
                    }

                    if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
                    {
                        DataTable dsAssetDetails = (DataTable)Session["PricingAssetDetails"];
                        decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
                        objRepaymentStructure.FunPubCalculateIRR(strStartDte, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                            , out dtRepaymentStructure
                               //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                            , out dtRepayDetailsOthers
                        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                            ,(DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], dtOutflow
                            , dcmTotalAssetValue.ToString(), txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                            , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                            ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium,false);
                    }
                    else
                    {
                        if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
                        {
                            objRepaymentStructure.FunPubCalculateIRR(strStartDte, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                                , out dtRepaymentStructure, out dtRepayDetails
                                //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                            , out dtRepayDetailsOthers
                                //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                                , (DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                                , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                                , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                                ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium, ddl_Interest_Levy.SelectedValue.ToString(), intDeffered, dtDocFBDate.ToString());

                            intSlNo = 0;
                            gvRepaymentDetails.DataSource = dtRepayDetails;
                            gvRepaymentDetails.DataBind();
                            ViewState["DtRepayGrid_TL"] = ((DataTable)ViewState["DtRepayGrid"]).Copy();
                            ViewState["DtRepayGrid"] = dtRepayDetails;
                        }
                        else
                        {
                            objRepaymentStructure.FunPubCalculateIRR(strStartDte, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                                , out dtRepaymentStructure
                                //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                            , out dtRepayDetailsOthers
                                //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                                ,(DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                                , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                                , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                                ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium,false);
                        }
                    }

                    dtRepaymentStructure.Columns["Charge"].ColumnName = "FinanceCharges";
                    ViewState["RepaymentStructure"] = dtRepaymentStructure;
                    grvRepayStructure.DataSource = dtRepaymentStructure;
                    grvRepayStructure.DataBind();

                    //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
                    if (dtRepayDetailsOthers != null)
                        ViewState["dtRepayDetailsOthers"] = dtRepayDetailsOthers;
                    //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end

                    txtAccountIRR_Repay.Text = douAccountingIRR.ToString("0.0000");
                    txtAccountingIRR.Text = douAccountingIRR.ToString("0.0000");

                    txtBusinessIRR_Repay.Text = douBusinessIRR.ToString("0.0000");
                    txtBusinessIRR.Text = douBusinessIRR.ToString("0.0000");

                    txtCompanyIRR_Repay.Text = douCompanyIRR.ToString("0.0000");
                    txtCompanyIRR.Text = douCompanyIRR.ToString("0.0000");
                }
                catch (Exception Ex1)
                {
                    FunRepayClear(Ex1.Message);
                }
            }
            else
            {
                gvRepaymentDetails.FooterRow.Visible = true;
                btnReset.Enabled = true;
            }

            if (dsRepayGrid.Tables[0].Rows.Count > 0)
            {
                if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
                {
                    FunPriShowRepaymetDetails((decimal)dtRepayDetails.Compute("SUM(TotalPeriodInstall)", "1=1"));
                    FunPriCalculateSummary(dtRepayDetails, "CashFlow", "TotalPeriodInstall");
                }
                else
                {
                    FunPriShowRepaymetDetails((decimal)dsRepayGrid.Tables[0].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
                }
            }
            else
            {
                /* It Calculates and displays the Repayment Details for ST-ADHOC */
                FunPriShowRepaymetDetails(decFinAmount + FunPriGetStructureAdhocInterestAmount());
            }

            FunPriGenerateNewRepayment();
            FunPriUpdateROIRule();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunRepayClear(string StrErrorMsg)
    {
        grvRepayStructure.DataSource = null;
        grvRepayStructure.DataBind();
        ViewState["RepaymentStructure"] = null;

        txtAccountIRR_Repay.Text = "";
        txtAccountingIRR.Text = "";
        txtBusinessIRR_Repay.Text = "";
        txtBusinessIRR.Text = "";
        txtCompanyIRR_Repay.Text = "";
        txtCompanyIRR.Text = "";
        if (StrErrorMsg != "")
            Utility.FunShowAlertMsg(this, StrErrorMsg);
    }

    #endregion

    #endregion

    #region Events

    #region Page Event

    //protected new void Page_PreInit(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunPriInitializePage();
    //    }
    //    catch (Exception objException)
    //    {
    //        if (Request.QueryString["Popup"] != null)
    //        {
    //            throw objException;
    //        }
    //        else
    //        {
    //            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix +  "Unable to Initialise the Controls in Page";
    //            cvApplicationProcessing.IsValid = false;
    //        }
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // WF Initializtion 
            ProgramCode = "038";
            
            obj_Page = this;

            FunPriLoadPage();

            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");

            txtValidTo_MLA.Attributes.Add("onblur", "fnDoDate(this,'" + txtValidTo_MLA.ClientID + "','" + strDateFormat + "',false,'F');");
            //Code Added by Saranya for Customer Changes
            if (gvGuarantor.FooterRow != null)
            {
                UserControls_LOBMasterView ucCustomerLov = gvGuarantor.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
                DropDownList ddlGuarantortype_GuarantorTab1 = gvGuarantor.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
                if (ddlGuarantortype_GuarantorTab1.SelectedIndex > 0)
                {
                    if (ddlGuarantortype_GuarantorTab1.SelectedItem.Text.StartsWith("G") && ddlGuarantortype_GuarantorTab1.SelectedItem.Text.EndsWith("1"))
                    {
                        ucCustomerLov.strLOV_Code = "GCMD";
                    }
                    else if (ddlGuarantortype_GuarantorTab1.SelectedItem.Text.Contains("G") && ddlGuarantortype_GuarantorTab1.SelectedItem.Text.Contains("2"))
                    {
                        ucCustomerLov.strLOV_Code = "PCMD";
                    }
                    else
                    {
                        //For Co-Applicant
                        ucCustomerLov.strLOV_Code = "COAP";
                    }

                }
                else
                {
                    ucCustomerLov.strLOV_Code = "CMD";

                }

                ucCustomerLov.strControlID = ucCustomerLov.ClientID;
                TextBox txt1 = (TextBox)ucCustomerLov.FindControl("txtName");
                txt1.Attributes.Add("onfocus", "fnLoadCustomerg()");
                //end
            }

        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }
    #endregion

    #region Button Events

    #region Common Control

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriClearPage();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Unable to Clear the data";
            cvApplicationProcessing.IsValid = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>


    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            #region Validation

            #region Round No

            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL") &&!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL")&& ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            {
                ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                if (((DataTable)ViewState["DtCashFlow"]).Rows.Count > 0)
                {
                    decimal decUMFC = (decimal)((DataTable)ViewState["DtCashFlow"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 34");
                    if (decUMFC != FunPriGetInterestAmount())
                    {
                        cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Unmatured Finance Charges (UMFC) should be equal to Interest";
                        cvApplicationProcessing.IsValid = false;
                        return;
                    }
                }
                else
                {
                    cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Recalculate IRR ";
                    cvApplicationProcessing.IsValid = false;
                    return;
                }
            }

            #endregion

            #region Cashflow
            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                if (((DataTable)ViewState["DtCashFlowOut"]).Rows.Count == 0)
                {
                    FunPriShowScriptManager("Cash Outflow details", "Add atleast one Outflow details");
                    return;
                }

                if (FunPriPaymentRuleValidation())
                {
                    return;
                }

                DataRow[] drFinanAmtRow = ((DataTable)ViewState["DtCashFlowOut"]).Select("CashFlow_Flag_ID = 41");
                if (drFinanAmtRow.Length > 0)
                {
                    decimal decToatlFinanceAmt = (decimal)((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");

                    if (Convert.ToDecimal(txtFinanceAmount.Text) != decToatlFinanceAmt)
                    {
                        cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Total amount financed in Cashoutflow should be equal to amount financed";
                        cvApplicationProcessing.IsValid = false;
                        return;
                    }
                }
                else
                {
                    cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Payment Cashflow Description not available in Outflow Details";
                    cvApplicationProcessing.IsValid = false;
                    return;
                }
            }
            DataRow[] drMarginAmountRow = ((DataTable)ViewState["DtCashFlowOut"]).Select("CashFlow_Flag_ID = 43");
            if (drMarginAmountRow.Length > 0)
            {
                decimal decToatlMarginAmt = (decimal)((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 43");
                if (txtMarginAmount.Text == "")
                {
                    cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Enter the Margin Amount in Asset(s)";
                    cvApplicationProcessing.IsValid = false;
                    return;
                }
                if (Convert.ToDecimal(txtMarginAmount.Text) != decToatlMarginAmt)
                {
                    cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Total Margin Amount in Outflow should be equal to Margin Amount in Assets";
                    cvApplicationProcessing.IsValid = false;
                    return;
                }
            }
            #endregion

            #region Repayment
            //added by saranya
            //if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") && ddl_Repayment_Mode.SelectedValue != "5")
            //{
            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL"))
            {
            if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
            {
                FunPriShowScriptManager("Repayment details", "Add atleast one Repayment details");

                return;
            }
            }
            //}
            #endregion

            #region Guarantor
            if (ViewState["dtGuarantorGrid"] != null)
            {
                DataTable dtGuarantor = ViewState["dtGuarantorGrid"] as DataTable;
                HiddenField hdnCustomerId = ucCustomerCodeLov.FindControl("hdnID") as HiddenField;
                if (hdnCustomerId.Value == "")
                {
                    hdnCustomerId.Value = Convert.ToString(ViewState["GuarantorCustomer"]);
                }
                DataRow[] drCustomerGuarantor = dtGuarantor.Select("Code = " + hdnCustomerId.Value);

                if (drCustomerGuarantor.Length > 0)
                {
                    FunPriShowScriptManager("Guarantor details", "Guarantor should be other than customer");
                    return;
                }
            }

            #endregion

            #region Non-Mandatory discussed with sudarsan
            //#region Alert
            //if (((DataTable)ViewState["DtAlertDetails"]).Rows.Count == 0)
            //{
            //    FunPriShowScriptManager("Alert details", "Add atleast one Alert details");

            //    return;
            //}
            //#endregion

            //#region Followup
            //if (((DataTable)ViewState["DtFollowUp"]).Rows.Count == 0)
            //{
            //    FunPriShowScriptManager("FollowUp details", "Add atleast one Follow up details");

            //    return;
            //}
            #endregion

            #endregion

            if (strMode != "Q")
            {
                if (!FunPriValidatePage())
                {
                    return;
                }

            }
            //added by saranya
            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("PL") )
            {
            if (txtAccountIRR_Repay.Text == "" || txtBusinessIRR_Repay.Text == "" || txtCompanyIRR_Repay.Text == "")
            {
                FunPriIRRReset();
                cvApplicationProcessing.IsValid = false;
                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Recalculate IRR ";
                return;

            }
            }

            FunPriSaveRecord();
        }
        catch (ApplicationException ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Unable to Create/Modify the Application";
            cvApplicationProcessing.IsValid = false;
        }
    }

    private bool FunIsDeferredStructPayment()
    {
        bool blnResult = false;
        DataTable dtAcctype = ((DataTable)ViewState["PaymentRules"]);
        dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";

        if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper() == "DEFERRED STRUCTURED")
        {
            if (!string.IsNullOrEmpty(Convert.ToString(((DataTable)ViewState["DtCashFlowOut"]).Compute("Count(CashFlow_Flag_ID)", "CashFlow_Flag_ID = 41"))))
            {
                Int32 IntTotalOutflow = (Int32)((DataTable)ViewState["DtCashFlowOut"]).Compute("Count(CashFlow_Flag_ID)", "CashFlow_Flag_ID = 41");
                if (IntTotalOutflow <= 1)
                {
                    Utility.FunShowAlertMsg(this, "Cash Outflow should be More than one for Deferred Structured");
                    blnResult = true;
                }
            }
        }

        return blnResult;
    }

    private bool FunPriPaymentRuleValidation()
    {
        bool blnResult = false;
        DataTable dtAcctype = ((DataTable)ViewState["PaymentRules"]);
        dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
        DataTable dtOutflow = (DataTable)ViewState["DtCashFlowOut"];
        DataRow[] drOutflowDate;

        drOutflowDate = dtOutflow.Select("CashFlow_Flag_ID = 41 and Date < #" + Utility.StringToDate(txtDate.Text) + "#");
        if (drOutflowDate.Length > 0)
        {
            Utility.FunShowAlertMsg(this, "Outflow date should be greater than or equal to Application date");
            blnResult = true;
        }

        if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().ToUpper() == "DEFERRED PAYMENT")
        {
            drOutflowDate = dtOutflow.Select("CashFlow_Flag_ID = 41 and Date <= #" + Utility.StringToDate(txtDate.Text) + "#");
            if (drOutflowDate.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Outflow date should be greater than Application date for Deferred Payment");
                blnResult = true;
            }

            drOutflowDate = dtOutflow.Select("CashFlow_Flag_ID = 41 and Date <= #" + Utility.StringToDate(txtDate.Text) + "#");
            if (drOutflowDate.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Outflow date should be greater than Application date for Deferred Payment");
                blnResult = true;
            }

        }
        if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().ToUpper() == "TRADE ADVANCE" ||
            dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().ToUpper() == "NORMAL PAYMENT")
        {
            drOutflowDate = dtOutflow.Select("CashFlow_Flag_ID = 41 and Date <> #" + Utility.StringToDate(txtDate.Text) + "#");
            if (drOutflowDate.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Outflow date should be equal to Application date for Normal Payment/Trade Advance");
                blnResult = true;
            }
        }
        if ((dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper() == "DEFERRED STRUCTURED") ||
            (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper() == "PROJECT FINANCE"))
        {
            if (!string.IsNullOrEmpty(Convert.ToString(((DataTable)ViewState["DtCashFlowOut"]).Compute("Count(CashFlow_Flag_ID)", "CashFlow_Flag_ID = 41"))))
            {
                Int32 IntTotalOutflow = (Int32)((DataTable)ViewState["DtCashFlowOut"]).Compute("Count(CashFlow_Flag_ID)", "CashFlow_Flag_ID = 41");
                if (IntTotalOutflow <= 1)
                {
                    Utility.FunShowAlertMsg(this, "Cash Outflow should be More than one for Deferred Structured/Project finance");
                    blnResult = true;
                }
            }
        }

        return blnResult;

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["qsCRMID"] != null)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "window.close();", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "window.parent.document.getElementById('ctl00_ContentPlaceHolder1_btnFrameCancel').click()", true);

                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hdnCustID.Value, false, 0);
                Response.Redirect("S3GOrgCRM.aspx?qsCustomer=" + FormsAuthentication.Encrypt(Ticket));
            }


            // wf cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else
                FunPriClosePage();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Unable to Cancel this Process";
            cvApplicationProcessing.IsValid = false;
        }
    }



    #endregion

    #region Page Specific
    protected void btnApplicationCancel_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriCancelApplication();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem,Unable to Create/Modify Application";
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void btnConfigure_Click(object sender, EventArgs e)
    {
        try
        {
            if (((DataTable)ViewState["DtCashFlowOut"]).Rows.Count == 0 || ((DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
            {
                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + " Define the Finance,OfferTerms,Repayment Related Details";
                cvApplicationProcessing.IsValid = false;
                TabContainerAP.ActiveTabIndex = 0;
                return;
            }
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            if (objRepaymentStructure.FunPubGetCashFlowDetails(intCompanyId, Convert.ToInt32(ddlLOB.SelectedValue)).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Define Installment Flag in Cashflow Master for selected Line of Business");
                return;
            }
            txtAccountingIRR.Text = "";
            txtAccountIRR_Repay.Text = "";
            txtBusinessIRR.Text = "";
            txtBusinessIRR_Repay.Text = "";
            txtCompanyIRR.Text = "";
            txtCompanyIRR_Repay.Text = "";
            strRRBDate = strDocumentDate = txtDate.Text;
            FunPriGenerateRepaymentSchedule(objRepaymentStructure, Utility.StringToDate(txtDate.Text));
        }

        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem,Unable to Configure the IRR/Repayment Details";
            cvApplicationProcessing.IsValid = false;
        }
    }

    #endregion

    #region Repayment Tab

    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriResetIRRDetails();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem, Unable to Reset the Repayment Schedule";
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void btnGenerateRepay_Click(object sender, EventArgs e)
    {
        try
        {

            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();

            if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
            {
                if (objRepaymentStructure.FunPubGetCashFlowDetails_TL_Princ(intCompanyId, Convert.ToInt32(ddlLOB.SelectedValue)).Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Define Principal and Interest Flags in Cashflow Master for selected Line of Business");
                    return;
                }
            }
            else
            {
                if (objRepaymentStructure.FunPubGetCashFlowDetails(intCompanyId, Convert.ToInt32(ddlLOB.SelectedValue)).Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Define Installment Flag in Cashflow Master for selected Line of Business");
                    return;
                }
            }
            FunPriIRRReset();
            strRRBDate = strDocumentDate = txtDate.Text;
            //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
            if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                DataTable dtAssetDtls = new DataTable();
                if (Session["PricingAssetDetails"] != null)
                {
                    dtAssetDtls = (DataTable)Session["PricingAssetDetails"];
                }
                if (dtAssetDtls.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtAssetDtls.Compute("Max(Required_FromDate)", "Noof_Units > 0").ToString()))
                        strRRBDate = strDocumentDate = strDocumentDate = dtAssetDtls.Compute("Max(Required_FromDate)", "Noof_Units > 0").ToString();
                }

            }

            //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

            if (!string.IsNullOrEmpty(txtFBDate.Text))
            {
                //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
                //DateTime dtDocDate = Utility.StringToDate(txtDate.Text);
                DateTime dtDocDate = Utility.StringToDate(strDocumentDate);
                //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

                DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
                dtformat.ShortDatePattern = "MM/dd/yy";
                string strFBDate = "";
                try
                {
                    strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
                }
                catch (Exception ex)
                {
                    DateTime lastDayOfCurrentMonth = new DateTime(dtDocDate.Year, dtDocDate.Month,
                                                        DateTime.DaysInMonth(dtDocDate.Year, dtDocDate.Month));
                    strFBDate = lastDayOfCurrentMonth.ToString(strDateFormat);
                }
                //string strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);

                //string strFBDate = dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year;
                FunPriGenerateRepaymentSchedule(objRepaymentStructure, Utility.StringToDate(strFBDate));
            }
            else
            {
                //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
                //FunPriGenerateRepaymentSchedule(objRepaymentStructure, Utility.StringToDate(txtDate.Text));
                FunPriGenerateRepaymentSchedule(objRepaymentStructure, Utility.StringToDate(strDocumentDate));
                //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end
            }
            /*UMFC has been calculated automatically for other than Product & TermLoan Return Pattern 
             (Also applicable to HP,FL,LN,TE,TL) Updated on 28th Oct 2010*/
            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL") && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            {
                FunPriInsertUMFC();
            }
            if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper() != "PRODUCT"))
            {
                grvRepayStructure.Columns[5].Visible = false;
                //grvRepayStructure.Columns[6].Visible = true;
                //grvRepayStructure.Columns[7].Visible = true;
            }
            else
            {
                grvRepayStructure.Columns[5].Visible = true;
                grvRepayStructure.Columns[6].Visible = false;
                grvRepayStructure.Columns[7].Visible = false;
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.StartsWith("object reference") || ex.Message.StartsWith("Specified cast"))
            {
                Utility.FunShowAlertMsg(this, "Incorrect cashflow Details,Unable to Generate Repayment Structure/Calculate IRR");
            }
            else
            {
                Utility.FunShowAlertMsg(this, ex.Message);
            }

        }
    }

    protected void btnCalIRR_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriIRRReset();
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            decimal decTotalAmount = 0;
            decimal decIRRActualAmount = 0;

            if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one Repayment Details");
                return;
            }

            decimal DecRoundOff;
            decimal decBreakPercent = 0;
            if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                DecRoundOff = Convert.ToDecimal(ViewState["hdnRoundOff"]);
            else
                DecRoundOff = 2;

            if ((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL") || ddlLOB.SelectedItem.Text.ToUpper().Contains("TE")) && ddl_Repayment_Mode.SelectedValue == "2")//Only for structure adhoc
            {
                int intValidation = objRepaymentStructure.FunPubValidateTotalAmountTL((DataTable)ViewState["DtRepayGrid"], txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue, txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "", DecRoundOff);
                if (intValidation == 1)
                {
                    Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                    return;
                }
                else if (intValidation == 2)
                {
                    Utility.FunShowAlertMsg(this, "Principal Amount Should be equal to finance amount (" + txtFinanceAmount.Text + ")");
                    return;
                }
                else if (intValidation == 3)
                {
                    Utility.FunShowAlertMsg(this, "Interest Amount Should be equal to  (" + (decTotalAmount - Convert.ToDecimal(txtFinanceAmount.Text)).ToString() + ")");
                    return;
                }
                else if (intValidation == 6)
                {
                    Utility.FunShowAlertMsg(this, "No Principal and Interest amount entered to calculate");
                    return;
                }
                else if (intValidation == 4)
                {
                    Utility.FunShowAlertMsg(this, "No Principal amount entered to calculate");
                    return;
                }
                else if (intValidation == 5)
                {
                    Utility.FunShowAlertMsg(this, "No Interest amount entered to calculate");
                    return;
                }


                //DataRow[] dr = ((DataTable)ViewState["DtRepayGrid"]).Select("CashFlow_Flag_ID = 23");
                //if (dr.Length > 0)
                //{
                //    decBreakPercent = Convert.ToDecimal(((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID in(23)"));
                //}
                //else
                //{
                //    decBreakPercent = Convert.ToDecimal(((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID in(35,91)"));
                //}
            }
            else if (!ddlLOB.SelectedItem.Text.ToUpper().Contains("TL") && !ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))
            {
                //code commented and added for IRR_StructAdhoc_CR_SISSL12E046_018 by saran on 26-Jul-2014  start
                //if (!objRepaymentStructure.FunPubValidateTotalAmount((DataTable)ViewState["DtRepayGrid"], txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue, (ddl_Return_Pattern.SelectedValue == "2") ? ViewState["decRate"].ToString() : txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "", DecRoundOff))
                //{
                //    Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                //    return;
                //}
                if (ddl_Return_Pattern.SelectedValue != "2" && ddl_Repayment_Mode.SelectedValue != "2")//For IRR with Sruct ad hoc
                {
                    if (!objRepaymentStructure.FunPubValidateTotalAmount((DataTable)ViewState["DtRepayGrid"], txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue, (ddl_Return_Pattern.SelectedValue == "2") ? ViewState["decRate"].ToString() : txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "", DecRoundOff))
                    {
                        Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                        return;
                    }
                }
                //code commented and added for IRR_StructAdhoc_CR_SISSL12E046_018 by saran on 26-Jul-2014  end

                //decBreakPercent = Convert.ToDecimal(((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));

            }

            DataRow[] dRows = ((DataTable)ViewState["DtRepayGrid"]).Select("CashFlow_Flag_ID = 23");
            if (dRows.Length > 0)
            {
                decBreakPercent = Convert.ToDecimal(((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID in(23)"));
            }
            else
            {
                decBreakPercent = Convert.ToDecimal(((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID in(35,91)"));
            }

            if (decBreakPercent != 0)
            {
                if (decBreakPercent != 100)
                {
                    Utility.FunShowAlertMsg(this, "Total break up percentage should be equal to 100%");
                    return;
                }
            }

            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                DataRow[] drFinanAmtRow = ((DataTable)ViewState["DtCashFlowOut"]).Select("CashFlow_Flag_ID = 41");
                if (drFinanAmtRow.Length > 0)
                {
                    decimal decToatlFinanceAmt = (decimal)((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");

                    if (Convert.ToDecimal(txtFinanceAmount.Text) != decToatlFinanceAmt)
                    {
                        Utility.FunShowAlertMsg(this, "Total amount financed in Cashoutflow should be equal to amount financed");
                        FunRepayClear("");
                        return;
                    }
                }
            }
            else
            {

                DataRow[] drFinanAmtRow = ((DataTable)ViewState["DtCashFlowOut"]).Select("CashFlow_Flag_ID = 41");
                if (drFinanAmtRow.Length == 0)
                {
                    DataTable dtOutflow = ((DataTable)ViewState["DtCashFlowOut"]).Clone();

                    DataRow drOutflow = dtOutflow.NewRow();
                    drOutflow["Date"] = Utility.StringToDate(txtDate.Text);
                    drOutflow["CashOutFlow"] = "OL Lease Amount";
                    drOutflow["EntityID"] = hdnCustID.Value;
                    drOutflow["Entity"] = S3GCustomerAddress1.CustomerName;
                    drOutflow["OutflowFromId"] = "144";
                    drOutflow["OutflowFrom"] = "Customer";
                    DataTable dsAssetDetails = (DataTable)Session["PricingAssetDetails"];
                    decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
                    drOutflow["Amount"] = dcmTotalAssetValue;
                    drOutflow["CashOutFlowID"] = "-1";
                    drOutflow["Accounting_IRR"] = true;
                    drOutflow["Business_IRR"] = true;
                    drOutflow["Company_IRR"] = true;
                    drOutflow["CashFlow_Flag_ID"] = "41";
                    dtOutflow.Rows.Add(drOutflow);
                    ViewState["DtCashFlowOut"] = dtOutflow;
                }
            }
            double douAccountingIRR = 0;
            double douBusinessIRR = 0;
            double douCompanyIRR = 0;
            DataTable dtRepaymentStructure = new DataTable();
            DataTable dtMoratorium = null;
            DataTable dtRepayDetails = new DataTable();
            //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
            DataTable dtRepayDetailsOthers = new DataTable();
            //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end
            try
            {

                //Checking if other than normal payment , start date should be last payment date.
                string strStartDte = txtDate.Text;

                DateTime dtDocFBDate = Utility.StringToDate(strStartDte);
                int intDeffered = 0;
                if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")
                {
                    DataTable dtAcctype = ((DataTable)ViewState["PaymentRules"]);
                    dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";
                    string strAcctType = dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString().Trim().ToUpper();

                    if (strAcctType == "PROJECT FINANCE" || strAcctType == "DEFERRED PAYMENT" || strAcctType == "DEFERRED STRUCTURED")
                    {
                        intDeffered = 1;//Defferred Payment
                        DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
                        if (DtCashFlowOut.Rows.Count > 0)
                        {
                            DataRow drOutFlw = DtCashFlowOut.Select("CashFlow_Flag_ID=41").Last();
                            if (drOutFlw != null)
                            {
                                strStartDte = drOutFlw["Date"].ToString();
                                if (!string.IsNullOrEmpty(txtFBDate.Text))
                                {
                                    //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
                                    //DateTime dtDocDate = Utility.StringToDate(txtDate.Text);
                                    DateTime dtDocDate = Utility.StringToDate(strStartDte);
                                    //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

                                    DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
                                    dtformat.ShortDatePattern = "MM/dd/yy";
                                    string strFBDate = "";
                                    try
                                    {
                                        strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
                                    }
                                    catch (Exception ex)
                                    {
                                        DateTime lastDayOfCurrentMonth = new DateTime(dtDocDate.Year, dtDocDate.Month,
                                                                            DateTime.DaysInMonth(dtDocDate.Year, dtDocDate.Month));
                                        strFBDate = lastDayOfCurrentMonth.ToString(strDateFormat);
                                    }
                                    //string strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
                                    dtDocFBDate = Utility.StringToDate(strFBDate);
                                }
                            }
                        }

                    }
                }

                if (((DataTable)ViewState["dtMoratorium"]).Rows.Count == 0)
                {
                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper() != "PRODUCT") && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Contains("ADHOC")))
                    {
                        objRepaymentStructure.FunPubCalculateIRRForTL(strStartDte, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                         , out dtRepaymentStructure, (DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                         , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                         , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                         ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium);
                    }
                    else if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Return_Pattern.SelectedValue == "6"))
                    {
                        objRepaymentStructure.FunPubCalculateIRR(strStartDte, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                                , out dtRepaymentStructure, out dtRepayDetails
                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                            , out dtRepayDetailsOthers
                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                                , (DataTable)ViewState["DtRepayGrid_TL"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                                , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                                , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                                ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium, ddl_Interest_Levy.SelectedValue.ToString(), intDeffered, dtDocFBDate.ToString());
                    }
                    else
                    {
                        objRepaymentStructure.FunPubCalculateIRR(strStartDte, hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                            , out dtRepaymentStructure
                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                            , out dtRepayDetailsOthers
                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                            , (DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                            , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                            , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                            ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium,false);

                    }
                }
                else
                {
                    DataRow[] drFirstRow;

                    if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper() != "PRODUCT"))
                    {
                        DataRow[] dr = ((DataTable)ViewState["DtRepayGrid"]).Select("CashFlow_Flag_ID = 23");
                        if (dr.Length > 0)
                        {
                            drFirstRow = ((DataTable)ViewState["DtRepayGrid"]).Select("CashFlow_Flag_ID = 23 and FromInstall=1");
                        }
                        else
                        {
                            drFirstRow = ((DataTable)ViewState["DtRepayGrid"]).Select("CashFlow_Flag_ID = 91 and FromInstall=1");
                        }
                    }
                    else
                    {
                        drFirstRow = ((DataTable)ViewState["DtRepayGrid"]).Select("CashFlow_Flag_ID = 23 and FromInstall=1");
                    }

                    if (drFirstRow.Length > 0)
                    {
                        if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedValue == "2"))
                        {
                            objRepaymentStructure.FunPubCalculateIRRForTL(DateTime.Parse(drFirstRow[0]["FromDate"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat), hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                            , out dtRepaymentStructure, (DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                            , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                            , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                            ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium);
                        }
                        //else if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && ddl_Repayment_Mode.SelectedValue != "5" && ddl_Return_Pattern.SelectedValue == "6")//Principal
                        //{
                        //     objRepaymentStructure.FunPubCalculateIRR(DateTime.Parse(drFirstRow[0]["FromDate"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat), hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                        //       , out dtRepaymentStructure, (DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                        //       , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                        //       , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                        //       ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium);
                        //}
                        else
                        {
                            objRepaymentStructure.FunPubCalculateIRR(DateTime.Parse(drFirstRow[0]["FromDate"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat), hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualValue.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                               , out dtRepaymentStructure
                                //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                            , out dtRepayDetailsOthers
                                //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                               
                               , (DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                               , txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                               , txtRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, ddl_IRR_Rest.SelectedValue,
                               ddl_Time_Value.SelectedValue, ddlLOB.SelectedItem.Text, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium,false);
                        }
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Add atleat one Repayment Details");
                        return;
                    }
                }
                dtRepaymentStructure.Columns["Charge"].ColumnName = "FinanceCharges";
                ViewState["RepaymentStructure"] = dtRepaymentStructure;
                grvRepayStructure.DataSource = dtRepaymentStructure;
                grvRepayStructure.DataBind();

                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
                if (dtRepayDetailsOthers != null)
                    ViewState["dtRepayDetailsOthers"] = dtRepayDetailsOthers;
                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end

                if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && (ddl_Repayment_Mode.SelectedItem.Text.ToUpper() != "PRODUCT"))
                {
                    grvRepayStructure.Columns[5].Visible = false;
                    //grvRepayStructure.Columns[6].Visible = true;
                    //grvRepayStructure.Columns[7].Visible = true;
                }
                else
                {
                    grvRepayStructure.Columns[5].Visible = true;
                    grvRepayStructure.Columns[6].Visible = false;
                    grvRepayStructure.Columns[7].Visible = false;
                }

                txtAccountIRR_Repay.Text = douAccountingIRR.ToString("0.0000");
                txtAccountingIRR.Text = douAccountingIRR.ToString("0.0000");

                txtBusinessIRR_Repay.Text = douBusinessIRR.ToString("0.0000");
                txtBusinessIRR.Text = douBusinessIRR.ToString("0.0000");

                txtCompanyIRR_Repay.Text = douCompanyIRR.ToString("0.0000");
                txtCompanyIRR.Text = douCompanyIRR.ToString("0.0000");
            }
            catch (Exception Ex1)
            {
                FunRepayClear(Ex1.Message);
            }
            /*UMFC has been calculated automatically for other than Product & TermLoan Return Pattern 
           (Also applicable to HP,FL,LN,TE,TL) Updated on 28th Oct 2010*/
            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL") && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            {
                FunPriInsertUMFC();
            }

        }

        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
            grvRepayStructure.DataSource = null;
            grvRepayStructure.DataBind();
        }
    }

    #endregion

    #region Inflow

    protected void btnAddInflow_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriInsertInflow();
            FunPriSetMaxLength_gvInflow();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Column 'Date, CashInFlowID, InflowFromID, EntityID' is constrained to be unique"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Inflow", "alert('Cash flow cannot be repeated for the same date with same Customer/Entity');", true);
            }
            else
            {
                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem,Unable to Add Inflow";
                cvApplicationProcessing.IsValid = false;
            }
        }
    }

    #endregion

    protected void btnAddOutflow_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            {
                if (ddlPaymentRuleList.SelectedValue == "0" && ddlBusinessOfferNoList.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Payment Rule');", true);
                    TabContainerAP.ActiveTabIndex = 1;
                    return;
                }
                if ((DataTable)ViewState["PaymentRules"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Payment Rule');", true);
                    TabContainerAP.ActiveTabIndex = 1;
                    return;
                }
            }
            FunPriInsertOutflow((DataTable)ViewState["PaymentRules"]);
            //added by saranya
            // Modified by Thalai - Apply condition for Term Loan - Product method too.
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("WORK") ||
                ddlLOB.SelectedItem.Text.ToUpper().Contains("FACT") ||
                ((ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TE") ||
                ddlLOB.SelectedItem.Text.ToUpper().StartsWith("TL")) &&
                ddl_Repayment_Mode.SelectedItem.Text.ToUpper().StartsWith("PRO")))
            {
                btnGenerateRepay_Click(sender, e);
                TabContainerAP.Tabs[2].Enabled = false;
            }
            else
                TabContainerAP.Tabs[2].Enabled = true;

            FunPriSetMaxLength_gvOutFlow();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Column 'Date, CashOutFlowID' is constrained to be unique"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Outflow", "alert('Cash flow cannot be repeated for the same date with same Customer/Entity');", true);
            }
            else if (ex.Message.Contains("Column 'Date, CashOutFlowID, OutflowFromID, EntityID' is constrained to be unique"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Outflow", "alert('Cash flow cannot be repeated for the same date with same Customer/Entity');", true);
            }
            else
            {
                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem,Unable to Add Outflow";
                cvApplicationProcessing.IsValid = false;
            }
        }

    }

    protected void btnFetchROI_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriFetchROIDetails();
            ddlROIRuleList.Focus();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void btnFetchPayment_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPaymentRuleList.SelectedIndex > 0)
            {
                FunPriFetchPaymentDetails();
                if (ViewState["vendor"].ToString().ToUpper() == "VENDOR")
                {
                    DataTable objAssetDetails = (DataTable)Session["PricingAssetDetails"];
                    if (objAssetDetails != null)
                    {
                        if (objAssetDetails.Rows.Count > 0)
                        {
                            DataRow[] drCustomerAsset = objAssetDetails.Select("Pay_To_ID = 138");
                            if (drCustomerAsset.Length > 0)
                            {
                                Utility.FunShowAlertMsg(this, "Asset(s) should be mapped with Entity only");
                                TabContainerAP.ActiveTabIndex = 0;
                                TabContainerMainTab.ActiveTabIndex = 1;
                                return;
                            }
                        }
                    }
                }
                ddlPaymentRuleList.Focus();
            }
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void btnAddRepayment_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriInsertRepayment();
            FunPriSetMaxLength_gvRepaymentDetails();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem, Unable to Add Repayment";
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void btnAddAlert_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriInsertAlert();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void btnAddFollowUp_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriInsertFollowup();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void btnAddGuarantor_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriInsertGuarantor();

        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void btnAddMoratorium_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriInsertMoratorium();

        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }


    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        if (ViewState["Guarantee"] == null)
        {
            if (ViewState["Guarantee"] != "G")
            {
                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                if (hdnCustomerId != null)
                {
                    if (hdnCustomerId.Value != "")
                    {
                        hdnCustID.Value = hdnCustomerId.Value;
                        FunPubQueryExistCustomerListEnquiryUpdation(Convert.ToInt32(hdnCustomerId.Value));

                        Dictionary<string, string> Procparam = new Dictionary<string, string>();
                        Procparam.Add("@Is_Active", "1");
                        Procparam.Add("@User_Id", intUserId.ToString());
                        Procparam.Add("@Company_ID", intCompanyId.ToString());
                        Procparam.Add("@Consitution_Id", hdnConstitutionId.Value);
                        Procparam.Add("@Program_Id", "38");
                        ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

                        FunPriAssignAssetLink();
                        txtCustNameAdd_Followup.Text = S3GCustomerAddress1.CustomerName + "\n" + S3GCustomerAddress1.CustomerAddress;
                    }
                }
            }
        }

    }

    #endregion

    #region GridEvents

    #region Asset Tab
    protected void gvAssetDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveAsset(sender, e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }
    //protected void grvloanasset_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
            
    //        FunPriRemoveDeleteloanAsset(sender, e);
    //    }
    //    catch (Exception ex)
    //    {
    //        cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
    //        cvApplicationProcessing.IsValid = false;
    //    }
    //}
    

    protected void gvAssetDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindAssetDetails(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }
    protected void grvloanassethdr_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindLoanAssetDetails(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    #endregion

    protected void gvInflow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveInflow(e);
            FunPriSetMaxLength_gvInflow();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvInflow_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriAssignInflowDateFormat(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvOutFlow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveOutflow(e);
            FunPriSetMaxLength_gvOutFlow();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvOutFlow_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriAssignOutflowDateFormat(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvRepaymentDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveRepayment(e);
            FunPriSetMaxLength_gvRepaymentDetails();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvRepaymentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindRepaymentDetails(e);
            FunPriSetMaxLength_gvRepaymentDetails();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvRepaymentDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriAssignRepaymentDateFormat(e);
            FunPriSetMaxLength_gvRepaymentDetails();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvAlert_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveAlert(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindAlertDetails(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    private void FunPriBindAlertDetails(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox ChkAlertEmail = e.Row.FindControl("ChkEmail") as CheckBox;
            CheckBox ChkAlertSMS = e.Row.FindControl("ChkSMS") as CheckBox;
            ChkAlertEmail.Enabled = ChkAlertSMS.Enabled = false;
        }
    }

    protected void gvFollowUp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveFollowup(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvFollowUp_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriAssignFollowupDateFormat(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void grvConsDocs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            FunPriBindConstitutionDocuments(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvGuarantor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveGuarantor(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvInvoiceDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindInvoiceDetails(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvGuarantor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindGuarantorDetails(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvMoratorium_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveMoratorium(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void gvPRDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindPRDD(e);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    #endregion

    #region DropDown Events

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlLOB.SelectedValue!="0")
            //{
            //    Procparam = new Dictionary<string, string>();
            //    Procparam.Add("@Company_ID", intCompanyId.ToString());
            //    Procparam.Add("@lob_id", ddlLOB.SelectedValue.ToString());
            //    DataTable dtlob=Utility.GetDefaultData(
            //}
            FunPriToggleLOBBased();
            ddlLOB.Focus();
            Session["PricingloanAssetDetails"] = null;

        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void ddlProductCodeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriToggleProductBased();
            ddlProductCodeList.Focus();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void ddlBusinessOfferNoList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriToggleOfferNoBased();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSalePersonCodeList.Clear();
            //if (ddlBranchList.SelectedIndex > 0)
            if (ddlBranchList.SelectedValue != "0")
            {
                if (ddlBusinessOfferNoList.SelectedIndex == 0)
                {
                    FunPriLoadOfferNo();
                }
                txtBranch_Followup.Text = ddlBranchList.SelectedText;
            }
            ddlBranchList.Focus();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }


    protected void ddl_Time_Value_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_Time_Value.SelectedIndex > 0)
            {
                txtFBDate.Text = "";
                if (ddl_Time_Value.SelectedValue == "1" || ddl_Time_Value.SelectedValue == "2")
                {

                    txtFBDate.Enabled = false;
                    rfvFBDate.Enabled = false;
                    rngFBDate.Enabled = false;
                }
                else
                {
                    rfvFBDate.Enabled = true;
                    txtFBDate.Enabled = true;
                    rngFBDate.Enabled = true;
                }
            }

        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void ddlEntityName_InFlowFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadInflowCustomerEntityDLL();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }

    }

    protected void ddlPaymentto_OutFlow_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadOutflowCustomerEntity();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }



    protected void ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlCashFlowDesc = sender as DropDownList;
            FunPriDoCashflowBasedValidation(ddlCashFlowDesc);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = "Error in fetching values based on cash flow details";
            cvApplicationProcessing.IsValid = false;
        }

    }
    #endregion

    #region TextBox Events

    //protected void txtCollateralTypeRate_OnTextChanged(object sender, EventArgs e)
    //{
    //    if (txtCollateralTypeRate.Text != "" && txtIRRRate.Text != "")
    //        txtRate.Text = Convert.ToString(Convert.ToDecimal(txtIRRRate.Text) + Convert.ToDecimal(txtCollateralTypeRate.Text));
    //}

    protected void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriSetDDLFirstitem(ddlBusinessOfferNoList);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem,Unable to Select Customer";
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void txtCustomerCode_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtCustomerCode.Text))
            {
                S3GCustomerAddress1.SetCustomerDetails("", "", "", "", "", "", "");
                txtCustNameAdd_Followup.Text = "";
                return;
            }
            FunPriToggleCustomerCodeBased();
            FunPriAssignAssetLink();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem, Unable to Load Customer Details";
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void txRepaymentFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtBoxFromdate = (TextBox)sender;
            if (Utility.CompareDates(txtDate.Text, txtBoxFromdate.Text) == -1)
            {
                Utility.FunShowAlertMsg(this, "From Date should be greater than or equal to Application Date");
                return;
            }
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            if (objRepaymentStructure.FunPubGetCashFlowDetails(intCompanyId, Convert.ToInt32(ddlLOB.SelectedValue)).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Define Installment Flag in Cashflow Master for selected Line of Business");
                return;
            }
            FunPriIRRReset();

            FunPriGenerateRepaymentSchedule(objRepaymentStructure, Utility.StringToDate(txtBoxFromdate.Text));
            /*UMFC has been calculated automatically for other than Product & TermLoan Return Pattern 
            (Also applicable to HP,FL,LN,TE,TL) Updated on 28th Oct 2010*/
            if (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL") && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            {
                FunPriInsertUMFC();
            }
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
            //cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix +  "Due to Data Problem,Unable to Generate Repayment Schedule";
            //cvApplicationProcessing.IsValid = false;
        }
    }

    #region ROI - Payment

    protected void txtResidualValue_Cashflow_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 start

            if (txtResidualValue_Cashflow.Text.Trim() != "" && (txtResidualValue_Cashflow.Text.Split('.')[0].Length <= 2))
            {
                rfvResidualValue.Enabled = false;
                //txtResidualAmt_Cashflow.ReadOnly = true;
                if (txtFinanceAmount.Text != "")
                {
                    txtResidualAmt_Cashflow.Text =
                        txtResidualValue.Text = Math.Round(((Convert.ToDecimal(txtResidualValue_Cashflow.Text) * Convert.ToDecimal(txtFinanceAmount.Text)) / 100), 0).ToString();
                }
            }
            else
            {
                rfvResidualValue.Enabled = true;
                txtResidualAmt_Cashflow.Text = "";
                //txtResidualAmt_Cashflow.ReadOnly = false;

            }
            //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 end
            txtResidualValue_Cashflow.Focus();

        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void txtResidualAmt_Cashflow_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //FunPriToggleResidualAmountBased();
            //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 start
            if (txtResidualAmt_Cashflow.Text.Trim() != "")
            {
                if (Convert.ToDecimal(txtResidualAmt_Cashflow.Text.Trim()) >
                    Convert.ToDecimal(txtFinanceAmount.Text.Trim())
                    && (!ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL")))
                {
                    Utility.FunShowAlertMsg(this, "Residual amount should be less than or equal to Finance amount");
                    txtResidualAmt_Cashflow.Text = "";
                    txtResidualValue_Cashflow.Text = "";
                    txtResidualAmt_Cashflow.Focus();
                }
                else
                {
                    rfvResidualValue.Enabled = false;
                    txtResidualValue_Cashflow.ReadOnly = true;
                    txtResidualValue_Cashflow.Text = "";
                    txtResidualValue.Text = txtResidualAmt_Cashflow.Text;
                }
            }
            else
            {
                txtResidualAmt_Cashflow.Text = "";
                rfvResidualValue.Enabled = true;
                txtResidualValue_Cashflow.ReadOnly = false;
            }
            //Code added by saran for observation raised by RS through mail dated on 18-Jan-2012 end

        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    protected void txt_Margin_Percentage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriAssignMarginAmount();
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }

    #endregion

    #endregion

    #region Other Control Events
    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }

    protected void lnkPRDDView_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriShowPRDD(sender);
        }
        catch (Exception ex)
        {
            cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvApplicationProcessing.IsValid = false;
        }
    }
    #endregion

    #endregion


    private void FunPriDoCashflowBasedValidation(DropDownList ddlCashFlowDesc)
    {
        try
        {

            string[] strvalues = ddlCashFlowDesc.SelectedValue.Split(',');
            TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            TextBox txtPerInstallmentAmount_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            TextBox txtBreakup_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;

            if (!ddlLOB.SelectedItem.Text.Contains("TL") && !ddlLOB.SelectedItem.Text.Contains("TE"))
            {
                if (strvalues[4].ToString() != "23")
                {
                    txtFromInstallment_RepayTab1_upd.Attributes.Remove("readonly");
                    txtFromInstallment_RepayTab1_upd.ReadOnly = false;
                    CalendarExtenderSD_ToDate_RepayTab.Enabled = false;
                    CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                    txtfromdate_RepayTab1_Upd.Text = "";
                    txtBreakup_RepayTab1.Text = "";
                    txtBreakup_RepayTab1.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
                    {
                        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                        objRepaymentStructure.dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedValue, Utility.StringToDate(DateTime.Now.ToString(strDateFormat)));
                        if (gvRepaymentDetails.Rows.Count > 0 && txtfromdate_RepayTab1_Upd.Text == "")  // 24 Jan 2012 By Rao. Fixed Observation- From Date Overlapping issue while selecting cashflow. 
                            txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    }
                    else
                    {
                        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                        objRepaymentStructure.FunPubGetNextRepaydate((DataTable)ViewState["DtRepayGrid"], ddl_Frequency.SelectedValue);
                        txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(objRepaymentStructure.intNextInstall + 1);
                        txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    }

                    if (ddl_Rate_Type.SelectedItem.Text.Trim().ToUpper() == "FLOATING")
                    {
                        if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
                        {
                            CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                            txtfromdate_RepayTab1_Upd.ReadOnly = false;
                        }
                        else
                        {
                            CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                            txtfromdate_RepayTab1_Upd.ReadOnly = true;
                        }
                    }
                    else
                    {
                        CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                        txtfromdate_RepayTab1_Upd.ReadOnly = true;
                    }

                    txtFromInstallment_RepayTab1_upd.Attributes.Add("readonly", "readonly");
                    txtBreakup_RepayTab1.Attributes.Remove("readonly");
                    txtFromInstallment_RepayTab1_upd.ReadOnly = true;

                    CalendarExtenderSD_ToDate_RepayTab.Enabled = true;
                    CalendarExtenderSD_ToDate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;


                    CalendarExtenderSD_fromdate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;
                }

            }
            else
            {
                if (strvalues[4].ToString() != "91" && strvalues[4].ToString() != "35")
                {
                    txtFromInstallment_RepayTab1_upd.Attributes.Remove("readonly");
                    txtFromInstallment_RepayTab1_upd.ReadOnly = false;
                    CalendarExtenderSD_ToDate_RepayTab.Enabled = false;
                    CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                    txtfromdate_RepayTab1_Upd.Text = "";
                    txtBreakup_RepayTab1.Text = "";
                    txtBreakup_RepayTab1.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
                    {
                        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                        objRepaymentStructure.dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedValue, Utility.StringToDate(DateTime.Now.ToString(strDateFormat)));
                        if (gvRepaymentDetails.Rows.Count > 0 && txtfromdate_RepayTab1_Upd.Text == "")  // 24 Jan 2012 By Rao. Fixed Observation- From Date Overlapping issue while selecting cashflow. 
                            txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    }
                    else
                    {
                        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                        objRepaymentStructure.FunPubGetNextRepaydateTL((DataTable)ViewState["DtRepayGrid"], ddl_Frequency.SelectedValue, strvalues[4].ToString());
                        txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(objRepaymentStructure.intNextInstall + 1);
                        txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    }

                    if (ddl_Rate_Type.SelectedItem.Text.Trim().ToUpper() == "FLOATING")
                    {
                        //if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
                        //{
                        CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                        txtfromdate_RepayTab1_Upd.ReadOnly = false;
                        //}
                        //else
                        //{
                        //    CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                        //    txtfromdate_RepayTab1_Upd.ReadOnly = true;
                        //}
                    }
                    else
                    {
                        CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                        txtfromdate_RepayTab1_Upd.ReadOnly = false;
                    }
                }
                //txtFromInstallment_RepayTab1_upd.Attributes.Add("readonly", "readonly");
                //txtBreakup_RepayTab1.Attributes.Remove("readonly");
                //txtFromInstallment_RepayTab1_upd.ReadOnly = true;

                CalendarExtenderSD_ToDate_RepayTab.Enabled = true;
                CalendarExtenderSD_ToDate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;


                CalendarExtenderSD_fromdate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw new ApplicationException(ex.Message);
        }
    }



    private void FunPriShowRepaymetDetails(decimal decAmountRepayble)
    {

        if (txtTenure.Text != "" || txtTenure.Text != string.Empty)
        {
            lblTotalAmount.Text = "Total Amount Repayable : " + decAmountRepayble.ToString();
            lblFrequency_Display.Text = "Tenure &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + txtTenure.Text + " " + ddlTenureType.SelectedItem.Text;
            if (txtRate.Text.Trim() != "")
            {
                if (ddl_Return_Pattern.SelectedValue == "2")
                {
                    if (ViewState["decRate"] != null)
                    {
                        lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + ViewState["decRate"].ToString();
                    }
                }
                else
                {
                    lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + txtRate.Text;
                }
            }

        }

    }
    private void FunPriLoadFileNameInPRDDT()
    {
        foreach (GridViewRow grvData in gvPRDDT.Rows)
        {
            Label myThrobber = (Label)grvData.FindControl("myThrobber");
            HiddenField hidThrobber = (HiddenField)grvData.FindControl("hidThrobber");

            if (hidThrobber.Value != "")
            {
                myThrobber.Text = hidThrobber.Value;
            }
        }

    }

    private void FunPriUpdateROIRuleDecRate()//Added on 3/11/2011 by saran for UAT raised mail modify mode not allowing to save forr IRR to flat rate
    {
        DataTable ObjDTROI = new DataTable(); ;
        ObjDTROI = (DataTable)ViewState["ROIRules"];
        decimal decRate = 0;
        switch (ddl_Return_Pattern.SelectedValue)
        {

            case "1":
                decRate = Convert.ToDecimal(txtRate.Text);
                break;
            case "2":
                //ObjCommonBusLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, Convert.ToDecimal(txtFacilityAmt.Text), Convert.ToDouble(9.6365), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decRate, Convert.ToDecimal(10.05), decPLR);
                if (ViewState["decRate"] != null)
                {
                    decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
                }//Hard Coded for testing IRR
                break;
        }
        ObjDTROI.Rows[0]["IRR_Rate"] = decRate;
        //ObjDTROI.Rows[0]["Collateral_Type_Rate"] = txtCollateralTypeRate.Text;
        ObjDTROI.Rows[0].AcceptChanges();
        ViewState["ROIRules"] = ObjDTROI;
    }
    private void FunPriInitializeAssetGridData()
    {
        DataRow dRow;
        dtasset = new DataTable();
        dtasset.Columns.Add("SI.NO");
        dtasset.Columns.Add("Property_Name");
        dRow = dtasset.NewRow();
        dRow["SI.NO"] = "";
        dRow["Property_Name"] = "";
        dtasset.Rows.Add(dRow);
        
      
        ViewState["dtasset"] = dtasset;
        FunFillgrid(grvloanassethdr, dtasset);
        ((DataTable)ViewState["dtasset"]).Rows.RemoveAt(0);
        grvloanassethdr.Rows[0].Visible = false;


    }

    private void FunFillgrid(GridView grv, DataTable dtbl)
    {
        try
        {
            grv.DataSource = dtbl;
            grv.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void grvloanassethdr_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {


            TextBox txtpropertyfooter = (TextBox)grvloanassethdr.FooterRow.FindControl("txtpropertyfooter");
            //LinkButton lnkAssetSerialNo = (LinkButton)grvloanassethdr.FooterRow.FindControl("lnkAssetSerialNo");
           

            DataTable dtasset = (DataTable)ViewState["dtasset"];

            DataRow dr = dtasset.NewRow();

            dr["SI.NO"] = dtasset.Rows.Count + 1;
            dr["Property_Name"] = txtpropertyfooter.Text.ToString();
           

            //ViewState["dtlegalclearance"] = dt;
            dtasset.Rows.Add(dr);

            ViewState["dtasset"] = dtasset;
            grvloanassethdr.DataSource = dtasset;
            grvloanassethdr.DataBind();


        }

    }

}
