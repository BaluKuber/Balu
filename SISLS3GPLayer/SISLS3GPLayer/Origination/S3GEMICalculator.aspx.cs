/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name               : Common 
/// Screen Name               : EMI Calcualtor
/// Created By                : VijayaKumar
/// Created Date              : 19-Sep-2011
/// Purpose                   : 
/// Last Updated By           : Saranya I
/// Last Updated Date         : 28/02/2012
/// Reason                    : To contorl the postbacks,UI changes,To display the Outflow and Inflow Details

/// <Program Summary>
#region NameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Configuration;
using S3GBusEntity.Collection;
using System.Globalization;
using System.Web.Services;
using System.Text;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using S3GAdminServicesReference;
using Microsoft.VisualBasic;

#endregion


public partial class S3GEMICalculator : ApplyThemeForProject
{
    #region Common Variable declaration

    int intCompanyID, intUserID = 0, intSerialNo = 0;
    Dictionary<string, string> Procparam = null;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    public string strDateFormat = string.Empty;
    static string strPageName = "EMI Calculator";
    static string strMessage = @"Correct the following validation(s): </br></br>   ";
    string strNewWin = "window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&NewCustomerID=0', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=620,resizable=yes,scrollbars=yes,top=50,left=50');return false;";

    S3GAdminServicesClient ObjS3GAdminServicesClient = null;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    public string strProgramID = "200";
    int CustomerID;

    DataTable DtAlertDetails = new DataTable();
    DataTable DtFollowUp = new DataTable();
    DataTable DtCashFlow = new DataTable();
    DataTable DtCashFlowOut = new DataTable();
    DataTable DtRepayGrid = new DataTable();
    DataTable DtRepaySummary = new DataTable();

    string strXMLRepayStructure;
    string strXMLCashFlow;
    string strXMLRepayDetails;

    #endregion

    #region Properties
    protected DateTime dtNextDate { get; set; }
    protected int intNextInstall { get; set; }
    #endregion

    # region Page_Load

    /// <summary>
    /// Event for Pre-Initialize the Components  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["IsFromProposal"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
                strProgramID = "42";
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvEMICalculator.ErrorMessage = "Unable to Initialize the EMI Details";
            cvEMICalculator.IsValid = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        S3GSession ObjS3GSession = null;
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPubSetIndex(1);
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            txtAmount.CheckGPSLength(true);
            txtAssetCost.CheckGPSLength(true);
            txtResidualValueAmt.CheckGPSLength(false);
            //txtCorporateTax.CheckGPSLength(false);

            txtResidualPer.SetDecimalPrefixSuffix(2, 2, false, "Residual Value%");
            txtCorporateTax.SetDecimalPrefixSuffix(3, 4, true, "Corporate Tax");
            if (ddlReturnPattern.SelectedIndex > 0)
            {
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                {
                    txtFlatRate.SetDecimalPrefixSuffix(5, 4, true, "Rate");
                }
                else
                {
                    txtFlatRate.SetDecimalPrefixSuffix(2, 4, true, "Rate");
                }
            }
            else
            {
                txtFlatRate.SetDecimalPrefixSuffix(2, 4, true, "Rate");
            }
            txtMarginMoneyPer_Cashflow.SetDecimalPrefixSuffix(2, 4, false);
            txtDate.Attributes.Add("Readonly", "true");
            txtFunder.SetDecimalPrefixSuffix(3, 4, false, "Funder Receipt");
            txtCustPTPA.SetDecimalPrefixSuffix(3, 4, false, "Cust PTPA");
            txtFunderReceipt.CheckGPSLength(true, "Funder Receipt");
            txtOnetimeFee.CheckGPSLength(false, "Onetime Fee");
            txtInterimRent.CheckGPSLength(false, "Interim Rent");
            txtSecurityDeposit.CheckGPSLength(false, "Security Deposit");
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString(strDateFormat);
                //ceDate.Format = strDateFormat;

                FunPubLoadLob();
                FunProGetIRRDetails();
                FunPriTimeDropdown(true, true, true, true);
                FunPriLoadCashflowDetails("0");
                ucCustomerCodeLov.Visible = false;

                FunPriLOBSelectedIndexChange();//default to OL
            }

            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
            txtUserName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtUserName.ToolTip = txtUserName.Text;

            txtQueryNo.Enabled = true;

            if (ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "OL" || ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "FL")
            {
                chkDepreStrucure.Enabled = true;
            }
            else
            {
                chkDepreStrucure.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            cvEMICalculator.ErrorMessage = " Error in loading EMI Calculator";
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {
            ObjS3GSession = null;
        }
    }

    # endregion Page_Load

    # region TextBox_Event Handlers

    /* protected void txtResidualPer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
            if (txtResidualPer.Text.Trim() != "")
            {
                txtResidualValueAmt.ReadOnly = true;
                rfvResidualValueAmt.Enabled = false;
            }
            else
            {
                txtResidualValueAmt.ReadOnly = false;
                rfvResidualValueAmt.Enabled = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void txtResidualValueAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
            if (txtResidualValueAmt.Text.Trim() != "")
            {
                txtResidualPer.ReadOnly = true;
                rfvResidualPer.Enabled = false;
            }
            else
            {
                txtResidualPer.ReadOnly = false;
                rfvResidualPer.Enabled = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
*/

    protected void txRepaymentFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtBoxFromdate = (TextBox)sender;
            FunPriGenerateRepayment(Utility.StringToDate(txtBoxFromdate.Text));

            if (ddlRepaymentMode.SelectedValue != "2")
                if (gvRepaymentDetails.FooterRow != null) gvRepaymentDetails.FooterRow.Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
            txtAssetCost.Text = txtAmount.Text;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void txtMarginMoneyPer_Cashflow_OnTextChanged(object sender, EventArgs e)
    {
        if (txtMarginMoneyPer_Cashflow.Text != string.Empty)
        {
            if (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) > 100)
            {
                txtMarginMoneyPer_Cashflow.Text = txtMarginMoneyAmount_Cashflow.Text = "";
                Utility.FunShowAlertMsg(this, "Margin Percentage should not be greater than 100%");
                return;
            }
            else
            {
                txtMarginMoneyPer_Cashflow.SetDecimalPrefixSuffix(2, 4, false);
                txtMarginMoneyAmount_Cashflow.Text = Convert.ToString(FunPriGetMarginAmout());
            }
        }
        else
            txtMarginMoneyAmount_Cashflow.Text = string.Empty;
    }

    protected void txtCorporateTax_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void txtTenure_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();

            //Condition for - Query number shud not be enabled for New Report.  
            //if (txtTenure.Text != string.Empty && ddlLOB.SelectedIndex > 0)
            if (txtTenure.Text != string.Empty )
                txtQueryNo.Enabled = false;
            else
                txtQueryNo.Enabled = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void txtRecoveryPatternYear1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    # endregion TextBox_Event Handlers

    # region Button_Events

    protected void btnEmail_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtAddress.Text == "" && txtEmail.Text == "" && txtMobile.Text == "")
            {
                cvEMICalculator.ErrorMessage = strMessage + " Enter the Address or Email or Mobile No.";
                cvEMICalculator.IsValid = false;
                return;
            }
            if (grvRepayStructure.Rows.Count == 0)
            {
                cvEMICalculator.ErrorMessage = strMessage + " Repayment structure is not exist for Email";
                cvEMICalculator.IsValid = false;
                return;
            }
            FunPriSave();
            FunPubSentMail(false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //if (txtAddress.Text == "" && txtEmail.Text == "" && txtMobile.Text == "")
        //{
        //    cvEMICalculator.ErrorMessage = strMessage + " Enter the Address or Email or Mobile No.";
        //    cvEMICalculator.IsValid = false;
        //    return;
        //}
        //if (grvRepayStructure.Rows.Count == 0)
        //{
        //    cvEMICalculator.ErrorMessage = strMessage + " Repayment structure is not exist for Print";
        //    cvEMICalculator.IsValid = false;
        //    return;
        //}

        try
        {
            //FunPriSave();
            PreviewPDF_Click(true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        string Lob = ddlLOB.SelectedItem.Text.Split('-')[0].ToString().Trim();
        #region "Recovery Pattern Validations"
        if (ddlRepaymentMode.SelectedValue == "3")
        {
            if (ddlReturnPattern.SelectedValue == "1" || ddlReturnPattern.SelectedValue == "2")//Recovery Pattern should be a percentage
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

            Dictionary<int, decimal> dictRecovery = new Dictionary<int, decimal>();
            if (!string.IsNullOrEmpty(txtRecoveryPatternYear1.Text) && Convert.ToDecimal(txtRecoveryPatternYear1.Text) != 0)
                dictRecovery.Add(1, Convert.ToDecimal(txtRecoveryPatternYear1.Text));

            if (!string.IsNullOrEmpty(txtRecoveryPatternYear2.Text) && Convert.ToDecimal(txtRecoveryPatternYear2.Text) != 0)
                dictRecovery.Add(2, Convert.ToDecimal(txtRecoveryPatternYear2.Text));

            if (!string.IsNullOrEmpty(txtRecoveryPatternYear3.Text) && Convert.ToDecimal(txtRecoveryPatternYear3.Text) != 0)
                dictRecovery.Add(3, Convert.ToDecimal(txtRecoveryPatternYear3.Text));

            if (!string.IsNullOrEmpty(txtRecoveryPatternYearRest.Text) && Convert.ToDecimal(txtRecoveryPatternYearRest.Text) != 0)
                dictRecovery.Add(4, Convert.ToDecimal(txtRecoveryPatternYearRest.Text));

            int inMax = dictRecovery.Keys.Max();
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            int intNoofYears = objRepaymentStructure.FunPubGetNoofYearsFromTenure(ddlTenureType.SelectedItem.Text, txtTenure.Text);

            if (inMax != 4)
            {
                if (inMax != intNoofYears)
                {
                    Utility.FunShowAlertMsg(this, "Tenure and Recovery Pattern are not matching");
                    return;
                }

            }
            else
            {
                if (intNoofYears < 4)
                {
                    Utility.FunShowAlertMsg(this, "Tenure and Recovery Pattern are not matching");
                    return;
                }
            }
        }

        decimal decToatlFinanceAmt = 0;

        //if (((DataTable)ViewState["DtCashFlowOut"]).Select("CashFlow_Flag_ID = '41'") == null || ((DataTable)ViewState["DtCashFlowOut"]).Select("CashFlow_Flag_ID = '41'").Length == 0)
        //{
        //    ScriptManagerAlert("Repayment details", "Add the Amount in OutFlow - Finance Amount");
        //    return;
        //}

        //if (!string.IsNullOrEmpty(Convert.ToString(((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = '41'"))))
        //    decToatlFinanceAmt = (decimal)((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = '41'");
        //else
        //{
        //    cvEMICalculator.ErrorMessage = "<br/> Correct the following validation(s): <br/><br/> Payment Cashflow Description not available in Outflow Details";
        //    cvEMICalculator.IsValid = false;
        //    Utility.FunShowAlertMsg(this, "Payment Cashflow Description not available in Outflow Details");
        //    return;
        //}
        //if (Convert.ToDecimal(txtAmount.Text) != decToatlFinanceAmt)
        //{
        //    ScriptManagerAlert("Repayment details", "Total amount financed in Cashoutflow should be equal to amount financed with margin money");
        //    return;
        //}



        #endregion

        try
        {


            if (ddlRepaymentMode.SelectedValue == "2")
            {
                grvRepayStructure.DataSource = null;
                grvRepayStructure.DataBind();
                ViewState["RepayStructure"] = null;
                FunLoadOutflowGrid(Convert.ToDecimal(txtAmount.Text), "O");

                FunPriCalculateIRR("");
            }
            else
            {
                
                
                bool blnflag = true;
                if (ViewState["DtRepayGrid"] != null)
                {
                    DataTable DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];

                    DataRow[] drselect = DtRepayGrid.Select("CashFlow_Flag_ID <> 23");
                    if (drselect.Length > 0)
                    {
                        blnflag = false;
                    }

                }

                if (blnflag)
                {
                    //txtDate.Text = "";
                    FunPriGridClear();
                    FunPriGenerateRepayment(Utility.StringToDate(txtDate.Text));
                }
                else
                {
                    FunPriCalculateIRR("");
                }

            }
            tcEMI.ActiveTabIndex = 3;


            //OPC Customization on 30-Aug-2014 start 




            decimal decFunderPer = 0, decCIRR1 = 0, decRentalPV = 0;
            decimal decFunderReceipt = 0, decFinanceAmount = 0, decGAP = 0, decOTF = 0, decIntRent = 0, decSecurtiyAmt = 0, decCash = 0, decCashflow = 0;
            if (!string.IsNullOrEmpty(txtFunder.Text))
                decFunderPer = Convert.ToDecimal(txtFunder.Text);
            if (!string.IsNullOrEmpty(txtAccountingIRR.Text))
                decCIRR1 = Convert.ToDecimal(txtAccountingIRR.Text);

            if (!string.IsNullOrEmpty(txtAmount.Text))
                decFinanceAmount = Convert.ToDecimal(txtAmount.Text);
            if (!string.IsNullOrEmpty(txtFunderReceipt.Text))
                decFunderReceipt = Convert.ToDecimal(txtFunderReceipt.Text);


            DataTable dtRepayStructure = new DataTable();
            if (ViewState["RepayStructure"] != null)
                dtRepayStructure = ((DataTable)ViewState["RepayStructure"]).Copy();

            if (dtRepayStructure.Rows.Count > 0)
            {
                if (!dtRepayStructure.Columns.Contains("PTPAPer"))
                    dtRepayStructure.Columns.Add("PTPAPer");
                if (!dtRepayStructure.Columns.Contains("PTPAAmt"))
                    dtRepayStructure.Columns.Add("PTPAAmt");

                decimal decstart = 1, decPTMAMount = 0, decPTMPer = 0;
                foreach (DataRow dr in dtRepayStructure.Rows)
                {
                    decPTMPer = (decstart / (1 + decFunderPer * Convert.ToInt32(dr["NoofDays"]) / 365));
                    decPTMAMount = Convert.ToDecimal(dr["InstallmentAmount"]) * decPTMPer;

                    dr["PTPAPer"] = decPTMPer.ToString();
                    dr["PTPAAmt"] = decPTMAMount.ToString();
                    decRentalPV = decRentalPV + decPTMAMount;
                }
                //Rental PV
                txtRentalPV.Text = decRentalPV.ToString(Funsetsuffix());

            }







            //For gapPer
            txtgapPer.Text = (decFunderPer - decCIRR1).ToString();

            //For Investment
            txtInvestment.Text = (decFunderReceipt - decFinanceAmount).ToString();

            //For GAP
            txtGAP.Text = (((decFinanceAmount - decFunderReceipt) / decFinanceAmount) * decFinanceAmount).ToString();

            if (!string.IsNullOrEmpty(txtGAP.Text))
                decGAP = Convert.ToDecimal(txtGAP.Text);
            if (!string.IsNullOrEmpty(txtOnetimeFee.Text))
                decOTF = Convert.ToDecimal(txtOnetimeFee.Text);
            if (!string.IsNullOrEmpty(txtInterimRent.Text))
                decIntRent = Convert.ToDecimal(txtInterimRent.Text);

            //For Cash
            txtCash.Text = (-decGAP + decOTF + decIntRent).ToString();

            if (!string.IsNullOrEmpty(txtCash.Text))
                decCash = Convert.ToDecimal(txtCash.Text);
            if (!string.IsNullOrEmpty(txtSecurityDeposit.Text))
                decSecurtiyAmt = Convert.ToDecimal(txtSecurityDeposit.Text);

            //For CashFlow
            txtCashflow.Text = (decCash + decSecurtiyAmt).ToString();

            if (!string.IsNullOrEmpty(txtCashflow.Text))
                decCashflow = Convert.ToDecimal(txtCashflow.Text);

            //For CashFlow %
            txtCashflowPer.Text = (decCashflow / decFinanceAmount).ToString();

            //OPC Customization on 30-Aug-2014 End



        }
        catch (Exception ex)
        {
            cvEMICalculator.ErrorMessage = strMessage + ex.Message.ToString();
            cvEMICalculator.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //if (ddlRepaymentMode.SelectedValue != "2" && gvRepaymentDetails.FooterRow != null && Lob != "FL" && Lob != "OL") 
            //if (gvRepaymentDetails.FooterRow != null) gvRepaymentDetails.FooterRow.Visible = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        FunPriClearControls();
    }

    # endregion Button_Events

    # region DropDownList_Events

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLOBSelectedIndexChange();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriLOBSelectedIndexChange()
    {
        FunPubActivityChange();
        FunPriAssignRate(ddlLOB.SelectedValue);
        FunPriGridClear();
        ddlRateType.SelectedValue = ddlRateType.Items[1].Value;
        ddlTime.SelectedValue = ddlTime.Items[2].Value;
        ddlTenureType.SelectedValue = ddlTenureType.Items[1].Value;
        //ddlFrequency.SelectedValue = ddlFrequency.Items[6].Value;
        ddlFrequency.SelectedValue = ddlFrequency.Items[7].Value;
        ddlReturnPattern.SelectedValue = ddlRateType.Items[1].Value;
        ddlReturnPattern_SelectedIndexChanged(null, null);
        ddlRepaymentMode.SelectedValue = ddlRepaymentMode.Items[1].Value;
        txtDepRate.Text = "";


        FunPriLoadCashflowDetails("0");
        FunOutflowInflowFooterFlowType();

        //if (ddlLOB.SelectedIndex > 0)
        //{
            txtQueryNo.Text = string.Empty;
            txtQueryNo.Enabled = false;
        //}
        //else
        //{
        //    txtQueryNo.Enabled = true;
        //}

        if (ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "OL" || ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "FL")
        {

            chkDepreStrucure.Enabled = true;
        }
        else
        {
            chkDepreStrucure.Enabled = false;
        }

        //Image1.Visible = true;
        //txtDate.ReadOnly = false;
        //txtDate.Text = DateTime.Now.ToString(strDateFormat);

        if (ddlLOB.SelectedItem.Text.StartsWith("OL"))
        {
            FunProToggleOLControls(true);
        }
        else
        {
            FunProToggleOLControls(false);
        }
    }

    protected void FunProToggleOLControls(bool blView)
    {
        lblTotalLease.Visible = lblInterestEarned.Visible =
            txtInterestEarned.Visible = txtTotalLease.Visible = blView;
    }

    private void FunOutflowInflowFooterFlowType()
    {
        if (gvOutFlow.FooterRow != null)
        {
            DropDownList ddlFlowType = gvOutFlow.FooterRow.FindControl("ddlFlowType") as DropDownList;
            DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            ddlOutflowDesc.Items.Clear();
            if (ddlFlowType.SelectedValue == "1")
            {
                Utility.FillDLL(ddlOutflowDesc, ((DataTable)ViewState["RepayCashInflowList"]), true);
                SetWhiteSpaceDLL(ddlOutflowDesc);
            }
            else if (ddlFlowType.SelectedValue == "2")
            {
                Utility.FillDLL(ddlOutflowDesc, ((DataTable)ViewState["CashOutflowList"]), true);
                SetWhiteSpaceDLL(ddlOutflowDesc);
            }
        }
    }

    protected void ddlAssetRegister_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
            txtAssetCost.Text = txtAmount.Text;
            txtDepRate.Text = "";
            ddlAssetRegister.ToolTip = ddlAssetRegister.SelectedItem.Text;
            if (ddlAssetRegister.SelectedIndex > 0)
            {
                if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")//For Existing Asset
                {
                    FungetLARassetvalue();
                }
            }

            /* Procparam = new Dictionary<string, string>();
             Procparam.Add("@Company_ID", intCompanyID.ToString());
             if (rdblAssetType.SelectedValue == "0")
                 Procparam.Add("@Asset_ID", ddlAssetRegister.SelectedValue);
             else
                 Procparam.Add("@LAR_Id", ddlAssetRegister.SelectedValue);

             DataTable dt = Utility.GetDefaultData("S3G_LOANAD_GetDepBolckRate", Procparam);

             if (dt.Rows.Count > 0)
             {
                 txtDepRate.Text = dt.Rows[0]["Block_Depreciation_Rate"].ToString();
             }*/
            txtResidualPer.Text = txtResidualValueAmt.Text = string.Empty;
            string strTenure = "0";
            if (!string.IsNullOrEmpty(txtTenure.Text))
                strTenure = txtTenure.Text;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@AssetCategory", ddlAssetRegister.SelectedValue);
            Procparam.Add("@Tenure", strTenure);
            DataTable dt = Utility.GetDefaultData("S3G_GET_RVPERCENTAGE", Procparam);
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["RV_Percentage"].ToString()))
                    txtResidualPer.Text = dt.Rows[0]["RV_Percentage"].ToString();
                if (!string.IsNullOrEmpty(txtAmount.Text) && !string.IsNullOrEmpty(txtResidualPer.Text))
                    txtResidualValueAmt.Text = (Convert.ToDecimal(txtAmount.Text) * Convert.ToDecimal(txtResidualPer.Text) / 100).ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('RV Matrix not defined.');", true);
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ddlRepaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
            FunPriRecoveryPatternMandotary(false);
            txtRecoveryPatternYear1.Text =
            txtRecoveryPatternYear2.Text =
            txtRecoveryPatternYear3.Text =
            txtRecoveryPatternYearRest.Text = "";//cannot be 0
            txtRecoveryPatternYear1.Enabled = txtRecoveryPatternYear2.Enabled =
                txtRecoveryPatternYear3.Enabled = txtRecoveryPatternYearRest.Enabled = false;
            if (Convert.ToInt32(ddlRepaymentMode.SelectedValue) == 3)
            {
                if (ddlReturnPattern.SelectedValue != "1" && ddlReturnPattern.SelectedValue != "2")
                {
                    FunprisetMandatoryRate(false);
                    FunprisetMandatoryBusinessIRR(false);
                }
                else
                {
                    FunprisetMandatoryRate(false);
                    FunprisetMandatoryBusinessIRR(false);
                    if (ddlReturnPattern.SelectedValue == "1")
                        FunprisetMandatoryRate(true);
                    else
                        FunprisetMandatoryBusinessIRR(true);
                }

                FunPriRecoveryPatternMandotary(true);
                txtRecoveryPatternYear1.Enabled =
                txtRecoveryPatternYear2.Enabled =
                txtRecoveryPatternYear3.Enabled =
                txtRecoveryPatternYearRest.Enabled = true;
            }
            else
            {
                FunprisetMandatoryRate(false);
                FunprisetMandatoryBusinessIRR(false);
                if (ddlReturnPattern.SelectedValue != "2")
                    FunprisetMandatoryRate(true);
                else
                    FunprisetMandatoryBusinessIRR(true);
            }
            if (Convert.ToInt32(ddlRepaymentMode.SelectedValue) == 2)
            {
                FunPriBindRepaymentDetails("0");
            }
            else
            {
                gvRepaymentDetails.DataSource = null;
                gvRepaymentDetails.DataBind();
            }

            ddlRepaymentMode.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ddlReturnPattern_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
            FunPriLoadRepayMode();
            ddlReturnPattern.Focus();

            //Condition for - Query number shud not be enabled for New Report. 
            //if (ddlReturnPattern.SelectedIndex > 0 && ddlLOB.SelectedIndex > 0)
            if (ddlReturnPattern.SelectedIndex > 0)
                txtQueryNo.Enabled = false;
            else
                txtQueryNo.Enabled = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadRepayMode()
    {
        string strType = ddlLOB.SelectedItem.Text.Split('-')[0].Trim();
        if (ddlReturnPattern.SelectedValue != "1" && ddlReturnPattern.SelectedValue != "2" && ddlRepaymentMode.SelectedValue == "3")
        {
            FunprisetMandatoryRate(false);
            FunprisetMandatoryBusinessIRR(false);
        }
        else
        {
            if (ddlReturnPattern.SelectedValue == "1")
            {
                FunprisetMandatoryRate(true);
                FunprisetMandatoryBusinessIRR(false);
            }
            else if (ddlReturnPattern.SelectedValue == "2")
            {
                FunprisetMandatoryRate(false);
                FunprisetMandatoryBusinessIRR(true);
            }
        }

        switch (strType.ToLower())
        {
            case "hp"://Hire Purchase
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(true, true, true, false, false);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(true, false, false, false, false);
                break;
            case "ln"://Loan
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(true, true, true, false, false);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(true, false, false, false, false);
                break;
            case "fl"://Financial Lease
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(true, true, true, false, false);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    //Funrepaymode(true, false, true, false, false);
                    Funrepaymode(true, true, true, false, false);//modified on 17-11-2011 by saran based on the observation raised by RS.
                else
                    Funrepaymode(true, false, false, false, false);
                break;
            case "ol"://Operating Lease
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(true, true, true, false, false);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    //Funrepaymode(true, false, true, false, false);
                    Funrepaymode(true, true, true, false, false);//modified on 17-11-2011 by saran based on the observation raised by RS.
                else
                    Funrepaymode(true, false, false, false, false);
                break;
            case "ft"://Factoring
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(false, false, false, true, false);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(false, false, false, false, false);
                break;
            case "tl"://Term Loan                     
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(true, true, true, false, true);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(true, false, false, false, false);
                break;

            case "te"://Term Loan Extendable

                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(true, true, true, false, true);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(true, false, false, false, false);
                break;

            case "wc"://Working Capital
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(false, false, false, true, false);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(false, false, false, false, false);
                break;
            case "vf"://Vendor Finance
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(false, false, false, false, true);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(false, false, false, false, false);
                break;
            case "cf"://Channel Finance
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(false, false, false, false, true);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(false, false, false, false, false);
                break;
            case "sf"://Security Finance
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(false, false, false, false, true);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(false, false, false, false, false);
                break;
            case "of"://Commodity Finance
                if (Convert.ToInt32(ddlReturnPattern.SelectedValue) == 1)
                    Funrepaymode(false, false, false, false, true);
                else if (Convert.ToInt32(ddlReturnPattern.SelectedValue) > 2)
                    Funrepaymode(true, false, true, false, false);
                else
                    Funrepaymode(false, false, false, false, false);
                break;
            default:

                break;
        }
    }

    protected void ddlRateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlTenureType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void chkDepreStrucure_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGridClear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    # endregion DropDownList_Events

    # region Functions

    private void FunPriAssignRate(string strLobId)
    {
        try
        {
            ViewState["hdnRoundOff"] = txtCorporateTax.Text = hdnPLR.Value = "";
            DataTable dtPLR = (DataTable)ViewState["IRRDetails"];
            dtPLR.DefaultView.RowFilter = "LOB_ID = " + strLobId;
            dtPLR = dtPLR.DefaultView.ToTable();
            if (dtPLR.Rows.Count > 0)
            {
                txtCorporateTax.Text = dtPLR.Rows[0]["Corporate_Tax_Rate"].ToString();
                hdnPLR.Value = dtPLR.Rows[0]["Prime_Lending_Rate"].ToString();
                ViewState["hdnRoundOff"] = dtPLR.Rows[0]["Roundoff"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriFillRateTypeDropdown(bool blnFixed, bool blnFloating)
    {
        try
        {
            ddlReturnPattern.Items.FindByValue("1").Enabled = blnFixed;
            ddlReturnPattern.Items.FindByValue("2").Enabled = blnFloating;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriRatePatternDropdown(bool blnRate, bool blnIRR, bool blnPTF, bool blnPLF, bool blnPMF)
    {
        try
        {
            ddlReturnPattern.Items.FindByValue("1").Enabled = blnRate;
            ddlReturnPattern.Items.FindByValue("2").Enabled = blnIRR;
            ddlReturnPattern.Items.FindByValue("3").Enabled = blnPTF;
            ddlReturnPattern.Items.FindByValue("4").Enabled = blnPLF;
            ddlReturnPattern.Items.FindByValue("5").Enabled = blnPMF;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriTimeDropdown(bool blnADV, bool blnARR, bool blnADR, bool blnARF)
    {
        try
        {
            ddlTime.Items.FindByValue("1").Enabled = blnADV;
            ddlTime.Items.FindByValue("2").Enabled = blnARR;
            ddlTime.Items.FindByValue("3").Enabled = false;// blnADR;
            ddlTime.Items.FindByValue("4").Enabled = false;//blnARF;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriFrequencyDropdown(bool blnWeekly, bool blnFortNightly, bool blnMonthly, bool blnBiMonthly, bool blnQuarterly, bool blnHalfYearly, bool blnAnnual)
    {
        try
        {
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
        }
    }

    private void FunPriRepaymentMode(bool blnEI, bool blnSAP, bool blnSF, bool blnProduct, bool blnTL)
    {
        try
        {
            ddlRepaymentMode.Items.FindByValue("1").Enabled = blnEI;
            ddlRepaymentMode.Items.FindByValue("2").Enabled = blnSAP;
            ddlRepaymentMode.Items.FindByValue("3").Enabled = blnSF;
            ddlRepaymentMode.Items.FindByValue("4").Enabled = blnProduct;
            ddlRepaymentMode.Items.FindByValue("5").Enabled = blnTL;
            ddlReturnPattern.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriRecoveryPatternMandotary(bool blnFlag)
    {
        try
        {
            lblRecoveryPatternYear1.Attributes.Add("class", "");
            if (blnFlag)
            {
                lblRecoveryPatternYear1.Attributes.Add("class", "styleReqFieldLabel");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriRateTypeDropdown(bool blnFlag)
    {
        try
        {
            ddlRateType.SelectedValue = "0";
            rfvTime.Enabled = rfvTenureType.Enabled = blnFlag;
            lblTime.Attributes.Add("class", "");
            lblTenure.Attributes.Add("class", "");

            if (blnFlag)
            {
                lblTime.Attributes.Add("class", "styleReqFieldLabel");
                lblTenure.Attributes.Add("class", "styleReqFieldLabel");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPubActivityChange()
    {
        try
        {
            FunPriFillRateTypeDropdown(false, false);
            FunPriRatePatternDropdown(false, false, false, false, false);
            FunPriTimeDropdown(false, false, false, false);
            FunPriFrequencyDropdown(false, false, false, false, false, false, false);
            FunPriRepaymentMode(false, false, false, false, false);

            txtRecoveryPatternYear1.Enabled = txtRecoveryPatternYear2.Enabled = txtRecoveryPatternYear3.Enabled = txtRecoveryPatternYearRest.Enabled = false;

            txtResidualValueAmt.Text = txtResidualPer.Text =
            txtRecoveryPatternYear1.Text = txtRecoveryPatternYear2.Text =
            txtRecoveryPatternYear3.Text = txtRecoveryPatternYearRest.Text =
            txtAccountingIRR.Text = txtBusinessIRR.Text = txtCompanyIRR.Text = "";

            //ddlReturnPattern.SelectedValue = ddlTime.SelectedValue =
            //ddlRepaymentMode.SelectedValue = ddlFrequency.SelectedValue = "0";

            rfvResidualValueAmt.Enabled = rfvResidualPer.Enabled = rfvAssetReg.Enabled = false;
            txtResidualValueAmt.ReadOnly = txtResidualPer.ReadOnly = true;
            txtAmount.ReadOnly = txtAssetCost.ReadOnly = ddlAssetRegister.Enabled = rdblAssetType.Visible = false;
            ddlAssetRegister.Items.Clear();
            //lblAssetRegister.CssClass = "styleDisplayLabel";
            rdblAssetType.SelectedValue = "0";
            rfvAssetCost.Enabled = true;
            rfvTime.Enabled = true;
            rfvFrequency.Enabled = true;

            lblCorporateTax.Text = "Corporate Tax";
            lblDepRate.Text = "Depreciation Rate";
            //rfvCorpTax.Enabled = false;
            //rfvDepreRate.Enabled = false;



            string strType = ddlLOB.SelectedItem.Text.Split('-')[0].Trim();
            switch (strType.ToLower())
            {
                case "hp":  //Hire Purchase
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRepaymentMode(true, true, true, false, false);
                        FunPriRatePatternDropdown(true, true, false, false, false);
                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        ddlAssetRegister.Enabled = true;
                        FunPubLoadAssetDetails();
                        break;
                    }
                case "ln": //Loan
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, true, false, false, false);
                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        FunPriRepaymentMode(true, true, true, false, false);
                        ddlAssetRegister.Enabled = true;
                        rfvAssetCost.Enabled = false;
                        FunPubLoadAssetDetails();
                        break;
                    }


                case "fl":  //Financial Leasing
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, true, true, true, true);
                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        FunPriRepaymentMode(true, true, true, false, false);

                        rfvResidualValueAmt.Enabled = rfvResidualPer.Enabled = true;
                        txtResidualValueAmt.ReadOnly = txtResidualPer.ReadOnly = false;
                        ddlAssetRegister.Enabled = true;
                        FunPubLoadAssetDetails();
                        break;
                    }
                case "ol":  //Operating Lease
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, true, true, true, true);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        FunPriRepaymentMode(true, true, true, false, false);
                        rfvResidualValueAmt.Enabled = rfvResidualPer.Enabled = rfvAssetReg.Enabled = true;
                        txtResidualValueAmt.ReadOnly = txtResidualPer.ReadOnly = false;

                        if (rdblAssetType.SelectedValue == "1")
                            txtAmount.ReadOnly = txtAssetCost.ReadOnly = true;
                        else
                            txtAmount.ReadOnly = txtAssetCost.ReadOnly = false;

                        ddlAssetRegister.Enabled = true;
                        //rdblAssetType.Visible = 
                        FunPubLoadAssetDetails();
                        //rfvCorpTax.Enabled = true;
                        //rfvDepreRate.Enabled = true;
                        lblCorporateTax.Text = "Corporate Tax *";
                        lblDepRate.Text = "Depreciation Rate *";
                        break;
                    }
                case "ft":  //Factoring
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRepaymentMode(false, false, false, true, false);
                        FunPriRatePatternDropdown(true, false, false, false, false);
                        rfvAssetCost.Enabled = false;
                        rfvTime.Enabled = false;
                        rfvFrequency.Enabled = false;
                        break;
                    }
                case "tl":  //Term Loan
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRepaymentMode(true, true, true, false, true);
                        FunPriRatePatternDropdown(true, true, false, false, false);
                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        ddlAssetRegister.Enabled = true;
                        FunPubLoadAssetDetails();

                        break;
                    }
                case "te":  //Term Loan Extentible
                    {

                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRepaymentMode(true, true, true, false, true);
                        FunPriRatePatternDropdown(true, true, false, false, false);

                        FunPriTimeDropdown(true, true, true, true);
                        FunPriFrequencyDropdown(true, true, true, true, true, true, true);
                        ddlAssetRegister.Enabled = true;
                        FunPubLoadAssetDetails();
                        break;
                    }
                case "wc":  //Working Capital
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false);
                        FunPriRepaymentMode(false, false, false, true, false);
                        rfvAssetCost.Enabled = false;
                        rfvTime.Enabled = false;
                        rfvFrequency.Enabled = false;
                        break;
                    }
                case "vf":  //Vendor Finance
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false);

                        FunPriTimeDropdown(true, true, false, false);
                        FunPriFrequencyDropdown(false, false, true, false, true, true, false);
                        FunPriRepaymentMode(false, false, false, false, true);
                        rfvAssetCost.Enabled = false;
                        break;
                    }
                case "cf":  //Channel Finance
                    {
                        FunPriRateTypeDropdown(true);
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false);

                        FunPriTimeDropdown(true, true, false, false);
                        FunPriFrequencyDropdown(false, false, true, false, true, true, false);
                        FunPriRepaymentMode(false, false, false, false, true);
                        rfvAssetCost.Enabled = false;
                        break;
                    }

                case "sf":  //Security Finance
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false);
                        FunPriRepaymentMode(false, false, false, false, true);
                        rfvAssetCost.Enabled = false;
                        break;
                    }
                case "of":  //Commodity Finance
                    {
                        FunPriFillRateTypeDropdown(true, true);
                        FunPriRatePatternDropdown(true, false, false, false, false);
                        FunPriRepaymentMode(false, false, false, false, true);
                        rfvAssetCost.Enabled = false;

                        break;
                    }
                default:
                    {
                        FunPriFillRateTypeDropdown(false, false);
                        FunPriRatePatternDropdown(true, false, false, false, false);
                        FunPriRepaymentMode(false, false, false, false, false);
                        FunPriRecoveryPatternMandotary(false);
                        rfvAssetCost.Enabled = true;
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPubLoadAssetDetails()
    {
        try
        {
            DataTable dtable = new DataTable();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            if (!string.IsNullOrEmpty(strMode))
                Procparam.Add("@mode", strMode);
            if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")
                Procparam.Add("@Existing", "1");
            if (Session["dtPrimaryGrid"] != null)
            {
                dtable = (DataTable)Session["dtPrimaryGrid"];
                DataRow[] dRow;
                DataTable dtUnique = dtable.DefaultView.ToTable(true, "AssetCategory", "AssetCategory_ID");
                dRow = dtUnique.Select("AssetCategory<>''");
                if (dRow.Length>0)
                {
                    ddlAssetRegister.Items.Clear();
                    ddlAssetRegister.DataSource = dtable;
                    ddlAssetRegister.DataTextField = "AssetCategory";
                    ddlAssetRegister.DataValueField = "AssetCategory_ID";
                    ddlAssetRegister.DataBind();
                    ddlAssetRegister.Items.Insert(0, "--Select--");
                }
                else
                {
                    ddlAssetRegister.BindDataTable("S3G_Get_Asset_Details_Enquiry_Updation", Procparam, new string[] { "Asset_ID", "Asset_Description" });
                }
            }
            else
            {
                ddlAssetRegister.BindDataTable("S3G_Get_Asset_Details_Enquiry_Updation", Procparam, new string[] { "Asset_ID", "Asset_Description" });
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPubLoadLob()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Program_Id", strProgramID);

            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, false, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            Procparam = new Dictionary<string, string>();
            DataTable dtDefault = Utility.GetDefaultData("S3G_ORG_GetROIRulesLookUp", Procparam);
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
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_TIME_VALUE'";
                dvSearchView.Sort = "Name Asc";
                dtDefaultNew = dvSearchView.ToTable();
                ddlTime.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_REPAYMENT_MODE'";
                dvSearchView.Sort = "Name Asc";
                dtDefaultNew = dvSearchView.ToTable();
                ddlRepaymentMode.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_RETURN_PATTERN'";
                dvSearchView.Sort = "Name Asc";
                dtDefaultNew = dvSearchView.ToTable();
                ddlReturnPattern.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_ROI_RULES_FREQUENCY'";
                dvSearchView.Sort = "Name Asc";
                dtDefaultNew = dvSearchView.ToTable();
                ddlFrequency.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();
                //ddlFrequency.Items.FindByValue("1").Enabled = false;

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Option", "22");
                ddlTenureType.BindDataTable(SPNames.S3G_ORG_GetPricing_List, Procparam, true, new string[] { "ID", "Name" });


                //For OPC Customization 30-Aug-2014 start 
                ddlFrequency.SelectedValue = "4";//Monthly
                ddlRepaymentMode.SelectedValue = "1";//Equated Install
                ddlReturnPattern.SelectedValue = "3";//default PTF
                //For OPC Customization 30-Aug-2014 end 

            }



        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void rdblAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtAmount.Text = txtAssetCost.Text = "";
            FunPriGridClear();

            if (rdblAssetType.SelectedValue == "1")
            {
                txtAmount.ReadOnly = txtAssetCost.ReadOnly = true;
                rfvResidualPer.Enabled = rfvResidualValueAmt.Enabled = false;
            }
            else
            {
                txtAmount.ReadOnly = txtAssetCost.ReadOnly = false;
                rfvResidualPer.Enabled = rfvResidualValueAmt.Enabled = true;
            }

            FunPubLoadAssetDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FungetLARassetvalue()
    {
        try
        {
            txtAmount.Text = txtAssetCost.Text = "";
            if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@LAR_Id", ddlAssetRegister.SelectedValue);
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")
                    Procparam.Add("@Existing", "1");
                DataTable dt = Utility.GetDefaultData("S3G_Get_Asset_Details_Enquiry_Updation", Procparam);

                if (dt.Rows.Count > 0)
                {
                    txtAmount.Text = txtAssetCost.Text = dt.Rows[0]["Assetvalue"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FunProGetAssetResidual()
    {
        if (ddlFrequency.SelectedValue != "0" && !string.IsNullOrEmpty(txtTenure.Text))
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@LAR_Id", ddlAssetRegister.SelectedValue);
            Procparam.Add("@Company_ID", intCompanyID.ToString());

            DateTime dtEndDate = DateTime.Today;

            switch (ddlTenureType.SelectedValue)
            {
                case "134":   //Months
                    dtEndDate = DateTime.Today.AddMonths(Convert.ToInt32(txtTenure.Text));
                    break;
                case "135":   //Weeks
                    dtEndDate = DateTime.Today.AddDays(Convert.ToInt32(txtTenure.Text) * 7);
                    break;
                case "136":   //Days
                    dtEndDate = DateTime.Today.AddDays(Convert.ToInt32(txtTenure.Text));
                    break;
            }

            Procparam.Add("@CALCULATIONDATE", dtEndDate.ToString());

            DataTable dtRV = Utility.GetDefaultData("S3G_LOANAD_GetDepreciationValue", Procparam);

            if (dtRV != null && dtRV.Rows.Count > 0)
            {
                txtResidualValueAmt.Text = dtRV.Rows[0][0].ToString();
                txtResidualPer.Text = ((Convert.ToDecimal(txtResidualValueAmt.Text) * Convert.ToDecimal(100.00)) / Convert.ToDecimal(txtAmount.Text)).ToString(Utility.SetSuffix());
            }
        }
    }

    public void FunPriSave()
    {
        ObjS3GAdminServicesClient = new S3GAdminServicesClient();
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", CompanyId);

            if (txtQueryNo.Text.Trim() != string.Empty)
            {
                int EMI_ID = Convert.ToInt32(txtQueryNo.Text);
                Procparam.Add("@EMI_Id", EMI_ID.ToString());
            }
            else
            {
                Procparam.Add("@EMI_Id", "0");
            }

            //if (ddlLOB.SelectedIndex > 0)
            //{
                Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
            //}

            if (txtProspectName.Text != "")
            {
                Procparam.Add("@Name", txtProspectName.Text);
            }
            if (txtEmail.Text != "")
            {
                Procparam.Add("@Email", txtEmail.Text);
            }
            if (txtMobile.Text != "")
            {
                Procparam.Add("@Mobile_No", txtMobile.Text);
            }
            if (txtAddress.Text != "")
            {
                Procparam.Add("@Address", txtAddress.Text);
            }
            if (txtAmount.Text != "")
            {
                Procparam.Add("@Finance_Amount", txtAmount.Text);
            }
            if (ddlAssetRegister.SelectedIndex > 0)
            {
                Procparam.Add("@Asset_ID", ddlAssetRegister.SelectedValue);
            }
            if (rdblAssetType.SelectedIndex > 0)
            {
                Procparam.Add("@Asset_Type", "1");
            }
            else
            {
                Procparam.Add("@Asset_Type", "0");
            }

            if (ddlAssetRegister.SelectedIndex > 0)
            {
                Procparam.Add("@LANNo", ddlAssetRegister.SelectedValue);
            }
            if (ddlTenureType.SelectedIndex > 0)
            {
                Procparam.Add("@TenureType", ddlTenureType.SelectedValue);
            }
            if (txtTenure.Text != "")
            {
                Procparam.Add("@Tenure", txtTenure.Text);
            }
            if (ddlReturnPattern.SelectedIndex > 0)
            {
                Procparam.Add("@ReturnPattern", ddlReturnPattern.SelectedValue);
            }
            if (txtAssetCost.Text != "")
            {
                Procparam.Add("@Repayment_Mode", ddlRepaymentMode.SelectedValue);
            }
            if (txtAccountingIRR.Text != "")
            {
                Procparam.Add("@Accounting_IRR", txtAccountingIRR.Text);
            }
            if (txtBusinessIRR.Text != "")
            {
                Procparam.Add("@Business_IRR", txtBusinessIRR.Text);
            }
            if (txtCompanyIRR.Text != "")
            {
                Procparam.Add("@Company_IRR", txtCompanyIRR.Text);
            }
            if (txtFlatRate.Text != "")
            {
                Procparam.Add("@Rate", txtFlatRate.Text);
            }

            Procparam.Add("@Created_By", UserId);

            Procparam.Add("@Created_On", DateTime.Now.ToString());

            //header values ends//

            if (txtCorporateTax.Text != "")
            {
                Procparam.Add("@CTR", txtCorporateTax.Text);
            }
            //if (ddlRateType.SelectedIndex > 0)
            //{
            Procparam.Add("@RateType", "0");
            //}
            if (ddlTime.SelectedIndex > 0)
            {
                Procparam.Add("@Time", ddlTime.SelectedValue);
            }
            if (ddlFrequency.SelectedIndex > 0)
            {
                Procparam.Add("@Frequency", ddlFrequency.SelectedValue);
            }
            if (txtAssetCost.Text != "")
            {
                Procparam.Add("@AssetCost", txtAssetCost.Text);
            }
            if (txtResidualPer.Text != "")
            {
                Procparam.Add("@ResidualPer", txtResidualPer.Text);
            }

            if (txtMarginMoneyPer_Cashflow.Text != "")
            {
                Procparam.Add("@MarginPer", txtMarginMoneyPer_Cashflow.Text);
            }
            if (txtRecoveryPatternYear1.Text != "")
            {
                Procparam.Add("@Recovery_Year1", txtRecoveryPatternYear1.Text);
            }
            if (txtRecoveryPatternYear2.Text != "")
            {
                Procparam.Add("@Recovery_Year2", txtRecoveryPatternYear2.Text);
            }
            if (txtRecoveryPatternYear3.Text != "")
            {
                Procparam.Add("@Recovery_Year3", txtRecoveryPatternYear3.Text);
            }
            if (txtRecoveryPatternYearRest.Text != "")
            {
                Procparam.Add("@Recovery_Remaining", txtRecoveryPatternYearRest.Text);
            }

            if (txtDepRate.Text != "")
            {
                Procparam.Add("@DepreciationRate", txtDepRate.Text);
            }

            if (txtResidualValueAmt.Text != "")
            {
                Procparam.Add("@ResidualAmount", txtResidualValueAmt.Text);
            }

            if (txtMarginMoneyAmount_Cashflow.Text != "")
            {
                Procparam.Add("@MarginAmount", txtMarginMoneyAmount_Cashflow.Text);
            }

            if (chkDepreStrucure.Checked)
            {
                Procparam.Add("@IsDeprecStructure", "1");
            }
            else
            {
                Procparam.Add("@IsDeprecStructure", "0");
            }

            if (ViewState["DtCashFlowOut"] != null)
            {
                strXMLCashFlow = ((DataTable)ViewState["DtCashFlowOut"]).FunPubFormXml();
            }

            strXMLRepayDetails = gvRepaymentDetails.FunPubFormXml();
            strXMLRepayStructure = grvRepayStructure.FunPubFormXml();

            Procparam.Add("@XMLCashflow", strXMLCashFlow.Replace("nbsp;", ""));
            Procparam.Add("@XMLRepayment", strXMLRepayDetails.Replace("nbsp;", ""));
            Procparam.Add("@XML_RepaymentStructure", strXMLRepayStructure.Replace("nbsp;", ""));

            int intResult = ObjS3GAdminServicesClient.FunPubSaveEMICalc(ObjSerMode, ClsPubSerialize.Serialize(Procparam, ObjSerMode));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (ObjS3GAdminServicesClient != null)
                ObjS3GAdminServicesClient.Close();
        }
    }

    public void FunPubSentMail(bool booMail)
    {
        try
        {
            if (strnewFile == "")
                PreviewPDF_Click(false);

            Dictionary<string, string> dictMail = new Dictionary<string, string>();
            dictMail.Add("FromMail", ViewState["CompanyMail"].ToString());
            dictMail.Add("ToMail", txtEmail.Text);
            dictMail.Add("Subject", " Enquiry for " + ddlLOB.SelectedItem.Text.Split('-')[1] + " - " + Utility.StringToDate(txtDate.Text));
            ArrayList arrMailAttachement = new ArrayList();
            StringBuilder strBody = GetHTMLTextEmail();
            //strBody.Append("Test");
            if (strnewFile != "")
            {
                arrMailAttachement.Add(strnewFile);
            }
            Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private StringBuilder GetHTMLTextEmail()
    {
        StringBuilder strMailBodey = new StringBuilder();
        strMailBodey.Append(

            "<font size=\"1\"  color=\"black\" face=\"Times New Roman\">" +

           " <table width=\"100%\">" +

  "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Dear Madam / Sir ," + "</font> " +
            "</td>" +
       " </tr>" +


      "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " This E-mail is in response to the enquiry from you for a line of credit. Please find below the details of the finance sought by you. </font> " +
            "</td>" +
       " </tr>" +

         "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " Please note that :- </font> " +
            "</td>" +
       " </tr>" +


         "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " 1) The statement is not an offer letter and the amounts given are illustrative in nature and may vary after your credit appraisal process. </font> " +
            "</td>" +
       " </tr>" +


         "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " 2) The lending rates/ quote are liable to change without any notice </font> " +
            "</td>" +
       " </tr>" +


         "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " 3) Miscellaneous charges / Taxes do not form part of this statement and may be applicable	 </font> " +
            "</td>" +
       " </tr>" +


         "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " 4) Actual payment and resulting monthly payments may vary depending upon type and use of vehicle, regional credit requirements, and the strength of your credit.</font> " +
            "</td>" +
       " </tr>" +


       "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;</font> " +
            "</td>" +
       " </tr>" +

       "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;</font> " +
            "</td>" +
       " </tr>" +

        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">For further enquiries you may mail us at " + ViewState["CompanyMail"].ToString() + " </font> " +
            "</td>" +
       " </tr>" +
    "</table>" + "</font>");

        return strMailBodey;

    }

    public string strnewFile = string.Empty;

    protected void PreviewPDF_Click(bool blnPrint)
    {
        strnewFile = "";
        try
        {

            if (string.IsNullOrEmpty(txtProspectName.Text))
            {
                cvEMICalculator.ErrorMessage = strMessage + " Enter the Customer Name.";
                cvEMICalculator.IsValid = false;
                return;
            }


            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("Assets");
            dtReport.Columns.Add("Tenor");
            dtReport.Columns.Add("Company_Name");
            dtReport.Columns.Add("Company_Address"); 
            dtReport.Columns.Add("Customer_Name");
            dtReport.Columns.Add("gpssuffix");
            dtReport.Columns.Add("Deal_Value", typeof(decimal));
            dtReport.Columns.Add("Funder", typeof(decimal));
            dtReport.Columns.Add("CIRR1", typeof(decimal));
            dtReport.Columns.Add("Gap_Per", typeof(decimal));
            dtReport.Columns.Add("CIRR2", typeof(decimal));
            dtReport.Columns.Add("Cust_PTPA", typeof(decimal));
            dtReport.Columns.Add("Rental_PV", typeof(decimal));
            dtReport.Columns.Add("Funder_rcpt", typeof(decimal));
            dtReport.Columns.Add("invstmt", typeof(decimal));
            dtReport.Columns.Add("Gap", typeof(decimal));
            dtReport.Columns.Add("Onetime_Fee", typeof(decimal));
            dtReport.Columns.Add("Int_Rent", typeof(decimal));
            dtReport.Columns.Add("Cash", typeof(decimal));
            dtReport.Columns.Add("Secdep", typeof(decimal));
            dtReport.Columns.Add("Cashflow", typeof(decimal));
            dtReport.Columns.Add("CashflowPer", typeof(decimal));


            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());

            DataTable dtCompany = Utility.GetDefaultData("S3G_Get_Company_Details", Procparam);

            DataRow drReport = dtReport.NewRow();

            drReport["Assets"] = ddlAssetRegister.SelectedItem.Text;
            string strTenure = string.Empty;

            strTenure = txtTenure.Text;

            if(ddlTenureType.SelectedItem.Text.ToLower().Trim() != "--select--")
                strTenure=strTenure+" " +ddlTenureType.SelectedItem.Text;

            if (ddlFrequency.SelectedItem.Text.ToLower().Trim() != "--select--")
                strTenure = strTenure + "/" + ddlFrequency.SelectedItem.Text;

            if (ddlTime.SelectedItem.Text.ToLower().Trim() != "--select--")
                strTenure = strTenure + "/" + ddlTime.SelectedItem.Text;

            drReport["Tenor"] = strTenure;


            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                ViewState["CompanyMail"] = dtCompany.Rows[0]["CD_Email_ID"].ToString();
                drReport["Company_Name"] = dtCompany.Rows[0]["Company_Name"].ToString();
                string strAddress = dtCompany.Rows[0]["Address1"].ToString();
                if (dtCompany.Rows[0]["Address2"].ToString() != "") strAddress += ", " + dtCompany.Rows[0]["Address2"].ToString();
                else strAddress += ", " + dtCompany.Rows[0]["City"].ToString();

                strAddress += ", " + dtCompany.Rows[0]["State"].ToString();
                if (dtCompany.Rows[0]["Zip_Code"].ToString() != "") strAddress += "-" + dtCompany.Rows[0]["Zip_Code"].ToString();
                drReport["Company_Address"] = strAddress;
            }
            if (!string.IsNullOrEmpty(txtProspectName.Text))
            drReport["Customer_Name"] = txtProspectName.Text.Trim();

            if (!string.IsNullOrEmpty(txtAmount.Text))
                drReport["Deal_Value"] = Convert.ToDecimal(txtAmount.Text);
            if (!string.IsNullOrEmpty(txtFunder.Text))
                drReport["Funder"] = Convert.ToDecimal(txtFunder.Text);
            if (!string.IsNullOrEmpty(txtAccountingIRR.Text))
                drReport["CIRR1"] = Convert.ToDecimal(txtAccountingIRR.Text);
            if (!string.IsNullOrEmpty(txtgapPer.Text))
                drReport["Gap_Per"] = Convert.ToDecimal(txtgapPer.Text);
            if (!string.IsNullOrEmpty(txtCompanyIRR.Text))
                drReport["CIRR2"] = Convert.ToDecimal(txtCompanyIRR.Text);
            if (!string.IsNullOrEmpty(txtCustPTPA.Text))
                drReport["Cust_PTPA"] = Convert.ToDecimal(txtCustPTPA.Text);
            if (!string.IsNullOrEmpty(txtRentalPV.Text))
                drReport["Rental_PV"] = Convert.ToDecimal(txtRentalPV.Text);
            if (!string.IsNullOrEmpty(txtFunderReceipt.Text))
                drReport["Funder_rcpt"] = Convert.ToDecimal(txtFunderReceipt.Text);
            if (!string.IsNullOrEmpty(txtInvestment.Text))
                drReport["invstmt"] = Convert.ToDecimal(txtInvestment.Text);
            if (!string.IsNullOrEmpty(txtGAP.Text))
                drReport["Gap"] = Convert.ToDecimal(txtGAP.Text);
            if (!string.IsNullOrEmpty(txtOnetimeFee.Text))
                drReport["Onetime_Fee"] = Convert.ToDecimal(txtOnetimeFee.Text);
            if (!string.IsNullOrEmpty(txtInterimRent.Text))
                drReport["Int_Rent"] = Convert.ToDecimal(txtInterimRent.Text);
            if (!string.IsNullOrEmpty(txtCash.Text))
                drReport["Cash"] = Convert.ToDecimal(txtCash.Text);
            if (!string.IsNullOrEmpty(txtSecurityDeposit.Text))
                drReport["Secdep"] = Convert.ToDecimal(txtSecurityDeposit.Text);
            if (!string.IsNullOrEmpty(txtCashflow.Text))
                drReport["Cashflow"] = Convert.ToDecimal(txtCashflow.Text);
            if (!string.IsNullOrEmpty(txtCashflowPer.Text))
                drReport["CashflowPer"] = Convert.ToDecimal(txtCashflowPer.Text);
            S3GSession ObjS3GSession = new S3GSession();
            drReport["gpssuffix"] = ObjS3GSession.ProGpsSuffixRW;
            dtReport.Rows.Add(drReport);

            Guid objGuid;
            objGuid = Guid.NewGuid();
            strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + objGuid + "DANPricing.pdf");

            ReportDocument rptd = new ReportDocument();
            //rptd.Load(Server.MapPath("~/LoanAdmin/EMICalculator.rpt"));
            rptd.Load(Server.MapPath("~/Origination/DanPricing.rpt"));
            rptd.SetDataSource(dtReport);
            rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);

            string strScipt = "";
            if (blnPrint)
            {
                strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strnewFile.Replace(@"\", "/") + "&qsNeedPrint=yes', '_blank','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", strScipt, true);
            }

        }
        catch (IOException winOpne)
        {
            if (blnPrint)
                System.Diagnostics.Process.Start(strnewFile);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriGridClear()
    {
        try
        {
            grvRepayStructure.DataSource = null;
            grvRepayStructure.DataBind();
            ViewState["RepayStructure"] = null;
            ViewState["DtRepayGrid"] = null;
            txtAccountingIRR.Text = txtCompanyIRR.Text = "";
            if (txtBusinessIRR.ReadOnly == true) txtBusinessIRR.Text = "";
            if (txtFlatRate.ReadOnly == true) txtFlatRate.Text = "";

            gvRepaymentDetails.DataSource = null;
            gvRepaymentDetails.DataBind();

            //if (Convert.ToInt32(ddlRepaymentMode.SelectedValue) != 2)
            //{
            //    gvRepaymentDetails.DataSource = null;
            //    gvRepaymentDetails.DataBind();

            //    //ViewState["DtCashFlow"] = null;
            //    //ViewState["CashFlowFrom"] = null;
            //    //ViewState["DtCashFlowOut"] = null;
            //    //ViewState["RepayCashInflowList"] = null;
            //    //ViewState["DtRepayGrid"] = null;
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //private void FunPriGetCTR()
    //{
    //    try
    //    {
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
    //        Procparam.Add("@Lob_ID", ddlLOB.SelectedValue);

    //        txtCorporateTax.Text = Utility.GetTableScalarValue("S3G_Common_EMI_CTR", Procparam);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    #region Grid Functionality

    protected void Repayment_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            DateTime dtTodate;
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            DropDownList ddlRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            // TextBox txtAmountRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtAmountRepaymentCashFlow_RepayTab") as TextBox;
            TextBox txtPerInstallmentAmount_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            TextBox txtBreakup_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;
            TextBox txtFromInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            TextBox txtToInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToInstallment_RepayTab") as TextBox;
            TextBox txtfromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            TextBox txtToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToDate_RepayTab") as TextBox;
            string[] strIds = ddlRepaymentCashFlow_RepayTab1.SelectedValue.ToString().Split(',');
            if (strIds[4] == "23")
            {
                if (DtRepayGrid.Rows.Count > 0)
                {
                    FunPriGetNextRepaydate();
                    if (Utility.StringToDate(txtfromdate_RepayTab1.Text) < dtNextDate)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlapped');", true);
                        return;
                    }
                }
            }

            Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
            objMethodParameters.Add("CashFlow", ddlRepaymentCashFlow_RepayTab1.SelectedItem.Text);
            objMethodParameters.Add("CashFlowId", ddlRepaymentCashFlow_RepayTab1.SelectedValue);
            objMethodParameters.Add("PerInstall", txtPerInstallmentAmount_RepayTab1.Text);
            objMethodParameters.Add("Breakup", txtBreakup_RepayTab1.Text);
            objMethodParameters.Add("FromInstall", txtFromInstallment_RepayTab1.Text);
            objMethodParameters.Add("ToInstall", txtToInstallment_RepayTab1.Text);
            objMethodParameters.Add("FromDate", txtfromdate_RepayTab1.Text);
            objMethodParameters.Add("Frequency", ddlFrequency.SelectedItem.Text); // ddl_Frequency.SelectedItem.Value);
            objMethodParameters.Add("TenureType", ddlTenureType.SelectedItem.Text); //ViewState["TenureType"].ToString());
            objMethodParameters.Add("Tenure", txtTenure.Text); //ViewState["Tenure"].ToString());
            objMethodParameters.Add("DocumentDate", txtDate.Text); //DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat));
            string strErrorMessage = "";
            DateTime dtNextFromdate;
            objRepaymentStructure.FunPubAddRepayment(out dtNextFromdate, out strErrorMessage, out DtRepayGrid, DtRepayGrid, objMethodParameters);
            if (strErrorMessage != "")
            {
                Utility.FunShowAlertMsg(this, strErrorMessage);
                return;
            }

            if (strIds[4] == "23")
            {
                decimal decIRRActualAmount = 0;
                decimal decTotalAmount = 0;
                string strFinAmount = txtAmount.Text.Trim();
                decimal DecRoundOff;
                if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                    DecRoundOff = Convert.ToDecimal(ViewState["hdnRoundOff"]);
                else
                    DecRoundOff = 2;
                //if (!objRepaymentStructure.FunPubValidateTotalAmount(DtRepayGrid, strFinAmount, txtMarginMoneyPer_Cashflow.Text, ddlReturnPattern.SelectedValue, txtFlatRate.Text, ViewState["TenureType"].ToString(), ViewState["Tenure"].ToString(), out decIRRActualAmount, out decTotalAmount, "1"))
                if (!objRepaymentStructure.FunPubValidateTotalAmount(DtRepayGrid, strFinAmount, "0", ddlReturnPattern.SelectedValue, txtFlatRate.Text, ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "1", DecRoundOff))
                {
                    Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                    if (DtRepayGrid.Rows.Count > 0)
                        DtRepayGrid.Rows.RemoveAt(DtRepayGrid.Rows.Count - 1);
                    return;
                }
                if (((decimal)DtRepayGrid.Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23")) > 100)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Breakup Percentage cannot be greater that 100%');", true);
                    if (DtRepayGrid.Rows.Count > 0)
                        DtRepayGrid.Rows.RemoveAt(DtRepayGrid.Rows.Count - 1);
                    return;
                }
            }

            if (DtRepayGrid.Rows.Count > 0)
            {
                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();
            }

            TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
            txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
            TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            //txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            ViewState["DtRepayGrid"] = DtRepayGrid;
            FunPriGenerateNewRepayment();
            FunPriIRRReset();
            FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvEMICalculator.ErrorMessage = ex.Message;
            cvEMICalculator.IsValid = false;
        }
    }

    private void FunPriIRRReset()
    {
        txtAccountingIRR.Text = txtBusinessIRR.Text = txtCompanyIRR.Text = "";
    }

    private void FunPriCalculateSummary(DataTable objDataTable, string strGroupByField, string strSumField)
    {
        try
        {
            DataTable dtSummaryDetails = Utility.FunPriCalculateSumAmount(objDataTable, strGroupByField, strSumField);
            DataTable dtSummaryDtls = new DataTable();
            DataColumn dc1 = new DataColumn("CashFlow_Description");
            DataColumn dc2 = new DataColumn("Amount");
            dtSummaryDtls.Columns.Add(dc1);
            dtSummaryDtls.Columns.Add(dc2);
            if (dtSummaryDetails.Rows.Count > 0)
            {
                for (int i = 0; i < dtSummaryDetails.Rows.Count; i++)
                {
                    DataRow dr = dtSummaryDtls.NewRow();
                    dr["CashFlow_Description"] = dtSummaryDetails.Rows[i]["CashFlow"];
                    dr["Amount"] = dtSummaryDetails.Rows[i]["TotalPeriodInstall"];
                    dtSummaryDtls.Rows.Add(dr);
                }
            }
            //gvRepaymentSummary.DataSource = dtSummaryDtls;
            //gvRepaymentSummary.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error in calculating Summary");
        }
    }

    protected void gvRepaymentDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            if (DtRepayGrid.Rows.Count > 0)
            {
                LessingSummaryAmount(e.RowIndex);
                DtRepayGrid.Rows.RemoveAt(e.RowIndex);

                if (DtRepayGrid.Rows.Count == 0)
                {
                    ViewState["DtRepayGrid"] = DtRepayGrid;
                    gvRepaymentDetails.Rows[0].Cells.Clear();
                    gvRepaymentDetails.Rows[0].Visible = false;
                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    txtFromInstallment_RepayTab1_upd.Text = "1";
                }
                else
                {
                    intSerialNo = 0;
                    gvRepaymentDetails.DataSource = DtRepayGrid;
                    gvRepaymentDetails.DataBind();
                    FunPriBindRepaymentCashflowDetails();

                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
                    FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
                    if (ddlRepaymentMode.SelectedValue != "2")
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
                FunPriIRRReset();
                grvRepayStructure.DataSource = null;
                grvRepayStructure.DataBind();
                ViewState["RepayStructure"] = null;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvEMICalculator.ErrorMessage = ex.Message;
            cvEMICalculator.IsValid = false;
        }
    }

    private void SetWhiteSpaceDLL(DropDownList ObjDLL)
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
            throw ex;
        }
    }

    private void FunPriBindRepaymentCashflowDetails()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 170;
            ObjStatus.Param1 = intCompanyID.ToString();
            //if (ddlLOB.SelectedIndex > 0)
                ObjStatus.Param2 = ddlLOB.SelectedValue.ToString();
            //else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
            //    ObjStatus.Param2 = ViewState["cashlobid"].ToString();
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["RepayCashInflowList"] = DtCashFlow;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }

        try
        {
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;

            CalendarExtenderSD_fromdate_RepayTab1.Format = strDateFormat;
            CalendarExtenderSD_ToDate_RepayTab1.Format = strDateFormat;

            //DropDownList ddlRepaymentCashFlow_RepayTab = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            //Utility.FillDLL(ddlRepaymentCashFlow_RepayTab, ((DataTable)ViewState["RepayCashInflowList"]), true);
            //SetWhiteSpaceDLL(ddlRepaymentCashFlow_RepayTab);

            int count = 0;

            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];

            foreach (GridViewRow gvr in gvRepaymentDetails.Rows)
            {
                DropDownList ddlRepaymentCashFlow_RepayTab1 = gvr.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
                TextBox txtfromdate_RepayTab1 = gvr.FindControl("txtfromdate_RepayTab") as TextBox;
                TextBox txtToDate_RepayTab1 = gvr.FindControl("txtToDate_RepayTab") as TextBox;

                if (ddlRepaymentCashFlow_RepayTab1 != null)
                {
                    txtfromdate_RepayTab1.Text = DateTime.Parse(DtRepayGrid.Rows[count]["FromDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    txtToDate_RepayTab1.Text = DateTime.Parse(DtRepayGrid.Rows[count]["ToDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                    Utility.FillDLL(ddlRepaymentCashFlow_RepayTab1, ((DataTable)ViewState["RepayCashInflowList"]), false);
                    SetSelectItem_DLL(ddlRepaymentCashFlow_RepayTab1, DtRepayGrid.Rows[count]["CashFlow"].ToString());
                    ddlRepaymentCashFlow_RepayTab1.Enabled = false;
                }
                count = count + 1;
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

    private void SetSelectItem_DLL(DropDownList ObjDrop, string str)
    {
        try
        {
            if (!string.IsNullOrEmpty(str))
            {
                ObjDrop.SelectedValue = str;
                ObjDrop.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriGenerateNewRepayment()
    {
        try
        {
            //DropDownList ddlRepaymentCashFlow_RepayTab = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            //Utility.FillDLL(ddlRepaymentCashFlow_RepayTab, ((DataTable)ViewState["RepayCashInflowList"]), true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Cashflow Description in Repayment");
        }
    }

    private void FunPriGetNextRepaydate()
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            DateTime dtNextFromdate = Utility.StringToDate(txtDate.Text); // DateTime.Now;
            DataRow[] drRow = DtRepayGrid.Select("CashFlow_Flag_ID = 23", "ToInstall desc");
            if (drRow.Length > 0)
            {
                dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddlTenureType.SelectedValue, Convert.ToDateTime(drRow[0].ItemArray[8].ToString()));
                intNextInstall = Convert.ToInt32(drRow[0].ItemArray[6].ToString());
            }
            else
            {
                dtNextDate = dtNextFromdate;
                intNextInstall = 0;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriGenerateRepayment(DateTime dtStartDate)
    {
        decimal dcTotalReceivable = 0;
        if (ddlLOB.SelectedItem.Text.StartsWith("OL"))
        {
            if (ddlAssetRegister.SelectedIndex > 0)
            {
                if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")//For Existing Asset
                {
                    FunProGetAssetResidual();
                    FunProGenerateRepay_New(dtStartDate);

                    dcTotalReceivable = Convert.ToDecimal(((DataTable)ViewState["RepayStructure"]).Compute("Sum(InstallmentAmount)", ""));

                    if (!string.IsNullOrEmpty(txtInterestEarned.Text))
                    {
                        txtResidualValueAmt.Text = (Convert.ToDecimal(txtAmount.Text) - (dcTotalReceivable - Convert.ToDecimal(txtInterestEarned.Text))).ToString();
                        txtResidualPer.Text = ((Convert.ToDecimal(txtResidualValueAmt.Text) * Convert.ToDecimal(100.00)) / Convert.ToDecimal(txtAmount.Text)).ToString(Utility.SetSuffix());

                        ViewState["RepayStructure"] = null;

                        FunProGenerateRepay_New(dtStartDate);
                    }
                }
            }
        }

        FunProGenerateRepay_New(dtStartDate);
        dcTotalReceivable = Convert.ToDecimal(((DataTable)ViewState["RepayStructure"]).Compute("Sum(InstallmentAmount)", ""));
        txtTotalLease.Text = dcTotalReceivable.ToString();

    }

    protected void FunProGenerateRepay_New(DateTime dtStartDate)
    {
        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
        try
        {
            string LOBID = "";
            string Lob = "";
            int tenure = Convert.ToInt32(txtTenure.Text);

            //if (ddlLOB.SelectedIndex > 0)
            //{
            LOBID = ddlLOB.SelectedValue;
            Lob = ddlLOB.SelectedItem.Text.Split('-')[0].ToString().Trim();
            //}
            if (objRepaymentStructure.FunPubGetCashFlowDetails(intCompanyID, Convert.ToInt32(LOBID)).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Define cashflow for Installment Payment");
                return;
            }

            Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
            //if (ddlLOB.SelectedIndex > 0)
            objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text);

            objMethodParameters.Add("Tenure", txtTenure.Text);
            objMethodParameters.Add("TenureType", ddlTenureType.SelectedItem.Text);
            string strFinAmount = txtAmount.Text;
            objMethodParameters.Add("FinanceAmount", txtAmount.Text);
            objMethodParameters.Add("ReturnPattern", ddlReturnPattern.SelectedValue);
            objMethodParameters.Add("MarginPercentage", "");

            if (ddlReturnPattern.SelectedValue == "2")
                objMethodParameters.Add("Rate", txtBusinessIRR.Text);
            else
                objMethodParameters.Add("Rate", txtFlatRate.Text);

            objMethodParameters.Add("TimeValue", ddlTime.SelectedValue);
            objMethodParameters.Add("RepaymentMode", ddlRepaymentMode.SelectedValue);
            objMethodParameters.Add("CompanyId", intCompanyID.ToString());
            objMethodParameters.Add("LobId", LOBID);
            objMethodParameters.Add("DocumentDate", txtDate.Text);
            objMethodParameters.Add("Frequency", ddlFrequency.SelectedValue);
            if (txtRecoveryPatternYear1.Text != string.Empty)
                objMethodParameters.Add("RecoveryYear1", txtRecoveryPatternYear1.Text);
            else
                objMethodParameters.Add("RecoveryYear1", "0");
            if (txtRecoveryPatternYear2.Text != string.Empty)
                objMethodParameters.Add("RecoveryYear2", txtRecoveryPatternYear2.Text);
            else
                objMethodParameters.Add("RecoveryYear2", "0");
            if (txtRecoveryPatternYear3.Text != string.Empty)
                objMethodParameters.Add("RecoveryYear3", txtRecoveryPatternYear3.Text);
            else
                objMethodParameters.Add("RecoveryYear3", "0");
            if (txtRecoveryPatternYearRest.Text != string.Empty)
                objMethodParameters.Add("RecoveryYear4", txtRecoveryPatternYearRest.Text);
            else
                objMethodParameters.Add("RecoveryYear4", "0");

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
            //Added by saranya
            FunLoadOutflowGrid(Convert.ToDecimal(txtAmount.Text), "O");
            //end

            DataTable dtMoratoriumInput = FunPubGetMoratorium();
            DataTable dtRepayStr = null;
            if (ddlReturnPattern.SelectedValue == "2")
            {
                if (txtResidualValueAmt.Text.Trim() != "" && txtResidualValueAmt.Text.Trim() != "0")
                {
                    objMethodParameters.Add("decResidualAmount", txtResidualValueAmt.Text);
                }
                if (txtResidualPer.Text.Trim() != "" && txtResidualPer.Text.Trim() != "0")
                {
                    objMethodParameters.Add("decResidualValue", txtResidualPer.Text);
                }
                switch ("1")
                //switch (ddl_IRR_Rest.SelectedValue)
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

                if (((DataTable)ViewState["DtCashFlow"]).Rows.Count > 0)
                    ((DataTable)ViewState["DtCashFlow"]).Rows.Clear();

                DataSet dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"], objMethodParameters, dtMoratoriumInput, out decRateOut);
                ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);
                //txtFlatRate.Text = Math.Round(Convert.ToDouble(decRateOut), 4).ToString();
                txtFlatRate.Text = Convert.ToDouble(decRateOut).ToString();

                dtRepayStr = dsRepayGrid.Tables[1];
                if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
                {
                    DtRepayGrid = dsRepayGrid.Tables[3];
                    ViewState["DtRepayGrid"] = dsRepayGrid.Tables[2];
                }
                else
                {
                    DtRepayGrid = dsRepayGrid.Tables[0];
                    ViewState["DtRepayGrid"] = DtRepayGrid;
                }
            }
            else
            {
                DataSet dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, objMethodParameters, dtMoratoriumInput);

                dtRepayStr = dsRepayGrid.Tables[1];
                if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
                {
                    DtRepayGrid = dsRepayGrid.Tables[3];
                    ViewState["DtRepayGrid"] = dsRepayGrid.Tables[2];
                }
                else
                {
                    DtRepayGrid = dsRepayGrid.Tables[0];
                    ViewState["DtRepayGrid"] = DtRepayGrid;
                }
            }

            if (DtRepayGrid.Rows.Count > 0)
            {
                ViewState["DtRepayGrid"] = DtRepayGrid;
                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();

                if (ddlRateType.SelectedItem.Text == "Floating")
                {
                    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
                    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
                }
                else
                {
                    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
                    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
                }
                FunPriCalculateIRR("");
                FunPriGenerateNewRepayment();
                FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");

                if (ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "OL" || ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "FL")
                {
                    DtRepayGrid = FunPubDepreciationCalc(dtRepayStr, DtRepayGrid);
                    ViewState["DtRepayGridDesc"] = DtRepayGrid;
                    chkDepreStrucure.Enabled = true;
                    if (chkDepreStrucure.Checked)
                    {
                        FunPriCalculateIRR("depreciation");
                    }
                    else
                    {
                        FunPriCalculateIRR("");
                    }
                    FunPriGenerateNewRepayment();
                    FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
                }
                else
                {

                    chkDepreStrucure.Enabled = false;
                }
            }

            strFinAmount = txtAmount.Text;// GetLOBBasedFinAmt();
            decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(strFinAmount, txtMarginMoneyPer_Cashflow.Text);

            if (DtRepayGrid.Rows.Count > 0)
            {
                FunPriShowRepaymetDetails((decimal)DtRepayGrid.Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
            }
            else
            {
                FunPriShowRepaymetDetails(decFinAmount + FunPriGetInterestAmount());
            }
            FillDataFrom_ViewState_CashOutflow();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FunPubGetMoratorium()
    {
        DataTable dtMoratoriumInput = new DataTable();
        //dtMoratoriumInput.Columns.Add("FromDate", System.Type.GetType("System.DateTime"));
        //dtMoratoriumInput.Columns.Add("ToDate", System.Type.GetType("System.DateTime"));
        //dtMoratoriumInput.Columns.Add("Moratoriumtype", System.Type.GetType("System.String"));

        //DataRow drMor = dtMoratoriumInput.NewRow();
        //drMor["FromDate"] = Convert.ToDateTime(DateTime.Now.AddDays(0).ToString().Split(' ')[0]);
        //drMor["ToDate"] = Convert.ToDateTime(DateTime.Now.AddDays(30).ToString().Split(' ')[0]);
        //drMor["Moratoriumtype"] = "INTEREST";
        //dtMoratoriumInput.Rows.Add(drMor);
        return dtMoratoriumInput;
    }


    private void FunLoadOutflowGrid(decimal Amount, string Mode)
    {
        DataTable dtOutflow = new DataTable();
        string flowDesc = "";
        if (ViewState["CashFlows"] != null)
            dtOutflow = (DataTable)ViewState["CashFlows"];
        if (Mode == "O")
            flowDesc = "OUTFLOW";
        else if (Mode == "I")
            flowDesc = "INFLOW";
        if (dtOutflow.Rows.Count > 0)
        {
            List<DataRow> rowsToDelete = new List<DataRow>();

            foreach (DataRow dr in dtOutflow.Rows)
            {
                if (dr["FlowDesc"].ToString().ToUpper() == flowDesc && dr["CashFlow_Flag_ID"].ToString() == "34")
                {
                    rowsToDelete.Add(dr);
                }
                if (Mode == "O")
                {
                    if (dr["FlowDesc"].ToString().ToUpper() == flowDesc && dr["CashFlow_Flag_ID"].ToString() == "41")
                    {
                        rowsToDelete.Add(dr);
                    }
                }

            }
            foreach (DataRow row in rowsToDelete)
            {
                dtOutflow.Rows.Remove(row);
            }
            dtOutflow.AcceptChanges();
        }



        DataRow drOutflow = dtOutflow.NewRow();
        drOutflow["Date"] = Utility.StringToDate(DateTime.Now.ToString(strDateFormat));
        if (Mode == "O")
        {
            drOutflow["CashOutFlow"] = "Finance Amount";
            drOutflow["CashFlow_Flag_ID"] = "41";
            drOutflow["FlowType"] = "61";
            drOutflow["FlowDesc"] = "OutFlow";
        }
        else if (Mode == "I")
        {
            drOutflow["CashOutFlow"] = "UMFC";
            drOutflow["CashFlow_Flag_ID"] = "34";
            drOutflow["FlowType"] = "60";
            drOutflow["FlowDesc"] = "InFlow";
        }
        if (CustomerID > 0)
            drOutflow["EntityID"] = CustomerID.ToString();
        else
            drOutflow["EntityID"] = "-1";
        drOutflow["Entity"] = txtProspectName.Text;//Customer NAme
        drOutflow["OutflowFromId"] = "144";
        drOutflow["OutflowFrom"] = "Customer";
        DataTable dsAssetDetails = null;
        decimal dcmTotalAssetValue = Amount;
        drOutflow["Amount"] = Amount.ToString();
        drOutflow["CashOutFlowID"] = "-1";
        drOutflow["Accounting_IRR"] = true;
        drOutflow["Business_IRR"] = true;
        drOutflow["Company_IRR"] = true;
        dtOutflow.Rows.Add(drOutflow);
        gvOutFlow.DataSource = dtOutflow;
        gvOutFlow.DataBind();

        ViewState["CashFlows"] = dtOutflow;

        ViewState["DtCashFlowOut"] = dtOutflow.Select("CashFlow_Flag_ID='41'").CopyToDataTable();
        DataRow[] drnew = dtOutflow.Select("FlowDesc = 'INFLOW' and CashFlow_Flag_ID<>'34'");
        if (drnew != null && drnew.Length > 0)
            ViewState["DtCashFlow"] = drnew.CopyToDataTable();
    }

    private void FunLoadInflowGrid(decimal InflowAmount)
    {
        DataTable dtInflow = new DataTable();
        if (ViewState["DtCashFlowOut"] != null)
            dtInflow = (DataTable)ViewState["DtCashFlowOut"];
        if (dtInflow.Rows.Count > 0)
        {
            foreach (DataRow dr in dtInflow.Rows)
            {
                dtInflow.BeginInit();
                if (dtInflow.Rows[0]["FlowDesc"].ToString().ToUpper() == "INFLOW")
                {
                    dr.Delete();
                }
                dtInflow.AcceptChanges();
            }
        }
        DataRow drInflow = dtInflow.NewRow();
        drInflow["Date"] = Utility.StringToDate(DateTime.Now.ToString(strDateFormat));
        drInflow["CashInFlow"] = "UMFC";
        if (CustomerID > 0)
            drInflow["EntityID"] = CustomerID.ToString();
        else
            drInflow["EntityID"] = "-1";
        drInflow["Entity"] = txtProspectName.Text;//Customer NAme
        drInflow["InflowFromId"] = "144";
        drInflow["InflowFrom"] = "Customer";
        drInflow["Amount"] = InflowAmount.ToString();
        drInflow["CashInFlowID"] = "-1";
        drInflow["Accounting_IRR"] = true;
        drInflow["Business_IRR"] = true;
        drInflow["Company_IRR"] = true;
        drInflow["CashFlow_Flag_ID"] = "34";
        drInflow["FlowType"] = "60";
        drInflow["FlowDesc"] = "InFlow";
        dtInflow.Rows.Add(drInflow);
        gvOutFlow.DataSource = dtInflow;
        gvOutFlow.DataBind();
        ViewState["DtCashFlow"] = dtInflow;



    }

    protected void FunProGetIRRDetails()
    {
        try
        {
            DataTable dtRepayStructure = Utility.FunPubGetGlobalIRRDetails(intCompanyID, null);
            ViewState["IRRDetails"] = dtRepayStructure;

            bool blnIsVisible = true;
            if (dtRepayStructure.Rows.Count > 0)
            {
                if (dtRepayStructure.Rows[0]["IsIRRApplicable"].ToString() == "True")
                {
                    blnIsVisible = true;
                }
                else
                {
                    blnIsVisible = false;
                }
                txtAccountingIRR.Visible = lblAccountingIRR.Visible =
                txtCompanyIRR.Visible = lblCompanyIRR.Visible =
                txtAccountingIRR.Visible = lblAccountingIRR.Visible = blnIsVisible;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriLoadCashflowDetails(string ResponseID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = null;
        try
        {
            ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 786;
            ObjStatus.Param1 = ResponseID;
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            ViewState["DtCashFlow"] = DtCashFlow;

            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();
            ViewState["CashFlowFrom"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            ObjStatus.Option = 786;
            ObjStatus.Param1 = ResponseID;
            DtCashFlowOut = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            gvOutFlow.DataSource = DtCashFlowOut;
            gvOutFlow.DataBind();

            if (DtCashFlowOut != null && DtCashFlowOut.Rows.Count > 0)
            {
                DtCashFlowOut.Rows.Clear();
                DtCashFlowOut.Dispose();
            }
            ViewState["DtCashFlowOut"] = DtCashFlowOut;
            ViewState["CashFlows"] = DtCashFlowOut;

            gvOutFlow.Rows[0].Cells.Clear();
            gvOutFlow.Rows[0].Visible = false;

            ObjStatus.Option = 311;
            ObjStatus.Param1 = intCompanyID.ToString();
            ObjStatus.Param2 = ddlLOB.SelectedValue.ToString();
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["CashOutflowList"] = DtCashFlow;

            FillDataFrom_ViewState_CashOutflow();

            ObjStatus.Option = 170;
            ObjStatus.Param1 = intCompanyID.ToString();
            //if (ddlLOB.SelectedIndex > 0)
                ObjStatus.Param2 = ddlLOB.SelectedValue.ToString();
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["RepayCashInflowList"] = DtCashFlow;

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (ObjCustomerService != null)
                ObjCustomerService.Close();
        }
    }

    private void FunPriCalculateIRR(string IRRCalType)
    {
        try
        {
            double douAccountingIRR = 0;
            double douBusinessIRR = 0;
            double douCompanyIRR = 0;
            string LOB = "";
            int tenure = Convert.ToInt32(txtTenure.Text);// Convert.ToInt32(ViewState["Tenure"]);
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            decimal decIRRActualAmount = 0;
            decimal decTotalAmount = 0;
            if (IRRCalType.ToUpper() == "DEPRECIATION")
                DtRepayGrid = (DataTable)ViewState["DtRepayGridDesc"];
            else
                DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            if (DtRepayGrid != null)
            {
                if (DtRepayGrid.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, " Repayment details not entered");
                    FunPriBindRepaymentDetails("0");
                    return;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, " Repayment details not entered");
                FunPriBindRepaymentDetails("0");
                return;
            }
            string strRate = txtFlatRate.Text;
            switch (ddlReturnPattern.SelectedValue)
            {
                case "1":
                    strRate = txtFlatRate.Text;
                    break;
                case "2":
                    if (ViewState["decRate"] != null)
                    {
                        strRate = ViewState["decRate"].ToString();
                    }
                    break;
            }
            if (strRate == "")
                strRate = "0";

            decimal DecRoundOff;
            if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                DecRoundOff = Convert.ToDecimal(ViewState["hdnRoundOff"]);
            else
                DecRoundOff = 2;

            string strFinAmount = txtAmount.Text;
            string stroption;
            if (IRRCalType == "")
            {
                if (!objRepaymentStructure.FunPubValidateTotalAmount(DtRepayGrid, strFinAmount, txtMarginMoneyPer_Cashflow.Text, ddlReturnPattern.SelectedValue, strRate,
                    ddlTenureType.SelectedItem.Text, txtTenure.Text, out decIRRActualAmount, out decTotalAmount, "", DecRoundOff))
                {
                    Utility.FunShowAlertMsg(this, "Total amount should be equal to finance amount + interest (" + decTotalAmount + ")");
                    return;
                }
                /*  if (ddlReturnPattern.SelectedIndex > 0)
                  {
                      if(Convert.ToInt32(ddlReturnPattern.SelectedValue) >2)
                          stroption = "3";
                      else
                          stroption = "2";
                  }
                  else
                      stroption = "3";
                  if (!FunPriValidateTotalAmount(out decIRRActualAmount, out decTotalAmount, stroption))
                  {
                      Utility.FunShowAlertMsg(this, "Total amount should be equal to finance amount + interest (" + decTotalAmount + ")");
                      return;
                  }*/

            }

            decimal decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
            if (decBreakPercent != 0)
            {
                if (decBreakPercent != 100)
                {
                    Utility.FunShowAlertMsg(this, "Total break up percentage should be equal to 100%");
                    return;
                }
            }
            LOB = FunPriGetLOBstring();

            DataTable dtRepaymentStructure = new DataTable();

            //objRepaymentStructure.FunPubCalculateIRR(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat), hdnPLR.Value, ddlFrequency.SelectedValue, strDateFormat, txtResidualValueAmt.Text, txtResidualPer.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
            objRepaymentStructure.FunPubCalculateIRR(txtDate.Text, hdnPLR.Value, ddlFrequency.SelectedValue, strDateFormat, txtResidualValueAmt.Text, txtResidualPer.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                , out dtRepaymentStructure, DtRepayGrid, (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                , strFinAmount, txtMarginMoneyPer_Cashflow.Text, ddlReturnPattern.SelectedValue
                , strRate, ddlTenureType.SelectedItem.Text,
                txtTenure.Text, "1", ddlTime.SelectedValue, LOB, ddlRepaymentMode.SelectedValue, "", FunPubGetMoratorium(),false, null, 0);

            // Add the Corportate Tax in repayment Structure
            if (dtRepaymentStructure.Rows.Count > 0)
            {
                grvRepayStructure.DataSource = dtRepaymentStructure;
                grvRepayStructure.DataBind();
                ViewState["RepayStructure"] = dtRepaymentStructure;
                PanRepay.Visible = true;
            }
            else
            {
                PanRepay.Visible = false;
            }

            if ((ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "OL" || ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "FL") && IRRCalType.ToUpper() == "DEPRECIATION")
            {
                //txtAccountingIRR.Text = douAccountingIRR.ToString("0.0000");
                //txtBusinessIRR.Text = douBusinessIRR.ToString("0.0000");
                txtCompanyIRR.Text = douCompanyIRR.ToString("0.0000");
            }
            else
            {
                txtAccountingIRR.Text = douAccountingIRR.ToString("0.0000");
                txtBusinessIRR.Text = douBusinessIRR.ToString("0.0000");
                txtCompanyIRR.Text = douCompanyIRR.ToString("0.0000");
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Incorrect Cash Flow details,cannot calculate IRR");
        }
    }



    private bool FunPriValidateTotalAmount(out decimal decActualAmount, out decimal decTotalAmount, string strOption)
    {
        try
        {
            if (strOption != "3")
            {
                decTotalAmount = FunPriGetAmountFinanced() + Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(ddlTenureType.SelectedItem.Text, FunPriGetAmountFinanced(), Convert.ToDecimal(txtFlatRate.Text), Convert.ToInt32(txtTenure.Text)), 0);
            }
            else
            {
                decTotalAmount = FunPriGetAmountFinanced();
            }
            decActualAmount = 0;
            if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count <= 0)
            {
                cvEMICalculator.ErrorMessage = " Add atleast one row Repayment details";
                cvEMICalculator.IsValid = false;
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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Total Amount");
        }

    }

    public DataTable FunPriGroupRepayDetails(DataTable DtRepaySchedule, DataTable dtRepaymentDetails, int intCashFlow, string strCashflowDesc, bool blnAccountingIRR, bool blnBusinessIRR, bool blnCompanyIRR)
    {
        int counter = 1;
        int iCounter = 1;
        decimal iCurAmt = 0;
        int iToPeriod = 0;
        decimal iPrvAmt = 0;
        int iFromPeriod = 0;
        DataTable DtNewRepaySchedule = DtRepaySchedule.Clone();

        DataRow drRepayRow;

        int j = 1;
        foreach (DataRow grvReapyRow in dtRepaymentDetails.Rows)
        {
            if (iCounter == counter)
            {
                int i = 1;
                foreach (DataRow grvNewReapyRow in dtRepaymentDetails.Rows)
                {

                    if (iCounter == 1)
                    {

                        iCurAmt = Convert.ToDecimal(grvNewReapyRow["InstallmentAmount"].ToString());
                        iPrvAmt = iCurAmt;
                        iFromPeriod = 1;
                        iToPeriod = 1;
                    }
                    else
                    {
                        if (iCounter == i)
                        {

                            iCurAmt = Convert.ToDecimal(grvNewReapyRow["InstallmentAmount"].ToString());
                            if (iCurAmt != iPrvAmt)
                            {
                                goto L1;
                            }
                            else
                            {
                                iToPeriod = iToPeriod + 1;
                            }
                        }
                        else
                        {
                            goto L2;
                        }
                    }
                    iCounter = iCounter + 1;
                L2: ++i;
                }
            L1: drRepayRow = DtNewRepaySchedule.NewRow();
                drRepayRow["SlNo"] = j;//+ DtRepaySchedule.Rows.Count;
                j++;

                drRepayRow["CashFlow"] = strCashflowDesc;
                drRepayRow["Amount"] = dtRepaymentDetails.Rows[0]["Amount"].ToString();
                drRepayRow["PerInstall"] = iPrvAmt;
                drRepayRow["BreakUp"] = 0;
                drRepayRow["FromInstall"] = iFromPeriod;
                drRepayRow["ToInstall"] = iToPeriod;
                drRepayRow["FlowDesc"] = strCashflowDesc;
                drRepayRow["CashFlowID"] = intCashFlow;
                int intTotalInstall = iToPeriod - iFromPeriod + 1;
                drRepayRow["TotalPeriodInstall"] = iPrvAmt * intTotalInstall;
                drRepayRow["Accounting_IRR"] = blnAccountingIRR;
                drRepayRow["Business_IRR"] = blnBusinessIRR;
                drRepayRow["company_IRR"] = blnCompanyIRR;
                drRepayRow["Cashflow_Flag_ID"] = 23;

                if (dtRepaymentDetails.Rows.Count > 1)
                {
                    if (counter < dtRepaymentDetails.Rows.Count)
                    {
                        drRepayRow["FromDate"] = dtRepaymentDetails.Rows[counter]["FromDate"].ToString();
                    }
                    else if (counter == dtRepaymentDetails.Rows.Count)
                        drRepayRow["FromDate"] = dtRepaymentDetails.Rows[counter - 1]["FromDate"].ToString();
                }
                else
                {
                    drRepayRow["FromDate"] = dtRepaymentDetails.Rows[0]["FromDate"].ToString();
                }
                drRepayRow["ToDate"] = dtRepaymentDetails.Rows[iCounter - 2]["ToDate"].ToString();

                DtNewRepaySchedule.Rows.Add(drRepayRow);
                iPrvAmt = iCurAmt;
                iFromPeriod = iCounter;
                iToPeriod = iCounter - 1;
            }
            ++counter;
        }
        return DtNewRepaySchedule;
    }

    private string FunPriGetLOBstring()
    {
        string LOB = "";
        //if (ddlLOB.SelectedIndex > 0)
            LOB = ddlLOB.SelectedItem.Text;
        return LOB;
    }
    /// <summary>
    /// To Get the Inflow and Outflow Details
    /// Added by saranya
    /// </summary>
    /// <param name="decAmountRepayble"></param>
    private void FunPriShowRepaymetDetails(decimal decAmountRepayble)
    {
        try
        {
            decimal InflowAmount = decAmountRepayble - Convert.ToDecimal(txtAmount.Text);
            FunLoadOutflowGrid(InflowAmount, "I");


            /*int tenure = 0;
           tenure = Convert.ToInt32(txtTenure.Text);
           if (ViewState["DtRepayGrid"] != null)
           {
               DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
           }

           if (tenure.ToString() != "" || tenure.ToString() != string.Empty || tenure.ToString() != "0")
           {
              lblTotalAmount.Text = "Total Amount Repayable : " + decAmountRepayble.ToString();
               lblFrequency_Display.Text = "Tenure &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + ViewState["Tenure"].ToString() + " " + ViewState["TenureType"].ToString();
               if (txtFlatRate.Text.Trim() != "")
               {
                   if (ViewState["decRate"] != null)
                   {
                       lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + ViewState["decRate"].ToString();
                   }
                   else
                   {
                       lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + txtFlatRate.Text;
                   }
               }
           }*/
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private decimal FunPriGetAmountFinanced()
    {
        try
        {
            decimal decFinanaceAmt;
            string strFinAmount = txtAmount.Text;// GetLOBBasedFinAmt();
            decFinanaceAmt = Convert.ToDecimal(strFinAmount);//- FunPriGetMarginAmout() ;
            return Math.Round(decFinanaceAmt, 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private decimal FunPriGetInterestAmount()
    {
        try
        {
            decimal decFinAmount = FunPriGetAmountFinanced();
            decimal decRate = 0;
            int tenure = 0;
            string tenuretype = "";
            tenure = Convert.ToInt32(txtTenure.Text);
            tenuretype = ddlTenureType.SelectedValue;

            switch (ddlReturnPattern.SelectedValue)
            {

                case "1":
                    decRate = Convert.ToDecimal(txtFlatRate.Text);
                    break;
                case "2":
                    if (ViewState["decRate"] != null)
                    {
                        decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
                    }//Hard Coded for testing IRR
                    break;
            }
            string strLOB = "";
            //if (ddlLOB.SelectedIndex > 0)
                strLOB = ddlLOB.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower();

            switch (strLOB)
            {
                case "tl":
                case "te":
                    if (ddlRepaymentMode.SelectedValue == "5")
                    {
                        decRate = 0;
                    }
                    break;
                case "ft":
                case "wc":
                    decRate = 0;
                    break;
            }
            return Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(tenuretype.ToLower(), FunPriGetAmountFinanced(), decRate, tenure), 0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvRepaymentDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtfromdate_RepayTab = e.Row.FindControl("txtfromdate_RepayTab") as TextBox;
                txtfromdate_RepayTab.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab = e.Row.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_fromdate_RepayTab.Format = DateFormate; //ObjS3GSession.ProDateFormatRW;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender calext_FromDate = e.Row.FindControl("calext_FromDate") as AjaxControlToolkit.CalendarExtender;
                calext_FromDate.Format = DateFormate;// ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            cvEMICalculator.ErrorMessage = ex.Message;
            cvEMICalculator.IsValid = false;
        }
    }

    protected void gvRepaymentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                intSerialNo += 1;
                e.Row.Cells[0].Text = intSerialNo.ToString();
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlRepaymentCashFlow_RepayTab = e.Row.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
                Utility.FillDLL(ddlRepaymentCashFlow_RepayTab, ((DataTable)ViewState["RepayCashInflowList"]), true);
                SetWhiteSpaceDLL(ddlRepaymentCashFlow_RepayTab);
            }
        }
        catch (Exception ex)
        {
            cvEMICalculator.ErrorMessage = ex.Message;
            cvEMICalculator.IsValid = false;
        }
    }

    private void FunprisetMandatoryRate(bool Blnflag)
    {
        try
        {
            txtFlatRate.Text = "";
            rfvFlatRate.Enabled = Blnflag;
            if (Blnflag)
            {
                //spnFlatRate.InnerText = "*"; //lblFlatRate.Attributes.Add("class", "styleReqFieldLabel");
                txtFlatRate.ReadOnly = false;
                lblFlatRate.Enabled = true;


            }
            else
            {
                //spnFlatRate.InnerText = ""; //lblFlatRate.Attributes.Add("class", "");
                txtFlatRate.ReadOnly = true;
                lblFlatRate.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunprisetMandatoryBusinessIRR(bool Blnflag)
    {
        try
        {
            txtBusinessIRR.Text = "";
            rfvBusinessIRR.Enabled = Blnflag;
            if (Blnflag)
            {
                spnBusinessIRR.InnerText = "*"; //lblFlatRate.Attributes.Add("class", "styleReqFieldLabel");
                txtBusinessIRR.ReadOnly = false;
            }
            else
            {
                spnBusinessIRR.InnerText = ""; //lblFlatRate.Attributes.Add("class", "");
                txtBusinessIRR.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            cvEMICalculator.ErrorMessage = "Error in fetching values based on cash flow details";
            cvEMICalculator.IsValid = false;
        }

    }

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
            if (ddlCashFlowDesc.SelectedValue != "-1" && strvalues[4].ToString() != "23")
            {
                txtFromInstallment_RepayTab1_upd.Attributes.Remove("readonly");
                txtFromInstallment_RepayTab1_upd.ReadOnly = false;
                CalendarExtenderSD_ToDate_RepayTab.Enabled = false;
                CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                txtfromdate_RepayTab1_Upd.Text = "";
                txtBreakup_RepayTab1.Attributes.Add("readonly", "readonly");
            }
            else
            {
                if (ddlTime.SelectedValue == "2" || ddlTime.SelectedValue == "4")
                {
                    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                    objRepaymentStructure.dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddlTenureType.SelectedValue, Utility.StringToDate(txtDate.Text));
                    txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                else
                {
                    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                    objRepaymentStructure.FunPubGetNextRepaydate((DataTable)ViewState["DtRepayGrid"], ddlTenureType.SelectedItem.Value);
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(objRepaymentStructure.intNextInstall + 1);
                    txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                txtFromInstallment_RepayTab1_upd.Attributes.Add("readonly", "readonly");
                txtBreakup_RepayTab1.Attributes.Remove("readonly");
                txtFromInstallment_RepayTab1_upd.ReadOnly = true;

                CalendarExtenderSD_ToDate_RepayTab.Enabled = CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                CalendarExtenderSD_ToDate_RepayTab.Format = CalendarExtenderSD_fromdate_RepayTab.Format = strDateFormat;

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException(ex.Message);
        }
    }

    private void LessingSummaryAmount(int RowIdx)
    {
        try
        {
            string FlowDesc = string.Empty;
            DataTable DtRepaydetails = new DataTable();
            DtRepaydetails = (DataTable)ViewState["DtRepayGrid"];
            DtRepaySummary = (DataTable)ViewState["RepaymentSummary"];
            FlowDesc = Convert.ToString(DtRepaydetails.Rows[RowIdx]["CashFlow"]);

            if (DtRepaySummary != null)
            {
                for (int Idx = 0; Idx <= DtRepaySummary.Rows.Count - 1; Idx++)
                {
                    if (FlowDesc == Convert.ToString(DtRepaySummary.Rows[Idx]["CashFlow_Description"]))
                    {
                        if (Convert.ToInt64(DtRepaySummary.Rows[Idx]["Amount"]) == Convert.ToInt64(DtRepaydetails.Rows[RowIdx]["Amount"]))
                        {
                            DtRepaySummary.Rows.RemoveAt(Idx);
                            //gvRepaymentSummary.DataSource = DtRepaySummary;
                            //gvRepaymentSummary.DataBind();
                            ViewState["RepaymentSummary"] = DtRepaySummary;
                        }
                        else
                        {
                            DtRepaySummary.Rows[Idx]["Amount"] = Convert.ToInt64(DtRepaySummary.Rows[Idx]["Amount"]) - Convert.ToInt64(DtRepaydetails.Rows[RowIdx]["Amount"]);
                            DtRepaySummary.AcceptChanges();
                            //gvRepaymentSummary.DataSource = DtRepaySummary;
                            //gvRepaymentSummary.DataBind();
                            ViewState["RepaymentSummary"] = DtRepaySummary;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriBindRepaymentDetails(string ResponseID)
    {
        try
        {
            OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            if (ResponseID == "0")
            {
                ObjStatus.Option = 52;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                intSerialNo = 0;
                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();

                DtRepayGrid.Rows.Clear();
                ViewState["DtRepayGrid"] = DtRepayGrid;

                gvRepaymentDetails.Rows[0].Cells.Clear();
                gvRepaymentDetails.Rows[0].Visible = false;

                RepaymentSummaryGrid(ResponseID);
            }
            else
            {
                ObjStatus.Option = 159;
                ObjStatus.Param1 = ResponseID;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                intSerialNo = 0;
                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();

                ViewState["DtRepayGrid"] = DtRepayGrid;

                RepaymentSummaryGrid(ResponseID);
            }

            if (gvRepaymentDetails.Rows.Count > 0)
            {
                FunPriBindRepaymentCashflowDetails();

                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;

                CalendarExtenderSD_fromdate_RepayTab1.Format = strDateFormat;
                CalendarExtenderSD_ToDate_RepayTab1.Format = strDateFormat;

                TextBox txtfromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
                TextBox txtToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToDate_RepayTab") as TextBox;

                txtfromdate_RepayTab1.Attributes.Add("readonly", "readonly");
                txtToDate_RepayTab1.Attributes.Add("readonly", "readonly");
            }

            ObjCustomerService.Close();

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

    private void RepaymentSummaryGrid(string ResponseID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 168;
            ObjStatus.Param1 = ResponseID;
            DtRepaySummary = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            //gvRepaymentSummary.DataSource = DtRepaySummary;
            //gvRepaymentSummary.DataBind();

            ViewState["RepaymentSummary"] = DtRepaySummary;
            ObjCustomerService.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (ObjCustomerService != null) ObjCustomerService.Close();
        }
    }

    #endregion

    private decimal FunPriGetMarginAmout()
    {
        decimal decMarginAmount;
        if (txtMarginMoneyPer_Cashflow.Text != "")
        {
            decMarginAmount = (Convert.ToDecimal(txtAssetCost.Text) * (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) / 100));
        }
        else
        {
            decMarginAmount = 0;
        }
        return decMarginAmount;
    }

    private void FunPriClearControls()
    {
        try
        {
            txtProspectName.Text = txtEmail.Text = txtAddress.Text = txtMobile.Text = txtQueryNo.Text = "";
            ((TextBox)ucCustomerCodeLov.FindControl("txtName")).Text = string.Empty;
            ((TextBox)ucCustomerCodeLov.FindControl("txtName")).ToolTip = string.Empty;
            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            hdnCID.Value = null;
            //if (ddlLOB.Items.Count > 0) ddlLOB.SelectedIndex = 0;
            txtDate.Text = DateTime.Now.ToString(strDateFormat);
            txtCorporateTax.Text = txtAmount.Text = txtAssetCost.Text = txtDepRate.Text = "";
            txtAmount.ReadOnly = false;

            //ddlAssetRegister.Items.Clear();
            //ddlAssetRegister.Enabled = rdblAssetType.Visible = false;

            //FunPubLoadAssetDetails();

            ddlAssetRegister.SelectedIndex = -1;

            if (ddlRateType.Items.Count > 0) ddlRateType.SelectedIndex = 0;
            if (ddlReturnPattern.Items.Count > 0) ddlReturnPattern.SelectedIndex = 0;
            if (ddlTenureType.Items.Count > 0) ddlTenureType.SelectedIndex = 0;
            txtTenure.Text = "";
            if (ddlTime.Items.Count > 0) ddlTime.SelectedIndex = 0;
            if (ddlFrequency.Items.Count > 0) ddlFrequency.SelectedIndex = 0;
            if (ddlRepaymentMode.Items.Count > 0) ddlRepaymentMode.SelectedIndex = 0;
            txtResidualPer.Text = txtResidualValueAmt.Text =
                txtMarginMoneyAmount_Cashflow.Text = txtMarginMoneyPer_Cashflow.Text = "";
            txtResidualPer.ReadOnly = txtResidualValueAmt.ReadOnly = true;
            txtRecoveryPatternYear1.Text = txtRecoveryPatternYear2.Text = txtRecoveryPatternYear3.Text =
                txtRecoveryPatternYearRest.Text = "";
            txtRecoveryPatternYear1.Enabled = txtRecoveryPatternYear2.Enabled = txtRecoveryPatternYear3.Enabled =
                txtRecoveryPatternYearRest.Enabled = false;
            txtFlatRate.Text = txtAccountingIRR.Text = txtBusinessIRR.Text = txtCompanyIRR.Text = "";
            txtAccountingIRR.ReadOnly = txtBusinessIRR.ReadOnly = txtCompanyIRR.ReadOnly = true;
            FunPriGridClear();

            FunprisetMandatoryRate(true);
            FunprisetMandatoryBusinessIRR(false);
            FunPriRecoveryPatternMandotary(false);

            gvOutFlow.DataSource = null;
            gvOutFlow.DataBind();

            txtQueryNo.Enabled = true;
            chkDepreStrucure.Checked = false;
            chkDepreStrucure.Enabled = false;

            //Image1.Visible = true;
            lblCorporateTax.Text = "Corporate Tax";
            lblDepRate.Text = "Depreciation Rate";

            txtFunder.Text = txtCustPTPA.Text = txtFunderReceipt.Text = txtOnetimeFee.Text = txtInterimRent.Text =
                txtSecurityDeposit.Text = txtgapPer.Text = txtRentalPV.Text = txtInvestment.Text = txtGAP.Text =
                txtCash.Text = txtCashflow.Text = txtCashflowPer.Text = string.Empty;









        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        // S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        // suffix = 0;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    # endregion Functions

    #region Outflow Grid

    protected void gvOutFlow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtCashFlowOut = (DataTable)ViewState["CashFlows"];
            if (DtCashFlowOut.Rows.Count > 0)
            {
                DtCashFlowOut.Rows.RemoveAt(e.RowIndex);
                ViewState["DtRepayGrid"] = null;
                if (DtCashFlowOut.Rows.Count == 0)
                {
                    ViewState["CashFlows"] = DtCashFlowOut;
                    gvOutFlow.Rows[0].Cells.Clear();
                    gvOutFlow.Rows[0].Visible = false;
                    ViewState["DtCashFlowOut"] = DtCashFlowOut;
                }
                else
                {
                    gvOutFlow.DataSource = DtCashFlowOut;
                    gvOutFlow.DataBind();
                    FillDataFrom_ViewState_CashOutflow();
                }
            }
        }
        catch (Exception ex)
        {
            cvEMICalculator.ErrorMessage = ex.Message;
            cvEMICalculator.IsValid = false;
        }
    }

    protected void gvOutFlow_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridOutflow = e.Row.FindControl("txtDate_GridOutflow") as TextBox;
                txtDate_GridOutflow.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_OutflowDate = e.Row.FindControl("CalendarExtenderSD_OutflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_OutflowDate.Format = strDateFormat;
            }
        }
        catch (Exception ex)
        {
            cvEMICalculator.ErrorMessage = ex.Message;
            cvEMICalculator.IsValid = false;
        }

    }

    private void FillDataFrom_ViewState_CashOutflow()
    {
        try
        {
            TextBox txtDate_GridOutflow2 = gvOutFlow.FooterRow.FindControl("txtDate_GridOutflow") as TextBox;
            txtDate_GridOutflow2.Attributes.Add("readonly", "readonly");

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_OutflowDate1 = gvOutFlow.FooterRow.FindControl("CalendarExtenderSD_OutflowDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_OutflowDate1.Format = strDateFormat;

            DropDownList ddlInflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            DropDownList ddlFlowType = gvOutFlow.FooterRow.FindControl("ddlFlowType") as DropDownList;

            ddlFlowType.Items.Clear();

            ddlFlowType.Items.Add(new ListItem("Select", "0"));
            ddlFlowType.Items.Add(new ListItem("InFlow", "1"));
            ddlFlowType.Items.Add(new ListItem("OutFlow", "2"));

            int count = 0;
            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];

            foreach (GridViewRow gvr in gvOutFlow.Rows)
            {
                TextBox txtDate_GridOutflow = gvr.FindControl("txtDate_GridOutflow") as TextBox;
                DropDownList ddlOutflowDesc = gvr.FindControl("ddlOutflowDesc") as DropDownList;
                TextBox txtAmount_Outflow = gvr.FindControl("txtAmount_Outflow") as TextBox;

                if (ddlOutflowDesc != null)
                {
                    Utility.FillDLL(ddlOutflowDesc, ((DataTable)ViewState["CashOutflowList"]), false);
                    txtDate_GridOutflow.Text = DateTime.Parse(DtCashFlowOut.Rows[count]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);
                    SetSelectItem_DLL(ddlOutflowDesc, DtCashFlowOut.Rows[count]["CashOutFlowID"].ToString());
                    txtAmount_Outflow.Text = DtCashFlowOut.Rows[count]["Amount"].ToString();

                    txtDate_GridOutflow.ReadOnly = true;
                    txtAmount_Outflow.ReadOnly = true;
                }
                count = count + 1;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void CashOutflow_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {

            DtCashFlowOut = (DataTable)ViewState["CashFlows"];

            if (DtCashFlowOut != null && DtCashFlowOut.Select("CashFlow_Flag_ID= 0").Length > 0)
            {
                DtCashFlowOut.Rows.RemoveAt(DtCashFlowOut.Rows.Count - 1);
            }

            TextBox txtDate_GridOutflow = gvOutFlow.FooterRow.FindControl("txtDate_GridOutflow") as TextBox;
            DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            DropDownList ddlFlowType = gvOutFlow.FooterRow.FindControl("ddlFlowType") as DropDownList;
            TextBox txtAmount_Outflow = gvOutFlow.FooterRow.FindControl("txtAmount_Outflow") as TextBox;

            DataRow dr = DtCashFlowOut.NewRow();

            dr["Date"] = Utility.StringToDate(txtDate_GridOutflow.Text);
            string[] strArrayIds = ddlOutflowDesc.SelectedValue.Split(',');
            dr["CashOutFlowID"] = strArrayIds[0];
            dr["FlowType"] = ddlFlowType.SelectedValue;
            dr["FlowDesc"] = ddlFlowType.SelectedItem.Text;
            dr["OutflowFromId"] = "144";
            dr["OutflowFrom"] = "Customer";
            dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
            dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
            dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
            dr["CashFlow_Flag_ID"] = strArrayIds[4];
            dr["CashOutFlow"] = ddlOutflowDesc.SelectedItem;
            dr["Amount"] = txtAmount_Outflow.Text;
            DtCashFlowOut.Rows.Add(dr);

            if (!string.IsNullOrEmpty(Convert.ToString(((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = '41'"))))
            {
                decimal decToatlFinanceAmt = (decimal)DtCashFlowOut.Compute("Sum(Amount)", "CashFlow_Flag_ID = '41'");

                if (FunPriGetAmountFinanced() < decToatlFinanceAmt)
                {
                    cvEMICalculator.ErrorMessage = "<br/> Correct the following validation(s): <br/><br/>  Total finance amount in cashoutflow should be equal to amount financed.";
                    cvEMICalculator.IsValid = false;
                    return;
                }
            }
            if (DtCashFlowOut.Rows.Count > 0)
            {
                decimal decToatlOutflowAmt = (decimal)DtCashFlowOut.Compute("Sum(Amount)", "CashFlow_Flag_ID > '0'");

                ViewState["CashFlows"] = DtCashFlowOut;
                if (DtCashFlowOut.Select("FlowType='2'").Length > 0)
                    ViewState["DtCashFlowOut"] = DtCashFlowOut.Select("FlowType='2'").CopyToDataTable();
                if (DtCashFlowOut.Select("FlowType='1'").Length > 0)
                    ViewState["DtCashFlow"] = DtCashFlowOut.Select("FlowType='1'").CopyToDataTable();

                gvOutFlow.DataSource = DtCashFlowOut;
                gvOutFlow.DataBind();
                FillDataFrom_ViewState_CashOutflow();
            }
            else
            {
                //FunPriBindCashFlowDetails();
            }
        }
        catch (Exception ex)
        {
            cvEMICalculator.ErrorMessage = ex.Message;
            cvEMICalculator.IsValid = false;
        }
    }

    protected void ddlFlowType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //FunPriLoadCashflowDetails("0");
            DropDownList ddlFlowType = (sender) as DropDownList;
            DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            ddlOutflowDesc.Items.Clear();
            if (ddlFlowType.SelectedValue == "1")
            {
                Utility.FillDLL(ddlOutflowDesc, ((DataTable)ViewState["RepayCashInflowList"]), true);
                SetWhiteSpaceDLL(ddlOutflowDesc);
            }
            else if (ddlFlowType.SelectedValue == "2")
            {
                Utility.FillDLL(ddlOutflowDesc, ((DataTable)ViewState["CashOutflowList"]), true);
                SetWhiteSpaceDLL(ddlOutflowDesc);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvEMICalculator.ErrorMessage = "Unable to load Cash Flows";
            cvEMICalculator.IsValid = false;
        }
    }

    private void ScriptManagerAlert(string Title, string AlertMsg)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Title, "alert('" + AlertMsg + "');", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FunPubDepreciationCalc(DataTable DtRepayStr, DataTable DtRepayGrid)
    {
        try
        {
            DateTime dtFinStDate = DateTime.Now;

            if (DateTime.Now.Month > Convert.ToInt32(ConfigurationSettings.AppSettings["StartMonth"].ToString()))
            {
                dtFinStDate = Convert.ToDateTime(ConfigurationSettings.AppSettings["StartMonth"].ToString() + "/" +
              "01/" + DateTime.Now.Year.ToString());
            }
            else
            {
                dtFinStDate = Convert.ToDateTime(ConfigurationSettings.AppSettings["StartMonth"].ToString() + "/" +
                    "01/" + ((DateTime.Now.Year) - 1).ToString());
            }
            DateTime dtFinMidDate = dtFinStDate.AddMonths(6);
            DateTime dtFinEndDate = dtFinStDate.AddYears(1).AddDays(-1);

            if (!DtRepayStr.Columns.Contains("InstDate"))
            {
                DataColumn dcInstDate = new DataColumn("InstDate", System.Type.GetType("System.DateTime"));
                DtRepayStr.Columns.Add(dcInstDate);
            }
            if (!DtRepayStr.Columns.Contains("InstAmount"))
            {
                DataColumn dcInstAmount = new DataColumn("InstAmount", System.Type.GetType("System.Decimal"));
                DtRepayStr.Columns.Add(dcInstAmount);
            }

            foreach (DataRow dr in DtRepayStr.Rows)
            {
                dr["InstDate"] = Utility.StringToDate(dr["InstallmentDate"].ToString());
                dr["InstAmount"] = Convert.ToDecimal(dr["InstallmentAmount"].ToString());
                dr.AcceptChanges();
            }

            int InstStYr = Convert.ToDateTime(DtRepayStr.Rows[0]["InstallmentDate"].ToString()).Year;
            int InstEndYr = Convert.ToDateTime(DtRepayStr.Rows[DtRepayStr.Rows.Count - 1]["InstallmentDate"].ToString()).Year;

            DataTable dt = null;
            DataTable DtRepayStrNew = DtRepayGrid.Clone();
            for (int i = InstStYr; InstStYr <= InstEndYr; InstStYr++)
            {
                if (txtDate.Text != "" && Utility.StringToDate(txtDate.Text) < dtFinMidDate && Utility.StringToDate(txtDate.Text).Year == dtFinMidDate.Year)
                {
                    dt = DtRepayStr.Select(" InstDate >=  '" + dtFinStDate + "' and  InstDate <=  '" + dtFinEndDate + "' ").CopyToDataTable();

                    decimal Value = Convert.ToDecimal(dt.Compute("Sum(InstAmount)", "InstAmount<>0").ToString());
                    Value = (Value * Convert.ToDecimal((txtDepRate.Text == "") ? "0" : txtDepRate.Text) * Convert.ToDecimal((txtCorporateTax.Text == "") ? "0" : txtCorporateTax.Text)) / (2 * 100 * 100);
                    dt.Rows[dt.Rows.Count - 1]["InstallmentAmount"] = (Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["InstallmentAmount"]) + Convert.ToInt32(Value)).ToString();
                }
                else
                {
                    dt = DtRepayStr.Select(" InstDate >=  '" + dtFinStDate + "' and  InstDate <=  '" + dtFinEndDate + "' ").CopyToDataTable();

                    string str = dt.Compute("Sum(InstAmount)", "InstAmount<>0").ToString();

                    decimal Value = Convert.ToDecimal((str == "") ? "0" : str);
                    Value = (Value * Convert.ToDecimal((txtDepRate.Text == "") ? "0" : txtDepRate.Text) * Convert.ToDecimal((txtCorporateTax.Text == "") ? "0" : txtCorporateTax.Text)) / (100 * 100);
                    dt.Rows[dt.Rows.Count - 1]["InstallmentAmount"] = (Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["InstallmentAmount"]) + Convert.ToInt32(Value)).ToString();

                }
                DtRepayStrNew.Merge(dt);
                dtFinStDate = dtFinStDate.AddYears(1);
                dtFinEndDate = dtFinEndDate.AddYears(1);
            }

            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            DataTable dtDesc = objRepaymentStructure.FunPubGetCashFlowDetails(intCompanyID, Convert.ToInt32(ddlLOB.SelectedValue));
            if (dtDesc != null && dtDesc.Rows.Count > 0)
            {
                int intCashflowId = Convert.ToInt32(dtDesc.Rows[0]["CashFlow_ID"].ToString());
                string strCashflowdesc = Convert.ToString(dtDesc.Rows[0]["CashFlow_Description"].ToString());
                DtRepayStrNew = FunPriGroupRepayDetails(DtRepayGrid, DtRepayStrNew, intCashflowId, strCashflowdesc, true, true, true);
            }
            return DtRepayStrNew;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region
    //public DataTable Moratorium(DataTable dtRepayStructure)
    //{


    //    DataTable DtMoraSt = dtRepayStructure.Clone();
    //    DataTable DtMoraEnd = null;
    //    DataTable DtMora = null;
    //    string MoraStDate = DateTime.Now.AddDays(0).ToString("MM/dd/yyyy");
    //    string MoraEndDate = Convert.ToDateTime(MoraStDate).AddDays(45).ToString("MM/dd/yyyy");

    //    if (dtRepayStructure.Select("InstallmentDate<='" + MoraStDate + "' ").Length > 0)
    //        DtMoraSt = dtRepayStructure.Select("InstallmentDate<='" + MoraStDate + "' ").CopyToDataTable();

    //    if (dtRepayStructure.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").Length > 0)
    //        DtMora = dtRepayStructure.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").CopyToDataTable();
    //    if (dtRepayStructure.Select("InstallmentDate>='" + MoraEndDate + "' ").Length > 0)
    //    {
    //        DataRow drMora = DtMoraSt.NewRow();
    //        drMora["NoofDays"] = 1;
    //        drMora["InstallmentNo"] = "1";
    //        if (ddlTime.SelectedItem.Text.Contains("ADV"))
    //        {
    //            drMora["FromDate"] = Convert.ToDateTime(MoraEndDate);
    //            drMora["From_Date"] = Convert.ToDateTime(MoraEndDate).ToString(strDateFormat);
    //        }
    //        else
    //        {
    //            drMora["FromDate"] = Convert.ToDateTime(DtMora.Rows[DtMora.Rows.Count - 1]["ToDate"].ToString());// DateTime.Now;
    //            drMora["From_Date"] = Convert.ToDateTime(DtMora.Rows[DtMora.Rows.Count - 1]["ToDate"].ToString()).ToString(strDateFormat);
    //        }
    //        drMora["To_Date"] = Convert.ToDateTime(MoraEndDate).ToString(strDateFormat);
    //        drMora["ToDate"] = Convert.ToDateTime(MoraEndDate); // DateTime.Now.AddDays(45);
    //        drMora["InstallmentDate"] = MoraEndDate;
    //        drMora["Installment_Date"] = MoraEndDate;
    //        drMora["InstallmentAmount"] = 0;
    //        drMora["Amount"] = 0;
    //        drMora["TransactionType"] = "Installment";
    //        drMora["Charge"] = "0.00";
    //        drMora["PrevBalance"] = "0.00";
    //        drMora["CurrBalance"] = "0.00";
    //        drMora["SlNo"] = "1";
    //        drMora["Tax"] = 0;
    //        drMora["Insurance"] = 0;
    //        drMora["Others"] = 0;
    //        DtMoraSt.Rows.Add(drMora);

    //        if (DtMoraSt != null)
    //            DtMoraSt.Merge(dtRepayStructure.Select("InstallmentDate>='" + MoraEndDate + "'").CopyToDataTable());
    //        else
    //            DtMoraSt = dtRepayStructure.Select("InstallmentDate>='" + MoraEndDate + "'").CopyToDataTable().Copy();
    //    }


    //    foreach (DataRow drRepaymentRow in DtMora.Rows)
    //    {
    //        drRepaymentRow["InstallmentAmount"] = 0;
    //        drRepaymentRow["Amount"] = 0;
    //        drRepaymentRow.AcceptChanges();
    //    }

    //    int intLoopCount = 0;
    //    decimal decTotalAmt = Convert.ToDecimal(txtAmount.Text);
    //    decimal decInterestAmount = Convert.ToDecimal(dtRepayStructure.Compute("sum(Charge)", "Charge<>0"));
    //    decimal decPerInstallmentAmt = 0;
    //    decimal decRoundOff = 2;

    //    #region Loop for arriving per installment amount
    //    intLoopCount = DtMoraSt.Rows.Count;
    //    if (intLoopCount > 0)
    //    {
    //        //switch (returnType)
    //        //{

    //        //case RepaymentType.FC:
    //        //case RepaymentType.WC:
    //        //case RepaymentType.TLE:
    //        //    decTotalAmt = decInterestAmount + decTotalAmt;
    //        //    decPerInstallmentAmt = decTotalAmt;
    //        //    break;
    //        //case RepaymentType.EMI:
    //        decTotalAmt = decInterestAmount + decTotalAmt;
    //        decPerInstallmentAmt = decTotalAmt / intLoopCount;
    //        //    break;
    //        //case RepaymentType.PMPT:
    //        //    decPerInstallmentAmt = (decTotalAmt / 1000) * decRateofInt;
    //        //    break;

    //        //case RepaymentType.PMPL:
    //        //    decPerInstallmentAmt = (decTotalAmt / 100000) * decRateofInt;
    //        //    break;
    //        //case RepaymentType.PMPM:
    //        //    decPerInstallmentAmt = (decTotalAmt / 1000000) * decRateofInt;
    //        //    break;

    //        //}
    //        //dtRepaymentDetails.Columns["InstallmentAmount"].DefaultValue = Math.Round(decPerInstallmentAmt, 0);
    //        decimal decPerInstallAmt = 0;
    //        int InstMentNo = 0;
    //        if (decRoundOff != 0)
    //        {
    //            decPerInstallAmt = Math.Round((decPerInstallmentAmt / decRoundOff), 0) * decRoundOff;
    //        }
    //        else
    //        {
    //            decPerInstallAmt = decPerInstallmentAmt;
    //        }

    //        foreach (DataRow drRepaymentRow in DtMoraSt.Rows)
    //        {
    //            drRepaymentRow["InstallmentAmount"] = decPerInstallAmt;
    //            drRepaymentRow["Amount"] = Math.Round(decTotalAmt, 0);
    //        }

    //        DtMoraSt.Merge(DtMora);

    //        DataView dvMoraSt = DtMoraSt.DefaultView;
    //        dvMoraSt.Sort = "ToDate ";

    //        DtMoraSt = dvMoraSt.ToTable();

    //        foreach (DataRow drRepaymentRow in DtMoraSt.Rows)
    //        {
    //            drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
    //            drRepaymentRow["SlNo"] = (InstMentNo + 1).ToString();
    //            if (InstMentNo != 0)
    //            {
    //                drRepaymentRow["From_Date"] = DtMoraSt.Rows[InstMentNo - 1]["To_Date"].ToString();
    //                drRepaymentRow["FromDate"] = Convert.ToDateTime(DtMoraSt.Rows[InstMentNo - 1]["ToDate"].ToString());
    //            }
    //            drRepaymentRow["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drRepaymentRow["FromDate"].ToString()), Convert.ToDateTime(drRepaymentRow["ToDate"].ToString()), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
    //            InstMentNo++;
    //        }
    //    }
    //    #endregion

    //    #region Check Round off impact and correct in 1st installment
    //    //if (returnType != RepaymentType.PMPL && returnType != RepaymentType.PMPM && returnType != RepaymentType.PMPT)
    //    //{
    //    if (decRoundOff != 0)
    //    {
    //        decimal decActualAmount = (decimal)DtMoraSt.Compute("Sum(InstallmentAmount)", "NoofDays >=0 ");
    //        decimal decbalamt;
    //        if (decActualAmount < decTotalAmt)
    //        {
    //            //blnIsRounded = true;
    //            decbalamt = Convert.ToInt64(decTotalAmt) - decActualAmount;
    //            DtMoraSt.Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(DtMoraSt.Rows[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
    //        }
    //        else if (decActualAmount > decTotalAmt)
    //        {
    //            //blnIsRounded = true;
    //            decbalamt = decActualAmount - Convert.ToInt64(decTotalAmt);
    //            DtMoraSt.Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(DtMoraSt.Rows[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
    //        }
    //    }
    //    //}
    //    #endregion

    //    return DtMoraSt;
    //}
    #endregion

    #region Customer control

    //protected void ddlNewCustomer_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlNewCustomer.SelectedIndex == 1)
    //        {
    //            FunPriToggleNewCustomerControls(false);
    //            lnkCreate.Attributes.Remove("onclick");
    //            lnkCreate.Visible = false;                
    //            Session["EnqNewCustomerId"] = null;
    //        }
    //        else
    //        {
    //            FunPriToggleNewCustomerControls(true);
    //            lnkCreate.Visible = true;                
    //            lnkCreate.Attributes.Add("onclick", strNewWin);
    //        }
    //        ddlNewCustomer.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        throw ex;
    //    }
    //}

    //private void FunPriToggleNewCustomerControls(bool blnIsEnabled)
    //{
    //    try
    //    {
    //        lblCreate.Enabled = blnIsEnabled;
    //        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
    //        txtName.Text = string.Empty;
    //        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
    //        hdnCustomerId.Value = string.Empty;
    //        ucCustomerCodeLov.Visible = !blnIsEnabled;
    //        lblCustomerReference.Visible = !blnIsEnabled;
    //    }
    //    catch (Exception objException)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
    //        throw objException;
    //    }
    //}

    protected void rdblCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
            txtProspectName.Text = txtEmail.Text = txtMobile.Text = txtAddress.Text = txtUserName.Text = "";

            if (chkExisting.Checked)
            {
                ucCustomerCodeLov.Visible = true;
                txtProspectName.Visible = false;
            }
            else
            {
                ucCustomerCodeLov.Visible = false;
                txtProspectName.Visible = true;
            }

            txtEmail.ReadOnly = txtMobile.ReadOnly = txtAddress.ReadOnly = (chkExisting.Checked) ? true : false;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCID != null && hdnCID.Value != "")
            {
                CustomerID = Convert.ToInt32(hdnCID.Value);

                if (CustomerID > 0)
                {
                    Dictionary<string, string> Procparam = new Dictionary<string, string>();

                    Procparam.Add("@Customer_ID", CustomerID.ToString());
                    DataTable dtCustomer = Utility.GetDefaultData("S3G_GetCustomerDetails", Procparam);

                    if (dtCustomer.Rows.Count > 0)
                    {
                        DataRow dtrCustomer = dtCustomer.Rows[0];

                        //if (dtrCustomer.Table.Columns["Customer_Code"] != null) txtCustomerCode1.Text = txtCustomerCode.Text = dtrCustomer["Customer_Code"].ToString();

                        if (dtrCustomer.Table.Columns["Title"] != null)
                            txtProspectName.Text = dtrCustomer["Title"].ToString() + " " + dtrCustomer["Customer_Name"].ToString();
                        else
                            txtProspectName.Text = dtrCustomer["Customer_Name"].ToString();

                        txtEmail.Text = dtrCustomer["Comm_Email"].ToString();
                        txtMobile.Text = dtrCustomer["Comm_Mobile"].ToString();
                        txtAddress.Text = Utility.SetCustomerAddress(dtrCustomer);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    #region Query Number Functionality

    protected void txtQueryNo_TextChanged(object sender, EventArgs e)
    {
        FunPriPopulateEMIDetailsForQueryNo();
    }

    private void FunPriPopulateEMIDetailsForQueryNo()
    {
        int EMI_ID;
        string Asset_Type;
        string Asset_ID;

        if (txtQueryNo.Text != "")
        {
            EMI_ID = Convert.ToInt32(txtQueryNo.Text);
        }
        else
        {
            EMI_ID = 0;
        }
        try
        {
            if (EMI_ID > 0)
            {
                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                Procparam.Add("@EMI_ID", EMI_ID.ToString());
                DataSet dsEMIDetails = Utility.GetDataset("S3G_GetEMIDetailsForQueryNo", Procparam);

                if (dsEMIDetails.Tables.Count > 0 && dsEMIDetails.Tables[1].Rows.Count > 0)
                {

                    ddlLOB.SelectedValue = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Lob_Id"]);
                    FunPubActivityChange();
                    ddlReturnPattern.SelectedValue = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["ReturnPattern"]);
                    FunPriLoadRepayMode();
                    ddlRepaymentMode.SelectedValue = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["Repayment_Mode"]);
                    txtProspectName.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Name"]);
                    TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
                    txtUserName.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Name"]);

                    txtEmail.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Email"]);
                    txtAddress.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Address"]);
                    txtMobile.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Mobile_No"]);
                    txtAmount.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Finance_Amount"]);
                    txtFlatRate.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Rate"]);
                    txtAccountingIRR.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Accounting_IRR"]);
                    txtBusinessIRR.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Business_IRR"]);
                    txtCompanyIRR.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Company_IRR"]);

                    //Set Date Format
                    txtDate.Text = Convert.ToDateTime(dsEMIDetails.Tables[0].Rows[0]["EMI_Date"]).ToString(strDateFormat);
                    //Image1.Visible = false;
                    //txtDate.ReadOnly = true;

                    txtQueryNo.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["EMI_ID"]);
                    ddlTenureType.SelectedValue = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Tenure_Type"]);
                    txtTenure.Text = Convert.ToString(dsEMIDetails.Tables[0].Rows[0]["Tenure"]);
                    Asset_Type = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["Asset_Type"]);

                    if (ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "OL" || ddlLOB.SelectedItem.Text.Split('-')[0].Trim().ToUpper() == "FL")
                    {
                        //to bind ddlAsset on Load for QueryNumber 
                        //rdblAssetType.Visible = true;
                        if ((Asset_Type).Trim() == "0")
                        {
                            ddlAssetRegister.Enabled = true;
                            rdblAssetType.SelectedValue = "0";
                            FunPubLoadAssetDetails();
                            ddlAssetRegister.SelectedValue = Convert.ToInt32(dsEMIDetails.Tables[4].Rows[0]["Asset_ID"]).ToString();
                        }
                        else
                        {
                            rdblAssetType.SelectedValue = "1";
                            FunPubLoadAssetDetails();
                            ddlAssetRegister.SelectedValue = Convert.ToInt32(dsEMIDetails.Tables[4].Rows[0]["Asset_ID"]).ToString();
                        }

                        chkDepreStrucure.Enabled = true;
                        string depStruval;
                        depStruval = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["IsDeprecStructure"]);
                        if (depStruval.Trim() == "1")
                        {
                            chkDepreStrucure.Checked = true;
                        }
                        else
                        {
                            chkDepreStrucure.Checked = false;
                        }
                    }
                    else
                    {
                        rdblAssetType.Visible = false;
                        rdblAssetType.SelectedValue = "0";
                        FunPubLoadAssetDetails();

                        if (!string.IsNullOrEmpty(dsEMIDetails.Tables[4].Rows[0]["Asset_ID"].ToString()))
                        {
                            ddlAssetRegister.Enabled = true;
                            ddlAssetRegister.SelectedValue = Convert.ToInt32(dsEMIDetails.Tables[4].Rows[0]["Asset_ID"]).ToString();
                        }
                        else
                        {
                            ddlAssetRegister.Enabled = false;
                            ddlAssetRegister.SelectedValue = "0";
                        }

                        chkDepreStrucure.Enabled = false;
                    }

                    if (ddlReturnPattern.SelectedValue != "1" && ddlReturnPattern.SelectedValue != "2" && ddlRepaymentMode.SelectedValue == "3")
                    {
                        FunprisetMandatoryRate(false);
                        FunprisetMandatoryBusinessIRR(false);
                    }
                    else
                    {
                        if (ddlReturnPattern.SelectedValue == "1")
                        {
                            txtFlatRate.ReadOnly = false;
                            txtBusinessIRR.ReadOnly = true;
                            rfvFlatRate.Enabled = true;
                            rfvBusinessIRR.Enabled = false;
                        }
                        else if (ddlReturnPattern.SelectedValue == "2")
                        {
                            txtFlatRate.ReadOnly = true;
                            txtBusinessIRR.ReadOnly = false;
                            rfvFlatRate.Enabled = false;
                            rfvBusinessIRR.Enabled = true;
                        }
                    }

                    txtCorporateTax.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["CTR"]);
                    txtRecoveryPatternYear1.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["Recovery_Year1"]);
                    txtRecoveryPatternYear2.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["Recovery_Year2"]);
                    txtRecoveryPatternYear3.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["Recovery_Year3"]);
                    txtRecoveryPatternYearRest.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["Recovery_Remaining"]);

                    if (string.IsNullOrEmpty(dsEMIDetails.Tables[4].Rows[0]["AssetCost"].ToString()))
                    {
                        txtAssetCost.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["AssetCost"]);
                    }
                    else
                    {
                        txtAssetCost.Text = string.Empty;
                    }
                    txtDepRate.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["DepreciationRate"]);

                    ddlRateType.SelectedValue = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["Rate_Type"]);

                    if (!string.IsNullOrEmpty(dsEMIDetails.Tables[4].Rows[0]["Time"].ToString()))
                    {
                        ddlTime.Enabled = true;
                        ddlTime.SelectedValue = Convert.ToInt32(dsEMIDetails.Tables[4].Rows[0]["Time"]).ToString();
                    }
                    else
                    {
                        ddlTime.SelectedValue = "0";
                        ddlTime.Enabled = false;
                    }

                    if (!string.IsNullOrEmpty(dsEMIDetails.Tables[4].Rows[0]["Frequency"].ToString()))
                    {
                        ddlFrequency.Enabled = true;
                        ddlFrequency.SelectedValue = Convert.ToInt32(dsEMIDetails.Tables[4].Rows[0]["Frequency"]).ToString();
                    }
                    else
                    {
                        ddlFrequency.Enabled = false;
                        ddlTime.SelectedValue = "0";
                    }

                    txtResidualPer.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["ResidualPer"]);
                    txtResidualValueAmt.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["ResidualAmount"]);
                    txtMarginMoneyAmount_Cashflow.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["MarginAmount"]);
                    txtMarginMoneyPer_Cashflow.Text = Convert.ToString(dsEMIDetails.Tables[4].Rows[0]["MarginPer"]);

                    gvOutFlow.DataSource = dsEMIDetails.Tables[1];
                    gvOutFlow.DataBind();
                    ViewState["DtCashFlowOut"] = dsEMIDetails.Tables[1];
                    ViewState["CashFlows"] = dsEMIDetails.Tables[1];
                    FillDataFrom_ViewState_CashOutflow();

                    gvRepaymentDetails.DataSource = dsEMIDetails.Tables[2];
                    gvRepaymentDetails.DataBind();
                    ViewState["DtRepayGrid"] = dsEMIDetails.Tables[2];

                    grvRepayStructure.DataSource = dsEMIDetails.Tables[3];
                    grvRepayStructure.DataBind();
                    ViewState["RepayStructure"] = dsEMIDetails.Tables[3];

                    //To Load Cashflow Grid details on Query Number mode
                    FunPriLoadCashflowDetailsForQueryNumber();

                    txtAssetCost.Text = txtAmount.Text;
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "No Record Exists");
                    FunPriClearControls();
                    txtQueryNo.Enabled = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Fill Details For Query Number");
        }
    }

    private void FunPriLoadCashflowDetailsForQueryNumber()
    {
        //To Load Cashflow Grid details on Query Number mode
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = null;
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

        ObjStatus.Option = 311;
        ObjStatus.Param1 = intCompanyID.ToString();
        ObjStatus.Param2 = ddlLOB.SelectedValue.ToString();
        DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
        ViewState["CashOutflowList"] = DtCashFlow;

        ObjStatus.Option = 170;
        ObjStatus.Param1 = intCompanyID.ToString();
        //if (ddlLOB.SelectedIndex > 0)
            ObjStatus.Param2 = ddlLOB.SelectedValue.ToString();
        DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
        ViewState["RepayCashInflowList"] = DtCashFlow;
    }

    #endregion

}


