/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// /// <Program Summary>
/// Last Updated By		      :   Thalaiselvam N
/// Last Updated Date         :   03-Sep-2011
/// Reason                    :   Encrypted Password Validation
/// Modified By         : Shibu 
/// Modified Date       : 28-Sep-2013
/// Purpose	            : Performance Tuning DDL to Change AutoSuggestion An Avoid Unwanted DDL Load Events
/// <Program Summary>
/// 
using System;
using System.Globalization;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using S3GBusEntity;
using System.Web.Security;


public partial class S3GLoanAdTLEWCTopupApproval_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    int intTLEWCTopup_ID = 0;
    int intUserID = 0;
    int intRow = 0;
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    SerializationMode SerMode = SerializationMode.Binary;
    DataTable dtAction = new DataTable();
    DataSet dsapproval = new DataSet();
    DataSet Ds = new DataSet();
    string strDateFormat;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    ApprovalMgtServicesReference.ApprovalMgtServicesClient ObjApproval ;
    S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable ObjApproval_DataTable = new S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable();
    public static S3GLoanAdTLEWCTopupApproval_Add obj_Page;
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        // WF Initializtion 
        ProgramCode = "071";
        obj_Page = this;
        try
        {
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                intTLEWCTopup_ID = Convert.ToInt32(fromTicket.Name);
                btnClear.Enabled = false;
            }
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            if (!IsPostBack)
            {
                ReqID.Visible = false;
             
                // ------------Landing Page(Begining)-----------------
                if (Request.QueryString.Get("qsMode") != null)
                {
                    if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
                    {
                        FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                        if (!(string.IsNullOrEmpty(fromTicket.Name)))
                        {
                            intTLEWCTopup_ID = Convert.ToInt32(fromTicket.Name);
                        }
                        btnGo.Enabled = false;
                    }
                    else
                    {
                        PubFunBind();  //Bind LOB
                    }
                }
               
                //--------Landing Page(Ending)-----------------
                if (intTLEWCTopup_ID > 0)
                {
                    FunPubViewApprovedDetails();
                    trAppStatus.Visible = false;  //Added by Tamilselvan.S
                }
            }
            if (PageMode == PageModes.WorkFlow && !IsPostBack)
            {
                // PreparePageForWFLoad();
            }
        }
        catch (Exception ex)
        {
            cvTopUpApproval.IsValid = false;
            cvTopUpApproval.ErrorMessage = "Unable to load page.";
        }
    }

    private void PreparePageForWFLoad()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Button (Save / Clear / Cancel)

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Label lblApprovalSNO = new Label();
        Label lblApproval_ID = new Label();
        TextBox txtRemarks = new TextBox();
        TextBox txtPassword = new TextBox();
        DropDownList ddlstatus = new DropDownList();

        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();

        try
        {
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
            ObjRow.Task_Number = ddlTLEWCNO.SelectedValue.ToString();
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('please select status');", true);
                return;
            }

           
            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                return;
            }

            ObjRow.Task_Type = "14";
            ObjRow.Password = txtPassword.Text.Trim();
            ObjRow.Remarks = txtRemarks.Text.Trim();
            ObjRow.Company_ID = intCompanyID.ToString();
            ObjRow.Created_By = intUserID.ToString();
            ObjApproval_DataTable.AddS3G_LOANAD_ApprovalRow(ObjRow);

            string strErrMsg = string.Empty;
            int intErrorCode = ObjApproval.FunPubCreateApprovals(out strErrMsg, SerMode, ClsPubSerialize.Serialize(ObjApproval_DataTable, SerMode));
            if (intErrorCode == 0)
            {
                //Added by Tamilselvan.S on 24/05/2011 for Setting final approval message hide for tamilselvan on 25/05/2011 for uniforminty
                //if (!string.IsNullOrEmpty(hdnTotalNoApproval.Value) && Convert.ToInt32(hdnTotalNoApproval.Value) == Convert.ToInt32(lblApprovalSNO.Text))
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Top Up Final Approval done successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=TUA';", true);
                //else
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Top Up Approval details added successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=TUA';", true);              
                if (isWorkFlowTraveler)
                {
                    WorkFlowSession WFValues = new WorkFlowSession();
                    try
                    {
                        int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strErrMsg, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    }
                    catch (Exception ex)
                    {
                        strErrMsg = "Work Flow is Not Assigned";
                    }
                    ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode, strErrMsg);
                    return;
                }
                return;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Top Up details Added failed.');", true);
                return;
            }
            else
            {
                Utility.FunShowValidationMsg(this.Page, "LATLE", intErrorCode, strErrMsg, false);
                return;
            }
        }
        catch (Exception ex)
        {
            cvTopUpApproval.ErrorMessage = "Unable to Approve the details.";
            cvTopUpApproval.IsValid = false;
        }
        finally
        {
            ObjS3GAdminServices.Close();
            ObjApproval.Close();
           
        }
    }

    protected void ReqID_serverclick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
        return;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        //if (ddlBranch.SelectedIndex > 0)
        //    ddlBranch.SelectedIndex = 0;
        ddlBranch.Clear();
        if (ddllLineOfBusiness.SelectedIndex > 0)
            ddllLineOfBusiness.SelectedIndex = 0;
        chkUnapproval.Checked = true;
        chkApproved.Checked = false;
        ddlTLEWCNO.Items.Clear();
        FunPubClear();
        S3GCustomerPermAddress.ClearCustomerDetails();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=TUA");
    }

    #endregion

    #region [Contorl Events]

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            S3GCustomerPermAddress.ClearCustomerDetails();
            if (ddlBranch.SelectedValue !="0")
            {
                FunPubClear();
                btnSave.Enabled = false;
                ddlTLEWCNO.Items.Clear();
                FunPubLoadTopUpNumber();

                //uinfo = new UserInfo();
                //dictDropdownListParam = new Dictionary<string, string>();
                //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
                //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                //dictDropdownListParam.Add("@Branch_ID", ddlBranch.SelectedValue.ToString());
                //dictDropdownListParam.Add("@Task_Type", "14");
                //if (chkApproved.Checked)
                //{
                //    dictDropdownListParam.Add("@UnApproved", "1");
                //}
                //else
                //{
                //    dictDropdownListParam.Add("@UnApproved", "0");
                //}
                //ddlTLEWCNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "TLEWCTopup_ID", "TLE_WC_No" });
                //dictDropdownListParam = null;
            }
            else
            {
                FunPubClear();
                btnSave.Enabled = false;
                btnGo.Enabled = true;
                ddlTLEWCNO.Items.Clear();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvTopUpApproval.IsValid = false;
            cvTopUpApproval.ErrorMessage = "Unable to load Topup Number.";
        }
    }

    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            uinfo = new UserInfo();
            //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@Is_Active", "1");
            //dictDropdownListParam.Add("@Program_Id", "71");
            //dictDropdownListParam.Add("@LOB_Id", ddllLineOfBusiness.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            ddlBranch.Clear();
            S3GCustomerPermAddress.ClearCustomerDetails();
            if (ddllLineOfBusiness.SelectedIndex > 0)
            {
                FunPubClear();
                btnSave.Enabled = false;
                ddlTLEWCNO.Items.Clear();
                FunPubLoadTopUpNumber();
                //uinfo = new UserInfo();
                //dictDropdownListParam = new Dictionary<string, string>();
                //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
                //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                //dictDropdownListParam.Add("@Branch_ID", ddlBranch.SelectedValue.ToString());
                //dictDropdownListParam.Add("@Task_Type", "14");
                //ddlTLEWCNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "TLEWCTopup_ID", "TLE_WC_No" });
                //dictDropdownListParam = null;

            }
            else
            {
                FunPubClear();
                btnSave.Enabled = false;
                btnGo.Enabled = true;
                ddlTLEWCNO.Items.Clear();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvTopUpApproval.IsValid = false;
            cvTopUpApproval.ErrorMessage = "Unable to load Topup Number.";
        }
    }

    protected void ddlTLEWCNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            S3GCustomerPermAddress.ClearCustomerDetails();
            if (ddlTLEWCNO.SelectedIndex > 0)
            {
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@Task_Type", "14");
                dictDropdownListParam.Add("@Task_Number", ddlTLEWCNO.SelectedValue.ToString());
                dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
                if (dsapproval.Tables[1].Rows.Count > 0)
                {
                    txtStatus.Text = dsapproval.Tables[1].Rows[0]["Status"].ToString();
                    txtTLEWCDate.Text = DateTime.Parse(dsapproval.Tables[1].Rows[0]["TLE_WC_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    S3GCustomerPermAddress.SetCustomerDetails(dsapproval.Tables[1].Rows[0], true);
                    txtMLA.Text = dsapproval.Tables[1].Rows[0]["PANum"].ToString();
                    if (!string.IsNullOrEmpty(dsapproval.Tables[1].Rows[0]["SANum"].ToString()))
                    {
                        if (dsapproval.Tables[1].Rows[0]["SANum"].ToString().Contains("DUMMY"))                      //Remove(0,dsapproval.Tables[1].Rows[0]["PANum"].ToString().Length).ToUpper()=="DUMMY")
                            txtSLA.Text = "";
                        else
                            txtSLA.Text = dsapproval.Tables[1].Rows[0]["SANum"].ToString();
                    }
                    txtamount.Text = dsapproval.Tables[1].Rows[0]["Current_Finance_Required"].ToString();

                    ReqID.Visible = true;
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlTLEWCNO.SelectedValue.ToString(), false, 0);
                    hdnID.Value = "../LoanAdmin/S3GLoanAdTLEWCTopup_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";
                }
            }
            else
            {
                FunPubClear();
                btnSave.Enabled = false;
                btnGo.Enabled = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvTopUpApproval.IsValid = false;
            cvTopUpApproval.ErrorMessage = "Unable to load Topup Details.";
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlTLEWCNO.SelectedIndex > 0)
            {
                btnSave.Enabled = true;
                uinfo = new UserInfo();
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                dictDropdownListParam.Add("@Task_Number", ddlTLEWCNO.SelectedValue.ToString());
                dictDropdownListParam.Add("@Task_Type", "14");
                DataTable dtCheck = Utility.GetDefaultData("S3G_LOANAD_CheckApprovalRevoke", dictDropdownListParam);


                Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);

                if (chkApproved.Checked && dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    ViewState["AppStatus"] = dtCheck.Rows[0]["Task_Status_Code"].ToString();

                    grvApprovalDetails.DataSource = dtCheck;
                    grvApprovalDetails.DataBind();
                    btnGo.Enabled = false;
                    btnRevoke.Enabled = true;
                    btnSave.Enabled = false;
                }
                else
                {
                    ViewState["AppStatus"] = "0";

                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        grvApprovalDetails.DataSource = Ds.Tables[0];
                        grvApprovalDetails.DataBind();
                        btnGo.Enabled = false;
                        hdnTotalNoApproval.Value = Convert.ToString(Ds.Tables[0].Rows[0]["TotalNoApproval"]);
                    }
                }
                //if (Ds.Tables[0].Rows.Count > 0)
                //{
                //    grvApprovalDetails.DataSource = Ds.Tables[0];
                //    grvApprovalDetails.DataBind();
                //    btnGo.Enabled = false;
                //}
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvTopUpApproval.ErrorMessage = "Unable to load the Approval Details.";
            cvTopUpApproval.IsValid = false;
        }
    }

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();

        try
        {
            if (grvApprovalDetails.Rows.Count > 0)
            {
                TextBox txtPassword = (TextBox)grvApprovalDetails.Rows[0].FindControl("txtPassword");
                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Password.');", true);
                    txtPassword.Focus();
                    return;
                }

               
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@OPTIONS", "3");  //Topup Revoke operation done this level in SP
                dictDropdownListParam.Add("@TASK_TYPE", "14");
                dictDropdownListParam.Add("@TASK_NUMBER", ddlTLEWCNO.SelectedValue);
                dictDropdownListParam.Add("@USER_ID", ObjUserInfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Password", txtPassword.Text.Trim());

                int ErrCode = ObjApproval.FunPubRevokeOrUpdateApprovedDetails(dictDropdownListParam);
                switch (ErrCode)
                {
                    case 0:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Topup Approval details Revoked successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=TUA';", true);
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
                    // Added By Thangam M On 29/Dec/2011 to check for month closure
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvTopUpApproval.IsValid = false;
            cvTopUpApproval.ErrorMessage = "Unable to revoke the Topup details.";
        }
        finally
        {
            ObjApproval.Close();

            //if (ObjApproval != null)
            //    ObjApproval.Close();
        }
    }

    protected void grvApprovalDetails_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                grvApprovalDetails.Columns[3].Visible = true;
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                DropDownList ddlstatus = (DropDownList)e.Row.FindControl("ddlstatus");
                ddlstatus.FillDataTable(Ds.Tables[1], "Lookup_Code", "Lookup_Description");
                if (intTLEWCTopup_ID > 0)
                {
                    if (dsapproval.Tables[3].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(txtStatus.Text))
                        {
                            grvApprovalDetails.Columns[3].Visible = false;
                            ddlstatus.SelectedValue = dsapproval.Tables[3].Rows[intRow]["Status_ID"].ToString();
                            txtRemarks.Enabled = false;
                            ddlstatus.Enabled = false;
                            btnSave.Enabled = false;

                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtStatus.Text) && chkApproved.Checked)
                    {
                        ddlstatus.SelectedValue = ViewState["AppStatus"].ToString();
                        if (ddlstatus.SelectedValue != "0")
                        {
                            ddlstatus.ClearDropDownList();
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
        catch (Exception ex)
        {
            cvTopUpApproval.ErrorMessage = "Unable to bind details Approval Details.";
            cvTopUpApproval.IsValid = false;
        }
    }

    protected void chkUnapproval_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClear();
            S3GCustomerPermAddress.ClearCustomerDetails();
            FunPubLoadTopUpNumber();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvTopUpApproval.IsValid = false;
            cvTopUpApproval.ErrorMessage = "Unable to load Topup Number.";
        }
    }

    protected void chkApproved_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClear();
            S3GCustomerPermAddress.ClearCustomerDetails();
            FunPubLoadTopUpNumber();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvTopUpApproval.IsValid = false;
            cvTopUpApproval.ErrorMessage = "Unable to load Topup Number.";
        }
    }

    #endregion [Contorl Events]

    #region [Function's]

    public void FunPubViewApprovedDetails()
    {
        try
        {
            btnGo.Enabled = false;
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());//Account Closure APPROVAL SCREEN LOOKUPCODE IS 15
            dictDropdownListParam.Add("@Task_Type", "14");//Account Closure APPROVAL SCREEN LOOKUPCODE IS 15
            dictDropdownListParam.Add("@Task_Number", intTLEWCTopup_ID.ToString());
            dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
            if (dsapproval.Tables[3].Rows.Count > 0)
            {
                txtStatus.Text = dsapproval.Tables[3].Rows[0]["status1"].ToString();
                txtTLEWCDate.Text = DateTime.Parse(dsapproval.Tables[3].Rows[0]["TLE_WC_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                S3GCustomerPermAddress.SetCustomerDetails(dsapproval.Tables[3].Rows[0], true);
                txtMLA.Text = dsapproval.Tables[3].Rows[0]["PANum"].ToString();
                if (!string.IsNullOrEmpty(dsapproval.Tables[3].Rows[0]["SANum"].ToString()))
                {
                    if (dsapproval.Tables[3].Rows[0]["SANum"].ToString().Contains("DUMMY"))                      //Remove(0,dsapproval.Tables[1].Rows[0]["PANum"].ToString().Length).ToUpper()=="DUMMY")
                        txtSLA.Text = "";
                    else
                        txtSLA.Text = dsapproval.Tables[3].Rows[0]["SANum"].ToString();

                }
                txtamount.Text = dsapproval.Tables[3].Rows[0]["Current_Finance_Required"].ToString();

                ddlTLEWCNO.FillDataTable(dsapproval.Tables[3], "TLEWCTopup_ID", "TLE_WC_No");
                ddlTLEWCNO.SelectedValue = dsapproval.Tables[3].Rows[0]["TLEWCTopup_ID"].ToString();
                ddlTLEWCNO.Enabled = false;

                ddllLineOfBusiness.FillDataTable(dsapproval.Tables[3], "LOB_ID", "LOB_Name");
                ddllLineOfBusiness.SelectedValue = dsapproval.Tables[3].Rows[0]["LOB_ID"].ToString();
                ddllLineOfBusiness.Enabled = false;

                //ddlBranch.FillDataTable(dsapproval.Tables[3], "Location_ID", "Location_Name");
                ddlBranch.SelectedText = dsapproval.Tables[3].Rows[0]["Location_Name"].ToString();
                ddlBranch.SelectedValue = dsapproval.Tables[3].Rows[0]["Location_ID"].ToString();
                ddlBranch.Enabled = false;

                Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);

                ReqID.Visible = true;
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlTLEWCNO.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../LoanAdmin/S3GLoanAdTLEWCTopup_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

                grvApprovalDetails.DataSource = dsapproval.Tables[3];
                grvApprovalDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    public void PubFunBind()
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            uinfo = new UserInfo();
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            dictDropdownListParam.Add("@Is_Active", "1");
            dictDropdownListParam.Add("@Program_Id", "71");
            ddllLineOfBusiness.BindDataTable("S3G_LOANAD_GetTLEWCLOBList", dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            //dictDropdownListParam.Add("@LOB_Id", ddllLineOfBusiness.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            uinfo = null;
            dictDropdownListParam = null;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    /// <summary>
    /// Created by Tamilselvan.S
    /// Created Date 25/04/2011
    /// For loading Topup Number based on Approval,LOB and  Branch
    /// </summary>
    public void FunPubLoadTopUpNumber()
    {
        try
        {
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            dictDropdownListParam.Add("@Task_Type", "14");
            if (chkApproved.Checked)
            {
                dictDropdownListParam.Add("@UnApproved", "1");
            }
            else
            {
                dictDropdownListParam.Add("@UnApproved", "0");
            }
            ddlTLEWCNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "TLEWCTopup_ID", "TLE_WC_No" });
            dictDropdownListParam = null;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    public void FunPubClear()
    {
        grvApprovalDetails.DataSource = dsapproval.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();

        ReqID.Visible = false;
        btnSave.Enabled = false;
        btnRevoke.Enabled = false;
        btnGo.Enabled = true;
        txtMLA.Text = string.Empty;
        txtSLA.Text = string.Empty;
        txtamount.Text = string.Empty;
        txtStatus.Text = string.Empty;
        txtTLEWCDate.Text = string.Empty;
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
        Procparam.Add("@Program_Id", "071");
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    #endregion [Function's]

}
