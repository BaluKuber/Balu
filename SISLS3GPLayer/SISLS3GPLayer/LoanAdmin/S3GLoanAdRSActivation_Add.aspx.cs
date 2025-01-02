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

public partial class LoanAdmin_S3GLoanAdRSActivation_Add : ApplyThemeForProject
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
    static DataTable dtinterimtotal;
    static DataTable dtintAMF;
    //Code end

    ContractMgtServicesReference.ContractMgtServicesClient objAccActivation_Client;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable objS3G_LOANAD_ActivationDataTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationRow objS3G_LOANAD_ActivationDataRow = null;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACBAC";

    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdRSActivation_Add.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACBAC';";

    string strNewWin = " window.showModalDialog('../LoanAdmin/S3GLoanAdInvoiceVendor_Add.aspx";
    string NewWinAttributes = "', 'Invoice Vendor Details', 'dialogwidth:800px;dialogHeight:950px;');";

    string strNewWinProforma = " window.showModalDialog('../Origination/S3GORGProforma_Add.aspx";
    string NewWinAttributesProforma = "', 'Proforma Details', 'dialogwidth:800px;dialogHeight:550px;');";

    public static LoanAdmin_S3GLoanAdRSActivation_Add obj_Page;
    static string strPageName = "RS Bulk Activation";
    #endregion

    #region Page Loading

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string name = PostBackControlId;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            obj_Page = this;
            //Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
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

            #region " WF INITIATION"
            ProgramCode = "334";
            #endregion

            if (!IsPostBack)
            {
                //FunPubSetIndex(1);
                FunPriSetPrefixSuffixLength();

                if (Request.QueryString["qsMode"] == "C")
                {
                    FunProPopulateBranchList();
                    FunProPopulateinterimList();
                }

                //txtActivationDate.Attributes.Add("readonly", "readonly");
                CalendarExtender1.Format = strDateFormat;
                clndrFrom.Format = strDateFormat;
                clndrTo.Format = strDateFormat;
                txtActivationDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtActivationDate.ClientID + "','" + strDateFormat + "',false,  false);");
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
                //rbtnType_SelectedIndexChanged(sender, e);
            }
            FunLOBRelatedDetails();
            FunLoadBranchRelatedDetails();
            //LoadMLARelatedDetails();
        }
    }
    #endregion


    #endregion

    #region Page Events

    #region Save Clear Cancel Go

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strPANum = "";

        foreach (GridViewRow grvRow in grvRSDet.Rows)
        {
            if (((CheckBox)grvRow.FindControl("chkSelect")).Checked)
            {
                strPANum += ((Label)grvRow.FindControl("lblRSNo")).Text + ",";
            }
        }
        //Added on 18Jun2015 for Round 1 Bug Fixing starts here
        if (Convert.ToString(strPANum) == "")
        {
            Utility.FunShowAlertMsg(this, "Select atleast one RS Number to Activate");
            return;
        }

        int flag = 0;

        string StrXMLAccounts = funpubcreatexml(ref flag);

        if (flag == 1 && (txtInterFrom.Text == "" || txtInterTo.Text == "") && ddlInterimMethod.SelectedValue == "11" && (Convert.ToDecimal(lbltotinterim.Text) + Convert.ToDecimal(lblTotAmf.Text)) > 0)
        {
            Utility.FunShowAlertMsg(this, "Interim Period From and Interim Period To should be entered");
            return;
        }

        if (ddlInterimMethod.SelectedValue == "11")
        {
            string str = string.Empty;
            string strAMF = string.Empty;
            for (int i = 0; i < grvRSDet.Rows.Count; i++)
            {
                if (((Label)grvRSDet.Rows[i].FindControl("lblInterimRentGridavg")).Text == "0" && ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).Text != "0")
                {
                    str += ((Label)grvRSDet.Rows[i].FindControl("lblRSNo")).Text + ",";
                }
            }
            for (int i = 0; i < grvRSDet.Rows.Count; i++)
            {
                if (Convert.ToDecimal(((Label)grvRSDet.Rows[i].FindControl("lblInterimAMFGridavg")).Text) == Convert.ToDecimal(0) && ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).Text != "0")
                {
                    strAMF += ((Label)grvRSDet.Rows[i].FindControl("lblRSNo")).Text + ",";
                }
            }

            if (strAMF != string.Empty)
            {
                Utility.FunShowAlertMsg(this, "AMF is not applicable for Rental Schedule Number " + strAMF.Substring(0, strAMF.Length - 1));
                return;
            }

            if (str != string.Empty)
            {
                Utility.FunShowAlertMsg(this, "Rental is not applicable for Rental Schedule Number " + str.Substring(0, str.Length - 1));
            }
        }

        objAccActivation_Client = new ContractMgtServicesReference.ContractMgtServicesClient();
        try
        {
            //if (Page.IsValid)
            //{
            if (Convert.ToInt32(ViewState["Created_By"]) != intUserID)
            {

                //Added on 18Jun2015 for Round 1 Bug Fixing ends here

                objS3G_LOANAD_ActivationDataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable();
                objS3G_LOANAD_ActivationDataRow = objS3G_LOANAD_ActivationDataTable.NewS3G_LOANAD_AccountActivationRow();
                objS3G_LOANAD_ActivationDataRow.Account_Activation_Number = strAccountActivation;
                objS3G_LOANAD_ActivationDataRow.Company_ID = intCompanyID;
                objS3G_LOANAD_ActivationDataRow.Created_By = intUserID;

                if (Convert.ToInt32(ddlInterimMethod.SelectedValue.ToString()) > 0)
                    objS3G_LOANAD_ActivationDataRow.interim_type = Convert.ToInt32(ddlInterimMethod.SelectedValue.ToString());
                if (txtInterimRate.Text != "")
                    objS3G_LOANAD_ActivationDataRow.Interim_Rate = txtInterimRate.Text.ToString();
                if (txtInterimRent.Text != "")
                    objS3G_LOANAD_ActivationDataRow.Interim_amount = txtInterimRent.Text.ToString();
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

                objS3G_LOANAD_ActivationDataRow.XMLRepayStruct = StrXMLAccounts; //strPANum;

                if (ddlTranche.SelectedValue != "0")
                    objS3G_LOANAD_ActivationDataRow.AccountIRR = ddlTranche.SelectedValue;

                objS3G_LOANAD_ActivationDataTable.AddS3G_LOANAD_AccountActivationRow(objS3G_LOANAD_ActivationDataRow);

                intErrCode = objAccActivation_Client.FunPubCreateBulkActivation(out strErrMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_ActivationDataTable, ObjSerMode));

                if (intErrCode == 106)
                {
                    Utility.FunShowAlertMsg(this, "Interim From Date should be greater or equal to RS Creation Date");
                    return;
                }

                //txn amount not get tallied
                if (intErrCode == 105)
                {
                    Utility.FunShowAlertMsg(this, "Error in Posting");
                    return;
                }

                /*Mar 9,2016 - Vinodha M - Validation - child contract start date should be greater than parent contract closure date in re write cases*/
                if (intErrCode == 107)
                {
                    Utility.FunShowValidationMsg(this.Page, "ACT", intErrCode, strErrMsg, false);
                    return;
                }
                /*Mar 9,2016 - Vinodha M - Validation - child contract start date should be greater than parent contract closure date in re write cases*/

                if (intErrCode == 108)
                {
                    Utility.FunShowAlertMsg(this, "Rental Schedule already activated.");
                    return;
                }

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
                        //strAlert += @"\n\nActivation Number - " + strErrMsg;
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
            string strScipt = "window.open('../LoanAdmin/btnPrint_Click.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Flow Print", strScipt, true);

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
            DataTable dtrsbulkcsh = new DataTable();
            dtrsbulkcsh = (DataTable)ViewState["DSSetTab1"];
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@XmlRSNumber", dtrsbulkcsh.FunPubFormXml());
            dsprint = Utility.GetDataset("S3G_Fund_RSBulkAct_Print", Procparam);
            Session["Is_Print"] = "2";
            Session["ProcParam"] = Procparam;
            if (dsprint.Tables[1].Rows.Count > 0)
            {
                string strScipt = "window.open('../LoanAdmin/S3G_RPT_RSBulkActivReportPage.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Flow Print", strScipt, true);
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
            if (Convert.ToInt32(ViewState["Activated_By"]) != intUserID)
            {

                objS3G_LOANAD_ActivationDataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable();

                objS3G_LOANAD_ActivationDataRow = objS3G_LOANAD_ActivationDataTable.NewS3G_LOANAD_AccountActivationRow();

                objS3G_LOANAD_ActivationDataRow.MLAStatus = Convert.ToInt32(ViewState["MLAStatus"]);
                objS3G_LOANAD_ActivationDataRow.IsModify = 0;
                objS3G_LOANAD_ActivationDataRow.IsRevoke = 1;
                objS3G_LOANAD_ActivationDataRow.Account_Activation_Number = strAccountActivation;
                objS3G_LOANAD_ActivationDataRow.Company_ID = intCompanyID;
                objS3G_LOANAD_ActivationDataRow.Created_By = intUserID;
                objS3G_LOANAD_ActivationDataRow.Account_Activated_Date = Utility.StringToDate(txtActivationDate.Text);

                string PANum = "";
                PANum += ("<Root>");

                foreach (GridViewRow grvRow in grvRSDet.Rows)
                {

                    if (((CheckBox)grvRow.FindControl("chkSelect")).Checked)
                    {
                        PANum += ("<Details ");
                        PANum += "RS_NO = '" + ((Label)grvRow.FindControl("lblRSNo")).Text + " '";
                        PANum += ("/> ");
                        //PANum += ((Label)grvRow.FindControl("lblRSNo")).Text;

                    }
                }

                PANum += ("</Root>");

                objS3G_LOANAD_ActivationDataRow.XMLRepayStruct = PANum;

                objS3G_LOANAD_ActivationDataTable.AddS3G_LOANAD_AccountActivationRow(objS3G_LOANAD_ActivationDataRow);
                intErrCode = objAccActivation_Client.FunPubCreateBulkActivation(out strErrMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_ActivationDataTable, ObjSerMode));
                if (intErrCode == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule has been revoked successfully');" + strRedirectPageView, true);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Migrated Rental Schedule Cannot Revoke - " + strErrMsg + "');", true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 11)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Note has been Approved. Cannot Revoke Rental Schedule - " + strErrMsg + "');", true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 12)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Transaction has been done. Cannot Revoke Rental Schedule - " + strErrMsg + "');", true);
                    lblErrorMessage.Text = string.Empty;
                }
                else if (intErrCode == 13)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Invoice Generated. Cannot Revoke Rental Schedule - " + strErrMsg + "');", true);
                    lblErrorMessage.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Rental Schedule cannot be revoked by the same user who activated the Rental Schedule');" + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
        finally
        {
            objAccActivation_Client.Close();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunProClearMainTabControls();
            FunProToggleSLA(false);

            ScriptManager.RegisterStartupScript(this, GetType(), "te", "fnClearAllTab(false);", true);
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void hyplnkViewPre_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvRSDet", strFieldAtt);
            Label lblPath = (Label)grvRSDet.Rows[gRowIndex].FindControl("lblPath");

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
            GridView gvSystemJournal = new GridView();

            if (ViewState["JVDet"] != null)
            {
                gvSystemJournal.DataSource = (DataTable)ViewState["JVDet"];
                gvSystemJournal.DataBind();
            }
            FunProExport(gvSystemJournal, "JournalVoucher");
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void btnExportAmort_Click(object sender, EventArgs e)
    {
        try
        {
            FunProGetAmortizationSchedule();
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void btnExportAmortTranche_Click(object sender, EventArgs e)
    {
        try
        {
            FunProGetAmortizationTranche();

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
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvRSDet", strFieldAtt);
            Label lblRSNo = (Label)grvRSDet.Rows[gRowIndex].FindControl("lblRSNo");

            DataTable dtTable = new DataTable();

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@PANum", lblRSNo.Text);
            dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_AccountViewFromActivation, Procparam);

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(dtTable.Rows[0]["ID"].ToString(), false, 0);
            string strScipt = "window.open('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&IsFromAccount=Y&qsMode=Q', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=no,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
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

    //protected void gvSystemJournal_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            Label lblDocDate = (Label)e.Row.FindControl("lblDocDate");
    //            Label lblValueDate = (Label)e.Row.FindControl("lblValueDate");
    //            Label lblStatus = (Label)e.Row.FindControl("lblStatus");

    //            //lblDocDate.Text = Utility.StringToDate(lblDocDate.Text).ToString(strDateFormat);
    //            //lblValueDate.Text = Utility.StringToDate(lblValueDate.Text).ToString(strDateFormat);

    //            lblDocDate.Text = DateTime.Parse(lblDocDate.Text, CultureInfo.CurrentCulture).ToString(strDateFormat);
    //            lblValueDate.Text = DateTime.Parse(lblValueDate.Text, CultureInfo.CurrentCulture).ToString(strDateFormat);

    //            if (lblStatus.Text == "Revoked")
    //            {
    //                e.Row.BackColor = System.Drawing.Color.WhiteSmoke;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        custRouterLogic.ErrorMessage = ex.Message;
    //        custRouterLogic.IsValid = false;
    //    }
    //}

    //protected void gvPDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            Label txtColletedDate = (Label)e.Row.FindControl("txtColletedDate");
    //            Label txtScannedDate = (Label)e.Row.FindControl("txtScannedDate");
    //            Label txtScannedBy = (Label)e.Row.FindControl("txtScannedBy");
    //            TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
    //            ImageButton hyplnkView = (ImageButton)e.Row.FindControl("hyplnkView");
    //            Label lblCanView = (Label)e.Row.FindControl("lblCanView");
    //            txtRemarks.Attributes.Add("readonly", "readonly");

    //            if (txtColletedDate.Text.Contains("1900"))
    //            {
    //                txtColletedDate.Text = "";
    //            }
    //            if (txtScannedDate.Text.Contains("1900"))
    //            {
    //                txtScannedDate.Text = "";
    //            }

    //            if (lblCanView.Text.Trim() == "0")
    //            {
    //                hyplnkView.Visible = txtScannedBy.Visible = txtScannedDate.Visible = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    //protected void gvPRDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            Label txtColletedDate = (Label)e.Row.FindControl("txtColletedDate");
    //            Label txtScannedDate = (Label)e.Row.FindControl("txtScannedDate");
    //            Label txtScannedBy = (Label)e.Row.FindControl("txtScannedBy");
    //            TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
    //            ImageButton hyplnkViewPre = (ImageButton)e.Row.FindControl("hyplnkViewPre");
    //            Label lblCanView = (Label)e.Row.FindControl("lblCanView");
    //            txtRemarks.Attributes.Add("readonly", "readonly");

    //            if (txtColletedDate.Text.Contains("1900"))
    //            {
    //                txtColletedDate.Text = "";
    //            }
    //            if (txtScannedDate.Text.Contains("1900"))
    //            {
    //                txtScannedDate.Text = "";
    //            }

    //            if (lblCanView.Text.Trim() == "0")
    //            {
    //                hyplnkViewPre.Visible = txtScannedBy.Visible = txtScannedDate.Visible = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    protected void grvRSDet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("chkSelectAll");
                chkSelectAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvRSDet.ClientID + "',this,'chkSelect');");
                if (strMode != "C")
                    chkSelectAll.Checked = true;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && strMode != "C")
            {
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                chkSelect.Checked = true;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!(txtInterimRent.Text.Equals("")))
                {
                    ((TextBox)e.Row.FindControl("lblInterimRentGrid")).ReadOnly = true;
                }
                if (!(txtInterAMF.Text.Equals("")))
                {
                    ((TextBox)e.Row.FindControl("lblInterimAMFGrid")).ReadOnly = true;
                }
            }

        }
        catch (Exception exp)
        {

        }
    }

    #endregion

    #region DDL Events

    //protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //FunProPopulateBranchList();
    //    FunLOBRelatedDetails();
    //}

    protected void ddlInterim_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlInterimMethod.SelectedValue == "1" || ddlInterimMethod.SelectedValue == "2")
            {
                txtInterimRate.ReadOnly = false;
                rfvInterimRate.Enabled = rfvInterimRate.Visible = true;
            }
            else
            {
                txtInterimRate.ReadOnly = true;
                rfvInterimRate.Enabled = rfvInterimRate.Visible = false;
            }
            if (ddlInterimMethod.SelectedValue == "11")
            {
                //rfvInterimRent.Enabled = rfvAMF.Enabled = rfvInterimfrom.Enabled = rfvInterTo.Enabled = true;
                txtInterimRent.ReadOnly = txtInterAMF.ReadOnly = txtInterFrom.ReadOnly = txtInterTo.ReadOnly = false;
                clndrFrom.Enabled = clndrTo.Enabled = true;

                for (int i = 0; i < grvRSDet.Rows.Count; i++)
                {
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).Text = "0";
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = false;
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).Text = "0";
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = false;
                }

                //rfvInterimRent.Enabled = rfvInterimRent.Visible = true;
            }
            else
            {
                rfvInterimRent.Enabled = rfvAMF.Enabled = rfvInterimfrom.Enabled = rfvInterTo.Enabled = false;
                txtInterimRent.ReadOnly = txtInterAMF.ReadOnly = txtInterFrom.ReadOnly = txtInterTo.ReadOnly = true;
                clndrFrom.Enabled = clndrTo.Enabled = false;
                rfvInterimRent.Enabled = rfvInterimRent.Visible = false;
                lblTotAmf.Text = lbltotinterim.Text = "0";

                for (int i = 0; i < grvRSDet.Rows.Count; i++)
                {
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).Text = string.Empty;
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = true;
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).Text = string.Empty;
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = true;
                }

            }
            txtInterimRate.Text = txtInterimRent.Text = string.Empty;
            lblInterimRent.CssClass = (ddlInterimMethod.SelectedValue == "11") ? "styleReqFieldLabel" : "styleDisplayLabel";
            lblInterimRate.CssClass = (ddlInterimMethod.SelectedValue == "1" || ddlInterimMethod.SelectedValue == "2") ? "styleReqFieldLabel" : "styleDisplayLabel";
        }
        catch (Exception objException)
        {
            custRouterLogic.ErrorMessage = objException.Message;
            custRouterLogic.IsValid = false;
        }
    }

    void FunLOBRelatedDetails()
    {
        try
        {
            FunProClearMainTabControls();
            FunProGetGloblaMLASLA();
            lblErrorMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void ddlLessee_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLessee.SelectedValue != "0")
        {
            FunProPopulateCheckInterimCustomer();
            LoadTranche(ddlLessee.SelectedValue);
            FunPriLoadRSDet();
        }
    }

    protected void LoadTranche(string strCustomer_ID)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "2");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Customer_ID", strCustomer_ID);
            ddlTranche.BindDataTable("S3G_LAD_GetRSLst_AGT", Procparam, new string[] { "Tranche_Header_Id", "Tranche_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunPriLoadRSDet()
    {
        try
        {
            if (Convert.ToInt64(ddlLessee.SelectedValue) == 0 || Convert.ToString(txtActivationDate.Text) == "")
            {
                grvRSDet.DataSource = null;
                grvRSDet.DataBind();
                btnExportAmort.Enabled = false;
                btnExportAmortTranche.Enabled = false;
                return;
            }

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Customer_ID", ddlLessee.SelectedValue);
            if (ddlTranche.SelectedValue != "0")
                Procparam.Add("@Tranche", ddlTranche.SelectedValue);
            Procparam.Add("@UserID", intUserID.ToString());
            Procparam.Add("@Activation_Date", Utility.StringToDate(txtActivationDate.Text).ToString());
            DataSet dtRSDet = Utility.GetDataset("S3G_LoadAd_GetRs", Procparam);
            grvRSDet.DataSource = dtRSDet.Tables[1];
            ViewState["dtRSDet"] = dtRSDet.Tables[1];
            ViewState["Header"] = dtRSDet.Tables[0].Rows[0]["chkall"].ToString();
            grvRSDet.DataBind();

            btnExportAmort.Enabled = true;
            if (ddlInterimMethod.SelectedIndex == 0)
            {
                rfvInterimRent.Enabled = rfvAMF.Enabled = rfvInterimfrom.Enabled = rfvInterTo.Enabled = false;
                txtInterimRent.ReadOnly = txtInterAMF.ReadOnly = txtInterFrom.ReadOnly = txtInterTo.ReadOnly = true;
                clndrFrom.Enabled = clndrTo.Enabled = false;
                rfvInterimRent.Enabled = rfvInterimRent.Visible = false;

                for (int i = 0; i < grvRSDet.Rows.Count; i++)
                {
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).Text = "0";
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = true;
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).Text = "0";
                    ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = true;
                }


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void ddlTranche_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadRSDet();
    }

    void FunLoadBranchRelatedDetails()
    {
        try
        {
            FunProPopulatePANum(ViewState["MLAStatus"].ToString());
            FunProClearMainTabControls();
            lblErrorMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            custRouterLogic.ErrorMessage = ex.Message;
            custRouterLogic.IsValid = false;
        }
    }

    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadMLARelatedDetails();
    }

    protected void ddlTaxType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txtActivationDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadRSDet();
            lbltotinterim.Text = "0";
            lblTotAmf.Text = "0";
            //FunProGetAmortizationSchedule();
        }
        catch (Exception objException)
        {
            custRouterLogic.ErrorMessage = objException.Message;
            custRouterLogic.IsValid = false;
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

            //if (Utility.StringToDate(txtActivationDate.Text) < Utility.StringToDate(txtAccountCreationDate.Text))
            //{
            //    args.IsValid = false;
            //    custRouterLogic.ErrorMessage = "Rental Schedule Activation date should be greater than or equal to Rental Schedule creation date";
            //    return;
            //}

            if (intGapDays <= Convert.ToInt32(ViewState["Days"]))
            {
                //to be add mla applicable test

                DataSet dsParameterSetup = new DataSet();
                DataTable dtApplicatble = new DataTable();
                DataTable dtINVAIE = new DataTable();

                DataView dtView = new DataView();
                DataTable dtCopy = new DataTable();

                Procparam = new Dictionary<string, string>();
                //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Module_ID", "7");
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@ParameterCode", "1,2,5,8,9,10");
                //Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
                dsParameterSetup = Utility.GetDataset("S3G_LOANAD_GetGlobalParameters", Procparam);

                dtApplicatble = dsParameterSetup.Tables[0];
                dtINVAIE = dsParameterSetup.Tables[1];

                //if ((rbtnType.SelectedIndex == 0 && dtApplicatble.Rows[0]["MLA_Applicable"].ToString() == "0") ||
                //    rbtnType.SelectedIndex == 1)
                //{
                //    if (!FunProLOBBasedValidations(ddlLOB.SelectedItem.Text.ToLower()))
                //    {
                //        args.IsValid = false;
                //        custRouterLogic.ErrorMessage = "Amortization schedule is mandatory for selected Line of Business";
                //        lblErrorMessage.Text = string.Empty;
                //        return;
                //    }

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
                Procparam.Add("@IS_OL", "0");

                DsPreReq = Utility.GetDataset("S3G_LOANAD_CheckForActivationPreReq", Procparam);

                if (Convert.ToBoolean(ViewState["DeliveryOrder"]) == true)
                {
                    DataTable dtDO = DsPreReq.Tables[0];

                    if (dtDO.Rows.Count > 0)
                    {
                        args.IsValid = false;
                        string msg = "";

                        for (int i = 0; i <= dtDO.Rows.Count - 1; i++)
                        {
                            msg += "&nbsp;&nbsp;&nbsp;&nbsp; " + dtDO.Rows[i]["Asset"].ToString() + " (" + dtDO.Rows[i]["Count"].ToString() + ")" + "<br>";
                        }
                        custRouterLogic.ErrorMessage = msg;
                        return;
                    }
                }

                if (Convert.ToBoolean(ViewState["Invoice"]) == true)
                {
                    DataTable dtInv = DsPreReq.Tables[1];
                    DataTable dtInvAssets = DsPreReq.Tables[2];

                    //if ((grvFile.Rows.Count <= 0 && grvInvoice.Rows.Count <= 0 && dtInv.Rows.Count > 0) || dtInvAssets.Rows.Count > 0)
                    //{
                    //    args.IsValid = false;
                    //    string msg = "The Invoice for the following asset(s) are not completed" + "<br><br>";

                    //    for (int i = 0; i <= dtInvAssets.Rows.Count - 1; i++)
                    //    {
                    //        msg += "&nbsp;&nbsp;&nbsp;&nbsp; " + dtInvAssets.Rows[i]["Asset"].ToString() + " (" + dtInvAssets.Rows[i]["Count"].ToString() + ")" + "<br>";
                    //    }

                    //    custRouterLogic.ErrorMessage = msg;
                    //    return;
                    //}
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

            //}
            //else
            //{
            //    args.IsValid = false;
            //    custRouterLogic.ErrorMessage = "The difference between Rental Schedule creation and Activation needs to be with in " + ViewState["Days"].ToString() + " day(s)";
            //    return;
            //}
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
            //lblTaxType.Visible = true;
            //ddlTaxType.Visible = true;
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //ddlTaxType.BindDataTable("S3G_ORG_GETTAXTYPES", Procparam, false, new string[] { "ID", "Name" });
            //ddlTaxType.Items.Insert(0, new ListItem("--Select--", "0"));

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
            btnCashflow.Visible = false;    //Added on 18Jun2015 for Round 1 bug fixing
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }

                    txtActivationDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    hdnactidate.Value = Utility.StringToDate(DateTime.Now.ToString()).ToString();
                    btnExportAmort.Enabled = btnExportJV.Enabled = btnRevoke.Enabled = btnExportAmortTranche.Enabled = false;
                    txtInterimRent.ReadOnly = txtInterimRate.ReadOnly = true;
                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (!bModify)
                    {
                        btnSave.Visible = false;

                    }
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    if (intUserID != 67)
                        btnRevoke.Enabled = false;
                    txtInterimRate.Attributes.Add("readonly", "readonly");
                    txtInterimRent.Attributes.Add("readonly", "readonly");
                    txtActivationDate.Attributes.Add("readonly", "readonly");
                    CalendarExtender1.Enabled = false;
                    for (int i = 0; i < grvRSDet.Rows.Count; i++)
                    {
                        ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = true;
                        ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = true;
                    }
                    break;

                case -1:// Query Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = btnClear.Enabled = btnRevoke.Enabled = false;
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }
                    txtInterimRate.Attributes.Add("readonly", "readonly");
                    txtInterimRent.Attributes.Add("readonly", "readonly");
                    txtActivationDate.Attributes.Add("readonly", "readonly");
                    CalendarExtender1.Enabled = false;
                    custRouterLogic.Enabled = false;
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
            dtTable = Utility.GetDefaultData(SPNames.S3G_SYSAD_GetGlobalMLASLA, Procparam);
            string strMLAStatus = string.Empty;

            if (dtTable.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dtTable.Rows[0]["OnlyMLA"]) == false && Convert.ToBoolean(dtTable.Rows[0]["MLAandSLA"]) == true)
                {
                    strMLAStatus = "0";
                    //FunProToggleSLA(true);
                }
                else if (Convert.ToBoolean(dtTable.Rows[0]["OnlyMLA"]) == true && Convert.ToBoolean(dtTable.Rows[0]["MLAandSLA"]) == false)
                {
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
            ddlInterimMethod.BindDataTable("S3G_ORG_GETMRA_LOOKUP", Procparam, false, new string[] { "ID", "Name" });
            ddlInterimMethod.Items.Insert(0, new ListItem("--Select--", "0"));

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
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Param1", "1");


            Procparam.Add("@Param3", strMLAStatus);
            Procparam.Add("@UserID", intUserID.ToString());
            //ddlMLA.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_List, Procparam, new string[] { "PANum", "PANum", "Customer_name" });

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
            //ddlMLA.BindDataTable(SPNames.S3G_LOANAD_ActivatedPANum, Procparam, new string[] { "PANum", "PANum", });

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
            //ddlSLA.BindDataTable(SPNames.S3G_LOANAD_ActivatedPANum, Procparam, new string[] { "SANum", "SANum" });

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
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
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


                ViewState["ESM"] = false;
                ViewState["IRR"] = false;
                ViewState["SOD"] = false;
                ViewState["Product"] = false;

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
            //DataSet dtSet = new DataSet();
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_Id", intCompanyID.ToString());
            //Procparam.Add("@SANum", strSLAValue);
            //Procparam.Add("@PANum", ddlMLA.SelectedValue);

            //dtSet = Utility.GetTableValues(SPNames.S3G_LOANAD_GetPASARefID, Procparam);
            //if (dtSet.Tables[0].Rows.Count > 0)
            //{
            //    ViewState["AccountDate"] = dtSet.Tables[0].Rows[0]["Creation_Date"].ToString();//Repayment_Code
            //    txtAccountCreationDate.Text = Convert.ToDateTime(dtSet.Tables[0].Rows[0]["Creation_Date"].ToString()).ToString(strDateFormat);

            //    txtBusinessIRR.Text = dtSet.Tables[0].Rows[0]["Business_IRR"].ToString();
            //    txtCompanyIRR.Text = dtSet.Tables[0].Rows[0]["Company_IRR"].ToString();
            //    txtAccIRR.Text = dtSet.Tables[0].Rows[0]["Accounting_IRR"].ToString();
            //    ViewState["Repayment_Code"] = dtSet.Tables[0].Rows[0]["Repayment_Code"].ToString();
            //    ViewState["Return_Pattern"] = dtSet.Tables[0].Rows[0]["Return_Pattern"].ToString();
            //    txtFinanceAmount.Text = dtSet.Tables[0].Rows[0]["Finance_Amount"].ToString();
            //    txtStatus.Text = dtSet.Tables[0].Rows[0]["Status"].ToString();
            //    ViewState["DeliveryOrder"] = dtSet.Tables[0].Rows[0]["Is_Delivery_Order_Require"].ToString();
            //    ViewState["PA_SA_REF_ID"] = dtSet.Tables[0].Rows[0]["PA_SA_REF_ID"].ToString();
            //    ViewState["Created_By"] = dtSet.Tables[0].Rows[0]["Created_By"].ToString();
            //    ViewState["UserLevel"] = dtSet.Tables[0].Rows[0]["User_Level_ID"].ToString();
            //    ViewState["SAN"] = strSLAValue;
            //    lblLeaseType1.Text = dtSet.Tables[0].Rows[0]["Lease_Type"].ToString(); 

            //    //Added by Thangam M on 20/Feb/2012 to check payment for Normal Payment
            //    if ((PageMode == PageModes.Create || PageMode == PageModes.WorkFlow) && dtSet.Tables[0].Rows[0]["Payment"].ToString() != "1")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('100% payment has to be made to activate this Rental Schedule.');", true);
            //        btnSave.Enabled = false;
            //        //ddlMLA.SelectedIndex = 0;
            //        //FunProClearMainTabControls();
            //        //ddlSLA.Items.Clear();
            //        //ddlMLA.Focus();
            //        //return;
            //    }
            //    else
            //    {
            //        if ((PageMode == PageModes.Create || PageMode == PageModes.WorkFlow))
            //        {

            //            if (((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE")))
            //              && ViewState["Repayment_Code"].ToString() != "5" && ViewState["Return_Pattern"].ToString() != "6")//Product ??not principal
            //            {
            //                txtBusinessIRR.Text = txtCompanyIRR.Text = txtAccIRR.Text = string.Empty;
            //                rfvBusineesIRR.Enabled =
            //                btnCalIRR.Visible =
            //                    //CEAccountCreationDate.Enabled =
            //                    //ImgAccountCreationDate.Visible = 
            //                true;
            //                txtAccountCreationDate.Text = Convert.ToDateTime(dtSet.Tables[0].Rows[0]["Payment_Date"].ToString()).ToString(strDateFormat);

            //            }
            //    }
            //}


            //}
            //if (dtSet.Tables[1].Rows.Count > 0)
            //{
            //    ViewState["AccountType"] = dtSet.Tables[1].Rows[0]["accounttype"].ToString();
            //}
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
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Type", "Type5");
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            //Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            //Procparam.Add("@Is_Active", "1");
            //Procparam.Add("@Param1", "2");
            //Procparam.Add("@Param2", strPAN);
            //Procparam.Add("@UserID", intUserID.ToString());
            //ddlSLA.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_List, Procparam, new string[] { "SANum", "SANum" });
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
            //if (ddlMLA.SelectedValue != "0")
            //{
            //    DataTable dtTable = new DataTable();
            //    if (Procparam != null)
            //        Procparam.Clear();
            //    else
            //        Procparam = new Dictionary<string, string>();
            //    Procparam.Add("@PANum", strPAN);
            //    dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetPANumCustomer, Procparam);

            //    S3GCustomerAddress1.SetCustomerDetails(dtTable.Rows[0], true);

            //    if (ddlSLA.Items.Count <= 1)
            //    {
            //        FunProPostPreDocument(Convert.ToBoolean(ViewState["Invoice"]));
            //    }
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Customer Details");
        }
    }
    protected void FunProPopulateCheckInterimCustomer()
    {
        try
        {
            DataTable dtTable = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Customer_ID", ddlLessee.SelectedValue);
            dtTable = Utility.GetDefaultData("S3G_LOANAD_GetMRARent_Cust", Procparam);
            if (dtTable.Rows.Count > 0)
            {
                if (dtTable.Rows[0]["appl"].ToString() == "1")
                {
                    rfvInterimMethod.Enabled = true;
                    rfvInterimMethod.Visible = true;
                    ddlInterimMethod.Enabled = true;
                }
                else
                {
                    rfvInterimMethod.Enabled = false;
                    rfvInterimMethod.Visible = false;
                    ddlInterimMethod.SelectedIndex = 0;
                    ddlInterimMethod.Enabled = false;
                    Utility.FunShowAlertMsg(this.Page, "No interim eligible as per MRA");

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
            //Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            //Procparam.Add("@PANUM", ddlMLA.SelectedValue.ToString());
            //if (ddlSLA.Items.Count > 0 && ddlSLA.SelectedValue != "0")
            //{
            //    Procparam.Add("@SANUM", ddlSLA.SelectedValue.ToString());
            //}
            ////Procparam.Add("@Customer_ID", txtCustCode.Attributes["Cust_ID"]);

            //dsDocuments = Utility.GetDataset("S3G_LOANAD_GetPrePostDisbursementDocs", Procparam);
            //dtProforma = dsDocuments.Tables[0];
            //dtPreDisb = dsDocuments.Tables[1];
            //dtInvoice = dsDocuments.Tables[2];
            //dtPostDisb = dsDocuments.Tables[3];

            //dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetProformaImage, Procparam);

            //if (dtProforma.Rows.Count > 0 || dtPreDisb.Rows.Count > 0)
            //{
            //    if (dtProforma.Rows.Count == 0)
            //    {
            //        pnlProforma.Visible = false;
            //    }
            //    else
            //    {
            //        pnlProforma.Visible = true;
            //    }

            //    if (dtPreDisb.Rows.Count == 0)
            //    {
            //        pnlPreDisb.Visible = false;
            //    }
            //    else
            //    {
            //        pnlPreDisb.Visible = true;
            //    }

            //    grvFile.DataSource = dtProforma;
            //    grvFile.DataBind();

            //    gvPRDDT.DataSource = dtPreDisb;
            //    gvPRDDT.DataBind();

            //    trPreMessage.Visible = false;
            //    ViewState["EnableTab3"] = "true";
            //}
            //else
            //{
            //    ViewState["EnableTab3"] = "false";
            //    trPreMessage.Visible = true;
            //    if (PageMode != PageModes.Create)
            //    {
            //        tcAccountActivation.Tabs[3].Enabled = false;
            //    }
            //}

            //if (dtInvoice.Rows.Count > 0 || dtPostDisb.Rows.Count > 0)
            //{
            //    if (dtInvoice.Rows.Count == 0)
            //    {
            //        pnlInvoice.Visible = false;
            //    }
            //    else
            //    {
            //        pnlInvoice.Visible = true;
            //    }

            //    if (dtPostDisb.Rows.Count == 0)
            //    {
            //        pnlPostDisb.Visible = false;
            //    }
            //    else
            //    {
            //        pnlPostDisb.Visible = true;
            //    }

            //    grvInvoice.DataSource = dtInvoice;
            //    grvInvoice.DataBind();

            //    gvPDDT.DataSource = dtPostDisb;
            //    gvPDDT.DataBind();

            //    trPostMessage.Visible = false;
            //    ViewState["EnableTab4"] = "true";
            //}

            //else
            //{
            //    ViewState["EnableTab4"] = "false";
            //    trPostMessage.Visible = true;
            //    if (PageMode != PageModes.Create)
            //    {
            //        tcAccountActivation.Tabs[4].Enabled = false;
            //    }
            //}

            //for (int i = 1; i <= 4; i++)
            //{
            //    if (ViewState["EnableTab" + i.ToString()] == null || string.IsNullOrEmpty(ViewState["EnableTab" + i.ToString()].ToString()))
            //    {
            //        ViewState["EnableTab" + i.ToString()] = "false";
            //    }
            //}

            //string strParam = Convert.ToString(ViewState["EnableTab1"]) + ",";
            //strParam += Convert.ToString(ViewState["EnableTab2"]) + ",";
            //strParam += Convert.ToString(ViewState["EnableTab3"]) + ",";
            //strParam += Convert.ToString(ViewState["EnableTab4"]);

            //ScriptManager.RegisterStartupScript(this, GetType(), "te", "finDisableTab(" + strParam + ");", true);

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
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            DataSet Dst = Utility.GetDataset("S3G_LOANAD_GetBulkActivationforModify", Procparam);
            FunProPopulateinterimList();
            dtTable = Dst.Tables[0];
            ViewState["DSSetTab1"] = (DataTable)dtTable;
            ddlLessee.SelectedValue = dtTable.Rows[0]["Customer_ID"].ToString();
            ddlLessee.SelectedText = dtTable.Rows[0]["Customer_Name"].ToString();
            ddlLessee.Enabled = false;
            ddlInterimMethod.SelectedValue = dtTable.Rows[0]["interim_type"].ToString();
            ddlInterimMethod.ClearDropDownList();
            txtInterimRate.Text = dtTable.Rows[0]["interim_rate"].ToString();
            txtInterFrom.Text = dtTable.Rows[0]["Interim_From"].ToString();
            txtInterTo.Text = dtTable.Rows[0]["Interim_To"].ToString();
            //txtInterimRent.Text = dtTable.Rows[0]["interim_rent"].ToString();
            //txtStatus.Text = dtTable.Rows[0]["Status"].ToString();
            txtActivationDate.Text = DateTime.Parse(dtTable.Rows[0]["Account_Activated_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            ViewState["Activated_By"] = dtTable.Rows[0]["Created_By"].ToString();
            grvRSDet.DataSource = dtTable;
            grvRSDet.DataBind();
            ViewState["dtRSDet"] = dtTable;
            lblTotAmf.Text = dtTable.Compute("Sum(Interim_AMF)", "").ToString();
            lbltotinterim.Text = dtTable.Compute("Sum(Interim_Rent)", "").ToString();
            for (int i = 0; i < grvRSDet.Rows.Count; i++)
            {

                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = true;

                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = true;
            }
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem(dtTable.Rows[0]["Tranche_Name"].ToString(), dtTable.Rows[0]["Tranche_Id"].ToString());
            ddlTranche.Items.Add(liSelect);

            ViewState["JVDet"] = (DataTable)Dst.Tables[1];

            lblInterimRent.CssClass = (ddlInterimMethod.SelectedValue == "11") ? "styleReqFieldLabel" : "styleDisplayLabel";
            lblInterimRate.CssClass = (ddlInterimMethod.SelectedValue == "1" || ddlInterimMethod.SelectedValue == "2") ? "styleReqFieldLabel" : "styleDisplayLabel";
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

            //foreach (GridViewRow grvFRow in grvFile.Rows)
            //{
            //    Label lblID = (Label)grvFRow.FindControl("lblID");
            //    strImage.Append("<Details Invoice_ID='" + lblID.Text + "' /> ");
            //}

            //strImage.Append("</Root>");

            //strProforma.Append("<Root>");

            //foreach (GridViewRow grvFRow in grvFile.Rows)
            //{
            //    Label lblPID = (Label)grvFRow.FindControl("lblID");
            //    strImage.Append("<Details Proforma_ID='" + lblPID.Text + "' /> ");
            //}
            //strProforma.Append("</Root>");
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
                //if (grvAmortization.Rows.Count <= 0)
                //{
                //    return false;
                //}
                //else
                //{
                return true;
                //}
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
            GridView grvAmortization = new GridView();
            grvAmortization.HeaderStyle.CssClass = "styleGridHeader";


            string strPANum = "";
            foreach (GridViewRow grvRow in grvRSDet.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelect")).Checked)
                {
                    strPANum += ((Label)grvRow.FindControl("lblRSNo")).Text + ",";
                }
            }
            //Added on 18Jun2015 for Round 1 Bug fixing Starts here
            if (Convert.ToString(strPANum) == "")
            {
                Utility.FunShowAlertMsg(this, "Select atleast one Rental Schedule");
                return;
            }
            //Added on 18Jun2015 for Round 1 Bug fixing Ends here
            DataTable dtAmort = new DataTable();
            DataTable dtAmortization = new DataTable();

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", (Convert.ToString(strMode) != "C") ? "1" : "2");
            Procparam.Add("@PANum", strPANum);
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID.ToString()));
            Procparam.Add("@User_ID", Convert.ToString(intUserID.ToString())); //Added by Tamilselvan.s on 22/10/2011
            Procparam.Add("@Activation_Date", Convert.ToString(Utility.StringToDate(txtActivationDate.Text)));
            dtAmort = Utility.GetDefaultData("S3G_LOANAD_GetBulkRSForCalculation", Procparam);

            if (dtAmort.Rows.Count > 0 && dtAmort.Columns.Count > 1)
            {
                grvAmortization.DataSource = dtAmort;
                grvAmortization.DataBind();
                string strMethod = "";
                //Changed By Thanagm M on 30/Dec/2011 to Check the calc mothod for modify mode
                //Changed By Narasimha Rao on 11/Jan/2012 to Check the condition for Create mode and WorkFlow Mode also.
            }
            else if (dtAmort.Rows.Count == 0 && (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow) && ViewState["MLAStatus"].ToString() == "1")
            {
                throw new ApplicationException("Global Parameter Setup is not defined for Amortization method.");
            }

            if (grvAmortization != null)
            {
                if (grvAmortization.Rows.Count == 0)
                {

                }
                else
                {
                    FunProExport(grvAmortization, "Amortization");
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to load Amortization Schedule");
        }
    }

    protected void FunProGetAmortizationTranche()
    {
        try
        {
            GridView grvAmortization = new GridView();
            grvAmortization.HeaderStyle.CssClass = "styleGridHeader";


            string strPANum = "";
            foreach (GridViewRow grvRow in grvRSDet.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelect")).Checked)
                {
                    strPANum += ((Label)grvRow.FindControl("lblRSNo")).Text + ",";
                }
            }
            //Added on 18Jun2015 for Round 1 Bug fixing Starts here
            if (Convert.ToString(strPANum) == "")
            {
                Utility.FunShowAlertMsg(this, "Select atleast one Rental Schedule");
                return;
            }
            //Added on 18Jun2015 for Round 1 Bug fixing Ends here
            DataTable dtAmort = new DataTable();
            DataTable dtAmortization = new DataTable();

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", (Convert.ToString(strMode) != "C") ? "3" : "2");
            Procparam.Add("@PANum", strPANum);
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID.ToString()));
            Procparam.Add("@User_ID", Convert.ToString(intUserID.ToString())); //Added by Tamilselvan.s on 22/10/2011
            Procparam.Add("@Activation_Date", Convert.ToString(Utility.StringToDate(txtActivationDate.Text)));
            dtAmort = Utility.GetDefaultData("S3G_LOANAD_GetBulkRSForCalculation", Procparam);

            if (dtAmort.Rows.Count > 0 && dtAmort.Columns.Count > 1)
            {
                grvAmortization.DataSource = dtAmort;
                grvAmortization.DataBind();
                string strMethod = "";
                //Changed By Thanagm M on 30/Dec/2011 to Check the calc mothod for modify mode
                //Changed By Narasimha Rao on 11/Jan/2012 to Check the condition for Create mode and WorkFlow Mode also.
            }
            else if (dtAmort.Rows.Count == 0 && (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow) && ViewState["MLAStatus"].ToString() == "1")
            {
                throw new ApplicationException("Global Parameter Setup is not defined for Amortization method.");
            }

            if (grvAmortization != null)
            {
                if (grvAmortization.Rows.Count == 0)
                {

                }
                else
                {
                    FunProExport(grvAmortization, "AmortizationTranche");
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


            //grvAmortization.Columns[1].Visible = CanShow;
            //grvAmortization.Columns[3].Visible = CanShow;

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
            //ddlSLA.Enabled = CanEnable;
            //if (CanEnable)
            //{
            //    lblSLA.CssClass = "styleReqFieldLabel";
            //}
            //else
            //{
            //    lblSLA.CssClass = "";
            //}
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
            ddlLessee.Clear();
            ddlInterimMethod.SelectedIndex = 0;
            if (ddlTranche.Items.Count > 0)
                ddlTranche.Items.Clear();
            txtInterimRate.Text = txtInterimRent.Text = txtInterAMF.Text = txtInterFrom.Text = txtInterTo.Text = "";// txtStatus.Text = "";
            lbltotinterim.Text = lblTotAmf.Text = "0";
            btnSave.Enabled = true;
            txtActivationDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
            grvRSDet.DataSource = null;
            grvRSDet.DataBind();
            btnExportAmort.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Clear Main tab controls");
        }

    }

    // Added By Shibu 24-Sep-2013 Branch List 
    //[System.Web.Services.WebMethod]
    //public static string[] GetBranchList(String prefixText, int count)
    //{
    //    Dictionary<string, string> Procparam;
    //    Procparam = new Dictionary<string, string>();
    //    List<String> suggestions = new List<String>();
    //    DataTable dtCommon = new DataTable();
    //    DataSet Ds = new DataSet();

    //    Procparam.Clear();

    //    Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
    //    Procparam.Add("@Type", "GEN");
    //    Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
    //    Procparam.Add("@Program_Id", obj_Page.ProgramCode);
    //    Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
    //    Procparam.Add("@Is_Active", "1");
    //    Procparam.Add("@PrefixText", prefixText);
    //    suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

    //    return suggestions.ToArray();
    //}


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

    [System.Web.Services.WebMethod]
    public static string[] GetCustomer(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();

        Procparam.Add("@Option", "1");
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        //suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3g_LoanAd_GetCustomer_AGT", Procparam));
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));    //Commented and Added on 18Jun2015

        return suggestions.ToArray();
    }

    protected void GenerateRepaymentSchedule()
    {
        //try
        //{

        //    //Getting Account details for repayment structure and IRR calculation
        //    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
        //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
        //    Procparam.Add("@CompanyId", intCompanyID.ToString());
        //    Procparam.Add("@PANum", ddlMLA.SelectedValue);
        //    if (ddlSLA.SelectedIndex > 0)
        //        Procparam.Add("@SANum", ddlSLA.SelectedValue);
        //    //cashOutFLow
        //    DataTable dtOutflow = null; DataTable dtInflow = null;
        //    //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
        //    DataTable DtRepayGrid = null; DataTable dtRepayDetailsOthers = null;
        //    //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
        //    DataSet DS = Utility.GetDataset("S3G_LAD_GET_ACCTDTLS", Procparam);

        //    string strTenure, strTenureType, strLob, strLobid, strRoundoff, strhdnPLR, strhdnCTR, strReturn_Pattern, strMarginMoneyPer_Cashflow
        //        , strRate, strTime_Value, strRepayment_Mode, strFrequency, strRecovery_Year1, strRecovery_Year2, strRecovery_Year3
        //        , strRecovery_YearRest, strIRR_Rest, strResidualValue_Cashflow, strResidualAmt_Cashflow, strtxtFBDate;
        //    decimal decFinAmount = 0;
        //    DateTime dtStartDate = Utility.StringToDate(txtAccountCreationDate.Text);

        //    strTenure = strTenureType = strLob = strLobid = strRoundoff = strhdnPLR = strhdnCTR = strReturn_Pattern = strMarginMoneyPer_Cashflow
        //               = strRate = strTime_Value = strRepayment_Mode = strFrequency = strRecovery_Year1 = strRecovery_Year2 = strRecovery_Year3
        //                    = strRecovery_YearRest = strIRR_Rest = strResidualValue_Cashflow = strResidualAmt_Cashflow = strtxtFBDate = string.Empty;


        //    if (DS.Tables[0].Rows.Count > 0)//Header Part
        //    {
        //        DataTable dthdr = DS.Tables[0].Copy();
        //        strTenure = dthdr.Rows[0]["Tenure"].ToString();
        //        strTenureType = dthdr.Rows[0]["TenureType"].ToString();
        //        strMarginMoneyPer_Cashflow = dthdr.Rows[0]["Offer_Margin"].ToString();
        //        if (!string.IsNullOrEmpty(dthdr.Rows[0]["Loan_Amount"].ToString()))
        //            decFinAmount = Convert.ToDecimal(dthdr.Rows[0]["Loan_Amount"].ToString());
        //        strResidualAmt_Cashflow = dthdr.Rows[0]["Offer_Residual_Value_Amount"].ToString();
        //        strResidualValue_Cashflow = dthdr.Rows[0]["Offer_Residual_Value"].ToString();
        //        strtxtFBDate = dthdr.Rows[0]["FB_Date"].ToString();
        //    }

        //    if (DS.Tables[1].Rows.Count > 0)//ROI Part
        //    {
        //        DataTable dtROI = DS.Tables[1].Copy();
        //        strRate = dtROI.Rows[0]["rate"].ToString();
        //        strReturn_Pattern = dtROI.Rows[0]["return_pattern"].ToString();
        //        strTime_Value = dtROI.Rows[0]["time_value"].ToString();
        //        strRepayment_Mode = dtROI.Rows[0]["Repayment_Mode"].ToString();
        //        strFrequency = dtROI.Rows[0]["frequency"].ToString();
        //        strRecovery_Year1 = dtROI.Rows[0]["recovery_pattern_year1"].ToString();
        //        strRecovery_Year2 = dtROI.Rows[0]["recovery_pattern_year2"].ToString();
        //        strRecovery_Year3 = dtROI.Rows[0]["recovery_pattern_year3"].ToString();
        //        strRecovery_YearRest = dtROI.Rows[0]["recovery_pattern_rest"].ToString();
        //        strIRR_Rest = dtROI.Rows[0]["irr_rest"].ToString();
        //    }
        //    if (DS.Tables[2].Rows.Count > 0)//cashInFLow
        //    {
        //        dtInflow = DS.Tables[2].Clone();
        //    }

        //    if (DS.Tables[3].Rows.Count > 0)//cashOutFLow
        //    {
        //        dtOutflow = DS.Tables[3].Copy();

        //        //Handling Last Payment date as outflow date start
        //        DataRow drOutFlw = dtOutflow.Select("CashFlow_Flag_ID=41").Last();
        //        drOutFlw.BeginEdit();
        //        drOutFlw["Date"] = dtStartDate;
        //        drOutFlw.EndEdit();

        //        //Handling Last Payment date as outflow date end

        //    }

        //    //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
        //    if (DS.Tables[4].Rows.Count > 0)//Repayment
        //    {
        //        DtRepayGrid = DS.Tables[4].Copy();
        //        ViewState["DtRepayGrid"] = DtRepayGrid;
        //    }
        //    //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end


        //    ViewState["dtOutflow"] = dtOutflow;
        //    DataSet dsRepayGrid = new DataSet();
        //    DataTable dtMoratorium = null;
        //    Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
        //    //Header Start
        //    objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text);
        //    objMethodParameters.Add("LobId", ddlLOB.SelectedValue);
        //    objMethodParameters.Add("CompanyId", intCompanyID.ToString());
        //    //objMethodParameters.Add("DocumentDate", (txtActivationDate.Text == "") ? txtApplicationDate.Text : txtAccountDate.Text);//Activation Date
        //    objMethodParameters.Add("DocumentDate", txtAccountCreationDate.Text);//Activation Date
        //    objMethodParameters.Add("Tenure", strTenure);//tble0//Tenure
        //    objMethodParameters.Add("TenureType", strTenureType);//tble0//TenureType
        //    objMethodParameters.Add("MarginPercentage", strMarginMoneyPer_Cashflow);//tble0//Offer_Margin
        //    objMethodParameters.Add("FinanceAmount", decFinAmount.ToString());//tble0//Loan_Amount

        //    //For Round OFF and PLR,CTR
        //    DataTable dtIRRDetails = Utility.FunPubGetGlobalIRRDetails(intCompanyID, null);
        //    ViewState["IRRDetails"] = dtIRRDetails;
        //    dtIRRDetails.DefaultView.RowFilter = "LOB_ID = " + ddlLOB.SelectedValue;
        //    dtIRRDetails = dtIRRDetails.DefaultView.ToTable();
        //    if (dtIRRDetails.Rows.Count > 0)
        //    {
        //        strhdnCTR = dtIRRDetails.Rows[0]["Corporate_Tax_Rate"].ToString();
        //        strhdnPLR = dtIRRDetails.Rows[0]["Prime_Lending_Rate"].ToString();
        //        strRoundoff = dtIRRDetails.Rows[0]["Roundoff"].ToString();
        //    }
        //    if (!string.IsNullOrEmpty(strRoundoff))
        //    {
        //        objMethodParameters.Add("Roundoff", strRoundoff);
        //    }
        //    else
        //    {
        //        objMethodParameters.Add("Roundoff", "2");
        //    }

        //    //ROI Start
        //    objMethodParameters.Add("ReturnPattern", strReturn_Pattern);//tble1//return_pattern

        //    objMethodParameters.Add("Rate", strRate);//tble1//rate
        //    objMethodParameters.Add("TimeValue", strTime_Value);//tble1//time_value
        //    objMethodParameters.Add("RepaymentMode", strRepayment_Mode);//tble1//Repayment_Mode



        //    objMethodParameters.Add("Frequency", strFrequency);//tble1//frequency
        //    objMethodParameters.Add("RecoveryYear1", strRecovery_Year1);//tble1//recovery_pattern_year1
        //    objMethodParameters.Add("RecoveryYear2", strRecovery_Year2);//tble1//recovery_pattern_year2
        //    objMethodParameters.Add("RecoveryYear3", strRecovery_Year3);//tble1//recovery_pattern_year3
        //    objMethodParameters.Add("RecoveryYear4", strRecovery_YearRest);//tble1//recovery_pattern_rest
        //    if (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE") && strRepayment_Mode != "5" && strReturn_Pattern == "6")
        //    {
        //        objMethodParameters.Add("PrincipalMethod", "1");
        //    }
        //    else
        //    {
        //        objMethodParameters.Add("PrincipalMethod", "0");
        //    }

        //    if (!string.IsNullOrEmpty(strtxtFBDate))
        //    {
        //        if (Convert.ToInt32(strtxtFBDate) > 0)
        //        {
        //            //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation start
        //            //DateTime dtDocDate = Utility.StringToDate(txtDate.Text);
        //            DateTime dtDocDate = dtStartDate;
        //            //Code commented and added by saran on 18-Jul-2012 for UAT fixing raised in account creation end

        //            DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
        //            dtformat.ShortDatePattern = "MM/dd/yy";
        //            string strFBDate = "";
        //            try
        //            {
        //                strFBDate = DateTime.Parse(dtDocDate.Month + "/" + strtxtFBDate + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
        //            }
        //            catch (Exception ex)
        //            {
        //                DateTime lastDayOfCurrentMonth = new DateTime(dtDocDate.Year, dtDocDate.Month,
        //                                                    DateTime.DaysInMonth(dtDocDate.Year, dtDocDate.Month));
        //                strFBDate = lastDayOfCurrentMonth.ToString(strDateFormat);
        //            }
        //            //string strFBDate = DateTime.Parse(dtDocDate.Month + "/" + txtFBDate.Text + "/" + dtDocDate.Year, dtformat).ToString(strDateFormat);
        //            dtStartDate = Utility.StringToDate(strFBDate);
        //        }
        //    }

        //    if (strReturn_Pattern == "2")
        //    {
        //        if (strResidualAmt_Cashflow.Trim() != "" && strResidualAmt_Cashflow.Trim() != "0")
        //        {
        //            if (Convert.ToDecimal(strResidualAmt_Cashflow) > 0)
        //                objMethodParameters.Add("decResidualAmount", strResidualAmt_Cashflow);//tble0//Offer_Residual_Value_Amount
        //        }
        //        if (strResidualValue_Cashflow.Trim() != "" && strResidualValue_Cashflow.Trim() != "0")
        //        {
        //            if (Convert.ToDecimal(strResidualValue_Cashflow) > 0)
        //                objMethodParameters.Add("decResidualValue", strResidualValue_Cashflow);//tble0//Offer_Residual_Value
        //        }
        //        switch (strIRR_Rest)//tble1//irr_rest
        //        {
        //            case "1":
        //                objMethodParameters.Add("strIRRrest", "daily");
        //                break;
        //            case "2":
        //                objMethodParameters.Add("strIRRrest", "monthly");
        //                break;
        //            default:
        //                objMethodParameters.Add("strIRRrest", "daily");
        //                break;

        //        }

        //        objMethodParameters.Add("decLimit", "0.10");






        //        decimal decRateOut = 0;
        //        //if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
        //        //{
        //        //    dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, dtInflow, dtOutflow, objMethodParameters, dtMoratorium, out decRateOut);
        //        //}
        //        //else
        //        //{
        //        dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, dtInflow, dtOutflow, objMethodParameters, dtMoratorium, out decRateOut);
        //        //}
        //        ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);
        //    }
        //    else
        //    {
        //        dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(dtStartDate, objMethodParameters, dtMoratorium);
        //    }

        //    //decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text);
        //    if (dsRepayGrid == null)
        //    {
        //        // It Calculates and displays the Repayment Details for ST-ADHOC //
        //        /*  grvRepayStructure.DataSource = null;
        //          grvRepayStructure.DataBind();
        //          FunPriShowRepaymetDetails(decFinAmount + FunPriGetStructureAdhocInterestAmount());
        //          gvRepaymentDetails.FooterRow.Visible = true;
        //          btnReset.Enabled = true;*/
        //        return;
        //    }
        //    if (dsRepayGrid.Tables[0].Rows.Count > 0)
        //    {
        //        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
        //        DtRepayGrid = dsRepayGrid.Tables[0];

        //        //gvRepaymentDetails.DataSource = dsRepayGrid.Tables[0];
        //        //gvRepaymentDetails.DataBind();
        //        DataTable DtRepayGridtemp = ((DataTable)ViewState["DtRepayGrid"]).Copy();
        //        DataRow[] drRepayGridtemp = DtRepayGridtemp.Select("CashFlow_Flag_ID = 23");
        //        if (drRepayGridtemp.Length > 0)
        //        {
        //            foreach (DataRow drRepayGridte in drRepayGridtemp)
        //                drRepayGridte.Delete();
        //            DtRepayGridtemp.AcceptChanges();
        //        }
        //        //DtRepayGrid.Merge(DtRepayGridtemp);

        //        foreach (DataRow drr in DtRepayGridtemp.Rows)
        //        {
        //            DataRow DtRepayGridRw = DtRepayGrid.NewRow();
        //            foreach (DataColumn dc in DtRepayGrid.Columns)
        //            {
        //                if (DtRepayGrid.Columns.Contains(dc.ColumnName) && DtRepayGridtemp.Columns.Contains(dc.ColumnName))
        //                {
        //                    if (dc.ColumnName == "Amount")
        //                        DtRepayGridRw[dc.ColumnName] = Convert.ToDecimal(drr[dc.ColumnName].ToString());
        //                    else if (dc.ColumnName == "CashFlow_Flag_ID")
        //                        DtRepayGridRw[dc.ColumnName] = Convert.ToInt16(drr[dc.ColumnName].ToString());
        //                    else
        //                        DtRepayGridRw[dc.ColumnName] = drr[dc.ColumnName].ToString();
        //                }
        //            }
        //            DtRepayGrid.Rows.Add(DtRepayGridRw);
        //            DtRepayGrid.AcceptChanges();
        //        }



        //        ViewState["DtRepayGrid"] = DtRepayGrid;

        //        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end

        //        //if (ddl_Rate_Type.SelectedItem.Text == "Floating" && string.IsNullOrEmpty(txtFBDate.Text))
        //        //{
        //        //    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
        //        //    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
        //        //}
        //        //else
        //        //{
        //        //    ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
        //        //    ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
        //        //}
        //        //btnReset.Enabled = false;
        //        //FunPriCalculateSummary(dsRepayGrid.Tables[0], "CashFlow", "TotalPeriodInstall");
        //        //decimal decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
        //        decimal decBreakPercent;// = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
        //        if (!((ddlLOB.SelectedItem.Text.ToUpper().Contains("TL")) || (ddlLOB.SelectedItem.Text.ToUpper().Contains("TE"))))
        //        {
        //            decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
        //        }
        //        else
        //        {
        //            DataRow[] dr = (((DataTable)ViewState["DtRepayGrid"])).Select("CashFlow_Flag_ID IN(35,91)");
        //            if (dr.Length == 0)
        //            {
        //                decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
        //            }
        //            else
        //            {
        //                decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID in(91,35)"));
        //            }


        //        }
        //        if (decBreakPercent != 0)
        //        {
        //            if (decBreakPercent != 100)
        //            {
        //                Utility.FunShowAlertMsg(this, "Total break up percentage should be equal to 100%");
        //                return;
        //            }
        //        }
        //        double douAccountingIRR = 0;
        //        double douBusinessIRR = 0;
        //        double douCompanyIRR = 0;
        //        DataTable dtRepaymentStructure = new DataTable();
        //        //if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
        //        //{
        //        //    DataTable dsAssetDetails = (DataTable)Session["ApplicationAssetDetails"];
        //        //    decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));

        //        //    objRepaymentStructure.FunPubCalculateIRR(txtActivationDate.Text, strhdnPLR,strFrequency, strDateFormat, strResidualAmt_Cashflow, strResidualValue_Cashflow, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
        //        //        , out dtRepaymentStructure, (DataTable)ViewState["DtRepayGrid"],dtInflow, dtOutflow
        //        //        , decFinAmount.ToString(), strMarginMoneyPer_Cashflow, strReturn_Pattern
        //        //        , strRate, strTenureType, strTenure, strIRR_Rest,
        //        //        strTime_Value, ddlLOB.SelectedItem.Text, strRepayment_Mode, "", dtMoratorium);
        //        //}
        //        //else
        //        //{
        //        objRepaymentStructure.FunPubCalculateIRR(txtAccountCreationDate.Text, strhdnPLR, strFrequency, strDateFormat, strResidualAmt_Cashflow, strResidualValue_Cashflow, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
        //         , out dtRepaymentStructure
        //            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
        //            , out dtRepayDetailsOthers
        //            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
        //         , (DataTable)ViewState["DtRepayGrid"], dtInflow, dtOutflow
        //         , decFinAmount.ToString(), strMarginMoneyPer_Cashflow, strReturn_Pattern
        //         , strRate, strTenureType, strTenure, strIRR_Rest,
        //         strTime_Value, ddlLOB.SelectedItem.Text, strRepayment_Mode, "", dtMoratorium);




        //        //}
        //        dtRepaymentStructure.Columns["Charge"].ColumnName = "FinanceCharges";
        //        ViewState["RepaymentStructure"] = dtRepaymentStructure;
        //        //grvRepayStructure.DataSource = dtRepaymentStructure;
        //        //grvRepayStructure.DataBind();


        //        //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
        //        if (dtRepayDetailsOthers != null)
        //            ViewState["dtRepayDetailsOthers"] = dtRepayDetailsOthers;
        //        //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 end

        //        //txtAccountIRR_Repay.Text = douAccountingIRR.ToString("0.0000");
        //        txtAccIRR.Text = douAccountingIRR.ToString("0.0000");

        //        //txtBusinessIRR_Repay.Text = douBusinessIRR.ToString("0.0000");
        //        txtBusinessIRR.Text = douBusinessIRR.ToString("0.0000");

        //        //txtCompanyIRR_Repay.Text = douCompanyIRR.ToString("0.0000");
        //        txtCompanyIRR.Text = douCompanyIRR.ToString("0.0000");

        //    }
        //    else
        //    {
        //        //gvRepaymentDetails.FooterRow.Visible = true;
        //        //btnReset.Enabled = true;
        //        //btnCalIRR.Enabled = true;
        //        ViewState["RepaymentStructure"] = null;
        //        txtBusinessIRR.Text = txtAccIRR.Text = txtCompanyIRR.Text = string.Empty;
        //        //grvRepayStructure.DataSource = null;
        //        //grvRepayStructure.DataBind();
        //    }
        //    //decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(txtFinanceAmount.Text, txtMarginMoneyPer_Cashflow.Text);
        //    //if (dsRepayGrid.Tables[0].Rows.Count > 0)
        //    //{
        //    //    FunPriShowRepaymetDetails((decimal)dsRepayGrid.Tables[0].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
        //    //}
        //    //else
        //    //{
        //    //    FunPriShowRepaymetDetails(decFinAmount + FunPriGetInterestAmount());
        //    //}

        //    //FunPriGenerateNewRepaymentRow();
        //    //FunPriUpdateROIRule();
        //    //if (ddl_Repayment_Mode.SelectedValue != "2")
        //    //{
        //    //    Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
        //    //    if (lblCashFlowId.Text != "23")
        //    //    {
        //    //        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
        //    //}

        //}
        //catch (Exception ex)
        //{
        //    ClsPubCommErrorLog.CustomErrorRoutine(ex, "Activation");
        //    throw ex;
        //}
    }

    private void FunPriSetPrefixSuffixLength()
    {
        try
        {
            txtInterimRate.SetDecimalPrefixSuffix(3, 2, false, false, "Interim Rate(%)");
            txtInterimRent.SetPercentagePrefixSuffix(13, 2, false, false, "Interim Amount");
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    #endregion

    private void InterimAppropriation()
    {
        var cnt = (from r in ((DataTable)ViewState["dtRSDet"]).AsEnumerable()
                   where r.Field<bool>("chk") == true
                   select r).Count();

        for (int i = 0; i < grvRSDet.Rows.Count; i++)
        {
            if (txtInterimRent.Text.Trim() != "")
                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = true;
            else
                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = false;

            if (txtInterAMF.Text.Trim() != "")
                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = true;
            else
                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = false;
        }

        if (cnt > 0)
        {

            DataRow[] drcopy1 = ((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "true" + " '");
            decimal dtavgtotalInterim = Convert.ToDecimal(((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "true" + " '").CopyToDataTable().Compute("Sum(Avg_Interim_rent)", ""));
            decimal dtavgtotalAMF = Convert.ToDecimal(((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "true" + " '").CopyToDataTable().Compute("Sum(Avg_AMF)", ""));
            foreach (DataRow dr in drcopy1)
            {
                if (txtInterimRent.Text.Trim() != "" && (hdnscript1.Value == "false" || hdnscript1.Value == ""))
                {
                    dr["Interim_amt"] = dtavgtotalInterim != 0 ?
                        Math.Round(Convert.ToDecimal(txtInterimRent.Text) *
                        Convert.ToDecimal(dr["Avg_Interim_rent"]) / dtavgtotalInterim) : 0;
                }
                else if (txtInterimRent.Text.Trim() == "" && (hdnscript1.Value == "false" || hdnscript1.Value == ""))
                {
                    dr["Interim_amt"] = 0;
                }

                if (txtInterAMF.Text.Trim() != "" && (hdnscript.Value == "false" || hdnscript.Value == ""))
                {
                    dr["Interim_AMF"] = dtavgtotalAMF != 0 ?
                        Math.Round(Convert.ToDecimal(txtInterAMF.Text) *
                        Convert.ToDecimal(dr["Avg_AMF"]) / dtavgtotalAMF) : 0;
                }
                else if (txtInterAMF.Text.Trim() == "" && (hdnscript.Value == "false" || hdnscript.Value == ""))
                {
                    dr["Interim_AMF"] = 0;
                }
            }

            if (txtInterimRent.Text != "" && Convert.ToDecimal(txtInterimRent.Text) > 0)
            {
                if (((DataTable)ViewState["dtRSDet"]).Select("chk = ' true '" + "and Interim_amt <> ' 0 '").Length > 0)
                {
                    dtinterimtotal = ((DataTable)ViewState["dtRSDet"]).Select("chk = ' true '" + "and Interim_amt <> ' 0 '").CopyToDataTable();
                }
                DataRow[] drinttotl = ((DataTable)ViewState["dtRSDet"]).Select("chk = ' true '" + "and Interim_amt <> ' 0 '");
                decimal dtcount = dtinterimtotal == null ? 0 : dtinterimtotal.Rows.Count;
                decimal totinterimcol = Convert.ToDecimal(dtinterimtotal.Compute("Sum(Interim_amt)", ""));

                decimal diff = 0;
                //if (totinterimcol > 0)
                diff = Convert.ToDecimal(txtInterimRent.Text) - totinterimcol;


                if (diff != 0)
                {
                    for (decimal i = Math.Abs((dtcount - Math.Abs(diff))) - 1; i < drinttotl.Length; i++)
                    {

                        if (diff > 0)
                        {
                            drinttotl[(int)i]["Interim_amt"] = Convert.ToDecimal(drinttotl[(int)i]["Interim_amt"]) + (dtcount < Math.Abs(diff) ? Math.Abs(diff) : dtcount > Math.Abs(diff) ? Math.Abs(diff) : 1);
                        }
                        else
                        {
                            drinttotl[(int)i]["Interim_amt"] = Convert.ToDecimal(drinttotl[(int)i]["Interim_amt"]) - (dtcount < Math.Abs(diff) ? Math.Abs(diff) : dtcount > Math.Abs(diff) ? Math.Abs(diff) : 1);
                        }

                        if (dtcount > Math.Abs(diff))
                            break;
                    }

                    //for (decimal i = Math.Abs((dtcount - Math.Abs(diff)))-1; i < dtinterimtotal.Rows.Count; i++)
                    //{
                    //    if (diff > 0)
                    //    {
                    //        dtinterimtotal.Rows[(int)i]["Interim_amt"] =
                    //            Convert.ToDecimal(dtinterimtotal.Rows[(int)i]["Interim_amt"]) + (dtcount < Math.Abs(diff) ? Math.Abs(diff) : 1);
                    //    }
                    //    else
                    //    {
                    //        dtinterimtotal.Rows[(int)i]["Interim_amt"] =
                    //            Convert.ToDecimal(dtinterimtotal.Rows[(int)i]["Interim_amt"]) - (dtcount < Math.Abs(diff) ? Math.Abs(diff) : 1);
                    //    }
                    //}
                }
            }

            if (txtInterAMF.Text != "" && Convert.ToDecimal(txtInterAMF.Text) > 0)
            {
                if (((DataTable)ViewState["dtRSDet"]).Select("chk = ' true '" + "and Interim_AMF <> ' 0 '").Length > 0)
                {
                    dtintAMF = ((DataTable)ViewState["dtRSDet"]).Select("chk = ' true '" + "and Interim_AMF <> ' 0 '").CopyToDataTable();
                }
                DataRow[] drinttotlamf = ((DataTable)ViewState["dtRSDet"]).Select("chk = ' true '" + "and Interim_AMF <> ' 0 '");
                decimal dtcountamf = dtintAMF == null ? 0 : dtintAMF.Rows.Count;
                decimal totinterimcolAMF = Convert.ToDecimal(dtintAMF.Compute("Sum(Interim_AMF)", ""));
                decimal diffAMF = Convert.ToDecimal(txtInterAMF.Text) - totinterimcolAMF;
                if (diffAMF != 0)
                {
                    for (decimal i = Math.Abs((dtcountamf - Math.Abs(diffAMF))) - 1; i <= drinttotlamf.Length; i++)
                    {
                        if (diffAMF > 0)
                        {
                            drinttotlamf[(int)i]["Interim_AMF"] = Convert.ToDecimal(drinttotlamf[(int)i]["Interim_AMF"]) + (dtcountamf < Math.Abs(diffAMF) ? Math.Abs(diffAMF) : dtcountamf > Math.Abs(diffAMF) ? Math.Abs(diffAMF) : 1);
                        }
                        else
                        {
                            drinttotlamf[(int)i]["Interim_AMF"] = Convert.ToDecimal(drinttotlamf[(int)i]["Interim_AMF"]) - (dtcountamf < Math.Abs(diffAMF) ? Math.Abs(diffAMF) : dtcountamf > Math.Abs(diffAMF) ? Math.Abs(diffAMF) : 1);
                        }
                        if (dtcountamf > Math.Abs(diffAMF))
                            break;
                    }
                }
            }






        }



        DataRow[] drcopy2 = ((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "false" + " '");
        if (drcopy2.Length > 0)
        {
            foreach (DataRow dr in drcopy2)
            {
                if (txtInterimRent.ReadOnly == false)
                    dr["Interim_amt"] = 0;
                if (txtInterAMF.ReadOnly == false)
                    dr["Interim_AMF"] = 0;
            }
        }


        if (txtInterAMF.Text.Trim() == "" && hdnscript.Value == "true")
        {

            for (int i = 0; i < grvRSDet.Rows.Count; i++)
            {
                ((DataTable)ViewState["dtRSDet"]).Rows[i]["Interim_AMF"] = ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).Text;

            }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>fneidtrent1(); </script>", false);  //,true --Footer Parameter 


        }

        if (txtInterimRent.Text.Trim() == "" && hdnscript1.Value == "true")
        {
            for (int i = 0; i < grvRSDet.Rows.Count; i++)
            {
                ((DataTable)ViewState["dtRSDet"]).Rows[i]["Interim_amt"] = ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).Text;

            }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>fneidtrent(); </script>", false);
        }

        ViewState["dtRSDet"] = ((DataTable)ViewState["dtRSDet"]);
        grvRSDet.DataSource = ((DataTable)ViewState["dtRSDet"]);
        grvRSDet.DataBind();
        ((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked = Convert.ToBoolean(ViewState["Header"]);









        //DataTable dttotalInerim = ((DataTable)ViewState["dtRSDet"]).Clone();
        //dttotalInerim.Columns["Interim_amt"].DataType = typeof(decimal);
        //foreach (DataRow drow in ((DataTable)ViewState["dtRSDet"]).Rows)
        //{
        //    dttotalInerim.ImportRow(drow);
        //}
        lblInterim_tot.Visible = true;
        lbltotinterim.Visible = true;
        if (dtinterimtotal != null)
            lbltotinterim.Text = ((DataTable)ViewState["dtRSDet"]).Compute("Sum(Interim_amt)", "").ToString();
        lblInterim_totAMF.Visible = true;
        lblTotAmf.Visible = true;
        if (dtintAMF != null)
            lblTotAmf.Text = dtintAMF.Compute("Sum(Interim_AMF)", "").ToString();


        hdnTotinterim.Value = ((DataTable)ViewState["dtRSDet"]).Compute("Sum(Interim_amt)", "").ToString();

        //DataTable dttotalAMF = ((DataTable)ViewState["dtRSDet"]).Clone();
        //dttotalAMF.Columns["Interim_AMF"].DataType = typeof(decimal);
        //foreach (DataRow drow in ((DataTable)ViewState["dtRSDet"]).Rows)
        //{
        //    dttotalAMF.ImportRow(drow);
        //}
        lblTotAmf.Visible = true;
        lblTotAmf.Text = ((DataTable)ViewState["dtRSDet"]).Compute("Sum(Interim_AMF)", "").ToString();
        lblInterim_totAMF.Visible = true;
        hdnTotAmf.Value = ((DataTable)ViewState["dtRSDet"]).Compute("Sum(Interim_AMF)", "").ToString();



    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox)
        {
            if (ddlInterimMethod.SelectedValue != "11" || strMode == "M")
            {
                return;
            }

            int rowindex = Utility.FunPubGetGridRowID("grvRSDet", ((CheckBox)sender).ClientID);
            if (((CheckBox)sender).ID.Equals("chkSelectAll"))
            {
                foreach (DataRow dr in ((DataTable)ViewState["dtRSDet"]).Rows)
                {
                    //if (txtInterimRent.ReadOnly != true && txtInterimRent.Text != string.Empty)
                    //{
                    //    dr["Interim_amt"] = Math.Round(Convert.ToDecimal(txtInterimRent.Text) / grvRSDet.Rows.Count).ToString();
                    //}

                    //else if (txtInterimRent.ReadOnly && ((TextBox)grvRSDet.HeaderRow.FindControl("lblInterimRentGrid")).Text != string.Empty)
                    //{
                    //    dr["Interim_amt"] = Math.Round(Convert.ToDecimal(((TextBox)grvRSDet.HeaderRow.FindControl("lblInterimRentGrid")).Text) 
                    //        / grvRSDet.Rows.Count).ToString();
                    //}

                    //if (txtInterAMF.ReadOnly != true && txtInterAMF.Text != string.Empty)
                    //{
                    //    dr["Interim_AMF"] = Math.Round(Convert.ToDecimal(txtInterAMF.Text) / grvRSDet.Rows.Count).ToString();
                    //}

                    //else if (txtInterAMF.ReadOnly && ((TextBox)grvRSDet.HeaderRow.FindControl("lblInterimAMFGrid")).Text != string.Empty)
                    //{
                    //    dr["Interim_AMF"] = Math.Round(Convert.ToDecimal(((TextBox)grvRSDet.HeaderRow.FindControl("lblInterimAMFGrid")).Text) 
                    //        / grvRSDet.Rows.Count).ToString();
                    //}



                    dr["chk"] = (!((CheckBox)sender).Checked) ? "false" : "true";

                    //dr["chkall"] = (!((CheckBox)sender).Checked) ? "false" : "true";
                }
                ViewState["Header"] = (!((CheckBox)sender).Checked) ? "false" : "true";

                ViewState["dtRSDet"] = (DataTable)ViewState["dtRSDet"];
                InterimAppropriation();

                //grvRSDet.DataSource = ((DataTable)ViewState["dtRSDet"]);
                //grvRSDet.DataBind();
                //((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked = Convert.ToBoolean(ViewState["Header"].ToString());

            }

            else
            {

                string no = ((Label)grvRSDet.Rows[rowindex].FindControl("lblRSNo")).Text;
                DataRow drcopy = ((DataTable)ViewState["dtRSDet"]).Select("PANum = '" + no + " '").FirstOrDefault();
                drcopy["chk"] = ((CheckBox)sender).Checked.ToString();

                var cnt = (from r in ((DataTable)ViewState["dtRSDet"]).AsEnumerable()
                           where r.Field<bool>("chk") == true
                           select r).Count();

                //DataRow[] drcopy1 = ((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "true" + " '");
                //if (drcopy1.Length > 0)
                //{
                //    for (int i = 0; i < drcopy1.Count(); i++)
                //    {
                //        drcopy1[i]["Interim_amt"] = txtInterimRent.Text != string.Empty ?
                //            Math.Round(Convert.ToDecimal(txtInterimRent.Text) / cnt).ToString() :
                //            ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).Text != string.Empty ?
                //            ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).Text : string.Empty;
                //        drcopy1[i]["Interim_AMF"] = txtInterAMF.Text != string.Empty ?
                //            Math.Round(Convert.ToDecimal(txtInterAMF.Text) / cnt).ToString() :
                //            ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).Text != string.Empty ?
                //            ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).Text : string.Empty;

                //    }
                //}

                //if (!(((CheckBox)sender).Checked))
                //{

                //    DataRow[] drcopy2 = ((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "false" + " '");
                //    if (drcopy2.Length > 0)
                //    {
                //        foreach (DataRow dr in drcopy2)
                //        {
                //            dr["Interim_amt"] = string.Empty;
                //            dr["Interim_AMF"] = string.Empty;
                //        }
                //    }
                //}

                if (!(((CheckBox)sender).Checked) && ((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked)
                {
                    ((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked = false;

                    ViewState["Header"] = Convert.ToString(((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked);


                }



                else
                {
                    if (cnt == ((DataTable)ViewState["dtRSDet"]).Rows.Count)
                        ((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked = true;
                    ViewState["Header"] = Convert.ToString(((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked);
                }
                ViewState["dtRSDet"] = ((DataTable)ViewState["dtRSDet"]);

                InterimAppropriation();

                //grvRSDet.DataSource = ((DataTable)ViewState["dtRSDet"]);
                //grvRSDet.DataBind();
            }

        }

        //else if (sender is TextBox)
        //{
        //    if (((TextBox)sender).ID.Equals("txtInterimRent"))
        //    {
        //        var cnt = (from r in ((DataTable)ViewState["dtRSDet"]).AsEnumerable()
        //                   where r.Field<bool>("chk") == true
        //                   select r).Count();
        //        if (!((TextBox)sender).Text.Equals(""))
        //        {
        //            for (int i = 0; i < grvRSDet.Rows.Count; i++)
        //            {
        //                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = true;
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < grvRSDet.Rows.Count; i++)
        //            {
        //                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimRentGrid")).ReadOnly = false;
        //            }
        //        }
        //        if (cnt > 0)
        //        {
        //            DataRow[] drcopy1 = ((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "true" + " '");

        //            foreach (DataRow dr in drcopy1)
        //            {
        //                if (!((TextBox)sender).Text.Equals(""))
        //                {
        //                    dr["Interim_amt"] = Math.Round(Convert.ToDecimal(txtInterimRent.Text) / cnt).ToString();

        //                }
        //                else
        //                {
        //                    dr["Interim_amt"] = string.Empty;
        //                    //dr["chk"] = false;


        //                }
        //            }

        //            ViewState["dtRSDet"] = ((DataTable)ViewState["dtRSDet"]);
        //            grvRSDet.DataSource = ((DataTable)ViewState["dtRSDet"]);
        //            grvRSDet.DataBind();
        //            ((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked = Convert.ToBoolean(ViewState["Header"]);

        //        }

        //    }

        //    else
        //    {
        //        var cnt = (from r in ((DataTable)ViewState["dtRSDet"]).AsEnumerable()
        //                   where r.Field<bool>("chk") == true
        //                   select r).Count();
        //        if (!((TextBox)sender).Text.Equals(""))
        //        {
        //            for (int i = 0; i < grvRSDet.Rows.Count; i++)
        //            {
        //                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = true;
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < grvRSDet.Rows.Count; i++)
        //            {
        //                ((TextBox)grvRSDet.Rows[i].FindControl("lblInterimAMFGrid")).ReadOnly = false;
        //            }
        //        }

        //        if (cnt > 0)
        //        {
        //            DataRow[] drcopy1 = ((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "true" + " '");

        //            foreach (DataRow dr in drcopy1)
        //            {
        //                if (!((TextBox)sender).Text.Equals(""))
        //                {
        //                    dr["Interim_AMF"] = Math.Round(Convert.ToDecimal(txtInterAMF.Text) / cnt).ToString();
        //                }
        //                else
        //                {
        //                    dr["Interim_AMF"] = string.Empty;
        //                    //dr["chk"] = false;

        //                }
        //            }
        //            ViewState["dtRSDet"] = ((DataTable)ViewState["dtRSDet"]);
        //            grvRSDet.DataSource = ((DataTable)ViewState["dtRSDet"]);
        //            grvRSDet.DataBind();
        //            ((CheckBox)grvRSDet.HeaderRow.FindControl("chkSelectAll")).Checked = Convert.ToBoolean(ViewState["Header"]);
        //        }
        //    }
        //}


    }

    protected void txtInterimRent_TextChanged(object sender, EventArgs e)
    {
        InterimAppropriation();
    }
    protected void txtInterAMF_TextChanged(object sender, EventArgs e)
    {
        InterimAppropriation();
    }
    protected void lblInterimRentGrid_TextChanged(object sender, EventArgs e)
    {


        if (((TextBox)sender).Text != "")
        {
            txtInterimRent.ReadOnly = true;
        }

        else
        {
            txtInterimRent.ReadOnly = false;
        }
    }
    protected void lblInterimAMFGrid_TextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            txtInterAMF.ReadOnly = true;
        }
        else
        {
            txtInterAMF.ReadOnly = false;
        }
    }

    private string funpubcreatexml(ref int flag)
    {
        //DataTable dtxml = ((DataTable)ViewState["dtRSDet"]).Select("chk = '" + "true" + " '").CopyToDataTable();
        StringBuilder strxml = new StringBuilder();
        strxml.Append("<Root>");

        for (int j = 0; j <= grvRSDet.Rows.Count - 1; j++)
        {
            if (((CheckBox)grvRSDet.Rows[j].FindControl("chkSelect")).Checked == true)
            {
                strxml.Append("<Details ");
                strxml.Append("RS_NO = '" + ((Label)grvRSDet.Rows[j].FindControl("lblRSNo")).Text + " '");
                if (((TextBox)grvRSDet.Rows[j].FindControl("lblInterimRentGrid")).Text != "")
                {
                    strxml.Append(" Interim_Rent = '" + ((TextBox)grvRSDet.Rows[j].FindControl("lblInterimRentGrid")).Text + " '");
                    flag = 1;
                }
                if (((TextBox)grvRSDet.Rows[j].FindControl("lblInterimAMFGrid")).Text != "")
                {
                    strxml.Append(" Interim_AMF = '" + ((TextBox)grvRSDet.Rows[j].FindControl("lblInterimAMFGrid")).Text + " '");
                    flag = 1;
                }
                if ((((TextBox)grvRSDet.Rows[j].FindControl("lblInterimRentGrid")).Text != "" || ((TextBox)grvRSDet.Rows[j].FindControl("lblInterimAMFGrid")).Text != "") && txtInterFrom.Text != "")
                {
                    strxml.Append(" Interim_From = '" + Utility.StringToDate(txtInterFrom.Text).ToString() + " '");
                }
                if ((((TextBox)grvRSDet.Rows[j].FindControl("lblInterimRentGrid")).Text != "" || ((TextBox)grvRSDet.Rows[j].FindControl("lblInterimAMFGrid")).Text != "") && txtInterTo.Text != "")
                {
                    strxml.Append(" Interim_To = '" + Utility.StringToDate(txtInterTo.Text).ToString() + " '");
                }

                strxml.Append(" Creation_date = '" + Utility.StringToDate(((Label)grvRSDet.Rows[j].FindControl("lblDate")).Text).ToString() + " '");

                strxml.Append("/>");
            }
        }

        strxml.Append("</Root>");
        return strxml.ToString();
    }

    private void funpricalculatetotal()
    {
        lblInterim_tot.Text = ((DataTable)ViewState["dtRSDet"]).Compute("Sum(Interim_amt)", "").ToString();
        lblInterim_totAMF.Text = ((DataTable)ViewState["dtRSDet"]).Compute("Sum(Interim_AMF)", "").ToString();
    }

}
