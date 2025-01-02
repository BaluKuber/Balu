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


public partial class S3GLoanAdManualJournalApproval_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    int intVoucher_No = 0;
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
    public static S3GLoanAdManualJournalApproval_Add obj_Page;
    ApprovalMgtServicesReference.ApprovalMgtServicesClient ObjApproval = null;
    S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable ObjApproval_DataTable = new S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable();
    #endregion
    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            intVoucher_No = Convert.ToInt32(fromTicket.Name);
        }
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (!IsPostBack)
        {
            ReqID.Visible = false;

            if (intVoucher_No > 0)
            {
                FunPubViewApprovedDetails();
                btnClear.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
            }

            if (PageMode == PageModes.Create)
            {
                PubFunBind();
            }
            if (PageMode == PageModes.Query)
            {
                btnRevoke.Visible = false;
                btnClear.Enabled = trCkbox.Visible = false;
            }

            if (Request.QueryString["qsViewId"] == null)
            {
                chkUnapproval.Checked = true;
                FunProFillUnApprovedDetails();
            }

            ddllLineOfBusiness.Focus();
        }
    }
    public void FunPubViewApprovedDetails()
    {
        btnGo.Enabled = false;
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Task_Type", "29");//Account Revision Specific APPROVAL SCREEN LOOKUPCODE IS 29
        dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
        dictDropdownListParam.Add("@Task_Number", intVoucher_No.ToString());
        dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
        if (dsapproval.Tables[3].Rows.Count > 0)
        {
            txtStatus.Text = dsapproval.Tables[3].Rows[0]["Status"].ToString();
            txtMJVDate.Text = DateTime.Parse(dsapproval.Tables[3].Rows[0]["Manual_Journal_Voucher_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            txtvaluedate.Text = DateTime.Parse(dsapproval.Tables[3].Rows[0]["Value_date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            //Performance Issue Added By Shibu 
            //ddlMJVNO.FillDataTable(dsapproval.Tables[3], "MANUAL_JOURNAL_VOUCHER_ID", "Manual_Journal_Voucher_No");
            //ddlMJVNO.Items.Add(new ListItem(dsapproval.Tables[3].Rows[0]["MANUAL_JOURNAL_VOUCHER_ID"].ToString(),dsapproval.Tables[3].Rows[0]["MANUAL_JOURNAL_VOUCHER_ID"].ToString()));
            ddlMJVNO.SelectedText = dsapproval.Tables[3].Rows[0]["MANUAL_JOURNAL_VOUCHER_No"].ToString();
            ddlMJVNO.SelectedValue = dsapproval.Tables[3].Rows[0]["MANUAL_JOURNAL_VOUCHER_ID"].ToString();
            ddlMJVNO.ToolTip = dsapproval.Tables[3].Rows[0]["MANUAL_JOURNAL_VOUCHER_ID"].ToString();
            ddlMJVNO.Enabled = false;

            //ddllLineOfBusiness.FillDataTable(dsapproval.Tables[3], "LOB_ID", "LOB_Name");
            ddllLineOfBusiness.Items.Add(new ListItem(dsapproval.Tables[3].Rows[0]["LOB_Name"].ToString(), dsapproval.Tables[3].Rows[0]["LOB_ID"].ToString()));
            ddllLineOfBusiness.ToolTip = dsapproval.Tables[3].Rows[0]["LOB_Name"].ToString();
            ddllLineOfBusiness.Enabled = false;

            // ddlBranch.FillDataTable(dsapproval.Tables[3], "Location_ID", "Location_Name");
            ddlBranch.SelectedText = dsapproval.Tables[3].Rows[0]["Location_Name"].ToString();
            ddlBranch.SelectedValue = dsapproval.Tables[3].Rows[0]["Location_ID"].ToString();
            ddlBranch.ToolTip = dsapproval.Tables[3].Rows[0]["Location_Name"].ToString();

            ddlBranch.Enabled = false;

            txtStatus.ReadOnly = true;
            txtMJVDate.ReadOnly = true;
            txtvaluedate.ReadOnly = true;


            btnGo.Enabled = false;

            ReqID.Visible = true;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlMJVNO.SelectedValue.ToString(), false, 0);
            hdnID.Value = "../LoanAdmin/S3GLoanAdManualJournal_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

            Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);

            grvApprovalDetails.DataSource = dsapproval.Tables[3];
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
            //ObjRow.Task_Number = ddlMJVNO.SelectedItem.Text.ToString();//stored varchar in approval table for mjvno.
            ObjRow.Task_Number = ddlMJVNO.SelectedText.ToString();//stored varchar in approval table for mjvno.
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('please select the status');", true);
                return;
            }

            //S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                return;
            }

            ObjRow.Task_Type = "29";
            ObjRow.Password = txtPassword.Text.Trim();
            ObjRow.Remarks = txtRemarks.Text.Trim();
            ObjRow.Company_ID = intCompanyID.ToString();
            ObjRow.Created_By = intUserID.ToString();
            ObjApproval_DataTable.AddS3G_LOANAD_ApprovalRow(ObjRow);
            //ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();
            string strErrMsg = string.Empty;
            int intErrorCode = ObjApproval.FunPubCreateApprovals(out strErrMsg, SerMode, ClsPubSerialize.Serialize(ObjApproval_DataTable, SerMode));
            if (intErrorCode == 0)
            {
                if (ddlstatus.SelectedValue == "3")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Manual Journal Approved successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=MJA';", true);
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Manual Journal Rejected successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=MJA';", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Manual Journal details Added failed.');", true);
                return;
            }
            else
            {
                Utility.FunShowValidationMsg(this.Page, "MJVA", intErrorCode, strErrMsg, false);
                return;
            }
        }

        catch (Exception ex)
        {
            //throw;
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
        //ddlBranch.SelectedIndex = 0;
        ddlBranch.Clear();
        if (ddllLineOfBusiness.SelectedIndex > 0)
            ddllLineOfBusiness.SelectedIndex = 0;
        //ddlMJVNO.Items.Clear();
        ddlMJVNO.Clear();
        FunPubClear();
        ddllLineOfBusiness.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=MJA");
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
        dictDropdownListParam.Add("@Program_ID", "81");
        ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });

        if (ddllLineOfBusiness.Items.Count == 2)
        {
            ddllLineOfBusiness.SelectedIndex = 1;
            ddllLineOfBusiness.ClearDropDownList();
        }

        if (PageMode == PageModes.Query)
        {
            // dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            // ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
        }
        uinfo = null;
        dictDropdownListParam = null;
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedValue != "0")
        {

            FunPubClear();
            btnSave.Enabled = false;
            //ddlMJVNO.Items.Clear();
            ddlMJVNO.Clear();
            uinfo = new UserInfo();
            //dictDropdownListParam = new Dictionary<string, string>();
            //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            //dictDropdownListParam.Add("@Task_Type", "29");
            //if (chkApproved.Checked)
            //{
            //    dictDropdownListParam.Add("@UnApproved", "1");
            //}
            //else
            //{
            //    dictDropdownListParam.Add("@UnApproved", "0");
            //}
            //ddlMJVNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Manual_Journal_Voucher_ID", "Manual_Journal_Voucher_No" });
            //dictDropdownListParam = null;
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;
            //ddlMJVNO.Items.Clear();
            ddlMJVNO.Clear();
        }

        ddlBranch.Focus();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetMJV(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", obj_Page.uinfo.ProCompanyIdRW.ToString());
        Procparam.Add("@User_Id", Convert.ToString(obj_Page.intUserID));
        Procparam.Add("@LOB_ID", obj_Page.ddllLineOfBusiness.SelectedValue.ToString());
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue.ToString());
        Procparam.Add("@Task_Type", "29");
        if (obj_Page.chkApproved.Checked)
        {
            Procparam.Add("@UnApproved", "1");
        }
        else
        {
            Procparam.Add("@UnApproved", "0");
        }

        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetApprovalReqDetail_AGT", Procparam));

        return suggetions.ToArray();
    }

    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Dictionary<string, string> dictDropdownListParam;
        //dictDropdownListParam = new Dictionary<string, string>();
        //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //dictDropdownListParam.Add("@Is_Active", "1");
        //dictDropdownListParam.Add("@Program_ID", "81");
        //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });

        //if (ddllLineOfBusiness.SelectedIndex > 0)
        //{
        //    FunPubClear();
        //    btnSave.Enabled = false;
        //    btnGo.Enabled = true;
        //    ddlMJVNO.Items.Clear();
        //    uinfo = new UserInfo();
        //    dictDropdownListParam = new Dictionary<string, string>();
        //    dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
        //    dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //    dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        //    dictDropdownListParam.Add("@Branch_ID", ddlBranch.SelectedValue.ToString());
        //    dictDropdownListParam.Add("@Task_Type", "29");
        //    if (chkApproved.Checked)
        //    {
        //        dictDropdownListParam.Add("@UnApproved", "1");
        //    }
        //    else
        //    {
        //        dictDropdownListParam.Add("@UnApproved", "0");
        //    }
        //    ddlMJVNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Manual_Journal_Voucher_ID", "Manual_Journal_Voucher_No" });
        //    dictDropdownListParam = null;

        //}
        //else
        //{

        ddlBranch.SelectedValue = "0";
        FunPubClear();
        btnSave.Enabled = false;
        //ddlMJVNO.Items.Clear();
        ddlMJVNO.Clear();
        ddllLineOfBusiness.Focus();
        //}
    }
    protected void ddlMJVNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlMJVNO.SelectedIndex > 0)
        if (ddlMJVNO.SelectedValue != "0")
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;

            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Task_Type", "29");
            dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
            dictDropdownListParam.Add("@Task_Number", ddlMJVNO.SelectedValue.ToString());
            dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
            if (dsapproval.Tables[1].Rows.Count > 0)
            {
                txtStatus.Text = dsapproval.Tables[1].Rows[0]["Status"].ToString();
                txtMJVDate.Text = DateTime.Parse(dsapproval.Tables[1].Rows[0]["Manual_Journal_Voucher_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtvaluedate.Text = DateTime.Parse(dsapproval.Tables[1].Rows[0]["Value_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                ReqID.Visible = true;
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlMJVNO.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../LoanAdmin/S3GLoanAdManualJournal_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

                txtStatus.ReadOnly = true;
                txtMJVDate.ReadOnly = true;
                txtvaluedate.ReadOnly = true;

                btnGo.Enabled = true;
            }
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;
        }

        ddlMJVNO.Focus();
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //if (ddlMJVNO.SelectedIndex > 0)
        if (ddlMJVNO.SelectedValue != "0")
        {
            btnSave.Enabled = true;
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@Task_Number", ddlMJVNO.SelectedText.ToString());
            dictDropdownListParam.Add("@Task_Type", "29");
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
            DropDownList ddlstatus = (DropDownList)e.Row.FindControl("ddlstatus");
            ddlstatus.FillDataTable(Ds.Tables[1], "Lookup_Code", "Lookup_Description");
            if (intVoucher_No > 0)
            {
                if (dsapproval.Tables[3].Rows.Count > 0)
                {
                    grvApprovalDetails.Columns[3].Visible = false;
                    ddlstatus.SelectedValue = dsapproval.Tables[3].Rows[intRow]["Status_ID"].ToString();
                    //txtRemarks.Enabled = false;
                    txtRemarks.ReadOnly = true;
                    ddlstatus.Enabled = false;
                    btnSave.Enabled = false;

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

            ddlstatus.Focus();
        }
    }
    public void FunPubClear()
    {
        grvApprovalDetails.DataSource = dsapproval.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();

        ReqID.Visible = false;
        btnSave.Enabled = false;
        btnGo.Enabled = false;
        btnRevoke.Enabled = false;
        btnClear.Enabled = true;

        txtStatus.Text = string.Empty;
        txtMJVDate.Text = string.Empty;
        txtvaluedate.Text = string.Empty;
        txtStatus.Text = string.Empty;
    }
    #endregion

    protected void FunProFillUnApprovedDetails()
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
            dictDropdownListParam.Add("@Program_ID", "81");
            ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            if (ddllLineOfBusiness.Items.Count == 2)
            {
                ddllLineOfBusiness.SelectedIndex = 1;
                ddllLineOfBusiness.ClearDropDownList();
            }
            if (PageMode == PageModes.Query)
            {
                //  dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            }
            //ddlMJVNO.Items.Clear();
            //ddlMJVNO.Items.Insert(0, "--Select--");
            ddlMJVNO.Clear();
            uinfo = null;
            dictDropdownListParam = null;
        }
        if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
        {
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            //btnClear.Enabled = false;

            ddllLineOfBusiness.Items.Clear();
            ddlBranch.Clear();
            //ddlMJVNO.Items.Clear();
            ddlMJVNO.Clear();

            FunPubClear();
            //FunPubloadCombo();
        }
    }

    protected void chkUnapproval_CheckedChanged(object sender, EventArgs e)
    {
        FunProFillUnApprovedDetails();
    }

    protected void chkApproved_CheckedChanged(object sender, EventArgs e)
    {
        if (chkApproved.Checked)
        {
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            //btnClear.Enabled = false;

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
            dictDropdownListParam.Add("@Program_ID", "81");
            ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            if (ddllLineOfBusiness.Items.Count == 2)
            {
                ddllLineOfBusiness.SelectedIndex = 1;
                ddllLineOfBusiness.ClearDropDownList();
            }
            if (PageMode == PageModes.Query)
            {
                //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            }
            //ddlMJVNO.Items.Clear();
            //ddlMJVNO.Items.Insert(0, "--Select--");
            ddlMJVNO.Clear();
            uinfo = null;
            dictDropdownListParam = null;
        }
        else
        {
            if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
            {


                ddllLineOfBusiness.Items.Clear();
                ddlBranch.Clear();
                //ddlMJVNO.Items.Clear();
                ddlMJVNO.Clear();

                FunPubClear();
                // FunPubloadCombo();
            }
        }

        ddllLineOfBusiness.Focus();
    }

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();
        try
        {
            //ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();

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
                dictDropdownListParam.Add("@OPTIONS", "2");
                dictDropdownListParam.Add("@TASK_TYPE", "29");
                dictDropdownListParam.Add("@TASK_NUMBER", ddlMJVNO.SelectedText);
                dictDropdownListParam.Add("@USER_ID", ObjUserInfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Password", txtPassword.Text.Trim());

                int ErrCode = ObjApproval.FunPubRevokeOrUpdateApprovedDetails(dictDropdownListParam);

                switch (ErrCode)
                {
                    case 0:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Manual Journal Approval details Revoked successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=MJA';", true);
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
            //throw;
        }
        finally
        {
            ObjApproval.Close();
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
        Procparam.Add("@Program_Id", "081");
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }


}
