using S3GBusEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity.FundManagement;


public partial class Fund_Management_S3G_FundMgt_NoteCreationApproval : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    int intNoteApproval_ID = 0;
    int intUserID = 0;
    int intRow = 0;
    string strCreate = string.Empty;
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    SerializationMode SerMode = SerializationMode.Binary;
    DataTable dtAction = new DataTable();
    DataSet dsNotedtls = new DataSet();
    DataSet Ds = new DataSet();
    string strDateFormat;
    string strPassword;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    FunderMgtServiceReference.FundMgtServiceClient objFundMgtServiceClient;
    FundMgtServices.S3G_ORG_NOTEAPPROVALDataTable objNoteDatatable;
    FundMgtServices.S3G_ORG_NOTEAPPROVALRow objNoteRow;
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    string strPageName = "Note Creation Approval";
    public static Fund_Management_S3G_FundMgt_NoteCreationApproval obj_Page;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        ProgramCode = "291";
        obj_Page = this;
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            intNoteApproval_ID = Convert.ToInt32(fromTicket.Name);
        }
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (!IsPostBack)
        {
            lblHeading.Text = "Note Creation Approval";
            if (intNoteApproval_ID > 0)
            {
                FunPriEnableDisableControls();
                FunPubViewApprovedDetails();
            }
            if (Request.QueryString["qsViewId"] == null)
                chkUnapproval.Checked = true;
            if (PageMode == PageModes.Query)
            {
                trCkbox.Visible = btnRevoke.Visible = false;
            }
            btnClear.Enabled = btnGo.Enabled = btnSave.Enabled = btnRevoke.Enabled = false;
            noteid.Visible = true;
            ViewState["strCreate"] = "0";
        }
    }

    protected void chkUnapproval_CheckedChanged(object sender, EventArgs e)
    {
        FunPubClear();
        ddlNoteNumber.Clear();
        ddlCustomer.Clear();
        ddlFunder.Clear();
        ddlTranche.Clear();
        btnGo.Enabled = false;
        txtnote.Text = "";
        hdnID.Value = "";
    }

    protected void chkApproved_CheckedChanged(object sender, EventArgs e)
    {
        FunPubClear();
        ddlNoteNumber.Clear();
        ddlCustomer.Clear();
        ddlFunder.Clear();
        ddlTranche.Clear();
        btnGo.Enabled = false;
        txtnote.Text = "";
        hdnID.Value = "";
    }

    #region "DropDown Events"

    protected void ddlNoteNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriShowDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlTranche_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriShowDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlCustomer_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            ddlNoteNumber.Clear();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void ddlFunder_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            ddlNoteNumber.Clear();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    #endregion

    private void FunPriShowDetails()
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Note_ID", ddlNoteNumber.SelectedValue.ToString());
            dictDropdownListParam.Add("@Tranche_Id", ddlTranche.SelectedValue.ToString());
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictDropdownListParam.Add("@OPTION", "1");
            dsNotedtls = Utility.GetDataset("S3G_ORG_GetNoteDtls_Approval", dictDropdownListParam);
            if (dsNotedtls.Tables[0].Rows.Count > 0)
            {
                txtStatus.Text = dsNotedtls.Tables[0].Rows[0]["status"].ToString();
                txtnote.Text = dsNotedtls.Tables[0].Rows[0]["Note_Creation_Date"].ToString();
                ddlFunder.SelectedText = dsNotedtls.Tables[0].Rows[0]["funder"].ToString();
                ddlFunder.SelectedValue = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Funder_ID"]);
                ddlCustomer.SelectedText = dsNotedtls.Tables[0].Rows[0]["customer"].ToString();
                ddlCustomer.SelectedValue = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Customer_ID"]);

                ddlNoteNumber.SelectedText = dsNotedtls.Tables[0].Rows[0]["Note_Number"].ToString();
                ddlNoteNumber.SelectedValue = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Note_Header_ID"]);

                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlNoteNumber.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../Fund Management/S3G_FundMgt_NoteCreation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";
                btnGo.Enabled = true;

            }
            ViewState["dtAction"] = dsNotedtls.Tables[1];
            ddlFunder.ReadOnly = ddlCustomer.ReadOnly = true;
        }
        catch (Exception objException)
        {
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

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        ApprovalMgtServicesReference.ApprovalMgtServicesClient ObjApproval = null;
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
                dictDropdownListParam.Add("@OPTIONS", "10");
                dictDropdownListParam.Add("@TASK_TYPE", "10");
                dictDropdownListParam.Add("@TASK_NUMBER", ddlNoteNumber.SelectedValue.ToString());
                dictDropdownListParam.Add("@USER_ID", ObjUserInfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Password", txtPassword.Text.Trim());

                int ErrCode = ObjApproval.FunPubRevokeOrUpdateApprovedDetails(dictDropdownListParam);

                switch (ErrCode)
                {
                    case 0:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Note Revoked Successfully.');window.location.href='../Fund Management/S3G_Fund_Translander.aspx?Code=APNOT';", true);
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
                    case 12:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Billing has been Processed. Cannot Revoke the Note!');", true);
                            return;
                        }
                    //Added by Sathiyanathan on 18Sep2015 to check Transfer records are disposed/not - Ends Here
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

    private void FunPriGoClick()
    {
        try
        {
            //if (ddlMRANumber.SelectedIndex > 0)
            if (ddlNoteNumber.SelectedValue != "0")
            {
                uinfo = new UserInfo();
                dictDropdownListParam = new Dictionary<string, string>();

                if (chkApproved.Checked)
                {
                    dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                    dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                    dictDropdownListParam.Add("@Task_Number", ddlNoteNumber.SelectedValue.ToString());
                    dictDropdownListParam.Add("@Task_Type", "291");

                    dsNotedtls = Utility.GetDataset("S3G_LOANAD_CheckApprovalRevoke", dictDropdownListParam);
                    if (dsNotedtls != null && dsNotedtls.Tables[0].Rows.Count > 0)
                    {
                        ViewState["AppStatus"] = dsNotedtls.Tables[0].Rows[0]["Action_ID"].ToString();
                        ViewState["dsNotedtls"] = dsNotedtls;
                        grvApprovalDetails.DataSource = dsNotedtls.Tables[0];
                        grvApprovalDetails.DataBind();
                        btnGo.Enabled = false;
                        btnRevoke.Enabled = true;
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('User does not have sufficient privilege to revoke.');", true);
                        return;
                    }
                }
                else
                {
                    ViewState["AppStatus"] = "0";
                    dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                    dictDropdownListParam.Add("@Note_ID", ddlNoteNumber.SelectedValue.ToString());
                    dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                    dictDropdownListParam.Add("@OPTION", "1");
                    dsNotedtls = Utility.GetDataset("S3G_ORG_GetNoteDtls_Approval", dictDropdownListParam);

                    ViewState["dsNotedtls"] = dsNotedtls;
                    btnSave.Enabled = true;
                    //btnRevoke.Enabled = true;
                    btnClear.Enabled = true;
                    btnClear.Enabled = true;

                    if (dsNotedtls.Tables[1].Rows.Count > 0)
                    {
                        grvApprovalDetails.DataSource = dsNotedtls.Tables[1];
                        grvApprovalDetails.DataBind();
                        btnGo.Enabled = false;
                    }
                }
            }
        }
        catch (Exception objException)
        {
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
                dsNotedtls = (DataSet)ViewState["dsNotedtls"];
                ddlAction.FillDataTable(dsNotedtls.Tables[2], "ID", "Name");
                if (Request.QueryString["qsMode"].Trim() == "Q")
                {
                    ddlAction.FillDataTable(dsNotedtls.Tables[2], "ID", "Name");
                    txtRemarks.ReadOnly = true;
                    grvApprovalDetails.Columns[3].Visible = false;
                    ddlAction.SelectedIndex = 1;
                    ddlAction.ClearDropDownList();
                }

                if (!string.IsNullOrEmpty(txtStatus.Text) && chkApproved.Checked)
                {
                    ddlAction.SelectedValue = ViewState["AppStatus"].ToString();
                    if (ddlAction.SelectedValue != "0")
                    {
                        ddlAction.ClearDropDownList();
                    }
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

    private void FunPriEnableDisableControls()
    {
        try
        {
            txtStatus.ReadOnly = txtnote.ReadOnly = true;
            btnSave.Enabled = btnClear.Enabled = btnGo.Enabled = ddlFunder.Enabled = ddlTranche.Enabled = ddlNoteNumber.Enabled = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void NoteIdID_serverclick(object sender, EventArgs e)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Note number.');", true);
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        FunSaveNoteApproval();
    }

    private void FunSaveNoteApproval()
    {
        try
        {
            Label lblApprovalSNO = new Label();
            Label lblNoteApproval_ID = new Label();
            TextBox txtRemarks = new TextBox();
            TextBox txtPassword = new TextBox();
            DropDownList ddlAction = new DropDownList();
            Label lblApprovalDate = new Label();
            foreach (GridViewRow grv in grvApprovalDetails.Rows)
            {
                lblApprovalSNO = (Label)grv.FindControl("lblApprovalSNO");
                lblNoteApproval_ID = (Label)grv.FindControl("lblNoteApproval_ID");
                ddlAction = (DropDownList)grv.FindControl("ddlAction");
                txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                txtPassword = (TextBox)grv.FindControl("txtPassword");
                lblApprovalDate = (Label)grv.FindControl("lblApprovalDate");
            }

            objNoteDatatable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_ORG_NOTEAPPROVALDataTable();
            objNoteRow = objNoteDatatable.NewS3G_ORG_NOTEAPPROVALRow();
            objNoteRow.NOTE_ID = Convert.ToInt32(ddlNoteNumber.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(lblNoteApproval_ID.Text))
                objNoteRow.NOTEApproval_ID = Convert.ToInt32(lblNoteApproval_ID.Text);
            else
                objNoteRow.NOTEApproval_ID = 0;
            if (!string.IsNullOrEmpty(lblApprovalSNO.Text.Trim()))
                objNoteRow.Approval_Serial_Number = Convert.ToInt32(lblApprovalSNO.Text.Trim());
            if (ddlAction.SelectedIndex > 0)
                objNoteRow.Action_ID = Convert.ToInt32(ddlAction.SelectedValue.ToString());
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Status');", true);
                return;
            }
            objNoteRow.Approver_ID = intUserID;
            objNoteRow.Approval_Date = Utility.StringToDate(lblApprovalDate.Text);
            objNoteRow.Remarks = Convert.ToString(txtRemarks.Text).Trim();
            objNoteRow.Company_ID = intCompanyID;

            objNoteRow.Created_By = Convert.ToInt32(ddlTranche.SelectedValue);

            objNoteDatatable.AddS3G_ORG_NOTEAPPROVALRow(objNoteRow);
            objFundMgtServiceClient = new FunderMgtServiceReference.FundMgtServiceClient();
            string strNoteApprovalNumber = "";
            Int64 intNoteApproval_No = 0;

            int intErrorCode = objFundMgtServiceClient.FunPubInsertNoteApproval(out strNoteApprovalNumber, out intNoteApproval_No, SerMode, ClsPubSerialize.Serialize(objNoteDatatable, SerMode));
            if (intErrorCode == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Note Approval done successfully.');window.location.href='../Fund Management/S3G_Fund_Translander.aspx?Code=APNOT&Modify=0';", true);
            }
            else if (intErrorCode == 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorization Rule Card not yet defined');", true);
                return;
            }
            else if (intErrorCode == 15)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Approval dont have access to approved');", true);
                return;
            }
            else if (intErrorCode == 20)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + strNoteApprovalNumber + "');", true);
                return;
            }
            else if (intErrorCode == 25)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Rental Schedule Not Yet Activated…!');", true);
                return;
            }
            else if (intErrorCode == 24)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Billing Has been Run for Backdated Entries');", true);
                return;
            }
            else if (intErrorCode == 30)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Sanction Limit Exceeds…!');", true);
                return;
            }
            else if (intErrorCode == 50)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Error in Note Approval.');", true);
                return;
            }
            else if (intErrorCode == 55)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Error in Note Approval.');", true);
                return;
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
            Label lblNoteApproval_ID = new Label();
            TextBox txtRemarks = new TextBox();
            TextBox txtPassword = new TextBox();
            DropDownList ddlAction = new DropDownList();
            Label lblApprovalDate = new Label();
            foreach (GridViewRow grv in grvApprovalDetails.Rows)
            {
                lblApprovalSNO = (Label)grv.FindControl("lblApprovalSNO");
                lblNoteApproval_ID = (Label)grv.FindControl("lblNoteApproval_ID");
                ddlAction = (DropDownList)grv.FindControl("ddlAction");
                txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                txtPassword = (TextBox)grv.FindControl("txtPassword");
                lblApprovalDate = (Label)grv.FindControl("lblApprovalDate");
            }
            
            if (ddlAction.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Status');", true);
                return;
            }

            if (Convert.ToString(txtPassword.Text) == "")
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
            string strAlert = "";
            if (ddlAction.SelectedValue.ToString() == "4")
            {
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@NOTE_ID", ddlNoteNumber.SelectedValue.ToString());
                string strCheck = Utility.GetTableScalarValue("S3G_ORG_CHECK_NOTEAPPROVAL", dictDropdownListParam);

                if (strCheck == "1")
                {
                    strAlert = "Billing Has been Run for Backdated Entries";
                    strAlert += @"\n\nWould you like to Continue?";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "if(confirm('" + strAlert + "')){ document.getElementById('" + Button1.ClientID + "').click();}else {}", true);
                }
                else if (strCheck == "2")
                {
                    Utility.FunShowAlertMsg(this.Page, "Rental Schedule details has been changed after Note creation, Re-arrive the Tranche and proceed.");
                    return;
                }
                else if (strCheck == "3")
                {
                    Utility.FunShowAlertMsg(this.Page, "Tranche details has been changed after Note creation. Re-arrive the Note and proceed.");
                    return;
                }
                else
                {
                    FunSaveNoteApproval();
                }
            }
            else
            {
                FunSaveNoteApproval();
            }

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    public void FunPubViewApprovedDetails()
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@OPTION", "2");
            dictDropdownListParam.Add("@Note_Approval_ID", Convert.ToString(intNoteApproval_ID));
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            DataSet dsNotedtls = Utility.GetDataset("S3G_ORG_GetNoteDtls_Approval", dictDropdownListParam);
            ViewState["dsNotedtls"] = dsNotedtls;
            if (dsNotedtls != null)
            {
                txtStatus.Text = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Status"]);
                txtnote.Text = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Note_Date"]);
                ddlNoteNumber.SelectedValue = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Note_Header_ID"]);
                ddlNoteNumber.SelectedText = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Note_Number"]);
                ddlFunder.SelectedText = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["funder"]);
                ddlCustomer.SelectedText = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["customer"]);
                ddlFunder.SelectedValue = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["funder"]);
                ddlCustomer.SelectedValue = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["customer"]);
                ddlTranche.SelectedValue = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Tranche_Id"]);
                ddlTranche.SelectedText = Convert.ToString(dsNotedtls.Tables[0].Rows[0]["Tranche_Name"]);

                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlNoteNumber.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../Fund Management/S3G_FundMgt_NoteCreation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

                grvApprovalDetails.DataSource = dsNotedtls.Tables[1];
                grvApprovalDetails.DataBind();

                noteid.Visible = true;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Fund management/S3G_Fund_Translander.aspx?Code=APNOT&Modify=0");
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
            ddlNoteNumber.Clear();
            ddlCustomer.Clear();
            ddlFunder.Clear();
            ddlTranche.Clear();
            btnGo.Enabled = false;
            txtnote.Text = "";
            hdnID.Value = "";
            // FunPubloadCombo();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }

    }

    public void FunPubClear()
    {
        try
        {
            grvApprovalDetails.DataSource = dsNotedtls.Tables.Add();  //make empty grid
            grvApprovalDetails.DataBind();

            hdnID.Visible = btnSave.Enabled = btnRevoke.Enabled = btnClear.Enabled = ddlFunder.ReadOnly = ddlCustomer.ReadOnly = false;
            txtStatus.Text = string.Empty;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    #region "WEB METHODS"

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheNumbers(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Program_id", "285");
        if (obj_Page.chkApproved.Checked)
            Procparam.Add("@Approved", "1");

        if (Convert.ToInt32(obj_Page.ddlFunder.SelectedValue) > 0 && Convert.ToString(obj_Page.ddlFunder.SelectedText) != "")
            Procparam.Add("@Funder_ID", Convert.ToString(obj_Page.ddlFunder.SelectedValue));
        if (Convert.ToInt32(obj_Page.ddlCustomer.SelectedValue) > 0 && Convert.ToString(obj_Page.ddlCustomer.SelectedText) != "")
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ddlCustomer.SelectedValue));

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetMRANumber_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetNoteNumbers(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Program_id", "291");
        if (obj_Page.chkApproved.Checked)
            Procparam.Add("@Approved", "1");

        if (Convert.ToInt32(obj_Page.ddlFunder.SelectedValue) > 0 && Convert.ToString(obj_Page.ddlFunder.SelectedText) != "")
            Procparam.Add("@Funder_ID", Convert.ToString(obj_Page.ddlFunder.SelectedValue));
        if (Convert.ToInt32(obj_Page.ddlCustomer.SelectedValue) > 0 && Convert.ToString(obj_Page.ddlCustomer.SelectedText) != "")
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ddlCustomer.SelectedValue));

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetMRANumber_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetFunderLst(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@Option", "1");
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        if (Convert.ToInt32(obj_Page.ddlCustomer.SelectedValue) > 0 && Convert.ToString(obj_Page.ddlCustomer.SelectedText) != "")
            Procparam.Add("@Customer_ID", Convert.ToString(obj_Page.ddlCustomer.SelectedValue));

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_FMGT_GETNOTECMNLST", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerLst(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@Option", "2");
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        if (Convert.ToInt32(obj_Page.ddlFunder.SelectedValue) > 0 && Convert.ToString(obj_Page.ddlFunder.SelectedText) != "")
            Procparam.Add("@Funder_ID", Convert.ToString(obj_Page.ddlFunder.SelectedValue));
        
        //Call Id : 4560
        if (obj_Page.chkApproved.Checked)
            Procparam.Add("@Approved", "1");

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_FMGT_GETNOTECMNLST", Procparam));

        return suggetions.ToArray();
    }

    #endregion

}