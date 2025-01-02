using System;
using S3GBusEntity;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data;
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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class LoanAdmin_S3GLOANADPreMatureClosureMemo_Add : ApplyThemeForProject
{
    #region [Local Field's]

    static decimal dcDue = 0, dcWaived = 0, dcPayable = 0, dcClosure = 0, dcReceived = 0, decCTR = 0, decPLR = 0;
    static decimal dcFooterDue = 0, dcFooterReceived = 0, dcFooterOS = 0;
    static decimal dcWaivedOff = 0, dcMinAmount = 0, Principle_Rate = 0, TotalPaid_Interest = 0;
    static decimal NextMth_FinanceCharge = 0, CurrentMth_FinanceCharge = 0, FuturePrincipalAmt = 0;
    static int Bill_InstallmentNo, Repayment_Mode_Code, Residual_Value;
    static string Time_Value = "", Frequency = string.Empty;
    static string strActivationType = string.Empty;
    static int RoundOff = 0, Return_Pattern = 0;
    static DateTime First_InstallmentDate = DateTime.Now, Current_InstallmentDt = DateTime.Now, Next_InstallmentDt = DateTime.Now;
    int intCompanyID, intUserID = 0, intGPS_DaysPerYear = 0;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    string strDateFormat = string.Empty;
    public string strProgramId = "201";
    //static string strCustomerEmail = string.Empty;
    public static LoanAdmin_S3GLOANADPreMatureClosureMemo_Add obj_Page;
    string strPANum = string.Empty;
    string strSANum = string.Empty;
    int intClosureDetailId = 0;
    string strAccountClosure = string.Empty;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";

    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLOANADPreMatureClosure_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PRC';";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=PRC";
    StringBuilder strbAccountClosureDetails = new StringBuilder();
    string strXMLAccountClosureDetails = "<Root><Details Desc='0' /></Root>";
    string strPageName = "Account PreClosure";
    ReportDocument rptd = new ReportDocument();

    #region [User Authorization]

    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;

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

    #endregion [Intialization]

    #region [Event's]

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        if (rptd != null)
        {
            rptd.Close();
            rptd.Dispose();
        }
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
        obj_Page = this;
        try
        {
            FunPubPageLoad();
            FunPubSetErrorMessageControl();
            txtPreRate.SetDecimalPrefixSuffix(2, 4, true, "Preclosure Rate");
            //txtPreAmount.SetDecimalPrefixSuffix(25, 2, true, "Preclosure Rate");
            txtPMCReqDate.Attributes.Add("Readonly", "true");
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
                ddlBranch.Clear();

            PopulateClosureType();

            if (ddlMLA.Items.Count > 0) ddlMLA.Items.Clear();
            if (ddlSLA.Items.Count > 0) ddlSLA.Items.Clear();

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
            //if (ddlBranch.Items.Count > 0)
             //   ddlBranch.SelectedValue = ddlBranch.Items[0].Value;

        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
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

    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            PopulateSANum(ddlMLA.SelectedValue);
            PopulatePANCustomer(ddlMLA.SelectedValue);
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            tbAccDetails.Enabled = tbCashFlow.Enabled = true;
            if (strAccountClosure == string.Empty)
            {
                FunGetAccountDetailsForClosure(ddlMLA.SelectedValue, ddlSLA.SelectedValue);
                PopulatePANCustomer(ddlMLA.SelectedValue);
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
            grvCashFlow.FooterRow.Cells[1].Text = dcDue.ToString();
            grvCashFlow.FooterRow.Cells[2].Text = dcWaived.ToString();
            grvCashFlow.FooterRow.Cells[3].Text = dcPayable.ToString();
            grvCashFlow.FooterRow.Cells[4].Text = txtPreAmount.Text = dcClosure.ToString();

            TextBox txtWaived = (sender) as TextBox;
            GridViewRow gvRow = txtWaived.Parent.Parent as GridViewRow;
            Label lblDue = gvRow.FindControl("lblDue") as Label;
            Label lblPayable = gvRow.FindControl("lblPayable") as Label;
            Label lblClosure = gvRow.FindControl("lblClosure") as Label;
            HiddenField hdnCashFlowID = gvRow.FindControl("hdnCashFlowID") as HiddenField;

            if (!String.IsNullOrEmpty(txtWaived.Text.Trim()))
            {
                //if (Convert.ToDecimal(txtWaived.Text) > dcWaivedOff)
                //{
                //    Utility.FunShowAlertMsg(this, "WaivedOff amount should be less than or equal to " + dcWaivedOff.ToString());
                //    txtWaived.Text = "";
                //    return;
                //}
                if (Convert.ToDecimal(txtWaived.Text) > Convert.ToDecimal(lblDue.Text))
                {
                    Utility.FunShowAlertMsg(this, "WaivedOff amount should be less than or equal to Due Amount");
                    txtWaived.Text = "";
                    return;
                }
                lblClosure.Text = (Convert.ToDecimal(lblDue.Text) - Convert.ToDecimal(txtWaived.Text) - Convert.ToDecimal(lblPayable.Text)).ToString();
            }
            else
                lblClosure.Text = (Convert.ToDecimal(lblDue.Text) - Convert.ToDecimal(lblPayable.Text)).ToString();

            if (ViewState["ClosureStatement"] != null)
            {
                DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                DataRow[] drClosureStatement = dtClosureStatement.Select(" ID = '" + hdnCashFlowID.Value + "'  ");
                drClosureStatement[0].BeginEdit();
                if (!String.IsNullOrEmpty(txtWaived.Text.Trim()))
                    drClosureStatement[0]["Waived"] = Convert.ToDecimal(txtWaived.Text.Trim());
                else
                    drClosureStatement[0]["Waived"] = "0.00";
                drClosureStatement[0].AcceptChanges();
                ViewState["ClosureStatement"] = dtClosureStatement;
            }

            Label lblReceived = gvRow.FindControl("lblReceived") as Label;
            lblReceived.Text = txtWaived.Text;

            dcWaived = dcClosure = dcReceived = 0;
            foreach (GridViewRow grRow in grvCashFlow.Rows)
            {
                lblClosure = grRow.FindControl("lblClosure") as Label;
                txtWaived = grRow.FindControl("txtWaived") as TextBox;
                lblReceived = grRow.FindControl("lblReceived") as Label;

                if (!string.IsNullOrEmpty(lblReceived.Text.Trim()))
                    dcReceived = dcReceived + Convert.ToDecimal(lblReceived.Text);
                if (!string.IsNullOrEmpty(txtWaived.Text.Trim()))
                    dcWaived = dcWaived + Convert.ToDecimal(txtWaived.Text);
                if (!string.IsNullOrEmpty(lblClosure.Text.Trim()))
                    dcClosure = dcClosure + Convert.ToDecimal(lblClosure.Text);
            }
            grvCashFlow.FooterRow.Cells[5].Text = dcReceived.ToString();
            grvCashFlow.FooterRow.Cells[2].Text = dcWaived.ToString();
            grvCashFlow.FooterRow.Cells[4].Text = txtPreAmount.Text = dcClosure.ToString();
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
    protected void txtPreRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriCalBrokenPeriodInterest();
            FunPubCalCulatePMCAmount();
            txtPreRate.Focus();
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = ex.Message;
            cvAccountPreClosure.IsValid = false;
        }
    }

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
            if (Time_Value.ToUpper() == "ADVANCE")
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
                    if (Time_Value.ToUpper().Contains("ADVANCE"))
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

    protected void txtPMCReqDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtPMCReqDate.Text.Trim()))
            {
                txtPreclosureDate.Text = txtPMCReqDate.Text.Trim();
                if (ddlMLA.SelectedValue.ToString() != "" && ddlMLA.SelectedValue.ToString()!="0")
                {
                    if (ddlSLA.SelectedValue == "0")
                        FunGetAccountDetailsForClosure(ddlMLA.SelectedValue, ddlMLA.SelectedValue + "DUMMY");
                    else
                        FunGetAccountDetailsForClosure(ddlMLA.SelectedValue, ddlSLA.SelectedValue);

                    FunPriCalBrokenPeriodInterest();
                    FunPubCalCulatePMCAmount();
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
            hdnIRR.Value = "0";
            if (strActivationType.ToUpper() != "IRR" && ddlPreType.SelectedItem.Text.ToUpper().Trim() == "IRR")
            {
                Utility.FunShowAlertMsg(this, " Amortization schedule method and Premature closure type are not same(Only for IRR). Unable to proceed Premature Closure");
                hdnIRR.Value = "2";
                ddlPreType.SelectedIndex = 0;
                return;
            }
            if (ddlPreType.SelectedValue == "30")
            {
                txtPreRate.ReadOnly = true;
                txtPreRate.Text = Principle_Rate.ToString();
            }
            else
            {
                txtPreRate.ReadOnly = false;
            }
            FunPriCalBrokenPeriodInterest();
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

                txtWaived.SetDecimalPrefixSuffix(25, 2, false, "Waived Amount");
                if (hdnCashFlowID.Value == "24" || hdnCashFlowID.Value == "26" || hdnCashFlowID.Value == "42")// Memo,penalty & ODI
                    txtWaived.Visible = true;
                else
                    txtWaived.Visible = false;

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
            FunPriSetFooterValue();
            FunSaveClick();
        }
        catch (Exception ex)
        {
            cvAccountPreClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_Save;
            cvAccountPreClosure.IsValid = false;
        }
    }

    public void FunSaveValidation(string strButton)
    {
        try
        {
            int intDtCom = 0;
            string strEndDate = "";
            if (strAccountClosure == string.Empty && txtPMCReqDate.Text.Trim() != "" && txtMatureDate.Text.Trim() != "")
            {
                intDtCom = Utility.CompareDates(txtMatureDate.Text, txtPMCReqDate.Text);
                if (intDtCom != -1)
                {
                    cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_LT_MaturityDt;
                    cvAccountPreClosure.IsValid = false;
                    return;
                }
                if (ddlSLA.Items.Count > 1)
                {
                    if (ddlSLA.SelectedValue == "0")
                    {
                        cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_Select_SubAc;
                        cvAccountPreClosure.IsValid = false;
                        return;
                    }
                }
            }
            if (strAccountClosure == string.Empty && strButton == "Save")
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
                if (intDtCom == -1)
                {
                    cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_EqlOrGT_CurrentDt;
                    cvAccountPreClosure.IsValid = false;
                    return;
                }
            }
            if (hdnIRR.Value == "1")
            {
                cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + "Error in calculating the Pre Closure Amount. Recalculate the Pre Closure Amount ";
                cvAccountPreClosure.IsValid = false;
                return;
            }
            if (hdnIRR.Value == "2")
            {
                cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + " Amortization schedule method  and Premature closure type are not same(Only for IRR). Unable to proceed Premature Closure";
                cvAccountPreClosure.IsValid = false;
                return;
            }
            if (grvAccountBalance.Rows.Count == 0 && grvCashFlow.Rows.Count == 0)
            {
                cvAccountPreClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_AccClosure_Gen;
                cvAccountPreClosure.IsValid = false;
                return;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    public void FunSaveClick()
    {

        try
        {
            FunSaveValidation("Save");
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
                            if (txtMode.Text.ToUpper().Trim() == "PDC")
                            {
                                strAlert += "All Post dated cheques are processed. No more pending Post dated cheques... ";
                            }

                            strAlert += Resources.ValidationMsgs.S3G_SucMsg_LoanAd_PreMature_Closed + " - " + strDocNo;
                            strAlert += @" \n\n" + strMessage;
                            strAlert += @"\n\nWould you like to view the Pre Closure Statement?";
                            strAlert = "if(confirm('" + strAlert + "')){ document.getElementById('" + Button1.ClientID + "').click();}else {if(confirm('" + (@Resources.ValidationMsgs.S3G_ValMsg_Next).Replace("\\n", "") + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}" + "}";
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
                            strAlert += @" \n\n" + strMessage;
                            strAlert += @"\n\nWould you like to view the Pre Closure Statement?";
                            strAlert = "if(confirm('" + strAlert + "')){ document.getElementById('" + Button1.ClientID + "').click();}else {if(confirm('" + (@Resources.ValidationMsgs.S3G_ValMsg_Next).Replace("\\n", "") + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}" + "}";
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
                case 8:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Pre Clsoure Amount has to be collected from Customer.");
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
                        Utility.FunShowAlertMsg(this.Page, "Billing should be run up to Pre Closure Date ");
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
            FunSaveValidation("");
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
            FunSaveValidation("");
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
        Response.Redirect(strRedirectPage,false);
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

            #endregion User Authorization

            if (Request.QueryString["Popup"] != null)  //transaction screen page load
                btnCancel.Enabled = false;

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender2.Format = strDateFormat;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strAccountClosure = fromTicket.Name.Split('~')[0];
                strPANum = fromTicket.Name.Split('~')[1];
                strSANum = fromTicket.Name.Split('~')[2];
                intClosureDetailId = Convert.ToInt32(fromTicket.Name.Split('~')[3]);
                hidClosureDetailId.Value = intClosureDetailId.ToString();
                if (strSANum == string.Empty)
                    strSANum = strPANum + "DUMMY";
            }

            if (!IsPostBack)
            {
                txtPreclosureDate.Text = DateTime.Now.ToString(strDateFormat);
              
            
                if (Request.QueryString["qsMode"] == "Q" && Request.QueryString["qsMode"] == "M")
                {
                    PopulateBranchList();
                }
                else
                {
                    PopulateLOBList();
                }
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (Request.QueryString["qsMode"] == "Q")
                {
                    //tbReceipt.Visible = true;
                    FunGetAccountsClosedForModify(strAccountClosure, strPANum, strSANum);
                    FunGetAccountDetailsForClosure(strPANum, strSANum);
                    FunPriDisableControls(-1);
                }
                else if (Request.QueryString["qsMode"] == "M")
                {
                    //tbReceipt.Visible = true;
                    FunGetAccountsClosedForModify(strAccountClosure, strPANum, strSANum);
                    FunGetAccountDetailsForClosure(strPANum, strSANum);
                    FunPriDisableControls(1);
                    //btnPrint.Enabled = true;
                }
                else
                {
                    txtPMCReqDate.Text = DateTime.Now.ToString(strDateFormat);
                    FunPriDisableControls(0);
                }
                txtClosureBy.Text = ObjUserInfo.ProUserNameRW.Trim().ToUpper();
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
            if (strAccountClosure == string.Empty) Procparam.Add("@Is_Active", "1");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

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
            Procparam = FunPriProcParam();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Type", "2");// Closure_Type code
            Procparam.Add("@Page", "PRC");

            if (strAccountClosure == string.Empty)
                Procparam.Add("@IsModify", "0");
            else
                Procparam.Add("@IsModify", "1");

            if (txtPMCReqDate.Text.Trim() != "")
                Procparam.Add("@PMCDate", Utility.StringToDate(txtPMCReqDate.Text.Trim()).ToString());

            ddlMLA.BindDataTable(SPNames.S3G_LOANAD_GetPANumForAccountClosure, Procparam, new string[] { "PANum", "PANum" });

            ddlMLA.Items[0].Text = "--Select--";
            if (ddlMLA.Items.Count == 2)
            {
                ddlMLA.SelectedValue = ddlMLA.Items[1].Value;
                PopulateSANum(ddlMLA.SelectedValue);
                PopulatePANCustomer(ddlMLA.SelectedValue);
            }
            else if (ddlMLA.Items.Count == 1)
            {
                ddlSLA.Items.Clear();
            }
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

            ddlSLA.BindDataTable(SPNames.S3G_LOANAD_GetAccountClosureSANum, Procparam, new string[] { "SANum", "SANum" });

            if (ddlSLA.Items.Count == 2 && strAccountClosure == "")
            {
                ddlSLA.SelectedValue = ddlSLA.Items[1].Value;
            }

            if (ddlSLA.Items.Count > 1)
                rfvSLA.Enabled = true;
            else
                rfvSLA.Enabled = false;

            if ((ddlSLA.Items.Count == 1 || ddlSLA.Items.Count == 2) && strAccountClosure == "")
            {
                tbAccDetails.Enabled = true;
                tbCashFlow.Enabled = true;
                if (strAccountClosure == string.Empty)
                {
                    if (ddlSLA.SelectedValue == "0")
                        FunGetAccountDetailsForClosure(ddlMLA.SelectedValue, ddlMLA.SelectedValue + "DUMMY");
                    else
                        FunGetAccountDetailsForClosure(ddlMLA.SelectedValue, ddlSLA.SelectedValue);
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
        try
        {
            objS3G_LOANAD_AccClosureTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDataTable();
            objS3G_LOANAD_AccClosureDataRow = objS3G_LOANAD_AccClosureTable.NewS3G_LOANAD_AccountClosureRow();

            objS3G_LOANAD_AccClosureDataRow.Company_ID = intCompanyID;
            objS3G_LOANAD_AccClosureDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objS3G_LOANAD_AccClosureDataRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objS3G_LOANAD_AccClosureDataRow.Closure_No = strAccountClosure;
            objS3G_LOANAD_AccClosureDataRow.PANum = ddlMLA.SelectedValue;
            if (ddlSLA.SelectedValue != "0") objS3G_LOANAD_AccClosureDataRow.SANum = ddlSLA.SelectedValue;
            else objS3G_LOANAD_AccClosureDataRow.SANum = ddlMLA.SelectedValue + "DUMMY";
            objS3G_LOANAD_AccClosureDataRow.Customer_ID = Convert.ToInt32(hdnCustomerID.Value);  // Convert.ToInt32(txtCustCode.Attributes["Cust_ID"]);
            objS3G_LOANAD_AccClosureDataRow.Closure_Date = Utility.StringToDate(txtPreclosureDate.Text);
            objS3G_LOANAD_AccClosureDataRow.Created_By = intUserID;
            objS3G_LOANAD_AccClosureDataRow.Created_On = DateTime.Now;
            objS3G_LOANAD_AccClosureDataRow.User_ID = intUserID;
            if (txtPreAmount.Text.Trim() != "")
                objS3G_LOANAD_AccClosureDataRow.Closure_Amount = Convert.ToDecimal(txtPreAmount.Text);
            else
                objS3G_LOANAD_AccClosureDataRow.Closure_Amount = 0;
            objS3G_LOANAD_AccClosureDataRow.XMLAccountClosureDetails = FunPriGenerateAccountClosureDetailsXMLDet();
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
                grvCashFlow.FooterRow.Cells[1].Text = dcDue.ToString();
                grvCashFlow.FooterRow.Cells[2].Text = dcWaived.ToString();
                grvCashFlow.FooterRow.Cells[3].Text = dcPayable.ToString();
                grvCashFlow.FooterRow.Cells[4].Text = txtPreAmount.Text = dcClosure.ToString();
                grvCashFlow.FooterRow.Cells[5].Text = dcReceived.ToString();
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
            if (grvAccountBalance.FooterRow != null)
            {
                grvAccountBalance.FooterRow.Cells[1].Text = dcFooterDue.ToString();
                grvAccountBalance.FooterRow.Cells[2].Text = dcFooterReceived.ToString();
                grvAccountBalance.FooterRow.Cells[3].Text = dcFooterOS.ToString();
            }
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
            if (ddlSLA.SelectedValue == "0")
                Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
            else
                Procparam.Add("@SANum", ddlSLA.SelectedValue);
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@Closure_Date", Utility.StringToDate(txtPMCReqDate.Text).ToString());

            Dst = Utility.GetDataset("S3G_LOANAD_AccountClosureMemo", Procparam);

            DataTable dtAccountBalance = null;
            DataTable dtClosureStatement = null;
            if (ViewState["AccountBalance"] != null)
                dtAccountBalance = ViewState["AccountBalance"] as DataTable;
            if (ViewState["ClosureStatement"] != null)
                dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                        
            rptd.Load(Server.MapPath("PrematureClosureMemo.rpt"));

            if (Dst != null && Dst.Tables.Count > 0)
            {
                if (Dst.Tables[0] != null && Dst.Tables[0].Rows.Count > 0)
                {
                    DataTable dtAccount = Dst.Tables[0];
                    if (dtAccount != null && dtAccount.Rows.Count > 0)
                    {
                        if (dtAccountBalance != null && dtAccountBalance.Rows.Count > 0)
                        {
                            DataRow[] drAccountBalance = dtAccountBalance.Select("Desc='Installment'");
                            if (drAccountBalance.Length > 0)
                            {
                                dtAccount.Rows[0]["Billed_Amount"] = Convert.ToDecimal(drAccountBalance[0]["Due"].ToString());
                                dtAccount.Rows[0]["Received_Amount"] = Convert.ToDecimal(drAccountBalance[0]["Received"].ToString());
                            }

                        }
                        if (dtClosureStatement != null && dtClosureStatement.Rows.Count > 0)
                        {
                            DataRow[] drODIWaived = dtClosureStatement.Select("ID='24'");
                            DataRow[] drOtherWaived = dtClosureStatement.Select("ID<>'24'");

                            decimal Closure_Amount = 0;
                            if (drODIWaived.Length > 0)
                            {
                                dtAccount.Rows[0]["ODI_Waived"] = dtClosureStatement.Compute("Sum(Waived)", "ID='24'");
                                Closure_Amount += Convert.ToDecimal(dtClosureStatement.Compute("Sum(Waived)", "ID='24'"));
                            }
                            if (drOtherWaived.Length > 0)
                            {
                                dtAccount.Rows[0]["Other_Waived"] = dtClosureStatement.Compute("Sum(Waived)", "ID<>'24'");
                                Closure_Amount += Convert.ToDecimal(dtClosureStatement.Compute("Sum(Waived)", "ID<>'24'"));
                            }
                            if (txtPreAmount.Text != "")
                                Closure_Amount += Convert.ToDecimal(txtPreAmount.Text);

                            dtAccount.Rows[0]["Closure_Amount"] = Closure_Amount;
                        }
                        dtAccount.Rows[0]["NetClosureAmount"] = Convert.ToDecimal(txtPreAmount.Text);
                        dtAccount.AcceptChanges();

                        rptd.SetDataSource(dtAccount);
                    }
                }

                if (dtClosureStatement != null && dtClosureStatement.Rows.Count > 0)
                {
                    rptd.Subreports["AccountDetails"].SetDataSource(dtClosureStatement);
                }
                if (Dst.Tables[1] != null && Dst.Tables[1].Rows.Count > 0)
                {
                    rptd.Subreports["Assetdetails"].SetDataSource(Dst.Tables[1]);
                }
                strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + objGuid + "PMCMemo.pdf");

                rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);//Server.MapPath(".") + "\\PDF Files\\" + ddlSLA.SelectedValue);
                string strScipt = "";

                if (blnPrint)
                {
                    strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strnewFile.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
                    //System.Diagnostics.Process.Start(strnewFile);
                }
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
            if (rptd != null)
            {
                rptd.Close();
                rptd.Dispose();
            }
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
            objCancelRow.Closure_No = strAccountClosure;
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

    private void PopulatePANCustomer(string strPAN)
    {
        try
        {
            DataTable dtTable = new DataTable();
            Procparam = FunPriProcParam();
            Procparam.Add("@PANum", strPAN);
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

    private void FunGetAccountDetailsForClosure(string strPANum, string strSANum)
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
            Procparam.Add("@PANum", strPANum);
            Procparam.Add("@SANum", strSANum);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@ClosureDetailId", intClosureDetailId.ToString());
            Procparam.Add("@PAGE", "PMC");
            Procparam.Add("@User_ID", intUserID.ToString());

            if (txtPMCReqDate.Text.Trim() != "") Procparam.Add("@PMCDate", Utility.StringToDate(txtPMCReqDate.Text).ToString());

            dtSet = Utility.GetTableValues("S3G_LOANAD_GetAccountDetailsForClosure", Procparam);
            //if (dtSet != null && dtSet.Tables.Count > 0)
            //    ViewState["dtSet"] = dtSet;

            /*ReportDocument rptd = new ReportDocument();
            rptd.Load(Server.MapPath("PrematureClosure.rpt"));
            rptd.SetDataSource(dtSet);
            rptd.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath(".") + "\\PDF Files\\" + ddlSLA.SelectedValue);*/

            if (dtSet != null)
            {
                if (dtSet.Tables[0] != null && dtSet.Tables[0].Rows.Count > 0)
                {
                    txtAccDate.Text = DateTime.Parse(dtSet.Tables[0].Rows[0]["Creation_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                    TxtAccStatus.Text = dtSet.Tables[0].Rows[0]["Status"].ToString();

                    txtPrincipal.Text = dtSet.Tables[0].Rows[0]["Finance_Amount"].ToString();

                    intTenureType = Convert.ToInt32(dtSet.Tables[0].Rows[0]["Tenure_Code"]);
                    intTenure = Convert.ToDouble(dtSet.Tables[0].Rows[0]["Tenure"]);

                    if (dtSet.Tables[0].Rows[0]["MatureDate"].ToString() != "") txtMatureDate.Text = DateTime.Parse(dtSet.Tables[0].Rows[0]["MatureDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    txtIRR.Text = dtSet.Tables[0].Rows[0]["Accounting_IRR"].ToString();
                    txtBusinessIRR.Text = dtSet.Tables[0].Rows[0]["Business_IRR"].ToString();
                    txtCompanyIRR.Text = dtSet.Tables[0].Rows[0]["Company_IRR"].ToString();
                    strTenureDesc = dtSet.Tables[0].Rows[0]["TenureDesc"].ToString();
                    txtTenure.Text = dtSet.Tables[0].Rows[0]["Tenure"].ToString() + " " + strTenureDesc;
                    Tenure = Int32.Parse(dtSet.Tables[0].Rows[0]["Tenure"].ToString());

                }
                if (dtSet.Tables[2] != null && dtSet.Tables[2].Rows.Count > 0)
                {
                    grvAsset.DataSource = dtSet.Tables[2];
                    grvAsset.DataBind();
                }
                if (dtSet.Tables[3] != null && dtSet.Tables[3].Rows.Count > 0)
                {
                    grvAccountBalance.DataSource = dtSet.Tables[3];
                    grvAccountBalance.DataBind();
                    FunPriBindAccountBalanceGrid(dtSet.Tables[3]);
                    ViewState["AccountBalance"] = dtSet.Tables[3];
                }

                if (dtSet.Tables[1] != null && dtSet.Tables[1].Rows.Count > 0)
                {
                    txtFlatRate.Text = dtSet.Tables[1].Rows[0]["Rate"].ToString();
                    txtMode.Text = dtSet.Tables[1].Rows[0]["Lookup_Description"].ToString();
                    if (txtPrincipal.Text.Trim() != "" && txtFlatRate.Text.Trim() != "" && txtTenure.Text.Trim() != "")
                        txtFinanceCharge.Text = Convert.ToString(Math.Round(CommonS3GBusLogic.FunPubInterestAmount(strTenureDesc, Convert.ToDecimal(txtPrincipal.Text), Convert.ToDecimal(txtFlatRate.Text), Tenure)));

                    Return_Pattern = Convert.ToInt32(dtSet.Tables[1].Rows[0]["Return_Pattern"].ToString());
                }
                if (dtSet.Tables[6] != null) ViewState["RepayStructure"] = dtSet.Tables[6];

                bool booLPO = false;
                if (dtSet.Tables[5] != null && dtSet.Tables[5].Rows.Count > 0)
                {
                    if (dtSet.Tables[5].Rows[0]["Waived_Amount"].ToString() != "")
                        dcWaivedOff = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["Waived_Amount"].ToString());
                    if (dtSet.Tables[5].Rows[0]["Min_Amount"].ToString() != "")
                        dcMinAmount = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["Min_Amount"].ToString());
                    if (dtSet.Tables[5].Rows[0]["Activation_Type"].ToString() != "")
                        strActivationType = dtSet.Tables[5].Rows[0]["Activation_Type"].ToString();

                    if (ddlPreType.Items.Count > 1)
                        ddlPreType.SelectedIndex = 1;
                    ddlPreType.Enabled = false;

                    if (dtSet.Tables[5].Rows[0]["PRINCIPLE_RATE"].ToString() != "")
                    {
                        Principle_Rate = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["PRINCIPLE_RATE"].ToString());
                    }
                    else
                    {
                        Principle_Rate = 0;
                    }

                    txtPreRate.ReadOnly = false;
                    txtPreRate.Text = Principle_Rate.ToString();

                    if (ddlPreType.SelectedValue == "30")
                    {
                        txtPreRate.ReadOnly = true;
                    }

                    if (dtSet.Tables[5].Rows[0]["Bill_InstallmentNo"].ToString() != "")
                        Bill_InstallmentNo = Convert.ToInt32(dtSet.Tables[5].Rows[0]["Bill_InstallmentNo"].ToString());
                    if (dtSet.Tables[5].Rows[0]["Repayment_Mode_Code"].ToString() != "")
                        Repayment_Mode_Code = Convert.ToInt32(dtSet.Tables[5].Rows[0]["Repayment_Mode_Code"].ToString());
                    if (dtSet.Tables[5].Rows[0]["Residual_Value"].ToString() != "")
                        Residual_Value = Convert.ToInt32(dtSet.Tables[5].Rows[0]["Residual_Value"].ToString());
                    if (dtSet.Tables[5].Rows[0]["Next_InstallmentDt"].ToString() != "")
                        Next_InstallmentDt = Utility.StringToDate(dtSet.Tables[5].Rows[0]["Next_InstallmentDt"].ToString());
                    if (dtSet.Tables[5].Rows[0]["Current_InstallmentDt"].ToString() != "")
                        Current_InstallmentDt = Utility.StringToDate(dtSet.Tables[5].Rows[0]["Current_InstallmentDt"].ToString());
                    if (dtSet.Tables[5].Rows[0]["First_InstallmentDate"].ToString() != "")
                        First_InstallmentDate = Utility.StringToDate(dtSet.Tables[5].Rows[0]["First_InstallmentDate"].ToString());
                    if (dtSet.Tables[5].Rows[0]["RoundOff"].ToString() != "")
                        RoundOff = Convert.ToInt32(dtSet.Tables[5].Rows[0]["RoundOff"].ToString());
                    if (dtSet.Tables[5].Rows[0]["decPLR"].ToString() != "")
                        decPLR = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["decPLR"].ToString());
                    if (dtSet.Tables[5].Rows[0]["decCTR"].ToString() != "")
                        decCTR = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["decCTR"].ToString());
                    if (dtSet.Tables[5].Rows[0]["NextMth_FinanceCharge"].ToString() != "")
                        NextMth_FinanceCharge = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["NextMth_FinanceCharge"].ToString());
                    if (dtSet.Tables[5].Rows[0]["CurrentMth_FinanceCharge"].ToString() != "")
                        CurrentMth_FinanceCharge = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["CurrentMth_FinanceCharge"].ToString());

                    Frequency = dtSet.Tables[5].Rows[0]["Frequency"].ToString();
                    Time_Value = dtSet.Tables[5].Rows[0]["Time_Value"].ToString();
                    if (dtSet.Tables[5].Rows[0]["TotalPaid_Interest"].ToString() != "")
                        TotalPaid_Interest = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["TotalPaid_Interest"].ToString());
                    if (dtSet.Tables[5].Rows[0]["FuturePrincipalAmt"].ToString() != "")
                        FuturePrincipalAmt = Convert.ToDecimal(dtSet.Tables[5].Rows[0]["FuturePrincipalAmt"].ToString());
                    if (dtSet.Tables[5].Rows[0]["LPO"].ToString() != "")
                        booLPO = Convert.ToBoolean(dtSet.Tables[5].Rows[0]["LPO"].ToString());
                }
                ViewState["ClosureStatement"] = dtSet.Tables[4];
                if (dtSet.Tables[4] != null && dtSet.Tables[4].Rows.Count > 0)
                {
                    FunPriBindCashFlowGrid();
                    if (intClosureDetailId == 0) FunPriCalBrokenPeriodInterest();
                }
                ViewState["Company"] = dtSet.Tables[7];
                if (ddlLOB.SelectedItem.Text.Contains("OL") && !booLPO && ddlMLA.SelectedValue != "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Lease purchase order is not generated. Can not preclose the account');" + strRedirectPageAdd, true);
                    return;
                }

                FunPubCalCulatePMCAmount();
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
                break;

            case 1: // Modify Mode

                txtPreRate.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                tbAccDetails.Enabled = tbCashFlow.Enabled = btnClosure.Visible = true;
                txtClosureBy.ReadOnly = txtPMCReqDate.ReadOnly = true;
                CalendarExtender2.Enabled = ddlPreType.Enabled = false;
                //btnPrint.Enabled = true;
                break;


            case -1:// Query Mode

                txtPreRate.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                ddlPreType.Enabled = CalendarExtender2.Enabled = btnSave.Enabled = false;
                tbAccDetails.Enabled = tbCashFlow.Enabled = true;
                txtClosureBy.ReadOnly = txtPMCReqDate.ReadOnly = txtPreRate.ReadOnly = true;
                btnEmail.Enabled = false;

                if (bClearList)
                {
                    //ddlBranch.ClearDropDownList();
                    ddlLOB.ClearDropDownList();
                    ddlMLA.ClearDropDownList();
                    ddlSLA.ClearDropDownList();
                    //ddlInvoice.ClearDropDownList();
                }
                foreach (GridViewRow gvRow in grvCashFlow.Rows)
                {
                    TextBox txtWaived = (TextBox)gvRow.FindControl("txtWaived");
                    TextBox txtRemarks = (TextBox)gvRow.FindControl("txtRemarks");
                    txtWaived.ReadOnly = txtRemarks.ReadOnly = true;
                }
                break;
        }
    }

    #endregion  [User Authorization]

    #region "To Get details for Modification"

    private void FunGetAccountsClosedForModify(string strAccountClosure, string strPANum, string strSANum)
    {
        DataTable dtTable = new DataTable();

        objAccountClosure_Client = new ContractMgtServicesReference.ContractMgtServicesClient();
        try
        {
            byte[] bytesAccount = objAccountClosure_Client.FunGetAccountsClosedForModify(strAccountClosure, strPANum, strSANum, intClosureDetailId, intCompanyID);

            dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesAccount, SerializationMode.Binary, typeof(DataTable));

            if (dtTable != null && dtTable.Rows.Count > 0)
            {
                txtAccClosureNo.Text = hidAccClosureNo.Value = strAccountClosure;
                if (dtTable.Rows[0]["PreClosure_Date"].ToString() != "")
                    txtPMCReqDate.Text = DateTime.Parse(dtTable.Rows[0]["PreClosure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                if (dtTable.Rows[0]["Closure_Date"].ToString() != "")
                    txtPreclosureDate.Text = DateTime.Parse(dtTable.Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dtTable.Rows[0]["LOBName"].ToString(), ddlMLA.SelectedValue = dtTable.Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = dtTable.Rows[0]["LOBName"].ToString();
               
                ddlBranch.SelectedValue = dtTable.Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = dtTable.Rows[0]["Location_Name"].ToString();
                ddlBranch.ToolTip = dtTable.Rows[0]["Location_Name"].ToString();
                ucdCustomer.SetCustomerDetails(dtTable.Rows[0], true);

                hdnCustomerID.Value = dtTable.Rows[0]["Customer_ID"].ToString();
                PopulatePANum();
                ddlMLA.SelectedValue = dtTable.Rows[0]["PANum"].ToString();
                PopulateSANum(dtTable.Rows[0]["PANum"].ToString());

                ddlSLA.SelectedValue = strSANum;
                PopulateClosureType();
                txtPreAmount.Text = dtTable.Rows[0]["Closure_Amount"].ToString();
                System.Web.UI.WebControls.ListItem objList;
                switch (dtTable.Rows[0]["PreClosure_Type"].ToString())
                {
                    case "28":
                        objList = new System.Web.UI.WebControls.ListItem("IRR", dtTable.Rows[0]["PreClosure_Type"].ToString());
                        ddlPreType.Items.Insert(0, objList);
                        break;
                    case "29":
                        objList = new System.Web.UI.WebControls.ListItem("NPV", dtTable.Rows[0]["PreClosure_Type"].ToString());
                        ddlPreType.Items.Insert(0, objList);
                        break;
                    case "30":
                        objList = new System.Web.UI.WebControls.ListItem("Principal", dtTable.Rows[0]["PreClosure_Type"].ToString());
                        ddlPreType.Items.Insert(0, objList);
                        break;
                }

                if (ddlLOB.SelectedItem.Text.Trim().Contains("Working") || ddlLOB.SelectedItem.Text.Trim().Contains("Factor"))
                    txtPreRate.Text = "";
                else
                    txtPreRate.Text = dtTable.Rows[0]["Closure_Rate"].ToString();

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
            }
            ddlLOB.Enabled = ddlBranch.Enabled = ddlMLA.Enabled = ddlSLA.Enabled = false;
            if (ddlLOB.SelectedItem.Text.Trim().Contains("Working") || ddlLOB.SelectedItem.Text.Trim().Contains("Factor"))
                rfvPreType.Enabled = rfvPreRate.Enabled = ddlPreType.Enabled = txtPreRate.Enabled = false;
            else
                rfvPreType.Enabled = rfvPreRate.Enabled = ddlPreType.Enabled = txtPreRate.Enabled = true;


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
        hdnIRR.Value = "0";
        txtPreAmount.Text = "";

        int intDtCom = 0;
        string strEndDate = "";
        if (strAccountClosure == string.Empty && txtPMCReqDate.Text.Trim() != "" && txtMatureDate.Text.Trim() != "")
        {
            intDtCom = Utility.CompareDates(txtMatureDate.Text, txtPMCReqDate.Text);
            if (intDtCom != -1)
            {
                Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_ValMsg_LoanAd_PreMatureDt_LT_MaturityDt);
                txtPMCReqDate.Text = txtPreclosureDate.Text = DateTime.Now.ToString(strDateFormat);
            }
        }

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

        if (ddlPreType.SelectedValue != "0" && txtPreRate.Text.Trim() != "" && txtPMCReqDate.Text.Trim() != "" && Convert.ToDecimal(txtPreRate.Text.Trim()) <= 100)
        {
            ClsRepaymentStructure objClsRepaymentStructure = null;
            CommonS3GBusLogic objCommonS3GBusLogic = null;
            decimal decPenalty = 0;
            decimal decPreRate = Convert.ToDecimal(txtPreRate.Text);
            DateTime dtPMCReqDate = Utility.StringToDate(txtPMCReqDate.Text);
            txtPreRate.ReadOnly = false;

            try
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
                            string strResidualValue = "", strResidualAmount = "", strIRR_Rest = "";
                            double decAccountingIRR = 0, decBusinessIRR = 0, decCompanyIRR = 0;

                            objClsRepaymentStructure = new ClsRepaymentStructure();
                            objCommonS3GBusLogic = new CommonS3GBusLogic();
                            RepaymentType rePayType = new RepaymentType();

                            Dictionary<string, string> objParameters = new Dictionary<string, string>();
                            objParameters.Add("@PANum", ddlMLA.SelectedValue);
                            if (ddlSLA.SelectedValue != "0") objParameters.Add("@SANum", ddlSLA.SelectedValue);
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
                            string[] arrTenure = txtTenure.Text.Split(' ');
                            if (arrTenure.Length > 1) strTenureType = arrTenure[1];
                            int intTenure = Convert.ToInt32(arrTenure[0]);
                            if (txtPrincipal.Text.Trim() != "") decPrincipleAmount = Convert.ToDecimal(txtPrincipal.Text);
                            if (txtAccDate.Text.Trim() != "") dtAppdate = Utility.StringToDate(txtAccDate.Text);

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
                            objMethodParameters.Add("FinanceAmount", txtPrincipal.Text);
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
                            objMethodParameters.Add("PrincipalMethod", "0");

                            decimal decRateOut = 0;
                            DataTable dtMoratorium = null;
                            DataSet dsRepayGrid = objClsRepaymentStructure.FunPubGenerateRepaymentSchedule(First_InstallmentDate, dsRepayStrucure.Tables[0], dsRepayStrucure.Tables[1], objMethodParameters, dtMoratorium, out decRateOut);
                            DataTable DtRepayGrid = dsRepayGrid.Tables[0];

                            ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);

                            DataSet dsRepaymentStructureTable = objCommonS3GBusLogic.FunPubGetRepaymentDetails(Frequency, intTenure,
                                strTenureType, decPrincipleAmount,
                               Convert.ToDecimal(ViewState["decRate"]), rePayType, decResidualValue,
                                 First_InstallmentDate, dtAppdate, intCashflowId, strCashflowdesc, Convert.ToDecimal(RoundOff),
                                 blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, Time_Value, dtMoratorium);

                            DataTable dtRepaymentStructure = objCommonS3GBusLogic.FunPubGenerateRepaymentStructure
                                (dsRepaymentStructureTable.Tables[0], dsRepayStrucure.Tables[0], dsRepayStrucure.Tables[1],
                                Frequency, intTenure, strTenureType, strDateFormat,
                                Convert.ToDecimal(objClsRepaymentStructure.FunPubGetAmountFinanced(decPrincipleAmount.ToString(),
                                strMargin_Percentage)), Convert.ToDecimal(ViewState["decRate"].ToString()), strIRR_Rest,
                                Time_Value, "", Convert.ToDecimal(10.05), decCTR, decPLR, First_InstallmentDate,
                                decResidualValue, decResidualAmount, rePayType, out decAccountingIRR, out decBusinessIRR,
                                out decCompanyIRR, "", dtMoratorium, ddlLOB.SelectedItem.Text.Trim().Split('-')[0].ToString().Trim(), "NIL", 0);

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
                                TimeSpan diff1 = Utility.StringToDate(txtMatureDate.Text).Subtract(dtPMCReqDate);
                                //decPenalty = //(Convert.ToDecimal(FuturePrincipalAmt) * Principle_Rate)/100;
                                //Math.Round((((Convert.ToDecimal(FuturePrincipalAmt)  * Principle_Rate)/100) / decRoundOff), 0) * decRoundOff;

                                decPenalty = Math.Round((Convert.ToDecimal(FuturePrincipalAmt) * Principle_Rate) / 100, 2);

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
                                    drPenaltyRow[0]["Due"] = decPenalty;// Math.Round(decPenalty, 2);
                                    drPenaltyRow[0]["Closure"] = decPenalty;// Math.Round(decPenalty, 2);
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
                }
            }
            catch (Exception ex)
            {
                hdnIRR.Value = "1";
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

                dcDue = Convert.ToDecimal(dtClosureStatement.Compute("sum(due)", "Description<>''"));
                dcPayable = Convert.ToDecimal(dtClosureStatement.Compute("sum(Payable)", "Description<>'' "));
                dcClosure = Convert.ToDecimal(dtClosureStatement.Compute("sum(Closure)", "Description<>'' "));
                dcWaived = Convert.ToDecimal(dtClosureStatement.Compute("sum(Waived)", "Description<>''  "));
                dcReceived = Convert.ToDecimal(dtClosureStatement.Compute("sum(Received)", "Description<>''  "));
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
            dcFooterDue = Convert.ToDecimal(dtAccountBalanc.Compute("sum(Due)", "Desc<>''"));
            dcFooterReceived = Convert.ToDecimal(dtAccountBalanc.Compute("sum(Received)", "Desc<>'' "));
            dcFooterOS = Convert.ToDecimal(dtAccountBalanc.Compute("sum(Outstanding)", "Desc<>'' "));
            FunPriSetABFooterValue();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
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
        if (ddlSLA.SelectedValue != "0")
        {
            strAccNo =
                  "  for Prime Account No  " + ddlMLA.SelectedItem.Text.Trim() +
                " and Sub Account No  " + ddlSLA.SelectedItem.Text.Trim();
        }
        else
        {
            strAccNo =
              "  for Prime Account No  " + ddlMLA.SelectedItem.Text.Trim();

        }
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
        if (ddlSLA.SelectedValue != "0")
        {
            strAccNo =
                  "  for Prime Account No  " + ddlMLA.SelectedItem.Text.Trim() +
                " and Sub Account No  " + ddlSLA.SelectedItem.Text.Trim();
        }
        else
        {
            strAccNo =
              "  for Prime Account No  " + ddlMLA.SelectedItem.Text.Trim();

        }
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
                + " With respect of you Premature Closure  Memo requested on " + txtPreclosureDate.Text.Trim() + " " + strAccNo + ", Please find the Premature Closure Memo statement in the attached file. </font> " +
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
        TxtAccStatus.Text = txtAccDate.Text = txtPrincipal.Text = txtMatureDate.Text = txtIRR.Text = "";
        txtBusinessIRR.Text = txtCompanyIRR.Text = txtFlatRate.Text = txtTenure.Text = txtMode.Text = txtFinanceCharge.Text = "";
        grvAsset.ClearGrid();
        grvAccountBalance.ClearGrid();
        //btnEmail.Enabled = btnPrint.Enabled = false;
        tbAccDetails.Enabled = tbCashFlow.Enabled = false;
        ucdCustomer.ClearCustomerDetails();
        txtPreRate.Text = txtPreAmount.Text = "";
        if (ddlPreType.Items.FindByValue("0") != null) ddlPreType.SelectedIndex = 0;
        grvCashFlow.DataSource = null;
        grvCashFlow.DataBind();
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
                Label lblDue = gvRow.FindControl("lblDue") as Label;
                Label lblReceived = gvRow.FindControl("lblReceived") as Label;
                Label lblPayable = gvRow.FindControl("lblPayable") as Label;
                HiddenField hdnCashFlowID = gvRow.FindControl("hdnCashFlowID") as HiddenField;

                strbAccountClosureDetails.Append("<Details ");
                strbAccountClosureDetails.Append(" Closure_Type_Code = '33'");
                strbAccountClosureDetails.Append(" Closure_Type = '2'");
                strbAccountClosureDetails.Append(" Closure_Status_Code = '1'");
                strbAccountClosureDetails.Append(" Closure_Status_Type_Code = '9'");
                strbAccountClosureDetails.Append(" PreClosure_Date = '" + Utility.StringToDate(txtPMCReqDate.Text) + "'");

                if (ddlSLA.Items.Count == 1 && ddlSLA.SelectedValue == "0")
                    strbAccountClosureDetails.Append(" SANum = '" + ddlMLA.SelectedValue + "DUMMY'");
                else
                    strbAccountClosureDetails.Append(" SANum = '" + ddlSLA.SelectedValue + "'");
                strbAccountClosureDetails.Append(" Cashflow_Component = '" + hdnCashFlowID.Value + "'");
                if (!string.IsNullOrEmpty(txtPreRate.Text.Trim()))
                    strbAccountClosureDetails.Append(" Closure_Rate = '" + txtPreRate.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Closure_Rate = '0'");

                if (!string.IsNullOrEmpty(lblPayable.Text.Trim()))
                    strbAccountClosureDetails.Append(" Payable_Amount = '" + lblPayable.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Payable_Amount = '0'");

                if (!string.IsNullOrEmpty(lblReceived.Text.Trim()))
                    strbAccountClosureDetails.Append(" Received_Amount = '" + lblReceived.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Received_Amount = '0'");

                if (!string.IsNullOrEmpty(txtWaived.Text.Trim()))
                    strbAccountClosureDetails.Append(" Waived_Amount = '" + txtWaived.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Waived_Amount = '0'");

                if (!string.IsNullOrEmpty(txtPreAmount.Text.Trim()))
                    strbAccountClosureDetails.Append(" Closure_Amount = '" + txtPreAmount.Text + "'");
                else
                    strbAccountClosureDetails.Append(" Closure_Amount = '0'");

                strbAccountClosureDetails.Append(" Remarks = '" + txtRemarks.Text.Trim() + "'");

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

    #region [Set ErrorMessage for control]

    public void FunPubSetErrorMessageControl()
    {
        //rfvBranch.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Branch;
        rfvMLA.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Select_PriAc;
        rfvLineOfBusiness.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LOB;
        rfvSLA.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Select_SubAc;
        rfvClsoureBy.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LoanAd_AcPreClosure_Done;
        rfvPreType.ErrorMessage = "Select the Preclosure type";
        rfvPreRate.ErrorMessage = "Enter the Preclosure rate";
        rfvPreAmount.ErrorMessage = "Preclosure amount can not be empty";
    }

    #endregion [Set ErrorMessage for control]

    #endregion [Function's]

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.Items.Count > 0) ddlLOB.SelectedIndex = 0;
            //if (ddlBranch.Items.Count > 0)
                //ddlBranch.SelectedIndex = 0;
                ddlBranch.Clear();
            if (ddlMLA.Items.Count > 0) ddlMLA.SelectedIndex = 0;
            if (ddlSLA.Items.Count > 0) ddlSLA.SelectedIndex = 0;
            txtPMCReqDate.Text = DateTime.Now.ToString(strDateFormat);
            tbgeneral.Enabled = true;
            tbAccDetails.Enabled = tbCashFlow.Enabled = false;
            ucdCustomer.ClearCustomerDetails();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }


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
        Procparam.Add("@Program_Id", "201");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue.ToString());
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }


}
