/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// /// <Program Summary>
/// Last Updated By		      :   Thalaiselvam N
/// Last Updated Date         :   03-Sep-2011
/// Reason                    :   Encrypted Password Validation
/// <Program Summary>
/// 
using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Collections;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
public partial class S3GLoanAdPaymentApproval_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    string strPaymentRequest_No = "";
    int intUserID = 0;
    int intRow = 0;
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    SerializationMode SerMode = SerializationMode.Binary;
    DataTable dtAction = new DataTable();
    DataSet dsApproval = new DataSet();
    DataSet Ds = new DataSet();
    string strDateFormat;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    ApprovalMgtServicesReference.ApprovalMgtServicesClient ObjApproval;
    S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable ObjApproval_DataTable = new S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable();
    public static S3GLoanAdPaymentApproval_Add obj_Page;
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        // WF Initializtion 
        ProgramCode = "056";
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        obj_Page = this;
        if (!IsPostBack)
        {
            btnRevoke.Enabled = false;
            chkUnapproval.Checked = true;
            PaymentReqID.Visible = false;
            PubFunBind();  //Bind LOB
            btnGo.Visible = true;
            if (Request.QueryString.Get("qsMode") != null)
            {
                if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                    if (!(string.IsNullOrEmpty(ticket.Name)))
                    {
                        strPaymentRequest_No = ticket.Name.ToString();
                        btnClear.Enabled = false;
                        btnGo.Enabled = false;
                    }
                }
            }
            //--------Landing Page(Ending)-----------------
            if (strPaymentRequest_No.Trim() != "")
            {
                FunPubViewApprovedDetails();
            }
            else
            {
                btnSave.Enabled = false;
                //btnClear.Enabled = false;
            }
        }

        // WORK FLOW IMPLEMENTATION
        if (PageMode == PageModes.WorkFlow)
        {
            try
            {
                PreparePageForWorkFlowLoad();
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
            }
        }
    }
    #region Workflow Methods
    /// <summary>
    /// Workflow Function
    /// </summary>
    private void PreparePageForWorkFlowLoad()
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            // Get The IDVALUE from Document Sequence #
            //DataTable HeaderValues = GetHeaderDetailsFromPANUMandSANUM(WFSessionValues.PANUM, ProgramCode, WFSessionValues.SANUM);            
            //WFSessionValues.WorkFlowDocumentNo = "2011-2012/pare/20";
            ddllLineOfBusiness.SelectedValue = WFSessionValues.LOBId.ToString();
            ddlBranch.SelectedValue = WFSessionValues.BranchID.ToString();
            LoadLOBDetails(false);
            ddlPaymentReqNumber.SelectedValue = WFSessionValues.WorkFlowDocumentNo;
            LoadPaymentRequestDetails();
            ClickGoButtonAction();
            //if(ddlBranch.Items.Count >0 ) ddlBranch.ClearDropDownList();
            if (ddllLineOfBusiness.Items.Count > 0) ddllLineOfBusiness.ClearDropDownList();
            if (Convert.ToInt32(ddlPaymentReqNumber.SelectedValue) > 0) ddlPaymentReqNumber.Clear();
            btnClear.Enabled = false;
        }
    }
    #endregion
    public void FunPubViewApprovedDetails()
    {
        btnGo.Enabled = false;
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Task_Type", "10");//Payment APPROVAL SCREEN LOOKUPCODE IS 12
        dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
        dictDropdownListParam.Add("@Task_Number", strPaymentRequest_No.ToString());
        dsApproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
        dictDropdownListParam = null;
        if (dsApproval.Tables[3].Rows.Count > 0)
        {
            txtStatus.Text = dsApproval.Tables[3].Rows[0]["Status"].ToString();
            txtPaymentReqDate.Text = DateTime.Parse(dsApproval.Tables[3].Rows[0]["Value_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            // txtpayto.Text = dsApproval.Tables[3].Rows[0]["Customer_Name"].ToString();

            txtamount.Text = Convert.ToDecimal(dsApproval.Tables[3].Rows[0]["Pay_Amount"]).ToString("0.00");
            txtPayMode.Text = dsApproval.Tables[3].Rows[0]["Pay_Mode_Code"].ToString();
            txtEntityCode.Text = dsApproval.Tables[3].Rows[0]["Entity_Code"].ToString();
            txtpayto.Text = dsApproval.Tables[3].Rows[0]["Lookup_Description"].ToString();
            if (txtpayto.Text.ToLower() == "general")
            {
                pnlCustomerDetails.Visible = false;
            }
            else
            {
                pnlCustomerDetails.Visible = true;
                if (!string.IsNullOrEmpty(dsApproval.Tables[3].Rows[0]["Customer_ID"].ToString().Trim())) //if customer mean it's excute
                {
                    //S3GCustomerPermAddress.ShowCustomerCode = false;
                    //S3GCustomerPermAddress.ShowCustomerName = false;
                    //txtCustomerCode.Text = dsApproval.Tables[3].Rows[0]["Customer_code"].ToString();
                    //txtCustomerName.Text = dsApproval.Tables[3].Rows[0]["Customer_Name"].ToString();
                    S3GCustomerPermAddress.SetCustomerDetails(dsApproval.Tables[3].Rows[0], true);
                    pnlCustomerDetails.GroupingText = "Customer Information";
                    S3GCustomerPermAddress.Caption = "Customer";
                }
                else if (Convert.ToInt32(dsApproval.Tables[3].Rows[0]["Funder_ID"]) > 0)
                {
                    dictDropdownListParam = new Dictionary<string, string>();
                    dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
                    dictDropdownListParam.Add("@TypeID", "150");
                    dictDropdownListParam.Add("@ID", Convert.ToString(dsApproval.Tables[3].Rows[0]["Funder_ID"]));
                    DataTable dtCustEntityDtls = Utility.GetDefaultData("S3G_LOANAD_GETCustomerorEntityDetails", dictDropdownListParam);
                    dictDropdownListParam = null;
                    if (dtCustEntityDtls.Rows.Count > 0)
                    {
                        S3GCustomerPermAddress.SetCustomerDetails(dtCustEntityDtls.Rows[0]["Code"].ToString(),
                                   dtCustEntityDtls.Rows[0]["Address1"].ToString() + "\n" +
                           dtCustEntityDtls.Rows[0]["Address2"].ToString() + "\n" +
                           dtCustEntityDtls.Rows[0]["city"].ToString() + "\n" +
                           dtCustEntityDtls.Rows[0]["state"].ToString() + "\n" +
                           dtCustEntityDtls.Rows[0]["country"].ToString() + "\n" +
                           dtCustEntityDtls.Rows[0]["pincode"].ToString(), dtCustEntityDtls.Rows[0]["Name"].ToString(),
                           dtCustEntityDtls.Rows[0]["Telephone"].ToString(),
                           dtCustEntityDtls.Rows[0]["Mobile"].ToString(),
                           dtCustEntityDtls.Rows[0]["email"].ToString(), dtCustEntityDtls.Rows[0]["website"].ToString());
                        pnlCustomerDetails.GroupingText = "Funder Information";
                        S3GCustomerPermAddress.Caption = "Funder";

                    }
                }
                else//it's not a customer like vendor or sundry creditor like that
                {
                    dictDropdownListParam = new Dictionary<string, string>();
                    dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
                    dictDropdownListParam.Add("@Entity_ID", dsApproval.Tables[3].Rows[0]["VENDOR_CODE"].ToString().Trim());
                    DataTable dtEntityDetails = Utility.GetDefaultData("S3G_ORG_GetEntity_Master_ForApproval", dictDropdownListParam);
                    dictDropdownListParam = null;
                    if (dtEntityDetails.Rows.Count > 0)
                    {
                        //S3GCustomerPermAddress.ShowCustomerCode = false;
                        //S3GCustomerPermAddress.ShowCustomerName = false;
                        //txtCustomerCode.Text = dtEntityDetails.Rows[0]["Customer_code"].ToString();
                        //txtCustomerName.Text = dtEntityDetails.Rows[0]["Customer_Name"].ToString();
                        S3GCustomerPermAddress.SetCustomerDetails(dtEntityDetails.Rows[0], true);
                        pnlCustomerDetails.GroupingText = "Entity Information";
                        S3GCustomerPermAddress.Caption = "Entity";

                    }

                }
            }
            //ddlPaymentReqNumber.FillDataTable(dsApproval.Tables[3], "Request_No", "Payment_Request_No");
            ddlPaymentReqNumber.SelectedValue = dsApproval.Tables[3].Rows[0]["Request_No"].ToString();
            ddlPaymentReqNumber.SelectedText = dsApproval.Tables[3].Rows[0]["Payment_Request_No"].ToString();
            ddlPaymentReqNumber.Enabled = false;

            ddllLineOfBusiness.FillDataTable(dsApproval.Tables[3], "LOB_ID", "LOB_Name");
            ddllLineOfBusiness.SelectedValue = dsApproval.Tables[3].Rows[0]["LOB_ID"].ToString();
            ddllLineOfBusiness.Enabled = false;

            //ddlBranch.FillDataTable(dsApproval.Tables[3], "Location_ID", "Location");
            ddlBranch.SelectedValue = dsApproval.Tables[3].Rows[0]["Location_ID"].ToString();
            ddlBranch.SelectedText = dsApproval.Tables[3].Rows[0]["Location"].ToString();
            ddlBranch.ToolTip = dsApproval.Tables[3].Rows[0]["Location"].ToString();
            ddlBranch.Enabled = false;


            if (dsApproval.Tables[3].Rows[0]["Pay_Mode_Code"].ToString() != "")
            {
                if (dsApproval.Tables[3].Rows[0]["Pay_Mode_Code"].ToString() == "1")
                    txtPayMode.Text = "Cheque";
                else if (dsApproval.Tables[3].Rows[0]["Pay_Mode_Code"].ToString() == "2")
                    txtPayMode.Text = "Demand Draft";
                else if (dsApproval.Tables[3].Rows[0]["Pay_Mode_Code"].ToString() == "3")
                    txtPayMode.Text = "Cash";
            }
            //if (dsApproval.Tables[3].Rows[0]["Customer_Code"].ToString().Trim()==string.Empty)
            //    pnlCustomerDetails.Visible = false;
            //else
            //    pnlCustomerDetails.Visible = true;

            dictDropdownListParam = new Dictionary<string, string>();
            Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);

            PaymentReqID.Visible = true;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlPaymentReqNumber.SelectedValue.ToString(), false, 0);
            hdnID.Value = "../LoanAdmin/S3GLoanAdPaymentRequest.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

            grvApprovalDetails.DataSource = dsApproval.Tables[3];
            grvApprovalDetails.DataBind();
        }
    }
    #endregion

    #region Button (Save / Clear / Cancel)
    protected void btnSave_Click(object sender, EventArgs e)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();
        try
        {
            Label lblApprovalSNO = new Label();
            Label lblApproval_ID = new Label();
            TextBox txtRemarks = new TextBox();
            TextBox txtPassword = new TextBox();
            DropDownList ddlstatus = new DropDownList();
            foreach (GridViewRow grv in grvApprovalDetails.Rows)
            {
                lblApprovalSNO = (Label)grv.FindControl("lblApprovalSNO");
                lblApproval_ID = (Label)grv.FindControl("lblApproval_ID");
                ddlstatus = (DropDownList)grv.FindControl("ddlstatus");
                txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                txtPassword = (TextBox)grv.FindControl("txtPassword");
            }
            S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalRow ObjRow;
            ObjRow = ObjApproval_DataTable.NewS3G_LOANAD_ApprovalRow();
            ObjRow.Task_Number = ddlPaymentReqNumber.SelectedValue.ToString();
            ObjRow.LOB_ID = ddllLineOfBusiness.SelectedValue;
            ObjRow.Branch_ID = ddlBranch.SelectedValue;

            if (!string.IsNullOrEmpty(lblApproval_ID.Text))
                ObjRow.Approval_ID = lblApproval_ID.Text;
            else
                ObjRow.Approval_ID = "0";
            if (!string.IsNullOrEmpty(lblApprovalSNO.Text.Trim()))
                ObjRow.Task_Approval_Serialvalue = lblApprovalSNO.Text.Trim();
            if (ddlstatus.SelectedIndex > 0)
            {
                ObjRow.Task_Status_Type_Code = "9"; //It's exist lookup
                ObjRow.Task_Status_Code = ddlstatus.SelectedValue.ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('please Select the Status');", true);
                return;
            }


            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                return;
            }

            ObjRow.Task_Type = "10";
            ObjRow.Password = txtPassword.Text.Trim();
            ObjRow.Remarks = txtRemarks.Text.Trim();
            ObjRow.Company_ID = intCompanyID.ToString();
            ObjRow.Created_By = intUserID.ToString();
            ObjApproval_DataTable.AddS3G_LOANAD_ApprovalRow(ObjRow);

            string strErrMsg = string.Empty;
            //int intErrorCode = 0;
            int intErrorCode = ObjApproval.FunPubCreateApprovals(out strErrMsg, SerMode, ClsPubSerialize.Serialize(ObjApproval_DataTable, SerMode));
            if (intErrorCode == 0)
            {
                string strAlert = "";
                if (isWorkFlowTraveler)
                {
                    WorkFlowSession WFValues = new WorkFlowSession();
                    if (ddlstatus.SelectedItem.Text == "Approved")
                    {
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strErrMsg, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }
                    }
                    else
                    {
                        try
                        {
                            int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }
                    }
                    ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode, strAlert);
                    return;
                }
                else
                {
                    // FORCE PULL IMPLEMENTATION KR
                    DataTable WFFP = new DataTable();

                    if (CheckForForcePullOperation(ProgramCode, ddlPaymentReqNumber.SelectedText, null, "L", CompanyId, out WFFP))
                    {
                        DataRow dtrForce = WFFP.Rows[0];
                        if (ddlstatus.SelectedItem.Text == "Approved")
                        {
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), strErrMsg, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), "", "", int.Parse(dtrForce["PRODUCTID"].ToString()));
                                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                                btnSave.Enabled = false;
                                //END
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            try
                            {
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());
                                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                                btnSave.Enabled = false;
                                //END
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is not assigned";
                            }
                        }
                    }
                    if (ddlstatus.SelectedValue == "3")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Payment Approved successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PAAP';", true);
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Payment Rejected successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PAAP';", true);
                    }
                    return;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Payment Approval details done successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PAAP';", true);
                    //return;
                }
            }
            else if (intErrorCode == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                return;
            }
            else if (intErrorCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorization rule card not yet set for this combination');", true);
                return;
            }
            else if (intErrorCode == 3)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_Approval_3 + "');", true);
                return;
            }
            else if (intErrorCode == 4)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_Approval_4 + "');", true);
                return;
            }
            else if (intErrorCode == 5)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert2", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_L3A + "');", true);
                return;
            }
            else if (intErrorCode == 15)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert12", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_L4A + "');", true);
                return;
            }
            else if (intErrorCode == 16)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert13", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_L35 + "');", true);
                return;
            }
            else if (intErrorCode == 20)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Payment Approval details added failed.');", true);
                return;
            }
            else if (intErrorCode == 101)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Payment Bank Details Tab Information in payment request.');", true);
                return;
            }
            else if (intErrorCode == 102)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Instrument Number in Payment Bank details tab in payment request.');", true);
                return;
            }
            else if (intErrorCode == 103)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Instrument/Transfer Date in Payment Bank details tab in payment request.');", true);
                return;
            }
            else if (intErrorCode == 104)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Payment Bank details tab in payment request.');", true);
                return;
            }
            else if (intErrorCode == 204)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Favouring name on Payment Bank details tab in payment request.');", true);
                return;
            }
            else if (intErrorCode == 205)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Payment Gateway Ref No on Payment Bank details tab in payment request.');", true);
                return;
            }
            else
            {
                Utility.FunShowValidationMsg(this.Page, "PaReA", intErrorCode, strErrMsg, false);
                return;
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            ObjApproval.Close();
            ObjS3GAdminServices.Close();
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlPaymentReqNumber.SelectedValue) > 0)
            ddlPaymentReqNumber.SelectedValue = "0";
        //if (ddlBranch.SelectedIndex > 0)
        //ddlBranch.SelectedIndex = 0;
        ddlBranch.Clear();
        if (ddllLineOfBusiness.SelectedIndex > 0)
            ddllLineOfBusiness.SelectedIndex = 0;
        ddlPaymentReqNumber.Clear();
        FunPubClear();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=PAAP");
    }
    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        try
        {
            if (grvApprovalDetails.Rows.Count > 0)
            {

                ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();

                TextBox txtPassword = (TextBox)grvApprovalDetails.Rows[0].FindControl("txtPassword");

                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Password.');", true);
                    txtPassword.Focus();
                    return;
                } 

                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@OPTIONS", "6");
                dictDropdownListParam.Add("@TASK_TYPE", "10");
                dictDropdownListParam.Add("@TASK_NUMBER", ddlPaymentReqNumber.SelectedValue.ToString());
                dictDropdownListParam.Add("@USER_ID", ObjUserInfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Password", txtPassword.Text.Trim());

                int ErrCode = ObjApproval.FunPubRevokeOrUpdateApprovedDetails(dictDropdownListParam);

                switch (ErrCode)
                {
                    case 0:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Payment Request Approval details Revoked successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PAAP';", true);
                            return;
                        }
                    case 1:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                            return;
                        }
                    case 2:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('User does not have sufficient privilege to revoke.');", true);
                            return;
                        }
                    case 5:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert(' " + Resources.ValidationMsgs.APPR_Month_Clsr + "');", true);
                            return;
                        }
                    case 8:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Next level approval has been completed, cannot revoke');", true);
                            return;
                        }
                    default:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Unable to revoke.');", true);
                            return;
                        }
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            ObjApproval.Close();
        }
    }
    #endregion

    #region Contorl Events
    public void PubFunBind()
    {
        dictDropdownListParam = new Dictionary<string, string>();
        uinfo = new UserInfo();
        dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        dictDropdownListParam.Add("@Is_Active", "1");
        dictDropdownListParam.Add("@Program_ID", "56");
        ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });

        if (ddllLineOfBusiness.Items.Count == 2)
        {
            ddllLineOfBusiness.SelectedIndex = 1;
            ddllLineOfBusiness.ClearDropDownList();
        }

        //if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
        //{
        //    dictDropdownListParam.Add("@LOB_ID", "0");
        //    ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
        //}
        uinfo = null;
        dictDropdownListParam = null;
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLOBDetails(false);
    }
    //void LoadBranchDetails()
    //{
    //    if (ddlBranch.SelectedIndex > 0)
    //    {
    //        FunPubClear();
    //        btnSave.Enabled = false;
    //        ddlPaymentReqNumber.Items.Clear();
    //        uinfo = new UserInfo();
    //        dictDropdownListParam = new Dictionary<string, string>();
    //        dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
    //        dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
    //        dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
    //        dictDropdownListParam.Add("@Task_Type", "10");
    //        dictDropdownListParam.Add("@Branch_ID", ddlBranch.SelectedValue.ToString());
    //        ddlPaymentReqNumber.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Request_No", "Payment_Request_No" });
    //        dictDropdownListParam = null;
    //    }
    //    else
    //    {
    //        FunPubClear();
    //        btnSave.Enabled = false;
    //        ddlPaymentReqNumber.Items.Clear();

    //    }
    //}
    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddllLineOfBusiness.SelectedValue) > 0)
        {
            ddlBranch.Clear();
        }
        if (ddllLineOfBusiness.SelectedValue == "0")
        {
            FunPubClear();
            ddlPaymentReqNumber.Clear();
            ddlBranch.Clear();
        }
        //  LoadLOBDetails(true);
    }

    protected void chkApproved_CheckedChanged(object sender, EventArgs e)
    {
        ddlBranch.Clear();
        ddlPaymentReqNumber.Clear();
        FunPubClear();
        btnSave.Enabled = false;
        btnRevoke.Enabled = false;
    }

    protected void chkUnapproval_CheckedChanged(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
        btnRevoke.Enabled = false;
        ddlBranch.Clear();
        ddlPaymentReqNumber.Clear();
        FunPubClear();
    }

    void LoadLOBDetails(bool FromLOB)
    {
        if (FromLOB)
        {
            //dictDropdownListParam = new Dictionary<string, string>();
            //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@Is_Active", "1");
            //dictDropdownListParam.Add("@Program_ID", "56");
            //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location" });
        }

        //if (ddllLineOfBusiness.SelectedIndex > 0)
        //{
        //    FunPubClear();
        //    btnSave.Enabled = false;
        //    //ddlPaymentReqNumber.Items.Clear();
        //    uinfo = new UserInfo();
        //    dictDropdownListParam = new Dictionary<string, string>();
        //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
        //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
        //dictDropdownListParam.Add("@Task_Type", "10");
        //    //ddlPaymentReqNumber.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Request_No", "Payment_Request_No" });
        //    dictDropdownListParam = null;
        //}
        //else
        //{

        //    FunPubClear();
        //    //ddlPaymentReqNumber.Items.Clear();
        //    btnSave.Enabled = false;
        //}
    }
    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetApplications(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@LOB_ID", obj_Page.ddllLineOfBusiness.SelectedValue.ToString());
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Task_Type", "10");
        Procparam.Add("@PrefixText", prefixText);
        if (obj_Page.chkApproved.Checked)
        {
            Procparam.Add("@UnApproved", "1");
        }
        else
        {
            Procparam.Add("@UnApproved", "0");
        }
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetPaymentRequestNumber_AGT", Procparam));

        return suggetions.ToArray();
    }
    protected void ddlPaymentReqNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPaymentRequestDetails();
    }
    void LoadPaymentRequestDetails()
    {
        FunPubClear();
        if (Convert.ToInt32(ddlPaymentReqNumber.SelectedValue) > 0)
        {
            btnSave.Enabled = false;
            PaymentReqID.Visible = true;
            btnGo.Enabled = true;
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Task_Type", "10");
            dictDropdownListParam.Add("@Task_Number", ddlPaymentReqNumber.SelectedValue.ToString());
            //dictDropdownListParam.Add("@LOB_ID", Convert.ToString(ddllLineOfBusiness.SelectedValue));
            //dictDropdownListParam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            //dictDropdownListParam.Add("@USER_ID", Convert.ToString(intUserID));

            dsApproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
            if (dsApproval.Tables[1].Rows.Count > 0)
            {
                txtStatus.Text = dsApproval.Tables[1].Rows[0]["Status"].ToString();
                txtPaymentReqDate.Text = DateTime.Parse(dsApproval.Tables[1].Rows[0]["Value_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtpayto.Text = dsApproval.Tables[1].Rows[0]["Customer_Name"].ToString();
                txtpayto.Text = dsApproval.Tables[1].Rows[0]["Lookup_Description"].ToString();
                if (txtpayto.Text.ToLower() == "general")
                {
                    pnlCustomerDetails.Visible = false;
                }
                else
                {
                    pnlCustomerDetails.Visible = true;
                    DataTable dtCustEntityDtls = new DataTable();
                    if (dictDropdownListParam != null)
                        dictDropdownListParam.Clear();
                    else
                        dictDropdownListParam = new Dictionary<string, string>();

                    dictDropdownListParam.Add("@Company_Id", intCompanyID.ToString());
                    if (Convert.ToInt32(dsApproval.Tables[1].Rows[0]["Pay_To"]) == 1)               // For Customer Pay To
                    {
                        dictDropdownListParam.Add("@TypeID", "144");
                        dictDropdownListParam.Add("@ID", Convert.ToString(dsApproval.Tables[1].Rows[0]["Customer_ID"]));
                    }
                    else if (Convert.ToInt32(dsApproval.Tables[1].Rows[0]["Pay_To"]) == 13)         // For Funder Pay To
                    {
                        dictDropdownListParam.Add("@TypeID", "150");
                        dictDropdownListParam.Add("@ID", Convert.ToString(dsApproval.Tables[1].Rows[0]["Funder_ID"]));
                    }
                    else            //For All other Entity Types
                    {
                        dictDropdownListParam.Add("@TypeID", "145");
                        dictDropdownListParam.Add("@ID", Convert.ToString(dsApproval.Tables[1].Rows[0]["VENDOR_CODE"]));
                    }
                    dtCustEntityDtls = Utility.GetDefaultData("S3G_LOANAD_GETCustomerorEntityDetails", dictDropdownListParam);


                    if (!string.IsNullOrEmpty(dsApproval.Tables[1].Rows[0]["Customer_ID"].ToString().Trim())) //if customer mean it's excute
                    {
                        txtStatus.Text = dsApproval.Tables[1].Rows[0]["Status"].ToString();
                        txtPaymentReqDate.Text = DateTime.Parse(dsApproval.Tables[1].Rows[0]["Value_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                        pnlCustomerDetails.GroupingText = "Customer Information";
                        S3GCustomerPermAddress.Caption = "Customer";
                        txtpayto.Text = dsApproval.Tables[1].Rows[0]["Customer_Name"].ToString();
                        txtamount.Text = Convert.ToDecimal(dsApproval.Tables[1].Rows[0]["Pay_Amount"]).ToString("0.00");
                        txtPayMode.Text = dsApproval.Tables[1].Rows[0]["Pay_Mode_Code"].ToString();
                        txtEntityCode.Text = dsApproval.Tables[1].Rows[0]["Entity_Code"].ToString();

                        S3GCustomerPermAddress.SetCustomerDetails(dtCustEntityDtls.Rows[0]["Code"].ToString(),
                                dtCustEntityDtls.Rows[0]["Address1"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["Address2"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["city"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["state"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["country"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["pincode"].ToString(), dtCustEntityDtls.Rows[0]["Name"].ToString(),
                        dtCustEntityDtls.Rows[0]["Telephone"].ToString(),
                        dtCustEntityDtls.Rows[0]["Mobile"].ToString(),
                        dtCustEntityDtls.Rows[0]["email"].ToString(), dtCustEntityDtls.Rows[0]["website"].ToString());
                    }
                    else//it's not a customer then excute this one
                    {
                        if (Convert.ToInt32(dsApproval.Tables[1].Rows[0]["Pay_To"]) == 13)
                        {
                            pnlCustomerDetails.GroupingText = "Funder Information";
                            S3GCustomerPermAddress.Caption = "Funder";
                        }
                        else
                        {
                            pnlCustomerDetails.GroupingText = "Entity Information";
                            S3GCustomerPermAddress.Caption = "Entity";
                        }

                        S3GCustomerPermAddress.SetCustomerDetails(dtCustEntityDtls.Rows[0]["Code"].ToString(),
                                dtCustEntityDtls.Rows[0]["Address1"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["Address2"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["city"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["state"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["country"].ToString() + "\n" +
                        dtCustEntityDtls.Rows[0]["pincode"].ToString(), dtCustEntityDtls.Rows[0]["Name"].ToString(),
                        dtCustEntityDtls.Rows[0]["Telephone"].ToString(),
                        dtCustEntityDtls.Rows[0]["Mobile"].ToString(),
                        dtCustEntityDtls.Rows[0]["email"].ToString(), dtCustEntityDtls.Rows[0]["website"].ToString());

                    }
                }
                txtamount.Text = Convert.ToDecimal(dsApproval.Tables[1].Rows[0]["Pay_Amount"]).ToString("0.00");

                if (dsApproval.Tables[1].Rows[0]["Pay_Mode_Code"].ToString() != "")
                {
                    if (dsApproval.Tables[1].Rows[0]["Pay_Mode_Code"].ToString() == "1")
                        txtPayMode.Text = "Cheque";
                    else if (dsApproval.Tables[1].Rows[0]["Pay_Mode_Code"].ToString() == "2")
                        txtPayMode.Text = "Demand Draft";
                    else if (dsApproval.Tables[1].Rows[0]["Pay_Mode_Code"].ToString() == "3")
                        txtPayMode.Text = "Cash";
                }
                txtEntityCode.Text = dsApproval.Tables[1].Rows[0]["Entity_Code"].ToString();

                //if (dsApproval.Tables[1].Rows[0]["Customer_Name"].ToString().Trim() == string.Empty)
                //{
                //    pnlCustomerDetails.Visible = false;
                //}
                //else
                //{
                //    pnlCustomerDetails.Visible = true;
                //}

                PaymentReqID.Visible = true;
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlPaymentReqNumber.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../LoanAdmin/S3GLoanAdPaymentRequest.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";


            }
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;

        }
    }
    protected void PaymentReqID_serverclick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
        return;
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        ClickGoButtonAction();
    }
    void ClickGoButtonAction()
    {
        if (Convert.ToInt32(ddlPaymentReqNumber.SelectedValue) > 0)
        {
            /*//Code added by saran on 15-Nov-2011 to check the bank details entered or not if paymode is DD/Cheque
            Dictionary<string, string> Procaparam = new Dictionary<string, string>();
            Procaparam.Add("@Request_No", ddlPaymentReqNumber.SelectedValue.ToString());
            Procaparam.Add("@Company_ID", intCompanyID.ToString());
            DataTable dt = Utility.GetDefaultData("S3G_LOANAD_PaymentBankdetailschk", Procaparam);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["ErrorCode"].ToString() == "1")
                {
                    Utility.FunShowAlertMsg(this, "Enter the Payment Bank details tab in payment request.");
                    return;
                }
                else if (dt.Rows[0]["ErrorCode"].ToString() == "2")
                {
                    Utility.FunShowAlertMsg(this, "Enter the Instrument Number in Payment Bank details tab in payment request.");
                    return;
                }
                else if (dt.Rows[0]["ErrorCode"].ToString() == "3")
                {
                    Utility.FunShowAlertMsg(this, "Enter the Instrument Date in Payment Bank details tab in payment request.");
                    return;
                }
                else if (dt.Rows[0]["ErrorCode"].ToString() == "4")
                {
                    Utility.FunShowAlertMsg(this, "Enter the Payment Bank details tab in payment request.");
                    return;
                }

            }
            else
            {
                return;
            }*/

            PaymentReqID.Visible = true;
            btnSave.Enabled = true;
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@Task_Number", ddlPaymentReqNumber.SelectedValue.ToString());
            dictDropdownListParam.Add("@Task_Type", "10");
            DataTable dtCheck = Utility.GetDefaultData("S3G_LOANAD_CheckApprovalRevoke", dictDropdownListParam);

            Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);

            if (chkApproved.Checked && dtCheck != null && dtCheck.Rows.Count > 0)
            {
                ViewState["AppStatus"] = dtCheck.Rows[0]["Task_Status_Code"].ToString();
                grvApprovalDetails.DataSource = dtCheck;
                grvApprovalDetails.DataBind();
                btnSave.Enabled = false;
                btnRevoke.Enabled = true;
                btnGo.Enabled = false;
            }
            else
            {
                ViewState["AppStatus"] = "0";
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    grvApprovalDetails.DataSource = Ds.Tables[0];
                    grvApprovalDetails.DataBind();
                    btnGo.Enabled = false;
                }
            }
        }
    }
    protected void grvApprovalDetails_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            grvApprovalDetails.Columns[3].Visible = true;
            TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
            DropDownList ddlAction = (DropDownList)e.Row.FindControl("ddlstatus");
            ddlAction.FillDataTable(Ds.Tables[1], "Lookup_Code", "Lookup_Description");
            if (strPaymentRequest_No.Trim() != "")
            {
                if (dsApproval.Tables[3].Rows.Count > 0)
                {
                    grvApprovalDetails.Columns[3].Visible = false;
                    ddlAction.SelectedValue = dsApproval.Tables[3].Rows[intRow]["Status_ID"].ToString();
                    txtRemarks.ReadOnly = true;
                    ddlAction.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtStatus.Text) && chkApproved.Checked)
                {
                    ddlAction.SelectedValue = ViewState["AppStatus"].ToString();
                    if (ddlAction.SelectedValue != "0")
                    {
                        ddlAction.ClearDropDownList();
                    }
                }
            }
            intRow++;
            Label lbldate = (Label)e.Row.FindControl("lblApprovalDate");
            if (lbldate.Text.Trim() != string.Empty)
            {
                DateTime Date = Convert.ToDateTime(lbldate.Text);
                lbldate.Text = Date.ToString(strDateFormat);
            }
        }
    }
    public void FunPubClear()
    {
        grvApprovalDetails.DataSource = dsApproval.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();


        PaymentReqID.Visible = false;
        btnSave.Enabled = false;
        btnGo.Enabled = true;
        txtStatus.Text = string.Empty;
        txtPaymentReqDate.Text = string.Empty;
        txtpayto.Text = string.Empty;
        txtEntityCode.Text = string.Empty;
        txtamount.Text = string.Empty;
        txtPayMode.Text = string.Empty;
        S3GCustomerPermAddress.ClearCustomerDetails();
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
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    #endregion
}
