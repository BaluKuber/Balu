/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// /// <Program Summary>
/// Last Updated By		      :   Thalaiselvam N
/// Last Updated Date         :   03-Sep-2011
/// Reason                    :   Encrypted Password Validation
/// <Program Summary>

using System;
using System.Globalization;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using S3GBusEntity;
using System.Web.Security;


public partial class S3GLoanAdAccountConsolidationApproval_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    string strConstitution_No = "";
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
    ApprovalMgtServicesReference.ApprovalMgtServicesClient ObjApproval = null;
    S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable ObjApproval_DataTable = new S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable();
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            strConstitution_No = fromTicket.Name.ToString();
            btnClear.Enabled = false;
        }
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (!IsPostBack)
        {
            ReqID.Visible = false;
            PubFunBind();
            if (strConstitution_No.Trim() != "")
            {
                FunPubViewApprovedDetails();

            }
            else
            {
                btnSave.Enabled = false;

            }
        }
    }

    public void FunPubViewApprovedDetails()
    {
        btnGo.Enabled = false;
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Task_Type", "30");//Account Revision Specific APPROVAL SCREEN LOOKUPCODE IS 28
        dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
        dictDropdownListParam.Add("@Task_Number", strConstitution_No.ToString());
        dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
        if (dsapproval.Tables[3].Rows.Count > 0)
        {
            txtStatus.Text = dsapproval.Tables[3].Rows[0]["Status"].ToString();
            txtDate.Text = DateTime.Parse(dsapproval.Tables[3].Rows[0]["Account_Consolidation_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            S3GCustomerPermAddress.SetCustomerDetails(dsapproval.Tables[3].Rows[0], true);

            ddlConsolidationNO.FillDataTable(dsapproval.Tables[3], "Consolidation_No", "Consolidation_No");
            ddlConsolidationNO.SelectedValue = dsapproval.Tables[3].Rows[0]["Consolidation_No"].ToString();
            ddlConsolidationNO.Enabled = false;

            ddllLineOfBusiness.FillDataTable(dsapproval.Tables[3], "LOB_ID", "LOB_Name");
            ddllLineOfBusiness.SelectedValue = dsapproval.Tables[3].Rows[0]["LOB_ID"].ToString();
            ddllLineOfBusiness.Enabled = false;

            ddlBranch.FillDataTable(dsapproval.Tables[3], "Location_ID", "Location_Name");
            ddlBranch.SelectedValue = dsapproval.Tables[3].Rows[0]["Location_ID"].ToString();
            ddlBranch.Enabled = false;


            btnGo.Enabled = false;

            ReqID.Visible = true;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlConsolidationNO.SelectedValue.ToString(), false, 0);
            hdnID.Value = "../LoanAdmin/S3GLoanAdAccountConsolidation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";


            Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);

            grvApprovalDetails.DataSource = dsapproval.Tables[3];
            grvApprovalDetails.DataBind();
        }
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
        ObjRow.Task_Number = ddlConsolidationNO.SelectedValue.ToString();
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

        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
            return;
        }

        ObjRow.Task_Type = "30";
        ObjRow.Password = txtPassword.Text.Trim();
        ObjRow.Remarks = txtRemarks.Text.Trim();
        ObjRow.Company_ID = intCompanyID.ToString();
        ObjRow.Created_By = intUserID.ToString();
        ObjApproval_DataTable.AddS3G_LOANAD_ApprovalRow(ObjRow);
        ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();
        string strErrMsg = string.Empty;
        int intErrorCode = ObjApproval.FunPubCreateApprovals(out strErrMsg, SerMode, ClsPubSerialize.Serialize(ObjApproval_DataTable, SerMode));
        switch (intErrorCode)
        {
            case 0:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + Utility.FunGetApprovalMessage("Account Consolidation",ddlstatus) + "');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=CONA';", true);
                break;
            case 1:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                break;
            case 2:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorization rule card not yet set for this combination');", true);
                break;
            case 3:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_Approval_3 + "');", true);
                break;
            case 4:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_Approval_4 + "');", true);
                break;
            case 5:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert2", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_L3A + "');", true);
                break;
            case 15:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert12", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_L4A + "');", true);
                break;
            case 16:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert13", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_L35 + "');", true);
                break;
            case 20:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Consolidation details Added failed.');", true);
                break;
            default:
                Utility.FunShowValidationMsg(this.Page, "ACONA", intErrorCode, strErrMsg, false);
                break;

        }
       
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (ddlConsolidationNO.SelectedIndex > 0)
            ddlConsolidationNO.SelectedIndex = 0;
        if (ddlBranch.SelectedIndex > 0)
            ddlBranch.SelectedIndex = 0;
        if (ddllLineOfBusiness.SelectedIndex > 0)
            ddllLineOfBusiness.SelectedIndex = 0;
        FunPubClear();
        ddlConsolidationNO.Items.Clear();


    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else            
        Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=CONA");
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
        dictDropdownListParam.Add("@FilterOption", "'HP','LN','FL'");
        dictDropdownListParam.Add("@Program_ID", "87");
        ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
        dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        dictDropdownListParam.Remove("@FilterOption");
        ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
        uinfo = null;
        dictDropdownListParam = null;
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddllLineOfBusiness.SelectedIndex > 0)
        {

            FunPubClear();
            ddlConsolidationNO.Items.Clear();
            uinfo = new UserInfo();
            btnSave.Enabled = false;
            ddlConsolidationNO.Items.Clear();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            dictDropdownListParam.Add("@Task_Type", "30");
            ddlConsolidationNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Consolidation_No", "Consolidation_No" });
            dictDropdownListParam = null;
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            ddlConsolidationNO.Items.Clear();
        }
    }

    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        dictDropdownListParam.Add("@Is_Active", "1");
        dictDropdownListParam.Add("@Program_ID", "87");
        dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
        

        if (ddlBranch.SelectedIndex > 0)
        {
            FunPubClear();
            ddlConsolidationNO.Items.Clear();
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            dictDropdownListParam.Add("@Task_Type", "30");
            ddlConsolidationNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Consolidation_No", "Consolidation_No" });
            dictDropdownListParam = null;

        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            ddlConsolidationNO.Items.Clear();
        }
    }

    protected void ddlConsolidationNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConsolidationNO.SelectedIndex > 0)
        {
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Task_Type", "30");
            dictDropdownListParam.Add("@Task_Number", ddlConsolidationNO.SelectedValue.ToString());
            dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
            if (dsapproval.Tables[1].Rows.Count > 0)
            {
                txtStatus.Text = dsapproval.Tables[1].Rows[0]["Status"].ToString();
                txtDate.Text = DateTime.Parse(dsapproval.Tables[1].Rows[0]["Account_Consolidation_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                S3GCustomerPermAddress.SetCustomerDetails(dsapproval.Tables[1].Rows[0], true);

                ReqID.Visible = true;
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlConsolidationNO.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../LoanAdmin/S3GLoanAdAccountConsolidation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";


                btnGo.Enabled = true;
            }
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;

        }
    }

    protected void ReqID_serverclick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
        return;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (ddlConsolidationNO.SelectedIndex > 0)
        {
            btnSave.Enabled = true;
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@Task_Number", ddlConsolidationNO.SelectedValue.ToString());
            dictDropdownListParam.Add("@Task_Type", "30");
            Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                grvApprovalDetails.DataSource = Ds.Tables[0];
                grvApprovalDetails.DataBind();
                btnGo.Enabled = false;
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
            if (strConstitution_No.Trim() != "")
            {
                if (dsapproval.Tables[3].Rows.Count > 0)
                {
                    grvApprovalDetails.Columns[3].Visible = false;
                    ddlstatus.SelectedValue = dsapproval.Tables[3].Rows[intRow]["Status_ID"].ToString();
                    txtRemarks.Attributes.Add("readonly", "readonly");
                    ddlstatus.Enabled = false;
                    btnSave.Enabled = false;

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
        grvApprovalDetails.DataSource = dsapproval.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();

        ReqID.Visible = false;
        btnSave.Enabled = false;

        txtStatus.Text = string.Empty;
        txtDate.Text = string.Empty;
        S3GCustomerPermAddress.ClearCustomerDetails();
    }

    #endregion
}
