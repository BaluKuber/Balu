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
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.text.html;
using System.Text;
using iTextSharp;
using iTextSharp.text.pdf;
using System.IO;
public partial class S3GLoanAdPreMatureClosureApproval_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    string StrPrematureClosure_No = "";
    int intUserID = 0;
    string strSubAccNo = "";
    int intRow = 0;
    string PANUM="";
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    SerializationMode SerMode = SerializationMode.Binary;
    DataTable dtAction = new DataTable();
    DataSet dsapproval = new DataSet();
    DataSet Ds = new DataSet();
    string strDateFormat;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    public static S3GLoanAdPreMatureClosureApproval_Add obj_Page;
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
            StrPrematureClosure_No = fromTicket.Name.ToString();
        }
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (!IsPostBack)
        {
            ReqID.Visible = false;
          
            if (PageMode == PageModes.Create)
            {
                PubFunBind(); //for payment approval prerequisite
            }
            if (StrPrematureClosure_No != "")
            {
                FunPubViewApprovedDetails();
                btnGo.Enabled = false;
                btnClear.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
            }

            if (PageMode == PageModes.Query)
            {
                //btnRevoke.Visible = false;
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
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Task_Type", "13");//Pre Mature Closure APPROVAL SCREEN LOOKUPCODE IS 12
        dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
        dictDropdownListParam.Add("@Task_Number", StrPrematureClosure_No.ToString());//it' s id clusure details id
        dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
        if (dsapproval.Tables[3].Rows.Count > 0)
        {
            txtStatus.Text = dsapproval.Tables[3].Rows[0]["status"].ToString();
            if (!string.IsNullOrEmpty(dsapproval.Tables[3].Rows[0]["Closure_Date"].ToString()))
                txtPMCDate.Text = DateTime.Parse(dsapproval.Tables[3].Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            S3GCustomerPermAddress.SetCustomerDetails(dsapproval.Tables[3].Rows[0], true);
            //txtMLA.Text = dsapproval.Tables[3].Rows[0]["PANum"].ToString();

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
            //        strSubAccNo = dsapproval.Tables[3].Rows[0]["SANum"].ToString() + "~" + dsapproval.Tables[3].Rows[0]["Closure_Details_ID"].ToString();
            //    }
            //}
          //  ddlSubAccountNo.Enabled = false;

            //ddlPMCNO.FillDataTable(dsapproval.Tables[3], "Closure_No", "Closure_No");
            ddlPMCNO.SelectedText = dsapproval.Tables[3].Rows[0]["Closure_No"].ToString();
            ddlPMCNO.SelectedValue = dsapproval.Tables[3].Rows[0]["Closure_id"].ToString();
            ddlPMCNO.Enabled = false;

            ddllLineOfBusiness.FillDataTable(dsapproval.Tables[3], "LOB_ID", "LOB_Name");
            ddllLineOfBusiness.SelectedValue = dsapproval.Tables[3].Rows[0]["LOB_ID"].ToString();
            ddllLineOfBusiness.Enabled = false;

            //ddlBranch.FillDataTable(dsapproval.Tables[3], "LOCATION_ID", "Location_Name");
            ddlBranch.SelectedText = dsapproval.Tables[3].Rows[0]["Location_Name"].ToString();
            ddlBranch.SelectedValue = dsapproval.Tables[3].Rows[0]["Location_ID"].ToString();
            ddlBranch.ToolTip = dsapproval.Tables[3].Rows[0]["Location_Name"].ToString();
            ddlBranch.Enabled = false;

            btnRevoke.Enabled = false;

            Ds = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalCommonDetail, dictDropdownListParam);

            ReqID.Visible = true;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlPMCNO.SelectedValue.ToString(), false, 0);
            hdnID.Value = "../LoanAdmin/S3GLOANADPreMatureClosure_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

            grvApprovalDetails.DataSource = dsapproval.Tables[3];
            grvApprovalDetails.DataBind();
        }
    }
    #endregion


    #region Button (Save  / Clear / Cancel)
    protected void btnSave_Click(object sender, EventArgs e)
   {
       string strAction = "";
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
                                                                    
        if (txtStatus.Text.Trim().ToUpper() == "APPROVED")
        {
            Utility.FunShowAlertMsg(this, "The selected Pre Mature Closure detail already approved.");
            return;
        }

        ObjRow = ObjApproval_DataTable.NewS3G_LOANAD_ApprovalRow();
        //if (ddlSubAccountNo.SelectedIndex != 0)
            ObjRow.Task_Number = ddlPMCNO.SelectedValue.ToString();
        //else
        //    ObjRow.Task_Number = hdnClsDtlsID.Value;
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
        //if (!FunGetGapDays()) // Manikandan
        //{
        //    return;
        //}

        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
            return;
        }

        ObjRow.Task_Type = "13";
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
            if (ddlstatus.SelectedValue == "3")
            {
                //FunProGeneratePDF(1);
            }
            strAction = ddlstatus.SelectedValue == "3" ? "Approved" : "Rejected";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Premature closure " + strAction + " Successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PCA';", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Premature closure Approval details added failed.');", true);
            return;
        }
        else if (intErrorCode == 51)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Premature Closure request is cancelled due to Cheque returned.');", true);
            return;
        }
        else
        {
            Utility.FunShowValidationMsg(this.Page, "PMCLA", intErrorCode, strErrMsg, false);
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
        //ddlPMCNO.Items.Clear();
        ddlPMCNO.Clear();
        ddlSubAccountNo.Items.Clear();
        FunPubClear();
        ddllLineOfBusiness.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else            
            Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=PCA");
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
        dictDropdownListParam.Add("@Program_ID", "86");
        ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
        if (PageMode == PageModes.Query)
        {
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
          //  ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
        }
        uinfo = null;
        dictDropdownListParam = null;
        ddllLineOfBusiness.SelectedIndex = 1;
        ddllLineOfBusiness.RemoveDropDownList();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPMCNumber(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", obj_Page.uinfo.ProCompanyIdRW.ToString());
        Procparam.Add("@User_Id", Convert.ToString(obj_Page.intUserID));
        Procparam.Add("@LOB_ID", obj_Page.ddllLineOfBusiness.SelectedValue.ToString());
        Procparam.Add("@Task_Type", "13");
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue.ToString());
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
        if (ddlBranch.SelectedValue!="0")
        {

            FunPubClear();
            btnSave.Enabled = false;
            //ddlPMCNO.Items.Clear();
            ddlPMCNO.Clear();
            ddlSubAccountNo.Items.Clear();
            uinfo = new UserInfo();
            //dictDropdownListParam = new Dictionary<string, string>();
            //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            //dictDropdownListParam.Add("@Task_Type", "13");
            //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            //if (chkApproved.Checked)
            //{
            //    dictDropdownListParam.Add("@UnApproved", "1");
            //}
            //else
            //{
            //    dictDropdownListParam.Add("@UnApproved", "0");
            //}
            //ddlPMCNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Closure_Details_ID", "Closure_No" });
            dictDropdownListParam = null;
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;
            //ddlPMCNO.Items.Clear();
            ddlPMCNO.Clear();
        }

        ddlBranch.Focus();
    }
    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, string> dictDropdownListParam;
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        dictDropdownListParam.Add("@Is_Active", "1");
        dictDropdownListParam.Add("@Program_ID", "86");
        dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
       // ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });

        if (ddllLineOfBusiness.SelectedIndex > 0)
        {

            FunPubClear();
            ddlBranch.Clear();
            DataTable dtTable = new DataTable();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue);
            dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
            dtTable = Utility.GetDefaultData("S3G_LOAnad_PrematureGapDays", dictDropdownListParam);

            if (dtTable.Rows.Count == 0 || string.IsNullOrEmpty(dtTable.Rows[0][0].ToString()))
            {
                Utility.FunShowAlertMsg(this, "Gap days not defined in Global Parameter.");
                ddllLineOfBusiness.SelectedValue = "0";
                return;
            }

            hdnNoDates.Text = dtTable.Rows[0]["GAP_DAYS"].ToString();

            btnSave.Enabled = false;
            //ddlPMCNO.Items.Clear();
            ddlPMCNO.Clear();
            ddlSubAccountNo.Items.Clear();
            //uinfo = new UserInfo();
            //dictDropdownListParam = new Dictionary<string, string>();
            //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            //dictDropdownListParam.Add("@Task_Type", "13");
            //ddlPMCNO.BindDataTable(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam, new string[] { "Closure_No", "Closure_No" });
            //dictDropdownListParam = null;

        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;
            //ddlPMCNO.Items.Clear();
            ddlPMCNO.Clear();
        }

        ddllLineOfBusiness.Focus();
    }
    protected void ddlPMCNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlPMCNO.SelectedIndex > 0)
        if (ddlPMCNO.SelectedValue != "0")
        {
            FunPubClear();
            ReqID.Visible = true;
            
            ddlSubAccountNo.Items.Clear();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Task_Type", "13");
            dictDropdownListParam.Add("@Task_Number", ddlPMCNO.SelectedValue.ToString());
            dsapproval = Utility.GetDataset(SPNames.S3G_LOANAD_GetApprovalReqDetail, dictDropdownListParam);
            dictDropdownListParam = null;
            if (dsapproval.Tables[1].Rows.Count > 0)
            {
                txtStatus.Text = dsapproval.Tables[1].Rows[0]["status"].ToString();
                if (!string.IsNullOrEmpty(dsapproval.Tables[1].Rows[0]["Closure_Date"].ToString()))
                    txtPMCDate.Text = DateTime.Parse(dsapproval.Tables[1].Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                S3GCustomerPermAddress.SetCustomerDetails(dsapproval.Tables[1].Rows[0], true);
                txtMLA.Text = dsapproval.Tables[1].Rows[0]["PANum"].ToString();


                dictDropdownListParam = new Dictionary<string, string>();//added latest 21/12/2010
                dictDropdownListParam.Add("@COMPANY_ID", intCompanyID.ToString());
                dictDropdownListParam.Add("@USER_ID", intUserID.ToString());
                dictDropdownListParam.Add("@PANum", dsapproval.Tables[1].Rows[0]["PANum"].ToString());//, new string[] { "Closure_Details_ID", "SANum" }
                dictDropdownListParam.Add("@Closure_Type", "2");
                dictDropdownListParam.Add("@Task_Type", "13");
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
                //ddlSubAccountNo.BindDataTable("S3G_LOANAD_GetAccountClosureApprovalReqDetail", dictDropdownListParam, new string[] { "Closure_Details_ID", "SANum" });

                //DataTable dtAcClosureApproval=Utility.GetDefaultData("S3G_LOANAD_GetAccountClosureApprovalReqDetail", dictDropdownListParam);
                //DataTable DtTemp = dtAcClosureApproval;
                //string strSaNum = "";
                //if (dtAcClosureApproval.Rows.Count == 0)
                //    Session["strSubAccNo"] = "";
                //for (int i = 0; i < dtAcClosureApproval.Rows.Count;i++)
                //{
                //    strSaNum =dtAcClosureApproval.Rows[i]["SANum"].ToString();
                //    if (strSaNum.Contains("DUMMY"))
                //    {
                //        hdnClsDtlsID.Value = dtAcClosureApproval.Rows[i]["Closure_Details_ID"].ToString();
                //        Session["strSubAccNo"] = dtAcClosureApproval.Rows[i]["SANum"].ToString();
                //        //Modified by Manikandan
                //        //DtTemp.Rows.RemoveAt(i);
                //        DtTemp.Rows[i].Delete();
                //    }

                //}
                //ddlSubAccountNo.BindDataTable(DtTemp);

                //if (ddlSubAccountNo.Items.Count > 1)
                //{
                //    rvsubacco.Enabled = true;
                //    Session["strSubAccNo"] = ddlSubAccountNo.SelectedItem.Text.Trim() + "~" + ddlSubAccountNo.SelectedValue;
                //    ReqID.Visible = false;
                //    txtPMCDate.Text = "";
                //}
                //else
                //{
                //    Session["strSubAccNo"] = "" + "~" + Dset.Tables[1].Rows[0]["Closure_Details_ID"].ToString();
                //    hdnClsDtlsID.Value = Dset.Tables[1].Rows[0]["Closure_Details_ID"].ToString();
                //    ReqID.Visible = true;
                //    rvsubacco.Enabled = false;
                //}
                

                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlPMCNO.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../LoanAdmin/S3GLOANADPreMatureClosure_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

                btnGo.Enabled = true;


                //DtTemp = null;
                //dtAcClosureApproval = null;




                // ReqID.Visible = true;
                //FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlPMCNO.SelectedValue.ToString() + "\\" + txtMLA.Text + "\\" + Session["strSubAccNo"].ToString(), false, 0);
                //hdnID.Value = "../LoanAdmin/S3GLOANADPreMatureClosure_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

                //btnGo.Enabled = true;
            }
        }
        else
        {
            FunPubClear();
            ddlSubAccountNo.Items.Clear();
            btnSave.Enabled = false;
            btnGo.Enabled = true;

        }

        ddlPMCNO.Focus();
    }
    protected void ReqID_serverclick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
        return;
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //if (ddlPMCNO.SelectedIndex > 0)
        if (ddlPMCNO.SelectedValue != "0")
        {
            btnSave.Enabled = true;
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
            dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            dictDropdownListParam.Add("@Task_Number", ddlPMCNO.SelectedValue.ToString());
            if (ddlSubAccountNo.SelectedIndex > 0)
            {
                dictDropdownListParam.Add("@Task_Number2", ddlSubAccountNo.SelectedItem.Text.ToString());
            }
            dictDropdownListParam.Add("@Task_Type", "13");

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
            if (StrPrematureClosure_No != "")
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

        txtPMCDate.Text = "";
        ReqID.Visible = false;
        btnSave.Enabled = false;
        btnGo.Enabled = true;
        txtStatus.Text = string.Empty;
        txtPMCDate.Text = string.Empty;
        S3GCustomerPermAddress.ClearCustomerDetails();
        txtMLA.Text = string.Empty;
    }
    public void FunPubSubAcountClear()
    {
        grvApprovalDetails.DataSource = dsapproval.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();

        txtPMCDate.Text = "";
        ReqID.Visible = false;
        btnSave.Enabled = false;
        btnGo.Enabled = true;
        txtStatus.Text = string.Empty;
        //txtPMCDate.Text = string.Empty;
        //S3GCustomerPermAddress.ClearCustomerDetails();
        //txtMLA.Text = string.Empty;

    }

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
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlPMCNO.SelectedValue.ToString(), false, 0);
            hdnID.Value = "../LoanAdmin/S3GLOANADPreMatureClosure_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

            dictDropdownListParam = new Dictionary<string, string>();//added latest 21/12/2010
            dictDropdownListParam.Add("@COMPANY_ID", intCompanyID.ToString());
            dictDropdownListParam.Add("@USER_ID", intUserID.ToString());
            dictDropdownListParam.Add("@PANum", txtMLA.Text.Trim());//, new string[] { "Closure_Details_ID", "SANum" }
            dictDropdownListParam.Add("@Closure_Type", "2");
            dictDropdownListParam.Add("@Task_Type", "13");
            dictDropdownListParam.Add("@SANum", ddlSubAccountNo.SelectedItem.Text.Trim());
            DataTable dtAcClosureApproval = Utility.GetDefaultData("S3G_LOANAD_GetAccountClosureApprovalReqDetail", dictDropdownListParam);
            DataTable DtTemp = dtAcClosureApproval;
            if (DtTemp.Rows.Count > 0)
            {
                txtStatus.Text = DtTemp.Rows[0]["status"].ToString();
                txtPMCDate.Text = Convert.ToDateTime(DtTemp.Rows[0]["Closure_Date"].ToString()).ToString(strDateFormat);
            }

        }
        else
        {
            FunPubSubAcountClear();
            ReqID.Visible = false;
        }

        ddlSubAccountNo.Focus();
    }

    public bool FunGetGapDays()
    {
        DateTime ReqDate = new DateTime();
        DateTime GetDate = new DateTime();
        ReqDate = Utility.StringToDate(txtPMCDate.Text);
        GetDate = DateTime.Today;
        hdnTodate.Text = Convert.ToString(GetDate);
        int GapDays;
        int intGapDays;
        GapDays = Convert.ToInt32(hdnNoDates.Text);
        TimeSpan tmSpan = GetDate.Subtract(ReqDate);
        intGapDays = Convert.ToInt32(tmSpan.TotalDays);
        // hdnTodate.Text
        if (Utility.StringToDate(hdnTodate.Text) < Utility.StringToDate(txtPMCDate.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The Pre Mature Request Date is Less than the Pre Mature Approval Date ');", true);
            return false;
        }
        else if (GapDays < intGapDays)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The date difference between Premature Closure Date and Approval date should be lesser than the Global Parameter Gap Days [" + hdnNoDates.Text + "]');", true);
            return false;
        }
        return true;
    }

    protected void chkUnapproval_CheckedChanged(object sender, EventArgs e)
    {
        FunProFillUnApprovedDetails();
    }

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
            dictDropdownListParam.Add("@Program_ID", "86");
            ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            ddllLineOfBusiness.SelectedIndex = 1;
            ddllLineOfBusiness.RemoveDropDownList();
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            //ddlPMCNO.Items.Clear();
            //ddlPMCNO.Items.Insert(0, "--Select--");
            ddlPMCNO.Clear();
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
            //ddlPMCNO.Items.Clear();
            ddlPMCNO.Clear();

            FunPubClear();
            //FunPubloadCombo();
        }

        ddllLineOfBusiness.Focus();
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
            dictDropdownListParam.Add("@Program_ID", "86");
            ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            ddllLineOfBusiness.SelectedIndex = 1;
            ddllLineOfBusiness.RemoveDropDownList();
            dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            //ddlPMCNO.Items.Clear();
            //ddlPMCNO.Items.Insert(0, "--Select--");
            ddlPMCNO.Clear();
            uinfo = null;
            dictDropdownListParam = null;
        }
        else
        {
            if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
            {


                ddllLineOfBusiness.Items.Clear();
                ddlBranch.Clear();
                //ddlPMCNO.Items.Clear();
                ddlPMCNO.Clear();

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
                dictDropdownListParam.Add("@TASK_TYPE", "13");
                dictDropdownListParam.Add("@TASK_NUMBER", ddlPMCNO.SelectedValue.ToString());
                dictDropdownListParam.Add("@USER_ID", ObjUserInfo.ProUserIdRW.ToString());
                dictDropdownListParam.Add("@Password", txtPassword.Text.Trim());

                int ErrCode = ObjApproval.FunPubRevokeOrUpdateApprovedDetails(dictDropdownListParam);                

                /*added by vinodha m on march 11,2016 - parent rs cannot be revoked since its child is active */
                if (ErrCode == 7)
                {
                    Dictionary<string, string> dictparam = new Dictionary<string, string>();
                    dictparam.Add("@TASK_NUMBER", ddlPMCNO.SelectedValue.ToString());
                    dictparam.Add("@USER_ID", ObjUserInfo.ProUserIdRW.ToString());
                    DataTable DT = Utility.GetDefaultData("S3G_LOANAD_REVOK_REWRITE_RSNO", dictparam);
                    PANUM = DT.Rows[0][0].ToString();
                }
                /*added by vinodha m on march 11,2016 - parent rs cannot be revoked since its child is active */

                switch (ErrCode)
                {
                    case 0:
                        {
                            //FunProGeneratePDF(0);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('PreMature Closure Approval details Revoked successfully.');window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PCA';", true);
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
                    case 3:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Lease Asset Transfer processed, Unable to Proceed.');", true);
                            return;
                        }
                    // Added By Thangam M On 29/Dec/2011 to check for month closure
                    case 5:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert(' " + Resources.ValidationMsgs.APPR_Month_Clsr + "');", true);
                            return;
                        }
                    /*ADDED BY VINODHA M ON MAR 11,2016 - PARENT RS CANNOT BE REVOKED SINCE ITS CHILD IS ACTIVE */
                    case 7:
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert(' RS No. " + PANUM + " already activated.Unable to revoke ');", true);
                            return;
                        }
                    /*ADDED BY VINODHA M ON MAR 11,2016 - PARENT RS CANNOT BE REVOKED SINCE ITS CHILD IS ACTIVE */
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

    protected void FunProGeneratePDF(int Is_Approval)
    {
        DataTable dtAcc = new DataTable();
        DataSet dSet = new DataSet();
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Closure_Id", ddlPMCNO.SelectedValue);

        dtAcc = Utility.GetDefaultData("S3G_LOANAD_GetPMC_Accounts", dictDropdownListParam);
        if (dtAcc.Rows.Count > 0)
        {
            foreach (DataRow rw in dtAcc.Rows)
            {
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@Company_Id", intCompanyID.ToString());
                dictDropdownListParam.Add("@Billing_ID", rw["Billing_Id"].ToString());
                dictDropdownListParam.Add("@PA_SA_Ref_Id", rw["PASA_Ref_id"].ToString());
                dictDropdownListParam.Add("@Is_Approval", Is_Approval.ToString());
                dSet = Utility.GetTableValues("S3G_LOANAD_PMC_Billing_PDF", dictDropdownListParam);
                try
                {
                    string strFolderNo, strBillNo, strCustomerName, strDocumentPath,
                               strbillperiod, strnewFile, strAcocuntno, strBranchName, strtranche;
                    //ReportDocument rptd = new ReportDocument();
                    string ReportPath = "";

                    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();

                    //rptd = new ReportDocument();

                    ReportPath = (string)AppReader.GetValue("BillPDFPath", typeof(string));
                    ReportPath += "Billing.RPT";


                    DataTable dt = dSet.Tables[0];
                    DataTable dt1 = dSet.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        strBranchName = dt.Rows[0]["Location"].ToString();
                        strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                        strtranche = dt.Rows[0]["tranche_name"].ToString();
                        strBillNo = dt.Rows[0]["Bill_Number"].ToString();
                        strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                        strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
                        strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
                        strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
                        strAcocuntno = dt.Rows[0]["ACCOUNT_NO"].ToString();

                        strnewFile += "\\" + "Rental";

                        if (!Directory.Exists(strnewFile))
                        {
                            Directory.CreateDirectory(strnewFile);
                        }

                        strnewFile += "\\" + strtranche + "_" + strAcocuntno + ".pdf";
                        FileInfo fl = new FileInfo(strnewFile);
                        if (fl.Exists == true)
                        {
                            fl.Delete();
                        }

                        //rptd.Load(ReportPath);
                        //rptd.SetDataSource(dt);
                        //rptd.Subreports["Subreport"].SetDataSource(dt1);
                        //rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);
                    }
                    //if (rptd != null)
                    //{
                    //    rptd.Close();
                    //    rptd.Dispose();
                    //}

                    //rptd = new ReportDocument();
                    ReportPath = "";

                    ReportPath = (string)AppReader.GetValue("BillPDFPath", typeof(string));
                    ReportPath += "Billing_AMF.RPT";
                    dt = dSet.Tables[3];
                    dt1 = dSet.Tables[2];

                    if (dt.Rows.Count > 0)
                    {
                        strBranchName = dt.Rows[0]["Location"].ToString();
                        strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                        strBillNo = dt.Rows[0]["Bill_Number"].ToString();
                        strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                        strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
                        strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
                        strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
                        strAcocuntno = dt.Rows[0]["ACCOUNT_NO"].ToString();
                        strtranche = dt.Rows[0]["tranche_name"].ToString();

                        strnewFile += "\\" + "AMF";

                        if (!Directory.Exists(strnewFile))
                        {
                            Directory.CreateDirectory(strnewFile);
                        }

                        strnewFile += "\\" + strtranche + "_" + strAcocuntno + ".pdf";
                        FileInfo fl1 = new FileInfo(strnewFile);
                        if (fl1.Exists == true)
                        {
                            fl1.Delete();
                        }
                        //rptd.Load(ReportPath);
                        //rptd.SetDataSource(dt);
                        //rptd.Subreports["Subreport"].SetDataSource(dt1);
                        //rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);
                    }

                    //if (rptd != null)
                    //{
                    //    rptd.Close();
                    //    rptd.Dispose();
                    //}
                }
                catch (Exception ex)
                {
                    return;
                }
            }
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
        Procparam.Add("@Program_Id", "086");
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue.ToString());
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }


    #endregion
}
