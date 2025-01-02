#region Page Header

/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Load Admin
/// Screen Name			: Account Activation
/// Created By			: Saishri
/// Created Date		: 17-Aug-2010
/// Purpose	            : To Activate the Accounts
/// Modified By         : Thangam M
/// Modified Date       : --
///  
///

#endregion

#region Namespaces

using System;
using S3GBusEntity;
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
using System.Linq;
using AjaxControlToolkit;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

#endregion

public partial class S3GLOANADAccountActivation_Add : ApplyThemeForProject
{
    #region Declaration

    int intCompanyID, intUserID = 0, intUserLevel = 0, intUserCheck = 0;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    string strDateFormat = string.Empty;
    string strAccountActivation = string.Empty;
    string strDeliveryOrder = string.Empty;
    bool blCanModify = true;
    StringBuilder strImage = new StringBuilder();
    StringBuilder strProforma = new StringBuilder();
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string strErrMsg = string.Empty;
    DataSet dsprint = new DataSet();
    //User authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    DataSet dSdebitnote = new DataSet();
    //ReportDocument rpd = new ReportDocument();
    //Code end

    ContractMgtServicesReference.ContractMgtServicesClient objAccActivation_Client;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable objS3G_LOANAD_ActivationDataTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationRow objS3G_LOANAD_ActivationDataRow = null;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACAC";

    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLOANADAccountActivation_Add.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACAC';";

    string strNewWin = " window.showModalDialog('../LoanAdmin/S3GLoanAdInvoiceVendor_Add.aspx";
    string NewWinAttributes = "', 'Invoice Vendor Details', 'dialogwidth:800px;dialogHeight:950px;');";

    string strNewWinProforma = " window.showModalDialog('../Origination/S3GORGProforma_Add.aspx";
    string NewWinAttributesProforma = "', 'Proforma Details', 'dialogwidth:800px;dialogHeight:550px;');";

    public static S3GLOANADAccountActivation_Add obj_Page;
    #endregion

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (rpd != null)
        //{
        //    rpd.Close();
        //    rpd.Dispose();
        //}
    }

    #region Page Loading

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            obj_Page = this;
            //Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender2.Format = strDateFormat;
            CEAccountCreationDate.Format = strDateFormat;
            //End
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end
            strMode = Request.QueryString["qsMode"];
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            intUserLevel = ObjUserInfo.ProUserLevelIdRW;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strAccountActivation = fromTicket.Name;
            }

            ddlBranch.TextBox.Attributes.Add("onchange", "fnClearAllTab(false);");

            #region " WF INITIATION"
            ProgramCode = "075";
            #endregion

            if (!IsPostBack)
            {
                FunPubSetIndex(1);
                
                if (Request.QueryString["qsMode"] == "C")
                {
                    FunProPopulateBranchList();
                    FunProPopulateLOBList();
                    FunProPopulateinterimList();
                    ddlLOB.Focus();
                }


                txtActivationDate.Attributes.Add("readonly", "readonly");
                txtAccountCreationDate.Attributes.Add("readonly", "readonly");
                ViewState["EnableTab2"] = "false";
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (Request.QueryString["qsMode"] == "Q")
                {
                    FunProLoadTaxTypes();
                    FunProActiavtedAccountForModification(strAccountActivation);
                    FunPriDisableControls(-1);                    
                }
                else if (Request.QueryString["qsMode"] == "M")
                {
                    FunProActiavtedAccountForModification(strAccountActivation);
                    // Modified by Thangam M on 28/Nov/2011 based on the UAT bug
                    if (blCanModify)
                    {
                        FunPriDisableControls(1);
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this.Page, @"Cannot modify or revoke the Activation.\n Payment has been processed.");
                        FunPriDisableControls(-1);
                    }
                }
                else if (PageMode == PageModes.WorkFlow)  // WORK FLOW
                {
                    try
                    {
                        PreparePageForWorkFlowLoad(sender, e);
                    }
                    catch (Exception ex)
                    {
                        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                        Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
                    }
                }
                else
                {
                    FunPriDisableControls(0);
                }


            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }
    #region Workflow Methods
    /// <summary>
    /// Workflow Function
    /// </summary>
    private void PreparePageForWorkFlowLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            // Get The IDVALUE from Document Sequence #
            DataRow HeaderValues = GetHeaderDetailsFromDOCNo(WFSessionValues.PANUM, ProgramCode);
            if (WFSessionValues.SANUM != "" && WFSessionValues.SANUM.ToUpper() != WFSessionValues.PANUM.ToUpper() + "DUMMY")
            {
                rbtnType.SelectedIndex = 1;
                rbtnType_SelectedIndexChanged(sender, e);
            }
            ddlLOB.SelectedValue = HeaderValues["LOB_ID"].ToString();
            FunLOBRelatedDetails();
            ddlBranch.SelectedValue = HeaderValues["Location_Id"].ToString();
            FunLoadBranchRelatedDetails();

            ddlMLA.SelectedValue = WFSessionValues.PANUM;
            LoadMLARelatedDetails();
            // if (ddlBranch.Items.Count > 0) ddlBranch.ClearDropDownList();
            //if (ddlLOB.Items.Count > 0) ddlLOB.ClearDropDownList();
            if (ddlMLA.Items.Count > 0) ddlMLA.ClearDropDownList();
            //if (ddlSLA.Items.Count > 0) ddlSLA.ClearDropDownList();
        }
    }
    #endregion


    #endregion

    #region Page Events

    #region Save Clear Cancel Go

    protected void btnSave_Click(object sender, EventArgs e)
    {
        objAccActivation_Client = new ContractMgtServicesReference.ContractMgtServicesClient();
        try
        {
            //if (Page.IsValid)
            //{
            if (Convert.ToInt32(ViewState["Created_By"]) != intUserID)
            {

                objS3G_LOANAD_ActivationDataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable();
                objS3G_LOANAD_ActivationDataRow = objS3G_LOANAD_ActivationDataTable.NewS3G_LOANAD_AccountActivationRow();
                objS3G_LOANAD_ActivationDataRow.Account_Activation_Number = strAccountActivation;
                objS3G_LOANAD_ActivationDataRow.Company_ID = intCompanyID;
                objS3G_LOANAD_ActivationDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                objS3G_LOANAD_ActivationDataRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
                objS3G_LOANAD_ActivationDataRow.Created_By = intUserID;
                objS3G_LOANAD_ActivationDataRow.PANum = ddlMLA.SelectedValue;
                if (Convert.ToInt32(ddlInterim.SelectedValue.ToString()) > 0)
                    objS3G_LOANAD_ActivationDataRow.interim_type = Convert.ToInt32(ddlInterim.SelectedValue.ToString());
                if (txtinterimrate.Text != "")
                    objS3G_LOANAD_ActivationDataRow.Interim_Rate = txtinterimrate.Text.ToString();
                if (txtInterimamount.Text != "")
                    objS3G_LOANAD_ActivationDataRow.Interim_amount = txtInterimamount.Text.ToString();
                if (ddlSLA.SelectedValue == "0" || ddlSLA.SelectedValue == "")
                    objS3G_LOANAD_ActivationDataRow.SANum = ddlMLA.SelectedValue + "DUMMY";
                else
                    objS3G_LOANAD_ActivationDataRow.SANum = ddlSLA.SelectedValue;

                objS3G_LOANAD_ActivationDataRow.MLAStatus = Convert.ToInt32(ViewState["MLAStatus"]);
                if (strAccountActivation == string.Empty)
                {
                    objS3G_LOANAD_ActivationDataRow.IsModify = 0;
                    objS3G_LOANAD_ActivationDataRow.IsRevoke = 0;
                }
                else
                {
                    objS3G_LOANAD_ActivationDataRow.IsModify = 1;
                    objS3G_LOANAD_ActivationDataRow.IsRevoke = 0;
                }
                objS3G_LOANAD_ActivationDataRow.Account_Activated_Date = Utility.StringToDate(txtActivationDate.Text);

                //TL /TLE Changes 
                //   if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE")))
                //&& ViewState["Repayment_Code"].ToString() != "5" && ViewState["Return_Pattern"].ToString() != "6")//Product // not a principal
                //   {
                //       if (ViewState["RepaymentStructure"] != null)
                //           objS3G_LOANAD_ActivationDataRow.XMLRepayStruct = ((DataTable)ViewState["RepaymentStructure"]).FunPubFormXml(true);
                //       else
                //       {
                //           Utility.FunShowAlertMsg(this, "Generate Repayment Structure");
                //           return;
                //       }
                //       if (ViewState["DtRepayGrid"] != null)
                //           objS3G_LOANAD_ActivationDataRow.XMLRepayDtls = ((DataTable)ViewState["DtRepayGrid"]).FunPubFormXml(true);
                //       if (ViewState["dtOutflow"] != null)
                //           objS3G_LOANAD_ActivationDataRow.XMLOutFlow = ((DataTable)ViewState["dtOutflow"]).FunPubFormXml(true);
                //       if (!string.IsNullOrEmpty(txtAccIRR.Text))
                //           objS3G_LOANAD_ActivationDataRow.AccountIRR = txtAccIRR.Text;
                //       if (!string.IsNullOrEmpty(txtBusinessIRR.Text))
                //           objS3G_LOANAD_ActivationDataRow.BusinessIRR = txtBusinessIRR.Text;
                //       if (!string.IsNullOrEmpty(txtCompanyIRR.Text))
                //           objS3G_LOANAD_ActivationDataRow.CompanyIRR = txtCompanyIRR.Text;

                //       if (ViewState["dtRepayDetailsOthers"] != null)
                //           objS3G_LOANAD_ActivationDataRow.XMLRepayDetailsOthers = ((DataTable)ViewState["dtRepayDetailsOthers"]).FunPubFormXml(true);

                //   }



                objS3G_LOANAD_ActivationDataTable.AddS3G_LOANAD_AccountActivationRow(objS3G_LOANAD_ActivationDataRow);

                intErrCode = objAccActivation_Client.FunPubCreateAccountActivation(out strErrMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_ActivationDataTable, ObjSerMode));

                if (intErrCode == 0 && strAccountActivation == string.Empty)
                {
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strErrMsg, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 19/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow not assigned";
                        }
                        ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode, strAlert);
                        return;
                    }
                    else
                    {
                        strAlert = "Rental Schedule activated successfully";
                        strAlert += @"\n\nActivation Number - " + strErrMsg;
                        strAlert += @"\n\nWould you like to activate one more Rental Schedule?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 19/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                    }
                }
                else if (intErrCode == -1)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);
                }
                else if (intErrCode == -2)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);
                }
                else if (intErrCode == 0 && strAccountActivation != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule Activation details updated successfully');" + strRedirectPageView, true);
                    //Code Added by Ganapathy on 19/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule Activation cannot be done in a closed month');" + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 7)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule has been activated already.');" + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 98)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Mapped Invoices and its Rental schedule are not closed.');" + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
                else
                {
                    Utility.FunShowValidationMsg(this.Page, "ACT", intErrCode, strErrMsg, false);
                    return;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule Activation cannot be done by the same user who created the Rental Schedule');" + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
                return;
            }

        }
        //}
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
        finally
        {
            objAccActivation_Client.Close();
        }
        //if (objAccActivation_Client != null)
        //{
        //    objAccActivation_Client.Close();
        //    objS3G_LOANAD_ActivationDataTable.Dispose();
        //}

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            // wf cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else
                Response.Redirect(strRedirectPage, false);
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {


            if (Procparam != null)
                Procparam.Clear();
            else

                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@account_activated_no", strAccountActivation);
            Session["Is_Print"] = "1";
            Session["ProcParam"] = Procparam;
            string strScipt = "window.open('../LoanAdmin/S3G_RPT_Interim_ReportPage.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Interim Rent Billing", strScipt, true);

        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }


    protected void btnCashflow_Click(object sender, EventArgs e)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else

                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@account_activated_no", strAccountActivation);
            dsprint = Utility.GetDataset("S3G_Fund_Get_InterimPrint", Procparam);
            Session["Is_Print"] = "2";
            Session["ProcParam"] = Procparam;
            if (dsprint.Tables[1].Rows.Count > 0)
            {
                string strScipt = "window.open('../LoanAdmin/S3G_RPT_Interim_ReportPage.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Interim Rent Billing", strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No data found");
                return;
            }

        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        objAccActivation_Client = new ContractMgtServicesReference.ContractMgtServicesClient();

        try
        {
            //if (Page.IsValid)
            //{
            if (Convert.ToInt32(ViewState["Activated_By"]) != intUserID)
            {

                objS3G_LOANAD_ActivationDataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable();

                objS3G_LOANAD_ActivationDataRow = objS3G_LOANAD_ActivationDataTable.NewS3G_LOANAD_AccountActivationRow();
                objS3G_LOANAD_ActivationDataRow.PANum = ddlMLA.SelectedValue;
                if (ddlSLA.SelectedValue == "0" || ddlSLA.SelectedValue == "")
                    objS3G_LOANAD_ActivationDataRow.SANum = ddlMLA.SelectedValue + "DUMMY";
                else
                    objS3G_LOANAD_ActivationDataRow.SANum = ddlSLA.SelectedValue;
                objS3G_LOANAD_ActivationDataRow.MLAStatus = Convert.ToInt32(ViewState["MLAStatus"]);
                objS3G_LOANAD_ActivationDataRow.IsModify = 0;
                objS3G_LOANAD_ActivationDataRow.IsRevoke = 1;
                objS3G_LOANAD_ActivationDataRow.Account_Activation_Number = strAccountActivation;
                objS3G_LOANAD_ActivationDataRow.Company_ID = intCompanyID;
                objS3G_LOANAD_ActivationDataRow.Created_By = intUserID;
                objS3G_LOANAD_ActivationDataRow.Account_Activated_Date = Utility.StringToDate(txtActivationDate.Text);
                objS3G_LOANAD_ActivationDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue.ToString());
                objS3G_LOANAD_ActivationDataRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue.ToString());
                objS3G_LOANAD_ActivationDataTable.AddS3G_LOANAD_AccountActivationRow(objS3G_LOANAD_ActivationDataRow);
                intErrCode = objAccActivation_Client.FunPubCreateAccountActivation(out strErrMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_ActivationDataTable, ObjSerMode));
                if (intErrCode == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule has been revoked sucessfully');" + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule Revoke cannot be done in a closed month');" + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 6)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Cannot Revoke Rental Schedule. One or more Sub Accounts are active for this Prime Account');" + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Migrated Rental Schedule Cannot Revoke');", true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 11)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Note has been Approved. Cannot Revoke Rental Schedule');", true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 12)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Transaction has been done. Cannot Revoke Rental Schedule');", true);
                    lblErrorMessage.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule cannot be revoked by the same user who activated the Rental Schedule');" + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            // }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
        finally
        {
            objAccActivation_Client.Close();
            //if (objAccActivation_Client != null)
            //{
            //    objAccActivation_Client.Close();
            //    objS3G_LOANAD_ActivationDataTable.Dispose();
            //}
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //rbtnType.Items[1].Enabled = true;
            rbtnType.SelectedValue = "0";
            //ddlLOB.SelectedIndex = -1;
            //ddlBranch.SelectedIndex = -1;
            ddlBranch.Clear();
            ddlMLA.Items.Clear();
            ddlSLA.Items.Clear();
            FunProClearMainTabControls();
            ddlLOB.Focus();
            FunProToggleSLA(false);

            ScriptManager.RegisterStartupScript(this, GetType(), "te", "fnClearAllTab(false);", true);
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void hyplnkViewPost_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvPDDT", strFieldAtt);
            Label lblPath = (Label)gvPDDT.Rows[gRowIndex].FindControl("lblPath");

            string strFileName = lblPath.Text.Replace("\\", "/").Trim();
            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void hyplnkViewPre_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvPRDDT", strFieldAtt);
            Label lblPath = (Label)gvPRDDT.Rows[gRowIndex].FindControl("lblPath");

            string strFileName = lblPath.Text.Replace("\\", "/").Trim();
            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region Exporting

    protected void btnExportJV_Click(object sender, EventArgs e)
    {
        try
        {
            FunProExport(gvSystemJournal, "JournalVoucher - " + ddlMLA.SelectedValue);
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void btnXLPorting_Click(object sender, EventArgs e)
    {
        try
        {
            FunProExport(grvAmortization, "Amortization - " + ddlMLA.SelectedValue);
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void anchAcct_serverclick(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedValue != "0" && ddlBranch.SelectedValue != "0" && ddlMLA.SelectedValue != "0")
            {
                DataTable dtTable = new DataTable();
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                // Procparam.Add("@PANum", ddlMLA.SelectedValue);
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                Procparam.Add("@PANum", ddlMLA.SelectedValue);
                if (ddlSLA.SelectedValue == string.Empty || ddlSLA.SelectedValue == "0")
                    Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
                else
                    Procparam.Add("@SANum", ddlSLA.SelectedValue);
                dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_AccountViewFromActivation, Procparam);

                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(dtTable.Rows[0]["ID"].ToString(), false, 0);

                //string strScipt = "window.showModalDialog('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&IsFromAccount=Y&qsMode=Q', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                //string strScipt = "window.open('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&IsFromAccount=Y&qsMode=Q', 'Invoice Vendor Details', 'dialogwidth:900px;dialogHeight:900px;');";
                string strScipt = "window.open('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&IsFromAccount=Y&qsMode=Q', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=no,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }
            else
            {
                lblErrorMessage.Text = "Please select Line of Business, Location and a Rental Schedule Number to view";
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    #endregion

    #region Gridview Events

    protected void grvFile_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string strPath = e.CommandArgument.ToString();
            string strApplicationPath = "/Data/Proforma/";
            strPath = strApplicationPath + strPath;
            FileInfo fileName = new FileInfo(Server.MapPath(".." + strPath));

            if (!fileName.Exists)
            {
                Utility.FunShowAlertMsg(this.Page, "File  does not exists");
                return;
            }
            else
            {
                if (e.CommandName == "Show")
                {
                    strPath = strApplicationPath + strPath;
                    string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strPath + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
                }
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void grvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string strPath = e.CommandArgument.ToString();
            string strApplicationPath = "/Data/Invoice/";
            strPath = strApplicationPath + strPath;
            FileInfo fileName = new FileInfo(Server.MapPath(".." + strPath));
            if (!fileName.Exists)
            {
                Utility.FunShowAlertMsg(this.Page, "File  does not exists");
                return;
            }
            else
            {

                if (e.CommandName == "Show")
                {
                    string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strPath + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
                }
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void grvAmortization_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDate = (Label)e.Row.FindControl("lblDate");
                Label lblInstallment = (Label)e.Row.FindControl("lblInstallment");
                Label lblPrincipal = (Label)e.Row.FindControl("lblPrincipal");
                Label lblInterest = (Label)e.Row.FindControl("lblInterest");
                Label lblInsurance = (Label)e.Row.FindControl("lblInsurance");
                Label lblOthers = (Label)e.Row.FindControl("lblOthers");
lblDate.Text = Utility.StringToDate(lblDate.Text).ToString(strDateFormat);

               



                //lblDate.Text = DateTime.Parse(lblDate.Text, CultureInfo.CurrentCulture).ToString(strDateFormat);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable dt = ((DataTable)grvAmortization.DataSource);
                ((Label)e.Row.FindControl("lblTotalInstallment")).Text = Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum(Installment)", null)).ToString();
                ((Label)e.Row.FindControl("lblTotalPrincipal")).Text = Math.Round((Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum([Principal Portion])", null))), 2).ToString("0.00");
                ((Label)e.Row.FindControl("lblTotalInterest")).Text = Math.Round(Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum([Interest Portion])", null)), 2).ToString("0.00");
                ((Label)e.Row.FindControl("lblTotalInsurance")).Text = ((DataTable)grvAmortization.DataSource).Compute("Sum(Insurance)", null) == DBNull.Value ? "" : Math.Round(Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum(Insurance)", null))).ToString("0.00");
                ((Label)e.Row.FindControl("lblTotalOthers")).Text = ((DataTable)grvAmortization.DataSource).Compute("Sum(Others)", null) == DBNull.Value ? "" : Math.Round(Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum(Others)", null))).ToString("0.00");
                ((Label)e.Row.FindControl("lblTotalTax")).Text = ((DataTable)grvAmortization.DataSource).Compute("Sum(rental_Tax)", null) == DBNull.Value ? "" : Math.Round(Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum(rental_Tax)", null))).ToString("0.00");
                ((Label)e.Row.FindControl("lblTotalST")).Text = ((DataTable)grvAmortization.DataSource).Compute("Sum(ST)", null) == DBNull.Value ? "" : Math.Round(Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum(ST)", null))).ToString("0.00");
                ((Label)e.Row.FindControl("lblTotAMF")).Text = ((DataTable)grvAmortization.DataSource).Compute("Sum(AMF)", null) == DBNull.Value ? "" : Math.Round(Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum(AMF)", null))).ToString("0.00");
                ((Label)e.Row.FindControl("lblTotAMFPrincipal")).Text = ((DataTable)grvAmortization.DataSource).Compute("Sum(AMF_Principal)", null) == DBNull.Value ? "" : Math.Round(Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum(AMF_Principal)", null))).ToString("0.00");
                ((Label)e.Row.FindControl("lblTotAMFInterst")).Text = ((DataTable)grvAmortization.DataSource).Compute("Sum(AMF_Interest)", null) == DBNull.Value ? "" : Math.Round(Convert.ToDecimal(((DataTable)grvAmortization.DataSource).Compute("Sum(AMF_Interest)", null))).ToString("0.00");
                //Label lbl = grvAmortization.Rows[0].FindControl("lblInstallment") as Label;
                //string strlblInstallment =Convert.ToString("");
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void grvInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblslNo = (Label)e.Row.FindControl("lblslNo");
                lblslNo.Text = (e.Row.RowIndex + 1).ToString();

                Label lblID = (Label)e.Row.FindControl("lblID");
                //Button BtnView = (Button)e.Row.FindControl("BtnView");
                ImageButton BtnView = (ImageButton)e.Row.FindControl("BtnView");
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblID.Text, false, 0);
                BtnView.Attributes.Add("onclick", strNewWin + "?IsFromAccount=Y&qsMode=Q" + "&qsViewId=" + FormsAuthentication.Encrypt(Ticket) + NewWinAttributes);
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void grvFile_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblslNo = (Label)e.Row.FindControl("lblslNo");
                lblslNo.Text = (e.Row.RowIndex + 1).ToString();

                Label lblID = (Label)e.Row.FindControl("lblID");
                //Button BtnView = (Button)e.Row.FindControl("BtnView");
                ImageButton BtnView = (ImageButton)e.Row.FindControl("BtnView");
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(lblID.Text, false, 0);
                BtnView.Attributes.Add("onclick", strNewWinProforma + "?IsFromAccount=Y&qsMode=Q" + "&qsViewId=" + FormsAuthentication.Encrypt(Ticket) + NewWinAttributesProforma);
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void gvSystemJournal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDocDate = (Label)e.Row.FindControl("lblDocDate");
                Label lblValueDate = (Label)e.Row.FindControl("lblValueDate");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                //lblDocDate.Text = Utility.StringToDate(lblDocDate.Text).ToString(strDateFormat);
                //lblValueDate.Text = Utility.StringToDate(lblValueDate.Text).ToString(strDateFormat);

                lblDocDate.Text = DateTime.Parse(lblDocDate.Text, CultureInfo.CurrentCulture).ToString(strDateFormat);
                lblValueDate.Text = DateTime.Parse(lblValueDate.Text, CultureInfo.CurrentCulture).ToString(strDateFormat);

                if (lblStatus.Text == "Revoked")
                {
                    e.Row.BackColor = System.Drawing.Color.WhiteSmoke;
                }
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void gvPDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label txtColletedDate = (Label)e.Row.FindControl("txtColletedDate");
                Label txtScannedDate = (Label)e.Row.FindControl("txtScannedDate");
                Label txtScannedBy = (Label)e.Row.FindControl("txtScannedBy");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                ImageButton hyplnkView = (ImageButton)e.Row.FindControl("hyplnkView");
                Label lblCanView = (Label)e.Row.FindControl("lblCanView");
                txtRemarks.Attributes.Add("readonly", "readonly");

                if (txtColletedDate.Text.Contains("1900"))
                {
                    txtColletedDate.Text = "";
                }
                if (txtScannedDate.Text.Contains("1900"))
                {
                    txtScannedDate.Text = "";
                }

                if (lblCanView.Text.Trim() == "0")
                {
                    hyplnkView.Visible = txtScannedBy.Visible = txtScannedDate.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvPRDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label txtColletedDate = (Label)e.Row.FindControl("txtColletedDate");
                Label txtScannedDate = (Label)e.Row.FindControl("txtScannedDate");
                Label txtScannedBy = (Label)e.Row.FindControl("txtScannedBy");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                ImageButton hyplnkViewPre = (ImageButton)e.Row.FindControl("hyplnkViewPre");
                Label lblCanView = (Label)e.Row.FindControl("lblCanView");
                txtRemarks.Attributes.Add("readonly", "readonly");

                if (txtColletedDate.Text.Contains("1900"))
                {
                    txtColletedDate.Text = "";
                }
                if (txtScannedDate.Text.Contains("1900"))
                {
                    txtScannedDate.Text = "";
                }

                if (lblCanView.Text.Trim() == "0")
                {
                    hyplnkViewPre.Visible = txtScannedBy.Visible = txtScannedDate.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region DDL Events

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FunProPopulateBranchList();
        FunLOBRelatedDetails();
    }

    protected void ddlInterim_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInterim.SelectedValue == "1" || ddlInterim.SelectedValue == "2")
        {
            txtinterimrate.ReadOnly = false;
            rfvInterimrate.Enabled = true;
            rfvInterimrate.Visible = true;
        }
        else
        {
            txtinterimrate.ReadOnly = true;
            rfvInterimrate.Enabled = false;
            rfvInterimrate.Visible = false;
        }
        if (ddlInterim.SelectedValue == "11")
        {
            txtInterimamount.ReadOnly = false;
            rfvInterimamt.Enabled = true;
            rfvInterimamt.Visible = true;
        }
        else
        {
            txtInterimamount.ReadOnly = true;
            rfvInterimamt.Enabled = false;
            rfvInterimamt.Visible = false;
        }

    }

    void FunLOBRelatedDetails()
    {
        try
        {
            FunProClearMainTabControls();
            ddlBranch.Clear();
            rbtnType.Items[1].Enabled = true;
            FunProGetGloblaMLASLA();

            if (ddlMLA.Items.Count > 0)
                ddlMLA.Items.Clear();
            lblErrorMessage.Text = string.Empty;
            //if (ddlLOB.SelectedIndex > 0)
            //{
            //FunProGetGlobalParameterDetails();
            //}

            ddlLOB.Focus();
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunLoadBranchRelatedDetails();
    }

    void FunLoadBranchRelatedDetails()
    {
        try
        {
            FunProPopulatePANum(ViewState["MLAStatus"].ToString());
            FunProClearMainTabControls();
            ddlSLA.Items.Clear();
            lblErrorMessage.Text = string.Empty;

            ddlBranch.Focus();
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadMLARelatedDetails();
    }

    protected void ddlTaxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlTaxType.SelectedIndex > 0)
        //{
        //    btnDebitNotes.Visible = true;
        //    btnDebitNotes.Enabled = true;
        //}
        //else
        //{
        //    btnDebitNotes.Visible = false;
        //}
    }

    protected void btnDebitNote_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlTaxType.SelectedValue) == 0)
            {
                Utility.FunShowAlertMsg(this, "Select a Tax Type");
                return;
            }
            else
            {
                if (Convert.ToInt32(ddlTaxType.SelectedValue) == 1 || Convert.ToInt32(ddlTaxType.SelectedValue) == 2 || Convert.ToInt32(ddlTaxType.SelectedValue) == 3 || Convert.ToInt32(ddlTaxType.SelectedValue) == 4)
                {
                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();

                    Procparam.Add("@ActivationNumber", strAccountActivation);
                    Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
                    Procparam.Add("@TaxType", ddlTaxType.SelectedValue);
                    dSdebitnote = Utility.GetDataset("S3G_LAD_RSDebitNotePrint", Procparam);
                    if ((Convert.ToInt32(ddlTaxType.SelectedValue) == 1 && Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["TAXTYPE_ID"]) == 1))
                    {
                        Utility.FunShowAlertMsg(this, "No Debit note to display");
                        return;
                    }
                    else if ((Convert.ToInt32(ddlTaxType.SelectedValue) == 2 && Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["TAXTYPE_ID"]) == 2))
                    {
                        Utility.FunShowAlertMsg(this, "No Debit note to display");
                        return;
                    }
                    else if ((Convert.ToInt32(ddlTaxType.SelectedValue) == 3 && Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["TAXTYPE_ID"]) == 3))
                    {
                        Utility.FunShowAlertMsg(this, "No Debit note to display");
                        return;
                    }
                    else if ((Convert.ToInt32(ddlTaxType.SelectedValue) == 4 && Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["TAXTYPE_ID"]) == 4))
                    {
                        Utility.FunShowAlertMsg(this, "No Debit note to display");
                        return;
                    }
                    else
                    {
                        if (Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["Amount"]) == 0)
                        {
                            Utility.FunShowAlertMsg(this, "No Debit note to display");
                            return;
                        }
                        else
                        {
                            if (dSdebitnote.Tables[0].Rows.Count > 0)
                            {
                                Guid objGuid;
                                objGuid = Guid.NewGuid();
                                

                                //rpd.Load(Server.MapPath("RSDebitNote.rpt"));

                                //rpd.SetDataSource(dSdebitnote.Tables[0]);

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
                                Utility.FunShowAlertMsg(this, "No data to export");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();

                    Procparam.Add("@ActivationNumber", strAccountActivation);
                    Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
                    Procparam.Add("@TaxType", ddlTaxType.SelectedValue);
                    dSdebitnote = Utility.GetDataset("S3G_LAD_RSDebitNotePrint", Procparam);
                    if ((Convert.ToInt32(ddlTaxType.SelectedValue) == 5 && Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["TAXTYPE_ID"]) == 5))
                    {
                        Utility.FunShowAlertMsg(this, "No Debit note to display");
                        return;
                    }
                    else if ((Convert.ToInt32(ddlTaxType.SelectedValue) == 6 && Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["TAXTYPE_ID"]) == 6))
                    {
                        Utility.FunShowAlertMsg(this, "No Debit note to display");
                        return;
                    }
                    else if ((Convert.ToInt32(ddlTaxType.SelectedValue) == 7 && Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["TAXTYPE_ID"]) == 7))
                    {
                        Utility.FunShowAlertMsg(this, "No Debit note to display");
                        return;
                    }
                    else if ((Convert.ToInt32(ddlTaxType.SelectedValue) == 8 && Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["TAXTYPE_ID"]) == 8))
                    {
                        Utility.FunShowAlertMsg(this, "No Debit note to display");
                        return;
                    }
                    else
                    {
                        if (Convert.ToInt32(dSdebitnote.Tables[1].Rows[0]["Amount"]) == 0)
                        {
                            Utility.FunShowAlertMsg(this, "No Debit note to display");
                            return;
                        }
                        else
                        {
                            if (dSdebitnote.Tables[0].Rows.Count > 0)
                            {
                                if (ddlTaxType.SelectedValue == "5")
                                {
                                    grvInvv.Columns[5].HeaderText = "Purchase Tax";
                                }
                                else if (ddlTaxType.SelectedValue == "6")
                                {
                                    grvInvv.Columns[5].HeaderText = "Reverse Charge Tax";
                                }
                                else if (ddlTaxType.SelectedValue == "7")
                                {
                                    grvInvv.Columns[5].HeaderText = "Entry Tax";
                                }
                                else if (ddlTaxType.SelectedValue == "8")
                                {
                                    grvInvv.Columns[5].HeaderText = "LBT";
                                }
                                grvInvv.DataSource = dSdebitnote.Tables[0];
                                grvInvv.DataBind();

                                Label lblBillamtf = (grvInvv).FooterRow.FindControl("lblBillamtf") as Label;
                                lblBillamtf.Text = Convert.ToDecimal(dSdebitnote.Tables[0].Compute("SUM(TOT_BILL_AMT)", "")).ToString();

                                Label lblTaxf = (grvInvv).FooterRow.FindControl("lblTaxf") as Label;
                                lblTaxf.Text = Convert.ToDecimal(dSdebitnote.Tables[0].Compute("SUM(Amount)", "")).ToString();

                                string attachment = "attachment; filename=RS_Debit_Note_Annexure.xls";
                                Response.ClearContent();
                                Response.AddHeader("content-disposition", attachment);
                                Response.ContentType = "application/vnd.xls";
                                StringWriter sw = new StringWriter();
                                HtmlTextWriter htw = new HtmlTextWriter(sw);
                                grvInvv.RenderControl(htw);
                                Response.Write(sw.ToString());
                                Response.End();
                            }
                            else
                            {
                                Utility.FunShowAlertMsg(this, "No data to export");
                                return;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            //if (rpd != null)
            //{
            //    rpd.Close();
            //    rpd.Dispose();
            //}
        }
    }    

    void LoadMLARelatedDetails()
    {
        try
        {
            FunProClearMainTabControls();
            lblErrorMessage.Text = string.Empty;
            rfvBusineesIRR.Enabled = ImgAccountCreationDate.Visible = CEAccountCreationDate.Enabled =
         btnCalIRR.Visible = false;
            if (ddlMLA.SelectedValue != "0")
            {

                if (Convert.ToString(ViewState["MLAStatus"]) == "0")
                {
                    if (rbtnType.SelectedValue == "1")
                    {
                        FunProPopulateSANum(ddlMLA.SelectedValue);
                    }

                    //Check for Sub account availability    
                    if ((ddlSLA.Items.Count == 1 || ddlSLA.Items.Count == 0) && rbtnType.SelectedValue == "1" && ddlMLA.SelectedValue.ToString() != "0")
                    {
                        Utility.FunShowAlertMsg(this, "No new Sub account is available to activate");
                        ddlMLA.SelectedIndex = 0;
                        ddlMLA.Focus();
                        return;
                    }
                }

                btnXLPorting.Enabled = true;

                if (ddlSLA.Items.Count == 1 || ddlSLA.Items.Count == 0)
                {
                    FunProGetAccCreationDate(ddlMLA.SelectedValue + "DUMMY");
                    FunProGetAmortizationSchedule();
                }
                else
                {
                    FunProGetAccCreationDate(ddlSLA.SelectedValue);
                    if (ddlSLA.SelectedIndex > 0)
                        FunProGetAmortizationSchedule();
                }

                FunProPopulatePANCustomer(ddlMLA.SelectedValue);
                FunProPopulateCheckInterimCustomer(ddlMLA.SelectedValue);
            }
            ddlMLA.Focus();
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rfvBusineesIRR.Enabled = ImgAccountCreationDate.Visible = CEAccountCreationDate.Enabled =
            btnCalIRR.Visible = false;
            btnSave.Enabled = true;
            if (ddlSLA.SelectedValue != "0")
            {
                FunProGetAccCreationDate(ddlSLA.SelectedValue);
                FunProGetAmortizationSchedule();
                FunProPostPreDocument(Convert.ToBoolean(ViewState["Invoice"]));
            }
            ddlSLA.Focus();
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void rbtnType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = -1;
            //ddlBranch.SelectedIndex = -1;
            ddlBranch.Clear();
            ddlMLA.Items.Clear();
            ddlSLA.Items.Clear();
            FunProClearMainTabControls();
            ddlLOB.Focus();

            if (rbtnType.SelectedValue == "1")
            {
                FunProToggleSLA(true);
            }
            else
            {
                FunProToggleSLA(false);
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "te", "fnClearAllTab(false);", true);
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void txtActivationDate_TextChanged(object sender, EventArgs e)
    {
        FunProGetAmortizationSchedule();
        if ((PageMode == PageModes.Create || PageMode == PageModes.WorkFlow))
        {

            if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE")))
              && ViewState["Repayment_Code"].ToString() != "5" && ViewState["Return_Pattern"].ToString() != "6")//Product ??not principal
            {
                txtBusinessIRR.Text = txtCompanyIRR.Text = txtAccIRR.Text = string.Empty;

            }
        }
    }

    #endregion

    #region Router Logic

    protected void custRouterLogic_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        return;
        try
        {
            DateTime ActivationDate = new DateTime();
            DateTime CreationDate = new DateTime();
            DataTable dtAsset = new DataTable();
            DataTable dtDelivery = new DataTable();
            int intGapDays;

            ActivationDate = Utility.StringToDate(txtActivationDate.Text);
            CreationDate = Utility.StringToDate(ViewState["AccountDate"].ToString());

            TimeSpan tmSpan = ActivationDate.Subtract(CreationDate);
            intGapDays = Convert.ToInt32(tmSpan.TotalDays);

            if (Utility.StringToDate(txtActivationDate.Text) < Utility.StringToDate(txtAccountCreationDate.Text))
            {
                args.IsValid = false;
                custRouterLogic.ErrorMessage = "Rental Schedule Activation date should be greater than or equal to Rental Schedule creation date";
                return;
            }

            if (intGapDays <= Convert.ToInt32(ViewState["Days"]))
            {
                //to be add mla applicable test

                DataSet dsParameterSetup = new DataSet();
                DataTable dtApplicatble = new DataTable();
                DataTable dtINVAIE = new DataTable();

                DataView dtView = new DataView();
                DataTable dtCopy = new DataTable();

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Module_ID", "7");
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@ParameterCode", "1,2,5,8,9,10");
                Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
                dsParameterSetup = Utility.GetDataset("S3G_LOANAD_GetGlobalParameters", Procparam);

                dtApplicatble = dsParameterSetup.Tables[0];
                dtINVAIE = dsParameterSetup.Tables[1];

                if ((rbtnType.SelectedIndex == 0 && dtApplicatble.Rows[0]["MLA_Applicable"].ToString() == "0") ||
                    rbtnType.SelectedIndex == 1)
                {
                    if (!FunProLOBBasedValidations(ddlLOB.SelectedItem.Text.ToLower()))
                    {
                        args.IsValid = false;
                        custRouterLogic.ErrorMessage = "Amortization schedule is mandatory for selected Line of Business";
                        lblErrorMessage.Text = string.Empty;
                        return;
                    }

                    dtView = new DataView(dtINVAIE);
                    dtView.RowFilter = "[Parameter_Name] LIKE 'Invoice' ";
                    dtCopy = dtView.ToTable();

                    if (dtCopy.Rows.Count == 2)
                    {
                        dtView.RowFilter = "[Parameter_Name] LIKE 'Invoice' and [Product_ID] Not IN ('0')";
                        dtCopy = dtView.ToTable();

                        ViewState["Invoice"] = dtCopy.Rows[0]["Parameter_Value"];
                    }
                    if (dtCopy.Rows.Count == 1)
                    {
                        ViewState["Invoice"] = dtCopy.Rows[0]["Parameter_Value"];
                    }
                    else if (dtCopy.Rows.Count == 0)
                    {
                        args.IsValid = false;
                        custRouterLogic.ErrorMessage = "Define Applicability of Invoice in Global Parameter for the selected Line of Business and Product";
                        return;
                    }

                    dtView.RowFilter = "[Parameter_Name] LIKE 'AssetEntry' ";
                    dtCopy = dtView.ToTable();

                    if (dtCopy.Rows.Count == 2)
                    {
                        dtView.RowFilter = "[Parameter_Name] LIKE 'AssetEntry' and [Product_ID] Not IN ('0')";
                        dtCopy = dtView.ToTable();

                        ViewState["AssetEntry"] = dtCopy.Rows[0]["Parameter_Value"];
                    }
                    if (dtCopy.Rows.Count == 1)
                    {
                        ViewState["AssetEntry"] = dtCopy.Rows[0]["Parameter_Value"];
                    }
                    else if (dtCopy.Rows.Count == 0)
                    {
                        args.IsValid = false;
                        custRouterLogic.ErrorMessage = "Define Applicability of Asset Identification in Global Parameter for the selected Line of Business and Product";
                        return;
                    }

                    DataSet DsPreReq = new DataSet();
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                    Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
                    if (rbtnType.SelectedIndex == 0)
                    {
                        Procparam.Add("@SANum", ddlMLA.SelectedValue.ToString() + "DUMMY");
                    }
                    else
                    {
                        Procparam.Add("@SANUM", ddlSLA.SelectedItem.Text);
                    }
                    if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OL"))
                    {
                        Procparam.Add("@IS_OL", "1");
                    }
                    else
                    {
                        Procparam.Add("@IS_OL", "0");
                    }

                    DsPreReq = Utility.GetDataset("S3G_LOANAD_CheckForActivationPreReq", Procparam);

                    if (Convert.ToBoolean(ViewState["DeliveryOrder"]) == true)
                    {
                        DataTable dtDO = DsPreReq.Tables[0];

                        if (dtDO.Rows.Count > 0)
                        {
                            args.IsValid = false;
                            string msg = "";

                            if (!ddlLOB.SelectedItem.Text.ToUpper().Contains("OL"))
                            {
                                msg = "Delivery Order for the following asset(s) are not made" + "<br><br>";
                            }
                            else
                            {
                                msg = "Local Purchase Order for the following asset(s) are not made" + "<br><br>";
                            }

                            for (int i = 0; i <= dtDO.Rows.Count - 1; i++)
                            {
                                msg += "&nbsp;&nbsp;&nbsp;&nbsp; " + dtDO.Rows[i]["Asset"].ToString() + " (" + dtDO.Rows[i]["Count"].ToString() + ")" + "<br>";
                            }
                            custRouterLogic.ErrorMessage = msg;
                            return;
                        }

                        //if (Procparam != null)
                        //    Procparam.Clear();
                        //else
                        //    Procparam = new Dictionary<string, string>();
                        //Procparam.Add("@PA_SA_REF_ID", ViewState["PA_SA_REF_ID"].ToString());
                        //dtAsset = Utility.GetDefaultData(SPNames.S3G_LOANAD_AccountAssetCount, Procparam);

                        //if (Procparam != null)
                        //    Procparam.Clear();
                        //else
                        //    Procparam = new Dictionary<string, string>();
                        //Procparam.Add("@SANum", ViewState["SAN"].ToString());
                        //Procparam.Add("@PANum", ddlMLA.SelectedValue);
                        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                        //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                        //Procparam.Add("@Company_ID", intCompanyID.ToString());
                        //dtDelivery = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetIsDeliveryOrder, Procparam);

                        //foreach (DataRow dRow in dtAsset.Rows)
                        //{
                        //    foreach (DataRow dtRow in dtDelivery.Rows)
                        //    {
                        //        if (dtRow["Asset_ID"].ToString() != dRow["Asset_ID"].ToString())
                        //        {
                        //            args.IsValid = false;
                        //            custRouterLogic.ErrorMessage = "Delivery Instruction for all Assets has not been defined";
                        //            return;
                        //        }
                        //        else
                        //        {
                        //            if (Convert.ToDecimal(dRow["Payment_Percentage"].ToString()) == 100)
                        //            {
                        //                args.IsValid = true;
                        //            }
                        //            else
                        //            {
                        //                args.IsValid = false;
                        //                custRouterLogic.ErrorMessage = "The Payment percentage is not 100%";
                        //                return;
                        //            }
                        //        }
                        //    }
                        //}
                    }

                    //else
                    //{
                    //    dtAsset = new DataTable();
                    //    if (Procparam != null)
                    //        Procparam.Clear();
                    //    else
                    //        Procparam = new Dictionary<string, string>();
                    //    Procparam.Add("@PA_SA_REF_ID", ViewState["PA_SA_REF_ID"].ToString());
                    //    dtAsset = Utility.GetDefaultData(SPNames.S3G_LOANAD_AccountAssetCount, Procparam);
                    //    foreach (DataRow dRow in dtAsset.Rows)
                    //    {
                    //        if (Convert.ToInt32(dRow["Payment_Percentage"]) == 100)
                    //        {
                    //            args.IsValid = true;
                    //        }
                    //        else
                    //        {
                    //            args.IsValid = false;
                    //            custRouterLogic.ErrorMessage = "The Payment percentage is not 100%";
                    //            return;
                    //        }
                    //    }

                    //}


                    if (Convert.ToBoolean(ViewState["Invoice"]) == true)
                    {
                        DataTable dtInv = DsPreReq.Tables[1];
                        DataTable dtInvAssets = DsPreReq.Tables[2];

                        if ((grvFile.Rows.Count <= 0 && grvInvoice.Rows.Count <= 0 && dtInv.Rows.Count > 0) || dtInvAssets.Rows.Count > 0)
                        {
                            args.IsValid = false;
                            string msg = "The Invoice for the following asset(s) are not completed" + "<br><br>";

                            for (int i = 0; i <= dtInvAssets.Rows.Count - 1; i++)
                            {
                                msg += "&nbsp;&nbsp;&nbsp;&nbsp; " + dtInvAssets.Rows[i]["Asset"].ToString() + " (" + dtInvAssets.Rows[i]["Count"].ToString() + ")" + "<br>";
                            }

                            custRouterLogic.ErrorMessage = msg;
                            return;
                        }
                    }

                    if (Convert.ToBoolean(ViewState["AssetEntry"]) == true)
                    {
                        DataTable dtAIE = DsPreReq.Tables[3];

                        if (dtAIE.Rows.Count > 0)
                        {
                            args.IsValid = false;
                            string msg = "The identification for the following asset(s) are not completed" + "<br><br>";

                            for (int i = 0; i <= dtAIE.Rows.Count - 1; i++)
                            {
                                msg += "&nbsp;&nbsp;&nbsp;&nbsp; " + dtAIE.Rows[i]["Asset"].ToString() + " (" + dtAIE.Rows[i]["Count"].ToString() + ")" + "<br>";
                            }
                            custRouterLogic.ErrorMessage = msg;
                            return;
                        }
                    }

                    if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OL"))
                    {
                        DataTable dtDisposed = DsPreReq.Tables[4];

                        if (dtDisposed.Rows.Count > 0)
                        {
                            args.IsValid = false;
                            string msg = "The following asset(s) are Disposed" + "<br><br>";

                            for (int i = 0; i <= dtDisposed.Rows.Count - 1; i++)
                            {
                                msg += "&nbsp;&nbsp;&nbsp;&nbsp; " + dtDisposed.Rows[i]["Asset"].ToString() + "<br>";
                            }
                            custRouterLogic.ErrorMessage = msg;
                            return;
                        }
                    }
                }
            }
            else
            {
                args.IsValid = false;
                custRouterLogic.ErrorMessage = "The difference between Rental Schedule creation and Activation needs to be with in " + ViewState["Days"].ToString() + " day(s)";
                return;
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    #endregion

    #endregion

    #region User Defined Functions

    protected void FunProLoadTaxTypes()
    {
        try
        {
            lblTaxType.Visible = true;
            ddlTaxType.Visible = true;
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            ddlTaxType.BindDataTable("S3G_ORG_GETTAXTYPES", Procparam, false, new string[] { "ID", "Name" });
            ddlTaxType.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to load Tax Types");
        }
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

                    btnXLPorting.Enabled = 
                    btnExportJV.Enabled =
                    tcAccountActivation.Tabs[1].Enabled =
                    tcAccountActivation.Tabs[2].Enabled =
                    tcAccountActivation.Tabs[3].Enabled =
                    tcAccountActivation.Tabs[4].Enabled = false;                    
                    txtActivationDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    btnDebitNotes.Visible = false;
                    btnCashflow.Visible = false;
                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (intUserID == 67)
                        btnRevoke.Visible = true;
                    if (!bModify)
                    {
                        btnSave.Visible = false;
                        btnRevoke.Enabled = false;
                    }
                    ddlSLA.Enabled = ddlMLA.Enabled = ddlLOB.Enabled = ddlBranch.Enabled = btnClear.Enabled = false;
                    btnSave.Text = "Save";
                    btnSave.ToolTip = "Save";
                    rbtnType.Enabled = false;
                    btnDebitNotes.Visible = false;
                    break;

                case -1:// Query Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = btnClear.Enabled = false;                    
                    rbtnType.Enabled = false;
                    ddlBranch.Enabled = false;
                    btnDebitNotes.Visible = true;
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }

                    if (bClearList)
                    {
                        //ddlBranch.ClearDropDownList();
                        //ddlLOB.ClearDropDownList();
                        ddlMLA.ClearDropDownList();
                        ddlSLA.ClearDropDownList();
                    }
                    imgDateofActivation.Visible = false;
                    CalendarExtender2.Enabled = false;
                    custRouterLogic.Enabled = false;

                    string strParam = Convert.ToString(ViewState["EnableTab1"]) + ",";
                    strParam += Convert.ToString(ViewState["EnableTab2"]) + ",";
                    strParam += Convert.ToString(ViewState["EnableTab3"]) + ",";
                    strParam += Convert.ToString(ViewState["EnableTab4"]);

                    ScriptManager.RegisterStartupScript(this, GetType(), "te", "finDisableTab(" + strParam + ");", true);
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to set the controls");
        }
    }

    protected void FunProGetGloblaMLASLA()
    {
        DataTable dtTable = new DataTable();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            dtTable = Utility.GetDefaultData(SPNames.S3G_SYSAD_GetGlobalMLASLA, Procparam);
            string strMLAStatus = string.Empty;

            if (dtTable.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dtTable.Rows[0]["OnlyMLA"]) == false && Convert.ToBoolean(dtTable.Rows[0]["MLAandSLA"]) == true)
                {
                    strMLAStatus = "0";
                    rbtnType.Items[1].Enabled = true;
                    //FunProToggleSLA(true);
                }
                else if (Convert.ToBoolean(dtTable.Rows[0]["OnlyMLA"]) == true && Convert.ToBoolean(dtTable.Rows[0]["MLAandSLA"]) == false)
                {
                    strMLAStatus = "1";
                    if (rbtnType.SelectedIndex == 1 && PageMode == PageModes.Create)
                    {
                        rbtnType.SelectedIndex = 0;
                        //rbtnType.Items[1].Enabled = false;
                        ddlSLA.Enabled = false;
                        lblSLA.CssClass = "";
                        FunProToggleSLA(false);

                        Utility.FunShowAlertMsg(this, "Sub Account is not applicable for the selected Line of Business");
                    }
                }
                ViewState["MLAStatus"] = strMLAStatus;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Golbal details of PANum and SANum");
        }
    }
    //Performance Issue Removed By Shibu  
    protected void FunProPopulateBranchList()
    {
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //if (PageMode == PageModes.Create)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            //Procparam.Add("@User_ID", intUserID.ToString());
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //Procparam.Add("@Program_ID", "75");
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Location list");
        }
    }


    protected void FunProPopulateinterimList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@type", "IR");
            ddlInterim.BindDataTable("S3G_ORG_GETMRA_LOOKUP", Procparam, false, new string[] { "ID", "Name" });
            ddlInterim.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Line of Business list");
        }
    }
    protected void FunProPopulateLOBList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Program_ID", "75");
            if (PageMode == PageModes.Create)
            {
                Procparam.Add("@Is_Active", "1");
            }
            //ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, false, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            FunLOBRelatedDetails();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Line of Business list");
        }
    }

    protected void FunProPopulatePANum(string strMLAStatus)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Type", "Type5");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Param1", "1");

            if (rbtnType.SelectedValue == "0")
                Procparam.Add("@Param2", "1");
            else
                Procparam.Add("@Param2", "2");
            Procparam.Add("@Param3", strMLAStatus);
            Procparam.Add("@UserID", intUserID.ToString());
            ddlMLA.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_List, Procparam, new string[] { "PANum", "PANum", "Customer_name" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Rental Schedule Numbers");
        }
    }

    protected void FunProPopulateActivatedPANum(string strClosureNo)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Account_Activation_Number", strClosureNo);
            ddlMLA.BindDataTable(SPNames.S3G_LOANAD_ActivatedPANum, Procparam, new string[] { "PANum", "PANum", });

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

    protected void FunProPopulateActivatedSANum(string strClosureNo)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Account_Activation_Number", strClosureNo);
            ddlSLA.BindDataTable(SPNames.S3G_LOANAD_ActivatedPANum, Procparam, new string[] { "SANum", "SANum" });

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

    protected void FunProGetGlobalParameterDetails()
    {
        try
        {
            DataTable dtTable = new DataTable();
            DataView dtView = new DataView();
            DataTable dtCopy = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Module_ID", "7");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            //Changed By Thangam M on 09/Feb/2012 to get Product Method
            Procparam.Add("@ParameterCode", "1,2,5,8,9,10,11");

            dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetGlobalParameters, Procparam);

            if (dtTable.Rows.Count > 0)
            {
                dtView = new DataView(dtTable);
                dtView.RowFilter = "[Parameter_Name] LIKE 'Days' ";
                dtCopy = dtView.ToTable();
                if (dtCopy.Rows.Count > 0)
                {
                    if (dtCopy.Rows[0]["Parameter_Value"].ToString() != "")
                        ViewState["Days"] = dtCopy.Rows[0]["Parameter_Value"];
                    else
                        ViewState["Days"] = 0;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Define Activation Gap days in Global Parameter for the selected Line of Business');", true);
                    lblErrorMessage.Text = string.Empty;
                    //ddlLOB.SelectedIndex = 0;
                    return;
                }

                dtView.RowFilter = "[Parameter_Name] LIKE 'Invoice' ";
                dtCopy = dtView.ToTable();
                if (dtCopy.Rows.Count > 0)
                {
                    ViewState["Invoice"] = dtCopy.Rows[0]["Parameter_Value"];
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Define Applicability of Invoice in Global Parameter for the selected Line of Business');", true);
                    lblErrorMessage.Text = string.Empty;
                    //ddlLOB.SelectedIndex = 0;
                    return;
                }

                dtView.RowFilter = "[Parameter_Name] LIKE 'AssetEntry' ";
                dtCopy = dtView.ToTable();
                if (dtCopy.Rows.Count > 0)
                {
                    ViewState["AssetEntry"] = dtCopy.Rows[0]["Parameter_Value"];
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Define Applicability of Asset Identification Entry in Global Parameter for the selected Line of Business');", true);
                    lblErrorMessage.Text = string.Empty;
                    //ddlLOB.SelectedIndex = 0;
                    return;
                }

                if (ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim() != "wc" || ddlLOB.SelectedItem.Text.ToLower().Split('-')[0].Trim() != "ft")
                {
                    dtView.RowFilter = "[Parameter_Name] LIKE 'ESM' ";
                    dtCopy = dtView.ToTable();
                    if (dtCopy.Rows.Count > 0)
                    {
                        ViewState["ESM"] = dtCopy.Rows[0]["Parameter_Value"];
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Define an income recognition method for the selected Line of Business');", true);
                        lblErrorMessage.Text = string.Empty;
                        //ddlLOB.SelectedIndex = 0;
                        return;
                    }

                    //Changed By Thangam M on 09/Feb/2012 to get Product Method
                    dtView.RowFilter = "[Parameter_Name] LIKE 'Product' ";
                    dtCopy = dtView.ToTable();
                    if (dtCopy.Rows.Count > 0)
                    {
                        ViewState["Product"] = dtCopy.Rows[0]["Parameter_Value"];
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Define an income recognition method for the selected Line of Business');", true);
                        lblErrorMessage.Text = string.Empty;
                        //ddlLOB.SelectedIndex = 0;
                        return;
                    }

                    dtView.RowFilter = "[Parameter_Name] LIKE 'IRR' ";
                    dtCopy = dtView.ToTable();
                    if (dtCopy.Rows.Count > 0)
                    {
                        ViewState["IRR"] = dtCopy.Rows[0]["Parameter_Value"];
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Define an income recognition method for the selected Line of Business');", true);
                        lblErrorMessage.Text = string.Empty;
                        //ddlLOB.SelectedIndex = 0;
                        return;
                    }

                    dtView.RowFilter = "[Parameter_Name] LIKE 'SOD' ";
                    dtCopy = dtView.ToTable();
                    if (dtCopy.Rows.Count > 0)
                    {
                        ViewState["SOD"] = dtCopy.Rows[0]["Parameter_Value"];
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Define an income recognition method for the selected Line of Business');", true);
                        lblErrorMessage.Text = string.Empty;
                        //ddlLOB.SelectedIndex = 0;
                        return;
                    }

                    if (ViewState["ESM"].ToString() == "False" && ViewState["IRR"].ToString() == "False" && ViewState["SOD"].ToString() == "False" && ViewState["Product"].ToString() == "False"
                        && !ddlLOB.SelectedItem.ToString().ToUpper().Contains("OPER") && !ddlLOB.SelectedItem.ToString().ToUpper().Contains("WORK") && !ddlLOB.SelectedItem.ToString().ToUpper().Contains("FACT"))
                    {
                        if (PageMode == PageModes.Create)
                        {
                            Utility.FunShowAlertMsg(this, "Global Parameter Setup is not defined for Amortization method.");
                            ddlLOB.SelectedIndex = -1;
                            //ddlBranch.SelectedIndex = -1;
                            ddlMLA.Items.Clear();
                            ddlSLA.Items.Clear();
                            FunProClearMainTabControls();
                            btnSave.Enabled = false;
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Global Parameter Setup is not defined for Amortization method.');" + strRedirectPageView, true);
                        }
                    }
                }
                else
                {
                    ViewState["ESM"] = false;
                    ViewState["IRR"] = false;
                    ViewState["SOD"] = false;
                    ViewState["Product"] = false;
                }
                dtView.Dispose();

                if (bCreate && PageMode != PageModes.Query)
                {
                    btnSave.Enabled = true;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Global Parmeter not defined for selected Line of Business");
                //ddlLOB.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Global parameter details");
        }
    }

    protected void FunProGetAccCreationDate(string strSLAValue)
    {
        try
        {
            rfvBusineesIRR.Enabled = ImgAccountCreationDate.Visible = CEAccountCreationDate.Enabled =
            btnCalIRR.Visible = false;
            DataSet dtSet = new DataSet();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyID.ToString());
            Procparam.Add("@SANum", strSLAValue);
            Procparam.Add("@PANum", ddlMLA.SelectedValue);

            dtSet = Utility.GetTableValues(SPNames.S3G_LOANAD_GetPASARefID, Procparam);
            if (dtSet.Tables[0].Rows.Count > 0)
            {
                ViewState["AccountDate"] = dtSet.Tables[0].Rows[0]["Creation_Date"].ToString();//Repayment_Code
                txtAccountCreationDate.Text = Convert.ToDateTime(dtSet.Tables[0].Rows[0]["Creation_Date"].ToString()).ToString(strDateFormat);

                txtBusinessIRR.Text = dtSet.Tables[0].Rows[0]["Business_IRR"].ToString();
                txtCompanyIRR.Text = dtSet.Tables[0].Rows[0]["Company_IRR"].ToString();
                txtAccIRR.Text = dtSet.Tables[0].Rows[0]["Accounting_IRR"].ToString();
                ViewState["Repayment_Code"] = dtSet.Tables[0].Rows[0]["Repayment_Code"].ToString();
                ViewState["Return_Pattern"] = dtSet.Tables[0].Rows[0]["Return_Pattern"].ToString();
                txtFinanceAmount.Text = dtSet.Tables[0].Rows[0]["Finance_Amount"].ToString();
                txtStatus.Text = dtSet.Tables[0].Rows[0]["Status"].ToString();
                ViewState["DeliveryOrder"] = dtSet.Tables[0].Rows[0]["Is_Delivery_Order_Require"].ToString();
                ViewState["PA_SA_REF_ID"] = dtSet.Tables[0].Rows[0]["PA_SA_REF_ID"].ToString();
                ViewState["Created_By"] = dtSet.Tables[0].Rows[0]["Created_By"].ToString();
                ViewState["UserLevel"] = dtSet.Tables[0].Rows[0]["User_Level_ID"].ToString();
                ViewState["SAN"] = strSLAValue;
                lblLeaseType1.Text = dtSet.Tables[0].Rows[0]["Lease_Type"].ToString(); 

                //Added by Thangam M on 20/Feb/2012 to check payment for Normal Payment
                if ((PageMode == PageModes.Create || PageMode == PageModes.WorkFlow) && dtSet.Tables[0].Rows[0]["Payment"].ToString() != "1")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('100% payment has to be made to activate this Rental Schedule.');", true);
                    btnSave.Enabled = false;
                    //ddlMLA.SelectedIndex = 0;
                    //FunProClearMainTabControls();
                    //ddlSLA.Items.Clear();
                    //ddlMLA.Focus();
                    //return;
                }
                else
                {
                    if ((PageMode == PageModes.Create || PageMode == PageModes.WorkFlow))
                    {

                        if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE")))
                          && ViewState["Repayment_Code"].ToString() != "5" && ViewState["Return_Pattern"].ToString() != "6")//Product ??not principal
                        {
                            txtBusinessIRR.Text = txtCompanyIRR.Text = txtAccIRR.Text = string.Empty;
                            rfvBusineesIRR.Enabled =
                            btnCalIRR.Visible =
                                //CEAccountCreationDate.Enabled =
                                //ImgAccountCreationDate.Visible = 
                            true;
                            txtAccountCreationDate.Text = Convert.ToDateTime(dtSet.Tables[0].Rows[0]["Payment_Date"].ToString()).ToString(strDateFormat);

                        }
                    }
                }


            }
            if (dtSet.Tables[1].Rows.Count > 0)
            {
                ViewState["AccountType"] = dtSet.Tables[1].Rows[0]["accounttype"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Rental Schedule creation details");
        }
    }

    protected void FunProPopulateSANum(string strPAN)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Type", "Type5");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Param1", "2");
            Procparam.Add("@Param2", strPAN);
            Procparam.Add("@UserID", intUserID.ToString());
            ddlSLA.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_List, Procparam, new string[] { "SANum", "SANum" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Sub Account Numbers");
        }
    }

    protected void FunProPopulatePANCustomer(string strPAN)
    {
        try
        {
            if (ddlMLA.SelectedValue != "0")
            {
                DataTable dtTable = new DataTable();
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                Procparam.Add("@PANum", strPAN);
                dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetPANumCustomer, Procparam);

                S3GCustomerAddress1.SetCustomerDetails(dtTable.Rows[0], true);

                if (ddlSLA.Items.Count <= 1)
                {
                    FunProPostPreDocument(Convert.ToBoolean(ViewState["Invoice"]));
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Customer Details");
        }
    }
    protected void FunProPopulateCheckInterimCustomer(string strPAN)
    {
        try
        {
            if (ddlMLA.SelectedValue != "0")
            {
                DataTable dtTable = new DataTable();
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                Procparam.Add("@PANum", strPAN);
                dtTable = Utility.GetDefaultData("S3G_LOANAD_GetMRARent_Cust", Procparam);
                if (dtTable.Rows.Count > 0)
                {
                    if (dtTable.Rows[0]["appl"].ToString() == "1")
                    {
                        RfvInterim.Enabled = true;
                        RfvInterim.Visible = true;
                    }
                    else
                    {
                        RfvInterim.Enabled = false;
                        RfvInterim.Visible = false;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "MRA not approved for Selected customer");
                    btnSave.Enabled = false;
                    return;
                }
                btnSave.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Customer Details");
        }
    }


    protected void FunProPostPreDocument(bool boolInvoice)
    {
        try
        {
            DataSet dsDocuments = new DataSet();
            DataTable dtProforma = new DataTable();
            DataTable dtInvoice = new DataTable();
            DataTable dtPreDisb = new DataTable();
            DataTable dtPostDisb = new DataTable();

            string strPath = string.Empty;

            //if (boolInvoice == true)
            //{

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@PANUM", ddlMLA.SelectedValue.ToString());
            if (ddlSLA.Items.Count > 0 && ddlSLA.SelectedValue != "0")
            {
                Procparam.Add("@SANUM", ddlSLA.SelectedValue.ToString());
            }
            //Procparam.Add("@Customer_ID", txtCustCode.Attributes["Cust_ID"]);

            dsDocuments = Utility.GetDataset("S3G_LOANAD_GetPrePostDisbursementDocs", Procparam);
            dtProforma = dsDocuments.Tables[0];
            dtPreDisb = dsDocuments.Tables[1];
            dtInvoice = dsDocuments.Tables[2];
            dtPostDisb = dsDocuments.Tables[3];

            //dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetProformaImage, Procparam);

            if (dtProforma.Rows.Count > 0 || dtPreDisb.Rows.Count > 0)
            {
                if (dtProforma.Rows.Count == 0)
                {
                    pnlProforma.Visible = false;
                }
                else
                {
                    pnlProforma.Visible = true;
                }

                if (dtPreDisb.Rows.Count == 0)
                {
                    pnlPreDisb.Visible = false;
                }
                else
                {
                    pnlPreDisb.Visible = true;
                }

                grvFile.DataSource = dtProforma;
                grvFile.DataBind();

                gvPRDDT.DataSource = dtPreDisb;
                gvPRDDT.DataBind();

                trPreMessage.Visible = false;
                ViewState["EnableTab3"] = "true";
            }
            else
            {
                ViewState["EnableTab3"] = "false";
                trPreMessage.Visible = true;
                if (PageMode != PageModes.Create)
                {
                    tcAccountActivation.Tabs[3].Enabled = false;
                }
            }

            if (dtInvoice.Rows.Count > 0 || dtPostDisb.Rows.Count > 0)
            {
                if (dtInvoice.Rows.Count == 0)
                {
                    pnlInvoice.Visible = false;
                }
                else
                {
                    pnlInvoice.Visible = true;
                }

                if (dtPostDisb.Rows.Count == 0)
                {
                    pnlPostDisb.Visible = false;
                }
                else
                {
                    pnlPostDisb.Visible = true;
                }

                grvInvoice.DataSource = dtInvoice;
                grvInvoice.DataBind();

                gvPDDT.DataSource = dtPostDisb;
                gvPDDT.DataBind();

                trPostMessage.Visible = false;
                ViewState["EnableTab4"] = "true";
            }
                
            else
            {
                ViewState["EnableTab4"] = "false";
                trPostMessage.Visible = true;
                if (PageMode != PageModes.Create)
                {
                    tcAccountActivation.Tabs[4].Enabled = false;
                }
            }

            for (int i = 1; i <= 4; i++)
            {
                if (ViewState["EnableTab" + i.ToString()] == null || string.IsNullOrEmpty(ViewState["EnableTab" + i.ToString()].ToString()))
                {
                    ViewState["EnableTab" + i.ToString()] = "false";
                }
            }

            string strParam = Convert.ToString(ViewState["EnableTab1"]) + ",";
            strParam += Convert.ToString(ViewState["EnableTab2"]) + ",";
            strParam += Convert.ToString(ViewState["EnableTab3"]) + ",";
            strParam += Convert.ToString(ViewState["EnableTab4"]);

            ScriptManager.RegisterStartupScript(this, GetType(), "te", "finDisableTab(" + strParam + ");", true);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to load Pre disbursement and Post disbursement documents");
        }
    }

    protected void FunProActiavtedAccountForModification(string strAccountNo)
    {
        try
        {
            DataTable dtTable = new DataTable();

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@ActivationNumber", strAccountNo.ToString());
            DataSet Dst = Utility.GetDataset("S3G_LOANAD_GetAccountActivationforModify", Procparam);
            FunProPopulateinterimList();
            //objAccActivation_Client = new ContractMgtServicesReference.ContractMgtServicesClient();

            //byte[] bytesAsserDetails = objAccActivation_Client.FunProActiavtedAccountForModification(strAccountNo);

            //dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesAsserDetails, ObjSerMode, typeof(DataTable));

            dtTable = Dst.Tables[0];

            ddlInterim.SelectedValue = dtTable.Rows[0]["interim_type"].ToString();
            ddlInterim.ClearDropDownList();
            txtinterimrate.Text = dtTable.Rows[0]["interim_rate"].ToString();
            txtInterimamount.Text = dtTable.Rows[0]["interim_rent"].ToString();
            if (txtInterimamount.Text == "0")
                btnprint.Enabled = false;
            txtActivationDate.Text = DateTime.Parse(dtTable.Rows[0]["Account_Activated_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            ddlLOB.Items.Add(new ListItem(dtTable.Rows[0]["LOB_Name"].ToString(), dtTable.Rows[0]["LOB_ID"].ToString()));
            ddlLOB.ToolTip = dtTable.Rows[0]["LOB_Name"].ToString();

            //ddlLOB.ClearDropDownList();
            ddlBranch.SelectedValue = dtTable.Rows[0]["Location_ID"].ToString();
            ddlBranch.SelectedText = dtTable.Rows[0]["Location_Name"].ToString();
            ddlBranch.ToolTip = dtTable.Rows[0]["Location_Name"].ToString();
            //ddlBranch.ClearDropDownList();
            ddlMLA.Items.Add(new ListItem(dtTable.Rows[0]["PANum"].ToString(), dtTable.Rows[0]["PANum"].ToString()));
            ddlMLA.SelectedValue = dtTable.Rows[0]["PANum"].ToString();

            if (!dtTable.Rows[0]["SANum"].ToString().Contains("DUMMY"))
            {
                ddlSLA.Items.Add(new ListItem(dtTable.Rows[0]["SANum"].ToString(), dtTable.Rows[0]["SANum"].ToString()));
                ddlSLA.SelectedValue = dtTable.Rows[0]["SANum"].ToString();
                rbtnType.SelectedValue = "1";
            }
            else
            {
                ddlSLA.Items.Add(new ListItem("--Select--", "0"));
                ddlSLA.SelectedValue = "0";
                rbtnType.SelectedValue = "0";
            }

            //FunProPopulateActivatedPANum(strAccountNo);
            //FunProPopulateActivatedSANum(strAccountNo);

            //ddlMLA.SelectedValue = dtTable.Rows[0]["PANum"].ToString();
            //ddlMLA.ClearDropDownList();
            //FunProPopulateSANum(ddlMLA.SelectedValue);
            //ddlSLA.SelectedValue = dtTable.Rows[0]["SANum"].ToString();
            //ddlSLA.ClearDropDownList();

            if (ddlSLA.SelectedValue == "0")
                FunProGetAccCreationDate(ddlMLA.SelectedValue + "Dummy");
            else
                FunProGetAccCreationDate(ddlSLA.SelectedValue);
            //FunProGetGlobalParameterDetails();
            FunProGetGloblaMLASLA();
            FunProGetAmortizationSchedule();
            S3GCustomerAddress1.SetCustomerDetails(dtTable.Rows[0], true);
            ViewState["Activated_By"] = dtTable.Rows[0]["Created_By"].ToString();

            if (Dst.Tables[1].Rows.Count > 0)
            {
                ViewState["EnableTab2"] = "true";
                gvSystemJournal.DataSource = Dst.Tables[1];
                gvSystemJournal.DataBind();
            }
            else
            {
                ViewState["EnableTab2"] = "false";
                if (PageMode != PageModes.Create)
                {
                    tcAccountActivation.Tabs[2].Enabled = false;
                }
            }

            FunProPostPreDocument(Convert.ToBoolean(ViewState["Invoice"]));

            if (Dst.Tables[0].Rows.Count == 0 || Dst.Tables[2].Rows[0][0].ToString() == "0")
            {
                btnRevoke.Enabled = false;
            }

            if (Dst.Tables[3].Rows[0][0].ToString() != "0")
            {
                blCanModify = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to load Activation details");
        }
    }

    protected void FunProGenerateXMLDocs()
    {
        try
        {
            strImage.Append("<Root>");

            foreach (GridViewRow grvFRow in grvFile.Rows)
            {
                Label lblID = (Label)grvFRow.FindControl("lblID");
                strImage.Append("<Details Invoice_ID='" + lblID.Text + "' /> ");
            }

            strImage.Append("</Root>");

            strProforma.Append("<Root>");

            foreach (GridViewRow grvFRow in grvFile.Rows)
            {
                Label lblPID = (Label)grvFRow.FindControl("lblID");
                strImage.Append("<Details Proforma_ID='" + lblPID.Text + "' /> ");
            }
            strProforma.Append("</Root>");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to generate XML documents");
        }
    }

    protected bool FunProLOBBasedValidations(string strType)
    {
        try
        {
            bool blnIsAmortization;
            strType = strType.Split('-')[0].Trim();
            switch (strType.ToLower())
            {

                case "wc": //hire purchase
                case "ft": //finamce lease
                case "ol":
                    blnIsAmortization = false;
                    break;
                default:
                    blnIsAmortization = true;
                    break;
            }
            //Changed By Thangam M on 09/Feb/2012 to get Product Method
            if (Convert.ToString(ViewState["Product"]) == "True")
            {
                blnIsAmortization = false;
            }

            if (blnIsAmortization)
            {
                if (grvAmortization.Rows.Count <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to check LOB based conditions");
        }
    }

    #region "Amortization Schedule"

    protected void FunProGetAmortizationSchedule()
    {
        try
        {
            if (strMode != "C")
            {
                if (!ddlLOB.SelectedItem.ToString().ToUpper().Contains("WORK") && !ddlLOB.SelectedItem.ToString().ToUpper().Contains("FACT"))
                {
                    DataSet dtSet = new DataSet();
                    DataTable dtAmortization = new DataTable();

                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();
                    Procparam.Add("@PANum", ddlMLA.SelectedValue);
                    if (ddlSLA.Items.Count == 0 || ddlSLA.SelectedValue == "0")
                    {
                        Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
                    }
                    else
                    {
                        Procparam.Add("@SANum", ddlSLA.SelectedValue);
                    }
                    if (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow)
                    {
                        Procparam.Add("@Page_Mode", "0");
                    }
                    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID.ToString()));
                    Procparam.Add("@User_ID", Convert.ToString(intUserID.ToString())); //Added by Tamilselvan.s on 22/10/2011
                    Procparam.Add("@Activation_Date", Convert.ToString(Utility.StringToDate(txtActivationDate.Text)));
                    dtSet = Utility.GetTableValues(SPNames.S3G_LOANAD_GetAccountDetailsForCalculation, Procparam);

                    int intTenure = 0;
                    int intTenureCode = 0;
                    int intTimeValue = 0;
                    int intNoOfInst = 0;
                    int intFrequency = 0;
                    decimal intAccIRR = 0;
                    decimal intFinance = 0;
                    decimal decRate = 0;
                    decimal decFinCharge = 0;
                    string strTenureDesc = "";
                    DateTime dtTime = new DateTime();

                    //Changed By Thangam M on 09/Feb/2012 to get Product Method
                    ViewState["Product"] = dtSet.Tables[0].Rows[0]["IS_Product"];

                    if (Convert.ToString(ViewState["Product"]) != "True" && dtSet.Tables[0].Rows.Count > 0 && dtSet.Tables[0].Rows.Count > 0)
                    {
                        dtTime = Utility.StringToDate(dtSet.Tables[0].Rows[0]["Creation_Date"].ToString());
                        intFinance = Convert.ToDecimal(dtSet.Tables[0].Rows[0]["Finance_Amount"].ToString());
                        intAccIRR = Convert.ToDecimal(dtSet.Tables[0].Rows[0]["Accounting_IRR"].ToString());

                        intTenure = Convert.ToInt32(dtSet.Tables[0].Rows[0]["Tenure"].ToString());
                        strTenureDesc = dtSet.Tables[0].Rows[0]["Tenure_Desc"].ToString();
                        if (dtSet.Tables[1].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dtSet.Tables[1].Rows[0]["Rate"].ToString()))
                                decRate = Convert.ToDecimal(dtSet.Tables[1].Rows[0]["Rate"].ToString());
                            if (!string.IsNullOrEmpty(dtSet.Tables[1].Rows[0]["Time_Value"].ToString()))
                                intTimeValue = Convert.ToInt32(dtSet.Tables[1].Rows[0]["Time_Value"].ToString());
                            if (!string.IsNullOrEmpty(dtSet.Tables[1].Rows[0]["NoOfInst"].ToString()))
                                intNoOfInst = Convert.ToInt32(dtSet.Tables[1].Rows[0]["NoOfInst"].ToString());
                            if (!string.IsNullOrEmpty(dtSet.Tables[1].Rows[0]["FinCharge"].ToString()))
                                decFinCharge = Convert.ToDecimal(dtSet.Tables[1].Rows[0]["FinCharge"].ToString());
                            if (!string.IsNullOrEmpty(dtSet.Tables[1].Rows[0]["Frequency"].ToString()))
                                intFrequency = Convert.ToInt32(dtSet.Tables[1].Rows[0]["Frequency"].ToString());
                        }
                        trAmortizationMessage.Visible = true;
                        /*Start the below changes done by Tamilselvan.S on 22/10/2011 for Amortization*/
                        //if (intNoOfInst != 0 && dtSet.Tables[2].Rows.Count > 0 && dtSet.Tables[2].Columns.Count > 1)
                        if (dtSet.Tables[2].Rows.Count > 0 && dtSet.Tables[2].Columns.Count > 1)
                        {
                            grvAmortization.DataSource = dtSet.Tables[2];
                            grvAmortization.DataBind();
                            trAmortizationMessage.Visible = false;
                            string strMethod = "";
                            //Changed By Thanagm M on 30/Dec/2011 to Check the calc mothod for modify mode
                            //Changed By Narasimha Rao on 11/Jan/2012 to Check the condition for Create mode and WorkFlow Mode also.

                            if (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow)
                            {
                                strMethod = Convert.ToBoolean(ViewState["SOD"]) == true ? "SOD" : Convert.ToBoolean(ViewState["ESM"]) == true ? "ESM" : Convert.ToBoolean(ViewState["IRR"]) == true ? "IRR" : "";
                            }
                            else
                            {
                                strMethod = dtSet.Tables[2].Rows[0]["Method"].ToString();
                            }
                            grvAmortization.Columns[1].Visible = strMethod == "IRR" ? false : true;
                            grvAmortization.Columns[3].Visible = strMethod == "IRR" ? false : true;
                        }
                        else if (dtSet.Tables[2].Rows.Count == 0 && (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow) && ViewState["MLAStatus"].ToString() == "1")
                        {
                            throw new ApplicationException("Global Parameter Setup is not defined for Amortization method.");
                        }
                        //DataTable dtAmortIRR = dtSet.Tables[2];

                        //if (intNoOfInst != 0)
                        //{
                        //    if (Convert.ToBoolean(ViewState["SOD"]) == true)
                        //    {
                        //        dtAmortIRR.Columns.Remove("Principal Portion");
                        //        dtAmortIRR.Columns.Remove("Interest Portion");

                        //        dtAmortization = CommonS3GBusLogic.FunPubSODCalculation(dtAmortIRR, dtTime, intFinance, decRate, intTenure, strTenureDesc, decFinCharge);
                        //        trAmortizationMessage.Visible = false;
                        //        dtAmortization = FunProHideColumns(true, dtAmortization);
                        //    }
                        //    else if (Convert.ToBoolean(ViewState["ESM"]) == true)
                        //    {
                        //        dtAmortIRR.Columns.Remove("Principal Portion");
                        //        dtAmortIRR.Columns.Remove("Interest Portion");

                        //        dtAmortization = CommonS3GBusLogic.FunPubESMCalculation(dtAmortIRR, dtTime, intFinance, decRate, intTenure, strTenureDesc, decFinCharge);
                        //        trAmortizationMessage.Visible = false;
                        //        dtAmortization = FunProHideColumns(true, dtAmortization);
                        //    }
                        //    else if (Convert.ToBoolean(ViewState["IRR"]) == true)
                        //    {
                        //        dtAmortization = dtAmortIRR;
                        //        trAmortizationMessage.Visible = false;
                        //        dtAmortization = FunProHideColumns(false, dtAmortization);
                        //        grvAmortization.Width = Unit.Percentage(60);
                        //    }
                        //    else
                        //    {

                        //    }

                        //    grvAmortization.DataSource = dtAmortization;
                        //    grvAmortization.DataBind();
                        //}
                        /*End the above changes done by Tamilselvan.S on 22/10/2011 for Amortization*/
                    }
                }
                if (grvAmortization != null)
                {
                    if (grvAmortization.Rows.Count == 0)
                    {
                        trAmortizationMessage.Visible = true;
                        ViewState["EnableTab1"] = "false";

                        if (PageMode != PageModes.Create)
                        {
                            tcAccountActivation.Tabs[1].Enabled = false;
                        }
                    }
                    else
                    {
                        ViewState["EnableTab1"] = "true";
                        trAmortizationMessage.Visible = false;
                        btnXLPorting.Attributes.Remove("disabled");
                    }
                }
                else
                {
                    ViewState["EnableTab1"] = "false";
                    trAmortizationMessage.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to load Amortization Schedule");
        }
    }

    protected DataTable FunProHideColumns(bool CanShow, DataTable dtAmortization)
    {
        try
        {
            if (!dtAmortization.Columns.Contains("Insurance"))
            {
                dtAmortization.Columns.Add("Insurance");
            }
            if (!dtAmortization.Columns.Contains("Others"))
            {
                dtAmortization.Columns.Add("Others");
            }
            if (!dtAmortization.Columns.Contains("Balance Payable"))
            {
                dtAmortization.Columns.Add("Balance Payable");
            }
            if (!dtAmortization.Columns.Contains("Cash Flow"))
            {
                dtAmortization.Columns.Add("Cash Flow");
            }


            grvAmortization.Columns[1].Visible = CanShow;
            grvAmortization.Columns[3].Visible = CanShow;

            return dtAmortization;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to load Amortization Schedule");
        }
    }

    protected DataTable FunProUpdateOtherAmounts(DataTable dtFrom, DataTable dtTo, string strColumn)
    {
        try
        {
            if (dtTo.Rows.Count >= dtFrom.Rows.Count)
            {
                for (int i = 0; i <= dtFrom.Rows.Count - 1; i++)
                {
                    dtTo.Rows[i][strColumn] = dtFrom.Rows[i][strColumn].ToString();
                }
            }

            //foreach (DataRow DRow in dtFrom.Rows)
            //{
            //    DataRow[] dtrRows = dtTo.Select("Date = '" + DRow["InstallmentDate"].ToString());

            //    if (dtrRows.Length > 0)
            //    {
            //        dtrRows[0][strColumn] = Convert.ToDecimal(DRow["InstallmentAmount"]);
            //        dtrRows[0].AcceptChanges();

            //    }
            //}

            return dtTo;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to load Amortization Schedule");
        }
    }

    #endregion

    protected void FunProExport(GridView Grv, string FileName)
    {
        try
        {
            //Type ExcellType = Type.GetTypeFromProgID("Excel.Application");
            //if (ExcellType == null)
            //{
            //Utility.FunShowAlertMsg(this, "Cannot export file. MS-Excel is not istalled in this System.");
            //return;
            //}

            string attachment = "attachment; filename=" + FileName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            if (Grv.Rows.Count > 0)
            {
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Export into Excel");
        }
    }

    public void FunProToggleSLA(bool CanEnable)
    {
        try
        {
            //rfvSubAc.Enabled = rfvSubAc1.Enabled = CanEnable;
            ddlSLA.Enabled = CanEnable;
            if (CanEnable)
            {
                lblSLA.CssClass = "styleReqFieldLabel";
            }
            else
            {
                lblSLA.CssClass = "";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to disable Sub Account Number");
        }
    }

    public void FunProClearMainTabControls()
    {
        try
        {
            txtActivationDate.Text =
                txtAccountCreationDate.Text =
                txtFinanceAmount.Text =
                txtBusinessIRR.Text =
                txtCompanyIRR.Text =
                txtAccIRR.Text =
                txtStatus.Text = "";

            ddlSLA.Items.Clear();
            S3GCustomerAddress1.ClearCustomerDetails();

            //Added by Thangam M 0n 21/Feb/2012
            btnSave.Enabled = true;

            txtActivationDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);

            grvAmortization.DataSource = grvFile.DataSource = grvInvoice.DataSource = null;
            grvAmortization.DataBind();
            grvInvoice.DataBind();
            grvFile.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Clear Main tab controls");
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
        Procparam.Add("@Program_Id", obj_Page.ProgramCode);
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }


    protected void btnCalIRR_Click(object sender, EventArgs e)
    {
        try
        {
            GenerateRepaymentSchedule();
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }


    protected void GenerateRepaymentSchedule()
    {
        try
        {

            //Getting Account details for repayment structure and IRR calculation
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@CompanyId", intCompanyID.ToString());
            Procparam.Add("@PANum", ddlMLA.SelectedValue);
            if (ddlSLA.SelectedIndex > 0)
                Procparam.Add("@SANum", ddlSLA.SelectedValue);
            //cashOutFLow
            DataTable dtOutflow = null; DataTable dtInflow = null;
            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
            DataTable DtRepayGrid = null; DataTable dtRepayDetailsOthers = null;
            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
            DataSet DS = Utility.GetDataset("S3G_LAD_GET_ACCTDTLS", Procparam);

            string strTenure, strTenureType, strLob, strLobid, strRoundoff, strhdnPLR, strhdnCTR, strReturn_Pattern, strMarginMoneyPer_Cashflow
                , strRate, strTime_Value, strRepayment_Mode, strFrequency, strRecovery_Year1, strRecovery_Year2, strRecovery_Year3
                , strRecovery_YearRest, strIRR_Rest, strResidualValue_Cashflow, strResidualAmt_Cashflow, strtxtFBDate;
            decimal decFinAmount = 0;
            DateTime dtStartDate = Utility.StringToDate(txtAccountCreationDate.Text);

            strTenure = strTenureType = strLob = strLobid = strRoundoff = strhdnPLR = strhdnCTR = strReturn_Pattern = strMarginMoneyPer_Cashflow
                       = strRate = strTime_Value = strRepayment_Mode = strFrequency = strRecovery_Year1 = strRecovery_Year2 = strRecovery_Year3
                            = strRecovery_YearRest = strIRR_Rest = strResidualValue_Cashflow = strResidualAmt_Cashflow = strtxtFBDate = string.Empty;


            if (DS.Tables[0].Rows.Count > 0)//Header Part
            {
                DataTable dthdr = DS.Tables[0].Copy();
                strTenure = dthdr.Rows[0]["Tenure"].ToString();
                strTenureType = dthdr.Rows[0]["TenureType"].ToString();
                strMarginMoneyPer_Cashflow = dthdr.Rows[0]["Offer_Margin"].ToString();
                if (!string.IsNullOrEmpty(dthdr.Rows[0]["Loan_Amount"].ToString()))
                    decFinAmount = Convert.ToDecimal(dthdr.Rows[0]["Loan_Amount"].ToString());
                strResidualAmt_Cashflow = dthdr.Rows[0]["Offer_Residual_Value_Amount"].ToString();
                strResidualValue_Cashflow = dthdr.Rows[0]["Offer_Residual_Value"].ToString();
                strtxtFBDate = dthdr.Rows[0]["FB_Date"].ToString();
            }

            if (DS.Tables[1].Rows.Count > 0)//ROI Part
            {
                DataTable dtROI = DS.Tables[1].Copy();
                strRate = dtROI.Rows[0]["rate"].ToString();
                strReturn_Pattern = dtROI.Rows[0]["return_pattern"].ToString();
                strTime_Value = dtROI.Rows[0]["time_value"].ToString();
                strRepayment_Mode = dtROI.Rows[0]["Repayment_Mode"].ToString();
                strFrequency = dtROI.Rows[0]["frequency"].ToString();
                strRecovery_Year1 = dtROI.Rows[0]["recovery_pattern_year1"].ToString();
                strRecovery_Year2 = dtROI.Rows[0]["recovery_pattern_year2"].ToString();
                strRecovery_Year3 = dtROI.Rows[0]["recovery_pattern_year3"].ToString();
                strRecovery_YearRest = dtROI.Rows[0]["recovery_pattern_rest"].ToString();
                strIRR_Rest = dtROI.Rows[0]["irr_rest"].ToString();
            }
            if (DS.Tables[2].Rows.Count > 0)//cashInFLow
            {
                dtInflow = DS.Tables[2].Clone();
            }

            if (DS.Tables[3].Rows.Count > 0)//cashOutFLow
            {
                dtOutflow = DS.Tables[3].Copy();

                //Handling Last Payment date as outflow date start
                DataRow drOutFlw = dtOutflow.Select("CashFlow_Flag_ID=41").Last();
                drOutFlw.BeginEdit();
                drOutFlw["Date"] = dtStartDate;
                drOutFlw.EndEdit();

                //Handling Last Payment date as outflow date end

            }

            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
            if (DS.Tables[4].Rows.Count > 0)//Repayment
            {
                DtRepayGrid = DS.Tables[4].Copy();
                ViewState["DtRepayGrid"] = DtRepayGrid;
            }
            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end


            ViewState["dtOutflow"] = dtOutflow;
            DataSet dsRepayGrid = new DataSet();
            DataTable dtMoratorium = null;
            Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
            //Header Start
            objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text);
            objMethodParameters.Add("LobId", ddlLOB.SelectedValue);
            objMethodParameters.Add("CompanyId", intCompanyID.ToString());
            //objMethodParameters.Add("DocumentDate", (txtActivationDate.Text == "") ? txtApplicationDate.Text : txtAccountDate.Text);//Activation Date
            objMethodParameters.Add("DocumentDate", txtAccountCreationDate.Text);//Activation Date
            objMethodParameters.Add("Tenure", strTenure);//tble0//Tenure
            objMethodParameters.Add("TenureType", strTenureType);//tble0//TenureType
            objMethodParameters.Add("MarginPercentage", strMarginMoneyPer_Cashflow);//tble0//Offer_Margin
            objMethodParameters.Add("FinanceAmount", decFinAmount.ToString());//tble0//Loan_Amount

            //For Round OFF and PLR,CTR
            DataTable dtIRRDetails = Utility.FunPubGetGlobalIRRDetails(intCompanyID, null);
            ViewState["IRRDetails"] = dtIRRDetails;
            dtIRRDetails.DefaultView.RowFilter = "LOB_ID = " + ddlLOB.SelectedValue;
            dtIRRDetails = dtIRRDetails.DefaultView.ToTable();
            if (dtIRRDetails.Rows.Count > 0)
            {
                strhdnCTR = dtIRRDetails.Rows[0]["Corporate_Tax_Rate"].ToString();
                strhdnPLR = dtIRRDetails.Rows[0]["Prime_Lending_Rate"].ToString();
                strRoundoff = dtIRRDetails.Rows[0]["Roundoff"].ToString();
            }
            if (!string.IsNullOrEmpty(strRoundoff))
            {
                objMethodParameters.Add("Roundoff", strRoundoff);
            }
            else
            {
                objMethodParameters.Add("Roundoff", "2");
            }

            //ROI Start
            objMethodParameters.Add("ReturnPattern", strReturn_Pattern);//tble1//return_pattern

            objMethodParameters.Add("Rate", strRate);//tble1//rate
            objMethodParameters.Add("TimeValue", strTime_Value);//tble1//time_value
            objMethodParameters.Add("RepaymentMode", strRepayment_Mode);//tble1//Repayment_Mode



            objMethodParameters.Add("Frequency", strFrequency);//tble1//frequency
            objMethodParameters.Add("RecoveryYear1", strRecovery_Year1);//tble1//recovery_pattern_year1
            objMethodParameters.Add("RecoveryYear2", strRecovery_Year2);//tble1//recovery_pattern_year2
            objMethodParameters.Add("RecoveryYear3", strRecovery_Year3);//tble1//recovery_pattern_year3
            objMethodParameters.Add("RecoveryYear4", strRecovery_YearRest);//tble1//recovery_pattern_rest
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE") && strRepayment_Mode != "5" && strReturn_Pattern == "6")
            {
                objMethodParameters.Add("PrincipalMethod", "1");
            }
            else
            {
                objMethodParameters.Add("PrincipalMethod", "0");
            }

            if (!string.IsNullOrEmpty(strtxtFBDate))
            {
                if (Convert.ToInt32(strtxtFBDate) > 0)
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
                        strFBDate = DateTime.Parse(dtDocDate.Month + "/" + strtxtFBDate + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
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

            if (strReturn_Pattern == "2")
            {
                if (strResidualAmt_Cashflow.Trim() != "" && strResidualAmt_Cashflow.Trim() != "0")
                {
                    if (Convert.ToDecimal(strResidualAmt_Cashflow) > 0)
                        objMethodParameters.Add("decResidualAmount", strResidualAmt_Cashflow);//tble0//Offer_Residual_Value_Amount
                }
                if (strResidualValue_Cashflow.Trim() != "" && strResidualValue_Cashflow.Trim() != "0")
                {
                    if (Convert.ToDecimal(strResidualValue_Cashflow) > 0)
                        objMethodParameters.Add("decResidualValue", strResidualValue_Cashflow);//tble0//Offer_Residual_Value
                }
                switch (strIRR_Rest)//tble1//irr_rest
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
                //if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
                //{
                //    dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, dtInflow, dtOutflow, objMethodParameters, dtMoratorium, out decRateOut);
                //}
                //else
                //{
                dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, dtInflow, dtOutflow, objMethodParameters, dtMoratorium, out decRateOut);
                //}
                ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);
            }
            else
            {
                dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, objMethodParameters, dtMoratorium);
            }

            //decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text);
            if (dsRepayGrid == null)
            {
                // It Calculates and displays the Repayment Details for ST-ADHOC //
                /*  grvRepayStructure.DataSource = null;
                  grvRepayStructure.DataBind();
                  FunPriShowRepaymetDetails(decFinAmount + FunPriGetStructureAdhocInterestAmount());
                  gvRepaymentDetails.FooterRow.Visible = true;
                  btnReset.Enabled = true;*/
                return;
            }
            if (dsRepayGrid.Tables[0].Rows.Count > 0)
            {
                //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                DtRepayGrid = dsRepayGrid.Tables[0];

                //gvRepaymentDetails.DataSource = dsRepayGrid.Tables[0];
                //gvRepaymentDetails.DataBind();
                DataTable DtRepayGridtemp = ((DataTable)ViewState["DtRepayGrid"]).Copy();
                DataRow[] drRepayGridtemp = DtRepayGridtemp.Select("CashFlow_Flag_ID = 23");
                if (drRepayGridtemp.Length > 0)
                {
                    foreach (DataRow drRepayGridte in drRepayGridtemp)
                        drRepayGridte.Delete();
                    DtRepayGridtemp.AcceptChanges();
                }
                //DtRepayGrid.Merge(DtRepayGridtemp);

                foreach (DataRow drr in DtRepayGridtemp.Rows)
                {
                    DataRow DtRepayGridRw = DtRepayGrid.NewRow();
                    foreach (DataColumn dc in DtRepayGrid.Columns)
                    {
                        if (DtRepayGrid.Columns.Contains(dc.ColumnName) && DtRepayGridtemp.Columns.Contains(dc.ColumnName))
                        {
                            if (dc.ColumnName == "Amount")
                                DtRepayGridRw[dc.ColumnName] = Convert.ToDecimal(drr[dc.ColumnName].ToString());
                            else if (dc.ColumnName == "CashFlow_Flag_ID")
                                DtRepayGridRw[dc.ColumnName] = Convert.ToInt16(drr[dc.ColumnName].ToString());
                            else
                                DtRepayGridRw[dc.ColumnName] = drr[dc.ColumnName].ToString();
                        }
                    }
                    DtRepayGrid.Rows.Add(DtRepayGridRw);
                    DtRepayGrid.AcceptChanges();
                }



                ViewState["DtRepayGrid"] = DtRepayGrid;

                //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end

                //if (ddl_Rate_Type.SelectedItem.Text == "Floating" && string.IsNullOrEmpty(txtFBDate.Text))
                //{
                //    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
                //    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
                //}
                //else
                //{
                //    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
                //    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
                //}
                //btnReset.Enabled = false;
                //FunPriCalculateSummary(dsRepayGrid.Tables[0], "CashFlow", "TotalPeriodInstall");
                //decimal decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
                decimal decBreakPercent;// = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
                if (!((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))))
                {
                    decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
                }
                else
                {
                    DataRow[] dr = (((DataTable)ViewState["DtRepayGrid"])).Select("CashFlow_Flag_ID IN(35,91)");
                    if (dr.Length == 0)
                    {
                        decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
                    }
                    else
                    {
                        decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID in(91,35)"));
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
                //if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
                //{
                //    DataTable dsAssetDetails = (DataTable)Session["ApplicationAssetDetails"];
                //    decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));

                //    objRepaymentStructure.FunPubCalculateIRR(txtActivationDate.Text, strhdnPLR,strFrequency, strDateFormat, strResidualAmt_Cashflow, strResidualValue_Cashflow, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                //        , out dtRepaymentStructure, (DataTable)ViewState["DtRepayGrid"],dtInflow, dtOutflow
                //        , decFinAmount.ToString(), strMarginMoneyPer_Cashflow, strReturn_Pattern
                //        , strRate, strTenureType, strTenure, strIRR_Rest,
                //        strTime_Value, ddlLOB.SelectedItem.Text, strRepayment_Mode, "", dtMoratorium);
                //}
                //else
                //{
                objRepaymentStructure.FunPubCalculateIRR(txtAccountCreationDate.Text, strhdnPLR, strFrequency, strDateFormat, strResidualAmt_Cashflow, strResidualValue_Cashflow, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                 , out dtRepaymentStructure
                    //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                    , out dtRepayDetailsOthers
                    //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                 , (DataTable)ViewState["DtRepayGrid"], dtInflow, dtOutflow
                 , decFinAmount.ToString(), strMarginMoneyPer_Cashflow, strReturn_Pattern
                 , strRate, strTenureType, strTenure, strIRR_Rest,
                 strTime_Value, ddlLOB.SelectedItem.Text, strRepayment_Mode, "", dtMoratorium, false);




                //}
                dtRepaymentStructure.Columns["Charge"].ColumnName = "FinanceCharges";
                ViewState["RepaymentStructure"] = dtRepaymentStructure;
                //grvRepayStructure.DataSource = dtRepaymentStructure;
                //grvRepayStructure.DataBind();


                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
                if (dtRepayDetailsOthers != null)
                    ViewState["dtRepayDetailsOthers"] = dtRepayDetailsOthers;
                //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end

                //txtAccountIRR_Repay.Text = douAccountingIRR.ToString("0.0000");
                txtAccIRR.Text = douAccountingIRR.ToString("0.0000");

                //txtBusinessIRR_Repay.Text = douBusinessIRR.ToString("0.0000");
                txtBusinessIRR.Text = douBusinessIRR.ToString("0.0000");

                //txtCompanyIRR_Repay.Text = douCompanyIRR.ToString("0.0000");
                txtCompanyIRR.Text = douCompanyIRR.ToString("0.0000");

            }
            else
            {
                //gvRepaymentDetails.FooterRow.Visible = true;
                //btnReset.Enabled = true;
                //btnCalIRR.Enabled = true;
                ViewState["RepaymentStructure"] = null;
                txtBusinessIRR.Text = txtAccIRR.Text = txtCompanyIRR.Text = string.Empty;
                //grvRepayStructure.DataSource = null;
                //grvRepayStructure.DataBind();
            }
            //decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text);
            //if (dsRepayGrid.Tables[0].Rows.Count > 0)
            //{
            //    FunPriShowRepaymetDetails((decimal)dsRepayGrid.Tables[0].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
            //}
            //else
            //{
            //    FunPriShowRepaymetDetails(decFinAmount + FunPriGetInterestAmount());
            //}

            //FunPriGenerateNewRepaymentRow();
            //FunPriUpdateROIRule();
            //if (ddl_Repayment_Mode.SelectedValue != "2")
            //{
            //    Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
            //    if (lblCashFlowId.Text != "23")
            //    {
            //        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
            //    }
            //}
            //else
            //{
            //    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
            //}

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, "Activation");
            throw ex;
        }
    }
    #endregion
}
