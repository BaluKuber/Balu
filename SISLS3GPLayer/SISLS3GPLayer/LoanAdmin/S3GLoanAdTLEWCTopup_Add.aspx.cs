#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: TLE / WC TopUP
/// Created By			: Kannan RC
/// Created Date		: 07-Sep-2010
/// Modified By         : Shibu 
/// Modified Date       : 28-Sep-2013
/// Purpose	            : Performance Tuning DDL to Change AutoSuggestion An Avoid Unwanted DDL Load Events
/// <Program Summary>
#endregion

#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
#endregion


public partial class LoanAdmin_S3GLoanAdTLEWCTopup_Add : ApplyThemeForProject
{

    #region Intialization
    Dictionary<string, string> Procparam = null;
    int intCompanyId;
    int intUserId;
    int intTLEWCID;
    int intErrCode;
    public string strDateFormat = string.Empty;
    public int intMaxDigit = 0;
    string strMode;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    static string Bussiness_IRR;
    static string Company_IRR;
    static string Accounting_IRR;
    string strErrorMessage = @"Correct the following validation(s):<ul><li> ";
    string strErrorMsgLast = @"</ul></li>";


    int i;
    bool status;
    string s;
    double intNewAmount = 0;
    string strKey;
    string strFILNo;
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdTLEWCTopup_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=TUP';";
    string strPageName = "Loan Admin - TEWCTop Up";
    public static LoanAdmin_S3GLoanAdTLEWCTopup_Add obj_Page;
    LoanAdminMgtServicesClient objLoanAdmin_MasterClient;

    ContractMgtServicesReference.ContractMgtServicesClient ObjSpecificRevisionClient;
    #endregion

    #region [Event's]

    #region [Page Event's]

    protected void Page_Load(object sender, EventArgs e)
    {
        // WF Initializtion 
        ProgramCode = "079";
        obj_Page = this;
        try
        {
            FunPubPageLoad();
            //gvCollateralDetails.DataSource = FunPriGetCollateralDetails();
            //gvCollateralDetails.DataBind();
            //  if (Convert.ToString(dtWorkflow.Rows[0]["Serial_Number"]) == "0") //dtWorkflow.Rows[0]["Serial_Number"].Equals("0"))
            //{
            // gvCollateralDetails.Rows[0].Visible = false;
            //}

            if (PageMode == PageModes.WorkFlow && !IsPostBack)
            {
                // PreparePageForWFLoad();
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvTLE.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad;
            cvTLE.IsValid = false;
        }
    }

    #endregion [Page Event's]

    #region [Button Event's]

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Convert.ToDecimal(txtSanctionedAmount.Text) < Convert.ToDecimal(txtCurrent.Text))
            //{
            //    cvTLE.ErrorMessage = strErrorMessage + " The current finance required should be lesser than or equal to Sanctioned Amount." + strErrorMsgLast;
            //    cvTLE.IsValid = false;
            //    return;
            //}
            if (string.IsNullOrEmpty(txtCurrent.Text.Trim()) || Convert.ToInt64(txtCurrent.Text) == 0)
            {
                if (!string.IsNullOrEmpty(txtCurrent.Text.Trim()) && Convert.ToInt64(txtCurrent.Text) == 0)
                {
                    cvTLE.ErrorMessage = strErrorMessage + " The current finance required should be greater than zero." + strErrorMsgLast;
                    cvTLE.IsValid = false;
                }
                return;
            }
            string[] str_Lobs = ddlLOB.SelectedItem.Text.Split('-');
            double doubleTotalAmount = 0;
            string strLOBCode = str_Lobs[0].ToString().Trim().ToLower();
            if (strLOBCode == "wc")  // only Working capital to insert any condition
            {
                if (gvNewStructure.Rows.Count != 0)
                {
                    doubleTotalAmount = Convert.ToDouble(((TextBox)gvNewStructure.Rows[0].FindControl("txtNewMontly")).Text);
                    if (doubleTotalAmount == Convert.ToDouble(Convert.ToDouble(hdnExistingAmount.Value) + Convert.ToDouble(txtCurrent.Text)))
                    {
                        FunPubSaveTopUp();
                    }
                    else
                    {
                        tcPLE.ActiveTabIndex = 1;
                        strAlert = strAlert.Replace("__ALERT__", "The sum of New installment amount should be equal to the sum of old installment amount and New requirement amount");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "A", strAlert, true);
                        return;
                    }
                }
                else
                {
                    cvTLE.ErrorMessage = strErrorMessage + " New Structure Details cann't be empty." + strErrorMsgLast;
                    cvTLE.IsValid = false;
                    return;
                }
            }
            else  // only Term loan Extensible to insert with the condition
            {
                if (gvNewStructure == null || (gvNewStructure != null && gvNewStructure.Rows.Count == 0))
                {
                    cvTLE.ErrorMessage = strErrorMessage + " New Structure Details cann't be empty." + strErrorMsgLast;
                    cvTLE.IsValid = false;
                    return;
                }
                string strNewInsAmount = ""; ;
                foreach (GridViewRow grv in gvNewStructure.Rows)
                {
                    strNewInsAmount = Convert.ToString(((TextBox)grv.FindControl("txtNewMontly")).Text);
                    if (strNewInsAmount == "")
                        break;
                    doubleTotalAmount += Convert.ToDouble(strNewInsAmount);
                }
                if (strNewInsAmount == "")
                {
                    cvTLE.ErrorMessage = strErrorMessage + " New Installment Amount cann't be empty." + strErrorMsgLast;
                    cvTLE.IsValid = false;
                    return;
                }
                if (doubleTotalAmount == Convert.ToDouble(Convert.ToDouble(hdnExistingAmount.Value) + Convert.ToDouble(txtCurrent.Text)))
                {
                    FunPubSaveTopUp();
                }
                else if (doubleTotalAmount != Convert.ToDouble(Convert.ToDouble(hdnExistingAmount.Value) + Convert.ToDouble(txtCurrent.Text)))
                {
                    tcPLE.ActiveTabIndex = 1;
                    strAlert = strAlert.Replace("__ALERT__", "The sum of New installment amount should be equal to the sum of old installment amount and New requirement amount");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "A", strAlert, true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvTLE.ErrorMessage = strErrorMessage + "Unable to top up" + strErrorMsgLast;
            cvTLE.IsValid = false;
        }
    }

    protected void btnCancelTopup_Click(object sender, EventArgs e)
    {
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            int intErrorcode = objLoanAdmin_MasterClient.FunPubCancelTopUpTLEWC(intTLEWCID, intCompanyId, intUserId);
            if (intErrorcode == 0)
            {
                strAlert = "TopUp Number : " + txtTLEWC.Text + " cancelled successfully";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
            }
            else if (intErrorcode == -3)
            {
                strAlert = "TopUp Number : " + txtTLEWC.Text + " cancelled failed, try again later.";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
            }
            else
            {
                string strProcess = intErrorcode == 2 ? "Under Process" : intErrorcode == 3 ? "Approved" : intErrorcode == 4 ? "Rejected" : intErrorcode == 5 ? "Cancelled" : "Error Occure";
                strAlert = "TopUp Number : " + txtTLEWC.Text + " " + strProcess + " , cannot cancelled the topup.";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "A", strAlert, true);
            strRedirectPageView = "";
        }
        catch (FaultException<LoanAdminMgtServicesReference.ClsPubFaultException> ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {
            if (objLoanAdmin_MasterClient != null)
                objLoanAdmin_MasterClient.Close();
        }
    }

    public string FunPubGetNewTopUpStructure()
    {
        string strXMLAccDetails = string.Empty;
        StringBuilder strbNewTopUpStructure = new StringBuilder();
        strbNewTopUpStructure.Append("<Root> ");
        try
        {
            foreach (GridViewRow gvr in gvNewStructure.Rows)
            {
                Label lblInsNo = gvr.FindControl("lblInstallmentNo") as Label;
                Label lblInsDate = gvr.FindControl("lblInsDate") as Label;
                Label lblRepaymentAmt = gvr.FindControl("lblRepaymentAmount") as Label;
                TextBox txtNewMthlyIns = gvr.FindControl("txtNewMontly") as TextBox;
                Label lblFinanceCharges = gvr.FindControl("lblFinanceCharges") as Label;
                Label lblNoofDays = gvr.FindControl("lblNoofDays") as Label;
                Label lblFromDate = gvr.FindControl("lblFromDate") as Label;
                Label lblToDate = gvr.FindControl("lblToDate") as Label;
                strbNewTopUpStructure.Append(" <Details ");
                strbNewTopUpStructure.Append(" PANum='" + ddlMLA.Text + "' ");
                if (ddlSLA.Items.Count == 0)
                    strbNewTopUpStructure.Append(" SANum='" + ddlMLA.Text + "DUMMY" + "'");
                else
                    strbNewTopUpStructure.Append(" SANum='" + ddlSLA.Text + "' ");
                strbNewTopUpStructure.Append(" InstallmentNo='" + lblInsNo.Text + "' ");
                strbNewTopUpStructure.Append(" InstallmentDate='" + lblInsDate.Text + "' ");
                strbNewTopUpStructure.Append(" NewInstallmentAmount='" + txtNewMthlyIns.Text + "' ");
                strbNewTopUpStructure.Append(" FinanceCharges='" + lblFinanceCharges.Text + "' ");
                strbNewTopUpStructure.Append(" PrincipalAmount='" + Convert.ToString(Convert.ToDouble(Convert.ToDouble(txtNewMthlyIns.Text) + Convert.ToDouble(lblFinanceCharges.Text))) + "' ");
                strbNewTopUpStructure.Append(" NoofDays='" + lblNoofDays.Text + "' ");
                strbNewTopUpStructure.Append(" FromDate='" + lblFromDate.Text + "' ");
                strbNewTopUpStructure.Append(" ToDate='" + lblToDate.Text + "' ");
                strbNewTopUpStructure.Append(" RepaymentAmount='" + lblRepaymentAmt.Text + "' ");
                strbNewTopUpStructure.Append(" > </Details>");
            }
            strbNewTopUpStructure.Append(" </Root>");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        return strbNewTopUpStructure.ToString();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else if (Request.QueryString["Popup"] != null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
        }
        else
        {
            Response.Redirect("../LoanAdmin/S3gLoanAdTransLander.aspx?Code=TUP");
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        FunPubClearAllControls();
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        //       string[] str_Lob = ddlLOB.SelectedItem.Text.Split('-');
        if (txtNextLiner.Text == "")
        {
            cvTLE.ErrorMessage = strErrorMessage + "Enter the Next Liner" + strErrorMsgLast;
            cvTLE.IsValid = false;
            return;
        }
        if (txtInstallmentAmount.Text == "")
        {
            cvTLE.ErrorMessage = strErrorMessage + "Enter the Installment Amount" + strErrorMsgLast;
            cvTLE.IsValid = false;
            return;
        }
        bool bolSelectStatus = false;
        foreach (GridViewRow grv in gvNewStructure.Rows)
        {
            if (((CheckBox)grv.FindControl("CbxChange")).Checked)
            {
                bolSelectStatus = true;
            }
        }
        if (!bolSelectStatus)
        {
            cvTLE.ErrorMessage = strErrorMessage + "Select the New Structure Details." + strErrorMsgLast;
            cvTLE.IsValid = false;
            return;
        }
        int intcount = 0;
        bool flag = false;
        foreach (GridViewRow gr in gvNewStructure.Rows)
        {
            CheckBox CbxSelect = (CheckBox)gr.FindControl("CbxChange");
            if (CbxSelect.Checked)
                flag = true;
            if (flag)
            {
                if (intcount < Convert.ToInt32(txtNextLiner.Text))
                {
                    TextBox txtNewMontly = (TextBox)gr.FindControl("txtNewMontly");
                    txtNewMontly.Text = txtInstallmentAmount.Text;
                }
                ++intcount;
            }
        }
    }

    protected void btnGetInterest_OnClick(object sender, EventArgs e)
    {
        try
        {
            //tpRepay.Enabled = tpCollateralDetails.Enabled = false;
            string[] str_Lob = ddlLOB.SelectedItem.Text.Split('-');
            btnSave.Enabled = false;
            if (str_Lob[0].ToString().Trim().ToLower() == "wc" && txtCurrent.Text != "" && txtCurrent.Text.Length != 0 && Convert.ToInt64(txtCurrent.Text) > 0)
            {
                if (gvNewStructure.Rows.Count > 0 && ((TextBox)gvNewStructure.Rows[0].FindControl("txtNewMontly")) != null)
                {
                    ((TextBox)gvNewStructure.Rows[0].FindControl("txtNewMontly")).Text = Convert.ToString(Convert.ToDouble(Convert.ToDouble(txtCurrent.Text) + Convert.ToDouble(hdnExistingAmount.Value)));
                    txtInstallmentAmount.Text = Convert.ToString(Convert.ToDouble(Convert.ToDouble(txtCurrent.Text) + Convert.ToDouble(hdnExistingAmount.Value)));
                    if (((Label)gvNewStructure.Rows[0].FindControl("lblRepaymentAmount")) != null)
                    {
                        string strCurrentReq = string.IsNullOrEmpty(Convert.ToString(hdnCurrentFinReq.Value)) ? "0" : hdnCurrentFinReq.Value;
                        ((Label)gvNewStructure.Rows[0].FindControl("lblRepaymentAmount")).Text = Convert.ToString(Convert.ToDouble(Convert.ToDouble(txtCurrent.Text) + Convert.ToDouble(((Label)gvNewStructure.Rows[0].FindControl("lblRepaymentAmount")).Text) - Convert.ToDouble(strCurrentReq)));
                        hdnCurrentFinReq.Value = txtCurrent.Text;
                    }
                }
                // tpRepay.Enabled = tpCollateralDetails.Enabled = true;
                if (strMode != "Q")
                    btnSave.Enabled = true;
            }
            else if (str_Lob[0].ToString().Trim().ToLower() == "te" && txtCurrent.Text != "" && txtCurrent.Text.Length != 0 && Convert.ToInt64(txtCurrent.Text) > 0)
            {
                if (gvNewStructure.Rows.Count > 0)
                {
                    if (gvNewStructure.Rows.Count == 1)
                    {
                        if (((TextBox)gvNewStructure.Rows[0].FindControl("txtNewMontly")) != null)
                        {
                            ((TextBox)gvNewStructure.Rows[0].FindControl("txtNewMontly")).Text = Convert.ToString(Convert.ToDouble(Convert.ToDouble(txtCurrent.Text) + Convert.ToDouble(hdnExistingAmount.Value)));
                            txtInstallmentAmount.Text = Convert.ToString(Convert.ToDouble(Convert.ToDouble(txtCurrent.Text) + Convert.ToDouble(hdnExistingAmount.Value)));
                            if (((Label)gvNewStructure.Rows[0].FindControl("lblRepaymentAmount")) != null)
                            {
                                string strCurrentReq1 = string.IsNullOrEmpty(Convert.ToString(hdnCurrentFinReq.Value)) ? "0" : hdnCurrentFinReq.Value;
                                ((Label)gvNewStructure.Rows[0].FindControl("lblRepaymentAmount")).Text = Convert.ToString(Convert.ToDouble(Convert.ToDouble(txtCurrent.Text) + Convert.ToDouble(((Label)gvNewStructure.Rows[0].FindControl("lblRepaymentAmount")).Text) - Convert.ToDouble(strCurrentReq1)));
                                hdnCurrentFinReq.Value = txtCurrent.Text;
                            }
                        }
                    }
                    else
                    {
                        foreach (GridViewRow gvr in gvNewStructure.Rows)
                        {
                            string strCurrentReq = string.IsNullOrEmpty(Convert.ToString(hdnCurrentFinReq.Value)) ? "0" : hdnCurrentFinReq.Value;
                            ((Label)gvr.FindControl("lblRepaymentAmount")).Text = Convert.ToString(Convert.ToDouble(Convert.ToDouble(txtCurrent.Text) + Convert.ToDouble(((Label)gvr.FindControl("lblRepaymentAmount")).Text)) - Convert.ToDouble(strCurrentReq));
                        }
                        hdnCurrentFinReq.Value = txtCurrent.Text;
                    }
                }
                // tpRepay.Enabled = tpCollateralDetails.Enabled = true;
                if (strMode != "Q")
                    btnSave.Enabled = true;
            }
            if (strMode == "M" && (txtTopupStatus.Text == "Approved" || txtTopupStatus.Text == "Rejected" || txtTopupStatus.Text == "Cancelled"))
            { btnSave.Enabled = false; }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvTLE.ErrorMessage = ex.Message;
            cvTLE.IsValid = false;
        }
    }

    #endregion [Button Event's]

    #region [Drop down Event's]

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedIndex > 0)
                FunPriGetLookupLocationList();
            else
            {
                //ddlBranch.Items.Clear();
                //ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            }
            FunPubLOBClearControls();
            txtNextLiner.ReadOnly = txtInstallmentAmount.ReadOnly = tpRepay.Enabled = tpCollateralDetails.Enabled = false;
            btnGO.Enabled = true;
            string[] str_Lob = ddlLOB.SelectedItem.Text.Split('-');
            if (str_Lob[0].ToString().Trim().ToLower() == "wc")
            {
                txtNextLiner.ReadOnly = txtInstallmentAmount.ReadOnly = true; btnGO.Enabled = false;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvTLE.ErrorMessage = ex.Message;
            cvTLE.IsValid = false;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubBranchClearControls();
            tpRepay.Enabled = tpCollateralDetails.Enabled = false;
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                FunPriGetMLAList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue));
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvTLE.ErrorMessage = ex.Message;
            cvTLE.IsValid = false;
        }
    }

    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSLA.Enabled = true;
            FunPubMLAClearControls();
            tpRepay.Enabled = tpCollateralDetails.Enabled = false;
            if (ddlMLA.SelectedValue != "0")
            {
                FunPriGetCustomerDetails(ddlMLA.SelectedItem.Text);
                //FunPriGetDate(ddlMLA.SelectedItem.Text);
                // FunPriGetCollateral(ddlMLA.SelectedItem.Text);
                if ((Convert.ToInt32(ddlLOB.SelectedValue) > 0) && (Convert.ToInt32(ddlBranch.SelectedValue) > 0))
                {
                    FunPriGetSLAList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), ddlMLA.SelectedItem.Text);
                    if (ddlSLA.Items.Count == 0)
                    {
                        ddlSLA.Enabled = false;
                        tpRepay.Enabled = tpCollateralDetails.Enabled = true;
                        FunPriGetExistingDetails(ddlMLA.SelectedItem.Text, "");
                        FunPriGetSanctionedAmount(ddlMLA.SelectedItem.Text, "");
                    }
                }
                if (FunPriGetCollateral("") == false)
                {
                    FunPubMLAClearControls();

                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvTLE.ErrorMessage = ex.Message;
            cvTLE.IsValid = false;
        }
    }

    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubSLAClearControls();
            tpRepay.Enabled = tpCollateralDetails.Enabled = false;
            if (ddlSLA.SelectedValue != "0")
            {
                tpRepay.Enabled = tpCollateralDetails.Enabled = true;
                FunPriGetExistingDetails(ddlMLA.SelectedItem.Text, ddlSLA.Text);
                FunPriGetSanctionedAmount(ddlMLA.SelectedItem.Text, ddlSLA.Text);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvTLE.ErrorMessage = ex.Message;
            cvTLE.IsValid = false;
        }
    }

    #endregion [Drop Down Event's]

    #region [Grid Event's]

    protected void gvNewStructure_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string[] str_Lob = ddlLOB.SelectedItem.Text.Split('-');
        if (strMode == "Q")
            e.Row.Cells[0].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtNewAmount = e.Row.FindControl("txtNewMontly") as TextBox;
            if ((intTLEWCID > 0) && (strMode == "Q"))
            {
                ((CheckBox)e.Row.FindControl("CbxChange")).Checked = false;
            }
            if (Convert.ToString(str_Lob[0].Trim().ToLower()) == "wc")
            {
                ((CheckBox)e.Row.FindControl("CbxChange")).Checked = false;
            }
            txtNewAmount.ReadOnly = (strMode == "M" && (txtTopupStatus.Text == "Approved" || txtTopupStatus.Text == "Rejected" || txtTopupStatus.Text == "Cancelled")) ? false : true;
            txtNewAmount.ReadOnly = strMode != "Q" ? false : true;
            intNewAmount += Convert.ToDouble(txtNewAmount.Text);
            if (intMaxDigit >= 10 && (strMode != "Q"))
                txtNewAmount.MaxLength = 10;
            else if (strMode != "Q")
                txtNewAmount.MaxLength = intMaxDigit;
        }
    }

    #endregion [Grid Event's]

    #region [Check Box Event's]

    protected void CbxChange_CheckedChanged(object sender, EventArgs e)
    {
        //Int64 intLOB_ID = 0;
        CheckBox chkLOB = null;
        GridViewRow gvr = ((GridViewRow)((CheckBox)sender).Parent.Parent);
        int intCount = 0;
        foreach (GridViewRow gvNew in gvNewStructure.Rows)
        {
            chkLOB = ((CheckBox)gvNew.FindControl("CbxChange"));
            //if (chkLOB.Checked)
            //{
            //    intLOB_ID = Convert.ToInt64(((Label)gvNew.FindControl("lblRepaymentAmount")).Text);
            //}
            if (intCount != gvr.RowIndex)
                chkLOB.Checked = false;
            intCount++;
        }

        rvNextLiner.MaximumValue = Convert.ToString(intCount - gvr.RowIndex);
        txtNextLiner.MaxLength = rvNextLiner.MaximumValue.Length;
        //if (intLOB_ID == 0)
        //{
        //    foreach (GridViewRow gvNew in gvNewStructure.Rows)
        //    {
        //        chkLOB = ((CheckBox)gvNew.FindControl("CbxChange"));
        //        chkLOB.Enabled = true;
        //    }
        //}
    }

    #endregion [Check Box Event's]

    #endregion [Event's]

    #region [Function's]

    public void FunPubPageLoad()
    {
        try
        {
            //if (Request.QueryString["Popup"] != null)  //transaction screen page load
            //    btnCancel.Enabled = false;
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                intTLEWCID = Convert.ToInt32(fromTicket.Name);
                strMode = Request.QueryString["qsMode"];
            }

            UserInfo ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            txtDate.Attributes.Add("readonly", "readonly");
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            intMaxDigit = ObjS3GSession.ProGpsPrefixRW;
            //cexDate.Format = strDateFormat;
            //cexDate.SelectedDate = DateTime.Today;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if (!IsPostBack)
            {
                rvNextLiner.MaximumValue = Convert.ToString(1);
                rvNextLiner.MinimumValue = "1";
                txtDate.Text = DateTime.Today.ToString(strDateFormat);
              //  FunPriGetLookUpLOBList();
                FunPriGetLookUpList();

                if (Request.QueryString["qsMode"] == "C")
                    FunPriGetLookUpLOBList();

                if (intMaxDigit >= 10 && (strMode != "Q"))
                {
                    
                    txtInstallmentAmount.MaxLength = 10;
                   // txtSanctionedAmount.MaxLength = txtCurrent.MaxLength = 10; //Changed by Tamilselvan.S on 1/11/2011
                    // txtCurrent.Attributes.Add("onblur", "fnLoadCustomer()");
                }
                else if (strMode != "Q")
                {
                    txtInstallmentAmount.MaxLength = intMaxDigit;
                    //txtSanctionedAmount.MaxLength = txtCurrent.MaxLength = intMaxDigit; //Changed by Tamilselvan.S on 1/11/2011
                    //txtCurrent.Attributes.Add("onblur", "fnLoadCustomer()");

                }
              
                int intGPSPrefix = ObjS3GSession.ProGpsPrefixRW <= 10 ? ObjS3GSession.ProGpsPrefixRW : 10;
                int intGPSSuffix = ObjS3GSession.ProGpsSuffixRW <= 3 ? ObjS3GSession.ProGpsSuffixRW : 3;
               // txtSanctionedAmount.SetDecimalPrefixSuffix(intGPSPrefix, intGPSSuffix, true, "Sanctioned Amount ");

                if ((intTLEWCID > 0) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                    // txtCurrent.Attributes.Add("onblur", "fnLoadCustomer()");
                }
                else if ((intTLEWCID > 0) && (strMode == "Q"))
                {
                    FunPriDisableControls(-1);
                    btnSave.Enabled = false;
                }
                else
                {
                    FunPriDisableControls(0);
                    tpRepay.Enabled = tpCollateralDetails.Enabled = false;
                    btnSave.Enabled = false;
                    //txtCurrent.Attributes.Add("onblur", "fnLoadCustomer()");
                }
                if (strMode == "M" && (txtTopupStatus.Text == "Approved" || txtTopupStatus.Text == "Rejected" || txtTopupStatus.Text == "Cancelled"))
                {
                    foreach (GridViewRow gvr in gvNewStructure.Rows)
                    {
                        TextBox txtamount = (TextBox)gvr.FindControl("txtNewMontly");
                        txtamount.ReadOnly = true;
                    }
                    btnSave.Enabled = false;
                }
                if (!IsPostBack)
                    FunPriGetCollateral("");
            }

            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #region  DateFormat
    /// <summary>
    /// Created by Tamilselvan.S
    /// Created Date 10/02/2011
    /// </summary>
    /// <param name="strDate"></param>
    /// <returns></returns>
    public string FormatDate(string strDate)
    {
        return DateTime.Parse(strDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    }

    #endregion

    #region [Dropdown Fill LOB and Branch]

    private void FunPriGetLookUpLOBList()
    {
        try
        {
          
            Procparam = new Dictionary<string, string>();
            if (intTLEWCID == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_Id", intUserId.ToString());
            Procparam.Add("@Program_Id", "79");
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            ddlLOB.BindDataTable("S3G_LOANAD_GetTLEWCLOBList", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //  Removed By Shibu DDL Control Changed Autosuggestion  
            ////ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
            //ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            //ddlBranch.AddItemToolTip();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetLookupLocationList()
    {
        try
        {//Removed By Shibu DDL Control Changed Autosuggestion 
            //Procparam = new Dictionary<string, string>();
            //if (intTLEWCID == 0)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            //Procparam.Add("@User_Id", intUserId.ToString());
            //Procparam.Add("@Program_Id", "79");
            //Procparam.Add("@Company_ID", intCompanyId.ToString());
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
            //ddlBranch.AddItemToolTip();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion [Dropdown Fill LOB and Branch]

    #region [Dropdown Fill TopupStatus and Collateral]

    private void FunPriGetLookUpList()
    {
        try
        {
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", intCompanyId.ToString());
            //Procparam.Add("@LookupType_Code", "9");
            //// ddlTopupStatus.BindGlobalDataTable("S3G_LOANAD_GetLookUpValues", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@LookupType_Code", "18");
            ddlCollateral.BindGlobalDataTable("S3G_LOANAD_GetLookUpValues", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion [Dropdown Fill TopupStatus and Collateral]

    #region [Clear Controls]

    public void FunPubLOBClearControls()
    {
        ucdCustomer.ClearCustomerDetails();
        txtCustID.Text = txtSanctionedAmount.Text = txtExistingFinanceAmount.Text = txtCurrent.Text = hdnCurrentFinReq.Value = "";//Changed by Tamilselvan.S on 1/11/2011
        ddlBranch.Clear();
        ddlMLA.Items.Clear();
        ddlSLA.Items.Clear();
        ddlSLA.Enabled = true;

        txtAccDate.Text = "";
        txtInstallmentAmount.Text = txtNextLiner.Text = "";
    }

    public void FunPubBranchClearControls()
    {
        ucdCustomer.ClearCustomerDetails();
        txtCustID.Text = txtSanctionedAmount.Text = txtExistingFinanceAmount.Text = txtCurrent.Text = hdnCurrentFinReq.Value = "";//Changed by Tamilselvan.S on 1/11/2011
        ddlMLA.Items.Clear();
        ddlSLA.Items.Clear();
        ddlSLA.Enabled = true;
        txtAccDate.Text = "";
        txtInstallmentAmount.Text = txtNextLiner.Text = "";
    }

    public void FunPubMLAClearControls()
    {
        ucdCustomer.ClearCustomerDetails();
        txtCustID.Text = txtSanctionedAmount.Text = txtExistingFinanceAmount.Text = txtCurrent.Text = hdnCurrentFinReq.Value = "";//Changed by Tamilselvan.S on 1/11/2011
        ddlSLA.Items.Clear();
        ddlSLA.Enabled = true;

        txtAccDate.Text = "";
        txtInstallmentAmount.Text = txtNextLiner.Text = "";
        gvExisting.DataSource = gvNewStructure.DataSource = null;
        gvExisting.DataBind();
        gvNewStructure.DataBind();
    }

    public void FunPubSLAClearControls()
    {
        txtInstallmentAmount.Text = txtNextLiner.Text = txtExistingFinanceAmount.Text = txtSanctionedAmount.Text = txtCurrent.Text = hdnCurrentFinReq.Value = "";//Changed by Tamilselvan.S on 1/11/2011
        Panel2.Visible = Panel4.Visible = false;

        gvExisting.DataSource = gvNewStructure.DataSource = null;
        gvExisting.DataBind();
        gvNewStructure.DataBind();
    }

    public void FunPubClearAllControls()
    {
        ddlLOB.SelectedIndex = 0;
        FunPubLOBClearControls();
        //ddlTopupStatus.SelectedIndex = 0;
        tpRepay.Enabled = tpCollateralDetails.Enabled = false;
        ddlCollateral.SelectedIndex = 0;
    }

    #endregion [Clear Controls]

    private void FunPriGetMLAList(int intLOBID, int intBranchID)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            Procparam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@User_ID ", Convert.ToString(intUserId));
            Procparam.Add("@Process_Mode", strMode == null ? "C" : strMode);
            ddlMLA.BindDataTable("S3G_LOANAD_GetMLANOTopUp", Procparam, new string[] { "PANum", "PANum" });
            DataTable dtMLA = (DataTable)ddlMLA.DataSource;
            if (dtMLA != null && dtMLA.Rows.Count == 1)
            {
                ddlMLA.SelectedIndex = 1;
                FunPriGetCustomerDetails(ddlMLA.SelectedItem.Text);
                if (FunPriGetCollateral(ddlMLA.SelectedItem.Text) == true)
                {
                    if ((Convert.ToInt32(ddlLOB.SelectedValue) > 0) && (Convert.ToInt32(ddlBranch.SelectedValue) > 0))
                    {
                        FunPriGetSLAList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), ddlMLA.SelectedItem.Text);
                        if (ddlSLA.Items.Count == 0)
                        {
                            tpRepay.Enabled = tpCollateralDetails.Enabled = true;
                            FunPriGetExistingDetails(ddlMLA.SelectedItem.Text, "");
                            FunPriGetSanctionedAmount(ddlMLA.SelectedItem.Text, "");
                        }
                    }
                }
                else
                {
                    FunPubMLAClearControls();
                    ddlMLA.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #region [Get Customer Details]

    private void FunPriGetCustomerDetails(string strMLAID)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@PANum", ddlMLA.SelectedItem.Text);
            DataTable dtCustomerDetails = Utility.GetDefaultData("S3G_LOANAD_GetTLEWCCustomerDetails", Procparam);
            if (dtCustomerDetails.Rows.Count > 0)
            {
                DataRow dtRow = dtCustomerDetails.Rows[0];
                ucdCustomer.SetCustomerDetails(dtRow, true);
                txtCustID.Text = dtRow["Customer_ID"].ToString();
                //Modified by Tamilselvan.S on 15/02/2011
                txtHDDate.Text = txtAccDate.Text = Utility.StringToDate(dtRow["Creation_Date"].ToString()).ToString(strDateFormat);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }

    }

    #endregion [Get Customer Details]

    private bool FunPriGetCollateral(string strMLAID)
    {
        int intRecordsCount = 0;
        bool bolIsEvaluated = true;
        try
        {
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", intCompanyId.ToString());
            //Procparam.Add("@PANum", ddlMLA.SelectedItem.Text);
            //DataTable dtCollateralDetails = Utility.GetDefaultData("S3G_LOANAD_GetCollateralExits", Procparam);
            //if (dtCollateralDetails.Rows.Count > 0)
            //{
            //    ddlCollateral.SelectedIndex = 1;
            //}
            //else
            //{
            //    ddlCollateral.SelectedIndex = 2;
            //}

            //if (ddlCollateral.SelectedIndex == 2 && ddlLOB.SelectedValue == "6")
            //{
            //    //tpRepay.Enabled = false;
            //}
            //else
            //{
            //   // tpRepay.Enabled = true;
            //}

            //if (ddlCollateral.SelectedIndex == 1 && ddlLOB.SelectedValue == "6")
            //{
            //    //   tpRepay.Enabled = false;
            //}
            //else
            //{
            //   // tpRepay.Enabled = true;
            //}

            gvCollateralDetails.DataSource = FunPriGetCollateralDetails(out intRecordsCount, out bolIsEvaluated);
            gvCollateralDetails.DataBind();
            if (intRecordsCount == 0)
                gvCollateralDetails.Rows[0].Visible = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        return bolIsEvaluated;
    }

    private DataTable FunPriGetCollateralDetails(out int intRecordsCount, out  bool bolIsEvaluated)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Customer_ID", txtCustID.Text.ToString());
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
        Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
        DataSet dsSLA = Utility.GetDataset("S3G_CLT_ExistingCollateralCombinedList", Procparam);
        bolIsEvaluated = true;
        DataTable dtCollateralDet = new DataTable();
        dtCollateralDet = dsSLA.Tables[0];
        intRecordsCount = dtCollateralDet.Rows.Count;
        if (dtCollateralDet.Rows.Count == 0)
        {
            /*Method called To load ddlcollateral - Kuppu - bugid - 6509*/
            FunPriGetLookUpList();
            ddlCollateral.SelectedValue = "2";
            chklCollateralExists.SelectedValue = "No";
            DataRow dr = dtCollateralDet.NewRow();
            dr["CollateralRefNo"] = "1";
            dr["CollateralDescription"] = "";
            dr["CollateralValue"] = 0;
            dr["Margin"] = 0;
            dr["SanctionedAmount"] = 0;
            dtCollateralDet.Rows.Add(dr);
        }
        else
        {
            /*Method called To load ddlcollateral - Kuppu - bugid - 6509*/
            FunPriGetLookUpList();
            ddlCollateral.SelectedValue = "1";
            chklCollateralExists.SelectedValue = "Yes";
            if (strMode != "Q")
            {
                DataRow[] dr = dtCollateralDet.Select("CollateralValue IS NULL");
                if (dr != null && dr.Length > 0)
                {
                    bolIsEvaluated = false;
                    strAlert = strAlert.Replace("__ALERT__", "The customer collateral details not evaluated.");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    ddlCollateral.SelectedIndex = 0;
                    chklCollateralExists.ClearSelection();
                }
            }
        }
        return dtCollateralDet;
    }

    private void FunPriGetSLAList(int intLOBID, int intBranchID, string strMLAID)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@PANum", Convert.ToString(ddlMLA.SelectedItem.Text));
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            //Procparam.Add("@Branch_ID ", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@User_ID", intUserId.ToString());
            DataSet dsSLA = Utility.GetDataset("S3G_LOANAD_GetSLANOTopUp", Procparam);
            ddlSLA.DataTextField = ddlSLA.DataValueField = "SANUM";

            ddlSLA.DataSource = dsSLA.Tables[0];
            ddlSLA.DataBind();
            if (dsSLA != null && dsSLA.Tables[0].Rows.Count > 0 && dsSLA.Tables[1].Rows.Count > 0 && Convert.ToInt32(dsSLA.Tables[1].Rows[0]["RecCount"]) > 0)
            {
                ddlSLA.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            }
            if (dsSLA.Tables[0].Rows.Count == 1)
            {
                tpRepay.Enabled = tpCollateralDetails.Enabled = true;
                ddlSLA.SelectedIndex = 1;
                FunPriGetExistingDetails(ddlMLA.SelectedItem.Text, ddlSLA.SelectedItem.Text);
                FunPriGetSanctionedAmount(ddlMLA.SelectedItem.Text, ddlSLA.SelectedItem.Text);
            }
            else if (dsSLA.Tables[0].Rows.Count > 1)
            {
                tpRepay.Enabled = tpCollateralDetails.Enabled = false;
            }
            //if (dtSLA != null && dtSLA.Rows.Count > 0)
            //{
            //    if (dtSLA.Rows.Count > 1 && Convert.ToString(dtSLA.Rows[0]["SANum"]) != Convert.ToString(ddlMLA.SelectedItem.Text) + "DUMMY")
            //    {
            //        ddlSLA.DataSource = dtSLA;
            //        ddlSLA.DataBind();
            //        ddlSLA.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            //    }
            //}
            //else if (dtSLA != null && dtSLA.Rows.Count == 0)
            //{
            //    ddlSLA.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetExistingDetails(string strMLAID, string strSLAID)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@PANum", ddlMLA.SelectedItem.Text);
        if (ddlSLA.Items.Count == 0)
            Procparam.Add("@SANum", ddlMLA.SelectedItem.Text + "DUMMY");  // Convert.ToString(ViewState["dum"])
        else
            Procparam.Add("@SANum", ddlSLA.SelectedItem.ToString());
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
        Procparam.Add("@Mode", strMode == null ? "C" : strMode);
        DataSet dsExistDetails = Utility.GetDataset("S3G_LOANAD_GetExistingDetails", Procparam);
        DataTable dtDetailsNew = new DataTable();
        if (dsExistDetails.Tables.Count > 1 && dsExistDetails.Tables[0].Rows.Count == 0)
        {
            tpRepay.Enabled = tpCollateralDetails.Enabled = false;
            if (strSLAID == "")
            {
                strAlert = strAlert.Replace("__ALERT__", "The Line of business " + ddlLOB.SelectedItem.Text + " with Account Number " + ddlMLA.SelectedItem.Text + " having no Repayment Structure, Cannot Topup");
                ddlMLA.SelectedIndex = 0;
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "The Line of business " + ddlLOB.SelectedItem.Text + " with Account Number " + ddlSLA.SelectedItem.Text + " having no Repayment Structure, Cannot Topup");
                ddlSLA.SelectedIndex = 0;
            }
            txtCustID.Text = txtSanctionedAmount.Text = txtExistingFinanceAmount.Text = txtCurrent.Text = txtAccDate.Text = "";//Changed by Tamilselvan.S on 1/11/2011
            ucdCustomer.ClearCustomerDetails();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "A", strAlert, true);
            return;
        }
        if (dsExistDetails.Tables.Count > 1 && dsExistDetails.Tables[1].Rows.Count > 0 && strMode != "M" && strMode != "Q")
        {
            if (dsExistDetails.Tables[1].Columns.Count == 1 && (Convert.ToString(dsExistDetails.Tables[1].Rows[0]["ROI"]) == "RRA" && Convert.ToInt32(dsExistDetails.Tables[0].Rows[0]["Mode"].ToString())<4))
            {
                if (strSLAID == "")
                {
                    strAlert = strAlert.Replace("__ALERT__", "The Line of business " + ddlLOB.SelectedItem.Text + " with Account Number " + ddlMLA.SelectedItem.Text + " having ROI Rule is RRA without Product method, Cannot Topup");
                    ddlMLA.SelectedIndex = 0;
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "The Line of business " + ddlLOB.SelectedItem.Text + " with Account Number " + ddlSLA.SelectedItem.Text + " having ROI Rule is RRA without Product method, Cannot Topup");
                    ddlSLA.SelectedIndex = 0;
                }
                tpRepay.Enabled = tpCollateralDetails.Enabled = false;
                txtCustID.Text = txtSanctionedAmount.Text = txtExistingFinanceAmount.Text = txtCurrent.Text = txtAccDate.Text = "";//Changed by Tamilselvan.S on 1/11/2011
                ScriptManager.RegisterStartupScript(this, this.GetType(), "A", strAlert, true);
                ucdCustomer.ClearCustomerDetails();
                return;
            }
        }
        if (dsExistDetails.Tables.Count > 0)
        {
            var varInsAmount = from row in dsExistDetails.Tables[0].AsEnumerable()
                               select row.Field<decimal>("Installment_Amount");
            hdnExistingAmount.Value = Convert.ToDouble(varInsAmount.Sum()).ToString();
            txtExistingFinanceAmount.Text = dsExistDetails.Tables[0].Rows[0]["Finance_Amount"].ToString(); //Added by Tamilselvan.S on 1/11/2011
            gvExisting.DataSource = dsExistDetails.Tables[0];
            gvExisting.DataBind();
            if (dsExistDetails.Tables[0].Rows[0]["ModeType"].ToString().StartsWith("Pro"))
            {
                tpRepay.Enabled = false;
                ViewState["ModeType"] = dsExistDetails.Tables[0].Rows[0]["ModeType"].ToString();
            }
        }


        if (dsExistDetails.Tables.Count == 2 && dsExistDetails.Tables[1].Columns.Count != 1 && dsExistDetails.Tables[1].Rows.Count > 0)
        {
            rvNextLiner.MinimumValue = "1";
            rvNextLiner.MaximumValue = Convert.ToString(dsExistDetails.Tables[1].Rows[dsExistDetails.Tables[1].Rows.Count - 1]["InstallmentNo"]);
            txtNextLiner.MaxLength = Convert.ToInt32(rvNextLiner.MaximumValue.Length);
            gvNewStructure.DataSource = dsExistDetails.Tables[1];
            gvNewStructure.DataBind();
            ViewState["dtDetailsNew"] = dsExistDetails.Tables[1];
        }
        else
        {
            DateTime dtFinStDate = DateTime.Now;
            if (DateTime.Now.Month > Convert.ToInt32(ConfigurationSettings.AppSettings["StartMonth"].ToString()))
            {
                dtFinStDate = Convert.ToDateTime(ConfigurationSettings.AppSettings["StartMonth"].ToString() + "/" + "01/" + DateTime.Now.Year.ToString());
            }
            else
            {
                dtFinStDate = Convert.ToDateTime(ConfigurationSettings.AppSettings["StartMonth"].ToString() + "/" + "01/" + ((DateTime.Now.Year) - 1).ToString());
            }
            DataRow[] drDetailsNew = dsExistDetails.Tables[0].Select("PaidStatus <> 'Paid' AND (BillStatus IS NULL OR BillStatus=0) AND [Installment_Date]>='" + dtFinStDate + "'");
            if (drDetailsNew.Count() > 0)
                dtDetailsNew = drDetailsNew.CopyToDataTable();
            else
                dtDetailsNew = dsExistDetails.Tables[0];//dsExistDetails.Tables[0].Clone();

            //ViewState["dtDetailsNew"] = dtDetailsNew;
            //gvNewStructure.DataSource = dtDetailsNew;
            if (dsExistDetails.Tables[0].Rows.Count > 0)
            {
                rvNextLiner.MinimumValue = "1";

                rvNextLiner.MaximumValue = Convert.ToString(dsExistDetails.Tables[0].Rows[dsExistDetails.Tables[0].Rows.Count - 1]["InstallmentNo"]);
            }

            txtNextLiner.MaxLength = Convert.ToInt32(rvNextLiner.MaximumValue.Length);
            ViewState["dtDetailsNew"] = dtDetailsNew;// dsExistDetails.Tables[0];
            gvNewStructure.DataSource = dtDetailsNew;//dsExistDetails.Tables[0]; //modified by tamilselvan.s on 11/01/2011
            gvNewStructure.DataBind();
        }

        //gvNewStructure.DataSource = dtDetailsNew;    
        ///this is correct one the above is just sample not correct its come paid and unpaid
        if (dsExistDetails.Tables[0].Rows.Count > 0)
        {
            Panel2.Visible = true;
            Panel4.Visible = true;
            string[] str_Lob = ddlLOB.SelectedItem.Text.Split('-');
            if (str_Lob[0].ToString().Trim().ToLower() == "wc")
            {
                txtNextLiner.Text = "";
                txtNextLiner.ReadOnly = true;
            }
            else
            {
                txtNextLiner.Text = "";
                txtNextLiner.ReadOnly = false;
            }
        }
        else
        {
            Panel2.Visible = false;
            Panel4.Visible = false;
            //string[] str_Lob = ddlLOB.SelectedItem.Text.Split('-');
            //if (str_Lob[0].ToString().Trim().ToLower() != "wc")
            //{
            //    cvTLE.ErrorMessage = "Repayment Details not availble for selected prime account number";
            //    cvTLE.IsValid = false;
            //    return;
            //}
        }
    }

    private void FunPriGetSanctionedAmount(string strMLAID, string strSLAID)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@PANum", ddlMLA.SelectedItem.Text);
        if (ddlSLA.Items.Count == 0)
            Procparam.Add("@SANum", ddlMLA.SelectedItem.Text + "DUMMY");  // Convert.ToString(ViewState["dum"])
        else
            Procparam.Add("@SANum", ddlSLA.SelectedItem.ToString());
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());


        DataTable dtSanctioned = new DataTable();
        dtSanctioned = Utility.GetDefaultData("S3G_LOANAD_GetSanctionedAmount", Procparam);
        if (dtSanctioned != null)
        {
            if (dtSanctioned.Rows.Count > 0)
            {
                txtSanctionedAmount.Text = dtSanctioned.Rows[0]["Finance_Amount"].ToString();
            }
        }
    }

    public void FunPriRepayType()
    {
        try
        {
            string strType;
            string stroption;
            RepaymentType rePayType = new RepaymentType();
            strType = ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim();
            switch (strType.ToLower())
            {
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
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected string FunProFormXML()
    {
        string strXMLAccDetails = string.Empty;
        StringBuilder strbSysJournal = new StringBuilder();
        strbSysJournal.Append("<Root> ");
        int counter = 1;
        int iCounter = 1;
        int iCurAmt;
        int iToPeriod = 0;
        int iPrvAmt = 0;
        int iFromPeriod = 0;
        int totnoofrec = 0;
        string strInstallPeriod = "";
        //DataTable dt_Details = new DataTable();
        //dt_Details = (DataTable)ViewState["dtDetailsNew"];
        //dt_Details = InstallPer(dt_Details);
        try
        {
            totnoofrec = gvNewStructure.Rows.Count;
            foreach (GridViewRow grvData in gvNewStructure.Rows)
            {
                DataTable dt_Details = new DataTable();
                dt_Details = (DataTable)ViewState["dtDetailsNew"];
                string strInstallDate = ((Label)grvData.FindControl("lblInsDate")).Text;
                string strInstallAmount = ((TextBox)grvData.FindControl("txtNewMontly")).Text;
                string strInstallno = dt_Details.Rows[0]["Installment_Number"].ToString();
                string strCompanyID = Convert.ToString(intCompanyId);

                if (iCounter == counter)
                {
                    int i = 1;
                    foreach (GridViewRow grvData1 in gvNewStructure.Rows)
                    {
                        if (iCounter == 1)
                        {
                            iCurAmt = Convert.ToInt32(((TextBox)grvData1.FindControl("txtNewMontly")).Text);
                            iPrvAmt = iCurAmt;
                            iFromPeriod = 1;
                            iToPeriod = 1;
                        }
                        else
                        {
                            if (iCounter == i)
                            {
                                iCurAmt = Convert.ToInt32(((TextBox)grvData1.FindControl("txtNewMontly")).Text);
                                if (iCurAmt != iPrvAmt)
                                {
                                    iToPeriod = iCounter - 1;
                                    strInstallPeriod = Convert.ToString(iFromPeriod) + "-" + Convert.ToString(iToPeriod);
                                    iFromPeriod = iCounter;
                                    iPrvAmt = iCurAmt;
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
                    if (totnoofrec == iCounter - 1)
                    {
                        strInstallPeriod = Convert.ToString(iFromPeriod) + "-" + Convert.ToString(iToPeriod);
                    }
                }
            //string strInstallPeriod = dt_Details.Rows[0]["Installment_Period"].ToString();
            L1: string strRepayID = dt_Details.Rows[0]["RepayID"].ToString();
                string strBackup = dt_Details.Rows[0]["BreakUp_Percentage"].ToString();

                strbSysJournal.Append(" <Details ");
                strbSysJournal.Append(" PANum='" + ddlMLA.SelectedItem.Text + "' ");
                if (ddlSLA.Items.Count == 0)
                {
                    strbSysJournal.Append(" SANum='" + Convert.ToString(ViewState["dum"]) + "'");

                }
                else
                {
                    strbSysJournal.Append(" SANum='" + ddlSLA.SelectedItem.Text + "' ");
                }
                strbSysJournal.Append(" Company_ID='" + intCompanyId + "' ");
                strbSysJournal.Append(" Repay_ID='" + strRepayID.ToString() + "' ");

                strbSysJournal.Append(" Installment_Period='" + strInstallPeriod.ToString() + "' ");
                strbSysJournal.Append(" Installment_Date='" + strInstallDate.ToString() + "' ");
                strbSysJournal.Append(" Installment_Number ='" + counter.ToString() + "' "); // check this and change it.
                strbSysJournal.Append(" Installment_Amount='" + strInstallAmount.ToString() + "' "); // check this and change it.
                strbSysJournal.Append(" BreakUp_Percentage='" + strBackup.ToString() + "' "); // check this and change it.
                strbSysJournal.Append(" /> ");
                ++counter;
            }
            strbSysJournal.Append(" </Root>");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        return strbSysJournal.ToString();
    }

    // private void FunPriGetModifyTLEWC()
    private void FunPriGetModifyTLEWC(string strTopUpStatus)
    {
        try
        {
            //Dictionary<string, string> dictParam = new Dictionary<string, string>();
            //dictParam.Add("@TLEWCTopup_ID", intTLEWCID.ToString());
            //dictParam.Add("@Company_ID", intCompanyId.ToString());
            //DataTable dtDeliveryDetails = Utility.GetDefaultData("S3G_LOANAD_GetTLEWCTopUp", dictParam);
            //DataRow dtRow = dtDeliveryDetails.Rows[0];
            //txtStatus.Text = dtDeliveryDetails.Rows[0]["Topup_Status_Code"].ToString();
            //modified by Tamilselvan.S on 21/02/2011 
            txtStatus.Text = strTopUpStatus;
            if (strTopUpStatus == "3") //txtStatus.Text == "3")
            {
                gvNewStructure.Columns[0].Visible = false;
                strAlert = strAlert.Replace("__ALERT__", "TopUp details has been Approved cannot be modified");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else if (strTopUpStatus == "4")  //txtStatus.Text == "4")
            {
                gvNewStructure.Columns[0].Visible = false;
                strAlert = strAlert.Replace("__ALERT__", "TopUp details has been Rejected cannot be modified");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else if (strTopUpStatus == "5")  //txtStatus.Text == "5")
            {
                gvNewStructure.Columns[0].Visible = false;
                strAlert = strAlert.Replace("__ALERT__", "TopUp details has been Cancelled cannot be modified");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetTLEWC()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@TLEWCTopup_ID", intTLEWCID.ToString());
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtDeliveryDetails = Utility.GetDefaultData("S3G_LOANAD_GetTLEWCTopUp", dictParam);
            DataRow dtRow = dtDeliveryDetails.Rows[0];
            ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dtDeliveryDetails.Rows[0]["LOB_Name"].ToString(), dtDeliveryDetails.Rows[0]["LOB_ID"].ToString()));
            ddlLOB.ToolTip = dtDeliveryDetails.Rows[0]["LOB_Name"].ToString();
           // FunPriGetLookupLocationList();
            string[] strLobarr = ddlLOB.SelectedItem.Text.Split('-');
            if (strLobarr[0].ToString().Trim().ToLower() == "wc")
            {
                // txtCurrent.Attributes.Add("onblur", "fnLoadCustomer()");
                txtNextLiner.ReadOnly = txtInstallmentAmount.ReadOnly = true; btnGO.Enabled = false;
            }
           
            ddlBranch.SelectedText = dtDeliveryDetails.Rows[0]["Location_Name"].ToString();
            ddlBranch.SelectedValue = dtDeliveryDetails.Rows[0]["Location_ID"].ToString();
            ddlBranch.ToolTip = dtDeliveryDetails.Rows[0]["Location_Name"].ToString();
            if (btnSave.Enabled == false)
                ddlBranch.ReadOnly = true;
            //Removed By Shibu Unwanted to Load DDLMLA  Control
            //FunPriGetMLAList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue));

            ddlMLA.Items.Add(new System.Web.UI.WebControls.ListItem(dtDeliveryDetails.Rows[0]["PANum"].ToString(), dtDeliveryDetails.Rows[0]["PANum"].ToString()));
            ddlMLA.ToolTip = dtDeliveryDetails.Rows[0]["PANum"].ToString();

            //Removed By Shibu Unwanted to Load ddlSLA  Control
            //FunPriGetSLAList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), ddlMLA.SelectedItem.Text);

            if (dtDeliveryDetails.Rows[0]["SANum"].ToString().Contains("DUMMY"))
            {
                lblSLA.CssClass = "styleReqFieldLabel";
                rfvSLA.Enabled = true;                
                //ddlSLA.SelectedItem.Text = dtDeliveryDetails.Rows[0]["SANum"].ToString();
                ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem(dtDeliveryDetails.Rows[0]["SANum"].ToString(), dtDeliveryDetails.Rows[0]["SANum"].ToString()));
                ddlSLA.ToolTip = dtDeliveryDetails.Rows[0]["SANum"].ToString();
                FunPriGetExistingDetails(ddlMLA.SelectedItem.Text, ddlSLA.Items.Count > 0 ? ddlSLA.SelectedItem.Text : "");
            }
            else
            {
                lblSLA.CssClass = "styleFieldLabel";
                rfvSLA.Enabled = false;
                ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            }
            string[] str_Lob = ddlLOB.SelectedItem.Text.Split('-');
            //if (str_Lob[0].ToString().Trim().ToLower() != "wc")
            //{
            //FunPriGetExistingDetails(ddlMLA.SelectedItem.Text, ddlSLA.SelectedItem.Text);
           
            FunPriGetCollateral(ddlMLA.SelectedItem.Text);
            //}
            //getSAN();
            //if (ddlSLA.Items.Count != 0)
            //{
            //    ddlSLA.SelectedValue = dtDeliveryDetails.Rows[0]["SANum"].ToString();
            //}
            //Removed By Shibu
           // FunPriGetCustomerDetails(ddlMLA.SelectedItem.Text);
            if (dtDeliveryDetails.Rows.Count > 0)
            {
                DataRow dtRow1 = dtDeliveryDetails.Rows[0];
                ucdCustomer.SetCustomerDetails(dtRow1, true);
                txtCustID.Text = dtRow1["Customer_ID"].ToString();
                txtHDDate.Text = txtAccDate.Text = Utility.StringToDate(dtRow1["Creation_Date"].ToString()).ToString(strDateFormat);
            }

            //FunPriGetDate(ddlMLA.SelectedItem.Text);

            txtDate.Text = Utility.StringToDate(dtRow["TLE_WC_Date"].ToString()).ToString(strDateFormat);
            txtTLEWC.Text = dtDeliveryDetails.Rows[0]["TLE_WC_No"].ToString();
            //ddlTopupStatus.SelectedValue = dtDeliveryDetails.Rows[0]["Topup_Status_Code"].ToString();
            hdnCurrentFinReq.Value = txtCurrent.Text = dtDeliveryDetails.Rows[0]["Current_Finance_Required"].ToString();
            //string strSanctionedAmount = Convert.ToDecimal(dtDeliveryDetails.Rows[0]["Sanctioned_Amount"]) == 0 ? "0" : dtDeliveryDetails.Rows[0]["Sanctioned_Amount"].ToString(); //hdnCurrentFinReq.Value : dtDeliveryDetails.Rows[0]["Sanctioned_Amount"].ToString();
            //txtSanctionedAmount.Text = Convert.ToDecimal(strSanctionedAmount).ToString(Utility.SetSuffix()); //Added by Tamilselvan.S on 1/11/2011
            txtSanctionedAmount.Text = dtDeliveryDetails.Rows[0]["Finance_Amount"].ToString();
            if (string.IsNullOrEmpty(strMode) && strMode == "C")
            {
                txtExistingFinanceAmount.Text = dtDeliveryDetails.Rows[0]["Finance_Amount"].ToString(); //Added by Tamilselvan.S on 1/11/2011
            }
            else
            {
                txtExistingFinanceAmount.Text = (Convert.ToInt32(dtDeliveryDetails.Rows[0]["Finance_Amount"]) - Convert.ToInt32(dtDeliveryDetails.Rows[0]["Current_Finance_Required"])).ToString(); //Added by Tamilselvan.S on 1/11/2011
            }
            //Added by Saranya
            txtSanctionedAmount.ReadOnly = true;
            txtRemarks.Text = dtDeliveryDetails.Rows[0]["Remarks"].ToString();
            txtRemarks.ReadOnly = true;

            txtStages.Text = dtDeliveryDetails.Rows[0]["Stages"].ToString();
            txtStages.ReadOnly = true;
            //End
            //txtInstallmentAmount.Text = dtDeliveryDetails.Rows[0]["Instalment_Amount"].ToString();
            //txtNextLiner.Text = dtDeliveryDetails.Rows[0]["Start_Instalment"].ToString();
            txtTopupStatus.Text = dtDeliveryDetails.Rows[0]["Topup_Status_Code"].ToString();
            if ((intTLEWCID > 0) && (strMode == "M")) // Added by Tamilselvan.S on 21/02/2011 for Replacing unwanted code.
                FunPriGetModifyTLEWC(Convert.ToString(dtDeliveryDetails.Rows[0]["Topup_Status_Code"]));
            txtTopupStatus.Text = Convert.ToString(dtDeliveryDetails.Rows[0]["TopUpStatusValue"]);
            //Commented by Saranya to disabled the Repay Tab in modify and query mode
            //tpRepay.Enabled = tpCollateralDetails.Enabled = true;
            tpCollateralDetails.Enabled = true;
            if (ViewState["ModeType"] != null)
            {
                if (ViewState["ModeType"].ToString().StartsWith("Pro"))
                {
                    tpRepay.Enabled = false;
                }
                else
                {
                    tpRepay.Enabled = true;
                }
            }
            //end
            if (Convert.ToString(dtDeliveryDetails.Rows[0]["Topup_Status_Code"]) == "3")
            {
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                txtAccDate.ReadOnly = true;
                txtDate.ReadOnly = true;
                txtSanctionedAmount.ReadOnly = txtCurrent.ReadOnly = true;//Changed by Tamilselvan.S on1/11/2011
                txtInstallmentAmount.ReadOnly = true;
                txtNextLiner.ReadOnly = true;
                btnSave.Enabled = btnCancelTopup.Enabled = btnGO.Enabled = false;
                btnClear.Enabled = false;
                ddlCollateral.Enabled = false;
                if (bClearList)
                {
                    // ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    //ddlMLA.ClearDropDownList();
                    if (ddlSLA.SelectedValue != "0")  //Modified by Tamiselvan.S on 10/02/2011 for bug fixing
                        ddlSLA.ClearDropDownList(); ;
                    ddlCollateral.ClearDropDownList();
                    // ddlTopupStatus.ClearDropDownList();
                }
            }
            else if (Convert.ToString(dtDeliveryDetails.Rows[0]["Topup_Status_Code"]) == "4")
            {
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                txtAccDate.ReadOnly = true;
                txtDate.ReadOnly = true;
                txtSanctionedAmount.ReadOnly = txtCurrent.ReadOnly = true;//changed by Tamilselvan.S on 1/11/2011
                txtInstallmentAmount.ReadOnly = true;
                txtNextLiner.ReadOnly = true;
                btnSave.Enabled = btnCancelTopup.Enabled = btnGO.Enabled = false;
                btnClear.Enabled = false;
                ddlCollateral.Enabled = false;
                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    ddlMLA.ClearDropDownList();
                    if (ddlSLA.Items.Count > 0)  //Modified by Tamiselvan.S on 10/02/2011 for bug fixing
                        ddlSLA.ClearDropDownList();
                    ddlCollateral.ClearDropDownList();
                    // ddlTopupStatus.ClearDropDownList();
                }
            }
            else if (Convert.ToString(dtDeliveryDetails.Rows[0]["Topup_Status_Code"]) == "5" || Convert.ToString(dtDeliveryDetails.Rows[0]["Topup_Status_Code"]) == "2")
            {
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                txtAccDate.ReadOnly = true;
                txtDate.ReadOnly = true;
                txtSanctionedAmount.ReadOnly = txtCurrent.ReadOnly = true;//Changed by Tamilselvan.S on 1/11/2011
                txtInstallmentAmount.ReadOnly = true;
                txtNextLiner.ReadOnly = true;
                btnSave.Enabled = btnCancelTopup.Enabled = btnGO.Enabled = false;
                btnClear.Enabled = false;
                ddlCollateral.Enabled = false;
                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    ddlMLA.ClearDropDownList();
                    if (ddlSLA.Items.Count > 0)
                        ddlSLA.ClearDropDownList();
                    ddlCollateral.ClearDropDownList();
                }
            }
            else if (Convert.ToString(dtDeliveryDetails.Rows[0]["Topup_Status_Code"]) != "2")
            {
                btnSave.Enabled = true;
                if (strMode=="C")                
                    btnClear.Enabled = true;               
                else               
                    btnClear.Enabled = false;               
                
                btnCancelTopup.Enabled = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #region [Role Access Setup]

    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                btnCancelTopup.Enabled = false;
                if (!bCreate)
                {
                    btnSave.Enabled = btnCancelTopup.Enabled = false;
                }
                ddlCollateral.Enabled = false;
                break;

            case 1: // Modify Mode

             
                //FunPriGetModifyTLEWC();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }

                ddlCollateral.Enabled = false;
                btnClear.Enabled = false;
                FunPriGetTLEWC();
                //Added by Tamiselvan.S on 10/02/2011 for bug fixing
                if (bClearList)
                {
                  //  ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    if (ddlMLA.Items.Count > 0)
                        ddlMLA.ClearDropDownList();
                    if (ddlSLA.Items.Count > 0)
                        ddlSLA.ClearDropDownList();
                    else
                        ddlSLA.Enabled = false;
                    ddlCollateral.ClearDropDownList();
                    // ddlTopupStatus.ClearDropDownList();
                }
                break;

            case -1:// Query Mode
                txtAccDate.ReadOnly = true;
                txtDate.ReadOnly = true;
                txtSanctionedAmount.ReadOnly = txtCurrent.ReadOnly = true;//changed by Tamilselvan.S on 1/11/2011
                txtInstallmentAmount.ReadOnly = true;
                txtNextLiner.ReadOnly = true;
                btnSave.Enabled = btnCancelTopup.Enabled = false;
                btnClear.Enabled = false;
                ddlCollateral.Enabled = false;
                btnGO.Enabled = false;

                FunPriGetTLEWC();

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                
                
                if (bClearList)
                {
                   // ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    if (ddlMLA.Items.Count > 0)
                        ddlMLA.ClearDropDownList();
                    if (ddlSLA.Items.Count > 0)     //Modified by Tamiselvan.S on 10/02/2011 for bug fixing
                        ddlSLA.ClearDropDownList();
                    else
                        ddlSLA.Enabled = false;
                    ddlCollateral.ClearDropDownList();
                    // ddlTopupStatus.ClearDropDownList();
                }
                break;
        }
    }

    #endregion [Role Access Setup]

    #region [Top Up Save]

    public void FunPubSaveTopUp()
    {
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable ObjS3G_TLEWCInsDataTable = new LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable();
        LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupRow ObjTLEWCInsRow = ObjS3G_TLEWCInsDataTable.NewS3G_LOAND_InsTLEWCTopupRow();
        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=TUP';";
        string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdTLEWCTopup_Add.aspx';";
        string strTLEWC_No = string.Empty;

        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@PANum", ddlMLA.SelectedItem.ToString());

            if (ddlSLA.Items.Count == 0)
                Procparam.Add("@SANum", ddlMLA.SelectedItem.ToString() + "DUMMY");
            else
                Procparam.Add("@SANum", ddlSLA.SelectedItem.ToString());

            ObjTLEWCInsRow.TLEWCTopup_ID = intTLEWCID;
            ObjTLEWCInsRow.Company_ID = intCompanyId;
            ObjTLEWCInsRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjTLEWCInsRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);

            ObjTLEWCInsRow.PANum = ddlMLA.Text;
            if (ddlSLA.Items.Count == 0)
                ObjTLEWCInsRow.SANum = Convert.ToString(ddlMLA.Text) + "DUMMY";
            else
                ObjTLEWCInsRow.SANum = Convert.ToString(ddlSLA.Text);

            ObjTLEWCInsRow.Customer_ID = Convert.ToInt32(txtCustID.Text);
            ObjTLEWCInsRow.TLE_WC_Date = Utility.StringToDate(txtDate.Text);
            ObjTLEWCInsRow.Topup_Status_Type_Code = 9;
            ObjTLEWCInsRow.Topup_Status_Code = 1;
            ObjTLEWCInsRow.Current_Finance_Required = Convert.ToDecimal(txtCurrent.Text);
            ObjTLEWCInsRow.Sanctioned_Amount = string.IsNullOrEmpty(txtSanctionedAmount.Text.Trim()) ? 0 : Convert.ToDecimal(txtSanctionedAmount.Text);//Added by Tamilselvan.S on 1/11/2011
            //Added by Saranya I on 21/02/2012
            ObjTLEWCInsRow.Stages = txtStages.Text;
            ObjTLEWCInsRow.Remarks = txtRemarks.Text;
            //END
            //ObjTLEWCInsRow.Start_Instalment = Convert.ToInt32(txtNextLiner.Text);
            ObjTLEWCInsRow.Start_Instalment = 1;
            string[] str_Lobs = ddlLOB.SelectedItem.Text.Split('-');
            string strLOBCode = str_Lobs[0].ToString().Trim().ToLower();
            //if (strLOBCode == "wc")
            //    ObjTLEWCInsRow.Instalment_Amount = Convert.ToDecimal(txtCurrent.Text);// Convert.ToDecimal(((TextBox)gvNewStructure.FindControl("txtNewMontly")).Text);
            //else
            //    ObjTLEWCInsRow.Instalment_Amount = Convert.ToDecimal(txtInstallmentAmount.Text);
            TextBox txtNewInsAmt = gvNewStructure.Rows[0].FindControl("txtNewMontly") as TextBox;
            ObjTLEWCInsRow.Instalment_Amount = Convert.ToDecimal(txtNewInsAmt.Text.Trim());
            ObjTLEWCInsRow.Account_Link_Key = 10;
            ObjTLEWCInsRow.Created_By = intUserId;
            ObjTLEWCInsRow.Created_On = DateTime.Now;
            ObjTLEWCInsRow.Modified_By = intUserId;
            ObjTLEWCInsRow.Modified_On = DateTime.Now;
            ObjTLEWCInsRow.TLE_WC_No = txtTLEWC.Text;

            ObjTLEWCInsRow.XML_TopupDeltails = FunPubGetNewTopUpStructure();
            ObjTLEWCInsRow.Txn_ID = 10;
            ObjS3G_TLEWCInsDataTable.AddS3G_LOAND_InsTLEWCTopupRow(ObjTLEWCInsRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjS3G_TLEWCInsDataTables = ClsPubSerialize.Serialize(ObjS3G_TLEWCInsDataTable, SerMode);

            if (intTLEWCID > 0)
            {
                intErrCode = objLoanAdmin_MasterClient.FunPubModifyTLEWC(SerMode, ObjS3G_TLEWCInsDataTables);
            }
            else
            {
                intErrCode = objLoanAdmin_MasterClient.FunPubCreateTLEWCIns(out strTLEWC_No, SerMode, ObjS3G_TLEWCInsDataTables);
            }
            if (intErrCode == 0)
            {
                if (intTLEWCID > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "TopUp Details updated successfully");
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                }
                else
                {
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strTLEWC_No, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not Assigned";
                        }
                        ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode, strAlert);
                        return;
                    }
                    else
                    {

                        strAlert = "TopUp Details " + strTLEWC_No + " added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    }
                }
            }
            else if (intErrCode == -1)
            {
                if (intTLEWCID == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    strRedirectPageView = "";
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Unable to update the TopUp Details");
                    strRedirectPageView = "";
                }
            }
            else if (intErrCode == -2)
            {
                if (intTLEWCID == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    strRedirectPageView = "";
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "TopUp Details already exists");
                strRedirectPageView = "";
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create TopUp");
            }
            if (intErrCode == 50)
            {
                strAlert = strAlert.Replace("__ALERT__", "Unable to update the TopUp Details");
                strRedirectPageView = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (FaultException<LoanAdminMgtServicesReference.ClsPubFaultException> ex)
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
            if (objLoanAdmin_MasterClient != null)
                objLoanAdmin_MasterClient.Close();
        }
    }

    #endregion [Top Up Save]

    #region [Calculate Interest]

    public void FunPubCalCulateInterest()
    {
        try
        {
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            //hdnCurrentFinanceInterest.Value = Convert.ToString(objRepaymentStructure.FunPubGetInterestAmount(txtCurrent.Text, hdnMarginPercentage.Value, hdnReturnPattern.Value, hdnRate.Value, hdnTenureType.Value, hdnTenure.Value));
            txtInstallmentAmount.Text = txtNextLiner.Text = "";
            gvNewStructure.DataSource = (DataTable)ViewState["dtDetailsNew"];
            gvNewStructure.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion [Calculate Interest]

    #region [Not Used Function]

    private void setinitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add("Installment_Date");
        dt.Columns.Add("Remayment_Amount");
        dt.Columns.Add("Monthly_Amount");
        dr = dt.NewRow();
        dr["Installment_Date"] = string.Empty;
        dr["Remayment_Amount"] = string.Empty;
        dr["Monthly_Amount"] = string.Empty;
        dt.Rows.Add(dr);
        ViewState["currenttable"] = dt;

    }

    private void FunPriGetDate(string strMLAID)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@PANum", ddlMLA.SelectedItem.Text);
        DataTable dtDetails = Utility.GetDefaultData("S3G_LOANAD_GetInstallmentDate", Procparam);
        DataRow dtRow = dtDetails.Rows[0];
        txtHDDate.Text = dtRow["Creation_Date"].ToString();
    }

    protected void getSAN()
    {
        for (i = 0; i < ddlSLA.Items.Count; i++)
        {
            s = ddlSLA.Items[i].ToString();
            status = s.Contains("DUMMY");
            if (status == true)
            {
                if (ddlSLA.Items.Count == 2)
                {
                    ViewState["dum"] = ddlSLA.Items[i].Text;
                    ddlSLA.Items.Clear();
                    break;
                }
                else
                {
                    ViewState["dum"] = ddlSLA.Items[i].Text;
                    ddlSLA.Items.RemoveAt(i);
                }
            }
        }
    }

    #region Date Format

    private string ConvertToCurrentFormat(string strDate)
    {
        //  string dT = strDate;

        try
        {
            if (strDate.Contains("1900"))
                strDate = string.Empty;


            strDate = strDate.Replace("12:00:00 AM", "");

            CultureInfo myDTFI = new CultureInfo("en-GB", true);
            DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
            DTF.ShortDatePattern = "dd/MM/yyyy";
            DateTime _Date = new DateTime();
            if (strDate != "")
            {
                _Date = System.Convert.ToDateTime(strDate, DTF);
                return _Date.ToString("dd/MM/yyyy");
            }
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            return strDate;
            // throw ex;
        }
    }

    #endregion

    //protected void FunCalculateIRR()
    //{
    //    try
    //    {

    //        CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();

    //        DataTable dtRepaymentTab = FunGetRepayDetails(ddlMLA.SelectedItem.ToString(), ddlSLA.SelectedItem.ToString());
    //        ViewState["PaymentDetails"] = dtRepaymentTab;


    //        Procparam.Add("@Flow_Type", "inflow");
    //        DataTable dtCashInflow = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetCashFlowDetails, Procparam);

    //        Procparam.Remove("@Flow_Type");
    //        Procparam.Add("@Flow_Type", "outflow");
    //        DataTable dtCashOutflow = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetCashFlowDetails, Procparam);

    //        Procparam.Clear();
    //        Procparam.Add("@Company_ID", intCompanyId.ToString());
    //        Procparam.Add("@Lob_ID", ddlLOB.SelectedValue);
    //        DataTable dtGlobalIRRDtls = Utility.GetDefaultData(SPNames.S3G_ORG_GetGlobalIRRDetails, Procparam);


    //        double decResultIrr = 0;
    //        bool blnIsRounded = true;
    //        string strType = ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim();
    //        Enum repay;
    //        if (strType == "wc")
    //        {
    //            repay = RepaymentType.WC;
    //        }
    //        else if (strType == "te")
    //        {
    //            repay = RepaymentType.TLE;

    //        }
    //        // existing calculation
    //        DataTable dtExistingRevisiedDetails = (DataTable)ViewState["AccountRevisionDetails"];
    //        DataTable dt_RepaymentDetails = ObjBusinessLogic.FunPubCalculateRepaymentDetails(

    //              //dtRepaymentTab,
    //            //dtRepaymentTab.Rows[0]["Frequency_Name"].ToString(),
    //            //Convert.ToInt32(dtRepaymentTab.Rows[0]["Tenure_Code"].ToString()),
    //            //dtRepaymentTab.Rows[0]["Tenure_Description"].ToString(),
    //            //Convert.ToInt32(dtExistingRevisiedDetails.Rows[2]["Existing"].ToString()),
    //            //Convert.ToDecimal(dtExistingRevisiedDetails.Rows[0]["Existing"].ToString()),
    //            //Convert.ToDecimal(dtExistingRevisiedDetails.Rows[1]["Existing"].ToString())
    //            //add the code change in irr Parameter 7 to 11)
    //           dtRepaymentTab.Rows[0]["Frequency_Name"].ToString(),
    //           Convert.ToInt32(dtRepaymentTab.Rows[0]["Tenure_Code"].ToString()),
    //           dtRepaymentTab.Rows[0]["Tenure_Description"].ToString(),
    //           Convert.ToDecimal(dtExistingRevisiedDetails.Rows[0]["Existing"].ToString()),
    //           Convert.ToDecimal(dtExistingRevisiedDetails.Rows[0]["Existing"].ToString()),
    //           RepaymentType.TLE,
    //           Convert.ToDecimal(dtExistingRevisiedDetails.Rows[1]["Existing"].ToString()),
    //           Utility.StringToDate(dtRepaymentTab.Rows[0]["Account_activated_date"].ToString()),
    //           Utility.StringToDate(dtRepaymentTab.Rows[0]["Account_activated_date"].ToString()),
    //           out blnIsRounded, null);
    //        ViewState["RepaymentDetails"] = dt_RepaymentDetails;


    //        // revised Calculation             
    //        DataTable dt_RevisedRepaymentDetails = ObjBusinessLogic.FunPubCalculateRepaymentDetails(
    //            //dtRepaymentTab,
    //            //dtRepaymentTab.Rows[0]["Frequency_Name"].ToString(),
    //            //Convert.ToInt32(dtRepaymentTab.Rows[0]["Tenure_Code"].ToString()),
    //            //dtRepaymentTab.Rows[0]["Tenure_Description"].ToString(),
    //            //Convert.ToInt32(dtExistingRevisiedDetails.Rows[2]["Revised"].ToString()),
    //            //Convert.ToDecimal(dtExistingRevisiedDetails.Rows[0]["Revised"].ToString()),
    //            //Convert.ToDecimal(dtExistingRevisiedDetails.Rows[1]["Revised"].ToString())
    //            //add the code change in irr Parameter 7 to 11)

    //           dtRepaymentTab.Rows[0]["Frequency_Name"].ToString(),
    //           Convert.ToInt32(dtRepaymentTab.Rows[0]["Tenure_Code"].ToString()),
    //           dtRepaymentTab.Rows[0]["Tenure_Description"].ToString(),
    //           Convert.ToDecimal(dtExistingRevisiedDetails.Rows[0]["Revised"].ToString()),
    //           Convert.ToDecimal(dtExistingRevisiedDetails.Rows[0]["Revised"].ToString()),
    //           RepaymentType.TLE,
    //           Convert.ToDecimal(dtExistingRevisiedDetails.Rows[1]["Revised"].ToString()),
    //           Utility.StringToDate(dtRepaymentTab.Rows[0]["Account_activated_date"].ToString()),
    //           Utility.StringToDate(dtRepaymentTab.Rows[0]["Account_activated_date"].ToString()),
    //           out blnIsRounded, null
    //           );
    //        ViewState["RevisedRepaymentDetails"] = dt_RevisedRepaymentDetails;


    //        //Function for Calculating IRR Called
    //        ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow,
    //            dtRepaymentTab.Rows[0]["Frequency_Name"].ToString(),
    //            Convert.ToInt32(dtRepaymentTab.Rows[0]["Tenure_Code"].ToString()),
    //            dtRepaymentTab.Rows[0]["Tenure_Type_Code"].ToString(), strDateFormat,
    //            Convert.ToDecimal(dtRepaymentTab.Rows[0]["Finance_Amount"].ToString()),
    //            Convert.ToDecimal(dtRepaymentTab.Rows[0]["Rate"].ToString()),
    //            dtRepaymentTab.Rows[0]["IRR_Rest_Name"].ToString(), "Empty",
    //            dtRepaymentTab.Rows[0]["TimeValue_Name"].ToString(),
    //            Convert.ToDecimal(0.10),
    //            IRRType.Accounting_IRR, out decResultIrr,
    //            Convert.ToDecimal(10.05),
    //            Convert.ToDecimal(dtGlobalIRRDtls.Rows[0]["Prime_Lending_Rate"].ToString()),
    //            Utility.StringToDate(dtRepaymentTab.Rows[0]["Account_activated_date"].ToString()),
    //            //add the code change in irr Parameter 15 to 18)
    //            null, null,
    //            RepaymentType.TLE);
    //        Utility.FunShowAlertMsg(this, "Accounting IRR:" + decResultIrr.ToString("0.0000").ToString());

    //        //FunIRR("Accounting IRR", decResultIrr);


    //        ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow,
    //             dtRepaymentTab.Rows[0]["Frequency_Name"].ToString(),
    //             Convert.ToInt32(dtRepaymentTab.Rows[0]["Tenure_Code"].ToString()),
    //             dtRepaymentTab.Rows[0]["Tenure_Type_Code"].ToString(), strDateFormat,
    //             Convert.ToDecimal(dtRepaymentTab.Rows[0]["Finance_Amount"].ToString()),
    //             Convert.ToDecimal(dtRepaymentTab.Rows[0]["Rate"].ToString()),
    //             dtRepaymentTab.Rows[0]["IRR_Rest_Name"].ToString(), "Empty",
    //             dtRepaymentTab.Rows[0]["TimeValue_Name"].ToString(),
    //             Convert.ToDecimal(0.10),
    //             IRRType.Accounting_IRR, out decResultIrr,
    //             Convert.ToDecimal(10.05),
    //             Convert.ToDecimal(dtGlobalIRRDtls.Rows[0]["Prime_Lending_Rate"].ToString()),
    //             Utility.StringToDate(dtRepaymentTab.Rows[0]["Account_activated_date"].ToString()),
    //            //add the code change in irr Parameter 15 to 18)
    //             null, null,
    //             RepaymentType.TLE);
    //        Utility.FunShowAlertMsg(this, "Accounting IRR:" + decResultIrr.ToString("0.0000").ToString());
    //        // FunIRR("Business IRR", decResultIrr);

    //        ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow,
    //            dtRepaymentTab.Rows[0]["Frequency_Name"].ToString(),
    //            Convert.ToInt32(dtRepaymentTab.Rows[0]["Tenure_Code"].ToString()),
    //            dtRepaymentTab.Rows[0]["Tenure_Type_Code"].ToString(), strDateFormat,
    //            Convert.ToDecimal(dtRepaymentTab.Rows[0]["Finance_Amount"].ToString()),
    //            Convert.ToDecimal(dtRepaymentTab.Rows[0]["Rate"].ToString()),
    //            dtRepaymentTab.Rows[0]["IRR_Rest_Name"].ToString(), "Empty",
    //            dtRepaymentTab.Rows[0]["TimeValue_Name"].ToString(),
    //            Convert.ToDecimal(0.10),
    //            IRRType.Accounting_IRR, out decResultIrr,
    //            Convert.ToDecimal(10.05),
    //            Convert.ToDecimal(dtGlobalIRRDtls.Rows[0]["Prime_Lending_Rate"].ToString()),
    //            Utility.StringToDate(dtRepaymentTab.Rows[0]["Account_activated_date"].ToString()),
    //            //add the code change in irr Parameter 15 to 18)
    //            null, null,
    //            RepaymentType.TLE);
    //        Utility.FunShowAlertMsg(this, "Accounting IRR:" + decResultIrr.ToString("0.0000").ToString());
    //        // FunIRR("Company IRR", decResultIrr);

    //        ObjBusinessLogic.FunPubCalculateIRR(dtRepaymentTab, dtCashInflow, dtCashOutflow,
    //            dtRepaymentTab.Rows[0]["Frequency_Name"].ToString(),
    //            Convert.ToInt32(dtRepaymentTab.Rows[0]["Tenure_Code"].ToString()),
    //            dtRepaymentTab.Rows[0]["Tenure_Type_Code"].ToString(), strDateFormat,
    //            Convert.ToDecimal(dtRepaymentTab.Rows[0]["Finance_Amount"].ToString()),
    //            Convert.ToDecimal(dtRepaymentTab.Rows[0]["Rate"].ToString()),
    //            dtRepaymentTab.Rows[0]["IRR_Rest_Name"].ToString(), "Empty",
    //            dtRepaymentTab.Rows[0]["TimeValue_Name"].ToString(),
    //            Convert.ToDecimal(0.10),
    //            IRRType.Accounting_IRR, out decResultIrr,
    //            Convert.ToDecimal(10.05),
    //            Convert.ToDecimal(dtGlobalIRRDtls.Rows[0]["Prime_Lending_Rate"].ToString()),
    //            Utility.StringToDate(dtRepaymentTab.Rows[0]["Account_activated_date"].ToString()),
    //            //add the code change in irr Parameter 15 to 18)
    //            null, null,
    //            RepaymentType.TLE);
    //        Utility.FunShowAlertMsg(this, "Accounting IRR:" + decResultIrr.ToString("0.0000").ToString());

    //        DataTable dt = (DataTable)ViewState["IRR"];

    //        dt.Rows[0]["Revised"] = Bussiness_IRR;
    //        dt.Rows[1]["Revised"] = Company_IRR;
    //        dt.Rows[2]["Revised"] = Accounting_IRR;
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + ex.Message.Replace("'", " ").Replace(";", " ") + "');", true);
    //    }
    //}

    private DataTable FunGetRepayDetails(string strPANum, string strSANum)
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@PANum", strPANum);
        Procparam.Add("@SANum", strSANum);
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@groupby", "0");

        DataTable dtRepaymentTab = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAccountRepayDetails, Procparam);
        Procparam.Remove("@groupby");
        Procparam.Add("@groupby", "1");

        DataTable dtRepaymentTabGrouped = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAccountRepayDetails, Procparam);
        Procparam.Remove("@groupby");

        DataTable dtRepayDetails = dtRepaymentTab.Clone();

        if (dtRepaymentTab != null && dtRepaymentTabGrouped != null && dtRepaymentTab.Rows.Count > 0 && dtRepaymentTabGrouped.Rows.Count > 0)
        {
            for (int i_loopGrouped = 0; i_loopGrouped < dtRepaymentTabGrouped.Rows.Count; i_loopGrouped++)
            {
                dtRepaymentTab.DefaultView.RowFilter = "Installment_Period = '" + dtRepaymentTabGrouped.Rows[i_loopGrouped]["Installment_Period"].ToString() + "'";
                DataRow dr = dtRepayDetails.NewRow();
                for (int i_copyrow = 0; i_copyrow < dtRepaymentTab.Columns.Count; i_copyrow++)
                    dr[i_copyrow] = dtRepaymentTab.DefaultView.ToTable().Rows[0][i_copyrow];

                dr["Amount"] = dtRepaymentTabGrouped.Rows[i_loopGrouped]["Installment_Amount"];
                dr["FromDate"] = Utility.StringToDate(dtRepaymentTab.DefaultView.ToTable().Rows[0]["Installment_Date"].ToString());
                dr["ToDate"] = Utility.StringToDate(dtRepaymentTab.DefaultView.ToTable().Rows[dtRepaymentTab.DefaultView.ToTable().Rows.Count - 1]["Installment_Date"].ToString());
                dr["FromInstall"] = (dtRepaymentTab.DefaultView.ToTable().Rows[0]["Installment_Number"].ToString());
                dr["ToInstall"] = (dtRepaymentTab.DefaultView.ToTable().Rows[dtRepaymentTab.DefaultView.ToTable().Rows.Count - 1]["Installment_Number"].ToString());

                dtRepayDetails.Rows.Add(dr);
            }
        }
        return dtRepayDetails;
    }

    protected void FunCalculation(object sender, EventArgs e)
    {
        decimal total = 0;
        string strFieldAtt = ((TextBox)sender).ClientID;
        string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvNewStructure")).Replace("gvNewStructure_ctl", "");
        int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
        gRowIndex = gRowIndex - 2;
        Label Quly = (Label)gvNewStructure.Rows[gRowIndex].FindControl("lblRepaymentAmount");

        TextBox calc = (TextBox)gvNewStructure.Rows[gRowIndex].FindControl("txtNewMontly");
        total = Convert.ToDecimal(Quly.Text) - Convert.ToDecimal(calc.Text);
        Quly.Text = total.ToString();

        if (gvNewStructure.Rows.Count > gRowIndex + 1)
        {
            Label lblNextRepay = (Label)gvNewStructure.Rows[gRowIndex + 1].FindControl("lblRepaymentAmount");
            lblNextRepay.Text = total.ToString();
        }
    }

    //protected string FunProFormXML()
    //{
    //    string strXMLAccDetails = string.Empty;
    //    StringBuilder strbSysJournal = new StringBuilder();
    //    strbSysJournal.Append("<Root> ");
    //    int counter = 1;        
    //    //DataTable dt_Details = new DataTable();
    //    //dt_Details = (DataTable)ViewState["dtDetailsNew"];
    //    //dt_Details = InstallPer(dt_Details);

    //    foreach (GridViewRow grvData in gvNewStructure.Rows)
    //    {
    //        DataTable dt_Details = new DataTable();
    //        dt_Details = (DataTable)ViewState["dtDetailsNew"];
    //        string strInstallDate = ((Label)grvData.FindControl("lblInsDate")).Text;
    //        string strInstallAmount = ((TextBox)grvData.FindControl("txtNewMontly")).Text;
    //        string strInstallno = dt_Details.Rows[0]["Installment_Number"].ToString();
    //        string strCompanyID = Convert.ToString(intCompanyId);
    //        string strInstallPeriod = dt_Details.Rows[0]["Installment_Period"].ToString();
    //        string strRepayID = dt_Details.Rows[0]["RepayID"].ToString();
    //        string strBackup = dt_Details.Rows[0]["BreakUp_Percentage"].ToString();

    //        strbSysJournal.Append(" <Details ");
    //        strbSysJournal.Append(" PANum='" + ddlMLA.SelectedItem.Text + "' ");
    //        if (ddlSLA.Items.Count == 0)
    //        {   
    //            strbSysJournal.Append(" SANum='" + Convert.ToString(ViewState["dum"]) + "'");

    //        }
    //        else
    //        {
    //            strbSysJournal.Append(" SANum='" + ddlSLA.SelectedItem.Text + "' ");
    //        }
    //        strbSysJournal.Append(" Company_ID='" + intCompanyId + "' ");
    //        strbSysJournal.Append(" Repay_ID='" + strRepayID.ToString() + "' ");
    //        strbSysJournal.Append(" Installment_Period='" + strInstallPeriod.ToString() + "' ");
    //        strbSysJournal.Append(" Installment_Date='" + strInstallDate.ToString() + "' ");
    //        strbSysJournal.Append(" Installment_Number ='" + counter.ToString() + "' "); // check this and change it.
    //        strbSysJournal.Append(" Installment_Amount='" + strInstallAmount.ToString() + "' "); // check this and change it.
    //        strbSysJournal.Append(" BreakUp_Percentage='" + strBackup.ToString() + "' "); // check this and change it.
    //        strbSysJournal.Append(" /> ");
    //        ++counter;
    //    }
    //    strbSysJournal.Append(" </Root>");
    //    return strbSysJournal.ToString();
    //}

    private DataTable InstallPer(DataTable dt_Details)
    {
        CheckBox strInstallPeriod = (CheckBox)gvNewStructure.FindControl("CbxChange");
        if (dt_Details != null && dt_Details.Rows.Count > 0)
        {
            decimal previousAmount = (Convert.ToDecimal("0.00"));
            int startValue = 0;
            string period;
            int end = 0;

            DataColumn dc = new DataColumn("Temp");
            dc.DefaultValue = "";

            dt_Details.Columns.Add(dc);

            for (int i = 0; i < dt_Details.Rows.Count; i++)
            {
                if (i == 0)
                {
                    startValue = 1;
                    previousAmount = Convert.ToDecimal(dt_Details.Rows[i]["Installment_Amount"]);
                }
                //if (i == startValue)
                //{
                //    period = (i + 1).ToString() + "-";
                //    previousAmount = Convert.ToDecimal(dt_Details.Rows[i]["txtNewMontly"]);
                //}
                if (previousAmount != Convert.ToDecimal(dt_Details.Rows[i]["Installment_Amount"]))
                {
                    period = (startValue).ToString() + "-";
                    period += (i).ToString();
                    startValue = (i + 1);
                    previousAmount = Convert.ToDecimal(dt_Details.Rows[i]["Installment_Amount"]);
                    dt_Details.Rows[i]["Temp"] = period;
                }
            }

            // reverse loop
            for (int i = dt_Details.Rows.Count - 1; i >= 0; i--)
            {
                if (i < (dt_Details.Rows.Count - 1))
                {
                    if (string.IsNullOrEmpty(dt_Details.Rows[i]["Temp"].ToString()))
                    {
                        dt_Details.Rows[i]["Temp"] = dt_Details.Rows[i + 1]["Temp"].ToString();
                    }
                }
            }
            // overwrite loop
            for (int i = 0; i < dt_Details.Rows.Count; i++)
            {
                dt_Details.Rows[i]["Installment_Period"] = dt_Details.Rows[i + 1]["Temp"].ToString();
            }
            // revoing temp col
            dt_Details.Columns.Remove("Temp");
        }
        return dt_Details;
    }

    //public void FunPubSaveTopUp()
    //{
    //    //}
    //    //code to archeive details...
    //    if (Procparam == null)
    //        Procparam = new Dictionary<string, string>();
    //    else
    //        Procparam.Clear();

    //    Procparam.Add("@PANum", ddlMLA.SelectedItem.ToString());
    //    Procparam.Add("@Company_ID", intCompanyId.ToString());

    //    //Procparam.Add("@SANum", ddlSLA.SelectedItem.ToString());

    //    if (ddlSLA.Items.Count == 0)
    //    {
    //        Procparam.Add("@SANum", ddlMLA.SelectedItem.ToString() + "DUMMY");

    //    }
    //    else
    //    {
    //        Procparam.Add("@SANum", ddlSLA.SelectedItem.ToString());
    //    }

    //    //Procparam.Add("@PANum", "2010-2011/FLMLA/201");
    //    //ddlMLA.SelectedItem.ToString());
    //    //Procparam.Add("@SANum", "2010-2011/SLA/7");
    //    //ddlSLA.SelectedItem.ToString());
    //    DataTable dtRepaydetails = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAccountRepayDetails_ByPASA, Procparam);
    //    ViewState["dtIsDetails"] = dtRepaydetails;

    //    string strRevisionNumber = string.Empty;
    //    string strRevision_Type_Code = "9"; // this want to hard code in lookup - to denote the Page type.
    //    string strRevision_Type = "1"; // this want to hard code in lookup - to denote the Page type.
    //    decimal Bussiness_IRR;
    //    decimal Company_IRR;
    //    decimal Accounting_IRR;
    //    int Repay_ID = 0;
    //    if (Procparam != null)
    //        Procparam.Clear();
    //    else
    //        Procparam = new Dictionary<string, string>();


    //    // XML String Start
    //    string strXMLAccDetails = string.Empty;
    //    StringBuilder strbSysJournal = new StringBuilder();
    //    strbSysJournal.Append("<Root> ");
    //    int counter = 1;
    //    for (int i_paydetails = 0; i_paydetails < dtRepaydetails.Rows.Count; i_paydetails++)
    //    {
    //        strbSysJournal.Append(" <Details ");
    //        strbSysJournal.Append(" Company_ID ='" + dtRepaydetails.Rows[i_paydetails]["Company_ID"].ToString() + "' ");
    //        strbSysJournal.Append(" PANum ='" + dtRepaydetails.Rows[i_paydetails]["PANum"].ToString() + "' ");
    //        strbSysJournal.Append(" SANum ='" + dtRepaydetails.Rows[i_paydetails]["SANum"].ToString() + "' ");
    //        strbSysJournal.Append(" Installment_Date ='" + dtRepaydetails.Rows[i_paydetails]["Installment_Date"].ToString() + "' ");
    //        strbSysJournal.Append(" Installment_Period ='" + dtRepaydetails.Rows[i_paydetails]["Installment_Period"].ToString() + "' ");
    //        strbSysJournal.Append(" Installment_Number ='" + dtRepaydetails.Rows[i_paydetails]["Installment_Number"].ToString() + "' ");
    //        strbSysJournal.Append(" Installment_Amount ='" + dtRepaydetails.Rows[i_paydetails]["Installment_Amount"].ToString() + "' ");
    //        strbSysJournal.Append(" BreakUp_Percentage ='" + dtRepaydetails.Rows[i_paydetails]["BreakUp_Percentage"].ToString() + "' ");
    //        strbSysJournal.Append(" /> ");
    //    }
    //    strbSysJournal.Append(" </Root>");

    //    // XML String End


    //    ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderDataTable ObjdtArch = new ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderDataTable();
    //    ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderRow ObjSpecificationRevisionArcheiveRow = ObjdtArch.NewS3G_LOANAD_RepayDetailsIRRArchiveHeaderRow();
    //    ObjSpecificationRevisionArcheiveRow.Company_ID = intCompanyId;
    //    ObjSpecificationRevisionArcheiveRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
    //    ObjSpecificationRevisionArcheiveRow.Revision_ID = 0;   // to do change it
    //    ObjSpecificationRevisionArcheiveRow.Revision_Type = 1; // this is to identify the specific page
    //    ObjSpecificationRevisionArcheiveRow.Revision_Type_Code = 1;// this is to identify the specific page           
    //    ObjSpecificationRevisionArcheiveRow.PANum = dtRepaydetails.Rows[0]["PANum"].ToString();
    //    ObjSpecificationRevisionArcheiveRow.SANum = dtRepaydetails.Rows[0]["SANum"].ToString();
    //    ObjSpecificationRevisionArcheiveRow.Repay_ID = Convert.ToInt32(dtRepaydetails.Rows[0]["RepayID"]);
    //    //ObjSpecificationRevisionArcheiveRow.Business_IRR = Convert.ToDecimal(dtIRR.Rows[0]["Existing"]); // to do - get the existing IRR
    //    //ObjSpecificationRevisionArcheiveRow.Company_IRR = Convert.ToDecimal(dtIRR.Rows[1]["Existing"]); // to do - get the existing IRR
    //    //ObjSpecificationRevisionArcheiveRow.Accounting_IRR = Convert.ToDecimal(dtIRR.Rows[2]["Existing"]); // to do - get the existing IRR
    //    ObjSpecificationRevisionArcheiveRow.Business_IRR = Convert.ToDecimal("1.1"); // to do - get the existing IRR
    //    ObjSpecificationRevisionArcheiveRow.Company_IRR = Convert.ToDecimal("1.1"); // to do - get the existing IRR
    //    ObjSpecificationRevisionArcheiveRow.Accounting_IRR = Convert.ToDecimal("1.1"); // to do - get the existing IRR
    //    ObjSpecificationRevisionArcheiveRow.Archive_Date = DateTime.Now;
    //    ObjSpecificationRevisionArcheiveRow.XMLArchDetails = strbSysJournal.ToString();
    //    // strXMLAccDetails.ToString();
    //    //FunProFormXML().ToString();

    //    //FunCalculateIRR();
    //    SerializationMode SerMode = SerializationMode.Binary;
    //    ObjdtArch.AddS3G_LOANAD_RepayDetailsIRRArchiveHeaderRow(ObjSpecificationRevisionArcheiveRow);
    //    ObjSpecificRevisionClient = new ContractMgtServicesReference.ContractMgtServicesClient();
    //    if (intTLEWCID == 0)
    //    {
    //        int intErrCode = ObjSpecificRevisionClient.FunPubCreateOrModifySpecificationArchive(SerMode, ClsPubSerialize.Serialize(ObjdtArch, SerMode));
    //    }



    //    LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable ObjS3G_TLEWCInsDataTable = new LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable();
    //    LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupRow ObjTLEWCInsRow;
    //    ObjTLEWCInsRow = ObjS3G_TLEWCInsDataTable.NewS3G_LOAND_InsTLEWCTopupRow();
    //    ObjTLEWCInsRow.TLEWCTopup_ID = intTLEWCID;
    //    ObjTLEWCInsRow.Company_ID = intCompanyId;
    //    ObjTLEWCInsRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
    //    ObjTLEWCInsRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
    //    ObjTLEWCInsRow.PANum = ddlMLA.SelectedItem.Text;
    //    //"2010-2011/FLMLA/201";
    //    //
    //    if (ddlSLA.Items.Count == 0)
    //    {
    //        ObjTLEWCInsRow.SANum = Convert.ToString(ddlMLA.SelectedItem) + "DUMMY";
    //        //  ObjTLEWCInsRow.SANum = Convert.ToString(ViewState["dum"]);
    //    }
    //    else
    //    {
    //        ObjTLEWCInsRow.SANum = Convert.ToString(ddlSLA.SelectedItem.Text);
    //    }
    //    //ObjTLEWCInsRow.SANum = ddlSLA.SelectedItem.Text;
    //    //"2010-2011/SLA/7";
    //    //
    //    ObjTLEWCInsRow.Customer_ID = Convert.ToInt32(txtCustID.Text);
    //    ObjTLEWCInsRow.TLE_WC_Date = Utility.StringToDate(txtDate.Text);
    //    ObjTLEWCInsRow.Topup_Status_Type_Code = 9;
    //    ObjTLEWCInsRow.Topup_Status_Code = 1;
    //    ObjTLEWCInsRow.Current_Finance_Required = Convert.ToInt32(txtCurrent.Text);

    //    string[] str_Lob = ddlLOB.SelectedItem.Text.Split('-');
    //    if (str_Lob[0].ToString().Trim().ToLower() == "wc")
    //    {
    //        ObjTLEWCInsRow.Start_Instalment = Convert.ToInt32(txtNextLiner.Text);
    //    }
    //    else
    //    {
    //        ObjTLEWCInsRow.Start_Instalment = 1;
    //    }
    //    if (str_Lob[0].ToString().Trim().ToLower() == "te")
    //    {
    //        if (txtNextLiner.Text == "")
    //        {
    //            cvTLE.ErrorMessage = "Enter the Next Liner";
    //            cvTLE.IsValid = false;
    //            return;
    //        }
    //        else
    //        {
    //            ObjTLEWCInsRow.Start_Instalment = Convert.ToInt32(txtNextLiner.Text);
    //        }
    //        if (txtInstallmentAmount.Text == "")
    //        {
    //            cvTLE.ErrorMessage = "Enter the Installment Amount";
    //            cvTLE.IsValid = false;
    //            return;
    //        }
    //        else
    //        {
    //            ObjTLEWCInsRow.Instalment_Amount = Convert.ToDecimal(txtInstallmentAmount.Text);
    //        }
    //    }


    //    ObjTLEWCInsRow.Instalment_Amount = Convert.ToDecimal(txtInstallmentAmount.Text);
    //    ObjTLEWCInsRow.Account_Link_Key = 10;
    //    ObjTLEWCInsRow.Created_By = intUserId;
    //    ObjTLEWCInsRow.Created_On = DateTime.Now;
    //    ObjTLEWCInsRow.Modified_By = intUserId;
    //    ObjTLEWCInsRow.Modified_On = DateTime.Now;

    //    ObjTLEWCInsRow.XML_TopupDeltails = FunProFormXML();
    //    ObjTLEWCInsRow.Txn_ID = 10;
    //    ObjS3G_TLEWCInsDataTable.AddS3G_LOAND_InsTLEWCTopupRow(ObjTLEWCInsRow);

    //    byte[] ObjS3G_TLEWCInsDataTables = ClsPubSerialize.Serialize(ObjS3G_TLEWCInsDataTable, SerMode);
    //    objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
    //    gvNewStructure.FunPubFormXml(enumGridType.TemplateGrid);


    //    if (intTLEWCID > 0)
    //    {
    //        intErrCode = objLoanAdmin_MasterClient.FunPubModifyTLEWC(SerMode, ObjS3G_TLEWCInsDataTables);
    //    }
    //    else
    //    {
    //        intErrCode = objLoanAdmin_MasterClient.FunPubCreateTLEWCIns(out strTLEWC_No, SerMode, ObjS3G_TLEWCInsDataTables);
    //    }
    //    if (intErrCode == 0)
    //    {
    //        if (intTLEWCID > 0)
    //        {
    //            strKey = "Modify";
    //            strAlert = strAlert.Replace("__ALERT__", "TopUp Details updated successfully");
    //        }
    //        else
    //        {
    //            strAlert = "TopUp Details " + strTLEWC_No + " added successfully";
    //            strAlert += @"\n\nWould you like to add one more record?";
    //            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
    //            strRedirectPageView = "";
    //        }

    //    }
    //    else if (intErrCode == -1)
    //    {
    //        if (intTLEWCID == 0)
    //        {
    //            strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
    //            strRedirectPageView = "";
    //        }
    //    }
    //    else if (intErrCode == -2)
    //    {
    //        if (intTLEWCID == 0)
    //        {
    //            strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
    //            strRedirectPageView = "";
    //        }

    //    }
    //    else if (intErrCode == 1)
    //    {
    //        strAlert = strAlert.Replace("__ALERT__", "TopUp Details already exists");
    //        strRedirectPageView = "";
    //    }
    //    else if (intErrCode == 2)
    //    {
    //        Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create TopUp");
    //    }
    //    //else if (intErrCode == -1)
    //    //{
    //    //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create TopUp");
    //    //}

    //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
    //    lblErrorMessage.Text = string.Empty;
    //    objLoanAdmin_MasterClient.Close();
    //    //}
    //    //WC
    //    //else
    //    //{
    //    //SerializationMode SerMode = SerializationMode.Binary;
    //    //LoanAdminMgtServices.S3G_LOAND_InsWCTopupDataTable ObjS3G_TLEWCInsDataTable = new LoanAdminMgtServices.S3G_LOAND_InsWCTopupDataTable();
    //    //LoanAdminMgtServices.S3G_LOAND_InsWCTopupRow ObjTLEWCInsRow;
    //    //ObjTLEWCInsRow = ObjS3G_TLEWCInsDataTable.NewS3G_LOAND_InsWCTopupRow();
    //    //ObjTLEWCInsRow.TLEWCTopup_ID = intTLEWCID;
    //    //ObjTLEWCInsRow.Company_ID = intCompanyId;
    //    //ObjTLEWCInsRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
    //    //ObjTLEWCInsRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
    //    //ObjTLEWCInsRow.PANum = ddlMLA.SelectedItem.Text;
    //    ////"2010-2011/FLMLA/201";
    //    ////
    //    ////ObjTLEWCInsRow.SANum = ddlSLA.SelectedItem.Text;
    //    ////"2010-2011/SLA/7";
    //    ////

    //    //if (ddlSLA.Items.Count == 0)
    //    //{
    //    //    ObjTLEWCInsRow.SANum = Convert.ToString(ViewState["dum"]);
    //    //}
    //    //else
    //    //{
    //    //    ObjTLEWCInsRow.SANum = Convert.ToString(ddlSLA.SelectedItem.Text);
    //    //}
    //    //ObjTLEWCInsRow.Customer_ID = Convert.ToInt32(txtCustID.Text);
    //    //ObjTLEWCInsRow.TLE_WC_Date = Utility.StringToDate(txtDate.Text);
    //    //ObjTLEWCInsRow.Topup_Status_Type_Code = 9;
    //    //ObjTLEWCInsRow.Topup_Status_Code = 1;
    //    //ObjTLEWCInsRow.Current_Finance_Required = Convert.ToInt32(txtCurrent.Text);
    //    //ObjTLEWCInsRow.Start_Instalment = 0;
    //    //ObjTLEWCInsRow.Instalment_Amount = 0;

    //    //ObjTLEWCInsRow.Account_Link_Key = 10;
    //    //ObjTLEWCInsRow.Created_By = intUserId;
    //    //ObjTLEWCInsRow.Created_On = DateTime.Now;
    //    //ObjTLEWCInsRow.Modified_By = intUserId;
    //    //ObjTLEWCInsRow.Modified_On = DateTime.Now;
    //    //ObjTLEWCInsRow.Txn_ID = 10;
    //    //ObjS3G_TLEWCInsDataTable.AddS3G_LOAND_InsWCTopupRow(ObjTLEWCInsRow);

    //    //byte[] ObjS3G_TLEWCInsDataTables = ClsPubSerialize.Serialize(ObjS3G_TLEWCInsDataTable, SerMode);
    //    //objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
    //    //if (intTLEWCID > 0)
    //    //{
    //    //    intErrCode = objLoanAdmin_MasterClient.FunPubModifyWC(SerMode, ObjS3G_TLEWCInsDataTables);
    //    //}
    //    //else
    //    //{
    //    //    intErrCode = objLoanAdmin_MasterClient.FunPubCreateWCIns(out strTLEWC_No, SerMode, ObjS3G_TLEWCInsDataTables);
    //    //}
    //    //if (intErrCode == 0)
    //    //{
    //    //    if (intTLEWCID > 0)
    //    //    {
    //    //        strKey = "Modify";
    //    //        strAlert = strAlert.Replace("__ALERT__", "TopUp Details updated successfully");
    //    //    }
    //    //    else
    //    //    {
    //    //        strAlert = "TopUp Details " + strTLEWC_No + " added successfully";
    //    //        strAlert += @"\n\nWould you like to add one more record?";
    //    //        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
    //    //        strRedirectPageView = "";
    //    //    }

    //    //}
    //    //else if (intErrCode == -1)
    //    //{
    //    //    if (intTLEWCID == 0)
    //    //    {
    //    //        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
    //    //        strRedirectPageView = "";
    //    //    }
    //    //}
    //    //else if (intErrCode == -2)
    //    //{
    //    //    if (intTLEWCID == 0)
    //    //    {
    //    //        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
    //    //        strRedirectPageView = "";
    //    //    }

    //    //}
    //    //else if (intErrCode == 1)
    //    //{
    //    //    strAlert = strAlert.Replace("__ALERT__", "TopUp Details already exists");
    //    //    strRedirectPageView = "";
    //    //}
    //    //else if (intErrCode == 2)
    //    //{
    //    //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create TopUp");
    //    //}
    //    ////else if (intErrCode == -1)
    //    ////{
    //    ////    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create TopUp");
    //    ////}
    //    //else if (intErrCode == 3)
    //    //{
    //    //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create TopUp");
    //    //}

    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
    //    //lblErrorMessage.Text = string.Empty;
    //    //objLoanAdmin_MasterClient.Close();

    //    // }
    //}

    #endregion [Not Used Function]

    #endregion [Function's]

    protected void gvCollateralDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

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

        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "079");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }
}