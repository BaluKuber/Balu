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


public partial class S3GLoanAdLeaseAssetSaleApproval_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    int intLeaseAssetSale_ID = 0;
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
    public static S3GLoanAdLeaseAssetSaleApproval_Add obj_Page;
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            intLeaseAssetSale_ID = Convert.ToInt32(fromTicket.Name);
            //btnClear.Enabled = false;

        }
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (!IsPostBack)
        {
            ReqID.Visible = false;
            if (PageMode == PageModes.Create)
                PubFunBind(); //for payment approval prerequisite
            btnGo.Enabled = false;
            if (intLeaseAssetSale_ID > 0)
            {
                FunPubViewApprovedDetails();
            }
            else
            {
                btnSave.Enabled = false;
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
                btnRevoke.Enabled = false;
                FunPriLoadLAS();
            }

            ddllLineOfBusiness.Focus();
        }
    }
    public void FunPubViewApprovedDetails()
    {
        btnGo.Enabled = false;
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Task_Type", "12");//Lease Asset Sale APPROVAL SCREEN LOOKUPCODE IS 12
        dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
        dictDropdownListParam.Add("@Task_Number", intLeaseAssetSale_ID.ToString());
        dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
        if (dsapproval.Tables[3].Rows.Count > 0)
        {
            txtStatus.Text = dsapproval.Tables[3].Rows[0]["status"].ToString();
            txtLASDate.Text = DateTime.Parse(dsapproval.Tables[3].Rows[0]["LeaseAssetSales_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //txtCustomerName.Text = dsapproval.Tables[3].Rows[0]["Customer_Name"].ToString();
            //txtEnitycode.Text = dsapproval.Tables[3].Rows[0]["Entity_Code"].ToString();
            //txtCustomerCode.Text = dsapproval.Tables[3].Rows[0]["Customer_Code"].ToString();
            //if (dsapproval.Tables[3].Rows[0]["Customer_Code"].ToString().Trim() == "")
            //    PnlCustomer.Visible = false;
            //else
            //    PnlCustomer.Visible = true;
            //txtCustomerAddress1.Text = dsapproval.Tables[3].Rows[0]["Comm_Address1"].ToString();
            //txtCustomerAddress2.Text = dsapproval.Tables[3].Rows[0]["Comm_Address2"].ToString();
            ////txtEntityType.Text = dsapproval.Tables[1].Rows[0]["Pay_Mode_Code"].ToString();
            //txtAccountClosureDate.Text = DateTime.Parse(dsapproval.Tables[3].Rows[0]["Account_Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //txtMLA.Text = dsapproval.Tables[3].Rows[0]["PANum"].ToString();
            //if (dsapproval.Tables[1].Rows[0]["SANum"].ToString().Contains("DUMMY"))
            //    txtSLA.Text = "";
            //else
            //    txtSLA.Text = dsapproval.Tables[1].Rows[0]["SANum"].ToString();
            //txtActualvalue.Text = dsapproval.Tables[1].Rows[0]["Actual_Values"].ToString();
            //txtresvalue.Text = dsapproval.Tables[1].Rows[0]["Residual_Value"].ToString();

            ddlLASNO.FillDataTable(dsapproval.Tables[3], "LeaseAsssetSales_ID", "LeaseAssetSales_No");
            ddlLASNO.SelectedValue = dsapproval.Tables[3].Rows[0]["LeaseAsssetSales_ID"].ToString();
            ddlLASNO.Enabled = false;

            ddllLineOfBusiness.FillDataTable(dsapproval.Tables[3], "LOB_ID", "LOB_Name");
            ddllLineOfBusiness.SelectedValue = dsapproval.Tables[3].Rows[0]["LOB_ID"].ToString();
            ddllLineOfBusiness.Enabled = false;

            //ddlBranch.FillDataTable(dsapproval.Tables[3], "Location_ID", "Location_Name");
            //ddlBranch.SelectedText = dsapproval.Tables[3].Rows[0]["Location_Name"].ToString();
            //ddlBranch.SelectedValue = dsapproval.Tables[3].Rows[0]["Location_ID"].ToString();
            //ddlBranch.ToolTip = dsapproval.Tables[3].Rows[0]["Location_Name"].ToString();
            //ddlBranch.Enabled = false;

            txtStatus.ReadOnly = true;
            txtLASDate.ReadOnly = true;
            txtLASDate.ReadOnly = true;
            btnGo.Visible = false;

            //ViewState["TASK_APPROVALUSERID"] = 

            ReqID.Visible = true;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlLASNO.SelectedValue.ToString(), false, 0);
            hdnID.Value = "../LoanAdmin/S3GLoanAd_LeaseAssetSale.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

            Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);

            grvApprovalDetails.DataSource = dsapproval.Tables[3];
            grvApprovalDetails.DataBind();
        }
    }
    #endregion

    #region Button (Save / Clear / Cancel)
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtStatus.Text.Trim().ToUpper() == "APPROVED")
        {
            Utility.FunShowAlertMsg(this, "The slected Lease Asset Sale detail already approved.");
            return;
        }
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
        ObjRow.Task_Number = ddlLASNO.SelectedValue.ToString();//here maintain id autogeneration number
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

        ObjRow.Task_Type = "12";
        ObjRow.Password = txtPassword.Text.Trim();
        ObjRow.Remarks = txtRemarks.Text.Trim();
        ObjRow.Company_ID = intCompanyID.ToString();
        ObjRow.Created_By = intUserID.ToString();

        ObjApproval_DataTable.AddS3G_LOANAD_ApprovalRow(ObjRow);
        ObjApproval = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();
        string strErrMsg = string.Empty;
        int intErrorCode = ObjApproval.FunPubCreateApprovals(out strErrMsg, SerMode, ClsPubSerialize.Serialize(ObjApproval_DataTable, SerMode));
        if (intErrorCode == 0)
        {            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + ViewState["Type"].ToString() +
                ((Convert.ToInt32(ddlstatus.SelectedValue) == 3) ? " Approved" : " Rejected") + " Successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=LAA';", true);
            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
            btnSave.Enabled = false;
            //END
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
            /*Call Fixing - 5369 - Thalai - Invalid Message - 3-Nov-2016 - Start*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert2", "alert('" + Resources.ValidationMsgs.S3G_ValMsg_L3A + "');", true);
            /*Call Fixing - 5369 - Thalai - Invalid Message - 3-Nov-2016 - End*/
            return;
        }

        else if (intErrorCode == 15)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert12", "alert('" + Resources.LocalizationResources.Level4andAboveApproval + "');", true);
            return;
        }
        else if (intErrorCode == 16)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert13", "alert('3 and 5 level users can only approve.');", true);
            return;
        }
        else if (intErrorCode == 20)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Lease Asset Sale Approval details added failed.');", true);
            return;
        }
        else
        {
            Utility.FunShowValidationMsg(this.Page, "LASA", intErrorCode, strErrMsg, false);
            return;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //if (ddlBranch.SelectedIndex > 0)
        //ddlBranch.SelectedIndex = 0;
        ddlBranch.Clear();
        if (ddllLineOfBusiness.SelectedIndex > 0)
            ddllLineOfBusiness.SelectedIndex = 0;
        ddlLASNO.Items.Clear();
        chkApproved.Checked = false;
        chkUnapproval.Checked = true;
        FunPubClear();
        ddllLineOfBusiness.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=LAA");
    }
    #endregion

    #region Contorl Events
    protected void ReqID_serverclick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
        return;
    }
    public void PubFunBind()
    {
        dictDropdownListParam = new Dictionary<string, string>();
        uinfo = new UserInfo();
        dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        dictDropdownListParam.Add("@Is_Active", "1");
        dictDropdownListParam.Add("@FilterOption", "'OL','FL'");
        dictDropdownListParam.Add("@Program_ID", "63");
        ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
        if (ddllLineOfBusiness.Items.Count == 2)
        {
            ddllLineOfBusiness.SelectedIndex = 1;
            ddllLineOfBusiness.ClearDropDownList();
        }
        dictDropdownListParam.Remove("@FilterOption");
        if (PageMode == PageModes.Query)
        {
            //dictDropdownListParam.Add("@LOB_ID", "0");
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
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
            ddlLASNO.Items.Clear();
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            dictDropdownListParam.Add("@Task_Type", "12");
            dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            if (chkApproved.Checked)
            {
                dictDropdownListParam.Add("@UnApproved", "1");
            }
            else
            {
                dictDropdownListParam.Add("@UnApproved", "0");
            }
            ddlLASNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "LeaseAsssetSales_ID", "LeaseAssetSales_No" });
            dictDropdownListParam = null;
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;
            ddlLASNO.Items.Clear();
        }

        ddlBranch.Focus();
    }
    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        //
        // Commanded As per bug id 2738
        //

        //if (ddllLineOfBusiness.SelectedIndex > 0)
        //{
        //    FunPubClear();
        //    ddlLASNO.Items.Clear();
        //    btnSave.Enabled = false;
        //    //btnClear.Enabled = false;
        //    uinfo = new UserInfo();
        //    dictDropdownListParam = new Dictionary<string, string>();
        //    dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
        //    dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //    dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        //    dictDropdownListParam.Add("@Branch_ID", ddlBranch.SelectedValue.ToString());
        //    dictDropdownListParam.Add("@Task_Type", "12");
        //    ddlLASNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "LeaseAsssetSales_ID", "LeaseAssetSales_No" });
        //    dictDropdownListParam = null;

        //}
        //else
        //{

        //dictDropdownListParam = new Dictionary<string, string>();
        //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //dictDropdownListParam.Add("@Is_Active", "1");
        //dictDropdownListParam.Add("@Program_ID", "63");
        //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });

        ddlBranch.Clear();
        FunPubClear();
        btnSave.Enabled = false;
        btnGo.Enabled = true;
        ddlLASNO.Items.Clear();
        ddllLineOfBusiness.Focus();
        //}
    }
    protected void ddlLASNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLASNO.SelectedIndex > 0)
        {
            FunPubClear();
            ReqID.Visible = true;
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Task_Type", "12");
            dictDropdownListParam.Add("@Task_Number", ddlLASNO.SelectedValue.ToString());
            dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
            if (dsapproval.Tables[1].Rows.Count > 0)
            {
                txtStatus.Text = dsapproval.Tables[1].Rows[0]["status"].ToString();
                if (!string.IsNullOrEmpty(dsapproval.Tables[1].Rows[0]["LeaseAssetSales_Date"].ToString()))
                    txtLASDate.Text = DateTime.Parse(dsapproval.Tables[1].Rows[0]["LeaseAssetSales_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                ViewState["Type"] = dsapproval.Tables[1].Rows[0]["Type"].ToString();
                //txtCustomerName.Text = dsapproval.Tables[1].Rows[0]["Customer_Name"].ToString();
                //txtEnitycode.Text = dsapproval.Tables[1].Rows[0]["Entity_Code"].ToString();
                //txtCustomerCode.Text = dsapproval.Tables[1].Rows[0]["Customer_Code"].ToString();
                //if (dsapproval.Tables[1].Rows[0]["Customer_Code"].ToString().Trim()=="")
                //    PnlCustomer.Visible = false;
                //else
                //    PnlCustomer.Visible = true; 
                //txtCustomerAddress1.Text = dsapproval.Tables[1].Rows[0]["Comm_Address1"].ToString();
                //txtCustomerAddress2.Text = dsapproval.Tables[1].Rows[0]["Comm_Address2"].ToString();
                ////txtEntityType.Text = dsapproval.Tables[1].Rows[0]["Pay_Mode_Code"].ToString();
                //txtAccountClosureDate.Text = DateTime.Parse(dsapproval.Tables[1].Rows[0]["Account_Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                //txtMLA.Text = dsapproval.Tables[1].Rows[0]["PANum"].ToString();
                //if (dsapproval.Tables[1].Rows[0]["SANum"].ToString().Contains("DUMMY"))
                //    txtSLA.Text = "";
                //else
                //    txtSLA.Text = dsapproval.Tables[1].Rows[0]["SANum"].ToString();
                // txtActualvalue.Text = dsapproval.Tables[1].Rows[0]["Actual_Values"].ToString();
                // txtresvalue.Text = dsapproval.Tables[1].Rows[0]["Residual_Value"].ToString();
                txtStatus.ReadOnly = true;
                txtLASDate.ReadOnly = true;
                //  txtCustomerName.ReadOnly = true;
                //  txtEnitycode.ReadOnly = true;
                //  txtCustomerAddress1.ReadOnly = true;
                //  txtCustomerAddress2.ReadOnly = true;
                //  txtCustomerCode.ReadOnly = true;
                ////  txtEntityType.ReadOnly = true;
                //  txtAccountClosureDate.ReadOnly = true;

                btnGo.Enabled = true;

                ReqID.Visible = true;
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlLASNO.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../LoanAdmin/S3GLoanAd_LeaseAssetSale.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";
            }
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;

        }

        ddlLASNO.Focus();
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (ddlLASNO.SelectedIndex > 0)
        {
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            btnSave.Enabled = true;
            // btnClear.Enabled = true;
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@Task_Number", ddlLASNO.SelectedValue.ToString());
            if (ViewState["Type"] == null)
            {
                dictDropdownListParam.Add("@Type_Value", "");
            }
            else
            {
                dictDropdownListParam.Add("@Type_Value", ViewState["Type"].ToString());
            }
            dictDropdownListParam.Add("@Task_Type", "12");

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
                    btnRevoke.Enabled = false;
                    btnSave.Enabled = true;
                }
            }

            //if (txtStatus.Text.Trim().ToUpper() == "APPROVED")
            //{
            //    btnSave.Enabled = false;
            //}
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
            if (intLeaseAssetSale_ID > 0)
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
                        //  btnClear.Enabled = false;

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

            ddlstatus.Focus();
        }
    }
    public void FunPubClear()
    {
        grvApprovalDetails.DataSource = dsapproval.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();

        txtStatus.Text = string.Empty;
        txtLASDate.Text = string.Empty;
        ReqID.Visible = false;
        btnSave.Enabled = false;
        //btnGo.Enabled = true;
        btnGo.Enabled = false;
        btnRevoke.Enabled = false;
        btnClear.Enabled = true;

    }
    #endregion

    protected void FunProFillUnApprovedDetails()
    {
        if (chkUnapproval.Checked)
        {
            /*
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
            dictDropdownListParam.Add("@FilterOption", "'OL','FL'");
            dictDropdownListParam.Add("@Program_ID", "63");
            ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            if (ddllLineOfBusiness.Items.Count == 2)
            {
                ddllLineOfBusiness.SelectedIndex = 1;
                ddllLineOfBusiness.ClearDropDownList();
            }
            dictDropdownListParam.Remove("@FilterOption");
            if (PageMode == PageModes.Query)
            {
                dictDropdownListParam.Add("@LOB_ID", "0");
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            }
            ddlLASNO.Items.Clear();
            ddlLASNO.Items.Insert(0, "--Select--");
            uinfo = null;
            dictDropdownListParam = null;
             * */
            FunPriLoadLAS();
        }
        if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
        {
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            //btnClear.Enabled = false;

            ddllLineOfBusiness.Items.Clear();
            ddlBranch.Clear();
            ddlLASNO.Items.Clear();

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
            /*
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
            dictDropdownListParam.Add("@FilterOption", "'OL','FL'");
            dictDropdownListParam.Add("@Program_ID", "63");
            ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            if (ddllLineOfBusiness.Items.Count == 2)
            {
                ddllLineOfBusiness.SelectedIndex = 1;
                ddllLineOfBusiness.ClearDropDownList();
            }
            dictDropdownListParam.Remove("@FilterOption");
            if (PageMode == PageModes.Query)
            {
                dictDropdownListParam.Add("@LOB_ID", "0");
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            }
            ddlLASNO.Items.Clear();
            ddlLASNO.Items.Insert(0, "--Select--");
            uinfo = null;
            dictDropdownListParam = null;
             * */
            FunPriLoadLAS();
        }
        else
        {
            if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
            {


                ddllLineOfBusiness.Items.Clear();
                ddlBranch.Clear();
                ddlLASNO.Items.Clear();

                FunPubClear();
                // FunPubloadCombo();
            }
        }

        ddllLineOfBusiness.Focus();
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
                dictDropdownListParam.Add("@OPTIONS", "1");
                dictDropdownListParam.Add("@TASK_TYPE", "12");
                dictDropdownListParam.Add("@TASK_NUMBER", ddlLASNO.SelectedValue.ToString());
                dictDropdownListParam.Add("@USER_ID", ObjUserInfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Password", txtPassword.Text.Trim());

                int ErrCode = ObjApproval.FunPubRevokeOrUpdateApprovedDetails(dictDropdownListParam);

                switch (ErrCode)
                {
                    case 0:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + ViewState["Type"].ToString() + " Revoked Successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=LAA';", true);
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
                    //Added by Sathiyanathan on 18Sep2015 to check Transfer records are disposed/not - Starts Here
                    case 11:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Some Invoices was disposed already.so unable to revoke Lease Asset Sale Transfer');", true);
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
        Procparam.Add("@Program_Id", "063");
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    //Added by Sathiyanathan on 28Jul2015 Starts here

    private void FunPriLoadLAS()
    {
        try
        {
            if (dictDropdownListParam != null)
                dictDropdownListParam.Clear();
            else
                dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            dictDropdownListParam.Add("@Task_Type", "12");
            if (chkApproved.Checked)
            {
                dictDropdownListParam.Add("@UnApproved", "1");
            }
            else
            {
                dictDropdownListParam.Add("@UnApproved", "0");
            }
            ddlLASNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "LeaseAsssetSales_ID", "LeaseAssetSales_No" });
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }
    //Added by Sathiyanathan on 28Jul2015 Ends here
}
