using System;
using S3GBusEntity;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text;
using System.Web.Security;
using System.Configuration;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

public partial class LoanAdmin_S3GLOANADPreMatureClosure_Add : ApplyThemeForProject
{
    #region [Local Field's]

    static decimal dcDue = 0, dcWaived = 0, dcPayable = 0, dcClosure = 0, dcReceived = 0, decCTR = 0, decPLR = 0;
    static decimal dcFooterDue = 0, dcFooterReceived = 0, dcFooterOS = 0;
    static decimal dcfunderdue = 0, dcfunderfuturerec = 0, dcfundernpv = 0;
    decimal funder_Total = 0;
    static decimal dcWaivedOff = 0, dcMinAmount = 0, Principle_Rate = 0, TotalPaid_Interest = 0;
    static decimal NextMth_FinanceCharge = 0, CurrentMth_FinanceCharge = 0, FuturePrincipalAmt = 0;
    static int Bill_InstallmentNo, Repayment_Mode_Code, Residual_Value;
    static string Time_Value = "", Frequency = string.Empty;
    static string strActivationType = "";
    static int RoundOff = 0, Return_Pattern = 0;
    static DateTime First_InstallmentDate = DateTime.Now, Current_InstallmentDt = DateTime.Now, Next_InstallmentDt = DateTime.Now;
    int intCompanyID, intUserID = 0, intGPS_DaysPerYear = 0;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    string strDateFormat = string.Empty;
    public string strProgramId = "85";
    //static string strCustomerEmail = string.Empty;
    DataTable dtfunder = new DataTable();
    string strPANum = string.Empty;
    string strSANum = string.Empty;
    int intClosureDetailId = 0;
    string strAccountClosure = string.Empty;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    DataTable dtpv = new DataTable();
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLOANADPreMatureClosure_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PRC';";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=PRC";
    StringBuilder strbAccountClosureDetails = new StringBuilder();
    string strXMLAccountClosureDetails = "<Root><Details Desc='0' /></Root>";
    string strPageName = "Account PreClosure";
    DataTable dtTax = new DataTable();
    DataTable dtRental = new DataTable();
    DataSet dsrental = new DataSet();
    public static LoanAdmin_S3GLOANADPreMatureClosure_Add obj_Page;
    #region [User Authorization]

    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    DataSet dtclosure = new DataSet();
    #endregion [User Authorization]

    #endregion [Local Field's]

    #region [Intialization]

    ContractMgtServicesReference.ContractMgtServicesClient objAccountClosure_Client;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDataTable objS3G_LOANAD_AccClosureTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureRow objS3G_LOANAD_AccClosureDataRow = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsDataTable objS3G_LOANAD_AccDetailsTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsRow objS3G_LOANAD_AccDetailsDataRow = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureDataTable objCancelDataTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureRow objCancelRow = null;

    //ReportDocument rptd = new ReportDocument();

    #endregion [Intialization]

    #region [Event's]

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (rptd != null)
        //{
        //    rptd.Close();
        //    rptd.Dispose();
        //}
    }

    #region [Page Event's]

    protected new void Page_PreInit(object sender, EventArgs e) //transaction screen page init
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
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            FunPubPageLoad();
            FunPubSetErrorMessageControl();
            txtPreRate.SetDecimalPrefixSuffix(2, 4, false, "Preclosure Rate");
            txtDiscRate.SetDecimalPrefixSuffix(2, 4, false, "Breaking Charges");
            txtfunderprerate.SetDecimalPrefixSuffix(2, 4, false, "Funder Pre Rate");
            // txtBreakingCharges.SetDecimalPrefixSuffix(2, 4, true, "Funder Breaking Charges");
            //txtPreAmount.SetDecimalPrefixSuffix(25, 2, true, "Preclosure Rate");
            //txtPMCReqDate.Attributes.Add("Readonly", "true");
            CalendarExtender1.Format = strDateFormat;
            CalendarExtenderTranchedate.Format = strDateFormat;
            intGPS_DaysPerYear = Convert.ToInt32(ConfigurationSettings.AppSettings["GPS_DaysPerYear"].ToString());
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad;
            cvAccountPreClosure.IsValid = false;
        }
    }

    #endregion [Page Event's]

    #region [Dropdown Event's]

    #region [LOB/Branch Drop Down List]

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            FunPriClearControls();

            if (ddlPreType.Items.Count > 0)
            {
                ddlPreType.Items.Clear();
            }

            //if (ddlBranch.Items.Count > 0)
            // ddlBranch.Clear();

            PopulateClosureType();

            //if (ddlMLA.Items.Count > 0) ddlMLA.Items.Clear();
            ddlMLA.Clear();
            // if (ddlSLA.Items.Count > 0) ddlSLA.Items.Clear();

            PopulatePANum();

            if (ddlLOB.SelectedItem.Text.Trim().Contains("Working") || ddlLOB.SelectedItem.Text.Trim().Contains("Factor"))
                rfvPreType.Enabled = rfvPreRate.Enabled = ddlPreType.Enabled = txtPreRate.Enabled = false;
            else
            {
                rfvPreType.Enabled = rfvPreRate.Enabled = ddlPreType.Enabled = txtPreRate.Enabled = true;
                if (ddlPreType.Items.Count == 1)
                {
                    Utility.FunShowAlertMsg(this.Page, "Preclosure Type not defined in Global Parameter Setup");
                    return;
                }

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Decimal DCPMCRate = Convert.ToDecimal(Utility.GetTableScalarValue("S3G_LOANAD_GetPMCRate", Procparam));

                if (DCPMCRate <= 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Preclosure Rate not defined in Global Parameter Setup Or Rate should be greater than Zero.");
                    return;
                }

            }


            PopulateBranchList();
            FunProToggleRental();
            //if (ddlBranch.Items.Count > 0)
            //    ddlBranch.SelectedValue = ddlBranch.Items[0].Value;

        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }


    protected void FunProToggleRental()
    {

        lblPreRate.Text = "Foreclosure Rate";


    }

    //protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //ddlMLA.Visible = true;
    //        //ddlMLA1.Visible = false;

    //        FunPriClearControls();
    //        PopulatePANum();
    //    }
    //    catch (Exception ex)
    //    {
    //        cvAccountPreClosure.ErrorMessage = ex.Message;
    //        cvAccountPreClosure.IsValid = false;
    //    }
    //}

    protected void uctranche_OnItem_Selected(object sender, EventArgs e)
    {
        try
        {


            PopulatePANum();
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    #endregion [LOB/Branch Drop Down List]

    #region [PANum /SANum Dropdown list]

    //protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunPriClearControls();
    //        PopulateSANum(ddlMLA.SelectedValue);
    //        PopulatePANCustomer(ddlMLA.SelectedValue);
    //    }
    //    catch (Exception ex)
    //    {
    //        cvAccountPreClosure.ErrorMessage = ex.Message;
    //        cvAccountPreClosure.IsValid = false;
    //    }
    //}

    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            FunPriClearControls();
            //tbAccDetails.Enabled =
            tbCashFlow.Enabled = true;
            if (strAccountClosure == string.Empty)
            {
                string strError = "";
                Procparam = FunPriProcParam();
                if (ddlMLA.SelectedValue.ToString() != "0")
                    Procparam.Add("@PA_SA_REF_ID", ddlMLA.SelectedValue.ToString());
                Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@Tranche_id", uctranche.SelectedValue);
                if (txtPMCReqDate.Text.Trim() != "")
                    Procparam.Add("@PMCDate", Utility.StringToDate(txtPMCReqDate.Text).ToString());

                strError = Utility.GetTableScalarValue("S3G_LOANAD_CheckTranForClosure", Procparam);

                if (strError == "1")
                {
                    Utility.FunShowAlertMsg(this, "Transactions are available and user to do the reversal and then to proceed");
                    ddlMLA.Clear();
                    uctranche.Clear();
                    return;
                }

                FunGetAccountDetailsForClosure();
                PopulatePANCustomer();
                System.Web.HttpContext.Current.Session["Tranche_Id"] = uctranche.SelectedValue;

            }
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void ddlMLA1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls1();
            //tbAccDetails.Enabled =
            tbCashFlow.Enabled = true;
            if (strAccountClosure == string.Empty)
            {
                string strError = "";
                Procparam = FunPriProcParam();
                if (ddlMLA.SelectedValue.ToString() != "0")
                    Procparam.Add("@PA_SA_REF_ID", ddlMLA.SelectedValue.ToString());
                Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@Tranche_id", uctranche.SelectedValue);
                if (txtPMCReqDate.Text.Trim() != "")
                    Procparam.Add("@PMCDate", Utility.StringToDate(txtPMCReqDate.Text).ToString());

                strError = Utility.GetTableScalarValue("S3G_LOANAD_CheckTranForClosure", Procparam);

                if (strError == "1")
                {
                    Utility.FunShowAlertMsg(this, "Transactions are available and user to do the reversal and then to proceed");
                    ddlMLA.Clear();
                    uctranche.Clear();
                    return;
                }

                FunGetAccountDetailsForClosure();
                PopulatePANCustomer();
            }
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    #endregion  [PANum /SANum Dropdown list]

    protected void txtWaived_TextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal decvalue = 0;
            decimal decVat = 0;
            decimal decbr = 0;
            decimal decST = 0;
            TextBox txtWaived = (sender) as TextBox;
            GridViewRow gvRow = txtWaived.Parent.Parent as GridViewRow;
            Label lblDue = gvRow.FindControl("lblDue") as Label;
            Label lblPayable = gvRow.FindControl("lblPayable") as Label;
            Label lblClosure = gvRow.FindControl("lblClosure") as Label;
            HiddenField hdnCashFlowID = gvRow.FindControl("hdnCashFlowID") as HiddenField;
            HiddenField hdnIs_Due = gvRow.FindControl("hdnIs_Due") as HiddenField;
            Label lblpanum = gvRow.FindControl("lblpanum") as Label;

            if (!String.IsNullOrEmpty(txtWaived.Text.Trim()))
            {
                if (hdnIs_Due.Value == "1" && Convert.ToDecimal(txtWaived.Text) > Convert.ToDecimal(lblDue.Text) && hdnCashFlowID.Value != "98")
                {
                    Utility.FunShowAlertMsg(this, "WaivedOff amount should be less than or equal to Due Amount");
                    txtWaived.Text = "";
                    return;
                }
                else if (hdnIs_Due.Value == "2" && Convert.ToDecimal(txtWaived.Text) > Convert.ToDecimal(lblDue.Text) && hdnCashFlowID.Value == "24")
                {
                    Utility.FunShowAlertMsg(this, "WaivedOff amount should be less than or equal to Due Amount");
                    txtWaived.Text = "";
                    return;
                }
                else if (hdnIs_Due.Value == "2" && Convert.ToDecimal(txtWaived.Text) > Convert.ToDecimal(lblPayable.Text)
                    && hdnCashFlowID.Value != "98" && hdnCashFlowID.Value != "24")
                {
                    Utility.FunShowAlertMsg(this, "WaivedOff amount should be less than or equal to PV Amount");
                    txtWaived.Text = "";
                    return;
                }
                else if (hdnCashFlowID.Value == "98" && Convert.ToDecimal(txtWaived.Text) > Convert.ToDecimal(txtPreAmount.Text))
                {
                    Utility.FunShowAlertMsg(this, "Write-off amount should be less than or equal to PreClosure Amount");
                    txtWaived.Text = "";
                    return;
                }
            }

            if (lblPayable.Text == "0.00" && txtWaived.Text != "")
                lblClosure.Text = (Convert.ToDecimal(lblDue.Text) - Convert.ToDecimal(txtWaived.Text)).ToString();
            else if (txtWaived.Text != "")
                lblClosure.Text = (Convert.ToDecimal(lblPayable.Text) - Convert.ToDecimal(txtWaived.Text)).ToString();
            else
                lblClosure.Text = lblPayable.Text;

            if (ViewState["ClosureStatement"] != null)
            {
                DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                DataRow[] drClosureStatement = dtClosureStatement.Select(" ID = '" + hdnCashFlowID.Value + "' and panum='" + lblpanum.Text + "'");

                drClosureStatement[0].BeginEdit();

                if (!String.IsNullOrEmpty(txtWaived.Text.Trim()))
                {
                    drClosureStatement[0]["Waived"] = Convert.ToDecimal(txtWaived.Text.Trim());
                    drClosureStatement[0]["Waived1"] = Convert.ToDecimal(txtWaived.Text.Trim());
                }
                else
                {
                    drClosureStatement[0]["Waived"] = "0.00";
                    drClosureStatement[0]["Waived1"] = "0.00";
                }

                if (!String.IsNullOrEmpty(lblClosure.Text.Trim()))
                {
                    drClosureStatement[0]["closure"] = Convert.ToDecimal(lblClosure.Text.Trim());
                    drClosureStatement[0]["closure1"] = Convert.ToDecimal(lblClosure.Text.Trim());
                }

                drClosureStatement[0].AcceptChanges();
                ViewState["ClosureStatement"] = dtClosureStatement;

                DataRow[] drClosureStatement1 = dtClosureStatement.Select(" ID in ('74','138','139','140') and panum='" + lblpanum.Text + "'");
                foreach (DataRow dr in drClosureStatement1)
                {
                    decvalue = decvalue + Convert.ToDecimal(dr["closure"].ToString());
                }

                DataTable dtTax = ViewState["dtTax"] as DataTable;
                decbr = (decvalue * Convert.ToDecimal(txtDiscRate.Text.ToString()) / 100);

                if (dtTax != null)
                {
                    DataRow[] drtax = dtTax.Select("PANUM='" + lblpanum.Text + "' and installment_amount>0");
                    if (drtax.Length > 0)
                    {

                        foreach (DataRow dr1 in drtax)
                        {
                            decVat = (decvalue * (Convert.ToDecimal(dr1["factor"].ToString()) / 100) * (Convert.ToDecimal(dr1["RatePercentage"].ToString()) / 100)) + (decvalue * (Convert.ToDecimal(dr1["factor"].ToString()) / 100) * (Convert.ToDecimal(dr1["Additional_tax"].ToString()) / 100));
                        }

                        decST = (decbr * (Convert.ToDecimal(drtax[0]["STRatePercentage"].ToString()) / 100)) + (decbr * ((Convert.ToDecimal(drtax[0]["STAdditional_tax"].ToString()) / 100)));
                    }
                }

            }

            if (ViewState["ClosureStatement"] != null)
            {
                DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;

                if (hdnCashFlowID.Value == "145" && (txtWaived.Text == "0.00" || txtWaived.Text == ""))
                {
                    DataRow[] drClosureStatement = dtClosureStatement.Select(" ID = 145 and panum='" + lblpanum.Text + "'");
                    drClosureStatement[0].BeginEdit();

                    drClosureStatement[0]["closure"] = decVat.ToString(Funsetsuffix());

                    drClosureStatement[0].AcceptChanges();
                    ViewState["ClosureStatement"] = dtClosureStatement;
                }

                if (hdnCashFlowID.Value == "73" && txtWaived.Text == "0.00")
                {

                    DataRow[] drClosureStatement1 = dtClosureStatement.Select(" ID = 73 and panum='" + lblpanum.Text + "'");
                    drClosureStatement1[0].BeginEdit();

                    drClosureStatement1[0]["closure"] = decbr.ToString(Funsetsuffix());

                    drClosureStatement1[0].AcceptChanges();
                    ViewState["ClosureStatement"] = dtClosureStatement;
                }

                if (hdnCashFlowID.Value == "172" && txtWaived.Text == "0.00")
                {

                    DataRow[] drClosureStatement2 = dtClosureStatement.Select(" ID = 172 and panum='" + lblpanum.Text + "'");
                    drClosureStatement2[0].BeginEdit();

                    drClosureStatement2[0]["closure"] = decST.ToString(Funsetsuffix());

                    drClosureStatement2[0].AcceptChanges();
                    ViewState["ClosureStatement"] = dtClosureStatement;
                }

            }
            DataTable dt1 = ViewState["ClosureStatement"] as DataTable;

            FunPriBindCashFlowGrid();

            txtPreAmount.Text = (dcClosure).ToString();
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void txtRemarks_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtRemarks = (sender) as TextBox;
            GridViewRow gvRow = txtRemarks.Parent.Parent as GridViewRow;
            HiddenField hdnCashFlowID = gvRow.FindControl("hdnCashFlowID") as HiddenField;
            if (ViewState["ClosureStatement"] != null)
            {
                DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                DataRow[] drClosureStatement = dtClosureStatement.Select(" ID = '" + hdnCashFlowID.Value + "'  ");
                drClosureStatement[0].BeginEdit();
                drClosureStatement[0]["Remarks"] = txtRemarks.Text.Trim();
                drClosureStatement[0].AcceptChanges();
                ViewState["ClosureStatement"] = dtClosureStatement;
            }
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }
    //protected void txtPreRate_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunPriCalBrokenPeriodInterest();
    //        FunPubCalCulatePMCAmount();
    //        txtPreRate.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //        cvAccountPreClosure.ErrorMessage = ex.Message;
    //        cvAccountPreClosure.IsValid = false;
    //    }
    //}

    private void FunPriCalBrokenPeriodInterest()
    {
        try
        {
            int intDaysPerYear = 0;
            decimal decBRPercentage = 0;
            if (intGPS_DaysPerYear == 365) intDaysPerYear = 31;
            else intDaysPerYear = 30;
            DateTime dtPMCReqDate = DateTime.Now;
            if (txtPMCReqDate.Text.Trim() != "")
                dtPMCReqDate = Utility.StringToDate(txtPMCReqDate.Text);
            if (Time_Value.ToUpper().Contains("ADVANCE"))
            {
                TimeSpan diff1 = Next_InstallmentDt.Subtract(dtPMCReqDate);
                decBRPercentage = Math.Round((CurrentMth_FinanceCharge / intDaysPerYear) * diff1.Days, 2);//RoundOff);
            }
            else
            {
                TimeSpan diff1 = dtPMCReqDate.Subtract(Current_InstallmentDt);
                decBRPercentage = Math.Round((NextMth_FinanceCharge / intDaysPerYear) * diff1.Days, 2);//RoundOff);
            }
            if (decBRPercentage == 0) decBRPercentage = decimal.Parse("0.00");
            if (ViewState["ClosureStatement"] != null)
            {
                DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                DataRow[] drBRPercentage = dtClosureStatement.Select(" ID = '73' ");
                if (drBRPercentage.Length == 0)
                {
                    DataRow drBrPer = dtClosureStatement.NewRow();
                    drBrPer["ID"] = 73;
                    drBrPer["Description"] = "Broken period interest";
                    if (Time_Value.ToUpper() == "ADVANCE")
                    {
                        drBrPer["Payable"] = decBRPercentage;
                        drBrPer["Closure"] = -decBRPercentage;
                        drBrPer["Due"] = decimal.Parse("0.00");
                    }
                    else
                    {
                        drBrPer["Due"] = decBRPercentage;
                        drBrPer["Closure"] = decBRPercentage;
                        drBrPer["Payable"] = decimal.Parse("0.00");
                    }
                    drBrPer["Waived"] = decimal.Parse("0.00");
                    drBrPer["Received"] = decimal.Parse("0.00");
                    dtClosureStatement.Rows.Add(drBrPer);
                }
                else
                {
                    drBRPercentage[0].BeginEdit();
                    if (Time_Value.ToUpper().Contains("ADVANCE"))
                    {
                        drBRPercentage[0]["Payable"] = decBRPercentage;
                        drBRPercentage[0]["Closure"] = -decBRPercentage;
                    }
                    else
                    {
                        drBRPercentage[0]["Due"] = decBRPercentage;
                        drBRPercentage[0]["Closure"] = decBRPercentage;
                    }
                    drBRPercentage[0].AcceptChanges();
                }
                ViewState["ClosureStatement"] = dtClosureStatement;
                FunPriBindCashFlowGrid();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkwaived_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunGetAccountDetailsForClosure_NPV();
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void txtPMCReqDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            ddlMLA.Clear();
            uctranche.Clear();
            grvaccount.DataSource = null;
            grvaccount.DataBind();
            if (!string.IsNullOrEmpty(txtPMCReqDate.Text.Trim()))
            {
                if (Convert.ToInt32(Utility.StringToDate(txtPMCReqDate.Text.ToString()).ToString("yyyyMM")) > Convert.ToInt32(DateTime.Today.ToString("yyyyMM")))
                {
                    Utility.FunShowAlertMsg(this, "Pre Mature Closure Date should be with in the current month");
                    txtPMCReqDate.Text = DateTime.Today.ToString(strDateFormat);
                }

                //if (Convert.ToInt32(Utility.StringToDate(txtPMCReqDate.Text.ToString()).ToString("yyyyMMdd")) < Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd")))
                //{
                //    Utility.FunShowAlertMsg(this, "Pre Mature Closure Date should be equal or greater than current date");
                //    txtPMCReqDate.Text = DateTime.Today.ToString(strDateFormat);
                //}

                txtPreclosureDate.Text = txtPMCReqDate.Text.Trim();
                if (ddlMLA.SelectedValue != "0" || uctranche.SelectedValue != "0")
                {
                    FunGetAccountDetailsForClosure();
                    FunPriCalBrokenPeriodInterest();
                }
            }
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void ddlPreType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //hdnIRR.Value = "0";
            if (strActivationType.ToUpper() != "IRR" && ddlPreType.SelectedItem.Text.ToUpper().Trim() == "IRR")
            {
                Utility.FunShowAlertMsg(this, " Amortization schedule method and Premature closure type are not same(Only for IRR). Unable to proceed Premature Closure");
                // hdnIRR.Value = "2";
                ddlPreType.SelectedIndex = 0;
                return;
            }
            if (ddlPreType.SelectedValue == "30")
            {
                txtPreRate.Text = Principle_Rate.ToString();
                txtPreRate.ReadOnly = true;
            }
            else
            {
                txtPreRate.ReadOnly = false;
            }
            //FunPriCalBrokenPeriodInterest();
            FunPubCalCulatePMCAmount();
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }
    #endregion [Dropdown Event's]

    #region [Grid Event's]

    protected void grvCashFlow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCashFlowID = e.Row.FindControl("hdnCashFlowID") as HiddenField;
                TextBox txtWaived = e.Row.FindControl("txtWaived") as TextBox;
                Label lblDue = e.Row.FindControl("lblDue") as Label;

                HiddenField hdnIs_Due = e.Row.FindControl("hdnIs_Due") as HiddenField;

                if (hdnIs_Due.Value == "1")
                    txtWaived.Attributes.Add("readonly", "readonly");

                txtWaived.SetDecimalPrefixSuffix(25, 2, false, "Waived Amount");
                //if (hdnCashFlowID.Value == "24" || hdnCashFlowID.Value == "26" || hdnCashFlowID.Value == "42" || hdnCashFlowID.Value == "98")// Memo,penalty,ODI & Writeoff Exp
                //    txtWaived.Visible = true;
                //else
                //    txtWaived.Visible = false;

                //if (!string.IsNullOrEmpty(txtWaived.Text.Trim()))
                //    dcWaived = dcWaived + Convert.ToDecimal(txtWaived.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //e.Row.Cells[1].Text = dcDue.ToString();
                //e.Row.Cells[2].Text = dcWaived.ToString();
                //e.Row.Cells[3].Text = dcPayable.ToString();
                //e.Row.Cells[4].Text = txtPreAmount.Text = dcClosure.ToString();
                //e.Row.Cells[5].Text = dcReceived.ToString();
                FunPriSetFooterValue();
            }
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    #endregion [Grid Event's]

    #region [Button Event's]

    #region [Saving Account Closure Details]

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnIRR.Value == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "The values has been changed.calculate NPV again");
                return;
            }

            FunPriSetFooterValue();
            FunSaveClick();
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_Save;
            cvAccountPreClosure.IsValid = false;
        }
    }

    public void FunSaveClick()
    {
        int intDtCom = 0;
        string strEndDate = "";
        try
        {

            if (uctranche.SelectedText == "" && ddlMLA.SelectedText == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Select either Tranche or RS Number");
                return;
            }
            //if (chkBoxWriteOff.Checked && ViewState["WriteOffAmount"] != null)
            //{ 
            //    decimal decWriteOffAmount = 0;

            //    if (txtWriteOffAmount.Text != "")
            //        decWriteOffAmount = Convert.ToDecimal(txtWriteOffAmount.Text);

            //    if (Convert.ToDecimal(ViewState["WriteOffAmount"]) != decWriteOffAmount)
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "WriteOff Amount Should be equal to Installment Amount");
            //        return;
            //    }

            //}

            // if (strAccountClosure == string.Empty && txtPMCReqDate.Text.Trim() != "" && txtMatureDate.Text.Trim() != "")
            //{
            //    intDtCom = Utility.CompareDates(txtMatureDate.Text, txtPMCReqDate.Text);
            //    if (intDtCom != -1)
            //    {
            //        cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_LT_MaturityDt;
            //        cvAccountPreClosure.IsValid = false;
            //        return;
            //    }
            //    //if (ddlSLA.Items.Count > 1)
            //    //{
            //    //    if (ddlSLA.SelectedValue == "0")
            //    //    {
            //    //        cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_Select_SubAc;
            //    //        cvAccountPreClosure.IsValid = false;
            //    //        return;
            //    //    }
            //    //}
            //}
            if (strAccountClosure == string.Empty)
            {
                strEndDate = DateTime.Now.Month + "-" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + "-" + DateTime.Now.Year;
                intDtCom = Utility.CompareDates(Convert.ToDateTime(strEndDate).ToString(strDateFormat), txtPMCReqDate.Text);
                if (intDtCom == 1)
                {
                    cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_WT_currentDt;
                    cvAccountPreClosure.IsValid = false;
                    return;
                }
                intDtCom = Utility.CompareDates(DateTime.Now.ToString(strDateFormat), txtPMCReqDate.Text);
                //if (intDtCom == -1)
                //{
                //    cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_EqlOrGT_CurrentDt;
                //    cvAccountPreClosure.IsValid = false;
                //    return;
                //}
            }
            //if (hdnIRR.Value == "")
            //{
            //    cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + "Error in calculating the Pre Closure Amount. Recalculate the Pre Closure Amount ";
            //    cvAccountPreClosure.IsValid = false;
            //    return;
            //}
            //if (hdnIRR.Value == "2")
            //{
            //    cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + " Amortization schedule method and Premature closure type are not same(Only for IRR). Unable to proceed Premature Closure";
            //    cvAccountPreClosure.IsValid = false;
            //    return;
            //}
            //if (grvAccountBalance.Rows.Count == 0 && grvCashFlow.Rows.Count == 0)
            //{
            //    cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_AccClosure_Gen;
            //    cvAccountPreClosure.IsValid = false;
            //    return;
            //}

            //if (Convert.ToDecimal(txtPreAmount.Text) != 0)
            //{
            //    cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + "Pre Closure Amount has to be collected from Customer.";
            //    cvAccountPreClosure.IsValid = false;
            //    return;
            //}

            string strDocNo = "";
            bool booMail = false;

            intErrCode = FunPubSaveAccountPreClosure(out strDocNo);
            hidAccClosureNo.Value = strDocNo;
            string strMessage = "";
            hidClosureDetailId.Value = intClosureDetailId.ToString();

            switch (intErrCode)
            {
                case 0:
                    {
                        if (strAccountClosure == string.Empty)
                        {
                            btnSave.Enabled = false;
                            strAccountClosure = strDocNo;
                            try
                            {
                                FunPubEmailSent(booMail);
                            }
                            catch (Exception exMail)
                            {
                                strMessage = "Invalid EMail ID. Mail not sent to the customer";
                            }
                            strAlert = "";
                            //if (txtMode.Text.ToUpper().Trim() == "PDC")
                            //{
                            //    strAlert += "All Post dated cheques are processed. No more pending Post dated cheques... ";
                            //}

                            strAlert += Resources.ValidationMsgs.S3G_SucMsg_LoanAd_PreMature_Closed + " - " + strDocNo;
                            strAlert += @"\n" + strMessage;
                            strAlert += @"\n\nWould you like to generate one more Pre Mature Closure ?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END

                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + " Premature Closure " + Resources.ValidationMsgs.S3G_SucMsg_Update + "');" + strRedirectPageView, true);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        break;
                    }
                case 1:
                    {
                        if (strAccountClosure == string.Empty)
                        {
                            btnSave.Enabled = false;
                            strAccountClosure = strDocNo;
                            try
                            {
                                FunPubEmailSent(booMail);
                            }
                            catch (Exception exMail)
                            {
                                strMessage = "Invalid EMail ID. Mail not sent to the customer";
                            }
                            strAlert = "__ALERT__";
                            //if (txtMode.Text.ToUpper().Trim() == "PDC")
                            //{
                            //    strAlert += "All Post dated cheques are processed. No more pending Post dated cheques... ";
                            //}



                            strAlert = strAlert.Replace("__ALERT__", "Premature Closure  Details updated successfully");
                            //strAlert += @"\n" + strMessage;
                            strAlert += @"\n\nWould you like to generate one more Pre Mature Closure ?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);

                            btnSave.Enabled = false;

                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END

                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + " Premature Closure " + Resources.ValidationMsgs.S3G_SucMsg_Update + "');" + strRedirectPageView, true);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        break;
                    }
                case 3:
                    {
                        Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                        break;
                    }
                case 4:
                    {
                        Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                        break;
                    }
                case 5:
                    {
                        if (strAccountClosure == string.Empty)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_MthClosure_Iniate_PreMatureClosure + "');" + strRedirectPageAdd, true);   //Month Closure should be open for iniatiating Pre Mature closure 
                        break;
                    }
                case 7:
                    {
                        if (strAccountClosure == string.Empty)
                        {
                            btnSave.Enabled = false;
                            strAccountClosure = strDocNo;
                            try
                            {
                                FunPubEmailSent(booMail);
                            }
                            catch (Exception exMail)
                            {
                                strMessage = "Invalid EMail ID. Mail not sent to the customer";
                            }

                            strAlert = "Existing Post dated cheques are not disposed to the customer... ";

                            strAlert += Resources.ValidationMsgs.S3G_SucMsg_LoanAd_PreMature_Closed + " - " + strDocNo;
                            strAlert += @"\n\nWould you like to generate one more Pre Mature Closure ?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            // ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + " Premature Closure " + Resources.ValidationMsgs.S3G_SucMsg_Update + "');" + strRedirectPageView, true);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        break;
                    }
                case 8:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Pre Closure Amount has to be collected from Customer.");
                        break;
                    }
                case 9:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Error in Calculating Over Due Interest up to Pre Closure Date  ");
                        break;
                    }
                case 10:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Amount should refund to the customer.");
                        break;
                    }
                case 12:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Billing Regularisation should be run up to Pre closure Date");
                        break;
                    }
                case 13:
                    {
                        Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.S3G_ErrMsg_ODIMonthLockLOB);
                        break;
                    }
                case 14:
                    {
                        Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.S3G_ErrMsg_ODIPrevMonthLockLOB);
                        break;
                    }
                case 15:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Error in Calculating ODI - Memo type was not added in the Memo Master");
                        break;
                    }
                case 19:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Lien Account ‘" + strDocNo + "’ for this Account not closed");
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion [Saving Account Closure Details]

    #region [Email / Print Functionality]

    protected void btnEmail_Click(object sender, EventArgs e)
    {
        bool booMail = true;
        try
        {
            FunPubEmailSent(booMail);
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = "Invalid EMail ID. Mail not sent.";// Resources.ValidationMsgs.S3G_ErrMsg_MailSent;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void PreviewPDF_Click(bool blnPrint)
    {
        try
        {
            FunPriSetFooterValue();
            FunPubPreviewAccountClosure(blnPrint);
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PreviewDoc;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            PreviewPDF_Click(true);
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PreviewDoc;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            PreviewPDF_Click(true);
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PreviewDoc;
            cvAccountPreClosure.IsValid = false;
        }
    }
    #endregion [Email / Print Functionality]

    #region [Premature Account Closure Cancellation // Only in Modify Mode]

    protected void btnClosure_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubCancelPreClosure();
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PreClosure_Cancel;
            cvAccountPreClosure.IsValid = false;
        }
    }

    #endregion [Premature Account Closure Cancellation // Only in Modify Mode]

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage);


    }


    protected void btnCalculateNPV_OnClick(object sender, EventArgs e)
    {
        if (txtfunderprerate.Text == "")
        {
            Utility.FunShowAlertMsg(this.Page, "Enter Funder Pre Closure Rate");
            return;
        }
        hdnIRR.Value = "1";
        FunGetAccountDetailsForClosure_NPV();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@company_id", intCompanyID.ToString());

            dtpv = new DataTable();
            dtpv = Utility.GetDefaultData("S3G_RPT_Get_pmc_pv", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtpv;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=PV Working.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnPrintAnnexures_Click(object sender, EventArgs e)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@closure_ID", Convert.ToString(intClosureDetailId));
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@USER_ID", Convert.ToString(intUserID));

            Procparam.Add("@option", Convert.ToString(2));
            dtpv = new DataTable();
            dtpv = Utility.GetDefaultData("S3G_FUNDMGT_GETPrintPMC", Procparam);

            GridView Grv = new GridView();

            if (dtpv.Rows.Count > 0)
            {
                Grv.DataSource = dtpv;
                Grv.DataBind();
                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;

                string attachment = "attachment; filename=PMC Annexure.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "No Records Found");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    #endregion [Button Event's]

    #endregion [Event's]

    #region [Function's]

    public void FunPubPageLoad()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (Request.QueryString["Popup"] == null)
                FunPubSetIndex(1);

            #region Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            #endregion Date Format

            #region User Authorization

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;

            #endregion User Authorization

            if (Request.QueryString["Popup"] != null)  //transaction screen page load
            {
                btnCancel.Enabled = false;
                btnPrint.Enabled = false;
            }

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            System.Web.HttpContext.Current.Session["intCompanyID"] = ObjUserInfo.ProCompanyIdRW;

            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            System.Web.HttpContext.Current.Session["intUserID"] = ObjUserInfo.ProUserIdRW;

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender2.Format = strDateFormat;
            CalendarExtender3.Format = strDateFormat;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strMode = Request.QueryString.Get("qsMode");

                //strAccountClosure = fromTicket.Name.Split('~')[0];
                //strPANum = fromTicket.Name.Split('~')[1];
                //strSANum = fromTicket.Name.Split('~')[2];
                intClosureDetailId = Convert.ToInt32(fromTicket.Name);
                hidClosureDetailId.Value = intClosureDetailId.ToString();
                //if (strSANum == string.Empty)
                //    strSANum = strPANum + "DUMMY";
            }
            //else
            //{
            //    btnSave.Click += new EventHandler(btnEmail_Click);
            //    btnSave.Click += new EventHandler(btnPrint_Click);
            //}
            if (!IsPostBack)
            {
                //txtPreclosureDate.Text = DateTime.Now.ToString(strDateFormat);
                //txtFunderdate.Text = DateTime.Now.ToString(strDateFormat);
                //PopulateLOBList();
                //if (Request.QueryString["qsMode"] == "C" || Request.QueryString["qsMode"] == "null")
                //{
                PopulateLOBList();
                //}

                System.Web.HttpContext.Current.Session["Tranche_Id"] = 0;
                txtClosureBy.Text = ObjUserInfo.ProUserNameRW.Trim().ToUpper();

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (Request.QueryString["qsMode"] == "Q")
                {
                    //tbReceipt.Visible = true;
                    FunGetAccountsClosedForModify(intClosureDetailId);
                    FunGetAccountDetailsForClosure();
                    FunPriDisableControls(-1);
                }
                else if (Request.QueryString["qsMode"] == "M")
                {
                    //tbReceipt.Visible = true;
                    FunGetAccountsClosedForModify(intClosureDetailId);
                    FunGetAccountDetailsForClosure();
                    FunPriDisableControls(1);
                    btnPrint.Enabled = true;
                    btnPrintST.Enabled = true;
                    btnexportann.Enabled = true;
                }
                else
                {
                    txtPMCReqDate.Text = DateTime.Now.ToString(strDateFormat);
                    txtNOCDate.Text = DateTime.Now.ToString(strDateFormat);
                    txtFunderdate.Text = DateTime.Now.ToString(strDateFormat);
                    txtPreclosureDate.Text = DateTime.Now.ToString(strDateFormat);
                    FunPriDisableControls(0);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #region [LOB/Branch Drop Down List]

    private void PopulateBranchList()
    {
        try
        {
            //Procparam = FunPriProcParam();
            //if (strAccountClosure == string.Empty) Procparam.Add("@Is_Active", "1");
            //Procparam.Add("@User_ID", intUserID.ToString());
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //Procparam.Add("@Program_ID", strProgramId);
            //if (ddlLOB.SelectedValue != "0") Procparam.Add("@Lob_ID", ddlLOB.SelectedValue);
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });

            //if (ddlBranch.Items.Count == 2)
            //{
            //    ddlBranch.SelectedValue = ddlBranch.Items[1].Value;
            //    PopulatePANum();
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void PopulateClosureType()
    {
        try
        {
            Procparam = FunPriProcParam();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            ddlPreType.BindDataTable("S3G_LOANAD_PreMatureClosureType", Procparam, new string[] { "Parameter_Code", "Parameter_Name" });
            ddlPreType.SelectedIndex = 1;
            ddlPreType.ClearDropDownList();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private Dictionary<string, string> FunPriProcParam()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        return Procparam;
    }

    private void PopulateLOBList()
    {
        try
        {
            Procparam = FunPriProcParam();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Program_ID", strProgramId);
            //kuppu - june 21 - to remove WC & FT -
            Procparam.Add("@FilterOption", "'WC','FT'");
            if (strAccountClosure == string.Empty) Procparam.Add("@Is_Active", "1");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.SelectedIndex = 1;
            ddlLOB.ClearDropDownList();
            PopulateClosureType();
            //if (ddlLOB.Items.Count == 2) ddlLOB.SelectedValue = ddlLOB.Items[1].Value;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion [LOB/Branch Drop Down List]

    #region [PANum /SANum Dropdown list]

    private void PopulatePANum()
    {
        try
        {
            //Procparam = FunPriProcParam();
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //Procparam.Add("@User_ID", intUserID.ToString());
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            //Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            //Procparam.Add("@Type", "2");// Closure_Type code
            //Procparam.Add("@Page", "PRC");

            //if (strAccountClosure == string.Empty)
            //    Procparam.Add("@IsModify", "0");
            //else
            //    Procparam.Add("@IsModify", "1");

            //if (txtPMCReqDate.Text.Trim() != "")
            //    Procparam.Add("@PMCDate", Utility.StringToDate(txtPMCReqDate.Text.Trim()).ToString());

            //ddlMLA.BindDataTable(SPNames.S3G_LOANAD_GetPANumForAccountClosure, Procparam, new string[] { "PANum", "PANum" });

            //ddlMLA.Items[0].Text = "--Select--";
            //if (ddlMLA.Items.Count == 2)
            //{
            //    ddlMLA.SelectedValue = ddlMLA.Items[1].Value;
            //    PopulateSANum(ddlMLA.SelectedValue);
            //    PopulatePANCustomer(ddlMLA.SelectedValue);
            //}
            //else if (ddlMLA.Items.Count == 1)
            //{
            //    ddlSLA.Items.Clear();
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void PopulateSANum(string strPAN)
    {
        try
        {
            Procparam = FunPriProcParam();
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@PANum", strPAN);
            Procparam.Add("@Type", "2");
            Procparam.Add("@Page", "PRC");

            if (strAccountClosure == string.Empty)
                Procparam.Add("@IsModify", "0");
            else
                Procparam.Add("@IsModify", "1");

            if (txtPMCReqDate.Text.Trim() != "")
                Procparam.Add("@PMCDate", Utility.StringToDate(txtPMCReqDate.Text.Trim()).ToString());

            // ddlSLA.BindDataTable(SPNames.S3G_LOANAD_GetAccountClosureSANum, Procparam, new string[] { "SANum", "SANum" });

            //if (ddlSLA.Items.Count == 2 && strAccountClosure == "")
            //{
            //    ddlSLA.SelectedValue = ddlSLA.Items[1].Value;
            //}

            //if (ddlSLA.Items.Count > 1)
            //    rfvSLA.Enabled = true;
            //else
            //    rfvSLA.Enabled = false;

            if (strAccountClosure == "")
            {
                //tbAccDetails.Enabled = true;
                tbCashFlow.Enabled = true;
                if (strAccountClosure == string.Empty)
                {
                    //if (ddlSLA.SelectedValue == "0")
                    FunGetAccountDetailsForClosure();
                    //else
                    //    FunGetAccountDetailsForClosure(ddlMLA.SelectedValue, ddlSLA.SelectedValue);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion  [PANum /SANum Dropdown list]

    #region [Save]

    public int FunPubSaveAccountPreClosure(out string strDocNo)
    {
        objAccountClosure_Client = new ContractMgtServicesReference.ContractMgtServicesClient();
        strDocNo = "";
        int intRetvalue = -5;
        dtfunder = (DataTable)ViewState["dtfunder"];
        try
        {
            objS3G_LOANAD_AccClosureTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDataTable();
            objS3G_LOANAD_AccClosureDataRow = objS3G_LOANAD_AccClosureTable.NewS3G_LOANAD_AccountClosureRow();

            objS3G_LOANAD_AccClosureDataRow.Company_ID = intCompanyID;
            objS3G_LOANAD_AccClosureDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objS3G_LOANAD_AccClosureDataRow.Branch_ID = 0;
            objS3G_LOANAD_AccClosureDataRow.Closure_No = txtAccClosureNo.Text.ToString();
            objS3G_LOANAD_AccClosureDataRow.PANum = ddlMLA.SelectedText.ToString();
            // if (ddlSLA.SelectedValue != "0") objS3G_LOANAD_AccClosureDataRow.SANum = ddlSLA.SelectedValue;
            // else
            objS3G_LOANAD_AccClosureDataRow.SANum = ddlMLA.SelectedText + "DUMMY";
            objS3G_LOANAD_AccClosureDataRow.Customer_ID = Convert.ToInt32(hdnCustomerID.Value);  // Convert.ToInt32(txtCustCode.Attributes["Cust_ID"]);
            objS3G_LOANAD_AccClosureDataRow.Closure_Date = Utility.StringToDate(txtPreclosureDate.Text);
            objS3G_LOANAD_AccClosureDataRow.Created_By = intUserID;
            objS3G_LOANAD_AccClosureDataRow.Created_On = DateTime.Now;
            objS3G_LOANAD_AccClosureDataRow.User_ID = intUserID;
            objS3G_LOANAD_AccClosureDataRow.Tranche_id = Convert.ToInt32(uctranche.SelectedValue.ToString());
            objS3G_LOANAD_AccClosureDataRow.Fund_Closure_Date = Utility.StringToDate(txtFunderdate.Text);
            objS3G_LOANAD_AccClosureDataRow.OPC_Closure_Rate = txtPreRate.Text.ToString();
            objS3G_LOANAD_AccClosureDataRow.OPC_Breaking_Rate = txtDiscRate.Text.ToString();
            objS3G_LOANAD_AccClosureDataRow.Fund_Closure_Rate = txtfunderprerate.Text.ToString();
            objS3G_LOANAD_AccClosureDataRow.Fund_Breaking_Rate = txtbreaking.Text.ToString();
            objS3G_LOANAD_AccClosureDataRow.IS_AMF = chkAMF.Checked.ToString();
            objS3G_LOANAD_AccClosureDataRow.IS_VAT = chkVAT.Checked.ToString();
            objS3G_LOANAD_AccClosureDataRow.IS_ST = chkServiceTax.Checked.ToString();
            objS3G_LOANAD_AccClosureDataRow.IS_WAIV = chkwaived.Checked.ToString();
            objS3G_LOANAD_AccClosureDataRow.NOC_date = Utility.StringToDate(txtNOCDate.Text);
            objS3G_LOANAD_AccClosureDataRow.fund_Xml = dtfunder.FunPubFormXml();
            if (txtPreAmount.Text.Trim() != "")
                objS3G_LOANAD_AccClosureDataRow.Closure_Amount = Convert.ToDecimal(txtPreAmount.Text);
            else
                objS3G_LOANAD_AccClosureDataRow.Closure_Amount = 0;
            objS3G_LOANAD_AccClosureDataRow.XMLAccountClosureDetails = FunPriGenerateAccountClosureDetailsXMLDet();

            //Added By Chandru K On 24-Sep-2013 For ISFC
            //if (chkBoxWriteOff.Checked)
            //    objS3G_LOANAD_AccClosureDataRow.WriteOff = 1;
            //else
            //    objS3G_LOANAD_AccClosureDataRow.WriteOff = 0;
            //if (txtWriteOffAmount.Text != "")
            //    objS3G_LOANAD_AccClosureDataRow.WriteOff_Amount = Convert.ToDecimal(txtWriteOffAmount.Text);
            //else
            //    objS3G_LOANAD_AccClosureDataRow.WriteOff_Amount = 0;
            //objS3G_LOANAD_AccClosureDataRow.Remarks = "";
            //End

            objS3G_LOANAD_AccClosureTable.AddS3G_LOANAD_AccountClosureRow(objS3G_LOANAD_AccClosureDataRow);

            intRetvalue = objAccountClosure_Client.FunPubCreateAccountClosure(out strDocNo, out intClosureDetailId, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_AccClosureTable, ObjSerMode), 1);
        }
        catch (FaultException<ContractMgtServicesReference.ClsPubFaultException> ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
            throw;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
            throw;
        }
        finally
        {
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
        }
        return intRetvalue;
    }

    #endregion [Save]

    private void FunPriSetFooterValue()
    {
        try
        {
            if (grvCashFlow.FooterRow != null)
            {
                grvCashFlow.FooterRow.Cells[3].Text = dcDue.ToString(Funsetsuffix());
                grvCashFlow.FooterRow.Cells[5].Text = dcWaived.ToString(Funsetsuffix());
                grvCashFlow.FooterRow.Cells[4].Text = dcPayable.ToString(Funsetsuffix());
                grvCashFlow.FooterRow.Cells[6].Text = dcClosure.ToString(Funsetsuffix());
                // grvCashFlow.FooterRow.Cells[7].Text = dcReceived.ToString(Funsetsuffix());

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriSetABFooterValue()
    {
        try
        {
            //if (grvAccountBalance.FooterRow != null)
            //{
            //    grvAccountBalance.FooterRow.Cells[3].Text = dcFooterDue.ToString(Funsetsuffix());
            //   grvAccountBalance.FooterRow.Cells[4].Text = dcFooterReceived.ToString(Funsetsuffix());
            //    grvAccountBalance.FooterRow.Cells[5].Text = dcFooterOS.ToString(Funsetsuffix());
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }


    #region [Email Sent]

    public void FunPubEmailSent(bool booMail)
    {
        FunPriSetFooterValue();
        string strCustomerEmail = (ucdCustomer.FindControl("txtEmail") as TextBox).Text.Trim();
        if (booMail && strCustomerEmail == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Customer does not have an Email Id');", true);
            return;
        }

        if (strCustomerEmail != string.Empty)
        {
            if (strnewFile == "")
                PreviewPDF_Click(false);

            Dictionary<string, string> dictMail = new Dictionary<string, string>();
            dictMail.Add("FromMail", "s3g@sundaraminfotech.in");
            dictMail.Add("ToMail", strCustomerEmail);
            dictMail.Add("Subject", "Account Closure Statement");
            ArrayList arrMailAttachement = new ArrayList();
            StringBuilder strBody = new StringBuilder();
            strBody = GetHTMLTextEmail();
            if (strnewFile != "")
            {
                arrMailAttachement.Add(strnewFile);
            }

            Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);

            /*CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
            try
            {
                string body;
                body = GetHTMLTextEmail();
                ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
                ObjCom_Mail.ProFromRW = "s3g@sundaraminfotech.in";
                ObjCom_Mail.ProTORW = strCustomerEmail;
                ObjCom_Mail.ProSubjectRW = "Account Closure Statement";
                ObjCom_Mail.ProMessageRW = body;
                if (strnewFile != "")
                {
                    ArrayList attArrayList = new ArrayList();
                    attArrayList.Add(strnewFile);
                    ObjCom_Mail.ProFileAttachementRW = attArrayList;
                }
                ObjCommonMail.FunSendMail(ObjCom_Mail);
                if (booMail)
                    Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_SucMsg_MailSent); //"Mail sent successfully");
            }
            catch (FaultException<CommonMailServiceReference.ClsPubFaultException> ex)
            {
                //if (booMail && ex.Message == "Mailbox unavailable. The server response was: 5.7.1 Unable to relay")
                //{
                //    Utility.FunShowAlertMsg(this, " Invalid EMail ID. Mail not sent." + Resources.ValidationMsgs.S3G_SucMsg_LoanAd_PreMature_Closed + " - " + strAccountClosure);
                //}

                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
                if (ObjCommonMail != null)
                    ObjCommonMail.Close();
                throw;
            }
            catch (Exception ex)
            {
                //if (booMail && ex.Message == "Mailbox unavailable. The server response was: 5.7.1 Unable to relay")
                //{
                //    Utility.FunShowAlertMsg(this, " Invalid EMail ID. Mail not sent." + Resources.ValidationMsgs.S3G_SucMsg_LoanAd_PreMature_Closed + " - " + strAccountClosure);
                //}
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
                if (ObjCommonMail != null)
                    ObjCommonMail.Close();
                throw;
            }
            finally
            {
                if (ObjCommonMail != null)
                    ObjCommonMail.Close();
            }*/


        }
    }

    #endregion [Email Sent]

    #region [Premature AccountPreclosure Preview]

    string strnewFile = "";
    public void FunPubPreviewAccountClosure(bool blnPrint)
    {
        try
        {
            String htmlText = GetHTMLText();
            Guid objGuid;
            objGuid = Guid.NewGuid();

            DataSet Dst = null;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@PANum", ddlMLA.SelectedValue);
            // if (ddlSLA.SelectedValue == "0")
            Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
            //else
            //    Procparam.Add("@SANum", ddlSLA.SelectedValue);
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            // Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@Closure_Details_ID", hidClosureDetailId.Value);

            Dst = Utility.GetDataset("S3G_LOANAD_AccountClosureStatement", Procparam);


            //rptd.Load(Server.MapPath("PrematureClosure.rpt"));
            //rptd.SetDataSource(Dst.Tables[0]);
            //rptd.Subreports["AccountDetails"].SetDataSource(Dst.Tables[1]);
            //rptd.Subreports["Assetdetails"].SetDataSource(Dst.Tables[2]);
            //if (Dst.Tables[3] != null) rptd.Subreports["Pdc details"].SetDataSource(Dst.Tables[3]);
            //if (Dst.Tables[4] != null) rptd.Subreports["Collateral"].SetDataSource(Dst.Tables[4]);

            ////string strClosureSt = ddlMLA.SelectedValue.Replace("/", "");
            ////if (ddlSLA.SelectedValue != "0") strClosureSt = strClosureSt + ddlSLA.SelectedValue.Replace("/", "");
            ////if(Dst.Tables[0]!=null && Dst.Tables[0].Rows[0]["Closure_Details_ID"]!=null) strClosureSt = strClosureSt + Dst.Tables[0].Rows[0]["Closure_Details_ID"].ToString();

            //strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + objGuid + "Closure Statement.pdf");

            ///*if (File.Exists(strnewFile))
            //    File.Delete(strnewFile);*/

            //rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);//Server.MapPath(".") + "\\PDF Files\\" + ddlSLA.SelectedValue);
            string strScipt = "";

            if (blnPrint)
            {
                strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strnewFile.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
                //System.Diagnostics.Process.Start(strnewFile);
            }
        }
        catch (IOException winOpne)
        {
            if (blnPrint)
                System.Diagnostics.Process.Start(strnewFile);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvAccountPreClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PreviewDoc;
            cvAccountPreClosure.IsValid = false;
        }
        finally
        {
            //if (rptd != null)
            //{
            //    rptd.Close();
            //    rptd.Dispose();
            //}
        }
    }

    #endregion [Premature AccountPreclosure Preview]

    #region [Canceling the Account PreClosure]

    public void FunPubCancelPreClosure()
    {
        objAccountClosure_Client = new ContractMgtServicesReference.ContractMgtServicesClient();
        try
        {
            objCancelDataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureDataTable();
            objCancelRow = objCancelDataTable.NewS3G_LOANAD_CancelClosureRow();
            objCancelRow.PANum = strPANum;
            objCancelRow.Closure_No = txtAccClosureNo.Text.ToString();
            objCancelRow.SANum = strSANum;
            objCancelRow.Closure_Type = 2;
            objCancelRow.Company_ID = intCompanyID;
            objCancelRow.User_Id = intUserID;

            objCancelDataTable.AddS3G_LOANAD_CancelClosureRow(objCancelRow);
            intErrCode = objAccountClosure_Client.FunPubCancelAccountClosure(ObjSerMode, ClsPubSerialize.Serialize(objCancelDataTable, ObjSerMode), 1);

            if (intErrCode == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Premature Closure cancelled sucessfully ');" + strRedirectPageView, true);
            }
            else if (intErrCode == 105)
            {
                Utility.FunShowAlertMsg(this, "Error in Posting");
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Account Closure  has gone through approval and cannot be cancelled');" + strRedirectPageView, true);
            }
        }
        catch (FaultException<ContractMgtServicesReference.ClsPubFaultException> ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
            throw;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
            throw;
        }
        finally
        {
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
        }
    }

    #endregion [Canceling the Account PreClosure]

    private void PopulatePANCustomer()
    {
        try
        {
            DataTable dtTable = new DataTable();
            Procparam = FunPriProcParam();
            if (ddlMLA.SelectedValue.ToString() != "0")
                Procparam.Add("@PANum", ddlMLA.SelectedValue);
            Procparam.Add("@program_id", strProgramId);
            if (Convert.ToInt32(uctranche.SelectedValue) > 0)
                Procparam.Add("@tranche_id", uctranche.SelectedValue);
            dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetPANumCustomer, Procparam);

            if (dtTable != null && dtTable.Rows.Count > 0)
            {
                dtTable.Rows[0]["Customer_Name"] = Convert.ToString(dtTable.Rows[0]["Customer_Name"]);
                ucdCustomer.SetCustomerDetails(dtTable.Rows[0], true);
                //strCustomerEmail = dtTable.Rows[0]["Comm_Email"].ToString();
                hdnCustomerID.Value = dtTable.Rows[0]["Customer_ID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunGetAccountDetailsForClosure()
    {
        int intTenureType = 0;
        double intTenure = 0;
        DateTime AccDate = new DateTime();
        string strTenureDesc = string.Empty;
        int Tenure = 0;

        try
        {
            DataSet dtSet = new DataSet();
            Procparam = FunPriProcParam();
            if (ddlMLA.SelectedValue.ToString() != "0")
                Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            // Procparam.Add("@SANum", strSANum);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            // Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@ClosureDetailId", intClosureDetailId.ToString());
            Procparam.Add("@PAGE", "PMC");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Tranche_id", uctranche.SelectedValue);
            if (txtPMCReqDate.Text.Trim() != "")
                Procparam.Add("@PMCDate", Utility.StringToDate(txtPMCReqDate.Text).ToString());

            dtSet = Utility.GetTableValues("S3G_LOANAD_GetAccountDetailsForClosure", Procparam);


            if (dtSet != null)
            {
                //if (dtSet.Tables[0] != null && dtSet.Tables[0].Rows.Count > 0)
                //{
                //    txtAccDate.Text = dtSet.Tables[0].Rows[0]["Creation_Date"].ToString();

                //    TxtAccStatus.Text = dtSet.Tables[0].Rows[0]["Status"].ToString();

                //   // txtPrincipal.Text = dtSet.Tables[0].Rows[0]["Finance_Amount"].ToString();

                //    intTenureType = Convert.ToInt32(dtSet.Tables[0].Rows[0]["Tenure_Code"]);
                //    intTenure = Convert.ToDouble(dtSet.Tables[0].Rows[0]["Tenure"]);

                //    if (dtSet.Tables[0].Rows[0]["MatureDate"].ToString() != "") 
                //        txtMatureDate.Text = dtSet.Tables[0].Rows[0]["MatureDate"].ToString();
                //    txtIRR.Text = dtSet.Tables[0].Rows[0]["Accounting_IRR"].ToString();
                //    txtBusinessIRR.Text = dtSet.Tables[0].Rows[0]["Business_IRR"].ToString();
                //    txtCompanyIRR.Text = dtSet.Tables[0].Rows[0]["Company_IRR"].ToString();
                //    strTenureDesc = dtSet.Tables[0].Rows[0]["TenureDesc"].ToString();
                //    txtTenure.Text = dtSet.Tables[0].Rows[0]["Tenure"].ToString() + " " + strTenureDesc;

                //    Tenure = Int32.Parse(dtSet.Tables[0].Rows[0]["Tenure"].ToString());
                //   // txtFinanceCharge.Text = dtSet.Tables[0].Rows[0]["finance_charge"].ToString();
                //}
                if (dtSet.Tables[0] != null && dtSet.Tables[0].Rows.Count > 0)
                {
                    grvaccount.DataSource = dtSet.Tables[0];
                    grvaccount.DataBind();
                }

                //if (dtSet.Tables[2] != null && dtSet.Tables[2].Rows.Count > 0)
                //{
                //    grvAsset.DataSource = dtSet.Tables[2];
                //    grvAsset.DataBind();
                //}
                //if (dtSet.Tables[1] != null && dtSet.Tables[1].Rows.Count > 0)
                //{
                //    grvAccountBalance.DataSource = dtSet.Tables[1];
                //    grvAccountBalance.DataBind();
                //    FunPriBindAccountBalanceGrid(dtSet.Tables[1]);
                //}
                if (strMode.ToString() == "C")
                {
                    if (ddlPreType.Items.Count > 1)
                        ddlPreType.SelectedIndex = 1;
                    ddlPreType.Enabled = false;
                }


                //if (intClosureDetailId == 0)
                //{
                //    

                //    //if (dtSet.Tables[5].Rows[0]["PRINCIPLE_RATE"].ToString() != "")
                //    //{
                //    //    Principle_Rate = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["PRINCIPLE_RATE"].ToString());
                //    //}
                //    //else
                //    //{
                //    //    Principle_Rate = 0;
                //    //}
                //    txtPreRate.ReadOnly = false;
                //    txtPreRate.Text = Principle_Rate.ToString();

                //    if (ddlPreType.SelectedValue == "30")
                //    {
                //        txtPreRate.ReadOnly = true;
                //    }
                //}

                //if (dtSet.Tables[5].Rows[0]["Bill_InstallmentNo"].ToString() != "")
                //    Bill_InstallmentNo = Convert.ToInt32(dtSet.Tables[5].Rows[0]["Bill_InstallmentNo"].ToString());
                //if (dtSet.Tables[5].Rows[0]["Repayment_Mode_Code"].ToString() != "")
                //    Repayment_Mode_Code = Convert.ToInt32(dtSet.Tables[5].Rows[0]["Repayment_Mode_Code"].ToString());
                //if (dtSet.Tables[5].Rows[0]["Residual_Value"].ToString() != "")
                //    Residual_Value = Convert.ToInt32(dtSet.Tables[5].Rows[0]["Residual_Value"].ToString());
                //if (dtSet.Tables[5].Rows[0]["Next_InstallmentDt"].ToString() != "")
                //    Next_InstallmentDt = Utility.StringToDate(dtSet.Tables[5].Rows[0]["Next_InstallmentDt"].ToString());
                //if (dtSet.Tables[5].Rows[0]["Current_InstallmentDt"].ToString() != "")
                //    Current_InstallmentDt = Utility.StringToDate(dtSet.Tables[5].Rows[0]["Current_InstallmentDt"].ToString());
                //if (dtSet.Tables[5].Rows[0]["First_InstallmentDate"].ToString() != "")
                //    First_InstallmentDate = Utility.StringToDate(dtSet.Tables[5].Rows[0]["First_InstallmentDate"].ToString());
                //if (dtSet.Tables[5].Rows[0]["RoundOff"].ToString() != "")
                //    RoundOff = Convert.ToInt32(dtSet.Tables[5].Rows[0]["RoundOff"].ToString());
                //if (dtSet.Tables[5].Rows[0]["decPLR"].ToString() != "")
                //    decPLR = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["decPLR"].ToString());
                //if (dtSet.Tables[5].Rows[0]["decCTR"].ToString() != "")
                //    decCTR = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["decCTR"].ToString());
                //if (dtSet.Tables[5].Rows[0]["NextMth_FinanceCharge"].ToString() != "")
                //    NextMth_FinanceCharge = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["NextMth_FinanceCharge"].ToString());
                //if (dtSet.Tables[5].Rows[0]["CurrentMth_FinanceCharge"].ToString() != "")
                //    CurrentMth_FinanceCharge = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["CurrentMth_FinanceCharge"].ToString());

                //Frequency = dtSet.Tables[5].Rows[0]["Frequency"].ToString();
                //Time_Value = dtSet.Tables[5].Rows[0]["Time_Value"].ToString();
                //if (dtSet.Tables[5].Rows[0]["TotalPaid_Interest"].ToString() != "")
                //    TotalPaid_Interest = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["TotalPaid_Interest"].ToString());


                //else
                //{
                //    if (dtSet.Tables[5].Rows[0]["FuturePrincipalAmt"].ToString() != "")
                //        FuturePrincipalAmt = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["FuturePrincipalAmt"].ToString());
                //}
                //if (dtSet.Tables[5].Rows[0]["LPO"].ToString() != "")
                //    booLPO = Convert.ToBoolean(dtSet.Tables[5].Rows[0]["LPO"].ToString());

                ViewState["dtTax"] = dtSet.Tables[5];

            }

            ViewState["Company"] = dtSet.Tables[2];
            if (strMode == "")
            {
                if (dtSet.Tables[3].Rows.Count > 0)
                {
                    if (dtSet.Tables[3].Rows[0]["pre_rate"].ToString() != "")
                        txtPreRate.Text = Convert.ToDecimal(dtSet.Tables[3].Rows[0]["pre_rate"].ToString()).ToString(Funsetsuffix());
                    if (dtSet.Tables[3].Rows[0]["disc_rate"].ToString() != "")
                        txtDiscRate.Text = Convert.ToDecimal(dtSet.Tables[3].Rows[0]["disc_rate"].ToString()).ToString(Funsetsuffix());
                    if (dtSet.Tables[3].Rows[0]["Fund_closure_rate"].ToString() != "")
                        txtfunderprerate.Text = Convert.ToDecimal(dtSet.Tables[3].Rows[0]["Fund_closure_rate"].ToString()).ToString(Funsetsuffix());
                }
            }
            if (dtSet.Tables[4].Rows.Count > 0)
            {
                grvasset1.DataSource = dtSet.Tables[4];
                grvasset1.DataBind();
            }


        }
        catch (FaultException<ContractMgtServicesReference.ClsPubFaultException> ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        finally
        {

        }
    }

    private void FunGetAccountDetailsForClosure_NPV()
    {
        int intTenureType = 0;
        double intTenure = 0;
        DateTime AccDate = new DateTime();
        string strTenureDesc = string.Empty;
        int Tenure = 0;

        try
        {
            DataSet dtSet = new DataSet();
            Procparam = FunPriProcParam();
            if (ddlMLA.SelectedValue.ToString() != "")
                Procparam.Add("@PANum", ddlMLA.SelectedValue);
            // Procparam.Add("@SANum", strSANum);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            // Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@ClosureDetailId", intClosureDetailId.ToString());
            Procparam.Add("@PAGE", "PMC");
            Procparam.Add("@option", "1");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Tranche_id", uctranche.SelectedValue);
            if (txtPreRate.Text.ToString() != "")
                Procparam.Add("@Pre_Closure_Rate", txtPreRate.Text.ToString());
            else
                Procparam.Add("@Pre_Closure_Rate", "0");
            if (txtDiscRate.Text.ToString() != "")
                Procparam.Add("@Breaking_charges", txtDiscRate.Text.ToString());
            else
                Procparam.Add("@Breaking_charges", "0");
            if (txtfunderprerate.Text.ToString() != "")
                Procparam.Add("@Funder_Closure_Rate", txtfunderprerate.Text.ToString());
            else
                Procparam.Add("@Funder_Closure_Rate", "0");
            // Procparam.Add("@Funder_Breaking_Cost", txtBreakingCharges.Text.ToString());
            Procparam.Add("@Is_AMF", Convert.ToInt16(chkAMF.Checked).ToString());
            Procparam.Add("@Is_VAT", Convert.ToInt16(chkVAT.Checked).ToString());
            Procparam.Add("@IS_ST", Convert.ToInt16(chkServiceTax.Checked).ToString());
            if (txtFunderdate.Text.Trim() != "")
                Procparam.Add("@Fund_PMCDate", Utility.StringToDate(txtFunderdate.Text).ToString());
            if (txtPMCReqDate.Text.Trim() != "")
                Procparam.Add("@PMCDate", Utility.StringToDate(txtPMCReqDate.Text).ToString());
            if (chkwaived.Checked == true)
                Procparam.Add("@Is_Waiv", "1");

            dtSet = Utility.GetTableValues("S3G_LOANAD_GetAccountDetailsForClosure", Procparam);

            ViewState["ClosureStatement"] = dtSet.Tables[0];
            if (dtSet.Tables[0] != null && dtSet.Tables[0].Rows.Count > 0)
            {
                Panel2.Visible = true;
                div2.Style.Add("display", "block");
                FunPriBindCashFlowGrid();
                //if (grvCashFlow.Rows.Count != 0)
                //{
                //    grvCashFlow.HeaderRow.Style.Add("position", "relative");
                //    grvCashFlow.HeaderRow.Style.Add("z-index", "auto");
                //    grvCashFlow.HeaderRow.Style.Add("top", "auto");

                //}
            }

            txtbreaking.Text = dtSet.Tables[2].Rows[0]["breaking_charge"].ToString();
            if (dtSet.Tables[1].Rows.Count > 0)
            {
                Panel5.Visible = true;
                grvfunderdetailss.DataSource = dtSet.Tables[1];
                grvfunderdetailss.DataBind();
                dcfunderdue = Convert.ToDecimal(dtSet.Tables[1].Compute("sum(due1)", ""));
                dcfunderfuturerec = Convert.ToDecimal(dtSet.Tables[1].Compute("sum(Future_receivables1)", ""));
                dcfundernpv = Convert.ToDecimal(dtSet.Tables[1].Compute("sum(NPV_amount1)", ""));

                grvfunderdetailss.FooterRow.Cells[3].Text = dcfunderdue.ToString(Funsetsuffix());
                grvfunderdetailss.FooterRow.Cells[4].Text = dcfunderfuturerec.ToString(Funsetsuffix());
                grvfunderdetailss.FooterRow.Cells[5].Text = dcfundernpv.ToString(Funsetsuffix());

                ViewState["dtfunder"] = dtSet.Tables[1];

                txtToTal.Text = (Convert.ToDecimal(txtbreaking.Text.ToString()) + dcfundernpv).ToString(Funsetsuffix());

            }

            if (dtSet.Tables[3].Rows.Count > 0)
            {
                ViewState["dtTax"] = dtSet.Tables[3];

            }
            txtPreAmount.Text = (dcClosure).ToString();

        }
        catch (FaultException<ContractMgtServicesReference.ClsPubFaultException> ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        finally
        {

        }
    }


    protected void txtbreaking_OnTextChanged(object sender, EventArgs e)
    {
        try
        {

            if (txtbreaking.Text.ToString() == "")
                txtbreaking.Text = "0.00";
            txtToTal.Text = (Convert.ToDecimal(txtbreaking.Text.ToString()) + dcfundernpv).ToString(Funsetsuffix());
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }


    #region [User Authorization]

    /// <summary>
    /// This is used to implement User Authorization
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                txtStatus.Text = "Pending";
                txtPMCReqDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtPMCReqDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtFunderdate.Attributes.Add("onblur", "fnDoDate(this,'" + txtFunderdate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtNOCDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtNOCDate.ClientID + "','" + strDateFormat + "',false,  true);");
                break;

            case 1: // Modify Mode

                txtPreRate.Enabled = uctranche.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                // tbAccDetails.Enabled = 
                tbCashFlow.Enabled = true;
                if (bDelete)
                    btnClosure.Visible = true;
                txtClosureBy.ReadOnly = txtPMCReqDate.ReadOnly = true;
                CalendarExtender2.Enabled = ddlPreType.Enabled = false;
                txtPMCReqDate.Attributes.Remove("onblur");
                //btnPrint.Enabled = true;
                break;


            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }
                ddlPreType.Enabled = CalendarExtender2.Enabled = btnSave.Enabled = false;
                //tbAccDetails.Enabled 
                tbCashFlow.Enabled = true;
                txtClosureBy.ReadOnly = txtPMCReqDate.ReadOnly = true;
                btnEmail.Enabled = uctranche.Enabled = false;
                btnPrint.Enabled = btnPrintST.Enabled = btnexportann.Enabled = true;
                btnCalculateNPV.Enabled = CalendarExtender1.Enabled = false;
                txtDiscRate.Attributes.Add("readonly", "readonly");
                txtfunderprerate.Attributes.Add("readonly", "readonly");
                txtPreRate.Attributes.Add("readonly", "readonly");
                txtFunderdate.Attributes.Add("readonly", "readonly");
                chkAMF.Enabled = chkServiceTax.Enabled = chkVAT.Enabled = chkwaived.Enabled = false;
                txtPMCReqDate.Attributes.Remove("onblur");
                if (bClearList)
                {
                    //ddlBranch.ClearDropDownList();
                    //ddlLOB.ClearDropDownList();
                    //ddlMLA.ClearDropDownList();
                    // ddlSLA.ClearDropDownList();
                    //ddlInvoice.ClearDropDownList();
                }
                foreach (GridViewRow gvRow in grvCashFlow.Rows)
                {
                    TextBox txtWaived = (TextBox)gvRow.FindControl("txtWaived");
                    //TextBox txtRemarks = (TextBox)gvRow.FindControl("txtRemarks");
                    txtWaived.ReadOnly = true;
                }
                break;

        }
    }

    #endregion  [User Authorization]

    #region "To Get details for Modification"

    private void FunGetAccountsClosedForModify(int intClosureDetailId)
    {
        DataTable dtTable = new DataTable();


        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@closure_id", intClosureDetailId.ToString());
            Procparam.Add("@USER_ID", intUserID.ToString());
            Procparam.Add("@company_id", intCompanyID.ToString());
            dtclosure = Utility.GetDataset("S3G_LOANAD_Get_DetailsPMC", Procparam);
            dtTable = dtclosure.Tables[0];
            if (dtTable != null && dtTable.Rows.Count > 0)
            {

                if (dtTable.Rows[0]["PreClosure_Date"].ToString() != "")
                    txtPMCReqDate.Text = DateTime.Parse(dtTable.Rows[0]["PreClosure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                if (dtTable.Rows[0]["Closure_Date"].ToString() != "")
                    txtPreclosureDate.Text = DateTime.Parse(dtTable.Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                //ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dtTable.Rows[0]["LOBName"].ToString(), ddlMLA.SelectedValue = dtTable.Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = dtTable.Rows[0]["LOBName"].ToString();
                uctranche.SelectedText = dtTable.Rows[0]["Tranche_Name"].ToString();
                uctranche.SelectedValue = dtTable.Rows[0]["Tranche_Id"].ToString();
                txtAccClosureNo.Text = dtTable.Rows[0]["closure_No"].ToString();
                //FunGetAccountDetailsForClosure();

                //ddlLOB.SelectedValue = dtTable.Rows[0]["LOB_ID"].ToString();
                FunProToggleRental();
                ddlMLA.SelectedValue = dtTable.Rows[0]["PA_SA_Ref_Id"].ToString();
                ddlMLA.SelectedText = dtTable.Rows[0]["PANum"].ToString();
                ddlMLA.ToolTip = dtTable.Rows[0]["PANum"].ToString();

                PopulatePANCustomer();

                //eND

                chkAMF.Checked = Convert.ToBoolean(dtTable.Rows[0]["IS_AMF"].ToString());
                chkVAT.Checked = Convert.ToBoolean(dtTable.Rows[0]["IS_VAT"].ToString());
                chkwaived.Checked = Convert.ToBoolean(dtTable.Rows[0]["IS_WAIV"].ToString());

                chkServiceTax.Checked = Convert.ToBoolean(dtTable.Rows[0]["IS_ST"].ToString());
                txtNOCDate.Text = dtTable.Rows[0]["NOD_Date"].ToString();
                txtFunderdate.Text = dtTable.Rows[0]["Fund_Closure_Date"].ToString();
                txtPreRate.Text = Convert.ToDecimal(dtTable.Rows[0]["OPC_Closure_Rate"].ToString()).ToString(Funsetsuffix());
                txtDiscRate.Text = Convert.ToDecimal(dtTable.Rows[0]["OPC_Breaking_Rate"].ToString()).ToString(Funsetsuffix());
                txtfunderprerate.Text = Convert.ToDecimal(dtTable.Rows[0]["Fund_Closure_Rate"].ToString()).ToString(Funsetsuffix());
                txtbreaking.Text = Convert.ToDecimal(dtTable.Rows[0]["fund_breaking_rate"].ToString()).ToString(Funsetsuffix());
                PopulateClosureType();
                txtPreAmount.Text = Convert.ToDecimal(dtTable.Rows[0]["Closure_Amount"].ToString()).ToString(Funsetsuffix());
                System.Web.UI.WebControls.ListItem objList;
                switch (dtTable.Rows[0]["PreClosure_Type"].ToString())
                {
                    case "28":
                        objList = new System.Web.UI.WebControls.ListItem("IRR", dtTable.Rows[0]["PreClosure_Type"].ToString());
                        ddlPreType.Items.Insert(0, objList);
                        break;
                    case "29":
                        objList = new System.Web.UI.WebControls.ListItem("Net Present Value", dtTable.Rows[0]["PreClosure_Type"].ToString());
                        ddlPreType.Items.Insert(0, objList);
                        break;
                    case "30":
                        objList = new System.Web.UI.WebControls.ListItem("Principal", dtTable.Rows[0]["PreClosure_Type"].ToString());
                        ddlPreType.Items.Insert(0, objList);
                        break;
                }

                //if (ddlLOB.SelectedItem.Text.Trim().Contains("Working") || ddlLOB.SelectedItem.Text.Trim().Contains("Factor"))
                //    txtPreRate.Text = "";
                //else
                //    txtPreRate.Text = dtTable.Rows[0]["Closure_Rate"].ToString();

                txtClosureBy.Text = dtTable.Rows[0]["User_Name"].ToString();
                btnEmail.Enabled = btnPrint.Enabled = btnClosure.Enabled = btnSave.Enabled = false;

                if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "1")
                {
                    txtStatus.Text = "Pending";
                    btnClosure.Enabled = btnSave.Enabled = true;
                }
                else if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "2")
                    txtStatus.Text = "Under Progress";
                else if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "3")
                {
                    txtStatus.Text = "Approved";
                    btnEmail.Enabled = btnPrint.Enabled = true;
                }
                else if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "4")
                    txtStatus.Text = "Rejected";
                else if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "5")
                    txtStatus.Text = "Cancelled";
            }
            ddlLOB.Enabled = true;
            ddlMLA.Enabled = false;
            if (ddlLOB.SelectedItem.Text.Trim().Contains("Working") || ddlLOB.SelectedItem.Text.Trim().Contains("Factor"))
                rfvPreType.Enabled = rfvPreRate.Enabled = ddlPreType.Enabled = txtPreRate.Enabled = false;
            else
                rfvPreType.Enabled = rfvPreRate.Enabled = ddlPreType.Enabled = txtPreRate.Enabled = true;
            ViewState["ClosureStatement"] = dtclosure.Tables[1];
            if (dtclosure.Tables[1] != null && dtclosure.Tables[1].Rows.Count > 0)
            {
                Panel2.Visible = true;
                div2.Style.Add("display", "block");
                FunPriBindCashFlowGrid();
            }
            if (dtclosure.Tables[2].Rows.Count > 0)
            {
                grvfunderdetailss.DataSource = dtclosure.Tables[2];
                grvfunderdetailss.DataBind();
                dcfunderdue = Convert.ToDecimal(dtclosure.Tables[2].Compute("sum(due1)", ""));
                dcfunderfuturerec = Convert.ToDecimal(dtclosure.Tables[2].Compute("sum(Future_receivables1)", ""));
                dcfundernpv = Convert.ToDecimal(dtclosure.Tables[2].Compute("sum(NPV_amount1)", ""));

                grvfunderdetailss.FooterRow.Cells[3].Text = dcfunderdue.ToString(Funsetsuffix());
                grvfunderdetailss.FooterRow.Cells[4].Text = dcfunderfuturerec.ToString(Funsetsuffix());
                grvfunderdetailss.FooterRow.Cells[5].Text = dcfundernpv.ToString(Funsetsuffix());

                ViewState["dtfunder"] = dtclosure.Tables[2];

                txtToTal.Text = (Convert.ToDecimal(txtbreaking.Text.ToString()) + dcfundernpv).ToString(Funsetsuffix());

            }

            txtPreAmount.Text = Convert.ToDecimal((dcClosure).ToString()).ToString(Funsetsuffix());

        }
        catch (FaultException<ContractMgtServicesReference.ClsPubFaultException> ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
            throw;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
            throw;
        }
        finally
        {
            if (objAccountClosure_Client != null)
                objAccountClosure_Client.Close();
        }
    }

    #endregion

    #region PMC Amount Calcualtion

    private void FunPubCalCulatePMCAmount()
    {
        int intDtCom = 0; string strEndDate = "";
        //if (strAccountClosure == string.Empty && txtPMCReqDate.Text.Trim() != "" && txtMatureDate.Text.Trim() != "")
        //{
        //    intDtCom = Utility.CompareDates(txtMatureDate.Text, txtPMCReqDate.Text);
        //    if (intDtCom != -1)
        //    {
        //        Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_LT_MaturityDt);
        //        txtPMCReqDate.Text = txtPreclosureDate.Text = DateTime.Now.ToString(strDateFormat);
        //    }
        //}

        if (strAccountClosure == string.Empty)
        {
            strEndDate = DateTime.Now.Month + "-" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + "-" + DateTime.Now.Year;
            intDtCom = Utility.CompareDates(Convert.ToDateTime(strEndDate).ToString(strDateFormat), txtPMCReqDate.Text);
            if (intDtCom == 1)
            {
                Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_WT_currentDt);
                txtPMCReqDate.Text = txtPreclosureDate.Text = DateTime.Now.ToString(strDateFormat);
            }
            intDtCom = Utility.CompareDates(DateTime.Now.ToString(strDateFormat), txtPMCReqDate.Text);
            if (intDtCom == -1)
            {
                Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_EqlOrGT_CurrentDt);
                txtPMCReqDate.Text = txtPreclosureDate.Text = DateTime.Now.ToString(strDateFormat);
            }
        }

        hdnIRR.Value = "0";

        //Code modified 
        txtPreAmount.Text = "";
        //txtPreAmount.Text = "0.00";

        if (ddlPreType.SelectedValue != "0" && txtPreRate.Text.Trim() != "" && txtPMCReqDate.Text.Trim() != "" && Convert.ToDecimal(txtPreRate.Text.Trim()) <= 100)
        {
            //if (strActivationType.ToUpper() != "IRR" && ddlPreType.SelectedItem.Text.ToUpper().Trim() == "IRR")
            //{
            //    Utility.FunShowAlertMsg(this, " Amortization schedule method and Premature closure type are not same(Only for IRR). Unable to proceed Premature Closure");
            //    hdnIRR.Value = "2";
            //    return;
            //}
            ClsRepaymentStructure objClsRepaymentStructure = null;
            CommonS3GBusLogic objCommonS3GBusLogic = null;
            decimal decPenalty = 0;
            decimal decPreRate = Convert.ToDecimal(txtPreRate.Text);
            DateTime dtPMCReqDate = Utility.StringToDate(txtPMCReqDate.Text);
            txtPreRate.ReadOnly = false;

            try
            {
                if (ddlPreType.Items.Count > 0)
                {
                    switch (Convert.ToInt32(ddlPreType.SelectedValue))
                    {
                        case 28: // IRR Method
                            {
                                int intCashflowId = 0;
                                bool blnAccountingIRR = false, blnBusinessIRR = false, blnCompanyIRR = false;
                                string strCashflowdesc = "", strTenureType = string.Empty;
                                decimal decPrincipleAmount = 0;
                                decimal? decResidualValue = null, decResidualAmount = null;
                                DateTime dtAppdate = DateTime.Now;
                                string strRecoveryYear1 = "", strRecoveryYear2 = "", strRecoveryYear3 = "", strRecoveryYear4 = "";
                                string strTime_Value = "", strMargin_Percentage = "", strRepayment_Mode = "";
                                string strIRR_Rest = "";
                                double decAccountingIRR = 0, decBusinessIRR = 0, decCompanyIRR = 0;

                                objClsRepaymentStructure = new ClsRepaymentStructure();
                                objCommonS3GBusLogic = new CommonS3GBusLogic();
                                RepaymentType rePayType = new RepaymentType();

                                Dictionary<string, string> objParameters = new Dictionary<string, string>();
                                objParameters.Add("@PANum", ddlMLA.SelectedValue);
                                //if (ddlSLA.SelectedValue != "0") objParameters.Add("@SANum", ddlSLA.SelectedValue);
                                objParameters.Add("@CompanyId", intCompanyID.ToString());
                                DataSet dsRepayStrucure = Utility.GetDataset("S3G_LOANAD_PreClosureType_GetRepayStructure", objParameters);
                                if (dsRepayStrucure.Tables[2] != null && dsRepayStrucure.Tables[2].Rows.Count > 0)
                                {
                                    if (dsRepayStrucure.Tables[2].Rows[0]["Residual_Value"].ToString() != "")
                                        decResidualValue = Convert.ToDecimal(dsRepayStrucure.Tables[2].Rows[0]["Residual_Value"].ToString());
                                    else
                                        decResidualValue = null;
                                    if (dsRepayStrucure.Tables[2].Rows[0]["Residual_Amount"].ToString() != "")
                                        decResidualAmount = Convert.ToDecimal(dsRepayStrucure.Tables[2].Rows[0]["Residual_Amount"].ToString());
                                    else
                                        decResidualAmount = null;
                                    strRepayment_Mode = dsRepayStrucure.Tables[2].Rows[0]["Repayment_Mode"].ToString();
                                    strMargin_Percentage = dsRepayStrucure.Tables[2].Rows[0]["Margin_Percentage"].ToString();
                                    strTime_Value = dsRepayStrucure.Tables[2].Rows[0]["Time_Value"].ToString();
                                    strRecoveryYear1 = dsRepayStrucure.Tables[2].Rows[0]["Recovery_Pattern_Year1"].ToString();
                                    strRecoveryYear2 = dsRepayStrucure.Tables[2].Rows[0]["Recovery_Pattern_Year2"].ToString();
                                    strRecoveryYear3 = dsRepayStrucure.Tables[2].Rows[0]["Recovery_Pattern_Year3"].ToString();
                                    strRecoveryYear4 = dsRepayStrucure.Tables[2].Rows[0]["Recovery_Pattern_Rest"].ToString();
                                    strIRR_Rest = dsRepayStrucure.Tables[2].Rows[0]["IRR_Rest"].ToString();
                                }
                                if (dsRepayStrucure.Tables[4] != null && dsRepayStrucure.Tables[4].Rows.Count > 0)
                                {
                                    if (dsRepayStrucure.Tables[4].Rows[0]["CashFlowID"].ToString() != "")
                                        intCashflowId = Convert.ToInt32(dsRepayStrucure.Tables[4].Rows[0]["CashFlowID"].ToString());
                                    if (dsRepayStrucure.Tables[4].Rows[0]["CashFlow"].ToString() != "")
                                        strCashflowdesc = Convert.ToString(dsRepayStrucure.Tables[4].Rows[0]["CashFlow"].ToString());
                                    if (dsRepayStrucure.Tables[4].Rows[0]["Accounting_IRR"].ToString() != "")
                                        blnAccountingIRR = Convert.ToBoolean(dsRepayStrucure.Tables[4].Rows[0]["Accounting_IRR"].ToString());
                                    if (dsRepayStrucure.Tables[4].Rows[0]["Business_IRR"].ToString() != "")
                                        blnBusinessIRR = Convert.ToBoolean(dsRepayStrucure.Tables[4].Rows[0]["Business_IRR"].ToString());
                                    if (dsRepayStrucure.Tables[4].Rows[0]["Company_IRR"].ToString() != "")
                                        blnCompanyIRR = Convert.ToBoolean(dsRepayStrucure.Tables[4].Rows[0]["Company_IRR"].ToString());
                                }
                                //   string[] arrTenure = txtTenure.Text.Split(' ');
                                int intTenure = 0;
                                //if (arrTenure.Length > 1)
                                //{
                                //    strTenureType = arrTenure[1];
                                //    intTenure = Convert.ToInt32(arrTenure[0]);
                                //}

                                //if (txtPrincipal.Text.Trim() != "") decPrincipleAmount = Convert.ToDecimal(txtPrincipal.Text);
                                //if (txtAccDate.Text.Trim() != "") dtAppdate = Utility.StringToDate(txtAccDate.Text);

                                switch (Repayment_Mode_Code.ToString())
                                {
                                    case "1":
                                        if (Return_Pattern.ToString() != "3" || Return_Pattern.ToString() != "4" || Return_Pattern.ToString() != "5")
                                            rePayType = RepaymentType.EMI;
                                        else
                                            rePayType = RepaymentType.Others;
                                        break;
                                    default:
                                        rePayType = RepaymentType.Others;
                                        break;
                                }

                                switch (Return_Pattern.ToString())
                                {
                                    case "3":
                                        rePayType = RepaymentType.PMPT;
                                        break;
                                    case "4":
                                        rePayType = RepaymentType.PMPL;
                                        break;
                                    case "5":
                                        rePayType = RepaymentType.PMPM;
                                        break;
                                }
                                switch (ddlLOB.SelectedValue.Split('-')[0])
                                {
                                    case "tl":
                                    case "te":
                                        if (Return_Pattern.ToString() == "5")
                                            rePayType = RepaymentType.TLE;
                                        break;
                                    case "ft":
                                        rePayType = RepaymentType.FC;
                                        break;
                                    case "wc":
                                        rePayType = RepaymentType.WC;
                                        break;
                                }

                                Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
                                objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text);
                                objMethodParameters.Add("Tenure", intTenure.ToString());
                                objMethodParameters.Add("TenureType", strTenureType);
                                //objMethodParameters.Add("FinanceAmount", txtPrincipal.Text);
                                objMethodParameters.Add("ReturnPattern", "2");
                                objMethodParameters.Add("MarginPercentage", strMargin_Percentage);
                                objMethodParameters.Add("Rate", decPreRate.ToString());
                                objMethodParameters.Add("TimeValue", strTime_Value);
                                objMethodParameters.Add("RepaymentMode", strRepayment_Mode);
                                objMethodParameters.Add("CompanyId", intCompanyID.ToString());
                                objMethodParameters.Add("LobId", ddlLOB.SelectedValue);
                                objMethodParameters.Add("DocumentDate", First_InstallmentDate.ToString());
                                objMethodParameters.Add("Frequency", Frequency);
                                objMethodParameters.Add("RecoveryYear1", strRecoveryYear1);
                                objMethodParameters.Add("RecoveryYear2", strRecoveryYear2);
                                objMethodParameters.Add("RecoveryYear3", strRecoveryYear3);
                                objMethodParameters.Add("RecoveryYear4", strRecoveryYear4);
                                objMethodParameters.Add("Roundoff", RoundOff.ToString());
                                if (decResidualValue.ToString() != "" && decResidualValue.ToString() != "0")
                                    objMethodParameters.Add("decResidualValue", decResidualValue.ToString());

                                if (decResidualAmount.ToString() != "" && decResidualAmount.ToString() != "0")
                                    objMethodParameters.Add("decResidualAmount", decResidualAmount.ToString());

                                objMethodParameters.Add("strIRRrest", strIRR_Rest);
                                objMethodParameters.Add("decLimit", "0.10");

                                //code added and commented as discussed with Thalaiselvam and Thangam. - August - 02 - 2012
                                //"PrincipalMethod" - parameter added to access FunPubGenerateRepaymentSchedule() method
                                //if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))) && strRepayment_Mode != "5" && ddl_Return_Pattern.SelectedValue == "6")
                                //{
                                //    objMethodParameters.Add("PrincipalMethod", "1");
                                //}
                                //else
                                //{
                                objMethodParameters.Add("PrincipalMethod", "0");
                                //}

                                decimal decRateOut = 0;
                                DataTable dtMor = null;

                                DataSet DsRepayGrid = objClsRepaymentStructure.FunPubGenerateRepaymentSchedule(First_InstallmentDate, dsRepayStrucure.Tables[0], dsRepayStrucure.Tables[1], objMethodParameters, dtMor, out decRateOut);
                                DataTable DtRepayGrid = DsRepayGrid.Tables[0];

                                ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);

                                DataSet dsRepaymentStructureTable = objCommonS3GBusLogic.FunPubGetRepaymentDetails(Frequency, intTenure,
                                    strTenureType, decPrincipleAmount,
                                   Convert.ToDecimal(ViewState["decRate"]), rePayType, decResidualValue,
                                     First_InstallmentDate, dtAppdate, intCashflowId, strCashflowdesc, Convert.ToDecimal(RoundOff),
                                     blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, Time_Value, dtMor);

                                DataTable dtRepaymentStructure = objCommonS3GBusLogic.FunPubGenerateRepaymentStructure
                                    (dsRepaymentStructureTable.Tables[0], dsRepayStrucure.Tables[0], dsRepayStrucure.Tables[1],
                                    Frequency, intTenure, strTenureType, strDateFormat,
                                    Convert.ToDecimal(objClsRepaymentStructure.FunPubGetAmountFinanced(decPrincipleAmount.ToString(),
                                    strMargin_Percentage)), Convert.ToDecimal(ViewState["decRate"].ToString()), strIRR_Rest,
                                    Time_Value, "", Convert.ToDecimal(10.05), decCTR, decPLR, First_InstallmentDate, decResidualValue,
                                    decResidualAmount, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, "",
                                    dtMor, ddlLOB.SelectedItem.Text.Trim().Split('-')[0].ToString().Trim(), "NIL", 0);

                                if (Bill_InstallmentNo > 0)
                                    decPenalty = Convert.ToDecimal(dtRepaymentStructure.Compute("sum(charge)", "InstallmentNo<=" + Bill_InstallmentNo + "")) - TotalPaid_Interest;

                                int intDaysPerYear = 0;
                                if (intGPS_DaysPerYear == 365)
                                    intDaysPerYear = 31;
                                else
                                    intDaysPerYear = 30;

                                decimal newBRPercentage = 0;
                                if (Time_Value.ToUpper() == "ADVANCE")
                                {
                                    TimeSpan diff1 = Next_InstallmentDt.Subtract(dtPMCReqDate);
                                    DataRow[] drRepay = dtRepaymentStructure.Select("InstallmentNo = " + Bill_InstallmentNo + "");
                                    if (drRepay.Length > 0)
                                    {
                                        newBRPercentage = Math.Round((Convert.ToDecimal(drRepay[0]["charge"].ToString()) / intDaysPerYear) * diff1.Days, 2);// RoundOff);
                                        newBRPercentage = newBRPercentage - Math.Round((CurrentMth_FinanceCharge / intDaysPerYear) * diff1.Days, 2);// RoundOff);
                                    }
                                }
                                else
                                {
                                    TimeSpan diff1 = dtPMCReqDate.Subtract(Current_InstallmentDt);
                                    DataRow[] drRepay = dtRepaymentStructure.Select("InstallmentNo > " + Bill_InstallmentNo + "");
                                    if (drRepay.Length > 0)
                                    {
                                        newBRPercentage = Math.Round((Convert.ToDecimal(drRepay[0]["charge"].ToString()) / intDaysPerYear) * diff1.Days, 2);// RoundOff);
                                        newBRPercentage = newBRPercentage - Math.Round((NextMth_FinanceCharge / intDaysPerYear) * diff1.Days, 2);// RoundOff);
                                    }
                                }
                                decPenalty = decPenalty + newBRPercentage;
                                if (ViewState["ClosureStatement"] != null)
                                {
                                    DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                                    DataRow[] drClosureStatement = dtClosureStatement.Select(" ID = '42'  ");
                                    /*if (drClosureStatement.Length > 0)
                                    {
                                        drClosureStatement[0].BeginEdit();
                                        drClosureStatement[0]["Due"] = Math.Round(decPenalty, 2);
                                        drClosureStatement[0]["Closure"] = Math.Round(decPenalty, 2);
                                        drClosureStatement[0].AcceptChanges();
                                    }*/
                                    if (drClosureStatement.Length == 0)
                                    {
                                        DataRow drPenalty = dtClosureStatement.NewRow();
                                        drPenalty["ID"] = 42;
                                        drPenalty["Description"] = "Penalty";
                                        drPenalty["Due"] = Math.Round(decPenalty, 2);
                                        drPenalty["Closure"] = Math.Round(decPenalty, 2);
                                        drPenalty["Waived"] = decimal.Parse("0.00");
                                        drPenalty["Received"] = decimal.Parse("0.00");
                                        drPenalty["Payable"] = decimal.Parse("0.00");
                                        dtClosureStatement.Rows.Add(drPenalty);
                                    }
                                    else
                                    {
                                        drClosureStatement[0].BeginEdit();
                                        drClosureStatement[0]["Due"] = Math.Round(decPenalty, 2);
                                        drClosureStatement[0]["Closure"] = Math.Round(decPenalty, 2);
                                        drClosureStatement[0]["Waived"] = decimal.Parse("0.00");
                                        drClosureStatement[0]["Received"] = decimal.Parse("0.00");
                                        drClosureStatement[0]["Payable"] = decimal.Parse("0.00");
                                        drClosureStatement[0].AcceptChanges();
                                    }
                                    ViewState["ClosureStatement"] = dtClosureStatement;
                                }
                                FunPriBindCashFlowGrid();
                                break;
                            }
                        case 29: // NPV
                            {
                                DataTable dtRepayStructure = null;
                                decimal decNpv = 0;
                                if (ViewState["RepayStructure"] != null && (ViewState["RepayStructure"] as DataTable).Select("Installment_No > " + Bill_InstallmentNo + "").Length > 0) dtRepayStructure = (ViewState["RepayStructure"] as DataTable).Select("Installment_No > " + Bill_InstallmentNo + "").CopyToDataTable();
                                DataColumn dcNPVAmount = new DataColumn("NPVAmount", System.Type.GetType("System.Decimal"));
                                if (dtRepayStructure != null)
                                {
                                    dtRepayStructure.Columns.Add(dcNPVAmount);

                                    foreach (DataRow dr in dtRepayStructure.Rows)
                                    {
                                        decimal decInstallAmt = Convert.ToDecimal(dr["InstallmentAmount"].ToString());
                                        DateTime dtInstallmentDate = Utility.StringToDate(dr["InstallmentDate"].ToString());
                                        TimeSpan tsNoOfDays = dtInstallmentDate.Subtract(dtPMCReqDate);
                                        dr.BeginEdit();
                                        decNpv = (decInstallAmt - ((decInstallAmt - (decInstallAmt / (1 + (decPreRate / 100)))) *
                                            Convert.ToDecimal(tsNoOfDays.Days / 365.00)));
                                        decNpv = Math.Round(Convert.ToDecimal(decNpv), 2);
                                        dr["NPVAmount"] = decNpv;
                                        dr.AcceptChanges();
                                    }
                                }
                                if ((ViewState["RepayStructure"] as DataTable).Select("Installment_No > " + Bill_InstallmentNo + "").Length > 0)
                                {
                                    decPenalty = Convert.ToDecimal(dtRepayStructure.Compute("sum(NPVAmount)", "PANUM<>''").ToString()) -
                                        Convert.ToDecimal(dtRepayStructure.Compute("sum(PrincipalAmount)", "PANUM<>''").ToString());
                                }

                                if (ViewState["ClosureStatement"] != null)
                                {
                                    DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                                    DataRow[] drPenaltyRow = dtClosureStatement.Select(" ID = '42'  ");
                                    if (drPenaltyRow.Length == 0)
                                    {
                                        DataRow drPenalty = dtClosureStatement.NewRow();
                                        drPenalty["ID"] = 42;
                                        drPenalty["Description"] = "Penalty";
                                        drPenalty["Due"] = decPenalty;
                                        drPenalty["Closure"] = decPenalty;
                                        drPenalty["Waived"] = decimal.Parse("0.00");
                                        drPenalty["Received"] = decimal.Parse("0.00");
                                        drPenalty["Payable"] = decimal.Parse("0.00");
                                        dtClosureStatement.Rows.Add(drPenalty);
                                    }
                                    else
                                    {
                                        drPenaltyRow[0].BeginEdit();
                                        drPenaltyRow[0]["Due"] = decPenalty;
                                        drPenaltyRow[0]["Closure"] = decPenalty;
                                        drPenaltyRow[0]["Waived"] = decimal.Parse("0.00");
                                        drPenaltyRow[0]["Received"] = decimal.Parse("0.00");
                                        drPenaltyRow[0]["Payable"] = decimal.Parse("0.00");
                                        drPenaltyRow[0].AcceptChanges();
                                    }
                                    ViewState["ClosureStatement"] = dtClosureStatement;
                                }
                                FunPriBindCashFlowGrid();
                                break;
                            }
                        case 30: // Principle 
                            {
                                txtPreRate.Text = Principle_Rate.ToString();
                                txtPreRate.ReadOnly = true;
                                if (ViewState["ClosureStatement"] != null)
                                {
                                    DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                                    DataRow[] drPenaltyRow = dtClosureStatement.Select(" ID = '42'  ");

                                    //if (txtMatureDate.Text != string.Empty)
                                    //{
                                    //    TimeSpan diff1 = Utility.StringToDate(txtMatureDate.Text).Subtract(dtPMCReqDate);
                                    //}
                                    //decPenalty = //(Convert.ToDecimal(FuturePrincipalAmt) * Principle_Rate)/100;
                                    //Math.Round((((Convert.ToDecimal(FuturePrincipalAmt)  * Principle_Rate)/100) / decRoundOff), 0) * decRoundOff;

                                    // by bhuvana for penalty change for Lob "ol" on jun 7th
                                    //decPenalty = Math.Round((Convert.ToDecimal(FuturePrincipalAmt) * Principle_Rate) / 100, 2);


                                    //if (ddlLOB.SelectedItem.Text.StartsWith("OL"))
                                    //{
                                    //    decPenalty = Math.Round((Convert.ToDecimal(FuturePrincipalAmt) * Principle_Rate), 2);
                                    //}
                                    //else
                                    //{
                                    //    decPenalty = Math.Round((Convert.ToDecimal(FuturePrincipalAmt) * Principle_Rate) / 100, 2);
                                    //}
                                    ////Changed By Palani Kumar.A on 21/01/2014 for Product Features
                                    //string strddlMLA = string.Empty;
                                    //if (ddlMLA.SelectedValue.ToString() != "0")
                                    //    strddlMLA = ddlMLA.SelectedText.Substring(0, ddlMLA.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();
                                    //else
                                    //    strddlMLA = ddlMLA.SelectedText.ToString();
                                    ////END

                                    //    if (ddlLOB.SelectedItem.Text.StartsWith("OL"))
                                    //    {
                                    //        Procparam = new Dictionary<string, string>();
                                    //        Procparam.Add("@PANUM", ddlMLA.SelectedValue.ToString());
                                    //        //if (ddlSLA.SelectedValue != "0")
                                    //        //{
                                    //        //    Procparam.Add("@SANUM", ddlSLA.SelectedItem.Text);
                                    //        //}
                                    //        Procparam.Add("@Ins_Date", Utility.StringToDate(txtPMCReqDate.Text).ToString());
                                    //        DataTable dtFutureInst = Utility.GetDefaultData("S3G_LOANAD_GetFutureInstallment", Procparam);

                                    //        decimal decFutureIns = 0;

                                    //        if (dtFutureInst != null && dtFutureInst.Rows.Count > 0)
                                    //        {
                                    //            decFutureIns = Convert.ToDecimal(dtFutureInst.Rows[0][0].ToString());
                                    //        }

                                    //        decPenalty = Math.Round((Convert.ToDecimal(decFutureIns) * Principle_Rate), 2);
                                    //    }
                                    //    else
                                    //    {
                                    //        //Changed by Chandru k For ISFC Changes
                                    //        decPenalty = Math.Round((Convert.ToDecimal(FuturePrincipalAmt) * Principle_Rate) / 100, 2);

                                    //        //if (ViewState["Penalty"] != null)
                                    //        //    decPenalty = Convert.ToDecimal(ViewState["Penalty"].ToString());
                                    //    }

                                    //    if (drPenaltyRow.Length == 0)
                                    //    {
                                    //        DataRow drPenalty = dtClosureStatement.NewRow();
                                    //        drPenalty["ID"] = 42;
                                    //        drPenalty["Description"] = "Penalty";
                                    //        drPenalty["Due"] = decPenalty;
                                    //        drPenalty["Closure"] = decPenalty;
                                    //        drPenalty["Waived"] = decimal.Parse("0.00");
                                    //        drPenalty["Received"] = decimal.Parse("0.00");
                                    //        drPenalty["Payable"] = decimal.Parse("0.00");
                                    //        dtClosureStatement.Rows.Add(drPenalty);
                                    //    }
                                    //    else
                                    //    {

                                    //        drPenaltyRow[0].BeginEdit();
                                    //        drPenaltyRow[0]["Due"] = decPenalty;// Math.Round(decPenalty, 2);
                                    //        drPenaltyRow[0]["Closure"] = decPenalty;// Math.Round(decPenalty, 2);
                                    //        drPenaltyRow[0]["Waived"] = decimal.Parse("0.00");
                                    //        drPenaltyRow[0]["Received"] = decimal.Parse("0.00");
                                    //        drPenaltyRow[0]["Payable"] = decimal.Parse("0.00");
                                    //        drPenaltyRow[0].AcceptChanges();
                                    //    }
                                    //    ViewState["ClosureStatement"] = dtClosureStatement;
                                }
                                //FunPriBindCashFlowGrid();
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                //hdnIRR.Value = "1";
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
                throw;
            }
            finally
            {
                objCommonS3GBusLogic = null;
                objClsRepaymentStructure = null;
            }
        }
    }

    private void FunPriBindCashFlowGrid()
    {
        try
        {
            if (ViewState["ClosureStatement"] != null)
            {
                DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                grvCashFlow.DataSource = dtClosureStatement;
                grvCashFlow.DataBind();

                dcDue = Convert.ToDecimal(dtClosureStatement.Compute("sum(due1)", "Description<>''"));
                dcPayable = Convert.ToDecimal(dtClosureStatement.Compute("sum(pv1)", "Description<>'' "));
                dcClosure = Convert.ToDecimal(dtClosureStatement.Compute("sum(Closure1)", "Description<>'' "));
                dcWaived = Convert.ToDecimal(dtClosureStatement.Compute("sum(Waived1)", "Description<>''  "));
                //dcReceived = Convert.ToDecimal(dtClosureStatement.Compute("sum(Received1)", "Description<>''  "));
                FunPriSetFooterValue();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriBindAccountBalanceGrid(DataTable dtAccountBalanc)
    {
        try
        {
            dcFooterDue = Convert.ToDecimal(dtAccountBalanc.Compute("sum(outstanding1)", "Desc<>''"));
            dcFooterReceived = Convert.ToDecimal(dtAccountBalanc.Compute("sum(OPC_fut_rec1)", "Desc<>'' "));
            dcFooterOS = Convert.ToDecimal(dtAccountBalanc.Compute("sum(fund_fut_rec1)", "Desc<>'' "));
            FunPriSetABFooterValue();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    protected void btnPrintRental_Click(object sender, EventArgs e)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@closure_ID", Convert.ToString(intClosureDetailId));
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@USER_ID", Convert.ToString(intUserID));
            dtRental = Utility.GetDefaultData("S3G_FUNDMGT_GETPrintPMC", Procparam);
            if (dtRental.Rows.Count > 0)
            {
                Guid objGuid;
                objGuid = Guid.NewGuid();
                //ReportDocument rpd = new ReportDocument();

                //rpd.Load(Server.MapPath("PMC_rental.rpt"));

                //rpd.SetDataSource(dtRental);
                //rpd.Subreports["BillReg_Inv.rpt"].SetDataSource(dsInterim.Tables[0]);
                //rpd.Subreports["BillRegInv_Sub"].SetDataSource(dsInterim.Tables[1]);

                string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + objGuid.ToString() + ".pdf";

                string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

                if (!(System.IO.Directory.Exists(strFolder)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strFolder);

                }

                string strScipt = "";
                //rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                strScipt = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "No Records Found");
                return;
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }

    }

    protected void btnPrintAMF_Click(object sender, EventArgs e)
    {
        //if (Procparam != null)
        //    Procparam.Clear();
        //else
        //    Procparam = new Dictionary<string, string>();

        //Procparam.Add("@closure_ID", Convert.ToString(intClosureDetailId));
        //Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
        //Procparam.Add("@USER_ID", Convert.ToString(intUserID));
        //Procparam.Add("@Option", "1");
        //dsrental = Utility.GetDataset("S3G_FUNDMGT_GETPrintPMC", Procparam);
        //if (dsrental.Tables[0].Rows.Count > 0)
        //{
        //    Guid objGuid;
        //    objGuid = Guid.NewGuid();
        //    ReportDocument rpd = new ReportDocument();

        //    rpd.Load(Server.MapPath("PMC_AMF.rpt"));


        //    rpd.SetDataSource(dsrental.Tables[0]);
        //    rpd.Subreports["Subreport"].SetDataSource(dsrental.Tables[1]);


        //    string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + objGuid.ToString() + ".pdf";

        //    string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

        //    if (!(System.IO.Directory.Exists(strFolder)))
        //    {
        //        DirectoryInfo di = Directory.CreateDirectory(strFolder);

        //    }

        //    string strScipt = "";
        //    rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

        //    strScipt = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

        //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        //}
        //else
        //{
        //    Utility.FunShowAlertMsg(this.Page, "No Records Found");
        //    return;
        //}

        FunPrintCoveringLetter();
    }

    private void FunPrintCoveringLetter()
    {
        try
        {
            string strHTML = string.Empty;

            strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyID, Convert.ToInt32(ddlLOB.SelectedValue)
                , 166, 22, intClosureDetailId.ToString());

            if (strHTML == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined");
                return;
            }

            string FileName = PDFPageSetup.FunPubGetFileName(intClosureDetailId + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss"));


            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string DownFile = FilePath + FileName + ".pdf";
            SaveDocument(strHTML, intClosureDetailId.ToString(), FilePath, FileName);


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void SaveDocument(string strHTML, string ReferenceNumber, string FilePath, string FileName)
    {
        try
        {
            List<string> filepaths = new List<string>();

            DataSet dsPrintDetails = new DataSet();

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@closure_ID", ReferenceNumber);
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@USER_ID", Convert.ToString(intUserID));

            dsPrintDetails = Utility.GetDataset("S3G_LAD_GETPrintPMC", Procparam);

            if (dsPrintDetails.Tables[0].Rows.Count > 0)
            {
                DataTable DT1 = dsPrintDetails.Tables[0].Clone();
                DataTable DT2 = dsPrintDetails.Tables[1].Clone();
                DataTable DT3 = dsPrintDetails.Tables[2].Clone();

                DataView DV1 = dsPrintDetails.Tables[0].AsDataView();
                DataView DV2 = dsPrintDetails.Tables[1].AsDataView();
                DataView DV3 = dsPrintDetails.Tables[2].AsDataView();

                string strHTMLtemp = string.Empty;

                for (int i = 0; i < dsPrintDetails.Tables[0].Rows.Count; i++)
                {
                    strHTMLtemp = strHTML;

                    DV1.RowFilter = "ACCOUNT_NO = '" + dsPrintDetails.Tables[0].Rows[i]["ACCOUNT_NO"].ToString() + "'";
                    DV2.RowFilter = "PANum = '" + dsPrintDetails.Tables[0].Rows[i]["ACCOUNT_NO"].ToString() + "'";
                    DV3.RowFilter = "PANum = '" + dsPrintDetails.Tables[0].Rows[i]["ACCOUNT_NO"].ToString() + "'";


                    DT1 = DV1.ToTable();
                    DT2 = DV2.ToTable();
                    DT3 = DV3.ToTable();

                    DataRow[] ObjIGSTDR = DT2.Select("InvTbl_IGST_Amount_Dbl > 0");

                    if (ObjIGSTDR.Length > 0)
                    {
                        if (strHTMLtemp.Contains("~InvoiceTable~"))
                            strHTMLtemp = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTMLtemp);

                        if (strHTMLtemp.Contains("~InvoiceTable1~"))
                        {
                            strHTMLtemp = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTMLtemp, DT2/*Invoice Breakup*/);
                        }
                    }
                    else
                    {
                        if (strHTMLtemp.Contains("~InvoiceTable1~"))
                            strHTMLtemp = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTMLtemp);

                        if (strHTMLtemp.Contains("~InvoiceTable~"))
                        {
                            strHTMLtemp = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTMLtemp, DT2/*Invoice Breakup*/);
                        }
                    }

                    if (strHTMLtemp.Contains("~SACTable~"))
                    {
                        strHTMLtemp = PDFPageSetup.FunPubBindTable("~SACTable~", strHTMLtemp, DT3/*SAC*/);
                    }

                    strHTMLtemp = PDFPageSetup.FunPubBindCommonVariables(strHTMLtemp, DT1/*HDR*/);

                    strHTMLtemp = PDFPageSetup.FunPubBindCommonVariables(strHTMLtemp, DT1/*HDR*/);

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    List<string> listImagePath = new List<string>();
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + intCompanyID.ToString() + "/CompanyLogo.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + intCompanyID.ToString() + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + intCompanyID.ToString() + "/POSignStamp.png"));

                    strHTMLtemp = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTMLtemp);

                    string text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069. ";

                    FunPubSaveDocument(strHTMLtemp, FilePath, FileName + i.ToString(), "P", text);
                    filepaths.Add(FilePath + FileName + i.ToString() + ".pdf");
                }

                string OutputFile = FilePath + "PreMatureClosure" + intUserID.ToString() + ".pdf";
                FunPriGenerateFiles(filepaths, OutputFile, "P");
                Response.AppendHeader("content-disposition", "attachment; filename=PreMatureClosure.pdf");
                Response.ContentType = "application/pdf";
                Response.WriteFile(OutputFile);
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "No Records Found");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriDownloadFile(string OutputFile)
    {
        Response.AppendHeader("content-disposition", "attachment; filename=PreMatureClosure.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(OutputFile);
    }

    private void FunPriGenerateFiles(List<string> filepaths, string OutputFile, string DocumentType)
    {
        //try
        //{
            object fileFormat = null;
            object file = null;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string[] filesToMerge = filepaths.ToArray();

            if (DocumentType == "P")
            {
                if (File.Exists(OutputFile) == true)
                {
                    File.Delete(OutputFile);
                }

                PDFPageSetup.MergePDFs(filesToMerge, OutputFile);

                for (int i = 0; i < filesToMerge.Length; i++)
                {
                    if (File.Exists(filesToMerge[i]) == true)
                    {
                        File.Delete(filesToMerge[i]);
                    }
                }
            }
            
        //}
        //catch (System.IO.IOException ex)
        //{
        //    Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print");
        //}
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, string FooterNote)
    {
        string strhtmlFile = FilePath + "\\" + FileName + ".html";
        string strwordFile = FilePath + "\\" + FileName + ".doc";
        string strpdfFile = FilePath + "\\" + FileName + ".pdf";
        object file = strhtmlFile;
        object oMissing = System.Reflection.Missing.Value;
        object readOnly = false;
        object oFalse = false;
        object fileFormat = null;

        Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
        Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            if (File.Exists(strwordFile) == true)
            {
                File.Delete(strwordFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            oDoc.ActiveWindow.Selection.Font.Size = 7;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.TypeText(textDisc);

            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);

            if (FooterNote != "")
            {
                oDoc.ActiveWindow.Selection.Font.Size = 9;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(FooterNote);
            }
            else
            {
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
            }

            string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            if (DocumentType == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
            }

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);
        }
    }


    #endregion

    #region [Email / Print Functionality]

    private string GetHTMLText()
    {
        DataTable dtCompany = ViewState["Company"] as DataTable;
        string strCompany = string.Empty;
        string strAddress = string.Empty;
        string strAccNo = string.Empty;
        //Changed By Palani Kumar.A on 21/01/2014 for Product Features
        //if (ddlSLA.SelectedValue != "0")
        //{
        //    strAccNo =
        //          "  for Prime Account No  " + ddlMLA.SelectedValue.Trim() +
        //        " and Sub Account No  " + ddlMLA.SelectedItem.Text.Trim();
        //}
        //else
        //{
        strAccNo =
          "  for Prime Account No  " + ddlMLA.SelectedValue.Trim();

        //}
        if (dtCompany != null && dtCompany.Rows.Count > 0)
        {
            strCompany = dtCompany.Rows[0]["Company_Name"].ToString();
            strAddress = dtCompany.Rows[0]["Address1"].ToString();
            if (dtCompany.Rows[0]["City"].ToString() != "") strAddress += ", " + dtCompany.Rows[0]["City"].ToString();
            if (dtCompany.Rows[0]["City"].ToString() != "") strAddress += "-" + dtCompany.Rows[0]["Zip_Code"].ToString();
        }

        return
            "<font size=\"1\"  color=\"black\" face=\"Times New Roman\">" +

           " <table width=\"100%\">" +
        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">" + strCompany + "</font> " +
            "</td>" +
       " </tr>" +

        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">" + strAddress + "</font> " +
            "</td>" +
       " </tr>" +

          "<tr >" +
            "<td  align=\"Center\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "<b>" + " Account Closure Statement" + "</font> " + "</b>" +
            "</td>" +
       " </tr>" +
        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "<b>" + " Dear Sir / Madam," + "</font> " + "</b>" +
            "</td>" +
       " </tr>" +

      "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " Account has been closed sucessfully " + strAccNo + ".</font> " +
            "</td>" +
       " </tr>" +

         "<tr >" +
            "<td  align=\"left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Premature Closure No: " + hidAccClosureNo.Value + "</font> " + "</b></u>" +
            "</td>" +
       " </tr>" +

         "<tr >" +
            "<td  align=\"left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Premature Closure Date: " + txtPreclosureDate.Text.Trim() + "</font> " + "</b></u>" +
            "</td>" +
       " </tr>" +

        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "<b><u>" + " Account Details" + "</font> " + "</b></u>" +
            "</td>" +
       " </tr>" +

         "<tr >" +
            "<td  align=\"Left\" >"
        + "<table width=\"100%\" border=\"1\">" +
            "<tr >" +
              "<th  align=\"Center\" >" + "Cash flow description" + "</th>" +
              "<th  align=\"Center\" >" + "Due Amount" + "</th>" +
              "<th  align=\"Center\" >" + "Waived Amount" + "</th>" +
              "<th  align=\"Center\" >" + "Payable Amount" + "</th>" +
              "<th align=\"Center\" >" + "Received Amount" + "</th>" +
              "<th align=\"Center\" >" + "Account Closure Amount" + "</th>" +

           "</tr>" + FunPriGetHtmlTable() +
         "</table>" +
         "</td>" +
        " </tr>" +

       "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Yours Truly," + "</font> " +
            "</td>" +
       " </tr>" +
        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">" + strCompany + "</font> " +
            "</td>" +
       " </tr>" +
    "</table>" + "</font>";

    }

    private StringBuilder GetHTMLTextEmail()
    {
        DataTable dtCompany = ViewState["Company"] as DataTable;
        string strCompany = string.Empty;
        string strAddress = string.Empty;
        string strAccNo = string.Empty;

        //if (ddlSLA.SelectedValue != "0")
        //{
        //    strAccNo =
        //          "  for Prime Account No  " + ddlMLA.SelectedValue.Trim() +
        //        " and Sub Account No  " + ddlSLA.SelectedItem.Text.Trim();
        //}
        //else
        //{
        strAccNo =
          "  for Prime Account No  " + ddlMLA.SelectedValue.Trim();

        //}
        if (dtCompany != null && dtCompany.Rows.Count > 0)
        {
            strCompany = dtCompany.Rows[0]["Company_Name"].ToString();
            strAddress = dtCompany.Rows[0]["Address1"].ToString();
            if (dtCompany.Rows[0]["City"].ToString() != "") strAddress += ", " + dtCompany.Rows[0]["City"].ToString();
            if (dtCompany.Rows[0]["City"].ToString() != "") strAddress += "-" + dtCompany.Rows[0]["Zip_Code"].ToString();
        }

        StringBuilder strMailBodey = new StringBuilder();

        strMailBodey.Append(

            "<font size=\"1\"  color=\"black\" face=\"Times New Roman\">" +

           " <table width=\"100%\">" +

  "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Dear Customer," + "</font> " +
            "</td>" +
       " </tr>" +


      "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " With respect of you Premature Closure requested on " + txtPreclosureDate.Text.Trim() + " " + strAccNo + ", Please find the Premature Closure statement in the attached file. </font> " +
            "</td>" +
       " </tr>" +


       "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Yours Truly," + "</font> " +
            "</td>" +
       " </tr>" +
        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">" + strCompany + "</font> " +
            "</td>" +
       " </tr>" +
    "</table>" + "</font>");

        return strMailBodey;

    }

    private string FunPriGetHtmlTable()
    {
        string strHtml = string.Empty;
        foreach (GridViewRow gvRow in grvCashFlow.Rows)
        {
            Label lblCash = gvRow.FindControl("lblCash") as Label;
            Label lblDue = gvRow.FindControl("lblDue") as Label;
            TextBox txtWaived = gvRow.FindControl("txtWaived") as TextBox;
            Label lblPayable = gvRow.FindControl("lblPayable") as Label;
            Label lblClosure = gvRow.FindControl("lblClosure") as Label;
            Label lblReceived = gvRow.FindControl("lblReceived") as Label;

            strHtml += "<tr>";
            strHtml += "<td>" + lblCash.Text.Trim() + "</td>";
            strHtml += "<td align='right'>" + lblDue.Text.Trim() + "</td>";
            strHtml += "<td align='right'>" + txtWaived.Text.Trim() + "</td>";
            strHtml += "<td align='right' >" + lblPayable.Text.Trim() + "</td>";
            strHtml += "<td align='right' >" + lblReceived.Text.Trim() + "</td>";
            strHtml += "<td align='right'>" + lblClosure.Text.Trim() + "</td>";
            strHtml += "</tr>";
        }

        strHtml += "<tr>";
        for (int i_column = 0; i_column < grvCashFlow.Columns.Count; i_column++)
        {
            if (i_column == 0 || i_column == 6)
                strHtml += "<td>" + grvCashFlow.FooterRow.Cells[i_column].Text.Trim() + "</td>";
            else
                strHtml += "<td align='right' >" + grvCashFlow.FooterRow.Cells[i_column].Text.Trim() + "</td>";
        }
        strHtml += "</tr>";
        //strHtml += " ";
        //dt.Clear();
        //dt.Dispose();
        return strHtml;
    }

    #endregion [Email / Print Functionality]

    private void FunPriClearControls()
    {
        hdnCustomerID.Value = "";
        //TxtAccStatus.Text = txtAccDate.Text = txtPrincipal.Text = txtMatureDate.Text = txtIRR.Text = "";
        //txtBusinessIRR.Text = txtCompanyIRR.Text = txtFlatRate.Text = txtTenure.Text = txtMode.Text = txtFinanceCharge.Text = "";
        //grvAsset.ClearGrid();
        //grvAccountBalance.ClearGrid();
        btnEmail.Enabled = btnPrint.Enabled = false;
        //tbAccDetails.Enabled = 
        tbCashFlow.Enabled = false;
        ucdCustomer.ClearCustomerDetails();
        txtPreRate.Text = txtPreAmount.Text = "";
        if (ddlPreType.Items.FindByValue("0") != null) ddlPreType.SelectedIndex = 0;
        grvCashFlow.DataSource = null;
        grvCashFlow.DataBind();
        Panel2.Visible = false;
        Panel5.Visible = false;
        //txtPMCReqDate.Text = DateTime.Today.ToString(strDateFormat);
        txtbreaking.Text = "0.00";
        txtToTal.Text = "0.00";
        div2.Style.Add("display", "none");
    }

    private void FunPriClearControls1()
    {
        hdnCustomerID.Value = "";
        //TxtAccStatus.Text = txtAccDate.Text = txtPrincipal.Text = txtMatureDate.Text = txtIRR.Text = "";
        //txtBusinessIRR.Text = txtCompanyIRR.Text = txtFlatRate.Text = txtTenure.Text = txtMode.Text = txtFinanceCharge.Text = "";
        //grvAsset.ClearGrid();
        //grvAccountBalance.ClearGrid();
        btnEmail.Enabled = btnPrint.Enabled = false;
        //tbAccDetails.Enabled = 
        tbCashFlow.Enabled = false;
        ucdCustomer.ClearCustomerDetails();
        txtPreRate.Text = txtPreAmount.Text = "";
        //ddlMLA.SelectedValue = "";
        //ddlMLA.SelectedText = "--Select--";
        if (ddlPreType.Items.FindByValue("0") != null) ddlPreType.SelectedIndex = 0;
        grvCashFlow.DataSource = null;
        grvCashFlow.DataBind();
        Panel2.Visible = false;
        Panel5.Visible = false;
        //txtPMCReqDate.Text = DateTime.Today.ToString(strDateFormat);
        txtbreaking.Text = "0.00";
        txtToTal.Text = "0.00";
        div2.Style.Add("display", "none");
    }

    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    #region [FunPriGenerateAccountClosureDetailsXMLDet]

    /// <summary>
    /// Created by Tamilselvan.S
    /// Ceated Date 28/01/2011
    /// For Grid values to XML conversion
    /// </summary>
    /// <returns></returns>
    private string FunPriGenerateAccountClosureDetailsXMLDet()
    {
        try
        {
            strbAccountClosureDetails.Append("<Root>");
            foreach (GridViewRow gvRow in grvCashFlow.Rows)
            {
                TextBox txtRemarks = gvRow.FindControl("txtRemarks") as TextBox;
                TextBox txtWaived = gvRow.FindControl("txtWaived") as TextBox;
                Label lblClosure = gvRow.FindControl("lblClosure") as Label;
                Label lblpasaid = gvRow.FindControl("lblpasaid") as Label;
                Label lblpanum = gvRow.FindControl("lblpanum") as Label;
                Label lblDue = gvRow.FindControl("lblDue") as Label;
                // Label lblReceived = gvRow.FindControl("lblReceived") as Label;
                Label lblPayable = gvRow.FindControl("lblPayable") as Label;
                HiddenField hdnCashFlowID = gvRow.FindControl("hdnCashFlowID") as HiddenField;

                strbAccountClosureDetails.Append("<Details ");
                strbAccountClosureDetails.Append(" Closure_Type_Code = '33'");
                strbAccountClosureDetails.Append(" Closure_Type = '2'");
                strbAccountClosureDetails.Append(" Closure_Status_Code = '1'");
                strbAccountClosureDetails.Append(" Closure_Status_Type_Code = '9'");
                strbAccountClosureDetails.Append(" PreClosure_Date = '" + Utility.StringToDate(txtPMCReqDate.Text) + "'");
                strbAccountClosureDetails.Append(" panum = '" + lblpanum.Text.Trim() + "'");
                strbAccountClosureDetails.Append(" pasa_id = '" + lblpasaid.Text.Trim() + "'");
                //if (ddlSLA.Items.Count == 1 && ddlSLA.SelectedValue == "0")
                strbAccountClosureDetails.Append(" SANum = '" + lblpanum.Text.Trim() + "DUMMY'");
                //else
                //    strbAccountClosureDetails.Append(" SANum = '" + ddlSLA.SelectedValue + "'");
                strbAccountClosureDetails.Append(" Cashflow_Component = '" + hdnCashFlowID.Value + "'");
                if (!string.IsNullOrEmpty(txtPreRate.Text.Trim()))
                    strbAccountClosureDetails.Append(" Closure_Rate = '" + txtPreRate.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Closure_Rate = '0'");

                if (!string.IsNullOrEmpty(lblPayable.Text.Trim()))
                    strbAccountClosureDetails.Append(" Payable_Amount = '" + lblPayable.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Payable_Amount = '0'");



                if (!string.IsNullOrEmpty(txtWaived.Text.Trim()))
                    strbAccountClosureDetails.Append(" Waived_Amount = '" + txtWaived.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Waived_Amount = '0'");

                if (!string.IsNullOrEmpty(txtPreAmount.Text.Trim()))
                    strbAccountClosureDetails.Append(" Closure_Amount = '" + txtPreAmount.Text + "'");
                else
                    strbAccountClosureDetails.Append(" Closure_Amount = '0'");

                //strbAccountClosureDetails.Append(" Remarks = '" + txtRemarks.Text.Trim() + "'");

                if (!string.IsNullOrEmpty(lblDue.Text.Trim()))
                    strbAccountClosureDetails.Append(" Due_Amount = '" + lblDue.Text.Trim() + "' ");
                else
                    strbAccountClosureDetails.Append(" Due_Amount = '0' ");

                strbAccountClosureDetails.Append(" PreClosure_Type = '" + ddlPreType.SelectedValue + "' ");
                strbAccountClosureDetails.Append(" Closure_Details_ID = '" + intClosureDetailId.ToString() + "' ");

                if (!string.IsNullOrEmpty(lblClosure.Text.Trim()))
                    strbAccountClosureDetails.Append(" AccClosure_Amount = '" + lblClosure.Text.Trim() + "' ");
                else
                    strbAccountClosureDetails.Append(" AccClosure_Amount = '0' ");

                strbAccountClosureDetails.Append(" PMCStatus = '" + HidPMCStatus.Value + "' ");
                strbAccountClosureDetails.Append(" PMC_Receipt_Amount = '" + HidPMC_Receipt_Amount.Value + "' ");
                strbAccountClosureDetails.Append(" />");
            }
            strbAccountClosureDetails.Append("</Root>");
            strXMLAccountClosureDetails = strbAccountClosureDetails.ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        return strXMLAccountClosureDetails;
    }

    #endregion [FunPriGenerateAccountClosureDetailsXMLDet]

    // Added By Shibu 24-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "085");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue.ToString());
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    #region [Set ErrorMessage for control]

    public void FunPubSetErrorMessageControl()
    {
        //rfvBranch.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Branch;
        //rfvMLA.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Select_PriAc;
        rfvLineOfBusiness.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LOB;
        //rfvSLA.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Select_SubAc;
        rfvClsoureBy.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LoanAd_AcPreClosure_Done;
        rfvPreType.ErrorMessage = "Preclosure type cannot be empty - Cashflow Details Tab";
        rfvPreRate.ErrorMessage = "Rental Months cannot be empty - Cashflow Details Tab";
        rfvPreAmount.ErrorMessage = "Preclosure amount cannot be empty - Cashflow Details Tab";
    }

    #endregion [Set ErrorMessage for control]

    //Added by Sathiyanathan on 31-Dec-2013

    [System.Web.Services.WebMethod]
    public static string[] GetPanumList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["intCompanyID"].ToString());
        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["intUserID"].ToString());
        Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue);
        //Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
        //if(obj_Page.uctranche.SelectedValue!="0")
        if (Convert.ToString(System.Web.HttpContext.Current.Session["Tranche_Id"]) != "0")
            Procparam.Add("@Tranche_id", System.Web.HttpContext.Current.Session["Tranche_Id"].ToString());
        Procparam.Add("@Type", "2");// Closure_Type code
        //Procparam.Add("@Page", "PRC");

        if (obj_Page.strAccountClosure == string.Empty)
            Procparam.Add("@IsModify", "0");
        else
            Procparam.Add("@IsModify", "1");

        if (obj_Page.txtPMCReqDate.Text.Trim() != "")
            Procparam.Add("@PMCDate", Utility.StringToDate(obj_Page.txtPMCReqDate.Text.Trim()).ToString());

        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GETPANUMON_PMC", Procparam));

        return suggestions.ToArray();
    }

    //End Sathiyanathan on 31-Dec-2013

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Program_id", obj_Page.strProgramId.ToString());

        if (obj_Page.txtPMCReqDate.Text.Trim() != "")
            Procparam.Add("@PMCDate", Utility.StringToDate(obj_Page.txtPMCReqDate.Text.Trim()).ToString());

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTrancheAGT", Procparam));
        return suggetions.ToArray();
    }

    #endregion [Function's]

}
