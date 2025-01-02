/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// /// <Program Summary>
/// Created Date            :   19Sep2014
/// Reason                  :   Approve MRA Creation
/// <Program Summary>
/// 
using System;
using System.Web.Security;
using System.Globalization;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using S3GBusEntity;
using System.Configuration;

public partial class Origination_S3G_ORG_MRA_Approval : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    int intMRAApproval_ID = 0;
    int intUserID = 0;
    int intRow = 0;
    string strCreate = string.Empty;
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    SerializationMode SerMode = SerializationMode.Binary;
    DataTable dtAction = new DataTable();
    DataSet dsApplicationNumberDetails = new DataSet();
    DataSet Ds = new DataSet();
    string strDateFormat;
    string strPassword;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjMRAApproval;
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRAAPPROVALDataTable ObjMRAApproval_DataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRAAPPROVALDataTable();
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    string strPageName = "MRA Approval";
    public static Origination_S3G_ORG_MRA_Approval obj_Page;
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ProgramCode = "282";
            obj_Page = this;
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                intMRAApproval_ID = Convert.ToInt32(fromTicket.Name);
            }
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            if (!IsPostBack)
            {
                btnRevoke.Enabled = false;
                PaymentReqID.Visible = false;
                ViewState["strCreate"] = "0";
                //FunPubloadCombo();
                if (Request.QueryString["qsViewId"] == null)
                {
                    FunProLoadLOB();
                }
                //if (blnIsWorkflowApplicable && Session["DocumentNo"] != null)
                //{

                //}
                //else
                //{
                if (intMRAApproval_ID > 0)
                {
                    FunPriEnableDisableControls();
                    FunPubViewApprovedDetails();
                }
                else
                {
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    btnRevoke.Enabled = false;
                    btnGo.Enabled = false;
                }

                // WORK FLOW Implementation
                if (PageMode == PageModes.WorkFlow)
                {
                    PreparePageForWFLoad();
                }
                //}
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Label lblApprovalSNO = new Label();
            Label lblMRAApproval_ID = new Label();
            TextBox txtRemarks = new TextBox();
            TextBox txtPassword = new TextBox();
            DropDownList ddlAction = new DropDownList();
            Label lblApprovalDate = new Label();
            foreach (GridViewRow grv in grvApprovalDetails.Rows)
            {
                lblApprovalSNO = (Label)grv.FindControl("lblApprovalSNO");
                lblMRAApproval_ID = (Label)grv.FindControl("lblMRAApproval_ID");
                ddlAction = (DropDownList)grv.FindControl("ddlAction");
                txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                txtPassword = (TextBox)grv.FindControl("txtPassword");
                lblApprovalDate = (Label)grv.FindControl("lblApprovalDate");
            }
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRAAPPROVALRow ObjRow;

            ObjRow = ObjMRAApproval_DataTable.NewS3G_ORG_MRAAPPROVALRow();
            ObjRow.MRA_ID = Convert.ToInt32(ddlMRANumber.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(lblMRAApproval_ID.Text))
                ObjRow.MRAApproval_ID = Convert.ToInt32(lblMRAApproval_ID.Text);
            else
                ObjRow.MRAApproval_ID = 0;
            if (!string.IsNullOrEmpty(lblApprovalSNO.Text.Trim()))
                ObjRow.Approval_Serial_Number = Convert.ToInt32(lblApprovalSNO.Text.Trim());
            if (ddlAction.SelectedIndex > 0)
                ObjRow.Action_ID = Convert.ToInt32(ddlAction.SelectedValue.ToString());
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Status');", true);
                return;
            }
            ObjRow.Approver_ID = intUserID;
            ObjRow.LOB_ID = Convert.ToInt32(ddllLineOfBusiness.SelectedValue);
            if (Convert.ToString(txtPassword.Text) == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Password.');", true);
                return;
            }
            ObjRow.Approval_Date = Utility.StringToDate(lblApprovalDate.Text);

            S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                return;
            }
            ObjRow.Remarks = Convert.ToString(txtRemarks.Text).Trim();
            ObjRow.Company_ID = intCompanyID;
            ObjRow.Created_By = intUserID;
            ObjMRAApproval_DataTable.AddS3G_ORG_MRAAPPROVALRow(ObjRow);
            ObjMRAApproval = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            string strMRAApprovalNumber = "";
            Int64 intMRAApproval_No = 0;
            int intErrorCode = ObjMRAApproval.FunPubInsertMRAApproval(out strMRAApprovalNumber, out intMRAApproval_No, SerMode, ClsPubSerialize.Serialize(ObjMRAApproval_DataTable, SerMode));
            if (intErrorCode == 0)
            {
                string strAlert = "";
                if (isWorkFlowTraveler)
                {
                    if (intMRAApproval_No > 0)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        if (ddlAction.SelectedItem.Text == "Approved")
                        {
                            try
                            {
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, 2);

                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here

                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is Not Assigned"; // Closing the status...
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                            }
                        }
                        else if (ddlAction.SelectedItem.Text == "Rejected")       // In case of Approval Rejected or cancelled - By Rao 27 Jan 2012.
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
                        ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId.ToString(), strAlert);
                        return;

                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing Approval done successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=MAPP&Modify=0';", true);
                }
                else
                {
                    if (intMRAApproval_No > 0)
                    {
                        DataTable WFFP = new DataTable();
                        // By Palani Kumar.A for Customer Name remove after Concatenation
                        string strBusinessNo = string.Empty;
                        if (!string.IsNullOrEmpty(ddlMRANumber.SelectedText.ToString()))
                        {
                            strBusinessNo = ddlMRANumber.SelectedText.Substring(0, ddlMRANumber.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();
                        }
                        else
                        {
                            strBusinessNo = ddlMRANumber.SelectedText.ToString();
                        }

                        if (CheckForForcePullOperation(null, strBusinessNo, ProgramCode, null, null, "O", CompanyId, null, int.Parse(hProductID.Value), ddllLineOfBusiness.SelectedItem.Text, null, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            if (ddlAction.SelectedItem.Text == "Approved")
                            {
                                try
                                {
                                    int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), strBusinessNo, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), 2);

                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here
                                    strAlert = "";
                                }
                                catch (Exception Ex)
                                {
                                    strAlert = "Work Flow is Not Assigned"; // Closing the Status.
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());

                                }
                            }
                            else if (ddlAction.SelectedItem.Text == "Rejected")      // In case of Approval Rejected or cancelled - By Rao 27 Jan 2012.
                            {
                                try
                                {
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());

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
                        }
                    }

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing Approval done successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0';", true);
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    if (ddlAction.SelectedValue == "4")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('MRA Approved successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=MAPP&Modify=0';", true);
                    }
                    else if (ddlAction.SelectedValue == "3")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('MRA Rejected successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=MAPP&Modify=0';", true);
                    }
                }

                return;
            }
            else if (intErrorCode == 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorization Rule Card not yet defined');", true);
                return;
            }
            else if (intErrorCode == 15)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Approver does not have access to approve');", true);
                return;
            }
            else if (intErrorCode == 50)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Error in MRA Approval.');", true);
                return;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select business offer number.');", true);
                return;
            }
            //Response.Write(hdnID.Value.ToString());
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClear();
            ddlMRANumber.Clear();
            btnGo.Enabled = false;
            FunPubloadCombo();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
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
                Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=MAPP&Modify=0");
        }

        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlMRANumber.Clear();
            FunPubClear();
            if (ddllLineOfBusiness.SelectedIndex == 0)
            {
                btnSave.Enabled = btnClear.Enabled = btnRevoke.Enabled = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlMRANumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlMRANumber.SelectedValue != "0")
            {
                FunPubClear();
                btnSave.Enabled = false;
                btnRevoke.Enabled = false;
                btnClear.Enabled = false;
                FunPriShowCustomerDetails();
                ddlMRANumber.ToolTip = ddlMRANumber.SelectedText.ToString();
            }
            else
            {
                FunPubClear();
                btnSave.Enabled = false;
                btnRevoke.Enabled = false;
                btnClear.Enabled = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriGoClick();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
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
                ddlAction.FillDataTable(dsApplicationNumberDetails.Tables[3], "ID", "Name");
                if (Request.QueryString["qsMode"].Trim() == "Q")
                {
                    txtRemarks.ReadOnly = true;
                    grvApprovalDetails.Columns[3].Visible = false;
                    ddlAction.SelectedIndex = 1;
                    ddlAction.ClearDropDownList();
                }
                intRow++;                
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjApplicationApproval = null;
        ObjApplicationApproval = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();

        try
        {

            //if (ddlMRANumber.SelectedIndex > 0)
            if (ddlMRANumber.SelectedValue != "0")
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

                S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
                if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                    return;
                }

                //ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjApplicationApproval = null;
                ObjApplicationApproval = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
                int intResult = ObjApplicationApproval.FunPubRevokeOrUpdateApprovedDetails(1, Convert.ToInt32(ddlMRANumber.SelectedValue), intUserID, strPassword.Trim());
                if (intResult == 0)
                {
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnRevoke.Enabled = false;
                    //End here

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Business offer number revoked successfully');window.location.href='../Origination/S3GORGTransLander.aspx?Code=MAPP&Modify=0';", true);
                    return;
                }
                else if (intResult == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Business offer number mapped to the transaction. can not revoke this Business offer number');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Please select Business offer number');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjApplicationApproval.Close();
        }

    }

    #endregion

    #region METHODS

    private void PreparePageForWFLoad()
    {
        //WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

        //try
        //{

        //    WorkFlowSession WFSessionValue = new WorkFlowSession();
        //    //WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();
        //    byte[] byte_ToDoList = objWorkflowToDoClient.FunPubLoadPricingApproval(WFSessionValue.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValue.Document_Type);
        //    DataSet dsEnquiryAppraisal = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_ToDoList, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
        //    if (dsEnquiryAppraisal.Tables.Count > 0)
        //    {
        //        if (dsEnquiryAppraisal.Tables[0].Rows.Count > 0)
        //        {
        //            intMRAApproval_ID = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Pricing_Id"]);
        //            FunPriShowUnApprovedDetails();
        //            ddllLineOfBusiness.SelectedValue = Convert.ToString(dsEnquiryAppraisal.Tables[0].Rows[0]["LOB_Id"]);
        //            FunPriLoadBusinessNo();
        //            ddlMRANumber.SelectedValue = Convert.ToString(dsEnquiryAppraisal.Tables[0].Rows[0]["Pricing_ID"]);
        //            FunPriShowCustomerDetails();
        //        }
        //    }
        //    GoClick();
        //    if (ddllLineOfBusiness.SelectedIndex > 0) ddllLineOfBusiness.ClearDropDownList();
        //    if (ddlMRANumber.SelectedValue != "0") ddlMRANumber.ReadOnly = true;
        //}
        //catch (Exception ex)
        //{
        //    ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        //    throw new ApplicationException("Unable Load Pricing page");
        //}
        //finally
        //{
        //    objWorkflowToDoClient.Close();
        //}

    }

    public void FunPubloadCombo()
    {
        try
        {
            ddllLineOfBusiness.Items.Insert(0, "--Select--");
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    public void FunPubViewApprovedDetails()
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@OPTION", "2");
            dictDropdownListParam.Add("@MRA_Approval_ID", Convert.ToString(intMRAApproval_ID));
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            DataSet dsApproval = Utility.GetDataset("S3G_ORG_GetMRADtls_Approval", dictDropdownListParam);
            dsApplicationNumberDetails = dsApproval;
            if (dsApproval != null)
            {
                txtStatus.Text = Convert.ToString(dsApproval.Tables[0].Rows[0]["Status"]);
                txtCreationDate.Text = Convert.ToString(dsApproval.Tables[0].Rows[0]["MRA_Creation_Date"]);
                S3GCustomerPermAddress.SetCustomerDetails(dsApproval.Tables[1].Rows[0], true);

                ddlMRANumber.SelectedValue = Convert.ToString(dsApproval.Tables[0].Rows[0]["MRA_ID"]);
                ddlMRANumber.SelectedText = Convert.ToString(dsApproval.Tables[0].Rows[0]["MRA_Number"]);

                ddllLineOfBusiness.FillDataTable(dsApproval.Tables[0], "LOB_ID", "LOB_Name");
                ddllLineOfBusiness.SelectedValue = Convert.ToString(dsApproval.Tables[0].Rows[0]["LOB_ID"]);
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlMRANumber.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../Origination/S3G_ORG_MRA_ADD.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

                grvApprovalDetails.DataSource = dsApproval.Tables[2];
                grvApprovalDetails.DataBind();

                PaymentReqID.Visible = true;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnableDisableControls()
    {
        try
        {
            txtStatus.ReadOnly = txtCreationDate.ReadOnly = txtCreationDate.ReadOnly = true;
            ddlMRANumber.Enabled = ddllLineOfBusiness.Enabled = btnSave.Enabled = btnClear.Enabled = btnGo.Enabled = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunProLoadLOB()
    {
        try
        {
            Dictionary<string, string> dictDropdownListParam;
            dictDropdownListParam = new Dictionary<string, string>();
            uinfo = new UserInfo();
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            dictDropdownListParam.Add("@Is_Active", "1");
            dictDropdownListParam.Add("@Program_ID", "31");
            ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            if (ddllLineOfBusiness != null && ddllLineOfBusiness.Items.Count == 2)
            {
                ddllLineOfBusiness.SelectedIndex = 1;
                ddllLineOfBusiness.ClearDropDownList();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriShowUnApprovedDetails()
    {
        try
        {
            FunPubClear();
            //ddllLineOfBusiness.Items.Clear();
            ddllLineOfBusiness.SelectedValue = "0";
            //ddlMRANumber.Items.Clear();
            ddlMRANumber.Clear();
            //ddlBranch.Items.Clear();
            //ddlBranch.Clear();

            //UserInfo uinfo = null;
            //Dictionary<string, string> dictDropdownListParam;
            //dictDropdownListParam = new Dictionary<string, string>();
            //uinfo = new UserInfo();
            //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@Is_Active", "1");
            //dictDropdownListParam.Add("@Program_ID", "31");
            //ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            //uinfo = null;
            //dictDropdownListParam = null;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadBusinessNo()
    {
        try
        {
            FunPubClear();
            //ddlMRANumber.Items.Clear();
            ddlMRANumber.Clear();
            //uinfo = new UserInfo();
            //dictDropdownListParam = new Dictionary<string, string>();
            //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            //ddlMRANumber.BindDataTable("S3G_ORG_GetPricingOfferNumber ", dictDropdownListParam, new string[] { "Pricing_ID", "Business_Offer_Number" });
            //dictDropdownListParam = null;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriGoClick()
    {
        try
        {
            //if (ddlMRANumber.SelectedIndex > 0)
            if (ddlMRANumber.SelectedValue != "0")
            {
                uinfo = new UserInfo();
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@MRA_ID", ddlMRANumber.SelectedValue.ToString());
                dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictDropdownListParam.Add("@OPTION", "1");
                dsApplicationNumberDetails = Utility.GetDataset("S3G_ORG_GetMRADtls_Approval ", dictDropdownListParam);


                btnSave.Enabled = true;
                //btnRevoke.Enabled = true;
                btnClear.Enabled = true;
                btnClear.Enabled = true;

                if (dsApplicationNumberDetails.Tables[2].Rows.Count > 0)
                {
                    grvApprovalDetails.DataSource = dsApplicationNumberDetails.Tables[2];
                    grvApprovalDetails.DataBind();
                    btnGo.Enabled = false;
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
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
            txtCreationDate.Text = string.Empty;
            S3GCustomerPermAddress.ClearCustomerDetails();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriShowCustomerDetails()
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@MRA_ID", ddlMRANumber.SelectedValue.ToString());
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictDropdownListParam.Add("@OPTION", "1");
            DataSet dsMRADtls = Utility.GetDataset("S3G_ORG_GetMRADtls_Approval ", dictDropdownListParam);
            if (dsMRADtls.Tables[0].Rows.Count > 0)
            {
                txtStatus.Text = dsMRADtls.Tables[0].Rows[0]["Name"].ToString();
                txtCreationDate.Text = Convert.ToString(dsMRADtls.Tables[0].Rows[0]["MRA_Creation_Date"]);

                S3GCustomerPermAddress.SetCustomerDetails(dsMRADtls.Tables[1].Rows[0], true);

                txtStatus.ReadOnly = true;
                txtCreationDate.ReadOnly = true;

                PaymentReqID.Visible = true;
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlMRANumber.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../Origination/S3G_ORG_MRA_ADD.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";
                btnGo.Enabled = true;

            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    #endregion

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
        Procparam.Add("@Program_Id", "31");
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetMRANumbers(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@LOB_ID", obj_Page.ddllLineOfBusiness.SelectedValue.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetMRANumber_AGT", Procparam));

        return suggetions.ToArray();
    }

}

