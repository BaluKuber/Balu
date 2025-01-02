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


public partial class S3GLoanAdAccountClosureApproval_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    string strAccountClosure_No = "";
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

    public static S3GLoanAdAccountClosureApproval_Add obj_Page;

    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            strAccountClosure_No = fromTicket.Name.ToString();
            btnClear.Enabled = false;
        }
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = Convert.ToString(ObjUserInfo.ProUserIdRW);
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (!IsPostBack)
        {
            ReqID.Visible = false;
            PubFunBind();
            if (strAccountClosure_No.Trim() != "")
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
                btnClear.Enabled =
                    trCkbox.Visible = false;
            }

            if (Request.QueryString["qsViewId"] == null)
            {
                chkUnapproval.Checked = true;
                FunProFillUnApprovedDetails();
                btnRevoke.Enabled = false;
            }

            ddllLineOfBusiness.Focus();
        }
    }
    public void FunPubViewApprovedDetails()
    {
        string strSubAccNo = "";
        btnGo.Enabled = false;
        uinfo = new UserInfo();
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());//Account Closure APPROVAL SCREEN LOOKUPCODE IS 15
        dictDropdownListParam.Add("@Task_Type", "15");//Account Closure APPROVAL SCREEN LOOKUPCODE IS 15
        dictDropdownListParam.Add("@Task_Number", strAccountClosure_No.ToString());
        dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
        if (dsapproval.Tables[3].Rows.Count > 0)
        {
            txtStatus.Text = dsapproval.Tables[3].Rows[0]["status"].ToString();
            if (!string.IsNullOrEmpty(dsapproval.Tables[3].Rows[0]["Closure_Date"].ToString()))
                txtACDate.Text = DateTime.Parse(dsapproval.Tables[3].Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            S3GCustomerPermAddress.SetCustomerDetails(dsapproval.Tables[3].Rows[0], true);

            if (dsapproval.Tables[3].Rows[0]["PANum"].ToString() != "0")
                txtMLA.Text = dsapproval.Tables[3].Rows[0]["PANum"].ToString();

            // Code added for Call Id : 2696 Strat
            if (dsapproval.Tables[3].Rows[0]["Tranche_Name"].ToString() != null)
                txtTranche.Text = dsapproval.Tables[3].Rows[0]["Tranche_Name"].ToString();
            // Call Id : 2696 End 

            //if (!string.IsNullOrEmpty(dsapproval.Tables[3].Rows[0]["SANum"].ToString()))
            //{
            //    if (!dsapproval.Tables[3].Rows[0]["SANum"].ToString().Contains("DUMMY"))
            //    {
            //        ddlSubAccountNo.FillDataTable(dsapproval.Tables[3], "SANum", "SANum");
            //        ddlSubAccountNo.SelectedValue = dsapproval.Tables[3].Rows[0]["SANum"].ToString();
            //        strSubAccNo = dsapproval.Tables[3].Rows[0]["SANum"].ToString() + "~" + dsapproval.Tables[3].Rows[0]["Closure_Details_ID"].ToString();
            //    }
            //    else
            //    {
            //        ddlSubAccountNo.SelectedValue = "";

            //        strSubAccNo = dsapproval.Tables[3].Rows[0]["SANum"].ToString() + "~" + dsapproval.Tables[3].Rows[0]["Closure_Details_ID"].ToString();
            //    }
            //}
            //ddlSubAccountNo.Enabled = false;

            //ddlACNo.FillDataTable(dsapproval.Tables[3], "Closure_No", "Closure_No");
            ddlACNo.SelectedValue = dsapproval.Tables[3].Rows[0]["Closure_id"].ToString();
            ddlACNo.SelectedText = dsapproval.Tables[3].Rows[0]["Closure_No"].ToString();
            ddlACNo.Enabled = false;

            ddllLineOfBusiness.FillDataTable(dsapproval.Tables[3], "LOB_ID", "LOB_Name");
            ddllLineOfBusiness.SelectedValue = dsapproval.Tables[3].Rows[0]["LOB_ID"].ToString();
            ddllLineOfBusiness.Enabled = false;

            //ddlBranch.FillDataTable(dsapproval.Tables[3], "Location_ID", "Location_Name");
            ddlBranch.SelectedValue = dsapproval.Tables[3].Rows[0]["Location_ID"].ToString();
            ddlBranch.SelectedText = dsapproval.Tables[3].Rows[0]["Location_Name"].ToString();
            ddlBranch.Enabled = false;



            ReqID.Visible = true;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlACNo.SelectedValue.ToString() , false, 0);
            hdnID.Value = "../LoanAdmin/S3GLOANADAccountClosure_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";


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
        string strAction = "";
        foreach (GridViewRow grv in grvApprovalDetails.Rows)
        {
            lblApprovalSNO = (Label)grv.FindControl("lblApprovalSNO");
            lblApproval_ID = (Label)grv.FindControl("lblApproval_ID");
            ddlstatus = (DropDownList)grv.FindControl("ddlstatus");
            txtRemarks = (TextBox)grv.FindControl("txtRemarks");
            txtPassword = (TextBox)grv.FindControl("txtPassword");
        }
        S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalRow ObjRow;

        if (txtStatus.Text.Trim().ToUpper() == "APPROVED")
        {
            Utility.FunShowAlertMsg(this, "The slected Account Closure detail already approved.");
            return;
        }

        ObjRow = ObjApproval_DataTable.NewS3G_LOANAD_ApprovalRow();

        if (ddlSubAccountNo.SelectedIndex != 0)
            ObjRow.Task_Number = ddlSubAccountNo.SelectedValue.ToString();
        else
            ObjRow.Task_Number = ddlACNo.SelectedValue;
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

        ObjRow.Task_Type = "15";
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
            System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = null;
            strAction = ddlstatus.SelectedValue == "3" ? "Approved" : "Rejected";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Rental Schedule Closure " + strAction + " Successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=AAP';", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Closure details Added failed.');", true);
            return;
        }
        else if (intErrorCode == 51)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Account Closure request is cancelled due to Cheque returned.');", true);
            return;
        }
        else
        {
            Utility.FunShowValidationMsg(this.Page, "ACLA", intErrorCode, strErrMsg, false);
            return;
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //if (ddlACNo.SelectedIndex > 0)
        //    ddlACNo.SelectedIndex = 0;
        //if (ddlBranch.SelectedIndex > 0)
        //    ddlBranch.SelectedIndex = 0;
        ddlBranch.Clear();
        if (ddllLineOfBusiness.SelectedIndex > 0)
            ddllLineOfBusiness.SelectedIndex = 0;
        //ddlACNo.Items.Clear();
        ddlACNo.Clear();
        ddlSubAccountNo.Items.Clear();
        FunPubClear();
        ddllLineOfBusiness.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel

        System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = null;

        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=AAP");
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
        dictDropdownListParam.Add("@Program_ID", "69");
        ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
        //dictDropdownListParam.Add("@LOB_ID", "0");
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
        uinfo = null;
        dictDropdownListParam = null;
        ddllLineOfBusiness.SelectedIndex = 1;
        ddllLineOfBusiness.RemoveDropDownList();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetACCNumber(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        //DataTable dtCommon = new DataTable();
        //DataSet Ds = new DataSet();

        obj_Page.uinfo = new UserInfo();
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", obj_Page.uinfo.ProCompanyIdRW.ToString());

        //Procparam.Add("@User_Id", Convert.ToString(obj_Page.intUserID));

        if (System.Web.HttpContext.Current.Session["AutoSuggestUserID"] != null)
            Procparam.Add("@User_Id", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
       
        Procparam.Add("@LOB_ID", obj_Page.ddllLineOfBusiness.SelectedValue.ToString());
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue.ToString());
        Procparam.Add("@Task_Type", "15");
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

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlACNo.Clear();
        if (ddllLineOfBusiness.SelectedIndex > 0)
        {
            FunPubClear();
            btnSave.Enabled = false;
            //ddlACNo.Items.Clear();
            ddlACNo.Clear();
            ddlSubAccountNo.Items.Clear();
            //uinfo = new UserInfo();
            //dictDropdownListParam = new Dictionary<string, string>();
            //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            //dictDropdownListParam.Add("@Task_Type", "15");
            //if (chkApproved.Checked)
            //{
            //    dictDropdownListParam.Add("@UnApproved", "1");
            //}
            //else
            //{
            //    dictDropdownListParam.Add("@UnApproved", "0");
            //}
            //ddlACNo.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Closure_Details_ID", "Closure_No" });
            //dictDropdownListParam = null;
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;
        }

        ddlBranch.Focus();
    }
    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        //dictDropdownListParam = new Dictionary<string, string>();
        //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //dictDropdownListParam.Add("@Is_Active", "1");
        //dictDropdownListParam.Add("@Program_ID", "69");
        //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });

        //ddlBranch.SelectedIndex = -1;
        //if (ddlBranch.SelectedIndex > 0)
        //{
        //    FunPubClear();
        //    btnSave.Enabled = false;
        //    ddlACNo.Items.Clear();
        //    ddlSubAccountNo.Items.Clear();
        //    uinfo = new UserInfo();
        //    dictDropdownListParam = new Dictionary<string, string>();
        //    dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
        //    dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //    dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        //    dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
        //    dictDropdownListParam.Add("@Task_Type", "15");
        //    ddlACNo.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "CLOSURE_NO", "Closure_No" });
        //    dictDropdownListParam = null;

        //}
        //else
        //{
            FunPubClear();
            //ddlACNo.Items.Clear();
            ddlACNo.Clear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;
        //}

        ddllLineOfBusiness.Focus();
    }

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
        Procparam.Add("@Program_Id", "69");
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue.ToString());
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    protected void ddlACNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["strSubAccNo"] = "";
        //if (ddlACNo.SelectedIndex > 0)
        if (ddlACNo.SelectedValue != "0")
        {
            ReqID.Visible = true;
            FunPubClear();
            ddlSubAccountNo.Items.Clear();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Task_Type", "15");
            dictDropdownListParam.Add("@Task_Number", ddlACNo.SelectedValue.ToString());
            dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
            dictDropdownListParam = null;
            if (dsapproval.Tables[1].Rows.Count > 0)
            {

                txtStatus.Text = dsapproval.Tables[1].Rows[0]["status"].ToString();
                if (!string.IsNullOrEmpty(dsapproval.Tables[1].Rows[0]["Closure_Date"].ToString()))
                    txtACDate.Text = DateTime.Parse(dsapproval.Tables[1].Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                S3GCustomerPermAddress.SetCustomerDetails(dsapproval.Tables[1].Rows[0], true);
                txtMLA.Text = dsapproval.Tables[1].Rows[0]["PANum"].ToString();
                txtTranche.Text = dsapproval.Tables[1].Rows[0]["Tranche_Name"].ToString();

                //-------------22/12/2010 added by ramesh
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@COMPANY_ID", intCompanyID.ToString());
                dictDropdownListParam.Add("@USER_ID", intUserID.ToString());
                dictDropdownListParam.Add("@PANum", dsapproval.Tables[1].Rows[0]["PANum"].ToString());
                dictDropdownListParam.Add("@Closure_Type", "1");
                dictDropdownListParam.Add("@Task_Type", "15");
                if (chkApproved.Checked)
                {
                    dictDropdownListParam.Add("@UnApproved", "1");
                }
                else
                {
                    dictDropdownListParam.Add("@UnApproved", "0");
                }
                DataSet Dset = Utility.GetDataset("S3G_LOANAD_GetAccountClosureApprovalReqDetail", dictDropdownListParam);

                ddlSubAccountNo.BindDataTable(Dset.Tables[0], new string[] { "Closure_Details_ID", "SANum" });


                //DataTable dtAcClosureApproval = Utility.GetDefaultData("S3G_LOANAD_GetAccountClosureApprovalReqDetail", dictDropdownListParam);
                //DataTable DtTemp = dtAcClosureApproval;
                //string strSaNum = "";
                //for (int i = 0; i < dtAcClosureApproval.Rows.Count -1; i++)
                //{
                //    strSaNum = dtAcClosureApproval.Rows[i]["SANum"].ToString();
                //    if (strSaNum.Contains("DUMMY"))
                //    {
                //        hdnClsDtlsID.Value = dtAcClosureApproval.Rows[i]["Closure_Details_ID"].ToString();
                //        Session["strSubAccNo"] = dtAcClosureApproval.Rows[i]["SANum"].ToString();
                //        //Modified by Manikandan
                //        DtTemp.Rows[i].Delete();
                //    }

                //}
                //ddlSubAccountNo.BindDataTable(DtTemp);

                //if (ddlSubAccountNo.Items.Count > 1)
                //{
                //    rvsubacco.Enabled = true;
                //    Session["strSubAccNo"] = ddlSubAccountNo.SelectedItem.Text.Trim() + "~" + ddlSubAccountNo.SelectedValue;
                //    ReqID.Visible = false;
                //    txtACDate.Text = "";
                //    hdnClsDtlsID.Value = Dset.Tables[1].Rows[0]["Closure_Details_ID"].ToString();
                //    //dtAcClosureApproval.Rows[i]["Closure_Details_ID"].ToString();
                //}
                //else
                //{
                //    hdnClsDtlsID.Value = Dset.Tables[1].Rows[0]["Closure_Details_ID"].ToString();
                //    Session["strSubAccNo"] = "" + "~" + Dset.Tables[1].Rows[0]["Closure_Details_ID"].ToString();
                //    ReqID.Visible = true;
                //    rvsubacco.Enabled = false;
                //}

                //DtTemp = null;
                //dtAcClosureApproval = null;

                ////-------------22/12/2010 added by ramesh


                // txtSLA.ReadOnly = true;

                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlACNo.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../LoanAdmin/S3GLOANADAccountClosure_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";


                btnGo.Enabled = true;
            }
        }
        else
        {
            FunPubClear();
            ddlSubAccountNo.Items.Clear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;

        }

        ddlACNo.Focus();
    }
    protected void ReqID_serverclick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
        return;
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //if (ddlACNo.SelectedIndex > 0)
        if (ddlACNo.SelectedValue != "0")
        {
            btnSave.Enabled = true;
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@Task_Number", ddlACNo.SelectedValue.ToString());
            if (ddlSubAccountNo.SelectedIndex > 0)
            {
                dictDropdownListParam.Add("@Task_Number2", ddlSubAccountNo.SelectedItem.Text.ToString());
            }
            dictDropdownListParam.Add("@Task_Type", "15");

            DataTable dtCheck = Utility.GetDefaultData("S3G_LOANAD_CheckApprovalRevoke", dictDropdownListParam);
            Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);
            //if (Ds.Tables[0].Rows.Count > 0)
            //{
            //    grvApprovalDetails.DataSource = Ds.Tables[0];
            //    grvApprovalDetails.DataBind();
            //    btnGo.Enabled = false;
            //}
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
            if (strAccountClosure_No.Trim() != "")
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

            ddlstatus.Focus();
        }
    }
    public void FunPubClear()
    {
        grvApprovalDetails.DataSource = dsapproval.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();
        ddlSubAccountNo.Items.Clear();

        txtACDate.Text = "";
        txtStatus.Text = string.Empty;
        ReqID.Visible = false;
        btnGo.Enabled = true;
        btnSave.Enabled = false;
        txtMLA.Text = string.Empty;
        // ddlSubAccountNo.Items.Clear();
        S3GCustomerPermAddress.ClearCustomerDetails();
        ddllLineOfBusiness.Focus();
    }
    public void FunPubSubAcountClear()
    {
        grvApprovalDetails.DataSource = dsapproval.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();

        txtACDate.Text = "";
        ReqID.Visible = false;
        btnSave.Enabled = false;
        btnGo.Enabled = true;
        txtStatus.Text = string.Empty;
        //txtPMCDate.Text = string.Empty;
        // S3GCustomerPermAddress.ClearCustomerDetails();
        //txtMLA.Text = string.Empty;

    }
    #endregion

    protected void ddlSubAccountNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSubAccountNo.SelectedIndex > 0)
        {
            FunPubSubAcountClear();
            if (grvApprovalDetails.Rows.Count > 0)
            {
                grvApprovalDetails.DataSource = dsapproval.Tables.Add();  //make empty grid
                grvApprovalDetails.DataBind();
                btnGo.Enabled = true;
            }
            ReqID.Visible = true;
            Session["strSubAccNo"] = ddlSubAccountNo.SelectedItem.Text.Trim() + "~" + ddlSubAccountNo.SelectedValue;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlACNo.SelectedValue.ToString() + "~" + txtMLA.Text + "~" + Session["strSubAccNo"].ToString(), false, 0);
            hdnID.Value = "../LoanAdmin/S3GLOANADAccountClosure_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

            dictDropdownListParam = new Dictionary<string, string>();//added latest 21/12/2010
            dictDropdownListParam.Add("@COMPANY_ID", intCompanyID.ToString());
            dictDropdownListParam.Add("@USER_ID", intUserID.ToString());
            dictDropdownListParam.Add("@PANum", txtMLA.Text.Trim());//, new string[] { "Closure_Details_ID", "SANum" }
            dictDropdownListParam.Add("@Closure_Type", "1");
            dictDropdownListParam.Add("@Task_Type", "15");
            dictDropdownListParam.Add("@SANum", ddlSubAccountNo.SelectedItem.Text.Trim());
            DataTable dtAcClosureApproval = Utility.GetDefaultData("S3G_LOANAD_GetAccountClosureApprovalReqDetail", dictDropdownListParam);
            DataTable DtTemp = dtAcClosureApproval;
            if (DtTemp.Rows.Count > 0)
            {
                txtStatus.Text = DtTemp.Rows[0]["status"].ToString();
                txtACDate.Text = Convert.ToDateTime(DtTemp.Rows[0]["Closure_Date"].ToString()).ToString(strDateFormat);
            }

        }
        else
        {
            FunPubSubAcountClear();
            ReqID.Visible = false;
        }

        ddlSubAccountNo.Focus();
    }

    protected void chkUnapproval_CheckedChanged(object sender, EventArgs e)
    {
        FunProFillUnApprovedDetails();
    }

    protected void FunProFillUnApprovedDetails()
    {
        //ddllLineOfBusiness.SelectedValue = "0";
        if (chkUnapproval.Checked)
        {
            FunPubClear();
            //ddllLineOfBusiness.Items.Clear();
            //ddlBranch.Items.Clear();
            ddlBranch.Clear();
            //UserInfo uinfo = null;
            //Dictionary<string, string> dictDropdownListParam;
            //dictDropdownListParam = new Dictionary<string, string>();
            //uinfo = new UserInfo();
            //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@Is_Active", "1");
            //dictDropdownListParam.Add("@Program_ID", "69");
            //ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            //dictDropdownListParam.Add("@LOB_ID", "0");
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            //ddlACNo.Items.Clear();
            //ddlACNo.Items.Insert(0, "--Select--");
            ddlACNo.Clear();
            //uinfo = null;
            //dictDropdownListParam = null;
        }
        if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
        {
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            //btnClear.Enabled = false;

            //ddllLineOfBusiness.Items.Clear();
            //ddlBranch.Items.Clear();
            ddlBranch.Clear();
            //ddlACNo.Items.Clear();
            ddlACNo.Clear();
            FunPubClear();
            //FunPubloadCombo();
        }

        ddllLineOfBusiness.Focus();
    }


    protected void chkApproved_CheckedChanged(object sender, EventArgs e)
    {
        //ddllLineOfBusiness.SelectedValue = "0";
        if (chkApproved.Checked)
        {
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            //btnClear.Enabled = false;

            FunPubClear();
            //ddlBranch.Items.Clear();
            ddlBranch.Clear();
            
            //UserInfo uinfo = null;
            //Dictionary<string, string> dictDropdownListParam;
            //dictDropdownListParam = new Dictionary<string, string>();
            //uinfo = new UserInfo();
            //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@Is_Active", "1");
            //dictDropdownListParam.Add("@Program_ID", "69");
            //ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            //dictDropdownListParam.Add("@LOB_ID", "0");
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            //ddlACNo.Items.Clear();
            //ddlACNo.Items.Insert(0, "--Select--");
            ddlACNo.Clear();
            uinfo = null;
            dictDropdownListParam = null;
        }
        else
        {
            if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
            {


                //ddllLineOfBusiness.Items.Clear();
                //ddlBranch.Items.Clear();
                ddlBranch.Clear();
                //ddlACNo.Items.Clear();
                ddlACNo.Clear();

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
                dictDropdownListParam.Add("@OPTIONS", "4");
                dictDropdownListParam.Add("@TASK_TYPE", "15");
                dictDropdownListParam.Add("@TASK_NUMBER", ddlACNo.SelectedValue.ToString());
                dictDropdownListParam.Add("@USER_ID", ObjUserInfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Password", txtPassword.Text.Trim());

                int ErrCode = ObjApproval.FunPubRevokeOrUpdateApprovedDetails(dictDropdownListParam);

                switch (ErrCode)
                {
                    case 0:
                        {
                            System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = null;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Rental Schedule Closure Approval details Revoked successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=AAP';", true);
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
            throw;
        }
        finally
        {
            ObjApproval.Close();
        }
    }
}
