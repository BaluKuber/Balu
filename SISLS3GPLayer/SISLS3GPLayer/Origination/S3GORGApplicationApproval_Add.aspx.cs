/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// /// <Program Summary>
/// Last Updated By		      :   Thalaiselvam N
/// Last Updated Date         :   03-Sep-2011
/// Reason                    :   Encrypted Password Validation
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

public partial class S3GORGApplicationApproval_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    int intApplication_Process_ID = 0;
    int intUserID = 0;
    int intRow = 0;
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    SerializationMode SerMode = SerializationMode.Binary;
    DataTable dtAction = new DataTable();
    DataSet Ds = new DataSet();
    DataSet dsApplicationNumberDetails = new DataSet();
    string strDateFormat;
    string strPassword;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjApplicationApproval;
    S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ApplicationApprovalDataTable ObjApplicationApproval_DataTable = new S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ApplicationApprovalDataTable();
    public static S3GORGApplicationApproval_Add obj_Page;
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            // WF Initializtion 
            ProgramCode = "037";

            obj_Page = this;

            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            if (!IsPostBack)
            {
                btnRevoke.Enabled = false;
                PaymentReqID.Visible = false;
                ViewState["strCreate"] = "0";
                if (Request.QueryString["qsViewId"] == null)
                {
                    chkUnapproval.Checked = true;
                    chkUnapproval_CheckedChanged(null, null);
                }
                // FunPubloadCombo();
                // ------------Landing Page(Begining)-----------------
                if (Request.QueryString.Get("qsMode") != null)//qsViewId
                {
                    if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
                    {
                        FormsAuthenticationTicket ApplicationNo = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                        if (!(string.IsNullOrEmpty(ApplicationNo.Name)))
                        {
                            ddlApplicationNumber.SelectedValue = ApplicationNo.Name;
                            intApplication_Process_ID = Convert.ToInt32(ApplicationNo.Name);
                            // btnRevoke.Enabled = false;

                        }
                        chkApproved.Checked = true;
                    }
                }
                //--------Landing Page(Ending)-----------------

                if (Request.QueryString.Get("qsMode") == "C")
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                }

                if (intApplication_Process_ID > 0)
                {
                    FunPubViewApprovedDetails();
                    if (Request.QueryString.Get("qsMode") == "M")
                    {
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    }

                    if (Request.QueryString.Get("qsMode") == "Q")
                    {
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    }

                }
                else
                {
                    btnSave.Enabled = false;
                    btnRevoke.Enabled = false;
                    btnClear.Enabled = false;
                    btnGo.Enabled = false;
                }


                // WORK FLOW IMPLEMENTATION
                if (PageMode == PageModes.WorkFlow)
                {
                    PreparePageForWFLoad();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void PreparePageForWFLoad()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();
        try
        {
            // WORK FLOW 
            WorkFlowSession WFSessionValue = new WorkFlowSession();
            byte[] byte_ToDoList = objWorkflowToDoClient.FunPubLoadApplicationApproval(WFSessionValue.WorkFlowDocumentNo, int.Parse(CompanyId), int.Parse(UserId));
            DataSet dsApplicationDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_ToDoList, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsApplicationDetails.Tables.Count > 0)
            {
                if (dsApplicationDetails.Tables[0].Rows.Count > 0)
                {
                    DataRow ApplicationRow = dsApplicationDetails.Tables[0].Rows[0];
                    IdValue = Convert.ToInt32(ApplicationRow["Application_Process_ID"]);
                    chkUnapproval.Checked = true;
                    ddllLineOfBusiness.SelectedValue = Convert.ToString(ApplicationRow["LOB_Id"]);
                    //ddllLineOfBusiness.ClearDropDownList();
                    ddlBranch.SelectedValue = Convert.ToString(ApplicationRow["Location_Id"]);
                    //ddlBranch.ClearDropDownList();

                    //ddlApplicationNumber.FillDataTable(dsApplicationDetails.Tables[0], "Application_Process_ID", "Application_Number");
                    ddlApplicationNumber.SelectedText = dsApplicationDetails.Tables[0].Rows[0]["Application_Number"].ToString();
                    ddlApplicationNumber.SelectedValue = IdValue.ToString();
                    //ddlApplicationNumber.ClearDropDownList();
                    LoadApplicationDetails();
                }
            }
            GoClick();
            ddllLineOfBusiness.ClearDropDownList();
            //ddlBranch.ClearDropDownList();
            ddlBranch.ReadOnly = true;
            ddlApplicationNumber.ReadOnly = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            objWorkflowToDoClient.Close();
        }
    }
    public void FunPubloadCombo()
    {
        try
        {
            ddllLineOfBusiness.Items.Insert(0, "--Select--");
            //ddlBranch.Items.Insert(0, "--Select--");
            //ddlApplicationNumber.Items.Insert(0, "--Select--");
            ddlApplicationNumber.Clear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public void FunPubViewApprovedDetails()
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Application_Process_ID", intApplication_Process_ID.ToString());
            dsApplicationNumberDetails = Utility.GetDataset("S3G_ORG_GetApplicationNumber", dictDropdownListParam);
            if (dsApplicationNumberDetails.Tables[5].Rows.Count > 0)
            {
                txtStatus.Text = dsApplicationNumberDetails.Tables[5].Rows[0]["ISstatus"].ToString();
                if (!string.IsNullOrEmpty(dsApplicationNumberDetails.Tables[5].Rows[0]["Date"].ToString()))
                    txtOfferDate.Text = DateTime.Parse(dsApplicationNumberDetails.Tables[5].Rows[0]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                S3GCustomerPermAddress.SetCustomerDetails(dsApplicationNumberDetails.Tables[5].Rows[0], true);

                ListItem lst;

                //ddlApplicationNumber.FillDataTable(dsApplicationNumberDetails.Tables[5], "Application_Process_ID", "Application_Number");
                ddlApplicationNumber.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[0]["Application_Process_ID"].ToString();
                ddlApplicationNumber.SelectedText = dsApplicationNumberDetails.Tables[5].Rows[0]["Application_Number"].ToString();
                ddlApplicationNumber.ToolTip = dsApplicationNumberDetails.Tables[5].Rows[0]["Application_Number"].ToString();
                ddlApplicationNumber.Enabled = false;

                lst = new ListItem(dsApplicationNumberDetails.Tables[5].Rows[0]["LOB_Name"].ToString(), dsApplicationNumberDetails.Tables[5].Rows[0]["LOB_ID"].ToString());
                ddllLineOfBusiness.Items.Add(lst);

                //ddllLineOfBusiness.FillDataTable(dsApplicationNumberDetails.Tables[5], "LOB_ID", "LOB_Name");
                ddllLineOfBusiness.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[0]["LOB_ID"].ToString();
                ddllLineOfBusiness.Enabled = false;

                //ddlBranch.FillDataTable(dsApplicationNumberDetails.Tables[5], "Location_ID", "Location_Name");
                ddlBranch.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = dsApplicationNumberDetails.Tables[5].Rows[0]["Location_Name"].ToString();
                ddlBranch.Enabled = false;

                txtStatus.ReadOnly = true;
                txtOfferDate.ReadOnly = true;
                txtOfferDate.ReadOnly = true;

                chkApproved.Enabled = false;
                chkUnapproval.Enabled = false;

                PaymentReqID.Visible = true;
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlApplicationNumber.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../Origination/S3G_ORG_ApplicationProcessing.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

                grvApprovalDetails.DataSource = dsApplicationNumberDetails.Tables[5];
                grvApprovalDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    #region Button (Save / Clear / Cancel)
    protected void btnSave_Click(object sender, EventArgs e)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        ObjApplicationApproval = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
        try
        {
            if (chkApproved.Checked == true || chkUnapproval.Checked == true)
            {
                Label lblApprovalSNO = new Label();
                Label lblApplication_Approval_ID = new Label();
                TextBox txtRemarks = new TextBox();
                TextBox txtPassword = new TextBox();
                DropDownList ddlAction = new DropDownList();
                foreach (GridViewRow grv in grvApprovalDetails.Rows)
                {
                    lblApprovalSNO = (Label)grv.FindControl("lblApprovalSNO");
                    lblApplication_Approval_ID = (Label)grv.FindControl("lblApplication_Approval_ID");
                    ddlAction = (DropDownList)grv.FindControl("ddlAction");
                    txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                    txtPassword = (TextBox)grv.FindControl("txtPassword");
                }
                S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ApplicationApprovalRow ObjRow;
                ObjRow = ObjApplicationApproval_DataTable.NewS3G_ORG_ApplicationApprovalRow();
                ObjRow.Application_Process_ID = Convert.ToInt32(ddlApplicationNumber.SelectedValue.ToString());
                if (!string.IsNullOrEmpty(lblApplication_Approval_ID.Text.Trim()))
                    ObjRow.Application_Approval_ID = Convert.ToInt32(lblApplication_Approval_ID.Text.Trim());
                else
                    ObjRow.Application_Approval_ID = 0;
                if (!string.IsNullOrEmpty(lblApprovalSNO.Text.Trim()))
                    ObjRow.Approval_Serial_Number = Convert.ToInt32(lblApprovalSNO.Text.Trim());
                if (ddlAction.SelectedIndex > 0)
                    ObjRow.Action_ID = Convert.ToInt32(ddlAction.SelectedValue.ToString());
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Status');", true);
                    return;
                }
                ObjRow.Approver_ID = intUserID;
                if (txtPassword.Text.Trim() != "")
                    ObjRow.Password = txtPassword.Text.Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Password.');", true);
                    return;
                }

                if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                    return;
                }

                if (txtRemarks.Text.Trim() != "")
                    ObjRow.Remarks = txtRemarks.Text.Trim();
                ObjRow.Company_ID = intCompanyID;
                ObjRow.Created_By = intUserID;
                ObjApplicationApproval_DataTable.AddS3G_ORG_ApplicationApprovalRow(ObjRow);
                int ApprovalStatus = 0;
                int intErrorCode = ObjApplicationApproval.FunPubCreateApplicationApproval(out ApprovalStatus, SerMode, ClsPubSerialize.Serialize(ObjApplicationApproval_DataTable, SerMode));
                if (intErrorCode == 0)
                {
                    string strAlert = "";
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        if (ApprovalStatus == 1)  // APPROVED
                        {
                            try
                            {
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, WFValues.Document_Type);

                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here

                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is not assigned";
                            }
                        }
                        else if (ApprovalStatus == 2 || ApprovalStatus == 4) // In case of Approval Rejected or cancelled - By Rao 25 Jan 2012.
                        {
                            try
                            {
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());

                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here

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
                        DataTable WFFP = new DataTable();
                        // By Palani Kumar.A for Customer Name remove after Concatenation on 17/01/2014
                        string strBusinessNo = string.Empty;
                        if (!string.IsNullOrEmpty(ddlApplicationNumber.SelectedText.ToString()))
                            strBusinessNo = ddlApplicationNumber.SelectedText.Substring(0, ddlApplicationNumber.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();

                        if (CheckForForcePullOperation(null, strBusinessNo, ProgramCode, null, null, "O", CompanyId, null, int.Parse(hProductId.Value), ddllLineOfBusiness.SelectedItem.Text, null, out WFFP))
                        {

                            DataRow dtrForce = WFFP.Rows[0];
                            if (ApprovalStatus == 1)
                            {
                                try
                                {
                                    int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), strBusinessNo, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), 3);

                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here

                                    strAlert = "";
                                }
                                catch (Exception ex)
                                {
                                    strAlert = "Work Flow not Assigned";
                                }
                            }
                            else if (ApprovalStatus == 2 || ApprovalStatus == 4)
                            {
                                try
                                {
                                    //int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(),UserId.ToString(),
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());

                                    strAlert = "";

                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here
                                }
                                catch (Exception ex)
                                {
                                    strAlert = "Work Flow is not assigned";
                                }
                            }

                        }

                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Application Approval done successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=APAP&mODIFY=0';", true);
                        if (ddlAction.SelectedValue == "47")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Application Approved successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=APAP&mODIFY=0';", true);
                        }
                        else if (ddlAction.SelectedValue == "120")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Application Cancelled successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=APAP&mODIFY=0';", true);
                        }
                        else if (ddlAction.SelectedValue == "51")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Application Rejected successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=APAP&mODIFY=0';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Application held successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=APAP&mODIFY=0';", true);
                        }
                        return;
                    }
                }
                else if (intErrorCode == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                    return;
                }
                else if (intErrorCode == 2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorization Rule Card not defined for this combination');", true);
                    return;
                }
                else if (intErrorCode == 3)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Approval should be in sequence as defined in Rule Card');", true);
                    return;
                }
                else if (intErrorCode == 4)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Approval Already done for this application number');", true);
                    return;
                }
                else if (intErrorCode == 5)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert2", "alert('Approver not having access to approve the Application.');", true);
                    return;
                }
                else if (intErrorCode == 10)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert14", "alert('Prime Account number not generating from Document number control. So approval action failed.');", true);
                    return;
                }
                else if (intErrorCode == 11)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert14", "alert('Prime Account number not generating from Document number control. Document Number control TO Number Exceed.');", true);
                    return;
                }
                else if (intErrorCode == 15)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert12", "alert('" + Resources.LocalizationResources.Level4andAboveApproval + "');", true);
                    return;
                }
                else if (intErrorCode == 16)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert13", "alert('3 and 5 level users can only approve.');", true);
                    //Modified by Kali on 09_Feb_2012.Since msg should be only Level 3 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert13", "alert('level 3 users can only approve.');", true);
                    return;
                }
                else if (intErrorCode == 20)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert4", "alert('Application Approval Details Approved Failed.');", true);
                    return;
                }
                else if (intErrorCode == 25)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert12", "alert('Customer Approved Amount limit is exceeded');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjS3GAdminServices.Close();
            ObjApplicationApproval.Close();
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClear();
            ddllLineOfBusiness.Items.Clear();
            ddlBranch.Clear();
            ddlApplicationNumber.Clear();
            chkApproved.Checked = false;
            chkUnapproval.Checked = false;
            btnGo.Enabled = false;
            FunPubloadCombo();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void PaymentReqID_serverclick(object sender, EventArgs e)
    {
        try
        {
            if (hdnID.Value.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select application number.');", true);
                return;
            }
            //Response.Write(hdnID.Value.ToString());
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //wf Cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else
                Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=APAP&mODIFY=0");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    #region Contorl Events
    protected void chkUnapproval_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkUnapproval.Checked)
            {


                FunPubClear();
                ddllLineOfBusiness.Items.Clear();
                ddlBranch.Clear();

                UserInfo uinfo = null;
                Dictionary<string, string> dictDropdownListParam;
                dictDropdownListParam = new Dictionary<string, string>();
                uinfo = new UserInfo();
                dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
                dictDropdownListParam.Add("@Is_Active", "1");
                dictDropdownListParam.Add("@Program_ID", "37");
                ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
                //if (Request.QueryString.Get("qsMode") != "C")
                //{
                //    ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
                //}
                ddlApplicationNumber.Clear();
                //ddlApplicationNumber.Items.Insert(0, "--Select--");
                uinfo = null;
                dictDropdownListParam = null;
            }
            if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
            {
                btnSave.Enabled = false;
                btnRevoke.Enabled = false;
                btnClear.Enabled = false;

                ddllLineOfBusiness.Items.Clear();
                ddlBranch.Clear();
                ddlApplicationNumber.Clear();

                FunPubClear();
                FunPubloadCombo();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }


    protected void chkApproved_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkApproved.Checked)
            {
                btnSave.Enabled = false;
                btnRevoke.Enabled = false;
                btnClear.Enabled = false;

                FunPubClear();
                ddllLineOfBusiness.Items.Clear();
                ddlBranch.Clear();
                UserInfo uinfo = null;
                Dictionary<string, string> dictDropdownListParam;
                dictDropdownListParam = new Dictionary<string, string>();
                uinfo = new UserInfo();
                dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
                dictDropdownListParam.Add("@Is_Active", "1");
                dictDropdownListParam.Add("@Program_ID", "37");
                ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
                //if (Request.QueryString.Get("qsMode") != "C")
                //{
                //    ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
                //}
                ddlApplicationNumber.Clear();
                //ddlApplicationNumber.Items.Insert(0, "--Select--");
                uinfo = null;
                dictDropdownListParam = null;
            }
            else
            {
                if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
                {


                    ddllLineOfBusiness.Items.Clear();
                    ddlBranch.Clear();
                    ddlApplicationNumber.Clear();

                    FunPubClear();
                    FunPubloadCombo();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
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
        if (obj_Page.chkUnapproval.Checked)
        {
            Procparam.Add("@Approved", "1");
        }
        else
        {
            Procparam.Add("@Approved", "2");
        }
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetApplicationNumber_AGT", Procparam));

        return suggetions.ToArray();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddllLineOfBusiness.SelectedIndex > 0)
            {
                if (chkUnapproval.Checked)
                {
                    FunPubClear();
                    ddlApplicationNumber.Clear();
                    //uinfo = new UserInfo();
                    //dictDropdownListParam = new Dictionary<string, string>();
                    //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                    //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                    //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                    //dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                    //ddlApplicationNumber.BindDataTable("S3G_ORG_GetApplicationNumber", dictDropdownListParam, new string[] { "Application_Process_ID", "Application_Number" });
                    //dictDropdownListParam = null;
                    //if (ddlApplicationNumber.Items.Count == 0)
                    //{
                    //    ddlApplicationNumber.Items.Insert(0, "--Select--");
                    //}
                }
                else
                {
                    FunPubClear();
                    ddlApplicationNumber.Clear();
                    //uinfo = new UserInfo();
                    //dictDropdownListParam = new Dictionary<string, string>();
                    //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                    //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                    //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                    //dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                    //Ds = Utility.GetDataset("S3G_ORG_GetApplicationNumber", dictDropdownListParam);
                    //if (Ds.Tables[7].Rows.Count > 0)
                    //{
                    //    ddlApplicationNumber.FillDataTable(Ds.Tables[7], "Application_Process_ID", "Application_Number");
                    //}
                    //if (ddlApplicationNumber.Items.Count == 0)
                    //{
                    //    ddlApplicationNumber.Items.Insert(0, "--Select--");
                    //}

                }
            }
            //if (ddlBranch.SelectedIndex == 0)
            if (ddlBranch.SelectedValue == "0")
            {
                btnSave.Enabled = false;
                btnRevoke.Enabled = false;
                btnClear.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "37");
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Code block added to clear controls - Bug_ID - 5706 - Kuppusamy.B - 24-March-2012
            //Starts
            ddlBranch.Clear();
            //ddlBranch.Items.Insert(0, "--Select--");
            ddlApplicationNumber.Clear();
            //ddlApplicationNumber.Items.Insert(0, "--Select--");
            FunPubClear();
            //Ends

            Dictionary<string, string> dictDropdownListParam;
            dictDropdownListParam = new Dictionary<string, string>();
            uinfo = new UserInfo();
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            dictDropdownListParam.Add("@Is_Active", "1");
            dictDropdownListParam.Add("@Program_ID", "37");
            if (ddllLineOfBusiness.SelectedValue != "0")
            {
                dictDropdownListParam.Add("@Lob_Id", ddllLineOfBusiness.SelectedValue.ToString());
            }
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });

            if (ddlBranch.SelectedValue != "0")
            {
                if (chkUnapproval.Checked)
                {
                    FunPubClear();
                    ddlApplicationNumber.Clear();
                    //uinfo = new UserInfo();
                    //dictDropdownListParam = new Dictionary<string, string>();
                    //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                    //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                    //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                    //dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                    //ddlApplicationNumber.BindDataTable("S3G_ORG_GetApplicationNumber", dictDropdownListParam, new string[] { "Application_Process_ID", "Application_Number" });
                    //dictDropdownListParam = null;
                    //if (ddlApplicationNumber.Items.Count == 0)
                    //{
                    //    ddlApplicationNumber.Items.Insert(0, "--Select--");
                    //}
                }
                else
                {

                    FunPubClear();
                    ddlApplicationNumber.Clear();
                    //uinfo = new UserInfo();
                    //dictDropdownListParam = new Dictionary<string, string>();
                    //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                    //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                    //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                    //dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                    //Ds = Utility.GetDataset("S3G_ORG_GetApplicationNumber", dictDropdownListParam);
                    //if (Ds.Tables[7].Rows.Count > 0)
                    //{
                    //    ddlApplicationNumber.FillDataTable(Ds.Tables[7], "Application_Process_ID", "Application_Number");
                    //}
                    //if (ddlApplicationNumber.Items.Count == 0)
                    //{
                    //    ddlApplicationNumber.Items.Insert(0, "--Select--");
                    //}
                }
            }
            if (ddllLineOfBusiness.SelectedIndex == 0)
            {
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                btnRevoke.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void ddlApplicationNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadApplicationDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void LoadApplicationDetails()
    {
        try
        {
            if (ddlApplicationNumber.SelectedValue != "0")
            {
                FunPubClear();
                btnSave.Enabled = false;
                btnRevoke.Enabled = false;
                btnClear.Enabled = false;
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@User_ID", intUserID.ToString());
                dictDropdownListParam.Add("@Application_Process_ID", ddlApplicationNumber.SelectedValue.ToString());
                dsApplicationNumberDetails = Utility.GetDataset("S3G_ORG_GetApplicationNumber", dictDropdownListParam);
                if (dsApplicationNumberDetails.Tables[1].Rows.Count > 0)
                {
                    txtStatus.Text = dsApplicationNumberDetails.Tables[1].Rows[0]["name"].ToString();
                    hProductId.Value = dsApplicationNumberDetails.Tables[1].Rows[0]["Product_Id"].ToString();
                    if (!string.IsNullOrEmpty(dsApplicationNumberDetails.Tables[1].Rows[0]["Date"].ToString()))
                        txtOfferDate.Text = DateTime.Parse(dsApplicationNumberDetails.Tables[1].Rows[0]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                    if (dsApplicationNumberDetails.Tables[1].Rows[0]["IsCreate"].ToString() == "1")
                    {
                        btnGo.Enabled = false;
                        ViewState["strCreate"] = "1";
                    }
                    else
                    {
                        btnGo.Enabled = true;
                    }

                    S3GCustomerPermAddress.SetCustomerDetails(dsApplicationNumberDetails.Tables[1].Rows[0], true);

                    txtStatus.ReadOnly = true;
                    txtOfferDate.ReadOnly = true;
                    txtOfferDate.ReadOnly = true;

                    PaymentReqID.Visible = true;
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlApplicationNumber.SelectedValue.ToString(), false, 0);
                    hdnID.Value = "../Origination/S3G_ORG_ApplicationProcessing.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

                    btnGo.Enabled = true;

                }
            }
            else
            {
                FunPubClear();
                btnSave.Enabled = false;
                btnRevoke.Enabled = false;
                btnClear.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            GoClick();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void GoClick()
    {
        try
        {
            if (ddlApplicationNumber.SelectedValue != "0")
            {
                uinfo = new UserInfo();
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Application_Process_ID", ddlApplicationNumber.SelectedValue.ToString());
                dsApplicationNumberDetails = Utility.GetDataset("S3G_ORG_GetApplicationNumber", dictDropdownListParam);


                btnSave.Enabled = true;
                btnClear.Enabled = true;

                if (dsApplicationNumberDetails.Tables[3].Rows.Count > 0)
                {
                    grvApprovalDetails.DataSource = dsApplicationNumberDetails.Tables[3];
                    grvApprovalDetails.DataBind();
                    btnGo.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
                DropDownList ddlAction = (DropDownList)e.Row.FindControl("ddlAction");
                ddlAction.FillDataTable(dsApplicationNumberDetails.Tables[2], "LookUp_ID", "Name");
                if (intApplication_Process_ID > 0)
                {
                    if (dsApplicationNumberDetails.Tables[5].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(txtStatus.Text))
                        {
                            if (txtStatus.Text.ToUpper().Trim() == "APPROVED" || txtStatus.Text.ToUpper().Trim() == "PENDING")
                            {
                                grvApprovalDetails.Columns[3].Visible = false;
                                ddlAction.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[0]["Action_ID"].ToString();
                                txtRemarks.Enabled = false;
                                ddlAction.Enabled = false;
                                btnSave.Enabled = false;
                                btnRevoke.Enabled = false;
                                btnClear.Enabled = false;
                                btnGo.Enabled = false;
                            }

                        }
                    }
                }
                if (intApplication_Process_ID == 0 || txtStatus.Text.ToUpper().Trim() == "APPROVED" || txtStatus.Text.ToUpper().Trim() == "REJECTED")
                {
                    if (dsApplicationNumberDetails.Tables[5].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(txtStatus.Text))
                        {
                            if (txtStatus.Text.ToUpper().Trim() == "APPROVED" || txtStatus.Text.ToUpper().Trim() == "REJECTED")
                            {
                                grvApprovalDetails.Columns[3].Visible = false;
                                ddlAction.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[intRow]["Action_ID"].ToString();
                                txtRemarks.Enabled = false;
                                ddlAction.Enabled = false;
                                btnSave.Enabled = false;
                                btnRevoke.Enabled = false;
                                btnClear.Enabled = false;
                                btnGo.Enabled = false;
                            }

                        }
                    }
                }
                if (dsApplicationNumberDetails.Tables[3].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtStatus.Text))
                    {
                        if (dsApplicationNumberDetails.Tables[3].Rows[intRow]["Action_ID"].ToString().Trim() != "")
                        {
                            ddlAction.SelectedValue = dsApplicationNumberDetails.Tables[3].Rows[intRow]["Action_ID"].ToString();
                            if (dsApplicationNumberDetails.Tables[3].Rows[intRow]["Action_ID"].ToString().Trim() != "49" && Request.QueryString["qsMode"].Trim() == "C")
                            {
                                ddlAction.Enabled = false;
                                btnSave.Enabled = false;
                                btnRevoke.Enabled = true;
                                txtRemarks.Enabled = false;
                            }
                            else
                            {
                                if (Request.QueryString["qsMode"].Trim() == "C")
                                {
                                    ddlAction.Enabled = true;
                                    btnSave.Enabled = true;
                                    btnRevoke.Enabled = false;
                                }
                            }


                        }
                        else
                        {
                            if (intApplication_Process_ID == 0)
                                ddlAction.SelectedValue = "0";
                        }
                        if (txtStatus.Text.ToUpper().Trim() == "APPROVED" && ddlAction.SelectedValue.Trim() == "0")
                        {
                            DataTable dttemp = new DataTable();
                            dttemp = null;
                            grvApprovalDetails.DataSource = dttemp;
                            grvApprovalDetails.DataBind();
                            btnSave.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alerts", "alert('This Application Number already approved by another user');", true);
                            return;
                        }
                        if (txtStatus.Text.ToUpper().Trim() == "REJECTED" && ViewState["strCreate"].ToString() == "1")
                        {
                            if (txtStatus.Text.ToUpper().Trim() == "REJECTED" && ddlAction.SelectedValue.Trim() == "0")
                            {

                                Label lbldate2 = (Label)e.Row.FindControl("lblApprovalDate");
                                if (lbldate2.Text.Trim() != string.Empty)
                                {
                                    DateTime Date = Convert.ToDateTime(lbldate2.Text);
                                    lbldate2.Text = Date.ToString(strDateFormat);
                                }
                                btnSave.Enabled = true;
                                return;
                            }

                        }
                        if (txtStatus.Text.ToUpper().Trim() == "REJECTED" && ddlAction.SelectedValue.Trim() == "0")
                        {
                            DataTable dttemp = new DataTable();
                            dttemp = null;
                            grvApprovalDetails.DataSource = dttemp;
                            grvApprovalDetails.DataBind();
                            btnSave.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alerts", "alert('This Application Number already rejected by another user');", true);
                            return;
                        }
                    }
                }
                if (Request.QueryString["qsMode"].Trim() == "Q")
                {
                    txtRemarks.Enabled = true;
                    txtRemarks.ReadOnly = true;
                }
                if ((ddlAction.SelectedValue.Trim() != "0") && PageMode == PageModes.WorkFlow)
                {
                    btnSave.Enabled = false;
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
            Label lbldate = (Label)e.Row.FindControl("lblApprovalDate");
            if (lbldate.Text.Trim() != string.Empty)
            {
                DateTime Date = Convert.ToDateTime(lbldate.Text);
                lbldate.Text = Date.ToString(strDateFormat);
            }
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public void FunPubClear()
    {
        try
        {
            grvApprovalDetails.DataSource = dsApplicationNumberDetails.Tables.Add();  //make empty grid
            grvApprovalDetails.DataBind();

            PaymentReqID.Visible = false;
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            btnClear.Enabled = false;
            txtStatus.Text = string.Empty;
            txtOfferDate.Text = string.Empty;

            S3GCustomerPermAddress.ClearCustomerDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion
    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {
            if (ddlApplicationNumber.SelectedValue != "0")
            {

                TextBox txtPassword = new TextBox();
                foreach (GridViewRow grv in grvApprovalDetails.Rows)
                {
                    txtPassword = (TextBox)grv.FindControl("txtPassword");
                }
                if (txtPassword.Text.Trim() != "")
                {
                    strPassword = txtPassword.Text.Trim();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Password.');", true);
                    return;
                }

                if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                    return;
                }

                ObjApplicationApproval = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
                int intResult = ObjApplicationApproval.FunPubRevokeOrUpdateApprovedDetails(2, Convert.ToInt32(ddlApplicationNumber.SelectedValue), intUserID, strPassword.Trim());
                if (intResult == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Application Number revoked successfully');window.location.href='../Origination/S3GORGTransLander.aspx?Code=APAP&mODIFY=0';", true);
                    return;
                }
                else if (intResult == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Application Number mapped to the transaction. cannot revoke this application number');", true);
                    return;
                }
                else if (intResult == 2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                    return;
                }
                else if (intResult == 7)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('No access rights for this User.');", true);
                    return;
                }
                else if (intResult == 8)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Next level approval has been completed, cannot revoke');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Please select application number');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
}
